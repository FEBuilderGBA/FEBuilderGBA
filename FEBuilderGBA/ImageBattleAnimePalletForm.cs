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
    public partial class ImageBattleAnimePalletForm : Form
    {
        public ImageBattleAnimePalletForm()
        {
            InitializeComponent();
            this.PaletteZoomComboBox.SelectedIndex = 0;
            this.PaletteIndexComboBox.SelectedIndex = 0;
            PaletteFormRef.MakePaletteUI(this, OnChangeColor, GetSampleBitmap);
        }


        private void PALETTE_POINTER_ValueChanged(object sender, EventArgs e)
        {
            if (PALETTE_ADDRESS.Value == 0)
            {
                return;
            }
            PaletteFormRef.MakePaletteROMToUI(this, (uint)PALETTE_ADDRESS.Value,true , this.PaletteIndexComboBox.SelectedIndex);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            DrawSample(this.BattleAnimeID, this.PaletteIndexComboBox.SelectedIndex);
        }
        private void PaletteIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PALETTE_POINTER_ValueChanged(null, null);
        }

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            uint newAddr = PaletteFormRef.MakePaletteUIToROM(this, (uint)PALETTE_ADDRESS.Value, true, this.PaletteIndexComboBox.SelectedIndex );
            if (newAddr == U.NOT_FOUND)
            {
                return;
            }
            PALETTE_ADDRESS.Value = U.toPointer(newAddr);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, newAddr);
        }

        void DrawSample(uint battleAnimeID,int paletteIndex)
        {
            Bitmap[] animeframe = new Bitmap[12];

            uint showsecstion = 0;
            uint showframe = 0;
            for (int index = 0; index < animeframe.Length; index++, showframe += 2)
            {
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , 0, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //何も描画されなければフレームをもうちょっと進めてみる.
                showframe += 2;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , 0, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //それでもだめならセクションを切り替える.
                showsecstion += 1;
                showframe = 0;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , 0, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index],10))
                {
                    continue;
                }
                //さらにダメならもう一つセクションを進める. それでもだめならあきらめる.
                showsecstion += 1;
                showframe = 0;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , 0, showsecstion, showframe, paletteIndex);
            }

            this.DrawBitmap = ImageUtil.Blank(360, 290, animeframe[0]);
            int x = 0;
            int y = 0;
            for (int index = 0; index < animeframe.Length; index++)
            {
                ImageUtil.BitBlt(this.DrawBitmap, x, y, animeframe[index].Width, animeframe[index].Height, animeframe[index], 0, 0);
                x += animeframe[index].Width;
                if (x >= this.DrawBitmap.Width)
                {
                    x = 0;
                    y += animeframe[index].Height;
                }
            }

            //パレットが違うので、1番目に移動しないといけない.
//            this.DrawBitmap =
//                ImageUtil.SwapPaletteAndImage(this.DrawBitmap, this.PaletteIndexComboBox.SelectedIndex);

            ReDrawBitmap();
        }
        Bitmap DrawBitmap;

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
            ColorPalette palette = this.DrawBitmap.Palette; //一度、値をとってからいじらないと無視される
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
                , InputFormRef.MakeSaveImageFilename(this, U.ToHexString(this.BattleAnimeID))
                , 1);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            bool r = PaletteFormRef.MakePaletteBitmapToUIEx(this, 0, this.DrawBitmap);
            if (!r)
            {
                return;
            }

            //書き込み
            PaletteWriteButton.PerformClick();
        }


        uint BattleAnimeID;
        public void JumpTo(uint battleAnimeID, uint paletteAddress, int paletteIndex)
        {
            this.BattleAnimeID = battleAnimeID;
            this.PALETTE_ADDRESS.Value = paletteAddress;
            this.PaletteIndexComboBox.SelectedIndex = paletteIndex;
        }

        private void PALETTE_TO_CLIPBOARD_BUTTON_Click(object sender, EventArgs e)
        {
            bool r = PaletteFormRef.PALETTE_TO_CLIPBOARD_BUTTON_Click(this);
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
    }
}
