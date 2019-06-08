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
    public partial class TextRefAddDialogForm : Form
    {
        public TextRefAddDialogForm()
        {
            InitializeComponent();
        }

        string OrignalCommandText;
        public void Init(uint textid , string str)
        {
            TargetTextBox.Text = str;
            OrignalCommandText = Program.UseTextIDCache.GetName(textid);
            CommentTextBox.Text = OrignalCommandText;
            CommentTextBox.Focus();
        }

        public string GetComment()
        {
            string comment = this.CommentTextBox.Text;
            if (comment == "")
            {
                if (OrignalCommandText == "")
                {//新規の場合は、追加なので、空文字を入れる
                    comment = " ";
                }
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

        private void TextRefAddDialogForm_Load(object sender, EventArgs e)
        {
            CommentTextBox.Placeholder = R._("参照される場所について説明してください");
        }
    }
}
