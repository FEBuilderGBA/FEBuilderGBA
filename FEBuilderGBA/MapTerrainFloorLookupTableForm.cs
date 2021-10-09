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
            U.ConvertComboBox(InputFormRef.GetTerrainSetDic(), ref FilterComboBox, true);
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
                    return U.ToHexString(i) + " " + name; 
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
                if (pointers[i] == 0)
                {
                    continue;
                }
                InputFormRef.ReInitPointer(pointers[i]);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name + U.ToHexString(i), new uint[] { });
            }
        }
        public static List<U.AddrResult> MakeListByUseTerrain(uint terrainid)
        {
            List<U.AddrResult> ret = new List<U.AddrResult>();

            InputFormRef InputFormRef = Init(null);
            var terrain_set_list = U.DictionaryToValuesList(InputFormRef.GetTerrainSetDic());
            uint[] pointers = GetPointers();
            for (int i = 0; i < pointers.Length; i++)
            {
                if (i >= terrain_set_list.Count)
                {
                    break;
                }
                if (pointers[i] == 0)
                {
                    continue;
                }
                InputFormRef.ReInitPointer(pointers[i]);
                List<U.AddrResult> a = InputFormRef.MakeList((uint addr) =>
                {
                    uint icon = Program.ROM.u8(addr + 0);
                    icon = icon - 1;
                    return (icon == terrainid);
                });
                string name = U.ToHexString(i) + ":" + terrain_set_list[i];
                InputFormRef.AppendNameString(a, "",name);

                ret.AddRange(a);
            }
            return ret;
        }

        private void MapTerrainFloorLookupTableForm_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(FilterComboBox, 0);
        }

        static uint[] g_PointersCache = null;
        public static void ClearCache()
        {
            g_PointersCache = null;
        }
        static uint[] GetPointers()
        {
            if (g_PointersCache == null)
            {
                if (PatchUtil.SearchExtendsBattleBG() == PatchUtil.ExtendsBattleBG_extends.Extends)
                {
                    g_PointersCache = MapTerrainBGLookupTableForm.GetPointersExtendsPatch(0);
                }
                else
                {
                    g_PointersCache = GetPointersVanilla();
                }
            }
            return g_PointersCache;
        }
        static uint[] GetPointersVanilla()
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
            ERROR_Not_Allocated.Hide();

            uint[] pointers = GetPointers();
            int selected = FilterComboBox.SelectedIndex;
            if (selected < 0
                || selected >= pointers.Length)
            {
                return ;
            }

            if (PatchUtil.SearchExtendsBattleBG() == PatchUtil.ExtendsBattleBG_extends.Extends)
            {
                if (U.IsBadPointerPointer(pointers[selected]))
                {
                    ERROR_Not_Allocated.Show();
                }
            }

            InputFormRef.ReInitPointer(pointers[selected]);
        }

        private void X_JUMP_BG_Click(object sender, EventArgs e)
        {
            MapTerrainBGLookupTableForm f = (MapTerrainBGLookupTableForm)
                InputFormRef.JumpForm<MapTerrainBGLookupTableForm>();
            f.JumpTo((uint)FilterComboBox.SelectedIndex, (uint)AddressList.SelectedIndex);
        }

        public void JumpTo(uint filterSelected, uint listSelected)
        {
            U.SelectedIndexSafety(FilterComboBox, filterSelected);
            U.SelectedIndexSafety(AddressList, listSelected);
        }
        public static void JumpToRef(string text)
        {
            string[] ptrn = RegexCache.Split(text , @"([0-9a-zA-Z]+) .+? ([0-9a-zA-Z]+):");
            if (ptrn.Length <= 2)
            {
                return;
            }
            uint listSelected = U.atoh(ptrn[1]);
            uint filterSelected = U.atoh(ptrn[2]);
            MapTerrainFloorLookupTableForm f = (MapTerrainFloorLookupTableForm)
                InputFormRef.JumpForm<MapTerrainFloorLookupTableForm>();
            f.JumpTo(filterSelected, listSelected);
        }
        public static byte[] GetDefaultData()
        {
            uint p = Program.ROM.RomInfo.lookup_table_battle_terrain_00_pointer();
            p = Program.ROM.p32(p);
            if (!U.isSafetyOffset(p))
            {
                return new byte[Program.ROM.RomInfo.map_terrain_type_count()];
            }
            return Program.ROM.getBinaryData(p, Program.ROM.RomInfo.map_terrain_type_count());
        }
        public static void MakeDataLength(List<Address> list, uint pointer, string strname)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(pointer);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, strname, new uint[] { });
        }
        static uint GetFilterIndexOfAddr(uint addr)
        {
            addr = U.toOffset(addr);

            uint filterSelected = 0;
            uint[] pointers = GetPointers();
            for (int i = 0; i < pointers.Length; i++)
            {
                uint p = pointers[i];
                if (p == 0)
                {
                    continue;
                }
                uint a = Program.ROM.p32(p);
                if (a == addr)
                {
                    filterSelected = (uint)i;
                    break;
                }
            }
            return filterSelected;
        }
        public static string GetNameByPointer(uint addr)
        {
            uint filterSelected = GetFilterIndexOfAddr(addr);
            var terrain_set_list = U.DictionaryToValuesList(InputFormRef.GetTerrainSetDic());
            if (filterSelected >= terrain_set_list.Count)
            {
                return "";
            }
            return terrain_set_list[(int)filterSelected];
        }
        public void JumpToPointer(uint addr)
        {
            uint filterSelected = GetFilterIndexOfAddr(addr);
            U.SelectedIndexSafety(FilterComboBox, filterSelected);
            U.SelectedIndexSafety(AddressList, 0);
        }

        private void ERROR_Not_Allocated_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("extendsBattleBG", 0);
        }
    }
}
