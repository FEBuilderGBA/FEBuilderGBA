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
    public class RichTextBoxEx : RichTextBox
    {

        public RichTextBoxEx()
        {
            this.DoubleBuffered = true;
            ClearUndoBuffer();
            MakeContextMenu();
        }

        protected override bool ProcessCmdKey(ref Message msg,Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.J))
            {
                return false;
            }
            else if (keyData == (Keys.Control | Keys.Z))
            {
                RunUndo();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Y))
            {
                RunRedo();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.A))
            {
                this.SelectAll();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.C))
            {//NOP コピペ
            }
            else if (keyData == (Keys.Control | Keys.V))
            {//コピペ
                if (!Clipboard.ContainsText())
                {
                    return true;
                }
            }
            else if (keyData == Keys.Space
                || (keyData >= Keys.D0 && keyData <= Keys.Z)
                || (keyData >= Keys.NumPad0 && keyData <= Keys.NumPad9)
                || (keyData >= Keys.Oem1 && keyData <= Keys.OemBackslash)
                || (keyData == Keys.Delete || keyData == Keys.Return || keyData == Keys.Back || keyData == Keys.Tab)
            )
            {
                //変更される場合、UNDOに積む.
                PushUndo();
            }
            if (this._Placeholder != "" && this.TextLength <= 0)
            {
                this.Invalidate();
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
            if (m.Msg == WM_IME_COMPOSITION)
            {
                if (((int)m.LParam & GCS_RESULTSTR) > 0)
                {
                    PushUndo();
                }
            }

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
                    if (_ErrorMessage.Length > 0 && _ToolTipEx != null)
                    {
                        Pen pen = new Pen(OptionForm.Color_Error_ForeColor(), 3);
                        g.DrawRectangle(pen, this.ClientRectangle);
                        pen.Dispose();
                    }
                }
            }
        }


        class UndoData
        {
            //UNDO サイズも小さいから、差分よりすべて記録する. 
            public string Text;
            public int SelectionStart;
        };
        List<UndoData> UndoBuffer;
        int UndoPosstion;
        //Undo履歴のクリア
        public void ClearUndoBuffer()
        {
            this.UndoBuffer = new List<UndoData>();
            this.UndoPosstion = 0;
        }
        public void PushUndo()
        {
            if (this.UndoPosstion < this.UndoBuffer.Count)
            {//常に先頭に追加したいので、リスト中に戻っている場合は、それ以降を消す.
                this.UndoBuffer.RemoveRange(this.UndoPosstion, this.UndoBuffer.Count - this.UndoPosstion);
            }
            UndoData p = new UndoData();
            p.Text = this.Text;
            p.SelectionStart = this.SelectionStart;
            this.UndoBuffer.Add(p);
            this.UndoPosstion = this.UndoBuffer.Count;
        }
        public void RunUndo()
        {
            if (this.UndoPosstion <= 0)
            {
                return ; //無理
            }
            if (this.UndoPosstion == this.UndoBuffer.Count)
            {//現在が、undoがない最新版だったら、redoできるように、現状を保存する.
                PushUndo();
                this.UndoPosstion = UndoPosstion - 1;
            }

            this.UndoPosstion = UndoPosstion - 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);

            if (this.UndoCallback != null)
            {
                this.UndoCallback(this, new EventArgs());
            }
            return;
        }
        public void RunRedo()
        {
            if (this.UndoPosstion + 1 >= this.UndoBuffer.Count)
            {
                return ; //無理
            }
            this.UndoPosstion = UndoPosstion + 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);

            if (this.UndoCallback != null)
            {
                this.UndoCallback(this, new EventArgs());
            }
            return;
        }

        void RunUndoRollback(UndoData u)
        {
            this.LockWindowUpdate();
            this.Text = u.Text;
            this.UnLockWindowUpdate();
            this.SelectionStart = u.SelectionStart;
        }

        int WhenLockSelected;
        public void LockWindowUpdate()
        {
            if (this.IsDisposed || this.Enabled == false)
            {
                return;
            }
            this.WhenLockSelected = this.SelectionStart;
            if (this.WhenLockSelected < 0)
            {
                this.WhenLockSelected = 0;
            }

            U.LockWindowUpdate(this.Handle);
            this.Enabled = false;
        }

        public void UnLockWindowUpdate()
        {
            if (this.IsDisposed)
            {
                return;
            }

            try
            {
                this.Enabled = true;
                this.Focus();
                this.SelectionStart = this.WhenLockSelected;
                this.SelectionLength = 0;
                U.LockWindowUpdate(IntPtr.Zero);
            }
            catch (ObjectDisposedException e)
            {
                //どうやっても、ObjectDisposedExceptionが発生するときがあるらしいので、例外で逃げます.
                Log.Error("UnLockWindowUpdate",e.ToString());
            }
        }

        //改行を \r\n にそろえる.
        public string Text2
        {
            get
            {
                string a = base.Text;
                if (a.IndexOf("\n\n") < 0)
                {//書き込んだものを即読むと \r\nのままになっていることがあるらしい.
                    if (a.IndexOf("\r\n") >= 0)
                    {
                        return a;
                    }
                }
                return a.Replace("\n", "\r\n");
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

        void CutAction(object sender, EventArgs e)
        {
            this.Cut();
            if (this.CutCallback != null)
            {
                this.CutCallback(sender, e);
            }
        }

        void CopyAction(object sender, EventArgs e)
        {
            this.Copy();
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                return;
            }
            this.Paste();
            if (this.PasteCallback != null)
            {
                this.PasteCallback(sender, e);
            }
        } 
        void UndoAction(object sender, EventArgs e)
        {
            RunUndo();
        }
        void SelectAllAction(object sender, EventArgs e)
        {
            this.SelectAll();
        }

        void MakeContextMenu()
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;
            menuItem = new MenuItem(R._("元に戻す(&Z)"));
            menuItem.Click += new EventHandler(UndoAction);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("切り取り(&X)"));
            menuItem.Click += new EventHandler(CutAction);
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("コピー(&C)"));
            menuItem.Click += new EventHandler(CopyAction);
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("貼り付け(&V)"));
            menuItem.Click += new EventHandler(PasteAction);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("すべて選択(&A)"));
            menuItem.Click += new EventHandler(SelectAllAction);
            contextMenu.MenuItems.Add(menuItem);

            this.ContextMenu = contextMenu;        
        }
        public void AppendContentMenu(string name,EventHandler clickHandler)
        {
            MenuItem menuItem = new MenuItem(name);
            menuItem.Click += new EventHandler(clickHandler);

            this.ContextMenu.MenuItems.Add(menuItem);
        }
        public void AppendContentMenuBar()
        {
            MenuItem menuItem = new MenuItem("-");

            this.ContextMenu.MenuItems.Add(menuItem);
        }

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
        public EventHandler UndoCallback;
        public EventHandler CutCallback;
        public EventHandler PasteCallback;
    }
}
