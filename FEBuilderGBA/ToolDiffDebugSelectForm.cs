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
    public partial class ToolDiffDebugSelectForm : Form
    {
        public ToolDiffDebugSelectForm()
        {
            InitializeComponent();
        }

        FindBackup FindBackup;
        
        private void ToolDiffDebugSelectForm_Load(object sender, EventArgs e)
        {
            if (Program.ROM.IsVirtualROM)
            {
                R.ShowStopError("仮想ROMなのでファイル名を取得できません。");
                this.Close();
                return;
            }
        }
        private void ToolDiffDebugSelectForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait())
            {
                this.FindBackup = new FindBackup();
                this.FindBackup.AppendOrignalROMToBackupList(); //末尾にバックアップROMを追加.

                this.PrefixTextBox.Text = this.FindBackup.Prefix;
                this.OrignalFilename.Text = this.FindBackup.OrignalFilename;

                BackupList.Items.Clear();
                BackupList.BeginUpdate();
                for (int i = 0; i < this.FindBackup.Files.Count; i++)
                {
                    string filename = Path.GetFileName(this.FindBackup.Files[i].FilePath);
                    string text = R._("{0} //{1}", filename, this.FindBackup.Files[i].Date.ToLocalTime());
                    BackupList.Items.Add(text);
                }
                BackupList.EndUpdate();
            }
        }

        private void AppendBackupDirectoryButton_Click(object sender, EventArgs e)
        {
            string title = R._("バックアップがある場所を指定してください。");
            string filter = R._("ROMs|*.gba;*.ups|GBA ROMs|*.gba|UPS files|*.ups|GBA.7Z|*.gba.7z|ROMRebuild|*.rebuild|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            open.ShowDialog();
            if (!U.CanReadFileRetry(open))
            {
                return ;
            }
            String dir = Path.GetDirectoryName(open.FileNames[0]);
            this.FindBackup.Scan(dir, this.PrefixTextBox.Text);
        }

        private void BackupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.BackupList.SelectedIndex < 0 || this.BackupList.SelectedIndex >= this.FindBackup.Files.Count)
            {
                return;
            }
            FindBackup.FileInfo fi = this.FindBackup.Files[this.BackupList.SelectedIndex];
            this.ThisFileInfo.Text = R._("ファイル名:\r\n{0}\r\n\r\n更新日付:\r\n{1}", fi.FilePath , fi.Date.ToLocalTime() );
        }

        bool CopyToEmu(FindBackup.FileInfo fi, string emulator_filename)
        {
            byte[] bin = MainFormUtil.OpenROMToByte(fi.FilePath, this.OrignalFilename.Text);
            if (bin.Length <= 0)
            {
                return false;
            }
            File.WriteAllBytes(emulator_filename, bin);
            return true;
        }

        bool CheckOrignalROMIfUPS(FindBackup.FileInfo fi)
        {
            string ext = U.GetFilenameExt(fi.FilePath);
            if (ext == ".UPS")
            {
                string errorMessage = MainFormUtil.CheckOrignalROM(OrignalFilename.Text);
                if (errorMessage != "")
                {
                    R.ShowStopError(errorMessage);
                    OrignalFilename.ErrorMessage = R._(errorMessage);
                    return false;
                }
                OrignalFilename.ErrorMessage = "";
            }
            return true;
        }

        private void BackupList_DoubleClick(object sender, EventArgs e)
        {
            if (this.BackupList.SelectedIndex < 0 || this.BackupList.SelectedIndex >= this.FindBackup.Files.Count)
            {
                return;
            }
            FindBackup.FileInfo fi = this.FindBackup.Files[this.BackupList.SelectedIndex];
            if (!CheckOrignalROMIfUPS(fi))
            {
                return;
            }

            string run_name = "emulator";
            string emulator_filename = U.MakeFilename(run_name);

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                bool r = CopyToEmu(fi, emulator_filename);
                if (!r)
                {
                    return;
                }
            }

            if (! File.Exists(emulator_filename))
            {
                R.ShowStopError("エミュレータにロードするROMを作れませんでした。\r\n{0}",emulator_filename);
                return;
            }

            //セーブファイルが壊されるかもしれないので保存しよう.
            string saveFile = MainFormUtil.FindCurrentSavFilename(Program.ROM.Filename);
            byte[] saveFileBin = null;
            if (File.Exists(saveFile))
            {
                saveFileBin = File.ReadAllBytes(saveFile);
            }

            Process process = MainFormUtil.RunAs(run_name, emulator_filename);
            if (process == null)
            {
                return;
            }
            process.Exited += (ss,ee) => {
                //エミュレータが終了したら、保存していたセーブファイルを書き戻す.
                if (saveFile != "" && saveFileBin != null)
                {
                    File.WriteAllBytes(saveFile,saveFileBin);
                }
            };
        }

        private void BackupList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BackupList_DoubleClick(sender, e);
            }
        }

        private void SelectROMButton_Click(object sender, EventArgs e)
        {
            if (this.BackupList.SelectedIndex < 0 || this.BackupList.SelectedIndex >= this.FindBackup.Files.Count)
            {
                return;
            }
            FindBackup.FileInfo ng_rom_info;
            FindBackup.FileInfo ok_rom_info;
            if (this.BackupList.SelectedIndex < 1)
            {
                //R.ShowOK("直前のバックアップだと、現在のROMとの2点DIFFだけとなり、精度が落ちます。");

                ng_rom_info = new FindBackup.FileInfo();
                ng_rom_info.FilePath = Program.ROM.Filename;
                ng_rom_info.Date = File.GetLastWriteTime(Program.ROM.Filename);
                if (!CheckOrignalROMIfUPS(ng_rom_info))
                {
                    return;
                }

                ok_rom_info = this.FindBackup.Files[this.BackupList.SelectedIndex];
                if (!CheckOrignalROMIfUPS(ok_rom_info))
                {
                    return;
                }
            }
            else
            {
                ng_rom_info = this.FindBackup.Files[this.BackupList.SelectedIndex - 1];
                if (!CheckOrignalROMIfUPS(ng_rom_info))
                {
                    return;
                }

                ok_rom_info = this.FindBackup.Files[this.BackupList.SelectedIndex];
                if (!CheckOrignalROMIfUPS(ok_rom_info))
                {
                    return;
                }
            }

            
            ToolDiffDebugSelectMethodPopup q = (ToolDiffDebugSelectMethodPopup)InputFormRef.JumpFormLow<ToolDiffDebugSelectMethodPopup>();
            q.Init(ng_rom_info.FilePath, ok_rom_info.FilePath);
            q.ShowDialog();
            if (q.DialogResult != System.Windows.Forms.DialogResult.Yes)
            {//ユーザーキャンセル.
                return;
            }
            ToolThreeMargeForm.DiffDebugMethod method = q.GetMethod();

            ToolThreeMargeForm f;
            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                byte[] ng_rom = MainFormUtil.OpenROMToByte(ng_rom_info.FilePath, this.OrignalFilename.Text);
                byte[] ok_rom = MainFormUtil.OpenROMToByte(ok_rom_info.FilePath, this.OrignalFilename.Text);

                if (ng_rom.Length <= 0)
                {
                    return;
                }
                if (ok_rom.Length <= 0)
                {
                    return;
                }

                f = (ToolThreeMargeForm)InputFormRef.JumpFormLow<ToolThreeMargeForm>();
                f.InitDiffDebug(wait, ok_rom, ng_rom , method);
            }
            if (! f.IsConflictData())
            {
                R.ShowWarning("相違点がありません。\r\n比較条件を変えてください。\r\n比較条件を変えても変わらない場合、比較対象のROMを見直してください。");
                return;
            }
            f.Show();
        }

        private void PrefixTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.PrefixTextBox.Text.Length <= 0)
            {
                return;
            }
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("無改造ROMを選択してください");
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

        private void TestPlayButton_Click(object sender, EventArgs e)
        {
            BackupList_DoubleClick(sender, e);
        }


    }
}
