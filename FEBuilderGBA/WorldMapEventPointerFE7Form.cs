﻿using System;
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

            ENDING1_EVENT.Value = Program.ROM.p32(Program.ROM.RomInfo.ending1_event_pointer);
            ENDING2_EVENT.Value = Program.ROM.p32(Program.ROM.RomInfo.ending2_event_pointer);
        }


        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.worldmap_event_on_stageselect_pointer
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
            //章拡張を表示するかどうか
            if (MapSettingForm.IsShowChapterExetdns(this.N_AddressList.Items.Count))
            {
                N_AddressListExpandsButton_255.Show();
            }
            else
            {
                this.N_AddressList.Height += N_AddressListExpandsButton_255.Height;
                N_AddressListExpandsButton_255.Hide();
            }
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
            U.SelectedIndexSafety(N_AddressList, value);
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
            {
                uint p = Program.ROM.RomInfo.ending1_event_pointer;
                string name = R._("エリウッドエンディング");
                EventScriptForm.ScanScript(list, p, true, true, name, tracelist);
            }
            {
                uint p = Program.ROM.RomInfo.ending2_event_pointer;
                string name = R._("ヘクトルエンディング");
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
                FELint.CheckEventPointer(event_addr, errors, FELint.Type.WORLDMAP_EVENT, p, true, tracelist);
            }

        }
        //テキストIDの取得
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            List<uint> tracelist = new List<uint>();
            {
                InputFormRef ifr = N_Init(null);

                string basename;
                basename = "WorldMapEvent ";

                uint p = ifr.BaseAddress;
                for (int i = 0; i < ifr.DataCount; i++, p += ifr.BlockSize)
                {
                    string name = basename + U.To0xHexString((uint)i);
                    EventCondForm.MakeVarsIDArrayByEventPointer(list, p, name, tracelist);
                }
            }
            {
                uint p = Program.ROM.RomInfo.ending1_event_pointer;
                string name = R._("エリウッドエンディング");
                EventCondForm.MakeVarsIDArrayByEventPointer(list, p, name, tracelist);
            }
            {
                uint p = Program.ROM.RomInfo.ending2_event_pointer;
                string name = R._("ヘクトルエンディング");
                EventCondForm.MakeVarsIDArrayByEventPointer(list, p, name, tracelist);
            }
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

        private void EventWriteButton_Click(object sender, EventArgs e)
        {
            Program.ROM.write_p32(Program.ROM.RomInfo.ending1_event_pointer, (uint)ENDING1_EVENT.Value);
            Program.ROM.write_p32(Program.ROM.RomInfo.ending2_event_pointer, (uint)ENDING2_EVENT.Value);
            InputFormRef.ShowWriteNotifyAnimation(this,U.NOT_FOUND);
        }
    }
}
