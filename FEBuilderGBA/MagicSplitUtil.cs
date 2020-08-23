using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FEBuilderGBA
{
    public class MagicSplitUtil
    {
        //魔法分離パッチの判別.
        public enum magic_split_enum
        {
             NO             //なし
           , FE8NMAGIC      //for FE8J   FE8N 魔力分離
           , FE7UMAGIC      //for FE7U   魔力分離
           , FE8UMAGIC      //for FE8U   魔力分離
           , NoCache = 0xFF
        };
        static magic_split_enum g_Cache_magic_split_enum = magic_split_enum.NoCache;
        static uint UnitTag;
        static uint ClassTag;
        public static magic_split_enum SearchMagicSplit()
        {
            if (g_Cache_magic_split_enum == magic_split_enum.NoCache)
            {
                g_Cache_magic_split_enum = SearchMagicSplitLow();

                UnitTag = U.NOT_FOUND;
                ClassTag = U.NOT_FOUND;
                if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC)
                {
                    FE7UInit();
                }
                else if (g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
                {
                    FE8UInit();
                }
            }
            return g_Cache_magic_split_enum;
        }
        public static void ClearCache()
        {
            g_Cache_magic_split_enum = magic_split_enum.NoCache;
        }

        static magic_split_enum SearchMagicSplitLow()
        {
            PatchUtil.PatchTableSt[] table = new PatchUtil.PatchTableSt[] { 
                new PatchUtil.PatchTableSt{ name="FE8NMAGIC",	ver = "FE8J", addr = 0x2a542,data = new byte[]{0x30 ,0x1C}},
                new PatchUtil.PatchTableSt{ name="FE7UMAGIC",	ver = "FE7U", addr = 0x68DE0,data = new byte[]{0x38 ,0x18 ,0x01 ,0x78}},
                new PatchUtil.PatchTableSt{ name="FE8UMAGIC",	ver = "FE8U", addr = 0x2BB44,data = new byte[]{0x01 ,0x4B ,0xA5 ,0xF0 ,0xC1 ,0xFE}},
            };
#if DEBUG
            if (Program.ROM == null)
            {
                return magic_split_enum.NO;
            }
#endif
            string version = Program.ROM.RomInfo.VersionToFilename();
            foreach(PatchUtil.PatchTableSt t in table)
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
                if (t.name == "FE8NMAGIC")
                {
                    return magic_split_enum.FE8NMAGIC;
                }
                if (t.name == "FE7UMAGIC")
                {
                    return magic_split_enum.FE7UMAGIC;
                }
                if (t.name == "FE8UMAGIC")
                {
                    return magic_split_enum.FE8UMAGIC;
                }
            }
            return magic_split_enum.NO;
        }
        
        static void FE7UInit()
        {
            string filename ;
            uint p;
            filename = Path.Combine(Program.BaseDirectory,"config", "patch2", "FE7U", "FE7-Str Mag Split", "Autolevelling and Saves", "Char Mag Autolevel.dmp");
            p = U.Grep4EndByDmp(filename, 0x100, 0);
            if (U.isSafetyOffset(p))
            {
                UnitTag = Program.ROM.p32(p);
            }

            filename = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE7U", "FE7-Str Mag Split", "Autolevelling and Saves", "Class Mag Autolevel.dmp");
            p = U.Grep4EndByDmp(filename, 0x100, 0);
            if (U.isSafetyOffset(p))
            {
                ClassTag = Program.ROM.p32(p);
            }
        }
        static void FE8UInit()
        {
            uint p;
            byte[] bin = { 0x00, 0xB5, 0x13, 0x48, 0x86, 0x46, 0x38, 0x68, 0x00, 0x79, 0x40, 0x00, 0x12, 0x49, 0x40, 0x18, 0x40, 0x78, 0x50, 0x44, 0x00, 0xF8, 0x01, 0xB4, 0x0E, 0x48, 0x86, 0x46, 0xF8, 0x7A, 0x00, 0xF8, 0x41, 0x68, 0x3A, 0x30, 0x00, 0x78, 0x09, 0x79, 0x89, 0x00, 0x0C, 0x4A, 0x52, 0x18, 0x92, 0x78, 0x02, 0xBC, 0x76, 0x18, 0x43, 0x18, 0x93, 0x42, 0x00, 0xDD, 0x11, 0x1A, 0x38, 0x1C, 0x7A, 0x30, 0x01, 0x70, 0x01, 0xBC, 0x00, 0x99, 0x03, 0x91, 0x01, 0x9B, 0x02, 0x93, 0xC2, 0x46, 0x00, 0x47, 0xA0, 0xB9, 0x02, 0x08, 0x30, 0x94, 0x01, 0x08 };
            p = U.GrepEnd(Program.ROM.Data, bin, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4, 0, true);
            if (U.isSafetyOffset(p))
            {
                UnitTag = Program.ROM.p32(p);
                ClassTag = Program.ROM.p32(p + 4);
            }
        }

        static void WriteUnitAs(uint uid, uint skip,uint newValue,Undo.UndoData undodata)
        {
            if (!U.isSafetyOffset(UnitTag))
            {
                return ;
            }
            uint a = UnitTag + (uid * 2) + skip;
            if (!U.isSafetyOffset(a))
            {
                return ;
            }
            Program.ROM.write_u8(a, newValue , undodata);
        }
        static void WriteClassAs(uint cid, uint skip, uint newValue, Undo.UndoData undodata)
        {
            if (!U.isSafetyOffset(ClassTag))
            {
                return ;
            }
            uint a = ClassTag + (cid * 4) + skip;
            if (!U.isSafetyOffset(a))
            {
                return ;
            }
            Program.ROM.write_u8(a, newValue, undodata);
        }

        static uint GetUnitAs(uint uid, uint skip)
        {
            if (!U.isSafetyOffset(UnitTag))
            {
                return 0;
            }
            uint a = UnitTag + (uid * 2) + skip;
            if (!U.isSafetyOffset(a))
            {
                return 0;
            }
            return Program.ROM.u8(a);
        }
        static uint GetClassAs(uint cid, uint skip)
        {
            if (!U.isSafetyOffset(ClassTag))
            {
                return 0;
            }
            uint a = ClassTag + (cid * 4) + skip;
            if (!U.isSafetyOffset(a))
            {
                return 0;
            }
            return Program.ROM.u8(a);
        }

        static public uint GetUnitBaseMagicExtends(uint uid, uint addr)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                return GetUnitAs(uid, 0);
            }
            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
            {
                return Program.ROM.u8(addr + 50);
            }
            return 0;
        }
        static public uint GetUnitGrowMagicExtends(uint uid, uint addr)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                return GetUnitAs(uid, 1);
            }
            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
            {
                return Program.ROM.u8(addr + 51);
            }
            return 0;
        }



        static public uint GetClassBaseMagicExtends(uint cid, uint addr)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                return GetClassAs(cid, 0);
            }
            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
            {
                return Program.ROM.u8(addr + 80);
            }
            return 0;
        }

        static public uint GetClassGrowMagicExtends(uint cid, uint addr)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                return GetClassAs(cid, 1);
            }
            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
            {
                return Program.ROM.u8(addr + 81);
            }
            return 0;
        }
        static public uint GetClassLimitMagicExtends(uint cid, uint addr)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                return GetClassAs(cid, 2);
            }
            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
            {//体力を使いまわす
                return Program.ROM.u8(addr + 25);
            }
            return 0;
        }
        static public uint GetClassPromotionGainMagicExtends(uint cid, uint addr)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                return GetClassAs(cid, 3);
            }
            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
            {
                return Program.ROM.u8(addr + 82);
            }
            return 0;
        }


        static public void WriteUnitBaseMagicExtends(uint uid, uint addr, uint newValue, Undo.UndoData undodata)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                WriteUnitAs(uid, 0, newValue, undodata);
                return;
            }

//            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
//            {//Unit構造体の未使用領域を利用するため不要
//            }
        }
        static public void WriteUnitGrowMagicExtends(uint uid, uint addr, uint newValue, Undo.UndoData undodata)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                WriteUnitAs(uid, 1 , newValue , undodata);
                return;
            }
//            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
//            {//Unit構造体の未使用領域を利用するため不要
//            }
        }

        static public void WriteClassBaseMagicExtends(uint cid, uint addr, uint newValue, Undo.UndoData undodata)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                WriteClassAs(cid, 0 , newValue , undodata);
                return;
            }
//            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
//            {//Class構造体の未使用領域を利用するため不要
//            }
        }

        static public void WriteClassGrowMagicExtends(uint cid, uint addr, uint newValue, Undo.UndoData undodata)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                WriteClassAs(cid, 1, newValue, undodata);
                return;
            }
//            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
//            {//Class構造体の未使用領域を利用するため不要
//            }
        }
        static public void WriteClassLimitMagicExtends(uint cid, uint addr, uint newValue, Undo.UndoData undodata)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                WriteClassAs(cid, 2, newValue, undodata);
                return;
            }
//            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
//            {//体力を使いまわす
//                return Program.ROM.u8(addr + 25);
//            }
        }
        static public void WriteClassPromotionGainMagicExtends(uint cid, uint addr, uint newValue, Undo.UndoData undodata)
        {
            if (g_Cache_magic_split_enum == magic_split_enum.FE7UMAGIC
                || g_Cache_magic_split_enum == magic_split_enum.FE8UMAGIC)
            {
                WriteClassAs(cid, 3, newValue, undodata);
                return;
            }
//            else if (g_Cache_magic_split_enum == magic_split_enum.FE8NMAGIC)
//            {//Class構造体の未使用領域を利用するため不要
//            }
        }

    }
}
