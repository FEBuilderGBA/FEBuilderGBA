using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapChangeForm : Form
    {
        public MapChangeForm()
        {
            InitializeComponent();

            this.N_InputFormRef = N_Init(this);
            this.InputFormRef = Init(this);

            //マップを最前面に移動する.
            MapPictureBox.BringToFront();

            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.AddressListExpandsEvent += N_AddressListExpandsEvent;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.map_setting_pointer()
                , Program.ROM.RomInfo.map_setting_datasize()
                , (int i, uint addr) =>
                {
                    //0 がポインタであればデータがあると考える.
                    return U.isPointer(Program.ROM.u32(addr + 0));
                }
                , (int i, uint addr) =>
                {
                    MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID((uint)i);
                    uint change_plist_addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CHANGE , plist.mapchange_plist);
                    string name = MapSettingForm.GetMapName((uint)i);

                    return new U.AddrResult(change_plist_addr, name, (uint)i);
                }
                );
        }

        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , 0
                , 12
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
        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            U.AddrResult  ar = InputFormRef.SelectToAddrResult(this.AddressList);
            uint mapid = ar.tag;
            MapPictureBox.LoadMap(mapid);
            this.MapPictureBox.ClearAllPoint();

            uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(change_addr))
            {
                this.N_InputFormRef.ClearSelect(true);
                this.N_AddressListExpandsButton.Enabled = false;
                return;
            }

            this.N_InputFormRef.ReInit(change_addr);
        }

        void SelectMapID(uint mapid)
        {
            MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID((uint)mapid);
            uint change_plist_addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CHANGE , plist.mapchange_plist);

            uint selected = InputFormRef.AddrToSelect(this.AddressList, change_plist_addr);
            if (selected == U.NOT_FOUND)
            {
                return;
            }

            U.SelectedIndexSafety(this.AddressList, selected);
        }

        public void JumpToMAPID(uint mapid)
        {
            SelectMapID(mapid);
        }
        public void JumpToMAPIDAndAddr(uint mapid,uint addr)
        {
            SelectMapID(mapid);

            uint id = this.N_InputFormRef.AddrToID(addr);
            if (id == U.NOT_FOUND)
            {
                return;
            }
            U.SelectedIndexSafety(N_AddressList, id);
        }

        private void N_P8_ValueChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef != null && this.InputFormRef.IsUpdateLock)
            {
                return;
            }

            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            uint mapid = ar.tag;
            int width = (int)N_B3.Value;
            int height = (int)N_B4.Value;
            uint change_address = (uint)N_P8.Value;
            MapPictureBox.SetDefualtIcon(
                MapSettingForm.DrawMapChange(mapid, width, height, change_address)
            );
        }

        public class ChangeSt
        {
            public uint no;
            public uint x;
            public uint y;
            public uint width;
            public uint height;
            public uint addr;
            public uint self_change_addr;
        };
        public static List<ChangeSt> MakeChangeList(uint mapid)
        {
            List<ChangeSt> changeList = new List<ChangeSt>();

            uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid );
            if (change_addr == U.NOT_FOUND)
            {
                return changeList;
            }

            InputFormRef N_InputFormRef = N_Init(null);
            N_InputFormRef.ReInit(change_addr);
            for (int i = 0 ; i < N_InputFormRef.DataCount ; i++)
            {
                ChangeSt p = new ChangeSt();
                p.no = Program.ROM.u8(change_addr+0);
                p.x = Program.ROM.u8(change_addr+1);
                p.y = Program.ROM.u8(change_addr+2);
                p.width = Program.ROM.u8(change_addr+3);
                p.height = Program.ROM.u8(change_addr+4);
                p.addr = Program.ROM.p32(change_addr + 8);
                p.self_change_addr = change_addr;
                changeList.Add(p);

                change_addr += N_InputFormRef.BlockSize;
            }

            return changeList;
        }
        public static ChangeSt MakeChangeOne(uint mapid,uint changeid)
        {
            List<ChangeSt> list = MakeChangeList(mapid);
            uint len = (uint)list.Count;
            if (changeid >= len)
            {
                return new ChangeSt();
            }
            return list[(int)changeid];
        }
        public static bool Write_OneData(ChangeSt changest,Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(changest.self_change_addr, 12));

            Program.ROM.write_u8(changest.self_change_addr + 0, changest.no);
            Program.ROM.write_u8(changest.self_change_addr + 1, changest.x);
            Program.ROM.write_u8(changest.self_change_addr + 2, changest.y);
            Program.ROM.write_u8(changest.self_change_addr + 3, changest.width);
            Program.ROM.write_u8(changest.self_change_addr + 4, changest.height);
            Program.ROM.write_p32(changest.self_change_addr + 8, changest.addr);

            return true;
        }

        private void N_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            N_P8_ValueChanged(null, null);
        }

        private void X_JUMP_MAPEDITOR_Click(object sender, EventArgs e)
        {
            if (AddressList.SelectedIndex < 0)
            {
                return;
            }
            if (N_AddressList.SelectedIndex < 0)
            {
                return;
            }

            MapEditorForm f = (MapEditorForm)InputFormRef.JumpForm<MapEditorForm>(U.NOT_FOUND);
            f.JumpTo((uint)AddressList.SelectedIndex, (uint)N_B0.Value);
        }
        //リストが拡張されたとき
        void N_AddressListExpandsEvent(object sender, EventArgs arg)
        {
            U.ReSelectList(this.AddressList);
        }

        //マップ変化のベース領域がなければ割り当てる
        public static bool PreciseChangeList(uint mapid)
        {
            List<ChangeSt> changeList = new List<ChangeSt>();

            uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid);
            if (U.isSafetyOffset(change_addr) )
            {
                return true;
            }

            return PreciseChangeListForce(mapid) != 0;
        }
        //マップ変化のベース領域の強制割り当て
        public static uint PreciseChangeListForce(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.CHANGE);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise MapChange", mapid.ToString("X"));

            byte[] data = new byte[12];
            data[0] = 0xFF; //終端.
            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.CHANGE, plist, write_addr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            r = MapSettingForm.WriteMapChangePlist(mapid, plist, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            Program.Undo.Push(undodata);

            return plist;
        }

        //プログラムからのデータ拡張
        public static bool ResizeChangeList(uint mapid,uint newcount)
        {
            List<ChangeSt> changeList = new List<ChangeSt>();

            uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid);
            if (change_addr == U.NOT_FOUND)
            {
                return false;
            }

            InputFormRef N_InputFormRef = N_Init(null);
            N_InputFormRef.ReInit(change_addr);

            uint olddatasize = N_InputFormRef.DataCount * N_InputFormRef.BlockSize;
            uint newdatasize = (newcount+1) * N_InputFormRef.BlockSize;

            //既存データの取得
            byte[] oldDataByte = Program.ROM.getBinaryData(change_addr, olddatasize);
            //新規
            byte[] newDataByte = new byte[newdatasize];
            Array.Copy(oldDataByte, newDataByte, olddatasize);

            //新規に確保した領域にindex番号等を振る.
            int i = (int)N_InputFormRef.DataCount;
            for (; i < newcount; i++)
            {
                uint offset = (uint)(i * N_InputFormRef.BlockSize);
                U.write_u8(newDataByte, (uint)offset + 0, (uint)i);
                U.write_u8(newDataByte, (uint)offset + 3, 1); //サイズw
                U.write_u8(newDataByte, (uint)offset + 4, 1); //サイズh
            }
            //終端データ(newdata+1で確保しているため安全)
            U.write_u8(newDataByte, (uint)(i * N_InputFormRef.BlockSize), (uint)0xff);

            Undo.UndoData undodata = Program.Undo.NewUndoData("Resize MapChange", mapid.ToString("X"), newcount.ToString("X") );
            
            //新規領域の確保
            uint newaddr = InputFormRef.AppendBinaryData(newDataByte, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return false;
            }

            //PLISTのポインタを書き換える.
            MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(mapid);
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.CHANGE, plist.mapchange_plist, newaddr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return false;
            }

            Program.Undo.Push(undodata);

            return true;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef N_InputFormRef = N_Init(null);
            
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                uint pointer;
                uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid, out pointer);
                if (change_addr == U.NOT_FOUND)
                {
                    continue;
                }

                //N_InputFormRef.ReInit(change_addr);
                N_InputFormRef.ReInitPointer(pointer);

                string name = "MapChange map:" + U.To0xHexString(mapid) ;
                FEBuilderGBA.Address.AddAddress(list, N_InputFormRef, name, new uint[] { 8 });

                List<U.AddrResult> arlist = N_InputFormRef.MakeList();
                for (int n = 0; n < arlist.Count; n++)
                {
                    uint addr = arlist[n].addr;
                    uint w = Program.ROM.u8((uint)addr + 3); //サイズw
                    uint h = Program.ROM.u8((uint)addr + 4); //サイズh
                    uint change_mar = Program.ROM.p32((uint)addr + 8); //変化ポインタ
                    if (!U.isSafetyOffset(change_mar))
                    {
                        continue;
                    }
                    name = "MapChange map:" + U.To0xHexString(mapid) + " n:" + U.To0xHexString(n);
                    FEBuilderGBA.Address.AddAddress(list,change_mar
                        , w * h * 2
                        , addr + 8
                        , name
                        , Address.DataTypeEnum.BIN);
                }
            }
        }


        public static void MakeCheckError(uint mapid,List<FELint.ErrorSt> errors)
        {
            InputFormRef N_InputFormRef = N_Init(null);

            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereMapID(mapid);
            if (plists.mapchange_plist == 0)
            {//マップ変化がない
                return;
            }

            uint change_addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CHANGE, plists.mapchange_plist);
            if (!U.isSafetyOffset(change_addr))
            {//マップ設定の方でトラップするので不要.
                return;
            }
            byte[] configUZ = ImageUtilMap.UnLZ77ChipsetData(plists.config_plist);
            if (configUZ == null)
            {//マップ設定の方でトラップするので不要.
                return;
            }

            //マップサイズ
            int mapwidth;
            int mapheight;
            ImageUtilMap.UnLZ77MapDataUShort(plists.mappointer_plist, out mapwidth, out mapheight);

            bool isFE6 = Program.ROM.RomInfo.version() == 6;

            N_InputFormRef.ReInit(change_addr);

            List<uint> useNumber = new List<uint>();
            uint addr = N_InputFormRef.BaseAddress;
            for (int i = 0; i < N_InputFormRef.DataCount; i++, addr += N_InputFormRef.BlockSize)
            {
                uint number = Program.ROM.u8(0 + addr);
                if (useNumber.IndexOf(number) >= 0)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                        , R._("マップ変化({0})の番号が重複しています", number)));
                    continue;
                }
                uint x = Program.ROM.u8(1 + addr);
                uint y = Program.ROM.u8(2 + addr);
                uint width = Program.ROM.u8(3 + addr);
                uint height = Program.ROM.u8(4 + addr);
                uint pointer = Program.ROM.u32(8 + addr);

//マップ変化を使いまわしているところが軒並みエラーになるのでやめておこう...
//                if (x + width > mapwidth)
//                {
//                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
//                        , R._("マップ変化({0})の幅(X:{1} Width:{2})は、マップの幅({3})より大きいです", number, x , width, mapwidth)));
//                }
//                if (y + height > mapheight)
//                {
//                    if (isFE6 && mapid == 0x28 && addr == 0x687A94)
//                    {
//                        //FE6にはマップ変化リストを間違えて指定している場所があるので無視する.
//                        continue;
//                    }
//                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
//                        , R._("マップ変化({0})の高さ(Y:{1} Height:{2})は、マップの高さ({3})より大きいです", number, y, height, mapheight)));
//                }

                if (pointer == 0)
                {//マップ変化ポインタ 0 を容認する.
                    continue;
                }
                if (! U.isSafetyPointer(pointer))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                        , R._("マップ変化({0})のポインタ({1})が危険です。", number, U.To0xHexString(pointer))));
                    continue;
                }

                //ポインタ先の検証.
                uint mapAddr = U.toOffset(pointer);
                uint limitAddr = mapAddr + (2 * width * height);
                if (!U.isSafetyOffset(limitAddr - 1))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                        , R._("マップ変化({0})のポインタ({1})か、マップ変化のサイズの指定が正しくありません。", number, U.To0xHexString(pointer))));
                    continue;
                }

                for (; mapAddr < limitAddr; mapAddr += 2 )
                {
                    int mapData = (int)Program.ROM.u16(mapAddr);
                    if (!ImageUtilMap.IsCorrectMapChip(mapData, configUZ, isFE6))
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                            , R._("マップ変化({0})のポインタ({1})先のデータで、不正なタイルデータ({2})。", number, U.To0xHexString(pointer), U.To0xHexString(mapData))));
                        break;
                    }
                }
            }
        }


    }
}
