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
    public partial class OverraideCheckWithErrorDialog : Form
    {
        public OverraideCheckWithErrorDialog()
        {
            InitializeComponent();
        }

        private void OverraideCheckDialog_Load(object sender, EventArgs e)
        {
            DiaplayFilename.Text = Program.ROM.Filename;

            this.MyCancelButton.Focus();
            System.Media.SystemSounds.Hand.Play();
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void MyOKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }
    }
}
