using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    class TranslateDic
    {
        List<KeyValuePair<string, string>> Dic;
        TinySegmenter TinySegmenter; //日本語分かち書き

        public TranslateDic(string filename,string from,string to)
        {
            this.Dic = new List<KeyValuePair<string, string>>();
            this.TinySegmenter = null;

            bool isRev;
            if (from == "ja" && to == "en")
            {
                isRev = false;
                this.TinySegmenter = new TinySegmenter();
            }
            else if (from == "en" && to == "ja")
            {
                isRev = true;
            }
            else
            {
                return;
            }

            string[] lines = File.ReadAllLines(filename);
            foreach(string line in lines)
            {
                string[] sp = line.Split('|');
                if (sp.Length < 4)
                {
                    continue;
                }

                if (isRev)
                {
                    Dic.Add(new KeyValuePair<string, string>(sp[2], sp[1]));
                }
                else
                {
                    Dic.Add(new KeyValuePair<string, string>(sp[1], sp[2]));
                }
            }
        }
        string TranslateOne(string str)
        {
            foreach (KeyValuePair<string, string> pair in Dic)
            {
                if (str == pair.Key)
                {
                    return pair.Value;
                }
            }
            return str;
        }

        public string Translate(string str)
        {
            if (Program.ROM.RomInfo.is_multibyte() == false)
            {//英語版ROMには、終端にパディングが入る時があるので読み飛ばす
                str = str.Replace("@001F", "");
            }


            foreach (KeyValuePair<string, string> pair in Dic)
            {
                if (str == pair.Key)
                {
                    return pair.Value;
                }
            }

            //存在しない場合、部分だけ翻訳できるか試してみる
            if (this.TinySegmenter != null)
            {
                string[] sp = this.TinySegmenter.Segment(str);
                for (int i = 0; i < sp.Length; i++ )
                {
                    if (str.Length <= 1)
                    {//日本語は漢字があるので、最低1文字
                        continue;
                    }
                    sp[i] = TranslateOne(sp[i]);
                }
                return string.Join("", sp);
            }
            else
            {
                string[] sp = str.Split(' ');
                for (int i = 0; i < sp.Length; i++)
                {
                    if (str.Length <= 2)
                    {//英語は単語が長いので、最低2文字
                        continue;
                    }
                    sp[i] = TranslateOne(sp[i]);
                }
                return string.Join(" ", sp);
            }
        }
    }
}
