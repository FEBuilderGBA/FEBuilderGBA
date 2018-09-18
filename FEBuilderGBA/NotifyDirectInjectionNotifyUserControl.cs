using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    //上の方に出す通知ボックス。 InjectNotifyのPR
    public partial class NotifyDirectInjectionNotifyUserControl : UserControl
    {
        public NotifyDirectInjectionNotifyUserControl()
        {
            InitializeComponent();

            Color Color_Notify_BackColor = OptionForm.Color_List_HoverColor();
            Color Color_Notify_ForeColor = OptionForm.Color_Control_ForeColor();

            this.BackColor = Color_Notify_BackColor;
            this.ForeColor = Color_Notify_ForeColor;

            this.BIG_TEXT.BackColor = Color_Notify_BackColor;
            this.BIG_TEXT.ForeColor = Color_Notify_ForeColor;

            this.AllowLabel.BackColor = Color_Notify_BackColor;
            this.AllowLabel.ForeColor = Color_Notify_ForeColor;
        }
        public void ShowAnimation(Form self,ListBox listbox, int write_notify_time)
        {
            Debug.Assert(write_notify_time > 0);

            int initWait = 1000;
            int baseHeight = this.Height;

            Point showPoint = listbox.PointToScreen(new Point(0, 0 ));
            showPoint.Y -= baseHeight;
            showPoint = self.PointToClient(showPoint);
            if (showPoint.Y < 20)
            {//微妙な端数があるととみすぼらしいので、その場合は0にする.
                baseHeight += showPoint.Y;
                showPoint.Y = 0;
            }

            this.Width = self.Width - 20;
            this.Height = 0;
            this.Location = new System.Drawing.Point(showPoint.X, showPoint.Y);
            this.Hide();
            self.Controls.Add(this);
            this.BringToFront();
            this.Show();

            int timeMS = 0;
            int speed = 4;
            Timer timer = new Timer();
            timer.Interval = 20 * write_notify_time;
            timer.Tick += (sender, e) =>
            {
                timeMS += 100;
                if (timeMS < initWait)
                {
                }
                else if (timeMS <= initWait + 300)
                {   //表示
                    if (this.Height + speed >= baseHeight)
                    {
                        this.Height = baseHeight;
                    }
                    else
                    {
                        this.Height += speed;
                        speed += 4;
                    }
                }
                else if (timeMS <= initWait + 2400)
                {
                    //ちょっと待機
                    speed = 4;
                }
                else
                {
                    if (this.Height - speed > 0)
                    {
                        //戻る
                        this.Height -= speed;
                    }
                    else
                    {
                        timer.Stop();
                        timer = null;

                        self.Controls.Remove(this);
                    }
                }
            };
            timer.Start();
        }
    }
}
