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
            this.InputFormRef.MakeGeneralAddressListContextMenu(false);
            this.InputFormRef.CheckProtectionPaddingALIGN4 = false;

            ShowZoomComboBox.SelectedIndex = 0;
            U.SetIcon(AnimationInportButton, Properties.Resources.icon_upload);
            U.SetIcon(AnimationExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ExportAllButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportAllButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });

            U.AllowDropFilename(this, new string[] { ".TXT" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    AnimationImportButton_Click(null, null);
                }
            });
        }

        uint TextBaseAddress;
        uint IconBaseAddress;
        uint AnimeBaseAddress;

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, uint textPointer)
        {
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
                    return U.ToHexString(i) + " " + GetSkillName((uint)i);
                }
            );
            return ifr;
        }



        private void SkillConfigSkillSystemForm_Load(object sender, EventArgs e)
        {

        }

        public static void ClearCache()
        {
            g_Cache_FindAssignPersonalSkillPointer = PatchUtil.NO_CACHE;
            g_Cache_FindAssignClassSkillPointer = PatchUtil.NO_CACHE;
            g_Cache_FindAssignClassLevelUpSkillPointer = PatchUtil.NO_CACHE;
            g_Cache_FindAssignUnitLevelUpSkillPointer = PatchUtil.NO_CACHE;
            g_Cache_FindIconPointer = PatchUtil.NO_CACHE;
            g_Cache_FindTextPointer = PatchUtil.NO_CACHE;
            g_Cache_FindAnimePointer = PatchUtil.NO_CACHE;
            g_Cache_SkillName = new Dictionary<uint,string>();
        }

        static uint g_Cache_FindAssignPersonalSkillPointer = PatchUtil.NO_CACHE;
        public static uint FindAssignPersonalSkillPointer()
        {
            if (g_Cache_FindAssignPersonalSkillPointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FindAssignPersonalSkillPointer = FindSkillPointer("ASSIGN", 0);
            }
            return g_Cache_FindAssignPersonalSkillPointer;
        }

        static uint g_Cache_FindAssignClassSkillPointer = PatchUtil.NO_CACHE;
        public static uint FindAssignClassSkillPointer()
        {
            if (g_Cache_FindAssignClassSkillPointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FindAssignClassSkillPointer = FindSkillPointer("ASSIGN", 4);
            }
            return g_Cache_FindAssignClassSkillPointer;
        }

        static uint g_Cache_FindAssignClassLevelUpSkillPointer = PatchUtil.NO_CACHE;
        public static uint FindAssignClassLevelUpSkillPointer()
        {
            if (g_Cache_FindAssignClassLevelUpSkillPointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FindAssignClassLevelUpSkillPointer = FindSkillPointer("LEVELUP", 0);
            }
            return g_Cache_FindAssignClassLevelUpSkillPointer;
        }

        static uint g_Cache_FindAssignUnitLevelUpSkillPointer = PatchUtil.NO_CACHE;
        public static uint FindAssignUnitLevelUpSkillPointer()
        {
            if (g_Cache_FindAssignUnitLevelUpSkillPointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FindAssignUnitLevelUpSkillPointer = FindSkillPointer("LEVELUP", 4);
            }
            return g_Cache_FindAssignUnitLevelUpSkillPointer;
        }

        static uint g_Cache_FindIconPointer = PatchUtil.NO_CACHE;
        public static uint FindIconPointer()
        {
            if (g_Cache_FindIconPointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FindIconPointer = FindSkillPointer("ICON", 0);
            }
            return g_Cache_FindIconPointer;
        }

        static uint g_Cache_FindTextPointer = PatchUtil.NO_CACHE;
        public static uint FindTextPointer()
        {
            if (g_Cache_FindTextPointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FindTextPointer = FindSkillPointer("TEXT", 0);
            }
            return g_Cache_FindTextPointer;
        }

        static uint g_Cache_FindAnimePointer = PatchUtil.NO_CACHE;
        public static uint FindAnimePointer()
        {
            if (g_Cache_FindAnimePointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FindAnimePointer = FindSkillPointer("ANIME", 0);
            }
            return g_Cache_FindAnimePointer;
        }


        static Dictionary<uint, string> g_Cache_SkillName = new Dictionary<uint,string>();
        public static Dictionary<uint, string> LoadSkillNames()
        {
            if (g_Cache_SkillName.Count <= 0)
            {
                g_Cache_SkillName = U.LoadDicResource(U.ConfigDataFilename("skill_extends_skillsystem_name_"));
            }
            return g_Cache_SkillName;
        }

        public struct SkillSystemsTableSt
        {
            public string name;
            public uint skip;
            public byte[] data;
        };
        static uint FindSkillPointer(string type, uint skip)
        {
            SkillSystemsTableSt[] table = new SkillSystemsTableSt[] { 
                new SkillSystemsTableSt{ name="ASSIGN", skip =	16, data = new byte[]{0x01,0x35,0x02,0x36,0xF1,0xE7,0x00,0x20,0x28,0x70,0x29,0x1C,0x02,0x48,0x09,0x1A}},
                new SkillSystemsTableSt{ name="LEVELUP", skip =	16 + 8, data = new byte[]{0x01,0x35,0x02,0x36,0xF1,0xE7,0x00,0x20,0x28,0x70,0x29,0x1C,0x02,0x48,0x09,0x1A}},
                new SkillSystemsTableSt{ name="ASSIGN", skip =	16, data = new byte[]{0x29,0x1C,0xFF,0xF7,0xFA,0xFF,0x01,0x1C,0x08,0x78,0x00,0x28,0xEF,0xD0,0x01,0x31,0xFA,0xE7,0x00,0x00}},
                new SkillSystemsTableSt{ name="LEVELUP", skip =	0, data = new byte[]{0x0A,0xD0,0x1A,0x78,0x00,0x2A,0x07,0xD0,0x8A,0x42,0x01,0xD0,0x02,0x33,0xF8,0xE7,0x5A,0x78,0x22,0x70,0x01,0x34,0xF9,0xE7,0x00,0x20,0x20,0x70,0x31,0xBC,0x70,0x47}},
                new SkillSystemsTableSt{ name="ANIME", skip =	32, data = new byte[]{0x00,0x2B,0x00,0xD1,0x06,0x4B,0x38,0x1C,0x9E,0x46,0x00,0xF8,0x05,0x48,0x00,0x47}},
                new SkillSystemsTableSt{ name="ICON", skip =	24, data = new byte[]{0x02,0x40,0x09,0x4C,0x05,0x48,0x00,0x47,0x05,0x48,0x00,0x47,0x05,0x48,0x00,0x47}},
                new SkillSystemsTableSt{ name="ICON", skip =	8, data = new byte[]{0x08,0x42,0x04,0xD1,0x12,0x79,0xAA,0x42,0x01,0xD1,0x01,0x20,0x03,0xE0,0x01,0x34,0xBF,0x2C,0xEA,0xDD,0x00,0x20,0x30,0xBC,0x02,0xBC,0x08,0x47}},
                new SkillSystemsTableSt{ name="ICON", skip =	0, data = new byte[]{0xFF,0xFF,0x0F,0x00,0x05,0x49,0x00,0x29,0x01,0xD1,0x03,0x49,0x09,0x68,0x00,0x06,0x40,0x0C,0x40,0x18,0x70,0x47,0x00,0x00,0x88,0x37,0x00,0x08}},
                new SkillSystemsTableSt{ name="TEXT", skip =	16, data = new byte[]{0x07,0x49,0x40,0x00,0x40,0x18,0x00,0x88,0x00,0x28,0x00,0xD1,0x06,0x48,0x21,0x1C}},
                new SkillSystemsTableSt{ name="LEVELUP", skip =	0, data = new byte[]{0xE4,0x58,0x00,0x2C,0x0C,0xD0,0x23,0x78,0x00,0x2B,0x09,0xD0,0x8B,0x42,0x01,0xDD,0x93,0x42,0x01,0xDD,0x02,0x34,0xF6,0xE7,0x63,0x78,0x33,0x70,0x01,0x36,0xF9,0xE7,0x00,0x20,0x30,0x70,0x71,0xBC,0x70,0x47}},
                new SkillSystemsTableSt{ name="LEVELUP", skip =	0, data = new byte[]{0x06,0xDD,0x14,0x3B,0x8B,0x42,0x01,0xDD,0x93,0x42,0x01,0xDD,0x02,0x34,0xEF,0xE7,0x63,0x78,0x33,0x70,0x01,0x36,0xF9,0xE7,0x0B,0x4C,0x43,0x68,0x1B,0x79,0x9B,0x00,0xE4,0x58,0x00,0x2C,0x0C,0xD0,0x23,0x78,0x00,0x2B,0x09,0xD0,0x8B,0x42,0x01,0xDD,0x93,0x42,0x01,0xDD,0x02,0x34,0xF6,0xE7,0x63,0x78,0x33,0x70,0x01,0x36,0xF9,0xE7,0x00,0x20,0x30,0x70,0x71,0xBC,0x70,0x47}},
                new SkillSystemsTableSt{ name="LEVELUP", skip =	0, data = new byte[]{0xF0,0xE7,0x02,0x2B,0x12,0xD0,0x03,0x2B,0x06,0xD1,0x0D,0x48,0x42,0x21,0x41,0x5C,0x20,0x22,0x11,0x42,0x0A,0xD1,0xE5,0xE7,0x04,0x2B,0x06,0xD1,0x08,0x48,0x14,0x21,0x41,0x5C,0x40,0x22,0x11,0x42,0x01,0xD1,0xDC,0xE7,0xDB,0xE7,0x63,0x78,0x33,0x70,0x01,0x36,0xD7,0xE7,0x00,0x20,0x30,0x70,0x06,0xBC,0xF1,0xBC,0x70,0x47,0x00,0x00,0xF0,0xBC,0x02,0x02}},
        };

            foreach (SkillSystemsTableSt t in table)
            {
                if (t.name != type)
                {
                    continue;
                }

                //チェック開始アドレス
                uint start = 0xB00000;
                uint found = U.Grep(Program.ROM.Data, t.data, start, 0, 4);
                if (found == U.NOT_FOUND)
                {
                    continue;
                }
                uint a = (uint)(found + t.data.Length + t.skip);
                a = a + skip;
                if (!U.isSafetyOffset(a))
                {
                    continue;
                }
                uint p = Program.ROM.u32(a);
                if (!U.isSafetyPointer(p))
                {
                    continue;
                }
                //ポインタで返す.
                return a;
            }
            return U.NOT_FOUND;
        }

        const uint SkillPalettePointer = 0x22370; //オリジナルROMからあるパレット.
        public static uint GetIconAddr(uint index, uint imageBaseAddress)
        {
            const uint size = 16 * 16 / 2;
            uint imageOffset = imageBaseAddress + (size * index);
            return imageOffset;
        }
        public static Bitmap DrawIcon(uint index,uint imageBaseAddress)
        {
            uint imageOffset = GetIconAddr(index, imageBaseAddress);
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
        public static string GetSkillName(uint index)
        {
            string text = GetSkillText(index);
            if (text != "")
            {
                string name = SkillTextToName(text);
                if (name != "")
                {
                    return name;
                }
            }

            Dictionary<uint,string> skills = LoadSkillNames();
            return U.at(skills,index);
        }
        public static string GetSkillText(uint index)
        {
            if (index == 0)
            {
                return "";
            }
            if (index == 0xFF)
            {
                return "";
            }

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
        static string SkillTextToName(string name)
        {
            int i = name.IndexOf(':');
            if (i < 0)
            {
                return "";
            }
            return name.Substring(0,i).Trim();
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
            this.IconAddr.Value = GetIconAddr((uint)AddressList.SelectedIndex, this.IconBaseAddress);

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
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
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
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return;
            }

            InputFormRef ifr;
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
            ifr = Init(null, basetextP);
            uint skill_addr = ifr.BaseAddress;
            for (uint i = 0; i < ifr.DataCount; i++, skill_addr += ifr.BlockSize)
            {
                uint name = Program.ROM.u16(skill_addr + 0);
                if (name == 0 || name == 0xFFFF)
                {
                    continue;
                }
                FELint.CheckText(name, "DETAIL3", errors, FELint.Type.SKILL_CONFIG, skill_addr, i);
            }

            uint anime = Program.ROM.p32(baseanimeP);
            for (uint i = 0; i < ifr.DataCount; i++, anime += 4)
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
                ImageUtilSkillSystemsAnimeCreator.MakeCheckError(errors, addr, i);
            }
        }
        public static void ExportAllData(string filename)
        {
            InputFormRef InputFormRef;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
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
        public static bool ImportAllData(string filename,bool recycleConvertSkillTextID)
        {
            InputFormRef InputFormRef;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return false;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData("ImportAllData");

            string basedir = Path.GetDirectoryName(filename);
            string[] lines = File.ReadAllLines(filename);
            {
                uint baseiconP = FindIconPointer();
                uint basetextP = FindTextPointer();
                uint baseanimeP = FindAnimePointer();

                if (baseiconP == U.NOT_FOUND)
                {
                    return false;
                }
                if (basetextP == U.NOT_FOUND)
                {
                    return false;
                }
                if (baseanimeP == U.NOT_FOUND)
                {
                    return false;
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
                    if (textid != 0)
                    {
                        if (recycleConvertSkillTextID)
                        {
                            textid = ConvertSkillTextIDWithRecrycle(i, textid);
                        }

                        Program.ROM.write_u16(textAddr + 0, textid, undodata);
                    }

                    uint animePAddr = U.atoh(sp[1]);
                    if (animePAddr == 0)
                    {//0の値が設定されている場合は、アニメ未指定になっているので0を書き込みます.
                        Program.ROM.write_p32(anime, 0, undodata);
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
            Program.Undo.Push(undodata);
            return true;
        }

        static uint ConvertSkillTextIDWithRecrycle(uint skillid, uint textID)
        {
            //Vengeance
            if (skillid == 0x90 && textID == 0xEB1)
            {
                return 0xF72;
            }

            //Imbue
            if (skillid == 0x91 && textID == 0xEB0)
            {
                return 0xF61;
            }

            //DoubleLion
            if (skillid == 0x97 && textID == 0xEB9)
            {
                return 0xF4C;
            }

            //Shade        
            if (skillid == 0x0C && textID == 0xE2B)
            {
                return 0xF5F;
            }

            //Glacies      
            if (skillid == 0x32 && textID == 0xE52)
            {
                return 0xF69;
            }

            //GreatShild 
            if (skillid == 0x6D && textID == 0xE8D)
            {
                return 0xF70;
            }

            //SkyBreaker   
            if (skillid == 0x92 && textID == 0xEAF)
            {
                return 0xF54;
            }

            //BlueFlame  
            if (skillid == 0x93 && textID == 0xEB2)
            {
                return 0xF4B;
            }

            //Gridmaster   
            if (skillid == 0xF8 && textID == 0xF41)
            {
                return 0xF6A;
            }

            //Assassinate  
            if (skillid == 0xF9 && textID == 0xF42)
            {
                return 0xF48;
            }
            
            //Corrosion  
            if ( skillid == 0x7D && textID == 0xE9C)
            {
                return 0xF64;
            }

            //ArcaneBalde 
            if (skillid == 0x7E && textID == 0xE9D)
            {
                return 0xF49;
            }

            //KeepUp       
            if (skillid == 0x79 && textID == 0xE78)
            {
                return 0xF66;
            }
            //KeepUp       
            if (skillid == 0x79 && textID == 0xE9E)
            {
                return 0xF66;
            }

            //NatureRush   
            if ( skillid == 0xDA && textID == 0xF23)
            {
                return 0xF67;
            }

            //Resourcefull 
            if (skillid == 0x58 && textID == 0xE78)
            {
                return 0xF55;
            }

            //Rightful Arch (元に戻す)
            if (skillid == 0x7F && textID == 0xF65)
            {
                return 0xE9E;
            }
            return textID;
        }

        //テキストの取得
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef ifr;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
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
                UseValsID.AppendTextID(list, FELint.Type.SKILL_CONFIG, ifr, new uint[] { 0 });
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
            string filter = R._("スキルアニメスクリプト|*.txt|アニメGIF|*.gif|Dump All|*.txt|All files|*");

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

            if (save.FilterIndex == 2)
            {//GIF
                ImageUtilSkillSystemsAnimeCreator.ExportGif(filename, (uint)ANIMATION.Value);
            }
            else if (save.FilterIndex == 3)
            {//All
                ImageUtilSkillSystemsAnimeCreator.Export(filename, (uint)ANIMATION.Value);
                filename = U.ChangeExtFilename(filename, ".gif");
                ImageUtilSkillSystemsAnimeCreator.ExportGif(filename, (uint)ANIMATION.Value);
            }
            else
            {//Script
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

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {

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
                filename = open.FileNames[0];
                Program.LastSelectedFilename.Save(this, "", open);
            }

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
            if (!U.isSafetyPointer(Program.ROM.u32(0x16740)))
            {//おそらく WeaponLockExはインストールされていない
                return;
            }
            //ShadowGiftを消して、WeaponLockExのフックを復活させる.
            Program.ROM.write_range(0x16738, new byte[] { 0x28, 0x30, 0x40, 0x18, 0x00, 0x4B, 0x18, 0x47 });
        }
        public static void Export(StringBuilder sb, bool isColorzCore)
        {
            uint iconP = FindIconPointer();

            if (iconP == U.NOT_FOUND)
            {
                return;
            }
            uint iconBaseAddress = Program.ROM.p32(iconP);
            ExportFunction.One(sb, "SkillIcons", iconBaseAddress, isColorzCore);
        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("SkillConfig|*.SkillConfig.tsv|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", save, "");

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                ExportAllData(save.FileName);
            }
        }

        private void ImportAllButton_Click(object sender, EventArgs e)
        {
            string title = R._("読込むファイル名を選択してください");
            string filter = R._("SkillConfig|*.SkillConfig.tsv|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;

            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (open.FileNames.Length <= 0 || !U.CanReadFileRetry(open.FileNames[0]))
            {
                return;
            }
            string filename = open.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", open);

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                bool r = ImportAllData(filename, recycleConvertSkillTextID: false);
                if (!r)
                {
                    R.ShowStopError("インポートに失敗しました。");
                    return;
                }
            }
            U.ReSelectList(this.AddressList);
            R.ShowOK("データのインポートが完了しました。");
        }

        public static void ExportExtraMenu(string filename)
        {
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            uint addr;
            uint[] menus = { 
                               0x4F, 0xF00, //全預け+ Give All+
                               0x63, 0xF75, //特殊
                               0x6B, 0x6BF, //待機
                           };
            for (int i = 0; i < menus.Length; i += 2)
            {
                uint menuid = menus[i+0];
                uint helpid = menus[i+1];

                addr = MenuCommandForm.SearchMenuUnitOrGame(menuid, helpid);
                if (addr != U.NOT_FOUND)
                {
                    byte[] bin = Program.ROM.getBinaryData(addr, MenuCommandForm.MENU_SIZE);
                    sb.AppendLine(U.HexsToString(bin));
                }
            }
            File.WriteAllText(filename, sb.ToString());
        }
        public static void ImportExtraMenu(string filename)
        {
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return;
            }
            if (! File.Exists(filename))
            {
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData("ImportExtraMenu");
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                byte[] bin = U.StringToHexs(line);
                if (bin.Length < MenuCommandForm.MENU_SIZE)
                {
                    continue;
                }
                uint menuID = U.u8(bin, 0x9);
                uint helpID = U.u16(bin, 0x6);
                uint addr = MenuCommandForm.SearchMenuUnitOrGame(menuID, helpID);
                if (addr != U.NOT_FOUND)
                {
                    if (menuID == 0x6B && helpID == 0x6BF)
                    {//wait
                        Program.ROM.write_range(addr, bin, undodata);
                    }
                    continue;
                }
                //空き部分に追加.
                addr = MenuCommandForm.SearchMenuUnitOrGame(0x0, 0x0);
                if (addr == U.NOT_FOUND)
                {//空きがない
                    continue;
                }
                Program.ROM.write_range(addr, bin, undodata);
            }
            Program.Undo.Push(undodata);

            CopySubMenu();
        }
        static void CopySubMenu()
        {
            uint[] menus = { 
                    0x4E, 0xf07,
                    0x4E, 0xf09,
                    0x4E, 0xE12,
                    0x4E, 0xE0C,
                    0x4E, 0xE0E,
                    0x4E, 0xE10,
                    0x4E, 0xEDB,
                    0x4E, 0xE0A,
                    0x4E, 0xEE0,
                    0x4E, 0xE14,
                    0x4E, 0xE16,
                    0x4E, 0xE18,
                    0x4E, 0xE1A,
                    0x4E, 0xE1C,
                    0x4E, 0xF77,
            };
            Undo.UndoData undodata = Program.Undo.NewUndoData("CopySubMenu");
            for (int i = 0; i < menus.Length; i += 2)
            {
                uint menuid = menus[i + 0];
                uint helpid = menus[i + 1];

                uint dest = MenuCommandForm.SearchMenu("DEBUG1", menuid, helpid);
                if (dest == U.NOT_FOUND)
                {
                    continue;
                }

                uint src = MenuCommandForm.SearchMenuUnitOrGame(menuid, helpid);
                if (src == U.NOT_FOUND)
                {
                    continue;
                }

                byte[] bin = Program.ROM.getBinaryData(src, MenuCommandForm.MENU_SIZE);
                Program.ROM.write_range(dest, bin, undodata);
            }
            Program.Undo.Push(undodata);
        }
    }
}
