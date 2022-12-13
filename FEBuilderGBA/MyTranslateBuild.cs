using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    class MyTranslateBuild
    {
        public MyTranslateBuild(string lang,bool isEnglishBase = false)
        {
            this.Lang = lang;
            this.GoogleTranslateLang = lang_to_googlelang(lang);

            string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", lang + ".txt");
            this.TranslateResource = new MyTranslateResourceLow();
            this.TranslateResource.LoadResource(resoucefilename);

            this.TranslateResourceEN = null;
            if (isEnglishBase && lang != "en")
            {
                //英語から翻訳する 日本語->多言語 より、 英語->多言語の方が効率が良い場合がある.
                string resoucefilenameEN = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "en.txt");
                if (File.Exists(resoucefilenameEN))
                {
                    this.TranslateResourceEN = new MyTranslateResourceLow();
                    this.TranslateResourceEN.LoadResource(resoucefilenameEN);
                }
            }

            //一行ずつ翻訳したものをキャッシュにいれる.
            this.TranslateCache = this.TranslateResource.ConvertOnelineSplitWord();
            //固定文の翻訳辞書
            this.TransDic = new Dictionary<string, string>();
            TranslateTextUtil.AppendFixedDic(this.TransDic, "ja", lang);
        }

        MyTranslateResourceLow TranslateResource;
        MyTranslateResourceLow TranslateResourceEN;
        string Lang;
        string GoogleTranslateLang;


        Dictionary<string, string> TranslateTargetFiles = new Dictionary<string, string>();
        public void ScanCS(string source_path)
        {
            TranslateTargetFiles.Clear();

            //ソースコードスキャン
            {
                string[] files = U.Directory_GetFiles_Safe(source_path, "*.cs", SearchOption.TopDirectoryOnly);
                foreach (string fullfilename in files)
                {
                    InputFormRef.DoEvents(null, fullfilename);
                    ScanStringForSourceCode(fullfilename);
                }
            }
            //翻訳して書き込み
            TransFileWriter();
        }

        //C#フォームデザイナーの余計なお世話を撃退する.
        //長い文章を書くと、ご丁寧なことに、途中に改行を入れてくれるので、
        //そういうのを全部消して、元通りの一行に戻す.
        //C#のフォームデザイナーはどこまで手間かけさせるんだ! どこのアホが作った!!
        string tryStrcatCode(string[] lines, int index)
        {
            string line = lines[index];

            for (int next = index + 1; next < lines.Length; next++)
            {
                //行末が " +\r\nかどうかを見る 
                string l = U.substr(line, -3);
                if (l != "\" +")
                {
                    break;
                }
                string nn = lines[next].Trim();
                line = line.Substring(0, line.Length - 3) + nn.Substring(1);
                lines[next] = "";
                lines[index] = line;
            }
            return line;
        }

        //C#の新しい、いやがらせを削除する
        //英語で長い文字を書くと、リソースファイルにするというとんでもない機能.
        //大きなお世話だ。 一部だけリソースにするアホがどこの世界にいる.
        //MSは大切な仕事はしないのに、余計な仕事ばかりする。 FUCK Microsoft! 地獄に落ちろ!!
        string tryStrcatCode2(string filename, string[] lines, int index)
        {
            string line = lines[index];
            string resources_GetString = " = resources.GetString(\"";
            int resourceFound = line.IndexOf(resources_GetString);
            if (resourceFound < 0)
            {
                return line;
            }
            int resourceFoundEnd = resourceFound + resources_GetString.Length;
            int n2 = line.IndexOf('\"', resourceFoundEnd);
            if (n2 < 0)
            {
                return line;
            }
            string resource = line.Substring(resourceFoundEnd, n2 - resourceFoundEnd);


            string resx = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filename)) + ".resx";
            resx = Path.Combine(Path.GetDirectoryName(filename), resx);
            if (!File.Exists(resx))
            {//リソースがないものは仕方ない.
                return line;
            }
            string[] resxLines = File.ReadAllLines(resx);

            string resstring = "<data name=\"" + resource + "\"";

            string str = "";
            for (int i = 0; i < resxLines.Length; i++)
            {
                if (resxLines[i].IndexOf(resstring) < 0)
                {
                    continue;
                }
                for (int k = i + 1; k < resxLines.Length; k++)
                {
                    string ll = U.unhtmlspecialchars( resxLines[k] );
                    ll = ll.Replace("\"", "\\\"");

                    str += ll + "\\r\\n";
                    int v = ll.IndexOf("</value>");
                    if (v >= 0)
                    {
                        break;
                    }
                }
                break;
            }
            if (str == "")
            {//対応するリソースがないらしい.
                return line;
            }

            //前後にある<value>と</value>を消す.
            string value = "<value>";
            int j = str.IndexOf(value);
            if (j >= 0)
            {
                str = str.Substring(j+value.Length);
            }

            value = "</value>";
            j = str.IndexOf(value);
            if (j >= 0)
            {
                str = str.Substring(0, j);
            }
            str = U.unhtmlspecialchars(str);


            //やっとリソースを取り戻せたので、 リソース部分とされた文字列を置換する.
            string newline = line.Substring(0, resourceFound);
            newline = newline + " = \"" + str + "\";";

            return newline;
        }

        void ScanStringForSourceCode(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.IndexOf("///No Translate") >= 0)
                {
                    continue;
                }
                line = tryStrcatCode(lines, i);
                line = tryStrcatCode2(filename, lines, i);

                int end = 0;
                while (true)
                {
                    int start = line.IndexOf("\"", end);
                    if (start < 0)
                    {
                        break;
                    }
                    start++;
                    int es = start;
                    end = -1;
                    while (true)
                    {
                        end = line.IndexOf("\"", es);
                        if (end <= 0)
                        {
                            break;
                        }
                        if (line[end - 1] == '\\')
                        {
                            es = end + 1;
                            continue;
                        }
                        break;
                    }
                    if (end < 0)
                    {
                        break;
                    }
                    string f = line.Substring(start, end - start);
                    if (U.isAsciiString(f))
                    {//日本語の特権アルファベットだけの文字列なら無視.
                        continue;
                    }

                    TranslateTargetFiles[f] = filename;

                    end++;
                }
            }
        }

        Dictionary<string, string> TranslateCache ; //キャッシュ(同じ文書を何度も問い合わせないようにする)
        Dictionary<string, string> TransDic; //固定文翻訳辞書
        string Translate(string text, string from)
        {
            if (this.TranslateCache.ContainsKey(text))
            {
                return this.TranslateCache[text];
            }
            string tt = text.Replace("\\\\\r\\\\\n", "\r\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            tt = text.Replace("\\\\r\\\\n", "\r\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            tt = text.Replace("\\\r\\\n", "\r\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            tt = text.Replace("\\r\\n", "\\r\\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            tt = text.Replace("\\\\\r\\\\\n", "\\r\\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            tt = text.Replace("\\\\r\\\\n", "\\r\\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            tt = text.Replace("\\\r\\\n", "\\r\\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            tt = text.Replace("\\r\\n", "\\r\\n");
            if (this.TranslateCache.ContainsKey(tt))
            {
                return this.TranslateCache[tt];
            }

            string t = text.Replace("\\r\\n", "\r\n");
            if (this.TranslateCache.ContainsKey(t))
            {
                return this.TranslateCache[t];
            }

            int eq = t.IndexOf("=");
            if (eq >= 1 && eq <= 10)
            {//01=xxx みたいなものがあったとき 
                t = t.Substring(eq + 1);
                if (this.TranslateCache.ContainsKey(t))
                {
                    return text.Substring(0, eq + 1) + this.TranslateCache[t];
                }
            }
            else
            {
                eq = 0;
            }

            //一行ずつ翻訳する
            t = OneLineTranslate(t, from);

            if (eq >= 1)
            {
                t = text.Substring(0, eq + 1) + t;
            }

            t = t.Replace("\r\n", "\\r\\n");

            this.TranslateCache[text] = t;
            return t;
        }

        
        string OneLineTranslate(string src,string from)
        {
            if (src.Length <= 0)
            {
                return src;
            }

            string ret = "";

            TranslateManage engine = new TranslateManage();
            string[] lines = src.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                string t;
                if (this.TranslateCache.ContainsKey(lines[i]))
                {
                    t = this.TranslateCache[lines[i]];
                }
                else
                {
                    t = engine.Trans(lines[i], from, this.GoogleTranslateLang);
                    Log.Debug("GoolgeTranslate:" + lines[i] + " => " + t);
                    this.TranslateCache[lines[i]] = t;
                }
                ret += "\r\n" + t;
            }
            return ret.Substring(2);
        }



        public void TransFileWriter()
        {
            bool goolge_translate_failed = false;
            
            List<string> lines = new List<string>();

            //ファイルに書き出す.
            foreach (var pair in TranslateTargetFiles)
            {
                string line = ":" + pair.Key;
                lines.Add(line);

                if (goolge_translate_failed == false)
                {
                    try
                    {
                        line = MakeTranslate(pair.Key);
                    }
                    catch (Exception e)
                    {
                        R.ShowStopError(e.ToString());
                        throw new Exception("Failed to translate google!");
                    }
                }

                line = line.Replace("\r\n", "\\r\\n");
                lines.Add(line);

                line = "";
                lines.Add(line);
            }
            string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", this.Lang + ".txt");
            if (!U.CanWriteFileRetry(resoucefilename))
            {
                return;
            }
            File.WriteAllLines(resoucefilename, lines.ToArray());

        }

        bool IsEnglishBase()
        {
            return this.TranslateResourceEN != null;
        }

        string MakeTranslate(string target)
        {
            if (U.isAsciiString(target))
            {//日本語の特権アルファベットだけの文字列なら無視.
                return target;
            }
            if (U.IsComment(target))
            {//コメントなので不要
                return target;
            }

            string test = target;
            string t = this.TranslateResource.str(test);
            if (t != test)
            {//翻訳がある
                return t;
            }
            if (this.Lang == "zh")
            {//中国語への翻訳の場合、漢字だけの文章は同一の文章になることがある.
                if (this.TranslateResource.Exist(test))
                {//翻訳があるらしい
                    return t;
                }
            }

            test = target.Replace("\\r\\n","\r\n");
            t = this.TranslateResource.str(test);
            if (t != test)
            {//翻訳がある
                return t;
            }

            test = target.Replace("\\\r\\\n", "\r\n");
            t = this.TranslateResource.str(test);
            if (t != test)
            {//翻訳がある
                return t;
            }
            test = target.Replace("\\\\r\\\\n", "\r\n");
            t = this.TranslateResource.str(test);
            if (t != test)
            {//翻訳がある
                return t;
            }


            if (t == "♪" ///No Translate
             || t == "↑" ///No Translate
             || t == "↓" ///No Translate
             || t == "←" ///No Translate
             || t == "→" ///No Translate
             || t == "～" ///No Translate
             || t == "　" ///No Translate
             || t == "。" ///No Translate
                )
            {//特殊指定
                return t;
            }

            //翻訳がない
            InputFormRef.DoEvents(null, target);
            if (this.IsEnglishBase())
            {
                string tempT = this.TranslateResourceEN.str(target);
                if (tempT == target)
                {
                    t = Translate(target, "ja");
                }
                else
                {
                    t = Translate(tempT, "en");
                }
            }
            else
            {
                t = Translate(target,"ja");
            }
            Log.Debug("->", target, t);
            return t;
        }

        string findCodeName(string[] lines, int index)
        {
            for (int i = index; i >= 0; i--)
            {
                int dqoute = lines[i].IndexOf('"');
                if (dqoute < 0)
                {// "1=foo" で、=にヒットさせたくない. "がないので、でかい値を入れる..
                    dqoute = int.MaxValue;
                }

                int eq = lines[i].IndexOf('=');
                if (eq < 0 || dqoute < eq)
                {
                    eq = lines[i].IndexOf('{');
                    if (eq < 0 || dqoute < eq)
                    {
                        continue;
                    }
                }
                int offset = index - i;
                string r = lines[i].Substring(0, eq).Trim();
                return r + "@" + offset;
            }
            //ない
            return "";
        }

        const string TRANSLATE_LINK = ".tlink.txt";

        void ScanStringForSourceCodeAndReplace(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            List<string> lineReplacesALL = new List<string>();

            if (File.Exists(filename + TRANSLATE_LINK))
            {//間違えて2回実行してしまった時のために、元データがあればマージする.
                lineReplacesALL.AddRange(File.ReadAllLines(filename + TRANSLATE_LINK));
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.IndexOf("///No Translate") >= 0)
                {
                    continue;
                }
                line = tryStrcatCode(lines, i);
                line = tryStrcatCode2(filename, lines, i);

                List<string> lineReplaces = new List<string>();

                int end = 0;
                while (true)
                {
                    int start = line.IndexOf("\"", end);
                    if (start < 0)
                    {
                        break;
                    }
                    start++;
                    int es = start;
                    end = -1;
                    while (true)
                    {
                        end = line.IndexOf("\"", es);
                        if (end <= 0)
                        {
                            break;
                        }
                        if (line[end - 1] == '\\')
                        {
                            es = end + 1;
                            continue;
                        }
                        break;
                    }
                    if (end < 0)
                    {
                        break;
                    }
                    string f = line.Substring(start, end - start);
                    if (U.isAsciiString(f))
                    {//日本語の特権アルファベットだけの文字列なら無視.
                        continue;
                    }

                    lineReplaces.Add(f);
                    end++;
                }

                foreach (string f in lineReplaces)
                {
                    string f2 = f.Replace("\\r\\n", "\r\n");

                    //翻訳データの取得.
                    string t = this.TranslateResource.str(f2);

                    //キーになる名前はあるか?
                    string codeName = findCodeName(lines, i);
                    if (codeName == "")
                    {//対応する構文がない??
                        continue;
                    }

                    string t2 = t.Replace("\r\n", "\\r\\n");

                    lineReplacesALL.Add(codeName + "\t" + f);
                    if (t != "")
                    {//翻訳がある場合、翻訳文字列を表示する.
                        line = line.Replace("\"" + f + "\"", "\"" + t2 + "\"");
                        lines[i] = line;
                    }
                }
            }
            if (!U.CanWriteFileRetry(filename))
            {
                return;
            }

            //ソースファイルに書き込み.
            File.WriteAllLines(filename, lines);
            //元の日本語名を保存 最初はコメントだったかC#デザイナーが削除してくるので、別ファイルに.
            File.WriteAllLines(filename + TRANSLATE_LINK, lineReplacesALL);

            //本当は、resxがあったら削除したい....ただprojectと結びついているので難しいところだな.
        }

        public void DesignStringConvert(string source_path)
        {
            string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", this.Lang + ".txt");
            this.TranslateResource.LoadResource(resoucefilename);

            //デザインのソースコードスキャンして日本語文字列を置き換え.
            {
                string[] files = U.Directory_GetFiles_Safe(source_path, "*.Designer.cs", SearchOption.TopDirectoryOnly);
                foreach (string fullfilename in files)
                {
                    InputFormRef.DoEvents(null, fullfilename);
                    ScanStringForSourceCodeAndReplace(fullfilename);
                }
            }
        }
        string[] findSourceCode(List<string> lineReplacesALL, string codeName)
        {
            string searchName = codeName + "\t";
            for (int i = 0; i < lineReplacesALL.Count; i++)
            {
                if (lineReplacesALL[i].IndexOf(searchName) == 0)
                {//発見.
                    return lineReplacesALL[i].Split('\t');
                }
            }
            return null;
        }

        void ScanStringForReverseSourceCode(string filename)
        {
            if (!File.Exists(filename + TRANSLATE_LINK))
            {//リンクデータがない!
                return;
            }
            List<string> lineReplacesALL = new List<string>();
            lineReplacesALL.AddRange(File.ReadAllLines(filename + TRANSLATE_LINK));

            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.IndexOf("///No Translate") >= 0)
                {
                    continue;
                }
                line = tryStrcatCode(lines, i);
                line = tryStrcatCode2(filename, lines, i);

                List<string> lineReplaces = new List<string>();

                int end = 0;
                while (true)
                {
                    int start = line.IndexOf("\"", end);
                    if (start < 0)
                    {
                        break;
                    }
                    start++;
                    int es = start;
                    end = -1;
                    while (true)
                    {
                        end = line.IndexOf("\"", es);
                        if (end <= 0)
                        {
                            break;
                        }
                        if (line[end - 1] == '\\')
                        {
                            es = end + 1;
                            continue;
                        }
                        break;
                    }
                    if (end < 0)
                    {
                        break;
                    }
                    string f = line.Substring(start, end - start);

                    lineReplaces.Add(f);
                    end++;
                }

                for (int n = 0; n < lineReplaces.Count; n++)
                {
                    string f = lineReplaces[n];


                    //キーになる名前はあるか?
                    string codeName = findCodeName(lines, i);
                    if (codeName == "")
                    {//対応する構文がない??
                        continue;
                    }
                    //ソースコードに対応する翻訳データを拾い上げる.
                    string[] org = findSourceCode(lineReplacesALL, codeName);
                    if (org == null)
                    {//ソースコードに対応する構文がない
                        continue;
                    }

                    string jpName = org[1];
                    string f2 = f.Replace("\\r\\n", "\r\n");
                    string jpName2 = jpName.Replace("\\r\\n", "\r\n");

                    //翻訳データはありますか?
                    string t = this.TranslateResource.str(jpName2);
                    if (t == "" || t != f2)
                    {//翻訳データと違うので新規作成.
                        this.TranslateResource.replaceTranslateString(jpName2, f2);
                    }
                    //日本語に戻す.
                    line = line.Replace("\"" + f + "\"", "\"" + jpName + "\"");
                    lines[i] = line;
                }
            }

            if (!U.CanWriteFileRetry(filename))
            {
                return;
            }
            //ソースコードをファイルに書き込み.
            File.WriteAllLines(filename, lines);
            //変換したので、linkファイルはいらないので消す.
            File.Delete(filename + TRANSLATE_LINK);
        }
        public void DesignStringReverse(string source_path)
        {
            string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", this.Lang + ".txt");
            this.TranslateResource.LoadResource(resoucefilename);

            //デザインのソースコードスキャンして英語等の言語を日本語文字列に置き換え.
            {
                string[] files = U.Directory_GetFiles_Safe(source_path, "*.Designer.cs", SearchOption.TopDirectoryOnly);
                foreach (string fullfilename in files)
                {
                    InputFormRef.DoEvents(null, fullfilename);
                    ScanStringForReverseSourceCode(fullfilename);
                }
            }
            //翻訳リソースをファイルに保存.
            this.TranslateResource.WriteResource(resoucefilename);
        }

        //パッチの翻訳
        public void ScanPatch()
        {
            string path = System.IO.Path.Combine(Program.BaseDirectory, "config", "patch2");
            string[] patchs = U.Directory_GetFiles_Safe(path, "PATCH_*.txt", SearchOption.AllDirectories);
            for (int i = 0; i < patchs.Length ; i++)
            {
                if (U.GetFileSize(patchs[i]) < 10)
                {
                    continue;
                }

                UpdatePatchAttribute(patchs[i], "NAME");
                UpdatePatchAttribute(patchs[i], "INFO");
                UpdatePatchAttribute(patchs[i], "AUTHOR");
                UpdatePatchAttribute(patchs[i], "EVENTSCRIPT");
                for (int n = 0; n < 10; n++)
                {
                    UpdatePatchAttribute(patchs[i], "EVENTSCRIPT:" + n);
                }

                UpdatePatchAttributeCOMBO(patchs[i]);
                UpdatePatchAttributeStruct(patchs[i]);
            }
        }
        //MODの翻訳
        public void ScanMOD()
        {
            string path = System.IO.Path.Combine(Program.BaseDirectory, "config", "patch2");
            string[] mods = U.Directory_GetFiles_Safe(path, "MOD_*.txt", SearchOption.AllDirectories);
            for (int i = 0; i < mods.Length; i++)
            {
                UpdateMODAttributeJ(mods[i]);
            }
        }


        void UpdatePatchAttribute(string patch, string key)
        {
            string en;
            string name = GetPatchAttribute(patch, key, out en);

            if (name == "" && key == "NAME")
            {//名前なのに名前がない
                //ファイル名から取り直す
                name = Path.GetFileNameWithoutExtension(patch);
                name = U.substr(name, 6); //skip "PATCH_"
            }

            if (en != "")
            {
                return;
            }
            if (name == "")
            {
                return;
            }

            en = MakeTranslate(name);
            if (en == "")
            {
                return;
            }
            if (en == name)
            {
                return;
            }

            AppendLines(patch, key + "." + this.Lang + "=" + en);
        }
        void UpdatePatchAttributeCOMBO(string patch)
        {
            string en;
            string value = GetPatchAttribute(patch, "COMBO", out en);

            if (en != "")
            {
                return;
            }
            if (value == "")
            {
                return;
            }

            string[] sp = value.Split('|');
            for (int i = 0; i < sp.Length; i+= 2 )
            {
                sp[i] = MakeTranslate(sp[i]);
            }
            en = string.Join("|", sp);
            if (en == "")
            {
                return;
            }
            if (value == en)
            {
                return;
            }

            AppendLines(patch, "COMBO." + this.Lang + "=" + en);
        }
        void UpdatePatchAttributeStruct(string patch)
        {
            List<string> list = GetPatchStruct(patch,":EVENT");
            for (int i = 0; i < list.Count; i++)
            {
                UpdatePatchAttribute(patch, list[i]);
            }
        }

        void UpdateMODAttributeJ(string mod)
        {
            List<string> list = GetPatchStruct(mod,"J_");
            for (int i = 0; i < list.Count; i++)
            {
                UpdatePatchAttribute(mod, list[i]);
            }
        }

        string lang_to_googlelang(string lang)
        {
            if (lang == "zh")
            {
                return "zh-CN";
            }
            return lang;
        }

        string GetPatchAttribute(string patch, string key, out string out_en)
        {
            out_en = "";

            string ret_value = "";
            string lang_key = key + "." + this.Lang;

            string[] lines = File.ReadAllLines(patch);
            for (int i = 0; i < lines.Length; i++ )
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                line = line.Trim();

                int sep = line.IndexOf('=');
                if (sep < 0)
                {
                    continue;
                }
                string kkey = line.Substring(0, sep);
                if (kkey == key)
                {
                    ret_value = line.Substring(sep + 1);
                }
                if (kkey == lang_key)
                {
                    out_en = line.Substring(sep + 1);
                }
            }
            return ret_value;
        }
        List<string> GetPatchStruct(string patch, string eventkey)
        {
            List<string> ret = new List<string>();

            string[] lines = File.ReadAllLines(patch);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                line = line.Trim();

                int sep = line.IndexOf('=');
                if (sep < 0)
                {
                    continue;
                }
                string kkey = line.Substring(0, sep);
                if (kkey.IndexOf(eventkey) < 0)
                {
                    continue;
                }
                if (sep >= 3 && kkey.Substring(sep - 3).IndexOf(".") >= 0)
                {
                    continue;
                }
                string value = line.Substring(sep + 1);
                if (U.isAsciiString(value))
                {
                    continue;
                }
                ret.Add(kkey);
            }
            return ret;
        }

        //データの翻訳
        public void ScanData()
        {
            string path = System.IO.Path.Combine(Program.BaseDirectory, "config", "data");
            string[] data = U.Directory_GetFiles_Safe(path, "*.txt", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < data.Length; i++)
            {
                string filename = Path.GetFileNameWithoutExtension(data[i]);
                if (filename.IndexOf(".") > 0)
                {
                    continue;
                }
                if (filename.IndexOf("template_event_") == 0)
                {
                    continue;
                }
                if (filename.IndexOf("song_instrument") == 0)
                {
                    continue;
                }

                string langfilename = System.IO.Path.Combine(path, filename + "." + this.Lang + ".txt");
                UpdateData(data[i], langfilename);
            }
        }
        void UpdateData(string filename, string langfilename)
        {
            string[] langlines;
            if (File.Exists(langfilename))
            {
                langlines = File.ReadAllLines(langfilename);
            }
            else
            {
                langlines = new string[0];
            }
            StringBuilder addline = new StringBuilder();
            bool isChange = false;

            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line))
                {
                    continue;   
                }
                line = line.Trim();

                string[] values = line.Split('\t');
                string[] eq = values[0].Split('=');

                if (! U.isAsciiString(eq[0]))
                {//トップが非アスキーなので危険.
                    Log.Error("file;{0} {1} is not acsii",filename,eq[0]);
                    //全部書き換えましょう.
                    WriteDataFileALL(filename, langfilename);
                    return;
                }

                bool isFound = false;
                for (int n = 0; n < langlines.Length; n++)
                {
                    string langline = langlines[n];
                    if (U.IsComment(langline))
                    {
                        continue;
                    }
//                    langline = U.ClipComment(langline);
                    langline = langline.Trim();

                    string[] langvalues = langline.Split('\t');
                    string[] langeq = langvalues[0].Split('=');

                    if (eq[0] == langeq[0])
                    {
                        isFound = true;
                        break;
                    }
                }
                if (isFound == true)
                {//存在するので無視する.
                    continue;
                }

                for (int n = 0; n < eq.Length; n++)
                {
                    string t = MakeTranslate(eq[n]);
                    if (t != eq[n])
                    {
                        eq[n] = t;
                        isChange = true;
                    }
                }
                for (int n = 1; n < values.Length; n++)
                {
                    string t = MakeTranslate(values[n]);
                    if (t != values[n])
                    {
                        values[n] = t;
                        isChange = true;
                    }
                }
                values[0] = string.Join("=", eq);
                line = string.Join("\t", values);
                addline.AppendLine(line);
            }

            if (addline.Length <= 0 || isChange == false)
            {
                return;
            }

            AppendLines(langfilename, addline.ToString() );
        }

        void WriteDataFileALL(string filename, string langfilename)
        {
            string[] langlines;
            if (File.Exists(langfilename))
            {//既にファイルが存在しているので何もしない
                return;
            }
            else
            {
                langlines = new string[0];
            }
            StringBuilder addline = new StringBuilder();
            bool isChange = false;

            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line))
                {
                    continue;
                }
                line = line.Trim();

                string[] values = line.Split('\t');

                for (int n = 0; n < values.Length; n++)
                {
                    string t = MakeTranslate(values[n]);
                    if (t != values[n])
                    {
                        values[n] = t;
                        isChange = true;
                    }
                }
                line = string.Join("\t", values);
                addline.AppendLine(line);
            }

            if (addline.Length <= 0 || isChange == false)
            {
                return;
            }

            AppendLines(langfilename, addline.ToString());
        }

        void AppendLines(string filename,string append)
        {

            string lines;
            if (File.Exists(filename))
            {
                lines = File.ReadAllText(filename);
                int len = lines.Length;
                if (lines.Length >= 2 && lines.Substring(len - 2) != "\r\n")
                {
                    lines += "\r\n";
                }
            }
            else
            {
                lines = "";
            }

            lines += append;

            if (!U.CanWriteFileRetry(filename))
            {
                return;
            }

            U.WriteAllText(filename, lines);
        }
    }
}
