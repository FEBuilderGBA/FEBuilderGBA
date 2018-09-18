using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class TextDicForm : Form
    {
        public TextDicForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.N1_InputFormRef = N1_Init(this);
            this.N2_InputFormRef = N2_Init(this);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N2_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {//辞書
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.dic_main_pointer()
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint text1 = Program.ROM.u16(addr + 2);
                    uint text2 = Program.ROM.u16(addr + 4);
                    if (text1 <= 0 || text2 <= 0)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint text1 = Program.ROM.u16(addr + 2);
                    return U.ToHexString(i) + " " + TextForm.Direct(text1);
                }
                );
        }

        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {//章タイトル
            return new InputFormRef(self
                , "N1_"
                , Program.ROM.RomInfo.dic_chaptor_pointer()
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < 9;
                }
                , (int i, uint addr) =>
                {
                    uint text1 = Program.ROM.u16(addr + 0);
                    return U.ToHexString(i) + " " + TextForm.Direct(text1);
                }
                );
        }

        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {//拠点ごとの確率
            return new InputFormRef(self
                , "N2_"
                , Program.ROM.RomInfo.dic_title_pointer()
                , 2
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < 12;
                }
                , (int i, uint addr) =>
                {
                    uint text1 = Program.ROM.u16(addr + 0);
                    return U.ToHexString(i) + " " + TextForm.Direct(text1);
                }
                );
        }



        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "dic_main", new uint[] { });
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "dic_chaptor", new uint[] { });
            }
            {
                InputFormRef InputFormRef = N2_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "dic_title", new uint[] { });
            }
        }


        private void TextDicForm_Load(object sender, EventArgs e)
        {
        }

        private void B0_ValueChanged(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(N2_AddressList, (uint)B0.Value);
            X_1.Text = N2_AddressList.Text;
        }

        private void B1_ValueChanged(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(N1_AddressList, (uint)B1.Value);
            X_2.Text = N1_AddressList.Text;
        }


        public static void MakeTextIDArray(List<TextID> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                TextID.AppendTextID(list, FELint.Type.DIC, InputFormRef, new uint[] { 2,4 });
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                TextID.AppendTextID(list, FELint.Type.DIC, InputFormRef, new uint[] { 0 });
            }
            {
                InputFormRef InputFormRef = N2_Init(null);
                TextID.AppendTextID(list, FELint.Type.DIC, InputFormRef, new uint[] { 0 });
            }
        }
        

    }
}
