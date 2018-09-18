using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ErrorPaletteShowForm : Form
    {
        public ErrorPaletteShowForm()
        {
            InitializeComponent();

            this.X_ReOrderImage1Panel.Hide();
            this.X_ReOrderImage2Panel.Hide();
            this.X_CancelImage.Image = SystemIcons.Error.ToBitmap();
        }

        private void ErrorPaletteShowForm_Load(object sender, EventArgs e)
        {
            //メッセージ（警告）を鳴らす
            System.Media.SystemSounds.Exclamation.Play();
        }

        public void SetErrorMessage(string message)
        {
            this.ErrorMessage.Text = message;
            this.ErrorMessage.Select(0, 0); //全選択解除.
        }

        void ChangeScale(InterpolatedPictureBox pic)
        {
            if (pic.Image == null)
            {
                return;
            }
            double ratio = pic.Image.Width / (double)pic.Image.Height;
            if (ratio < 0.2)
            {
                pic.SizeMode = PictureBoxSizeMode.Normal;
            }
            else
            {
                pic.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        Bitmap OrignalImage;
        public void SetOrignalImage(Bitmap bitmap)
        {
            this.OrignalImage = bitmap;
            this.X_OrignalImage.Image = bitmap;
            ChangeScale(this.X_OrignalImage);
        }
        Bitmap ReOrderImage1 = null;
        public void SetReOrderImage1(Bitmap bitmap)
        {
            this.ReOrderImage1 = bitmap;
            this.X_ReOrderImage1.Image = bitmap;
            ChangeScale(this.X_ReOrderImage1);

            if (bitmap == null)
            {
                X_ReOrderImage1Panel.Hide();
                return;
            }
            X_ReOrderImage1Panel.Show();
        }
        Bitmap ReOrderImage2 = null;
        public void SetReOrderImage2(Bitmap bitmap)
        {

            this.ReOrderImage2 = bitmap;
            this.X_ReOrderImage2.Image = bitmap;
            ChangeScale(this.X_ReOrderImage2);

            if (bitmap == null)
            {
                X_ReOrderImage2Panel.Hide();
                return;
            }
            X_ReOrderImage2Panel.Show();
        }
        Bitmap ResultBitmap = null;
        public Bitmap GetResultBitmap()
        {
            return this.ResultBitmap;
        }


        public void ShowForceButton()
        {
            this.ForceButton.Show();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.ResultBitmap = null;
            this.Close();
        }

        private void ForceButton_Click(object sender, EventArgs e)
        {
            this.ResultBitmap = OrignalImage;
            this.Close();
        }

        private void ReOrder1Button_Click(object sender, EventArgs e)
        {
            this.ResultBitmap = ReOrderImage1;
            this.Close();
        }

        private void ReOrder2Button_Click(object sender, EventArgs e)
        {
            this.ResultBitmap = ReOrderImage2;
            this.Close();
        }

        private void X_CancelImage_Click(object sender, EventArgs e)
        {
            CloseButton.PerformClick();
        }

        private void X_OrignalImage_Click(object sender, EventArgs e)
        {
            ForceButton.PerformClick();
        }

        private void X_ReOrderImage1_Click(object sender, EventArgs e)
        {
            ReOrder1Button.PerformClick();
        }

        private void X_ReOrderImage2_Click(object sender, EventArgs e)
        {
            ReOrder2Button.PerformClick();
        }
    }
}
