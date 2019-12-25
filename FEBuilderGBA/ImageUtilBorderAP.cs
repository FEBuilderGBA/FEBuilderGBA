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
        public static bool ImportBorder(Form self, out byte[] out_image,out byte[] out_oam)
        {
            out_image = null;
            out_oam = null;
            string imagefilename = ImageFormRef.OpenFilenameDialogFullColor(self);
            if (imagefilename == "")
            {
                R.ShowStopError("ファイルがありません。\r\nファイル名:{0}", imagefilename);
                return false;
            }
            string name_filename = MakeBorderNameImageFileName(imagefilename);
            if (! File.Exists(name_filename))
            {
                R.ShowStopError("ファイルがありません。\r\nファイル名:{0}", name_filename);
                return false;
            }
            string basedir = Path.GetDirectoryName(imagefilename);

            ImageUtilOAM.ImportOAM oam = new ImageUtilOAM.ImportOAM();
            oam.SetIsBorderAPOAM(true);
            oam.SetBaseDir(basedir);
            oam.MakeBorderAP(imagefilename);
            uint oam1st = oam.GetOAMByteCount();
            oam.MakeBorderAP(name_filename);
            oam.Term();
            uint oam2nd = oam.GetOAMByteCount();

            List<ImageUtilOAM.image_data> images = oam.GetImages();
            if (images.Count >= 2)
            {
                R.ShowStopError("画像が大きすぎて、256x160のシートに入りきりませんでした");
                return false;
            }

            byte[] battleAOM = oam.GetRightToLeftOAM();
            if (battleAOM.Length <= 1)
            {
                return false;
            }
            byte[] apOAM = battleAOM;
            List<byte> newOam = new List<byte>();
            //ap_data header
            U.append_u16(newOam, 4); //ap_data header SHORT (frame_list - ap_data)
            U.append_u16(newOam, 8); //ap_data header SHORT (anim_list - ap_data)
            //frame_list
            uint addr_frame_list = (uint)newOam.Count;
            U.append_u16(newOam, 0); //SHORT (frame_0 - frame_list)
            U.append_u16(newOam, 0); //SHORT (frame_1 - frame_list)
            //anim_list
            uint addr_anim_list = (uint)newOam.Count;
            U.append_u16(newOam, 0); //SHORT (anim_0 - anim_list)
            U.append_u16(newOam, 0); //SHORT (anim_1 - anim_list)
            //frame_0
            uint addr_frame_0 = (uint)newOam.Count;
            U.append_u16(newOam, oam1st / 12); // oam entries
            for (uint i = 0; i < oam1st; i ++)
            {
                U.append_u8(newOam, apOAM[i] ); // oam entries
            }
            //frame_1
            uint addr_frame_1 = (uint)newOam.Count;
            U.append_u16(newOam, (oam2nd - oam1st) / 12); // oam entries
            for (uint i = oam1st; i < oam2nd; i++)
            {
                U.append_u8(newOam, apOAM[i]); // oam entries
            }
            //anim_0:
            uint addr_anim_0 = (uint)newOam.Count;
            U.append_u16(newOam, 4); U.append_u16(newOam, 0);
            U.append_u16(newOam, 0);  U.append_u16(newOam, 0xffff);// loop back to start
            //anim_1:
            uint addr_anim_1 = (uint)newOam.Count;
            U.append_u16(newOam, 4); U.append_u16(newOam, 1);
            U.append_u16(newOam, 0); U.append_u16(newOam, 0xffff);// loop back to start

            out_oam = newOam.ToArray();
            //フレームの位置を書き込む
            U.write_u16(out_oam, 4, addr_frame_0 - addr_frame_list); //SHORT  (frame_0 - frame_list)
            U.write_u16(out_oam, 6, addr_frame_1 - addr_frame_list); //SHORT  (frame_1 - frame_list)
            U.write_u16(out_oam, 8, addr_anim_0 - addr_anim_list); //SHORT  (anim_0 - anim_list)
            U.write_u16(out_oam, 10, addr_anim_1 - addr_anim_list); //SHORT (anim_1 - anim_list)

            out_image = LZ77.decompress(images[0].data,0);
            return true;
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

            for (uint i = 0; i < battle.Length - 1; i += 12)
            {
                uint oam0 = 0;
                uint oam1 = 0;
                uint oam2 = 0;

                ImageUtilAP.OAMParse ap = new ImageUtilAP.OAMParse();
                int x = (short)U.u16(battle, i + 6);
                int y = (short)U.u16(battle, i + 8);
                sbyte image_x = (sbyte)(x - MaxRC.Left - shiftX);
                sbyte image_y = (sbyte)(y - MaxRC.Top  - shiftY);
                uint tile = battle[i + 4];
                uint tile_x = tile & 0x1F;
                uint tile_y = (tile & 0xE0) >> 5;
                ap.tile = tile_x + (tile_y * 31);

                oam0 |= (uint)(battle[i + 1] & 0xC0) ;
                oam1 |= (uint)(battle[i + 3] & 0xC0) ;

                oam1 |= (uint)(image_x & 0x1FF);
                oam0 |= (uint)(image_y & 0x0FF);

                oam2 |= (ap.tile & 0x3FF);

//                U.write_u32(ret, (uint)i, oam0);
//                U.write_u32(ret, (uint)i + 4, oam1);
//                U.write_u32(ret, (uint)i + 8, oam2);
                U.write_u32(ret, (uint)i, U.u32(battle,i));
                U.write_u32(ret, (uint)i+4, U.u32(battle,i+4));
                U.write_u32(ret, (uint)i+8, U.u32(battle,i+8));
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
            return new Rectangle(xTop, yTop, xButtom - xTop, yButtom - yTop);
        }
    }
}
