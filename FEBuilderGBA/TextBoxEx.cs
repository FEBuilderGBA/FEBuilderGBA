using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FEBuilderGBA
{
    public class TextBoxEx : TextBox
    {
        public TextBoxEx()
        {
            this.DoubleBuffered = true;
        }
        protected override void Dispose(bool disposing)
        {
        }

        protected override bool ProcessCmdKey(ref Message msg,Keys keyData)
        {
            if (this._Placeholder != "" && this.TextLength <= 0)
            {
                this.Invalidate();
            }

            if (keyData == (Keys.Control | Keys.A))
            {//全選択
                this.SelectAll();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        //see
        //http://blog.goo.ne.jp/hahafire/e/741f9a0d60bb5f2f855e076ce583af4c
        // IMEでキーが押されたかのフラグ
        private const int WM_IME_COMPOSITION = 0x010F;
        // 変換確定後文字取得に使用する値(ひらがな)
        private const int GCS_RESULTSTR = 0x0800;

        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 15) //  WM_PAINT == 15
            {
                using (var g = this.CreateGraphics())
                {
                    if (this.BackgroundImage != null)
                    {
                        g.DrawImage(this.BackgroundImage, 0, 0);
                    }
                    else if (this.Enabled && !this.ReadOnly && (_Placeholder.Length > 0) && (this.TextLength == 0))
                    {
                        // 描画を一旦消してしまう
                        Brush b = new System.Drawing.SolidBrush(this.BackColor);
                        g.FillRectangle(b, this.ClientRectangle);
                        b.Dispose();

                        // プレースホルダのテキスト色を、前景色と背景色の中間として文字列を描画する
                        var placeholderTextColor = System.Drawing.Color.FromArgb(
                                (this.ForeColor.A + this.BackColor.A) / 2
                            , (this.ForeColor.R + this.BackColor.R) / 2
                            , (this.ForeColor.G + this.BackColor.G) / 2
                            , (this.ForeColor.B + this.BackColor.B) / 2);
                        b = new System.Drawing.SolidBrush(placeholderTextColor);
                        g.DrawString(_Placeholder, this.Font, b, 1, 1);
                        b.Dispose();
                    }

                    //エラーがあれば枠の色を変える.
                    if (_ErrorMessage.Length > 0)
                    {
                        Pen pen = new Pen(OptionForm.Color_Error_ForeColor(), 3);
                        g.DrawRectangle(pen, this.ClientRectangle);
                        pen.Dispose();
                    }
                }
            }
        }

        //https://qiita.com/otagaisama-1/items/ec46796d5e93f5e6ccb2
        private string _Placeholder = "";

        //プレースフォルダ
        public string Placeholder
        {
            get { return _Placeholder; }
            set
            {
                if (_Placeholder != value)
                {
                    _Placeholder = value;
                    Invalidate();
                }
            }
        }
        string _ErrorMessage = "";
        //エラーメッセージ
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                if (_ErrorMessage != value)
                {
                    _ErrorMessage = value;
                    Invalidate();
                    Update();
                }
            }
        }

        public void SetToolTipEx(ToolTipEx tooltip)
        {
            this._ToolTipEx = tooltip;
        }
        ToolTipEx _ToolTipEx;


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_ErrorMessage.Length > 0 && _ToolTipEx != null)
            {
                _ToolTipEx.HideEvent(this, e);
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_ErrorMessage.Length > 0 && _ToolTipEx != null)
            {
                _ToolTipEx.FireEvent(this, _ErrorMessage);

                //通常のポップアップメッセージが出てしまうので抑制する.
                //base.OnMouseMove(e);
            }
            else
            {
                base.OnMouseMove(e);
            }
        }

        public void AllowDropFilename()
        {
            this.AllowDrop = true;
            this.DragEnter += (sender, e) =>
            {
                //ファイルがドラッグされている場合、カーソルを変更する
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            };
            this.DragDrop += (sender, e) =>
            {
                //ドロップされたファイルの一覧を取得
                string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (fileName.Length <= 0)
                {
                    return;
                }

                //TextBoxの内容をファイル名に変更
                this.Text = fileName[0];
            };

            this.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllSystemSources;
            this.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        }
    }
}
