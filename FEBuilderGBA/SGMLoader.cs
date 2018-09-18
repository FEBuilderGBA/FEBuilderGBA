using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    //VBA-M sgm save format
    public class SGMLoader
    {
        public byte[] IRAM0x03000000 { get; private set; }
        public byte[] WRAM0x02000000 { get; private set; }

        public SGMLoader(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            using (GZipStream gzip = new GZipStream(reader.BaseStream, CompressionMode.Decompress,false))
            {
                //ヘッダーとか、レジスタ情報とかをすっ飛ばす.

                //シークするとバグるんですが、 gzipさんはそれでいいんですか?
                //gzip.Seek(0x1DF, SeekOrigin.Begin);
                byte[] dummy = new byte[0x1DF];
                gzip.Read(dummy, 0, dummy.Length);

                //IRAM 0x03000000h - 03007FFFh
                this.IRAM0x03000000 = new byte[0x8000];
                gzip.Read(this.IRAM0x03000000, 0, this.IRAM0x03000000.Length);

                //skip palette
                //gzip.Seek(0x400, SeekOrigin.Current); //同様に、シークするとバグる.
                byte[] palette = new byte[0x400];
                gzip.Read(palette, 0, palette.Length);

                //WRAM 0x02000000h - 0303FFFFh
                this.WRAM0x02000000 = new byte[0x40000];
                gzip.Read(this.WRAM0x02000000, 0, this.WRAM0x02000000.Length);

                //以降  vram, 0x20000  oam, 0x400  ..etc... と、なるらしい.
                //src/gba/GBA.cpp を見るよろし.
            }
        }

        //マップIDの取得
        public uint getMapID()
        {
            return U.u8(this.WRAM0x02000000, Program.ROM.RomInfo.workmemory_mapid_address() - 0x02000000);
        }
        //最後に表示した文字列IDの取得
        public uint getLastStringID()
        {
            return U.u32(this.WRAM0x02000000, Program.ROM.RomInfo.workmemory_last_string_address() - 0x02000000);
        }

        public bool GrepEvent(uint eventAddress, out uint[] out_events)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return GrepEventFE6(eventAddress, out out_events);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                return GrepEventFE7(eventAddress, out out_events);
            }
            else
            {
                return GrepEventFE8(eventAddress, out out_events);
            }
        }


        //よくわからないのですが、
        //[00 00 00 00] [イベントポインタ][ポインタA][ポインタB] みたいに連続してメモリ上に存在していたら、たぶんそれが実行中のイベントですw
        //
        //たぶん、 
        //[イベントポインタ] は、イベント条件とかで指定するトップレベルのイベントです。
        //[ポインタA]は、現在実行中のイベントポインタです。
        //単純なイベントでは[イベントポインタ] ==  [ポインタA] ですが、
        //別イベントを call したりすると、変わってきます。
        //
        //ただ、[ポインタA]だけだと怖いので、 
        //[ポインタA]を優先するが、 [イベントポインタ]  も利用します。
        //そのため、out uint[] 型で、複数のイベントポインタを返すようにしています.
        public bool GrepEventFE8(uint eventAddress,out uint[] out_events)
        {
            uint eventPointer = U.toPointer(eventAddress);

            bool[] isSkip = new bool[4 * 4];
            isSkip[8] = true;
            isSkip[9] = true;
            isSkip[10] = true;
            isSkip[12] = true;
            isSkip[13] = true;
            isSkip[14] = true;

            byte[] m = new byte[4 * 4];
            U.write_u32(m, 0, 0);
            U.write_u32(m, 4, eventPointer);
            U.write_u32(m, 8, eventPointer);
            U.write_u32(m, 12, eventPointer);

            uint a = U.GrepPatternMatch(this.WRAM0x02000000, m, isSkip, 0, 0, 4);
            if (a == U.NOT_FOUND)
            {
                out_events = null;
                return false;
            }

            out_events = new uint[2];
            out_events[0] = U.p32(this.WRAM0x02000000, a + 4 + 4);
            out_events[1] = U.p32(this.WRAM0x02000000, a + 4);

            return true;
        }
        //よくわからないのですが、
        //[イベントポインタ][ポインタA][00 00 00 00][00 00 00 00] みたいに連続してメモリ上に存在していたら、たぶんそれが実行中のイベントですw
        //
        //たぶん、 
        //[イベントポインタ] は、イベント条件とかで指定するトップレベルのイベントです。
        //[ポインタA]は、現在実行中のイベントポインタです。
        //FE8と違い実行中には、[ポインタA]はどんどん進んでいきます
        //
        //ただ、[ポインタA]だけだと怖いので、 
        //[ポインタA]を優先するが、 [イベントポインタ]  も利用します。
        //そのため、out uint[] 型で、複数のイベントポインタを返すようにしています.
        public bool GrepEventFE7(uint eventAddress, out uint[] out_events)
        {
            uint eventPointer = U.toPointer(eventAddress);

            bool[] isSkip = new bool[4 * 4];
            isSkip[4] = true;
            isSkip[5] = true;
            isSkip[6] = true;

            byte[] m = new byte[4 * 4];
            U.write_u32(m, 0, eventPointer);
            U.write_u32(m, 4, eventPointer);
            U.write_u32(m, 8, 0);
            U.write_u32(m, 12, 0);

            uint a = U.GrepPatternMatch(this.WRAM0x02000000, m, isSkip, 0, 0, 4);
            if (a != U.NOT_FOUND)
            {
                out_events = new uint[2];
                out_events[0] = U.p32(this.WRAM0x02000000, a + 4);
                out_events[1] = U.p32(this.WRAM0x02000000, a + 0);
                return true;
            }

            out_events = null;
            return false;
        }


        //よくわからないのですが、
        //[イベントポインタ][ポインタA][00 00 00 00][ポインタ] みたいに連続してメモリ上に存在していたら、たぶんそれが実行中のイベントですw
        //
        //たぶん、 
        //[イベントポインタ] は、イベント条件とかで指定するトップレベルのイベントです。
        //[ポインタA]は、現在実行中のイベントポインタです。
        //FE8と違い実行中には、[ポインタA]はどんどん進んでいきます
        //
        //ただ、[ポインタA]だけだと怖いので、 
        //[ポインタA]を優先するが、 [イベントポインタ]  も利用します。
        //そのため、out uint[] 型で、複数のイベントポインタを返すようにしています.
        public bool GrepEventFE6(uint eventAddress, out uint[] out_events)
        {
            uint eventPointer = U.toPointer(eventAddress);

            bool[] isSkip = new bool[4 * 4];
            isSkip[4] = true;
            isSkip[5] = true;
            isSkip[6] = true;

            isSkip[12] = true;
            isSkip[13] = true;
            isSkip[14] = true;

            byte[] m = new byte[4 * 4];
            U.write_u32(m, 0, eventPointer);
            U.write_u32(m, 4, eventPointer);
            U.write_u32(m, 8, 0);
            U.write_u32(m, 12, eventPointer);

            uint a = U.GrepPatternMatch(this.WRAM0x02000000, m, isSkip, 0, 0, 4);
            if (a != U.NOT_FOUND)
            {
                out_events = new uint[2];
                out_events[0] = U.p32(this.WRAM0x02000000, a + 4);
                out_events[1] = U.p32(this.WRAM0x02000000, a + 0);
                return true;
            }

            out_events = null;
            return false;
        }
#if DEBUG
        public void DebugSave()
        {
            File.WriteAllBytes("__IRAM0x03000000.bin", this.IRAM0x03000000);
            File.WriteAllBytes("__WRAM0x02000000.bin", this.WRAM0x02000000);
        }
#endif
    }
}
