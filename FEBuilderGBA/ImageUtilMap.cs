using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    class ImageUtilMap
    {
        public const uint MAP_MAX_WIDTH = 60;
        public const uint MAP_MAX_HEIGHT = 63;
        public const uint MAP_TILE_LIMIT = 1536;

        public const uint MAP_MIN_WIDTH = 15;
        public const uint MAP_MIN_HEIGHT = 10;

        //マップチップだけを描画する
        public static Bitmap DrawMapChipOnly(
              uint obj_plist        //image
            , uint palette_plist    //palette
            , MapSettingForm.MapAnimations anime = null
            )
        {
            uint obj_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.OBJECT, obj_plist & 0xFF);
            uint obj2_plist = (obj_plist >> 8) & 0xFF; //FE8にはないが FE7は、 plistを2つ設定できることがある.
            uint obj2_offset = 0;
            uint palette_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
            if (!U.isSafetyOffset(obj_offset))
            {
                return null;
            }
            if (!U.isSafetyOffset(palette_offset))
            {
                return null;
            }

            if (obj2_plist > 0)
            {//plist2があれば
                obj2_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.OBJECT, obj2_plist);
                if (!U.isSafetyOffset(obj2_offset))
                {
                    return null;
                }
            }

            byte[] objUZ = LZ77.decompress(Program.ROM.Data, obj_offset); //image
            if (objUZ.Length <= 0)//image
            {
                return null;
            }

            if (obj2_plist > 0)
            {//plist2があれば
                byte[] obj2UZ = LZ77.decompress(Program.ROM.Data, obj2_offset);
                if (obj2UZ.Length <= 0)//image
                {
                    return null;
                }
                objUZ = U.ArrayAppend(objUZ, obj2UZ);
            }

            byte[] paletteData = Program.ROM.getBinaryData(palette_offset, (2 * 16) * 16);

            //マップアニメが定義されていればアニメーション補正する.
            if (anime != null)
            {
                if (anime.change_bitmap_bytes != null)
                {
                    U.ArrayPatch(anime.change_bitmap_bytes, 0, objUZ, 32 * (8 / 2) * 4 * 16);
                }
                if (anime.change_palette_bytes != null)
                {
                    U.ArrayPatch(anime.change_palette_bytes, 0, paletteData, (anime.change_palette_start_index) * 2);
                }
            }


            //マップチップの読込 
            int obj_width = 32 * 8;
            int obj_height = ImageUtil.CalcHeight(obj_width, objUZ.Length);
            Bitmap obj_image = ImageUtil.ByteToImage16Tile(obj_width, obj_height
                , objUZ, 0
                , paletteData , 0
                , 0
            );
            return obj_image;
        }

        //チップセットの読込(マップチップの画像をどう解釈するか定義するデータ)
        public static byte[] UnLZ77ChipsetData(uint config_plist)
        {
            uint config_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CONFIG, config_plist);
            if (!U.isSafetyOffset(config_offset))
            {
                return null;
            }

            byte[] configUZ = LZ77.decompress(Program.ROM.Data, config_offset);
            if (configUZ.Length <= 0)//TSA
            {
                return null;
            }
            return configUZ;
        }

        public static UInt16[] UnLZ77MapDataUShort(uint mappointer_plist, out int out_width, out int out_height)
        {
            //マップ配置データの読込
            byte[] mappointerUZ = ImageUtilMap.UnLZ77MapData(mappointer_plist);
            if (mappointerUZ == null)
            {
                out_width = 0;
                out_height = 0;
                return null;
            }

            int mapWidth = mappointerUZ[0];
            int mapHeight = mappointerUZ[1];
            if (mapWidth <= 0 || mapHeight <= 0)
            {
                out_width = 0;
                out_height = 0;
                return null;
            }

            UInt16[] mar = new UInt16[(mapWidth) * (mapHeight)];
            int n = 0;
            int length = mappointerUZ.Length;
            for (int i = 2; i + 1 < length; i += 2, n++)
            {
                if (n >= mar.Length)
                {
                    break;
                }

                //マップデータを読む
                int m = (mappointerUZ[i] + ((UInt16)mappointerUZ[i + 1] << 8));
                mar[n] = (UInt16)m;
            }
            out_width = mapWidth;
            out_height = mapHeight;
            return mar;
        }

        //マップの配置データの読込 
        public static byte[] UnLZ77MapData(uint mappointer_plist)
        {
            uint mappointer_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.MAP, mappointer_plist);
            if (!U.isSafetyOffset(mappointer_offset))
            {
                return null;
            }

            byte[] mappointerUZ = LZ77.decompress(Program.ROM.Data, mappointer_offset); //tsa
            if (mappointerUZ.Length <= 0)
            {
                return null;
            }
            return mappointerUZ;
        }

        public static Bitmap DrawMapStyle(
              uint obj_plist        //image
            , uint palette_plist    //palette
            , uint config_plist    //tsa
            )
        {
            //マップチップの読込
            Bitmap MapObjImage = ImageUtilMap.DrawMapChipOnly(obj_plist, palette_plist);
            if (MapObjImage == null)
            {
                MapObjImage = ImageUtil.BlankDummy();
            }
            //チップセットの読込(マップチップの画像をどう解釈するか定義するデータ)
            byte[] configUZ = ImageUtilMap.UnLZ77ChipsetData(config_plist);
            if (configUZ == null)
            {
                configUZ = new byte[0];
            }

            Bitmap mapObjCels = ImageUtil.Blank(16 * 32, ImageUtilMap.CHIPSET_SEP_BYTE / 16, MapObjImage);
            int chip = 0;
            int y = 0;
            while (chip < ImageUtilMap.CHIPSET_SEP_BYTE / 8)
            {
                for (int x = 0; x < 32; x++)
                {
                    ImageUtil.BitBlt(mapObjCels, x * 16, y * 16, 16, 16, ImageUtilMap.DrawOneChipset(chip << 2, configUZ, MapObjImage), 0, 0);

                    chip++;
                }
                y++;
            }
            return mapObjCels;
        }
        //マップを描画する.
        public static Bitmap DrawMap(
              uint obj_plist        //image
            , uint palette_plist    //palette
            , uint config_plist    //tsa
            , uint mappointer_plist //mar
            , MapSettingForm.MapAnimations anime = null
            )
        {
            //マップチップの読込
            Bitmap obj_image = DrawMapChipOnly(obj_plist, palette_plist , anime);
            if (obj_image == null)
            {
                return ImageUtil.BlankDummy();
            }

            //チップセットの読込(マップチップの画像をどう解釈するか定義するデータ)
            byte[] configUZ = UnLZ77ChipsetData(config_plist);
            if (configUZ == null)
            {
                return ImageUtil.BlankDummy();
            }

            //マップ配置データの読込
            byte[] mappointerUZ = UnLZ77MapData(mappointer_plist);
            if (mappointerUZ == null)
            {
                return ImageUtil.BlankDummy();
            }

            if (mappointerUZ.Length < 2)
            {
                Debug.Assert(false);
                return ImageUtil.BlankDummy();
            }

            //マップデータの先頭2バイトにマップの幅高さが入っている.
            //チップの数なので、 16*width=ピクセル  16*height=ピクセル である.
            int width = mappointerUZ[0];
            int height = mappointerUZ[1];

            if (width <= 0 || height <= 0)
            {//サイズが不正
                return ImageUtil.BlankDummy();
            }

            UInt16[] tsa = new UInt16[(width * 2) * (height * 2)];

            int x = 0;
            int y = 0;
            for (int i = 2; i + 1 < mappointerUZ.Length; i += 2)
            {
                //マップデータを読む
                int m = (mappointerUZ[i] + ((UInt16)mappointerUZ[i + 1] << 8));
                int tile_tsa_index = m << 1;
                if (tile_tsa_index + 7 >= configUZ.Length)
                {//不正なTSA
                    return ImageUtil.BlankDummy();
                }

                //チップセットのTSAデータを読み込む.
                UInt16 lefttop = (UInt16)(configUZ[tile_tsa_index] + ((UInt16)configUZ[tile_tsa_index + 1] << 8));
                UInt16 righttop = (UInt16)(configUZ[tile_tsa_index + 2] + ((UInt16)configUZ[tile_tsa_index + 3] << 8));
                UInt16 leftbottom = (UInt16)(configUZ[tile_tsa_index + 4] + ((UInt16)configUZ[tile_tsa_index + 5] << 8));
                UInt16 rightbottom = (UInt16)(configUZ[tile_tsa_index + 6] + ((UInt16)configUZ[tile_tsa_index + 7] << 8));

                int tsaIndex;
                tsaIndex = (x) + ((y) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = lefttop;

                tsaIndex = (x + 1) + ((y) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = righttop;

                tsaIndex = (x) + ((y + 1) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = leftbottom;

                tsaIndex = (x + 1) + ((y + 1) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = rightbottom;

                x += 2;
                if (x >= width * 2)
                {
                    x = 0;
                    y += 2;
                    if (y >= height * 2)
                    {
                        break;
                    }
                }
            }
            Bitmap mapcanvas = ImageUtil.Blank(width * 16, height * 16, obj_image);
            ImageUtil.BitBltTSA(mapcanvas, obj_image, tsa, 0);

            return mapcanvas;
        }

        //マップをチェックする.
        public static Size GetMapSize(
              uint obj_plist        //image
            , uint palette_plist    //palette
            , uint config_plist    //tsa
            , uint mappointer_plist //mar
            , out string out_error
            )
        {
            out_error= "";
            //チップセットの読込(マップチップの画像をどう解釈するか定義するデータ)
            byte[] configUZ = UnLZ77ChipsetData(config_plist);
            if (configUZ == null)
            {
                out_error = R._("チップセット(plist config)を読み込めませんでした。");
                return new Size(0, 0);
            }

            //マップ配置データの読込
            byte[] mappointerUZ = UnLZ77MapData(mappointer_plist);
            if (mappointerUZ == null)
            {
                out_error = R._("マップデータ(plist map)を読み込めませんでした。");
                return new Size(0, 0);
            }

            if (mappointerUZ.Length < 2)
            {
                out_error = R._("マップデータ(plist map)のデータの長さが2バイト以下です。");
                return new Size(0, 0);
            }

            //マップデータの先頭2バイトにマップの幅高さが入っている.
            //チップの数なので、 16*width=ピクセル  16*height=ピクセル である.
            int width = mappointerUZ[0];
            int height = mappointerUZ[1];

            if (width <= 0 || height <= 0)
            {//サイズが不正
                out_error = R._("マップデータ(plist map)のデータのサイズが正しくありません。幅({0}),高さ({1})。", width, height);
                return new Size(0, 0);
            }

            UInt16[] tsa = new UInt16[(width * 2) * (height * 2)];

            int x = 0;
            int y = 0;
            for (int i = 2; i + 1 < mappointerUZ.Length; i += 2)
            {
                //マップデータを読む
                int m = (mappointerUZ[i] + ((UInt16)mappointerUZ[i + 1] << 8));
                int tile_tsa_index = m << 1;
                if (tile_tsa_index + 7 >= configUZ.Length)
                {//不正なTSA
                    out_error = R._("マップデータ(plist map)のデータ中に不正なタイル({0})が(X:{1},Y:{2})にあります。", U.To0xHexString(tile_tsa_index), x/2,y/2 );
                    return new Size(0, 0);
                }

                //チップセットのTSAデータを読み込む.
                UInt16 lefttop = (UInt16)(configUZ[tile_tsa_index] + ((UInt16)configUZ[tile_tsa_index + 1] << 8));
                UInt16 righttop = (UInt16)(configUZ[tile_tsa_index + 2] + ((UInt16)configUZ[tile_tsa_index + 3] << 8));
                UInt16 leftbottom = (UInt16)(configUZ[tile_tsa_index + 4] + ((UInt16)configUZ[tile_tsa_index + 5] << 8));
                UInt16 rightbottom = (UInt16)(configUZ[tile_tsa_index + 6] + ((UInt16)configUZ[tile_tsa_index + 7] << 8));

                int tsaIndex;
                tsaIndex = (x) + ((y) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = lefttop;

                tsaIndex = (x + 1) + ((y) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = righttop;

                tsaIndex = (x) + ((y + 1) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = leftbottom;

                tsaIndex = (x + 1) + ((y + 1) * (width * 2));
                if (tsaIndex < tsa.Length) tsa[tsaIndex] = rightbottom;

                x += 2;
                if (x >= width * 2)
                {
                    x = 0;
                    y += 2;
                    if (y >= height * 2)
                    {
                        break;
                    }
                }
            }
            return new Size(width , height );
        }


        //マップの部分変更
        public static Bitmap DrawChangeMap(
              uint obj_plist
            , uint palette_plist
            , uint config_plist
            , int width
            , int height
            , uint change_address
            , MapSettingForm.MapAnimations anime = null
            )
        {
            change_address = U.toOffset(change_address);
            if (!U.isSafetyOffset(change_address))
            {
                return ImageUtil.BlankDummy();
            }

            //マップチップの読込
            Bitmap obj_image = DrawMapChipOnly(obj_plist, palette_plist, anime);
            if (obj_image == null)
            {
                return ImageUtil.BlankDummy();
            }
            //チップセットの読込(マップチップの画像をどう解釈するか定義するデータ)
            byte[] configUZ = UnLZ77ChipsetData(config_plist);
            if (configUZ == null)
            {
                return ImageUtil.BlankDummy();
            }

            //マップ変更の場合、幅高さはユーザが指定する.
            UInt16[] tsa = new UInt16[(width * 2) * (height * 2)];

            int x = 0;
            int y = 0;
            int length = width * height * 2;
            if (! U.isSafetyOffset( change_address + (uint)length - 1) )
            {//サイズ超過.
                R.Error("map size over {0}-{1}", U.ToHexString(change_address), U.ToHexString(length));
                return ImageUtil.BlankDummy();
            }

            for (int i = 0; i + 1 < length; i += 2)
            {
                //マップデータを読む
                int m = (int)Program.ROM.u16((uint)(change_address + i));
                int tile_tsa_index = m << 1;
                if (tile_tsa_index + 7 >= configUZ.Length)
                {//不正なTSA
                    return ImageUtil.BlankDummy(16);
                }

                //チップセットのTSAデータを読み込む.
                UInt16 lefttop = (UInt16)(configUZ[tile_tsa_index] + ((UInt16)configUZ[tile_tsa_index + 1] << 8));
                UInt16 righttop = (UInt16)(configUZ[tile_tsa_index + 2] + ((UInt16)configUZ[tile_tsa_index + 3] << 8));
                UInt16 leftbottom = (UInt16)(configUZ[tile_tsa_index + 4] + ((UInt16)configUZ[tile_tsa_index + 5] << 8));
                UInt16 rightbottom = (UInt16)(configUZ[tile_tsa_index + 6] + ((UInt16)configUZ[tile_tsa_index + 7] << 8));

                tsa[(x) + ((y) * (width * 2))] = lefttop;
                tsa[(x + 1) + ((y) * (width * 2))] = righttop;
                tsa[(x) + ((y + 1) * (width * 2))] = leftbottom;
                tsa[(x + 1) + ((y + 1) * (width * 2))] = rightbottom;

                x += 2;
                if (x >= width * 2)
                {
                    x = 0;
                    y += 2;
                    if (y >= height * 2)
                    {
                        break;
                    }
                }
            }
            Bitmap mapcanvas = ImageUtil.Blank(width * 16, height * 16, obj_image);
            ImageUtil.BitBltTSA(mapcanvas, obj_image, tsa, 0);

            return mapcanvas;
        }


        //ワールドマップを描画する
        public static Bitmap DrawWorldMap(
              uint image
            , uint palette     //通常パレット
            , uint palettemap //パレットマップ
            )
        {
            image = U.toOffset(image);
            palette = U.toOffset(palette);
            palettemap = U.toOffset(palettemap);
            if (!U.isSafetyOffset(image))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(palette))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(palettemap))
            {
                return ImageUtil.BlankDummy();
            }
            byte[] palettemapUZ = LZ77.decompress(Program.ROM.Data, palettemap);

            return ImageUtil.ByteToImage16TilePaletteMap(480, 320
                , Program.ROM.Data, (int)image
                , Program.ROM.Data, (int)palette
                , palettemapUZ, 0
                );
        }
        //FE7のワールドマップを描画する
        //詳細は、呼び出し元のメソッドを見てね.
        public static Bitmap DrawWorldMapFE7(uint imagemap, uint palette, uint tsamap)
        {
            Bitmap map = ImageUtil.Blank(1024, 688);
            map.Palette = ImageUtil.ByteToPalette(map.Palette, Program.ROM.Data, (int)palette);

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    uint image = Program.ROM.p32(imagemap);
                    uint tsa = Program.ROM.p32(tsamap);

                    Bitmap piece = ImageUtil.ByteToImage16TileHeaderTSA(256, 256, Program.ROM.Data, (int)image, Program.ROM.Data, (int)palette, Program.ROM.Data, (int)tsa);
                    ImageUtil.BitBlt(map, x * 256, y * 256, 256, 256, piece, 0, 0);

                    imagemap += 4;
                    tsamap += 4;

                }
            }
            //最後のラインだけheightが違う.
            {
                int y = 2;
                for (int x = 0; x < 4; x++)
                {
                    uint image = Program.ROM.p32(imagemap);
                    uint tsa = Program.ROM.p32(tsamap);

                    Bitmap piece = ImageUtil.ByteToImage16TileHeaderTSA(256, 256, Program.ROM.Data, (int)image, Program.ROM.Data, (int)palette, Program.ROM.Data, (int)tsa);
                    ImageUtil.BitBlt(map, x * 256, y * 256, 256, 176, piece, 0, 0);

                    imagemap += 4;
                    tsamap += 4;
                }
            }

            return map;
        }
        public static Bitmap DrawWorldMapFE6(uint image256Z, uint paletteZ)
        {
            byte[] image256UZ = LZ77.decompress(Program.ROM.Data, image256Z);
            byte[] paletteUZ = LZ77.decompress(Program.ROM.Data, paletteZ);
            if (image256UZ.Length <= 0 || paletteUZ.Length <= 0)
            {
                return ImageUtil.BlankDummy();
            }

            return ImageUtil.ByteToImage256Liner(240, 160, image256UZ, 0, paletteUZ, 0);
        }

        //正しいマップチップかどうか.
        public static bool IsCorrectMapChip(int chipset_id, byte[] configUZ,bool isFE6)
        {
            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= configUZ.Length)
            {//不正なTSA
                return false;
            }

            int terrain_offset = ((chipset_id >> 3) * 2) + CHIPSET_SEP_BYTE;
            if (terrain_offset + 1 >= configUZ.Length)
            {//変なデータ
                return false;
            }
            uint terrain_data;
            if ((chipset_id & 0x4) > 0)
            {
                terrain_data = configUZ[terrain_offset + 1];
            }
            else
            {
                terrain_data = configUZ[terrain_offset];
            }

            if (isFE6)
            {
                if (terrain_data > 50)
                {
                    return false;
                }
            }
            else
            {
                if (terrain_data > 64)
                {
                    return false;
                }
            }
            return true;
        }

        public static Bitmap DrawOneChipset(int chipset_id, byte[] configUZ, Bitmap MapObjImage)
        {
            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= configUZ.Length)
            {//不正なTSA
                return ImageUtil.Blank(16, 16, MapObjImage);
            }

            UInt16 lefttop = (UInt16)(configUZ[tile_tsa_index] + ((UInt16)configUZ[tile_tsa_index + 1] << 8));
            UInt16 righttop = (UInt16)(configUZ[tile_tsa_index + 2] + ((UInt16)configUZ[tile_tsa_index + 3] << 8));
            UInt16 leftbottom = (UInt16)(configUZ[tile_tsa_index + 4] + ((UInt16)configUZ[tile_tsa_index + 5] << 8));
            UInt16 rightbottom = (UInt16)(configUZ[tile_tsa_index + 6] + ((UInt16)configUZ[tile_tsa_index + 7] << 8));

            UInt16[] tsa = new UInt16[4];
            tsa[0] = lefttop;
            tsa[1] = righttop;
            tsa[2] = leftbottom;
            tsa[3] = rightbottom;

            Bitmap mapcanvas = ImageUtil.Blank(16, 16, MapObjImage);
            ImageUtil.BitBltTSA(mapcanvas, MapObjImage, tsa, 0);

            return mapcanvas;
        }

        //チップセット解釈の分かれ目  0x2000 まではチップセットの配置 0x2000以降は地形の種類の解釈が入る
        public const int CHIPSET_SEP_BYTE = 0x2000;
        public const int CHIPSET_MAX_SIZE = CHIPSET_SEP_BYTE + ((0xFFF >> 3) * 2);

        public static uint GetChipsetID(int chipset_id, byte[] configUZ)
        {
            //たとえば、そのチップセットが 0x208 だったとして
            //
            //その地形が何であるかは、 
            //チップセット[ ((0x208 >> 3)*2) + 0x2000 ] より1*2バイト
            //struct terrain
            //{
            //    u8 terrain_data1;
            //    u8 terrain_data2;
            //};
            //terrain_data とは、草原や宝箱みたいに地形を1バイトで表現する.
            //
            //2つあるデータのうちどちらを参照するかは、 マップチップの 0x4 ビット目で判別する.
            // if ( 0x208 & 0x4 ) terrain_data2
            // else               terrain_data1  
            if (null == configUZ)
            {//変なデータ
                return U.NOT_FOUND;
            }

            int terrain_offset = ((chipset_id >> 3) * 2) + CHIPSET_SEP_BYTE;
            if (terrain_offset + 1 >= configUZ.Length)
            {//変なデータ
                return U.NOT_FOUND;
            }
            uint terrain_data;
            if ((chipset_id & 0x4) > 0)
            {
                terrain_data = configUZ[terrain_offset + 1];
            }
            else
            {
                terrain_data = configUZ[terrain_offset];
            }
            return terrain_data;
        }


        //OBJECTの割り当て
        public static uint PreciseObjectArea(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.OBJECT);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise ObjectArea", mapid.ToString("X"));

            //OBJECT領域を新規に割り当てる.
            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereMapID(mapid);
            Bitmap bmp;
            if (plists.obj_plist >= 0x100)
            {//2つのplistが必要だからコピーできない
                bmp = ImageUtil.Blank(32 * 8, 32 * 8);
            }
            else
            {
                bmp = DrawMapChipOnly(plists.obj_plist, plists.palette_plist, null);
            }

            byte[] data = ImageUtil.ImageToByte16Tile(bmp, bmp.Width, bmp.Height);
            data = LZ77.compress(data);

            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.OBJECT, plist, write_addr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            Program.Undo.Push(undodata);

            return plist;
        }


        //CONFIGの割り当て
        public static uint PreciseConfigArea(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.CONFIG);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise ConfigArea", mapid.ToString("X"));

            //CONFIG領域を新規に割り当てる.
            byte[] data = new byte[CHIPSET_MAX_SIZE];
            data = LZ77.compress(data);

            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereMapID(mapid);
            uint config_addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CONFIG, plists.config_plist);
            if (U.isSafetyOffset(config_addr))
            {//既存configがあればコピーする.
                uint length = LZ77.getCompressedSize(Program.ROM.Data, config_addr);
                data = Program.ROM.getBinaryData(config_addr, length);
            }

            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.CONFIG, plist, write_addr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            Program.Undo.Push(undodata);

            return plist;
        }

        //パレットの割り当て
        public static uint PrecisePaletteArea(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.PALETTE);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise PaletteArea", mapid.ToString("X"));


            //パレット領域を新規に割り当てる.
            byte[] data = new byte[5 * 2 * 16];

            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereMapID(mapid);
            uint palette_addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, plists.palette_plist);
            if (U.isSafetyOffset(palette_addr))
            {//既存パレットがあればコピーする.
                data = Program.ROM.getBinaryData(palette_addr, data.Length);
            }

            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.PALETTE, plist, write_addr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            Program.Undo.Push(undodata);

            return plist;
        }

        //マップ領域のベース領域の強制割り当て
        public static uint PreciseMapDataArea(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.MAP);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise MapDataArea", mapid.ToString("X"));


            //マップ領域を新規に割り当てる
            byte[] data = new byte[2 + (15 * 10)];
            data[0] = 15;
            data[1] = 10;

            data = LZ77.compress(data);

            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereMapID(mapid);
            uint map_addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.MAP, plists.mappointer_plist);
            if (U.isSafetyOffset(map_addr))
            {//既存マップがあればコピーする.
                uint length = LZ77.getCompressedSize(Program.ROM.Data, map_addr);
                data = Program.ROM.getBinaryData(map_addr, length);
            }

            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.MAP, plist, write_addr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            Program.Undo.Push(undodata);

            return plist;
        }

        //マップの最大値を取得.
        public static uint GetLimitMapWidth(int height)
        {
            return GetLimitMapWidth((uint)height);
        }
        public static uint GetLimitMapWidth(uint height)
        {
            if (height < 10)
            {
                return 0;
            }
            if (height > 63)
            {
                return 0;
            }
            return MapWidthLimit[height - 10];
        }

        static uint[] MapWidthLimit = new uint[]{
63,	//10
63,	//11
63,	//12
63,	//13
63,	//14
63,		//15
63,		//16
63,		//17
63,		//18
63,		//19
63,		//20
63,		//21
63,		//22
63,		//23
63,		//24
62,		//25
60,		//26
58,		//27
56,		//28
54,		//29
52,		//30
49,		//31
48,		//32
47,		//33
46,		//34
44,		//35
43,		//36
42,		//37
41,		//38
40,		//39
38,		//40
37,		//41
36,		//42
35,		//43
34,		//44
34,		//45
33,		//46
32,		//47
32,		//48
30,		//49
30,		//50
29,		//51
28,		//52
28,		//53
27,		//54
27,		//55
26,		//56
26,		//57
25,		//58
25,		//59
24,		//60
24,		//61
23,		//62
23,		//63
};
        public static Bitmap DrawWorldMapEvent()
        {
            return DrawWorldMapEvent(
                Program.ROM.p32(Program.ROM.RomInfo.worldmap_event_image_pointer()) , 
                Program.ROM.p32(Program.ROM.RomInfo.worldmap_event_palette_pointer()) , 
                Program.ROM.p32(Program.ROM.RomInfo.worldmap_event_tsa_pointer())
                ) ; 
        }

        //イベント用のワールドマップを描画する
        public static Bitmap DrawWorldMapEvent(
              uint image
            , uint palette     //通常パレット
            , uint tsa  //TSA
            )
        {
            image = U.toOffset(image);
            palette = U.toOffset(palette);
            tsa = U.toOffset(tsa);
            if (!U.isSafetyOffset(image))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(palette))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(tsa))
            {
                return ImageUtil.BlankDummy();
            }
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
            byte[] tsaUZ = LZ77.decompress(Program.ROM.Data, tsa);

            return ImageUtil.ByteToImage16TileHeaderTSA(256, 160
                , imageUZ, 0
                , Program.ROM.Data, (int)palette
                , tsaUZ, 0
                );
        }

        public static byte[] ConvertTSAToMapConfig(byte[] tsa)
        {
            byte[] config = new byte[0x2400];

            //TSAは横に並んでいるので、4マスこどに並べなおす必要がある
            //TSA1 TSA2 TSA3 TSA4
            //-->
            //TSA1 TSA2 TSA40 TSA41

            //TSAは横に並んでいるので、4マスこどに並べなおす必要がある
            //TSA1 TSA2 TSA3 TSA4
            //-->
            //TSA1 TSA2 TSA40 TSA41

            int srcx = 0;
            int destx = 0;
            int innerx = 0;
            while (srcx < tsa.Length)
            {
                if (innerx >= 0x80)
                {
                    innerx = 0;
                    srcx += 0x80;
                    destx += 0x0;
                }
                int srcy = srcx / 0x80;
                int n = ((srcy + 1) * 0x80) + (innerx);
                if (n >= tsa.Length)
                {
                    break;
                }
                if (destx >= 0x2000)
                {
                    break;
                }
                config[destx] = tsa[srcx];
                config[destx + 1] = tsa[srcx + 1];
      
                config[destx + 2] = tsa[srcx + 2];
                config[destx + 3] = tsa[srcx + 3];
                
                config[destx + 4] = tsa[n];
                config[destx + 5] = tsa[n + 1];
                
                config[destx + 6] = tsa[n + 2];
                config[destx + 7] = tsa[n + 3];

                destx += 8;
                srcx += 4;
                innerx += 4;
            }
            for (int i = 0x2000; i < config.Length; i++)
            {
                config[i] = 0x01; //草原
            }

            return config;
        }

#if DEBUG
        public static void TEST_ConvertTSAToMapConfig_1()
        {
            byte[] tsa = File.ReadAllBytes( Program.GetTestData("plain.mapchip_config") );
            byte[] r = ConvertTSAToMapConfig(tsa);
            Debug.Assert(r[0x0] == 0x00);
            Debug.Assert(r[0x1] == 0x00);
            Debug.Assert(r[0x2] == 0x01);
            Debug.Assert(r[0x3] == 0x00);
            Debug.Assert(r[0x4] == 0x40);
            Debug.Assert(r[0x5] == 0x00);

            Debug.Assert(r[0x100] == 0x80);
            Debug.Assert(r[0x102] == 0x81);
            Debug.Assert(r[0x104] == 0xc0);
        }
#endif
    }
}
