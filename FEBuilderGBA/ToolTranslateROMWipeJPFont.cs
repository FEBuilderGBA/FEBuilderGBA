using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    class ToolTranslateROMWipeJPFont
    {
        //フォントキープ
        class KeepFont
        {
            public bool IsItemFont;
            public string Moji;
            public uint MojiCode;
            public uint Width;
            public byte[] Data;

            public uint rewitePointer;
        }
        List<KeepFont> KeepFontList = new List<KeepFont>();
        PatchUtil.PRIORITY_CODE PriorityCode;
        Undo.UndoData UndoData;

        public ToolTranslateROMWipeJPFont(Undo.UndoData undoData)
        {
            this.UndoData = undoData;
            this.PriorityCode = PatchUtil.SearchPriorityCode();
        }

        void AddKeepFont(bool isItemFont, uint moji, uint rewitePointer = U.NOT_FOUND)
        {
            uint topaddress = FontForm.GetFontPointer(isItemFont);
            uint prevaddress;
            uint fontaddress = FontForm.FindFontData(topaddress
                , moji
                , out prevaddress
                , PriorityCode);
            if (fontaddress == U.NOT_FOUND)
            {
                return;
            }
            KeepFont kf = new KeepFont();
            kf.IsItemFont = isItemFont;
            kf.Moji = "Code"+U.To0xHexString(moji);
            kf.MojiCode = moji;
            kf.Width = Program.ROM.u8(fontaddress + 5);
            kf.Data = Program.ROM.getBinaryData(fontaddress + 8, 64);
            kf.rewitePointer = rewitePointer;
            KeepFontList.Add(kf);
        }
        void AddKeepFont(bool isItemFont, string one, uint rewitePointer = U.NOT_FOUND)
        {
            uint moji = U.ConvertMojiCharToUnit(one, PriorityCode);
            uint topaddress = FontForm.GetFontPointer(isItemFont);
            uint prevaddress;
            uint fontaddress = FontForm.FindFontData(topaddress
                , moji
                , out prevaddress
                , PriorityCode);
            if (fontaddress == U.NOT_FOUND)
            {
                return;
            }
            KeepFont kf = new KeepFont();
            kf.IsItemFont = isItemFont;
            kf.Moji = one;
            kf.MojiCode = moji;
            kf.Width = Program.ROM.u8(fontaddress + 5);
            kf.Data = Program.ROM.getBinaryData(fontaddress + 8, 64);
            kf.rewitePointer = rewitePointer;
            KeepFontList.Add(kf);
        }
        public void AddJPFonts(List<Address> list)
        {
            if (!Program.ROM.RomInfo.is_multibyte)
            {//英語ROMなので無関係
                return;
            }
            OptionForm.textencoding_enum textencoding = OptionForm.textencoding();
            if (textencoding == OptionForm.textencoding_enum.ZH_TBL)
            {//フォントシステムが違うので無理
                return;
            }

            if (Program.ROM.RomInfo.version != 8)
            {//FE8のみ
                return;
            }

            //消したらダメなのを登録
            MakeKeepFontFE8J();


            //既存のフォントテーブルのクリア
            uint textFontStart = 0x5942F4;
            uint textFontEnd = 0x5B8CDC;
            SerchCustomFonts(false, textFontEnd, list);
            Address.AddAddress(list
                , textFontStart
                , textFontEnd - textFontStart
                , U.NOT_FOUND
                , "TextFont Wipe"
                , Address.DataTypeEnum.BIN
                );
            uint itemFontStart = 0x579CCC;
            uint itemFontEnd = 0x593ECC;
            SerchCustomFonts(true, itemFontEnd, list);
            Address.AddAddress(list
                , itemFontStart
                , itemFontEnd - itemFontStart
                , U.NOT_FOUND
                , "ItemFont Wipe"
                , Address.DataTypeEnum.BIN
                );
            //アイテムフォントのテーブルを消す
            Program.ROM.write_fill(0x57994C, 896, 0);
            //テキストフォントのテーブルを消す
            Program.ROM.write_fill(0x593F74, 896, 0);
        }

        void SerchCustomFonts(bool isItemFont, uint ignoteEnd, List<Address> list)
        {
            uint topaddress = FontForm.GetFontPointer(isItemFont);

            for (uint moji1 = 0x1f; moji1 <= 0xff; moji1++)
            {
                //リストの元になるポインタへ移動.
                uint fontlist = topaddress + (moji1 << 2) - 0x100;
                if (!U.isSafetyOffset(fontlist))
                {
                    continue;
                }

                uint p = Program.ROM.p32(fontlist);
                if (!U.isSafetyOffset(p))
                {
                    continue;
                }
                uint before_pointer = fontlist;

                //同一ハッシュキーがあるため、リストをたどりながら目的のフォントを探します.
                //struct{
                //    void* next;
                //    byte  moji2
                //    byte  width
                //    byte  nazo1
                //    byte  nazo2
                //} //sizeof()==8
                //+64byte bitmap(4pp)
                while (p > 0)
                {
                    if (p >= ignoteEnd)
                    {
                        uint moji2 = Program.ROM.u8(p + 4);
                        FEBuilderGBA.Address.AddAddress(list
                            , p, 8 + 64
                            , before_pointer
                            , "WipeJP Font"
                            , Address.DataTypeEnum.BIN);
                    }

                    uint next = Program.ROM.u32(p);
                    if (next == 0)
                    {//リスト終端.
                        break;
                    }

                    if (!U.isSafetyPointer(next))
                    {//リストが壊れている.
                        break;
                    }

                    before_pointer = p;

                    //次のリストへ進む.
                    p = U.toOffset(next);
                }
            }
        }

        void MakeKeepFontFE8J()
        {
            AddKeepFont(false, "０"); ///No Translate
            AddKeepFont(false, "１"); ///No Translate
            AddKeepFont(false, "２"); ///No Translate
            AddKeepFont(false, "３"); ///No Translate
            AddKeepFont(false, "４"); ///No Translate
            AddKeepFont(false, "５"); ///No Translate
            AddKeepFont(false, "６"); ///No Translate
            AddKeepFont(false, "７"); ///No Translate
            AddKeepFont(false, "８"); ///No Translate
            AddKeepFont(false, "９"); ///No Translate
            AddKeepFont(false, 0x7a81); //ハート ///No Translate
            AddKeepFont(true, 0x7a81); //ハート  ///No Translate
            AddKeepFont(true, "０", 0x593ECC); ///No Translate
            AddKeepFont(true, "１", 0x593ED0); ///No Translate
            AddKeepFont(true, "２", 0x593ED4); ///No Translate
            AddKeepFont(true, "３", 0x593ED8); ///No Translate
            AddKeepFont(true, "４", 0x593EDC); ///No Translate
            AddKeepFont(true, "５", 0x593EE0); ///No Translate
            AddKeepFont(true, "６", 0x593EE4); ///No Translate
            AddKeepFont(true, "７", 0x593EE8); ///No Translate
            AddKeepFont(true, "８", 0x593EEC); ///No Translate
            AddKeepFont(true, "９", 0x593EF0); ///No Translate
            AddKeepFont(true, "ⅹ", 0x593EF4); ///No Translate
            AddKeepFont(true, "ⅰ", 0x593EF8); ///No Translate
            AddKeepFont(true, "ⅱ", 0x593EFC); ///No Translate
            AddKeepFont(true, "ⅲ", 0x593F00); ///No Translate
            AddKeepFont(true, "ⅳ", 0x593F04); ///No Translate
            AddKeepFont(true, "ⅴ", 0x593F08); ///No Translate
            AddKeepFont(true, "ⅵ", 0x593F0C); ///No Translate
            AddKeepFont(true, "ⅶ", 0x593F10); ///No Translate
            AddKeepFont(true, "ⅷ", 0x593F14); ///No Translate
            AddKeepFont(true, "ⅸ", 0x593F18); ///No Translate
            AddKeepFont(true, "ー", 0x593F1C); ///No Translate
            AddKeepFont(true, "＋", 0x593F20); ///No Translate
            AddKeepFont(true, "／", 0x593F24); ///No Translate
            AddKeepFont(true, "～", 0x593F28); ///No Translate
            AddKeepFont(true, "Ｓ", 0x593F2C); ///No Translate
            AddKeepFont(true, "Ａ", 0x593F30); ///No Translate
            AddKeepFont(true, "Ｂ", 0x593F34); ///No Translate
            AddKeepFont(true, "Ｃ", 0x593F38); ///No Translate
            AddKeepFont(true, "Ｄ", 0x593F3C); ///No Translate
            AddKeepFont(true, "Ｅ", 0x593F40); ///No Translate
            AddKeepFont(true, "Ｇ", 0x593F44); ///No Translate
            AddKeepFont(true, "ε", 0x593F48); ///No Translate
            AddKeepFont(true, "：", 0x593F4C); ///No Translate
            AddKeepFont(true, "．", 0x593F50); ///No Translate
            AddKeepFont(true, "Ｈ", 0x593F54); ///No Translate
            AddKeepFont(true, "Ｐ", 0x593F58); ///No Translate
            AddKeepFont(true, "＃", 0x593F5C); ///No Translate
            AddKeepFont(true, "＊", 0x593F60); ///No Translate
            AddKeepFont(true, "→", 0x593F64); ///No Translate
            AddKeepFont(true, "⊆", 0x593F68); ///No Translate
            AddKeepFont(true, "⊇", 0x593F6C); ///No Translate
            AddKeepFont(true, "％", 0x593F70); ///No Translate
        }
        void WriteBackFontKF(KeepFont kf, RecycleAddress ra)
        {
            uint topaddress = FontForm.GetFontPointer(kf.IsItemFont);

            uint prevaddr;
            uint fontaddr = FontForm.FindFontData(topaddress, kf.MojiCode, out prevaddr, this.PriorityCode);
            if (fontaddr != U.NOT_FOUND)
            {//知ってるらしい
                return;
            }
            if (prevaddr == U.NOT_FOUND)
            {//追加不可能
                return;
            }

            byte[] newFontData = FontForm.MakeNewFontData(kf.MojiCode
                , kf.Width
                , kf.Data
                , Program.ROM
                , this.PriorityCode);

            U.write_u32(newFontData, 0, 0);   //NULL リストの末尾に追加するので.

            uint newaddr = ra.Write(newFontData, this.UndoData);
            if (newaddr == U.NOT_FOUND)
            {//エラー
                return;
            }

            //ひとつ前のフォントリストのポインタを、現在追加した最後尾にすげかえる.
            Program.ROM.write_u32(prevaddr + 0, U.toPointer(newaddr), this.UndoData);

            if (kf.rewitePointer != U.NOT_FOUND)
            {
                Program.ROM.write_u32(kf.rewitePointer, U.toPointer(newaddr), this.UndoData);
            }
        }

        public void WriteBackFont(RecycleAddress ra)
        {
            foreach (KeepFont kf in KeepFontList)
            {
                WriteBackFontKF(kf, ra);
            }
        }
    }
}
