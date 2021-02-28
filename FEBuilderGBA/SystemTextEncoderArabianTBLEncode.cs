using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    public class SystemTextEncoderArabianTBLEncode : SystemTextEncoderTBLEncodeInterface
    {
        class DicC
        {
            public String Arabian;
            public String English;
            public String ArabianIsolated;
        }
        List<DicC> Dic = new List<DicC>();
        SystemTextEncoderTBLEncode FE6Inner;
        Encoding SJISEncoder;

        public SystemTextEncoderArabianTBLEncode(string fullfilename,SystemTextEncoderTBLEncode inner)
        {
            this.SJISEncoder = System.Text.Encoding.GetEncoding("Shift_jis");

            TBLLoad(fullfilename,inner);
        }
        public void TBLLoad(string fullfilename,SystemTextEncoderTBLEncode inner)
        {
            this.FE6Inner = inner;
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
                    int count = (sp.Length / 2) * 2;
                    for (int i = 0; i < count; i += 2)
                    {
                        DicC d = new DicC();
                        d.English = U.Reverse(sp[i + 0]);
                        d.Arabian = sp[i + 1];
                        d.ArabianIsolated = sp[1];

                        if (!CheckAlready(d))
                        {
                            Dic.Add(d);
                        }
                    }
                }
            }
            Dic.Sort((a, b) => { return (int)(b.English.Length - a.English.Length); });
        }
        //既に辞書にあるかどうか
        bool CheckAlready(DicC newD)
        {
            foreach (DicC d in this.Dic)
            {
                if (d.Arabian == newD.Arabian && d.English == newD.English)
                {
                    return true;
                }
            }
            return false;
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
            string ret;
            if (this.FE6Inner != null)
            {
                ret = this.FE6Inner.Decode(str, start, len);
            }
            else
            {
                ret = this.SJISEncoder.GetString(str, start, len);
            }

            foreach (DicC d in this.Dic)
            {
                ret = ret.Replace(d.English, d.ArabianIsolated);
            }
//            ret = U.Reverse(ret);
            return ret;
        }


        public byte[] Encode(string str)
        {
            foreach (DicC d in this.Dic)
            {
                if (d.Arabian == "")
                {
                    continue;
                }
                str = str.Replace(d.Arabian, d.English);
            }

            if (this.FE6Inner != null)
            {
                return this.FE6Inner.Encode(str);
            }

            List<byte> data = new List<byte>(str.Length * 3);
            uint len = (uint)str.Length;
            if (len <= 0)
            {
                return data.ToArray();
            }

            uint lastI = 0;
            for (uint i = 0; i < len; )
            {
                string key = str.Substring((int)i, 1);
                if (key == "@")
                {
                    byte[] sjisstr = AppendStr(str, i, lastI);
                    data.AddRange(sjisstr);

                    sjisstr = U.SkipAtMark(str, i , this.SJISEncoder);
                    i += (uint)sjisstr.Length;
                    data.AddRange(sjisstr);

                    lastI = i;
                    continue;
                }

                i++;
            }
            {
                byte[] sjisstr = AppendStr(str, len, lastI);
                data.AddRange(sjisstr);
            }
            return data.ToArray();
        }
        byte[] AppendStr(string str, uint i , uint lastI)
        {
            if (i <= lastI)
            {
                return new byte[]{};
            }
            string key = str.Substring((int)lastI, (int)(i - lastI));

            foreach (DicC d in this.Dic)
            {
                key = key.Replace(d.Arabian, d.English);
            }

            if (this.FE6Inner != null)
            {
                return this.FE6Inner.Encode(key);
            }
            byte[] sjisstr = this.SJISEncoder.GetBytes(key);
            return sjisstr;
        }
        public Dictionary<string, uint> GetEncodeDicLow()
        {
            return new Dictionary<string,uint>();
        }
    }
}
