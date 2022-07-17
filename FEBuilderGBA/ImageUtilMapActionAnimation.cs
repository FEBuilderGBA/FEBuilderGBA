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
    public class ImageUtilMapActionAnimation
    {
        const int SCREEN_WIDTH  = 64;
        const int SCREEN_HEIGHT = 64;

        public static Bitmap Draw(uint anime_address, uint showFrameData, out string log)
        {
            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                log = "";
                return ImageUtil.BlankDummy();
            }

            uint frame = FindFrame(showFrameData, anime_address, Program.ROM.Data);
            if (frame == U.NOT_FOUND)
            {
                log = "";
                return ImageUtil.BlankDummy();
            }
            //フレームを発見したので描画する.
            Bitmap retImage = DrawFrameImage(frame, out log);
            return retImage;
        }

        static uint FindFrame(uint showFrameData
            , uint anime_address
            , byte[] frameData)
        {
            uint frameI = 0;

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = anime_address + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            for (uint n = anime_address; n < limitter; n += 12)
            {
                uint term1 = U.u32(frameData, n);
                uint term2 = U.u32(frameData, n + 4);
                if (term1 == 0 && term2 == 0)
                {
                    break;
                }

                if (showFrameData == frameI)
                {
                    return n;
                }

                frameI++;
            }
            return U.NOT_FOUND;
        }

        static Bitmap DrawFrameImage(uint frame, out string log)
        {
            if (!U.isSafetyOffset(frame))
            {
                log = "";
                return ImageUtil.BlankDummy();
            }
            //struct Frames{
            //  byte   wait;
            //  byte   00;
            //  ushort sound;
            //  void*  img;
            //  void*  pal;
            //}//sizeof()==12

            uint objOffset = Program.ROM.p32(frame + 4);
            uint palOffset = Program.ROM.p32(frame + 8);

            if (!U.isSafetyOffset(objOffset))
            {
                log = R._("BAD OBJ_OFFSET {0}", U.To0xHexString(objOffset));
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(palOffset))
            {
                log = R._("BAD PAL_OFFSET {0}", U.To0xHexString(palOffset));
                return ImageUtil.BlankDummy();
            }
            log = R._("IMG {0}, PAL {1}", U.To0xHexString(objOffset), U.To0xHexString(palOffset));

            //objは圧縮されている.
            //palは、0x20(16色1パレット)固定.
            byte[] obj = LZ77.decompress(Program.ROM.Data, objOffset);

            Bitmap retImage = ImageUtil.ByteToImage16Tile(SCREEN_WIDTH, SCREEN_HEIGHT, obj, 0, Program.ROM.Data, (int)palOffset);
            return retImage;
        }

        public static void ExportGif(
              string filename    //書き込むファイル名
            , uint anime_address)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return ;
            }

            //struct Frames{
            //  byte   wait;
            //  byte   00;
            //  ushort sound;
            //  void*  img;
            //  void*  pal;
            //}//sizeof()==12

            //objは圧縮されている.
            //palは、0x20(16色1パレット)固定.

            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = anime_address + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            for (uint n = anime_address; n < limitter; n += 12)
            {
                uint wait = Program.ROM.u8(n + 0);
                uint term1 = Program.ROM.u32(n);
                uint term2 = Program.ROM.u32(n + 4);
                if (term1 == 0 && term2 == 0)
                {
                    break;
                }

                string log;
                Bitmap bitmap = DrawFrameImage(n, out log);
                bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, wait));
            }

            //アニメgif生成
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
        }

        public static void Export(
              string filename    //書き込むファイル名
            , uint anime_address
            , string name)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return;
            }

            List<string> lines = new List<string>();
            if (name != "")
            {
                lines.Add("//NAME=" + name);
            }

            //同じアニメを何度も出力しないように記録する.
            Dictionary<uint, Bitmap> animeHash = new Dictionary<uint, Bitmap>();

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = anime_address + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);
            uint id = 0; //IDの概念がないので、0から適当に振ります

            for (uint n = anime_address; n < limitter; n += 12)
            {
                uint wait = Program.ROM.u8(n + 0);
                uint term1 = Program.ROM.u32(n);
                uint imgOffset = Program.ROM.p32(n + 4);
                uint palOffset = Program.ROM.p32(n + 8);
                if (term1 == 0 && imgOffset == 0)
                {
                    break;
                }

                uint hash = imgOffset + palOffset;

                string imagefilename;
                Bitmap bitmap;
                if (animeHash.ContainsKey(hash))
                {
                    bitmap = animeHash[hash];
                    imagefilename = MakeFilename(basename, bitmap);
                }
                else
                {
                    string log;
                    bitmap = DrawFrameImage(n, out log);
                    bitmap.Tag = id;
                    id++;

                    int paletteCount = ImageUtil.GetPalette16Count(bitmap);
                    if (paletteCount < 1)
                    {
                        paletteCount = 1;
                    }

                    //利用していないパレットを消す.
                    ImageUtil.BlackOutUnnecessaryColors(bitmap, paletteCount);

                    imagefilename = MakeFilename(basename, bitmap);
                    U.BitmapSave(bitmap, Path.Combine(basedir, imagefilename));

                    animeHash[hash] = bitmap;
                }
                string line = wait.ToString() + "\t" + imagefilename;

                uint sound = Program.ROM.u16(n + 2);
                if (sound != 0)
                {
                    line += "\t" + U.To0xHexString(sound);
                }
                lines.Add(line);
            }

            //まとめて書き込み
            File.WriteAllLines(filename, lines);
        }

        static string MakeFilename(string basename, Bitmap bitmap)
        {
            string imagefilename = basename.Replace(" ", "_") + "g" + ((uint)bitmap.Tag).ToString("000") + ".png";
            return imagefilename;
        }

        class anime
        {
            public byte[] image;
            public byte[] pal;
            public uint image_addr;
            public uint pal_addr;
        };

        public static string Import(string filename, uint anime_pointer)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            //他のアニメーションとはデータ構造が違うので注意
            List<anime> poolList = new List<anime>();
            List<byte> frames = new List<byte>();
            string[] lines = File.ReadAllLines(filename);

            for (uint lineCount = 0; lineCount < lines.Length; lineCount++)
            {
                string line = lines[lineCount];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                if (line.Length <= 0)
                {
                    continue;
                }
                InputFormRef.DoEvents(null, "Line:" + lineCount);

                string[] sp = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (sp.Length <= 1)
                {
                    sp = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                }
                if (sp.Length <= 1)
                {
                    continue;
                }

                string command = sp[0];
                uint wait = U.atoi0x(command);
                string image = sp[1];
                uint song = 0;
                if (sp.Length >= 2+1)
                {
                    song = U.atoi0x(sp[2]);
                }

                string fullfilename = Path.Combine(basedir, image);
                Bitmap bitmap = ImageUtil.OpenBitmap(fullfilename);
                if (bitmap == null)
                {
                    return R.Error("ファイル名が見つかりませんでした。\r\nFile: {0} line:{1}\r\n\r\nエラー内容:\r\n{2}", filename, lineCount, line);
                }
                if (bitmap.Width != SCREEN_WIDTH || bitmap.Height != SCREEN_HEIGHT)
                {
                    bitmap = ImageUtil.Copy(bitmap, 0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
                }

                anime a = new anime();
                a.image = ImageUtil.ImageToByte16Tile(bitmap, SCREEN_WIDTH, SCREEN_HEIGHT);
                a.image = LZ77.compress(a.image);

                a.pal = ImageUtil.ImageToPalette(bitmap, 1);
                poolList.Add(a);

                U.append_u16(frames, wait);
                U.append_u16(frames, song);
                U.append_u32(frames, 0x0);
                U.append_u32(frames, 0x0);
            }
            //term
            U.append_u32(frames, 0x0);
            U.append_u32(frames, 0x0);
            U.append_u32(frames, 0x0);

            byte[] mainData = frames.ToArray();

            anime_pointer = U.toOffset(anime_pointer);
            if (!U.isSafetyOffset(anime_pointer))
            {
                return R._("アドレスが無効です");
            }
            uint anime_address = Program.ROM.p32(anime_pointer);

            List<Address> recycle = new List<Address>();
            RecycleOldAnime(ref recycle, basename, false, anime_address);
            RecycleAddress ra = new RecycleAddress(recycle);

            Undo.UndoData undodata = Program.Undo.NewUndoData("ImageUtilMapActionAnimation.Import");

            uint n = 0;
            for (int i = 0; i < poolList.Count; i++, n += 12)
            {
                uint image_addr = CheckDupImage(poolList, i);
                if (image_addr == U.NOT_FOUND)
                {
                    image_addr = ra.Write(poolList[i].image, undodata);
                    poolList[i].image_addr = image_addr;
                }
                uint pal_addr = CheckDupPal(poolList, i);
                if (pal_addr == U.NOT_FOUND)
                {
                    pal_addr = ra.Write(poolList[i].pal, undodata);
                    poolList[i].pal_addr = pal_addr;
                }
                U.write_p32(mainData, n + 4, image_addr);
                U.write_p32(mainData, n + 8, pal_addr);
            }

            //アニメポインタの書き換え.
            ra.WriteAndWritePointer(anime_pointer, mainData, undodata);
            //名前の設定
            MakeImportDataName(anime_pointer, filename, lines);

            Program.Undo.Push(undodata);
            return "";
        }
        static void MakeImportDataName(uint anime_pointer, string filename, string[] lines)
        {
            string name = GetImportDataName(filename, lines);
            Program.CommentCache.Update(anime_pointer, name);
        }

        static string GetImportDataName(string filename, string[] lines)
        {
            string need = "//NAME=";
            foreach (string l in lines)
            {
                int found = l.IndexOf(need);
                if (found < 0)
                {
                    continue;
                }
                string name = U.substr(l, found + need.Length);
                return name;
            }

            {
                string name = RegexCache.MatchSimple(filename, "MapActionAnimation_[^ ]+(.+)\\.");
                return name;
            }
        }

        static uint CheckDupImage(List<anime> poolList, int now)
        {
            byte[] current_image = poolList[now].image;
            for (int i = 0; i < now; i++)
            {
                if (U.memcmp(current_image , poolList[i].image) == 0)
                {
                    return poolList[i].image_addr;
                }
            }
            return U.NOT_FOUND;
        }
        static uint CheckDupPal(List<anime> poolList, int now)
        {
            byte[] current_pal = poolList[now].pal;
            for (int i = 0; i < now; i++)
            {
                if (U.memcmp(current_pal, poolList[i].pal) == 0)
                {
                    return poolList[i].pal_addr;
                }
            }
            return U.NOT_FOUND;
        }

        //上書きされるアニメデータ領域を使いまわす
        public static void RecycleOldAnime(ref List<Address> recycle,string basename,bool isPointerOnly, uint anime_address)
        {
            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return;
            }

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = anime_address + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            uint n = anime_address;
            for ( ; n < limitter; n += 12)
            {
                uint term1 = Program.ROM.u32(n);
                uint imgOffset = Program.ROM.p32(n + 4);
                if (term1 == 0 && imgOffset == 0)
                {
                    break;
                }

                //OBJ画像をリサイクルリストに突っ込む.
                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , n + 4
                    , basename + "OBJ"
                    , isPointerOnly
                    , Address.DataTypeEnum.LZ77IMG);

                FEBuilderGBA.Address.AddPointer(recycle
                    , n + 8
                    , 0x20   //16色*2バイト=0x20バイト
                    , basename + "PAL"
                    , Address.DataTypeEnum.PAL);
            }
            if (n >= limitter)
            {
                return ;
            }

            InputFormRef ifr = Init(null);
            ifr.ReInit(anime_address, (n - anime_address) / 12 );
            FEBuilderGBA.Address.AddAddress(recycle
                , ifr
                , basename
                , new uint[]{ 4, 8}
                );
        }

        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 12
                , (int i, uint addr) =>
                {
                    uint term1 = Program.ROM.u32(addr);
                    uint term2 = Program.ROM.u32(addr + 4);
                    return !(term1 == 0 && term2 == 0);
                }
                , (int i, uint addr) =>
                {
                    return i.ToString();
                }
                );
        }

        //エラーチェック
        public static void MakeCheckError(List<FELint.ErrorSt> errors, uint anime_address, uint index = U.NOT_FOUND)
        {
            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return;
            }

            string basename = "MapActionAnimation:" + U.To0xHexString(index) + " " + R._("行動アニメーションが破損しています。\r\n");

            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return;
            }

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = anime_address + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            uint n = anime_address;
            for (; n < limitter; n += 12)
            {
                uint term1 = Program.ROM.u32(n);
                uint imgOffset = Program.ROM.p32(n + 4);
                uint palOffset = Program.ROM.p32(n + 8);
                if (term1 == 0 && imgOffset == 0)
                {
                    break;
                }

                FELint.CheckLZ77(imgOffset, errors, FELint.Type.MAP_ACTION_ANIMATION, anime_address, index);
            }

            if (n >= limitter)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAP_ACTION_ANIMATION, anime_address, basename + R._("データ終端がありませんでした。"), index));
                return;
            }
        }
    }
}
