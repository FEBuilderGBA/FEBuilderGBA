using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapImageForm : Form
    {
        public WorldMapImageForm()
        {
            InitializeComponent();

            Border_InputFormRef = Border_Init(this);
            ICON_InputFormRef = ICON_Init(this);

            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(DarkMAPExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(DarkMAPImportButton, Properties.Resources.icon_upload);
            U.SetIcon(BORDER_ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(BORDER_ImportButton, Properties.Resources.icon_upload);
        }

        ImageFormRef WMEvent;
        ImageFormRef WMMini;
        ImageFormRef WMPoint1;
        ImageFormRef WMPoint2;
        ImageFormRef WMRoad;

        private void WorldMapImageForm_Load(object sender, EventArgs e)
        {
            WMPictureBox.Image = DrawWorldMap();
            WMImage.Value = Program.ROM.u32( Program.ROM.RomInfo.worldmap_big_image_pointer());
            WMPalette.Value = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_palette_pointer());
            WMdPalette.Value = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_dpalette_pointer());
            WMPaletteMap.Value = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

            WMEvent = new ImageFormRef(this, "WMEvent", 32 * 8, 20 * 8, 4, Program.ROM.RomInfo.worldmap_event_image_pointer(), Program.ROM.RomInfo.worldmap_event_tsa_pointer(), Program.ROM.RomInfo.worldmap_event_palette_pointer());
            WMMini = new ImageFormRef(this, "WMMini", 8 * 8, 8 * 8, 1, Program.ROM.RomInfo.worldmap_mini_image_pointer(), 0, Program.ROM.RomInfo.worldmap_mini_palette_pointer());
            WMPoint1 = new ImageFormRef(this, "WMPoint1", 32 * 8, 8 * 8, 1, Program.ROM.RomInfo.worldmap_icon1_pointer(), 0, Program.ROM.RomInfo.worldmap_icon_palette_pointer());
            WMPoint2 = new ImageFormRef(this, "WMPoint2", 12 * 8, 4 * 8, 1, Program.ROM.RomInfo.worldmap_icon2_pointer(), 0, Program.ROM.RomInfo.worldmap_icon_palette_pointer());

            WMRoad = new ImageFormRef(this, "WMRoad", 8, 120, 1, Program.ROM.RomInfo.worldmap_road_tile_pointer(), 0, Program.ROM.RomInfo.worldmap_icon_palette_pointer());
        }

        InputFormRef Border_InputFormRef;
        static InputFormRef Border_Init(Form self)
        {
            return new InputFormRef(self
                , "BORDER_"
                , Program.ROM.RomInfo.worldmap_county_border_pointer()
                , 12
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr))
                        && U.isPointer(Program.ROM.u32(addr+4))
                        ;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        public InputFormRef ICON_InputFormRef;
        static InputFormRef ICON_Init(Form self)
        {
            return new InputFormRef(self
                , "ICON_"
                , Program.ROM.RomInfo.worldmap_icon_data_pointer()
                , 16
                , (int i, uint addr) =>
                {//終端データは存在しない
                    uint p = Program.ROM.u32(addr + 4);
                    if (!U.isPointer(p))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    string name = "";
                    return U.ToHexString(i) + " " + name;
                }
                );
        }





        private void WriteButton_Click(object sender, EventArgs e)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_image_pointer(), WMImage, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_image_pointer(), WMImage, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_palette_pointer(), WMPalette, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_dpalette_pointer(), WMdPalette, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_palettemap_pointer(), WMPaletteMap, undodata);

            WMEvent.WritePointer(undodata);
            WMMini.WritePointer(undodata);
            WMPoint1.WritePointer(undodata);
            WMPoint2.WritePointer(undodata);
            WMRoad.WritePointer(undodata);

            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this,0);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawWorldMap();
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "WorldMap"), 4);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 480;
            int height = 320;
            int palette_count = 4;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width,bitmap.Height,width,height);
                return;
            }
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (bitmap_palette_count > palette_count)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return;
            }

            //画像
            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);
            
            //パレットマップの作成
            byte[] palettemap;
            string error_string = ImageUtil.ImageToPaletteMap(bitmap, width, height, palette_count, out palettemap);
            if (error_string != "")
            {
                R.ShowStopError(error_string);
                return;
            }

            //パレット
            byte[] palette = ImageUtil.ImageToPalette(bitmap, palette_count);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                ImageFormRef.WriteImageData(this, this.WMImage
                    , Program.ROM.RomInfo.worldmap_big_image_pointer()
                    , image, false, undodata);
                ImageFormRef.WriteImageData(this, this.WMPalette
                    , Program.ROM.RomInfo.worldmap_big_palette_pointer()
                    , palette, false, undodata);
                ImageFormRef.WriteImageData(this, this.WMPaletteMap
                    , Program.ROM.RomInfo.worldmap_big_palettemap_pointer()
                    , palettemap, true, undodata); //パレットマップだけ lz77圧縮
                Program.Undo.Push(undodata);
            }
            //ポインタの書き込み
            this.AllWriteButton.PerformClick();

            WMPictureBox.Image = DrawWorldMap();
        }

        private void DarkMAPExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawDarkWorldMap();
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "WorldMap"), 4);
        }

        private void DarkMAPImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 480;
            int height = 320;
            int palette_count = 4;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width,bitmap.Height,width,height);
                return;
            }
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (bitmap_palette_count > palette_count)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return;
            }

            //パレット
            byte[] palette = ImageUtil.ImageToPalette(bitmap, palette_count);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            ImageFormRef.WriteImageData(this,this.WMdPalette
                , Program.ROM.RomInfo.worldmap_big_dpalette_pointer()
                , palette, false, undodata);
            Program.Undo.Push(undodata);

            //ポインタの書き込み
            this.AllWriteButton.PerformClick();
        }

        
        public static Bitmap DrawWorldMap()
        {
            if (Program.ROM.RomInfo.version() == 7)
            {
                return WorldMapImageFE7Form.DrawWorldMap();
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                return WorldMapImageFE6Form.DrawWorldMap();
            }


            uint image = Program.ROM.p32( Program.ROM.RomInfo.worldmap_big_image_pointer());
            uint palette = Program.ROM.p32( Program.ROM.RomInfo.worldmap_big_palette_pointer());
            uint palettemap = Program.ROM.p32( Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

            return ImageUtilMap.DrawWorldMap(image, palette, palettemap);
        }
        public static Bitmap DrawDarkWorldMap()
        {
            uint image = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_image_pointer());
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_dpalette_pointer());
            uint palettemap = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

            return ImageUtilMap.DrawWorldMap(image, palette, palettemap);
        }
        
        public static Bitmap DrawWorldMapIcon(uint icon)
        {
            uint palette = Program.ROM.p32( Program.ROM.RomInfo.worldmap_icon_palette_pointer());
            if (icon == 7 || icon == 8 || icon == 0xC)
            {
                uint image = Program.ROM.p32(Program.ROM.RomInfo.worldmap_icon2_pointer());
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
                int width = 8 * 12;
                int height = ImageUtil.CalcHeight(width, imageUZ.Length);
                Bitmap bitmap = ImageUtil.ByteToImage16Tile(width, height, imageUZ, 0, Program.ROM.Data, (int)palette);
                if (icon == 7)
                {
                    return ImageUtil.Copy(bitmap, 0, 0, 32, 32);
                }
                if (icon == 8)
                {
                    return ImageUtil.Copy(bitmap, 32, 0, 32, 32);
                }
                //0xC
                return ImageUtil.Copy(bitmap, 64, 0, 32, 32);

            }
            else if (icon <= 0x10)
            {
                uint image = Program.ROM.p32(Program.ROM.RomInfo.worldmap_icon1_pointer());
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
                int width = 8*32;
                int height = ImageUtil.CalcHeight(width,imageUZ.Length);
                Bitmap bitmap = ImageUtil.ByteToImage16Tile(width, height, imageUZ, 0, Program.ROM.Data, (int)palette);
                if (icon == 0)
                {
                    return ImageUtil.Copy(bitmap, 0, 0, 32, 32);
                }
                if (icon == 1)
                {
                    return ImageUtil.Copy(bitmap, 32, 0, 32, 32);
                }
                if (icon == 2 || icon == 3 || icon == 4)
                {
                    return ImageUtil.Copy(bitmap, 64, 0, 32, 32);
                }
                if (icon == 5)
                {
                    return ImageUtil.Copy(bitmap, 144, 0, 16, 16);
                }
                if (icon == 6)
                {
                    return ImageUtil.Copy(bitmap, 208, 0, 32, 32);
                }
                if (icon == 9)
                {
                    return ImageUtil.Copy(bitmap, 192, 0, 16, 16);
                }
                if (icon == 0xA)
                {
                    return ImageUtil.Copy(bitmap, 144, 16, 16, 16);
                }
                if (icon == 0xB)
                {
                    return ImageUtil.Copy(bitmap, 160, 0, 16, 16);
                }
                if (icon == 0xD)
                {
                    return ImageUtil.Copy(bitmap, 128, 0, 16, 32);
                }
                if (icon == 0xE)
                {
                    return ImageUtil.Copy(bitmap, 96, 0, 32, 32);
                }
                if (icon == 0xF)
                {
                    return ImageUtil.Copy(bitmap, 160, 16, 8, 8);
                }
                if (icon == 0x10)
                {
                    return ImageUtil.Copy(bitmap, 144, 0, 16, 16);
                }
            }
            return ImageUtil.Blank(1, 1);
        }

        //ワールドマップのアイコンへ
        public void JumpToWMIcon()
        {
            WMTabControl.SelectedTab = WMPointIconTabPage;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            {
                uint image = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_image_pointer());
                uint palette = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_palette_pointer());
                uint dpalette = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_dpalette_pointer());
                uint palettemap = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

                FEBuilderGBA.Address.AddAddress(list, image
                    , isPointerOnly ? 0 : (uint)(480 / 2 * 320)
                    , Program.ROM.RomInfo.worldmap_big_image_pointer()
                    , "worldmap_big_image"
                    , FEBuilderGBA.Address.DataTypeEnum.BIN);
                FEBuilderGBA.Address.AddAddress(list, palette
                    , 0x20 * 4
                    , Program.ROM.RomInfo.worldmap_big_palette_pointer()
                    , "worldmap_big_palette"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
                FEBuilderGBA.Address.AddAddress(list, dpalette
                    , 0x20 * 4
                    , Program.ROM.RomInfo.worldmap_big_dpalette_pointer()
                    , "worldmap_big_dpalette"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
                FEBuilderGBA.Address.AddAddress(list, palettemap
                    , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, palettemap)
                    , Program.ROM.RomInfo.worldmap_big_palettemap_pointer()
                    , "worldmap_big_palettemap"
                    , FEBuilderGBA.Address.DataTypeEnum.BIN);
            }
            {
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_event_image_pointer()
                    , "worldmap_event_image"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_event_tsa_pointer()
                    , "worldmap_event_image"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);
                FEBuilderGBA.Address.AddPointer(list
                    , Program.ROM.RomInfo.worldmap_event_palette_pointer()
                    , 0x20 * 4
                    , "worldmap_event_palette"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
            {
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_mini_image_pointer()
                    , "worldmap_mini_image"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddPointer(list
                    , Program.ROM.RomInfo.worldmap_mini_palette_pointer()
                    , 0x20 * 1
                    , "worldmap_mini_palette"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
            {
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_icon1_pointer()
                    , "worldmap_icon1"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddPointer(list
                    , Program.ROM.RomInfo.worldmap_icon_palette_pointer()
                    , 0x20 * 2
                    , "worldmap_icon_palette"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
            {
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_icon2_pointer()
                    , "worldmap_icon2"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            }
            {
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_road_tile_pointer()
                    , "worldmap_road_tile_image"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            }
            {
                InputFormRef ifr = Border_Init(null);
                FEBuilderGBA.Address.AddAddress(list, ifr, "WorldmapCountyBorder", new uint[] { 0, 4 });

                uint addr = ifr.BaseAddress;
                for (int i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
                {
                    string name = "WorldmapCountyBorder " + U.To0xHexString(i);
                    FEBuilderGBA.Address.AddLZ77Pointer(list, 0 + addr
                        , name + " IMAGE"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.POINTER);
                    FEBuilderGBA.Address.AddROMTCSPointer(list, 0 + addr
                        , name + " ROMTCS"
                        , isPointerOnly
                        );
                }
            }
            {
                string name = "WorldMapIconData";
                InputFormRef InputFormRef = ICON_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 4 });
            }
        }

        private void DecreaseColorTSAToolButton_Click(object sender, EventArgs e)
        {
            DecreaseColorTSAToolForm f = (DecreaseColorTSAToolForm)InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
            f.InitMethod(3);

        }

        private void DecreaseColorTSAToolForWorldmapEventButton_Click(object sender, EventArgs e)
        {
            DecreaseColorTSAToolForm f = (DecreaseColorTSAToolForm)InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
            f.InitMethod(4);

        }

        void DrawBorderImages()
        {
            Bitmap parts = ImageUtilBorderAP.DrawBorderBitmap((uint)BORDER_P0.Value);
            BORDER_X_BG_PIC.Image = parts;
            if (parts == null)
            {
                X_BORDER_DRAW_SAMPLE.Image = null;
                return;
            }

            Bitmap retScreen = ImageUtilBorderAP.DrawBorderImages(parts, (uint)BORDER_P4.Value, (int)BORDER_W8.Value, (int)BORDER_W10.Value);
            X_BORDER_DRAW_SAMPLE.Image = retScreen;
        }


        private void BORDER_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawBorderImages();
        }
        private void BORDER_ExportButton_Click(object sender, EventArgs e)
        {
            string filename = Border_InputFormRef.MakeSaveImageFilename();
            ImageUtilBorderAP.SaveAPImages(filename, (uint)BORDER_P0.Value, (uint)BORDER_P4.Value, (int)BORDER_W8.Value, (int)BORDER_W10.Value);
        }



        private void BORDER_ImportButton_Click(object sender, EventArgs e)
        {
            uint origin_x = (uint)BORDER_W8.Value;
            uint origin_y = (uint)BORDER_W10.Value;
            if (origin_x >= 60)
            {
                origin_x = 60;
            }
            if (origin_y >= 50)
            {
                origin_y = 50;
            }

            byte[] image ;
            byte[] oam ;
            bool r = ImageUtilBorderAP.ImportBorder(this,
                origin_x,
                origin_y,
                out image , out oam);
            if (!r)
            {
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            //画像の書き込み
            this.Border_InputFormRef.WriteImageData(this.BORDER_P0, image, true, undodata);

            //APの書き込み
            uint newaddr = InputFormRef.WriteBinaryData(this
                , (uint)this.BORDER_P4.Value
                , oam
                , InputFormRef.get_data_pos_callback_ap
                , undodata
            );
            if (newaddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return;
            }
            this.BORDER_P4.Value = U.toPointer(newaddr);
            Program.Undo.Push(undodata);

            //ポインタの書き込み
            this.BORDER_WriteButton.PerformClick();
            return;
        }

        private void BORDER_W8_ValueChanged(object sender, EventArgs e)
        {
            if (sender != BORDER_W8)
            {
                return;
            }
            DrawBorderImages();
        }

        private void BORDER_W10_ValueChanged(object sender, EventArgs e)
        {
            if (sender != BORDER_W10)
            {
                return;
            }
            DrawBorderImages();
        }

    }
}
