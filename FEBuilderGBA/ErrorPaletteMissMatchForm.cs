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
    public partial class ErrorPaletteMissMatchForm : Form
    {
        public ErrorPaletteMissMatchForm()
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

        Bitmap CopyPalette(Bitmap paletteBitmap, Bitmap baseBitmap)
        {
            Bitmap bitmap = ImageUtil.CloneBitmap(baseBitmap);
            ColorPalette src  = paletteBitmap.Palette;
            ColorPalette dest = bitmap.Palette;
            int length = Math.Min(src.Entries.Length, dest.Entries.Length);
            for (int i = 0; i < length; i++ )
            {
                dest.Entries[i] = src.Entries[i];
            }
            bitmap.Palette = dest;
            return bitmap;
        }

        Bitmap OrignalImage;
        public void SetOrignalImage(Bitmap paletteBitmap, Bitmap baseBitmap)
        {
            this.OrignalImage = CopyPalette(paletteBitmap, baseBitmap);
            this.X_OrignalImage.Image = this.OrignalImage;
            ChangeScale(this.X_OrignalImage);
        }
        Bitmap ReOrderImage1 = null;
        public void SetReOrderImage1(Bitmap paletteBitmap,Bitmap baseBitmap)
        {
            if (paletteBitmap != null)
            {
                this.ReOrderImage1 = CopyPalette(paletteBitmap, baseBitmap);
                this.X_ReOrderImage1.Image = this.ReOrderImage1;
                ChangeScale(this.X_ReOrderImage1);
            }
            else
            {
                X_ReOrderImage1Panel.Hide();
                return;
            }
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
    }
}
