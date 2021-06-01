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

            MapPictureBox.HideCommandBar2();

            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.AddressListExpandsEvent += N_AddressListExpandsEvent;
            this.N_InputFormRef.PostWriteHandler += N_PostWriteEvent;

            InputFormRef.markupJumpLabel(X_MAPEDITOR_LINK);
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
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr) ;
                }
                );
        }

        private void MapExitPointForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = true;

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
                this.N_AddressListExpandsButton_80.Enabled = false;
                return;
            }

            this.N_InputFormRef.ReInit(change_addr);
        }

        bool SelectMapID(uint mapid)
        {
            MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID((uint)mapid);
            if (plist.mapchange_plist == 0)
            {//タイルチェンジが設定されていない
                return false;
            }
            uint change_plist_addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CHANGE , plist.mapchange_plist);

            uint selected = InputFormRef.AddrToSelect(this.AddressList, change_plist_addr);
            if (selected == U.NOT_FOUND)
            {
                return false;
            }

            U.SelectedIndexSafety(this.AddressList, selected);
            return true;
        }

        public void JumpToMAPID(uint mapid)
        {
            SelectMapID(mapid);
        }
        public void JumpToMAPIDAndAddr(uint mapid,uint addr)
        {
            bool r = SelectMapID(mapid);
            if (!r)
            {//とりあえず最初のマップを選択しておく.
                U.SelectedIndexSafety(this.AddressList, 0);
                return;
            }

            uint id = this.N_InputFormRef.AddrToID(addr);
            if (id == U.NOT_FOUND)
            {
                return;
            }
            U.SelectedIndexSafety(N_AddressList, id);
        }
        public void JumpToMAPIDAndID(uint mapid, uint id)
        {
            SelectMapID(mapid);
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
            if (!U.isSafetyPointer(change_address))
            {
                if (change_address == 0)
                {
                    N_J_8.ErrorMessage = R._("データが設定されていません。マップエディタから、データを作成してください。");
                }
                else
                {
                    N_J_8.ErrorMessage = R._("ポインタが正しくありません");
                }
            }
            else
            {
                N_J_8.ErrorMessage = "";
            }
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
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            uint mapid = ar.tag;

            MapEditorForm f = (MapEditorForm)InputFormRef.JumpForm<MapEditorForm>(U.NOT_FOUND);
            f.JumpTo(mapid, (uint)N_B0.Value);
        }
        //リストが拡張されたとき
        void N_AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            //
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "ClearTileChangePointer");
            addr = addr + ((eearg.OldDataCount - 1) * eearg.BlockSize);            //-1は id は 0x00から始まるため
            for (int i = (int)eearg.OldDataCount - 1; i < count; i++)
            {
                //タイルチェンジID
                Program.ROM.write_u8(addr + 0, (uint)i, undodata);
                //タイル変化ポインタを0にする.
                Program.ROM.write_u32(addr + 8, 0, undodata);

                addr += eearg.BlockSize;
            }

            Program.Undo.Push(undodata);

            U.ReSelectList(this.AddressList);
            NotifyMapEditor();
        }
        void N_PostWriteEvent(object sender, EventArgs arg)
        {
            NotifyMapEditor();
        }
        void NotifyMapEditor()
        {
            Form f = InputFormRef.GetForm<MapEditorForm>();
            if (f == null)
            {
                return;
            }
            uint mapid = (uint)AddressList.SelectedIndex;

            MapEditorForm mapeditor = (MapEditorForm)f;
            mapeditor.OnUpdateMapChangeForm(mapid);
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

            if (newdatasize < olddatasize)
            {//新しく確保するテーブル数が小さい場合
             //特に何もしない.
                return true;
            }

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

                uint addr = N_InputFormRef.BaseAddress;
                for (int i = 0; i < N_InputFormRef.DataCount; i++, addr += N_InputFormRef.BlockSize)
                {
                    uint w = Program.ROM.u8((uint)addr + 3); //サイズw
                    uint h = Program.ROM.u8((uint)addr + 4); //サイズh
                    uint change_mar = Program.ROM.p32((uint)addr + 8); //変化ポインタ
                    if (!U.isSafetyOffset(change_mar))
                    {
                        continue;
                    }
                    name = "MapChange map:" + U.To0xHexString(mapid) + " n:" + U.To0xHexString(i);
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
                        , R._("マップ変化({0})のIDが重複しています", U.To0xHexString(number))));
                    continue;
                }
                if (number > 0x7f )
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                        , R._("マップ変化({0})のIDが、0x7Fを超えています。\r\nマップ変化IDは、0x7F以下でなければなりません。", U.To0xHexString(number))));
                    continue;
                }
                uint x = Program.ROM.u8(1 + addr);
                uint y = Program.ROM.u8(2 + addr);
                uint width = Program.ROM.u8(3 + addr);
                uint height = Program.ROM.u8(4 + addr);
                uint pointer = Program.ROM.u32(8 + addr);

                if (pointer == 0)
                {//マップ変化ポインタ 0 を容認する.
                    continue;
                }
                if (! U.isSafetyPointer(pointer))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                        , R._("マップ変化({0})のポインタ({1})が危険です。", U.To0xHexString(number), U.To0xHexString(pointer))));
                    continue;
                }

                //ポインタ先の検証.
                uint mapAddr = U.toOffset(pointer);
                uint limitAddr = mapAddr + (2 * width * height);
                if (!U.isSafetyOffset(limitAddr - 1))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                        , R._("マップ変化({0})のポインタ({1})か、マップ変化のサイズの指定が正しくありません。", U.To0xHexString(number), U.To0xHexString(pointer))));
                    continue;
                }

                for (; mapAddr < limitAddr; mapAddr += 2 )
                {
                    int mapData = (int)Program.ROM.u16(mapAddr);
                    if (!ImageUtilMap.IsCorrectMapChip(mapData, configUZ, isFE6))
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.MAPCHANGE, addr
                            , R._("マップ変化({0})のポインタ({1})先のデータで、不正なタイルデータ({2})。", U.To0xHexString(number), U.To0xHexString(pointer), U.To0xHexString(mapData))));
                        break;
                    }
                }
            }
        }

        public static void MakeFlagIDArray(uint mapid,List<UseFlagID> list)
        {
            uint pointer;
            uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid, out pointer);
            if (change_addr == U.NOT_FOUND)
            {
                return;
            }

            InputFormRef N_InputFormRef = N_Init(null);
            N_InputFormRef.ReInitPointer(pointer);
            UseFlagID.AppendFlagIDFixedMapID(list, FELint.Type.MAPCHANGE, N_InputFormRef, 5, mapid);
        }

        public static uint CheckDuplicateMapChangeID(uint mapid , int current_no)
        {
            List<ChangeSt> changeList = new List<ChangeSt>();
            uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid);
            if (change_addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            InputFormRef N_InputFormRef = N_Init(null);
            N_InputFormRef.ReInit(change_addr);
            if (current_no >= N_InputFormRef.DataCount)
            {
                return U.NOT_FOUND;
            }

            change_addr = change_addr + (uint)(current_no * N_InputFormRef.BlockSize);
            if (!U.isSafetyOffset(change_addr))
            {
                return U.NOT_FOUND;
            }

            uint no = Program.ROM.u8(change_addr + 0);
            if (CheckDuplicateMapChangeIDLow(mapid, N_InputFormRef, no, change_addr))
            {
                return MakeNewUniqeMapChangeID(mapid, N_InputFormRef, change_addr);
            }

            return U.NOT_FOUND;
        }
        static bool CheckDuplicateMapChangeIDLow(uint mapid
            , InputFormRef N_InputFormRef , uint current_no , uint current_addr)
        {
            uint change_addr = N_InputFormRef.BaseAddress;
            for (int i = 0; i < N_InputFormRef.DataCount; i++)
            {
                if (change_addr != current_addr)
                {
                    uint no = Program.ROM.u8(change_addr + 0);
                    if (no == current_no)
                    {
                        return true;
                    }
                }

                change_addr += N_InputFormRef.BlockSize;
            }
            return false;
        }
        static uint MakeNewUniqeMapChangeID(uint mapid, InputFormRef N_InputFormRef, uint current_addr)
        {
            for (uint findid = 0; findid < 0x7f; findid++)
            {
                bool isDup = false;
                uint change_addr = N_InputFormRef.BaseAddress;
                for (int i = 0; i < N_InputFormRef.DataCount; i++)
                {
                    if (change_addr == current_addr)
                    {
                        continue;
                    }
                    uint no = Program.ROM.u8(change_addr + 0);
                    if (no == findid)
                    {
                        isDup = true;
                        break;
                    }

                    change_addr += N_InputFormRef.BlockSize;
                }

                if (isDup == false)
                {
                    return findid;
                }
            }
            //判別不能
            return 0x0;
        }


        private void X_MAPEDITOR_LINK_Click(object sender, EventArgs e)
        {
            X_JUMP_MAPEDITOR_Click(sender, e);
        }

        //マップエディタからの変更通知
        public void OnUpdateMapEditorForm(uint mapid)
        {
            uint current_mapid = (uint)this.AddressList.SelectedIndex;
            if (mapid != current_mapid)
            {//現在変更しているマップではない
                return;
            }
            //強制再読み込み
            U.ReSelectList(this.AddressList);
        }

        //名前は、IDでよく参照されるのでキャッシュする
        static Dictionary<uint , string> g_MapChangeName_Cache = new Dictionary<uint,string>();
        public static void ClearCache()
        {
            g_MapChangeName_Cache = new Dictionary<uint, string>();
        }
        public static string GetName(uint mapid, uint changeid)
        {
            string ret;
            uint key = (mapid * 256) + changeid;
            if (g_MapChangeName_Cache.TryGetValue(key, out ret))
            {
                return ret;
            }

            ret = GetNameLow(mapid, changeid);
            g_MapChangeName_Cache[key] = ret;
            return ret;
        }

        static string GetNameLow(uint mapid, uint changeid)
        {
            uint pointer;
            uint change_addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid, out pointer);
            if (change_addr == U.NOT_FOUND)
            {
                return "";
            }

            InputFormRef N_InputFormRef = N_Init(null);
            N_InputFormRef.ReInitPointer(pointer);
            for (int i = 0; i < N_InputFormRef.DataCount; i++)
            {
                uint no = Program.ROM.u8(change_addr + 0);
                if (no == changeid)
                {
                    return Program.CommentCache.At(change_addr);
                }
                change_addr += N_InputFormRef.BlockSize;
            }
            return "";
        }
        //複数個所で参照されているタイル変化IDかどうか求める
        public static uint CountRefence(uint addr, List<Address> list)
        {
            uint count = 0;
            addr = U.toOffset(addr);

            foreach (Address a in list)
            {
                if (a.Addr == addr)
                {
                    count++;
                }
            }
            return count;
        }

        //複数個所で参照されているタイル変化IDかどうか求める
        public static uint SearchSameData(byte[] data, List<Address> list)
        {
            foreach (Address a in list)
            {
                if (a.DataType != Address.DataTypeEnum.BIN)
                {
                    continue;
                }
                if (a.Length != data.Length)
                {
                    continue;
                }
                byte[] bin = Program.ROM.getBinaryData(a.Addr, a.Length);
                if (U.memcmp(data, bin) != 0)
                {
                    continue;
                }
                return a.Addr;
            }
            return U.NOT_FOUND;
        }
    }
}
