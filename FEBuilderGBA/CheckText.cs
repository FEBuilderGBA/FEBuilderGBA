using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace FEBuilderGBA
{
    public enum CheckBlockResult
    {
        NoError
       ,ErrorWidth
       ,ErrorHeight
       ,ErrorFont
    }

    public class CheckText
    {
        public string ErrorString { get; private set; }
        public string ErrorFont { get; private set; }

        bool FoundUnkownFont;
        bool IsMultiByte;
        bool HasAutoNewLine;
        OptionForm.textencoding_enum TextEncoding;
        OptionForm.lint_text_skip_bug_enum LintTextSkipBug;

        public CheckText(string mainText)
        {
            this.TextEncoding = OptionForm.textencoding();
            this.LintTextSkipBug = OptionForm.lint_text_skip_bug();
            this.IsMultiByte = Program.ROM.RomInfo.is_multibyte();
            this.HasAutoNewLine = CheckHasAutoNewLine(mainText);
        }

        bool CheckHasAutoNewLine(string text)
        {
            if (PatchUtil.SearchAutoNewLinePatch() == PatchUtil.AutoNewLine_enum.AutoNewLine)
            {//自動改行が入っている場合は、長さのチェックをしない
                if (text.IndexOf("@0080@0090") >= 0
                    || text.IndexOf("@0080@0091") >= 0
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public CheckBlockResult CheckBlockBox(string text, int widthLimit, int heightLimit, bool isItemFont)
        {
            if (this.TextEncoding == OptionForm.textencoding_enum.ZH_TBL)
            {//中国語の場合、今のところフォントデータが取れないので何もチェックできない.
                return CheckBlockResult.NoError;
            }
            if (Program.ROM.RomInfo.is_multibyte())
            {//日本語の場合 (.+?)を消す. (ワイバーンナイト)とか
                text = RegexCache.Replace(text, @"\(.+?\)", "");
            }
            this.FoundUnkownFont = false;

            string[] blocks = text.Split(new string[] { "@0002", "@0004", "@0005", "@0006", "@0007" }, StringSplitOptions.RemoveEmptyEntries);
            for (int n = 0; n < blocks.Length; n++)
            {
                Size size = MeasureTextMultiLine(blocks[n], isItemFont);
                if (this.FoundUnkownFont)
                {
                    if (this.LintTextSkipBug == OptionForm.lint_text_skip_bug_enum.DetectButExceptForVanilla)
                    {//検出するが、無改造ROMにある、もとからあるものは除く
                        if (IsOrignalBug(blocks[n], n, size))
                        {
                            continue;
                        }
                    }

                    this.ErrorString = R._("警告:フォントがありません。\r\n文字:{0}\r\n{1}", this.ErrorFont, blocks[n]);
                    return CheckBlockResult.ErrorFont;
                }
                if (size.Width > widthLimit)
                {
                    if (this.HasAutoNewLine)
                    {//自動改行が入っている場合は、長さのチェックをしない
                    }
                    else
                    {
                        this.ErrorString = R._("警告:テキストが横に長すぎます。\r\n想定ドット数:({0} , {1})\r\n{2}", size.Width, size.Height, blocks[n]);
                        return CheckBlockResult.ErrorWidth;
                    }
                }
                if (size.Height > heightLimit)
                {
                    if (this.LintTextSkipBug == OptionForm.lint_text_skip_bug_enum.None)
                    {//設定により無視
                        continue;
                    }
                    else if (this.LintTextSkipBug == OptionForm.lint_text_skip_bug_enum.MoreThan4Lines)
                    {//4行以上
                        if (size.Height / 16 < 4)
                        {
                            continue;
                        }
                    }
                    else if (this.LintTextSkipBug == OptionForm.lint_text_skip_bug_enum.DetectButExceptForVanilla)
                    {//検出するが、無改造ROMにある、もとからあるものは除く
                        if (IsOrignalBug(blocks[n], n, size))
                        {
                            continue;
                        }
                    }
                    else
                    {//すべて検出する.
                    }

                    this.ErrorString = R._("警告:テキストの行数が多すぎます。\r\n想定ドット数({0} , {1})\r\n{2}", size.Width, size.Height, blocks[n]);
                    return CheckBlockResult.ErrorHeight;
                }
            }
            return CheckBlockResult.NoError;
        }

        bool IsOrignalBug(string str, int n, Size size)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    if (n == 0 && size.Width == 156 && size.Height == 48 &&
                        str.IndexOf("エイリーク様はヒーニアス王子救出に\r\n") >= 0) ///No Translate
                    {
                        return true;
                    }
                    if (n == 0 && size.Width == 172 && size.Height == 48 &&
                        str.IndexOf("それに、せっかくここにいるのに、\r\n") >= 0) ///No Translate
                    {
                        return true;
                    }
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    if (n == 0 && size.Width == 107 && size.Height == 48 &&
                        str.IndexOf("狭い通路と出入り口を\r\n") > 0) ///No Translate
                    {
                        return true;
                    }
                }
                else
                {
                    if (n == 5 && size.Width == 171 && size.Height == 48 &&
                        str.IndexOf("But I'm so young,") > 0) ///No Translate
                    {
                        return true;
                    }
                    if (n == 0 && size.Width == 126 && size.Height == 48 &&
                        str.IndexOf("My name is Serra.") > 0) ///No Translate
                    {
                        return true;
                    }
                }
            }
            else
            {//FE6
                if (str == "官吏") ///No Translate
                {
                    return true;
                }

            }
            return false;
        }

        //フォントで描画した場合の幅と高さを求める.
        Size MeasureTextMultiLine(string str, bool IsItemFont)
        {
            uint maxwidth = 0;
            uint maxheight = 0;

            string[] lines = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            uint height = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int code0003Pos = line.IndexOf("@0003");

                line = TextForm.StripAllCode(line);
                if (height == 0 && line == "")
                {//最初の空行なので無視が妥当.
                    continue;
                }

                height++;
                if (code0003Pos == 0)
                {//冒頭に@0003がある場合は行に含めてはいけない.
                    height--;
                }
                if (code0003Pos >= 0)
                {
                    if (height > maxheight)
                    {
                        maxheight = height;
                    }
                    height = 0;
                }

                uint width = MeasureTextWidthOneLine(line, IsItemFont);
                if (code0003Pos > 0)
                {//@0003がある場合、2ドット使えるサイズが小さいらしい.
                    width += 2;
                }
                if (width > maxwidth)
                {
                    maxwidth = width;
                }
            }

            //最後の残り
            if (height > maxheight)
            {
                maxheight = height;
            }
            return new Size((int)maxwidth, 16 * (int)maxheight);
        }
        //フォントで描画した場合の幅を求める.
        uint MeasureTextWidthOneLine(string str, bool IsItemFont)
        {
            uint sum = 0;
            uint[] widths = FontForm.MeasureTextWidthOneLineInts(str, IsItemFont);
            for (int i = 0; i < widths.Length; i++)
            {
                if (widths[i] <= 0)
                {
                    char o = str[i];
                    if (o == 0x001F)
                    {
                        continue;
                    }
                    if (this.IsMultiByte == false)
                    {//シングルバイト圏ではこのチェックをしない.
                        continue;
                    }
                    this.FoundUnkownFont = true;
                    this.ErrorFont = o.ToString();
                    break;
                }
                else
                {
                    sum += widths[i];
                }
            }
            return sum;
        }
    }
}
