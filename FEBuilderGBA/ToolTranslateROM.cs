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
        RecycleAddress Recycle = new RecycleAddress();

        public void BlackOut(Undo.UndoData undodata)
        {
            this.Recycle.BlackOut(undodata);
        }

        void AddRecycle(uint id, List<Address> list)
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
                    if (length <= 0)
                    {
                        return;
                    }
                }
                else if (U.isPointer(paddr))
                {
                    data_addr = U.toOffset(paddr);
                    TextDecode.huffman_decode(data_addr, out length);
                    if (length <= 1)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (data_addr == this.TextID0Addr)
                {
                    return;
                }
                FEBuilderGBA.Address.AddAddress(list
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
//                FEBuilderGBA.Address.AddCString(list, p);
//            }
        }

        void WriteText(uint id, string text, Undo.UndoData undodata)
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
                WriteTextUnHffman(id, writetext, undodata);
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
        void WriteTextUnHffman(uint id, string text, Undo.UndoData undodata)
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
            uint newaddr = this.Recycle.Write(encode, undodata);
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

        public void CheckTextImportPatch(bool checkDrawFontPatch, bool checkLocalizationPatch)
        {
            ApplyAntiHuffmanPatch();
            if (checkDrawFontPatch)
            {
                ApplyDrawFontPatch();
            }
            if (checkLocalizationPatch)
            {
                ApplyStatusToLocalizationPatch();
            }
        }


        //パッチの適用
        public void ApplyTranslatePatch(string to)
        {
            this.CheckTextImportPatch(true, true);
            //メニューのサイズを調整する
            this.ChangeMainMenuWidth(to);
            this.ChangeStatusScreenSkill(to);
        }

        public bool ImportAllText(Form self, Undo.UndoData undodata)
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

            ImportAllText(self, filename, false, undodata);
            return true;
        }

        //ハフマン符号化の再構築
        public void RebuildTextCharCode(Dictionary<uint, string> textDataDic, Undo.UndoData undodata)
        {
            ToolTextCharRecreate recreater = new ToolTextCharRecreate();
            foreach(var pair in textDataDic)
            {
                recreater.AddEN(pair.Value);
            }
            TextCharCodeForm.RebuildAllData(recreater, undodata);
        }

        //日本語フォントを上書きしてもいい場合
        public void WipeJPFont(Form self, Undo.UndoData undodata)
        {
            List<Address> list = new List<FEBuilderGBA.Address>();

            ToolTranslateROMWipeJPFont jpfont = new ToolTranslateROMWipeJPFont(undodata);
            jpfont.AddJPFonts(list);

            this.Recycle.AddRecycle(list);
            this.Recycle.RecycleOptimize();

            jpfont.WriteBackFont(this.Recycle);
        }
        public void WipeJPTitle(Undo.UndoData undodata)
        {
            List<Address> list = new List<FEBuilderGBA.Address>();

            ToolTranslateROMWipeJPChapterName jpChapter = new ToolTranslateROMWipeJPChapterName(undodata);
            jpChapter.Wipe(list);

            this.Recycle.AddRecycle(list);
            this.Recycle.RecycleOptimize();
        }
        public void WipeJPClassReelFont(Undo.UndoData undodata)
        {
            List<Address> list = new List<FEBuilderGBA.Address>();

            ToolTranslateROMWipeJPClassReelFont jpClassReel = new ToolTranslateROMWipeJPClassReelFont(undodata);
            jpClassReel.Wipe(list);

            this.Recycle.AddRecycle(list);
            this.Recycle.RecycleOptimize();
        }

        public void ImportAllText(Form self, string filename, bool IsRebuildHuffmanCharCode, Undo.UndoData undodata)
        {
            ToolTextCharRecreate charRecreate = new ToolTextCharRecreate();

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
                U.ReleaseMemory(pleaseWait);
                Dictionary<uint, string> textDataDic = ReadTextDataDic(filename);

                //上書きするテキスト領域を再利用リストに突っ込む
                List<Address> list = new List<FEBuilderGBA.Address>();
                foreach (var pair in textDataDic)
                {
                    AddRecycle(pair.Key, list);
                }
                this.Recycle.AddRecycle(list);
                this.Recycle.RecycleOptimize();

                //ハフマンコードの再構築をするかどうか
                if (IsRebuildHuffmanCharCode)
                {
//                    RebuildTextCharCode(textDataDic, undodata);
                }

                foreach (var pair in textDataDic)
                {
                    AddRecycle(pair.Key, list);
                    //次の数字があったので、現在のテキストの書き込み.
                    pleaseWait.DoEvents("Write:" + U.To0xHexString(pair.Key));
                    WriteText(pair.Key, pair.Value, undodata);
                }
            }
        }
        Dictionary<uint, string> ReadTextDataDic(string filename)
        {
            Dictionary<uint, string> textDataDic = new Dictionary<uint,string>();

            uint id = U.NOT_FOUND;
            string[] lines = File.ReadAllLines(filename);
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

                if (id != U.NOT_FOUND)
                {
                    //次の数字があったので、現在のテキストの書き込み.
                    textDataDic[id] = text;
                }

                //次のテキスト
                id = U.atoh(U.substr(line, 1));
                text = "";
            }

            //最後のデータ
            textDataDic[id] = text;
            return textDataDic;
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
        public void ImportFont(Form self, string FontROMTextBox, bool FontAutoGenelateCheckBox, Font ttf, Undo.UndoData undodata)
        {
            ToolTranslateROMFont transFont = new ToolTranslateROMFont();
            transFont.ImportFont(self, FontROMTextBox, FontAutoGenelateCheckBox, ttf, this.Recycle ,undodata);
        }

    }
}
