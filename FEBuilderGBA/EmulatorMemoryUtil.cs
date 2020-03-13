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
            uint gSomeWMEventRelatedStruct;
            if (Program.ROM.RomInfo.is_multibyte())
            {//FE8J
                gSomeWMEventRelatedStruct = 0x03005270;
            }
            else
            {//FE8U
                gSomeWMEventRelatedStruct = 0x03005280;
            }
            nodeid = Program.RAM.u8(gSomeWMEventRelatedStruct + 0x11);
            return nodeid;
        }

        public static void CHEAT_WARP_FE8(EmulatorMemoryForm form, uint warp_chapter, uint edtion, uint worldmap_node)
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 8);

            uint work_address = Program.ROM.RomInfo.workmemory_last_string_address() - 0x70; //テキストバッファの一番下をデータ置き場として利用する.
            uint gSomeWMEventRelatedStruct;
            uint eventExecuteFucntion;
            uint endAllMenusFunction;
            uint deletePlayerPhaseInterface6CsFunction;

            PatchUtil.mnc2_fix_enum use_mnc2 = PatchUtil.SearchSkipWorldMapPatch();
            if (use_mnc2 == PatchUtil.mnc2_fix_enum.NO)
            {
                R.ShowStopError("MNC2の修正パッチを適応していないので、章ワープを利用できません。");
                return;
            }
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
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした");
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
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_mapid_address() - 0xE;
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
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした");
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
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_mapid_address() - 0xE;
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
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした");
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
                R.ShowStopError("MAPTASK Procsの位置を特定できませんでした");
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
    }
}
