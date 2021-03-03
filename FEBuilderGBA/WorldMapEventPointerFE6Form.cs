using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapEventPointerFE6Form : Form
    {
        public WorldMapEventPointerFE6Form()
        {
            InitializeComponent();

            this.N_InputFormRef = N_Init(this);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので、警告不能.
        }


        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.map_setting_pointer()
                , Program.ROM.RomInfo.map_setting_datasize()
                , (int i, uint addr) =>
                {
                    //0 がポインタであればデータがあると考える.
                    uint a = Program.ROM.u32(addr + 0);
                    //                    return U.isPointer(a) || a == 0;
                    return U.isPointer(a);
                }
                , (int i, uint addr) =>
                {
                    U.AddrResult r = new U.AddrResult();

                    uint worldmapEventPlist = MapSettingForm.GetWorldMapEventIDWhereAddr(addr);
                    if (worldmapEventPlist <= 0)
                    {
                        return r;
                    }

                    r.name = U.ToHexString(i) + MapSettingForm.GetMapNameWhereAddr(addr);
                    r.addr = Program.ROM.p32(Program.ROM.RomInfo.map_map_pointer_pointer()) + (worldmapEventPlist * 4);
                    return r;
                }
                );
        }

        private void WorldMapEventPointerFE6Form_Load(object sender, EventArgs e)
        {

        }

        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = N_Init(null);
            return InputFormRef.MakeList();
        }

        public static uint GetEventByMapID(uint mapid)
        {
            uint wmapid = MapSettingForm.GetWorldMapEventIDWhereMapID(mapid);
            if (wmapid == 0)
            {//存在しない
                return U.NOT_FOUND;
            }
            //FE6はPLISTが格納されている.
            return MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.WORLDMAP_FE6ONLY, wmapid);
        }

        public void JumpTo(uint value)
        {
            U.SelectedIndexSafety(N_AddressList, value);
        }
        public static bool isWorldMapEvent(uint addr)
        {
            addr = U.toOffset(addr);
            {
                InputFormRef InputFormRef = N_Init(null);
                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    uint eventAddr = Program.ROM.p32(p);
                    if (addr == eventAddr)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            List<uint> tracelist = new List<uint>();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                uint wmapid = MapSettingForm.GetWorldMapEventIDWhereMapID(mapid);
                if (wmapid == 0)
                {//存在しない
                    continue;
                }
                //FE6はPLISTが格納されている.
                uint p;
                uint event_addr = MapPointerForm.PlistToOffsetAddrFast(MapPointerForm.PLIST_TYPE.WORLDMAP_FE6ONLY, wmapid, out p);
                if (event_addr == U.NOT_FOUND)
                {
                    continue;
                }

                string name = "WorldMapEvent " + U.To0xHexString(mapid) + " ";
                EventScriptForm.ScanScript(list, p, true, true, name, tracelist);
            }
        }
        //エラー検出
        public static void MakeCheckErrors(uint mapid, List<FELint.ErrorSt> errors)
        {
            List<uint> tracelist = new List<uint>();
            uint wmapid = MapSettingForm.GetWorldMapEventIDWhereMapID(mapid);
            if (wmapid == 0)
            {//存在しない
                return ;
            }
            //FE6はPLISTが格納されている.
            uint p;
            uint event_addr = MapPointerForm.PlistToOffsetAddrFast(MapPointerForm.PLIST_TYPE.WORLDMAP_FE6ONLY, wmapid, out p);
            if (event_addr == U.NOT_FOUND)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_WORLDMAP,U.NOT_FOUND
                    ,R._("対応するワールドマップイベント({0})が存在しません。",U.To0xHexString(wmapid) )));
            }
            else
            {
                FELint.CheckEvent(event_addr, errors, FELint.Type.WORLDMAP_EVENT, p, true, tracelist);
            }
        }

        //テキストIDの取得
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            List<uint> tracelist = new List<uint>();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                uint wmapid = MapSettingForm.GetWorldMapEventIDWhereMapID(mapid);
                if (wmapid == 0)
                {//存在しない
                    continue;
                }
                //FE6はPLISTが格納されている.
                uint p;
                uint event_addr = MapPointerForm.PlistToOffsetAddrFast(MapPointerForm.PLIST_TYPE.WORLDMAP_FE6ONLY, wmapid, out p);
                if (event_addr == U.NOT_FOUND)
                {
                    continue;
                }

                string name = "WorldMapEvent " + U.To0xHexString(mapid) + " ";
                EventCondForm.MakeVarsIDArrayByEventPointer(list, p, name, tracelist);
            }

        }
    }
}
