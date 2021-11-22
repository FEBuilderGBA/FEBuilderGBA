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
    public partial class ImageMapActionAnimationForm : Form
    {
        static uint g_AnimationPointer = 0;
        static uint FindAnimationPointer()
        {
            if (g_AnimationPointer == 0)
            {
                g_AnimationPointer = FindAnimationPointerLow();
            }
            return g_AnimationPointer;
        }
        static Dictionary<uint, string> g_DefaultNameCache = null;
        static string GetNameDefaultName(uint id)
        {
            if (g_DefaultNameCache == null)
            {
                Dictionary<uint, string> a = U.LoadDicResource(U.ConfigDataFilename("MapActionAnimation_"));
                g_DefaultNameCache = a;
                return U.at(a, id);
            }
            return U.at(g_DefaultNameCache,id);
        }

        static uint FindAnimationPointerLow()
        {
            byte[] bin;
            if (Program.ROM.RomInfo.is_multibyte())
            {//FE8J
                bin = new byte[]{0x54, 0x3C, 0x08, 0x08, 0xEC, 0xE1, 0x03, 0x02, 0xE8, 0xA4, 0x03, 0x02, 0x68, 0xA5, 0x03, 0x02, 0xFF, 0xFF, 0x00, 0x00};
            }
            else
            {//FE8U
                bin = new byte[] { 0x14, 0x19, 0x08, 0x08, 0xF0, 0xE1, 0x03, 0x02, 0xEC, 0xA4, 0x03, 0x02, 0x6C, 0xA5, 0x03, 0x02, 0xFF, 0xFF, 0x00, 0x00 };
            }
            uint p = U.GrepEnd(Program.ROM.Data, bin, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4, 0, true);
            if (p == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            p = p - (uint)bin.Length - 4;
            uint a = Program.ROM.u32(p);
            if (! U.isPointer(a))
            {
                return U.NOT_FOUND;
            }
            return p;
        }


        public ImageMapActionAnimationForm()
        {
            InitializeComponent();

            uint AnimeP = FindAnimationPointer();
            if (AnimeP == U.NOT_FOUND)
            {
                return;
            }
            uint animeBaseAddress = Program.ROM.p32(AnimeP);

            this.AddressList.OwnerDraw(DrawMapAttackAnimationAndText, DrawMode.OwnerDrawFixed);
            InputFormRef = Init(this, AnimeP);
            this.InputFormRef.MakeGeneralAddressListContextMenu(false);
            this.InputFormRef.CheckProtectionPaddingALIGN4 = false;

            ShowZoomComboBox.SelectedIndex = 0;
            U.SetIcon(AnimationImportButton, Properties.Resources.icon_upload);
            U.SetIcon(AnimationExportButton, Properties.Resources.icon_arrow);

            U.AllowDropFilename(this, new string[] { ".TXT" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    AnimationImportButton_Click(null, null);
                }
            });
        }
        //攻撃モーション + テキストを書くルーチン
        Size DrawMapAttackAnimationAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (ListBoxEx.OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            Bitmap bitmap = ImageMapActionAnimationForm.DrawIcon((uint)index);
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);
            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;

            brush.Dispose();
            return new Size(bounds.X, bounds.Y);
        }


        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, uint animePointer)
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , animePointer
                , 8
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (addr + 4 > Program.ROM.Data.Length)
                    {
                        return false;
                    }
                    uint a = Program.ROM.u32(addr);
                    if (U.isSafetyPointerOrNull(a))
                    {
                        return true;
                    }
                    return false;
                }
                , (int i, uint addr) =>
                {
                    string name = InputFormRef.GetCommentSA(addr);
                    if (name == "")
                    {
                        name = U.SA(GetNameDefaultName((uint)i));
                    }
                    return U.ToHexString(i) + name;
                }
            );
            return ifr; 
        }


        public static void ClearCache()
        {
            g_AnimationPointer = 0;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Comment.Text == "")
            {
                Comment.Text = GetNameDefaultName((uint)AddressList.SelectedIndex);
            }

            if (AddressList.SelectedIndex <= 0)
            {
                AnimationPanel.Hide();
                AnimationImportButton.Hide();
                AnimationExportButton.Hide();
                NOTIFY_KeepEmpty.Show();
                return;
            }
            NOTIFY_KeepEmpty.Hide();
            AnimationImportButton.Show();

            uint addr = U.toOffset((uint)P0.Value);
            if (U.isSafetyOffset(addr))
            {
                AnimationPanel.Show();
                AnimationExportButton.Show();
                ShowFrameUpDown.Value = 0;
                ShowFrameUpDown_ValueChanged(null,null);
            }
            else
            {
                AnimationPanel.Hide();
                AnimationExportButton.Hide();
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            uint AnimeP = FindAnimationPointer();
            if (AnimeP == U.NOT_FOUND)
            {
                return;
            }
            uint animeBaseAddress = Program.ROM.p32(AnimeP);

            InputFormRef InputFormRef;
            InputFormRef = Init(null, AnimeP);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MapActionAnimation", new uint[] {0});

            animeBaseAddress += InputFormRef.BlockSize; //skip empty 00
            for (uint i = 1; i < InputFormRef.DataCount; i++, animeBaseAddress += InputFormRef.BlockSize)
            {
                uint addr = Program.ROM.p32(animeBaseAddress);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                string name = "MapActionAnime:" + U.To0xHexString(i) + " ";

                ImageUtilMapActionAnimation.RecycleOldAnime(ref list
                    ,name
                    ,isPointerOnly
                    ,addr);
            }
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            uint AnimeP = FindAnimationPointer();
            if (AnimeP == U.NOT_FOUND)
            {
                return;
            }
            uint animeBaseAddress = Program.ROM.p32(AnimeP);

            InputFormRef InputFormRef;
            InputFormRef = Init(null, AnimeP);
            animeBaseAddress += InputFormRef.BlockSize; //skip empty 00
            for (uint i = 1; i < InputFormRef.DataCount; i++, animeBaseAddress += InputFormRef.BlockSize)
            {
                uint addr = Program.ROM.p32(animeBaseAddress);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                string name = "MapActionAnime:" + U.To0xHexString(i) + " ";

                ImageUtilMapActionAnimation.MakeCheckError(errors, addr, i);
            }
        }

        private void ShowZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ShowZoomComboBox.SelectedIndex == 0)
            {
                AnimationPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                AnimationPictureBox.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void ShowFrameUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint anime = U.toOffset((uint)P0.Value);
            uint frame = (uint)ShowFrameUpDown.Value;
            AnimationPictureBox.Image = ImageUtilMapActionAnimation.Draw(anime, frame);
        }
        public static String GetName(uint id)
        {
            uint AnimeP = FindAnimationPointer();
            if (AnimeP == U.NOT_FOUND)
            {
                return "";
            }
            InputFormRef InputFormRef;
            InputFormRef = Init(null, AnimeP);

            return InputFormRef.GetNameFull(id);
        }

        public static Bitmap DrawIcon(uint id, uint frame = 3)
        {
            if (id == 0)
            {
                return ImageUtil.BlankDummy();
            }

            uint AnimeP = FindAnimationPointer();
            if (AnimeP == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }
            uint animeBaseAddress = Program.ROM.p32(AnimeP);

            uint addr = animeBaseAddress + (id * 8);
            if (! U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }
            uint anime = Program.ROM.p32(addr);
            return ImageUtilMapActionAnimation.Draw(anime, frame);
        }

        private void AnimationExportButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("攻撃アニメスクリプト|*.MapActionAnimation.txt|アニメGIF|*.gif|Dump All|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, "MapActionAnimation_" + this.AddressList.Text.Trim());

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

            uint addr = U.toOffset((uint)P0.Value);
            if (save.FilterIndex == 2)
            {//GIF
                ImageUtilMapActionAnimation.ExportGif(filename, addr);
            }
            else if (save.FilterIndex == 3)
            {//All
                ImageUtilMapActionAnimation.Export(filename, addr ,this.Comment.Text);
                filename = U.ChangeExtFilename(filename, ".gif");
                ImageUtilMapActionAnimation.ExportGif(filename, addr);
            }
            else
            {//Script
                ImageUtilMapActionAnimation.Export(filename, addr, this.Comment.Text);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void AnimationImportButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {

                string title = R._("開くファイル名を選択してください");
                string filter = R._("攻撃アニメスクリプト|*.MapActionAnimation.txt|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                open.FileName = "MapActionAnimation_" + this.AddressList.Text.Trim();
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                if (!U.CanReadFileRetry(open))
                {
                    return;
                }
                filename = open.FileNames[0];
                Program.LastSelectedFilename.Save(this, "", open);
            }

            uint id = (uint)this.AddressList.SelectedIndex;
            string error = MapActionAnimationImportDirect(id, filename);

            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }
        }

        public string MapActionAnimationImportDirect(uint id, string filename)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return R._("現在他の処理中です");
            }

            if (id <= 0)
            {
                return R._("指定されたID({0})は存在しません。", U.To0xHexString(id));
            }
            uint animePointer = InputFormRef.BaseAddress + (InputFormRef.BlockSize * (uint)AddressList.SelectedIndex);
            string error = "";

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                error = ImageUtilMapActionAnimation.Import(filename, animePointer);
            }

            if (error != "")
            {
                return error;
            }

            U.ReSelectList(this.AddressList);
            //書き込み通知
            InputFormRef.ShowWriteNotifyAnimation(this, 0);

            return "";
        }


        private void X_N_JumpEditor_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            uint ID = (uint)AddressList.SelectedIndex;

            string filehint = AddressList.Text;

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            //テンポラリディレクトリを利用する
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                string filename = Path.Combine(tempdir.Dir, "anime.txt");
                uint addr = U.toOffset((uint)P0.Value);
                ImageUtilMapActionAnimation.Export(filename, addr , this.Comment.Text);
                if (!File.Exists(filename))
                {
                    R.ShowStopError("アニメーションエディタを表示するために、アニメーションをエクスポートしようとしましたが、アニメをファイルにエクスポートできませんでした。\r\n\r\nファイル:{0}",filename);
                    return;
                }

                ToolAnimationCreatorForm f = (ToolAnimationCreatorForm)InputFormRef.JumpFormLow<ToolAnimationCreatorForm>();
                f.Init(ToolAnimationCreatorUserControl.AnimationTypeEnum.MapActionAnimation
                    , ID, filehint, filename);
                f.Show();
            }
        }

        private void ImageMapActionAnimationForm_Load(object sender, EventArgs e)
        {
            uint AnimeP = FindAnimationPointer();
            if (AnimeP == U.NOT_FOUND)
            {
                R.ShowStopError("攻撃モーションの、アニメーションテーブルを取得できません。\r\nパッチがインストールされていない可能性があります。");
                this.Close();
                return;
            }
        }
    }
}
