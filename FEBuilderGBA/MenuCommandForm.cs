using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MenuCommandForm : Form
    {
        public MenuCommandForm()
        {
            InitializeComponent();
            Explain();
            InputFormRef.OwnerDrawColorCombo(L_8_COMBO);

            List<U.AddrResult> menuDefineList = MenuDefinitionForm.MakeListAll();
            U.ConvertComboBox(menuDefineList,ref FilterComboBox);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 36
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr+0xc));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + GetMenuName(addr);
                }
                );
        }

        public static string GetMenuName(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }

            String name = "";
            if (Program.ROM.RomInfo.is_multibyte())
            {
                uint nameAddr = Program.ROM.p32(addr);
                if (nameAddr > 0xFFFF && U.isSafetyOffset(nameAddr))
                {
                    name = Program.ROM.getString(nameAddr);
                }
                if (name == "")
                {//日本語ポインタがない場合、文字列IDを参照する.
                    name = TextForm.Direct(Program.ROM.u16(addr + 4));
                }
            }
            else
            {
                name = TextForm.Direct(Program.ROM.u16(addr + 4));
            }
            return name;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef == null)
            {
                return;
            }

            uint addr = InputFormRef.SelectToAddr(FilterComboBox);
            if (addr == U.NOT_FOUND)
            {
                return;
            }
            if (!U.isSafetyOffset(addr + 8))
            {
                return;
            }
            uint menuAddr = Program.ROM.p32(addr + 8);
            if (!U.isSafetyOffset(menuAddr))
            {
                return;
            }

            this.InputFormRef.ReInit(menuAddr);
        }


        public void JumpToAddr(uint addr)
        {
            int selected = -1;
            for (int i = 0; i < FilterComboBox.Items.Count; i++)
            {
                uint paddr = InputFormRef.SelectToAddr(FilterComboBox,i);
                if (paddr == U.NOT_FOUND)
                {
                    continue;
                }
                if (paddr +8 == addr)
                {//ポインタ一致
                    selected = i;
                    break;
                }

                if (!U.isSafetyOffset(paddr + 8))
                {
                    continue;
                }
                uint menuAddr = Program.ROM.p32(paddr + 8);
                if (!U.isSafetyOffset(menuAddr))
                {
                    continue;
                }
                if (menuAddr == addr)
                {
                    selected = i;
                    break;
                }
            }

            FilterComboBox.SelectedIndex = selected;
            this.InputFormRef.ReInit(addr);
        }

        public static uint GetUsabilityAddrByAddr(uint addr)
        {
            if (!U.isSafetyOffset(addr + 12))
            {
                return U.NOT_FOUND;
            }

            uint func = Program.ROM.u32(addr + 12);
            if (U.IsValueOdd(func) && U.isSafetyPointer(func))
            {
                return U.toOffset(func) - 1;
            }
            return U.NOT_FOUND;
        }
        public static uint GetEffectAddrByAddr(uint addr)
        {
            if (!U.isSafetyOffset(addr + 12))
            {
                return U.NOT_FOUND;
            }

            uint func = Program.ROM.u32(addr + 20);
            if (U.IsValueOdd(func) && U.isSafetyPointer(func))
            {
                return U.toOffset(func) - 1;
            }
            return U.NOT_FOUND;
        }

        public static List<U.AddrResult> MakeListPointer(uint pointer = U.NOT_FOUND)
        {
            InputFormRef InputFormRef = Init(null);
            if (pointer != U.NOT_FOUND)
            {
                InputFormRef.ReInitPointer(pointer);
            }
            return InputFormRef.MakeList();
        }
        public static uint GetMenuCommandID(uint addr)
        {
            if (!U.isSafetyOffset(addr + 9))
            {
                return U.NOT_FOUND;
            }
            return Program.ROM.u8(addr + 9);
        }

        //全データの取得
        public static void MakeAllDataLengthP(List<Address> list,uint pointer,string name)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(pointer);
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , "MENU"
                , new uint[] {0, 12, 16, 20, 24, 28, 32 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_MIX
                );

            string lang = OptionForm.lang();
            bool isJP = (lang == "ja") || Program.ROM.RomInfo.is_multibyte();

            string p12label = R._("可否診断ルーチンポインタ");
            string p16label = R._("描画ルーチンポインタ");
            string p20label = R._("選択時に実行する効果ポインタ");
            string p24label = R._("選択時に毎ターン呼び出されるポインタ");
            string p28label = R._("カーソルで選択されたときの動作ポインタ");
            string p32label = R._("キャンセルされたときの動作ポインタ");

            uint p = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                string menuname;
                if (isJP)
                {
                    menuname = name + " " + TextForm.Direct(Program.ROM.p32(0 + p));
                }
                else
                {
                    menuname = name + " " + TextForm.Direct(Program.ROM.u16(4 + p));
                }

                FEBuilderGBA.Address.AddCString(list, 0 + p);

                uint paddr = Program.ROM.p32(12 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 12
                    , menuname + p12label
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(16 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 16
                    , menuname + p16label
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(20 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 20
                    , menuname + p20label
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(24 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 24
                    , menuname + p24label
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(28 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 28
                    , menuname + p28label
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);

                paddr = Program.ROM.p32(32 + p);
                FEBuilderGBA.Address.AddAddress(list,
                        DisassemblerTrumb.ProgramAddrToPlain(paddr)
                    , 0 //プログラムなので長さ不明
                    , p + 32
                    , menuname + p32label
                    , FEBuilderGBA.Address.DataTypeEnum.ASM);
            }
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors, uint pointer)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(pointer);

            uint p = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                uint paddr;
                paddr = Program.ROM.u32(0 + p);
                FELint.CheckPointerOrNullErrors(paddr, errors, FELint.Type.MENU, p, i);

                if (Program.ROM.RomInfo.is_multibyte() == false)
                {
                    uint name = Program.ROM.u16(4 + p);
                    FELint.CheckText(name, "MENUNAME1", errors, FELint.Type.MENU, p, i);
                }

                uint detail = Program.ROM.u16(6 + p);
                FELint.CheckText(detail, "MENUDETAIL3", errors, FELint.Type.MENU, p, i);

                paddr = Program.ROM.u32(12 + p);
                FELint.CheckASMPointerOrNullErrors(paddr, errors, FELint.Type.MENU, p, i);

                paddr = Program.ROM.u32(16 + p);
                FELint.CheckASMPointerOrNullErrors(paddr, errors, FELint.Type.MENU, p, i);

                paddr = Program.ROM.u32(20 + p);
                FELint.CheckASMPointerOrNullErrors(paddr, errors, FELint.Type.MENU, p, i);

                paddr = Program.ROM.u32(24 + p);
                FELint.CheckASMPointerOrNullErrors(paddr, errors, FELint.Type.MENU, p, i);

                paddr = Program.ROM.u32(28 + p);
                FELint.CheckASMPointerOrNullErrors(paddr, errors, FELint.Type.MENU, p, i);

                paddr = Program.ROM.u32(32 + p);
                FELint.CheckASMPointerOrNullErrors(paddr, errors, FELint.Type.MENU, p, i);
            }
        }
        public static void MakeTextIDArray(List<UseTextID> list, uint pointer)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(pointer);
            UseTextID.AppendTextID(list, FELint.Type.MENU, InputFormRef, new uint[] { 4 });
        }

        void Explain()
        {
            J_12_ASM.AccessibleDescription = R._("この項目をメニューに表示するかどうかを決定する関数を指定します。\r\nこの関数の戻り値(r0)が1だった場合、この項目がメニューが表示されます。\r\n戻り値(r0)が3だった場合は、メニューが表示されません。\r\nこの項目は必須項目です。") + "\r\n\r\n" + Explain12();
            J_16_ASM.AccessibleDescription = R._("この項目を描画するときに利用する関数を指定します。\r\n0の場合は、デフォルト描画が呼び出されます。単純にテキストを描画します。");
            J_20_ASM.AccessibleDescription = R._("この項目が選択された時に動作させる関数を指定します。\r\nこの項目は必須項目です。");
            J_24_ASM.AccessibleDescription = R._("この項目を選んでいる時に毎ターン呼び出すルーチンを指定します。\r\n0の場合ディフォルト動作になります。");
            J_28_ASM.AccessibleDescription = R._("この項目をカーソルで選択された時に呼び出されるルーチンを指定します。\r\n0の場合ディフォルト動作になります。");
            J_32_ASM.AccessibleDescription = R._("メニューがキャンセルされたときに呼び出されるルーチンを指定します。\r\n0の場合ディフォルト動作になります。");
        }
        string Explain12()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return R._("よく使われる関数のメモ\r\n{0} 常にメニューの項目を表示します。"
                    , U.ToHexString8(U.toPointer(Program.ROM.RomInfo.menu_J12_always_address() + 1))
                    );
            }

            return R._("よく使われる関数のメモ\r\n{0} 常にメニューの項目を表示します。\r\n{1} メニューを非表示にします。"
                ,U.ToHexString8(U.toPointer(Program.ROM.RomInfo.menu_J12_always_address()+1))
                ,U.ToHexString8(U.toPointer(Program.ROM.RomInfo.menu_J12_hide_address()+1))
                );
        }

        public static uint ExpandsArea(Form form, string typename, uint newdatacount, Undo.UndoData undodata)
        {
            uint pointer;
            if (typename == "UNITMENU")
            {
                pointer = MenuDefinitionForm.GetUnitMenuPointer();
            }
            else if (typename == "GAMEMENU")
            {
                pointer = MenuDefinitionForm.GetGameMenuPointer();
            }
            else
            {
                return U.NOT_FOUND;
            }

            uint newaddr;
            {
                InputFormRef.ExpandsEventArgs eearg = new InputFormRef.ExpandsEventArgs();

                InputFormRef ifr = Init(null);
                ifr.ReInitPointer(pointer);

                eearg.OldBaseAddress = ifr.BaseAddress;
                eearg.OldDataCount = ifr.DataCount;
                eearg.BlockSize = ifr.BlockSize;

                newaddr = ifr.ExpandsArea(form, newdatacount, undodata, pointer);
                if (newaddr == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }

                eearg.NewBaseAddress = newaddr;
                eearg.NewDataCount = newdatacount ;
                AddressListExpandsEventInner(form, eearg, undodata);
            }
            InputFormRef.ClearCacheDataCount();
            return newaddr;
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.WriteButtonToYellow(WriteButton, false);

            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "ClearAllocMenu");
            AddressListExpandsEventInner(this, eearg, undodata);
            eearg.NewDataCount -= 1;
            eearg.IsReload = true;
            Program.Undo.Push(undodata);
        }

        public static void WriteNullMenu(uint addr, Undo.UndoData undodata)
        {
            if (! U.isSafetyOffset(addr + 36))
            {
                return;
            }
            uint alwayFalse = 0;
            if (Program.ROM.RomInfo.version() != 6)
            {
                alwayFalse = U.toPointer(Program.ROM.RomInfo.MenuCommand_UsabilityNever() + 1);
            }

            Program.ROM.write_u32(addr + 0, 0, undodata);
            Program.ROM.write_u32(addr + 4, 0, undodata);
            Program.ROM.write_u32(addr + 8, 0, undodata);
            Program.ROM.write_u32(addr + 12, alwayFalse, undodata); //常にメニューを表示しないを選択
            Program.ROM.write_u32(addr + 16, 0, undodata);
            Program.ROM.write_u32(addr + 20, 0, undodata);
            Program.ROM.write_u32(addr + 24, 0, undodata);
            Program.ROM.write_u32(addr + 28, 0, undodata);
            Program.ROM.write_u32(addr + 32, 0, undodata);
        }

        static void AddressListExpandsEventInner(Form from, InputFormRef.ExpandsEventArgs eearg, Undo.UndoData undodata)
        {
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;
            if (count <= 2)
            {
                return;
            }

            uint rom_length = (uint)Program.ROM.Data.Length;

            //配置後アドレスを0クリアします.
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);
            for (int i = (int)eearg.OldDataCount; i < count - 1; i++)
            {
                if (addr + 36 > rom_length)
                {
                    break;
                }
                WriteNullMenu(addr, undodata);
                addr += eearg.BlockSize;
            }

            //最後のメニューと新規確保したメニューの位置を入れ替える.
            if (eearg.OldDataCount < eearg.NewDataCount && eearg.OldDataCount >= 1)
            {
                uint oldEndAddr = eearg.NewBaseAddress + ((eearg.OldDataCount - 1) * eearg.BlockSize);
                uint newEndAddr = eearg.NewBaseAddress + ((eearg.NewDataCount - 2) * eearg.BlockSize);
                byte[] endEndData = Program.ROM.getBinaryData(oldEndAddr, eearg.BlockSize);
                byte[] newEndData = Program.ROM.getBinaryData(newEndAddr, eearg.BlockSize);

                Program.ROM.write_range(oldEndAddr, newEndData, undodata);
                Program.ROM.write_range(newEndAddr, endEndData, undodata);
            }

            {//終端
                addr = eearg.NewBaseAddress + ((eearg.NewDataCount - 1) * eearg.BlockSize);
                Program.ROM.write_u32(addr + 0, 0, undodata);
                Program.ROM.write_u32(addr + 4, 0, undodata);
                Program.ROM.write_u32(addr + 8, 0, undodata);
                Program.ROM.write_u32(addr + 12, 0, undodata);
                Program.ROM.write_u32(addr + 16, 0, undodata);
                Program.ROM.write_u32(addr + 20, 0, undodata);
                Program.ROM.write_u32(addr + 24, 0, undodata);
                Program.ROM.write_u32(addr + 28, 0, undodata);
                Program.ROM.write_u32(addr + 32, 0xFFFFFFFF, undodata); //nullが連続していると、間違われることがあるので、最終単に0xFFFFFFFFを入れる.
            }
        }

        public static uint AllocNullMenuAddress(uint needCount, string typename)
        {
            uint pointer;
            if (typename == "UNITMENU")
            {
                pointer = MenuDefinitionForm.GetUnitMenuPointer();
            }
            else if (typename == "GAMEMENU")
            {
                pointer = MenuDefinitionForm.GetGameMenuPointer();
            }
            else
            {
                return U.NOT_FOUND;
            }

            uint alwayFalse = 0;
            if (Program.ROM.RomInfo.version() != 6)
            {
                alwayFalse = U.toPointer(Program.ROM.RomInfo.MenuCommand_UsabilityNever() + 1);
            }

            uint nullCount = 0;
            uint foundAddr = U.NOT_FOUND;

            InputFormRef ifr = Init(null);
            ifr.ReInitPointer(pointer);

            uint addr = ifr.BaseAddress;
            for (int i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
            {

                uint a1 = Program.ROM.u32(addr + 0);
                uint a2 = Program.ROM.u32(addr + 4);
                uint a3 = Program.ROM.u32(addr + 8);
                uint a4 = Program.ROM.u32(addr + 12); //常にメニューを表示しないを選択
                uint a5 = Program.ROM.u32(addr + 16);
                uint a6 = Program.ROM.u32(addr + 20);
                uint a7 = Program.ROM.u32(addr + 24);
                uint a8 = Program.ROM.u32(addr + 28);
                uint a9 = Program.ROM.u32(addr + 32);

                if (a1 == 0 && a2 == 0 && a3 == 0 && a4 == alwayFalse
                    && a5 == 0 && a6 == 0 && a7 == 0 && a8 == 0 && a9 == 0)
                {
                    if (nullCount <= 0)
                    {
                        foundAddr = addr;
                    }

                    nullCount++;
                    if (nullCount >= needCount)
                    {
                        return foundAddr;
                    }
                }
                else
                {
                    nullCount = 0;
                    foundAddr = U.NOT_FOUND;
                }
            }
            return U.NOT_FOUND;
        }
    }
}
