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
    public partial class ToolAutomaticRecoveryROMHeaderForm : Form
    {
        public ToolAutomaticRecoveryROMHeaderForm()
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

        private void ApplyButton_Click(object sender, EventArgs e)
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

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            byte[] bin = rom.getBinaryData(0, 0x100);
            Program.ROM.write_range(0, bin,undodata);
            Program.Undo.Push(undodata);
            R.ShowOK("ROMヘッダーを復旧させました");

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }

        private void UPSPatchForm_Load(object sender, EventArgs e)
        {
        }

        private void UPSOpenSimpleForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                String dir = Path.GetDirectoryName(Program.ROM.Filename);
                String filename = Path.GetFileName(Program.ROM.Filename);
                pleaseWait.DoEvents(R._("{0}に適合する元ファイルを自動検索中・・・", filename));

                uint srcCRC32 = Program.ROM.RomInfo.orignal_crc32;

                string orignal_romfile = MainFormUtil.FindOrignalROMByCRC32(dir, srcCRC32);
                this.OrignalFilename.Text = orignal_romfile;

            }
            if (File.Exists(this.OrignalFilename.Text))
            {
//                ApplyButton.PerformClick();
            }
        }
    }
}
