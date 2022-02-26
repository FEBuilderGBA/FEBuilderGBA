using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventBattleTalkFE6Form : Form
    {
        public EventBattleTalkFE6Form()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnit2AndText, DrawMode.OwnerDrawFixed);
            this.N_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.N_InputFormRef = N_Init(this);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.event_ballte_talk_pointer()
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint unit = Program.ROM.u16(addr);
                    if (unit == 0x0 || unit == 0xFFFF)
                    {
                        return false;
                    }
                    if (i > 10 && Program.ROM.IsEmpty(addr, 12 * 10))
                    {//終端符号を無視して 0x00等を利用している人がいるため
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = (uint)Program.ROM.u8(addr);
                    uint unit_id2 = (uint)Program.ROM.u8(addr + 1);
                    return U.ToHexString(unit_id)
                        + " " 
                        + UnitForm.GetUnitName(unit_id)
                        + " -> "
                        + U.ToHexString(unit_id2) 
                        + " " 
                        + UnitForm.GetUnitName(unit_id2) 
                        ;
                }
            );
        }
        public InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.event_ballte_talk2_pointer()
                , 16
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint unit = Program.ROM.u16(addr);
                    if (unit == 0x0 || unit == 0xFFFF)
                    {
                        return false;
                    }
                    if (i > 10 && Program.ROM.IsEmpty(addr, 16 * 10))
                    {//終端符号を無視して 0x00等を利用している人がいるため
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = (uint)Program.ROM.u8(addr);
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitName(unit_id);
                }
            );
        }
        
        private void EventBattleTalkFE6Form_Load(object sender, EventArgs e)
        {
        }

        public void JumpTo(uint search_unit_id)
        {
            JumpToTable1(search_unit_id);
            JumpToTable2(search_unit_id);
        }
        public void JumpToTable1(uint search_unit_id)
        {
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint unit_id = (uint)Program.ROM.u8(addr);
                uint unit_id2 = (uint)Program.ROM.u8(addr + 1);
                if (search_unit_id == unit_id || search_unit_id == unit_id2)
                {
                    U.SelectedIndexSafety(this.AddressList, i);
                    return;
                }

                addr += InputFormRef.BlockSize;
            }
        }
        public void JumpToTable2(uint search_unit_id)
        {
            uint addr = N_InputFormRef.BaseAddress;
            for (int i = 0; i < N_InputFormRef.DataCount; i++)
            {
                uint unit_id = (uint)Program.ROM.u8(addr);
                uint unit_id2 = (uint)Program.ROM.u8(addr + 1);
                if (search_unit_id == unit_id || search_unit_id == unit_id2)
                {
                    U.SelectedIndexSafety(this.N_AddressList, i);
                    return;
                }

                addr += InputFormRef.BlockSize;
            }
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "EventBattleTalkFE6Form";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            }
            {
                InputFormRef InputFormRef = N_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + "_2", new uint[] { });
            }
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            {
                InputFormRef InputFormRef = Init(null);
                if (InputFormRef.DataCount < 2)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.BATTTLE_TALK, U.NOT_FOUND
                        , R._("交戦会話が極端に少ないです。破損している可能性があります。")));
                }

                uint battletalk_addr = InputFormRef.BaseAddress;
                for (uint i = 0; i < InputFormRef.DataCount; i++, battletalk_addr += InputFormRef.BlockSize)
                {
                    uint flag = Program.ROM.u16(battletalk_addr + 8);
                    FELint.CheckFlag(flag, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);

                    uint textid = Program.ROM.u16(battletalk_addr + 4);
                    FELint.DeathQuoteTextMessage(textid, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);
                }
            }
            {
                InputFormRef InputFormRef = N_Init(null);
                if (InputFormRef.DataCount < 2)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.BATTTLE_TALK, U.NOT_FOUND
                        , R._("交戦会話が極端に少ないです。破損している可能性があります。")));
                }

                uint battletalk_addr = InputFormRef.BaseAddress;
                for (uint i = 0; i < InputFormRef.DataCount; i++, battletalk_addr += InputFormRef.BlockSize)
                {
                    uint flag = Program.ROM.u16(battletalk_addr + 8);
                    FELint.CheckFlag(flag, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);

                    uint textid = Program.ROM.u16(battletalk_addr + 4);
                    FELint.DeathQuoteTextMessage(textid, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);
                }
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                UseValsID.AppendTextID(list, FELint.Type.BATTTLE_TALK, InputFormRef, new uint[] { 4 });
            }
            {
                InputFormRef InputFormRef = N_Init(null);
                UseValsID.AppendTextID(list, FELint.Type.BATTTLE_TALK, InputFormRef, new uint[] { 4 });
            }
        }
        public static void MakeFlagIDArray(List<UseFlagID> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                UseFlagID.AppendFlagID(list, FELint.Type.BATTTLE_TALK, InputFormRef, 8, 2);
            }
            {
                InputFormRef InputFormRef = N_Init(null);
                UseFlagID.AppendFlagID(list, FELint.Type.BATTTLE_TALK, InputFormRef, 8, 1);
            }
        }
    }
}
