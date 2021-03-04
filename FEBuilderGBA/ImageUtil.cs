using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Diagnostics;


namespace FEBuilderGBA
{
    class ImageUtil
    {
        public static Bitmap BlankDummy(int wh = 8)
        {
            return Blank(wh, wh);
        }
        //ダミー用
        public static Bitmap Blank(int width, int height)   
        {
            ValidateWidthAndHeight(ref width, ref height);
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            return pic;
        }
        //ダミー用
        public static Bitmap Blank(int width, int height, byte[] palette, int palette_pos)
        {
            ValidateWidthAndHeight(ref width, ref height);
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレットの読込.
            pic.Palette = ByteToPalette(pic.Palette, palette, palette_pos);
            return pic;
        }
        //ダミー用
        public static Bitmap Blank(int width, int height, Bitmap palettebitmap)
        {
            ValidateWidthAndHeight(ref width , ref height);
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレットの読込.
            pic.Palette = palettebitmap.Palette;
            return pic;
        }
        static void ValidateWidthAndHeight(ref int ref_width, ref int ref_height)
        {
            if (ref_width <= 0)
            {
                Log.Error(R.Error("ValidateWidthAndHeight {0} , {1}" , ref_width , ref_height));
                Debug.Assert(false);
                ref_width = 8;
            }
            if (ref_height <= 0)
            {
                Log.Error(R.Error("ValidateWidthAndHeight {0} , {1}", ref_width, ref_height));
                Debug.Assert(false);
                ref_height = 8;
            }
        }

        //パレットを使った256色BMPかどうか.
        public static bool Is8bppIndexed(Bitmap pic)
        {
            return pic.PixelFormat == PixelFormat.Format8bppIndexed;
        }

        //FONT用 4色
        public static Bitmap ByteToImage4(int width, int height, byte[] image, int image_pos, Color bgColor)
        {
            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレット
            if (bgColor == null)
            {
                bgColor = Color.FromArgb(255, 255, 255);
            }
            ColorPalette palette = pic.Palette; //一度、値をとってからいじらないと無視される・・ C#むずかしい
            //日本語、英語版は以下のルールで統一
            palette.Entries[0] = bgColor;
            palette.Entries[1] = Color.FromArgb(0xA8, 0xA8, 0xA7); //グレー
            palette.Entries[2] = Color.FromArgb(0xF8, 0xF8, 0xF8); //白
            palette.Entries[3] = Color.FromArgb(0x28, 0x28, 0x28); //黒

            for (int i = 4; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = Color.FromArgb(0, 0, 0);
            }
            pic.Palette = palette;

            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int length = image_pos + ((width * height) / 4);
            length = Math.Min(length, image.Length);

            int x = 0;
            int y = 0;
            for (int i = image_pos; i < length; i++)
            {
                byte a = image[i];
                byte a0 = (byte)(a & 0x03);
                byte a1 = (byte)((a >> 2) & 0x03);
                byte a2 = (byte)((a >> 4) & 0x03);
                byte a3 = (byte)((a >> 6) & 0x03);

                if (a0 > 0)
                    Marshal.WriteByte(adr, (x + 0) + bmpData.Stride * y, a0);
                if (a1 > 0)
                    Marshal.WriteByte(adr, (x + 1) + bmpData.Stride * y, a1);
                if (a2 > 0)
                    Marshal.WriteByte(adr, (x + 2) + bmpData.Stride * y, a2);
                if (a3 > 0)
                    Marshal.WriteByte(adr, (x + 3) + bmpData.Stride * y, a3);

                x += 4;
                if (x >= width)
                {
                    x = 0;
                    y++;
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }

        //FONT用 4色 (中国語拡張)
        public static Bitmap ByteToImage4ZH(int width, int height, byte[] image, int image_pos, Color bgColor)
        {
            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレット
            if (bgColor == null)
            {
                bgColor = Color.FromArgb(255, 255, 255);
            }
            ColorPalette palette = pic.Palette; //一度、値をとってからいじらないと無視される・・ C#むずかしい

            //中国語版はパレットが少し違う.
            palette.Entries[0] = bgColor;
            palette.Entries[1] = Color.FromArgb(0xF8, 0xF8, 0xF8); //白
            palette.Entries[2] = Color.FromArgb(0xA8, 0xA8, 0xA7); //グレー
            palette.Entries[3] = Color.FromArgb(0x28, 0x28, 0x28); //黒
            for (int i = 4; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = Color.FromArgb(0, 0, 0);
            }
            pic.Palette = palette;

            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int length = image_pos + ((width * height) / 4);
            length = Math.Min(length, image.Length);

            int x = 0;
            int y = 0;
            for (int i = image_pos; i < length; i++)
            {
                byte a = image[i];
                byte a0 = (byte)(a & 0x03);
                byte a1 = (byte)((a >> 2) & 0x03);
                byte a2 = (byte)((a >> 4) & 0x03);
                byte a3 = (byte)((a >> 6) & 0x03);

                if (a0 > 0)
                    Marshal.WriteByte(adr, (x) + bmpData.Stride * y, a0);
                x++; //中国語版の場合、バイト列は4バイト単位とは限らない
                if (x >= width)
                {
                    x = 0;
                    y++;
                }
                
                if (a1 > 0)
                    Marshal.WriteByte(adr, (x) + bmpData.Stride * y, a1);
                x++; //中国語版の場合、バイト列は4バイト単位とは限らない
                if (x >= width)
                {
                    x = 0;
                    y++;
                }

                if (a2 > 0)
                    Marshal.WriteByte(adr, (x) + bmpData.Stride * y, a2);
                x++; //中国語版の場合、バイト列は4バイト単位とは限らない
                if (x >= width)
                {
                    x = 0;
                    y++;
                }

                if (a3 > 0)
                    Marshal.WriteByte(adr, (x) + bmpData.Stride * y, a3);
                x++; //中国語版の場合、バイト列は4バイト単位とは限らない
                if (x >= width)
                {
                    x = 0;
                    y++;
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }
        //中国語 FONT用 4色のインポート 16x16
        public static byte[] Image4ToByteZH(Bitmap bitmap, int font_width_hint)
        {
            byte[] data = new byte[40];

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int height = bitmap.Height;
            int width = Math.Min(bitmap.Width , font_width_hint);

            int nn = 0;
            for (int y = 0; y < height; y++)
            {
                int n = 0;
                byte one = 0;
                for (int x = 0; x < width; x++)
                {
                    byte a = Marshal.ReadByte(adr, x + bmpData.Stride * y);
                    a = (byte)(a & 0x03);
                    a = (byte)(a << (n * 2));
                    one = (byte)(one | a);
                    n++;
                    if (n >= 4)
                    {
                        data[nn++] = one;
                        n = 0;
                        one = 0;

                        if (nn >= 40)
                        {
                            break;
                        }
                    }
                }
                if (nn >= 40)
                {
                    break;
                }
            }
            bitmap.UnlockBits(bmpData);
            return data;
        }


        //バイト列からパレットの構築.
        public static ColorPalette ByteToPalette(ColorPalette basepalette, byte[] palette, int palette_pos)
        {
            try
            {
                return ByteToPaletteLow(basepalette, palette, palette_pos);
            }
            catch (IndexOutOfRangeException e)
            {
                //たまにこのエラーが発生する原因不明なので、ログを取る.
                int pal = 0;
                if (basepalette.Entries != null)
                {
                    pal = basepalette.Entries.Length;
                }
                throw new IndexOutOfRangeException("ByteToPalette basepalette=" + pal
                    + " palette=" + palette.Length 
                    + " palette_pos=" + palette_pos 
                    + " msg=" + e.ToString()
                    , e
                    );
            }
        }
        //バイト列からパレットの構築.
        static ColorPalette ByteToPaletteLow(ColorPalette basepalette, byte[] palette, int palette_pos)
        {
            int n = 0;
            if (palette_pos < 0)
            {//パレットの位置がマイナスになることがたまにあるらしい.
                Debug.Assert(false);
                for (; n < basepalette.Entries.Length; n++)
                {
                    basepalette.Entries[n] = Color.FromArgb(255, 0, 0);
                }
                return basepalette;
            }

            int length = palette_pos + (2 * 16) * 16; //2バイト16色のパレットが16個ある.
            length = Math.Min(length, palette.Length);

            for (int i = palette_pos; i < length; i+=2,n++)
            {
                if (n >= basepalette.Entries.Length)
                {
                    Debug.Assert(false);
                    break;
                }
                if (i + 1 >= length)
                {
                    Debug.Assert(false);
                    break;
                }

                UInt16 p = (UInt16)(palette[i] + (palette[i+1] << 8));

                byte r = (byte) (p & 0x1F);
                byte g = (byte) ((p >> 5) & 0x1F);
                byte b = (byte) ((p >> 10) & 0x1F);
                basepalette.Entries[n] = Color.FromArgb(r<<3, g<<3, b<<3);
            }
            for (; n < basepalette.Entries.Length; n++)
            {
                basepalette.Entries[n] = Color.FromArgb(255, 0, 0);
            }
            return basepalette;
        }


        //Tile + 16色
        //タイル形式は、 8x8ピクセルごとに分断されている.
        //0123 0123 
        //4567 4567 
        //89ab 89ab
        //cdef cdef
        //0123 0123
        //4567 4567
        //89ab 89ab
        //cdef cdef
        public static Bitmap ByteToImage16Tile(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, int palette_count = 0)
        {
            if (width <= 0)
            {
                width = 8;
                Debug.Assert(false);
            }
            if (height <= 0)
            {
                height = 8;
                Debug.Assert(false);
            }

            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            //パレットの読込.
            pic.Palette = ByteToPalette(pic.Palette, palette, palette_pos);
            if (image_pos < 0) 
            {//画像の場所がマイナスになるパターンが稀にあるらしい.
                Debug.Assert(false);
                return pic;
            }


            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            //パレットは16色のパレットが16個ある
            int paletteIndexCap = (palette_count * 16);

            int length = image_pos + ((width * height) / 2);
            length = Math.Min(length, image.Length);

            int x = 0;
            int y = 0;
            for (int i = image_pos; i < length; )
            {
                for (int y8 = 0; y8 < 8; y8++)
                {
                    for (int x8 = 0; x8 < 8; x8+=2)
                    {
                        byte a = image[i];
                        byte a0 = (byte)(((a & 0x0F) + paletteIndexCap) & 0xFF);
                        byte a1 = (byte)((((a >> 4) & 0x0F) + paletteIndexCap) & 0xFF);

                        Marshal.WriteByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8), a0);
                        Marshal.WriteByte(adr, (x + x8 + 1) + bmpData.Stride * (y + y8), a1);

                        i++;
                        if (i >= length)
                        {
                            pic.UnlockBits(bmpData);
                            return pic;
                        }
                    }
                }
                x += 8;

                if (x >= width)
                {
                    x = 0;
                    y+=8;
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }

        //Tile + 16色 + パレットマップ(でかいWM用)
        public static Bitmap ByteToImage16TilePaletteMap(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, byte[] palettemap,int palettemap_pos)
        {
            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレットの読込.
            pic.Palette = ByteToPalette(pic.Palette, palette, palette_pos);

            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int length = image_pos + ((width * height) / 2);
            length = Math.Min(length, image.Length);

            int x = 0;
            int y = 0;
            int palettemap_index = 0x0;
            for (int i = image_pos; i < length; )
            {
                uint paletteIndexCap = (uint)U.at(palettemap, palettemap_pos + (palettemap_index / 2), 0);
                if((palettemap_index & 0x1) > 0)
                {
                    paletteIndexCap = (paletteIndexCap >> 4) & 0xF;
                }
                else
                {
                    paletteIndexCap = paletteIndexCap & 0xF;
                }
                paletteIndexCap = paletteIndexCap * 0x10;

                for (int y8 = 0; y8 < 8; y8++)
                {
                    for (int x8 = 0; x8 < 8; x8 += 2)
                    {
                        byte a = image[i];
                        byte a0 = (byte)(((a & 0x0F) + paletteIndexCap) & 0xFF);
                        byte a1 = (byte)((((a >> 4) & 0x0F) + paletteIndexCap) & 0xFF);

                        Marshal.WriteByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8), a0);
                        Marshal.WriteByte(adr, (x + x8 + 1) + bmpData.Stride * (y + y8), a1);

                        i++;
                        if (i >= length)
                        {
                            pic.UnlockBits(bmpData);
                            return pic;
                        }
                    }
                }
                x += 8;
                palettemap_index++;

                if (x >= width)
                {
                    x = 0;
                    y += 8;
                    //ワールドマップのパレットマップはHEADERTSAみたいに、画面外の余白があるらしい...
                    palettemap_index+=4;
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }

        //TSA読込
        public static UInt16[] ByteToTSA(byte[] tsa, int tsa_pos, int width, int height)
        {
            int size = (width / 8)  * (height / 8);
            UInt16[] tile = new UInt16[size];
            Array.Clear(tile, 0, size);

            int length = tsa_pos + ( size * 2);
            length = Math.Min(length, tsa.Length / 2 * 2);

            int n = 0;
            for (int i = tsa_pos; i < length; i+=2)
            {
                UInt16 tsadata = (UInt16)(tsa[i] + ((UInt16)tsa[i + 1] << 8));
                tile[n++] = tsadata;
            }
            return tile;
        }

        //ヘッダー付きTSA読込 0x080043C 32ビットモード (FE8J)
        public static UInt16[] ByteToHeaderTSA(byte[] tsa, int tsa_pos, int width, int height)
        {
            int size = (width / 8) * (height / 8);

            if (tsa.Length < tsa_pos + 2)
            {//ヘッダーが壊れている.
                UInt16[] blankTSA = new UInt16[size];
                return blankTSA;
            }

            //最初の2バイトにヘッダがある.
            int i = tsa_pos;
            int master_headerx = tsa[i];
            int master_headery = tsa[i + 1];
            if (master_headerx > 32 || master_headery > 32)
            {//ヘッダーが壊れている.
                UInt16[] blankTSA = new UInt16[size];
                return blankTSA;
            }


            if (master_headerx * master_headery > size)
            {//指示された値よりヘッダの方がでかいばあいはヘッダに従う.
                size = master_headerx * master_headery;
            }
            UInt16[] tile = new UInt16[size];

            int length = tsa_pos + (size * 2) + 2;
            length = Math.Min(length, tsa.Length);

            i += 2;

            //後ろから、補正値を入れて足しこむらしい.
            int n = 0 + (master_headery << 5); //*32
            if (n >= size)
            {//エラー
                return tile;
            }

            for (int headery = 0; headery <= master_headery; headery++)
            {
                for (int headerx = 0; headerx <= master_headerx; headerx++)
                {
                    if (i+1 >= length)
                    {//途中でデータがなくなった...
                        return tile;
                    }
                    if (n >= tile.Length)
                    {
                        return tile;
                    }
                    UInt16 tsadata = (UInt16)(tsa[i] + ((UInt16)tsa[i + 1] << 8));
                    tile[n] = (UInt16)(tsadata );

                    i += 2;
                    n++;
                }
                n = n - master_headerx ;
                n = n - (0x42 / 2);
            }
            return tile;
        }

        //Tile + 16色 + TSA
        public static Bitmap ByteToImage16TileInner(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, UInt16[] tile, int palette_count)
        {
            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレットの読込.
            pic.Palette = ByteToPalette(pic.Palette, palette, palette_pos);

            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            //TSA参照がある場合、参照限界は、画像データそのもののサイズ.
            int length = image.Length;

            int x = 0;
            int y = 0;

            //処理を早くするため、8の倍数に補正する.
            width = (width / 8) * 8;
            height = (height / 8) * 8;

            //TSAで読み込む場合、ソース位置が変更になるらしい.
            int tilelength =tile.Length;
            for (int tsaindex = 0; tsaindex < tilelength; tsaindex++, x += 8)
            {
                if (x >= width)
                {
                    x = 0;
                    y += 8;
                    if (y >= height)
                    {
                        break;
                    }
                }
                UInt16 tsatile = tile[tsaindex];

                if (tsatile == 0xFFFF)
                {
                    continue;
                }
                //TSA see https://dw.ngmansion.xyz/doku.php?id=column:tsa
                UInt16 tileNumber = (UInt16)(tsatile & 0x03FF);

                //TSAのtileNumberの場所にソースデータがある.
                int i = image_pos + tileNumber * 32;
                if (i >= length)
                {
                    continue;
                }

                //パレット番号 パレットは16色が16個(合計16*16)ある.
                //パレット番号に挿げ替える.
                int paletteIndexCap = (((tsatile >> 12) & 0xF) * 16) + (palette_count*16);

                for (int y8 = 0; y8 < 8; y8++)
                {
                    for (int x8 = 0; x8 < 8; x8 += 2)
                    {
                        byte a = image[i];
                        byte a0 = (byte)(((a & 0x0F) + paletteIndexCap) & 0xFF);
                        byte a1 = (byte)((((a >> 4) & 0x0F) + paletteIndexCap) & 0xFF);

                        if ((tsatile & 0x0400) > 0)
                        {//横(Horizontal )反転する
                            if ((tsatile & 0x0800) > 0)
                            {//横(Horizontal )と 縦(Vertical )反転する
                                Marshal.WriteByte(adr, (x + (6 - x8) + 1) + bmpData.Stride * (y + (7 - y8)), a0);
                                Marshal.WriteByte(adr, (x + (6 - x8) + 0) + bmpData.Stride * (y + (7 - y8)), a1);
                            }
                            else
                            {//横(Horizontal )反転する
                                Marshal.WriteByte(adr, (x + (6 - x8) + 1) + bmpData.Stride * (y + y8), a0);
                                Marshal.WriteByte(adr, (x + (6 - x8) + 0) + bmpData.Stride * (y + y8), a1);
                            }
                        }
                        else
                        {
                            if ((tsatile & 0x0800) > 0)
                            {//縦(Vertical )反転する
                                Marshal.WriteByte(adr, (x + x8 + 0) + bmpData.Stride * (y + (7 - y8)), a0);
                                Marshal.WriteByte(adr, (x + x8 + 1) + bmpData.Stride * (y + (7 - y8)), a1);
                            }
                            else
                            {//なにもしないで書き込み
                                Marshal.WriteByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8), a0);
                                Marshal.WriteByte(adr, (x + x8 + 1) + bmpData.Stride * (y + y8), a1);
                            }
                        }

                        i++;
                        if (i >= length)
                        {
                            y8 = 0xf;
                            break;
                        }
                    }
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }

        //Tile + 16色 + TSA
        public static Bitmap ByteToImage16Tile(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, byte[] tsa, int tsa_pos, int palette_count=0)
        {
            //TSA読込
            UInt16[] tile = ByteToTSA(tsa, tsa_pos, width, height);
            return ByteToImage16TileInner(width, height, image, image_pos, palette, palette_pos, tile, palette_count);
        }
        //Tile + 16色 + ヘッダ付きTSA
        public static Bitmap ByteToImage16TileHeaderTSA(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, byte[] tsa, int tsa_pos)
        {
            //TSA読込
            UInt16[] tile = ByteToHeaderTSA(tsa, tsa_pos, width, height);
            return ByteToImage16TileInner(width, height, image, image_pos, palette, palette_pos, tile,0);
        }

        //256色
        public static Bitmap ByteToImage256Tile(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos)
        {
            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレットの読込.
            pic.Palette = ByteToPalette(pic.Palette, palette, palette_pos);

            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int length = image_pos + ((width * height) );
            length = Math.Min(length, image.Length);

            int x = 0;
            int y = 0;
            for (int i = image_pos; i < length; )
            {
                for (int y8 = 0; y8 < 8; y8++)
                {
                    for (int x8 = 0; x8 < 8; x8 ++)
                    {
                        byte a = image[i];

                        Marshal.WriteByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8), a);

                        i++;
                        if (i >= length)
                        {
                            pic.UnlockBits(bmpData);
                            return pic;
                        }
                    }
                }
                x += 8;

                if (x >= width)
                {
                    x = 0;
                    y += 8;
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }

        //Tile + 256色 + TSA
        public static Bitmap ByteToImage256Tile(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, byte[] tsa, int tsa_pos)
        {
            //TSA読込
            UInt16[] tile = ByteToTSA(tsa, tsa_pos, width, height);
            return ByteToImage256TileInner(width, height, image, image_pos, palette, palette_pos, tile);
        }
        //Tile + 256色 + ヘッダ付きTSA
        public static Bitmap ByteToImage256TileHeaderTSA(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, byte[] tsa, int tsa_pos)
        {
            //TSA読込
            UInt16[] tile = ByteToHeaderTSA(tsa, tsa_pos, width, height);
            return ByteToImage256TileInner(width, height, image, image_pos, palette, palette_pos, tile);
        }
        //Tile + 256色 + TSA
        public static Bitmap ByteToImage256TileInner(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos, UInt16[] tile)
        {
            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレットの読込.
            pic.Palette = ByteToPalette(pic.Palette, palette, palette_pos);

            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            //TSA参照がある場合、参照限界は、画像データそのもののサイズ.
            int length = image.Length;

            int x = 0;
            int y = 0;

            //処理を早くするため、8の倍数に補正する.
            width = (width / 8) * 8;
            height = (height / 8) * 8;

            //TSAで読み込む場合、ソース位置が変更になるらしい.
            int tilelength = tile.Length;
            for (int tsaindex = 0; tsaindex < tilelength; tsaindex++, x += 8)
            {
                if (x >= width)
                {
                    x = 0;
                    y += 8;
                    if (y >= height)
                    {
                        break;
                    }
                }
                UInt16 tsatile = tile[tsaindex];

                if (tsatile == 0xFFFF)
                {
                    continue;
                }
                //TSA see https://dw.ngmansion.xyz/doku.php?id=column:tsa
                UInt16 tileNumber = (UInt16)(tsatile & 0x03FF);

                //TSAのtileNumberの場所にソースデータがある.
                int i = image_pos + tileNumber * 32;
                if (i >= length)
                {
                    continue;
                }

                //パレット番号 パレットは16色が16個(合計16*16)ある.
                //パレット番号に挿げ替える.
                int paletteIndexCap = (((tsatile >> 12) & 0xF) * 16) ;

                for (int y8 = 0; y8 < 8; y8++)
                {
                    for (int x8 = 0; x8 < 8; x8 ++)
                    {
                        byte a = image[i];

                        if ((tsatile & 0x0400) > 0)
                        {//横(Horizontal )反転する
                            if ((tsatile & 0x0800) > 0)
                            {//横(Horizontal )と 縦(Vertical )反転する
                                Marshal.WriteByte(adr, (x + (7 - x8) + 0) + bmpData.Stride * (y + (7 - y8)), a);
                            }
                            else
                            {//横(Horizontal )反転する
                                Marshal.WriteByte(adr, (x + (7 - x8) + 0) + bmpData.Stride * (y + y8), a);
                            }
                        }
                        else
                        {
                            if ((tsatile & 0x0800) > 0)
                            {//縦(Vertical )反転する
                                Marshal.WriteByte(adr, (x + x8 + 0) + bmpData.Stride * (y + (7 - y8)), a);
                            }
                            else
                            {//なにもしないで書き込み
                                Marshal.WriteByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8), a);
                            }
                        }

                        i++;
                        if (i >= length)
                        {
                            pic.UnlockBits(bmpData);
                            return pic;
                        }
                    }
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }

        //Liner + 256色
        //FE6 のWMAP 082c8874(LZ77) とかで使われる、単純な256色形式
        public static Bitmap ByteToImage256Liner(int width, int height, byte[] image, int image_pos, byte[] palette, int palette_pos)
        {
            //BITMAP生成.
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //パレットの読込.
            pic.Palette = ByteToPalette(pic.Palette, palette, palette_pos);

            //BITMAP塗りつぶす.
            Rectangle rect = new Rectangle(new Point(), pic.Size);
            BitmapData bmpData = pic.LockBits(rect, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int length = image_pos + ((width * height) );
            length = Math.Min(length, image.Length);

            int x = 0;
            int y = 0;
            for (int i = image_pos; i < length; )
            {
                byte a = image[i];

                Marshal.WriteByte(adr, (x) + bmpData.Stride * (y), a);

                i++;
                x++;

                if (x >= width)
                {
                    x = 0;
                    y ++;
                }
            }
            pic.UnlockBits(bmpData);
            return pic;
        }
        
        
        //高さを計算する
        public static int CalcHeight(int width, int image_size,int align=8)
        {
            int height = image_size / (width / 2);
            if (image_size % (width / 2) != 0)
            {
                height++;
            }
            if (height % align != 0)
            {
                height += align;
            }
            return height / align * align;
        }
        //高さを計算する TSAを利用したバージョン.
        public static int CalcHeightbyTSA(int width, int tsa_size,int align=8)
        {
            width = width / 8;
            tsa_size = tsa_size / 2;

            int height = tsa_size / (width);
            if (tsa_size % (width) != 0)
            {
                height++;
            }
            if (height % align != 0)
            {
                height += align;
            }
            return height * 8 / align * align;
        }

        //bitblt
        public static void BitBlt(Bitmap destBitmap, int xdest, int ydest, int width, int height, Bitmap srcBitmap, int xsrc, int ysrc, int palette_count = 0, int transparent_index = 0xff, bool vflip = false, bool hflip = false)
        {
            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            BitmapData srcBmpData = srcBitmap.LockBits(srcrect, ImageLockMode.ReadWrite, srcBitmap.PixelFormat);
            IntPtr srcPtr = srcBmpData.Scan0;

            //パレットは16色のパレットが16個ある
            int paletteIndexCap = (palette_count * 16);

            if (ydest < 0)
            {
                ysrc += Math.Abs(ydest);
                height -= Math.Abs(ydest);
                ydest = 0;
            }
            if (ysrc < 0)
            {
                height -= Math.Abs(ysrc);
                ysrc = 0;
            }
            if (ysrc + height > srcrect.Height)
            {
                height -= (ysrc + height) - srcrect.Height;
            }
            if (ydest + height > destrect.Height)
            {
                height -= (ydest + height) - destrect.Height;
            }

            if (xdest < 0)
            {
                xsrc += Math.Abs(xdest);
                width -= Math.Abs(xdest);
                xdest = 0;
            }
            if (xsrc < 0)
            {
                width -= Math.Abs(xsrc);
                xsrc = 0;
            }
            if (xsrc + width > srcrect.Width)
            {
                width -= (xsrc + width) - srcrect.Width;
            }
            if (xdest + width > destrect.Width)
            {
                width -= (xdest + width) - destrect.Width;
            }

            if (height <= 0 || width <= 0)
            {//コピーの必要なし
                destBitmap.UnlockBits(destBmpData);
                srcBitmap.UnlockBits(srcBmpData);
                return;
            }

            if (vflip)
            {
                if (hflip)
                {
                    int ysrc_offset = srcBmpData.Stride * (ysrc + (height - 1 - 0));
                    int ydest_offset = destBmpData.Stride * ydest;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int src = xsrc + (width - 1 - x) + ysrc_offset;
                            int dest = xdest + x + ydest_offset;

                            byte a = Marshal.ReadByte(srcPtr, src);
                            if (a == transparent_index)
                            {
                                continue;
                            }
                            a = (byte)((a + paletteIndexCap) & 0xFF);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                        ysrc_offset -= srcBmpData.Stride;
                        ydest_offset += destBmpData.Stride;
                    }
                }
                else
                {
                    int ysrc_offset = srcBmpData.Stride * ysrc;
                    int ydest_offset = destBmpData.Stride * ydest;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int src = xsrc + (width - 1 - x) + ysrc_offset;
                            int dest = xdest + x + ydest_offset;

                            byte a = Marshal.ReadByte(srcPtr, src);
                            if (a == transparent_index)
                            {
                                continue;
                            }
                            a = (byte)((a + paletteIndexCap) & 0xFF);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                        ysrc_offset += srcBmpData.Stride;
                        ydest_offset += destBmpData.Stride;
                    }
                }
            }
            else
            {
                if (hflip)
                {
                    int ysrc_offset = srcBmpData.Stride * (ysrc + (height - 1 - 0));
                    int ydest_offset = destBmpData.Stride * ydest;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int src = xsrc + x + ysrc_offset;
                            int dest = xdest + x + ydest_offset;

                            byte a = Marshal.ReadByte(srcPtr, src);
                            if (a == transparent_index)
                            {
                                continue;
                            }
                            a = (byte)((a + paletteIndexCap) & 0xFF);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                        ysrc_offset -= srcBmpData.Stride;
                        ydest_offset += destBmpData.Stride;
                    }
                }
                else if (palette_count == 0 && transparent_index == 0xff)
                {//反転しない パレットも変更しない場合、最速チート転送をする
                    int ysrc_offset = srcBmpData.Stride * ysrc;
                    int ydest_offset = destBmpData.Stride * ydest;
                    if (width - 0 > 0)
                    {
                        uint size = (uint)(width - 0);
                        for (int y = 0; y < height; y++)
                        {
                            int src = xsrc + ysrc_offset;
                            int dest = xdest + ydest_offset;

                            U.CopyMemory(IntPtr.Add(destPtr, dest), IntPtr.Add(srcPtr, src), size);

                            ysrc_offset += srcBmpData.Stride;
                            ydest_offset += destBmpData.Stride;
                        }
                    }
                }
                else
                {
                    int ysrc_offset = srcBmpData.Stride * ysrc;
                    int ydest_offset = destBmpData.Stride * ydest;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int src = xsrc + x + ysrc_offset;
                            int dest = xdest + x + ydest_offset;

                            byte a = Marshal.ReadByte(srcPtr, src);
                            if (a == transparent_index)
                            {
                                continue;
                            }
                            a = (byte)((a + paletteIndexCap) & 0xFF);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                        ysrc_offset += srcBmpData.Stride;
                        ydest_offset += destBmpData.Stride;
                    }
                }
            }
            destBitmap.UnlockBits(destBmpData);
            srcBitmap.UnlockBits(srcBmpData);
        }
#if DEBUG
        public static void TEST_bitblt()
        {
            {//height よりマイナス
                Bitmap src = ImageUtil.OpenBitmap(Program.GetTestData("rotatetest.png"));
                Bitmap dest = ImageUtil.Blank(30, 30, src);
                BitBlt(dest, 0, -4, 2, 2, src, 0, 8);

                Bitmap test = ImageUtil.OpenBitmap(Program.GetTestData("bitblt_test5.png"));
                bool r = ImageUtil.CompareBitmap(dest, test);
                Debug.Assert(r == true);
            }
            {
                Bitmap src = ImageUtil.OpenBitmap(Program.GetTestData("rotatetest.png"));
                Bitmap dest = ImageUtil.Blank(30, 30, src);
                BitBlt(dest, 0, -1, 16, 16, src, 0, 9);

                Bitmap test = ImageUtil.OpenBitmap(Program.GetTestData("bitblt_test4.png"));
                bool r = ImageUtil.CompareBitmap(dest, test);
                Debug.Assert(r == true);
            }
            {
                Bitmap src = ImageUtil.OpenBitmap(Program.GetTestData("rotatetest.png"));
                Bitmap dest = ImageUtil.Blank(30, 30, src);
                BitBlt(dest, 0, -9, 16, 16, src, 0, 9);

                bool r = ImageUtil.CompareBitmap(dest, ImageUtil.OpenBitmap(Program.GetTestData("bitblt_test3.png")));
                Debug.Assert(r == true);
            }
            {
                Bitmap src = ImageUtil.OpenBitmap(Program.GetTestData("rotatetest.png"));
                Bitmap dest = ImageUtil.Blank(30, 30, src);
                BitBlt(dest, 0, 9, 16, 16, src, 0, 9);

                bool r = ImageUtil.CompareBitmap(dest, ImageUtil.OpenBitmap(Program.GetTestData("bitblt_test2.png")));
                Debug.Assert(r == true);
            }
            {
                Bitmap src = ImageUtil.OpenBitmap(Program.GetTestData("rotatetest.png"));
                Bitmap dest = ImageUtil.Blank(30, 30, src);
                BitBlt(dest, 0, 0, 16, 16, src, 0, 9);

                bool r = ImageUtil.CompareBitmap(dest, ImageUtil.OpenBitmap(Program.GetTestData("bitblt_test1.png")));
                Debug.Assert(r == true);
            }
            {
                Bitmap src = ImageUtil.OpenBitmap(Program.GetTestData("rotatetest.png"));
                Bitmap dest = ImageUtil.Blank(30,30,src);
                BitBlt(dest, 0, 0, 16, 16, src, 0, 0);

                bool r = ImageUtil.CompareBitmap(dest, ImageUtil.OpenBitmap(Program.GetTestData("bitblt_test0.png")));
                Debug.Assert(r == true);
            }
        }
#endif

        //bitblt srcとdestの反転
        public static void BitBltRev(Bitmap srcBitmap, int xsrc, int ysec, int width, int height, Bitmap destBitmap, int xdest, int ydest, int palette_count = 0, int transparent_index = 0xff, bool vflip = false, bool hflip = false)
        {
            BitBlt(destBitmap, xdest, ydest, width, height, srcBitmap, xsrc, ysec, palette_count, transparent_index,vflip,hflip);
        }

        //bitbltみたいに転送するが TSAを加味する.
        public static void BitBltTSA(Bitmap destBitmap, Bitmap srcBitmap, byte[] tsa, int tsa_pos, int palette_count = 0)
        {
            //TSA読込
            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            UInt16[] tile = ByteToTSA(tsa, tsa_pos, srcrect.Width, srcrect.Height);

            BitBltTSA(destBitmap, srcBitmap, tile, palette_count);
        }
        public static void BitBltTSA(Bitmap destBitmap, Bitmap srcBitmap, UInt16[] tile, int palette_count = 0)
        {
            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            BitmapData srcBmpData = srcBitmap.LockBits(srcrect, ImageLockMode.ReadWrite, srcBitmap.PixelFormat);
            IntPtr srcPtr = srcBmpData.Scan0;

            int xdest = 0;
            int ydest = 0;

            //TSAで読み込む場合、ソース位置が変更になるらしい.
            int tilelength = tile.Length;
            for (int tsaindex = 0; tsaindex < tilelength; tsaindex++, xdest += 8)
            {
                if (xdest >= destrect.Width)
                {
                    xdest = 0;
                    ydest += 8;
                }

                UInt16 tsatile = tile[tsaindex];

                if (tsatile == 0xFFFF)
                {
                    continue;
                }
                //TSA see https://dw.ngmansion.xyz/doku.php?id=column:tsa
                UInt16 tileNumber = (UInt16)(tsatile & 0x03FF);

                //TSAのtileNumberの場所にソースデータがある.
                int xsrc = tileNumber % (srcrect.Width / 8) * 8;
                int ysrc = tileNumber / (srcrect.Width / 8) * 8;
                //パレット番号 パレットは16色が16個(合計16*16)ある.
                //パレット番号に挿げ替える.
                int paletteIndexCap = (((tsatile >> 12) & 0xF) * 16) + (palette_count);

                if (xsrc + 8 > srcrect.Width)
                {
                    continue;
                }
                if (ysrc + 8 > srcrect.Height)
                {
                    continue;
                }
                if (xdest + 8 > destrect.Width)
                {
                    continue;
                }
                if (ydest + 8 > destrect.Height)
                {
                    continue;
                }

                for (int y8 = 0; y8 < 8; y8++)
                {
                    for (int x8 = 0; x8 < 8; x8++)
                    {
                        byte a;
                        if ((tsatile & 0x0400) > 0)
                        {//横(Horizontal )反転する
                            if ((tsatile & 0x0800) > 0)
                            {//横(Horizontal )と 縦(Vertical )反転する
                                a = Marshal.ReadByte(srcPtr, xsrc + (7 - x8) + srcBmpData.Stride * (ysrc + (7 - y8)));
                            }
                            else
                            {//横(Horizontal )反転する
                                a = Marshal.ReadByte(srcPtr, xsrc + (7 - x8) + srcBmpData.Stride * (ysrc + y8));
                            }
                        }
                        else
                        {
                            if ((tsatile & 0x0800) > 0)
                            {//縦(Vertical )反転する
                                a = Marshal.ReadByte(srcPtr, xsrc + x8 + srcBmpData.Stride * (ysrc + (7 - y8)));
                            }
                            else
                            {//なにもしないで書き込み
                                a = Marshal.ReadByte(srcPtr, xsrc + x8 + srcBmpData.Stride * (ysrc + y8));
                            }
                        }

                        int dest = xdest + x8 + destBmpData.Stride * (ydest + y8);
                        a = (byte)((a + paletteIndexCap) & 0xFF);
                        Marshal.WriteByte(destPtr, dest, a);
                    }
                }
            }
            destBitmap.UnlockBits(destBmpData);
            srcBitmap.UnlockBits(srcBmpData);
        }

        //x,y,w,h  (x,y,w,h) の範囲を 反転、左右逆転とコピーします。
        //(x,y,w,h)(x,y,w,h)
        public static Bitmap Copy4(Bitmap srcBitmap, int xsrc, int ysrc, int width, int height)
        {
            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            BitmapData srcBmpData = srcBitmap.LockBits(srcrect, ImageLockMode.ReadWrite, srcBitmap.PixelFormat);
            IntPtr srcPtr = srcBmpData.Scan0;

            Bitmap destBitmap = new Bitmap(width*2, height*2, PixelFormat.Format8bppIndexed);
            destBitmap.Palette = srcBitmap.Palette;
            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            int xwidth  = Math.Min(width + xsrc, srcrect.Width);
            int yheight = Math.Min(height + ysrc, srcrect.Height);

            //A B
            //C D

            //A
            int ydest = 0;
            for (int y = ysrc; y < yheight; y++, ydest++)
            {
                int xdest = 0;
                for (int x = xsrc; x < xwidth; x++, xdest++)
                {
                    int src = x + srcBmpData.Stride * (y);
                    int dest = xdest + destBmpData.Stride * (ydest);

                    byte a = Marshal.ReadByte(srcPtr, src);
                    Marshal.WriteByte(destPtr, dest, a);
                }
            }

            //B
            ydest = 0;
            for (int y = ysrc; y < yheight; y++, ydest++)
            {
                int xdest = xwidth - xsrc + width - 1;
                for (int x = xsrc; x < xwidth; x++, xdest--)
                {
                    int src = x + srcBmpData.Stride * (y);
                    int dest = xdest + destBmpData.Stride * (ydest);

                    byte a = Marshal.ReadByte(srcPtr, src);
                    Marshal.WriteByte(destPtr, dest, a);
                }
            }

            //C
            ydest = yheight - ysrc + height - 1;
            for (int y = ysrc; y < yheight; y++, ydest--)
            {
                int xdest = 0;
                for (int x = xsrc; x < xwidth; x++, xdest++)
                {
                    int src = x + srcBmpData.Stride * (y);
                    int dest = xdest + destBmpData.Stride * (ydest);

                    byte a = Marshal.ReadByte(srcPtr, src);
                    Marshal.WriteByte(destPtr, dest, a);
                }
            }

            //D
            ydest = yheight - ysrc + height - 1;
            for (int y = ysrc; y < yheight; y++, ydest--)
            {
                int xdest = xwidth - xsrc + width - 1;
                for (int x = xsrc; x < xwidth; x++, xdest--)
                {
                    int src = x + srcBmpData.Stride * (y);
                    int dest = xdest + destBmpData.Stride * (ydest);

                    byte a = Marshal.ReadByte(srcPtr, src);
                    Marshal.WriteByte(destPtr, dest, a);
                }
            }

            destBitmap.UnlockBits(destBmpData);
            srcBitmap.UnlockBits(srcBmpData);

            return destBitmap;
        }

        public static Bitmap Copy(Bitmap srcBitmap, int xsrc, int ysrc, int width, int height,bool vflip=false,bool hflip=false)
        {
            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            BitmapData srcBmpData = srcBitmap.LockBits(srcrect, ImageLockMode.ReadWrite, srcBitmap.PixelFormat);
            IntPtr srcPtr = srcBmpData.Scan0;

            Bitmap destBitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = srcBitmap.Palette;
            if (pal.Entries.Length > 0)
            {
                destBitmap.Palette = pal;
            }
            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            int xwidth = Math.Min(width + xsrc, srcrect.Width);
            int yheight = Math.Min(height + ysrc, srcrect.Height);

            if (vflip)
            {
                if (hflip)
                {//Y,X軸両方で反転.
                    int ydest = 0;
                    for (int y = yheight - 1; y >= ysrc; y--, ydest++)
                    {
                        int xdest = 0;
                        for (int x = xwidth - 1; x >= xsrc; x--, xdest++)
                        {
                            int src = x + srcBmpData.Stride * (y);
                            int dest = xdest + destBmpData.Stride * (ydest);

                            byte a = Marshal.ReadByte(srcPtr, src);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                    }
                }
                else
                {//X軸 縦方向に反転
                    int ydest = 0;
                    for (int y = ysrc; y < yheight; y++, ydest++)
                    {
                        int xdest = 0;
                        for (int x = xwidth - 1; x >= xsrc; x--, xdest++)
                        {
                            int src = x + srcBmpData.Stride * (y);
                            int dest = xdest + destBmpData.Stride * (ydest);

                            byte a = Marshal.ReadByte(srcPtr, src);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                    }
                }
            }
            else
            {
                if (hflip)
                {//Y軸 横方向に反転
                    int ydest = 0;
                    for (int y = yheight - 1; y >= ysrc; y--, ydest++)
                    {
                        int xdest = 0;
                        for (int x = xsrc; x < xwidth; x++, xdest++)
                        {
                            int src = x + srcBmpData.Stride * (y);
                            int dest = xdest + destBmpData.Stride * (ydest);

                            byte a = Marshal.ReadByte(srcPtr, src);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                    }
                }
                else
                {//flipしない.
                    int ydest = 0;
                    for (int y = ysrc; y < yheight; y++, ydest++)
                    {
                        int xdest = 0;
                        for (int x = xsrc; x < xwidth; x++, xdest++)
                        {
                            int src = x + srcBmpData.Stride * (y);
                            int dest = xdest + destBmpData.Stride * (ydest);

                            byte a = Marshal.ReadByte(srcPtr, src);
                            Marshal.WriteByte(destPtr, dest, a);
                        }
                    }
                }
            }
            destBitmap.UnlockBits(destBmpData);
            srcBitmap.UnlockBits(srcBmpData);

            return destBitmap;
        }

        public static Bitmap CopyByPalette(Bitmap srcBitmap,int copyPalette, int xsrc, int ysrc, int width, int height)
        {
            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            BitmapData srcBmpData = srcBitmap.LockBits(srcrect, ImageLockMode.ReadWrite, srcBitmap.PixelFormat);
            IntPtr srcPtr = srcBmpData.Scan0;

            Bitmap destBitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = srcBitmap.Palette;
            if (pal.Entries.Length > 0)
            {
                destBitmap.Palette = pal;
            }
            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            int xwidth = Math.Min(width + xsrc, srcrect.Width);
            int yheight = Math.Min(height + ysrc, srcrect.Height);

            int paletteBaseStart = copyPalette * 16;
            int paletteBaseEnd   = (copyPalette+1) * 16;

            int ydest = 0;
            for (int y = ysrc; y < yheight; y++, ydest++)
            {
                int xdest = 0;
                for (int x = xsrc; x < xwidth; x++, xdest++)
                {
                    int src = x + srcBmpData.Stride * (y);
                    int dest = xdest + destBmpData.Stride * (ydest);

                    byte a = Marshal.ReadByte(srcPtr, src);
                    if (a >= paletteBaseStart && a < paletteBaseEnd)
                    {
                        Marshal.WriteByte(destPtr, dest, (byte)(a - paletteBaseStart));
                    }
                }
            }
            destBitmap.UnlockBits(destBmpData);
            srcBitmap.UnlockBits(srcBmpData);

            return destBitmap;
        }

        //パレットの交換 指定した 16色のパレットを 0番目のパレットにします.
        public static Bitmap SwapPalette(Bitmap srcbitmap, int palette_index, int palette_size = 0x10)
        {
            Bitmap bitmap = ImageUtil.CloneBitmap(srcbitmap);
            if (palette_index <= 0)
            {
                return bitmap;
            }
            ColorPalette pal = bitmap.Palette;
            for (int i = 0; i < palette_size ; i++)
            {
                Color old = pal.Entries[i];
                pal.Entries[i] = pal.Entries[palette_index*0x10 + i];
                pal.Entries[palette_index*0x10 + i] = old;
            }
            bitmap.Palette = pal;
            return bitmap;
        }
        //パレットの交換と同時に、色データも交換します.
        public static Bitmap SwapPaletteAndImage(Bitmap srcbitmap, int palette_index)
        {
            Bitmap a = SwapPalette(srcbitmap, palette_index);
            BitBlt(a, 0, 0, a.Width, a.Height, srcbitmap, 0, 0, -palette_index, 0);
            
            return a;
        }

        public static int FindPalette(Bitmap srcbitmap, Bitmap newImage)
        {
            int palette_count = GetPalette16Count(srcbitmap);

            ColorPalette newImagePal = newImage.Palette;
            ColorPalette pal = srcbitmap.Palette;

            for (int u = 0 ; u < palette_count ;u++ )
            {
                int i = 0;
                for (i = 0; i < 0x10; i++)
                {
                    if (newImagePal.Entries[i] == pal.Entries[i + (u * 0x10)])
                    {
                        continue;
                    }
                    break;
                }
                if (i >= 0x10)
                {
                    return u;
                }
            }
            return -1;
        }

        public static int AppendPalette(Bitmap srcbitmap, Bitmap newImage, int overraide_palette_index = 0xff)
        {
            int newImage_palette_count = GetPalette16Count(newImage);
            int palette_count = overraide_palette_index;
            if (palette_count >= 0xF)
            {
                palette_count = GetPalette16Count(srcbitmap);
            }
            if (newImage_palette_count + palette_count >= 0x10)
            {
                throw new Exception("palette size overflow!");
            }

            ColorPalette newImagePal = newImage.Palette;
            ColorPalette pal = srcbitmap.Palette;
            for (int n = 0; n < newImage_palette_count; n++)
            {
                for (int i = 0; i < 0x10; i++)
                {
                    pal.Entries[i + ((n + palette_count) * 0x10)] = newImagePal.Entries[i + (n * 0x10)];
                }
            }
            srcbitmap.Palette = pal;
            return palette_count;
        }

        //FONT用 4色のインポート 16x16
        public static byte[] Image4ToByte(Bitmap bitmap)
        {
            byte[] data = new byte[16/4*16];

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int nn = 0;
            for (int y = 0; y < 16; y++)
            {
                int n = 0;
                byte one = 0;
                for (int x = 0; x < 16; x++)
                {
                    byte a = Marshal.ReadByte(adr, (x + 0) + bmpData.Stride * y);
                    one = (byte)(one | ((a & 0x03) << (n * 2)));
                    n++;
                    if (n >= 4)
                    {
                        data[nn++] = one;
                        n = 0;
                        one = 0;
                    }
                }
            }
            bitmap.UnlockBits(bmpData);
            return data;
        }

        public static uint ColorToGBARGB(Color c)
        {
            return 
                (uint)((c.R >> 3) & 0x1F)
                + (((uint)(c.G >> 3) & 0x1F) << 5)
                + (((uint)(c.B >> 3) & 0x1F) << 10)
                ;
        }
        public static Color GBARGBToColor(uint  c)
        {
            uint r = c & 0x1F;
            uint g = (c >> 5 )& 0x1F;
            uint b = (c >> 10 )& 0x1F;
            return Color.FromArgb((int)r << 3, (int)g << 3, (int)b << 3);
        }

        //パレットからバイト列.
        public static byte[] ImageToPalette(Bitmap bitmap,int palette_count = 1)
        {
            byte[] newpalette = new byte[2 * 16 * palette_count];

            ColorPalette palette = bitmap.Palette;
            int nn = 0;
            for (int i = 0; i < palette_count; i++)
            {
                for (int n = 0; n < 16; n++)
                {
                    Color c;
                    if (nn < palette.Entries.Length)
                    {
                        c = palette.Entries[nn];
                    }
                    else
                    {
                        //Debug.Assert(false); //パレットの方が先になくなるのはありえない...
                        c = Color.Black;
                    }
                    uint p = ColorToGBARGB(c);
                    U.write_u16(newpalette, (uint)nn * 2, p);
                    nn++;
                }
            }
            return newpalette;
        }

        //Tile + 16色
        //タイル形式は、 8x8ピクセルごとに分断されている.
        //0123 0123 
        //4567 4567 
        //89ab 89ab
        //cdef cdef
        //0123 0123
        //4567 4567
        //89ab 89ab
        //cdef cdef
        public static byte[] ImageToByte16Tile(Bitmap bitmap,int width, int height)
        {
            byte[] data = new byte[width/2*height];

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int nn = 0;
            for (int y = 0; y < height; y+=8)
            {
                for (int x = 0; x < width; x+=8)
                {
                    for (int y8 = 0; y8 < 8; y8++)
                    {
                        for (int x8 = 0; x8 < 8; x8 += 2)
                        {
                            byte a = Marshal.ReadByte(adr, (x + x8 + 0) + bmpData.Stride * (y+y8));
                            byte b = Marshal.ReadByte(adr, (x + x8 + 1) + bmpData.Stride * (y + y8));

                            if (nn >= data.Length)
                            {
                                break;
                            }
                            data[nn] = (byte)((a&0xF) + ((b&0xF)<<4));
                            nn++;
                        }
                    }
                }
            }

            bitmap.UnlockBits(bmpData);
            return data;    
        }

        //non-Pack TSA 同一タイルをすし詰めにしない プレーンなTSAデータの作成.
        public static byte[] ImageToBytePlainTSA(Bitmap bitmap, int width, int height,out string out_error)
        {
            byte[] data = new byte[width / 4 * height / 8];

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int nn = 0;
            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    uint palette = 255;
                    for (int y8 = 0; y8 < 8; y8++)
                    {
                        for (int x8 = 0; x8 < 8; x8 ++)
                        {
                            byte a = Marshal.ReadByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8));
                            uint selectpalette = (uint)(a / 16);
                            if (palette == 255)
                            {//初期値
                                palette = selectpalette;
                            }
                            else if (palette != selectpalette)
                            {//フォーマット違反 8x8セルの中で異なるパレットを使用してはいけない.
                                bitmap.UnlockBits(bmpData);
                                out_error = R.Error("TSAフォーマット違反。X:{0} Y:{1} から8x8の範囲である X:{2} Y:{3}で、異なるパレット番号{4}が使われました。他はパレット番号{5}を使っています。\r\n\r\n手動で問題を修正するか、\r\n「減色ツール」を利用して変換してください。", x, y, x8 + x, y8 + y, selectpalette, palette);
                                return null;
                            }
                        }
                    }

                    if (palette == 255)
                    {
                        palette = 0;
                    }
                    U.write_u16(data,(uint)nn*2,( ((uint)nn) & 0x3FF) | ((palette & 0xF) << 12));
                    nn++;
                }
            }

            bitmap.UnlockBits(bmpData);
            out_error = "";
            return data;
        }


        //ヘッダー付きTSAに変換する
        public static byte[] TSAToHeaderTSA(byte[] tsa, int width, int height, int tsa_width_margin = 2)
        {
            int master_headerx = (width / 8) - tsa_width_margin - 1;
            int master_headery = (height / 8) - 1;

            int size = 2 + (master_headerx + 1) * (master_headery + 1) * 2;
            byte[] newtsa = new byte[size];

            //最初の2バイトにヘッダがある.
            newtsa[0] = (byte)master_headerx;
            newtsa[1] = (byte)master_headery;
            int i = 0;

            //後ろから、補正値を入れて足しこむらしい(こんかいは作る側なので逆に引く)
            int n = 0x2 + ((master_headery * (master_headerx+1)) * 2); //0x474;
            if (n > size)
            {//エラー
                return newtsa;
            }

            for (int headery = 0; headery <= master_headery; headery++)
            {
                for (int headerx = 0; headerx <= master_headerx; headerx++)
                {
                    if (i >= tsa.Length)
                    {
                        break;
                    }
                    UInt16 tsadata = (UInt16)U.u16(tsa, (uint)i);
                    UInt16 newtsadata = (UInt16)(tsadata);
                    U.write_u16(newtsa, (uint)n, newtsadata);
                    i += 2;
                    n += 2;
                }
                i = i + tsa_width_margin * 2;
                n = n + tsa_width_margin * 2;
                n = n - ((master_headerx) * 2);
                n = n - (0x42);
            }
            return newtsa;
        }


        public static int GetPalette16Count(Bitmap bitmap, int width, int height)
        {
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            uint maxPalette = 0;

            width = width / 8 * 8;
            height = height / 8 * 8;
            
            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y8 = 0; y8 < 8; y8++)
                    {
                        for (int x8 = 0; x8 < 8; x8++)
                        {
                            byte a = Marshal.ReadByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8));
                            uint selectpalette = (uint)(a / 16);
                            if (selectpalette > maxPalette)
                            {
                                maxPalette = selectpalette;
                            }
                        }
                    }
                }
            }

            bitmap.UnlockBits(bmpData);
            return (int)maxPalette+1;
        }
        public static int GetPalette16Count(Bitmap bitmap)
        {
            return GetPalette16Count(bitmap, bitmap.Width, bitmap.Height);
        }

        //TSAがある場合、利用しているパレット数は、TSAをスキャンしてみないとわからない.
        public static int GetPalette16CountForTSA(byte[] tsa,uint start , uint length)
        {
            length = Math.Min( (uint)tsa.Length,length);

            int maxPaletteNumber = 0;
            uint p = start + 1;
            for (int i = 0 ; i < length; i++ , p += 2)
            {
                if (p >= length)
                {
                    break;
                }
                int pal = ((tsa[p] & 0xF0) >> 4);
                if (pal > maxPaletteNumber)
                {
                    maxPaletteNumber = pal;
                }
            }
            return maxPaletteNumber + 1;
        }

        public static int GetColorCount(Bitmap bitmap)
        {
            Rectangle destrect = new Rectangle(new Point(), bitmap.Size);
            List<Color> entries = new List<Color>();

            int width = bitmap.Width;
            int height = bitmap.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color cc = bitmap.GetPixel(x, y);

                    //どうせGBAパレットで下3ビットは消えるので見ない.
                    Color c = Color.FromArgb(
                          (cc.R >> 3) << 3
                        , (cc.G >> 3) << 3
                        , (cc.B >> 3) << 3
                        );

                    int palettecount = entries.Count;
                    int paletteno = 0;
                    if (cc.A > 0)
                    {//透明じゃないので色があるはず.
                        for (; paletteno < palettecount; paletteno++)
                        {
                            if (entries[paletteno] == c)
                            {//既存のパレットに色があった
                                break;
                            }
                        }
                    }

                    if (paletteno >= palettecount)
                    {//未知の色なのでパレットに追加.
                        entries.Add(c);
                    }
                }
            }

            return entries.Count;
        }
 

        static byte[] TileVFlip(byte[] tile)
        {//縦方向に反転
            byte[] ret = new byte[8 / 2 * 8];
            int ydest = 0;
            for (int y = 0; y < 8; y++, ydest++)
            {
                int xdest = 0;
                for (int x = 8 / 2 - 1; x >= 0; x--, xdest++)
                {
                    int src = x + (8 / 2) * (y);
                    int dest = xdest + (8 / 2) * (ydest);

                    byte a = tile[src];
                    ret[dest] =  (byte)( ((a & 0xf0) >> 4) | ((a & 0x0f) << 4));
                }
            }
            return ret;
        }
        static byte[] TileHFlip(byte[] tile)
        {//横方向に反転
            byte[] ret = new byte[8 / 2 * 8];
            int ydest = 0;
            for (int y = 8 - 1; y >= 0; y--, ydest++)
            {
                int xdest = 0;
                for (int x = 0; x < 8/2; x++, xdest++)
                {
                    int src = x + (8 / 2) * (y);
                    int dest = xdest + (8 / 2) * (ydest);

                    byte a = tile[src];
                    ret[dest] = a;
                }
            }
            return ret;
        }
        static byte[] TileVHFlip(byte[] tile)
        {//縦横方向に反転
            byte[] ret = new byte[8 / 2 * 8];
            int ydest = 0;
            for (int y = 8 - 1; y >= 0; y--, ydest++)
            {
                int xdest = 0;
                for (int x = 8 / 2 - 1; x >= 0; x--, xdest++)
                {
                    int src = x + (8 / 2) * (y);
                    int dest = xdest + (8 / 2) * (ydest);

                    byte a = tile[src];
                    ret[dest] = (byte) ( ((a & 0xf0) >> 4) | ((a & 0x0f) << 4) );
                }
            }
            return ret;
        }

        public static string ImageToByteHeaderPackedTSA(
            Bitmap bitmap
            ,int width
            ,int height
            ,out byte[] out_image,out byte[] out_tsa
            ,int tsa_width_margin = 2
            ,bool isPackedImage = true
            )
        {
            byte[] tsa;
            if (isPackedImage)
            {
                string error = ImageToBytePackedTSA(bitmap, width, height,tsa_width_margin, out out_image, out tsa);
                if (error != "")
                {
                    out_tsa = null;
                    return error;
                }
            }
            else
            {
                string error;
                out_image = ImageToByte16Tile(bitmap, width, height);
                tsa = ImageToBytePlainTSA(bitmap, width, height, out error);
                out_tsa = tsa;
                if (tsa == null)
                {
                    return error;
                }
            }

            //HEADER付きTSAを作成する.
            out_tsa = TSAToHeaderTSA(tsa, width, height, tsa_width_margin);

            return "";
        }


        //TSAを維持して変換する.
        public static byte[] ImageToByteKeepTSA(Bitmap bitmap, int width, int height, ushort[] orignal_tsa, byte[] orignal_image)
        {
            Debug.Assert(width % 8 == 0);
            Debug.Assert(height % 8 == 0);
            Debug.Assert(orignal_tsa.Length == (width / 8) * (height / 8));
            byte[] image = ImageToByte16Tile(bitmap, width, height);
            byte[] dest = (byte[])orignal_image.Clone();

            for (int tsaindex = 0; tsaindex < orignal_tsa.Length; tsaindex++)
            {
                UInt16 tsatile = orignal_tsa[tsaindex];

                if (tsatile == 0xFFFF)
                {
                    continue;
                }
                //TSA see https://dw.ngmansion.xyz/doku.php?id=column:tsa
                UInt16 tileNumber = (UInt16)(tsatile & 0x03FF);

                //TSAのtileNumberの場所にソースデータがある.
                int i = tileNumber * 32;
                if (i + 32 > dest.Length)
                {
                    continue;
                }
                if (tsaindex*32 + 32 > image.Length)
                {
                    continue;
                }

                if ( (tsatile & 0x0C00) == 0x0C00)
                {
                    byte[] tile = U.getBinaryData(image, (uint)tsaindex * 32, 32);
                    byte[] vhfliptile = TileVHFlip(tile);
                    Array.Copy(vhfliptile, 0, dest, i , 32);
                }
                else if ( (tsatile & 0x0400) == 0x0400)
                {
                    byte[] tile = U.getBinaryData(image, (uint)tsaindex * 32, 32);
                    byte[] vfliptile = TileVFlip(tile);
                    Array.Copy(vfliptile, 0, dest, i , 32);
                }
                else if ( (tsatile & 0x0800) == 0x0800)
                {
                    byte[] tile = U.getBinaryData(image, (uint)tsaindex * 32, 32);
                    byte[] hfliptile = TileHFlip(tile);
                    Array.Copy(hfliptile, 0, dest, i , 32);
                }
                else
                {
                    Array.Copy(image, tsaindex * 32, dest, i, 32);
                }

                //パレット番号 パレットは16色が16個(合計16*16)ある.
                //パレット番号に挿げ替える.
//                int paletteIndexCap = (((tsatile >> 12) & 0xF) * 16) ;
            }
            return dest;
        }
        //右端のブロックかどうか判定
        static bool IsMargineData(int tile,int width , int tsa_width_margin)
        {
            if (tsa_width_margin <= 0)
            {
                return false;
            }
            int width_tile = width / 8;
            int index = tile % width_tile;
            if (index >= width_tile - tsa_width_margin)
            {
                return true;
            }
            return false;
        }

        //8x8ごとのブロックで評価して、同一ブロックはコピーしないでTSAデータを作ることで補う.
        public static string ImageToBytePackedTSA(Bitmap bitmap, int width, int height,int tsa_width_margin,out byte[] out_image,out byte[] out_tsa)
        {
            out_image = null;
            out_tsa = null;
            string error = "";

            const int BLOCK_SIZE = 8 * 8 / 2;
            byte[] image = ImageToByte16Tile(bitmap, width, height);
            byte[] tsa = ImageToBytePlainTSA(bitmap, width, height, out error);
            if (tsa == null)
            {
                return error;
            }

            //8x8ごとのブロックで評価して、同一ブロックはコピーしないでTSAデータを作ることで補う.
            byte[] packedimage = new byte[image.Length];
            uint packedimage_pos = 0;
            uint tsaindex = 0;

            if (tsa_width_margin == 0xff)
            {//絶対に先方にullを作らない(マップチップ用)
                tsa_width_margin = 0;
            }
            else if (tsa_width_margin >= 1)
            {//header付きTSAの場合、右側に余白ができる. 余白部分を最初に格納するのが一番問題が少ないようだ
                packedimage_pos += BLOCK_SIZE;
            }
            else if (width >= 230)
            {//ヘッダーがなかったとしても、大きな画像で、NULLブロックがあるなら先頭に配置する.
                byte[] tile = U.FillArray(BLOCK_SIZE,0 );
                uint found = U.Grep(image, tile, 0, (uint)packedimage.Length, BLOCK_SIZE);
                if (found != U.NOT_FOUND)
                {
                    packedimage_pos += BLOCK_SIZE;
                }
            }

            for (int nn = 0; nn < image.Length; nn += BLOCK_SIZE, tsaindex += 2)
            {
                if (IsMargineData(nn / BLOCK_SIZE, width, tsa_width_margin))
                {//余白 常に0にする. 初期値が0なので書き込み必要はない.
                   continue;
                }

                byte[] tile = U.getBinaryData(image, (uint)nn, BLOCK_SIZE);
//                Log.Debug("T",U.HexDump(tile));

                uint filpflag = 0x0;
                uint found = U.Grep(packedimage, tile, 0, packedimage_pos, BLOCK_SIZE);
                if (found == U.NOT_FOUND)
                {//ない 反転させながら探索
                    filpflag = 0x0400;

                    byte[] vfliptile = TileVFlip(tile);
//                        Log.Debug("V", U.HexDump(vfliptile));
                    found = U.Grep(packedimage, vfliptile, 0, packedimage_pos, BLOCK_SIZE);
                    if (found == U.NOT_FOUND)
                    {//ない
                        filpflag = 0x0800;

                        byte[] hfliptile = TileHFlip(tile);
//                            Log.Debug("H", U.HexDump(hfliptile));
                        found = U.Grep(packedimage, hfliptile, 0, packedimage_pos, BLOCK_SIZE);
                        if (found == U.NOT_FOUND)
                        {//ない
                            filpflag = 0x0C00;

                            byte[] vhfliptile = TileVHFlip(tile);
//                                Log.Debug("X", U.HexDump(vhfliptile));
                            found = U.Grep(packedimage, vhfliptile, 0, packedimage_pos, BLOCK_SIZE);
                        }
                    }
                }

                if (found == U.NOT_FOUND)
                {//ない... ので、データとして追加する
                    Array.Copy(tile, 0, packedimage, (int)packedimage_pos, BLOCK_SIZE);

                    //歯抜けデータができるので、TSAのtilenumber位置を再計算する
                    uint tilenumber = packedimage_pos / BLOCK_SIZE;
                    uint a = U.u16(tsa, tsaindex);
                    uint b = (a & 0xFC00) | (tilenumber & 0x3FF) ;
                    U.write_u16(tsa, tsaindex, b);

                    packedimage_pos += BLOCK_SIZE;
                }
                else
                {//ある... ので、TSAの tilenumberとしてのみ表現する.
                    uint tilenumber = found / BLOCK_SIZE;
                    uint a = U.u16(tsa, tsaindex);
                    uint b = (a & 0xF000) | (tilenumber & 0x3FF) | filpflag;
                    U.write_u16(tsa, tsaindex, b);
                }
            }
            out_image = U.subrange(packedimage, 0, packedimage_pos);
            out_tsa = tsa;

            return "";
        }
#if DEBUG
        //8x8ごとのブロックで評価して、同一ブロックはコピーしないでTSAデータを作ることで補う.
        public static void TEST_ImageToBytePackedTSA()
        {
            {
                Bitmap a = OpenBitmap(Program.GetTestData("tsa_pack_test.png"));
                Bitmap ans = OpenBitmap(Program.GetTestData("tsa_pack_test_ans.png"));
                byte[] ans_image = ImageToByte16Tile(ans,16,8);
                byte[] pal = ImageToPalette(ans);


                byte[] image;
                byte[] tsa;
                string error = ImageToBytePackedTSA(a, 48, 8, 0, out image, out tsa);


//                Bitmap test = ByteToImage16Tile(48, 8, image, 0, pal,0);
//                test.Save("test.png");

                Debug.Assert(error == "");
                Debug.Assert(image.Length == (8*8/2)*2 );
                Debug.Assert(U.memcmp(image,ans_image) == 0);
            }
        }
#endif
        //8x8ごとのブロックで評価して、同一ブロックはコピーしないでTSAデータを作ることで補う.
        public static string ImageToByteNonPackedTSA(Bitmap bitmap, int width, int height, out byte[] out_image, out byte[] out_tsa)
        {
            out_image = null;
            out_tsa = null;
            string error = "";

            byte[] image = ImageToByte16Tile(bitmap, width, height);
            byte[] tsa = ImageToBytePlainTSA(bitmap, width, height, out error);
            if (tsa == null)
            {
                return error;
            }

            out_image = image;
            out_tsa = tsa;

            return "";
        }
        //パレットマップの作成
        public static string ImageToPaletteMap(Bitmap bitmap,int width,int height,int palette_count,out byte[] out_plette_map)
        {
            out_plette_map = null;
            byte[] palettemap = new byte[(width / 2 + 4) * height];

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int nn = 0;
            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    uint palette = 255;
                    for (int y8 = 0; y8 < 8; y8++)
                    {
                        for (int x8 = 0; x8 < 8; x8 ++)
                        {
                            byte a = Marshal.ReadByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8));
                            uint selectpalette = (uint)(a / 16);
                            if (palette == 255)
                            {//初期値
                                palette = selectpalette;
                            }
                            else if (palette != selectpalette)
                            {//フォーマット違反 8x8セルの中で異なるパレットを使用してはいけない.
                                bitmap.UnlockBits(bmpData);
                                return R.Error("TSAフォーマット違反。X:{0} Y:{1} から8x8の範囲である X:{2} Y:{3}で、異なるパレット番号{4}が使われました。他はパレット番号{5}を使っています。\r\n\r\n手動で問題を修正するか、\r\n「減色ツール」を利用して変換してください。", x, y, x8 + x, y8 + y, selectpalette, palette);
                            }
                        }
                    }

                    if (palette == 255)
                    {
                        palette = 0;
                    }

                    //X = パレット
                    //Y = パレット
                    //0xXY 0xXY .... 0xXY となります。
                    //
                    //Xは偶数時パレット
                    //Yは奇数時パレット
                    if ((nn & 0x01) > 0)
                    {//奇数
                        byte a = palettemap[(nn / 2)];
                        byte b = (byte)((a & 0x0F) | ((byte)palette & 0xF) << 4);
                        palettemap[(nn / 2)] = b;
                    }
                    else
                    {//偶数
                        palettemap[(nn / 2)] = (byte)((palette & 0xF) );
                    }
                    nn++;
                }
                //パレットマップにもHEADETSAみたいに余白がいるらしい..?
                nn += 4;
            }

            bitmap.UnlockBits(bmpData);
            out_plette_map = palettemap;
            return "";
        }

        //何も描画されていないビットマップかどうか判別する.
        public static bool IsBlankBitmap(Bitmap bitmap ,int emptySize = 0)
        {
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int dotCount = 0;

            int bitmapsize = (rect.Width * rect.Height);
            for(int i = 0; i < bitmapsize ; i ++)
            {
                byte a = Marshal.ReadByte(adr, i);
                if (a > 0)
                {
                    dotCount++;
                }
            }

            bitmap.UnlockBits(bmpData);
            return (dotCount <= emptySize) ;
        }

        public static void ChangeColorPixel(Bitmap bitmap, byte src, byte dest,bool withPaletteSwap = true)
        {
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int bitmapsize = (rect.Width * rect.Height);
            for (int i = 0; i < bitmapsize; i ++)
            {
                byte a = Marshal.ReadByte(adr, i);
                if (a == src)
                {
                    Marshal.WriteByte(adr,i, dest);
                }
                else if (a == dest)
                {
                    Marshal.WriteByte(adr, i, src);
                }
            }

            bitmap.UnlockBits(bmpData);

            if (withPaletteSwap)
            {
                ColorPalette pal = bitmap.Palette;
                U.Swap<Color>(ref pal.Entries[src], ref pal.Entries[dest]);
                bitmap.Palette = pal;
            }
        }

        //タイル単位でスキャンして、データがない所には true , データがあるところには false のマークを立てた地図を作成します.
        public static bool[] MakeUseTileData(Bitmap bitmap)
        {   
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            //画像で利用しているタイルをマークするバッファを作成します.
            bool[] useTileData = new bool[(rect.Width / 8) * (rect.Height / 8)];

            //ないことの証明は難しいので、データがあるところにフラグをたてます
            int widthtile = rect.Width / 8;
            int bitmapsize = (rect.Width * rect.Height);
            for(int i = 0; i < bitmapsize ; i+= 8 )
            {
                int xtile = i % rect.Width / 8;
                int ytile = i / rect.Width / 8;
                int tileindex = xtile + (ytile*widthtile);
                for(int x = 0 ; x < 8 ; x ++)
                {
                    byte a = Marshal.ReadByte(adr, i+x);
                    if (a > 0 )
                    {//データがあった
                        useTileData[tileindex] = true;
                        break;
                    }
                }
            }
            //データがないところを知りたいので、フラグの反転
            for(int i = 0; i < useTileData.Length ; i++ )
            {
                useTileData[i] = !useTileData[i];
            }
 
            //右上にはカラーパレットマップ(FEditorAdv準拠)があるので無視するため、空白フラグを立てる.
            useTileData[(rect.Width / 8) - 1] = true;

            bitmap.UnlockBits(bmpData);
            return useTileData;
        }
                

        //指定された画像がかかれている位置を返す
        //ただし、比較は8x8単位で行う.
        public static bool GrepTileBitmap(Bitmap bitmap,Bitmap need,out Point out_point)
        {
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            Rectangle needrect = new Rectangle(new Point(), need.Size);
            BitmapData needbmpData = need.LockBits(needrect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr needadr = needbmpData.Scan0;

            int width = rect.Width - needrect.Width;
            int height = rect.Height - needrect.Height;
            if (width < 0 || height < 0)
            {
                bitmap.UnlockBits(bmpData);
                need.UnlockBits(needbmpData);
                out_point = new Point();
                return false;
            }
            uint needwidth = (uint)needrect.Width;

            for (int y = 0; y < height; y+=8)
            {
                for (int x = 0; x < width; x+=8)
                {
                    int needy;
                    for (needy = 0; needy < needrect.Height; needy ++)
                    {
                        IntPtr addrbase = IntPtr.Add(adr, x + bmpData.Stride * (y+needy));
                        IntPtr needbase = IntPtr.Add(needadr, needbmpData.Stride * (needy));

                        if (U.memcmp(addrbase, needbase, (IntPtr)needwidth) != 0)
                        {
                            break;
                        }
                    }

                    if (needy >= needrect.Height)
                    {
                        bitmap.UnlockBits(bmpData);
                        need.UnlockBits(needbmpData);

                        out_point = new Point(x,y);

                        return true;
                    }
                }
            }

            bitmap.UnlockBits(bmpData);
            need.UnlockBits(needbmpData);

            out_point = new Point();
            return false;
        }

#if DEBUG
        static void TEST_GrepTileBitmap()
        {
            {
                Bitmap a = OpenBitmap(Program.GetTestData("TEST_GrepTileBitmap3.png"));
                Bitmap need = OpenBitmap(Program.GetTestData("TEST_GrepTileBitmap3Need.png"));

                Point xy;
                bool r = ImageUtil.GrepTileBitmap(a, need, out xy);
                Debug.Assert(r == false);
            }
            {
                Bitmap a = OpenBitmap(Program.GetTestData("TEST_GrepTileBitmap.png"));
                Bitmap need = OpenBitmap(Program.GetTestData("TEST_GrepTileBitmapNeed.png"));

                Point xy;
                bool r = ImageUtil.GrepTileBitmap(a, need, out xy);
                Debug.Assert(r == true);
                Debug.Assert(xy.X == 40);
                Debug.Assert(xy.Y == 0);
            }
            {
                Bitmap a = OpenBitmap(Program.GetTestData("TEST_GrepTileBitmap2.png"));
                Bitmap need = OpenBitmap(Program.GetTestData("TEST_GrepTileBitmapNeed.png"));

                Point xy;
                bool r = ImageUtil.GrepTileBitmap(a, need, out xy);
                Debug.Assert(r == true);
                Debug.Assert(xy.X == 16);
                Debug.Assert(xy.Y == 8);
            }
        }
#endif
        public static bool CompareBitmap(Bitmap img1, Bitmap img2)
        {
            byte[] byte1;
            using (MemoryStream stream = new MemoryStream())
            {
                img1.Save(stream, ImageFormat.Png);
                byte1 = stream.ToArray();
            }
            byte[] byte2;
            using (MemoryStream stream = new MemoryStream())
            {
                img2.Save(stream, ImageFormat.Png);
                byte2 = stream.ToArray();
            }
            
            return U.memcmp(byte1,byte2) == 0;
        }

        //使っていないパレットをブラックアウト
        public static void BlackOutUnnecessaryColors(Bitmap b, int paletterows = 1)
        {
            ColorPalette pal = b.Palette;
            for (int i = paletterows * 16; i < 256; i++)
            {
                pal.Entries[i] = Color.Black;
            }
            b.Palette = pal;
        }


        //FE Editor用に利用しているパレット一覧を右上に描画する.
        public static void AppendPaletteMark(Bitmap bitmap, int paletteCount = 1)
        {
            Debug.Assert(bitmap.Width >= 8);

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            byte nn = 0;
            int maxY = paletteCount * 2;
            for (int y = 0; y < maxY; y++)
            {
                for (int x = rect.Width - 1; x >= rect.Width - 8; x --)
                {
                    Marshal.WriteByte(adr, (x) + bmpData.Stride * (y),nn);
                    nn++;
                }
            }

            bitmap.UnlockBits(bmpData);
        }

        //FEitrorの画像かどうか 右上にパレットマップがあるかどうか
        static bool IsFeditorImage(Bitmap bitmap,int paletteCount)
        {
            Debug.Assert(paletteCount >= 1);

            if (bitmap.Height < 80)
            {
                if (bitmap.Height != 64)
                {//シリアライズされたアニメシートの高さが64.
                    return false;
                }
            }
            if (bitmap.Width < 8)
            {
                return false;
            }


            Bitmap retbitmap = ImageUtil.Blank(bitmap.Width, bitmap.Height);
            Rectangle destrect = new Rectangle(new Point(), bitmap.Size);

            //16bit 24bit color 
            //FEditor のパレットマークがあるかどうか調べる
            ColorPalette newpal = retbitmap.Palette;
            int maxY = paletteCount * 2;
            for (int y = 0; y < maxY; y++)
            {
                for (int i = 0; i < 8; i++)
                {
                    newpal.Entries[i + (8 * y)] = bitmap.GetPixel(destrect.Width - 1 - i, y);
                }

            }
            //全部同じ色だったら、ただの真っ黒画像
            for (int palette = 0; palette < paletteCount; palette++)
            {
                int palShift = palette * 16;
                for (int i = 0; i < 16; i++)
                {
                    int n;
                    for (n = 0; n < 16; n++)
                    {
                        if (newpal.Entries[i + palShift] != newpal.Entries[n + palShift])
                        {
                            break;
                        }
                    }
                    if (n >= 16)
                    {//全部同じ色.
                        return false;
                    }
                }
            }

            //パレットマークの下は 0x00 色で埋まっているはず.
            int heightMax = Math.Min(bitmap.Height, 80);
            for (int y = maxY; y < heightMax; y++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (newpal.Entries[0] != bitmap.GetPixel(destrect.Width - 1 - i, y))
                    {
                        return false;
                    }
                }
            }

            //たぶん、FEditorの画像だろう.
            return true;
        }

        public static Bitmap LoadAndCheckPaletteUI(Control self, Bitmap paletteHint, int width, int height)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(self);
            if (bitmap == null)
            {
                return null;
            }

            if (width != 0)
            {
                if (bitmap.Width != width)
                {
                    R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                    return null;
                }
            }
            if (height != 0)
            {
                if (bitmap.Height != height)
                {
                    R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                    return null;
                }
            }

            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , paletteHint.Palette
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, paletteHint.Palette));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, paletteHint));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return null;
                    }
                }
            }
            return bitmap;
        }


        public static Bitmap LoadAndConvertDecolorUI(Control self,Bitmap paletteHint, int width, int height, bool useTSA, int maxPalette, bool isReserve1StPalette = true)
        {
            string imagefilename = ImageFormRef.OpenFilenameDialogFullColor(self);
            if (imagefilename == "")
            {
                return null;
            }

            return LoadAndConvertDecolorUILow(imagefilename, paletteHint, width, height, useTSA, maxPalette, isReserve1StPalette);
        }

        public static Bitmap LoadAndConvertDecolorUILow(string imagefilename, Bitmap paletteHint, int width, int height, bool useTSA, int maxPalette, bool isReserve1StPalette = true)
        {
            //パレットで開いてみる
            string errormessage;
            Bitmap bitmap = ImageUtil.OpenBitmap(imagefilename, paletteHint, out errormessage);
            if (bitmap == null)
            {//パレットでは開けない。フルカラーかな?
                bitmap = ImageUtil.OpenLowBitmap(imagefilename); //bitmapそのものの色で開く.
                if (bitmap == null)
                {//フルカラーでも開けない
                    R.ShowStopError(R._("画像の読み込みに失敗しました。\r\n\r\n{0}"), errormessage);
                    return null;
                }
            }
            if (width == 0)
            {
                width = bitmap.Width;
            }
            if (height == 0)
            {
                height = bitmap.Height;
            }

            bitmap = ConvertDecolorUI(bitmap, width, height, useTSA, maxPalette, isReserve1StPalette);
            if (bitmap == null)
            {//フルカラーでも開けない
                return bitmap;
            }
            return bitmap;
        }
        public static Bitmap ConvertDecolorUI(Bitmap bitmap, int width, int height, bool useTSA, int maxPalette, bool isReserve1StPalette = true)
        {
            bool isFullColor = (ImageUtil.IsFullColorBitmap(bitmap)) ;
            int yohaku = 0;

            if (bitmap.Width != width || bitmap.Height != height)
            {
                if (useTSA && width == 256 && bitmap.Width == 240)
                {
                    //右側に余白がない場合、自動的に挿入する
                    yohaku = 2;
                }
                else
                {
                    R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                    return null;
                }
            }

            string errorMessage = "";
            if (useTSA)
            {
                //エラーを確認するためにTSAを構築してみる
                ImageUtil.ImageToBytePlainTSA(bitmap, bitmap.Width, bitmap.Height, out errorMessage);
            }

            if (errorMessage == "")
            {
                if (isFullColor)
                {
                    errorMessage = R._("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", "FullColor", maxPalette) + "\r\n" + DecreaseColorTSAToolForm.GetExplainDecreaseColor();
                }
            }

            if (errorMessage == "")
            {
                int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
                if (bitmap_palette_count > maxPalette)
                {
                    errorMessage = R._("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, maxPalette) + "\r\n" + DecreaseColorTSAToolForm.GetExplainDecreaseColor();
                }
            }

            if (errorMessage != "")
            {
                ErrorTSAErrorForm f = (ErrorTSAErrorForm)InputFormRef.JumpFormLow<ErrorTSAErrorForm>();
                f.SetErrorMessage(errorMessage);
                f.SetReOrderImage1(bitmap, maxPalette, yohaku, isReserve1StPalette);
                f.ShowDialog();
                bitmap = f.GetResultBitmap();
                if (bitmap == null)
                {
                    return null;
                }
            }

            return bitmap;
        }

        public static Bitmap ConvertPaletteTransparentUI(Bitmap bitmap)
        {
            //透過色テスト
            if (ImageUtil.CheckPaletteTransparent(bitmap))
            {//問題なし
                return bitmap;
            }

            ErrorPaletteTransparentForm f = (ErrorPaletteTransparentForm)InputFormRef.JumpFormLow<ErrorPaletteTransparentForm>();
            f.SetOrignalImage(bitmap);
            f.ShowDialog();

            int backgroundColorIndex = f.GetResultColorIndex();
            if (backgroundColorIndex < 0)
            {//キャンセル.
                bitmap.Dispose();
                return null;
            }
            //通過色を入れ変える.
            ImageUtil.ChangeColorPixel(bitmap, (byte)0, (byte)(backgroundColorIndex));
            return bitmap;
        }

        public static Bitmap OpenBitmap(string filename)
        {
            string errormessage;
            return OpenBitmap(filename,null, out errormessage);
        }

        public static Bitmap OpenBitmap(string filename,Bitmap paletteHint,out string errormessage)
        {
            errormessage = "";

            if (!File.Exists(filename))
            {
                errormessage = R.Error(("ファイルがありません。\r\nファイル名:{0}"), filename);
                return null;
            }

            Bitmap bitmap;
            try
            {
                FileStream fs = new System.IO.FileStream(
                    filename,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                Bitmap tempBitmap = new Bitmap(fs);
                fs.Close();

                //FileStreamだとパレットにアクセスした場合などでエラーが発生することがあるので、
                //安全を取るために、バイナリレベルでコピーする.
                bitmap = CloneBitmap(tempBitmap);
                tempBitmap.Dispose();
            }
            catch (ArgumentException e)
            {
                errormessage = R.Error(("ファイルを画像として読みこめませんでした。\r\nファイル名:{0}\r\n{1}"), filename, e.ToString());
                return null;
            }
            if (bitmap == null)
            {
                errormessage = R.Error(("ファイルがありません。\r\nファイル名:{0}"), filename);
                return null;
            }
            Bitmap ret = ConvertIndexedBitmap(bitmap, paletteHint, out errormessage);
            bitmap.Dispose(); //一応解放しておく.
            if (ret == null)
            {
                errormessage += "\r\n"+R._("ファイル名:{0}",filename)+"\r\n";
                return null;
            }
            return ret;
        }
        static bool IsDouplicateColorFEditorImage(Bitmap bitmap, ColorPalette newpal)
        {
            if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                if (bitmap.PixelFormat != PixelFormat.Format4bppIndexed)
                {
                    //パレットを利用していない場合、利用できない.
                    return false;
                }
            }

            int limit = Math.Min(16, newpal.Entries.Length);

            //パレットに重複はありますか?
            for (int i = 1; i < limit; i++)
            {
                Color c = newpal.Entries[i];
                Color zero = newpal.Entries[0];
                if (c.R == zero.R
                    && c.G == zero.G
                    && c.B == zero.B)
                {
                    continue;
                }

                for (int n = i + 1; n < limit; n++)
                {
                    Color nc = newpal.Entries[n];
                    if (nc.R == c.R
                        && nc.G == c.G
                        && nc.B == c.B)
                    {//同じ色が複数回利用されている.
                        return true;
                    }
                }
            }

            return false;
        }
        static Bitmap ConvertIndexedBitmapFEditorPaletteIndex(Bitmap bitmap
            , ColorPalette newpal
            , out string errormessage)
        {
            Bitmap retbitmap;
            if (bitmap.PixelFormat == PixelFormat.Format4bppIndexed)
            {//16色 正しいんだけど、0xf バイトは扱いづらいので 256色に拡張する.
                retbitmap = ConvertIndexedBitmap16Color(bitmap, null, out errormessage);
            }
            else
            {//ロックされないようにコピーする.
                retbitmap = CloneBitmap(bitmap);
            }
            //256色の画像のはずです.
            Debug.Assert(retbitmap.PixelFormat == PixelFormat.Format8bppIndexed);

            int bitmapsize = retbitmap.Width * retbitmap.Height;

            Rectangle destrect = new Rectangle(new Point(), retbitmap.Size);
            BitmapData destbmpData = retbitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            ColorPalette pal = retbitmap.Palette;

            byte[] paletteMap = new byte[256];
            for (int i = 0; i < newpal.Entries.Length; i++)
            {
                if (i >= pal.Entries.Length)
                {
                    break;
                }
                Color nc = newpal.Entries[i];
                Color c = pal.Entries[i];
                paletteMap[i] = (byte)i;

                if (nc.R == c.R
                    && nc.G == c.G
                    && nc.B == c.B)
                {
                    continue;
                }
                //色が違う場合、どの色と置き換えるのか探索する.
                for (int n = 0; n < pal.Entries.Length; n++)
                {
                    Color cc = pal.Entries[n];
                    if (nc.R == cc.R
                        && nc.G == cc.G
                        && nc.B == cc.B)
                    {
                        paletteMap[i] = (byte)n;
                        break;
                    }
                }
            }

            retbitmap.Palette = newpal;
            for (int y = 0; y < destrect.Height; y++)
            {
                for (int x = 0; x < destrect.Width; x++)
                {
                    int pos = x + (y * destrect.Width);

                    byte c;
                    if (x >= destrect.Width - 8)
                    {//パレットマークはいらないので塗りつぶす.
                        c = 0;
                    }
                    else
                    {
                        byte index = Marshal.ReadByte(dest, pos);
                        c = paletteMap[index];
                    }
                    Marshal.WriteByte(dest, pos, c);
                }
            }

            errormessage = "";
            retbitmap.UnlockBits(destbmpData);
            return retbitmap;
        }

        static Bitmap ConvertIndexedBitmapFEditor(Bitmap bitmap, Bitmap paletteHint,int paletteCount, out string errormessage)
        {
            Debug.Assert(paletteCount >= 1);
            errormessage = "";

            Bitmap retbitmap = ImageUtil.Blank(bitmap.Width, bitmap.Height);
            int bitmapsize = bitmap.Width * bitmap.Height;

            Rectangle destrect = new Rectangle(new Point(), retbitmap.Size);
            BitmapData destbmpData = retbitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;


            Debug.Assert(IsFeditorImage(bitmap, paletteCount));

            ColorPalette newpal = retbitmap.Palette;
            int maxY = paletteCount * 2;
            for (int y = 0; y < maxY; y++)
            {
                for (int i = 0; i < 8; i++)
                {
                    newpal.Entries[i + (8 * y)] = bitmap.GetPixel(destrect.Width - 1 - i, y);
                }
            }
            //重複する色をもっているFEidtorの画像形式か?
            bool isDuplicateColorFEditorImage = IsDouplicateColorFEditorImage(bitmap, newpal);

            int maxPalette = paletteCount * 16;
            for (int i = maxPalette; i < newpal.Entries.Length; i++)
            {
                newpal.Entries[i] = Color.Black;
            }
            retbitmap.Palette = newpal;

            if (isDuplicateColorFEditorImage)
            {
                retbitmap.UnlockBits(destbmpData);
                retbitmap.Dispose();
                return ConvertIndexedBitmapFEditorPaletteIndex(bitmap, newpal , out errormessage);
            }

            Dictionary<Color, int> colorCahce = new Dictionary<Color, int>();

            for (int y = 0; y < destrect.Height; y++)
            {
                for (int x = 0; x < destrect.Width; x++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    if (x >= destrect.Width - 8)
                    {//パレットマークはいらないので塗りつぶす.
                        c = newpal.Entries[0];
                    }

                    int paletteno = 0;
                    if (colorCahce.ContainsKey(c))
                    {
                        paletteno = colorCahce[c];
                    }
                    else
                    {
                        for (; paletteno < maxPalette; paletteno++)
                        {
                            if (newpal.Entries[paletteno] == c)
                            {
                                break;
                            }
                        }

                        if (paletteno >= maxPalette)
                        {//近い色はあるのかな？
                            double min_distance = 500;
                            for (int i = 0; i < maxPalette; i++)
                            {
                                double distance =
                                    Math.Sqrt(
                                        Math.Pow(newpal.Entries[i].R - c.R, 2) +
                                        Math.Pow(newpal.Entries[i].G - c.G, 2) +
                                        Math.Pow(newpal.Entries[i].B - c.B, 2)
                                        );
                                if (min_distance > distance)
                                {
                                    paletteno = i;
                                    min_distance = distance;
                                }
                            }
                            if (min_distance >= 1000)
                            {//近い色がない
                                errormessage = R.Error(("FEditor形式パレットに存在しない色があるため、読み込めません.\r\nX:{0} Y:{1}"), x, y);
                                retbitmap.UnlockBits(destbmpData);
                                return null;
                            }
                        }
                        colorCahce[c] = paletteno;
                    }
                    int pos = x + (y * destrect.Width);
                    Marshal.WriteByte(dest, pos, (byte)paletteno);
                }
            }

            retbitmap.UnlockBits(destbmpData);
            return retbitmap;

        }

        //ロックされないようにコピーする.
        public static Bitmap CloneBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }

            Bitmap retbitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);

            if (retbitmap.PixelFormat == PixelFormat.Format1bppIndexed
                || retbitmap.PixelFormat == PixelFormat.Format4bppIndexed
                || retbitmap.PixelFormat == PixelFormat.Format8bppIndexed
                || retbitmap.PixelFormat == PixelFormat.Indexed
                )
            {
                //パレットの読込.
                retbitmap.Palette = bitmap.Palette;
            }

            Rectangle rect = new Rectangle(new Point(), retbitmap.Size);

            BitmapData srcbmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr src = srcbmpData.Scan0;

            BitmapData destbmpData = retbitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr dest = destbmpData.Scan0;

            U.CopyMemory(dest, src, (uint)(srcbmpData.Stride * bitmap.Height));

            retbitmap.UnlockBits(destbmpData);
            bitmap.UnlockBits(srcbmpData);
            return retbitmap;
        }

        public static string HashBitmap(string filename, string dir)
        {
            string fullfilename = Path.Combine(dir, filename);
            return HashBitmap(fullfilename);
        }
        public static string HashBitmap(string fullfilename)
        {
            Bitmap bitmap = OpenBitmap(fullfilename);
            if (bitmap == null)
            {//Not Found
                return "";
            }
            string hash = HashBitmap(bitmap);

            bitmap.Dispose();

            return hash;
        }

        //画像のハッシュ値を取得する 同一画像判定に利用する
        public static string HashBitmap(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);

            BitmapData srcbmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr src = srcbmpData.Scan0;
            byte[] dest = new byte[srcbmpData.Stride * bitmap.Height];

            Marshal.Copy(src, dest, 0, dest.Length);

            string ret = U.md5(dest);

            bitmap.UnlockBits(srcbmpData);

            return ret;
        }

        static Bitmap ConvertIndexedBitmap16Color(Bitmap bitmap, Bitmap paletteHint, out string errormessage)
        {
            errormessage = "";

            Bitmap retbitmap = ImageUtil.Blank(bitmap.Width, bitmap.Height);
            int bitmapsize = bitmap.Width * bitmap.Height;

            Rectangle destrect = new Rectangle(new Point(), retbitmap.Size);
            BitmapData destbmpData = retbitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            ColorPalette newpal = retbitmap.Palette;

            //16色 正しいんだけど、0xf バイトは扱いづらいので 256色に拡張する.
            Debug.Assert(bitmap.PixelFormat == PixelFormat.Format4bppIndexed);
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr src = bmpData.Scan0;

            ColorPalette openpal = bitmap.Palette;
            int palette_count = Math.Min(openpal.Entries.Length , 0x10);
            for (int i = 0; i < palette_count; i++)
            {
                newpal.Entries[i] = openpal.Entries[i];
            }
            for (int i = palette_count; i <= 0xff; i++)
            {
                newpal.Entries[i] = Color.Black;
            }
            retbitmap.Palette = newpal;

            for (int i = 0; i < bitmapsize; i++)
            {
                byte a;
                if ((i & 0x1) <= 0)
                {
                    a = Marshal.ReadByte(src, i / 2);
                    a = (byte)((a >> 4) & 0xF);
                }
                else
                {
                    a = Marshal.ReadByte(src, i / 2);
                    a = (byte)(a & 0xF);
                }
                Marshal.WriteByte(dest, i, a);
            }
            bitmap.UnlockBits(bmpData);
            retbitmap.UnlockBits(destbmpData);
            return retbitmap;
        }
        static Bitmap ConvertIndexedBitmap4Color(Bitmap bitmap, Bitmap paletteHint, out string errormessage)
        {
            errormessage = "";

            Bitmap retbitmap = ImageUtil.Blank(bitmap.Width, bitmap.Height);
            int bitmapsize = bitmap.Width * bitmap.Height;

            Rectangle destrect = new Rectangle(new Point(), retbitmap.Size);
            BitmapData destbmpData = retbitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            ColorPalette newpal = retbitmap.Palette;

            //1色 扱いずらいので 256色に拡張する.
            Debug.Assert(bitmap.PixelFormat == PixelFormat.Format1bppIndexed);

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr src = bmpData.Scan0;

            ColorPalette openpal = bitmap.Palette;
            newpal.Entries[0] = U.at(openpal.Entries, 0, Color.Black);
            newpal.Entries[1] = U.at(openpal.Entries, 1, Color.Black);

            for (int i = 0x2; i <= 0xff; i++)
            {
                newpal.Entries[i] = Color.Black;
            }
            retbitmap.Palette = newpal;

            for (int y = 0; y < rect.Height; y++)
            {
                for (int x = 0; x < rect.Width; x++)
                {
                    int pos = (x >> 3) + bmpData.Stride * y;
                    byte a = Marshal.ReadByte(src, pos);

                    byte b = (byte)(0x80 >> (x & 0x7));

                    if ((a & b) > 0)
                    {
                        int writepos = (x) + destbmpData.Stride * y;
                        Marshal.WriteByte(dest, writepos, 1);
                    }
                }
            }

            bitmap.UnlockBits(bmpData);
            retbitmap.UnlockBits(destbmpData);
            return retbitmap;
        }

        static Bitmap ConvertIndexedBitmapPaletteHint(Bitmap bitmap, Bitmap paletteHint, out string errormessage)
        {
            errormessage = "";

            Bitmap retbitmap = ImageUtil.Blank(bitmap.Width, bitmap.Height);
            int bitmapsize = bitmap.Width * bitmap.Height;

            Rectangle destrect = new Rectangle(new Point(), retbitmap.Size);
            BitmapData destbmpData = retbitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            ColorPalette newpal = retbitmap.Palette;

            //パレットヒントがある場合、そのパレットをベースとして展開する
            Debug.Assert (paletteHint != null && paletteHint.Palette != null);

            int palettecount = paletteHint.Palette.Entries.Length;
            for (int paletteno = 0; paletteno < palettecount; paletteno++)
            {
                newpal.Entries[paletteno] = paletteHint.Palette.Entries[paletteno];
            }
            Dictionary<Color, int> colorCahce = new Dictionary<Color, int>();

            for (int y = 0; y < destrect.Height; y++)
            {
                for (int x = 0; x < destrect.Width; x++)
                {
                    Color cc = bitmap.GetPixel(x, y);

                    int paletteno = 0;
                    if (cc.A > 0)
                    {//透明じゃないので色があるはず.
                        if (colorCahce.ContainsKey(cc))
                        {
                            paletteno = colorCahce[cc];
                        }
                        else
                        {
                            double min_distance = 1000;
                            for (int i = 0; i < 16; i++)
                            {
                                double distance =
                                    Math.Sqrt(
                                        Math.Pow(newpal.Entries[i].R - cc.R, 2) +
                                        Math.Pow(newpal.Entries[i].G - cc.G, 2) +
                                        Math.Pow(newpal.Entries[i].B - cc.B, 2)
                                        );
                                if (min_distance > distance)
                                {
                                    paletteno = i;
                                    min_distance = distance;
                                }
                            }
                            if (min_distance >= 1000)
                            {//近い色がない
                                errormessage = R.Error(("パレットが255を超えました.\r\nX:{0} Y:{1}"), x, y);
                                retbitmap.UnlockBits(destbmpData);
                                return null;
                            }
                            colorCahce[cc] = paletteno;
                        }
                    }
                        
                    int pos = x + (y * destrect.Width);
                    Marshal.WriteByte(dest, pos, (byte)paletteno);
                }
            }
            retbitmap.UnlockBits(destbmpData);

            retbitmap.Palette = newpal;
            return retbitmap;
        }

        static Bitmap ConvertIndexedBitmapOtherPaletteFormat(Bitmap bitmap, Bitmap paletteHint, out string errormessage)
        {
            errormessage = "";

            Bitmap retbitmap = ImageUtil.Blank(bitmap.Width, bitmap.Height);
            int bitmapsize = bitmap.Width * bitmap.Height;

            Rectangle destrect = new Rectangle(new Point(), retbitmap.Size);
            BitmapData destbmpData = retbitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            ColorPalette newpal = retbitmap.Palette;

            int palettecount = 0;

            for (int y = 0; y < destrect.Height; y++)
            {
                for (int x = 0; x < destrect.Width; x++)
                {
                    Color cc = bitmap.GetPixel(x, y);

                    //どうせGBAパレットで下3ビットは消えるので見ない.
                    Color c = Color.FromArgb(
                          (cc.R >> 3) << 3
                        , (cc.G >> 3) << 3
                        , (cc.B >> 3) << 3
                        );

                    int paletteno = 0;
                    if (cc.A > 0)
                    {//透明じゃないので色があるはず.
                        for (; paletteno < palettecount; paletteno++)
                        {
                            if (newpal.Entries[paletteno] == c)
                            {//既存のパレットに色があった
                                break;
                            }
                        }
                    }

                    if (paletteno >= palettecount)
                    {//未知の色なのでパレットに追加.
                        if (palettecount >= 0xFF)
                        {//パレットが満杯
                            errormessage = R.Error(("パレットが255を超えました.\r\nX:{0} Y:{1}"), x, y);
                            retbitmap.UnlockBits(destbmpData);
                            return null;
                        }

                        newpal.Entries[palettecount] = c;
                        palettecount++;
                    }

                    int pos = x + (y * destrect.Width);
                    Marshal.WriteByte(dest, pos, (byte)paletteno);
                }
            }
            retbitmap.UnlockBits(destbmpData);

            retbitmap.Palette = newpal;
            return retbitmap;
        }
 
        //bitmapをパレットbitmapとして変換します
        public static Bitmap ConvertIndexedBitmap(Bitmap bitmap, Bitmap paletteHint, out string errormessage)
        {
            errormessage = "";
            if (IsFeditorImage(bitmap, 1))
            {//FEditor の画像らしい.
                return ConvertIndexedBitmapFEditor(bitmap, paletteHint, 1, out errormessage);
            }
            if (IsFeditorImage(bitmap, 4))
            {//HighColorのFEditor の画像らしい.
                return ConvertIndexedBitmapFEditor(bitmap, paletteHint, 4, out errormessage);
            }
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {//256色パレット このツールの基本フォーマットなので何もしないでそのまま採用.
                //ロックされないようにコピーする.
                return CloneBitmap(bitmap);
            }
            if (bitmap.PixelFormat == PixelFormat.Format4bppIndexed)
            {//16色 正しいんだけど、0xf バイトは扱いづらいので 256色に拡張する.
                return ConvertIndexedBitmap16Color(bitmap, paletteHint, out errormessage);
            }
            if (bitmap.PixelFormat == PixelFormat.Format1bppIndexed)
            {//1色 扱いずらいので 256色に拡張する.
                return ConvertIndexedBitmap4Color(bitmap, paletteHint, out errormessage);
            }

            //パレットヒントがある場合、そのパレットをベースとして展開する
            if (paletteHint != null && paletteHint.Palette != null)
            {
                return ConvertIndexedBitmapPaletteHint(bitmap, paletteHint, out errormessage);
            }

            //謎のイメージ 機械的に256色で読み込んでみる.
            Bitmap r = ConvertIndexedBitmapOtherPaletteFormat(bitmap, paletteHint, out errormessage);
            if (r != null)
            {
                return r;
            }
            return ImageConvert24biTo8bit(bitmap);
        }

        unsafe static Bitmap ImageConvert24biTo8bit(Bitmap bmpSource)
        {
            Bitmap dstBmp = bmpSource.Clone(new Rectangle(0, 0, bmpSource.Width, bmpSource.Height), PixelFormat.Format8bppIndexed);
            return dstBmp;
        }

        public static byte[] ImageToByte256Tile(Bitmap bitmap, int width, int height)
        {
            byte[] data = new byte[width * height];

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int nn = 0;
            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    for (int y8 = 0; y8 < 8; y8++)
                    {
                        for (int x8 = 0; x8 < 8; x8 ++)
                        {
                            byte a = Marshal.ReadByte(adr, (x + x8 + 0) + bmpData.Stride * (y + y8));
                            data[nn] = (byte)(a);
                            nn++;
                        }
                    }
                }
            }

            bitmap.UnlockBits(bmpData);
            return data;
        }
        public static byte[] ImageToByte256Liner(Bitmap bitmap, int width, int height)
        {
            byte[] data = new byte[width * height];

            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;

            int nn = 0;
            for (int y = 0; y < height; y ++)
            {
                for (int x = 0; x < width; x ++)
                {
                    byte a = Marshal.ReadByte(adr, (x) + bmpData.Stride * (y));
                    data[nn] = a;
                    nn++;
                }
            }

            bitmap.UnlockBits(bmpData);
            return data;
        }


        //Rotate 回転
        public static void RotateDirect(Bitmap destBitmap, int rx2, int ry2, int rw1, int rh1, Bitmap srcBitmap, int rx1, int ry1, int jikux, int jikuy, double Sin, double Cos)
        {
            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            BitmapData srcBmpData = srcBitmap.LockBits(srcrect, ImageLockMode.ReadWrite, srcBitmap.PixelFormat);
            IntPtr srcPtr = srcBmpData.Scan0;

            int srcx, srcy;
            int rx, ry;
            int tx, ty;
            int min_x, max_x, min_y, max_y;
            double RSin, RCos;//サインコサインのテンポラリ

            //逆サイン 逆コサインを取得!!
            RSin = -1* Sin;  //sin(radian(360 - deg))
            RCos = Cos;      //cos(radian(360 - deg))  
            //テクスチャの各頂点を jiku を中心に回転させます
            //これによって、最大、最低の転送範囲を取得します
            /*
	            min
	            ------------------------
                |                      |
                |                      |
                |          jiku        |
                |                      |
                |                      |
                |                      |
	            ------------------------
							            max
            */
            //左上
            tx = srcx = rx1 - jikux;
            ty = srcy = ry1 - jikuy;
            rx = (int)Math.Round(tx * Cos - ty * Sin);
            ry = (int)Math.Round(tx * Sin + ty * Cos);
            min_x = max_x = rx;
            min_y = max_y = ry;
            //右上
            tx = srcx + rw1;
            ty = srcy;
            rx = (int)Math.Round(tx * Cos - ty * Sin);
            ry = (int)Math.Round(tx * Sin + ty * Cos);
            if (min_x > rx) min_x = rx;
            if (max_x < rx) max_x = rx;
            if (min_y > ry) min_y = ry;
            if (max_y < ry) max_y = ry;
            //左下
            tx = srcx;
            ty = srcy + rh1;
            rx = (int)Math.Round(tx * Cos - ty * Sin);
            ry = (int)Math.Round(tx * Sin + ty * Cos);
            if (min_x > rx) min_x = rx;
            if (max_x < rx) max_x = rx;
            if (min_y > ry) min_y = ry;
            if (max_y < ry) max_y = ry;
            //右下
            tx = srcx + rw1;
            ty = srcy + rh1;
            rx = (int)Math.Round(tx * Cos - ty * Sin);
            ry = (int)Math.Round(tx * Sin + ty * Cos);
            if (min_x > rx) min_x = rx;
            if (max_x < rx) max_x = rx;
            if (min_y > ry) min_y = ry;
            if (max_y < ry) max_y = ry;
            //min から初めて　max までデータを転送します.
            //でも、その前に　受け取る側の開始アドレスを求めます.
            //min max シリーズは軸座標です.
            //それを加えて(ほどんど　マイナスなので引き算だけど)転送開始場所もとめ.
            srcx = rx2 + min_x;
            srcy = ry2 + min_y;
            max_x++; max_y++;

            //クリッピング
            if (srcx < 0)
            { srcx = 0; min_x = -rx2; }
            //クリッピング
            if (srcy < 0)
            { srcy = 0; min_y = -ry2; }
            //クリッピング
            if (srcx + (max_x - min_x) > destrect.Width)
            { max_x = destrect.Width - srcx + min_x - 1; }
            if (srcy + (max_y - min_y) > destrect.Height)
            { max_y = destrect.Height - srcy + min_y - 1; }

            //転送開始アドレス取得
            int AddPitch1 = srcBmpData.Stride;
            int data1 = rx1 + srcBmpData.Stride * ry1;

            int AddPitch2 = destBmpData.Stride;
            int data2 = srcx + destBmpData.Stride * srcy;

            //デバッグ用(転送範囲を白いボックスで塗りつぶす)
            double Xx, Xy;
            double Yx, Yy;
            //初期値を求める.
            Yx = (min_x * RCos) - (min_y * RSin);
            Yy = (min_x * RSin) + (min_y * RCos);
            //テクスチャを横断して画面に水平に貼り付けます.
            /*

	            Texter             Screen
            |---------------|      |-----------------------|
            |  -^           |      |   ------------------> |
            | |             |  ->  |   ------------------> |
            |-              |      |   ------------------> |
            |---------------|      |                       |
                斜めに横断         |                       |
                                    -------------------------
						            まっすぐ絵画
            */
            //転送開始
            for (srcy = min_y; srcy <= max_y; srcy++)
            {
                Xx = Yx;
                Xy = Yy;

                int BackupPoint2 = data2;
                for (srcx = min_x; srcx <= max_x; srcx++)
                {
                    //回転.
                    rx = (int)Math.Round(Xx) + jikux;
                    ry = (int)Math.Round(Xy) + jikuy;
                    //クリッピング処理
                    if (rx >= 0 && rx < rw1 && ry >= 0 && ry < rh1)
                    {
                        //転送
                        byte a = Marshal.ReadByte(srcPtr, data1 + (rx) + (ry * AddPitch1));

                        Marshal.WriteByte(destPtr, data2, a);
                    }
                    Xx += RCos;
                    Xy += RSin;
                    data2++;
                }
                Yx -= RSin;
                Yy += RCos;
                data2 = BackupPoint2;
                data2 += AddPitch2;
            }
            destBitmap.UnlockBits(destBmpData);
            srcBitmap.UnlockBits(srcBmpData);
        }

        //Rotate 回転
        public static void Rotate(Bitmap destBitmap, int rx2, int ry2, int rw1, int rh1, Bitmap srcBitmap, int rx1, int ry1,int jikux,int jikuy,double deg)
        {
            double seeta = deg * Math.PI / 180;

            double Sin = Math.Sin(seeta);
            double Cos = Math.Cos(seeta);
            RotateDirect(destBitmap, rx2, ry2, rw1, rh1, srcBitmap, rx1, ry1, jikux, jikuy, Sin, Cos);
        }
        //Rotate 回転
        public static void RotateCenter(Bitmap destBitmap, int rx2, int ry2, Bitmap srcBitmap, double seeta)
        {
            Rotate(destBitmap, rx2 + (srcBitmap.Width / 2), ry2 + (srcBitmap.Height / 2), srcBitmap.Width, srcBitmap.Height, srcBitmap, 0, 0, srcBitmap.Width / 2, srcBitmap.Height / 2, seeta);
        }
#if DEBUG
        public static void TEST_RotateCenter()
        {
            Bitmap a = OpenBitmap(Program.GetTestData("rotatetest.png"));
            Bitmap dest = ImageUtil.Blank(129 * 2, 50 * 2, a);
            RotateCenter(dest, 129/2, 50/2, a, 119.80094368561922);
            Bitmap test = OpenBitmap(Program.GetTestData("rotatetest_result.png"));

            bool r = ImageUtil.CompareBitmap(dest, test);
            Debug.Assert(r == true);
        }
#endif
        //拡大縮小
        public static void Scale(Bitmap destBitmap, int rx2, int ry2, int rw2, int rh2, Bitmap srcBitmap, int rx1, int ry1, int rw1, int rh1, int transparent_index = 0xff)
        {
            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            Rectangle srcrect = new Rectangle(new Point(), srcBitmap.Size);
            BitmapData srcBmpData = srcBitmap.LockBits(srcrect, ImageLockMode.ReadWrite, srcBitmap.PixelFormat);
            IntPtr srcPtr = srcBmpData.Scan0;

            int x, y;
            double fx, fy;
            double sfx, sfy;
            int ew, eh;

            if (rw2 != 0)
            {
                sfx = ((double)(rw1) / (double)(rw2)); //比率計算
            }
            else
            {
                sfx = 0;
            }
            if (rh2 != 0)
            {
                sfy = ((double)(rh1) / (double)(rh2));
            }
            else
            {
                sfy = 0;
            }

            ew = rw2 ;
            eh = rh2 ;
            for (y = 0, fy = ry1 ; y < eh ; y++, fy += sfy)
            {
                for (x = 0, fx = rx1 ; x < ew ; x++, fx += sfx)
                {
                    //取得 本当は、線形補完したいが、パレットだから無理だねww
                    //linerInterpolate(bmd1, fx, fy, &rgb);
                    int srcx = (int)Math.Round(fx);
                    int srcy = (int)Math.Round(fy);
                    if (srcx < 0 || srcx >= srcBmpData.Width || srcy < 0 || srcy >= srcBmpData.Height)
                    {
                        continue;
                    }
                    int destx = (int)(x + rx2);
                    int desty = (int)(y + ry2);
                    if (destx < 0 || destx >= destBmpData.Width || desty < 0 || desty >= destBmpData.Height)
                    {
                        continue;
                    }

                    //転送
                    byte a = Marshal.ReadByte(srcPtr, (int) (srcx) + (srcy * srcBmpData.Stride) );
                    if (transparent_index != a)
                    {
                        Marshal.WriteByte(destPtr, destx + (desty * destBmpData.Stride), a);
                    }
                }
            }
            destBitmap.UnlockBits(destBmpData);
            srcBitmap.UnlockBits(srcBmpData);
        }
#if DEBUG
        static void TEST_ScaleBitmap()
        {
            {
                Bitmap src = OpenBitmap(Program.GetTestData("rotatetest.png"));
                Bitmap dest = ImageUtil.Blank(100, 100, src);
                ImageUtil.Scale(dest, 50, 50,50,50 , src, 0, 0, src.Width, src.Height);
                Bitmap test = OpenBitmap(Program.GetTestData("scale_result.png"));

                bool r = ImageUtil.CompareBitmap(dest, test);
                Debug.Assert(r == true);
            }
        }
#endif
        //拡大縮小
        public static void ScaleAll(Bitmap destBitmap, int rx2, int ry2, int rw2, int rh2, Bitmap srcBitmap, int rx1, int ry1)
        {
            Scale(destBitmap, rx2, ry2, rw2, rh2,srcBitmap, 0, 0, srcBitmap.Width, srcBitmap.Height);
        }

        public static void SettingFontPalette(Bitmap pic,Color bgColor)
        {
            //パレット
            if (bgColor == null)
            {
                bgColor = Color.FromArgb(255, 255, 255);
            }
            ColorPalette palette = pic.Palette; //一度、値をとってからいじらないと無視される・・ C#むずかしい
            palette.Entries[0] = bgColor;
            palette.Entries[1] = Color.FromArgb(0xA8, 0xA8, 0xA7); //グレー
            palette.Entries[2] = Color.FromArgb(0xF8, 0xF8, 0xF8); //白
            palette.Entries[3] = Color.FromArgb(0x28, 0x28, 0x28); //黒
            for (int i = 4; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = Color.FromArgb(0, 0, 0);
            }
            pic.Palette = palette;
        }

        //足りないフォントの生成.
        public static Bitmap AutoGenerateFont(string moji, Font font, bool isItemFont, bool isSquareFont, out int out_width)
        {
            if (isItemFont)
            {
                return AutoGenerateItemFont(moji, font, out out_width);
            }
            else
            {
                return AutoGenerateTextFont(moji, font, out out_width);
            }
        }

        static Bitmap AutoGenerateTextFont(string moji, Font font, out int out_width)
        {
            Bitmap baseBitmap = new Bitmap(16,16);
            using (Graphics g = Graphics.FromImage(baseBitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, 16, 16));

                g.TextContrast = 0;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(moji, font, Brushes.Black, new Point(0, 0));

                g.Dispose();
            }

            Bitmap scaleBitmap = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(scaleBitmap))
            {
                g.DrawImage(baseBitmap, 0, 0, 16, 22); 
            }

            Bitmap fontBitmap = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(fontBitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, 16, 16));
                g.DrawImage(scaleBitmap, -2, 0);
            }

            Bitmap destBitmap = ImageUtil.BlankDummy(16);
            SettingFontPalette(destBitmap, FontForm.SerifFontColor);

            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            int max_width = 0;
            int y, x;
            {//セリフフォント
                for (y = 0; y < 16; y++)
                {
                    for (x = 0; x < 16; x++)
                    {
                        byte a;
                        Color c = fontBitmap.GetPixel(x, y);
                        if (IsFontColorFore(c))
                        {//黒文字
                            a = 3; //フォントは3番目が黒
                        }
                        else
                        {//白背景
                            a = 0; //フォントは0番目が背景
                        }

                        if (a == 3 && x > max_width)
                        {
                            max_width = x;
                        }
                        Marshal.WriteByte(destPtr, (x) + (y) * destBmpData.Stride, a);
                    }
                }
            }
            destBitmap.UnlockBits(destBmpData);

            out_width = Math.Min(max_width + 1, 16);
            return destBitmap;
        }

        static bool IsFontColorFore(Color c)
        {
            if (c == null)
            {
                return false;
            }
            return c.R < 0xa0 ;
        }

        static Bitmap AutoGenerateItemFont(string moji, Font font, out int out_width)
        {
            Bitmap baseBitmap = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(baseBitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, 16, 16));

                g.TextContrast = 0;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(moji, font, Brushes.Black, new Point(0, 0));

                g.Dispose();
            }

            Bitmap scaleBitmap = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(scaleBitmap))
            {
                g.DrawImage(baseBitmap, 0, 0, 16, 16);
            }

            Bitmap fontBitmap = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(fontBitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, 16, 16));
                g.DrawImage(scaleBitmap, -1, 2);
            }

            Bitmap destBitmap = ImageUtil.BlankDummy(16);
            SettingFontPalette(destBitmap, FontForm.ItemFontColor);

            Rectangle destrect = new Rectangle(new Point(), destBitmap.Size);
            BitmapData destBmpData = destBitmap.LockBits(destrect, ImageLockMode.ReadWrite, destBitmap.PixelFormat);
            IntPtr destPtr = destBmpData.Scan0;

            int max_width = 0;
            int y, x;
            {//アイテムフォントとして白枠を作ります
                for (y = 0; y < 16; y++)
                {
                    for (x = 0; x < 16; x++)
                    {
                        byte a;
                        Color c = fontBitmap.GetPixel(x, y);
                        if (IsFontColorFore(c))
                        {//黒文字
                            a = 2; //フォントは2番目が白
                        }
                        else
                        {//白背景
                            a = 0; //フォントは0番目が背景
                            if (x > 1)
                            {
                                Color l = fontBitmap.GetPixel(x - 1, y);
                                if (IsFontColorFore(l))
                                {//右に文字があれば 黒3で埋める
                                    a = 3;
                                }
                            }
                            if (x < 15)
                            {
                                Color  r = fontBitmap.GetPixel(x + 1, y);
                                if (IsFontColorFore(r))
                                {//左に文字があれば 黒3で埋める
                                    a = 3;
                                }
                            }
                            if (y > 0)
                            {
                                Color t = fontBitmap.GetPixel(x, y - 1);
                                if (IsFontColorFore(t))
                                {//上に文字があれば 黒3で埋める
                                    a = 3;
                                }
                            }
                            if (y < 10)
                            {
                                Color b = fontBitmap.GetPixel(x, y + 1);
                                if (IsFontColorFore(b))
                                {//下に文字があれば 黒3で埋める
                                    a = 3;
                                }
                            }
                        }

                        if (a == 2 && x > max_width)
                        {
                            max_width = x;
                        }
                        Marshal.WriteByte(destPtr, (x) + (y) * destBmpData.Stride, a);
                    }
                }

            }
            destBitmap.UnlockBits(destBmpData);

            out_width = Math.Min(max_width+1,16);
            return destBitmap;
        }

        public static Bitmap AffineTransform(Bitmap bitmap, double angle, double scalex, double scaley)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;
            Bitmap retBitmap = new Bitmap(w*2, h*2);

            Bitmap fullColor = ImageUtil.CloneBitmap(bitmap);
            Graphics g = Graphics.FromImage(retBitmap);
            g.Clear(bitmap.Palette.Entries[0]);

            //中心点を軸にして回転
            g.TranslateTransform(w , h);
            g.RotateTransform((float)angle);
            g.TranslateTransform(- (w ), - (h ));
            g.TranslateTransform((float)(w*2 * (1 - scalex) / 2), (float)(h*2 * (1 - scaley) / 2));
            g.ScaleTransform((float)scalex, (float)scaley);
            g.DrawImage(fullColor, w / 2, h / 2);
            g.Dispose();

            string errormessage;
            Bitmap ret = ConvertIndexedBitmap(retBitmap, bitmap, out errormessage);
            retBitmap.Dispose();
            if (ret == null)
            {
                throw new Exception(errormessage);
            }
            return ret;
        }


        public static Bitmap BitmapSizeChange(Bitmap src,int x,int y,int width,int height)
        {
            Bitmap retBitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(retBitmap);
            
            //切り取る部分の範囲を決定する
            Rectangle srcRect = new Rectangle(x, y, width, height);
            //描画する部分の範囲を決定する
            Rectangle desRect = new Rectangle(0, 0, width, height);

            g.DrawImage(src, srcRect, desRect, GraphicsUnit.Pixel);
            g.Dispose();

            return retBitmap;
        }
        public static Bitmap BitmapSizeChange256(Bitmap src, int x, int y, int width, int height)
        {
            Bitmap retBitmap = BitmapSizeChange(src,x , y , width, height);

            string errormessage;
            Bitmap ret = ConvertIndexedBitmap(retBitmap, src, out errormessage);
            retBitmap.Dispose();
            if (ret == null)
            {
                throw new Exception(errormessage);
            }
            retBitmap.Dispose();

            return ret;
        }

        public static Bitmap BitmapScale(Bitmap src, int width, int height)
        {
            Bitmap retBitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(retBitmap);

            int imageWidth = src.Width;
            int imageHeight = src.Height;

            int newWidth,newHeight;

            //クライアント領域サイズ
            if ((double)imageWidth / imageHeight < (double)width / height)
            {
                newWidth = width;
                newHeight = width * imageHeight / imageWidth;
            }
            else
            {
                newHeight = height;
                newWidth = height * imageWidth / imageHeight;
            }

            g.DrawImage(src, 0, 0, newWidth, newHeight);
            g.Dispose();

            return BitmapSizeChange(retBitmap,0,0,width,height);
        }

        public static Bitmap BitmapScale256(Bitmap src, int width, int height)
        {
            Bitmap retBitmap = BitmapScale(src, width, height);

            string errormessage;
            Bitmap ret = ConvertIndexedBitmap(retBitmap, src, out errormessage);
            retBitmap.Dispose();
            if (ret == null)
            {
                throw new Exception(errormessage);
            }
            retBitmap.Dispose();

            return ret;
        }

        //指定したパレットに置換します.
        public static Bitmap OverraidePalette(Bitmap bitmap, ColorPalette pal)
        {
            Bitmap ret = ImageUtil.CloneBitmap(bitmap);
            //パレットの読込
            ret.Palette = pal;

            return ret;
        }
        //指定したパレットに置換します.
        public static Bitmap OverraidePalette(Bitmap bitmap, byte[] data, uint basepalette)
        {
            Bitmap ret = ImageUtil.CloneBitmap(bitmap);

            //パレットの読込
            ret.Palette = ByteToPalette(ret.Palette, data, (int)basepalette);
            return ret;
        }
        //指定されたパレットの順番に並び替えて再構築します.
        public static Bitmap ReOrderPalette(Bitmap bitmap, Bitmap paletteHint)
        {
            //bitmapをパレットbitmapとして変換します
            string errorMessage;
            Bitmap ret = ConvertIndexedBitmapPaletteHint(bitmap, paletteHint, out errorMessage);
            return ret;
        }

        //指定されたパレットの順番に並び替えて再構築します.
        public static Bitmap ReOrderPalette(Bitmap bitmap,byte[] data, uint basepalette)
        {
            //パレットの読込
            Bitmap paletteHint = new Bitmap(8, 8, PixelFormat.Format8bppIndexed);
            paletteHint.Palette = ByteToPalette(paletteHint.Palette, data, (int)basepalette);

            //bitmapをパレットbitmapとして変換します
            string errorMessage;
            Bitmap ret = ConvertIndexedBitmapPaletteHint(bitmap, paletteHint, out errorMessage);
            return ret;
        }

        public static Bitmap MakePaletteHint(byte[] data, uint basepalette)
        {
            Bitmap paletteHint = new Bitmap(8, 8, PixelFormat.Format8bppIndexed);
            paletteHint.Palette = ByteToPalette(paletteHint.Palette, data, (int)basepalette);
            return paletteHint;
        }
        public static Bitmap MakePaletteHintP(byte[] data, uint basepalettePointer)
        {
            uint basepalette = U.p32(data, basepalettePointer);

            Bitmap paletteHint = new Bitmap(8, 8, PixelFormat.Format8bppIndexed);
            paletteHint.Palette = ByteToPalette(paletteHint.Palette, data, (int)basepalette);
            return paletteHint;
        }
        public static Bitmap MakePaletteHint(uint basepalette)
        {
            return MakePaletteHint(Program.ROM.Data,  basepalette);
        }
        public static Bitmap MakePaletteHintP(uint basepalettePointer)
        {
            return MakePaletteHintP(Program.ROM.Data, basepalettePointer);
        }

        //指定されたパレットの順番に並び替えて再構築します.
        public static Bitmap ReOrderPaletteSetTransparent(Bitmap bitmap, byte[] data, uint basepalette)
        {
            //パレットの読込
            Bitmap paletteHint = new Bitmap(8, 8, PixelFormat.Format8bppIndexed);
            paletteHint.Palette = ByteToPalette(paletteHint.Palette, data, (int)basepalette);

            Bitmap bmp = ImageUtil.CloneBitmap(bitmap);
            U.MakeTransparent(bmp);

            //bitmapをパレットbitmapとして変換します
            string errorMessage;
            Bitmap ret = ConvertIndexedBitmapPaletteHint(bmp, paletteHint, out errorMessage);
            bmp.Dispose();

            return ret;
        }

        public static string CheckPalette(ColorPalette yourpalette,byte[] data, uint basepalette, uint basepalette2)
        {
            //パレットの読込.
            Bitmap pic1 = new Bitmap(8, 8, PixelFormat.Format8bppIndexed);
            pic1.Palette = ByteToPalette(pic1.Palette, data, (int)basepalette);

            if (basepalette2 == U.NOT_FOUND)
            {
                return CheckPalette(yourpalette, pic1.Palette, null);
            }
            else
            {
                //パレットの読込.
                Bitmap pic2 = new Bitmap(8, 8, PixelFormat.Format8bppIndexed);
                pic2.Palette = ByteToPalette(pic2.Palette, data, (int)basepalette2);

                return CheckPalette(yourpalette, pic1.Palette, pic2.Palette);
            }
        }

        public static string CheckPalette(ColorPalette yourpalette, ColorPalette basepalette, ColorPalette basepalette2 = null)
        {
            bool isOK = true;
            StringBuilder sb = new StringBuilder();
            string label;
            if (basepalette2 == null)
            {
                label = R._(" 番号 インポートRGB 規定パレット");
            }
            else
            {
                label = R._(" 番号 インポートRGB 規定パレット1 規定パレット2");
            }
            sb.AppendLine(label);

            int data_length = Math.Min(16, yourpalette.Entries.Length);
            data_length = Math.Min(data_length, basepalette.Entries.Length);

            for (int i = 0; i < data_length; i++)
            {
                string a = "";
                bool isMatch = false;
                if ((yourpalette.Entries[i].R == basepalette.Entries[i].R
                    && yourpalette.Entries[i].G == basepalette.Entries[i].G
                    && yourpalette.Entries[i].B == basepalette.Entries[i].B))
                {
                    isMatch = true;
                }
                if (isMatch != true && basepalette2 != null)
                {
                    if ((yourpalette.Entries[i].R == basepalette2.Entries[i].R
                        && yourpalette.Entries[i].G == basepalette2.Entries[i].G
                        && yourpalette.Entries[i].B == basepalette2.Entries[i].B))
                    {
                        isMatch = true;
                    }
                }
                if (isMatch)
                {
                    a += "  ";
                }
                else
                {//エラー
                    a += "!!";
                }
                a += i.ToString("X02");
                a += ":    ";

                a += yourpalette.Entries[i].R.ToString("X02");
                a += ",";
                a += yourpalette.Entries[i].G.ToString("X02");
                a += ",";
                a += yourpalette.Entries[i].B.ToString("X02");
                a += "      ";

                a += basepalette.Entries[i].R.ToString("X02");
                a += ",";
                a += basepalette.Entries[i].G.ToString("X02");
                a += ",";
                a += basepalette.Entries[i].B.ToString("X02");

                if (basepalette2 != null)
                {
                    a += "      ";
                    a += basepalette2.Entries[i].R.ToString("X02");
                    a += ",";
                    a += basepalette2.Entries[i].G.ToString("X02");
                    a += ",";
                    a += basepalette2.Entries[i].B.ToString("X02");
                }

                if (isMatch == false)
                {
                    isOK = false;
                }
                sb.AppendLine(a);
            }

            sb.AppendLine(R._("!!マークが出ているのが問題のパレットです。"));

            if (isOK)
            {
                return "";
            }
            else
            {
                return sb.ToString();
            }
        }

        //縁取りします.
        static public Bitmap Fuchidori(Bitmap bitmap,byte blackColorIndex,Rectangle targetRect)
        {
            Bitmap retbitmap = ImageUtil.Blank(bitmap.Width, bitmap.Height,bitmap);
            int bitmapsize = bitmap.Width * bitmap.Height;
            Rectangle rect = new Rectangle(new Point(), retbitmap.Size);

            BitmapData srcbmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr src = srcbmpData.Scan0;

            BitmapData destbmpData = retbitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            int width = rect.Width;
            int height = rect.Height;
            int endWidthMinus1 = targetRect.X + targetRect.Width - 1;
            int endHeightMinus1 = targetRect.Y + targetRect.Height - 1;

            U.CopyMemory(dest, src, (uint)bitmapsize);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte cc = FuchidoriGetFillIndex(
                      x,y
                    , src ,srcbmpData
                    , dest,destbmpData
                    , targetRect.X, targetRect.Y, endWidthMinus1, endHeightMinus1
                    , blackColorIndex);
                    //ぬりぬり
                    int d = x + destbmpData.Stride * y;
                    Marshal.WriteByte(dest, d, cc);
                }
            }

            retbitmap.UnlockBits(destbmpData);
            bitmap.UnlockBits(srcbmpData);
            return retbitmap;
        }
        //縁取りするべきか?
        static byte FuchidoriGetFillIndex(
              int x,int y
            , IntPtr src ,BitmapData srcbmpData
            , IntPtr dest,BitmapData destbmpData
            , int startX,int startY,int endWidthMinus1,int endHeightMinus1
            , byte blackColorIndex)
        {
            byte cc = Marshal.ReadByte(src, x + srcbmpData.Stride * y);
            if (x < startX
                || x > endWidthMinus1
                || y < startY
                || y > endHeightMinus1
                )
            {
                //指定範囲外なので縁取りしない.
                return cc;
            }


            if (cc <= 0)
            {//透明
                return 0;
            }

            int TransCount = 0;

            //左
            bool isLTrans = false;
            bool isLBlack = false;
            if (x > startX)
            {
                byte c = Marshal.ReadByte(dest, (x - 1) + destbmpData.Stride * y);
                if (c == blackColorIndex)
                {
                    isLBlack = true;
                }
                c = Marshal.ReadByte(src, (x - 1) + srcbmpData.Stride * y);
                if (c == 0)
                {
                    isLTrans = true;
                    TransCount ++;
                }
            }
            //右
            bool isRTrans = false;
            bool isRBlack = false;
            if (x < endWidthMinus1)
            {
                byte c = Marshal.ReadByte(dest, (x + 1) + destbmpData.Stride * y);
                if (c == blackColorIndex)
                {
                    isRBlack = true;
                }
                c = Marshal.ReadByte(src, (x + 1) + srcbmpData.Stride * y);
                if (c == 0)
                {
                    isRTrans = true;
                    TransCount++;
                }
            }

            //上
            bool isUTrans = false;
            bool isUBlack = false;
            if (y > startY)
            {
                byte c = Marshal.ReadByte(dest, x + destbmpData.Stride * (y - 1));
                if (c == blackColorIndex)
                {
                    isUBlack = true;
                }
                c = Marshal.ReadByte(src, x + srcbmpData.Stride * (y - 1));
                if (c == 0)
                {
                    isUTrans = true;
                    TransCount++;
                }
            }
            //下
            bool isDTrans = false;
            bool isDBlack = false;
            if (y < endHeightMinus1)
            {
                byte c = Marshal.ReadByte(dest, x + destbmpData.Stride * (y + 1));
                if (c == blackColorIndex)
                {
                    isDBlack = true;
                }
                c = Marshal.ReadByte(src, x + srcbmpData.Stride * (y + 1));
                if (c == 0)
                {
                    isDTrans = true;
                    TransCount++;
                }
            }

            if (TransCount >= 3)
            {//3方向が透明
                return cc;
            }

            if (isLTrans || isRTrans)
            {//左右が透明
                if (isUBlack == false || isDBlack == false)
                {//上下は黒ではない
                    return blackColorIndex;//ふちと認定して黒を塗る.
                }
            }
            if (isUTrans || isDTrans)
            {//上下が透明
                if (isLBlack == false || isRBlack == false)
                {//左右は黒ではない
                    return blackColorIndex;//ふちと認定して黒を塗る.
                }
            }
            return cc;
        }

        static public int FindPaletteIndexByColor(Bitmap bitmap, Color color)
        {
            ColorPalette pal = bitmap.Palette;
            if (pal == null)
            {
                return -1;
            }
            for (int i = 0; i < pal.Entries.Length; i++)
            {
                if (pal.Entries[i].R == color.R
                    && pal.Entries[i].G == color.G
                    && pal.Entries[i].B == color.B
                    )
                {
                    return i;
                }
            }
            return -1;
        }

        //もっとも黒い色をパレットから探します.
        static public int FindBlackColorFromPalette(Bitmap bitmap,int start,int end)
        {
            Color mostBlack = Color.FromArgb(255, 255, 255);
            int colorIndex = start;

            ColorPalette pal = bitmap.Palette;
            for (int i = start; i < pal.Entries.Length; i++)
            {
                if (i >= end)
                {
                    break;
                }
                Color c  = pal.Entries[i];

                if (c.R <= mostBlack.R && c.G <= mostBlack.G && c.B <= mostBlack.B)
                {
                    mostBlack = c;
                    colorIndex = i;
                }
            }
            return colorIndex;
        }

        //顔画像で利用できない場所を透明クリアする.
        public static Bitmap PortraitSideMask(Bitmap bitmap
            , int tileWidth
            , int tileHeight
            )
        {
            Bitmap retBitmap = new Bitmap(bitmap.Width,bitmap.Height);
            U.MakeTransparent(retBitmap);
            Graphics g = Graphics.FromImage(retBitmap);

            Color c = bitmap.GetPixel(127,0);
            SolidBrush WakuBrush = new SolidBrush(Color.FromArgb(255, c.R, c.G, c.B));
            g.DrawImage(bitmap, 0, 0);
            g.FillRectangle(WakuBrush
                , (int)(tileWidth * 0)
                , (int)(tileHeight * 0)
                , tileWidth / 2
                , tileHeight * 3
            );
            g.FillRectangle(WakuBrush
                , (int)(tileWidth * 3) - (tileWidth / 2)
                , (int)(tileHeight * 0)
                , tileWidth / 2
                , tileHeight * 3
            );

            g.Dispose();

            return retBitmap;
        }

        public static Bitmap OpenLowBitmap(string imagefilename)
        {
            if (!File.Exists(imagefilename))
            {
                R.ShowStopError(R._("ファイルがありません。\r\nファイル名:{0}"), imagefilename);
                return null;
            }

            Bitmap bitmap;
            try
            {
                FileStream fs = new System.IO.FileStream(
                    imagefilename,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                Bitmap tempBitmap = new Bitmap(fs);
                fs.Close();

                //FileStreamだとパレットにアクセスした場合などでエラーが発生することがあるので、
                //安全を取るために、バイナリレベルでコピーする.
                bitmap = CloneBitmap(tempBitmap);
                tempBitmap.Dispose();
            }
            catch (ArgumentException e)
            {
                R.ShowStopError(("ファイルを画像として読みこめませんでした。\r\nファイル名:{0}\r\n{1}"), imagefilename, e.ToString());
                return null;
            }
            if (bitmap == null)
            {
                R.ShowStopError(R._("ファイルがありません。\r\nファイル名:{0}"), imagefilename);
                return null;
            }
            return bitmap;
        }

        //背景色は0番目パレットですか?
        public static bool CheckPaletteTransparent(Bitmap bitmap)
        {
            if (bitmap.Palette.Entries.Length < 1 || bitmap.Width < 8 || bitmap.Height < 8)
            {//それ以前の問題.
                return false;
            }
            Color c = bitmap.Palette.Entries[0];
            Color cc = bitmap.GetPixel(bitmap.Width - 1, 0);
            if (c == cc)
            {//ok
                return true;
            }
            cc = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1);
            if (c == cc)
            {//ok
                return true;
            }
            cc = bitmap.GetPixel(0, 0);
            if (c == cc)
            {//ok
                return true;
            }
            //NG
            return false;
        }
        public static bool IsFullColorBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed
                || bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed
                )
            {
                return false;
            }
            return true;
        }
        public static bool Is16ColorBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed)
            {//16色
                return true;
            }
            else if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {//256色
                int palette = ImageUtil.GetPalette16Count(bitmap);
                if (palette <= 1)
                {//16色
                    return true;
                }
                //パレットだけど16色をこえる
                return false;
            }
            else
            {//フルカラー
                int colors = ImageUtil.GetColorCount(bitmap);
                if (colors <= 16)
                {//フルカラーだけど16色
                    return true;
                }

                return false;
            }
        }

        //壊されたパレットを、元画像から復元します.
        //画像がパレットが違うだけで同一である前提です。
        public static Bitmap ReColorPaletteWithHint(Bitmap target,Bitmap hintBitmap)
        {
            ColorPalette hintpal = hintBitmap.Palette;
            Debug.Assert(hintpal.Entries.Length >= 16);

            Bitmap ret_bitmap = ImageUtil.CloneBitmap(target);

            //変更結果を記録する
            ColorPalette changepal = ret_bitmap.Palette;

            int pal_len = Math.Min(changepal.Entries.Length ,hintpal.Entries.Length);

            int width = target.Width;
            int height = target.Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color change_c = ret_bitmap.GetPixel(x, y);
                    Color hint_c = hintBitmap.GetPixel(x, y);

                    for (int n = 0; n < pal_len; n++)
                    {
                        if (hintpal.Entries[n] == hint_c)
                        {
                            changepal.Entries[n] = change_c;
                        }
                    }
                }
            }
            ret_bitmap.Palette = changepal;
            return ret_bitmap;
        }

        //グリッドを描画 Graphicsを利用するのでフルカラーになる.
        public static Bitmap DrawGrid(Bitmap target,Color lineColor, int size = 16)
        {
            Pen pen = new Pen(lineColor);
            int width = target.Width;
            int heiht = target.Height;

            Bitmap fullColor = (Bitmap)target.Clone(new Rectangle(0,0,target.Width , target.Height) , PixelFormat.Format32bppRgb);

            Graphics g = Graphics.FromImage(fullColor);
            for(int x = size ; x < width ; x += size)
            {
                g.DrawLine(pen, x, 0, x, heiht);
            }
            for (int y = size; y < width; y += size)
            {
                g.DrawLine(pen, 0, y, width , y);
            }
            g.Dispose();

            return fullColor;
        }

        public static uint CalcByteLengthForHeaderTSAData(byte[] data,int pos)
        {
            if (pos + 2 >= data.Length)
            {//無効
                return 0;
            }
            uint master_headerx = (uint)(data[pos]) + 1;
            uint master_headery = (uint)(data[pos+1]) + 1;
            return 2 + (master_headerx * master_headery * 2);
        }

        public static Size CalcSizeForHeaderTSAData(byte[] data, int pos)
        {
            if (pos + 2 >= data.Length)
            {//無効
                return new Size(0,0);
            }
            uint master_headerx = (uint)(data[pos]) + 1;
            uint master_headery = (uint)(data[pos + 1]) + 1;
            return new Size((int)master_headerx, (int)master_headery);
        }

        public static Bitmap ConvertSizeFormat(Bitmap bitmap,int width,int height)
        {
            Debug.Assert(width % 8 == 0);
            Debug.Assert(height % 8 == 0);
            if (bitmap.Width % 8 != 0 || bitmap.Height % 8 != 0)
            {//8の倍数ではない.
                return null;
            }

            Bitmap ret = ImageUtil.Blank(width, height, bitmap);
            int srcx  = 0;
            int srcy  = 0;
            for (int desty = 0; desty < height; desty += 8)
            {
                for (int destx = 0; destx < width; destx += 8)
                {
                    ImageUtil.BitBlt(ret, destx, desty, 8, 8, bitmap, srcx, srcy);
                    srcx += 8;
                    if (srcx >= bitmap.Width)
                    {
                        if (srcy >= bitmap.Height)
                        {//もうデータがない.
                            ret.Dispose();
                            return null;
                        }
                        srcx = 0;
                        srcy += 8;
                    }
                }
            }
            return ret;
        }

        public static string SaveDummyImage(
              string basedir
            , string basename
            , int width
            , int height
            )
        {
            Bitmap dummy = ImageUtil.Blank(width, height);
            ImageUtil.BlackOutUnnecessaryColors(dummy);
            string path = Path.Combine(basedir,basename);
            U.BitmapSave(dummy, path);
            dummy.Dispose();

            return path;
        }

    }
}
