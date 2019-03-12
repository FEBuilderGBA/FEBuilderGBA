using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventBattleTalkFE7Form : Form
    {
        public EventBattleTalkFE7Form()
        {
            InitializeComponent();


            this.AddressList.OwnerDraw(ListBoxEx.DrawUnit2AndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.N1_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.N1_InputFormRef = N1_Init(this);
            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.event_ballte_talk_pointer()
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
                    uint unit_id2 = (uint)Program.ROM.u8(addr + 1);
                    return U.ToHexString(unit_id) 
                        + " " 
                        + UnitForm.GetUnitNameAndANY(unit_id) 
                        + " -> "
                        + U.ToHexString(unit_id2) 
                        + " " 
                        + UnitForm.GetUnitNameAndANY(unit_id2);
                }
            );
        }
        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {
            return new InputFormRef(self
                , "N1_"
                , Program.ROM.RomInfo.event_ballte_talk2_pointer()
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint unit = Program.ROM.u8(addr);
                    if (unit == 0x0 || unit == 0xFF)
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
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitNameAndANY(unit_id);
                }
            );
        }

        private void EventBattleTalkForm_Load(object sender, EventArgs e)
        {
            this.EXPLAIN.Text
                += "\r\n"
                + R._("FE7では、特別に、会話時に発動するイベントをここでも設定できます。");
        }

        public void JumpTo(uint search_unit_id)
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
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "EventBattleTalk1", new uint[] { 8 });

                List<uint> tracelist = new List<uint>();
                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    uint textid = Program.ROM.u16(addr + 4);
                    if (textid <= 0)
                    {
                        continue;
                    }

                    uint spEventP = Program.ROM.u32(addr + 8);
                    if (!U.isSafetyPointer(spEventP))
                    {
                        continue;
                    }
                    uint unitid = Program.ROM.u8(addr + 0);
                    string event_name = "EventBattleTalk1" + " " + U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);

                    EventScriptForm.ScanScript(list, addr + 8, true, false, event_name, tracelist);
                }
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "EventBattleTalk2", new uint[] { });
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

                List<uint> tracelist = new List<uint>();
                uint battletalk_addr = InputFormRef.BaseAddress;
                for (uint i = 0; i < InputFormRef.DataCount; i++, battletalk_addr += InputFormRef.BlockSize)
                {
                    uint flag = Program.ROM.u16(battletalk_addr + 12);
                    FELint.CheckFlagErrors(flag, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);

                    uint textid = Program.ROM.u16(battletalk_addr + 4);
                    FELint.DeathQuoteTextMessage(textid, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);

                    if (textid <= 0)
                    {
                        uint event_addr = Program.ROM.u32(battletalk_addr + 8);
                        FELint.CheckEventPointerErrors(event_addr, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, false, tracelist);
                    }
                }
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                if (InputFormRef.DataCount < 2)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.BATTTLE_TALK, U.NOT_FOUND
                        , R._("交戦会話が極端に少ないです。破損している可能性があります。")));
                }

                uint battletalk_addr = InputFormRef.BaseAddress;
                for (uint i = 0; i < InputFormRef.DataCount; i++, battletalk_addr += InputFormRef.BlockSize)
                {
                    uint flag = Program.ROM.u16(battletalk_addr + 8);
                    FELint.CheckFlagErrors(flag, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);

                    uint textid = Program.ROM.u16(battletalk_addr + 4);
                    FELint.DeathQuoteTextMessage(textid, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);
                }
            }
        }
        public static void MakeTextIDArray(List<UseTextID> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                UseTextID.AppendTextID(list, FELint.Type.BATTTLE_TALK, InputFormRef, new uint[] { 4 });
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                UseTextID.AppendTextID(list, FELint.Type.BATTTLE_TALK, InputFormRef, new uint[] { 4 });
            }
        }
        public static void MakeFlagIDArray(List<UseFlagID> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                UseFlagID.AppendFlagID(list, FELint.Type.BATTTLE_TALK, InputFormRef,  12 , 2);
            }
            {
                InputFormRef InputFormRef = N1_Init(null);
                UseFlagID.AppendFlagID(list, FELint.Type.BATTTLE_TALK, InputFormRef, 8, 1);
            }
        }
    }
}
