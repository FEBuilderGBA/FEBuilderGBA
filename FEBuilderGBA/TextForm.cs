using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class TextForm : Form
    {
        public class TextBlock
        {
            public string SrcText;
            public uint Code1;
            public uint Code2;
            public uint Code3;

            public string Error;
            public bool isJump;

            public uint[] Units;

            public TextBlock()
            {
                this.SrcText = "";
                this.Error = "";
                this.Units = new uint[8];
            }
        };
        List<TextBlock> SimpleList = new List<TextBlock>();

        public TextForm()
        {
            InitializeComponent();
            HideFloatingControlpanel();
            this.AddressList.OwnerDraw(DrawAddressListTextDelay, DrawMode.OwnerDrawFixed, true);

            this.TextList.OwnerDraw(Draw, DrawMode.OwnerDrawVariable, false);

            this.InputFormRef = Init(this);
            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            UpdateDataCountCache(this.InputFormRef);
            TranslateLanguageAutoSelect();

            this.RefListBox.ItemHeight = (int)(this.RefListBox.Font.Height * 2.4);
            this.RefListBox.OwnerDraw(InputFormRef.DrawRefTextList, DrawMode.OwnerDrawFixed, false);

            InitRichEditEx(this.TextArea);
            InitRichEditEx(this.TextListSpTextTextBox);
            InitRichEditEx(this.TextListSpSerifuTextBox);

            MakeEditListboxContextMenuText(this.TextList, this.TextList_KeyDown);
            U.SetIcon(Export, Properties.Resources.icon_arrow);
            U.SetIcon(Import, Properties.Resources.icon_upload);

            X_ExportFilterCombo.SelectedIndex = 0;

            InputFormRef.markupJumpLabel(this.TextListSpShowCharLabel);
        }
        void InitRichEditEx(RichTextBoxEx editor)
        {
            editor.AppendContentMenuBar();
            editor.AppendContentMenu(R._("読み上げ"), new EventHandler(OptionTextToSpeechByRichText));
            if (PatchUtil.SearchIrregularFontPatch() == PatchUtil.IrregularFont_enum.NarrowFont)
            {
                editor.AppendContentMenuBar();
                editor.AppendContentMenu(R._("NarrowFont変換"), new EventHandler(OptionConvertAnrrowFont));
            }
            editor.AppendContentMenuBar();
            editor.AppendContentMenu(R._("エスケープコード(&N)"), new EventHandler(SelectEscapeText));
            editor.CutCallback += OnPasteOrUndoOrCutText;
            editor.UndoCallback += OnPasteOrUndoOrCutText;
            editor.PasteCallback += OnPasteOrUndoOrCutText;
        }

        public static void MakeEditListboxContextMenuText(ListBox listbox, KeyEventHandler keydownfunc)
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;
            menuItem = new MenuItem(R._("元に戻す(&Z)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.Z));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("削除(DEL)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Delete));
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("コピー(&C)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.C));
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("貼り付け(&V)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.V));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("読み上げ"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.Alt | Keys.O));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("編集画面を出す(ENTER)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.Enter));
            contextMenu.MenuItems.Add(menuItem);

            listbox.ContextMenu = contextMenu;
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            UpdateDataCountCache();
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(TextForm self)
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.text_pointer
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint p = Program.ROM.u32(addr + 0);
                    if (U.isPointer(p))
                    {
                        return true;
                    }
                    if (FETextEncode.IsUnHuffmanPatchPointer(p))
                    {//海外改造によくある unHuffman patch
                        return true;
                    }

                    if (Is_RAMPointerArea(p))
                    {//ramにデータをおいている人がいるらしい
                        return true;
                    }
                    return false;
                }
                , (int i, uint addr) =>
                {
                    uint text_addr = Program.ROM.u32(addr);
                    return (i).ToString("X04");
                }
                );

            uint text_base = Program.ROM.p32(Program.ROM.RomInfo.text_pointer);
            if (!U.isSafetyOffset(text_base))
            {//テキストポインタを壊しているので復帰する.
                text_base = Program.ROM.RomInfo.text_recover_address;
                ifr.ReInit(text_base);
            }
            return ifr;
        }
        private void TextForm_Load(object sender, EventArgs e)
        {
        }

        //書き込んでいない途中のテキストがありますか？
        bool hasTextOnTheWay()
        {
            if (! InputFormRef.IsWriteButtonToYellow(this.AllWriteButton))
            {//書き込みボタンがハイライトされていない
                if (UpdateButton.Visible)
                {//セリフなどの変更が表示されている
                    if (!InputFormRef.IsWriteButtonToYellow(this.UpdateButton))
                    {//セリフの変更ボタンがハイライトされていない
                        return false;
                    }
                }
                else
                {//書き込みボタンがハイライトされていない
                    return false;
                }
            }

            if (this.PrevSelectTextID == U.NOT_FOUND)
            {
                return true;
            }
            DialogResult dr = R.ShowNoYes("書き込んでいないテキストがありますが、テキストを切り替えてもよろしいですか？");
            if (dr != System.Windows.Forms.DialogResult.No)
            {
                return false;
            }

            //変更キャンセル処理
            uint id = this.PrevSelectTextID;
            //キャンセル処理で再度 hasTextOnTheWay が呼ばれないように補正する
            this.PrevSelectTextID = U.NOT_FOUND; 

            U.SelectedIndexSafety(this.AddressList, id);

            this.PrevSelectTextID = id;
            return true;
        }
        //前回選択していた場所
        uint PrevSelectTextID = 0;

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hasTextOnTheWay())
            {
                return;
            }

            Control prevFocusedControl = this.ActiveControl;
            ClearUndoBuffer();
            HideFloatingControlpanel();

            uint id = (uint)(this.AddressList.SelectedIndex);
            this.PrevSelectTextID = id;
            UpdateTextArea(FETextDecode.Direct(id));

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);

            if (this.SimpleList.Count <= 1)
            {
                TextTabControl.SelectedTab = this.SrcTabPage;
            }
            else
            {
                TextTabControl.SelectedTab = this.EasyTabPage;
            }

            //必要ならばキーワードハイライトをする.
            KeywordHigtLightAuto();
            //参照数の更新.
            UpdateRef(id);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);

            this.ActiveControl = prevFocusedControl;
        }
        void UpdateTextArea(string text)
        {
            this.Cache_MainArea = text;
            UpdateBlockSize(Cache_MainArea);
            SetEditorText(this.TextArea, text);

            uint write_pointer = this.InputFormRef.BaseAddress + (this.InputFormRef.BlockSize * (uint)this.AddressList.SelectedIndex);
            uint write_addr = Program.ROM.p32(write_pointer);
            this.AddressPointer.Text = write_addr.ToString("X");

            UpdateTextList(text, out this.SimpleList, -1);

            UpdateDetailErrorMessageBox();
        }

        void UpdateDetailErrorMessageBox()
        {
            StringBuilder error = new StringBuilder();
            for (int i = 0; i < this.SimpleList.Count; i++)
            {
                TextBlock tb = this.SimpleList[i];
                if (tb.Error != "")
                {
                    error.AppendLine(tb.Error + "\r\n --> " + tb.SrcText + "\r\n");
                }
            }

            bool isSystemReserve = IsSystemReserve((uint)this.AddressList.SelectedIndex);
            if (error.Length == 0 && isSystemReserve && this.SimpleList.Count == 0)
            {//空いているけど予約されているので使わないでくださいという表示
                this.DetailErrorMessageBox.ErrorMessage = "";
                this.DetailErrorMessageBox.Text = R._("この領域は、システムを拡張するパッチ用の文字列領域として予約されています。\r\n利用しないでください。");
                this.DetailErrorMessageBox.Show();
            }
            else if (error.Length == 0)
            {
                this.DetailErrorMessageBox.Hide();
            }
            else
            {
                this.DetailErrorMessageBox.ErrorMessage = R.Error("以下のテキストにエラーがあります。\r\nエラーを修正して、「書き込み」ボタンを押してください。");
                this.DetailErrorMessageBox.Text = error.ToString();
                this.DetailErrorMessageBox.Show();
            }
        }

        //システム予約
        public static bool IsSystemReserve(uint textid)
        {
            if (Program.ROM.RomInfo.version == 8)
            {
                if (textid >= 0xE00 && textid <= 0xFFF)
                {
                    return true;
                }
            }
            else if (Program.ROM.RomInfo.version == 7)
            {
                if (textid >= 0x1E00 && textid <= 0x1FFF)
                {
                    return true;
                }
            }

            return false;
        }

        private static InputFormRef.ADDR_AND_LENGTH get_data_pos_callback(uint addr, bool useUnHuffmanPatch)
        {
            FETextDecode decoder = new FETextDecode();
            int datasize;
            string text;
            if (useUnHuffmanPatch)
            {
                text = decoder.UnHffmanPatchDecode(addr, out datasize);
            }
            else
            {
                text = decoder.huffman_decode(addr, out datasize);
            }
            InputFormRef.ADDR_AND_LENGTH aal = new InputFormRef.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = (uint)datasize;
            return aal;
        }

        public static bool Is_RAMPointerArea(uint addr)
        {
            return (U.is_03RAMPointer(addr)
                || FETextEncode.IsUnHuffmanPatch_IW_RAMPointer(addr)
                || U.is_02RAMPointer(addr)
                || FETextEncode.IsUnHuffmanPatch_EW_RAMPointer(addr)
                );
        }
        static bool Is_RAMPointerArea(
            uint baseaddress
            , uint id)
        {
            uint write_pointer = baseaddress + (id * 4);
            uint write_addr = Program.ROM.u32(write_pointer);

            if (Is_RAMPointerArea(write_addr))
            {//iw-ram / ew-ram にデータをおいている人がいるらしい
                return true;
            }

            return false;
        }


        //海外改造によくある unHuffman patchの可能性を調べる
        static bool EnableUnHuffmanPatch(
            uint baseaddress
            ,uint id)
        {
            uint write_pointer = baseaddress + (id * 4);
            if (! U.isSafetyOffset(write_pointer))
            {
                return false;
            }
            uint write_addr = Program.ROM.u32(write_pointer);

            return  (FETextEncode.IsUnHuffmanPatchPointer(write_addr));
        }
        private void TextWriteButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {
                R.ShowStopError(InputFormRef.GetBusyErrorExplain());
                return;
            }
            if (!this.InputFormRef.CheckWriteProtectionID00())
            {//TextID:0x00に書き込みを拒否された.
                return;
            }

            string text = GetEditorText(this.TextArea);

            uint textid = (uint)this.AddressList.SelectedIndex;
            if (Is_RAMPointerArea(InputFormRef.BaseAddress, textid))
            {
                R.ShowStopError("RAMエリアのため、書き込めません." , U.To0xHexString(textid));
                return;
            }

            uint write_addr = WriteText(InputFormRef.BaseAddress
                , InputFormRef.DataCount
                , textid
                , text
                );
            if(write_addr == U.NOT_FOUND)
            {
                R.ShowStopError("テキストを書き込み中にエラーが発生しました。\r\nTextID:{0}" , U.To0xHexString(textid));
                return;
            }
            InputFormRef.ShowWriteNotifyAnimation(this, write_addr);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.UpdateAllTextID(textid);
            InputFormRef.ReloadAddressList();
        }

        public static uint WriteText(
             uint baseaddress
            ,uint datacount
            ,uint id
            ,string text)
        {
            string undoname = "TEXT:" + U.ToHexString(id);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undoname);

            uint write_addr = WriteText(baseaddress
                , datacount
                , id
                , text
                , undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return U.NOT_FOUND;
            }

            Program.Undo.Push(undodata);
            return write_addr;
        }

        static void NeedAntiHuffman(string error)
        {
            string lang = OptionForm.lang();
            if (lang == "ja" || lang == "zh")
            {
                TextBadCharPopupForm f = (TextBadCharPopupForm)InputFormRef.JumpFormLow<TextBadCharPopupForm>();
                string dialog_text = R.Error("文字:{0}はシステムに登録されていません。", error);
                f.SetErrorMessage(dialog_text);
                f.ShowDialog();
            }
            else
            {
                HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.Anti_Huffman_By_English);
            }
        }

        public static uint WriteText(
             uint baseaddress
            , uint datacount
            , uint id
            , string text
            , Undo.UndoData undodata)
        {
            byte[] encode;
            string error;

            if (id >= GetDataCount())
            {
                R.Error("文字テーブル範囲外のため、書き込めません.\r\nTextID:{0}" , U.To0xHexString(id));
                return U.NOT_FOUND;
            }

            if (Is_RAMPointerArea(baseaddress, id))
            {
                R.Error("RAMエリアのため、書き込めません.\r\nTextID:{0}" , U.To0xHexString(id));
                return U.NOT_FOUND;
            }

            //とにかくハフマン符号化する
            error = Program.FETextEncoder.Encode(text, out encode);

            bool raiseUnHuffman = false;
            if (error.Length > 0)
            {//ハフマン符号化中にエラー発生.
                R.Notify("文字:{0}はシステムに登録されていません。", error);

                bool use_anti_Huffman = PatchUtil.SearchAntiHuffmanPatch();
                if (use_anti_Huffman == false)
                {//un Huffmanパッチが入っていないのでエラー表示.
                    NeedAntiHuffman(error);
                    //インストールされたか確認する.
                    use_anti_Huffman = PatchUtil.SearchAntiHuffmanPatch();
                    if (use_anti_Huffman == false)
                    {
                        return U.NOT_FOUND;
                    }
                }
                //unHuffmanをキメる.
                Program.FETextEncoder.UnHuffmanEncode(text, out encode);
                raiseUnHuffman = true;
            }

            uint write_addr = InputFormRef.WriteTextData(
                  baseaddress
                , datacount
                , id
                , Program.ROM.RomInfo.text_data_start_address
                , Program.ROM.RomInfo.text_data_end_address
                , encode
                , raiseUnHuffman
                , get_data_pos_callback
                , undodata
            );
            return write_addr;
        }
        static void UpdatePosstion(string srctext, ref List<TextBlock> simpleList)
        {
            uint[] units = new uint[9];
            CheckText ct = new CheckText(srctext);

            int len = simpleList.Count;
            for (int i = 0; i < len; i++)
            {
                TextBlock code = simpleList[i];
                code.Units = units;
                if (code.Code1 < 0x8)
                {
                    continue;
                }

                uint pos = code.Code1 - 0x8;
                if (pos >= units.Length)
                {//壊れてる.
                    code.Error = R._("ユニットの位置が0x08から0xFの範囲にありません。");
                    continue;
                }

                if (code.Code2 == 0x10)
                {//表示
                    units[pos] = code.Code3;
                }
                code.Units = (uint[])units.Clone();

                if (code.Code2 == 0x10)
                {//表示
                    continue;
                }
                else if (code.Code2 == 0x11)
                {//消去
                    units[pos] = 0;
                }
                else if (IsMoveOrJump(code.Code2, code.Code3))
                {
                    uint newPos = code.Code3 - 0xA;
                    if (newPos >= units.Length)
                    {//別命令
                        continue;
                    }
                    if (pos == newPos)
                    {//ジャンプ
                        code.isJump = true;
                        continue;
                    }
                    //既にその場所にキャラがいたら位置を入れ替える.
                    U.Swap<uint>(ref units[pos],ref units[newPos]);
                }
                else if (code.Code1 >= 0x8) 
                {//セリフ
                    CheckBlockResult result = ct.CheckBlockBox(code.SrcText, MAX_SERIF_WIDTH, 16 * 2, false);
                    if (result != CheckBlockResult.NoError)
                    {
                        code.Error = ct.ErrorString;
                    }
                }
            }
        }


        void UpdateTextList(string srctext, out List<TextBlock> simpleList, int selected)
        {
            ParseTextList(srctext, out simpleList);
            //位置を更新
            UpdatePosstion(srctext, ref simpleList);

            //リストの更新.
            this.TextList.DummyAlloc(this.SimpleList.Count, selected);
        }

        static int skip_linebreak(string text,int i)
        {
	        if(i >= text.Length )
	        {
		        return 0;
	        }
	        if (text[i]=='\r' && text[i+1]=='\n')
	        {
		        return 2;
	        }
	        return 0;
        }
        static bool CheckPosCodeOnly(string str)
        {
            return (str == "@0008" || str == "@0009" || str == "@000a" || str == "@000b" || str == "@000c" || str == "@000d" || str == "@000e" || str == "@000f" || str == "@000A" || str == "@000B" || str == "@000C" || str == "@000D" || str == "@000E" || str == "@000F");
        }

        //テキストをパースする
        static void ParseTextList(string srctext, out List<TextBlock> simpleList)
        {
            simpleList = new List<TextBlock>();

            uint lastPosstion = 0x0;
            int lastPosstionIndex = 0;

            int textstart = 0;
            int len = srctext.Length;
            for (int i = 0; i < len; )
            {
                if (srctext[i] != '@')
                {
                    i++;
                    continue;
                }
                int codestart = i;

                uint code1 = U.atoh(U.substr(srctext, i + 1, 4));
                i += 5;

                //1コード分先読みする.
                int next_i = i;

                uint code2 = 0;
                next_i += skip_linebreak(srctext, next_i); //FE7では、いきなり改行されるときがある.
                if (next_i <= len - 5 && srctext[next_i] == '@')
                {//次もコード
                    code2 = U.atoh(U.substr(srctext, next_i + 1, 4));
                    next_i += 5;
                }

                if (code1 == 0x80 && CheckTextEngineRework_ParseTextList(code2, srctext , ref next_i))
                {
                    i = next_i;
                    continue;
                }

                if ((code1 >= 8 && code1 <= 0xF))
                {//場所を定義するコード @0008 - @000F
                    //位置変更のコードが始まる前の部分にセリフがあればテキストとして保存.
                    string seriftext = U.substr(srctext, textstart, i - 5 - textstart);
                    if (seriftext != "")
                    {
                        TextBlock current = new TextForm.TextBlock();
                        current.SrcText = seriftext;
                        current.Code1 = lastPosstion;
                        current.Code2 = 0;
                        current.Code3 = 0;
                        simpleList.Add(current);

                        textstart = i - 5;
                    }

                    lastPosstion = code1;
                    if (code2 > 0)
                    {
                        lastPosstionIndex = i - 5;
                    }

                    continue;
                }

                if ( (code1 == 0x0010 && code2 > 0x100) || IsMoveOrJump(code1,code2) || code1 == 0x11)
                {
                    i = next_i;

                    //コードが始まる前の部分をテキストとして保存.
                    string seriftext;
                    if (lastPosstionIndex < textstart)
                    {
                        seriftext = U.substr(srctext, textstart, codestart - textstart);
                    }
                    else if (lastPosstionIndex == textstart && codestart - textstart > 7)
                    {//文章の途中で @0010@0112 みたいに、位置情報を無視して、表記が来た場合.
                        seriftext = U.substr(srctext, textstart, codestart - textstart);
                    }
                    else
                    {
                        seriftext = U.substr(srctext, textstart, lastPosstionIndex - textstart);
                        codestart = lastPosstionIndex;
                    }

                    if (lastPosstion <= 0)
                    {//FE7までは、@0010の前の位置情報を省略されるときがある
                        lastPosstion = 0x8;
                    }

                    if (code1 == 0x11)
                    {
                        if (code2 > 0)
                        {
                            i = i - 5;
                            code2 = 0;
                        }
                    }

                    
                    if (seriftext != "" && CheckPosCodeOnly(seriftext) == false)
                    {
                        TextBlock current = new TextForm.TextBlock();
                        current.SrcText = seriftext;
                        current.Code1 = lastPosstion;
                        current.Code2 = 0;
                        current.Code3 = 0;
                        simpleList.Add(current);
                    }

                    //コード部分の保存.
                    string codetext = U.substr(srctext, codestart, i - codestart);
                    {
                        TextBlock current = new TextForm.TextBlock();
                        current.SrcText = codetext;
                        current.Code1 = lastPosstion;
                        current.Code2 = code1;
                        current.Code3 = code2;
                        simpleList.Add(current);
                    }
                    textstart = i;

                    if (code1 == 0x0080)
                    {//移動させたりするコード @0080
                        lastPosstion = lastPosstion - 0x0002;
                    }
                    continue;
                }
            }
            //最後のかけら
            {
                string seriftext = U.substr(srctext, textstart);
                if (seriftext != "")
                {
                    TextBlock current = new TextForm.TextBlock();
                    current.SrcText = seriftext;
                    current.Code1 = lastPosstion;
                    current.Code2 = 0;
                    current.Code3 = 0;
                    simpleList.Add(current);
                }
            }
        }
        static bool IsMoveOrJump(uint code1,uint code2)
        {
            return code1 == 0x0080 && (code2 >= 0xA && code2 <= 0xF);
        }
#if DEBUG
        public static void TEST_TEXTPARSE1()
        {
            string text =
            "@0008@0010@0139@000D@0010@015A\r\n"            ///No Translate
            + "@000D@0080@000D\r\n"                         ///No Translate
            + "Nino!?@0005\r\n"   //先頭に @000D が省略されている. ///No Translate
            + "Why@0003\r\n"       ///No Translate
            + "\r\n@0008\r\n";      ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            Debug.Assert(simpleList[0].SrcText == "@0008@0010@0139");     ///No Translate
            Debug.Assert(simpleList[1].SrcText == "@000D@0010@015A");     ///No Translate
            Debug.Assert(simpleList[2].SrcText == "\r\n");                ///No Translate
            Debug.Assert(simpleList[3].SrcText == "@000D@0080@000D");     ///No Translate
            Debug.Assert(simpleList[4].SrcText == "\r\nNino!?@0005\r\nWhy@0003\r\n\r\n");    ///No Translate
            Debug.Assert(simpleList[5].SrcText == "@0008\r\n");           ///No Translate
        }
        public static void TEST_TEXTPARSE2()
        {
            string text =
            "@000C\r\n"		//途中で改行が入っている FE7:D1 中略     ///No Translate
            + "@0010@0137\r\n"                                       ///No Translate
            + "@0016？@0016@0003\r\n"                                ///No Translate
            + "\r\n"                                                 ///No Translate
            + "なんだい？@0004\r\n"                                  ///No Translate
            + "？@0003\r\n"                                          ///No Translate
            + "\r\n"                                                 ///No Translate
            + "@0002\r\n"                                            ///No Translate
            + "フン\r\n"                               ///No Translate
            + "\r\n"                                                 ///No Translate
            + "@0011@0006\r\n"                                       ///No Translate
            + "@0009@0010@0127\r\n"                                  ///No Translate
            + "・・・@0003\r\n"                        ///No Translate
            + "\r\n"                                                 ///No Translate
            + "まあいい・・・@0004\r\n"                              ///No Translate
            + "一人の方が@0003";                           ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            Debug.Assert(simpleList[0].SrcText == "@000C\r\n@0010@0137"); ///No Translate
            Debug.Assert(simpleList[1].SrcText == "\r\n@0016？@0016@0003\r\n\r\nなんだい？@0004\r\n？@0003\r\n\r\n@0002\r\nフン\r\n\r\n");    ///No Translate
            Debug.Assert(simpleList[2].SrcText == "@0011");    ///No Translate
            Debug.Assert(simpleList[3].SrcText == "@0006\r\n"); ///No Translate
            Debug.Assert(simpleList[4].SrcText == "@0009@0010@0127"); ///No Translate
            Debug.Assert(simpleList[5].SrcText == "\r\n・・・@0003\r\n\r\nまあいい・・・@0004\r\n一人の方が@0003");  ///No Translate
        }
        public static void TEST_TEXTPARSE3()
        {
            //A91 表示と同時にセリフ
            string text =
             "@0009@0010@016B\r\n"                      ///No Translate
            + "援軍が・・・@0004\r\n"           ///No Translate
            + "なぜだ・・・。@0003\r\n"                 ///No Translate
            + "@0015\r\n";                              ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);
            Debug.Assert(simpleList[0].SrcText == "@0009@0010@016B");     ///No Translate
            Debug.Assert(simpleList[1].SrcText == "\r\n援軍が・・・@0004\r\nなぜだ・・・。@0003\r\n@0015\r\n"); ///No Translate
        }

        public static void TEST_TEXTPARSE4()
        {
            string text =
            "@0009@0010@0102@000C@0010@0104@000Cエイリーク様\r\n" ///No Translate
            + "ここが@0003\r\n"        ///No Translate
            + "\r\n"                                    ///No Translate
            + "この港から\r\n"             ///No Translate
            + "ロストンまでは@0003\r\n"///No Translate
            + "@0009潮の匂いがします・・・\r\n"     ///No Translate
            + "とても@0003\r\n"   ///No Translate
            ;
            
            
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);
            Debug.Assert(simpleList[0].SrcText == "@0009@0010@0102"); ///No Translate
            Debug.Assert(simpleList[1].SrcText == "@000C@0010@0104"); ///No Translate
            Debug.Assert(simpleList[2].SrcText == "@000Cエイリーク様\r\nここが@0003\r\n\r\nこの港から\r\nロストンまでは@0003\r\n");   ///No Translate
            Debug.Assert(simpleList[2].Code1 == 0xC);
            Debug.Assert(simpleList[3].SrcText == "@0009潮の匂いがします・・・\r\nとても@0003\r\n");   ///No Translate
            Debug.Assert(simpleList[3].Code1 == 0x9);
        }

        public static void TEST_TEXTPARSE5()
        {
            string text =
            "@000B@0010@011A@0017\r\n" + ///No Translate
            "私はリン。\r\n" + ///No Translate
            "ロルカ族の娘。@0003@0002@0004\r\n" + ///No Translate
            "あなたは？\r\n" + ///No Translate
            "名前を教えて？@0003@0015@0006\r\n" + ///No Translate
            "@000B\r\n" + ///No Translate
            "@0080@0020っていうの？@0005\r\n";        ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            Debug.Assert(simpleList[0].SrcText == "@000B@0010@011A");     ///No Translate
            Debug.Assert(simpleList[1].SrcText == "@0017\r\n私はリン。\r\nロルカ族の娘。@0003@0002@0004\r\nあなたは？\r\n名前を教えて？@0003@0015@0006\r\n");     ///No Translate
            Debug.Assert(simpleList[2].SrcText == "@000B\r\n@0080@0020っていうの？@0005\r\n");  ///No Translate
        }
        public static void TEST_TEXTPARSE6()
        {
            string text =
            "@000B@0010@011A@0017\r\n" + ///No Translate
            "私はリン。\r\n" + ///No Translate
            "ロルカ族の娘。@0003@0002@0004\r\n" + ///No Translate
            "あなたは？\r\n" + ///No Translate
            "名前を教えて？@0003@0015@0006\r\n" + ///No Translate
            "@000B\r\n" + ///No Translate
            "@0080@0020っていうの？@0005\r\n";        ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            Debug.Assert(simpleList[0].SrcText == "@000B@0010@011A");     ///No Translate
            Debug.Assert(simpleList[1].SrcText == "@0017\r\n私はリン。\r\nロルカ族の娘。@0003@0002@0004\r\nあなたは？\r\n名前を教えて？@0003@0015@0006\r\n");     ///No Translate
            Debug.Assert(simpleList[2].SrcText == "@000B\r\n@0080@0020っていうの？@0005\r\n");  ///No Translate
        }
        public static void TEST_TEXTPARSE7()
        {
            string text =
            "@000B@0017\r\n" + ///No Translate
            "You're safe now.@0003@0002@0005\r\n" + ///No Translate
            "Can you remember your name?@0003@0015@0006\r\n" + ///No Translate
            "Your name is @0080@0020?@0005\r\n" + ///No Translate
            "What an odd-sounding name...@0003\r\n" + ///No Translate
            "But@0005 pay me no mind.\r\n" + ///No Translate
            "It is a good name.@0003@0002\r\n" + ///No Translate
            "you are a traveler.@0003\r\n" + ///No Translate
            "What brings you to the Sacae Plains?@0005\r\n" + ///No Translate
            "Would you share your story with me?@0003@0015@0017\r\n" + ///No Translate
            "@0010@0116\r\n" + ///No Translate
            "abc"; ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            Debug.Assert(simpleList[0].Code1 == 0xB);     ///No Translate
            Debug.Assert(simpleList[0].Code2 == 0x0);     ///No Translate

            Debug.Assert(simpleList[1].Code1 == 0xB);   //自動補完  ///No Translate
            Debug.Assert(simpleList[1].Code2 == 0x10);     ///No Translate
            Debug.Assert(simpleList[1].Code3 == 0x116);     ///No Translate
            Debug.Assert(simpleList[1].SrcText == "@0010@0116");     ///No Translate

            Debug.Assert(simpleList[2].Code1 == 0xB);     ///No Translate
            Debug.Assert(simpleList[2].Code2 == 0x0);     ///No Translate
            Debug.Assert(simpleList[2].SrcText == "\r\nabc");     ///No Translate
        }
        public static void TESTNOW_TEXTPARSE8()
        {
            string text =
            "@000B@0010@0116\r\n" + ///No Translate
            "You're safe now.@0003@0002@0005\r\n" + ///No Translate
            "@000B@0011" + ///No Translate
            ""; ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            Debug.Assert(simpleList[0].Code1 == 0xB);   //自動補完  ///No Translate
            Debug.Assert(simpleList[0].Code2 == 0x10);     ///No Translate
            Debug.Assert(simpleList[0].Code3 == 0x116);     ///No Translate
            Debug.Assert(simpleList[0].SrcText == "@000B@0010@0116");     ///No Translate

            Debug.Assert(simpleList[1].Code1 == 0xB);   //自動補完  ///No Translate
            Debug.Assert(simpleList[1].SrcText == "\r\nYou're safe now.@0003@0002@0005\r\n");     ///No Translate

            Debug.Assert(simpleList[2].Code1 == 0xB);     ///No Translate
            Debug.Assert(simpleList[2].Code2 == 0x11);     ///No Translate
        }

        public static void TESTNOW_TEXTPARSE9()
        {
            string text =
            "@000C@0010@0145"+ ///No Translate
            "@0009@0010@0102"+ ///No Translate
            "@000A@0010@0108"+ ///No Translate
            "@000AXXXXXXXXXXX@0003"+ ///No Translate
            "@000Cxxxxxxx @0003"+ ///No Translate
            "xxxxxxx@0003"+ ///No Translate
            "@0009@0080@000C@000Cxxxxxxx @0003"+ ///No Translate
            "xxxxxxx@0003"; ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            //位置を更新
            UpdatePosstion(text,ref simpleList);

            Debug.Assert(simpleList[5].Code1 == 0x9);   //自動補完  ///No Translate
            Debug.Assert(simpleList[5].Code2 == 0x80);     ///No Translate
            Debug.Assert(simpleList[5].Code3 == 0xC);     ///No Translate

            Debug.Assert(simpleList[5].Units[0] == 0x0);
            Debug.Assert(simpleList[5].Units[1] == 0x102);
            Debug.Assert(simpleList[5].Units[2] == 0x108);
            Debug.Assert(simpleList[5].Units[3] == 0x0);
            Debug.Assert(simpleList[5].Units[4] == 0x145);
            Debug.Assert(simpleList[5].Units[5] == 0x0);
            Debug.Assert(simpleList[5].Units[6] == 0x0);
            Debug.Assert(simpleList[5].Units[7] == 0x0);


            Debug.Assert(simpleList[6].Code1 == 0xC);   //自動補完  ///No Translate
            Debug.Assert(simpleList[6].Code2 == 0x0);     ///No Translate
            Debug.Assert(simpleList[6].Code3 == 0x0);     ///No Translate

            Debug.Assert(simpleList[6].Units[0] == 0x0);
            Debug.Assert(simpleList[6].Units[1] == 0x108);
            Debug.Assert(simpleList[6].Units[2] == 0x102);
            Debug.Assert(simpleList[6].Units[3] == 0x0);
            Debug.Assert(simpleList[6].Units[4] == 0x145);
            Debug.Assert(simpleList[6].Units[5] == 0x0);
            Debug.Assert(simpleList[6].Units[6] == 0x0);
            Debug.Assert(simpleList[6].Units[7] == 0x0);
        }

        public static void TESTNOW_TEXTPARSE10()
        {
            string text =
            "@000C@0010@0102@0080@001D@0016...@0004@0003\r\n" + ///No Translate
            "@0080@001C.....@0005@0003" ; ///No Translate
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);

            //位置を更新
            UpdatePosstion(text, ref simpleList);

            Debug.Assert(simpleList[0].Code1 == 0xc);
            Debug.Assert(simpleList[0].Code2 == 0x10); 
            Debug.Assert(simpleList[0].Code3 == 0x102);
            Debug.Assert(simpleList[0].SrcText == "@000C@0010@0102");///No Translate

            Debug.Assert(simpleList[1].Code1 == 0xc);
            Debug.Assert(simpleList[1].Code2 == 0x0);
            Debug.Assert(simpleList[1].Code3 == 0x0);
            Debug.Assert(simpleList[1].SrcText == "@0080@001D@0016...@0004@0003\r\n@0080@001C.....@0005@0003");///No Translate
        }

#endif


        private static void DrawUnitName(uint pos, uint face_id100, int lineHeight, Graphics g, Font font, SolidBrush brush, bool isWithDraw, Rectangle bounds)
        {
            string text = GetUnitNameWhereFaceID100(face_id100);
            Rectangle b = bounds;
            b.Height = lineHeight * 1;
            U.DrawText(text, g, font, brush, isWithDraw, bounds);
        }

        private static void DrawFacePos(uint pos, uint face_id100, int lineHeight, Graphics g, Font font, SolidBrush brush, bool isWithDraw, Rectangle bounds)
        {
            int name_x = (lineHeight * 7);

            Bitmap facebmp = DrawPortraitAuto100(face_id100);

            Rectangle b = bounds;
            if (pos <= 2)
            {
                b.X = b.X + (int)(name_x + ((lineHeight * 3) / 3 * pos));
                facebmp = ImageUtil.Copy(facebmp, 0, 0, facebmp.Width, facebmp.Height, true, false); //右向きに反転
            }
            else if (pos == 0xE - 0x8)
            {//左端
                b.X = b.X + (int)(name_x);
                facebmp = ImageUtil.Copy(facebmp, 0, 0, facebmp.Width, facebmp.Height, true, false); //右向きに反転
            }
            else
            {
                pos -= 3;
                b.X = b.X + (int)(name_x + (lineHeight * 22) + ((lineHeight * 3) / 3 * pos));
            }
            b.Width = lineHeight * 3;
            b.Height = lineHeight * 3;
            U.MakeTransparent(facebmp);
            U.DrawPicture(facebmp, g, isWithDraw, b);
        }

        private Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.SimpleList.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            TextBlock code = this.SimpleList[index];

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            Debug.Assert(listbounds.Height > 0);

            string text;
            Rectangle bounds = listbounds;

            SizeF size = g.MeasureString(" BCG", lb.Font);
            int lineHeight = (int)size.Height;
            int maxHeight = (int)size.Height;

            uint pos = code.Code1 - 0x8;
            uint face_id = pos >= code.Units.Length ? 0 : code.Units[pos];

            if (code.Code2 == 0x10)
            {//表示
                bounds.X += U.DrawText(R._("[表示]")
                    , g, boldFont, brush, isWithDraw, bounds);

                DrawUnitName(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                DrawFacePos(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                maxHeight = Math.Max(maxHeight, lineHeight * 3);
            }
            else if (code.Code2 == 0x11)
            {//消去
                bounds.X += U.DrawText(R._("[消去]")
                    , g, boldFont, brush, isWithDraw, bounds);

                DrawUnitName(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                DrawFacePos(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                maxHeight = Math.Max(maxHeight, lineHeight * 3);
            }
            else if (code.isJump)
            {//移動(ジャンプ)
                bounds.X += U.DrawText(R._("[ジャンプ]")
                    , g, boldFont, brush, isWithDraw, bounds);

                DrawUnitName(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                DrawFacePos(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                maxHeight = Math.Max(maxHeight, lineHeight * 3);
            }
            else if (IsMoveOrJump(code.Code2, code.Code3))
            {//移動
                bounds.X += U.DrawText(R._("[移動]")
                    , g, boldFont, brush, isWithDraw, bounds);

                DrawUnitName(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                DrawFacePos(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                maxHeight = Math.Max(maxHeight, lineHeight * 3);
            }
            else if (code.Code1 >= 0x8)
            {//セリフ
                bounds.X += U.DrawText(R._("[セリフ]")
                    , g, boldFont, brush, isWithDraw, bounds);

                DrawUnitName(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                DrawFacePos(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                maxHeight = Math.Max(maxHeight, lineHeight * 3);


                text = StripDrawSerifText(code);
                text = ConvertEscapeText(text);
                if (pos <= 2)
                {//左側
                    Rectangle b = bounds;
                    b.X = b.X + lineHeight * (8 + 4);
                    Size bb = U.DrawTextMulti(text, g, normalFont, brush, isWithDraw, b);
                    maxHeight = Math.Max(maxHeight, b.Y + bb.Height);
                }
                else
                {//右側
                    Rectangle b = bounds;
                    b.X = b.X + lineHeight * (4 + 4);
                    Size bb = U.DrawTextMulti(text, g, normalFont, brush, isWithDraw, b);
                    maxHeight = Math.Max(maxHeight, b.Y + bb.Height);
                }
            }
            else
            {//テキスト
                text = ConvertEscapeText(code.SrcText);
                Rectangle b = bounds;
                Size bb = U.DrawTextMulti(text, g, normalFont, brush, isWithDraw, b);
                maxHeight = Math.Max(maxHeight, b.Y + bb.Height);
            }

            if (code.Error != "")
            {
                U.DrawErrorRectangle(g, isWithDraw, listbounds);
            }

            brush.Dispose();
            boldFont.Dispose();

            bounds.Y += maxHeight;
            return new Size(bounds.X, bounds.Y);
        }


        void MakePosCombo(ref ComboBox combo, TextBlock code)
        {
            combo.BeginUpdate();
            combo.Items.Clear();

            string text;

            text = "@0008 " + R._("左端") + " " + GetUnitNameWhereFaceID100(code.Units[0]);
            combo.Items.Add(text);
            text = "@0009 " + R._("左中") + " " + GetUnitNameWhereFaceID100(code.Units[1]);
            combo.Items.Add(text);
            text = "@000A " + R._("左右") + " " + GetUnitNameWhereFaceID100(code.Units[2]);
            combo.Items.Add(text);
            text = "@000B " + R._("右左") + " " + GetUnitNameWhereFaceID100(code.Units[3]);
            combo.Items.Add(text);
            text = "@000C " + R._("右中") + " " + GetUnitNameWhereFaceID100(code.Units[4]);
            combo.Items.Add(text);
            text = "@000D " + R._("右端") + " " + GetUnitNameWhereFaceID100(code.Units[5]);
            combo.Items.Add(text);
            text = "@000E " + R._("左画面外") + " " + GetUnitNameWhereFaceID100(code.Units[6]);
            combo.Items.Add(text);
            text = "@000F " + R._("右画面外") + " " + GetUnitNameWhereFaceID100(code.Units[7]);
            combo.Items.Add(text);
            combo.Tag = code;
            combo.EndUpdate();

            uint pos = code.Code1 - 0x8;
            if (pos < code.Units.Length)
            {
                U.SelectedIndexSafety(combo, pos);
            }
        }
        void MakePortait(ref NumericUpDown nud, TextBlock code)
        {
            if (code.Code3 == 0xFFFF)
            {//最後にある FFFF訪問したキャラ
                U.SelectedIndexSafety(nud, 0xFFFF);
            }
            else if (code.Code3 >= 0x100)
            {
                int faceid = (int)code.Code3 - 0x100;
                U.SelectedIndexSafety(nud, (uint)faceid);
            }
        }

        //文字列で使われる @0102 みたいな 顔ID+100表記
        static string GetUnitNameWhereFaceID100(uint face_id100)
        {
            if (face_id100 == 0xFFFF)
            {
                return R._("FFFF訪問したキャラ");
            }
            if (face_id100 >= 0x100)
            {
                return ImagePortraitForm.GetPortraitName(face_id100 - 0x100);
            }
            return "";
        }
        static Bitmap DrawPortraitAuto100(uint face_id100)
        {
            return ImagePortraitForm.DrawPortraitAuto(face_id100 - 0x100);
        }
        string StripFirstCodeBySerifu(string text)
        {
            int i = 0;
            int length = text.Length;
            for (; i < length; i += 5)
            {
                if (text[i] != '@')
                {
                    return U.substr(text, i);
                }
                uint code = U.atoh(U.substr(text, i + 1, 4));
                if (code >= 0x8 && code <= 0x11 )
                {//位置表示　または表示非表示命令なので stripしてよい
                    continue;
                }
                if (code >= 0x100 )
                {//表示命令だと思われるので stripしてよい
                    continue;
                }
                //移動命令や、表情の命令なので stripしてはいけない.
                return U.substr(text, i);
            }
            return "";
        }

        public static string StripAllCode(string text)
        {
            return RegexCache.Replace(text, @"@([0-9A-F][0-9A-F][0-9A-F][0-9A-F])", "").Trim(new char[] { ' ', (char)0x1f, '\r', '\n', '　' });
        }

        private void TextList_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
            TextBlock code;
            if (this.TextList.SelectedIndex < 0 || this.TextList.SelectedIndex >= this.SimpleList.Count)
            {
                code = new TextBlock();
            }
            else
            {
                code = this.SimpleList[this.TextList.SelectedIndex];
            }

            //テキストには常に全部
            SetEditorText(this.TextListSpTextTextBox, code.SrcText);
            //セリフ
            MakePosCombo(ref this.TextListSpSerifuPosComboBox, code);
            if (code.Code1 > 0)
            {
                string text = StripFirstCodeBySerifu(code.SrcText);
                SetEditorText(this.TextListSpSerifuTextBox,text);
            }
            else
            {
                SetEditorText(this.TextListSpSerifuTextBox, code.SrcText);
            }
            //キャラ登場
            U.CopyCombo(this.TextListSpSerifuPosComboBox, ref this.TextListSpShowPosComboBox);
            MakePortait(ref this.TextListSpShowCharNumericUpDown, code);
            //キャラ消去
            U.CopyCombo(this.TextListSpSerifuPosComboBox, ref this.TextListSpHidePosComboBox);
            //移動
            U.CopyCombo(this.TextListSpSerifuPosComboBox, ref this.TextListSpMovePosComboBox);
            U.CopyCombo(this.TextListSpSerifuPosComboBox,ref this.TextListSpMoveNewPosComboBox);
            //ジャンプ
            U.CopyCombo(this.TextListSpSerifuPosComboBox,ref this.TextListSpJumpPosComboBox);

            Control prevFocusedControl = this.ActiveControl;
            if (code.Code2 == 0x10)
            {//表示
                TextListSpTab.SelectedTab = this.TextListSpShowPage;
            }
            else if (code.Code2 == 0x11)
            {//消去
                TextListSpTab.SelectedTab = this.TextListSpHidePage;
            }
            else if (code.isJump)
            {//移動(ジャンプ)
                TextListSpTab.SelectedTab = this.TextListSpJumpPage;
                U.SelectedIndexSafety(this.TextListSpMoveNewPosComboBox, code.Code3 - 0xA);
            }
            else if (IsMoveOrJump(code.Code2, code.Code3))
            {//移動
                uint newpos = code.Code3 - 0xA;
                if (newpos < this.TextListSpMoveNewPosComboBox.Items.Count)
                {
                    U.SelectedIndexSafety(this.TextListSpMoveNewPosComboBox, newpos);
                    TextListSpTab.SelectedTab = this.TextListSpMovePage;
                }
                else
                {
                    TextListSpTab.SelectedTab = this.TextListSpTextPage;
                }
            }
            else if (code.Code1 >= 0x8)
            {//セリフ
                TextListSpTab.SelectedTab =  this.TextListSpSerifuPage;
            }
            else
            {//テキスト
                TextListSpTab.SelectedTab = this.TextListSpTextPage;
            }

            this.ActiveControl = prevFocusedControl;
        }

        TextBlock BuildTextBlock()
        {
            TextBlock code = new TextBlock();
            if (TextListSpTab.SelectedTab == this.TextListSpShowPage)
            {//表示
                code.Code1 = (uint)this.TextListSpShowPosComboBox.SelectedIndex + 0x8;
                code.Code2 = 0x10;

                uint portraitID = (uint)TextListSpShowCharNumericUpDown.Value;
                if (portraitID >= 0xF000)
                {
                    code.Code3 = portraitID;
                }
                else
                {
                    code.Code3 = portraitID + 0x100;
                }
                code.SrcText = "@" + code.Code1.ToString("X04") 
                    + "@" + code.Code2.ToString("X04")  
                    + "@" + code.Code3.ToString("X04")
                    ;
            }
            else if (TextListSpTab.SelectedTab == this.TextListSpHidePage)
            {//消去
                code.Code1 = (uint)this.TextListSpHidePosComboBox.SelectedIndex + 0x8;
                code.Code2 = 0x11;
                code.SrcText = "@" + code.Code1.ToString("X04") 
                    + "@" + code.Code2.ToString("X04")  
                    ;
            }
            else if (TextListSpTab.SelectedTab == this.TextListSpJumpPage)
            {//移動(ジャンプ)
                code.Code1 = (uint)this.TextListSpJumpPosComboBox.SelectedIndex + 0x8;
                code.Code2 = 0x80;
                code.Code3 = (uint)this.TextListSpJumpPosComboBox.SelectedIndex + 0xA;
                code.isJump = true;
                code.SrcText = "@" + code.Code1.ToString("X04")
                    + "@" + code.Code2.ToString("X04")
                    + "@" + code.Code3.ToString("X04")
                    ;
            }
            else if (TextListSpTab.SelectedTab == this.TextListSpMovePage)
            {//移動
                code.Code1 = (uint)this.TextListSpMovePosComboBox.SelectedIndex + 0x8;
                code.Code2 = 0x80;
                code.Code3 = (uint)this.TextListSpMoveNewPosComboBox.SelectedIndex + 0xA;
                code.SrcText = "@" + code.Code1.ToString("X04")
                    + "@" + code.Code2.ToString("X04")
                    + "@" + code.Code3.ToString("X04")
                    ;
            }
            else if (TextListSpTab.SelectedTab == this.TextListSpSerifuPage)
            {//セリフ
                code.Code1 = (uint)this.TextListSpSerifuPosComboBox.SelectedIndex + 0x8;
                code.SrcText = "@" + code.Code1.ToString("X04")
                    + GetEditorText(this.TextListSpSerifuTextBox)
                    ;
                code.SrcText = AutoAppend0x0003(code.SrcText);
            }
            else
            {//テキスト
                code.SrcText = GetEditorText(this.TextListSpTextTextBox);
            }

            return code;
        }

        //セリフの末尾に @0003 がなければ追加する.
        static string AutoAppend0x0003(string text)
        {
            string trimtext = text.TrimEnd();
            string last = U.substr(trimtext, -5);
            if (last.Length >= 1  && last[0] == '@')
            {//エスケープが追加されているのでなにもしない.
                return text;
            }
            return trimtext + "@0003";
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (this.TextList.SelectedIndex < 0 || this.TextList.SelectedIndex >= this.SimpleList.Count)
            {
                NewButton_Click(sender,e);
                return;
            }
            //変更するのでUNDOに積む.
            this.TextArea.PushUndo();

            int selected = this.TextList.SelectedIndex;
            TextBlock block = BuildTextBlock();
            this.SimpleList[this.TextList.SelectedIndex] = block;

            string text = ConvertSimpleListToText(this.SimpleList);
            SetEditorText(this.TextArea, text);
            TextAreaUpdate();//TextAreaを確実に再描画

            //リストの更新.
            UpdateTextList(text, out this.SimpleList, selected);

            HideFloatingControlpanel();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (this.TextList.SelectedIndex < 0 || this.TextList.SelectedIndex >= this.SimpleList.Count)
            {
                return;
            }
            //変更するのでUNDOに積む.
            this.TextArea.PushUndo();

            int selected = this.TextList.SelectedIndex;
            this.SimpleList.RemoveAt(this.TextList.SelectedIndex);

            string text = ConvertSimpleListToText(this.SimpleList);
            SetEditorText(this.TextArea, text);
            TextAreaUpdate();//TextAreaを確実に再描画

            UpdateTextList(text, out this.SimpleList, selected);
            HideFloatingControlpanel();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            //変更するのでUNDOに積む.
            this.TextArea.PushUndo();

            int selected = this.TextList.SelectedIndex;
            TextBlock block = BuildTextBlock();

            //選択されている部分に追加
            if (selected < 0)
            {
                this.SimpleList.Add(block);
            }
            else
            {
                this.SimpleList.Insert(this.TextList.SelectedIndex+1, block);
            }

            string text = ConvertSimpleListToText(this.SimpleList);
            SetEditorText(this.TextArea, text);
            TextAreaUpdate();//TextAreaを確実に再描画

            UpdateTextList(text, out this.SimpleList , selected + 1);
            HideFloatingControlpanel();
        }

        String ConvertSimpleListToText(List<TextBlock> simpleList)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < simpleList.Count; i++)
            {
                sb.Append(simpleList[i].SrcText);
            }
            return sb.ToString();
        }


        private void TextListSpSerifuPosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (! (TextListSpSerifuPosComboBox.Tag is TextBlock))
            {
                return ;
            }
            TextBlock code = (TextBlock)TextListSpSerifuPosComboBox.Tag;
            
            TextListSpSerifuCharPictureBox.Image =
                DrawPortraitAuto100
                    ((uint)code.Units[TextListSpSerifuPosComboBox.SelectedIndex]);

            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
            }
        }

        public static List<U.AddrResult> MakeItemList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        public void JumpToSearch(string str)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                uint id = SearchText(str);
                if (id < AddressList.Items.Count)
                {
                    this.SearchTextBox.Clear();
                    this.SearchResultListBox.Items.Clear();

                    U.ForceUpdate(AddressList, id);
                }
            }
        }

        public static uint SearchText(string findstr)
        {
            if (findstr.Length <= 0)
            {
                return U.NOT_FOUND;
            }
            string lang = OptionForm.lang();
            bool isJP = (lang == "ja");
            findstr = U.CleanupFindString(findstr, isJP);
            FETextDecode textdecoder = new FETextDecode();

            InputFormRef InputFormRef = Init(null);
            UpdateDataCountCache(InputFormRef);

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            for (int i = 0; i < arlist.Count; i++)
            {
                U.AddrResult ar = arlist[i];

                int size;
                string str = textdecoder.DecodeAddr(ar.addr, out size);

                int hitpos = U.CleanupFindString(str, isJP).IndexOf(findstr);
                if (hitpos < 0)
                {//NO HIT
                    continue;
                }

                return (uint)i;
            }

            return U.NOT_FOUND;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string findstr = GetEditorText(SearchTextBox);
            if (findstr.Length <= 0)
            {
                return;
            }
            string lang = OptionForm.lang();
            bool isJP = (lang == "ja");

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                findstr = U.CleanupFindString(findstr, isJP);

                FETextDecode textdecoder = new FETextDecode();
                List<U.AddrResult> result = InputFormRef.MakeList((U.AddrResult ar) =>
                {
                    int size;
                    string str = textdecoder.DecodeAddr(ar.addr, out size);

                    int hitpos = U.CleanupFindString(str, isJP).IndexOf(findstr);
                    if (hitpos < 0)
                    {//NO HIT
                        return new U.AddrResult();
                    }
                    string name;
                    if (str.Length - hitpos >= 40)
                    {
                        name = ar.name + " " + str.Substring(hitpos, 40) + "...";
                    }
                    else
                    {
                        name = ar.name + " " + U.substr(str,hitpos);
                    }

                    return new U.AddrResult(ar.addr, name, ar.tag);
                });

                SearchResultListBox.Items.Clear();

                U.ConvertListBox(result, ref this.SearchResultListBox);
            }
        }

        private void HideFloatingControlpanel()
        {
            if (ControlPanel.Visible)
            {
                ControlPanel.Hide();
                if (this.TextTabControl.SelectedIndex == 0)
                {
                    TextList.Focus();
                }
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchButton.PerformClick();
            }
        }

        //ミニバージョン イベント命令のテキストプレビューとしてよびだされる
        public static Size DrawMini(string srctext, ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw, bool isFirstOnly)
        {
            List<TextBlock> simpleList;
            ParseTextList(srctext, out simpleList);
            //位置を更新
            UpdatePosstion(srctext, ref simpleList);

            Rectangle bounds = listbounds;
            int totalHeight = 0;

            for (int index = 0; index < simpleList.Count; index++)
            {
                TextBlock code = simpleList[index];

                Size size = DrawMiniOne(code, lb, g, bounds, isWithDraw);
                size.Width = size.Width - bounds.X;
                size.Height = size.Height - bounds.Y;
                bounds.Y += size.Height;
                bounds.Height -= size.Height;
                totalHeight += size.Height;

                if (isFirstOnly)
                {
                    if (bounds.Height <= 0)
                    {
                        break;
                    }
                    if (totalHeight >= listbounds.Height)
                    {
                        break;
                    }
                }
            }
            return new Size(bounds.X, bounds.Y);
        }
        static Size DrawMiniOne(TextBlock code, ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            uint pos = code.Code1 - 0x8;
            uint face_id = pos >= code.Units.Length ? 0 : code.Units[pos];

            if (code.Code2 == 0x10)
            {//表示
                //NOP カット
                return new Size(bounds.X, bounds.Y);
            }
            else if (code.Code2 == 0x11)
            {//消去
                //NOP カット
                return new Size(bounds.X, bounds.Y);
            }
            else if (code.isJump)
            {//移動(ジャンプ)
                //NOP カット
                return new Size(bounds.X, bounds.Y);
            }
            else if (IsMoveOrJump(code.Code2, code.Code3))
            {//移動
                //NOP カット
                return new Size(bounds.X, bounds.Y);
            }
            else if (code.Code1 >= 0x8)
            {//セリフ
                text = StripDrawMiniSerifText(code);
                if (face_id <= 0 || text == "")
                {//文字がないセリフは出さない.
                    return new Size(bounds.X, bounds.Y);
                }
                text = ConvertEscapeText(text);

                bounds.X -= lineHeight * 7;
                
                DrawFacePos(pos, face_id, lineHeight, g, boldFont, brush, isWithDraw, bounds);
                maxHeight = Math.Max(maxHeight, bounds.Y + lineHeight * 3);

                if (pos <= 2)
                {//左側
                    Rectangle b = bounds;
                    b.X = b.X + lineHeight * (8 + 4);
                    Size bb = U.DrawTextMulti(text, g, normalFont, brush, isWithDraw, b);
                    maxHeight = Math.Max(maxHeight, b.Y + bb.Height);
                }
                else
                {//右側
                    Rectangle b = bounds;
                    b.X = b.X + lineHeight * (4 + 4);
                    Size bb = U.DrawTextMulti(text, g, normalFont, brush, isWithDraw, b);
                    maxHeight = Math.Max(maxHeight, b.Y + bb.Height);
                }
            }
            else
            {//テキスト
                if (StripAllCode(code.SrcText) == "")
                {//コードだけなので無視しましょう.
                    return new Size(bounds.X, bounds.Y);
                }
                text = ConvertEscapeText(code.SrcText);
                Rectangle b = bounds;
                text = code.SrcText;
                Size bb = U.DrawTextMulti(text, g, normalFont, brush, isWithDraw, b);
                maxHeight = Math.Max(maxHeight, b.Y + bb.Height);
            }

            brush.Dispose();
            boldFont.Dispose();

            bounds.Y += maxHeight;
            return new Size(bounds.X, bounds.Y);
        }
        static string StripDrawMiniSerifText(TextBlock code)
        {
            return StripAllCode(code.SrcText);
        }
        static string StripDrawSerifText(TextBlock code)
        {
            if (code.SrcText.Length > 5 && code.SrcText[0] == '@')
            {//@0009セリフ みたいな普通の形式であれば、 @0009等の位置情報を削る.
                if (code.Code1 > 0xF)
                {
                    return code.SrcText;
                }

                //削るコードが位置情報である確認
                string stripCodeString = code.SrcText.Substring(1,5);
                uint stripCode = U.atoh(stripCodeString);
                if (stripCode > 0xF)
                {//削ってはいけないコード
                    return code.SrcText;
                }

                string text = code.SrcText.Substring(5);
                return text;
            }
            return code.SrcText;
        }
#if DEBUG
        public static void TEST_StripDrawSerifText()
        {//削っていいコードかどうか確認する.
            TextBlock code = new TextBlock();
            code.Code1 = 0xc;
            code.Code2 = 0x0;
            code.Code3 = 0x0;
            code.Error = "";
            code.isJump = false;
            code.SrcText = "@0080@001D@0016...@0004@0003\r\n@0080@001C.....@0005@0003";///No Translate
            
            string a = StripDrawSerifText(code);
            Debug.Assert(a == code.SrcText);
        }
#endif //DEBUG

        void ShowFloatingControlpanel()
        {
            int y;
            int index = this.TextList.SelectedIndex;
            if (index < 0)
            {//一件もない
                y = 0;
            }
            else
            {
                //編集する項目の近くに移動させます.
                Rectangle rect = this.TextList.GetItemRectangle(index);
                y = this.TextList.Location.Y
                    + rect.Y + rect.Height + 20
                    ;
                if (y + ControlPanel.Height >= TextList.Height)
                {//下に余白がないので上に出す.
                    y = this.TextList.Location.Y
                        + rect.Y
                        - ControlPanel.Height - 20;
                    if (y < 0)
                    {//上にも余白がないので、 Y = 0 の位置に出す
                        y = 0;
                    }
                }
            }
            //変更ボタンが光っていたら、それをやめさせる.
            InputFormRef.WriteButtonToYellow(this.UpdateButton, false);
            InputFormRef.WriteButtonToYellow(this.NewButton, false);

            ControlPanel.Location = new Point(ControlPanel.Location.X, y);
            KeywordHigtLightAuto(); //必要ならばキーワードハイライトをする
            ControlPanel.Show();

            if (TextListSpTab.SelectedIndex == 0)
            {
                TextListSpTextTextBox.Focus();
                TextListSpTextTextBox.Select(0, 0); //全選択解除.
            }
            else if (TextListSpTab.SelectedIndex == 1)
            {
                TextListSpSerifuTextBox.Focus();
                TextListSpSerifuTextBox.Select(0,0); //全選択解除.
            }
            else if (TextListSpTab.SelectedIndex == 2)
            {
                TextListSpShowPosComboBox.Focus();
            }
            else if (TextListSpTab.SelectedIndex == 3)
            {
                TextListSpHidePosComboBox.Focus();
            }
            else if (TextListSpTab.SelectedIndex == 4)
            {
                TextListSpMovePosComboBox.Focus();
            }
            else if (TextListSpTab.SelectedIndex == 5)
            {
                TextListSpJumpPosComboBox.Focus();
            }
        }

        private void TextList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.TextList.SelectedIndex < 0)
            {//1件もデータがないと初期化されないため強引に初期化する
                TextList_SelectedIndexChanged(null, null);
            }
            ShowFloatingControlpanel();
        }

        private void TextList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                RunUndo();
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                RunRedo();
                return;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ShowFloatingControlpanel();
                return;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.O)
            {
                TextToSpeechForm.OptionTextToSpeech(this.TextArea.Text2);
            }

            int i = this.TextList.SelectedIndex;
            if (i < 0)
            {
                return;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                U.SetClipboardText(TextPieceToTextOne(i));
                U.SelectedIndexSafety(this.TextList, i, true);
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                TextPieceToEvent(Clipboard.GetText(), i);
                U.SelectedIndexSafety(this.TextList, i + 1, true);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                RemoveButton_Click(null, null);
            }
        }
        void OptionTextToSpeechByRichText(Object sender, EventArgs e)
        {
            if ((sender is MenuItem))
            {
                sender = ((MenuItem)sender).GetContextMenu().SourceControl;
            }
            if (!(sender is RichTextBoxEx))
            {
                return;
            }
            RichTextBoxEx editor = (RichTextBoxEx)sender;
            TextToSpeechForm.OptionTextToSpeech(editor.Text2);
        }
        void OptionConvertAnrrowFont(Object sender, EventArgs e)
        {
            if ((sender is MenuItem))
            {
                sender = ((MenuItem)sender).GetContextMenu().SourceControl;
            }
            if (!(sender is RichTextBoxEx))
            {
                return;
            }
            RichTextBoxEx editor = (RichTextBoxEx)sender;
            string str = editor.Text2;
            if (str.Length <= 0)
            {
                return;
            }
            if (str.IndexOf("[_") >= 0)
            {
                str = MultiByteJPUtil.ConvertNarrowFontToAlpha(str);
            }
            else
            {
                str = U.ConvertNarrowFont(str);
            }

            editor.Text = str;
        }

        public const int MAX_SERIF_WIDTH = 214;
        public const int MAX_DEATH_QUOTE_WIDTH = 152;

        bool IsDeathQuoteSerif()
        {
            return (this.SelectDataTypeOf == FELint.Type.BATTTLE_TALK
                || this.SelectDataTypeOf == FELint.Type.HAIKU);
        }

        private void TextListSpSerifuTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = GetEditorText(this.TextListSpSerifuTextBox);
            CheckText ct = new CheckText(this.Cache_MainArea);

            int widthLimit ;
            if (IsDeathQuoteSerif())
            {
                widthLimit = MAX_DEATH_QUOTE_WIDTH;
            }
            else
            {
                widthLimit = MAX_SERIF_WIDTH;
            }

            CheckBlockResult result = ct.CheckBlockBox(text, widthLimit, 16 * 2, false);
            if (result == CheckBlockResult.NoError)
            {
                ERROR_SERIFU.Hide();
            }
            else
            {
                if (result == CheckBlockResult.ErrorFont)
                {
                    ERROR_SERIFU.Text = R._("警告:\r\nフォントが\r\nありません\r\n「{0}」",ct.ErrorFont);
                    ERROR_SERIFU.Show();
                }
                else if (result == CheckBlockResult.ErrorWidth)
                {
                    ERROR_SERIFU.Text = R._("警告:\r\n最大文字幅が\r\nセリフ上限を\r\n超えています。");
                    ERROR_SERIFU.Show();
                }
                else if (result == CheckBlockResult.ErrorHeight)
                {
                    ERROR_SERIFU.Text = R._("警告:\r\nセリフが\r\n3行以上に\r\nなっています");
                    ERROR_SERIFU.Show();
                }
            }

            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);

                //キーワードハイライト
                KeywordHighLight(TextListSpSerifuTextBox);
            }

        }

        public void JumpTo(uint v)
        {//使い勝手を上げるため、テキスト領域にフォーカスを当てる.
            
            if (v >= this.AddressList.Items.Count)
            {
                return;
            }
            U.SelectedIndexSafety(this.AddressList, v);

            if (this.SimpleList.Count <= 1)
            {
                TextArea.Focus();
            }
            else
            {
                TextList.Focus();
            }
        }


        void TranslateLanguageAutoSelect()
        {
            int from, to;
            TranslateTextUtil.TranslateLanguageAutoSelect(out from,out to);

            Translate_from.SelectedIndex = from;
            Translate_to.SelectedIndex = to;
        }


        private void TranslateButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            string from = U.InnerSplit(Translate_from.Text, "=", 0);
            string to = U.InnerSplit(Translate_to.Text,"=",0);
            if (from == to)
            {
                return;
            }
            String dir = Path.GetDirectoryName(Program.ROM.Filename);
            string from_rom = MainFormUtil.FindOrignalROMByLang(dir,from);
            string to_rom = MainFormUtil.FindOrignalROMByLang(dir, to);

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //よくある定型文の翻訳辞書
                Dictionary<string, string> transDic = TranslateTextUtil.LoadTranslateDic(from, to, from_rom, to_rom);

                string text = GetEditorText(this.TextArea);
                string result;
                try
                {
                    result = TranslateTextUtil.TranslateText((uint)this.AddressList.SelectedIndex
                        , text
                        , from
                        , to
                        , transDic, true , false);
                }
                catch (System.Net.WebException ee)
                {
                    R.ShowStopError("Google翻訳がエラーを返しました。\r\n翻訳リクエストの送りすぎです。\r\n\r\n{0}", ee.ToString());
                    return ;
                }

                SetEditorText(this.TextArea, result);
                UpdateTextList(result, out this.SimpleList , -1);
            }
            if (this.SimpleList.Count <= 1)
            {
                TextTabControl.SelectedTab = this.SrcTabPage;
            }
            else
            {
                TextTabControl.SelectedTab = this.EasyTabPage;
            }
        }

        static uint g_GetDataCount_Cache;
        public static uint GetDataCount()
        {
//この関数はあまりにも多用するためキャッシュすることにする.
//            InputFormRef InputFormRef = Init(null);
//            return InputFormRef.DataCount;
//            Debug.Assert(g_GetDataCount_Cache > 0x100);
            return g_GetDataCount_Cache;
        }
        static void UpdateDataCountCache(InputFormRef inputformref)
        {
            g_GetDataCount_Cache = inputformref.DataCount;
        }
        public static void UpdateDataCountCache()
        {
            InputFormRef InputFormRef = Init(null);
            UpdateDataCountCache(InputFormRef);
        }

        public static uint GetBaseAddress()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.BaseAddress;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list , bool isPointerOnly)
        {
            string name = "Text";
            InputFormRef InputFormRef = Init(null);
            UpdateDataCountCache(InputFormRef);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 }, Address.DataTypeEnum.TEXTPOINTERS);

            name = "Text ";
            FETextDecode textdecoder = new FETextDecode();
            List<U.AddrResult> arlist = InputFormRef.MakeList();
            for(int i = 0 ; i < arlist.Count ; i++)
            {
                int size = 0;
                uint paddr = Program.ROM.u32(arlist[i].addr);

                if (FETextEncode.IsUnHuffmanPatchPointer(paddr))
                {//un-huffman patch?
                    uint unhuffman_addr = U.toOffset(FETextEncode.ConvertUnHuffmanPatchToPointer(paddr));
                    if (!isPointerOnly)
                    {
                        textdecoder.UnHffmanPatchDecode(unhuffman_addr, out size);
                    }
                    FEBuilderGBA.Address.AddAddress(list,unhuffman_addr
                        , (uint)size
                        , arlist[i].addr
                        , name + U.ToHexString(i)
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                else if (U.isPointer(paddr))
                {
                    uint addr = U.toOffset(paddr);
                    if (!isPointerOnly)
                    {
                        textdecoder.huffman_decode(addr, out size);
                    }
                    FEBuilderGBA.Address.AddAddress(list,addr
                        , (uint)size
                        , arlist[i].addr
                        , name + U.ToHexString(i)
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
            }

/*
            {
                uint text_data_start = Program.ROM.RomInfo.text_data_start_address;
                uint text_data_end   = Program.ROM.RomInfo.text_data_end_address;
                if (text_data_end < text_data_start)
                {
                    U.Swap<uint>(ref text_data_start,ref text_data_end);
                }
                //テキストの割当領域
                FEBuilderGBA.Address.AddAddress(list,text_data_start
                    , text_data_end - text_data_start
                    , U.NOT_FOUND
                    , "TEXT AREA"
                    , FEBuilderGBA.Address.DataTypeEnum.BIN);
            }
*/
        }

        //現在のテキスト長を求めます.
        uint CalcTextSize(string text)
        {
            byte[] encode;
            if (EnableUnHuffmanPatch(InputFormRef.BaseAddress
                ,(uint)this.AddressList.SelectedIndex))
            {//unHuffmanをキメてやがる.
                Program.FETextEncoder.UnHuffmanEncode(text, out encode);
            }
            else
            {//ふつーなので ハフマン符号化する
                string error = Program.FETextEncoder.Encode(text, out encode);
                //エンコードのエラー確認.
                if (error.Length > 0)
                {//ハフマン符号化中にエラー発生.
                    bool use_anti_Huffman = PatchUtil.SearchAntiHuffmanPatch();
                    if (use_anti_Huffman == false)
                    {//un Huffmanパッチが入っていないのでエラー表示.
                        return 0;
                    }
                    //unHuffmanをキメる.
                    Program.FETextEncoder.UnHuffmanEncode(text, out encode);
                }
            }

            if (encode == null)
            {
                return 0;
            }

            return (uint)encode.Length;
        }

        private void TextArea_TextChanged(object sender, EventArgs e)
        {
            this.Cache_MainArea = GetEditorText(this.TextArea);
            UpdateBlockSize(Cache_MainArea);

            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {//手動で変更されたときの変更イベントをハンドルします.
             //ハイライトイベントも変更通知を生むときがあるっぽい? そうするととてつもなく遅くなるので.
                TextAreaUpdate();
            }
        }
        //テキスト全文のキャッシュ
        //よく参照するので、キャッシュを作って保存しておく.
        //CacheMainArea = GetEditorText(this.TextArea);
        string Cache_MainArea;

        void TextAreaUpdate()
        {
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);

            //キーワードハイライト
            KeywordHighLight(TextArea);

            //DetailErrorMessageBox
        }
        //const int TEXTBUFFER_LIMIT = 4096; //本来の限界値
        const int TEXTBUFFER_LIMIT = 5636;   //テキストバッファの下にはフェードイン用のパレットバッファがあるので、そこてまでの超過を認める
        static string CheckUnpackSizeTooLong(string text)
        {
            if (text.Length <= 2000)
            {//UTF16でそれほどでかくないなら超える可能性はないので足切り
                return "";
            }
            byte[] encode;
            Program.FETextEncoder.UnHuffmanEncode(text, out encode);

            if (encode.Length >= TEXTBUFFER_LIMIT)
            {
                return R._("警告:\r\n展開時の会話の長さ({0}bytes)が長すぎるので、会話を2つに分割してください。\r\n4096バイトを超えるとテキストバッファを超過し危険です。(ただし背後はフェードアウト用のパレットバッファなので死にはしません。)\r\n5636バイトを超えると主要なバッファへダメージを与えて、ゲームがクラッシュします。", encode.Length);
            }
            return "";
        }
        void UpdateBlockSize(string text)
        {
            uint datasize = CalcTextSize(text);
            this.BlockSize.Text = datasize.ToString();
            this.BlockSize.ErrorMessage = CheckUnpackSizeTooLong(text);
        }

        private void TextListSpTextTextBox_TextChanged(object sender, EventArgs e)
        {
            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
                //キーワードハイライト
                KeywordHighLight(TextListSpTextTextBox);
            }
        }

        private void TextListSpShowPosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
            }
        }

        private void TextListSpHidePosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
            }
        }

        private void TextListSpMovePosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
            }
        }

        private void TextListSpMoveNewPosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
            }
        }

        private void TextListSpJumpPosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            if (isOrderOfHuman)
            {
                //変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
            }
        }


        void ClearUndoBuffer()
        {
            this.TextArea.ClearUndoBuffer();
        }
        void RunUndo()
        {
            this.TextArea.RunUndo();

            string text = GetEditorText(this.TextArea);
            UpdateTextArea(text);
            //必要ならばキーワードハイライトをする.
            KeywordHigtLightAuto();
        }
        void RunRedo()
        {
            this.TextArea.RunRedo();

            string text = GetEditorText(this.TextArea);
            UpdateTextArea(text);
            //必要ならばキーワードハイライトをする.
            KeywordHigtLightAuto();
        }

        private void UNDOButton_Click(object sender, EventArgs e)
        {
            RunUndo();
        }

        private void REDOButton_Click(object sender, EventArgs e)
        {
            RunRedo();
        }

        private void TextPieceToEvent(string str, int insertPoint = -1)
        {
            this.TextArea.PushUndo();

            StringBuilder sb = new StringBuilder();
            int n;
            for (n = 0; n < insertPoint; n++)
            {
                sb.Append(this.SimpleList[n].SrcText);
            }

            sb.Append(str);

            for (; n < this.SimpleList.Count ; n++)
            {
                sb.Append(this.SimpleList[n].SrcText);
            }

            //テキスト全体を更新して、SimpleListを再構築
            string text = sb.ToString();
            UpdateTextArea(text);
            //必要ならばキーワードハイライトをする.
            KeywordHigtLightAuto();
        }

        private string TextPieceToTextOne(int number)
        {
            TextBlock code = this.SimpleList[number];
            return code.SrcText;
        }

        void KeywordHigtLightAuto()
        {
            if (TextTabControl.SelectedIndex == 1)
            {//キーワードハイライトは時間がかかるので、選択されたときに実行します.
                //キーワードハイライト
                KeywordHighLight(TextArea);
            }
            else if (TextTabControl.SelectedIndex == 0)
            {
                //簡易タブのキーワードハイライト
                if (TextListSpTab.SelectedIndex == 0)
                {
                    KeywordHighLight(TextListSpTextTextBox);
                }
                else if (TextListSpTab.SelectedIndex == 1)
                {
                    KeywordHighLight(TextListSpSerifuTextBox);
                }
            }
        }

        public static void KeywordHighLight(RichTextBoxEx target)
        {
            if (target.IsDisposed)
            {
                return;
            }

            target.LockWindowUpdate();
            if (OptionForm.text_escape() == OptionForm.text_escape_enum.FEditorAdv)
            {
                KeywordHighLightFEditor(target);
            }
            else
            {
                KeywordHighLightProjectFEGBA(target);
            }
            target.UnLockWindowUpdate();
        }

        static void KeywordHighLightProjectFEGBA(RichTextBox target)
        {
            Color keywordBackColor = OptionForm.Color_Keyword_BackColor();
            Color keywordForeColor = OptionForm.Color_Keyword_ForeColor();

            //ハイライトを解除.
            target.SelectionStart = 0;
            target.SelectionLength = target.TextLength;
            target.SelectionColor = OptionForm.Color_Input_ForeColor();
            target.SelectionBackColor = OptionForm.Color_Input_BackColor();

            //改行コードが違うので、必ず取得しなおす
            string text = target.Text;
            int length = text.Length;
            for(int i = 0 ; i < length ; i++)
            {
                if (text[i] != '@')
                {
                    continue;
                }
                if (i + 4 >= length)
                {
                    break;
                }
                if (!U.ishex(text[i + 1])
                    || !U.ishex(text[i + 2])
                    || !U.ishex(text[i + 3])
                    || !U.ishex(text[i + 4])
                    )
                {
                    continue;
                }
                target.SelectionStart = i;
                target.SelectionLength = 5;
                target.SelectionColor = keywordForeColor;
                target.SelectionBackColor = keywordBackColor;

                i += 4;
            }
        }
        static void KeywordHighLightFEditor(RichTextBox target)
        {
            Color keywordBackColor = OptionForm.Color_Keyword_BackColor();
            Color keywordForeColor = OptionForm.Color_Keyword_ForeColor();

            target.SelectionStart = 0;
            target.SelectionLength = target.TextLength;
            target.SelectionColor = OptionForm.Color_Input_ForeColor();
            target.SelectionBackColor = OptionForm.Color_Input_BackColor();

            //ハイライトを解除.
            target.SelectionStart = 0;
            target.SelectionLength = target.TextLength;
            target.SelectionColor = OptionForm.Color_Input_ForeColor();
            target.SelectionBackColor = OptionForm.Color_Input_BackColor();

            //改行コードが違うので、必ず取得しなおす
            string text = target.Text;
            
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                if (text[i] != '[')
                {
                    continue;
                }
                if (i+1 >= length)
                {
                    break;
                }
                // [xxxxx] というキーワードがあればマーク
                int end = text.IndexOf(']',i+1);
                if (end < 0)
                {
                    break;
                }
                string parts = text.Substring(i,end - i + 1);
                bool found = Program.TextEscape.Find(parts);
                if (!found)
                {
                    if (parts.Length >= 3 && parts[1] == '0' && parts[2] == 'x')
                    {//16進数の可能性.
                        found = true;
                        for(int n = 3 ; n < parts.Length ; n ++)
                        {
                            if (parts[n] == ']')
                            {
                                break;
                            }
                            if(!U.ishex(parts[n]))
                            {
                                found = false;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        continue;
                    }
                }

                int len = parts.Length;
                target.SelectionStart = i;
                target.SelectionLength = len;
                target.SelectionColor = keywordForeColor;
                target.SelectionBackColor = keywordBackColor;

                i += len - 1;
            }
        }

        static void SetEditorText(RichTextBoxEx target, string text)
        {
            target.Text = ConvertEscapeText(text);
        }
        public static string ConvertEscapeText(string text)
        {
            if (PatchUtil.SearchTextEngineReworkPatch() == PatchUtil.TextEngineRework_enum.TeqTextEngineRework)
            {
                return ConvertTeqTextEngineRework(text, OptionForm.text_escape());
            }

            if (OptionForm.text_escape() == OptionForm.text_escape_enum.FEditorAdv)
            {
                return ConvertEscapeToFEditor(text);
            }
            return text;
        }
        public static string ConvertEscapeTextRev(string text)
        {
            if (OptionForm.text_escape() == OptionForm.text_escape_enum.FEditorAdv)
            {
                return ConvertFEditorToEscape(text);
            }
            return text;
        }
        public static string GetLineBreak()
        {
            if (OptionForm.text_escape() == OptionForm.text_escape_enum.FEditorAdv)
            {
                return "[A]";
            }
            return "@0003";
        }

        static string GetEditorText(RichTextBoxEx target)
        {
            string text = target.Text2;
            if (OptionForm.texteditor_auto_convert_space() == OptionForm.texteditor_auto_convert_space_enum.AutoConvertByLang)
            {
                text = AutoConvertSpaceByLang(text);
            }

            //text = ConvertUnicodeDirect(text);
            if (OptionForm.text_escape() == OptionForm.text_escape_enum.FEditorAdv)
            {
                return ConvertFEditorToEscape(text);
            }
            return text;
        }
        static string AutoConvertSpaceByLang(string text)
        {
            PatchUtil.draw_font_enum draw_font = PatchUtil.SearchDrawFontPatch();

            if (Program.ROM.RomInfo.is_multibyte)
            {
                if (draw_font == PatchUtil.draw_font_enum.DrawSingleByte || draw_font == PatchUtil.draw_font_enum.DrawUTF8)
                {//SingleByte描画パッチが入っているので、何もしない
                    return text;
                }
                OptionForm.textencoding_enum textencoding = OptionForm.textencoding();
                if (textencoding == OptionForm.textencoding_enum.ZH_TBL)
                {//わからない FE8CNでは、この文字にすることが多いようだ
                    return text.Replace(' ', '，');
                }
                else if (textencoding == OptionForm.textencoding_enum.EN_TBL)
                {//倍角スペースを半角スペースへ
                    return text.Replace('　', ' ');
                }
                else
                {//半角スペースを倍角スペース
                    return text.Replace(' ', '　');
                }
            }
            else
            {//倍角スペースを半角スペースへ
                if (draw_font == PatchUtil.draw_font_enum.DrawMultiByte || draw_font == PatchUtil.draw_font_enum.DrawUTF8)
                {//MultiByte描画パッチが入っているので、何もしない
                    return text;
                }
                return text.Replace('　', ' ');
            }
        }
        static string ConvertUnicodeDirect(string str)
        {
            str = RegexCache.Replace(str, @"\[U\+([0-9A-F]+)\]", (m) => { return U.ToUnicode(U.atoh(m.Groups[1].Value)); });
            return str;
        }


        //@0003 -> [A] とFEditor表記に変換する
        public static string ConvertEscapeToFEditor(string str)
        {
            str = RegexCache.Replace(str, @"@0010@0([0-9A-F][0-9A-F][0-9A-F])", "[LoadFace][0x$1]");
            str = Program.TextEscape.table_replace(str);
            str = RegexCache.Replace(str, @"@([0-9A-F][0-9A-F][0-9A-F][0-9A-F])", "[0x$1]");
            return str;
        }
        //[A] -> @0003 とFEditor表記から変換する
        public static string ConvertFEditorToEscape(string str)
        {
            str = RegexCache.Replace(str, @"\[LoadFace\]\[0x00([0-9A-F][0-9A-F][0-9A-F])\]", "@0010@0$1");
            str = RegexCache.Replace(str, @"\[LoadFace\]\[0x([0-9A-F][0-9A-F][0-9A-F])\]", "@0010@0$1");
            str = Program.TextEscape.table_replace_rev(str);
            str = str.Replace("[N]", "");///No Translate
            str = str.Replace("[X]", "");///No Translate
            str = RegexCache.Replace(str, @"\[0x([0-9A-F])\]", "@000$1");
            str = RegexCache.Replace(str, @"\[0x([0-9A-F][0-9A-F])\]", "@00$1");
            str = RegexCache.Replace(str, @"\[0x([0-9A-F][0-9A-F][0-9A-F])\]", "@0$1");
            str = RegexCache.Replace(str, @"\[0x([0-9A-F][0-9A-F][0-9A-F][0-9A-F])\]", "@$1");
            return str;
        }

        private void TextTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TextTabControl.SelectedIndex == 0)
            {//簡易リストにしたときに、詳細モードで変更していたテキストがあるといけないので、更新処理を行う.
                string text = GetEditorText(this.TextArea);
                UpdateTextArea(text);
            }

            KeywordHigtLightAuto(); //必要ならばキーワードハイライトをする
        }

        private void TextListSpTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeywordHigtLightAuto(); //必要ならばキーワードハイライトをする
        }



        public static uint ExpandsArea(Form form, uint newdatacount, Undo.UndoData undodata)
        {
            uint newaddr;
            {
                InputFormRef ifr = Init(null);
                newaddr = ifr.ExpandsArea(form, newdatacount, undodata, Program.ROM.RomInfo.text_pointer);
            }
            InputFormRef.ClearCacheDataCount();
            return newaddr;
        }

        private void Export_Click(object sender, EventArgs e)
        {
            ToolTranslateROM trans = new ToolTranslateROM();
            trans.InitExportFilter((uint)X_ExportFilterCombo.SelectedIndex);
            trans.ExportallText(this);
        }

        private void Import_Click(object sender, EventArgs e)
        {
            ToolTranslateROM trans = new ToolTranslateROM();
            trans.CheckTextImportPatch(false, false);

            Undo.UndoData undodata = Program.Undo.NewUndoData("Import Textfile");
            bool r = trans.ImportAllText(this, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return;
            }
            trans.BlackOut(undodata);
            Program.Undo.Push(undodata);

            this.InputFormRef = Init(this);
            UpdateDataCountCache(this.InputFormRef);
            U.ReSelectList(this.AddressList);

            InputFormRef.ShowWriteNotifyAnimation(this,U.NOT_FOUND);
        }
        public static string GetExplainOneLine()
        {
            return R._("改行やエスケープシーケンを入れてはいけません。\r\n") + GetExplainTextID();
        }
        public static string GetExplainOPCLASS2()
        {
            return R._("最大で2行まで書くことができます。\r\n") + GetExplainTextID();
        }
        public static string GetExplainThreeLine()
        {
            return R._("最大で3行まで書くことができます。\r\n") + GetExplainTextID();
        }
        public static string GetExplainTextID()
        {
            return R._("FEでは文字列は、文字列テーブルのIDで管理されています。\r\n");
        }


        private void TextForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.J)
            {
                GotoTextTabSwap();
                return;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
            }
        }

        void GotoTextTabSwap()
        {
            if (this.TextTabControl.SelectedIndex == 1)
            {
                GotoSimpleList();
            }
            else
            {
                GotoDetail();
            }
        }

        void GotoDetail()
        {
            bool isWriteButtonChange = InputFormRef.IsWriteButtonToYellow(this.AllWriteButton);
            HideFloatingControlpanel();
            this.TextTabControl.SelectedIndex = 1;
            this.TextArea.Focus();

            //フォーカスを移動すると、OnChangeTextが発生するときがある。
            //C#はよくわからない...
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, isWriteButtonChange);
        }
        void GotoSimpleList()
        {
            HideFloatingControlpanel();
            this.TextTabControl.SelectedIndex = 0;
            this.TextArea.Focus();
        }

        static bool IsFE8SplitMenu(uint textid)
        {
            if (Program.ROM.RomInfo.version != 8)
            {
                return false;
            }
            if (Program.ROM.RomInfo.is_multibyte)
            {//FE8J
            }
            else
            {//FE8U
                if (textid == 0xc15 || textid == 0xc16)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetErrorMessage(string text,uint textid ,string arg1)
        {
            if (textid == 0 || textid >= 0xFFFE)
            {
                return "";
            }
            if (textid > TextForm.GetDataCount())
            {
                return R._("警告:\r\n範囲外のTextID:{0}を参照しています。", U.ToHexString(textid));
            }

            if (text == "")
            {
                return "";
            }

            if (arg1 == "NAME1")
            {
                return CheckOneLineTextMessage(text, 10 * 8, 1 * 16, true);
            }
            if (arg1 == "DETAIL3")
            {
                return CheckOneLineTextMessage(text, 24 * 8, 3 * 16, false);
            }
            if (arg1 == "EDTITLE1")
            {
                return CheckOneLineTextMessage(text, 120, 1 * 16, true);
            }
            if (arg1 == "EDAFTER5")
            {
                return CheckOneLineTextMessage(text, 250, 5 * 16, true);
            }
            if (arg1 == "FE6EDAFTER6")
            {
                return CheckOneLineTextMessage(text, 250, 6 * 16, true);
            }
            if (arg1 == "LYNEDAFTER8")
            {
                return CheckOneLineTextMessage(text, 250, 8 * 16, false);
            }
            if (arg1 == "SOUND1")
            {
                return CheckOneLineTextMessage(text, 24 * 8, 1 * 16, false);
            }
            if (arg1 == "MAPNAME1")
            {
                return CheckOneLineTextMessage(text, 140, 1 * 16, true);
            }
            if (arg1 == "MAPARMY1")
            {
                return CheckOneLineTextMessage(text, 80, 1 * 16, true);
            }
            if (arg1 == "MAPGOAL1")
            {
                return CheckOneLineTextMessage(text, 110, 1 * 16, true);
            }
            if (arg1 == "MAPGOAL2")
            {
                return CheckOneLineTextMessage(text, 110, 2 * 16, true);
            }
            if (arg1 == "DICNAME1")
            {
                return CheckOneLineTextMessage(text, 142, 1 * 16, true);
            }
            if (arg1 == "MENUNAME1")
            {
                if (IsFE8SplitMenu(textid))
                {
                    return "";
                }
                return CheckOneLineTextMessage(text, 10 * 8, 1 * 16, true);
            }
            if (arg1 == "MENUDETAIL2")
            {
                return CheckOneLineTextMessage(text, 24 * 8, 2 * 16, false);
            }
            if (arg1 == "MENUDETAIL3")
            {
                return CheckOneLineTextMessage(text, 24 * 8, 3 * 16, false);
            }
            if (arg1 == "TERRAINNAME1")
            {
                return CheckOneLineTextMessage(text, 5 * 8, 1 * 16, true);
            }
            if (arg1 == "OPCLASS2")
            {
                return CheckOneLineTextMessage(text, MAX_SERIF_WIDTH, 2 * 16, false);
            }
            if (arg1 == "ITEM3")
            {
                return CheckOneLineTextMessage(text, 24 * 8, 3 * 16, false);
            }
            if (arg1 == "ITEM1")
            {
                return CheckOneLineTextMessage(text, 24 * 8, 1 * 16, false);
            }
            if (arg1 == "ITEMX")
            {//とりあえず3行でチェック
                return CheckOneLineTextMessage(text, 24 * 8, 3 * 16, false);
            }
            if (arg1 == "CONVERSATION")
            {
                return CheckConversationTextMessage(text, textid, MAX_SERIF_WIDTH);
            }
            if (arg1 == "DEATHQUOTE")
            {
                return CheckConversationTextMessage(text, textid, MAX_DEATH_QUOTE_WIDTH);
            }
            return "";
        }

        static string CheckParse(string text, int widthLimit, int heightLimit, bool isItemFont)
        {//実際に文字列をパースして調べてみます.
            //実際にパースしてみよう.
            List<TextBlock> simpleList;
            ParseTextList(text, out simpleList);
            CheckText ct = new CheckText(text);
            for (int i = 0; i < simpleList.Count; i++)
            {
                TextBlock code = simpleList[i];

                if (code.Code2 == 0x10)
                {//表示
                    continue;
                }
                else if (code.Code2 == 0x11)
                {//消去
                    continue;
                }
                else if (code.isJump)
                {//移動(ジャンプ)
                    continue;
                }
                else if (IsMoveOrJump(code.Code2, code.Code3))
                {//移動
                    continue;
                }

                //セリフとテキスト
                CheckBlockResult r = ct.CheckBlockBox(code.SrcText, widthLimit, heightLimit, isItemFont);
                if (r != CheckBlockResult.NoError)
                {
                    return ct.ErrorString;
                }
            }

            //全体の長さの確認
            return CheckUnpackSizeTooLong(text);
        }

        enum CheckBlockResult
        {
             NoError
            ,ErrorWidth
            ,ErrorHeight
            ,ErrorFont
        }

        class CheckText
        {
            public string ErrorString { get; private set; }
            public string ErrorFont { get; private set; }

            bool FoundUnkownFont;
            bool IsMultiByte;
            bool HasAutoNewLine;
            bool HasEnable3Line;
            OptionForm.textencoding_enum TextEncoding;
            OptionForm.lint_text_skip_bug_enum LintTextSkipBug;

            public CheckText(string mainText)
            {
                this.TextEncoding = OptionForm.textencoding();
                this.LintTextSkipBug = OptionForm.lint_text_skip_bug();
                this.IsMultiByte = Program.ROM.RomInfo.is_multibyte;
                this.HasAutoNewLine = CheckHasAutoNewLine(mainText);
                this.HasEnable3Line = CheckEnable3LineTeqEscapeTeqTextEngineRework(mainText);
            }

            static bool CheckHasAutoNewLine(string text)
            {
                if (PatchUtil.SearchAutoNewLinePatch() == PatchUtil.AutoNewLine_enum.AutoNewLine
                    || OptionForm.felint_check_text_width()
                    )
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
            static bool CheckEnable3LineTeqEscapeTeqTextEngineRework(string text)
            {
                if (PatchUtil.SearchTextEngineReworkPatch() == PatchUtil.TextEngineRework_enum.TeqTextEngineRework)
                {//Teqのテキストエンジンリワークの場合は、奴が使うコードを消す
                    return text.IndexOf("@0080@002B@0003") >= 0;
                }
                return false;
            }
            static string RemoveEscapeTeqTextEngineRework(string text)
            {
                text = text.Replace("\r\n", "@0001");
                text = RegexCache.Replace(text, "@0080@00(?:2(?:[689ABC]|(?:[7E]|(?:F@00[0-9A-F][0-9A-F]@00[0-9A-F][0-9A-F]|D)@00[0-9A-F][0-9A-F]@00[0-9A-F][0-9A-F])@00[0-9A-F][0-9A-F])|3[012345678])@00[0-9A-F][0-9A-F]", "");
                text = text.Replace("@0001", "\r\n");
                return text;
            }


            public CheckBlockResult CheckBlockBox(string text, int widthLimit, int heightLimit,bool isItemFont)
            {
                if (this.TextEncoding == OptionForm.textencoding_enum.ZH_TBL)
                {//中国語の場合、今のところフォントデータが取れないので何もチェックできない.
                    return CheckBlockResult.NoError;
                }
                if (Program.ROM.RomInfo.is_multibyte)
                {//日本語の場合 (.+?)を消す. (ワイバーンナイト)とか
                    text = RegexCache.Replace(text, @"\(.+?\)", "");
                }
                if (PatchUtil.SearchTextEngineReworkPatch() == PatchUtil.TextEngineRework_enum.TeqTextEngineRework)
                {//Teqのテキストエンジンリワークの場合は、奴が使うコードを消す
                    text = RemoveEscapeTeqTextEngineRework(text);
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
                        else if (this.LintTextSkipBug == OptionForm.lint_text_skip_bug_enum.DetectButExceptForVanilla)
                        {//検出するが、無改造ROMにある、もとからあるものは除く
                            if (IsOrignalBug(blocks[n], n, size))
                            {
                                continue;
                            }
                        }
                        else if (this.LintTextSkipBug == OptionForm.lint_text_skip_bug_enum.MoreThan4Lines)
                        {//4行以上
                            if (size.Height / 16 < 4)
                            {
                                continue;
                            }
                        }
                        else
                        {//すべて検出する.
                        }

                        //除外ケース                      
                        if (this.HasAutoNewLine)
                        {//自動改行が入っている場合は、高さのチェックをしない
                            continue;
                        }
                        else if (this.HasEnable3Line)
                        {//高さ3行までOKの設定が入っているか
                            if (size.Height / 16 <= 3)
                            {//3行までならセーフ
                                continue;
                            }
                        }

                        this.ErrorString = R._("警告:テキストの行数が多すぎます。\r\n想定ドット数({0} , {1})\r\n{2}", size.Width, size.Height, blocks[n]);
                        return CheckBlockResult.ErrorHeight;
                    }
                }
                return CheckBlockResult.NoError;
            }

            bool IsOrignalBug(string str,int n, Size size)
            {
                if (Program.ROM.RomInfo.version == 8)
                {
                    if (Program.ROM.RomInfo.is_multibyte)
                    {
                        if (n == 0 && size.Width==156 && size.Height==48 &&
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
                else if (Program.ROM.RomInfo.version == 7)
                {
                    if (Program.ROM.RomInfo.is_multibyte)
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

        //FE6には、王都奪還のツァイスとゲイルの会話シーンに0バイトの会話文があります。
        //これを無視するためにチェックします
        static bool CheckFE6Glitch15B(uint textid)
        {
            if (Program.ROM.RomInfo.version != 6)
            {
                return false;
            }
            if (textid == 0x15B)
            {
                return true;
            }
            return false;
        }

        //会話テキストのエラーチェック
        public static string CheckConversationTextMessage(string text, uint textid, int widthLimit)
        {
            if (text.Length <= 0)
            {
                if (textid == 0 || textid >= 0xFFFF)
                {
                    return "";
                }
                if (CheckFE6Glitch15B(textid))
                {
                    return "";
                }

                if (widthLimit == TextForm.MAX_DEATH_QUOTE_WIDTH)
                {
                    return R._("警告:\r\n会話文なのに0バイトの文字列が指定されています。\r\n利用しない場合は、TextIDに0を割り当ててください。");
                }
                return R._("警告:\r\n会話文なのに0バイトの文字列が指定されています。");
            }

            text = ConvertEscapeTextRev(text);

            string[] required_words2 = new string[] { "@0003", "@0018", "@0019", "@0017", "@0010", "@0006", "@0007" };///No Translate
            bool found_required2_words = false;
            for (int i = 0; i < required_words2.Length; i++)
            {
                if (text.IndexOf(required_words2[i]) >= 0)
                {
                    found_required2_words = true;
                    break;
                }
            }
            if (!found_required2_words)
            {
                return ConvertEscapeToFEditor(R._("警告:\r\n会話用のテキストなのに@0003等の記号がありません。"));
            }

            string[] required_words = new string[] { "@0008", "@0009", "@000A", "@000B", "@000C", "@000D", "@000E", "@000F", "@0010", "@0080@000A", "@0080@000B", "@0080@000C", "@0080@000D", "@0080@000E", "@0080@000F", "@0080@0010", "@0080@0011", "@0080@0016", "@0080@0018", "@0080@0019", "@0080@001B", "@0080@001C", "@0080@001D", "@0080@001E", "@0018", "@0019" };///No Translate
            bool found_required_words = false;
            for (int i = 0; i < required_words.Length; i++)
            {
                if (text.IndexOf(required_words[i]) >= 0)
                {
                    found_required_words = true;
                    break;
                }
            }
            if (found_required_words == false)
            {
                //システムメッセージみたいなものを出そうとしている?
                return CheckSystemTextMessage(text);
            }

            return CheckParse(text, widthLimit, 16 * 2, false);
        }
        //システムメッセージのエラーチェック
        public static string CheckSystemTextMessage(string text)
        {
            if (text.Length <= 0)
            {
                return "";
            }
            text = ConvertEscapeTextRev(text);

            string[] no_words = new string[] { "@0008", "@0009", "@000A", "@000B", "@000C", "@000D", "@000E", "@000F", "@0010", "@0080@000A", "@0080@000B", "@0080@000C", "@0080@000D", "@0080@000E", "@0080@000F", "@0080@0010", "@0080@0011", "@0080@0016", "@0080@0018", "@0080@0019", "@0080@001B", "@0080@001C", "@0080@001D", "@0080@001E" };///No Translate

            for (int i = 0; i < no_words.Length; i++)
            {
                if (text.IndexOf(no_words[i]) >= 0)
                {
                    return R._("警告:\r\nシステムメッセージなのに{0}があります。", ConvertEscapeText(no_words[i]));
                }
            }

            return CheckParse(text, MAX_SERIF_WIDTH, 16 * 5, false);
        }
        //一行テキストのエラーチェック
        public static string CheckOneLineTextMessage(string text, int width, int height,bool isItemFont)
        {
            if (text.Length <= 0)
            {
                return "";
            }
            text = ConvertEscapeTextRev(text);

            string[] no_words = new string[] { "@0003", "@0002", "@0008", "@0009", "@000A", "@000B", "@000C", "@000D", "@000E", "@000F", "@0010", "@0080" };///No Translate

            for (int i = 0; i < no_words.Length; i++)
            {
                if (text.IndexOf(no_words[i]) >= 0)
                {
                    if (no_words[i] == "@0080" && text.IndexOf("@0080@0020") >= 0)
                    {//軍師の名前
                        continue;
                    }
                    return R._("警告:\r\n{0}行メッセージなのに{1}があります。", height / 16, ConvertEscapeText(no_words[i]));
                }
            }

            return CheckParse(text, width, height, isItemFont);
        }

        private void RefCountTextBox_DoubleClick(object sender, EventArgs e)
        {
            this.TextTabControl.SelectedTab = this.RefPage;
        }

        FELint.Type SelectDataTypeOf = FELint.Type.FELINT_SYSTEM_ERROR;
        void UpdateRef(uint id)
        {
            List<UseValsID> textIDList = Program.AsmMapFileAsmCache.GetVarsIDArray();
            if (textIDList == null)
            {
                RefCountTextBox.Text = R._("計測中...");
                this.SelectDataTypeOf = FELint.Type.FELINT_SYSTEM_ERROR;
                this.RefNotFoundPanel.Hide();
                return;
            }

            RefListBox.BeginUpdate();
            RefListBox.Items.Clear();

            int refCount = 0;
            int count = textIDList.Count;
            for (int i = 0; i < count; i++)
            {
                UseValsID t = textIDList[i];
                if (t.TargetType != UseValsID.TargetTypeEnum.TEXTID
                    || t.ID != id)
                {
                    continue;
                }
                refCount++;
                RefListBox.Items.Add(t);
                this.SelectDataTypeOf = t.DataType;
            }

            {
                UseValsID t = Program.UseTextIDCache.MakeUseTextID(id);
                if (t != null)
                {
                    refCount++;
                    RefListBox.Items.Add(t);
                }
            }

            RefListBox.EndUpdate();
            RefCountTextBox.Text = refCount.ToString();

            if (refCount <= 0 && id > 0)
            {
                this.RefNotFoundPanel.Show();
            }
            else
            {
                this.RefNotFoundPanel.Hide();
            }
        }

        //アドレスリストのテキストを遅延描画
        private Size DrawAddressListTextDelay(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            int lineHeight = (int)lb.Font.Height;
            Rectangle bounds = listbounds;

            if (isWithDraw)
            {
                SolidBrush brush = new SolidBrush(lb.ForeColor);

                string text = index.ToString("X04");
                bounds.X += U.DrawText(text, g, lb.Font, brush, isWithDraw, bounds);
                bounds.X += 5;

                string decodeText = DirectAndStripAllCode((uint)index);
                decodeText = U.nl2space(decodeText); //改行を無視.
                text = U.substr(decodeText, 0, 20);
                bounds.X += U.DrawText(text, g, lb.Font, brush, isWithDraw, bounds);

                brush.Dispose();
            }

            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }

        void GotoRef()
        {
            int index = this.RefListBox.SelectedIndex;
            if (index < 0 || index >= this.RefListBox.Items.Count)
            {
                return;
            }
            UseValsID t = (UseValsID)this.RefListBox.Items[index];
            if (t.DataType == FELint.Type.TEXTID_FOR_USER)
            {
                ShowRefAddDialog();
                return;
            }
            MainSimpleMenuEventErrorForm.GotoEvent(t.DataType, t.Addr, t.Tag, 0);
        }

        private void RefListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GotoRef();
            }
        }

        private void TextArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                SelectEscapeText(sender, e);
            }
        }
        public static string Direct(uint textid)
        {
            string str = FETextDecode.Direct(textid);
            str = str.Replace("@001F", "");
            str = ConvertEscapeText(str);
            return str;
        }
        public static string DirectAndStripAllCode(uint textid)
        {
            return TextForm.StripAllCode(FETextDecode.Direct(textid));
        }

        void SelectEscapeText(Object sender, EventArgs e)
        {
            if ((sender is MenuItem))
            {
                sender = ((MenuItem)sender).GetContextMenu().SourceControl;
            }
            if (!(sender is RichTextBoxEx))
            {
                return;
            }
            RichTextBoxEx editor = (RichTextBoxEx)sender;

            //詳細モードを出してよいかどうか.
            bool isDetail = editor.Name != "TextListSpSerifuTextBox";

            TextScriptFormCategorySelectForm f =
                (TextScriptFormCategorySelectForm)InputFormRef.JumpFormLow<TextScriptFormCategorySelectForm>();
            f.Init(isDetail);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            TextScriptFormCategorySelectForm.TextEscape te = f.Script;
            string insertString = ConvertEscapeText(te.Code);

            //これから更新するので、Undoポイントを作る.
            editor.PushUndo();

            //テキストを変更すると、スクロールしてしまうので、一時的にウィンドウを無効にして固める
            editor.LockWindowUpdate();
            editor.SelectedText = insertString; //更新
            editor.UnLockWindowUpdate();

            //変更したテキストの末尾へ
            editor.SelectionLength = 0;
            editor.SelectionStart += insertString.Length;

            //変更ボタンを光らせる.
            InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
            InputFormRef.WriteButtonToYellow(this.NewButton, true);
            //キーワードハイライトをもう一回行う.
            KeywordHighLight(editor);
        }
        void OnPasteOrUndoOrCutText(Object sender, EventArgs e)
        {
            if (sender is RichTextBoxEx)
            {
                KeywordHighLight((RichTextBoxEx)sender);
            }
        }

        private void TextForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TextToSpeechForm.Stop();
        }

        public static uint GetTextIDToDataAddr(uint textID)
        {
            InputFormRef InputFormRef = Init(null);
            uint write_pointer = InputFormRef.BaseAddress + (InputFormRef.BlockSize * textID);
            uint write_addr = Program.ROM.u32(write_pointer);

            if (FETextEncode.IsUnHuffmanPatchPointer(write_addr))
            {
                write_addr = FETextEncode.ConvertUnHuffmanPatchToPointer(write_addr);
            }
            return U.toOffset(write_addr);
        }
        public static uint GetTextIDToDataPointer(uint textID)
        {
            InputFormRef InputFormRef = Init(null);
            uint write_pointer = InputFormRef.BaseAddress + (InputFormRef.BlockSize * textID);

            return write_pointer;
        }



        private void SearcFreeArea_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            List<UseValsID> textIDList = Program.AsmMapFileAsmCache.GetVarsIDArray();
            if (textIDList == null)
            {
                R.ShowStopError("現在、参照されているテキストを分岐中です。\r\n分岐処理が終わってから再度実行してください。");
                return;
            }
            Program.UseTextIDCache.AppendList(textIDList);

            string lang = OptionForm.lang();
            bool isJP = (lang == "ja");

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                Dictionary<uint, bool> textmap = UseValsID.ConvertMaps(textIDList);

                FETextDecode textdecoder = new FETextDecode();
                uint id = 0;
                List<U.AddrResult> result = InputFormRef.MakeList((U.AddrResult ar) =>
                {
                    if ( textmap.ContainsKey(id++))
                    {//HIT
                        return new U.AddrResult();
                    }
                    
                    int size;
                    string str = textdecoder.DecodeAddr(ar.addr, out size);
                    str = StripAllCode(str);
                    string name;
                    if (str.Length >= 40)
                    {
                        name = ar.name + " " + str.Substring(0, 40) + "...";
                    }
                    else
                    {
                        name = ar.name + " " + str;
                    }

                    Debug.Print(name.Replace(" ","\t").Replace("\r\n"," ") + "\t{J}");
                    return new U.AddrResult(ar.addr, name, ar.tag);
                });

                SearchResultListBox.Items.Clear();

                U.ConvertListBox(result, ref this.SearchResultListBox);
            }
        }

        private void AddRefButton_Click(object sender, EventArgs e)
        {
            ShowRefAddDialog();
        }
        void ShowRefAddDialog()
        {
            string text = GetEditorText(this.TextArea);
            uint textid = (uint)AddressList.SelectedIndex;

            TextRefAddDialogForm f = (TextRefAddDialogForm)InputFormRef.JumpFormLow<TextRefAddDialogForm>();
            f.Init(textid , text);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            Program.UseTextIDCache.Update(textid, f.GetComment());
            UpdateRef(textid);
        }

        void JumpFromSearchResult()
        {
            int id = (int)U.atoh(SearchResultListBox.Text);
            if (id < AddressList.Items.Count)
            {
                U.ForceUpdate(AddressList, id);
            }
        }

        private void SearchResultListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            JumpFromSearchResult();
        }
        private void SearchResultListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                JumpFromSearchResult();
            }
        }

        private void SearchResultListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            JumpFromSearchResult();
        }

        private void RefListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GotoRef();
        }

        private void TextListSpShowCharPictureBox_Click(object sender, EventArgs e)
        {
            TextListSpShowCharLabel_Click(sender,e);
        }

        private void TextListSpShowCharLabel_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version == 6)
            {
                InputFormRef.JumpForm<ImagePortraitFE6Form>((uint)TextListSpShowCharNumericUpDown.Value, "AddressList", TextListSpShowCharNumericUpDown);
            }
            else
            {
                InputFormRef.JumpForm<ImagePortraitForm>((uint)TextListSpShowCharNumericUpDown.Value, "AddressList", TextListSpShowCharNumericUpDown);
            }
        }

        private void TextListSpShowCharNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint portraitID = (uint)TextListSpShowCharNumericUpDown.Value;
            if (portraitID == 0xFFFF)
            {
                this.TextListSpShowCharText.Text = R._("FFFF訪問したキャラ");
            }
            else
            {
                this.TextListSpShowCharText.Text = U.ToHexString(portraitID) + " " + ImagePortraitForm.GetPortraitName(portraitID);
            }

            TextListSpShowCharPictureBox.Image =
                ImagePortraitForm.DrawPortraitAuto(portraitID);
        }

        static uint GetOneCharOrAtCode(string text,ref int ref_i)
        {
            if (ref_i >= text.Length)
            {
                return 0;
            }
            if (text[ref_i] == '@')
            {
                uint code = U.atoh(U.substr(text, ref_i + 1, 4));
                ref_i += 5;
                return code;
            }
            else if (text[ref_i] == '\r' && text[ref_i+1] == '\n')
            {
                uint code = 1;
                ref_i+=2;
                return code;
            }
            else
            {
                uint code = text[ref_i];
                ref_i++;
                return code;
            }
        }

        static string SubStrAndEscape(string text, int start , int length)
        {
            string rettext = U.substr(text, start, length);
            if (OptionForm.text_escape() == OptionForm.text_escape_enum.FEditorAdv)
            {
                return ConvertEscapeToFEditor(rettext);
            }
            return rettext;
        }
        static string MakeCodeAndEscape(uint code, OptionForm.text_escape_enum escape_enum)
        {
            if (escape_enum == OptionForm.text_escape_enum.FEditorAdv)
            {
                return "[0x00" + U.ToHexString2(code) + "]";
            }
            return "@00" + U.ToHexString2(code);
        }

        //Teqのreworkは、既存ルールと重複しているのがあるので、いい感じに調整する.
        static string ConvertTeqTextEngineRework(string text, OptionForm.text_escape_enum escape_enum)
        {
            StringBuilder sb = new StringBuilder();

            int text_stsrt = 0;
            int len = text.Length;
            for (int i = 0; i < len; )
            {
                uint code1 = GetOneCharOrAtCode(text, ref i);
                if (code1 != 0x80)
                {
                    continue;
                }

                uint code2 = GetOneCharOrAtCode(text, ref i);
                if (code2 == 0x26 || (code2 >= 0x28 && code2 <= 0x2C) || (code2 >= 0x30 && code2 <= 0x38))
                {
                    string block = SubStrAndEscape(text, text_stsrt, i - text_stsrt);
                    sb.Append(block);

                    uint code3 = GetOneCharOrAtCode(text, ref i);
                    if (code3 != 0)
                    {
                        sb.Append(MakeCodeAndEscape(code3, escape_enum));
                    }
                    text_stsrt = i;

                }
                else if (code2 == 0x27 || code2 == 0x2E)
                {
                    string block = SubStrAndEscape(text, text_stsrt, i - text_stsrt);
                    sb.Append(block);

                    uint code3 = GetOneCharOrAtCode(text, ref i);
                    uint code4 = GetOneCharOrAtCode(text, ref i);
                    if (code3 != 0 && code4 != 0)
                    {
                        sb.Append(MakeCodeAndEscape(code3, escape_enum));
                        sb.Append(MakeCodeAndEscape(code4, escape_enum));
                    }
                    text_stsrt = i;
                }
                else if (code2 == 0x2D)
                {
                    string block = SubStrAndEscape(text, text_stsrt, i - text_stsrt);
                    sb.Append(block);

                    uint code3 = GetOneCharOrAtCode(text, ref i);
                    uint code4 = GetOneCharOrAtCode(text, ref i);
                    uint code5 = GetOneCharOrAtCode(text, ref i);
                    uint code6 = GetOneCharOrAtCode(text, ref i);
                    if (code3 != 0 && code4 != 0 && code5 != 0 && code6 != 0)
                    {
                        sb.Append(MakeCodeAndEscape(code3, escape_enum));
                        sb.Append(MakeCodeAndEscape(code4, escape_enum));
                        sb.Append(MakeCodeAndEscape(code5, escape_enum));
                        sb.Append(MakeCodeAndEscape(code6, escape_enum));
                    }
                    text_stsrt = i;
                }
                else if (code2 == 0x2F)
                {
                    string block = SubStrAndEscape(text, text_stsrt, i - text_stsrt);
                    sb.Append(block);

                    uint code3 = GetOneCharOrAtCode(text, ref i);
                    uint code4 = GetOneCharOrAtCode(text, ref i);
                    uint code5 = GetOneCharOrAtCode(text, ref i);
                    uint code6 = GetOneCharOrAtCode(text, ref i);
                    uint code7 = GetOneCharOrAtCode(text, ref i);
                    uint code8 = GetOneCharOrAtCode(text, ref i);
                    if (code3 != 0 && code4 != 0 && code5 != 0 && code6 != 0 && code7 != 0 && code8 != 0)
                    {
                        sb.Append(MakeCodeAndEscape(code3, escape_enum));
                        sb.Append(MakeCodeAndEscape(code4, escape_enum));
                        sb.Append(MakeCodeAndEscape(code5, escape_enum));
                        sb.Append(MakeCodeAndEscape(code6, escape_enum));
                        sb.Append(MakeCodeAndEscape(code7, escape_enum));
                        sb.Append(MakeCodeAndEscape(code8, escape_enum));
                    }
                    text_stsrt = i;
                }
            }
            //最後っ屁
            {
                string block = SubStrAndEscape(text, text_stsrt , text.Length - text_stsrt);
                sb.Append(block);
            }

            return sb.ToString();
        }
        static bool CheckTextEngineRework_ParseTextList(uint code2, string srctext , ref int next_i)
        {
            if (PatchUtil.SearchTextEngineReworkPatch() != PatchUtil.TextEngineRework_enum.TeqTextEngineRework)
            {
                return false;
            }

            if (code2 == 0x26 || (code2 >= 0x28 && code2 <= 0x2C) || (code2 >= 0x30 && code2 <= 0x38))
            {
                if (next_i + 5 > srctext.Length || srctext[next_i] != '@')
                {
                    return false;
                }
                uint code3 = U.atoh(U.substr(srctext, next_i + 1, 4));
                next_i += 5;
            }
            else if (code2 == 0x27 || code2 == 0x2E)
            {
                if (next_i + 10 > srctext.Length || srctext[next_i] != '@' || srctext[next_i + 5] != '@')
                {
                    return false;
                }
                next_i += 10;
            }
            else if (code2 == 0x2D)
            {
                if (next_i + 20 > srctext.Length || srctext[next_i] != '@' || srctext[next_i + 5] != '@' || srctext[next_i + 10] != '@' || srctext[next_i + 15] != '@')
                {
                    return false;
                }
                next_i += 20;
            }
            else if (code2 == 0x2F)
            {
                if (next_i + 30 > srctext.Length || srctext[next_i] != '@' || srctext[next_i + 5] != '@' || srctext[next_i + 10] != '@' || srctext[next_i + 15] != '@' || srctext[next_i + 20] != '@' || srctext[next_i + 25] != '@')
                {
                    return false;
                }
                next_i += 30;
            }
            else
            {
                return false;
            }

            return true;
        }

        private void AddressList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'F')
            {
                TextTabControl.SelectedTab = SearchTabPage;
                SearchTextBox.Focus();
            }
        }


    }
}
