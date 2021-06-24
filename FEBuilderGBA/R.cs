using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    //string.Format(Properties.Resources,,,)と書くのが面倒なので短縮形をつくる.
    static class R
    {
        public static string Notify(string str, params object[] args)
        {
            string s = MyTranslateResource.str(str, args);
            Log.Notify(s);
            return s;
        }
        public static string _(string str, params object[] args)
        {
            if (args.Length > 0)
            {
                string s = MyTranslateResource.str(str, args);
                return s;
            }
            else
            {
                return MyTranslateResource.str(str);
            }
        }
        public static string Error(string str, params object[] args)
        {
            string s = MyTranslateResource.str(str, args);
            Log.Error(s);
            return s;
        }
        public static string Debug(string str, params object[] args)
        {
            string s = MyTranslateResource.str(str, args);
#if DEBUG
            Log.Debug(s);
#endif
            return s;
        }
        public static string ExceptionToString(System.Runtime.InteropServices.ExternalException e)
        {
            return R._("GDI+ Exceptionが発生しました。\r\nもう一度試してください。\r\n何度も再発する場合は、report7zを送ってください。\r\nErrorCode:{0} {1}\r\nMessage:\r\n{2}\r\n{3}", e.ErrorCode, U.HRESULTtoString(e.ErrorCode), e.ToString(), e.StackTrace);
        }
        public static string ExceptionToString(System.UnauthorizedAccessException e)
        {
            return R._("System.UnauthorizedAccessExceptionが発生しました。\r\n何度も再発する場合は、report7zを送ってください。\r\nMessage:\r\n{0}\r\n{1}", e.ToString(), e.StackTrace);
        }
        public static string ExceptionToString(System.OutOfMemoryException e)
        {
            return R._("System.OutOfMemoryExceptionが発生しました。\r\n何度も再発する場合は、report7zを送ってください。\r\nMessage:\r\n{0}\r\n{1}", e.ToString(), e.StackTrace);
        }
        public static string ExceptionToString(System.Exception e)
        {
            return R._("例外が発生しました。\r\n何度も再発する場合は、report7zを送ってください。\r\nMessage:\r\n{0}\r\n{1}", e.ToString(), e.StackTrace);
        }

        //エラーメッセージ OKだけ
        public static void ShowStopError(string str, params object[] args)
        {
            string message = R.Error(MyTranslateResource.str(str, args));
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return;
            }

            if (U.CountLines(message) > 10 || message.IndexOf("https://") >= 0)
            {
                try
                {
                    ErrorLongMessageDialogForm f = (ErrorLongMessageDialogForm)InputFormRef.JumpFormLow<ErrorLongMessageDialogForm>();
                    f.SetErrorMessage(message);
                    f.ShowDialog();

                    return;
                }
                catch (Exception e)
                {
                    Log.Error(R.ExceptionToString(e));
                }
            }

            try
            {
                string title = R._("エラー");
                MessageBox.Show(message
                    , title
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                Log.Error(R.ExceptionToString(e));
            }
        }
        public static void ShowStopError(string str)
        {
            string message = R.Error(MyTranslateResource.str(str));
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return;
            }

            if (U.CountLines(message) > 10 || message.IndexOf("https://") >= 0)
            {
                try
                {
                    ErrorLongMessageDialogForm f = (ErrorLongMessageDialogForm)InputFormRef.JumpFormLow<ErrorLongMessageDialogForm>();
                    f.SetErrorMessage(message);
                    f.ShowDialog();

                    return;
                }
                catch (Exception e)
                {
                    Log.Error(R.ExceptionToString(e));
                }
            }

            try
            {
                string title = R._("エラー");
                MessageBox.Show(message
                    , title
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                Log.Error(R.ExceptionToString(e));
            }
        }

        static string ClipIfVeryLong(string str)
        {
            if (str.Length >= 2048)
            {
                return str.Substring(0, 2048) + "...";
            }
            return str;
        }
        public static void ShowStopError(string str,Exception ex)
        {
            ShowStopError("{0}\r\n\r\n{1}:\r\n{2}", ex.GetType().ToString(), ex.ToString());
        }

        //警告メッセージ OKだけ
        public static void ShowWarning(string str, params object[] args)
        {
            try
            {
                string message = R.Notify(str, args);
                string title = R._("警告");
                MessageBox.Show(ClipIfVeryLong(message)
                    , title
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                Log.Error(R.ExceptionToString(e));
            }
        }
        public static void ShowWarning(string str)
        {
            try
            {
                string message = R.Notify(str);
                string title = R._("警告");
                MessageBox.Show(ClipIfVeryLong(message)
                    , title
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                Log.Error(R.ExceptionToString(e));
            }
        }

        //YES NO CALCEL
        public static DialogResult ShowQ(string str, params object[] args)
        {
            string message = R.Notify(str, args);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.Yes;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.YesNoCancel
                , MessageBoxIcon.Exclamation);
        }
        public static DialogResult ShowQ(string str)
        {
            string message = R.Notify(str);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.OK;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.YesNoCancel
                , MessageBoxIcon.Exclamation);
        }

        public static DialogResult ShowYesNo(string str, params object[] args)
        {
            string message = R.Notify(str, args);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.Yes;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Exclamation);
        }
        public static DialogResult ShowYesNo(string str)
        {
            string message = R.Notify(str);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.Yes;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question);
        }
        public static DialogResult ShowNoYes(string str, params object[] args)
        {
            string message = R.Notify(str, args);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.Yes;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Exclamation
                , MessageBoxDefaultButton.Button2);
        }
        public static DialogResult ShowNoYes(string str)
        {
            string message = R.Notify(str);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.Yes;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Exclamation
                , MessageBoxDefaultButton.Button2);
        }
        public static DialogResult ShowOK(string str, params object[] args)
        {
            string message = R.Notify(str, args);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.OK;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information);
        }
        public static DialogResult ShowOK(string str)
        {
            string message = R.Notify(str);
            if (Program.IsCommandLine)
            {
                U.echo(message);
                return DialogResult.OK;
            }
            return MessageBox.Show(ClipIfVeryLong(message)
                , ""
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information);
        }



    }
}
