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
    public partial class SkillConfigSkillSystemForm : Form
    {
        public SkillConfigSkillSystemForm()
        {
            InitializeComponent();

            uint iconP = FindIconPointer();
            uint textP = FindTextPointer();
            uint animeP = FindAnimePointer();

            if (iconP == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、アイコンを取得できません。");
                return;
            }
            if (textP == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、テキストを取得できません。");
                return;
            }
            if (animeP == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、アニメを取得できません。");
                return;
            }
            this.TextBaseAddress = Program.ROM.p32(textP);
            this.IconBaseAddress = Program.ROM.p32(iconP);
            this.AnimeBaseAddress = Program.ROM.p32(animeP);

            this.AddressList.OwnerDraw(DrawSkillAndText, DrawMode.OwnerDrawFixed);
            InputFormRef = Init(this, textP);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            ShowZoomComboBox.SelectedIndex = 0;
            U.SetIcon(AnimationInportButton, U.GetShell32Icon(45));
            U.SetIcon(AnimationExportButton, U.GetShell32Icon(122));
        }

        uint TextBaseAddress;
        uint IconBaseAddress;
        uint AnimeBaseAddress;

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, uint textPointer)
        {
            Dictionary<uint, string> names = U.LoadDicResource(U.ConfigDataFilename("skill_extends_skillsystem_name_"));

            InputFormRef ifr = new InputFormRef(self
                , ""
                , textPointer
                , 2
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < 255;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + U.at(names,(uint)i);
                }
            );
            return ifr;
        }



        private void SkillConfigSkillSystemForm_Load(object sender, EventArgs e)
        {

        }

        public static uint FindAssignPersonalSkillPointer()
        {
            return FindSkillPointerToPointer("ASSIGN", 0);
        }
        public static uint FindAssignClassSkillPointer()
        {
            return FindSkillPointerToPointer("ASSIGN", 4);
        }
        public static uint FindAssignClassLevelUpSkillPointer()
        {
            return FindSkillPointerToPointer("ASSIGN", 8);
        }
        public static uint FindIconPointer()
        {
            uint r = FindSkillPointerToPointer("ICON", 0);
            if (r == U.NOT_FOUND)
            {
                r = FindSkillPointerToPointer("ICON2", 0);
            }
            return r;
        }
        public static uint FindTextPointer()
        {
            return FindSkillPointerToPointer("TEXT", 0);
        }
        public static uint FindAnimePointer()
        {
            return FindSkillPointerToPointer("ANIME", 0);
        }

        static uint FindSkillPointerToPointer(string type,uint skip = 0)
        {
            uint a = FindSkillPointer(type);
            if (a == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            uint p = Program.ROM.u32(a + skip);
            if (!U.isSafetyPointer(p))
            {
                return U.NOT_FOUND;
            }
            //ポインタで返す.
            return a + skip;
        }

        static uint FindSkillPointer(string type)
        {
            string filename = U.ConfigDataFilename("skill_extends_skillsystem_parse_");
            string[] lines = File.ReadAllLines(filename);
            string version = Program.ROM.VersionToFilename();
            for (int i = 0; i < lines.Length; i++)
            {
                if (U.IsComment(lines[i]))
                {
                    continue;
                }
                string line = U.ClipComment(lines[i]);
                string[] sp = line.Split('\t');
                if (sp.Length < 3)
                {
                    continue;
                }
                if (sp[0] != type)
                {
                    continue;
                }

                string[] hexStrings = sp[2].Split(' ');
                byte[] need = new byte[hexStrings.Length];
                for (int n = 0; n < hexStrings.Length; n++)
                {
                    need[n] = (byte)U.atoh(hexStrings[n]);
                }

                //チェック開始アドレス
                uint start = 0xB00000;

                uint found = U.Grep(Program.ROM.Data, need, start, 0, 4);
                if (found == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }
                uint skip = U.atoi(sp[1]);
                return (uint)(found + need.Length + skip);
            }
            return U.NOT_FOUND;
        }

        const uint SkillPalettePointer = 0x22370; //オリジナルROMからあるパレット.
        public static Bitmap DrawIcon(uint index,uint imageBaseAddress)
        {
            uint size = 16 * 16 / 2;
            uint imageOffset = imageBaseAddress + (size * index);
            uint palette = Program.ROM.p32(SkillPalettePointer); //オリジナルROMからあるパレット.
            return 
                ImageUtil.ByteToImage16Tile(16, 16, Program.ROM.Data, (int)imageOffset, Program.ROM.Data, (int)palette);
        }
        public static string GetSkillText(uint index, uint textBaseAddress)
        {
            uint size = 2;
            uint textOffset = textBaseAddress + (size * index);
            uint id = Program.ROM.u16(textOffset);

            return TextForm.Direct(id);
        }
        public static string GetSkillText(uint index)
        {
            uint basetextP = FindTextPointer();
            if (!U.isSafetyOffset(basetextP))
            {
                return "";
            }
            uint p = Program.ROM.p32(basetextP);
            if (!U.isSafetyOffset(p))
            {
                return "";
            }
            return GetSkillText(index, p);
        }
        public static Bitmap DrawSkillIcon(uint index)
        {
            uint iconP = FindIconPointer();
            if (!U.isSafetyOffset(iconP))
            {
                return ImageUtil.Blank(16,16);
            }
            uint p = Program.ROM.p32(iconP);
            if (!U.isSafetyOffset(p))
            {
                return ImageUtil.Blank(16, 16);
            }
            return DrawIcon(index, p);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SKILLICON.Image = DrawIcon((uint)AddressList.SelectedIndex, this.IconBaseAddress);

            uint anime = this.AnimeBaseAddress + (4 * (uint)AddressList.SelectedIndex);
            uint a = Program.ROM.p32(anime);
            ANIMATION.Value = a;
            if (U.isSafetyOffset(a))
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

        private void WriteButton_Click(object sender, EventArgs e)
        {
            //スキルアニメポインタを追加書き込み. //別テーブルでアドレスが違う.
            uint anime = this.AnimeBaseAddress + (4 * (uint)AddressList.SelectedIndex);
            Program.Undo.Push("SKILL ANIME P", anime, 4);
            Program.ROM.write_p32( anime,(uint)ANIMATION.Value);
        }

        //Skill + テキストを書くルーチン
        Size DrawSkillAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            Bitmap bitmap = SkillConfigSkillSystemForm.DrawIcon((uint)index, this.IconBaseAddress);
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

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawIcon((uint)AddressList.SelectedIndex, this.IconBaseAddress);
            ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 2 * 8;
            int height = 2 * 8;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                return;
            }

            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , Program.ROM.p32(SkillPalettePointer)
                        , U.NOT_FOUND
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, Program.ROM.p32(SkillPalettePointer)));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, Program.ROM.p32(SkillPalettePointer)));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            uint index = (uint)this.AddressList.SelectedIndex;
            uint size = 16 * 16 / 2;
            uint addr = this.IconBaseAddress + (size * index);

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            Program.ROM.write_range(U.toOffset(addr), image, undodata);
            Program.Undo.Push(undodata);

            InputFormRef.ReloadAddressList();
            InputFormRef.ShowWriteNotifyAnimation(this, addr);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            InputFormRef InputFormRef;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            {
                uint baseiconP = FindIconPointer();
                uint basetextP = FindTextPointer();
                uint baseanimeP = FindAnimePointer();

                if (baseiconP == U.NOT_FOUND)
                {
                    return;
                }
                if (basetextP == U.NOT_FOUND)
                {
                    return;
                }
                if (baseanimeP == U.NOT_FOUND)
                {
                    return;
                }
                InputFormRef = Init(null, basetextP);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "SkillConfigSkillSystem", new uint[] { });

                uint anime = Program.ROM.p32(baseanimeP);
                for (uint i = 0; i < InputFormRef.DataCount; i++ , anime += 4)
                {
                    if (!U.isSafetyOffset(anime))
                    {
                        break;
                    }
                    uint addr = Program.ROM.p32(anime);
                    if (!U.isSafetyOffset(addr))
                    {
                        continue;
                    }
                    string name = "SkillAnime:" + U.To0xHexString(i) + " ";
                    FEBuilderGBA.Address.AddAddress(list, addr, 0, anime, name, FEBuilderGBA.Address.DataTypeEnum.POINTER);

                    ImageUtilSkillSystemsAnimeCreator.RecycleOldAnime(ref list
                        ,name
                        ,isPointerOnly
                        ,addr);
                }
            }
        }
        public static void ExportAllData(string filename)
        {
            InputFormRef InputFormRef;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            string basedir = Path.GetDirectoryName(filename);
            List<string> lines = new List<string>();
            {
                uint baseiconP = FindIconPointer();
                uint basetextP = FindTextPointer();
                uint baseanimeP = FindAnimePointer();

                if (baseiconP == U.NOT_FOUND)
                {
                    return;
                }
                if (basetextP == U.NOT_FOUND)
                {
                    return;
                }
                if (baseanimeP == U.NOT_FOUND)
                {
                    return;
                }
                InputFormRef = Init(null, basetextP);
                uint textAddr = InputFormRef.BaseAddress;

                uint anime = Program.ROM.p32(baseanimeP);
                for (uint i = 0; i < InputFormRef.DataCount; 
                    i++, anime += 4, textAddr += 2)
                {
                    if (!U.isSafetyOffset(anime))
                    {
                        break;
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append(U.ToHexString(Program.ROM.u16(textAddr + 0)));

                    uint addr = Program.ROM.p32(anime);
                    sb.Append("\t");
                    sb.Append(U.ToHexString(addr));
                    lines.Add(sb.ToString());

                    if (!U.isExtrendsROMArea(addr) || addr == 0)
                    {//既存領域内なので新しいアニメで上書きする.
                        continue;
                    }

                    //拡張領域のアニメはexportしないといけないようだ.
                    //内部に非拡張領域のデータを使いまわしていることがあるため
                    string animedir = Path.Combine(basedir, "anime" + U.ToHexString(i));
                    U.mkdir(animedir);
                    string anime_filename = Path.Combine(animedir, "anime.txt");
                    ImageUtilSkillSystemsAnimeCreator.Export(anime_filename, addr);
                }
            }
            File.WriteAllLines(filename, lines);
        }
        public static void ImportAllData(string filename)
        {
            InputFormRef InputFormRef;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            string basedir = Path.GetDirectoryName(filename);
            string[] lines = File.ReadAllLines(filename);
            {
                uint baseiconP = FindIconPointer();
                uint basetextP = FindTextPointer();
                uint baseanimeP = FindAnimePointer();

                if (baseiconP == U.NOT_FOUND)
                {
                    return;
                }
                if (basetextP == U.NOT_FOUND)
                {
                    return;
                }
                if (baseanimeP == U.NOT_FOUND)
                {
                    return;
                }
                InputFormRef = Init(null, basetextP);
                uint textAddr = InputFormRef.BaseAddress;

                uint anime = Program.ROM.p32(baseanimeP);
                for (uint i = 0; i < InputFormRef.DataCount;
                    i++, anime += 4, textAddr += 2)
                {
                    if (!U.isSafetyOffset(anime))
                    {
                        break;
                    }
                    if (i >= lines.Length)
                    {
                        break;
                    }
                    string[] sp = lines[i].Split('\t');
                    if (sp.Length < 2)
                    {
                        continue;
                    }
                    uint textid = U.atoh(sp[0]);
                    Program.ROM.write_u16(textAddr + 0, textid);

                    uint animePAddr = U.atoh(sp[1]);
                    if (animePAddr == 0)
                    {//0の値が設定されている場合は、アニメ未指定になっているので0を書き込みます.
                        Program.ROM.write_p32(anime, 0);
                        continue;
                    }
                    string anime_filename = Path.Combine(basedir, "anime" + U.ToHexString(i), "anime.txt");
                    if (File.Exists(anime_filename))
                    {//インポートしなおす
                        ImageUtilSkillSystemsAnimeCreator.Import(anime_filename, anime);
                    }
                    //それ以外の値の場合、ディフォルト設定だとして、最新の値を採用します.
                }
            }
        }

        //テキストの取得
        public static void MakeTextIDArray(List<TextID> list)
        {
            InputFormRef ifr;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            {
                uint basetextP = FindTextPointer();
                if (basetextP == U.NOT_FOUND)
                {
                    return;
                }
                ifr = Init(null, basetextP);
                TextID.AppendTextID(list, FELint.Type.SKILL_CONFIG, ifr, new uint[] { 0 });
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
            uint anime = (uint)ANIMATION.Value;
            uint frame = (uint)ShowFrameUpDown.Value;
            AnimationPictureBox.Image = ImageUtilSkillSystemsAnimeCreator.Draw(anime, frame);
        }

        private void AnimationExportButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("スキルアニメスクリプト|*.txt|アニメGIF|*.gif|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, "skill_" + this.AddressList.Text.Trim());

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

            string ext = U.GetFilenameExt(save.FileName);
            if (ext == ".GIF")
            {
                ImageUtilSkillSystemsAnimeCreator.ExportGif(filename, (uint)ANIMATION.Value);
            }
            else
            {
                ImageUtilSkillSystemsAnimeCreator.Export(filename, (uint)ANIMATION.Value);
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

            string title = R._("開くファイル名を選択してください");
            string filter = R._("スキルアニメスクリプト|*.txt|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            open.FileName = "skill_" + this.AddressList.Text.Trim();
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }
            string filename = open.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", open);

            uint id = (uint)this.AddressList.SelectedIndex;
            string error = SkillAnimeImportDirect(id,filename);

            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }
        }

        public string SkillAnimeImportDirect(uint id, string filename)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return R._("現在他の処理中です");
            }

            if (id <= 0)
            {
                return R._("指定されたID({0})は存在しません。", U.To0xHexString(id));
            }
            uint animePointer = this.AnimeBaseAddress + (4 * id);
            
            string error = "";

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                error = ImageUtilSkillSystemsAnimeCreator.Import(filename, animePointer);
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
                ImageUtilSkillSystemsAnimeCreator.Export(filename, (uint)ANIMATION.Value);
                if (!File.Exists(filename))
                {
                    R.ShowStopError("アニメーションエディタを表示するために、アニメーションをエクスポートしようとしましたが、アニメをファイルにエクスポートできませんでした。\r\n\r\nファイル:{0}",filename);
                    return;
                }

                ToolAnimationCreatorForm f = (ToolAnimationCreatorForm)InputFormRef.JumpFormLow<ToolAnimationCreatorForm>();
                f.Init(ToolAnimationCreatorUserControl.AnimationTypeEnum.Skill
                    , ID, filehint, filename);
                f.Show();
            }
        }

        //ShadowGiftがWeaponLockExを壊す問題を修正する
        public static void FixWeaponLockEx()
        {
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            if (!U.isPointer(Program.ROM.u32(0x16740)))
            {//おそらく WeaponLockExはインストールされていない
                return;
            }
            //ShadowGiftを消して、WeaponLockExのフックを復活させる.
            Program.ROM.write_range(0x16738, new byte[] { 0x28, 0x30, 0x40, 0x18, 0x00, 0x4B, 0x18, 0x47 });
        }

    }
}
