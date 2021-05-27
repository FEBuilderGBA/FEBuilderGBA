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
    public partial class ToolMagicEffectMakerForm : Form
    {
        public ToolMagicEffectMakerForm()
        {
            InitializeComponent();

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, SoundFire, SoundFireInfo, 0, "SONG", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, SoundFire, SoundFirePlaySoundButton, 0, "SONGPLAY", new string[] { });

            InputFormRef.makeLinkEventHandler("", controls, SoundHit, SoundHitInfo, 0, "SONG", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, SoundHit, SoundHitPlaySoundButton, 0, "SONGPLAY", new string[] { });

            this.ConvertType.SelectedIndex = 0;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("スクリプト|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", save , "");

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                ConvertBitmaps(filename);
            }

            U.SelectFileByExplorer(filename);
        }

        Bitmap ConvertBitmap(Bitmap src,int srcx,int srcy)
        {
            U.MakeTransparent(src);

            Bitmap bitmap = new Bitmap( (int)this.BitmapW, (int)this.BitmapH);
            Graphics g = Graphics.FromImage(bitmap);

            g.DrawImage(src
                , new Rectangle((int)this.ResizeX.Value,(int)this.ResizeY.Value
                ,(int)this.ResizeW.Value,(int)this.ResizeH.Value)
                , new Rectangle(srcx, srcy
                , (int)this.PartsX.Value, (int)this.PartsY.Value)
                , GraphicsUnit.Pixel
                );

            g.Dispose();

            Bitmap ret;
            if (src.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed)
            {//元々が16色
                string errormessage;
                ret = ImageUtil.ConvertIndexedBitmap(bitmap, src, out errormessage);
            }
            else if (src.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {//元々が256色
                string errormessage;
                Bitmap temp = ImageUtil.ConvertIndexedBitmap(bitmap, src, out errormessage);

                DecreaseColor d = new DecreaseColor();
                ret = d.Convert(temp, 1, 0, true , false);

                temp.Dispose();
            }
            else
            {//もともとがフルカラー
                DecreaseColor d = new DecreaseColor();
                ret = d.Convert(bitmap, 1, 0, true , false );
            }

            bitmap.Dispose();

            return ret;
        }
        void ConvertBitmaps(string saveFilename)
        {
            Bitmap src = ImageUtil.OpenLowBitmap(OrignalFilename.Text);
            if (src == null)
            {
                return;
            }
            if (!U.CanWriteFileRetry(saveFilename))
            {
                return;
            }
            List<string> liner = new List<string>();

            string dir = Path.GetDirectoryName(saveFilename);

            int partsW = (int)this.PartsX.Value;
            int partsH = (int)this.PartsY.Value;
            int frameWait = (int)this.FrameWait.Value;
            int selected = ConvertType.SelectedIndex;

            int fileCount = 0;
            string dummyObjectFilename = "animeobj.png";

            //ヘッダ
            int sound = (int)this.SoundFire.Value;
            if (IsSkillAnime(selected))
            {
                if (selected == 1)
                {
                    liner.Add("D");
                }
                string line = "S" + U.ToHexString(sound);
                liner.Add(line);
            }
            else if (IsMagicAnimeByFEidtor(selected))
            {
                liner.Add("C00");
                liner.Add("C00");
                liner.Add("C00");
                liner.Add("C00");
                liner.Add("C40 //Scrolls the screen");
                liner.Add("S" + U.ToHexString(sound));

                Bitmap obj = ImageUtil.Blank(264,64);
                obj.Save(Path.Combine(dir, dummyObjectFilename));                
            }
            else if (IsMagicAnimeByCSACreator(selected))
            {
                liner.Add("C000000");
                liner.Add("C40 //Scrolls the screen");
                liner.Add("S" + U.ToHexString(sound));
            
                Bitmap obj = ImageUtil.Blank(480, 160);
                obj.Save(Path.Combine(dir, dummyObjectFilename));
            }

            for (int srcy = 0; srcy + partsH < src.Height; srcy += partsH)
            {
                for (int srcx = 0; srcx + partsW < src.Width; srcx += partsW)
                {
                    Bitmap bitmap = ConvertBitmap(src, srcx, srcy);

                    string imgFilename;
                    imgFilename ="anime" + fileCount.ToString("000") + ".png";
                    bitmap.Save(Path.Combine(dir, imgFilename));

                    fileCount++;

                    if (IsSkillAnime(selected))
                    {
                        string line = frameWait + " " + imgFilename;
                        liner.Add(line); 
                    }
                    else if (IsMagicAnimeByFEidtor(selected))
                    {
                        string line;
                        line = "O" + " p- " + dummyObjectFilename;
                        liner.Add(line);
                        line = "B" + " p- " + imgFilename;
                        liner.Add(line);
                        line = frameWait.ToString();
                        liner.Add(line); 
                    }
                    else if (IsMagicAnimeByCSACreator(selected))
                    {
                        string line;
                        line = "O" + " " + dummyObjectFilename;
                        liner.Add(line);
                        line = "B" + " " + imgFilename;
                        liner.Add(line);
                        line = frameWait.ToString();
                        liner.Add(line);
                    }
                }
            }

            if (IsMagicAnimeByFEidtor(selected) || IsMagicAnimeByCSACreator(selected))
            {
                liner.Add("C1A");
                liner.Add("C1F");
                liner.Add("~~~ // miss terminator");
                liner.Add("S0133 // Hit sfx");
                liner.Add("~~~");
            }

            File.WriteAllLines(saveFilename, liner.ToArray());
        }

        bool IsSkillAnime(int selected)
        {
            return selected == 0 || selected == 1;
        }
        bool IsMagicAnimeByFEidtor(int selected)
        {
            return selected == 2 || selected == 4;
        }
        bool IsMagicAnimeByCSACreator(int selected)
        {
            return selected == 3 || selected == 5;
        }

        private void ToolMagicEffectMakerForm_Load(object sender, EventArgs e)
        {
        }

        uint BitmapW;
        uint BitmapH;
        
        private void ConvertType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = ConvertType.SelectedIndex;
            if (selected == 0)
            {//クラススキル
                this.BitmapW = 240;
                this.BitmapH = 224;
                this.ResizeW.Value = 122;
                this.ResizeH.Value = 122;
                this.ResizeX.Value = 80;
                this.ResizeY.Value = 20;
                this.SoundFire.Value = 0x3D1;
                this.SoundHit.Value = 0x0;
            }
            else if (selected == 1)
            {//クラススキル(FE8U:防衛スキル)
                this.BitmapW = 240;
                this.BitmapH = 224;
                this.ResizeW.Value = 122;
                this.ResizeH.Value = 122;
                this.ResizeX.Value = 80;
                this.ResizeY.Value = 40;
                this.SoundFire.Value = 0x3D1;
                this.SoundHit.Value = 0x0;
            }
            else if (selected == 2)
            {//魔法(FEditor) (画面中央)
                this.BitmapW = 240;
                this.BitmapH = 60;
                this.ResizeW.Value = 240;
                this.ResizeH.Value = 60;
                this.ResizeX.Value = 20;
                this.ResizeY.Value = 0;
                this.SoundFire.Value = 0x100;
                this.SoundHit.Value = 0x133;
            }
            else if (selected == 3)
            {//魔法(CSA) (画面中央)
                this.BitmapW = 240;
                this.BitmapH = 160;
                this.ResizeW.Value = 240;
                this.ResizeH.Value = 150;
                this.ResizeX.Value = 20;
                this.ResizeY.Value = 0;
                this.SoundFire.Value = 0x100;
                this.SoundHit.Value = 0x133;
            }
            else if (selected == 4)
            {//魔法(FEditor) (ユニットの位置)
                this.BitmapW = 240;
                this.BitmapH = 60;
                this.ResizeW.Value = 112;
                this.ResizeH.Value = 40;
                this.ResizeX.Value = 80;
                this.ResizeY.Value = 10;
                this.SoundFire.Value = 0x100;
                this.SoundHit.Value = 0x133;
            }
            else if (selected == 5)
            {//魔法(CSA) (ユニットの位置)
                this.BitmapW = 240;
                this.BitmapH = 160;
                this.ResizeW.Value = 112;
                this.ResizeH.Value = 112;
                this.ResizeX.Value = 80;
                this.ResizeY.Value = 40;
                this.SoundFire.Value = 0x100;
                this.SoundHit.Value = 0x133;
            }

            SoundHitLabel.Visible = (this.SoundHit.Value != 0);
            SoundHit.Visible = (this.SoundHit.Value != 0);
            SoundHitInfo.Visible = (this.SoundHit.Value != 0);
            SoundHitPlaySoundButton.Visible = (this.SoundHit.Value != 0);
        }

        private void OrignalFilenameSelectButton_Click(object sender, EventArgs e)
        {
            string filename = ImageFormRef.OpenFilenameDialogFullColor(this);
            if (filename == "")
            {
                return;
            }

            this.OrignalFilename.Text = filename;
        }

        private void OrignalFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OrignalFilenameSelectButton.PerformClick();
        }
    }
}
