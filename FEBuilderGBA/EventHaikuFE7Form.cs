using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventHaikuFE7Form : Form
    {
        public EventHaikuFE7Form()
        {
            InitializeComponent();


            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.event_haiku_pointer()
                , 16
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (Program.ROM.u8(addr) == 0x0)
                    {
                        return false;
                    }
                    if (i > 10 && Program.ROM.IsEmpty(addr, 16 * 10))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = (uint)Program.ROM.u8(addr);
                    uint map_id = (uint)Program.ROM.u8(addr + 1);
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitNameAndANY(unit_id) + 
                        " " + "(" + MapSettingForm.GetMapNameAndANYFF(map_id) + ")";
                }
            );
        }


        private void EventHaikuForm_Load(object sender, EventArgs e)
        {
        }
        public void JumpTo(uint search_unit_id, uint search_map_id)
        {
            int hit_uid_only_pos = -1;

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint unit_id = (uint)Program.ROM.u8(addr);
                uint map_id = (uint)Program.ROM.u8(addr + 1);
                if (search_unit_id == unit_id )
                {
                    if (map_id == 0x45 || search_map_id == map_id)
                    {//マップも含めて完全一致
                        U.SelectedIndexSafety(this.AddressList, i);
                        return;
                    }
                    hit_uid_only_pos = i;
                }

                addr += InputFormRef.BlockSize;
            }

            if (hit_uid_only_pos >= 0)
            {//キャラだけ一致
                U.SelectedIndexSafety(this.AddressList, hit_uid_only_pos);
                return;
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "Haiku", new uint[] { 8 });

            List<uint> tracelist = new List<uint>();
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint textid = Program.ROM.u16(addr + 6);
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
                string event_name = "Haiku" + " " + U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);

                EventScriptForm.ScanScript(list, addr + 8, true, false, event_name , tracelist);
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.HAIKU, U.NOT_FOUND
                    , R._("死亡セリフが極端に少ないです。破損している可能性があります。")));
            }

            List<uint> tracelist = new List<uint>();
            uint haiku_addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, haiku_addr += InputFormRef.BlockSize)
            {
                uint flag = Program.ROM.u16(haiku_addr + 12);
                FELint.CheckFlagErrors(flag, errors, FELint.Type.HAIKU, haiku_addr, i);

                uint textid = Program.ROM.u16(haiku_addr + 4);
                FELint.ConversationTextMessage(textid, errors, FELint.Type.HAIKU, haiku_addr, i);

                if (textid <= 0)
                {
                    uint event_addr = Program.ROM.u32(haiku_addr + 8);
                    FELint.CheckEventPointerErrors(event_addr, errors, FELint.Type.HAIKU, haiku_addr, false, tracelist);
                }
            }
        }

        public static void MakeTextIDArray(List<UseTextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseTextID.AppendTextID(list, FELint.Type.HAIKU, InputFormRef, new uint[] { 4 });
        }
        public static void MakeFlagIDArray(List<UseFlagID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseFlagID.AppendFlagID(list, FELint.Type.HAIKU, InputFormRef, 12, 1);
        }
    }
}
