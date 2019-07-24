using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolUpdateDialogForm : Form
    {
        public ToolUpdateDialogForm()
        {
            InitializeComponent();
        }

        private void UpdateDialog_Load(object sender, EventArgs e)
        {
            FormIcon.Image = SystemIcons.Question.ToBitmap();
        }

        string Version;
        string URL;
        public void Init(string version, string url)
        {
            this.Version = version;
            this.URL = url;

            this.Message.Text = string.Format(this.Message.Text, version, url);
        }


        private void AutoUpdateButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            if (Program.ROM != null && Program.ROM.Modified)
            {
                DialogResult dr = R.ShowQ("未保存の変更があるようです。\r\n保存してもよろしいですか？");
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    MainFormUtil.SaveForce(Program.MainForm());
                }
                else if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //実行中のファイルは上書きできないので、アップデーターに処理を引き継がなくてはいけない。

                string updater_org_txt = System.IO.Path.Combine(Program.BaseDirectory, "config", "data", "updater.bat.txt");
                string updater = Path.Combine(Program.BaseDirectory, "updater.bat");
                if (!File.Exists(updater_org_txt))
                {
                    BrokenDownload(R._("アップデーターのバッチファイルがありません。\r\n{0}",updater_org_txt));
                    this.Close();
                    return;
                }

                try
                {
                    File.Copy(updater_org_txt, updater, true);
                }
                catch (Exception ee)
                {
                    BrokenDownload(R._("アップデーターのバッチファイルをコピーできませんでした。\r\n{0}", ee.ToString()));
                    this.Close();
                    return;
                }

                if (!File.Exists(updater))
                {
                    BrokenDownload(R._("アップデーターのバッチファイルをコピーできませんでした。\r\n{0}", updater));
                    this.Close();
                    return;
                }


                string update7z = Path.Combine(Program.BaseDirectory, "dltemp_" + DateTime.Now.Ticks.ToString() + ".7z");

                //ダウンロード
                try
                {
                    DownloadNewVersion(update7z, this.URL, this.Version, pleaseWait);
                }
                catch (Exception ee)
                {
                    BrokenDownload(ee);
                    this.Close();
                    return;
                }
                if (! File.Exists(update7z))
                {
                    BrokenDownload(R._("ダウンロードしたはずのファイルがありません。"));
                    this.Close();
                    return;
                }
                if (U.GetFileSize(update7z) < 2 * 1024 * 1024)
                {
                    BrokenDownload(R._("ダウンロードしたファイルが小さすぎます。"));
                    this.Close();
                    return;
                }

                pleaseWait.DoEvents("Extract...");

                //解凍
                try
                {
                    string _update = Path.Combine(Program.BaseDirectory, "_update");
                    U.mkdir(_update);
                    string r = ArchSevenZip.Extract(update7z, _update);
                    if (r != "")
                    {
                        BrokenDownload(R._("ダウンロードしたファイルを解凍できませんでした。") + "\r\n" + r);
                        this.Close();
                        return;
                    }
                }
                catch (Exception ee)
                {
                    BrokenDownload(ee);
                    this.Close();
                    return;
                }

                string updateNewVersionFilename = Path.Combine(Program.BaseDirectory, "_update", "FEBuilderGBA.exe");
                if (!File.Exists(updateNewVersionFilename))
                {
                    BrokenDownload(R._("ダウンロードしたファイルを解凍した中に、実行ファイルがありませんでした。"));
                    this.Close();
                    return;
                }
                if (U.GetFileSize(updateNewVersionFilename) < 2 * 1024 * 1024)
                {
                    BrokenDownload(R._("ダウンロードしたファイルを解凍した中にあった、実行ファイルが小さすぎます。"));
                    this.Close();
                    return;
                }

                pleaseWait.DoEvents("GO!");

                int pid = Process.GetCurrentProcess().Id;
                string args = pid.ToString();
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = updater;
                    p.StartInfo.Arguments = args;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                }
                catch (Exception ee)
                {
                    BrokenDownload(ee);
                    return;
                }
            }

            Application.Exit();

            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
        }

        void BrokenDownload(Exception e)
        {
            BrokenDownload(e.ToString());
        }
        void BrokenDownload(string errormessage)
        {
            R.ShowStopError("エラーにより自動アップデートできませんでした。\r\n代わりにURLをブラウザで表示します。\r\n手動でダウンロードしてください。\r\n{0}", errormessage);
            OpenBrower();
        }

        private void OpenBrowserButton_Click(object sender, EventArgs e)
        {
            OpenBrower();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void IgnoreButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        void DownloadNewVersion(string save_filename, string download_url, string version, InputFormRef.AutoPleaseWait pleaseWait)
        {
            if (download_url.IndexOf("getuploader") > 0)
            {
                DownloadNewVersionByGetUploader(save_filename, download_url, version, pleaseWait);
            }
            else
            {
                DownloadNewVersionByGithub(save_filename, download_url, version, pleaseWait);
            }
        }

        void DownloadNewVersionByGithub(string save_filename, string download_url, string version, InputFormRef.AutoPleaseWait pleaseWait)
        {
            string durl = download_url;
            Log.Notify("download url:{0}", durl);
            U.HttpDownload(save_filename, durl, download_url, pleaseWait);
        }

        void DownloadNewVersionByGetUploader(string save_filename,string download_url,string version,InputFormRef.AutoPleaseWait pleaseWait)
        {
            string url = download_url;
            string contents = U.HttpGet(url);
            Log.Debug("front page:{0}", contents);
            contents = U.skip(contents, "name=\"token\"");
            string token = U.cut(contents, "value=\"", "\"");
            token = Uri.UnescapeDataString(token);
            token = U.unhtmlspecialchars(token);
            if (token.Length <= 8)
            {
                Log.Error("token NOT FOUND:", token);
                return;
            }

            Dictionary<string, string> args = new Dictionary<string, string>();
            args["token"] = token;
            contents = U.HttpPost(download_url, args, download_url);
            Log.Debug("download page:{0}", contents);
            contents = U.skip(contents, "http-equiv=\"refresh\"");
            string durl = U.cut(contents, "URL=", "\"");
            durl = Uri.UnescapeDataString(durl);
            durl = U.unhtmlspecialchars(durl);
            if (durl == "" && durl.IndexOf("http") < 0)
            {
                Log.Error("download url NOT FOUND:{0}", durl);
                return;
            }

            Log.Notify("download url:{0}", durl);
            U.HttpDownload(save_filename, durl, download_url, pleaseWait);
        }

        void OpenBrower()
        {
            U.OpenURLOrFile(this.URL);
        }
    }
}
