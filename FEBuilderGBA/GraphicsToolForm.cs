using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class GraphicsToolForm : Form
    {
        public GraphicsToolForm()
        {
            InitializeComponent();

            this.ImageOption.SelectedIndex = 0;
            this.TSAOption.SelectedIndex = 0;
            this.PaletteOption.SelectedIndex = 0;
            this.KeepTSAComboBox.SelectedIndex = 0;

            this.FoundImages = FindImage();
            if (FoundImages.Count > 0)
            {
                ZImageNo.Maximum = FoundImages.Count - 1;
            }
            U.ForceUpdate(ZImageNo, 0);
            U.ForceUpdate(ZoomComboBox, 1);

        }

        public class FoundImage
        {
            public uint pointer { get; private set; }
            public uint addr { get; private set; }
            public uint size { get; private set; }

            public FoundImage(uint pointer,uint addr , uint size)
            {
                this.pointer = pointer;
                this.addr = addr;
                this.size = size;
            }
        };
        List<FoundImage> FoundImages;

        private void GraphicsToolForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
        }
        static bool IsAlrealyFoundImage(List<FoundImage> list,uint addr)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i].addr == addr)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<FoundImage> FindImage()
        {
            //誤爆すると面倒なことになるフレームとOAMのデータ群
            Dictionary<uint,bool> ignoreDic = new Dictionary<uint,bool>();
            ImageBattleAnimeForm.MakeBattleFrameAndOAMDictionary(ignoreDic);
            SoundFootStepsForm.MakeIgnoreDictionary(ignoreDic);

            List<FoundImage> list = new List<FoundImage>();
            uint length = (uint)Program.ROM.Data.Length - 4;
            for (uint addr = 0x100; addr < length; addr += 4)
            {
                uint a = (uint)Program.ROM.Data[addr + 3];
                if (a != 0x08 && a != 0x09)
                {//ポインタ以外無視する.
                    continue;
                }

                a = Program.ROM.p32(addr);
                if (!U.isSafetyOffset(a))
                {//危険なポインタは無視
                    continue;
                }

                if (a < Program.ROM.RomInfo.compress_image_borderline_address)
                {
                    continue;
                }

                if (ignoreDic.ContainsKey(a))
                {//戦闘アニメのフレーム,OAM等のlz77で圧縮されているデータ
                    continue;
                }

                //ポインタ先は圧縮されているか？
                uint imageDataSize = LZ77.getUncompressSize(Program.ROM.Data, a);
                if (IsBadImageSize(imageDataSize))
                {
                    continue;
                }

                //ポインタは連続してあらわれるのでそのチェックをする.
                if (!IsContinuousPointer(addr, length))
                {
                    continue;
                }

                //ポインタ先をすでに知っている場合は無視.
                if (IsAlrealyFoundImage(list,a))
                {
                    continue;
                }

                //解凍して中身を見てみる.
                byte[] image = LZ77.decompress(Program.ROM.Data, a);
                if (image.Length != imageDataSize)
                {//解凍したらデータ容量が違う
                    continue;
                }
//                if (IsTSAData(image, 0, imageDataSize))
//                {
//                    continue;
//                }
//                if (IsHeaderTSAData(image, 0, imageDataSize))
//                {
//                    continue;
//                }
                //たぶん画像だと判断する.
                list.Add(new FoundImage(addr, a, imageDataSize));
            }
            return list;
        }

        uint[] Pointers(uint a)
        {
            List<uint> pointers = new List<uint>();
            int i;
            for (i = 0; i < 10; i++)
            {
                a = U.toOffset(a);
                if (!U.isSafetyOffset(a))
                {
                    break;
                }
                pointers.Add(a);

                a = Program.ROM.u32(a);
                if (!U.isPointerOrNULL(a))
                {
                    break;
                }
            }
            return pointers.ToArray();
        }


        bool IsPaletteMap(byte[] data,uint offset)
        {
            if (data.Length <= offset+100)
            {//小さすぎる
                return false;
            }
            for (uint n = 0; n < 100; n+=2)
            {
                if (U.u8(data, offset+n) > 0x33)
                {//パレットマップは 0-3 までのパレットの切り替えなので、
                 //それ以上のデータがくることはありえない
                    return false;
                }
            }
            return true;
        }
        static bool IsTSAData(byte[] data, uint offset, uint imageDataSize)
        {
            if (data.Length <= offset+100)
            {//小さすぎる
                return false;
            }

            //画像を8x8に分割した場合のサイズを求めます. (16色なので8/2)
            uint tilenumberMax = imageDataSize / (8 / 2) / 8;

            for (uint n = 0; n < 100; n+=2)
            {
                uint tilenumber = U.u16(data, offset+n) & 0x3FF;
                if (tilenumber > tilenumberMax)
                {//TSAタイルとしては、大きすぎる
                    return false;
                }
            }
            return true;
        }
        static bool IsHeaderTSAData(byte[] data, uint offset, uint imageDataSize)
        {
            if (data.Length <= offset + 2 + (29 * 19 * 2))
            {//小さすぎる
                return false;
            }
            uint width = U.u8(data, offset+0);
            uint height = U.u8(data, offset+1);
            if (width != 29)
            {
                return false;
            }
            if (height != 19)
            {
                return false;
            }

            //画像を8x8に分割した場合のサイズを求めます. (16色なので8/2)
            uint tilenumberMax = imageDataSize / (8 / 2) / 8;
            //常に右端に余白が+2(2*8)あります
            tilenumberMax += 19*2*2; 

            for (uint n = 2; n < 2+(29 * 19 * 2); n += 2)
            {
                uint tilenumber = U.u16(data, offset+n) & 0x3FF;
                if (tilenumber > tilenumberMax)
                {//TSAタイルとしては、大きすぎる
                    return false;
                }
            }
            return true;
        }
        //パレットかどうかの判別はできない.
        //ただ、あきらかに違う すべて 0x00 で真っ黒みたいなものを排除することはできる.
        static bool IsPalette(byte[] data, uint offset)
        {
            if (data.Length <= offset + 0x20)
            {//小さすぎる
                return false;
            }
            int blackPaletteCount = 0;
            int whitePaletteCount = 0;

            for (uint n = 0; n < 0x20; n+=2)
            {
                //paletteの 0x800 は、正常なテータでも1が書き込まれているケースがあるので、見てはいけない.
                uint pal = U.u16(data, offset + n) & 0x7FF;
                byte r = (byte)(pal & 0x1F);
                byte g = (byte)((pal >> 5) & 0x1F);
                byte b = (byte)((pal >> 10) & 0x1F);

                if (r <= 0x3 && g <= 0x3 && b <= 0x3)
                {
                    blackPaletteCount++;
                }
                else if (r >= 0x1A && g >= 0x1A && b >= 0x1A)
                {
                    whitePaletteCount++;
                }
            }

            if (blackPaletteCount >= 0x8)
            {//真っ黒
                return false;
            }
            if (whitePaletteCount >= 0x8)
            {//真っ白
                return false;
            }
            return true;
        }

        static uint ToPaletteOffset(uint v, uint palette_number)
        {
            return U.toOffset(v) + palette_number * 0x20;
        }

        private void Image_ValueChanged(object sender, EventArgs e)
        {
            if (ImageOption.SelectedIndex == 1)
            {//無圧縮の場合、高さの計算ができないので無視
            }
            else
            {
                uint addr = (uint)Image.Value;

                //幅と高さの推測
                Size size = U.CalcLZ77ImageToSize(addr);
                size.Width /= 8;
                size.Height /= 8;

                //高さ自動計算
                if (size.Height >= 32)
                {
                    size.Height = 32;
                }
                U.ForceUpdate(this.PicWidth, size.Width);
                U.ForceUpdate(this.PicHeight, size.Height);

                //TSAとパレットレコメンド
                MakeTSAAndPaletteRecomend(addr);
            }

            Draw();
        }

        Bitmap DrawBimap;
        private void Draw()
        {
            byte[] image;
            int image_pos;
            if (ImageOption.SelectedIndex == 0 || ImageOption.SelectedIndex == 3 || ImageOption.SelectedIndex == 4)
            {//圧縮画像
                image = LZ77.decompress(Program.ROM.Data
                    , U.toOffset(Image.Value) );
                image_pos = 0;
                if (image.Length <= 2)
                {
                    X_BG_PIC.Image = null;
                    this.USE_PALETTE_NUMBER.Text = "1";
                    this.DrawBimap = null;
                    return;
                }
            }
            else if (ImageOption.SelectedIndex == 2)
            {//第2圧縮画像
                image = LZ77.decompress(Program.ROM.Data, U.toOffset(Image.Value));
                byte[] image2 = LZ77.decompress(Program.ROM.Data, U.toOffset(Image2.Value));
                image_pos = 0;
                if (image.Length <= 2)
                {
                    X_BG_PIC.Image = null;
                    this.USE_PALETTE_NUMBER.Text = "1";
                    this.DrawBimap = null;
                    return;
                }
                if (image2.Length > 2)
                {
                    List<byte> imageUZList = new List<byte>();
                    imageUZList.AddRange(image);
                    imageUZList.AddRange(image2);

                    image = imageUZList.ToArray();
                }
            }
            else
            {//無圧縮画像
                image = Program.ROM.Data;
                image_pos = (int)U.toOffset(Image.Value);
            }
            byte[] palette;
            if (PaletteOption.SelectedIndex == 1)
            {//lz77 palette
                palette = LZ77.decompress(Program.ROM.Data, U.toOffset(PALETTE.Value));
                uint palette_offset = ToPaletteOffset(0, (uint)PALETTENO.Value);
                palette = U.subrange(palette, palette_offset, palette_offset + (0x20 * 16));
            }
            else
            {//通常のパレット
                uint palette_addr = ToPaletteOffset((uint)PALETTE.Value, (uint)PALETTENO.Value);
                palette = Program.ROM.getBinaryData(palette_addr , 0x20 * 16);
            }

            byte[] tsa;
            int tsa_pos;
            if (TSAOption.SelectedIndex == 0)
            {//TSAを利用しない
                tsa = null;
                tsa_pos = 0;

                Bitmap bitmap;
                if (ImageOption.SelectedIndex == 3)
                {///256
                    bitmap = ImageUtil.ByteToImage256Tile((int)PicWidth.Value * 8
                        , (int)PicHeight.Value * 8
                        , image
                        , image_pos
                        , palette
                        , 0
                        );
                    this.USE_PALETTE_NUMBER.Text = "16";
                }
                else if (ImageOption.SelectedIndex == 4)
                {///224
                    bitmap = ImageUtil.ByteToImage224BGTile((int)PicWidth.Value * 8
                        , (int)PicHeight.Value * 8
                        , image
                        , image_pos
                        , palette
                        , 0
                        );
                    this.USE_PALETTE_NUMBER.Text = "16";
                }
                else
                {
                    bitmap = ImageUtil.ByteToImage16Tile((int)PicWidth.Value * 8
                        , (int)PicHeight.Value * 8
                        , image
                        , image_pos
                        , palette
                        , 0
                        , 0
                        );
                    this.USE_PALETTE_NUMBER.Text = ImageUtil.GetPalette16Count(bitmap).ToString();
                }
                X_BG_PIC.Image = U.Zoom(bitmap,ZoomComboBox.SelectedIndex);
                this.DrawBimap = bitmap;
            }
            else if (TSAOption.SelectedIndex == 1)
            {//圧縮TSAを利用する
                tsa = LZ77.decompress(Program.ROM.Data, U.toOffset(TSA.Value));
                tsa_pos = 0;

                if (tsa.Length <= 2)
                {
                    X_BG_PIC.Image = ImageUtil.BlankDummy();
                    return;
                }
                Bitmap bitmap;
                if (ImageOption.SelectedIndex == 3)
                {///256
                    bitmap = ImageUtil.ByteToImage256Tile((int)PicWidth.Value * 8
                        , (int)PicHeight.Value * 8
                        , image
                        , image_pos
                        , palette
                        , 0
                        , tsa
                        , tsa_pos
                        );
                    this.USE_PALETTE_NUMBER.Text = "16";
                }
                else
                {
                    bitmap = ImageUtil.ByteToImage16Tile((int)PicWidth.Value * 8
                        , (int)PicHeight.Value * 8
                        , image
                        , image_pos
                        , palette
                        , 0
                        , tsa
                        , tsa_pos
                        , 0
                        );
                    this.USE_PALETTE_NUMBER.Text = ImageUtil.GetPalette16CountForTSA(tsa, (uint)tsa_pos, (uint)(PicWidth.Value * PicHeight.Value)).ToString();
                }
                X_BG_PIC.Image = U.Zoom(bitmap,ZoomComboBox.SelectedIndex);
                this.DrawBimap = bitmap;
            }
            else if (TSAOption.SelectedIndex == 2)
            {//圧縮ヘッダ付きTSAを利用する
                tsa = LZ77.decompress(Program.ROM.Data, U.toOffset(TSA.Value));
                tsa_pos = 0;
                if (tsa.Length <= 2)
                {
                    X_BG_PIC.Image = ImageUtil.BlankDummy();
                    return;
                }

                Bitmap bitmap;
                if (ImageOption.SelectedIndex == 3)
                {///256
                    bitmap = ImageUtil.ByteToImage256TileHeaderTSA((int)PicWidth.Value * 8, (int)PicHeight.Value * 8, image, image_pos, palette, 0, tsa, tsa_pos);
                    this.USE_PALETTE_NUMBER.Text = "16";
                }
                else
                {
                    bitmap = ImageUtil.ByteToImage16TileHeaderTSA((int)PicWidth.Value * 8, (int)PicHeight.Value * 8, image, image_pos, palette, 0, tsa, tsa_pos);
                    this.USE_PALETTE_NUMBER.Text = ImageUtil.GetPalette16CountForTSA(tsa, (uint)tsa_pos + 2, (uint)(PicWidth.Value * PicHeight.Value)).ToString();
                }
                X_BG_PIC.Image = U.Zoom(bitmap,ZoomComboBox.SelectedIndex);
                this.DrawBimap = bitmap;
            }
            else if (TSAOption.SelectedIndex == 3)
            {//無圧縮ヘッダ付きTSAを利用する
                tsa = Program.ROM.Data;
                tsa_pos = (int)U.toOffset(TSA.Value);

                Bitmap bitmap;
                if (ImageOption.SelectedIndex == 3)
                {///256
                    bitmap = ImageUtil.ByteToImage256TileHeaderTSA((int)PicWidth.Value * 8, (int)PicHeight.Value * 8, image, image_pos, palette, 0, tsa, tsa_pos);
                    this.USE_PALETTE_NUMBER.Text = "16";
                }
                else
                {
                    bitmap = ImageUtil.ByteToImage16TileHeaderTSA((int)PicWidth.Value * 8, (int)PicHeight.Value * 8, image, image_pos, palette, 0, tsa, tsa_pos);
                    this.USE_PALETTE_NUMBER.Text = ImageUtil.GetPalette16CountForTSA(tsa, (uint)tsa_pos + 2, (uint)(PicWidth.Value * PicHeight.Value)).ToString();
                }
                X_BG_PIC.Image = U.Zoom(bitmap,ZoomComboBox.SelectedIndex);
                this.DrawBimap = bitmap;
            }
            else if (TSAOption.SelectedIndex == 4)
            {//無圧縮TSAを利用する
                tsa = Program.ROM.Data;
                tsa_pos = (int)U.toOffset(TSA.Value);

                Bitmap bitmap;
                if (ImageOption.SelectedIndex == 3)
                {///256
                    bitmap = ImageUtil.ByteToImage256Tile((int)PicWidth.Value * 8, (int)PicHeight.Value * 8, image, image_pos, palette, 0, tsa, tsa_pos);
                    this.USE_PALETTE_NUMBER.Text = "16";
                }
                else
                {
                    bitmap = ImageUtil.ByteToImage16Tile((int)PicWidth.Value * 8, (int)PicHeight.Value * 8, image, image_pos, palette, 0, tsa, tsa_pos);
                    this.USE_PALETTE_NUMBER.Text = ImageUtil.GetPalette16CountForTSA(tsa, (uint)tsa_pos, (uint)(PicWidth.Value * PicHeight.Value)).ToString();
                }
                X_BG_PIC.Image = U.Zoom(bitmap, ZoomComboBox.SelectedIndex);
                this.DrawBimap = bitmap;
            }
            else if (TSAOption.SelectedIndex == 5)
            {//パレットマップとして解釈する
                tsa = Program.ROM.Data;
                tsa_pos = (int)U.toOffset(TSA.Value);

                Bitmap bitmap = ImageUtil.ByteToImage16TilePaletteMap((int)PicWidth.Value * 8
                    , (int)PicHeight.Value * 8
                    , image
                    , image_pos
                    , palette
                    , 0
                    , tsa
                    , tsa_pos
                    );
                X_BG_PIC.Image = U.Zoom(bitmap,ZoomComboBox.SelectedIndex);
                this.USE_PALETTE_NUMBER.Text = "4";
                this.DrawBimap = bitmap;
            }
            else
            {
                this.DrawBimap = null;
            }

            bool tsaeditorVisible = false;
            if (ImageOption.SelectedIndex != 1)
            {//無圧縮ではない
                if (TSAOption.SelectedIndex >= 1)
                {//TSAを利用する
                    tsaeditorVisible = true;
                }
            }
            TSAEditorButton.Visible = tsaeditorVisible;
        }


        private void ZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void ImageOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetupImageOption();
            }
            catch (Exception ee)
            {
                R.Error(R.ExceptionToString(ee));
            }
            Draw();
        }

        void SetupImageOption()
        {
            if (ImageOption.SelectedIndex == 1)
            {
                SecondImagePanel.Hide();
                YYCHARMODEPAgeUp.Show();
                YYCHARMODEPAgeDown.Show();
            }
            else if (ImageOption.SelectedIndex == 2)
            {
                YYCHARMODEPAgeUp.Hide();
                YYCHARMODEPAgeDown.Hide();
                SecondImagePanel.Show();
            }
            else
            {
                YYCHARMODEPAgeUp.Hide();
                YYCHARMODEPAgeDown.Hide();
                SecondImagePanel.Hide();
            }

            if (TSAOption.SelectedIndex == 0)
            {
                KeepTSAComboBox.Hide();
            }
            else
            {
                KeepTSAComboBox.Show();
            }
        }
        private void TSA_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetupTSAOption();
            }
            catch (Exception ee)
            {
                R.Error(R.ExceptionToString(ee));
            }

            ImageOption_SelectedIndexChanged(sender, e);
        }
        void SetupTSAOption()
        {
            Size size = new Size(0, 0);
            if (TSAOption.SelectedIndex == 2)
            {//圧縮ヘッダ付きTSAを利用する
                byte[] tsa = LZ77.decompress(Program.ROM.Data, U.toOffset(TSA.Value));
                size = ImageUtil.CalcSizeForHeaderTSAData(tsa, 0);
            }
            else if (TSAOption.SelectedIndex == 3)
            {
                size = ImageUtil.CalcSizeForHeaderTSAData(Program.ROM.Data, (int)U.toOffset(TSA.Value));
            }
            if (size.Width > 0 && size.Height > 0)
            {
                //幅高さ自動計算
                if (size.Width >= 30)
                {
                    size.Width = 32;
                }
                if (size.Height >= 32)
                {
                    size.Height = 32;
                }
                this.PicWidth.Value = size.Width;
                this.PicHeight.Value = size.Height;
            }
        }

        uint refPointer(uint addr)
        {
            return U.GrepPointer(Program.ROM.Data,U.toPointer(addr));
        }

        string MakePatch()
        {
            uint imageAddress = (uint)this.Image.Value;
            uint imagePointer = refPointer(imageAddress);
            if (imagePointer == U.NOT_FOUND)
            {
                R.ShowStopError(R._("画像アドレス0x{0}を参照するポインタがありません"), this.Image.Value);
                return "";
            }

            string patch = "";
            patch += "NAME=<<PATCH NAME>>\r\n";
            patch += "\r\n";
            patch += "TYPE=IMAGE\r\n";
            patch += "\r\n";
            patch += "WIDTH=" + (this.PicWidth.Value * 8) + "\r\n";
            patch += "HEIGHT=" + (this.PicHeight.Value * 8) + "\r\n";
            patch += "\r\n";

            if (ImageOption.SelectedIndex == 0)
            {//圧縮画像
                patch += "ZIMAGE_POINTER=" + U.To0xHexString(U.toOffset(imagePointer)) + "//" + U.To0xHexString(U.toOffset(imageAddress)) + "\r\n";
            }
            else if (ImageOption.SelectedIndex == 2)
            {//第2圧縮画像がある場合
                uint image2Address = (uint)this.Image2.Value;
                uint image2Pointer = refPointer(image2Address);
                if (image2Pointer == U.NOT_FOUND)
                {
                    R.ShowStopError(R._("第2の画像アドレス0x{0}を参照するポインタがありません"), this.Image.Value);
                    return "";
                }

                patch += "ZIMAGE_POINTER=" + U.To0xHexString(U.toOffset(imagePointer)) + "//" + U.To0xHexString(U.toOffset(imageAddress)) + "\r\n";
                patch += "Z2IMAGE_POINTER=" + U.To0xHexString(U.toOffset(image2Pointer)) + "//" + U.To0xHexString(U.toOffset(image2Address)) + "\r\n";
            }
            else if (ImageOption.SelectedIndex == 3)
            {//圧縮256色画像
                patch += "Z256IMAGE_POINTER=" + U.To0xHexString(U.toOffset(imagePointer)) + "//" + U.To0xHexString(U.toOffset(imageAddress)) + "\r\n";
            }
            else
            {//無圧縮画像
                patch += "IMAGE_POINTER=" + U.To0xHexString(U.toOffset(imagePointer)) + "//" + U.To0xHexString(U.toOffset(imageAddress)) + "\r\n";
            }

            if (TSAOption.SelectedIndex == 0)
            {//TSAを利用しない
            }
            else
            {
                if (this.TSA.Value == 0)
                {
                    R.ShowStopError(R._("TSAアドレス0x{0}を参照するポインタがありません"), this.TSA.Value);
                    return "";
                }

                uint tsaAddress = (uint)this.TSA.Value;
                uint tsaPointer = refPointer(tsaAddress);
                if (tsaPointer == U.NOT_FOUND)
                {
                    R.ShowStopError(R._("TSAアドレス0x{0}を参照するポインタがありません"), this.TSA.Value);
                    return "";
                }

                if (TSAOption.SelectedIndex == 1)
                {//圧縮TSAを利用する
                    patch += "ZTSA_POINTER=" + U.To0xHexString(U.toOffset(tsaPointer)) + "//" + U.To0xHexString(U.toOffset(tsaAddress)) + "\r\n";
                }
                else if (TSAOption.SelectedIndex == 2)
                {//圧縮ヘッダ付きTSAを利用する
                    patch += "ZHEADERTSA_POINTER=" + U.To0xHexString(U.toOffset(tsaPointer)) + "//" + U.To0xHexString(U.toOffset(tsaAddress)) + "\r\n";
                }
                else if (TSAOption.SelectedIndex == 3)
                {//無圧縮ヘッダ付きTSAを利用する
                    patch += "HEADERTSA_POINTER=" + U.To0xHexString(U.toOffset(tsaPointer)) + "//" + U.To0xHexString(U.toOffset(tsaAddress)) + "\r\n";
                }
                else if (TSAOption.SelectedIndex == 4)
                {//無圧縮TSAを利用する
                    patch += "TSA_POINTER=" + U.To0xHexString(U.toOffset(tsaPointer)) + "//" + U.To0xHexString(U.toOffset(tsaAddress)) + "\r\n";
                }

                if (KeepTSAComboBox.SelectedIndex == 1)
                {
                    patch += "KEEPTSA=true\r\n";
                }
            }

            if (this.PALETTE.Value == 0)
            {
                R.ShowStopError(R._("パレットアドレス0x{0}を参照するポインタがありません"), this.PALETTE.Value);
                return "";
            }

            uint paletteAddress = (uint)this.PALETTE.Value + (uint)(this.PALETTENO.Value * 0x20);
            uint palettePointer = refPointer(paletteAddress);
            if (palettePointer != U.NOT_FOUND)
            {
                patch += "PALETTE_POINTER=" + U.To0xHexString(U.toOffset(palettePointer)) + "//" + U.To0xHexString(U.toOffset(paletteAddress)) + "\r\n";
            }
            else
            {
                patch += "PALETTE_ADDRESS=" + U.To0xHexString(U.toOffset(paletteAddress)) + "\r\n";
                patch += "KEEPPALETTE=1\r\n";
            }

            return patch;
        }

        private void PatchMakerButton_Click(object sender, EventArgs e)
        {
            string patch = MakePatch();
            if (patch == "")
            {
                R.ShowStopError("パッチの生成に失敗しました");
                return;
            }

            //パッチ内容を画面に表示
            GraphicsToolPatchMakerForm f = (GraphicsToolPatchMakerForm)InputFormRef.JumpFormLow<GraphicsToolPatchMakerForm>();
            f.Init(patch);
            f.ShowDialog();
        }

        private void ZImageNo_ValueChanged(object sender, EventArgs e)
        {
            int index = (int)ZImageNo.Value;
            if ( index >= FoundImages.Count)
            {
                return ;
            }
            U.ForceUpdate(Image, FoundImages[index].addr);
            U.ForceUpdate(ImageOption, 0);
            
        }

        //TSAとパレットレコメンド
        void MakeTSAAndPaletteRecomend(uint image_address)
        {
            image_address = U.toOffset(image_address);

            TSA_Recomend.Items.Clear();
            PALETTE_Recomend.Items.Clear();

            uint image_pointer = refPointer(image_address);
            if (image_pointer == U.NOT_FOUND)
            {
                return;
            }
            //画像のサイズ
            uint imageDataSize = LZ77.getUncompressSize(Program.ROM.Data, image_address);
            if (imageDataSize <= 0)
            {
                imageDataSize = (uint)(PicWidth.Value * 8 * PicHeight.Value * 8);
            }

            //画像アドレスポインタ -0x100 ～ +0x100 の範囲を探索 
            uint p = image_pointer - 0x100;
            if (! U.isSafetyOffset(p))
            {
                return ;
            }

            uint end = image_pointer + 0x100;
            if (! U.isSafetyOffset(end))
            {
                end = (uint)Program.ROM.Data.Length;
            }

            TSA_Recomend.BeginUpdate();
            PALETTE_Recomend.BeginUpdate();

            TSA_Recomend.Items.Add("0(NONE)");
            PALETTE_Recomend.Items.Add("0(NONE)");

            for (; p < end; p += 4)
            {
                if (p == image_pointer)
                {//自分自身
                    continue;
                }
                uint a = Program.ROM.Data[p+3];
                if (a != 0x08 && a != 0x09)
                {//探索を早くするため +3 がポインタマークでなければ切り捨て
                    continue;
                }

                a = U.p32(Program.ROM.Data,p);
                if (! U.isSafetyOffset(a))
                {//危険なポインタなので無視
                    continue;
                }
                if (a < Program.ROM.RomInfo.compress_image_borderline_address)
                {
                    continue;
                }
                if (! U.isPadding4(a))
                {
                    continue;
                }
                
                uint unCompressSize = LZ77.getUncompressSize(Program.ROM.Data,a);
                if (unCompressSize > 0x10)
                {//圧縮されている
                    if (unCompressSize >= 32 * 32 * 2)
                    {//TSAだとしても、大きすぎるため、没
                        continue;
                    }
                    //解凍して内容を確認する.
                    byte[] b = LZ77.decompress(Program.ROM.Data, a);
                    if (b.Length < unCompressSize)
                    {//偽圧縮データ
                        continue;
                    }

                    if (IsTSAData(b, 0, imageDataSize))
                    {//圧縮TSA
                        TSA_Recomend.Items.Add(U.ToHexString(a) + "(Z)");
                    }
                    else if (IsHeaderTSAData(b, 0, imageDataSize))
                    {//圧縮HEADERTSA
                        TSA_Recomend.Items.Add(U.ToHexString(a) + "(ZHEADER)");
                    }
//                    else if (IsPaletteMap(b,0))
//                    {//パレットマップ(worldmapにしかないので意味はないのだが..)
//                        TSA_Recomend.Items.Add(U.ToHexString(a) + "(PALMAP)");
//                    }
                    //無関係のデータなので無視
                    continue;
                }
                else
                {//無圧縮データ
                    if (IsHeaderTSAData(Program.ROM.Data, a, imageDataSize))
                    {//無圧縮HEADERTSA
                        TSA_Recomend.Items.Add(U.ToHexString(a) + "(HEADER)");
                    }
                    else if (IsPalette(Program.ROM.Data, a))
                    {//パレットかなあ・・・ パレットの判別はできない。明らかに違うものをはじくだけ
                        PALETTE_Recomend.Items.Add(U.ToHexString(a));
                    }
                }
            }
            PALETTE_Recomend.EndUpdate();
            TSA_Recomend.EndUpdate();

            //パレットは誤っていも問題ないので、選択してみる
            if (!U.SelectedIndexSafety(PALETTE_Recomend, 1, false))
            {
                U.SelectedIndexSafety(PALETTE_Recomend, 0, false);
            }
            //TSAは間違っているとまずいので無選択
            U.SelectedIndexSafety(TSA_Recomend, 0, false);
        }


        private void TSA_Recomend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TSA_Recomend.SelectedIndex < 0)
            {
                return;
            }
            U.ForceUpdate(TSA,  U.atoh(TSA_Recomend.Text));

            if (TSA_Recomend.Text.IndexOf("(Z)") >= 0)
            {
                U.ForceUpdate(TSAOption,1);  //圧縮TSAを利用する
            }
            else if (TSA_Recomend.Text.IndexOf("(ZHEADER)") >= 0)
            {
                U.ForceUpdate(TSAOption, 2);  //圧縮ヘッダ付きTSAを利用する
            }
            else if (TSA_Recomend.Text.IndexOf("(HEADER)") >= 0)
            {
                U.ForceUpdate(TSAOption, 3);  //無圧縮ヘッダ付きTSAを利用する
            }
            else if (TSA_Recomend.Text.IndexOf("(TSA)") >= 0)
            {
                U.ForceUpdate(TSAOption, 4);  //無圧縮TSAを利用する
            }
            else
            {
                U.ForceUpdate(TSAOption, 0); //TSAを利用しない
            }
        }
        private void PALETTE_Recomend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PALETTE_Recomend.SelectedIndex < 0)
            {
                return;
            }
            PALETTE.Value = U.atoh(PALETTE_Recomend.Text);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (this.DrawBimap == null)
            {
                return;
            }
            ImageFormRef.ExportImage(this
                , this.DrawBimap
                , InputFormRef.MakeSaveImageFilename(this, U.ToHexString((uint)Image.Value))
                , (int)U.atoi(this.USE_PALETTE_NUMBER.Text) 
                );
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            string patch = MakePatch();
            if (patch == "")
            {
                R.ShowStopError("パッチの生成に失敗しました");
                return;
            }

            uint img = (uint)this.Image.Value;
            uint img2 = (uint)this.Image2.Value;
            uint tsa = (uint)this.TSA.Value;
            uint pal = (uint)this.PALETTE.Value;

            //パッチを生成してimportしてみよう.
            Panel dummy = new Panel();
            this.Controls.Add(dummy);
            bool r = PatchForm.ImportImageOneTime(patch, dummy, this, ref img, ref img2, ref tsa, ref pal);
            this.Controls.Remove(dummy);
            if (r == false)
            {
                R.ShowStopError("ImportImageOneTimeが失敗しました");
                return;
            }

            U.ForceUpdate(this.Image, img);
            U.ForceUpdate(this.Image2, img2);

            U.ForceUpdate(this.TSA, tsa);
            U.ForceUpdate(this.PALETTE, pal);

            Draw();
            this.FoundImages = FindImage();
        }

        private void TSAEditorButton_Click(object sender, EventArgs e)
        {
            ImageTSAEditorForm f = (ImageTSAEditorForm)InputFormRef.JumpForm<ImageTSAEditorForm>(U.NOT_FOUND);
            uint width8 = (uint)PicWidth.Value;
            uint height8 = (uint)PicHeight.Value;

            uint imageAddress = (uint)this.Image.Value;
            uint zimgPointer = refPointer(imageAddress);

            bool isHeaderTSA = false;
            bool isLZ77TSA = false;
            if (this.TSAOption.SelectedIndex == 1)
            {//圧縮TSAを利用する
                isLZ77TSA = true;
            }
            else if (this.TSAOption.SelectedIndex == 2)
            {//圧縮ヘッダ付きTSAを利用する
                isLZ77TSA = true;
                isHeaderTSA = true;
            }
            else if (this.TSAOption.SelectedIndex == 3)
            {//無圧縮ヘッダ付きTSAを利用する
                isHeaderTSA = true;
            }
            //else if (this.TSAOption.SelectedIndex == 4)
            //{//無圧縮TSAを利用する
            //}

            uint tasAddress = (uint)this.TSA.Value;
            uint tsaPointer = refPointer(tasAddress);

            uint paletteAddress = (uint)this.PALETTE.Value;
            uint palettePointer = refPointer(paletteAddress);

            int paletteCount = (int)U.atoi(USE_PALETTE_NUMBER.Text);
            if (paletteCount <= 0)
            {
                paletteCount = 1;
            }

            f.Init(width8, height8, zimgPointer, isHeaderTSA, isLZ77TSA, tsaPointer, palettePointer, paletteAddress, paletteCount);

            Draw();
            this.FoundImages = FindImage();
        }

        private void YYCHARMODEPAgeUp_Click(object sender, EventArgs e)
        {
            int page = (int)PicWidth.Value * 8 * (int)PicHeight.Value * 8;
            int addr = (int)Image.Value;
            if (addr < page)
            {
                addr = 0;
            }
            else
            {
                addr -= page;
            }
            U.ForceUpdate(Image, addr);
        }

        private void YYCHARMODEPAgeDown_Click(object sender, EventArgs e)
        {
            int page = (int)PicWidth.Value * 8 * (int)PicHeight.Value * 8;
            int addr = (int)Image.Value;
            if (addr >= Program.ROM.Data.Length)
            {
                addr = Program.ROM.Data.Length;
            }
            else
            {
                addr += page;
            }
            U.ForceUpdate(Image, addr);
        }

        private void KeepTSAComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void PaletteEditorButton_Click(object sender, EventArgs e)
        {
            if (this.DrawBimap == null)
            {
                return;
            }
            if (PaletteOption.SelectedIndex == 1)
            {
                R.ShowStopError("lz77パレットなので変更できません。");
                return;
            }

            ImagePalletForm f = (ImagePalletForm)InputFormRef.JumpForm<ImagePalletForm>(U.NOT_FOUND);

            int usePaletteNumber = (int)U.atoi(USE_PALETTE_NUMBER.Text);
            if (usePaletteNumber <= 0)
            {
                usePaletteNumber = 1;
            }

            if (usePaletteNumber >= 16)
            {
                f.JumpTo(this.DrawBimap, (uint)(this.PALETTE.Value + (this.PALETTENO.Value * 0x20)), 16);
            }
            else
            {
                f.JumpTo(this.DrawBimap, (uint)(this.PALETTE.Value + (this.PALETTENO.Value * 0x20)), 1);
            }
            f.FormClosed += (s, ee) =>
            {
                if (this.IsDisposed)
                {
                    return;
                }
                Image_ValueChanged(sender, e);
            };
        }
        public static bool IsBadImageSize(uint imageDataSize)
        {
            return (imageDataSize < (8 * 8 / 2)  //圧縮されていないか、小さすぎる
                || imageDataSize >= (8 / 2 * 32 * 8 * 32) //大きすぎる
                || imageDataSize % (8 * 8 / 2) != 0 //32(8*8/2)で割り切れない
                );
        }

        //ポインタは連続してあらわれるのでそのチェックをする.
        static bool IsContinuousPointer(uint addr, uint length)
        {
            if (addr <= Program.ROM.RomInfo.compress_image_borderline_address)
            {//プログラム領域であればポインタが散在していてもいい
                return true;
            }

            //データ領域であれば、ポインタが連続しているはずだ.
            //画像にはパレットやTSAがセットになるはずだから、ポインタだけが出てくるわけがないのだ.
            for (int checkRange = 4; checkRange <= 0x8; checkRange += 4)
            {
                uint a = (uint)(addr - checkRange);
                uint p = Program.ROM.u32( a );
                if (U.isSafetyPointer(p))
                {//安全なポインタ
                    return true;
                }
            }
            for (int checkRange = 4; checkRange <= 0xC; checkRange += 4)
            {
                uint a = (uint)(addr + checkRange);
                if (a + 4 >= length)
                {
                    break;
                }
                uint p = Program.ROM.u32(a);
                if (U.isSafetyPointer(p))
                {//安全なポインタ
                    return true;
                }
            }
            //おそらくこのマッチは間違っている.
            return false;
        }
        static void MakeIgnoreDictionnaryFromList(Dictionary<uint, bool> dic, List<Address> list)
        {
            foreach(Address a in list)
            {
                dic[a.Addr] = true;
            }
        }

        public static void MakeLZ77DataList(List<Address> list)
        {
            //誤爆すると面倒なことになるフレームとOAMのデータ群
            Dictionary<uint, bool> ignoreDic = new Dictionary<uint, bool>();
            ImageBattleAnimeForm.MakeBattleFrameAndOAMDictionary(ignoreDic);
            SoundFootStepsForm.MakeIgnoreDictionary(ignoreDic);
            WorldMapPointForm.MakeIgnoreDictionary(ignoreDic);
            MakeIgnoreDictionnaryFromList(ignoreDic, list);

            string name = R._("圧縮データ");
            uint length = (uint)Program.ROM.Data.Length - 4;
            for (uint addr = 0x100; addr < length; addr += 4)
            {
                uint a = (uint)Program.ROM.Data[addr + 3];
                if (a != 0x08 && a != 0x09)
                {//ポインタ以外無視する.
                    continue;
                }
                a = Program.ROM.p32(addr);
                if (!U.isSafetyOffset(a))
                {//危険なポインタは無視
                    continue;
                }
                if (a < Program.ROM.RomInfo.compress_image_borderline_address)
                {
                    continue;
                }
                if (!U.isPadding4(a))
                {//4バイトパディングされていないlz77データはありえないとする.
                    continue;
                }

                if (ignoreDic.ContainsKey(a))
                {//戦闘アニメのフレーム,OAM等のlz77で圧縮されているデータ
                    continue;
                }

                //ポインタ先は圧縮されているか？
                uint imageDataSize = LZ77.getUncompressSize(Program.ROM.Data, a);
                if (IsBadImageSize(imageDataSize) )
                {
                    continue;
                }

                //ポインタは連続してあらわれるのでそのチェックをする.
                if (!IsContinuousPointer(addr, length))
                {
                    continue;
                }

                uint getcompsize = LZ77.getCompressedSize(Program.ROM.Data, a);
                if (getcompsize == 0)
                {
                    continue;
                }

                //たぶん画像だと判断する.
                FEBuilderGBA.Address.AddAddress(list, a, getcompsize, addr, name + U.To0xHexString(a), Address.DataTypeEnum.LZ77IMG);
                if (InputFormRef.DoEvents(null, "MakeLZ77DataList " + U.ToHexString(addr))) return;
            }
        }

        public void Jump(int width,int height, uint image,int imageType,uint tsa,int tsaType,uint palette,int paletteType,int paletteCount,uint image2)
        {
            this.Image.Value = U.toOffset(image);
            this.Image2.Value = U.toOffset(image2);
            U.SelectedIndexSafety(this.ImageOption, imageType);
            this.PicWidth.Value = width / 8;
            this.PicHeight.Value = height / 8;

            this.TSA.Value = U.toOffset(tsa);
            U.SelectedIndexSafety(this.TSAOption, tsaType);

            this.PALETTE.Value = U.toOffset(palette);
            this.PALETTENO.Value = 0;
            U.SelectedIndexSafety(this.PaletteOption, paletteType);

            this.Image.Focus();

            if (paletteCount >= 1)
            {
                USE_PALETTE_NUMBER.Text = paletteCount.ToString();
            }
        }

        private void PaletteType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageOption_SelectedIndexChanged(sender, e);
        }

        private void DataDumpButton_Click(object sender, EventArgs e)
        {
            if (this.DrawBimap == null)
            {
                return;
            }
            string savefilename = 
            ImageFormRef.ExportImage(this
                , this.DrawBimap
                , InputFormRef.MakeSaveImageFilename(this, U.ToHexString((uint)Image.Value))
                , (int)U.atoi(this.USE_PALETTE_NUMBER.Text)
                );
            if (savefilename == "")
            {
                return;
            }

            if (ImageOption.SelectedIndex == 0 || ImageOption.SelectedIndex == 3)
            {//圧縮画像
                byte[] image = LZ77.GetCompressDataLow(Program.ROM.Data
                    , U.toOffset(Image.Value));
                if (image.Length > 0)
                {
                    U.WriteAllBytes(savefilename + ".dmp", image);
                }
            }
            else if (ImageOption.SelectedIndex == 2)
            {//第2圧縮画像
                byte[] image = LZ77.GetCompressDataLow(Program.ROM.Data
                    , U.toOffset(Image.Value));
                if (image.Length > 0)
                {
                    U.WriteAllBytes(savefilename + ".dmp", image);
                }

                byte[] image2 = LZ77.GetCompressDataLow(Program.ROM.Data
                    , U.toOffset(Image2.Value));
                if (image.Length > 0)
                {
                    U.WriteAllBytes(savefilename + ".img2.dmp", image2);
                }
            }
            else
            {//無圧縮画像
                uint image_size = ((uint)PicWidth.Value * 8 / 2) * ((uint)PicHeight.Value * 8);
                byte[] image = U.getBinaryData(Program.ROM.Data, U.toOffset(Image.Value), image_size);
                U.WriteAllBytes(savefilename + ".dmp", image);
            }

            if (PaletteOption.SelectedIndex == 1)
            {//lz77 palette
                byte[] palette = LZ77.GetCompressDataLow(Program.ROM.Data
                    , U.toOffset(PALETTE.Value));
                if (palette.Length > 0)
                {
                    U.WriteAllBytes(savefilename + ".pal.dmp", palette);
                }
            }
            else
            {//通常のパレット
                uint palette_addr = ToPaletteOffset((uint)PALETTE.Value, (uint)PALETTENO.Value);

                uint palette_count = U.atoi(this.USE_PALETTE_NUMBER.Text);
                if (palette_count < 0)
                {//パレットがないのはありえないので.
                    palette_count = 1;
                }
                if (ImageOption.SelectedIndex == 3)
                {//256色
                    palette_count = 16;
                }

                byte[] palette = Program.ROM.getBinaryData(palette_addr, 0x20 * palette_count);
                U.WriteAllBytes(savefilename + ".pal.dmp", palette);
            }

            if (TSAOption.SelectedIndex == 0)
            {//TSAを利用しない
            }
            else if (TSAOption.SelectedIndex == 1 || TSAOption.SelectedIndex == 2)
            {//圧縮TSAを利用する , or 圧縮ヘッダ付きTSAを利用する
                byte[] tsa = LZ77.GetCompressDataLow(Program.ROM.Data
                    , U.toOffset(TSA.Value));
                if (tsa.Length > 0)
                {
                    U.WriteAllBytes(savefilename + ".tsa.dmp", tsa);
                }

            }
            else if (TSAOption.SelectedIndex == 3)
            {//無圧縮ヘッダ付きTSAを利用する
                uint tsa_addr = U.toOffset(TSA.Value);
                uint tsa_size = ImageUtil.CalcByteLengthForHeaderTSAData(Program.ROM.Data, (int)tsa_addr);
                if (tsa_size > 0)
                {
                    byte[] tsa = Program.ROM.getBinaryData(tsa_addr, tsa_size);
                    U.WriteAllBytes(savefilename + ".tsa.dmp", tsa);
                }
            }
            else if (TSAOption.SelectedIndex == 4)
            {//無圧縮TSAを利用する
                uint width = (uint)PicWidth.Value * 8;
                uint height = (uint)PicHeight.Value * 8;

                uint tsa_addr = U.toOffset(TSA.Value);
                uint tsa_size = width * height / 32;
                if (tsa_size > 0)
                {
                    byte[] tsa = Program.ROM.getBinaryData(tsa_addr, tsa_size);
                    U.WriteAllBytes(savefilename + ".tsa.dmp", tsa);
                }
            }
            else if (TSAOption.SelectedIndex == 5)
            {//パレットマップとして解釈する
                uint width = (uint)PicWidth.Value * 8;
                uint height = (uint)PicHeight.Value * 8;

                uint tsa_addr = U.toOffset(TSA.Value);
                uint tsa_size = width * height / 2;
                if (tsa_size > 0)
                {
                    byte[] tsa = Program.ROM.getBinaryData(tsa_addr, tsa_size);
                    U.WriteAllBytes(savefilename + ".tsa.dmp", tsa);
                }
            }
        }
    }
}
