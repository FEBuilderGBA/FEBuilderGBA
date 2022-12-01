using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class TextCharCodeForm : Form
    {
        public TextCharCodeForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.PostWriteHandler += PostWriteHandler;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.mask_pointer
                , 4
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 255;
                }
                , (int i, uint addr) =>
                {
                    uint c = Program.ROM.u16(addr);

                    string str = Program.ROM.getString(addr, 2);
                    if (!Program.ROM.RomInfo.is_multibyte)
                    {
                        if (c >= 0x81 && c <= 0xFF)
                        {//英語版FEにはUnicodeの1バイトだけ表記があるらしい.
                            str = "@00" + c.ToString("X02");
                        }
                    }

                    str = FETextEncode.RevConvertSPMoji(str);
                    return i.ToString("X04") + " " + c.ToString("X04") + " " + str;
                }
                );
        }

        private void TextCharCodeForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addr = InputFormRef.SelectToAddr(this.AddressList);
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();

            //アイテム
            ItemFontPictureBox.Image = FontForm.DrawFont(Program.ROM.u16(addr), true, priorityCode);

            //セリフ
            SerifFontPictureBox.Image = FontForm.DrawFont(Program.ROM.u16(addr), false, priorityCode);
        }


        private void SEARCH_CHAR_BUTTON_Click(object sender, EventArgs e)
        {
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();
            uint code = U.ConvertMojiCharToUnit(SEARCH_CHAR.Text, priorityCode);

            //リストから検索して選択する
            uint addr = (uint)InputFormRef.BaseAddress;
            int limitsize = (int)InputFormRef.DataCount;
            for (int i = 0; i < limitsize; i++)
            {
                if ( Program.ROM.u16(addr) == code )
                {
                    AddressList.SelectedIndex = i;
                    break;
                }
                addr += InputFormRef.BlockSize;
            }
        }

        static Dictionary<uint, FETextDecode.huffman_count_st> GetUsedCharDic(List<U.AddrResult> list, InputFormRef ifr)
        {
            Dictionary<uint, FETextDecode.huffman_count_st> dic = new Dictionary<uint, FETextDecode.huffman_count_st>();
            //初期値を入れる.
            uint addr = (uint)ifr.BaseAddress;
            int limitsize = (int)ifr.DataCount;
            for (int i = 0; i < limitsize; i++)
            {
                FETextDecode.huffman_count_st st = new FETextDecode.huffman_count_st();
                st.code_number = i;
                dic[Program.ROM.u16(addr)] = st;
                addr += ifr.BlockSize;
            }

            //テキスト探索.
            FETextDecode textdecoder = new FETextDecode();
            for (int i = 0; i < list.Count; i++)
            {
                uint textaddr = Program.ROM.p32(list[i].addr);
                textdecoder.huffman_count(textaddr, ref dic);
            }
            return dic;
        }

        private void SEARCH_COUNT_BUTTON_Click(object sender, EventArgs e)
        {
            List<U.AddrResult> list = TextForm.MakeItemList();
            Dictionary<uint, FETextDecode.huffman_count_st> dic = GetUsedCharDic(list, InputFormRef);

            SEARCH_COUNT_LIST.BeginUpdate();
            SEARCH_COUNT_LIST.Items.Clear();
            uint minimaumCount = (uint)SEARCH_COUNT.Value;
            foreach(var pair in dic)
            {
                if (pair.Key == 0)
                {
                    continue;
                }
                if (pair.Value.count > minimaumCount)
                {
                    continue;
                }
                string line = pair.Value.code_number.ToString();

                line = line + "," + U.ConvertUnitToMojiChar((uint)pair.Key);
                line = line + "," + pair.Value.count.ToString();

                for (int n = 0; n < pair.Value.unsing_text_addr.Count; n++)
                {
                    for(int i = 0 ; i < list.Count ; i++ )
                    {
                        uint textaddr = Program.ROM.p32(list[i].addr);
                        if (pair.Value.unsing_text_addr[n] == textaddr)
                        {
                            line = line + "," + i.ToString("X");
                            break;
                        }
                    }
                }

                SEARCH_COUNT_LIST.Items.Add(line);
            }
            SEARCH_COUNT_LIST.EndUpdate();
        }



        private void SEARCH_COUNT_LIST_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] sp = SEARCH_COUNT_LIST.Text.Split(',');
            if (sp.Length > 2)
            {
                SEARCH_CHAR.Text = sp[1];
                SEARCH_CHAR_BUTTON.PerformClick();
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "TextCharCode";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }

        void PostWriteHandler(object sender, EventArgs e)
        {
            Program.ReBuildFETextEncoder();
        }

        public static void RebuildAllData(ToolTextCharRecreate recreate, Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = Init(null);
            List<ToolTextCharRecreate.CharCounter> list = recreate.GetSortedList();

            int i = 0;
            uint addr = InputFormRef.BaseAddress;
            uint length = addr + (4 * (InputFormRef.DataCount));
            for (; addr < length; addr += 4)
            {
                uint a = Program.ROM.u16(addr);
                if (a <= 0x300)
                {
                    continue;
                }
                Program.ROM.write_u16(addr, list[i].mojiBin, undodata);
                i++;
                if (i >= list.Count)
                {
                    break;
                }
            }
            Program.ReBuildFETextEncoder();
        }

        public static void RebuildEmpty(ToolTextCharRecreate recreate, Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = Init(null);
            List<ToolTextCharRecreate.CharCounter> list = recreate.GetSortedList();

            List<U.AddrResult> stringList = TextForm.MakeItemList();
            Dictionary<uint, FETextDecode.huffman_count_st> dic = GetUsedCharDic(stringList, InputFormRef);

            int i = 0;
            foreach (var pair in dic)
            {
                if (pair.Key == 0)
                {
                    continue;
                }
                if (pair.Value.count > 0)
                {//一度でも使われているものは除外
                    continue;
                }

                uint addr = (uint)(pair.Value.code_number * 4) + (InputFormRef.BaseAddress);

                //uint a = Program.ROM.u16(addr);
                Program.ROM.write_u16(addr, list[i].mojiBin, undodata);
                i++;
                if (i >= list.Count)
                {
                    break;
                }
            }
            Program.ReBuildFETextEncoder();
        }
    }
}
