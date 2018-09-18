using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MonsterWMapProbabilityForm : Form
    {
        public MonsterWMapProbabilityForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.N1_InputFormRef = N1_Init(this);
            this.N2_InputFormRef = N2_Init(this);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N2_InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.N1_Filter.SelectedIndex = 0;
            this.N2_Filter.SelectedIndex = 0;

            N2_BaseNameUpdate(null, null);
            WriteButton.Click += N2_BaseNameUpdate;

            N2_B0.ValueChanged += N2_SUM;
            N2_B1.ValueChanged += N2_SUM;
            N2_B2.ValueChanged += N2_SUM;
            N2_B3.ValueChanged += N2_SUM;
            N2_B4.ValueChanged += N2_SUM;
            N2_B5.ValueChanged += N2_SUM;
            N2_B6.ValueChanged += N2_SUM;
            N2_B7.ValueChanged += N2_SUM;
            N2_B8.ValueChanged += N2_SUM;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {//魔物が発生する拠点
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.monster_wmap_base_point_pointer()
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < 0x9;
                }
                , (int i, uint addr) =>
                {
                    uint baseID = Program.ROM.u8(addr);
                    return U.ToHexString(i) + " " + WorldMapPointForm.GetWorldMapPointName(baseID);
                }
                );
        }

        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {//進行
            return new InputFormRef(self
                , "N1_"
                , 0
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < 0xB;
                }
                , (int i, uint addr) =>
                {
                    uint mapID = Program.ROM.u8(addr);
                    return U.ToHexString(i) + " " + MapSettingForm.GetMapName(mapID);
                }
                );
        }

        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {//拠点ごとの確率
            return new InputFormRef(self
                , "N2_"
                , 0 
                , 9
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < 0xB;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) ;
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
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MonsterWMapProbability", new uint[] { });
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_stage_1_pointer()));
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MonsterWMapStageEirika", new uint[] { });

                InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_stage_2_pointer()));
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MonsterWMapStageEphraim", new uint[] { });
            }
            {
                InputFormRef InputFormRef = N2_Init(null);
                InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_probability_1_pointer()));
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MonsterWMapProbabilityEirika", new uint[] { });

                InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_probability_2_pointer()));
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MonsterWMapProbabilityEphraim", new uint[] { });
            }
        }

        void N2_BaseNameUpdate(object sender,EventArgs e)
        {
            List<U.AddrResult> list = InputFormRef.MakeList();
            if (list.Count <= 8)
            {
                return ;
            }
            N2_J_0.Text = list[0].name;
            N2_J_1.Text = list[1].name;
            N2_J_2.Text = list[2].name;
            N2_J_3.Text = list[3].name;
            N2_J_4.Text = list[4].name;
            N2_J_5.Text = list[5].name;
            N2_J_6.Text = list[6].name;
            N2_J_7.Text = list[7].name;
            N2_J_8.Text = list[8].name;
        }

        void N2_SUM(object sender, EventArgs e)
        {
            uint sum = 
                ((uint)N2_B0.Value) + 
                ((uint)N2_B1.Value) + 
                ((uint)N2_B2.Value) + 
                ((uint)N2_B3.Value) + 
                ((uint)N2_B4.Value) + 
                ((uint)N2_B5.Value) + 
                ((uint)N2_B6.Value) + 
                ((uint)N2_B7.Value) + 
                ((uint)N2_B8.Value) 
                ;
            this.SUM.Text = sum.ToString() + "%";
        }

        private void N1_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == N1_AddressList)
            {
                U.SelectedIndexSafety(N2_AddressList, N1_AddressList.SelectedIndex);
            }
        }

        private void N2_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == N2_AddressList)
            {
                U.SelectedIndexSafety(N1_AddressList, N2_AddressList.SelectedIndex);
            }
        }

        private void N1_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (N1_Filter.SelectedIndex == 0)
            {
                N1_InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_stage_1_pointer()));
            }
            else
            {
                N1_InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_stage_2_pointer()));
            }

            if (sender == N1_Filter)
            {
                U.SelectedIndexSafety(N2_Filter, N1_Filter.SelectedIndex);
            }
        }

        private void N2_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (N2_Filter.SelectedIndex == 0)
            {
                N2_InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_probability_1_pointer()));
            }
            else if (N2_Filter.SelectedIndex == 1)
            {
                N2_InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_probability_2_pointer()));
            }
//            else if (N2_Filter.SelectedIndex == 2)
//            {
//                N2_InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_probability_after_1_pointer()), 1);
//            }
//            else //if (N2_Filter.SelectedIndex == 3)
//            {
//                N2_InputFormRef.ReInitPointer((Program.ROM.RomInfo.monster_wmap_probability_after_2_pointer()) , 1);
//            }
            if (sender == N2_Filter)
            {
                U.SelectedIndexSafety(N1_Filter, N2_Filter.SelectedIndex);
            }
        }

        

    }
}
