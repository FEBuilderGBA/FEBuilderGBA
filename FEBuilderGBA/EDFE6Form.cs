using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EDFE6Form : Form
    {
        public EDFE6Form()
        {
            InitializeComponent();

            this.N2_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.N2_InputFormRef = N2_Init(this);

            this.N2_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }


        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {
            return new InputFormRef(self
                , "N2_"
                , Program.ROM.RomInfo.ed_3a_pointer
                , 8
                , (int i, uint addr) =>
                {
                    if (i >= 0x42)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint uid1 = (uint)i;
                    return U.ToHexString(uid1) + " " + UnitForm.GetUnitName(uid1);
                }
                );
        }

        private void EDFE6Form_Load(object sender, EventArgs e)
        {
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "EDFE6Form";
            {
                InputFormRef InputFormRef = N2_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            {
                InputFormRef InputFormRef = N2_Init(null);
                UseValsID.AppendTextID(list, FELint.Type.ED, InputFormRef, new uint[] { 0 ,2 , 4, 6 });
            }
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            {
                InputFormRef ifr = N2_Init(null);

                uint addr = ifr.BaseAddress;
                for (uint i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
                {
                    uint name = Program.ROM.u16(addr + 0);
                    FELint.CheckText(name, "FE6EDAFTER6", errors, FELint.Type.ED, addr, i);

                    name = Program.ROM.u16(addr + 2);
                    FELint.CheckText(name, "FE6EDAFTER6", errors, FELint.Type.ED, addr, i);

                    name = Program.ROM.u16(addr + 6);
                    FELint.CheckText(name, "EDTITLE1", errors, FELint.Type.ED, addr, i);
                }
            }
        }
    }
}
