using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MonsterProbabilityForm : Form
    {
        public MonsterProbabilityForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            B5.ValueChanged += AddressList_SelectedIndexChanged;
            B6.ValueChanged += AddressList_SelectedIndexChanged;
            B7.ValueChanged += AddressList_SelectedIndexChanged;
            B8.ValueChanged += AddressList_SelectedIndexChanged;
            B9.ValueChanged += AddressList_SelectedIndexChanged;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.monster_probability_pointer()
                , 12
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

        private void MonsterProbabilityForm_Load(object sender, EventArgs e)
        {

        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sum = (int)B5.Value + (int)B6.Value + (int)B7.Value + (int)B8.Value + (int)B9.Value;
            SUM.Text = sum.ToString() + "%";
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                string name = "MonsterProbabilityForm";
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            }
        }
    }
}
