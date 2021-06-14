using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolDisasmSourceCode : Form
    {
        public ToolDisasmSourceCode()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }
        private void OrignalSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("無改造ROMを選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            if (Program.LastSelectedFilename != null)
            {
                Program.LastSelectedFilename.Load(this, "", open);
            }
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }

            if (Program.LastSelectedFilename != null)
            {
                Program.LastSelectedFilename.Save(this, "", open);
            }
            OrignalFilename.Text = open.FileNames[0];
        }
        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }
        private void ToolDisasmSourceCode_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(null))
            {
                SrcCodeFilename.Text = StoreSrcCode;

                String dir = Path.GetDirectoryName(Program.ROM.Filename);
                string orignal_romfile = MainFormUtil.FindOrignalROM(dir);

                this.OrignalFilename.Text = orignal_romfile;
            }
        }
        public string StoreSrcCode;

        private void ToolDisasmSourceCode_Load(object sender, EventArgs e)
        {

        }

        private void MakeButton_Click(object sender, EventArgs e)
        {
            if (this.StoreSrcCode == null)
            {
                return;
            }
            if (! File.Exists(this.OrignalFilename.Text))
            {
                return;
            }
            string appPath = Path.Combine(Program.BaseDirectory, "FEBuilderGBA.exe");
            string args = U.escape_shell_args(this.OrignalFilename.Text) + " " + "--disasm=" + U.escape_shell_args(StoreSrcCode) ;

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                wait.DoEvents(R._("ソースコードの生成するには数分かかります。"));
                MainFormUtil.ProgramRunAsAndEndWait(appPath, args);
            }
            this.Close();
        }
    }
}
