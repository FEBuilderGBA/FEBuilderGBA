using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace FEBuilderGBA
{
    //EAの簡易パーサ 今は、簡単なことしかできません。
    class EAUtil
    {
        public enum DataEnum
        {
            ORG
            ,MIX //incbinされたデータ 判別不能
            ,ASM //incbinされたデータ ASM
            ,BIN //incbinされたデータ BIN
            ,LYN //lynによってインポートされるelfファイル
            ,LYNHOOK //lynによるフック 16バイト
            ,POINTER_ARRAY
            ,PROCS
        }
        public class Data
        {
            public string Name { get; private set; }
            public string Dir { get; private set; }
            public uint ORGAddr { get; private set; }
            public byte[] BINData { get; private set; }
            public DataEnum DataType { get; private set; }
            public uint Append { get; private set; }

            public Data(uint orgaddr, DataEnum dataType, uint append = 0)
            {
                this.ORGAddr = orgaddr;
                this.DataType = dataType;
                this.Append = append;
                this.Name = "";
                this.Dir = "";
            }
            public Data(string name, byte[] data, DataEnum dataType, uint append = 0)
            {
                this.Name = name;
                this.Dir = "";
                this.BINData = data;
                this.DataType = dataType;
                this.Append = append;
            }
            public Data(string filename, string dir, byte[] data, DataEnum dataType, uint append = 0)
            {
                this.Name = filename;
                this.Dir = dir;
                this.BINData = data;
                this.DataType = dataType;
                this.Append = append;
            }
        }
        public List<Data> DataList { get; private set; }
        public List<string> IfNDefList { get; private set; }
        public string Filename { get; private set; }
        public string Dir { get; private set; }
        EAUtilLynDumpMode LynDump;

        public EAUtil(string filename)
        {
            Parse(filename);
        }

        void Parse(string filename)
        {
            this.DataList = new List<Data>();
            this.Filename = filename;
            this.Dir = Path.GetDirectoryName(filename);
            this.IfNDefList = new List<string>();
       
            this.CurrentLabel = "";
            string[] lines = File.ReadAllLines(filename);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = U.ClipComment(lines[i]);
                if (line == "")
                {
                    continue;
                }

                ParseJumpToHack(line);
                ParseIfNDef(line);
            }
            //本格的なパース.
            for (int i = 0; i < lines.Length; i++)
            {
                string line = U.ClipComment(lines[i]);
                if (ParseLynDump(line, lines[i]))
                {
                    continue;
                }
                if (line == "")
                {
                    continue;
                }

                ParseORG(line);
                ParseIncBIN(line, lines[i]);
                ParseLynELF(line, lines[i]);
                ParseLynHook(line, lines[i]);
                ParseLynInclude(line, lines[i]);
                ParsePng2Dmp(line, lines[i]);
                ParseString(line, lines[i]);
                ParseLabel(line, lines[i]);
            }
            ParseLynDump("", "");
        }
        bool ParseLynDump(string line, string orignalIine)
        {
            if (this.LynDump == null)
            {
                if (line == "")
                {
                    return false;
                }
                if (orignalIine.IndexOf("// lyn output of ", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    this.LynDump = new EAUtilLynDumpMode();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (this.LynDump == null)
            {
                return false;
            }

            bool r = this.LynDump.ParseLine(line);
            if (r)
            {//LYNDUMPの処理を行うので、続きのパースは不要.
                return true;
            }

            //LYNDUMPの終了
            AddLynDump(this.LynDump, "");

            this.LynDump = null;
            return false;
        }
        void AddLynDump(EAUtilLynDumpMode lyn,string filename)
        {
            if (lyn.GetCount() == 0)
            {//ORG指定がない場合
                if (filename == "")
                {
                    filename = "LYNDUMP";
                }
                Data data = new Data(filename, this.Dir, lyn.GetDataAll(), DataEnum.LYN, 0);
                this.DataList.Add(data);
                return;
            }

            int count = lyn.GetCount();
            for (int i = 0; i < count; i++)
            {
                Data data = new Data("LYNDUMP_" + lyn.GetName(i), lyn.GetData(i), DataEnum.LYN, 0);
                this.DataList.Add(data);
            }
        }

        void ParseLabel(string line, string orignalIine)
        {
            string a = line.Trim();
            int pos = a.IndexOf(':');
            if (pos < 0)
            {
                return ;
            }
            this.CurrentLabel = a.Substring(0, pos);


            if (orignalIine.IndexOf("HINT=POINTER_ARRAY", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                uint append = ParseAdd(orignalIine);
                Data data = new Data(this.CurrentLabel, new byte[] { }, DataEnum.POINTER_ARRAY, append);
                this.DataList.Add(data);
            }
            else if (orignalIine.IndexOf("HINT=PROCS", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                uint append = ParseAdd(orignalIine);
                byte[] procsTopBIN = ParseProcs(orignalIine);
                Data data = new Data(this.CurrentLabel, procsTopBIN, DataEnum.PROCS, append);
                this.DataList.Add(data);
            }
        }
        byte[] ParseProcs(string orignalIine)
        {
            int pos = orignalIine.IndexOf("HINT=PROCS");
            if (pos < 0)
            {
                return new byte[] { };
            }
            string str = orignalIine.Substring(pos);
            str = U.cut(str, "\"", "\"");
            if (str == "")
            {
                return new byte[] { };
            }
            byte[] needString =  Program.SystemTextEncoder.Encode(str);
            uint hintStringAddr = U.Grep(Program.ROM.Data, needString, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
            if (hintStringAddr == U.NOT_FOUND)
            {
                return new byte[] { };
            }
            byte[] needProcString = new byte[8] ;
            U.write_u8(needProcString , 0 , 0x01);
            U.write_p32(needProcString, 4, hintStringAddr);

            return needProcString;
        }
        int FindJumpToHack(string line)
        {
            int pos = line.IndexOf("jumpToHack", StringComparison.OrdinalIgnoreCase);
            if (pos >= 0)
            {
                return pos;
            }
            pos = line.IndexOf("callHack", StringComparison.OrdinalIgnoreCase);
            if (pos >= 0)
            {
                return pos;
            }
            pos = line.IndexOf("replaceWithHack", StringComparison.OrdinalIgnoreCase);
            if (pos >= 0)
            {
                return pos;
            }
            return -1;
        }

        void ParseIfNDef(string line)
        {
            int pos = line.IndexOf("#ifndef", StringComparison.OrdinalIgnoreCase);
            if (pos < 0)
            {
                return;
            }

            string ifdef_keyword = line.Substring(pos + 7 + 1).Trim();
            if (ifdef_keyword == "")
            {
                return;
            }
            IfNDefList.Add(ifdef_keyword);
        }

        void ParseJumpToHack(string line)
        {
            int pos = FindJumpToHack(line);
            if (pos < 0)
            {
                return;
            }
            string label = U.cut(line, "(", ")");
            JumpToHackLabeles.Add(label);
        }
        bool isCode(string fullPath)
        {
            //ソースコードがあればASMだろう.
            string srcFilename = U.ChangeExtFilename(fullPath, ".s");
            if (File.Exists(srcFilename))
            {//ソースコードがあったのでASMです
                return true;
            }
            srcFilename = U.ChangeExtFilename(fullPath, ".asm");
            if (File.Exists(srcFilename))
            {//ソースコードがあったのでASMです
                return true;
            }

            if (this.JumpToHackLabeles.IndexOf(this.CurrentLabel) >= 0)
            {//JumpToHackで呼び出されているのでASMです
                return true;
            }
            return false;
        }

        string CurrentLabel;
        List<string> JumpToHackLabeles = new List<string>();


        bool ParseIncBIN(string line , string orignalIine)
        {
            string a = Keyword(line, "#incbin");
            if (a == "")
            {
                return false;
            }
            string filename = U.cut(a, "\"", "\"");
            string fullbinname = Path.Combine( this.Dir, filename);

            DataEnum dataType ;
            if (orignalIine.IndexOf("HINT=BIN", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                dataType = DataEnum.BIN;
            }
            else if (orignalIine.IndexOf("HINT=ASM", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                dataType = DataEnum.ASM;
            }
            else if (filename.IndexOf(".png.") >= 0)
            {//png
                dataType = DataEnum.BIN;
            }
            else
            {//ヒントがない場合は、 JumpToHackの有無で判断します.
                if (isCode(fullbinname))
                {
                    dataType = DataEnum.ASM;
                }
                else
                {
                    dataType = DataEnum.MIX;
                }
            }
            if (!File.Exists(fullbinname))
            {
                Data emptydata = new Data(filename, this.Dir, new byte[0], dataType);
                this.DataList.Add(emptydata);
                return false;
            }

            Data data = new Data(filename, this.Dir, File.ReadAllBytes(fullbinname), dataType);
            this.DataList.Add(data);
            return true;
        }
        bool ParseLynHook(string line, string orignalIine)
        {
            string keyword = "HINT=LYN_HOOK=";
            int pos = orignalIine.IndexOf(keyword);
            if (pos < 0)
            {
                return false;
            }
            uint orgaddr = U.atoi0x(orignalIine.Substring(pos + keyword.Length));

            Data data = new Data(orgaddr, DataEnum.ORG);
            this.DataList.Add(data);
            return true;
        }

        bool ParseLynELF(string line , string orignalIine)
        {
            bool inctevent_lyn = false;
            string a = Keyword(line, "#inctevent lyn");
            if (a == "")
            {
                a = Keyword(line, "#inctext lyn");
                if (a == "")
                {
                    return false;
                }
            }
            else
            {
                inctevent_lyn = true;
            }
            string filename = U.cut(a, "\"", "\"");
            string fullbinname = Path.Combine( this.Dir, filename);

            DataEnum dataType = DataEnum.LYN;
            if (!File.Exists(fullbinname))
            {
                Data emptydata = new Data(filename, this.Dir, new byte[0], dataType);
                this.DataList.Add(emptydata);
                return false;
            }

            Elf elf = new Elf(fullbinname , useHookMode: false);
            Data data = new Data(filename, this.Dir, elf.ProgramBIN, dataType);
            this.DataList.Add(data);

            if (inctevent_lyn == false)
            {
                ParseLynSecondArgs(a);
            }
            return true;
        }
        bool ParseLynSecondArgs(string a )
        {
            //次の引数へ
            a = U.skip(a, "\"");
            a = U.skip(a, "\"");
            string filename = U.cut(a, "\"", "\"");
            string fullbinname = Path.Combine(this.Dir, filename);

            if (!File.Exists(fullbinname))
            {
                return false;
            }

            Elf elf = new Elf(fullbinname, useHookMode: true);
            foreach (Elf.Sym sym in elf.SymList)
            {
                if (! U.isPointer(sym.addr))
                {
                    continue;
                }
                uint addr = U.toOffset(sym.addr);
                addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
                Data data = new Data(addr, DataEnum.LYNHOOK);
                this.DataList.Add(data);
            }

            return true;
        }
        bool ParseLynInclude(string line, string orignalIine)
        {
            int start = line.IndexOf("#include");
            if (start < 0)
            {
                return false;
            }

            int lyn_event_pos = line.IndexOf("lyn.event",start);
            if (lyn_event_pos < 0)
            {
                return false;
            }
            string filename = U.cut(line, "\"", "\"");
            string fullfilename = Path.Combine(this.Dir, filename);

            if (!File.Exists(fullfilename))
            {
                return false;
            }

            string[] lines = File.ReadAllLines(fullfilename);
            EAUtilLynDumpMode lyndmp = new EAUtilLynDumpMode();
            foreach (string l in lines)
            {
                bool r = lyndmp.ParseLine(l);
                if (!r)
                {
                    break;
                }
            }

            //LYNDUMPの終了
            AddLynDump(lyndmp, filename);
            return true;
        }
        bool ParseString(string line, string orignalIine)
        {
            int start = line.IndexOf("String(");
            if (start < 0)
            {
                return false;
            }

            start += 7;
            int term = line.IndexOf(')', start);
            if (term <= 0)
            {
                return false;
            }
            string str = line.Substring(start, term - start);
            byte[] lowbin = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(str);

            uint size = (uint)lowbin.Length + 1;
            byte[] bin = new byte[size];
            Array.Copy(lowbin, bin, lowbin.Length);

            Data data = new Data("String("+str+")", bin,DataEnum.BIN);
            this.DataList.Add(data);

            return true;
        }

        bool ParsePng2Dmp(string line, string orignalIine)
        {
            string a = Keyword(line, "#incext Png2Dmp");
            if (a == "")
            {
                return false;
            }
            string filename = U.cut(a, "\"", "\"");
            string fullbinname = Path.Combine(this.Dir, filename);

            if (!File.Exists(fullbinname))
            {
                return false;
            }

            DataEnum dataType = DataEnum.BIN;
            if (orignalIine.IndexOf("--lz77") >= 0)
            {
                byte[] image = Png2DmpLZ77(fullbinname);
                Data data = new Data(filename, this.Dir, image, dataType);
                this.DataList.Add(data);
            }

            string errorMessage;
            Bitmap bitmap = ImageUtil.OpenBitmap(fullbinname, null, out errorMessage);
            if (bitmap == null)
            {
                return false;
            }
            if (bitmap.Width < 8 || bitmap.Height < 8)
            {
                bitmap.Dispose();
                return false;
            }

            if (orignalIine.IndexOf("--palette-only") >= 0)
            {//パレットのみ
                byte[] palette = ImageUtil.ImageToPalette(bitmap, 1);

                Data data = new Data(filename, this.Dir, palette, dataType);
                this.DataList.Add(data);
            }
            else
            {
                byte[] image = ImageUtil.ImageToByte16Tile(bitmap, bitmap.Width, bitmap.Height);

                Data data = new Data(filename, this.Dir, image, dataType);
                this.DataList.Add(data);
            }

            bitmap.Dispose();

            return true;
        }
        byte[] Png2DmpLZ77(string filename)
        {
            string hint = filename + ".dmp";
            if (File.Exists(hint))
            {
                return File.ReadAllBytes(hint);
            }
            //ヒントファイルがない!!
            Debug.Assert(false);
            return new byte[] { };
        }

        bool ParseORG(string line)
        {
            string a = Keyword(line , "ORG");
            if (a == "")
            {
                return false;
            }
            uint addr = U.atoi0x(a);
            if (addr <= 0)
            {
                return false;
            }
            addr = U.toOffset(addr);
            if (!U.isSafetyOffset(addr))
            {
                return false;
            }

            Data data = new Data(addr, DataEnum.ORG);
            this.DataList.Add(data);
            return true;
        }

        string Keyword(string line,string keyword)
        {
            int pos = line.IndexOf(keyword + " ", StringComparison.OrdinalIgnoreCase);
            if (pos < 0)
            {
                pos = line.IndexOf(keyword + "\t", StringComparison.OrdinalIgnoreCase);
                if (pos < 0)
                {
                    return "";
                }
            }
            string r = line.Substring(pos + keyword.Length + 1).Trim();
            return r;
        }

        public static bool IsFBGTemp(string filename)
        {
            string file = Path.GetFileName(filename);
            return (file.IndexOf("_FBG_Temp_") == 0);
        }

        static bool IsInjectHackInstallation(string target_text)
        {
            if (target_text.IndexOf("jumpToHack") < 0)
            {
                return false;
            }
            if (target_text.IndexOf("Hack Installation.txt") < 0)
            {
                return true;
            }
            
            return false;
        }

        public static string MakeEAAutoDef(string target_filename, uint freearea, uint org_sp, bool isColorzCore)
        {
            StringBuilder sb = new StringBuilder();

            string target_text = File.ReadAllText(target_filename);
            if (IsInjectHackInstallation(target_text))
            {
                sb.AppendLine("#include \"Extensions/Hack Installation.txt\"");
            }

            if (freearea == U.NOT_FOUND && org_sp == U.NOT_FOUND)
            {
                sb.AppendLine(String.Format("#include \"{0}\"\r\n"
                    , Path.GetFileName(target_filename)));
                return sb.ToString();
            }

            EAUtil ea = new EAUtil(target_filename);
            for (int i = 0; i < ea.IfNDefList.Count; i++)
            {
                string ifndef_keyword = ea.IfNDefList[i];
                switch (ifndef_keyword)
                {
                    case "FreeSpace":
                        if (freearea != 0)
                        {
                            sb.AppendLine("#define FreeSpace " + U.To0xHexString(freearea));
                        }
                        break;
                }
            }

            sb.AppendLine("#define ItemImage "
                + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.icon_pointer())));
            sb.AppendLine("#define ItemPalette "
                + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.icon_palette_pointer())));
            sb.AppendLine("#define ItemTable "
                + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.item_pointer())));
            sb.AppendLine("#define TextTable "
                + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.text_pointer())));
            sb.AppendLine("#define PortraitTable "
                + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.face_pointer())));
            if (Program.ROM.RomInfo.version() == 8)
            {
                sb.AppendLine("#define SummonUnitTable "
                    + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.summon_unit_pointer())));
                if (PatchUtil.SearchSkillSystem() == PatchUtil.skill_system_enum.SkillSystem)
                {
                    SkillConfigSkillSystemForm.Export(sb, isColorzCore);
                }
            }
            sb.AppendLine("#define AI1Table "
                + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.ai1_pointer())));
            sb.AppendLine("#define AI2Table "
                + U.To0xHexString(Program.ROM.p32(Program.ROM.RomInfo.ai2_pointer())));
            

            UnitActionPointerForm.SupportActionRework(sb);
            if (org_sp != U.NOT_FOUND)
            {
                sb.AppendLine("#define FEBUILDER_EXTRA_ORG " + U.To0xHexString(org_sp));
            }

            Program.ExportFunction.ExportEA(sb, isColorzCore);

            if (freearea != 0)
            {
                sb.AppendLine(String.Format("ORG {0}\r\n#include \"{1}\"\r\n"
                    , U.To0xHexString(freearea), target_filename));
            }
            else
            {
                sb.AppendLine(String.Format("#include \"{0}\"\r\n"
                    , Path.GetFileName(target_filename)));
            }
            return sb.ToString();
        }
        uint ParseAdd(string orignalIine)
        {
            int hint_pos = orignalIine.IndexOf("HINT=");
            if (hint_pos < 0)
            {
                return 0;
            }
            int add_pos = orignalIine.IndexOf("ADD=", hint_pos + 5);
            if (add_pos < 0)
            {
                return 0;
            }
            return U.atoi(orignalIine.Substring(add_pos + 4));
        }
    }
}
