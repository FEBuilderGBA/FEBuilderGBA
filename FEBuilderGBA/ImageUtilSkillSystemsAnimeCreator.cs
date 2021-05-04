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
    public class ImageUtilSkillSystemsAnimeCreator
    {
        const int SCREEN_WIDTH  = 240;
        const int SCREEN_HEIGHT = 160;

        enum AnimeType
        {
             None   //攻撃スキルのアニメ
            ,D      //防御スキルのアニメ
        }

        static uint SkipCode(uint anime_address
            ,out string out_ProgramCode
            ,out AnimeType out_AnimeType)
        {
            Debug.Assert(U.isSafetyOffset(anime_address));
            if (Program.ROM.RomInfo.is_multibyte())
            {//FE8Jの場合 先頭にプログラムコードはないよ
                out_ProgramCode = "";
                out_AnimeType = AnimeType.None;
                return anime_address;
            }

            //FE8Uの場合、プログラムが先頭に入る.            
            string[] codeList = new string[] { "skillanimtemplate_2016_11_04.dmp","skillanimtemplate_defender_2017_01_24.dmp"};

            byte[] bin = Program.ROM.getBinaryData(anime_address,0x150);

            //信じられないが、スキルアニメ毎にプログラムコードが設定される.
            for(int i = 0 ; i < codeList.Length; i++)
            {
                string prog = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE8U","skill",codeList[i]);
                if (!File.Exists(prog))
                {
                    continue;
                }
                byte[] target = File.ReadAllBytes(prog);
                if (U.memcmp(bin, target, (IntPtr)target.Length) != 0)
                {
                    continue;
                }
                out_ProgramCode = codeList[i];
                if (out_ProgramCode.IndexOf("defender") >= 0)
                {//防御スキル
                    out_AnimeType = AnimeType.D;
                }
                else
                {//攻撃スキル
                    out_AnimeType = AnimeType.None;
                }
                return anime_address + (uint)target.Length;
            }

            out_ProgramCode = "";
            out_AnimeType = AnimeType.None;
            return U.NOT_FOUND;
        }


        public static Bitmap Draw(uint anime_address, uint showFrameData)
        {

            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return null;
            }

            string programCode;
            AnimeType animeType;
            uint anime_config_address = SkipCode(anime_address, out programCode, out animeType);
            if (anime_config_address == U.NOT_FOUND)
            {//先頭に埋め込まれているプログラムを検出できない
                return null;
            }
            if (anime_config_address + (4 * 5) > Program.ROM.Data.Length)
            {//範囲外
                return null;
            }
            //POIN Frames
            //POIN TSAList
            //POIN GraphicsList
            //POIN PalettesList
            //WORD 0x3d1 //sound id
            uint frames = Program.ROM.p32(anime_config_address + (4 * 0));
            uint tsalist = Program.ROM.p32(anime_config_address + (4 * 1));
            uint graphiclist = Program.ROM.p32(anime_config_address + (4 * 2));
            uint palettelist = Program.ROM.p32(anime_config_address + (4 * 3));
            //uint sound_id = Program.ROM.u32(anime_config_address + (4 * 4));

            if (!U.isSafetyOffset(frames))
            {
                return null;
            }
            if (!U.isSafetyOffset(tsalist))
            {
                return null;
            }
            if (!U.isSafetyOffset(graphiclist))
            {
                return null;
            }
            if (!U.isSafetyOffset(palettelist))
            {
                return null;
            }

            uint frame = FindFrame(showFrameData, frames, Program.ROM.Data);
            if (frame == U.NOT_FOUND)
            {
                return null;
            }
            //フレームを発見したので描画する.
            Bitmap retImage = DrawFrameImage(frame, graphiclist, tsalist, palettelist);
            return retImage;
        }

        static uint FindFrame(uint showFrameData, uint frame, byte[] frameData)
        {
            uint frameI = 0;

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frame + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            for (uint n = frame; n < limitter; n += 4)
            {
                uint image = U.u16(frameData, n + 0);
                //uint wait = U.u16(frameData, n + 2);
                if (image == 0xFFFF)
                {//終端
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

        static Bitmap DrawFrameImage(uint frame, uint graphiclist, uint tsalist, uint palettelist)
        {
            if (!U.isSafetyOffset(frame))
            {
                return ImageUtil.BlankDummy();
            }
            //struct Frames{
            //  ushort id;
            //  ushort wait;
            //}
            uint id = Program.ROM.u16(frame + 0);

            //画像リストなどから、指定ID数、読み飛ばす.
            uint objPointer = graphiclist + (id * 4);
            uint tsaPointer = tsalist + (id * 4);
            uint palPointer = palettelist + (id * 4);

            if (!U.isSafetyOffset(objPointer+4))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(tsaPointer+4))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(palPointer+4))
            {
                return ImageUtil.BlankDummy();
            }
            uint objOffset = Program.ROM.p32(objPointer);
            uint tsaOffset = Program.ROM.p32(tsaPointer);
            uint palOffset = Program.ROM.p32(palPointer);
            if (!U.isSafetyOffset(objOffset))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(tsaOffset))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isSafetyOffset(palOffset + 0x20))
            {
                return ImageUtil.BlankDummy();
            }

            //objとtsaは圧縮されている.
            //palは、0x20(16色1パレット)固定.
            byte[] obj = LZ77.decompress(Program.ROM.Data, objOffset);
            byte[] tsa = LZ77.decompress(Program.ROM.Data, tsaOffset);

            int width = SCREEN_WIDTH;
            int height = ImageUtil.CalcHeightbyTSA(width, tsa.Length);
            if (height < 160)
            {
                height = 160;
            }

            Bitmap retImage = ImageUtil.ByteToImage16Tile(width, height, obj, 0, Program.ROM.Data, (int)palOffset, tsa, 0);
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

            string programCode;
            AnimeType animeType;
            uint anime_config_address = SkipCode(anime_address, out programCode, out animeType);
            if (anime_config_address == U.NOT_FOUND)
            {
                return ;
            }
            if (anime_config_address + (4 * 5) > Program.ROM.Data.Length)
            {//範囲外
                return ;
            }
            //POIN Frames
            //POIN TSAList
            //POIN GraphicsList
            //POIN PalettesList
            //WORD 0x3d1 //sound id
            uint frames = Program.ROM.p32(anime_config_address + (4 * 0));
            uint tsalist = Program.ROM.p32(anime_config_address + (4 * 1));
            uint graphiclist = Program.ROM.p32(anime_config_address + (4 * 2));
            uint palettelist = Program.ROM.p32(anime_config_address + (4 * 3));
            uint sound_id = Program.ROM.u32(anime_config_address + (4 * 4));

            if (!U.isSafetyOffset(frames))
            {
                return ;
            }
            if (!U.isSafetyOffset(tsalist))
            {
                return;
            }
            if (!U.isSafetyOffset(graphiclist))
            {
                return ;
            }
            if (!U.isSafetyOffset(palettelist))
            {
                return ;
            }

            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frames + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            for (uint n = frames; n < limitter; n += 4)
            {
                uint id = Program.ROM.u16(n + 0);
                uint wait = Program.ROM.u16(n + 2);
                if (id == 0xFFFF)
                {
                    break;
                }

                Bitmap bitmap = DrawFrameImage(n, graphiclist, tsalist, palettelist);
                bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, wait));
            }

            //アニメgif生成
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
        }

        public static void Export(
              string filename    //書き込むファイル名
            , uint anime_address)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);


            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return;
            }


            string programCode;
            AnimeType animeType;
            uint anime_config_address = SkipCode(anime_address, out programCode, out animeType);
            if (anime_config_address == U.NOT_FOUND)
            {
                return;
            }
            if (anime_config_address + (4 * 5) > Program.ROM.Data.Length)
            {//範囲外
                return;
            }

            //POIN Frames
            //POIN TSAList
            //POIN GraphicsList
            //POIN PalettesList
            //WORD 0x3d1 //sound id
            uint frames = Program.ROM.p32(anime_config_address + (4 * 0));
            uint tsalist = Program.ROM.p32(anime_config_address + (4 * 1));
            uint graphiclist = Program.ROM.p32(anime_config_address + (4 * 2));
            uint palettelist = Program.ROM.p32(anime_config_address + (4 * 3));
            uint sound_id = Program.ROM.u32(anime_config_address + (4 * 4));

            if (!U.isSafetyOffset(frames))
            {
                return;
            }
            if (!U.isSafetyOffset(tsalist))
            {
                return;
            }
            if (!U.isSafetyOffset(graphiclist))
            {
                return;
            }
            if (!U.isSafetyOffset(palettelist))
            {
                return;
            }

            //同じアニメを何度も出力しないように記録する.
            Dictionary<uint, Bitmap> animeHash = new Dictionary<uint, Bitmap>();
            List<string> lines = new List<string>();

            if (animeType == AnimeType.D)
            {
                lines.Add("D #is defender anim");
            }
            if (sound_id > 0)
            {
                lines.Add("S" + sound_id.ToString("X04") + " #play sound " + SongTableForm.GetSongName(sound_id));
            }

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frames + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            for (uint n = frames; n < limitter; n += 4)
            {
                uint id = Program.ROM.u16(n + 0);
                uint wait = Program.ROM.u16(n + 2);
                if (id == 0xFFFF)
                {
                    break;
                }

                Bitmap bitmap;
                string imagefilename = basename.Replace(" ","_") + "g" + id.ToString("000") + ".png";
                if (animeHash.ContainsKey(id))
                {
                    bitmap = animeHash[id];
                }
                else
                {
                    bitmap = DrawFrameImage(n, graphiclist, tsalist, palettelist);

                    int paletteCount = ImageUtil.GetPalette16Count(bitmap);
                    if (paletteCount < 1)
                    {
                        paletteCount = 1;
                    }

                    //利用していないパレットを消す.
                    ImageUtil.BlackOutUnnecessaryColors(bitmap, paletteCount);

                    U.BitmapSave(bitmap, Path.Combine(basedir, imagefilename));
                    animeHash[id] = bitmap;
                }

                string line = wait.ToString() + " " + imagefilename;
                lines.Add(line);
            }

            //まとめて書き込み
            File.WriteAllLines(filename, lines);
        }

        //拡張領域に定義されているアニメーションかどうか
        public static bool IsExtendsAreaAnime(uint anime_address)
        {
            List<Address> recycle = new List<Address>();
            RecycleOldAnime(ref recycle, "anime", false, anime_address);

            if (recycle.Count <= 0)
            {//不明
                return true;
            }
            for (int i = 0; i < recycle.Count; i++)
            {
                Address a = recycle[i];
                if (U.isExtrendsROMArea(a.Addr))
                {
                    return true;
                }
                if (a.Pointer != U.NOT_FOUND)
                {
                    if (U.isExtrendsROMArea(a.Pointer))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        class anime
        {
            public byte[] tsa;
            public byte[] image;
            public byte[] pal;
            public string filename;
        }


        public static string Import(string filename, uint anime_pointer)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            //同じアニメを何度も入力しないように記録する.
            List<anime> anime_list = new List<anime>();
            List<byte> frames = new List<byte>();
            string[] lines = File.ReadAllLines(filename);

            uint sound_id = 0x3d1;
            AnimeType animeType = AnimeType.None;
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

                line = line.Replace("\t", " ");
                string[] sp = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string command = sp[0];
                if (command.Length <= 0)
                {
                    continue;
                }
                if (command[0] == 'S')
                {
                    sound_id = U.atoh(command.Substring(1));
                    continue;
                }
                if (command[0] == 'D')
                {
                    animeType = AnimeType.D;
                    continue;
                }
                if (!(U.isNumString(command) && sp.Length >= 2))
                {
                    continue;
                }
                if (sp.Length <= 1)
                {
                    Debug.Assert(false);
                    continue;
                }
                uint wait = U.atoi(command);
                string image = sp[1];

                uint id = FindImage(anime_list, image);
                if (id == U.NOT_FOUND)
                {
                    id = (uint)anime_list.Count;

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
                    string error = ImageUtil.ImageToBytePackedTSA(bitmap, SCREEN_WIDTH, SCREEN_HEIGHT,0, out a.image, out a.tsa);
                    if (error != "")
                    {
                        return R.Error("画像をインポートできません。\r\nFile: {0} line:{1}\r\n\r\nエラー内容:\r\n{2}", filename, lineCount, error);
                    }
                    a.image = LZ77.compress(a.image);
                    a.tsa = LZ77.compress(a.tsa);
                    a.pal = ImageUtil.ImageToPalette(bitmap, 1);
                    a.filename = image;
                    anime_list.Add(a);
                }

                U.append_u16(frames, id);
                U.append_u16(frames, wait);

            }
            //term
            U.append_u16(frames, 0xFFFF);
            U.append_u16(frames, 0xFFFF);

            anime_pointer = U.toOffset(anime_pointer);
            if (!U.isSafetyOffset(anime_pointer))
            {
                return R._("アドレスが無効です");
            }
            uint anime_address = Program.ROM.p32(anime_pointer);

            List<Address> recycle = new List<Address>();
            RecycleOldAnime(ref recycle, basename, false, anime_address);
            RecycleAddress ra = new RecycleAddress(recycle);

            Undo.UndoData undodata = Program.Undo.NewUndoData("ImageUtilSkillSystemsAnimeCreator.Import");

            List<byte> image_list = new List<byte>();
            List<byte> tsa_list = new List<byte>();
            List<byte> pal_list = new List<byte>();
            for (int i = 0; i < anime_list.Count; i++)
            {
                uint p;
                p = ra.Write(anime_list[i].image, undodata);
                U.append_u32(image_list, U.toPointer(p));

                p = ra.Write(anime_list[i].tsa, undodata);
                U.append_u32(tsa_list, U.toPointer(p));

                p = ra.Write(anime_list[i].pal, undodata);
                U.append_u32(pal_list, U.toPointer(p));
            }

            List<byte> mainData = new List<byte>();
            if (Program.ROM.RomInfo.is_multibyte())
            {//FE8J
                //FE8Jには、スキルごとにプログラムは存在しない.
            }
            else
            {//FE8U
                //信じられないが、スキルアニメ毎にプログラムコードが設定される.
                if (animeType == AnimeType.D)
                {
                    string prog = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE8U", "skill", "skillanimtemplate_defender_2017_01_24.dmp");
                    mainData.AddRange(File.ReadAllBytes(prog));
                }
                else
                {
                    string prog = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE8U", "skill", "skillanimtemplate_2016_11_04.dmp");
                    mainData.AddRange(File.ReadAllBytes(prog));
                }
            }

            //プログラムの下にデータがある.
            {
                uint p;
                p = ra.Write(frames.ToArray(), undodata);
                U.append_u32(mainData,U.toPointer(p));

                p = ra.Write(tsa_list.ToArray(), undodata);
                U.append_u32(mainData, U.toPointer(p));

                p = ra.Write(image_list.ToArray(), undodata);
                U.append_u32(mainData, U.toPointer(p));

                p = ra.Write(pal_list.ToArray(), undodata);
                U.append_u32(mainData, U.toPointer(p));

                U.append_u32(mainData, sound_id);
            }

            //アニメポインタの書き換え.
            ra.WriteAndWritePointer(anime_pointer, mainData.ToArray(), undodata);

            Program.Undo.Push(undodata);
            return "";
        }
        static uint FindImage(List<anime> anime_list,string image)
        {
            for (int i = 0; i < anime_list.Count; i++)
            {
                if (anime_list[i].filename == image)
                {
                    return (uint)i;
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


            string programCode;
            AnimeType animeType;
            uint anime_config_address = SkipCode(anime_address, out programCode, out animeType);
            if (anime_config_address == U.NOT_FOUND)
            {
                return;
            }
            if (anime_config_address + (4 * 5) >= Program.ROM.Data.Length)
            {//範囲外
                return;
            }

            //POIN Frames
            //POIN TSAList
            //POIN GraphicsList
            //POIN PalettesList
            //WORD 0x3d1 //sound id
            uint frames = Program.ROM.p32(anime_config_address + (4 * 0));
            uint tsalist = Program.ROM.p32(anime_config_address + (4 * 1));
            uint graphiclist = Program.ROM.p32(anime_config_address + (4 * 2));
            uint palettelist = Program.ROM.p32(anime_config_address + (4 * 3));
            //uint sound_id = Program.ROM.u32(anime_config_address + (4 * 4));

            if (!U.isSafetyOffset(frames))
            {
                return;
            }
            if (!U.isSafetyOffset(tsalist))
            {
                return;
            }
            if (!U.isSafetyOffset(graphiclist))
            {
                return;
            }
            if (!U.isSafetyOffset(palettelist))
            {
                return;
            }

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frames + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            uint count = 0;
            uint n;
            for (n = frames; n < limitter; n += 4)
            {
                uint id = Program.ROM.u16(n + 0);
                uint wait = Program.ROM.u16(n + 2);
                if (id == 0xFFFF)
                {
                    break;
                }

                uint objPointer = graphiclist + (id * 4);
                uint tsaPointer = tsalist + (id * 4);
                uint palPointer = palettelist + (id * 4);
                count ++;

                if (!U.isSafetyOffset(objPointer + 4))
                {
                    return ;
                }
                if (!U.isSafetyOffset(tsaPointer + 4))
                {
                    return ;
                }
                if (!U.isSafetyOffset(palPointer + 4))
                {
                    return ;
                }
                uint objOffset = Program.ROM.p32(objPointer);
                uint tsaOffset = Program.ROM.p32(tsaPointer);
                uint palOffset = Program.ROM.p32(palPointer);
                if (!U.isSafetyOffset(objOffset))
                {
                    return ;
                }
                if (!U.isSafetyOffset(tsaOffset))
                {
                    return ;
                }
                if (!U.isSafetyOffset(palOffset + 0x20))
                {
                    return ;
                }
                //OBJ画像をリサイクルリストに突っ込む.
                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , objPointer
                    , basename + "OBJ"
                    , isPointerOnly
                    , Address.DataTypeEnum.LZ77IMG);

                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , tsaPointer
                    , basename + "TSA"
                    , isPointerOnly
                    , Address.DataTypeEnum.LZ77TSA);

                FEBuilderGBA.Address.AddPointer(recycle
                    , palPointer
                    , 0x20   //16色*2バイト=0x20バイト
                    , basename + "PAL"
                    , Address.DataTypeEnum.PAL);
            }
            if (n >= limitter)
            {
                return ;
            }
            FEBuilderGBA.Address.AddAddress(recycle
                , anime_address
                , anime_config_address - anime_address + (4 * 5)
                , U.NOT_FOUND
                , basename + "POINTER"
                , Address.DataTypeEnum.POINTER);

            FEBuilderGBA.Address.AddPointer(recycle
                , anime_config_address + (4 * 0)
                , (count + 1) * 4
                , basename + "ROMANIMEFRAME"
                , Address.DataTypeEnum.ROMANIMEFRAME);

            FEBuilderGBA.Address.AddPointer(recycle
                , anime_config_address + (4 * 1)
                , (count) * 4
                , basename + "TSALIST"
                , Address.DataTypeEnum.POINTER);
            FEBuilderGBA.Address.AddPointer(recycle
                , anime_config_address + (4 * 2)
                , (count) * 4
                , basename + "IMAGELIST"
                , Address.DataTypeEnum.POINTER);
            FEBuilderGBA.Address.AddPointer(recycle
                , anime_config_address + (4 * 3)
                , (count) * 4
                , basename + "PALETEELIST"
                , Address.DataTypeEnum.POINTER);
        }

        //エラーチェック
        public static void MakeCheckError(List<FELint.ErrorSt> errors, uint anime_address, uint skillindex = U.NOT_FOUND)
        {
            anime_address = U.toOffset(anime_address);
            if (!U.isSafetyOffset(anime_address))
            {
                return;
            }

            string basename = "SkillAnime:" + U.To0xHexString(skillindex) + " " + R._("スキルアニメーションが破損しています。\r\n");

            string programCode;
            AnimeType animeType;
            uint anime_config_address = SkipCode(anime_address, out programCode, out animeType);
            if (anime_config_address == U.NOT_FOUND)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("SkipCodeに失敗しました"), skillindex));
                return;
            }
            if (anime_config_address + (4 * 5) >= Program.ROM.Data.Length)
            {//範囲外
                errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("データがROM末尾より先に書かれています。"), skillindex));
                return;
            }

            //POIN Frames
            //POIN TSAList
            //POIN GraphicsList
            //POIN PalettesList
            //WORD 0x3d1 //sound id
            uint frames = Program.ROM.p32(anime_config_address + (4 * 0));
            uint tsalist = Program.ROM.p32(anime_config_address + (4 * 1));
            uint graphiclist = Program.ROM.p32(anime_config_address + (4 * 2));
            uint palettelist = Program.ROM.p32(anime_config_address + (4 * 3));
            //uint sound_id = Program.ROM.u32(anime_config_address + (4 * 4));

            if (!U.isSafetyOffset(frames))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("framesが取得できません。"), skillindex));
                return;
            }
            if (!U.isSafetyOffset(tsalist))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("tsalistが取得できません。"), skillindex));
                return;
            }
            if (!U.isSafetyOffset(graphiclist))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("graphiclistが取得できません。"), skillindex));
                return;
            }
            if (!U.isSafetyOffset(palettelist))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("palettelistが取得できません。"), skillindex));
                return;
            }

            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint limitter = frames + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

            uint count = 0;
            uint n;
            for (n = frames; n < limitter; n += 4)
            {
                uint id = Program.ROM.u16(n + 0);
                uint wait = Program.ROM.u16(n + 2);
                if (id == 0xFFFF)
                {
                    break;
                }

                uint objPointer = graphiclist + (id * 4);
                uint tsaPointer = tsalist + (id * 4);
                uint palPointer = palettelist + (id * 4);
                count++;

                if (!U.isSafetyOffset(objPointer + 4))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("objPointerを取得中にエラーが発生しました。"), skillindex));
                    return;
                }
                if (!U.isSafetyOffset(tsaPointer + 4))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("tsaPointerを取得中にエラーが発生しました。"), skillindex));
                    return;
                }
                if (!U.isSafetyOffset(palPointer + 4))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("palPointerを取得中にエラーが発生しました。"), skillindex));
                    return;
                }
                uint objOffset = Program.ROM.p32(objPointer);
                uint tsaOffset = Program.ROM.p32(tsaPointer);
                uint palOffset = Program.ROM.p32(palPointer);
                if (!U.isSafetyOffset(objOffset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("objOffsetを取得中にエラーが発生しました。"), skillindex));
                    return;
                }
                if (!U.isSafetyOffset(tsaOffset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("tsaOffsetを取得中にエラーが発生しました。"), skillindex));
                    return;
                }
                if (!U.isSafetyOffset(palOffset + 0x20))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("palOffsetを取得中にエラーが発生しました。"), skillindex));
                    return;
                }

                FELint.CheckLZ77(objOffset, errors, FELint.Type.SKILL_CONFIG, anime_config_address, skillindex);
                FELint.CheckLZ77(tsaOffset, errors, FELint.Type.SKILL_CONFIG, anime_config_address, skillindex);
            }
            if (n >= limitter)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CONFIG, anime_config_address, basename + R._("データ終端がありませんでした。"), skillindex));
                return;
            }
        }
    }
}
