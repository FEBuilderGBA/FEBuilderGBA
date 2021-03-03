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
    public partial class EventScriptInnerControl : UserControl
    {
        public EventScriptInnerControl()
        {
            InitializeComponent();

            Popup = new EventScriptPopupUserControl();
            Popup.BorderStyle = BorderStyle.FixedSingle;
            Popup.Visible = false;
            this.Controls.Add(Popup);
            Popup.BringToFront();
            InputFormRef.ControlTranslate(Popup);


            this.AddressList.OwnerDraw(Draw, DrawMode.OwnerDrawVariable, false);

            ScriptEditSetTables = new EventScriptForm.ScriptEditSetTable[10];
            ScriptEditSetTables[0].ParamLabel = ParamLabel1; ScriptEditSetTables[0].ParamSrc = ParamSrc1; ScriptEditSetTables[0].ParamValue = ParamValue1; ScriptEditSetTables[0].ParamPicture = ParamImage1;
            ScriptEditSetTables[1].ParamLabel = ParamLabel2; ScriptEditSetTables[1].ParamSrc = ParamSrc2; ScriptEditSetTables[1].ParamValue = ParamValue2; ScriptEditSetTables[1].ParamPicture = ParamImage2;
            ScriptEditSetTables[2].ParamLabel = ParamLabel3; ScriptEditSetTables[2].ParamSrc = ParamSrc3; ScriptEditSetTables[2].ParamValue = ParamValue3; ScriptEditSetTables[2].ParamPicture = ParamImage3;
            ScriptEditSetTables[3].ParamLabel = ParamLabel4; ScriptEditSetTables[3].ParamSrc = ParamSrc4; ScriptEditSetTables[3].ParamValue = ParamValue4; ScriptEditSetTables[3].ParamPicture = ParamImage4;
            ScriptEditSetTables[4].ParamLabel = ParamLabel5; ScriptEditSetTables[4].ParamSrc = ParamSrc5; ScriptEditSetTables[4].ParamValue = ParamValue5; ScriptEditSetTables[4].ParamPicture = ParamImage5;
            ScriptEditSetTables[5].ParamLabel = ParamLabel6; ScriptEditSetTables[5].ParamSrc = ParamSrc6; ScriptEditSetTables[5].ParamValue = ParamValue6; ScriptEditSetTables[5].ParamPicture = ParamImage6;
            ScriptEditSetTables[6].ParamLabel = ParamLabel7; ScriptEditSetTables[6].ParamSrc = ParamSrc7; ScriptEditSetTables[6].ParamValue = ParamValue7; ScriptEditSetTables[6].ParamPicture = ParamImage7;
            ScriptEditSetTables[7].ParamLabel = ParamLabel8; ScriptEditSetTables[7].ParamSrc = ParamSrc8; ScriptEditSetTables[7].ParamValue = ParamValue8; ScriptEditSetTables[7].ParamPicture = ParamImage8;
            ScriptEditSetTables[8].ParamLabel = ParamLabel9; ScriptEditSetTables[8].ParamSrc = ParamSrc9; ScriptEditSetTables[8].ParamValue = ParamValue9; ScriptEditSetTables[8].ParamPicture = ParamImage9;
            ScriptEditSetTables[9].ParamLabel = ParamLabel10; ScriptEditSetTables[9].ParamSrc = ParamSrc10; ScriptEditSetTables[9].ParamValue = ParamValue10; ScriptEditSetTables[9].ParamPicture = ParamImage10;

            for (int i = 0; i < ScriptEditSetTables.Length; i++)
            {
                ScriptEditSetTables[i].ParamSrc.ValueChanged += ParamSrc_ValueChanged;
                ScriptEditSetTables[i].ParamSrc.Enter += ParamSrc_Focused;
                ScriptEditSetTables[i].ParamSrc.Leave += ParamSrc_UnFocused;
                ScriptEditSetTables[i].ParamSrc.DoubleClick += ParamSrc_Focused;

                ScriptEditSetTables[i].ParamLabel.Click += ParamLabel_Clicked;
                ScriptEditSetTables[i].ParamValue.DoubleClick += ParamLabel_Clicked;
            }

            HideFloatingControlpanel();

            InputFormRef.markupJumpLabel(AddressLabel);
            ReadStartAddress.Focus();
            ClearUndoBuffer();

            U.SetIcon(EventToFileButton, Properties.Resources.icon_arrow);
            U.SetIcon(FileToEventButton, Properties.Resources.icon_upload);
        }

        ToolTipEx ToolTip;
        public void Init(ToolTipEx toolTip, KeyEventHandler parentFormKeydownfunc)
        {
            this.ToolTip = toolTip;
            for (int i = 0; i < ScriptEditSetTables.Length; i++)
            {
                ScriptEditSetTables[i].ParamValue.SetToolTipEx(this.ToolTip);
            }
            //FE8だけテンプレートをメニューに出します
            bool useTemplate;
            if (Program.ROM.RomInfo.version() == 8)
            {
                useTemplate = true;
            }
            else
            {
                useTemplate = false;
                TemplateButton.Hide();
            }

            InputFormRef.MakeEditListboxContextMenuN(this.AddressList, this.AddressList_KeyDown, parentFormKeydownfunc ,useTemplate);
        }


        public EventHandler AddressListExpandsEvent;

        static uint GetScriptConditionalID(EventScript.OneCode code)
        {
            for (int i = 0; i < code.Script.Args.Length; i++)
            {
                EventScript.Arg arg = code.Script.Args[i];
                if (arg.Type == EventScript.ArgType.IF_CONDITIONAL
                    || arg.Type == EventScript.ArgType.GOTO_CONDITIONAL)
                {
                    uint v = EventScript.GetArgValue(code, arg);
                    return v;
                }
            }
            return U.NOT_FOUND;
        }
        static uint GetScriptLabelID(EventScript.OneCode code)
        {
            for (int i = 0; i < code.Script.Args.Length; i++)
            {
                EventScript.Arg arg = code.Script.Args[i];
                if (arg.Type == EventScript.ArgType.LABEL_CONDITIONAL)
                {
                    uint v = EventScript.GetArgValue(code, arg);
                    return v;
                }
            }
            return U.NOT_FOUND;
        }

        EventScriptPopupUserControl Popup;
        List<EventScript.OneCode> EventAsm;
        public static void JisageReorder(List<EventScript.OneCode> eventAsm)
        {
            List<uint> needLabel = new List<uint>();
            uint jisageCount = 0;
            bool isBeforeGoto = false;
            for (int i = 0; i < eventAsm.Count; i++)
            {
                EventScript.OneCode code = eventAsm[i];
                if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                {
                    if (jisageCount > 0)
                    {
                        uint cond_id = GetScriptLabelID(code);

                        needLabel.RemoveAll((uint x) => { return x == cond_id; });
                        if (needLabel.Count == 0)
                        {//必要なラベルをすべて探索し終わった。おそらく複雑な字下げが行われているのだろう.
                            jisageCount = 0;
                        }
                        else
                        {
                            jisageCount--;
                        }
                        code.JisageCount = jisageCount;
                    }
                    if (needLabel.Count != 0 && isBeforeGoto)
                    {//まだ探索しなければいけないラベルがあり、直前にgotoがあった場合
                        //おそらくこれはelse区です
                        jisageCount++;
                    }
                    isBeforeGoto = false;
                }
                else if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                {
                    code.JisageCount = jisageCount ++;
                    uint conditional_id = GetScriptConditionalID(code);
                    if (conditional_id != U.NOT_FOUND)
                    {
                        needLabel.Add(conditional_id);
                    }
                    isBeforeGoto = false;
                }
                else if (code.Script.Has == EventScript.ScriptHas.GOTO_CONDITIONAL)
                {
                    code.JisageCount = jisageCount;
                    uint conditional_id = GetScriptConditionalID(code);
                    if (conditional_id != U.NOT_FOUND)
                    {
                        needLabel.Add(conditional_id);
                    }
                    isBeforeGoto = true;
                }
                else
                {
                    code.JisageCount = jisageCount;
                    isBeforeGoto = false;
                }
            }
        }

        private void ReloadListButton_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer(); //リロードしたときはundoバッファクリアしてみるか.
            ReloadEvent();
        }

        void ReloadEvent()
        {
            HideFloatingControlpanel();

            this.EventAsm = new List<EventScript.OneCode>();

            uint addr = (uint)ReadStartAddress.Value;
            if (U.isPointer(addr))
            {
                addr = U.toOffset(addr);
                U.ForceUpdate(ReadStartAddress, addr);
            }
            if (!U.isSafetyOffset(addr))
            {
                this.AddressList.DummyAlloc(this.EventAsm.Count, this.AddressList.SelectedIndex);
                return;
            }

            //変更通知.
            if (Navigation != null)
            {
                NavigationEventArgs arg = new NavigationEventArgs();
                arg.Address = addr;
                Navigation(this, arg);
            }

            if (this.ReadCount.Value == 0 || this.IsLengthAutoChecked)
            {
                bool isWorldMapEvent = WorldMapEventPointerForm.isWorldMapEvent(addr);
                U.ForceUpdate(this.ReadCount, EventScript.SearchEveneLength(Program.ROM.Data,addr, isWorldMapEvent));
                //自動的に長さを求めました
                this.IsLengthAutoChecked = true;
            }


            //現在イベントのマップID
            this.MapID = FindMapID(addr);

            uint bytecount = (uint)ReadCount.Value;
            uint limit = Math.Min(addr + bytecount, (uint)Program.ROM.Data.Length);
            while (addr < limit)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                this.EventAsm.Add(code);

                addr += (uint)code.Script.Size;
            }
            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, this.AddressList.SelectedIndex);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            this.AddressList.Focus();
        }
        //システムが利用する関数を書き換える場合　警告を出す
        bool CheckSystemFunctionalEvent(uint addr)
        {
            List<Address> list = new List<Address>();
            EventScript.MakeEventASMMAPList(list, true, "", true);

            foreach (Address a in list)
            {
                if (a.Addr == addr)
                {
                    DialogResult dr = R.ShowNoYes("この領域は、システムが利用するイベント処理ルーチンとして予約されています。\r\nここを変更するとゲームシステムが不安定になる可能性が高いです。\r\n本当に変更してもよろしいですか？\r\n");
                    if (dr == DialogResult.Yes)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(null))
            {
                R.ShowStopError(InputFormRef.GetBusyErrorExplain());
                return;
            }
            if (this.EventAsm == null || this.EventAsm.Count <= 0)
            {
                R.ShowStopError("リストが空なので書き込めません");
                return;
            }
            Form parent_self = U.ControlToParentForm(this);
            if (InputFormRef.IsPleaseWaitDialog(parent_self))
            {//2重割り込み禁止
                return;
            }


            uint addr = (uint)ReadStartAddress.Value;
            if (U.isPointer(addr))
            {
                addr = U.toOffset(addr);
                ReadStartAddress.Value = addr;
            }
            if (!U.CheckZeroAddressWriteHigh(addr))
            {
                return;
            }
            if (!U.CheckPaddingALIGN4(addr))
            {
                return;
            }
            if (! CheckSystemFunctionalEvent(addr))
            {
                return;
            }

            bool isWorldMapEvent = WorldMapEventPointerForm.isWorldMapEvent(addr);
            bool isTopLevelEvent = false;
            if (isWorldMapEvent == false)
            {
                isTopLevelEvent = EventCondForm.isTopLevelEvent(addr);
            }

            bool hasTerm = false;
            List<byte> databyte = new List<byte>();
            for (int i = 0; i < this.EventAsm.Count; i++)
            {
                databyte.AddRange(this.EventAsm[i].ByteData);

                if (this.EventAsm[i].Script.Has == EventScript.ScriptHas.TERM)
                {
                    hasTerm = true;
                }
                if (isWorldMapEvent && this.EventAsm[i].Script.Has == EventScript.ScriptHas.MAPTERM)
                {
                    hasTerm = true;
                }
            }

            //終端コードがない場合は、一番下に追加.
            if (!hasTerm)
            {
                if (isWorldMapEvent)
                {
                    databyte.AddRange(Program.ROM.RomInfo.defualt_event_script_mapterm_code());
                }
                else if (isTopLevelEvent)
                {
                    databyte.AddRange(Program.ROM.RomInfo.defualt_event_script_toplevel_code());
                }
                else
                {
                    databyte.AddRange(Program.ROM.RomInfo.defualt_event_script_term_code());
                }
            }

            string undoname = this.Text + ":" + U.ToHexString(addr);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undoname);

            uint newaddr = InputFormRef.WriteBinaryData(parent_self
                , addr
                , databyte.ToArray()
                , get_data_pos_callback
                , undodata
            );
            Program.CommentCache.UpdateCode(newaddr ,(uint)ReadCount.Value, this.EventAsm);

            Program.Undo.Push(undodata);

            ReadStartAddress.Value = newaddr;
            this.IsLengthAutoChecked = true;

            FireAddressListExpandsEvent(addr, newaddr);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(parent_self, newaddr);

            ReloadEvent();
            //ReloadListButton_Click(null, null); //UndoBufferもクリアしてしまうので控える.
        }

        //拡張されていた場合イベントの送信.
        void FireAddressListExpandsEvent(uint addr,uint newaddr)
        {
            if (addr == newaddr)
            {//同一
                return;
            }

            if (this.AddressListExpandsEvent != null)
            {
                InputFormRef.ExpandsEventArgs eventarg = new InputFormRef.ExpandsEventArgs();
                eventarg.OldBaseAddress = addr;
                eventarg.BlockSize = 0;
                eventarg.OldDataCount = 0;
                eventarg.NewBaseAddress = newaddr;
                eventarg.NewDataCount = 0;
                this.AddressListExpandsEvent(this.AddressList, eventarg);
            }
        }

        private MoveToUnuseSpace.ADDR_AND_LENGTH get_data_pos_callback(uint addr)
        {
            bool isWorldMapEvent = WorldMapEventPointerForm.isWorldMapEvent(addr);
            uint length = EventScript.SearchEveneLength(Program.ROM.Data, addr, isWorldMapEvent);


            //範囲外探索 00 00 00 00 が続く限り検索してみる.
            uint more = Program.ROM.getBlockDataCount(addr + length, 4
                , (i, p) => { return Program.ROM.u32(p) == 0x00000000; }) * 4;

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = length + more;
            return aal;
        }



        EventScriptForm.ScriptEditSetTable[] ScriptEditSetTables;

        //自動的に長さを求めた場合 true / 手動で長さを求めている場合は true
        bool IsLengthAutoChecked = false;



        private void EventScriptForm_Load(object sender, EventArgs e)
        {
        }
        
        private Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.EventAsm.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            EventScript.OneCode code = this.EventAsm[index];
            return EventScriptForm.DrawCode(lb, g, listbounds, isWithDraw, code);
        }



        int ShowFloatingControlpanelInner(EventScript.OneCode code,int index)
        {
            //パラメータ数が少ない場合、ダイアログを上に引き揚げます.
            int heighest = ParamSrc9.Location.Y + ParamSrc9.Size.Height;
            if (code.Script.Args.Length < 3)
            {
                ControlPanelCommand.Location = new Point(0, ParamSrc3.Location.Y);
                ControlPanel.Height = heighest + ControlPanelCommand.Size.Height - ParamSrc9.Size.Height - ParamSrc7.Size.Height - ParamSrc5.Size.Height - ParamSrc3.Size.Height;
            }
            else if (code.Script.Args.Length < 5)
            {
                ControlPanelCommand.Location = new Point(0, ParamSrc5.Location.Y);
                ControlPanel.Height = heighest + ControlPanelCommand.Size.Height - ParamSrc9.Size.Height - ParamSrc7.Size.Height - ParamSrc5.Size.Height;
            }
            else if (code.Script.Args.Length < 7)
            {
                ControlPanelCommand.Location = new Point(0, ParamSrc7.Location.Y);
                ControlPanel.Height = heighest + ControlPanelCommand.Size.Height - ParamSrc9.Size.Height - ParamSrc7.Size.Height;
            }
            else if (code.Script.Args.Length < 9)
            {
                ControlPanelCommand.Location = new Point(0, ParamSrc7.Location.Y);
                ControlPanel.Height = heighest + ControlPanelCommand.Size.Height - ParamSrc9.Size.Height;
            }
            else
            {
                ControlPanelCommand.Location = new Point(0, heighest);
                ControlPanel.Height = heighest + ControlPanelCommand.Size.Height;
            }

            if (index < 0)
            {
                return 0;
            }

            //編集する項目の近くに移動させます.
            Rectangle rect = this.AddressList.GetItemRectangle(index);
            int y = this.MainPanel.Location.Y
                + this.AddressList.Location.Y
                + rect.Y + rect.Height + 20
                ;
            if (y + ControlPanel.Height >= AddressList.Height)
            {//下に余白がないので上に出す.
                y = this.MainPanel.Location.Y
                    + this.AddressList.Location.Y
                    + rect.Y
                    - ControlPanel.Height - 20;
                if (y < 0)
                {//上にも余白がないので、 Y = 0 の位置に出す
                    y = 0;
                }
            }
            return y;
        }


        void ShowFloatingControlpanel()
        {
            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.EventAsm.Count)
            {//一件もない
                this.ASMTextBox.Text = "";
                this.ScriptCodeName.Text = "";
                this.CommentTextBox.Text = "";
                this.AddressTextBox.Text = "";
                OneLineDisassembler();
            }
            else
            {
                EventScript.OneCode code = this.EventAsm[index];

                //コードを書く
                this.ASMTextBox.Text = U.convertByteToStringDump(code.ByteData);
                this.ScriptCodeName.Text = EventScript.makeCommandComboText(code.Script , false);
                this.CommentTextBox.Text = code.Comment;
                this.AddressTextBox.Text = U.ToHexString(EventScript.ConvertSelectedToAddr((uint)this.ReadStartAddress.Value, index, this.EventAsm));

                //Eventをデコードして、引数等を求めなおす.
                OneLineDisassembler();
                Debug.Assert( this.ASMTextBox.Text == U.convertByteToStringDump(code.ByteData));
            }
            //変更ボタンが光っていたら、それをやめさせる.
            InputFormRef.WriteButtonToYellow(this.UpdateButton, false);
            InputFormRef.WriteButtonToYellow(this.NewButton, false);

            ControlPanel.Show();

            //先頭のパラメータにフォーカスを移動する.
            EventScriptForm.SelectFirstParam(this.ScriptEditSetTables);

            //ホップアップが出てしまうと見栄えが悪いので消す.
            this.Popup.Hide();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
        }


        private void ASMTextBox_Leave(object sender, EventArgs e)
        {
            OneLineDisassembler();
        }


        private void OneLineDisassembler()
        {
            int i;
            if (this.ASMTextBox.Text.Length < 4)
            {//命令は最低でも4バイトでないとダメ.
                for(i = this.ASMTextBox.Text.Length ; i < 4 ; i++)
                {
                    this.ASMTextBox.Text += "0";
                }
            }

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);
            if (selectedByteData.Length < 4)
            {
                return;
            }
            string hint = this.ScriptCodeName.Text;

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.EventScript.DisAseemble(selectedByteData, 0, hint);

            //命令を選択.
            this.ScriptCodeName.Text = EventScript.makeCommandComboText(code.Script,false);

            //引数
            int n = 0;
            for (i = 0; i < ScriptEditSetTables.Length; i++, n++)
            {
                if (n >= code.Script.Args.Length)
                {
                    break;
                }
                EventScript.Arg arg = code.Script.Args[n];
                uint v = EventScript.GetArgValue(code, arg);

                if (EventScript.IsFixedArg(arg))
                {//固定長になっているところは入力できないようにする.
                    i--;
                    continue;
                }

                EventScriptForm.SetOneScriptEditSetTables(ScriptEditSetTables[i], arg, v);
            }

            for (; i < ScriptEditSetTables.Length; i++)
            {
                //使わないパラメータはあっかりーんする
                EventScriptForm.HideOneScriptEditSetTables(ScriptEditSetTables[i]);
            }

            int y = ShowFloatingControlpanelInner(code, this.AddressList.SelectedIndex);
            ControlPanel.Location = new Point(ControlPanel.Location.X, y);
        }

        bool Get_Select_ParamSrc_Object(object sender
            ,out EventScript.OneCode outCode //編集しているコード
            ,out int outSelectID //選択されたNumboxのindex(label等に関連づく)
            ,out int outArg  //引数の数(FIxed等非表示の引数があるので調べる)
            )
        {
            outCode = null;
            outSelectID = 0;
            outArg = 0;
            if (this.AddressList.SelectedIndex < 0 || this.AddressList.SelectedIndex >= this.EventAsm.Count)
            {
                return false;
            }

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);
            string hint = this.ScriptCodeName.Text;

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.EventScript.DisAseemble(selectedByteData, 0, hint);

            Control senderobject = ((Control)sender);

            int selectID = (int)U.atoi(senderobject.Name.Substring("ParamSrc".Length));
            if (selectID <= 0)
            {
                selectID = (int)U.atoi(senderobject.Name.Substring("ParamLabel".Length));
                if (selectID <= 0)
                {
                    selectID = (int)U.atoi(senderobject.Name.Substring("ParamImage".Length));
                    if (selectID <= 0)
                    {
                        selectID = (int)U.atoi(senderobject.Name.Substring("ParamValue".Length));
                        if (selectID <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            selectID = selectID - 1;
            EventScript.Arg arg;
            int i = 0;
            int n = 0;
            for (; i < code.Script.Args.Length; i++)
            {
                arg = code.Script.Args[i];
                if (EventScript.IsFixedArg(arg))
                {//固定長になっているところは入力できないようにする.
                    continue;
                }
                if (n < selectID)
                {
                    n++;
                    continue;
                }
                break;
            }

            if (i >= code.Script.Args.Length)
            {
                return false;
            }
            outCode = code; //編集しているコード
            outSelectID = selectID; //選択されたNumboxのindex(label等に関連づく)
            outArg = i; //引数の数(FIxed等非表示の引数があるので調べる)
            return true;
        }

        bool UpdateMAPXAndY(object sender, EventScript.Arg arg, int selectID, ref bool ref_isOrderOfHuman)
        {
            NumericUpDown xObj;
            NumericUpDown yObj;
            if (arg.Type == EventScript.ArgType.MAPX
                || arg.Type == EventScript.ArgType.SCREENX
                || arg.Type == EventScript.ArgType.WMAPX)
            {
                if (selectID + 1 >= ScriptEditSetTables.Length)
                {
                    return false;
                }
                xObj = (NumericUpDown)sender;
                yObj = ScriptEditSetTables[selectID + 1].ParamSrc;
            }
            else
            {
                if (selectID <= 0 || selectID - 1 >= ScriptEditSetTables.Length)
                {
                    return false;
                }
                xObj = ScriptEditSetTables[selectID - 1].ParamSrc;
                yObj = (NumericUpDown)sender;
            }

            if (ref_isOrderOfHuman == false)
            {
                //隣のフォームから入れている場合は、人間の操作といえるだろう
                ref_isOrderOfHuman = (xObj == sender || yObj == sender);
            }

            if (ref_isOrderOfHuman)
            {
                Popup.MAP.SetPoint(this.AddressList.SelectedIndex.ToString(), (int)xObj.Value, (int)yObj.Value);
            }

            return true;
        }

        private void ParamSrc_ValueChanged(object sender, EventArgs e)
        {
            EventScript.OneCode code;
            int selectID;
            int argindex;
            if (!Get_Select_ParamSrc_Object(sender, out code, out selectID, out argindex))
            {
                return;
            }
            if (selectID < 0 || selectID > ScriptEditSetTables.Length)
            {
                return;
            }

            EventScript.Arg arg = code.Script.Args[argindex];
            uint v = EventScriptForm.WriteOneScriptEditSetTables(ScriptEditSetTables[selectID], arg, code);
            EventScriptForm.WriteAliasScriptEditSetTables(ScriptEditSetTables[selectID], arg, code);

            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            string text = "";
            string errormessage = "";
            Bitmap image = null;
            Bitmap backgroundImage = null;
            if (arg.Type == EventScript.ArgType.TEXT)
            {
                text = TextForm.DirectAndStripAllCode((v));
            }
            else if (arg.Type == EventScript.ArgType.CONVERSATION_TEXT)
            {
                text = TextForm.Direct((v));
                errormessage = TextForm.CheckConversationTextMessage(text, TextForm.MAX_SERIF_WIDTH);
                text = TextForm.StripAllCode(text);
            }
            else if (arg.Type == EventScript.ArgType.SYSTEM_TEXT)
            {
                text = TextForm.Direct(v);
                errormessage = TextForm.CheckSystemTextMessage(text);
                text = TextForm.StripAllCode(text);
            }
            else if (arg.Type == EventScript.ArgType.ONELINE_TEXT)
            {
                text = TextForm.Direct(v);
                errormessage = TextForm.CheckOneLineTextMessage(text, 100 * 8, 1 * 16 , false);
                text = TextForm.StripAllCode(text);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_TEXT)
            {
                text = TextForm.Direct(v);
                errormessage = TextForm.CheckOneLineTextMessage(text, 100 * 8, 1 * 16, false);
                text = TextForm.StripAllCode(text);
            }
            else if (arg.Type == EventScript.ArgType.UNIT)
            {
                text = UnitForm.GetUnitName(v);
                image = UnitForm.DrawUnitMapFacePicture(v);
            }
            else if (arg.Type == EventScript.ArgType.CLASS)
            {
                text = ClassForm.GetClassName(v);
                image = ClassForm.DrawWaitIcon(v);
            }
            else if (arg.Type == EventScript.ArgType.BG && this.ActiveControl == sender)
            {
                image = ImageBGForm.DrawBG(v);
                if (isOrderOfHuman)
                {
                    PopupDialog_Image((NumericUpDown)sender, image);
                }
            }
            else if (arg.Type == EventScript.ArgType.CG && this.ActiveControl == sender)
            {
                image = ImageCGForm.DrawImageByID(v);
                if (isOrderOfHuman)
                {
                    PopupDialog_Image((NumericUpDown)sender, image);
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_UNIT)
            {
                if (U.toOffset(v) == EventUnitForm.INVALIDATE_UNIT_POINTER)
                {
                    text = R._("読み込むユニットを設定してください");
                }
                else
                {
                    backgroundImage = EventScriptForm.DrawUnitsList(v, ScriptEditSetTables[selectID].ParamValue.Height - 2);
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_MENUEXTENDS)
            {
                if (U.toOffset(v) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = MenuDefinitionForm.MakeMenuPreview(U.toOffset(v));
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_UNITSSHORTTEXT)
            {//unitに関連付けられたshort型データ
                if (U.toOffset(v) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = U.ToHexString(v);
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICOORDINATE)
            {
                if (U.toOffset(v) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = AIASMCoordinateForm.GetCoordPreview(U.toOffset(v));
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT4)
            {
                if (U.toOffset(v) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = AIASMUnit4Form.GetUnit4Preview(U.toOffset(v));
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICALLTALK)
            {
                if (U.toOffset(v) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = AIASMCALLTALKForm.GetUnit2Preview(U.toOffset(v));
                }
            }
            else if ((arg.Type == EventScript.ArgType.PORTRAIT || arg.Type == EventScript.ArgType.REVPORTRAIT) && this.ActiveControl == sender)
            {
                image = ImagePortraitForm.DrawPortraitAuto(v);
                if (arg.Type == EventScript.ArgType.REVPORTRAIT)
                {
                    image.RotateFlip(RotateFlipType.Rotate180FlipY);
                }
                if (isOrderOfHuman)
                {
                    PopupDialog_Image((NumericUpDown)sender, image);
                }
            }
            else if (arg.Type == EventScript.ArgType.ITEM)
            {
                text = ItemForm.GetItemName(v);
                image = ItemForm.DrawIcon(v);
            }
            else if (arg.Type == EventScript.ArgType.MUSIC || arg.Type == EventScript.ArgType.SOUND)
            {
                text = SongTableForm.GetSongName(v);
                if (isOrderOfHuman)
                {
                    PopupDialog_Music((NumericUpDown)sender, v);
                }
            }
            else if (arg.Type == EventScript.ArgType.FOG)
            {
                text = InputFormRef.GetFOG(v);
            }
            else if (arg.Type == EventScript.ArgType.WEATHER)
            {
                text = InputFormRef.GetWEATHER(v);
            }
            else if (arg.Type == EventScript.ArgType.UNIT_COLOR)
            {
                text = InputFormRef.GetUNIT_COLOR(v);
            }
            else if (arg.Type == EventScript.ArgType.EARTHQUAKE)
            {
                text = InputFormRef.GetEARTHQUAKE(v);
            }
            else if (arg.Type == EventScript.ArgType.ATTACK_TYPE)
            {
                text = InputFormRef.GetATTACK_TYPE(v);
            }
            else if (arg.Type == EventScript.ArgType.MENUCOMMAND)
            {
                text = InputFormRef.GetMENUCOMMAND(v);
            }
            else if (arg.Type == EventScript.ArgType.AI1)
            {
                text = InputFormRef.GetAI1(v);
            }
            else if (arg.Type == EventScript.ArgType.AI2)
            {
                text = InputFormRef.GetAI2(v);
            }
            else if (arg.Type == EventScript.ArgType.CG)
            {
                text = InputFormRef.GetCGComment(v);
            }
            else if (arg.Type == EventScript.ArgType.BG)
            {
                text = ImageBGForm.GetComment(v);
            }
            else if (arg.Type == EventScript.ArgType.WMSTYLE1)
            {
                text = InputFormRef.GetWMSTYLE1(v);
            }
            else if (arg.Type == EventScript.ArgType.WMSTYLE2)
            {
                text = InputFormRef.GetWMSTYLE2(v);
            }
            else if (arg.Type == EventScript.ArgType.WMSTYLE3)
            {
                text = InputFormRef.GetWMSTYLE3(v);
            }
            else if (arg.Type == EventScript.ArgType.WMREGION)
            {
                text = InputFormRef.GetWMREGION(v);
            }
            else if (arg.Type == EventScript.ArgType.WMENREGION)
            {
                text = InputFormRef.GetWMENREGION(v);
            }
            else if (arg.Type == EventScript.ArgType.AFFILIATION)
            {
                text = InputFormRef.GetAFFILIATION(v);
            }
            else if (arg.Type == EventScript.ArgType.WMAPAFFILIATION)
            {
                text = InputFormRef.GetWMAPAFFILIATION(v);
            }
            else if (arg.Type == EventScript.ArgType.WMAP2AFFILIATION)
            {
                text = InputFormRef.GetWMAP2AFFILIATION(v);
            }
            else if (arg.Type == EventScript.ArgType.EVENTUNITPOS)
            {
                text = InputFormRef.GetEVENTUNITPOS(v);
            }
            else if (arg.Type == EventScript.ArgType.DIRECTION)
            {
                text = InputFormRef.GetDIRECTION(v);
            }
            else if (arg.Type == EventScript.ArgType.PORTRAIT_DIRECTION)
            {
                text = " " + InputFormRef.GetPORTRAIT_DIRECTION(v);
            }
            else if (arg.Type == EventScript.ArgType.WMLOCATION)
            {//ワールドマップの名前
                text = WorldMapPointForm.GetWorldMapPointName(v);
            }
            else if (arg.Type == EventScript.ArgType.WMPATH)
            {//ワールドマップの道
                text = WorldMapPathForm.GetPathName(v);
            }
            else if (arg.Type == EventScript.ArgType.MAPCHAPTER)
            {
                text = MapSettingForm.GetMapName(v);
                if (isOrderOfHuman)
                {
                    PopupDialog_MAP((NumericUpDown)sender, v);
                }
            }
            else if (arg.Type == EventScript.ArgType.MAP_CHANGE)
            {
                text = InputFormRef.GetMapChangeName(this.MapID, v);
                if (isOrderOfHuman)
                {
                    uint mapid = ScanMAPID();
                    PopupDialog_MAPAndChange((NumericUpDown)sender, mapid , v);
                }
            }
            else if (arg.Type == EventScript.ArgType.MAPX || arg.Type == EventScript.ArgType.MAPY
                ||   arg.Type == EventScript.ArgType.SCREENX || arg.Type == EventScript.ArgType.SCREENY
                ||   arg.Type == EventScript.ArgType.WMAPX || arg.Type == EventScript.ArgType.WMAPY)
            {
                if (! UpdateMAPXAndY(sender, arg, selectID, ref isOrderOfHuman))
                {
                    return;
                }
            }
            else if (arg.Type == EventScript.ArgType.MAPXY)
            {
                text = InputFormRef.GetMAPXY(v);
                if (isOrderOfHuman)
                {
                    NumericUpDown xyObj = (NumericUpDown)sender;
                    Popup.MAP.SetPoint(this.AddressList.SelectedIndex.ToString(), (int)U.ParsePosX((uint)xyObj.Value), (int)U.ParsePosY((uint)xyObj.Value));
                }
            }
            else if (arg.Type == EventScript.ArgType.FLAG)
            {//フラグ
                text = InputFormRef.GetFlagName(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.MAGVELY)
            {//FE8の世界地図の移動 -8 ～ 52の範囲
                text = InputFormRef.GetMagvelYName((short)v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_PROCS)
            {//PROC
                text = Program.AsmMapFileAsmCache.GetASMName(v, true, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_ASM)
            {//ASM
                text = Program.AsmMapFileAsmCache.GetASMName(v, false, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_EVENT)
            {//EVENT
                text = Program.AsmMapFileAsmCache.GetEventName(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.MEMORYSLOT)
            {//MemorySlot
                text = " " + InputFormRef.GetMEMORYSLOT(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.COUNTER)
            {//Counter
                text = " " + InputFormRef.GetCOUNTER(v, out errormessage);
            }
            if (arg.Type == EventScript.ArgType.TILE)
            {//タイル名
                text = " " + MapTerrainNameForm.GetName(v);
            }
            else if (arg.Type == EventScript.ArgType.PACKED_MEMORYSLOT)
            {//MemorySlotPacked
                text = " " + InputFormRef.GetPACKED_MEMORYSLOT(v, code.Script.Info[0], out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.RAM_UNIT_STATE)
            {//RAM_UNIT_STATE
                text = " " + InputFormRef.GetRAM_UNIT_STATE(v);
            }
            else if (arg.Type == EventScript.ArgType.MAPEMOTION)
            {//MAPEMOTION
                text = " " + InputFormRef.GetMAPEMOTION(v);
            }
            else if (arg.Type == EventScript.ArgType.DISABLEOPTIONS)
            {//DISABLEOPTIONS
                text = " " + InputFormRef.GetDISABLEOPTIONS(v);
            }
            else if (arg.Type == EventScript.ArgType.DISABLEWEAPONS)
            {//DISABLEWEAPONS
                text = " " + InputFormRef.GetDISABLEWEAPONS(v);
            }
            else if (arg.Type == EventScript.ArgType.IGNORE_KEYS)
            {//IGNORE_KEYS
                text = " " + InputFormRef.GetIGNORE_KEYS(v);
            }
            else if (arg.Type == EventScript.ArgType.KEYS)
            {//KEYS
                text = " " + InputFormRef.GetPressKEYS(v);
            }
            else if (arg.Type == EventScript.ArgType.FSEC)
            {//FSEC
                text = " " + InputFormRef.GetFSEC(v);
            }
            else if (arg.Type == EventScript.ArgType.MAPXY)
            {//MAPXY
                text = " " + InputFormRef.GetMAPXY(v);
            }
            else if (arg.Type == EventScript.ArgType.RAM_UNIT_PARAM)
            {//RAM_UNIT_PARAM
                text = " " + InputFormRef.GetRAM_UNIT_PARAM(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.RAM_UNIT_VALUE)
            {//RAM_UNIT_PARAM
                uint prevIndex = EventScript.GetRAMUnitParamIndex(code);
                uint prevValue = 0;
                EventScript.GetArg(code, (int)prevIndex, out prevValue);

                text = " " + InputFormRef.GetRAM_UNIT_VALUE(prevValue, v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.BOOL)
            {//BOOL
                text = " " + InputFormRef.GetBOOL(v);
            }
            else if (arg.Type == EventScript.ArgType.TRAP)
            {//TRAP
                text = " " + InputFormRef.GetTRAP(v);
            }
            else if (arg.Type == EventScript.ArgType.WMAP_SPRITE_ID)
            {//WMAP_SPRITE_ID
                text = " " + InputFormRef.GetWMAP_SPRITE_ID(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.EVBIT)
            {//EVBIT
                text = " " + InputFormRef.GetEVBIT(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.EVBIT_MODIFY)
            {//EVBIT_MODIFY
                text = " " + InputFormRef.GetEVBIT_MODIFY(v, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.BADSTATUS)
            {//BADSTATUS
                text = " " + InputFormRef.GetBADSTATUS(v);
            }
            else if (arg.Type == EventScript.ArgType.SKILL)
            {//SKILL
                image = InputFormRef.DrawSkillIcon(v);
                text = " " + InputFormRef.GetSkillName(v);
            }
            else if (arg.Type == EventScript.ArgType.EDITION)
            {
                text = " " + InputFormRef.GetEditon(v);
            }
            else if (arg.Type == EventScript.ArgType.DIFFICULTY)
            {
                text = " " + InputFormRef.GetDifficulty(v);
            }
            else if (arg.Type == EventScript.ArgType.SUPPORT_LEVEL)
            {
                text = " " + InputFormRef.GetSuportLevel(v);
            }
            else if (arg.Type == EventScript.ArgType.GAMEOPTION)
            {
                image = StatusOptionForm.DrawIcon(v);
                text = " " + StatusOptionForm.GetNameIndex(v);
            }
            else if (arg.Type == EventScript.ArgType.GAMEOPTION_VALUE)
            {
                uint prevIndex = EventScript.GetGameOptionParamIndex(code);
                uint prevValue = 0;
                EventScript.GetArg(code, (int)prevIndex, out prevValue);

                text = " " + StatusOptionForm.GetValueNameIndex(prevValue, v);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICOORDINATE)
            {
                text = " " + AIASMCoordinateForm.GetCoordPreview(v);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT4)
            {
                text = " " + AIASMUnit4Form.GetUnit4Preview(v);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICALLTALK)
            {
                text = " " + AIASMCALLTALKForm.GetUnit2Preview(v);
            }
            else if (arg.Type == EventScript.ArgType.COUNTER)
            {//Counter
                text = " " + InputFormRef.GetCOUNTER(v, out errormessage);
            }
            if (arg.Type == EventScript.ArgType.TILE)
            {//タイル名
                text = " " + MapTerrainNameForm.GetName(v);
            }

            else if (arg.Type == EventScript.ArgType.None)
            {//10進数表記を書いてやる.
                text = " " + InputFormRef.GetDigitHint(v);
            }

            U.MakeTransparent(image);
            ScriptEditSetTables[selectID].ParamValue.ErrorMessage = errormessage;
            ScriptEditSetTables[selectID].ParamValue.BackgroundImage = backgroundImage;
            ScriptEditSetTables[selectID].ParamValue.Text = text;
            ScriptEditSetTables[selectID].ParamPicture.Image = image;
            this.ASMTextBox.Text = U.convertByteToStringDump(code.ByteData);

            if (isOrderOfHuman)
            {//現在このコントロールから値を入力している場合は、他のコントロールも連動して変更する

                //変更があったので、変更ボタンを光らせる.
                InputFormRef.WriteButtonToYellow(this.UpdateButton, true);
                InputFormRef.WriteButtonToYellow(this.NewButton, true);
            }
        }

        private void ParamLabel_Clicked(object sender, EventArgs e)
        {
            EventScript.OneCode code;
            int selectID;
            int argindex;
            if (!Get_Select_ParamSrc_Object(sender, out code, out selectID, out argindex))
            {
                return;
            }

            NumericUpDown src_object = ScriptEditSetTables[selectID].ParamSrc;

            uint value = (uint)src_object.Value;
            EventScript.Arg arg = code.Script.Args[argindex];
            if (arg.Type == EventScript.ArgType.TEXT
                || arg.Type == EventScript.ArgType.CONVERSATION_TEXT
                || arg.Type == EventScript.ArgType.SYSTEM_TEXT
                || arg.Type == EventScript.ArgType.ONELINE_TEXT
                || arg.Type == EventScript.ArgType.POINTER_TEXT
                )
            {
                TextForm f = (TextForm)InputFormRef.JumpForm<TextForm>(U.NOT_FOUND);
                MakeInjectionCallback(f
                    , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                        (InputFormRef.GetAllControls(f), "AddressList")
                    , src_object, false);
                f.JumpTo(value);
            }
            else if (arg.Type == EventScript.ArgType.UNIT)
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    UnitForm f = (UnitForm)InputFormRef.JumpForm<UnitForm>(value - 1);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
                else if (Program.ROM.RomInfo.version() >= 7)
                {//FE7
                    UnitFE7Form f = (UnitFE7Form)InputFormRef.JumpForm<UnitFE7Form>(value - 1);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
                else
                {//FE6
                    UnitFE6Form f = (UnitFE6Form)InputFormRef.JumpForm<UnitFE6Form>(value - 1);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.CLASS)
            {
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 - FE8
                    ClassForm f = (ClassForm)InputFormRef.JumpForm<ClassForm>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
                else
                {//FE6
                    ClassFE6Form f = (ClassFE6Form)InputFormRef.JumpForm<ClassFE6Form>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.BG)
            {
                ImageBGForm f = (ImageBGForm)InputFormRef.JumpForm<ImageBGForm>(value);
                MakeInjectionCallback(f
                    , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                        (InputFormRef.GetAllControls(f), "AddressList")
                    , src_object, false);
            }
            else if (arg.Type == EventScript.ArgType.CG)
            {
                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
                {//FE7U
                    ImageCGFE7UForm f = (ImageCGFE7UForm)InputFormRef.JumpForm<ImageCGFE7UForm>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
                else
                {
                    ImageCGForm f = (ImageCGForm)InputFormRef.JumpForm<ImageCGForm>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.ITEM)
            {
                ItemForm f = (ItemForm)InputFormRef.JumpForm<ItemForm>(value);
                MakeInjectionCallback(f
                    , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                        (InputFormRef.GetAllControls(f), "AddressList")
                    , src_object, false);
            }
            else if (arg.Type == EventScript.ArgType.MUSIC)
            {
                SongTableForm f = (SongTableForm)InputFormRef.JumpForm<SongTableForm>(value);
                MakeInjectionCallback(f
                    , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                        (InputFormRef.GetAllControls(f), "AddressList")
                    , src_object, false);
            }
            else if (arg.Type == EventScript.ArgType.SOUND)
            {
                SongTableForm f = (SongTableForm)InputFormRef.JumpForm<SongTableForm>(value);
                MakeInjectionCallback(f
                    , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                        (InputFormRef.GetAllControls(f), "AddressList")
                    , src_object, false);
            }
            else if (arg.Type == EventScript.ArgType.MAPCHAPTER)
            {
                MapSettingForm f = (MapSettingForm)InputFormRef.JumpForm<MapSettingForm>(value);
                MakeInjectionCallback(f
                    , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                        (InputFormRef.GetAllControls(f), "AddressList")
                    , src_object, false);
            }
            else if (arg.Type == EventScript.ArgType.PORTRAIT || arg.Type == EventScript.ArgType.REVPORTRAIT)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    ImagePortraitFE6Form f = (ImagePortraitFE6Form)InputFormRef.JumpForm<ImagePortraitFE6Form>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
                else
                {
                    ImagePortraitForm f = (ImagePortraitForm)InputFormRef.JumpForm<ImagePortraitForm>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.WMLOCATION)
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    WorldMapPointForm f = (WorldMapPointForm)InputFormRef.JumpForm<WorldMapPointForm>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.WMPATH)
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    WorldMapPathForm f = (WorldMapPathForm)InputFormRef.JumpForm<WorldMapPathForm>(value);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "AddressList")
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_UNIT)
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    EventUnitForm f = (EventUnitForm)InputFormRef.JumpForm<EventUnitForm>(U.NOT_FOUND);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "EVENT_LISTBOX")
                        , src_object, true);
                    f.MakeAddressListExpandsCallback(MakeAddressListExpandsCallback_Handler(src_object));
                    if (U.toOffset(value) == EventUnitForm.INVALIDATE_UNIT_POINTER)
                    {
                        f.JumpToMap(this.MapID);
                    }
                    else
                    {
                        f.JumpTo(value);
                    }
                }
                else if (Program.ROM.RomInfo.version() >= 7)
                {//FE7
                    EventUnitFE7Form f = (EventUnitFE7Form)InputFormRef.JumpForm<EventUnitFE7Form>(U.NOT_FOUND);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "EVENT_LISTBOX")
                        , src_object, true);
                    f.MakeAddressListExpandsCallback(MakeAddressListExpandsCallback_Handler(src_object));
                    if (U.toOffset(value) == EventUnitForm.INVALIDATE_UNIT_POINTER)
                    {
                        f.JumpToMap(this.MapID);
                    }
                    else
                    {
                        f.JumpTo(value);
                    }
                }
                else
                {//FE6
                    EventUnitFE6Form f = (EventUnitFE6Form)InputFormRef.JumpForm<EventUnitFE6Form>(U.NOT_FOUND);
                    MakeInjectionCallback(f
                        , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                            (InputFormRef.GetAllControls(f), "EVENT_LISTBOX")
                        , src_object, true);
                    f.MakeAddressListExpandsCallback(MakeAddressListExpandsCallback_Handler(src_object));
                    if (U.toOffset(value) == EventUnitForm.INVALIDATE_UNIT_POINTER)
                    {
                        f.JumpToMap(this.MapID);
                    }
                    else
                    {
                        f.JumpTo(value);
                    }
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_EVENT)
            {
                if (Navigation != null)
                {
                    NavigationEventArgs navi_arg = new NavigationEventArgs();
                    navi_arg.IsNewTab = true;
                    navi_arg.Address = value;
                    Navigation(this, navi_arg);
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_PROCS)
            {
                ProcsScriptForm f = (ProcsScriptForm)InputFormRef.JumpForm<ProcsScriptForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_ASM)
            {
                DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>(U.NOT_FOUND);
                f.JumpTo(DisassemblerTrumb.ProgramAddrToPlain(value));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_EVENTBATTLEDATA)
            {
                EventBattleDataFE7Form f = (EventBattleDataFE7Form)InputFormRef.JumpForm<EventBattleDataFE7Form>(U.NOT_FOUND);
                value = f.AllocIfNeed(src_object);
                f.MakeAddressListExpandsCallback(MakeAddressListExpandsCallback_Handler(src_object));
                f.JumpToAddr(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_EVENTMOVEDATA)
            {
                EventMoveDataFE7Form f = (EventMoveDataFE7Form)InputFormRef.JumpForm<EventMoveDataFE7Form>(U.NOT_FOUND);
                value = f.AllocIfNeed(src_object);
                f.MakeAddressListExpandsCallback(MakeAddressListExpandsCallback_Handler(src_object));
                f.JumpToAddr(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_TALKGROUP)
            {
                EventTalkGroupFE7Form f = (EventTalkGroupFE7Form)InputFormRef.JumpForm<EventTalkGroupFE7Form>(U.NOT_FOUND);
                value = f.AllocIfNeed(src_object);
                f.MakeAddressListExpandsCallback(MakeAddressListExpandsCallback_Handler(src_object));
                f.JumpToAddr(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_MENUEXTENDS)
            {
                MenuExtendSplitMenuForm f = (MenuExtendSplitMenuForm)InputFormRef.JumpForm<MenuExtendSplitMenuForm>(U.NOT_FOUND);
                value = f.AllocIfNeed(src_object);
                f.JumpToAddr(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICOORDINATE)
            {
                AIASMCoordinateForm f = (AIASMCoordinateForm)InputFormRef.JumpFormLow<AIASMCoordinateForm>();
                value = f.AllocIfNeed(src_object);
                f.JumpToAddr(value);
                f.ShowDialog();
                U.SetActiveControl(src_object);
                U.ForceUpdate(src_object, U.toPointer(f.GetBaseAddress()));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT4)
            {
                AIASMUnit4Form f = (AIASMUnit4Form)InputFormRef.JumpFormLow<AIASMUnit4Form>();
                value = f.AllocIfNeed(src_object);
                f.JumpToAddr(value);
                f.ShowDialog();
                U.SetActiveControl(src_object);
                U.ForceUpdate(src_object, U.toPointer(f.GetBaseAddress()));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICALLTALK)
            {
                AIASMCALLTALKForm f = (AIASMCALLTALKForm)InputFormRef.JumpFormLow<AIASMCALLTALKForm>();
                value = f.AllocIfNeed(src_object);
                f.JumpToAddr(value);
                f.ShowDialog();
                U.SetActiveControl(src_object);
                U.ForceUpdate(src_object, U.toPointer(f.GetBaseAddress()));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_UNITSSHORTTEXT)
            {
                UnitsShortTextForm f = (UnitsShortTextForm)InputFormRef.JumpFormLow<UnitsShortTextForm>();
                value = f.AllocIfNeed(src_object);
                f.JumpTo(value);
                f.ShowDialog();
                U.SetActiveControl(src_object);
                U.ForceUpdate(src_object, U.toPointer(f.GetBaseAddress()));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_TEXT)
            {
                CStringForm f = (CStringForm)InputFormRef.JumpForm<CStringForm>();
                f.Init(src_object);
            }
            else if (arg.Type == EventScript.ArgType.FLAG)
            {
                ToolFlagNameForm f = (ToolFlagNameForm)InputFormRef.JumpForm<ToolFlagNameForm>(U.NOT_FOUND);
                f.JumpTo(value, MakeAddressListFlagExpandsCallback_Handler(src_object) , this.MapID);
            }
            else if (arg.Type == EventScript.ArgType.RAM_UNIT_STATE)
            {
                RAMUnitStateFlagForm f = (RAMUnitStateFlagForm)InputFormRef.JumpForm<RAMUnitStateFlagForm>(U.NOT_FOUND);
                f.JumpTo(value);
                MakeInjectionApplyButtonCallback(f
                    , (Button)InputFormRef.FindObjectByForm<Button>
                        (InputFormRef.GetAllControls(f), "ApplyButton")
                    , src_object);
            }
            else if (arg.Type == EventScript.ArgType.GAMEOPTION)
            {
                StatusOptionForm f = (StatusOptionForm)InputFormRef.JumpForm<StatusOptionForm>();
                MakeInjectionCallback(f
                    , (ListBox)InputFormRef.FindObjectByForm<ListBox>
                        (InputFormRef.GetAllControls(f), "AddressList")
                    , src_object, false);
            }
            else if (arg.Type == EventScript.ArgType.DISABLEOPTIONS
                  || arg.Type == EventScript.ArgType.DISABLEWEAPONS
                  || arg.Type == EventScript.ArgType.IGNORE_KEYS
                  || arg.Type == EventScript.ArgType.KEYS
                  || arg.Type == EventScript.ArgType.ATTACK_TYPE
                )
            {
                UshortBitFlagForm f = (UshortBitFlagForm)InputFormRef.JumpForm<UshortBitFlagForm>();
                f.JumpTo(arg.Type,value);
                MakeInjectionApplyButtonCallback(f
                    , (Button)InputFormRef.FindObjectByForm<Button>
                        (InputFormRef.GetAllControls(f), "ApplyButton")
                    , src_object);
            }
        }

        EventHandler MakeAddressListExpandsCallback_Handler(NumericUpDown injectionCallback)
        {
            EventHandler eventHandler = 
            (object sender, EventArgs e) =>
            {
                ListBox lb = (ListBox)sender;
                InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)e;
                uint addr = eearg.NewBaseAddress;

                injectionCallback.Focus(); //フォーカスの是非を見ているので一度フォーカス一度が必要.
                U.ForceUpdate(injectionCallback, U.toPointer(addr));

                HideFloatingControlpanel(); //popアップがでることがあるので黙らせる. 

                lb.Focus(); //フォーカスを戻す.
            };
            return eventHandler;
        }
        EventHandler MakeAddressListFlagExpandsCallback_Handler(NumericUpDown injectionCallback)
        {
            EventHandler eventHandler =
            (object sender, EventArgs e) =>
            {
                ListBox lb = (ListBox)sender;
                InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)e;
                uint addr = eearg.NewBaseAddress;

                injectionCallback.Focus(); //フォーカスの是非を見ているので一度フォーカス一度が必要.
                U.ForceUpdate(injectionCallback, addr);
                HideFloatingControlpanel(); //popアップがでることがあるので黙らせる. 

                lb.Focus(); //フォーカスを戻す.
            };
            return eventHandler;
        }

        //adresslist doubleclickで、呼び出し元の injecttionCallbackへ値を代入. フォームを閉じる
        public void MakeInjectionCallback(Form f, ListBox addressList, NumericUpDown injectionCallback,bool isGetAddr)
        {
            if (injectionCallback == null)
            {
                return;
            }

            Form srcForm = U.ControlToParentForm(injectionCallback);
            if (srcForm == null)
            {
                return;
            }
            InputFormRef.ShowUpperNotifyAnimation(f, addressList);

            addressList.MouseDoubleClick += (sender, e) =>
            {
                uint value;
                if (isGetAddr)
                {
                    value = InputFormRef.SelectToAddr(addressList);
                    value = U.toPointer(value);
                }
                else
                {
                    int correctValue = InputFormRef.GetCorrectionStartID(f);
                    value = (uint)(addressList.SelectedIndex + correctValue);
                }
                injectionCallback.Focus(); //フォーカスの是非を見ているので一度フォーカス一度が必要.
                U.ForceUpdate(injectionCallback, value);
                this.Popup.Hide();              //popアップがでることがあるので黙らせる.
                //HideFloatingControlpanel();   //ここはpopupだけを消したいのでダイアログを消してはいけない
                f.Close();
            };
        }
        //ボタンclickで、呼び出し元の injecttionCallbackへ値を代入. フォームを閉じる
        void MakeInjectionApplyButtonCallback(Form f, Button applyButton, NumericUpDown injectionCallback)
        {
            if (injectionCallback == null)
            {
                return;
            }

            Form srcForm = U.ControlToParentForm(injectionCallback);
            if (srcForm == null)
            {
                return;
            }

            applyButton.MouseClick += (sender, e) =>
            {
                uint value = (uint)applyButton.Tag;

                injectionCallback.Focus(); //フォーカスの是非を見ているので一度フォーカス一度が必要.
                U.ForceUpdate(injectionCallback, value);
                this.Popup.Hide();              //popアップがでることがあるので黙らせる.
                //HideFloatingControlpanel();   //ここはpopupだけを消したいのでダイアログを消してはいけない
                f.Close();
            };
        }


        void ParamSrc_Focused(object sender, EventArgs e)
        {
            EventScript.OneCode code;
            int selectID;
            int argindex;
            if (!Get_Select_ParamSrc_Object(sender, out code, out selectID, out argindex))
            {
                Popup.Visible = false;
                return;
            }


            EventScript.Arg arg = code.Script.Args[argindex];
            uint value = EventScriptForm.GetValueOneScriptEditSetTables(ScriptEditSetTables[selectID], arg);
            if (arg.Type == EventScript.ArgType.CG)
            {
                Bitmap image = ImageCGForm.DrawImageByID(value);
                PopupDialog_Image((NumericUpDown)sender, image);
            }
            else if (arg.Type == EventScript.ArgType.BG)
            {
                Bitmap image = ImageBGForm.DrawBG(value);
                PopupDialog_Image((NumericUpDown)sender, image);
            }
            else if (arg.Type == EventScript.ArgType.PORTRAIT || arg.Type == EventScript.ArgType.REVPORTRAIT)
            {
                Bitmap image = ImagePortraitForm.DrawPortraitAuto(value);
                if (arg.Type == EventScript.ArgType.REVPORTRAIT)
                {
                    image.RotateFlip(RotateFlipType.Rotate180FlipY);
                }
                PopupDialog_Image((NumericUpDown)sender, image);
            }
//            else if (arg.Type == EventScript.ArgType.POINTER_UNIT)
//            {
//                Bitmap image = EventUnitForm.DrawMapAndUnit(this.ScanMAPID(), U.toOffset(value));
//                PopupDialog_Image((NumericUpDown)sender, image);
//            }
            else if (arg.Type == EventScript.ArgType.MAPCHAPTER)
            {
                PopupDialog_MAP((NumericUpDown)sender, value);
            }
            else if (arg.Type == EventScript.ArgType.MAP_CHANGE)
            {
                uint mapid = ScanMAPID();
                PopupDialog_MAPAndChange((NumericUpDown)sender, mapid, value);
            }
            else if (arg.Type == EventScript.ArgType.MUSIC || arg.Type == EventScript.ArgType.SOUND)
            {
                PopupDialog_Music((NumericUpDown)sender, value);
            }
            else if (arg.Type == EventScript.ArgType.MAPX || arg.Type == EventScript.ArgType.MAPY)
            {
                uint mapid = ScanMAPID();
                NumericUpDown xObj;
                NumericUpDown yObj;
                if (arg.Type == EventScript.ArgType.MAPX)
                {
                    if (selectID + 1 >= ScriptEditSetTables.Length)
                    {
                        Popup.Visible = false;
                        return;
                    }
                    xObj = (NumericUpDown)sender;
                    yObj = ScriptEditSetTables[selectID + 1].ParamSrc;
                }
                else
                {
                    if (selectID <= 0 || selectID - 1 >= ScriptEditSetTables.Length)
                    {
                        Popup.Visible = false;
                        return;
                    }

                    xObj = ScriptEditSetTables[selectID - 1].ParamSrc;
                    yObj = (NumericUpDown)sender;
                }
                bool updateLock = false;

                Func<int, int, int> movecallback = (int x, int y) =>
                {
                    if (updateLock)
                    {
                        return 0;
                    }
                    updateLock = true;

                    U.ForceUpdate(xObj, x);
                    U.ForceUpdate(yObj, y);

                    updateLock = false;

                    //補正しないといけないっぽい?
                    PopupDialog_MAPUpdatePos(x, y);
                    return -1;
                };

                PopupDialog_MAP((NumericUpDown)sender, mapid, (int)xObj.Value, (int)yObj.Value, movecallback);
            }
            else if (arg.Type == EventScript.ArgType.WMAPX || arg.Type == EventScript.ArgType.WMAPY)
            {
                uint mapid = ScanMAPID();
                NumericUpDown xObj;
                NumericUpDown yObj;
                if (arg.Type == EventScript.ArgType.WMAPX)
                {
                    if (selectID + 1 >= ScriptEditSetTables.Length)
                    {
                        Popup.Visible = false;
                        return;
                    }
                    xObj = (NumericUpDown)sender;
                    yObj = ScriptEditSetTables[selectID + 1].ParamSrc;
                }
                else
                {
                    if (selectID <= 0 || selectID - 1 >= ScriptEditSetTables.Length)
                    {
                        Popup.Visible = false;
                        return;
                    }
                    xObj = ScriptEditSetTables[selectID - 1].ParamSrc;
                    yObj = (NumericUpDown)sender;
                }
                bool updateLock = false;

                Func<int, int, int> movecallback = (int x, int y) =>
                {
                    if (updateLock)
                    {
                        return 0;
                    }
                    updateLock = true;

                    U.ForceUpdate(xObj, x);
                    U.ForceUpdate(yObj, y);

                    updateLock = false;

                    //補正しないといけないっぽい
                    PopupDialog_MAPUpdatePos(x, y);
                    return -1;
                };

                PopupDialog_MAP((NumericUpDown)sender, mapid, (int)xObj.Value, (int)yObj.Value, movecallback);
            }
            else if (arg.Type == EventScript.ArgType.SCREENX || arg.Type == EventScript.ArgType.SCREENY)
            {
                NumericUpDown xObj;
                NumericUpDown yObj;
                if (arg.Type == EventScript.ArgType.SCREENX)
                {
                    if (selectID + 1 >= ScriptEditSetTables.Length)
                    {
                        Popup.Visible = false;
                        return;
                    }
                    xObj = (NumericUpDown)sender;
                    yObj = ScriptEditSetTables[selectID + 1].ParamSrc;
                }
                else
                {
                    if (selectID <= 0 || selectID - 1 >= ScriptEditSetTables.Length)
                    {
                        Popup.Visible = false;
                        return;
                    }
                    xObj = ScriptEditSetTables[selectID - 1].ParamSrc;
                    yObj = (NumericUpDown)sender;
                }
                bool updateLock = false;

                Func<int, int, int> movecallback = (int x, int y) =>
                {
                    if (updateLock)
                    {
                        return 0;
                    }
                    updateLock = true;

                    U.ForceUpdate(xObj, x);
                    U.ForceUpdate(yObj, y);

                    updateLock = false;

                    //補正しないといけないっぽい
                    PopupDialog_MAPUpdatePos(x, y);
                    return -1;
                };

                PopupDialog_Screen((NumericUpDown)sender, (int)xObj.Value, (int)yObj.Value, movecallback);
            }
            else if (arg.Type == EventScript.ArgType.EVENTUNITPOS)
            {
                uint mapid = ScanMAPID();
                NumericUpDown eventUnitPos = (NumericUpDown)sender;
                bool updateLock = false;

                Func<int, int, int> movecallback = (int x, int y) =>
                {
                    if (updateLock)
                    {
                        return 0;
                    }
                    updateLock = true;

                    uint v = (uint)eventUnitPos.Value;
                    v = (uint)((v & ~0xFFF) | ((uint)x & 0x3F) | (((uint)y & 0x3F) << 6));

                    U.ForceUpdate(eventUnitPos, v);

                    updateLock = false;

                    //補正しないといけないっぽい
                    PopupDialog_MAPUpdatePos(x, y);
                    return -1;
                };

                PopupDialog_MAP((NumericUpDown)sender, mapid
                    ,(int)U.ParseFE8UnitPosX((uint)eventUnitPos.Value)
                    ,(int)U.ParseFE8UnitPosY((uint)eventUnitPos.Value)
                    , movecallback);
            }
            else if (arg.Type == EventScript.ArgType.MAPXY)
            {
                uint mapid = ScanMAPID();
                NumericUpDown mapxy = (NumericUpDown)sender;
                bool updateLock = false;

                Func<int, int, int> movecallback = (int x, int y) =>
                {
                    if (updateLock)
                    {
                        return 0;
                    }
                    updateLock = true;

                    uint v = (uint)mapxy.Value;
                    v = (uint)( ((uint)x & 0xFFFF) | (((uint)y & 0xFFFF) << 16));

                    U.ForceUpdate(mapxy, v);

                    updateLock = false;

                    //補正しないといけないっぽい
                    PopupDialog_MAPUpdatePos(x, y);
                    return -1;
                };

                PopupDialog_MAP((NumericUpDown)sender, mapid
                    , (int)U.ParsePosX((uint)mapxy.Value)
                    , (int)U.ParsePosY((uint)mapxy.Value)
                    , movecallback);
            }
            else if (arg.Type == EventScript.ArgType.SCREENXY)
            {
                NumericUpDown screenxy = (NumericUpDown)sender;
                bool updateLock = false;

                Func<int, int, int> movecallback = (int x, int y) =>
                {
                    if (updateLock)
                    {
                        return 0;
                    }
                    updateLock = true;

                    uint v = (uint)screenxy.Value;
                    v = (uint)(((uint)x & 0xFFFF) | (((uint)y & 0xFFFF) << 16));

                    U.ForceUpdate(screenxy, v);

                    updateLock = false;

                    //補正しないといけないっぽい
                    PopupDialog_MAPUpdatePos(x, y);
                    return -1;
                };

                PopupDialog_Screen((NumericUpDown)sender
                    , (int)U.ParsePosX((uint)screenxy.Value)
                    , (int)U.ParsePosY((uint)screenxy.Value)
                    , movecallback);
            }
            else
            {
                Popup.Visible = false;
            }
        }

        void ParamSrc_UnFocused(object sender, EventArgs e)
        {
            if (Popup.IsFocusedControl())
            {
                return;
            }
            Popup.Visible = false;
        }

        //現在編集しているイベントのアドレスを求める.
        uint NowSelectedEventScriptAddr()
        {
            if (this.AddressList.SelectedIndex < 0 || this.AddressList.SelectedIndex >= this.EventAsm.Count)
            {
                return U.NOT_FOUND;
            }
            uint addr = (uint)this.ReadStartAddress.Value;
            for (int i = 0; i < this.AddressList.SelectedIndex; i++)
            {
                addr += (uint)this.EventAsm[i].ByteData.Length;
            }
            return addr;
        }

        //途中でマップ変更がされているかもしれないので、現在のマップIDを検出する.
        uint ScanMAPID()
        {
            //無限再帰を避けるために、読んだ所をリストアップしていく.
            List<uint> tracelist = new List<uint>();

            //マップ探索開始
            return ScanMAPIDLow((uint)this.ReadStartAddress.Value
                , NowSelectedEventScriptAddr()
                , this.MapID
                , tracelist);
        }
        static uint ScanMAPIDLow(uint event_addr, uint stop_addr, uint start_mapid, List<uint> tracelist)
        {
            uint mapid = start_mapid;
            uint lastBranchAddr = 0;
            int unknown_count = 0;
            uint addr = event_addr;
            while (true)
            {
                //バイト列をイベント命令としてDisassembler.
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                if (EventScript.IsExitCode(code, addr, lastBranchAddr))
                {//終端命令
                    break;
                }
                else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                {
                    unknown_count++;
                    if (unknown_count > 10)
                    {//不明命令が10個連続して続いたら打ち切る
                        break;
                    }
                }
                else
                {
                    //少なくとも不明ではない.
                    unknown_count = 0;

                    if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                    {//IF文で分岐の前に、現在のマップを記録します.
                        //終端のラベルで書き戻します.
                        start_mapid = mapid;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                    {//ラベル条件で戻ってきた時、MAPIDを復元します.
                        //完ぺきではないが、そこそこ正しいを目指す.
                        mapid = start_mapid;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.POINTER_UNIT_OR_EVENT)
                    {//イベント命令へジャンプするものをもっているらしい.
                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                            {
                                uint v = EventScript.GetArgValue(code, arg);

                                v = U.toOffset(v);
                                if (U.isSafetyOffset(v)             //安全であり
                                    && tracelist.IndexOf(v) < 0     //まだ読んだことがなければ
                                    )
                                {
                                    tracelist.Add(v);
                                    mapid = ScanMAPIDLow(v, stop_addr, mapid, tracelist);
                                }
                            }
                        }
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.MAP)
                    {//マップ切り替え命令をもっているらしい.
                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.MAPCHAPTER)
                            {
                                mapid = EventScript.GetArgValue(code, arg);
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
                if (addr == stop_addr)
                {//停止位置
                    break;
                }
                addr += (uint)code.Script.Size;
            }
            return mapid;
        }


        //ポップアップの位置調整
        void AjustPopup(NumericUpDown sender)
        {
            //位置調整
            Point numObjectPos = sender.Location;
            numObjectPos.X += ControlPanel.Location.X;
            numObjectPos.Y += ControlPanel.Location.Y;

            int x = numObjectPos.X + sender.Width;
            int y = numObjectPos.Y - Popup.Height + sender.Height * 2;
            if (x + Popup.Width > this.Width)
            {
                x = numObjectPos.X - Popup.Width;
                if (x < 0)
                {
                    x = 0;
                }
            }
            if (y < 0)
            {
                y = 0;
            }

            Popup.Location = new Point(x, y);
            Popup.BringToFront();
            Popup.Visible = true;
        }

        void PopupDialog_Image(NumericUpDown sender, Bitmap image)
        {
            Popup.LoadImage(image);
            AjustPopup(sender);
        }
        void PopupDialog_Music(NumericUpDown sender, uint value)
        {
            Popup.LoadMusic(value);
            AjustPopup(sender);
        }

        void PopupDialog_MAPAndChange(NumericUpDown sender, uint value, uint changeid)
        {
            Popup.Visible = true; //デザイナーのバグがあり、コンストラクタで初期化できないので、 Load イベントで初期化するため、ここで表示する.
            Popup.LoadMap(value);
            Popup.MAP.SetMapChange(changeid);
            AjustPopup(sender);
        }
        void PopupDialog_MAP(NumericUpDown sender, uint value)
        {
            Popup.Visible = true; //デザイナーのバグがあり、コンストラクタで初期化できないので、 Load イベントで初期化するため、ここで表示する.
            Popup.LoadMap(value);
            AjustPopup(sender);
        }
        void PopupDialog_MAP(NumericUpDown sender, uint value, int x, int y)
        {
            Popup.Visible = true; //デザイナーのバグがあり、コンストラクタで初期化できないので、 Load イベントで初期化するため、ここで表示する.
            Popup.LoadMap(value);
            Popup.MAP.SetPoint(this.AddressList.SelectedIndex.ToString(), x, y);
            AjustPopup(sender);
        }
        void PopupDialog_MAP(NumericUpDown sender, uint value, int x, int y, Func<int, int, int> movecallback)
        {
            Popup.Visible = true; //デザイナーのバグがあり、コンストラクタで初期化できないので、 Load イベントで初期化するため、ここで表示する.
            Popup.LoadMap(value);
            Popup.MAP.SetPoint(this.AddressList.SelectedIndex.ToString(), x, y);
            Popup.MAP.setNotifyMode(this.AddressList.SelectedIndex.ToString(), movecallback);

            AjustPopup(sender);
        }
        void PopupDialog_Screen(NumericUpDown sender, int x, int y, Func<int, int, int> movecallback)
        {
            Popup.Visible = true; //デザイナーのバグがあり、コンストラクタで初期化できないので、 Load イベントで初期化するため、ここで表示する.
            Popup.LoadScreen();
            Popup.MAP.SetPoint(this.AddressList.SelectedIndex.ToString(), x, y);
            Popup.MAP.setNotifyMode(this.AddressList.SelectedIndex.ToString(), movecallback);

            AjustPopup(sender);
        }
        void PopupDialog_MAPUpdatePos(int x, int y)
        {
            this.Popup.MAP.SetPoint(this.AddressList.SelectedIndex.ToString(), x, y);
            //this.Popup.Hide();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            int selected = this.AddressList.SelectedIndex;
            if (selected < 0 || selected >= this.EventAsm.Count)
            {//追加で処理する.
                NewButton.PerformClick();
                return;
            }
            CheckPatches();

            OneLineDisassembler();

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.EventScript.DisAseemble(selectedByteData, 0);
            code.Comment = this.CommentTextBox.Text;

            //選択されているコードを入れ替える.
            this.EventAsm[this.AddressList.SelectedIndex] = code;

            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, selected);

            HideFloatingControlpanel();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (this.EventAsm == null)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            CheckPatches();

            OneLineDisassembler();

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.EventScript.DisAseemble(selectedByteData, 0);
            code.Comment = this.CommentTextBox.Text;

            int selected;
            //選択されている部分に追加
            if (this.AddressList.SelectedIndex < 0)
            {
                this.EventAsm.Add(code);
                selected = this.EventAsm.Count - 1;
            }
            else
            {
                if (IsSelectedTermCode())
                {//終端コードの後ろには追加させない
                    this.EventAsm.Insert(this.AddressList.SelectedIndex, code);
                    selected = this.AddressList.SelectedIndex;
                }
                else
                {
                    this.EventAsm.Insert(this.AddressList.SelectedIndex + 1, code);
                    selected = this.AddressList.SelectedIndex + 1;
                }
            }
            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, selected);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();
        }

        //各種パッチ探索
        void CheckPatches()
        {
            CheckSkipWorldmapPatch();
            CheckCAMERA_Event_OutOfBand_Fix();
            CheckCAMERA_Event_NotExistsUnit_Fix();
            CheckGetUnitStateEvent_0x33_Fix();
            CheckUpdateUnitStateEvent_0x34_Fix();
            CheckWakuEvent_0x3B_Fix();
        }

        void CheckSkipWorldmapPatch()
        {
            string command = ScriptCodeName.Text;
            if (command.IndexOf("(MNC2)") >= 0 || command.IndexOf("(MNC3)") >= 0)
            {
                HowDoYouLikePatch2Form.CheckAndShowPopupDialog(HowDoYouLikePatch2Form.TYPE.SkipWorldmapFix);
            }
        }

        void CheckCAMERA_Event_OutOfBand_Fix()
        {
            string command = ScriptCodeName.Text;
            if (command.IndexOf("(CAM1)") >= 0)
            {
                HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.CAMERA_Event_OutOfBand_Fix);
            }
        }

        void CheckCAMERA_Event_NotExistsUnit_Fix()
        {
            string command = ScriptCodeName.Text;
            if (command.IndexOf("(CAM1") >= 0 || command.IndexOf("(CAM2") >= 0)
            {
                HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.CAMERA_Event_NotExistsUnit_Fix);
            }
        }

        void CheckGetUnitStateEvent_0x33_Fix()
        {
            string command = ScriptCodeName.Text;
            if (command.IndexOf("(CHECK_STATUS)") >= 0
                || command.IndexOf("(CHECK_COORDS)") >= 0
                || command.IndexOf("(CHECK_CLASS)") >= 0
                || command.IndexOf("(CHECK_LUCK)") >= 0
                || command.IndexOf("(CHECK_COORDS)") >= 0
                || command.IndexOf("(CHECK_CLASS)") >= 0
                || command.IndexOf("(CHECK_DEPLOYED)") >= 0
                || command.IndexOf("(CHECK_ACTIVEID)") >= 0
                || command.IndexOf("(CHECK_ALLEGIANCE)") >= 0
                || command.IndexOf("(CHECK_EXISTS)") >= 0
                )
            {
                HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.UnitGetStateEvent_0x33_Fix);
            }
        }

        void CheckUpdateUnitStateEvent_0x34_Fix()
        {
            string command = ScriptCodeName.Text;
            if (command.IndexOf("(REMU)") >= 0
                || command.IndexOf("(REVEAL)") >= 0
                || command.IndexOf("(CUSA)") >= 0
                || command.IndexOf("(CUSN)") >= 0
                || command.IndexOf("(CUSE)") >= 0
                || command.IndexOf("(SET_HP)") >= 0
                || command.IndexOf("(SET_ENDTURN)") >= 0
                || command.IndexOf("(SET_STATE)") >= 0
                || command.IndexOf("(SET_SOMETHING)") >= 0
                || command.IndexOf("(DISA_IF)") >= 0
                || command.IndexOf("(REMU)") >= 0
                || command.IndexOf("(DISA)") >= 0
                )
            {
                HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.UnitUpdateStateEvent_0x34_Fix);
            }
        }

        void CheckWakuEvent_0x3B_Fix()
        {
            string command = ScriptCodeName.Text;
            if (command.IndexOf("CUMO)") >= 0)
            {
                HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.WakuEvent_0x3B_Fix);
            }
        }


        //選択しているのは、終端コードかどうか？
        bool IsSelectedTermCode()
        {
            int selected = this.AddressList.SelectedIndex;
            if (selected < 0)
            {
                return false;
            }
            if (selected >= this.EventAsm.Count)
            {
                return false;
            }
            if (this.EventAsm[selected].Script.Has == EventScript.ScriptHas.TERM)
            {//選択しているのは終端コード
                return true;
            }
            return false;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int selected = this.AddressList.SelectedIndex;
            if (selected < 0 || selected >= this.EventAsm.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            this.EventAsm.RemoveAt(this.AddressList.SelectedIndex);
            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, this.AddressList.SelectedIndex - 1);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();        
        }

        private void ReadStartAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReloadListButton_Click(null, null);
            }
        }

        private void ReadCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReloadListButton_Click(null, null);
            }
            else
            {
                //何かやっているので手動で値を求めたとする.
                this.IsLengthAutoChecked = false;
            }
        }
        private void ReadCount_MouseDown(object sender, MouseEventArgs e)
        {
            //何かやっているので手動で値を求めたとする.
            this.IsLengthAutoChecked = false;
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex < 1 || this.AddressList.SelectedIndex >= this.EventAsm.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            U.SwapUp(this.EventAsm, this.AddressList,this.AddressList.SelectedIndex);

            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, this.AddressList.SelectedIndex - 1);

            //コントロールパネルを閉じたくない.
            ControlPanel.Show();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex < 0 || this.AddressList.SelectedIndex+1 >= this.EventAsm.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            U.SwapDown(this.EventAsm,this.AddressList, this.AddressList.SelectedIndex);

            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, this.AddressList.SelectedIndex + 1);

            //コントロールパネルを閉じたくない.
            ControlPanel.Show();
        }

        void JumpTo(uint addr)
        {
            addr = U.toOffset(addr);
            U.ForceUpdate(ReadStartAddress, addr);

            //自動で長さを求めましたということにして再取得すれば自動的に長さを計算する
            this.IsLengthAutoChecked = true;
            ReloadListButton_Click(null, null);
        }
        public void JumpTo(uint addr, uint currnt_event)
        {
            JumpTo(addr);

            if (currnt_event != U.NOT_FOUND)
            {
                int selected = EventScript.ConvertAddrToSelected(this.EventAsm, addr, currnt_event);
                if (selected >= 0)
                {
                    this.AddressList.SelectedIndex = selected;
                }
            }
        }


        //現在イベントのマップID
        uint MapID = 0;
        //イベントIDからマップアドレスの逆変換
        public static uint FindMapID(uint addr)
        {
            uint maxcount = MapSettingForm.GetDataCount();
            for(uint mapid = 0 ; mapid < maxcount; mapid++)
            {
                //イベント命令　一覧の取得
                List<U.AddrResult> list = EventCondForm.MakeEventScriptPointer(mapid);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].addr == addr)
                    {
                        return mapid;
                    }
                }
            }

            //ワールドマップイベントの可能性
            {
                List<U.AddrResult> list = WorldMapEventPointerForm.MakeEventScriptPointer();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].addr == addr)
                    {
                        return maxcount; //MapPicture.cs内でのWorldMapは MAX
                    }
                }
            }
            return 0;
        }

        private void ScriptChangeButton_Click(object sender, EventArgs e)
        {
            FireChanegCommandByDirectMode(isDirectMode: false);
        }

        void InsertEventScript(EventScript.Script script)
        {
            //選択した命令を代入
            byte[] selectedByteData = script.Data;
            this.ASMTextBox.Text = U.convertByteToStringDump(selectedByteData);
            this.ScriptCodeName.Text = EventScript.makeCommandComboText(script, false);

            //イベントを逆アセンブルして確定する.
            OneLineDisassembler();

            //値1を自動選択
            if (ParamSrc1.Visible)
            {
                ParamSrc1.Focus();

                //もし、自動でポップアップメニューが表示される場合OFFにする.
                this.Popup.Hide();
            }
        }
        void InsertEventTemplate(List<EventScript.OneCode> codes)
        {
            HideFloatingControlpanel();

            int insertedPoint = this.AddressList.SelectedIndex;
            if (insertedPoint < 0)
            {
                insertedPoint = 0;
            }

            //追加するのでUndoポイントを作成する
            PushUndo();

            //テンプレートからデータを追加.
            this.EventAsm.InsertRange(insertedPoint, codes);
            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc(this.EventAsm.Count, insertedPoint + 1);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
        }

        void FireChanegCommandByDirectMode(bool isDirectMode)
        {
            EventScriptFormCategorySelectForm form = (EventScriptFormCategorySelectForm)InputFormRef.JumpFormLow<EventScriptFormCategorySelectForm>();
            form.Init(this.MapID, this);
            DialogResult dr = form.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {//イベント命令の選択
                InsertEventScript(form.Script);
                return;
            }
            if (dr == System.Windows.Forms.DialogResult.Retry)
            {//イベントテンプレートの選択
                InsertEventTemplate(form.EventTemplateCode);
                return;
            }

            if (isDirectMode)
            {//直接編集モードだったら、キャンセルされた場合は、コントロールパネルも消す.
                HideFloatingControlpanel();
            }
            return;

        }

        private static byte[] LineToEventByte(string line)
        {
            List<byte> ret = new List<byte>();

            line = line.Trim();
            int length = line.Length;
            for (int i = 0; i < length; i += 2)
            {
                if (!U.ishex(line[i]))
                {
                    break;
                }
                if (i + 1 >= length)
                {
                    break;
                }
                if (!U.ishex(line[i+1]))
                {
                    break;
                }

                byte b = (byte)U.atoh(line.Substring(i, 2));
                ret.Add(b);
            }
            return ret.ToArray();
        }

        private void TextToEvent(string str,int insertPoint = -1,bool isClear = false)
        {
            this.PushUndo();

            if (isClear)
            {
                this.EventAsm.Clear();
            }


            //追加した場所.
            int insertedPoint;
            if (insertPoint <= -1)
            {
                insertedPoint = this.EventAsm.Count;
            }
            else
            {
                insertedPoint = insertPoint + 1;
            }

            string[] lines = str.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                byte[] bin = LineToEventByte(lines[i]);
                if (bin.Length < 4)
                {//壊れているか違うコード
                    continue;
                }

                EventScript.OneCode code = Program.EventScript.DisAseemble(bin,0);
                if (insertPoint <= -1)
                {//末尾に追加.
                    this.EventAsm.Add(code);
                }
                else
                {//特定場所に追加.
                    this.EventAsm.Insert(insertPoint ,code);
                }
            }

            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, insertedPoint + 1);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
        }

        private string EventToTextAll()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.EventAsm.Count; i++)
            {
                sb.Append(EventToTextOne(i));
            }
            return sb.ToString();
        }
        private string EventToTextOne(int number)
        {
            StringBuilder sb = new StringBuilder();
            EventScript.OneCode code = this.EventAsm[number];
            return EventScriptInnerControl.EventToTextOne(code);
        }

        static public string EventToTextOne(EventScript.OneCode code)
        {
            StringBuilder sb = new StringBuilder();
            for (int n = 0; n < code.ByteData.Length; n++)
            {
                sb.Append(U.ToHexString(code.ByteData[n]));
            }
            sb.Append("\t//");

            //スクリプト名.
            sb.Append(code.Script.Info[0]);

            for (int i = 1; i < code.Script.Info.Length; i += 2)
            {
                char symbol = ' ';
                if (code.Script.Info[i].Length > 2)
                {// [X みたいな文字列が入る. 2文字目のXが シンボル名.
                    symbol = code.Script.Info[i][1];
                }

                for (int n = 0; n < code.Script.Args.Length; n++)
                {
                    EventScript.Arg arg = code.Script.Args[n];
                    if (EventScript.IsFixedArg(arg))
                    {//固定長になっているところは入力できないようにする.
                        continue;
                    }
                    if (symbol != arg.Symbol)
                    {
                        continue;
                    }

                    sb.Append("[");

                    uint v;
                    string hexstring = EventScript.GetArg(code, n, out v);
                    sb.Append(arg.Name);
                    sb.Append(":");

                    sb.Append(hexstring);

                    if (arg.Type == EventScript.ArgType.TEXT
                        || arg.Type == EventScript.ArgType.CONVERSATION_TEXT
                        || arg.Type == EventScript.ArgType.SYSTEM_TEXT
                        || arg.Type == EventScript.ArgType.ONELINE_TEXT
                        || arg.Type == EventScript.ArgType.POINTER_TEXT
                        )
                    {
                        sb.Append(" ");
                        string text = TextForm.DirectAndStripAllCode((v));
                        if (text.Length > 30)
                        {//長いテキストは省略
                            sb.Append(U.escape_return(U.strimwidth(text, 0, 20)));
                        }
                        else
                        {//長くないテキストはすぐ横に表示
                            sb.Append(U.escape_return(text));
                        }
                    }
                    else if (arg.Type == EventScript.ArgType.UNIT)
                    {
                        sb.Append(" ");
                        sb.Append(UnitForm.GetUnitName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.CLASS)
                    {
                        sb.Append(" ");
                        sb.Append(ClassForm.GetClassName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.PORTRAIT)
                    {
                        sb.Append(" ");
                        sb.Append(ImagePortraitForm.GetPortraitName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.ITEM)
                    {
                        sb.Append(" ");
                        sb.Append(ItemForm.GetItemName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.MAPCHAPTER)
                    {
                        sb.Append(" ");
                        sb.Append(MapSettingForm.GetMapName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.MUSIC || arg.Type == EventScript.ArgType.SOUND)
                    {
                        sb.Append(" ");
                        sb.Append(SongTableForm.GetSongName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WEATHER)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWEATHER(v));
                    }
                    else if (arg.Type == EventScript.ArgType.FOG)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetFOG(v));
                    }
                    else if (arg.Type == EventScript.ArgType.UNIT_COLOR)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetUNIT_COLOR(v));
                    }
                    else if (arg.Type == EventScript.ArgType.EARTHQUAKE)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetEARTHQUAKE(v));
                    }
                    else if (arg.Type == EventScript.ArgType.ATTACK_TYPE)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetATTACK_TYPE(v));
                    }
                    else if (arg.Type == EventScript.ArgType.MENUCOMMAND)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetMENUCOMMAND(v));
                    }
                    else if (arg.Type == EventScript.ArgType.AI1)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetAI1(v));
                    }
                    else if (arg.Type == EventScript.ArgType.AI2)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetAI2(v));
                    }
                    else if (arg.Type == EventScript.ArgType.CG)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetCGComment(v));
                    }
                    else if (arg.Type == EventScript.ArgType.BG)
                    {
                        sb.Append(" ");
                        sb.Append(ImageBGForm.GetComment(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMSTYLE1)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWMSTYLE1(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMSTYLE2)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWMSTYLE2(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMSTYLE3)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWMSTYLE3(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMREGION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWMREGION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMENREGION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWMENREGION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.AFFILIATION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetAFFILIATION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMAPAFFILIATION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWMAPAFFILIATION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMAP2AFFILIATION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetWMAP2AFFILIATION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.EVENTUNITPOS)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetEVENTUNITPOS(v));
                    }
                    else if (arg.Type == EventScript.ArgType.DIRECTION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetDIRECTION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.PORTRAIT_DIRECTION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetPORTRAIT_DIRECTION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMLOCATION)
                    {//ワールドマップの名前
                        sb.Append(" ");
                        sb.Append(WorldMapPointForm.GetWorldMapPointName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMPATH)
                    {//ワールドマップの道
                        sb.Append(" ");
                        sb.Append(WorldMapPathForm.GetPathName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.FLAG)
                    {//フラグ
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetFlagName(v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.MAGVELY)
                    {//FE8の世界地図の移動 -8 ～ 52の範囲
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetMagvelYName((short)v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_PROCS)
                    {//PROC
                        sb.Append(" ");
                        string dummy;
                        sb.Append(Program.AsmMapFileAsmCache.GetASMName(v, true, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_ASM)
                    {//ASM
                        sb.Append(" ");
                        string dummy;
                        sb.Append(Program.AsmMapFileAsmCache.GetASMName(v, false, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                    {//EVENT
                        sb.Append(" ");
                        string dummy;
                        sb.Append(Program.AsmMapFileAsmCache.GetEventName(v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.MEMORYSLOT)
                    {//MemorySlot
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetMEMORYSLOT(v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.COUNTER)
                    {//Counter
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetCOUNTER(v, out dummy));
                    }
                    if (arg.Type == EventScript.ArgType.TILE)
                    {//タイル名
                        sb.Append(" ");
                        sb.Append(MapTerrainNameForm.GetName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.PACKED_MEMORYSLOT)
                    {//MemorySlotPacked
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetPACKED_MEMORYSLOT(v, code.Script.Info[0], out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.RAM_UNIT_STATE)
                    {//RAM_UNIT_STATE
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetRAM_UNIT_STATE(v));
                    }
                    else if (arg.Type == EventScript.ArgType.MAPEMOTION)
                    {//MAPEMOTION
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetMAPEMOTION(v));
                    }
                    else if (arg.Type == EventScript.ArgType.DISABLEOPTIONS)
                    {//DISABLEOPTIONS
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetDISABLEOPTIONS(v));
                    }
                    else if (arg.Type == EventScript.ArgType.DISABLEWEAPONS)
                    {//DISABLEWEAPONS
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetDISABLEWEAPONS(v));
                    }
                    else if (arg.Type == EventScript.ArgType.IGNORE_KEYS)
                    {//IGNORE_KEYS
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetIGNORE_KEYS(v));
                    }
                    else if (arg.Type == EventScript.ArgType.KEYS)
                    {//KEYS
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetPressKEYS(v));
                    }
                    else if (arg.Type == EventScript.ArgType.FSEC)
                    {//FSEC
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetFSEC(v));
                    }
                    else if (arg.Type == EventScript.ArgType.MAPXY)
                    {//MAPXY
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetMAPXY(v));
                    }
                    else if (arg.Type == EventScript.ArgType.RAM_UNIT_PARAM)
                    {//RAM_UNIT_PARAM
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetRAM_UNIT_PARAM(v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.RAM_UNIT_VALUE)
                    {//RAM_UNIT_PARAM
                        uint prevIndex = EventScript.GetRAMUnitParamIndex(code);
                        uint prevValue = 0;
                        EventScript.GetArg(code, (int)prevIndex, out prevValue);

                        string dummy;
                        sb.Append(InputFormRef.GetRAM_UNIT_VALUE(prevValue, v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.BOOL)
                    {//BOOL
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetBOOL(v));
                    }
                    else if (arg.Type == EventScript.ArgType.TRAP)
                    {//TRAP
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetTRAP(v));
                    }
                    else if (arg.Type == EventScript.ArgType.WMAP_SPRITE_ID)
                    {//WMAP_SPRITE_ID
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetWMAP_SPRITE_ID(v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.EVBIT)
                    {//EVBIT
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetEVBIT(v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.EVBIT_MODIFY)
                    {//EVBIT_MODIFY
                        sb.Append(" ");
                        string dummy;
                        sb.Append(InputFormRef.GetEVBIT_MODIFY(v, out dummy));
                    }
                    else if (arg.Type == EventScript.ArgType.BADSTATUS)
                    {//BADSTATUS
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetBADSTATUS(v));
                    }
                    else if (arg.Type == EventScript.ArgType.SKILL)
                    {//SKILL
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetSkillName(v));
                    }
                    else if (arg.Type == EventScript.ArgType.EDITION)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetEditon(v));
                    }
                    else if (arg.Type == EventScript.ArgType.DIFFICULTY)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetDifficulty(v));
                    }
                    else if (arg.Type == EventScript.ArgType.SUPPORT_LEVEL)
                    {
                        sb.Append(" ");
                        sb.Append(InputFormRef.GetSuportLevel(v));
                    }
                    else if (arg.Type == EventScript.ArgType.GAMEOPTION)
                    {
                        sb.Append(" ");
                        sb.Append(StatusOptionForm.GetNameIndex(v));
                    }
                    else if (arg.Type == EventScript.ArgType.GAMEOPTION_VALUE)
                    {
                        uint prevIndex = EventScript.GetGameOptionParamIndex(code);
                        uint prevValue = 0;
                        EventScript.GetArg(code, (int)prevIndex, out prevValue);

                        sb.Append(" ");
                        sb.Append(StatusOptionForm.GetValueNameIndex(prevValue, v));
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_AICOORDINATE)
                    {
                        sb.Append(" ");
                        sb.Append(AIASMCoordinateForm.GetCoordPreview(v));
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT4)
                    {
                        sb.Append(" ");
                        sb.Append(AIASMUnit4Form.GetUnit4Preview(v));
                    }
                    else if (arg.Type == EventScript.ArgType.POINTER_AICALLTALK)
                    {
                        sb.Append(" ");
                        sb.Append(AIASMCALLTALKForm.GetUnit2Preview(v));
                    }

                    sb.Append("]");
                    break;
                }

                if (i + 1 < code.Script.Info.Length)
                {
                    sb.Append(code.Script.Info[i + 1]);
                }
            }
            sb.AppendLine("");

            return sb.ToString();
        }


        private void EventToFileButton_Click(object sender, EventArgs ee)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|EA|*.event|All files|*");

            uint addr = U.toOffset((uint)this.ReadStartAddress.Value);

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, U.To0xHexString(addr) );

            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileName;
            Program.LastSelectedFilename.Save(this, "", save);

            string ext = U.GetFilenameExt(filename);
            if (ext == ".EVENT")
            {
                string errorOutput;
                bool r;
                try
                {
                    r = MainFormUtil.DisasembleEventAssembler(addr, filename, "ToEnd", "none",false, out errorOutput);
                }
                catch (Win32Exception e)
                {
                    r = false;
                    errorOutput = R._("プロセスを実行できません。\r\nfilename:{0}\r\n{1}", filename, e.ToString());
                }
                if (!r)
                {
                    R.ShowStopError("EAでエクスポートできませんでした。\r\n{0}",errorOutput);
                    return;
                }
            }
            else
            {
                U.WriteAllText(filename, EventToTextAll());
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void FileToEventButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("TEXT,EA|*.txt;*.event|TEXT|*.txt|EA|*.event|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }
            string filename = open.FileName;
            Program.LastSelectedFilename.Save(this, "", open);

            string ext = U.GetFilenameExt(filename);
            if (ext == ".EVENT")
            {
                ImportEA(filename);
            }
            else
            {
                string text = File.ReadAllText(filename);

                DialogResult dr2 = R.ShowQ("スクリプトを追記してもよろしいですか？\r\n\r\n「はい」ならば、既存のスクリプトの下に、ファイルから読みこんだスクリプトを追記します。\r\n「いいえ」ならば、既存のスクリプトを消去して、ファイルから読みこんだスクリプトで書き換えます。\r\n");
                if (dr2 == System.Windows.Forms.DialogResult.No)
                {
                    TextToEvent(text, -1, true);
                }
                else if (dr2 == System.Windows.Forms.DialogResult.Yes)
                {
                    TextToEvent(text);
                }
            }
        }
        public enum TermCode
        {
            NoTerm,
            DefaultTermCode,
            SimpleTermCode,
        }
        public static byte[] ConverteventTextToBin(string filename
            , TermCode addTerm = TermCode.DefaultTermCode
            ,string XXXXXXXX = null,string YYYYYYYY = null)
        {
            List<byte> binarray = new List<byte>();

            string text = File.ReadAllText(filename);
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.OtherLangLine(line))
                {
                    continue;
                }
                if (XXXXXXXX != null)
                {
                    line = line.Replace("XXXXXXXX", XXXXXXXX);
                    line = line.Replace("XXXX", XXXXXXXX);
                }
                if (YYYYYYYY != null)
                {
                    line = line.Replace("YYYYYYYY", YYYYYYYY);
                    line = line.Replace("YYYY", YYYYYYYY);
                }
                byte[] bin = LineToEventByte(line);
                if (bin.Length < 4)
                {//壊れているか違うコード
                    continue;
                }
                binarray.AddRange(bin);
            }

            if (addTerm == TermCode.DefaultTermCode)
            {//終端の追加.
                binarray.AddRange(Program.ROM.RomInfo.defualt_event_script_term_code());
            }
            else if (addTerm == TermCode.SimpleTermCode)
            {//終端の追加.
                binarray.AddRange(Program.ROM.RomInfo.defualt_event_script_term_code());
            }
            return binarray.ToArray();
        }

        void ImportEA(string filename)
        {
            //EventAssemblerで対象物をコンパイル
            string errorOutput;
            string symbolOutput;
            bool r;
            try
            {
                r = MainFormUtil.CompilerEventAssembler(filename, 0, U.NOT_FOUND, out errorOutput, out symbolOutput);
            }
            catch (Win32Exception e)
            {
                r = false;
                errorOutput = R._("プロセスを実行できません。\r\nfilename:{0}\r\n{1}", filename, e.ToString());
            }
            if (!r)
            {
                R.ShowStopError("EAでエクスポートできませんでした。\r\n{0}",errorOutput);
                return;
            }
            byte[] ea_romdata = File.ReadAllBytes(errorOutput);
            File.Delete(errorOutput); //temp ROMの削除

            //EAで変更された temp romからイベントを回収します.
            //イベント以外が変更されていたら無視します.
            uint change_addr = EventAssemblerForm.FindORG(filename);

            uint addr = (uint)ReadStartAddress.Value;
            if (U.isPointer(addr))
            {
                addr = U.toOffset(addr);
            }

            uint dataCount = (uint)ReadCount.Value;
            if (change_addr < addr || change_addr >= addr + dataCount)
            {
                DialogResult dr = R.ShowYesNo("EAで変更されたアドレスが現在開いているスクリプトの範囲外です。\r\n変更された部分をスクリプトとして取り込んでもよろしいですか？\r\n変更されたアドレス:{0}\r\n", U.To0xHexString(change_addr));
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                //変更された部分を取りに行きます.
                addr = change_addr;
            }

            //Undoに積む
            this.PushUndo();

            bool isWorldMapEvent = WorldMapEventPointerForm.isWorldMapEvent(addr);
            uint bytecount = EventScript.SearchEveneLength(ea_romdata, addr, isWorldMapEvent);

            this.EventAsm = new List<EventScript.OneCode>();

            uint limit = Math.Min(addr + bytecount, (uint)ea_romdata.Length);
            while (addr < limit)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(ea_romdata, addr);
                this.EventAsm.Add(code);

                addr += (uint)code.Script.Size;
            }
            //最後に自下げ処理実行.
            JisageReorder(this.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, 0);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
        }

        private void ClipBoardToEventButton_Click(object sender, EventArgs e)
        {
            string text = Clipboard.GetText();
            if (text == "" || U.atoi(text) <= 0)
            {
                return;
            }

            int i = this.AddressList.SelectedIndex;
            if (i < 0)
            {
                TextToEvent(text);
                return;
            }
            TextToEvent(text,i);
        }
        
        
        private void EventToClipBoardButton_Click(object sender, EventArgs e)
        {
            int i = this.AddressList.SelectedIndex;
            if (i < 0)
            {
                return;
            }

            U.SetClipboardText(EventToTextOne(i));
        }

        private void AddressList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                RunUndo();
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                RunRedo();
                return;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ShowFloatingControlpanel();
                return;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.T)
            {
                UseTemplate();
            }

            int i = this.AddressList.SelectedIndex;
            if (i < 0 || i >= this.AddressList.Items.Count)
            {
                return;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                U.SetClipboardText(EventToTextOne(i));
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                TextToEvent(Clipboard.GetText(), i);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteButton_Click(null, null);
            }
            else if (e.Control && e.KeyCode == Keys.J)
            {
                ShowFloatingControlpanel();
                EventScriptForm.DirectJump(0, this.ScriptEditSetTables, ParamLabel_Clicked);
                return;
            }
            else if (e.Alt && e.KeyCode == Keys.J)
            {
                ShowFloatingControlpanel();
                EventScriptForm.DirectJump(1, this.ScriptEditSetTables, ParamLabel_Clicked);
                return;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
                return;
            }
        }

        public void HideFloatingControlpanel()
        {
            this.ControlPanel.Hide();
            this.Popup.Hide();

            //ツールチップを出しているなら消す.
            if (this.ToolTip != null)
            {
                this.ToolTip.HideEvent();
            }
            AddressList.Focus();
        }
        public void ChangeDirect()
        {
            HideFloatingControlpanel();
            int i = this.AddressList.SelectedIndex;
            if (i < 0 || i >= this.AddressList.Items.Count)
            {
                return;
            }
            ShowFloatingControlpanel();
            ScriptChangeButton.Focus();
            FireChanegCommandByDirectMode(isDirectMode: true);
        }
        public void ChangeComment()
        {
            int i = this.AddressList.SelectedIndex;
            if (i < 0 || i >= this.AddressList.Items.Count)
            {
                return;
            }
            ShowFloatingControlpanel();
            CommentTextBox.Focus();
        }

        private void AddressList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowFloatingControlpanel();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
        }


        class UndoData
        {
            //UNDO サイズも小さいから、差分よりすべて記録する. 
            public List<EventScript.OneCode> EventAsm;
        };

        List<UndoData> UndoBuffer;
        int UndoPosstion;
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
            p.EventAsm = EventScript.CloneEventList(this.EventAsm);
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
        }
        void RunUndoRollback(UndoData u)
        {
            this.EventAsm = EventScript.CloneEventList(u.EventAsm);

            //リストの更新.
            this.AddressList.DummyAlloc( this.EventAsm.Count, this.AddressList.SelectedIndex);
        }

        private void UNDOButton_Click(object sender, EventArgs e)
        {
            RunUndo();
        }

        private void REDOButton_Click(object sender, EventArgs e)
        {
            RunRedo();
        }

        private void ScriptCodeName_DoubleClick(object sender, EventArgs e)
        {
            ScriptChangeButton.PerformClick();
        }

        void ShowFloatingControlpanelHandler(Object sender,EventArgs e)
        {
            ShowFloatingControlpanel();
        }

        private void EventScriptForm_Resize(object sender, EventArgs e)
        {
        }

        private void EventScriptForm_ResizeEnd(object sender, EventArgs e)
        {
        }

        private void ScriptCodeName_MouseEnter(object sender, EventArgs e)
        {
            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.EventAsm.Count)
            {
                return ;
            }
            EventScript.OneCode code = this.EventAsm[index];
            if (code.Script.PopupHint == "")
            {
                return;
            }
            this.ToolTip.FireEvent(this.ScriptCodeName, this.ScriptCodeName.Text + "\r\n" + code.Script.PopupHint);
        }

        private void ScriptCodeName_MouseLeave(object sender, EventArgs e)
        {
            this.ToolTip.HideEvent(sender, e);
        }

        public class NavigationEventArgs : EventArgs
        {
            public bool IsNewTab; //新しいタブを開いてほしい時
            public uint Address;  //表示するアドレス
        };

        //変更があったとき
        public EventHandler Navigation;

        public void SetFocus()
        {
            if (this.AddressList.Items.Count == 0)
            {//件数が0件ならアドレス
                this.ReadStartAddress.Focus();
            }
            else
            {//件数があるならリスト
                this.AddressList.Focus();
            }
        }

        private void AddressTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddressLabel_Click(sender, e);
        }

        private void AddressLabel_Click(object sender, EventArgs e)
        {
            PointerToolCopyToForm f = (PointerToolCopyToForm)InputFormRef.JumpFormLow<PointerToolCopyToForm>();
            f.Init((uint)U.atoh(AddressTextBox.Text));
            f.ShowDialog();
        }

        private void AddressTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void UseTemplate()
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                return;
            }

            EventScriptTemplateForm f = (EventScriptTemplateForm)InputFormRef.JumpFormLow<EventScriptTemplateForm>();
            f.Init(this.MapID, this);
            f.ShowDialog();

            if (f.DialogResult != DialogResult.OK)
            {
                return;
            }

            InsertEventTemplate(f.Codes);
        }

        private void TemplateButton_Click(object sender, EventArgs e)
        {
            UseTemplate();
        }

        public bool IsUseLabelID(uint checkLabelID)
        {
            for (int i = 0; i < this.EventAsm.Count; i++)
            {
                EventScript.OneCode code = this.EventAsm[i];
                uint cond_id = GetScriptLabelID(code);
                if (cond_id == U.NOT_FOUND)
                {
                    continue;
                }
                if (cond_id == checkLabelID)
                {
                    return true;
                }
            }
            return false;
        }
        //ポインタの更新の通知
        public void NotifyChangePointer(uint oldaddr, uint newaddr)
        {
            if (!U.isPointer(oldaddr) || ! U.isPointer(newaddr))
            {
                return;
            }
            EventScript.NotifyChangePointer(this.EventAsm , oldaddr , newaddr);
        }

    }
}
