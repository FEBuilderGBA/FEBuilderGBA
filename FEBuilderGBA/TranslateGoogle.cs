using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    //hack the googletranslate original source code by translate-shell(awk) :p)
    public class TranslateGoogle
    {
	    //謎の値1
        long genRL(long a, long[] x, long size)
	    {
            for (long c = 0; c < size - 2; c += 3)
            {
                long d = (long)x[c + 2];
			    d = d >= 97 ? d - 87 :
				    d - 48;
			    d = x[c + 1] == 43 ? (a >> (int)d) : (a << (int)d);
                a = (long)(x[c] == 43 ? ((a + d) & 4294967295) : (a ^ d));
		    }
		    return a;
	    }

	    //謎の値2
	    string genTK(byte[] text,long time)
	    {
            long tkk = (long)(time / 3600);
            long[] ub = new long[] { 43, 45, 51, 94, 43, 98, 43, 45, 102 };
            long[] vb = new long[] { 43, 45, 97, 94, 43, 54 };

            long ub_count = ub.Length;
            long vb_count = vb.Length;

            long dLen = text.Length;
            long a = tkk;
            for (long e = 0; e < dLen; e++)
		    {
                long c = (long)text[e];
			    a = a + c;
			    a = this.genRL(a, vb ,vb_count );
		    }
            a = (long)this.genRL(a, ub, ub_count);
		    if (0 > a)
		    {
                a = (long)((a & 2147483647) + 2147483648);
		    }
		    a %= 1000000;

		    return string.Format("{0}.{1}",a, a ^ tkk);
	    }

        static long GetUnixTime(DateTime targetTime)
        {
            DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);	


            // UTC時間に変換
            targetTime = targetTime.ToUniversalTime();

            // UNIXエポックからの経過時間を取得
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;
    
            // 経過秒数に変換
            return (long)elapsedTime.TotalSeconds;
        }

        //複数行の翻訳
        public string TransMulti(string src, string from, string to, long time = 0)
        {
            if (src.Length <= 0)
            {
                return src;
            }

            string ret = "";

            string[] lines = src.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                string t = Trans(lines[i], from, to);
                ret += "\r\n" + t;
            }
            return ret.Substring(2);
        }

        //1行の翻訳
        public string Trans(string src, string from, string to, long time = 0)
        {
            List<string> list = new List<string>();

            char term_char = '。';
            int term = src.IndexOf(term_char);
            if (term < 0)
            {
                term_char = '.';
                term = src.IndexOf(term_char);
                if (term < 0)
                {
                    term_char = ' ';
                }
            }
            
            string[] lines = src.Split(term_char);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    continue;
                }

                string a;
                if (term_char == ' ')
                {
                    a = lines[i];
                }
                else
                {
                    a = lines[i] + term_char;
                }
                
                //URLが壊されないように変換する
                System.Text.RegularExpressions.Match match = RegexCache.Match(a, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$");
                if (match.Groups.Count >= 2)
                {
                    a.Replace(match.Groups[1].Value ,"AURL");
                }

                string b = TransOne(a, from, to, time);
                if (match.Groups.Count >= 2)
                {//URLをもとに戻す.
                    b.Replace("AURL", match.Groups[1].Value);
                }

                list.Add(b);
            }

            return string.Join("", list.ToArray());
        }

        //1行の翻訳
	    public string TransOne(string src,string from,string to,long time=0)
	    {
            if (src.Trim() == "")
            {
                return src;
            }
            if (src == "。" || src == ".")
            {
                return src;
            }

		    if (time == 0)
		    {
                DateTime targetTime = DateTime.Now;
			    time = GetUnixTime(targetTime);
		    }

            byte[] srcUTF8 = Encoding.UTF8.GetBytes(src);

		    string tk = this.genTK(srcUTF8,time);
		    string q = System.Uri.EscapeDataString(src);


		    string url = string.Format(
		    		"http://translate.googleapis.com/translate_a/single?client=gtx&ie=UTF-8&oe=UTF-8&dt=bd&dt=ex&dt=ld&dt=md&dt=rw&dt=rm&dt=ss&dt=t&dt=at&dt=qc&sl={0}&tl={1}&hl={2}&q={4}"
			    ,from
			    ,to
			    ,from
			    ,tk
			    ,q
		    );

            string contents = U.HttpGet(url);
            string a = U.cut(contents, "[\"", "\",");
            a = cleaning_text(a);
            return a;
	    }
        string cleaning_text(string text)
        {
            text = text.Replace("\\\"", "\"");
            text = text.Replace("\\u003c", "<");
            text = text.Replace("\\u003e", ">");
            text = text.Replace("\xE2\x80\x8B", ""); //なんかゴミが入るときがあるらしい.
            text = U.unhtmlspecialchars(text);
            
            return text;
        }

#if DEBUG
        public static void TEST_genTK()
        {
            byte[] srcUTF8 = Encoding.UTF8.GetBytes("hello");
        	TranslateGoogle g = new TranslateGoogle();
	        string r = g.genTK(srcUTF8,1493107850);
            Debug.Assert(r == "415694.1006");

//            string a = g.TransUTF8("hello", "en", "ja");
        }   
#endif
    }
}
