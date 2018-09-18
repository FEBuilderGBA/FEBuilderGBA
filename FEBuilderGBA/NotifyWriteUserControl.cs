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
    public partial class NotifyWriteUserControl : UserControl
    {
        public NotifyWriteUserControl()
        {
            InitializeComponent();

            Color Color_NotifyWrite_BackColor = OptionForm.Color_NotifyWrite_BackColor();
            Color Color_NotifyWrite_ForeColor = OptionForm.Color_NotifyWrite_ForeColor();

            this.BackColor = Color_NotifyWrite_BackColor;
            this.ForeColor = Color_NotifyWrite_ForeColor;

            this.BIG_TEXT.BackColor = Color_NotifyWrite_BackColor;
            this.BIG_TEXT.ForeColor = Color_NotifyWrite_ForeColor;
        }
        public void UpdateText(uint addr)
        {
            if (addr == 0)
            {
                BIG_TEXT.Text = R._("データを書き込みました");
            }
            else
            {
                BIG_TEXT.Text = BIG_TEXT.Text.Replace("{}", addr.ToString("X08"));
            }
        }
        public string GetBigText()
        {
            return BIG_TEXT.Text;
        }
        public void ShowAnimation(Form self, int write_notify_time)
        {
            Debug.Assert(write_notify_time > 0);

            int initHeight = self.Height - 50;
            int nowHeight = initHeight;

            this.Width = self.Width - 10 - 20;
            this.Height = 0;
            this.Location = new System.Drawing.Point(10, nowHeight);
            this.Hide();
            self.Controls.Add(this);
            this.BringToFront();
            this.Show();

            int timeMS = 0;
            int speedUp = 10;
            Timer timer = new Timer();
            timer.Interval = 20 * write_notify_time;
            timer.Tick += (sender, e) =>
            {
                timeMS += 100;
                if (timeMS <= 400)
                {
                    //表示
                    nowHeight -= speedUp;
                    this.Location = new System.Drawing.Point(10, nowHeight);
                    this.Height += speedUp;
                    speedUp += 4;
                }
                else if (timeMS <= 2000)
                {
                    //ちょっと待機
                    speedUp = 5;
                    this.Location = new System.Drawing.Point(10, nowHeight);
                }
                else
                {
                    if (this.Height - speedUp > 0)
                    {
                        //戻る
                        this.Height -= speedUp;
                        this.Location = new System.Drawing.Point(10, nowHeight);
                        nowHeight += speedUp;
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
