using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapTerrainFloorLookupTableForm : Form
    {
        public MapTerrainFloorLookupTableForm()
        {
            InitializeComponent();
            U.ConvertComboBox(InputFormRef.MakeTerrainSet(), ref FilterComboBox, true);
            this.InputFormRef = Init(this);
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
                    string name;
                    if (Program.ROM.RomInfo.is_multibyte())
                    {
                        name = MapTerrainNameForm.GetName((uint)i);
                    }
                    else
                    {
                        name = MapTerrainNameEngForm.GetName((uint)i);
                    }

                    return U.ToHexString(i + 1) + " " + name; 
                }
                );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "MapTerrainFloorLookupTable";

            InputFormRef InputFormRef = Init(null);
            uint[] pointers = GetPointers();
            for (int i = 0; i < pointers.Length; i++)
            {
                InputFormRef.ReInitPointer(pointers[i]);
            }
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }

        private void MapTerrainFloorLookupTableForm_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(FilterComboBox, 0);
        }

        static uint[] GetPointers()
        {
            return new uint[] { 
                Program.ROM.RomInfo.lookup_table_battle_terrain_00_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_01_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_02_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_03_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_04_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_05_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_06_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_07_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_08_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_09_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_10_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_11_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_12_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_13_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_14_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_15_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_16_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_17_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_18_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_19_pointer()
               ,Program.ROM.RomInfo.lookup_table_battle_terrain_20_pointer()
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
    }
}
