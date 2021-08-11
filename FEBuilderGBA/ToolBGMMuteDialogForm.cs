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
    public partial class ToolBGMMuteDialogForm : Form
    {
        public ToolBGMMuteDialogForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        private void ToolBGMMuteDialogForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = ToggleButton;
        }
        public void Init(int trackNumber, bool isMute, string instName )
        {
            if (isMute)
            {
                label1.Text = R._("トラック({0})をミュート解除にしますか？\r\n{1}", trackNumber, instName);
                ToggleButton.Text = R._("ミュート解除する");
                ToggleButton.ForeColor = OptionForm.Color_Keyword_ForeColor();
            }
            else
            {
                label1.Text = R._("トラック({0})をミュートしますか？\r\n{1}", trackNumber , instName);
                ToggleButton.Text = R._("ミュートする");
                ToggleButton.ForeColor = OptionForm.Color_Error_ForeColor();
            }
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void OnlyPlayButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

    }
}
