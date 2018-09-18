using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MonsterItemForm : Form
    {
        public MonsterItemForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.N1_InputFormRef = N1_Init(this);
            this.N2_InputFormRef = N2_Init(this);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N2_InputFormRef.MakeGeneralAddressListContextMenu(true);


            N1_B0.ValueChanged += N1_AddressList_SelectedIndexChanged;
            N1_B1.ValueChanged += N1_AddressList_SelectedIndexChanged;
            N1_B2.ValueChanged += N1_AddressList_SelectedIndexChanged;
            N1_B3.ValueChanged += N1_AddressList_SelectedIndexChanged;
            N1_B4.ValueChanged += N1_AddressList_SelectedIndexChanged;
            N2_B11.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B12.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B13.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B14.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B15.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B16.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B17.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B18.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B19.ValueChanged += N2_AddressList_SelectedIndexChanged;
            N2_B20.ValueChanged += N2_AddressList_SelectedIndexChanged;


            N2_B1.Enter += JumpToItemSelect;
            N2_B1.ValueChanged += JumpToItemSelect;
            N2_B2.Enter += JumpToItemSelect;
            N2_B2.ValueChanged += JumpToItemSelect;
            N2_B3.Enter += JumpToItemSelect;
            N2_B3.ValueChanged += JumpToItemSelect;
            N2_B4.Enter += JumpToItemSelect;
            N2_B4.ValueChanged += JumpToItemSelect;
            N2_B5.Enter += JumpToItemSelect;
            N2_B5.ValueChanged += JumpToItemSelect;
            N2_B6.Enter += JumpToItemSelect;
            N2_B6.ValueChanged += JumpToItemSelect;
            N2_B7.Enter += JumpToItemSelect;
            N2_B7.ValueChanged += JumpToItemSelect;
            N2_B8.Enter += JumpToItemSelect;
            N2_B8.ValueChanged += JumpToItemSelect;
            N2_B9.Enter += JumpToItemSelect;
            N2_B9.ValueChanged += JumpToItemSelect;
            N2_B10.Enter += JumpToItemSelect;
            N2_B10.ValueChanged += JumpToItemSelect;

            N2_B21.Enter += JumpToProbabilitySelect;
            N2_B21.ValueChanged += JumpToProbabilitySelect;
            N2_B22.Enter += JumpToProbabilitySelect;
            N2_B22.ValueChanged += JumpToProbabilitySelect;
            N2_B23.Enter += JumpToProbabilitySelect;
            N2_B23.ValueChanged += JumpToProbabilitySelect;
            N2_B24.Enter += JumpToProbabilitySelect;
            N2_B24.ValueChanged += JumpToProbabilitySelect;
            N2_B25.Enter += JumpToProbabilitySelect;
            N2_B25.ValueChanged += JumpToProbabilitySelect;
            N2_B26.Enter += JumpToProbabilitySelect;
            N2_B26.ValueChanged += JumpToProbabilitySelect;
            N2_B27.Enter += JumpToProbabilitySelect;
            N2_B27.ValueChanged += JumpToProbabilitySelect;
            N2_B28.Enter += JumpToProbabilitySelect;
            N2_B28.ValueChanged += JumpToProbabilitySelect;
            N2_B29.Enter += JumpToProbabilitySelect;
            N2_B29.ValueChanged += JumpToProbabilitySelect;
            N2_B30.Enter += JumpToProbabilitySelect;
            N2_B30.ValueChanged += JumpToProbabilitySelect;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {//魔物所持アイテム アイテム確率
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.monster_item_item_pointer()
                , 5
                , (int i, uint addr) =>
                {//読込最大値検索
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {//魔物所持アイテム アイテム確率
            return new InputFormRef(self
                , "N1_"
                , Program.ROM.RomInfo.monster_item_probability_pointer()
                , 5
                , (int i, uint addr) =>
                {//読込最大値検索
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr);
                }
                );
        }


        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {//魔物所持アイテム アイテム確率
            return new InputFormRef(self
                , "N2_"
                , Program.ROM.RomInfo.monster_item_table_pointer()
                , 32
                , (int i, uint addr) =>
                {//読込最大値検索
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    uint class_id = Program.ROM.u8(addr);
                    return U.ToHexString(i) + U.SA(ClassForm.GetClassName(class_id)) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void MonsterItemForm_Load(object sender, EventArgs e)
        {

        }

        private void JumpToItemSelect(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(NumericUpDown))
            {
                return;
            }
            NumericUpDown a = (NumericUpDown)sender;
            if (!a.Focused)
            {
                return;
            }
            if (a.Value <= 0)
            {
                this.AddressList.SelectedIndex = -1;
                return;
            }
            if (a.Value - 1 > this.AddressList.Items.Count)
            {
                this.AddressList.SelectedIndex = -1;
                return;
            }
            this.AddressList.SelectedIndex = (int)(a.Value );
        }
        private void JumpToProbabilitySelect(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(NumericUpDown))
            {
                return;
            }
            NumericUpDown a = (NumericUpDown)sender;
            if (!a.Focused)
            {
                return;
            }
            if (a.Value <= 0)
            {
                this.N1_AddressList.SelectedIndex = -1;
                return;
            }
            if (a.Value - 1 > this.N1_AddressList.Items.Count)
            {
                this.N1_AddressList.SelectedIndex = -1;
                return;
            }
            this.N1_AddressList.SelectedIndex = (int)(a.Value );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                string name = "MonsterItemForm";
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            }
            {
                string name = "MonsterItemFormProbability";
                InputFormRef InputFormRef = N1_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            }
            {
                string name = "MonsterItemFormTable";
                InputFormRef InputFormRef = N2_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            }
        }

        private void N1_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sum = (int)N1_B0.Value + (int)N1_B1.Value + (int)N1_B2.Value + (int)N1_B3.Value + (int)N1_B4.Value;
            N1_SUM.Text = sum.ToString() + "%";
        }

        private void N2_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sum;
            sum = (int)N2_B11.Value + (int)N2_B12.Value + (int)N2_B13.Value + (int)N2_B14.Value + (int)N2_B15.Value;
            N2_SUM1.Text = sum.ToString() + "%";

            sum = (int)N2_B16.Value + (int)N2_B17.Value + (int)N2_B18.Value + (int)N2_B19.Value + (int)N2_B20.Value;
            N2_SUM2.Text = sum.ToString() + "%";
        }

    }
}
