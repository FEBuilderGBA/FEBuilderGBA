using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Drawing;

namespace FEBuilderGBA
{
    class ImageUtilAP
    {
        class Anime
        {
            public uint Wait;
            public uint Frame;
        }

        class Frame
        {
            public uint OAM0;
            public uint OAM1;
            public uint OAM2;
        }

        class AnimeArr
        {
            public List<Anime> Animes;
        }
        List<AnimeArr> AnimeArray = new List<AnimeArr>();

        class FrameArr
        {
            public bool UseRorate;
            public uint CountRorate;
            public List<Frame> Frames;
        }
        List<FrameArr> FrameArray = new List<FrameArr>();

        public class OAMParse
        {
            public bool v_flipped;  // 垂直に反転する
            public bool h_flipped;  // 水平に反転する
            public int width;       // コピーするシートにあるオブジェクト パーツの幅
            public int height;      // コピーするシートにあるオブジェクト パーツの高さ
            public int image_x;     // 画面X
            public int image_y;     // 画面Y
            public uint tile;       // シート番号
            public int paletteShift; //利用するパレット0-3
            public OAMFlag oamFlag;
        };
        public enum OAMFlag
        {
            OAMFlag_None = 0,
            OAMFlag_Semi_Transparent = 1,
            OAMFlag_Obj_Window = 2,
            OAMFlag_Unk3 = 3,
        }

        static Size[][] SharpTable = new Size[][]{
             new Size[]{ new Size(1,1),new Size(2,2),new Size(4,4),new Size(8,8) } //square
            ,new Size[]{ new Size(2,1),new Size(4,1),new Size(4,2),new Size(8,4) } //horizontal
            ,new Size[]{ new Size(1,2),new Size(1,4),new Size(2,4),new Size(4,8) } //vertical
            ,new Size[]{ new Size(0,0),new Size(0,0),new Size(0,0),new Size(0,0) } //invalid
        };

        static OAMParse FrameToDump(Frame f)
        {//このルーチンは、stan の apdump.pyの成果を参考にしました。

            OAMParse p = new OAMParse();

            int sharp1 = (int)((f.OAM0 >> 14) & 0x3);
            int sharp2 = (int)((f.OAM1 >> 14) & 0x3);
            Size s = SharpTable[sharp1][sharp2];
            p.width = s.Width;
            p.height = s.Height;

            p.image_x = (int)(f.OAM1 & 0x1FF);
            p.image_y = (int)(f.OAM0 & 0x0FF);

            if ((p.image_x & 0x100) == 0x100)
            {
                p.image_x = (p.image_x & 0xFF) - 256;
            }
            if ((p.image_y & 0x80) == 0x80)
            {
                p.image_y = (p.image_y & 0x7F) - 128;
            }
            p.tile = (f.OAM2 & 0x3FF);
            p.paletteShift = (int)((f.OAM2 & 0xF000) >> 12);
            p.oamFlag = (OAMFlag)((f.OAM0 & 0x0C00) >> 10);
            p.v_flipped = (f.OAM1 & 0x2000) == 0x2000;	// 垂直に反転する
            p.h_flipped = (f.OAM1 & 0x1000) == 0x1000;	// 水平に反転する

            return p;
        }
        public Bitmap DrawFrame(Bitmap ret , int index, int originX, int originY, Bitmap parts)
        {
            if (index >= FrameArray.Count)
            {
                return ret;
            }

            int graphicsWidth = parts.Width / 8;

            FrameArr fa = FrameArray[index];
            for (int i = 0; i < fa.Frames.Count; i++)
            {
                Frame f = fa.Frames[i];
                OAMParse t = FrameToDump(f);

                int src = (int)(t.tile);
                int src_x = (src % graphicsWidth) * 8;
                int src_y = (src / graphicsWidth) * 8;

                ImageUtil.BitBlt(ret
                    , originX + t.image_x, originY + t.image_y
                    , t.width * 8
                    , t.height * 8
                    , parts
                    , src_x, src_y
                    , t.paletteShift
                    , 0
                    , t.v_flipped
                    , t.h_flipped
                );
//                Log.Debug("_" + index + "_" + i + "  " + t.image_x + "," + t.image_y + "@" + t.width * 8 + "," + t.height * 8);

//                ret.Save("_" + index + "_" + i + ".png");
            }
            return ret;
        }

        uint BaseAddr;
        uint Length;
        string ErrorMessage;

        public uint GetLength()
        {
            return U.Padding4(this.Length);
        }
        public string GetErrorMessage()
        {
            if (ErrorMessage == null)
            {
                return "";
            }
            return ErrorMessage;
        }

        public bool Parse(uint addr)
        {
            this.BaseAddr = addr;
            if (!U.isSafetyOffset(addr + 4 - 1))
            {//おかしなデータ
                this.ErrorMessage = R.Error("APのデータ({0})が壊れています。ヘッダがROM終端を超えています", U.To0xHexString(this.BaseAddr));
                return false;
            }

            uint frameDataOffset = Program.ROM.u16(addr);
            uint animeTableOffset = Program.ROM.u16(addr + 2);

            UpdateLength(addr + 4);

            //終端マークが存在しないので、読込むときには工夫が必要.
            uint minData = 0xFFFF;

            //フレームはアニメ開始地点まで
            uint end = this.BaseAddr + animeTableOffset;
            if (!U.isSafetyOffset(end - 1))
            {//おかしなデータ
                this.ErrorMessage = R.Error("APのデータ({0})が壊れています。フレーム終端がROM終端を超えました。({1})", U.To0xHexString(this.BaseAddr), U.To0xHexString(addr));
                return false;
            }

            for (addr = this.BaseAddr + frameDataOffset; addr < end; addr += 2)
            {
                uint f = Program.ROM.u16(addr);
                if (minData > f)
                {
                    minData = f;
                }
                FrameArr farr = ParseFrame(this.BaseAddr + frameDataOffset + f);
                if (farr == null)
                {
                    return false;
                }
                FrameArray.Add(farr);
            }

            if (minData >= 0x100)
            {//おかしなデータ
                this.ErrorMessage = R.Error("APのデータ({0})が壊れています。Animeデータの個数がありえない数です({1})。", U.To0xHexString(this.BaseAddr), minData);
                return false;
            }

            //アニメの終了地点は最初のフレームの開始位置まで
            end = this.BaseAddr + frameDataOffset + minData;
            if (!U.isSafetyOffset(end - 1))
            {//おかしなデータ
                this.ErrorMessage = R.Error("APのデータ({0})が壊れています。Anime終端がROM終端を超えました。({1})", U.To0xHexString(this.BaseAddr), U.To0xHexString(addr));
                return false;
            }

            for (addr = this.BaseAddr + animeTableOffset; addr < end; addr += 2)
            {
                uint a = Program.ROM.u16(addr);

                AnimeArr aarr = ParseAnime(this.BaseAddr + animeTableOffset + a);
                if (aarr == null)
                {
                    return false;
                }
                AnimeArray.Add(aarr);
            }

            UpdateLength(addr);
            return true;
        }
        void UpdateLength(uint addr)
        {
            uint a = addr - this.BaseAddr;
            if (a > this.Length)
            {//データの最大値を更新
                this.Length = a;
            }
        }

        public static void TEST_FE8J_APParseTest_WordmapBorder()
        {
            if (Program.ROM.RomInfo.VersionToFilename != "FE8J")
            {
                return;
            }

            uint ap = 0xA175C8;  //0xB2449C;
            ImageUtilAP p = new ImageUtilAP();
            p.Parse(ap);
        }
        
        AnimeArr ParseAnime(uint addr)
        {
            AnimeArr arr = new AnimeArr();
            arr.Animes = new List<Anime>();

            for (; ; addr += 4)
            {
                if (!U.isSafetyOffset(addr + 2 - 1))
                {//おかしなデータ
                    this.ErrorMessage = R.Error("APのデータ({0})が壊れています。Animeをスキャン中に、ROM終端を超えました。({1} @ {2})", U.To0xHexString(this.BaseAddr), U.To0xHexString(addr), arr.Animes.Count);
                    return null;
                }

                Anime a = new Anime();
                a.Wait = Program.ROM.u16(addr + 0);
                a.Frame = Program.ROM.u16(addr + 2);
                arr.Animes.Add(a);
                if (a.Wait == 0)
                {//アニメ終端
                    break;
                }
            }
            addr += 4;
            UpdateLength(addr);
            return arr;
        }
        FrameArr ParseFrame(uint addr)
        {
            FrameArr arr = new FrameArr();
            arr.Frames = new List<Frame>();

            uint count = Program.ROM.u16(addr);
            addr += 2;
            uint rotateCount = 0;
            if ((count & 0x8000) == 0x8000)
            {//回転などのデータがある場合
                rotateCount = count & 0x7FFF;
                if (rotateCount > 0x100)
                {//おかしなデータ
                    this.ErrorMessage = R.Error("APのデータ({0})が壊れています。OAM回転データの個数がありえない数です({1})。", U.To0xHexString(this.BaseAddr) , rotateCount );
                    return null;
                }
                count = rotateCount;

                arr.UseRorate = true;
                arr.CountRorate = rotateCount;
            }

            if (count > 0x100)
            {//おかしなデータ
                this.ErrorMessage = R.Error("APのデータ({0})が壊れています。データの個数がありえない数です({1})。", U.To0xHexString(this.BaseAddr), count);
                return null;
            }

            for (int i = 0; i < count; addr += 6, i++)
            {
                if (!U.isSafetyOffset(addr + 6 - 1))
                {//おかしなデータ
                    this.ErrorMessage = R.Error("APのデータ({0})が壊れています。APをスキャン中に、ROM終端を超えました。({1} @ {2})", U.To0xHexString(this.BaseAddr), U.To0xHexString(addr) , i);
                    return null;
                }

                Frame f = new Frame();
                f.OAM0 = Program.ROM.u16(addr + 0);
                f.OAM1 = Program.ROM.u16(addr + 2);
                f.OAM2 = Program.ROM.u16(addr + 4);
                arr.Frames.Add(f);
            }

            UpdateLength(addr);
            return arr;
        }

        public static void CheckAPErrors(uint apAddress, List<FELint.ErrorSt> errors, FELint.Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            ImageUtilAP ap = new ImageUtilAP();
            bool r = ap.Parse(apAddress);
            if (!r)
            {//エラー
                errors.Add(new FELint.ErrorSt(cond
                    , U.toOffset(addr)
                    , ap.GetErrorMessage()
                    , tag));
            }
        }


        //APのサイズを自動的に計算します.
        public static uint CalcAPLength(uint addr)
        {

            ImageUtilAP ap = new ImageUtilAP();
            bool r = ap.Parse(addr);
            if (!r)
            {//サイズ不明
                return 0;
            }
            uint newapLen = ap.GetLength();
            return newapLen;
        }

        public static uint CalcROMTCSLength(uint addr)
        {
            return CalcROMTCSLength(addr , Program.ROM);
        }

        //ROMTCSのサイズを自動的に計算します.
        //APと似ているが違う
        public static uint CalcROMTCSLength(uint addr,ROM rom)
        {
            byte[][] needArray = new byte[][]{
                 new byte[] { 0x00,0x00,0x00,0x00,0xFF,0xFF,0x04,0x00,0x01,0x00,0x00,0x00,0xFF,0xFF }
                ,new byte[] { 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 , 0xFF, 0xFF }
                ,new byte[] { 0x05, 0x00, 0x00, 0x00, 0xFF, 0xFF }
                ,new byte[] { 0x00, 0x00, 0xFF, 0xFF ,0x00, 0x00, 0x10, 0x00}
                ,new byte[] { 0x00, 0x00, 0xFF, 0xFF ,0x10, 0x00}
            };

            uint[] plusOffsetArray = new uint[] { 14, 8, 6, 6, 4 };

            uint limit = addr + 20000;
            if (limit > rom.Data.Length)
            {
                limit = (uint)rom.Data.Length;
            }

            uint endAddr = U.NOT_FOUND;
            uint plusOffset = 0;
            for (int i = 0; i < needArray.Length; i++)
            {
                uint a = U.Grep(rom.Data, needArray[i], addr, limit, 2);
                if (endAddr > a)
                {
                    endAddr = a;
                    plusOffset = plusOffsetArray[i];
                }
            }

            if (endAddr == U.NOT_FOUND)
            {
                return 0;
            }
            uint apLen = (endAddr + plusOffset) - addr;
            return apLen;
        }

        public static uint CalcPopupSimpleLength(uint addr)
        {
            return CalcPopupSimpleLength(addr , Program.ROM);
        }
        public static uint CalcPopupSimpleLength(uint addr, ROM rom)
        {
            byte[][] needArray = new byte[][]{
                 new byte[] { 0x00, 0x00,0x00, 0x00, 0x00, 0x00 , 0x00, 0x00 , 0x00, 0x00}
            };

            uint[] plusOffsetArray = new uint[] { 10 };

            uint limit = addr + 20000;
            if (limit > rom.Data.Length)
            {
                limit = (uint)rom.Data.Length;
            }

            uint endAddr = U.NOT_FOUND;
            uint plusOffset = 0;
            for (int i = 0; i < needArray.Length; i++)
            {
                uint a = U.Grep(rom.Data, needArray[i], addr, limit, 2);
                if (endAddr > a)
                {
                    endAddr = a;
                    plusOffset = plusOffsetArray[i];
                }
            }

            if (endAddr == U.NOT_FOUND)
            {
                return 0;
            }
            uint apLen = (endAddr + plusOffset) - addr;
            return apLen;
        }
    }
}
