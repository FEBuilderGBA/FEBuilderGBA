using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class TacticianAffinityFE7 : Form
    {
        public TacticianAffinityFE7()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawAtributeAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.tactician_affinity_pointer()
                , 0x4
                , (int i, uint addr) =>
                {
                    if (Program.ROM.RomInfo.is_multibyte())
                    {
                        return i < 48;
                    }
                    else
                    {
                        return i < 12;
                    }
                }
                , (int i, uint addr) =>
                {
                    uint affinity_id = Program.ROM.u32(addr + 0);

                    return U.ToHexString(affinity_id) + " " + InputFormRef.GetAFFILIATION(affinity_id);
                }
                );
            return ifr;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "TacticianAffinity", new uint[] { });
        }

        private void TacticianAffinityFE7_Load(object sender, EventArgs e)
        {

        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint select = (uint)this.AddressList.SelectedIndex;

            if (Program.ROM.RomInfo.is_multibyte())
            {
                uint birth = select / 4;
                uint blood_type = select - (birth * 4);
                string l = R._("誕生月") + ":" + InputFormRef.GetMonthName(birth) + " " + R._("血液型") + ":" + InputFormRef.GetBloodType(blood_type);
                this.Explain.Text = l;
            }
            else
            {
                uint birth = select;
                string l = R._("誕生月") + ":" + InputFormRef.GetMonthName(birth);
                this.Explain.Text = l;
            }

        }
    }
}
