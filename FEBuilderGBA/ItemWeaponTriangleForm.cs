using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemWeaponTriangleForm : Form
    {
        public ItemWeaponTriangleForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawWeaponTypeIcon2AndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.item_cornered_pointer()
                , 4
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 255;
                }
                , (int i, uint addr) =>
                {
                    uint weapon1 = Program.ROM.u8(addr);
                    uint weapon2 = Program.ROM.u8(addr+1);
                    return U.ToHexString(weapon1)  + " " + InputFormRef.GetWeaponTypeName(weapon1)
                        + " -> " + U.ToHexString(weapon2)  + " " + InputFormRef.GetWeaponTypeName(weapon2)
                        ;
                }
                );
        }

        private void ItemWeaponTriangleForm_Load(object sender, EventArgs e)
        {
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "ItemWeaponTriangle";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
    }
}
