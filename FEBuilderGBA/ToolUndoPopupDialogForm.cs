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
    public partial class ToolUndoPopupDialogForm : Form
    {
        public ToolUndoPopupDialogForm()
        {
            InitializeComponent();
        }

        private void ToolUndoPopupDialogForm_Load(object sender, EventArgs e)
        {
        }

        public void Init(string version)
        {
            FormIcon.Image = SystemIcons.Question.ToBitmap();
            Info.Text = R._("このバージョン({{0}})に戻してもよろしいですか？");
        }

        private void TestPlayButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

        private void RunUndoButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
