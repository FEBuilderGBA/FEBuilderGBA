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
    public partial class HexEditorMark : Form
    {
        public HexEditorMark()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        private void HexEditorMark_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(this.AddressList, 0, true);
        }

        public void Init(List<HexBox.Mark> marks)
        {
            this.AddressList.Items.Clear();
            this.AddressList.BeginUpdate();
            for (int i = 0; i < marks.Count; i++)
            {
                this.AddressList.Items.Add(U.ToHexString(marks[i].address) );
            }
            this.AddressList.EndUpdate();
        }
        public void Init(List<ToolThreeMargeForm.ChangeDataSt> marks)
        {
            this.AddressList.Items.Clear();
            this.AddressList.BeginUpdate();
            for (int i = 0; i < marks.Count; i++)
            {
                if (marks[i].mark)
                {
                    this.AddressList.Items.Add(U.ToHexString(marks[i].addr));
                }
            }
            this.AddressList.EndUpdate();
        }

        private void AddressList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            JumpTo.PerformClick();
        }

        private void JumpTo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Address = U.atoh(this.AddressList.Text);
            this.Close();
        }

        uint Address;
        public uint SelectAddr()
        {
            return this.Address;
        }

    }
}
