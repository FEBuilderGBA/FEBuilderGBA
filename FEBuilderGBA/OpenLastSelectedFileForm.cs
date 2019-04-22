using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OpenLastSelectedFileForm : Form
    {
        public OpenLastSelectedFileForm()
        {
            InitializeComponent();

            LastSelectedFilename.Text = Program.LastSelectedFilename.GetLastFilename();
            U.AddCancelButton(this);
        }

        private void RunAsButton_Click(object sender, EventArgs e)
        {
            U.OpenURLOrFile(LastSelectedFilename.Text);
        }

        private void OpenDirButton_Click(object sender, EventArgs e)
        {
            U.SelectFileByExplorer(LastSelectedFilename.Text);
        }

        private void OpenLastSelectedFileForm_Load(object sender, EventArgs e)
        {

        }
    }
}
