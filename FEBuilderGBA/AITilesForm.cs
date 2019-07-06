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
    public partial class AITilesForm : Form
    {
        public AITilesForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 1
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint tileid = Program.ROM.u8(addr + 0);
                    return U.ToHexString(tileid) + " " + MapTerrainNameForm.GetName(tileid);
                }
                );
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
            addr = U.toOffset(addr);
            uint start = addr;
            uint length = (uint)Program.ROM.Data.Length;
            for (; addr < length; addr += 1)
            {
                if (Program.ROM.u8(addr) == 0x00)
                {
                    break;
                }
            }

            return addr - start;
        }
        public static string GetNames(uint addr)
        {
            StringBuilder sb = new StringBuilder();
            addr = U.toOffset(addr);
            uint start = addr;
            uint length = (uint)Program.ROM.Data.Length;
            for (; addr < length; addr += 1)
            {
                uint value = Program.ROM.u8(addr);
                if (value == 0x00)
                {
                    break;
                }
                if (sb.Length != 0)
                {
                    sb.Append(", ");
                }
                string text = MapTerrainNameForm.GetName(value);
                sb.Append(U.ToHexString(value));
                sb.Append('(');
                sb.Append(text);
                sb.Append(')');
            }

            return sb.ToString();
        }
        
    }
}
