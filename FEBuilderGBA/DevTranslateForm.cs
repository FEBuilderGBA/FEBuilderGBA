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
    public partial class DevTranslateForm : Form
    {
        public DevTranslateForm()
        {
            InitializeComponent();
            InputFormRef.LoadComboResource(toLang, U.ConfigDataFilename("func_lang_"));
            InputFormRef.LoadComboResource(toLang2, U.ConfigDataFilename("func_lang_"));
        }

#if DEBUG
        public static void CommandLineTranslateOnly()
        {
            StackFrame sf = new StackFrame(1, true);
            string path = Path.GetDirectoryName(sf.GetFileName());
            Debug.Assert(path != "");

            {//英語
                string lang = "en";
                MyTranslateBuild t = new MyTranslateBuild(lang, false);
                t.ScanPatch();
                t.ScanMOD();
                t.ScanData();
                t.ScanCS(path);
            }
            {//中国語
                string lang = "zh";
                MyTranslateBuild t = new MyTranslateBuild(lang, false);
                t.ScanPatch();
                t.ScanMOD();
                t.ScanData();
                t.ScanCS(path);
            }
        }
#endif
        private void TranslateButton_Click(object sender, EventArgs e)
        {
            string lang = U.SelectValueComboboxText(toLang.Text);
            if (lang == "auto" || lang == "ja" || lang == "")
            {
                R.ShowStopError("翻訳言語を指定してください。\r\njaとautoは選択できません。");
                return;
            }
            string path = SelectSourceCodeDirectory();
            if (path == "")
            {
                return;
            }
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                MyTranslateBuild t = new MyTranslateBuild(lang, TranslateCheckBox.Checked);
                t.ScanPatch();
                t.ScanMOD();
                t.ScanData();
                t.ScanCS(path);
            }

            R.ShowWarning("翻訳完了。ツールを再起動してください。");
        }

        private void DevTranslateForm_Load(object sender, EventArgs e)
        {

        }

        string SelectSourceCodeDirectory()
        {
            string title = R._("ソースコードのディレクトリを選択してください");
            string filter = R._("C#|DevTranslateForm.cs|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "";
            }
            if (!U.CanReadFileRetry(open))
            {
                return "";
            }
            string path = Path.GetDirectoryName(open.FileName);
            return path;
        }

        private void DesignStringConvertButton_Click(object sender, EventArgs e)
        {
            string lang = U.SelectValueComboboxText(toLang2.Text);
            if (lang == "auto" || lang == "ja" || lang == "")
            {
                R.ShowStopError("翻訳言語を指定してください。\r\njaとautoは選択できません。");
                return;
            }

            string path = SelectSourceCodeDirectory();
            if (path == "")
            {
                return;
            }
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                MyTranslateBuild t = new MyTranslateBuild(lang);
                t.DesignStringConvert(path);
            }
            
        }

        private void DesignStringReverseButton_Click(object sender, EventArgs e)
        {
            string lang = U.SelectValueComboboxText(toLang2.Text);
            if (lang == "auto" || lang == "ja" || lang == "")
            {
                R.ShowStopError("翻訳言語を指定してください。\r\njaとautoは選択できません。");
                return;
            }

            string path = SelectSourceCodeDirectory();
            if (path == "")
            {
                return;
            }
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                MyTranslateBuild t = new MyTranslateBuild(lang);
                t.DesignStringReverse(path);
            }
        }
    }
}
