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
    public partial class HexEditorJump : Form
    {
        public HexEditorJump()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void Init(List<string> list)
        {
            AddrComboBox.BeginUpdate();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                AddrComboBox.Items.Add(list[i]);
            }
            AddrComboBox.EndUpdate();
        }

        private void HexEditorJump_Load(object sender, EventArgs e)
        {

        }


    }
}
