using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Diagnostics;

namespace FEBuilderGBA
{
    public partial class ImagePortraitImporterForm : Form
    {
        Pen EyePen = new Pen(Color.Chartreuse, 1);
        Pen MouthPen = new Pen(Color.Aqua, 1);

        public ImagePortraitImporterForm()
        {
            InitializeComponent();

            U.SetIcon(SimpleImportButton, U.GetShell32Icon(45));
            U.SetIcon(ImportButton, U.GetShell32Icon(45));
        }

        private void ImagePortraitImporterForm_Load(object sender, EventArgs e)
        {
            if (this.IsFE6Image)
            {
                this.MouthX.Focus();
                this.EyeBlockX.Enabled = false;
                this.EyeBlockY.Enabled = false;
                this.EyeX.Enabled = false;
                this.EyeY.Enabled = false;
                this.EyeW.Enabled = false;
                this.EyeH.Enabled = false;
                this.X_EYE.Hide();
            }
            else
            {
                this.EyeX.Focus();
                this.EyeBlockX.Enabled = true;
                this.EyeBlockY.Enabled = true;
                this.EyeX.Enabled = true;
                this.EyeY.Enabled = true;
                this.EyeW.Enabled = true;
                this.EyeH.Enabled = true;
                this.X_EYE.Show();
            }
            GenSimplePreview();
        }

        bool IsFE6ImageCheck(Bitmap bitmap)
        {
            for (int y = 16 * 3; y < 16 * 3 + 16; y++)
            {
                for (int x = 96; x < 96 + 32; x++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    if (c != TransparentColor)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        bool IsFE6Image;
        Bitmap OrignalImage;
        Color TransparentColor;
        public void SetOrignalImage(Bitmap bitmap
            ,int eyeBlockX,int eyeBlockY
            ,int mouthBlockX,int mouthBlockY)
        {
            this.TransparentColor = bitmap.GetPixel(127, 0);
            this.IsFE6Image = IsFE6ImageCheck(bitmap);
            bitmap.MakeTransparent(this.TransparentColor);
            this.OrignalImage = bitmap;

            U.SelectedIndexSafety(this.EyeBlockX, eyeBlockX);
            U.SelectedIndexSafety(this.EyeBlockY, eyeBlockY);
            U.SelectedIndexSafety(this.MouthBlockX, mouthBlockX);
            U.SelectedIndexSafety(this.MouthBlockY, mouthBlockY);

            EyeX_ValueChanged(null, null);
        }
        public int GetEyeBlockX()
        {
            return (int)this.EyeBlockX.Value;
        }
        public int GetEyeBlockY()
        {
            return (int)this.EyeBlockY.Value;
        }
        public int GetMouthBlockX()
        {
            return (int)this.MouthBlockX.Value;
        }
        public int GetMouthBlockY()
        {
            return (int)this.MouthBlockY.Value;
        }


        void GenEye()
        {
            Bitmap face = new Bitmap(32*3, 16*3);
            Graphics g = Graphics.FromImage(face);

            //顔のベース
            g.DrawImage(this.OrignalImage, new Rectangle(0, 0, face.Width, face.Height), new Rectangle(96, 48, 32, 16), GraphicsUnit.Pixel);

            //目の位置枠
            g.DrawRectangle(EyePen
                , (int)this.EyeX.Value * 3
                , (int)this.EyeY.Value * 3
                , (int)this.EyeW.Value * 3
                , (int)this.EyeH.Value * 3
            );

            this.X_EYE.Image = face;

            g.Dispose();
        }

        void GenMouth()
        {
            Bitmap face = new Bitmap(32 * 3, 16 * 3);
            Graphics g = Graphics.FromImage(face);

            //顔のベース
            g.DrawImage(this.OrignalImage, new Rectangle(0, 0, face.Width, face.Height), new Rectangle(0, 80, 32, 16), GraphicsUnit.Pixel);

            //口の位置枠
            g.DrawRectangle(MouthPen
                , (int)this.MouthX.Value * 3
                , (int)this.MouthY.Value * 3
                , (int)this.MouthW.Value * 3
                , (int)this.MouthH.Value * 3
            );

            this.X_MOUTH.Image = face;

            g.Dispose();
        }

        Bitmap DecreaseColor16(Bitmap bitmap)
        {
            //16色に減色.
            DecreaseColor d = new DecreaseColor();
            Bitmap bitmap16 = d.Convert(bitmap, 1, 0, true , false);

            if (this.checkBoxFuchidori.Checked)
            {
                //縁取りすると綺麗に見える
                //黒色を探す
                int blackColorIndex = ImageUtil.FindBlackColorFromPalette(bitmap16, 1, 16);
                //メイン
                bitmap16 = ImageUtil.Fuchidori(bitmap16, (byte)blackColorIndex, new Rectangle(0, 0, 96, 80));
            }

            //パーツをきれいに見せるために再構成する.
            int face_eye_x_id = (int)this.EyeBlockX.Value;
            int face_eye_y_id = (int)this.EyeBlockY.Value; 
            
            int seet_eye_x = (int)(this.EyeX.Value);
            int seet_eye_w = (int)(this.EyeW.Value);
            int seet_eye_y = (int)(this.EyeY.Value);
            int seet_eye_h = (int)(this.EyeH.Value);

            Bitmap tempBitmap;
            if (!this.IsFE6Image)
            {
                tempBitmap = ImageUtil.Copy(bitmap16, face_eye_x_id * 8, face_eye_y_id * 8, 32, 16);
                ImageUtil.BitBlt(tempBitmap, seet_eye_x, seet_eye_y, seet_eye_w, seet_eye_h, bitmap16, seet_eye_x + 96, seet_eye_y + 16 * 3);
                ImageUtil.BitBlt(bitmap16, 96, 16 * 3, 32, 16, tempBitmap, 0, 0);

                tempBitmap = ImageUtil.Copy(bitmap16, face_eye_x_id * 8, face_eye_y_id * 8, 32, 16);
                ImageUtil.BitBlt(tempBitmap, seet_eye_x, seet_eye_y, seet_eye_w, seet_eye_h, bitmap16, seet_eye_x + 96, seet_eye_y + 16 * 4);
                ImageUtil.BitBlt(bitmap16, 96, 16 * 4, 32, 16, tempBitmap, 0, 0);
            }

            int face_mouth_x_id = (int)this.MouthBlockX.Value;
            int face_mouth_y_id = (int)this.MouthBlockY.Value;

            int seet_mouth_x = (int)(this.MouthX.Value);
            int seet_mouth_w = (int)(this.MouthW.Value);
            int seet_mouth_y = (int)(this.MouthY.Value);
            int seet_mouth_h = (int)(this.MouthH.Value);
            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 8, face_mouth_y_id * 8, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 0, seet_mouth_y + 16 * 5);
            ImageUtil.BitBlt(bitmap16, 32 * 0, 16 * 5, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 8, face_mouth_y_id * 8, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 1, seet_mouth_y + 16 * 5);
            ImageUtil.BitBlt(bitmap16, 32 * 1, 16 * 5, 32, 16, tempBitmap, 0, 0);

            if (!this.IsFE6Image)
            {
                tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 8, face_mouth_y_id * 8, 32, 16);
                ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 2, seet_mouth_y + 16 * 5);
                ImageUtil.BitBlt(bitmap16, 32 * 2, 16 * 5, 32, 16, tempBitmap, 0, 0);
            }

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 8, face_mouth_y_id * 8, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 3, seet_mouth_y + 16 * 5);
            ImageUtil.BitBlt(bitmap16, 32 * 3, 16 * 5, 32, 16, tempBitmap, 0, 0);


            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 8, face_mouth_y_id * 8, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 0, seet_mouth_y + 16 * 6);
            ImageUtil.BitBlt(bitmap16, 32 * 0, 16 * 6, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 8, face_mouth_y_id * 8, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 1, seet_mouth_y + 16 * 6);
            ImageUtil.BitBlt(bitmap16, 32 * 1, 16 * 6, 32, 16, tempBitmap, 0, 0);

            if (!this.IsFE6Image)
            {
                tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 8, face_mouth_y_id * 8, 32, 16);
                ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 2, seet_mouth_y + 16 * 6);
                ImageUtil.BitBlt(bitmap16, 32 * 2, 16 * 6, 32, 16, tempBitmap, 0, 0);
            }

            return bitmap16;
        }


        Bitmap PreviewBitmap16;
        void GenPreview()
        {
            this.PreviewBitmap16 = DecreaseColor16(this.OrignalImage);

            ColorPalette pal = this.PreviewBitmap16.Palette;
            pal.Entries[0] = this.TransparentColor;
            this.PreviewBitmap16.Palette = pal;

            this.X_Preview.Image = this.PreviewBitmap16;
        }

        Bitmap ResultBitmap = null;
        public Bitmap GetResultBitmap()
        {
            return this.ResultBitmap;
        }

        void GenPreviewMainChar()
        {
            Bitmap face = ImageUtil.Blank(96, 80 , this.PreviewBitmap16);

            int eyex = (int)this.EyeBlockX.Value * 8;
            int eyey = (int)this.EyeBlockY.Value * 8;
            int mouthx = (int)this.MouthBlockX.Value * 8;
            int mouthy = (int)this.MouthBlockY.Value * 8;

            ImageUtil.BitBlt(face, 0, 0, 96, 80, this.PreviewBitmap16, 0, 0);
            int f = (int)X_Frame.Value;
            if (f == 10)
            {
                ImageUtil.BitBlt(face, eyex, eyey, 32, 16, this.PreviewBitmap16, 96, 48, 0, 0);
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 0, 80, 0, 0);
                X_STATUS.Text = R._("位置確認用");
            }
            else if (f == 1)
            {//半目
                ImageUtil.BitBlt(face, eyex, eyey, 32, 16, this.PreviewBitmap16, 96, 48, 0 ,0);
                X_STATUS.Text = R._("半目");
            }
            else if (f == 2)
            {//閉じ目
                ImageUtil.BitBlt(face, eyex, eyey, 32, 16, this.PreviewBitmap16, 96, 48 + 16, 0, 0);
                X_STATUS.Text = R._("とじ目");
            }
            else if (f == 3)
            {//口1
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 0, 80, 0, 0);
                X_STATUS.Text = R._("口1");
            }
            else if (f == 4)
            {//口2
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 32, 80, 0, 0);
                X_STATUS.Text = R._("口2");
            }
            else if (f == 5)
            {//口3
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 32+32, 80, 0, 0);
                X_STATUS.Text = R._("口3");
            }
            else if (f == 6)
            {//口4
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 32 + 32 + 32, 80, 0, 0);
                X_STATUS.Text = R._("ステータス画面 口4");
            }
            else if (f == 7)
            {//口5
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 0, 80 + 16, 0, 0);
                X_STATUS.Text = R._("口5");
            }
            else if (f == 8)
            {//口6
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 32, 80 + 16, 0, 0);
                X_STATUS.Text = R._("口6");
            }
            else if (f == 9)
            {//口7
                ImageUtil.BitBlt(face, mouthx, mouthy, 32, 16, this.PreviewBitmap16, 32+32, 80 + 16, 0, 0);
                X_STATUS.Text = R._("口7");
            }
            else
            {
                X_STATUS.Text = R._("通常時");
            }

            this.X_PreviewMainChar.Image = face;
        }

        private void EyeX_ValueChanged(object sender, EventArgs e)
        {
            GenEye();
            GenMouth();
            GenPreview();
            GenPreviewMainChar();
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            GenPreview();
            this.ResultBitmap = this.PreviewBitmap16;
            this.Close();
        }



        Bitmap SimplePreviewBitmap16;
        void GenSimplePreview()
        {
            //16色に減色.
            DecreaseColor d = new DecreaseColor();
            this.SimplePreviewBitmap16 = d.Convert(this.OrignalImage, 1, 0, true , false);

            ColorPalette pal = this.SimplePreviewBitmap16.Palette;
            pal.Entries[0] = this.TransparentColor;
            this.SimplePreviewBitmap16.Palette = pal;

            this.X_SimplePreview.Image = this.SimplePreviewBitmap16;
        }

        private void SimpleImportButton_Click(object sender, EventArgs e)
        {
            GenSimplePreview();
            this.ResultBitmap = this.SimplePreviewBitmap16;
            this.Close();
        }

        public bool IsDetailMode()
        {
            return this.MainTabControl.SelectedIndex == 1;
        }

    }
}
