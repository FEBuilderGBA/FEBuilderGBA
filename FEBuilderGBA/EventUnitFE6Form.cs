using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventUnitFE6Form : Form
    {
        public EventUnitFE6Form()
        {
            InitializeComponent();

            this.MAP_LISTBOX.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
            this.AddressList.OwnerDraw(EventUnitForm.AddressList_Draw, DrawMode.OwnerDrawVariable);
            this.EVENT_LISTBOX.OwnerDraw(EventUnitForm.EVENT_LISTBOX_Draw, DrawMode.OwnerDrawVariable);

            this.InputFormRef = Init(this);

            //マップIDリストを作る.
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref this.MAP_LISTBOX);

            //AI
            EventUnitForm.AI1ToCombo(L_12_COMBO);
            EventUnitForm.AI2ToCombo(L_13_COMBO);
            EventUnitForm.AI3ToCombo(L_14_AI3_HYOUTEKI);

            //右クリックメニューを出す.
            this.InputFormRef.MakeGeneralAddressListContextMenu(true, true, CustomKeydownHandler);

            this.InputFormRef.PreAddressListExpandsEvent += EventUnitForm.OnPreClassExtendsWarningHandler;
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;

            this.MapPictureBox.MapMouseDownEvent += MapMouseDownEvent;

//            //ユニットID重複チェック
//            //ダメ、重複が許されるケースがあった
//            this.B0.ValueChanged += EventUnitForm_CheckDuplicatePlayerUnits;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(EventUnitFE6Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , Program.ROM.RomInfo.eventunit_data_size()
                , (int i, uint addr) =>
                {//読込最大値検索
                    //00まで検索
                    return Program.ROM.u8(addr) != 0;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = Program.ROM.u8(addr);
                    if (unit_id == 0)
                    {
                        return null;
                    }
                    uint class_id = Program.ROM.u8(addr + 1);
                    uint unitgrow = Program.ROM.u16(addr + 3);
                    if (class_id == 0)
                    {//クラスIDが0だったらユーザIDで補完する
                        class_id = UnitForm.GetClassID(unit_id);
                    }

                    String unit_name = UnitForm.GetUnitName(unit_id);
                    String class_name = ClassForm.GetClassName(class_id);
                    uint level = U.ParseUnitGrowLV(unitgrow);

                    return unit_name + "(" + class_name + ")" + "  " + "Lv:" + level.ToString();
                }
                );
        }

        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                return;
            }
            uint addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            List<U.AddrResult> list = EventCondForm.MakeUnitPointer(mapid);
            U.ConvertListBox(list, ref this.EVENT_LISTBOX);

            //未記帳の拡張した領域があれば追加する.
            EventUnitForm.AppendNoWriteNewData(list, mapid);

            if (this.EVENT_LISTBOX.Items.Count > 0)
            {
                this.EVENT_LISTBOX.SelectedIndex = 0;
            }
            else
            {
                MapPictureBox.LoadMap(mapid);
            }
        }


        private void EVENT_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.EVENT_LISTBOX);
            if (!U.isSafetyOffset(ar.addr))
            {
                this.MapPictureBox.ClearAllPoint();
                return;
            }
            //タグにマップ番号が入っている.
            MapPictureBox.LoadMap(ar.tag);

            this.MapPictureBox.ClearAllPoint();
            this.InputFormRef.ReInit(ar.addr);
        }


        public void JumpToMap(uint mapid)
        {
            U.SelectedIndexSafety(MAP_LISTBOX, mapid);
        }
        public void JumpTo(uint addr, int unitIndex = 0)
        {
            addr = U.toOffset(addr);

            //アドレスからマップとイベントの逆変換
            int mapindex, eventindex;
            if (ConvertAddrToMapAndEvent(0, (uint)MAP_LISTBOX.Items.Count, addr, out mapindex, out eventindex))
            {
                if (mapindex < this.MAP_LISTBOX.Items.Count)
                {
                    this.MAP_LISTBOX.SelectedIndex = mapindex;
                    if (eventindex < this.EVENT_LISTBOX.Items.Count)
                    {
                        this.EVENT_LISTBOX.SelectedIndex = eventindex;

                        if (unitIndex < this.AddressList.Items.Count)
                        {
                            this.AddressList.SelectedIndex = unitIndex;
                        }
                        return;
                    }
                }
            }

            this.MapPictureBox.ClearAllPoint();
            this.InputFormRef.ReInit(addr);
        }

        bool ConvertAddrToMapAndEvent(uint starti,uint endi,uint addr, out int out_mapindex, out int out_eventindex)
        {
            for (uint i = starti; i < endi; i++)
            {
                List<U.AddrResult> eventlist = EventCondForm.MakeUnitPointer(i);
                for (int n = 0; n < eventlist.Count; n++)
                {
                    if (eventlist[n].addr == addr)
                    {//FOUND!
                        out_mapindex = (int)i;
                        out_eventindex = n;
                        return true;
                    }
                }
            }
            out_mapindex = -1;
            out_eventindex = -1;
            return false;
        }

        private void JUMP_BATTLETALK_Click(object sender, EventArgs e)
        {
            EventBattleTalkFE6Form f = (EventBattleTalkFE6Form)InputFormRef.JumpForm<EventBattleTalkFE6Form>(U.NOT_FOUND);
            f.JumpTo((uint)B0.Value);
        }

        private void JUMP_HAIKU_Click(object sender, EventArgs e)
        {
            EventHaikuFE6Form f = (EventHaikuFE6Form)InputFormRef.JumpForm<EventHaikuFE6Form>(U.NOT_FOUND);
            f.JumpTo((uint)B0.Value, (uint)MAP_LISTBOX.SelectedIndex);
        }

        public GrowSimulator BuildSim()
        {
            GrowSimulator sim = new GrowSimulator();
            UnitForm.GetSim(ref sim
                , (uint)B0.Value //ユニット
            );
            ClassForm.GetSim(ref sim
                , (uint)B1.Value //クラス
            );

            return sim;
        }
        

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawAllUnits();
            PosSyncCheck();

            SetNotifyMode();
            CheckCoord();
        }

        void CheckCoord()
        {
            if (!this.MapPictureBox.IsMapLoad())
            {//まだマップを読みこんでいない.
                return;
            }

            int mapwidth = this.MapPictureBox.GetMapBitmapWidth() / 16;
            int mapheight = this.MapPictureBox.GetMapBitmapHeight() / 16;

            if (B4.Value > mapwidth || B5.Value > mapheight)
            {
                this.EVENTUNIT_BEFORE_COORD.ErrorMessage = R._("ユニットの座標({0},{1})は、マップのサイズ({2},{3})より大きい座標です。"
                    , B4.Value, B5.Value, mapwidth, mapheight);
            }
            else
            {
                this.EVENTUNIT_BEFORE_COORD.ErrorMessage = "";
            }

            if (B6.Value > mapwidth || B7.Value > mapheight)
            {
                this.EVENTUNIT_AFTER_COORD.ErrorMessage = R._("ユニットの座標({0},{1})は、マップのサイズ({2},{3})より大きい座標です。"
                    , B6.Value, B7.Value, mapwidth, mapheight);
            }
            else
            {
                this.EVENTUNIT_AFTER_COORD.ErrorMessage = "";
            }
        }

        void SetNotifyMode()
        {
            if (this.MapPictureBox.GetNotifyMode())
            {//既に通知モードなので何もしない.
                return;
            }

            //一度フォーカスを当てて、位置選択状態にする
            Control prevFocusedControl = this.ActiveControl;
            B4.Focus();
            if (prevFocusedControl == null)
            {
                AddressList.Focus();
            }
            else
            {
                prevFocusedControl.Focus();
            }
        }

        void PosSyncCheck()
        {
            if (B4.Value == B6.Value && B5.Value == B7.Value)
            {//配置前と配置後が同一の場合、同時変更オプションを提示
                PosSyncUpdateComboBox.SelectedIndex = 0;
            }
            else
            {
                PosSyncUpdateComboBox.SelectedIndex = 1;
            }
        }

        ToolTipEx X_Tooltip;
        private void EventUnitFE6Form_Load(object sender, EventArgs e)
        {
            X_Tooltip = InputFormRef.GetToolTip<EventUnitFE6Form>();
            this.EVENTUNIT_BEFORE_COORD.SetToolTipEx(this.X_Tooltip);
            this.EVENTUNIT_AFTER_COORD.SetToolTipEx(this.X_Tooltip);
            MapPictureBox.SetDefualtIcon(ImageSystemIconForm.Blank16());
        }
        public static List<U.AddrResult> MakeList(uint addr)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInit(addr);
            return InputFormRef.MakeList();
        }
        public void DrawAllUnits()
        {
            MapPictureBox.ClearStaticItem();
            List<U.AddrResult> list = InputFormRef.MakeList();
            for (int i = 0; i < list.Count; i++)
            {
                if (AddressList.SelectedIndex == i)
                {//選択しているものは別ルーチンで詳細に描画する.
                    DrawSelectedUnit();
                }
                else
                {//選択していないものは、移動後座標だけ描画する.
                    MapPictureBox.StaticItem sitem = EventUnitFE7Form.DrawAfterPosUnit(list[i].addr);
                    MapPictureBox.SetStaticItem("o" + i.ToString(), sitem.x, sitem.y, sitem.bitmap, sitem.draw_x_add, sitem.draw_y_add);
                }
            }
            MapPictureBox.Invalidate();
        }

        public void DrawSelectedUnit()
        {
            uint unit_id = (uint)B0.Value;
            uint class_id = (uint)B1.Value;
            if (class_id == 0)
            {//クラスIDが0だったらユーザIDで補完する
                class_id = UnitForm.GetClassID(unit_id);
            }

            List<MapPictureBox.StaticItem> list =
                EventUnitFE7Form.DrawUnit(
                  class_id
                , (uint)B3.Value
                , (int)B4.Value
                , (int)B5.Value
                , (int)B6.Value
                , (int)B7.Value
                );
            for (int n = list.Count - 1; n >= 0; n--)
            {
                MapPictureBox.SetStaticItem("c" + n.ToString(), list[n].x, list[n].y, list[n].bitmap, list[n].draw_x_add, list[n].draw_y_add);
            }

        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            uint rom_length = (uint)Program.ROM.Data.Length;

            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "FixUnitPlacer");

            //途中にnullが含まれている場合は、補正します.
            for (int i = 0; i < count; i++)
            {
                if (addr + 2 > rom_length)
                {
                    Log.Error("ROM Broken! Address after allocation is out of range. {0}+2/{1}", U.ToHexString8(addr), U.ToHexString8(rom_length));
                    break;
                }
                if (Program.ROM.u8(addr + 0) == 0)
                {//アドレスが空だったら増やす必要がある
                    //とりあえずUnitID: 0x01 を設置する.
                    Program.ROM.write_u8(addr + 0, 0x1, undodata);
                    Program.ROM.write_u8(addr + 1, 0x1, undodata);
                }

                addr += eearg.BlockSize;
            }
            Program.Undo.Push(undodata);

            U.ReSelectList(this.MAP_LISTBOX, this.EVENT_LISTBOX);
        }

        private void MapMouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {//右クリックされたとき
                int clickx = MapPictureBox.CursolToTile(e.X);
                int clicky = MapPictureBox.CursolToTile(e.Y);

                for (int index = 0; true; index++)
                {
                    //表示領域の都合上、選択されたユニットでなければ、最終移動位置しか描画していないため、探索するときはそれを踏まえる.
                    bool isSelectedUnit = (this.AddressList.SelectedIndex == index);

                    uint addr = InputFormRef.SelectToAddr(this.AddressList, index);
                    if (!U.isSafetyOffset(addr))
                    {
                        break;
                    }
                    int before_x = (int)Program.ROM.u8(addr + 4);
                    int before_y = (int)Program.ROM.u8(addr + 5);
                    int after_x = (int)Program.ROM.u8(addr + 6);
                    int after_y = (int)Program.ROM.u8(addr + 7);

                    if (isSelectedUnit)
                    {//選択している場合は、配置前座標を探索.
                        if (before_x == clickx && before_y == clicky)
                        {
                            this.AddressList.SelectedIndex = index; //選択されたユニットへ移動
                            this.B4.Focus(); //移動場所にフォーカスを当てることで notifyモードに切り替える.
                            break;
                        }
                    }
                    //配置後探索
                    if (after_x == clickx && after_y == clicky)
                    {
                        this.AddressList.SelectedIndex = index; //選択されたユニットへ移動

                        //ただし配置後と配置前か同一の場合、配置前を選択する.
                        if (before_x == after_x && before_y == after_y)
                        {
                            this.B4.Focus(); //配置前
                        }
                        else
                        {
                            this.B6.Focus(); //移動場所にフォーカスを当てることで notifyモードに切り替える.
                        }

                        break;
                    }
                }
            }
        }

        private void B4_ValueChanged(object sender, EventArgs e)
        {
            if (PosSyncUpdateComboBox.SelectedIndex == 0)
            {//配置後も自動変更 X
                U.ForceUpdate(B6,B4.Value);
            }
            else
            {
                AddressList_SelectedIndexChanged(null, null);
            }
        }

        private void B5_ValueChanged(object sender, EventArgs e)
        {
            if (PosSyncUpdateComboBox.SelectedIndex == 0)
            {//配置後も自動変更 Y
                U.ForceUpdate(B7, B5.Value);
            }
            else
            {
                AddressList_SelectedIndexChanged(null, null);
            }
        }
        private void NewButton_Click(object sender, EventArgs e)
        {
            EventUnitForm.CreateNewData(EVENT_LISTBOX, MAP_LISTBOX.SelectedIndex);
        }
        public void MakeAddressListExpandsCallback(EventHandler eventHandler)
        {
            this.InputFormRef.MakeAddressListExpandsCallback(eventHandler);
        }

        public static string CheckUnitsEvenetArg(uint units_address)
        {
            if (!U.isSafetyPointer(units_address))
            {
                return R._("ユニットを読みこむポインタの指定が正しくありません。: {0}", U.To0xHexString(units_address));
            }

            units_address = U.toOffset(units_address);
            if (!U.isSafetyOffset(units_address))
            {
                return R._("ユニットを読みこむポインタの指定が正しくありません。: {0}", U.To0xHexString(units_address));
            }

            int count = 0;
            uint addr = units_address;
            while (Program.ROM.u8(addr) != 0x0)
            {
                if (!U.isSafetyOffset(addr + 4))
                {
                    break;
                }

//                string errorMessage;
//                errorMessage = EventUnitForm.CheckUnitsUnitType(addr);
//                if (errorMessage.Length > 0)
//               {
//                    return R._("ユニットを読込 {0} 、 {1}番目のユニットのデータに問題があります。:{2}", U.To0xHexString(units_address), count, errorMessage);
//               }

                addr += Program.ROM.RomInfo.eventunit_data_size();
                if (!U.isSafetyOffset(addr))
                {
                    break;
                }
                count++;
            }

            return "";
        }


        private void AddressList_KeyDown(object sender, KeyEventArgs e)
        {
        }
        //プレイヤーユニットの重複を警告する.
        private void EventUnitForm_CheckDuplicatePlayerUnits(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            U.AddrResult selectEventAR = InputFormRef.SelectToAddrResult(this.EVENT_LISTBOX);
            U.AddrResult selectUnitAR = InputFormRef.SelectToAddrResult(this.AddressList);

            uint unitID = (uint)B0.Value;
            uint unitGrow = (uint)B3.Value;
            uint posHash = ((uint)B4.Value) << 8 | ((uint)B5.Value);
            L_0_UNIT.ErrorMessage = EventUnitForm.ErrorCheckDuplicatePlayerUnits(unitID
                , unitGrow
                , posHash
                , selectEventAR.addr
                , selectUnitAR.addr
                , mapid);
        }

        void CustomKeydownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ClearData((ListBoxEx)sender);
                return;
            }
            this.InputFormRef.GeneralAddressList_KeyDown(sender, e);
        }
        void ClearData(ListBoxEx listbox)
        {
            uint destAddr = InputFormRef.SelectToAddr(listbox);
            if (destAddr == U.NOT_FOUND)
            {
                return;
            }

            DialogResult dr = R.ShowYesNo("このユニットを消去して、データの終端にしてもよろしいですか？");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            uint blockSize = Program.ROM.RomInfo.eventunit_data_size();
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            Program.ROM.write_fill(destAddr, blockSize, 0, undodata);

            Program.Undo.Push(undodata);

            //再描画と再選択.
            //listbox.Invalidate();
            U.ReSelectList(listbox);

            InputFormRef.ShowWriteNotifyAnimation(this, destAddr);
        }
    }
}
