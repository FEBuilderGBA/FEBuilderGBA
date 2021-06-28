using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public class SongUtil
    {
        static int[] WaitCode = new int[]{
            0, //0,
            1, //1,
            2, //2,
            3, //3,
            4, //4,
            5, //5,
            6, //6,
            7, //7,
            8, //8,
            9, //9,
            10, //10,
            11, //11,
            12, //12,
            13, //13,
            14, //14,
            15, //15,
            16, //16,
            17, //17,
            18, //18,
            19, //19,
            20, //20,
            21, //21,
            22, //22,
            23, //23,
            24, //24,
            28, //25,
            30, //26,
            32, //27,
            36, //28,
            40, //29,
            42, //30,
            44, //31,
            48, //32,
            52, //33,
            54, //34,
            56, //35,
            60, //36,
            64, //37,
            66, //38,
            68, //39,
            72, //40,
            76, //41,
            78, //42,
            80, //43,
            84, //44,
            88, //45,
            90, //46,
            92, //47,
            96, //48,
        };

        const uint WAIT_START = 0x80; //W00から
        const uint WAIT_END = 0x80 + 48;
        const uint EOT = 0xCE;
        const uint TIE = 0xCF;
        const uint NOTE_START = 0xD0; //N01から
        const uint NOTE_END = 0xFF;

        //ループのとび先も記録したいので適当なコードをねつ造する.
        const uint LOOP_LABEL_CODE = 0xFEFEFEFE;
        //番兵等の便宜上入れるが、意味をなさないイベント
        const uint DUMMY_CODE = 0xFFEEFFEE;
        //番兵等の便宜上入れるが、意味をなさないイベント
        const uint REVERB_CODE = 0xDDDDDDDD;

        static string[] KEYCODE = new string[]{
            "Cn",//0  CnM2 - Cn8
            "Cs",//1
            "Dn",//2
            "Ds",//3
            "En",//4
            "Fn",//5
            "Fs",//6
            "Gn",//7
            "Gs",//8
            "An",//9
            "As",//10
            "Bn",//11
        };

        static string[] MEMACC = new string[]{
            "mem_set", //0x0;
            "mem_add", //0x1;
            "mem_sub", //0x2;
            "mem_mem_set", //0x3;
            "mem_mem_add", //0x4;
            "mem_mem_sub", //0x5;
            "mem_beq", //0x6;
            "mem_bne", //0x7;
            "mem_bhi", //0x8;
            "mem_bhs", //0x9;
            "mem_bls", //0xA;
            "mem_blo", //0xB;
            "mem_mem_beq", //0xC;
            "mem_mem_bne", //0xD;
            "mem_mem_bhi", //0xE;
            "mem_mem_bhs", //0xF;
            "mem_mem_bls", //0x10;
            "mem_mem_blo", //0x11;
        };

        static uint byteToWait(uint b)
        {
            if (b < WAIT_START)
            {
                return 0;
            }
            if (b - WAIT_START >= WaitCode.Length)
            {
                return 0;
            }
            return (uint)WaitCode[(int)b - WAIT_START];
        }
        static uint byteToNote(uint b)
        {
            if (b + 1 < NOTE_START)
            {
                return 0;
            }
            if (b + 1 - NOTE_START >= WaitCode.Length)
            {
                return 0;
            }
            return (uint)WaitCode[(int)b + 1 - NOTE_START];
        }

        public class Code
        {
            public uint addr;
            public uint waitCount;
            public uint type;
            public uint value;
            public uint value2;
            public uint value3;

            public Code(uint addr, uint waitCount, byte type, uint value = 0, uint value2 = 0, uint value3 = 0)
            {
                this.addr = addr;
                this.waitCount = waitCount;
                this.type = type;
                this.value = value;
                this.value2 = value2;
                this.value3 = value3;
            }
            public Code(uint addr, uint waitCount, uint type, uint value = 0, uint value2 = 0, uint value3 = 0)
            {
                this.addr = addr;
                this.waitCount = waitCount;
                this.type = type;
                this.value = value;
                this.value2 = value2;
                this.value3 = value3;
            }

        };

        public class Track //1トラック
        {
            //実データ
            public List<Code> codes = new List<Code>();
            public uint basepointer;
        }
        static bool CheckGTPRange(uint gtp)
        {
            return gtp >= 1 && gtp <= 3;
        }

        public static List<Track> ParseTrack(uint song_addr, uint trackcount)
        {
            List<Track> tracks = new List<Track>();
            if (!U.isSafetyOffset(song_addr))
            {
                return tracks;
            }

            //終端
            uint limitter = (uint)Program.ROM.Data.Length;

            for (int trackindex = 0; trackindex < trackcount; trackindex++)
            {
                //header
                //[trackcount] ?? ?? ?? [P:voca] [P:track1] [P:track2] [P:track3]  ... [P:trackN]
                uint trackpointer = song_addr + 8 + (uint)(trackindex * 4);
                Track track = ParseTrackOne(trackpointer);
                tracks.Add(track);
            }
            return tracks;
        }
        public static Track ParseTrackOne(uint trackpointer)
        {
            //終端
            uint limitter = (uint)Program.ROM.Data.Length;

            Track track = new Track();
            if (trackpointer >= limitter)
            {
                return track;
            }

            uint trackaddr = Program.ROM.u32(trackpointer);
            if (!U.isSafetyPointer(trackaddr))
            {
                return track;
            }
            trackaddr = U.toOffset(trackaddr);
            track.basepointer = trackpointer;

            uint waitCount = 0;
            for (uint addr = trackaddr; true; )
            {
                if (addr >= limitter)
                {//終端を超えました.
                    break;
                }

                int index = (int)(addr - trackaddr);

                uint b = Program.ROM.u8(addr);

                if (b == 0xB1)
                {
                    track.codes.Add(new Code(addr, waitCount, b));
                    addr++;
                    break;
                }
                else if (b == 0xB2 || b == 0xB3)
                {//ループ　ポインタ
                    uint loopaddr = Program.ROM.p32(addr + 1);
                    track.codes.Add(new Code(addr, waitCount, b, loopaddr));
                    addr += 5;
                    waitCount += 96; //これだけで1小節分
                }
                else if (b == EOT)
                {//TIE - EOTまで音をならしっばにする.
                    track.codes.Add(new Code(addr, waitCount, b));
                    addr++;
                }
                else if (b == 0xBD || b == 0xBB || b == 0xBC || b == 0xBE || b == 0xBF || b == 0xC0 || b == 0xC1 || b == 0xC2 || b == 0xC3 || b == 0xC4 || b == 0xC5 || b == 0xC8)
                {
                    if (addr + 1 >= limitter)
                    {//終端を超えました.
                        break;
                    }
                    uint next_byte = Program.ROM.u8(addr + 1);
                    track.codes.Add(new Code(addr, waitCount, b, next_byte));
                    addr += 2;
                }
                else if (b == 0xb9)
                {//MEMACC 4バイト命令
                    if (addr + 3 >= limitter)
                    {//終端を超えました.
                        break;
                    }
                    uint b1 = Program.ROM.u8(addr + 1);
                    uint b2 = Program.ROM.u8(addr + 2);
                    uint b3 = Program.ROM.u8(addr + 3);
                    track.codes.Add(new Code(addr, waitCount, b, b1, b2, b3));
                    addr += 4;
                }
                else if (b >= WAIT_START && b <= (uint)WAIT_END)
                {
                    track.codes.Add(new Code(addr, waitCount, b));
                    waitCount += byteToWait(b);
                    addr++;
                }
                else if (b >= TIE && b <= (uint)NOTE_END)
                {
                    if (addr + 1 >= limitter)
                    {//終端を超えました.
                        break;
                    }

                    uint key = Program.ROM.u8(addr + 1);
                    if (key <= 127)
                    {
                        if (addr + 2 >= limitter)
                        {//終端を超えました.
                            break;
                        }

                        uint velocity = Program.ROM.u8(addr + 2);
                        if (velocity <= 127)
                        {//N96 Gn7 v10 みたいな3バイト命令
                            uint gtp = Program.ROM.u8(addr + 3);
                            if (CheckGTPRange(gtp))
                            {//N96 Gn7 v10 gtp1 みたいな4バイト命令
                                track.codes.Add(new Code(addr, waitCount, b, key, velocity, gtp));
                                addr += 4;
                            }
                            else
                            {//N96 Gn7 v10 みたいな3バイト命令
                                track.codes.Add(new Code(addr, waitCount, b, key, velocity));
                                addr += 3;
                            }
                        }
                        else
                        {//velocity >=128なのでこれは違う命令
                            //N96 Gn7 みたいな2バイト命令
                            track.codes.Add(new Code(addr, waitCount, b, key, U.NOT_FOUND));
                            addr += 2;
                        }
                    }
                    else
                    {//key >= 128なので別の命令
                        //N96 みたいな1バイト命令
                        track.codes.Add(new Code(addr, waitCount, b, U.NOT_FOUND, U.NOT_FOUND));
                        addr += 1;
                    }
                }
                else if (b <= 127)
                {
                    if (addr + 1 >= limitter)
                    {//終端を超えました.
                        break;
                    }

                    uint key = b;
                    uint velocity = Program.ROM.u8(addr + 1);
                    if (velocity <= 127)
                    {
                        track.codes.Add(new Code(addr, waitCount, key, velocity));
                        addr += 2;
                    }
                    else
                    {//velocity >=128なのでこれは違う命令
                        track.codes.Add(new Code(addr, waitCount, key, U.NOT_FOUND));
                        addr += 1;
                    }
                }
                else
                {
                    track.codes.Add(new Code(addr, waitCount, b));
                    addr++;
                }
            }
            insertLoopLabel(track);
            return track;
        }
        static void insertLoopLabel(Track track)
        {
            List<uint> insertLabel = new List<uint>();

            for (int i = 0; i < track.codes.Count; i++)
            {
                if (track.codes[i].type != 0xB2 && track.codes[i].type != 0xB3)
                {
                    continue;
                }

                if (insertLabel.IndexOf(track.codes[i].value) >= 0)
                {
                    //すでにラベルを追加済み
                    continue;
                }

                insertLabel.Add(track.codes[i].value);
            }

            for (int n = 0; n < insertLabel.Count; n++)
            {
                uint findaddr = insertLabel[n];
                for (int i = 0; i < track.codes.Count; i++)
                {
                    if (track.codes[i].addr == findaddr)
                    {
                        track.codes.Insert(i
                            , new Code(
                                track.codes[i].addr
                                , track.codes[i].waitCount
                                , LOOP_LABEL_CODE
                                ));
                        break;
                    }
                }
            }
        }

        public static void TrackToListBox(ListBox listbox, Track track)
        {
            listbox.BeginUpdate();
            listbox.Items.Clear();

            uint tune = 0;
            uint lastTuneWait = 0;
            const uint tempo = 96;

            if (track.codes.Count <= 0)
            {
                listbox.Items.Add(string.Format("{0}@",0));
            }
            else
            {
                listbox.Items.Add(string.Format("{0}@{1}", tune, U.ToHexString(track.codes[0].addr)));
            }

            for (int i = 0; i < track.codes.Count; i++)
            {
                Code code = track.codes[i];
                if (code.waitCount >= lastTuneWait + tempo)
                {
                    if (code.type == 0xb4 || code.type == 0xb1 || code.type == 0xb3)
                    {//PEND FINE は、例外として、最後のトラックに記載する.
                    }
                    else
                    {//次のトラックへ
                        lastTuneWait = code.waitCount;
                        tune++;
                        listbox.Items.Add(string.Format("{0}@{1}", tune, U.ToHexString(code.addr + 1)));
                    }
                }
                if (isDummyCode(code.type))
                {
                    continue;
                }
                string name = SongUtil.trackToString(code);
                listbox.Items.Add(name);

                if (code.type == 0xb4 || code.type == 0xb1 || code.type == 0xb3)
                {
                    lastTuneWait = code.waitCount;
                    tune++;
                    listbox.Items.Add(string.Format("{0}@{1}", tune, U.ToHexString(code.addr + 1)));
                }
            }
            listbox.EndUpdate();
        }

        static bool isDummyCode(uint type)
        {
            return type == DUMMY_CODE || type == REVERB_CODE;
        }

        public static string getKeyCode(uint code)
        {
            int key = (int)code % 12;
            int keyN = (int)code / 12;
            if (keyN == 0)
            {
                return KEYCODE[key] + "M2";
            }
            else if (keyN == 1)
            {
                return KEYCODE[key] + "M1";
            }
            else if (keyN >= 2)
            {
                return KEYCODE[key] + (keyN - 2);
            }
            return "";
        }


        public static string trackToString(Code code)
        {
            if (code.type >= WAIT_START && code.type <= (uint)WAIT_END)
            {//W94
                return "W" + byteToWait(code.type).ToString("00");
            }
            if (code.type >= TIE && code.type <= (uint)NOTE_END)
            {// N24, Dn3 ,v100
                // N24, Dn3
                string name = "";
                if (code.type == TIE)
                {
                    name += "TIE";
                }
                else
                {
                    name += "N" + byteToNote(code.type).ToString("00");
                }
                if (code.value != U.NOT_FOUND)
                {
                    name += " ,";
                    name += getKeyCode(code.value);

                    if (code.value2 != U.NOT_FOUND)
                    {
                        name += " ," + "v" + code.value2.ToString("000");
                        if (code.value3 != U.NOT_FOUND && CheckGTPRange(code.value3))
                        {
                            name += " ," + "gtp" + code.value3.ToString();
                        }
                    }
                }
                return name;
            }
            if (code.type <= 127)
            {// Dn3 ,v100
                // Dn3

                string name = "";
                name += getKeyCode(code.type);

                if (code.value != U.NOT_FOUND)
                {
                    name += " ," + "v" + code.value.ToString("000");
                }
                return name;
            }

            switch (code.type)
            {
                case 0xCE:
                    return "EOT";
                case 0xb1:
                    return "FINE";
                case 0xb2:
                    return "GOTO, " + "Label_" + U.ToHexString(code.value);
                case 0xb3:
                    return "PATT, " + "Label_" + U.ToHexString(code.value);
                case 0xb4:
                    return "PEND ";
                case 0xb9:
                    return "MEMACC, " + U.at(MEMACC, code.value, U.To0xHexString(code.value)) +", "+ U.To0xHexString(code.value2) +", "+ code.value3;
                case 0xba:
                    return "PRIO, " + (code.value);
                case 0xbb:
                    return "TEMPO, " + (code.value);
                case 0xbc:
                    return "KEYSH, " + U.ToPlus((int)((sbyte)code.value));
                case 0xbd:
                    return "VOICE, " + (code.value);
                case 0xbe:
                    return "VOL, " + (code.value);
                case 0xbf:
                    return "PAN," + "c_v" + U.ToPlus(((int)code.value) - 0x40);
                case 0xc0:
                    return "BEND," + "c_v" + U.ToPlus(((int)code.value) - 0x40);
                case 0xc1:
                    return "BENDR, " + (code.value);
                case 0xc2:
                    return "LFOS " + (code.value);
                case 0xc3:
                    return "LFODL " + (code.value);
                case 0xc4:
                    return "MOD " + (code.value);
                case 0xc5:
                    return "MODT " + (code.value);
                case 0xc8:
                    return "TUNE " + (code.value);
                case LOOP_LABEL_CODE:
                    return "Label_" + (code.addr) + ":";
                case DUMMY_CODE:
                    Debug.Assert(false);
                    return "DUMMY_CODE";
                case REVERB_CODE:
                    Debug.Assert(false);
                    return "REVERB_CODE";
                default:
                    return U.ToHexString(code.type);
            }
        }



        public static void ExportSFile(string filename, string songname, List<Track> tracks, int numBlks, int priority, int reverb, uint instrument_addr)
        {
            int c_v = 0x40;

            using (StreamWriter w = new StreamWriter(filename))
            {
                w.WriteLine("	.include \"MPlayDef.s\"");
                w.WriteLine("");
                w.WriteLine("	.equ	" + songname + "_grp, voicegroup000");
                w.WriteLine("	.equ	" + songname + "_pri, " + priority);
                w.WriteLine("	.equ	" + songname + "_rev, " + reverb);
                w.WriteLine("	.equ	" + songname + "_mvl, 127");
                w.WriteLine("	.equ	" + songname + "_key, 0");
                w.WriteLine("	.equ	" + songname + "_tbs, 1");
                w.WriteLine("	.equ	" + songname + "_exg, 0");
                w.WriteLine("	.equ	" + songname + "_cmp, 1");
                w.WriteLine("");
                w.WriteLine("	.section .rodata");
                w.WriteLine("	.global	" + songname);
                w.WriteLine("	.align	2");
                w.WriteLine("");

                for (int t = 0; t < tracks.Count; t++)
                {
                    w.WriteLine("");
                    w.WriteLine("@**************** Track " + (t + 1) + " (Midi-Chn." + t + ") ****************@");
                    w.WriteLine("");
                    w.WriteLine(songname + "_" + (t + 1).ToString("000") + ":");

                    uint tune = 0;
                    uint lastTuneWait = 0;
                    const uint tempo = 96;
                    w.WriteLine("@ " + tune.ToString("000") + "   ----------------------------------------");
                    for (int i = 0; i < tracks[t].codes.Count; i++)
                    {
                        Code code = tracks[t].codes[i];
                        if (code.waitCount >= lastTuneWait + tempo)
                        {
                            if (code.type == 0xb4 || code.type == 0xb1 || code.type == 0xb3)
                            {//PEND FINE PATT は、例外として、最後のトラックに記載する.
                            }
                            else
                            {//次のトラックへ
                                lastTuneWait = code.waitCount;
                                tune++;
                                w.WriteLine("@ " + tune.ToString("000") + "   ----------------------------------------");
                            }
                        }
                        if (code.type == 0xb3)
                        {//PATTなので強制的に小節をわける
                            lastTuneWait = 0;
                            tune++;
                            w.WriteLine("@ " + tune.ToString("000") + "   ----------------------------------------");
                        }

                        if (code.type == LOOP_LABEL_CODE)
                        {//LOOP_LABEL_CODE
                            w.WriteLine("Label_" + t + "_" + U.ToHexString(code.addr) + ":");
                        }
                        else if (code.type == 0xB2)
                        {//GOTO
                            w.WriteLine(" .byte   GOTO");
                            w.WriteLine("  .word " + "Label_" + t + "_" + U.ToHexString(code.value));
                        }
                        else if (code.type == 0xB3)
                        {//PATT
                            w.WriteLine(" .byte   PATT");
                            w.WriteLine("  .word " + "Label_" + t + "_" + U.ToHexString(code.value));
                        }
                        else if (code.type == 0xBA)
                        {//PRIO
                            //nop
                        }
                        else if (code.type == 0xBB)
                        {//TEMPO
                            w.WriteLine(" .byte   TEMPO , " + (code.value * 2) + "*" + songname + "_tbs/2");
                        }
                        else if (code.type == 0xBC)
                        {//KEYSH
                            w.WriteLine(" .byte   KEYSH , " + songname + "_key" + U.ToPlus((int)((sbyte)code.value)));
                        }
                        else if (code.type == 0xBD)
                        {//VOICE
                            w.WriteLine(" .byte   VOICE , " + (code.value));
                        }
                        else if (code.type == 0xBE)
                        {//VOL
                            w.WriteLine(" .byte   VOL , " + (code.value) + "*" + songname + "_mvl/mxv");
                        }
                        else if (code.type == 0xBF)
                        {//PAN
                            w.WriteLine(" .byte   PAN , " + "c_v" + U.ToPlus(((int)code.value) - c_v));
                        }
                        else if (code.type == 0xC0)
                        {//BEND
                            w.WriteLine(" .byte   BEND , " + "c_v" + U.ToPlus(((int)code.value) - c_v));
                        }
                        else if (isDummyCode(code.type))
                        {//無視
                        }
                        else
                        {
                            string l = trackToString(code);
                            w.WriteLine(" .byte   " + l);
                        }
                    }
                }
                w.WriteLine("");
                w.WriteLine("@******************************************************@");
                w.WriteLine("	.align	2");
                w.WriteLine("");
                w.WriteLine(songname + ":");
                w.WriteLine("	.byte	" + tracks.Count + "	@ NumTrks");
                w.WriteLine("	.byte	0	@ NumBlks");
                w.WriteLine("	.byte	" + songname + "_pri	@ Priority");
                w.WriteLine("	.byte	" + songname + "_rev	@ Reverb.");
                w.WriteLine("    ");
                w.WriteLine("	.word	" + songname + "_grp");
                w.WriteLine("    ");

                for (int t = 0; t < tracks.Count; t++)
                {
                    w.WriteLine("	.word	" + songname + "_" + (t + 1).ToString("000"));
                }
                w.WriteLine("");
                w.WriteLine("	.end");
            }
        }


        class PlayKeys
        {
            public byte key;
            public uint stopTime;
            public PlayKeys(byte key, uint stopTime)
            {
                this.key = key;
                this.stopTime = stopTime;
            }
        };
        //Nxx以外の特殊停止
        const uint WAIT_TIE = U.NOT_FOUND;
        const uint WAIT_LOOPEND = U.NOT_FOUND - 1;

        public static void ExportMidiFile(string filename, string songname, List<Track> tracks, int numBlks, int priority, int reverb, uint instrument_addr)
        {
            List<byte> midi = new List<byte>();
            midi.Add((byte)'M');
            midi.Add((byte)'T');
            midi.Add((byte)'h');
            midi.Add((byte)'d');
            U.append_big32(midi, 6); //block size 6
            U.append_big16(midi, 1); //format 1
            U.append_big16(midi, 1 + (uint)tracks.Count); //track数
            U.append_big16(midi, 24); //四分音符の分解能

            //指揮トラック
            {
                int t = 0; //最初のトラックのイベントを見て、指揮情報を入れ込む.
                List<byte> data = new List<byte>();

                //ループを探索.
                uint loopStartWaitCount = U.NOT_FOUND;
                uint loopEndWaitCount = U.NOT_FOUND;
                for (int i = 0; i < tracks[t].codes.Count; i++)
                {
                    Code code = tracks[t].codes[i];
                    if (code.type == 0xB2)
                    {//ループ
                        int index = findLabel(code.value, tracks[t].codes);
                        if (index < tracks[t].codes.Count)
                        {
                            loopStartWaitCount = tracks[t].codes[index].waitCount;
                            loopEndWaitCount = code.waitCount;
                        }
                        break;
                    }
                }

                uint totalDeltaTime = 0;
                for (int i = 0; i < tracks[t].codes.Count; i++)
                {
                    Code code = tracks[t].codes[i];

                    if (code.type >= WAIT_START && code.type <= (uint)WAIT_END)
                    {//W94
                        uint wait = byteToWait(code.type);
                        totalDeltaTime += wait;
                        continue;
                    }
                    if (code.type == 0xBB)
                    {//TEMPO
                        uint tempo = code.value * 2;
                        tempo = (uint)(60000000.0 / tempo);

                        U.append_vlength_code(data, (int)totalDeltaTime);
                        data.Add(0xFF);
                        data.Add(0x51);
                        data.Add(0x03);
                        U.append_big24(data, tempo);

                        totalDeltaTime = 0;
                        continue;
                    }
                    if (code.type == LOOP_LABEL_CODE)
                    {//ラベル.
                        if (code.waitCount == loopStartWaitCount)
                        {
                            string loopstart = "loopStart";

                            U.append_vlength_code(data, (int)totalDeltaTime);
                            data.Add(0xFF); //マーカー
                            data.Add(0x6);
                            data.Add((byte)loopstart.Length); //文字列長さ
                            U.append_range(data, loopstart);
                        }
                        totalDeltaTime = 0;
                        continue;
                    }
                    if (code.type == 0xB2)
                    {//ループ
                        if (code.waitCount == loopEndWaitCount)
                        {
                            string loopstart = "loopEnd";

                            U.append_vlength_code(data, (int)totalDeltaTime);
                            data.Add(0xFF); //マーカー
                            data.Add(0x6);
                            data.Add((byte)loopstart.Length); //文字列長さ
                            U.append_range(data, loopstart);
                        }
                        totalDeltaTime = 0;
                        continue;
                    }
                }

                //トラック終端
                data.Add(0x00);
                data.Add(0xFF);
                data.Add(0x2F);
                data.Add(0x00);

                //midiトラックの書き込み
                //データサイズを描かないといけない。
                midi.Add((byte)'M');
                midi.Add((byte)'T');
                midi.Add((byte)'r');
                midi.Add((byte)'k');
                U.append_big32(midi, (uint)data.Count);//データサイズ
                midi.AddRange(data);
            }

            for (int t = 0; t < tracks.Count; t++)
            {
                List<byte> data = new List<byte>();

                //PATTループを展開しないといけない.
                List<int> callstack = new List<int>();

                byte last_keyshift = 0;

                byte last_key = 0;
                byte last_velocity = 0;
                List<PlayKeys> playKeys = new List<PlayKeys>(); //和音の可能性がある

                //リバーブ処理
                data.Add(0x0);  //即実行
                data.Add(0xB0); //コントロールコード
                data.Add(0x5B);  //ビブラート(リバーブ)
                data.Add((byte)(((uint)reverb) & 0x7f)); //値

                for (int i = 0; i < tracks[t].codes.Count; i++)
                {
                    Code code = tracks[t].codes[i];

                    if (code.type >= WAIT_START && code.type <= (uint)WAIT_END)
                    {//W94
                        uint wait = byteToWait(code.type);

                        //ウェイトが分解されるか確認
                        uint currentWait = code.waitCount;
                        uint endWait = code.waitCount + wait;
                        while (currentWait < endWait)
                        {
                            uint minWait = searchMinimamWait(
                                  currentWait
                                , wait
                                , playKeys);
                            currentWait += minWait;
                            wait -= minWait;

                            data.Add((byte)minWait); //待機

                            //過ぎてしまったイベントキーの終了
                            bool isFirstWait = true;
                            for (int n = playKeys.Count - 1; n >= 0; )
                            {
                                if (playKeys[n].stopTime == WAIT_LOOPEND)
                                {//このループで止める.
                                }
                                else if (playKeys[n].stopTime > currentWait)
                                {//まだ停止させる時間に行っていない.
                                    n--;
                                    continue;
                                }
                                if (isFirstWait == false)
                                {
                                    data.Add(0); //今すぐに停止
                                }
                                data.Add(0x80);  //ランニングステータスしない 省略しない
                                data.Add(playKeys[n].key); //指定した音を消す.
                                data.Add(0);

                                isFirstWait = false;

                                //停止したのでリストから削除.
                                playKeys.RemoveAt(n);
                                n = playKeys.Count - 1;
                            }

                            if (isFirstWait)
                            {//停止させるものがない。単純な全休符?
                                data.Add(0x80);  //ランニングステータスしない 省略しない
                                data.Add(0); //指定した音を消す.
                                data.Add(0);
                            }

                        }

                        continue;
                    }
                    if (code.type >= TIE && code.type <= (uint)NOTE_END)
                    {// N24, Dn3 ,v100
                        // N24, Dn3
                        uint stopTime;
                        if (code.type == TIE)
                        {
                            stopTime = WAIT_TIE;
                        }
                        else
                        {
                            stopTime = byteToNote(code.type) + code.waitCount;
                        }

                        if (code.value != U.NOT_FOUND)
                        {
                            last_key = (byte)(code.value);
                            if (code.value2 != U.NOT_FOUND)
                            {
                                last_velocity = (byte)(code.value2); //ベロシティ(音の強さ)
                            }
                        }
                        else
                        {
                            //指定がない場合、最後に鳴らした音をもう一回鳴らす.
                        }
                        last_key += last_keyshift;

                        data.Add(0); //即音を鳴らす.
                        data.Add(0x90);  //ランニングステータスしない省略しない
                        data.Add(last_key); //key(note)
                        data.Add((byte)last_velocity); //velocity

                        playKeys.Add(new PlayKeys(last_key, stopTime));
                        continue;
                    }
                    if (code.type <= 127)
                    {// Dn3 ,v100
                        // Dn3
                        last_key = (byte)(code.type);
                        if (code.value != U.NOT_FOUND)
                        {
                            last_velocity = (byte)(code.value); //ベロシティ(音の強さ)
                        }
                        last_key += last_keyshift;

                        data.Add(0); //即音を鳴らす.
                        data.Add(0x90);  //ランニングステータスしない省略しない
                        data.Add(last_key); //key(note)
                        data.Add(last_velocity); //velocity

                        //次の待機終了まで鳴らす
                        playKeys.Add(new PlayKeys(last_key, WAIT_LOOPEND));
                        continue;
                    }

                    if (code.type == 0xB3)
                    {//PATT ループ呼び出しCALL命令
                        callstack.Add(i);
                        i = findLabel(code.value, tracks[t].codes);
                        continue;
                    }
                    if (code.type == 0xB4)
                    {//PEND ループ命令の RET 命令.
                        if (callstack.Count > 0)
                        {
                            int lastindex = callstack.Count - 1;
                            i = callstack[lastindex];
                            callstack.RemoveAt(lastindex);
                        }
                        continue;
                    }
                    if (code.type == 0xBC)
                    {//KEYSH
                        last_keyshift = (byte)code.value;
                        continue;
                    }
                    if (code.type == 0xBD)
                    {//VOICE
                        // ただしいmidi楽器にすると、音が非常に小さくなるものがある.
                        //uint voice = SongInstrumentForm.ToMidiInstrument(instrument_addr, code.value, code.value);

                        uint voice = code.value;
                        data.Add(0x0);  //即実行
                        data.Add(0xC0); //楽器変更
                        data.Add((byte)voice); //変更する楽器
                        continue;
                    }
                    if (code.type == 0xBE)
                    {//VOL
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x7);  //音量
                        data.Add((byte)expVolRev(code.value)); //音量の値
                        continue;
                    }
                    if (code.type == 0xBF)
                    {//PAN
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x0A);  //PAN
                        data.Add((byte)code.value); //PAN値
                        continue;
                    }
                    if (code.type == 0xC0)
                    {//BEND
                        uint arg1, arg2;
                        CalcPitchBendGBAToMidi(code.value, out arg1, out arg2);

                        data.Add(0x0);
                        data.Add(0xE0); //Pich bend
                        data.Add((byte)(arg1)); //上位（LSB）データ（7ビット）
                        data.Add((byte)(arg2));//下位（MSB）データ（7ビット） 
                        continue;
                    }
                    if (code.type == 0xC1)
                    {//BENDR
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x14); //Pitch bend range
                        data.Add((byte)code.value); //値
                        continue;
                    }
                    if (code.type == 0xC2)
                    {//LFOS
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x15); //LFO Speed
                        data.Add((byte)code.value); //値
                        continue;
                    }
                    if (code.type == 0xC3)
                    {//LFODL
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x1A); // LFO delay
                        data.Add((byte)code.value); //値
                        continue;
                    }
                    if (code.type == 0xC4)
                    {//MOD
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x01); // LFO depth
                        data.Add((byte)code.value); //値
                        continue;
                    }
                    if (code.type == 0xC5)
                    {//MODT
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x16); //LFO type
                        data.Add((byte)code.value); //値
                        continue;
                    }
                    if (code.type == 0xC8)
                    {//TUNE
                        data.Add(0x0);  //即実行
                        data.Add(0xB0); //コントロールコード
                        data.Add(0x18); //Detune
                        data.Add((byte)code.value); //値
                        continue;
                    }
                    if (code.type == EOT)
                    {
                        for (int n = 0; n < playKeys.Count; )
                        {
                            if (playKeys[n].stopTime == WAIT_TIE)
                            {//停止させる.
                                data.Add(0); //即音を鳴らす.
                                data.Add(0x80);  //ランニングステータスしない省略しない
                                data.Add(playKeys[n].key); //key(note)
                                data.Add(0); //velocity
                                //停止したのでリストから削除.
                                playKeys.RemoveAt(n);
                                continue;
                            }
                            else
                            {
                                n++;
                            }
                        }
                        continue;
                    }
                }

                //トラック終端
                data.Add(0x00);
                data.Add(0xFF);
                data.Add(0x2F);
                data.Add(0x00);

                //midiトラックの書き込み
                //データサイズを描かないといけない。
                midi.Add((byte)'M');
                midi.Add((byte)'T');
                midi.Add((byte)'r');
                midi.Add((byte)'k');
                U.append_big32(midi, (uint)data.Count);//データサイズ
                midi.AddRange(data);
            }

            U.WriteAllBytes(filename, midi.ToArray());
        }

        class MidiKey
        {
            public uint time;
            public uint key;
            public uint velocity;
        };

        static uint ParseMidi(byte[] midi, uint pos
            , Track[] tracks, Track condTrack
            , uint t, uint onpu4
            )
        {
            byte[] miniheader = { (byte)'M', (byte)'T', (byte)'r', (byte)'k' };
            pos = U.Grep(midi, miniheader, pos);
            if (pos == U.NOT_FOUND)
            {
                return pos;
            }
            uint headersize = U.big32(midi, pos + 4);
            MidiKey[] playKey = new MidiKey[128 * 0xf];

            uint total_time = 0;
            uint i = pos + 4 + 4;
            uint end = i + headersize;
            uint lastStatusByte = 0; //ランニングステータスに対応するため最後に送信した命令の記録.
            uint loopstart = U.NOT_FOUND;
            //see https://sites.google.com/site/yyagisite/material/smfspec#RunningStatus
            while (i < end)
            {
                uint time = U.read_vlength_code(midi, i, out i);
                total_time += time;

                uint statusbyte = midi[i];
                if ((midi[i] & 0x80) == 0x80)
                {//ランニングステータスしていない.
                    //最後に送信したステータスバイトとして記録.
                    lastStatusByte = statusbyte;
                    i++;
                }
                else
                {//ランニングステータスしている.
                    //よって、最後に送信したステータスバイト命令を再送.
                    statusbyte = lastStatusByte;
                }

                uint tracknumber = (uint)(statusbyte & 0x0F);
                Track track = tracks[(t * 0xf) + tracknumber];

                if ((statusbyte & 0xF0) == 0x80
                    || (statusbyte & 0xF0) == 0x90)
                {//NOTEOFF / NOTEON
                    uint key = midi[i++];
                    uint velocity = midi[i++];
                    if (key > 127)
                    {
                        R.Error("このmidiはkeyが127以上です. {0}",key); Debug.Assert(false);
                        key = 127;
                    }
                    if (velocity > 127)
                    {
                        R.Error("このmidiはvelocityが127以上です. {0}", velocity); Debug.Assert(false);
                        velocity = 127;
                    }
                    uint index = key * 0xf + tracknumber;

                    if ((statusbyte & 0xF0) == 0x80
                        || velocity == 0
                        )
                    {//音を消す //仮にNOTEONでも vekocityを0にすると音を消すことができる.
                        //やはり、midi formatはおかしい.
                        MidiDelta_to_GBA_Note(track, playKey[index], total_time, onpu4);
                        playKey[index] = null;
                    }
                    else
                    {//音を出す.
                        MidiKey m = new MidiKey();
                        m.velocity = velocity;
                        m.time = total_time;
                        m.key = key;
                        playKey[index] = m;
                    }
                }
                else if ((statusbyte & 0xF0) == 0xA0)
                {//キーアフタータッチ
                    uint arg1 = midi[i++]; //コントロール
                    uint arg2 = midi[i++]; //値
                    Log.Notify("Skip", total_time.ToString(), "key after touch", arg1.ToString(), arg2.ToString());
                    continue;
                }
                else if ((statusbyte & 0xF0) == 0xB0)
                {//コントロールイベント
                    uint arg1 = midi[i++]; //コントロール
                    uint arg2 = midi[i++]; //値
                    if (arg1 == 0x7)
                    {//音量
                        track.codes.Add(new Code(total_time, total_time, 0xBE, expVol(arg2)));
                    }
                    else if (arg1 == 0xA)
                    {//PAN
                        int pan = ((int)arg2) + (int)0;
                        track.codes.Add(new Code(total_time, total_time, 0xBF, (uint)pan));
                    }
                    else if (arg1 == 0x14)
                    {//BENDR
                        track.codes.Add(new Code(total_time, total_time, 0xC1, arg2));
                    }
                    else if (arg1 == 0x15)
                    {//LFOS
                        track.codes.Add(new Code(total_time, total_time, 0xC2, arg2));
                    }
                    else if (arg1 == 0x1A)
                    {//LFODL
                        track.codes.Add(new Code(total_time, total_time, 0xC3, arg2));
                    }
                    else if (arg1 == 0x01)
                    {//MOD
                        track.codes.Add(new Code(total_time, total_time, 0xC4, arg2));
                    }
                    else if (arg1 == 0x16)
                    {//MODT
                        track.codes.Add(new Code(total_time, total_time, 0xC5, arg2));
                    }
                    else if (arg1 == 0x18)
                    {//TUNE
                        track.codes.Add(new Code(total_time, total_time, 0xC8, arg2));
                    }
                    else if (arg1 == 0x5B)
                    {//ビブラート(リバーブ)
                        track.codes.Add(new Code(total_time, total_time, REVERB_CODE, arg2));
                    }
                }
                else if ((statusbyte & 0xF0) == 0xC0)
                {//楽器変更 //この命令だけ1つしか引数がない・・・ midi formatは、やはりおかしい.
                    uint arg1 = midi[i++];

                    track.codes.Add(new Code(total_time, total_time, 0xBD, arg1));
                }
                else if ((statusbyte & 0xF0) == 0xD0)
                {//ChannelA fter Touch
                    uint arg1 = midi[i++]; //コントロール
                    uint arg2 = midi[i++]; //値
                    Log.Notify("Skip", total_time.ToString(), "ChannelA fter Touch", arg1.ToString(), arg2.ToString());
                    continue;
                }
                else if ((statusbyte & 0xF0) == 0xE0)
                {//PITCHBEND  BEND
                    uint arg1 = midi[i++];
                    uint arg2 = midi[i++];
                    uint bend = CalcPitchBendMidiToGBA(arg1, arg2);
                    track.codes.Add(new Code(total_time, total_time, 0xC0, bend));
                }
                else if (statusbyte == 0xF7 || statusbyte == 0xF0)
                {//SysExイベント see https://sites.google.com/site/yyagisite/material/smfspec
                    //0xF0 [length] data.... 0xF0
                    //        or
                    //0xF7 [length] data....
                    uint length = U.read_vlength_code(midi, i, out i);
                    i = i + length;
                }
                else if (statusbyte == 0xFF)
                {//特殊コマンド系.
                    uint arg = midi[i++];
                    uint length = U.read_vlength_code(midi, i, out i);
                    uint dataI = i;
                    //メタイベント (0xFF だと思われる)
                    i = i + length;

                    if (arg == 0x2F)
                    {//終端
                        //FINEは使えないので、適当なダミーコードを入れる.
                        condTrack.codes.Add(new Code(total_time
                            , DeltaTimeToGBATime(total_time, onpu4)
                            , DUMMY_CODE, loopstart));
                        break;
                    }
                    else if (arg == 0x51)
                    {//テンポ変更
                        uint tempo = U.big24(midi, dataI);

                        uint gba_tempo = (uint)(60000000.0 / (tempo * 2));
                        condTrack.codes.Add(new Code(total_time
                            , DeltaTimeToGBATime(total_time, onpu4)
                            , 0xBB, gba_tempo));
                    }
                    else if (arg >= 0x1 && arg <= 0x7)
                    {//ラベル
                        string labelstr = U.getASCIIString(midi, dataI, (int)length);

                        if (labelstr == "loopStart" || labelstr == "[")
                        {
                            loopstart = total_time;
                            condTrack.codes.Add(new Code(total_time
                                , DeltaTimeToGBATime(total_time, onpu4)
                                , LOOP_LABEL_CODE, loopstart));
                        }
                        else if (labelstr == "loopEnd" || labelstr == "]")
                        {
                            if (loopstart == U.NOT_FOUND)
                            {
                                R.Error("loopStartがないのに、loopEndがありました。無視します。");
                            }
                            else
                            {
                                condTrack.codes.Add(new Code(total_time
                                    , DeltaTimeToGBATime(total_time, onpu4)
                                    , 0xB2, loopstart));
                            }
                        }
                    }
                    else
                    {
                        string labelstr = U.getASCIIString(midi, dataI, (int)length);
                        Log.Debug(labelstr);
                    }
                }
                else
                {//未対応のコード
                    //とりあえず2バイトほど読み飛ばす.
                    i += 2;
                }
            }

            return i;
        }

        static uint CalcPitchBendMidiToGBA(uint arg1, uint arg2)
        {
            return arg2;
        }
        static void CalcPitchBendGBAToMidi(uint arg, out uint out_arg1, out uint out_arg2)
        {
            out_arg1 = 0;
            out_arg2 = arg;
        }

        public static string ImportMidiFileMID2AGB(string filename, uint songtable_address
            , uint instrument_addr
            , int option_v
            , int option_r
            , bool ignoreMOD
            , bool ignoreBEND
            , bool ignoreLFOS
            )
        {
            string error;
            bool r = MainFormUtil.CompilerMID2AGB(filename, option_v, option_r, out error);
            if (r == false)
            {
                return R.Error("mid2agbがエラーになりました。\r\n") + error;
            }
            //正しい場合、sファイル名が格納される.
            string tempSFile = error;
            if (ignoreMOD || ignoreBEND || ignoreLFOS)
            {
                LevelingSFile(tempSFile, ignoreMOD, ignoreBEND, ignoreLFOS);
            }
            return ImportS(tempSFile, songtable_address, instrument_addr);
        }
        static bool LevelingSFile(string filename
            ,bool ignoreMOD
            ,bool ignoreBEND
            ,bool ignoreLFOS
            )
        {
            if (ignoreMOD)
            {
                LevelingSFileLow(filename, "MOD");
            }
            if (ignoreBEND)
            {
                LevelingSFileLow(filename, "BEND");
            }
            if (ignoreLFOS)
            {
                LevelingSFileLow(filename, "LFOS");
            }

            return true;
        }

        static bool LevelingSFileLow(string filename
            , string matchName
            )
        {
            string[] lines = File.ReadAllLines(filename);
            List<string> outLines = new List<string>(lines.Length);
            string needString = ".byte		" + matchName;

            bool is_match = false;
            foreach (string l in lines)
            {

                if (l.IndexOf(needString) >= 0)
                {
                    is_match = true;
                    continue;
                }
                if (is_match)
                {
                    if (l.IndexOf(".byte		        c_v") >= 0)
                    {//直後の命令には、BENDやMODは含まれないので、これで消す
                        continue;
                    }
                    else if (l.IndexOf(".byte	W") >= 0)
                    {//直前のwaitはそのままにしてよい.
                    }
                    else
                    {
                        is_match = false;
                    }
                }
                outLines.Add(l);
            }
            File.WriteAllLines(filename, outLines);
            return true;
        }

        public static string ImportMidiFile(string filename, uint songtable_address
            , uint instrument_addr
            , bool ignoreMOD
            , bool ignoreBEND
            , bool ignoreLFOS
            , bool ignoreHEAD
            , bool ignoreBACK
            )
        {
            uint songheader_address = Program.ROM.p32(songtable_address + 0);

            byte[] midi = File.ReadAllBytes(filename);

            if (midi[0] != 'M'
               || midi[1] != 'T'
               || midi[2] != 'h'
               || midi[3] != 'd'
                )
            {
                return R.Error("midiヘッダーがありません");
            }

            uint headersize = U.big32(midi, 4);
            uint format = U.big16(midi, 4 + 4);
            uint trackcount = U.big16(midi, 4 + 4 + 2);//track数
            uint onpu4 = U.big16(midi, 4 + 4 + 2 + 2);//四分音符の分解能

            uint pos = 4 + 4 + headersize;

            if (format == 1)
            {
                Log.Debug(filename, "format1");
                if (trackcount <= 1)
                {
                    return R.Error("指揮トラックしかありません");
                }
                trackcount--;
            }
            else if (format == 0)
            {
                Log.Debug(filename, "format0");
            }
            else
            {
                return R.Error("未対応の midi formatです");
            }

            //最大トラック数を確保、後で使わないのは削る.
            Track[] tracks = new Track[0xF * (trackcount + 1)];
            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i] = new Track();
            }

            Track condTrack = new Track();
            if (format == 1)
            {
                InputFormRef.DoEvents(null, "Track:Cond");
                pos = ParseMidi(midi, pos, tracks, condTrack, 0, onpu4);
            }

            for (uint t = 0; t < trackcount; t++)
            {
                InputFormRef.DoEvents(null, "Track:" + t);

                pos = ParseMidi(midi, pos, tracks, condTrack, t, onpu4);
                if (pos == U.NOT_FOUND)
                {
                    break;
                }
            }
            uint reverb = GetReverb(tracks, condTrack);
            filterMidiTrack(tracks, condTrack, ignoreMOD, ignoreBEND, ignoreLFOS, ignoreHEAD, ignoreBACK);
            AppendInitVolTrack(tracks);
            List<Track> ret_tracks = margeMidiTrack(tracks, condTrack, onpu4, format);
            if (ret_tracks.Count >= 16)
            {
                return R.Error("トラック数が16トラックを超えています。GBAでは、最大16トラックまでしか利用できません。");
            }

            string tempSFile = filename + ".s";
            ExportSFile(tempSFile, "m", ret_tracks, ret_tracks.Count, 0xA, (int)reverb, instrument_addr);
            if (songtable_address == 0)
            {
                return "";
            }
            return ImportS(tempSFile, songtable_address, instrument_addr);
        }

        static List<Track> margeMidiTrack(Track[] tracks
            , Track condTrack
            , uint onpu4
            , uint format
            )
        {
            //一番長いトラックの検出
            uint maxWaitCount = 0;
            uint maxAddr = 0;
            if (condTrack.codes.Count > 0)
            {
                Code c = condTrack.codes[condTrack.codes.Count - 1];
                maxWaitCount = c.waitCount;
                maxAddr = c.addr;
            }
            for (int i = 0; i < tracks.Length; i++)
            {
                if (tracks[i].codes.Count <= 0)
                {
                    continue;
                }
                Code c = tracks[i].codes[tracks[i].codes.Count - 1];
                if (c.waitCount > maxWaitCount)
                {
                    maxWaitCount = c.waitCount;
                }
                if (c.addr > maxAddr)
                {
                    maxAddr = c.addr;
                }
            }

            //ループが存在しないなら追加. ついでにFINEも
            AppendFineLoopIfYet(condTrack, maxAddr, maxWaitCount);

            List<Track> ret_tracks = new List<Track>();
            for (int i = 0; i < tracks.Length; i++)
            {
                //使わないトラックをそぎ落とします.
                if (isNullTrack(tracks[i]))
                {
                    continue;
                }

                //指揮トラックの命令を追加します.
                InsertCondEvent(condTrack, tracks[i]);

                //トラック最適化
                OptimizeTrackPATT(tracks[i]);
                //トラック最適化 楽譜最適化Nxxの省略
                OptimizeTrackNxx(tracks[i]);

                //Wait命令を追加します.
                InsertWait(tracks[i], onpu4);

                //利用するトラックとして格納.
                ret_tracks.Add(tracks[i]);
            }

            return ret_tracks;
        }

        static void filterMidiTrack(Track track
            , bool ignoreMOD
            , bool ignoreBEND
            , bool ignoreLFOS
            )
        {
            for (int i = 0; i < track.codes.Count; )
            {
                Code c = track.codes[i];
                if (ignoreMOD)
                {
                    if (c.type == 0xC5 || c.type == 0xC4)
                    {
                        track.codes.RemoveAt(i);
                        continue;
                    }
                }
                if (ignoreLFOS)
                {
                    if (c.type == 0xC2 || c.type == 0xC3)
                    {
                        track.codes.RemoveAt(i);
                        continue;
                    }
                }
                if (ignoreBEND)
                {
                    if (c.type == 0xC1 || c.type == 0xC0)
                    {
                        track.codes.RemoveAt(i);
                        continue;
                    }
                }
                i++;
            }
        }
        static uint SearchHeadNullWait(Track track)
        {
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                if (c.type >= EOT && c.type <= NOTE_END)
                {
                    return c.addr;
                }
            }
            return U.NOT_FOUND;
        }
        static uint SearchBackNullWait(Track track)
        {
            for (int i = track.codes.Count - 1; i >= 0; i--)
            {
                Code c = track.codes[i];
                if (c.type >= EOT && c.type <= NOTE_END)
                {
                    return c.addr;
                }
            }

            return 0;
        }
        static void TrimHeadNullWait(Track track, uint minHeadNullAddr)
        {
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                if (c.addr < minHeadNullAddr)
                {
                    c.addr = 0;
                }
                else
                {
                    c.addr -= minHeadNullAddr;
                }
                //念のため..
                if (c.waitCount < minHeadNullAddr)
                {
                    c.waitCount = 0;
                }
                else
                {
                    c.waitCount -= minHeadNullAddr;
                }
            }
        }
        static void TrimBackNullWait(Track track, uint maxBackNullAddr)
        {
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                if (c.addr > maxBackNullAddr)
                {
                    c.addr = maxBackNullAddr;
                }
                //念のため..
                if (c.waitCount > maxBackNullAddr)
                {
                    c.waitCount = maxBackNullAddr;
                }
            }
        }
        static uint GetReverb(Track track)
        {
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                if (c.type == REVERB_CODE)
                {
                    return c.value;
                }
            }
            return U.NOT_FOUND;
        }

        static uint GetReverb(Track[] tracks, Track condTrack)
        {
            uint reverb = GetReverb(condTrack);
            if (reverb != U.NOT_FOUND)
            {
                return reverb;
            }
            for (int i = 0; i < tracks.Length; i++)
            {
                reverb = GetReverb(condTrack);
                if (reverb != U.NOT_FOUND)
                {
                    return reverb;
                }
            }
            //なし
            return 0;
        }
        static void AppendInitVolTrack(Track[] tracks)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                if (tracks[i].codes.Count <= 0)
                {
                    continue;
                }
                bool foundkeysh = CheckTypeInHeader(tracks[i], 0xBC, 10);
                if (!foundkeysh)
                {
                    tracks[i].codes.Insert(0,new Code(0,0,0xBC,0,0));
                }
                bool foundVol = CheckTypeInHeader(tracks[i], 0xBE, 10);
                if (!foundVol)
                {
                    tracks[i].codes.Insert(0, new Code(0, 0, 0xBE, 80, 0));
                }
            }
        }
        static bool CheckTypeInHeader(Track track , uint code,int limit=0)
        {
            int count;
            if (limit <= 0)
            {
                count = track.codes.Count;
            }
            else
            {
                count = Math.Min(limit, track.codes.Count);
            }

            for (int i = 0; i < count; i++)
            {
                Code c = track.codes[i];
                if (c.type == code)
                {
                    return true;
                }
            }
            return false;
        }
        static void filterMidiTrack(Track[] tracks, Track condTrack
            , bool ignoreMOD
            , bool ignoreBEND
            , bool ignoreLFOS
            , bool ignoreHEAD
            , bool ignoreBACK)
        {
            if (condTrack.codes.Count > 0)
            {
                filterMidiTrack(condTrack, ignoreMOD, ignoreBEND, ignoreLFOS);
                sortTrackByAddr(condTrack);
            }
            for (int i = 0; i < tracks.Length; i++)
            {
                if (tracks[i].codes.Count <= 0)
                {
                    continue;
                }
                filterMidiTrack(tracks[i], ignoreMOD, ignoreBEND, ignoreLFOS);
                sortTrackByAddr(tracks[i]);
            }


            if (ignoreHEAD)
            {
                uint minHeadNullAddr = U.NOT_FOUND;
                {
                    uint wait = SearchHeadNullWait(condTrack);
                    if (wait < minHeadNullAddr)
                    {
                        minHeadNullAddr = wait;
                    }
                }
                for (int i = 0; i < tracks.Length; i++)
                {
                    if (isNullTrack(tracks[i]))
                    {
                        continue;
                    }

                    uint wait = SearchHeadNullWait(tracks[i]);
                    if (wait < minHeadNullAddr)
                    {
                        minHeadNullAddr = wait;
                    }
                }

                if (minHeadNullAddr != U.NOT_FOUND)
                {
                    TrimHeadNullWait(condTrack, minHeadNullAddr);
                    for (int i = 0; i < tracks.Length; i++)
                    {
                        TrimHeadNullWait(tracks[i], minHeadNullAddr);
                    }
                }
            }

            if (ignoreBACK)
            {
                uint maxBackNullAddr = 0;

                {
                    uint wait = SearchBackNullWait(condTrack);
                    if (wait > maxBackNullAddr)
                    {
                        maxBackNullAddr = wait;
                    }
                }
                for (int i = 0; i < tracks.Length; i++)
                {
                    if (isNullTrack(tracks[i]))
                    {
                        continue;
                    }

                    uint wait = SearchBackNullWait(tracks[i]);
                    if (wait > maxBackNullAddr)
                    {
                        maxBackNullAddr = wait;
                    }
                    TrimBackNullWait(tracks[i], maxBackNullAddr);
                }

                if (maxBackNullAddr != 0)
                {
                    TrimBackNullWait(condTrack, maxBackNullAddr);
                    for (int i = 0; i < tracks.Length; i++)
                    {
                        TrimBackNullWait(tracks[i], maxBackNullAddr);
                    }
                }
            }
        }


        static void sortTrackByAddr(Track track)
        {
            track.codes.Sort((a, b) =>
            {
                if (a.addr == b.addr)
                {
                    return ((int)a.type) - ((int)b.type);
                }
                return ((int)a.addr) - ((int)b.addr);
            });
        }

        //ループが存在しないなら追加.
        static void AppendFineLoopIfYet(Track track, uint maxAddr, uint maxWaitCount)
        {
            //ループが存在するかチェック.
            bool loopFound = CheckTypeInHeader(track, 0xb2);

            if (!loopFound)
            {//ループがないなら、最初に戻すループを追加する.
                track.codes.Insert(0, new Code(0
                    , 0
                    , LOOP_LABEL_CODE
                    ));
                track.codes.Add(new Code(maxAddr
                    , maxWaitCount
                    , 0xB2
                    , 0));
            }

            //最後の部分にFINEを追加.
            track.codes.Add(new Code(maxAddr
                , maxWaitCount
                , 0xB1));
        }


        //小節ごとに分類
        class SplitTuneData
        {
            public uint StartWaitCount;
            public uint EndWaitCount;
            public int StartDataIndex;
            public int EndDataIndex;
            public int TuneCount;

            public int patt_to; //call
            public int pend_to; //ret
        };

        static List<SplitTuneData> SplitTune(Track track)
        {
            List<SplitTuneData> splittunes = new List<SplitTuneData>();
            const uint tempo = 96;
            uint lastTuneWait = 0;
            int lastIndex = 0;
            int TuneCount = 0;
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code code = track.codes[i];
                TuneCount++;
                if (code.waitCount >= lastTuneWait + tempo)
                {
                    if (code.type == 0xb4 || code.type == 0xb1 || code.type == 0xb3)
                    {//PEND FINE は、例外として、最後のトラックに記載する.
                    }
                    else
                    {//次のトラックへ
                        SplitTuneData ts = new SplitTuneData();
                        ts.StartWaitCount = lastTuneWait;
                        ts.EndWaitCount = code.waitCount;
                        ts.StartDataIndex = lastIndex;
                        ts.EndDataIndex = i;
                        ts.TuneCount = TuneCount;
                        ts.patt_to = -1;
                        ts.pend_to = -1;
                        splittunes.Add(ts);
                        lastTuneWait = code.waitCount;
                        lastIndex = i;
                        TuneCount = 0;
                    }
                }
            }
            if (TuneCount > 0)
            {
                SplitTuneData ts = new SplitTuneData();
                ts.StartWaitCount = lastTuneWait;
                ts.EndWaitCount = track.codes[track.codes.Count - 1].waitCount + 1;
                ts.StartDataIndex = lastIndex;
                ts.EndDataIndex = track.codes.Count;
                ts.TuneCount = TuneCount;
                ts.patt_to = -1;
                ts.pend_to = -1;

                splittunes.Add(ts);
            }
            return splittunes;
        }

        //トラック最適化
        static void OptimizeTrackPATT(Track track)
        {
            List<Code> ret_codes = new List<Code>();

            //小節ごとに再利用できるところはくくりだす.
            List<SplitTuneData> splittunes = SplitTune(track);
            for (int i = 0; i < splittunes.Count - 1; i++)
            {
                SplitTuneData src = splittunes[i];
                if (src.TuneCount <= 1)
                {//♪が1つ以下ならくくりだすデメリットの方が大きく、バイト数が逆に大きくなるので、無視する.
                    continue;
                }
                uint tempoWait = src.EndWaitCount - src.StartWaitCount;

                for (int n = 0; n < i; n++)
                {
                    SplitTuneData dest = splittunes[n];
                    if (src.TuneCount != dest.TuneCount)
                    {//♪の数が違うならマッチするわけない.
                        continue;
                    }
                    uint destTempoWait = dest.EndWaitCount - dest.StartWaitCount;
                    if (tempoWait != destTempoWait)
                    {//小節の時間が違う
                        continue;
                    }

                    if (compareTune(track, src, dest))
                    {
                        src.patt_to = n;
                        dest.pend_to = i;
                        break;
                    }
                }
            }

            for (int i = 0; i < splittunes.Count; i++)
            {
                SplitTuneData s = splittunes[i];
                if (s.patt_to >= 0)
                {
                    Code origC = track.codes[s.StartDataIndex];

                    SplitTuneData d = splittunes[s.patt_to];
                    Code loopC = track.codes[d.StartDataIndex];

                    Code c = new Code(origC.addr
                        , origC.waitCount
                        , 0xb3  //PATT
                        , loopC.addr);
                    ret_codes.Add(c);
                    continue;
                }

                if (s.pend_to >= 0)
                {//PENDのためのラベルを追加.
                    Code c = track.codes[s.StartDataIndex];
                    ret_codes.Add(new Code(c.addr
                        , c.waitCount
                        , LOOP_LABEL_CODE
                        ));
                }
                //pattサブルーチン化しない場合、そのままコピー
                for (int k = s.StartDataIndex; k < s.EndDataIndex; k++)
                {
                    Code c = track.codes[k];
                    ret_codes.Add(c);
                }

                if (s.pend_to >= 0)
                {//PENDを挿入する必要がある.
                    Debug.Assert(s.TuneCount >= 2);
                    Code lastC = track.codes[s.EndDataIndex];

                    Code c = new Code(lastC.addr
                        , lastC.waitCount
                        , 0xb4); //PEND
                    ret_codes.Add(c);
                }
            }

            track.codes = ret_codes;
        }
        static bool compareTune(Track track, SplitTuneData src, SplitTuneData dest)
        {
            for (int k = 0; k < src.TuneCount; k++)
            {
                Code srcc = track.codes[k + src.StartDataIndex];
                Code destc = track.codes[k + dest.StartDataIndex];
                if (srcc.type != destc.type
                    || srcc.value != destc.value
                    || srcc.value2 != destc.value2)
                {
                    return false;
                }
            }
            return true;
        }
        //楽譜最適化
        static void OptimizeTrackNxx(Track track)
        {
            uint lastType = U.NOT_FOUND;
            uint lastKey = U.NOT_FOUND;
            uint lastVelocity = U.NOT_FOUND;
            uint lastWait = U.NOT_FOUND;
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                uint n = c.type;
                if (n < TIE || n > NOTE_END)
                {
                    lastType = U.NOT_FOUND;
                    lastKey = U.NOT_FOUND;
                    lastVelocity = U.NOT_FOUND;
                    lastWait = U.NOT_FOUND;
                    continue;
                }

                uint type = c.type;
                uint key = c.value;
                uint velocity = c.value2;
                uint wait = c.waitCount;

                if (type == lastType
                    && wait != lastWait //同一タイム(和音)のときには Nxxを省略してはいけないらしい
                 )
                {//Nxxが同一
                    if (velocity == lastVelocity)
                    {//keyだけ残す.
                        c.type = key;
                        c.value = U.NOT_FOUND;
                        c.value2 = U.NOT_FOUND;
                    }
                    else
                    {//keyと vel
                        c.type = key;
                        c.value = velocity;
                        c.value2 = U.NOT_FOUND;
                    }
                }
                else if (velocity == lastVelocity)
                {//Nxxが違う ベロシティは同一
                    if (key == lastKey)
                    {
                        c.type = type;
                        c.value = U.NOT_FOUND;
                        c.value2 = U.NOT_FOUND;
                    }
                    else
                    {
                        c.type = type;
                        c.value = key;
                        c.value2 = U.NOT_FOUND;
                    }
                }
                else
                {
                    //nop
                }
                lastType = type;
                lastKey = key;
                lastVelocity = velocity;
                lastWait = wait;
            }
        }

        static bool isNullTrack(Track track)
        {
            if (track.codes.Count <= 0)
            {
                return true;
            }

            //♪が一つでもあるかどうか.
            for (int i = 0; i < track.codes.Count; i++)
            {
                if (track.codes[i].type >= EOT && track.codes[i].type <= NOTE_END)
                {
                    return false;
                }
            }
            //♪が一つもない.
            return true;
        }

        static int findLabel(uint label, List<Code> codes)
        {
            for (int i = 0; i < codes.Count; i++)
            {
                Code code = codes[i];
                if (code.type == LOOP_LABEL_CODE)
                {//LOOP_LABEL_CODE
                    if (code.addr == label)
                    {
                        return i;
                    }
                }
            }
            //見つからない
            return 0xFFFFFF;
        }
        //ウェイトが分解されるか確認
        static uint searchMinimamWait(uint currentWaitCountPos
            , uint wait
            , List<PlayKeys> playKeys)
        {
            uint min = wait;
            for (uint w = 0; w < wait; w++)
            {
                for (int n = 0; n < playKeys.Count; n++)
                {
                    if (playKeys[n].stopTime > currentWaitCountPos + w)
                    {//まだ停止させる時間に行っていない.
                        continue;
                    }
                    if (min > w)
                    {
                        min = w;
                    }
                }
            }
            return min;
        }

        static uint DeltaTimeToGBATime(uint delta, uint onpu4)
        {
            if (onpu4 == 0)
            {
                return 0;
            }
            return delta / (onpu4 / 24);
        }

        static void MidiDelta_to_GBA_Note(Track track, MidiKey playKey, uint nowDeltaTime, uint onpu4)
        {
            if (playKey == null)
            {//既に音は消えている.
                return;
            }
            uint gba_time = DeltaTimeToGBATime(nowDeltaTime - playKey.time, onpu4);
            uint addr = playKey.time;
            /*
                        if (gba_time > 96)
                        {//96以上の時間鳴り続ける場合、TIEにしてみる.
                            //TIE key,vXX
                            track.codes.Add(new Code(addr
                                , DeltaTimeToGBATime(addr, onpu4)
                                , TIE
                                , playKey.key
                                , playKey.velocity));

                            //EOT で停止命令
                            addr += gba_time;
                            track.codes.Add(new Code(addr
                                , DeltaTimeToGBATime(addr, onpu4)
                                , EOT
                                ));
                            return;
                        }
                        */
            while (gba_time > 0)
            {
                for (int i = WaitCode.Length - 1; i >= 0;)
                {
                    if (WaitCode[i] <= gba_time)
                    {
                        //Nxx key,vXX
                        track.codes.Add(new Code(addr
                            , DeltaTimeToGBATime(addr, onpu4)
                            , (uint)(NOTE_START + i - 1)
                            , playKey.key
                            , playKey.velocity));
                        addr += (uint)WaitCode[i];
                        gba_time -= (uint)WaitCode[i];
                        if (gba_time <= 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }
        static void InsertWait(Track track, uint onpu4)
        {
            List<Code> ret_codes = new List<Code>();

            //番兵として、一番最後の命令をもう一度入れる
            Code lastCode = track.codes[track.codes.Count - 1];
            uint stopTime = lastCode.addr;
            if (lastCode.type >= TIE && lastCode.type <= (uint)NOTE_END)
            {
                stopTime = byteToNote(lastCode.type) + lastCode.addr;
            }
            track.codes.Add(new Code(stopTime, 0, 0));


            for (int k = 0; k < track.codes.Count - 1; k++)
            {
                Code code = track.codes[k];
                Code nextcode = track.codes[k + 1];
                ret_codes.Add(code);
                if (code.type == 0xb3)
                {//PATT 小節呼び出しなので、waitは関係ないです.
                    continue;
                }
                if (code.addr >= nextcode.addr)
                {//差分時間がない.
                    continue;
                }
                //差分時間の取得.
                uint difftime = DeltaTimeToGBATime(nextcode.addr - code.addr, onpu4);
                uint addr = code.addr;

                //差分時間をWxxに変換します.
                while (difftime > 0)
                {
                    for (int i = WaitCode.Length - 1; i >= 0; )
                    {
                        if (WaitCode[i] <= difftime)
                        {
                            //Wxx
                            ret_codes.Add(new Code(addr
                                , DeltaTimeToGBATime(addr, onpu4)
                                , (uint)(WAIT_START + i)
                            ));
                            addr += (uint)WaitCode[i];
                            difftime -= (uint)WaitCode[i];
                            if (difftime <= 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            i--;
                        }
                    }
                }
            }
            track.codes = ret_codes;
        }

        static void InsertCondEvent(Track condTrack, Track track)
        {
            int condN = 0;

            List<Code> ret_codes = new List<Code>();
            for (int k = 0; k < track.codes.Count; k++)
            {
                Code code = track.codes[k];
                while (condN < condTrack.codes.Count)
                {
                    Code codeK = condTrack.codes[condN];
                    if (codeK.type == LOOP_LABEL_CODE
                        || (codeK.type > WAIT_END && codeK.type < EOT))
                    {//ループラベルかコントロールコードならば、先頭に入れいます.
                        if (code.addr < codeK.addr)
                        {//入れる場所ではない
                            break;
                        }
                    }
                    else
                    {//ループラベル以外は、後方に入れます.
                        if (code.addr <= codeK.addr)
                        {//入れる場所ではない
                            break;
                        }
                    }
                    ret_codes.Add(codeK);
                    condN++;
                }

                ret_codes.Add(code);
            }

            while (condN < condTrack.codes.Count)
            {
                ret_codes.Add(condTrack.codes[condN]);
                condN++;
            }
            track.codes = ret_codes;
        }

        public static void RecycleOldSong(ref List<Address> recycle,string basename, uint track_basepointer)
        {
            uint track_baseaddress = Program.ROM.u32(track_basepointer);
            if (!U.isPointer(track_baseaddress))
            {
                return ;
            }
            track_baseaddress = U.toOffset(track_baseaddress);
            if (!U.isSafetyOffset(track_baseaddress))
            {
                return ;
            }

            //トラックデータ.
            uint trackcount = Program.ROM.u8(track_baseaddress);
            FEBuilderGBA.Address.AddPointer(recycle
                , track_basepointer
                , 8 + trackcount * 4
                , basename + "HEADER"
                , FEBuilderGBA.Address.DataTypeEnum.SONGTRACK);

            List<Track> tracks = ParseTrack(track_baseaddress, trackcount);
            for (int i = 0; i < tracks.Count; i++)
            {
                int len = tracks[i].codes.Count;
                if (len <= 0)
                {
                    continue;
                }
                uint startaddr = tracks[i].codes[0].addr;
                uint fineaddr = tracks[i].codes[len - 1].addr;
                FEBuilderGBA.Address.AddPointer(recycle
                    , tracks[i].basepointer
                    , U.Padding4(fineaddr - startaddr + 1)
                    , basename + "TRACK " + U.To0xHexString(i)
                    , FEBuilderGBA.Address.DataTypeEnum.SONGSCORE);
            }
        }


        class SongInnerDataSt
        {
            public int globalID; //list[n] と同じ値.
            public string name;
            public uint ROMAllocAddr;
            public List<int> useLabelRegist;
            public List<byte> list;
        };

        public static string ImportS(string filename, uint songtable_address, uint instrument_addr)
        {
            uint songheader_address = Program.ROM.p32(songtable_address + 0);

            Dictionary<string, int> equ = new Dictionary<string, int>();
            equ["voicegroup000"] = -1; //楽器テーブル　例外的な扱いをするので適当な値を入れる
            equ["MusicVoices"] = -1; //楽器テーブル　例外的な扱いをするので適当な値を入れる
            equ["mxv"] = (int)0x7F;
            equ["c_v"] = (int)0x40;
            equ["EOT"] = 0xCE;
            equ["FINE"] = 0xB1;
            equ["GOTO"] = 0xb2;
            equ["PATT"] = 0xb3;
            equ["PEND"] = 0xb4;
            equ["MEMACC"] = 0xb9;
            equ["PRIO"] = 0xba;
            equ["TEMPO"] = 0xbb;
            equ["KEYSH"] = 0xbc;
            equ["VOICE"] = 0xbd;
            equ["VOL"] = 0xbe;
            equ["PAN"] = 0xbf;
            equ["BEND"] = 0xc0;
            equ["BENDR"] = 0xc1;
            equ["LFOS"] = 0xc2;
            equ["LFODL"] = 0xc3;
            equ["MOD"] = 0xc4;
            equ["MODT"] = 0xc5;
            equ["TUNE"] = 0xc8;
            equ["gtp1"] = 0x01;
            equ["gtp2"] = 0x02;
            equ["gtp3"] = 0x03;

            for (uint i = WAIT_START; i <= WAIT_END; i++)
            {
                equ["W" + byteToWait(i).ToString("00")] = (int)i;
            }
            equ["TIE"] = (int)TIE;
            for (uint i = TIE + 1; i <= NOTE_END; i++)
            {
                equ["N" + byteToNote(i).ToString("00")] = (int)i;
            }
            for (uint i = 0; i <= 127; i++)
            {
                equ[getKeyCode(i)] = (int)i;
            }
            for (uint i = 0; i <= 127; i++)
            {
                equ["v" + i.ToString("000")] = (int)i;
            }
            for (uint i = 0; i < MEMACC.Length; i++)
            {
                equ[MEMACC[i]] = (int)i;
            }
            //16進数の場合があるらしいので、ある程度のルックアップテーブルを作ろう.
            for (uint i = 0; i < 0xf; i++)
            {
                equ["0x" + i.ToString("X")] = (int)i;
            }
            for (uint i = 0; i < 0xff; i++)
            {
                equ[U.To0xHexString(i)] = (int)i;
            }

            string[] lines = File.ReadAllLines(filename);

            string globalName = null;
            List<SongInnerDataSt> global = new List<SongInnerDataSt>();
            SongInnerDataSt current = null;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                //@以降はコメントなので消し去る.
                line = U.ClipCommentWithCharpAndAtmark(line);

                if (line.Length <= 1)
                {
                    continue;
                }
                if (i % 128 == 0)
                {
                    InputFormRef.DoEvents(null, "Lines:" + i);
                }

                //トークン分割   RemoveEmptyEntries で空値は無視.
                string[] token = line.Split(new string[] { " ", "\t", "," }, StringSplitOptions.RemoveEmptyEntries);
                if (token.Length <= 0)
                {
                    continue;
                }
                if (token[0] == ".equ")
                {
                    if (token.Length <= 2)
                    {//エラー
                        return R.Error(".equは2つの引数が必要です.\r\n例: .equ name, value\r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }
                    string v = "";
                    for (int n = 2; n < token.Length; n++)
                    {
                        v += token[n];
                    }
                    try
                    {
                        equ[token[1]] = Expr(v, equ);
                    }
                    catch (SyntaxErrorException e)
                    {
                        string errorMessage = e.ToString() + "\r\n" + R._("説明:\r\n") + MainFormUtil.GetExplainOfSFileURL();
                        return R.Error("{2}\r\n\r\nFile:{0} Line:{1}", filename, i + 1, errorMessage);
                    }
                    catch (EvaluateException e)
                    {
                        string errorMessage = e.ToString() + "\r\n" + R._("説明:\r\n") + MainFormUtil.GetExplainOfSFileURL();
                        return R.Error("{2}\r\n\r\nFile:{0} Line:{1}", filename, i + 1, errorMessage);
                    }
                    continue;
                }
                if (token[0] == ".global")
                {
                    if (globalName != null)
                    {
                        InputFormRef.DoEvents(null, "Lines:" + i);
                        return R.Error("global が2回定義されました。\r\n例: global aaa\r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }
                    globalName = token[1];
                    continue;
                }

                if (token[0].IndexOf(":") == token[0].Length - 1)
                {//ラベル
                    if (globalName == null)
                    {
                        InputFormRef.DoEvents(null, "Lines:" + i);
                        return R.Error("ラベルを書く前に、global 情報を定義してください.\r\nこのラベルがグローバルなのか、ローカルなのか調べるのに必要です。\r\n例: global aaa\r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }

                    string name = token[0].Substring(0, token[0].Length - 1);
                    if (isGlobalLabel(globalName, token[0]))
                    {
                        if (findGlobal(global, name) >= 0)
                        {
                            InputFormRef.DoEvents(null, "Lines:" + i);
                            return R.Error("グローバルラベル{2} すべてに利用されています。\r\n\r\nFile:{0} Line:{1}", filename, i + 1, name);
                        }
                        current = new SongInnerDataSt();
                        current.name = name;
                        current.useLabelRegist = new List<int>();
                        current.list = new List<byte>();
                        current.globalID = global.Count;
                        global.Add(current);
                    }
                    else
                    {
                        if (current == null)
                        {
                            InputFormRef.DoEvents(null, "Lines:" + i);
                            return R.Error("グローバルラベルがないのに、ローカルラベルが使われました。\r\nまずはグローバルラベルを定義してください。 \r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                        }
                    }

                    equ[name] = (current.globalID << 24) + current.list.Count; //相対座標で記録.
                    continue;
                }

                if (token[0] == ".byte")
                {//byte単位で書き込む.
                    if (current == null)
                    {
                        InputFormRef.DoEvents(null, "Lines:" + i);
                        return R.Error("グローバルラベルがないのに、 .byte命令が使われました。\r\nまずはグローバルラベルを定義してください。 \r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }

                    if (token.Length <= 1)
                    {//エラー
                        InputFormRef.DoEvents(null, "Lines:" + i);
                        return R.Error(".byteは1つ以上の引数が必要です.\r\n例: .byte arg1....\r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }
                    for (int n = 1; n < token.Length; n++)
                    {
                        try
                        {
                            int v = Expr(token[n], equ);
                            current.list.Add((byte)v);
                        }
                        catch (SyntaxErrorException e)
                        {
                            InputFormRef.DoEvents(null, "Lines:" + i);
                            string errorMessage = e.ToString() + "\r\n" + R._("説明:\r\n") + MainFormUtil.GetExplainOfSFileURL();
                            return R.Error(".byteパース中にエラー {2} \r\n{3}\r\n\r\nFile:{0} Line:{1}", filename, i + 1, token[n], errorMessage);
                        }
                        catch (EvaluateException e)
                        {
                            InputFormRef.DoEvents(null, "Lines:" + i);
                            string errorMessage = e.ToString() + "\r\n" + R._("説明:\r\n") + MainFormUtil.GetExplainOfSFileURL();
                            return R.Error(".byteパース中にエラー {2} \r\n{3}\r\n\r\nFile:{0} Line:{1}", filename, i + 1, token[n], errorMessage);
                        }
                    }
                    continue;
                }
                if (token[0] == ".word")
                {//wordといったが実際が4バイトポインタ
                    //
                    if (current == null)
                    {
                        InputFormRef.DoEvents(null, "Lines:" + i);
                        return R.Error("グローバルラベルがないのに、 .word命令が使われました。\r\nまずはグローバルラベルを定義してください。 \r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }

                    if (token.Length <= 1)
                    {//エラー
                        InputFormRef.DoEvents(null, "Lines:" + i);
                        return R.Error(".wordは1つ以上のラベル引数が必要です.\r\n例: .byte arg1....\r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }
                    for (int n = 1; n < token.Length; n++)
                    {
                        try
                        {
                            uint v = (uint)Expr(token[n], equ);

                            {//それ以外の相対値 
                                current.useLabelRegist.Add(current.list.Count);
                            }

                            U.append_u32(current.list, v);
                        }
                        catch (SyntaxErrorException e)
                        {
                            InputFormRef.DoEvents(null, "Lines:" + i);
                            string errorMessage = e.ToString() + "\r\n" + R._("説明:\r\n") + MainFormUtil.GetExplainOfSFileURL();
                            return R.Error(".wordパース中にエラー {2} \r\n{3}\r\n\r\nFile:{0} Line:{1}", filename, i + 1, token[n], errorMessage);
                        }
                        catch (EvaluateException e)
                        {
                            InputFormRef.DoEvents(null, "Lines:" + i);
                            string errorMessage = e.ToString() + "\r\n" + R._("説明:\r\n") + MainFormUtil.GetExplainOfSFileURL();
                            return R.Error(".wordパース中にエラー {2} \r\n{3}\r\n\r\nFile:{0} Line:{1}", filename, i + 1, token[n], errorMessage);
                        }

                    }
                    continue;
                }
                if (token[0] == ".end")
                {
                    if (current == null)
                    {
                        InputFormRef.DoEvents(null, "Lines:" + i);
                        return R.Error("グローバルラベルがないのに、 .word命令が使われました。\r\nまずはグローバルラベルを定義してください。 \r\n\r\nFile:{0} Line:{1}", filename, i + 1);
                    }
                    U.append_u32(current.list, 0);
                    break;
                }
            }
            InputFormRef.DoEvents(null, "Term..");

            //書き込む前の事前チェック
            //ソングテーブルがあるかどうか確認.
            if (findGlobal(global, globalName) < 0)
            {
                return R.Error("ソングテーブルがありません。\r\n\r\nglobal sss\r\nsss:\r\nこのようなデータが必要です。\r\n");
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData("song", Path.GetFileName(filename));

            InputFormRef.DoEvents(null, "Recycle");
            //古い曲をリサイクル情報に登録..
            RecycleAddress ra = SongTableForm.MakeRecycleSong(songtable_address);

            InputFormRef.DoEvents(null, "Write");
            //とりあえずデータを書き込む
            for (int i = 0; i < global.Count; i++)
            {
                byte[] data = global[i].list.ToArray();
                uint write_addr = ra.Write(data, undodata);

                global[i].ROMAllocAddr = write_addr;

                if (global[i].name == globalName)
                {//グローバルラベルなので、ソングテーブルです.
                    Program.ROM.write_p32(songtable_address + 0, global[i].ROMAllocAddr, undodata);
                }
            }
            //相対アドレスで書いている部分を描き戻す.
            for (int i = 0; i < global.Count; i++)
            {
                for (int n = 0; n < global[i].useLabelRegist.Count; n++)
                {
                    uint rewrite_offset = (uint)global[i].useLabelRegist[n];
                    uint rewrite_addr = global[i].ROMAllocAddr + rewrite_offset;

                    uint rewrite_info = Program.ROM.u32(rewrite_addr);
                    if (rewrite_info == U.NOT_FOUND)
                    {
                        rewrite_info = 0;
                    }
                    int globalID = (int)((rewrite_info >> 24) & 0xFF);
                    uint offset = (rewrite_info & 0xFFFFFF);

                    uint new_pointer = global[globalID].ROMAllocAddr + offset;

                    Program.ROM.write_p32(rewrite_addr, new_pointer, undodata);
                }
            }
            //楽器テーブルの更新
            {
                uint songheader = Program.ROM.u32(songtable_address);
                if (! U.isSafetyPointer(songheader + 4))
                {//ソングヘッダーが存在しない.
                    Program.Undo.Rollback(undodata);
                    return R.Error("ソングテーブルにソングヘッダがありません。");
                }
                Program.ROM.write_p32( U.toOffset(songheader) + 4, instrument_addr, undodata);
            }
            ra.BlackOut(undodata);

            Program.Undo.Push(undodata);
            return "";
        }

        static int findGlobal(List<SongInnerDataSt> global, string name)
        {
            for (int i = 0; i < global.Count; i++)
            {
                if (global[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        static int Expr(string expr_value, Dictionary<string, int> equ)
        {
            List<KeyValuePair<string, int>> equ_sorted = U.OrderBy<string, int>(equ, (x) => { return -(x.Key.Length); });

            string expr = expr_value;

            //変数を実際の値に置換します.
            foreach (var pair in equ_sorted)
            {
                expr = expr.Replace(pair.Key, pair.Value.ToString());
            }

            if (!isExprString(expr))
            {
                throw new SyntaxErrorException(R.Error("数式以外の文字が入っています。 expr:{0}", expr));
            }

            //see https://dobon.net/vb/dotnet/programing/eval.html
            System.Data.DataTable dt = new System.Data.DataTable();
            object result = dt.Compute(expr, "");
            string str = result.ToString();

            int ret = 0;
            if (!int.TryParse(str, out ret))
            {
                ret = (int)U.atoi(str);
            }
            return ret;
        }
        static bool isExprString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!
                    ((str[i] >= '0' && str[i] <= '9')
                   || (str[i] == '+' || str[i] == '-' || str[i] == '*' || str[i] == '/' || str[i] == '(' || str[i] == ')' || str[i] == '.')
                   || (str[i] == '\0')
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        static bool isGlobalLabel(string globalName, string token)
        {
            //グローバルラベル
            //global:
            //global_1:
            //global1:
            //
            //非グローバルラベル
            //global_1_1:
            //foo:
            if (token.IndexOf(globalName) != 0)
            {
                return false;
            }
            int i = globalName.Length;
            //skip _
            while (token[i] == '_')
            {
                i++;
            }
            //is number
            while (U.isnum(token[i]))
            {
                i++;
            }
            if (token[i] == ':')
            {//this is global label
                return true;
            }
            return false;
        }
#if DEBUG
        public static void TEST_isGlobalLabel()
        {
            {
                bool r = isGlobalLabel("aaa", "aaa:");
                Debug.Assert(r == true);
            }
            {
                bool r = isGlobalLabel("aaa", "aaa1:");
                Debug.Assert(r == true);
            }
            {
                bool r = isGlobalLabel("aaa", "aaa_1:");
                Debug.Assert(r == true);
            }
            {
                bool r = isGlobalLabel("aaa", "axx_1:");
                Debug.Assert(r == false);
            }
            {
                bool r = isGlobalLabel("aaa", "axx1:");
                Debug.Assert(r == false);
            }
            {
                bool r = isGlobalLabel("aaa", "aaa_1_1:");
                Debug.Assert(r == false);
            }
            {
                bool r = isGlobalLabel("aaa", "aaa1_1:");
                Debug.Assert(r == false);
            }
        }
#endif

        public static bool IsDirectSoundData(byte[] rom,uint addr)
        {
            if (addr + 12 + 4 > rom.Length)
            {
                return false;
            }
//            uint header = U.u16(rom , addr + 0x0);
//            if (header != 0x0)
//            {
//                return false;
//            }
//            uint header2 = U.u8(rom, addr + 0x2);
//            if (header2 != 0x0)
//            {
//                return false;
//            }

            uint len = U.u32(rom, (uint)(addr + 12));
            if (len >= 1024*1024*4)
            {//4MB使う音源とかマジですか?
                return false;
            }
            if (len <= 4)
            {//短すぎる
                return false;
            }

            if (addr + 12 + 4 + len > rom.Length)
            {
                return false;
            }

            //どうやら正しいデータのようだ.
            return true;
        }

        public static byte[] byteToWav(byte[] data, uint pos)
        {
            //周波数??
            uint samples_per_sec1024 = U.u32(data, (uint)(pos + 4)); //周波数*1024
            samples_per_sec1024 = samples_per_sec1024 / 1024;

            //長さ
            uint len = U.u32(data, (uint)(pos + 12));

            List<byte> fp = new List<byte>();
            U.append_range(fp, "RIFF"); //riff_chunk_ID
            U.append_u32(fp, (uint)(36 + len + 1));  //riff_chunk_size
            U.append_range(fp, "WAVE"); //riff_form_type
            U.append_range(fp, "fmt "); //fmt_chunk_ID
            U.append_u32(fp, 16);   //fmt_chunk_size  
            U.append_u16(fp, 1);   //fmt_wave_format_type  
            U.append_u16(fp, 1);   //fmt_channel  
            U.append_u32(fp, (uint)samples_per_sec1024);   //fmt_samples_per_sec   標本化周波数
            U.append_u32(fp, (uint)(samples_per_sec1024 * 8 / 8));   //fmt_bytes_per_sec
            U.append_u16(fp, (uint)(1));   //fmt_block_size
            U.append_u16(fp, (uint)(8));   //fmt_bits_per_sample 量子化精度

            U.append_range(fp, "data"); //data_chunk_ID
            U.append_u32(fp, (uint)(len + 1));   //data_chunk_size

            fp.Add((byte)0x80); //データ長+1あるらしい. 謎の余白.
            for (int i = 0; i < len; i++)
            {
                uint d = U.u8(data, (uint)(i + pos + 12 + 4));
                int dd = ((int)d) - 0x80;

                fp.Add((byte)dd);
            }
            return fp.ToArray();
        }

        public static uint wavToDataSec(byte[] data)
        {
            if (data.Length < (44 + 1))
            {
                return U.NOT_FOUND;
            }

            uint fmt_bytes_per_sec = U.u32(data, 28);
            if (fmt_bytes_per_sec <= 0)
            {
                return U.NOT_FOUND;
            }

            uint data_chunk_size = U.u32(data, 40);
            if (data_chunk_size > data.Length - (44 + 1))
            {//チャンクのデータサイズが不正だったら修正する.
                data_chunk_size = (uint)(data.Length - (44 + 1));
            }

            uint ret = data_chunk_size / fmt_bytes_per_sec;
            if (data_chunk_size % fmt_bytes_per_sec != 0)
            {
                ret += 1;
            }

            return Math.Max(ret, 1);
        }

        public static byte[] wavToByte(byte[] data)
        {
            List<byte> wave = new List<byte>();
            if (data[0] != 'R'
            || data[1] != 'I'
            || data[2] != 'F'
            || data[3] != 'F'
                )
            {
                R.ShowStopError("Waveファイルではありません. RIFFヘッダがありません");
                return null;
            }
            if (data.Length < (44 + 1))
            {
                R.ShowStopError("Waveファイルではありません. データが小さすぎます");
                return null;
            }

            uint riff_chunk_size = U.u32(data, 4);
            uint riff_form_type = U.u32(data, 8);
            uint fmt_chunk_ID = U.u32(data, 12);
            uint fmt_chunk_size = U.u32(data, 16);
            uint fmt_wave_format_type = U.u16(data, 20);
            uint fmt_channel = U.u16(data, 32);
            uint fmt_samples_per_sec = U.u32(data, 24);
            uint fmt_bytes_per_sec = U.u32(data, 28);
            uint fmt_block_size = U.u16(data, 32);
            uint fmt_bits_per_sample = U.u16(data, 34);
            uint data_chunk_ID = U.u32(data, 36);
            uint data_chunk_size = U.u32(data, 40);
            if (data_chunk_size > data.Length - (44 + 1))
            {//チャンクのデータサイズが不正だったら修正する.
                data_chunk_size =(uint) (data.Length - (44 + 1));
            }
            if (data_chunk_size <= 1)
            {
                R.ShowStopError("Waveファイルではありません. data_chunk_size({0})が小さすぎます", data_chunk_size);
                return null;
            }

            PatchUtil.ImprovedSoundMixer withImprovedSoundMixer = PatchUtil.SearchImprovedSoundMixer();
            if (fmt_bits_per_sample > 8)
            {//サンプルビット数が8ビットを超える
                R.ShowStopError("Waveファイルが高音質すぎます。{0}bit\r\n品質は、8bit 12khz monoぐらいにしてください。", fmt_bits_per_sample);
                return null;
            }
            if (data_chunk_size >= 1024 * 100)
            {//あまりにでかい場合最終確認
                DialogResult dr = R.ShowNoYes("このWavファイルはとても大きく{0}バイトも消費しますが、本当にインポートしてもよろしいですか？", data_chunk_size);
                if (dr != DialogResult.Yes)
                {
                    return null;
                }
                //ユーザーがやると言えば従うのみ.
            }


            U.append_u32(wave, 0); //ループするかどうかのフラグ?  ループしない 0x00 00 00 00
            //                             ループする   0x00 00 00 04
            U.append_u32(wave, fmt_samples_per_sec * 1024);//周波数*1024
            U.append_u32(wave, 0); //不明
            U.append_u32(wave, data_chunk_size - 1); //データ長

            for (int n = 0; n < data_chunk_size - 1; n++)
            {
                byte d = (byte)(((sbyte)data[44 + 1 + n]) + 0x80);
                wave.Add(d);
            }
            wave.Add(0); //なぜか、 長さ-1して、空データを末尾に追加する.
            //少なくともsappyの挙動はそうなっている. bug?
            return wave.ToArray();
        }

        public static string ImportWave(string filename, uint songtable_address, bool useLoop)
        {
            uint songheader_address = Program.ROM.p32(songtable_address + 0);

            byte[] wave = File.ReadAllBytes(filename);
            byte[] gbawave = SongUtil.wavToByte(wave);
            if (gbawave == null)
            {
                return R.Error("Waveファイルのインポートを取りやめました");
            }
            uint playsec = wavToDataSec(wave);
            if (playsec == U.NOT_FOUND)
            {
                return R.Error("Waveファイルのインポートを取りやめました");
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData("wave import", Path.GetFileName(filename));

            //gbawave + 楽器データ 12バイト * 2(終端用に無効値) + 音を鳴らす楽譜

            InputFormRef.DoEvents(null, "Recycle");
            RecycleAddress ra = SongTableForm.MakeRecycleSongAndInst(songtable_address);

            InputFormRef.DoEvents(null, "Write");
            uint gbawave_addr = ra.Write(gbawave, undodata);

            //楽器データ
            byte[] voca = new byte[12 * 2];
            voca[1] = 0x3c;
            U.write_p32(voca, 4, gbawave_addr); //gba waveデータのアドレス書き込み
            voca[8] = 0xFF;
            voca[9] = 0x00;
            voca[10] = 0xFF;
            voca[11] = 0xA5;

            //保険のために無効値を入れます.
            U.write_u32(voca, 12, U.NOT_FOUND);
            U.write_u32(voca, 16, U.NOT_FOUND);
            U.write_u32(voca, 20, U.NOT_FOUND);
            uint voca_addr = ra.Write(voca, undodata);

            //楽譜データ
            List<byte> track = new List<byte>();
            U.append_u8(track,0xBE); //VOL //index:0
            U.append_u8(track, 127); //    //index:1
            U.append_u8(track, 0xBC); //KEYSH //index:2
            U.append_u8(track, 0); //         //index:3
            U.append_u8(track, 0xBD); //楽器変更 //index:4
            U.append_u8(track, 0); //楽器0番     //index:5
            U.append_u8(track, TIE); //TIE       //index:6
            U.append_u8(track, 60); //Cn3        //index:7
            U.append_u8(track, 127); //v127      //index:8

            if (useLoop == false)
            {
                U.append_u8(track, 48 + 0x80); //W96 //index:9
            }
            else
            {
                //全休符
                uint zenkyufu = playsec/2;
                for (uint i = 0; i < zenkyufu; i++)
                {
                    U.append_u8(track, 48 + 0x80); //W96
                }
                if (playsec % 2 == 1)
                {
                    U.append_u8(track, 48 + 0x80); //W96
                }
                //微妙にずれるらしいので補正
                uint yohaku = (uint)(playsec * 0.11f);
                for (uint i = 0; i < yohaku; i++)
                {
                    U.append_u8(track, 48 + 0x80); //W96
                }
                U.append_u8(track, EOT); //EOT
            }
            U.append_u8(track, 0xB2); //goto //index:10
            U.append_u32(track, 0); //addr   //index:11,12,13,14
            U.append_u8(track, 0xB1); //FINE   //index:15

            uint track_addr = ra.Write(track.ToArray(), undodata);

            //ループアドレスに正しいものを入れる.
            if (useLoop == false)
            {
                Debug.Assert(track.Count - 4 - 1 == 11);
                //無音無限
                Program.ROM.write_p32((uint)(track_addr + track.Count - 4 - 1), track_addr + 9);
            }
            else
            {
                //音をもう一回
                Program.ROM.write_p32((uint)(track_addr + track.Count - 4 - 1), track_addr + 6);
            }

            //ソングヘッダ
            byte[] songheader = new byte[4 + 4 + 4];
            songheader[0] = 1; //1トラック
            U.write_p32(songheader, 4, voca_addr); //楽器データへのポインタ
            U.write_p32(songheader, 8, track_addr); //楽譜データへのポインタ
            uint songheader_addr = ra.Write(songheader, undodata);

            Program.ROM.write_p32(songtable_address + 0, songheader_addr);
            ra.BlackOut(undodata);
            return "";
        }

        public class ChangeVoiceSt
        {
            public int from;
            public int to;
        }
        public static void GetVoices(Track track, ref List<ChangeVoiceSt> voices)
        {
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                if (c.type != 0xBD)
                {
                    continue;
                }
                int voice = (int)c.value;
                if (voice < 0 || voice >= 128)
                {
                    continue;
                }

                bool found = false;
                for (int n = 0; n < voices.Count; n++)
                {
                    if (voices[n].from == voice)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    continue;
                }
                ChangeVoiceSt v = new ChangeVoiceSt();
                v.from = (int)voice;
                v.to = (int)voice;
                voices.Add(v);
            }
        }
        public static List<ChangeVoiceSt> GetVoices(Track track)
        {
            List<ChangeVoiceSt> voices = new List<ChangeVoiceSt>();
            GetVoices(track, ref voices);
            return voices;
        }
        public static void ChangeTrackAndWrite(Track track
            , List<ChangeVoiceSt> changeVoices
            , int changeVol
            , int changePan
            , int changeTempo
            , bool changeVelocity
            )
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData("ChangeTrack");
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                if (c.type == 0xBD)
                {//VOICE(楽器)
                    for (int n = 0; n < changeVoices.Count; n++)
                    {
                        if (c.value == changeVoices[n].from)
                        {
                            c.value = (uint)changeVoices[n].to;

                            Program.ROM.write_u8(c.addr + 1, c.value, undodata);
                        }
                    }
                    continue;
                }
                if (changeVol != 0)
                {
                    if (c.type == 0xbe)
                    {//VOL
                        int a = (int)c.value;
                        a += changeVol;
                        if (a > 127)
                        {
                            a = 127;
                        }
                        else if (a < 0)
                        {
                            a = 0;
                        }
                        c.value = (uint)a;

                        Program.ROM.write_u8(c.addr + 1, c.value, undodata);

                        continue;
                    }
                    else if (changeVelocity)
                    {
                        if (c.type >= TIE && c.type <= (uint)NOTE_END)
                        {// N24, Dn3 ,v100
                            // N24, Dn3
                            if (c.value != U.NOT_FOUND)
                            {
                                if (c.value2 != U.NOT_FOUND)
                                {//ベロシティも変更する.
                                    int a = (int)c.value2;
                                    a += changeVol;
                                    if (a > 127)
                                    {
                                        a = 127;
                                    }
                                    else if (a < 0)
                                    {
                                        a = 0;
                                    }
                                    c.value2 = (uint)a;

                                    Program.ROM.write_u8(c.addr + 2, c.value2, undodata);
                                }
                            }
                        }
                        else if (c.type <= 127)
                        {// Dn3 ,v100
                            // Dn3
                            if (c.value != U.NOT_FOUND)
                            {//ベロシティも変更する.
                                int a = (int)c.value;
                                a += changeVol;
                                if (a > 127)
                                {
                                    a = 127;
                                }
                                else if (a < 0)
                                {
                                    a = 0;
                                }
                                c.value = (uint)a;

                                undodata.list.Add(new Undo.UndoPostion(c.addr + 1, 1));
                                Program.ROM.write_u8(c.addr + 1, c.value);
                            }
                        }
                    }
                }
                if (changePan != 0 && c.type == 0xbf)
                {//PAN
                    int a = (int)c.value;
                    a += changePan;
                    if (a > 127)
                    {
                        a = 127;
                    }
                    else if (a < 0)
                    {
                        a = 0;
                    }
                    c.value = (uint)a;

                    Program.ROM.write_u8(c.addr + 1, c.value, undodata);

                    continue;
                }
                if (changeTempo != 0 && c.type == 0xbb)
                {//TEMPO
                    int a = (int)c.value;
                    a += changeTempo;
                    if (a > 255)
                    {
                        a = 255;
                    }
                    else if (a < 0)
                    {
                        a = 0;
                    }
                    c.value = (uint)a;

                    Program.ROM.write_u8(c.addr + 1, c.value, undodata);

                    continue;
                }
            }
            Program.Undo.Push(undodata);
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors, Track track, uint songinst_addr, uint songaddr, uint song_id, uint track_number, bool isMapBGM)
        {
            bool checkTIE = false;
            for (int i = 0; i < track.codes.Count; i++)
            {
                Code c = track.codes[i];
                if (c.type == 0xbd)
                {//VOICE(楽器)
                    string err = SongInstrumentForm.CheckInst(songinst_addr, c.value);
                    if (err != "")
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.SONGTRACK, U.toOffset(songaddr)
                            , R._("SongID {0}のトラック「{1}」は、壊れた楽器「{2}」を再生するように命令されています。\r\nVOICE命令を確認してください。\r\n{3}", U.To0xHexString(song_id), track_number + 1, SongInstrumentForm.GetNameFull(songinst_addr, c.value), err), song_id));
                    }
                }
                else if (c.type == TIE)
                {
                    checkTIE = true;
                }
                else if (c.type == EOT)
                {
                    checkTIE = false;
                }
                else if (c.type == 0xbd) //VOICE
                {//楽器切替でもTIEを落としてもいいかな
                    checkTIE = false;
                }
                else if (c.type == 0xb3) //PATT
                {//PATT内でEOTされているかもしれないので検出しない
                    checkTIE = false;
                }
                else if (c.type == 0xb2)
                {//GOTO
                    if (isMapBGM && checkTIE && !IsEnvSound(song_id) )
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.SONGTRACK, U.toOffset(songaddr)
                            , R._("SongID {0}のトラック「{1}」は、TIEに対するEOTを忘れてGOTOでループしています。\r\nEOTを忘れてループすると、音が鳴りっぱなしになるます。楽譜のGOTOの前にEOTを追加してください。", U.To0xHexString(song_id), track_number + 1), song_id));
                    }
                }
            }
        }

        static bool IsEnvSound(uint song_id)
        {
            string name = SongTableForm.GetSongName(song_id);
            if (name.IndexOf("環境音:") >= 0 || name.IndexOf("EnvSound:") >= 0) ///No Translate
            {
                return true;
            }
            return false;
        }

        //midifix4agbの音量補正ルーチンをもとにしています.
        static uint expVol(uint volume)
        {
            double returnValue = volume;
            if (returnValue == 0) return 0;
            returnValue /= 127;
            returnValue = Math.Pow(returnValue, 10.0 / 6.0);
            returnValue = returnValue * 127;

            if (returnValue < 0)
            {
                return 0;
            }
            if (returnValue >= 127)
            {
                return 127;
            }
            uint r = Convert.ToByte(returnValue);
            return r;
        }
        //逆変換
        static uint expVolRev(uint volume)
        {
            double returnValue = volume;
            if (returnValue == 0) return 0;
            returnValue /= 127;
            returnValue = Math.Pow(returnValue, 1.0 / (10.0 / 6.0));
            returnValue = returnValue * 127;

            if (returnValue < 0)
            {
                return 0;
            }
            if (returnValue >= 127)
            {
                return 127;
            }
            uint r = Convert.ToByte(returnValue);
            return r;
        }
#if DEBUG
        public static void TEST_expVol()
        {
            uint r = expVol(100);
            uint rr = expVolRev(r);
            Debug.Assert(rr == 100);
        }
#endif

        public static void ExportInstrument(string filename, uint instrument_addr)
        {
            SongInstrumentForm.ExportAllLow(filename, instrument_addr);
        }
        public static string ImportInstrument(string filename, uint songtable_address)
        {
            Undo.UndoData undodata =  Program.Undo.NewUndoData("INST:" + Path.GetFileName(filename));
            uint addr = SongInstrumentForm.ImportAllLow(filename, undodata);
            if (addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return R.Error("楽器データをインポートできませんでした。");
            }

            songtable_address = U.toOffset(songtable_address);
            uint songheader_address = Program.ROM.p32(songtable_address + 0);

            Program.ROM.write_p32(songheader_address + 4, addr, undodata);
            return "";
        }

        public static bool UseGBAMusRiper()
        {
            string filename = OptionForm.GetGBAMusRiper();
            return File.Exists(filename);
        }

        public static bool ExportMidiFileByGBAMusRiper(string filename, uint songtrack_addr)
        {
            string error;
            bool r = MainFormUtil.ExportMidiByGBAMusRiper(filename, songtrack_addr, out error);
            if (r == false)
            {
                R.ShowStopError("GBAMusRiperがエラーを返しました。\r\nFEBuilderGBA側で代用します。\r\n\r\nGBAMusRiperのエラーメッセージ\r\n{0}", error);

                return false;
            }
            return true;
        }
        public static void ExportSoundFontByGBAMusRiper(string filename, uint songtrack_addr)
        {
            string error;
            bool r = MainFormUtil.ExportSoundFontByGBAMusRiper(filename, songtrack_addr, out error);
            if (r == false)
            {
                R.ShowStopError("GBAMusRiperがエラーを返しました。\r\n{0}", error);
                return;
            }
        }
        public static string ConvertMidfix4agb(string filename)
        {
            string error;
            bool r = MainFormUtil.ConvertMidfix4agb(filename, out error);
            if (r == false)
            {
                R.ShowStopError("midfix4agbがエラーになりました。\r\n{0}", error);
                return "";
            }
            return error;
        }
        public static uint FindSongTablePointer(byte[] data)
        {
            byte[] search = new byte[] {
                0x00, 0xB5, 0x00, 0x04, 0x07, 0x4A, 0x08, 0x49,
                0x40, 0x0B, 0x40, 0x18, 0x83, 0x88, 0x59, 0x00,
                0xC9, 0x18, 0x89, 0x00, 0x89, 0x18, 0x0A, 0x68,
                0x01, 0x68, 0x10, 0x1C, 0x00, 0xF0
            };
            uint foundPoint = U.Grep(data, search);
            if (foundPoint == U.NOT_FOUND)
            {//見つからなかった
                return U.NOT_FOUND;
            }
            uint songpointer = foundPoint + (uint)search.Length + 10;
            songpointer = U.toOffset(songpointer);

            uint songlist = U.u32(data, songpointer);
            if (!U.isPointer(songlist))
            {
                return U.NOT_FOUND;
            }
            return songpointer;
        }

    }
}
