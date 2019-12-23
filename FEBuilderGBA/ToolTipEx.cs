using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Text;
using System.Drawing;

namespace FEBuilderGBA
{
    public class ToolTipEx : ToolTip
    {
        bool IsShow = false;
        Control SelectConstol = null; 
        Dictionary<Control, string> Hint = new Dictionary<Control,string>();
        const int HEIGHT_MARGINE = 2;

        public ToolTipEx() : base()
        {
        }
        public ToolTipEx(IContainer cont)
            : base(cont)
        {
        }

        public new void SetToolTip(Control control, string caption)
        {
            Hint[control] = caption;

            control.MouseMove -= FireEventInner;
            control.MouseLeave -= HideEvent;
            control.MouseMove += FireEventInner;
            control.MouseLeave += HideEvent;
        }

        public void SetToolTipIfNew(Control control, string caption)
        {
            if (Hint.ContainsKey(control))
            {
                return;
            }
            SetToolTip(control, caption);
        }

        void FireEventInner(object sender, EventArgs e)
        {
            if (IsShow)
            {
                return;
            }
            if (!(sender is Control))
            {
                return;
            }
            Control control = (Control)sender;
            string caption = GetToolTip(control);

            FireEvent(control, caption);
        }
        bool IsReverseShow(Control control, int tooltipHeight, int posWidth, int posHeight)
        {
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            Point p = control.PointToScreen(new Point(posWidth, posHeight + HEIGHT_MARGINE));
            if (p.Y + tooltipHeight > screenHeight)
            {
                return true;
            }

            return false;
        }
        public void FireEvent(Control control, string caption)
        {
            if (IsShow)
            {
                return;
            }
            if (caption == "")
            {
                return;
            }

            int lineCount = U.GetCountOfLines(caption);
            int tooltipHeight = lineCount * (control.Font.Height + (HEIGHT_MARGINE * 3));
            if (IsReverseShow(control, tooltipHeight,0, control.Height))
            {
                base.Show(caption, control, 0, -tooltipHeight - HEIGHT_MARGINE);
            }
            else
            {
                base.Show(caption, control, 0, control.Height + HEIGHT_MARGINE);
            }

            IsShow = true;
            SelectConstol = control;
        }
        public void FirePointEvent(Control control, int x, int y, string caption)
        {
            FireListEvent(control, new Rectangle(x, y, 8, 8), caption);
        }

        public void FireListEvent(Control control, Rectangle rc, string caption)
        {
            if (IsShow)
            {
                return;
            }
            if (caption == "")
            {
                return;
            }

            int tooltipHeight = U.GetCountOfLines(caption) * (control.Font.Height + (HEIGHT_MARGINE * 3));
            if (IsReverseShow(control, tooltipHeight, rc.X , rc.Y + rc.Height))
            {
                base.Show(caption, control, rc.X, rc.Y  - tooltipHeight - HEIGHT_MARGINE);
            }
            else
            {
                base.Show(caption, control, rc.X, (rc.Y + rc.Height) + HEIGHT_MARGINE);
            }

            IsShow = true;
            SelectConstol = control;
        }

        public void HideEvent(object sender,EventArgs e)
        {
            IsShow = false;
            if (!(sender is Control))
            {
                if (SelectConstol == null)
                {
                    return;
                }
                sender = SelectConstol;
            }

            base.Hide((Control)sender);
            SelectConstol = null;
        }
        public void HideEvent()
        {
            HideEvent(null, null);
        }
        public new string GetToolTip(Control control)
        {
            string ret;
            if (Hint.TryGetValue(control, out ret))
            {
                return ret;
            }
            return "";
        }
    }
}
