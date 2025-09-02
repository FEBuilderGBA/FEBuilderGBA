using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Policy;

namespace FEBuilderGBA
{
    class ToolTranslateROMFont
    {
        Font UseAutoGenFont;
        PatchUtil.PRIORITY_CODE MyselfPriorityCode;
        PatchUtil.PRIORITY_CODE YourPriorityCode;
        Dictionary<string, bool> ProcessedFont;
        ROM YourROM;
        PatchUtil.PRIORITY_CODE ExtraFontPriorityCode;
        ROM ExtraFontROM;
        Undo.UndoData UndoData;
        RecycleAddress Recycle;

        public void ImportFont(Form self, string FontROMTextBox, string extrafontrom, bool FontAutoGenelateCheckBox, Font ttf, RecycleAddress recycle, Undo.UndoData undodata)
        {
            string filename = FontROMTextBox;
            this.YourROM = new ROM();
            this.Recycle = recycle;
            this.UndoData = undodata;
            this.ProcessedFont = new Dictionary<string, bool>();
            this.MyselfPriorityCode = PatchUtil.SearchPriorityCode();

            string version;

            //追加フォントROM
            this.ExtraFontROM = new ROM();
            if (this.ExtraFontROM.Load(extrafontrom, out version))
            {//フォントを取るようのROM
                this.ExtraFontPriorityCode = PatchUtil.SearchPriorityCode(this.ExtraFontROM);
            }
            else
            {
                this.ExtraFontROM = null;
            }

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
                if (Program.ROM.RomInfo.is_multibyte)
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
                if (Program.ROM.RomInfo.is_multibyte)
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
                if (Program.ROM.RomInfo.is_multibyte && Program.ROM.RomInfo.version == 7)
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
                //エイリアス
                AliasFont();
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

        static byte[] GetFontFromROM(uint moji, bool isItemFont, ROM rom, PatchUtil.PRIORITY_CODE priorityCode)
        {
            if (rom == null)
            {
                return null;
            }

            //相手のROMにはあるかな？
            uint topaddress_your;
            uint fontaddress_your;
            uint prevaddress_your;

            topaddress_your = FontForm.GetFontPointer(isItemFont, rom);
            fontaddress_your = FontForm.FindFontData(topaddress_your
                , moji
                , out prevaddress_your
                , rom
                , priorityCode);
            if (fontaddress_your == U.NOT_FOUND)
            {//相手のROMにもない
                return null;
            }
            else
            {
                //fontデータの取得
                return rom.getBinaryData(fontaddress_your, 8 * 64);
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
                newFontData = GetFontFromROM(moji, isItemFont, this.YourROM, this.YourPriorityCode);
                if (newFontData != null)
                {
                    Log.Notify("Font Porting", one);
                    FontForm.TransportFontStruct(newFontData, moji, this.MyselfPriorityCode, this.YourPriorityCode);
                }
                else
                {
                    newFontData = GetFontFromROM(moji, isItemFont, this.ExtraFontROM, this.ExtraFontPriorityCode);
                    if (newFontData != null)
                    {
                        Log.Notify("Font Porting", one);
                        FontForm.TransportFontStruct(newFontData, moji, this.MyselfPriorityCode, this.ExtraFontPriorityCode);
                    }
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

            uint newaddr = this.Recycle.Write(newFontData, this.UndoData);
            if (newaddr == U.NOT_FOUND)
            {//エラー
                return;
            }

            //ひとつ前のフォントリストのポインタを、現在追加した最後尾にすげかえる.
            Program.ROM.write_u32(prevaddress_my + 0, U.toPointer(newaddr), this.UndoData);
        }

        void AliasFont()
        {
            if (Program.ROM.RomInfo.is_multibyte)
            {
                AliasFontOne("0", "０");///No Translate
                AliasFontOne("1", "１");///No Translate
                AliasFontOne("2", "２");///No Translate
                AliasFontOne("3", "３");///No Translate
                AliasFontOne("4", "４");///No Translate
                AliasFontOne("5", "５");///No Translate
                AliasFontOne("6", "６");///No Translate
                AliasFontOne("7", "７");///No Translate
                AliasFontOne("8", "８");///No Translate
                AliasFontOne("9", "９");///No Translate
            }
            else
            {
                AliasFontOne("０", "0");///No Translate
                AliasFontOne("１", "1");///No Translate
                AliasFontOne("２", "2");///No Translate
                AliasFontOne("３", "3");///No Translate
                AliasFontOne("４", "4");///No Translate
                AliasFontOne("５", "5");///No Translate
                AliasFontOne("６", "6");///No Translate
                AliasFontOne("７", "7");///No Translate
                AliasFontOne("８", "8");///No Translate
                AliasFontOne("９", "9");///No Translate
            }
        }
        void AliasFontOne(string one_from, string one_to)
        {
            AliasFontOne(one_from, one_to, false, false);
            AliasFontOne(one_from, one_to, true, false);
        }
        void AliasFontOne(string one_from, string one_to, bool isItemFont, bool isSquareFont)
        {
            uint moji_from = U.ConvertMojiCharToUnit(one_from, this.MyselfPriorityCode);
            if (moji_from < 0x20 || moji_from == 0x80)
            {//制御文字なので無視
                Debug.Assert(false);
                return;
            }
            uint moji_to = U.ConvertMojiCharToUnit(one_to, this.MyselfPriorityCode);
            if (moji_to < 0x20 || moji_to == 0x80)
            {//制御文字なので無視
                Debug.Assert(false);
                return;
            }

            uint topaddress;
            uint fontaddress_from;
            uint prevaddress_from;
            topaddress = FontForm.GetFontPointer(isItemFont);
            fontaddress_from = FontForm.FindFontData(topaddress
                , moji_from
                , out prevaddress_from
                , this.MyselfPriorityCode);
            if (fontaddress_from == U.NOT_FOUND)
            {
                Debug.Assert(false);
                return;
            }

            uint fontaddress_to;
            uint prevaddress_to;
            fontaddress_to = FontForm.FindFontData(topaddress
                , moji_to
                , out prevaddress_to
                , this.MyselfPriorityCode);
            if (fontaddress_to == U.NOT_FOUND)
            {//ないならコピー
                byte[] bin = Program.ROM.getBinaryData(fontaddress_from, 72);
                U.write_u32(bin, 0, 0);   //NULL リストの末尾に追加するので.

                uint moji2 = (moji_to & 0xff);
                U.write_u8(bin, 4, moji2);   //SJIS2バイト目

                uint newaddr = this.Recycle.Write(bin, this.UndoData);
                if (newaddr == U.NOT_FOUND)
                {
                    Debug.Assert(false);
                    return;
                }
                //ひとつ前のフォントリストのポインタを、現在追加した最後尾にすげかえる.
                Program.ROM.write_u32(prevaddress_to + 0, U.toPointer(newaddr), this.UndoData);
            }
            else
            {
                byte[] bin = Program.ROM.getBinaryData(fontaddress_from + 5, 72 - 5);
                Program.ROM.write_range(fontaddress_to + 5, bin, this.UndoData);
            }
        }
    }
}
