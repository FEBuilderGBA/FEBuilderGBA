using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace FEBuilderGBA
{
    //色変更ができるcombobox
    //see http://mibc.blog.fc2.com/blog-entry-70.html
    class CustomColorGroupBox : GroupBox
    {
        // 枠色（デフォルト：白）
        private Color borderColor;
 
        // コンストラクタ
        public CustomColorGroupBox()
        {
            this.borderColor = Color.Empty;

            // ダブルバッファリングを有効
            SetStyle(ControlStyles.DoubleBuffer, true);
 
            // グループボックスの描画をオーナードローにする
            SetStyle(ControlStyles.UserPaint, true);
        }

        void ResetColor()
        {
            this.borderColor = OptionForm.Color_Control_ForeColor();
        }

        // プロパティ
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                this.Refresh();
            }
        }
 
        // OnPrintイベント
        protected override void OnPaint(PaintEventArgs e)
        {
            // テキストサイズを取得
            Size tTextSize = TextRenderer.MeasureText(this.Text, this.Font);
 
            // グループボックスの領域を取得
            Rectangle tBorderRect = this.ClientRectangle;
 
            // テキストを考慮（グループボックス枠線がテキスト（高さ）の真ん中に来るように）して枠を描画
            tBorderRect.Y += tTextSize.Height / 2;
            tBorderRect.Height -= tTextSize.Height / 2;
            ControlPaint.DrawBorder(e.Graphics, tBorderRect, borderColor == Color.Empty ? this.ForeColor : borderColor , ButtonBorderStyle.Solid);

            SolidBrush BackBrush = new SolidBrush(this.BackColor);
            SolidBrush ForeBrush = new SolidBrush(this.ForeColor);

            // テキストを描画
            Rectangle tTextRect = this.ClientRectangle;
            tTextRect.X += 6;                           // テキストの描画開始位置(X)をグループボックスの領域から6ドットずらす
            tTextRect.Width = tTextSize.Width + 6;
            tTextRect.Height = tTextSize.Height;
            e.Graphics.FillRectangle(BackBrush, tTextRect);
            e.Graphics.DrawString(this.Text, this.Font, ForeBrush, tTextRect);

            BackBrush.Dispose();
            ForeBrush.Dispose();
        }
    }
}
