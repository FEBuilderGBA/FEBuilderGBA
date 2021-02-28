using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    class ToolTranslateROMFont
    {
        Font UseAutoGenFont;
        PatchUtil.PRIORITY_CODE MyselfPriorityCode;
        PatchUtil.PRIORITY_CODE YourPriorityCode;
        Dictionary<string, bool> ProcessedFont;
        ROM YourROM;
        Undo.UndoData UndoData;
        public void ImportFont(Form self, string FontROMTextBox, bool FontAutoGenelateCheckBox, Font ttf)
        {
            string filename = FontROMTextBox;
            this.YourROM = new ROM();

            this.ProcessedFont = new Dictionary<string, bool>();
            this.MyselfPriorityCode = PatchUtil.SearchPriorityCode();

            string version;
            if (this.YourROM.Load(filename, out version))
            {//フォントを取るようのROM
                this.YourPriorityCode = PatchUtil.SearchPriorityCode(this.YourROM);
            }
            else
            {
                this.YourROM = null;
            }

            if (FontAutoGenelateCheckBox)
            {//自動生成する
                this.UseAutoGenFont = ttf;
            }
            else
            {//自動生成しない
                this.UseAutoGenFont = null;
            }
            FETextDecode decode = new FETextDecode();


            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
                this.UndoData = Program.Undo.NewUndoData("FONT Import");

                //文字列からフォントを探索
                {
                    List<U.AddrResult> list = TextForm.MakeItemList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        string text = decode.Decode((uint)i);
                        pleaseWait.DoEvents("String:" + U.To0xHexString((uint)i));

                        FontImporter(text);
                    }
                }
                //メニュー1
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    List<U.AddrResult> menuDefineList = MenuDefinitionForm.MakeListAll();
                    for (int n = 0; n < menuDefineList.Count; n++)
                    {
                        if (!U.isSafetyOffset(menuDefineList[n].addr + 8))
                        {
                            continue;
                        }
                        uint p = menuDefineList[n].addr + 8;
                        if (!U.isSafetyOffset(Program.ROM.p32(p)))
                        {
                            continue;
                        }
                        List<U.AddrResult> list = MenuCommandForm.MakeListPointer(p);
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (!U.isSafetyOffset(list[i].addr))
                            {
                                continue;
                            }
                            uint textid = Program.ROM.u32(list[i].addr + 0);
                            string str = FETextDecode.Direct(textid);

                            pleaseWait.DoEvents("Menu:" + U.To0xHexString(textid));
                            FontImporter(str);
                        }
                    }
                }

                //地形
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    List<U.AddrResult> list = MapTerrainNameForm.MakeList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!U.isSafetyOffset(list[i].addr))
                        {
                            continue;
                        }
                        uint textid = Program.ROM.u32(list[i].addr + 0);
                        string str = FETextDecode.Direct(textid);

                        pleaseWait.DoEvents("Terrain:" + U.To0xHexString(textid));
                        FontImporter(str);
                    }
                }

                //サウンドルーム
                //FE7のサウンドルームは、日本語直地
                if (Program.ROM.RomInfo.is_multibyte() && Program.ROM.RomInfo.version() == 7)
                {
                    List<U.AddrResult> list = SoundRoomForm.MakeList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!U.isSafetyOffset(list[i].addr))
                        {
                            continue;
                        }
                        uint textid = Program.ROM.u32(list[i].addr + 12);
                        string str = FETextDecode.Direct(textid);

                        pleaseWait.DoEvents("SoundRoom:" + U.To0xHexString(textid));
                        FontImporter(str);
                    }
                }
                //その他文字列
                {
                    List<U.AddrResult> list = OtherTextForm.MakeList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!U.isSafetyOffset(list[i].addr))
                        {
                            continue;
                        }

                        uint p_str = Program.ROM.p32(list[i].addr);
                        string str = Program.ROM.getString(p_str);

                        pleaseWait.DoEvents("Other:" + U.To0xHexString(p_str));
                        FontImporter(str);
                    }
                }

                Program.Undo.Push(this.UndoData);
            }
        }
        void FontImporter(string text)
        {
            for (int n = 0; n < text.Length; )
            {
                if (text[n] == '@')
                {//skip escape code
                    n += 5;
                    continue;
                }
                if (text[n] == '\r')
                {
                    n += 2;
                    continue;
                }
                if (text[n] == '\n')
                {//念のため
                    n += 1;
                    continue;
                }
                string one = text.Substring(n, 1);
                bool isSquareFont = false;

                if (!this.ProcessedFont.ContainsKey(one))
                {
                    FontImporterOne(one, true, isSquareFont);
                    FontImporterOne(one, false, isSquareFont);
                    this.ProcessedFont[one] = true;
                }
                n++;
            }
        }
        void FontImporterOne(string one, bool isItemFont, bool isSquareFont)
        {
            uint moji = U.ConvertMojiCharToUnit(one, this.MyselfPriorityCode);
            if (moji < 0x20 || moji == 0x80)
            {//制御文字なので無視
                return;
            }

            uint topaddress_my;
            uint fontaddress_my;
            uint prevaddress_my;

            topaddress_my = FontForm.GetFontPointer(isItemFont);
            fontaddress_my = FontForm.FindFontData(topaddress_my
                , moji
                , out prevaddress_my
                , this.MyselfPriorityCode);
            if (fontaddress_my != U.NOT_FOUND)
            {//既存ROMに存在している
                return;
            }
            if (prevaddress_my == U.NOT_FOUND)
            {//このフォントはルールにより登録できない.日本語フォントで 0x100以下とか.
                return;
            }

            //既存ROMにはないフォント
            byte[] newFontData = null;
            if (this.YourROM != null)
            {//相手のROMにあるかどうか.
                uint your_moji = U.ConvertMojiCharToUnit(one, this.YourPriorityCode);
                if (your_moji < 0x20)
                {//制御文字なので無視
                    return;
                }

                //相手のROMにはあるかな？
                uint topaddress_your;
                uint fontaddress_your;
                uint prevaddress_your;

                topaddress_your = FontForm.GetFontPointer(isItemFont, this.YourROM);
                fontaddress_your = FontForm.FindFontData(topaddress_your
                    , moji
                    , out prevaddress_your
                    , this.YourROM
                    , this.YourPriorityCode);
                if (fontaddress_your == U.NOT_FOUND)
                {//相手のROMにもない
                    newFontData = null;
                }
                else
                {
                    Log.Notify("Font Porting", one);

                    //fontデータの取得
                    newFontData = this.YourROM.getBinaryData(fontaddress_your, 8 * 64);

                    FontForm.TransportFontStruct(newFontData ,moji , this.MyselfPriorityCode , this.YourPriorityCode );
                }
            }

            if (newFontData == null //存在しないフォントで
                && this.UseAutoGenFont != null //自動生成する場合
                )
            {
                Log.Notify("Font Auto gen", one);

                //自動生成.
                int font_width;
                Bitmap autogen = ImageUtil.AutoGenerateFont(one
                    , this.UseAutoGenFont
                    , isItemFont
                    , isSquareFont
                    , out font_width);
                if (autogen == null)
                {//ない
                    return;
                }
                byte[] fontimage = ImageUtil.Image4ToByte(autogen);

                newFontData = FontForm.MakeNewFontData(moji
                    , (uint)font_width
                    , fontimage
                    , Program.ROM
                    , this.MyselfPriorityCode);
            }

            if (newFontData == null)
            {//存在しない
                return;
            }

            U.write_u32(newFontData, 0, 0);   //NULL リストの末尾に追加するので.

            uint newaddr = InputFormRef.AppendBinaryData(newFontData, this.UndoData);
            if (newaddr == U.NOT_FOUND)
            {//エラー
                return;
            }

            //ひとつ前のフォントリストのポインタを、現在追加した最後尾にすげかえる.
            Program.ROM.write_u32(prevaddress_my + 0, U.toPointer(newaddr), this.UndoData);
        }
    }
}
