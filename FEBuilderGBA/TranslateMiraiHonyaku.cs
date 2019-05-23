using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    public class TranslateMiraiHonyaku
    {
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

        System.Net.CookieContainer Cookie = new System.Net.CookieContainer();

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

            string url = "https://miraitranslate.com/trial/";
            string preContent = U.HttpGet(url, "", this.Cookie);
            Match m = RegexCache.Match(preContent, "id=\"tranInput\" value=\"(.+?)\"");
            if (m.Groups.Count <= 1)
            {
                throw new Exception(R.Error("tranInputがありません"));
            }

            url = "https://miraitranslate.com/trial/translate.php";
            string referer = "https://miraitranslate.com/trial/";
            Dictionary<string, string> args = new Dictionary<string, string>();
            args["input"] = src;
            args["source"] = from;
            args["target"] = to;
            args["profile"] = "nmt";
            args["kind"] = "nmt";
            args["bt"] = "false";
            args["tran"] = m.Groups[1].Value;
            string contents = U.HttpPost(url, args, referer , this.Cookie);

            string a = U.cut(contents, "\"output\":\"", "\"}");
            if (a == "")
            {
                throw new Exception(R.Error("翻訳データがnullになりました。\r\nURL:{0}\r\nSRC:{1}",url,src));
            }
            a = cleaning_text(a);
            return a;
	    }
        string cleaning_text(string text)
        {
            text = text.Replace("\\\"", "\"");
            text = text.Replace("\\u003c", "<");
            text = text.Replace("\\u003e", ">");
            text = text.Replace("\\u0027", "'");
            text = text.Replace("\xE2\x80\x8B", ""); //なんかゴミが入るときがあるらしい.
            text = U.unhtmlspecialchars(text);
            
            return text;
        }

#if DEBUG
        public static void TEST_TranslateMiraiHonyaku()
        {
//            string q = "山を食べる";
//            TranslateMiraiHonyaku t = new TranslateMiraiHonyaku();
//            string e = t.Trans(q,"ja","en");
        }
#endif
    }
}
