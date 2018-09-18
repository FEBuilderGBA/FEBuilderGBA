using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class PointerToolBatchInputForm : Form
    {
        public PointerToolBatchInputForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }


        private void PointerToolCopyToForm_Load(object sender, EventArgs e)
        {
            this.TextData.Focus();
        }

        public string GetTextData()
        {
            return this.TextData.Text;
        }
        public void SetTextData(string text)
        {
            if(text != "")
            {
                this.RunButton.Text = R._("完了");
            }
            this.TextData.Text = text;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }
    }
}
