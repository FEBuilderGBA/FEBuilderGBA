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
    public class ToolROMRebuildMake
    {
        ROM Vanilla;
        bool romcmp(uint addr, uint vanilla_addr, uint blockSize)
        {
            if (isRebuildAddress(addr))
            {
                return false;
            }
            byte[] s = Program.ROM.getBinaryData(addr, blockSize);
            byte[] v = this.Vanilla.getBinaryData(vanilla_addr, blockSize);
            return (U.memcmp(s, v) == 0);
        }
        bool romcmpPointer(uint addr, uint vanilla_addr, uint blockSize)
        {
            if (isRebuildAddress(addr))
            {
                return false;
            }
            if (!U.isSafetyOffset(addr) || !U.isSafetyOffset(vanilla_addr, this.Vanilla))
            {
                return false;
            }

            uint sp = Program.ROM.p32(addr);
            uint vp = this.Vanilla.p32(vanilla_addr);

            if (isRebuildAddress(sp))
            {
                return false;
            }
            if (!U.isSafetyOffset(sp) || !U.isSafetyOffset(vp, this.Vanilla))
            {
                return false;
            }

            byte[] s = Program.ROM.getBinaryData(sp, blockSize);
            byte[] v = this.Vanilla.getBinaryData(vp, blockSize);
            return (U.memcmp(s, v) == 0);
        }
        public bool romcmplz77Pointer(uint addr, uint vanilla_addr)
        {
            if (isRebuildAddress(addr))
            {
                return false;
            }

            uint sp = Program.ROM.p32(addr);
            uint vp = this.Vanilla.p32(vanilla_addr);

            if (isRebuildAddress(sp))
            {
                return false;
            }
            if (!U.isSafetyOffset(sp) || !U.isSafetyOffset(vp, this.Vanilla))
            {
                return false;
            }

            byte[] s = LZ77.decompress(Program.ROM.Data, sp);
            byte[] v = LZ77.decompress(this.Vanilla.Data, vp);
            return (U.memcmp(s, v) == 0);
        }

        class RefCmd
        {
            //コマンドのデータ
            public string Cmd = "";
            //このコマンドが定義するポインタ値
            public Address UseAddress;
        };

        //重複するアドレスがあった場合、より説明できる方を採用する.
        void OptimizeList(InputFormRef.AutoPleaseWait wait,List<Address> list)
        {
            int orignalCount = (int)list.Count;

            wait.DoEvents(R._("データを最適化中... {0}({1})", orignalCount,0));
            Dictionary<Address, uint> scoreCache = new Dictionary<Address, uint>();
            for (int i = 0; i < list.Count; i++ )
            {
                Address a = list[i];
                uint a_point = 0;
                if (a.DataType == Address.DataTypeEnum.UnkMIX)
                {//不明なデータ LDR参照で見つけられたデータ
                    scoreCache[a] = 0;
                    continue;
                }

                if (a.DataType == Address.DataTypeEnum.BATTLEFRAME)
                {//BATTLEFRAME
                    a_point += 0x30;
                }
                else if (a.DataType == Address.DataTypeEnum.NEW_TARGET_SELECTION_STRUCT)
                {//NEW_TARGET_SELECTION_STRUCT
                    a_point += 0x20;
                }
                else if (Address.IsLZ77(a.DataType) && a.Length > 0)
                {//LZ77
                    a_point += 0x10;
                }
                else if (Address.IsASMOnly(a.DataType))
                {//ASM
                    a_point += 0x15;
                }
                else if (Address.IsPointerableType(a.DataType))
                {//ポインタ
                    a_point += 0x10;
                }
                else if (Address.IsIFR(a.DataType))
                {//IFR
                    a_point += 0x10;
                }
                else if (Address.IsBINType(a.DataType))
                {//BIN
                    a_point += 0x15;
                }
                
                a_point += a.Length;
                scoreCache[a] = a_point;
            }

            for (int i = 0; i < list.Count; )
            {
                Address a = list[i];
                uint a_point = scoreCache[a];

                bool isDeleteIT = false;
                for (int n = i+1; n < list.Count;  )
                {
                    Address b = list[n];
                    if (a.Addr != b.Addr)
                    {
                        n++;
                        continue;
                    }

                    uint b_point = scoreCache[b];

                    if (a_point < b_point)
                    {
                        isDeleteIT = true;
                        break;
                    }
                    else
                    {
                        list.RemoveAt(n);
                        wait.DoEvents(R._("データを最適化中... {0}({1})", orignalCount, list.Count - orignalCount));
                        continue;
                    }
                }

                if (isDeleteIT)
                {
                    list.RemoveAt(i);
                    wait.DoEvents(R._("データを最適化中... {0}({1})", orignalCount, list.Count - orignalCount));
                    continue;
                }

                i++;
            }

        }

        Dictionary<uint, bool> PointerMark;

    
        //パッチなどにないハックを検知します.
        void FindUnknownHack2(List<RefCmd> refCmdList, bool[] processedAddress)
        {
            for (int i = 0; i < refCmdList.Count; i++)
            {
                RefCmd rc = refCmdList[i];
                Address a = rc.UseAddress;

                uint addr = a.Addr;
                uint end = addr + a.Length;
                end = Math.Min(end, (uint)processedAddress.Length);

                if (rc.Cmd.IndexOf("@DEF") == 0)
                {
                    for (; addr < end; addr++)
                    {
                        processedAddress[addr] = false;
                    }
                    continue;
                }

                for (; addr < end; addr++)
                {
                    processedAddress[addr] = true;
                }
            }


            uint vallinaLength = (uint)this.Vanilla.Data.Length;
            uint length = Math.Min(vallinaLength, this.RebuildAddress);
            uint start = U.NOT_FOUND;
            for (uint i = 0x0; i < length; i++)
            {
                if (processedAddress[i])
                {
                    if (start != U.NOT_FOUND)
                    {
                        RefCmd refCmd = UnkBin(start, i - start);
                        refCmdList.Add(refCmd);
                        start = U.NOT_FOUND;
                    }

                    continue;
                }

                uint c = Program.ROM.u8(i);
                uint v = this.Vanilla.u8(i);
                if (c != v)
                {
                    if (start == U.NOT_FOUND)
                    {
                        start = i;
                    }
                }
                else
                {
                    if (start != U.NOT_FOUND)
                    {
                        RefCmd refCmd = UnkBin(start, i - start);
                        refCmdList.Add(refCmd);
                        start = U.NOT_FOUND;
                    }
                }
            }

            if (this.RebuildAddress > vallinaLength)
            {//無改造ROMより大きい領域で、コピーできる領域.
                length = this.RebuildAddress - vallinaLength;
                RefCmd refCmd = UnkBin(vallinaLength, length);
                refCmdList.Add(refCmd);
            }
        }
        bool IsNULLData(byte[] bin)
        {
            for (int i = 0; i < bin.Length; i++)
            {
                if (bin[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        RefCmd UnkBin(uint addr, uint length)
        {
            Address address = new Address(addr, length, U.NOT_FOUND, "UnkHack", Address.DataTypeEnum.BIN);

            byte[] bin = Program.ROM.getBinaryData(address.Addr, length);

            StringBuilder sb = new StringBuilder();
            if (IsNULLData(bin))
            {
                sb.Append("@00 ");
                sb.Append(U.ToHexString8(address.Addr));
                sb.Append(" :");
                sb.Append(U.ToHexString(address.Length)); //blocksize
            }
            else
            {
                string basename = MakeName(address);
                string filename = Path.Combine("rebuild_bin", basename + ".bin");

                sb.Append("@BIN ");
                sb.Append(U.ToHexString8(address.Addr));
                sb.Append(" ");
                sb.Append(filename);

                string fullfilename = Path.Combine(this.BaseDir, filename);
                U.WriteAllBytes(fullfilename, bin);
            }

            RefCmd refCmd = new RefCmd();
            refCmd.Cmd = sb.ToString();
            refCmd.UseAddress = address;

            return refCmd;
        }

        List<Address> StructList;
        string BaseDir;
        bool Mkdir(string name)
        {
            string fullname = Path.Combine(this.BaseDir, name);
            U.mkdir(fullname);

            if (! Directory.Exists(fullname))
            {
                R.ShowStopError("ディレクトリを作成できませんでした。\r\nディレクトリ:{0}", fullname);
                return false;
            }
            return true;
        }
        void AppendPointer()
        {
            int length = this.StructList.Count;
            for (int i = 0; i < length; i++ )
            {
                Address a = this.StructList[i];
                if (!U.isSafetyOffset(a.Pointer))
                {
                    continue;
                }
                if (isRebuildAddress(a.Pointer))
                {//リビルド領域に設置するポインタは無意味である。なぜなら追加だからだ.
                    continue;
                }
                Address.AddAddress(this.StructList
                    , a.Pointer
                    , 0
                    , U.NOT_FOUND
                    , "POINTER:" + a.Info
                    , Address.DataTypeEnum.POINTER);
            }
        }
        //LDR DATA が正しいかどうか検証する
        bool CheckTrueLDRData(uint addr)
        {
            if (this.PointerMark.ContainsKey(addr))
            {//既に知っている.
                return false;
            }

            for (int i = 0; i < this.StructList.Count; i++)
            {
                Address a = this.StructList[i];
                if (   addr > a.Addr
                    && addr < a.Addr + a.Length)
                {
                    if (a.DataType == Address.DataTypeEnum.ASM
                        || a.DataType == Address.DataTypeEnum.PATCH_ASM)
                    {
                        break;
                    }
                    return false;
                }
            }

            return true;
        }
        void AppendLDR(List<DisassemblerTrumb.LDRPointer> ldrmap)
        {
            for (int i = 0; i < ldrmap.Count; i++)
            {
                DisassemblerTrumb.LDRPointer ldr = ldrmap[i];

                uint addr = U.toOffset(ldr.ldr_data);

                AppendUnkLDRPointer(addr , ldr.ldr_data_address);
                AppendUnkLDRData(addr , ldr.ldr_data_address);
            }
        }

        void AppendUnkLDRData(uint addr, uint pointer)
        {
            Debug.Assert(Program.ROM.p32(pointer) == addr);

            uint addrP = U.toPointer(addr);
            uint pointerP = U.toPointer(pointer);

            //LDRが指すデータの先
            if (U.IsValueOdd(addr))
            {
                if (!PointerMark.ContainsKey(addrP) 
                    && !PointerMark.ContainsKey(addrP - 1))
                {
                    Address.AddAddress(this.StructList
                        , addr - 1, 0, pointer
                        , "LDR DATA ASM" + Program.AsmMapFileAsmCache.GetName(addr)
                        , Address.DataTypeEnum.ASM);
                    PointerMark[addrP - 1] = true;
                }
            }
            else
            {
                if (!PointerMark.ContainsKey(addrP))
                {
                    if (IsLZ77Data(addr))
                    {
                        Address.AddAddress(this.StructList
                            , addr, 0, pointer
                            , "LDR DATA LZ77" + Program.AsmMapFileAsmCache.GetName(addr)
                            , Address.DataTypeEnum.LZ77IMG);
                    }
                    else if (DisassemblerTrumb.IsCode(addr))
                    {
                        Address.AddAddress(this.StructList
                            , addr, 0, pointer
                            , "LDR DATA ASM_" + Program.AsmMapFileAsmCache.GetName(addr)
                            , Address.DataTypeEnum.ASM);
                    }
                    else if (IsPointerData(addr))
                    {
                        Address.AddAddress(this.StructList
                            , addr, 4, pointer
                            , "LDR REF DATA " + Program.AsmMapFileAsmCache.GetName(addr)
                            , Address.DataTypeEnum.POINTER);
                    }
                    else
                    {
                        Address.AddAddress(this.StructList
                            , addr, 0, pointer
                            , "LDR DATA " + Program.AsmMapFileAsmCache.GetName(addr)
                            , Address.DataTypeEnum.UnkMIX);
                    }
                    PointerMark[addrP] = true;
                }
            }
        }

        bool IsPointerData(uint addr)
        {
            uint dataP = Program.ROM.u32(addr);
            if (!U.isPointer(dataP))
            {
                if (U.is_RAMPointer(dataP))
                {//RAMポインタだと思われる.
                    return true;
                }
                return false;
            }
            if (PointerMark.ContainsKey(dataP))
            {//既知のポインタ
                return true;
            }

            uint data = U.toOffset(dataP);
            Address a = GetBLFunctionAddress(data);
            if (a != null)
            {//既知の領域へのポインタだとおもわれる.
                return true;
            }

            return false;
        }

        void AppendUnkLDRPointer(uint addr, uint pointer)
        {
            Debug.Assert(Program.ROM.p32(pointer) == addr);

//            uint addrP = U.toPointer(addr);
            uint pointerP = U.toPointer(pointer);

            //LDRが指すデータの先
            if (U.IsValueOdd(addr))
            {
                if (!PointerMark.ContainsKey(pointerP))
                {
                    Address.AddAddress(this.StructList
                        , pointer, 0, U.NOT_FOUND
                        , "LDR REF ADDRESS"
                        , Address.DataTypeEnum.POINTER_ASM);
                    PointerMark[pointerP] = true;
                }
            }
            else
            {
                if (!PointerMark.ContainsKey(pointerP))
                {
                    Address.AddAddress(this.StructList
                        , pointer, 0, U.NOT_FOUND
                        , "LDR REF ADDRESS"
                        , Address.DataTypeEnum.POINTER);
                    PointerMark[pointerP] = true;
                }
            }
        }

        bool IsLZ77Data(uint addr)
        {
            //ポインタ先は圧縮されているか？
            uint imageDataSize = LZ77.getUncompressSize(Program.ROM.Data, addr);
            if (GraphicsToolForm.IsBadImageSize(imageDataSize))
            {
                return false;
            }

            //解凍して中身を見てみる.
            byte[] image = LZ77.decompress(Program.ROM.Data, addr);
            if (image.Length != imageDataSize )
            {//壊れたデータ
                return false;
            }
            return true;
        }
        void ResolvUnkLength()
        {
            for (int i = 0; i < this.StructList.Count; i++)
            {
                ResolvUnkLength(this.StructList[i]);
            }
        }

        void ResolvUnkLength(Address address)
        {
            //ASMの場合LDR参照があるのでサイズを求めなおす
            if (address.DataType == Address.DataTypeEnum.ASM
                || address.DataType == Address.DataTypeEnum.PATCH_ASM)
            {
                List<uint> ldrtable = new List<uint>();
                uint calclength = DisassemblerTrumb.CalcLength(Program.ROM.Data
                    , address.Addr, (uint)Program.ROM.Data.Length, ldrtable);
                if (address.Length >= calclength)
                {
                }
                else
                {
                    //既存サイズの方が大きい場合は、大きい方を取る
                    address.Length = Math.Max(calclength, address.Length);
                    //発見したLDRを追加
                    ReScanLDR(address);
                }

//克服できたかも
//                if (isRebuildAddress(address.Addr))
//                {//拡張領域のASMは何か変な書き方をされている mug_exceedとか、変なのが多いので、余分に領域をスキャンすることにする.
//                    address.Length += 32; //保険のため2LINE追加.
//                }
            }
            if (address.Length <= 0)
            {//サイズが0の場合、推測します.
                if (address.DataType == Address.DataTypeEnum.POINTER)
                {
                    address.Length = 4;
                }
                else if (address.DataType == Address.DataTypeEnum.POINTER_ASM)
                {
                    address.Length = 4;
                    ReScanOneLDR(address.Addr);
                }
                else if (address.DataType == Address.DataTypeEnum.MAGICFRAME_CSA
                    || address.DataType == Address.DataTypeEnum.MAGICFRAME_FEITORADV)
                {//間違ったフレームデータ
                    return ;
                }
                else if (Address.IsLZ77(address.DataType))
                {//壊れたlz77データ
                    return ;
                }
                else if (address.DataType == Address.DataTypeEnum.SONGINSTDIRECTSOUND)
                {//サイズがわからない 楽器データ.
                    address.Length = CheckUnkLength(address, UNKNOWN_DATA_DEFAULT_SIZE, UNKNOWN_DATA_DEFAULT_SIZE);
                }
                else
                {
                    address.Length = CheckUnkLength(address, UNKNOWN_DATA_DEFAULT_SIZE, UNKNOWN_DATA_DEFAULT_SIZE);
                }
            }
        }

        bool isRebuildAddress(uint addr)
        {
            return addr >= this.RebuildAddress;
        }

        uint RebuildAddress;
        public void Make(InputFormRef.AutoPleaseWait wait
            , string orignalFilename
            , string filename
            , uint rebuildAddress)
        {
            this.RebuildAddress = rebuildAddress;
            this.Vanilla = new ROM();
            string version;
            bool r = this.Vanilla.Load(orignalFilename, out version);
            if (!r)
            {
                R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                return;
            }

            this.BaseDir = Path.GetDirectoryName(filename);
            r = Mkdir("rebuild_ifr");    //フォームデータ
            if (!r)
            {
                return;
            }
            r = Mkdir("rebuild_mix");    //データとポインタの混在するデータ
            if (!r)
            {
                return;
            }
            r = Mkdir("rebuild_bin");    //固定データ
            if (!r)
            {
                return;
            }

            //各種スクリプトの辞書を構築.
            this.EventScriptWithoutPatchDic = new EventScript();
            this.EventScriptWithoutPatchDic.Load(FEBuilderGBA.EventScript.EventScriptType.Event_without_Patch);

            //念のためパッチのCheckIFをスキャンをやり直す.
            PatchForm.ClearCheckIF();

            wait.DoEvents("GrepAllStructPointers");
            List<DisassemblerTrumb.LDRPointer> ldrmap = DisassemblerTrumb.MakeLDRMap(Program.ROM.Data, 0x100, Program.ROM.RomInfo.compress_image_borderline_address(), true);
            this.StructList = U.MakeAllStructPointersList(false);
            U.AppendAllASMStructPointersList(StructList
                , ldrmap
                , isPatchInstallOnly: true
                , isPatchPointerOnly: false
                , isPatchStructOnly:  false
                , isPatchStoreSymbol: false
                , isUseOtherGraphics: true
                , isUseOAMSP: false
                );
            AppendPointer();

            wait.DoEvents(R._("データを準備中..."));
            //ポインタを高速に探索するためにルックアップテーブルを作成する.
            MakePointerMark();

            //残っているLDRデータを追加する.
            AppendLDR(ldrmap);
            ResolvUnkLength();

            //処理済みアドレスマーク
            bool[] processedAddress = new bool[32 * 1024 * 1024];
            List<RefCmd> refCmdList = new List<RefCmd>(65535);
            OptimizeList(wait,StructList);

            int nextDoEvents = 0;
            for (int i = 0; i < StructList.Count; i++)
            {
                Address a = StructList[i];

                if (processedAddress[a.Addr])
                {
                    continue;
                }
                processedAddress[a.Addr] = true;

                if (i > nextDoEvents)
                {//毎回更新するのは無駄なのである程度の間隔で更新して以降
                    wait.DoEvents(a.Info);
                    nextDoEvents = i + 0xff;
                }

                RefCmd refCmd;
                if (a.BlockSize > 0)
                {//ifrデータ
                    if (a.DataType == Address.DataTypeEnum.TEXTPOINTERS)
                    {//TEXT POINTERSは、Anti-Huffmanの可能性を考慮しないといけない.
                        refCmd = IFR(a,  ASMC_Delect.TEXTPOINTERS);
                    }
                    else if (a.DataType == Address.DataTypeEnum.InputFormRef)
                    {//データポインタ
                        refCmd = IFR(a,  ASMC_Delect.NONE);
                    }
                    else if (a.DataType == Address.DataTypeEnum.InputFormRef_ASM)
                    {//ASMポインタ
                        refCmd = IFR(a,  ASMC_Delect.AUTO);
                    }
                    else 
                    {//データとASMポインタの混在
                        refCmd = IFR(a,  ASMC_Delect.AUTO);
                    }
                }
                else if (Address.IsPointerableType(a.DataType))
                {//ポインタが存在するデータ
                    refCmd = Mix(a);
                }
                else
                {//固定データ ポインタはこの中には発生しない
                    refCmd = BIN(a);
                }

                if (refCmd.Cmd == "")
                {
                    Def(refCmd,a);
                }

                refCmd.UseAddress = a;
                refCmdList.Add(refCmd);
            }

            wait.DoEvents(R._("ポインタ検出中..."));
            for (int i = 0; i < StructList.Count; i++)
            {
                Address a = StructList[i];
                if (a.Pointer == U.NOT_FOUND)
                {
                    continue;
                }

                if (processedAddress[a.Pointer])
                {
                    continue;
                }
                processedAddress[a.Pointer] = true;

                Address p = new Address(a.Pointer, 4, U.NOT_FOUND, "POINTER_" + a.Info, Address.DataTypeEnum.POINTER);

                RefCmd refCmd;
                refCmd = Mix(p);
                if (refCmd.Cmd == "")
                {
                    Def(refCmd, p);
                }

                refCmd.UseAddress = p;
                refCmdList.Add(refCmd);
            }

            wait.DoEvents(R._("未知のハックを探索中..."));
            FindUnknownHack2(refCmdList,processedAddress);

            StringBuilder rebuildData = new StringBuilder();
            rebuildData.Append(RefSortSimple(wait, refCmdList));
//            rebuildData.Append(AppendAssert(ldrmap));

            U.WriteAllText(filename, rebuildData.ToString());
        }

        string AppendAssert(List<DisassemblerTrumb.LDRPointer> ldrmap)
        {
            Dictionary<uint, bool> alreadyProcess = new Dictionary<uint, bool>();


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ldrmap.Count; i++)
            {
                DisassemblerTrumb.LDRPointer ldr = ldrmap[i];

                if (alreadyProcess.ContainsKey(ldr.ldr_data_address))
                {
                    continue;
                }
                alreadyProcess[ldr.ldr_data_address] = true;

                if (isRebuildAddress(ldr.ldr_data_address) 
                    || !U.isSafetyOffset(ldr.ldr_data_address))
                {
                    continue;
                }
                uint p = U.toOffset(ldr.ldr_data);
                if (isRebuildAddress(p)
                    || !U.isSafetyOffset(p))
                {
                    continue;
                }   

                uint v = Program.ROM.u32(p);

                sb.Append("@_ASSERT ");
                sb.Append(U.ToHexString(ldr.ldr_data_address));
                sb.Append(" ");
                sb.Append(U.ToHexString(v));
                sb.Append("\t//");
                sb.Append(U.ToHexString(ldr.ldr_data));
                sb.Append(" ");
                sb.Append(Program.AsmMapFileAsmCache.GetName(ldr.ldr_data));
                sb.AppendLine();
            }
            return sb.ToString();
        }

        void Def(RefCmd refCmd, Address a)
        {
            Debug.Assert(refCmd.Cmd == "");
            refCmd.Cmd = "@DEF " + U.ToHexString8(a.Addr) + "\t//" + a.Info;
        }

        static int CompareRefCmd(RefCmd a, RefCmd b)
        {
            return (int)((int)a.UseAddress.Addr - (int)b.UseAddress.Addr);
        }

        string RefSortSimple(InputFormRef.AutoPleaseWait wait, List<RefCmd> refCmdList)
        {
            wait.DoEvents("Make List....");
            StringBuilder sb = new StringBuilder();
            U.CRC32 crc32 = new U.CRC32();
            sb.Append("@_CRC32 ");
            sb.Append(U.ToHexString(crc32.Calc(this.Vanilla.Data)));
            sb.AppendLine(" //"  + R._("対象とする無改造ROMのCRC32ハッシュ"));

            sb.Append("@_REBUILDADDRESS ");
            sb.Append(U.ToHexString(this.RebuildAddress));
            sb.AppendLine(" //" + R._("再構築を開始するアドレス。このアドレス以降を再構築します。"));

            refCmdList.Sort(CompareRefCmd);
            for (int i = 0; i < refCmdList.Count; i++)
            {
                RefCmd refCmd = refCmdList[i];

                sb.AppendLine(refCmd.Cmd);
            }

            wait.DoEvents("Save File!");
            return sb.ToString();

        }

        void MakePointerMark()
        {
            this.PointerMark = new Dictionary<uint, bool>(65535);
            for (int i = 0; i < StructList.Count; i++)
            {
                Address address = StructList[i];
                if (address.Pointer == U.NOT_FOUND || address.Pointer == 0)
                {
                }
                else
                {
                    this.PointerMark[U.toPointer(address.Pointer)] = true;
                }

                this.PointerMark[U.toPointer(address.Addr)] = true;
            }
        }
        RefCmd IFR(Address address, ASMC_Delect asmdelect)
        {
            Debug.Assert(address.BlockSize > 0);
            StringBuilder sb = new StringBuilder();

            uint vanila_addr;
            if (address.Pointer == 0 || address.Pointer == U.NOT_FOUND)
            {//ポインタがないので代わりに現在のアドレスで比較してみる.
                vanila_addr = address.Addr;
            }
            else if (U.isSafetyOffset(address.Pointer, this.Vanilla))
            {
                vanila_addr = this.Vanilla.p32(address.Pointer);
                if (!U.isSafetyOffset(vanila_addr, this.Vanilla))
                {//ポインタ先が不明なのでアドレスで補う
                    vanila_addr = address.Addr;
                }
            }
            else
            {//ポインタがないので、現在のアドレスで比較するしかない.
                vanila_addr = address.Addr;
            }

            string basename = MakeName(address);
            string filename = Path.Combine("rebuild_ifr", basename + ".txt");
            StringBuilder infsb = new StringBuilder();

            RefCmd refCmd = new RefCmd();

            uint addr = address.Addr;
            uint startaddr = addr;
            uint vanila_addr_work = address.Addr;
            uint end = addr + address.Length;                   //終端データまで含めたデータ末尾の位置
            uint endMinusOneBlock = end - address.BlockSize;    //終端データを含めないデータ末尾の位置

            byte[] bin = Program.ROM.Data;
            for (uint no = 0; addr < end; 
                addr += address.BlockSize, vanila_addr_work += address.BlockSize, no++)
            {
//                if (romcmp(addr,  vanila_addr_work, address.BlockSize))
//                {//無改造ROMと同一なので記録する必要なし.
//                    continue;
//                }

                infsb.Append("=");
                infsb.Append(U.ToHexString8(no));

                bool isTermData = (endMinusOneBlock == addr); //終端データ
                uint inner_end = Math.Min(addr + address.BlockSize, (uint)bin.Length);
                for (uint a = addr; a < inner_end; a++)
                {
                    infsb.Append(' ');
                    if (a % 4 == 0 //ARMなので4バイトアライメント
                        && isTermData == false //終端データのポインタは無効のデータがあるので無視していい
                        && U.GetIndexOf(address.PointerIndexes, a - addr) >= 0 //ポインタがあるカラムかどうか
                        )
                    {
                        bool r = IsRegistAddr(refCmd
                                    , infsb
                                    , bin
                                    , startaddr
                                    , endMinusOneBlock
                                    , a
                                    , asmdelect
                                    );
                        if (r)
                        {
                            a += 3;
                            continue;
                        }
                    }
                    infsb.Append(U.ToHexString(bin[a]));
                }
                infsb.AppendLine();
            }

            if (infsb.Length == 0)
            {//記録するデータがない
                return refCmd;
            }

            string fullfilename = Path.Combine(this.BaseDir, filename);
            U.WriteAllText(fullfilename, infsb.ToString());

            sb.Append("@IFR ");
            sb.Append(U.ToHexString8(address.Addr)); //addr
            sb.Append(" :");
            sb.Append(U.ToHexString(address.BlockSize)); //blocksize
            sb.Append(" *");
            sb.Append(U.ToHexString(address.Length / address.BlockSize)); //count
            if (vanila_addr == address.Addr)
            {//ポインタがないか、アドレスを変更していない。
            }
            else
            {
                sb.Append(" =");
                sb.Append(U.ToHexString8(vanila_addr)); //無改造ROMのアドレス
            }
            sb.Append(" ");
            sb.Append(filename);

            refCmd.Cmd = sb.ToString();
            return refCmd;
        }
        void Apeend4Bytes(StringBuilder sb, byte[] romdata, uint addr)
        {
            sb.Append(U.ToHexString(romdata[addr + 0]));
            sb.Append(' ');
            sb.Append(U.ToHexString(romdata[addr + 1]));
            sb.Append(' ');
            sb.Append(U.ToHexString(romdata[addr + 2]));
            sb.Append(' ');
            sb.Append(U.ToHexString(romdata[addr + 3]));
        }

        enum ASMC_Delect
        {
            NONE
            ,AUTO
            ,TEXTPOINTERS
            ,ASM
        };
        bool IsRegistAddrTextPointers(RefCmd refCmd
            , StringBuilder sb
            , byte[] romdata
            , uint startaddr
            , uint endaddr
            , uint addr
            )
        {
            uint srcp = U.u32(romdata, addr);
            uint srcoffset;
            bool isAntiHuffman;

            if (srcp >= 0x80000000 && srcp < 0x8A000000)
            {//Anti-Huffman
                isAntiHuffman = true;
                srcp -= 0x80000000;
            }
            else
            {//通常のポインタ
                isAntiHuffman = false;
            }


            if (!U.isSafetyPointer(srcp))
            {//正しいポインタではない.
                return false;
            }

            srcoffset = U.toOffset(srcp);

            if (PointerMark.ContainsKey(srcp))
            {
                if (isAntiHuffman)
                {
                    sb.Append('`');
                }
                else
                {
                    sb.Append('@');
                }
                sb.Append(U.ToHexString(srcp));
            
                return true;
            }

            return false;
        }

        bool IsRegistAddr(RefCmd refCmd
            , StringBuilder sb
            , byte[] romdata
            , uint startaddr
            , uint endaddr
            , uint addr
            , ASMC_Delect asmcdelect
            )
        {
            if (asmcdelect == ASMC_Delect.TEXTPOINTERS)
            {
                return IsRegistAddrTextPointers(refCmd, sb, romdata, startaddr, endaddr, addr);
            }

            uint srcp = U.u32(romdata, addr);
            if (!U.isSafetyPointer(srcp))
            {
                return false;
            }
            uint srcoffset = U.toOffset(srcp);

            if (PointerMark.ContainsKey(srcp))
            {
                if (srcoffset == startaddr
                 && IsEnableSelfPointer(asmcdelect)
                    )
                {//自己参照
                    sb.Append('+');
                    sb.Append(U.ToHexString(srcoffset - startaddr));
                }
                else if (asmcdelect == ASMC_Delect.ASM && U.IsValueOdd(srcp))
                {//ASMポインタ
                    sb.Append('&');
                    sb.Append(U.ToHexString(srcp - 1));
                }
                else
                {//ポインタ
                    sb.Append('@');
                    sb.Append(U.ToHexString(srcp));
                }

                return true;
            }
            else if (srcoffset >= startaddr && srcoffset < endaddr
                 && IsEnableSelfPointer(asmcdelect)
                )
            {//自己参照
                sb.Append('+');
                sb.Append(U.ToHexString(srcoffset - startaddr));
                return true;
            }
            else if (   U.IsValueOdd(srcp)
                        && PointerMark.ContainsKey(srcp - 1)
                        && IsEnableASMPointer(asmcdelect)                
                )
            {//ASM参照
                sb.Append('&');
                sb.Append(U.ToHexString(srcp));
                return true;
            }
            else
            {//謎のアドレス
                return false;
            }
        }
        bool IsEnableASMPointer(ASMC_Delect asmcdelect)
        {
            return asmcdelect == ASMC_Delect.AUTO || asmcdelect == ASMC_Delect.ASM;
        }
        bool IsEnableSelfPointer(ASMC_Delect asmcdelect)
        {
            return asmcdelect != ASMC_Delect.ASM;
        }

        //ワイルドカード
        bool WildCard(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length, ASMC_Delect asmc_delect)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);
            bool isPointer = false;

            for (; addr < end; addr++)
            {
                infsb.Append(' ');
                if (addr % 4 == 0)
                {
                    bool r = IsRegistAddr(refCmd
                                , infsb
                                , romdata
                                , startaddr
                                , end
                                , addr
                                , asmc_delect
                                );
                    if (r)
                    {
                        addr += 3;
                        isPointer = true;
                        continue;
                    }
                }
                infsb.Append(U.ToHexString(romdata[addr]));
            }

            return isPointer;
        }
        //MIX
        void MixRec(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length, uint[] pointerIndexes)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            for (; addr < end; addr++)
            {
                infsb.Append(' ');
                if (addr % 4 == 0
                    && U.GetIndexOf(pointerIndexes, addr - startaddr) >= 0)
                {
                    bool r = IsRegistAddr(refCmd
                                , infsb
                                , romdata
                                , startaddr
                                , end
                                , addr
                                , ASMC_Delect.AUTO
                                );
                    if (r)
                    {
                        addr += 3;
                        continue;
                    }
                }
                infsb.Append(U.ToHexString(romdata[addr]));
            }
        }

        //楽譜専用
        void SongScore(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            uint position = addr;
            uint percussion = 0;
            while (true)
            {
                uint b = romdata[position];
                position++;

                infsb.Append(' ');
                infsb.Append(U.ToHexString(b));

                if (b == 0xB1)
                {
                    break;
                }
                else if (b == 0xB2 || b == 0xB3)
                {
                    infsb.Append(' ');
                    //repointer
                    bool r = IsRegistAddr(refCmd
                                , infsb
                                , romdata
                                , startaddr
                                , end
                                , position
                                , ASMC_Delect.NONE
                                );
                    if (!r)
                    {
                        Apeend4Bytes(infsb, romdata, position);
                    }
                    position += 4;
                }
                else if (b == 0xBD || b == 0xBB || b == 0xBC || b == 0xBE || b == 0xBF || b == 0xC0 || b == 0xC1)
                {
                    // These commands take a data byte that must not be processed.
                    infsb.Append(' ');
                    infsb.Append(U.ToHexString(romdata[position]));
                    position++;
                }
                else if (b == 0xb9)
                {//MEMACC 4バイト命令
                    //最初の1バイトはコピー済みなので、残りの3バイトコピーする.
                    infsb.Append(' ');
                    infsb.Append(U.ToHexString(romdata[position]));
                    position++;
                    infsb.Append(' ');
                    infsb.Append(U.ToHexString(romdata[position]));
                    position++;
                    infsb.Append(' ');
                    infsb.Append(U.ToHexString(romdata[position]));
                    position++;
                }
                else if (percussion != 0 && b < 0x80)
                {
                    while (romdata[position] < 0x80)
                    {// Volume marker
                        infsb.Append(' ');
                        infsb.Append(U.ToHexString(romdata[position]));
                        position++;
                    }
                }
            }
        }

        //Font
        void Font(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length)
        {
            //同一ハッシュキーがあるため、リストをたどりながら目的のフォントを探します.
            //struct{
            //    void* next;
            //    byte  sjis2
            //    byte  width
            //    byte  nazo1
            //    byte  nazo2
            //} //sizeof()==8
            //+64byte bitmap(4pp)
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            //先頭だけポインタ
            {
                infsb.Append(' ');
                bool r = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr
                            , ASMC_Delect.NONE
                            );
                if (!r)
                {
                    Apeend4Bytes(infsb, romdata, addr);
                }
                addr += 4;
            }

            //以下データ
            for (; addr < end; addr++)
            {
                infsb.Append(' ');
                infsb.Append(U.ToHexString(romdata[addr]));
            }
        }


        //ASM
        void ASM(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length)
        {
            List<DisassemblerTrumb.LDRPointer> ldrmap = DisassemblerTrumb.MakeLDRMap(romdata, addr,addr + length, true);
            Dictionary<uint, bool> ldrmapFast = new Dictionary<uint, bool>();
            for (int i = 0; i < ldrmap.Count; i++)
            {
                DisassemblerTrumb.LDRPointer ldr = ldrmap[i];
                ldrmapFast[ldr.ldr_data_address] = true;
                Debug.Assert(ldr.ldr_data_address % 4 == 0);
            }

            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            for (; addr < end; addr++)
            {
                infsb.Append(' ');
                if (addr % 4 == 0
                    && ldrmapFast.ContainsKey(addr) )
                {
                    bool r = IsRegistAddr(refCmd
                                , infsb
                                , romdata
                                , startaddr
                                , end
                                , addr
                                , ASMC_Delect.ASM
                                );
                    if (r)
                    {
                        addr += 3;
                        continue;
                    }
                }
                infsb.Append(U.ToHexString(romdata[addr]));
            }
        }

        void NoPointer(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length)
        {
            uint end = Math.Min(addr + length, (uint)romdata.Length);
            for (; addr < end; addr++)
            {
                infsb.Append(' ');
                infsb.Append(U.ToHexString(romdata[addr]));
            }
        }

        void EventCond12_16(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            bool isFE7 = Program.ROM.RomInfo.version() == 7;


            while (addr < end)
            {
                if (addr + 12 > end)
                {
                    break;
                }

                uint type = romdata[addr];
                if (type == 0)
                {//終端
                    break;
                }

                infsb.Append(' ');
                Apeend4Bytes(infsb, romdata, addr + 0);

                infsb.Append(' ');
                bool r = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr + 4
                            , ASMC_Delect.NONE //常にイベント
                            );
                if (!r)
                {
                    Apeend4Bytes(infsb, romdata, addr + 4);
                }

                infsb.Append(' ');
                bool r2 = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr + 8
                            , ASMC_Delect.AUTO //ASMCの可能性あり
                            );
                if (!r2)
                {
                    Apeend4Bytes(infsb, romdata, addr + 8);
                }

                if (isFE7 && type == 0x02)
                {
                    infsb.Append(' ');
                    Apeend4Bytes(infsb, romdata, addr + 12);
                    addr += 16;
                }
                else
                {
                    addr += 12;
                }
            }
            //端数データがあれば記録する
            NoPointer(refCmd, infsb, romdata, addr, end - addr);
        }

        void EventCond12(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            while (addr < end)
            {
                if (addr + 12 > end)
                {//端数を何とか格納する
                    break;
                }

                uint type = romdata[addr];
                if (type == 0)
                {//終端
                    break;
                }
                infsb.Append(' ');
                Apeend4Bytes(infsb, romdata, addr + 0);

                infsb.Append(' ');
                bool r = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr + 4
                            , ASMC_Delect.NONE //常にイベント
                            );
                if (!r)
                {
                    Apeend4Bytes(infsb, romdata, addr + 4);
                }

                infsb.Append(' ');
                bool r2 = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr + 8
                            , ASMC_Delect.AUTO //ASMCの可能性あり
                            );
                if (!r2)
                {
                    Apeend4Bytes(infsb, romdata, addr + 8);
                }

                addr += 12;
            }
            //端数データがあれば記録する
            NoPointer(refCmd, infsb, romdata, addr, end - addr);
        }
        void EventCond16(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            bool isFE7 = Program.ROM.RomInfo.version() == 7;

            while (addr < end)
            {
                if (addr + 12 > end)
                {//端数を何とか格納する
                    break;
                }

                uint type = romdata[addr];
                if (type == 0)
                {//終端
                    break;
                }

                infsb.Append(' ');
                Apeend4Bytes(infsb, romdata, addr + 0);

                infsb.Append(' ');
                bool r = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr + 4
                            , ASMC_Delect.NONE //常にイベント
                            );
                if (!r)
                {
                    Apeend4Bytes(infsb, romdata, addr + 4);
                }

                infsb.Append(' ');
                bool r2 = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr + 8
                            , ASMC_Delect.AUTO //ASMCの可能性あり
                            );
                if (!r2)
                {
                    Apeend4Bytes(infsb, romdata, addr + 8);
                }

                infsb.Append(' ');
                bool r3 = IsRegistAddr(refCmd
                            , infsb
                            , romdata
                            , startaddr
                            , end
                            , addr + 12
                            , ASMC_Delect.AUTO //ASMCの可能性あり
                            );
                if (!r3)
                {
                    Apeend4Bytes(infsb, romdata, addr + 12);
                }

                addr += 16;
            }
            //端数データがあれば記録する
            NoPointer(refCmd, infsb, romdata, addr, end - addr);
        }

        void EventScript(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length, EventScript scriptDic)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);
            while (addr < end)
            {
                List<uint> pointerIndexes = new List<uint>();
                EventScript.OneCode code = scriptDic.DisAseemble(romdata, addr);
                for (int i = 0; i < code.Script.Args.Length; i++)
                {
                    EventScript.Arg arg = code.Script.Args[i];
                    EventScript.ArgType type = arg.Type;
                    if (type == FEBuilderGBA.EventScript.ArgType.POINTER
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_AIUNIT
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_ASM
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_EVENT
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_EVENTBATTLEDATA
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_EVENTMOVEDATA
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_PROCS
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_TALKGROUP
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_TEXT
                        || type == FEBuilderGBA.EventScript.ArgType.POINTER_UNIT
                        )
                    {
                        pointerIndexes.Add((uint)arg.Position);
                    }
                }

                MixRec(refCmd
                    , infsb
                    , romdata
                    , addr
                    , (uint)code.Script.Size
                    , pointerIndexes.ToArray());

                addr += (uint)code.Script.Size;
            }
        }

        //戦闘アニメ or 魔法アニメなどの 0x85フレームがあるもの専用
        void BattleAnimation(RefCmd refCmd, StringBuilder infsb, byte[] romdata, uint addr, uint length, uint pointerCount)
        {
            uint startaddr = addr;
            uint end = Math.Min(addr + length, (uint)romdata.Length);

            for (; addr < end; addr += 4)
            {
                if (!U.isSafetyZArray(addr + 4 + (4 * pointerCount), romdata))
                {
                    break;
                }

                infsb.Append(' ');
                Apeend4Bytes(infsb, romdata, addr + 0);

                if (romdata[addr + 3] != 0x86)
                {
                    continue;
                }
                for (int n = 0; n < pointerCount; n++)
                {
                    addr += 4;

                    infsb.Append(' ');
                    bool r = IsRegistAddr(refCmd
                                , infsb
                                , romdata
                                , startaddr
                                , end
                                , addr
                                , ASMC_Delect.NONE
                                );
                    if (!r)
                    {
                        Apeend4Bytes(infsb, romdata, addr);
                    }
                }
            }

            //端数データがあれば記録する.
            NoPointer(refCmd, infsb, romdata, addr, end - addr);
        }

        //LDRを呼び出しをBL Jumpと誤読してしまうことがあるので取り除く.
        bool IsFalseBL(List<DisassemblerTrumb.LDRPointer> ldrmap,uint addr)
        {
            for (int i = 0; i < ldrmap.Count; i++)
            {
                if (addr >= ldrmap[i].ldr_data_address
                    && addr < ldrmap[i].ldr_data_address + 4)
                {
                    return true;
                }
            }
            return false;
        }

        void ReScanLDR(Address address)
        {
            //LDRデータを再検出
            uint limit = address.Addr + address.Length;
            List<DisassemblerTrumb.LDRPointer> ldrmap = DisassemblerTrumb.MakeLDRMap(
                Program.ROM.Data
                , address.Addr
                , limit
                , true);
            for (int i = 0; i < ldrmap.Count; i++)
            {
                uint addr = ldrmap[i].ldr_data_address;
                if (addr >= address.Addr && addr < limit)
                {
                    ReScanOneLDR(addr);
                }
            }

            //BLマップの検出
            List<DisassemblerTrumb.BLPointer> blmap = DisassemblerTrumb.MakeBLMap(
                Program.ROM.Data
                , address.Addr
                , limit
                , true);
            for (int i = 0; i < blmap.Count; i++)
            {
                uint addrP = blmap[i].bl_function;
                uint addr = U.toOffset(addrP);
                if (!isRebuildAddress(addr))
                {//リビルド対象外のデータへのBLなので無視.
                    continue;
                }
                if (!U.isSafetyPointer(addrP))
                {//危険なポインタなので無視.
                    continue;
                }
                if (addrP % 4 != 0)
                {//4で割り切れない.
                    continue;
                }
                if (!PointerMark.ContainsKey(addrP)
                    && !PointerMark.ContainsKey(addrP - 1))
                {//まだ知らないデータ
                    if (DisassemblerTrumb.IsCallBX( Program.ROM.u16(addr) ))
                    {//bx r3 などの bx callなので無視してよい.
                        continue;
                    }
                    if (IsFalseBL(ldrmap, addr))
                    {//LDRDATAを誤爆しているので無視.
                        continue;
                    }
                    //内容チェック
                    Address a = GetBLFunctionAddress(addr);
                    if (a != null)
                    {//BLのとび先が存在している.
                        if (!(a.DataType == Address.DataTypeEnum.ASM
                            || a.DataType == Address.DataTypeEnum.PATCH_ASM
                            || a.DataType == Address.DataTypeEnum.BL_ASM)
                            )
                        {//ASMコード以外に飛ぼうとしている.
                            continue;
                        }
                    }

                    Address b = GetBLFunctionAddress(blmap[i].bl_address);
                    if (b != null)
                    {//BLの呼び出したコードの素性を知っている場合
                        if (!(b.DataType == Address.DataTypeEnum.ASM
                            || b.DataType == Address.DataTypeEnum.PATCH_ASM
                            || b.DataType == Address.DataTypeEnum.BL_ASM)
                            )
                        {//ASMコード以外がBLを呼び出した.
                            continue;
                        }
                    }


                    Address.AddAddress(this.StructList
                        , addr
                        , 0
                        , U.NOT_FOUND
                        , "BL_CALLED_" + U.ToHexString(blmap[i].bl_address)
                        , Address.DataTypeEnum.BL_ASM
                        );
                }
            }
        }


        //BLが割り込む先を知っているならば、そのアドレスを返す.
        Address GetBLFunctionAddress(uint addr)
        {
            for (int i = 0; i < this.StructList.Count; i++)
            {
                Address a = this.StructList[i];
                if (addr >= a.Addr
                    && addr < a.Addr + a.Length)
                {
                    //データに割り込もうとしているので違う.
                    return a;
                }
            }

            return null;
        }

        void ReScanOneLDR(uint addr)
        {
            uint srcp = Program.ROM.u32(addr);
            if (!U.isSafetyPointer(srcp))
            {
                return;
            }

            if (U.isSafetyOffset(addr, this.Vanilla))
            {
                uint orig = this.Vanilla.u32(addr);
                if (orig == srcp)
                {//無変更なので、何もしなくてもよい.
                    return;
                }
            }

            uint srcaddr = U.toOffset(srcp);

            //内容チェック
            if (!CheckTrueLDRData(srcaddr))
            {
                return;
            }

            //変更されているようなので、データを追加で記録しないといけない.
            AppendUnkLDRPointer(srcaddr, addr);
            AppendUnkLDRData(srcaddr, addr);
        }

        const uint UNKNOWN_DATA_DEFAULT_SIZE = 256;
        //わからないデータの長さを求める.
        uint CheckUnkLength(Address a,uint min0x09,uint max0x09)
        {
            uint length;
            if (U.isSafetyOffset(a.Addr, this.Vanilla))
            {
                length = CheckUnk0x08Length(a);
            }
            else
            {
                length = CheckUnk0x09Length(a, min0x09, max0x09);
            }
            return length;
        }


        uint CheckUnk0x09Length(Address a, uint min0x09, uint max0x09)
        {
            uint addr = U.toPointer(a.Addr);
            uint currentROMLength ;
            if (Program.ROM.Data.Length >= 0x02000000)
            {
                currentROMLength = 0x0A000000;
            }
            else
            {
                currentROMLength = U.toPointer((uint)Program.ROM.Data.Length);
            }
            for (uint i = addr + 2; i < currentROMLength; i += 2)
            {
                if (this.PointerMark.ContainsKey(i))
                {
                    uint length = i - addr;
                    if (length < min0x09)
                    {
                        return min0x09;
                    }
                    if (length > max0x09)
                    {
                        return max0x09;
                    }
                    return length;
                }
            }
            uint ret_length = currentROMLength - addr;
            if (ret_length > max0x09)
            {
                return max0x09;
            }
            return ret_length;
        }
        uint CheckUnk0x08Length(Address a)
        {
            uint addr = a.Addr;
            uint vallinaLength = (uint)this.Vanilla.Data.Length;
            vallinaLength = U.Padding2Before(vallinaLength);

            uint missCount = 0;
            uint maybeEnd = addr;
            for (uint i = addr; i < vallinaLength; i += 2)
            {
                uint c = Program.ROM.u16(i);
                uint v = this.Vanilla.u16(i);
                if (c == v)
                {
                    if (missCount == 0)
                    {
                        maybeEnd = i;
                    }

                    missCount++;
                    if (missCount >= 10)
                    {//10個ミスが続いたら終了.
                        return maybeEnd - addr;
                    }
                }
                else
                {
                    missCount = 0;
                }
            }
            return maybeEnd - addr;
        }

        EventScript EventScriptWithoutPatchDic;

        RefCmd Mix(Address address)
        {
            RefCmd refCmd = new RefCmd();

            if (address.Length <= 0)
            {//サイズが0の場合、推測します.
                if (address.DataType == Address.DataTypeEnum.MAGICFRAME_CSA
                    || address.DataType == Address.DataTypeEnum.MAGICFRAME_FEITORADV)
                {//間違ったフレームデータ
                    return BrokenData(address);
                }
                else
                {
                    return refCmd;
                }
            }


            uint vanila_addr;
            if (address.Pointer == 0 || address.Pointer == U.NOT_FOUND)
            {//ポインタがないので代わりに現在のアドレスで比較してみる.
                if (romcmp(address.Addr, address.Addr, address.Length))
                {//無改造ROMと同一なので記録する必要なし.
                    return refCmd;
                }
                vanila_addr = address.Addr;
            }
            else if (U.isSafetyOffset(address.Pointer, this.Vanilla))
            {
                if (romcmpPointer(address.Pointer, address.Pointer, address.Length))
                {//ポインタ上では、記録する必要なし
                    if (romcmp(address.Addr, address.Addr, address.Length))
                    {//無改造ROMと同一なので記録する必要なし.
                        return refCmd;
                    }
                }

                vanila_addr = this.Vanilla.p32(address.Pointer);
                if (!U.isSafetyOffset(vanila_addr, this.Vanilla))
                {//バニラのアドレスが不明
                    vanila_addr = address.Addr;
                }
            }
            else
            {//ポインタはあるがバニラにはないということは拡張領域にあるデータだと思われる.
                if (romcmp(address.Addr, address.Addr, address.Length))
                {//無改造ROMと同一なので記録する必要なし.
                    return refCmd;
                }
                vanila_addr = address.Addr;
            }


            string basename = MakeName(address);
            string filename = Path.Combine("rebuild_mix", basename + ".txt");
            StringBuilder infsb = new StringBuilder();

            StringBuilder sb = new StringBuilder();

            if (address.DataType == Address.DataTypeEnum.BATTLEFRAME)
            {//LZ77で圧縮が必要
                byte[] bin = LZ77.decompress(Program.ROM.Data, address.Addr);
                BattleAnimation(refCmd, infsb, bin, 0, (uint)bin.Length,  2);
                sb.Append("@MIXLZ77 ");
            }
            else if (address.DataType == Address.DataTypeEnum.MAGICFRAME_FEITORADV)
            {//魔法フレーム1
                BattleAnimation(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, 6);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.MAGICFRAME_CSA)
            {//魔法フレーム2
                BattleAnimation(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, 7);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.EVENTCOND_ALWAYS)
            {//常時条件
                EventCond12(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.EVENTCOND_OBJECT)
            {//オブジェクト
                EventCond12(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.EVENTCOND_TALK)
            {//会話
                if (Program.ROM.RomInfo.version() == 6)
                {
                    EventCond12(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                }
                else
                {
                    EventCond16(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                }
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.EVENTCOND_TURN)
            {//ターン
                if (Program.ROM.RomInfo.version() == 7)
                {
                    EventCond12_16(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                }
                else
                {
                    EventCond12(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                }
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.SONGSCORE)
            {//音楽データ
                SongScore(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.EVENTSCRIPT)
            {
                EventScript(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, EventScriptWithoutPatchDic);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.AISCRIPT)
            {
                EventScript(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, Program.AIScript);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.PROCS)
            {
                EventScript(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, Program.ProcsScript);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.SONGTRACK)
            {
                WildCard(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, ASMC_Delect.NONE);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.ASM
                || address.DataType == Address.DataTypeEnum.PATCH_ASM
                || address.DataType == Address.DataTypeEnum.BL_ASM
                )
            {
                ASM(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.FONT)
            {//フォント
                Font(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.POINTER)
            {//ポインタ
                bool r = WildCard(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, ASMC_Delect.NONE);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.POINTER_ASM)
            {//ポインタ
                bool r = WildCard(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, ASMC_Delect.ASM);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.NEW_TARGET_SELECTION_STRUCT)
            {//ポインタ
                bool r = WildCard(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, ASMC_Delect.ASM);
                sb.Append("@MIX ");
            }
            else if (address.DataType == Address.DataTypeEnum.POINTER_ARRAY)
            {//ポインタ
                bool r = WildCard(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, ASMC_Delect.NONE);
                sb.Append("@MIX ");
            }
            else
            {
                WildCard(refCmd, infsb, Program.ROM.Data, address.Addr, address.Length, ASMC_Delect.AUTO);
                sb.Append("@MIX ");
            }

            if (infsb.Length <= 0)
            {
                return refCmd;
            }


            //MIXデータを書き込む.
            infsb.Remove(0, 1);
            string fullfilename = Path.Combine(this.BaseDir, filename);
            U.WriteAllText(fullfilename, infsb.ToString());

            sb.Append(U.ToHexString8(address.Addr)); //addr

            if (address.DataType == Address.DataTypeEnum.BATTLEFRAME)
            {//LZ77で圧縮する場合、データのサイズが可変なので、上書きできるか調べるにはサイズデータが必要です.
                sb.Append(" :");
                sb.Append(U.ToHexString(address.Length)); //blocksize
            }

            sb.Append(" ");
            sb.Append(filename);


            refCmd.Cmd = sb.ToString();
            return refCmd;
        }

        RefCmd BIN(Address address)
        {
            Debug.Assert(address.BlockSize <= 0);

            RefCmd refCmd = new RefCmd();
            if (address.Length <= 0)
            {
                return BrokenData(address);
            }

            uint vanila_addr;
            if (address.Pointer == 0 || address.Pointer == U.NOT_FOUND)
            {//ポインタがないので代わりに現在のアドレスで比較してみる.
                if (romcmp(address.Addr, address.Addr, address.Length))
                {//無改造ROMと同一なので記録する必要なし.
                    return refCmd;
                }
                vanila_addr = address.Addr;
            }
            else if (U.isSafetyOffset(address.Pointer, this.Vanilla))
            {
                if (romcmpPointer(address.Pointer, address.Pointer, address.Length))
                {//無改造ROMと同一なので記録する必要なし.
                    if (romcmp(address.Addr, address.Addr, address.Length))
                    {//無改造ROMと同一なので記録する必要なし.
                        return refCmd;
                    }
                }
                vanila_addr = this.Vanilla.p32(address.Pointer);
                if (!U.isSafetyOffset(vanila_addr, this.Vanilla))
                {//バニラのアドレスが不明
                    vanila_addr = address.Addr;
                }
            }
            else
            {//ポインタはあるがバニラにはないということは拡張領域にあるデータだと思われる.
                if (romcmp(address.Addr, address.Addr, address.Length))
                {//無改造ROMと同一なので記録する必要なし.
                    return refCmd;
                }
                vanila_addr = address.Addr;
            }
            StringBuilder sb = new StringBuilder();

            string basename = MakeName(address);
            string filename = Path.Combine("rebuild_bin", basename + ".bin");

            sb.Append("@BIN ");
            sb.Append(U.ToHexString8(address.Addr));
            sb.Append(" ");
            sb.Append(filename);


            string fullfilename = Path.Combine(this.BaseDir, filename);
            byte[] bin = Program.ROM.getBinaryData(address.Addr, address.Length);
            U.WriteAllBytes(fullfilename, bin);

            refCmd.Cmd = sb.ToString();
            return refCmd;
        }

        //壊れたデータ
        RefCmd BrokenData(Address address)
        {
            RefCmd refCmd = new RefCmd();
            StringBuilder sb = new StringBuilder();

            string basename = MakeName(address);

            sb.Append("@BROKENDATA ");
            sb.Append(U.ToHexString8(address.Addr));
            sb.Append("//");
            sb.Append(address.Info);

            refCmd.Cmd = sb.ToString();
            return refCmd;
        }
        string MakeName(Address address)
        {
            string basename;
            if (address.Info != "")
            {
                basename = U.escape_filename(address.Info) + "_";
                basename = basename.Replace(' ', '_');
                basename = basename.Replace("_____", "_");
                basename = basename.Replace("____", "_");
                basename = basename.Replace("___", "_");
                basename = basename.Replace("__", "_");
            }
            else
            {
                basename = "";
            }

            if (basename.Length >= 25)
            {
                basename = basename.Substring(0,25);
            }

            if (address.Pointer == 0 || address.Pointer == U.NOT_FOUND)
            {
                return basename + "." + U.ToHexString8(address.Addr);
            }
            else
            {
                return basename + ".P" + U.ToHexString8(address.Pointer);
            }
        }



    }
}
