using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace FEBuilderGBA
{
    public partial class DumpStructSelectDialogForm : Form
    {
        public DumpStructSelectDialogForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }
        void Init(uint addr)
        {
            this.ValueTextBox.Text = U.ToHexString(addr);
        }

        public enum Func
        {
             Func_Cancel
            ,Func_Binary
            ,Func_CSV
            ,Func_TSV
            ,Func_STRUCT
            ,Func_EA
            ,Func_NMM
            ,Func_Clipbord_Pointer
            ,Func_Clipbord_Copy
            ,Func_Clipbord_LittleEndian
            ,Func_Clipbord_NoDollBreakPoint
            ,Func_Import
        };
        Func CallFunc = Func.Func_Cancel;
        public Func GetCallFunc()
        {
            return this.CallFunc;
        }

        private void BinaryButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_Binary;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void EAALLButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_EA;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CSVButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_CSV;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void TSVButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_TSV;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void STRUCTButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_STRUCT;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_Cancel;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        class NameMapping : System.IComparable
        {
            public uint Id;
            public NumericUpDown Value;
            public Control Label;
            public Control Jump;
            public string TypeName;

            public int CompareTo(object obj)
            {
                if (obj == null || this.GetType() != obj.GetType())
                {
                    return 1;
                }

                if (((NameMapping)obj).Id == this.Id)
                {
                    return 0;
                }
                if (((NameMapping)obj).Id > this.Id)
                {
                    return -1;
                }
                return 0;
            }
            public string LabelText()
            {
                if (Label == null) return "";
                return U.nl2space(Label.Text);
            }
            public string LabelName()
            {
                if (Label == null) return "";
                return U.nl2space(Label.Name);
            }
            public string JumpText()
            {
                if (Jump == null) return "";
                return U.nl2space(Jump.Text);
            }
            public string JumpName()
            {
                if (Jump == null) return "";
                return U.nl2space(Jump.Name);
            }
            
            public string GetName()
            {
                string name = this.JumpText();
                if (IsBadName(name))
                {
                    string labelName = LabelName();
                    if (labelName.IndexOf("_WEAPON") >= 0)
                    {
                        if (Program.ROM.RomInfo.version() >= 7)
                        {
                            for (uint i = 0; i < 8; i++)
                            {
                                if (labelName == R._("L_{0}_WEAPON", 20 + i) || labelName == R._("L_{0}_WEAPON", 44 + i))
                                {
                                    name = InputFormRef.GetWeaponTypeName(i);
                                }
                            }
                        }
                        else
                        {
                            for (uint i = 0; i < 8; i++)
                            {
                                if (labelName == R._("L_{0}_WEAPON", 20 + i) || labelName == R._("L_{0}_WEAPON", 40 + i))
                                {
                                    name = InputFormRef.GetWeaponTypeName(i);
                                }
                            }
                        }
                    }
                }
                if (IsBadName(name))
                {
                    name = JumpText();
                }
                if (IsBadName(name))
                {
                    name = LabelText();
                }
                if (IsBadName(name))
                {
                    name = U.nl2space(Value.Name);
                }

                return name;
            }
            bool IsBadName(string name)
            {
                name = name.Trim();
                return (name == "" || name == "-" || name == "S" || name == "A" || name == "B" || name == "C" || name == "D" || name == "E");
            }
        };
        static List<NameMapping> MakeStruct(InputFormRef ifr, List<Control> controls)
        {
            List<String> unionPrefList = ifr.UnionPrefixList;
            List<NameMapping> dic = new List<NameMapping>();

            foreach (Control info in controls)
            {
                if (!(info is NumericUpDown))
                {
                    continue;
                }
                string prefix = ifr.Prefix;
                NumericUpDown info_object = ((NumericUpDown)info);

                String name = InputFormRef.SkipPrefixName(info.Name, prefix);
                if (name.Length <= 0)
                {
                    continue;
                }
                if (! InputFormRef.IsTypeWord(name[0]))
                {
                    if (ifr.UnionTab != null)
                    {
                        prefix = InputFormRef.ConvertUniTabPageToSelectedUnionPrefixName(ifr.UnionTab);
                        name = InputFormRef.SkipPrefixName(info.Name, prefix);
                        if (name.Length <= 0)
                        {
                            continue;
                        }
                    }

                    if (! InputFormRef.IsTypeWord(name[0]))
                    {
                        continue;
                    }
                }

                NameMapping nm = new NameMapping();
                nm.TypeName = name;
                nm.Id = U.atoi(name.Substring(1));
                nm.Value = info_object;


                uint id = U.atoi(name.Substring(1));
                string labelname = prefix + "L_" + id + "_";
                nm.Label = InputFormRef.FindObjectByFormFirstMatch<Control>(controls, labelname);

                string jumpname = prefix + "J_" + id + "_";
                nm.Jump = InputFormRef.FindObjectByFormFirstMatch<Control>(controls, jumpname);
                if (nm.Jump == null)
                {
                    jumpname = prefix + "J_" + id;
                    nm.Jump = InputFormRef.FindObjectByForm<Control>(controls, jumpname);
                }

                dic.Add(nm);

            }

            dic.Sort();

            return dic;
        }

        public string MakeStructString(InputFormRef ifr)
        {
            List<Control> controls = InputFormRef.GetAllControls(ifr.SelfForm);
            ListBox listbox = ifr.AddressList;
            List<NameMapping> dic = MakeStruct( ifr, controls);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("struct " + ifr.SelfForm.Name + " {" + "//" + ifr.SelfForm.Text);

            foreach (NameMapping nm in dic)
            {
                string comment = ";//" + nm.GetName();

                if (nm.TypeName[0] == 'b')
                {
                    sb.AppendLine("sbyte    _" + nm.Id + comment);
                }
                else if (nm.TypeName[0] == 'l')
                {
                    sb.AppendLine("byte    _" + nm.Id + comment);
                }
                else if (nm.TypeName[0] == 'h')
                {
                }
                else if (nm.TypeName[0] == 'B')
                {
                    sb.AppendLine("byte    _" + nm.Id + comment);
                }
                else if (nm.TypeName[0] == 'W')
                {
                    sb.AppendLine("ushort   _" + nm.Id + comment);
                }
                else if (nm.TypeName[0] == 'D')
                {
                    sb.AppendLine("dword   _" + nm.Id + comment);
                }
                else if (nm.TypeName[0] == 'P')
                {
                    sb.AppendLine("void*   _" + nm.Id + comment);
                }
            }
            sb.AppendLine("}; sizeof(" + ifr.BlockSize +")");

            return sb.ToString();
        }

        string MakeTSVString(InputFormRef ifr,bool isCSV)
        {
            List<Control> controls = InputFormRef.GetAllControls(ifr.SelfForm);
            ListBox listbox = ifr.AddressList;
            List<NameMapping> dic = MakeStruct(ifr, controls);

            StringBuilder sb = new StringBuilder();

            sb.Append(U.To0xHexString(ifr.BaseAddress));
            foreach (NameMapping nm in dic)
            {
                string comment = nm.GetName();
                if (isCSV)
                {
                    sb.Append(",");
                    comment = U.EncloseDoubleQuotesIfNeed(comment);
                }
                else
                {
                    sb.Append("\t");
                }

                if (nm.TypeName[0] == 'b')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'l')
                {
                    sb.AppendLine(comment);
                }
                else if (nm.TypeName[0] == 'h')
                {
                }
                else if (nm.TypeName[0] == 'B')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'W')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'D')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'P')
                {
                    sb.Append(comment);
                }
            }

            U.AddrResult[] addr_array;
            addr_array = new U.AddrResult[ifr.DataCount];
            for (int i = 0; i < ifr.DataCount; i++)
            {
                addr_array[i] = InputFormRef.SelectToAddrResult(listbox, i);
            }
            sb.AppendLine("");

            for (int i = 0; i < addr_array.Length; i++)
            {
                U.AddrResult ar = addr_array[i];
                if (ar.isNULL())
                {
                    continue;
                }

                if (isCSV)
                {
                    sb.Append(U.EncloseDoubleQuotesIfNeed(ar.name));
                }
                else
                {
                    sb.Append(ar.name);
                }
                uint addr = ar.addr;
                foreach (NameMapping nm in dic)
                {
                    if (isCSV)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        sb.Append("\t");
                    }

                    if (nm.TypeName[0] == 'b')
                    {
                        uint v = Program.ROM.u8(addr + nm.Id);
                        sb.Append(U.To0xHexString(v));
                    }
                    else if (nm.TypeName[0] == 'l')
                    {
                        uint v = Program.ROM.u8(addr + nm.Id);
                        sb.Append(U.To0xHexString(v));
                    }
                    else if (nm.TypeName[0] == 'h')
                    {
                    }
                    else if (nm.TypeName[0] == 'B')
                    {
                        uint v = Program.ROM.u8(addr + nm.Id);
                        sb.Append(U.To0xHexString(v));
                    }
                    else if (nm.TypeName[0] == 'W')
                    {
                        uint v = Program.ROM.u16(addr + nm.Id);
                        sb.Append(U.To0xHexString(v));
                    }
                    else if (nm.TypeName[0] == 'D')
                    {
                        uint v = Program.ROM.u32(addr + nm.Id);
                        sb.Append(U.To0xHexString(v));
                    }
                    else if (nm.TypeName[0] == 'P')
                    {
                        uint v = Program.ROM.p32(addr + nm.Id);
                        sb.Append(U.To0xHexString(v));
                    }
                }
                sb.AppendLine("");
            }
            return sb.ToString();
        }

        public static void ImportTSV(InputFormRef ifr, bool isCSV, string filename, Undo.UndoData undodata)
        {
            List<Control> controls = InputFormRef.GetAllControls(ifr.SelfForm);
            ListBox listbox = ifr.AddressList;
            List<NameMapping> dic = MakeStruct(ifr, controls);

            uint addr = ifr.BaseAddress;
            string[] lines = File.ReadAllLines(filename);
            for (int i = 1; i < lines.Length; i++, addr += ifr.BlockSize)
            {
                if (i > ifr.DataCount)
                {
                    break;
                }
                string line = lines[i];

                uint[] array;
                if (isCSV)
                {
                    array = U.ParseTSVLine(line, true, ',');
                }
                else
                {
                    array = U.ParseTSVLine(line,true);
                }

                for( int n = 0; n < dic.Count ; n ++)
                {
                    if (n >= array.Length)
                    {
                        break;
                    }
                    NameMapping nm = dic[n];

                    uint v = array[n];

                    if (nm.TypeName[0] == 'b')
                    {
                        Program.ROM.write_u8(addr + nm.Id , v , undodata);
                    }
                    else if (nm.TypeName[0] == 'l')
                    {
                        Program.ROM.write_u8(addr + nm.Id, v, undodata);
                    }
                    else if (nm.TypeName[0] == 'h')
                    {
                        continue;
                    }
                    else if (nm.TypeName[0] == 'B')
                    {
                        Program.ROM.write_u8(addr + nm.Id, v, undodata);
                    }
                    else if (nm.TypeName[0] == 'W')
                    {
                        Program.ROM.write_u16(addr + nm.Id, v, undodata);
                    }
                    else if (nm.TypeName[0] == 'D')
                    {
                        Program.ROM.write_u32(addr + nm.Id, v, undodata);
                    }
                    else if (nm.TypeName[0] == 'P')
                    {
                        Program.ROM.write_p32(addr + nm.Id, v, undodata);
                    }
                }

            }
        }

        string MakEA2String(InputFormRef ifr, uint target)
        {
            List<Control> controls = InputFormRef.GetAllControls(ifr.SelfForm);
            ListBox listbox = ifr.AddressList;
            List<NameMapping> dic = MakeStruct(ifr, controls);

            StringBuilder sb = new StringBuilder();

            sb.Append("//");
            foreach (NameMapping nm in dic)
            {
                string comment = nm.GetName();

                sb.Append("\t");
                if (nm.TypeName[0] == 'b')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'l')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'h')
                {
                }
                else if (nm.TypeName[0] == 'B')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'W')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'D')
                {
                    sb.Append(comment);
                }
                else if (nm.TypeName[0] == 'P')
                {
                    sb.Append(comment);
                }
            }

            U.AddrResult[] addr_array;
            if (target == U.NOT_FOUND)
            {
                addr_array = new U.AddrResult[ifr.DataCount];
                for (int i = 0; i < ifr.DataCount; i++)
                {
                    addr_array[i] = InputFormRef.SelectToAddrResult(listbox, i);
                }
            }
            else
            {
                addr_array = new U.AddrResult[] { InputFormRef.SelectToAddrResult(listbox, (int)target) };
            }
            sb.AppendLine("");
            sb.AppendLine("PUSH");
            sb.AppendLine("ORG " + U.To0xHexString(addr_array[0].addr));
            string lastData = "";

            for (int i = 0; i < addr_array.Length; i++)
            {
                string line = "";
                U.AddrResult ar = addr_array[i];
                if (ar.isNULL())
                {
                    continue;
                }
                uint addr = ar.addr;
                foreach (NameMapping nm in dic)
                {
                    if (nm.TypeName[0] == 'b')
                    {
                        if (lastData != "BYTE")
                        {
                            lastData = "BYTE";
                            line += ";" + lastData;
                        }
                        line += " ";
                        uint v = Program.ROM.u8(addr + nm.Id);
                        if (nm.Value.Hexadecimal)
                        {
                            line += U.To0xHexString(v);
                        }
                        else
                        {
                            line += (sbyte)v;
                        }
                    }
                    else if (nm.TypeName[0] == 'l')
                    {
                        if (lastData != "BYTE")
                        {
                            lastData = "BYTE";
                            line += ";" + lastData;
                        }
                        line += " ";
                        uint v = Program.ROM.u8(addr + nm.Id);
                        if (nm.Value.Hexadecimal)
                        {
                            line += U.To0xHexString(v);
                        }
                        else
                        {
                            line += (sbyte)v;
                        }
                    }
                    else if (nm.TypeName[0] == 'h')
                    {
                    }
                    else if (nm.TypeName[0] == 'B')
                    {
                        if (lastData != "BYTE")
                        {
                            lastData = "BYTE";
                            line += ";" + lastData;
                        }
                        line += " ";

                        uint v = Program.ROM.u8(addr + nm.Id);
                        if (nm.Value.Hexadecimal)
                        {
                            line += U.To0xHexString(v);
                        }
                        else
                        {
                            line += v;
                        }
                    }
                    else if (nm.TypeName[0] == 'W')
                    {
                        if (lastData != "SHORT")
                        {
                            lastData = "SHORT";
                            line += ";" + lastData;
                        }
                        line += " ";

                        uint v = Program.ROM.u16(addr + nm.Id);
                        if (nm.Value.Hexadecimal)
                        {
                            line += U.To0xHexString(v);
                        }
                        else
                        {
                            line += v;
                        }
                    }
                    else if (nm.TypeName[0] == 'D')
                    {
                        if (lastData != "WORD")
                        {
                            lastData = "WORD";
                            line += ";" + lastData;
                        }
                        line += " ";

                        uint v = Program.ROM.u32(addr + nm.Id);
                        if (nm.Value.Hexadecimal)
                        {
                            line += U.To0xHexString(v);
                        }
                        else
                        {
                            line += v;
                        }
                    }
                    else if (nm.TypeName[0] == 'P')
                    {
                        if (lastData != "POIN")
                        {
                            lastData = "POIN";
                            line += ";" + lastData;
                        }
                        line += " ";

                        uint v = Program.ROM.p32(addr + nm.Id);
                        if (nm.Value.Hexadecimal)
                        {
                            line += U.To0xHexString(v);
                        }
                        else
                        {
                            line += v;
                        }
                    }
                }
                line = line + " " + "//" + ar.name;
                lastData = "";
                sb.AppendLine(U.substr(line, 1));
            }
            sb.AppendLine("POP");
            return sb.ToString();
        }
        public string MakeEAString(InputFormRef ifr)
        {
            return MakEA2String(ifr, U.NOT_FOUND);
        }
        string EAType(string formName)
        {
            if (formName.IndexOf("EventUnit") >= 0)
            {
                return "unit";
            }
            if (formName.IndexOf("ItemShop") >= 0)
            {
                return "shopList";
            }
            return "none";
        }
        public string MakNMMString(InputFormRef ifr, string basefilename, Dictionary<string, string> addFiles)
        {
            List<Control> controls = InputFormRef.GetAllControls(ifr.SelfForm);
            ListBox listbox = ifr.AddressList;
            List<NameMapping> dic = MakeStruct(ifr, controls);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("1");
            sb.AppendLine(ifr.SelfForm.Text + " by FEBuilderGBA");
            sb.AppendLine(U.To0xHexString(ifr.BaseAddress));
            sb.AppendLine(ifr.DataCount.ToString());
            sb.AppendLine(ifr.BlockSize.ToString());
            {
                string dropDownFilename = MakeNMMDropDownList(ifr, basefilename, addFiles, controls, "INDEX", 0);
                sb.AppendLine(dropDownFilename);
            }
            sb.AppendLine("NULL");
            sb.AppendLine("");

            foreach (NameMapping nm in dic)
            {
                string name = nm.GetName();
                sb.AppendLine(name);
                sb.AppendLine(nm.Id.ToString()); //index
                bool is_unsigned = true;

                if (nm.TypeName[0] == 'b')
                {
                    sb.AppendLine("1"); //size
                    is_unsigned = false;
                }
                else if (nm.TypeName[0] == 'l')
                {
                    sb.AppendLine("1"); //size
                    is_unsigned = false;
                }
                else if (nm.TypeName[0] == 'h')
                {
                    continue;
                }
                else if (nm.TypeName[0] == 'B')
                {
                    sb.AppendLine("1"); //size
                }
                else if (nm.TypeName[0] == 'W')
                {
                    sb.AppendLine("2"); //size
                }
                else if (nm.TypeName[0] == 'D')
                {
                    sb.AppendLine("4"); //size
                }
                else if (nm.TypeName[0] == 'P')
                {
                    sb.AppendLine("4"); //size
                }

                string[] args;
                string linktype = NameToArgs(nm.LabelName(), ifr.Prefix,out args);
                string dropDownFilename = MakeNMMDropDownList(ifr, basefilename, addFiles, controls, linktype, (int)nm.Id);

                string type = "N";
                if (dropDownFilename == "NULL")
                {
                    type += "E";
                }
                else
                {
                    type += "D";
                }

                if (nm.Value.Hexadecimal)
                {//16進数
                    type += "H";
                }
                else
                {//10進数
                    type += "D";
                }

                if (is_unsigned)
                {
                    type += "U";
                }
                else
                {
                    type += "S";
                }
                sb.AppendLine(type);
                sb.AppendLine(dropDownFilename);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        void ImportTSV(InputFormRef ifr)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return ;
            }

            string title = R._("開くファイル名を選択してください");
            string filter = R._("TSV形式|*.tsv|CSV形式|*.csv|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return ;
            }
            if (!U.CanReadFileRetry(open))
            {
                return ;
            }
            Program.LastSelectedFilename.Save(this, "", open);
            string filename = open.FileName;

            bool isCSV = false;
            if (U.GetFilenameExt(filename) == ".CSV")
            {
                isCSV = true;
            }

            if (! CheckImportData(ifr, filename, isCSV))
            {//ユーザキャンセル
                return;
            }
            
            ImportTSV(ifr, filename,isCSV);
            
            ifr.ReloadAddressList();
            R.ShowOK("データのインポートが完了しました。");

            return;
        }
        void ImportTSV(InputFormRef ifr, string filename, bool isCSV)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "ImportTSV");
            ImportTSV(ifr, isCSV, filename, undodata);
            Program.Undo.Push(undodata);
        }
        bool CheckImportData(InputFormRef ifr, string filename, bool isCSV)
        {
            List<Control> controls = InputFormRef.GetAllControls(ifr.SelfForm);
            ListBox listbox = ifr.AddressList;
            List<NameMapping> dic = MakeStruct(ifr, controls);

            uint addr = ifr.BaseAddress;
            string[] lines = File.ReadAllLines(filename);
            if (lines.Length > ifr.DataCount + 1)
            {
                DialogResult dr = R.ShowNoYes("件数が足りませんが、処理を続行しますか？\r\n現在{0}件しかテーブルを確保していませんが、インポートしようとしているファイルには、{1}件のデータがあります。\r\n\r\n処理を続行して、インポートできるところまで、インポートしますか？", ifr.DataCount, lines.Length - 1);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return false;
                }
            }

            for (int i = 1; i < lines.Length; i++, addr += ifr.BlockSize)
            {
                if (i > ifr.DataCount)
                {
                    break;
                }
                string line = lines[i];

                uint[] array;
                if (isCSV)
                {
                    array = U.ParseTSVLine(line, true, ',');
                }
                else
                {
                    array = U.ParseTSVLine(line, true);
                }

                if (dic.Count != array.Length)
                {
                    DialogResult dr = R.ShowNoYes("{0}行目のカラム数が想定の物と違いますが、処理を続行しますか？\r\nこの項目には、{1}件のデータカラム数が必要ですが、インポートしようとしているファイルには、{2}件のデータカラム数があります。\r\nこのファイルは正しいダンプファイルですか？\r\n\r\n処理を続行して、インポートできるところまで、インポートしますか？", i+1, dic.Count, array.Length);
                    return (dr == System.Windows.Forms.DialogResult.Yes);
                }
            }
            {
                DialogResult dr = R.ShowYesNo("ファイル({0})の内容をインポートしてもよろしいですか？", Path.GetFileName(filename));
                return (dr == System.Windows.Forms.DialogResult.Yes);
            }
        }

        string NameToArgs(string name,string prefix,out string[] out_args)
        {
            String link_name = InputFormRef.SkipPrefixName(name, prefix);

            string[] sp = link_name.Split('_');
            if (sp.Length <= 2)
            {
                out_args = new string[0];
                return "";
            }

            out_args = U.subrange(sp, 3, (uint)sp.Length);
            return sp[2];
        }


        string MakeNMMDropDownList(InputFormRef ifr,string basefilename,Dictionary<string,string> addFiles,List<Control> controls,string linktype,int num)
        {
            string filename = basefilename + linktype + ".txt";
            if (addFiles.ContainsKey(linktype))
            {
                return addFiles[filename];
            }

            string data = "";
            if (linktype == "INDEX")
            {
                data = MakeNMMDropDownListInner(ifr.MakeList());
            }
            else if (linktype == "PORTRAIT")
            {
                data = MakeNMMDropDownListInner(ImagePortraitForm.MakePortraitList());
            }
            else if (linktype == "UNIT")
            {
                List<U.AddrResult> list = UnitForm.MakeUnitList();
                data = MakeNMMDropDownListInner(list);
            }
            else if (linktype == "CLASS")
            {
                data = MakeNMMDropDownListInner(ClassForm.MakeClassList());
            }
            else if (linktype == "ITEM")
            {
                data = MakeNMMDropDownListInner(ItemForm.MakeItemList());
            }
            else if (linktype == "SONG")
            {
                data = MakeNMMDropDownListInner(SongTableForm.MakeItemList());
            }
            else if (linktype == "COMBO")
            {
                Control c = InputFormRef.FindObject(ifr.Prefix, controls, num + "_" + linktype);
                if (c is ComboBox)
                {
                    data = MakeNMMDropDownListInner((ComboBox)c);
                    data = string.Join("\r\n", U.ComboBoxToStringList((ComboBox)c));
                }
            }
            else if (linktype == "ATTRIBUTE")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("8");
                for (uint n = 0; n < 8; n++)
                {
                    sb.AppendLine(U.To0xHexString(n) + " " + InputFormRef.GetAttributeName(n));
                }
                data = sb.ToString();
            }
            else if (linktype == "WEAPON")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("7");
                sb.AppendLine("0x0 -");
                sb.AppendLine("0x1 E");
                sb.AppendLine("0x31 D");
                sb.AppendLine("0x71 C");
                sb.AppendLine("0x121 B");
                sb.AppendLine("0x181 A");
                sb.AppendLine("0x251 S");
                data = sb.ToString();
            }
            else if (linktype == "FLAG")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("128");
                string dummy;
                for (uint n = 0; n < 128; n++)
                {
                    sb.AppendLine(U.To0xHexString(n) + " " + InputFormRef.GetFlagName(n, out dummy));
                }
                data = sb.ToString();
            }
            else if (linktype == "BIT")
            {
                filename = basefilename + linktype + num + ".txt";
                if (addFiles.ContainsKey(linktype))
                {
                    return addFiles[filename];
                }

                data = MakeBitsListInner(ifr, controls, linktype, num);
            }

            if (data == "")
            {
                return "NULL";
            }
            addFiles[filename] = data;
            return filename;
        }

        string MakeBitsListInner(InputFormRef ifr, List<Control> controls, string linktype, int num)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("256");

            string[] bitnames = new string[] { "01", "02", "04", "08", "10", "20", "40", "80" };
            string[] bits = new string[8];
            for (uint n = 0; n < 8; n++)
            {
                string name = ifr.Prefix + "L_" + num + "_BIT_" + bitnames[n];

                Control c = InputFormRef.FindObject(ifr.Prefix, controls, name);
                if (c is CheckBox)
                {
                    bits[n] = c.Text;
                }
                else
                {
                    bits[n] = "";
                }
            }

            sb.AppendLine("0x00 None");
            for (uint n = 1; n < 256; n++)
            {
                string str = "";
                if ((n & 0x01) == 0x01)
                {
                    str += "," + bits[0];
                }
                if ((n & 0x02) == 0x02)
                {
                    str += "," + bits[1];
                }
                if ((n & 0x04) == 0x04)
                {
                    str += "," + bits[2];
                }
                if ((n & 0x08) == 0x08)
                {
                    str += "," + bits[3];
                }
                if ((n & 0x010) == 0x010)
                {
                    str += "," + bits[4];
                }
                if ((n & 0x020) == 0x020)
                {
                    str += "," + bits[5];
                }
                if ((n & 0x040) == 0x040)
                {
                    str += "," + bits[6];
                }
                if ((n & 0x080) == 0x080)
                {
                    str += "," + bits[7];
                }
                sb.AppendLine(U.To0xHexString(n) + " " + str.Substring(1));
            }
            return sb.ToString();
        }

        string MakeNMMDropDownListInner(List<U.AddrResult> list)
        {
            if (list.Count <= 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(list.Count.ToString());
            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendLine("0x" + list[i].name);
            }

            return sb.ToString();
        }
        string MakeNMMDropDownListInner(ComboBox c)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(c.Items.Count.ToString());
            for (int i = 0; i < c.Items.Count; i++)
            {
                sb.AppendLine(U.To0xHexString((uint)i) + " " + c.Items[i]);
            }

            return sb.ToString();
        }

        private void DumpStructSelectDialogForm_Load(object sender, EventArgs e)
        {

        }

        private void NMMButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_NMM;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CopyPointer_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_Clipbord_Pointer;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CopyClipboard_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_Clipbord_Copy;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CopyLittleEndian_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_Clipbord_LittleEndian;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CopyNoDollGBARadBreakPoint_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_Clipbord_NoDollBreakPoint;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public static void ShowDumpSelectDialog(InputFormRef ifr,string selectAddress)
        {
            uint addr = U.atoh(selectAddress);
            if (U.is_RAMPointer(addr))
            {//RAMの場合Dumpできないので、代わりにポインタツールの選択画面を開こう
                PointerToolCopyToForm ptcopyForm = (PointerToolCopyToForm)InputFormRef.JumpFormLow<PointerToolCopyToForm>();
                ptcopyForm.Init((uint)addr);
                ptcopyForm.ShowDialog();

                return;
            }
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            DumpStructSelectDialogForm f = (DumpStructSelectDialogForm)InputFormRef.JumpFormLow<DumpStructSelectDialogForm>();
            f.Init(addr);
            f.ShowDialog();


            DumpStructSelectDialogForm.Func ff = f.GetCallFunc();
            if (ff == DumpStructSelectDialogForm.Func.Func_Binary)
            {
                HexEditorForm hexeditor = (HexEditorForm)InputFormRef.JumpForm<HexEditorForm>(U.NOT_FOUND);
                hexeditor.JumpTo(addr);
                return;
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_Clipbord_Pointer)
            {
                U.SetClipboardText(U.ToHexString(U.toPointer(addr)));
                return;
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_Clipbord_Copy)
            {
                U.SetClipboardText(U.ToHexString(addr));
                return;
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_Clipbord_LittleEndian)
            {
                uint a = U.toPointer(addr);
                uint r = (((a & 0xFF) << 24)
                    + ((a & 0xFF00) << 8)
                    + ((a & 0xFF0000) >> 8)
                    + ((a & 0xFF000000) >> 24));

                U.SetClipboardText(U.ToHexString(r));
                return;
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_Clipbord_NoDollBreakPoint)
            {
                U.SetClipboardText("[" + U.ToHexString(U.toPointer(addr)) + "]?");
                return;
            }

            string text;
            string filename;
            Dictionary<string, string> addFiles = new Dictionary<string, string>();
            if (ff == DumpStructSelectDialogForm.Func.Func_STRUCT)
            {
                filename = ifr.SelfForm.Name + ifr.Prefix + ".h";
                text = f.MakeStructString(ifr);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_CSV)
            {
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".csv";
                text = f.MakeTSVString(ifr, true);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_TSV)
            {
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".tsv";
                text = f.MakeTSVString(ifr, false);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_EA)
            {
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".event";
                text = f.MakeEAString(ifr);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_NMM)
            {
                string basename = ifr.SelfForm.Name + "_" + ifr.Prefix;
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".nmm";
                text = f.MakNMMString(ifr, basename, addFiles);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_Import)
            {
                f.ImportTSV(ifr);
                return;
            }
            else
            {
                return;
            }

            DumpStructSelectToTextDialogForm showForm = (DumpStructSelectToTextDialogForm)InputFormRef.JumpFormLow<DumpStructSelectToTextDialogForm>();
            showForm.Init(filename, text, addFiles);
            showForm.ShowDialog();
        }
        public static string SaveDumpAutomatic(InputFormRef ifr
            , DumpStructSelectDialogForm.Func ff
            , string saveDir
            )
        {
            DumpStructSelectDialogForm f = (DumpStructSelectDialogForm)InputFormRef.JumpFormLow<DumpStructSelectDialogForm>();
            f.Init(ifr.BaseAddress);

            string text;
            string filename;
            Dictionary<string, string> addFiles = new Dictionary<string, string>();
            if (ff == DumpStructSelectDialogForm.Func.Func_CSV)
            {
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".csv";
                text = f.MakeTSVString(ifr, true);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_TSV)
            {
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".tsv";
                text = f.MakeTSVString(ifr, false);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_EA)
            {
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".event";
                text = f.MakeEAString(ifr);
            }
            else if (ff == DumpStructSelectDialogForm.Func.Func_NMM)
            {
                string basename = ifr.SelfForm.Name + "_" + ifr.Prefix;
                filename = ifr.SelfForm.Name + ifr.Prefix + "_" + U.ToHexString8(ifr.BaseAddress) + ".nmm";
                text = f.MakNMMString(ifr, basename, addFiles);
            }
            else
            {
                return "";
            }

            string fullfilename = Path.Combine(saveDir, filename);
            U.WriteAllText(fullfilename, text);
            return fullfilename;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            this.CallFunc = Func.Func_Import;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
