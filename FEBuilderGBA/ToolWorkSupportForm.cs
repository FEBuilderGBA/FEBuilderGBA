using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using System.Net;

namespace FEBuilderGBA
{
    public partial class ToolWorkSupportForm : Form
    {
        public ToolWorkSupportForm()
        {
            InitializeComponent();
            Explain();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            //アップデートする準備ができているかどうか。
            if (!CheckReady())
            {
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //更新できますか？
                UPDATE_RESULT ur = CheckUpdate();
                if (ur == UPDATE_RESULT.ERROR)
                {
                    return ;
                }
                if (ur == UPDATE_RESULT.LATEST)
                {
                    if (this.IsSlientMode) return;

                    ToolWorkSupport_UpdateQuestionDialogForm f = (ToolWorkSupport_UpdateQuestionDialogForm)InputFormRef.JumpFormLow<ToolWorkSupport_UpdateQuestionDialogForm>();
                    f.SetVersion(GetUPSDateTimeString());
                    DialogResult dr = f.ShowDialog();
                    if (dr != System.Windows.Forms.DialogResult.Retry)
                    {
                        return;
                    }
                    //強制アップデート!
                }
                //更新の実行
                RunDownloadAndExtract(pleaseWait);
            }
        }

        //アップデートする準備ができているかどうか。
        bool CheckReady()
        {
            if (Program.ROM == null)
            {
                return false;
            }
            if (Program.ROM.IsVirtualROM)
            {//仮想ROMは更新できない
                if (!this.IsSlientMode) R.ShowOK("仮想ROMなので更新できません");
                return false;
            }
            if (Program.ROM.Modified)
            {
                if (this.IsSlientMode)
                {
                    return false;
                }
                DialogResult dr = R.ShowNoYes("警告\r\n変更した内容をROMに保存していませんが、無視してアップデートしてもよろしいですか？");
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return false;
                }
            }
            return true;
        }


        private void CommunityButton_Click(object sender, EventArgs e)
        {
            string url = U.at(this.Lines, "COMMUNITY_URL");
            if (url == "")
            {
                return;
            }
            U.OpenURLOrFile(url);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            string filename = this.Filename;
            if (!File.Exists(filename))
            {
                return;
            }
            U.OpenURLOrFile(filename);
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            LoadInfo();
        }

        Dictionary<string, string> Lines = new Dictionary<string,string>();
        string Filename = "";
        bool IsSlientMode = false;

        public static string GetUpdateInfo(string romfilename)
        {
            string filename = U.ChangeExtFilename(romfilename, ".updateinfo.txt");
            if (File.Exists(filename))
            {
                return filename;
            }

            //余計なものを取り払った探索
            //BSFE_1.0.ups -> BSFE.updateinfo.txt
            //fe8kaitou.en.ups -> fe8kaitou.updateinfo.txt
            filename = Path.GetFileNameWithoutExtension(romfilename);
            List<string> ver = new List<string>();
            for (int i = 0; i < filename.Length; i++)
            {
                if (filename[i] == '.' || filename[i] == '_' || filename[i] == '-' || filename[i] == ' ')
                {
                    ver.Add( filename.Substring(0, i) );
                }
            }

            string basedir = Path.GetDirectoryName(romfilename);
            for (int i = ver.Count - 1; i >= 0; i-- )
            {
                filename = Path.Combine(basedir, ver[i] + ".updateinfo.txt");
                if (File.Exists(filename))
                {
                    return filename;
                }
            }

            //not found
            filename = U.ChangeExtFilename(romfilename, ".updateinfo.txt");
            return filename;
        }

        string ExplainUpdateInfo()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:%E4%BD%9C%E5%93%81%E6%94%AF%E6%8F%B4";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:guid:febuildergba:work_support";
            }
            return url;
        }

        public static Dictionary<string, string>  LoadUpdateInfo(string filename)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                line = line.Trim();

                int sep = line.IndexOf('=');
                if (sep < 0)
                {
                    continue;
                }
                string key = line.Substring(0, sep);
                string value = line.Substring(sep + 1);

                if (key == "")
                {
                    continue;
                }

                ret[key] = value;
            }
            return ret;
        }
        bool Open(string romfilename)
        {
            this.Filename = GetUpdateInfo(romfilename);

            if (! File.Exists(this.Filename))
            {
                this.Lines = new Dictionary<string, string>();
                return false;
            }
            this.Lines = LoadUpdateInfo(this.Filename);
            return true;
        }
        void InitAutoFeedback()
        {
            //0 = 拒否
            //1 = 許可
            //255 = 不明
            uint autofeedback = U.atoi(Program.WorkSupportCache.At(2, "255"));
            if (autofeedback == 255)
            {
                string name = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
                string url = U.at(this.Lines, "AUTOFEEDBACK_URL");
                if (url != "")
                {
                    uint defaultSetting = U.atoi(U.at(this.Lines, "AUTOFEEDBACK_DEFAULT_SETTING","1"));
                    if (defaultSetting == 1)
                    {//ディフォルトは送信
                        autofeedback = 1;
                    }
                    else if (defaultSetting == 0)
                    {//ディフォルトは送信しない
                        autofeedback = 0;
                    }
                    else
                    {//ディフォルトはユーザーに聞く
                        DialogResult dr = R.ShowYesNo("「{0}」を遊んでくれてありがとう。\r\nこのゲームの進行状況を、作者に自動フィードバックしてもよろしいですか？\r\nフィードバックの内容は、ゲーム内の情報だけで、完全に匿名で行います。\r\nあなたのフィードバックは、ゲームの改善と、製作者のモチベーションの向上につながるので、ぜひご協力ください。\r\nこの設定は、「Menu->実行->作品支援」からいつでも変更できます。\r\n自動フィードバックを有効にしてもよろしいですか?\r\n", name);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            autofeedback = 1;
                        }
                        else if (dr == System.Windows.Forms.DialogResult.No)
                        {
                            autofeedback = 0;
                        }
                    }

                    UpdateAutoFeedback(autofeedback);
                }
            }

            bool enableAutoFeedback = (autofeedback == 1);
            AutoFeedback.Init(this.Lines, enableAutoFeedback);
        }
        void UpdateAutoFeedback(uint autofeedback)
        {
            Program.WorkSupportCache.Update(2, autofeedback.ToString());
            Program.WorkSupportCache.Save(Path.GetFileNameWithoutExtension(Program.ROM.Filename));
        }

        public static ToolWorkSupportAutoFeedback AutoFeedback = new ToolWorkSupportAutoFeedback();
        static public bool UseAutoFeedback()
        {
            return AutoFeedback.GetIsAutoFeedBack();
        }

        private void WorkSupport_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        void LoadInfo()
        {
            InfoTextBox.Text = "";
            this.IsSlientMode = false;
            if (Program.ROM == null)
            {
                return ;
            }
            if (Program.ROM.IsVirtualROM)
            {//仮想ROMは更新できない
                return ;
            }
            bool r = Open(Program.ROM.Filename);
            if (!r)
            {
                InfoTextBox.Text += R._("このプロジェクトには、updateinfo.txtが作成されていません。\r\n作成する方法は、以下のURLをご覧ください。\r\n") + ExplainUpdateInfo() + "\r\n";
            }

            InitAutoFeedback();
            this.NameTextBox.Text = GetName();
            this.AuthorTextBox.Text = U.at(this.Lines, "AUTHOR");
            this.CommunityTextBox.Text = U.at(this.Lines, "COMMUNITY_URL");
            this.VersionTextBox.Text = GetUPSDateTimeString();

            InfoTextBox.Text += this.Filename + "\r\n";

            LOGO.Image = MakeLogo();
            SetToggleAutoFeedbackButton();
        }
        void SetToggleAutoFeedbackButton()
        {
            if (! AutoFeedback.GetAutoFeedBackEnable())
            {
                ToggleAutoFeedbackButton.Hide();
            }
            else
            {
                ToggleAutoFeedbackButton.Show();
            }

            if (AutoFeedback.GetIsAutoFeedBack())
            {
                ToggleAutoFeedbackButton.Text = R._("自動フィードバックを無効にする");
            }
            else
            {
                ToggleAutoFeedbackButton.Text = R._("自動フィードバックを有効にする");
            }
        }
        string GetName()
        {
            string name = U.at(this.Lines, "NAME");
            if (name != "")
            {
                return name;
            }
            return Path.GetFileNameWithoutExtension(Program.ROM.Filename);
        }
        Image MakeLogo()
        {
            string logofilename = U.at(this.Lines, "LOGO_FILENAME");
            string romfilename = Path.GetDirectoryName(this.Filename);

            return MakeLogoLow(logofilename , romfilename);
        }

        public static Image MakeLogoLow(string logofilename, string romfilename)
        {
            logofilename = Path.Combine(romfilename, logofilename);
            if (!File.Exists(logofilename))
            {
                return MakeLogoNoLogo();
            }

            try
            {
                //ファイルがロックされないようにCloneしてから渡します
                Bitmap bitmap = new Bitmap(logofilename);
                Bitmap retimage = ImageUtil.CloneBitmap(bitmap);
                bitmap.Dispose();
                return retimage;
            }
            catch (Exception)
            {
                return MakeLogoNoLogo();
            }
        }
        static Image MakeLogoNoLogo()
        {
            Icon icon = Properties.Resources.Icon1;
            return icon.ToBitmap();
        }

        static DateTime GetROMDateTime(string romfilename)
        {
            DateTime dt = U.GetFileDateLastWriteTime(romfilename);
            string filename = U.ChangeExtFilename(romfilename, ".ups");
            if (File.Exists(filename))
            {
                DateTime ups_dt = U.GetFileDateLastWriteTime(filename);
                if (ups_dt > dt)
                {//新しい方を採用
                    dt = ups_dt;
                }
            }
            return dt;
        }
        string GetUPSDateTimeString()
        {
            string filename = U.ChangeExtFilename(Program.ROM.Filename, ".ups");
            if (File.Exists(filename))
            {
                DateTime ups_dt = U.GetFileDateLastWriteTime(filename);
                return ups_dt.ToString();
            }
            //UPSがない場合は、ROMの派ージョンを返す.
            DateTime dt = U.GetFileDateLastWriteTime(Program.ROM.Filename);
            return dt.ToString() + "(ROM)";
        }



        void RunDownloadAndExtract(InputFormRef.AutoPleaseWait pleaseWait)
        {
            string url = U.at(this.Lines, "UPDATE_URL");
            if (url == "")
            {
                R.ShowStopError("UPDATE_URLの項目がありません");
                return ;
            }

            string regex = U.at(this.Lines, "UPDATE_REGEX");
            if (regex == "")
            {
                R.ShowStopError("UPDATE_REGEXの項目がありません");
                return ;
            }

            if (regex == "@DIRECT_URL")
            {//直リン
            }
            else
            {
                string html = U.HttpGet(url);
                Match m = RegexCache.Match(html, regex);
                if (m.Groups.Count < 2)
                {
                    R.ShowStopError("UPDATE_REGEXでマッチしたデータがありませんでした。\r\n{0}", html);
                    return;
                }
                url = m.Groups[1].ToString();
            }

            Program.UpdateWatcher.Stop();
            //少し時間がかかるので、しばらくお待ちください表示.
            bool r = DownloadAndExtract(url, pleaseWait);
            if (!r)
            {
                Program.UpdateWatcher.Resume();
                return;
            }

            R.ShowOK("アップデートが完了しました。\r\nROMを再読み込みします。\r\n");
            MainFormUtil.Open(this, Program.ROM.Filename, true, "");

            Program.UpdateWatcher.Resume();
        }

        bool DownloadAndExtract(string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            string romdir = Path.GetDirectoryName(Program.ROM.Filename);
            string update7z = Path.GetTempFileName();

            //ダウンロード
            try
            {
                U.DownloadFile(update7z, download_url, pleaseWait);
            }
            catch (Exception ee)
            {
                BrokenDownload(R.ExceptionToString(ee));
                return false;
            }
            if (!File.Exists(update7z))
            {
                BrokenDownload(R._("ダウンロードしたはずのファイルがありません。"));
                return false;
            }
            if (U.GetFileSize(update7z) <= 256)
            {
                BrokenDownload(R._("ダウンロードしたファイルが小さすぎます。"));
                File.Delete(update7z);
                return false;
            }

            pleaseWait.DoEvents("Extract...");

            if (UPSUtil.IsUPSFile(update7z))
            {
                string upsName = Path.Combine(romdir, RecomendUPSName(download_url));
                File.Copy(update7z, upsName, true);
            }
            else
            {
                //解凍
                try
                {
                    using (U.MakeTempDirectory t = new U.MakeTempDirectory())
                    {
                        string r = ArchSevenZip.Extract(update7z, t.Dir);
                        if (r != "")
                        {
                            BrokenDownload(R._("ダウンロードしたファイルを解凍できませんでした。") + "\r\n" + r);
                            return false;
                        }
                        U.CopyDirectory1Trim(t.Dir, romdir);
                    }
                }
                catch (Exception ee)
                {
                    BrokenDownload(R.ExceptionToString(ee));
                    File.Delete(update7z);
                    return false;
                }
            }
            File.Delete(update7z);
            pleaseWait.DoEvents("Select Vanilla ROM");

            string[] ups_files = U.Directory_GetFiles_Safe(romdir, "*.ups", SearchOption.AllDirectories);
            if (ups_files.Length <= 0)
            {
                BrokenDownload(R._("UPSファイルが見つかりませんでした") );
                return false;
            }

            ToolWorkSupport_SelectUPSForm f = (ToolWorkSupport_SelectUPSForm)InputFormRef.JumpFormLow<ToolWorkSupport_SelectUPSForm>();
            f.OpenUPS(ups_files[0]);
            if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return false;
            }

            pleaseWait.DoEvents("UPS");
            string orignalROMFilename = f.GetOrignalFilename();
            if (orignalROMFilename == "")
            {
                return false;
            }

            for (int i = 0; i < ups_files.Length; i++)
            {
                ROM rom = new ROM();
                string version;
                bool rr = rom.Load(orignalROMFilename, out version);
                if (!rr)
                {
                    R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                    return false;
                }

                rr = UPSUtil.ApplyUPS(rom, ups_files[i]);
                if (!rr)
                {
                    R.ShowStopError("UPSパッチを適応できませんでした" + "\r\n" + ups_files[i]);
                }

                string savegba = U.ChangeExtFilename(ups_files[i], ".gba");
                rom.Save(savegba, true);
            }

            pleaseWait.DoEvents("ReOpen...");
            MainFormUtil.ForceReopen();
            return true;
        }
        void BrokenDownload(string errormessage)
        {
            R.ShowStopError("エラーにより自動アップデートできませんでした。\r\n{0}", errormessage);
        }
        string RecomendUPSName(string url)
        {
            string filename = Path.GetFileName(url);
            if (filename.IndexOf(".ups") > 0)
            {
                return filename;
            }
            return U.ChangeExtFilename(Program.ROM.Filename, "ups");
        }

        public enum UPDATE_RESULT
        {
            ERROR
            , LATEST
            , UPDATEABLE
        }
        UPDATE_RESULT CheckUpdate()
        {
            return CheckUpdateLow(this.Lines,Program.ROM.Filename, this.IsSlientMode);
        }
        public static UPDATE_RESULT CheckUpdateLow(Dictionary<string,string> lines,string romfilename, bool isSlientMode)
        {
            string url = U.at(lines, "CHECK_URL");
            if (url == "")
            {
                if (!isSlientMode)
                {
                    R.ShowStopError("CHECK_URLの項目がありません");
                }
                return UPDATE_RESULT.ERROR;
            }

            string regex = U.at(lines, "CHECK_REGEX");
            if (regex == "")
            {
                if (!isSlientMode)
                {
                    R.ShowStopError("CHECK_REGEXの項目がありません");
                }
                return UPDATE_RESULT.ERROR;
            }

            string dateString;
            string match;
            if (regex == "@DIRECT_URL")
            {
                match = url;
            }
            else
            {
                string html;
                try
                {
                    html = U.HttpGet(url);
                }
                catch (Exception e)
                {
                    R.ShowStopError("更新を確認しようとしたら、Webサイト({0})がエラーを返しました。\r\n{1}", url, e.ToString());
                    return UPDATE_RESULT.LATEST;
                }

                Match m = RegexCache.Match(html, regex);
                if (m.Groups.Count < 2)
                {
                    if (!isSlientMode)
                    {
                        R.ShowStopError("CHECK_REGEXでマッチしたデータがありませんでした。\r\n{0}", html);
                    }
                    return UPDATE_RESULT.ERROR;
                }
                match = m.Groups[1].ToString();
            }

            if (U.isURL(match))
            {//直URL
                WebResponse rsp;
                try
                {
                    rsp = U.HttpHead(url);
                }
                catch(Exception e)
                {
                    R.ShowStopError("更新を確認しようとしたら、Webサイト({0})がエラーを返しました。\r\n{1}", url, e.ToString());
                    return UPDATE_RESULT.LATEST;
                }

                dateString = rsp.Headers["LastModified"];
                if (dateString == null)
                {
                    if (!isSlientMode)
                    {
                        R.ShowStopError("Last-Modifiedを検知できませんでした。\r\n仕方がないので、常に最新版に更新します。");
                    }
                    dateString = DateTime.Now.ToString();
                }
            }
            else
            {
                dateString = match;
            }

            DateTime datetime;
            try
            {
                if (url.IndexOf("getuploader.com") >= 0)
                {
                    datetime = DateTime.Parse(dateString, new CultureInfo("ja-JP", false));
                }
                else
                {
                    if (U.TryParseUnitTime(dateString, out datetime))
                    {//unittmie
                    }
                    else
                    {//通常の日付
                        datetime = DateTime.Parse(dateString);
                    }
                }
            }
            catch(Exception )
            {
                if (!isSlientMode)
                {
                    R.ShowStopError("日付({0})をパースできませんでした。\r\n仕方がないので、常に最新版に更新します。", dateString);
                }
                datetime = DateTime.Now;
            }

            DateTime romDatetime = GetROMDateTime(romfilename);
            Log.Notify("romDatetime:{0} vs datetime:{1}", romDatetime.ToString(), datetime.ToString());
            if (romDatetime < datetime)
            {//更新する必要あり!
                return UPDATE_RESULT.UPDATEABLE;
            }
            //更新する必要なし
            return UPDATE_RESULT.LATEST;
        }


        private void UpdateThreadCallBackEvent(object sender, EventArgs e)
        {
            bool wasSlientMode = this.IsSlientMode;

            this.IsSlientMode = false;
            //ダイアログを表示して、更新ボタンを押す.
            this.Show();
            InputFormRef.DoEvents();

            string msg = R._("このゲームの新しいバージョンが公開されています。\r\n最新版に自動アップデートしますか？");
            if (wasSlientMode)
            {
                msg += "\r\n\r\n" + R._("自動更新をキャンセルした後に、手動で更新したい場合は、「メニュー」->「実行」->「作品支援」を利用してください。");
            }
            DialogResult dr = R.ShowYesNo(msg);
            if (dr != System.Windows.Forms.DialogResult.No)
            {
                this.UpdateButton.PerformClick();
            }
        }

        static public void CheckUpdateSlientByThread()
        {
            if (Program.ROM == null)
            {
                return;
            }
            if (Program.ROM.IsVirtualROM)
            {//仮想ROMは更新できない
                return;
            }
            if (UpdateCheck.IsAutoUpdateTime()
                || InputFormRef.GetForm<ToolUpdateDialogForm>() != null)
            {//FEBuilderGBAの更新があるので、今は自動確認してはいけない。
                return;
            }

            ToolWorkSupportForm f = (ToolWorkSupportForm)InputFormRef.JumpFormLow<ToolWorkSupportForm>();
            f.IsSlientMode = true;
            f.CheckUpdateSlientMain(Program.ROM.Filename);
        }
        void CheckUpdateSlientMain(string romfilename)
        {
            if (!Open(romfilename))
            {
                return;
            }
            InitAutoFeedback();
            if (!CheckUpdateTodayAndOverraideTime(romfilename))
            {
                return;
            }

            System.Threading.Thread s1 = new System.Threading.Thread(t =>
            {
                try
                {
                    if (this.IsDisposed)
                    {
                        return;
                    }
                    //更新できますか？
                    UPDATE_RESULT ur = CheckUpdate();
                    if (ur != UPDATE_RESULT.UPDATEABLE)
                    {
                        return;
                    }
                    //イベント通知
                    if (Application.OpenForms.Count <= 0)
                    {//通知するべきフォームがない.
                        return;
                    }
                    Form f = Application.OpenForms[0];
                    if (f == null || f.IsDisposed)
                    {
                        return;
                    }
                    EventHandler callback = UpdateThreadCallBackEvent;
                    f.Invoke(callback, new object[] { this, null });
                }
                catch(Exception e)
                {
                    Log.Error(e.ToString());
                    return;
                }
            });
            s1.Start();
        }
        //更新チェックをしてもよいか?
        bool CheckUpdateTodayAndOverraideTime(string romfilename)
        {
            int func_auto_update = OptionForm.auto_update();
            if (func_auto_update == 0)
            {//自動アップデート確認をしない.
                RegistWorkSupportIfNotRegist(romfilename);
                return false;
            }

            DateTime dt = DateTime.Now.AddDays(-func_auto_update);
            uint now = U.atoi(dt.ToString("yyyyMMdd"));

            uint romDate = U.atoi(U.GetFileDateLastWriteTime(romfilename).ToString("yyyyMMdd"));

            uint LastUpdateCheck = U.atoi(Program.WorkSupportCache.At(1));
            if (romDate > LastUpdateCheck)
            {//ROMの方が新しいなら上書き.
                LastUpdateCheck = romDate;
            }
            if (now <= LastUpdateCheck)
            {//まだアップデート確認する時間じゃない.
                RegistWorkSupportIfNotRegist(romfilename);
                return false;
            }

            //アップデートチェック時間の上書き
            Program.WorkSupportCache.Update(0, romfilename);
            Program.WorkSupportCache.Update(1, now.ToString());
            Program.WorkSupportCache.Save(Path.GetFileNameWithoutExtension(romfilename));
            return true;
        }
        void RegistWorkSupportIfNotRegist(string romfilename)
        {
            if (Program.WorkSupportCache.At(0) != romfilename)
            {//WorkSupportCacheに登録がないならば作成します.
                Program.WorkSupportCache.Update(0, romfilename);
                Program.WorkSupportCache.Update(1, "0");
                Program.WorkSupportCache.Save(Path.GetFileNameWithoutExtension(romfilename));
            }
        }

#if DEBUG
        public static void TEST_Parse1()
        {
            string html = File.ReadAllText( Program.GetTestData("test_getuploader.txt"));
            {
                string regex = "fe8_kaitou.*?MB</td><td>(.+?)</td><td>";

                Match m = RegexCache.Match(html, regex);
                string r = m.Groups[1].ToString();
                Debug.Assert(r == "20/01/29 13:22");
            }
            {
                string regex = "<td><a href=\"(.+?)\" title=\"fe8_kaitou.+?\">";

                Match m = RegexCache.Match(html, regex);
                string r = m.Groups[1].ToString();
                Debug.Assert(r == "https://ux.getuploader.com/FE4/download/1310");
            }
        }
        public static void TEST_DateParseTest()
        {
            {
                string a = "Expires: Wed, 27 Jan 2021 04:03:09 GMT\r\nLast-Modified: Mon, 27 Jan 2020 07:06:32 GMT\r\nVary: Accept-Encoding";
                string regex = "Last-Modified: (.+?)\r\n";
                Match m = RegexCache.Match(a, regex);
                string r = m.Groups[1].ToString();
                Debug.Assert(r == "Mon, 27 Jan 2020 07:06:32 GMT");
            }
        }
#endif //DEBUG


        string MakeFeedBackFilename()
        {
            string ups = "feedback." + DateTime.Now.ToString("yyyyMMddHHmmss");
            string upsFullPath = U.MakeFilename(ups, ".feedback.7z");
            return upsFullPath;
        }
        string MakeFeedBack(string fullfilename)
        {
            //テンポラリディレクトリを利用する
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                //セーブデータの回収
                ToolProblemReportForm.CollectSaveData(tempdir.Dir);

                //フィードバックコメント
                File.WriteAllText(Path.Combine(tempdir.Dir, "log.txt"), MakeFeedBackInfo());

                //7z圧縮
                InputFormRef.DoEvents(this, R._("7z圧縮中"));
                return ArchSevenZip.Compress(fullfilename, tempdir.Dir);
            }
        }

        string MakeFeedBackInfo()
        {
            StringBuilder sb = new StringBuilder();
            //情報を書き込む.
            sb.AppendLine("\r\n------\r\n");
            string FEVersion = "";
            FEVersion = Program.ROM.RomInfo.VersionToFilename;
            FEVersion += " @ROMSize: " + Program.ROM.Data.Length;

            sb.AppendLine(typeof(U).Assembly.GetName().Name + ":" + U.getVersion());
            sb.AppendLine("FEVersion:" + FEVersion);
            sb.AppendLine("Emu:" + OptionForm.GetEmulatorNameOnly());

            DateTime dt = U.GetFileDateLastWriteTime(Program.ROM.Filename);
            sb.AppendLine("ROM DateTime:" + dt.ToString());

            string filename = U.ChangeExtFilename(Program.ROM.Filename, ".ups");
            if (File.Exists(filename))
            {
                DateTime ups_dt = U.GetFileDateLastWriteTime(filename);
                sb.AppendLine("UPS DateTime:" + ups_dt.ToString());
            }

            return sb.ToString();
        }

        private void DcoumentButton_Click(object sender, EventArgs e)
        {
            string url = ExplainUpdateInfo();
            U.OpenURLOrFile(url);
        }

        private void SnowAllWorksButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolAllWorkSupportForm>();
        }

        void Explain()
        {
            UpdateButton.AccessibleDescription = R._("このゲームを最新版に更新します。");
            CommunityButton.AccessibleDescription = R._("開発コミニティにアクセスします。\r\nゲームへの感想やフィードバックレポートを送りプロジェクトに貢献しましょう。");
            ToggleAutoFeedbackButton.AccessibleDescription = R._("自動フィードバックレポートを送るかどうかを設定します。");
        }

        private void ToggleAutoFeedbackButton_Click(object sender, EventArgs e)
        {
            AutoFeedback.SetAutoFeedBackStatus(! AutoFeedback.GetIsAutoFeedBack());
            SetToggleAutoFeedbackButton();

            uint autoFeedback = 0;
            if (AutoFeedback.GetIsAutoFeedBack())
            {
                autoFeedback = 1;
            }
            UpdateAutoFeedback(autoFeedback);
        }
    }
}
