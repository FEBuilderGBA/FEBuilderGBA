using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Drawing;

namespace FEBuilderGBA
{
    public partial class ErrorReportForm : Form
    {
        public ErrorReportForm()
        {
            InitializeComponent();
            ErrorIcon.Image = SystemIcons.Error.ToBitmap();
        }

        void CheckNeedUpdateMsg()
        {
            bool r = VersionForm.IsOldVersion();
            if (!r)
            {
                MainTab.SelectedIndex = 0;
                MainTab.TabPages.Remove(this.NeedUpdatePage);
                return;
            }
            X_NEED_UPDATE.Text = VersionForm.MakeUpdateMessage();
            MainTab.SelectedTab = this.NeedUpdatePage;
        }


        static string ReadbleStackTrace(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            StackTrace trace = new StackTrace(ex, true);
            foreach (StackFrame frame in trace.GetFrames())
            {
                MethodBase m = frame.GetMethod();
                if (m == null 
                    || m.DeclaringType.FullName.IndexOf("FEBuilderGBA") < 0)
                {
                    continue;
                }
                sb.Append(m.DeclaringType.Name.ToString());
                sb.Append(".cs ");
                sb.Append(m.ToString());
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void SetException(Exception ex,string inputformref_debuginfo)
        {
            string threadname = Thread.CurrentThread.Name;
            string FEVersion = "";
            if (Program.ROM != null)
            {
                FEVersion = Program.ROM.RomInfo.VersionToFilename;
                FEVersion += " @ROMSize:" + Program.ROM.Data.Length;
            }

            string errorMessage = "ErrorMessage:" + ex.GetType().ToString() + " " + ex.Message + "\r\n"
              + typeof(U).Assembly.GetName().Name + ":" + U.getVersion() + "\r\n"
              + "FEVersion:" + FEVersion + "\r\n"
              + inputformref_debuginfo
              + "MessageName:" + threadname + "\r\n"
            ;
            string stacktrace = ReadbleStackTrace(ex);

            Exception inex = ex.InnerException;
            while (inex != null)
            {
                stacktrace += "---------------\r\n"
                 + "Message:" + inex.Message
                 + "StackTrace:\n" + ReadbleStackTrace(inex);
                inex = inex.InnerException;
            }

            //ログに書く.
            try
            {
                Log.Error("ERROR!");
                Log.Error("ERROR:");
                Log.Error(errorMessage);
                Log.Error("STACKTRACE:");
                Log.Error(stacktrace);
                Log.SyncLog();
            }
            catch(Exception)
            {
            }
            this.ErrorMessage.Text = U.TrimPersonalInfomation(errorMessage);
            this.StackTrace.Text = U.TrimPersonalInfomation(stacktrace);

            this.UserMessage.Focus();
        }


        private void ErrorReportForm_Load(object sender, EventArgs e)
        {
            //システムエラーを鳴らす
            System.Media.SystemSounds.Hand.Play();

            //エラーが発生した場合、次回起動時にアップデートチェックするようにしてみよう.
            Program.Config["LastUpdateCheck"] = "0";
            Program.Config.Save();

            CheckNeedUpdateMsg();
            this.UserMessage.Placeholder = R._("例:戦闘アニメをインポートしようとしたら、エラーが表示された。\r\n試した戦闘アニメは、 http://mydata.example.com/aaa/ から、ダウンロードしたものだ。\r\nバグってるだろうおおおお\r\n");
            this.UserMessage.Focus();
            System.Media.SystemSounds.Hand.Play();
        }

        private void SendErrorMessageButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                pleaseWait.DoEvents();
                
                {
                    Dictionary<string, string> args = new Dictionary<string, string>();
                    args["ifq"] = "";
                    args["entry.189843227"] = this.UserMessage.Text;
                    args["entry.1065468019"] = this.ErrorMessage.Text;
                    args["entry.61988854"] = this.StackTrace.Text;
                    args["submit"] = "Submit";

                    U.HttpPost("https://docs.google.com/forms/d/e/1FAIpQLSekzQBwAMXBuNaFxx0VsYQeDq7oJRmZH3lIreDVJr4_2N-xYw/formResponse", args);
                }
            }

            R.ShowOK("エラー報告ありがとうございます。\r\n\r\nバグの原因がわかったら、ソフトウェアのアップデートしたいと思います。");
            this.Close();
        }

    }
}
