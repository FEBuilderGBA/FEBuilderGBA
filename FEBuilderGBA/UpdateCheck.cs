using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    class UpdateCheck
    {
        public static void CheckUpdateUI()
        {
            string download_url;
            string net_version;

            string error = CheckUpdateURLByGitHub(out download_url, out net_version);
            if (error != "")
            {
                if (net_version != "")
                {//バージョンが取れたということは、現在のが最新
                    OverradeLastUpdateTime();
                    R.ShowOK(error);
                    return;
                }
                else
                {//何かエラーが発生
                    R.ShowStopError(error);
                    return;
                }
            }
            CheckUpdateUI(download_url, net_version);
        }
        static void CheckUpdateUI(string download_url, string net_version)
        {
            //まずアップデートした日付を記録する
            OverradeLastUpdateTime();

            //確認ダイアログの表示
            ToolUpdateDialogForm f = (ToolUpdateDialogForm)InputFormRef.JumpFormLow<ToolUpdateDialogForm>();
            f.Init(net_version,download_url);
            f.ShowDialog();
        }

        public class UpdateEventArgs : EventArgs
        {
            public string error;
            public string download_url;
            public string net_version;
        }
        public static void CheckUpdateUI(UpdateEventArgs e)
        {
            if (e.error != "")
            {
                if (e.net_version != "")
                {//バージョンが取れたということは、現在のが最新
                    OverradeLastUpdateTime();
                    return;
                }
                else
                {//何かエラーが発生
                    return;
                }
            }
            
            //開いているフォームの中にUpdateDialogFormはすでにあるか？
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                Form f = Application.OpenForms[i];
                if (f.Name == "UpdateDialogForm")
                {//すでにアップデート処理中
                    return;
                }
            }

            CheckUpdateUI(e.download_url, e.net_version);
        }

        static void OverradeLastUpdateTime()
        {
            string yyyymmdd = DateTime.Now.ToString("yyyyMMdd");
            Program.Config["LastUpdateCheck"] = yyyymmdd;
            //Configのセーブ.
            Program.Config.Save();
        }
        //自動アップデート確認をしてもいいの?
        public static bool IsAutoUpdateTime()
        {
            int func_auto_update = OptionForm.auto_update();
            if (func_auto_update == 0)
            {//自動アップデート確認をしない.
                return false;
            }

            DateTime dt = DateTime.Now.AddDays(-func_auto_update);
            uint now = U.atoi(dt.ToString("yyyyMMdd"));
            uint LastUpdateCheck = U.atoi(Program.Config.at("LastUpdateCheck", "0"));
            if (now <= LastUpdateCheck)
            {//まだアップデート確認する時間じゃない.
                return false;
            }
            return true;
        }

        public EventHandler EventHandler;
        public void CheckUpdateThread()
        {
            if (!IsAutoUpdateTime())
            {//まだアップデート確認する時間じゃない.
                return;
            }

            System.Threading.Thread s1 = new System.Threading.Thread(t =>
            {
                try
                {
                    string download_url;
                    string net_version;
                    string error = CheckUpdateURLByGitHub(out download_url, out net_version);

                    UpdateEventArgs args = new UpdateEventArgs();
                    args.error = error;
                    args.download_url = download_url;
                    args.net_version = net_version;

                    if (Application.OpenForms.Count <= 0)
                    {//通知するべきフォームがない.
                        return;
                    }
                    Form f = Application.OpenForms[0];
                    if (f == null || f.IsDisposed )
                    {
                        return;
                    }
                    f.Invoke(EventHandler, new object[] { this, args });
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    return;
                }
            });
            s1.Start();
        }

        static string CheckUpdateURLByGitHub(out string out_url, out string out_version)
        {
            out_url = "";
            out_version = "";

            string versionString = U.getVersion();
            double version = U.atof(versionString);

            string url = "https://api.github.com/repos/FEBuilderGBA/FEBuilderGBA/releases/latest";
            string contents;
            try
            {
                contents = U.HttpGet(url);
            }
            catch (Exception e)
            {
#if DEBUG
                R.Error("Webサイトにアクセスできません。 URL:{0} Message:{1}", url, e.ToString());
                throw;
#else
                return R.Error("Webサイトにアクセスできません。 URL:{0} Message:{1}", url, e.ToString());
#endif
            }


            string downloadurl;
            {
                System.Text.RegularExpressions.Match match = RegexCache.Match(contents
                , "\"browser_download_url\": \"(.+)\""
                );
                if (match.Groups.Count < 2)
                {
                    return R._("サイトの結果が期待外でした。\r\n{0}", url) + "\r\n\r\n" 
                        + "browser_download_url not found" + "\r\n"
                        + "contents:\r\n" +  contents + "\r\n"
                        + "match.Groups:\r\n" +  U.var_dump(match.Groups);
                }
                downloadurl = match.Groups[1].Value;
            }

            {
                System.Text.RegularExpressions.Match match = RegexCache.Match(contents
                , "download/ver_([0-9.]+)/"
                );
                if (match.Groups.Count < 2)
                {
                    return R._("サイトの結果が期待外でした。\r\n{0}", url) + "\r\n\r\n" 
                        + "download/ver_ not found" + "\r\n"
                        + "contents:\r\n" + contents + "\r\n"
                        + "match.Groups:\r\n" + U.var_dump(match.Groups);
                }
                out_version = match.Groups[1].Value;

                double net_version = U.atof(out_version);
                if (version >= net_version)
                {
                    if (net_version == 0)
                    {
                        return R._("サイトの結果が期待外でした。\r\n{0}", url) + "\r\n\r\n" 
                            + "version can not parse" + "\r\n"
                            + "contents:\r\n" + contents + "\r\n"
                            + "match.Groups:\r\n" + U.var_dump(match.Groups);
                    }
                    return R._("現在のバージョンが最新です。version:{0}", version);
                }
            }

            out_url = downloadurl;
            return "";
        }

        static string CheckUpdateURLByGetUploader(out string out_url, out string out_version)
        {
            out_url = "";
            out_version = "";

            string versionString = U.getVersion();
            double version = U.atof(versionString);

            string url = "https://ux.getuploader.com/FE4/";
            string contents;
            try
            {
                contents = U.HttpGet(url);
            }
            catch (Exception e)
            {
#if DEBUG
                R.Error("Webサイトにアクセスできません。 URL:{0} Message:{1}", url, e.ToString());
                throw;
#else
                return R.Error("Webサイトにアクセスできません。 URL:{0} Message:{1}", url, e.ToString());
#endif
            }

            System.Text.RegularExpressions.Match match = RegexCache.Match(contents
            , "<td><a href=\"(https://ux.getuploader.com/FE4/download/[0-9]+)\" title=\"FEBuilder(?:GBA)?_[0-9.]+.7z\">FEBuilder(?:GBA)?_([0-9.]+).7z</a></td><td></td><td>FEBuilder(?:GBA)?_[0-9.]+.7z</td><td>(2|3|4)\\.[0-9] MB</td>"
            );
            Log.Error(U.var_dump(match.Groups));
            if (match.Groups.Count < 2)
            {
                return R._("サイトの結果が期待外でした。\r\n{0}", url) + "\r\n\r\n"
                    + "href can not parse" + "\r\n"
                    + "contents:\r\n" + contents + "\r\n"
                    + "match.Groups:\r\n" + U.var_dump(match.Groups);
            }

            out_version = match.Groups[2].Value;
            double net_version = U.atof(match.Groups[2].Value);
            if (version >= net_version)
            {
                if (net_version == 0)
                {
                    return R._("サイトの結果が期待外でした。\r\n{0}", url) + "\r\n\r\n"
                        + "version can not parse" + "\r\n"
                        + "contents:\r\n" + contents + "\r\n"
                        + "match.Groups:\r\n" + U.var_dump(match.Groups);
                }
                return R._("現在のバージョンが最新です。version:{0}", version);
            }

            double yyyymmdd = U.atof(DateTime.Now.AddDays(3).ToString("yyyyMMdd.HH"));
            if (net_version > yyyymmdd)
            {//いたずらで変な日付のものが挙げられた可能性あり
                return R._("サイトの結果が期待外でした。\r\n{0}", url) + "\r\n\r\n"
                    + "date can not parse" + "\r\n"
                    + "contents:\r\n" + contents + "\r\n"
                    + "match.Groups:\r\n" + U.var_dump(match.Groups);
            }

            out_url = match.Groups[1].Value;
            return "";
        }

    }
}
