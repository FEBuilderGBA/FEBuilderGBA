using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventForceSortieFE7Form : Form
    {
        public EventForceSortieFE7Form()
        {
            InitializeComponent();

            this.N_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.N_InputFormRef = N_Init(this);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef = Init(this);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            //天地の剣の資料より
            //強制出撃は 竜の門からスタート

            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.event_force_sortie_pointer() 
                , 4
                , (int i, uint addr) =>
                {
                    return i < 23;
                }
                , (int i, uint addr) =>
                {
                    return MapSettingForm.GetMapName((uint)i + 0x17);
                }
                );
        }

        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , 0
                , 4
                , (int i, uint addr) =>
                {//00 または D1 で終端
                    uint id = Program.ROM.u8(addr);
                    uint term = Program.ROM.u8(addr+3);
                    return (id != 0x00 && term != 0xD1);
                }
                , (int i, uint addr) =>
                {
                    uint uid = Program.ROM.u8(addr);
                    return U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid);
                }
                );
        }

        private void MapExitPointForm_Load(object sender, EventArgs e)
        {
        }
        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)AddressList.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                return;
            }
            uint addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            U.ForceUpdate(this.ReadStartAddress,addr);

            this.InputFormRef.ReInit(addr);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addrp  = InputFormRef.SelectToAddr(this.AddressList);
            if (!U.isSafetyOffset(addrp))
            {
                this.N_InputFormRef.ReInit(0);
                return;
            }

            uint addr = Program.ROM.u32(addrp);
            if (!U.isPointer(addr))
            {
                this.N_InputFormRef.ReInit(0);
                return;
            }

            addr = U.toOffset(addr);
            this.N_InputFormRef.ReInit(addr);
        }

        public void JumpToMAPIDAndAddr(uint mapid, uint exitindex)
        {
            AddressList.SelectedIndex = (int)mapid;
            U.SelectedIndexSafety(N_AddressList, exitindex, false);
        }

        private void N_AddressListExpandsButton_Click(object sender, EventArgs e)
        {

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "ForceSorite", new uint[]{0});

            uint addrp = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addrp += InputFormRef.BlockSize)
            {
                uint addr = Program.ROM.p32(addrp);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                InputFormRef N_InputFormRef = N_Init(null);
                N_InputFormRef.ReInitPointer(addrp);

                FEBuilderGBA.Address.AddAddress(list,N_InputFormRef , "ForceSorite", new uint[]{} );
            }
        }
    }
}
