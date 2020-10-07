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
    public partial class SMEPromoListForm : Form
    {
        public SMEPromoListForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawClass2AndText, DrawMode.OwnerDrawFixed);

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
                    uint id = Program.ROM.u8(addr + 0);
                    uint id2 = Program.ROM.u8(addr + 1);
                    return U.ToHexString(id) + " " + ClassForm.GetClassName(id) + "-> " + U.ToHexString(id2) + " " + ClassForm.GetClassName(id2);
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
        static public string GetNames(uint addr)
        {
            StringBuilder sb = new StringBuilder();

            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInit(addr);

            List<U.AddrResult> list = InputFormRef.MakeList();
            foreach(U.AddrResult ar in list)
            {
                sb.Append(',');
                sb.Append(ar.name);
            }
            return U.ToString_StringBuilder(sb);
        }
        public static void MakeDataLength(List<Address> list, uint pointer, string strname)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(pointer);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, strname, new uint[] { });
        }

        private void SMEPromoListForm_Load(object sender, EventArgs e)
        {

        }
    }
}
