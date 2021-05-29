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
    public static class EmulatorMemoryUtil
    {
        public static uint GetWorldmapNode(uint next_chaper)
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 8);

            uint nodeid = WorldMapPointForm.GetNodeIDByChapter(next_chaper);
            if (nodeid != U.NOT_FOUND)
            {
                return nodeid;
            }

            //不明な場合は、現在の拠点IDを返す.
            return GetCurrentWorldmapNode();
        }
        public static uint GetCurrentWorldmapNode()
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 8);

            //不明な場合は、現在の拠点IDを返す.
            uint gSomeWMEventRelatedStruct;
            if (Program.ROM.RomInfo.is_multibyte())
            {//FE8J
                gSomeWMEventRelatedStruct = 0x03005270;
            }
            else
            {//FE8U
                gSomeWMEventRelatedStruct = 0x03005280;
            }
            uint nodeid = Program.RAM.u8(gSomeWMEventRelatedStruct + 0x11);
            return nodeid;
        }
        //編を求める
        public static uint GetEdition()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//6には編が存在しない.
                return U.NOT_FOUND;
            }
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            uint ramPointer = stageStructAddr + 0x1B;
            return Program.RAM.u8(ramPointer);
        }
        public static string ConvertEditionToString(uint v)
        {
            if (v == U.NOT_FOUND)
            {
                return "";
            }
            string ret = U.ToHexString(v);
            string edition = InputFormRef.GetEditon(v);
            if (edition == "")
            {
                edition = "???";
            }
            return ret + "=" + edition;
        }
        //難易度を求める.
        public static uint GetDiffecly()
        {
            uint ret = 0;
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();

            uint ramPointer1 = stageStructAddr + 0x14;
            uint v = Program.RAM.u8(ramPointer1);
            if ((v & 0x40) == 0x40)
            {
                ret = ret | 0x40;//難しい
            }

            uint ramPointer2 = stageStructAddr + 0x42;
            v = Program.RAM.u8(ramPointer2);
            if ((v & 0x20) == 0x20)
            {
                ret = ret | 0x20;//初めてではない
            }

            return ret;
        }
        public static string ConvertDiffeclyToString(uint v)
        {
            string ret = "";
            if ((v & 0x40) == 0x40)
            {
                ret += R._("難易度:難しい");
                ret += " ";
            }
            if ((v & 0x20) != 0x20)
            {
                ret += R._("難易度:初めて");
                ret += " ";
            }
            if (ret == "")
            {
                ret += R._("難易度:普通");
                ret += " ";
            }
            return ret;
        }
        public static string ConvertWorldmapNodeToString(uint v)
        {
            if (v == U.NOT_FOUND)
            {
                return "";
            }
            string ret = U.ToHexString(v);
            string edition = WorldMapPointForm.GetWorldMapPointName(v);
            if (edition == "")
            {
                edition = "???";
            }
            return R._("拠点") + ": " + ret + "=" + edition;
        }
        static bool IsWarpChapterFE8(uint chapterID)
        {
            PatchUtil.mnc2_fix_enum use_mnc2 = PatchUtil.SearchSkipWorldMapPatch();
            if (use_mnc2 == PatchUtil.mnc2_fix_enum.NO)
            {
                if (MapLoadFunctionForm.IsEnterChapterAlways(chapterID))
                {
                    return true;
                }
            }

            return true;
        }

        public static void CHEAT_WARP_FE8(EmulatorMemoryForm form, uint warp_chapter, uint edtion, uint worldmap_node)
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 8);

            uint work_address = Program.ROM.RomInfo.workmemory_last_string_address() - 0x70; //テキストバッファの一番下をデータ置き場として利用する.
            uint gSomeWMEventRelatedStruct;
            uint eventExecuteFucntion;
            uint endAllMenusFunction;
            uint deletePlayerPhaseInterface6CsFunction;

            if (Program.ROM.RomInfo.is_multibyte())
            {//FE8J
                gSomeWMEventRelatedStruct = 0x03005270;
                eventExecuteFucntion = 0x0800D340;
                endAllMenusFunction = 0x0804FCAC;
                deletePlayerPhaseInterface6CsFunction = 0x0808F44C;
            }
            else
            {//FE8U
                gSomeWMEventRelatedStruct = 0x03005280;
                eventExecuteFucntion = 0x0800D07C;
                endAllMenusFunction = 0x0804ef20;
                deletePlayerPhaseInterface6CsFunction = 0x0808d150;
            }

            //Search MAPTASK Procs
            uint maptask = SearchMapTaskProcsAddr();
            if (maptask == U.NOT_FOUND)
            {
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした。\r\n章に入っていますか？\r\nこの機能を使うには章の中に入らないといけません。");
                return;
            }

            byte[] warpCode = { 
            //ASM
            0x00, 0xB5, 0x12, 0x4A, 0x42, 0x60, 0x12, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x11, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x78, 0x46, 0x0E, 0x30, 0x01, 0x21, 0x0F, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x01, 0xBC, 0x00, 0x47, 0xC0, 0x46, 
            //Event +24
            0x22, 0x2A, 0xFF, 0xFF,                        //MNC2
            0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00,//NoFade+Term
            0x00, 0x00, 0x00, 0x00,                        //padding
            //hook procs +34
            0x02, 0x00, 0x00, 0x00, 0x61, 0xB6, 0x02, 0x02, 0x0E, 0x00, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x30, 0x5E, 0x5C, 0x08, //backCode
            0xAC, 0xFC, 0x04, 0x08, 
            0x4C, 0xF4, 0x08, 0x08, 
            0x40, 0xD3, 0x00, 0x08  //eventExecuteFucntion
            };

            PatchUtil.mnc2_fix_enum use_mnc2 = PatchUtil.SearchSkipWorldMapPatch();
            if (use_mnc2 != PatchUtil.mnc2_fix_enum.NO
                || MapLoadFunctionForm.IsEnterChapterAlways(warp_chapter))
            {//MNC2でワープ可能
                //章ID
                U.write_u16(warpCode, 0x26, warp_chapter);
            }
            else
            {//MNCHが必要
                uint worldmap_node_minus1 = 0;
                if (worldmap_node > 0)
                {
                    worldmap_node_minus1 = worldmap_node - 1;
                }

                byte[] mnch_code = {
                    0x40, 0xA6, 0x00, 0x00,
                    0x00, 0x00, (byte)worldmap_node_minus1, 0x00,
                    0x21, 0x2A, (byte)warp_chapter, 0x00,
                    0x20, 0x01, 0x00, 0x00,
                }; 
                U.write_range(warpCode,0x24,mnch_code);
            }

            //Procsで実行を指定するASMコードの位置
            U.write_u32(warpCode, 0x38, work_address + 1);

            //メニューがあれば閉じる命令
            U.write_u32(warpCode, 0x50, endAllMenusFunction);
            //TI PIなどのプレイヤーUIがあれば閉じる命令
            U.write_u32(warpCode, 0x54, deletePlayerPhaseInterface6CsFunction);

            //イベント命令を実行する命令
            U.write_u32(warpCode, 0x58, eventExecuteFucntion);

            //復帰するProcsのコード
            uint backCode = Program.RAM.u32(maptask + 4);
            U.write_u32(warpCode, 0x4C, backCode);
            Program.RAM.write_range(work_address, warpCode);

            uint procs_jump_addr = work_address + 0x34;
            Program.RAM.write_u32(maptask + 4, procs_jump_addr);

            //Edition
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            Program.RAM.write_u8(stageStructAddr + 0x1b, edtion);

            //拠点
            Program.RAM.write_u8(gSomeWMEventRelatedStruct + 0x11, worldmap_node);

            InputFormRef.ShowWriteNotifyAnimation(form, procs_jump_addr);
            return;
        }

        public static void CHEAT_WARP_FE7(EmulatorMemoryForm form, uint warp_chapter, uint edtion)
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 7);

            uint work_address = Program.ROM.RomInfo.workmemory_last_string_address() - 0x70; //テキストバッファの一番下をデータ置き場として利用する.
            uint eventExecuteFucntion;
            uint endAllMenusFunction;
            uint deletePlayerPhaseInterface6CsFunction;

            if (Program.ROM.RomInfo.is_multibyte())
            {//FE7J
                eventExecuteFucntion = 0x0800AEB0;
                endAllMenusFunction = 0x0804AC78;
                deletePlayerPhaseInterface6CsFunction = 0x0808667C;
            }
            else
            {//FE7U
                eventExecuteFucntion = 0x0800af74;
                endAllMenusFunction = 0x0804A490;
                deletePlayerPhaseInterface6CsFunction = 0x08085C7C;
            }

            //Search MAPTASK Procs
            uint maptask = SearchMapTaskProcsAddr();
            if (maptask == U.NOT_FOUND)
            {
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした。\r\n章に入っていますか？\r\nこの機能を使うには章の中に入らないといけません。");
                return;
            }

            byte[] warpCode = { 
            //ASM
            0x00, 0xB5, 0x12, 0x4A, 0x42, 0x60, 0x12, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x11, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x78, 0x46, 0x0E, 0x30, 0x01, 0x21, 0x0F, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x01, 0xBC, 0x00, 0x47, 0xC0, 0x46, 
            //Event +24
            0x7F, 0x00, 0xFF, 0xFF,	                       //MNCH
            0x01, 0x00, 0x00, 0x00,                        //_1
            0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//ENDA
            //hook procs +34
            0x02, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x02, 0x02, 0x0E, 0x00, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x08, //backCode
            0x00, 0x00, 0x00, 0x08, 
            0x00, 0x00, 0x00, 0x08, 
            0x00, 0x00, 0x00, 0x08  //eventExecuteFucntion
            };
            if (Program.ROM.RomInfo.is_multibyte() == false)
            {//FE7Uだと、MNCHは、0x7f 0x00 ではなく、 0x81 0x00.
                U.write_u8(warpCode, 0x24, 0x81);
            }
            //章ID
            U.write_u16(warpCode, 0x26, warp_chapter);
            //Procsで実行を指定するASMコードの位置
            U.write_u32(warpCode, 0x38, work_address + 1);

            //メニューがあれば閉じる命令
            U.write_u32(warpCode, 0x50, endAllMenusFunction);
            //TI PIなどのプレイヤーUIがあれば閉じる命令
            U.write_u32(warpCode, 0x54, deletePlayerPhaseInterface6CsFunction);

            //イベント命令を実行する命令
            U.write_u32(warpCode, 0x58, eventExecuteFucntion);

            //復帰するProcsのコード
            uint backCode = Program.RAM.u32(maptask + 4);
            U.write_u32(warpCode, 0x4C, backCode);
            Program.RAM.write_range(work_address, warpCode);

            uint procs_jump_addr = work_address + 0x34;
            Program.RAM.write_u32(maptask + 4, procs_jump_addr);

            //Edition
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            Program.RAM.write_u8(stageStructAddr + 0x1b, edtion);

            InputFormRef.ShowWriteNotifyAnimation(form, procs_jump_addr);
        }

        public static void CHEAT_WARP_FE6(EmulatorMemoryForm form, uint warp_chapter)
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 6);

            uint work_address = Program.ROM.RomInfo.workmemory_last_string_address() - 0x70; //テキストバッファの一番下をデータ置き場として利用する.
            uint eventExecuteFucntion;
            uint endAllMenusFunction;
            uint deletePlayerPhaseInterface6CsFunction;

            eventExecuteFucntion = 0x0800d9b8;
            endAllMenusFunction = 0x08041A38;
            deletePlayerPhaseInterface6CsFunction = 0x08073324;

            //Search MAPTASK Procs
            uint maptask = SearchMapTaskProcsAddr();
            if (maptask == U.NOT_FOUND)
            {
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした。\r\n章に入っていますか？\r\nこの機能を使うには章の中に入らないといけません。");
                return;
            }


            byte[] warpCode = { 
            //ASM
            0x00, 0xB5, 0x12, 0x4A, 0x42, 0x60, 0x12, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x11, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x78, 0x46, 0x0E, 0x30, 0x01, 0x21, 0x0F, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x01, 0xBC, 0x00, 0x47, 0xC0, 0x46, 
            
            //Event +24
            0x3D, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00,//MNCH
            0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//END
            //hook procs +34
            0x02, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x02, 0x02, 0x0E, 0x00, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x08, //backCode
            0x00, 0x00, 0x00, 0x08, 
            0x00, 0x00, 0x00, 0x08, 
            0x00, 0x00, 0x00, 0x08  //eventExecuteFucntion
            };
            //章ID
            U.write_u16(warpCode, 0x28, warp_chapter);
            //Procsで実行を指定するASMコードの位置
            U.write_u32(warpCode, 0x38, work_address + 1);

            //メニューがあれば閉じる命令
            U.write_u32(warpCode, 0x50, endAllMenusFunction);
            //TI PIなどのプレイヤーUIがあれば閉じる命令
            U.write_u32(warpCode, 0x54, deletePlayerPhaseInterface6CsFunction);

            //イベント命令を実行する命令
            U.write_u32(warpCode, 0x58, eventExecuteFucntion);

            //復帰するProcsのコード
            uint backCode = Program.RAM.u32(maptask + 4);
            U.write_u32(warpCode, 0x4C, backCode);
            Program.RAM.write_range(work_address, warpCode);

            uint procs_jump_addr = work_address + 0x34;
            Program.RAM.write_u32(maptask + 4, procs_jump_addr);

            InputFormRef.ShowWriteNotifyAnimation(form, procs_jump_addr);
        }

        static uint SearchMapTaskProcsAddr()
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {//FE8J
                    return SearchProcsAddr(0x085c5de8);
                }
                else
                {//FE8U
                    return SearchProcsAddr(0x0859d908);
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {//FE7J
                    return SearchProcsAddr(0x08C05464);
                }
                else
                {//FE7U
                    return SearchProcsAddr(0x08B961A8);
                }
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                return SearchProcsAddr(0x085C7BE4);
            }
            else
            {
                return U.NOT_FOUND;
            }
        }

        static uint SearchProcsAddr(uint search_procs_code)
        {
            byte[] need = new byte[] { 0xFF, 0xFF, 0xFF, 0x08, 0x00, 0x00, 0x00, 0x08 };
            bool[] mask = new bool[] { false, false, false, false, true, true, true, false };

            U.write_u32(need, 0, search_procs_code);

            uint maptask = Program.RAM.GrepPatternMatch0x02(need, mask, 4);
            return maptask;
        }


        public static void CHEAT_CALLENDEvent(EmulatorMemoryForm form)
        {
            uint work_address = Program.ROM.RomInfo.workmemory_last_string_address() - 0x70; //テキストバッファの一番下をデータ置き場として利用する.
            uint endAllMenusFunction;
            uint deletePlayerPhaseInterface6CsFunction;
            uint setFlagFunction;
            uint callEndEventFunction;

            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {//FE8J
                    endAllMenusFunction = 0x0804FCAC;
                    deletePlayerPhaseInterface6CsFunction = 0x0808F44C;
                    setFlagFunction = 0x080860A8;
                    callEndEventFunction = 0x080855B8;
                }
                else
                {//FE8U
                    endAllMenusFunction = 0x0804ef20;
                    deletePlayerPhaseInterface6CsFunction = 0x0808d150;
                    setFlagFunction = 0x08083D80;
                    callEndEventFunction = 0x08083280;
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {//FE7J
                    endAllMenusFunction = 0x0804AC78;
                    deletePlayerPhaseInterface6CsFunction = 0x0808667C;
                    setFlagFunction = 0x0807A0B4;
                    callEndEventFunction = 0x0807a208;
                }
                else
                {//FE7U
                    endAllMenusFunction = 0x0804A490;
                    deletePlayerPhaseInterface6CsFunction = 0x08085C7C;
                    setFlagFunction = 0x080798E4;
                    callEndEventFunction = 0x08079A38;
                }
            }
            else
            {//FE6
                endAllMenusFunction = 0x08041A38;
                deletePlayerPhaseInterface6CsFunction = 0x08073324;
                setFlagFunction = 0x0806BA48;
                callEndEventFunction = 0x0806B5B0;
            }


            //Search MAPTASK Procs
            uint maptask = SearchMapTaskProcsAddr();
            if (maptask == U.NOT_FOUND)
            {
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした。\r\n章に入っていますか？\r\nこの機能を使うには章の中に入らないといけません。");
                return;
            }

            byte[] warpCode = { 
            //ASM
            0x00, 0xB5, 0x08, 0x49, 0x41, 0x60, 0x08, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x07, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x03, 0x20, 0x06, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x06, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x01, 0xBC, 0x00, 0x47,
            0x11, 0x11, 0x11, 0x11, 
            0x22, 0x22, 0x22, 0x22, 
            0x33, 0x33, 0x33, 0x33, 
            0x44, 0x44, 0x44, 0x44, 
            0x55, 0x55, 0x55, 0x55,

            //hook procs +38
            0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x02, 0x0E, 0x00, 0x78, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x30, 0x5E, 0x5C, 0x08, //backCode
            0xAC, 0xFC, 0x04, 0x08, 
            0x4C, 0xF4, 0x08, 0x08, 
            0x40, 0xD3, 0x00, 0x08  //eventExecuteFucntion
            };
            //Procsで実行を指定するASMコードの位置
            U.write_u32(warpCode, 0x3C, work_address + 1);

            //メニューがあれば閉じる命令
            U.write_u32(warpCode, 0x28, endAllMenusFunction);
            //TI PIなどのプレイヤーUIがあれば閉じる命令
            U.write_u32(warpCode, 0x2C, deletePlayerPhaseInterface6CsFunction);
            //フラグを立てる命令
            U.write_u32(warpCode, 0x30, setFlagFunction);
            //イベント命令を実行する命令
            U.write_u32(warpCode, 0x34, callEndEventFunction);

            //復帰するProcsのコード
            uint backCode = Program.RAM.u32(maptask + 4);
            U.write_u32(warpCode, 0x24, backCode);
            Program.RAM.write_range(work_address, warpCode);

            uint procs_jump_addr = work_address + 0x38;
            Program.RAM.write_u32(maptask + 4, procs_jump_addr);

            InputFormRef.ShowWriteNotifyAnimation(form, procs_jump_addr);
            return;
        }
        public static string GetRAMUnitAIDToName(uint aid)
        {
            if (aid == 0)
            {
                return "";
            }

            uint addr = Program.ROM.RomInfo.RAMSlotTable_address();
            uint t = ( (aid) & 0xff) * 4;
            addr += t;

            uint ramUnitAddr = Program.ROM.u32(addr);
            if (ramUnitAddr == 0)
            {
                return "";
            }

            uint romUnitAddr = Program.RAM.u32(ramUnitAddr);
            romUnitAddr = U.toOffset(romUnitAddr);
            if (!U.isSafetyOffset(romUnitAddr))
            {
                return "";
            }
            uint textid = Program.ROM.u16(romUnitAddr); //Unit.Name
            uint unitid = Program.ROM.u8(romUnitAddr + 4); //Unit.ID
            return U.ToHexString(unitid) + " " + FETextDecode.Direct(textid);
        }
        public static string GetAIDUnitString(uint addr)
        {
            uint aid;
            if (Program.ROM.RomInfo.version() == 6)
            {
                aid = Program.RAM.u16(addr + 0x19);
            }
            else
            {
                aid = Program.RAM.u16(addr + 0x1B);
            }
            string aidString = EmulatorMemoryUtil.GetRAMUnitAIDToName(aid);
            return aidString;
        }
        public static string GetRAMUnitStatus(uint addr)
        {
            uint state;
            if (Program.ROM.RomInfo.version() == 6)
            {//状態1,2だけ
                state = Program.RAM.u16(addr + 0x0C);
            }
            else
            {//状態1,2,3,4
                state = Program.RAM.u32(addr + 0x0C);
            }
            string stateString = InputFormRef.GetRAM_UNIT_STATE(state);
            return stateString;
        }

        public class AddressList
        {
            public string Name;
            public string Type;
            public uint   Size;
            public uint   Plus;
            public AddressList(uint plus,string name,string type,uint size)
            {
                this.Name = name;
                this.Type = type;
                this.Size = size;
                this.Plus = plus;
            }
        }

        static public List<EmulatorMemoryUtil.AddressList> GetChapterDataStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                ret.Add(new AddressList(0x00, "Clock", "", 4));
                ret.Add(new AddressList(0x04, "Unknown4", "", 4));
                ret.Add(new AddressList(0x08, "Gold", "DEC", 4));
                ret.Add(new AddressList(0x0C, "SaveSlotIndex", "", 1));
                ret.Add(new AddressList(0x0D, "Fog", "FOG", 1));
                ret.Add(new AddressList(0x0E, "MapID", "MAP", 1));
                ret.Add(new AddressList(0x0F, "Phase", "PHASE", 1));
                ret.Add(new AddressList(0x10, "Turns", "DEC", 2));
                ret.Add(new AddressList(0x12, "CorsolX", "DEC", 1));
                ret.Add(new AddressList(0x13, "CorsolY", "DEC", 1));
                ret.Add(new AddressList(0x14, "ChapterStuff", "CHAPTERSTUFF", 1));
                ret.Add(new AddressList(0x15, "Weather", "WEATHER", 1));
                ret.Add(new AddressList(0x16, "Support Gain Total", "", 2));
                ret.Add(new AddressList(0x18, "PlaythroughId", "", 2));
                ret.Add(new AddressList(0x1A, "LastUnitListSortType", "", 1));
                ret.Add(new AddressList(0x1B, "Editon", "EDITON", 1));
                ret.Add(new AddressList(0x1C, "Config", "CHAPTERCONFIG", 0x4));
            }
            else
            {//FE7 , FE8
                ret.Add(new AddressList(0x00, "Clock", "", 4));
                ret.Add(new AddressList(0x04, "Unknown4", "", 4));
                ret.Add(new AddressList(0x08, "Gold", "DEC", 4));
                ret.Add(new AddressList(0x0C, "SaveSlotIndex", "", 1));
                ret.Add(new AddressList(0x0D, "Fog", "FOG", 1));
                ret.Add(new AddressList(0x0E, "MapID", "MAP", 1));
                ret.Add(new AddressList(0x0F, "Phase", "PHASE", 1));
                ret.Add(new AddressList(0x10, "Turns", "DEC", 2));
                ret.Add(new AddressList(0x12, "CorsolX", "DEC", 1));
                ret.Add(new AddressList(0x13, "CorsolY", "DEC", 1));
                ret.Add(new AddressList(0x14, "ChapterStuff", "CHAPTERSTUFF", 1));
                ret.Add(new AddressList(0x15, "Weather", "WEATHER", 1));
                ret.Add(new AddressList(0x16, "SupportGainTotal", "", 2));
                ret.Add(new AddressList(0x18, "PlaythroughId", "", 2));
                ret.Add(new AddressList(0x1A, "LastUnitListSortType", "", 1));
                ret.Add(new AddressList(0x1B, "Editon", "EDITON", 1));
                ret.Add(new AddressList(0x1C, "WeaponTypeLookup", "", 4));
                ret.Add(new AddressList(0x20, "TacticianName", "STRING", 0xB));
                ret.Add(new AddressList(0x2C, "Unknown2C", "", 0x4));
                ret.Add(new AddressList(0x30, "FundsTotalDifference", "", 0x4));
                ret.Add(new AddressList(0x34, "Unknown34", "", 0x4));
                ret.Add(new AddressList(0x38, "Padding_38", "", 0x2));
                ret.Add(new AddressList(0x40, "Config", "CHAPTERCONFIG", 0x4));
                ret.Add(new AddressList(0x44, "Unknown44", "", 0x4));
                ret.Add(new AddressList(0x48, "Unknown48", "", 0x2));
                ret.Add(new AddressList(0x4A, "Unknown4A", "", 0x2));
            }

            return ret;
        }
        static public List<EmulatorMemoryUtil.AddressList> GetBattleUnitStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                ret.Add(new AddressList(0x00, "UnitPointer", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x04, "ClassPointer", "ROMCLASSPOINTER", 4));
                ret.Add(new AddressList(0x08, "Level", "DEC", 1));
                ret.Add(new AddressList(0x09, "EXP", "DEC", 1));
                ret.Add(new AddressList(0x0A, "Recovery mode", "", 1));
                ret.Add(new AddressList(0x0B, "UnitTableID", "", 1));
                ret.Add(new AddressList(0x0C, "State", "RAMUNITSTATE", 2));
                ret.Add(new AddressList(0x0E, "X", "DEC", 1));
                ret.Add(new AddressList(0x0F, "Y", "DEC", 1));
                ret.Add(new AddressList(0x10, "MAX HP", "DEC", 1));
                ret.Add(new AddressList(0x11, "Current HP", "DEC", 1));
                ret.Add(new AddressList(0x12, "Str", "DEC", 1));
                ret.Add(new AddressList(0x13, "Skill", "DEC", 1));
                ret.Add(new AddressList(0x14, "Spd", "DEC", 1));
                ret.Add(new AddressList(0x15, "Def", "DEC", 1));
                ret.Add(new AddressList(0x16, "Ref", "DEC", 1));
                ret.Add(new AddressList(0x17, "Luck", "DEC", 1));
                ret.Add(new AddressList(0x18, "CON", "DEC", 1));
                ret.Add(new AddressList(0x19, "TrvID", "RAMUNITAID", 1));
                ret.Add(new AddressList(0x1A, "Unknown1C", "", 1));
                ret.Add(new AddressList(0x1B, "MOV", "", 1));
                ret.Add(new AddressList(0x1C, "ItemID1", "ITEM", 1));
                ret.Add(new AddressList(0x1D, "ItemStock1", "DEC", 1));
                ret.Add(new AddressList(0x1E, "ItemID2", "ITEM", 1));
                ret.Add(new AddressList(0x1F, "ItemStock2", "DEC", 1));
                ret.Add(new AddressList(0x20, "ItemID3", "ITEM", 1));
                ret.Add(new AddressList(0x21, "ItemStock3", "DEC", 1));
                ret.Add(new AddressList(0x22, "ItemID4", "ITEM", 1));
                ret.Add(new AddressList(0x23, "ItemStock4", "DEC", 1));
                ret.Add(new AddressList(0x24, "ItemID5", "ITEM", 1));
                ret.Add(new AddressList(0x25, "ItemStock5", "DEC", 1));
                ret.Add(new AddressList(0x26, "Sword EXP", "", 1));
                ret.Add(new AddressList(0x27, "Lance EXP", "", 1));
                ret.Add(new AddressList(0x28, "Axs EXP", "", 1));
                ret.Add(new AddressList(0x29, "Bow EXP", "", 1));
                ret.Add(new AddressList(0x2A, "Staff EXP", "", 1));
                ret.Add(new AddressList(0x2B, "Rule EXP", "", 1));
                ret.Add(new AddressList(0x2C, "Light EXP", "", 1));
                ret.Add(new AddressList(0x2D, "Dark EXP", "", 1));
                ret.Add(new AddressList(0x1E, "State and turns", "", 0x1));
                ret.Add(new AddressList(0x1F, "PureWaterTourch", "", 0x1));
                ret.Add(new AddressList(0x30, "Support1", "", 0x1));
                ret.Add(new AddressList(0x31, "Support2", "", 0x1));
                ret.Add(new AddressList(0x32, "Support3", "", 0x1));
                ret.Add(new AddressList(0x33, "Support4", "", 0x1));
                ret.Add(new AddressList(0x34, "Support5", "", 0x1));
                ret.Add(new AddressList(0x35, "Support6", "", 0x1));
                ret.Add(new AddressList(0x36, "Support7", "", 0x1));
                ret.Add(new AddressList(0x37, "Support8", "", 0x1));
                ret.Add(new AddressList(0x38, "Support9", "", 0x1));
                ret.Add(new AddressList(0x39, "Support10", "", 0x1));
                ret.Add(new AddressList(0x3A, "SupportFlag", "", 0x1));
                ret.Add(new AddressList(0x3B, "Unknown3B", "", 0x1));
                ret.Add(new AddressList(0x3C, "Unknown3C", "", 0x1));
                ret.Add(new AddressList(0x3D, "Unknown3D", "", 0x1));
                ret.Add(new AddressList(0x3E, "Unknown3E", "", 0x1));
                ret.Add(new AddressList(0x3F, "Unknown3F", "", 0x1));
                ret.Add(new AddressList(0x40, "AI3", "", 0x1));
                ret.Add(new AddressList(0x41, "AI4", "", 0x1));
                ret.Add(new AddressList(0x42, "AI1", "AI1", 0x1));
                ret.Add(new AddressList(0x43, "AI1 Counter", "", 0x1));
                ret.Add(new AddressList(0x44, "AI2", "AI2", 0x1));
                ret.Add(new AddressList(0x45, "AI2 Counter", "", 0x1));
                ret.Add(new AddressList(0x46, "Unknown46", "", 0x1));
                ret.Add(new AddressList(0x47, "Unknown47", "", 0x1));
                ret.Add(new AddressList(0x48, "weapon ItemID", "ITEM", 0x1));
                ret.Add(new AddressList(0x49, "weapon Stock", "DEC", 0x1));
                ret.Add(new AddressList(0x4A, "weaponBefore ItemID", "ITEM", 0x1));
                ret.Add(new AddressList(0x4B, "weaponBefore Stock", "DEC", 0x1));
                ret.Add(new AddressList(0x4C, "weaponAttributes", "", 0x4));
                ret.Add(new AddressList(0x50, "weaponType", "WEAPONTYPE", 0x1));
                ret.Add(new AddressList(0x51, "weaponSlotIndex", "", 0x1));
                ret.Add(new AddressList(0x52, "canCounter", "", 0x1));
                ret.Add(new AddressList(0x53, "wTriangleHitBonus", "DEC", 0x1));
                ret.Add(new AddressList(0x54, "wTriangleDmgBonus", "DEC", 0x1));
                ret.Add(new AddressList(0x55, "terrainId", "", 0x1));
                ret.Add(new AddressList(0x56, "terrainDefense", "DEC", 0x1));
                ret.Add(new AddressList(0x57, "terrainAvoid", "DEC", 0x1));
                ret.Add(new AddressList(0x58, "terrainResistance", "DEC", 0x1));
                ret.Add(new AddressList(0x59, "pad59", "", 0x1));
                ret.Add(new AddressList(0x5A, "battleAttack", "DEC", 0x2));
                ret.Add(new AddressList(0x5C, "battleDefense", "DEC", 0x2));
                ret.Add(new AddressList(0x5E, "battleSpeed", "DEC", 0x2));
                ret.Add(new AddressList(0x60, "battleHitRate", "DEC", 0x2));
                ret.Add(new AddressList(0x62, "battleAvoidRate", "DEC", 0x2));
                ret.Add(new AddressList(0x64, "battleEffectiveHitRate", "DEC", 0x2));
                ret.Add(new AddressList(0x66, "battleCritRate", "DEC", 0x2));
                ret.Add(new AddressList(0x68, "battleDodgeRate", "DEC", 0x2));
                ret.Add(new AddressList(0x6A, "battleEffectiveCritRate", "DEC", 0x2));
                ret.Add(new AddressList(0x6C, "battleSilencerRate", "DEC", 0x2));
                ret.Add(new AddressList(0x6E, "expGain", "DEC", 0x1));
                ret.Add(new AddressList(0x6F, "statusOut", "", 0x1));
                ret.Add(new AddressList(0x70, "levelPrevious", "DEC", 0x1));
                ret.Add(new AddressList(0x71, "expPrevious", "DEC", 0x1));
                ret.Add(new AddressList(0x72, "hpInitial", "DEC", 0x1));
                ret.Add(new AddressList(0x73, "changeHP", "DEC", 0x1));
                ret.Add(new AddressList(0x74, "changePow", "DEC", 0x1));
                ret.Add(new AddressList(0x75, "changeSkl", "DEC", 0x1));
                ret.Add(new AddressList(0x76, "changeSpd", "DEC", 0x1));
                ret.Add(new AddressList(0x77, "changeDef", "DEC", 0x1));
                ret.Add(new AddressList(0x78, "changeRes", "DEC", 0x1));
                ret.Add(new AddressList(0x79, "changeLck", "DEC", 0x1));
                ret.Add(new AddressList(0x7A, "changeCon", "DEC", 0x1));
                ret.Add(new AddressList(0x7B, "wexpMultiplier", "DEC", 0x1));
                ret.Add(new AddressList(0x7C, "nonZeroDamage", "", 0x1));
                ret.Add(new AddressList(0x7D, "weaponBroke", "", 0x1));
                ret.Add(new AddressList(0x7E, "hasItemEffectTarget", "", 0x1));
                ret.Add(new AddressList(0x7F, "pad7f", "", 0x1));
            }
            else
            {//FE7 , FE8
                ret.Add(new AddressList(0x00, "UnitPointer", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x04, "ClassPointer", "ROMCLASSPOINTER", 4));
                ret.Add(new AddressList(0x08, "Level", "DEC", 1));
                ret.Add(new AddressList(0x09, "EXP", "DEC", 1));
                ret.Add(new AddressList(0x0A, "Recovery mode", "", 1));
                ret.Add(new AddressList(0x0B, "UnitTableID", "", 1));
                ret.Add(new AddressList(0x0C, "State", "RAMUNITSTATE", 4));
                ret.Add(new AddressList(0x10, "X", "DEC", 1));
                ret.Add(new AddressList(0x11, "Y", "DEC", 1));
                ret.Add(new AddressList(0x12, "MAX HP", "DEC", 1));
                ret.Add(new AddressList(0x13, "Current HP", "DEC", 1));
                ret.Add(new AddressList(0x14, "Str", "DEC", 1));
                ret.Add(new AddressList(0x15, "Skill", "DEC", 1));
                ret.Add(new AddressList(0x16, "Spd", "DEC", 1));
                ret.Add(new AddressList(0x17, "Def", "DEC", 1));
                ret.Add(new AddressList(0x18, "Ref", "DEC", 1));
                ret.Add(new AddressList(0x19, "Luck", "DEC", 1));
                ret.Add(new AddressList(0x1A, "CON", "DEC", 1));
                ret.Add(new AddressList(0x1B, "TrvID", "RAMUNITAID", 1));
                ret.Add(new AddressList(0x1C, "Unknown1C", "", 1));
                ret.Add(new AddressList(0x1D, "MOV", "", 1));
                ret.Add(new AddressList(0x1E, "ItemID1", "ITEM", 1));
                ret.Add(new AddressList(0x1F, "ItemStock1", "DEC", 1));
                ret.Add(new AddressList(0x20, "ItemID2", "ITEM", 1));
                ret.Add(new AddressList(0x21, "ItemStock2", "DEC", 1));
                ret.Add(new AddressList(0x22, "ItemID3", "ITEM", 1));
                ret.Add(new AddressList(0x23, "ItemStock3", "DEC", 1));
                ret.Add(new AddressList(0x24, "ItemID4", "ITEM", 1));
                ret.Add(new AddressList(0x25, "ItemStock4", "DEC", 1));
                ret.Add(new AddressList(0x26, "ItemID5", "ITEM", 1));
                ret.Add(new AddressList(0x27, "ItemStock5", "DEC", 1));
                ret.Add(new AddressList(0x28, "Sword EXP", "", 1));
                ret.Add(new AddressList(0x29, "Lance EXP", "", 1));
                ret.Add(new AddressList(0x2A, "Axs EXP", "", 1));
                ret.Add(new AddressList(0x2B, "Bow EXP", "", 1));
                ret.Add(new AddressList(0x2C, "Staff EXP", "", 1));
                ret.Add(new AddressList(0x2D, "Rule EXP", "", 1));
                ret.Add(new AddressList(0x2E, "Light EXP", "", 1));
                ret.Add(new AddressList(0x2F, "Dark EXP", "", 1));
                ret.Add(new AddressList(0x30, "State and turns", "", 0x1));
                ret.Add(new AddressList(0x31, "PureWaterTourch", "", 0x1));
                ret.Add(new AddressList(0x32, "Support1", "", 0x1));
                ret.Add(new AddressList(0x34, "Support2", "", 0x1));
                ret.Add(new AddressList(0x34, "Support3", "", 0x1));
                ret.Add(new AddressList(0x35, "Support4", "", 0x1));
                ret.Add(new AddressList(0x36, "Support5", "", 0x1));
                ret.Add(new AddressList(0x37, "Support6", "", 0x1));
                ret.Add(new AddressList(0x38, "Support7", "", 0x1));
                ret.Add(new AddressList(0x39, "SupportFlag", "", 0x1));
                ret.Add(new AddressList(0x3A, "Unknown3A", "", 0x1));
                ret.Add(new AddressList(0x3B, "Unknown3B", "", 0x1));
                ret.Add(new AddressList(0x3C, "Unknown3C", "", 0x1));
                ret.Add(new AddressList(0x3D, "Unknown3D", "", 0x1));
                ret.Add(new AddressList(0x3E, "Unknown3E", "", 0x1));
                ret.Add(new AddressList(0x3F, "Unknown3F", "", 0x1));
                ret.Add(new AddressList(0x40, "AI3", "", 0x1));
                ret.Add(new AddressList(0x41, "AI4", "", 0x1));
                ret.Add(new AddressList(0x42, "AI1", "AI1", 0x1));
                ret.Add(new AddressList(0x43, "AI1 Counter", "", 0x1));
                ret.Add(new AddressList(0x44, "AI2", "AI2", 0x1));
                ret.Add(new AddressList(0x45, "AI2 Counter", "", 0x1));
                ret.Add(new AddressList(0x46, "Unknown46", "", 0x1));
                ret.Add(new AddressList(0x47, "Unknown47", "", 0x1));
                ret.Add(new AddressList(0x48, "weapon ItemID", "ITEM", 0x1));
                ret.Add(new AddressList(0x49, "weapon Stock", "DEC", 0x1));
                ret.Add(new AddressList(0x4A, "weaponBefore ItemID", "ITEM", 0x1));
                ret.Add(new AddressList(0x4B, "weaponBefore Stock", "DEC", 0x1));
                ret.Add(new AddressList(0x4C, "weaponAttributes", "", 0x4));
                ret.Add(new AddressList(0x50, "weaponType", "WEAPONTYPE", 0x1));
                ret.Add(new AddressList(0x51, "weaponSlotIndex", "", 0x1));
                ret.Add(new AddressList(0x52, "canCounter", "", 0x1));
                ret.Add(new AddressList(0x53, "wTriangleHitBonus", "DEC", 0x1));
                ret.Add(new AddressList(0x54, "wTriangleDmgBonus", "DEC", 0x1));
                ret.Add(new AddressList(0x55, "terrainId", "TILE", 0x1));
                ret.Add(new AddressList(0x56, "terrainDefense", "DEC", 0x1));
                ret.Add(new AddressList(0x57, "terrainAvoid", "DEC", 0x1));
                ret.Add(new AddressList(0x58, "terrainResistance", "DEC", 0x1));
                ret.Add(new AddressList(0x59, "pad59", "", 0x1));
                ret.Add(new AddressList(0x5A, "battleAttack", "DEC", 0x2));
                ret.Add(new AddressList(0x5C, "battleDefense", "DEC", 0x2));
                ret.Add(new AddressList(0x5E, "battleSpeed", "DEC", 0x2));
                ret.Add(new AddressList(0x60, "battleHitRate", "DEC", 0x2));
                ret.Add(new AddressList(0x62, "battleAvoidRate", "DEC", 0x2));
                ret.Add(new AddressList(0x64, "battleEffectiveHitRate", "DEC", 0x2));
                ret.Add(new AddressList(0x66, "battleCritRate", "DEC", 0x2));
                ret.Add(new AddressList(0x68, "battleDodgeRate", "DEC", 0x2));
                ret.Add(new AddressList(0x6A, "battleEffectiveCritRate", "DEC", 0x2));
                ret.Add(new AddressList(0x6C, "battleSilencerRate", "DEC", 0x2));
                ret.Add(new AddressList(0x6E, "expGain", "DEC", 0x1));
                ret.Add(new AddressList(0x6F, "statusOut", "", 0x1));
                ret.Add(new AddressList(0x70, "levelPrevious", "DEC", 0x1));
                ret.Add(new AddressList(0x71, "expPrevious", "DEC", 0x1));
                ret.Add(new AddressList(0x72, "hpInitial", "DEC", 0x1));
                ret.Add(new AddressList(0x73, "changeHP", "DEC", 0x1));
                ret.Add(new AddressList(0x74, "changePow", "DEC", 0x1));
                ret.Add(new AddressList(0x75, "changeSkl", "DEC", 0x1));
                ret.Add(new AddressList(0x76, "changeSpd", "DEC", 0x1));
                ret.Add(new AddressList(0x77, "changeDef", "DEC", 0x1));
                ret.Add(new AddressList(0x78, "changeRes", "DEC", 0x1));
                ret.Add(new AddressList(0x79, "changeLck", "DEC", 0x1));
                ret.Add(new AddressList(0x7A, "changeCon", "DEC", 0x1));
                ret.Add(new AddressList(0x7B, "wexpMultiplier", "DEC", 0x1));
                ret.Add(new AddressList(0x7C, "nonZeroDamage", "", 0x1));
                ret.Add(new AddressList(0x7D, "weaponBroke", "", 0x1));
                ret.Add(new AddressList(0x7E, "hasItemEffectTarget", "", 0x1));
                ret.Add(new AddressList(0x7F, "pad7f", "", 0x1));
            }
            return ret;
        }
        static public List<EmulatorMemoryUtil.AddressList> GetArenaDataStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                ret.Add(new AddressList(0x00, "playerUnit", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x04, "opponentUnit", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x08, "betMoney", "DEC", 2));
                ret.Add(new AddressList(0x0A, "battleResults", "", 1));
                ret.Add(new AddressList(0x0B, "unk0B", "", 1));
                ret.Add(new AddressList(0x0C, "range", "", 1));
                ret.Add(new AddressList(0x0D, "playerWpnType", "WEAPONTYPE", 1));
                ret.Add(new AddressList(0x0E, "opponentWpnType", "WEAPONTYPE", 1));
                ret.Add(new AddressList(0x0F, "playerClassId", "CLASS", 1));
                ret.Add(new AddressList(0x10, "opponentClassId", "CLASS", 1));
                ret.Add(new AddressList(0x11, "playerLevel", "DEC", 1));
                ret.Add(new AddressList(0x12, "oppenentLevel", "DEC", 1));
                ret.Add(new AddressList(0x13, "playerIsMagic", "", 1));
                ret.Add(new AddressList(0x14, "opponentIsMagic", "", 1));
                ret.Add(new AddressList(0x15, "_pad_15", "", 1));
                ret.Add(new AddressList(0x16, "playerPowerWeight", "DEC", 2));
                ret.Add(new AddressList(0x18, "opponentPowerWeight", "DEC", 2));
                ret.Add(new AddressList(0x1A, "playerWeapon ItemID", "ITEM", 1));
                ret.Add(new AddressList(0x1B, "playerWeapon Stock", "DCE", 1));
                ret.Add(new AddressList(0x1C, "opponentWeapon ItemID", "ITEM", 1));
                ret.Add(new AddressList(0x1D, "opponentWeapon Stock", "DEC", 1));
                ret.Add(new AddressList(0x1E, "_pad_1e", "", 2));

                ret.Add(new AddressList(0x20, "gArenaOpponent", "LABEL", 0));
                ret.Add(new AddressList(0x20, "UnitPointer", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x24, "ClassPointer", "ROMCLASSPOINTER", 4));
                ret.Add(new AddressList(0x28, "Level", "DEC", 1));
                ret.Add(new AddressList(0x29, "EXP", "DEC", 1));
                ret.Add(new AddressList(0x2A, "Recovery mode", "", 1));
                ret.Add(new AddressList(0x2B, "UnitTableID", "", 1));
                ret.Add(new AddressList(0x2C, "State", "RAMUNITSTATE", 2));
                ret.Add(new AddressList(0x2E, "X", "DEC", 1));
                ret.Add(new AddressList(0x2F, "Y", "DEC", 1));
                ret.Add(new AddressList(0x30, "MAX HP", "DEC", 1));
                ret.Add(new AddressList(0x31, "Current HP", "DEC", 1));
                ret.Add(new AddressList(0x32, "Str", "DEC", 1));
                ret.Add(new AddressList(0x33, "Skill", "DEC", 1));
                ret.Add(new AddressList(0x34, "Spd", "DEC", 1));
                ret.Add(new AddressList(0x35, "Def", "DEC", 1));
                ret.Add(new AddressList(0x36, "Ref", "DEC", 1));
                ret.Add(new AddressList(0x37, "Luck", "DEC", 1));
                ret.Add(new AddressList(0x38, "CON", "DEC", 1));
                ret.Add(new AddressList(0x39, "TrvID", "RAMUNITAID", 1));
                ret.Add(new AddressList(0x3A, "Unknown1C", "", 1));
                ret.Add(new AddressList(0x3B, "MOV", "", 1));
                ret.Add(new AddressList(0x3C, "ItemID1", "ITEM", 1));
                ret.Add(new AddressList(0x3D, "ItemStock1", "DEC", 1));
                ret.Add(new AddressList(0x3E, "ItemID2", "ITEM", 1));
                ret.Add(new AddressList(0x3F, "ItemStock2", "DEC", 1));
                ret.Add(new AddressList(0x40, "ItemID3", "ITEM", 1));
                ret.Add(new AddressList(0x41, "ItemStock3", "DEC", 1));
                ret.Add(new AddressList(0x42, "ItemID4", "ITEM", 1));
                ret.Add(new AddressList(0x43, "ItemStock4", "DEC", 1));
                ret.Add(new AddressList(0x44, "ItemID5", "ITEM", 1));
                ret.Add(new AddressList(0x45, "ItemStock5", "DEC", 1));
                ret.Add(new AddressList(0x46, "Sword EXP", "", 1));
                ret.Add(new AddressList(0x47, "Lance EXP", "", 1));
                ret.Add(new AddressList(0x48, "Axs EXP", "", 1));
                ret.Add(new AddressList(0x49, "Bow EXP", "", 1));
                ret.Add(new AddressList(0x4A, "Staff EXP", "", 1));
                ret.Add(new AddressList(0x4B, "Rule EXP", "", 1));
                ret.Add(new AddressList(0x4C, "Light EXP", "", 1));
                ret.Add(new AddressList(0x4D, "Dark EXP", "", 1));
                ret.Add(new AddressList(0x4E, "State and turns", "", 0x1));
                ret.Add(new AddressList(0x4F, "PureWaterTourch", "", 0x1));
                ret.Add(new AddressList(0x50, "Support1", "", 0x1));
                ret.Add(new AddressList(0x51, "Support2", "", 0x1));
                ret.Add(new AddressList(0x52, "Support3", "", 0x1));
                ret.Add(new AddressList(0x53, "Support4", "", 0x1));
                ret.Add(new AddressList(0x54, "Support5", "", 0x1));
                ret.Add(new AddressList(0x55, "Support6", "", 0x1));
                ret.Add(new AddressList(0x56, "Support7", "", 0x1));
                ret.Add(new AddressList(0x57, "Support8", "", 0x1));
                ret.Add(new AddressList(0x58, "Support9", "", 0x1));
                ret.Add(new AddressList(0x59, "Support10", "", 0x1));
                ret.Add(new AddressList(0x5A, "SupportFlag", "", 0x1));
                ret.Add(new AddressList(0x5B, "Unknown3B", "", 0x1));
                ret.Add(new AddressList(0x5C, "Unknown3C", "", 0x1));
                ret.Add(new AddressList(0x5D, "Unknown3D", "", 0x1));
                ret.Add(new AddressList(0x5E, "Unknown3E", "", 0x1));
                ret.Add(new AddressList(0x5F, "Unknown3F", "", 0x1));
                ret.Add(new AddressList(0x60, "AI3", "", 0x1));
                ret.Add(new AddressList(0x61, "AI4", "", 0x1));
                ret.Add(new AddressList(0x62, "AI1", "AI1", 0x1));
                ret.Add(new AddressList(0x63, "AI1 Counter", "", 0x1));
                ret.Add(new AddressList(0x64, "AI2", "AI2", 0x1));
                ret.Add(new AddressList(0x65, "AI2 Counter", "", 0x1));
                ret.Add(new AddressList(0x66, "Unknown46", "", 0x1));
                ret.Add(new AddressList(0x67, "Unknown47", "", 0x1));
            }
            else
            {//FE7 , FE8
                ret.Add(new AddressList(0x00, "playerUnit", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x04, "opponentUnit", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x08, "betMoney", "DEC", 2));
                ret.Add(new AddressList(0x0A, "battleResults", "", 1));
                ret.Add(new AddressList(0x0B, "unk0B", "", 1));
                ret.Add(new AddressList(0x0C, "range", "", 1));
                ret.Add(new AddressList(0x0D, "playerWpnType", "WEAPONTYPE", 1));
                ret.Add(new AddressList(0x0E, "opponentWpnType", "WEAPONTYPE", 1));
                ret.Add(new AddressList(0x0F, "playerClassId", "CLASS", 1));
                ret.Add(new AddressList(0x10, "opponentClassId", "CLASS", 1));
                ret.Add(new AddressList(0x11, "playerLevel", "DEC", 1));
                ret.Add(new AddressList(0x12, "oppenentLevel", "DEC", 1));
                ret.Add(new AddressList(0x13, "playerIsMagic", "", 1));
                ret.Add(new AddressList(0x14, "opponentIsMagic", "", 1));
                ret.Add(new AddressList(0x15, "_pad_15", "", 1));
                ret.Add(new AddressList(0x16, "playerPowerWeight", "DEC", 2));
                ret.Add(new AddressList(0x18, "opponentPowerWeight", "DEC", 2));
                ret.Add(new AddressList(0x1A, "playerWeapon ItemID", "ITEM", 1));
                ret.Add(new AddressList(0x1B, "playerWeapon Stock", "DCE", 1));
                ret.Add(new AddressList(0x1C, "opponentWeapon ItemID", "ITEM", 1));
                ret.Add(new AddressList(0x1D, "opponentWeapon Stock", "DCE", 1));
                ret.Add(new AddressList(0x1E, "_pad_1e", "DCE", 2));

                ret.Add(new AddressList(0x20, "gArenaOpponent", "LABEL", 0));
                ret.Add(new AddressList(0x20, "UnitPointer", "ROMUNITPOINTER", 4));
                ret.Add(new AddressList(0x24, "ClassPointer", "ROMCLASSPOINTER", 4));
                ret.Add(new AddressList(0x28, "Level", "DEC", 1));
                ret.Add(new AddressList(0x29, "EXP", "DEC", 1));
                ret.Add(new AddressList(0x2A, "Recovery mode", "", 1));
                ret.Add(new AddressList(0x2B, "UnitTableID", "", 1));
                ret.Add(new AddressList(0x2C, "State", "RAMUNITSTATE", 4));
                ret.Add(new AddressList(0x20, "X", "DEC", 1));
                ret.Add(new AddressList(0x31, "Y", "DEC", 1));
                ret.Add(new AddressList(0x32, "MAX HP", "DEC", 1));
                ret.Add(new AddressList(0x33, "Current HP", "DEC", 1));
                ret.Add(new AddressList(0x34, "Str", "DEC", 1));
                ret.Add(new AddressList(0x35, "Skill", "DEC", 1));
                ret.Add(new AddressList(0x36, "Spd", "DEC", 1));
                ret.Add(new AddressList(0x37, "Def", "DEC", 1));
                ret.Add(new AddressList(0x38, "Ref", "DEC", 1));
                ret.Add(new AddressList(0x39, "Luck", "DEC", 1));
                ret.Add(new AddressList(0x3A, "CON", "DEC", 1));
                ret.Add(new AddressList(0x3B, "TrvID", "RAMUNITAID", 1));
                ret.Add(new AddressList(0x3C, "Unknown1C", "", 1));
                ret.Add(new AddressList(0x3D, "MOV", "", 1));
                ret.Add(new AddressList(0x3E, "ItemID1", "ITEM", 1));
                ret.Add(new AddressList(0x3F, "ItemStock1", "DEC", 1));
                ret.Add(new AddressList(0x40, "ItemID2", "ITEM", 1));
                ret.Add(new AddressList(0x41, "ItemStock2", "DEC", 1));
                ret.Add(new AddressList(0x42, "ItemID3", "ITEM", 1));
                ret.Add(new AddressList(0x43, "ItemStock3", "DEC", 1));
                ret.Add(new AddressList(0x44, "ItemID4", "ITEM", 1));
                ret.Add(new AddressList(0x45, "ItemStock4", "DEC", 1));
                ret.Add(new AddressList(0x46, "ItemID5", "ITEM", 1));
                ret.Add(new AddressList(0x47, "ItemStock5", "DEC", 1));
                ret.Add(new AddressList(0x48, "Sword EXP", "", 1));
                ret.Add(new AddressList(0x49, "Lance EXP", "", 1));
                ret.Add(new AddressList(0x4A, "Axs EXP", "", 1));
                ret.Add(new AddressList(0x4B, "Bow EXP", "", 1));
                ret.Add(new AddressList(0x4C, "Staff EXP", "", 1));
                ret.Add(new AddressList(0x4D, "Rule EXP", "", 1));
                ret.Add(new AddressList(0x4E, "Light EXP", "", 1));
                ret.Add(new AddressList(0x4F, "Dark EXP", "", 1));
                ret.Add(new AddressList(0x50, "State and turns", "", 0x1));
                ret.Add(new AddressList(0x51, "PureWaterTourch", "", 0x1));
                ret.Add(new AddressList(0x52, "Support1", "", 0x1));
                ret.Add(new AddressList(0x54, "Support2", "", 0x1));
                ret.Add(new AddressList(0x54, "Support3", "", 0x1));
                ret.Add(new AddressList(0x55, "Support4", "", 0x1));
                ret.Add(new AddressList(0x56, "Support5", "", 0x1));
                ret.Add(new AddressList(0x57, "Support6", "", 0x1));
                ret.Add(new AddressList(0x58, "Support7", "", 0x1));
                ret.Add(new AddressList(0x59, "SupportFlag", "", 0x1));
                ret.Add(new AddressList(0x5A, "Unknown3A", "", 0x1));
                ret.Add(new AddressList(0x5B, "Unknown3B", "", 0x1));
                ret.Add(new AddressList(0x5C, "Unknown3C", "", 0x1));
                ret.Add(new AddressList(0x5D, "Unknown3D", "", 0x1));
                ret.Add(new AddressList(0x5E, "Unknown3E", "", 0x1));
                ret.Add(new AddressList(0x5F, "Unknown3F", "", 0x1));
                ret.Add(new AddressList(0x60, "AI3", "", 0x1));
                ret.Add(new AddressList(0x61, "AI4", "", 0x1));
                ret.Add(new AddressList(0x62, "AI1", "AI1", 0x1));
                ret.Add(new AddressList(0x63, "AI1 Counter", "", 0x1));
                ret.Add(new AddressList(0x64, "AI2", "AI2", 0x1));
                ret.Add(new AddressList(0x65, "AI2 Counter", "", 0x1));
                ret.Add(new AddressList(0x66, "Unknown46", "", 0x1));
                ret.Add(new AddressList(0x67, "Unknown47", "", 0x1));
            }
            return ret;
        }
        static public List<EmulatorMemoryUtil.AddressList> GetDungeonDataStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            if (Program.ROM.RomInfo.version() != 8)
            {//FE6 FE7
                return ret;
            }
            ret.Add(new AddressList(0x00, "Exps", "DEC", 2));
            ret.Add(new AddressList(0x02, "DeployUnits", "DEC", 1));
            ret.Add(new AddressList(0x03, "ClearTurns", "DEC", 1));
            ret.Add(new AddressList(0x04, "Unk4", "", 2));
            ret.Add(new AddressList(0x06, "DefeatsCount", "DEC", 2));
            ret.Add(new AddressList(0x08, "Unk8", "", 1));
            ret.Add(new AddressList(0x09, "Unk9", "", 1));
            ret.Add(new AddressList(0x0A, "UnkA", "", 1));
            ret.Add(new AddressList(0x0B, "UnkB", "", 1));

            ret.Add(new AddressList(0x0C, "gBeforeDungeonState", "LABEL", 0));
            ret.Add(new AddressList(0x0C, "beforeExps", "DEC", 2));
            ret.Add(new AddressList(0x0E, "beforeDeployUnits", "DEC", 1));
            ret.Add(new AddressList(0x0F, "beforeClearTurns", "DEC", 1));
            ret.Add(new AddressList(0x10, "beforeUnk4", "", 2));
            ret.Add(new AddressList(0x12, "beforeDefeatsCount", "DEC", 2));
            ret.Add(new AddressList(0x14, "beforeUnk8", "", 1));
            ret.Add(new AddressList(0x15, "beforeUnk9", "", 1));
            ret.Add(new AddressList(0x16, "beforeUnkA", "", 1));
            ret.Add(new AddressList(0x17, "beforeUnkB", "", 1));

            ret.Add(new AddressList(0x18, "gDungeonSome", "LABEL", 0));
            ret.Add(new AddressList(0x18, "gDungeonSomething00", "", 4));
            ret.Add(new AddressList(0x1C, "gDungeonSomething04", "", 4));
            ret.Add(new AddressList(0x20, "gDungeonSomething08", "", 4));
            ret.Add(new AddressList(0x24, "gDungeonType", "DUNGEONTYPE", 1));
            ret.Add(new AddressList(0x25, "_pad_D", "", 1));
            ret.Add(new AddressList(0x26, "_pad_E", "", 1));
            ret.Add(new AddressList(0x27, "_pad_F", "", 1));
            return ret;
        }

        static public List<EmulatorMemoryUtil.AddressList> GetWorldmapStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            if (Program.ROM.RomInfo.version() != 8)
            {//FE6 FE7
                return ret;
            }
            ret.Add(new AddressList(0x00, "Flag", "WMFLAG1", 1));
            ret.Add(new AddressList(0x01, "Unk01", "", 1));
            ret.Add(new AddressList(0x02, "gWMCamera", "", 2));
            ret.Add(new AddressList(0x04, "Unk04", "", 2));
            ret.Add(new AddressList(0x06, "Unk06", "", 2));
            ret.Add(new AddressList(0x08, "Unk08", "", 4));
            ret.Add(new AddressList(0x08, "Unk0C", "", 4));
            ret.Add(new AddressList(0x10, "Unk10", "", 1));
            ret.Add(new AddressList(0x11, "gWMCurrentNode", "WMLOCATION", 1));
            ret.Add(new AddressList(0x12, "Unk12", "", 2));
            ret.Add(new AddressList(0x14, "Unk14", "", 2));
            ret.Add(new AddressList(0x18, "Unk18", "", 4));
            ret.Add(new AddressList(0x1C, "Unk1C", "", 4));
            ret.Add(new AddressList(0x20, "Unk20", "", 1));
            ret.Add(new AddressList(0x21, "Unk21", "", 1));
            ret.Add(new AddressList(0x22, "Unk22", "", 2));
            ret.Add(new AddressList(0x24, "Unk24", "", 2));
            ret.Add(new AddressList(0x26, "Unk26", "", 2));
            ret.Add(new AddressList(0x28, "Unk28", "", 2));
            ret.Add(new AddressList(0x2C, "Unk2C", "", 4));

            ret.Add(new AddressList(0x30, "WorldmapNode", "LABEL", 0));
            ret.Add(new AddressList(0x30, "Node00 Border Mulan", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x34, "Node01 Castle Frelia", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x38, "Node02 Ide", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x3C, "Node03 Borgo Ridge", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x40, "Node04 Zaha Woods", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x44, "Node05 Serafew", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x48, "Node06 Adlas Plains", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x4C, "Node07 Renvall", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x50, "Node08 Renvall", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x54, "Node09 Port Kiris", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x58, "Node0A Teraz Plateau", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x5C, "Node0B Caer Pelyn", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x60, "Node0C Hamill Canyon", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x64, "Node0D Jehanna Hall", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x68, "Node0E Fort Rigwald", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x6C, "Node0F Bethroen", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x70, "Node10 Taizel", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x74, "Node11 Zaalbul Marsh", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x78, "Node12 Grado Keep", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x7C, "Node13 Jehanna Hall", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x80, "Node14 Renais Castle", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x84, "Node15 Narube River", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x88, "Node16 Neleras Peak", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x8C, "Node17 Rausten Court", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x90, "Node18 Darkling Woods", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x94, "Node19 Black Temple", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x98, "Node1A Tower of Valni", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0x9C, "Node1B Lagdou Ruins", "WMNODE_FLAG", 4));
            ret.Add(new AddressList(0xA0, "Node1C Melkaen Coast", "WMNODE_FLAG", 4));

            ret.Add(new AddressList(0xA4, "WorldmapPath", "LABEL", 0));
            ret.Add(new AddressList(0xA4, "RoadID 00", "WMPATH", 1));
            ret.Add(new AddressList(0xA5, "RoadID 01", "WMPATH", 1));
            ret.Add(new AddressList(0xA6, "RoadID 02", "WMPATH", 1));
            ret.Add(new AddressList(0xA7, "RoadID 03", "WMPATH", 1));
            ret.Add(new AddressList(0xA8, "RoadID 04", "WMPATH", 1));
            ret.Add(new AddressList(0xA9, "RoadID 05", "WMPATH", 1));
            ret.Add(new AddressList(0xAA, "RoadID 06", "WMPATH", 1));
            ret.Add(new AddressList(0xAB, "RoadID 07", "WMPATH", 1));
            ret.Add(new AddressList(0xAC, "RoadID 08", "WMPATH", 1));
            ret.Add(new AddressList(0xAD, "RoadID 09", "WMPATH", 1));
            ret.Add(new AddressList(0xAE, "RoadID 0A", "WMPATH", 1));
            ret.Add(new AddressList(0xAF, "RoadID 0B", "WMPATH", 1));
            ret.Add(new AddressList(0xB0, "RoadID 0C", "WMPATH", 1));
            ret.Add(new AddressList(0xB1, "RoadID 0D", "WMPATH", 1));
            ret.Add(new AddressList(0xB2, "RoadID 0E", "WMPATH", 1));
            ret.Add(new AddressList(0xB3, "RoadID 0F", "WMPATH", 1));
            ret.Add(new AddressList(0xB4, "RoadID 10", "WMPATH", 1));
            ret.Add(new AddressList(0xB5, "RoadID 11", "WMPATH", 1));
            ret.Add(new AddressList(0xB6, "RoadID 12", "WMPATH", 1));
            ret.Add(new AddressList(0xB7, "RoadID 13", "WMPATH", 1));
            ret.Add(new AddressList(0xB8, "RoadID 14", "WMPATH", 1));
            ret.Add(new AddressList(0xB9, "RoadID 15", "WMPATH", 1));
            ret.Add(new AddressList(0xBA, "RoadID 16", "WMPATH", 1));
            ret.Add(new AddressList(0xBB, "RoadID 17", "WMPATH", 1));
            ret.Add(new AddressList(0xBC, "RoadID 18", "WMPATH", 1));
            ret.Add(new AddressList(0xBD, "RoadID 19", "WMPATH", 1));
            ret.Add(new AddressList(0xBE, "RoadID 1A", "WMPATH", 1));
            ret.Add(new AddressList(0xBF, "RoadID 1B", "WMPATH", 1));
            ret.Add(new AddressList(0xC0, "RoadID 1C", "WMPATH", 1));
            ret.Add(new AddressList(0xC1, "RoadID 1D", "WMPATH", 1));
            ret.Add(new AddressList(0xC2, "RoadID 1E", "WMPATH", 1));
            ret.Add(new AddressList(0xC3, "Road Count", "DEC", 1));

            return ret;
        }
        static public List<EmulatorMemoryUtil.AddressList> GetAIDataStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            ret.Add(new AddressList(0x00, "aiUnits0", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x01, "aiUnits1", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x02, "aiUnits2", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x03, "aiUnits3", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x04, "aiUnits4", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x05, "aiUnits5", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x06, "aiUnits6", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x07, "aiUnits7", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x08, "aiUnits8", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x09, "aiUnits9", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x0A, "aiUnits..", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x74, "aiUnitIt", "", 4));
            ret.Add(new AddressList(0x78, "cpOrderNext", "", 1));
            ret.Add(new AddressList(0x79, "cpDecideNext", "", 1));
            ret.Add(new AddressList(0x7A, "dangerMapActive", "", 1));
            ret.Add(new AddressList(0x7B, "aiConfig", "", 1));
            ret.Add(new AddressList(0x7C, "Unk7C", "", 1));
            ret.Add(new AddressList(0x7D, "Unk7D", "", 1));
            ret.Add(new AddressList(0x7E, "Unk7E", "", 1));
            ret.Add(new AddressList(0x7F, "_pad7F", "", 1));
            ret.Add(new AddressList(0x80, "someFlag80", "", 4));
            ret.Add(new AddressList(0x84, "Unk84", "", 1));
            ret.Add(new AddressList(0x85, "highestBlueMov", "", 1));
            ret.Add(new AddressList(0x86, "AI Result1", "", 1));
            ret.Add(new AddressList(0x87, "AI Result2", "", 1));
            ret.Add(new AddressList(0x88, "AI Result3", "", 1));
            ret.Add(new AddressList(0x89, "AI Result4", "", 1));
            ret.Add(new AddressList(0x8A, "AI Result5", "", 1));
            ret.Add(new AddressList(0x8B, "_pad8B", "", 1));
            ret.Add(new AddressList(0x8C, "_pad8C", "", 1));
            ret.Add(new AddressList(0x8D, "_pad8D", "", 1));
            ret.Add(new AddressList(0x8E, "_pad8E", "", 1));
            ret.Add(new AddressList(0x8F, "_pad8F", "", 1));
            ret.Add(new AddressList(0x90, "AiDecision", "LABEL", 0));
            ret.Add(new AddressList(0x90, "decisionType", "", 1));
            ret.Add(new AddressList(0x91, "unitIndex", "RAMUNITINDEX", 1));
            ret.Add(new AddressList(0x92, "xMovement", "DEC", 1));
            ret.Add(new AddressList(0x93, "yMovement", "DEC", 1));
            ret.Add(new AddressList(0x94, "pad_04", "", 1));
            ret.Add(new AddressList(0x95, "pad_05", "", 1));
            ret.Add(new AddressList(0x96, "unitTargetIndex", "", 1));
            ret.Add(new AddressList(0x97, "usedItemSlot", "", 1));
            ret.Add(new AddressList(0x98, "xOther", "", 1));
            ret.Add(new AddressList(0x99, "yOther", "", 1));
            ret.Add(new AddressList(0x9A, "decisionTaken", "", 1));
            ret.Add(new AddressList(0x9B, "decisionType", "", 1));
            ret.Add(new AddressList(0x9C, "UnkAIStruct", "LABEL", 0));
            ret.Add(new AddressList(0x9C, "Unk0", "", 1));
            ret.Add(new AddressList(0x9D, "Unk1", "", 1));
            ret.Add(new AddressList(0x9E, "Unk2", "", 1));
            ret.Add(new AddressList(0x9F, "Unk3", "", 1));

            return ret;
        }
        static public List<EmulatorMemoryUtil.AddressList> GetActionDataStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            ret.Add(new AddressList(0x00, "Unk00", "", 2));
            ret.Add(new AddressList(0x00, "Unk02", "", 2));
            ret.Add(new AddressList(0x00, "Unk04", "", 2));
            ret.Add(new AddressList(0x06, "Unk06", "", 2));
            ret.Add(new AddressList(0x08, "Unk08", "", 2));
            ret.Add(new AddressList(0x0A, "Unk0A", "", 2));
            ret.Add(new AddressList(0x0B, "Unk0B", "", 2));
            ret.Add(new AddressList(0x0C, "subjectIndex", "", 1));
            ret.Add(new AddressList(0x0D, "targetIndex", "", 1));
            ret.Add(new AddressList(0x0E, "xMove", "DEC", 1));
            ret.Add(new AddressList(0x0F, "yMove", "DEC", 1));
            ret.Add(new AddressList(0x10, "moveCount", "DEC", 1));
            ret.Add(new AddressList(0x11, "unitActionType", "UNITACTION", 1));
            ret.Add(new AddressList(0x12, "itemSlotIndex", "", 1));
            ret.Add(new AddressList(0x13, "xOther", "DEC", 1));
            ret.Add(new AddressList(0x14, "yOther", "DEC", 1));
            ret.Add(new AddressList(0x15, "trapType", "", 1));
            ret.Add(new AddressList(0x16, "suspendPointType", "", 1));
            ret.Add(new AddressList(0x18, "scriptedBattleHits", "", 4));
            ret.Add(new AddressList(0x1C, "Unk1C", "", 4));

            return ret;
        }
        static public List<EmulatorMemoryUtil.AddressList> GetBattleSomeDataStruct()
        {
            List<AddressList> ret = new List<AddressList>();
            if (Program.ROM.RomInfo.version() != 8)
            {//FE6 FE7
                return ret;
            }
            ret.Add(new AddressList(0x00, "UnknownBattleFlag", "", 4));
            ret.Add(new AddressList(0x04, "BoolBattleDeamonActive", "", 4));
            ret.Add(new AddressList(0x08, "EkrBattleDeamon", "POINTER", 4));
            ret.Add(new AddressList(0x0C, "Unk0C", "", 2));
            ret.Add(new AddressList(0x0E, "BattleBG", "BATTLEBG", 2));
            ret.Add(new AddressList(0x10, "BattleAttackPhaseBool", "", 4));
            ret.Add(new AddressList(0x14, "Unk10", "", 2));
            ret.Add(new AddressList(0x16, "Unk12", "", 2));
            ret.Add(new AddressList(0x18, "BattleActorTargetOrder", "", 4));
            ret.Add(new AddressList(0x1C, "BattleAnimId 1", "BATTLEANIME-1", 2));
            ret.Add(new AddressList(0x1E, "BattleAnimId 2", "BATTLEANIME-1", 2));
            ret.Add(new AddressList(0x20, "BattlePalette 1", "UNITPALETTE-1", 2));
            ret.Add(new AddressList(0x22, "BattlePalette 2", "UNITPALETTE-1", 2));
            ret.Add(new AddressList(0x24, "Unk24", "", 2));
            ret.Add(new AddressList(0x26, "SomeAllegienceIdShort", "", 2));
            ret.Add(new AddressList(0x28, "BattleSpellAnimationId 1", "SPELLANIMATION", 2));
            ret.Add(new AddressList(0x2A, "BattleSpellAnimationId 2", "SPELLANIMATION", 2));
            ret.Add(new AddressList(0x2C, "BattleTerrain 1", "BATTLETERRAIN", 2));
            ret.Add(new AddressList(0x2E, "BattleTerrain 2", "BATTLETERRAIN", 2));
            ret.Add(new AddressList(0x30, "AnimAndDistance", "", 2));
            ret.Add(new AddressList(0x32, "Unk32", "", 2));
            ret.Add(new AddressList(0x34, "Unk34", "", 2));
            ret.Add(new AddressList(0x36, "Unk36", "", 2));
            ret.Add(new AddressList(0x38, "Unk38", "", 2));

            ret.Add(new AddressList(0x3A, "AISRelatedShorts", "LABEL", 0));
            ret.Add(new AddressList(0x3A, "AISRelatedShorts[0]", "", 2));
            ret.Add(new AddressList(0x3B, "AISRelatedShorts[1]", "", 2));
            ret.Add(new AddressList(0x3E, "AISRelatedShorts[2]", "", 2));
            ret.Add(new AddressList(0x40, "AISRelatedShorts[3]", "", 2));
            ret.Add(new AddressList(0x42, "AISRelatedShorts[4]", "", 2));
            ret.Add(new AddressList(0x44, "AISRelatedShorts[5]", "", 2));
            ret.Add(new AddressList(0x46, "AISRelatedShorts[6]", "", 2));
            ret.Add(new AddressList(0x48, "AISRelatedShorts[7]", "", 2));
            ret.Add(new AddressList(0x4A, "AISRelatedShorts[8]", "", 2));
            ret.Add(new AddressList(0x4C, "AISRelatedShorts[9]", "", 2));
            ret.Add(new AddressList(0x4E, "AISRelatedShorts...", "", 2));

            ret.Add(new AddressList(0x7A, "SomeBattle2", "LABEL", 0));
            ret.Add(new AddressList(0x7A, "Unk7A", "", 2));
            ret.Add(new AddressList(0x7C, "Unk7C", "", 2));
            ret.Add(new AddressList(0x92, "BattleAnimId_B 1", "BATTLEANIME-1", 2));
            ret.Add(new AddressList(0x94, "BattleAnimId_B 2", "BATTLEANIME-1", 2));
            ret.Add(new AddressList(0x96, "Pad96", "", 2));
            ret.Add(new AddressList(0x98, "ToDefenserPointer", "POINTER", 4));
            ret.Add(new AddressList(0x9C, "ToAttackerPointer", "POINTER", 4));
            ret.Add(new AddressList(0xA0, "BatttleUnitID 1", "UNIT", 1));
            ret.Add(new AddressList(0xA1, "BatttleUnitID 2", "UNIT", 1));
            ret.Add(new AddressList(0xA2, "UnkA2", "", 4));
            ret.Add(new AddressList(0xA4, "ExtraUnit 1", "", 4));
            ret.Add(new AddressList(0xA8, "ExtraUnit 2", "", 4));
            ret.Add(new AddressList(0xAC, "UnkAC", "", 4));
            ret.Add(new AddressList(0xB0, "UnkB0", "", 4));

            ret.Add(new AddressList(0xB4, "DisplayValue", "LABEL", 0));
            ret.Add(new AddressList(0xB4, "BoolLockSheetGraphics", "", 4));
            ret.Add(new AddressList(0xB8, "UnkB8", "", 4));
            ret.Add(new AddressList(0xBC, "BattleScreenHP_A 1", "DEC", 2));
            ret.Add(new AddressList(0xBE, "BattleScreenHP_A 2", "DEC", 2));
            ret.Add(new AddressList(0xC0, "BattleScreenHP_B 1", "DEC", 2));
            ret.Add(new AddressList(0xC2, "BattleScreenHP_B 2", "DEC", 2));
            ret.Add(new AddressList(0xC4, "BattleScreenHP_C 1", "DEC", 2));
            ret.Add(new AddressList(0xC6, "BattleScreenHP_C 2", "DEC", 2));
            ret.Add(new AddressList(0xC8, "BattleScreenHit 1", "DEC", 2));
            ret.Add(new AddressList(0xCA, "BattleScreenHit 2", "DEC", 2));
            ret.Add(new AddressList(0xCC, "BattleScreenDmg 1", "DEC", 2));
            ret.Add(new AddressList(0xCE, "BattleScreenDmg 2", "DEC", 2));
            ret.Add(new AddressList(0xD0, "BattleScreenCrit 1", "DEC", 2));
            ret.Add(new AddressList(0xD2, "BattleScreenCrit 2", "DEC", 2));
            ret.Add(new AddressList(0xD4, "UnkD4", "", 2));
            ret.Add(new AddressList(0xD6, "UnkD6", "", 2));
            ret.Add(new AddressList(0xD8, "UnkD8", "", 2));
            ret.Add(new AddressList(0xDA, "UnkDA", "", 2));
            ret.Add(new AddressList(0xDC, "UnkDC", "", 2));
            ret.Add(new AddressList(0xDC, "UnkDE", "", 2));
            ret.Add(new AddressList(0xE0, "UnkE0", "", 2));
            ret.Add(new AddressList(0xE2, "UnkE2", "", 2));
            ret.Add(new AddressList(0xE4, "UnkE4", "", 2));
            ret.Add(new AddressList(0xE6, "UnkE6", "", 2));
            ret.Add(new AddressList(0xE8, "UnkE8", "", 2));
            ret.Add(new AddressList(0xEA, "UnkEA", "", 2));
            ret.Add(new AddressList(0xEC, "UnkEC", "", 2));
            ret.Add(new AddressList(0xEE, "UnkEE", "", 2));
            ret.Add(new AddressList(0xF0, "UnkF0", "", 4));

            ret.Add(new AddressList(0xF4, "SomeBattle3", "LABEL", 0));
            ret.Add(new AddressList(0xF4, "BoolIsArena", "", 4));
            ret.Add(new AddressList(0xF8, "UnkF8", "", 4));
            ret.Add(new AddressList(0xFC, "UnkFC", "", 2));
            ret.Add(new AddressList(0xFE, "UnkFE", "", 2));
            return ret;
        }
        public static string GetAddressList(AddressList a, uint addr)
        {
            if (a.Size == 0)
            {
                return "";
            }

            uint v = 0;
            if (a.Size == 1)
            {
                v = Program.RAM.u8(addr);
            }
            else if (a.Size == 2)
            {
                v = Program.RAM.u16(addr);
            }
            else if (a.Size == 4)
            {
                v = Program.RAM.u32(addr);
            }

            if (a.Type == "DEC")
            {
                return v.ToString();
            }
            else if (a.Type == "MAP")
            {
                return U.To0xHexString(v) + " (" + MapSettingForm.GetMapName(v) + ")";
            }
            else if (a.Type == "FOG")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetFOG(v)  + ")";
            }
            else if (a.Type == "PHASE")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetPHASE(v) + ")";
            }
            else if (a.Type == "CHAPTERSTUFF")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetChatperStaff(v) + ")";
            }
            else if (a.Type == "EDITON")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetEditon(v) + ")";
            }
            else if (a.Type == "WEATHER")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetWEATHER(v) + ")";
            }
            else if (a.Type == "STRING")
            {
                FETextDecode decoder = new FETextDecode();
                uint len = Program.RAM.strlen(addr);
                len = Math.Min(len, a.Size);
                byte[] srcdata = Program.RAM.getBinaryData(addr, len);
                return decoder.UnHffmanPatchDecodeLow(srcdata);
            }
            else if (a.Type == "CHAPTERCONFIG")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetChapterDataConfig(v) + ")";
            }
            else if (a.Type == "WMFLAG1")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetWorldmapStructWMFLAG1(v) + ")";
            }
            else if (a.Type == "UNIT")
            {
                return U.To0xHexString(v) + " (" + UnitForm.GetUnitName(v) + ")";
            }
            else if (a.Type == "CLASS")
            {
                return U.To0xHexString(v) + " (" + ClassForm.GetClassName(v) + ")";
            }
            else if (a.Type == "ITEM")
            {
                return U.To0xHexString(v) + " (" + ItemForm.GetItemName(v) + ")";
            }
            else if (a.Type == "WMLOCATION")
            {
                return U.To0xHexString(v) + " (" + WorldMapPointForm.GetWorldMapPointName(v) + ")";
            }
            else if (a.Type == "WMPATH")
            {
                return U.To0xHexString(v) + " (" + WorldMapPathForm.GetPathName(v) + ")";
            }
            else if (a.Type == "TILE")
            {
                return U.To0xHexString(v) + " (" + MapTerrainNameForm.GetName(v) + ")";
            }
            else if (a.Type == "RAMUNITSTATE")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetRAM_UNIT_STATE(v) + ")";
            }
            else if (a.Type == "RAMUNITAID")
            {
                return U.To0xHexString(v) + " (" + EmulatorMemoryUtil.GetRAMUnitAIDToName(v) + ")";
            }
            else if (a.Type == "RAMUNITINDEX")
            {
                return U.To0xHexString(v) + " (" + EmulatorMemoryUtil.GetRAMUnitAIDToName(v) + ")";
            }
            else if (a.Type == "AI1")
            {
                return U.To0xHexString(v) + " (" + EventUnitForm.GetAIName1(v) + ")";
            }
            else if (a.Type == "AI2")
            {
                return U.To0xHexString(v) + " (" + EventUnitForm.GetAIName2(v) + ")";
            }
            else if (a.Type == "WEAPONTYPE")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetWeaponTypeName(v) + ")";
            }
            else if (a.Type == "UNITACTION")
            {
                return U.To0xHexString(v) + " (" + UnitActionPointerForm.GetActionName(v) + ")";
            }
            else if (a.Type == "WMNODE_FLAG")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetWorldmapNodeFLAG(v) + ")";
            }
            else if (a.Type == "DUNGEONTYPE")
            {
                return U.To0xHexString(v) + " (" + InputFormRef.GetDungeonType(v) + ")";
            }
            else if (a.Type == "SONG")
            {
                return U.To0xHexString(v) + " (" + SongTableForm.GetSongName(v) + ")";
            }
            else if (a.Type == "BATTLEBG")
            {
                return U.To0xHexString(v) + " (" + ImageBattleBGForm.GetName(v) + ")";
            }
            else if (a.Type == "BATTLETERRAIN")
            {
                return U.To0xHexString(v) + " (" + ImageBattleTerrainForm.GetBattleTerrainName(v) + ")";
            }
            else if (a.Type == "SPELLANIMATION")
            {
                return U.To0xHexString(v) + " (" + ItemWeaponEffectForm.GetEffectName(v) + ")";
            }
            else if (a.Type == "BATTLEANIME-1")
            {
                return U.To0xHexString(v) + "+1 (" + ImageBattleAnimeForm.GetBattleAnimeName(v + 1) + ")";
            }
            else if (a.Type == "UNITPALETTE-1")
            {
                return U.To0xHexString(v) + "+1 (" + ImageUnitPaletteForm.GetPaletteName(v + 1) + ")";
            }
            else if (a.Type == "ROMUNITPOINTER" || a.Type == "ROMCLASSPOINTER")
            {
                string name = "";
                if (U.isSafetyPointer(v))
                {
                    uint textid = Program.ROM.u16(U.toOffset(v));
                    uint id = Program.ROM.u8(U.toOffset(v + 4));
                    name = U.To0xHexString(id) + " " + FETextDecode.Direct(textid);
                }
                return U.To0xHexString(v) + " (" + name + ")";
            }
            else if (a.Type == "POINTER")
            {
                string name = "";
                if (U.is_ROMorRAMPointer(v))
                {
                    name = Program.AsmMapFileAsmCache.GetName(v);
                }
                return U.To0xHexString(v) + " (" + name + ")";
            }
            else
            {
                return U.To0xHexString(v);
            }
        }
    }
}
