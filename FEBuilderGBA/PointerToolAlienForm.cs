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
    public partial class PointerToolAlienForm : Form
    {
        public PointerToolAlienForm()
        {
            InitializeComponent();
            SRCFilename.AllowDropFilename();

            AllowDropFilename();
        }

        void AllowDropFilename()
        {
            U.AllowDropFilename(this
                , new string[] { ".S", ".ASM", ".INC", ".EVENT", ".TXT","" }
                , (string filename) =>
                {
                    SRCFilename.Text = filename;
                    this.RunButton.PerformClick();
                });
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void SRCSelectButton_Click(object sender, EventArgs e)
        {

        }

        private void PointerToolAlienForm_Load(object sender, EventArgs e)
        {

        }

        public string GetFilename()
        {
            return this.SRCFilename.Text;
        }
    }
}
