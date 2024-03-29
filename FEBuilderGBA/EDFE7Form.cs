﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EDFE7Form : Form
    {
        public EDFE7Form()
        {
            InitializeComponent();
            SetPopupHelp();
            this.N2_FilterComboBox.SelectedIndex = 0;

            this.N1_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.N2_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.N3_AddressList.OwnerDraw(ListBoxEx.DrawUnit2AndText, DrawMode.OwnerDrawFixed);
            this.N4_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.N1_InputFormRef = N1_Init(this);
            this.N2_InputFormRef = N2_Init(this);
            this.N3_InputFormRef = N3_Init(this);
            this.N4_InputFormRef = N4_Init(this);

            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N3_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N4_InputFormRef.MakeGeneralAddressListContextMenu(true);
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
                    return Program.ROM.u32(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint uid = Program.ROM.u32(addr + 0);
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

        InputFormRef N3_InputFormRef;
        static InputFormRef N3_Init(Form self)
        {
            InputFormRef ifr = new InputFormRef(self
                , "N3_"
                , 0 //ポインタ指定ができない...
                , 12
                , (int i, uint addr) =>
                {
                    return Program.ROM.u32(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint uid = Program.ROM.u32(addr);
                    return U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid);
                }
                );
            ifr.ReInit(Program.ROM.RomInfo.ed_3c_pointer);
            return ifr;
        }

        public InputFormRef N4_InputFormRef;
        static InputFormRef N4_Init(Form self)
        {
            return new InputFormRef(self
                , "N4_"
                , Program.ROM.RomInfo.ed_1_pointer
                , 4
                , (int i, uint addr) =>
                {
                    return Program.ROM.u32(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint uid = Program.ROM.u8(addr + 0);
                    uint flag = Program.ROM.u8(addr + 1);
                    return U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid);
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
            {//エリウッド
                N2_InputFormRef.ReInitPointer
                    ((Program.ROM.RomInfo.ed_3b_pointer));
            }
            else
            {//ヘクトル
                N2_InputFormRef.ReInitPointer
                    ((Program.ROM.RomInfo.ed_3a_pointer));
            }
            N2_ReloadListButton.PerformClick();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "EDFE7Form";
            {
                InputFormRef InputFormRef = N3_Init(null);
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
            {
                InputFormRef InputFormRef = N4_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + "_4", new uint[] { });
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            {
                InputFormRef InputFormRef = N3_Init(null);
                UseValsID.AppendTextID(list, FELint.Type.ED, InputFormRef, new uint[] { 4 ,8 });
            }
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
            {
                InputFormRef ifr = N3_Init(null);

                uint addr = ifr.BaseAddress;
                for (uint i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
                {
                    uint name = Program.ROM.u32(addr + 4);
                    FELint.CheckText(name, "LYNEDAFTER8", errors, FELint.Type.ED, addr, i);

                    name = Program.ROM.u32(addr + 8);
                    FELint.CheckText(name, "LYNEDAFTER8", errors, FELint.Type.ED, addr, i);
                }
            }
        }
        void SetPopupHelp()
        {
            if (Program.ROM.RomInfo.is_multibyte)
            {
                N4_L_1.AccessibleDescription = "00=TextID 100F にて最期をとげる。\r\n01=TextID 1010 にて負傷。一行と別れる。\r\n02=TextID 1011 にて負傷するも、最後まで一行と旅を共にする。\r\n03=ホークアイ\r\n04=パントとルイーズ TextID 1086, 1087\r\n05=アスト";///No Translate
            }
            else
            {
                N4_L_1.AccessibleDescription = "00=TextID 1019 Died at\r\n01=TextID 101A Wounded at and parted ways with the company.\r\n02=TextID 0x101B-101C Wounded at but remained until the end.\r\n03=Hawkeye\r\n04=Pent and Louise\r\n05=Athos";///No Translate
            }
        }
    }
}
