using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;

namespace FEBuilderGBA
{
    class ImageUtilMagic
    {
        static magic_system_enum  g_Cache_magic_system_enum;
        public static void ClearCache()
        {
            g_Cache_magic_system_enum = magic_system_enum.NoCache;
            g_Cache_CSASpellTableAddr = U.NOT_FOUND;
            g_Cache_CSASpellTablePointer = U.NOT_FOUND;
        }

        //魔法システムの判別.
        public enum magic_system_enum
        {
             NO = 0
           , FEDITOR_ADV = 1
           , CSA_CREATOR = 2
           , NoCache = 0xFF
        };
        public static magic_system_enum SearchMagicSystem()
        {
            if (g_Cache_magic_system_enum == magic_system_enum.NoCache)
            {
                g_Cache_magic_system_enum = SearchMagicSystemLow();
            }
            return g_Cache_magic_system_enum;
        }
        static magic_system_enum SearchMagicSystemLow()
        {
            uint baseaddr, dimaddr, nodimaddr;
            return SearchMagicSystem(out baseaddr, out dimaddr, out nodimaddr);
        }

        public struct MagicPatchTableSt
        {
            public string name;
            public string ver;
            public uint addr;
            public byte[] data;
            public uint dim;
            public uint no_dim;
        };
        public static magic_system_enum SearchMagicSystem(out uint baseaddr, out uint dimaddr, out uint nodimaddr)
        {
            MagicPatchTableSt[] table = new MagicPatchTableSt[] { 
                new MagicPatchTableSt{ name="SCA_Creator",	ver = "FE8U", addr = 0x95d780,data = new byte[]{0x01 ,0x00 ,0x00 ,0x00 ,0x90 ,0xD7 ,0x95 ,0x08 ,0x03 ,0x00 ,0x00 ,0x00 ,0xD9 ,0xD8 ,0x95 ,0x08},dim = 0x95d7ed,no_dim = 0x95d899},
                new MagicPatchTableSt{ name="FEditor",	ver = "FE8U", addr = 0x95d780,data = new byte[]{0x01 ,0x00 ,0x00 ,0x00 ,0x90 ,0xD7 ,0x95 ,0x08 ,0x03 ,0x00 ,0x00 ,0x00 ,0x39 ,0xD9 ,0x95 ,0x08},dim = 0x95D7ED,no_dim = 0x95D8EF},
                new MagicPatchTableSt{ name="SCA_Creator",	ver = "FE8J", addr = 0x9cd3bc,data = new byte[]{0x01 ,0x00 ,0x00 ,0x00 ,0xCC ,0xD3 ,0x9C ,0x08 ,0x03 ,0x00 ,0x00 ,0x00 ,0x15 ,0xD5 ,0x9C ,0x08},dim = 0x9CD429,no_dim = 0x9CD4D5},
                new MagicPatchTableSt{ name="SCA_Creator",	ver = "FE8J", addr = 0x5BDC80,data = new byte[]{0x01 ,0x00 ,0x00 ,0x00 ,0xCC ,0xD3 ,0x9C ,0x08 ,0x03 ,0x00 ,0x00 ,0x00 ,0x15 ,0xD5 ,0x9C ,0x08},dim = 0x5BDCED,no_dim = 0x5BDD99},	//fixed version
                new MagicPatchTableSt{ name="FEditor",	ver = "FE8J", addr = 0xEFBE00,data = new byte[]{0x01 ,0x00 ,0x00 ,0x00 ,0x10 ,0xBE ,0xEF ,0x08 ,0x03 ,0x00 ,0x00 ,0x00 ,0xB9 ,0xBF ,0xEF ,0x08},dim = 0xEFBE6D,no_dim = 0xEFBF6F},
                new MagicPatchTableSt{ name="SCA_Creator",	ver = "FE7U", addr = 0xCB680,data = new byte[]{0x19 ,0x00 ,0x00 ,0x00 ,0x90 ,0xB6 ,0x0C ,0x08 ,0x03 ,0x00 ,0x00 ,0x00 ,0xD9 ,0xB7 ,0x0C ,0x08},dim = 0xCB6ED,no_dim = 0xCB799},
                new MagicPatchTableSt{ name="FEditor",	ver = "FE7U", addr = 0xCB680,data =     new byte[]{0x19, 0x00, 0x00, 0x00, 0x00 ,0x00 ,0x00 ,0x00 ,0x03 ,0x00 ,0x00 ,0x00 ,0xD9 ,0xB7 ,0x0C ,0x08},dim = 0xCB699,no_dim = 0xCB787},
                new MagicPatchTableSt{ name="FEditor",	ver = "FE7J", addr = 0xC69B4,data = new byte[]{0x19 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x03 ,0x00 ,0x00 ,0x00 ,0x29 ,0x6B ,0x0C ,0x08},dim = 0xC69CD,no_dim = 0xC69CD},
                new MagicPatchTableSt{ name="SCA_Creator",	ver = "FE6", addr = 0x2DC078,data = new byte[]{0x19 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x03 ,0x00 ,0x00 ,0x00 ,0x61 ,0xC1 ,0x2D ,0x08},dim = 0x2DC091,no_dim = 0x2dc129},
                new MagicPatchTableSt{ name="FEditor",	ver = "FE6", addr = 0x2DC078,data = new byte[]{0x19 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x03 ,0x00 ,0x00 ,0x00 ,0xC5 ,0xC1 ,0x2D ,0x08},dim = 0x2dc091,no_dim = 0x2DC17F },
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach(MagicPatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "FEditor")
                {
                    g_Cache_CSASpellTableAddr = FindCSASpellTableLow("FEditor", out g_Cache_CSASpellTablePointer);
                    if (g_Cache_CSASpellTablePointer == U.NOT_FOUND)
                    {//テーブルが見つからないので、ぶっ壊れている
                        continue;
                    }

                    baseaddr = t.addr;
                    dimaddr = t.dim;
                    nodimaddr = t.no_dim;
                    g_Cache_magic_system_enum = magic_system_enum.FEDITOR_ADV;
                    return g_Cache_magic_system_enum;
                }
                if (t.name == "SCA_Creator")
                {
                    g_Cache_CSASpellTableAddr = FindCSASpellTableLow("SCA_Creator", out g_Cache_CSASpellTablePointer);
                    if (g_Cache_CSASpellTablePointer == U.NOT_FOUND)
                    {//テーブルが見つからないので、ぶっ壊れている
                        continue;
                    }

                    baseaddr = t.addr;
                    dimaddr = t.dim;
                    nodimaddr = t.no_dim;
                    g_Cache_magic_system_enum = magic_system_enum.CSA_CREATOR;
                    return g_Cache_magic_system_enum;
                }
            }
            baseaddr = U.NOT_FOUND;
            dimaddr = U.NOT_FOUND;
            nodimaddr = U.NOT_FOUND;
            g_Cache_magic_system_enum = magic_system_enum.NO;
            return g_Cache_magic_system_enum;
        }

        static uint g_Cache_CSASpellTableAddr = U.NOT_FOUND;
        static uint g_Cache_CSASpellTablePointer = U.NOT_FOUND;
        public static uint GetCSASpellTableAddr()
        {
            return g_Cache_CSASpellTableAddr;
        }
        public static uint GetCSASpellTablePointer()
        {
            return g_Cache_CSASpellTablePointer;
        }
        
        public struct SpellTableSt
        {
            public string name;
            public string ver;
            public byte[] data;
        };
        static uint FindCSASpellTableLow(string type, out uint out_pointer)
        {
            SpellTableSt[] table = new SpellTableSt[] { 
                new SpellTableSt{ name="SCA_Creator",	ver = "FE8U",data = new byte[]{0x1C ,0x58 ,0x05 ,0x08 ,0x00 ,0x01 ,0x00 ,0x80 ,0xED ,0xD7 ,0x95 ,0x08 ,0x99 ,0xD8 ,0x95 ,0x08}},
                new SpellTableSt{ name="FEditor",	ver = "FE8U",data = new byte[]{0x01 ,0xB4 ,0x7D ,0xE7 ,0x34 ,0xFF ,0x03 ,0x02 ,0x80 ,0xD7 ,0x95 ,0x08 ,0x1A ,0xE1 ,0x03 ,0x02}},
                new SpellTableSt{ name="SCA_Creator",	ver = "FE8J",data = new byte[]{0xB8 ,0x67 ,0x05 ,0x08 ,0x00 ,0x01 ,0x00 ,0x80 ,0x29 ,0xD4 ,0x9C ,0x08 ,0xD5 ,0xD4 ,0x9C ,0x08}},
                new SpellTableSt{ name="FEditor",	ver = "FE8J",data = new byte[]{0x01 ,0xB4 ,0x7D ,0xE7 ,0x34 ,0xFF ,0x03 ,0x02 ,0x00 ,0xBE ,0xEF ,0x08 ,0x16 ,0xE1 ,0x03 ,0x02}},
                new SpellTableSt{ name="SCA_Creator",	ver = "FE7U",data = new byte[]{0x0C ,0x06 ,0x05 ,0x08 ,0x00 ,0x01 ,0x00 ,0x80 ,0xED ,0xB6 ,0x0C ,0x08 ,0x99 ,0xB7 ,0x0C ,0x08}},
                new SpellTableSt{ name="FEditor",	ver = "FE7U",data = new byte[]{0x00 ,0x28 ,0x17 ,0xD1 ,0x18 ,0xE0 ,0x70 ,0xB5 ,0x05 ,0x1C ,0x00 ,0x20 ,0x01 ,0xB4 ,0x87 ,0xE7 ,0x34 ,0xFF ,0x03 ,0x02 ,0x80 ,0xB6 ,0x0C ,0x08 ,0x26 ,0xE0 ,0x03 ,0x02}},
                new SpellTableSt{ name="FEditor",	ver = "FE7J",data = new byte[]{0x01 ,0xB4 ,0x79 ,0xE7 ,0x34 ,0xFF ,0x03 ,0x02 ,0xB4 ,0x69 ,0x0C ,0x08 ,0xFE ,0xDF ,0x03 ,0x02}},
                new SpellTableSt{ name="SCA_Creator",	ver = "FE6",data = new byte[]{0x48 ,0x19 ,0x02 ,0x02 ,0x00 ,0x01 ,0x00 ,0x80 ,0x91 ,0xC0 ,0x2D ,0x08 ,0x29 ,0xC1 ,0x2D ,0x08}},
                new SpellTableSt{ name="FEditor",	ver = "FE6",data = new byte[]{0xE7 ,0x7D ,0xB4 ,0x01 ,0x34 ,0xFF ,0x03 ,0x02 ,0x80 ,0xD7 ,0x95 ,0x08 ,0x1A ,0xE1 ,0x03 ,0x02}},
            };


            out_pointer = U.NOT_FOUND;
            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (SpellTableSt t in table)
            {
                if (t.name != type)
                {
                    continue;
                }
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                uint start = 0x10000;
                uint f = U.Grep(Program.ROM.Data, t.data, start, 0, 4);
                if (f == U.NOT_FOUND)
                {
                    continue;
                }
                uint csa_spell_table_pointer = f + (uint)t.data.Length;
                out_pointer = csa_spell_table_pointer;

                uint csa_spell_table = Program.ROM.p32(csa_spell_table_pointer);
                if (!U.isSafetyOffset(csa_spell_table))
                {
                    continue;
                }

                return csa_spell_table;
            }
            return U.NOT_FOUND;
        }

        //魔法拡張は大量の0x00地帯が生れるので、フリー領域と誤認しないように確認する.
        public static bool IsMagicArea(ref uint addr)
        {
            magic_system_enum magicType = SearchMagicSystem();
            if (magicType == magic_system_enum.NO)
            {
                return false;
            }
            uint csaSpellTable = ImageUtilMagic.GetCSASpellTableAddr();
            if (csaSpellTable == U.NOT_FOUND)
            {
                Debug.Assert(false);
                return false;
            }

            uint effect_table_addr = Program.ROM.p32(Program.ROM.RomInfo.magic_effect_pointer());
            if (!U.isSafetyOffset(effect_table_addr))
            {
                return false;
            }

            uint end = effect_table_addr + (0x4 * 0xff);
            if (addr >= effect_table_addr && addr < end)
            {
                addr = end;
                return true;
            }
            end = csaSpellTable + (0x4 * 0x5 * 0xff);
            if (addr >= csaSpellTable && addr < end)
            {
                addr = end;
                return true;
            }
            return false;
        }
    }
}
