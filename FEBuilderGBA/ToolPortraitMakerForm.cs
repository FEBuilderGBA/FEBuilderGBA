using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolPortraitMakerForm : Form
    {
        Pen WakuPen = new Pen(Color.Red, 3);
        Pen WakuSmallPen = new Pen(Color.Red, 1);
        Pen EyePen = new Pen(Color.Chartreuse, 1);
        Pen MouthPen = new Pen(Color.Aqua, 1);

        Brush WakuBrush = new SolidBrush(Color.Red);

        const int PADDING_WIDTH = 128;
        const int PADDING_HEIGHT = 48;
        Bitmap CanvasCache;

        public ToolPortraitMakerForm()
        {
            InitializeComponent();
            RotateYComboBox.SelectedIndex = 0;
            FaceTestType.SelectedIndex = 0;
            RevLeftRightCheckBox.Checked = false;
            DecreaseColorCheckBox.Checked = true;

            FaceX.Value = 221;
            FaceY.Value = 111;
            FaceW.Value = 145;
            ClipX.Value = 167;
            ClipY.Value = 90;
            ClipScale.Value = 272;
            EyeX.Value = 254;
            EyeY.Value = 188;
            EyeW.Value = 65;
            EyeH.Value = 30;
            MouthX.Value = 276;
            MouthY.Value = 224;
            MouthW.Value = 23;
            MouthH.Value = 15;
        }

        private void BustupFilename_DoubleClick(object sender, EventArgs e)
        {
            BustshotSelectButton.PerformClick();
        }

        private void FaceFilename_DoubleClick(object sender, EventArgs e)
        {
            FaceSelectButton.PerformClick();
        }

        Bitmap BustshotBitmap;
        Bitmap FaceBitmap;
        public static string OpenFilenameDialogFullColor(Form self, string addName = "")
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("IMAGES|*.png;*.bmp;*.jpg|PNG|*.png|BMP|*.bmp|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(self, addName, open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "";
            }
            if (!U.CanReadFileRetry(open))
            {
                return "";
            }
            Program.LastSelectedFilename.Save(self, addName, open);
            string filename = open.FileNames[0];
            return filename;
        }

        private void BustupSelectButton_Click(object sender, EventArgs e)
        {
            string filename = OpenFilenameDialogFullColor(this,"");
            if (filename == "")
            {
                return;
            }

            Bitmap bitmap = new Bitmap(filename);
            if (bitmap == null)
            {
                R.ShowStopError(R._("ファイルがありません。\r\nファイル名:{0}"), filename);
                return;
            }
            BustshotBitmap = bitmap.Clone(new Rectangle(0,0,bitmap.Width,bitmap.Height),bitmap.PixelFormat);
            bitmap.Dispose();
            
            this.BustshotFilename.Text = filename;
            MakeCanvasCache();
            Draw();
        }

        void MakeCanvasCache()
        {
            if (BustshotBitmap == null || FaceBitmap == null)
            {
                return ;
            }
            AjastFace();
            this.CanvasCache = null;
        }

        private void FaceSelectButton_Click(object sender, EventArgs e)
        {
            string filename = OpenFilenameDialogFullColor(this, "");
            if (filename == "")
            {
                return;
            }

            Bitmap bitmap = new Bitmap(filename);
            if (bitmap == null)
            {
                R.ShowStopError(R._("ファイルがありません。\r\nファイル名:{0}"), filename);
                return;
            }
            FaceBitmap = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
            bitmap.Dispose();

            this.FaceFilename.Text = filename;
            MakeCanvasCache();
            Draw();
        }

        void DrawImage(Graphics g, Bitmap bitmap, int x, int y, int w, int h)
        {
            if (RevLeftRightCheckBox.Checked)
            {//反転
                g.DrawImage(bitmap, x + w - 1, y, -w - 1, h);
            }
            else
            {
                g.DrawImage(bitmap, x, y, w, h);
            }
        }
        void DrawImage(Graphics g, Bitmap bitmap, Rectangle destRect, Rectangle srcRect)
        {
            if (RevLeftRightCheckBox.Checked)
            {//反転
                Rectangle destRect2 = new Rectangle(destRect.X + destRect.Width - 1, destRect.Y, -destRect.Width - 1, destRect.Height);
                Rectangle srcRect2 = new Rectangle(srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);
                g.DrawImage(bitmap, destRect2, srcRect2, GraphicsUnit.Pixel);
            }
            else
            {
                g.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        void DrawImageType(Graphics g, Bitmap FaceBitmap, Rectangle destRect, Rectangle srcRect, int canvasWidth, int canvasHeight)
        {
            Bitmap temp = new Bitmap(canvasWidth,canvasHeight);
            Graphics gg = Graphics.FromImage(temp);
            GraphicsSetting(gg);

            //一度テンポラリに拡大して描画する
            Rectangle destFaceRect = new Rectangle((int)this.FaceX.Value, (int)this.FaceY.Value, (int)this.FaceW.Value, (int)this.FaceW.Value);
            DrawImage(gg, FaceBitmap, destFaceRect, srcRect);

            //そこから必要なパーツだけを本番にコピーする.
            g.DrawImage(temp, destRect, destRect, GraphicsUnit.Pixel);

            gg.Dispose();
            temp.Dispose();
        }

        enum CATEGORY
        {
              PLAIN
            , TYPE
            , TYPE_EYE
            , TYPE_MOUTH
        };

        int GetFaceX(int facetype)
        {
            int width = FaceBitmap.Width / 4;
            int[] x_array = new int[] { 0, width * 1, width * 2, width * 3, 0, width * 1, width * 2, width * 3 };

            return x_array[facetype];
        }
        int GetFaceY(int facetype)
        {
            int height = FaceBitmap.Height / 2;
            int[] y_array = new int[] { 0, 0, 0, 0, height, height, height, height };

            return y_array[facetype];
        }

        Bitmap DrawBitmap(int facetype, CATEGORY category)
        {
            Bitmap CanvasBitmap = new Bitmap(this.Canvas.Width, this.Canvas.Height);
            if (BustshotBitmap == null || FaceBitmap == null)
            {
                return CanvasBitmap;
            }
            Graphics g = Graphics.FromImage(CanvasBitmap);
            GraphicsSetting(g);

            int widthpadding = (this.Canvas.Width - BustshotBitmap.Width) / 2;
            int heightpadding = PADDING_HEIGHT;
            DrawImage(g, BustshotBitmap, widthpadding, heightpadding, BustshotBitmap.Width, BustshotBitmap.Height);


            int width = FaceBitmap.Width / 4;
            int height = FaceBitmap.Height / 2;
            int face_x = GetFaceX(facetype);
            int face_y = GetFaceY(facetype);

            if (category == CATEGORY.TYPE)
            {
                Rectangle srcRect = new Rectangle(face_x, face_y, width, height);
                Rectangle destRect = new Rectangle((int)this.FaceX.Value, (int)this.FaceY.Value, (int)this.FaceW.Value, (int)this.FaceW.Value);
                DrawImage(g, FaceBitmap, destRect, srcRect);
            }
            else if (category == CATEGORY.TYPE_EYE)
            {
                Rectangle srcRect = new Rectangle(face_x, face_y, width, height);
                Rectangle destRect = new Rectangle((int)this.EyeX.Value, (int)this.EyeY.Value, (int)this.EyeW.Value, (int)this.EyeW.Value);
                DrawImageType(g, FaceBitmap, destRect, srcRect, this.Canvas.Width, this.Canvas.Height);
            }
            else if (category == CATEGORY.TYPE_MOUTH)
            {
                Rectangle srcRect = new Rectangle(face_x, face_y, width, height);
                Rectangle destRect = new Rectangle((int)this.MouthX.Value, (int)this.MouthY.Value, (int)this.MouthW.Value, (int)this.MouthW.Value);
                DrawImageType(g, FaceBitmap, destRect, srcRect, this.Canvas.Width, this.Canvas.Height);
            }
            g.Dispose();

            int rotateY = RotateYComboBox.SelectedIndex;
            if (rotateY == 0)
            {
                return CanvasBitmap;
            }

            CanvasBitmap = Rotate3DY(CanvasBitmap, rotateY * 10);
            return CanvasBitmap;
        }

        void Draw()
        {
            if (BustshotBitmap == null || FaceBitmap == null)
            {
                return;
            }

            if (this.CanvasCache == null)
            {//台形変換が重いので、キャッシュすることにした
                int facetesttype = FaceTestType.SelectedIndex;
                this.CanvasCache = DrawBitmap(facetesttype, CATEGORY.TYPE);
            }

            Bitmap CanvasBitmap = ImageUtil.CloneBitmap(this.CanvasCache);
            Graphics g = Graphics.FromImage(CanvasBitmap);
            GraphicsSetting(g);

            int wakuWidth = MakeWakuWidth();
            int wakuHeight = MakeWakuHeight();
            int tileWidth = wakuWidth / 3;
            int tileHeight = wakuHeight / 5;
            
            //切り抜く枠
            g.DrawRectangle(WakuPen
                , (int)this.ClipX.Value
                , (int)this.ClipY.Value
                , wakuWidth
                , wakuHeight
            );
            for (int w = 0; w < 3; w++)
            {
                for (int h = 0; h < 5; h++)
                {
                    g.DrawRectangle(WakuSmallPen
                        , (int)this.ClipX.Value + (tileWidth * w)
                        , (int)this.ClipY.Value + (tileHeight * h)
                        , tileWidth
                        , tileHeight
                    );
                }
            }
            //利用できない範囲にマーク
            g.FillRectangle(WakuBrush
                , (int)this.ClipX.Value + (tileWidth * 0)
                , (int)this.ClipY.Value + (tileHeight * 0)
                , tileWidth / 2
                , tileHeight * 3
            );
            g.FillRectangle(WakuBrush
                , (int)this.ClipX.Value + (tileWidth * 3) - (tileWidth / 2)
                , (int)this.ClipY.Value + (tileHeight * 0)
                , tileWidth / 2
                , tileHeight * 3
            );


            //目の位置枠
            g.DrawRectangle(EyePen
                , (int)this.EyeX.Value
                , (int)this.EyeY.Value
                , (int)this.EyeW.Value
                , (int)this.EyeH.Value
            );

            //口の位置枠
            g.DrawRectangle(MouthPen
                , (int)this.MouthX.Value
                , (int)this.MouthY.Value
                , (int)this.MouthW.Value
                , (int)this.MouthH.Value
            );

            Canvas.Image = CanvasBitmap;
            g.Dispose();
        }

        int MakeWakuWidth()
        {
            double scale = ((double)ClipScale.Value) / 100;
            int wakuWidth = (int)(96 * scale);
            return wakuWidth;
        }
        int MakeWakuHeight()
        {
            double scale = ((double)ClipScale.Value) / 100;
            int wakuHeight = (int)(80 * scale);
            return wakuHeight;
        }

        void GraphicsSetting(Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
        }

        private void ReDraw(object sender, EventArgs e)
        {
            Draw();
        }
        private void ReDrawNoCache(object sender, EventArgs e)
        {
            this.CanvasCache = null;
            Draw();
        }

        private void MakeButton_Click(object sender, EventArgs e)
        {
            if (BustshotBitmap == null || FaceBitmap == null)
            {
                return;
            }
            int wakuWidth = MakeWakuWidth();
            int wakuHeight = MakeWakuHeight();
            int tileWidth = wakuWidth / 3;
            int tileHeight = wakuHeight / 5;
            {
                string errorMessage = CheckPartsRange(tileWidth, tileHeight);
                if (errorMessage != "")
                {
                    R.ShowStopError(errorMessage);
                    return;
                }
            }

            Bitmap CanvasBitmap = DrawBitmap(0, CATEGORY.PLAIN);

            Bitmap PortraitBitmap = new Bitmap(128, 112);
            Graphics gg = Graphics.FromImage(PortraitBitmap);
            GraphicsSetting(gg);

            //切り取る部分の範囲を決定する
            Rectangle srcRect = new Rectangle((int)this.ClipX.Value, (int)this.ClipY.Value, wakuWidth, wakuHeight);
            Rectangle destRect = new Rectangle(0, 0, 96, 80);
            //ベースを切り抜く
            gg.DrawImage(CanvasBitmap, destRect, srcRect, GraphicsUnit.Pixel);

            //MAP顔
            destRect = new Rectangle(96, 16, 32, 16 * 2);
            gg.DrawImage(CanvasBitmap, destRect, srcRect, GraphicsUnit.Pixel);

            int eyeX = (((int)this.EyeX.Value - (int)this.ClipX.Value) / tileWidth) * tileWidth + (int)this.ClipX.Value;
            int eyeY = (((int)this.EyeY.Value - (int)this.ClipY.Value) / tileHeight) * tileHeight + (int)this.ClipY.Value;
            int mouthX = (((int)this.MouthX.Value - (int)this.ClipX.Value) / tileWidth) * tileWidth + (int)this.ClipX.Value;
            int mouthY = (((int)this.MouthY.Value - (int)this.ClipY.Value) / tileHeight) * tileHeight + (int)this.ClipY.Value;
            Rectangle eycSrcRect = new Rectangle(eyeX, eyeY, tileWidth, tileHeight);
            Rectangle mouthSrcRect = new Rectangle(mouthX, mouthY, tileWidth, tileHeight);

            //目の位置枠 とろん目
            CanvasBitmap = DrawBitmap(2, CATEGORY.TYPE_EYE);
            destRect = new Rectangle(96, 16 * 3, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, eycSrcRect, GraphicsUnit.Pixel);
            //とろん目の口
            CanvasBitmap = DrawBitmap(2, CATEGORY.TYPE_MOUTH);
            destRect = new Rectangle(32 * 2, 16 * 5, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, mouthSrcRect, GraphicsUnit.Pixel);

            //目の位置枠 閉じ目
            CanvasBitmap = DrawBitmap(7, CATEGORY.TYPE_EYE);
            destRect = new Rectangle(96, 16 * 4, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, eycSrcRect, GraphicsUnit.Pixel);
            //閉じ目の口は通常の口を利用しにないとダメ. ステータス画面に使われる.
            CanvasBitmap = DrawBitmap(0, CATEGORY.TYPE_MOUTH);
            destRect = new Rectangle(96, 16 * 5, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, mouthSrcRect, GraphicsUnit.Pixel);

            //笑い目の口
            CanvasBitmap = DrawBitmap(6, CATEGORY.TYPE_MOUTH);
            destRect = new Rectangle(32 * 2, 16 * 6, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, mouthSrcRect, GraphicsUnit.Pixel);

            //大開の口
            CanvasBitmap = DrawBitmap(1, CATEGORY.TYPE_MOUTH);
            destRect = new Rectangle(32 * 0, 16 * 5, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, mouthSrcRect, GraphicsUnit.Pixel);

            CanvasBitmap = DrawBitmap(1, CATEGORY.TYPE_MOUTH);
            destRect = new Rectangle(32 * 0, 16 * 6, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, mouthSrcRect, GraphicsUnit.Pixel);

            //驚き顔の口
            CanvasBitmap = DrawBitmap(4, CATEGORY.TYPE_MOUTH);
            destRect = new Rectangle(32 * 1, 16 * 5, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, mouthSrcRect, GraphicsUnit.Pixel);

            //戸惑い顔の口
            CanvasBitmap = DrawBitmap(5, CATEGORY.TYPE_MOUTH);
            destRect = new Rectangle(32 * 1, 16 * 6, 32, 16);
            gg.DrawImage(CanvasBitmap, destRect, mouthSrcRect, GraphicsUnit.Pixel);


            if (DecreaseColorCheckBox.Checked == true)
            {
                Bitmap PortraitBitmap16 = DecreaseColor16(PortraitBitmap, tileWidth, tileHeight);
                ImageFormRef.ExportImage(this,PortraitBitmap16, "Portrait", 1);
                //エクスプローラで選択は自動的にやってくれる.
                //U.SelectFileByExplorer(savefilename);
            }
            else
            {   //そのまま保存.
                string savefilename = ImageFormRef.ExportImageLow(this, PortraitBitmap, "Portrait");
                //エクスプローラで選択
                U.SelectFileByExplorer(savefilename);
            }

            gg.Dispose();
        }

        string CheckPartsRange(int tileWidth, int tileHeight)
        {
            int face_eye_x_id = (((int)this.EyeX.Value - (int)this.ClipX.Value) / tileWidth);
            int face_eye_y_id = (((int)this.EyeY.Value - (int)this.ClipY.Value) / tileHeight);

            int face_eye_x_id2 = (((int)this.EyeX.Value - (int)this.ClipX.Value + (int)this.EyeW.Value) / tileWidth);
            int face_eye_y_id2 = (((int)this.EyeY.Value - (int)this.ClipY.Value + (int)this.EyeH.Value) / tileHeight);

            if (   face_eye_x_id != face_eye_x_id2 
                || face_eye_y_id != face_eye_y_id2)
            {
                return R.Error("目の位置が、複数のセルに分断されています。一つのセルに収める必要があります。");
            }

            int face_mouth_x_id = (((int)this.MouthX.Value - (int)this.ClipX.Value) / tileWidth);
            int face_mouth_y_id = (((int)this.MouthY.Value - (int)this.ClipY.Value) / tileHeight);

            int face_mouth_x_id2 = (((int)this.MouthX.Value - (int)this.ClipX.Value + (int)this.MouthW.Value) / tileWidth);
            int face_mouth_y_id2 = (((int)this.MouthY.Value - (int)this.ClipY.Value + (int)this.MouthH.Value) / tileHeight);
            if (face_mouth_x_id != face_mouth_x_id2
                || face_mouth_y_id != face_mouth_y_id2)
            {
                return R.Error("口の位置が、複数のセルに分断されています。一つのセルに収める必要があります。");
            }
            return "";
        }

        Bitmap DecreaseColor16(Bitmap bitmap, int tileWidth, int tileHeight)
        {
            //16色に減色.
            DecreaseColor d = new DecreaseColor();
            Bitmap bitmap16 = d.Convert(bitmap, 1, 0, true, false );

            //縁取りすると綺麗に見える
            //黒色を探す
            int blackColorIndex = ImageUtil.FindBlackColorFromPalette(bitmap16, 1, 16);
            //メイン
            bitmap16 = ImageUtil.Fuchidori(bitmap16, (byte)blackColorIndex, new Rectangle(0, 0, 96, 80));
            //パーツをきれいに見せるために再構成する.
            int face_eye_x_id = (((int)this.EyeX.Value - (int)this.ClipX.Value) / tileWidth);
            int face_eye_y_id = (((int)this.EyeY.Value - (int)this.ClipY.Value) / tileHeight);

            int seet_eye_x = (int)(32 * (this.EyeX.Value - (int)this.ClipX.Value - (face_eye_x_id * tileWidth))) / tileWidth;
            int seet_eye_w = (int)Math.Ceiling((32 * (this.EyeW.Value)) / tileWidth);
            int seet_eye_y = (int)(16 * (this.EyeY.Value - (int)this.ClipY.Value - (face_eye_y_id * tileHeight))) / tileHeight;
            int seet_eye_h = (int)Math.Ceiling((16 * (this.EyeH.Value)) / tileHeight);

            Bitmap tempBitmap;
            tempBitmap = ImageUtil.Copy(bitmap16, face_eye_x_id * 32, face_eye_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_eye_x, seet_eye_y, seet_eye_w, seet_eye_h, bitmap16, seet_eye_x + 96, seet_eye_y + 16 * 3);
            ImageUtil.BitBlt(bitmap16, 96, 16 * 3, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_eye_x_id * 32, face_eye_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_eye_x, seet_eye_y, seet_eye_w, seet_eye_h, bitmap16, seet_eye_x + 96, seet_eye_y + 16 * 4);
            ImageUtil.BitBlt(bitmap16, 96, 16 * 4, 32, 16, tempBitmap, 0, 0);


            int face_mouth_x_id = (((int)this.MouthX.Value - (int)this.ClipX.Value) / tileWidth);
            int face_mouth_y_id = (((int)this.MouthY.Value - (int)this.ClipY.Value) / tileHeight);

            int seet_mouth_x = (int)(32 * (this.MouthX.Value - (int)this.ClipX.Value - (face_mouth_x_id * tileWidth))) / tileWidth;
            int seet_mouth_w = (int)Math.Ceiling((32 * (this.MouthW.Value)) / tileWidth);
            int seet_mouth_y = (int)(16 * (this.MouthY.Value - (int)this.ClipY.Value - (face_mouth_y_id * tileHeight))) / tileHeight;
            int seet_mouth_h = (int)Math.Ceiling((16 * (this.MouthH.Value)) / tileHeight);
            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 32, face_mouth_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 0, seet_mouth_y + 16 * 5);
            ImageUtil.BitBlt(bitmap16, 32 * 0, 16 * 5, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 32, face_mouth_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 1, seet_mouth_y + 16 * 5);
            ImageUtil.BitBlt(bitmap16, 32 * 1, 16 * 5, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 32, face_mouth_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 2, seet_mouth_y + 16 * 5);
            ImageUtil.BitBlt(bitmap16, 32 * 2, 16 * 5, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 32, face_mouth_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 3, seet_mouth_y + 16 * 5);
            ImageUtil.BitBlt(bitmap16, 32 * 3, 16 * 5, 32, 16, tempBitmap, 0, 0);


            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 32, face_mouth_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 0, seet_mouth_y + 16 * 6);
            ImageUtil.BitBlt(bitmap16, 32 * 0, 16 * 6, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 32, face_mouth_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 1, seet_mouth_y + 16 * 6);
            ImageUtil.BitBlt(bitmap16, 32 * 1, 16 * 6, 32, 16, tempBitmap, 0, 0);

            tempBitmap = ImageUtil.Copy(bitmap16, face_mouth_x_id * 32, face_mouth_y_id * 16, 32, 16);
            ImageUtil.BitBlt(tempBitmap, seet_mouth_x, seet_mouth_y, seet_mouth_w, seet_mouth_h, bitmap16, seet_mouth_x + 32 * 2, seet_mouth_y + 16 * 6);
            ImageUtil.BitBlt(bitmap16, 32 * 2, 16 * 6, 32, 16, tempBitmap, 0, 0);

            return bitmap16;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            Control active = this.ActiveControl;
            if (active == FaceX || active == FaceY)
            {
                FaceX.Value = e.X;
                FaceY.Value = e.Y;
            }
            else if (active == ClipX || active == ClipY)
            {
                ClipX.Value = e.X;
                ClipY.Value = e.Y;
            }
            else if (active == EyeX || active == EyeY)
            {
                EyeX.Value = e.X;
                EyeY.Value = e.Y;
            }
            else if (active == EyeW || active == label123)
            {
                if (e.X <= EyeX.Value || e.Y <= EyeY.Value)
                {
                    return;
                }
                EyeW.Value = e.X - EyeX.Value;
                EyeH.Value = e.Y - EyeY.Value;
            }
            else if (active == MouthX || active == MouthY)
            {
                MouthX.Value = e.X;
                MouthY.Value = e.Y;
            }
            else if (active == MouthW || active == MouthH)
            {
                if (e.X <= MouthX.Value || e.Y <= MouthY.Value)
                {
                    return;
                }
                MouthW.Value = e.X - MouthX.Value;
                MouthH.Value = e.Y - MouthY.Value;
            }
        }

        //表情画像位置の自動調整
        void AjastFace()
        {
            Debug.Assert(BustshotBitmap != null);
            Debug.Assert(FaceBitmap != null);
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            int canvasWidth = this.Canvas.Width;
            int canvasHeight = this.Canvas.Height;
            int width = FaceBitmap.Width / 4;
            int height = FaceBitmap.Height / 2;

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                for (int size = 100; size < 300; size++)
                {
                    pleaseWait.DoEvents(R._("表情画像の位置を検索中:{0}/300",size));

                    //表情を描画する
                    Bitmap facetemp = new Bitmap(size, size);
                    Graphics gg = Graphics.FromImage(facetemp);
                    GraphicsSetting(gg);

                    Rectangle srcRect = new Rectangle(0, 0, width, height);
                    Rectangle destRect = new Rectangle(0, 0, size, size);
                    gg.DrawImage(FaceBitmap, destRect, srcRect, GraphicsUnit.Pixel);

                    uint LU = ImageUtil.ColorToGBARGB(facetemp.GetPixel(0, 0));
                    uint LC = ImageUtil.ColorToGBARGB(facetemp.GetPixel(size/2, 0));
                    uint LD = ImageUtil.ColorToGBARGB(facetemp.GetPixel(0, size - 1));
                    uint RU = ImageUtil.ColorToGBARGB(facetemp.GetPixel(size - 1, 0));
                    uint RC = ImageUtil.ColorToGBARGB(facetemp.GetPixel(0, size / 2));
                    uint RD = ImageUtil.ColorToGBARGB(facetemp.GetPixel(size - 1, size - 1));
                    uint CC = ImageUtil.ColorToGBARGB(facetemp.GetPixel(size / 2, size / 2));
                    gg.Dispose();
                    facetemp.Dispose();

                    //マッチする左上があるかどうか探す.
                    int searchWidth = BustshotBitmap.Width - size;
                    int searchHeight = BustshotBitmap.Height - size;
                    for (int y = 0; y < searchHeight; y++)
                    {
                        for (int x = 0; x < searchWidth; x++)
                        {
                            uint tCC = ImageUtil.ColorToGBARGB(BustshotBitmap.GetPixel(x + size / 2, y + size / 2));
                            if (CC != tCC)
                            {
                                continue;
                            }
                            uint tLU = ImageUtil.ColorToGBARGB(BustshotBitmap.GetPixel(x, y));
                            if (LU != tLU)
                            {
                                continue;
                            }
                            uint tLC = ImageUtil.ColorToGBARGB(BustshotBitmap.GetPixel(x + size / 2, y));
                            if (LC != tLC)
                            {
                                continue;
                            }
                            uint tLD = ImageUtil.ColorToGBARGB(BustshotBitmap.GetPixel(x, y + size - 1));
                            if (LD != tLD)
                            {
                                continue;
                            }
                            uint tRC = ImageUtil.ColorToGBARGB(BustshotBitmap.GetPixel(x, y + size / 2));
                            if (RC != tRC)
                            {
                                continue;
                            }
                            uint tRU = ImageUtil.ColorToGBARGB(BustshotBitmap.GetPixel(x + size - 1, y));
                            if (RU != tRU)
                            {
                                continue;
                            }
                            uint tRD = ImageUtil.ColorToGBARGB(BustshotBitmap.GetPixel(x + size - 1, y + size - 1));
                            if (RD != tRD)
                            {
                                continue;
                            }
                            //マッチ!
                            int widthpadding = (canvasWidth - BustshotBitmap.Width) / 2;
                            int heightpadding = PADDING_HEIGHT;

                            this.FaceX.Value = x + widthpadding;
                            this.FaceY.Value = y + heightpadding;
                            this.FaceW.Value = size;


                            return;
                        }
                    }
                }
            }
        }
/*
        //目と口の自動認識
        void AjastEyeMouth()
        {
            Debug.Assert(BustshotBitmap != null);
            Debug.Assert(FaceBitmap != null);

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                pleaseWait.DoEvents(R._("目と口を検出中... {0}",1));

                int width = FaceBitmap.Width / 4;
                int height = FaceBitmap.Height / 2;

                bool[] change = new bool[width * height];
                uint[] baseface = new uint[width * height];
                {
                    //ベースの顔
                    Bitmap basefacetemp = new Bitmap(width, height);
                    Graphics g = Graphics.FromImage(basefacetemp);
                    GraphicsSetting(g);

                    Rectangle srcRect = new Rectangle(0, 0, width, height);
                    Rectangle destRect = new Rectangle(0, 0, width, height);
                    g.DrawImage(FaceBitmap, destRect, srcRect, GraphicsUnit.Pixel);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            uint a = ImageUtil.ColorToGBARGB(basefacetemp.GetPixel(x, y));
                            baseface[x + y * width] = a;
                        }
                    }
                    g.Dispose();
                    basefacetemp.Dispose();
                }

                pleaseWait.DoEvents(R._("目と口を検出中... {0}", 2));
                for (int facetype = 1; facetype < 8; facetype++)
                {
                    int face_x = GetFaceX(facetype);
                    int face_y = GetFaceY(facetype);

                    //表情を描画する
                    Bitmap facetemp = new Bitmap(width, height);
                    Graphics g = Graphics.FromImage(facetemp);
                    GraphicsSetting(g);

                    Rectangle srcRect = new Rectangle(face_x, face_y, width, height);
                    Rectangle destRect = new Rectangle(0, 0, width, height);
                    g.DrawImage(FaceBitmap, destRect, srcRect, GraphicsUnit.Pixel);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            uint c = baseface[x + y * width];
                            uint a = ImageUtil.ColorToGBARGB(facetemp.GetPixel(x, y));
                            if (c != a)
                            {
                                change[x + y * width] = true;
                            }
                        }
                    }
                    g.Dispose();
                }

                pleaseWait.DoEvents(R._("目と口を検出中... {0}", 3));
                List<Rectangle> rects = new List<Rectangle>();
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (!change[x + y * width])
                            {
                                continue;
                            }
                            AjastEyeMouthPaint paint = new AjastEyeMouthPaint(change,width,height);
                            paint.SearchDirectionHint(x,y);
                            Rectangle a = paint.getRect();
                            if (a.Width <= 1 || a.Height <= 1)
                            {//小さい変更を拾っても仕方ないため.
                                continue;
                            }
                            rects.Add(a);
                        }
                    }
                }

                if (rects.Count <= 1)
                {//発見できず.
                    return;
                }

                {
                    //一番横に長い変更点が目だろう.
                    int maxI = 0;
                    int maxWidth = 0;
                    for (int i = 0; i < rects.Count; i++)
                    {
                        if (rects[i].Width > maxWidth)
                        {
                            maxI = i;
                            maxWidth = rects[i].Width;
                        }
                    }

                    Rectangle c = rects[maxI];
                    this.EyeX.Value = c.X + (int)this.FaceX.Value;
                    this.EyeY.Value = c.Y + (int)this.FaceY.Value;
                    this.EyeW.Value = c.Width;
                    this.EyeH.Value = c.Height;
                }
                {
                    //一番下にある地点が口だろう.
                    int maxI = 0;
                    int maxY = 0;
                    for (int i = 0; i < rects.Count; i++)
                    {
                        if (rects[i].Y > maxY)
                        {
                            maxI = i;
                            maxY = rects[i].Y;
                        }
                    }

                    Rectangle c = rects[maxI];
                    this.MouthX.Value = c.X + (int)this.FaceX.Value;
                    this.MouthY.Value = c.Y + (int)this.FaceY.Value;
                    this.MouthW.Value = c.Width;
                    this.MouthH.Value = c.Height;
                }
            }
        }
        class AjastEyeMouthPaint
        {
            bool[] Change;
            int Width;
            int Height;
            Point Min;
            Point Max;
            public AjastEyeMouthPaint(bool[] change, int width, int height)
            {
                this.Change = change;
                this.Width = width;
                this.Height = height;
                this.Min = new Point(width, height);
                this.Max = new Point(0, 0);
            }
            public Rectangle getRect()
            {
                return new Rectangle(this.Min.X, this.Min.Y, this.Max.X - this.Min.X + 1, this.Max.Y - this.Min.Y + 1);
            }
            public void SearchDirectionHint(int x,int y)
            {
                this.Change[x + y * this.Width] = false;
                if (x < this.Min.X)
                {
                    this.Min.X = x;
                }
                if (x > this.Max.X)
                {
                    this.Max.X = x;
                }
                if (y < this.Min.Y)
                {
                    this.Min.Y = y;
                }
                if (y > this.Max.Y)
                {
                    this.Max.Y = y;
                }

                if (x > 0 && this.Change[(x - 1) + y * this.Width])
                {
                    SearchDirectionHint(x - 1, y);
                }
                if (x < Width - 1 && this.Change[(x + 1) + y * this.Width])
                {
                    SearchDirectionHint(x + 1, y);
                }
                if (y > 0 && this.Change[x + (y - 1) * this.Width])
                {
                    SearchDirectionHint(x,y - 1);
                }
                if (y < Height - 1 && this.Change[x + (y + 1) * this.Width])
                {
                    SearchDirectionHint(x, y + 1);
                }
            }
        }
*/
        //疑似3D Y軸っぽい回転
        //C#では3D回転はないのだけど(C#では4隅の自由変換はできない)。
        //しかし、掲示板で面白い解法を見つけた人がいて、
        //ラインごとに画像をきりながら、強引に台形変換をやっている人がいたので真似してみる.
        //あまりに傾けると、おかしくなるのでちょっとだけの傾きに利用できそう.
        public Bitmap Rotate3DY(Bitmap bitmap,int hosei )
        {
            Bitmap temp = new Bitmap(bitmap.Width,bitmap.Height);
            Graphics gg = Graphics.FromImage(temp);
            GraphicsSetting(gg);

            int widthpadding = (this.Canvas.Width - BustshotBitmap.Width) / 2;

            int start = (bitmap.Height - PADDING_HEIGHT) - hosei;
            int end = (bitmap.Height-PADDING_HEIGHT) + hosei;
            double subH = (end - start) / (double)bitmap.Height;
            start = (bitmap.Width - (widthpadding*2)) - hosei;
            end = (bitmap.Width - (widthpadding*2)) + hosei;
            double subW = (end - start) / (double)bitmap.Width;

            int addXW = (int)(1 / subW);
            int addXH = (int)(1 / subH);
            addXW = Math.Max(1, addXW);
            addXH = Math.Max(1, addXH);

            double x = hosei - (subW * PADDING_HEIGHT);
            double y = hosei - (subH * widthpadding);
            for (int srcx = widthpadding; srcx < bitmap.Width - widthpadding; srcx += addXH)
            {
                Rectangle destRect = new Rectangle(
                      srcx + (int)Math.Round(x)
                    , PADDING_HEIGHT + (int)Math.Round(y)
                    , addXH
                    , (int)Math.Round(bitmap.Height - (y * 2)));
                Rectangle srcRect = new Rectangle(
                      srcx
                    , PADDING_HEIGHT
                    , addXH
                    , bitmap.Height);
                gg.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);

                x -= subW * addXW;
                y -= subH * addXH;
            }
            gg.Dispose();
            return temp;
        }

        private void ToolPortraitMakerForm_Load(object sender, EventArgs e)
        {

        }


    }
}
