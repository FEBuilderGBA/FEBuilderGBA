using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    //see https://qiita.com/satto_sann/items/be4177360a0bc3691fdf
    public class TranslateGoogle2
    {
        //複数行の翻訳
        public string TransMulti(string src, string from, string to)
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
        public string Trans(string src, string from, string to)
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
                    a.Replace(match.Groups[1].Value, "AURL");
                }

                string b = TransOne(a, from, to);
                if (match.Groups.Count >= 2)
                {//URLをもとに戻す.
                    b.Replace("AURL", match.Groups[1].Value);
                }

                list.Add(b);
            }

            return string.Join("", list.ToArray());
        }

        //1行の翻訳
        public string TransOne(string src, string from, string to)
        {
            if (src.Trim() == "")
            {
                return src;
            }
            if (src == "。" || src == ".")
            {
                return src;
            }

            byte[] srcUTF8 = Encoding.UTF8.GetBytes(src);
            string q = System.Uri.EscapeDataString(src);

            string url = string.Format(
                    "https://script.google.com/macros/s/AKfycbwdsEtvxxeqM6hs5j8-CzuJ79jbdc_X8TABn_6JICaSykAGP6pfSNzCPjx_8L5qp4Pgzw/exec?text={0}&source={1}&target={2}"
                , q
                , from
                , to
            );

            string contents = U.HttpGet(url);
            string a = cleaning_text(contents);
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

    }
}
