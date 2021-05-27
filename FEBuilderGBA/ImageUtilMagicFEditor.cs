using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    class ImageUtilMagicFEditor
    {
        //ROM内に記録される形式 高さはサイズから自動計算する
        public const int OBJ_SEAT_TILE_WIDTH = 256 / 8;
        public const int OBJ_SEAT_TILE_HEIGHT = 32 / 8; //実測値が32なんだよなあ

        public const int BG_SEAT_TILE_WIDTH = 256 / 8;
        public const int BG_SEAT_TILE_HEIGHT = 64 / 8;

        //スクリプトから読み込まれるサイズ
        public const int SRC_OBJ_SEAT_TILE_WIDTH = 488 / 8;
        public const int SRC_OBJ_SEAT_TILE_HEIGHT = 160 / 8;

        public const int SRC_BG_SEAT_TILE_WIDTH = 264 / 8;
        public const int SRC_BG_SEAT_TILE_HEIGHT = 64 / 8;

        //ゲーム中に表示する画面サイズ
        public const int SCREEN_TILE_WIDTH = 240 / 8;
        public const int SCREEN_TILE_HEIGHT = 64 / 8;


        public static uint SpellDataCount()
        {
            uint baseaddr = Program.ROM.p32(Program.ROM.RomInfo.magic_effect_pointer());
            uint baseid = Program.ROM.RomInfo.magic_effect_original_data_count();
            uint seachsrart_addr = baseaddr + (baseid * 4);
            uint p = seachsrart_addr;
            for (; p < Program.ROM.Data.Length - 4; p += 4)
            {
                uint d = Program.ROM.u32(p);
                if (U.isPointerOrNULL(d))
                {
                    continue;
                }
                break;
            }
            uint count = (p - baseaddr) / 4 ;
            if (count <= 0)
            {
                return 0;
            }
            uint a = count - 1;
            if (a >= 0xFD)
            {//0xFF,0xFEは予約されているので.
                return 0xFD;
            }
            return count - 1;
        }


        public static string ExportOBjFrameImage(uint frame
            , byte[] frameData
            , uint objRightToLeftOAM
            , uint objBGRightToLeftOAM
            , string basedir
            , string basename
            , List<uint> animeHash
            )
        {
            basename = basename + "o_";

            //0  frame16 byte1 x86
            //4  objImagePointer
            //8  OAMAbsoStart
            //12 OAMBGAbsoStart
            //16 bgImagePointer
            //20 objPalettePointer
            //24 bgPalettePointer
            uint OAMAbsoStart = U.u32(frameData, frame + 8);
            uint OAMBGAbsoStart = U.u32(frameData, frame + 12);
            uint objImagePointer = U.u32(frameData, frame + 4);
            uint objPalettePointer = U.u32(frameData, frame + 20);

            uint imageHash = (objImagePointer << 8) | (OAMAbsoStart & 0xff);
            string framefilename = "";
            int seatnumber = animeHash.IndexOf(imageHash);
            if (seatnumber >= 0)
            {//すでに出力しているのでカットする.
                framefilename = basename + seatnumber.ToString("000") + ".png";
                return framefilename;
            }

            byte[] objPalette = Program.ROM.getBinaryData(U.toOffset(objPalettePointer), 0x20);
            byte[] obj_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(objImagePointer));
            if (obj_UZ.Length <= 0)
            {//壊れたデータ
                Log.Error("obj_UZ", U.ToHexString(objImagePointer), "is null. broken frame.");
                //まれにOBJデータがnullになるらしい?
                obj_UZ = new byte[0x1000];
                //                return "";
            }

            //まだ出力していない画像なので作成します.
            seatnumber = animeHash.Count;
            animeHash.Add(imageHash);

            Log.Debug("obj: ", U.ToHexString(objImagePointer), obj_UZ.Length.ToString(), U.ToHexString(imageHash));

            int width = 256;
            int height = ImageUtil.CalcHeight(width, obj_UZ.Length);
            Bitmap obj = ImageUtil.ByteToImage16Tile(width, height, obj_UZ, 0, objPalette, 0);
            Bitmap tempCanvas = ImageUtil.Blank((ImageUtilOAM.SEAT_TILE_WIDTH - 2) * 8, ImageUtilOAM.SEAT_TILE_HEIGHT * 8 * 2, obj);

            width = 480;
            height = 160;
            Bitmap retImage = ImageUtil.Blank(width, height, obj);
            {
                //利用するOAMデータの開始位置
                //OAMは 12byte で 最初の1バイト目が 1になるまで続きます.
                ImageUtilOAM.DrawOAM oam = new ImageUtilOAM.DrawOAM();
                oam.SetIsMagicOAM(true);
                oam.Parse(Program.ROM.Data, objRightToLeftOAM + OAMAbsoStart);
                tempCanvas = oam.Draw(tempCanvas, obj);

                ImageUtil.BitBlt(retImage, 0, 0, width, height, tempCanvas, 0, 0);
            }
            {
                //利用するOAMデータの開始位置
                //OAMは 12byte で 最初の1バイト目が 1になるまで続きます.
                ImageUtilOAM.DrawOAM oam = new ImageUtilOAM.DrawOAM();
                oam.SetIsMagicOAM(true);
                oam.Parse(Program.ROM.Data, objBGRightToLeftOAM + OAMBGAbsoStart);
                tempCanvas = oam.Draw(tempCanvas, obj);

                ImageUtil.BitBlt(retImage, 240, 0, width, height, tempCanvas, 0, 0);
            }
            {
                //利用するOAMデータの開始位置
                //OAMは 12byte で 最初の1バイト目が 1になるまで続きます.
                ImageUtilOAM.DrawOAM oam = new ImageUtilOAM.DrawOAM();
                oam.SetIsMagicOAM(true);
                oam.Parse(Program.ROM.Data, objBGRightToLeftOAM + OAMBGAbsoStart);
                tempCanvas = oam.Draw(tempCanvas, obj);

                ImageUtil.BitBlt(retImage, 240, 0, width, height, tempCanvas, 0, 0);
            }

            ////パレットマークを右上に描画
            ImageUtil.AppendPaletteMark(retImage);
            //利用していないパレットを消す.
            ImageUtil.BlackOutUnnecessaryColors(retImage, 1);

            framefilename = basename + seatnumber.ToString("000") + ".png";
            if (!U.BitmapSave(retImage, Path.Combine(basedir, framefilename)))
            {
                return "";
            }

            return framefilename;
        }

        static string ExportBGFrameImage(
            uint frame
            , byte[] frameData
            , string basedir
            , string basename
            , List<uint> animeHash
            )
        {
            basename = basename + "b_";

            //0  frame16 byte1 x86
            //4  objImagePointer
            //8  dummyOAM?
            //12 dummyOAM?
            //16 bgImagePointer
            //20 objPalettePointer
            //24 bgPalettePointer
            uint bgPointer = U.u32(frameData, frame + 16);
            uint bgPalettePointer = U.u32(frameData, frame + 24);

            uint imageHash = (bgPointer);
            string framefilename = "";
            int seatnumber = animeHash.IndexOf(imageHash);
            if (seatnumber >= 0)
            {//すでに出力しているのでカットする.
                framefilename = basename + seatnumber.ToString("000") + ".png";
                return framefilename;
            }

            byte[] bgPalette = Program.ROM.getBinaryData(U.toOffset(bgPalettePointer), 0x20);
            byte[] bg_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(bgPointer));
            if (bg_UZ.Length <= 0)
            {//壊れたデータ
                Log.Error("bg_UZ", U.ToHexString(bgPointer),"is null. broken frame.");
                return "";
            }

            //まだ出力していない画像なので作成します.
            seatnumber = animeHash.Count;
            animeHash.Add(imageHash);

            Log.Debug("bg: ", U.ToHexString(bgPointer), bg_UZ.Length.ToString(), U.ToHexString(imageHash));

//            int width = 256 - 8 - 8; //CSA Creator
//            int height = 160;
            int width = 256;
            int height = 64;
            Bitmap bg = ImageUtil.ByteToImage16Tile(width, height, bg_UZ, 0, bgPalette, 0, 0);

            width = 264;
            height = 64;
            Bitmap retImage = ImageUtil.Blank(width, height, bgPalette, 0);
            ImageUtil.BitBlt(retImage, 0, 0, bg.Width, bg.Height, bg, 0, 0);


            ////パレットマークを右上に描画 
            ImageUtil.AppendPaletteMark(retImage);
            //利用していないパレットを消す.
            ImageUtil.BlackOutUnnecessaryColors(retImage, 1);

            framefilename = basename + seatnumber.ToString("000") + ".png";
            if (!U.BitmapSave(retImage, Path.Combine(basedir, framefilename)))
            {
                return "";
            }
            retImage.Dispose();
            return framefilename;
        }


        public static byte[] MakeDummyOAM(uint imageCount)
        {
            List<byte> oam = new List<byte>();
            for (int i = 0; i < imageCount; i++)
            {
                oam.Add(0x1);
                oam.Add(0x0);
                oam.Add(0x0);
                oam.Add(0x0);

                oam.Add(0x0);
                oam.Add(0x0);
                oam.Add(0x0);
                oam.Add(0x0);

                oam.Add(0x0);
                oam.Add(0x0);
                oam.Add(0x0);
                oam.Add(0x0);

                oam.Add(0x0);
                oam.Add(0x0);
                oam.Add(0x0);
                oam.Add(0x0);
            }
            return oam.ToArray();
        }
        public static string GetFullPath(string imagefilename, string basedir)
        {
            return Path.Combine(basedir, imagefilename);
        }

        //最後のOAMフレームの終端を求める.
        public static uint calcOAMLength(uint duumyOAMoffset, uint maxOAM)
        {
            duumyOAMoffset = U.toOffset(duumyOAMoffset);
            if (duumyOAMoffset == 0)
            {
                return 0;
            }
            uint limitter = duumyOAMoffset + maxOAM + 2048; //2kサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            uint oam = duumyOAMoffset + maxOAM;
            while (true)
            {
                if (oam >= limitter)
                {//危険 ROM末尾を突き破っている.
                    return 0;
                }

                uint a = Program.ROM.u32(oam);
                oam += 12;
                if (a == 0x01)
                {//終端フレーム
                    break;
                }
            }

            return oam - duumyOAMoffset;
        }

        static uint FindFrame(uint showFrameData, uint frame, byte[] frameData)
        {
            uint frameI = 0;

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frame + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            for (uint n = frame; n < limitter; n += 4)
            {
                if (frameData[n + 3] == 0x80) //0x80 Term
                {
                    if (frameData[n + 1] == 0x01) //0x00 0x01 0x00 0x80 の場合続くときがある
                    {
                        continue;
                    }
                    break;
                }
                if (frameData[n + 3] != 0x86) //0x86 pointer
                {
                    if (frameData[n + 3] == 0x85)
                    {
                        continue;
                    }
                    //不明な命令.
                    break;
                }

                if (showFrameData == frameI)
                {
                    return n;
                }

                frameI++;
                n += 24; // 24+4 = 28bytes
            }
            return U.NOT_FOUND;
        }

        static Bitmap DrawFrameImage(uint frame, byte[] frameData
            , uint objRightToLeftOAM
            , uint objBGRightToLeftOAM
            )
        {
            //0  frame16 byte1 x86
            //4  objImagePointer
            //8  OAMAbsoStart
            //12 OAMBGAbsoStart
            //16 bgImagePointer
            //20 objPalettePointer
            //24 bgPalettePointer

            uint objPalettePointer = U.u32(frameData, frame + 20);
            if (! U.isSafetyPointer(objPalettePointer))
            {
                return ImageUtil.BlankDummy();
            }
            byte[] objPalette = Program.ROM.getBinaryData(U.toOffset(objPalettePointer), 0x20);

            uint bgPalettePointer = U.u32(frameData, frame + 24);
            if (!U.isSafetyPointer(bgPalettePointer))
            {
                return ImageUtil.BlankDummy();
            }
            byte[] bgPalette = Program.ROM.getBinaryData(U.toOffset(bgPalettePointer), 0x20);


            Bitmap retImage = ImageUtil.Blank(SCREEN_TILE_WIDTH * 8, SCREEN_TILE_HEIGHT * 8, bgPalette, 0);

                
            //BGの読み込み
            uint bgPointer = U.u32(frameData, frame + 16);
            if (!U.isSafetyPointer(bgPointer))
            {
                return ImageUtil.BlankDummy();
            }

            byte[] bg_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(bgPointer));
            if (bg_UZ.Length > 0)
            {
                Log.Debug("bg: ", U.ToHexString(bgPointer), bg_UZ.Length.ToString());

                int rom_image_width = BG_SEAT_TILE_WIDTH * 8;
                int rom_image_height = ImageUtil.CalcHeight(rom_image_width, bg_UZ.Length);

                Bitmap bg = ImageUtil.ByteToImage16Tile(rom_image_width, rom_image_height, bg_UZ, 0, bgPalette, 0);
                ImageUtil.AppendPalette(retImage, bg, 1);
                ImageUtil.BitBlt(retImage, 0, 0, bg.Width, bg.Height, bg, 0, 0, 1);
            }

            //OBJの読み込み
            uint objImagePointer = U.u32(frameData, frame + 4);
            if (!U.isSafetyPointer(objImagePointer))
            {
                return ImageUtil.BlankDummy();
            }
            byte[] obj_UZ = LZ77.decompress(Program.ROM.Data, U.toOffset(objImagePointer));
            if (obj_UZ.Length > 0)
            {
                int width = 256;
                int height = ImageUtil.CalcHeight(width, obj_UZ.Length);
                if (height < 64)
                {
                    height = 64;
                }
                Bitmap obj = ImageUtil.ByteToImage16Tile(width, height, obj_UZ, 0, objPalette, 0);

                {
                    //利用するOAMデータの開始位置
                    //OAMは 12byte で 最初の1バイト目が 1になるまで続きます.
                    uint OAMBGAbsoStart = U.u32(frameData, frame + 12);
                    ImageUtilOAM.DrawOAM oam = new ImageUtilOAM.DrawOAM();
                    oam.SetIsMagicOAM(true);
                    oam.Parse(Program.ROM.Data, objBGRightToLeftOAM + OAMBGAbsoStart);
                    Bitmap tempCanvas = ImageUtil.Blank((ImageUtilOAM.SEAT_TILE_WIDTH - 2) * 8, ImageUtilOAM.SEAT_TILE_HEIGHT * 8 * 2, objPalette, 0);
                    tempCanvas = oam.Draw(tempCanvas, obj);

                    ImageUtil.BitBlt(retImage, 0, 0, tempCanvas.Width, tempCanvas.Height, tempCanvas, 0, 0, 0, 0);
                    tempCanvas.Dispose();
                }
                {
                    //利用するOAMデータの開始位置
                    //OAMは 12byte で 最初の1バイト目が 1になるまで続きます.
                    uint OAMAbsoStart = U.u32(frameData, frame + 8);
                    ImageUtilOAM.DrawOAM oam = new ImageUtilOAM.DrawOAM();
                    oam.SetIsMagicOAM(true);
                    oam.Parse(Program.ROM.Data, objRightToLeftOAM + OAMAbsoStart);
                    Bitmap tempCanvas = ImageUtil.Blank((ImageUtilOAM.SEAT_TILE_WIDTH - 2) * 8, ImageUtilOAM.SEAT_TILE_HEIGHT * 8 * 2, objPalette, 0);
                    tempCanvas = oam.Draw(tempCanvas, obj);

                    ImageUtil.BitBlt(retImage, 0, 0, tempCanvas.Width, tempCanvas.Height, tempCanvas, 0, 0, 0, 0);
                    tempCanvas.Dispose();
                }
            }


            return retImage;
        }


        //BG OAMデータはダミーデータしか作られないので、無視する.
        public static Bitmap Draw(uint showFrameData, uint frameData
            , uint objRightToLeftOAM, uint objBGRightToLeftOAM)
        {
            frameData = U.toOffset(frameData);
            objRightToLeftOAM = U.toOffset(objRightToLeftOAM);
            objBGRightToLeftOAM = U.toOffset(objBGRightToLeftOAM);
            uint frame = FindFrame(showFrameData, frameData, Program.ROM.Data);
            if (frame == U.NOT_FOUND)
            {
                return null;
            }
            Bitmap retImage = DrawFrameImage(frame, Program.ROM.Data, objRightToLeftOAM, objBGRightToLeftOAM);

            return retImage;
        }

        public static void ExportGif(
              string filename    //書き込むファイル名
            , uint frame
            , uint objRightToLeftOAM
            , uint objBGRightToLeftOAM
            )
        {
            objRightToLeftOAM = U.toOffset(objRightToLeftOAM);
            objBGRightToLeftOAM = U.toOffset(objBGRightToLeftOAM);

            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            //同じアニメを何度も出力しないように記録する.
            Dictionary<uint, Bitmap> animeHash = new Dictionary<uint, Bitmap>();
            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();

            frame = U.toOffset(frame);
            byte[] frameData = Program.ROM.Data;

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frame + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            int termCount = 0;
            for (uint n = frame; n < limitter; n += 4)
            {
                if (frameData[n + 3] == 0x80) //0x80 Term
                {//終端.
                    termCount++;
                    if (frameData[n + 1] == 0x01) //0x00 0x01 0x00 0x80 の場合続くときがある.
                    {
                        if (termCount == 1)
                        {
                            continue;
                        }
                    }
                    break;
                }
                else if (frameData[n + 3] == 0x85) //0x85 コマンド
                {
                    if (frameData[n] == 0x48)
                    {//音楽再生なのだが魔法ではS命令不可.
                    }
                    else
                    {//それ以外の 0x85命令
                    }
                    continue;
                }
                else if (frameData[n + 3] != 0x86)
                {//不明な命令なので終了
                    break;
                }

                //0x86 画像 pointer
                uint wait = U.u16(frameData, n);
                Bitmap bitmap = DrawFrameImage(n, frameData, objRightToLeftOAM, objBGRightToLeftOAM);

                bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, wait));


                n += 24; // 24+4 = 28bytes
            }

            //アニメgif生成
//            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps.ToArray(), 2, 0);
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
        }

        public static void Export(
             bool enableComment //コメントを出すかどうか
            ,string filename    //書き込むファイル名
            ,uint frame
            ,uint objRightToLeftOAM
            ,uint objBGRightToLeftOAM
            )
        {
            objRightToLeftOAM = U.toOffset(objRightToLeftOAM);
            objBGRightToLeftOAM = U.toOffset(objBGRightToLeftOAM);

            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            //読みやすいようにコメントを入れます.
            Dictionary<uint, string> Comment_85command_Dic = U.LoadDicResource(U.ConfigDataFilename("battleanime_85command_"));
            U.MapMarge(ref Comment_85command_Dic, U.LoadDicResource(U.ConfigDataFilename("magic_command_")));

            List<String> lines = new List<String>();
            string line;
            if (enableComment)
            {
                lines.Add("#######################################################");
                lines.Add("#");
                lines.Add("#" + R._("FEditorAdvにインポートする時には各行の#以降を削除してください。"));
                lines.Add("#######################################################");
            }
            lines.Add("/// - Start Animation");

            //同じアニメを何度も出力しないように記録する.
            List<uint> animeHash = new List<uint>();

            frame = U.toOffset(frame);
            byte[] frameData = Program.ROM.Data;

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frame + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            int termCount = 0;
            for (uint n = frame; n < limitter; n += 4)
            {
                if (frameData[n + 3] == 0x80) //0x80 Term
                {//終端.
                    termCount++;
                    if (frameData[n + 1] == 0x01) //0x00 0x01 0x00 0x80 の場合続くときがある.
                    {
                        line = "~~~";
                        if (termCount == 1)
                        {
                            if (enableComment)
                            {
                                line += "                               #miss terminator";
                            }
                            lines.Add(line);
                            continue;
                        }
                        else
                        {
                            if (enableComment)
                            {
                                line += "                               #terminator";
                            }
                            lines.Add(line);
                        }
                    }
                    break;
                }
                if (frameData[n + 3] == 0x85) //0x85 コマンド
                {
                    if (frameData[n] == 0x48)
                    {//音楽再生なのだが魔法ではS命令不可.
                        uint musicid = U.u16(frameData, n + 1);
                        uint id = U.u24(frameData, n);
                        line = "C" + U.ToHexString(id);
                        if (enableComment)
                        {
                            line += "                               #Sound " + musicid + " " + SongTableForm.GetSongName(musicid);
                        }
                        lines.Add(line);
                    }
                    else
                    {//それ以外の 0x85命令
                        uint id = U.u24(frameData, n);
                        line = "C" + U.ToHexString(id);
                        if (enableComment)
                        {
                            line += "                               #" + U.at(Comment_85command_Dic, frameData[n]);
                        }
                        lines.Add(line);
                    }
                    continue;
                }
                else if (frameData[n + 3] != 0x86)
                {//不明な命令なので終了
                    break;
                }

                //0x86 画像 pointer
                uint wait = U.u16(frameData, n);

                //OBJの出力
                string obj_framefilename = ExportOBjFrameImage(
                     n
                    , frameData
                    , objRightToLeftOAM
                    , objBGRightToLeftOAM
                    , basedir
                    , basename
                    , animeHash
                    );
                //BGの出力
                string bg_framefilename = ExportBGFrameImage(n, frameData
                    , basedir
                    , basename
                    , animeHash
                    );
                if (obj_framefilename == "")
                {
                    Log.Error("borken obj frame", n.ToString());
                    obj_framefilename = ImageUtil.SaveDummyImage(basedir, basename + "_broken_obj" + n + ".png"
                        , 480, 160);
                }
                if (bg_framefilename == "")
                {
                    Log.Error("borken bg frame", n.ToString());
                    bg_framefilename = ImageUtil.SaveDummyImage(basedir, basename + "_broken_bg"+n+".png"
                        , 264, 64);
                }

                line = "O  p- " + obj_framefilename;
                lines.Add(line);
                line = "B  p- " + bg_framefilename;
                lines.Add(line);

                //表示時間の出力.
                line = wait.ToString();
                lines.Add(line);

                n += 24; // 24+4 = 28bytes
            }

//            line = "~~~";
//            lines.Add(line);

            line = "/// - End of animation";
            lines.Add(line);

            //まとめて書き込み
            File.WriteAllLines(filename,lines);
        }

        //上書きされるアニメデータ領域を使いまわす
        public static void RecycleOldAnime(ref List<Address> recycle, string basename, bool isPointerOnly, uint magic_baseaddress)
        {
            uint frameData_offset = Program.ROM.p32(magic_baseaddress + 0);     //
            uint OBJRightToLeftOAM_offset = Program.ROM.u32(magic_baseaddress + 4); //OBJ OAM
            uint OBJLeftToRightOAM_offset = Program.ROM.u32(magic_baseaddress + 8); //OBJ OAM
            uint OBJRightToLeftBGOAM_offset = Program.ROM.u32(magic_baseaddress + 12); //OBJ BG OAM
            uint OBJLeftToRightBGOAM_offset = Program.ROM.u32(magic_baseaddress + 16); //OBJ BG OAM

            if (frameData_offset == 0)
            {
                return;
            }

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frameData_offset + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            //最大OAM数を求める.
            uint maxObjOAM = 0;
            uint maxBGOAM = 0;

            byte[] frameData = Program.ROM.Data;
            uint i;
            for (i = frameData_offset; i < limitter; i += 4)
            {
                if (frameData[i + 3] == 0x80)
                {//終端データ
                    if (frameData[i + 1] == 0x01) //0x00 0x01 0x00 0x80 の場合続くときがある.
                    {
                        continue;
                    }
                    i += 4;
                    break;
                }
                else if (frameData[i + 3] != 0x86)
                {
                    if (frameData[i + 3] == 0x85)
                    {
                        continue;
                    }
                    //不明な命令.
                    break;
                }

                //OBJ画像をリサイクルリストに突っ込む.
                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , (uint)(i + 4)
                    , basename + "OBJ"
                    , isPointerOnly
                    , Address.DataTypeEnum.LZ77IMG);

                //BG画像をリサイクルリストに突っ込む.
                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , (uint)(i + 16)
                    , basename + "BG"
                    , isPointerOnly
                    , Address.DataTypeEnum.LZ77IMG);

                //OBJパレットをリサイクルリストに突っ込む.
                FEBuilderGBA.Address.AddPointer(recycle
                    , (uint)(i + 20)
                    , 0x20   //16色*2バイト=0x20バイト
                    , basename + "OBJ PAL"
                    , Address.DataTypeEnum.PAL);

                //BGパレットをリサイクルリストに突っ込む.
                FEBuilderGBA.Address.AddPointer(recycle
                    , (uint)(i + 24)
                    , 0x20   //16色*2バイト=0x20バイト
                    , basename + "BG PAL"
                    , Address.DataTypeEnum.PAL);

                //最大OAMを求める.
                uint objOAM = U.u32(frameData, (uint)(i + 8));
                uint bgOAM = U.u32(frameData, (uint)(i + 12));
                if (objOAM > maxObjOAM) maxObjOAM = objOAM;
                if (bgOAM > maxBGOAM) maxBGOAM = bgOAM;

                i += 24; // 24+4 = 28bytes
            }

            if (i > limitter)
            {//リミッターを超えているので、危険なので、フレーム等の再利用はしない

            }
            else
            {
                FEBuilderGBA.Address.AddPointer(recycle
                    , magic_baseaddress + 0
                    , i - frameData_offset
                    , basename + "FRAME"
                    , Address.DataTypeEnum.MAGICFRAME_FEITORADV);

                FEBuilderGBA.Address.AddPointer(recycle
                    , magic_baseaddress + 4
                    , ImageUtilMagicFEditor.calcOAMLength(OBJRightToLeftOAM_offset, maxObjOAM)
                    , basename + "RihtToLeftOAM"
                    , Address.DataTypeEnum.MAGICOAM);

                FEBuilderGBA.Address.AddPointer(recycle
                    , magic_baseaddress + 8
                    , ImageUtilMagicFEditor.calcOAMLength(OBJLeftToRightOAM_offset, maxObjOAM)
                    , basename + "LeftRightOAM"
                    , Address.DataTypeEnum.MAGICOAM);

                FEBuilderGBA.Address.AddPointer(recycle
                    , magic_baseaddress + 12
                    , ImageUtilMagicFEditor.calcOAMLength(OBJRightToLeftBGOAM_offset, maxBGOAM)
                    , basename + "OBJ OAM"
                    , Address.DataTypeEnum.MAGICOAM);

                FEBuilderGBA.Address.AddPointer(recycle
                    , magic_baseaddress + 16
                    , ImageUtilMagicFEditor.calcOAMLength(OBJLeftToRightBGOAM_offset, maxBGOAM)
                    , basename + "BG OAM"
                    , Address.DataTypeEnum.MAGICOAM);
            }
        }

        public static void MakeCheckError(ref List<FELint.ErrorSt> errors, string basename, uint magic_baseaddress, uint magicindex)
        {
            uint frameData_offset = Program.ROM.p32(magic_baseaddress + 0);     //
            uint OBJRightToLeftOAM_offset = Program.ROM.u32(magic_baseaddress + 4); //OBJ OAM
            uint OBJLeftToRightOAM_offset = Program.ROM.u32(magic_baseaddress + 8); //OBJ OAM
            uint OBJRightToLeftBGOAM_offset = Program.ROM.u32(magic_baseaddress + 12); //OBJ BG OAM
            uint OBJLeftToRightBGOAM_offset = Program.ROM.u32(magic_baseaddress + 16); //OBJ BG OAM

            if (frameData_offset == 0)
            {
                return;
            }

            if (!U.isSafetyOffset(frameData_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAGIC_ANIME_EXTENDS, frameData_offset, basename + " " + R._("frameDataが範囲外です"), magicindex));
            }
            if (!U.isSafetyPointer(OBJRightToLeftOAM_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAGIC_ANIME_EXTENDS, OBJRightToLeftOAM_offset, basename + " " + R._("OBJRightToLeftOAMが範囲外です"), magicindex));
            }
            if (!U.isSafetyPointer(OBJLeftToRightOAM_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAGIC_ANIME_EXTENDS, OBJLeftToRightOAM_offset, basename + " " + R._("OBJRightToLeftOAMが範囲外です"), magicindex));
            }
            if (!U.isSafetyPointer(OBJRightToLeftBGOAM_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAGIC_ANIME_EXTENDS, OBJRightToLeftBGOAM_offset, basename + " " + R._("OBJRightToLeftOAMが範囲外です"), magicindex));
            }
            if (!U.isSafetyPointer(OBJLeftToRightBGOAM_offset))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAGIC_ANIME_EXTENDS, OBJLeftToRightBGOAM_offset, basename + " " + R._("OBJRightToLeftOAMが範囲外です"), magicindex));
            }

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frameData_offset + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            //最大OAM数を求める.
            uint maxObjOAM = 0;
            uint maxBGOAM = 0;

            byte[] frameData = Program.ROM.Data;
            uint i;
            for (i = frameData_offset; i < limitter; i += 4)
            {
                if (frameData[i + 3] == 0x80)
                {//終端データ
                    if (frameData[i + 1] == 0x01) //0x00 0x01 0x00 0x80 の場合続くときがある.
                    {
                        continue;
                    }
                    i += 4;
                    break;
                }
                else if (frameData[i + 3] != 0x86)
                {
                    if (frameData[i + 3] == 0x85)
                    {
                        FELint.Check85Command(errors, frameData, i, FELint.Type.MAGIC_ANIME_EXTENDS, magic_baseaddress, magicindex);
                        continue;
                    }
                    //不明な命令.
                    break;
                }

                //OBJ画像
                FELint.CkeckMagicLZ77Pointer((uint)(i + 4),ref errors, magic_baseaddress, basename + "OBJ", magicindex);
                //BG画像
                FELint.CkeckMagicLZ77Pointer((uint)(i + 16), ref errors, magic_baseaddress, basename + "BG", magicindex);

                //最大OAMを求める.
                uint objOAM = U.u32(frameData, (uint)(i + 8));
                uint bgOAM = U.u32(frameData, (uint)(i + 12));
                if (objOAM > maxObjOAM) maxObjOAM = objOAM;
                if (bgOAM > maxBGOAM) maxBGOAM = bgOAM;

                i += 24; // 24+4 = 28bytes
            }
        }
            
        public static string Import(
            string filename    //読み込むファイル名
            , uint magic_baseaddress   //魔法アニメの書き換えるアドレス
        )
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            List<byte> frameData = new List<byte>();
            List<ImageUtilOAM.image_data> bgImagesData = new List<ImageUtilOAM.image_data>();

            //変換したアニメの記録
            Dictionary<string, ImageUtilOAM.animedata> animeDic = new Dictionary<string, ImageUtilOAM.animedata>();

            //CSA Creatorだけど、53コマンドで背景を拡大しているケース
            bool csaCreatorExpands53Command = false; 

            int lineCount = 0;
            string[] lines = File.ReadAllLines(filename);

            uint image_bg_number = 0;
            uint image_obj_number = 0;
            ImageUtilOAM.ImportOAM oam = new ImageUtilOAM.ImportOAM();
            oam.SetBaseDir(Path.GetDirectoryName(filename));
            oam.SetIsMagicOAM(true);


            //魔法の開始には C00 が 連続して5つ必要.
            int countC00 = 0;
            while ( lineCount < lines.Length )
            {
                string line = lines[lineCount];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    lineCount++;
                    continue;
                }
                InputFormRef.DoEvents(null, "Parse:" + lineCount);

                if (line[0] == 'C' 
                    && U.atoh(line.Substring(1)) == 0 )
                {
                    U.append_u32(frameData, 0x85000000);
                    countC00++;
                    lineCount++;
                    continue;
                }
                break;
            }
            //C00 が5個必要。ないなら追加で入れる.
            for (int i = countC00; i < 5; i++)
            {
                U.append_u32(frameData, 0x85000000);
            }


            while ( lineCount < lines.Length )
            {
                string line = lines[lineCount];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    lineCount++;
                    continue;
                }
                InputFormRef.DoEvents(null, "Parse:" + lineCount);

                line = U.ClipCommentWithCharpAndAtmark(line);
                if (line == "")
                {
                    lineCount++;
                    continue;
                }

                if (line[0] == '~')
                {
                    U.append_u32(frameData, 0x80000100);

                    lineCount++;
                    continue;
                }

                if (line[0] == 'C')
                {//85コマンド
                    uint command = U.atoh(line.Substring(1));

                    if ((command & 0xFF) == 0x53)
                    {//CSACreatorだけど、背景を拡張していて切り詰めているケース
                        csaCreatorExpands53Command = (command & 0xFF00) > 0;
                        lineCount++;
                        continue;
                    }

                    uint a = (command & 0x00FFFFFF) | 0x85000000;
                    U.append_u32(frameData, a);

                    if (command == 0x0D)
                    {//0x0D の場合は、00 00 00 80 フレームを追加します.
                        U.append_u32(frameData, 0x80000000);
                    }
                    lineCount++;
                    continue;
                }

                if (line[0] == 'S')
                {//音楽再生
                    uint music = U.atoh(line.Substring(1));
                    uint a = ((music & 0xFFFF) << 8) | 0x85000048;
                    U.append_u32(frameData, a);

                    lineCount++;
                    continue;
                }

                if (line[0] != 'O' && line[0] != 'B')
                {
                    //不明な命令なので無視する.
                    lineCount++;
                    continue;
                }

                //O p- objblank.png
                //B p- bg3.png
                //1 
                ImageUtilOAM.animedata objAnimeData = null;
                ImageUtilOAM.animedata bgAnimeData = null;
                uint frameSec = U.NOT_FOUND;
                for (int n = 1; n <= 3; )
                {
                    line = lines[lineCount];
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        lineCount++;
                        continue;
                    }
                    line = U.ClipCommentWithCharpAndAtmark(line);
                    if (line == "")
                    {
                        lineCount++;
                        continue;
                    }

                    if (U.isnum(line[0]))
                    {
                        if (frameSec != U.NOT_FOUND)
                        {
                            return R.Error("時間指定が連続しています。\r\nO filename\r\nB filename\r\ntime\r\n\r\nFile:{0} Line:{1}\r\n", filename, lineCount + 1);
                        }
                        frameSec = U.atoi(line);

                        lineCount++;
                        n++;
                        continue;
                    }


                    string imagefilename = ImageUtilOAM.parsePFilename(line);
                    if (imagefilename.Length <= 0)
                    {
                        return R.Error("ファイル名が見つかりませんでした。\r\nFile: {0} line:{1}\r\n\r\nエラー内容:\r\n{2}", filename, lineCount, oam.ErrorMessage);
                    }

                    if (line[0] == 'O')
                    {
                        if (objAnimeData != null)
                        {
                            return R.Error("OBJ指定が連続しています。\r\nO filename\r\nB filename\r\ntime\r\n\r\nFile:{0} Line:{1}\r\n", filename, lineCount + 1);
                        }

                        string errormessage;
                        objAnimeData = ImportObjImageToData(imagefilename
                            , basedir
                            , animeDic
                            , oam
                            , ref image_obj_number
                            , out errormessage
                            );
                        if (objAnimeData == null)
                        {
                            return R.Error("OBJ画像をロードできません。 \r\n{2}\r\nFile:{0} Line:{1}", filename, lineCount + 1, errormessage);
                        }

                        lineCount++;
                        n++;
                        continue;
                    }
                    if (line[0] == 'B')
                    {
                        if (bgAnimeData != null)
                        {
                            return R.Error("BG指定が連続しています。\r\nO filename\r\nB filename\r\ntime\r\n\r\nFile:{0} Line:{1}\r\n", filename, lineCount + 1);
                        }

                        string errormessage;
                        bgAnimeData = ImportBGImageToData(imagefilename
                            , basedir
                            , animeDic
                            , bgImagesData
                            , csaCreatorExpands53Command
                            , out errormessage
                            );
                        if (bgAnimeData == null)
                        {
                            return R.Error("BG画像をロードできません。 \r\n{2}\r\nFile:{0} Line:{1}", filename, lineCount + 1, errormessage);
                        }
                        lineCount++;
                        n++;
                        continue;
                    }
                }
                if (objAnimeData == null)
                {
                    return R.Error("OBJ画像がありません。 \r\nO filename\r\nB filename\r\ntime\r\n\r\nみたいに、セットで登録する必要があります。\r\nFile:{0} Line:{1}", filename, lineCount + 1);
                }
                if (bgAnimeData == null)
                {
                    return R.Error(("IMAGE_POINTER or ZIMAGE_POINTER の指定が必要です。BG画像がありません。 \r\nO filename\r\nB filename\r\n"), filename, lineCount + 1);
                }
                if (frameSec == U.NOT_FOUND)
                {
                    return R.Error("時間指定がありません。 \r\nO filename\r\nB filename\r\ntime\r\n\r\nみたいに、セットで登録する必要があります。\r\nFile:{0} Line:{1}", filename, lineCount + 1);
                }

                //0x86コマンド
                {
                    uint a = (frameSec & 0xFFFF) | ((image_bg_number & 0xFF) << 16) | 0x86000000;
                    U.append_u32(frameData, a);
                    U.append_u32(frameData, objAnimeData.image_pointer);
                    U.append_u32(frameData, objAnimeData.oam_pos); //OBJ OAM
                    U.append_u32(frameData, objAnimeData.oam2_pos); //OBJ BG OAM
                    U.append_u32(frameData, bgAnimeData.image_pointer);
                    U.append_u32(frameData, objAnimeData.palette_pointer);
                    U.append_u32(frameData, bgAnimeData.palette_pointer);
                    image_bg_number++;
                }
            }
            
            InputFormRef.DoEvents(null, "Term");

            //終端.
            U.append_u32(frameData, 0x80000000);
            //登録完了処理
            oam.Term();

            Undo.UndoData undodata = Program.Undo.NewUndoData("ImportFEditorSCA" ,Path.GetFileName(filename));

            //上書きされるアニメデータ領域を使いまわす
            List<Address> recycle = new List<Address>();
            RecycleOldAnime(ref recycle,basename,false, magic_baseaddress);

            RecycleAddress ra = new RecycleAddress(recycle);
            //書き込みます.(魔法アニメはなぜか無圧縮)
            ra.WriteAndWritePointer(magic_baseaddress + 4, oam.GetRightToLeftOAM(), undodata);
            ra.WriteAndWritePointer(magic_baseaddress + 8, oam.GetLeftToRightOAM(), undodata);

            //BG用にダミーのOAMを作成
            //byte[] dummyOAM = MakeDummyOAM(image_bg_number);
            ra.WriteAndWritePointer(magic_baseaddress + 12, oam.GetRightToLeftOAMBG(), undodata);
            ra.WriteAndWritePointer(magic_baseaddress + 16, oam.GetLeftToRightOAMBG(), undodata);

            //BG
            for (int i = 0; i < bgImagesData.Count; i++)
            {
                bgImagesData[i].write_addr = ra.Write(bgImagesData[i].data, undodata);
            }
            //OBJ
            List<ImageUtilOAM.image_data> objImages = oam.GetImages();
            for (int i = 0; i < objImages.Count; i++)
            {
                objImages[i].write_addr = ra.Write(objImages[i].data, undodata);
            }

            //画像の書き込みアドレスが決定したら、画像ポインタをかかないといけないFrameDataを更新します。
            byte[] frameDataUZ = frameData.ToArray();
            string errorFrame = updateFrameDataAddress(frameDataUZ, bgImagesData, objImages);
            if (errorFrame != "")
            {
                return R.Error("OAMフレーム更新中にエラーが発生しました。\r\nこのエラーが頻繁に出る場合は、アニメデータと一緒にreport7zを送ってください。") + "\r\n" + errorFrame;
            }
            ra.WriteAndWritePointer(magic_baseaddress + 0, frameDataUZ, undodata);

            //端数の再利用的ない古いデータは0x00クリア.
            ra.BlackOut(undodata);

            Program.Undo.Push(undodata);
            return "";
        }

        static ImageUtilOAM.animedata ImportObjImageToData(string imagefilename
            ,string basedir
            , Dictionary<string, ImageUtilOAM.animedata> animeDic
            ,ImageUtilOAM.ImportOAM oam
            ,ref uint image_number
            ,out string errormessage
            )
        {
            string key = "OBJ" + imagefilename;

            ImageUtilOAM.animedata magic_animedata;
            if (animeDic.ContainsKey(key))
            {
                errormessage = "";
                magic_animedata = animeDic[key];
                return magic_animedata;
            }
            string hash = ImageUtil.HashBitmap(imagefilename, basedir);
            magic_animedata = ImageUtilOAM.FindHash(hash, animeDic);
            if (magic_animedata != null)
            {
                errormessage = "";
                return magic_animedata;
            }

            Bitmap loadbitmap = ImageUtil.OpenBitmap(GetFullPath(imagefilename, basedir),null, out errormessage);
            if (loadbitmap == null)
            {
                return null;
            }

            int width = 480;
            int height = 160;
            if (loadbitmap.Width < width || loadbitmap.Height < height)
            {
                errormessage = R.Error("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", loadbitmap.Width, loadbitmap.Height, width, height, imagefilename);
                loadbitmap.Dispose();
                return null;
            }

            magic_animedata = oam.MakeMagicAnime(imagefilename);
            if (magic_animedata == null)
            {
                errormessage = oam.ErrorMessage;
                loadbitmap.Dispose();
                return null;
            }
            magic_animedata.image_number = image_number++;
            magic_animedata.imageHash = hash;

            animeDic[key] = magic_animedata;
            errormessage = "";
            loadbitmap.Dispose();
            return magic_animedata;
        }
        static ImageUtilOAM.animedata ImportBGImageToData(string imagefilename
            , string basedir
            , Dictionary<string, ImageUtilOAM.animedata> animeDic
            , List<ImageUtilOAM.image_data> bgImagesData
            , bool csaCreatorExpands53Command
            , out string errormessage
            )
        {
            string key = "BG" + imagefilename;

            ImageUtilOAM.animedata magic_animedata;
            if (animeDic.ContainsKey(key))
            {
                errormessage = "";
                magic_animedata = animeDic[key];
                return magic_animedata;
            }

            string hash = ImageUtil.HashBitmap(imagefilename, basedir);
            magic_animedata = ImageUtilOAM.FindHash(hash, animeDic);
            if (magic_animedata != null)
            {
                errormessage = "";
                return magic_animedata;
            }

            magic_animedata = new ImageUtilOAM.animedata();
            string bgfilename = GetFullPath(imagefilename, basedir);
            Bitmap loadbitmap = ImageUtil.OpenBitmap(bgfilename,null, out errormessage);
            if (loadbitmap == null)
            {
                return null;
            }

            int width = BG_SEAT_TILE_WIDTH * 8;
            int height = BG_SEAT_TILE_HEIGHT * 8;
            if (loadbitmap.Width >= 240 && loadbitmap.Height == 160)
            {
                if (csaCreatorExpands53Command)
                {//CSA Creatorだけど拡大表示するので、上半分を切り詰める必要がある
                }
                else
                {//CSA Creator用の画像大きいので、縮小する必要がある.
                    //SCA Creatorの大きい背景の魔法サポート
                    Bitmap temp = ImageUtil.Blank(width, height, loadbitmap);
                    ImageUtil.Scale(temp, 0, 0, width, height, loadbitmap, 0, 0, 240, 160 - 32);
                    loadbitmap = temp;
                }
            }
            else if (loadbitmap.Width < 240 || loadbitmap.Height < 64)
            {
                errormessage = R.Error("画像サイズが正しくありません。\r\n{4}\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", loadbitmap.Width, loadbitmap.Height, width, height, imagefilename);
                loadbitmap.Dispose();
                return null;
            }


            Bitmap savebitmap = ImageUtil.Copy(loadbitmap, 0, 0, width, height);
            //ハッシュ値
            magic_animedata.imageHash = hash;

            //画像
            magic_animedata.image_pointer = (uint)bgImagesData.Count;
            ImageUtilOAM.image_data image_data = new ImageUtilOAM.image_data();
            image_data.data = ImageUtil.ImageToByte16Tile(savebitmap, savebitmap.Width, savebitmap.Height);
            image_data.data = LZ77.compress(image_data.data);
            bgImagesData.Add(image_data);

            //パレット
            magic_animedata.palette_pointer = (uint)bgImagesData.Count;
            ImageUtilOAM.image_data palette_data = new ImageUtilOAM.image_data();
            palette_data.data = ImageUtil.ImageToPalette(savebitmap);
            bgImagesData.Add(palette_data);

            animeDic[key] = magic_animedata;
            errormessage = "";
            loadbitmap.Dispose();
            return magic_animedata;
        }

        //画像の書き込みアドレスが決定したら、画像ポインタをかかないといけないFrameDataを更新します。
        static string updateFrameDataAddress(byte[] frameData, List<ImageUtilOAM.image_data> bgImagesData, List<ImageUtilOAM.image_data> objImagesData)
        {
            uint length = (uint)frameData.Length;
            for (uint n = 0; n < length; n += 4)
            {
                if (frameData[n + 3] != 0x86) //0x86 コマンド以外は skip
                {
                    continue;
                }

                //0  frame16 byte1 x86
                //4  objImagePointer
                //8  OAMAbsoStart
                //12 OAMBGAbsoStart
                //16 bgImagePointer
                //20 objPalettePointer
                //24 bgPalettePointer
                int a = (int)U.u32(frameData, n + 4);
                U.write_p32(frameData, n + 4,  objImagesData[a].write_addr);

                a = (int)U.u32(frameData, n + 16);
                U.write_p32(frameData, n + 16, bgImagesData[a].write_addr);

                a = (int)U.u32(frameData, n + 20);
                U.write_p32(frameData, n + 20, objImagesData[a].write_addr);

                a = (int)U.u32(frameData, n + 24);
                U.write_p32(frameData, n + 24, bgImagesData[a].write_addr);
                n += 4 * 6;
            }
            return "";
        }

    }
}
