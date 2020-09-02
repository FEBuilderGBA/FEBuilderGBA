﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MenuDefinitionForm : Form
    {
        public MenuDefinitionForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.menu_definiton_pointer()
                , 36
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr + 8));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + MakeMenuPreview(addr + 8);
                }
                );
            return ifr;
        }
        public static string MakeMenuPreview(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            List<U.AddrResult> menuList = MenuCommandForm.MakeListPointer(addr);

            string ret = "";
            for (int i = 0; i < menuList.Count; i++ )
            {
                ret += "/" + U.skip(menuList[i].name, " ");
            }
            return U.substr(ret,1);
        }

        public static List<U.AddrResult> MakeListAll()
        {
            uint[] pointers = GetPointers();

            List<U.AddrResult> ret = new List<U.AddrResult>();
            InputFormRef InputFormRef = Init(null);

            for (int n = 0; n < pointers.Length; n++)
            {
                if (pointers[n] == 0)
                {
                    continue;
                }
                InputFormRef.ReInitPointer(pointers[n]);
                List<U.AddrResult> list = InputFormRef.MakeList();
                ret.AddRange(list);
            }

            return ret;
        }

        private void MenuDefinitionForm_Load(object sender, EventArgs e)
        {
        }
        public void JumpToAddr(uint addr)
        {
            this.InputFormRef.ReInit(addr);
        }

        public static string GetMenuNameWhereMenuCommandID(uint num)
        {
            List<U.AddrResult> menuDefineList = MenuDefinitionForm.MakeListAll();
            for (int n = 0; n < menuDefineList.Count; n++)
            {
                if (!U.isSafetyOffset(menuDefineList[n].addr + 8))
                {
                    continue;
                }
                uint p = menuDefineList[n].addr + 8;
                if (!U.isSafetyOffset(Program.ROM.p32(p)))
                {
                    continue;
                }
                List<U.AddrResult> list = MenuCommandForm.MakeListPointer(p);
                for (int i = 0; i < list.Count; i++)
                {
                    if (!U.isSafetyOffset(list[i].addr))
                    {
                        continue;
                    }
                    uint menuCommandID = MenuCommandForm.GetMenuCommandID(list[i].addr);
                    if (menuCommandID == num)
                    {
                        return MenuCommandForm.GetMenuName(list[i].addr);
                    }
                }
            }
            return "";
        }

        static uint[] GetPointers()
        {
            return new uint[] { 
                           Program.ROM.RomInfo.menu_definiton_pointer()
                        ,  Program.ROM.RomInfo.menu_promotion_pointer()
                        ,  Program.ROM.RomInfo.menu_promotion_branch_pointer()
                        ,  Program.ROM.RomInfo.menu_definiton_split_pointer()
                        ,  Program.ROM.RomInfo.menu_definiton_worldmap_pointer()
                        ,  Program.ROM.RomInfo.menu_definiton_worldmap_shop_pointer()
            };

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            uint[] pointers = GetPointers();

            for (int n = 0; n < pointers.Length;n++ )
            {
                if (pointers[n] == 0)
                {
                    continue;
                }
                MakeAllDataLength(list, pointers[n], isDirectAddress: false);
            }
        }
        public static void MakeAllDataLength(List<Address> list, uint pointer, bool isDirectAddress)
        {
            InputFormRef InputFormRef = Init(null);
            if (isDirectAddress)
            {
//                InputFormRef.ReInit(pointer);
                InputFormRef.ReInitPointer(pointer , 1);
            }
            else
            {
                InputFormRef.ReInitPointer(pointer);
            }

            FEBuilderGBA.Address.AddAddressButDoNotLengthPuls1(list
                , InputFormRef
                , "MenuDefinition"
                , new uint[] { 8, 12, 16, 20, 24, 28, 32 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_MIX
                );

            uint p = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                string name = "MenuDef" + i + "_";
                uint paddr;

                paddr = Program.ROM.p32(8 + p);
                if (!U.isSafetyOffset(paddr))
                {
                    continue;
                }
                MenuCommandForm.MakeAllDataLengthP(list, 8 + p, name);

                paddr = Program.ROM.p32(12 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 12
                    , name + "_HandleBPress"
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(16 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 16
                    , name + "_HandleRPress"
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(20 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 20
                    , name + "_Construction"
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(24 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 24
                    , name + "_Destruction"
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(28 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 28
                    , name + "_UnkP28"
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(32 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 32
                    , name + "_Unk32"
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);
            }
        }
        //エラーチェック
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            uint[] pointers = GetPointers();

            InputFormRef InputFormRef = Init(null);
            for (int n = 0; n < pointers.Length; n++)
            {
                if (pointers[n] == 0)
                {
                    continue;
                }

                InputFormRef.ReInitPointer(pointers[n]);
                uint p = InputFormRef.BaseAddress;
                for (uint i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    string name = "MenuDef" + i + "_";
                    uint paddr;

                    paddr = Program.ROM.p32(8 + p);
                    if (!U.isSafetyOffset(paddr))
                    {
                        continue;
                    }
                    FELint.CheckPointer( U.toPointer(paddr), errors, FELint.Type.MENU_DEFINE, p, i);
                    MenuCommandForm.MakeCheckError(errors, 8 + p);

                    paddr = Program.ROM.u32(12 + p);
                    FELint.CheckASMPointerOrNull(paddr, errors, FELint.Type.MENU_DEFINE, p, i);

                    paddr = Program.ROM.u32(16 + p);
                    FELint.CheckASMPointerOrNull(paddr, errors, FELint.Type.MENU_DEFINE, p, i);

                    paddr = Program.ROM.u32(20 + p);
                    FELint.CheckASMPointerOrNull(paddr, errors, FELint.Type.MENU_DEFINE, p, i);

                    paddr = Program.ROM.u32(24 + p);
                    FELint.CheckASMPointerOrNull(paddr, errors, FELint.Type.MENU_DEFINE, p, i);

                    paddr = Program.ROM.u32(28 + p);
                    FELint.CheckASMPointerOrNull(paddr, errors, FELint.Type.MENU_DEFINE, p, i);

                    paddr = Program.ROM.u32(32 + p);
                    FELint.CheckASMPointerOrNull(paddr, errors, FELint.Type.MENU_DEFINE, p, i);
                }
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            uint[] pointers = GetPointers();

            for (int n = 0; n < pointers.Length; n++)
            {
                MakeVarsIDArray(list, pointers[n], isDirectAddress: false);
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list, uint pointer,bool isDirectAddress)
        {
            if (pointer == 0)
            {
                return;
            }

            InputFormRef InputFormRef = Init(null);
            if (isDirectAddress)
            {
                InputFormRef.ReInit(pointer);
            }
            else
            {
                InputFormRef.ReInitPointer(pointer);
            }
            uint p = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                uint paddr;
                paddr = Program.ROM.p32(8 + p);
                if (!U.isSafetyOffset(paddr))
                {
                    continue;
                }
                MenuCommandForm.MakeVarsIDArray(list, 8 + p);
            }

        }

        public static uint GetUnitMenuPointer()
        {
            return Program.ROM.RomInfo.menu_unit_pointer();
        }
        public static uint GetGameMenuPointer()
        {
            return Program.ROM.RomInfo.menu_game_pointer();
        }
        public static uint GetDebug1MenuPointer()
        {
            return Program.ROM.RomInfo.menu_debug1_pointer();
        }
    }
}
