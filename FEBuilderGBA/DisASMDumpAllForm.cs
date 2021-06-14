using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    public partial class DisASMDumpAllForm : Form
    {
        public DisASMDumpAllForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }


        public enum Func
        {
             Func_DisASM
            ,Func_IDAMAP
            ,Func_NODOLLSYM
        };
        Func CallFunc;
        private void AllDisASMButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CallFunc = DisASMDumpAllForm.Func.Func_DisASM;
            this.Close();
        }

        private void AllMakeMAPFileButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CallFunc = DisASMDumpAllForm.Func.Func_IDAMAP;
            this.Close();
        }
        public Func GetCallFunc()
        {
            return this.CallFunc;
        }

        private void AllMakeMAPFileNodollSymButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CallFunc = DisASMDumpAllForm.Func.Func_NODOLLSYM;
            this.Close();
        }

        public static void RunAllMakeNoDollSymFileButton(Form self)
        {
            if (InputFormRef.IsPleaseWaitDialog(self))
            {//2重割り込み禁止
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("sym|*.sym|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(self, "", save, U.GetFirstPeriodFilename(Program.ROM.Filename));

            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(self))
            {
                //No$gba用のシンボルを作る.
                AllMakeNoDollSymFile(self, save.FileNames[0] , wait);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileNames[0]);
        }

        public static void AllMakeNoDollSymFile(Form self, string symfilename, InputFormRef.AutoPleaseWait wait)
        {
            Encoding encoding = U.GetSystemDefault();

            using (StreamWriter writer = new StreamWriter(symfilename, false, encoding))
            {
                wait.DoEvents("GrepAllStructPointers");
                AsmMapFile asmMapFile = new AsmMapFile();

                DisassemblerTrumb Disassembler = new DisassemblerTrumb(asmMapFile);


                List<DisassemblerTrumb.LDRPointer> ldrmap = Program.AsmMapFileAsmCache.GetLDRMapCache();

                List<Address> structlist = new List<Address>();
                //no$gbaは32546lines以上の symを読みこむと落ちるので手加減する.
                //                List<Address> structlist = U.MakeAllStructPointersList(); //既存の構造体
                U.AppendAllASMStructPointersList(structlist
                    , ldrmap
                    , isPatchInstallOnly: true
                    , isPatchPointerOnly: false
                    , isPatchStructOnly: false
                    , isUseOtherGraphics: false
                    , isUseOAMSP: false
                    );
                asmMapFile.AppendMAP(structlist);

                //コメントデータ
                Program.CommentCache.MakeAddressList(structlist);
                asmMapFile.AppendMAP(structlist);

                uint lastNumber = 0;
                string line;
                Dictionary<uint, AsmMapFile.AsmMapSt> asmmap = asmMapFile.GetAsmMap();
                foreach (var pair in asmmap)
                {
                    if (pair.Key == 0x0 || pair.Key == U.NOT_FOUND)
                    {
                        continue;
                    }
                    if (pair.Key >= 0x08000000 && pair.Key <= 0x08000200)
                    {
                        continue;
                    }

                    if (pair.Value.IsPointer)
                    {//ポインタデータは不要
                        continue;
                    }

                    //長いと不便なので、名前以外不要.
                    string name = pair.Value.Name;
                    name = U.term(name, "\t");
                    name = name.Replace(" ", "_"); //スペースがあるとダメらしい.
                    name = name.Trim();
                    if (name == "")
                    {//名前が空
                        continue;
                    }

                    if (name.IndexOf("6CStructHeader") >= 0)
                    {//6Cは全部表示する
                    }
                    else
                    {
                        uint arrayNumner = ParseArrayIndex(name);
                        if (arrayNumner >= 10)
                        {//容量削減のため2桁の配列は1つのみ
                            if (lastNumber == arrayNumner)
                            {
                                continue;
                            }
                            lastNumber = arrayNumner;
                        }
                    }

                    line = string.Format("{0} {1}", U.ToHexString(pair.Key), name);

                    writer.WriteLine(line);
                }
            }
        }

        //配列の値があったら、添え字を返す.
        static uint ParseArrayIndex(string str)
        {
            Match deep = RegexCache.Match(str, @"\[(\d+)\]");
            if (deep.Groups.Count >= 2)
            {
                string m = deep.Groups[1].ToString();
                return U.atoi(m);
            }
            return 0;
        }
        public static void TEST_ParseArrayIndex()
        {
            uint r = ParseArrayIndex("aaa");
            Debug.Assert(r == 0);

            r = ParseArrayIndex("aaa[10]");
            Debug.Assert(r == 10);

            r = ParseArrayIndex("Unit@Player[10].UnitPointer");
            Debug.Assert(r == 10);
        }

        static Dictionary<uint, Address> MakeAllStructMapping(List<Address> structlist)
        {
            Dictionary<uint, Address> dic = new Dictionary<uint, Address>();

            for (int i = 0; i < structlist.Count; i++)
            {
                Address a = structlist[i];
                if (a.Length < 2)
                {
                    continue;
                }

                uint addr = U.Padding2Before(a.Addr);
                uint length = addr + a.Length;
                for (; addr < length; addr += 2)
                {
                    if (!dic.ContainsKey(addr))
                    {
                        dic.Add(addr, a);
                    }
                }
            }
            for (int i = 0; i < structlist.Count; i++)
            {
                Address a = structlist[i];
                if (a.Pointer == U.NOT_FOUND)
                {
                    continue;
                }
                uint addr = U.Padding2Before(a.Pointer);
                if (!dic.ContainsKey(addr))
                {
                    dic.Add(addr, a);
                }
                if (!dic.ContainsKey(addr + 2))
                {
                    dic.Add(addr + 2, a);
                }
            }
            return dic;
        }
        static void UnpackBINByCode(List<Address> structlist)
        {
            for (int i = 0; i < structlist.Count; i++)
            {
                Address a = structlist[i];
                if (a.Addr < Program.ROM.RomInfo.compress_image_borderline_address())
                {
                    if (a.Info.IndexOf("@BIN") >= 0)
                    {
                        a.Length = 0;
                    }
                }
            }
        }

        public static void RunAllDisASMButton(Form self)
        {
            if (InputFormRef.IsPleaseWaitDialog(self))
            {//2重割り込み禁止
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            MakeAllDisASMButton(self, save.FileNames[0], notifyUpdateMessage: false);
        }

        public static void MakeAllDisASMButton(Form self, string store_filename, bool notifyUpdateMessage)
        {
            uint addr = 0x100;

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(self))
            using (StreamWriter writer = new StreamWriter(store_filename))
            {
                writer.WriteLine("//FEBuilderGBA " + R._("逆アセンブラ"));
                if (notifyUpdateMessage)
                {
                    writer.WriteLine("//" +  DateTime.Now.ToString("yyyyMMdd") +  " " + R._("ソースコードを更新する場合は、このファイル消すか、0バイトの空ファイルにしてください。"));
                }

                wait.DoEvents("GrepAllStructPointers");

                AsmMapFile asmMapFile = new AsmMapFile();

                DisassemblerTrumb Disassembler = new DisassemblerTrumb(asmMapFile);

                List<DisassemblerTrumb.LDRPointer> ldrmap = Program.AsmMapFileAsmCache.GetLDRMapCache();

                List<Address> structlist = U.MakeAllStructPointersList(false); //既存の構造体
                U.AppendAllASMStructPointersList(structlist
                    , ldrmap
                    , isPatchInstallOnly: false
                    , isPatchPointerOnly: false
                    , isPatchStructOnly: false
                    , isUseOtherGraphics: true
                    , isUseOAMSP: true
                    );
                UnpackBINByCode(structlist);

                AsmMapFile.MakeFreeDataList(structlist, 0x100, 0x00, 16); //フリー領域
                AsmMapFile.MakeFreeDataList(structlist, 0x100, 0xFF, 16); //フリー領域
                asmMapFile.AppendMAP(structlist);

                //コメントデータ
                Program.CommentCache.MakeAddressList(structlist);
                asmMapFile.AppendMAP(structlist);


                {//設計をミスった。 綺麗なリストを作りたいので、もう一回読みこみなおそう...
                    AsmMapFile asmMapFile2 = new AsmMapFile();
                    asmMapFile2.MakeAllDataLength(structlist);
                }

                uint limit = (uint)Program.ROM.Data.Length;

                int jisage = 0;  //字下げする数
                string jisageSpaceData = "";  //字下げに利用するマージンデータ
                List<uint> jmplabel = new List<uint>();  //ジャンプラベル　字下げに使う
                Dictionary<uint, uint> ldrtable = new Dictionary<uint, uint>();  //LDR参照データがある位置を記録します. コードの末尾などにあります. 数が多くなるのでマップする.
                AsmMapFile.MakeSwitchDataList(ldrtable, 0x100, 0);

                wait.DoEvents(R._("データを準備中..."));
                //探索を早くするために、データをアドレスへマッピングする. メモリを大量に使うが早い.
                Dictionary<uint, Address> lookupStructMap = MakeAllStructMapping(structlist);
                structlist = null;

                uint nextDoEvents = 0;
                bool prevPointer = false; //ひとつ前がポインタだった
                Address matchAddress;
                DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
                while (addr < limit)
                {
                    if (addr > nextDoEvents)
                    {//毎回更新するのは無駄なのである程度の間隔で更新して以降
                        wait.DoEvents(String.Format("{0}/{1}", addr, limit));
                        nextDoEvents = addr + 0xfff;
                    }


                    if (lookupStructMap.TryGetValue(addr, out matchAddress))
                    {
                        if (matchAddress.Pointer <= addr && addr < matchAddress.Pointer + 4)
                        {//ポインタ?
                            writer.WriteLine(U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, 4) + "   //POINTER " + matchAddress.Info);
                            addr += 4;
                        }
                        else
                        {//データ
                            uint newaddr = U.Padding2(matchAddress.Addr + matchAddress.Length);
                            uint length = (newaddr - addr);
                            Debug.Assert(length < 10000000);
                            writer.WriteLine("{0} - {1} //{2} ({3}bytes)", U.toPointer(addr).ToString("X08"), U.toPointer(newaddr).ToString("X08"), matchAddress.Info, length);
                            addr = U.Padding4(newaddr);
                        }
                        prevPointer = true;
                        continue;
                    }

                    //LDR参照とスイッチ参照
                    if (ldrtable.ContainsKey(addr))
                    {//LDR参照のポインタデータが入っている
                        uint ldr = ldrtable[addr];
                        if (ldr == U.NOT_FOUND)
                        {//switch case
                            writer.WriteLine(U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, 4) + "   //SWITCH CASE");
                        }
                        else
                        {
                            writer.WriteLine(U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, 4) + "   //LDRDATA");
                        }
                        addr += 4;
                        prevPointer = true;
                        continue;
                    }

                    if (prevPointer)
                    {//ひとつ前がポインタの場合、野生のポインタをチェック
                        uint data = Program.ROM.u32(addr);
                        if (U.isPointer(data))
                        {
                            if (lookupStructMap.TryGetValue(U.toOffset(data), out matchAddress))
                            {
                                writer.WriteLine(U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, 4) + "   //Wild POINTER " + U.ToHexString8(data) + " " + matchAddress.Info);
                                addr += 4;
                                continue;
                            }
                        }
                    }

                    //ひとつ前はポインタではない.
                    prevPointer = false;

                    //Disassembler
                    DisassemblerTrumb.Code code =
                        Disassembler.Disassembler(Program.ROM.Data, addr, limit, vm);
                    if (code.Type == DisassemblerTrumb.CodeType.BXJMP)
                    {//関数の出口なので字下げをすべて取り消す.
                        jisage = 0;
                        jmplabel.Clear();
                        jisageSpaceData = "";
                    }
                    else
                    {
                        for (int i = 0; i < jmplabel.Count; )
                        {
                            if (addr >= jmplabel[i])
                            {
                                jmplabel.RemoveAt(i);
                                jisage--;
                                jisageSpaceData = U.MakeJisageSpace(jisage);
                                i = 0;
                                continue;
                            }
                            i++;
                        }
                    }

                    writer.WriteLine(jisageSpaceData + U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, code.GetLength()) + "   " + code.ASM.ToLower() + code.Comment);

                    if (code.Type == DisassemblerTrumb.CodeType.CONDJMP //条件式なので字下げ開始
                        )
                    {
                        uint jumplabel = U.toOffset(code.Data);
                        if (addr < jumplabel)
                        {//とび先が自分より後ろであること. 前方はすでに過ぎてしまったので字下げできない.
                            jisage++;
                            jmplabel.Add(jumplabel);
                            jisageSpaceData = U.MakeJisageSpace(jisage);

                        }
                    }
                    else if (code.Type == DisassemblerTrumb.CodeType.BXJMP)
                    {//関数の終わりなので空行を入れる.
                        writer.WriteLine("");
                    }

                    if (code.Type == DisassemblerTrumb.CodeType.LDR)
                    {//LDR参照位置を記録していく.
                        ldrtable[code.Data2] = addr;
                    }

                    addr += code.GetLength();
                }
            }

            if (self != null)
            {
                //エクスプローラで選択しよう
                U.SelectFileByExplorer(store_filename);
            }
        }

        public static void RunAllMakeMAPFileButton(Form self)
        {
            if (InputFormRef.IsPleaseWaitDialog(self))
            {//2重割り込み禁止
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("MAP|*.map|TEXT|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(self))
            using (StreamWriter writer = new StreamWriter(save.FileNames[0]))
            {
                writer.WriteLine(" Start         Length     Name                   Class");

                wait.DoEvents("GrepAllStructPointers");
                AsmMapFile asmMapFile = new AsmMapFile();

                DisassemblerTrumb Disassembler = new DisassemblerTrumb(asmMapFile);

                List<DisassemblerTrumb.LDRPointer> ldrmap = Program.AsmMapFileAsmCache.GetLDRMapCache();

                List<Address> structlist = U.MakeAllStructPointersList(false); //既存の構造体
                U.AppendAllASMStructPointersList(structlist
                    , ldrmap
                    , isPatchInstallOnly: false
                    , isPatchPointerOnly: false
                    , isPatchStructOnly: false
                    , isUseOtherGraphics: true
                    , isUseOAMSP: true
                    );
                AsmMapFile.MakeFreeDataList(structlist, 0x100, 0x00, 16); //フリー領域
                AsmMapFile.MakeFreeDataList(structlist, 0x100, 0xFF, 16); //フリー領域
                asmMapFile.AppendMAP(structlist);

                //コメントデータ
                Program.CommentCache.MakeAddressList(structlist);
                asmMapFile.AppendMAP(structlist);


                string line;
                Dictionary<uint, AsmMapFile.AsmMapSt> asmmap = asmMapFile.GetAsmMap();
                foreach (var pair in asmmap)
                {
                    if (pair.Key == 0x0 || pair.Key == U.NOT_FOUND)
                    {
                        continue;
                    }
                    if (pair.Key >= 0x08000000 && pair.Key <= 0x08000200)
                    {
                        continue;
                    }
                    //長いと不便なので、名前以外不要.
                    string name = pair.Value.Name;
                    name = U.term(name, "\t");

                    if (pair.Value.Length > 0)
                    {
                        line = string.Format(" 0000:{0} 0{1}H {2}  DATA", U.ToHexString(pair.Key), pair.Value.Length.ToString("X08"), name);
                    }
                    else
                    {
                        line = string.Format(" 0000:{0}       {1}", U.ToHexString(pair.Key), name);
                    }

                    writer.WriteLine(line);
                }



            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileNames[0]);
        }

        private void DisASMDumpAllForm_Load(object sender, EventArgs e)
        {

        }

        private void ArgGrepButton1_Click(object sender, EventArgs e)
        {
            DisASMDumpAllArgGrepForm f = (DisASMDumpAllArgGrepForm)InputFormRef.JumpFormLow<DisASMDumpAllArgGrepForm>();
            f.ShowDialog();
        }

        public static int CommandLineDisasm(string filename)
        {
            U.echo("CommandLineDisasm");

            MakeAllDisASMButton(null, filename, notifyUpdateMessage: true);
            return 0;
        }
    }
}
