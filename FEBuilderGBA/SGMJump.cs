using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    class SGMJump
    {
        static string FindTargetSGMFilename(string romfilename)
        {
            DateTime romLastModified = File.GetLastWriteTime(romfilename);

            DateTime newest = romLastModified;
            string targetfilename = "";

            string dir = Path.GetDirectoryName(romfilename);
            string name = Path.GetFileNameWithoutExtension(romfilename);
            for (int i = 1; i <= 10; i++)
            {
                string sgmfilename = Path.Combine(dir, name + i + ".sgm");
                if (! File.Exists(sgmfilename))
                {//セーブファイルがない.
                    continue;
                }
                DateTime sgmLastModified = File.GetLastWriteTime(sgmfilename);
                if (sgmLastModified <= romLastModified)
                {//エミュ実行後にセーブされていない.
                    continue;
                }
                if (newest > sgmLastModified)
                {
                    continue;
                }
                newest = sgmLastModified;
                targetfilename = sgmfilename;
            }

            return targetfilename;
        }
        public static void Jump(string romfilename)
        {
            if (OptionForm.sgm_jump() == OptionForm.sgm_jump_enum.None)
            {
                return;
            }

            string targetfilename = FindTargetSGMFilename(romfilename);
            if (targetfilename.Length <= 0)
            {
                return;
            }

            SGMLoader sgm = new SGMLoader(targetfilename);
//            sgm.DebugSave();
            uint mapmax = MapSettingForm.GetDataCount();

            uint[] events;
            if (!FindEvent(sgm, mapmax, out events))
            {//イベントが見つからない. 

                //マップIDはとれる?
                uint mapid = sgm.getMapID();
                if (mapid >= MapSettingForm.GetDataCount())
                {//マップの最大値を超えています.
                    return;
                }

                InputFormRef.JumpForm<MainSimpleMenuForm>(mapid, "MAP_LISTBOX");
                return;
            }

            List<uint> tracelist = new List<uint>();
            for (int i = 0; i < events.Length; i++)
            {
                uint found_eventaddr = events[i];

                //イベントが保持しているテキストを取得.
                List<U.AddrResult> text_arlist = new List<U.AddrResult>();
                EventCondForm.MakeTextIDEventScan(ref text_arlist, found_eventaddr, tracelist);

                uint event_current_addr = U.NOT_FOUND;

                uint last_stringid = sgm.getLastStringID();
                if (last_stringid >= TextForm.GetDataCount())
                {
                    //sgmから読み込めた、最後に表示した文字列IDが正しくない
                    break;
                }

                uint index = U.FindList(text_arlist, last_stringid);
                if (index == U.NOT_FOUND)
                {//実行しているイベントに、あるはずの文字が取れていない.
                    continue;
                }

                //取得できた その文字列IDの部分あたりを実行しているらしい.
                event_current_addr = text_arlist[(int)index].tag;

                EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                f.JumpTo(found_eventaddr, event_current_addr);
                return;
            }

            {
                //確実な方の event[1] を返す.
                //events[0]は、たぶん、現在実行しているイベントアドレス.
                //events[1]は、イベント条件等に掲載されているイベントアドレス
                uint found_eventaddr = events[1];

                //文字列マッチに失敗した.
                EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                f.JumpTo(found_eventaddr);
                return;
            }
        }

        static bool FindEvent(SGMLoader sgm,uint mapmax,out uint[] out_events)
        {
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                List<U.AddrResult> event_arlist = EventCondForm.MakeEventScriptPointer(mapid);
                if ( FindEventInner(sgm, event_arlist ,out out_events))
                {
                    return true;
                }
            }

            //イベントが見つからない場合、ワールドマップイベントからも探す
            if (Program.ROM.RomInfo.version() == 6)
            {
                List<U.AddrResult> event_arlist = WorldMapEventPointerFE6Form.MakeList();
                if ( FindEventInner(sgm, event_arlist ,out out_events))
                {
                    return true;
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                List<U.AddrResult> event_arlist = WorldMapEventPointerFE7Form.MakeList();
                if ( FindEventInner(sgm, event_arlist ,out out_events))
                {
                    return true;
                }
            }
            else
            {
                List<U.AddrResult> event_arlist = WorldMapEventPointerForm.MakeBeforeEventList();
                if ( FindEventInner(sgm, event_arlist ,out out_events))
                {
                    return true;
                }

                event_arlist = WorldMapEventPointerForm.MakeAfterEventList();
                if (FindEventInner(sgm, event_arlist, out out_events))
                {
                    return true;
                }

                uint speventaddr = Program.ROM.p32(Program.ROM.RomInfo.oping_event_pointer());
                if (sgm.GrepEvent(speventaddr , out out_events))
                {
                    return true;
                }

                speventaddr = Program.ROM.p32(Program.ROM.RomInfo.ending1_event_pointer());
                if (sgm.GrepEvent(speventaddr , out out_events))
                {
                    return true;
                }

                speventaddr = Program.ROM.p32(Program.ROM.RomInfo.ending2_event_pointer());
                if (sgm.GrepEvent(speventaddr , out out_events))
                {
                    return true;
                }
            }

            return false;
        }

        static bool FindEventInner(SGMLoader sgm, List<U.AddrResult> event_arlist,out uint[] out_events)
        {
            for (int i = 0; i < event_arlist.Count; i++)
            {
                if (sgm.GrepEvent(event_arlist[i].addr, out out_events))
                {
                    return true;
                }
            }

            out_events = null;
            return false;
        }
    }
}
