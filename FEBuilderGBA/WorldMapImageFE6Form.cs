using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapImageFE6Form : Form
    {
        public WorldMapImageFE6Form()
        {
            InitializeComponent();
        }

        ImageFormRef WMZoomOut;
        ImageFormRef WMZoomNW;
        ImageFormRef WMZoomNE;
        ImageFormRef WMZoomSW;
        ImageFormRef WMZoomSE;

        private void WorldMapImageFE6Form_Load(object sender, EventArgs e)
        {
            //see https://serenesforest.net/forums/index.php?/topic/41095-fe6-localization-patch-v10-seriously-we-did-something/&page=48
            //For those interested in editing the world map bitmaps, there's 5 of them in the game. One zoomed out and 4 zoomed in for each corners (NW, NE, SW, SE). Here's the offsets for each:
            //$082AADA4: Elibe world map bitmap (NW, zoomed in)
            //$082B2380: Elibe world map bitmap (NE, zoomed in)
            //$082B9E64: Elibe world map bitmap (SW, zoomed in)
            //$082C1224: Elibe world map bitmap (SE, zoomed in)
            //$082C8874: Elibe world map bitmap (global, zoomed out)
            //There's 2 256-clrs palettes. One for the zoomed out map, and one for all the zoomed in ones (they use the same palette)
            //$082D1964: Elibe world map palette (all, zoomed in)
            //$082D1BA0: Elibe world map palette (global, zoomed out)
            WMZoomOut = new ImageFormRef(this, "WMZoomOUT", 240, 160, 16, Program.ROM.RomInfo.worldmap_big_image_pointer(), 0, U.toPointer(Program.ROM.RomInfo.worldmap_big_palette_pointer()));
            WMZoomNW = new ImageFormRef(this, "WMZoomNW", 240, 160, 16, Program.ROM.RomInfo.worldmap_big_image_pointer() + 8, 0, U.toPointer(Program.ROM.RomInfo.worldmap_big_palette_pointer() + 8));
            WMZoomNE = new ImageFormRef(this, "WMZoomNE", 240, 160, 16, Program.ROM.RomInfo.worldmap_big_image_pointer() + 16, 0, U.toPointer(Program.ROM.RomInfo.worldmap_big_palette_pointer() + 16));
            WMZoomSW = new ImageFormRef(this, "WMZoomSW", 240, 160, 16, Program.ROM.RomInfo.worldmap_big_image_pointer() + 24, 0, U.toPointer(Program.ROM.RomInfo.worldmap_big_palette_pointer() + 24));
            WMZoomSE = new ImageFormRef(this, "WMZoomSE", 240, 160, 16, Program.ROM.RomInfo.worldmap_big_image_pointer() + 32, 0, U.toPointer(Program.ROM.RomInfo.worldmap_big_palette_pointer() + 32));
        }
        private void WriteButton_Click(object sender, EventArgs e)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            WMZoomOut.WritePointer(undodata);
            WMZoomNW.WritePointer(undodata);
            WMZoomNE.WritePointer(undodata);
            WMZoomSW.WritePointer(undodata);
            WMZoomSE.WritePointer(undodata);

            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this,0);
        }


        public static Bitmap DrawWorldMap()
        {
            //FE6のワールドマップは 256色 tileではなく、通常の画像のようなlinerである.
            //
            //情報元:
            uint image256Z = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_image_pointer());
            uint paletteZ = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_palette_pointer());

            return ImageUtilMap.DrawWorldMapFE6(image256Z, paletteZ);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_image_pointer()
                , "worldmap_big_image"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_palette_pointer()
                , "worldmap_big_palette"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77PAL);


            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_image_pointer() + 8
                , "worldmap_big_imageNW"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_palette_pointer() + 8
                , "worldmap_big_paletteNW"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77PAL);


            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_image_pointer() + 16
                , "worldmap_big_imageNE"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_palette_pointer() + 16
                , "worldmap_big_paletteNE"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77PAL);


            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_image_pointer() + 24
                , "worldmap_big_imageSW"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_palette_pointer() + 24
                , "worldmap_big_paletteSW"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77PAL);


            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_image_pointer() + 32
                , "worldmap_big_imageSE"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , Program.ROM.RomInfo.worldmap_big_palette_pointer() + 32
                , "worldmap_big_paletteSE"
                , isPointerOnly
                , Address.DataTypeEnum.LZ77PAL);
        }
        
    }
}
