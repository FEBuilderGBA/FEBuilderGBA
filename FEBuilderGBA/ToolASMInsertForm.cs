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
    public partial class ToolASMInsertForm : Form
    {
        public ToolASMInsertForm()
        {
            InitializeComponent();
            SRCFilename.AllowDropFilename();
            AllowDropFilename();
            this.Method.SelectedIndex = 0;
            this.HookRegister.SelectedIndex = 3;
            this.ELFComboBox.SelectedIndex = 1;
            this.DebugSymbolComboBox.SelectedIndex = 3;
            U.ForceUpdate(FREEAREA, InputFormRef.AllocBinaryData(1024 * 1024 , isProgramArea: true)); //とりあえず1MBの空きがあるところ.
            this.ComplieBinFilename = "";

            SetExplain();
        }


        void AllowDropFilename()
        {
            U.AllowDropFilename(this
                , new string[] { ".ASM", ".S", ".TXT", ".c", ".cc", ".cpp", ".cxx", ".c++", "" }
                , (string filename) =>
                {
                    SRCFilename.Text = filename;
                    this.WriteButton.PerformClick();
                });
        }

        string ComplieBinFilename;

        private void SRCFilename_DoubleClick(object sender, EventArgs e)
        {
            SRCSelectButton.PerformClick();
        }

        private void ToolASMInsertForm_Load(object sender, EventArgs e)
        {
        }

        private void Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Method.SelectedIndex == 1)
            {
                AddressLabel.Text = R._("書き込む");
                AddressLabel.Show();
                Address.Show();
                HookRegisterLabel.Hide();
                HookRegister.Hide();
                FREEAREALabel.Hide();
                FREEAREA.Hide();
                PatchMakerButton.Show();
                DebugSymbol.Show();
                DebugSymbolComboBox.Show();
                ELFLabel.Hide();
                ELFComboBox.Hide();
                Address.Value = FREEAREA.Value;
            }
            else if (this.Method.SelectedIndex == 2)
            {
                AddressLabel.Text = R._("フックするアドレス");
                AddressLabel.Show();
                Address.Show();
                HookRegisterLabel.Show();
                HookRegister.Show();
                FREEAREALabel.Show();
                FREEAREA.Show();
                PatchMakerButton.Show();
                DebugSymbol.Show();
                DebugSymbolComboBox.Show();
                ELFLabel.Hide();
                ELFComboBox.Hide();
                Address.Value = 0;
            }
            else
            {
                AddressLabel.Hide();
                Address.Hide();
                HookRegisterLabel.Hide();
                HookRegister.Hide();
                FREEAREALabel.Hide();
                FREEAREA.Hide();
                PatchMakerButton.Hide();
                DebugSymbol.Hide();
                DebugSymbolComboBox.Hide();
                ELFLabel.Show();
                ELFComboBox.Show();
            }
        }

        public void InitByHook(uint Addr)
        {
            Address.Value = U.toOffset(Addr);
            this.Method.SelectedIndex = 2;
        }

        MainFormUtil.CompileType GetCompileType()
        {
            if (ELFComboBox.Visible)
            {
                if (this.ELFComboBox.SelectedIndex == 4)
                {
                    return MainFormUtil.CompileType.CONVERT_LYN;
                }
                if (this.ELFComboBox.SelectedIndex == 2 || ELFComboBox.SelectedIndex == 3)
                {
                    return MainFormUtil.CompileType.KEEP_ELF;
                }
            }
            return MainFormUtil.CompileType.NONE;
        }
        SymbolUtil.DebugSymbol GetPlanOfDebugSymbol()
        {
            if (ELFComboBox.Visible)
            {
                if (ELFComboBox.SelectedIndex == 1
                    || ELFComboBox.SelectedIndex == 3
                    || ELFComboBox.SelectedIndex == 4
                    )
                {
                    return SymbolUtil.DebugSymbol.SaveSymTxt;
                }
                return SymbolUtil.DebugSymbol.None;
            }
            return (SymbolUtil.DebugSymbol)DebugSymbolComboBox.SelectedIndex;
        }
        bool IsMakeLynEventMode()
        {
            return (this.ELFComboBox.SelectedIndex == 4);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            this.ComplieBinFilename = "";
            string fullpath = this.SRCFilename.Text;
            if (! File.Exists(fullpath))
            {
                R.ShowStopError(("ファイルがありません。\r\nファイル名:{0}"), fullpath);
                return;
            }

            if (IsMakeLynEventMode() && this.Method.SelectedIndex != 0)
            {
                R.ShowStopError("lyn.eventを作成する時は、ROMに書き込むことはできません。");
                return;
            }
            
            string result;
            string symbol;
            bool r = MainFormUtil.Compile(fullpath, out result, out symbol, GetCompileType());
            if (r == false)
            {
                R.ShowStopError("エラーが返されました。\r\n{0}", result);
                return;
            }

            this.ComplieBinFilename = result;
            if (this.Method.SelectedIndex == 0)
            {
                SymbolUtil.ProcessSymbolByComment(fullpath, symbol, GetPlanOfDebugSymbol(), 0);

                //ROMに書き込まない場合、成果物を選択しよう.
                U.SelectFileByExplorer(result,false);
                return;
            }

            uint addr = U.toOffset((uint)this.Address.Value);
            if (!U.CheckZeroAddressWrite(addr))
            {
                return;
            }
            if (!U.isSafetyOffset(addr))
            {
                if (addr != Program.ROM.Data.Length)
                {
                    R.ShowStopError("無効なポインタです。\r\nこの設定は危険です。");
                    return;
                }
            }

            if (! File.Exists(result))
            {
                R.ShowStopError("ファイルがありません。\r\nファイル名:{0}", result);
                return;
            }
            byte[] bin = File.ReadAllBytes(result);
            if (bin.Length <= 0)
            {
                R.ShowStopError("ファイルがゼロバイトです。\r\nファイル名:{0}", result);
                return;
            }
            if (bin.Length > 1024 * 10)
            {
                DialogResult dr = R.ShowNoYes("このファイルは {0}バイトも書き換えますが、本当によろしいですか？\r\n\r\n(ORG指定を間違って利用していませんか?)\r\nファイル名:{1}", bin.Length, result);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            if (this.Method.SelectedIndex == 1)
            {
                uint newlength = addr + (uint)bin.Length;
                if (!U.isSafetyOffset(newlength))
                {
                    Program.ROM.write_resize_data(newlength);
                }
                Program.ROM.write_range(addr, bin, undodata);
                Program.CommentCache.RemoveRange(addr, addr + (uint)bin.Length);
                SymbolUtil.ProcessSymbolByComment(fullpath, symbol, GetPlanOfDebugSymbol(), addr);
            }
            else
            {
                uint freeaddr = U.toOffset((uint)this.FREEAREA.Value);
                if (!U.CheckZeroAddressWrite(freeaddr))
                {
                    return;
                }
                //フックに利用する場所
                undodata.list.Add(new Undo.UndoPostion(addr, 2 * 4));

                uint endaddr = freeaddr + (uint)bin.Length;
                undodata.list.Add(new Undo.UndoPostion(freeaddr, (uint)bin.Length));

                if (endaddr > Program.ROM.Data.Length)
                {//長さが増える場合、ROMを増設する.
                    Program.ROM.write_resize_data(endaddr);
                }
                Program.ROM.write_range(freeaddr, bin);
                uint usereg = GetSaftyRegister();

                byte[] jumpCode = DisassemblerTrumb.MakeInjectJump(addr, freeaddr, usereg);
                Program.ROM.write_range(addr, jumpCode, undodata);

                Program.CommentCache.RemoveRange(freeaddr, freeaddr + (uint)bin.Length);
                SymbolUtil.ProcessSymbolByComment(fullpath, symbol, GetPlanOfDebugSymbol(), freeaddr);
            }
            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this, addr);

            this.UndoButton.Show();
        }

        uint GetSaftyRegister()
        {
            int usereg = this.HookRegister.SelectedIndex;
            if (usereg < 0) return 0;
            if (usereg > 8) return 8;

            return (uint)usereg;
        }

        private void PatchMakerButton_Click(object sender, EventArgs e)
        {
            string patch = MakePatch();
            if (patch == "")
            {
                return;
            }

            //パッチ内容を画面に表示
            GraphicsToolPatchMakerForm f = new GraphicsToolPatchMakerForm();
            f.Init(patch);
            f.ShowDialog();
        }
        string MakePatch()
        {
            if (!File.Exists(this.ComplieBinFilename))
            {
                R.ShowStopError("コンパイルした結果のファイルがないので、パッチを作成できません。");
                return "";
            }
            uint addr = U.toOffset((uint)this.Address.Value);
            if (!U.CheckZeroAddressWrite(addr))
            {
                return "";
            }
            if (this.Method.SelectedIndex == 0)
            {
                return "";
            }

            string patch = "";
            patch += "NAME=<<PATCH NAME>>\r\n";
            patch += "\r\n";
            patch += "TYPE=BIN\r\n";


            if (this.Method.SelectedIndex == 1)
            {
                byte[] bin = File.ReadAllBytes(this.ComplieBinFilename);
                patch += "PATCHED_IF:" + U.To0xHexString(addr) + "=" + U.HexDumpLiner0x(bin,0,32) + "\r\n";

                patch += "BIN:" + U.To0xHexString(addr) + "=" + Path.GetFileName(this.ComplieBinFilename);
            }
            else
            {
                uint freeaddr = U.toOffset((uint)this.FREEAREA.Value);
                uint usereg = GetSaftyRegister();
                byte[] jumpCode = DisassemblerTrumb.MakeInjectJump(addr, freeaddr, usereg);
                patch += "PATCHED_IF:" + U.To0xHexString(addr) + "=" + U.HexDumpLiner0x(jumpCode) + "\r\n";

                patch += "BIN:$FREEAREA=" + Path.GetFileName(this.ComplieBinFilename);
                patch += "\r\nJUMP:" + U.To0xHexString(addr) + ":r" + usereg + "=" + Path.GetFileName(this.ComplieBinFilename);
            }

            return patch;
        }

        private void SRCSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
#if DEBUG
            open.Filter = "asm|*.asm;*.s|c|*.c;*.cpp";
#else
            open.Filter = "asm|*.asm;*.s";
#endif
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            this.SRCFilename.Text = open.FileNames[0];
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            string text = R._("最後の動作を取り消してよろしいですか？");
            string title = R._("UNDO確認");

            DialogResult r =
                MessageBox.Show(text
                    , title
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Exclamation);
            if (r != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            Program.Undo.RunUndo();  //操作の取り消し
            UndoButton.Hide();

            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }
        void SetExplain()
        {
            HookRegisterLabel.AccessibleDescription = R._("フックするのに利用するレジスタを選択します。\r\nr3を利用するとEAのJumToHackと同じ意味になります。");
            FREEAREALabel.AccessibleDescription = R._("データを格納する領域を定義します。\r\nディフォルトではROM末尾が自動的に指定されています。");
        }

    }
}
