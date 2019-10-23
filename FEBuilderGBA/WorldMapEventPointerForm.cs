using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapEventPointerForm : Form
    {
        public WorldMapEventPointerForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.N_InputFormRef = N_Init(this);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
 
            OPNING_EVENT.Value = Program.ROM.p32(Program.ROM.RomInfo.oping_event_pointer());
            ENDING1_EVENT.Value = Program.ROM.p32(Program.ROM.RomInfo.ending1_event_pointer());
            ENDING2_EVENT.Value = Program.ROM.p32(Program.ROM.RomInfo.ending2_event_pointer());
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.worldmap_event_on_stageclear_pointer()
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

        public static List<U.AddrResult> MakeBeforeEventList()
        {
            InputFormRef InputFormRef = N_Init(null);
            return InputFormRef.MakeList();
        }
        public static List<U.AddrResult> MakeAfterEventList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }
        public static uint GetEventByMapID(uint mapid,bool isBefore)
        {
            //FE8はINDEX
            uint p;
            if (isBefore)
            {
                uint wmapid = MapSettingForm.GetWorldMapEventIDWhereMapID(mapid);
                if (wmapid == 0)
                {//存在しない
                    return U.NOT_FOUND;
                }

                InputFormRef InputFormRef = N_Init(null);
                p = InputFormRef.IDToAddr(wmapid);
            }
            else
            {
                uint wmapid = mapid;
                if (wmapid == 0)
                {//存在しない
                    return U.NOT_FOUND;
                }

                InputFormRef InputFormRef = Init(null);
                p = InputFormRef.IDToAddr(wmapid);
            }
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
            U.SelectedIndexSafety(this.AddressList, value);
            U.SelectedIndexSafety(this.N_AddressList, value);
        }

        public static List<U.AddrResult> MakeEventScriptPointer()
        {
            List<U.AddrResult> list = new List<U.AddrResult>();
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint event_addr = Program.ROM.p32(addr);
                if (U.isSafetyOffset(event_addr))
                {
                    list.Add(new U.AddrResult(
                        event_addr
                        , i.ToString()
                        , 0
                    ));
                }

                addr += InputFormRef.BlockSize;
            }


            InputFormRef N_InputFormRef = N_Init(null);
            addr = N_InputFormRef.BaseAddress;
            for (int i = 0; i < N_InputFormRef.DataCount; i++)
            {
                uint event_addr = Program.ROM.p32(addr);
                if (U.isSafetyOffset(event_addr))
                {
                    list.Add(new U.AddrResult(
                        event_addr
                        , i.ToString()
                        , 1
                    ));
                }

                addr += N_InputFormRef.BlockSize;
            }
            return list;
        }

        private void EventWriteButton_Click(object sender, EventArgs e)
        {
            Program.ROM.write_p32(Program.ROM.RomInfo.oping_event_pointer(), (uint)OPNING_EVENT.Value);
            Program.ROM.write_p32(Program.ROM.RomInfo.ending1_event_pointer(), (uint)ENDING1_EVENT.Value);
            Program.ROM.write_p32(Program.ROM.RomInfo.ending2_event_pointer(), (uint)ENDING2_EVENT.Value);
            InputFormRef.ShowWriteNotifyAnimation(this, Program.ROM.RomInfo.oping_event_pointer());
        }

        private void JUMP_OPNING_EVENT_Click(object sender, EventArgs e)
        {
            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>();
            f.JumpTo((uint)OPNING_EVENT.Value);
        }

        private void JUMP_ENDING1_EVENT_Click(object sender, EventArgs e)
        {
            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>();
            f.JumpTo((uint)ENDING1_EVENT.Value);
        }

        private void JUMP_ENDING2_EVENT_Click(object sender, EventArgs e)
        {
            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>();
            f.JumpTo((uint)ENDING2_EVENT.Value);
        }

        public static bool isWorldMapEvent(uint addr)
        {
            if (Program.ROM.RomInfo.version() == 7)
            {
                return WorldMapEventPointerFE7Form.isWorldMapEvent(addr);
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                return WorldMapEventPointerFE6Form.isWorldMapEvent(addr);
            }

            addr = U.toOffset(addr);
            {
                InputFormRef InputFormRef = Init(null);
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
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "WorldMapEvent Before", new uint[] { 0});

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    string name = "WorldMapEvent Before " + U.To0xHexString((uint)i);

                    EventScriptForm.ScanScript(list, p, true, true, name, tracelist);
                }
            }
            {
                InputFormRef InputFormRef = N_Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "WorldMapEvent After", new uint[] { 0 });

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    string name = "WorldMapEvent After " + U.To0xHexString((uint)i);
                    EventScriptForm.ScanScript(list, p, true, true, name, tracelist);
                }
            }
            {
                uint p = Program.ROM.RomInfo.oping_event_pointer();
                string name = R._("オープニングイベント");
                EventScriptForm.ScanScript( list, p, true, true , name , tracelist);
            }
            {
                uint p = Program.ROM.RomInfo.ending1_event_pointer();
                string name = R._("エイリークエンディング");
                EventScriptForm.ScanScript( list, p, true, true, name , tracelist);
            }
            {
                uint p = Program.ROM.RomInfo.ending2_event_pointer();
                string name = R._("エフラムエンディング");
                EventScriptForm.ScanScript( list, p, true, true, name , tracelist);
            }
        }

        //テキストIDの取得
        public static void MakeTextIDArray(List<UseTextID> list)
        {
            List<uint> tracelist = new List<uint>();
            {
                InputFormRef InputFormRef = Init(null);

                string basename = "WorldMapEvent Before ";

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    string name = basename + U.To0xHexString((uint)i);
                    EventCondForm.MakeTextIDArrayByEventPointer(list, p, name, tracelist);
                }
            }

            //FE8だけ、フリーマップがあるので複雑なイベントが設定されています.
            {
                InputFormRef InputFormRef = N_Init(null);

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    string name = "WorldMapEvent After " + U.To0xHexString((uint)i);
                    EventCondForm.MakeTextIDArrayByEventPointer(list, p, name, tracelist);
                }
            }
            {
                uint p = Program.ROM.RomInfo.oping_event_pointer();
                string name = R._("オープニングイベント");
                EventCondForm.MakeTextIDArrayByEventPointer(list, p, name, tracelist);
            }
            {
                uint p = Program.ROM.RomInfo.ending1_event_pointer();
                string name = R._("エイリークエンディング");
                EventCondForm.MakeTextIDArrayByEventPointer(list, p, name, tracelist);
            }
            {
                uint p = Program.ROM.RomInfo.ending2_event_pointer();
                string name = R._("エフラムエンディング");
                EventCondForm.MakeTextIDArrayByEventPointer(list, p, name, tracelist);
            }
        }

        private void X_JUMP_ROAD_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<WorldMapPathForm>();
        }

        private void X_JUMP_WORLDMAP_POINT_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<WorldMapPointForm>();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.AddressList.Focused)
            {
                U.SelectedIndexSafety(this.N_AddressList , this.AddressList.SelectedIndex);
            }
        }

        private void N_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.N_AddressList.Focused)
            {
                U.SelectedIndexSafety(this.AddressList, this.N_AddressList.SelectedIndex);
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
            //FE8はINDEX
            uint p;
            {
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
                    FELint.CheckEventPointer(event_addr, errors, FELint.Type.WORLDMAP_EVENT, p, true, tracelist);
                }
            }
            {
                InputFormRef InputFormRef = Init(null);
                p = InputFormRef.IDToAddr(wmapid);
                if (p == U.NOT_FOUND)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_WORLDMAP,U.NOT_FOUND
                        ,R._("対応するワールドマップイベント({0})が存在しません。",U.To0xHexString(wmapid) )));
                }
                else
                {
                    uint event_addr = Program.ROM.u32(p);
                    FELint.CheckEventPointer(event_addr, errors, FELint.Type.WORLDMAP_EVENT, p, true, tracelist);
                }
            }
        }

        private void WorldMapEventPointerForm_Load(object sender, EventArgs e)
        {
            //拡張ボタンを表示するかどうか
            uint node_count = WorldMapPointForm.GetDataCount();
            if (WorldMapPointForm.IsShowWorldmapPointExetdns((int)node_count))
            {
                AddressListExpandsButton_255.Show();
                N_AddressListExpandsButton_255.Show();
            }
            else
            {
                this.AddressList.Height += AddressListExpandsButton_255.Height;
                AddressListExpandsButton_255.Hide();

                this.N_AddressList.Height += N_AddressListExpandsButton_255.Height;
                N_AddressListExpandsButton_255.Hide();
            }
        }
    }
}
