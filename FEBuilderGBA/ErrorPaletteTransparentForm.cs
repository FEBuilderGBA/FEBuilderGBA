using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FEBuilderGBA
{
    public partial class ErrorPaletteTransparentForm : Form
    {
        public ErrorPaletteTransparentForm()
        {
            InitializeComponent();
        }

        private void ErrorPaletteShowForm_Load(object sender, EventArgs e)
        {
            //メッセージ（警告）を鳴らす
            System.Media.SystemSounds.Exclamation.Play();
        }

        Bitmap ResultBitmap;
        public void SetOrignalImage(Bitmap bitmap)
        {
            this.ResultBitmap = bitmap;
            this.X_OrignalImage.Image = bitmap;

            int palmax = Math.Min(16, bitmap.Palette.Entries.Length);
            for (int i = 0; i < palmax; i++)
            {
                Color c = bitmap.Palette.Entries[i];
                SetColor(i+1, c.R, c.G, c.B);
            }
        }

        int ResultColorIndex = -1;
        public int GetResultColorIndex()
        {
            return this.ResultColorIndex;
        }

        void Selceted(int index)
        {
            Debug.Assert(this.ResultBitmap != null);
            Debug.Assert(index > 0);

            this.ResultColorIndex = index - 1;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void SetColor(int index, int r, int g, int b)
        {
            string info = R._("R:{0}, G:{1}, B:{2}", r, g, b);

            Color rgb = Color.FromArgb(r, g, b);
            if (index == 1)
            {
                this.Info_1.Text = info;
                this.Color_1.BackColor = rgb;
            }
            else if (index == 2)
            {
                this.Info_2.Text = info;
                this.Color_2.BackColor = rgb;
            }
            else if (index == 3)
            {
                this.Info_3.Text = info;
                this.Color_3.BackColor = rgb;
            }
            else if (index == 4)
            {
                this.Info_4.Text = info;
                this.Color_4.BackColor = rgb;
            }
            else if (index == 5)
            {
                this.Info_5.Text = info;
                this.Color_5.BackColor = rgb;
            }
            else if (index == 6)
            {
                this.Info_6.Text = info;
                this.Color_6.BackColor = rgb;
            }
            else if (index == 7)
            {
                this.Info_7.Text = info;
                this.Color_7.BackColor = rgb;
            }
            else if (index == 8)
            {
                this.Info_8.Text = info;
                this.Color_8.BackColor = rgb;
            }
            else if (index == 9)
            {
                this.Info_9.Text = info;
                this.Color_9.BackColor = rgb;
            }
            else if (index == 10)
            {
                this.Info_10.Text = info;
                this.Color_10.BackColor = rgb;
            }
            else if (index == 11)
            {
                this.Info_11.Text = info;
                this.Color_11.BackColor = rgb;
            }
            else if (index == 12)
            {
                this.Info_12.Text = info;
                this.Color_12.BackColor = rgb;
            }
            else if (index == 13)
            {
                this.Info_13.Text = info;
                this.Color_13.BackColor = rgb;
            }
            else if (index == 14)
            {
                this.Info_14.Text = info;
                this.Color_14.BackColor = rgb;
            }
            else if (index == 15)
            {
                this.Info_15.Text = info;
                this.Color_15.BackColor = rgb;
            }
            else if (index == 16)
            {
                this.Info_16.Text = info;
                this.Color_16.BackColor = rgb;
            }
        }

        private void Color_1_Click(object sender, EventArgs e)
        {
            Selceted(1);
        }

        private void Color_2_Click(object sender, EventArgs e)
        {
            Selceted(2);
        }

        private void Color_3_Click(object sender, EventArgs e)
        {
            Selceted(3);
        }

        private void Color_4_Click(object sender, EventArgs e)
        {
            Selceted(4);
        }

        private void Color_5_Click(object sender, EventArgs e)
        {
            Selceted(5);
        }

        private void Color_6_Click(object sender, EventArgs e)
        {
            Selceted(6);
        }

        private void Color_7_Click(object sender, EventArgs e)
        {
            Selceted(7);
        }

        private void Color_8_Click(object sender, EventArgs e)
        {
            Selceted(8);
        }

        private void Color_9_Click(object sender, EventArgs e)
        {
            Selceted(9);
        }

        private void Color_10_Click(object sender, EventArgs e)
        {
            Selceted(10);
        }

        private void Color_11_Click(object sender, EventArgs e)
        {
            Selceted(11);
        }

        private void Color_12_Click(object sender, EventArgs e)
        {
            Selceted(12);
        }

        private void Color_13_Click(object sender, EventArgs e)
        {
            Selceted(13);
        }

        private void Color_14_Click(object sender, EventArgs e)
        {
            Selceted(14);
        }

        private void Color_15_Click(object sender, EventArgs e)
        {
            Selceted(15);
        }

        private void Color_16_Click(object sender, EventArgs e)
        {
            Selceted(16);
        }
    }
}
