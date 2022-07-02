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
            this.Is32ColorMode = false;
            this.PFR = new PaletteFormRef(this);
            PFR.MakePaletteUI(OnChangeColor, GetSampleBitmap);
            SetExpain();

            U.AllowDropFilename(this, new string[] { ".PNG" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    this.ImportButton_Click(null, null);
                }
            });
        }
        PaletteFormRef PFR;
        //32Colorモードかどうか
        bool Is32ColorMode;

        private void PALETTE_POINTER_ValueChanged(object sender, EventArgs e)
        {
            if (PALETTE_ADDRESS.Value == 0)
            {
                return;
            }
            PFR.MakePaletteROMToUI( (uint)PALETTE_ADDRESS.Value,true , this.PaletteIndexComboBox.SelectedIndex);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            if (this.Is32ColorMode)
            {//32colorモード 常に最初のパレットで描画
                DrawSample(this.BattleAnimeID, 0);
            }
            else
            {//通常モード
                DrawSample(this.BattleAnimeID, this.PaletteIndexComboBox.SelectedIndex);
            }
        }
        private void PaletteIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PALETTE_POINTER_ValueChanged(null, null);
        }

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            uint newAddr = PFR.MakePaletteUIToROM((uint)PALETTE_ADDRESS.Value, true, this.PaletteIndexComboBox.SelectedIndex );
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
            if (this.Is32ColorMode)
            {//32colorモード
             //パレットを切り替えられないので、現在の場所を絶対値で求める必要がある
                int palettenoABS = (PaletteIndexComboBox.SelectedIndex * 16) + paletteno;
                palette.Entries[palettenoABS] = color;
            }
            else
            {//通常モード
             //パレットをPlayer,Enemy,Other,4thごとに切り替えるため、そのまま変更するだけでよい
                palette.Entries[paletteno] = color;
            }
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
            bool r = PFR.MakePaletteBitmapToUIEx(0, this.DrawBitmap);
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

            DrawSample(battleAnimeID, 0);
            int palette_count = ImageUtil.GetPalette16Count(this.DrawBitmap);
            if (palette_count >= 2)
            {//32Colorモード
                this.Is32ColorMode = true;
            }
            else
            {//通常モード
                this.Is32ColorMode = false;
            }
            this.Warning32ColorMode.Visible = this.Is32ColorMode;
            this.PaletteIndexComboBox.SelectedIndex = paletteIndex;
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

        private void ImageBattleAnimePalletForm_Load(object sender, EventArgs e)
        {
        }

        void SetExpain()
        {
            Warning32ColorMode.AccessibleDescription = R._("この戦闘アニメーションは、32モードで作られています。\r\n通常、戦闘アニメーションは、自軍(青)、敵軍(赤)、友軍(緑)、グレー(グレー)の4つがあります。\r\n32Colorモードでは、自軍(青)と敵軍(赤)のパレットを同時に利用して、32色のキャラクターを描画します。");
        }

        private void X_PIC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                return;
            }
            PFR.SpoitTool_SelectPalette(this.X_PIC, this.PaletteZoomComboBox.SelectedIndex, e);
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
