using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemStatBonusesForm : Form
    {
        public ItemStatBonusesForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            this.ItemListBox.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            this.ItemListBox.ItemListToJumpForm( "ITEM");
            this.InputFormRef = Init(this);
            this.InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので、警告不能.
            this.InputFormRef.IsSurrogateStructure = true; //アイテムの構造体を利用するがサイズは異なる.
            this.InputFormRef.MakeGeneralAddressListContextMenu(false);

            InputFormRef.markupJumpLabel(X_EXPAIN_HOWTOADD);

            this.BlockSize.Text = "12";
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.item_pointer
                , Program.ROM.RomInfo.item_datasize
                , (int i, uint addr) =>
                {//12補正 16特効 がポインタ or nullであれば
                    return U.isPointerOrNULL(Program.ROM.u32(addr + 12))
                        && U.isPointerOrNULL(Program.ROM.u32(addr + 16))
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint ITEMSTATBOOSTER = Program.ROM.u32(addr + 12);
                    if (! U.isPointer(ITEMSTATBOOSTER))
                    {
                        return new U.AddrResult();
                    }

                    U.AddrResult ar = new U.AddrResult();
                    ar.addr = U.toOffset( ITEMSTATBOOSTER );

                    uint id = Program.ROM.u16(addr);
                    ar.name = U.ToHexString(i) + " " + TextForm.Direct(id);

                    return ar;
                }
                );
        }

        private void ITEMSTATBOOSTERForm_Load(object sender, EventArgs e)
        {
            //英語版の魔法分離パッチ
            MagicSplitUtil.magic_split_enum magic_split = MagicSplitUtil.SearchMagicSplit();
            if (magic_split == MagicSplitUtil.magic_split_enum.FE8UMAGIC ||
                magic_split == MagicSplitUtil.magic_split_enum.FE7UMAGIC)
            {
                J_9.Text = R._("魔力");
            }
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //該当アイテム.
            uint selectaddress = (uint)this.Address.Value;
            List<U.AddrResult>  list = ItemForm.MakeItemList( (uint addr) =>
            {
                uint ITEMSTATBOOSTER = Program.ROM.p32(addr + 12);
                return (selectaddress == ITEMSTATBOOSTER);
            }
            );
            U.ConvertListBox(list, ref this.ItemListBox);
        }

        public static uint[] GetValues(uint pointer)
        {
            uint[] ret = new uint[12];
            pointer = U.toOffset(pointer);
            if (!U.isSafetyOffset(pointer + 11))
            {
                return ret;
            }

            ret[0] = Program.ROM.u8(pointer + 0); //HP
            ret[1] = Program.ROM.u8(pointer + 1); //攻撃
            ret[2] = Program.ROM.u8(pointer + 2); //技
            ret[3] = Program.ROM.u8(pointer + 3); //速さ
            ret[4] = Program.ROM.u8(pointer + 4); //守備
            ret[5] = Program.ROM.u8(pointer + 5); //魔防
            ret[6] = Program.ROM.u8(pointer + 6); //幸運
            ret[7] = Program.ROM.u8(pointer + 7); //移動
            ret[8] = Program.ROM.u8(pointer + 8); //体格
            ret[9] = Program.ROM.u8(pointer + 9); //??
            ret[10] = Program.ROM.u8(pointer + 10); //??
            ret[11] = Program.ROM.u8(pointer + 11); //??

            return ret;
        }

        private void X_EXPAIN_HOWTOADD_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version == 6)
            {
                InputFormRef.JumpForm<ItemFE6Form>();
            }
            else
            {
                InputFormRef.JumpForm<ItemForm>();
            }
        }

    }
}
