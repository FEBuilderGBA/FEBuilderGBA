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
    public partial class ToolProblemReportSearchSavForm : Form
    {
        public ToolProblemReportSearchSavForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        private void SavSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("SAV|*.sav|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            SavFilename.Text = open.FileNames[0];

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public string GetFilename()
        {
            return SavFilename.Text;
        }

        private void ToolProblemReportSearchSavForm_Load(object sender, EventArgs e)
        {

        }
    }
}
