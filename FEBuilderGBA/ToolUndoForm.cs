using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolUndoForm : Form
    {
        public ToolUndoForm()
        {
            InitializeComponent();
        }

        private void UndoForm_Load(object sender, EventArgs e)
        {
            Redraw();
        }

        void Redraw()
        {
            this.AddressList.BeginUpdate();
            this.AddressList.Items.Clear();
            for (int i = Program.Undo.UndoBuffer.Count ; i >= 0; i--)
            {
                string name = Program.Undo.MakeName(i);
                this.AddressList.Items.Add(name);
            }
            this.AddressList.EndUpdate();

            //現在地をマーク
            int index = Program.Undo.UndoBuffer.Count - Program.Undo.Postion;
            U.SelectedIndexSafety(this.AddressList,index);
        }

        private void AddressList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RollbackThisVersion();
        }

        private void RollbackThisVersion()
        {
            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }
            int rollbackPOS = Program.Undo.UndoBuffer.Count - this.AddressList.SelectedIndex;
            if (rollbackPOS < 0)
            {
                return;
            }
            if (Program.Undo.Postion == rollbackPOS)
            {
                return;
            }

            string name = Program.Undo.MakeName(rollbackPOS,false);

            ToolUndoPopupDialogForm f = (ToolUndoPopupDialogForm)InputFormRef.JumpFormLow<ToolUndoPopupDialogForm>();
            f.Init(name);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Program.Undo.Rollback(rollbackPOS);
                Redraw();
                InputFormRef.ShowWriteNotifyAnimation(this, 0);
                return;
            }
            if (f.DialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Program.Undo.TestPlayThisVersion(rollbackPOS);
                return;
            }
        }

        void TestPlayThisVersion()
        {
            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }
            int rollbackPOS = Program.Undo.UndoBuffer.Count - this.AddressList.SelectedIndex;
            if (rollbackPOS < 0)
            {
                return;
            }
            Program.Undo.TestPlayThisVersion(rollbackPOS);
        }

        private void UndoForm_Activated(object sender, EventArgs e)
        {
            Redraw();
        }

        private void rollbackButton_Click(object sender, EventArgs e)
        {
            RollbackThisVersion();
        }

        private void testplayButton_Click(object sender, EventArgs e)
        {
            TestPlayThisVersion();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
