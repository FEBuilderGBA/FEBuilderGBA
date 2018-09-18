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
    public partial class ToolUPSOpenSimpleForm : Form
    {
        public ToolUPSOpenSimpleForm()
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
            if (Program.LastSelectedFilename != null)
            {
                Program.LastSelectedFilename.Load(this, "", open);
            }
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return ;
            }
            if (!U.CanReadFileRetry(open))
            {
                return ;
            }

            if (Program.LastSelectedFilename != null)
            {
                Program.LastSelectedFilename.Save(this, "", open);
            }
            OrignalFilename.Text = open.FileNames[0];
        }

        private void ApplyUPSPatchButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string errorMessage = MainFormUtil.CheckOrignalROM(OrignalFilename.Text);
            if (errorMessage != "")
            {
                R.ShowStopError("無改造ROMを指定してください。" + "\r\n" + errorMessage);
                OrignalFilename.ErrorMessage = R._("無改造ROMを指定してください。" + "\r\n" + errorMessage);
                return;
            }
            OrignalFilename.ErrorMessage = "";

            ROM rom = new ROM();
            string version;
            bool r = rom.Load(OrignalFilename.Text, out version);
            if (!r)
            {
                R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                r = UPSUtil.ApplyUPS(rom, this.UPSFilename);
                if (!r)
                {
                    R.ShowStopError("UPSパッチを適応できませんでした");
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

            if (this.UseReOpen)
            {//メインフォームを開きなおさないといけない場合
                MainFormUtil.ReOpenMainForm();
            }

            if (this.IsSaveFileCheckBox.Checked)
            {
                string newFilename = U.ChangeExtFilename(this.UPSFilename, ".gba");
                rom.Save(newFilename, false);

                //エクスプローラで選択しよう
                U.SelectFileByExplorer(newFilename);

                //保存したROMを開く.
                Program.LoadROM(newFilename, this.ForceVersion);
            }
            else
            {
                //保存しない場合、メモリ上の存在になる.
                Program.LoadVirtualROM(rom, this.UPSFilename);
            }
        }

        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }

        private void UPSPatchForm_Load(object sender, EventArgs e)
        {
        }

        string UPSFilename;
        bool UseReOpen;
        string ForceVersion;
        public void OpenUPS(string upsFilename, bool useReOpen, string forceversion)
        {
            this.UPSFilename = upsFilename;
            this.UseReOpen = useReOpen;
            this.ForceVersion = forceversion;
        }

        private void UPSOpenSimpleForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                String dir = Path.GetDirectoryName(this.UPSFilename);
                String filename = Path.GetFileName(this.UPSFilename);
                pleaseWait.DoEvents(R._("{0}に適合する元ファイルを自動検索中・・・", filename));

                uint srcCRC32 = UPSUtil.GetUPSSrcCRC32(this.UPSFilename);

                string orignal_romfile = MainFormUtil.FindOrignalROMByCRC32(dir, srcCRC32);
                this.OrignalFilename.Text = orignal_romfile;
            }
        }
    }
}
