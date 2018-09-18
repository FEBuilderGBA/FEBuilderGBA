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
        public void SetReOrderImage1(Bitmap src, int maxPalette, int yohaku, bool isReserve1StPalette)
        {
            //減色する
            DecreaseColor deColor = new DecreaseColor();
            this.ReOrderImage1 = deColor.Convert(src, maxPalette, yohaku, isReserve1StPalette , false);

            this.X_ReOrderImage1.Image = src;//this.ReOrderImage1;
            ChangeScale(this.X_ReOrderImage1);

            X_ReOrderImage1Panel.Show();
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
    }
}
