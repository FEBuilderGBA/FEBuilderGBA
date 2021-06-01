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
    public partial class ErorrUnknownROM : Form
    {
        public ErorrUnknownROM()
        {
            InitializeComponent();
            this.ResultVersion = "NAZO";
        }

        string ResultVersion;
        public string GetResultVersion()
        {
            return ResultVersion;
        }


        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void FE6Button_Click(object sender, EventArgs e)
        {
            this.ResultVersion = "FE6";
            this.Close();
        }


        public void Init(string version)
        {
            this.ROMVersion.Text = R._("ROMのバージョン情報:") + version;
        }

        private void FE8UButton_Click(object sender, EventArgs e)
        {
            this.ResultVersion = "FE8U";
            this.Close();
        }

        private void FE8JButton_Click(object sender, EventArgs e)
        {
            this.ResultVersion = "FE8J";
            this.Close();
        }

        private void FE7UButton_Click(object sender, EventArgs e)
        {
            this.ResultVersion = "FE7U";
            this.Close();
        }

        private void FE7JButton_Click(object sender, EventArgs e)
        {
            this.ResultVersion = "FE7J";
            this.Close();
        }
        private void FE0Button_Click(object sender, EventArgs e)
        {
            this.ResultVersion = "NAZO";
            this.Close();
        }

        private void MyCancelButton_Click_1(object sender, EventArgs e)
        {
            this.ResultVersion = "";
        }

        private void ErorrUnknownROM_Load(object sender, EventArgs e)
        {

        }

    }
}
