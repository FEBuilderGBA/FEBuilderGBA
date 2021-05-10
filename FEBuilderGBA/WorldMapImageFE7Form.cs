using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapImageFE7Form : Form
    {
        public WorldMapImageFE7Form()
        {
            InitializeComponent();
        }

        ImageFormRef WMEvent;



        private void WorldMapImageFE7Form_Load(object sender, EventArgs e)
        {
            WMPictureBox.Image = DrawWorldMap();
            WMImageMap.Value = Program.ROM.u32( Program.ROM.RomInfo.worldmap_big_image_pointer());
            WMPalette.Value = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_palette_pointer());
            WMtsaMap.Value = Program.ROM.u32(Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

            WMEvent = new ImageFormRef(this, "WMEvent", 32 * 8, 20 * 8, 4, Program.ROM.RomInfo.worldmap_event_image_pointer(), Program.ROM.RomInfo.worldmap_event_tsa_pointer(), Program.ROM.RomInfo.worldmap_event_palette_pointer());
        }
        private void WriteButton_Click(object sender, EventArgs e)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_image_pointer(), WMImageMap, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_image_pointer(), WMImageMap, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_palette_pointer(), WMPalette, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.worldmap_big_palettemap_pointer(), WMtsaMap, undodata);

            WMEvent.WritePointer(undodata);

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
            int width = 1024;
            int height = 688;
            int palette_count = 4;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                return;
            }
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (bitmap_palette_count > palette_count)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return;
            }

            List<byte[]> images = new List<byte[]>();
            List<byte[]> tsaes = new List<byte[]>();

            //データは、1024x688 だが、内部データは 1024x768で計算しているみたいなので、足りないラインを増設します.
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Bitmap piece = ImageUtil.Copy(
                        bitmap
                        , x * 256
                        , y * 256
                        , 256, 256
                        );

                    byte[] image; //画像
                    byte[] tsa;   //TSA
                    string error_string = ImageUtil.ImageToByteHeaderPackedTSA(
                        piece
                        , 256, 256
                        , out image, out tsa
                        ,0 //余白を作らない
                        , false //同一タイルの最適化をしない
                        );
                    if (error_string != "")
                    {
                        R.ShowStopError("12分割のx:{0} y:{1} w:256 h:256の部分の中で問題がありました"
                            , x * 256, y * 256
                            , error_string);
                        return;
                    }

                    images.Add(image);
                    tsaes.Add(tsa);
                }
            }
            {
                int y = 2;
                for (int x = 0; x < 4; x++)
                {
                    //最後のラインは、heightが 176なのだが、TSAを計算するときは 256でやる必要がある.
                    Bitmap piece = ImageUtil.Copy(
                        bitmap
                        , x * 256
                        , y * 256
                        , 256, 256);

                    byte[] image; //画像
                    byte[] tsa;   //TSA
                    string error_string = ImageUtil.ImageToByteHeaderPackedTSA(piece
                        , 256, 256
                        , out image, out tsa
                        , 0 //余白を作らない
                        , false //同一タイルの最適化をしない
                        );
                    if (error_string != "")
                    {
                        R.ShowStopError("12分割のx:{0} y:{1} w:256 h:256の部分の中で問題がありました"
                            , x * 256, y * 256
                            , error_string);
                        return;
                    }

                    //imageだけ高さを 176に変更します.
                    image = U.subrange(image,0,256 / 2 * 176);

                    images.Add(image);
                    tsaes.Add(tsa);
                }
            }

            //パレット
            byte[] palette_data = ImageUtil.ImageToPalette(bitmap, palette_count);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            //すべて無圧縮データなのでそのままかけます. 位置の変換は絶対に起きえません.
            uint imagemap = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_image_pointer());
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_palette_pointer());
            uint tsamap = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

            Program.ROM.write_range(palette, palette_data, undodata);

            int index = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    uint image = Program.ROM.p32(imagemap);
                    uint tsa = Program.ROM.p32(tsamap);

                    Program.ROM.write_range(image, images[index], undodata);
                    Program.ROM.write_range(tsa, tsaes[index], undodata);

                    imagemap += 4;
                    tsamap += 4;
                    index++;
                }
            }

            Program.Undo.Push(undodata);

            //ポインタの書き込み
            this.AllWriteButton.PerformClick();

            WMPictureBox.Image = DrawWorldMap();
        }

        public static Bitmap DrawWorldMap()
        {
            //FE7U のワールドマップは12分割されている 1024x688
            //PAL:574990 16色*4種類
            //IMAGE   HEADERTSA  SIZE
            //[0x0E7848] [0xB5BEC]      
            //DATA --------------------
            //574A10-5CAA10     256x256
            //57CA10-5CB214     256x256
            //584A10-5CBA18     256x256
            //58CA10-5CC21C     256x256
            //594A10-5CCA20     256x256
            //59CA10-5CD224     256x256
            //5A4A10-5CDA28     256x256
            //5ACA10-5CE22C     256x256
            //5B4A10-5CEA30     256x176
            //5BA210-5CF234     256x176
            //5BFA10-5CFA38     256x176
            //5C5210-5D023C     256x176
            //
            //情報元:
            //see https://serenesforest.net/forums/index.php?/topic/34362-fe7-world-map-tsa-tips/
            uint imagemap = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_image_pointer());
            uint palette = Program.ROM.p32( Program.ROM.RomInfo.worldmap_big_palette_pointer());
            uint tsamap = Program.ROM.p32( Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

            return ImageUtilMap.DrawWorldMapFE7(imagemap, palette, tsamap);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            {
                uint imagemap = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_image_pointer());
                uint palette = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_palette_pointer());
                uint tsamap = Program.ROM.p32(Program.ROM.RomInfo.worldmap_big_palettemap_pointer());

                FEBuilderGBA.Address.AddAddress(list, palette
                    , 0x20 * 4
                    , Program.ROM.RomInfo.worldmap_big_palette_pointer()
                    , "worldmap_big_palette"
                    , Address.DataTypeEnum.PAL);

                uint pointer = imagemap;
                for (int i = 0; i < 12; i++, pointer += 4)
                {
                    uint image = Program.ROM.p32(pointer);
                    uint imagelength = 256 / 2 * 256;
                    FEBuilderGBA.Address.AddAddress(list, image, imagelength, pointer, "worldmap_big_image" + i , Address.DataTypeEnum.IMG);



                    uint tsa = Program.ROM.p32(tsamap);
                    uint tsalength = 256 / 8 * 256 / 8;
                    FEBuilderGBA.Address.AddAddress(list, tsa, tsalength, tsamap, "worldmap_big_tsa" + i, Address.DataTypeEnum.TSA);
                }
            }
            {
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_event_image_pointer()
                    , "worldmap_event_image"
                    , isPointerOnly
                    , Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , Program.ROM.RomInfo.worldmap_event_tsa_pointer()
                    , "worldmap_event_tsa"
                    , isPointerOnly
                    , Address.DataTypeEnum.LZ77TSA);
                FEBuilderGBA.Address.AddPointer(list
                    , Program.ROM.RomInfo.worldmap_event_palette_pointer()
                    , 0x20 * 4
                    , "worldmap_event_palette"
                    , Address.DataTypeEnum.PAL);
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
    }
}
