using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemUsagePointerForm : Form
    {
        public ItemUsagePointerForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            PromotionItemExplain.Hide();
            InputFormRef.markupJumpLabel(this.PromotionItemLink);
            StatBoosterItemExplain.Hide();
            InputFormRef.markupJumpLabel(this.StatBoosterItemLink);

            this.InputFormRef = Init(this);
            this.InputFormRef.CheckProtectionAddrHigh = false; //書き換える対象がswitchなので低い位地に書き換えるデータがあります。
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.FilterComboBox.SelectedIndex = 0;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {
                    return false;
                }
                , (int i, uint addr) =>
                {
                    uint baseaddr = addr - (uint)(4 * i);
                    uint itemID = 0;
                    if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_usability_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_usability_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_effect_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_effect_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_promotion1_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_promotion1_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_promotion2_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_promotion2_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_staff1_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_staff1_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_staff2_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_staff2_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_statbooster1_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_statbooster1_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_statbooster2_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_statbooster2_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_errormessage_array_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_errormessage_array_switch2_address()) + (uint)i;
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.item_name_article_pointer()))
                    {
                        itemID = Program.ROM.u8(Program.ROM.RomInfo.item_name_article_switch2_address()) + (uint)i;
                    }
                    else
                    {
                        return "";
                    }
                    //Log.Debug(U.ToHexString(U.toPointer(Program.ROM.p32(addr)))+"="+ItemForm.GetItemName(itemID));
                    return U.ToHexString(itemID) + " " + ItemForm.GetItemName(itemID);
                }
                );
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PromotionItemExplain.Hide();
            StatBoosterItemExplain.Hide();

            int selected = FilterComboBox.SelectedIndex;
            string configFilename = "";
            switch (selected)
            {
                case 0: //0=アイテムを利用できるか判定する
                default:
                    configFilename = (U.ConfigDataFilename("item_usability_array_"));
                    break;
                case 1: //1=アイテムを利用した場合の効果を定義する
                    configFilename = (U.ConfigDataFilename("item_effect_array_"));
                    break;
                case 2: //2=CCアイテムを使った場合の処理を定義する
                    configFilename = (U.ConfigDataFilename("item_promotion1_array_"));
                    PromotionItemExplain.Show();
                    break;
                case 3: //3=CCアイテムかどうかを定義する(FE7のみ)
                    configFilename = (U.ConfigDataFilename("item_promotion2_array_"));
                    PromotionItemExplain.Show();
                    break;
                case 4: //4=アイテムのターゲット選択の方法を定義する(多分)
                    configFilename = (U.ConfigDataFilename("item_staff1_array_"));
                    break;
                case 5: //5=杖の種類を定義する
                    configFilename = (U.ConfigDataFilename("item_staff2_array_"));
                    break;
                case 6: //6=ドーピングアイテムを利用した時のメッセージを定義する
                    if (PatchUtil.SearchStatboosterExtends() == PatchUtil.StatboosterExtends.SkillSystems_strmag_slipt)
                    {
                        configFilename = (U.ConfigDataFilename("item_statbooster1_skill_array_"));
                    }
                    else
                    {
                        configFilename = (U.ConfigDataFilename("item_statbooster1_array_"));
                    }
                    StatBoosterItemExplain.Show();
                    break;
                case 7: //7=ドーピングアイテムとCCアイテムかどうかを定義する
                    configFilename = (U.ConfigDataFilename("item_statbooster2_array_"));
                    StatBoosterItemExplain.Show();
                    break;
                case 8: //8=エラーメッセージを定義する
                    configFilename = (U.ConfigDataFilename("item_errormessage_array_"));
                    StatBoosterItemExplain.Show();
                    break;
                case 9: //9=アイテム名の前置詞
                    configFilename = (U.ConfigDataFilename("item_name_article_"));
                    break;
            }

            this.L_0_COMBO.Items.Clear();
            if (File.Exists(configFilename))
            {
                string[] lines = File.ReadAllLines(configFilename);

                this.L_0_COMBO.BeginUpdate();
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    line = U.ClipComment(line);
                    line = line.Trim();
                    this.L_0_COMBO.Items.Add(line);
                }
                this.L_0_COMBO.EndUpdate();
            }
            
            uint addr = ReInit(selected, this.InputFormRef);
            if (addr == U.NOT_FOUND)
            {
                this.AddressList.Items.Clear();
                this.SwitchListExpandsButton.Hide();
                this.WriteButton.Hide();
                this.ERROR_NOT_FOUND.Show();
                return;
            }

            this.SwitchListExpandsButton.Show();
            this.WriteButton.Show();
            this.ERROR_NOT_FOUND.Hide();

        }

        static uint ReInit(int selected,InputFormRef ifr)
        {
            //Log.Debug("-------------------");
            if (ifr == null)
            {
                return U.NOT_FOUND;
            }
            bool enable = false;
            uint pointer = 0;
            uint addr = 0;
            uint count = 0;
            switch (selected)
            {
                case 0: //0=アイテムを利用できるか判定する
                default:
                    pointer = Program.ROM.RomInfo.item_usability_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_usability_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_usability_array_switch2_address());
                    break;
                case 1: //1=アイテムを利用した場合の効果を定義する
                    pointer = Program.ROM.RomInfo.item_effect_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_effect_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_effect_array_switch2_address());
                    break;
                case 2: //2=CCアイテムを使った場合の処理を定義する
                    pointer = Program.ROM.RomInfo.item_promotion1_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_promotion1_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_promotion1_array_switch2_address());
                    break;
                case 3: //3=CCアイテムかどうかを定義する(FE7のみ)
                    pointer = Program.ROM.RomInfo.item_promotion2_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_promotion2_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_promotion2_array_switch2_address());
                    break;
                case 4: //4=アイテムのターゲット選択の方法を定義する(多分)
                    pointer = Program.ROM.RomInfo.item_staff1_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_staff1_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_staff1_array_switch2_address());
                    break;
                case 5: //5=杖の種類を定義する
                    pointer = Program.ROM.RomInfo.item_staff2_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_staff2_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_staff2_array_switch2_address());
                    break;
                case 6: //6=ドーピングアイテムを利用した時のメッセージを定義する
                    pointer = Program.ROM.RomInfo.item_statbooster1_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_statbooster1_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_statbooster1_array_switch2_address());
                    break;
                case 7: //7=ドーピングアイテムとCCアイテムかどうかを定義する
                    pointer = Program.ROM.RomInfo.item_statbooster2_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_statbooster2_array_switch2_address()+2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_statbooster2_array_switch2_address());
                    break;
                case 8: //8=エラーメッセージを定義する
                    pointer = Program.ROM.RomInfo.item_errormessage_array_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_errormessage_array_switch2_address() + 2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_errormessage_array_switch2_address());
                    break;
                case 9: //9=アイテム名の前置詞
                    pointer = Program.ROM.RomInfo.item_name_article_pointer();
                    addr = Program.ROM.p32(pointer);
                    count = Program.ROM.u8(Program.ROM.RomInfo.item_name_article_switch2_address() + 2);
                    enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.item_name_article_switch2_address());
                    break;
            }
            if (enable == false)
            {
                return U.NOT_FOUND;
            }
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }

            ifr.ReInitPointer(pointer, count + 1);
            return addr;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            for (int n = 0; n < 10; n++)
            {
                uint addr = ReInit(n, InputFormRef);
                if (addr == U.NOT_FOUND)
                {
                    continue;
                }

                string name = "ItemUsageP" + n;
                FEBuilderGBA.Address.AddAddress(list
                    , InputFormRef
                    , name
                    , new uint[] { 0 }
                    , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

                List<U.AddrResult> arlist = InputFormRef.MakeList();
                FEBuilderGBA.Address.AddFunctions(list, arlist, 0, " " + name);
            }
        }

        public void JumpTo(uint search_item_id)
        {
            this.InputFormRef.JumpTo(search_item_id);
        }

        private void SwitchListExpandsButton_Click(object sender, EventArgs e)
        {
            int selected = this.FilterComboBox.SelectedIndex;
            uint newCount = ItemForm.DataCount();

            if (this.L_0_COMBO.Items.Count <= 0)
            {
                return ;
            }
            uint defAddr = U.FindComboSelectHexFromValueWhereName(this.L_0_COMBO, "-");
            if (defAddr == U.NOT_FOUND)
            {//見つからない場合、先頭の要素を利用します
                defAddr = U.atoh(this.L_0_COMBO.Items[0].ToString());
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ItemUsage SwitchExpands" + selected);
            switch (selected)
            {
                case 0: //0=アイテムを利用できるか判定する
                default:
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_usability_array_pointer()
                        , Program.ROM.RomInfo.item_usability_array_switch2_address()
                        , newCount
                        , defAddr
                        ,undodata);
                    break;
                case 1: //1=アイテムを利用した場合の効果を定義する
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_effect_array_pointer()
                        , Program.ROM.RomInfo.item_effect_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 2: //2=CCアイテムを使った場合の処理を定義する
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_promotion1_array_pointer()
                        , Program.ROM.RomInfo.item_promotion1_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 3: //3=CCアイテムかどうかを定義する(FE7のみ)
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_promotion2_array_pointer()
                        , Program.ROM.RomInfo.item_promotion2_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 4: //4=アイテムのターゲット選択の方法を定義する(多分)
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_staff1_array_pointer()
                        , Program.ROM.RomInfo.item_staff1_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 5: //5=杖の種類を定義する
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_staff2_array_pointer()
                        , Program.ROM.RomInfo.item_staff2_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 6: //6=ドーピングアイテムを利用した時のメッセージを定義する
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_statbooster1_array_pointer()
                        , Program.ROM.RomInfo.item_statbooster1_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 7: //7=ドーピングアイテムとCCアイテムかどうかを定義する
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_statbooster2_array_pointer()
                        , Program.ROM.RomInfo.item_statbooster2_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 8: //8=エラーメッセージを定義する
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_errormessage_array_pointer()
                        , Program.ROM.RomInfo.item_errormessage_array_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
                case 9: //9=アイテム名の前置詞を定義する
                    PatchUtil.Switch2Expands(Program.ROM.RomInfo.item_name_article_pointer()
                        , Program.ROM.RomInfo.item_name_article_switch2_address()
                        , newCount
                        , defAddr
                        , undodata);
                    break;
            }

            Program.Undo.Push(undodata);

            ReInit(selected,this.InputFormRef);
        }

        private void PromotionItemLink_Click(object sender, EventArgs e)
        {
            uint itemid = U.atoh(this.AddressList.Text);
            InputFormRef.JumpForm<ItemPromotionForm>(itemid, "ITEM_LIST");
        }

        private void StatBoosterItemLink_Click(object sender, EventArgs e)
        {
            uint itemid = U.atoh(this.AddressList.Text);
            InputFormRef.JumpForm<ItemStatBonusesForm>(itemid);
        }

    }
}
