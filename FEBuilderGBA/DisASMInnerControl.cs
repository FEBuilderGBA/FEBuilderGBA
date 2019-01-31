using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    public partial class DisASMInnerControl : UserControl
    {
        //自動的に長さを求めた場合 true / 手動で長さを求めている場合は true
        bool IsLengthAutoChecked = false;


        public DisASMInnerControl()
        {
            InitializeComponent();
        }


        private void ReloadListButton_Click(object sender, EventArgs e)
        {
            uint addr = (uint)ReadStartAddress.Value;
            if (U.isPointer(addr))
            {
                addr = U.toOffset(addr);
                U.ForceUpdate(ReadStartAddress, addr);
            }
            if (DisassemblerTrumb.ProgramAddrToPlain(addr) != addr)
            {
                addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
                U.ForceUpdate(ReadStartAddress, addr);
            }
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            uint addr_1 = addr;

            //変更通知.
            if (Navigation != null)
            {
                NavigationEventArgs arg = new NavigationEventArgs();
                arg.Address = addr;
                Navigation(this, arg);
            }

            DisassemblerTrumb Disassembler = new DisassemblerTrumb(Program.AsmMapFileAsmCache.GetAsmMapFile());

            uint length = (uint)Program.ROM.Data.Length;
            if (ReadCount.Value == 0 || this.IsLengthAutoChecked)
            {
                List<uint> ldrbuffer = new List<uint>();
                U.ForceUpdate(ReadCount, DisassemblerTrumb.CalcLength(Program.ROM.Data
                    , addr, length, ldrbuffer));
                //自動的に長さを求めました
                this.IsLengthAutoChecked = true;
            }
            uint bytecount = (uint)ReadCount.Value;


            this.AddressList.Items.Clear();
            this.AddressList.BeginUpdate();

            uint limit = Math.Min(addr + bytecount,length);

            int jisage = 0;  //字下げする数
            string jisageSpaceData = "";  //字下げに利用するマージンデータ
            List<uint> jmplabel = new List<uint>();  //ジャンプラベル　字下げに使う
            Dictionary<uint, uint> ldrtable = new Dictionary<uint, uint>();  //LDR参照データがある位置を記録します. コードの末尾などにあります. 数が多くなるのでマップする.
            AsmMapFile.MakeSwitchDataList(ldrtable, addr, limit);

            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            while (addr < limit)
            {
                if (ldrtable.ContainsKey(addr))
                {//LDR参照のポインタデータが入っている
                    uint ldr = ldrtable[addr];
                    if (ldr == U.NOT_FOUND)
                    {//switch case
                        this.AddressList.Items.Add(U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, 4) + "   //SWITCH CASE");
                    }
                    else
                    {
                        this.AddressList.Items.Add(U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, 4) + "   //LDRDATA");
                    }
                    addr += 4;
                    continue;
                }

                //Disassembler
                DisassemblerTrumb.Code code =
                    Disassembler.Disassembler(Program.ROM.Data, addr, length, vm);
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

                this.AddressList.Items.Add(jisageSpaceData + U.toPointer(addr).ToString("X08") + " " + U.MakeOPData(addr, code.GetLength()) + "   " + code.ASM + code.Comment);

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
                if (code.Type == DisassemblerTrumb.CodeType.LDR)
                {//LDR参照位置を記録していく.
                    ldrtable[code.Data2] = addr;
                }

                addr += code.GetLength();
            }
            this.AddressList.EndUpdate();

            U.SelectedIndexSafety(this.AddressList, 0, true);

            // 若设置了反编译器则自动反编译
            string retdec = OptionForm.GetRetDec(); ;
            if (!String.IsNullOrEmpty(retdec))
            {
                string retdec_option = OptionForm.GetRetDecOption();
                //FIXME get output file from options
                string output_c = Path.ChangeExtension(Program.ROM.Filename, ".c");
                // 双引号中可以包含空格，区分绝对路径和相对路径，参数解析太麻烦了，所以先假定没有设置输出文件
                // MatchCollection mc = Regex.Matches(retdec_option, @"-o(utput)?\s+\S+\s");
                retdec_option += " --raw-entry-point " + "0x" + (addr_1 + 0x8000000).ToString("x") + " --select-ranges " + "0x" + (addr_1 + 0x8000000).ToString("x") +  "-" + "0x" + (limit + 0x8000000).ToString("x");
                string args = "\"" + retdec + "\" " + retdec_option + " -o \"" + output_c + "\" \"" + Program.ROM.Filename + "\"";
                Log.Debug("Decompile: ", addr_1.ToString("x"), bytecount.ToString());
                Log.Debug(args);
                string output = MainFormUtil.ProgramRunAsAndEndWait("python3", args);
                Log.Debug(output);
                // 弹窗显示反编译出的C伪代码
                DecompileResult newForm = new DecompileResult();
                newForm.ShowDialog();
            }
        }

        private void DisASMForm_Load(object sender, EventArgs e)
        {
            MakeEditListboxContextMenu(this.AddressList, this.AddressList_KeyDown);

            InputFormRef.markupJumpLabel(ParamLabel1);
            ReadStartAddress.Focus();
        }

        public void JumpTo(uint addr)
        {
            addr = U.toOffset(addr);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            U.ForceUpdate(this.ReadStartAddress, addr);
            //自動的に長さを求めたということにして再取得すれば自動的に長さを再計算する.
            this.IsLengthAutoChecked = true;
            this.ReloadListButton.PerformClick();
        }

        private void ToClipBordButton_Click(object sender, EventArgs e)
        {
            string data = U.ConvertListBoxAllItemsToText(AddressList);
            U.SetClipboardText(data);
        }

        private void ToFileButton_Click(object sender, EventArgs e)
        {
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
            Program.LastSelectedFilename.Save(this, "", save);

            using (StreamWriter writer = new StreamWriter(save.FileNames[0]))
            {
                for (int i = 0; i < AddressList.Items.Count; i++)
                {
                    writer.WriteLine(AddressList.Items[i]);
                }
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileNames[0]);
        }

        private void ReadStartAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ReloadListButton.PerformClick();
            }
        }

        private void ReadCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ReloadListButton.PerformClick();
            }
            else
            {//何かやっているので手動で値を求めたとする.
                this.IsLengthAutoChecked = false;
            }
        }

        private void ReadCount_MouseDown(object sender, MouseEventArgs e)
        {
            //何かやっているので手動で値を求めたとする.
            this.IsLengthAutoChecked = false;
        }

        private void HexEditorButton_Click(object sender, EventArgs e)
        {
            uint start;
            if (AddressList.SelectedIndex >= 0)
            {
                start = U.atoh(AddressList.Text.Trim());
            }
            else
            {
                start = (uint)ReadStartAddress.Value;
            }
            HexEditorForm f = (HexEditorForm)InputFormRef.JumpForm<HexEditorForm>();
            f.JumpTo(start);
        }







        private void DumpAllButton_Click(object sender, EventArgs e)
        {
            DisASMDumpAllForm f = (DisASMDumpAllForm)InputFormRef.JumpFormLow<DisASMDumpAllForm>();
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            if (f.GetCallFunc() == DisASMDumpAllForm.Func.Func_DisASM)
            {
                DisASMDumpAllForm.RunAllDisASMButton(this.ParentForm);
            }
            else if (f.GetCallFunc() == DisASMDumpAllForm.Func.Func_IDAMAP)
            {
                DisASMDumpAllForm.RunAllMakeMAPFileButton(this.ParentForm);
            }
            else if (f.GetCallFunc() == DisASMDumpAllForm.Func.Func_NODOLLSYM)
            {
                DisASMDumpAllForm.RunAllMakeNoDollSymFileButton(this.ParentForm);
            }
        }
        public void DisASMForm_Resize(object sender, EventArgs e)
        {
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        public void HideFloatingControlpanel()
        {
            ControlPanel.Hide();
        }


        private void AddressList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowFloatingControlpanel();
                return;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
            }
            else if (e.Control && e.KeyCode == Keys.J)
            {
                ParamLabel1_Click(sender,e);
                return;
            }
            else if (e.Alt && e.KeyCode == Keys.J)
            {
                ParamLabel1_Click(sender, e);
                return;
            }
        }


        ContextMenu MakeEditListboxContextMenu(ListBox listbox, KeyEventHandler keydownfunc)
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;
            menuItem = new MenuItem(R._("ジャンプ(&J)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.J));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("詳細(ENTER)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.Enter));
            contextMenu.MenuItems.Add(menuItem);

            listbox.ContextMenu = contextMenu;
            return contextMenu;
        }

        private void AddressList_DoubleClick(object sender, EventArgs e)
        {
            ShowFloatingControlpanel();
        }

        void ShowFloatingControlpanel()
        {
            int y;
            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.AddressList.Items.Count)
            {//一件もない
                y = 0;
                return;
            }
            else
            {
                //編集する項目の近くに移動させます.
                Rectangle rect = this.AddressList.GetItemRectangle(index);
                y = this.MainPanel.Location.Y
                    + this.AddressList.Location.Y
                    + rect.Y + rect.Height + 20
                    ;
                if (y + ControlPanel.Height >= AddressList.Height)
                {//下に余白がないので上に出す.
                    y = this.MainPanel.Location.Y
                        + this.AddressList.Location.Y
                        + rect.Y
                        - ControlPanel.Height - 20;
                    if (y < 0)
                    {//上にも余白がないので、 Y = 0 の位置に出す
                        y = 0;
                    }
                }
            }
            //変更ボタンが光っていたら、それをやめさせる.
//            InputFormRef.WriteButtonToYellow(this.UpdateButton, false);
//            InputFormRef.WriteButtonToYellow(this.NewButton, false);

            ControlPanel.Location = new Point(ControlPanel.Location.X, y);
            ControlPanel.Show();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();

            string code = this.AddressList.Text;
            ScriptCodeName.Text = code;
            int pos;
            string jumpCode = " BL 0x";
            pos = code.IndexOf(jumpCode);
            if (pos < 0)
            {
                jumpCode = " B 0x";
                pos = code.IndexOf(jumpCode);
            }
            if (pos > 0)
            {
                pos += jumpCode.Length;
                ParamSrc1.Value = U.atoh(code.Substring(pos));

                ParamLabel1.Show();
                ParamSrc1.Show();
                ParamExplain1.Show();
            }
            else
            {
                ParamSrc1.Value = 0;
                ParamLabel1.Hide();
                ParamSrc1.Hide();
                ParamExplain1.Hide();
            }
        }

        private void ParamLabel1_Click(object sender, EventArgs e)
        {
            uint addr = (uint)ParamSrc1.Value;
            addr = U.toOffset(addr);
            addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
            if (! U.isSafetyOffset(addr))
            {
                return;
            }
            HideFloatingControlpanel();

            if (Navigation != null)
            {
                NavigationEventArgs arg = new NavigationEventArgs();
                arg.IsNewTab = true;
                arg.Address = addr;
                Navigation(this, arg);
            }
        }

        public class NavigationEventArgs : EventArgs
        {
            public bool IsNewTab; //新しいタブを開いてほしい時
            public uint Address;  //表示するアドレス
        };

        //変更があったとき
        public EventHandler Navigation;

        public void SetFocus()
        {
            if (this.AddressList.Items.Count == 0)
            {//件数が0件ならアドレス
                this.ReadStartAddress.Focus();
            }
            else
            {//件数があるならリスト
                this.AddressList.Focus();
            }
        }

        private void FileToASMButton_Click(object sender, EventArgs e)
        {
            string code = this.AddressList.Text;
            uint addr = U.atoh(code);

            ToolASMInsertForm f = (ToolASMInsertForm)InputFormRef.JumpForm<ToolASMInsertForm>();
            f.InitByHook(addr);
            f.FormClosed += (s, ee) =>
                {
                    if (this.IsDisposed)
                    {
                        return;
                    }
                    this.ReloadListButton.PerformClick();
                };
        }

        private void ReadCount_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}