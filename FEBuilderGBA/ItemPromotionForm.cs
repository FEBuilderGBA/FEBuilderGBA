using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemPromotionForm : Form
    {
        public ItemPromotionForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.ITEM_LIST.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            MakeCCItemList();

            if (PatchUtil.ItemUsingExtendsPatch() == PatchUtil.ItemUsingExtends.IER)
            {
                InputFormRef.markupJumpLabel(this.X_IER_PATCH);
                this.X_IER_PATCH.Show();
            }
            else
            {
                this.X_IER_PATCH.Hide();
            }

            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;
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
                    uint class_id = Program.ROM.u8(addr);
                    return U.ToHexString(class_id) + " " + ClassForm.GetClassName(class_id);
                }
                );
        }

        private void MakeCCItemList()
        {
            uint id;
            id = Program.ROM.RomInfo.cc_item_hero_crest_itemid();
            this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));
            
            id = Program.ROM.RomInfo.cc_item_knight_crest_itemid();
            this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));
            
            id = Program.ROM.RomInfo.cc_item_orion_bolt_itemid();
            this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));
            
            id = Program.ROM.RomInfo.cc_elysian_whip_itemid();
            this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));

            id = Program.ROM.RomInfo.cc_guiding_ring_itemid();
            this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));

            if (Program.ROM.RomInfo.version() >= 7)
            {
                id = Program.ROM.RomInfo.cc_fallen_contract_itemid();
                this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));

                id = Program.ROM.RomInfo.cc_master_seal_itemid();
                this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));

                id = Program.ROM.RomInfo.cc_ocean_seal_itemid();
                this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));

                id = Program.ROM.RomInfo.cc_moon_bracelet_itemid();
                this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));

                id = Program.ROM.RomInfo.cc_sun_bracelet_itemid();
                this.ITEM_LIST.Items.Add(U.ToHexString(id) + " " + ItemForm.GetItemName(id));
            }
        }


        private void ItemCCForm_Load(object sender, EventArgs e)
        {
            
        }

        private void ITEM_LIST_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ITEM_LIST.SelectedIndex)
            {
                case 0: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_item_hero_crest_pointer())); break;
                case 1: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_item_knight_crest_pointer())); break;
                case 2: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_item_orion_bolt_pointer())); break;
                case 3: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_elysian_whip_pointer())); break;
                case 4: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_guiding_ring_pointer())); break;
                case 5: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_fallen_contract_pointer())); break;
                case 6: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_master_seal_pointer())); break;
                case 7: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_ocean_seal_pointer())); break;
                case 8: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_moon_bracelet_pointer())); break;
                case 9: this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_sun_bracelet_pointer())); break;
            }
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            int index = this.AddressList.SelectedIndex;
            ReloadListButton.PerformClick();
            U.ForceUpdate(this.AddressList, index);
        }


        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "CCItem";
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_item_hero_crest_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_item_knight_crest_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_item_orion_bolt_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_elysian_whip_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_guiding_ring_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_fallen_contract_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_master_seal_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_ocean_seal_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_moon_bracelet_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            
            InputFormRef.ReInitPointer((Program.ROM.RomInfo.cc_sun_bracelet_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }

        private void X_IER_PATCH_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("ItemEffectRevamp", 0, PatchForm.SortEnum.SortName);
        }
    }
}
