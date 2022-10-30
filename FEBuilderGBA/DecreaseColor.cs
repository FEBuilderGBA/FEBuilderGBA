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
    class DecreaseColor
    {
        class ColorRanking
        {
            public int R { get; private set; }
            public int G { get; private set; }
            public int B { get; private set; }
//            public double Yc { get; private set; }
//            public double Ycb { get; private set; }
//            public double Ycr { get; private set; }
            public int Count;
            public int PaletteNumber;

            public ColorRanking()
            {
            }
            public ColorRanking(int r,int g,int b)
            {
                SetRGB(r, g, b);
                Count = 1;
                PaletteNumber = -1;
            }
            public ColorRanking Clone()
            {
                ColorRanking a = new ColorRanking();
                a.R = this.R;
                a.G = this.G;
                a.B = this.B;
//                a.Yc  = this.Yc;
//                a.Ycb = this.Ycb;
//                a.Ycr = this.Ycr;
                a.Count = this.Count;
                a.PaletteNumber = this.PaletteNumber;
                return a;
            }
            public void SetRGB(int r, int g, int b)
            {
                R = r;
                G = g;
                B = b;
                //RGB to YCbCr 
                //Y =   0.257R + 0.504G + 0.098B + 16
                //Cb = -0.148R - 0.291G + 0.439B + 128
                //Cr =  0.439R - 0.368G - 0.071B + 128
                //YCbCr to RGB 
                //R = 1.164(Y-16)                 + 1.596(Cr-128)
                //G = 1.164(Y-16) - 0.391(Cb-128) - 0.813(Cr-128)
                //B = 1.164(Y-16) + 2.018(Cb-128)
                //this.Yc  = 0.257 * (double)r + 0.504 * (double)g + 0.098 * (double)b + 16;
                //this.Ycb = -0.148 * (double)r - 0.291 * (double)g + 0.439 * (double)b + 128;
                //this.Ycr =  0.439 * (double)r - 0.368 * (double)g - 0.071 * (double)b + 128;


            }
        };
        class TileMapping
        {
            public int Palette;
            public List<ColorRanking> Rank;
            public int X;
            public int Y;
        };

        public Bitmap Convert(Bitmap src, int maxPalette, int yohaku, bool isReserve1StPalette, bool ignoreTSA)
        {
            if (maxPalette == 1 || ignoreTSA)
            {//1パレット16色 または、 TSA無効の場合
                return ConvertIgnoreTSA(src, maxPalette * 16, yohaku, isReserve1StPalette , maxPalette < 16);
            }

            int width = src.Width;
            int height = src.Height;
            width = U.Padding8(width);
            height = U.Padding8(height);
            Bitmap DestBitmap = ImageUtil.Blank(width + yohaku, height);

            List<ColorRanking> totalRank = new List<ColorRanking>();
            List<TileMapping> tileMapping = new List<TileMapping>();
            for (int y = 0; y < height; y += 8)
            {
                if (y + 8 > height)
                {
                    continue;
                }
                for (int x = 0; x < width; x += 8)
                {
                    if (x + 8 > width)
                    {
                        continue;
                    }

                    TileMapping tm = new TileMapping();
                    tm.Rank = new List<ColorRanking>();
                    tm.Palette = -1;
                    tm.X = x;
                    tm.Y = y;
                    for (int yy = 0; yy < 8; yy++)
                    {
                        for (int xx = 0; xx < 8; xx++)
                        {
                            try
                            {
                                Color c = src.GetPixel(x + xx, y + yy);
                                if (c.A == 0)
                                {
                                    continue;
                                }
                                VoteColor(tm.Rank, c);
                            }
                            catch (System.AccessViolationException)
                            {//なんで例外が飛んでくるんだ?
                                break;
                            }
                        }
                    }
                    SortColor(tm.Rank);
                    tileMapping.Add(tm);

                    //複数パレットの場合、そのタイルの中で最も人気の色
                    if (tm.Rank.Count > 0)
                    {
                        VoteColor(totalRank, tm.Rank[0]);
                    }
                }
            }
            SortColor(totalRank);

            //上位の色たちをパレットに割り当てていきます.
            List<List<ColorRanking>> countList = new List<List<ColorRanking>>();
            for (int i = 0; i < maxPalette; i++)
            {
                List<ColorRanking> pal = AssingPalette(i, totalRank, tileMapping);
                countList.Add(pal);
            }
            //まだ割り当てていないタイルがあれば、一番近い色セットを持つどれかのパレットに割り当てます.
            AssingaPaletteByUnassignedTile(countList, tileMapping, totalRank);

            //パレットの色数を16色にします.
            List<ColorRanking[]> paletteList = new List<ColorRanking[]>();
            for (int i = 0; i < maxPalette; i++)
            {
                paletteList.Add(Convert16Color(countList[i], isReserve1StPalette));
            }

            //パレットの適応
            ColorPalette cp = DestBitmap.Palette;
            for (int i = 0; i < maxPalette; i++)
            {
                for(int n = 0 ; n < 16 ; n++)
                {
                    cp.Entries[i * 16 + n] = Color.FromArgb(paletteList[i][n].R
                        ,paletteList[i][n].G
                        ,paletteList[i][n].B
                        );
                }
            }
            //利用しないところはゼロクリア
            for (int i = maxPalette; i < 16; i++)
            {
                for (int n = 0; n < 16; n++)
                {
                    cp.Entries[i * 16 + n] = Color.FromArgb(0,0,0);
                }
            }
            DestBitmap.Palette = cp;

            //ピクセルの変換
            Rectangle destrect = new Rectangle(new Point(), DestBitmap.Size);
            BitmapData destbmpData = DestBitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            for (int i = 0; i < tileMapping.Count; i++)
            {
                int paletteno = tileMapping[i].Palette;
                if (paletteno < 0)
                {
                    Debug.Assert(false);
                    continue;
                }
                int x = tileMapping[i].X;
                int y = tileMapping[i].Y;
                for (int yy = 0; yy < 8; yy++)
                {
                    for (int xx = 0; xx < 8; xx++)
                    {
                        int index;

                        try
                        {
                            Color c = src.GetPixel(x + xx, y + yy);
                            if (c.A == 0)
                            {
                                index = 0;
                            }
                            else
                            {
                                index = getPaletteIndex(c, countList[paletteno]);
                            }
                        }
                        catch (System.AccessViolationException)
                        {//なんで例外が飛んでくるんだ?
                            break;
                        }

                        int pos = (x + xx) + ((y + yy) * destrect.Width);
                        byte cc = (byte)(paletteno * 16 + index);
                        Marshal.WriteByte(dest, pos, cc);
                    }
                }
            }

            DestBitmap.UnlockBits(destbmpData);
            return DestBitmap;
        }

        Bitmap ConvertIgnoreTSA(Bitmap src, int maxColor , int yohaku, bool isReserve1StPalette,bool isUseTransparent)
        {
            int width = src.Width;
            int height = src.Height;
            width = U.Padding8(width);
            height = U.Padding8(height);
            Bitmap DestBitmap = ImageUtil.Blank(width + yohaku, height);

            List<ColorRanking> totalRank = new List<ColorRanking>();
            List<TileMapping> tileMapping = new List<TileMapping>();
            for (int y = 0; y < height; y += 8)
            {
                if (y + 8 > height)
                {
                    continue;
                }
                for (int x = 0; x < width; x += 8)
                {
                    if (x + 8 > width)
                    {
                        continue;
                    }

                    TileMapping tm = new TileMapping();
                    tm.Rank = new List<ColorRanking>();
                    tm.Palette = -1;
                    tm.X = x;
                    tm.Y = y;
                    for (int yy = 0; yy < 8; yy++)
                    {
                        for (int xx = 0; xx < 8; xx++)
                        {
                            try
                            {
                                Color c = src.GetPixel(x + xx, y + yy);
                                if (c.A == 0 && isUseTransparent)
                                {
                                    continue;
                                }
                                VoteColor(tm.Rank, c);
                            }
                            catch (System.AccessViolationException)
                            {//なんで例外が飛んでくるんだ?
                                break;
                            }
                        }
                    }
                    SortColor(tm.Rank);
                    tileMapping.Add(tm);

                    for (int i = 0; i < tm.Rank.Count; i++)
                    {
                        VoteColor(totalRank, tm.Rank[i]);
                    }
                }
            }
            SortColor(totalRank);

            //パレットの色数を16色 or 256色にします.
            List<ColorRanking> paletteList = new List<ColorRanking>();
            paletteList.AddRange(Convert16Color(totalRank, isReserve1StPalette, maxColor));

            //パレットの適応
            ColorPalette cp = DestBitmap.Palette;
            for (int i = 0; i < maxColor; i++)
            {
                cp.Entries[i] = Color.FromArgb(
                      paletteList[i].R
                    , paletteList[i].G
                    , paletteList[i].B
                    );
            }
            //利用しないところはゼロクリア
            for (int i = maxColor; i < 16 * 16; i++)
            {
                cp.Entries[i] = Color.FromArgb(0, 0, 0);
            }
            DestBitmap.Palette = cp;

            int startColor = 0;
            if (isReserve1StPalette && isUseTransparent == false)
            {//最初の色が背景なのに、背景色を使わないということは、color index==0は予約されている
                startColor = 1;
            }

            //ピクセルの変換
            Rectangle destrect = new Rectangle(new Point(), DestBitmap.Size);
            BitmapData destbmpData = DestBitmap.LockBits(destrect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr dest = destbmpData.Scan0;

            for (int i = 0; i < tileMapping.Count; i++)
            {
                int x = tileMapping[i].X;
                int y = tileMapping[i].Y;
                for (int yy = 0; yy < 8; yy++)
                {
                    for (int xx = 0; xx < 8; xx++)
                    {
                        int index;

                        try
                        {
                            Color c = src.GetPixel(x + xx, y + yy);
                            if (c.A == 0 && isUseTransparent)
                            {
                                index = 0;
                            }
                            else
                            {
                                index = getPaletteIndex(c, paletteList, startColor);
                            }
                        }
                        catch (System.AccessViolationException)
                        {//なんで例外が飛んでくるんだ?
                            break;
                        }

                        int pos = (x + xx) + ((y + yy) * destrect.Width);
                        byte cc = (byte)(index);
                        Marshal.WriteByte(dest, pos, cc);
                    }
                }
            }

            DestBitmap.UnlockBits(destbmpData);
            return DestBitmap;
        }


        //色からパレット変換
        int getPaletteIndex(Color c,List<ColorRanking> rank, int startColor = 0)
        {
            int r = ((c.R >> 3) << 3);
            int g = ((c.G >> 3) << 3);
            int b = ((c.B >> 3) << 3);
            for (int i = startColor; i < rank.Count; i++)
            {
                if (rank[i].R == r
                    && rank[i].G == g
                    && rank[i].B == b
                    )
                {
                    return rank[i].PaletteNumber;
                }
            }

            //ありえないはずなのだが一応
            int best = 0;
            int min_score = Int32.MaxValue;
            for (int i = startColor; i < rank.Count; i++)
            {
                int score = CalcColorScore(rank[i],r,g,b);
                if (score < min_score)
                {
                    min_score = score;
                    best = i;
                }
            }
            return best;
        }

        //パレットを16色にします.
        static ColorRanking[] Convert16Color(List<ColorRanking> countList, bool isReserve1StPalette,int maxColor = 16)
        {
            SortColor(countList);

            ColorRanking[] center = new ColorRanking[maxColor];
            int pal16;
            int first;
            if (isReserve1StPalette)
            {//最初のパレットが背景色で予約されている場合
                pal16 = maxColor - 1;
                first = 1;
                center[0] = new ColorRanking();
            }
            else
            {
                pal16 = maxColor;
                first = 0;
            }


            if (countList.Count < pal16)
            {//16色以下しかないなら、それで確定.
                for (int i = 0; i < countList.Count; i++)
                {
                    countList[i].PaletteNumber = i + first;
                    center[i + first] = countList[i].Clone();
                }
                for (int i = countList.Count; i < pal16; i++)
                {
                    center[i + first] = new ColorRanking();
                }
                return center;
            }

            //k-means法で16色にクラスタ化していきます。
            for (int k = 0; k < pal16; k++)
            {
                countList[k].PaletteNumber = k + first;
                center[k + first] = countList[k].Clone();
            }

            while(true)
            {
                //Assignment処理
                //一番近い中心点に所属を変えていく.
                bool isUpdate = false;
                for (int i = 0; i < countList.Count; i++)
                {
                    int best = 0;
                    int min_score = Int32.MaxValue;
                    for (int k = 0; k < pal16; k++)
                    {
                        int kk = k + first;
                        int score = CalcColorScore(center[kk], countList[i]);
                        if (score < min_score)
                        {
                            min_score = score;
                            best = kk;
                        }
                    }
                    if (countList[i].PaletteNumber != best)
                    {
                        countList[i].PaletteNumber = best;
                        isUpdate = true;
                    }
                }
                if (isUpdate == false)
                {//クラスタに変化がないので終了.
                    break;
                }

                //Update 
                //クラスタの中心点の移動.
                for (int k = 0; k < pal16; k++)
                {
                    int kk = k + first;
                    UInt64 r = 0;
                    UInt64 g = 0;
                    UInt64 b = 0;
                    UInt64 count = 0;
                    for (int i = 0; i < countList.Count; i++)
                    {
                        if (countList[i].PaletteNumber != kk)
                        {
                            continue;
                        }

                        count += (UInt64)countList[i].Count;
                        r += (UInt64)(countList[i].R * countList[i].Count);
                        g += (UInt64)(countList[i].G * countList[i].Count);
                        b += (UInt64)(countList[i].B * countList[i].Count);
                    }

                    if (count == 0)
                    {
                        center[kk].SetRGB(0,0,0);
                    }
                    else
                    {
                        center[kk].SetRGB((int)(r / count), (int)(g / count), (int)(b / count));
                    }
                }
            }
            return center;
        }

        //まだ割り当てていないタイルがあれば、一番近い色セットを持つどれかのパレットに割り当てます.
        void AssingaPaletteByUnassignedTile(List<List<ColorRanking>> countList, List<TileMapping> tileMapping, List<ColorRanking> totalRank)
        {
            for (int i = 0; i < tileMapping.Count; i++)
            {
                if (tileMapping[i].Palette >= 0)
                {//既に割り当て済み
                    continue;
                }
                int pal = BestPalette(countList, tileMapping[i].Rank);
                tileMapping[i].Palette = pal;

                //タイルの色をすべてパレットに追加. ついでに、色ランキングから消去.
                InsertColorRank(countList[pal], tileMapping[i].Rank, totalRank);
            }
        }


        //一番近いパレットを探す
        int BestPalette(List<List<ColorRanking>> countList, List<ColorRanking> rank)
        {
            int best_pal = 0;
            int min_score = Int32.MaxValue;
            for (int i = 0; i < countList.Count; i++)
            {
                int score = CalcPaletteScore(countList[i], rank);
                if (score < min_score)
                {
                    min_score = score;
                    best_pal = i;
                }
            }
            return best_pal;
        }

        //パレット間の類似スコアを計算します. 小さい方がよい結果です
        int CalcPaletteScore(List<ColorRanking> palette,List<ColorRanking> rank)
        {
            uint count = 0;
            uint total_score = 0;
            for (int i = 0; i < rank.Count; i++)
            {
                int min_score = Int32.MaxValue;
                for (int n = 0; n < palette.Count; n++)
                {
                    int score = CalcColorScore(rank[i], palette[n]);
                    if (score < min_score)
                    {
                        min_score = score;
                    }
                }
                count += (uint)(rank[i].Count);
                total_score += (uint)(min_score * rank[i].Count);
            }
            if (count <= 0)
            {
                return 0;
            }
            return (int)(total_score / count);
        }

        //色がどれだけ似ていないかを求めます. 0は同一 数値が大きなればなるほど似ていません.
        static int CalcColorScore(ColorRanking c, ColorRanking c2)
        {
            return CalcColorScore(c, c2.R, c2.G, c2.B);
//            return CalcColorScoreYCbCr(c, c2.Yc, c2.Ycb, c2.Ycr);
        }
        //三平方の定理で、類似度を判定します.
        static int CalcColorScore(ColorRanking c, int r2, int g2, int b2)
        {
//            ColorRanking c2 = new ColorRanking(r2,g2,b2);
//            return CalcColorScoreYCbCr(c, c2.Yc, c2.Ycb, c2.Ycr);
            int r = c.R - r2;
            int g = c.G - g2;
            int b = c.B - b2;
            return (int)Math.Sqrt((r * r) + (g * g) + (b * b));
        }

        //三平方の定理で、類似度を判定します.(YCbCr)
//        static int CalcColorScoreYCbCr(ColorRanking c, double Yc, double Ycb, double Ycr)
//        {
//            double r = c.Yc - Yc;
//            double g = c.Ycb - Ycb;
//            double b = c.Ycr - Ycr;
//            return (int)Math.Sqrt((r * r) + (g * g) + (b * b));
//        }

        List<ColorRanking> AssingPalette(int palette, List<ColorRanking> totalRank, List<TileMapping> tileMapping)
        {
            List<ColorRanking> pal = new List<ColorRanking>();
            if (totalRank.Count <= 0)
            {//もう未割当の色はない.
                return pal;
            }

            //最も利用されている色を取り出す.
            ColorRanking topC = totalRank[0];
            totalRank.RemoveAt(0);

            //この色が使われているタイルの処理.
            for (int i = 0; i < tileMapping.Count; i++)
            {
                int found = FindColor(tileMapping[i].Rank, topC);
                if (found >= 0)
                {
                    //タイルの割り当て
                    tileMapping[i].Palette = palette;
                    //タイルの色をすべてパレットに追加. ついでに、色ランキングから消去.
                    InsertColorRank(pal, tileMapping[i].Rank, totalRank);
                }
            }
            return pal;
        }
        void InsertColorRank(List<ColorRanking> pal, List<ColorRanking> rank, List<ColorRanking> totalRank)
        {
            for (int i = 0; i < rank.Count; i++)
            {
                int found = FindColor(pal, rank[i]);
                if (found >= 0)
                {
                    pal[found].Count += rank[i].Count;
                }
                else
                {
                    ColorRanking c = rank[i].Clone();

                    pal.Add(c);
                }

                //割り当てた色は、色ランキングから消す.
                found = FindColor(totalRank, rank[i]);
                if (found >= 0)
                {
                    totalRank.RemoveAt(found);
                }
            }
        }

        static void SortColor(List<ColorRanking> rank)
        {
            rank.Sort((a, b) => { return (b.Count) - (a.Count); });
        }

        int FindColor(List<ColorRanking> rank, ColorRanking c)
        {
            for (int i = 0; i < rank.Count; i++ )
            {
                if (rank[i].R == c.R && rank[i].G == c.G && rank[i].B == c.B)
                {
                    return i;
                }
            }
            return -1;
        }

        void VoteColor(List<ColorRanking> rank, int r,int g,int b)
        {
            for (int i = 0; i < rank.Count; i++)
            {
                if (rank[i].R == r && rank[i].G == g && rank[i].B == b)
                {
                    rank[i].Count++;
                    return;
                }
            }
            ColorRanking rr = new ColorRanking(r,g,b);
            rank.Add(rr);
        }
        void VoteColor(List<ColorRanking> rank, ColorRanking c)
        {
            VoteColor(rank, c.R, c.G, c.B);
        }
        void VoteColor(List<ColorRanking> rank, Color c)
        {
            VoteColor(rank
                , (c.R >> 3) <<3
                , (c.G >> 3) <<3
                , (c.B >> 3) <<3
                );
        }

        public static void ForceConvertTSA(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(new Point(), bitmap.Size);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr adr = bmpData.Scan0;
            int width = bitmap.Width;
            int height = bitmap.Height;

            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < width; x += 8)
                {
                    uint palette = 255;
                    for (int y8 = 0; y8 < 8; y8++)
                    {
                        for (int x8 = 0; x8 < 8; x8++)
                        {
                            int n = (x + x8 + 0) + bmpData.Stride * (y + y8);
                            byte a = Marshal.ReadByte(adr, n);
                            uint selectpalette = (uint)(a / 16);
                            if (palette == 255)
                            {//初期値
                                palette = selectpalette;
                            }
                            else if (palette != selectpalette)
                            {//フォーマット違反 8x8セルの中で異なるパレットを使用してはいけない.
                                x8 = 8;
                                y8 = 8;
                                break;
                            }
                        }
                    }

                    if (palette == 255)
                    {
                        palette = 0;
                    }

                    for (int y8 = 0; y8 < 8; y8++)
                    {
                        for (int x8 = 0; x8 < 8; x8++)
                        {
                            int n = (x + x8 + 0) + bmpData.Stride * (y + y8);
                            byte a = Marshal.ReadByte(adr, n);
                            uint selectpalette = (uint)(a / 16);
                            if (palette != selectpalette)
                            {//フォーマット違反 8x8セルの中で異なるパレットを使用してはいけない.
                                a = (byte)(palette * 16);
                            }
                            Marshal.WriteByte(adr, n, a);
                        }
                    }
                }
            }

            bitmap.UnlockBits(bmpData);
        }
    }
}
