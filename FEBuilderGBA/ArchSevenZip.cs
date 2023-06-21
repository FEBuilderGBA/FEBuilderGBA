using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace FEBuilderGBA
{
    public class ArchSevenZip
    {
        [DllImport("7-zip32.dll", CharSet = CharSet.Ansi)]
        static extern int SevenZip(
            IntPtr hwnd,            // ウィンドウハンドル
            string szCmdLine,       // コマンドライン
            StringBuilder szOutput, // 処理結果文字列
            int dwSize);            // 引数szOutputの文字列サイズ

        public static string Extract(string a7z, string dir, bool isHide)
        {
            try
            {
                string basedir1 = Path.GetDirectoryName(a7z) + "\\";
                string basedir2 = Path.GetDirectoryName(dir) + "\\";
                if (basedir1 == basedir2)
                {
                    string a7z_relativePath = U.GetRelativePath(basedir1, a7z);
                    string dir_relativePath = U.GetRelativePath(basedir2, dir);
                    string errorMessage;
                    using (new U.ChangeCurrentDirectory(basedir1))
                    {
                        errorMessage = ExtractLow(a7z_relativePath, dir_relativePath, isHide);
                    }
                    //いくつかの環境では相対パスでうまくいかないことがあるらしい.
                    if (errorMessage.Length <= 0)
                    {//上手くいった
                        return "";
                    }

                    //絶対パスで再取得.
                }
                return ExtractLow(a7z, dir, isHide);
            }
            catch (Exception e)
            {
                Debug.Assert(false);
                return R.Error("7z解凍中にエラーが発生しました。\r\nターゲットファイル:{0}\r\n{1}", a7z , e.ToString());
            }
        }
        static string ExtractLow(string a7z, string dir, bool isHide)
        {
            string command = "x -y ";
            if (isHide)
            {
                command += "-hide ";
            }
            command += "-o" + "\"" + dir + "\"" + " " + "\"" + a7z + "\"";

            StringBuilder sb = new StringBuilder(1024);
            int r = SevenZip(IntPtr.Zero, command, sb, 1024);
            if (r != 0)
            {
                return sb.ToString();
            }
            return "";
        }
        public static string Compress(string a7z, string target, uint checksize = 1024)
        {
            try
            {
                string basedir1 = Path.GetDirectoryName(a7z) + "\\";
                string basedir2 = Path.GetDirectoryName(target) + "\\";

                if (basedir1 == basedir2)
                {
                    string a7z_relativePath = U.GetRelativePath(basedir1, a7z);
                    string target_relativePath = U.GetRelativePath(basedir2, target);
                    string errorMessage;
                    using (new U.ChangeCurrentDirectory(basedir1))
                    {
                        errorMessage = CompressLow(a7z_relativePath, target_relativePath, checksize);
                    }

                    //いくつかの環境では相対パスでうまくいかないことがあるらしい.
                    if (errorMessage.Length <= 0)
                    {//上手くいった
                        return "";
                    }
                }
                return CompressLow(a7z, target, checksize);
            }
            catch (Exception e)
            {
                Debug.Assert(false);
                return R.Error("7z圧縮中にエラーが発生しました。\r\n{0}", e.ToString());
            }
        }
        static string CompressLow(string a7z, string target , uint checksize)
        {
            string command = "a -hide " + "\"" + a7z + "\"" + " " + "\"" + target + "\"";

            StringBuilder sb = new StringBuilder(1024);
            int r = SevenZip(IntPtr.Zero, command, sb, 1024);
            if (r != 0)
            {//エラー発生
                return sb.ToString();
            }

            if (!File.Exists(a7z))
            {
                return "file not found";
            }
            else if (U.GetFileSize(a7z) < checksize)
            {
                File.Delete(a7z);
                return "file size too short";
            }

            return "";
        }
    }
}
