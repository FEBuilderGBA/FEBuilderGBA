using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    //C#のリソースはなんだかいけていないので、
    //自前リソースを作る.
    //
    //:文字列
    //翻訳
    //
    //:文字列
    //翻訳
    //
    //
    public class MyTranslateResourceLow
    {
        Dictionary<string,string> Dic = new Dictionary<string,string>();

        //翻訳文字列の取得
        public string str(string src)
        {
            string dest;
            if (Dic.TryGetValue(src, out dest))
            {
                return dest;
            }
            return src;
        }
        //翻訳があるかどうか取得 開発用
        public bool Exist(string src)
        {
            return Dic.ContainsKey(src);
        }
        //翻訳の変更.開発用
        public void replaceTranslateString(string f,string t)
        {
            Dic[f] = t;
        }
        public void LoadResource(string fullfilename)
        {
            Dic = new Dictionary<string,string>();

            if (!File.Exists(fullfilename))
            {//リソースがない.
                return;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string src = null;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length <= 0)
                    {
                        src = null;
                        continue;
                    }
                    if (src == null)
                    {
                        if (line[0] != ':' )
                        {
                            continue;
                        }
                        src = line.Substring(1);
                        src = src.Replace("\\r\\n", "\r\n");
                    }
                    else
                    {
                        line = line.Replace("\\r\\n", "\r\n");
                        Dic[src] = line;
                    }
                }
            }
        }

        public void WriteResource(string fullfilename)
        {
            List<string> lines = new List<string>();
            foreach (var pair in Dic)
            {
                string f = pair.Key;
                string f2 = f.Replace("\r\n","\\r\\n");
                string t = pair.Value;
                string t2 = t.Replace("\r\n", "\\r\\n");

                string line = ":" + f2;
                lines.Add(line);

                line = t2;
                lines.Add(line);

                //空改行
                lines.Add("");
            }

            File.WriteAllLines(fullfilename, lines.ToArray());
        }


#if DEBUG
        static void TESTSUB_OpenDialogResource(MyTranslateResourceLow trans)
        {
            foreach (var pair in trans.Dic)
            {
                if (pair.Key.IndexOf("All files") < 0)
                {
                    continue;
                }
                System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
                open.Filter = pair.Value;
                open.Dispose();
            }
        }
        //重たいのでフルテストの時のみ
        public static void TESTFULL_RESOURCE()
        {
            string[] langs = new string[] { "en", "zh", "de" };
            for (int i = 0; i < langs.Length; i++)
            {
                MyTranslateResourceLow trans = new MyTranslateResourceLow();
                trans.LoadResource(System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", langs[i] + ".txt"));

                TESTSUB_OpenDialogResource(trans);
            }
        }

#endif

        public Dictionary<string, string> ConvertOnelineSplitWord()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>(Dic);
            foreach (var pair in Dic)
            {
                string[] lines = pair.Key.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] enlines = pair.Value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if (lines.Length != pair.Key.Length || pair.Value.Length <= 1)
                {
                    continue;
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    dic[lines[i]] = enlines[i];
                }
            }
            return dic;
        }
    }
}
