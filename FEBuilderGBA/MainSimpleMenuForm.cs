using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Text;
using Microsoft.Win32;

namespace FEBuilderGBA
{
    public partial class MainSimpleMenuForm : Form
    {
        public MainSimpleMenuForm()
        {
            InitializeComponent();
            this.MAP_LISTBOX.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
        }
        private void MainSimpleMenuForm_Load(object sender, EventArgs e)
        {
            ToolRunHintMessageForm.RemoveRunTestMenuIfUserWant(this.menuStrip1, this.TestRunStripMenuItem);
            InputFormRef.RecolorMenuStrip(this.menuStrip1);

            this.EventList.OwnerDraw(DrawEventList, DrawMode.OwnerDrawFixed);
            InputFormRef.markupJumpLabel(this.EventCond_Label);
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref  this.MAP_LISTBOX);

            this.MaximizeBox = true;
            MainSimpleMenuForm_Resize(null, null);

            Map.HideCommandBar();
            Map.SetZoom(150);
            Map.MapMouseDownEvent += MapMouseDownEvent;
            Map.MapDoubleClickEvent += MapDoubleClickEvent;
            U.SelectedIndexSafety(MAP_LISTBOX,0);

            SystemEvents.SessionEnding +=
                    new SessionEndingEventHandler(SystemEvents_SessionEnding);
        }

        private void UnitButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                InputFormRef.JumpForm<UnitFE6Form>();
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                InputFormRef.JumpForm<UnitFE7Form>();
            }
            else
            {
                InputFormRef.JumpForm<UnitForm>();
            }
        }

        private void ClassButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                InputFormRef.JumpForm<ClassFE6Form>();
            }
            else 
            {
                InputFormRef.JumpForm<ClassForm>();
            }
        }

        private void MapSettingButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                InputFormRef.JumpForm<MapSettingFE6Form>((uint)MAP_LISTBOX.SelectedIndex);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (!Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<MapSettingFE7UForm>((uint)MAP_LISTBOX.SelectedIndex);
                }
                else
                {
                    InputFormRef.JumpForm<MapSettingFE7Form>((uint)MAP_LISTBOX.SelectedIndex);
                }
            }
            else
            {
                InputFormRef.JumpForm<MapSettingForm>((uint)MAP_LISTBOX.SelectedIndex);
            }
        }

        private void LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<LogForm>();
        }


        private void EventUnitButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                EventUnitFE6Form f = (EventUnitFE6Form)InputFormRef.JumpForm<EventUnitFE6Form>(U.NOT_FOUND);
                f.JumpToMap((uint)MAP_LISTBOX.SelectedIndex);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                EventUnitFE7Form f = (EventUnitFE7Form)InputFormRef.JumpForm<EventUnitFE7Form>(U.NOT_FOUND);
                f.JumpToMap((uint)MAP_LISTBOX.SelectedIndex);
            }
            else
            {
                EventUnitForm f = (EventUnitForm)InputFormRef.JumpForm<EventUnitForm>(U.NOT_FOUND);
                f.JumpToMap((uint)MAP_LISTBOX.SelectedIndex);
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUndoForm>();
        }


        public void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.Open(this);
        }


        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.SaveOverraide(this);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.SaveAs(this);
        }




        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.Quit(this);
        }

        private void RunAsEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("emulator");
        }

        private void RunAsEmulator2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("emulator2");
        }

        private void RunAsBinaryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("binary_editor");
        }

        private void GraphicsToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("graphics_tool");
        }

        private void RunAsSappyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("sappy");
        }

        private void RunAsProgram1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program1");
        }

        private void RunAsProgram2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program2");
        }

        private void RunAsProgram3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program3");
        }

        private void RunAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs(this);
        }

        private void SettingOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OptionForm>();

        }


        private void SettingVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<VersionForm>();
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                InputFormRef.JumpForm<ItemFE6Form>();
            }
            else
            {
                InputFormRef.JumpForm<ItemForm>();
            }
        }


        private void SongTableButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SongTableForm>();
        }


        private void GraphicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<GraphicsToolForm>();

        }

        private void SongImportOtherROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SongExchangeForm>();

        }


        private void MapEditorButton_Click(object sender, EventArgs e)
        {
            MapEditorForm f = (MapEditorForm)InputFormRef.JumpForm<MapEditorForm>(U.NOT_FOUND);
            f.JumpTo((uint)MAP_LISTBOX.SelectedIndex);
        }

        private void PatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<PatchForm>();
        }


        private void DisassemblerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>();
            f.JumpTo(0x0);
        }


        private void LZ77ToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolLZ77Form>();
        }


        private void MainFE8Form_FormClosing(object sender, FormClosingEventArgs e)
        {
#if DEBUG
            if (Program.ArgsDic.ContainsKey("--lastrom"))
            {//デバッグ時はいちいち聞かれると面倒なのでスルーする
                return;
            }
#endif
            if (! MainFormUtil.IsNotSaveYet(this))
            {
                e.Cancel = true;
            }
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            if (!MainFormUtil.IsNotSaveYet(this))
            {//キャンセルをリクエスト
                e.Cancel = true;
            }
        }

        private void DetailMenuButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                MainFE6Form f = (MainFE6Form)InputFormRef.JumpForm<MainFE6Form>();
                f.SetFilter();
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                MainFE7Form f = (MainFE7Form)InputFormRef.JumpForm<MainFE7Form>();
                f.SetFilter();
            }
            else
            {
                MainFE8Form f = (MainFE8Form)InputFormRef.JumpForm<MainFE8Form>();
                f.SetFilter();
            }
        }
        private void PointerToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<PointerToolForm>();
        }

        List<U.AddrResult> EventAddrList;

        public class UnitsAddr
        {
            public uint addr;
            public List<MapPictureBox.StaticItem> items;
            public List<uint> items_addr;
        };
        List<UnitsAddr> UnitsAddrList;
        List<U.AddrResult> ExitPointList;
        List<U.AddrResult> MapObjectList;
        List<U.AddrResult> MapTrapList;
        Point MapTransporter; //FE7のみ　輸送体の位置(エリウッド編) yusoutai

        List<U.AddrResult> TurnZouenList;

        const uint FELINTBUZY_MESSAGE = 0xEECCCC00;
        const uint SYSTEMERROR_MESSAGE = 0xEEDDDD00;
        const uint MAPERROR_MESSAGE    = 0xEEEEEE00;
        const uint WORLDMAP_EVENT = 0xfb;

        void ScanMap(uint mapid)
        {
            Map.ClearStaticItem();
            Map.LoadMap(mapid);

            //離脱ポイント
            ExitPointList = MapExitPointForm.MakeList(mapid);

            //ユニット配置
            List<U.AddrResult> units = EventCondForm.MakeUnitPointer(mapid);
            //ユニットを表示
            UnitsAddrList = DrawUnits(units, mapid);
            //マップオブジェクトリスト
            MapObjectList = EventCondForm.MakePointerListBox(mapid,EventCondForm.CONDTYPE.OBJECT);
            //トラップリスト
            MapTrapList = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.TRAP);

            Bitmap exitPoint = ImageSystemIconForm.ExitPoint();
            for (int i = 0; i < ExitPointList.Count; i++)
            {
                uint addr = ExitPointList[i].addr;
                if (!U.isSafetyOffset(addr + 2))
                {
                    Log.Error("exit broken ", U.ToHexString(addr)); Debug.Assert(false);
                    continue;
                }
                uint x = Program.ROM.u8(addr + 0);
                uint y = Program.ROM.u8(addr + 1);
                Map.SetStaticItem("exit_" + i
                    , (int)x
                    , (int)y
                    , exitPoint
                    , 0
                    , 0
                    );
            }

            Bitmap cursol = ImageSystemIconForm.Cursol();
            for (int i = 0; i < MapObjectList.Count; i++)
            {
                uint addr = MapObjectList[i].addr;
                if (!U.isSafetyOffset(addr + 12))
                {
                    Log.Error("obj broken ",U.ToHexString(addr));  Debug.Assert(false);
                    continue;
                }

                uint x = Program.ROM.u8(addr + 8);
                uint y = Program.ROM.u8(addr + 9);
                Map.SetStaticItem("obj_" + i
                    , (int)x
                    , (int)y
                    , cursol
                    , 0
                    , 0
                    );
            }
            for (int i = 0; i < MapTrapList.Count; i++)
            {
                uint addr = MapTrapList[i].addr;
                if (!U.isSafetyOffset(addr + 8))
                {
                    Log.Error("trap broken ", U.ToHexString(addr)); Debug.Assert(false);
                    continue;
                }

                uint x = Program.ROM.u8(addr + 1);
                uint y = Program.ROM.u8(addr + 2);
                Map.SetStaticItem("trap_" + i
                    , (int)x
                    , (int)y
                    , cursol
                    , 0
                    , 0
                    );
            }

            for (int i = 0; i < UnitsAddrList.Count; i++)
            {
                List<MapPictureBox.StaticItem> items = UnitsAddrList[i].items;
                int len = items.Count;
                for (int n = 0; n < len; n++)
                {
                    Map.SetStaticItem("unit_" + i + " " + n
                        , items[n].x
                        , items[n].y
                        , items[n].bitmap
                        , items[n].draw_x_add
                        , items[n].draw_y_add
                        );
                }
            }

            this.MapTransporter = MapSettingForm.GetTransporter(mapid,true);

            if (Program.ROM.RomInfo.version() == 7 && this.MapTransporter.X < 255)
            {//FE7のみ輸送体の位置
                Bitmap yusoutai = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x3A, 0, true);
                Map.SetStaticItem("yusoutai"
                    , MapTransporter.X
                    , MapTransporter.Y
                    , yusoutai
                    );
            }

            //右側のイベントリストを作ります.
            this.EventAddrList = new List<U.AddrResult>();

            //システムエラーのチェック.
            List<FELint.ErrorSt>  systemErrorList = Program.AsmMapFileAsmCache.GetFELintCache(FELint.SYSTEM_MAP_ID);
            if (systemErrorList == null )
            {//準備中という表記を出す.
                this.EventAddrList.Add(new U.AddrResult(FELINTBUZY_MESSAGE, R._("計測中..."), FELINTBUZY_MESSAGE));
                if (! Program.AsmMapFileAsmCache.IsBusyThread())
                {//なぜ解析スレッドは停止しているのにnullになるケースがあるらしい
                    //もう一回作るように指示
                    Program.AsmMapFileAsmCache.BuildThread();
                }
            }
            else if (systemErrorList.Count > 0)
            {//エラーを表示する.
                this.EventAddrList.Add(new U.AddrResult(SYSTEMERROR_MESSAGE, R._("{0}個のエラーがあります", systemErrorList.Count), SYSTEMERROR_MESSAGE));
            }

            //章内のエラーのチェック
            List<FELint.ErrorSt>  mapErrorList = Program.AsmMapFileAsmCache.GetFELintCache(mapid);
            if (mapErrorList == null)
            {//こちらは準備中にはできないので、消す
            }
            else if (mapErrorList.Count > 0)
            {
                this.EventAddrList.Add(new U.AddrResult(MAPERROR_MESSAGE, R._("{0}個のエラーがあります", mapErrorList.Count), MAPERROR_MESSAGE));
            }

            //開始イベント
            List<U.AddrResult> eventList = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.START_EVENT);
            this.EventAddrList.AddRange(eventList);

            //終了イベント
            eventList = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.END_EVENT);
            this.EventAddrList.AddRange(eventList);

            //ターン条件
            eventList = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.TURN);
            TurnZouenList = ScanTurnZouen(eventList, mapid); //ターン増援を調べて tagに追加.
            this.EventAddrList.AddRange(eventList);

            //話す条件
            eventList = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.TALK);
            this.EventAddrList.AddRange(eventList);

            //常時条件
            eventList = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.ALWAYS);
            this.EventAddrList.AddRange(eventList);

            //ワールドマップ
            if (Program.ROM.RomInfo.version() == 6)
            {
                uint addr = WorldMapEventPointerFE6Form.GetEventByMapID(mapid);
                if (addr != U.NOT_FOUND)
                {
                    this.EventAddrList.Add(new U.AddrResult(addr, "WorldMapEvent", WORLDMAP_EVENT));
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                uint addr = WorldMapEventPointerFE7Form.GetEventByMapID(mapid);
                if (addr != U.NOT_FOUND)
                {
                    this.EventAddrList.Add(new U.AddrResult(addr, "WorldMapEvent", WORLDMAP_EVENT));
                }
            }
            else
            {
                uint addr = WorldMapEventPointerForm.GetEventByMapID(mapid,true);
                if (addr == U.NOT_FOUND)
                {
                    addr = WorldMapEventPointerForm.GetEventByMapID(mapid, false);
                }
                //前か後のどちらかがあればいい
                if (addr != U.NOT_FOUND)
                {
                    this.EventAddrList.Add(new U.AddrResult(addr, "WorldMapBeforeEvent", WORLDMAP_EVENT));
                }
            }

            U.ConvertListBox(this.EventAddrList, ref EventList);
        }

        //ユニット配置を検索して取得.
        List<UnitsAddr> DrawUnits(List<U.AddrResult> units, uint mapid)
        {
            List<UnitsAddr> list = new List<UnitsAddr>();

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].tag != mapid)
                {
                    continue;
                }
                for (int n = 0; n < EventCondForm.MapCond.Count; n++)
                {
                    if (EventCondForm.MapCond[n].Type == EventCondForm.CONDTYPE.FREEMAP_ENEMY_UNIT
                      || EventCondForm.MapCond[n].Type == EventCondForm.CONDTYPE.FREEMAP_PLAYER_UNIT
                        )
                    {//フリーマップは対象外
                        continue;
                    }
                    if (units[i].name.IndexOf(EventCondForm.MapCond[n].Name) < 0)
                    {
                        continue;
                    }


                    //戦闘準備前の茶番で表示されるユニットをできるだけ出さないように心掛ける.
                    bool isPlayerUnitJoinOK = false;
                    if (EventCondForm.MapCond[n].Type == EventCondForm.CONDTYPE.PLAYER_UNIT //プレイヤー初期配置
                        || EventCondForm.MapCond[n].Type == EventCondForm.CONDTYPE.TURN //Nターン後のイベント
                        || EventCondForm.MapCond[n].Type == EventCondForm.CONDTYPE.OBJECT //マップオブジェクト
                        || EventCondForm.MapCond[n].Type == EventCondForm.CONDTYPE.ALWAYS //常時
                        )
                    {//茶番ではないと思われるので自軍のユニットを許可する
                        isPlayerUnitJoinOK = true;
                    }

                    UnitsAddr p = new UnitsAddr();
                    p.addr = units[i].addr;
                    p.items = new List<MapPictureBox.StaticItem>();
                    p.items_addr = new List<uint>();
 
                    List<U.AddrResult> arlist = EventUnitForm.MakeList(p.addr);
                    for (int k = 0; k < arlist.Count; k ++)
                    {
                        uint assign = U.ParseUnitGrowAssign(Program.ROM.u8(arlist[k].addr + 3));
                        if (assign == 0  && !isPlayerUnitJoinOK)
                        {//自軍だが、現在追加は許可されていない。たぶん、進撃準備前の茶番だろうから無視する.
                            continue; 
                        }

                        p.items_addr.Add(arlist[k].addr);
                        p.items.Add(EventUnitForm.DrawAfterPosUnit(arlist[k].addr));
                    }

                    if (p.items.Count <= 0)
                    {
                        continue;
                    }
                    list.Add(p);
                    break;
                }
            }

            return list;
        }

        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)this.MAP_LISTBOX.SelectedIndex;

            ScanMap(mapid);
            if (!this.Map.IsMapLoad())
            {
                Debug.Assert(false);
                return;
            }

            int mapheight = (int)(this.Map.GetMapBitmapHeight() * 1.8);
            if (this.X_HELP_MESSAGE.Top < mapheight  )
            {
                this.X_HELP_MESSAGE.Hide();
            }
            else
            {
                this.X_HELP_MESSAGE.Show();
            }
        }

        bool MapMouseDownEvent_ExitPointList(uint mapid, int clickx, int clicky,bool rev)
        {
            for (int ii = 0; ii < ExitPointList.Count; ii++)
            {
                int i;
                if (rev)
                {
                    i = ExitPointList.Count - 1 - ii;
                }
                else
                {
                    i = ii;
                }

                uint addr = ExitPointList[i].addr;
                uint x = Program.ROM.u8(addr + 0);
                uint y = Program.ROM.u8(addr + 1);
                if (x == clickx && y == clicky)
                {
                    MapExitPointForm f = (MapExitPointForm)InputFormRef.JumpForm<MapExitPointForm>(U.NOT_FOUND);
                    f.JumpToMAPIDAndAddr(mapid, (uint)i);
                    return true;
                }
            }
            return false;
        }
        bool MapMouseDownEvent_MapObjectList(uint mapid, int clickx, int clicky, bool rev)
        {
            for (int ii = 0; ii < MapObjectList.Count; ii++)
            {
                int i;
                if (rev)
                {
                    i = MapObjectList.Count - 1 - ii;
                }
                else
                {
                    i = ii;
                }


                uint addr = MapObjectList[i].addr;
                uint x = Program.ROM.u8(addr + 8);
                uint y = Program.ROM.u8(addr + 9);
                if (x == clickx && y == clicky)
                {
                    if (rev)
                    {
                        bool r = JumpIfEvent(addr, (uint)EventCondForm.CONDTYPE.OBJECT);
                        if (r)
                        {
                            return true;
                        }
                    }

                    EventCondForm f = (EventCondForm)InputFormRef.JumpForm<EventCondForm>(U.NOT_FOUND);
                    f.JumpToMAPIDAndAddr(mapid, EventCondForm.CONDTYPE.OBJECT, (uint)addr);
                    return true;
                }
            }
            return false;
        }
        bool MapMouseDownEvent_UnitsAddrList(uint mapid, int clickx, int clicky, bool rev)
        {
            for (int ii = 0; ii < UnitsAddrList.Count; ii++)
            {
                int i; 
                if (rev)
                {
                    i = UnitsAddrList.Count - 1 - ii;
                }
                else
                {
                    i = ii;
                }

                List<MapPictureBox.StaticItem> items = UnitsAddrList[i].items;
                int len = items.Count;
                for (int nn = 0; nn < len; nn++)
                {
                    int n;
                    if (rev)
                    {
                        n = len - 1 - nn;
                    }
                    else
                    {
                        n = nn;
                    }

                    if (clickx == items[n].x && clicky == items[n].y)
                    {
                        if (Program.ROM.RomInfo.version() == 6)
                        {
                            EventUnitFE6Form f = (EventUnitFE6Form)InputFormRef.JumpForm<EventUnitFE6Form>(U.NOT_FOUND);
                            f.JumpTo(UnitsAddrList[i].addr, n);
                        }
                        else if (Program.ROM.RomInfo.version() == 7)
                        {
                            EventUnitFE7Form f = (EventUnitFE7Form)InputFormRef.JumpForm<EventUnitFE7Form>(U.NOT_FOUND);
                            f.JumpTo(UnitsAddrList[i].addr, n);
                        }
                        else
                        {
                            EventUnitForm f = (EventUnitForm)InputFormRef.JumpForm<EventUnitForm>(U.NOT_FOUND);
                            f.JumpTo(UnitsAddrList[i].addr, n);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        bool MapMouseDownEvent_MapTrapList(uint mapid, int clickx, int clicky, bool rev)
        {
            for (int ii = 0; ii < MapTrapList.Count; ii++)
            {
                int i;
                if (rev)
                {
                    i = MapTrapList.Count - 1 - ii;
                }
                else
                {
                    i = ii;
                }

                uint addr = MapTrapList[i].addr;
                uint x = Program.ROM.u8(addr + 1);
                uint y = Program.ROM.u8(addr + 2);
                if (x == clickx && y == clicky)
                {
                    EventCondForm f = (EventCondForm)InputFormRef.JumpForm<EventCondForm>(U.NOT_FOUND);
                    f.JumpToMAPIDAndAddr(mapid, EventCondForm.CONDTYPE.TRAP, (uint)addr);
                    return true;
                }
            }
            return false;
        }
        bool MapMouseDownEvent_Ysoutai(uint mapid, int clickx, int clicky, bool rev)
        {
            if (Program.ROM.RomInfo.version() == 7 && this.MapTransporter.X < 255)
            {//FE7のみ輸送体の位置
                if (clickx == MapTransporter.X && clicky == MapTransporter.Y)
                {
                    if (Program.ROM.RomInfo.is_multibyte())
                    {
                        InputFormRef.JumpForm<MapSettingFE7Form>(mapid);
                    }
                    else
                    {
                        InputFormRef.JumpForm<MapSettingFE7UForm>(mapid);
                    }
                    return true;
                }
            }
            return false;
        }
        void MapMouseDownEvent(object sender, MouseEventArgs e)
        {
            if (MAP_LISTBOX.SelectedIndex < 0)
            {
                return;
            }

            int clickx = Map.CursolToTile(e.X);
            int clicky = Map.CursolToTile(e.Y);
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {//反転
                bool rev = true;
                if (MapMouseDownEvent_ExitPointList(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_MapTrapList(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_MapObjectList(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_UnitsAddrList(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_Ysoutai(mapid, clickx, clicky, rev))
                {
                    return;
                }
            }
        }
        private void MapDoubleClickEvent(object sender, MouseEventArgs e)
        {
            if (MAP_LISTBOX.SelectedIndex < 0)
            {
                return;
            }

            int clickx = Map.CursolToTile(e.X);
            int clicky = Map.CursolToTile(e.Y);
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            {
                bool rev = false;
                if (MapMouseDownEvent_Ysoutai(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_UnitsAddrList(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_MapObjectList(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_MapTrapList(mapid, clickx, clicky, rev))
                {
                    return;
                }
                if (MapMouseDownEvent_ExitPointList(mapid, clickx, clicky, rev))
                {
                    return;
                }
            }
        }

        private Size DrawEventList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index >= this.EventAddrList.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            U.AddrResult ar = this.EventAddrList[index];

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font,FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            uint tag = (uint)(ar.tag & 0xff);

            if (ar.tag == SYSTEMERROR_MESSAGE)
            {
                SolidBrush errorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());
                text = R._("システムエラー:");
                U.DrawText(text, g, boldFont, errorBrush, isWithDraw, bounds);
                bounds.Y += lineHeight;

                U.DrawText(ar.name, g, normalFont, errorBrush, isWithDraw, bounds);
                errorBrush.Dispose();
            }
            else if (ar.tag == MAPERROR_MESSAGE)
            {
                SolidBrush errorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());
                text = R._("エラー:");
                U.DrawText(text, g, boldFont, errorBrush, isWithDraw, bounds);
                bounds.Y += lineHeight;

                U.DrawText(ar.name, g, normalFont, errorBrush, isWithDraw, bounds);
                errorBrush.Dispose();
            }
            else if (ar.tag == FELINTBUZY_MESSAGE)
            {//エラー計測中
                SolidBrush errorBrush = new SolidBrush(OptionForm.Color_Keyword_ForeColor());
                text = R._("FELint:");
                U.DrawText(text, g, boldFont, errorBrush, isWithDraw, bounds);
                bounds.Y += lineHeight;

                U.DrawText(ar.name, g, normalFont, errorBrush, isWithDraw, bounds);
                errorBrush.Dispose();
            }
            else if (tag == WORLDMAP_EVENT)
            {//ワールドマップイベント
                text = R._("ワールドマップイベント:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                if ( Program.ROM.RomInfo.version() != 8)
                {//FE8の場合、2分割されたので無理.
                    int maxWidth = bounds.X;

                    //次の行にコメントを書けるなら書く.
                    bounds = listbounds;
                    bounds.Y += lineHeight;
                    bounds.X += EventCondForm.DrawComment(U.toPointer(ar.addr), g, normalFont, isWithDraw, bounds);
                    //大きい行数の方を採用.
                    bounds.X = Math.Max(maxWidth, bounds.X);
                }
            }
            else if (tag == (uint)EventCondForm.CONDTYPE.START_EVENT)
            {//開始イベント
                text = R._("開始イベント:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            else if (tag == (uint)EventCondForm.CONDTYPE.END_EVENT)
            {//終了イベント
                text = R._("終了イベント:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            else if (!U.isSafetyOffset(ar.addr + 12))
            {
                text = R._("壊れています:{0}",U.To0xHexString(ar.addr) );
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
                Log.Error("event list broken ", U.ToHexString(ar.addr), ar.tag.ToString()); Debug.Assert(false);
            }
            else if (tag == (uint)EventCondForm.CONDTYPE.TURN)
            {
                uint start_turn = Program.ROM.u8(ar.addr + 8);
                uint end_turn = Program.ROM.u8(ar.addr + 9);
                if (start_turn < end_turn)
                {
                    text = R._("ターン:") + start_turn + "～" + end_turn;
                }
                else
                {
                    text = R._("ターン:") + start_turn;
                }
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                uint turntype = Program.ROM.u8(ar.addr + 10);
                if (turntype == 0)
                {
                    text = R._("(自ターン)");
                }
                else if (turntype == 0x40)
                {
                    text = R._("(友軍ターン)");
                }
                else if (turntype == 0x80)
                {
                    text = R._("(敵軍ターン)");
                }
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                int maxWidth = bounds.X;

                //次の行
                bounds = listbounds;
                bounds.Y += lineHeight;

                //このイベントに対応する増援があれば、描画する.
                for (int i = 0; i < this.TurnZouenList.Count; i++)
                {
                    if (this.TurnZouenList[i].addr == ar.addr
                        && U.isSafetyOffset(this.TurnZouenList[i].tag)
                        )
                    {
                        uint addr = this.TurnZouenList[i].tag;
                        while (Program.ROM.u8(addr) != 0x0)
                        {
                            Bitmap icon = EventUnitForm.DrawUnitIconOnly(addr);
                            U.MakeTransparent(icon);

                            Rectangle b = bounds;
                            b.Width = 14;
                            b.Height = 14;
                            bounds.X += U.DrawPicture(icon, g, isWithDraw, b);
                            icon.Dispose();

                            addr += Program.ROM.RomInfo.eventunit_data_size();
                            if (!U.isSafetyOffset(addr))
                            {
                                Debug.Assert(false);
                                break;
                            }
                        }
                        break;
                    }
                }
                //コメントを書けるなら書く.
                bounds.X += EventCondForm.DrawAchievementFlag(ar, g, normalFont, isWithDraw, bounds);
                bounds.X += EventCondForm.DrawComment(ar, g, normalFont, isWithDraw, bounds);
                //大きい行数の方を採用.
                bounds.X = Math.Max(maxWidth, bounds.X);
            }
            else if (tag == (uint)EventCondForm.CONDTYPE.TALK)
            {//話す
                uint from_unit = Program.ROM.u8(ar.addr + 8);
                uint to_unit = Program.ROM.u8(ar.addr + 9);
                text = R._("話す:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                Bitmap from_unit_image = UnitForm.DrawUnitMapFacePicture(from_unit);
                Bitmap to_unit_image = UnitForm.DrawUnitMapFacePicture(to_unit);

                U.MakeTransparent(from_unit_image);
                U.MakeTransparent(to_unit_image);

                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;
                bounds.X += U.DrawPicture(from_unit_image, g, isWithDraw, b);
                maxHeight = Math.Max(maxHeight, b.Height);

                text = " -> ";
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;
                bounds.X += U.DrawPicture(to_unit_image, g, isWithDraw, b);
                maxHeight = Math.Max(maxHeight, b.Height);

                from_unit_image.Dispose();
                to_unit_image.Dispose();
                int maxWidth = bounds.X;

                //コメントを書けるなら書く.
                bounds.X += EventCondForm.DrawComment(ar, g, normalFont, isWithDraw, bounds);
                //下にフラグを書こう.
                b = bounds;
                b.X = maxWidth + 4;
                b.Y += lineHeight;
                maxWidth += EventCondForm.DrawAchievementFlag(ar, g, normalFont, isWithDraw, b);

                //大きい行数の方を採用.
                bounds.X = Math.Max(maxWidth, bounds.X);
            }
            else if (tag == (uint)EventCondForm.CONDTYPE.ALWAYS)
            {
                uint type = Program.ROM.u8(ar.addr + 0);
                if (type == 0x1)
                {//常時条件 判定フラグ
                    uint flag = Program.ROM.u16(ar.addr + 8);

                    text = R._("常時条件:");
                    bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                    if (flag == 0)
                    {
                        text = U.To0xHexString(flag) + "(" + R._("常に実行") + ")";
                    }
                    else if (Program.FlagCache.TryGetValue(flag, out text))
                    {
                        text = U.To0xHexString(flag) + "(" + text + ")";
                    }
                    else
                    {
                        text = U.To0xHexString(flag);
                    }
                    bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
                }
                else if (type == 0xB)
                {//範囲条件
                    text = R._("範囲条件:");
                    bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                    uint x1 = Program.ROM.u8(ar.addr + 8);
                    uint y1 = Program.ROM.u8(ar.addr + 9);
                    uint x2 = Program.ROM.u8(ar.addr + 10);
                    uint y2 = Program.ROM.u8(ar.addr + 11);

                    text = "(" + x1 + "," + y1 + ")～(" + x2 + "," + y2 + ")";
                    bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
                }
                else
                {//常時条件asm
                    text = R._("常時条件:");
                    bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                    uint asmPointer = Program.ROM.u32(ar.addr + 8);
                    string dummy;
                    string asmName = Program.AsmMapFileAsmCache.GetASMName(asmPointer, false, out dummy);
                    if (asmName == "")
                    {
                        text = "(ASM:" + U.To0xHexString(U.toOffset(asmPointer)) + ")";
                    }
                    else
                    {
                        text = "(ASM:" + asmName + ")";
                    }
                    bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
                }

                int maxWidth = bounds.X;

                //次の行にコメントを書けるなら書く.
                bounds = listbounds;
                bounds.Y += lineHeight;
                bounds.X += EventCondForm.DrawAchievementFlag(ar, g, normalFont, isWithDraw, bounds);
                bounds.X += EventCondForm.DrawComment(ar, g, normalFont, isWithDraw, bounds);
                //大きい行数の方を採用.
                bounds.X = Math.Max(maxWidth, bounds.X);
            }

            bounds.Y += maxHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        private void MainSimpleMenuForm_Activated(object sender, EventArgs e)
        {//フォーカスが戻ってきたときに、すべて再構築する
            int select = this.MAP_LISTBOX.SelectedIndex;

            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref  this.MAP_LISTBOX);

            MAP_LISTBOX.SelectedIndex = -1; //確実に変更イベントを起こす.
            U.SelectedIndexSafety(MAP_LISTBOX, select, true);
        }

        void EnterEvent(bool canDirectAccess)
        {
            if (MAP_LISTBOX.SelectedIndex < 0)
            {
                return;
            }
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;

            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.EventList);
            uint tag = (uint)(ar.tag & 0xff);

            if (ar.tag == SYSTEMERROR_MESSAGE)
            {//システムエラー
                MainSimpleMenuEventErrorForm f = (MainSimpleMenuEventErrorForm)InputFormRef.JumpForm<MainSimpleMenuEventErrorForm>(U.NOT_FOUND);
                f.Init(FELint.SYSTEM_MAP_ID, false);
                return;
            }
            if (ar.tag == MAPERROR_MESSAGE)
            {//章内のエラー
                MainSimpleMenuEventErrorForm f = (MainSimpleMenuEventErrorForm)InputFormRef.JumpForm<MainSimpleMenuEventErrorForm>(U.NOT_FOUND);
                f.Init(mapid, false);
                return;
            }
            if (ar.tag == FELINTBUZY_MESSAGE)
            {//現在分析中
                return;
            }

            if (tag == WORLDMAP_EVENT)
            {//ワールドマップイベント
                if (Program.ROM.RomInfo.version() == 6)
                {
                    EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                    f.JumpTo(ar.addr);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                    f.JumpTo(ar.addr);
                }
                else
                {
                    WorldMapEventPointerForm f = (WorldMapEventPointerForm)InputFormRef.JumpForm<WorldMapEventPointerForm>(U.NOT_FOUND);
                    f.JumpTo(mapid + 1);
                }
                return;
            }
            if (tag == (uint)EventCondForm.CONDTYPE.START_EVENT)
            {//開始イベント
                EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                f.JumpTo(ar.addr);
                return;
            }
            if (tag == (uint)EventCondForm.CONDTYPE.END_EVENT)
            {//終了イベント
                EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                f.JumpTo(ar.addr);
                return;
            }

            //イベント
            if (canDirectAccess)
            {
                if (JumpIfEvent(ar.addr, tag))
                {
                    return;
                }
            }

            //イベント設定画面を開く.
            {
                EventCondForm f = (EventCondForm)InputFormRef.JumpForm<EventCondForm>(U.NOT_FOUND);
                f.JumpToMAPIDAndAddr(mapid, (EventCondForm.CONDTYPE)tag, (uint)ar.addr);
            }
        }

        bool JumpIfEvent(uint eventAddr,uint tag)
        {
            if (!U.isSafetyOffset(eventAddr + 12))
            {
                return false;
            }

            if (   tag == (uint)EventCondForm.CONDTYPE.TALK
                || tag == (uint)EventCondForm.CONDTYPE.TURN
                || tag == (uint)EventCondForm.CONDTYPE.ALWAYS)
            {
                uint addr = Program.ROM.p32(eventAddr + 4);
                if (! U.isSafetyOffset(addr))
                {
                    return false;
                }

                //飛べそうなので飛ばす
                EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                f.JumpTo(addr);
                return true;
            }
            if (tag == (uint)EventCondForm.CONDTYPE.OBJECT)
            {
                uint addr = Program.ROM.p32(eventAddr + 4);
                if (!U.isSafetyOffset(addr))
                {
                    return false;
                }
                uint type = Program.ROM.u8(eventAddr + 10);
                if (EventCondForm.IsShopObjectType(type))
                {
                    //店
                    ItemShopForm f = (ItemShopForm)InputFormRef.JumpForm<ItemShopForm>(U.NOT_FOUND);
                    f.JumpTo(addr);
                    return true;
                }
                if (EventCondForm.IsChestObjectType(type))
                {
                    //宝箱
                    return false;
                }

                //それ以外ならば、イベントに飛ばせる.
                {
                    EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                    f.JumpTo(addr);
                    return true;
                }
            }

            //無理
            return false;
        }

        private void EventList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EnterEvent(canDirectAccess: false);
        }

        private void MainSimpleMenuImageSubButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MainSimpleMenuImageSubForm>();
        }

        private void MainSimpleMenuForm_Resize(object sender, EventArgs e)
        {
            int y = MenuPanel.Location.Y + MenuPanel.Height;

            EventPanel.Location = new Point(this.Width - EventPanel.Width - 10, y);
            MapPanel.Location = new Point(MapListPanel.Location.X + MapListPanel.Width, y);
            MapPanel.Size = new Size(EventPanel.Location.X - (MapListPanel.Location.X + MapListPanel.Width), EventPanel.Height);

            int mapminus = -14;
            Map.Size = new Size(EventPanel.Location.X - (MapListPanel.Location.X + MapListPanel.Width), EventPanel.Height + Math.Abs(mapminus) );
            Map.Location = new Point(0, mapminus);
        }

        private void DiffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolDiffForm>();
        }

        private void eventAssembler_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventAssemblerForm>();
        }

        private void OpenLastUsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OpenLastSelectedFileForm>();
        }

        private void TranslateROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolTranslateROMForm>();
        }

        private void DecreaseColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
        }

        private void PortraitMakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolPortraitMakerForm>();
        }


        private void UPSSimpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUPSPatchSimpleForm>();
        }

        private void DiffDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolDiffDebugSelectForm>();
        }

        private void lintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolFELintForm>();
        }

        private void EventList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EnterEvent(canDirectAccess: false);
            }
        }

        private void ASMInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolASMInsertForm>();
        }

        private void EventList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {//左クリックは通常モードなので無視.
                return;
            }

            EventList.SelectedIndex = EventList.IndexFromPoint(e.X, e.Y);
            if (EventList.SelectedIndex < 0)
            {
                return;
            }
            EnterEvent(canDirectAccess: true);
        }

        private void FlagNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolFlagNameForm>();
        }

        private void ExportEAEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolExportEAEventForm f = (ToolExportEAEventForm)InputFormRef.JumpForm<ToolExportEAEventForm>();
            f.JumpToMAPID((uint)MAP_LISTBOX.SelectedIndex);
        }

        private void EmulatorMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EmulatorMemoryForm>();
        }

        static List<U.AddrResult> ScanTurnZouen(List<U.AddrResult> eventList, uint mapid)
        {
            List<uint> tracelist = new List<uint>();
            List<U.AddrResult> ret = new List<U.AddrResult>();
            for (int i = 0; i < eventList.Count; i++)
            {
                uint addr = eventList[i].addr;
                if (!U.isSafetyOffset(addr + 4))
                {
                    continue;
                }
                uint event_addr = Program.ROM.p32(addr + 4); //イベント

                if (!U.isSafetyOffset(event_addr))
                {
                    continue;
                }

                List<U.AddrResult> unitlist = new List<U.AddrResult>();
                EventCondForm.MakeUnitPointerEventScan(ref unitlist, "turn", event_addr, mapid, tracelist);

                if (unitlist.Count > 0)
                {
                    //とりあえず一番最初のやつを選択.
                    ret.Add(new U.AddrResult(addr, "", unitlist[0].addr));
                }
            }
            return ret;
        }

        private void ToolMagicEffectMakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolMagicEffectMakerForm>();
        }

        private void ToolProblemReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolProblemReportForm>();
        }

        private void ToolProblemReportToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolProblemReportForm>();
        }

        private void EventCond_Label_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {//Clickで処理するので不要
                EventCondForm f = (EventCondForm)InputFormRef.JumpForm<EventCondForm>(U.NOT_FOUND);
                f.JumpToMAPID((uint)MAP_LISTBOX.SelectedIndex);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ToolUseFlagForm f = (ToolUseFlagForm)InputFormRef.JumpForm<ToolUseFlagForm>(U.NOT_FOUND);
                f.JumpToMAPID((uint)MAP_LISTBOX.SelectedIndex);
            }

        }

        private void ToolUseFlagStripMenuItem6_Click(object sender, EventArgs e)
        {
            ToolUseFlagForm f = (ToolUseFlagForm)InputFormRef.JumpForm<ToolUseFlagForm>(U.NOT_FOUND);
            f.JumpToMAPID((uint)MAP_LISTBOX.SelectedIndex);
        }

        bool IsFELintBusied()
        {
            foreach (var a in this.EventAddrList)
            {
                if (a.tag == FELINTBUZY_MESSAGE)
                {
                    return true;
                }
            }
            return false;
        }

        //別スレッドで実行しているFELintが完成した場合の通知
        public void FELintUpdateCallback()
        {
            if (!IsFELintBusied())
            {
                return;
            }
            MAP_LISTBOX_SelectedIndexChanged(null, null);
        }

        private void MainSimpleMenuForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemEvents.SessionEnding -=
                    new SessionEndingEventHandler(SystemEvents_SessionEnding);
        }

        private void InitWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunToolInitWizard();
        }

        private void ChangeProjectNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolChangeProjectnameForm>();
        }
        static public uint GetCurrentMapID()
        {
            Form f = Program.MainForm();
            if (f is MainSimpleMenuForm && f.IsDisposed == false)
            {//メインフォームがある
                uint mapid = (uint)((MainSimpleMenuForm)f).MAP_LISTBOX.SelectedIndex;
                return mapid;
            }
            return 0;
        }
        private void WorkSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolWorkSupportForm>();
        }

        private void TestRunStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolRunHintMessageForm.Run();
        }

        private void OnlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoManual();
        }

        private void DiscordURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoCommunities();
        }
    }
}
