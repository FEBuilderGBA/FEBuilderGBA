using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    //バトルアニメ
    class ImageUtilOAM
    {
        public const int SEAT_TILE_WIDTH = 256 / 8;
        public const int SEAT_TILE_HEIGHT = 64 / 8;

        public const int SEAT_MAGIC_TILE_HEIGHT = 32 / 8; //魔法だと使えるタイル数が半分になる.
        public const int SEAT_BORDERAP_TILE_HEIGHT = 40 / 8; //国境AP.

        public const int SCREEN_TILE_WIDTH = 248 / 8;
        public const int SCREEN_TILE_HEIGHT = 160 / 8;
        public const int SCREEN_TILE_APPENDMODE2_WIDE = 488 / 8;

        public const int SCREEN_TILE_WIDTH_M1 = 240 / 8;


        const byte square = 0;
        const byte horizontal = (byte)(1 << 6);
        const byte vertical = (byte)(2 << 6);
        const byte times1 = 0;
        const byte times2 = (byte)(1 << 6);
        const byte times4 = (byte)(2 << 6);
        const byte times8 = (byte)(3 << 6);

        const int tile_scale = 1;
        const int bitmap_addx = 0x94;
        const int bitmap_addy = 0x58;

        const int bitmap_spell_addx = 0xAC;


        public static Bitmap DrawBattleAnime(uint showSectionData, uint showFrameData, uint sectionData, uint frameData, uint rightToLeftOAM, uint palettes)
        {
            uint sectionData_offset = U.toOffset(sectionData); //int[0xC]個 ヘッダの区切りバイト 絶対値

            //解凍する.
            //固定長のsectionData以外はLZ77で圧縮されている.
            byte[] frameData_UZ = UnCompressFrame(frameData); //画像をどう切り出すかを提起したデータ
            byte[] rightToLeftOAM_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(rightToLeftOAM)); //OAM
            byte[] palettes_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(palettes)); //Palette
            if (frameData_UZ.Length <= 0 || rightToLeftOAM_UZ.Length <= 0 || palettes_UZ.Length <= 0)
            {
                return ImageUtil.BlankDummy();
            }

            //sectionDataデータは、 int sectionData[0x0C] として、
            //0xC個場合の、frameData_UZへの参照位置を返します.
            uint frameDataStart;
            uint frameDataEnd;

            if (showSectionData >= 0xC
                || showSectionData == 0x2 - 1 || showSectionData == 0x4 - 1)
            {//とりあえず全部だすか
                frameDataStart = 0;
                frameDataEnd = (uint)frameData_UZ.Length;
            }
            else 
            {//mode0  - modeC
                getSectionDataStartEnd(showSectionData, sectionData_offset, (uint)frameData_UZ.Length
                    , out frameDataStart, out frameDataEnd);
            }

            //frameDataは4バイトずつのデータからなり、
            //?? ?? ?? 0x86 のあとには、4バイトの画像へのポインタと、 4バイトの 変換する OAMへの絶対位置を返します.
            uint frame = FindFrame(showFrameData, frameDataStart, frameDataEnd, frameData_UZ);
            if (frame == U.NOT_FOUND)
            {
                return ImageUtil.Blank(SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, palettes_UZ, 0);
            }
            Bitmap bitmap = DrawFrameImage
                (frame, frameData_UZ, palettes_UZ, rightToLeftOAM_UZ);

            if (IsSeet01or03(showSectionData))
            {//mode1 と mode3 は特別.  mode1は mode2と、 mode3は mode4 と、ともに利用する.
                if (IsNoWeaponAnimation(sectionData_offset, frameData_UZ))
                {//武器をもっていないモーションなので、0x01と0x03のような貫通している武器モーションは存在しない.
                    return bitmap;
                }
                uint frameDataStart2,frameDataEnd2;
                getSectionDataStartEnd(showSectionData + 1, sectionData_offset, (uint)frameData_UZ.Length
                    , out frameDataStart2, out frameDataEnd2);
                Debug.Assert(frameDataStart2 == Program.ROM.u32((showSectionData + 1) * 4 + sectionData_offset));
                Debug.Assert(frameDataEnd2 == Program.ROM.u32((showSectionData+ 1 + 1) * 4 + sectionData_offset));

                uint frame2 = FindFrame(showFrameData, frameDataStart2, frameDataEnd2, frameData_UZ);
                if (frame2 != U.NOT_FOUND)
                {
                    Bitmap bitmap2 = DrawFrameImage
                        (frame2, frameData_UZ, palettes_UZ, rightToLeftOAM_UZ);
                    ImageUtil.BitBlt(bitmap2, 0, 0, bitmap2.Width, bitmap2.Height, bitmap, 0, 0, 0, 0x00);
                    return bitmap2;
                }
            }
            return bitmap;
        }

        //武器を持っていないモーションのアニメーションか?
        //ROMにある武器をもっていないアニメーションは、00と01に共通のデータが設定されている。
        //普通、問題にはならないが、0x6Aファルコンナイトのように1タイルずれているものがある
        //そういうものを表示しないように、
        //武器をもっていないモーション(C02が先頭に定義されているもの)は、
        //01と03のアニメーションの表示をカットする.
        static bool IsNoWeaponAnimation(uint frameDataStart, uint frameDataEnd, byte[] frameData_UZ)
        {
            //frameDataは4バイトずつのデータからなり、
            //?? ?? ?? 0x86 のあとには、4バイトの画像へのポインタと、 4バイトの 変換する OAMへの絶対位置を返します.
            uint frameI = 0;
            for (uint n = frameDataStart; n < frameDataEnd; n += 4)
            {
                if (n + 3 >= frameData_UZ.Length)
                {
                    Debug.Assert(false);
                    break;
                }
                if (frameData_UZ[n + 3] == 0x85)
                {//85command
                    byte command = frameData_UZ[n];
                    if (command == 0x02)
                    {//回避モーションの定義
                        return true;
                    }
                    continue;
                }

                if (frameData_UZ[n + 3] != 0x86) //0x86 pointer
                {
                    continue;
                }

                if (1 <= frameI)
                {
                    return false;
                }

                frameI++;
                n += 8; // 8+4 = 12bytes
            }
            return false;
        }
        static bool IsNoWeaponAnimation(uint sectionData_offset, byte[] frameData_UZ)
        {
            uint frameDataStart, frameDataEnd;
            getSectionDataStartEnd(0, sectionData_offset, (uint)frameData_UZ.Length
                , out frameDataStart, out frameDataEnd);
            return IsNoWeaponAnimation(frameDataStart, frameDataEnd, frameData_UZ);
        }
        
        static uint FindFrame(uint showFrameData, uint frameDataStart, uint frameDataEnd, byte[] frameData_UZ)
        {
            //frameDataは4バイトずつのデータからなり、
            //?? ?? ?? 0x86 のあとには、4バイトの画像へのポインタと、 4バイトの 変換する OAMへの絶対位置を返します.
            uint frameI = 0;
            for (uint n = frameDataStart; n < frameDataEnd; n += 4)
            {
                if (n + 3 >= frameData_UZ.Length)
                {
                    Debug.Assert(false);
                    break;
                }

                if (frameData_UZ[n + 3] != 0x86) //0x86 pointer
                {
                    continue;
                }

                if (showFrameData == frameI)
                {
                    return n;
                }

                frameI++;
                n += 8; // 8+4 = 12bytes
            }
            return U.NOT_FOUND;
        }

        static Bitmap DrawFrameImage(uint frame, byte[] frameData_UZ, byte[] palettes_UZ, byte[] rightToLeftOAM_UZ)
        {
            Bitmap retImage = ImageUtil.Blank(SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, palettes_UZ, 0);

            // Graphics referred to by the frame data. Each graphic
            // is byte-serialized; it's a 256x64, 16-colour bitmap
            // (8192 bytes).
            uint graphicsPointer = U.u32(frameData_UZ, frame + 4);
            byte[] graphics_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(graphicsPointer));
            Bitmap graphicsSeet = ImageUtil.ByteToImage16Tile(256, 64, graphics_UZ, 0, palettes_UZ, 0);

            //利用するOAMデータの開始位置
            //OAMは 12byte で 最初の1バイト目が 1になるまで続きます.
            uint OAMAbsoStart = U.u32(frameData_UZ, frame + 8);
            DrawOAM oam = new DrawOAM();
            oam.Parse(rightToLeftOAM_UZ, OAMAbsoStart);
            oam.Draw(retImage, graphicsSeet);

            return retImage;
        }

        public class DrawOAM
        {
            public class OAM_Data
            {
                public bool sizeDouble; // 回転する。回転した結果 width,heightからはみ出た領域は切り捨てる
                public bool rotation;   // 回転する。ただし、転送する枠width,heightでとれるやつを2倍と仮定して計算する.回転ではみ出る領域が少なくなる.
                public int affineNum;   // 回転に利用する affine行列[N] 0-31 合計32個
                public bool v_flipped;  // 垂直に反転する
                public bool h_flipped;  // 水平に反転する
                public int paletteShift; //利用するパレット0-3
                public int width;       // コピーするシートにあるオブジェクト パーツの幅
                public int height;      // コピーするシートにあるオブジェクト パーツの高さ
                public int sheet_x;     // シートのどこを切り抜くかのX位置 sheet_x,sheet_y ～ width ,height の範囲を切り抜いて、画面の image_x,image_y にコピーします
                public int sheet_y;
                public int image_x;     // 画面X
                public int image_y;     // 画面Y
            };
            List<OAM_Data> translist = new List<OAM_Data>();

            public class OAM_Affine
            {
                public double angle;
                public double xScaling;
                public double yScaling;
            };
            OAM_Affine[] affine = new OAM_Affine[32];


            bool IsMagicOAM; //追加魔法のOAM
            public void SetIsMagicOAM(bool isMagicOAM)
            {
                this.IsMagicOAM = isMagicOAM;
            }

            public void Parse(byte[] oam, uint start)
            {
                for (uint index = start; ; index += 12)
                {
                    if (!parseOAM(oam, index))
                    {
                        break;
                    }
                }
            }

            void parseAffineOAM(byte[] oam, uint index)
            {
                //固定小数点 
                //Bit   Expl.
                //0-7   Fractional portion (8 bits)   小数部
                //8-14  Integer portion    (7 bits)   整数部
                //15    Sign               (1 bit)    符号
                double PA = ((double)((short)U.u16(oam, index + 4))) / 256; //4000020h - BG2PA - BG2 Rotation/Scaling Parameter A (alias dx) (W)
                double PB = ((double)((short)U.u16(oam, index + 6))) / 256; //4000022h - BG2PB - BG2 Rotation/Scaling Parameter B (alias dmx) (W)
                double PC = ((double)((short)U.u16(oam, index + 8))) / 256; //4000024h - BG2PC - BG2 Rotation/Scaling Parameter C (alias dy) (W)
                double PD = ((double)((short)U.u16(oam, index + 10))) / 256; //4000026h - BG2PD - BG2 Rotation/Scaling Parameter D (alias dmy) (W)

                double angle = U.RadianToDegree(Math.Atan(PB / PA));
                if (PA < 0 && PB < 0) { angle -= 180; }
                if (PA < 0 && PB > 0) { angle += 180; }
                if (double.IsInfinity(angle) || double.IsNaN(angle))
                {
                    angle = 0;
                }

                // Calculate the scaling factors
                double xmag = Math.Cos(U.DegreeToRadian(angle)) / PA;							// calc for x_scaling
                if (xmag >= 10) { xmag = Math.Abs(Math.Sin(U.DegreeToRadian(angle)) / PB); }	// used if angle is invalid over cos
                double ymag = Math.Cos(U.DegreeToRadian(angle)) / PD;							// calc for y_scaling //?
                if (ymag >= 10) { ymag = Math.Abs(Math.Sin(U.DegreeToRadian(angle)) / PC); }	// used if angle is invalid over cos

                if (double.IsInfinity(xmag) || double.IsNaN(xmag))
                {
                    xmag = 1;
                }
                if (double.IsInfinity(ymag) || double.IsNaN(ymag))
                {
                    ymag = 1;
                }

                int affineNum = (oam[index+0] & 0x1F) - 1;
                if (affineNum < 0)
                {
                    affineNum = 0;
                }
                OAM_Affine a = new OAM_Affine();
                a.angle = angle;
                a.xScaling = xmag;
                a.yScaling = ymag;
                this.affine[affineNum] = a;
            }

            bool parseOAM(byte[] oam, uint index)
            {
                if (oam.Length < index + 12)
                {//謎終端 データ不足 壊れているデータ?
                    return false;
                }
                if (oam[index] == 0 && oam[index + 1] == 0xFF && oam[index + 2] == 0xFF && oam[index + 3] == 0xFF)
                {//FEditorシリアライズを読み込んだときは別終端がある?
                    return false;
                }

                if (oam[index + 2] == 0xFF && oam[index + 3] == 0xFF)
                {//2バイト目と3バイト目が FFだったら 別ルーチン
                    parseAffineOAM(oam,index);
                    return true;
                }
                if (oam[index] == 1)
                {//終端
                    return false;
                }

                if (oam[index + 0] != 0)
                {//終端ではないのに、 0x00 が先頭にあるバグっているOAMデータ
                    //Debug.Assert(oam[index + 0] == 0);
                    //Debug.Assert(false);
                    return false;
                }

                byte align = oam[index + 1];
                bool rotation = (align & 0x01) == 0x01;		// 回転するはみ出た領域は切り捨てる
                bool sizeDouble = (align & 0x02) == 0x02;	// 回転する。ただし、転送する枠convertAlignAreaToWHでとれるやつを2倍にする。回転ではみ出る領域が少なくなる.

                byte area = oam[index + 3];             //転送する範囲 convertAlignAreaToWHで解読する.
                bool v_flipped = (area & 0x10) == 0x10;	// 垂直に反転する
                bool h_flipped = (area & 0x20) == 0x20;	// 水平に反転する


                int affineNum = area & 0x1F; //アフィン移動に利用する行列
                int paletteShift = (oam[index + 5] >> 4) & 0xF; //利用するパレット0-3

                //幅高さの取得
                int width,height;
                convertAlignAreaToWH(align,area,out width,out height);

                int sheet_x = (oam[index + 4] & 0x1F);
                int sheet_y = (((oam[index + 4]) & 0xE0) >> 5);

//                int vram_x = (sbyte)(oam[index + 6]);
//                int vram_y = (sbyte)(oam[index + 8]);
                int vram_x = (short)U.u16(oam, index + 6);
                int vram_y = (short)U.u16(oam, index + 8);

                int image_x;
                int image_y = ((vram_y + bitmap_addy) / tile_scale);
                if (this.IsMagicOAM)
                {
                    image_x = ((vram_x + bitmap_spell_addx) / tile_scale);
                }
                else
                {
                    image_x = ((vram_x + bitmap_addx) / tile_scale);
                }
                if (image_x >= 256)
                {
                    image_x = image_x & 0xff;
                }

                if (paletteShift >= 4)
                {//バグフレーム
#if DEBUG
                    Log.Debug("OAM BUG FRAME:", image_x.ToString(), image_y.ToString(), "sheet", sheet_x.ToString(), sheet_y.ToString(), "w:", width.ToString(), "h:", height.ToString(), "rotation:", rotation.ToString(), "sizeDouble:", sizeDouble.ToString(), "affineNum:", affineNum.ToString(), "v_flipped:", v_flipped.ToString(), "h_flipped:", h_flipped.ToString(), "paletteShift:", paletteShift.ToString(), "BIN:", oam[index + 0].ToString(), oam[index + 1].ToString(), oam[index + 2].ToString(), oam[index + 3].ToString(), oam[index + 4].ToString(), oam[index + 5].ToString(), oam[index + 6].ToString(), oam[index + 7].ToString(), oam[index + 8].ToString(), oam[index + 9].ToString(), oam[index + 10].ToString(), oam[index + 11].ToString());
#endif
                    return true;
                }

                OAM_Data trans = new OAM_Data();
                trans.width = width * 8;
                trans.height = height * 8;
                trans.image_x = image_x * 1;
                trans.image_y = image_y * 1;
                trans.sheet_x = sheet_x * 8;
                trans.sheet_y = sheet_y * 8;
                trans.rotation = rotation;
                trans.sizeDouble = sizeDouble;
                trans.affineNum = affineNum;
                trans.v_flipped = v_flipped;
                trans.h_flipped = h_flipped;
                trans.paletteShift = paletteShift; //利用するパレット0-3
                translist.Add(trans);

#if DEBUG
//                Log.Debug("image", image_x.ToString(), image_y.ToString(), "sheet", sheet_x.ToString(), sheet_y.ToString(), "w:", width.ToString(), "h:", height.ToString(),"rotation:",rotation.ToString(),"sizeDouble:",sizeDouble.ToString(),"affineNum:",affineNum.ToString(),"v_flipped:",v_flipped.ToString(),"h_flipped:",h_flipped.ToString(),"paletteShift:",paletteShift.ToString(),"BIN:", oam[index + 0].ToString(), oam[index + 1].ToString(), oam[index + 2].ToString(), oam[index + 3].ToString(), oam[index + 4].ToString(), oam[index + 5].ToString(), oam[index + 6].ToString(), oam[index + 7].ToString(), oam[index + 8].ToString(), oam[index + 9].ToString(), oam[index + 10].ToString(), oam[index + 11].ToString());
#endif
                return true;
            }

            public static void convertAlignAreaToWH(uint align ,uint area ,out int width,out int height)
            {
                align &= 0xC0;
                area &= 0xC0;

                width = 0;
                height = 0;

                if (area == times8)
                {//times8: // area 64 or 32
                    if (align == vertical) { width = 4; height = 8; }
                    else if (align == horizontal) { width = 8; height = 4; }
                    else if (align == square) { width = 8; height = 8; }
                }
                else if (area == times4)
                {//times4: // area 16 or 8
                    if (align == vertical) { width = 2; height = 4; }
                    else if (align == horizontal) { width = 4; height = 2; }
                    else if (align == square) { width = 4; height = 4; }
                }
                else if (area == times2)
                {//times2: // area 4
                    if (align == vertical) { width = 1; height = 4; }
                    else if (align == horizontal) { width = 4; height = 1; }
                    else if (align == square) { width = 2; height = 2; }
                }
                else if (area == times1)
                {//times1: // area 2 or 1
                    if (align == vertical) { width = 1; height = 2; }
                    else if (align == horizontal) { width = 2; height = 1; }
                    else if (align == square) { width = 1; height = 1; }
                }
            }

            public string Lint(byte[] oam, uint start)
            {
                string errorMessage = "";
                for (uint index = start; ; index += 12)
                {
                    if (!checkOAM(oam, index, out errorMessage))
                    {
                        break;
                    }
                }
                return errorMessage;
            }
            bool checkOAM(byte[] oam, uint index, out string out_errorMessage)
            {
                if (oam.Length < index || oam.Length < index + 12)
                {//謎終端 データ不足 壊れているデータ?
                    out_errorMessage = R.Error("OAMが壊れています。データ長が範囲外を参照しています。 OAMサイズ:{0} 参照:{1}", oam.Length, index);
                    return false;
                }
                if (oam[index] == 0 && oam[index + 1] == 0xFF && oam[index + 2] == 0xFF && oam[index + 3] == 0xFF)
                {//FEditorシリアライズを読み込んだときは別終端がある?
                    out_errorMessage = "";
                    return false;
                }

                if (oam[index + 2] == 0xFF && oam[index + 3] == 0xFF)
                {//2バイト目と3バイト目が FFだったら 別ルーチン
                    out_errorMessage = "";
                    return true;
                }
                if (oam[index] == 1)
                {//終端
                    out_errorMessage = "";
                    return false;
                }

                if (oam[index + 0] != 0)
                {//終端ではないのに、 0x00 が先頭にあるバグっているOAMデータ
                    out_errorMessage = R.Error("OAMの先頭データが0x00または0x01ではありません。おそらく、データが破損しています。 OAMサイズ:{0} 参照:{1} Hex:{2}"
                        , oam.Length, index, U.HexDumpLiner(oam, index, 12));
                    return false;
                }

                out_errorMessage = "";
                return true;
            }


            public Bitmap Draw(Bitmap retpic, Bitmap graphicsSeetPic)
            {
                //OAMは逆順に描画する
                for (int i = translist.Count - 1; i >= 0 ; i--)
                {
                    OAM_Data t = translist[i];
                    if (this.affine[t.affineNum] != null && (t.rotation || t.sizeDouble))
                    {//拡大回転などの特殊な処理をする場合

                        if (t.width <= 0 || t.height <= 0)
                        {
                            continue;
                        }

                        Bitmap parts = ImageUtil.Blank(t.width, t.height, retpic);

                        //シートから、切り出した画像(反転等があるので)
                        ImageUtil.BitBlt(parts
                            , 0, 0
                            , t.width
                            , t.height
                            , graphicsSeetPic
                            , t.sheet_x, t.sheet_y
                            , 0
                            , 0xff
                            , t.v_flipped
                            , t.h_flipped
                        );

                        Bitmap a;
                        try
                        {
                            a = ImageUtil.AffineTransform(parts, this.affine[t.affineNum].angle, this.affine[t.affineNum].xScaling, this.affine[t.affineNum].yScaling);
                        }
                        catch(Exception e)
                        {
                            //C#はなぜか例外を投げる時がある 原因不明
                            throw new Exception(R.Error("ImageUtil.AffineTransform( ({0},{1}),{2},{3},{4})", parts.Width , parts.Height, this.affine[t.affineNum].angle, this.affine[t.affineNum].xScaling, this.affine[t.affineNum].yScaling), e);
                        }

                        int singleRotateAddX=0;
                        int singleRotateAddY=0;
                        if (t.sizeDouble == false)
                        {
                            //見栄えを考慮して、常にsizeDoubleで計算するので、sizeDoubleではないものは補正が必要.
                            singleRotateAddX = -(t.width / 2);
                            singleRotateAddY = -(t.height / 2);
                        }

                        ImageUtil.BitBlt(retpic
                            , t.image_x + singleRotateAddX
                            , t.image_y + singleRotateAddY  
                            , a.Width
                            , a.Height
                            , a
                            , 0, 0
                            , 0
                            , 0
                        );
                    }
                    else
                    {//拡大回転をしない場合、単純にコピーするだけでよい
                        ImageUtil.BitBlt(retpic
                            , t.image_x, t.image_y
                            , t.width
                            , t.height
                            , graphicsSeetPic
                            , t.sheet_x, t.sheet_y
                            , t.paletteShift
                            , 0
                            , t.v_flipped
                            , t.h_flipped
                        );

#if DEBUG
//                        Log.Debug("OAM BitBlt", i.ToString(), "dest", t.image_x.ToString(), t.image_y.ToString(), "w:", t.width.ToString(), "h:", t.height.ToString(), "src:", t.sheet_x.ToString(), t.sheet_y.ToString(), "vh", t.v_flipped.ToString(), t.h_flipped.ToString());
//                        graphicsSeetPic.Save("bb" + i.ToString("X00") + ".png");
//                        retpic.Save("aa" + i.ToString("X00") + ".png");
#endif
                    }

                }
                return retpic;
            }

        }


        public class ImportOAM
        {
            string BaseDir;
            public void SetBaseDir(string basedir)
            {
                this.BaseDir = basedir;
            }

            bool IsMagicOAM; //追加魔法のOAM パレットがシートごとにあるのが違い.
            public void SetIsMagicOAM(bool isMagicOAM)
            {
                this.IsMagicOAM = isMagicOAM;
            }

            bool IsBorderAPOAM; //国境AP用のOAMデータ
            public void SetIsBorderAPOAM(bool isBorderAPOAM)
            {
                this.IsBorderAPOAM = isBorderAPOAM;
            }

            bool IsMultiPaletteOAM; //複数のパレットを許可するOAMかどうか
            public void SetIsMultiPaletteOAM(bool isMultiPaletteOAM)
            {
                this.IsMultiPaletteOAM = isMultiPaletteOAM;
            }

            static bool[] MakeUseTileData(byte[] image, int width, int height)
            {
                //画像で利用しているタイルをマークするバッファを作成します.
                bool[] useTileData = new bool[(width / 8) * (height / 8)];
                //空白は利用しないので利用しているマークを立てて無視します.
                int end = useTileData.Length;
                for (int i = 0; i < end; i++)
                {
                    int n = i * (8 / 2 * 8);
                    int n_end = (i + 1) * (8 / 2 * 8);
                    for (; n < n_end; n++)
                    {
                        if (image[n] != 0)
                        {//画像データがある.
                            break;
                        }
                    }
                    if (n >= n_end)
                    {//空白データ
                        useTileData[i] = true;
                    }
                }

                //右上にはカラーパレットマップ(FEditorAdv準拠)があるので無視するため、空白フラグを立てる.
                useTileData[(width / 8) - 1] = true;

                return useTileData;
            }

            List<byte> RightToLeftOAM;
            List<byte> RightToLeftOAMBG;
            byte[] Palette;
            List<image_data> Images;
            Bitmap SeatBitmap;
            bool[] SeatUsingFlags;

            public string ErrorMessage { get; private set; }
            public ImportOAM()
            {
                RightToLeftOAM = new List<byte>();
                RightToLeftOAMBG = new List<byte>();
                Palette = new byte[0x20];
                Images = new List<image_data>();
                SeatBitmap = null;
                SeatUsingFlags = null;
            }

            static string GetFullPath(string imagefilename,string basedir)
            {
                return Path.Combine(basedir, imagefilename);
            }
            public animedata MakeBattleAnime(string imagefilename, bool isMode2)
            {
                Debug.Assert(this.IsMagicOAM != true);

                string errormessage;
                Bitmap paletteHint;
                if (this.SeatBitmap != null)
                {//現在のシートが有効ならば、そのシートのパレットをヒントに使う.
                    paletteHint = this.SeatBitmap;
                }
                else if (this.Images.Count <= 0)
                {//過去に1つも画像を読みこんでいなければ、パレットもないのでヒントを作りようがない
                    paletteHint = null;
                }
                else
                {//過去に画像を読みこんでいるということはヒントを作れるはず
                    if (U.IsNullArray(this.Palette))
                    {//なぜかパレットがすべてnullだった.
                        Debug.Assert(false);
                        paletteHint = null;
                    }
                    else
                    {//過去に取得したパレットがあるので、そのパレットを再利用する.
                        paletteHint = ImageUtil.Blank(8, 8, this.Palette, 0);
                    }
                }

                Bitmap bitmap = ImageUtil.OpenBitmap(GetFullPath(imagefilename, this.BaseDir), paletteHint, out errormessage);
                if (bitmap == null)
                {
                    this.ErrorMessage = errormessage + R._("\r\n画像ファイル名:{0}"
                        , Path.GetFileName(imagefilename)
                    );
                    return null;
                }

                animedata r;
                if (isMode2)
                {
                    Bitmap seatBitmapBackup = ImageUtil.CloneBitmap(SeatBitmap);
                    int oamBackupPostion = this.RightToLeftOAM.Count;

                    r = MakeBattleAnime(bitmap, false, this.RightToLeftOAM);
                    if (r == null)
                    {
                        return null;
                    }
                    animedata objbg = MakeBattleAnime(bitmap, true, this.RightToLeftOAM);
                    if (objbg == null)
                    {
                        return null;
                    }
                    Log.Debug(Path.GetFileNameWithoutExtension(imagefilename) + " " + r.oam_pos + " " + objbg.oam_pos);

                    if (r.image_pointer != objbg.image_pointer)
                    {//同一シートにないといけない
                        SeatBitmap = seatBitmapBackup;
                        this.RightToLeftOAM.RemoveRange(oamBackupPostion, this.RightToLeftOAM.Count - oamBackupPostion);
                        //次のシートへ
                        NextSeat();

                        r = MakeBattleAnime(bitmap, false, this.RightToLeftOAM);
                        objbg = MakeBattleAnime(bitmap, true, this.RightToLeftOAM); //戦闘アニメの場合は、RightToLeftOAMBGではない.
                        if (r.image_pointer != objbg.image_pointer)
                        {
                            return null;
                        }
                    }

                    r.oam2_pos = objbg.oam_pos;
                }
                else
                {
                    r = MakeBattleAnime(bitmap, false, this.RightToLeftOAM);
                }

                bitmap.Dispose();
                return r;
            }

            public animedata MakeMagicAnime(string imagefilename)
            {
                Debug.Assert(this.IsMagicOAM == true);

                string errormessage;
                //魔法の場合パレットを切り替えられるので、何も気にしない.
                Bitmap paletteHint = null ;
                Bitmap bitmap = ImageUtil.OpenBitmap(GetFullPath(imagefilename, this.BaseDir), paletteHint, out errormessage);
                if (bitmap == null)
                {
                    this.ErrorMessage = errormessage + R._("\r\n画像ファイル名:{0}"
                        , Path.GetFileName(imagefilename)
                    );
                    return null;
                }

                animedata r;
                Bitmap seatBitmapBackup = ImageUtil.CloneBitmap(SeatBitmap);
                int oamBackupPostion = this.RightToLeftOAM.Count;
                int oamBGBackupPostion = this.RightToLeftOAMBG.Count;

                r = MakeBattleAnime(bitmap, false, this.RightToLeftOAM);
                if (r == null)
                {
                    this.ErrorMessage = "MakeBattleAnime OBJ1 Error" + R._("\r\n画像ファイル名:{0}"
                        , Path.GetFileName(imagefilename)
                    );
                    bitmap.Dispose();
                    return null;
                }
                animedata bg = MakeBattleAnime(bitmap, true, this.RightToLeftOAMBG);
                if (bg == null)
                {
                    this.ErrorMessage = "MakeBattleAnime BG1 Error" + R._("\r\n画像ファイル名:{0}"
                        , Path.GetFileName(imagefilename)
                    );
                    bitmap.Dispose();
                    return null;
                }
                Log.Debug(Path.GetFileNameWithoutExtension(imagefilename) + " " + r.oam_pos + " " + bg.oam_pos);

                if (r.image_pointer != bg.image_pointer)
                {//同一シートにないといけない
                    SeatBitmap = seatBitmapBackup;
                    this.RightToLeftOAM.RemoveRange(oamBackupPostion, this.RightToLeftOAM.Count - oamBackupPostion);
                    this.RightToLeftOAMBG.RemoveRange(oamBGBackupPostion, this.RightToLeftOAMBG.Count - oamBGBackupPostion);
                    //次のシートへ
                    NextSeat();

                    r = MakeBattleAnime(bitmap, false, this.RightToLeftOAM);
                    if (r == null)
                    {
                        this.ErrorMessage = "MakeBattleAnime OBJ2 Error" + R._("\r\n画像ファイル名:{0}"
                            , Path.GetFileName(imagefilename)
                        );
                        bitmap.Dispose();
                        return null;
                    }
                    bg = MakeBattleAnime(bitmap, true, this.RightToLeftOAMBG);
                    if (bg == null)
                    {
                        this.ErrorMessage = "MakeBattleAnime BG2 Error" + R._("\r\n画像ファイル名:{0}"
                            , Path.GetFileName(imagefilename)
                        );
                        bitmap.Dispose();
                        return null;
                    }
                    if (r.image_pointer != bg.image_pointer)
                    {
                        bitmap.Dispose();
                        return r;
                    }
                }

                r.oam2_pos = bg.oam_pos;

                bitmap.Dispose();
                return r;
            }
            public animedata MakeBorderAP(string imagefilename)
            {
                return MakeBattleAnime(imagefilename,isMode2: false);
            }
            void InitSeat(Bitmap bitmap)
            {
                //初手の場合、シートと、処理済み部分を初期化する.
                if (this.IsMagicOAM)
                {
                    this.SeatBitmap = ImageUtil.Blank(SEAT_TILE_WIDTH * 8, SEAT_MAGIC_TILE_HEIGHT * 8, bitmap);
                    this.SeatUsingFlags = new bool[SEAT_TILE_WIDTH * SEAT_MAGIC_TILE_HEIGHT];
                }
                else if (this.IsBorderAPOAM)
                {
                    this.SeatBitmap = ImageUtil.Blank(SEAT_TILE_WIDTH * 8, SEAT_BORDERAP_TILE_HEIGHT * 8, bitmap);
                    this.SeatUsingFlags = new bool[SEAT_TILE_WIDTH * SEAT_BORDERAP_TILE_HEIGHT];
                }
                else
                {
                    this.SeatBitmap = ImageUtil.Blank(SEAT_TILE_WIDTH * 8, SEAT_TILE_HEIGHT * 8, bitmap);
                    this.SeatUsingFlags = new bool[SEAT_TILE_WIDTH * SEAT_TILE_HEIGHT];
                }

            }

            bool IsSwapBackgroundColor = false; //背景色を入れ替えるか
            Color BackgroundColor;  //背景色
            enum PaletteResult
            {
                 Cancel
                ,OK
                ,MagicChangePalette
            };

            PaletteResult CheckAndConvertPalette(ref Bitmap bitmap, bool firsttry)
            {
                if (firsttry && this.IsSwapBackgroundColor == false)
                {
                    //透過色テスト
                    if (!ImageUtil.CheckPaletteTransparent(bitmap))
                    {
                        ErrorPaletteTransparentForm f = (ErrorPaletteTransparentForm)InputFormRef.JumpFormLow<ErrorPaletteTransparentForm>();
                        f.SetOrignalImage(bitmap);
                        f.ShowDialog();

                        int backgroundColorIndex = f.GetResultColorIndex();
                        if (backgroundColorIndex < 0)
                        {//キャンセル.
                            this.ErrorMessage = R._("透過色を認識できません");
                            return PaletteResult.Cancel;
                        }
                        this.IsSwapBackgroundColor = true;
                        this.BackgroundColor = bitmap.Palette.Entries[backgroundColorIndex];
                    }
                }
                if (this.IsSwapBackgroundColor)
                {
                    //通過色を入れ変える.
                    int backgroundColorIndex = ImageUtil.FindPaletteIndexByColor(bitmap, this.BackgroundColor);
                    if (backgroundColorIndex > 0)
                    {
                        ImageUtil.ChangeColorPixel(bitmap, (byte)0, (byte)(backgroundColorIndex));
                    }
                }

                //パレットをバイナリデータとして保存.
                byte[] imagepalette;
                if (this.IsMultiPaletteOAM)
                {
                    imagepalette = ImageUtil.ImageToPalette(bitmap, 4);
                }
                else
                {
                    imagepalette = ImageUtil.ImageToPalette(bitmap, 1);
                }

                if (firsttry)
                {
                    //初手ならば、比較するデータがないので、自分を基準とする.
                    this.Palette = (byte[])imagepalette.Clone();
                    return PaletteResult.OK;
                }
                if (U.memcmp(imagepalette, this.Palette) != 0)
                {
                    if (IsMagicOAM)
                    {//魔法の場合パレットを切り替えれば済む話である.
                        return PaletteResult.MagicChangePalette;
                    }
                    else
                    {
                        //バトルアニメの場合は、パレットは共通なのでエラーを返す.
                        string errorPaletteMessage = ImageUtil.CheckPalette(bitmap.Palette
                            , this.SeatBitmap.Palette
                            , null
                            );
                        string errorMessage = R.Error("パレットがほかと異なります\r\n{0}", errorPaletteMessage);

                        ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                        f.SetErrorMessage(errorPaletteMessage);
                        f.SetOrignalImage(ImageUtil.ReOrderPalette(bitmap, this.Palette, 0));
                        f.ShowForceButton();
                        f.ShowDialog();

                        bitmap = f.GetResultBitmap();
                        if (bitmap == null)
                        {//キャンセル.
                            this.ErrorMessage = errorMessage;
                            return PaletteResult.Cancel;
                        }
                    }
                }
                return PaletteResult.OK;
            }

            animedata MakeBattleAnime(Bitmap orignalBitmap,bool isMode2, List<byte> oam)
            {
                animedata animedata = new animedata();
                animedata.oam_pos = (uint)oam.Count;
                animedata.image_pointer = (uint)this.Images.Count;
                if (this.IsMagicOAM)
                {//魔法の場合、パレットがシートごとに切り替えられるため パレットもデータとして保存する.
                    animedata.palette_pointer = (uint)(this.Images.Count + 1);
                }
                Bitmap bitmap;

                if (isMode2)
                {
                    if (orignalBitmap.Width >= (SCREEN_TILE_APPENDMODE2_WIDE - SCREEN_TILE_WIDTH) * 8 
                        && orignalBitmap.Height >= SCREEN_TILE_HEIGHT * 8)
                    {
                        bitmap = ImageUtil.Copy(orignalBitmap
                            , (SCREEN_TILE_APPENDMODE2_WIDE - SCREEN_TILE_WIDTH) * 8
                            , 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8);
                    }
                    else
                    {
                        AppendTermOAM(oam);
                        return animedata;
                    }
                }
                else
                {
                    if (orignalBitmap.Width == SCREEN_TILE_WIDTH * 8 && orignalBitmap.Height == SCREEN_TILE_HEIGHT * 8)
                    {
                        //nop
                        bitmap = orignalBitmap;
                    }
                    else if (orignalBitmap.Width >= SCREEN_TILE_WIDTH * 8 && orignalBitmap.Height >= SCREEN_TILE_HEIGHT * 8)
                    {
                        bitmap = ImageUtil.Copy(orignalBitmap, 0, 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8);
                    }
                    else
                    {
                        this.ErrorMessage = R.Error("画像サイズが正しくありません。\r\n選択された画像のサイズ Width:{0} Height:{1}", orignalBitmap.Width, orignalBitmap.Height);
                        return null;
                    }
                }

                bool firsttry = (this.SeatBitmap == null);
                if (firsttry)
                {//最初なのでシートの初期化.
                    InitSeat(bitmap);
                }
                //パレットの確認.
                PaletteResult paletteResult = CheckAndConvertPalette(ref bitmap, firsttry);
                if (paletteResult == PaletteResult.Cancel)
                {//ユーザーキャンセル.
                    return null;
                }
                else if (paletteResult == PaletteResult.MagicChangePalette)
                {//魔法の場合、パレットが違った、場合、シートごと切り替えてしまえばよい
                    Debug.Assert(firsttry == false);

                    //満杯になったデータの保存 次のシートへ
                    NextSeat();

                    //もう一回トライ
                    return MakeBattleAnime(orignalBitmap, isMode2, oam);
                }

                //途中でシートが満杯でリトライになったときのためにバックアップを作成します.
                Bitmap seatBitmapBackup = ImageUtil.CloneBitmap(SeatBitmap);
                int oamBackupPostion = oam.Count;

                for (int oam_palette = 0; oam_palette < 4; oam_palette++)
                {
                    Bitmap bmp = ImageUtil.CopyByPalette(bitmap, oam_palette, 0 , 0 , bitmap.Width , bitmap.Height );
                    bool r = MakeBattleAnimeLow(bmp, animedata, oam_palette, oam);
                    if (!r)
                    {
                        //バックアップから復元します.
                        SeatBitmap = seatBitmapBackup;
                        oam.RemoveRange(oamBackupPostion, oam.Count - oamBackupPostion);

                        if (firsttry)
                        {//初回でシートが満杯になるはずがないだろう。いい加減にしろ.
                            this.ErrorMessage = R.Error("イメージが大きすぎます。\r\n初手で 256x64のシートが全部埋まりました。\r\n背景色の指定を間違えていませんか？\r\nまたは、キャラクターが256x64以上の巨大な画像です。\r\n");
                            return null;
                        }

                        //満杯になったデータの保存 次のシートへ
                        NextSeat();

                        //もう一回最初からトライ
                        return MakeBattleAnime(orignalBitmap, isMode2 , oam);
                    }

                    if (this.IsMultiPaletteOAM == false)
                    {//単一OAMならば、最初の16色だけを見ます.
                        break;
                    }
                }

                AppendTermOAM(oam);
                return animedata;
            }

            bool MakeBattleAnimeLow(Bitmap bitmap, animedata animedata, int oam_palette , List<byte> oam)
            {
                //ビットマップ画像で、使っていないところをマークする.
                bool[] useTileData = ImageUtil.MakeUseTileData(bitmap);

                int end = useTileData.Length;
                for (int i = 0; i < end; i++)
                {
                    if (useTileData[i])
                    {//すでに転送済み(または空白)だったら読み飛ばす.
                        continue;
                    }
                    Point vramxy = new Point();
                    if (IsMagicOAM)
                    {
                        vramxy.X = (i % SCREEN_TILE_WIDTH) * 8 / tile_scale - bitmap_spell_addx;
                    }
                    else
                    {
                        vramxy.X = (i % SCREEN_TILE_WIDTH) * 8 / tile_scale - bitmap_addx;
                    }
                    vramxy.Y = (i / SCREEN_TILE_WIDTH) * 8 / tile_scale - bitmap_addy;

                    Point seatxy;
                    if (!this.IsMagicOAM)
                    {//魔法は、シートが半分になるので、 8*8の転送は利用できません
                        //8x8
                        if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 8, 8, out seatxy))
                        {
                            AppendOAM(square, times8, oam, seatxy, vramxy, oam_palette);
                            continue;
                        }
                    }
                    //8x4
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 8, 4, out seatxy))
                    {
                        AppendOAM(horizontal, times8, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    if (!this.IsMagicOAM)
                    {//魔法は、シートが半分になるので、 8*8の転送は利用できません
                        //4x8
                        if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 4, 8, out seatxy))
                        {
                            AppendOAM(vertical, times8, oam, seatxy, vramxy, oam_palette);
                            continue;
                        }
                    }

                    //4x4
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 4, 4, out seatxy))
                    {
                        AppendOAM(square, times4, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //4x2
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 4, 2, out seatxy))
                    {
                        AppendOAM(horizontal, times4, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //2x4
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 2, 4, out seatxy))
                    {
                        AppendOAM(vertical, times4, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //2x2
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 2, 2, out seatxy))
                    {
                        AppendOAM(square, times2, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //4x1
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 4, 1, out seatxy))
                    {
                        AppendOAM(horizontal, times2, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //1x4
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 1, 4, out seatxy))
                    {
                        AppendOAM(vertical, times2, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //2x1
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 2, 1, out seatxy))
                    {
                        AppendOAM(horizontal, times1, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //1x2
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 1, 2, out seatxy))
                    {
                        AppendOAM(vertical, times1, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }
                    //1x1
                    if (tryCopy(bitmap, useTileData, SeatBitmap, SeatUsingFlags, i, 1, 1, out seatxy))
                    {
                        AppendOAM(square, times1, oam, seatxy, vramxy, oam_palette);
                        continue;
                    }

                    //シートが満杯らしい
                    return false;
                }

                return true;
            }

            static bool tryCopy(Bitmap bitmap, bool[] useTileData, Bitmap seatBitmap, bool[] seatUsingFlags, int bitmapxy, int w, int h, out Point out_seatxy)
            {
                int bitmapx = bitmapxy % SCREEN_TILE_WIDTH;
                int bitmapy = bitmapxy / SCREEN_TILE_WIDTH;
                if (bitmapx + w > SCREEN_TILE_WIDTH)
                {
                    out_seatxy = new Point();
                    return false;
                }
                if (bitmapy + h > SCREEN_TILE_HEIGHT)
                {
                    out_seatxy = new Point();
                    return false;
                }

                //画像 w h を満たせるか確認する.
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (useTileData[bitmapxy + x + (y * SCREEN_TILE_WIDTH)])
                        {//すでに使っている
                            out_seatxy = new Point();
                            return false;
                        }
                    }
                }

                //すでにこのブロックを保持しているなら使いまわす
                Bitmap blockbitmap = ImageUtil.Copy(bitmap,bitmapx * 8, bitmapy * 8,w * 8, h * 8);
                if (ImageUtil.GrepTileBitmap(seatBitmap, blockbitmap, out out_seatxy))
                {
                    out_seatxy.X /= 8;
                    out_seatxy.Y /= 8;

                    //画像に、利用マークを立てる
                    setMarkInSeat(useTileData, bitmapx, bitmapy, w, h, SCREEN_TILE_WIDTH);
                    return true;
                }

                //コピーするシートで w h の範囲が空いているか確認します.


                int seat_tile_height = seatBitmap.Height / 8;
                for (int y = 0; y <= seat_tile_height - h; y++)
                {
                    for (int x = 0; x <= SEAT_TILE_WIDTH - w; x++)
                    {
                        if (checkSeatEmpty(seatUsingFlags, x, y, w, h))
                        {//シートに空きがある
                            out_seatxy.X = x;
                            out_seatxy.Y = y;

                            //データのコピー
                            ImageUtil.BitBlt(seatBitmap, x * 8, y * 8, w * 8, h * 8, bitmap, bitmapx * 8, bitmapy * 8);

                            //画像とシートに、利用マークを立てる
                            setMarkInSeat(useTileData, bitmapx, bitmapy, w, h, SCREEN_TILE_WIDTH);
                            setMarkInSeat(seatUsingFlags, x, y, w, h, SEAT_TILE_WIDTH);
                            return true;
                        }
                    }
                }
                return false;
            }


            static void setMarkInSeat(bool[] seat, int seatx, int seaty, int w, int h, int seatwidth)
            {
                //画像 w h を満たせるか確認する.
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        seat[seatx + x + ((seaty + y) * seatwidth)] = true;
                    }
                }
            }

            static bool checkSeatEmpty(bool[] seatUsingFlags, int seatx, int seaty, int w, int h)
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (seatUsingFlags[seatx + x + ((seaty + y) * SEAT_TILE_WIDTH)])
                        {//すでに使っている
                            return false;
                        }
                    }
                }
                return true;
            }

            static void AppendOAM(byte align
                , byte area
                , List<byte> oam
                , Point seatxy
                , Point vramxy
                , int oam_palette
                )
            {
                oam.Add(0);
                oam.Add(align);
                oam.Add(0);
                oam.Add(area);

                oam.Add((byte)((seatxy.X & 0x1F) | ((seatxy.Y << 5) & 0xE0) ));
                oam.Add((byte)((oam_palette & 0xF) << 4));

                U.append_u16(oam,(uint)((short)vramxy.X));

                U.append_u16(oam, (uint)((short)vramxy.Y));

                oam.Add(0);
                oam.Add(0);
            }

            //終端コード.
            static void AppendTermOAM(
              List<byte> oam
            )
            {
                oam.Add(1);
                oam.Add(0);
                oam.Add(0);
                oam.Add(0);

                oam.Add(0);
                oam.Add(0);

                oam.Add(0);
                oam.Add(0);

                oam.Add(0);
                oam.Add(0);

                oam.Add(0);
                oam.Add(0);
            }


            //mode2 mode4 でデータがない場合ダミーフレームを入れる.
            static void AppendMode2DummyOAM(
              List<byte> oam
            )
            {
                oam.Add(0);
                oam.Add(128);
                oam.Add(0);
                oam.Add(128);

                oam.Add(0);
                oam.Add(0);

                oam.Add(248);
                oam.Add(255);

                oam.Add(232);
                oam.Add(255);

                oam.Add(0);
                oam.Add(0);
            }



            //登録終了
            public void Term()
            {
                if (SeatBitmap == null)
                {
                    return;
                }

                NextSeat();
            }

            public uint AppendTermOAM()
            {
                int pos = RightToLeftOAM.Count;
                AppendTermOAM(RightToLeftOAM);
                return (uint)pos;
            }

            //次のシートへ
            public void NextSeat()
            {
                byte[] seatimage = ImageUtil.ImageToByte16Tile(SeatBitmap, SeatBitmap.Width, SeatBitmap.Height);
                image_data image_data = new image_data();
                image_data.data = LZ77.compress(seatimage);
                Images.Add(image_data);

                if (this.IsMagicOAM)
                {//魔法の場合、パレットがシートごとに切り替えられるため パレットもデータとして保存する.
                    image_data palette_data = new image_data();
                    palette_data.data = (byte[])this.Palette.Clone();
                    Images.Add(palette_data);
                }

                //新規に作らせるために null値にする
                SeatBitmap = null;
                SeatUsingFlags = null;
            }

            public byte[] GetRightToLeftOAM_Z()
            {
                Debug.Assert(this.IsMagicOAM == false);
                return LZ77.compress(GetRightToLeftOAM());
            }
            public byte[] GetLeftToRightOAM_Z()
            {
                Debug.Assert(this.IsMagicOAM == false);
                return LZ77.compress(GetLeftToRightOAM());
            }
            public byte[] GetRightToLeftOAM()
            {
                return this.RightToLeftOAM.ToArray();
            }
            public byte[] GetLeftToRightOAM()
            {
                return GetLeftToRightOAM(this.RightToLeftOAM);
            }
            public byte[] GetRightToLeftOAMBG()
            {
                Debug.Assert(this.IsMagicOAM == true);
                return this.RightToLeftOAMBG.ToArray();
            }
            public byte[] GetLeftToRightOAMBG()
            {
                Debug.Assert(this.IsMagicOAM == true);
                return GetLeftToRightOAM(this.RightToLeftOAMBG);
            }

            public uint GetOAMByteCount()
            {
                return (uint)this.RightToLeftOAM.Count;
            }

            
            byte[] GetLeftToRightOAM(List<byte> oam)
            {
                byte[] leftToRight = oam.ToArray();
                for (int i = 0; i < leftToRight.Length; i += 12)
                {
                    if (leftToRight[i] == 1)
                    {//end code
                        continue;
                    }
                    if (leftToRight[i + 2] == 0xff && leftToRight[i + 3] == 0xff)
                    {//回転とかの別ルーチン
                        continue;
                    }
                    uint align = (uint)(leftToRight[i + 1]);
                    uint area = (uint)(leftToRight[i + 3]);

                    //幅高さの取得
                    int width, height;
                    DrawOAM.convertAlignAreaToWH(align, area, out width, out height);

                    //反転フラグ
                    leftToRight[i + 3] = (byte)(leftToRight[i + 3] | 0x10);

                    //描画位置調整
                    int vram_x = (short)U.u16(leftToRight, (uint)(i + 6));
                    vram_x = -(width * 8) - vram_x;

                    U.write_u16(leftToRight, (uint)(i + 6), (uint)vram_x);
                }
                return leftToRight;
            }
            public int GetOAMSize()
            {
                return this.RightToLeftOAM.Count;
            }
            public byte[] GetPalette()
            {
                return this.Palette;
            }
            public List<image_data> GetImages()
            {
               return this.Images;
            }
            public string GetBaseDir()
            {
                return this.BaseDir;
            }
        }

        //ユーザーがパレットを指定しているか？
        public static byte[] FindUserPaletteOrder(string first_imagename,string type)
        {
            if (first_imagename == "")
            {
                return null;
            }
            string ext = Path.GetExtension(first_imagename);
            string name = Path.GetFileNameWithoutExtension(first_imagename);
            string dir = Path.GetDirectoryName(first_imagename);

            string search_filename = Path.Combine(dir, name + type + ext);
            if (!File.Exists(search_filename))
            {
                return null;
            }
            Bitmap bitmap = ImageUtil.OpenBitmap(search_filename);
            if (bitmap == null)
            {
                return null;
            }
            byte[] imagepalette = ImageUtil.ImageToPalette(bitmap, 1);
            return imagepalette;
        }

        //敵、NPC(友軍)、グレーのパレットを作成します.
        public static byte[] MakeBattle4Palette_Z(string first_imagename,byte[] basePalette, Dictionary<uint, ReColorMap> reColorMap)
        {
            if (basePalette.Length >= 16 * 4)
            {//既に色がそろっている場合.
                return LZ77.compress(basePalette);
            }

            //バトルアニメのパレットは4つ用意しないとダメ
            byte[] fourPalette = new byte[16 * 2 * 4];

            //色をユーザが指定することができるらしい.
            byte[] orderPaletteEnemy = FindUserPaletteOrder(first_imagename,"enemy");
            byte[] orderPaletteNPC = FindUserPaletteOrder(first_imagename, "ally");
            byte[] orderPaletteGray = FindUserPaletteOrder(first_imagename, "playerfour");

            //既存の色割り当てを模倣して、敵とかを配色をする
            for(uint i = 0 ; i < 16 ; i++)
            {
                uint player = U.u16(basePalette,i*2);
                uint enemy,npc,gray;
                
                if (reColorMap.ContainsKey(player) )
                {
                    ReColorMap m = reColorMap[player];
                    enemy = m.Enemy;
                    npc = m.NPC;
                    gray = m.Gray;
                }
                else
                {
                    enemy = player;
                    npc = player;
                    gray = player;
                }

                if (orderPaletteEnemy != null)
                {
                    enemy = U.u16(orderPaletteEnemy, (i * 2));
                }
                if (orderPaletteNPC != null)
                {
                    npc = U.u16(orderPaletteNPC, (i * 2));
                }
                if (orderPaletteGray != null)
                {
                    gray = U.u16(orderPaletteGray, (i * 2));
                }

                U.write_u16(fourPalette,(i * 2) + (0),player);
                U.write_u16(fourPalette,(i * 2) + (2 * 16 * 1),enemy);
                U.write_u16(fourPalette,(i * 2) + (2 * 16 * 2),npc);
                U.write_u16(fourPalette,(i * 2) + (2 * 16 * 3),gray);
            }

            return LZ77.compress(fourPalette);
        }        
        
        //画像の書き込みアドレスが決定したら、画像ポインタをかかないといけないFrameDataを更新します。
        static string updateFrameDataAddress(byte[] frameData, List<image_data> images)
        {
            uint length = (uint)frameData.Length;
            for (uint n = 0; n < length; )
            {
                if (frameData[n + 3] != 0x86) //0x86 コマンド以外は skip
                {
                    n += 4;
                    continue;
                }

                int a = (int)U.u32(frameData, n + 4);
                if (a < 0 || a >= images.Count)
                {
                    return R._("out of range a:{0} images.Count:{1} index:{2} dump:{3}", a, images.Count, n + 4,U.DumpByte(frameData));
                }

                U.write_p32(frameData, n + 4, images[a].write_addr);
                n += 4 + 4 + 4;
            }
            return "";
        }


        // p- filname のパース
        public static string parsePFilename(string line)
        {
            string imagefilename;
            imagefilename = U.skip(line,"p- ");
            if (imagefilename.Length <= 0)
            {
                imagefilename = U.skip(line,"p-\t");
                if (imagefilename.Length <= 0)
                {
                    //p- がない形式?
                    int i = 0;
                    for (; i < line.Length; i++)
                    {
                        if (line[i] == ' ' || line[i] == '\t')
                        {
                            break;
                        }
                    }
                    if (i >= line.Length)
                    {//存在しない
                        return "";
                    }
                    imagefilename = line.Substring(i);
                }
            }

            imagefilename = imagefilename.Trim();
            if (imagefilename.Length <= 0)
            {
                return "";
            }

            int imagefilenameend = imagefilename.IndexOf(".png",StringComparison.OrdinalIgnoreCase);
            if(imagefilenameend > 0)
            {
                imagefilename = imagefilename.Substring(0,imagefilenameend + 4);
                return imagefilename;
            }
            imagefilenameend = imagefilename.IndexOf(".bmp",StringComparison.OrdinalIgnoreCase);
            if(imagefilenameend > 0)
            {
                imagefilename = imagefilename.Substring(0,imagefilenameend + 4);
                return imagefilename;
            }

            //不明 とりあえず適当に返す.
            return imagefilename;
        }

        static Bitmap DrawFrameImageWide(uint frame
            , uint frameDataStartMode2, uint frameDataEndMode2
            , uint frameDataStart, uint frameDataEnd
            , byte[] frameData_UZ, byte[] palettes_UZ, byte[] rightToLeftOAM_UZ)
        {
            Debug.Assert(frame >= frameDataStart && frame < frameDataEnd);

            Bitmap bitmap = DrawFrameImage(frame, frameData_UZ, palettes_UZ, rightToLeftOAM_UZ);
            if (frameDataStartMode2 == 0 || frameDataEndMode2 == 0)
            {//普通に描画するだけ
                return bitmap;
            }
            //Mode1はMode2 と、 Mode3はMode4 共に別処理
            Debug.Assert(frame >= frameDataStart);

            uint frame2 = frameDataStartMode2 + (frame - frameDataStart);
            if (frame2 >= frameDataEndMode2)
            {//普通に描画するだけ
                return bitmap;
            }

            Bitmap bitmap2 = DrawFrameImage(frame2, frameData_UZ, palettes_UZ, rightToLeftOAM_UZ);
            if (ImageUtil.IsBlankBitmap(bitmap2))
            {//mode2 or mode4 で描画するものがない場合は通常描画
                return bitmap;
            }

            Bitmap widebitmap = ImageUtil.Blank(SCREEN_TILE_APPENDMODE2_WIDE * 8, SCREEN_TILE_HEIGHT * 8, bitmap);
            ImageUtil.BitBlt(widebitmap, 0, 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, bitmap, 0,0 );
            ImageUtil.BitBlt(widebitmap, (SCREEN_TILE_WIDTH-1) * 8, 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, bitmap2, 0 , 0 , 0, 0x00);
//            ImageUtil.BitBlt(widebitmap, (SCREEN_TILE_WIDTH - 2) * 8, 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, bitmap2, 0, 0, 0, 0x00); //間違い

            return widebitmap;
        }

        static uint getSectionData(uint sectionDataIndex, uint sectionData_offset, uint frameData_length)
        {
            if (sectionDataIndex >= 0xC)
            {
                return frameData_length;
            }

            uint read_offset = (sectionDataIndex * 4) + sectionData_offset;
            if (!U.isSafetyOffset(read_offset))
            {
                return frameData_length;
            }

            uint frameDataEnd = Program.ROM.u32(read_offset);
            if (frameDataEnd > frameData_length)
            {
                return frameData_length;
            }
            return frameDataEnd;
        }

        static void getSectionDataStartEnd(uint sectionDataIndex, uint sectionData_offset, uint frameData_length
            , out uint out_startFrameAddr, out uint out_endFrameAddr)
        {
            uint start = getSectionData(sectionDataIndex,sectionData_offset,frameData_length);
            uint end = getSectionData(sectionDataIndex + 1, sectionData_offset, frameData_length);

            if (end == 0)
            {//終了地点が0なので補正する
                end = frameData_length;
            }
            if (start == 0)
            {//開始地点が0らしい。補正する
                end = getSectionData(1, sectionData_offset, frameData_length);
            }

            Debug.Assert(start <= end);
            out_startFrameAddr = start;
            out_endFrameAddr = end;
        }

        //後ろから数えて framesec秒前にループシンボルを埋め込む
        static void appendLoopStartSymbol(uint framesec, List<string> lines, string loopline)
        {
            uint totalframe = 0;
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                string a = lines[i];
                if (a.Length <= 1)
                {//? ゴミデータ??
                    continue;
                }
                if ( a[0] == '~' )
                {//モードが終わってしまった... バグっている.
                    return ;
                }
                if (!U.isnum(a[0]))
                {//数字でなければ別命令なので無視.
                    continue;
                }
                //1 p- filename みたいなデータ
                totalframe += 3;

                if (totalframe >= framesec)
                {//ループマークを差し込む
                    lines.Insert(i, loopline);
                    return;
                }
            }
        }
        public static void ExportBattleAnime(string filehint //もっと詳しい情報 01 ロード 槍 みたいなの
            , bool enableComment //コメントを出すかどうか
            , string filename    //書き込むファイル名
            , uint sectionData
            , uint frameData
            , uint rightToLeftOAM
            , uint palettes
            , int palette_count)
        {
            try
            {
                ExportBattleAnimeLow(filehint //もっと詳しい情報 01 ロード 槍 みたいなの
                    , enableComment //コメントを出すかどうか
                    , filename    //書き込むファイル名
                    , sectionData
                    , frameData
                    , rightToLeftOAM
                    , palettes
                    , palette_count
                    );
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                R.ShowStopError(R.ExceptionToString(e));
            }
        }

        //圧縮されていないフレームの長さを求める
        //通常フレームは圧縮されているが、昔のツールでは圧縮されていないフレームがあった。
        static uint CalcUnCompressFrameLength(uint frameData_offset)
        {
            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frameData_offset + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);
            byte[] frameData = Program.ROM.Data;
            uint i;
            uint frameCount = 0;
            for (i = frameData_offset; i < limitter; i += 4)
            {
                if (frameData[i + 3] == 0x80)
                {//終端データ
                    frameCount++;
                    i += 4;
                    if (frameCount > 0xC)
                    {//アニメはフレーム0xCまである
                        break;
                    }
                    continue;
                }
                else if (frameData[i + 3] != 0x86)
                {
                    if (frameData[i + 3] == 0x85)
                    {
                        continue;
                    }
                    //不明な命令.
                    continue;
                }
                i += 8;
            }
            return i - frameData_offset;
        }

        static byte[] UnCompressFrame(uint frameAddr)
        {
            //FEの戦闘アニメフレームはlz77で圧縮されています。
            //しかし、昔のツールでは、フレームの圧縮ができなかったため、無圧縮フレームというデータ構造があるらしい
            if (FETextEncode.IsUnHuffmanPatchPointer(frameAddr))
            {
                frameAddr = FETextEncode.ConvertUnHuffmanPatchToPointer(frameAddr);
                frameAddr = U.toOffset(frameAddr);
                uint length = CalcUnCompressFrameLength(frameAddr);
                return Program.ROM.getBinaryData(frameAddr, length);
            }
            else
            {
                frameAddr = U.toOffset(frameAddr);
                return LZ77.decompress(Program.ROM.Data, frameAddr);
            }
        }

        static void ExportBattleAnimeLow(string filehint //もっと詳しい情報 01 ロード 槍 みたいなの
            , bool enableComment //コメントを出すかどうか
            , string filename    //書き込むファイル名
            , uint sectionData
            , uint frameData
            , uint rightToLeftOAM
            , uint palettes
            , int palette_count)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            uint sectionData_offset = U.toOffset(sectionData); //int[0xC]個 ヘッダの区切りバイト 絶対値

            //解凍する.
            //固定長のsectionData以外はLZ77で圧縮されている.
            byte[] frameData_UZ = UnCompressFrame(frameData); //画像をどう切り出すかを提起したデータ
            byte[] rightToLeftOAM_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(rightToLeftOAM)); //OAM
            byte[] palettes_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(palettes)); //Palette

            //読みやすいようにコメントを入れます.
            Dictionary<uint, string> Comment_85command_Dic = U.LoadDicResource(U.ConfigDataFilename("battleanime_85command_"));
            Dictionary<uint, string> Comment_Mode_Dic = U.LoadDicResource(U.ConfigDataFilename("battleanime_mode_"));

            //武器をもっていないモーションかどうか
            //武器をもっていないモーションなので、0x01と0x03のような貫通している武器モーションは存在しない.
            bool isNoWeaponAnimation = IsNoWeaponAnimation(sectionData_offset, frameData_UZ);


            List<String> lines = new List<String>();
            string line;
            if (enableComment)
            {
                lines.Add("#######################################################");
                lines.Add("#" + filehint);
                lines.Add("#");
                lines.Add("#" + R._("FEditorAdvにインポートする時には各行の#以降を削除してください。"));
                lines.Add("#######################################################");
            }
            //同じアニメを何度も出力しないように記録する.
            List<uint> animeHash = new List<uint>();

            for (uint sectionDataIndex = 0; sectionDataIndex < 0xC; sectionDataIndex++)
            {
                //Mode 2 と Mode 4 は特別処理が別途するのでここにはこない.
                Debug.Assert(sectionDataIndex != 2 - 1 && sectionDataIndex != 4 - 1);

                //sectionDataデータは、 int sectionData[0x0C] として、
                //frameData_UZへの参照位置を返します.
                uint frameDataStart, frameDataEnd;
                getSectionDataStartEnd(sectionDataIndex, sectionData_offset, (uint)frameData_UZ.Length
                    , out frameDataStart, out frameDataEnd);

                //Mode1 と Mode3 のときは、 特別処理が必要.
                //Mode1 は Mode2 と、
                //Mode3 は Mode4 は、伴って処理しないといけない.
                uint frameDataStartMode2 = 0;
                uint frameDataEndMode2 = 0;

                if (IsSeet01or03(sectionDataIndex) && isNoWeaponAnimation == false)
                {
                    getSectionDataStartEnd(sectionDataIndex + 1, sectionData_offset, (uint)frameData_UZ.Length
                        , out frameDataStartMode2, out frameDataEndMode2);
                }

                line = "/// - Mode " + (sectionDataIndex + 1);
                if (enableComment)
                {
                    line += "               #" + U.at(Comment_Mode_Dic, sectionDataIndex + 1);
                }
                lines.Add(line);

                //frameDataは4バイトずつのデータからなり、
                //?? ?? ?? 0x86 のあとには、4バイトの画像へのポインタと、 4バイトの 変換する OAMへの絶対位置を返します.
                uint frameI = 0;
                for (uint n = frameDataStart; n < frameDataEnd; n += 4)
                {
                    if (frameData_UZ[n + 3] == 0x85) //0x85 コマンド
                    {
                        if (frameData_UZ[n] == 0x48)
                        {//音楽再生
                            uint musicid = U.u16(frameData_UZ, n + 1);
                            line = "S" + U.ToHexString(musicid);
                            if (enableComment)
                            {
                                line += "                               #Sound " + musicid + " " + SongTableForm.GetSongName(musicid);
                            }
                            lines.Add(line);
                        }
                        else if (frameData_UZ[n] == 0x01)
                        {//01ハックしてループ命令がくることがある.
                            uint framesec = U.u16(frameData_UZ, n + 1);
                            if (framesec > 0)
                            {
                                line = "L";
                                if (enableComment)
                                {
                                    line += "                                 #" + "LOOPSTART {";
                                }
                                appendLoopStartSymbol(framesec, lines, line);

                                line = "C01";
                                if (enableComment)
                                {
                                    line += "                               #" + "LOOPEND }  " + U.at(Comment_85command_Dic, frameData_UZ[1]);
                                }
                            }
                            else
                            {
                                line = "C01";
                                if (enableComment)
                                {
                                    line += "                               #" + U.at(Comment_85command_Dic, frameData_UZ[1]);
                                }
                            }
                            lines.Add(line);
                        }
                        else
                        {//それ以外の 0x85命令
                            uint id = U.u24(frameData_UZ, n);
                            line = "C" + U.ToHexString(id);
                            if (enableComment)
                            {
                                line += "                               #" + U.at(Comment_85command_Dic, frameData_UZ[n]);
                            }
                            lines.Add(line);
                        }
                        continue;
                    }
                    else if (frameData_UZ[n + 3] == 0x80)
                    {//終端命令 ~が終端を兼ねているので、特に何も表示しない.
                        continue;
                    }
                    else if (frameData_UZ[n + 3] != 0x86)
                    {//不明な命令なので読み飛ばす
                        continue;
                    }

                    //0x86 画像 pointer
                    uint wait = U.u16(frameData_UZ,n);
                    //ポインタ<<8 + OAM座標としてuniqなIDを作ります.
                    uint imageHash = (U.p32(frameData_UZ, n + 4)<<8) + U.u32(frameData_UZ, n + 8);

                    string framefilename = "";
                    int seatnumber = animeHash.IndexOf(imageHash);
                    if (seatnumber < 0)
                    {//まだ出力していない画像なので作成します.
                        seatnumber = animeHash.Count;
                        animeHash.Add(imageHash);

                        Bitmap bitmap = DrawFrameImageWide
                                (n
                                ,frameDataStartMode2, frameDataEndMode2
                                ,frameDataStart, frameDataEnd
                                ,frameData_UZ, palettes_UZ, rightToLeftOAM_UZ);

                        ImageUtil.AppendPaletteMark(bitmap, palette_count);
                        ImageUtil.BlackOutUnnecessaryColors(bitmap, palette_count);

                        framefilename = basename + seatnumber.ToString("000") + ".png";
                        U.BitmapSave(bitmap, Path.Combine(basedir, framefilename));
                        bitmap.Dispose();
                    }
                    else
                    {//すでに出力しているのでカットする.
                        framefilename = basename + seatnumber.ToString("000") + ".png";
                    }

                    line = wait.ToString() + " p- " + framefilename;
                    lines.Add(line);


                    frameI++;
                    n += 8;
                }

                line = "~~~";
                lines.Add(line);

                //mode2 と mode4は別処理しているので飛ばす.
                if (IsSeet01or03(sectionDataIndex))
                {
                    sectionDataIndex++;
                }
            }

            line = "/// - End of animation";
            lines.Add(line);

            //まとめて書き込み
            File.WriteAllLines(filename, lines);
            return ;
        }

        //最大パレット数を求める
        public static int CalcMaxPaletteCount(
              uint sectionData
            , uint frameData
            , uint rightToLeftOAM
            , uint palettes
            )
        {
            uint sectionData_offset = U.toOffset(sectionData); //int[0xC]個 ヘッダの区切りバイト 絶対値

            //解凍する.
            //固定長のsectionData以外はLZ77で圧縮されている.
            byte[] frameData_UZ = UnCompressFrame(frameData); //画像をどう切り出すかを提起したデータ
            byte[] rightToLeftOAM_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(rightToLeftOAM)); //OAM
            byte[] palettes_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(palettes)); //Palette

            //武器をもっていないモーションかどうか
            //武器をもっていないモーションなので、0x01と0x03のような貫通している武器モーションは存在しない.
            bool isNoWeaponAnimation = IsNoWeaponAnimation(sectionData_offset, frameData_UZ);

            //同じアニメを何度も出力しないように記録する.
            Dictionary<uint, Bitmap> animeHash = new Dictionary<uint, Bitmap>();
            int maxPaletteCount = 1;

            for (uint sectionDataIndex = 0; sectionDataIndex < 0xC; sectionDataIndex++)
            {
                //Mode 2 と Mode 4 は特別処理が別途するのでここにはこない.
                Debug.Assert(sectionDataIndex != 2 - 1 && sectionDataIndex != 4 - 1);

                //sectionDataデータは、 int sectionData[0x0C] として、
                //frameData_UZへの参照位置を返します.
                uint frameDataStart, frameDataEnd;
                getSectionDataStartEnd(sectionDataIndex, sectionData_offset, (uint)frameData_UZ.Length
                    , out frameDataStart, out frameDataEnd);

                //Mode1 と Mode3 のときは、 特別処理が必要.
                //Mode1 は Mode2 と、
                //Mode3 は Mode4 は、伴って処理しないといけない.
                uint frameDataStartMode2 = 0;
                uint frameDataEndMode2 = 0;
                if (IsSeet01or03(sectionDataIndex) && isNoWeaponAnimation == false)
                {
                    getSectionDataStartEnd(sectionDataIndex + 1, sectionData_offset, (uint)frameData_UZ.Length
                        , out frameDataStartMode2, out frameDataEndMode2);
                }

                //frameDataは4バイトずつのデータからなり、
                //?? ?? ?? 0x86 のあとには、4バイトの画像へのポインタと、 4バイトの 変換する OAMへの絶対位置を返します.
                uint frameI = 0;
                for (uint n = frameDataStart; n < frameDataEnd; n += 4)
                {
                    if (frameData_UZ[n + 3] == 0x85) //0x85 コマンド
                    {
                        continue;
                    }
                    else if (frameData_UZ[n + 3] == 0x80)
                    {//終端命令 ~が終端を兼ねているので、特に何も表示しない.
                        continue;
                    }
                    else if (frameData_UZ[n + 3] != 0x86)
                    {//不明な命令なので読み飛ばす
                        continue;
                    }

                    //0x86 画像 pointer
                    uint wait = U.u16(frameData_UZ, n);
                    //ポインタ<<8 + OAM座標としてuniqなIDを作ります.
                    uint imageHash = (U.p32(frameData_UZ, n + 4) << 8) + U.u32(frameData_UZ, n + 8);

                    Bitmap bitmap;
                    if (!animeHash.ContainsKey(imageHash))
                    {//まだ出力していない画像なので作成します.
                        bitmap = DrawFrameImageWide
                                (n
                                , frameDataStartMode2, frameDataEndMode2
                                , frameDataStart, frameDataEnd
                                , frameData_UZ, palettes_UZ, rightToLeftOAM_UZ);
                        if (bitmap.Width > SEAT_TILE_WIDTH * 8)
                        {//敵にめり込んだ部分がワイド出力されているので、畳み込む.
                            Bitmap temp = ImageUtil.Copy(bitmap, 0, 0, SEAT_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8);
                            ImageUtil.BitBlt(temp, 0, 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, bitmap, (SCREEN_TILE_WIDTH - 1) * 8, 0, 0, 0x00);

                            bitmap = temp;
                        }
                        //パレットをクリップ
                        int c = ImageUtil.GetPalette16Count(bitmap);
                        maxPaletteCount = Math.Max(maxPaletteCount, c);

                        animeHash.Add(imageHash, bitmap);
                    }
                    else
                    {//すでに出力しているのでカットする.
                    }

                    frameI++;
                    n += 8;
                }

                //mode2 と mode4は別処理しているので飛ばす.
                if (IsSeet01or03(sectionDataIndex))
                {
                    sectionDataIndex++;
                }
            }

            if (maxPaletteCount <= 1)
            {
                return 1;
            }
            else
            {
                return 4;
            }
        }

        static bool IsSeet01or03(uint sectionDataIndex)
        {
            return (sectionDataIndex == 1 - 1 || sectionDataIndex == 3 - 1);
        }

        //単位はフレーム秒
        public static readonly uint[] HIT_HOLD_TIMES = { 15, 0, 15, 0, 25, 25, 25, 25, 0, 0, 0, 8 }; //const
        public const uint THROW_AXE_HOLD_TIME = 24;
        public const uint FIRST_FRAME_HOLD_TIME = 30;

        //アニメーションGIFで出力する.
        public static void ExportBattleAnimeGIF(
              string filename    //書き込むファイル名
            , uint sectionDataIndex
            , uint sectionData
            , uint frameData
            , uint rightToLeftOAM
            , uint palettes
            , int palette_count
            )
        {
            try
            {
                ExportBattleAnimeGIFLow(
                          filename    //書き込むファイル名
                        , sectionDataIndex
                        , sectionData
                        , frameData
                        , rightToLeftOAM
                        , palettes
                        , palette_count
                    );
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                R.ShowStopError(R.ExceptionToString(e));
            }
        }

        static void ExportBattleAnimeGIFLow(
              string filename    //書き込むファイル名
            , uint sectionDataIndex
            , uint sectionData
            , uint frameData
            , uint rightToLeftOAM
            , uint palettes
            , int palette_count
            )
        {
            uint sectionData_offset = U.toOffset(sectionData); //int[0xC]個 ヘッダの区切りバイト 絶対値

            //解凍する.
            //固定長のsectionData以外はLZ77で圧縮されている.
            byte[] frameData_UZ = UnCompressFrame(frameData); //画像をどう切り出すかを提起したデータ
            byte[] rightToLeftOAM_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(rightToLeftOAM)); //OAM
            byte[] palettes_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(palettes)); //Palette

            //武器をもっていないモーションかどうか
            //武器をもっていないモーションなので、0x01と0x03のような貫通している武器モーションは存在しない.
            bool isNoWeaponAnimation = IsNoWeaponAnimation(sectionData_offset, frameData_UZ);

            //同じアニメを何度も出力しないように記録する.
            Dictionary<uint, Bitmap> animeHash = new Dictionary<uint, Bitmap>();
            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();

            for (sectionDataIndex = 0; sectionDataIndex < 0xC; sectionDataIndex++)
            {
                //Mode 2 と Mode 4 は特別処理が別途するのでここにはこない.
                Debug.Assert(sectionDataIndex != 2 - 1 && sectionDataIndex != 4 - 1);

                //sectionDataデータは、 int sectionData[0x0C] として、
                //frameData_UZへの参照位置を返します.
                uint frameDataStart, frameDataEnd;
                getSectionDataStartEnd(sectionDataIndex, sectionData_offset, (uint)frameData_UZ.Length
                    , out frameDataStart, out frameDataEnd);

                //Mode1 と Mode3 のときは、 特別処理が必要.
                //Mode1 は Mode2 と、
                //Mode3 は Mode4 は、伴って処理しないといけない.
                uint frameDataStartMode2 = 0;
                uint frameDataEndMode2 = 0;
                if (IsSeet01or03(sectionDataIndex) && isNoWeaponAnimation == false)
                {
                    getSectionDataStartEnd(sectionDataIndex + 1, sectionData_offset, (uint)frameData_UZ.Length
                        , out frameDataStartMode2, out frameDataEndMode2);
                }

                //攻撃モーションなどでウェイトを入れる FEditor Adv Gif Dumperより
                //85Commandによる遅延. ダメージエフェクトとか手斧なげたときとかの遅延
                uint hitPauseTime = 0;

                //frameDataは4バイトずつのデータからなり、
                //?? ?? ?? 0x86 のあとには、4バイトの画像へのポインタと、 4バイトの 変換する OAMへの絶対位置を返します.
                uint frameI = 0;
                for (uint n = frameDataStart; n < frameDataEnd; n += 4)
                {
                    if (frameData_UZ.Length < n + 4)
                    {//データが途中で終わっている変なデータ
                        Debug.Assert(false);
                        break;
                    }

                    if (frameData_UZ[n + 3] == 0x85) //0x85 コマンド
                    {
                        if (frameData_UZ[n] == 0x48)
                        {//音楽再生
                        }
                        else if (frameData_UZ[n] == 0x01)
                        {//01ハックしてループ命令がくることがある.
                            hitPauseTime = Math.Max(hitPauseTime, HIT_HOLD_TIMES[sectionDataIndex]);
                        }
                        else if (frameData_UZ[n] == 0x13)
                        {//13 手斧を投げたときだと思われる.
                            hitPauseTime = Math.Max(hitPauseTime, THROW_AXE_HOLD_TIME);
                        }
                        else
                        {//それ以外の 0x85命令
                        }
                        continue;
                    }
                    else if (frameData_UZ[n + 3] == 0x80)
                    {//終端命令 ~が終端を兼ねているので、特に何も表示しない.
                        continue;
                    }
                    else if (frameData_UZ[n + 3] != 0x86)
                    {//不明な命令なので読み飛ばす
                        continue;
                    }

                    //0x86 画像 pointer
                    uint wait = U.u16(frameData_UZ, n);
                    //ポインタ<<8 + OAM座標としてuniqなIDを作ります.
                    uint imageHash = (U.p32(frameData_UZ, n + 4) << 8) + U.u32(frameData_UZ, n + 8);

                    Bitmap bitmap;
                    if (!animeHash.ContainsKey(imageHash))
                    {//まだ出力していない画像なので作成します.
                        bitmap = DrawFrameImageWide
                                (n
                                , frameDataStartMode2, frameDataEndMode2
                                , frameDataStart, frameDataEnd
                                , frameData_UZ, palettes_UZ, rightToLeftOAM_UZ);
                        if (bitmap.Width > SEAT_TILE_WIDTH * 8)
                        {//敵にめり込んだ部分がワイド出力されているので、畳み込む.
                            Bitmap temp = ImageUtil.Copy(bitmap, 0, 0, SEAT_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8);
                            ImageUtil.BitBlt(temp, 0, 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, bitmap, (SCREEN_TILE_WIDTH - 1) * 8, 0, 0, 0x00);

                            bitmap = temp;
                        }
                        //パレットをクリップ
                        bitmap = ImageUtil.Copy(bitmap, 0, 0, SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8);

                        ImageUtil.AppendPaletteMark(bitmap, palette_count);
                        ImageUtil.BlackOutUnnecessaryColors(bitmap, palette_count);

                        animeHash.Add(imageHash, bitmap);
                    }
                    else
                    {//すでに出力しているのでカットする.
                        bitmap = animeHash[imageHash];
                    }
                    
                    if (hitPauseTime > 0 && bitmaps.Count >= 1)
                    {//前回のコマンドの後に遅延命令が書いてあれば、前回の画像をもう一度遅延させて表示する.
                        bitmaps[bitmaps.Count - 1].Wait += hitPauseTime;
                        hitPauseTime = 0;
                    }
                    else if (bitmaps.Count == 0)
                    {//最初の画像は、30フレームだけ遅延させる
                        wait += FIRST_FRAME_HOLD_TIME;
                    }
                    bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, wait));

                    frameI++;
                    n += 8;
                }

                //mode2 と mode4は別処理しているので飛ばす.
                if (IsSeet01or03(sectionDataIndex))
                {
                    sectionDataIndex++;
                }
            }

            //アニメgif生成
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
            return;
        }


        //ループを作れるかどうか
        //条件 ループ挿入地点に C05 | C04 があること
        static bool isMakeingLoop(string[] lines,int now)
        {
            int innerImageCount = 0;

            for (int i = now - 1; i >= 0; i-- )
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipCommentWithCharpAndAtmark(line);
                if (line == "")
                {
                    continue;
                }
                if (line[0] == '~')
                {//モード終端記号
                    return false;
                }
                if (line[0] == 'S')
                {//音楽
                    return false;
                }

                if (line[0] == 'C')
                {//85 command
                    uint command = U.atoi(line.Substring(1));
                    if (command == 0x05 || command == 0x04)
                    {
                        return true;
                    }
                    return false;
                }
                if (line[0] >= '0' && line[0] <= '9')
                {//画像がはさまってもいいらしい?
                    innerImageCount++;
                    //2つあってもいいらしい.
//                    if (innerImageCount >= 2) 
//                    {//流石に2つ以上はダメじゃね?
//                        return false;
//                    }
                }
                //不明な命令は無視
                continue;
            }
            return false;
        }

        public class animedata
        {
            public uint image_pointer;
            public uint palette_pointer; //追加魔法用 魔法はシートごとにパレットを切り替えられる.
            public uint tsa_pointer;     //追加魔法CSA_Creator用 BGにTSAを使う.
            public int  height;          //画像の高さ

            public uint oam_pos;         //OBJ
            public uint oam2_pos;        //OBJ BG
            public uint image_number;

            public string imageHash;
        };
        public class image_data
        {
            public byte[] data;
            public uint write_addr;
        };

        //パレット数を求める.
        static int CalcPalette(string basedir,string imagefilename)
        {
            string fullpath = Path.Combine(basedir,imagefilename);
            Bitmap bitmap = ImageUtil.OpenBitmap(fullpath);
            if (bitmap == null)
            {//エラーファイルがないよ
                R.ShowWarning("最初に指定されている画像か見つかりません。\r\n{0}",fullpath);
                return 1; //とりあえず1パレットということにしておこう.どうせ他でエラーになるから.
            }
            
            int paletteCount = ImageUtil.GetPalette16Count(bitmap);
            if (paletteCount <= 1)
            {
                return 1;
            }
            else if (paletteCount <= 4)
            {//OAMは最大4パレット
                return 4;
            }
            else 
            {//ルール違反
                R.ShowWarning("この画像には4パレット以上の色が存在します。\r\nルールにより、利用できるのは、4パレットまでです。\r\nルールに違反しているので、一番よくつかわれている1パレットのアニメーションとして読み込みます。\r\n{0}",fullpath);
                return 1;
            }
        }

        public static animedata FindHash(string hash, Dictionary<string, animedata> animeDic)
        {
            foreach (var m in animeDic)
            {
                if (m.Value.imageHash == hash)
                {
                    return m.Value;
                }
            }
            return null;
        }
        public static string ImportBattleAnime(string filename
            , uint battleanime_baseaddress   //戦闘アニメの書き換えるアドレス
            , uint top_battleanime_baseaddress
            , uint bottum_battleanime_baseaddress
            )
        {
            try
            {
                return 
                    ImportBattleAnimeLow(filename
                    , battleanime_baseaddress   //戦闘アニメの書き換えるアドレス
                    , top_battleanime_baseaddress
                    , bottum_battleanime_baseaddress
                    );
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                return R.ExceptionToString(e);
            }
        }

        static string ImportBattleAnimeLow(string filename
            , uint battleanime_baseaddress   //戦闘アニメの書き換えるアドレス
            , uint top_battleanime_baseaddress
            , uint bottum_battleanime_baseaddress
            )
        {
            byte[] sectionData = new byte[0xC * 4];
            List<byte> frameData = new List<byte>();
            List<byte> frameDataMode2 = new List<byte>(); //Mode2とMode4は特別処理が必要.
            ImportOAM oam = new ImportOAM();
            oam.SetBaseDir(Path.GetDirectoryName(filename));

            //変換したアニメの記録
            Dictionary<string, animedata> animeDic = new Dictionary<string, animedata>();
            uint image_number = 0;
            uint countLoopFrame = U.NOT_FOUND;

            //現在のモード(modeは1から数えるので、mode+1 が実際のモードです)
            int mode = 0;
            //mode1はmode2 を、 mode3はmode4 を共に生成しないという特殊ルールがある.
            bool isMode1 = true; //mode1(つまり mode=0)なのでいきなり特殊処理
            //パレット数
            int paletteCount = -1; //最初の画像を解析するときに 1 または 4が入ります.

            InputFormRef.DoEvents(null, "Check..");

            string[] lines = File.ReadAllLines(filename);
            if (!checkC47Code(lines))
            {
                return R._("ユーザによって取り消されました。");
            }
            if (!checkC26Code(lines))
            {
                return R._("ユーザによって取り消されました。");
            }
            if (!checkC27Code(lines))
            {
                return R._("ユーザによって取り消されました。");
            }
            CheckAndPatchC01Code(lines);
            CheckAndPatchC48Code(lines);

            for(int i = 0 ; i < lines.Length ; i++)
            {
                int lineCount = i + 1;
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipCommentWithCharpAndAtmark(line);
                if (line == "")
                {
                    continue;
                }
                InputFormRef.DoEvents(null,"Parse:"+ lineCount);

                if (line[0] == '~')
                {//モード終端

                    //フレーム終端命令
                    U.append_u32(frameData, 0x80000000);
                    if (isMode1) U.append_u32(frameDataMode2, 0x80000000);

                    if (countLoopFrame != U.NOT_FOUND)
                    {//ループ終端の C01 がないまま モードが終わりました。
                        return R.Error(("ループ終端の C01 がないまま モードが終わりました。\r\nFile: {0} line:{1}"), filename, lineCount);
                    }

                    mode++;
                    if (mode >= 0xC )
                    {//シート 0xCまでいったら終了
                        break;
                    }
                    //次のデータの開始位置を記録.
                    U.write_u32(sectionData, (uint)(mode * 4), (uint)frameData.Count);

                    if (isMode1)
                    {
                        frameData.AddRange(frameDataMode2);
                        frameDataMode2.Clear();

                        mode++;

                        //次のデータの開始位置を記録.
                        U.write_u32(sectionData, (uint)(mode * 4), (uint)frameData.Count);
                    }

                    if (mode == 0x3 - 1)
                    {//mode3 は mode4を共に生成する. mode0とmode1の処理はすでに完了しているのでmode3だけチェック
                        isMode1 = true;
                    }
                    else
                    {
                        isMode1 = false;
                    }
                    continue;
                }

                if (line[0] == 'C')
                {//85コマンド
                    uint command = U.atoh(line.Substring(1));

                    if (command == 0x01 && countLoopFrame != U.NOT_FOUND)
                    {//ループ終端なので作成する.
                        if (countLoopFrame > 0xff)
                        {//ループが長すぎます。 0xffまでです。
                            return R.Error(("ループが長すぎます。 0xffまでです。\r\nFile: {0} line:{1}"), filename, lineCount);
                        }
                        if (countLoopFrame < 0x3)
                        {//ループが短すぎる。 0x3は必要です。
                            return R.Error(("ループが短すぎます。 最低でも0x3フレームの画像が必要です。\r\nFile: {0} line:{1}"), filename, lineCount);
                        }

                        command = (countLoopFrame << 8) | 0x00000001;
                        countLoopFrame = U.NOT_FOUND; //ループ終わり.
                    }

                    uint a = (command & 0x00FFFFFF) | 0x85000000;
                    U.append_u32(frameData, a);
                    if(isMode1) U.append_u32(frameDataMode2, a);
                        
                    if (command == 0x0D)
                    {//0x0D の場合は、00 00 00 80 フレームを追加します.
                        U.append_u32(frameData, 0x80000000);
                        if (isMode1) U.append_u32(frameDataMode2, 0x80000000);
                    }
                    continue;
                }

                if (line[0] == 'L')
                {//ループ
                    if (countLoopFrame != U.NOT_FOUND)
                    {//ループが終わるまでに再度ループが開始されました。
                        return R.Error("ループが終わるまでに再度ループが開始されました。\r\nまずは前のループを0x01で終わらせてください。\r\n\r\nFile: {0} line:{1}", filename, lineCount);
                    }
                    if (!isMakeingLoop(lines,i))
                    {//ループ直前に C05 or C04 が必要です。
                        return R.Error("ループの直前に C05 or C04 が必要です。\r\nFile: {0} line:{1}", filename, lineCount);
                    }
                    //C01 が出るまで監視します.
                    countLoopFrame = 0;
                    continue;
                }

                if (line[0] == 'S')
                {//音楽再生
                    uint music = U.atoh(line.Substring(1));
                    uint a = ((music & 0xFFFF) << 8) | 0x85000048;
                    U.append_u32(frameData, a);
                    if (isMode1) U.append_u32(frameDataMode2, a);

                    continue;
                }

                if (U.isnum(line[0]) && line.IndexOf("p-") > 0)
                {//86コマンド 画像
                    uint frameSec = U.atoi(line); //フレーム秒

                    if (countLoopFrame != U.NOT_FOUND)
                    {//ループ処理が有効な場合、ループする秒を数える.
                        countLoopFrame += 3;
                    }

                    //p- filename
                    string imagefilename = parsePFilename(line);
                    if (imagefilename.Length <= 0)
                    {
                        return R.Error("ファイル名が見つかりませんでした。\r\nFile: {0} line:{1}\r\n\r\nエラー内容:\r\n{2}", filename, lineCount, oam.ErrorMessage);
                    }
                    if (paletteCount <= 0)
                    {//最初の画像なので色数を調べます. 1か4になります.
                        paletteCount = CalcPalette(oam.GetBaseDir(), imagefilename);
                        oam.SetIsMultiPaletteOAM(paletteCount >= 4);
                    }

                    animedata anime;
                    if (animeDic.ContainsKey(imagefilename))
                    {//すでに変換ずみなのでアドレスデータのコピー
                        anime = animeDic[imagefilename];
                    }
                    else
                    {//未登録なので作成します.
                        //同一画像のハッシュがあるのでは?
                        string hash = ImageUtil.HashBitmap(imagefilename, oam.GetBaseDir());

                        anime = FindHash(hash, animeDic);
                        if (anime == null)
                        {//ハッシュにもない
                            anime = oam.MakeBattleAnime(imagefilename, isMode1);
                            if (anime == null)
                            {
                                return R.Error("エラーが発生しました。 \r\nFile: {0} line:{1}\r\n\r\nエラー内容:\r\n{2}", filename, lineCount, oam.ErrorMessage);
                            }

                            anime.image_number = image_number++;
                            anime.imageHash = hash;
                            animeDic[imagefilename] = anime;
                        }
                    }

                    uint a = (frameSec & 0xFFFF) | ((anime.image_number & 0xFF) << 16) | 0x86000000;
                    U.append_u32(frameData, a);  //86command
                    U.append_u32(frameData, anime.image_pointer);
                    U.append_u32(frameData, anime.oam_pos);
                    if (!isMode1)
                    {
                        continue;
                    }

                    U.append_u32(frameDataMode2, a); //86command
                    U.append_u32(frameDataMode2, anime.image_pointer);
                    U.append_u32(frameDataMode2, anime.oam2_pos);
                    continue;
                }

                //不明な命令なので無視する.

            }
            InputFormRef.DoEvents(null, "Term");

            //登録完了処理
            oam.Term();

            if (mode < 0xC)
            {
                return R._("アニメーションが途中で終ってしまいました。\r\nアニメーションはモード12まで必要です。\r\nこのファイルにはモード({0})までしかありません。\r\n\r\n戦闘アニメーションには、1,3,5,6,7,8,9,10,11,12 のモードが必要です。\r\nおそらく、どれかのモードが欠落しています。\r\nすべてのモードが正しく書かれているか、確認してください。\r\n", mode);
            }

            if (! checkOAMSize(oam.GetOAMSize()))
            {//OAMサイズ警告
                return R._("ユーザによって取り消されました。");
            }
            if (!checkFrameSize(frameData.Count))
            {//フレームサイズ警告
                return R._("ユーザによって取り消されました。");
            }

            //上書きされる古いアニメデータの領域を使いまわす.

            List<Address> recycle = new List<Address>();
            RecycleOldAnime(ref recycle, battleanime_baseaddress);
            RecycleAddress ra = new RecycleAddress(recycle);
            subConfilctArea(ra, battleanime_baseaddress, top_battleanime_baseaddress, bottum_battleanime_baseaddress);

            Dictionary<uint, ReColorMap> reColorMap = LoadReColorRule();


            Undo.UndoData undodata = Program.Undo.NewUndoData("ImportBattleAnime", Path.GetFileName(filename));

            //圧縮して書き込みます.
            byte[] z = oam.GetRightToLeftOAM_Z();
            ra.WriteAndWritePointer(battleanime_baseaddress + 20, z , undodata);

            z = oam.GetLeftToRightOAM_Z();
            ra.WriteAndWritePointer(battleanime_baseaddress + 24, z, undodata);

            z = MakeBattle4Palette_Z(GetFirstImage(oam.GetBaseDir(),lines), oam.GetPalette(), reColorMap);
            ra.WriteAndWritePointer(battleanime_baseaddress + 28, z, undodata);

            List<image_data> images = oam.GetImages();
            for(int i = 0 ; i < images.Count ; i++)
            {
                images[i].write_addr = ra.Write(images[i].data, undodata);
            }

            //画像の書き込みアドレスが決定したら、画像ポインタをかかないといけないFrameDataを更新します。
            z = frameData.ToArray();
            string errorFrame = updateFrameDataAddress(z, images);
            if (errorFrame != "")
            {
                return R.Error("OAMフレーム更新中にエラーが発生しました。\r\nこのエラーが頻繁に出る場合は、アニメデータと一緒にreport7zを送ってください。") + "\r\n" + errorFrame;
            }
            z = LZ77.compress(z);
            ra.WriteAndWritePointer(battleanime_baseaddress + 16, z, undodata);

            //セクションデータ(固定長だがそのまま使うと テーブル拡張時に失敗する.)
            ra.WriteAndWritePointer(battleanime_baseaddress + 12, sectionData, undodata);

            //もし、リサイクルできない端数が残ったら、それらは0x00で総クリアする
            ra.BlackOut(undodata);

            Program.Undo.Push(undodata);
            return "";
        }

        public class ReColorMap
        {
//            public uint Palyer;  keyに移動
            public uint Enemy;
            public uint NPC;
            public uint Gray;
        };


        //色割り当てデータのロード
        static Dictionary<uint, ReColorMap> LoadReColorRule()
        {
            Dictionary<uint, ReColorMap> map = new Dictionary<uint, ReColorMap>();
            string filename = U.ConfigDataFilename("battleanime_auto_recolor_");
            if (!U.IsRequiredFileExist(filename))
            {
                return map;
            }

            string[] lines = File.ReadAllLines(filename);

            for (int i = 0; i < lines.Length; i++)
            {
                int lineCount = i + 1;
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipCommentWithCharpAndAtmark(line);
                if (line == "")
                {
                    continue;
                }
                string[] sp = line.Split('\t');
                if (sp.Length < 4)
                {
                    continue;
                }
                uint player = U.atoh(sp[0]);
                if (player <= 0)
                {
                    continue;
                }

                ReColorMap m = new ReColorMap();
                m.Enemy = U.atoh(sp[1]);
                m.NPC = U.atoh(sp[2]);
                m.Gray = U.atoh(sp[3]);
                map[player] = m;
            }

            return map;
        }

#if DEBUG
        //ReColor用の色割り当てを作成します。
        //既存のバトルアニメの色配色から、カラー割り当てを模倣します.
        public static Dictionary<uint, ReColorMap> MakeReColorRule(uint top_battleanime_baseaddress, uint bottum_battleanime_baseaddress)
        {
//            Dictionary<uint, ReColorMap> map = new Dictionary<uint, ReColorMap>();
            Dictionary<uint, ReColorMap> map = LoadReColorRule();
            uint id = 0;
            for (uint p = top_battleanime_baseaddress; p <= bottum_battleanime_baseaddress; p += 32 , id++ )
            {
                uint palette = Program.ROM.p32(p + 28);
                if (! U.isSafetyOffset(palette))
                {
                    continue;
                }

                byte[] palette_bin = LZ77.decompress(Program.ROM.Data,palette);
                if (palette_bin.Length < 2 * 16 * 4)
                {//パレットが足りない 16色*4配色必要.
                    continue;
                }

                for (uint i = 0; i < 16; i++ )
                {
                    uint player = U.u16(palette_bin, (i * 2) + (0));
                    if (map.ContainsKey(player))
                    {//既に変換を知っている色なので、次へ.
                        continue;
                    }

                    uint enemy = U.u16(palette_bin, (i * 2) + (2 * 16 * 1));
                    uint npc = U.u16(palette_bin, (i * 2) + (2 * 16 * 2));
                    uint gray = U.u16(palette_bin, (i * 2) + (2 * 16 * 3));
                    if (player == enemy || player == npc || player == gray )
                    {//同じ色が使われているので無視.
                        continue;
                    }
                    if (npc == gray)
                    {
                        continue;
                    }
                    string name = Program.ROM.getString(p);
                    Log.Debug(U.ToHexString(player) + "\t" + U.ToHexString(enemy) + "\t" + U.ToHexString(npc) + "\t" + U.ToHexString(gray) + "//" + U.To0xHexString(id) + " " + name);
                    ReColorMap m = new ReColorMap();
                    m.Enemy = enemy;
                    m.NPC = npc;
                    m.Gray = gray;
                    map[player] = m;
                }
            }

            return map;
        }
#endif
        static bool checkOAMSize(int size)
        {
            string error = checkOAMSizeSimple(size);
            if (error == "")
            {
                return true;
            }
            DialogResult dr = R.ShowYesNo(error + "\r\n" + R._("GBAは携帯機なので利用できるメモリには限界があります。\r\n正常に動作しない可能性があります。\r\n処理を続行してもよろしいですか？\r\n"));
            if (dr == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
        public static string checkOAMSizeSimple(int size)
        {
            //tikiが言うには、22512(0x57F0)らしい
            int limit = 22512;

            if (size > limit)
            {
                string error = R._("このアニメーションは、あまりに巨大です。\r\n消費OAMデータ{0}バイト\r\n限界OAMサイズ{1}バイト\r\n", size, limit);
                return error;
            }
            return "";
        }
        static bool checkFrameSize(int size)
        {
            string error = checkFrameSizeSimple(size);
            if (error == "")
            {
                return true;
            }
            DialogResult dr = R.ShowYesNo(error + "\r\n" + R._("GBAは携帯機なので利用できるメモリには限界があります。\r\n正常に動作しない可能性があります。\r\n処理を続行してもよろしいですか？\r\n"));
            if (dr == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
        public static string checkFrameSizeSimple(int size)
        {
            int limit;
            limit = 10200;

            if (size > limit)
            {
                string error = R._("このアニメーションは、あまりに巨大です。\r\n消費フレームデータ{0}バイト\r\n限界フレームサイズ{1}バイト\r\n", size, limit);
                return error;
            }
            return "";
        }

        static bool checkCCode(string[] lines, uint search_code, char syntaxsuger = ' ')
        {
            for (int i = 0; i < lines.Length; i++)
            {
                int lineCount = i + 1;
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipCommentWithCharpAndAtmark(line);
                if (line == "")
                {
                    continue;
                }
                if (syntaxsuger != ' ')
                {
                    if (syntaxsuger == line[0])
                    {//Sなどの簡易表記.
                        return true;
                    }
                }
                if (line[0] != 'C')
                {
                    continue;
                }
                uint command = U.atoh(line.Substring(1)) & 0xFF;
                if (command == search_code)
                {
                    return true;
                }
            }
            return false;
        }
        static bool checkC26Code(string[] lines)
        {
            if (!checkCCode(lines, 0x26))
            {//Code26 は入っていない
                return true;
            }
            DialogResult dr = R.ShowNoYes("このスクリプトには、 C26命令が含まれています。\r\nこの命令があると正しくインポートできません。\r\nC26命令をコメントアウトしてから再インポートすることをお勧めします。\r\n\r\nそれでも、インポート処理を続行してもよろしいですか？");
            if (dr == DialogResult.No)
            {
                //ユーザが拒否
                return false;
            }
            //続行
            return true;
        }
        static bool checkC27Code(string[] lines)
        {
            if (!checkCCode(lines, 0x27))
            {//Code27 は入っていない
                return true;
            }
            DialogResult dr = R.ShowNoYes("このスクリプトには、 C27命令が含まれています。\r\nこの命令があると正しくインポートできません。\r\nC27命令をコメントアウトしてから再インポートすることをお勧めします。\r\n\r\nそれでも、インポート処理を続行してもよろしいですか？");
            if (dr == DialogResult.No)
            {
                //ユーザが拒否
                return false;
            }
            //続行
            return true;
        }

        static bool checkC47Code(string[] lines)
        {
            if (!checkCCode(lines, 0x47))
            {//Code47 は入っていない
                return true;
            }
            DialogResult dr = R.ShowNoYes("このスクリプトには、 C47命令が含まれています。\r\nこの命令は、マントコマンドの構造体を設定していないと動作しません。\r\n(C01ハックパッチがあるので、C47命令を利用することもうありません。)\r\n\r\nインポート処理を続行してもよろしいですか？");
            if (dr == DialogResult.No)
            {
                //ユーザが拒否
                return false;
            }
            //続行
            return true;
        }
        public static void CheckAndPatchC01Code(string[] lines)
        {
            if (PatchUtil.SearchC01HackPatch())
            {
                return ;
            }

            if (!checkCCode(lines, 0x01))
            {//Code01 は入っていない
                return;
            }
            HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.C01Hack_By_ImageBattleAnimation);
        }
        public static void CheckAndPatchC48Code(string[] lines)
        {
            if (PatchUtil.SearchC48HackPatch())
            {
                return;
            }

            if (!checkCCode(lines, 0x48, 'S'))
            {//Code48 は入っていない
                return;
            }
            HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.C48Hack_By_ImageBattleAnimation);
        }

        //最初にでてくる画像を検出
        static string GetFirstImage(string basedir,string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                int lineCount = i + 1;
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipCommentWithCharpAndAtmark(line);
                if (line == "")
                {
                    continue;
                }
                if (U.isnum(line[0]) && line.IndexOf("p-") > 0)
                {//86コマンド 画像
                    string imagefilename = parsePFilename(line);
                    if (imagefilename.Length > 0)
                    {
                        return Path.Combine(basedir, imagefilename);
                    }
                }
            }
            return "";
        }
        //java シリアライズみたいな形式で取得
        public static void ExportBattleAnimeOnFEditorSerialize(
              string filename    //書き込むファイル名
            , uint hintData
            , uint sectionData
            , uint frameData
            , uint rightToLeftOAM
            , uint leftToRightOAM
            , uint palettes
            , int palette_count
            )
        {
            try
            {
                ExportBattleAnimeOnFEditorSerializeLow(
                      filename    //書き込むファイル名
                    , hintData
                    , sectionData
                    , frameData
                    , rightToLeftOAM
                    , leftToRightOAM
                    , palettes
                    , palette_count
                    );
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                R.ShowStopError(R.ExceptionToString(e));
            }
        }


        //java シリアライズみたいな形式で取得
        static void ExportBattleAnimeOnFEditorSerializeLow(
              string filename    //書き込むファイル名
            , uint hintData
            , uint sectionData
            , uint frameData
            , uint rightToLeftOAM
            , uint leftToRightOAM
            , uint palettes
            , int palette_count
            )
        {
            string basename = Path.GetFileNameWithoutExtension(filename);
            string basedir = Path.GetDirectoryName(filename);

            uint hintData_offset = U.toOffset(hintData); //名前のヒント
            uint sectionData_offset = U.toOffset(sectionData); //int[0xC]個 ヘッダの区切りバイト 絶対値

            //解凍する.
            //固定長のsectionData以外はLZ77で圧縮されている.
            byte[] frameData_UZ = UnCompressFrame(frameData); //画像をどう切り出すかを提起したデータ
            byte[] rightToLeftOAM_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(rightToLeftOAM)); //OAM
            byte[] leftToRightOAM_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(leftToRightOAM)); //OAM
            byte[] palettes_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(palettes)); //Palette

            List<uint> animeSeet = new List<uint>();

            for (uint sectionDataIndex = 0; sectionDataIndex < 0xC; sectionDataIndex++)
            {
                //sectionDataデータは、 int sectionData[0x0C] として、
                //frameData_UZへの参照位置を返します.
                uint frameDataStart, frameDataEnd;
                getSectionDataStartEnd(sectionDataIndex, sectionData_offset, (uint)frameData_UZ.Length
                    , out frameDataStart, out frameDataEnd);

                //frameDataは4バイトずつのデータからなり、
                //?? ?? ?? 0x86 のあとには、4バイトの画像へのポインタと、 4バイトの 変換する OAMへの絶対位置を返します.
                uint frameI = 0;
                for (uint n = frameDataStart; n < frameDataEnd; n += 4)
                {
                    if (frameData_UZ[n + 3] != 0x86)
                    {//86命令以外相手にしない.
                        continue;
                    }

                    uint imageP = U.p32(frameData_UZ, n + 4);

                    //知らない画像ならば、リストに追加.
                    int imageIndex = animeSeet.IndexOf(imageP);
                    if (imageIndex < 0)
                    {
                        imageIndex = animeSeet.Count;
                        animeSeet.Add(imageP);

                        //初めて遭遇したイメージなのでファイルに出力します.
                        string imagefilename = Path.Combine(basedir, basename + " Sheet " + (imageIndex + 1) + ".png");
                        byte[] image_UZ = LZ77.decompress(Program.ROM.Data, imageP);

                        Bitmap bitmap = ImageUtil.ByteToImage16Tile(SEAT_TILE_WIDTH * 8, SEAT_TILE_HEIGHT * 8, image_UZ, 0, palettes_UZ, 0, 0);
                        bitmap = ImageUtil.Copy(bitmap, 0, 0, SEAT_TILE_WIDTH * 8 + 8, SEAT_TILE_HEIGHT * 8);
                        ImageUtil.AppendPaletteMark(bitmap, palette_count);
                        ImageUtil.BlackOutUnnecessaryColors(bitmap, palette_count);
                        U.BitmapSave(bitmap, imagefilename);
                        bitmap.Dispose();
                    }
                    //ポインタを相対番号に変換します.
                    U.write_u32(frameData_UZ, n + 4, (uint)imageIndex);

                    frameI++;
                    n += 8;
                }
            }

            List<byte> binData = new List<byte>();
            binData.AddRange(new byte[] { 0xAC, 0xED, 0x00, 0x05, 0x73, 0x72, 0x00, 0x0C, 0x6A, 0x61, 0x76, 0x61, 0x2E, 0x69, 0x6F, 0x2E, 0x46, 0x69, 0x6C, 0x65, 0x04, 0x2D, 0xA4, 0x45, 0x0E, 0x0D, 0xE4, 0xFF, 0x03, 0x00, 0x01, 0x4C, 0x00, 0x04, 0x70, 0x61, 0x74, 0x68, 0x74, 0x00, 0x12, 0x4C, 0x6A, 0x61, 0x76, 0x61, 0x2F, 0x6C, 0x61, 0x6E, 0x67, 0x2F, 0x53, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x3B, 0x78, 0x70, 0x74 });

            //フレームデータ
            {
                string frameDataBaseFilename = basename + " Frame Data.dmp";
                string framefilename = Path.Combine(basedir, frameDataBaseFilename);
                U.WriteAllBytes(framefilename, frameData_UZ);

                U.append_big16(binData , (uint)frameDataBaseFilename.Length);
                U.append_range(binData, frameDataBaseFilename);
            }
            binData.AddRange(new byte[] { 0x77, 0x02, 0x00, 0x5C, 0x78, 0x73, 0x72, 0x00, 0x13, 0x6A, 0x61, 0x76, 0x61, 0x2E, 0x75, 0x74, 0x69, 0x6C, 0x2E, 0x41, 0x72, 0x72, 0x61, 0x79, 0x4C, 0x69, 0x73, 0x74, 0x78, 0x81, 0xD2, 0x1D, 0x99, 0xC7, 0x61, 0x9D, 0x03, 0x00, 0x01, 0x49, 0x00, 0x04, 0x73, 0x69, 0x7A, 0x65, 0x78, 0x70});
            U.append_big32(binData , (uint)animeSeet.Count);
            binData.AddRange(new byte[] { 0x77 ,0x04 });
            U.append_big32(binData, (uint)animeSeet.Count);
            binData.AddRange(new byte[] { 0x73, 0x71, 0x00, 0x7E, 0x00, 0x00, 0x74});
            for (int imageIndex = 0; imageIndex < animeSeet.Count; imageIndex++)
            {
                string imagefilename = basename + " Sheet " + (imageIndex + 1) + ".png";
                U.append_big16(binData, (uint)imagefilename.Length);
                U.append_range(binData, imagefilename);

                if (imageIndex + 1 == animeSeet.Count)
                {
                    break;
                }
                binData.AddRange(new byte[] { 0x77, 0x02, 0x00, 0x5C, 0x78, 0x73, 0x71, 0x00, 0x7E, 0x00, 0x00, 0x74 });
            }
            binData.AddRange(new byte[] { 0x77, 0x02, 0x00, 0x5C, 0x78, 0x78, 0x75, 0x72, 0x00, 0x02, 0x5B, 0x49, 0x4D, 0xBA, 0x60, 0x26, 0x76, 0xEA, 0xB2, 0xA5, 0x02, 0x00, 0x00, 0x78, 0x70, 0x00, 0x00, 0x00, 0x03});

            byte[] hintNameBIN = Program.ROM.getBinaryData(hintData_offset, 12);
            hintNameBIN[11] = 0; //最後は0終端らしい.

            //よくわからないが、4バイトずつ反転させて記録するらしい.
            //0123456789AB
            //ABCDEFGHIJK0
            //32107654BA98
            //DCBAHGFE.KJI
            binData.AddRange(new byte[] { 
                     hintNameBIN[3],  hintNameBIN[2],  hintNameBIN[1],  hintNameBIN[0]
                    ,hintNameBIN[7],  hintNameBIN[6],  hintNameBIN[5],  hintNameBIN[4]
                    ,hintNameBIN[11], hintNameBIN[10], hintNameBIN[9],  hintNameBIN[8]
            });

            binData.AddRange(new byte[] {0x75, 0x72, 0x00, 0x02, 0x5B, 0x42, 0xAC, 0xF3, 0x17, 0xF8, 0x06, 0x08, 0x54, 0xE0, 0x02, 0x00, 0x00, 0x78, 0x70 });
            U.append_big32(binData, 4 * 12); //section uint * 12個の長さ

            byte[] sectionDataBin = Program.ROM.getBinaryData(sectionData_offset,4 * 12);
            binData.AddRange(sectionDataBin);

            //
            //75 71 00 7E 00 0A -> アニメシート 1個
            //75 71 00 7E 00 0C -> アニメシート 2個
            //75 71 00 7E 00 0E -> アニメシート 3個
            //75 71 00 7E 00 10 -> アニメシート 4個
            //75 71 00 7E 00 20 -> アニメシート 12個
            //つまり、以下の式になるらしい.
            //正しくは、 0x7E 00 00 + obj_number みたいだけど・・・

            uint object_number = (uint)(0x7E0000 + 8 + (animeSeet.Count * 2));

            binData.AddRange(new byte[] { 0x75, 0x71, 0x00});
            U.append_big24(binData, (uint)object_number);
            U.append_big32(binData, (uint)rightToLeftOAM_UZ.Length);
            binData.AddRange(rightToLeftOAM_UZ);


            binData.AddRange(new byte[] { 0x75, 0x71, 0x00});
            U.append_big24(binData, (uint)object_number);
            U.append_big32(binData, (uint)leftToRightOAM_UZ.Length);
            binData.AddRange(leftToRightOAM_UZ);

            binData.AddRange(new byte[] { 0x75, 0x71, 0x00});
            U.append_big24(binData, (uint)object_number);
            U.append_big32(binData, (uint)palettes_UZ.Length);
            binData.AddRange(palettes_UZ);

            //まとめて書き込み
            U.WriteAllBytes(filename, binData.ToArray());
            return;
        }
        //FE Editorの java シリアライズされたバトルアニメの読み込み
        public static string ImportBattleAnimeOnFEditorSerialize(string filename
            , uint battleanime_baseaddress   //戦闘アニメの書き換えるアドレス
            , uint top_battleanime_baseaddress
            , uint bottum_battleanime_baseaddress
            )
        {
            try
            {
                return 
                    ImportBattleAnimeOnFEditorSerializeLow( filename
                    , battleanime_baseaddress   //戦闘アニメの書き換えるアドレス
                    , top_battleanime_baseaddress
                    , bottum_battleanime_baseaddress
                    );
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                return R.ExceptionToString(e);
            }
        }


        //FE Editorの java シリアライズされたバトルアニメの読み込み
        static string ImportBattleAnimeOnFEditorSerializeLow(string filename
            , uint battleanime_baseaddress   //戦闘アニメの書き換えるアドレス
            , uint top_battleanime_baseaddress
            , uint bottum_battleanime_baseaddress
            )
        {
            string basename = Path.GetFileNameWithoutExtension(filename);
            string basedir = Path.GetDirectoryName(filename);

            //OAMのほうのformat
            //
            //\xxur(5C 78 78 75 72) + 0x38 
            // section uint * 12個
            //終端ヘッダー75 71 00 7E 00 + 0x5
            // rightToLeftOAM
            //終端ヘッダー75 71 00 7E 00 + 0x5
            // letToRightOAM
            //終端ヘッダー75 71 00 7E 00 + 0x5
            // パレット?
            //終端
            byte[] oamBIN = File.ReadAllBytes(filename);

            byte[] header = new byte[] { 0x5C, 0x78, 0x78, 0x75, 0x72 };
            uint header_skip = (uint)(0x38 + header.Length); //ヘッダーからよくわからないデータ 0x38 読み飛ばし.
            byte[] footer = new byte[] { 0x75, 0x71, 0x00, 0x7E, 0x00 };
            uint footer_skip = (uint)(0x5 + footer.Length);

            uint oam_start = U.Grep(oamBIN, header, 0);
            if (oam_start == U.NOT_FOUND)
            {//ファイルが壊れている OAM開始地点がありません
                //変種がいるらしい
                //\xpp*pxur(5C 78 70 * 70 78 75 72) + 0x38 
                header = new byte[] { 0x5C, 0x78, 0x70 };
                oam_start = U.Grep(oamBIN, header, 0);
                if (oam_start == U.NOT_FOUND)
                {
                    return R.Error("ファイルが壊れています OAM開始地点がありません");
                }

                byte[] header_term = new byte[] { 0x70, 0x78, 0x75, 0x72 };
                uint oam_start_term = U.Grep(oamBIN, header_term, oam_start);
                if (oam_start_term == U.NOT_FOUND)
                {
                    return R.Error("ファイルが壊れています OAM開始地点がありません");
                }
                header_skip = ((oam_start_term + (uint)header_term.Length) - oam_start) + 0x38;
            }
            //よくわからないデータ header_skip 読み飛ばし.
            oam_start += header_skip;

            //セクションデータ(0xC * 4の固定長)
            byte[] sectionData = U.subrange(oamBIN, oam_start, oam_start + (0xC * 4));
            oam_start += (0xC * 4);

            //次のデータへ
            oam_start = oam_start + footer_skip;

            //終端位置.
            uint end_data = U.Grep(oamBIN, footer, oam_start);
            if (end_data == U.NOT_FOUND)
            {
                return R.Error("ファイルが壊れています OAM終了地点がありません");
            }
            Debug.Assert((end_data - oam_start) % 12 == 0);

            //データ本体
            List<byte> rightToLeftOAM = new List<byte>(U.subrange(oamBIN, oam_start, end_data));

            //次のデータへ
            oam_start = end_data + footer_skip;

            //終端位置その2.
            end_data = U.Grep(oamBIN, footer, oam_start);
            if (end_data == U.NOT_FOUND)
            {
                return R.Error("ファイルが壊れています OAM終了地点がありません");
            }
            Debug.Assert((end_data - oam_start) % 12 == 0);

            //データ本体
            List<byte> leftToRightOAM = new List<byte>(U.subrange(oamBIN, oam_start, end_data));

            //終端にあるのはパレット?
            uint palette_start = end_data + footer_skip;
            byte[] order_palette = U.subrange(oamBIN, palette_start, (uint)oamBIN.Length);


            //frameのほうのformat
            //単純にフレームをdumpしているだけなので先頭から読めばok
            //ファイル名は、 oamのほうのデータに名前が埋め込まれている面倒なので決め打ちにする.
            string frameDataFilename = Path.Combine(basedir, basename + " Frame Data.dmp");
            if (!File.Exists(frameDataFilename))
            {
                return R.Error("フレームを定義するファイルがありません\r\n{0}", frameDataFilename);
            }
            byte[] frameBIN = File.ReadAllBytes(frameDataFilename);
            List<byte> frame = new List<byte>(frameBIN);

            {
                int i = 1;
                string imagefilename = Path.Combine(basedir, basename + " Sheet " + i + ".png");
                if (!File.Exists(imagefilename))
                {
                    return R.Error("画像シートが一枚もありません\r\n{0}", imagefilename);
                }
            }


            //画像シート
            byte[] palette = new byte[0x20];
            List<image_data> Images = new List<image_data>();
            for (int i = 1; i < 0xff; i++)
            {
                InputFormRef.DoEvents(null, "Sheet:" + i);

                string imagefilename = Path.Combine(basedir, basename + " Sheet " + i + ".png");
                Bitmap bitmap = ImageUtil.OpenBitmap(imagefilename);
                if (bitmap == null)
                {//ファイルがないのでここでシートは終わり.
                    break;
                }

                if (bitmap.Width != (SEAT_TILE_WIDTH + 1) * 8 || bitmap.Height != SEAT_TILE_HEIGHT * 8)
                {
                    string error = R.Error("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, (SEAT_TILE_WIDTH + 1) * 8, SEAT_TILE_HEIGHT * 8);
                    bitmap.Dispose();
                    return error;
                }
                //右側の余計なパレットヘッダー部分を削る
                bitmap = ImageUtil.Copy(bitmap, 0, 0, SEAT_TILE_WIDTH * 8, SEAT_TILE_HEIGHT * 8);

                //パレット
                byte[] imagepalette = ImageUtil.ImageToPalette(bitmap, 1);
                if (i == 1)
                {
                    Array.Copy(imagepalette, palette, 0x20);
                }
                else
                {//2個目以降は同一かどうか調べる.
                    if (U.memcmp(imagepalette, palette) != 0)
                    {
                        string ErrorPaletteMessage = ImageUtil.CheckPalette(bitmap.Palette
                            , palette
                            , 0
                            , 0
                            );
                        Log.Error(ErrorPaletteMessage);

                        string error = R.Error("パレットがほかと異なります\r\n\r\n画像ファイル名:{0}", imagefilename);
                        bitmap.Dispose();
                        return error;
                    }
                }

                byte[] seatimage = ImageUtil.ImageToByte16Tile(bitmap, bitmap.Width, bitmap.Height);

                image_data image_data = new image_data();
                image_data.data = LZ77.compress(seatimage);
                Images.Add(image_data);

                bitmap.Dispose();
            }

            InputFormRef.DoEvents(null, "Term");


            if (!checkOAMSize(rightToLeftOAM.Count))
            {//OAMサイズ警告
                return R._("ユーザによって取り消されました。");
            }
            if (!checkFrameSize(frame.Count))
            {//フレームサイズ警告
                return R._("ユーザによって取り消されました。");
            }

            //上書きされる古いアニメデータの領域を使いまわす.
            List<Address> recycle = new List<Address>();
            RecycleOldAnime(ref recycle, battleanime_baseaddress);

            RecycleAddress ra = new RecycleAddress(recycle);
            subConfilctArea(ra, battleanime_baseaddress, top_battleanime_baseaddress, bottum_battleanime_baseaddress);

            Dictionary<uint, ReColorMap> reColorMap = LoadReColorRule();

            Undo.UndoData undodata = Program.Undo.NewUndoData("ImportBattleAnimeOnFEditorSerialize", Path.GetFileName(filename));

            //圧縮して書き込みます.
            byte[] z;
            z = LZ77.compress(rightToLeftOAM.ToArray());
            uint addr = InputFormRef.AppendBinaryData(z, undodata);
            ra.WriteAndWritePointer(battleanime_baseaddress + 20, z, undodata);

            z = LZ77.compress(leftToRightOAM.ToArray());
            addr = InputFormRef.AppendBinaryData(z, undodata);
            ra.WriteAndWritePointer(battleanime_baseaddress + 24, z, undodata);

            //パレットは4つ 自軍、敵軍、友軍、グレーを作り、lz77圧縮する.
            if (order_palette.Length == 0x80)
            {//シリアライズされたデータにパレットが指定されていた
                z = LZ77.compress(order_palette);
            }
            else
            {//パレットが指定されていない場合は、自分で作る.
                z = MakeBattle4Palette_Z("", palette, reColorMap);
            }
            addr = InputFormRef.AppendBinaryData(z, undodata);
            ra.WriteAndWritePointer(battleanime_baseaddress + 28, z, undodata);

            //画像シートの書き込み.
            for (int i = 0; i < Images.Count; i++)
            {
                Images[i].write_addr = ra.Write(Images[i].data, undodata);
            }

            //画像の書き込みアドレスが決定したら、画像ポインタをかかないといけないFrameDataを更新します。
            z = frame.ToArray();
            string errorFrame = updateFrameDataAddress(z, Images);
            if (errorFrame != "")
            {
                return R.Error("OAMフレーム更新中にエラーが発生しました。\r\nこのエラーが頻繁に出る場合は、アニメデータと一緒にreport7zを送ってください。") + "\r\n" + errorFrame;
            }

            z = LZ77.compress(z);
            addr = InputFormRef.AppendBinaryData(z, undodata);
            ra.WriteAndWritePointer(battleanime_baseaddress + 16, z, undodata);

            //セクションデータ(固定長だがそのまま使うと テーブル拡張時に失敗する.)
            ra.WriteAndWritePointer(battleanime_baseaddress + 12, sectionData, undodata);

            //もし、リサイクルできない端数が残ったら、それらは0x00で総クリアする
            ra.BlackOut(undodata);

            Program.Undo.Push(undodata);
            return "";
        }

        static void RecycleOldAnime(ref List<Address> list, uint battleanime_baseaddress)
        {
            List<uint> seatNumberList = new List<uint>();
            MakeAllDataLength(list
                , false
                , ""
                , battleanime_baseaddress
                , seatNumberList);
        }
 
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly, string info, uint battleanime_baseaddress, List<uint> seatNumberList)
        {
            if (!U.isSafetyZArray(battleanime_baseaddress + 32 - 1))
            {
                return;
            }

            string name = Program.ROM.getString(battleanime_baseaddress,12);
            info += " " + name;
            uint sectionData = Program.ROM.p32(battleanime_baseaddress + 12); //セクションデータ 固定長 
            uint frameData_offset = Program.ROM.p32(battleanime_baseaddress + 16);     //
            uint rightToLeftOAM_offset = Program.ROM.p32(battleanime_baseaddress + 20); //OAM
            uint leftToRightOAM_offset = Program.ROM.p32(battleanime_baseaddress + 24); //OAM
            uint palettes_offset = Program.ROM.p32(battleanime_baseaddress + 28);             //パレット

            //解凍する.
            //固定長のsectionData以外はLZ77で圧縮されている.
            byte[] frameData_UZ = UnCompressFrame(frameData_offset); //画像をどう切り出すかを提起したデータ

            FEBuilderGBA.Address.AddAddress(list,sectionData
                , 0xC * 4
                , battleanime_baseaddress + 12
                , info + " section"
                , FEBuilderGBA.Address.DataTypeEnum.BIN
                );
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , battleanime_baseaddress + 16
                , info + " frame"
                , isPointerOnly
                , FEBuilderGBA.Address.DataTypeEnum.BATTLEFRAME);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , battleanime_baseaddress + 20
                , info + " rightToLeftOAM"
                , isPointerOnly
                , FEBuilderGBA.Address.DataTypeEnum.BATTLEOAM);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , battleanime_baseaddress + 24
                , info + " leftToRightOAM"
                , isPointerOnly
                , FEBuilderGBA.Address.DataTypeEnum.BATTLEOAM);
            FEBuilderGBA.Address.AddLZ77Pointer(list
                , battleanime_baseaddress + 28
                , info + " palettes"
                , isPointerOnly
                , FEBuilderGBA.Address.DataTypeEnum.LZ77PAL);

            int number = 0;
            for (int i = 0; i < frameData_UZ.Length; i += 4)
            {
                if (!U.isSafetyZArray(i + 8, frameData_UZ))
                {
                    break;
                }
                if (frameData_UZ[i + 3] != 0x86)
                {
                    continue;
                }
                //シート画像をリサイクルリストに突っ込む.
                uint imageOffset = U.u32(frameData_UZ, (uint)(i + 4));
                if (U.isPointer(imageOffset))
                {
                    imageOffset = U.toOffset(imageOffset);
                    if (seatNumberList.IndexOf(imageOffset) < 0)
                    {
                        //lz77された中にあるのでポインタは存在しない.
                        FEBuilderGBA.Address.AddLZ77Address(list, imageOffset
                            , U.NOT_FOUND
                            , info + " seat" + number
                            , isPointerOnly
                            , Address.DataTypeEnum.BATTLEFRAMEIMG
                            );
                        number++;
                        seatNumberList.Add(imageOffset);
                    }
                }
                i = i + 4 + 4; // 4+4+ 4 = 12
            }
        }

        static void subConfilctArea(RecycleAddress ra,uint battleanime_baseaddress,uint top_battleanime_baseaddress,uint bottum_battleanime_baseaddress)
        {
            for (uint p = top_battleanime_baseaddress; p <= bottum_battleanime_baseaddress; p += 32)
            {
                if (p == battleanime_baseaddress)
                {//自アニメ　別探索するので不要.
                    continue;
                }

                List<Address> check_anime = new List<Address>();
                RecycleOldAnime(ref check_anime , p);
                ra.SubRecycle(check_anime);
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors,uint battleanime_baseaddress,uint id,List<uint> seatNumberList)
        {
            if (!U.isSafetyZArray(battleanime_baseaddress + 32 - 1))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメの構造体が破損しています。"), id)
                    );
                return;
            }

            uint sectionData_offset = Program.ROM.p32(battleanime_baseaddress + 12); //セクションデータ 固定長 
            uint frameData_offset = Program.ROM.p32(battleanime_baseaddress + 16);     //
            uint rightToLeftOAM_offset = Program.ROM.p32(battleanime_baseaddress + 20); //OAM
            uint leftToRightOAM_offset = Program.ROM.p32(battleanime_baseaddress + 24); //OAM
            uint palettes_offset = Program.ROM.p32(battleanime_baseaddress + 28);             //パレット

            if (!U.isSafetyZArray(sectionData_offset + 4 * 12 - 1))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメのsectionDataが破損しています。"), id)
                    );
                return;
            }

            //解凍する.
            //固定長のsectionData以外はLZ77で圧縮されている.
            byte[] sectionData = Program.ROM.getBinaryData(sectionData_offset, 4 * 12);
            byte[] frameData_UZ = UnCompressFrame(frameData_offset); //画像をどう切り出すかを提起したデータ
            byte[] rightToLeftOAM_UZ = LZ77.decompress(Program.ROM.Data, rightToLeftOAM_offset); //OAM
            byte[] leftToRightOAM_UZ = LZ77.decompress(Program.ROM.Data, leftToRightOAM_offset); //OAM
            byte[] palettes_UZ = LZ77.decompress(Program.ROM.Data, palettes_offset); //Palette
            if (frameData_UZ.Length < 4)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメのframeDataが破損しています。"), id)
                    );
                return;
            }
            if (rightToLeftOAM_UZ.Length < 12)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメのrightToLeftOAMが破損しています。"), id)
                    );
                return;
            }
            if (!U.isPadding4(rightToLeftOAM_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメのrightToLeftOAMが破損しています。")
                    + "\r\n"
                    + R._("アドレス「{0}」は4で割り切れない数字です。\r\n実行時にクラッシュする可能性があります。", U.To0xHexString(rightToLeftOAM_offset)), id)
                    );
            }

            if (leftToRightOAM_UZ.Length < 12)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメのleftToRightOAMが破損しています。"), id)
                    );
                return;
            }
            if (!U.isPadding4(leftToRightOAM_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメのrightToLeftOAMが破損しています。")
                    + "\r\n"
                    + R._("アドレス「{0}」は4で割り切れない数字です。\r\n実行時にクラッシュする可能性があります。", U.To0xHexString(leftToRightOAM_offset)), id)
                    );
            }

            if (palettes_UZ.Length < 0x20 * 4)
            {
                if (palettes_UZ.Length < 0x20)
                {//昔バグで0x20しかかけていなかったんだよなあ
                    errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                        , R._("戦闘アニメのパレットの長さが正しくありません。"), id)
                        );
                    return;
                }
            }
            if (!U.isPadding4(palettes_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                    , R._("戦闘アニメのパレットのが破損しています。")
                    + "\r\n"
                    + R._("アドレス「{0}」は4で割り切れない数字です。\r\n実行時にクラッシュする可能性があります。", U.To0xHexString(palettes_offset)), id)
                    );
            }

            bool isAttackAnimation = MakeCheckError_IsAttackAnimation(sectionData, frameData_UZ, rightToLeftOAM_UZ);
//            if (isAttackAnimation)
//            {
//                MakeCheckError_AnimaStartCommand(errors, battleanime_baseaddress, id, sectionData, frameData_UZ, rightToLeftOAM_UZ);
//            }
            MakeCheckError_Frame(errors, battleanime_baseaddress, id, seatNumberList,isAttackAnimation,
                sectionData, frameData_UZ, rightToLeftOAM_UZ);

        }

        //攻撃アニメーションを有するデータ
        static bool MakeCheckError_IsAttackAnimation(byte[] sectionData, byte[] frameData_UZ, byte[] rightToLeftOAM_UZ)
        {
            for (int mode = 0; mode < 12; mode++)
            {
                if (mode == 1 - 1  //Mode 1  直接攻撃
                    || mode == 3 - 1  //Mode 3  遠隔攻撃
                    )
                {
                    uint i = U.u32(sectionData, (uint)mode * 4);

                    if (i + 4 >= frameData_UZ.Length)
                    {
                        return false;
                    }

                    if (frameData_UZ[i + 3] != 0x85)
                    {
                        continue;
                    }
                    uint code = frameData_UZ[i + 0];
                    if (code == 0x3 || code == 0x7)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static void MakeCheckError_AnimaStartCommand(List<FELint.ErrorSt> errors, uint battleanime_baseaddress, uint id, byte[] sectionData, byte[] frameData_UZ, byte[] rightToLeftOAM_UZ)
        {
            for (int mode = 0; mode < 12; mode++)
            {
                if (mode == 1 - 1  //Mode 1  直接攻撃
                    || mode == 3 - 1  //Mode 3  遠隔攻撃
                    || mode == 12 - 1 //Mode 11 攻撃ミス
                    )
                {
                    uint i = U.u32(sectionData, (uint)mode * 4);

                    if (i + 4 >= frameData_UZ.Length)
                    {
                        return;
                    }
                    if (frameData_UZ[i + 3] != 0x85)
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                            , R._("戦闘アニメのMode:{0}の冒頭のコマンドが 85 Command の 03 でありません。", mode + 1), id)
                            );
                        continue;
                    }
                    uint code = frameData_UZ[i + 0];
                    if (!(code == 0x3 || code == 0x7 || code == 0x53 || code == 0x4))
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                            , R._("戦闘アニメのMode:{0}の冒頭のコマンドが 85 Command の 03 でありません。", mode + 1), id)
                            );
                        continue;
                    }
                }
            }
        }

        static void MakeCheckError_Frame(List<FELint.ErrorSt> errors, uint battleanime_baseaddress, uint id, List<uint> seatNumberList,bool isAttackAnimation,
            byte[] sectionData, byte[] frameData_UZ, byte[] rightToLeftOAM_UZ)
        {
            int frameLimit = Math.Min(frameData_UZ.Length, U.Padding4(frameData_UZ.Length));
            for (int mode = 0; mode < 12; mode++)
            {
                uint i = U.u32(sectionData, (uint)mode * 4);

                if (i >= frameData_UZ.Length)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                        , R._("戦闘アニメのMode:{0}が壊れています。フレームデータが不足しています。", mode + 1), id)
                        );
                    return;
                }

                bool haveImage = false;
                for (; i < frameLimit; i += 4)
                {
                    if (i + 4 >= frameData_UZ.Length)
                    {
                        break;
                    }
                    if (frameData_UZ[i + 3] == 0x80)
                    {
                        break;
                    }
                    if (frameData_UZ[i + 3] == 0x85)
                    {
                        FELint.Check85Command(errors, frameData_UZ, i, FELint.Type.BATTLE_ANIME, battleanime_baseaddress, id);
                        continue;
                    }

                    if (frameData_UZ[i + 3] != 0x86)
                    {
                        continue;
                    }
                    haveImage = true;

                    //シートのサイズを検証
                    uint imageOffset = U.u32(frameData_UZ, (uint)(i + 4));
                    if (U.isPointer(imageOffset))
                    {
                        imageOffset = U.toOffset(imageOffset);
                        if (seatNumberList.IndexOf(imageOffset) < 0)
                        {
                            FELint.CheckLZ77(imageOffset, errors, FELint.Type.BATTLE_ANIME, battleanime_baseaddress, id);
                            seatNumberList.Add(imageOffset);
                        }
                    }
                    uint OAMAbsoStart = U.u32(frameData_UZ, i + 8);
                    DrawOAM oam = new DrawOAM();
                    string oamErrorMessage = oam.Lint(rightToLeftOAM_UZ, OAMAbsoStart);
                    if (oamErrorMessage != "")
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                            , R._("戦闘アニメのMode:{0}のrightToLeftOAMデータに破損があります。", mode + 1) + "\r\n" + oamErrorMessage, id)
                            );
                        return;
                    }

                    i = i + 4 + 4; // 4+4+ 4 = 12
                }
                if (!haveImage)
                {
                    if (mode + 1 == 12)
                    {//ミス時のモーションは存在しなくてもエラーにはならないならしい.
                    }
                    else if (isAttackAnimation == false && mode + 1 == 11)
                    {//攻撃アニメーションでなければ、mode11に画像がなくてもいいらしい.
                    }
                    else
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, battleanime_baseaddress
                            , R._("戦闘アニメのMode:{0}に画像がありません。", mode + 1), id)
                            );
                    }
                }
            }
        }
    }
}
