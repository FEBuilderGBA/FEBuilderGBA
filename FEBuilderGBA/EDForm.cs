using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EDForm : Form
    {
        public EDForm()
        {
            InitializeComponent();
            SetPopupHelp();
            this.N2_FilterComboBox.SelectedIndex = 0;

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.N1_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.N2_AddressList.OwnerDraw(ListBoxEx.DrawUnit2AndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.N1_InputFormRef = N1_Init(this);
            this.N2_InputFormRef = N2_Init(this);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N2_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ed_1_pointer
                , 4
                , (int i, uint addr) =>
                {
                    return Program.ROM.u32(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint uid = Program.ROM.u8(addr+0);
                    uint flag = Program.ROM.u8(addr+1);
                    return U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid);
                }
                );
        }

        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {
            return new InputFormRef(self
                , "N1_"
                , Program.ROM.RomInfo.ed_2_pointer
                , 8
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint uid = Program.ROM.u8(addr + 0);
                    uint tourina = Program.ROM.u32(addr + 4);
                    return U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid) + " " + TextForm.Direct(tourina);
                }
                );
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
                    return Program.ROM.u32(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint flag = Program.ROM.u8(addr + 0);
                    uint uid1 = Program.ROM.u8(addr + 1);
                    uint uid2 = Program.ROM.u8(addr + 2);
                    if (flag == 1)
                    {
                        return U.ToHexString(uid1) + " " + UnitForm.GetUnitName(uid1);
                    }
                    if (flag == 2)
                    {
                        return U.ToHexString(uid1) + " " + UnitForm.GetUnitName(uid1)
                            + " & " + U.ToHexString(uid2) + " " + UnitForm.GetUnitName(uid2);
                    }
                    return U.ToHexString(uid1) + " " + UnitForm.GetUnitName(uid1)
                        + " ?? " + U.ToHexString(uid2) + " " + UnitForm.GetUnitName(uid2);
                }
                );
        }

        private void EDForm_Load(object sender, EventArgs e)
        {
        }

        private void N2_FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.N2_InputFormRef == null)
            {
                return ;
            }
            if ( this.N2_FilterComboBox.SelectedIndex == 1)
            {//エフラム
                N2_InputFormRef.ReInitPointer
                    ((Program.ROM.RomInfo.ed_3b_pointer));
            }
            else
            {//エイルーク
                N2_InputFormRef.ReInitPointer
                    ((Program.ROM.RomInfo.ed_3a_pointer));
            }
            N2_ReloadListButton.PerformClick();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "EDForm";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + "_1", new uint[] { });
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + "_2", new uint[] { });
            }
            {
                InputFormRef InputFormRef = N2_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + "_3a", new uint[] { });

                InputFormRef.ReInitPointer
                    ((Program.ROM.RomInfo.ed_3b_pointer));
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + "_3b", new uint[] { });
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            {
                InputFormRef InputFormRef = N1_Init(null);
                UseValsID.AppendTextID(list, FELint.Type.ED, InputFormRef, new uint[] { 4 });
            }
            {
                InputFormRef InputFormRef = N2_Init(null);
                UseValsID.AppendTextID(list, FELint.Type.ED, InputFormRef, new uint[] { 4 });

                InputFormRef.ReInitPointer
                    ((Program.ROM.RomInfo.ed_3b_pointer));
                UseValsID.AppendTextID(list, FELint.Type.ED, InputFormRef, new uint[] { 4 });
            }
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            {
                InputFormRef ifr = N1_Init(null);

                uint addr = ifr.BaseAddress;
                for (uint i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
                {
                    uint name = Program.ROM.u32(addr + 4);
                    FELint.CheckText(name, "EDTITLE1", errors, FELint.Type.ED, addr, i);
                }
            }
            {
                InputFormRef ifr = N2_Init(null);

                uint addr = ifr.BaseAddress;
                for (uint i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
                {
                    uint unitid1 = Program.ROM.u8(addr + 1);
                    if (unitid1 == 0)
                    {
                        continue;
                    }
                    uint name = Program.ROM.u32(addr + 4);

                    FELint.CheckText(name, "EDAFTER5", errors, FELint.Type.ED, addr, i);
                }
            }
        }

        void SetPopupHelp()
        {
            if (Program.ROM.RomInfo.is_multibyte)
            {
                L_1.AccessibleDescription = "00=TextID 0749 にて最期をとげる。\r\n01=TextID 074A にて負傷。一行と別れる。\r\n02=TextID 074B にて負傷するも、最後まで一行と旅を共にする。";///No Translate
            }
            else
            {
                L_1.AccessibleDescription = "00=TextID 07D1 Died at\r\n01=TextID 07D2 Wounded at and parted ways with the company.\r\n02=TextID 0x7D3-07D4 Wounded at but remained until the end.";///No Translate
            }
        }


    }
}
