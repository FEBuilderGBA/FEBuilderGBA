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
using System.Collections.Concurrent;

namespace FEBuilderGBA
{
    public static class PatchUtil
    {
        static public void ClearCache()
        {
            g_Cache_portrait_extends = portrait_extends.NoCache;
            g_Cache_skill_system_enum = skill_system_enum.NoCache;
            g_Cache_draw_font_enum = draw_font_enum.NoCache;
            g_Cache_class_type_enum = class_type_enum.NoCache;
            g_Cache_itemicon_extends = itemicon_extends.NoCache;
            g_Cache_shinan_table = NO_CACHE;
            g_Cache_SkipWorldMap_enum = mnc2_fix_enum.NoCache;
            g_Cache_ImprovedSoundMixer = ImprovedSoundMixer.NoCache;
            g_LevelMaxCaps = NO_CACHE;
            g_Cache_AutoNewLine_enum = AutoNewLine_enum.NoCache;
            g_Cache_Escape_enum = Escape_enum.NoCache;
            g_Cache_FE8UItemSkill_enum = FE8UItemSkill_enum.NoCache;
            g_Cache_Raid_enum = Raid_enum.NoCache;
            g_Cache_MeleeAndMagicFix_enum = MeleeAndMagicFix_enum.NoCache;
            g_Cache_IrregularFont_enum = IrregularFont_enum.NoCache;
            g_Cache_StatboosterExtends = StatboosterExtends.NoCache;
            g_Cache_SearchFlag0x28ToMapSecondPalettePatch = MapSecondPalette_extends.NoCache;
            g_Cache_ClearTurn2x = ClearTurn2x_extends.NoCache;
            g_Cache_FourthAllegiance = FourthAllegiance_extends.NoCache;
            g_Cache_AntiHuffmanEnum = AntiHuffmanEnum.NoCache;
            g_Cache_grows_mod_enum = growth_mod_enum.NoCache;
            g_Cache_HandAxsWildCard = HandAxsWildCard_extends.NoCache;
            g_Cache_soundroom_unlock_enum = soundroom_unlock_enum.NoCache;

            g_WeaponLockArrayTableAddr = U.NOT_FOUND;
            g_InstrumentSet = null;
        }

        public const uint NO_CACHE = 0xff;
        public enum SpecialHack_enum
        {
            No,
            MoDUPS,  //FE6でマップデータの形式が拡張されているパッチ
            NoCache = (int)NO_CACHE
        }
        static SpecialHack_enum g_SpecialHack = SpecialHack_enum.NoCache;
        public static SpecialHack_enum SearchSpecialHack()
        {
            if (g_SpecialHack == SpecialHack_enum.NoCache)
            {
                g_SpecialHack = SearchSpecialHackLow();
            }
            return g_SpecialHack;
        }
        static SpecialHack_enum SearchSpecialHackLow()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                if (Program.ROM.u16(0x2BB12) == 0x2048)
                {
                    return SpecialHack_enum.MoDUPS;
                }
            }
            return SpecialHack_enum.No;
        }

        //最大レベルの検索
        static uint g_LevelMaxCaps = NO_CACHE;
        public static uint GetLevelMaxCaps()
        {
            if (g_LevelMaxCaps == NO_CACHE)
            {
                if (PatchUtil.SearchSkillSystem() == PatchUtil.skill_system_enum.SkillSystem)
                {//不明なので31とする
                    g_LevelMaxCaps = 31;
                }
                if (PatchUtil.SearchGrowsMod() == PatchUtil.growth_mod_enum.Vennou)
                {//不明なので31とする
                    g_LevelMaxCaps = 31;
                }
                else
                {
                    g_LevelMaxCaps = Program.ROM.u8(Program.ROM.RomInfo.max_level_address());
                }
            }
            return g_LevelMaxCaps;
        }
        //サウンドルーム全開の判別. ちょっとだけコストがかかる.
        public enum soundroom_unlock_enum
        {
            NO,             //なし
            Enable,
            NoCache = (int)NO_CACHE
        };
        static soundroom_unlock_enum g_Cache_soundroom_unlock_enum = soundroom_unlock_enum.NoCache;
        public static soundroom_unlock_enum SearchSoundRoomUnlock()
        {
            if (g_Cache_soundroom_unlock_enum == soundroom_unlock_enum.NoCache)
            {
                g_Cache_soundroom_unlock_enum = SearchSoundRoomUnlockLow();
            }
            return g_Cache_soundroom_unlock_enum;
        }
        static soundroom_unlock_enum SearchSoundRoomUnlockLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="SoundRoomUnlock",	ver = "FE8U", addr = 0xaede0,data = new byte[]{0xFF,}},
                new PatchTableSt{ name="SoundRoomUnlock",	ver = "FE8J", addr = 0xB3A00,data = new byte[]{0xC0, 0x46,}},
                new PatchTableSt{ name="SoundRoomUnlock",	ver = "FE7U", addr = 0xa6d96,data = new byte[]{0x00, 0x00,}},
                new PatchTableSt{ name="SoundRoomUnlock",	ver = "FE7J", addr = 0xA60EA,data = new byte[]{0x00, 0x00,}},
            };
            if (SearchPatchBool(table))
            {
                return soundroom_unlock_enum.Enable;
            }
            return soundroom_unlock_enum.NO;
        }

        //grows_modの判別. ちょっとだけコストがかかる.
        public enum growth_mod_enum
        {
            NO,             //なし
            Vennou,    //for FE8U
            NoCache = (int)NO_CACHE
        };
        static growth_mod_enum g_Cache_grows_mod_enum = growth_mod_enum.NoCache;
        public static growth_mod_enum SearchGrowsMod()
        {
            if (g_Cache_grows_mod_enum == growth_mod_enum.NoCache)
            {
                g_Cache_grows_mod_enum = SearchGrowsModLow();
            }
            return g_Cache_grows_mod_enum;
        }
        static growth_mod_enum SearchGrowsModLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="GrowsMod",	ver = "FE8U", addr = 0x02BA2A,data = new byte[]{0x4E, 0x46, 0x45, 0x46, 0x60, 0xB4, 0x8B, 0xB0, 0x07, 0x1C, 0xFF, 0xF7, 0xDE, 0xFF, 0x00, 0x06, 0x00, 0x28 ,0x00 ,0xD1 ,0x8E ,0xE0 ,0x78 ,0x7A ,0x63 ,0x28 ,0x00 ,0xD8 ,0x8A ,0xE0 ,0x03 ,0x1C ,0x64 ,0x3B ,0x7B ,0x72 ,0x38 ,0x7A ,0x42 ,0x1C ,0x3A ,0x72 ,0x38 ,0x68 ,0x79 ,0x68 ,0x80 ,0x6A ,0x89 ,0x6A ,0x08 ,0x43 ,0x80 ,0x21 ,0x09 ,0x03 ,0x08 ,0x40 ,0x00 ,0x28 ,0x04 ,0xD0 ,0x10 ,0x06 ,0x00 ,0x16 ,0x0A ,0x28 ,0x0B ,0xD1 ,0x03 ,0xE0 ,0x10 ,0x06 ,0x00,}},
            };
            //FE8U
            if ( SearchPatchBool(table) )
            {
                return growth_mod_enum.Vennou;
            }
            return growth_mod_enum.NO;
        }

        //スキルシステムの判別. ちょっとだけコストがかかる.
        public enum skill_system_enum
        {
            NO,             //なし
            FE8N,           //for FE8J
            FE8N_ver2,      //for FE8J   FE8Nの2018/01 に新しく追加されたもの
            yugudora,       //for FE8J   FE8Nのカスタマイズ
            midori,         //for FE8J   初期から独自スキルを実装していた拡張
            SkillSystem,    //for FE8U
            NoCache = (int)NO_CACHE
        };
        static skill_system_enum g_Cache_skill_system_enum = skill_system_enum.NoCache;
        public static skill_system_enum SearchSkillSystem()
        {
            if (g_Cache_skill_system_enum == skill_system_enum.NoCache)
            {
                g_Cache_skill_system_enum = SearchSkillSystemLow();
            }
            return g_Cache_skill_system_enum;
        }
        static skill_system_enum SearchSkillSystemLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="yugudora",	ver = "FE8J", addr = 0xEE594,data = new byte[]{0x4B ,0xFA ,0x2F ,0x59}},
                new PatchTableSt{ name="FE8N",	ver = "FE8J", addr = 0x89268,data = new byte[]{0x00 ,0x4B ,0x9F ,0x46}},
                new PatchTableSt{ name="midori",	ver = "FE8J", addr = 0xFE58E0,data = new byte[]{0x05 ,0x1C ,0x00 ,0xF0 ,0x25 ,0xF8 ,0x01 ,0x29 ,0x04 ,0xD0 ,0x28 ,0x1C ,0x00 ,0xF0 ,0x28 ,0xF8}},
                new PatchTableSt{ name="SkillSystem",	ver = "FE8U", addr = 0x2ACF8,data = new byte[]{0x70 ,0x47}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
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
                if (t.name == "FE8N")
                {
                    if (SkillConfigFE8NVer2SkillForm.IsFE8NVer2())
                    {
                        return skill_system_enum.FE8N_ver2;
                    }
                    return skill_system_enum.FE8N;
                }
                if (t.name == "yugudora")
                {
                    return skill_system_enum.yugudora;
                }
                if (t.name == "midori")
                {
                    return skill_system_enum.midori;
                }
                if (t.name == "SkillSystem")
                {
                    return skill_system_enum.SkillSystem;
                }
            }
            return skill_system_enum.NO;
        }

        //スキルシステムの判別. ちょっとだけコストがかかる.
        public enum class_type_enum
        {
            NO,             //なし
            SkillSystems_Rework,          //for FE8U
            NoCache = (int)NO_CACHE
        };
        static class_type_enum g_Cache_class_type_enum = class_type_enum.NoCache;
        public static class_type_enum SearchClassType()
        {
            if (g_Cache_class_type_enum == class_type_enum.NoCache)
            {
                g_Cache_class_type_enum = SearchSkillSystemsEffectivenesReworkLow();
            }
            return g_Cache_class_type_enum;
        }
        static class_type_enum SearchSkillSystemsEffectivenesReworkLow()
        {
            if (Program.ROM.RomInfo.version() == 8 && Program.ROM.RomInfo.is_multibyte() == false)
            {
                bool r = Program.ROM.CompareByte(0x2AAEC
                    , new byte[] { 0x00, 0x25, 0x00, 0x28, 0x00, 0xD0, 0x05, 0x1C });
                if (r)
                {
                    return class_type_enum.SkillSystems_Rework;
                }
                r = Program.ROM.CompareByte(0x2AAEC
                    , new byte[] { 0x01, 0x4B, 0xA6, 0xF0, 0xED, 0xFE, 0x01, 0xE0 });
                if (r)
                {
                    return class_type_enum.SkillSystems_Rework;
                }
            }
            return class_type_enum.NO;
        }

        public enum AntiHuffmanEnum
        {
            NO,             //なし
            Enable,
            NoCache = (int)NO_CACHE
        };
        static AntiHuffmanEnum g_Cache_AntiHuffmanEnum = AntiHuffmanEnum.NoCache;
        
        //un-Huffmanの判別.
        public static bool SearchAntiHuffmanPatch()
        {
            if (g_Cache_AntiHuffmanEnum == AntiHuffmanEnum.NoCache)
            {
                if (SearchAntiHuffmanPatch_Low())
                {
                    g_Cache_AntiHuffmanEnum = AntiHuffmanEnum.Enable;
                }
                else
                {
                    g_Cache_AntiHuffmanEnum = AntiHuffmanEnum.NO;
                }
            }

            return g_Cache_AntiHuffmanEnum == AntiHuffmanEnum.Enable;
        }
        static bool SearchAntiHuffmanPatch_Low()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="AntiHuffman",	ver = "FE6", addr = 0x384c,data = new byte[]{0x03, 0xB5, 0x02, 0xB0}},
                new PatchTableSt{ name="AntiHuffman",	ver = "FE7J", addr = 0x13324,data = new byte[]{0x02, 0x49, 0x28 , 0x1C}},
                new PatchTableSt{ name="AntiHuffman",	ver = "FE8J", addr = 0x2af4,data = new byte[]{0x00 ,0xB5 ,0xC2 ,0x0F}},
                new PatchTableSt{ name="AntiHuffman",	ver = "FE7U", addr = 0x12C6C,data = new byte[]{0x02, 0x49, 0x28 , 0x1C}},
                new PatchTableSt{ name="AntiHuffman",	ver = "FE8U", addr = 0x2BA4,data = new byte[]{0x00, 0xB5, 0xC2, 0x0F}},
                new PatchTableSt{ name="AntiHuffman_snake1",	ver = "FE8U", addr = 0x2ba4,data = new byte[]{0x78 ,0x47 ,0xC0 ,0x46}},
            };

            return SearchPatchBool(table);
        }

        //フォントの描画ルーチン
        public enum draw_font_enum
        {
            NO,             //なし
            DrawMultiByte,  //FE7U/FE8Uに日本語を描画するパッチ
            DrawSingleByte, //FE7J/FE8Uに英語を描画するパッチ
            DrawUTF8,       //FE8UにUTF-8を描画するパッチ
            NoCache = (int)NO_CACHE
        };
        //DrawFontPatch(DrawMultiByte/DrawSingleByte)の判別.
        static draw_font_enum g_Cache_draw_font_enum = draw_font_enum.NoCache;
        public static draw_font_enum SearchDrawFontPatch()
        {
            if (g_Cache_draw_font_enum == draw_font_enum.NoCache)
            {
                g_Cache_draw_font_enum = SearchDrawFontPatch(Program.ROM);
            }
            return g_Cache_draw_font_enum;
        }
        public struct PatchTableSt
        {
            public string name;
            public string ver;
            public uint addr;
            public byte[] data;
        };
        public static draw_font_enum SearchDrawFontPatch(ROM rom)
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="DrawSingle",	ver = "FE7J", addr = 0x56e2,data = new byte[]{0x00, 0x00, 0x00, 0x49, 0x8F, 0x46}},
                new PatchTableSt{ name="DrawSingle",	ver = "FE8J", addr = 0x40c2,data = new byte[]{0x00 ,0x00 ,0x00 ,0x49 ,0x8F ,0x46}},
                new PatchTableSt{ name="DrawMulti",	ver = "FE7U", addr = 0x5BD6,data = new byte[]{0x00 ,0x00 ,0x00 ,0x4B ,0x9F ,0x46}},
                new PatchTableSt{ name="DrawMulti",	ver = "FE8U", addr = 0x44D2,data = new byte[]{0x00 ,0x00 ,0x00 ,0x49 ,0x8F ,0x46}},
                new PatchTableSt{ name="DrawUTF8",	ver = "FE7U", addr = 0x5B6A,data = new byte[]{0x00 ,0x00 ,0x00 ,0x4B ,0x18 ,0x47}},
                new PatchTableSt{ name="DrawUTF8",	ver = "FE8U", addr = 0x44D2,data = new byte[]{0x00 ,0x00 ,0x00 ,0x4B ,0x18 ,0x47}},
            };

            string version = rom.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = rom.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "DrawSingle")
                {
                    return draw_font_enum.DrawSingleByte;
                }
                if (t.name == "DrawMulti")
                {
                    return draw_font_enum.DrawMultiByte;
                }
                if (t.name == "DrawUTF8")
                {
                    return draw_font_enum.DrawUTF8;
                }
            }
            return draw_font_enum.NO;
        }

        public enum PRIORITY_CODE
        {
            LAT1,
            SJIS,
            UTF8,
        };
        //文字コードエンコードがパディングしてしまったときの優先変換方法.
        public static PRIORITY_CODE SearchPriorityCode()
        {
            if (Program.ROM.RomInfo.is_multibyte())
            {
                return PRIORITY_CODE.SJIS;
            }

            draw_font_enum dfe = SearchDrawFontPatch();
            if (dfe == draw_font_enum.DrawMultiByte)
            {
                return PRIORITY_CODE.SJIS;
            }
            if (dfe == draw_font_enum.DrawUTF8)
            {
                return PRIORITY_CODE.UTF8;
            }
            return PRIORITY_CODE.LAT1;
        }

        public static PRIORITY_CODE SearchPriorityCode(ROM rom)
        {
            if (rom == null)
            {
                return PRIORITY_CODE.SJIS;
            }

            if (rom.RomInfo.is_multibyte())
            {
                return PRIORITY_CODE.SJIS;
            }
            else
            {
                draw_font_enum dfe = SearchDrawFontPatch(rom);
                if (dfe == draw_font_enum.DrawMultiByte)
                {
                    return PRIORITY_CODE.SJIS;
                }
                if (dfe == draw_font_enum.DrawUTF8)
                {
                    return PRIORITY_CODE.UTF8;
                }
                return PRIORITY_CODE.LAT1;
            }
        }

        //C01Hack(マント)
        public static bool SearchC01HackPatch()
        {
            uint check_value;
            uint address = Program.ROM.RomInfo.patch_C01_hack(out check_value);
            if (address == 0)
            {
                return false;
            }
            uint a = Program.ROM.u32(address);
            return (a == check_value);
        }
        //C48Hack
        public static bool SearchC48HackPatch()
        {
            uint check_value;
            uint address = Program.ROM.RomInfo.patch_C48_hack(out check_value);
            if (address == 0)
            {
                return false;
            }
            //C48だけはNOT条件です
            uint a = Program.ROM.u32(address);
            return (a != check_value);
        }
        //sound16trackパッチの判別.
        public static bool Search16tracks12soundsPatch()
        {
            uint check_value;
            uint address = Program.ROM.RomInfo.patch_16_tracks_12_sounds(out check_value);
            if (address == 0)
            {
                return false;
            }
            uint a = Program.ROM.u32(address);
            return (a == check_value);
        }

        //StairsHack
        public static bool SearchStairsHackPatch()
        {
            uint check_value;
            uint address = Program.ROM.RomInfo.patch_stairs_hack(out check_value);
            if (address == 0)
            {
                return false;
            }
            uint a = Program.ROM.u32(address);
            return (a == check_value);
        }


        //UnitActionRework
        public static bool SearchUnitActionReworkPatch()
        {
            uint check_value;
            uint address = Program.ROM.RomInfo.patch_unitaction_rework_hack(out check_value);
            if (address == 0)
            {
                return false;
            }
            uint a = Program.ROM.u32(address);
            return (a == check_value);
        }

        //ワールドマップスキップパッチが適応されているかどうか判定する
        public enum mnc2_fix_enum
        {
            NO,             //なし
            Aera_Version,   //aeraさんの作ったバージョン
            OldFix,         //古いルーチン
            Stan_20190505,  //Stanが2019/5/5 に提案した方式
            NoCache = (int)NO_CACHE
        };
        static mnc2_fix_enum g_Cache_SkipWorldMap_enum = mnc2_fix_enum.NoCache;
        public static mnc2_fix_enum SearchSkipWorldMapPatch()
        {
            if (g_Cache_SkipWorldMap_enum == mnc2_fix_enum.NoCache)
            {
                g_Cache_SkipWorldMap_enum = SearchSkipWorldMapPatchLow();
            }
            return g_Cache_SkipWorldMap_enum;
        }
        static mnc2_fix_enum SearchSkipWorldMapPatchLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="OldFix",	ver = "FE8J", addr = 0xc1e7c,data = new byte[]{0xB8, 0xE0}},
                new PatchTableSt{ name="Stan_20190505",	ver = "FE8J", addr = 0x0F664,data = new byte[]{0x94, 0xF6, 0x00, 0x08}}, //NOT条件
                new PatchTableSt{ name="Aera_Version" ,	ver = "FE8J", addr = 0xc03e0,data = new byte[]{0x01, 0x48, 0x80, 0x7B, 0x70, 0x47, 0x00, 0x00, 0xEC, 0xBC, 0x02, 0x02}},
                new PatchTableSt{ name="OldFix",	ver = "FE8U", addr = 0xBD070,data = new byte[]{0xB8, 0xE0}},
                new PatchTableSt{ name="Stan_20190505",	ver = "FE8U", addr = 0x0F464,data = new byte[]{0x98, 0xF4, 0x00, 0x08}}, //NOT条件
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    if (t.name == "Stan_20190505")
                    {
                        return mnc2_fix_enum.Stan_20190505;
                    }
                    continue;
                }
                if (t.name == "OldFix")
                {
                    return mnc2_fix_enum.OldFix;
                }
                if (t.name == "Aera_Version")
                {
                    return mnc2_fix_enum.Aera_Version;
                }
            }
            return mnc2_fix_enum.NO;
        }

        public static bool SearchGenericEnemyPortraitExtendsPatch(out uint out_pointer)
        {
            uint check_value;
            uint address = Program.ROM.RomInfo.patch_generic_enemy_portrait_extends(out check_value);
            if (address == 0)
            {
                out_pointer = U.NOT_FOUND;
                return false;
            }
            uint a = Program.ROM.u32(address);
            if (a != check_value)
            {
                out_pointer = U.NOT_FOUND;
                return false;
            }
            out_pointer = address + 20;
            if (!U.isSafetyPointer(Program.ROM.u32(out_pointer)))
            {
                return false;
            }
            return true;
        }

        public static bool SearchNIMAP()
        {
            List<U.AddrResult> iset = PatchUtil.SearchInstrumentSet(100);
            for (int i = 0; i < iset.Count; i++)
            {
                string name = iset[i].name;
                if (name.IndexOf("NIMAP") >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public enum MeleeAndMagicFix_enum
        {
            NO,             //なし
            Enable,
            NoCache = (int)NO_CACHE
        };
        static MeleeAndMagicFix_enum g_Cache_MeleeAndMagicFix_enum = MeleeAndMagicFix_enum.NoCache;

        //武器魔法を同時に利用できるパッチの判別.
        public static bool SearchMeleeAndMagicFixPatch()
        {
            if (g_Cache_MeleeAndMagicFix_enum == MeleeAndMagicFix_enum.NoCache)
            {
                if (SearchMeleeAndMagicFixPatchLow())
                {
                    g_Cache_MeleeAndMagicFix_enum = MeleeAndMagicFix_enum.Enable;
                }
                else
                {
                    g_Cache_MeleeAndMagicFix_enum = MeleeAndMagicFix_enum.NO;
                }
            }
            return g_Cache_MeleeAndMagicFix_enum == MeleeAndMagicFix_enum.Enable;
        }
        static bool SearchMeleeAndMagicFixPatchLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="GIRLS",	ver = "FE8J", addr = 0x18752,data = new byte[]{0x18}},
                new PatchTableSt{ name="FE8NMAGIC",	ver = "FE8J", addr = 0x2a542,data = new byte[]{0x30 ,0x1C}},
                new PatchTableSt{ name="MeleeAndMagicFix",	ver = "FE8U", addr = 0x18A58,data = new byte[]{0x00 ,0xB5 ,0xFE ,0xF7}},
                new PatchTableSt{ name="MeleeAndMagicFix",	ver = "FE8J", addr = 0x1876C,data = new byte[]{0x00 ,0xB5 ,0xFE ,0xF7}},
                new PatchTableSt{ name="MeleeAndMagicFix",	ver = "FE7J", addr = 0x188CC,data = new byte[]{0x00 ,0xB5 ,0xFE ,0xF7}},
                new PatchTableSt{ name="MeleeAndMagicFix",	ver = "FE8U", addr = 0x18A58,data = new byte[]{0x00 ,0xB5 ,0xFE ,0xF7}},
                new PatchTableSt{ name="MeleeAndMagicFix",	ver = "FE7U", addr = 0x184DC,data = new byte[]{0x00 ,0xB5 ,0xFE ,0xF7}},
                new PatchTableSt{ name="MeleeAndMagicFix",	ver = "FE6", addr = 0x18188,data = new byte[]{0x00 ,0xB5 ,0xFE ,0xF7}},
                new PatchTableSt{ name="FE7UMAGIC",	ver = "FE7U", addr = 0x68DE0,data = new byte[]{0x38 ,0x18 ,0x01 ,0x78}}, //魔力分離パッチ FE7U
                new PatchTableSt{ name="FE8UMAGIC",	ver = "FE8U", addr = 0x2BB44,data = new byte[]{0x01 ,0x4B ,0xA5 ,0xF0 ,0xC1 ,0xFE}}, //魔力分離パッチ FE8U
            };
            return SearchPatchBool(table);
        }

        static bool SearchPatchBool(PatchTableSt[] table)
        {
            PatchTableSt p = SearchPatch(table);
            return p.addr != 0;
        }

        static PatchTableSt SearchPatch(PatchTableSt[] table)
        {
            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
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
                return t;
            }
            return new PatchTableSt();
        }

        public struct GrepPatchTableSt
        {
            public string name;
            public string patch_dmp;
        };
        static GrepPatchTableSt GrepPatch(GrepPatchTableSt[] table)
        {
            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (GrepPatchTableSt t in table)
            {
                string fullfilename = Path.Combine(Program.BaseDirectory, "config", "patch2", version, t.patch_dmp);
                if (! File.Exists(fullfilename))
                {
                    continue;
                }
                byte[] data = File.ReadAllBytes(fullfilename);
                uint addr = U.Grep(Program.ROM.Data, data, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
                if (addr == U.NOT_FOUND)
                {
                    continue;
                }
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                return t;
            }
            return new GrepPatchTableSt();
        }

        public struct Grep2PatchTableSt
        {
            public string name;
            public string ver;
            public byte[] data;
        };
        static Grep2PatchTableSt GrepPatch(Grep2PatchTableSt[] table)
        {
            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (Grep2PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }
                uint addr = U.Grep(Program.ROM.Data, t.data, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
                if (addr == U.NOT_FOUND)
                {
                    continue;
                }
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                return t;
            }
            return new Grep2PatchTableSt();
        }

        //カメラを移動する命令で、画面外に飛び出してしまうバグを修正するパッチの検出
        public static bool SearchCAMERA_Event_OutOfBand_FixPatch()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Fix CAM1/CAMERA2 going out of bounds",	ver = "FE8J", addr = 0x15D5E,data = new byte[]{0xE}},
                new PatchTableSt{ name="Fix CAM1/CAMERA2 going out of bounds",	ver = "FE8U", addr = 0x15D52,data = new byte[]{0xE}},
            };
            return SearchPatchBool(table);
        }
        //移動アイコンを0xffまで拡張するパッチの堅守
        public static bool SearchExtendedMovingMapAnimationListPatch()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Extended Moving Map Animation List",	ver = "FE8J", addr = 0x266AA,data = new byte[]{0xFF}},
                new PatchTableSt{ name="Extended Moving Map Animation List",	ver = "FE8U", addr = 0x26706,data = new byte[]{0xFF}},
                new PatchTableSt{ name="Extended Moving Map Animation List",	ver = "FE7J", addr = 0x25202,data = new byte[]{0xFF}},
                new PatchTableSt{ name="Extended Moving Map Animation List",	ver = "FE7U", addr = 0x24D76,data = new byte[]{0xFF}},
            };
            return SearchPatchBool(table);
        }

        public static bool SearchStatusToLocalization_FixPatch()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="StatusToLocalization",	ver = "FE8J", addr = 0x286BC,data = new byte[]{0x00, 0x20}},
            };
            return SearchPatchBool(table);
        }


        //存在ユニットを選択したときフリーズしないように
        public static bool SearchCAMERA_Event_NotExistsUnit_FixPatch()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Prevent Freeze For Camera Event 0x26",	ver = "FE8J", addr = 0xF468,data = new byte[]{0x00, 0x20}},
                new PatchTableSt{ name="Prevent Freeze For Camera Event 0x26",	ver = "FE8U", addr = 0xF25C,data = new byte[]{0x00, 0x20}},
            };
            return SearchPatchBool(table);
        }
        //存在ユニットを選択したときフリーズしないように
        public static bool SearchGetUnitStateEvent_0x33_FixPatch()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Prevent Freeze For Unit State Event 0x33",	ver = "FE8J", addr = 0x103D8,data = new byte[]{0x00, 0x20, 0x02, 0xE0}},
                new PatchTableSt{ name="Prevent Freeze For Unit State Event 0x33",	ver = "FE8U", addr = 0x1027C,data = new byte[]{0x00, 0x20, 0x02, 0xE0}},
            };
            return SearchPatchBool(table);
        }
        //存在ユニットを選択したときフリーズしないように
        public static bool SearchUpdateUnitStateEvent_0x34_FixPatch()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Prevent Freeze For Unit State Event 0x34",	ver = "FE8J", addr = 0x10430,data = new byte[]{0x00, 0x20}},
                new PatchTableSt{ name="Prevent Freeze For Unit State Event 0x34",	ver = "FE8U", addr = 0x102D4,data = new byte[]{0x00, 0x20}},
            };
            return SearchPatchBool(table);
        }
        //存在ユニットを選択したときフリーズしないように
        public static bool SearchWakuEvent_0x3B_FixPatch()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Prevent Freeze For Event 0x3B",	ver = "FE8J", addr = 0x10950,data = new byte[]{0x00, 0x20}},
                new PatchTableSt{ name="Prevent Freeze For Event 0x3B",	ver = "FE8U", addr = 0x10804,data = new byte[]{0x00, 0x20}},
            };
            return SearchPatchBool(table);
        }


        public enum Escape_enum
        {
            NO,             //なし
            EscapeArrivePath,
            EscapeMenuPath,
            NoCache = (int)NO_CACHE
        };
        static Escape_enum g_Cache_Escape_enum = Escape_enum.NoCache;
        public static Escape_enum SearchEscapePatch()
        {
            if (g_Cache_Escape_enum == Escape_enum.NoCache)
            {
                g_Cache_Escape_enum = SearchEscapePatchLow();
            }
            return g_Cache_Escape_enum;
        }
        static Escape_enum SearchEscapePatchLow()
        {
            {
                PatchTableSt[] table = new PatchTableSt[] { 
                    new PatchTableSt{ name="escape_arrive",	ver = "FE8U", addr = 0x187A8,data = new byte[]{0x00, 0x4b, 0x18, 0x47 }},
                };
                PatchTableSt p = SearchPatch(table);
                if (p.name == "escape_arrive")
                {
                    return Escape_enum.EscapeArrivePath;
                }
            }
            {
                Grep2PatchTableSt[] table = new Grep2PatchTableSt[] { 
                    new Grep2PatchTableSt{ name="ver20191022", ver="FE8U", data = new byte[]{0x00,0xB5,0x07,0x48,0x06,0x4A,0x00,0x78,0x02,0x21,0x48,0x43,0x05,0x49,0x40,0x18,0x00,0x88,0x10,0x80,0x02,0xBC,0x08,0x47,0xC0,0x46,0xC0,0x46,0xC0,0x46,0xC0,0x46,0xC0,0x04,0x00,0x03 }},
                };
                Grep2PatchTableSt t = GrepPatch(table);
                if (t.name == "ver20191022")
                {
                    return Escape_enum.EscapeArrivePath;
                }
            }
            {
                GrepPatchTableSt[] table = new GrepPatchTableSt[] { 
                    new GrepPatchTableSt{ name="escape_menu",patch_dmp="EscapeMenu/IsLoca0x13.dmp"},
                };
                GrepPatchTableSt p = GrepPatch(table);
                if (p.name == "escape_menu")
                {
                    return Escape_enum.EscapeMenuPath;
                }
            }
            return Escape_enum.NO;
        }

        //文字コードのルールを無視するフォント群
        public enum IrregularFont_enum
        {
            NO,             //なし
            NarrowFont,
            NoCache,
        };
        static IrregularFont_enum g_Cache_IrregularFont_enum = IrregularFont_enum.NoCache;
        public static IrregularFont_enum SearchIrregularFontPatch()
        {
            if (g_Cache_IrregularFont_enum == IrregularFont_enum.NoCache)
            {
                g_Cache_IrregularFont_enum = SearchIrregularFontPatchLow();
            }
            return g_Cache_IrregularFont_enum;
        }
        static IrregularFont_enum SearchIrregularFontPatchLow()
        {
            {
                GrepPatchTableSt[] table = new GrepPatchTableSt[] { 
                    new GrepPatchTableSt{ name="NarrowFont",patch_dmp="NarrowFont/MenuLowercase/z.img.bin"},
                };
                GrepPatchTableSt p = GrepPatch(table);
                if (p.name == "NarrowFont")
                {
                    return IrregularFont_enum.NarrowFont;
                }
            }
            return IrregularFont_enum.NO;
        }

        public enum Raid_enum
        {
            NO,             //なし
            RaidPath,
            NoCache = (int)NO_CACHE
        };
        static Raid_enum g_Cache_Raid_enum = Raid_enum.NoCache;
        public static Raid_enum SearchRaidPatch()
        {
            if (g_Cache_Raid_enum == Raid_enum.NoCache)
            {
                g_Cache_Raid_enum = SearchRaidPatchLow();
            }
            return g_Cache_Raid_enum;
        }
        static Raid_enum SearchRaidPatchLow()
        {
            {
                Grep2PatchTableSt[] table = new Grep2PatchTableSt[] { 
                    new Grep2PatchTableSt{ name="ver20191022", ver="FE8U", data = new byte[]{0xF0 ,0xB5 ,0x29 ,0x48 ,0x02 ,0x68 ,0x51 ,0x68 ,0x09 ,0x79 ,0x04 ,0x1C ,0x51 ,0x29 ,0x46 ,0xD0 ,0xD0 ,0x68 ,0x40 ,0x21 ,0x08 ,0x40 ,0x00 ,0x28 ,0x41 ,0xD1 ,0x11 ,0x20 ,0x10 ,0x56 ,0x23 ,0x49 ,0x09 ,0x68 ,0x80 ,0x00 ,0x40 ,0x18 ,0x10 ,0x21 }},
                };
                Grep2PatchTableSt t = GrepPatch(table);
                if (t.name == "ver20191022")
                {
                    return Raid_enum.RaidPath;
                }
            }
            return Raid_enum.NO;
        }

        public enum FE8UItemSkill_enum
        {
            NO,             //なし
            FE8USkillSystems,
            NoCache = (int)NO_CACHE
        };
        static FE8UItemSkill_enum g_Cache_FE8UItemSkill_enum = FE8UItemSkill_enum.NoCache;
        public static FE8UItemSkill_enum SearchFE8UItemSkill()
        {
            if (g_Cache_FE8UItemSkill_enum == FE8UItemSkill_enum.NoCache)
            {
                g_Cache_FE8UItemSkill_enum = SearchFE8UItemSkillLow();
            }
            return g_Cache_FE8UItemSkill_enum;
        }
        static FE8UItemSkill_enum SearchFE8UItemSkillLow()
        {
            {
                Grep2PatchTableSt[] table = new Grep2PatchTableSt[] { 
                    new Grep2PatchTableSt{ name="FE8USkillSystems", ver="FE8U", data = new byte[]{0xFF ,0x21 ,0x08 ,0x40 ,0x00 ,0x28 ,0x07 ,0xD0 ,0x24 ,0x21 ,0x48 ,0x43 ,0x05, 0x49 ,0x40 ,0x18 ,0x23 ,0x21 ,0x40 ,0x5C ,0xA0 ,0x42 ,0x01 ,0xD0 ,0x00 ,0x20 ,0x30 ,0xBD ,0x01 ,0x20 ,0x30 ,0xBD }},
                    new Grep2PatchTableSt{ name="FE8USkillSystems", ver="FE8U", data = new byte[]{0x00 ,0xF8 ,0xFF ,0x21, 0x08, 0x40, 0x00, 0x28, 0x10, 0xD0, 0x0F, 0x49, 0x8E, 0x46, 0x00, 0xF8, 0x23, 0x21, 0x40, 0x5C, 0x00, 0x28 ,0x09 ,0xD0 ,0x0C ,0x49, 0x09, 0x78, 0x09 ,0x02, 0x08, 0x43 }},
                    new Grep2PatchTableSt{ name="FE8USkillSystems", ver="FE8U", data = new byte[]{0xB0, 0x6B, 0x02 ,0x02 ,0x90, 0x6B, 0x02, 0x02, 0xB4, 0x6B, 0x02, 0x02, 0x84, 0xE8, 0x03, 0x02, 0x20, 0xAF, 0xB2, 0x08, 0x20, 0xAE ,0xB2, 0x08, 0x75 ,0x0F, 0xB4, 0x08, 0x00, 0xB5, 0x05, 0x4B }},
                };
                Grep2PatchTableSt t = GrepPatch(table);
                if (t.name == "FE8USkillSystems")
                {
                    return FE8UItemSkill_enum.FE8USkillSystems;
                }
            }
            return FE8UItemSkill_enum.NO;
        }

        //SearchAutoNewLine
        public enum AutoNewLine_enum
        {
            NO,             //なし
            AutoNewLine,
            NoCache = (int)NO_CACHE
        };
        static AutoNewLine_enum g_Cache_AutoNewLine_enum = AutoNewLine_enum.NoCache;
        public static AutoNewLine_enum SearchAutoNewLinePatch()
        {
            if (g_Cache_AutoNewLine_enum == AutoNewLine_enum.NoCache)
            {
                g_Cache_AutoNewLine_enum = SearchAutoNewLinePatchLow();
            }
            return g_Cache_AutoNewLine_enum;
        }
        static AutoNewLine_enum SearchAutoNewLinePatchLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="AutoNewLine",	ver = "FE8U", addr = 0x464476,data = new byte[]{0xC0,0x46,0x10,0xB4,0x00,0xB5,0x03,0x4C,0x00,0xF0,0x04,0xF8,0x10,0xBC,0xA6,0x46,0x10,0xBC,0x70,0x47,0x20,0x47}},
            };
            if (SearchPatchBool(table))
            {
                return AutoNewLine_enum.AutoNewLine;
            }
            return AutoNewLine_enum.NO;
        }

        //指南パッチの設定アドレスの場所
        static uint g_Cache_shinan_table = NO_CACHE;
        public static uint SearchShinanTablePatch()
        {
            if (g_Cache_shinan_table == NO_CACHE)
            {
                g_Cache_shinan_table = SearchShinanTablePatchLow();
            }
            return g_Cache_shinan_table;
        }
        static uint SearchShinanTablePatchLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Shinan",	ver = "FE8J", addr = 0xDB000,data = new byte[]{0x00,0xB5,0xC0,0x46,0x06,0x48,0xC0,0x46,0x06,0x49,0x89,0x7B,0x89,0x00,0x40,0x58,0x01,0x21,0x00,0xF0,0x02,0xF8,0x17,0x20,0x00,0xBD,0xC0,0x46,0x02,0x4B,0x9F,0x46}},
                new PatchTableSt{ name="ShinanEA",	ver = "FE8J", addr = 0xDB000,data = new byte[]{0x00,0xB5,0x26,0x20,0x07,0x4B,0x9E,0x46,0x00,0xF8,0x09,0x48,0x06,0x49,0x89,0x7B,0x89,0x00,0x40,0x58,0x01,0x21,0x05,0x4B,0x9E,0x46,0x00,0xF8,0x17,0x20,0x02,0xBC,0x08,0x47,0x00,0x00,0xA8,0x60,0x08,0x08,0xEC,0xBC,0x02,0x02,0x40,0xD3,0x00,0x08}},
                new PatchTableSt{ name="Shinan",	ver = "FE8U", addr = 0xDB000,data = new byte[]{0x00,0xB5,0xC0,0x46,0x06,0x48,0xC0,0x46,0x06,0x49,0x89,0x7B,0x89,0x00,0x40,0x58,0x01,0x21,0x00,0xF0,0x02,0xF8,0x17,0x20,0x00,0xBD,0xC0,0x46,0x02,0x4B,0x9F,0x46}},
                new PatchTableSt{ name="ShinanEA",	ver = "FE8U", addr = 0xDB000,data = new byte[]{0x00,0xB5,0x26,0x20,0x07,0x4B,0x9E,0x46,0x00,0xF8,0x09,0x48,0x06,0x49,0x89,0x7B,0x89,0x00,0x40,0x58,0x01,0x21,0x05,0x4B,0x9E,0x46,0x00,0xF8,0x17,0x20,0x02,0xBC,0x08,0x47,0x00,0x00,0x80,0x3D,0x08,0x08,0xF0,0xBC,0x02,0x02,0x7C,0xD0,0x00,0x08}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                uint addr = U.GrepEnd(Program.ROM.Data, t.data, t.addr, 0, 4, 0, true);
                if (addr == U.NOT_FOUND)
                {
                    continue;
                }
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                addr = Program.ROM.p32(addr);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                return U.toOffset(addr);
            }
            return U.NOT_FOUND;
        }

        //タイトルをテキストから自動生成するパッチがあるか判別.
        public static bool SearchChaptorNamesAsTextFixPatch()
        {
            uint check_value;
            uint address = Program.ROM.RomInfo.patch_chaptor_names_text_fix(out check_value);
            if (address == 0)
            {
                return false;
            }
            uint a = Program.ROM.u32(address);
            return (a == check_value);
        }
        //顔画像拡張システム.
        public enum portrait_extends
        {
            NO,             //なし
            MUG_EXCEED,     //tikiの顔画像拡張
            HALFBODY,       //上半身表示拡張
            NoCache = (int)NO_CACHE
        };
        static portrait_extends g_Cache_portrait_extends = portrait_extends.NoCache;
        public static portrait_extends SearchPortraitExtends()
        {
            if (g_Cache_portrait_extends == portrait_extends.NoCache)
            {
                g_Cache_portrait_extends = SearchPortraitExtendsLow();
            }
            return g_Cache_portrait_extends;
        }
        static portrait_extends SearchPortraitExtendsLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="MUG_EXCEED",	ver = "FE8J", addr = 0x54da,data = new byte[]{0xC0 ,0x46 ,0x01 ,0xB0 ,0x03 ,0x4B}},
                new PatchTableSt{ name="MUG_EXCEED",	ver = "FE8U", addr = 0x55D2,data = new byte[]{0xC0 ,0x46 ,0x01 ,0xB0 ,0x03 ,0x4B}},
                new PatchTableSt{ name="MUG_EXCEED",	ver = "FE7U", addr = 0x6BCA,data = new byte[]{0xC0 ,0x46 ,0x01 ,0xB0 ,0x03 ,0x4B}},
                new PatchTableSt{ name="MUG_EXCEED",	ver = "FE7J", addr = 0x6A5A,data = new byte[]{0xC0 ,0x46 ,0x01 ,0xB0 ,0x03 ,0x4B}},
                new PatchTableSt{ name="HALFBODY",	ver = "FE8U", addr = 0x8540,data = new byte[]{0x0A ,0x1C}},
                new PatchTableSt{ name="HALFBODY",	ver = "FE8J", addr = 0x843C,data = new byte[]{0x0A ,0x1C}},
                new PatchTableSt{ name="HALFBODY",	ver = "FE8U", addr = 0x8540,data = new byte[]{0x01 ,0x3A}},
                new PatchTableSt{ name="HALFBODY",	ver = "FE8J", addr = 0x843C,data = new byte[]{0x01 ,0x3A}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
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

                if (t.name == "MUG_EXCEED")
                {
                    return portrait_extends.MUG_EXCEED;
                }
                if (t.name == "HALFBODY")
                {
                    return portrait_extends.HALFBODY;
                }
            }
            return portrait_extends.NO;
        }


        //音楽ルーチンの改良パッチの有無.
        public enum ImprovedSoundMixer
        {
            NO,             //なし
            ImprovedSoundMixer,
            NoCache = (int)NO_CACHE
        };
        static ImprovedSoundMixer g_Cache_ImprovedSoundMixer = ImprovedSoundMixer.NoCache;
        public static ImprovedSoundMixer SearchImprovedSoundMixer()
        {
            if (g_Cache_ImprovedSoundMixer == ImprovedSoundMixer.NoCache)
            {
                g_Cache_ImprovedSoundMixer = SearchImprovedSoundMixerLow();
            }
            return g_Cache_ImprovedSoundMixer;
        }
        public static ImprovedSoundMixer SearchImprovedSoundMixerLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="ImprovedSoundMixer",	ver = "FE8J", addr = 0xD4234,data = new byte[]{0xb1, 0x6c, 0x00, 0x03, 0x06, 0x00}},
                new PatchTableSt{ name="ImprovedSoundMixer",	ver = "FE8U", addr = 0xd01d0,data = new byte[]{0xb0, 0x6c, 0x00, 0x03, 0x18, 0x02}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "ImprovedSoundMixer")
                {
                    return ImprovedSoundMixer.ImprovedSoundMixer;
                }
            }
            return ImprovedSoundMixer.NO;
        }

        //StatboosterExtends
        public enum StatboosterExtends
        {
            NO,             //なし
            SkillSystems_strmag_slipt,
            NoCache = (int)NO_CACHE
        };
        static StatboosterExtends g_Cache_StatboosterExtends = StatboosterExtends.NoCache;
        public static StatboosterExtends SearchStatboosterExtends()
        {
            if (g_Cache_StatboosterExtends == StatboosterExtends.NoCache)
            {
                g_Cache_StatboosterExtends = SearchStatboosterExtendsLow();
            }
            return g_Cache_StatboosterExtends;
        }
        public static StatboosterExtends SearchStatboosterExtendsLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="SkillSystems_strmag_slipt",	ver = "FE8U", addr = 0x2F8FC,data = new byte[]{0x13, 0x25, 0x05, 0xE0,0x14, 0x25, 0x03, 0xE0}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "SkillSystems_strmag_slipt")
                {
                    return StatboosterExtends.SkillSystems_strmag_slipt;
                }
            }
            return StatboosterExtends.NO;
        }

        //顔画像拡張システム.
        public enum itemicon_extends
        {
            NO,             //なし
            IconExpands,    //FEまで拡張
            SkillSystems,   //SkillSystems
            NoCache = (int)NO_CACHE
        };
        static itemicon_extends g_Cache_itemicon_extends = itemicon_extends.NoCache;
        public static itemicon_extends SearchItemIconExtends()
        {
            if (g_Cache_itemicon_extends == itemicon_extends.NoCache)
            {
                g_Cache_itemicon_extends = SearchItemIconExpandsPatchLow();
            }
            return g_Cache_itemicon_extends;
        }
        public static bool SearchIconExpandsPatch()
        {
            return SearchItemIconExtends() != itemicon_extends.NO;
        }
        public struct PatchItemIconExpandsSt
        {
            public string name;
            public string ver;
            public uint addr;
            public byte[] data;
        };
        public static itemicon_extends SearchItemIconExpandsPatchLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="IconExpands",	ver = "FE8J", addr = 0x34FC,data = new byte[]{0xFE, 0x01, 0x00, 0x01, 0x90, 0x6E, 0x02, 0x02}},
                new PatchTableSt{ name="IconExpands",	ver = "FE8U", addr = 0x35B0,data = new byte[]{0xFE, 0x01, 0x00, 0x01, 0x90, 0x6E, 0x02, 0x02}},
                new PatchTableSt{ name="SkillSystems",	ver = "FE8U", addr = 0x3586,data = new byte[]{0x03, 0x4C, 0x00, 0xF0, 0x03, 0xF8, 0x10, 0xBC, 0x02, 0xBC, 0x08, 0x47, 0x20, 0x47}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "IconExpands")
                {
                    return itemicon_extends.IconExpands;
                }
                if (t.name == "SkillSystems")
                {
                    return itemicon_extends.SkillSystems;
                }
            }
            return itemicon_extends.NO;
        }

        //マップ第2パレット.
        public enum  MapSecondPalette_extends
        {
            NO,             //なし
            Flag0x28_146,
            Flag0x28_45,   
            NoCache = (int)NO_CACHE
        };
        static MapSecondPalette_extends g_Cache_SearchFlag0x28ToMapSecondPalettePatch = MapSecondPalette_extends.NoCache;
        public static MapSecondPalette_extends SearchFlag0x28ToMapSecondPalettePatch()
        {
            if (g_Cache_SearchFlag0x28ToMapSecondPalettePatch == MapSecondPalette_extends.NoCache)
            {
                g_Cache_SearchFlag0x28ToMapSecondPalettePatch = SearchFlag0x28ToMapSecondPalettePatchLow();
            }
            return g_Cache_SearchFlag0x28ToMapSecondPalettePatch;
        }
        static MapSecondPalette_extends SearchFlag0x28ToMapSecondPalettePatchLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="Flag0x28_146",	ver = "FE8J", addr = 0x19628,data = new byte[]{0x00, 0x4A}},
                new PatchTableSt{ name="Flag0x28_45",	ver = "FE8J", addr = 0x19628,data = new byte[]{0x00, 0x49}},
                new PatchTableSt{ name="Flag0x28_146",	ver = "FE8U", addr = 0x19950,data = new byte[]{0x00, 0x4A}},
                new PatchTableSt{ name="Flag0x28_45",	ver = "FE8U", addr = 0x19950,data = new byte[]{0x00, 0x49}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "Flag0x28_146")
                {
                    return MapSecondPalette_extends.Flag0x28_146;
                }
                if (t.name == "Flag0x28_45")
                {
                    return MapSecondPalette_extends.Flag0x28_45;
                }
            }
            return MapSecondPalette_extends.NO;
        }

        //手斧の汎用モーション化
        public enum HandAxsWildCard_extends
        {
            NO,             //なし
            Enable,
            NoCache = (int)NO_CACHE
        };
        static HandAxsWildCard_extends g_Cache_HandAxsWildCard = HandAxsWildCard_extends.NoCache;
        public static HandAxsWildCard_extends SearchCache_HandAxsWildCard()
        {
            if (g_Cache_HandAxsWildCard == HandAxsWildCard_extends.NoCache)
            {
                g_Cache_HandAxsWildCard = SearchCache_HandAxsWildCardLow();
            }
            return g_Cache_HandAxsWildCard;
        }
        static HandAxsWildCard_extends SearchCache_HandAxsWildCardLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="HandAxsWildCard",	ver = "FE8J", addr = 0x596F0,data = new byte[]{0x00, 0x4B, 0x18, 0x47}},
                new PatchTableSt{ name="HandAxsWildCard",	ver = "FE8U", addr = 0x588C0,data = new byte[]{0x00, 0x4B, 0x18, 0x47}},
            };

            bool r = SearchPatchBool(table);
            if (r)
            {
                return HandAxsWildCard_extends.Enable;
            }
            return HandAxsWildCard_extends.NO;
        }

        


        //クリアターン数2倍
        public enum ClearTurn2x_extends
        {
            NO,             //なし
            _2x,
            NoCache = (int)NO_CACHE
        };
        static ClearTurn2x_extends g_Cache_ClearTurn2x = ClearTurn2x_extends.NoCache;
        public static ClearTurn2x_extends SearchCache_ClearTurn2x()
        {
            if (g_Cache_ClearTurn2x == ClearTurn2x_extends.NoCache)
            {
                g_Cache_ClearTurn2x = SearchCache_ClearTurn2xLow();
            }
            return g_Cache_ClearTurn2x;
        }
        static ClearTurn2x_extends SearchCache_ClearTurn2xLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="2x",	ver = "FE8J", addr = 0xa8bac,data = new byte[]{0x02}},
                new PatchTableSt{ name="2x",	ver = "FE8U", addr = 0xA4168,data = new byte[]{0x02}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "2x")
                {
                    return ClearTurn2x_extends._2x;
                }
            }
            return ClearTurn2x_extends.NO;
        }

        //Fourth-Allegiance
        public enum FourthAllegiance_extends
        {
            NO,             //なし
            FourthAllegiance,
            NoCache = (int)NO_CACHE
        };
        static FourthAllegiance_extends g_Cache_FourthAllegiance = FourthAllegiance_extends.NoCache;
        public static FourthAllegiance_extends SearchCache_FourthAllegiance()
        {
            if (g_Cache_FourthAllegiance == FourthAllegiance_extends.NoCache)
            {
                g_Cache_FourthAllegiance = SearchCache_FourthAllegianceLow();
            }
            return g_Cache_FourthAllegiance;
        }
        static FourthAllegiance_extends SearchCache_FourthAllegianceLow()
        {
            PatchTableSt[] table = new PatchTableSt[] { 
                new PatchTableSt{ name="FourthAllegiance",	ver = "FE8U", addr = 0x17BDC,data = new byte[]{0xC0, 0x20, 0xF8, 0xE7}},
            };

            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach (PatchTableSt t in table)
            {
                if (t.ver != version)
                {
                    continue;
                }

                //チェック開始アドレス
                byte[] data = Program.ROM.getBinaryData(t.addr, t.data.Length);
                if (U.memcmp(t.data, data) != 0)
                {
                    continue;
                }
                if (t.name == "FourthAllegiance")
                {
                    return FourthAllegiance_extends.FourthAllegiance;
                }
            }
            return FourthAllegiance_extends.NO;
        }

        public static MoveToUnuseSpace.ADDR_AND_LENGTH get_data_pos_callback(uint addr)
        {
            int length = 0;
            string str = Program.ROM.getString(addr, out length);

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = (uint)length + 1; //nullを入れる.
            return aal;
        }

        //共有しているデータの分割
        public static uint WriteIndependence(uint readAddr, uint blockSize, uint writeSettingAddr, string warnningName, Undo.UndoData undodata)
        {
            DialogResult dr = R.ShowYesNo("{0}のデータを、分離独立させますか？", warnningName);
            if (dr != DialogResult.Yes)
            {
                return U.NOT_FOUND;
            }

            byte[] d = Program.ROM.getBinaryData(readAddr, blockSize);
            uint addr = InputFormRef.AppendBinaryData(d, undodata);
            if (addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }

            Program.ROM.write_p32(writeSettingAddr, addr, undodata);

            return addr;
        }

        public static bool IsSwitch2Enable(uint array_switch2_address)
        {
            //こういうのを探す
            //sub r0, #0x19
            //cmp r0, #0x37
            uint extraByte = 0;
            if (Program.ROM.u16(array_switch2_address + 2) == 0x9A00)
            {//古いコンパイラは、 9A00   ldr r2,[sp, #0x0] を挟むときがあるらしい.
                //sub r0, #0x19
                //ldr r2,[sp, #0x0]
                //cmp r0, #0x37
                extraByte = 2;
            }

            uint op = Program.ROM.u8(array_switch2_address + 1);
            if (op < 0x38 || op > 0x3D)
            {//SUB以外無視
                return false;
            }
            op = Program.ROM.u8(array_switch2_address + 3 + extraByte);
            if (op < 0x28 || op > 0x2d)
            {//CMP以外無視
                return false;
            }
            return true;
        }

        //switch文の拡張
        public static uint Switch2Expands(uint array_pointer
                        , uint array_switch2_address
                        , uint newCount
                        , uint defaultJumpAddr
                        , Undo.UndoData undodata)
        {
            uint pointeraddr = Program.ROM.p32(array_pointer);

            uint extraByte = 0;
            if (Program.ROM.u16(array_switch2_address + 2) == 0x9A00)
            {//古いコンパイラは、 9A00   ldr r2,[sp, #0x0] を挟むときがあるらしい.
                //sub r0, #0x19
                //ldr r2,[sp, #0x0]
                //cmp r0, #0x37
                extraByte = 2;
            }

            //384b     	sub	r0, #4b //ライブ 利用した時の効果
            //2876     	cmp	r0, #76
            uint start = Program.ROM.u8(array_switch2_address + 0);
            uint count = Program.ROM.u8(array_switch2_address + 2 + extraByte) + 1;
            if (newCount <= start + count)
            {
                R.ShowStopError("既に十分な数を確保しています。\r\nあなたの要求:{0} 既存サイズ:{1}+{2}={3}"
                    , U.To0xHexString(newCount), U.To0xHexString(start), U.To0xHexString(count), U.To0xHexString(start + count));
                return U.NOT_FOUND;
            }

            //オペコードの確認
            uint op = Program.ROM.u8(array_switch2_address + 1);
            if (op < 0x38 || op > 0x3D)
            {
                R.ShowStopError("別のパッチでオペコードを書き換えられているので、拡張できません\r\nアドレス:{0} オペコード:{1}"
                    , U.To0xHexString(array_switch2_address + 1), U.To0xHexString(op));
                return U.NOT_FOUND;
            }
            op = Program.ROM.u8(array_switch2_address + 3 + extraByte);
            if (op < 0x28 || op > 0x2d)
            {
                R.ShowStopError("別のパッチでオペコードを書き換えられているので、拡張できません\r\nアドレス:{0} オペコード:{1}"
                    , U.To0xHexString(array_switch2_address + 3), U.To0xHexString(op));
                return U.NOT_FOUND;
            }
            //ユーザに確認を求める.
            DialogResult dr = R.ShowYesNo("配列を {0}まで拡張してもよろしいですか？"
                , U.To0xHexString(newCount));
            if (dr != DialogResult.Yes)
            {
                return U.NOT_FOUND;
            }

            byte[] dd = Program.ROM.getBinaryData(pointeraddr, count * 4);
            byte[] d = new byte[(newCount + 1) * 4];
            for (uint i = 0; i < start; i++)
            {
                U.write_p32(d, i * 4, defaultJumpAddr);
            }
            Array.Copy(dd, 0, d, start * 4, count * 4);
            for (uint i = start + count; i < newCount; i++)
            {
                U.write_p32(d, i * 4, defaultJumpAddr);
            }

            uint newaddr = InputFormRef.AppendBinaryData(d, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }

            Program.ROM.write_p32(array_pointer, newaddr, undodata);
            Program.ROM.write_u8(array_switch2_address + 0, 0, undodata);
            Program.ROM.write_u8(array_switch2_address + 2 + extraByte, newCount - 1, undodata);

            return newaddr;
        }
        public static bool IsSwitch1Enable(uint array_switch1_address)
        {
            uint op = Program.ROM.u8(array_switch1_address + 1);
            if (op < 0x28 || op > 0x2d)
            {//CMP
                return false;
            }
            return true;
        }

        //switch文の拡張
        public static uint Switch1Expands(uint array_pointer
                        , uint array_switch1_address
                        , uint newCount
                        , uint defaultJumpAddr
                        , Undo.UndoData undodata)
        {
            uint pointeraddr = Program.ROM.p32(array_pointer);
            //0802fbc4 2876     	cmp	r0, #76
            uint start = 0;
            uint count = Program.ROM.u8(array_switch1_address + 0);
            if (newCount <= count)
            {
                R.ShowStopError("既に十分な数を確保しています。\r\nあなたの要求:{0} 既存サイズ:{1}+{2}={3}"
                    , U.To0xHexString(newCount), U.To0xHexString(start), U.To0xHexString(count), U.To0xHexString(start + count));
                return U.NOT_FOUND;
            }

            //オペコードの確認
            uint op = Program.ROM.u8(array_switch1_address + 1);
            if (op < 0x28 || op > 0x2d)
            {
                R.ShowStopError("別のパッチでオペコードを書き換えられているので、拡張できません\r\nアドレス:{0} オペコード:{1}"
                    , U.To0xHexString(array_switch1_address + 1), U.To0xHexString(op));
                return U.NOT_FOUND;
            }
            //ユーザに確認を求める.
            DialogResult dr = R.ShowYesNo("配列を {0}まで拡張してもよろしいですか？"
                , U.To0xHexString(newCount));
            if (dr != DialogResult.Yes)
            {
                return U.NOT_FOUND;
            }

            byte[] dd = Program.ROM.getBinaryData(pointeraddr, count * 4);
            byte[] d = new byte[(newCount + 1) * 4];
            for (uint i = 0; i < start; i++)
            {
                U.write_p32(d, i * 4, defaultJumpAddr);
            }
            Array.Copy(dd, 0, d, start * 4, count * 4);
            for (uint i = start + count; i < newCount; i++)
            {
                U.write_p32(d, i * 4, defaultJumpAddr);
            }

            uint newaddr = InputFormRef.AppendBinaryData(d, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }

            undodata.list.Add(new Undo.UndoPostion(array_pointer, 4));
            undodata.list.Add(new Undo.UndoPostion(array_switch1_address + 0, 1));
            undodata.list.Add(new Undo.UndoPostion(array_switch1_address + 2, 1));

            Program.ROM.write_p32(array_pointer, newaddr);
            Program.ROM.write_u8(array_switch1_address + 0, newCount - 1);

            return newaddr;
        }

        public static uint GetEndWeaponDebuffTable3(uint start_offset, string value)
        {
            uint end = start_offset + (3 * 256);
            end = Math.Min(end, (uint)Program.ROM.Data.Length);
            start_offset += 3; //先頭は0x00 0x00 0x00 なので読み飛ばす.

            uint found = end;
            byte[] need = new byte[] { 0x00, 0x00, 0x00 };
            uint new_found = U.Grep(Program.ROM.Data, need, start_offset, end);
            found = Math.Min(found, new_found);

            need = new byte[] { 0xFF, 0xFF, 0x00 };
            new_found = U.Grep(Program.ROM.Data, need, start_offset, end);
            found = Math.Min(found, new_found);

            need = new byte[] { 0xFF, 0xFF, 0xFF };
            new_found = U.Grep(Program.ROM.Data, need, start_offset, end);
            found = Math.Min(found, new_found);

            return found;
        }
        public static uint GetEndWeaponDebuffTable4(uint start_offset, string value)
        {
            uint end = start_offset + (4 * 256);
            end = Math.Min(end, (uint)Program.ROM.Data.Length);
            start_offset += 4; //先頭は0x00 0x00 0x00 0x00 なので読み飛ばす.

            uint found = end;
            byte[] need = new byte[] { 0x00, 0x00, 0x00 };
            uint new_found = U.Grep(Program.ROM.Data, need, start_offset, end);
            found = Math.Min(found, new_found);

            need = new byte[] { 0xFF, 0xFF, 0x00 };
            new_found = U.Grep(Program.ROM.Data, need, start_offset, end);
            found = Math.Min(found, new_found);

            need = new byte[] { 0xFF, 0xFF, 0xFF };
            new_found = U.Grep(Program.ROM.Data, need, start_offset, end);
            found = Math.Min(found, new_found);

            return found;
        }

        static List<U.AddrResult> g_InstrumentSet = null;

        public static List<U.AddrResult> SearchInstrumentSet(uint currentData)
        {
            if (g_InstrumentSet == null)
            {
                g_InstrumentSet = SearchInstrumentSetLow(U.ConfigDataFilename("song_instrumentset_"), currentData);
            }
            return g_InstrumentSet;
        }

        static List<U.AddrResult> SearchInstrumentSetLow(string filename, uint currentData)
        {
            List<U.AddrResult> iset = new List<U.AddrResult>();
            iset.Add(new U.AddrResult(currentData, U.ToHexString(U.toPointer(currentData)) + "=Current"));

            bool hasNimap2 = false;

            string[] lines = File.ReadAllLines(filename);
            string version = Program.ROM.RomInfo.VersionToFilename();
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
                    if (sp[1] != "ALL")
                    {
                        continue;
                    }
                }
                string[] hexStrings = sp[2].Split(' ');
                byte[] need = new byte[hexStrings.Length];
                for (int n = 0; n < hexStrings.Length; n++)
                {
                    need[n] = (byte)U.atoh(hexStrings[n]);
                }

                //Grepして調べる 結構重い.
                uint v = U.Grep(Program.ROM.Data, need, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
                if (v == U.NOT_FOUND)
                {
                    continue;
                }

                if (sp[0] == "AllInstrument")
                {//All Instrumentは、マルチトラックしかないので、データのポインタでサンプルを取ります。
                    v = U.GrepPointer(Program.ROM.Data, v, Program.ROM.RomInfo.compress_image_borderline_address());
                    if (v == U.NOT_FOUND)
                    {
                        continue;
                    }
                    v -= 8;
                }
                else if (sp[0] == "NatveInstrumentMap2")
                {
                    hasNimap2 = true;
                }
                else if (sp[0] == "NatveInstrumentMap")
                {
                    if (hasNimap2)
                    {
                        continue;
                    }
                }

                v = U.toPointer(v);
                iset.Add(new U.AddrResult(v, U.ToHexString(v) + "=" + sp[0]));
            }
            return iset;
        }

        //VennouWeaponLockArray
        public static uint g_WeaponLockArrayTableAddr = U.NOT_FOUND;
        public static bool SearchVennouWeaponLockArray()
        {
            if (g_WeaponLockArrayTableAddr == U.NOT_FOUND)
            {
                g_WeaponLockArrayTableAddr = SearchVennouWeaponLockArrayAddrLow();
            }
            return U.isSafetyOffset(g_WeaponLockArrayTableAddr);
        }
        public static uint SearchVennouWeaponLockArrayAddr()
        {
            if (g_WeaponLockArrayTableAddr == U.NOT_FOUND)
            {
                g_WeaponLockArrayTableAddr = SearchVennouWeaponLockArrayAddrLow();
            }
            return g_WeaponLockArrayTableAddr;
        }
        static uint SearchVennouWeaponLockArrayAddrLow()
        {
            //FE8U Only
            if (Program.ROM.RomInfo.version() != 8)
            {
                return 0;
            }
            if (Program.ROM.RomInfo.is_multibyte() != false)
            {
                return 0;
            }
            //パッチがインストールされているか?
            uint data = Program.ROM.u32(0x16DD8);
            if (data != 0xFF3D3C00)
            {
                return 0;
            }

            //本体を探す.
            string program_dmp = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE8U", "WeaponLockArray_SkillSystems", "AdvWeaponLocks.dmp");
            if (!File.Exists(program_dmp))
            {
                return 0;
            }
            byte[] program_dmp_bin = File.ReadAllBytes(program_dmp);
            uint addr = U.GrepEnd(Program.ROM.Data, program_dmp_bin, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
            if (addr == U.NOT_FOUND)
            {
                return 0;
            }
            addr = addr + 4;
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }
            addr = Program.ROM.p32(addr);
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }
            return addr;
        }
        public static bool IsWriteBuildVersion()
        {
            uint enable_value;
            uint addr = Program.ROM.RomInfo.patch_write_build_version(out enable_value);
            if (addr == 0)
            {
                return false;
            }
            return (Program.ROM.u32(addr) == enable_value);
        }

        public static bool IsPreparationBGMByChapter()
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                return false;
            }

            if (Program.ROM.RomInfo.is_multibyte() == true)
            {
                uint addr = 0x340cc;
                uint enable_value = 0xFBFCF064;
                uint r = Program.ROM.u32(addr);
                if (r == enable_value)
                {
                    return true;
                }
            }
            else
            {
                uint addr = 0x341C4;
                uint enable_value = 0xFA0EF062;
                uint r = Program.ROM.u32(addr);
                if (r == enable_value)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
