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
    public partial class ErrorTSAErrorForm : Form
    {
        public ErrorTSAErrorForm()
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

        public void SetErrorMessage(string errorMessage)
        {
            X_ErrorMessage.Text = errorMessage;
        }

        Bitmap ReOrderImage1 = null;
        Bitmap ReOrderImage2 = null;
        public void SetReOrderImage1(Bitmap src, int maxPalette, int yohaku, bool isReserve1StPalette)
        {
            //減色する
            DecreaseColor deColor = new DecreaseColor();
            this.ReOrderImage1 = deColor.Convert(src, maxPalette, yohaku * 8, isReserve1StPalette, false);
            if (this.ReOrderImage1 == null)
            {
                X_ReOrderImage1Panel.Hide();
                X_ReOrderImage2Panel.Hide();
                return;
            }

            this.X_ReOrderImage1.Image = ReOrderImage1;//this.ReOrderImage1;
            ChangeScale(this.X_ReOrderImage1);
            X_ReOrderImage1Panel.Show();

            if (src.PixelFormat == PixelFormat.Format8bppIndexed 
                && src.Height == 160)
            {
                Bitmap pic2 = ImageUtil.CloneBitmap(src);
                DecreaseColor.ForceConvertTSA(pic2);
                this.ReOrderImage2 = pic2;
                this.X_ReOrderImage2.Image = (Image)ReOrderImage2.Clone();//this.ReOrderImage1;
                ChangeScale(this.X_ReOrderImage2);
                X_ReOrderImage2Panel.Show();
            }
        }
        Bitmap ResultBitmap = null;
        public Bitmap GetResultBitmap()
        {
            return this.ResultBitmap;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.ResultBitmap = null;
            this.Close();
        }

        private void ReOrder1Button_Click(object sender, EventArgs e)
        {
            this.ResultBitmap = ReOrderImage1;
            this.Close();
        }

        private void X_CancelImage_Click(object sender, EventArgs e)
        {
            CloseButton.PerformClick();
        }

        private void X_ReOrderImage1_Click(object sender, EventArgs e)
        {
            ReOrder1Button.PerformClick();
        }

        private void ReOrder2Button_Click(object sender, EventArgs e)
        {
            this.ResultBitmap = ReOrderImage2;
            this.Close();
        }
    }
}
