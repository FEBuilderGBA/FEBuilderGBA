using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SummonUnitForm : Form
    {
        public SummonUnitForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.summon_unit_pointer()
                , 2
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint unitid = Program.ROM.u8(addr + 0);
                    return U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);
                }
                );
        }


        private void SummonUnitForm_Load(object sender, EventArgs e)
        {

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list,InputFormRef, "Summon", new uint[]{});
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            if (Program.ROM.RomInfo.version() != 8)
            {
                Debug.Assert(false);
                eearg.IsCancel = true;
                return;
            }
            if (count >= 257 || count <= 0)
            {
                Debug.Assert(false);
                eearg.IsCancel = true;
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "SummonRepointExtraData");

            //table+1のアドレスを指定している部分があるので、その部分も書き換える必要あり
            {
                uint[] write_count_addr;
                if (Program.ROM.RomInfo.is_multibyte())
                {//For FE8J
                    write_count_addr = new uint[] { 0x024450, 0x07D14C };
                }
                else
                {//For FE8U
                    write_count_addr = new uint[] { 0x0244A0, 0x07AE04, searchSkillSystemsSummonPlus1()};
                }
                for (int i = 0; i < write_count_addr.Length; i++)
                {
                    if (write_count_addr[i] == U.NOT_FOUND)
                    {
                        continue;
                    }
                    Program.ROM.write_u32(write_count_addr[i], U.toPointer(eearg.NewBaseAddress+1));
                }
            }

            //件数を書く必要があるらしい.
            {
                uint[] write_count_addr;
                if (Program.ROM.RomInfo.is_multibyte())
                {//For FE8J
                    write_count_addr = new uint[] { 0x07D0B6, 0x0243EA };
                }
                else
                {//For FE8U
                    write_count_addr = new uint[] { 0x07AD66, 0x024436, searchSkillSystemsSummonCount()};
                }
                for (int i = 0; i < write_count_addr.Length; i++)
                {
                    if (write_count_addr[i] == U.NOT_FOUND)
                    {
                        continue;
                    }
                    Program.ROM.write_u8(write_count_addr[i], (uint)(count - 1));
                }
            }
            Program.Undo.Push(undodata);
        }
        static uint searchSkillSystemsSummonPlus1()
        {
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return U.NOT_FOUND;
            }

            byte[] need = new byte[] { 0xB6, 0x46, 0x00, 0xF8, 0x02, 0x1C, 0x00, 0x2A, 0x12, 0xD0, 0x10, 0x68, 0x00, 0x28, 0x0F, 0xD0, 0x00, 0x79, 0x29, 0x78, 0x88, 0x42, 0x0B, 0xD1, 0xD1, 0x68, 0x04, 0x48, 0x08, 0x40, 0x00, 0x28, 0xCC, 0xD1, 0xBE, 0xE7 };
            uint addr = U.GrepEnd(Program.ROM.Data, need, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
            if (addr == U.NOT_FOUND)
            {
                R.ShowWarning("SkillSystemsがインストールされているのに、searchSkillSystemsSummonPlus1 に失敗しました。\r\n処理は続行しますが、うまく動作しない可能性があります。");
            }
            return addr;

        }
        static uint searchSkillSystemsSummonCount()
        {
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return U.NOT_FOUND;
            }

            byte[] need = new byte[] { 0x1D, 0xE0, 0x03, 0x20, 0x48, 0xE0, 0x00, 0x00, 0x50, 0x4E, 0x00, 0x03, 0xA5, 0x5C, 0x02, 0x08, 0x29, 0xFD, 0x04, 0x08, 0xFF, 0xFF, 0x00, 0x00, 0x30, 0x95, 0x00, 0x09, 0x0B, 0x48, 0x01, 0x40, 0xD1, 0x60, 0x38, 0xE0, 0x01, 0x32 };
            uint addr = U.GrepEnd(Program.ROM.Data, need, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
            if (addr == U.NOT_FOUND)
            {
                R.ShowWarning("SkillSystemsがインストールされているのに、searchSkillSystemsSummonCount に失敗しました。\r\n処理は続行しますが、うまく動作しない可能性があります。");
            }
            return addr;
        }
    }
}
