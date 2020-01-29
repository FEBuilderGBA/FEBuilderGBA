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
    public partial class ToolWorkSupport : Form
    {
        public ToolWorkSupport()
        {
            InitializeComponent();
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
                if (!CheckUpdate(pleaseWait))
                {
                    R.ShowOK("現在のバージョンが最新です。version:{0}", GetUPSDateTimeString());
                    return;
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
                R.ShowOK("仮想ROMなので更新できません");
                return false;
            }
            if (Program.ROM.Modified)
            {
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
            U.OpenURLOrFile(url);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            string filename = this.FilenameTextBox.Text;
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

        string GetUpdateInfo()
        {
            string filename = U.ChangeExtFilename(Program.ROM.Filename, ".updateinfo.txt");
            if (File.Exists(filename))
            {
                return filename;
            }

            //余計なものを取り払った探索
            //BSFE_1.0.ups -> BSFE.updateinfo.txt
            //fe8kaitou.en.ups -> fe8kaitou.updateinfo.txt
            filename = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
            List<string> ver = new List<string>();
            for (int i = 0; i < filename.Length; i++)
            {
                if (filename[i] == '.' || filename[i] == '_' || filename[i] == '-' || filename[i] == ' ')
                {
                    ver.Add( filename.Substring(0, i) );
                }
            }

            string basedir = Path.GetDirectoryName(Program.ROM.Filename);
            for (int i = ver.Count - 1; i >= 0; i-- )
            {
                filename = Path.Combine(basedir, ver[i] + ".updateinfo.txt");
                if (File.Exists(filename))
                {
                    return filename;
                }
            }

            //not found
            filename = U.ChangeExtFilename(Program.ROM.Filename, ".updateinfo.txt");
            return filename;
        }

        bool Open()
        {
            this.Filename = GetUpdateInfo();
            this.Lines = new Dictionary<string, string>();

            if (! File.Exists(this.Filename))
            {
                R.ShowStopError("このプロジェクトには、updateinfo.txtが作成されていません。\r\n作成する方法は、以下のURLをご覧ください。\r\nhttps://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:%E4%BD%9C%E5%93%81%E6%94%AF%E6%8F%B4");
                return false;
            }

            string[] lines = File.ReadAllLines(this.Filename);
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

                Lines[key] = value;
            }
            return true;
        }

        private void WorkSupport_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        void LoadInfo()
        {
            Open();

            this.NameTextBox.Text = GetName();
            this.AuthorTextBox.Text = U.at(this.Lines, "AUTHOR");
            this.CommunityTextBox.Text = U.at(this.Lines, "COMMUNITY_URL");
            this.VersionTextBox.Text = GetUPSDateTimeString();

            FilenameTextBox.Text = this.Filename;

            LOGO.Image = MakeLogo();
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
            logofilename = Path.Combine( Path.GetDirectoryName(this.Filename) , logofilename);
            if (!File.Exists(logofilename))
            {
                return MakeLogoNoLogo();
            }

            try
            {
                return Image.FromFile(logofilename);
            }
            catch (Exception)
            {
                return MakeLogoNoLogo();
            }
        }
        Image MakeLogoNoLogo()
        {
            Icon icon = Properties.Resources.Icon1;
            return icon.ToBitmap();
        }

        DateTime GetCurrentDateTime()
        {
            DateTime dt = U.GetFileDateLastWriteTime(Program.ROM.Filename);
            string filename = U.ChangeExtFilename(Program.ROM.Filename, ".ups");
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
            string html = U.HttpGet(url);

            Match m = RegexCache.Match(html, regex);
            if (m.Groups.Count < 2)
            {
                R.ShowStopError("UPDATE_REGEXでマッチしたデータがありませんでした。\r\n{0}", html);
                return ;
            }
            string match = m.Groups[1].ToString();
            if (U.isURL(match))
            {//直URL
                url = match;
            }
            else
            {
                m = RegexCache.Match(html, regex);
                if (m.Groups.Count < 2)
                {
                    R.ShowStopError("UPDATE_REGEXでマッチしたデータをパースできませんでした。\r\n{0}", html);
                    return ;
                }
                url = m.Groups[1].ToString();
            }

            //少し時間がかかるので、しばらくお待ちください表示.
            DownloadAndExtract(url, pleaseWait);

            R.ShowOK("アップデートが完了しました。\r\nROMを再読み込みします。\r\n");
            MainFormUtil.Open(this, Program.ROM.Filename, true, "");
        }

        void DownloadAndExtract(string download_url, InputFormRef.AutoPleaseWait pleaseWait)
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
                return;
            }
            if (!File.Exists(update7z))
            {
                BrokenDownload(R._("ダウンロードしたはずのファイルがありません。"));
                return;
            }
            if (U.GetFileSize(update7z) <= 256)
            {
                BrokenDownload(R._("ダウンロードしたファイルが小さすぎます。"));
                File.Delete(update7z);
                return;
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
                            return;
                        }
                        U.CopyDirectory1Trim(t.Dir, romdir);
                    }
                }
                catch (Exception ee)
                {
                    BrokenDownload(R.ExceptionToString(ee));
                    File.Delete(update7z);
                    return;
                }
            }
            File.Delete(update7z);
            pleaseWait.DoEvents("UPS!");

            string[] ups_files = U.Directory_GetFiles_Safe(romdir, "*.ups", SearchOption.AllDirectories);
            if (ups_files.Length <= 0)
            {
                BrokenDownload(R._("UPSファイルが見つかりませんでした") );
                return;
            }

            ToolWorkSupport_SelectUPSForm f = (ToolWorkSupport_SelectUPSForm)InputFormRef.JumpFormLow<ToolWorkSupport_SelectUPSForm>();
            f.OpenUPS(ups_files[0]);
            if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string orignalROMFilename = f.GetOrignalFilename();
            if (orignalROMFilename == "")
            {
                return;
            }

            for (int i = 0; i < ups_files.Length; i++)
            {
                ROM rom = new ROM();
                string version;
                bool rr = rom.Load(orignalROMFilename, out version);
                if (!rr)
                {
                    R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                    return;
                }

                rr = UPSUtil.ApplyUPS(rom, ups_files[i]);
                if (!rr)
                {
                    R.ShowStopError("UPSパッチを適応できませんでした" + "\r\n" + ups_files[i]);
                }

                string savegba = U.ChangeExtFilename(ups_files[i], ".gba");
                rom.Save(savegba, true);
            }

            pleaseWait.DoEvents("END");
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

        bool CheckUpdate(InputFormRef.AutoPleaseWait pleaseWait)
        {
            string url = U.at(this.Lines, "CHECK_URL");
            if (url == "")
            {
                R.ShowStopError("CHECK_URLの項目がありません");
                return false;
            }

            string regex = U.at(this.Lines, "CHECK_REGEX");
            if (regex == "")
            {
                R.ShowStopError("CHECK_REGEXの項目がありません");
                return false;
            }
            string html = U.HttpGet(url);

            Match m = RegexCache.Match(html, regex);
            if (m.Groups.Count < 2)
            {
                R.ShowStopError("CHECK_REGEXでマッチしたデータがありませんでした。\r\n{0}",html);
                return false;
            }
            string dateString;
            string match = m.Groups[1].ToString();
            if (U.isURL(match))
            {//直URL
                WebResponse rsp = U.HttpHead(url);
                dateString = rsp.Headers["LastModified"];
                if (dateString == null)
                {
                    R.ShowStopError("Last-Modifiedを検知できませんでした。\r\n仕方がないので、常に最新版に更新します。");
                    dateString = DateTime.Now.ToString();
                }
            }
            else
            {
                dateString = m.Groups[1].ToString();
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
                    datetime = DateTime.Parse(dateString);
                }
            }
            catch(Exception )
            {
                R.ShowStopError("日付({0})をパースできませんでした。\r\n仕方がないので、常に最新版に更新します。", dateString);
                datetime = DateTime.Now;
            }

            DateTime romDatetime = GetCurrentDateTime();
            Log.Notify("romDatetime:{0} vs datetime:{1}", romDatetime.ToString(), datetime.ToString());
            if (romDatetime < datetime)
            {//更新する必要あり!
                return true;
            }
            //更新する必要なし
            return false;
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

                DateTime d = DateTime.Parse(r);
                r = d.ToString();
                Debug.Assert(r == "2020/01/27 16:06:32");
            }
            {
                string r = "20/01/27 20:04";

                DateTime d = DateTime.Parse(r, new CultureInfo("ja-JP", false));
                r = d.ToString();
                Debug.Assert(r == "2020/01/27 20:04:00");
            }
        }
#endif //DEBUG

        private void MakeErrorReportButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            DialogResult dr = R.ShowYesNo("ゲームのセーブデータを圧縮して、フィードバックレポートを作成します。\r\nセーブデータがあれば、状況の確認や、ゲームバランスの確認にとても役に立ちます。\r\n");
            if (dr != DialogResult.Yes)
            {
                return;
            }

            string title = R._("フィードバックを保存するファイル名を選択してください");
            string filter = R._("report.7z|*.report.7z|All files|*");
            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, MakeFeedBackFilename());

            dr = save.ShowDialog();
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
                string error = MakeFeedBack(save.FileNames[0]);
                if (error != "")
                {
                    R.ShowStopError("レポートを作れませんでした。\r\n{0}", error);
                    return;
                }
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileNames[0]);
            R.ShowOK("フィードバックレポートを作成しました。\r\nこのファイルをコミニティに送信してください。");
        }

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
            FEVersion = Program.ROM.RomInfo.VersionToFilename();
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

    }
}
