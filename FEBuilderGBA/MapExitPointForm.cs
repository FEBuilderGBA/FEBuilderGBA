using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapExitPointForm : Form
    {
        public MapExitPointForm()
        {
            InitializeComponent();

            this.N_InputFormRef = N_Init(this);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.CheckProtectionAddrHigh = false;
            this.InputFormRef = Init(this);
            this.InputFormRef.CheckProtectionAddrHigh = false;

            //マップを最前面に移動する.
            MapPictureBox.BringToFront();

            if (Program.ROM.RomInfo.version() == 6)
            {//たぶんFE6には、NPC離脱ポインタは存在しない
                FilterComboBox.Hide();
            }
            else
            {
                FilterComboBox.SelectedIndex = 0;
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.map_exit_point_pointer() 
                , 4
                , (int i, uint addr) =>
                {
                    uint p = Program.ROM.u32(addr);
                    if (!U.isPointerOrNULL(p))
                    {
                        return false;
                    }

                    return i < Program.ROM.RomInfo.map_exit_point_npc_blockadd();
                }
                , (int i, uint addr) =>
                {
                    return MapSettingForm.GetMapName((uint)i);
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
                {
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
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

            MapPictureBox.LoadMap(mapid);

            this.MapPictureBox.ClearAllPoint();
            this.InputFormRef.ReInit(addr);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint exit_point_addrp  = InputFormRef.SelectToAddr(this.AddressList);
            if (!U.isSafetyOffset(exit_point_addrp))
            {
                this.N_InputFormRef.ReInit(0);
                return;
            }

            MapPictureBox.LoadMap((uint)this.AddressList.SelectedIndex);

            this.MapPictureBox.ClearAllPoint();

            uint exit_point_addr = Program.ROM.u32(exit_point_addrp);
            if (!U.isPointer(exit_point_addr))
            {
                this.N_InputFormRef.ReInit(0);
                return;
            }

            exit_point_addr = U.toOffset(exit_point_addr);
            if (!U.isSafetyOffset(exit_point_addrp))
            {
                this.N_InputFormRef.ReInit(0);
                return;
            }

            if (exit_point_addr == Program.ROM.RomInfo.map_exit_point_blank())
            {//一つも離脱ポインタがない 共通NULLマーク
                N_AddressListExpandsButton.Hide();
                NewListAlloc.Show();
            }
            else
            {
                NewListAlloc.Hide();
                N_AddressListExpandsButton.Show();
            }

            this.N_InputFormRef.ReInit(exit_point_addr);
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.FilterComboBox.SelectedIndex == 0)
            {//自軍
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.map_exit_point_pointer()));

            }
            else
            {//友軍
                this.InputFormRef.ReInit(
                    Program.ROM.p32(Program.ROM.RomInfo.map_exit_point_pointer()) + (4 * Program.ROM.RomInfo.map_exit_point_npc_blockadd()));
                    ;
            }
        }
        public static void MakeCheckError(uint mapid, List<FELint.ErrorSt> errors)
        {
            List<U.AddrResult> list = MakeList(mapid);//効率は悪いが理解しやすさからマップ単位で見ましょう.
            if (list.Count >= 0x12)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.MAPEXIT, U.NOT_FOUND
                    , R._("離脱ポイントが大量に存在します({0})。データが壊れていませんか？", list.Count), mapid));
            }
        }

        public static List<U.AddrResult> MakeList(uint mapid)
        {
            InputFormRef InputFormRef = Init(null);
            List<U.AddrResult> map_arlist = InputFormRef.MakeList();
            if (map_arlist.Count <= mapid)
            {
                return new List<U.AddrResult>();
            }

            uint addr = map_arlist[(int)mapid].addr;
            if (! U.isSafetyOffset(addr))
            {
                return new List<U.AddrResult>();
            }
            addr = Program.ROM.p32(addr);
            if (!U.isSafetyOffset(addr))
            {
                return new List<U.AddrResult>();
            }

            InputFormRef N_InputFormRef = N_Init(null);
            N_InputFormRef.ReInit(addr);
            List<U.AddrResult> arlist = N_InputFormRef.MakeList();

            return arlist;
        }
        public void JumpToMAPIDAndAddr(uint mapid, uint exitindex)
        {
            U.SelectedIndexSafety(this.AddressList, mapid);
            U.SelectedIndexSafety(N_AddressList, exitindex, false);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MapExit", new uint[] { 0 });
            
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                uint exit_addr = InputFormRef.IDToAddr(mapid);
                if (exit_addr == U.NOT_FOUND)
                {
                    continue;
                }

                {
                    uint a = Program.ROM.p32(exit_addr);
                    if (a == U.NOT_FOUND)
                    {
                        continue;
                    }

                    InputFormRef N_InputFormRef = N_Init(null);
                    N_InputFormRef.ReInitPointer(exit_addr);

                    string name = "MapExit map:" + U.To0xHexString(mapid);
                    FEBuilderGBA.Address.AddAddress(list, N_InputFormRef, name, new uint[] {  });
                }
            }

            //NPC離脱
            if (Program.ROM.RomInfo.version() == 6)
            {//たぶんFE6には、NPC離脱ポインタは存在しない
                return;
            }
            InputFormRef.ReInit(
                Program.ROM.p32(Program.ROM.RomInfo.map_exit_point_pointer()) + (4 * Program.ROM.RomInfo.map_exit_point_npc_blockadd()));
            FEBuilderGBA.Address.AddAddressButIgnorePointer(list, InputFormRef, "MapExit NPC", new uint[] { 0 });

            mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                uint exit_addr = InputFormRef.IDToAddr(mapid);
                if (exit_addr == U.NOT_FOUND)
                {
                    continue;
                }

                {
                    uint a = Program.ROM.p32(exit_addr);
                    if (a == U.NOT_FOUND)
                    {
                        continue;
                    }

                    InputFormRef N_InputFormRef = N_Init(null);
                    N_InputFormRef.ReInitPointer(exit_addr);

                    string name = "MapExit map:" + U.To0xHexString(mapid) + " NPC";
                    FEBuilderGBA.Address.AddAddress(list, N_InputFormRef, name, new uint[] {  });
                }
            }
        }

        private void NewListAlloc_Click(object sender, EventArgs e)
        {
            uint exit_point_addrp = InputFormRef.SelectToAddr(this.AddressList);
            if (!U.isSafetyOffset(exit_point_addrp))
            {
                return;
            }

            byte[] data = new byte[8];
            data[4] = 0xFF;

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"MapExit NewAlloc");
            uint newaddr = InputFormRef.AppendBinaryData(data, undodata);

            Program.ROM.write_p32(exit_point_addrp, newaddr, undodata);

            Program.Undo.Push(undodata);

            U.ReSelectList(AddressList);
        }


    
    }
}
