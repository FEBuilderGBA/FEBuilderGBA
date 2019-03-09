using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    class UPSUtil
    {
        public static uint GetUPSSrcCRC32(string upsfilename)
        {
            byte[] patch = File.ReadAllBytes(upsfilename);
            if (patch.Length < 16)
            {
                return U.NOT_FOUND;
            }
            uint src_crc32 = U.u32(patch, (uint)(patch.Length - 12));
            return src_crc32;
        }

        public static bool ApplyUPS(ROM retrom, string upsfilename)
        {
            byte[] patch = File.ReadAllBytes(upsfilename);
            if (patch.Length < 16)
            {
                R.ShowStopError("UPSファイルが壊れています。最低サイズ以下です。");
                return false;
            }

            if (patch[0] != 'U'
              || patch[1] != 'P'
              || patch[2] != 'S'
              || patch[3] != '1')
            {
                R.ShowStopError("UPSファイルが壊れています。ヘッダUPS1がありません。");
                return false;
            }

            U.CRC32 crc32 = new U.CRC32();
            {
                uint patch_calc_crc32 = crc32.Calc(U.subrange(patch, 0, (uint)(patch.Length - 4)));
                uint patch_crc32 = U.u32(patch, (uint)(patch.Length - 4));
                if (patch_calc_crc32 != patch_crc32)
                {
                    R.ShowStopError("UPSファイルが壊れています。CRCが一致しません。");
                    return false;
                }
            }
            {
                uint src_calc_crc32 = crc32.Calc(retrom.Data);
                uint src_crc32 = U.u32(patch, (uint)(patch.Length - 12));
                if (src_calc_crc32 != src_crc32)
                {
                    R.ShowStopError("現在開いているROMには適応できません。CRCが一致しません。");
                    return false;
                }
            }

            uint i = 4; //skip UPS1 header
            uint source_size = read_val_code(patch, i, out i);
            uint dest_size = read_val_code(patch, i, out i);

            //無改造ROMのデータ
            byte[] bin = new byte[dest_size];
            Array.Copy(retrom.Data, 0, bin, 0, Math.Min(retrom.Data.Length,dest_size) );

            uint romi = 0;
            uint end = (uint)(patch.Length - 4 * 3);
            for (; i < end; i++)
            {
                uint skip_size = read_val_code(patch, i, out i);
                romi += skip_size;
                for (; i < end; i++)
                {
                    if (romi >= dest_size)
                    {
                        break;
                    }

                    bin[romi] = (byte)(bin[romi] ^ patch[i]);
                    romi++;
                    if (patch[i] == 0x00)
                    {
                        break;
                    }
                }
            }

            uint dest_calc_crc32 = crc32.Calc(bin);
            uint dest_crc32 = U.u32(patch, (uint)(patch.Length - 8));
            if (dest_calc_crc32 != dest_crc32)
            {
                R.ShowStopError("UPSを適応した結果が正しくありません。CRCが不一致です。");
                return false;
            }

            retrom.SwapNewROMDataDirect(bin);
            retrom.ClearModifiedFlag();
            return true;
        }


        //https://github.com/rameshvarun/ups/blob/master/writer/writer.go
        // based off of the source for http://www.romhacking.net/utilities/606/
        static void append_val_code(List<byte> data, uint v)
        {
            byte x = (byte)(v & 0x7f);
            v = v >> 7;

            while (v != 0x0)
            {
                data.Add((byte)(x));
                v--;
                x = (byte)(v & 0x7f);
                v = v >> 7;
            }
            data.Add((byte)(x | 0x80));
        }
        static uint read_val_code(byte[] data, uint i, out uint out_nextI)
        {
            uint shift = 1;
            uint value = 0;
            byte b;
            while (true)
            {
                b = data[i];
                i++;

                value += ((uint)(b & 0x7f)) * shift;
                if ((b & 0x80) == 0x80)
                {
                    break;
                }
                shift <<= 7;
                value += shift;
            }
            out_nextI = i;
            return value;
        }


        static public void MakeUPS(string orignalROMFilename, string upsFilename)
        {
            byte[] s = File.ReadAllBytes(orignalROMFilename);
            byte[] d = Program.ROM.Data;
            MakeUPS(s, d, upsFilename);
        }

        static public void MakeUPS(byte[] s, byte[] d, string upsFilename)
        {
            List<byte> ups = new List<byte>();
            ups.Add((byte)'U');
            ups.Add((byte)'P');
            ups.Add((byte)'S');
            ups.Add((byte)'1');
            append_val_code(ups, (uint)s.Length);
            append_val_code(ups, (uint)d.Length);

            uint length = (uint)Math.Max(s.Length, d.Length);
            uint last_point = 0;
            for (uint i = 0; i < length; i++)
            {
                int ss = i < s.Length ? s[i] : 0;
                int dd = i < d.Length ? d[i] : 0;

                if (ss == dd)
                {
                    continue;
                }

                append_val_code(ups, (uint)(i - last_point));

                List<byte> diff = new List<byte>();
                diff.Add((byte)(ss ^ dd));

                uint n;
                for (n = i + 1; n < length; n++)
                {
                    ss = n < s.Length ? s[n] : 0;
                    dd = n < d.Length ? d[n] : 0;
                    if (ss == dd)
                    {
                        break;
                    }
                    diff.Add((byte)(ss ^ dd));
                }
                //i-n
                ups.AddRange(diff.ToArray());
                ups.Add(0);

                i = n;
                last_point = n + 1;
            }
            U.CRC32 crc32 = new U.CRC32();
            U.append_u32(ups, crc32.Calc(s));
            U.append_u32(ups, crc32.Calc(d));
            U.append_u32(ups, crc32.Calc(ups.ToArray()));

            File.WriteAllBytes(upsFilename, ups.ToArray());
        }

    }
}
