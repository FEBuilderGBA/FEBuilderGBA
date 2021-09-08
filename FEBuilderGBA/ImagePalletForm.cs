﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImagePalletForm : Form
    {
        public ImagePalletForm()
        {
            InitializeComponent();
            this.PaletteZoomComboBox.SelectedIndex = 0;
            this.PFR = new PaletteFormRef(this);
            this.PFR.MakePaletteUI(OnChangeColor, GetSampleBitmap);
            
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });
        }
        PaletteFormRef PFR;
        private void PALETTE_POINTER_ValueChanged(object sender, EventArgs e)
        {
            if (PALETTE_ADDRESS.Value == 0)
            {
                return;
            }

            PFR.MakePaletteROMToUI((uint)PALETTE_ADDRESS.Value, false, this.PaletteIndexComboBox.SelectedIndex);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
        }
        private void PaletteIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PALETTE_POINTER_ValueChanged(null, null);
        }

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            uint newAddr = PFR.MakePaletteUIToROM((uint)PALETTE_ADDRESS.Value, false, this.PaletteIndexComboBox.SelectedIndex );
            if (newAddr == U.NOT_FOUND)
            {
                return;
            }
            PALETTE_ADDRESS.Value = U.toPointer(newAddr);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, newAddr);
        }

        Bitmap DrawBitmap;
        Bitmap BaseBitmap;

        Bitmap GetSampleBitmap()
        {
            return this.DrawBitmap;
        }

        private bool OnChangeColor(Color color,int paletteno)
        {
            if (this.DrawBitmap == null)
            {
                return true;
            }

            int palindex = (int)this.PaletteIndexComboBox.SelectedIndex;
            if (palindex > 0)
            {
                paletteno = palindex * 16 + paletteno;
            }

            ColorPalette palette = this.DrawBitmap.Palette; //一度、値をとってからいじらないと無視される
            if (paletteno >= palette.Entries.Length)
            {
                return false;
            }

            palette.Entries[paletteno] = color;
            this.DrawBitmap.Palette = palette;

            ReDrawBitmap();

            return true;
        }
        void ReDrawBitmap()
        {
            PaletteFormRef.SetScaleSampleImage(this.X_PIC,this.AutoScrollPanel, this.DrawBitmap, this.PaletteZoomComboBox.SelectedIndex);
        }


        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (this.DrawBitmap == null)
            {
                return;
            }

            ImageFormRef.ExportImage(this
                , this.DrawBitmap
                , InputFormRef.MakeSaveImageFilename(this, U.ToHexString(this.PALETTE_ADDRESS.Value))
                , this.MaxPaletteCount );
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            bool r = PFR.MakePaletteBitmapToUIEx(this.PaletteIndexComboBox.SelectedIndex, this.DrawBitmap);
            if (!r)
            {
                return;
            }

            //書き込み
            PaletteWriteButton.PerformClick();
        }

        int MaxPaletteCount;
        public void JumpTo(Bitmap baseBitmap, uint paletteAddress, int maxPaletteCount = 16, int DefaultSelectPalette = 0, string[] paletteNames = null)
        {
            Debug.Assert(maxPaletteCount >= 1);
            
            PaletteIndexComboBox.BeginUpdate();
            PaletteIndexComboBox.Items.Clear();
            for(int i = 0 ; i < maxPaletteCount ; i++)
            {
                string name;
                if (paletteNames == null)
                {
                    name = R._("パレット No{0}", i);
                }
                else
                {
                    name = paletteNames[i];
                }
                PaletteIndexComboBox.Items.Add(name);
            }
            PaletteIndexComboBox.EndUpdate();

            if (maxPaletteCount <= 1)
            {
                PaletteIndexComboBox.Hide();
                PaletteIndexLabel.Hide();
            }
            else
            {
                PaletteIndexComboBox.Show();
                PaletteIndexLabel.Show();
            }
            this.MaxPaletteCount = maxPaletteCount;
            this.BaseBitmap = baseBitmap;

            this.DrawBitmap = ImageUtil.CloneBitmap(this.BaseBitmap);
            this.X_PIC.Image = this.DrawBitmap;

            this.PALETTE_ADDRESS.Value = paletteAddress;
            this.PaletteIndexComboBox.SelectedIndex = DefaultSelectPalette;
        }


        private void ImagePalletForm_Load(object sender, EventArgs e)
        {
        }

        private void PALETTE_TO_CLIPBOARD_BUTTON_Click(object sender, EventArgs e)
        {
            bool r = PFR.PALETTE_TO_CLIPBOARD_BUTTON_Click();
            if (r)
            {
                //書き込み
                PaletteWriteButton.PerformClick();
            }
        }

        private void PaletteZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReDrawBitmap();
        }

        public static string GetExplainColor()
        {
            return R._("左クリックで色選択ダイアログから色を選べます。\r\n右クリックで色を交換するすることができます。\r\n");
        }
        public static string GetExplainColor0()
        {
            return GetExplainColor() + R._("通常最初のパレットには、背景色が設定されます。\r\n");
        }

        private void X_PIC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                return;
            }
            PFR.SpoitTool_SelectPalette(this.X_PIC,this.PaletteZoomComboBox.SelectedIndex, e);
        }

        private void UNDOButton_Click(object sender, EventArgs e)
        {
            PFR.RunUndo();
        }

        private void REDOButton_Click(object sender, EventArgs e)
        {
            PFR.RunRedo();
        }




    }
}
