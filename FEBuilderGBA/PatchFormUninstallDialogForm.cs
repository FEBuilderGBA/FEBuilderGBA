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
    public partial class PatchFormUninstallDialogForm : Form
    {
        public PatchFormUninstallDialogForm()
        {
            InitializeComponent();

            OrignalFilename.AllowDropFilename();
            U.AddCancelButton(this);
        }

        List<PatchForm.BinMapping> TargetBinMAP; //const 参照のみ C#だとできないんだよなあ..
        bool IsAutomatic; //自動でアンインストールを開始する.
        public void Init(List<PatchForm.BinMapping> binmap, bool isAutomatic)
        {
            this.TargetBinMAP = binmap;
            this.IsAutomatic = isAutomatic;
        }

        private void OrignalSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("アンインストールしたパッチを含んでいないROMを選択してください");
            string filter = R._("ROMs|*.gba;*.ups|GBA ROMs|*.gba|UPS files|*.ups|GBA.7Z|*.gba.7z|ROMRebuild|*.rebuild|Binary files|*.bin|All files|*");

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
            string filename = open.FileNames[0];

            //念のため確認する
            byte[] rom = MainFormUtil.OpenROMToByte(
                  filename
                , this.FindBackup.OrignalFilename);
            if (! SearchNotContainThisPatchBy(rom))
            {
                R.ShowStopError("このROMはアンインストール対象のパッチを含んでいるので利用できません。");
                return ;
            }
            //利用できるらしい.
            OrignalFilename.Text = filename;
        }


        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }

        FindBackup FindBackup;

        private void UPSOpenSimpleForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                this.FindBackup = new FindBackup();
                this.OrignalFilename.Text = SearchNotContainThisPatch(pleaseWait);
            }

            if (this.IsAutomatic)
            {
                UninstallPatchButton.PerformClick();
            }
        }

        //このパッチを含んでいない最も新しいバックアップを探索します
        string SearchNotContainThisPatch(InputFormRef.AutoPleaseWait pleaseWait)
        {
            //現在のROMが仮想ROMの場合は無理
            if (Program.ROM.IsVirtualROM)
            {
                return this.FindBackup.OrignalFilename;
            }
            for (int i = 0; i < this.FindBackup.Files.Count; i++)
            {
                pleaseWait.DoEvents(R._("パッチを含まないバックアップを探索中。進捗:{1}/{2} {0}"
                    , Path.GetFileName(this.FindBackup.Files[i].FilePath)
                    , i, this.FindBackup.Files.Count));

                byte[] rom = MainFormUtil.OpenROMToByte(
                     this.FindBackup.Files[i].FilePath
                    ,this.FindBackup.OrignalFilename);

                if (SearchNotContainThisPatchBy(rom))
                {
                    return this.FindBackup.Files[i].FilePath;
                }
            }

            return this.FindBackup.OrignalFilename;
        }

        //このROMはパッチを含んでいませんか?
        bool SearchNotContainThisPatchBy(byte[] rom)
        {
            {
                uint orignal_crc32 = Program.ROM.RomInfo.orignal_crc32();
                U.CRC32 crc32 = new U.CRC32();
                if (orignal_crc32 == crc32.Calc(rom))
                {//無改造ROM
                    return false;
                }
            }

            uint limit = (uint)rom.Length;
            for (int i = 0; i < this.TargetBinMAP.Count; i++)
            {
                PatchForm.BinMapping t = this.TargetBinMAP[i];
                uint length = (uint)t.bin.Length;
                if (t.addr + length > limit)
                {
                    continue;
                }
                if (length <= 2)
                {
                    continue;
                }
                byte[] bin = U.getBinaryData(rom, t.addr, length);
                if (U.memcmp(bin, t.bin) == 0)
                {//含んでいる
                    return false;
                }
            }

            //含んでいない
            return true;
        }


        public byte[] GetOrignalROMData()
        {
            byte[] rom = MainFormUtil.OpenROMToByte(
                  this.OrignalFilename.Text
                , this.FindBackup.OrignalFilename);
            return rom;
        }

        private void UninstallPatchButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void PatchFormUninstallDialogForm_Load(object sender, EventArgs e)
        {

        }
    }
}
