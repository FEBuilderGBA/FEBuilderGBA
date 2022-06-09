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
    public partial class ToolSubtitleSetingDialogForm : Form
    {
        public ToolSubtitleSetingDialogForm()
        {
            InitializeComponent();
            SimpleTranslateFromROMFilename.AllowDropFilename();
            SimpleTranslateToROMFilename.AllowDropFilename();
            SimpleTranslateToTranslateDataFilename.AllowDropFilename();
        }

        string FromLangString;
        string ToLangString;
        int ShortLength;
        private void ToolSubtitleSetingDialogForm_Load(object sender, EventArgs e)
        {
            int from, to;
            TranslateTextUtil.TranslateLanguageAutoSelect(out from, out to);

            String dir = Path.GetDirectoryName(Program.ROM.Filename);
            FromLangString = TranslateTextUtil.GetLangIntCodeToLangText(from);
            SimpleTranslateFromROMFilename.Text = MainFormUtil.FindOrignalROMByLang(dir, FromLangString);

            ToLangString = TranslateTextUtil.GetLangIntCodeToLangText(to);
            SimpleTranslateToROMFilename.Text = MainFormUtil.FindOrignalROMByLang(dir, ToLangString);

            if (Program.ROM.RomInfo.is_multibyte)
            {
                ShortLength = 10;
            }
            else
            {
                ShortLength = 20;
            }
        }
        private void TranslateFormROMFilenameSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("定型文を抽出するROMを選択してください");
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
            this.SimpleTranslateFromROMFilename.Text = open.FileNames[0];
        }

        private void TranslateToROMFilenameSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("定型文を抽出するROMを選択してください");
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
            this.SimpleTranslateToROMFilename.Text = open.FileNames[0];
        }
        private void SimpleTranslateToTranslateDataFilenameButton_Click(object sender, EventArgs e)
        {
            string title = R._("翻訳データを指定してください");
            string filter = R._("Text|*.txt|All files|*");

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
            this.SimpleTranslateToTranslateDataFilename.Text = open.FileNames[0];
        }

        private void SimpleTranslateToTranslateDataFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SimpleTranslateToTranslateDataFilenameButton.PerformClick();
        }

        private void TranslateFormROMFilename_DoubleClick(object sender, EventArgs e)
        {
            SimpleTranslateFormROMFilenameSelectButton.PerformClick();
        }

        private void TranslateToROMFilename_DoubleClick(object sender, EventArgs e)
        {
            SimpleTranslateToROMFilenameSelectButton.PerformClick();
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        Dictionary<string, string> MakeTransDic()
        {
            Dictionary<string, string> transDic = TranslateTextUtil.MakeFixedDic(FromLangString, ToLangString , SimpleTranslateFromROMFilename.Text , SimpleTranslateToROMFilename.Text);
            TranslateTextUtil.AppendDicFixedFile(transDic, SimpleTranslateToTranslateDataFilename.Text);

            return transDic;
        }

        static ToolSubtitleOverlayForm g_SubTitleOverlay;
        public static bool OptionTextSubtile()
        {
            ToolSubtitleSetingDialogForm f = (ToolSubtitleSetingDialogForm)InputFormRef.JumpFormLow<ToolSubtitleSetingDialogForm>();
            f.ShowDialog();

            if (f.DialogResult == DialogResult.OK)
            {
                if (g_SubTitleOverlay == null)
                {
                    g_SubTitleOverlay = (ToolSubtitleOverlayForm)InputFormRef.JumpFormLow<ToolSubtitleOverlayForm>();
                    g_SubTitleOverlay.FormClosed += (ss, ee) =>
                    {
                        g_SubTitleOverlay = null;
                    };
                }

                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait())
                {
                    g_SubTitleOverlay.Init(f.SimpleTranslateToTranslateDataFilename.Text, f.MakeTransDic(), f.ShortLength,  f.ShowAlways.Checked);
                }
                g_SubTitleOverlay.Show();
            }
            else
            {
                if (g_SubTitleOverlay != null)
                {
                    g_SubTitleOverlay.Close();
                }
                g_SubTitleOverlay = null;
            }
            return g_SubTitleOverlay != null;
        }

        public static void ShowSubtile(string text,int startPoint,int endPoint)
        {
            if (g_SubTitleOverlay == null)
            {//未初期化
                return;
            }
            g_SubTitleOverlay.ShowSubtile(text, startPoint, endPoint);
        }

        public static void CloseSubTile()
        {
            if (g_SubTitleOverlay == null)
            {//未初期化
                return;
            }
            g_SubTitleOverlay.Close();
            g_SubTitleOverlay = null;
        }
    }
}
