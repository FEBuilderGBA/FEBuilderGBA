using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolProblemReportForm : Form
    {
        public ToolProblemReportForm()
        {
            InitializeComponent();
            InputFormRef.TabControlHideTabOption(MainTab);
            OrignalFilename.AllowDropFilename();

            SetExplain();

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, this.X_MapID, this.X_MapName, 0, "MAP", new string[]{ });
            U.ForceUpdate(this.X_MapID,0);
            InputFormRef.makeLinkEventHandler("", controls, this.X_UnitID, this.X_UnitName, 0, "UNIT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.X_UnitID, this.X_UnitIcon, 0, "UNITICON", new string[] { });
            
            U.ForceUpdate(this.X_UnitID, 1);

            this.Problem.Placeholder = R._("例: 主人公が戦闘しようとするとフリーズします。\r\n主人公以外のキャラクタでも発生します。\r\n武器や魔法を変更してもフリーズします。\r\n序章で、エイリークがグラド兵と戦闘するとフリーズを再現できます。\r\n(どういう問題があるか、どうしたら問題を再現できるか、できるだけ詳しい情報を書いてください)");
            this.URLTextBoxEx.Text = MainFormUtil.GetCommunitiesURL();

            this.AboutReport7zText.Text = MainFormUtil.GetReport7zURL();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                this.FindBackup = new FindBackup();
                this.OrignalFilename.Text = FindBackup.OrignalFilename;
            }

            MainTab.SelectedTab = Step1Page;
        }

        private void Step1NextButton_Click(object sender, EventArgs e)
        {
            if (Problem.Text == "")
            {
                Problem.ErrorMessage = R._("問題点を記載してください。");
                return;
            }
            Problem.ErrorMessage = "";
            MainTab.SelectedTab = Step2Page;
        }

        private void Step2PrevButton_Click(object sender, EventArgs e)
        {
            MainTab.SelectedTab = Step1Page;
        }

        private void Step2NextButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            string errorMessage = MainFormUtil.CheckOrignalROM(OrignalFilename.Text);
            if (errorMessage != "")
            {
                R.ShowStopError(errorMessage);
                OrignalFilename.ErrorMessage = R._(errorMessage);
                return;
            }

            OrignalFilename.ErrorMessage = "";


            string title = R._("レポートを保存するファイル名を選択してください");
            string filter = R._("report.7z|*.report.7z|All files|*");
            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, MakeReportFilename());

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                string error = MakeReport(save.FileNames[0]);
                if (error != "")
                {
                    R.ShowStopError("レポートを作れませんでした。\r\n{0}", error);
                    return;
                }
            }
            MainTab.SelectedTab = EndPage;

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileNames[0]);
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            if (this.OpenCommunities.Checked)
            {
                MainFormUtil.GotoCommunities();
            }
            this.Close();
        }

        FindBackup FindBackup;
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
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            OrignalFilename.Text = open.FileNames[0];
            OrignalFilename.ErrorMessage = "";
        }

        string MakeReportFilename()
        {
            string ups = "REPORT." + DateTime.Now.ToString("yyyyMMddHHmmss");
            string upsFullPath = U.MakeFilename(ups, ".report.7z");
            return upsFullPath;
        }


        string FindSrcByFilename(int oldIndex)
        {
            int length = this.FindBackup.Files.Count;
            int i = 0;
            if (i >= length)
            {
                return "";
            }
            string filename = this.FindBackup.Files[i].FilePath;

            int searchIndex = i + oldIndex;
            if (searchIndex >= this.FindBackup.Files.Count)
            {//ない.
                return "";
            }

            return this.FindBackup.Files[searchIndex].FilePath;
        }

        string MakeReport(string fullfilename)
        {
            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            //テンポラリディレクトリを利用する
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                string orignalFilename = OrignalFilename.Text;
                byte[] s = File.ReadAllBytes(orignalFilename);

                //セーブデータの回収
                CollectSaveData(tempdir.Dir);

                //現在のROMのUPSデータの回収
                CollectUPSsCurrentROM(tempdir.Dir, s);

                //動作しないUPSと動作するUPSデータの回収
                CollectUPSs(tempdir.Dir,s);

                //ログとユーザの説明を書き込む
                string log = Path.Combine(tempdir.Dir, "log.txt");
                U.WriteAllText(log, MakeReportLog());

                //etcの内容をコピー
                //lintやコメントなどの設定がほしい
                CopyEtcData(tempdir.Dir);

                //添付データ
                AttachData(tempdir.Dir);

                //7z圧縮
                InputFormRef.DoEvents(this, R._("7z圧縮中"));
                return ArchSevenZip.Compress(fullfilename, tempdir.Dir);
            }
        }
        void AttachData(string tempdir)
        {
            if (! File.Exists(this.AttachDataFilename.Text))
            {
                return;
            }
            string fullfilename = Path.Combine(tempdir, Path.GetFileName(this.AttachDataFilename.Text));
            File.Copy(this.AttachDataFilename.Text , fullfilename , true);
        }

        void CopyEtcData(string tempdir)
        {
            string tempEtcDir = Path.Combine(tempdir, "etc");
            U.mkdir(tempEtcDir);

            string[] etcFiles = new string[] { "lint_", "comment_", "flag_" };
            foreach(var name in etcFiles)
            {
                string src = U.ConfigEtcFilename(name);
                if (! File.Exists(src))
                {
                    continue;
                }

                string dest = Path.Combine(tempEtcDir, Path.GetFileName(src));
                InputFormRef.DoEvents(this,"=>" + Path.GetFileName(dest));
                File.Copy(src, dest);
            }
        }

        bool MakeUPS(string tempdir, byte[] s, string targetFilename)
        {
            string filename_only = Path.GetFileName(targetFilename);
            if (filename_only.IndexOf(".backup.") <= -1)
            {
                return false;
            }
            InputFormRef.DoEvents(this, "=>" + Path.GetFileName(targetFilename));

            string orignalFilename = OrignalFilename.Text;

            byte[] d = MainFormUtil.OpenROMToByte(targetFilename, orignalFilename);
            string ups = Path.Combine(tempdir, Path.GetFileNameWithoutExtension(targetFilename) + ".ups");
            UPSUtil.MakeUPS(s, d, ups);
            U.CopyTimeStamp(targetFilename, ups); //タイムスタンプを元のファイルに合わせる.

            return true;
        }

        //動作しないUPSと動作するUPSデータの回収
        void CollectUPSs(string tempdir, byte[] s)
        {
            bool r = CollectUPSsInner(tempdir, s);
            if (r)
            {
                return;
            }

            //１つもバックアップがない場合は聞く.
            ToolProblemReportSearchBackupForm f =
                (ToolProblemReportSearchBackupForm)InputFormRef.JumpFormLow<ToolProblemReportSearchBackupForm>();
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string moreOlderFilename = f.GetFilename();
            MakeUPS(tempdir, s, moreOlderFilename);
        }
        bool CollectUPSsInner(string tempdir, byte[] s)
        {
            bool found = false;

            //古いバックアップがあれば取得する
            int[] olderPickup = new int[] { 0, 1 , 2 , 4 , 6, 10, 15, 20, 30, 60, 100, 300, 600,  800 , 1000 };
            for (int i = 0; i < olderPickup.Length; i++)
            {
                string moreOlderFilename = FindSrcByFilename(olderPickup[i]);
                if (moreOlderFilename != ""
                    && File.Exists(moreOlderFilename))
                {
                    bool r = MakeUPS(tempdir, s, moreOlderFilename);
                    if (r)
                    {
                        found = true;
                    }
                }
                else
                {
                    break;
                }
            }
            return found;
        }

        //現在のROMのUPSデータの回収
        void CollectUPSsCurrentROM(string tempdir, byte[] s)
        {
            //現在のROMデータを保存する.
            {
                string file = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
                string ups = Path.Combine(tempdir, file + ".ups");
                UPSUtil.MakeUPS(s, Program.ROM.Data, ups);
            }

            //保存していないデータが存在する場合 変更する前のデータをバックアップとして回収する
            if (Program.ROM.Modified && !Program.ROM.IsVirtualROM)
            {
                byte[] d = MainFormUtil.OpenROMToByte(Program.ROM.Filename);
                DateTime date = File.GetLastWriteTime(Program.ROM.Filename);
                string file = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
                string backup = "backup." + date.ToString("yyyyMMddHHmmss");
                string ups = Path.Combine(tempdir, file + "." + backup + ".GBA.ups");
                UPSUtil.MakeUPS(s, d , ups);
                U.CopyTimeStamp(Program.ROM.Filename, ups); //タイムスタンプを元のファイルに合わせる.
            }

        }

        //セーブデータの回収
        void CollectSaveData(string tempdir)
        {
            bool r = CollectSaveDataInner(tempdir);
            if (r)
            {
                return;
            }

            //セーブデータがない場合は、聞く.
            ToolProblemReportSearchSavForm f = 
                (ToolProblemReportSearchSavForm)InputFormRef.JumpFormLow<ToolProblemReportSearchSavForm>();
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return ;
            }

            string savFilename = f.GetFilename();
            PickupOneFile(tempdir , savFilename);
        }
        bool CollectSaveDataInner(string tempdir)
        {
            bool foundSav = false;
            bool r = PickupSaveData(tempdir, ".sav");
            if (r == false)
            {
                //no$gbaの場合は、BATTERYフォルダの下にある.
                foundSav = CollectNoDollSaveData(tempdir, ".sav");
            }

            r = PickupSaveData(tempdir, ".emulator.sav");
            if (r)
            {
                foundSav = true;
            }

            for (int i = 1; i < 10; i++)
            {
                string[] pathes = new string[] {
                    ".emulator" + i + ".sgm",
                    "" + i + ".sgm",
                    ".emulator.ss" + i,
                    ".ss" + i,
                    ".emulator.sg" + i,
                    ".sg" + i,
                    ".sa" + i,
                };
                foreach(string path in pathes)
                {
                    r = PickupSaveData(tempdir, path);
                    if (r)
                    {
                        foundSav = true;
                    }
                }
            }

            return foundSav;
        }

        bool PickupOneFile(string tempdir, string target)
        {
            if (!File.Exists(target))
            {
                return false;
            }
            string destFilename = Path.Combine(tempdir, Path.GetFileName(target));
            File.Copy(target, destFilename, true);
            return true;
        }

        bool PickupSaveData(string tempdir, string needExt)
        {
            string dir  = Path.GetDirectoryName(Program.ROM.Filename);
            string file = Path.GetFileNameWithoutExtension(Program.ROM.Filename);

            string savFilename = Path.Combine(dir, file + needExt);
            if (! File.Exists(savFilename))
            {
                return false;
            }
            string destFilename = Path.Combine(tempdir, file + needExt);
            File.Copy(savFilename, destFilename, true);
            return true;
        }
        //no$gbaの場合は、BATTERYフォルダの下にある.
        bool CollectNoDollSaveData(string tempdir, string needExt)
        {
            string emudir = Program.Config.at("emulator");
            if (emudir == "")
            {
                return false;
            }
            emudir = Path.GetDirectoryName(emudir);
            string dir = Path.Combine(emudir , "BATTERY");
            if (! Directory.Exists(dir))
            {
                return false;
            }

            string file = Path.GetFileNameWithoutExtension(Program.ROM.Filename);

            string savFilename = Path.Combine(dir, file + needExt);
            if (!File.Exists(savFilename))
            {
                return false;
            }
            string destFilename = Path.Combine(tempdir, file + needExt);
            InputFormRef.DoEvents(this, "=>" + Path.GetFileName(savFilename));
            File.Copy(savFilename, destFilename, true);

            return true;
        }

        string MakeReportLog()
        {
            StringBuilder sb = new StringBuilder();
            //User名を消す
            sb.Append(U.TrimPersonalInfomation(Log.LogToString(1024)));
            //情報を書き込む.
            sb.AppendLine("\r\n------\r\n");
            string FEVersion = "";
            if (Program.ROM != null)
            {
                FEVersion = Program.ROM.RomInfo.VersionToFilename();
                FEVersion += " @ROMSize: " + Program.ROM.Data.Length;

                U.CRC32 crc32 = new U.CRC32();
                FEVersion += " @CRC32: " + U.ToHexString8(crc32.Calc(Program.ROM.Data));
            }

            sb.AppendLine(typeof(U).Assembly.GetName().Name + ":" + U.getVersion());
            sb.AppendLine("FEVersion:" + FEVersion);
            sb.AppendLine("Emu:" + OptionForm.GetEmulatorNameOnly());

            sb.AppendLine("MapID:");
            sb.AppendLine(U.ToHexString(this.X_MapID.Value) + ":" + this.X_MapName.Text);

            sb.AppendLine("UnitID:");
            sb.AppendLine(U.ToHexString(this.X_UnitID.Value) + ":" + this.X_UnitName.Text);

            //ユーザが書いた問題点
            sb.AppendLine("Problem:");
            sb.AppendLine(Problem.Text);
            return sb.ToString();
        }

        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }

        private void ToolProblemReportForm_Load(object sender, EventArgs e)
        {
        }

        private void URLTextBoxEx_DoubleClick(object sender, EventArgs e)
        {
            MainFormUtil.GotoCommunities();
        }

        private void AttachDataSelectButton_Click(object sender, EventArgs e)
        {
            this.AttachDataFilename.ErrorMessage = "";

            string title = R._("添付データ");
            string filter = R._("All files|*");

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
            if (U.GetFileSize(open.FileName) > 2 * 1024 * 1024)
            {
                this.AttachDataFilename.ErrorMessage = R._("サイズが大きすぎます。2MBまでにしてください。\r\n間違えてROMを添付できないように、サイズを制限しています。");
                return;
            }


            this.AttachDataFilename.Text = open.FileName;
        }

        private void ToolProblemReportForm_Shown(object sender, EventArgs e)
        {
            StartButton.Focus();
        }
        void SetExplain()
        {
            X_MapIDLabel.AccessibleDescription = R._("どの章でエラーが発生しますか？\r\n問題を再現するためには、どの章で誰が何をすると、どんな問題が発生するのかを知る必要があります。\r\nどの章で問題が発生するかを記載しない人があまりに多いので、項目を追加しました。");
        }

        private void AboutReport7zText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MainFormUtil.GotoReport7zURL();
        }
    }
}
