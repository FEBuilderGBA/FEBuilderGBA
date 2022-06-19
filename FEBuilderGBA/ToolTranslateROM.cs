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

namespace FEBuilderGBA
{
    public partial class ToolTranslateROM
    {
        public ToolTranslateROM()
        {
            this.MaxTextCount = TextForm.GetDataCount();
            this.TextBaseAddress = TextForm.GetBaseAddress();
            this.TextID0Addr = Program.ROM.p32(this.TextBaseAddress);
        }
        uint MaxTextCount;
        uint TextBaseAddress;
        bool UseGoolgeTranslate;
        FETextDecode TextDecode = new FETextDecode();
        uint TextID0Addr;
        bool IsWipeJPFont;

        void AddRecycle(uint id, List<Address> recycle)
        {
            //無効なID
            if (id <= 0)
            {
                return;
            }
            if (id < this.MaxTextCount)
            {
                uint addr = this.TextBaseAddress + (id * 4);
                uint paddr = Program.ROM.u32(addr);
                if (TextForm.Is_RAMPointerArea(paddr))
                {
                    return;
                }
                uint data_addr;

                int length;
                if (FETextEncode.IsUnHuffmanPatchPointer(paddr))
                {//un-huffman patch?
                    uint unhuffman_addr = U.toOffset(FETextEncode.ConvertUnHuffmanPatchToPointer(paddr));
                    data_addr = unhuffman_addr;
                    TextDecode.UnHffmanPatchDecode(unhuffman_addr, out length);
                }
                else if (U.isPointer(paddr))
                {
                    data_addr = U.toOffset(paddr);
                    TextDecode.huffman_decode(data_addr, out length);
                }
                else
                {
                    return;
                }

                if (length <= 0)
                {
                    return;
                }
                if (data_addr == this.TextID0Addr)
                {
                    return;
                }
                FEBuilderGBA.Address.AddAddress(recycle
                    , data_addr
                    , (uint)length
                    , U.NOT_FOUND
                    , "text " + U.ToHexString(id)
                    , FEBuilderGBA.Address.DataTypeEnum.BIN);
            }
//CStringに対しては、リサイクルバッファを利用する方式を利用しない
//なぜなら、FE6には大量のポインタ参照があるためです。
//容量も大きくないので、メリットよりデメリットの方が上回る
//            else if (U.isSafetyPointer(id))
//            {
//                uint p = U.toOffset(id);
//                FEBuilderGBA.Address.AddCString(recycle, p);
//            }
        }

        void WriteText(uint id, string text,RecycleAddress ra, Undo.UndoData undodata)
        {
            //無効なID
            if (id <= 0)
            {
                return;
            }
            //最後の改行の削除.
            if (text.Length < 2)
            {
                return;
            }
            string writetext = U.substr(text, 0, text.Length - 2);
            writetext = TextForm.ConvertFEditorToEscape(writetext);

            if (id < this.MaxTextCount)
            {
                WriteTextUnHffman(id, writetext, ra, undodata);
            }
            else if (U.isSafetyPointer(id))
            {
                WriteCString(id, writetext, undodata);
            }
            else
            {
                Log.Error("不明な文字列", id.ToString(), writetext);
                return;
            }
        }
        void WriteTextUnHffman(uint id, string text, RecycleAddress ra, Undo.UndoData undodata)
        {
            uint addr = this.TextBaseAddress + (id * 4);
            uint paddr = Program.ROM.u32(addr);
            if (TextForm.Is_RAMPointerArea(paddr))
            {//iw-ram / ew-ram にデータをおいている人がいるらしい
                return ;
            }

            byte[] encode;
            Program.FETextEncoder.UnHuffmanEncode(text, out encode);

            string undoname = "Text:" + U.ToHexString(id);
            uint newaddr = ra.Write(encode, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                return;
            }
            newaddr = U.toPointer(newaddr);
            newaddr = FETextEncode.ConvertPointerToUnHuffmanPatchPointer(newaddr);
            Program.ROM.write_u32(addr, newaddr, undodata);
        }

        void WriteCString(uint pointer, string text, Undo.UndoData undodata)
        {
            Debug.Assert(U.isSafetyPointer(pointer));

            byte[] stringbyte = Program.SystemTextEncoder.Encode(text);
            stringbyte = U.ArrayAppend(stringbyte, new byte[] { 0x00 });

            string undoname = "CString:" + U.ToHexString(pointer);
            InputFormRef.WriteBinaryDataPointer(null
                , pointer
                , stringbyte
                , PatchUtil.get_data_pos_callback
                , undodata
            );
        }
        void ApplyAntiHuffmanPatch()
        {
            HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.Anti_Huffman_By_Translate);
        }
        void ApplyDrawFontPatch()
        {
            HowDoYouLikePatch2Form.CheckAndShowPopupDialog(HowDoYouLikePatch2Form.TYPE.DrawFont_By_Translate);
        }
        void ApplyStatusToLocalizationPatch()
        {
            HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.StatusToLocalization);
        }

        public void CheckTextImportPatch(bool checkDrawFontPatch)
        {
            ApplyAntiHuffmanPatch();
            if (checkDrawFontPatch)
            {
                ApplyDrawFontPatch();
            }
            ApplyStatusToLocalizationPatch();
        }
        public void SetWipeJPFont(bool wipeJPFont)
        {
            this.IsWipeJPFont = wipeJPFont;
        }


        //パッチの適用
        public void ApplyTranslatePatch(string to)
        {
            this.CheckTextImportPatch(true);
            //メニューのサイズを調整する
            this.ChangeMainMenuWidth(to);
            this.ChangeStatusScreenSkill(to);
        }

        public bool ImportAllText(Form self)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(self, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return false;
            }
            if (!U.CanReadFileRetry(open))
            {
                return false;
            }

            Program.LastSelectedFilename.Save(self, "", open);
            string filename = open.FileNames[0];

            ImportAllText(self, filename);
            return true;
        }
        public void ImportAllText(Form self,string filename)
        {
            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
                Undo.UndoData undodata = Program.Undo.NewUndoData("ImportAllText" + Path.GetFileName(filename)　);

                uint id = U.NOT_FOUND;
                string[] lines = File.ReadAllLines(filename);

                //上書きするテキスト領域を再利用リストに突っ込む
                List<Address> recycle = new List<FEBuilderGBA.Address>();
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (U.IsCommentSlashOnly(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    line = U.ClipComment(line);
                    if (line.Length <= 0)
                    {
                        continue;
                    }

                    if (!TranslateTextUtil.IsTextIDCode(line))
                    {
                        continue;
                    }

                    AddRecycle(id, recycle);

                    //次のテキスト
                    id = U.atoh(U.substr(line, 1));
                }

                //日本語フォントを上書きしてもいい場合
                if (this.IsWipeJPFont)
                {
                    ToolTranslateROMWipeJPFont jpfont = new ToolTranslateROMWipeJPFont(undodata);
                    jpfont.AddJPFonts(recycle);
                }

                RecycleAddress ra = new RecycleAddress(recycle);

                id = U.NOT_FOUND;
                string text = "";
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (U.IsCommentSlashOnly(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    line = U.ClipComment(line);
                    if (line.Length <= 0)
                    {
                        continue;
                    }

                    if (! TranslateTextUtil.IsTextIDCode(line))
                    {
                        text += line + "\r\n";
                        continue;
                    }

                    //次の数字があったので、現在のテキストの書き込み.
                    pleaseWait.DoEvents("Write:" + U.To0xHexString(id));
                    WriteText(id, text, ra, undodata);

                    //次のテキスト
                    id = U.atoh(U.substr(line, 1));
                    text = "";
                }

                //最後のデータ
                WriteText(id, text, ra, undodata);
                ra.BlackOut(undodata);

                Program.Undo.Push(undodata);
            }
        }


        public void ExportallText(Form self)
        {
            ExportallText(self, false, "", "", "", "" , false, true , false);
        }
        public void ExportallText(Form self
            , bool useAutoTranslateCheckBox
            , string Translate_from
            , string Translate_to
            , string TranslateFormROMFilename
            , string TranslateToROMFilename
            , bool useGoolgeTranslate
            , bool modifiedTextOnly
            , bool isOneLiner
            )
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(self, "", save);

            string from = "";
            string to = "";
            string fromrom = "";
            string torom = "";

            if (useAutoTranslateCheckBox)
            {
                //翻訳言語
                from = U.InnerSplit(Translate_from, "=", 0);
                to = U.InnerSplit(Translate_to, "=", 0);
                if (from == to)
                {
                    return;
                }

                fromrom = TranslateFormROMFilename;
                torom = TranslateToROMFilename;
                if (! File.Exists(fromrom))
                {
                    return;
                }
                if (!File.Exists(torom))
                {
                    return;
                }
            }

            this.UseGoolgeTranslate = useGoolgeTranslate;
            ExportallText(self, save.FileNames[0], from, to, fromrom, torom, modifiedTextOnly, isOneLiner);

            U.SelectFileByExplorer(save.FileNames[0]);
        }

        void ExportText(StreamWriter writer,uint id, string text, string tralnslate_from, string tralnslate_to,Dictionary<string,string> transDic, bool isModifiedTextOnly, bool isOneLiner)
        {
            string translatetext;
            if (tralnslate_from == "" || tralnslate_to == "")
            {
                translatetext = text; //変換不能
            }
            else
            {
                try
                {
                    translatetext = TranslateTextUtil.TranslateText(id
                        , text
                        , tralnslate_from
                        , tralnslate_to
                        , transDic
                        , this.UseGoolgeTranslate
                        , isModifiedTextOnly);
                    if (translatetext == "[@SKIP]")
                    {
                        return;
                    }
                }
                catch (System.Net.WebException ee)
                {
                    R.ShowStopError("Google翻訳がエラーを返しました。\r\n翻訳リクエストの送りすぎです。\r\n以降は、Google翻訳を無効にして処理を実装します。\r\n\r\n{0}", ee.ToString());
                    this.UseGoolgeTranslate = false;
                    translatetext = text;
                }
            }

            if ( Program.ROM.RomInfo.is_multibyte )
            {//マルチバイトROMならば、001fパディングを消す.
                translatetext = translatetext.Replace("@001F", "");
            }

            //エスケープシーケンスの変換
            translatetext = TextForm.ConvertEscapeText(translatetext);

            if (isOneLiner)
            {//ワンライナー
                writer.Write(translatetext.Replace("\r\n", "\\r\\n")+"\r\n");
                return;
            }

            writer.Write("[" + U.ToHexString(id) + "]\r\n");
            writer.Write(translatetext + "\r\n");
        }

        Dictionary<int, bool> ExportFilterArray = null;
        public void InitExportFilter(uint filter)
        {
            List<UseValsID> list = new List<UseValsID>();
            if (filter == 1)
            {//ユニット関係のみ
                UnitForm.MakeVarsIDArray(list);
            }
            else if (filter == 2)
            {//クラス関係のみ
                ClassForm.MakeVarsIDArray(list);
            }
            else if (filter == 3)
            {//アイテム関係のみ
                ItemForm.MakeVarsIDArray(list);
            }
            else if (filter == 4)
            {//サウンドルーム関係のみ
                if (Program.ROM.RomInfo.version == 6)
                {
                    SoundRoomFE6Form.MakeVarsIDArray(list);
                }
                else
                {
                    SoundRoomForm.MakeVarsIDArray(list);
                }
            }
            else if (filter == 5)
            {//支援会話関係
                SupportTalkForm.MakeVarsIDArray(list);
            }
            else if (filter == 6)
            {//スキル関係
                if (Program.ROM.RomInfo.is_multibyte)
                {
                    SkillConfigFE8NSkillForm.MakeVarsIDArray(list);
                    SkillConfigFE8NVer2SkillForm.MakeVarsIDArray(list);
                }
                else
                {
                    SkillConfigSkillSystemForm.MakeVarsIDArray(list);
                }
            }
            else
            {//all
                ExportFilterArray = null;
                return;
            }

            ExportFilterArray = new Dictionary<int,bool>();
            foreach (UseValsID val in list)
            {
                ExportFilterArray[(int)val.ID] = true;
            }
        }

        public void ExportallText(Form self
            , string writeTextFileName
            , string tralnslate_from, string tralnslate_to
            , string rom_from, string rom_to
            , bool isModifiedTextOnly
            , bool isOneLiner
            )
        {
            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
                FETextDecode decode = new FETextDecode();

                Dictionary<string, string> transDic = TranslateTextUtil.MakeFixedDic(tralnslate_from, tralnslate_to , rom_from , rom_to);
                if (ExportFilterArray != null)
                {
                    using (StreamWriter writer = new StreamWriter(writeTextFileName))
                    {
                        List<U.AddrResult> list = TextForm.MakeItemList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (! ExportFilterArray.ContainsKey(i))
                            {
                                continue;
                            }
                            string text = decode.Decode((uint)i);

                            pleaseWait.DoEvents("Text:" + U.To0xHexString((uint)i));
                            ExportText(writer, (uint)i, text, tralnslate_from, tralnslate_to, transDic, isModifiedTextOnly, isOneLiner);
                        }
                    }
                    return;
                }

                using (StreamWriter writer = new StreamWriter(writeTextFileName))
                {

                    //テキスト
                    {
                        List<U.AddrResult> list = TextForm.MakeItemList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (ExportFilterArray != null && ExportFilterArray[i] != false)
                            {
                                continue;
                            }
                            string text = decode.Decode((uint)i);

                            pleaseWait.DoEvents("Text:" + U.To0xHexString((uint)i));
                            ExportText(writer, (uint)i, text, tralnslate_from, tralnslate_to, transDic,  isModifiedTextOnly, isOneLiner);
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
                                uint text_pointer = list[i].addr + 0;
                                uint textid = Program.ROM.u32(text_pointer);
                                string str = FETextDecode.Direct(textid);
                                if (str.Trim() == "")
                                {
                                    continue;
                                }

                                pleaseWait.DoEvents("Menu:" + U.To0xHexString(textid));
                                ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, isModifiedTextOnly, isOneLiner);
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
                            uint text_pointer = list[i].addr + 0;
                            uint textid = Program.ROM.u32(text_pointer);
                            string str = FETextDecode.Direct(textid);
                            if (str.Trim() == "")
                            {
                                continue;
                            }

                            pleaseWait.DoEvents("Terrain:" + U.To0xHexString(textid));
                            ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, isModifiedTextOnly, isOneLiner);
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
                            uint text_pointer = list[i].addr + 12;
                            uint textid = Program.ROM.u32(text_pointer);
                            string str = FETextDecode.Direct(textid);
                            if (str.Trim() == "")
                            {
                                continue;
                            }

                            pleaseWait.DoEvents("SoundRoom:" + U.To0xHexString(textid));
                            ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, isModifiedTextOnly, isOneLiner);
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
                            uint text_pointer = list[i].addr + 0;
                            uint p_str = Program.ROM.p32(text_pointer);
                            string str = Program.ROM.getString(p_str);
                            if (str.Trim() == "")
                            {
                                continue;
                            }

                            pleaseWait.DoEvents("Other:" + U.To0xHexString(p_str));
                            ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, isModifiedTextOnly, isOneLiner);
                        }
                    }
                }
            }
        }
        public void ChangeMainMenuWidth(string to)
        {
            uint length = Program.ROM.u8( Program.ROM.RomInfo.main_menu_width_address );
            if (to == "ja" || to == "zh")
            {
                if (length <= 6)
                {
                    Program.ROM.write_u8( Program.ROM.RomInfo.main_menu_width_address , 6);
                }
            }
            else
            {
                if (length <= 8)
                {
                    Program.ROM.write_u8( Program.ROM.RomInfo.main_menu_width_address , 8);
                }
            }
        }
        public void ChangeStatusScreenSkill(string to)
        {
            uint status = Program.ROM.p32(Program.ROM.RomInfo.status_param1_pointer);
            if (! U.isSafetyOffset(status))
            {
                return ;
            }

            uint length = Program.ROM.u8(status + 0x09);
            if (to == "ja" || to == "zh")
            {
            }
            else
            {
                if (length == 4)
                {
                    Program.ROM.write_u8(status + 0x09, 0);
                }
            }
            
        }

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
            public void AddJPFonts(List<Address> recycle)
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
                uint textFontEnd   = 0x5B8CDC;
                SerchCustomFonts(false, textFontEnd, recycle);
                Address.AddAddress(recycle
                    ,textFontStart
                    ,textFontEnd - textFontStart
                    , U.NOT_FOUND
                    , "TextFont Wipe"
                    , Address.DataTypeEnum.BIN
                    );
                uint itemFontStart = 0x579CCC;
                uint itemFontEnd = 0x593ECC;
                SerchCustomFonts(true, itemFontEnd, recycle);
                Address.AddAddress(recycle
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

                //書き戻す
                WriteBackFont();
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
                AddKeepFont(false, "♥"); ///No Translate
                AddKeepFont(true, "♥"); ///No Translate
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
            void WriteBackFontKF(KeepFont kf)
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

                uint newaddr = InputFormRef.AppendBinaryData(newFontData, this.UndoData);
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

            void WriteBackFont()
            {
                foreach (KeepFont kf in KeepFontList)
                {
                    WriteBackFontKF(kf);
                }
            }
        }
    }
}
