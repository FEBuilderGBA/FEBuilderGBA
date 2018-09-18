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
    public partial class AITargetForm : Form
    {
        public AITargetForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ai3_pointer()
                , 20
                , (int i, uint addr) =>
                {
                    return i < 8;
                }
                , (int i, uint addr) =>
                {
                    switch (i)
                    {
                        case 0:
                        default:
                            return R._("標的AI00");
                        case 1:
                            return R._("標的AI08");
                        case 2:
                            return R._("標的AI10");
                        case 3:
                            return R._("標的AI18");
                        case 4:
                            return R._("標的AI20");
                        case 5:
                            return R._("標的AI28");
                        case 6:
                            return R._("標的AI30");
                        case 7:
                            return R._("標的AI38");
                    }
                }
                );
        }

        private void AITargetForm_Load(object sender, EventArgs e)
        {

        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "AI3", new uint[] { });
        }
    }
}
