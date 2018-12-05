using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolSubtitleOverlayForm : Form
    {
        public ToolSubtitleOverlayForm()
        {
            InitializeComponent();
        }

        private void ToolSubtitleOverlayForm_Load(object sender, EventArgs e)
        {

        }

        string SimpleTranslateToTranslateDataFilename;
        Dictionary<string,string> TransDic;
        int ShortLength;
        bool ShowAlways;
        public void Init(string simpleTranslateToTranslateDataFilename,Dictionary<string, string> transDic, int shortLength, bool showAlways)
        {
            this.SimpleTranslateToTranslateDataFilename = simpleTranslateToTranslateDataFilename;
            this.CurrentText = "";
            this.CurrentSubtile = "";
            this.IsHideForm = false;
            this.TransDic = transDic;
            this.ShortLength = shortLength;
            this.ShowAlways = showAlways;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Text = R._("字幕");
        }

        string CurrentText;
        string CurrentSubtile;
        int CurrentPoint;
        bool IsHideForm;

        int CountOf0010;
        int CountOf00800004;
        int CountOf0003;

        public void ShowSubtile(string text, int startPoint, int endPoint)
        {
            if (text.Length < this.ShortLength)
            {//短すぎる.
                HideForm();
                return;
            }

            if (CurrentText == text)
            {
                if (CurrentPoint == startPoint)
                {//変更なし
                    return;
                }
            }
            else
            {
                CurrentText = text;
                if (!TransDic.TryGetValue(text.ToUpper(), out CurrentSubtile))
                {
                    CurrentSubtile = "";
                }
                this.Subtile.Text = TextForm.ConvertEscapeText(this.CurrentSubtile);
            }

            CurrentPoint = startPoint;

            UpdateCount(startPoint, endPoint);

            PickupLine();

            ShowForm();
        }
        void ShowForm()
        {
            if (this.IsHideForm)
            {
                if (!this.ShowAlways)
                {//ウィンドウを表示する.
                    this.Opacity = 1.0;
                }
                this.IsHideForm = false;
            }
        }
        void HideForm()
        {
            if (!this.ShowAlways)
            {
                this.Opacity = 0.01;
                this.IsHideForm = true;
            }
        }

        void PickupLine()
        {
            string code0010;
            string code00800004;
            string code0003 = TextForm.GetLineBreak();
            if (OptionForm.text_escape() == OptionForm.text_escape_enum.FEditorAdv)
            {
                code0010 = "[LoadFace]";
                code00800004 = "[LoadOverworldFaces]";
            }
            else
            {
                code0010 = "@0010";
                code00800004 = "@0080@0004";
            }

            int snipPoint;
            string r = TextForm.ConvertEscapeText(this.CurrentSubtile);
            r = r.Replace("\r\n", "\n");
            if (this.CountOf00800004 > 0)
            {
                r = PickupLineSub(r, this.CountOf00800004, code00800004, out snipPoint);
            }
            else
            {
                r = PickupLineSub(r, this.CountOf0010, code0010, out snipPoint);
            }

            int snipPoint2;
            r = PickupLineSub(r, this.CountOf0003, code0003, out snipPoint2);

            int startPoint = snipPoint + snipPoint2;

            //キーワードハイライトト
            TextForm.KeywordHighLight(this.Subtile);

            Color displayBackColor = OptionForm.Color_NotifyWrite_BackColor();
            Color displayForeColor = OptionForm.Color_NotifyWrite_ForeColor();
            //表示部分の選択
            this.Subtile.SelectionStart = startPoint;
            this.Subtile.SelectionLength = r.Length;
            this.Subtile.SelectionColor = displayForeColor;
            this.Subtile.SelectionBackColor = displayBackColor;

            //選択位置の調整
            this.Subtile.SelectionStart = startPoint;
            this.Subtile.SelectionLength = 0;
        }
        string PickupLineSub(string subtileText, int searchPoint, string keyword, out int out_p)
        {
            int p = 0;
            for (int i = 0; true; i++)
            {
                int f = subtileText.IndexOf(keyword, p);
                if (f < 0)
                {
                    out_p = p;
                    return subtileText.Substring(p);
                }
                if (i >= searchPoint)
                {
                    out_p = p;
                    return subtileText.Substring(p, f - p);
                }
                p = f + keyword.Length;
            }
        }

        void UpdateCount(int startPoint, int endPoint)
        {
            //英訳すると、@0003の数がかわってしまうことがあるため、
            //まず、@0010の相対位置で引っ掛けて、
            //その中から@0003を探索します.
            int first0010 = this.CurrentText.IndexOf("@0010");
            int first00800004 = this.CurrentText.IndexOf("@0080@0004");

            int snipPoint;
            string r;
            if (first0010 <= 0 && first00800004 > 0)
            {//ワールドマップイベントとしてパースしてみる
                this.CountOf0010 = 0;
                r = PickupLineSub2(this.CurrentText, startPoint, "@0080@0004", out this.CountOf00800004, out snipPoint);
            }
            else
            {//会話イベント
                this.CountOf00800004 = 0;
                r = PickupLineSub2(this.CurrentText, startPoint, "@0010", out this.CountOf0010, out snipPoint);
            }

            int newStartPoint = startPoint - snipPoint;
            if (newStartPoint < 0)
            {
                newStartPoint = 0;
            }
            r = PickupLineSub2(r, newStartPoint, "@0003", out this.CountOf0003, out snipPoint);
            Log.Debug("Subtile", startPoint.ToString() , this.CountOf0010.ToString(), this.CountOf0003.ToString());

            if (startPoint == endPoint)
            {//既にネストさせているので、これ以上は無理.
                return;
            }

            if (first0010 < 0 && first00800004 < 0)
            {//@0010 と @0080@0004が一つもない
                return;
            }

            //なぜかたくさん@0010があると、先頭の0がヒットしてしまうことがある.
            //そうなると会話冒頭のメッセージが表示できないので、抜け道を作る
            r = TextForm.StripAllCode(r);
            if (r != "")
            {
                return;
            }
            //終端をベースに、もう一回取り直す.
            UpdateCount(endPoint - 1,endPoint - 1);
        }
        string PickupLineSub2(string subtileText, int startPoint, string keyword, out int out_i, out int out_p)
        {
            int p = 0;
            for (int i = 0; true; i++)
            {
                int f = subtileText.IndexOf(keyword, p);
                if (f < 0)
                {
                    out_i = i;
                    out_p = p;
                    return subtileText.Substring(p);
                }
                if (f >= startPoint)
                {
                    out_i = i;
                    out_p = p;
                    return subtileText.Substring(p, f - p);
                }
                p = f + keyword.Length;
            }
        }

        private void ToolSubtitleOverlayForm_Activated(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void Subtile_DoubleClick(object sender, EventArgs e)
        {

        }

        //タイトルバーがないので、どこでもドラッグできるようにする.
        //see https://dobon.net/vb/dotnet/form/moveform.html
        //マウスのクリック位置を記憶
        private Point MousePoint;

        private void Subtile_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                this.MousePoint = new Point(e.X, e.Y);
            }
        }

        private void Subtile_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - this.MousePoint.X;
                this.Top += e.Y - this.MousePoint.Y;
            }
        }

        private void Subtile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }
    }
}
