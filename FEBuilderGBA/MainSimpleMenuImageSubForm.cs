using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MainSimpleMenuImageSubForm : Form
    {
        public MainSimpleMenuImageSubForm()
        {
            InitializeComponent();

            ImageBGButton.BackgroundImage = MakeTransparent(Trim(ImageBGForm.DrawBG(0)));

            if (Program.ROM.RomInfo.version() >= 7)
            {
                BigCGButton.BackgroundImage = MakeTransparent(Trim(ImageCGForm.DrawImageByID(0)));
            }
            else
            {
                BigCGButton.Hide();
            }
            
            ImagePortraitButton.BackgroundImage = MakeTransparent(ImagePortraitForm.DrawPortraitUnit(2));
            ImageBattleAnimeButton.BackgroundImage = MakeTransparent(BattleZoom(ImageBattleAnimeForm.DrawBattleAnime(1, ImageBattleAnimeForm.ScaleTrim.SCALE_90)));
            ImageUnitWaitIconButton.BackgroundImage = MakeTransparent(ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(1,0,false));
            ImageUnitMoveIconButton.BackgroundImage = MakeTransparent(ImageUnitMoveIconFrom.DrawMoveUnitIconBitmap(3, 0,0));
            ImageIconButton.BackgroundImage = MakeTransparent(ImageItemIconForm.DrawIconWhereID(0xB));
            SystemIconButton.BackgroundImage = MakeTransparent(ImageSystemIconForm.YubiYoko());
            //BattleScreenButton.BackgroundImage = MakeTransparent(ImageSystemIconForm.Allows(8));
            BattleScreenButton.BackgroundImage = MakeTransparent(ImageSystemIconForm.WeaponIcon(0));

            if (Program.ROM.RomInfo.version() == 8
                || Program.ROM.RomInfo.version() == 7
                )
            {
                ImageBattleFieldButton.BackgroundImage = MakeTransparent(ImageBattleBGForm.DrawBG(2));
            }
            else
            {//FE6
                ImageBattleFieldButton.Hide();
            }
            
            ImageBattleTerrainButton.BackgroundImage = MakeTransparent((ImageBattleTerrainForm.Draw(2)));

            if (Program.ROM.RomInfo.version() == 8)
            {
                ImageUnitPaletteButton.BackgroundImage = MakeTransparent(BattleZoom(UnitPaletteForm.DrawSample(2, 3)));
            }
            else 
            {//for FE6 , FE7
                ImageUnitPaletteButton.BackgroundImage = MakeTransparent(BattleZoom(ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID(UnitForm.GetClassID(2))
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90, UnitForm.GetPaletteLowClass(2))));
            }

            WorldMapImageButton.BackgroundImage = MakeTransparent(WorldMapImageForm.DrawWorldMap());

            if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
            {//英語版FE7は、章タイトルをテキストで保持していて、40260c nazo fontで、描画している.
                ImageChapterTitleButton.Hide();
            }
            else
            {
                ImageChapterTitleButton.BackgroundImage = MakeTransparent(ImageChapterTitleForm.DrawSample(0));
            }
            

            if (ImageUtilMagic.SearchMagicSystem() != ImageUtilMagic.magic_system_enum.NO)
            {
                ImageMagicButton.BackgroundImage = MakeTransparent(ImageSystemIconForm.WeaponIcon(8-3));
            }
            else
            {
                ImageMagicButton.Hide();
            }

            if (Program.ROM.RomInfo.is_multibyte())
            {
                OptionForm.textencoding_enum textencoding = OptionForm.textencoding();
                if (textencoding == OptionForm.textencoding_enum.ZH_TBL)
                {
//                    FontButton.BackgroundImage = MakeTransparent(FontZHForm.DrawFontString("字形", true));
                }
                else
                {
                    FontButton.BackgroundImage = MakeTransparent(FontForm.DrawFontString("フォント", true));
                }
            }
            else
            {
                FontButton.BackgroundImage = MakeTransparent(FontForm.DrawFontString("Font", true));
            }
        }
        private void MainSimpleMenuImageSubForm_Load(object sender, EventArgs e)
        {
            //文字が読みにくいのでTextShadowをかける.
            //だけど、Button の OwnerDrawを止める方法がないので、テキストをTagに移して消し去るw
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    c.Tag = c.Text;
                    c.Text = "";
                }
            }
        }

        Bitmap MakeTransparent(Bitmap bitmap)
        {
            int palettecount = ImageUtil.GetPalette16Count(bitmap);
            ImageUtil.BlackOutUnnecessaryColors(bitmap, palettecount);
            U.MakeTransparent(bitmap);
            return bitmap;
        }
        Bitmap Trim(Bitmap bitmap)
        {
            if (bitmap.Width < 16)
            {
                return bitmap;
            }
            return ImageUtil.Copy(bitmap, 0, 0, bitmap.Width - 16, bitmap.Height);
        }
        Bitmap BattleZoom(Bitmap bitmap)
        {
            if (bitmap.Width < 16)
            {
                return bitmap;
            }
            return ImageUtil.Copy(bitmap, 16, 32, bitmap.Width-16, bitmap.Height-48);
        }
        Bitmap BattleTerrainCenter(Bitmap bitmap)
        {
            if (bitmap.Width < 16)
            {
                return bitmap;
            }
            Bitmap bigsize = ImageUtil.Blank(bitmap.Width, bitmap.Height * 3, bitmap);
            ImageUtil.BitBlt(bigsize, 0, bitmap.Height * 2, bitmap.Width, bitmap.Height, bitmap, 0, 0);
            return bigsize;
        }


        private void ImageBGButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBGForm>();
        }

        private void BigCGButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                InputFormRef.JumpForm<ImageCGForm>();
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (!Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<ImageCGFE7UForm>();
                }
                else
                {
                    InputFormRef.JumpForm<ImageCGForm>();
                }
            }
        }

        private void ImagePortraitButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                InputFormRef.JumpForm<ImagePortraitFE6Form>();
            }
            else
            {
                InputFormRef.JumpForm<ImagePortraitForm>();
            }
        }

        private void ImageBattleAnimeButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleAnimeForm>(0,"N_AddressList");
        }

        private void ImageUnitWaitIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageUnitWaitIconFrom>();
        }

        private void ImageUnitMoveIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageUnitMoveIconFrom>();
        }

        private void ImageIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageItemIconForm>();
        }

        private void SystemIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageSystemIconForm>();
        }

        private void ImageBattleFieldButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleBGForm>();
        }

        private void ImageBattleTerrainButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleTerrainForm>();
        }

        private void ImageUnitPaletteButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageUnitPaletteForm>();
        }

        private void WorldMapImageButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                InputFormRef.JumpForm<WorldMapImageForm>();
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                InputFormRef.JumpForm<WorldMapImageFE7Form>();
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                InputFormRef.JumpForm<WorldMapImageFE6Form>();
            }
        }

        private void ImageChapterTitleButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                InputFormRef.JumpForm<ImageChapterTitleForm>();
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<ImageChapterTitleFE7Form>();
                }
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                InputFormRef.JumpForm<ImageChapterTitleFE7Form>(); //FE6 とFe7は同一フォーマット
            }
        }

        private void ImageMagicButton_Click(object sender, EventArgs e)
        {
            ImageUtilMagic.magic_system_enum magic = ImageUtilMagic.SearchMagicSystem();
            if (magic == ImageUtilMagic.magic_system_enum.FEDITOR_ADV)
            {
                InputFormRef.JumpForm<ImageMagicFEditorForm>();
            }
            else if (magic == ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                InputFormRef.JumpForm<ImageMagicCSACreatorForm>();
            }
        }

        private void BattleScreenButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleScreenForm>();
        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.is_multibyte())
            {
                OptionForm.textencoding_enum textencoding = OptionForm.textencoding();
                if (textencoding == OptionForm.textencoding_enum.ZH_TBL)
                {
                    InputFormRef.JumpForm<FontZHForm>();
                    return;
                }
            }
            InputFormRef.JumpForm<FontForm>();
        }

        //文字が読みずらいので、TextShadowぽいのを作る
        //see http://www.r-nakai.com/archives/280
        private void Button_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;

            //アンチエリアス
            System.Drawing.Drawing2D.SmoothingMode initialMode
                                        = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode =
                   System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //レンダリング品質
            System.Drawing.Drawing2D.CompositingQuality initialQuality
                                          = e.Graphics.CompositingQuality;
            e.Graphics.CompositingQuality
                = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            string str = (String)btn.Tag;

            SizeF size = e.Graphics.MeasureString(str, btn.Font);
            int x = (int)((btn.Width / 2) - (size.Width / 1.5));
            int y = btn.Height - btn.Font.Height - 10;

            System.Drawing.Drawing2D.GraphicsPath gp =
                new System.Drawing.Drawing2D.GraphicsPath();

            gp.AddString(str, btn.Font.FontFamily
                            ,(int)FontStyle.Regular
                            ,btn.Font.Size+10
                            ,new Point(x, y), StringFormat.GenericDefault);

            //パスの線分を描画
            Pen drawPen = new Pen(btn.BackColor, 3.0F);
            e.Graphics.DrawPath(drawPen, gp);

            //塗る
            Brush fillBrush = new SolidBrush(btn.ForeColor);

            e.Graphics.FillPath(fillBrush, gp);

            drawPen.Dispose();
            fillBrush.Dispose();

            //念のため元に戻しておきます
            e.Graphics.SmoothingMode = initialMode;
            e.Graphics.CompositingQuality = initialQuality;
        }

    }
}
