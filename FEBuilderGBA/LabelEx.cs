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
    public class LabelEx : Label
    {
        public LabelEx()
        {
        }
        protected override void Dispose(bool disposing)
        {
        }



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
    }
}
