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
    public partial class AIUnitsForm : Form
    {
        public AIUnitsForm()
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
                , 0
                , 2
                , (int i, uint addr) =>
                {
                    return Program.ROM.u16(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint unitid = Program.ROM.u8(addr + 0);
                    return U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);
                }
                );
        }

        private void AIUnitsForm_Load(object sender, EventArgs e)
        {

        }
        public void JumpTo(uint addr)
        {
            this.InputFormRef.ReInit(addr);
        }
        public uint GetBaseAddress()
        {
            return this.InputFormRef.BaseAddress;
        }
        public static uint CalcLength(uint addr)
        {
            uint start = addr;
            uint length = (uint)Program.ROM.Data.Length;
            for (; addr < length; addr += 2)
            {
                if (Program.ROM.u16(addr) == 0x00)
                {
                    break;
                }
            }

            return addr - start;
        }
    }
}
