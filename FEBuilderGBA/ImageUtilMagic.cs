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

        public static magic_system_enum SearchMagicSystem(out uint baseaddr, out uint dimaddr, out uint nodimaddr)
        {
            string filename = U.ConfigDataFilename("magic_extends_");
            if (!U.IsRequiredFileExist(filename))
            {
                baseaddr = U.NOT_FOUND;
                dimaddr = U.NOT_FOUND;
                nodimaddr = U.NOT_FOUND;
                g_Cache_magic_system_enum = magic_system_enum.NO;
                return g_Cache_magic_system_enum;
            }

            string[] lines = File.ReadAllLines(filename);
            string version = Program.ROM.VersionToFilename();
            for (int i = 0; i < lines.Length; i++)
            {
                if (U.IsComment(lines[i]))
                {
                    continue;
                }
                string line = U.ClipComment(lines[i]);
                string[] sp = line.Split('\t');
                if (sp.Length < 3)
                {
                    continue;
                }
                if (sp[1] != version)
                {
                    continue;
                }

                string[] hexStrings = sp[3].Split(' ');
                byte[] need = new byte[hexStrings.Length];
                for (int n = 0; n < hexStrings.Length; n++)
                {
                    need[n] = (byte)U.atoh(hexStrings[n]);
                }

                //チェック開始アドレス
                uint start = U.atoh(sp[2]);

                byte[] data = Program.ROM.getBinaryData(start, need.Length);
                if (U.memcmp(need, data) != 0)
                {
                    continue;
                }
                if (sp[0] == "FEditor")
                {
                    baseaddr = start;
                    dimaddr = U.atoh(sp[4]);
                    nodimaddr = U.atoh(sp[5]);
                    g_Cache_magic_system_enum = magic_system_enum.FEDITOR_ADV;
                    return g_Cache_magic_system_enum;
                }
                if (sp[0] == "SCA_Creator")
                {
                    baseaddr = start;
                    dimaddr = U.atoh(sp[4]);
                    nodimaddr = U.atoh(sp[5]);
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
        //拡張された魔法テーブル(CSASpellTable)を検索します.
        public static uint FindCSASpellTable(string type, out uint out_pointer)
        {
            if (g_Cache_CSASpellTableAddr == U.NOT_FOUND)
            {
                g_Cache_CSASpellTableAddr = FindCSASpellTableLow(type, out g_Cache_CSASpellTablePointer);
            }
            out_pointer = g_Cache_CSASpellTablePointer;
            if (g_Cache_CSASpellTableAddr == U.NOT_FOUND)
            {
                return g_Cache_CSASpellTableAddr;
            }
            Debug.Assert(Program.ROM.p32(g_Cache_CSASpellTablePointer) == g_Cache_CSASpellTableAddr);
            return g_Cache_CSASpellTableAddr;
        }
        static uint FindCSASpellTableLow(string type, out uint out_pointer)
        {
            out_pointer = U.NOT_FOUND;

            string filename = U.ConfigDataFilename("magic_csa_spell_table_");
            string[] lines = File.ReadAllLines(filename);
            string version = Program.ROM.VersionToFilename();
            for (int i = 0; i < lines.Length; i++)
            {
                if (U.IsComment(lines[i]))
                {
                    continue;
                }
                string line = U.ClipComment(lines[i]);
                string[] sp = line.Split('\t');
                if (sp.Length < 3)
                {
                    continue;
                }
                if (sp[0] != type)
                {
                    continue;
                }
                if (sp[1] != version)
                {
                    continue;
                }

                string[] hexStrings = sp[2].Split(' ');
                byte[] need = new byte[hexStrings.Length];
                for (int n = 0; n < hexStrings.Length; n++)
                {
                    need[n] = (byte)U.atoh(hexStrings[n]);
                }

                //チェック開始アドレス
                uint start = 0x10000;
                uint f = U.Grep(Program.ROM.Data, need, start, 0, 4);
                if (f == U.NOT_FOUND)
                {
                    continue;
                }
                uint csa_spell_table_pointer = f + (uint)need.Length;
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
        public static bool IsMagicArea(uint addr)
        {
            magic_system_enum magicType = SearchMagicSystem();
            if (magicType == magic_system_enum.NO)
            {
                return false;
            }
            uint csaSpellTable, csaSpellTablePointer;
            if (magicType == magic_system_enum.CSA_CREATOR)
            {
                csaSpellTable = ImageUtilMagic.FindCSASpellTable("SCA_Creator", out csaSpellTablePointer);
            }
            else if (magicType == magic_system_enum.FEDITOR_ADV)
            {
                csaSpellTable = ImageUtilMagic.FindCSASpellTable("FEditor", out csaSpellTablePointer);
            }
            else
            {
                Debug.Assert(false);
                return false;
            }

            uint effect_table_addr = Program.ROM.p32(Program.ROM.RomInfo.magic_effect_pointer());
            if (!U.isSafetyOffset(effect_table_addr))
            {
                return false;
            }

            if (addr >= effect_table_addr && addr < effect_table_addr + (0x4 * 0xff))
            {
                return true;
            }
            if (addr >= csaSpellTable && addr < csaSpellTable + (0x4 * 0x5 * 0xff))
            {
                return true;
            }
            return false;
        }
    }
}
