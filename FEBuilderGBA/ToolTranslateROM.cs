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
    public partial class ToolTranslateROM
    {
        public ToolTranslateROM()
        {
            this.MaxTextCount = TextForm.GetDataCount();
            this.TextBaseAddress = TextForm.GetBaseAddress();
        }
        uint MaxTextCount;
        uint TextBaseAddress;
        
        void WriteText(uint id,string text)
        {
            //最後の改行の削除.
            if (text.Length < 2)
            {
                return;
            }
            string writetext = U.substr(text, 0, text.Length - 2);

            if (id < this.MaxTextCount)
            {
                TextForm.WriteText(this.TextBaseAddress
                    , this.MaxTextCount
                    , id
                    , writetext);
                return;
            }
            if (U.isSafetyPointer(id))
            {
                uint p_text_pointer = U.toOffset(id);
                uint text_pointer = Program.ROM.u32(p_text_pointer);
                if (!U.isSafetyPointer(text_pointer))
                {
                    Log.Error("ポインタではありません", id.ToString(), text);
                    return;
                }
                uint new_textpointer = CStringForm.WriteCString(text_pointer, writetext);
                if (new_textpointer == U.NOT_FOUND)
                {
                    return;
                }

                Program.Undo.Push("CSTRING_P",p_text_pointer,4);
                Program.ROM.write_p32(p_text_pointer, new_textpointer);
                return;
            }

            Log.Error("不明な文字列", id.ToString(), text);
            return;
        }
        void ApplyAntiHuffmanPatch()
        {
            HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.Anti_Huffman_By_Translate);
        }
        void ApplyDrawFontPatch()
        {
            HowDoYouLikePatch2Form.CheckAndShowPopupDialog(HowDoYouLikePatch2Form.TYPE.DrawFont_By_Translate);
        }

        public void CheckTextImportPatch(bool checkDrawFontPatch)
        {
            ApplyAntiHuffmanPatch();
            if (checkDrawFontPatch)
            {
                ApplyDrawFontPatch();
            }
        }

        public void ImportAllText(Form self)
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
                return ;
            }
            if (!U.CanReadFileRetry(open))
            {
                return ;
            }

            Program.LastSelectedFilename.Save(self, "", open);
            string filename = open.FileNames[0];

            ImportAllText(self, filename);
        }

        public void ImportAllText(Form self,string filename)
        {
            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
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

                    if (line[0] != '[' && line[line.Length - 1] != ']')
                    {
                        text += line + "\r\n";
                        continue;
                    }


                    //次の数字があったので、現在のテキストの書き込み.
                    pleaseWait.DoEvents("Write:" + U.To0xHexString(id));
                    WriteText(id, text);

                    //次のテキスト
                    if (line[0] == '[' && line[line.Length - 1] == ']')
                    {
                        id = U.atoh(U.substr(line, 1));
                        text = "";
                    }
                }

                //最後のデータ
                WriteText(id, text);
            }
        }

        public void ExportallText(Form self)
        {
            ExportallText(self, false, "", "", "", "" , false);
        }
        public void ExportallText(Form self
            , bool useAutoTranslateCheckBox
            , string Translate_from
            , string Translate_to
            , string TranslateFormROMFilename
            , string TranslateToROMFilename
            , bool useGoolgeTranslate)
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

            ExportallText(self,save.FileNames[0], from, to, fromrom, torom, useGoolgeTranslate);

            U.SelectFileByExplorer(save.FileNames[0]);
        }

        void ExportText(StreamWriter writer,uint id, string text, string tralnslate_from, string tralnslate_to,Dictionary<string,string> transDic,bool useGoolgeTranslate)
        {
            writer.Write("[" + U.ToHexString(id) + "]\r\n");
            if (tralnslate_from == "" || tralnslate_to == "")
            {
                writer.Write(text + "\r\n");
            }
            else
            {
                string translatetext = TranslateTextUtil.TranslateText(id
                    , text
                    , tralnslate_from
                    , tralnslate_to
                    , transDic
                    , useGoolgeTranslate);
                writer.Write(translatetext + "\r\n");
            }
        }

        public void ExportallText(Form self
            , string writeTextFileName
            , string tralnslate_from, string tralnslate_to
            , string rom_from, string rom_to
            , bool useGoolgeTranslate)
        {
            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
                FETextDecode decode = new FETextDecode();

                //よくある定型文の翻訳辞書
                Dictionary<string, string> transDic = new Dictionary<string, string>();
                if (rom_from != "" && rom_to != "")
                {
                    transDic = TranslateTextUtil.LoadTranslateDic(tralnslate_from, tralnslate_to, rom_from, rom_to);
                }

                using (StreamWriter writer = new StreamWriter(writeTextFileName))
                {
                    //テキスト
                    {
                        List<U.AddrResult> list = TextForm.MakeItemList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            string text = decode.Decode((uint)i);

                            pleaseWait.DoEvents("Text:" + U.To0xHexString((uint)i));
                            ExportText(writer, (uint)i, text, tralnslate_from, tralnslate_to, transDic, useGoolgeTranslate);
                        }
                    }

                    //メニュー1
                    if (Program.ROM.RomInfo.is_multibyte())
                    {
                        List<U.AddrResult> menuDefineList = MenuDefinitionForm.MakeList();
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
                                ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, useGoolgeTranslate);
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
                            uint text_pointer = list[i].addr + 0;
                            uint textid = Program.ROM.u32(text_pointer);
                            string str = FETextDecode.Direct(textid);
                            if (str.Trim() == "")
                            {
                                continue;
                            }

                            pleaseWait.DoEvents("Terrain:" + U.To0xHexString(textid));
                            ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, useGoolgeTranslate);
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
                            uint text_pointer = list[i].addr + 12;
                            uint textid = Program.ROM.u32(text_pointer);
                            string str = FETextDecode.Direct(textid);
                            if (str.Trim() == "")
                            {
                                continue;
                            }

                            pleaseWait.DoEvents("SoundRoom:" + U.To0xHexString(textid));
                            ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, useGoolgeTranslate);
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
                            ExportText(writer, U.toPointer(text_pointer), str, tralnslate_from, tralnslate_to, transDic, useGoolgeTranslate);
                        }
                    }
                }
            }
        }
        public void ChangeMainMenuWidth(string to)
        {
            uint length = Program.ROM.u8( Program.ROM.RomInfo.main_menu_width_address() );
            if (to == "ja" || to == "zh")
            {
                if (length <= 6)
                {
                    Program.ROM.write_u8( Program.ROM.RomInfo.main_menu_width_address() , 6);
                }
            }
            else
            {
                if (length <= 8)
                {
                    Program.ROM.write_u8( Program.ROM.RomInfo.main_menu_width_address() , 8);
                }
            }
        }
        public void ChangeStatusScreenSkill(string to)
        {
            uint status = Program.ROM.p32(Program.ROM.RomInfo.status_param1_pointer());
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
    }
}
