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

        static bool Cache_IsCached = false;
        static Dictionary<uint, string> Cache_ItemEffectAndAppendMagic = new Dictionary<uint, string>();
        static Dictionary<uint, string> MakeItemEffectAndAppendMagic()
        {
            if (!Cache_IsCached)
            {
                Cache_ItemEffectAndAppendMagic = MakeItemEffectAndAppendMagicLow();
                Cache_IsCached = true;
            }
            return Cache_ItemEffectAndAppendMagic;
        }
        public static void ClearCache()
        {
            Cache_IsCached = false;
        }

        static Dictionary<uint, string> MakeItemEffectAndAppendMagicLow()
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
        public static string GetEffectName(uint id)
        {
            Dictionary<uint, string> dic = MakeItemEffectAndAppendMagic();
            return U.at(dic, id);
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
            this.MaximizeBox = true;
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
                    U.SelectedIndexSafety(this.AddressList, i);
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

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.ITEM_WEAPON_EFFECT, U.NOT_FOUND
                    , R._("アイテムエフェクトが極端に少ないです。破損している可能性があります。")));
            }
            Dictionary<uint, string> magic_effect_dic = MakeItemEffectAndAppendMagic();

            uint addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint id = Program.ROM.u8(addr);
                uint magic_effectid = Program.ROM.u16(addr + 4);
                if (! magic_effect_dic.ContainsKey(magic_effectid))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.ITEM_WEAPON_EFFECT, U.toOffset(addr)
                        , R._("アイテム {0}のエフェクトID「{1}」は無効です。", U.To0xHexString(id), U.To0xHexString(magic_effectid)), id));
                }

                uint map_effect_procs = Program.ROM.u32(addr + 8);
                if (map_effect_procs != 0)
                {
                    if (!U.isSafetyPointer(map_effect_procs) )
                    {//無効なポインタ
                        errors.Add(new FELint.ErrorSt(FELint.Type.ITEM_WEAPON_EFFECT, U.toOffset(addr)
                            , R._("アイテム {0}のマップ使用時エフェクトポインタ「{1}」は無効です。", U.To0xHexString(id), U.To0xHexString(map_effect_procs)), id));
                    }
                    if (map_effect_procs % 4 != 0)
                    {//無効なポインタ
                        errors.Add(new FELint.ErrorSt(FELint.Type.ITEM_WEAPON_EFFECT, U.toOffset(addr)
                            , R._("アイテム {0}のマップ使用時エフェクトポインタ「{1}」が4で割り切れません。この数字は0または4で割り切れるポインタである必要があります", U.To0xHexString(id), U.To0xHexString(map_effect_procs)), id));
                    }
                }
            }
        }
    }
}
