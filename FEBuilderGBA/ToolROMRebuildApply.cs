using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    class ToolROMRebuildApply
    {
        //書き込んでいるデータ
        //当初は無改造ROMからスタートしデータをどんどん書き込んでいきます。
        //ただし、リサイズによる無駄を避けるために、32MBを確保します。
        byte[] WriteROMData32MB;
        //現在書き込んでいるROMサイズを指します.
        uint WriteOffset;

        //ROMリサイズ
        void write_resize_data(uint addr)
        {
            this.WriteOffset = U.Padding4(addr);
        }

        //戦闘アニメのフレームはポインタがあるのに可変長のLZ77なのでそれを何とかして解決するために利用する.
        //基本的に、無圧縮で格納して、最後にROM末尾に配置する.
        class LZ77Struct
        {
            public byte[] Bin;
            public uint OrignalAddr;
            public uint OrignalDataSize;
            public string DebugInfo;

            public LZ77Struct(byte[] bin, uint orignalAddr, uint orignalDataSize, string debugInfo)
            {
                this.Bin = bin;
                this.OrignalAddr = orignalAddr;
                this.OrignalDataSize = orignalDataSize;
                this.DebugInfo = debugInfo;
            }
        };
        List<LZ77Struct> LZ77StructList; 

        //どのアドレスをどこに再マップしたかのテーブル
        Dictionary<uint, uint> AddressMap;
        //LDRキャッシュ (@IFRリポイントの度にやっていてはきりがないので、キャッシュする.)
        List<DisassemblerTrumb.LDRPointer> LDRCache;
        void CheckLDRCache()
        {
            if (LDRCache != null)
            {
                return;
            }
            this.LDRCache = DisassemblerTrumb.MakeLDRMap(this.WriteROMData32MB, 0x100, this.WriteOffset, true);
        }
        void ClearLDRCache()
        {
            this.LDRCache = null;
        }
        bool isVanilaExtrendsROMArea(uint addr)
        {
            return addr > this.RebuildAddress;
        }

        ToolROMRebuildFreeArea FreeArea = new ToolROMRebuildFreeArea();

        uint Alloc(uint size)
        {
            //フリー領域を使えるか?
            uint writeaddr = FreeArea.CanAllocFreeArea(size);
            if (writeaddr == U.NOT_FOUND)
            {//フリーがない場合末尾から確保
                writeaddr = this.WriteOffset;
                writeaddr = U.Padding4(writeaddr);
                write_resize_data(writeaddr + size);
            }

            return writeaddr;
        }

        enum UseFreeAreaEnum
        {
             None = 0
            ,UseReBuildAddress = 1
            ,Use0x09000000 = 2
        }

        uint RebuildAddress;
        public bool Apply(InputFormRef.AutoPleaseWait wait, ROM vanilla, string filename, int useFreeArea)
        {
            this.ApplyLog = new StringBuilder();
            this.AddressMap = new Dictionary<uint, uint>();
            this.MissingPointerList = new List<MissingPointer>();
            this.LZ77StructList = new List<LZ77Struct>();
            this.WriteROMData32MB = new byte[32 * 1024 * 1024]; //32MB memory reserve
            U.write_range(this.WriteROMData32MB , 0 , vanilla.Data);
            this.WriteOffset = (uint)vanilla.Data.Length;

            string dir = Path.GetDirectoryName(filename);

            RemoveLog(filename);

            string[] lines = File.ReadAllLines(filename);
            this.RebuildAddress = GetRebuildAddress(lines);
            if (this.RebuildAddress > this.WriteOffset)
            {
                write_resize_data(this.RebuildAddress);
                this.RebuildAddress = this.WriteOffset;
            }

            bool isMakeFreeAreaList = false;

            int nextDoEvents = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string srcline = line;
                line = U.ClipComment(line);
                string[] sp = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (sp.Length < 2)
                {
                    continue;
                }
                if (sp[0] == "@_CRC32" 
                    || sp[0] == "@_REBUILDADDRESS"
                    || sp[0] == "@_ASSERT")
                {
                    continue;
                }

                uint vanilla_addr;
                uint blocksize;
                uint count;
                string targetfilename;
                uint addr = ParseLine(sp
                    , out vanilla_addr
                    , out targetfilename
                    , out blocksize
                    , out count);

                if (sp[0] == "@DEF")
                {
                    DEF(addr, srcline);
                    continue;
                }
                else if (sp[0] == "@BROKENDATA")
                {
                    BrokenData(addr, srcline);
                    continue;
                }
                else if (sp[0] == "@00")
                {
                    Fixed00(addr, blocksize, srcline);
                    continue;
                }

                if (isMakeFreeAreaList == false && addr >= this.RebuildAddress)
                {//リビルドしない領域が終わったら、フリー領域リストを再構築する.
                    isMakeFreeAreaList = true;
                    if (useFreeArea == (int)UseFreeAreaEnum.UseReBuildAddress)
                    {
                        FreeArea.MakeFreeAreaList(this.WriteROMData32MB, this.RebuildAddress, this.AddressMap);
                    }
                    else if (useFreeArea == (int)UseFreeAreaEnum.Use0x09000000)
                    {
                        FreeArea.MakeFreeAreaList(this.WriteROMData32MB, U.toOffset(Program.ROM.RomInfo.extends_address()), this.AddressMap);
                    }
                }

                if (i > nextDoEvents)
                {//毎回更新するのは無駄なのである程度の間隔で更新して以降
                    wait.DoEvents(i + ":" + line);
                    nextDoEvents = i + 0xff;
                }

                targetfilename = Path.Combine(dir, targetfilename);
                if (!File.Exists(targetfilename))
                {
                    R.ShowStopError("ファイル({1})がありません。\r\n行数:{0}\r\n", i + 1, targetfilename);
                    return false;
                }

                if (sp[0] == "@IFR")
                {
                    IFR(addr, vanilla_addr, targetfilename, blocksize, count, srcline);
                }
                else if (sp[0] == "@BIN")
                {
                    Bin(addr, targetfilename, srcline);
                }
                else if (sp[0] == "@MIX")
                {
                    Mix(addr, targetfilename, srcline);
                }
                else if (sp[0] == "@MIXLZ77")
                {
                    MixLZ77(addr, targetfilename, blocksize, srcline);
                }
                else
                {
                    continue;
                }
            }
            wait.DoEvents("WriteBackMixLZ77");
            //後回しにしていたLZ77の解決.
            WriteBackMixLZ77();

            wait.DoEvents("CheckSelf");
            bool r = CheckSelf(filename, lines);
            if (!r)
            {
                R.ShowStopError("適応した結果にエラーがあるようです。\r\nログを確認してください。\r\nログ:{0}", GetLogFilename(filename));
                return false;
            }

            wait.DoEvents("ApplyVanillaROM");
            ApplyVanillaROM(vanilla);
            return true;
        }
        void ApplyVanillaROM(ROM vanilla)
        {
            vanilla.write_resize_data(this.WriteOffset);

            vanilla.write_range(0, U.getBinaryData(this.WriteROMData32MB , 0 , (uint)vanilla.Data.Length));
        }

        void RemoveLog(string filename)
        {
            string logfilename = GetLogFilename(filename);
            if (File.Exists(logfilename))
            {
                File.Delete(logfilename);
            }
        }

        public static string GetLogFilename(string basefilename)
        {
            string logfilename = basefilename + ".log.txt";
            return logfilename;
        }

        bool CheckSelf(string filename , string[] lines)
        {
            string logfilename = GetLogFilename(filename);

            StringBuilder sb = new StringBuilder();

            //これでも不明なポインタがあれば表示
            ShowMissingPointers(sb);
            //ASSERTによる自己診断.
            //CheckAssert(sb, lines);


            bool isOK = true;
            if (sb.Length > 0)
            {//エラーがあれば出力する.
                sb.Insert(0,"== ERROR! ==\r\n");
                isOK = false;
            }

            sb.AppendLine("== MAPPING ==");
            sb.Append(this.ApplyLog);

            File.WriteAllText(logfilename, sb.ToString());
            return isOK;
        }

        void ShowMissingPointers(StringBuilder sb)
        {
            for (int i = 0; i < this.MissingPointerList.Count; i++)
            {
                MissingPointer m = this.MissingPointerList[i];
                sb.Append("Missing!");
                sb.Append(" P:");
                sb.Append(U.To0xHexString(m.FindPointer));
                sb.Append(" Pos:");
                sb.Append(U.To0xHexString(m.WroteAddr));
                sb.Append(" ");
                sb.Append(m.Type);
                sb.Append(m.IsLZ77 ? " (LZ77)" : "");
                sb.Append(" //");
                sb.Append(m.DebugTargetfilename);
                sb.AppendLine();
            }
        }
        void CheckAssert(StringBuilder sb, string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line = U.ClipComment(line);
                string[] sp = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (sp.Length < 3)
                {
                    continue;
                }
                if (sp[0] != "@_ASSERT")
                {
                    continue;
                }

                uint addr = U.atoh(sp[1]);
                uint needData = U.atoh(sp[2]);

                uint pointer = Program.ROM.p32(addr);
                if (!U.isSafetyOffset(pointer))
                {
                    sb.Append(U.ToHexString(pointer));
                    sb.Append(" <--");
                    sb.Append("BAD POINTER");
                    sb.Append(line);
                    sb.AppendLine();
                    continue;
                }
                uint currentData = Program.ROM.u32(pointer);
                if (needData != currentData)
                {
                    sb.Append(U.ToHexString(currentData));
                    sb.Append(" <-- ");
                    sb.Append(line);
                    sb.AppendLine();
                    continue;
                }
            }
        }

        //LZ77のID部の位置.
        const int LZ77UNIQ_SHIFT = 20;

        //ポインタが解決された
        void ResolvedPointer(uint labelPointer, uint addrPointer,string debugInfo)
        {
            Debug.Assert(labelPointer >= 0x08000000);
            Debug.Assert(addrPointer >= 0x08000000);

            this.AddressMap[labelPointer] = addrPointer;
            WriteApplyLog(labelPointer, addrPointer, debugInfo);

            //不明なポインタが解決された場合、書き戻す.
            for (int i = 0; i < this.MissingPointerList.Count; i++)
            {
                MissingPointer m = this.MissingPointerList[i];

                if (m.FindPointer == labelPointer)
                {
                    uint p = addrPointer;
                    if (m.Type == PointerType.ANTI_HUFFMAN)
                    {
                        p += 0x80000000;
                    }

                    if (m.IsLZ77)
                    {
                        uint lz77uniq = (uint)(m.WroteAddr >> LZ77UNIQ_SHIFT);
                        uint pos = (uint)(m.WroteAddr & ((1 << LZ77UNIQ_SHIFT) - 1) );
                        LZ77Struct lz77struct = this.LZ77StructList[(int)lz77uniq];
                        U.write_u32(lz77struct.Bin, pos, p);
                    }
                    else
                    {
                        U.write_u32(this.WriteROMData32MB, m.WroteAddr, p);
                    }
                    //解決されたのでリストから消す.
                    this.MissingPointerList.RemoveAt(i);
                    i --;
                }
                else if (m.Type == PointerType.ASM)
                {
                    uint l;
                    uint p;
                    if (U.IsValueOdd(m.FindPointer))
                    {
                        if (U.IsValueOdd(labelPointer))
                        {
                            continue;
                        }
                        l = labelPointer + 1;

                        if (U.IsValueOdd(labelPointer))
                        {
                            p = addrPointer;
                        }
                        else
                        {
                            p = addrPointer + 1;
                        }
                    }
                    else
                    {
                        if (!U.IsValueOdd(labelPointer))
                        {
                            continue;
                        }
                        l = labelPointer - 1;

                        if (U.IsValueOdd(labelPointer))
                        {
                            p = addrPointer - 1;
                        }
                        else
                        {
                            p = addrPointer;
                        }
                    }

                    if (m.FindPointer != l)
                    {
                        continue;
                    }

                    if (m.IsLZ77)
                    {
                        uint lz77uniq = (uint)(m.WroteAddr >> LZ77UNIQ_SHIFT);
                        uint pos = (uint)(m.WroteAddr & ((1 << LZ77UNIQ_SHIFT) - 1));
                        LZ77Struct lz77struct = this.LZ77StructList[(int)lz77uniq];
                        U.write_u32(lz77struct.Bin, pos, p);
                    }
                    else
                    {
                        U.write_u32(this.WriteROMData32MB, m.WroteAddr, p);
                    }
                    //解決されたのでリストから消す.
                    this.MissingPointerList.RemoveAt(i);
                    i --;
                }
            }

        }

        void DEF(uint addr,string debugInfo)
        {
            uint labelPointer = U.toPointer(addr);
            ResolvedPointer(labelPointer, labelPointer, debugInfo);
        }

        void BrokenData(uint addr, string debugInfo)
        {
            if (!isVanilaExtrendsROMArea(addr))
            {//非拡張データなので無視する.
                uint labelPointer = U.toPointer(addr);
                ResolvedPointer(labelPointer, labelPointer, debugInfo);
                return ;
            }

            //壊れている魔法アニメなどの画像データ
            //とりあえず4バイト null を入れる.
            Bin(addr, new byte[] { 0, 0, 0, 0 }, debugInfo);
        }

        void Fixed00(uint addr, uint length, string debugInfo)
        {
            U.write_fill(this.WriteROMData32MB, addr, length, 0);
        }

        StringBuilder ApplyLog;
        void WriteApplyLog(uint labelPointer, uint addrPointer, string debugInfo)
        {
            if (labelPointer != addrPointer)
            {
                ApplyLog.Append(U.ToHexString8(addrPointer));
                ApplyLog.Append(" <=re= ");
            }
            ApplyLog.Append(U.ToHexString8(labelPointer));
            ApplyLog.Append(' ');
            ApplyLog.Append(debugInfo);
            ApplyLog.AppendLine();
        }

        void Bin(uint addr, string targetfilename, string debugInfo)
        {
            byte[] bin = File.ReadAllBytes(targetfilename);
            Bin(addr, bin, debugInfo);
        }
        void Bin(uint addr, byte[] bin, string debugInfo)
        {
            uint writeaddr;
            if (!isVanilaExtrendsROMArea(addr + (uint)bin.Length))
            {//非拡張領域
                writeaddr = addr;
            }
            else
            {
                if (bin.Length >= 8)
                {
                    //実はこのデータが既にROMにあったりしますか?
                    uint foundAddr = U.Grep(this.WriteROMData32MB, bin, 0x100, this.WriteOffset, 4);
                    if (foundAddr != U.NOT_FOUND)
                    {//既にROMにあるので共有させましょう
                        writeaddr = foundAddr;
                        ResolvedPointer(U.toPointer(addr), U.toPointer(writeaddr), debugInfo + "//SHARE!");
                        return;
                    }
                }
                //リポイントが必須
                writeaddr = Alloc((uint)bin.Length);
            }

            U.write_range(this.WriteROMData32MB, writeaddr, bin);
            ResolvedPointer(U.toPointer(addr), U.toPointer(writeaddr), debugInfo);
        }


        enum PointerType
        {
            NONE
            ,ASM          //+0x1
            ,ANTI_HUFFMAN //+0x80 00 00 00
        }

        //まだ発見していない不明なポインタ
        class MissingPointer
        {
            public uint FindPointer; //探しているポインタ
            public uint WroteAddr; //このデータがある位置
            public PointerType Type;
            public bool IsLZ77;     //LZ77の仮想アドレス
            public string DebugTargetfilename;

            public MissingPointer(uint findPointer
                , uint wroteAddr
                , PointerType type
                , bool isLZ77
                , string debugTargetfilename)
            {
                this.FindPointer = findPointer;
                this.WroteAddr = wroteAddr;
                this.Type = type;
                this.IsLZ77 = isLZ77;
                this.DebugTargetfilename = debugTargetfilename;
            }
        }
        List<MissingPointer> MissingPointerList;

        List<byte> WriteBytes(int startIndex //spの中で書き込みを開始するデータ位置 1 or 0
            , string[] sp                    //ファイルから読みこんでパースしたデータ
            , uint writeTopBaseAddr          //トップデータの位置 (lz77の場合 0)
            , uint currentBaseAddr           //書き込む予定の位置 (lz77の場合 ユニークな値)
            , string debugInfo               //デバッグ用
        )
        {
            bool isLZ77 = (writeTopBaseAddr == 0);

            List<byte> bin = new List<byte>();
            for (int i = startIndex; i < sp.Length; i++)
            {
                string s = sp[i];
                if (s.Length <= 0)
                {
                }
                else if (s[0] == '@')
                {//ポインタ
                    uint p = U.toPointer(U.atoh(s.Substring(1)));

                    uint new_pointer;
                    if (this.AddressMap.TryGetValue(p, out new_pointer))
                    {
                        U.append_u32(bin, new_pointer);
                    }
                    else
                    {//アドレスのデータがないよ!
                        //不明なポインタとしてリストに登録. 後で判明したら書き戻す
                        this.MissingPointerList.Add(new MissingPointer(p
                            , currentBaseAddr + (uint)bin.Count
                            , PointerType.NONE, isLZ77, debugInfo));
                        //とりあえずそのポインタの値で書き込む.
                        U.append_u32(bin, p);
                    }
                }
                else if (s[0] == '`')
                {//anti-huffmanのポインタ
                    uint p = U.toPointer(U.atoh(s.Substring(1)));
                    uint new_pointer;
                    if (this.AddressMap.TryGetValue(p, out new_pointer))
                    {
                        U.append_u32(bin, new_pointer + 0x80000000);
                    }
                    else
                    {//アドレスのデータがないよ!
                        //不明なポインタとしてリストに登録. 後で判明したら書き戻す
                        this.MissingPointerList.Add(new MissingPointer(p
                            , currentBaseAddr + (uint)bin.Count
                            , PointerType.ANTI_HUFFMAN, isLZ77, debugInfo));
                        U.append_u32(bin, p + 0x80000000);
                    }
                }
                else if (s[0] == '&')
                {//ASM
                    Debug.Assert(isLZ77 == false);

                    uint p = U.toPointer(U.atoh(s.Substring(1)));

                    uint new_pointer;
                    if (this.AddressMap.TryGetValue(p, out new_pointer))
                    {
                        U.append_u32(bin, new_pointer);
                    }
                    else if (
                        U.IsValueOdd(p)
                        && this.AddressMap.TryGetValue(p - 1, out new_pointer))
                    {
                        U.append_u32(bin, new_pointer + 1);
                    }
                    else
                    {//アドレスのデータがないよ!
                        //不明なポインタとしてリストに登録. 後で判明したら書き戻す
                        this.MissingPointerList.Add(new MissingPointer(p
                            , currentBaseAddr + (uint)bin.Count
                            , PointerType.ASM, isLZ77, debugInfo));
                        U.append_u32(bin, p);
                    }
                }
                else if (s[0] == '+')
                {//自己参照
                    uint plus = U.atoh(s.Substring(1));
                    uint new_pointer = U.toPointer(writeTopBaseAddr + plus);
                    U.append_u32(bin, new_pointer);
                }
                else
                {
                    U.append_u8(bin, U.atoh(s));
                }
            }
            return bin;
        }
        uint CalcWriteDataSize(string[] sp)
        {
            uint datasize = 0;
            for (int i = 0; i < sp.Length; i++)
            {
                if (sp[i][0] == '@' //ポインタ
                 || sp[i][0] == '&' //ASMコード ポインタ+1
                 || sp[i][0] == '+' //自己参照
                    )
                {
                    datasize += 4;
                }
                else
                {
                    datasize++;
                }
            }
            return datasize;
        }

        void Mix(uint addr, string targetfilename, string debugInfo)
        {
            string lines = File.ReadAllText(targetfilename);
            string[] sp = lines.Split(new char[]{' '} , StringSplitOptions.RemoveEmptyEntries );

            //まずサイズを求めないといけない.
            uint datasize = CalcWriteDataSize(sp);

            //領域の確保
            uint writeaddr = 0;
            if (!isVanilaExtrendsROMArea(addr + datasize))
            {//非拡張領域
                //そのまま書き換えられる
                writeaddr = addr;
            }
            else
            {//リポイントが必要
                writeaddr = Alloc(datasize);
            }
            List<byte> bin = WriteBytes(0, sp, writeaddr, writeaddr, debugInfo);

            //データの書き込み
            U.write_range(this.WriteROMData32MB, writeaddr, bin.ToArray());
            ResolvedPointer(U.toPointer(addr), U.toPointer(writeaddr), debugInfo);
        }

        void MixLZ77(uint addr, string targetfilename, uint datasize, string debugInfo)
        {
            string lines = File.ReadAllText(targetfilename);
            string[] sp = lines.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int oldMissingSize = this.MissingPointerList.Count;
            uint lz77uniq = (uint)(this.LZ77StructList.Count << LZ77UNIQ_SHIFT);
            List<byte> bin = WriteBytes(0, sp, 0, lz77uniq, debugInfo);

            int newMissingSize = this.MissingPointerList.Count;
            if (oldMissingSize == newMissingSize)
            {//不明なポインタは存在しない　よって、書き込めるはず
                WriteLZ77Bin(bin.ToArray(), addr, datasize, debugInfo);
            }
            else
            {//不明なポインタがあるので、末尾に回す
                this.LZ77StructList.Add(new LZ77Struct(bin.ToArray(), addr, datasize, debugInfo));
            }

        }
        void WriteLZ77Bin(byte[] bin, uint addr, uint datasize, string debugInfo)
        {
            byte[] newbin = LZ77.compress(bin);

            //領域の確保
            uint writeaddr = 0;
            if (!isVanilaExtrendsROMArea(addr + datasize)
                && datasize >= newbin.Length)
            {//非拡張領域
                //そのまま書き換えられる
                writeaddr = addr;
            }
            else
            {//リポイントが必要
                writeaddr = Alloc((uint)newbin.Length);
            }

            //データの書き込み
            U.write_range(this.WriteROMData32MB, writeaddr, newbin);
            ResolvedPointer(U.toPointer(addr), U.toPointer(writeaddr), debugInfo);
        }
        void WriteBackMixLZ77()
        {
            for(int i = 0 ; i < this.LZ77StructList.Count ; i++ )
            {
                LZ77Struct lz77Struct = this.LZ77StructList[i];
                WriteLZ77Bin(lz77Struct.Bin, lz77Struct.OrignalAddr, lz77Struct.OrignalDataSize, lz77Struct.DebugInfo);
            }
        }

        void IFR(
            uint addr, uint vanilla_addr, string targetfilename, uint blocksize, uint count
            , string debugInfo)
        {
            uint writeaddr;
            if (!isVanilaExtrendsROMArea(addr + blocksize * count))
            {//非拡張領域
                //そのまま書き換えられる
                writeaddr = addr;
            }
            else
            {//リポイントが必要

                writeaddr = Alloc(blocksize * count);
                uint writeaddr_pointer = U.toPointer(writeaddr);

                //データのコピー
                byte[] bin = U.getBinaryData(this.WriteROMData32MB, vanilla_addr, blocksize * count);
                U.write_range(this.WriteROMData32MB, writeaddr, bin);

                uint vanilla_pointer = U.toPointer(vanilla_addr);

                //IFRの場合 複数のLDRを書き換える必要あり
                //ClearLDRCache();
                CheckLDRCache();
                for (int i = 0; i < this.LDRCache.Count; i++)
                {
                    DisassemblerTrumb.LDRPointer ldr = this.LDRCache[i];
                    if (ldr.ldr_data == vanilla_pointer)
                    {
                        U.write_u32(this.WriteROMData32MB, ldr.ldr_data_address, writeaddr_pointer);
                        ldr.ldr_data = writeaddr_pointer;
                    }
                }
            }

            string[] lines = File.ReadAllLines(targetfilename);
            for (int n = 0; n < lines.Length; n++)
            {
                string line = lines[n];
                string[] sp = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                uint dataIndex = U.atoh(sp[0].Substring(1));

                uint pos = writeaddr + (blocksize * dataIndex);
                List<byte> bin = WriteBytes(1, sp, writeaddr, pos, debugInfo);

                U.write_range(this.WriteROMData32MB, pos, bin.ToArray());
            }
            ResolvedPointer(U.toPointer(addr), U.toPointer(writeaddr), debugInfo);
        }

        uint ParseLine(string[] sp
           , out uint out_vanilla_addr
           , out string out_filename
           , out uint out_blocksize
           , out uint out_count)
        {
            out_vanilla_addr = 0;
            out_filename = "";
            out_blocksize = 0;
            out_count = 0;

            for (int i = 2; i < sp.Length; i++)
            {
                if (sp[i][0] == '=')
                {//無改造ROMのアドレス
                    out_vanilla_addr = U.atoh(sp[i].Substring(1));
                    out_vanilla_addr = U.toOffset(out_vanilla_addr);
                }
                else if (sp[i][0] == '*')
                {//個数
                    out_count = U.atoh(sp[i].Substring(1));
                }
                else if (sp[i][0] == ':')
                {//ブロックサイズ
                    out_blocksize = U.atoh(sp[i].Substring(1));
                }
                else
                {//不明なので多分ファイルだと思う.
                    out_filename = string.Join(" ", sp, i, sp.Length - i);
                    break;
                }
            }

            uint addr = U.atoh(sp[1]);
            return U.toOffset(addr);
        }

        public static uint GetCRC32(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string[] sp = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (sp[0] == "@_CRC32" && sp.Length >= 2)
                {
                    return U.atoh(sp[1]);
                }
            }
            return U.NOT_FOUND;
        }
        public static uint GetRebuildAddress(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string[] sp = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (sp[0] == "@_REBUILDADDRESS" && sp.Length >= 2)
                {
                    return U.atoh(sp[1]);
                }
            }
            return U.NOT_FOUND;
        }
    }
}
