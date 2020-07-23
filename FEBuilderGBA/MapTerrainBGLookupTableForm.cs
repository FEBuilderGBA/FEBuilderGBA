using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapTerrainBGLookupTableForm : Form
    {
        public MapTerrainBGLookupTableForm()
        {
            InitializeComponent();
            U.ConvertComboBox(InputFormRef.MakeTerrainSet(), ref FilterComboBox, true);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < Program.ROM.RomInfo.map_terrain_type_count();
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    string name = MapTerrainNameForm.GetName((uint)i);
                    return U.ToHexString(i + 1) + " " + name; 
                }
                );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "MapTerrainBGLookupTable";

            InputFormRef InputFormRef = Init(null);
            uint[] pointers = GetPointers();
            for (int i = 0; i < pointers.Length; i++)
            {
                InputFormRef.ReInitPointer(pointers[i]);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + U.ToHexString(i), new uint[] { });
            }
        }
        public static List<U.AddrResult> MakeListByUseTerrain(uint terrainid)
        {
            List<U.AddrResult> ret = new List<U.AddrResult>();

            InputFormRef InputFormRef = Init(null);
            var terrain_set_list = U.DictionaryToValuesList( InputFormRef.MakeTerrainSet() );
            uint[] pointers = GetPointers();
            for (int i = 0; i < pointers.Length; i++)
            {
                InputFormRef.ReInitPointer(pointers[i]);
                List<U.AddrResult> a = InputFormRef.MakeList((uint addr) =>
                {
                    uint icon = Program.ROM.u8(addr + 0);
                    icon = icon - 1;
                    return (icon == terrainid);
                });
                if (i >= terrain_set_list.Count)
                {
                    continue;
                }
                string name = U.ToHexString(i) + ":" + terrain_set_list[i];
                InputFormRef.AppendNameString(a, "", name);

                ret.AddRange(a);
            }
            return ret;
        }

        private void MapTerrainFloorLookupTableForm_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(FilterComboBox, 0);
        }

        static uint[] GetPointers()
        {
            return new uint[] { 
                Program.ROM.RomInfo.lookup_table_battle_bg_00_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_01_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_02_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_03_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_04_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_05_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_06_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_07_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_08_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_09_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_10_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_11_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_12_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_13_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_14_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_15_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_16_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_17_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_18_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_19_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_bg_20_pointer()
            };
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint[] pointers = GetPointers();
            int selected = FilterComboBox.SelectedIndex;
            if (selected < 0
                || selected >= pointers.Length)
            {
                return ;
            }
            
            InputFormRef.ReInitPointer(pointers[selected]);
        }

        private void X_JUMP_FLOOR_Click(object sender, EventArgs e)
        {
            MapTerrainFloorLookupTableForm f = (MapTerrainFloorLookupTableForm)
                InputFormRef.JumpForm<MapTerrainFloorLookupTableForm>();
            f.JumpTo((uint)FilterComboBox.SelectedIndex, (uint)AddressList.SelectedIndex);
        }

        public void JumpTo(uint filterSelected, uint listSelected)
        {
            U.SelectedIndexSafety(FilterComboBox, filterSelected);
            U.SelectedIndexSafety(AddressList, listSelected);
        }
        public static void JumpToRef(string text)
        {
            string[] ptrn = RegexCache.Split(text, @"([0-9a-zA-Z]+) .+? ([0-9a-zA-Z]+):");
            if (ptrn.Length <= 2)
            {
                return;
            }
            uint listSelected = U.atoh(ptrn[1]) - 1;
            uint filterSelected = U.atoh(ptrn[2]);
            MapTerrainBGLookupTableForm f = (MapTerrainBGLookupTableForm)
                InputFormRef.JumpForm<MapTerrainBGLookupTableForm>();
            f.JumpTo(filterSelected, listSelected);
        }
    }
}
