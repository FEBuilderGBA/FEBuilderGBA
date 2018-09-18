using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemRandomChestForm : Form
    {
        public ItemRandomChestForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.PreWriteHandler += OnPreWrite;
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
                    return Program.ROM.u8(addr)!=0x00;
                }
                , (int i, uint addr) =>
                {
                    uint item_id = Program.ROM.u8(addr);
                    return U.ToHexString(item_id) + " " + ItemForm.GetItemName(item_id);
                }
                );
        }
        private void ItemShopForm_Load(object sender, EventArgs e)
        {
        }

        public void JumpTo(uint addr, EventHandler addressListExpandsEvent = null)
        {
            InputFormRef.ReInit(addr);
            if (addressListExpandsEvent != null)
            {
                InputFormRef.AddressListExpandsEvent += addressListExpandsEvent;
            }
        }

        public static void MakeAllDataLength(List<Address> list,uint pointer, string appendName)
        {
            uint addr = Program.ROM.p32(pointer);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            string name = "RandomChest mapid:" + appendName;
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(pointer);

            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
        private void OnPreWrite(object sender, EventArgs e)
        {
            if (this.B0.Value == 0)
            {
                this.B1.Value = 0;
            }
        }

    }
}
