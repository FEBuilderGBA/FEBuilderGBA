using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

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

            public uint alpha;
            public uint xMag;
            public uint yMag;

            public uint TileGfx;
        }

        class AnimeArr
        {
            public List<Anime> Animes;
        }
        List<AnimeArr> AnimeArray = new List<AnimeArr>();

        class FrameArr
        {
            public bool UseRorate;
            public List<Frame> Frames;
        }
        List<FrameArr> FrameArray = new List<FrameArr>();

        uint BaseAddr;
        uint Length;

        public uint GetLength()
        {
            return U.Padding4(this.Length);
        }

        public void Parse(uint addr)
        {
            this.BaseAddr = addr;

            uint frameDataOffset = Program.ROM.u16(addr);
            uint animeTableOffset = Program.ROM.u16(addr + 2);

            UpdateLength(addr + 4);

            //終端マークが存在しないので、読込むときには工夫が必要.
            uint minData = 0xFFFF;

            //フレームはアニメ開始地点まで
            uint end = this.BaseAddr + animeTableOffset;
            for (addr = this.BaseAddr + frameDataOffset; addr < end; addr += 2)
            {
                uint f = Program.ROM.u16(addr);
                if (minData > f)
                {
                    minData = f;
                
                }
                FrameArray.Add(ParseFrame(this.BaseAddr + frameDataOffset + f));
            }

            //アニメの終了地点は最初のフレームの開始位置まで
            end = this.BaseAddr + frameDataOffset + minData;
            for (addr = this.BaseAddr + animeTableOffset; addr < end; addr += 2)
            {
                uint a = Program.ROM.u16(addr);
                AnimeArray.Add(ParseAnime(this.BaseAddr + animeTableOffset + a));
            }

            UpdateLength(addr);
        }
        void UpdateLength(uint addr)
        {
            uint a = addr - this.BaseAddr;
            if (a > this.Length)
            {//データの最大値を更新
                this.Length = a;
            }
        }

        
        AnimeArr ParseAnime(uint addr)
        {
            AnimeArr arr = new AnimeArr();
            arr.Animes = new List<Anime>();

            for (; ; addr += 4)
            {
                Anime a = new Anime();
                a.Wait = Program.ROM.u16(addr + 0);
                a.Frame = Program.ROM.u16(addr + 2);
                if (a.Wait == 0)
                {//アニメ終端
                    break;
                }
                arr.Animes.Add(a);
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

                arr.UseRorate = true;
                for (int i = 0; i < rotateCount; addr += 6, i++)
                {
                    Frame f = new Frame();
                    f.alpha = Program.ROM.u16(addr + 0);
                    f.xMag = Program.ROM.u16(addr + 2);
                    f.yMag = Program.ROM.u16(addr + 4);
                    arr.Frames.Add(f);
                }
                count = Program.ROM.u16(addr);
                addr += 2;
            }

            for (int i = 0; i < count; addr += 6, i++)
            {
                if (i < rotateCount)
                {//既に回転データを入れている場合
                    Frame f = arr.Frames[i];
                    f.OAM0 = Program.ROM.u16(addr + 0);
                    f.OAM1 = Program.ROM.u16(addr + 2);
                    f.OAM2 = Program.ROM.u16(addr + 4);
                }
                else
                {//回転データがない場合、通常のデータを追加.
                    Frame f = new Frame();
                    f.OAM0 = Program.ROM.u16(addr + 0);
                    f.OAM1 = Program.ROM.u16(addr + 2);
                    f.OAM2 = Program.ROM.u16(addr + 4);
                    arr.Frames.Add(f);
                }
            }

            //Tile
            for (int i = 0; i < count; addr += 2, i++)
            {
                Frame f = arr.Frames[i];
                f.TileGfx = Program.ROM.u16(addr + 0);
            }

            UpdateLength(addr);
            return arr;
        }

        //APのサイズを自動的に計算します.
        public static uint CalcAPLength(uint addr)
        {
            ImageUtilAP ap = new ImageUtilAP();
            ap.Parse(addr);
            uint newapLen = ap.GetLength();
/*
            //やはりこちらの方がいいのでは・・・?
            byte[] need1 = new byte[] { 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00 };
            uint endAddr1 = U.Grep(Program.ROM.Data, need1, addr, 0, 2);

            byte[] need2 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF };
            uint endAddr2 = U.Grep(Program.ROM.Data, need2, addr, 0, 2);

            uint endAddr = Math.Min(endAddr1, endAddr2);
            if (endAddr == U.NOT_FOUND)
            {
                return 0;
            }
            uint aplen = (endAddr + 6) - addr;
            if (aplen >= 16384)
            {
                return 0;
            }
//            Debug.Assert(aplen == newapLen);
            return aplen;
*/
            return newapLen;
        }
    }
}
