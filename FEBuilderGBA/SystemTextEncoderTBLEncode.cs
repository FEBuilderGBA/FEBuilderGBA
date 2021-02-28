using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public class SystemTextEncoderTBLEncode : SystemTextEncoderTBLEncodeInterface
    {
        Dictionary<string, uint> EncodeDic = new Dictionary<string, uint>();
        Dictionary<uint, string> DecodeDic = new Dictionary<uint, string>();
        Encoding SJISEncoder;

        public SystemTextEncoderTBLEncode(string fullfilename)
        {
            this.SJISEncoder = System.Text.Encoding.GetEncoding("Shift_jis");

            TBLLoad(fullfilename);
        }
        public void TBLLoad(string fullfilename)
        {
            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line))
                    {
                        continue;
                    }
                    string[] sp = line.Split('\t');

                    int noescape = sp[0].IndexOf("-");
                    if (noescape > 0)
                    {//81f4-81ff	平假名 こういうの
                        uint start = U.atoh(sp[0].Substring(0, noescape));
                        uint end = U.atoh(sp[0].Substring(noescape + 1));
                        byte[] sjis = new byte[2];
                        for (uint i = start; i <= end; i++)
                        {
                            uint key = i;
                            key = U.ChangeEndian16(key); //TBLは big-endianなので little endianに

                            sjis[0] = (byte)((i & 0xFF00) >> 8);
                            sjis[1] = (byte)((i & 0xFF));
                            string value = this.SJISEncoder.GetString(sjis,0,2);

                            this.EncodeDic[value] = key;
                            this.DecodeDic[key] = value;
                        }
                    }
                    else
                    {//839f	啊      通常データ
                        uint key = U.atoh(sp[0]);
                        key = U.ChangeEndian16(key); //TBLは big-endianなので little endianに
                        string value = sp[1];

                        this.EncodeDic[value] = key;
                        this.DecodeDic[key] = value;
                    }
                }
            }
        }
        public string Decode(byte[] str)
        {
            int len = str.Length;
            return Decode(str, 0,len);
        }

        public string Decode(byte[] str,int start,int len)
        {
            if (len <= 0 || str.Length <= 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            uint end = (uint)(start + len);
            if (end > str.Length)
            {
                end = (uint)str.Length;
            }

            for (uint i = (uint)start; i < end ; )
            {
                uint key;
                if (i + 1 >= str.Length)
                {//もうサイズがないので2バイト読めない.
                    key = U.u8(str, i);
                }
                else
                {
                    key = U.u16(str, i);
                }
                string value;
                if (this.DecodeDic.TryGetValue(key, out value))
                {
                    sb.Append(value);
                    i += 2;
                }
                else if (U.isSJIS1stCode((byte)((key) & 0xff)) && U.isSJIS2ndCode((byte)((key>>8) & 0xff)))
                {
                    value = this.SJISEncoder.GetString(str, (int)i, 2);
                    sb.Append(value);
                    i += 2;
                }
                else
                {
                    value = this.SJISEncoder.GetString(str, (int)i, 1);
                    sb.Append(value);
                    i += 1;
                }
            }
            return sb.ToString();
        }

        public byte[] Encode(string str)
        {
            List<byte> data = new List<byte>(str.Length*3);

            uint len = (uint)str.Length;
            if (len <= 0)
            {
                return data.ToArray();
            }

            for (uint i = 0; i < len; )
            {
                string key = str.Substring((int)i,1);
                if (key == "@")
                {
                    byte[] sjisstr = U.SkipAtMark(str, i , this.SJISEncoder);
                    i += (uint)sjisstr.Length;
                    data.AddRange(sjisstr);
                    continue;
                }

                uint value;
                if (this.EncodeDic.TryGetValue(key, out value))
                {
                    U.append_u16(data, value);
                }
                else
                {
                    byte[] sjisstr = this.SJISEncoder.GetBytes(key);
                    data.AddRange(sjisstr);
                }
                i++;
            }
            return data.ToArray();
        }
        public Dictionary<string, uint> GetEncodeDicLow()
        {
            return EncodeDic;
        }
    }
}
