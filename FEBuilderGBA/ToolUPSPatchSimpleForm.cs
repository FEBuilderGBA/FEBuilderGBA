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
    public partial class ToolUPSPatchSimpleForm : Form
    {
        public ToolUPSPatchSimpleForm()
        {
            InitializeComponent();

            OrignalFilename.AllowDropFilename();
            U.AddCancelButton(this);
        }

        private void OrignalSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("無改造ROMを選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return ;
            }
            if (!U.CanReadFileRetry(open))
            {
                return ;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            OrignalFilename.Text = open.FileNames[0];
        }

        string MakeUPSFilename()
        {
            string ups = "PATCH." + DateTime.Now.ToString("yyyyMMddHHmmss");
            string upsFullPath = U.MakeFilename(ups,".ups");
            return upsFullPath;
        }

        private void MakeUPSPatchButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string errorMessage = MainFormUtil.CheckOrignalROM(OrignalFilename.Text);
            if (errorMessage != "")
            {
                string error = R._("無改造ROMを指定してください。") + "\r\n" + errorMessage;
                OrignalFilename.ErrorMessage = error;
                R.ShowStopError(error);
                return;
            }
            OrignalFilename.ErrorMessage = "";

            string title = R._("作成するUPSファイル名を選択してください");
            string filter = R._("UPS|*.ups|All files|*");
            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, MakeUPSFilename());

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                UPSUtil.MakeUPS(OrignalFilename.Text, save.FileName);
            }
            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileName);
        }

        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }

        private void UPSPatchForm_Load(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(null))
            {
                String dir = Path.GetDirectoryName(Program.ROM.Filename);
                string orignal_romfile = MainFormUtil.FindOrignalROM(dir);

                this.OrignalFilename.Text = orignal_romfile;
            }
        }

    }
}
