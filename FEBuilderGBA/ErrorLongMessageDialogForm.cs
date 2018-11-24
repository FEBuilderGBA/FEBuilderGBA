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
    public partial class ErrorLongMessageDialogForm : Form
    {
        public ErrorLongMessageDialogForm()
        {
            InitializeComponent();
        }

        public void SetErrorMessage(string message)
        {
            this.ErrorMessage.Text = message;
        }

        private void ErrorLongMessageDialogForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
        }

        private void MyCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ErrorMessage_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void ErrorLongMessageDialogForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
