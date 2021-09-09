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
            string name = "MapTerrainBGLookupTable";

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
            var terrain_set_list = U.DictionaryToValuesList( InputFormRef.GetTerrainSetDic() );
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
                InputFormRef.AppendNameString(a, "", name);

                ret.AddRange(a);
            }
            return ret;
        }

        private void MapTerrainFloorLookupTableForm_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(FilterComboBox, 0);
        }

        static uint GetExtendsPointer()
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {//FE8J
                    return 0x58D34;
                }
                else
                {//FE8U
                    return 0x57EE8;
                }
            }
            return 0x0;
        }
        public static uint[] GetPointersExtendsPatch(uint plus = 0)
        {
            uint pointer = GetExtendsPointer();
            if (pointer == 0)
            {
                return GetPointersVanilla();
            }
            uint addr = Program.ROM.p32(pointer);
            if (!U.isSafetyOffset(addr))
            {
                return GetPointersVanilla();
            }

            List<uint> pointers = new List<uint>(0x4f);
            for (int i = 0; i < 0xff; i++ , addr += 8 )
            {
                uint p = Program.ROM.u32(addr + plus);
                if (p == 0xffffffff)
                {
                    break;
                }
                pointers.Add(addr + plus);
            }
            return pointers.ToArray();
        }

        static Dictionary<uint, string> MakeCache_Cache_TerrainSetDicLow()
        {
            string filename = U.ConfigDataFilename("battleterrain_set_");
            Dictionary<uint, string> data = U.LoadDicResource(filename);

            if (PatchUtil.SearchCache_ExtendsBattleBG() != PatchUtil.ExtendsBattleBG_extends.Extends)
            {
                return data;
            }
            uint pointer = GetExtendsPointer();
            if (pointer == 0)
            {
                return data;
            }
            uint addr = Program.ROM.p32(pointer);
            if (!U.isSafetyOffset(addr))
            {
                return data;
            }

            int baseSize = data.Count;
            List<uint> pointers = new List<uint>(0x4f);
            for (int i = 0; i < 0xff; i++, addr += 8)
            {
                uint p = Program.ROM.u32(addr);
                if (p == 0xffffffff)
                {
                    break;
                }

                string name = Program.CommentCache.At(addr);
                if (i < baseSize)
                {
                    if (name != "")
                    {
                        data[(uint)i] = name;
                    }
                    continue;
                }

                if (name == "")
                {
                    name = "Extends" + U.ToHexString2(i);
                }
                data[(uint)i] = name;
            }

            return data;
        }

        static bool Cache_IsCached = false;
        static Dictionary<uint, string> Cache_TerrainSetDic = new Dictionary<uint, string>();
        public static Dictionary<uint, string> GetTerrainSetDic()
        {
            if (!Cache_IsCached)
            {
                Cache_TerrainSetDic = MakeCache_Cache_TerrainSetDicLow();
                Cache_IsCached = true;
            }
            return Cache_TerrainSetDic;
        }

        static uint[] g_PointersCache = null;
        public static void ClearCache()
        {
            Cache_IsCached = false;
            g_PointersCache = null;
        }
        static uint[] GetPointers()
        {
            if (g_PointersCache == null)
            {
                if (PatchUtil.SearchCache_ExtendsBattleBG() == PatchUtil.ExtendsBattleBG_extends.Extends)
                {
                    g_PointersCache = GetPointersExtendsPatch(4);
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
            ERROR_Not_Allocated.Hide();

            uint[] pointers = GetPointers();
            int selected = FilterComboBox.SelectedIndex;
            if (selected < 0
                || selected >= pointers.Length)
            {
                return ;
            }

            if (PatchUtil.SearchCache_ExtendsBattleBG() == PatchUtil.ExtendsBattleBG_extends.Extends)
            {
                if (U.IsBadPointerPointer(pointers[selected]))
                {
                    ERROR_Not_Allocated.Show();
                }
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
            uint listSelected = U.atoh(ptrn[1]);
            uint filterSelected = U.atoh(ptrn[2]);
            MapTerrainBGLookupTableForm f = (MapTerrainBGLookupTableForm)
                InputFormRef.JumpForm<MapTerrainBGLookupTableForm>();
            f.JumpTo(filterSelected, listSelected);
        }
        public static byte[] GetDefaultData()
        {
            uint p = Program.ROM.RomInfo.lookup_table_battle_bg_00_pointer();
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
