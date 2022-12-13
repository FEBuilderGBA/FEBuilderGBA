using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace FEBuilderGBA
{
    class TranslateTextUtil
    {
        static List<string> SplitEscapeString(string src)
        {
            List<string> list = new List<string>();
            int textstart = 0;
            for (int i = 0; i < src.Length; )
            {
                if (src[i] == '@')
                {
                    if (i - textstart > 0)
                    {
                        string text = U.substr(src, textstart, i - textstart);
                        list.Add(text);
                    }
                    string code = U.substr(src, i, 5);
                    uint codeint = U.atoh(code.Substring(1));
                    if (codeint == 3 && U.substr(src, i + 5, 2) == "\r\n")
                    {//@0003の後は改行をいれているのでそれも回収する.
                        code = U.substr(src, i, 5 + 2);
                        list.Add(code);
                        i += 5 + 2;
                    }
                    else
                    {
                        list.Add(code);
                        i += 5;
                    }

                    textstart = i;
                }
                else
                {
                    i++;
                }
            }

            if (textstart == 0)
            {//エスケープがない場合文字全体が対象
                list.Add(src);
            }
            return list;
        }

        //FE8は 日本語版と英語版で顔の並び順が違う!!!
        //日本語版だけ 0x48 というダミーデータがあるため
        //このルーチンは、0x48以降の顔データ参照を相互変換します.
        enum FE8FaceCode48Fix
        {
             NONE
            ,INC //JA to EN
            ,DEC //EN to JA
        };
        static FE8FaceCode48Fix is_FE8FaceCode48Fix(ROM from_rom,ROM to_rom)
        {
            if (from_rom.RomInfo.version != 8)
            {
                return FE8FaceCode48Fix.NONE;
            }
            if (from_rom.RomInfo.is_multibyte)
            {
                if (to_rom.RomInfo.is_multibyte)
                {//JA TO JA
                    return FE8FaceCode48Fix.NONE;
                }
                else
                {//JA to EN
                    return FE8FaceCode48Fix.INC;
                }
            }
            else
            {
                if (to_rom.RomInfo.is_multibyte)
                {//EN to JA
                    return FE8FaceCode48Fix.DEC;
                }
                else
                {//EN to EN
                    return FE8FaceCode48Fix.NONE;
                }
            }
        }
        static string Trim001F(string text)
        {
            return text.Replace(((char)0x1f).ToString(),"");
        }

        static string FE8SkipFace48(string text,FE8FaceCode48Fix fix)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; )
            {
                if (text[i] == '@')
                {
                    string code = U.substr(text, i, 5);
                    uint codeint = U.atoh(code.Substring(1));
                    if (codeint > 0x148 && codeint < 0xFFF)
                    {
                        if (fix == FE8FaceCode48Fix.INC)
                        {
                            codeint++;
                        }
                        else
                        {
                            codeint--;
                        }
                        string newFaceCode = codeint.ToString("X04");
                        sb.Append('@');
                        sb.Append(newFaceCode);
                    }
                    else
                    {
                        sb.Append(code);
                    }
                    i += 5;
                }
                else
                {
                    sb.Append(text[i]);
                    i++;
                }
            }
            return sb.ToString();
        }

        //固定辞書の読込
        public static void AppendFixedDic(Dictionary<string, string> dic,string from, string to)
        {
            bool isRev;
            string fullfilename;

            if (from == "ja" && to == "en")
            {
                isRev = false;
                fullfilename = Path.Combine(Program.BaseDirectory, "config", "translate", "dic_ja_en.txt");
            }
            else if (from == "en" && to == "ja")
            {
                isRev = true;
                fullfilename = Path.Combine(Program.BaseDirectory, "config", "translate", "dic_ja_en.txt");
            }
            else
            {
                return;
            }

            if (! File.Exists(fullfilename))
            {//辞書がない
                return ;
            }

            string[] lines = File.ReadAllLines(fullfilename);
            foreach (string line in lines)
            {
                string[] sp = line.Split('\t');
                if (sp.Length < 2)
                {
                    continue;
                }

                string key;
                string value;

                if (isRev)
                {
                    key = sp[1];
                    value = sp[0];
                }
                else
                {
                    key = sp[0];
                    value = sp[1];
                }
                key = key.Replace("\\r\\n", "\r\n");
                value = value.Replace("\\r\\n", "\r\n");

                key = key.ToUpper();
                if (dic.ContainsKey(key))
                {
                    continue;
                }

                dic[key] = value;
            }
        }

        //翻訳用のよくあるテキスト集の作成.
        public static Dictionary<string, string> LoadTranslateDic(string from, string to, string rom_from, string rom_to)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            ROM rom_f = new ROM();
            string version;
            if (!rom_f.Load(rom_from, out version))
            {
                return dic;
            }

            ROM rom_t = new ROM();
            if (!rom_t.Load(rom_to, out version))
            {
                return dic;
            }

            if (rom_f.RomInfo.version != rom_t.RomInfo.version)
            {
                return dic;
            }

            SystemTextEncoder from_tbl,to_tbl;
            int from_n;
            bool trimEnd1F = true;
            if (from == "ja")
            {
                from_n = 0;
                from_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.Shift_JIS,rom_f);
            }
            else if (from == "en")
            {
                from_n = 1;
                if (rom_f.RomInfo.version == 6)
                {
                    from_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.EN_TBL, rom_f);
                }
                else
                {
                    from_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.Shift_JIS, rom_f);
                }
                trimEnd1F = false;
            }
            else if (from == "zh" || from == "zh-CN")
            {
                from_n = 0;
                from_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.ZH_TBL, rom_f);
            }
            else
            {//fromが対応外.
                return dic;
            }

            int to_n;
            if (to == "ja")
            {
                to_n = 0;
                to_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.Shift_JIS,rom_t);
            }
            else if (to == "en")
            {
                to_n = 1;
                if (rom_t.RomInfo.version == 6)
                {
                    to_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.EN_TBL, rom_t);
                }
                else
                {
                    to_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.Shift_JIS, rom_t);
                }
            }
            else if (to == "zh" || to == "zh-CN")
            {
                to_n = 0;
                to_tbl = new SystemTextEncoder(OptionForm.textencoding_enum.ZH_TBL, rom_t);
            }
            else
            {//toが対応外.
                return dic;
            }

            //FE8は、日本語版と英語版で顔画像の並びが違うので補正するかどうかのチェック
            FE8FaceCode48Fix fe8faceCode48Fix = is_FE8FaceCode48Fix(rom_f, rom_t);

            string filename = U.ConfigDataFilename("translate_textid_", rom_f);
            string[] lines = File.ReadAllLines(filename);
            FETextDecode from_decoder = new FETextDecode(rom_f, from_tbl);
            FETextDecode to_decoder = new FETextDecode(rom_t, to_tbl);
            for (int i = 0; i < lines.Length; i++)
            {
                if (U.IsComment(lines[i]))
                {
                    continue;
                }
                string line = U.ClipComment(lines[i]);
                string[] sp = line.Split('\t');
                if (sp.Length < 2)
                {
                    continue;
                }
                string from_string, to_string;
                uint from_key = U.atoh(sp[from_n]);
                uint orignal_from_key = from_key; 
                if (from_key <= 0)
                {
                    continue;
                }
                if (U.isSafetyPointer(from_key, rom_f))
                {//ポインタの場合、実アドレスを求める.
                    from_key = rom_f.u32(U.toOffset(from_key));
                    if (! U.isSafetyPointer(from_key, rom_f))
                    {
                        continue;
                    }
                }
                from_string = from_decoder.Decode(from_key);
                from_string = from_string.ToUpper();

                uint to_key = U.atoh(sp[to_n]);
                int change_string_pos = sp[to_n].IndexOf('|');
                if (change_string_pos < 0)
                {
                    if (to_key <= 0)
                    {
                        dic[U.ToHexString(orignal_from_key) + "#NOTFOUND#" + from_string] = "";
                        continue;
                    }
                    if (U.isSafetyPointer(to_key, rom_t))
                    {//ポインタの場合、実アドレスを求める.
                        to_key = rom_t.u32(U.toOffset(to_key));
                        if (!U.isSafetyPointer(to_key, rom_t))
                        {
                            dic[U.ToHexString(orignal_from_key) + "#NOTFOUND#" + from_string] = "";
                            continue;
                        }
                    }
                    to_string = to_decoder.Decode(to_key);
                }
                else
                {//長さ当の問題で置換できないものは、代替えテキストを入れます.
                    to_string = sp[to_n].Substring(change_string_pos+1);
                }

                //FE8顔画像の修正.
                if (fe8faceCode48Fix != FE8FaceCode48Fix.NONE)
                {
                    to_string = FE8SkipFace48(to_string, fe8faceCode48Fix);
                }

                if (trimEnd1F)
                {
                    //末尾の001Fを消す
                    to_string = Trim001F(to_string);
                }

                dic[from_string] = to_string;
                dic[U.ToHexString(orignal_from_key) + "|"+from_string] = to_string;
            }

            return dic;
        }


        static string TranslateTextDic(uint fromkey,string text, Dictionary<string, string> transDic)
        {
            string upper_text = text.ToUpper();

            {
                string key = U.ToHexString(fromkey) + "|" + upper_text;
                if (transDic.ContainsKey(key))
                {
                    return transDic[key];
                }
            }
            {
                string key = U.ToHexString(fromkey) + "#NOTFOUND#" + upper_text;
                if (transDic.ContainsKey(key))
                {//NOT FOUND
                    return "";
                }
            }
            {
                string key = upper_text;
                if (transDic.ContainsKey(key))
                {
                    return transDic[key];
                }
            }
            return "";
        }

        static string TranslateTextGoogleTranslate(string text, string from, string to)
        {
            //定型文で変換できない場合GoogleTranslateへ
            List<string> list = SplitEscapeString(text);
            bool use0001 = false;
            bool use0003 = false;
            if (text.IndexOf("\r\n") >= 0)
            {
                use0001 = true;
            }
            if (text.IndexOf("@0003") >= 0)
            {
                use0003 = true;
            }

            uint addchar = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string oneline = list[i];
                if (oneline == "")
                {
                    continue;
                }
                if (oneline[0] == '@')
                {//エスケープ文字列は翻訳しない.
                    if (oneline.IndexOf("@0002") >= 0
                    || oneline.IndexOf("@0008") >= 0
                    || oneline.IndexOf("@0009") >= 0
                    || oneline.IndexOf("@000A") >= 0
                    || oneline.IndexOf("@000B") >= 0
                    || oneline.IndexOf("@000C") >= 0
                    || oneline.IndexOf("@000D") >= 0
                    || oneline.IndexOf("@000E") >= 0
                    || oneline.IndexOf("@000F") >= 0
                    || oneline.IndexOf("@0001") >= 0
                    || oneline.IndexOf("\r\n") >= 0
                        )
                    {
                        addchar = 0;
                    }
                    continue;
                }
                oneline = Cat0003(oneline , i, list);

                //英語の場合、改行はスペースを兼ねるので。
                if (from == "en")
                {
                    oneline = oneline.Replace("\r\n", " ");
                }
                else
                {
                    oneline = oneline.Replace("\r\n", "");
                }

                TranslateManage engine = new TranslateManage();
                string transline = engine.Trans(oneline, from, to);

                if (use0001)
                {
                    transline = InsertSerifnl(transline, to, use0003, ref addchar);
                }
                list[i] = transline;
            }

            string resultext = string.Join("", list);
            return resultext;
        }
        static string Cat0003(string oneline ,int index,List<string> list)
        {
            string ret = oneline;
            //次のデータが@0003ならば結合できる可能性を検討する.
            for (int i = index + 1; i < list.Count; i++)
            {
                string nextline = list[i];
                if (nextline.IndexOf("@0003") < 0)
                {//@0003ではないらしい
                    break;
                }
                if (i + 1 >= list.Count)
                {
                    break;
                }

                string morenextline = list[i + 1];
                if (morenextline.IndexOf("@") >= 0)
                {//次に続くのが@命令らしい
                    break;
                }
                list[i] = "";//結合したので、不要なので消す.
                list[i+1] = "";//結合したので、不要なので消す.
                ret += "\r\n" + morenextline;
            }
            return ret;
        }
        //テキストを翻訳する(時間がかかるので注意)
        public static string TranslateText(uint fromkey,string text, string from, string to, Dictionary<string, string> transDic, bool useGoolgeTranslate, bool modifiedTextOnly)
        {
            if (text == "" || from == to)
            {
                return "";
            }

            //定型文で変換できるか？
            string r = TranslateTextDic(fromkey,text, transDic);
            if (r == "")
            {
                //trimしたものでもマッチできないか?
                r = TranslateTextDic(fromkey, text.Trim(), transDic);
            }
            if (r != "")
            {//定型文で変換できるらしい.
                if (modifiedTextOnly)
                {//定型文で翻訳できるものには、利用しない場合は、[@SKIP]を返す.
                    return "[@SKIP]";
                }
                return r;
            }

            if (to == "en")
            {
                if (U.isAsciiString(text))
                {//既に英語じゃん
                    return text;
                }
            }
            else if (to == "ja")
            {
                if (U.isJAString(text))
                {//既に日本語じゃん
                    return text;
                }
            }

            if (useGoolgeTranslate == false)
            {//google翻訳を使わない場合、未翻訳の文章をそのまま返す.
                return text;
            }


            //定型文で変換できない場合GoogleTranslateへ
            r = TranslateTextGoogleTranslate(text, from, to);
            return r;
        }

        //適度に改行を挿入する
        static string InsertSerifnl(string serif, string to, bool use0003, ref uint ref_addchar)
        {
            if (serif.Trim() == "")
            {
                return "";
            }

            uint[] widths = FontForm.MeasureTextWidthOneLineInts(serif, false);

            //フォントがない場合の幅.
            uint unkownFontSize = FontForm.GetUnkownFontSize(to);
            int linesCount = 0;

            string result = "";
            int starti = 0;
            uint sum = ref_addchar;
            for (int i = 0; i < widths.Length; i++)
            {
                uint w = widths[i];
                if (w <= 0)
                {//フォントがないので長さが取れない場合推測する.
                    w = unkownFontSize;
                }
                sum += w + 1;
                if (sum + unkownFontSize < TextForm.MAX_SERIF_WIDTH)
                {
                    continue;
                }

                i = SearchBreakPoint(serif, starti, i - starti,to);

                string a = serif.Substring(starti, i - starti);
                a = a.Trim();
                if (a != "")
                {
                    linesCount++;

                    result += a;
                    if (use0003)
                    {//@0003が使える場合は、 2行枚に @0003 を挟む.
                        if (linesCount % 2 == 0)
                        {
                            result += "@0003";
                        }
                    }
                    result += "\r\n";
                }
                starti = i;
                sum = 0;
            }

            {
                string a = serif.Substring(starti);
                a = a.Trim();
                if (a != "")
                {
                    result += a;
                    ref_addchar = sum;
                }
            }
            return result;
        }

        //テキスト送りで、都合の良い分岐点を探す.
        static int SearchBreakPoint(string serif, int start, int length,string to)
        {
            //分割しても違和感がない、文末、または句読点、 ,等
            {
                char[] seplators = new char[] { '、', '。', '.', '・', ',', '，' };///No Translate

                int maxSeplator = 0;
                for (int n = 0; n < seplators.Length; n++)
                {
                    for (int i = start + length; i >= start + 1; i--)
                    {
                        if (serif[i] == seplators[n])
                        {//綺麗に分割できることが条件.
                            if (maxSeplator < i)
                            {
                                maxSeplator = i + 1;
                            }
                        }
                    }
                }
                if (maxSeplator > 0)
                {
                    return maxSeplator;
                }
            }

            //分かち書きで分割できるか？
            if (to == "ja")
            {
                TinySegmenter seg = new TinySegmenter();
                string[] sp = seg.Segment(serif.Substring(start, length));
                //分割できそうな文字があるか？
                string[] seplators = new string[] { "は", "を", "と", "の", "　", " " }; ///No Translate

                int maxSeplator = 0;
                for (int n = 0; n < seplators.Length; n++)
                {
                    int i = start;
                    for (int k = 0; k < sp.Length; k++)
                    {
                        i += sp[k].Length;
                        if (sp[k] == seplators[n])
                        {//綺麗に分割できることが条件.
                            if (maxSeplator < i)
                            {
                                maxSeplator = i;
                            }
                        }
                    }
                }
                if (maxSeplator > 0)
                {
                    return maxSeplator;
                }
            }
            else
            {
                char[] seplators = new char[] { ' ','　' };///No Translate

                int maxSeplator = 0;
                for (int n = 0; n < seplators.Length; n++)
                {
                    for (int i = start + length; i >= start + 1; i--)
                    {
                        if (serif[i] == seplators[n])
                        {//綺麗に分割できることが条件.
                            if (maxSeplator < i)
                            {
                                maxSeplator = i + 1;
                            }
                        }
                    }
                }
                if (maxSeplator > 0)
                {
                    return maxSeplator;
                }
            }

            //不明な場合は、すべて入れるしかない.
            return start + length;
        }


        public static void TranslateLanguageAutoSelect(out int out_from,out int out_to)
        {
            //from
            if (Program.ROM.RomInfo.is_multibyte)
            {
                if (OptionForm.textencoding() == OptionForm.textencoding_enum.ZH_TBL)
                {//中国語
                    out_from = 2;
                }
                else
                {//日本語
                    out_from = 0;
                }
            }
            else
            {//英語
                out_from = 1;
            }

            //to
            string lang = OptionForm.lang();
            if (lang == "zh")
            {
                out_to = 2;
            }
            else if (lang == "en")
            {
                out_to = 1;
            }
            else
            {
                out_to = 0;
            }

            //from == to ならば、適当にずらす.
            if (out_from == out_to)
            {
                if (out_to == 0)
                {
                    out_to = 1;
                }
                else
                {
                    out_to = 0;
                }
            }
        }

        public static string GetLangIntCodeToLangText(int lang)
        {
            if (lang == 0)
            {
                return "ja";
            }
            else if (lang == 1)
            {
                return "en";
            }
            else if (lang == 2)
            {
                return "zh";
            }

            return "en";
        }
        //テキストIDの区切り
        public static bool IsTextIDCode(string line)
        {
            if (line.Length < 4)
            {
                return false;
            }
            if (line.Length > 11)
            {
                return false;
            }
            if (line[0] != '[')
            {
                return false;
            }
            if (line[line.Length - 1] != ']')
            {
                return false;
            }
            if (line == "[XXXX]")
            {//無効なIDとして処理
                return true;
            }
            for (int i = 1; i < line.Length - 1; i++)
            {
                if (!U.ishex(line[i]))
                {
                    return false;
                }
            }
            
            return true;
        }

        static void AppendDicTextID(Dictionary<string, string> transDic, uint id, string text)
        {
            if (id <= 0)
            {
                return;
            }
            string to_string = U.substr(text, 0, text.Length - 2);
            to_string = TextForm.ConvertFEditorToEscape(to_string);

            FETextDecode decode = new FETextDecode();
            string from_string = decode.Decode(id);
            from_string = from_string.ToUpper();

            transDic[from_string] = to_string;
            transDic[U.ToHexString(id) + "|" + from_string] = to_string;
        }

        public static void AppendDicFixedFile(Dictionary<string, string> transDic, string filename)
        {
            if (!File.Exists(filename))
            {
                return;
            }

            uint id = U.NOT_FOUND;
            string text = "";
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                if (line.Length <= 0)
                {
                    continue;
                }

                if (!IsTextIDCode(line))
                {
                    text += line + "\r\n";
                    continue;
                }

                AppendDicTextID(transDic, id, text);

                //次のテキスト
                id = U.atoh(U.substr(line, 1));
                text = "";
            }

            //最後のデータ
            AppendDicTextID(transDic, id, text);
        }
        public static Dictionary<string, string> MakeFixedDic(
              string tralnslate_from, string tralnslate_to
            , string rom_from, string rom_to
        )
        {
            //よくある定型文の翻訳辞書
            Dictionary<string, string> transDic = new Dictionary<string, string>();
            if (rom_from != "" && rom_to != "")
            {
                transDic = TranslateTextUtil.LoadTranslateDic(tralnslate_from, tralnslate_to, rom_from, rom_to);
            }
            //固定文の辞書
            TranslateTextUtil.AppendFixedDic(transDic, tralnslate_from, tralnslate_to);

            return transDic;
        }

        public static uint GrepReverseSearchDic(string filename,string search)
        {
            string[] lines = File.ReadAllLines(filename);
            for (uint i = 0; i < lines.Length; i++)
            {
                string s = lines[i];
                if (s.IndexOf(search) >= 0)
                {
                    return i + 1;
                }
            }
            return U.NOT_FOUND;
        }
    }
}
