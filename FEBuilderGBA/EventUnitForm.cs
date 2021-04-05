using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class EventUnitForm : Form
    {
        public EventUnitForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.InputFormRef.PreWriteHandler += PreWriteHandler;
            this.N_InputFormRef = N_Init(this);

            this.MAP_LISTBOX.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
            this.AddressList.OwnerDraw(EventUnitForm.AddressList_Draw, DrawMode.OwnerDrawVariable);
            this.EVENT_LISTBOX.OwnerDraw(EventUnitForm.EVENT_LISTBOX_Draw, DrawMode.OwnerDrawVariable);
            this.FE8CoordListBox.OwnerDraw(FE8CoordListBox_Draw, DrawMode.OwnerDrawFixed, false);

            //マップIDリストを作る.
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref this.MAP_LISTBOX);

            EventUnitForm.AI1ToCombo(L_16_COMBO);
            EventUnitForm.AI2ToCombo(L_17_COMBO);
            EventUnitForm.AI3ToCombo(L_18_AI3_HYOUTEKI);

            //右クリックメニューを出す.
            this.InputFormRef.MakeGeneralAddressListContextMenu(true, true, CustomKeydownHandler);

            InputFormRef.MakeEditListboxContextMenu(FE8CoordListBox, FE8CoordListBox_KeyDown);

            this.InputFormRef.PreAddressListExpandsEvent += EventUnitForm.OnPreClassExtendsWarningHandler;
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;
            this.N_InputFormRef.AddressListExpandsEvent += N_AddressListExpandsEvent;

            this.MapPictureBox.MapMouseDownEvent += MapMouseDownEvent;
            this.MapPictureBox.MapMouseDownEvent += MapDoubleClickEvent;


            this.L_3_UNITGROW_LV.ValueChanged += Sim_Change_EventHandler;
            this.L_3_UNITGROW_GROW.SelectedIndexChanged += Sim_Change_EventHandler;

            this.L_3_UNITGROW_LV.Enter += Sim_Show_EventHandler;
            this.L_3_UNITGROW_GROW.Enter += Sim_Show_EventHandler;

            this.L_3_UNITGROW_LV.Leave += Sim_Hide_EventHandler;
            this.L_3_UNITGROW_GROW.Leave += Sim_Hide_EventHandler;
            this.X_Sim.Hide();

            InitCoordMainPanel();
        }
        int MoveCallback(int x,int y)
        {
            ControlPanel.Show();

            U.ForceUpdate(this.F_X, x);
            U.ForceUpdate(this.F_Y, y);
            
            ShowFloatingControlpanel();
            return 0;
        }
        void LinkMapFocusFunction(object sender,EventArgs e)
        {
            this.MapPictureBox.setNotifyMode("ControlPanel", MoveCallback);
        }

        void InitCoordMainPanel()
        {
            this.F_X.GotFocus += LinkMapFocusFunction;
            this.F_Y.GotFocus += LinkMapFocusFunction;

            EventHandler xy_value_change_function = (sender, e) =>
            {
                int x = (int)this.F_X.Value;
                int y = (int)this.F_Y.Value;

                DrawAllUnits();
                this.MapPictureBox.SetPoint("ControlPanel", x, y);
                CheckCoord();
            };
            this.F_X.ValueChanged += xy_value_change_function;
            this.F_Y.ValueChanged += xy_value_change_function;

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, this.F_SPEED, this.F_SPEED_COMBO, 0, "COMBO", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.F_UNITID, this.F_UNITID_NAME, 0, "UNIT", new string[] { });

            EXPLAIN_F_UNITID.AccessibleDescription = R._("このユニットが行動し終わった後に行動します。\r\n利用しない場合は、0を指定してください。");
            EXPLAIN_COORD.AccessibleDescription = R._("ユニットを配置する座標を指定します。\r\nダブルクリック、またはエンターキーを押すと、値を変更できます。\r\nFE8では、複数座標を指定できるようになりました。\r\nユニットは、リストの上の座標に登場し、リストの順番に行動します。\r\n");
            EXPLAIN_RAWDATA.AccessibleDescription = R._("ユニットの座標の生データです。デバッグ用に表示しています。\r\n値の内容は、以下の通りです。\r\n1.配置前座標をushortにパックしたもの、\r\n2.配置後座標の件数、\r\n3.配置後座標のポインタ。\r\n");
        }

        public static void AI1ToCombo(ComboBox combo)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            for (int i = 0; i < AI1.Count; i++)
            {
                combo.Items.Add(AI1[i].Name);
            }
            combo.EndUpdate();
            U.SelectedIndexSafety(combo, 0);
        }
        public static void AI2ToCombo(ComboBox combo)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            for (int i = 0; i < AI2.Count; i++)
            {
                combo.Items.Add(AI2[i].Name);
            }
            combo.EndUpdate();
            U.SelectedIndexSafety(combo, 0);
        }
        public static void AI3ToCombo(ComboBox combo)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            for (int i = 0; i < AI3.Count; i++)
            {
                combo.Items.Add(AI3[i].Name);
            }
            combo.EndUpdate();
            U.SelectedIndexSafety(combo, 0);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(EventUnitForm self)
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
                        return "-EMPTY-";
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
        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(EventUnitForm self)
        {
            return new InputFormRef(self
                , "N_"
                , 0
                , 8
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i<self.B7.Value;
                }
                , (int i, uint addr) =>
                {
                    return addr.ToString("X");
                }
                );
        }

        ToolTipEx X_Tooltip;
        private void EventUnitForm_Load(object sender, EventArgs e)
        {
            X_Tooltip = InputFormRef.GetToolTip<EventUnitForm>();
            MapPictureBox.SetDefualtIcon(ImageSystemIconForm.Blank16());
            COORD_PANEL.SetToolTipEx(X_Tooltip);

            LowScreenUpYCoordPanel();
        }

        //低い解像度の場合、操作できるようにパネルの位置を上に上げます。
        void LowScreenUpYCoordPanel()
        {
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            if (screenHeight < this.Height)
            {
                ControlPanel.Location = new Point(ControlPanel.Location.X, 120);
            }
        }

        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                return;
            }

            List<U.AddrResult> list = EventCondForm.MakeUnitPointer(mapid);
            //未記帳の拡張した領域があれば追加する. もし記帳していたらNewDataから削除する.
            EventUnitForm.AppendNoWriteNewData(list, mapid);
            U.ConvertListBox(list, ref this.EVENT_LISTBOX);

            U.SelectedIndexSafety(this.EVENT_LISTBOX,0);
            if (this.EVENT_LISTBOX.Items.Count > 0)
            {
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

        private void ReadAfterPlacement(object sender, EventArgs e)
        {

        }

        public void JumpToMap(uint mapid)
        {
            U.SelectedIndexSafety(MAP_LISTBOX, mapid);
        }

        public void JumpTo(uint addr,int unitIndex = 0)
        {
            addr = U.toOffset(addr);

            //アドレスからマップとイベントの逆変換
            int mapindex, eventindex;
            if (ConvertAddrToMapAndEvent(0,(uint)MAP_LISTBOX.Items.Count, addr, out mapindex, out eventindex))
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
            EventBattleTalkForm f = (EventBattleTalkForm)InputFormRef.JumpForm<EventBattleTalkForm>(U.NOT_FOUND);
            f.JumpTo((uint)B0.Value, (uint)MAP_LISTBOX.SelectedIndex);
        }

        private void JUMP_BATTLEBGM_Click(object sender, EventArgs e)
        {
            SoundBossBGMForm f = (SoundBossBGMForm)InputFormRef.JumpForm<SoundBossBGMForm>(U.NOT_FOUND);
            f.JumpTo((uint)B0.Value);
        }

        private void JUMP_HAIKU_Click(object sender, EventArgs e)
        {
            EventHaikuForm f = (EventHaikuForm)InputFormRef.JumpForm<EventHaikuForm>(U.NOT_FOUND);
            f.JumpTo((uint)B0.Value, (uint)MAP_LISTBOX.SelectedIndex);
        }

     

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef != null && this.InputFormRef.IsUpdateLock)
            {
                return;
            }
            ControlPanel.Hide();

            MakeUnitCoordList();
            DrawAllUnits();

            SetNotifyMode();

            this.FE8CoordListBox.DummyAlloc(FE8CoordList.Count , 0);
        }

        void PreWriteHandler(object sender, EventArgs e)
        {
            if (ControlPanel.Visible && InputFormRef.IsWriteButtonToYellow(UpdateButton))
            {//座標で、まだ書き込んでいないデータがある
                UpdateButton.PerformClick(); //座標を保存する.
            }

            if (FE8CoordList.Count < 1)
            {//不正なデータ。座標が1つもないはずがないのだが。
                return;
            }
            Pos pos = FE8CoordList[0];
            this.W4.Value = U.MakeFe8UnitPos(pos.x, pos.y, pos.ext);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            int count = FE8CoordList.Count - 1;
            if (count == 0)
            {//移動座標なし
                uint currentAddr = U.toOffset((uint)P8.Value);
                uint currentCount = (uint)B7.Value;
                if (currentCount > 0 && U.isSafetyOffset(currentAddr) )
                {//既存で移動座標がある場合、クリアする.
                    Program.ROM.write_fill(currentAddr, 8 * currentCount);
                    Program.Undo.Push(undodata);
                }

                P8.Value = 0;
                B7.Value = 0;

                return;
            }

            byte[] bin = new byte[8 * count];
            uint addr = 0;
            for (int i = 1; i < FE8CoordList.Count; i++, addr += 8)
            {
                pos = FE8CoordList[i];

                uint unitpos = U.MakeFe8UnitPos(pos.x, pos.y, pos.ext);
                U.write_u16(bin, addr + 0, unitpos);
                U.write_u8(bin, addr + 2, pos.speed);
                U.write_u8(bin, addr + 3, pos.unitid);
                U.write_u8(bin, addr + 4, pos.unk1);
                U.write_u8(bin, addr + 5, pos.unk2);
                U.write_u16(bin, addr + 6, pos.wait);
            }

            uint newaddr = InputFormRef.WriteBinaryData(this, (uint)P8.Value, bin, (uint a) =>
            {
                MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
                aal.addr = a;
                aal.length = (uint)this.B7.Value * 8;
                return aal;
            }, undodata);
            if (newaddr == U.NOT_FOUND)
            {//書き換えられなかった..
                return;
            }
            Program.Undo.Push(undodata);

            P8.Value = U.toPointer(newaddr);
            B7.Value = count;
        }

        void SetNotifyMode()
        {
            if (this.MapPictureBox.GetNotifyMode())
            {//既に通知モードなので何もしない.
                return;
            }
            LinkMapFocusFunction(null, null);
        }

        public static List<U.AddrResult> MakeList(uint addr)
        {
            if (Program.ROM.RomInfo.version() >= 8)
            {
                InputFormRef InputFormRef = Init(null);
                InputFormRef.ReInit(addr);
                return InputFormRef.MakeList();
            }
            else if (Program.ROM.RomInfo.version() >= 7)
            {//FE7
                return EventUnitFE7Form.MakeList(addr);
            }
            else
            {//FE6
                return EventUnitFE6Form.MakeList(addr);
            }
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
                    MapPictureBox.StaticItem sitem = DrawAfterPosUnit(list[i].addr);
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
                DrawUnit2(
                  class_id
                , (uint)B3.Value
                );

            int count = list.Count;
            for (int n = count - 1; n >= 0; n--)
            {
                MapPictureBox.SetStaticItem("c" + n.ToString(), list[n].x, list[n].y, list[n].bitmap, list[n].draw_x_add, list[n].draw_y_add);
            }
        }

        public static Bitmap DrawUnitIconOnly(uint addr)
        {
            if (! U.isSafetyOffset(addr+4))
            {
                return ImageUtil.BlankDummy();
            }

            uint unit_id = Program.ROM.u8(addr + 0);
            uint class_id = Program.ROM.u8(addr + 1);
            uint unitgrow = Program.ROM.u8(addr + 3);

            if (class_id == 0)
            {//クラスIDが0だったらユーザIDで補完する
                class_id = UnitForm.GetClassID(unit_id);
            }

            int palette_type = (int)U.ParseUnitGrowAssign(unitgrow);
            if (palette_type == 3 
                && PatchUtil.SearchCache_FourthAllegiance() == PatchUtil.FourthAllegiance_extends.FourthAllegiance)
            {
                palette_type = 4; //第4の忠誠
            }
            return ClassForm.DrawWaitIcon(class_id, palette_type,true);
        }

        public static Bitmap DrawItemIconOnly(uint addr,int itemcount)
        {
            if (Program.ROM.RomInfo.version() >= 8)
            {
                uint p = addr + 12 + (uint)itemcount;
                if (!U.isSafetyZArray(p))
                {
                    return ImageUtil.BlankDummy();
                }
                uint item_id = Program.ROM.u8(p);
                return ItemForm.DrawIcon(item_id);
            }
            else 
            {//FE7 or FE6
                return EventUnitFE7Form.DrawItemIconOnly(addr, itemcount);
            }
        }

        //アイコンの背丈の補正 中央に出すための補正値を取得
        public static void GetDrawAddXY(Bitmap icon, out int draw_x_add, out int draw_y_add)
        {
            if (icon.Width >= 32)
            {//でかいアイコン
                draw_x_add = 16 - icon.Width + 8;
                draw_y_add = 16 - icon.Height + 2;
            }
            else
            {
                //たまに背丈がでかくて16x16を超えてしまうやつがいるので補正する
                draw_x_add = 16 - icon.Width;
                draw_y_add = 16 - icon.Height;
            }
        }

        //ユニットの位置を描画 移動前->途中->移動後と、すべて出す
        List<MapPictureBox.StaticItem> DrawUnit2(uint class_id, uint unitgrow)
        {
            MapPictureBox.StaticItem st;
            List<MapPictureBox.StaticItem> list = new List<MapPictureBox.StaticItem>();

            int palette_type = (int)U.ParseUnitGrowAssign(unitgrow);
            if (palette_type == 3
                && PatchUtil.SearchCache_FourthAllegiance() == PatchUtil.FourthAllegiance_extends.FourthAllegiance)
            {
                palette_type = 4; //第4の忠誠
            }

            Bitmap icon = ClassForm.DrawWaitIcon(class_id, palette_type);

            //アイコンの背丈の補正 中央に出すための補正値を取得
            int draw_x_add;
            int draw_y_add;
            EventUnitForm.GetDrawAddXY(icon, out draw_x_add, out draw_y_add);

            uint assign = U.ParseUnitGrowAssign(unitgrow);
            int selected = FE8CoordListBox.SelectedIndex;
            if (FE8CoordList.Count <= 0)
            {
                return list;
            }
            Pos pos = FE8CoordList[0];
            int x = (int)pos.x;
            int y = (int)pos.y;
            if (selected == 0 && ControlPanel.Visible == true)
            {//現在編集しているので、最新のデータで描画する
                x = (int)F_X.Value;
                y = (int)F_Y.Value;
            }

            st = new MapPictureBox.StaticItem();
            st.bitmap = icon;
            st.x = x;
            st.y = y;
            st.draw_x_add = draw_x_add;
            st.draw_y_add = draw_y_add;
            list.Add(st);

            if (FE8CoordList.Count <= 1)
            {
                return list;
            }
            int coordCount = FE8CoordList.Count;
            if (coordCount >= 20)
            {//20以上は異常値として無視します.
                coordCount = 20;
            }

            for (int i = 1; i < coordCount; i++)
            {
                pos = FE8CoordList[i];
                int newx = (int)pos.x;
                int newy = (int)pos.y;
                if (selected == i && ControlPanel.Visible == true)
                {//現在編集しているので、最新のデータで描画する
                    newx = (int)F_X.Value;
                    newy = (int)F_Y.Value;
                }

                if (x == newx && y == newy)
                {
                    continue;
                }

                st = new MapPictureBox.StaticItem();
                st.bitmap = icon;
                st.x = newx;
                st.y = newy;
                st.draw_x_add = draw_x_add;
                st.draw_y_add = draw_y_add;
                list.Add(st);

                AppendDrawAllow(ref list, x, y, newx, newy);

                x = newx;
                y = newy;
            }
            return list;
        }

        //移動後の終端位置にいるキャラの描画
        public static MapPictureBox.StaticItem DrawAfterPosUnit(uint addr)
        {
            if (Program.ROM.RomInfo.version() >= 8)
            {
                uint unit_id = Program.ROM.u8(addr );
                uint class_id = Program.ROM.u8(addr + 1);
                uint unitgrow = Program.ROM.u8(addr + 3);
                uint beforre_unitpos = Program.ROM.u16(addr + 4);
                uint count = Program.ROM.u8(addr + 7);
                uint after_address = Program.ROM.u32(addr + 8);

                if (class_id == 0)
                {//クラスIDが0だったらユーザIDで補完する
                    class_id = UnitForm.GetClassID(unit_id);
                }

                return
                    DrawAfterPosUnit(class_id, unitgrow, beforre_unitpos, count, after_address);
            }
            else
            {//FE7 or FE6
                return EventUnitFE7Form.DrawAfterPosUnit(addr);
            }
        }
        public static MapPictureBox.StaticItem DrawAfterPosUnit(uint class_id, uint unitgrow, uint beforre_unitpos, uint count, uint after_address)
        {
            MapPictureBox.StaticItem st;

            int palette_type = (int)U.ParseUnitGrowAssign(unitgrow);
            if (palette_type == 3
                && PatchUtil.SearchCache_FourthAllegiance() == PatchUtil.FourthAllegiance_extends.FourthAllegiance)
            {
                palette_type = 4; //第4の忠誠
            }

            Bitmap icon = ClassForm.DrawWaitIcon(class_id, palette_type);

            //アイコンの背丈の補正 中央に出すための補正値を取得
            int draw_x_add;
            int draw_y_add;
            EventUnitForm.GetDrawAddXY(icon, out draw_x_add, out draw_y_add);

            uint assign = U.ParseUnitGrowAssign(unitgrow);
            int x = (int)U.ParseFE8UnitPosX(beforre_unitpos);
            int y = (int)U.ParseFE8UnitPosY(beforre_unitpos);
            st = new MapPictureBox.StaticItem();
            st.bitmap = icon;
            st.x = x;
            st.y = y;
            st.draw_x_add = draw_x_add;
            st.draw_y_add = draw_y_add;

            if (count <= 0)
            {
                return st;
            }

            after_address = U.toOffset(after_address);
            if (!U.isSafetyOffset(after_address))
            {//壊れている.
                return st;
            }

            uint addr = after_address + ((count - 1) * 8);
            if (!U.isSafetyOffset(addr+1))
            {//壊れている.
                return st;
            }

            uint after_unitpos = Program.ROM.u16(addr);
            int newx = (int)U.ParseFE8UnitPosX(after_unitpos);
            int newy = (int)U.ParseFE8UnitPosY(after_unitpos);

            st = new MapPictureBox.StaticItem();
            st.bitmap = icon;
            st.x = newx;
            st.y = newy;
            st.draw_x_add = draw_x_add;
            st.draw_y_add = draw_y_add;
            return st;
        }

        //やじるしを描画
        public static void AppendDrawAllow(ref List<MapPictureBox.StaticItem> list,int x,int y,int newx,int newy)
        {
            int lastYUpdate = 0;

            //起点
            MapPictureBox.StaticItem st = new MapPictureBox.StaticItem();
            st.x = x;
            st.y = y;
            if (x < newx)
            {
                if (x + 1 == newx)
                {//1マス違いの場合、まずは横方向に探索しないと変になる.
                    st.bitmap = ImageSystemIconForm.Allows(0);
                    list.Add(st);
                    x++;

                    if (y < newy)
                    {
                        st.bitmap = ImageSystemIconForm.Allows(5);
                        st.x = x;
                        st.y = y;
                        list.Add(st);
                        y++;
                        lastYUpdate = 1;
                    }
                    else if (y == newy)
                    {//無理
                    }
                    else
                    {
                        st.bitmap = ImageSystemIconForm.Allows(7);
                        st.x = x;
                        st.y = y;
                        list.Add(st);
                        y--;
                        lastYUpdate = 1;
                    }
                }
                else if (y < newy)
                {
                    st.bitmap = ImageSystemIconForm.Allows(1);
                    y++;
                    lastYUpdate = 1;
                }
                else if (y == newy)
                {
                    st.bitmap = ImageSystemIconForm.Allows(0);
                }
                else
                {
                    st.bitmap = ImageSystemIconForm.Allows(3);
                    y--;
                    lastYUpdate = -1;
                }
                list.Add(st);
            }
            else if (x == newx)
            {
                if (y < newy)
                {
                    st.bitmap = ImageSystemIconForm.Allows(3);
                    list.Add(st);
                    y++;
                    lastYUpdate = 1;
                }
                else if (y == newy)
                {//?
                }
                else
                {
                    st.bitmap = ImageSystemIconForm.Allows(1);
                    list.Add(st);
                    y--;
                    lastYUpdate = -1;
                }
            }
            else
            {
                if (x - 1 == newx)
                {//1マス違いの場合、まずは横方向に探索しないと変になる.
                    st.bitmap = ImageSystemIconForm.Allows(2);
                    x--;
                    if (y < newy)
                    {
                        st.bitmap = ImageSystemIconForm.Allows(4);
                        st.x = x;
                        st.y = y;
                        list.Add(st);
                        y++;
                        lastYUpdate = 1;
                    }
                    else if (y == newy)
                    {//無理
                    }
                    else
                    {
                        st.bitmap = ImageSystemIconForm.Allows(6);
                        st.x = x;
                        st.y = y;
                        list.Add(st);
                        y--;
                        lastYUpdate = 1;
                    }
                }
                else if (y < newy)
                {
                    st.bitmap = ImageSystemIconForm.Allows(1);
                    y++;
                    lastYUpdate = 1;
                }
                else if (y == newy)
                {
                    st.bitmap = ImageSystemIconForm.Allows(2);
                }
                else
                {
                    st.bitmap = ImageSystemIconForm.Allows(3);
                    y--;
                    lastYUpdate = -1;
                }
                list.Add(st);
            }
            list.Add(st);

            while (true)
            {
                st = new MapPictureBox.StaticItem();
                st.x = x;
                st.y = y;

                if (x < newx)
                {
                    if (y < newy)
                    {
                        st.bitmap = ImageSystemIconForm.Allows(13);
                        list.Add(st);
                        y++;
                        lastYUpdate = 1;
                    }
                    else if (y == newy)
                    {
                        if (x + 1 == newx)
                        {
                            st.bitmap = ImageSystemIconForm.Allows(8);
                            list.Add(st);
                            break;
                        }

                        if (lastYUpdate == 1)
                        {
                            lastYUpdate = 0;
                            st.bitmap = ImageSystemIconForm.Allows(6);
                        }
                        else if (lastYUpdate == -1)
                        {
                            lastYUpdate = 0;
                            st.bitmap = ImageSystemIconForm.Allows(4);
                        }
                        else
                        {
                            st.bitmap = ImageSystemIconForm.Allows(15);
                        }
                        list.Add(st);
                        x++;
                    }
                    else
                    {
                        st.bitmap = ImageSystemIconForm.Allows(12);
                        list.Add(st);
                        y--;
                        lastYUpdate = -1;
                    }
                }
                else if (x == newx)
                {
                    if (y < newy)
                    {
                        if (y + 1 == newy)
                        {
                            st.bitmap = ImageSystemIconForm.Allows(9);
                            list.Add(st);
                            break;
                        }

                        st.bitmap = ImageSystemIconForm.Allows(13);
                        list.Add(st);
                        y++;
                        lastYUpdate = 1;
                    }
                    else if (y == newy)
                    {//?
                        break;
                    }
                    else
                    {
                        if (y - 1 == newy)
                        {
                            st.bitmap = ImageSystemIconForm.Allows(11);
                            list.Add(st);
                            break;
                        }

                        st.bitmap = ImageSystemIconForm.Allows(12);
                        list.Add(st);
                        y--;
                        lastYUpdate = -1;
                    }
                }
                else
                {
                    if (y < newy)
                    {
                        st.bitmap = ImageSystemIconForm.Allows(13);
                        list.Add(st);
                        y++;
                        lastYUpdate = 1;
                    }
                    else if (y == newy)
                    {
                        if (x - 1 == newx)
                        {
                            st.bitmap = ImageSystemIconForm.Allows(10);
                            list.Add(st);
                            break;
                        }

                        if (lastYUpdate == 1)
                        {
                            lastYUpdate = 0;
                            st.bitmap = ImageSystemIconForm.Allows(7);
                        }
                        else if (lastYUpdate == -1)
                        {
                            lastYUpdate = 0;
                            st.bitmap = ImageSystemIconForm.Allows(5);
                        }
                        else
                        {
                            st.bitmap = ImageSystemIconForm.Allows(14);
                        }

                        list.Add(st);
                        x--;
                    }
                    else
                    {
                        st.bitmap = ImageSystemIconForm.Allows(12);
                        list.Add(st);
                        y--;
                        lastYUpdate = -1;
                    }
                }
            }
        }
        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            //配置後座標のアドレスをコピーしないようにします.
            InputFormRef.WriteButtonToYellow(WriteButton, false);

            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            uint rom_length = (uint)Program.ROM.Data.Length;

            //配置後アドレスを0クリアします.
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ClearP8AndB7");
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);
            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                if (addr + 8 + 4 > rom_length)
                {
                    Log.Error("ROM Broken! Address after allocation is out of range. {0}+8+4/{1}", U.ToHexString8(addr), U.ToHexString8(rom_length));
                    break;
                }

                Program.ROM.write_u8(addr + 7, 0, undodata);
                Program.ROM.write_u32(addr + 8, 0, undodata);
                addr += eearg.BlockSize;
            }

            //途中にnullが含まれている場合は、補正します.
            addr = eearg.NewBaseAddress;
            for (int i = 0; i < count; i++)
            {
                if (addr + 8 + 4 > rom_length)
                {
                    Log.Error("ROM Broken! Address after allocation is out of range. {0}+8+4/{1}", U.ToHexString8(addr), U.ToHexString8(rom_length));
                    break;
                }
                if (Program.ROM.u8(addr + 0) == 0)
                {//アドレスが空だったら増やす必要がある
                    //とりあえずUnitID: 0x01 を設置する.
                    Program.ROM.write_u8(addr + 0, 0x1, undodata);
                    Program.ROM.write_u8(addr + 1, 0x2, undodata);
                }

                addr += eearg.BlockSize;
            }
            Program.Undo.Push(undodata);

            U.ReSelectList(this.MAP_LISTBOX, this.EVENT_LISTBOX);
        }


        //配置座標が拡張されたとき
        void N_AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;
            if (count > 255)
            {
                count = 255;
            }

            U.ForceUpdate(this.P8, addr);
            U.ForceUpdate(this.B7, count);

            //ユニットデータに位置を書き込もう.
            uint unitaddr = InputFormRef.SelectToAddr(this.AddressList);
            if ( ! U.isSafetyOffset(unitaddr))
            {
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "ExpandsPostPlace");
            Program.ROM.write_u8(unitaddr + 7, (uint)count);
            Program.ROM.write_u32(unitaddr + 8, addr);
            Program.Undo.Push(undodata);
        }

        class Pos
        {
            public uint x;
            public uint y;
            public uint ext;
            public uint unitid;
            public uint wait;
            public uint speed;
            public uint unk1;
            public uint unk2;
        }
        List<Pos> FE8CoordList = new List<Pos>();

        void MakeUnitCoordList()
        {
            ClearUndoBuffer();
            FE8CoordList = new List<Pos>();
            Pos pos = new Pos();

            uint unitpos = (uint)this.W4.Value;
            pos.x = U.ParseFE8UnitPosX(unitpos);
            pos.y = U.ParseFE8UnitPosY(unitpos);
            pos.ext = U.ParseFE8UnitPosExtraBit(unitpos);
            pos.unk1 = 0xff;
            pos.unk2 = 0xff;
            FE8CoordList.Add(pos);

            F_X.Value = pos.x;
            F_Y.Value = pos.y;
            F_EXT.SelectedIndex = (int)pos.ext;
            F_SPEED.Value = 0;
            F_UNITID.Value = 0;
            F_WAIT.Value = 0;
            F_UNK1.Value = (int)pos.unk1;
            F_UNK2.Value = (int)pos.unk2;

            int count = (int)B7.Value;
            uint addr = U.toOffset((uint)P8.Value);
            for (int i = 0; i < count; i++ , addr += 8)
            {
                if (!U.isSafetyOffset(addr + 7))
                {
                    break;
                }

                pos = new Pos();
                unitpos = Program.ROM.u16(addr + 0);
                pos.x = U.ParseFE8UnitPosX(unitpos);
                pos.y = U.ParseFE8UnitPosY(unitpos);
                pos.ext = U.ParseFE8UnitPosExtraBit(unitpos);
                pos.speed = Program.ROM.u8(addr + 2);
                pos.unitid = Program.ROM.u8(addr + 3);
                pos.unk1 = Program.ROM.u8(addr + 4);
                pos.unk2 = Program.ROM.u8(addr + 5);
                pos.wait = Program.ROM.u16(addr + 6);

                FE8CoordList.Add(pos);
            }
            //アイテムドロップ
            UpdateItemDropLabel();
            //ランダムモンスター
            UpdateRandomMonster();
        }

        Size FE8CoordListBox_Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= FE8CoordList.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            SolidBrush brush = new SolidBrush(lb.ForeColor);
            SolidBrush errorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());
            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            Pos p = FE8CoordList[index];

            bounds.X += U.DrawText("X:", g, boldFont, brush, isWithDraw, bounds);
            U.DrawText(p.x.ToString(), g, normalFont, brush, isWithDraw, bounds);
            bounds.X += lineHeight*2;

            bounds.X += U.DrawText("Y:", g, boldFont, brush, isWithDraw, bounds);
            U.DrawText(p.y.ToString(), g, normalFont, brush, isWithDraw, bounds);
            bounds.X += lineHeight * 2;

            if (this.MapPictureBox.IsMapLoad())
            {
                int mapwidth = this.MapPictureBox.GetMapBitmapWidth() / 16;
                int mapheight = this.MapPictureBox.GetMapBitmapHeight() / 16;
                if (p.x >= mapwidth || p.y >= mapheight)
                {
                    bounds.X += U.DrawText(R._("マップ範囲外"), g, boldFont, errorBrush, isWithDraw, bounds);
                }
            }

            if (p.ext != 0)
            {
                bounds.X += U.DrawText(R._("特殊") + ":", g, boldFont, brush, isWithDraw, bounds);
                bounds.X += U.DrawText(GetUnitPosExtra(p.ext), g, normalFont, brush, isWithDraw, bounds);
                bounds.X += 4;
            }
            if (index >= 1)
            {
                if (p.speed != 0)
                {
                    bounds.X += U.DrawText(R._("移動速度") + ":", g, boldFont, brush, isWithDraw, bounds);
                    bounds.X += U.DrawText(GetUnitMoveSpeed(p.speed), g, normalFont, brush, isWithDraw, bounds);
                    bounds.X += 4;
                }
                if (p.wait != 0)
                {
                    bounds.X += U.DrawText(R._("待機") + ":", g, boldFont, brush, isWithDraw, bounds);
                    bounds.X += U.DrawText(p.wait.ToString(), g, normalFont, brush, isWithDraw, bounds);
                    bounds.X += 4;
                }
                if (p.unitid != 0)
                {
                    bounds.X += U.DrawText(R._("追従") + ":", g, boldFont, brush, isWithDraw, bounds);
                    bounds.X += U.DrawText(UnitForm.GetUnitName(p.unitid), g, normalFont, brush, isWithDraw, bounds);
                    bounds.X += 4;
                }
                if (!(p.unk1 == 0xFF && p.unk2 == 0xFF))
                {
                    bounds.X += U.DrawText(R._("FF") + ":", g, boldFont, brush, isWithDraw, bounds);
                    bounds.X += U.DrawText(U.To0xHexString(p.unk1) + "," + U.To0xHexString(p.unk2), g, normalFont, brush, isWithDraw, bounds);
                    bounds.X += 4;
                }
            }

            brush.Dispose();
            errorBrush.Dispose();
            boldFont.Dispose();

            bounds.Y += maxHeight;
            return new Size(bounds.X, bounds.Y);
        }

        string GetUnitMoveSpeed(uint v)
        {
            if (v == 0)
            {
                return R._("普通");
            }
            else if (v == 1)
            {
                return R._("鈍足");
            }
            else
            {
                return R._("不明") + U.To0xHexString(v);
            }
        }
        string GetUnitPosExtra(uint ext)
        {
            switch (ext)
            {
                case 0:
                    return R._("なし");
                case 1:
                    return R._("魔物");
                case 2:
                    return R._("アイテムドロップ");
                case 3:
                    return R._("魔物+アイテムドロップ");
                case 4:
                    return R._("特殊");
                case 5:
                    return R._("特殊+魔物");
                case 6:
                    return R._("特殊+アイテムドロップ");
                case 7:
                    return R._("特殊+魔物+アイテムドロップ");
                default:
                    return R._("不明") + U.To0xHexString(ext);
            }
        }


        public static Size EVENT_LISTBOX_Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            U.AddrResult ar = InputFormRef.SelectToAddrResult(lb, index);
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;

            string text = lb.Items[index].ToString();
            Size s = EventCondForm.DrawEventByAddr(text,lb, g, listbounds, isWithDraw);

            int lineMaxWidth = bounds.X + s.Width;
            bounds.Y += lineHeight;

            do
            {
                uint addr = ar.addr;
                if (!U.isSafetyOffset(addr))
                {
                    break;
                }

                while (Program.ROM.u8(addr) != 0x0)
                {
                    Bitmap icon = EventUnitForm.DrawUnitIconOnly(addr);
                    U.MakeTransparent(icon);

                    Rectangle b = bounds;
                    b.Width = 12;
                    b.Height = 12;
                    bounds.X += U.DrawPicture(icon, g, isWithDraw, b);
                    lineMaxWidth = Math.Max(lineMaxWidth, bounds.X);
                    icon.Dispose();

                    addr += Program.ROM.RomInfo.eventunit_data_size();
                    if (!U.isSafetyOffset(addr))
                    {
                        break;
                    }
                }
            }
            while (false);

            bounds.X = lineMaxWidth;
            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }

        public static Size AddressList_Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Font smallFont = new Font(lb.Font.FontFamily,0.8f);

            Rectangle bounds = listbounds;

            const int LINE_HEIGHT = 24;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = LINE_HEIGHT;

            Rectangle b = bounds;
            b.X += 24;
            
            U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            do
            {
                uint addr = InputFormRef.SelectToAddr(lb, index);
                if (!U.isSafetyOffset(addr))
                {
                    break;
                }
                Bitmap icon = EventUnitForm.DrawUnitIconOnly(addr);
                U.MakeTransparent(icon);

                //ユニット
                b = bounds;
                b.Width = LINE_HEIGHT;
                b.Height = LINE_HEIGHT;
                U.DrawPicture(icon, g, isWithDraw, b);
                icon.Dispose();

                //所持アイテムを小さいアイコンで描画
                for (int item = 0; item < 4; item++)
                {
                    icon = EventUnitForm.DrawItemIconOnly(addr, item);
                    U.MakeTransparent(icon);

                    b = bounds;
                    b.X += LINE_HEIGHT + 4 + (item * 13);
                    b.Y += lineHeight - 1;
                    b.Width = LINE_HEIGHT - (lineHeight - 1);
                    b.Height = LINE_HEIGHT - (lineHeight - 1);
                    U.DrawPicture(icon, g, isWithDraw, b);
                    icon.Dispose();
                }

                addr += Program.ROM.RomInfo.eventunit_data_size();
            }
            while (false);

            bounds.Y += maxHeight;

            brush.Dispose();
            smallFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }
        private void MapMouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {//右クリックされたとき
                MapDoubleClickEvent(sender, e);
            }
        }
        void MapDoubleClickEvent(object sender, MouseEventArgs e)
        {
            int clickx = MapPictureBox.CursolToTile(e.X);
            int clicky = MapPictureBox.CursolToTile(e.Y);
            int x, y;

            for (int index = 0; true; index++)
            {
                //表示領域の都合上、選択されたユニットでなければ、最終移動位置しか描画していないため、探索するときはそれを踏まえる.
                bool isSelectedUnit = (this.AddressList.SelectedIndex == index);

                uint addr = InputFormRef.SelectToAddr(this.AddressList, index);
                if (!U.isSafetyOffset(addr + 12))
                {
                    break;
                }
                uint count = Program.ROM.u8(addr + 7);

                if (isSelectedUnit || count <= 0)
                {//選択している場合、または、配置後がない場合は、配置前座標を探索.
                    uint beforre_unitpos = Program.ROM.u16(addr + 4);
                    x = (int)U.ParseFE8UnitPosX(beforre_unitpos);
                    y = (int)U.ParseFE8UnitPosY(beforre_unitpos);
                    if (x == clickx && y == clicky)
                    {
                        U.SelectedIndexSafety(this.AddressList, index, false); //選択されたユニットへ移動
                        //U.SelectedIndexSafety(this.FE8CoordList, 0, false);
                        break;
                    }
                }

                uint after_address = Program.ROM.p32(addr + 8);
                if (!U.isSafetyOffset(after_address))
                {
                    continue;
                }
                for (int n = 0; n < count; n++, after_address += 8)
                {
                    if (!U.isSafetyOffset(after_address + 7))
                    {
                        break;
                    }

                    if (isSelectedUnit || n + 1 == count)
                    {//選択している場合、または、最終位置
                        uint after_unitpos = Program.ROM.u16(after_address + 0);
                        x = (int)U.ParseFE8UnitPosX(after_unitpos);
                        y = (int)U.ParseFE8UnitPosY(after_unitpos);
                        if (x == clickx && y == clicky)
                        {
                            U.SelectedIndexSafety(this.AddressList, index, false); //選択されたユニットへ移動
                            U.SelectedIndexSafety(this.FE8CoordListBox, n+1, false);
                            //this.N_W0.Focus(); //移動場所にフォーカスを当てることで notifyモードに切り替える.
                            break;
                        }
                    }
                }
            }
        }


        public static Bitmap DrawMapAndUnit(uint mapid,uint unit_addr)
        {
            Bitmap map = MapSettingForm.DrawMap(mapid);
    
            List<U.AddrResult> list = MakeList(unit_addr);
            for (int i = 0; i < list.Count; i++)
            {
                //とりあえず、移動後座標だけ描画する.
                MapPictureBox.StaticItem sitem = DrawAfterPosUnit(list[i].addr);
                int p = ImageUtil.FindPalette(map, sitem.bitmap);
                if (p <= -1)
                {
                   p = ImageUtil.AppendPalette(map, sitem.bitmap);
                }
                ImageUtil.BitBlt(map
                    , sitem.x * 16 + sitem.draw_x_add, sitem.y * 16 + sitem.draw_y_add
                    , sitem.bitmap.Width,sitem.bitmap.Height
                    , sitem.bitmap,0,0 
                    , p,0 );
            }
            return map;
        }

        public class AI1st
        {
            public string Name;
        };
        public class AI2st
        {
            public string Name;
        };
        public class AI3TargetSt
        {
            public string Name;
        };
        public static List<AI1st> AI1 { get; private set; }
        public static List<AI2st> AI2 { get; private set; }
        public static List<AI3TargetSt> AI3 { get; private set; }

        public static void PreLoadResourceAI1(string fullfilename)
        {
            AI1 = new List<AI1st>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    string[] sp = line.Split('\t');
                    if (sp.Length <= 0)
                    {
                        continue;
                    }

                    string str = sp[0];
                    AI1st p = new AI1st();

                    //[0x5D2908]のユニット みたいなアドレス表記を埋める
                    str = InputFormRef.ConvertAddToValue(str);
                    p.Name = R._(str);
                    AI1.Add(p);
                }
            }

            uint addr = Program.ROM.p32(Program.ROM.RomInfo.ai1_pointer());
            uint count = AIScriptForm.DataCount(Program.ROM.RomInfo.ai1_pointer());
            for (int i = AI1.Count; i < count; i++)
            {
                AI1st p = new AI1st();

                string str = U.ToHexString(i) + "=" 
                    + R._("追加AI:") 
                    + GetAINameByAddrOrID(i, addr + ((uint)i * 4));
                p.Name = R._(str);
                AI1.Add(p);
            }
        }
        public static void PreLoadResourceAI2(string fullfilename)
        {
            AI2 = new List<AI2st>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    string[] sp = line.Split('\t');
                    if (sp.Length <= 0)
                    {
                        continue;
                    }

                    string str = sp[0];
                    AI2st p = new AI2st();

                    //[0x5D2908]のユニット みたいなアドレス表記を埋める
                    str = InputFormRef.ConvertAddToValue(str);
                    p.Name = str;
                    AI2.Add(p);
                }
            }

            uint addr = Program.ROM.p32(Program.ROM.RomInfo.ai2_pointer());
            uint count = AIScriptForm.DataCount(Program.ROM.RomInfo.ai2_pointer());
            for (int i = AI2.Count; i < count; i++)
            {
                AI2st p = new AI2st();

                string str = U.ToHexString(i) + "=" 
                    + R._("追加AI:") 
                    + GetAINameByAddrOrID(i, addr + ((uint)i * 4));
                p.Name = R._(str);
                AI2.Add(p);
            }
        }
        public static void PreLoadResourceAI3(string fullfilename)
        {
            AI3 = new List<AI3TargetSt>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    string[] sp = line.Split('\t');
                    if (sp.Length <= 0)
                    {
                        continue;
                    }

                    string str = sp[0];
                    AI3TargetSt p = new AI3TargetSt();

                    //[0x5D2908]のユニット みたいなアドレス表記を埋める
                    str = InputFormRef.ConvertAddToValue(str);
                    p.Name = str;
                    AI3.Add(p);
                }
            }
        }

        static string GetAINameByAddrOrID(int i, uint addr)
        {
            if (U.isSafetyOffset(addr))
            {
                uint c = Program.ROM.p32(addr);
                if (U.isSafetyOffset(c))
                {
                    return InputFormRef.GetCommentSA(c);
                }
            }
            return U.ToHexString(i);
        }

        public static string GetAIName1(uint ai)
        {
            if (ai >= AI1.Count)
            {
                return "";
            }
            return AI1[(int)ai].Name;
        }
        public static string GetAIName2(uint ai)
        {
            if (ai >= AI2.Count)
            {
                return "";
            }
            return AI2[(int)ai].Name;
        }
        public static string GetAIName3(uint ai)
        {
            if (ai >= AI3.Count)
            {
                return "";
            }
            return AI3[(int)ai].Name;
        }

        //新規に確保した領域
        static List<U.AddrResult> NewAllocData = new List<U.AddrResult>();
        private void NewButton_Click(object sender, EventArgs e)
        {
            EventUnitForm.CreateNewData(EVENT_LISTBOX, MAP_LISTBOX.SelectedIndex);
        }
        public static void CreateNewData(ListBox event_listbox,int mapid)
        {
            if (mapid < 0)
            {
                return;
            }

            EventUnitNewAllocForm form = (EventUnitNewAllocForm)InputFormRef.JumpFormLow<EventUnitNewAllocForm>();
            DialogResult dr = form.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            uint datacount = form.AllocCount;
            byte[] data = new byte[datacount * Program.ROM.RomInfo.eventunit_data_size() + 1]; //+1 は termデータ
            for (int i = 0; i < datacount; i++)
            {//リスト項目が出るように、無効なデータとならないように適当な値を入れる.
                data[i * Program.ROM.RomInfo.eventunit_data_size() + 0] = 1;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData("EevntUnit NEW");
            uint newaddr = InputFormRef.AppendBinaryData(data, undodata);

            U.AddrResult newData = new U.AddrResult(newaddr, "NEW", (uint)mapid);
            NewAllocData.Add(newData);

            List<U.AddrResult> arlist = (List<U.AddrResult>)event_listbox.Tag;
            arlist.Add(newData);
            event_listbox.Items.Add("NEW");
            Program.Undo.Push(undodata);

            //追加した領域を選択
            event_listbox.SelectedIndex = event_listbox.Items.Count - 1;
        }
        //未記帳の拡張した領域があれば追加する.(記帳済みのデータがあったら消す)
        public static void AppendNoWriteNewData(List<U.AddrResult> list,uint mapid)
        {
            for (int i = 0; i < NewAllocData.Count; i++)
            {
                if (NewAllocData[i].tag != mapid)
                {
                    continue;
                }
                //すでにないか確認.
                bool found = false;
                for (int n = 0; n < list.Count; n++)
                {
                    if (list[n].addr == NewAllocData[i].addr)
                    {//このデータは記帳している
                        NewAllocData.RemoveAt(i);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    list.Add(NewAllocData[i]);
                }
            }
        }
        public static void ClearNewData()
        {
            NewAllocData = new List<U.AddrResult>();
        }
        public static bool IsEventUnitReserve(ref uint addr)
        {
            List<Address> recycle = new List<Address>();
            RecycleReserveUnits(ref recycle);

            foreach (Address a in recycle)
            {
                if (addr >= a.Addr && addr < a.Addr + a.Length)
                {
                    addr = a.Addr + a.Length;
                    return true;
                }
            }
            return false;
        }

        //未記帳領域があれば利用しているリストに追記する
        public static void RecycleReserveUnits(ref List<Address> recycle)
        {
            foreach (U.AddrResult ar in NewAllocData)
            {
                InputFormRef InputFormRef = Init(null);
                InputFormRef.ReInit(ar.addr);

                RecycleOldUnitsLow(ref recycle, "NEW", InputFormRef);
            }
        }

        public static void RecycleOldUnits(ref List<Address> recycle,string basename, uint script_pointer)
        {
            uint script_addr = Program.ROM.u32(script_pointer);
            if (!U.isPointer(script_addr))
            {
                return ;
            }
            script_addr = U.toOffset(script_addr);
            if (!U.isSafetyOffset(script_addr))
            {
                return ;
            }

            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(script_pointer);

            RecycleOldUnitsLow(ref recycle, basename, InputFormRef);
        }

        static void RecycleOldUnitsLow(ref List<Address> recycle, string basename, InputFormRef InputFormRef)
        {
            if (Program.ROM.RomInfo.version() <= 7)
            {
                FEBuilderGBA.Address.AddAddress(recycle
                    , InputFormRef
                    , basename + " EVENT UNIT"
                    , new uint[] {} );
            }
            else
            {
                FEBuilderGBA.Address.AddAddress(recycle
                    , InputFormRef
                    , basename + " EVENT UNIT"
                    , new uint[] { 8 });

                List<U.AddrResult> list = InputFormRef.MakeList();
                for (int i = 0; i < list.Count; i++)
                {
                    uint addr = list[i].addr;
                    uint count = Program.ROM.u8(addr + 7);
                    uint after_address = Program.ROM.p32(addr + 8);
                    if (count > 0)
                    {
                        FEBuilderGBA.Address.AddAddress(recycle
                            , after_address
                            , count * 8
                            , addr + 8
                            , basename + " EVENT UNIT COORD " + i
                            , FEBuilderGBA.Address.DataTypeEnum.BIN);
                    }
                }
            }
        }
        public void MakeAddressListExpandsCallback(EventHandler eventHandler)
        {
            this.InputFormRef.MakeAddressListExpandsCallback(eventHandler);
        }
        public static void OnPreClassExtendsWarningHandler(object sender, EventArgs e)
        {
            InputFormRef.ExpandsEventArgs eventarg = (InputFormRef.ExpandsEventArgs)e;
            for (int i = 0; i < NewAllocData.Count; i++)
            {
                if (NewAllocData[i].addr == eventarg.OldBaseAddress)
                {
                    R.ShowStopError("このデータはどこにも関連づけられていません。サイズの変更は関連づけをした後にしてください。");
                    eventarg.IsCancel = true;
                    return;
                }
            }
        }

        static string CheckUnitsFE8AfterPos(uint addr)
        {
            uint count = Program.ROM.u8(addr + 7);
            uint after_address = Program.ROM.u32(addr + 8);

            if (count <= 0)
            {//OK
                return "";
            }

            if (!U.isSafetyPointer(after_address))
            {
                return R._("配置後座標が正しくありません。:{0}", U.To0xHexString(after_address));
            }

            after_address = U.toOffset(after_address);

            for (uint i = 0; i < count; i++)
            {
                uint addr2 = after_address + (i * 8);
                if (!U.isSafetyOffset(addr2 + 7))
                {//データがあるといったのにデータがないよ!
                    return R._("配置後座標が指定された数だけありません。:{0} Count:{1}", U.To0xHexString(after_address) , count);
                }

                //全データがnullなら警告を出す
                uint after_move_0 = Program.ROM.u32(addr2 + 0);
                uint after_move_4 = Program.ROM.u32(addr2 + 4);

                if (after_move_0 == 0 && after_move_4 == 0)
                {
                    return R._("配置後座標で、すべてのデータがゼロになっています。:{0} Count:{1}", U.To0xHexString(after_address), count);
                }
                if (after_move_0 == 0xFFFFFFFF && after_move_4 == 0xFFFFFFFF)
                {
                    return R._("配置後座標で、すべてのデータがFFになっています。:{0} Count:{1}", U.To0xHexString(after_address), count);
                }
            }
            return "";
        }
        static string CheckUnitMeleeAndRange(uint addr)
        {
            uint items = Program.ROM.u32(addr + 12);
            if (items == 0)
            {//アイテムをもっていないので関係ない.
                return "";
            }

            uint unit_id = Program.ROM.u8(addr);
            uint class_id = Program.ROM.u8(addr + 1);
            if (class_id == 0)
            {//判別不能
                return "";
            }

            bool unit_melee;
            bool unit_magic;
            UnitForm.GetWeaponType(unit_id, out unit_melee, out unit_magic);
            bool class_melee;
            bool class_magic;
            ClassForm.GetWeaponType(class_id, out class_melee, out class_magic);

            if ((unit_melee || class_melee) && (unit_magic || class_magic))
            {
                return R._("ユニットに想定と異なるクラスを割り当てています。\r\nこれでは、武器レベルで近接と魔法を混在させることになります。\r\n混在を可能にするパッチを当てていない状態で、近接と魔法を混在すると、戦闘アニメが正しく動作しません。");
            }
            return "";
        }

        public static string CheckUnitsEvenetArg(uint units_address)
        {
            if (units_address == EventUnitForm.INVALIDATE_UNIT_POINTER)
            {
                return R._("読み込むユニットを設定してください") + ":" + U.To0xHexString(units_address);
            }

            if (Program.ROM.RomInfo.version() == 7)
            {
                return EventUnitFE7Form.CheckUnitsEvenetArg(units_address);
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                return EventUnitFE6Form.CheckUnitsEvenetArg(units_address);
            }
            if (!U.isSafetyPointer(units_address))
            {
                if (units_address == 0xFFFFFFFF)
                {
                    //FE8の場合メモリースロット参照.
                    return "";
                }
                return R._("ユニットを読みこむポインタの指定が正しくありません。: {0}", U.To0xHexString(units_address));
            }

            units_address = U.toOffset(units_address);
            if (!U.isSafetyOffset(units_address))
            {
                return R._("ユニットを読みこむポインタの指定が正しくありません。: {0}", U.To0xHexString(units_address));
            }

            uint lvCap = PatchUtil.GetLevelMaxCaps();

            int count = 0;
            uint addr = units_address;
            uint pageSize = Program.ROM.RomInfo.eventunit_data_size();
            while (Program.ROM.u8(addr) != 0x0)
            {
                if (!U.isSafetyOffset(addr + pageSize))
                {
                    break;
                }

                string errorMessage;
                errorMessage = CheckPlayerUnitLevelCap(addr, lvCap);
                if (errorMessage.Length > 0)
                {
                    return R._("ユニットを読込 {0} 、 {1}番目のユニットのデータに問題があります。:{2}", U.To0xHexString(units_address), count, errorMessage);
                }

                errorMessage = CheckUnitsFE8AfterPos(addr);
                if (errorMessage.Length > 0)
                {
                    return R._("ユニットを読込 {0} 、 {1}番目のユニットのデータに問題があります。:{2}", U.To0xHexString(units_address) , count , errorMessage);
                }

                addr += pageSize;
                count ++;
            }

            if (IsIgnoreEventFE8(units_address))
            {
                return "";
            }
//            if (PatchUtil.SearchSkillSystem() == PatchUtil.skill_system_enum.SkillSystem)
//            {
//                if (count > 16)
//                {
//                    return R._("ユニットを読込 {0} 、SkillSystemsを利用している場合、16人以上のユニットを同時にロードできません。", U.To0xHexString(units_address));
//                }
//            }
            if (count > 50)
            {
                return R._("ユニットを読込 {0} 、50人以上のユニットを同時にロードできません。", U.To0xHexString(units_address));
            }

            return "";
        }

        static bool IsIgnoreEventFE8(uint units_address)
        {
            //メレカナ海岸で、51体の敵をロードしているので、無視するように指示する.
            if (Program.ROM.RomInfo.is_multibyte())
            {//FE8J
                if (units_address == 0x924118)
                {
                    return true;
                }
            }
            else
            {//FE8U
                if (units_address == 0x8cfcc4)
                {
                    return true;
                }
            }
            return false;
        }

        static string CheckPlayerUnitLevelCap(uint addr, uint lvCap)
        {
            uint unitGrow = Program.ROM.u8(addr + 3);

            uint assign = U.ParseUnitGrowAssign(unitGrow);
            if (assign != 0)
            {//自軍でないなら関係ない.
                return "";
            }

            uint lv = U.ParseUnitGrowLV(unitGrow);
            if (lv > lvCap)
            {
                return R._("自軍ユニットに、レベル上限({0})より大きいレベル({1})が指定されています。",lvCap,lv);
            }

            return "";
        }


        private void FE8CoordListBox_DoubleClick(object sender, EventArgs e)
        {
            RomToControlPanel();
            ShowFloatingControlpanel();
        }

        void RomToControlPanel()
        {
            int index = FE8CoordListBox.SelectedIndex;
            if (index < 0 || index >= this.FE8CoordList.Count)
            {
                U.ForceUpdate(F_X, 0);
                U.ForceUpdate(F_Y, 0);
                F_EXT.SelectedIndex = 0;
                F_SPEED.Value = 0;
                F_UNITID.Value = 0;
                F_WAIT.Value = 0;
                F_UNK1.Value = 0xff;
                F_UNK2.Value = 0xff;
            }
            else
            {
                Pos pos = this.FE8CoordList[index];
                U.ForceUpdate(F_X, pos.x);
                U.ForceUpdate(F_Y, pos.y);
                U.SelectedIndexSafety(F_EXT, pos.ext);
                U.SelectedIndexSafety(F_SPEED, pos.speed);
                U.SelectedIndexSafety(F_UNITID, pos.unitid);
                U.SelectedIndexSafety(F_WAIT, pos.wait);
                U.SelectedIndexSafety(F_UNK1, pos.unk1);
                U.SelectedIndexSafety(F_UNK2, pos.unk2);
            }
            //初期化したことで、変更されているマークが出るので消す.
            InputFormRef.WriteButtonToYellow(UpdateButton, false);
            InputFormRef.WriteButtonToYellow(NewButton2, false);
        }
        void CheckCoord()
        {
            if (!this.MapPictureBox.IsMapLoad())
            {//まだマップを読みこんでいない.
                return;
            }
            int mapwidth = this.MapPictureBox.GetMapBitmapWidth() / 16;
            int mapheight = this.MapPictureBox.GetMapBitmapHeight() / 16;

            if (F_X.Value >= mapwidth || F_Y.Value >= mapheight)
            {
                COORD_PANEL.ErrorMessage = R._("座標がマップの範囲外です。");
            }
            else
            {
                COORD_PANEL.ErrorMessage = "";
            }
        }
        Pos ControlPanelToPos()
        {
            int index = FE8CoordListBox.SelectedIndex;
            if (index < 0 || index >= this.FE8CoordList.Count)
            {
                return null;
            }
            else
            {
                Pos pos = new Pos();
                pos.x = (uint)F_X.Value;
                pos.y = (uint)F_Y.Value;
                pos.ext = (uint)F_EXT.SelectedIndex;
                pos.speed = (uint)F_SPEED.Value;
                pos.unitid = (uint)F_UNITID.Value;
                pos.wait = (uint)F_WAIT.Value;
                pos.unk1 = (uint)F_UNK1.Value;
                pos.unk2 = (uint)F_UNK2.Value;
                return pos;
            }
        }

        void ShowFloatingControlpanel()
        {
            //最初のデータでない場合は、移動のためのデータなので詳細のメニューを表示.
            int index = FE8CoordListBox.SelectedIndex;
            F_POST_COORD_PANEL.Visible = (index > 0);

            ControlPanel.Show();
            F_X.Focus();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
            FE8CoordListBox.Invalidate();
        }

        private void FE8CoordListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
            RomToControlPanel();
        }

        void HideFloatingControlpanel()
        {
            ControlPanel.Hide();
            DrawAllUnits();

            //アイテムドロップの更新
            UpdateItemDropLabel();
            //ランダムモンスターの更新
            UpdateRandomMonster();
        }

        private void FE8CoordListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                FE8CoordListBox_CopyToClipbord();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                FE8CoordListBox_ClipbordToPaste();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                FE8CoordListBox_DoubleClick(null, null);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                RemoveButton_Click(null, null);
            }
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                RunUndo();
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                RunRedo();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
            }
        }
        public void FE8CoordListBox_CopyToClipbord()
        {
            int selected = FE8CoordListBox.SelectedIndex;
            if (selected < 0 || selected >= FE8CoordList.Count)
            {
                return;
            }
            Pos pos = FE8CoordList[selected];
            string text = U.ToHexString(pos.x) 
                + "@" + U.ToHexString(pos.y)
                + "@" + U.ToHexString(pos.ext)
                + "@" + U.ToHexString(pos.speed)
                + "@" + U.ToHexString(pos.unitid)
                + "@" + U.ToHexString(pos.unk1)
                + "@" + U.ToHexString(pos.unk2)
                + "@" + U.ToHexString(pos.wait)
                + "@" + this.Name + "FE8CoordList";
            U.SetClipboardText(text);
        }
        public void FE8CoordListBox_ClipbordToPaste()
        {
            string text = Clipboard.GetText();
            string[] sp = text.Split('@');
            if (sp.Length < 9 || sp[sp.Length - 1] != this.Name + "FE8CoordList")
            {
                return;
            }
            PushUndo();

            Pos pos = new Pos();
            pos.x = U.atoh(sp[0]);
            pos.y = U.atoh(sp[1]);
            pos.ext = U.atoh(sp[2]);
            pos.speed = U.atoh(sp[3]);
            pos.unitid = U.atoh(sp[4]);
            pos.unk1 = U.atoh(sp[5]);
            pos.unk2 = U.atoh(sp[6]);
            pos.wait = U.atoh(sp[7]);

            FE8CoordList.Add(pos);
            FE8CoordListBox.DummyAlloc(FE8CoordList.Count , -1);
            DrawAllUnits();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
            PushUndo();

            int selected = this.FE8CoordListBox.SelectedIndex;
            if (selected < 0 || selected >= this.FE8CoordList.Count)
            {//追加で処理する.
                NewButton.PerformClick();
                return;
            }

            //タブで選択されている内容からAnimeStを生成する
            Pos code = ControlPanelToPos();
            if (code == null)
            {
                return;
            }

            //選択されているコードを入れ替える.
            this.FE8CoordList[this.FE8CoordListBox.SelectedIndex] = code;

            //リストの更新.
            this.FE8CoordListBox.DummyAlloc(this.FE8CoordList.Count, selected);

            HideFloatingControlpanel();
        }

        private void NewButton2_Click(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
            PushUndo();

            //タブで選択されている内容からAnimeStを生成する
            Pos code = ControlPanelToPos();
            if (code == null)
            {
                return;
            }

            int selected;
            //選択されている部分に追加
            if (this.FE8CoordListBox.SelectedIndex < 0)
            {
                this.FE8CoordList.Add(code);
                selected = this.FE8CoordList.Count - 1;
            }
            else
            {
                this.FE8CoordList.Insert(this.FE8CoordListBox.SelectedIndex + 1, code);
                selected = this.FE8CoordListBox.SelectedIndex + 1;
            }
            //リストの更新.
            this.FE8CoordListBox.DummyAlloc(this.FE8CoordList.Count, selected);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int selected = this.FE8CoordListBox.SelectedIndex;
            if (selected < 0 || selected >= this.FE8CoordList.Count)
            {
                return;
            }
            if (this.FE8CoordList.Count <= 1)
            {
                R.ShowStopError("ユニットの座標をすべて消すことはできません。");
                return;
            }
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
            PushUndo();

            this.FE8CoordList.RemoveAt(this.FE8CoordListBox.SelectedIndex);

            //リストの更新.
            this.FE8CoordListBox.DummyAlloc(this.FE8CoordList.Count, this.FE8CoordListBox.SelectedIndex - 1);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();        
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (this.FE8CoordListBox.SelectedIndex < 1 || this.FE8CoordListBox.SelectedIndex >= this.FE8CoordList.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
            PushUndo();

            U.SwapUp(this.FE8CoordList, this.FE8CoordListBox, this.FE8CoordListBox.SelectedIndex);

            //リストの更新.
            this.FE8CoordListBox.DummyAlloc(this.FE8CoordList.Count, this.FE8CoordListBox.SelectedIndex - 1);

            //コントロールパネルを閉じたくない.
            ControlPanel.Show();

            //アイテムドロップの更新
            UpdateItemDropLabel();
            //ランダムモンスターの更新
            UpdateRandomMonster();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (this.FE8CoordListBox.SelectedIndex < 0 || this.FE8CoordListBox.SelectedIndex + 1 >= this.FE8CoordList.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
            PushUndo();

            U.SwapDown(this.FE8CoordList, this.FE8CoordListBox, this.FE8CoordListBox.SelectedIndex);

            //リストの更新.
            this.FE8CoordListBox.DummyAlloc(this.FE8CoordList.Count, this.FE8CoordListBox.SelectedIndex + 1);

            //コントロールパネルを閉じたくない.
            ControlPanel.Show();

            //アイテムドロップの更新
            UpdateItemDropLabel();
            //ランダムモンスターの更新
            UpdateRandomMonster();
        }
        class UndoData
        {
            //UNDO サイズも小さいから、差分よりすべて記録する. 
            public List<Pos> CoordList;
        };

        List<UndoData> UndoBuffer;
        int UndoPosstion;
        static List<Pos> ClonePosList(List<Pos> posList)
        {
            List<Pos> list = new List<Pos>();
            int length = posList.Count;
            for (int i = 0; i < length; i++)
            {
                Pos code = posList[i];
                Pos a = new Pos();
                a.x = code.x;
                a.y = code.y;
                a.ext = code.ext;
                a.speed = code.speed;
                a.unitid = code.unitid;
                a.unk1 = code.unk1;
                a.unk2 = code.unk2;
                a.wait = code.wait;
                list.Add(a);
            }
            return list;
        }

        //Undo履歴のクリア
        void ClearUndoBuffer()
        {
            this.UndoBuffer = new List<UndoData>();
            this.UndoPosstion = 0;
        }
        void PushUndo()
        {
            if (this.UndoPosstion < this.UndoBuffer.Count)
            {//常に先頭に追加したいので、リスト中に戻っている場合は、それ以降を消す.
                this.UndoBuffer.RemoveRange(this.UndoPosstion, this.UndoBuffer.Count - this.UndoPosstion);
            }
            UndoData p = new UndoData();
            p.CoordList = ClonePosList(this.FE8CoordList);
            this.UndoBuffer.Add(p);
            this.UndoPosstion = this.UndoBuffer.Count;
        }
        void RunUndo()
        {
            if (this.UndoPosstion <= 0)
            {
                return; //無理
            }
            if (this.UndoPosstion == this.UndoBuffer.Count)
            {//現在が、undoがない最新版だったら、redoできるように、現状を保存する.
                PushUndo();
                this.UndoPosstion = UndoPosstion - 1;
            }

            this.UndoPosstion = UndoPosstion - 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);

        }
        void RunRedo()
        {
            if (this.UndoPosstion + 1 >= this.UndoBuffer.Count)
            {
                return; //無理
            }
            this.UndoPosstion = UndoPosstion + 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);
            UpdateItemDropLabel();
            UpdateRandomMonster();
        }
        void RunUndoRollback(UndoData u)
        {
            this.FE8CoordList = ClonePosList(u.CoordList);

            //リストの更新.
            this.FE8CoordListBox.DummyAlloc(this.FE8CoordList.Count, this.AddressList.SelectedIndex);
        }

        private void EventUnitForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
            }
        }

        private void F_EXT_SelectedIndexChanged(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(UpdateButton, true);
            InputFormRef.WriteButtonToYellow(NewButton2, true);
        }

        //プレイヤーユニットの重複を警告する.
        private void EventUnitForm_CheckDuplicatePlayerUnits(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            U.AddrResult selectEventAR = InputFormRef.SelectToAddrResult(this.EVENT_LISTBOX);
            U.AddrResult selectUnitAR = InputFormRef.SelectToAddrResult(this.AddressList);

            uint unitID = (uint)B0.Value;
            uint unitGrow = (uint)B3.Value;
            uint posHash = (uint)W4.Value;
            L_0_UNIT.ErrorMessage = ErrorCheckDuplicatePlayerUnits(unitID
                , unitGrow
                , posHash
                , selectEventAR.addr
                , selectUnitAR.addr
                , mapid);
        }
        public static string ErrorCheckDuplicatePlayerUnits(uint unitid
            , uint unitGrow
            , uint posHash
            , uint selectEventAddr
            , uint selectUnitAddr 
            , uint mapid)
        {
            if (unitid == 0 || unitid == 0xFF)
            {//未選択
                return "";
            }
            if (mapid == U.NOT_FOUND)
            {//不明なので判別不能
                return "";
            }
            if (!U.isSafetyOffset(selectEventAddr))
            {//不明なので判別不能
                return "";
            }
            if (!U.isSafetyOffset(selectUnitAddr))
            {//不明なので判別不能
                return "";
            }
            uint assign = U.ParseUnitGrowAssign(unitGrow);
            if (assign != 0)
            {//自軍でないなら関係ない.
                return "";
            }

            if (EventCondForm.IsPlayerUnit(selectEventAddr, mapid))
            {//自軍配置なので判別不能. 重複が普通にありうる.
                return "";
            }
            List<U.AddrResult> list = MakeList(selectEventAddr);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (addr == selectUnitAddr)
                {//自分自身なので無視.
                    continue;
                }

                uint targetUnitID = Program.ROM.u8(addr + 0);
                if (targetUnitID != unitid)
                {//ユニットIDが同一ではない
                    continue;
                }
                uint targetUnitGrow = Program.ROM.u8(addr + 3);
                uint targetUnitAssign = U.ParseUnitGrowAssign(targetUnitGrow);
                if (targetUnitAssign != 0)
                {//ユニットの所属が自軍ではない
                    continue;
                }

                uint targetPosHash = Program.ROM.u16(addr + 4);
                if (posHash == targetPosHash)
                {//配置前座標が同じなのでまだ警告を出すべきではない。おそらくリストを拡張した時にコピーされたんですね?
                    continue;
                }

                //問題発見!
                return R.Error("ユニット配置で、自軍のユニットID({0})が重複して書かれています。\r\n進撃開始に利用する自軍配置以外で、このような書き方をするのは極めてまれです。\r\nユニットの所属を友軍か敵軍と間違えてませんか？\r\n自軍の場合、同一ユニットIDは、一つのユニットにまとめられます。", UnitForm.GetUnitName(unitid)); 
            }
            //問題なし.
            return "";
        }

        //ポインタの共有をさせたくないので、移動ポインタのデータを落とす.
        void MaskMovePointerByClipbord()
        {
            string text = Clipboard.GetText();
            string[] sp = text.Split(' ');
            if (sp.Length != this.InputFormRef.BlockSize + 1)
            {
                return;
            }
            sp[7 + 1] = "0";
            sp[8 + 1] = "0";
            sp[9 + 1] = "0";
            sp[10 + 1] = "0";
            sp[11 + 1] = "0";
            text = String.Join(" ", sp);

            U.SetClipboardText(text);
        }

        public const uint INVALIDATE_UNIT_POINTER = 0xFFFFFF;

        private void X_ITEMDROP_Click(object sender, EventArgs e)
        {
            if (this.FE8CoordList.Count <= 0)
            {
                return;
            }

            EventUnitItemDropForm f = (EventUnitItemDropForm)InputFormRef.JumpFormLow<EventUnitItemDropForm>();
            DialogResult dr = f.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            PushUndo();
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                this.FE8CoordList[0].ext |= 0x2;
            }
            else if (dr == System.Windows.Forms.DialogResult.No)
            {
                if ((this.FE8CoordList[0].ext & 0x2) == 0x2)
                {
                    this.FE8CoordList[0].ext -= 0x2;
                }
            }
            //移動リストの再描画
            FE8CoordListBox.Invalidate();
            if (ControlPanel.Visible)
            {
                int index = FE8CoordListBox.SelectedIndex;
                if (index == 0)
                {//もし、移動設定ダイアログの最初のが表示されている場合、
                    //表示も更新しないと、おかしなことになる.
                    U.ForceUpdate(F_EXT, (int)this.FE8CoordList[0].ext);
                }
            }
            //アイテムドロップのラベルの更新
            UpdateItemDropLabel();
            //ランダムモンスターの更新
            UpdateRandomMonster();
            //設定を変更したので、ライトボタンの点灯
            InputFormRef.WriteButtonToYellow(WriteButton, true);
        }
        void UpdateItemDropLabel()
        {
            uint ext;
            if (this.FE8CoordList.Count <= 0)
            {
                ext = 0;
            }
            else
            {
                ext = this.FE8CoordList[0].ext;
            }

            if ((ext & 0x2) == 0x2)
            {//アイテムドロップ
                X_ITEMDROP.Text = R._("アイテムドロップ: ドロップする");
                X_ITEMDROP.ForeColor = OptionForm.Color_ControlComment_ForeColor();
            }
            else
            {
                X_ITEMDROP.Text = R._("アイテムドロップ: ドロップしない");
                X_ITEMDROP.ForeColor = OptionForm.Color_Control_ForeColor();
            }
        }
        void UpdateRandomMonster()
        {
            uint classid = (uint)this.B1.Value;
            if (IsRandomMonster())
            {
                string errorMessage;
                X_RANDOMMONSTER_VIEW.BackgroundImage = MonsterProbabilityForm.DrawUnitsList(classid, L_1_CLASS.Height, out errorMessage);
                X_RANDOMMONSTER_VIEW.ErrorMessage = errorMessage;
                if (X_RANDOMMONSTER.Visible)
                {
                    X_RANDOMMONSTER_VIEW.Invalidate();
                }
                else
                {
                    X_RANDOMMONSTER.BringToFront();
                    X_RANDOMMONSTER.Show();
                }
            }
            else
            {
                X_RANDOMMONSTER.Hide();
            }
        }
        bool IsRandomMonster()
        {
            uint ext;
            if (this.FE8CoordList.Count <= 0)
            {
                ext = 0;
            }
            else
            {
                ext = this.FE8CoordList[0].ext;
            }

            if ( (ext & 0x1) == 0x01 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            List<Address> recycle = new List<Address>();
            uint eventunit_data_size = Program.ROM.RomInfo.eventunit_data_size();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                List<U.AddrResult> list = EventCondForm.MakeUnitPointer(mapid);
                foreach (U.AddrResult ar in list)
                {
                    InputFormRef InputFormRef = Init(null);
                    InputFormRef.ReInit(ar.addr);
                    if (InputFormRef.DataCount <= 0)
                    {
                        continue;
                    }
                    uint length = InputFormRef.DataCount * eventunit_data_size;
                    recycle.Add(new Address(ar.addr,
                        length,
                        U.NOT_FOUND,
                        ar.name,
                        FEBuilderGBA.Address.DataTypeEnum.BIN));
                }
            }
            int maxlist = recycle.Count;
            for (int i = 0; i < maxlist; i++)
            {
                uint istart = recycle[i].Addr;
                uint iend = recycle[i].Addr + recycle[i].Length;

                for (int n = 0; n < maxlist; n++)
                {
                    if (i == n)
                    {
                        continue;
                    }

                    uint nstart = recycle[n].Addr;
                    if (nstart > istart && nstart < iend)
                    {
                        if ((nstart - istart) % eventunit_data_size == 0)
                        {//部分を利用していているだけ
                            continue;
                        }

                        errors.Add(new FELint.ErrorSt(FELint.Type.EVENTUNITS, nstart
                            , R._("ユニット配置で、「{0}」と「{1}」が重複しています。\r\nどちらかのデータをリポイントしてください。", U.To0xHexString(istart), U.To0xHexString(nstart)), nstart));
                    }
                }
            }
        }
        void CustomKeydownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ClearData((ListBoxEx)sender);
                return;
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                MaskMovePointerByClipbord();
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

        private void X_RANDOMMONSTER_DoubleClick(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MonsterProbabilityForm>((uint)this.B1.Value,"AddressList",this.B1);
        }
        private void Sim_Change_EventHandler(object sender, EventArgs e)
        {
            if (! X_Sim.Visible)
            {
                return;
            }

            uint lv = (uint)this.L_3_UNITGROW_LV.Value;
            uint grow = (uint)this.L_3_UNITGROW_GROW.SelectedIndex;
            uint unitid = (uint)this.B0.Value;
            uint classid = (uint)this.B1.Value;
            this.X_Sim.SetParam(lv,grow, unitid, classid);
        }
        private void Sim_Show_EventHandler(object sender, EventArgs e)
        {
            if (X_Sim.Visible)
            {
                return;
            }

            this.X_Sim.Show();
            Sim_Change_EventHandler(sender, e);
        }
        private void Sim_Hide_EventHandler(object sender, EventArgs e)
        {
            this.X_Sim.Hide();
        }
    }

}
