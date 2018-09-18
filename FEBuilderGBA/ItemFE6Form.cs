using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemFE6Form : Form
    {
        public ItemFE6Form()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);

            InputFormRef.LoadComboResource(L_30_COMBO, U.ConfigDataFilename("item_staff_use_effect_"));
            InputFormRef.LoadComboResource(L_31_COMBO, U.ConfigDataFilename("item_weapon_effect_"));
            this.CLASS_LISTBOX.ItemListToJumpForm("CLASS");

            this.InputFormRef = Init(this);
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            InputFormRef.markupJumpLabel(JumpToITEMEFFECT);
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
                    return U.isPointerOrNULL( Program.ROM.u32(addr + 12) )
                        && U.isPointerOrNULL(Program.ROM.u32(addr + 16))
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u16(addr);
                    return U.ToHexString(i) + " " + TextForm.Direct(id);
                }
                );
        }



        private void ItemFE6Form_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            ToolTipEx tooltip = InputFormRef.GetToolTip<ItemFE6Form>();
            InputFormRef.LoadCheckboxesResource(U.ConfigDataFilename("item_checkbox_"), controls, tooltip, "", "L_8_BIT_", "L_9_BIT_", "L_10_BIT_", "L_11_BIT_");
        }


        private void P12_ValueChanged(object sender, EventArgs e)
        {
            uint[] bonuses = ItemStatBonusesForm.GetValues((uint)P12.Value);
            X_SIM_HP.Value = (sbyte)bonuses[0]; //HP
            X_SIM_STR.Value = (sbyte)bonuses[1]; //攻撃
            X_SIM_SKILL.Value = (sbyte)bonuses[2]; //技
            X_SIM_SPD.Value = (sbyte)bonuses[3]; //速さ
            X_SIM_DEF.Value = (sbyte)bonuses[4]; //守備
            X_SIM_RES.Value = (sbyte)bonuses[5]; //魔防
            X_SIM_LUCK.Value = (sbyte)bonuses[6]; //幸運
            X_SIM_MOVE.Value = (sbyte)bonuses[7]; //移動
            X_SIM_BODY.Value = (sbyte)bonuses[8]; //体格
        }
        private void P16_ValueChanged(object sender, EventArgs e)
        {
            List<U.AddrResult> arlist = ItemEffectivenessForm.MakeCriticalClassList((uint)P16.Value);
            U.ConvertListBox(arlist, ref CLASS_LISTBOX);
        }

        private void W26_ValueChanged(object sender, EventArgs e)
        {
            X_VALUE_SHOP.Value = B20.Value * W26.Value;
            X_VALUE_SHINGEKI_SHOP.Value = (uint)(((uint)(B20.Value * W26.Value)) * 1.5);
            X_VALUE_SEL.Value = (B20.Value * W26.Value) / 2;
        }

        private void JumpToITEMEFFECT_Click(object sender, EventArgs e)
        {
            ItemWeaponEffectForm f = (ItemWeaponEffectForm)InputFormRef.JumpForm<ItemWeaponEffectForm>();
            f.JumpTo((uint)this.AddressList.SelectedIndex);
        }



    }
}
