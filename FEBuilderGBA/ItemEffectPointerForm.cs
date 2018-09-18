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
    public partial class ItemEffectPointerForm : Form
    {
        public ItemEffectPointerForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            EffectDic = U.LoadDicResource(U.ConfigDataFilename("item_anime_effect_"));
        }
        Dictionary<uint, string> EffectDic;

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.item_effect_pointer_table_pointer()
                , 4
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    if (!U.isPointerOrNULL(a))
                    {
                        return false;
                    }
                    if (a == 0)
                    {//nullがあるらしいのでその場合無視.
                        return true;
                    }
                    if (a <= 0x08000100)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + "C"+i.ToString("X02");
                }
                );
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.X_NAME.Text = U.at(EffectDic, this.AddressList.SelectedIndex);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "ItemEffectPointer";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_MIX);

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            int limit = Math.Min((int)Program.ROM.RomInfo.magic_effect_original_data_count()
                , arlist.Count); //0x48以降は魔法テーブルです.

            for (int i = 0; i < limit; i++)
            {
                uint pointer = arlist[i].addr;
                FEBuilderGBA.Address.AddFunction(list, pointer, arlist[i].name + name);
            }
        }
    
    }
}
