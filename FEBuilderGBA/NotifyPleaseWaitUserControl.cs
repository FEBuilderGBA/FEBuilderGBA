using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class NotifyPleaseWaitUserControl : UserControl
    {
        public NotifyPleaseWaitUserControl()
        {
            InitializeComponent();

            Color Color_NotifyWrite_BackColor = OptionForm.Color_NotifyWrite_BackColor();
            Color Color_NotifyWrite_ForeColor = OptionForm.Color_NotifyWrite_ForeColor();

            this.BackColor = Color_NotifyWrite_BackColor;
            this.ForeColor = Color_NotifyWrite_ForeColor;

            this.BIG_TEXT.BackColor = Color_NotifyWrite_BackColor;
            this.BIG_TEXT.ForeColor = Color_NotifyWrite_ForeColor;
        }
        public void Message(string message)
        {
            MessageLabel.Text = message;
        }
        public string GetMessage()
        {
            return MessageLabel.Text;
        }
    }
}
