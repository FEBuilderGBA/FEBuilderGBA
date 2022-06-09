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
    public partial class AIStealItemForm : Form
    {
        public AIStealItemForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ai_steal_item_pointer
                , 2
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    uint item_id = Program.ROM.u8(addr);
                    return U.ToHexString(item_id) + " " + ItemForm.GetItemName(item_id);
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
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "AIStealItem", new uint[] { });
        }
    }
}
