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
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;
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
                    write_count_addr = new uint[] { 0x0244A0, 0x07AE04 };
                }
                for (int i = 0; i < write_count_addr.Length; i++)
                {
                    Program.ROM.write_u8(write_count_addr[i], U.toPointer(eearg.NewBaseAddress+1));
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
                    write_count_addr = new uint[] { 0x07AD66, 0x024436 };
                }
                for (int i = 0; i < write_count_addr.Length; i++)
                {
                    Program.ROM.write_u8(write_count_addr[i], (uint)(count - 1));
                }
            }
            Program.Undo.Push(undodata);
        }
    }
}
