using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemEffectivenessForm : Form
    {
        public ItemEffectivenessForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            this.N_AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.ItemListBox.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            this.ItemListBox.ItemListToJumpForm( "ITEM");
            
            this.InputFormRef = Init(this);
            this.InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので、警告不能.
            this.InputFormRef.IsSurrogateStructure = true; //アイテムの構造体を利用するがサイズは異なる.
            this.N_InputFormRef = N_Init(this);

            this.N_InputFormRef.AddressListExpandsEvent += N_AddressListExpandsEvent;
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.item_pointer()
                , Program.ROM.RomInfo.item_datasize()
                , (int i, uint addr) =>
                {//12補正 16特効 がポインタ or nullであれば
                    return U.isPointerOrNULL(Program.ROM.u32(addr + 12))
                        && U.isPointerOrNULL(Program.ROM.u32(addr + 16))
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint itemCritical = Program.ROM.u32(addr + 16);
                    if (!U.isPointer(itemCritical))
                    {
                        return new U.AddrResult();
                    }

                    U.AddrResult ar = new U.AddrResult();
                    ar.addr = U.toOffset(itemCritical);

                    uint id = Program.ROM.u16(addr);
                    ar.name = U.ToHexString(i) + " " + TextForm.Direct(id);

                    return ar;
                }
                );
        }
        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , 0
                , 1
                , (int i, uint addr) =>
                {//12補正 16特効 がポインタ or nullであれば
                    return Program.ROM.u8(addr) != 0;
                }
                , (int i, uint addr) =>
                {
                    uint class_id = Program.ROM.u8(addr);

                    U.AddrResult ar = new U.AddrResult();
                    ar.addr = addr;

                    ar.name = U.ToHexString(class_id) + " " + ClassForm.GetClassName(class_id);

                    return ar;
                }
                );
        }

        private void ItemCriticalForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //該当アイテム.
            uint selectaddress = (uint)this.Address.Value;
            List<U.AddrResult> list = ItemForm.MakeItemList( (uint addr) =>
            {
                uint itemCritical = Program.ROM.p32(addr + 16);
                return (selectaddress == itemCritical);
            }
            );
            U.ConvertListBox(list, ref this.ItemListBox);

            this.N_InputFormRef.ReInit(selectaddress);
        }
        public static List<U.AddrResult> MakeCriticalClassList(uint addr)
        {
            InputFormRef N_InputFormRef = N_Init(null);
            N_InputFormRef.ReInit(addr);
            return N_InputFormRef.MakeList();
        }
        //リストが拡張されたとき
        void N_AddressListExpandsEvent(object sender, EventArgs arg)
        {
            int index = this.AddressList.SelectedIndex;
            ReloadListButton.PerformClick();
            U.ForceUpdate(this.AddressList, index);
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }
    }
}
