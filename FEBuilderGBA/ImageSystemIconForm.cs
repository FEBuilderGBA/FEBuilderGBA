using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageSystemIconForm : Form
    {
        public ImageSystemIconForm()
        {
            InitializeComponent();
        }


        ImageFormRef system_icon;
        ImageFormRef system_move_allowicon;
        ImageFormRef system_weapon_icon_icon;
        ImageFormRef system_music_icon_icon;

        ImageFormRef systemmenu_goal;
        ImageFormRef systemmenu_terrain;
        ImageFormRef systemmenu_name;
        ImageFormRef systemmenu_battlepreview;


        ImageFormRef systemmenu_badstatus;
        ImageFormRef systemmenu_old_badstatus;


        static Size GetSystemIconImageSize()
        {
            Size size = new System.Drawing.Size(144, 32);

            uint width = Program.ROM.u8(Program.ROM.RomInfo.system_icon_width_address());
            if (width > 32)
            {
                width = 32;
            }
            else if (width < 0x12)
            {
                width = 0x12;
            }

            size.Width = (int)width * 8;
            return size;
        }

        private void SystemIconForm_Load(object sender, EventArgs e)
        {
            Size system_icon_size = GetSystemIconImageSize();

            system_icon = new ImageFormRef(this, "system_icon", system_icon_size.Width, system_icon_size.Height, 1, Program.ROM.RomInfo.system_icon_pointer(), 0, Program.ROM.RomInfo.system_icon_palette_pointer());
            system_move_allowicon = new ImageFormRef(this, "system_move_allowicon", 32 * 8, 2 * 8, 1, Program.ROM.RomInfo.system_move_allowicon_pointer(), 0, Program.ROM.RomInfo.system_move_allowicon_palette_pointer());
            system_weapon_icon_icon = new ImageFormRef(this, "system_weapon_icon_icon", 32 * 8, 32, 1, Program.ROM.RomInfo.system_weapon_icon_pointer(), 0, Program.ROM.RomInfo.system_weapon_icon_palette_pointer());
            system_music_icon_icon = new ImageFormRef(this, "system_music_icon_icon", 32 * 8, 32, 1, Program.ROM.RomInfo.system_music_icon_pointer(), 0, Program.ROM.RomInfo.system_music_icon_palette_pointer());

            if (Program.ROM.RomInfo.version() >= 7)
            {
                systemmenu_goal = new ImageFormRef(this, "systemmenu_goal", 256, 256, 4, Program.ROM.RomInfo.systemmenu_common_image_pointer(), Program.ROM.RomInfo.systemmenu_goal_tsa_pointer(), Program.ROM.RomInfo.systemmenu_common_palette_pointer());
                systemmenu_goal_panel.Show();
            }
            else
            {//FE6
                systemmenu_goal_panel.Hide();
                WMTabControl.TabPages.Remove(SystemIcon2tabPage);
            }
            WMTabControl.TabPages.Remove(tabPage3);

            if (Program.ROM.RomInfo.version() >= 8)
            {//FE8
                systemmenu_badstatus = new ImageFormRef(this, "systemmenu_badstatus", 40, 8 * 9, 1, Program.ROM.RomInfo.systemmenu_badstatus_image_pointer(), 0, Program.ROM.RomInfo.systemmenu_badstatus_palette_pointer());
                systemmenu_old_badstatus_panel.Hide();

                systemmenu_badstatus_panel.Height = systemmenu_badstatus_panel.Height * 2;
                systemmenu_badstatus_Picture.Height = systemmenu_badstatus_Picture.Height * 2;
                InputFormRef.markupJumpLabel(X_StatusBackgroundLink);
                InputFormRef.markupJumpLabel(X_SystemMenuPaletteLink);
            }
            else if (Program.ROM.RomInfo.version() >= 7)
            {//FE7
                systemmenu_badstatus = new ImageFormRef(this, "systemmenu_badstatus", 32, 8 * 4, 1, Program.ROM.RomInfo.systemmenu_badstatus_image_pointer(), 0, Program.ROM.RomInfo.systemmenu_badstatus_palette_pointer());
                systemmenu_old_badstatus = new ImageFormRef(this, "systemmenu_old_badstatus", 256, 32, 1, Program.ROM.RomInfo.systemmenu_badstatus_old_image_pointer(), 0, Program.ROM.RomInfo.systemmenu_badstatus_old_palette_pointer());
                X_StatusBackgroundLink.Hide();
                X_SystemMenuPaletteLink.Hide();
            }
            else
            {//FE6
                systemmenu_old_badstatus = new ImageFormRef(this, "systemmenu_old_badstatus", 256, 32, 1, Program.ROM.RomInfo.systemmenu_badstatus_old_image_pointer(), 0, Program.ROM.RomInfo.systemmenu_badstatus_old_palette_pointer());
                systemmenu_badstatus_panel.Hide();
                systemmenu_old_badstatus_panel.Location = systemmenu_badstatus_panel.Location;
                X_StatusBackgroundLink.Hide();
                X_SystemMenuPaletteLink.Hide();
            }

            systemmenu_terrain = new ImageFormRef(this, "systemmenu_terrain", 256, 256, 4, Program.ROM.RomInfo.systemmenu_common_image_pointer(), Program.ROM.RomInfo.systemmenu_terrain_tsa_pointer(), Program.ROM.RomInfo.systemmenu_common_palette_pointer());
            systemmenu_name = new ImageFormRef(this, "systemmenu_name", 256, 256, 4, Program.ROM.RomInfo.systemmenu_name_image_pointer(), Program.ROM.RomInfo.systemmenu_name_tsa_pointer(), Program.ROM.RomInfo.systemmenu_name_palette_pointer());
            systemmenu_battlepreview = new ImageFormRef(this, "systemmenu_battlepreview", 256, 256, 4, Program.ROM.RomInfo.systemmenu_battlepreview_image_pointer(), Program.ROM.RomInfo.systemmenu_battlepreview_tsa_pointer(), Program.ROM.RomInfo.systemmenu_battlepreview_palette_pointer());

            systemarea_move_gradation_palette.Value = Program.ROM.p32(Program.ROM.RomInfo.systemarea_move_gradation_palette_pointer());
            systemarea_attack_gradation_palette.Value = Program.ROM.p32(Program.ROM.RomInfo.systemarea_attack_gradation_palette_pointer());
            systemarea_staff_gradation_palette.Value = Program.ROM.p32(Program.ROM.RomInfo.systemarea_staff_gradation_palette_pointer());

            InputFormRef.markupJumpLabel(X_Jump_Patch);
            InputFormRef.markupJumpLabel(X_GraphicsTool);
            InputFormRef.markupJumpLabel(X_Internet);

            InputFormRef.markupJumpLabel(unit_icon_Label);
            InputFormRef.markupJumpLabel(unit_icon_enemy_Label);
            InputFormRef.markupJumpLabel(unit_icon_npc_Label);
            InputFormRef.markupJumpLabel(unit_icon_four_Label);
            InputFormRef.markupJumpLabel(unit_icon_gray_Label);
            InputFormRef.markupJumpLabel(icon_palette_Label);

            systemIconPictureBox1.Image = ImageSystemIconForm.Allows(8);
            systemIconPictureBox2.Image = ImageSystemIconForm.Fort();
            systemIconPictureBox3.Image = ImageSystemIconForm.Vendor();
            IconRedraw();
        }

        void IconRedraw()
        {
            unit_icon_Picture.Image = DrawUnits(0);
            unit_icon_enemy_Picture.Image = DrawUnits(2);
            unit_icon_npc_Picture.Image = DrawUnits(1);
            unit_icon_four_Picture.Image = DrawUnits(4);
            unit_icon_gray_Picture.Image = DrawUnits(3);
            item_icon_Picture.Image = DrawItemIcons();

            unit_icon_PALETTE.Value = Program.ROM.RomInfo.unit_icon_palette_address();
            unit_icon_enemy_PALETTE.Value = Program.ROM.RomInfo.unit_icon_enemey_palette_address();
            unit_icon_npc_PALETTE.Value = Program.ROM.RomInfo.unit_icon_npc_palette_address();
            unit_icon_four_PALETTE.Value = Program.ROM.RomInfo.unit_icon_four_palette_address();
            unit_icon_gray_PALETTE.Value = Program.ROM.RomInfo.unit_icon_gray_palette_address();
            item_icon_PALETTE.Value = Program.ROM.p32(Program.ROM.RomInfo.icon_palette_pointer());

            if (Program.ROM.RomInfo.version() >= 7)
            {
                unit_icon_lightrune_Picture.Image = DrawUnits2(5, Program.ROM.RomInfo.lightrune_uniticon_id());
                unit_icon_sepia_Picture.Image = DrawUnits(6);
                unit_icon_lightrune_PALETTE.Value = Program.ROM.RomInfo.unit_icon_lightrune_palette_address();
                unit_icon_sepia_PALETTE.Value = Program.ROM.RomInfo.unit_icon_sepia_palette_address();
            }
        }

        const int ICON_COUNT = 10;
        Bitmap DrawUnits(int palette_type)
        {
            Bitmap bitmap = null;
            for (int i = 0; i < ICON_COUNT; i++)
            {
                Bitmap a = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap((uint)(i + 1), palette_type, true);
                if (bitmap == null)
                {
                    bitmap = ImageUtil.Blank(16 * ICON_COUNT, 16, a);
                }
                if (ImageUtil.IsBlankBitmap(a))
                {
                    a = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap((uint)(i + 1) * 2, palette_type, true);
                }
                ImageUtil.BitBlt(bitmap, i * 16, 0, 16, 16, a, 0, 0);
            }
            return bitmap;
        }
        Bitmap DrawUnits2(int palette_type, uint icon_id)
        {
            Bitmap first = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap((uint)(icon_id), palette_type, true);
            Bitmap bitmap = ImageUtil.Blank(16 * ICON_COUNT, 16, first);
            ImageUtil.BitBlt(bitmap, 0, 0, 16, 16, first, 0, 0);

            for (int i = 1; i < ICON_COUNT; i++)
            {
                Bitmap a = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap((uint)(i), palette_type, true);
                if (bitmap == null)
                {
                    bitmap = ImageUtil.Blank(16 * ICON_COUNT, 16, a);
                }
                if (ImageUtil.IsBlankBitmap(a))
                {
                    a = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap((uint)(i) * 2, palette_type, true);
                }
                ImageUtil.BitBlt(bitmap, i * 16, 0, 16, 16, a, 0, 0);
            }
            return bitmap;
        }
        Bitmap DrawItemIcons()
        {
            Bitmap bitmap = null;
            for (int i = 0; i < ICON_COUNT; i++)
            {
                Bitmap a = ImageItemIconForm.DrawIconWhereID((uint)(i + 1)*10);
                if (bitmap == null)
                {
                    bitmap = ImageUtil.Blank(16 * ICON_COUNT, 16, a);
                }
                if (ImageUtil.IsBlankBitmap(a))
                {
                    a = ImageItemIconForm.DrawIconWhereID((uint)(i + 1) * 10 + 1);
                }
                ImageUtil.BitBlt(bitmap, i * 16, 0, 16, 16, a, 0, 0);
            }
            return bitmap;
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            ClearCache();
        }
        public static void ClearCache()
        {
            SystemIconCache = null;
            SystemMoveAllowCache = null;
            SystemMusicCache = null;
            SystemWeaponCache = null;
            MapFieldsCache = null;
            MapVillageCache = null;
        }

        //FE8では、バトルプレビューの画像のポインタを4箇所にコピーしないといけない.
        public static void Fix_FE8_systemmenu_battlepreview_image(uint imagePointer,Undo.UndoData undodata)
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                return;
            }
            uint p = Program.ROM.RomInfo.systemmenu_battlepreview_image_pointer();
            if (imagePointer != p)
            {
                return;
            }

            uint orig = Program.ROM.u32(p);

            Program.ROM.write_p32(p + 4,orig,undodata);
            Program.ROM.write_p32(p + 8, orig, undodata);
            Program.ROM.write_p32(p + 12, orig, undodata);
        }



        private void unit_icon_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawUnits(0);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "unit_icon"));
        }

        private void unit_icon_enemy_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawUnits(2);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "unit_icon_enemy"));
        }

        private void unit_icon_npc_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawUnits(1);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "unit_icon_npc"));
        }

        private void unit_icon_four_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawUnits(4);
            ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename(this, "unit_icon_four"));
        }
        private void unit_icon_gray_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawUnits(3);
            ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename(this, "unit_icon_gray"));
        }

        private void icon_palette_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawItemIcons();
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "item_icon"));
        }

        private void unit_icon_lightrune_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawUnits(5);
            ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename(this, "unit_icon_lightrune"));
        }
        private void unit_icon_sepia_Export_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawUnits(6);
            ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename(this, "unit_icon_sepia"));
        }

        private void Import_Palette_By_Address(uint palette_address)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int palette_count = 1;
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (bitmap_palette_count > palette_count)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return;
            }
            //パレット
            byte[] palette = ImageUtil.ImageToPalette(bitmap, palette_count);

            Program.Undo.Push(this.Text + "_unit_icon", palette_address, 0x20);
            Program.ROM.write_range(palette_address, palette);

            IconRedraw();

            InputFormRef.ShowWriteNotifyAnimation(this, palette_address);
        }
        
        private void unit_icon_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.RomInfo.unit_icon_palette_address());
        }

        private void unit_icon_enemy_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.RomInfo.unit_icon_enemey_palette_address());
        }

        private void unit_icon_npc_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.RomInfo.unit_icon_npc_palette_address());
        }
        private void unit_icon_four_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.RomInfo.unit_icon_four_palette_address());
        }
        private void unit_icon_gray_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.RomInfo.unit_icon_gray_palette_address());
        }
        private void unit_icon_lightrune_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.RomInfo.unit_icon_lightrune_palette_address());
        }
        private void unit_icon_sepia_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.RomInfo.unit_icon_sepia_palette_address());
        }

        private void icon_palette_Import_Click(object sender, EventArgs e)
        {
            Import_Palette_By_Address(Program.ROM.p32(Program.ROM.RomInfo.icon_palette_pointer()));
        }

        //よく使うのでキャッシュする.
        static Bitmap SystemIconCache;
        static Bitmap SystemMoveAllowCache;
        static Bitmap SystemMusicCache;
        static Bitmap SystemWeaponCache;
        static Bitmap MapFieldsCache;
        static Bitmap MapVillageCache;

        //下絵の取得
        public static Bitmap BaseImage()
        {
            if (SystemIconCache != null)
            {
                return SystemIconCache;
            }
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.system_icon_palette_pointer());
            uint image = Program.ROM.p32(Program.ROM.RomInfo.system_icon_pointer());
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);

            Size system_icon_size = GetSystemIconImageSize();
            SystemIconCache = ImageUtil.ByteToImage16Tile(system_icon_size.Width, system_icon_size.Height, imageUZ, 0, Program.ROM.Data, (int)palette);
            return SystemIconCache;
        }
        //やじるしの下絵
        public static Bitmap BaseAllowImage()
        {
            if (SystemMoveAllowCache != null)
            {
                return SystemMoveAllowCache;
            }
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.system_move_allowicon_palette_pointer());
            uint image = Program.ROM.p32(Program.ROM.RomInfo.system_move_allowicon_pointer());
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);

            SystemMoveAllowCache = ImageUtil.ByteToImage16Tile(32 * 8, 2 * 8, imageUZ, 0, Program.ROM.Data, (int)palette);
            return SystemMoveAllowCache;
        }
        //武器の下絵
        public static Bitmap BaseWeaponImage()
        {
            if (SystemWeaponCache != null)
            {
                return SystemWeaponCache;
            }
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.system_weapon_icon_palette_pointer());
            uint image = Program.ROM.p32(Program.ROM.RomInfo.system_weapon_icon_pointer());
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);

            if (Program.ROM.RomInfo.version() == 6)
            {
                SystemWeaponCache = ImageUtil.ByteToImage16Tile(32 * 8, 3 * 8, imageUZ, 0, Program.ROM.Data, (int)palette);
            }
            else
            {
                SystemWeaponCache = ImageUtil.ByteToImage16Tile(32 * 8, 2 * 8, imageUZ, 0, Program.ROM.Data, (int)palette);
            }

            return SystemWeaponCache;
        }
        //音楽系の下絵
        public static Bitmap BaseMusicImage()
        {
            if (SystemMusicCache != null)
            {
                return SystemMusicCache;
            }
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.system_music_icon_palette_pointer());
            uint image = Program.ROM.p32(Program.ROM.RomInfo.system_music_icon_pointer());
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);

            SystemMusicCache = ImageUtil.ByteToImage16Tile(32 * 8, 4 * 8, imageUZ, 0, Program.ROM.Data, (int)palette);
            return SystemMusicCache;
        }
        //フィールドマップの下絵
        static Bitmap BaseFieldsMapCache()
        {
            if (MapFieldsCache != null)
            {
                return MapFieldsCache;
            }

            uint obj_plist ;
            uint palette_plist ;
            uint config_plist ;
            //フィールドの画像を取得
            if (Program.ROM.RomInfo.version() == 8)
            {//FE8
                obj_plist = 0x01;
                palette_plist = 0x02;
                config_plist = 0x03;
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {//FE7
                obj_plist = 0x1d1c;
                palette_plist = 0x1e;
                config_plist = 0x1f;
            }
            else
            {//FE6
                obj_plist = 0x0201;
                palette_plist = 0x03;
                config_plist = 0x04;
            }
            MapFieldsCache = ImageUtilMap.DrawMapStyle(obj_plist, palette_plist, config_plist);
            return MapFieldsCache;
        }
        //街マップの下絵
        static Bitmap BaseVillageMapCache()
        {
            if (MapVillageCache != null)
            {
                return MapVillageCache;
            }

            uint obj_plist;
            uint palette_plist;
            uint config_plist;
            //フィールドの画像を取得
            if (Program.ROM.RomInfo.version() == 8)
            {//FE8
                obj_plist = 0x0E;
                palette_plist = 0x0F;
                config_plist = 0x10;
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {//FE7
                obj_plist = 0x10;
                palette_plist = 0x11;
                config_plist = 0x12;
            }
            else
            {//FE6
                obj_plist = 0x12;
                palette_plist = 0x13;
                config_plist = 0x14;
            }
            MapVillageCache = ImageUtilMap.DrawMapStyle(obj_plist, palette_plist, config_plist);
            return MapVillageCache;
        }
        static Bitmap FiledMapTile(int x, int y, int width = 1 , int height = 1)
        {
            Bitmap bitmap = BaseFieldsMapCache();
            Bitmap r = ImageUtil.Copy(bitmap, x * 16, y * 16, width*16, height*16);
            return r;
        }
        static Bitmap VillageMapTile(int x, int y, int width = 1, int height = 1)
        {
            Bitmap bitmap = BaseVillageMapCache();
            Bitmap r = ImageUtil.Copy(bitmap, x * 16, y * 16, width * 16, height * 16);
            return r;
        }

        public static Bitmap Stairs()
        {
            return VillageMapTile(4, 12);
        }
        public static Bitmap Chest()
        {
            return FiledMapTile(1, 1);
        }
        public static Bitmap Woods()
        {
            return FiledMapTile(14, 24);
        }
        public static Bitmap House()
        {
            return FiledMapTile(4, 25);
        }
        public static Bitmap Vendor()
        {
            return FiledMapTile(4, 26);
        }
        public static Bitmap Armory()
        {
            return FiledMapTile(4, 28);
        }
        public static Bitmap SecretShop()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return ImageItemIconForm.DrawIconWhereID(0x6b);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                return ImageItemIconForm.DrawIconWhereID(0x84);
            }
            else
            {
                return ImageItemIconForm.DrawIconWhereID(0x84);
            }
        }
        public static Bitmap FlagIcon()
        {
            if (Program.ROM.RomInfo.version() >= 7)
            {
                return ImageSystemIconForm.MusicIcon(14);
            }
            else
            {
                return ImageSystemIconForm.MusicIcon(0);
            }
        }
        public static Bitmap StatBooster(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return ImageItemIconForm.DrawIconWhereID(0x56 + num);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                return ImageItemIconForm.DrawIconWhereID(0x59 + num);
            }
            else
            {
                return ImageItemIconForm.DrawIconWhereID(0x59 + num);
            }
        }
        public static Bitmap PromotionItem(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return ImageItemIconForm.DrawIconWhereID(0x5d + num);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                return ImageItemIconForm.DrawIconWhereID(0x62 + num);
            }
            else
            {
                return ImageItemIconForm.DrawIconWhereID(0x62 + num);
            }
        }
        public static Bitmap Fort()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return FiledMapTile(4, 28);
            }
            else
            {
                return FiledMapTile(4, 29);
            }
        }
        public static Bitmap Arena()
        {
            return FiledMapTile(4, 30);
        }
        public static Bitmap Mountain()
        {
            return FiledMapTile(16, 21);
        }
        public static Bitmap Forest()
        {
            return FiledMapTile(16, 22);
        }
        public static Bitmap Village()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return VillageMapTile(22, 2, 2, 2);
            }
            else
            {
                return VillageMapTile(0, 29, 2, 2);
            }
        }
        public static Bitmap VillageCenter()
        {
            return FiledMapTile(7, 25 , 2, 2);
        }

        public static Bitmap Castle()
        {
            return FiledMapTile(19,26, 3, 3);
        }
        public static Bitmap Door()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return VillageMapTile(0, 13);
            }
            else
            {
                return VillageMapTile(4, 11);
            }
        }
        public static Bitmap Throne()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return VillageMapTile(21, 10, 1, 2);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {//FE7には玉座のマップが、村にはない. 城で代用
                return FiledMapTile(19, 26, 3, 3);
            }
            else
            {
                return VillageMapTile(1, 3, 1, 2);
            }
        }

        public static Bitmap TalkIcon()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return ImagePortraitForm.DrawPortraitMap(2);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {//FE7には玉座のマップが、村にはない. 城で代用
                return ImagePortraitForm.DrawPortraitMap(0x16);
            }
            else
            {
                return ImagePortraitForm.DrawPortraitMap(2);
            }
        }
        public static Bitmap BaristaIcon(uint num = 0)
        {
            return ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(Program.ROM.RomInfo.unit_wait_barista_id() + num, 0, true);
        }

        public static Bitmap Blank16()
        {
            Bitmap bitmap = BaseImage();
            Bitmap r = ImageUtil.Blank(16, 16, bitmap);
            return r;
        }

        public static Bitmap Cursol()
        {
            Bitmap bitmap = BaseImage();
            Bitmap r = ImageUtil.Copy4(bitmap, 40, 0, 8, 8);
            return r;
        }
        public static Bitmap CursolDoubleCross()
        {
            Bitmap bitmap = BaseImage();
            Bitmap r = ImageUtil.Copy4(bitmap, 40, 8, 8, 8);
            return r;
        }
        public static Bitmap YubiYoko()
        {
            Bitmap bitmap = BaseImage();
            Bitmap r = ImageUtil.Copy(bitmap, 0, 0, 16, 16);
            return r;
        }
        public static Bitmap YubiTate()
        {
            Bitmap bitmap = BaseImage();
            Bitmap r = ImageUtil.Copy(bitmap, 48, 0, 16, 16);
            return r;
        }
        public static Bitmap ExitPoint()
        {
            Bitmap bitmap = BaseImage();
            Bitmap ret = ImageUtil.Blank(16, 16, bitmap);
            if (Program.ROM.RomInfo.version() == 6)
            {
                ImageUtil.BitBlt(ret, 4, 8, 8, 8, bitmap, 32, 0);
            }
            else
            {
                ImageUtil.BitBlt(ret, 8, 8, 8, 8, bitmap, 136, 16);
            }
            return ret;
        }
        public static Bitmap Allows(int number)
        {
            Bitmap bitmap = BaseAllowImage();
            Bitmap ret = ImageUtil.Blank(16, 16, bitmap);
            ImageUtil.BitBlt(ret, 0, 0, 16, 16, bitmap, number * 16, 0);
            return ret;
        }
        //武器アイコン
        public static Bitmap WeaponIcon(uint type)
        {
            Bitmap bitmap = BaseWeaponImage();
            Bitmap ret = ImageUtil.Blank(16, 16, bitmap);
            if (Program.ROM.RomInfo.version() == 6)
            {
                ImageUtil.BitBlt(ret, 0, 0, 16, 16, bitmap, (int)((type + 6) * 16), 8);
            }
            else
            {
                ImageUtil.BitBlt(ret, 0, 0, 16, 16, bitmap, (int)((type + 3) * 16), 0);
            }
            return ret;
        }
        //音楽アイコン
        public static Bitmap MusicIcon(uint type)
        {
            Bitmap bitmap = BaseMusicImage();
            Bitmap ret = ImageUtil.Blank(16, 16, bitmap);
            if (type >= 16)
            {
                ImageUtil.BitBlt(ret, 0, 0, 16, 16, bitmap, (int)((type % 16) * 16), (int)((type / 16) * 16));
            }
            else
            {
                ImageUtil.BitBlt(ret, 0, 0, 16, 16, bitmap, (int)(type * 16), 0);
            }
            return ret;
        }
        //属性アイコン
        public static Bitmap Attribute(uint type)
        {//属性アイコンはアイテムアイコンと共有しているらしい。
         //7A=炎	        1
         //7B=稲妻	        2
         //7C=風	        3
         //7D=氷	        4
         //7E=闇	        5
         //7F=光	        6
         //80=理	        7
         //ただし、パレットは武器アイコンを参照していると思われる.
            if (type <= 0)
            {
                return ImageUtil.BlankDummy();
            }
            return ImageItemIconForm.DrawIconWhereID_UsingWeaponPalette(type+0x7A - 1);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            uint image,palette,tsa;
            image   = Program.ROM.p32(Program.ROM.RomInfo.system_icon_pointer());
            palette = Program.ROM.p32(Program.ROM.RomInfo.system_icon_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,image
                , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                , Program.ROM.RomInfo.system_icon_pointer()
                , "system_icon image"
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20 * 2
                , Program.ROM.RomInfo.system_icon_palette_pointer()
                , "system_icon pal"
                , Address.DataTypeEnum.PAL);

            image = Program.ROM.p32(Program.ROM.RomInfo.system_move_allowicon_pointer());
            palette = Program.ROM.p32(Program.ROM.RomInfo.system_move_allowicon_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,image
                , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                , Program.ROM.RomInfo.system_move_allowicon_pointer()
                , "system_icon image"
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20
                , Program.ROM.RomInfo.system_move_allowicon_palette_pointer()
                , "system_move_allowicon pal"
                , Address.DataTypeEnum.LZ77PAL);

            image = Program.ROM.p32(Program.ROM.RomInfo.system_weapon_icon_pointer());
            palette = Program.ROM.p32(Program.ROM.RomInfo.system_weapon_icon_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,image
                , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                , Program.ROM.RomInfo.system_weapon_icon_pointer()
                , "system_weapon image"
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20
                , Program.ROM.RomInfo.system_weapon_icon_palette_pointer()
                , "system_weapon pal"
                , Address.DataTypeEnum.PAL);

            image = Program.ROM.p32(Program.ROM.RomInfo.system_music_icon_pointer());
            palette = Program.ROM.p32(Program.ROM.RomInfo.system_music_icon_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,image
                , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                , Program.ROM.RomInfo.system_music_icon_pointer()
                , "system_music image"
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20
                , Program.ROM.RomInfo.system_music_icon_palette_pointer()
                , "system_music pal"
                , Address.DataTypeEnum.PAL);

            palette = (Program.ROM.RomInfo.unit_icon_palette_address());
            FEBuilderGBA.Address.AddAddress(list, palette
                , 0x20
                , U.NOT_FOUND
                , "unit_icon_play pal"
                , Address.DataTypeEnum.PAL);
            palette = (Program.ROM.RomInfo.unit_icon_enemey_palette_address());
            FEBuilderGBA.Address.AddAddress(list, palette
                , 0x20
                , U.NOT_FOUND
                , "unit_icon_enemey pal"
                , Address.DataTypeEnum.PAL);
            palette = (Program.ROM.RomInfo.unit_icon_npc_palette_address());
            FEBuilderGBA.Address.AddAddress(list, palette
                , 0x20
                , U.NOT_FOUND
                , "unit_icon_npc pal"
                , Address.DataTypeEnum.PAL);
            palette = (Program.ROM.RomInfo.unit_icon_gray_palette_address());
            FEBuilderGBA.Address.AddAddress(list, palette
                , 0x20
                , U.NOT_FOUND
                , "unit_icon_gray pal"
                , Address.DataTypeEnum.PAL);
            palette = (Program.ROM.RomInfo.unit_icon_four_palette_address());
            FEBuilderGBA.Address.AddAddress(list, palette
                , 0x20
                , U.NOT_FOUND
                , "unit_icon_for pal"
                , Address.DataTypeEnum.PAL);

            if (Program.ROM.RomInfo.version() >= 7)
            {
                image = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_common_image_pointer());
                palette = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_common_palette_pointer());

                FEBuilderGBA.Address.AddAddress(list,image
                    , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                    , Program.ROM.RomInfo.systemmenu_common_image_pointer()
                    , "systemmenu_goal image"
                    , Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddHeaderTSAPointer(list
                    , Program.ROM.RomInfo.systemmenu_goal_tsa_pointer()
                    , "systemmenu_goal tsa",isPointerOnly
                    );
                FEBuilderGBA.Address.AddAddress(list,palette
                    , 0x20 * 4
                    , Program.ROM.RomInfo.systemmenu_common_palette_pointer()
                    , "systemmenu_goal pal"
                    , Address.DataTypeEnum.PAL);
            }

            image = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_common_image_pointer());
            tsa = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_terrain_tsa_pointer());
            palette = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_common_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,image
                , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                , Program.ROM.RomInfo.systemmenu_common_image_pointer()
                , "systemmenu_common image"
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddHeaderTSAPointer(list
                , Program.ROM.RomInfo.systemmenu_terrain_tsa_pointer()
                , "systemmenu_common tsa", isPointerOnly);
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20 * 4
                , Program.ROM.RomInfo.systemmenu_common_palette_pointer()
                , "systemmenu_common"
                , Address.DataTypeEnum.LZ77IMG);

            FEBuilderGBA.Address.AddHeaderTSAPointer(list
                , Program.ROM.RomInfo.systemmenu_name_tsa_pointer()
                , "systemmenu_name tsa", isPointerOnly);

            image = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_battlepreview_image_pointer());
            palette = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_battlepreview_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,image
                , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                , Program.ROM.RomInfo.systemmenu_battlepreview_image_pointer()
                , "systemmenu_battlepreview image"
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddHeaderTSAPointer(list
                , Program.ROM.RomInfo.systemmenu_battlepreview_tsa_pointer()
                , "systemmenu_battlepreview tsa", isPointerOnly);
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20 * 4
                , Program.ROM.RomInfo.systemmenu_battlepreview_palette_pointer()
                , "systemmenu_battlepreview pal"
                , Address.DataTypeEnum.PAL);
            if (Program.ROM.RomInfo.version() == 8)
            {//FE8の場合、画像イメージは4つのポインタがあります。
                uint other_image_p = Program.ROM.RomInfo.systemmenu_battlepreview_image_pointer();
                FEBuilderGBA.Address.AddLZ77Pointer(list, other_image_p + 4, "systemmenu_battlepreview_enemy", isPointerOnly,Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddLZ77Pointer(list, other_image_p + 8, "systemmenu_battlepreview_npc", isPointerOnly, Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddLZ77Pointer(list, other_image_p + 12, "systemmenu_battlepreview_4th", isPointerOnly, Address.DataTypeEnum.LZ77IMG);
            }

            palette = Program.ROM.p32(Program.ROM.RomInfo.systemarea_move_gradation_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20 * 3
                , Program.ROM.RomInfo.systemarea_move_gradation_palette_pointer()
                , "systemarea_move_gradation"
                , Address.DataTypeEnum.PAL);
            palette = Program.ROM.p32(Program.ROM.RomInfo.systemarea_attack_gradation_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20 * 3
                , Program.ROM.RomInfo.systemarea_attack_gradation_palette_pointer()
                , "systemarea_attack_gradation"
                , Address.DataTypeEnum.PAL);
            palette = Program.ROM.p32(Program.ROM.RomInfo.systemarea_staff_gradation_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list,palette
                , 0x20 * 3
                , Program.ROM.RomInfo.systemarea_staff_gradation_palette_pointer()
                , "systemarea_staff_gradation"
                , Address.DataTypeEnum.PAL);

            if (Program.ROM.RomInfo.version() >= 8)
            {//FE8
                image = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_badstatus_image_pointer());
                FEBuilderGBA.Address.AddAddress(list,image
                    , 40 * (8 * 9) / 2
                    , Program.ROM.RomInfo.systemmenu_badstatus_image_pointer()
                    , "systemmenu_badstatus"
                    , Address.DataTypeEnum.LZ77IMG);
            }
            else if (Program.ROM.RomInfo.version() >= 7)
            {//FE7
                image = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_badstatus_image_pointer());
                FEBuilderGBA.Address.AddAddress(list, image
                    , 40 * (8 * 4) / 2
                    , Program.ROM.RomInfo.systemmenu_badstatus_image_pointer()
                    , "systemmenu_badstatus"
                    , Address.DataTypeEnum.LZ77IMG);
            }
            else
            {//FE6
            }

            palette = Program.ROM.p32(Program.ROM.RomInfo.systemmenu_badstatus_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list, palette
                , 0x20
                , Program.ROM.RomInfo.systemmenu_badstatus_palette_pointer()
                , "systemmenu_badstatus_palette"
                , Address.DataTypeEnum.PAL);


            FEBuilderGBA.Address.AddHeaderTSAPointer(list
                , Program.ROM.RomInfo.system_tsa_16color_304x240_pointer()
                , "system_tsa_16color_304x240", isPointerOnly);
        }

        private void systemarea_move_gradation_palette_Button_Click(object sender, EventArgs e)
        {
            ImageSystemAreaForm f = (ImageSystemAreaForm)InputFormRef.JumpForm<ImageSystemAreaForm>(); 
            f.JumpToAddr((uint)systemarea_move_gradation_palette.Value);
        }

        private void systemarea_attack_gradation_palette_Button_Click(object sender, EventArgs e)
        {
            ImageSystemAreaForm f = (ImageSystemAreaForm)InputFormRef.JumpForm<ImageSystemAreaForm>();
            f.JumpToAddr((uint)systemarea_attack_gradation_palette.Value);
        }

        private void systemarea_staff_gradation_palette_Button_Click(object sender, EventArgs e)
        {
            ImageSystemAreaForm f = (ImageSystemAreaForm)InputFormRef.JumpForm<ImageSystemAreaForm>();
            f.JumpToAddr((uint)systemarea_staff_gradation_palette.Value);
        }
        public void JumpToPage(uint page)
        {
            this.WMTabControl.SelectedIndex = (int)page;
        }

        private void X_StatusBackgroundLink_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("StatusBackground", 1);
        }
        private void X_SystemMenuPaletteLink_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("SystemMenuPalette", 1);
        }

        private void X_GraphicsTool_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<GraphicsToolForm>();
        }

        private void X_Jump_Patch_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("#IMAGE", 0, PatchForm.SortEnum.SortName);
        }

        private void X_Internet_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoMoreData();
        }

        void OpenPaletteEditor(Bitmap baseBitmap, uint addr)
        {
            ImagePalletForm f = (ImagePalletForm)InputFormRef.JumpForm<ImagePalletForm>(U.NOT_FOUND);

            f.JumpTo(baseBitmap, addr, 1);
            f.FormClosed += (s, ee) =>
            {
                if (this.IsDisposed)
                {
                    return;
                }
            };
        }
        private void unit_icon_Label_Click(object sender, EventArgs e)
        {
            OpenPaletteEditor(DrawUnits(0), (uint)unit_icon_PALETTE.Value);
        }

        private void unit_icon_enemy_Label_Click(object sender, EventArgs e)
        {
            OpenPaletteEditor(DrawUnits(2), (uint)unit_icon_enemy_PALETTE.Value);
        }

        private void unit_icon_npc_Label_Click(object sender, EventArgs e)
        {
            OpenPaletteEditor(DrawUnits(1), (uint)unit_icon_npc_PALETTE.Value);
        }

        private void unit_icon_four_Label_Click(object sender, EventArgs e)
        {
            OpenPaletteEditor(DrawUnits(4), (uint)unit_icon_four_PALETTE.Value);
        }

        private void unit_icon_gray_Label_Click(object sender, EventArgs e)
        {
            OpenPaletteEditor(DrawUnits(3), (uint)unit_icon_gray_PALETTE.Value);
        }

        private void icon_palette_Label_Click(object sender, EventArgs e)
        {
            OpenPaletteEditor(DrawItemIcons(), (uint)item_icon_PALETTE.Value);
        }





    }
}
