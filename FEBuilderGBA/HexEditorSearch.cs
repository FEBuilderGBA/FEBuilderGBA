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
    public partial class HexEditorSearch : Form
    {
        public HexEditorSearch()
        {
            InitializeComponent();
            U.AddCancelButton(this);

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

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void HexEditorSearch_Load(object sender, EventArgs e)
        {

        }

    }
}
