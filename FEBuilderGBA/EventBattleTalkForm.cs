using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventBattleTalkForm : Form
    {
        public EventBattleTalkForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnit2AndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.event_ballte_talk_pointer
                , 16
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (Program.ROM.u16(addr) == 0xFFFF)
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
                    uint unit_id = (uint)Program.ROM.u16(addr);
                    uint unit_id2 = (uint)Program.ROM.u16(addr + 2);
                    uint map_id = (uint)Program.ROM.u16(addr + 4);
                    return U.ToHexString(unit_id) + " " 
                        + UnitForm.GetUnitNameAndANY(unit_id) 
                        + " -> " 
                        + U.ToHexString(unit_id2) 
                        + " " 
                        + UnitForm.GetUnitNameAndANY(unit_id2) 
                        + " " 
                        + "(" 
                            + MapSettingForm.GetMapNameAndANYFF(map_id) 
                        + ")";
                }
            );
        }
        private void EventBattleTalkForm_Load(object sender, EventArgs e)
        {
        }

        public void JumpTo(uint search_unit_id, uint search_map_id)
        {
            int hit_uid_only_pos = -1;

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint unit_id = (uint)Program.ROM.u16(addr);
                uint unit_id2 = (uint)Program.ROM.u16(addr + 2);
                uint map_id = (uint)Program.ROM.u16(addr + 4);
                if (search_unit_id == unit_id || search_unit_id == unit_id2)
                {
                    if (search_map_id == map_id)
                    {//マップも含めて完全一致
                        U.SelectedIndexSafety(this.AddressList, i);
                        return;
                    }
                    hit_uid_only_pos = i;
                }

                addr += InputFormRef.BlockSize;
            }

            if (hit_uid_only_pos > 0)
            {//キャラだけ一致
                U.SelectedIndexSafety(this.AddressList, hit_uid_only_pos);
                return;
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            List<uint> tracelist = new List<uint>();
            string name = "EventBattleTalkForm";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 12 });

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
//                    uint textid = Program.ROM.u16(addr + 8);
//                    if (textid <= 0)
//                    {
//                        continue;
//                    }

                    uint spEventP = Program.ROM.u32(addr + 12);
                    if (!U.isSafetyPointer(spEventP))
                    {
                        continue;
                    }
                    uint unitid = Program.ROM.u8(addr + 0);
                    string event_name = "EventBattleTalkForm" + " " + U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);

                    EventScriptForm.ScanScript(list, addr + 12, true, false, event_name , tracelist);
                }
            }
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BATTTLE_TALK, U.NOT_FOUND
                    , R._("交戦会話が極端に少ないです。破損している可能性があります。")));
            }

            List<uint> tracelist = new List<uint>();
            uint battletalk_addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, battletalk_addr += InputFormRef.BlockSize)
            {
                uint flag = Program.ROM.u16(battletalk_addr + 6);
                FELint.CheckFlag(flag, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);

                uint textid = Program.ROM.u16(battletalk_addr + 8);
                FELint.DeathQuoteTextMessage(textid, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, i);

                if (textid <= 0)
                {
                    uint event_addr = Program.ROM.u32(battletalk_addr + 12);
                    FELint.CheckEventPointer(event_addr, errors, FELint.Type.BATTTLE_TALK, battletalk_addr, false, tracelist);
                }
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            string infobase = R._("交戦会話");

            InputFormRef InputFormRef = Init(null);
            List<uint> tracelist = new List<uint>();
            uint haiku_addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, haiku_addr += InputFormRef.BlockSize)
            {
                uint unitid = Program.ROM.u8(haiku_addr + 0);
                uint unitid2 = Program.ROM.u8(haiku_addr + 2);

                string info = infobase + " " + UnitForm.GetUnitName(unitid) + " " + UnitForm.GetUnitName(unitid2);
                uint textid = Program.ROM.u16(haiku_addr + 8);

                if (textid <= 0)
                {
                    uint event_addr = Program.ROM.p32(haiku_addr + 12);
                    EventCondForm.MakeVarsIDArrayByEventAddress(list, event_addr, info, tracelist);
                }
                else
                {
                    UseValsID.AppendTextID(list, FELint.Type.BATTTLE_TALK, haiku_addr, info, textid, i);
                }
            }

//            UseValsID.AppendTextID(list, FELint.Type.BATTTLE_TALK, InputFormRef, new uint[] { 8 });
        }
        public static void MakeFlagIDArray(List<UseFlagID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseFlagID.AppendFlagID(list, FELint.Type.BATTTLE_TALK, InputFormRef, 6, 4);
        }
    }
}
