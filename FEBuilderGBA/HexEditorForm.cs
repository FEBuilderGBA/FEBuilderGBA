using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class HexEditorForm : Form
    {
        public HexEditorForm()
        {
            InitializeComponent();
            this.HexBox.OnSelectionChange = HexBox_OnSelectionChange;
        }

        private void HexEditorForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
            HexEditorForm_Resize(null, null);
        }

        private void HexBox_OnSelectionChange(object sender, EventArgs e)
        {
            uint start = this.HexBox.GetCursolPosStart();
            uint end = this.HexBox.GetCursolPosEnd();

            if (start == end)
            {
                this.SelectAddress.Text = U.To0xHexString(start);
            }
            else
            {
                uint select = end - start + 1;
                this.SelectAddress.Text = U.To0xHexString(start) + "-" + U.To0xHexString(end) + " " + U.To0xHexString(select) + "(" + select + ")Bytes";
            }
            ShowHint(end);
        }

        void ShowHint(uint addr)
        {
            uint pointer = U.toPointer(addr);
            AsmMapFile asmMap = Program.AsmMapFileAsmCache.GetAsmMapFile(); 
            AsmMapFile.AsmMapSt p ;

            //カーソル値のデータのヒントを与える.
            string hint = "";
            if (asmMap.TryGetValue(pointer, out p))
            {
                hint = p.ToStringInfo();
                if (p.Length > 0)
                {
                    hint += " Length:" + p.Length; 
                }
            }
            else
            {
                uint near_pointer = asmMap.SearchNear(pointer);
                if (asmMap.TryGetValue(near_pointer, out p))
                {
                    if (pointer < near_pointer + p.Length)
                    {
                        uint offset = (pointer - near_pointer);
                        hint = U.To0xHexString(U.toOffset(near_pointer)) + "+" + offset + "(" + U.To0xHexString(offset) + ")/" + p.Length + p.ToStringInfo();
                    }
                }

            }
            Hint.Text = hint;
        }

        private void DisASMButton_Click(object sender, EventArgs e)
        {
            uint start = this.HexBox.GetCursolPosStart();
            DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>();
            f.JumpTo(start);
        }
        public void JumpTo(uint addr, uint end = U.NOT_FOUND)
        {
            addr = U.toOffset(addr);
            this.HexBox.JumpTo(U.ToHexString(addr),false);
            this.HexBox.Focus();
        }

        private void HexEditorForm_Resize(object sender, EventArgs e)
        {
            this.HexBox.Width = this.Width - 4;
            this.HexBox.Height = this.Height - 4 - this.ControlPanel.Height*2;

            this.ControlPanel.Width = this.Width - 4;
            this.ControlPanel.Location = new Point(0, this.Height - this.ControlPanel.Height*2);
            this.Hint.Width = this.ControlPanel.Width - 12;
        }


        private void SelectAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                this.HexBox.Focus();
            }
        }

        List<string> JumpHistory = new List<string>();
        List<string> SearchHistory = new List<string>();

        private void JumpButton_Click(object sender, EventArgs e)
        {
            HexEditorJump f = (HexEditorJump)InputFormRef.JumpFormLow<HexEditorJump>();

            f.Init(this.JumpHistory);

            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string addr = f.AddrComboBox.Text;
            if (this.JumpHistory.IndexOf(addr) < 0)
            {
                this.JumpHistory.Add(addr);
            }
            bool forceLittleEndian = f.LittleEndianCheckBox.Checked;
            this.HexBox.Focus();
            this.HexBox.JumpTo(addr, forceLittleEndian);
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            HexEditorSearch f = (HexEditorSearch)InputFormRef.JumpFormLow<HexEditorSearch>();
            DialogResult dr = f.ShowDialog();

            f.Init(this.SearchHistory);
            
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string addr = f.AddrComboBox.Text;
            Search(addr
                , f.LittleEndianCheckBox.Checked
                , f.RevCheckBox.Checked
                , f.Align4.Checked)
                ;
        }
        public void Search(string need, bool isLitteEndian = false, bool isRev = false, bool isAlign4 = false)
        {
            if (this.SearchHistory.IndexOf(need) < 0)
            {
                this.SearchHistory.Add(need);
            }
            this.HexBox.Focus();
            this.HexBox.Search(need, isLitteEndian, isRev, isAlign4, false);
        }

        private void HexBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && (e.Shift || e.Control))
            {
                this.SearchButton.PerformClick();
                return;
            }
            if (e.KeyCode == Keys.J && (e.Shift || e.Control)
                || e.KeyCode == Keys.G && (e.Shift || e.Control)
                )
            {
                this.JumpButton.PerformClick();
                return;
            }
            if (e.KeyCode == Keys.M && (e.Shift || e.Control))
            {
                this.SetMarkButton.PerformClick();
                return;
            }
            if (e.KeyCode == Keys.L && (e.Shift || e.Control))
            {
                this.MarkListButton.PerformClick();
                return;
            }
            if (e.KeyCode == Keys.S && (e.Control))
            {
                this.WriteButton.PerformClick();
                return;
            }
            if (!(e.Shift || e.Control) &&
                (
                (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                || (e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)
                ))
            {
                InputFormRef.WriteButtonToYellow(this.WriteButton, true);
            }
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            {
                //ROMより大きくなる場合は、リサイズする
                uint length = this.HexBox.getDataLength();
                if (length > Program.ROM.Data.Length)
                {
                    bool isResizeSuccess = Program.ROM.write_resize_data(length);
                    if (isResizeSuccess == false)
                    {
                        return ;
                    }
                }
            }

            List<HexBox.UndoData> list = this.HexBox.getWriteData();
            for (int i = 0; i < list.Count; i++)
            {
                Program.ROM.write_u8(list[i].addr, list[i].newvalue, undodata);
            }
            Program.Undo.Push(undodata);
            this.HexBox.Focus();
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);

            Program.AsmMapFileAsmCache.ClearCache();
        }

        private void SetMarkButton_Click(object sender, EventArgs e)
        {
            this.HexBox.SetMark();
            this.HexBox.Focus();
        }

        private void MarkListButton_Click(object sender, EventArgs e)
        {
            HexEditorMark f = (HexEditorMark)InputFormRef.JumpFormLow<HexEditorMark>();

            f.Init(this.HexBox.GetMarks());

            DialogResult dr = f.ShowDialog();
            this.HexBox.Focus();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            this.HexBox.JumpTo(f.SelectAddr(), false);
        }

    }
}
