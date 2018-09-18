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

            U.SetIcon(ExportButton, U.GetShell32Icon(122));
            U.SetIcon(ImportButton, U.GetShell32Icon(45));
            U.SetIcon(DarkMAPExportButton, U.GetShell32Icon(122));
            U.SetIcon(DarkMAPImportButton, U.GetShell32Icon(45));
            U.SetIcon(BORDER_ExportButton, U.GetShell32Icon(122));
            U.SetIcon(BORDER_ImportButton, U.GetShell32Icon(45));
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
//                    FEBuilderGBA.Address.AddAPPointer(list, 0 + addr
//                        , name + " AP"
//                        , isPointerOnly
//                        );
                }
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

        private void BORDER_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BORDER_X_BG_PIC.Image = DrawBorderBitmap((uint)BORDER_P0.Value);
        }
        Bitmap DrawBorderBitmap(uint img)
        {
            byte[] bin = LZ77.decompress(Program.ROM.Data, U.toOffset(img));
            uint pal = Program.ROM.p32(Program.ROM.RomInfo.worldmap_county_border_palette_pointer());

            int height = ImageUtil.CalcHeight(32 * 8, bin.Length);
            if (height <= 0)
            {
                return ImageUtil.Blank(32*8,4 * 8);
            }
            return ImageUtil.ByteToImage16Tile(32 * 8, height
                , bin, 0
                , Program.ROM.Data, (int)pal);
        }
        private void BORDER_ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawBorderBitmap((uint)BORDER_P0.Value);
            ImageFormRef.ExportImage(this, bitmap, Border_InputFormRef.MakeSaveImageFilename());
        }

        private void BORDER_ImportButton_Click(object sender, EventArgs e)
        {
            int width = 8 * 32; //256
            int height = 8 * 4; //32
            int palette_count = 1;
            Bitmap bitmap = ImageUtil.LoadAndConvertDecolorUI(this, null, width, height, true, palette_count);
            if (bitmap == null)
            {
                return;
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            //check palette
            {
                uint pal = Program.ROM.p32(Program.ROM.RomInfo.worldmap_county_border_palette_pointer());
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , (pal)
                        , U.NOT_FOUND
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, pal));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            this.Border_InputFormRef.WriteImageData(this.BORDER_P0, image, true, undodata);
            Program.Undo.Push(undodata);

            //ポインタの書き込み
            this.BORDER_WriteButton.PerformClick();
        }

    }
}
