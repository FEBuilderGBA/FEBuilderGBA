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
    public partial class AIPerformItemForm : Form
    {
        public AIPerformItemForm()
        {
            InitializeComponent();

            PatchUtil.ItemUsingExtends itemUsingExtends = PatchUtil.ItemUsingExtendsPatch();
            if (itemUsingExtends == PatchUtil.ItemUsingExtends.IER)
            {//IERはItemIDをItemEffectIDに変えてしまう
                this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
                J_0_ITEM.Name = "J_0";
                J_0_ITEM.Text = R._("アイテム利用効果ID");
                L_0_ITEM.Name = "L_0_ITEMSTAFFUSEEFFECT";
                L_0_ITEMICON.Hide();
            }
            else
            {//バニラ
                this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            }

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.CheckProtectionAddrHigh = false;
            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ai_preform_item_pointer
                , 8
                , (int i, uint addr) =>
                {
                    return Program.ROM.u16(addr) != 0x0;
                }
                , (int i, uint addr) =>
                {
                    uint item_id = Program.ROM.u16(addr);
                    return U.ToHexString(item_id) + " " + AIPerformItemForm.GetColumnName(item_id);
                }
                );
        }

        public static string GetColumnName(uint id)
        {
            PatchUtil.ItemUsingExtends itemUsingExtends = PatchUtil.ItemUsingExtendsPatch();
            if (itemUsingExtends == PatchUtil.ItemUsingExtends.IER)
            {//IERはItemIDをItemEffectIDに変えてしまう
                return ItemForm.GetItemStaffUseEffectName(id);
            }
            else
            {//バニラ
                return ItemForm.GetItemName(id);
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "AIPerformItem", new uint[] { 4 });

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 4, "AIPerformItem_ASM_");
        }

        void AddressListExpandsEvent(object sender, EventArgs arg)
        {//アイテムテーブルの途中にある参照を変更する必要がある
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            Program.ROM.write_p32(Program.ROM.RomInfo.ai_preform_item_direct_asm_pointer, addr + 4, undodata);
            Program.Undo.Push(undodata);
        }

        private void AIPerformItemForm_Load(object sender, EventArgs e)
        {

        }
    }
}
