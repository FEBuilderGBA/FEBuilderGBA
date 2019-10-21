using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemForm : Form
    {
        
        public ItemForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            if (PatchUtil.SearchClassType() == PatchUtil.class_type_enum.SkillSystems_Rework)
            {//SkillSystemsによる 特効リワーク
                this.CLASS_LISTBOX.OwnerDraw(ListBoxEx.DrawClassTypeAndText, DrawMode.OwnerDrawFixed);
            }
            else
            {
                this.CLASS_LISTBOX.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
                this.CLASS_LISTBOX.ItemListToJumpForm("CLASS");
            }


            this.InputFormRef = Init(this);
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            InputFormRef.LoadComboResource(L_30_COMBO, U.ConfigDataFilename("item_staff_use_effect_"));
            InputFormRef.LoadComboResource(L_31_COMBO, U.ConfigDataFilename("item_weapon_effect_"));

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
                    if (i > 0xff)
                    {
                        return false;
                    }
                    return U.isPointerOrNULL(Program.ROM.u32(addr + 12))
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



        private void ItemForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            ToolTipEx tooltip = InputFormRef.GetToolTip<ItemForm>();
            InputFormRef.LoadCheckboxesResource(U.ConfigDataFilename("item_checkbox_"), controls, tooltip, "", "L_8_BIT_", "L_9_BIT_", "L_10_BIT_", "L_11_BIT_");
        }

        public static String GetItemName(uint item_id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(item_id);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint textid = Program.ROM.u16(addr);
            return TextForm.Direct(textid);
        }
        //アイコンを画く.
        public static Bitmap DrawIcon(uint item_id)
        {
            if (item_id <= 0)
            {
                return ImageUtil.BlankDummy(16);
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(item_id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy(16);
            }
            uint iconid = Program.ROM.u8(addr + 29);
            return ImageItemIconForm.DrawIconWhereID(iconid);
        }


        //アイテムリストを得る
        public static List<U.AddrResult> MakeItemList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }
        //アイテムリストを得る
        public static List<U.AddrResult> MakeItemList(Func<uint, bool> condCallback)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList(condCallback);
        }
        public static List<U.AddrResult> MakeItemListByUseIcon(uint iconid)
        {
            bool first = true;
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList((uint addr) => {
                if (first)
                {
                    first = false;
                    return false;
                }
                uint icon = Program.ROM.u8(addr + 29);
                return (icon == iconid);
            });
        }
        //手斧だけのリストを作る.
        public static List<U.AddrResult> MakeItemListByHandAxs()
        {
            InputFormRef InputFormRef = Init(null);
            List<U.AddrResult> src = InputFormRef.MakeList();
            List<U.AddrResult> dest = new List<U.AddrResult>();
            for (int i = 0; i < src.Count; i++)
            {
                uint b7 = Program.ROM.u8(src[i].addr + 7);
                if (b7 != 2)
                {//斧ではない
                    continue;
                }
                uint b25 = Program.ROM.u8(src[i].addr + 25);
                if (b25 <= 0x11)
                {//射程1
                    continue;
                }
                uint b9 = Program.ROM.u8(src[i].addr + 9);
                if ((b9 & 0x1C) > 0)
                {//専用武器 &0x04 or &0x8 or 0x10
                    continue; //専用武器のため判定不能
                }
                uint b10 = Program.ROM.u8(src[i].addr + 10);
                if ((b10 & 0x3C) > 0)
                {//専用武器 &0x04 or &0x8 or 0x10 or 0x20
                    continue; //専用武器のため判定不能
                }

                //手斧
                src[i].tag = (uint)i; //IDを入れる 参照しやすいように.
                dest.Add(src[i]);
            }
            return dest;
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
            if (PatchUtil.SearchClassType() == PatchUtil.class_type_enum.SkillSystems_Rework)
            {//SkillSystemsによる 特効リワーク
                List<U.AddrResult> arlist = ItemEffectivenessSkillSystemsReworkForm.MakeCriticalClassList((uint)P16.Value);
                U.ConvertListBox(arlist, ref CLASS_LISTBOX);
            }
            else
            {
                List<U.AddrResult> arlist = ItemEffectivenessForm.MakeCriticalClassList((uint)P16.Value);
                U.ConvertListBox(arlist, ref CLASS_LISTBOX);
            }
        }

        private void W26_ValueChanged(object sender, EventArgs e)
        {
            X_VALUE_SHOP.Value = B20.Value * W26.Value;
            X_VALUE_SHINGEKI_SHOP.Value = (uint)(((uint)(B20.Value * W26.Value)) * 1.5);
            X_VALUE_SEL.Value = (B20.Value * W26.Value) / 2;
        }

        private void X_JUMP_USEITEM_Click(object sender, EventArgs e)
        {

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "Item", new uint[] { 12 , 16 });

                //SkillSystemsによる 特効リワーク
                PatchUtil.class_type_enum effectivenesRework = PatchUtil.SearchClassType();

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    uint itemStatBonuses = Program.ROM.p32(addr + 12);
                    uint vennoExtends = Program.ROM.u8(addr + 34);
                    if (itemStatBonuses > 0)
                    {
                        if (vennoExtends == 1)
                        {//vennoの拡張 16バイトです
                            FEBuilderGBA.Address.AddAddress(list, itemStatBonuses
                                , 16
                                , addr + 12
                                , "StatBooster " + U.To0xHexString(i)
                                , FEBuilderGBA.Address.DataTypeEnum.BIN);
                        }
                        else
                        {//通常は12バイト
                            FEBuilderGBA.Address.AddAddress(list, itemStatBonuses
                                , 12
                                , addr + 12
                                , "StatBooster " + U.To0xHexString(i)
                                , FEBuilderGBA.Address.DataTypeEnum.BIN);
                        }
                    }

                    uint itemEffectiveness = Program.ROM.p32(addr + 16);
                    if (itemEffectiveness > 0)
                    {
                        if (effectivenesRework == FEBuilderGBA.PatchUtil.class_type_enum.SkillSystems_Rework)
                        {
                            List<U.AddrResult> arlist = ItemEffectivenessSkillSystemsReworkForm.MakeCriticalClassList((uint)itemEffectiveness);
                            FEBuilderGBA.Address.AddAddress(list, itemEffectiveness
                                , (uint)(arlist.Count + 1)  * 4
                                , addr + 16
                                , "ItemEffectiveness " + U.To0xHexString(i)
                                , FEBuilderGBA.Address.DataTypeEnum.BIN);
                        }
                        else
                        {
                            List<U.AddrResult> arlist = ItemEffectivenessForm.MakeCriticalClassList((uint)itemEffectiveness);
                            FEBuilderGBA.Address.AddAddress(list, itemEffectiveness
                                , (uint)(arlist.Count + 1)
                                , addr + 16
                                , "ItemEffectiveness " + U.To0xHexString(i)
                                , FEBuilderGBA.Address.DataTypeEnum.BIN);
                        }
                    }
                }
            }
        }

        public static uint GetItemWeaponLevelAddr(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }
            return Program.ROM.u8(addr + 28);
        }

        public static string ChcekTextItem1ErrorMessage(uint id, string text, uint textid, uint type)
        {
            if (type <= 7 && type != 4)
            {
                if (text == "")
                {
                    if (Program.ROM.RomInfo.version() == 8)
                    {//FE8では武器の名前を""にすると、耐久などが表示されない
                        if (Program.ROM.RomInfo.is_multibyte())
                        {
                            return R._("アイテムの説明欄が空です。\r\nFE8では、空にする場合は、倍角スペースを入れる必要があります。");
                        }
                        else
                        {
                            return R._("アイテムの説明欄が空です。\r\nFE8では、空にする場合は、空白スペースと[.]を入れる必要があります。\r\n例: \" [.]\"");
                        }
                    }
                }

                string errormessage = TextForm.GetErrorMessage(text,textid, "ITEM1");

                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
                {//FE7Uにはバグがある
                    if (id == 0x59 || id == 0x99)
                    {//無視
                        errormessage = "";
                    }
                }
                return errormessage;
            }
            else
            {
                return TextForm.GetErrorMessage(text, textid, "ITEM3");
            }
        }
        public static string ChcekTextItem2ErrorMessage(uint id, string text,uint textid, uint type)
        {
            return TextForm.GetErrorMessage(text,textid, "ITEM3");
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex > 0)
            {
                L_2_TEXT_ITEMX.ErrorMessage = ChcekTextItem1ErrorMessage((uint)B6.Value, L_2_TEXT_ITEMX.Text, (uint)W2.Value, (uint)B7.Value);
                L_4_TEXT_ITEM2.ErrorMessage = ChcekTextItem2ErrorMessage((uint)B6.Value, L_2_TEXT_ITEMX.Text, (uint)W2.Value, (uint)B7.Value);
            }
            else
            {
                L_2_TEXT_ITEMX.ErrorMessage = "";
                L_4_TEXT_ITEM2.ErrorMessage = "";
            }

            if (P12.Value == 0 
                && this.AddressList.SelectedIndex > 0)
            {
                L_12_NEWALLOC_ITEMSTATBOOSTER.Show();
            }
            else
            {
                L_12_NEWALLOC_ITEMSTATBOOSTER.Hide();
            }
            if (P16.Value == 0 
                && this.AddressList.SelectedIndex > 0)
            {
                L_16_NEWALLOC_ITEMCRTIICAL.Show();
            }
            else
            {
                L_16_NEWALLOC_ITEMCRTIICAL.Hide();
            }
        }
        public static uint DataCount()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
        }
        static void MakeCheckErrorWeaponRange(
              Dictionary<uint, U.AddrResult> itemWeaponEffectDic
            , uint item_addr
            , uint id
            ,List<FELint.ErrorSt> errors)
        {
            U.AddrResult ar;
            if (!itemWeaponEffectDic.TryGetValue(id, out ar))
            {//間接効果がないとマップアニメになります
                return;
            }


            //物理武器かどうか
            uint flag1 = Program.ROM.u8(item_addr + 8);
            if ((flag1 & 0x01) == 0x01)
            {//ダメージエフェクトがONのはず
                //ダメージエフェクトの有無
                uint IsDamageEffect = Program.ROM.u8(ar.addr + 12);
                if (IsDamageEffect == 0)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.ITEM_WEAPON_EFFECT, ar.addr
                        , R._("物理武器ですが、攻撃した時の「ダメージエフェクト」が「なし」に設定されています。"), id));
                    return;
                }
            }


            uint b25 = Program.ROM.u8(item_addr + 25);
            if (b25 <= 0x11)
            {//射程1以下
                return;
            }

            //マップ使用エフェクトがあるか?
            uint mapEffectPointer = Program.ROM.u32(ar.addr + 8);
            if (mapEffectPointer != 0 
                && false == U.isSafetyPointer(mapEffectPointer))
            {//マップ使用ポインタが危険です
                errors.Add(new FELint.ErrorSt(FELint.Type.ITEM_WEAPON_EFFECT, ar.addr
                    , R._("マップ使用ポインタが危険です。範囲外を参照してしまいます。"), id));
                return;
            }

            uint effectID = Program.ROM.u16(ar.addr + 4);
            if (effectID == 0xFFFF && mapEffectPointer == 0)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.ITEM_WEAPON_EFFECT, ar.addr
                    , R._("間接攻撃できる武器なのに、間接エフェクトが0xFFFFに設定されています。\r\nこの設定では、戦闘アニメで敵のHPが減るモーションが表示されません。"), id));
                return;
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.ITEM, U.NOT_FOUND
                    , R._("アイテムデータが極端に少ないです。破損している可能性があります。")));
            }

            Dictionary<uint, U.AddrResult> itemWeaponEffectDic = ItemWeaponEffectForm.MakeDic();

            uint item_addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, item_addr += InputFormRef.BlockSize)
            {
                uint name = Program.ROM.u16(item_addr + 0);
                FELint.CheckText(name, "NAME1", errors, FELint.Type.ITEM, item_addr, i);

                uint id = Program.ROM.u8(item_addr + 6);
                if (id == 0)
                {//ただの使っていないデータ
                    continue;
                }

                if (i > 0)
                {
                    uint info = Program.ROM.u16(item_addr + 2);
                    uint info2 = Program.ROM.u16(item_addr + 4);
                    uint type = Program.ROM.u8(item_addr + 7);
                    string errorMessage = ChcekTextItem1ErrorMessage(id , FETextDecode.Direct(info),info, type);
                    if (errorMessage != "")
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.ITEM, U.toOffset(item_addr)
                            , R._("TextID:{0}\r\n{1}", U.To0xHexString(info), errorMessage), i));
                    }

                    errorMessage = ChcekTextItem2ErrorMessage(id , FETextDecode.Direct(info2),info2, type);
                    if (errorMessage != "")
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.ITEM, U.toOffset(item_addr)
                            , R._("TextID:{0}\r\n{1}", U.To0xHexString(info2), errorMessage), i));
                    }
                }

                //武器攻撃範囲のチェック
                MakeCheckErrorWeaponRange(itemWeaponEffectDic,item_addr,i,errors);
            }
        }

        public static void MakeTextIDArray(List<UseTextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseTextID.AppendTextID(list, FELint.Type.ITEM, InputFormRef, new uint[] { 0 , 2, 4 });
        }

        private void JumpToITEMEFFECT_Click(object sender, EventArgs e)
        {
            ItemWeaponEffectForm f = (ItemWeaponEffectForm)InputFormRef.JumpForm<ItemWeaponEffectForm>();
            f.JumpTo((uint)this.AddressList.SelectedIndex);
        }

        bool IsFE8NVer2SkillNoSyo()
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                return false;
            }
            if (B30.Value != 0x2e)
            {//メティスの書でなければボツ
                return false;
            }
            if (B6.Value < 0x25)
            {//0x25未満ならばボツ
                return false;
            }
            if (Program.ROM.u32(0x28846) != 0x4B0046C0)
            {//スキルの書のパッチが当たっていない
                return false;
            }
            if (PatchUtil.SearchSkillSystem() != FEBuilderGBA.PatchUtil.skill_system_enum.FE8N_ver2)
            {
                return false;
            }
            return true;
        }


        private void B30_ValueChanged(object sender, EventArgs e)
        {
            if (IsFE8NVer2SkillNoSyo())
            {//FE8NVer2のスキルの書
                J_21.Text = R._("スキル");
                InputFormRef.markupJumpLabel(J_21);
            }
            else
            {
                J_21.Text = R._("攻撃");
                InputFormRef.unmarkupJumpLabel(J_21);
            }
        }

        private void J_21_Click(object sender, EventArgs e)
        {
            if (IsFE8NVer2SkillNoSyo())
            {
                InputFormRef.JumpTo(B21, J_21, "SKILLASSIGNMENT", new string[] {} );
            }
        }

    }
}
