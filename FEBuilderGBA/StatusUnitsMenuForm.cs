using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class StatusUnitsMenuForm : Form
    {
        public StatusUnitsMenuForm()
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
                , Program.ROM.RomInfo.status_units_menu_pointer
                , 16
                , (int i, uint addr) =>
                {
                    uint order = Program.ROM.u32(addr + 0);
                    return order < 0xFF;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + GetNameFast(addr);
                }
                );
        }

        static string GetNameFast(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint textid = Program.ROM.u32(addr + 4);
            return TextForm.DirectAndStripAllCode(textid);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            string name = "UnitsMenu";
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.STATUS_UNITS_MENU, InputFormRef, new uint[] { 4, 12 });
        }


        private void StatusUnitsMenuForm_Load(object sender, EventArgs e)
        {

        }
    }
}
