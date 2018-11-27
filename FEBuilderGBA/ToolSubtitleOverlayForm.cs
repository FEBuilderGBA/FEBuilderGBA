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

        Dictionary<string,string> TransDic;
        int ShortLength;
        bool ShowAlways;
        public void Init(Dictionary<string,string> transDic
            , int shortLength, bool showTitleBar , bool showAlways)
        {
            this.CurrentText = "";
            this.CurrentSubtile = "";
            this.IsHideForm = false;
            this.TransDic = transDic;
            this.ShortLength = shortLength;
            this.ShowAlways = showAlways;

            if (showTitleBar)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }
            this.Text = R._("字幕");
        }

        string CurrentText;
        string CurrentSubtile;
        int CurrentPoint;
        bool IsHideForm;

        public void ShowSubtile(string text,int startPoint)
        {
            if (text.Length < this.ShortLength)
            {//短すぎる.
                HideForm();
                return;
            }

            int showLines = 0;
            if (CurrentText == text)
            {
                if (CurrentPoint == startPoint)
                {//変更なし
                    return;
                }
                CurrentPoint = startPoint;
                showLines = CalcLine(startPoint);
            }
            else
            {
                CurrentText = text;
                if (!TransDic.TryGetValue(text, out CurrentSubtile))
                {
                    CurrentSubtile = "";
                }
                CurrentPoint = 0;
            }

            string subtile = PickupLine(showLines);
            this.Subtile.Text = TextForm.StripAllCode(subtile);

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
                this.Opacity = 0.3;
                this.IsHideForm = true;
            }
        }

        string PickupLine(int showLines)
        {
            string lineBreak = "@0003";
            int p = 0;
            for (int i = 0; true; i++)
            {
                int f = this.CurrentSubtile.IndexOf(lineBreak, p);
                if (f < 0)
                {
                    return this.CurrentSubtile.Substring(p);
                }
                if (i >= showLines)
                {
                    return this.CurrentSubtile.Substring(p, f - p);
                }
                p = f + lineBreak.Length;
            }
        }
        int CalcLine(int showLines)
        {
            string lineBreak = "@0003";
            int p = 0;
            for(int i = 0 ; true ; i ++)
            {
                int f = this.CurrentText.IndexOf(lineBreak, p);
                if (f < 0)
                {
                    return i;
                }
                if (f >= showLines)
                {
                    return i;
                }
                p = f + lineBreak.Length;
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
    }
}
