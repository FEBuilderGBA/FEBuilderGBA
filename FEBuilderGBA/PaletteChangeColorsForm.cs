using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class PaletteChangeColorsForm : Form
    {
        RefColorSt[] RefColor;
        public PaletteChangeColorsForm()
        {
            InitializeComponent();

            RefColor = new RefColorSt[16 + 1];
            RefColor[1] = new RefColorSt();
            RefColor[1].CheckBox = this.Ref01;
            RefColor[1].Panel = this.Ref01Color;

            RefColor[2] = new RefColorSt();
            RefColor[2].CheckBox = this.Ref02;
            RefColor[2].Panel = this.Ref02Color;

            RefColor[3] = new RefColorSt();
            RefColor[3].CheckBox = this.Ref03;
            RefColor[3].Panel = this.Ref03Color;

            RefColor[4] = new RefColorSt();
            RefColor[4].CheckBox = this.Ref04;
            RefColor[4].Panel = this.Ref04Color;

            RefColor[5] = new RefColorSt();
            RefColor[5].CheckBox = this.Ref05;
            RefColor[5].Panel = this.Ref05Color;

            RefColor[6] = new RefColorSt();
            RefColor[6].CheckBox = this.Ref06;
            RefColor[6].Panel = this.Ref06Color;

            RefColor[7] = new RefColorSt();
            RefColor[7].CheckBox = this.Ref07;
            RefColor[7].Panel = this.Ref07Color;

            RefColor[8] = new RefColorSt();
            RefColor[8].CheckBox = this.Ref08;
            RefColor[8].Panel = this.Ref08Color;

            RefColor[9] = new RefColorSt();
            RefColor[9].CheckBox = this.Ref09;
            RefColor[9].Panel = this.Ref09Color;

            RefColor[10] = new RefColorSt();
            RefColor[10].CheckBox = this.Ref10;
            RefColor[10].Panel = this.Ref10Color;

            RefColor[11] = new RefColorSt();
            RefColor[11].CheckBox = this.Ref11;
            RefColor[11].Panel = this.Ref11Color;

            RefColor[12] = new RefColorSt();
            RefColor[12].CheckBox = this.Ref12;
            RefColor[12].Panel = this.Ref12Color;

            RefColor[13] = new RefColorSt();
            RefColor[13].CheckBox = this.Ref13;
            RefColor[13].Panel = this.Ref13Color;

            RefColor[14] = new RefColorSt();
            RefColor[14].CheckBox = this.Ref14;
            RefColor[14].Panel = this.Ref14Color;

            RefColor[15] = new RefColorSt();
            RefColor[15].CheckBox = this.Ref15;
            RefColor[15].Panel = this.Ref15Color;

            RefColor[16] = new RefColorSt();
            RefColor[16].CheckBox = this.Ref16;
            RefColor[16].Panel = this.Ref16Color;
        }

        private void PaletteChangeColorsForm_Load(object sender, EventArgs e)
        {
            AutoSelectColor();
        }

        int MainColorIndex = 1;
        public void SetMainColorIndex(int index)
        {
            this.MainColorIndex = index;
        }

        Bitmap Bitmap;
        public void SetPreviewBitmap(Bitmap bitmap)
        {
            this.Bitmap = (Bitmap)bitmap.Clone();
            this.Preview.Image = bitmap;
        }
        
        class RefColorSt
        {
            public CheckBox CheckBox;
            public Panel Panel;
            public Color NewRGB;
        }

        public void SetColor(int index, int r, int g, int b)
        {
            Color rgb = Color.FromArgb(r, g, b);
            if (index == this.MainColorIndex)
            {
                RefColor[index].CheckBox.Checked = true;
                RefColor[index].CheckBox.Enabled = false;
                OldColor.BackColor = rgb;
                OldColorInfo.Text = "R:" + rgb.R + " G:" + rgb.G + " B:" + rgb.B;

                NewColor.BackColor = rgb;
                NewColorR.Value = r;
                NewColorG.Value = g;
                NewColorB.Value = b;
            }
            else
            {
                RefColor[index].CheckBox.Checked = true;
                RefColor[index].CheckBox.Enabled = true;
            }

            RefColor[index].Panel.BackColor = rgb;
            RefColor[index].NewRGB = rgb;
        }

        private void NewColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = NewColor.BackColor;
            if (cd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.NewColorR.Value = cd.Color.R;
            this.NewColorG.Value = cd.Color.G;
            this.NewColorB.Value = cd.Color.B;
            NewColor.BackColor = Color.FromArgb((int)this.NewColorR.Value, (int)this.NewColorG.Value, (int)this.NewColorB.Value);
            UpdateSample();
        }
        private void NewColorR_ValueChanged(object sender, EventArgs e)
        {
            if (U.IsOrderOfHuman(sender))
            {
                NewColor.BackColor = Color.FromArgb((int)this.NewColorR.Value, (int)this.NewColorG.Value, (int)this.NewColorB.Value);
                UpdateSample();
            }
        }
        private void Ref01_CheckedChanged(object sender, EventArgs e)
        {
            if (U.IsOrderOfHuman(sender))
            {
                UpdateSample();
            }
        }
        private void ResetButton_Click(object sender, EventArgs e)
        {
            NewColor.BackColor = OldColor.BackColor; 
            UpdateSample();
        }

        void AutoSelectColor(double filter = 80f)
        {
            Color mainRGB = OldColor.BackColor;

            for (int i = 1 ; i < 16 + 1; i++)
            {
                if (i == this.MainColorIndex)
                {
                    continue;
                }
                if (i == 1)
                {//背景なので無視
                    RefColor[i].CheckBox.Checked = false;
                    continue;
                }

                Color c = RefColor[i].Panel.BackColor;

                double diff = Math.Sqrt(Math.Pow(mainRGB.R - c.R , 2) 
                    + Math.Pow(mainRGB.G - c.G , 2) 
                    + Math.Pow(mainRGB.B - c.B , 2)  );
                Debug.WriteLine(i + " " + diff);
                if (diff < filter)
                {
                    RefColor[i].CheckBox.Checked = true;
                }
                else
                {
                    RefColor[i].CheckBox.Checked = false;
                }
            }
        }

        int LimitColor255(float a)
        {
            if (a < 0)
            {
                return 0;
            }
            if (a > 255)
            {
                return 255;
            }
            if (float.IsInfinity(a))
            {
                return 255;
            }
            if (float.IsNaN(a))
            {
                return 0;
            }
            if (float.IsNegativeInfinity(a))
            {
                return 0;
            }
            if (float.IsPositiveInfinity(a))
            {
                return 0;
            }
            return (int)a;
        }

        void UpdateSample()
        {
            Color oldColor = OldColor.BackColor;
            Color newColor = NewColor.BackColor;

            ColorPalette pal = this.Bitmap.Palette;
            for (int i = 1; i < 16 + 1; i++)
            {

                if (i == this.MainColorIndex)
                {//メインの色
                    RefColor[i].NewRGB = newColor;
                }
                else if (RefColor[i].CheckBox.Checked)
                {//変換する
                    Color c = RefColor[i].Panel.BackColor;

                    int r = LimitColor255((newColor.R - oldColor.R) + c.R);
                    int g = LimitColor255((newColor.G - oldColor.G) + c.G);
                    int b = LimitColor255((newColor.B - oldColor.B) + c.B);

                    RefColor[i].NewRGB = Color.FromArgb(r, g, b);
                }
                else
                {//変換しないのでそのままコピーする
                    RefColor[i].NewRGB = RefColor[i].Panel.BackColor;
                }

                pal.Entries[ i - 1] = RefColor[i].NewRGB;
            }
            this.Bitmap.Palette = pal;
            this.Preview.Image = this.Bitmap;
        }

        public Color GetColor(int index)
        {
            return RefColor[index].NewRGB;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
