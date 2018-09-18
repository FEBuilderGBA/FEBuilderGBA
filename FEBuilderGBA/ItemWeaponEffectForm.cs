using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class ItemWeaponEffectForm : Form
    {
        public ItemWeaponEffectForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            InputFormRef.OwnerDrawColorCombo(L_14_COMBO);

            InputFormRef.LoadComboResource(L_4_COMBO, MakeItemEffectAndAppendMagic() );
            U.CopyCombo(L_4_COMBO, ref L_6_COMBO);
            
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        Dictionary<uint, string> MakeItemEffectAndAppendMagic()
        {
            ImageUtilMagic.magic_system_enum magic = ImageUtilMagic.SearchMagicSystem();
            if (magic == ImageUtilMagic.magic_system_enum.FEDITOR_ADV)
            {
                return ImageMagicFEditorForm.MakeItemEffectAndAppendMagic(null);
            }
            else if (magic == ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                return ImageMagicCSACreatorForm.MakeItemEffectAndAppendMagic(null);
            }
            return U.LoadDicResource(U.ConfigDataFilename("item_anime_effect_"));
        }




        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.item_effect_pointer()
                , 16
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (Program.ROM.u16(addr) == 0xFFFF)
                    {
                        return false;
                    }
                    if (i > 10 && Program.ROM.IsEmpty(addr, 16 * 10))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint item_id = (uint)Program.ROM.u8(addr);
                    return U.ToHexString(item_id) + " " + ItemForm.GetItemName(item_id);
                }
            );
        }
        private void ItemEffectForm_Load(object sender, EventArgs e)
        {
        }
        public static string GetName(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint item_id = (uint)Program.ROM.u8(addr);
            return ItemForm.GetItemName(item_id);
        }

    
        public void JumpTo(uint search_item_id)
        {
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint item_id = (uint)Program.ROM.u8(addr);
                if (search_item_id == item_id)
                {
                    this.AddressList.SelectedIndex = i;
                    return;
                }

                addr += InputFormRef.BlockSize;
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "ItemWeaponEffect";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 8 });
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++ ,addr += InputFormRef.BlockSize)
            {
                uint mapAnime = Program.ROM.p32(addr + 8);
                if (!U.isSafetyOffset(mapAnime))
                {
                    continue;
                }
                uint itemid = Program.ROM.u8(addr + 0);
                string proc_name = name + "_PROC_" + U.ToHexString(itemid) + " " + ItemForm.GetItemName(itemid);

                FEBuilderGBA.Address.AddPointer(list 
                    , addr + 8
                    , ProcsScriptForm.CalcLengthAndCheck(mapAnime)
                    , proc_name
                    , FEBuilderGBA.Address.DataTypeEnum.PROCS
                    );
            }
        }

        //全データの取得
        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }
        //全データの取得(アイテムIDのリストで取得)
        public static Dictionary<uint, U.AddrResult> MakeDic()
        {
            Dictionary<uint, U.AddrResult> ret = new Dictionary<uint, U.AddrResult>();

            InputFormRef InputFormRef = Init(null);
            List<U.AddrResult>  list = InputFormRef.MakeList();
            for (int i = 0; i < list.Count; i++)
            {
                uint id = Program.ROM.u8(list[i].addr + 0);
                if (! ret.ContainsKey(id))
                {
                    ret[id] = list[i];
                }
            }
            return ret;
        }
        
    }
}
