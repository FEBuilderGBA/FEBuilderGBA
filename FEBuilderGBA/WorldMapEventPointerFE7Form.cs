using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapEventPointerFE7Form : Form
    {
        public WorldMapEventPointerFE7Form()
        {
            InitializeComponent();

            this.N_InputFormRef = N_Init(this);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }


        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.worldmap_event_on_stageselect_pointer()
                , 4
                , (int i, uint addr) =>
                {
                    if (i == 0)
                    {
                        return true;
                    }
                    return U.isPointer(Program.ROM.u32(addr))
                        ;
                }
                , (int i, uint addr) =>
                {
                    string name = "";
                    if (i > 0)
                    {
                        name = MapSettingForm.GetMapNameFromWorldMapEventID((uint)i);
                    }
                    return U.ToHexString(i) + " " + name;
                }
                );
        }

        private void WorldMapEventPointerFE7Form_Load(object sender, EventArgs e)
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
            //FE7はINDEX
            InputFormRef InputFormRef = N_Init(null);
            uint p = InputFormRef.IDToAddr(wmapid);
            if (p == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            uint event_addr = Program.ROM.p32(p);
            if (!U.isSafetyOffset(event_addr))
            {
                return U.NOT_FOUND;
            }
            return event_addr;
        }
        public void JumpTo(uint value)
        {
            N_AddressList.SelectedIndex = (int)value;
        }
        public static bool isWorldMapEvent(uint addr)
        {
            addr = U.toOffset(addr);
            {
                InputFormRef InputFormRef = N_Init(null);
                for (int i = 0; i < InputFormRef.DataCount; i++)
                {
                    uint p = InputFormRef.BaseAddress + (uint)i * InputFormRef.BlockSize;
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
            {
                InputFormRef InputFormRef = N_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "WorldMapEvent ", new uint[] { 0 });

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    string name = "WorldMapEvent " + U.To0xHexString(i) + " ";
                    EventScriptForm.ScanScript( list, p, true, true, name , tracelist);
                }
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
            //FE7はINDEX
            uint p;
            InputFormRef InputFormRef = N_Init(null);
            p = InputFormRef.IDToAddr(wmapid);
            if (p == U.NOT_FOUND)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_WORLDMAP,U.NOT_FOUND
                    ,R._("対応するワールドマップイベント({0})が存在しません。",U.To0xHexString(wmapid) )));
            }
            else
            {
                uint event_addr = Program.ROM.u32(p);
                FELint.CheckEventPointerErrors(event_addr, errors, FELint.Type.WORLDMAP_EVENT, p, true, tracelist);
            }
        }
    }
}
