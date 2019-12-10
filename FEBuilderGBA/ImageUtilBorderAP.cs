using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    static class ImageUtilBorderAP
    {
        public static Bitmap DrawBorderImages(Bitmap parts, uint ap_addr, int x, int y)
        {
            ap_addr = U.toOffset(ap_addr);
            ImageUtilAP ap = new ImageUtilAP();
            bool r = ap.Parse(ap_addr);
            if (!r)
            {
                return null;
            }

            Bitmap borderimg = ImageUtil.Blank(256, 160, parts);
            ap.DrawFrame(borderimg, 0, x, y , parts);
            ap.DrawFrame(borderimg, 1, x, y , parts);

            Bitmap worldmap = ImageUtilMap.DrawWorldMapEvent();
            Bitmap retScreen = new Bitmap(256, 160);
            using (Graphics g = Graphics.FromImage(retScreen))
            {
                g.DrawImage(worldmap, 0, 0);

                U.MakeTransparent(borderimg);
                g.DrawImage(borderimg, 0, 0);
                borderimg.Dispose();
            }
            return retScreen;
        }
        public static Bitmap DrawBorderImages(uint image_addr,uint ap_addr,int x, int y)
        {
            image_addr = U.toOffset(image_addr);
            Bitmap parts = DrawBorderBitmap(image_addr);
            return DrawBorderImages(parts, ap_addr, x, y);
        }
        public static Bitmap DrawBorderBitmap(uint img)
        {
            byte[] bin = LZ77.decompress(Program.ROM.Data, U.toOffset(img));
            uint pal = Program.ROM.p32(Program.ROM.RomInfo.worldmap_county_border_palette_pointer());

            int height = ImageUtil.CalcHeight(32 * 8, bin.Length);
            if (height <= 0)
            {
                return ImageUtil.Blank(32 * 8, 4 * 8);
            }
            return ImageUtil.ByteToImage16Tile(32 * 8, height
                , bin, 0
                , Program.ROM.Data, (int)pal);
        }
        public static void SaveAPImages(string filename, uint image_addr,uint ap_addr,int x, int y)
        {
            image_addr = U.toOffset(image_addr);
            Bitmap parts = DrawBorderBitmap(image_addr);

            ap_addr = U.toOffset(ap_addr);
            ImageUtilAP ap = new ImageUtilAP();
            bool r = ap.Parse(ap_addr);
            if (!r)
            {
                return;
            }

            Bitmap borderimg = ImageUtil.Blank(256, 160, parts);
            ap.DrawFrame(borderimg, 0, x,y, parts);
            string export_filename = ImageFormRef.ExportImage(null, borderimg, filename);

            string name_filename = MakeBorderNameImageFileName(export_filename);
            borderimg = ImageUtil.Blank(256, 160, parts);
            ap.DrawFrame(borderimg, 1, x, y, parts);
            ImageUtil.BlackOutUnnecessaryColors(borderimg, 1);
            borderimg.Save(name_filename);
        }

        static string MakeBorderNameImageFileName(string border_filename)
        {
            string basedir = Path.GetDirectoryName(border_filename);
            string filename = Path.GetFileNameWithoutExtension(border_filename);
            string ext = Path.GetExtension(border_filename);

            return Path.Combine(basedir , filename + "_NAME" + ext);
        }
        public static void ImportBorder(Form self)
        {
            string imagefilename = ImageFormRef.OpenFilenameDialogFullColor(self);
            if (imagefilename == "")
            {
                R.ShowStopError("ファイルがありません。\r\nファイル名:{0}", imagefilename);
                return;
            }
            string name_filename = MakeBorderNameImageFileName(imagefilename);
            if (! File.Exists(name_filename))
            {
                R.ShowStopError("ファイルがありません。\r\nファイル名:{0}", name_filename);
                return;
            }
            string basedir = Path.GetDirectoryName(imagefilename);

            ImageUtilOAM.ImportOAM oam = new ImageUtilOAM.ImportOAM();
            oam.SetIsBorderAPOAM(true);
            oam.SetBaseDir(basedir);
            oam.MakeBorderAP(imagefilename);
            uint oam1st = oam.GetOAMByteCount();
            oam.MakeBorderAP(name_filename);
            uint oam2nd = oam.GetOAMByteCount();
            oam.Term();

            List<ImageUtilOAM.image_data> images = oam.GetImages();
            if (images.Count >= 2)
            {
                R.ShowStopError("画像が大きすぎて、256x160のシートに入りきりませんでした");
                return ;
            }

            byte[] battleAOM = oam.GetRightToLeftOAM();
            if (battleAOM.Length <= 1)
            {
                return;
            }
            byte[] apOAM = BattleOAMToAPOAM(battleAOM);
        }
        //戦闘アニメの12バイトOAMデータを、APの12バイトOAMに変換します
        static byte[] BattleOAMToAPOAM(byte[] battle)
        {
            Debug.Assert(battle.Length % 12 == 0);   
            byte[] ret = new byte[battle.Length];

            int shiftX = 0;
            int shiftY = 0;
            //最大描画範囲を取得
            Rectangle MaxRC = BattleOAMMaxRectngle(battle);
            if (MaxRC.Height >= 0x80)
            {//AP OAMには、Y軸を0x80までしか格納できないので、それ以降は原点をずらすしかない。
                shiftY  = MaxRC.Height - 0x80;
            }

            for (int i = 0; i < battle.Length - 1; i += 12)
            {
                uint oam0 = 0;
                uint oam1 = 0;
                uint oam2 = 0;

                ImageUtilAP.OAMParse ap = new ImageUtilAP.OAMParse();
                int x = (short)U.u16(battle, (uint)i + 6);
                int y = (short)U.u16(battle, (uint)i + 8);
                sbyte image_x = (sbyte)(x - MaxRC.Left - shiftX);
                sbyte image_y = (sbyte)(y - MaxRC.Top  - shiftY);
                uint tile = battle[i + 4];
                ap.tile = tile;

                oam0 |= (uint)((battle[1] & 0x3) << 14);
                oam1 |= (uint)((battle[3] & 0x3) << 14);

                oam1 |= (uint)(image_x & 0x1FF);
                oam0 |= (uint)(image_y & 0x0FF);

                oam2 |= (tile & 0x3FF);

                U.write_u32(ret, (uint)i, oam0);
                U.write_u32(ret, (uint)i + 4, oam1);
                U.write_u32(ret, (uint)i + 8, oam2);
            }
            return ret;
        }

        static Rectangle BattleOAMMaxRectngle(byte[] battle)
        {
            int xTop = 256;
            int yTop = 160;
            int xButtom = 0;
            int yButtom = 0;
            for (int i = 0; i < battle.Length; i += 12)
            {
                int x = (short)U.u16(battle,(uint)i + 6);
                if (x < xTop)
                {
                    xTop = x;
                }
                if (x > xButtom)
                {
                    xButtom = x;
                }
                int y = (short)U.u16(battle, (uint)i + 8);
                if (y < yTop)
                {
                    yTop = y;
                }
                if (y > yButtom)
                {
                    yButtom = y;
                }
            }
            return new Rectangle(xTop, xTop, xButtom - xTop, yButtom - yTop);
        }
    }
}
