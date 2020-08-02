using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageRomAnimeForm : Form
    {
        public ImageRomAnimeForm()
        {
            InitializeComponent();
#if DEBUG
            ImageRomAnimeForm.PreLoadResource();
#endif

            this.AddressList.BeginUpdate();
            this.AddressList.Items.Clear();
            foreach (var pair in g_ROMAnime)
            {
                string name = U.ToHexString(pair.Key) + " " + U.at(pair.Value, 6);
                this.AddressList.Items.Add(name);
            }
            this.AddressList.EndUpdate();
            U.SelectedIndexSafety(this.AddressList, 0, true);
            U.SetIcon(AnimeExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(AnimeImportButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, new string[] {".TXT"} , (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    AnimeImportButton_Click(null, null);
                }
            });
        }
        static Dictionary<uint, string[]> g_ROMAnime;
        public static void PreLoadResource()
        {
            g_ROMAnime = U.LoadTSVResource(U.ConfigDataFilename("romanime_"));
        }

        private void ImageRomAnimeForm_Load(object sender, EventArgs e)
        {

        }
        int ImageWidth;
        string Option;
        uint FramePointer;
        uint TSAPointer;
        uint ImagePointer;
        uint PalettePointer;
        List<uint> TSAList;
        List<uint> ImageList;
        List<uint> PaletteList;

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint id = U.atoh(this.AddressList.Text);
            string[] sp;
            if (!g_ROMAnime.TryGetValue(id, out sp))
            {
                this.ImageWidth = 30;
                this.FramePointer = 0;
                this.TSAPointer = 0;
                this.ImagePointer = 0;
                this.PalettePointer = 0;
                this.TSAList = new List<uint>();
                this.ImageList = new List<uint>();
                this.PaletteList = new List<uint>();
                return;
            }
            this.ImageWidth = (int)U.atoi(U.at(sp, 0));
            this.Option = U.at(sp, 1);
            this.FramePointer = U.atoh(U.at(sp, 2));
            this.TSAPointer = U.atoh(U.at(sp, 3));
            this.ImagePointer = U.atoh(U.at(sp, 4));
            this.PalettePointer = U.atoh(U.at(sp, 5));

            this.TSAList = GetPointerListCount(TSAPointer);
            this.ImageList = GetPointerListCount(ImagePointer);
            this.PaletteList = GetPalettePointerListCount(PalettePointer, this.FramePointer, this.Option);

            X_INFO.Text = DumpInfo();
            this.ShowFrameUpDown.Value = 0;
            Draw(0);
        }

        static List<uint> GetPointerListCount(uint p)
        {
            List<uint> ret = new List<uint>();

            if (!U.isSafetyOffset(p))
            {//PointerLISTではない
                return ret;
            }
            uint a = Program.ROM.p32(p);
            if (!U.isSafetyOffset(a))
            {//PointerLISTではない
                return ret;
            }

            uint length = (uint)Program.ROM.Data.Length - 4;
            for (; a < length; a += 4)
            {
                uint p2 = Program.ROM.u32(a);
                if (!U.isSafetyPointer(p2))
                {//PointerLISTではない
                    break;
                }
                ret.Add(U.toOffset(p2));
            }

            if (ret.Count <= 0)
            {
                ret.Add(U.toOffset(a));
            }
            return ret;
        }

        static List<uint> GetPalettePointerListCount(uint p,uint framePointer,string option)
        {
            List<uint> ret = new List<uint>();

            if (!U.isSafetyOffset(p))
            {//PointerLISTではない
                return ret;
            }
            uint a = Program.ROM.p32(p);
            if (!U.isSafetyOffset(a))
            {//PointerLISTではない
                return ret;
            }

            uint length = (uint)Program.ROM.Data.Length - 4;
            for (; a < length; a += 4)
            {
                uint p2 = Program.ROM.u32(a);
                if (!U.isSafetyPointer(p2))
                {//PointerLISTではない
                    break;
                }
                ret.Add(U.toOffset(p2));
            }

            if (ret.Count <= 0)
            {
                if (option == "COMMONPALETTE")
                {
                    ret.Add(U.toOffset(a));
                }
                else if (framePointer < 0x100)
                {
                    for (uint i = 0; i < framePointer; i++)
                    {
                        ret.Add(U.toOffset(a + (i * (2 * 16))));
                    }
                }
                else
                {
                    ret.Add(U.toOffset(a));
                }
            }
            return ret;
        }

        static uint GetFrameCountLow(uint framePointer)
        {
            if (!U.isSafetyOffset(framePointer))
            {
                return U.NOT_FOUND;
            }
            else
            {
                uint addr = Program.ROM.p32(framePointer);
                if (!U.isSafetyOffset(addr))
                {
                    return U.NOT_FOUND;
                }

                //圧縮されていないデータなので、事故防止のため リミッターをかける.
                uint limitter = addr + 1024 * 1024; //1MBサーチしたらもうあきらめる.
                limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

                uint id = 0;
                uint wait = 0;
                uint i;
                for (i = 0; addr < limitter; i++, addr += 4)
                {
                    id = Program.ROM.u16(addr);
                    wait = Program.ROM.u16(addr + 2);

                    if (id == 0xFFFF)
                    {
                        break;
                    }
                }
                return i;
            }
        }

        public static uint GetPaletteFrameCountLow(byte[] rom, uint frameAddr)
        {
            //圧縮されていないデータなので、事故防止のため リミッターをかける.
            uint addr = frameAddr;
            uint limitter = addr + 1024 * 1024; //1MBサーチしたらもうあきらめる.
            limitter = (uint)Math.Min(limitter, rom.Length);

            uint i;
            for (i = 0; addr < limitter; i++, addr += 2)
            {
                uint data = U.u16(rom,addr);

                if (data >= 0xFFFE)
                {
                    break;
                }
            }
            return i;
        }

        private void ShowFrameUpDown_ValueChanged(object sender, EventArgs e)
        {
            Draw((int)this.ShowFrameUpDown.Value);
        }
        void Draw(int frame)
        {
            if (!U.isSafetyOffset(this.FramePointer))
            {
                DrawDirect(frame);
                return;
            }
            else
            {
                uint addr = Program.ROM.p32(this.FramePointer);
                if (!U.isSafetyOffset(addr))
                {
                    DummyImage();
                    return;
                }

                uint id = 0;
                uint wait = 0;
                for (int i = 0; i < frame+1; i++ , addr += 4)
                {
                    id = Program.ROM.u16(addr);
                    wait = Program.ROM.u16(addr + 2);

                    if (id == 0xFFFF)
                    {
                        DummyImage();
                        return;
                    }
                }
                DrawDirect((int)id);
                return;
            }
        }
        void DrawDirect(int frame)
        {
            if (this.TSAList.Count <= 0)
            {
                DummyImage();
                return;
            }
            uint tsa;
            if (frame >= this.TSAList.Count)
            {
                tsa = this.TSAList[this.TSAList.Count - 1];
            }
            else
            {
                tsa = this.TSAList[frame];
            }

            if (this.ImageList.Count <= 0)
            {
                DummyImage();
                return;
            }
            uint image;
            if (frame >= this.ImageList.Count)
            {
                image = this.ImageList[this.ImageList.Count - 1];
            }
            else
            {
                image = this.ImageList[frame];
            }

            if (this.PaletteList.Count <= 0)
            {
                DummyImage();
                return;
            }
            uint palette;
            if (frame >= this.PaletteList.Count)
            {
                palette = this.PaletteList[this.PaletteList.Count - 1];
            }
            else
            {
                palette = this.PaletteList[frame];
            }

            DrawImage(tsa, image, palette);
        }

        string DumpInfo()
        {
            string line;
            uint addr;
            StringBuilder sb = new StringBuilder();

            if (U.isSafetyOffset(this.FramePointer))
            {
                addr = Program.ROM.p32(this.FramePointer);
            }
            else
            {
                addr = 0;
            }
            line = R._("FramePointer:{0} -> {1}", U.To0xHexString(FramePointer) ,U.ToHexString(addr) );
            sb.AppendLine(line);

            line = R._("ImageWidth:{0}", ImageWidth * 8);
            sb.AppendLine(line);

            addr = Program.ROM.p32(this.TSAPointer);
            line = R._("TSAPointer:{0} -> {1} ({2} count)", U.To0xHexString(TSAPointer), U.To0xHexString(addr), this.TSAList.Count);
            sb.AppendLine(line);

            addr = Program.ROM.p32(this.ImagePointer);
            line = R._("ImagePointer:{0} -> {1} ({2} count)", U.To0xHexString(ImagePointer), U.To0xHexString(addr), this.ImageList.Count);
            sb.AppendLine(line);

            addr = Program.ROM.p32(this.PalettePointer);
            line = R._("PalettePointer:{0} -> {1} ({2} count)", U.To0xHexString(PalettePointer), U.To0xHexString(addr), this.PaletteList.Count);
            sb.AppendLine(line);

            return sb.ToString();
        }

        void DummyImage()
        {
            this.X_ANIME_PIC.Image = null;
            this.DisplayImageWidth = 0;
            this.DisplayImageHeight = 0;
            this.DisplayTSAAddress = 0;
            this.DisplayImageAddress = 0;
            this.DisplayPaletteAddress = 0;
        }
        void DrawImage(uint tsa,uint image,uint palette)
        {
            Bitmap bitmap = DrawImageLow(tsa, image, palette);
            if (bitmap == null)
            {
                DummyImage();
                return;
            }

            this.X_ANIME_PIC.Image = bitmap;

            this.DisplayImageWidth = bitmap.Width;
            this.DisplayImageHeight = bitmap.Height;
            this.DisplayTSAAddress = tsa;
            this.DisplayImageAddress = image;
            this.DisplayPaletteAddress = palette;
        }
        Bitmap DrawImageLow(uint tsa, uint image, uint palette)
        {
            byte[] tsaByte = LZ77.decompress(Program.ROM.Data, tsa);
            byte[] imageByte = LZ77.decompress(Program.ROM.Data, image);

            if (tsaByte.Length <= 0 || imageByte.Length <= 0)
            {
                return null;
            }

            int imageHeight = ImageUtil.CalcHeightbyTSA(this.ImageWidth * 8, tsaByte.Length);
            if (imageHeight > 8 * 16)
            {
                imageHeight = 8 * 16;
            }

            Bitmap bitmap =
                ImageUtil.ByteToImage16Tile(this.ImageWidth * 8, imageHeight, imageByte, 0, Program.ROM.Data, (int)palette, tsaByte, 0, 0);
            return bitmap;
        }

        Bitmap DrawDirectLow(int frame)
        {
            if (this.TSAList.Count <= 0)
            {
                return null;
            }
            uint tsa;
            if (frame >= this.TSAList.Count)
            {
                tsa = this.TSAList[this.TSAList.Count - 1];
            }
            else
            {
                tsa = this.TSAList[frame];
            }

            if (this.ImageList.Count <= 0)
            {
                return null;
            }
            uint image;
            if (frame >= this.ImageList.Count)
            {
                image = this.ImageList[this.ImageList.Count - 1];
            }
            else
            {
                image = this.ImageList[frame];
            }

            if (this.PaletteList.Count <= 0)
            {
                return null;
            }
            uint palette;
            if (frame >= this.PaletteList.Count)
            {
                palette = this.PaletteList[this.PaletteList.Count - 1];
            }
            else
            {
                palette = this.PaletteList[frame];
            }

            return DrawImageLow(tsa, image, palette);
        }

        void MakeRecycleList(ref List<Address> recycle, string basename, bool isPointerOnly)
        {
            if (!U.isSafetyOffset(this.FramePointer))
            {
                for (int i = 0; i < this.FramePointer; i++)
                {
                    if (this.TSAList.Count > 0)
                    {
                        uint tsa;
                        if (i >= this.TSAList.Count)
                        {
                            tsa = this.TSAList[this.TSAList.Count - 1];
                        }
                        else
                        {
                            tsa = this.TSAList[i];
                        }
                        FEBuilderGBA.Address.AddAddress(recycle
                            , tsa
                            , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, tsa)
                            , U.NOT_FOUND
                            , basename + "TSA"
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);
                    }

                    if (this.ImageList.Count > 0)
                    {
                        uint image;
                        if (i >= this.ImageList.Count)
                        {
                            image = this.ImageList[this.ImageList.Count - 1];
                        }
                        else
                        {
                            image = this.ImageList[i];
                        }
                        FEBuilderGBA.Address.AddAddress(recycle
                            , image
                            , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                            , U.NOT_FOUND
                            , basename + "IMAGE"
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);
                    }

                    if (this.PaletteList.Count > 0)
                    {
                        uint palette;
                        if (i >= this.PaletteList.Count)
                        {
                            palette = this.PaletteList[this.PaletteList.Count - 1];
                        }
                        else
                        {
                            palette = this.PaletteList[i];
                        }
                        FEBuilderGBA.Address.AddAddress(recycle
                            , palette
                            , 2 * 16
                            , U.NOT_FOUND
                            , basename + "PAL"
                            , FEBuilderGBA.Address.DataTypeEnum.PAL);
                    }
                }
            }
            else
            {
                //同じアニメを何度も出力しないように記録する.
                Dictionary<uint, bool> animeHash = new Dictionary<uint, bool>();

                uint addr = Program.ROM.p32(FramePointer);
                uint startAddr = addr;

                //圧縮されていないデータなので、事故防止のため リミッターをかける.
                uint limitter = addr + 1024 * 1024; //1MBサーチしたらもうあきらめる.
                limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

                uint id = 0;
                uint wait = 0;
                for (; addr < limitter; addr += 4)
                {
                    id = Program.ROM.u16(addr);
                    wait = Program.ROM.u16(addr + 2);

                    if (id == 0xFFFF)
                    {
                        break;
                    }

                    int i = (int)id;
                    if (this.TSAList.Count > 0)
                    {
                        uint tsa;
                        if (i >= this.TSAList.Count)
                        {
                            tsa = this.TSAList[this.TSAList.Count - 1];
                        }
                        else
                        {
                            tsa = this.TSAList[i];
                        }
                        FEBuilderGBA.Address.AddAddress(recycle
                            , tsa
                            , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, tsa)
                            , U.NOT_FOUND
                            , basename + "TSA"
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);
                    }

                    if (this.ImageList.Count > 0)
                    {
                        uint image;
                        if (i >= this.ImageList.Count)
                        {
                            image = this.ImageList[this.ImageList.Count - 1];
                        }
                        else
                        {
                            image = this.ImageList[i];
                        }
                        FEBuilderGBA.Address.AddAddress(recycle
                            , image
                            , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                            , U.NOT_FOUND
                            , basename + "IMAGE"
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    }

                    if (this.PaletteList.Count > 0)
                    {
                        uint palette;
                        if (i >= this.PaletteList.Count)
                        {
                            palette = this.PaletteList[this.PaletteList.Count - 1];
                        }
                        else
                        {
                            palette = this.PaletteList[i];
                        }
                        FEBuilderGBA.Address.AddAddress(recycle
                            , palette
                            , 2 * 16
                            , U.NOT_FOUND
                            , basename + "PAL"
                            , FEBuilderGBA.Address.DataTypeEnum.PAL);
                    }
                }
                FEBuilderGBA.Address.AddPointer(recycle
                    , FramePointer
                    , addr - startAddr
                    , basename + "FRAME"
                    , FEBuilderGBA.Address.DataTypeEnum.ROMANIMEFRAME);
            }
        }

        int DisplayImageWidth;
        int DisplayImageHeight;
        uint DisplayTSAAddress;
        uint DisplayImageAddress;
        uint DisplayPaletteAddress;
        private void X_JumpGraphicsTool_Click(object sender, EventArgs e)
        {
            if (this.DisplayImageWidth == 0)
            {
                return;
            }

            GraphicsToolForm f = (GraphicsToolForm)InputFormRef.JumpFormLow<GraphicsToolForm>();
            f.Jump(this.DisplayImageWidth
                , this.DisplayImageHeight
                , this.DisplayImageAddress
                , 0
                , this.DisplayTSAAddress
                , 1
                , this.DisplayPaletteAddress
                , 0
                , 0
                , 0);
            f.Show();
        }


        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            foreach (var pair in g_ROMAnime)
            {
                string[] sp = pair.Value;
                int imageWidth = (int)U.atoi(U.at(sp, 0));
                string option = U.at(sp, 1);
                uint framePointer = U.atoh(U.at(sp, 2));
                uint tsaPointer = U.atoh(U.at(sp, 3));
                uint imagePointer = U.atoh(U.at(sp, 4));
                uint palettePointer = U.atoh(U.at(sp, 5));
                string name = U.at(sp, 6);

                uint frameCount = U.NOT_FOUND;
                if (U.isSafetyOffset(framePointer))
                {
                    FEBuilderGBA.Address.AddAddress(list, framePointer
                        , 4
                        , U.NOT_FOUND
                        , name + " FRAME Pointer"
                        , FEBuilderGBA.Address.DataTypeEnum.POINTER);
                    if (!isPointerOnly)
                    {
                        frameCount = GetFrameCountLow(framePointer);
                        if (frameCount != U.NOT_FOUND)
                        {
                            uint p = Program.ROM.p32(framePointer);
                            FEBuilderGBA.Address.AddAddress(list, p
                                , frameCount * 4
                                , framePointer
                                , name + " FRAME"
                                , FEBuilderGBA.Address.DataTypeEnum.BIN);
                        }
                    }
                }

                if (frameCount == U.NOT_FOUND)
                {
                    continue;
                }

                FEBuilderGBA.Address.AddAddress(list, tsaPointer
                    , 4
                    , U.NOT_FOUND
                    , name + " TSA Pointer"
                    , FEBuilderGBA.Address.DataTypeEnum.POINTER);
                if (!isPointerOnly)
                {
                    List<uint> tsaList = GetPointerListCount(tsaPointer);
                    uint baseAddr = Program.ROM.p32(tsaPointer);
                    for (int i = 0; i < tsaList.Count; i++)
                    {
                        uint p = baseAddr + ((uint)i * 4);
                        uint a = tsaList[i];
                        if (Program.ROM.p32(p) != a)
                        {
                            p = U.NOT_FOUND;
                        }
                        FEBuilderGBA.Address.AddAddress(list, a
                            , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, a)
                            , p
                            , name + " TSA"
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);
                    }
                }


                FEBuilderGBA.Address.AddAddress(list, imagePointer
                    , 4
                    , U.NOT_FOUND
                    , name + " Image Pointer"
                    , FEBuilderGBA.Address.DataTypeEnum.POINTER);
                if (!isPointerOnly)
                {
                    List<uint> imageList = GetPointerListCount(imagePointer);
                    uint baseAddr = Program.ROM.p32(imagePointer);
                    for (int i = 0; i < imageList.Count; i++)
                    {
                        uint p = baseAddr + ((uint)i * 4);
                        uint a = imageList[i];
                        if (Program.ROM.p32(p) != a)
                        {
                            p = U.NOT_FOUND;
                        }
                        FEBuilderGBA.Address.AddAddress(list, a
                            , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, a)
                            , p
                            , name + " Image"
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    }
                }

                FEBuilderGBA.Address.AddAddress(list, palettePointer
                    , 4
                    , U.NOT_FOUND
                    , name + " Palette Pointer"
                    , FEBuilderGBA.Address.DataTypeEnum.POINTER);
                if (!isPointerOnly)
                {   
                    List<uint> paletteList = GetPalettePointerListCount(palettePointer, framePointer , option);
                    uint baseAddr = Program.ROM.p32(palettePointer);
                    for (int i = 0; i < paletteList.Count; i++)
                    {
                        uint p = baseAddr + ((uint)i * 4);
                        uint a = paletteList[i];
                        if (Program.ROM.p32(p) != a)
                        {
                            p = U.NOT_FOUND;
                        }
                        FEBuilderGBA.Address.AddAddress(list, a
                            , 2 * 16
                            , p
                            , name + " Palette"
                            , FEBuilderGBA.Address.DataTypeEnum.PAL);
                    }
                }
            }
        }

        void ExportGif(string filename)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();

            if (!U.isSafetyOffset(this.FramePointer))
            {
                for (int i = 0; i < this.FramePointer; i++)
                {
                    Bitmap bitmap;
                    bitmap = DrawDirectLow(i);
                    if (bitmap == null)
                    {//デコードできなかった.
                        bitmap = ImageUtil.Blank(30 * 8, 16 * 8);
                    }

                    //利用していないパレットを消す.
                    ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);

                    bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, 1));
                }
            }
            else
            {
                //同じアニメを何度も出力しないように記録する.
                Dictionary<uint, Bitmap> animeHash = new Dictionary<uint, Bitmap>();

                uint addr = Program.ROM.p32(FramePointer);

                //圧縮されていないデータなので、事故防止のため リミッターをかける.
                uint limitter = addr + 1024 * 1024; //1MBサーチしたらもうあきらめる.
                limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

                uint id = 0;
                uint wait = 0;
                uint i;
                for (i = 0; addr < limitter; i++, addr += 4)
                {
                    id = Program.ROM.u16(addr);
                    wait = Program.ROM.u16(addr + 2);

                    if (id == 0xFFFF)
                    {
                        break;
                    }

                    Bitmap bitmap;
                    string imagefilename = basename.Replace(" ", "_") + "g" + id.ToString("000") + ".png";
                    if (animeHash.ContainsKey(id))
                    {
                        bitmap = animeHash[id];
                    }
                    else
                    {
                        bitmap = DrawDirectLow((int)id);
                        if (bitmap == null)
                        {//デコードできなかった.
                            bitmap = ImageUtil.Blank(30 * 8, 16 * 8);
                        }

                        //利用していないパレットを消す.
                        ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);
                        animeHash[id] = bitmap;
                    }

                    bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, wait));
                }
            }

            //アニメgif生成
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
        }
        void Export(string filename)
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            List<string> lines = new List<string>();

            if (this.TSAList.Count >= 2 && this.PaletteList.Count == 1)
            {
                lines.Add("//" + R._("共通パレットを利用する必要があります。最初の画像のパレットのみが利用され、以後は共通パレットが利用されているものとして処理されます。"));
            }
            if (!U.isSafetyOffset(this.FramePointer))
            {
                if (this.TSAList.Count == 1 && this.PaletteList.Count >= 2)
                {
                    lines.Add("//" + R._("パレットアニメーションである必要があります。最初の画像のみ利用され、以後の画像はパレットだけ利用されます。"));
                }
                lines.Add("//" + R._("このアニメーションは、フレーム数が固定されています。最大フレーム数が、{0}({1})である必要があります", U.To0xHexString(this.FramePointer), this.FramePointer));

                for (int i = 0; i < this.FramePointer; i++)
                {
                    Bitmap bitmap;
                    string imagefilename = basename.Replace(" ", "_") + "g" + i.ToString("000") + ".png";
                    bitmap = DrawDirectLow(i);
                    if (bitmap == null)
                    {//デコードできなかった.
                        bitmap = ImageUtil.Blank(30 * 8, 16 * 8);
                    }

                    //利用していないパレットを消す.
                    ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);

                    U.BitmapSave(bitmap, Path.Combine(basedir, imagefilename) );
                    string line = "1" + " " + imagefilename;
                    lines.Add(line);
                }
            }
            else
            {
                //同じアニメを何度も出力しないように記録する.
                Dictionary<uint, Bitmap> animeHash = new Dictionary<uint, Bitmap>();

                uint addr = Program.ROM.p32(FramePointer);

                //圧縮されていないデータなので、事故防止のため リミッターをかける.
                uint limitter = addr + 1024 * 1024; //1MBサーチしたらもうあきらめる.
                limitter = (uint)Math.Min(limitter, Program.ROM.Data.Length);

                uint id = 0;
                uint wait = 0;
                uint i;
                for (i = 0; addr < limitter; i++, addr += 4)
                {
                    id = Program.ROM.u16(addr);
                    wait = Program.ROM.u16(addr + 2);

                    if (id == 0xFFFF)
                    {
                        break;
                    }

                    Bitmap bitmap;
                    string imagefilename = basename.Replace(" ", "_") + "g" + id.ToString("000") + ".png";
                    if (animeHash.ContainsKey(id))
                    {
                        bitmap = animeHash[id];
                    }
                    else
                    {
                        bitmap = DrawDirectLow((int)id);
                        if (bitmap == null)
                        {//デコードできなかった.
                            bitmap = ImageUtil.Blank(30 * 8, 16 * 8);
                        }

                        //利用していないパレットを消す.
                        ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);

                        U.BitmapSave(bitmap, Path.Combine(basedir, imagefilename));

                        animeHash[id] = bitmap;
                    }

                    string line = wait.ToString() + " " + imagefilename;
                    lines.Add(line);
                }
            }
            U.WriteAllLinesInError(filename, lines);
        }

        class anime
        {
            public byte[] tsa;
            public byte[] image;
            public byte[] pal;
            public string filename;
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


        string Import(
              string filename    //読込むファイル名
            )
        {
            string basename = Path.GetFileNameWithoutExtension(filename) + "_";
            string basedir = Path.GetDirectoryName(filename);

            //同じアニメを何度も入力しないように記録する.
            List<anime> anime_list = new List<anime>();
            int imageWidth = this.ImageWidth * 8;
            int imageHeight = 8*16;
            List<byte> frames = new List<byte>();

            string[] lines = File.ReadAllLines(filename);
            for (uint lineCount = 0; lineCount < lines.Length; lineCount++)
            {
                string line = lines[lineCount++];
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
                if (!(U.isNumString(command) && sp.Length >= 2))
                {
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
                    if (bitmap.Width != imageWidth || bitmap.Height != imageHeight)
                    {
                        bitmap = ImageUtil.Copy(bitmap, 0, 0, imageWidth, imageHeight);
                    }

                    anime a = new anime();
                    string error = ImageUtil.ImageToBytePackedTSA(bitmap, imageWidth, imageHeight, 0, out a.image, out a.tsa);
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
            U.append_u16(frames, 0x0);

            if (anime_list.Count <= 0)
            {
                return R._("書き込むアニメーションがありません");
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData("ImageRomAnimeForm.Import");

            List<Address> recycle = new List<FEBuilderGBA.Address>();
            MakeRecycleList(ref recycle , "", false);

            RecycleAddress ra = new RecycleAddress(recycle);

            uint p;
            if (!U.isSafetyOffset(this.FramePointer))
            {//固定フレームのアニメ
                if (this.FramePointer != anime_list.Count)
                {
                    return R._("このアニメーションは固定アニメーションです。フレームが{0}個必要です。指定されたアニメには{1}のフレームがあり、数が一致していません。", this.FramePointer , anime_list.Count);
                }

                if (this.TSAList.Count == 1 && this.PaletteList.Count >= 2)
                {//パレットアニメ
                    List<byte> pal_list = new List<byte>();
                    for (int i = 0; i < anime_list.Count; i++)
                    {
                        p = ra.Write(anime_list[i].pal, undodata);
                        U.append_u32(pal_list, U.toPointer(p));
                    }

                    ra.WriteAndWritePointer(this.TSAPointer, anime_list[0].tsa, undodata);
                    ra.WriteAndWritePointer(this.ImagePointer, anime_list[0].image, undodata);
                    ra.WriteAndWritePointer(this.PalettePointer, pal_list.ToArray(), undodata);
                }
                else
                {
                    List<byte> image_list = new List<byte>();
                    List<byte> tsa_list = new List<byte>();
                    List<byte> pal_list = new List<byte>();
                    for (int i = 0; i < anime_list.Count; i++)
                    {
                        p = ra.Write(anime_list[i].image, undodata);
                        U.append_u32(image_list, U.toPointer(p));

                        p = ra.Write(anime_list[i].tsa, undodata);
                        U.append_u32(tsa_list, U.toPointer(p));

                        p = ra.Write(anime_list[i].pal, undodata);
                        U.append_u32(pal_list, U.toPointer(p));
                    }

                    ra.WriteAndWritePointer(this.TSAPointer, tsa_list.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.ImagePointer, image_list.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.PalettePointer, pal_list.ToArray(), undodata);
                }
            }
            else
            {//フレームを利用するアニメ
                if (this.TSAList.Count >= 2 && this.PaletteList.Count == 1)
                {//共通パレット
                    List<byte> image_list = new List<byte>();
                    List<byte> tsa_list = new List<byte>();
                    for (int i = 0; i < anime_list.Count; i++)
                    {
                        p = ra.Write(anime_list[i].image, undodata);
                        U.append_u32(image_list, U.toPointer(p));

                        p = ra.Write(anime_list[i].tsa, undodata);
                        U.append_u32(tsa_list, U.toPointer(p));
                    }

                    ra.WriteAndWritePointer(this.FramePointer, frames.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.TSAPointer, tsa_list.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.ImagePointer, image_list.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.PalettePointer, anime_list[0].pal, undodata);
                }
                else
                {//複数パレット
                    List<byte> image_list = new List<byte>();
                    List<byte> tsa_list = new List<byte>();
                    List<byte> pal_list = new List<byte>();
                    for (int i = 0; i < anime_list.Count; i++)
                    {
                        p = ra.Write(anime_list[i].image, undodata);
                        U.append_u32(image_list, U.toPointer(p));

                        p = ra.Write(anime_list[i].tsa, undodata);
                        U.append_u32(tsa_list, U.toPointer(p));

                        p = ra.Write(anime_list[i].pal, undodata);
                        U.append_u32(pal_list, U.toPointer(p));
                    }

                    ra.WriteAndWritePointer(this.FramePointer, frames.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.TSAPointer, tsa_list.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.ImagePointer, image_list.ToArray(), undodata);
                    ra.WriteAndWritePointer(this.PalettePointer, pal_list.ToArray(), undodata);
                }
            }
            ra.BlackOut(undodata);

            Program.Undo.Push(undodata);
            return "";
        }

        private void AnimeExportButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("アニメスクリプト|*.txt|アニメGIF|*.gif|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, "romanime_" + this.AddressList.Text.Trim());

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);

            string ext = U.GetFilenameExt(save.FileName);
            if (ext == ".GIF")
            {
                ExportGif(filename);
            }
            else
            {
                Export(filename);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void AnimeImportButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {
                string title = R._("開くファイル名を選択してください");
                string filter = R._("アニメスクリプト|*.txt|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                open.FileName = "romanime_" + this.AddressList.Text.Trim();
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                if (!U.CanReadFileRetry(open))
                {
                    return;
                }
                filename = open.FileNames[0];
                Program.LastSelectedFilename.Save(this, "", open);
            }

            string error = "";
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                error = Import(filename);
            }
            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }

            U.ReSelectList(this.AddressList);
        }
    }
}
