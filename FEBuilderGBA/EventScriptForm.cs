using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace FEBuilderGBA
{
    public partial class EventScriptForm : Form
    {
        public EventScriptForm()
        {
            InitializeComponent();
        }
        public struct ScriptEditSetTable
        {
            public Label ParamLabel;
            public NumericUpDown ParamSrc;
            public TextBoxEx ParamValue;
            public PictureBox ParamPicture;
        };
        //パラメータの初期値を作成する
        public static void SetOneScriptEditSetTables(ScriptEditSetTable table, EventScript.Arg arg,uint v)
        {
            table.ParamLabel.Text = arg.Name;

            //進数の指定
            if (EventScript.IsDecimal(arg.Type))
            {//10
                Color Color_InputDecimal_BackColor = OptionForm.Color_InputDecimal_BackColor();
                Color Color_InputDecimal_ForeColor = OptionForm.Color_InputDecimal_ForeColor();

                table.ParamSrc.ForeColor = Color_InputDecimal_ForeColor;
                table.ParamSrc.BackColor = Color_InputDecimal_BackColor;
                table.ParamSrc.Hexadecimal = false;

            }
            else
            {//16
                Color Color_Input_BackColor = OptionForm.Color_Input_BackColor();
                Color Color_Input_ForeColor = OptionForm.Color_Input_ForeColor();

                table.ParamSrc.ForeColor = Color_Input_ForeColor;
                table.ParamSrc.BackColor = Color_Input_BackColor;
                table.ParamSrc.Hexadecimal = true;
            }

            if (EventScript.IsSigned(arg.Type))
            {//符号あり
                if (arg.Size == 1)
                {//1バイト
                    U.SelectedIndexSafety(table.ParamSrc, -128, 127, (sbyte)v);
                }
                else if (arg.Size == 2)
                {//2バイト
                    U.SelectedIndexSafety(table.ParamSrc,-32768, 32767, (short)v);
                }
                else //if (arg.Size == 4)
                {//4バイト
                    Debug.Assert(arg.Size == 3); //3バイトの符号化は未対応.
                    U.SelectedIndexSafety(table.ParamSrc,-2147483648, 2147483647, (int)v);
                }
            }
            else
            {//符号なし ディフォルト
                if (arg.Size == 1)
                {//1バイト
                    U.SelectedIndexSafety(table.ParamSrc, 0, 0xff, v);
                }
                else if (arg.Size == 2)
                {//2バイト
                    U.SelectedIndexSafety(table.ParamSrc, 0, 0xffff, v);
                }
                else if (arg.Size == 3)
                {//3バイト
                    U.SelectedIndexSafety(table.ParamSrc, 0, 0xffffff, v);
                }
                else //if (arg.Size == 4)
                {//4バイト
                    U.SelectedIndexSafety(table.ParamSrc,0,0xffffffff,v);
                }
            }

            //飛べる項目ならばラベルを変化させる. (イベントの登録の関係上、すべてを同一にはできない・・・)
            bool isLabelJump = false;
            if (arg.Type == EventScript.ArgType.TEXT
                || arg.Type == EventScript.ArgType.CONVERSATION_TEXT
                || arg.Type == EventScript.ArgType.SYSTEM_TEXT
                || arg.Type == EventScript.ArgType.ONELINE_TEXT
                || arg.Type == EventScript.ArgType.POINTER_TEXT
                )
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.UNIT)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.CLASS)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.BG)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.CG)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.ITEM)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.MUSIC)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.SOUND)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.MAPCHAPTER)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_UNIT)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_EVENT)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_EVENTBATTLEDATA)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_EVENTMOVEDATA)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_TALKGROUP)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_MENUEXTENDS)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICOORDINATE)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT4)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICALLTALK)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_UNITSSHORTTEXT)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_PROCS)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_ASM)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AITILE)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.WMLOCATION)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.WMPATH)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.POINTER_TEXT)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.PORTRAIT || arg.Type == EventScript.ArgType.REVPORTRAIT)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.FLAG)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.RAM_UNIT_STATE)
            {//RAM_UNIT_STATE
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.DISABLEOPTIONS)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.DISABLEWEAPONS)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.IGNORE_KEYS)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.UNITCLASSABILITY)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.KEYS)
            {
                isLabelJump = true;
            }
            else if (arg.Type == EventScript.ArgType.ATTACK_TYPE)
            {
                isLabelJump = true;
            }

            if (isLabelJump)
            {
                InputFormRef.markupJumpLabel(table.ParamLabel);
            }
            else
            {
                InputFormRef.unmarkupJumpLabel(table.ParamLabel);
            }

            table.ParamLabel.Visible = true;
            table.ParamSrc.Visible = true;
            table.ParamValue.Visible = true;
            if (table.ParamPicture != null)
            {
                table.ParamPicture.Visible = true;
            }
        }
        //使わないパラメータはあっかりーんする
        public static void HideOneScriptEditSetTables(ScriptEditSetTable table)
        {
            table.ParamLabel.Visible = false;
            table.ParamSrc.Visible = false;
            table.ParamValue.Visible = false;
            if (table.ParamPicture != null)
            {
                table.ParamPicture.Visible = false;
            }
        }

        public static void SelectFirstParam(EventScriptForm.ScriptEditSetTable[] tables)
        {
            if (tables.Length <= 0)
            {
                return ;
            }
            if (tables[0].ParamSrc.Visible == false)
            {
                return;
            }
            tables[0].ParamSrc.Focus();
        }

        public static uint GetValueOneScriptEditSetTables(ScriptEditSetTable table, EventScript.Arg arg)
        {
            uint value;
            if (EventScript.IsSigned(arg.Type))
            {
                if (arg.Size == 1)
                {//1バイト
                    value = (uint)((sbyte)((int)table.ParamSrc.Value));
                }
                else if (arg.Size == 2)
                {//2バイト
                    value = (uint)((short)((int)table.ParamSrc.Value));
                }
                else //if (arg.Size == 4)
                {//4バイト
                    value = (uint)((int)table.ParamSrc.Value);
                }
            }
            else
            {
                value = (uint)(table.ParamSrc.Value);
            }
            return value;
        }

        public static uint WriteOneScriptEditSetTables(ScriptEditSetTable table, EventScript.Arg arg , EventScript.OneCode code)
        {
            uint value = GetValueOneScriptEditSetTables(table, arg);
            if (arg.Size == 1)
            {//1バイト
                U.write_u8(code.ByteData, (uint)arg.Position, value);
            }
            else if (arg.Size == 2)
            {//2バイト
                U.write_u16(code.ByteData, (uint)arg.Position, value);
            }
            else if (arg.Size == 3)
            {//3バイト
                U.write_u24(code.ByteData, (uint)arg.Position, value);
            }
            else if (EventScript.IsPointerArgs(arg.Type))
            {//ポインタは勝手にポインタにする.
                U.write_p32(code.ByteData, (uint)arg.Position, value);
            }
            else //if (arg.Size == 4)
            {//4バイト
                U.write_u32(code.ByteData, (uint)arg.Position, value);
            }
            return value;
        }
        //本体を変更したので、そこを参照するAliasを更新する.
        public static void WriteAliasScriptEditSetTables(ScriptEditSetTable table, EventScript.Arg arg, EventScript.OneCode code)
        {
            for (int i = 0; i < code.Script.Args.Length; i++)
            {
                EventScript.Arg a = code.Script.Args[i];
                if (a.Type == EventScript.ArgType.FIXED)
                {
                    continue;
                }
                if (a.Alias == U.NOT_FOUND)
                {//aliasではない
                    continue;
                }
                if (arg.Symbol != a.Symbol)
                {
                    continue;
                }
                WriteOneScriptEditSetTables(table, a , code);
            }
        }

        public static void ScanScript(List<Address> list, uint script_pointer, bool isWithEventUnit, bool isWorldMapEvent, string basename, List<uint> tracelist)
        {
            ScanScriptHelper helper = new ScanScriptHelper(list, isWithEventUnit, isWorldMapEvent, basename, tracelist);
            helper.Scan(script_pointer);
        }

        class ScanScriptHelper
        {
            List<Address> RefList;
            bool IsWithEventUnit;
            bool IsWorldMapEvent;
            string Basename;
            List<uint> TraceList;

            public ScanScriptHelper(List<Address> list
                , bool isWithEventUnit
                , bool isWorldMapEvent
                , string basename
                , List<uint> tracelist)
            {
                this.RefList = list;
                this.IsWithEventUnit = isWithEventUnit;
                this.IsWorldMapEvent = isWorldMapEvent;
                this.Basename = basename;
                this.TraceList = tracelist;
            }

            public void Scan(uint event_pointer)
            {
                uint event_addr = Program.ROM.u32(event_pointer);
                if (!U.isPointer(event_addr))
                {
                    return;
                }
                event_addr = U.toOffset(event_addr);
                if (!U.isSafetyOffset(event_addr))
                {
                    return;
                }
                if (TraceList.IndexOf(event_addr) >= 0)
                {//既知
                    FEBuilderGBA.Address.AddPointer(RefList
                        , event_pointer
                        , 0
                        , Basename
                        , Address.DataTypeEnum.EVENTSCRIPT
                        );
                    return;
                }
                TraceList.Add(event_addr);

                uint addr = event_addr;
                uint lastBranchAddr = 0;
                int unknown_count = 0;
                while (true)
                {
                    //バイト列をイベント命令としてDisassembler.
                    EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
	                if (EventScript.IsExitCode(code , addr , lastBranchAddr))
                    {//終端命令
                        uint len = (uint)(addr - event_addr + code.Script.Size);
                        FEBuilderGBA.Address.AddPointer(RefList
                            , event_pointer
                            , len
                            , Basename
                            , Address.DataTypeEnum.EVENTSCRIPT
                            );
                        return;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                    {
                        unknown_count++;
                        if (unknown_count > 10)
                        {//不明命令が10個連続して続いたら打ち切る
                            FEBuilderGBA.Address.AddPointer(RefList
                                , event_pointer
                                , 0
                                , Basename
                                , Address.DataTypeEnum.EVENTSCRIPT
                                );
                            return;
                        }
                    }
                    else
                    {
                        //少なくとも不明ではない.
                        unknown_count = 0;

                        if (code.Script.Has == EventScript.ScriptHas.POINTER_UNIT_OR_EVENT
                            || code.Script.Has == EventScript.ScriptHas.POINTER_OTHER
                            )
                        {//イベント命令へジャンプするものをもっているらしい.
                            for (int i = 0; i < code.Script.Args.Length; i++)
                            {
                                if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_EVENT)
                                {
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        Scan(v);
                                    }
                                }
                                else if (IsWithEventUnit == false)
                                {//nop
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_UNIT)
                                {//ユニット配置の回収(オプション)
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (v == EventUnitForm.INVALIDATE_UNIT_POINTER)
                                    {//ユニット指定がない状態
                                    }
                                    else if (U.isSafetyOffset(v)    //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        EventUnitForm.RecycleOldUnits(ref RefList, Basename, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_EVENTBATTLEDATA)
                                {//FE7戦闘バトルポインタの回収(オプション)
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        EventBattleDataFE7Form.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_EVENTMOVEDATA)
                                {//FE7イベントの移動配置座標の回収(オプション)
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        EventMoveDataFE7Form.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_TALKGROUP)
                                {//FE7会話グループポインタの回収(オプション)
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        EventTalkGroupFE7Form.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_MENUEXTENDS)
                                {//メニューオプション
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        MenuExtendSplitMenuForm.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_UNITSSHORTTEXT)
                                {//unitに関連付けられたshort型データ
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        UnitsShortTextForm.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_AICOORDINATE)
                                {//AI座標
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        AIASMCoordinateForm.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_AIUNIT4)
                                {//AIユニット4人
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        AIASMUnit4Form.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_AICALLTALK)
                                {//敵AIから話しかける
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        AIASMCALLTALKForm.RecycleOldData(ref RefList, v);
                                    }
                                }
                                else if (code.Script.Args[i].Type == EventScript.ArgType.POINTER_ASM)
                                {//ASM
                                    uint v = EventScript.GetArgPointer(code, i, addr);
                                    if (U.isSafetyOffset(v)         //安全で
                                        && TraceList.IndexOf(v) < 0 //まだ読んだことがなければ
                                        )
                                    {
                                        TraceList.Add(v);
                                        FEBuilderGBA.Address.AddFunction(RefList
                                            , v
                                            , ""
                                            );
                                    }
                                }
                            }
                        }
                        else if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                        {//LABEL
                            lastBranchAddr = 0;
                        }
                        else if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                        {//IF
                            lastBranchAddr = addr;
                        }
                    }
                    addr += (uint)code.Script.Size;
                }
            }
        }


        ToolTipEx ToolTip;
        private void EventScriptForm_Load(object sender, EventArgs e)
        {
            this.ToolTip = InputFormRef.GetToolTip<EventScriptForm>();
            //最大化を許可する.
            this.MaximizeBox = true;

            this.MainTab.NewTabEvent += (s, ee) =>
            {
                JumpTo(0x0);
            };
        }

        string ConvertName(uint addr, out int out_ExistingTabIndex)
        {
            addr = U.toOffset(addr);

            string name;
            if (U.isSafetyOffset(addr))
            {
                string dummy;
                string comment = Program.AsmMapFileAsmCache.GetEventName(U.toPointer(addr), out dummy);
                name = addr.ToString("X");
                if (comment.Length > 0)
                {
                    name = comment;
                }
            }
            else
            {
                name = "";
            }

            if (addr == 0)
            {//addr == 0 のタブだけは特別に何個も開ける.
                out_ExistingTabIndex = -1;
            }
            else
            {
                out_ExistingTabIndex = this.MainTab.FindTab(addr);
            }
            return name;
        }

        public void JumpTo(uint addr, uint event_current_addr = U.NOT_FOUND, EventHandler addressListExpandsEvent = null)
        {
            int existingTabIndex;
            string name = ConvertName(addr, out existingTabIndex);
            if (existingTabIndex >= 0)
            {//既存タブがある
                this.MainTab.SelectedIndex = existingTabIndex;
                return;
            }

            EventScriptInnerControl f = new EventScriptInnerControl();
            InputFormRef.InitControl(f, this.ToolTip);
            f.Init(this.ToolTip, this.EventScriptForm_KeyDown);
            f.JumpTo(addr, event_current_addr);
            if (addressListExpandsEvent != null)
            {
                f.AddressListExpandsEvent += addressListExpandsEvent;
            }

            this.MainTab.Add(name, f, addr);
            EventScriptForm_Resize(null, null);

            f.Navigation += OnNavigation;

            f.SetFocus();
        }

        void OnNavigation(object sender, EventArgs e)
        {
            if (!(e is EventScriptInnerControl.NavigationEventArgs))
            {
                return;
            }
            EventScriptInnerControl.NavigationEventArgs arg = (EventScriptInnerControl.NavigationEventArgs)e;
            if (arg.IsNewTab)
            {
                JumpTo(arg.Address);
            }
            else
            {
                int dummy;
                string name = ConvertName(arg.Address, out dummy);
                MainTab.UpdateTab(this.MainTab.SelectedIndex, name, arg.Address);
            }
        }

        private void EventScriptForm_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < MainTab.TabCount; i++)
            {
                TabPage tab = MainTab.TabPages[i];
                for (int n = 0; n < tab.Controls.Count; n++)
                {
                    Control c = tab.Controls[n];
                    if (c is EventScriptInnerControl)
                    {
                        {
                            c.Width = tab.Width;
                            c.Height = tab.Height;
                        }
                    }
                }
            }
        }

        private void EventScriptForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
                return;
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                ChangeDirect();
                return;
            }
            else if ((e.Control && e.KeyCode == Keys.Divide) || (e.Control && e.KeyCode == Keys.OemQuestion))
            {
                ChangeComment();
                return;
            }
        }
        void HideFloatingControlpanel()
        {
            if (this.MainTab.SelectedIndex < 0 || this.MainTab.SelectedIndex >= this.MainTab.TabCount)
            {
                return;
            }

            TabPage tab = MainTab.TabPages[this.MainTab.SelectedIndex];
            for (int n = 0; n < tab.Controls.Count; n++)
            {
                Control c = tab.Controls[n];
                if (c is EventScriptInnerControl)
                {
                    ((EventScriptInnerControl)c).HideFloatingControlpanel();
                }
            }

        }
        void ChangeDirect()
        {
            if (this.MainTab.SelectedIndex < 0 || this.MainTab.SelectedIndex >= this.MainTab.TabCount)
            {
                return;
            }

            TabPage tab = MainTab.TabPages[this.MainTab.SelectedIndex];
            for (int n = 0; n < tab.Controls.Count; n++)
            {
                Control c = tab.Controls[n];
                if (c is EventScriptInnerControl)
                {
                    ((EventScriptInnerControl)c).ChangeDirect();
                }
            }
        }
        void ChangeComment()
        {
            if (this.MainTab.SelectedIndex < 0 || this.MainTab.SelectedIndex >= this.MainTab.TabCount)
            {
                return;
            }

            TabPage tab = MainTab.TabPages[this.MainTab.SelectedIndex];
            for (int n = 0; n < tab.Controls.Count; n++)
            {
                Control c = tab.Controls[n];
                if (c is EventScriptInnerControl)
                {
                    ((EventScriptInnerControl)c).ChangeComment();
                }
            }
        }


        static string CheckEnableEvenetArg(EventScript.OneCode code, EventScript.Arg arg, uint v)
        {
            string errormessage = "";
            if (arg.Type == EventScript.ArgType.CONVERSATION_TEXT)
            {
                string text = FETextDecode.Direct(v);
                errormessage = TextForm.CheckConversationTextMessage(text, TextForm.MAX_SERIF_WIDTH);
            }
            else if (arg.Type == EventScript.ArgType.SYSTEM_TEXT)
            {
                string text = FETextDecode.Direct(v);
                errormessage = TextForm.CheckSystemTextMessage(text);
            }
            else if (arg.Type == EventScript.ArgType.ONELINE_TEXT)
            {
                string text = FETextDecode.Direct(v);
                errormessage = TextForm.CheckOneLineTextMessage(text, 100 * 8, 1 * 16 , true);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_UNIT)
            {
                errormessage = EventUnitForm.CheckUnitsEvenetArg(v);
            }
            else if (arg.Type == EventScript.ArgType.FLAG)
            {//フラグ
                if (!Program.FlagCache.CheckFast(v))
                {
                    errormessage = R._("この領域は利用できません");
                }
            }
            else if (arg.Type == EventScript.ArgType.MAGVELY)
            {//FE8の世界地図の移動 -8 ～ 52の範囲
                InputFormRef.GetMagvelYName((short)v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_PROCS)
            {//PROC
                Program.AsmMapFileAsmCache.GetASMName(v, true, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_ASM)
            {//ASM
                Program.AsmMapFileAsmCache.GetASMName(v, false, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.MEMORYSLOT)
            {//MemorySlot
                InputFormRef.GetMEMORYSLOT(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.PACKED_MEMORYSLOT)
            {//MemorySlotPacked
                InputFormRef.GetPACKED_MEMORYSLOT(v, code.Script.Info[0], out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.WMAP_SPRITE_ID)
            {//WMAP_SPRITE_ID
                InputFormRef.GetWMAP_SPRITE_ID(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.EVBIT)
            {//EVBIT
                InputFormRef.GetEVBIT(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.EVBIT_MODIFY)
            {//EVBIT_MODIFY
                InputFormRef.GetEVBIT_MODIFY(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.MAPCHAPTER)
            {
                if (code.Script.LowCode == "222AXXXX")
                {//MNC2
                    CheckMNC2(v, out errormessage);
                }
                CheckCallebleFromEvent(v, ref errormessage);
            }

            return errormessage;
        }

        //エラー検出
        static void CheckCallebleFromEvent(uint mapid, ref string errormessage)
        {
            uint mapaddr = MapSettingForm.GetMapAddr(mapid);
            if (mapaddr == U.NOT_FOUND)
            {
                return;
            }
            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereAddr(mapaddr);
            if (plists.event_plist == 0)
            {
                errormessage = R._("マップ({0})のイベントPLISTIDが0です。\r\nこのマップを呼び出すとフリーズします。\r\nイベントPLISTに適切な値を設定してください。", U.To0xHexString(mapid));
                return;
            }

            return;
        }

        public static void CheckMNC2(uint mapid, out string errormessage)
        {
            errormessage = "";
            if (Program.ROM.RomInfo.version() != 8)
            {
                return;
            }
            PatchUtil.mnc2_fix_enum mnc2fix = PatchUtil.SearchSkipWorldMapPatch();
            if (mnc2fix != PatchUtil.mnc2_fix_enum.NO)
            {//MNC2 Fixが導入されている
                return;
            }
            switch(mapid)
            {
                case 0x02: case 0x03:case 0x04:
                case 0x06: case 0x07:case 0x08:case 0x09:case 0x0A:case 0x0B:case 0x0D:case 0x0E:
                case 0x10: case 0x11:case 0x12:case 0x13:case 0x14:
                case 0x17: case 0x18:
                case 0x1A: case 0x1B:case 0x1C:case 0x1D:case 0x1E:case 0x1F:case 0x20:case 0x21:
                case 0x48: case 0x49:case 0x4A:case 0x4B:case 0x4C:case 0x4D:case 0x4E:

                errormessage = R._("マップ{0}にMNC2で移動するには「Skip Worldmap」パッチが必要です。", U.To0xHexString(mapid));
                return;
            }

            return;
        }

        //有効なイベントかどうかテストする
        public static void CheckEnableEvenet(uint start_addr, bool isWorldMapEvent, List<FELint.ErrorSt> errors, List<uint> tracelist)
        {
            tracelist.Add(start_addr);

            List<FELint.LabelCheckSt> labelCheck = new List<FELint.LabelCheckSt>();

            uint alert_unk_event_code = (uint)OptionForm.alert_unk_event_code();
            if (alert_unk_event_code == 0)
            {
                return;
            }

            int unknown_count = 0;
            int unknown_nulldata_count = 0;
            uint lastBranchAddr = 0;
            uint addr = start_addr;
            while (true)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                if (EventScript.IsExitCode(code , addr , lastBranchAddr))
                {//終端命令
                    addr += (uint)code.Script.Size;
                    break;
                }
                else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                {
                    unknown_count++;
                    if (unknown_count > alert_unk_event_code)
                    {//不明命令が個連続して続いたら打ち切る
                        errors.Add(new FELint.ErrorSt(FELint.Type.EVENTSCRIPT, U.toOffset(start_addr)
                            , R._("イベント「{0}」の先に多数の無効な命令が連続して現れました。\r\nイベントポインタが間違っている可能性があります。", U.To0xHexString(start_addr))));
                        return;
                    }
                    if (U.IsFillData00_Or_FF(code.ByteData))
                    {
                        unknown_nulldata_count++;
                        if (unknown_count >= 2)
                        {//0x00または0xffだけのイベントが2つ連続した
                            errors.Add(new FELint.ErrorSt(FELint.Type.EVENTSCRIPT, U.toOffset(start_addr)
                                , R._("イベント「{0}」の先に多数の無効な命令が連続して現れました。\r\nイベントポインタが間違っている可能性があります。", U.To0xHexString(start_addr))));
                            return;
                        }
                    }
                    else
                    {
                        unknown_nulldata_count = 0;
                    }
                }
                else
                {//不明でない命令
                    unknown_count = 0;
                    unknown_nulldata_count = 0;

                    for (int n = 0; n < code.Script.Args.Length; n++)
                    {
                        EventScript.Arg arg = code.Script.Args[n];
                        uint v = EventScript.GetArgValue(code, arg);

                        string errormessage = CheckEnableEvenetArg(code, arg, v);
                        if (errormessage.Length > 0)
                        {
                            errors.Add(new FELint.ErrorSt(FELint.Type.EVENTSCRIPT
                                , start_addr
                                , R._("イベント「{0}」の引数「型:{1}」「名前:{2}」「値:{3}({4})」\r\n{5}", U.To0xHexString(start_addr), arg.Type, arg.Name, v, U.To0xHexString(v), errormessage)
                                ,addr));
                        }

                        if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                        {//EVENT
                            v = U.toOffset(v);
                            if (U.isSafetyOffset(v)
                                && tracelist.IndexOf(v) < 0)
                            {
                                CheckEnableEvenet(v, isWorldMapEvent,errors, tracelist);
                            }
                        }
                        else if (arg.Type == EventScript.ArgType.LABEL_CONDITIONAL)
                        {//LABEL
                            labelCheck.Add(new FELint.LabelCheckSt(addr, v, FELint.LabelCheckStEnum.Def));
                        }
                        else if (arg.Type == EventScript.ArgType.IF_CONDITIONAL
                              || arg.Type == EventScript.ArgType.GOTO_CONDITIONAL)
                        {//IF
                            labelCheck.Add(new FELint.LabelCheckSt(addr, v, FELint.LabelCheckStEnum.Jump));
                        }
                    }

                    if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                    {//LABEL
                        lastBranchAddr = 0;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                    {//IF
                        lastBranchAddr = addr;
                    }
                }


                addr += (uint)code.Script.Size;
            }

            FELint.LabelCheck(errors, start_addr,labelCheck);
        }

        static int DrawPictureAndDispose(Bitmap pic, int width, int height, ref Rectangle bounds, int maxHeight, Graphics g, bool isWithDraw)
        {
            U.MakeTransparent(pic);

            Rectangle b = bounds;
            b.Width = width;
            b.Height = height;

            bounds.X += U.DrawPicture(pic, g, isWithDraw, b);
            pic.Dispose();
            maxHeight = Math.Max(maxHeight, b.Height);
            return maxHeight;
        }

        public static Size DrawCode(ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw, EventScript.OneCode code)
        {
            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font,FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            bounds.X += (int)(lb.Font.Height * 2 * code.JisageCount);

            text = code.Script.Info[0];
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            int i;
            string longtext = "";
            for (i = 1; i < code.Script.Info.Length; i += 2)
            {
                char symbol = ' ';
                if (code.Script.Info[i].Length > 2)
                {// [X みたいな文字列が入る. 2文字目のXが シンボル名.
                    symbol = code.Script.Info[i][1];
                }

                for (int n = 0; n < code.Script.Args.Length; n++)
                {
                    EventScript.Arg arg = code.Script.Args[n];
                    if (arg.Type == EventScript.ArgType.FIXED)
                    {
                        continue;
                    }
                    if (arg.Alias != U.NOT_FOUND)
                    {//別名が付けられているだけなのでパラメータを出さない
                        continue;
                    }
                    if (symbol != arg.Symbol)
                    {
                        continue;
                    }

                    text = code.Script.Args[n].Name;
                    bounds.X += U.DrawText(text + "[", g, normalFont, brush, isWithDraw, bounds);

                    uint v;
                    text = EventScript.GetArg(code, n, out v);

                    if (arg.Type == EventScript.ArgType.IF_CONDITIONAL
                        || arg.Type == EventScript.ArgType.LABEL_CONDITIONAL
                        || arg.Type == EventScript.ArgType.GOTO_CONDITIONAL
                        || arg.Type == EventScript.ArgType.WMAP_SPRITE_ID
                        )
                    {//条件IDまたは、WMAPのスプライト
                        SolidBrush keywordBrush = new SolidBrush(OptionForm.Color_Keyword_ForeColor());
                        bounds.X += U.DrawText(text, g, boldFont, keywordBrush, isWithDraw, bounds);
                        keywordBrush.Dispose();
                    }
                    else
                    {//それ以外のカテゴリ
                        bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                    }

                    if (arg.Type == EventScript.ArgType.TEXT
                        || arg.Type == EventScript.ArgType.CONVERSATION_TEXT
                        || arg.Type == EventScript.ArgType.SYSTEM_TEXT
                        || arg.Type == EventScript.ArgType.ONELINE_TEXT
                        || arg.Type == EventScript.ArgType.POINTER_TEXT
                    )
                    {
                        text = FETextDecode.Direct(v);
                        if (text.Length > 30)
                        {//長いテキストは画面隅に
                            longtext = text;
                        }
                        else if (text.Length > 0)
                        {//長くないテキストはすぐ横に表示
                            text = TextForm.StripAllCode(text);
                            bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);
                        }
                    }
                    else if (arg.Type == EventScript.ArgType.UNIT)
                    {
                        text = UnitForm.GetUnitName(v);
                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap image = UnitForm.DrawUnitMapFacePicture(v);
                        U.MakeTransparent(image);

                        Rectangle b = bounds;
                        b.Width = lineHeight * 2;
                        b.Height = lineHeight * 2;
                        bounds.X += U.DrawPicture(image, g, isWithDraw, b);
                        image.Dispose();
                        maxHeight = Math.Max(maxHeight, b.Height);
                    }
                    else if (arg.Type == EventScript.ArgType.CLASS)
                    {
                        text = ClassForm.GetClassName(v);
                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);
                        int affiliation = EventScript.GetAffiliation(code, n);
                        Bitmap image = ClassForm.DrawWaitIcon(v, affiliation);
                        maxHeight = DrawPictureAndDispose(image, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.ITEM)
                    {
                        text = ItemForm.GetItemName(v);
                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap image = ItemForm.DrawIcon(v);
                        maxHeight = DrawPictureAndDispose(image, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.BG)
                    {
                        text = ImageBGForm.GetComment(v);
                        if (text != "")
                        {
                            bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);
                        }

                        bounds.X += 2;
                        Bitmap image = ImageBGForm.DrawBG(v);
                        maxHeight = DrawPictureAndDispose(image, lineHeight * 6, lineHeight * 4
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.CG)
                    {
                        text = InputFormRef.GetCGComment(v);
                        if (text != "")
                        {
                            bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);
                        }

                        bounds.X += 2;
                        Bitmap image = ImageCGForm.DrawImageByID(v);
                        maxHeight = DrawPictureAndDispose(image, lineHeight * 6, lineHeight * 4
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.PORTRAIT || arg.Type == EventScript.ArgType.REVPORTRAIT)
                    {
                        Bitmap bitmap = ImagePortraitForm.DrawPortraitAuto(v);
                        if (arg.Type == EventScript.ArgType.REVPORTRAIT)
                        {
                            bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
                        }
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 4, lineHeight * 4
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_UNIT)
                    {
                        Bitmap bitmap = DrawUnitsList(v, lineHeight * 2);

                        bounds.X += 10;
                        maxHeight = DrawPictureAndDispose(bitmap, bitmap.Width, bitmap.Height
                            , ref bounds, maxHeight, g, isWithDraw);
                        bounds.X += 10;
                    }
                    else if (arg.Type == EventScript.ArgType.MAPCHAPTER)
                    {
                        text = MapSettingForm.GetMapName(v);
                        if (text.Length > 0)
                        {
                            bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                            Bitmap bitmap = MapSettingForm.DrawMap(v);
                            maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 4, lineHeight * 4
                                , ref bounds, maxHeight, g, isWithDraw);
                        }
                    }
                    else if (arg.Type == EventScript.ArgType.MUSIC || arg.Type == EventScript.ArgType.SOUND)
                    {
                        text = SongTableForm.GetSongName(v);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap;
                        if (arg.Type == EventScript.ArgType.MUSIC)
                        {//MUSIC
                            bitmap = ImageSystemIconForm.MusicIcon(6);
                        }
                        else
                        {//効果音
                            bitmap = ImageSystemIconForm.MusicIcon(7);
                        }
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.FLAG)
                    {//フラグ
                        string dummy;
                        text = InputFormRef.GetFlagName(v, out dummy);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap = ImageSystemIconForm.FlagIcon();
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_PROCS)
                    {//PROC
                        text = Program.AsmMapFileAsmCache.GetProcsName(v);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap = ImageSystemIconForm.MusicIcon(4);
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_ASM)
                    {//ASM
                        string dummy;
                        text = Program.AsmMapFileAsmCache.GetASMName(v, false, out dummy);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap = ImageSystemIconForm.MusicIcon(3);
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                    {//EVENT
                        string dummy;
                        text = Program.AsmMapFileAsmCache.GetEventName(v, out dummy);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap = ImageSystemIconForm.MusicIcon(3);
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.WMLOCATION)
                    {//ワールドマップの名前
                        text = " " + WorldMapPointForm.GetWorldMapPointName(v);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap = WorldMapImageForm.DrawWorldMapIcon(0xB);
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.WMPATH)
                    {//ワールドマップの道
                        text = " " + WorldMapPathForm.GetPathName(v);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap = WorldMapImageForm.DrawWorldMapIcon(0xB);
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.SKILL)
                    {//スキル
                        text = " " + InputFormRef.GetSkillName(v);

                        bounds.X += U.DrawText(" " + text, g, boldFont, brush, isWithDraw, bounds);

                        Bitmap bitmap = InputFormRef.DrawSkillIcon(v);
                        maxHeight = DrawPictureAndDispose(bitmap, lineHeight * 2, lineHeight * 2
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT)
                    {
                        text = "";
                        Bitmap bitmap = AIUnitsForm.DrawAIUnitsList(v, lineHeight * 2 - 2);
                        maxHeight = DrawPictureAndDispose(bitmap, bitmap.Width, bitmap.Height
                            , ref bounds, maxHeight, g, isWithDraw);
                    }
                    else
                    {
                        bool isENumText = false;
                        if (arg.Type == EventScript.ArgType.WEATHER)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWEATHER(v);
                        }
                        else if (arg.Type == EventScript.ArgType.FOG)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetFOG(v);
                        }
                        else if (arg.Type == EventScript.ArgType.UNIT_COLOR)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetUNIT_COLOR(v);
                        }
                        else if (arg.Type == EventScript.ArgType.EARTHQUAKE)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetEARTHQUAKE(v);
                        }
                        else if (arg.Type == EventScript.ArgType.ATTACK_TYPE)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetATTACK_TYPE(v);
                        }
                        else if (arg.Type == EventScript.ArgType.MENUCOMMAND)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetMENUCOMMAND(v);
                        }
                        else if (arg.Type == EventScript.ArgType.AI1)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetAI1(v);
                        }
                        else if (arg.Type == EventScript.ArgType.AI2)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetAI2(v);
                        }
                        else if (arg.Type == EventScript.ArgType.WMSTYLE1)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWMSTYLE1(v);
                        }
                        else if (arg.Type == EventScript.ArgType.WMSTYLE2)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWMSTYLE2(v);
                        }
                        else if (arg.Type == EventScript.ArgType.WMSTYLE3)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWMSTYLE3(v);
                        }
                        else if (arg.Type == EventScript.ArgType.WMREGION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWMREGION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.WMENREGION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWMENREGION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.AFFILIATION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetAFFILIATION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.WMAPAFFILIATION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWMAPAFFILIATION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.WMAP2AFFILIATION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetWMAP2AFFILIATION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.EVENTUNITPOS)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetEVENTUNITPOS(v);
                        }
                        else if (arg.Type == EventScript.ArgType.DIRECTION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetDIRECTION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.PORTRAIT_DIRECTION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetPORTRAIT_DIRECTION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.MAGVELY)
                        {//FE8の世界地図の移動 -8 ～ 52の範囲
                            isENumText = true;
                            string dummy;
                            text = " " + InputFormRef.GetMagvelYName((short)v, out dummy);
                        }
                        else if (arg.Type == EventScript.ArgType.MEMORYSLOT)
                        {//MEMORYSLOT
                            isENumText = true;
                            string dummy;
                            text = " " + InputFormRef.GetMEMORYSLOT(v, out dummy);
                        }
                        else if (arg.Type == EventScript.ArgType.PACKED_MEMORYSLOT)
                        {//MEMORYSLOT
                            isENumText = true;
                            string dummy;
                            text = " " + InputFormRef.GetPACKED_MEMORYSLOT(v, code.Script.Info[0], out dummy);
                        }
                        else if (arg.Type == EventScript.ArgType.EVBIT)
                        {//EVBIT
                            isENumText = true;
                            string dummy;
                            text = " " + InputFormRef.GetEVBIT(v, out dummy);
                        }
                        else if (arg.Type == EventScript.ArgType.EVBIT_MODIFY)
                        {//EVBIT_MODIFY
                            isENumText = true;
                            string dummy;
                            text = " " + InputFormRef.GetEVBIT_MODIFY(v, out dummy);
                        }
                        else if (arg.Type == EventScript.ArgType.BADSTATUS)
                        {//BADSTATUS
                            isENumText = true;
                            text = " " + InputFormRef.GetBADSTATUS(v);
                        }
                        else if (arg.Type == EventScript.ArgType.PROBABILITY)
                        {//確率
                            isENumText = true;
                            text = "%";
                        }
                        else if (arg.Type == EventScript.ArgType.RAM_UNIT_STATE)
                        {//RAM_UNIT_STATE
                            isENumText = true;
                            text = " " + InputFormRef.GetRAM_UNIT_STATE(v);
                        }
                        else if (arg.Type == EventScript.ArgType.MAPEMOTION)
                        {//MAPEMOTION
                            isENumText = true;
                            text = " " + InputFormRef.GetMAPEMOTION(v);
                        }
                        else if (arg.Type == EventScript.ArgType.DISABLEOPTIONS)
                        {//DISABLEOPTIONS
                            isENumText = true;
                            text = " " + InputFormRef.GetDISABLEOPTIONS(v);
                        }
                        else if (arg.Type == EventScript.ArgType.DISABLEWEAPONS)
                        {//DISABLEWEAPONS
                            isENumText = true;
                            text = " " + InputFormRef.GetDISABLEWEAPONS(v);
                        }
                        else if (arg.Type == EventScript.ArgType.UNITCLASSABILITY)
                        {//UNITCLASSABILITY
                            isENumText = true;
                            text = " " + InputFormRef.GetUNITCLASSABILITY(v);
                        }
                        else if (arg.Type == EventScript.ArgType.IGNORE_KEYS)
                        {//IGNORE_KEYS
                            isENumText = true;
                            text = " " + InputFormRef.GetIGNORE_KEYS(v);
                        }
                        else if (arg.Type == EventScript.ArgType.KEYS)
                        {//KEYS
                            isENumText = true;
                            text = " " + InputFormRef.GetPressKEYS(v);
                        }
                        else if (arg.Type == EventScript.ArgType.RAM_UNIT_PARAM)
                        {//RAMUNITSTATUS
                            isENumText = true;
                            string dummy;
                            text = " " + InputFormRef.GetRAM_UNIT_PARAM(v,out dummy);
                        }
                        else if (arg.Type == EventScript.ArgType.RAM_UNIT_VALUE)
                        {//RAM_UNIT_VALUE
                            isENumText = true;

                            uint prevIndex = EventScript.GetRAMUnitParamIndex(code);
                            uint prevValue = 0;
                            EventScript.GetArg(code, (int)prevIndex, out prevValue);
 
                            string dummy;
                            text = " " + InputFormRef.GetRAM_UNIT_VALUE(prevValue , v, out dummy);
                        }
                        else if (arg.Type == EventScript.ArgType.BOOL)
                        {//BOOL
                            isENumText = true;
                            text = " " + InputFormRef.GetBOOL(v);
                        }
                        else if (arg.Type == EventScript.ArgType.TRAP)
                        {//TRAP
                            isENumText = true;
                            text = " " + InputFormRef.GetTRAP(v);
                        }
                        else if (arg.Type == EventScript.ArgType.FSEC)
                        {//FSEC
                            isENumText = true;
                            text = " " + InputFormRef.GetFSEC(v);
                        }
                        else if (arg.Type == EventScript.ArgType.MAPXY)
                        {//MAPXY
                            isENumText = true;
                            text = " " + InputFormRef.GetMAPXY(v);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_AITILE)
                        {
                            isENumText = true;
                            text = " " + AITilesForm.GetNames(v);
                        }
                        else if (arg.Type == EventScript.ArgType.TILE)
                        {
                            isENumText = true;
                            text = " " + MapTerrainNameForm.GetNameExcept00(v);
                        }
                        else if (arg.Type == EventScript.ArgType.EDITION)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetEditon(v);
                        }
                        else if (arg.Type == EventScript.ArgType.DIFFICULTY)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetDifficulty(v);
                        }
                        else if (arg.Type == EventScript.ArgType.SUPPORT_LEVEL)
                        {
                            isENumText = true;
                            text = " " + InputFormRef.GetSuportLevel(v);
                        }
                        else if (arg.Type == EventScript.ArgType.GAMEOPTION)
                        {
                            isENumText = true;
                            text = " " + StatusOptionForm.GetNameIndex(v);
                        }
                        else if (arg.Type == EventScript.ArgType.GAMEOPTION_VALUE)
                        {
                            isENumText = true;

                            uint prevIndex = EventScript.GetGameOptionParamIndex(code);
                            uint prevValue = 0;
                            EventScript.GetArg(code, (int)prevIndex, out prevValue);

                            text = " " + StatusOptionForm.GetValueNameIndex(prevValue, v);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_AICOORDINATE)
                        {
                            isENumText = true;
                            text = " " + AIASMCoordinateForm.GetCoordPreview(v);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT4)
                        {
                            isENumText = true;
                            text = " " + AIASMUnit4Form.GetUnit4Preview(v);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_AICALLTALK)
                        {
                            isENumText = true;
                            text = " " + AIASMCALLTALKForm.GetUnit2Preview(v);
                        }

                        if (isENumText)
                        {
                            bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);
                        }
                    }

                    bounds.X += U.DrawText("]", g, normalFont, brush, isWithDraw, bounds);
                }

                if (i + 1 >= code.Script.Info.Length)
                {
                    break;
                }
                text = code.Script.Info[i + 1];
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }

            if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
            {//不明なコードの場合、LOWデータを出力する
                text = U.convertByteToStringDump(code.ByteData);
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }

            //現在1行でまだアイコンを書いていない場合
            if (maxHeight == lineHeight)
            {
                Bitmap bitmap = null;
                if (code.Script.Category.IndexOf("{GOLD}") >= 0)
                {//お金をもらうのはアイテムアイコン
                    bitmap = ImageSystemIconForm.WeaponIcon(8);
                }
                else if (code.Script.Category.IndexOf("{MUSIC}") >= 0)
                {//音楽カテゴリは効果音のアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(7);
                }
                else if (code.Script.Category.IndexOf("{PREPARATION}") >= 0)
                {//戦闘と進撃準備は剣のアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(10);
                }
                else if (code.Script.Category.IndexOf("{BATTLE}") >= 0)
                {//戦闘と進撃準備は剣のアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(10);
                }
                else if (code.Script.Category.IndexOf("{CC}") >= 0)
                {//CCは剣のアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(10);
                }
                else if (code.Script.Category.IndexOf("{FADE}") >= 0)
                {//フェードインアウトは 色補正のアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(8);
                }
                else if (code.Script.Category.IndexOf("{CAMERA}") >= 0)
                {//カメラではGBAのアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(9);
                }
                else if (code.Script.Category.IndexOf("{WAKU}") >= 0)
                {//枠ではカーソル枠
                    bitmap = ImageSystemIconForm.Cursol();
                }
                else if (code.Script.Category.IndexOf("{CLEAR}") >= 0)
                {//クリアなので、×カーソル
                    bitmap = ImageSystemIconForm.CursolDoubleCross();
                }
                else if (code.Script.Category.IndexOf("{WAIT}") >= 0)
                {//待機なので自動ターンオフのぐるぐるアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(12);
                }
                else if (code.Script.Category.IndexOf("{SUBROUTINE}") >= 0)
                {//サブルーチンは目立つ緑のアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(3);
                }
                else if (code.Script.Category.IndexOf("{WEATHER}") >= 0)
                {//天気は魔法の杖
                    bitmap = ImageSystemIconForm.WeaponIcon(4);
                }
                else if (code.Script.Category.IndexOf("{FLAG}") >= 0)
                {//フラグ
                    bitmap = ImageSystemIconForm.FlagIcon();
                }
                else if (code.Script.Category.IndexOf("{EVBIT}") >= 0)
                {//EVBIT
                    bitmap = ImageSystemIconForm.Stairs();
                }
                else if (code.Script.Category.IndexOf("{EVBIT_MODIFY}") >= 0)
                {//EVBIT_MODIFY
                    bitmap = ImageSystemIconForm.Stairs();
                }

                if (bitmap != null)
                {
                    U.MakeTransparent(bitmap);

                    Rectangle b = bounds;
                    b.Width = lineHeight * 2;
                    b.Height = lineHeight * 2;

                    bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                    bitmap.Dispose();
                    maxHeight = Math.Max(maxHeight, b.Height);
                }
            }

            if (longtext.Length > 0)
            {
                Rectangle b = bounds;
                b.Width = 330;
                b.Height = lineHeight * 2;

                Size bb = TextForm.DrawMini(longtext, lb, g, b, isWithDraw , true);
                maxHeight = Math.Max(maxHeight, b.Y + bb.Height);
                bounds.X += 330;
            }

            if (code.Comment != "")
            {
                SolidBrush commentBrush = new SolidBrush(OptionForm.Color_Comment_ForeColor());
                bounds.X += U.DrawText(" //" + code.Comment, g, normalFont, commentBrush, isWithDraw, bounds);
                commentBrush.Dispose();
            }

            brush.Dispose();
            boldFont.Dispose();

            bounds.Y += maxHeight;
            return new Size(bounds.X, bounds.Y);
        }
        public static Bitmap DrawUnitsList(uint units_address, int iconSize)
        {
            units_address = U.toOffset(units_address);
            if (!U.isSafetyOffset(units_address))
            {
                return ImageUtil.BlankDummy();
            }
            if (units_address == EventUnitForm.INVALIDATE_UNIT_POINTER)
            {//ユニット未設定
                return ImageUtil.BlankDummy();
            }

            int count = 0;
            uint addr = units_address;
            while (Program.ROM.u8(addr) != 0x0)
            {
                addr += Program.ROM.RomInfo.eventunit_data_size();
                if (!U.isSafetyOffset(addr))
                {
                    break;
                }
                count++;
            }
            if (count <= 0)
            {
                return ImageUtil.BlankDummy();
            }

            Bitmap bitmap = new Bitmap(iconSize * count, iconSize);
            Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                addr = units_address;
                for (int i = 0; i < count; i++)
                {
                    Bitmap icon = EventUnitForm.DrawUnitIconOnly(addr);
                    U.MakeTransparent(icon);

                    Rectangle b = bounds;
                    b.Width = iconSize;
                    b.Height = iconSize;

                    bounds.X += U.DrawPicture(icon, g, true, b);
                    addr += Program.ROM.RomInfo.eventunit_data_size();
                }
            }
            return bitmap;
        }
        public static void DirectJump(int jumpCount,ScriptEditSetTable[] table, EventHandler callback)
        {
            int jumpLabelCount = 0;
            for (int i = 0; i < table.Length; i++)
            {
                Label label = table[i].ParamLabel;
                if (label.Cursor == Cursors.Hand)
                {
                    if (jumpCount == jumpLabelCount)
                    {
                        callback(label, new EventArgs());
                        return;
                    }

                    jumpLabelCount++;
                }
            }

            if (jumpCount <= 0)
            {
                return;
            }
            //このパラメータはないけど、ひとつ前のはあるのかな?
            DirectJump(jumpCount - 1,table, callback);
        }

        //ポインタの更新の通知
        public void NotifyChangePointer(uint oldaddr, uint newaddr)
        {
            for (int i = 0; i < MainTab.TabCount; i++)
            {
                TabPage tab = MainTab.TabPages[i];
                for (int n = 0; n < tab.Controls.Count; n++)
                {
                    Control c = tab.Controls[n];
                    if (c is EventScriptInnerControl)
                    {
                        ((EventScriptInnerControl)c).NotifyChangePointer(oldaddr, newaddr);
                    }
                }
            }
        }


    }
}
