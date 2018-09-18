using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MainSimpleMenuEventErrorIgnoreErrorForm : Form
    {
        public MainSimpleMenuEventErrorIgnoreErrorForm()
        {
            InitializeComponent();
        }

        public void Init(FELint.ErrorSt error)
        {
            ErrorTextBox.Text = U.ToHexString(error.Addr) 
                + "\r\n" + error.ErrorMessage;
            CommentTextBox.Focus();
        }

        public string GetComment()
        {
            string comment = this.CommentTextBox.Text;
            if (comment == "")
            {
                comment = " ";
            }
            return comment;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void MainSimpleMenuEventErrorIgnoreErrorForm_Load(object sender, EventArgs e)
        {
            CommentTextBox.Placeholder = R._("非表示にする理由");
        }
    }
}
