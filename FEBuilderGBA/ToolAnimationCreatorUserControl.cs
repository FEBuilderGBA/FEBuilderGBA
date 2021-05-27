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
    public partial class ToolAnimationCreatorUserControl : UserControl
    {
        public ToolAnimationCreatorUserControl()
        {
            InitializeComponent();

            this.AnimeList = new List<AnimeSt>();
            this.AnimeObj = new Dictionary<string, Bitmap>();
            this.AnimeBG = new Dictionary<string, Bitmap>();

            this.DummyBitmap = ImageUtil.Blank(ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8);
            {
                U.SelectedIndexSafety(this.BattleFocus, 0);

                List<U.AddrResult> list = ImageBattleTerrainForm.MakeList();
                list.Insert(0, new U.AddrResult(1, R._("00 表示しない")));
                U.ConvertComboBox(list, ref this.BattleTerrain);

                if (Program.ROM.RomInfo.version() == 8)
                {
                    U.SelectedIndexSafety(this.BattleTerrain, 0x12 + 1);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    U.SelectedIndexSafety(this.BattleTerrain, 0x12 + 1);
                }
                else
                {
                    U.SelectedIndexSafety(this.BattleTerrain, 0x12 + 1);
                }
            }
            {
                List<U.AddrResult> list = ImageBattleBGForm.MakeList();
                list.Insert(0, new U.AddrResult(1, R._("00 マップを表示")));
                list.Insert(1, new U.AddrResult(0x100, R._("100 パレット0")));
                list.Insert(2, new U.AddrResult(0x101, R._("101 黒単色")));
                U.ConvertComboBox(list, ref this.BattleBG);

                U.SelectedIndexSafety(this.BattleBG, 0x4 + 3 - 1);
            }
            {
                List<U.AddrResult> list = new List<U.AddrResult>();
                list.Add(new U.AddrResult(1, R._("00 表示しない")));
                list.Add(new U.AddrResult(2, R._("01 戦闘画面")));
                U.ConvertComboBox(list, ref this.BattleScreen);

                U.SelectedIndexSafety(this.BattleScreen, 0x1);
            }
            {
                List<U.AddrResult> list = ImageBattleAnimeForm.MakeBattleList();
                list.Insert(0, new U.AddrResult(1, R._("00 表示しない")));
                U.ConvertComboBox(list, ref this.TargetEnemy);
                U.ConvertComboBox(list, ref this.PlayerUnit);

                if (Program.ROM.RomInfo.version() == 8)
                {
                    U.SelectedIndexSafety(this.TargetEnemy, 0x19);
                    U.SelectedIndexSafety(this.PlayerUnit, 0x6F);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    U.SelectedIndexSafety(this.TargetEnemy, 0x19);
                    U.SelectedIndexSafety(this.PlayerUnit, 0x6F);
                }
                else
                {
                    U.SelectedIndexSafety(this.TargetEnemy, 0x23);
                    U.SelectedIndexSafety(this.PlayerUnit, 0x12);
                }
                InputFormRef.LoadComboResource(TargetEnemySection, U.ConfigDataFilename("battleanime_mode_"));
                InputFormRef.LoadComboResource(PlayerUnitSection, U.ConfigDataFilename("battleanime_mode_"));
                U.SelectedIndexSafety(this.TargetEnemySection, 0);
                U.SelectedIndexSafety(this.PlayerUnitSection, 0);
            }
            {
                ToolTipEx tooltip = InputFormRef.GetToolTip<ToolAnimationCreatorForm>();
                SoundInfo.SetToolTipEx(tooltip);
                SkillSoundInfo.SetToolTipEx(tooltip);
            }

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, Sound, SoundInfo, 0, "SONG", new string[] { "SFX" });
            InputFormRef.makeLinkEventHandler("", controls, Sound, SoundPlaySoundButton, 0, "SONGPLAY", new string[] { });

            InputFormRef.makeLinkEventHandler("", controls, SkillSound, SkillSoundInfo, 0, "SONG", new string[] { "SFX" });
            InputFormRef.makeLinkEventHandler("", controls, SkillSound, SkillPlaySoundButton, 0, "SONGPLAY", new string[] { });

            //読みやすいようにコメントを入れます.
            Comment_85command_Dic = U.LoadDicResource(U.ConfigDataFilename("battleanime_85command_"));
            Comment_Mode_Dic = U.LoadDicResource(U.ConfigDataFilename("battleanime_mode_"));

            Comment_Magic85command_Dic = U.LoadDicResource(U.ConfigDataFilename("battleanime_85command_"));
            U.MapMarge(ref Comment_Magic85command_Dic, U.LoadDicResource(U.ConfigDataFilename("magic_command_")));

            this.AddressList.OwnerDraw(Draw, DrawMode.OwnerDrawVariable, false);

            this.OBJ.OwnerDraw(DrawOBJCombo, DrawMode.OwnerDrawVariable);
            this.BG.OwnerDraw(DrawBGCombo, DrawMode.OwnerDrawVariable);
            InputFormRef.markupJumpLabel(JumpToSoundTableCode);
            InputFormRef.markupJumpLabel(JumpToSoundTableSound);

            InputFormRef.MakeEditListboxContextMenu(this.AddressList, AddressList_KeyDown);

            ClearUndoBuffer();

            this.Code.ValueChanged += LightupButtonOnFloatingControlpanel;
            this.Sound.ValueChanged += LightupButtonOnFloatingControlpanel;
            this.SkillSound.ValueChanged += LightupButtonOnFloatingControlpanel;
            this.SkillType.SelectedIndexChanged += LightupButtonOnFloatingControlpanel;
            this.Wait.ValueChanged += LightupButtonOnFloatingControlpanel;
            this.OBJ.SelectedIndexChanged += LightupButtonOnFloatingControlpanel;
            this.BG.SelectedIndexChanged += LightupButtonOnFloatingControlpanel;

            SettingPlayButton(false);

            this.Disposed += ToolAnimationCreatorUserontrol_Disposed
                ;
        }

        //0x29 Set brightness and opacity levels for the background.
        //  Argument XX is the brightness level from 0 to 100% (0x0 through 0x10)
        //  Argument YY is the opacity level from 100% to 50% (0x0 through 0x10)
        //背景の明るさと不透明度を設定します。
        //  引数XXは0〜100％の輝度レベル（0x0〜0x10）です。 0x10=通常 0x00=常に黒
        //  引数YYは100％から50％の不透明度（0x0から0x10）です。 0x00=不透明 0x10=半透明(50%)
        System.Drawing.Imaging.ImageAttributes MakeMagicBGC29ImageAttributes(int brightness, int opacity)
        {
            //ColorMatrixオブジェクトの作成
            //指定された値をRBGの各成分にプラスする

            float plusVal = (float)((brightness - 16f) * (100f / 16f)) / 100f;
            float opacityLevel = ((32f - opacity)) * (100f / 32f) / 100f;
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix(
                    new float[][] {
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0}, 
                    new float[] {0, 0, 0, opacityLevel, 0},
                    new float[] {plusVal, plusVal, plusVal, 0, 1}
                });

            //ImageAttributesオブジェクトの作成
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrixを設定する
            ia.SetColorMatrix(cm);
            return ia;
        }

        //加算ブレンド
        //なぜかC#ではできないっぽいので、自前で作る.
        void AddBland(Bitmap dest, Bitmap src)
        {
            int height = Math.Min(dest.Height , src.Height);
            int width  = Math.Min(dest.Width,   src.Width);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color add = src.GetPixel(x, y);
                    if (add.R != 0 || add.G != 0 || add.B != 0)
                    {
                        Color rgb = dest.GetPixel(x, y);
                        byte r = (byte)Math.Min(255, rgb.R + add.R);
                        byte g = (byte)Math.Min(255, rgb.G + add.G);
                        byte b = (byte)Math.Min(255, rgb.B + add.B);
                        dest.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
        }

        bool IsMagicAnimationType()
        {
            return (this.AnimationType == AnimationTypeEnum.MagicAnime_CSACreator || this.AnimationType == AnimationTypeEnum.MagicAnime_FEEDitor);
        }
        bool IsSkillAnimationType()
        {
            return (this.AnimationType == AnimationTypeEnum.Skill);
        }

        private void ToolAnimationCreatorUserontrol_Disposed(object sender, EventArgs e)
        {
            foreach (var pair in AnimeObj)
            {
                pair.Value.Dispose();
            }
            foreach (var pair in AnimeBG)
            {
                pair.Value.Dispose();
            }
            this.DummyBitmap.Dispose();
        }
        private void Code_ValueChanged(object sender, EventArgs e)
        {
            uint command = (uint)this.Code.Value;
            uint c = command & 0xff;

            uint argsXX = (command >> 16) & 0xff;
            uint argsYY = (command >> 8) & 0xff;
            string argsText = "";
            if (argsXX > 0 || argsYY > 0)
            {
                argsText = R._("ARGS\r\nXX: {0}\r\nYY: {1}\r\n", U.To0xHexString(argsXX), U.To0xHexString(argsYY));
            }

            string soundText = SongTableForm.GetC85SoundEffect(c);
            if (soundText == "")
            {
                SoundPlayCodeButton.Hide();
            }
            else
            {
                argsText += R._("効果音") + "\r\n" + soundText;
                SoundPlayCodeButton.Show();
            }

            if (this.AnimationType == AnimationTypeEnum.BattleAnime)
            {
                this.CodeInfo.Text = R._("Command {0} //{1}\r\n\r\n{2}", U.To0xHexString(c), U.at(Comment_85command_Dic, c), argsText);
            }
            else
            {
                this.CodeInfo.Text = R._("Command {0} //{1}\r\n\r\n{2}", U.To0xHexString(c), U.at(Comment_Magic85command_Dic, c), argsText);
            }
        }

        private void LightupButtonOnFloatingControlpanel(object sender, EventArgs e)
        {
            //変更ボタンを光らせる
            InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
            InputFormRef.WriteButtonToYellow(this.NewButton, true);
        }


        //読みやすいようにコメントを入れます.
        Dictionary<uint, string> Comment_85command_Dic;
        Dictionary<uint, string> Comment_Magic85command_Dic;
        Dictionary<uint, string> Comment_Mode_Dic;

        public enum AnimationTypeEnum
        {
             BattleAnime
            ,MagicAnime_FEEDitor
            ,MagicAnime_CSACreator
            ,Skill
            ,TSAAnime
            ,ROMAnime
        }
        AnimationTypeEnum AnimationType;
        uint ID;
        Bitmap DummyBitmap;

        enum AnimeStEnum
        {
             Image
            ,Sound
            ,Code
            ,Loop
            ,Skill
            ,Term
        }
        class AnimeSt
        {
            public AnimeStEnum type;
            public uint code; //code or wait or sound.  unionがほしい
            public string obj;  //キャラクタ
            public string bg;   //背景
            public uint jisage;
            public uint mode;
        }
        List<AnimeSt> AnimeList;
        Dictionary<string,Bitmap> AnimeObj;
        Dictionary<string, Bitmap> AnimeBG;
    
        string GetCodeName(uint v, out Bitmap out_bitmap)
        {
            uint c = (v & 0xff);
            if (this.AnimationType == AnimationTypeEnum.BattleAnime)
            {
                if (c == 0x47 || c == 0x26 || c == 0x27)
                {//VERY hardcoded
                    out_bitmap = ImageSystemIconForm.MusicIcon(4);
                }
                else if (c == 0x1)
                {//HPが減りきるまで待機 ぐるぐる
                    out_bitmap = ImageSystemIconForm.MusicIcon(12);
                }
                else if (c == 0x2)
                {//回避モーション開始 道具袋
                    out_bitmap = ImageSystemIconForm.WeaponIcon(8);
                }
                else if (c == 0x5)
                {//魔法の呼び出し 理
                    out_bitmap = ImageSystemIconForm.WeaponIcon(5);
                }
                else if (c == 0x4)
                {//魔法の攻撃でのHPを減らす準備 理
                    out_bitmap = ImageSystemIconForm.WeaponIcon(5);
                }
                else if (c == 0x8 || c == 0x9 || c == 0xA || c == 0xB)
                {//クリティカルヒット 光魔法
                    out_bitmap = ImageSystemIconForm.WeaponIcon(6);
                }
                else if (c == 0x13 || c == 0x14 || c == 0x15)
                {//手斧や揺れるエフェクト 斧
                    out_bitmap = ImageSystemIconForm.WeaponIcon(2);
                }
                else if (c == 0x39 || c == 0x51)
                {//フラッシュ 色補正アイコン
                    out_bitmap = ImageSystemIconForm.MusicIcon(8);
                }
                else if (c == 0x2C || c == 0x2E || c == 0x30 || c == 0x31 || c == 0x32 || c == 0x4E)
                {//エフェクト系 とりあえず汎用asm
                    out_bitmap = ImageSystemIconForm.MusicIcon(3);
                }
                else if (c >= 0x19 && c <= 0x4D)
                {//効果音 ♪
                    out_bitmap = ImageSystemIconForm.MusicIcon(7);
                }
                else
                {//汎用asm
                    out_bitmap = ImageSystemIconForm.MusicIcon(3);
                }

                return U.at(this.Comment_85command_Dic, c);
            }
            else
            {
                if (c == 0x08)
                {//HP吸収 闇
                    out_bitmap = ImageSystemIconForm.WeaponIcon(7);
                }
                else if ((c >= 0x14 && c <= 0x28)
                    ||   (c >= 0x41 && c <= 0x47)
                    ||   (c >= 0x49 && c <= 0x52)
                )
                {//攻撃者へ渡すパラメータ
                    out_bitmap = ImageSystemIconForm.MusicIcon(3);
                }
                else if (c == 0x29 || c == 0x2A)
                {//明るさの調整 色補正アイコン
                    out_bitmap = ImageSystemIconForm.MusicIcon(8);
                }
                else if (c >= 0x2b && c <= 0x3f)
                {//防衛者へ渡すパラメータ
                    out_bitmap = ImageSystemIconForm.MusicIcon(3);
                }
                else if (c == 0x40)
                {//スクロール 色補正アイコン
                    out_bitmap = ImageSystemIconForm.MusicIcon(8);
                }
                else if (c == 0x48)
                {//効果音 ♪
                    out_bitmap = ImageSystemIconForm.MusicIcon(7);
                }
                else
                {//汎用asm
                    out_bitmap = ImageSystemIconForm.MusicIcon(3);
                }
                return U.at(this.Comment_Magic85command_Dic, c);
            }
        }

        private Size DrawOBJCombo(ComboBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            string filename = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;

            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxWidth;

            //ファイル名
            maxWidth = U.DrawText(filename, g, normalFont, brush, isWithDraw, bounds);

            //画像
            Bitmap bitmap = U.at(this.AnimeObj,filename,this.DummyBitmap);
            bounds.Y += lineHeight;

            Rectangle b = bounds;
            b.Width = (int)(bitmap.Width * 0.6);
            b.Height = (int)(bitmap.Height * 0.6);

            U.DrawPicture(bitmap, g, isWithDraw, b);
            //bitmap.Dispose();
            maxWidth = Math.Max(maxWidth, b.Width);

            brush.Dispose();

            bounds.X += maxWidth;
            bounds.Y += b.Height;
            return new Size(bounds.X, bounds.Y);
        }


        private Size DrawBGCombo(ComboBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            string filename = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;

            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxWidth;

            //ファイル名
            maxWidth = U.DrawText(filename, g, normalFont, brush, isWithDraw, bounds);

            //画像
            Bitmap bitmap = U.at(this.AnimeBG, filename, this.DummyBitmap);
            bounds.Y += lineHeight;

            Rectangle b = bounds;
            b.Width = (int)(bitmap.Width * 0.6);
            b.Height = (int)(bitmap.Height * 0.6);

            U.DrawPicture(bitmap, g, isWithDraw, b);
            //bitmap.Dispose();
            maxWidth = Math.Max(maxWidth, b.Width);

            brush.Dispose();

            bounds.X += maxWidth;
            bounds.Y += b.Height;
            return new Size(bounds.X, bounds.Y);
        }

        public void JisageReorder()
        {
            if (this.AnimationType == AnimationTypeEnum.BattleAnime)
            {
                //戦闘アニメーションの自下げ(戦闘アニメーションにしか字下げはない)
                uint jisageCount = 0;
                //モード
                uint mode = 1;

                for (int i = 0; i < this.AnimeList.Count; i++)
                {
                    AnimeSt code = this.AnimeList[i];
                    code.jisage = jisageCount;
                    code.mode = mode;

                    if (code.type == AnimeStEnum.Loop)
                    {
                        jisageCount++;
                    }
                    else if (code.type == AnimeStEnum.Code && code.code == 0x01)
                    {
                        jisageCount = 0;
                        code.jisage = 0; //ゼロで閉じる.
                    }
                    else if (code.type == AnimeStEnum.Term)
                    {
                        jisageCount = 0;
                        code.jisage = 0; //ゼロで閉じる.

                        if (mode == 0x01)
                        {
                            mode = 0x03;
                        }
                        else if (mode == 0x03)
                        {
                            mode = 0x05;
                        }
                        else
                        {
                            mode++;
                        }
                    }
                }
            }
            else if (IsMagicAnimationType())
            {
                //モード
                uint mode = 1;

                for (int i = 0; i < this.AnimeList.Count; i++)
                {
                    AnimeSt code = this.AnimeList[i];
                    code.mode = mode;

                    if (code.type == AnimeStEnum.Term)
                    {
                        mode++;
                    }
                }
            }
        }

        private Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.AnimeList.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            AnimeSt code = this.AnimeList[index];

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            SolidBrush foreKeywordBrush = new SolidBrush(OptionForm.Color_Keyword_ForeColor());
            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxWidth = 0;
            int maxHeight = (int)lb.Font.Height;

            if (this.AnimationType == AnimationTypeEnum.BattleAnime)
            {
                if (index <= 0 || code.mode != this.AnimeList[index - 1].mode)
                {//モードがかわったときはコメントを入れる.
                    text = string.Format("/// - Mode: {0}  {1}", code.mode, U.at(Comment_Mode_Dic, code.mode));
                    maxWidth = U.DrawText(text, g, boldFont, foreKeywordBrush, isWithDraw, bounds);
                    bounds.Y += lineHeight;
                }
            }

            bounds.X += (int)(lb.Font.Height * 2 * code.jisage);
            if (code.type == AnimeStEnum.Code)
            {
                uint v = code.code;

                text = R._("コード");
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                bounds.X += U.DrawText(":", g, normalFont, brush, isWithDraw, bounds);

                text = U.ToHexString(v);
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);

                Bitmap bitmap;
                text = GetCodeName(v, out bitmap);
                U.MakeTransparent(bitmap);
                bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);
                bounds.X += 4;

                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;

                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();
                maxHeight = Math.Max(maxHeight, b.Height);
            }
            else if (code.type == AnimeStEnum.Loop)
            {
                uint v = code.code;

                text = R._("ループ開始");
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);

                text = U.ToHexString(v);
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                bounds.X += 4;

                Bitmap bitmap = ImageSystemIconForm.MusicIcon(4);
                U.MakeTransparent(bitmap);

                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;

                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();
                maxHeight = Math.Max(maxHeight, b.Height);
            }
            else if (code.type == AnimeStEnum.Term)
            {
                uint v = code.code;

                text = string.Format("~~~");
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);

                if (IsMagicAnimationType())
                {
                    if (this.AnimeList.Count != index)
                    {
                        text = "///" + R._("ミスした場合の終端");
                        bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                    }
                }
                
                bounds.X += 4;

                Bitmap bitmap = ImageSystemIconForm.MusicIcon(10);
                U.MakeTransparent(bitmap);

                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;

                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();
                maxHeight = Math.Max(maxHeight, b.Height);
            }
            else if (code.type == AnimeStEnum.Skill)
            {
                uint v = code.code;

                if (code.bg == "D")
                {
                    text = R._("防衛スキル");
                }
                else
                {
                    text = R._("スキル");
                }
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                bounds.X += U.DrawText(":", g, normalFont, brush, isWithDraw, bounds);

                text = U.ToHexString(v);
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);

                text = SongTableForm.GetSongName(v);
                bounds.X += U.DrawText(" " + text, g, normalFont, brush, isWithDraw, bounds);
                bounds.X += 4;

                Bitmap bitmap = ImageSystemIconForm.MusicIcon(16);
                U.MakeTransparent(bitmap);

                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;

                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();
                maxHeight = Math.Max(maxHeight, b.Height);
            }
            else if (code.type == AnimeStEnum.Sound)
            {
                uint v = code.code;

                text = R._("効果音");
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                bounds.X += U.DrawText(":", g, normalFont, brush, isWithDraw, bounds);

                text = U.ToHexString(v);
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);

                text = SongTableForm.GetSongName(v);
                bounds.X += U.DrawText(" " + text, g, normalFont, brush, isWithDraw, bounds);
                bounds.X += 4;

                Bitmap bitmap = ImageSystemIconForm.MusicIcon(7);
                U.MakeTransparent(bitmap);

                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;

                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();
                maxHeight = Math.Max(maxHeight, b.Height);

                string errorMessage = SongTableForm.GetErrorMessage(v , "SFX");
                if (errorMessage != "")
                {
                    U.DrawErrorRectangle(g, isWithDraw, listbounds);
                }
            }
            else if (code.type == AnimeStEnum.Image)
            {
                int x = bounds.X;

                text = code.code.ToString();
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                text = string.Format(" p- ");
                U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
                bounds.X = x + 40; //固定位置に出したい.

                if (!U.IsEmpty(code.obj))
                {
                    bounds.X += 2;
                    Bitmap bitmap = U.at(this.AnimeObj, code.obj, this.DummyBitmap);
                    Rectangle b = bounds;
                    b.Width = (int)(bitmap.Width * 0.7);
                    b.Height = (int)(bitmap.Height * 0.7);
                    bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                    //bitmap.Dispose();
                    maxHeight = Math.Max(maxHeight, b.Height);
                }
                if (!U.IsEmpty(code.bg))
                {
                    bounds.X += 2;
                    Bitmap bitmap = U.at(this.AnimeBG, code.bg, this.DummyBitmap);
                    Rectangle b = bounds;
                    b.Width = (int)(bitmap.Width * 0.7);
                    b.Height = (int)(bitmap.Height * 0.7);
                    bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                    //bitmap.Dispose();
                    maxHeight = Math.Max(maxHeight, b.Height);
                }
            }

            maxWidth = Math.Max(maxWidth, bounds.X - listbounds.X);

            brush.Dispose();
            boldFont.Dispose();

            return new Size(listbounds.X + maxWidth, bounds.Y + maxHeight);
        }

        public void Init(
            AnimationTypeEnum type
            , uint id
            , string filehint
            , string filename
            )
        {
            this.AnimationType = type;

            if (type == AnimationTypeEnum.BattleAnime)
            {
                this.PlayerUnit.Hide();
                this.PlayerUnitFream.Hide();
                this.PlayerUnitFreamLabel.Hide();
                this.PlayerUnitLabel.Hide();
                this.PlayerUnitSection.Hide();
                this.PlayerUnitSectonLabel.Hide();

                this.C29Label.Hide();
                this.C29Brightness.Hide();
                this.C29BrightnessLabel.Hide();
                this.C29Opacity.Hide();
                this.C29OpacityLabel.Hide();
                this.C53Label.Hide();
                this.C53Expand.Hide();
                this.C53ExpandLabel.Hide();
           
                this.BG.Hide();
                this.BGLabel.Hide();
                this.BGImportButton.Hide();
                this.BGExportButton.Hide();

                this.OBJ.BeginUpdate();
                this.OBJ.Items.Clear();
                InitBattleAnimeParse(filename);
                this.OBJ.ItemHeight = (int)(this.DummyBitmap.Height * 0.6);
                this.OBJ.Width = this.DummyBitmap.Width * 2;
                this.OBJ.EndUpdate();

                //スキルページがあったら消去する.
                int skillpage = this.MainTab.TabPages.IndexOf(this.SkillTabPage);
                if (skillpage >= 0)
                {
                    this.MainTab.TabPages.RemoveAt(skillpage);
                }

                TermExplain.Text = R._("戦闘アニメーションには、Mode1,Mode3,5,6,7,8,9,10,11,12 のモードが必要になります。");
            }
            else if (IsMagicAnimationType())
            {
                this.BG.BeginUpdate();
                this.BG.Items.Clear();

                this.OBJ.BeginUpdate();
                this.OBJ.Items.Clear();
                InitMagicAnimeParse(filename);
                this.OBJ.ItemHeight = (int)(this.DummyBitmap.Height * 0.6);
                this.OBJ.EndUpdate();

                this.BG.ItemHeight = (int)(this.DummyBitmap.Height * 0.6);
                this.BG.EndUpdate();

                //範囲攻撃、敵視点にする
                U.SelectedIndexSafety(this.BattleFocus, 2);
                //魔法使いが魔法を放つフレームにする.
                U.SelectedIndexSafety(this.PlayerUnitSection, 2);
                this.PlayerUnitFream.Value = 0x9;

                //スキルページがあったら消去する.
                int skillpage = this.MainTab.TabPages.IndexOf(this.SkillTabPage);
                if (skillpage >= 0)
                {
                    this.MainTab.TabPages.RemoveAt(skillpage);
                }
                //ループページを消す
                int looppage = this.MainTab.TabPages.IndexOf(this.LoopTabPage);
                if (looppage >= 0)
                {
                    this.MainTab.TabPages.RemoveAt(looppage);
                }
                TermExplain.Text = R._("魔法アニメーションには、ミス終端として利用されます。\r\n攻撃がミスの場合は、ミス終端で終了します。\r\nミス終端以降に、命中したアニメーションを定義できます。");

                if (this.AnimationType == AnimationTypeEnum.MagicAnime_FEEDitor)
                {
                    this.C53Label.Hide();
                    this.C53Expand.Hide();
                    this.C53ExpandLabel.Hide();
                }
                C29Brightness.Value = 0x10;
                C29Opacity.Value = 0x10;
                C53Expand.Value = CalcFEditorMagicInitCode();
            }
            else if (type == AnimationTypeEnum.Skill)
            {
                this.C29Label.Hide();
                this.C29Brightness.Hide();
                this.C29BrightnessLabel.Hide();
                this.C29Opacity.Hide();
                this.C29OpacityLabel.Hide();
                this.C53Label.Hide();
                this.C53Expand.Hide();
                this.C53ExpandLabel.Hide();

                this.BG.Hide();
                this.BGLabel.Hide();
                this.BGImportButton.Hide();
                this.BGExportButton.Hide();

                this.OBJ.BeginUpdate();
                this.OBJ.Items.Clear();
                InitSkillAnimeParse(filename);
                this.OBJ.ItemHeight = (int)(this.DummyBitmap.Height * 0.6);
                this.OBJ.Width = this.DummyBitmap.Width * 2;
                this.OBJ.EndUpdate();

                //Code,Sound,Loop,Termページを消すする.
                int commandpage = this.MainTab.TabPages.IndexOf(this.CodeTabPage);
                if (commandpage >= 0)
                {
                    this.MainTab.TabPages.RemoveAt(commandpage);
                }
                int soundpage = this.MainTab.TabPages.IndexOf(this.SoundTabPage);
                if (soundpage >= 0)
                {
                    this.MainTab.TabPages.RemoveAt(soundpage);
                }
                int looppage = this.MainTab.TabPages.IndexOf(this.LoopTabPage);
                if (looppage >= 0)
                {
                    this.MainTab.TabPages.RemoveAt(looppage);
                }
                int termpage = this.MainTab.TabPages.IndexOf(this.TermPage);
                if (termpage >= 0)
                {
                    this.MainTab.TabPages.RemoveAt(termpage);
                }
            }

            //字下げとモードの割り当て
            JisageReorder();

            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, 0);

            this.ID = id;
        }

        void InitSkillAnimeParse(string filename)
        {
            string basedir = Path.GetDirectoryName(filename);

            string skillType = "";
            uint   skillSound = 0;

            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                int lineCount = i + 1;
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                if (line == "")
                {
                    continue;
                }

                if (line == "D")
                {//ループ
                    skillType = "D";
                    continue;
                }

                if (line[0] == 'S')
                {//音楽再生
                    uint music = U.atoh(line.Substring(1));
                    skillSound = music;
                    continue;
                }

                if (U.isnum(line[0]))
                {//86コマンド 画像
                    uint frameSec = U.atoi(line); //フレーム秒

                    //filename
                    string imagefilename = ImageUtilOAM.parsePFilename(line);
                    if (imagefilename.Length <= 0)
                    {
                        continue;
                    }

                    if (!this.AnimeObj.ContainsKey(imagefilename))
                    {
                        string imagefullfilename = Path.Combine(basedir, imagefilename);
                        Bitmap bitmap = ImageUtil.OpenLowBitmap(imagefullfilename);
                        if (bitmap == null)
                        {
                            continue;
                        }

                        this.AnimeObj[imagefilename] = bitmap;
                        this.OBJ.Items.Add(imagefilename);
                    }

                    AppendImage(frameSec, imagefilename);
                    continue;
                }
            }

            //スキルの音楽と種類が判明している場合、先頭に追加する
            if (skillSound > 0)
            {
                InsertSkill(skillSound, skillType);
            }
        }
        bool ExportSkillAnimeWrite(string filename)
        {
            string basedir = Path.GetDirectoryName(filename);

            List<string> lines = new List<string>();
            for (int i = 0; i < AnimeList.Count; i++)
            {
                string line;
                AnimeSt code = AnimeList[i];

                if (code.type == AnimeStEnum.Skill)
                {
                    if (code.bg == "D")
                    {
                        line = "D";
                        lines.Add(line);
                    }
                    line = "S" + U.ToHexString(code.code);
                    lines.Add(line);
                    break;
                }
            }

            for (int i = 0; i < AnimeList.Count; i++)
            {
                string line;
                AnimeSt code = AnimeList[i];

                if (code.type == AnimeStEnum.Image)
                {
                    line = code.code + " " + code.obj;
                    lines.Add(line);

                    Debug.Assert(this.AnimeObj.ContainsKey(code.obj));
                    string imagefullfilename = Path.Combine(basedir, code.obj);

                    if (!U.BitmapSave(this.AnimeObj[code.obj], imagefullfilename))
                    {
                        return false;
                    }
                }
            }
            return U.WriteAllLinesInError(filename, lines);
        }


        void InitBattleAnimeParse(string filename)
        {
            string basedir = Path.GetDirectoryName(filename);

            //現在のモード(modeは1から数えるので、mode+1 が実際のモードです)
            int mode = 0;
            //mode1はmode2 を、 mode3はmode4 を共に生成しないという特殊ルールがある.
            bool isMode1 = true; //mode1(つまり mode=0)なのでいきなり特殊処理

            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                int lineCount = i + 1;
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                if (line == "")
                {
                    continue;
                }

                if (line[0] == '~')
                {//モード終端
                    AppendTerm();

                    mode++;
                    if (mode >= 0xC)
                    {//シート 0xCまでいったら終了
                        break;
                    }

                    if (isMode1)
                    {
                        mode++;
                    }

                    if (mode == 0x3 - 1)
                    {//mode3 は mode4を共に生成する. mode0とmode1の処理はすでに完了しているのでmode3だけチェック
                        isMode1 = true;
                    }
                    else
                    {
                        isMode1 = false;
                    }
                    continue;
                }

                if (line[0] == 'C')
                {//85コマンド
                    uint command = U.atoh(line.Substring(1));
                    uint c = command & 0xff;
                    if (c == 0x48 && command > 0x100)
                    {//Command48拡張による音楽再生.
                        uint music = command >> 8;
                        AppendSound(music);
                        continue;
                    }
                    AppendCode(command);
                    continue;
                }

                if (line[0] == 'L')
                {//ループ
                    AppendLoop();
                    continue;
                }

                if (line[0] == 'S')
                {//音楽再生
                    uint music = U.atoh(line.Substring(1));
                    AppendSound(music);
                    continue;
                }

                if (U.isnum(line[0]) && line.IndexOf("p-") > 0)
                {//86コマンド 画像
                    uint frameSec = U.atoi(line); //フレーム秒

                    //p- filename
                    string imagefilename = ImageUtilOAM.parsePFilename(line);
                    if (imagefilename.Length <= 0)
                    {
                        continue;
                    }

                    if (!this.AnimeObj.ContainsKey(imagefilename))
                    {
                        string imagefullfilename = Path.Combine(basedir, imagefilename);
                        Bitmap bitmap = ImageUtil.OpenLowBitmap(imagefullfilename);
                        if (bitmap == null)
                        {
                            continue;
                        }

                        this.AnimeObj[imagefilename] = bitmap;
                        this.OBJ.Items.Add(imagefilename);
                    }

                    AppendImage(frameSec,imagefilename);
                    continue;
                }
            }
        }
        bool ExportBattleAnimeWrite(string filename)
        {
            string basedir = Path.GetDirectoryName(filename);

            List<string> lines = new List<string>();
            for (int i = 0; i < AnimeList.Count; i++)
            {
                string line;
                AnimeSt code = AnimeList[i];

                if (code.type == AnimeStEnum.Code)
                {
                    line = "C" + U.ToHexString(code.code);
                    lines.Add(line);
                }
                else if (code.type == AnimeStEnum.Image)
                {
                    line = code.code + " p- " + code.obj;
                    lines.Add(line);

                    Debug.Assert(this.AnimeObj.ContainsKey(code.obj));
                    string imagefullfilename = Path.Combine(basedir, code.obj);

                    if (!U.BitmapSave(this.AnimeObj[code.obj], imagefullfilename))
                    {
                        return false;
                    }
                }
                else if (code.type == AnimeStEnum.Loop)
                {
                    line = "L";
                    lines.Add(line);
                }
                else if (code.type == AnimeStEnum.Sound)
                {
                    line = "S" + U.ToHexString(code.code);
                    lines.Add(line);
                }
                else if (code.type == AnimeStEnum.Term)
                {
                    line = "~~~";
                    lines.Add(line);
                }
            }

            return U.WriteAllLinesInError(filename, lines);
        }



        void InitMagicAnimeParse(string filename)
        {
            string basedir = Path.GetDirectoryName(filename);

            int mode = 0;
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                int lineCount = i + 1;
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                if (line == "")
                {
                    continue;
                }

                if (line[0] == '~')
                {//モード終端
                    AppendTerm();

                    mode++;
                    if (mode >= 2)
                    {//ミス終端と本当の終端の最大2つ
                        break;
                    }
                    continue;
                }

                if (line[0] == 'C')
                {//85コマンド
                    uint command = U.atoh(line.Substring(1));
                    uint c = command & 0xff;
                    if (c == 0x48 && command > 0x100)
                    {//Command48拡張による音楽再生.
                        uint music = command >> 8;
                        AppendSound(music);
                        continue;
                    }
                    AppendCode(command);
                    continue;
                }

                if (line[0] == 'S')
                {//音楽再生
                    uint music = U.atoh(line.Substring(1));
                    AppendSound(music);
                    continue;
                }

                if (line[0] != 'O' && line[0] != 'B')
                {
                    //不明な命令なので無視する.
                    continue;
                }

                //O p- objblank.png
                //B p- bg3.png
                //1 
                string objAnimeFilename = "";
                string bgAnimeFilename = "";
                uint frameSec = U.NOT_FOUND;
                for (int n = 1; n <= 3; )
                {
                    if (i >= lines.Length)
                    {
                        break;
                    }
                    line = lines[i];
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        i++;
                        continue;
                    }
                    line = U.ClipComment(line);
                    if (line == "")
                    {
                        i++;
                        continue;
                    }

                    if (U.isnum(line[0]))
                    {
                        if (frameSec != U.NOT_FOUND)
                        {
                            break;
                        }
                        frameSec = U.atoi(line);

                        i++;
                        n++;
                        continue;
                    }

                    string imagefilename = ImageUtilOAM.parsePFilename(line);
                    if (imagefilename.Length <= 0)
                    {
                        break;
                    }

                    if (line[0] == 'O')
                    {
                        if (objAnimeFilename != "")
                        {
                            break;
                        }
                        objAnimeFilename = imagefilename;

                        i++;
                        n++;
                        continue;
                    }
                    if (line[0] == 'B')
                    {
                        if (bgAnimeFilename != "")
                        {
                            break;
                        }
                        bgAnimeFilename = imagefilename;

                        i++;
                        n++;
                        continue;
                    }
                }
                if (objAnimeFilename == "")
                {
                    continue;
                }
                if (bgAnimeFilename == "")
                {
                    continue;
                }
                if (frameSec == U.NOT_FOUND)
                {
                    continue;
                }

                if (!this.AnimeObj.ContainsKey(objAnimeFilename))
                {
                    string imagefullfilename = Path.Combine(basedir, objAnimeFilename);
                    Bitmap bitmap = ImageUtil.OpenLowBitmap(imagefullfilename);
                    if (bitmap == null)
                    {
                        continue;
                    }

                    this.AnimeObj[objAnimeFilename] = bitmap;
                    this.OBJ.Items.Add(objAnimeFilename);
                }
                if (!this.AnimeBG.ContainsKey(bgAnimeFilename))
                {
                    string imagefullfilename = Path.Combine(basedir, bgAnimeFilename);
                    Bitmap bitmap = ImageUtil.OpenLowBitmap(imagefullfilename);
                    if (bitmap == null)
                    {
                        continue;
                    }

                    this.AnimeBG[bgAnimeFilename] = bitmap;
                    this.BG.Items.Add(bgAnimeFilename);
                }

                AppendImage(frameSec, objAnimeFilename, bgAnimeFilename);
                i--;
            }
        }
        bool ExportMagicAnimeWrite(string filename)
        {
            string basedir = Path.GetDirectoryName(filename);

            List<string> lines = new List<string>();
            for (int i = 0; i < AnimeList.Count; i++)
            {
                string line;
                AnimeSt code = AnimeList[i];

                if (code.type == AnimeStEnum.Code)
                {
                    line = "C" + U.ToHexString(code.code);
                    lines.Add(line);
                }
                else if (code.type == AnimeStEnum.Image)
                {
                    line = "O " + code.obj;
                    lines.Add(line);

                    line = "B " + code.bg;
                    lines.Add(line);

                    line = code.code.ToString(); //wait
                    lines.Add(line);

                    Debug.Assert(this.AnimeObj.ContainsKey(code.obj));
                    string imagefullfilename = Path.Combine(basedir, code.obj);
                    if (!U.BitmapSave(this.AnimeObj[code.obj], imagefullfilename))
                    {
                        return false;
                    }

                    Debug.Assert(this.AnimeBG.ContainsKey(code.bg));
                    imagefullfilename = Path.Combine(basedir, code.bg);
                    if (!U.BitmapSave(this.AnimeBG[code.bg], imagefullfilename))
                    {
                        return false;
                    }
                }
                else if (code.type == AnimeStEnum.Sound)
                {
                    line = "S" + U.ToHexString(code.code);
                    lines.Add(line);
                }
                else if (code.type == AnimeStEnum.Term)
                {
                    line = "~~~";
                    lines.Add(line);
                }
            }
            return U.WriteAllLinesInError(filename, lines);
        }
        
        void AppendCode(uint code)
        {
            AnimeSt a = new AnimeSt();
            a.type = AnimeStEnum.Code;
            a.code = code;
            AnimeList.Add(a);
        }
        void AppendImage(uint wait,string obj)
        {
            AnimeSt a = new AnimeSt();
            a.type = AnimeStEnum.Image;
            a.code = wait;
            a.obj = obj;
            AnimeList.Add(a);
        }
        void AppendImage(uint wait, string obj, string bg)
        {
            AnimeSt a = new AnimeSt();
            a.type = AnimeStEnum.Image;
            a.code = wait;
            a.obj = obj;
            a.bg = bg;
            AnimeList.Add(a);
        }
        void AppendSound(uint sound)
        {
            AnimeSt a = new AnimeSt();
            a.type = AnimeStEnum.Sound;
            a.code = sound;
            AnimeList.Add(a);
        }
        void AppendLoop()
        {
            AnimeSt a = new AnimeSt();
            a.type = AnimeStEnum.Loop;
            AnimeList.Add(a);
        }
        void InsertSkill(uint sound, string skilltype)
        {
            AnimeSt a = new AnimeSt();
            a.type = AnimeStEnum.Skill;
            a.code = sound;
            a.bg = skilltype;
            AnimeList.Insert(0,a);
        }
        void AppendTerm()
        {
            AnimeSt a = new AnimeSt();
            a.type = AnimeStEnum.Term;
            AnimeList.Add(a);
        }


        void DrawSampleScreenUnitLow(AnimeSt code
            , out Bitmap out_bg_bitmap
            , out Bitmap out_obj_bitmap
            , out Bitmap out_obj2_bitmap)
        {
            Debug.Assert(code.type == AnimeStEnum.Image);

            if (U.IsEmpty(code.bg))
            {
                out_bg_bitmap = ImageUtil.CloneBitmap(this.DummyBitmap);
            }
            else
            {//magic
                Bitmap bitmap = U.at(this.AnimeBG, code.bg, this.DummyBitmap);

                if (this.AnimationType == AnimationTypeEnum.MagicAnime_FEEDitor)
                {//ハーフサイズのBG 引き延ばして表示する.
                    out_bg_bitmap = ImageUtil.Copy(bitmap, 0, 0, 240, 64);
                }
                else //if (this.AnimationType == AnimationTypeEnum.MagicAnime_CSACreator)
                {
                    if (C53Expand.Value == 1)
                    {//拡大して表示
                        out_bg_bitmap = ImageUtil.Copy(bitmap, 0, 0, 240, 64);
                    }
                    else
                    {
                        if (bitmap.Height == 64)
                        {//ハーフサイズのBG 引き延ばして表示する.
                            out_bg_bitmap = ImageUtil.Copy(bitmap, 0, 0, 240, 64);
                        }
                        else
                        {//CSACreator専用のサイズ 240x160
                            out_bg_bitmap = ImageUtil.Copy(bitmap, 0, 0, 240, 160);
                        }
                    }
                }

                //bitmap.Dispose();
            }

            if (U.IsEmpty(code.obj))
            {
                out_obj_bitmap = ImageUtil.CloneBitmap(this.DummyBitmap);
                out_obj2_bitmap = ImageUtil.CloneBitmap(this.DummyBitmap);
            }
            else
            {
                Bitmap bitmap = U.at(this.AnimeObj, code.obj, this.DummyBitmap);
                out_obj_bitmap = ImageUtil.Copy(bitmap, 0, 0, ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8);
                if (bitmap.Width > ImageUtilOAM.SCREEN_TILE_WIDTH * 8)
                {//めりこむ剣先
                    out_obj2_bitmap = ImageUtil.Copy(bitmap, (ImageUtilOAM.SCREEN_TILE_WIDTH - 1) * 8, 0, ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8);
                }
                else
                {
                    out_obj2_bitmap = ImageUtil.CloneBitmap(this.DummyBitmap);
                }
                //bitmap.Dispose();
            }

        }
        void DrawSampleScreenUnit(int index
            , out Bitmap out_bg_bitmap
            , out Bitmap out_obj_bitmap
            , out Bitmap out_obj2_bitmap)
        {
            //前方に検索
            for (int i = index; i >= 0; i--)
            {
                AnimeSt code = this.AnimeList[i];
                if (code.type == AnimeStEnum.Image)
                {
                    DrawSampleScreenUnitLow(code
                        , out out_bg_bitmap
                        , out out_obj_bitmap
                        , out out_obj2_bitmap);
                    return;
                }
            }

            //後方に検索
            for (int i = index+1; i < this.AnimeList.Count; i++)
            {
                AnimeSt code = this.AnimeList[i];
                if (code.type == AnimeStEnum.Image)
                {
                    DrawSampleScreenUnitLow(code
                        , out out_bg_bitmap
                        , out out_obj_bitmap
                        , out out_obj2_bitmap);
                    return;
                }
            }

            out_bg_bitmap = ImageUtil.CloneBitmap(this.DummyBitmap);
            out_obj_bitmap = ImageUtil.CloneBitmap(this.DummyBitmap);
            out_obj2_bitmap = ImageUtil.CloneBitmap(this.DummyBitmap);
        }

        //背景の描画
        void GraphicsDrawBG(Graphics g,Bitmap obj_bitmap)
        {
            uint id = (uint)this.BattleBG.SelectedIndex;
            if (id >= 3)
            {
                id = id - (3-1);
                Bitmap battlebg = ImageBattleBGForm.DrawBG(id);
                U.MakeTransparent(battlebg);
                g.DrawImage(battlebg, 0, 0);
                battlebg.Dispose();
                return;
            }
            else if (id == 0)
            {//マップを暗くして表示
                System.Drawing.Imaging.ImageAttributes ia =
                    this.MakeMagicBGC29ImageAttributes((int)0x10-0x02
                    , 0x00);

                Bitmap mapbitmap = MapSettingForm.DrawMap(0);
                g.DrawImage(mapbitmap
                    , new Rectangle(0, 0, ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8)
                    , 0, 0, 30 * 8, 20 * 8
                    , GraphicsUnit.Pixel, ia);
                mapbitmap.Dispose();
                return;
            }
            else if (id == 1)
            {//パレット0
                //背景を標準の色で塗りつぶす. 黒だとみずらいので
                if (obj_bitmap.Palette != null)
                {
                    Brush brush = new SolidBrush(obj_bitmap.Palette.Entries[0]);
                    g.FillRectangle(brush, 0, 0, ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8);
                    brush.Dispose();
                }
                return;
            }
            else if (id == 2)
            {//黒単色
                Brush brush = new SolidBrush(Color.Black);
                g.FillRectangle(brush, 0, 0, ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8);
                brush.Dispose();
            }
        }

        Bitmap DrawSampleScreen(int index)
        {
            //画面合成は面倒なのでフルカラーでやる.
            Bitmap retScreen = new Bitmap(ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8);

            using (Graphics g = Graphics.FromImage(retScreen))
            {
                uint id;

                //距離によってユニットを描画する位置を変更しないといけない
                ImageBattleTerrainForm.RangeType rangeType = (ImageBattleTerrainForm.RangeType)this.BattleFocus.SelectedIndex;
                int addRangeTypeEnemyX;
                int addRangeTypePlayerX;
                if (rangeType == ImageBattleTerrainForm.RangeType.Range_Player)
                {
                    addRangeTypeEnemyX = -7 * 8;
                    addRangeTypePlayerX = 3 * 8;
                }
                else if (rangeType == ImageBattleTerrainForm.RangeType.Range_Enemy)
                {
                    addRangeTypeEnemyX =  -4 * 8;
                    addRangeTypePlayerX =  6 * 8;
                }
                else
                {
                    addRangeTypeEnemyX = 0;
                    addRangeTypePlayerX = 0;
                }

                //ユニットの描画準備 準備するだけで描画はまた後で
                Bitmap bg_bitmap;
                Bitmap obj_bitmap;
                Bitmap obj2_bitmap;
                DrawSampleScreenUnit(index, out bg_bitmap, out obj_bitmap, out obj2_bitmap);

                //戦闘背景
                GraphicsDrawBG(g,obj_bitmap);

                //地形
                id = (uint)this.BattleTerrain.SelectedIndex;
                if (id >= 1)
                {
                    Bitmap battleTerrain = ImageBattleTerrainForm.DrawSquare(id,rangeType);
                    U.MakeTransparent(battleTerrain);
                    g.DrawImage(battleTerrain, 0, 0);
                    battleTerrain.Dispose();
                }

                //標的の後ろに描画する剣先
                if (IsMagicAnimationType())
                {//魔法アニメ
                    U.MakeTransparent(obj2_bitmap);
                    g.DrawImage(obj2_bitmap, 0, 0);
                }
                else
                {//戦闘アニメ
                    U.MakeTransparent(obj2_bitmap);
                    g.DrawImage(obj2_bitmap, addRangeTypePlayerX, 0);
                }

                //標的
                id = (uint)this.TargetEnemy.SelectedIndex;
                if (id >= 1)
                {
                    uint showSectionData = U.atoh(TargetEnemySection.Text) - 1;
                    uint showFrameData = (uint)TargetEnemyFream.Value;
                    Bitmap target = ImageBattleAnimeForm.DrawBattleAnime(id
                        , ImageBattleAnimeForm.ScaleTrim.NO_BUT_FLIP
                        , 0
                        , showSectionData
                        , showFrameData
                        , 1);
                    U.MakeTransparent(target);
                    g.DrawImage(target, addRangeTypeEnemyX, 0);
                    target.Dispose();
                }

                if (this.AnimationType != AnimationTypeEnum.BattleAnime)
                {//戦闘アニメでないならプレイヤユニットを描画する
                    id = (uint)this.PlayerUnit.SelectedIndex;
                    if (id >= 1)
                    {
                        uint showSectionData = U.atoh(PlayerUnitSection.Text) - 1;
                        uint showFrameData = (uint)PlayerUnitFream.Value;
                        Bitmap player = ImageBattleAnimeForm.DrawBattleAnime(id
                            , ImageBattleAnimeForm.ScaleTrim.NO
                            , 0
                            , showSectionData
                            , showFrameData
                            , 0);
                        U.MakeTransparent(player);
                        g.DrawImage(player, addRangeTypePlayerX, 0);
                        player.Dispose();
                    }
                }

                if (IsMagicAnimationType())
                {//魔法アニメ

                    U.MakeTransparent(bg_bitmap);
                    System.Drawing.Imaging.ImageAttributes ia =
                        this.MakeMagicBGC29ImageAttributes((int)this.C29Brightness.Value
                        , (int)this.C29Opacity.Value);

                    //魔法の背景
                    if (bg_bitmap.Height >= 160 &&
                        this.AnimationType == AnimationTypeEnum.MagicAnime_CSACreator)
                    {
                        g.DrawImage(bg_bitmap
                            , new Rectangle(0, 0, 30 * 8, 160)
                            , 0, 0, bg_bitmap.Width, bg_bitmap.Height
                            , GraphicsUnit.Pixel, ia);
                    }
                    else
                    {
                        g.DrawImage(bg_bitmap
                            , new Rectangle(0, 0, 30 * 8, 64 * 2)
                            , 0, 0, bg_bitmap.Width, bg_bitmap.Height
                            , GraphicsUnit.Pixel, ia);
                    }


                    //魔法のオブジェクト
                    U.MakeTransparent(obj_bitmap);
                    g.DrawImage(obj_bitmap, 0, 0);
                }
                else if (IsSkillAnimationType())
                {//スキルアニメ 加算ブレンド
                    AddBland(retScreen, obj_bitmap);
                }
                else
                {
                    //ユニットのメインオブジェクト
                    U.MakeTransparent(obj_bitmap);
                    g.DrawImage(obj_bitmap, addRangeTypePlayerX, 0);
                }

                //戦闘画面のフレーム
                id = (uint)this.BattleScreen.SelectedIndex;
                if (id == 1)
                {
                    Bitmap battlescreen = ImageBattleScreenForm.DrawBattleSreenBitmap30x20();
                    U.MakeTransparent(battlescreen);
                    g.DrawImage(battlescreen, 0, 0);
                    battlescreen.Dispose();
                }

                bg_bitmap.Dispose();
                obj_bitmap.Dispose();
                obj2_bitmap.Dispose();
            }
            return retScreen;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();

            int index = this.AddressList.SelectedIndex;
            X_ANIME_PIC.Image = DrawSampleScreen(index);

            ModeHint.Text = DrawModeHint(index);
        }

        string DrawModeHint(int index )
        {
            if (index < 0 || index >= this.AnimeList.Count)
            {//一件もない
                return "";
            }
            AnimeSt code = this.AnimeList[index];
            if (this.AnimationType == AnimationTypeEnum.BattleAnime)
            {
                return string.Format("Mode: {0}  {1}", code.mode, U.at(Comment_Mode_Dic, code.mode));
            }
            else if (this.IsMagicAnimationType())
            {//魔法アニメ
                if (code.mode == 1)
                {
                    return R._("魔法の発動");
                }
                else if (code.mode == 2)
                {
                    return R._("魔法が命中");
                }
            }
            return "";
        }


        int ShowFloatingControlpanelInner(int index)
        {
            if (index < 0)
            {
                return 0;
            }
            //編集する項目の近くに移動させます.
            Rectangle rect = this.AddressList.GetItemRectangle(index);
            int y = this.MainPanel.Location.Y
                + this.AddressList.Location.Y
                + rect.Y + rect.Height + 20
                ;
            if (y + ControlPanel.Height >= AddressList.Height)
            {//下に余白がないので上に出す.
                y = this.MainPanel.Location.Y
                    + this.AddressList.Location.Y
                    + rect.Y
                    - ControlPanel.Height - 20;
                if (y < 0)
                {//上にも余白がないので、 Y = 0 の位置に出す
                    y = 0;
                }
            }
            return y;
        }
        
        void ShowFloatingControlpanel()
        {
            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.AnimeList.Count)
            {//一件もない

            }
            else
            {
                AnimeSt code = this.AnimeList[index];
                if (code.type == AnimeStEnum.Code)
                {
                    this.Code.Value = code.code;
                    this.MainTab.SelectedTab = this.CodeTabPage;
                }
                else if (code.type == AnimeStEnum.Image)
                {
                    this.Wait.Value = code.code;
                    if (!U.IsEmpty(code.obj))
                    {
                        U.FindListAndSelect(this.OBJ, code.obj);
                    }
                    if (!U.IsEmpty(code.bg))
                    {
                        U.FindListAndSelect(this.BG, code.bg);
                    }
                    this.MainTab.SelectedTab = this.ImageTabPage;
                }
                else if (code.type == AnimeStEnum.Loop)
                {
                    this.MainTab.SelectedTab = this.LoopTabPage;
                }
                else if (code.type == AnimeStEnum.Skill)
                {
                    this.SkillSound.Value = code.code;
                    if (code.bg == "D")
                    {
                        this.SkillType.SelectedIndex = 1;
                    }
                    else
                    {
                        this.SkillType.SelectedIndex = 0;
                    }
                    this.MainTab.SelectedTab = this.SkillTabPage;
                }
                else if (code.type == AnimeStEnum.Sound)
                {
                    this.Sound.Value = code.code;
                    this.MainTab.SelectedTab = this.SoundTabPage;
                }
                else if (code.type == AnimeStEnum.Term)
                {
                    this.MainTab.SelectedTab = this.TermPage;
                }
            }
            int y = ShowFloatingControlpanelInner(this.AddressList.SelectedIndex);
            ControlPanel.Location = new Point(ControlPanel.Location.X, y);

            //変更ボタンが光っていたら、それをやめさせる.
            InputFormRef.WriteButtonToYellow(this.UpdateButton, false);
            InputFormRef.WriteButtonToYellow(this.NewButton, false);

            ControlPanel.Show();
        }
        void HideFloatingControlpanel()
        {
            ControlPanel.Hide();
            
        }

        private void AddressList_DoubleClick(object sender, EventArgs e)
        {
            ShowFloatingControlpanel();
        }

        private void FloatCloseButton_Click(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
        }

        void SettingPlayButton(bool isPlay)
        {
            if (isPlay)
            {
                this.PlayButton.Text = R._("アニメーションの停止");
            }
            else
            {
                this.PlayButton.Text = R._("アニメーションを再生");
            }
        }

        //次回アニメーションを移動するTick
        long NextTickSec;
        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                SettingPlayButton(false);
                return;
            }

            if (this.AnimationType == AnimationTypeEnum.MagicAnime_FEEDitor 
             || this.AnimationType == AnimationTypeEnum.MagicAnime_CSACreator)
            {
                C29Brightness.Value = 0x10;
                C29Opacity.Value = 0x10;
                C53Expand.Value = CalcFEditorMagicInitCode();
            }


            //最初に戻す.
            U.SelectedIndexSafety(AddressList, 0);
            SettingPlayButton(true);

            //最初の画像だけ30フレーム秒だけ追加で表示するらしい.
            this.NextTickSec = DateTime.Now.Ticks + U.GameFrameSecToTickSec(30);
            timer1.Start();
        }
        int CalcFEditorMagicInitCode()
        {
            //先頭5つがCode 0x00だったら、FEEditor互換の魔法
            if (this.AnimeList.Count < 5)
            {
                return 0;
            }
            for (int index = 0; index < 5; index++)
            {
                AnimeSt code = this.AnimeList[index];
                if (code.type != AnimeStEnum.Code)
                {
                    return 0;
                }
                if (code.code != 0)
                {
                    return 0;
                }
            }
            return 1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Ticks < this.NextTickSec)
            {//また実行する時刻ではない.
                return;
            }

            uint hitPauseTime = 0;
            for (int index = AddressList.SelectedIndex + 1; index < this.AddressList.Items.Count; index++)
            {
                //攻撃モーションなどでウェイトを入れる FEditor Adv Gif Dumperより
                //85Commandによる遅延. ダメージエフェクトとか手斧なげたときとかの遅延
                AnimeSt code = this.AnimeList[index];
                if (code.type == AnimeStEnum.Code)
                {
                    if (code.code == 0x01)
                    {//01ハックしてループ命令がくることがある.
                        Debug.Assert(code.mode >= 1 && code.mode <= 0xC);
                        hitPauseTime = Math.Max(hitPauseTime, ImageUtilOAM.HIT_HOLD_TIMES[code.mode - 1]);
                    }
                    else if (code.code == 0x13)
                    {//13 手斧を投げたときだと思われる.
                        hitPauseTime = Math.Max(hitPauseTime, ImageUtilOAM.THROW_AXE_HOLD_TIME);
                    }
                    else if ((code.code & 0xff) == 0x29)
                    {
                        U.SelectedIndexSafety(C29Brightness , ((code.code >> 16) & 0xff));
                        U.SelectedIndexSafety(C29Opacity , ((code.code >> 8) & 0xff));
                    }
                    else if ((code.code & 0xff) == 0x53)
                    {
                        U.SelectedIndexSafety(C53Expand, ((code.code >> 8) & 0xff));
                    }
                }
                else if (code.type == AnimeStEnum.Image)
                {
                    if (hitPauseTime > 0)
                    {//手斧を多下駄理、ダメージモーションなどで追加して表示しなければいけない場合.
                        this.NextTickSec += U.GameFrameSecToTickSec(hitPauseTime);
                        hitPauseTime = 0;
                        if (DateTime.Now.Ticks < this.NextTickSec)
                        {//また実行する時刻ではない.
                            return;
                        }
                    }

                    //現在の画像をロード
                    U.SelectedIndexSafety(this.AddressList, index);
                    //この画像を表示し続ける時刻を記録.
                    uint wait = code.code;
                    this.NextTickSec = DateTime.Now.Ticks + U.GameFrameSecToTickSec(wait);
                    return;
                }

                U.SelectedIndexSafety(this.AddressList, index);
            }

            //終端なので止める.
            timer1.Stop();
            SettingPlayButton(false);
        }

        class UndoData
        {
            //UNDO サイズも小さいから、差分よりすべて記録する. 
            public List<AnimeSt> AnimeList;
        };

        List<UndoData> UndoBuffer;
        int UndoPosstion;

        static List<AnimeSt> CloneAnimeList(List<AnimeSt> animeList)
        {
            List<AnimeSt> list = new List<AnimeSt>();
            int length = animeList.Count;
            for (int i = 0; i < length; i++)
            {
                AnimeSt code = animeList[i];
                AnimeSt a = new AnimeSt();
                a.type = code.type;
                a.bg = code.bg;
                a.obj = code.obj;
                a.jisage = code.jisage;
                a.mode = code.mode;
                a.code = code.code;
                list.Add(a);
            }
            return list;
        }

        //Undo履歴のクリア
        void ClearUndoBuffer()
        {
            this.UndoBuffer = new List<UndoData>();
            this.UndoPosstion = 0;
        }
        void PushUndo()
        {
            if (this.UndoPosstion < this.UndoBuffer.Count)
            {//常に先頭に追加したいので、リスト中に戻っている場合は、それ以降を消す.
                this.UndoBuffer.RemoveRange(this.UndoPosstion, this.UndoBuffer.Count - this.UndoPosstion);
            }
            UndoData p = new UndoData();
            p.AnimeList = CloneAnimeList(this.AnimeList);
            this.UndoBuffer.Add(p);
            this.UndoPosstion = this.UndoBuffer.Count;
        }
        void RunUndo()
        {
            if (this.UndoPosstion <= 0)
            {
                return; //無理
            }
            if (this.UndoPosstion == this.UndoBuffer.Count)
            {//現在が、undoがない最新版だったら、redoできるように、現状を保存する.
                PushUndo();
                this.UndoPosstion = UndoPosstion - 1;
            }

            this.UndoPosstion = UndoPosstion - 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);

        }
        void RunRedo()
        {
            if (this.UndoPosstion + 1 >= this.UndoBuffer.Count)
            {
                return; //無理
            }
            this.UndoPosstion = UndoPosstion + 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);
        }
        void RunUndoRollback(UndoData u)
        {
            this.AnimeList = CloneAnimeList(u.AnimeList);

            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, this.AddressList.SelectedIndex);
        }

        private void UNDOButton_Click(object sender, EventArgs e)
        {
            RunUndo();
        }

        private void REDOButton_Click(object sender, EventArgs e)
        {
            RunRedo();
        }

        AnimeSt MakeTabToAnimeSt()
        {
            AnimeSt a = new AnimeSt();

            if (this.MainTab.SelectedTab == this.ImageTabPage)
            {
                a.type = AnimeStEnum.Image;
                if (IsMagicAnimationType())
                {
                    if (this.BG.SelectedIndex < 0)
                    {
                        R.ShowStopError("背景を選択してください。");
                        return null;
                    }

                    a.bg = this.BG.Text;
                }
                else
                {
                    a.bg = null;
                }

                if (this.OBJ.SelectedIndex < 0)
                {
                    R.ShowStopError("画像を選択してください。");
                    return null;
                }
                a.obj = this.OBJ.Text;
                a.code = (uint)this.Wait.Value;
            }
            else if (this.MainTab.SelectedTab == this.SoundTabPage)
            {
                a.type = AnimeStEnum.Sound;
                a.code = (uint)this.Sound.Value;
            }
            else if (this.MainTab.SelectedTab == this.CodeTabPage)
            {
                a.type = AnimeStEnum.Code;
                a.code = (uint)this.Code.Value;
            }
            else if (this.MainTab.SelectedTab == this.SkillTabPage)
            {
                a.type = AnimeStEnum.Skill;
                a.code = (uint)this.SkillSound.Value;
                if (this.SkillType.SelectedIndex == 1)
                {
                    a.obj = "D";
                }
                else
                {
                    a.obj = "";
                }
            }
            else if (this.MainTab.SelectedTab == this.LoopTabPage)
            {
                a.type = AnimeStEnum.Loop;
            }
            else if (this.MainTab.SelectedTab == this.TermPage)
            {
                a.type = AnimeStEnum.Term;
            }
            else
            {
                Debug.Assert(false);
                return null;
            }
            return a;
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            int selected = this.AddressList.SelectedIndex;
            if (selected < 0 || selected >= this.AnimeList.Count)
            {//追加で処理する.
                NewButton.PerformClick();
                return;
            }

            //タブで選択されている内容からAnimeStを生成する
            AnimeSt code = MakeTabToAnimeSt();
            if (code == null)
            {
                return;
            }

            //選択されているコードを入れ替える.
            this.AnimeList[this.AddressList.SelectedIndex] = code;

            //最後に自下げ処理実行.
            JisageReorder();

            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, selected);

            HideFloatingControlpanel();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (this.AnimeList == null)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            //タブで選択されている内容からAnimeStを生成する
            AnimeSt code = MakeTabToAnimeSt();
            if (code == null)
            {
                return;
            }

            int selected;
            //選択されている部分に追加
            if (this.AddressList.SelectedIndex < 0)
            {
                this.AnimeList.Add(code);
                selected = this.AnimeList.Count - 1;
            }
            else
            {
                if (IsSelectedTermCode())
                {//終端コードの後ろには追加させない
                    this.AnimeList.Insert(this.AddressList.SelectedIndex, code);
                    selected = this.AddressList.SelectedIndex;
                }
                else
                {
                    this.AnimeList.Insert(this.AddressList.SelectedIndex + 1, code);
                    selected = this.AddressList.SelectedIndex + 1;
                }
            }
            //最後に自下げ処理実行.
            JisageReorder();

            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, selected);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();
        }

        //選択しているのは、終端コードかどうか？
        bool IsSelectedTermCode()
        {
            int selected = this.AddressList.SelectedIndex;
            if (selected < 0)
            {
                return false;
            }
            if (selected >= this.AnimeList.Count)
            {
                return false;
            }
            if (this.AnimeList[selected].type == AnimeStEnum.Term)
            {//選択しているのは終端コード
                return true;
            }
            return false;
        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int selected = this.AddressList.SelectedIndex;
            if (selected < 0 || selected >= this.AnimeList.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            this.AnimeList.RemoveAt(this.AddressList.SelectedIndex);
            //最後に自下げ処理実行.
            JisageReorder();

            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, this.AddressList.SelectedIndex - 1);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();        
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex < 1 || this.AddressList.SelectedIndex >= this.AnimeList.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            U.SwapUp(this.AnimeList, this.AddressList, this.AddressList.SelectedIndex);

            //最後に自下げ処理実行.
            JisageReorder();

            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, this.AddressList.SelectedIndex - 1);

            //コントロールパネルを閉じたくない.
            ControlPanel.Show();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex < 0 || this.AddressList.SelectedIndex + 1 >= this.AnimeList.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            U.SwapDown(this.AnimeList, this.AddressList, this.AddressList.SelectedIndex);

            //最後に自下げ処理実行.
            JisageReorder();

            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, this.AddressList.SelectedIndex + 1);

            //コントロールパネルを閉じたくない.
            ControlPanel.Show();
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(null))
            {
                R.ShowStopError(InputFormRef.GetBusyErrorExplain());
                return;
            }

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(U.ControlToParentForm(this)))
            //テンポラリディレクトリを利用する
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                string filename = Path.Combine(tempdir.Dir, "anime.txt");
                if (this.AnimationType == AnimationTypeEnum.BattleAnime)
                {
                    if (!ExportBattleAnimeWrite(filename))
                    {
                        return;
                    }

                    ImageBattleAnimeForm f = (ImageBattleAnimeForm)InputFormRef.JumpForm<ImageBattleAnimeForm>(U.NOT_FOUND);
                    string error = f.BattleAnimeImportDirect(this.ID, filename);
                    if (error != "")
                    {
                        R.ShowStopError(error);
                        return;
                    }
                }
                else if (this.AnimationType == AnimationTypeEnum.MagicAnime_CSACreator)
                {
                    if (! ExportMagicAnimeWrite(filename))
                    {
                        return;
                    }

                    ImageMagicCSACreatorForm f = (ImageMagicCSACreatorForm)InputFormRef.JumpForm<ImageMagicCSACreatorForm>(U.NOT_FOUND);
                    string error = f.MagicAnimeImportDirect(this.ID, filename);
                    if (error != "")
                    {
                        R.ShowStopError(error);
                        return;
                    }
                }
                else if (this.AnimationType == AnimationTypeEnum.MagicAnime_FEEDitor)
                {
                    if (! ExportMagicAnimeWrite(filename) )
                    {
                        return;
                    }

                    ImageMagicFEditorForm f = (ImageMagicFEditorForm)InputFormRef.JumpForm<ImageMagicFEditorForm>(U.NOT_FOUND);
                    string error = f.MagicAnimeImportDirect(this.ID, filename);
                    if (error != "")
                    {
                        R.ShowStopError(error);
                        return;
                    }
                }
                else if (this.AnimationType == AnimationTypeEnum.Skill)
                {
                    if (! ExportSkillAnimeWrite(filename) )
                    {
                        return;
                    }

                    PatchUtil.skill_system_enum skillsystem = PatchUtil.SearchSkillSystem();
                    if (skillsystem == PatchUtil.skill_system_enum.FE8N_ver2)
                    {
                        SkillConfigFE8NVer2SkillForm f = (SkillConfigFE8NVer2SkillForm)InputFormRef.JumpForm<SkillConfigFE8NVer2SkillForm>(U.NOT_FOUND);
                        string error = f.SkillAnimeImportDirect(this.ID, filename);
                        if (error != "")
                        {
                            R.ShowStopError(error);
                            return;
                        }
                    }
                    else if (skillsystem == PatchUtil.skill_system_enum.SkillSystem)
                    {
                        SkillConfigSkillSystemForm f = (SkillConfigSkillSystemForm)InputFormRef.JumpForm<SkillConfigSkillSystemForm>(U.NOT_FOUND);
                        string error = f.SkillAnimeImportDirect(this.ID, filename);
                        if (error != "")
                        {
                            R.ShowStopError(error);
                            return;
                        }
                    }
                    else
                    {
                        Debug.Assert(false);
                        R.ShowStopError(R._("スキル拡張がありません。\r\nスキル拡張パッチをインストールしてから利用してください。")); 
                    }
                }
            }
            InputFormRef.WriteButtonToYellow(AllWriteButton,false);
        }


        string AnimeToTextOne(int i)
        {
            AnimeSt code = this.AnimeList[i];
            return
                (int)code.type
                    + "\t" + code.code
                    + "\t" + (code.obj == null ? "" : code.obj)
                    + "\t" + (code.bg == null ? "" : code.bg)
                    + "\t" + ToolAnimationCreatorForm.MakeUniqID(this.AnimationType, this.ID)
                    + "\t@" + this.Name;
        }
        void TextToAnime(string text,int i)
        {
            string[] sp = text.Split('\t');
            if (sp.Length < 6)
            {
                return;
            }
            if ("@" + this.Name != sp[5])
            {
                return;
            }
            AnimeSt code = new AnimeSt();
            code.type = (AnimeStEnum) U.atoi(sp[0]);
            code.code = U.atoi(sp[1]);
            code.obj = sp[2];
            code.bg = sp[3];

            if (code.type == AnimeStEnum.Image)
            {
                uint uniq = U.atoi(sp[4]);
                if (uniq != ToolAnimationCreatorForm.MakeUniqID(this.AnimationType, this.ID))
                {
                    R.ShowStopError("今はまだ、別アニメーションの画像をコピーできません。\r\nそのアニメーションをエクスポートして、画像単体で追加してください。");
                    return;
                }

                if (!U.IsEmpty(code.bg) && !this.AnimeBG.ContainsKey(code.bg))
                {
                    R.ShowStopError("画像({0})がリスト存在しないため、ペーストできません。");
                    return;
                }
                if (!U.IsEmpty(code.obj) && !this.AnimeObj.ContainsKey(code.obj))
                {
                    R.ShowStopError("画像({0})がリスト存在しないため、ペーストできません。");
                    return;
                }
            }

            PushUndo();
            if (this.AnimeList.Count <= 0)
            {
                this.AnimeList.Add(code);
            }
            else
            {
                this.AnimeList.Insert(i, code);
            }

            JisageReorder();
            
            //リストの更新.
            this.AddressList.DummyAlloc(this.AnimeList.Count, i);
        }

        private void AddressList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                RunUndo();
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                RunRedo();
                return;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ShowFloatingControlpanel();
                return;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                RemoveButton_Click(sender,e);
                return;
            }

            int i = this.AddressList.SelectedIndex;
            if (i < 0 || i >= this.AddressList.Items.Count)
            {
                return;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                U.SetClipboardText(AnimeToTextOne(i));
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                TextToAnime(Clipboard.GetText(), i);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
                return;
            }

        }

        string MakeUniqFilename(Dictionary<string,Bitmap>.KeyCollection keys, string name)
        {
            foreach (var key in keys)
            {
                if (key == name)
                {
                    //同名のファイルがあれば名前を変えます.
                    return MakeUniqFilename(keys, "_" + name);
                }
            }
            return name;
        }

        private void OBJImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap;
            if (this.AnimationType == AnimationTypeEnum.BattleAnime)
            {//戦闘アニメはいろいろな制限がある
                Bitmap paletteHint;
                if (this.OBJ.Items.Count <= 0 
                    || !this.AnimeObj.ContainsKey(this.OBJ.Text))
                {//最初の画像なので何もヒントがない
                    paletteHint = null;
                }
                else
                {//ヒントがある
                    paletteHint = this.AnimeObj[this.OBJ.Text];
                }

                bitmap = ImageFormRef.ImportFilenameDialog(this, paletteHint);
                if (bitmap == null)
                {
                    return;
                }

                if (bitmap.Height < ImageUtilOAM.SCREEN_TILE_HEIGHT * 8)
                {
                    R.ShowStopError("画像サイズが正しくありません。\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height);
                    return;
                }


                if (paletteHint != null)
                {//ヒントの画像とパレットは一致するか?
                    byte[] paletteHintPalette = ImageUtil.ImageToPalette(paletteHint, 1);
                    byte[] imagepalette = ImageUtil.ImageToPalette(bitmap, 1);
                    if (U.memcmp(imagepalette, paletteHintPalette) != 0)
                    {
                        //バトルアニメの場合は、パレットは共通なのでエラーを返す.
                        string errorPaletteMessage = ImageUtil.CheckPalette(bitmap.Palette
                            , paletteHint.Palette
                            , null
                            );
                        string errorMessage = R.Error("パレットがほかと異なります\r\n{0}", errorPaletteMessage);

                        ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                        f.SetErrorMessage(errorPaletteMessage);
                        f.SetOrignalImage(ImageUtil.ReOrderPalette(bitmap, paletteHintPalette, 0));
                        f.ShowForceButton();
                        f.ShowDialog();

                        bitmap = f.GetResultBitmap();
                        if (bitmap == null)
                        {//キャンセル.
                            R.ShowStopError(errorMessage);
                            return ;
                        }
                    }
                }

                //透過色テスト
                bitmap = ImageUtil.ConvertPaletteTransparentUI(bitmap);
                if (bitmap == null)
                {//キャンセル.
                    R.ShowStopError("透過色を認識できません");
                    return;
                }
            }
            else if (this.AnimationType == AnimationTypeEnum.MagicAnime_CSACreator || this.AnimationType == AnimationTypeEnum.MagicAnime_FEEDitor)
            {
                bitmap = ImageFormRef.ImportFilenameDialog(this);
                if (bitmap == null)
                {
                    return;
                }

                //透過色テスト
                bitmap = ImageUtil.ConvertPaletteTransparentUI(bitmap);
                if (bitmap == null)
                {//キャンセル.
                    R.ShowStopError("透過色を認識できません");
                    return;
                }
                //魔法の場合、パレットを切り替えられるのでチェックは意味がない.
            }
            else if (this.AnimationType == AnimationTypeEnum.Skill)
            {
                bitmap = ImageFormRef.ImportFilenameDialog(this);
                if (bitmap == null)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            //インポートできる.
            string lastSelectedFilename = Program.LastSelectedFilename.GetLastFilename(this, "");
            if (lastSelectedFilename == "")
            {
                lastSelectedFilename = "_";
            }
            else
            {
                lastSelectedFilename = Path.GetFileName(lastSelectedFilename);
            }
            lastSelectedFilename = MakeUniqFilename(this.AnimeObj.Keys, lastSelectedFilename);

            this.AnimeObj[lastSelectedFilename] = bitmap;
            this.OBJ.Items.Add(lastSelectedFilename);
            U.SelectedIndexSafety(this.OBJ, this.OBJ.Items.Count - 1);
        }

        private void OBJExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = U.at(this.AnimeObj,this.OBJ.Text);
            ImageFormRef.ExportImage(this,bitmap, this.OBJ.Text);
        }

        private void BGImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap;
            if (this.AnimationType == AnimationTypeEnum.MagicAnime_CSACreator || this.AnimationType == AnimationTypeEnum.MagicAnime_FEEDitor)
            {
                bitmap = ImageFormRef.ImportFilenameDialog(this);
                if (bitmap == null)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            //インポートできる.
            string lastSelectedFilename = Program.LastSelectedFilename.GetLastFilename(this, "");
            if (lastSelectedFilename == "")
            {
                lastSelectedFilename = "_";
            }
            else
            {
                lastSelectedFilename = Path.GetFileName(lastSelectedFilename);
            }
            lastSelectedFilename = MakeUniqFilename(this.AnimeBG.Keys, lastSelectedFilename);

            this.AnimeBG[lastSelectedFilename] = bitmap;
            this.BG.Items.Add(lastSelectedFilename);
            U.SelectedIndexSafety(this.BG, this.BG.Items.Count - 1);
        }

        private void BGExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = U.at(this.AnimeBG, this.BG.Text);
            ImageFormRef.ExportImage(this,bitmap, this.BG.Text);
        }

        public void SetFocus()
        {
            this.AddressList.Focus();
        }

        private void ToolAnimationCreatorUserControl_Resize(object sender, EventArgs e)
        {
            WriteButtonPanel.Location = new Point(WriteButtonPanel.Location.X
                        , this.Height - WriteButtonPanel.Height);
            MainPanel.Location = new Point(this.Width - MainPanel.Width, 0);
            MainPanel.Height =  this.Height - 4;
            AddressList.Height =  MainPanel.Height;
            ControlPanel.Location = new Point(MainPanel.Location.X, ControlPanel.Location.Y);

            if (MainPanel.Location.X + ControlPanel.Width > this.Width)
            {
                ControlPanel.Location = new Point(this.Width - ControlPanel.Width , ControlPanel.Location.Y);
            }

            SamplePanel.Width = this.Width - MainPanel.Width - 4;
            SamplePanel.Height = this.Height - WriteButtonPanel.Height - 4;
            ModeHint.Width = SamplePanel.Width - 4;

            SampleOptionPanel.Location = new Point(PlayButton.Location.X
                , SamplePanel.Height - SampleOptionPanel.Height);
            PlayButton.Location = new Point(PlayButton.Location.X
                , SampleOptionPanel.Location.Y - PlayButton.Height - 8);
            PlayButton.Width = SamplePanel.Width - PlayButton.Location.X - 4;

            //サンプル画面をできるだけ大きく表示
            X_ANIME_PIC.Width = SamplePanel.Width - X_ANIME_PIC.Location.X - 4;
            X_ANIME_PIC.Height = PlayButton.Location.Y - X_ANIME_PIC.Location.Y - 8;
        }

        private void SoundPlayCodeButton_Click(object sender, EventArgs e)
        {
            uint command = (uint)this.Code.Value;
            uint c = command & 0xff;

            string soundText = SongTableForm.GetC85SoundEffect(c);
            if (soundText == "")
            {
                return;
            }
            uint sound = U.atoh(soundText);
            MainFormUtil.RunAsSappy(sound);
        }

        private void JumpToSoundTableCode_Click(object sender, EventArgs e)
        {
            uint command = (uint)this.Code.Value;
            uint c = command & 0xff;

            string soundText = SongTableForm.GetC85SoundEffect(c);
            if (soundText == "")
            {
                InputFormRef.JumpForm<SongTableForm>();
                return;
            }
            uint sound = U.atoh(soundText);
            InputFormRef.JumpForm<SongTableForm>(sound);
        }
        private void JumpToSoundTableSound_Click(object sender, EventArgs e)
        {
            uint sound = (uint)this.Sound.Value;
            InputFormRef.JumpForm<SongTableForm>(sound);
        }



    }
}
