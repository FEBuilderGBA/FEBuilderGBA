using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class AIScriptForm : Form
    {
        EventScriptForm.ScriptEditSetTable[] ScriptEditSetTables;

        public AIScriptForm()
        {
            InitializeComponent();

            Popup = new EventScriptPopupUserControl();
            this.AIAsm = new List<EventScript.OneCode>();

            this.Script.OwnerDraw(Draw, DrawMode.OwnerDrawVariable, false);
            this.InputFormRef = Init(this);
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEventNoCopyPointer;

            this.FilterComboBox.BeginUpdate();
            this.FilterComboBox.Items.Add(R._("0=AI1"));
            this.FilterComboBox.Items.Add(R._("1=AI2"));
            this.FilterComboBox.EndUpdate();

            ScriptEditSetTables = new EventScriptForm.ScriptEditSetTable[5];
            ScriptEditSetTables[0].ParamLabel = ParamLabel1; ScriptEditSetTables[0].ParamSrc = ParamSrc1; ScriptEditSetTables[0].ParamValue = ParamValue1; 
            ScriptEditSetTables[1].ParamLabel = ParamLabel2; ScriptEditSetTables[1].ParamSrc = ParamSrc2; ScriptEditSetTables[1].ParamValue = ParamValue2; 
            ScriptEditSetTables[2].ParamLabel = ParamLabel3; ScriptEditSetTables[2].ParamSrc = ParamSrc3; ScriptEditSetTables[2].ParamValue = ParamValue3; 
            ScriptEditSetTables[3].ParamLabel = ParamLabel4; ScriptEditSetTables[3].ParamSrc = ParamSrc4; ScriptEditSetTables[3].ParamValue = ParamValue4; 
            ScriptEditSetTables[4].ParamLabel = ParamLabel5; ScriptEditSetTables[4].ParamSrc = ParamSrc5; ScriptEditSetTables[4].ParamValue = ParamValue5; 

            for (int i = 0; i < 5; i++)
            {
                ScriptEditSetTables[i].ParamSrc.ValueChanged += ParamSrc_ValueChanged;
                ScriptEditSetTables[i].ParamSrc.Enter += ParamSrc_Focused;
                ScriptEditSetTables[i].ParamSrc.Leave += ParamSrc_UnFocused;

                ScriptEditSetTables[i].ParamLabel.Click += ParamLabel_Clicked;
                ScriptEditSetTables[i].ParamValue.DoubleClick += ParamLabel_Clicked;
            }

            InputFormRef.markupJumpLabel(AddressLabel);
            HideFloatingControlpanel(); 
            ClearUndoBuffer();

            U.SelectedIndexSafety(this.FilterComboBox , 0);
            InputFormRef.MakeEditListboxContextMenuN(this.Script, this.Script_KeyDown, this.Script_KeyDown, useTemplate: false);

            U.SetIcon(EventToFileButton, Properties.Resources.icon_arrow);
            U.SetIcon(FileToEventButton, Properties.Resources.icon_upload);
        }
        //リストが拡張された分のポインタをNULLにする.
        void AddressListExpandsEventNoCopyPointer(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            //増えた分のP4をゼロにする.
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ClearPointer");
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);
            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                Program.ROM.write_u32(addr, 0, undodata);
                addr += eearg.BlockSize;
            }
            if (eearg.NewDataCount > eearg.OldDataCount)
            {
                //末尾のデータを0xffにする.
                Program.ROM.write_u32(addr, U.NOT_FOUND, undodata);
            }

            Program.Undo.Push(undodata);

            ReloadAISetting();

            eearg.IsReload = true;
        }
        //AI設定の再読み込み.
        void ReloadAISetting()
        {
            if (this.FilterComboBox.SelectedIndex == 1)
            {
                EventUnitForm.PreLoadResourceAI2(U.ConfigDataFilename("ai2_"));
            }
            else
            {
                EventUnitForm.PreLoadResourceAI1(U.ConfigDataFilename("ai1_"));
            }
        }
        
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ai1_pointer()
                , 4
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    if (!U.isPointerOrNULL(a))
                    {
                        return false;
                    }

                    if (U.isExtrendsROMArea(addr))
                    {//拡張済みなのでサイズは終端まで
                    }
                    else
                    {//未拡張
                        uint baseaddr = addr - (uint)(4 * i);
                        if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.ai1_pointer()))
                        {
                            if (i >= EventUnitForm.AI1.Count)
                            {
                                return false;
                            }
                        }
                        else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.ai2_pointer()))
                        {
                            if (i >= EventUnitForm.AI2.Count)
                            {
                                return false;
                            }
                        }
                    }


                    return true;
                }
                , (int i, uint addr) =>
                {
                    string name = "";
                    uint baseaddr = addr - (uint)(4 * i);
                    if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.ai1_pointer()))
                    {
                        if (i < EventUnitForm.AI1.Count)
                        {
                            name = EventUnitForm.AI1[i].Name;
                        }
                        else
                        {
                            name = U.ToHexString(i);
                        }
                    }
                    else if (baseaddr == Program.ROM.p32(Program.ROM.RomInfo.ai2_pointer()))
                    {
                        if (i < EventUnitForm.AI2.Count)
                        {
                            name = EventUnitForm.AI2[i].Name;
                        }
                        else
                        {
                            name = U.ToHexString(i);
                        }
                    }
                    return name;
                }
                );
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            U.ReSelectList(this.FilterComboBox);
            U.ReSelectList(this.AddressList);
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef == null)
            {
                return;
            }

            int selected = this.FilterComboBox.SelectedIndex;
            if (selected == 0)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.ai1_pointer()));
            }
            else if (selected == 1)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.ai2_pointer()));
            }
        }



        public static uint DataCount(uint pointer = U.NOT_FOUND)
        {
            InputFormRef InputFormRef = Init(null);
            if (pointer != U.NOT_FOUND)
            {
                InputFormRef.ReInitPointer(pointer);
            }
            return InputFormRef.DataCount;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            uint[] addlist = new uint[] { Program.ROM.RomInfo.ai1_pointer(), Program.ROM.RomInfo.ai2_pointer()};

            for (int aiType = 0; aiType < addlist.Length; aiType++)
            {
                uint aiAddr = addlist[aiType];
                if (aiAddr == 0)
                {
                    continue;
                }
                MakeAllDataLengthSub(list,isPointerOnly,aiAddr,aiType);
            }
        }
        public static void MakeAllDataLengthSub(List<Address> list, bool isPointerOnly,uint aiAddr,int aiType)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(aiAddr);
            string name = "AI" + (aiType + 1);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 });

            uint p = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                if (!U.isSafetyOffset(p))
                {
                    continue;
                }

                name = "AI" + (aiType + 1) + " ";
                if (aiType == 0)
                {
                    name += EventUnitForm.GetAIName1(i);
                }
                else
                {
                    name += EventUnitForm.GetAIName2(i);
                }

                uint aiscript = Program.ROM.p32(p);
                uint length = CalcLength(aiscript);

                FEBuilderGBA.Address.AddAddress(list, aiscript, length, p, name, FEBuilderGBA.Address.DataTypeEnum.AISCRIPT);

                uint end = aiscript + length;
                for (uint k = aiscript; k < end; k += 16)
                {
                    uint pp;
                    pp = Program.ROM.u32(k + 8);
                    if (U.isPointer(pp))
                    {
                        if ((pp % 2) == 1)
                        {//thumbプログラムコード
                            FEBuilderGBA.Address.AddFunction(list, k + 8, name + " CallASM");
                        }
                        else
                        {//データ
                            FEBuilderGBA.Address.AddAddress(list, pp
                                , isPointerOnly ? 0 : AIUnitsForm.CalcLength(pp)
                                , k + 8, name
                                , FEBuilderGBA.Address.DataTypeEnum.BIN);
                        }
                    }
                    pp = Program.ROM.u32(k + 12);
                    if (U.isPointer(pp))
                    {
                        FEBuilderGBA.Address.AddAddress(list, pp
                            , isPointerOnly ? 0 : AIUnitsForm.CalcLength(pp)
                            , k + 12, name
                            , FEBuilderGBA.Address.DataTypeEnum.BIN);
                    }
                }
            }
        }

        //エラーチェック
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            {
                InputFormRef InputFormRef = Init(null);
                InputFormRef.ReInitPointer(Program.ROM.RomInfo.ai1_pointer());
                if (InputFormRef.DataCount <= 5)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.AISCRIPT, U.NOT_FOUND
                        , R._("AI1のデータが極端に少ないです。破損している可能性があります。")));
                }
            }
            {
                InputFormRef InputFormRef = Init(null);
                InputFormRef.ReInitPointer(Program.ROM.RomInfo.ai2_pointer());
                if (InputFormRef.DataCount <= 5)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.AISCRIPT, U.NOT_FOUND
                        , R._("AI2のデータが極端に少ないです。破損している可能性があります。")));
                }
            }
        }

        List<EventScript.OneCode> AIAsm;
        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint baseaddr = InputFormRef.SelectToAddr(this.AddressList);

            HideFloatingControlpanel();
            this.AIAsm.Clear();
            this.Script.Items.Clear();
            if (!U.isSafetyOffset(baseaddr))
            {
                this.N_ReadCount.Value = 0;
                return;
            }
            uint addr = Program.ROM.p32(baseaddr);
            if (!U.isSafetyOffset(addr))
            {
                this.N_ReadCount.Value = 0;
                return;
            }
            this.Address.Value = addr;
            this.N_ReadCount.Value = (uint)CalcLength(addr);
            this.N_ReloadListButton.PerformClick();
        }

        //長さを求める.
        static uint CalcLength(uint addr)
        {
            uint start = addr;
            uint limit = (uint)Program.ROM.Data.Length;
            
            while (addr + 16 <= limit)
            {
                uint code = Program.ROM.u8(addr + 0);
                addr += 16; //命令は16バイト固定.
                if (addr + 16 > limit)
                {
                    break;
                }
                if (code == 0x03)
                {//EXIT
                    //1命令先読みして1Bラベルがあるかどうかを見るあるならまだ続く
                    uint nextcode = Program.ROM.u8(addr + 0);
                    if (! (nextcode == 0x1B || nextcode == 0x1C))
                    {//1B or 1Cではないので終わり.
                        break;
                    }
                    addr += 16;
                }
            }
            return addr - start;
        }

        private Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.AIAsm.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            EventScript.OneCode code = this.AIAsm[index];
            return EventScriptForm.DrawCode(lb, g, listbounds, isWithDraw, code);
        }



        EventScriptPopupUserControl Popup;
        private void Script_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
        }
        private void OneLineDisassembler()
        {
            int i;
            if (this.ASMTextBox.Text.Length < 16)
            {//命令は最低でも16バイトでないとダメ.
                for (i = this.ASMTextBox.Text.Length; i < 16; i++)
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
            EventScript.OneCode code = Program.AIScript.DisAseemble(selectedByteData, 0, hint);

            //命令を選択.
            this.ScriptCodeName.Text = EventScript.makeCommandComboText(code.Script,false);

            //引数
            int n = 0;
            for (i = 0; i < 5; i++, n++)
            {
                if (n >= code.Script.Args.Length)
                {
                    break;
                }
                EventScript.Arg arg = code.Script.Args[n];
                if (EventScript.IsFixedArg(arg))
                {//固定値になっているところはパラメータを出さない.
                    i--;
                    continue;
                }

                uint v = EventScript.GetArgValue(code, arg);
                EventScriptForm.SetOneScriptEditSetTables(ScriptEditSetTables[i], arg, v);
            }

            for (; i < 5; i++)
            {
                //使わないパラメータはあっかりーんする
                EventScriptForm.HideOneScriptEditSetTables(ScriptEditSetTables[i]);
            }
            int y = ShowFloatingControlpanelInner(code, this.Script.SelectedIndex);
            ControlPanel.Location = new Point(ControlPanel.Location.X, y);
        }

        bool Get_Select_ParamSrc_Object(object sender
            , out EventScript.OneCode outCode //編集しているコード
            , out int outSelectID //選択されたNumboxのindex(label等に関連づく)
            , out int outArg  //引数の数(FIxed等非表示の引数があるので調べる)
            )
        {
            outCode = null;
            outSelectID = 0;
            outArg = 0;
            if (this.Script.SelectedIndex < 0 || this.Script.SelectedIndex >= this.AIAsm.Count)
            {
                return false;
            }

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);
            string hint = this.ScriptCodeName.Text;

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.AIScript.DisAseemble(selectedByteData, 0, hint);

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
                        return false;
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
            uint value = EventScriptForm.WriteOneScriptEditSetTables(ScriptEditSetTables[selectID], arg, code);
            EventScriptForm.WriteAliasScriptEditSetTables(ScriptEditSetTables[selectID], arg, code);

            bool isOrderOfHuman = (this.ActiveControl == sender); //人間の操作によるものか
            string text = "";
            Bitmap backgroundImage = null;
            string errormessage = "";
            if (arg.Type == EventScript.ArgType.UNIT)
            {
                text = UnitForm.GetUnitName(value);
            }
            else if (arg.Type == EventScript.ArgType.CLASS)
            {
                text = ClassForm.GetClassName(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT)
            {
                backgroundImage = AIUnitsForm.DrawAIUnitsList(value, ScriptEditSetTables[selectID].ParamValue.Height - 2);
            }
            else if (arg.Type == EventScript.ArgType.MAPX || arg.Type == EventScript.ArgType.MAPY)
            {
                NumericUpDown xObj;
                NumericUpDown yObj;
                if (arg.Type == EventScript.ArgType.MAPX)
                {
                    xObj = (NumericUpDown)sender;
                    yObj = ScriptEditSetTables[selectID + 1].ParamSrc;
                }
                else
                {
                    xObj = ScriptEditSetTables[selectID - 1].ParamSrc;
                    yObj = (NumericUpDown)sender;
                }

                if (isOrderOfHuman)
                {
                    Popup.MAP.SetPoint(this.Script.SelectedIndex.ToString(), (int)xObj.Value, (int)yObj.Value);
                }
                else
                {
                    //隣のフォームから入れている場合は、人間の操作といえるだろう
                    isOrderOfHuman = (xObj == sender || yObj == sender);
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_ASM)
            {
                text = Program.AsmMapFileAsmCache.GetASMName(value, false, out errormessage);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AITILE)
            {
                text = AITilesForm.GetNames(value);
            }
            else if (arg.Type == EventScript.ArgType.TILE)
            {
                text = MapTerrainNameForm.GetNameExcept00(value);
            }
            else if (arg.Type == EventScript.ArgType.SUPPORT_LEVEL)
            {
                text = InputFormRef.GetSuportLevel(value);
            }
            else if (arg.Type == EventScript.ArgType.EDITION)
            {
                text = InputFormRef.GetEditon(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICOORDINATE)
            {
                if (U.toOffset(value) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = AIASMCoordinateForm.GetCoordPreview(U.toOffset(value));
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT4)
            {
                if (U.toOffset(value) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = AIASMUnit4Form.GetUnit4Preview(U.toOffset(value));
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AICALLTALK)
            {
                if (U.toOffset(value) == 0)
                {
                    text = R._("ラベルをクリックして領域を確保してください");
                }
                else
                {
                    text = AIASMCALLTALKForm.GetUnit2Preview(U.toOffset(value));
                }
            }

            ScriptEditSetTables[selectID].ParamValue.Text = text;
            ScriptEditSetTables[selectID].ParamValue.BackgroundImage = backgroundImage;
            ScriptEditSetTables[selectID].ParamValue.ErrorMessage = errormessage;

            if (isOrderOfHuman)
            {//現在このコントロールから値を入力している場合は、他のコントロールも連動して変更する
                this.ASMTextBox.Text = U.convertByteToStringDump(code.ByteData);

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

            EventScript.Arg arg = code.Script.Args[argindex];
            uint value = EventScriptForm.GetValueOneScriptEditSetTables(ScriptEditSetTables[selectID], arg);
            if (arg.Type == EventScript.ArgType.UNIT)
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    UnitForm f = (UnitForm)InputFormRef.JumpForm<UnitForm>(value - 1);
                    MakeInjectionCallback(f
                        , this.Script
                        , src_object, false);
                }
                else if (Program.ROM.RomInfo.version() >= 7)
                {//FE7
                    UnitFE7Form f = (UnitFE7Form)InputFormRef.JumpForm<UnitFE7Form>(value - 1);
                    MakeInjectionCallback(f
                        , this.Script
                        , src_object, false);
                }
                else
                {//FE6
                    UnitFE6Form f = (UnitFE6Form)InputFormRef.JumpForm<UnitFE6Form>(value - 1);
                    MakeInjectionCallback(f
                        , this.Script
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.CLASS)
            {
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 - FE8
                    ClassForm f = (ClassForm)InputFormRef.JumpForm<ClassForm>(value);
                    MakeInjectionCallback(f
                        , this.Script
                        , src_object, false);
                }
                else
                {//FE6
                    ClassFE6Form f = (ClassFE6Form)InputFormRef.JumpForm<ClassFE6Form>(value);
                    MakeInjectionCallback(f
                        , this.Script
                        , src_object, false);
                }
            }
            else if (arg.Type == EventScript.ArgType.POINTER_ASM)
            {
                DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>(U.NOT_FOUND);
                f.JumpTo(DisassemblerTrumb.ProgramAddrToPlain(value));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT)
            {
                AIUnitsForm f = (AIUnitsForm)InputFormRef.JumpFormLow<AIUnitsForm>();
                f.JumpTo(value);
                f.ShowDialog();
                U.SetActiveControl(src_object);
                U.ForceUpdate(src_object, U.toPointer(f.GetBaseAddress()));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_AITILE)
            {
                AITilesForm f = (AITilesForm)InputFormRef.JumpFormLow<AITilesForm>();
                f.JumpTo(value);
                f.ShowDialog();
                U.SetActiveControl(src_object);
                U.ForceUpdate(src_object, U.toPointer(f.GetBaseAddress()));
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
            
        }
        //adresslist doubleclickで、呼び出し元の injecttionCallbackへ値を代入. フォームを閉じる
        void MakeInjectionCallback(Form f, ListBox addressList, NumericUpDown injectionCallback, bool isGetAddr)
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
                    value = (uint)addressList.SelectedIndex;
                }
                injectionCallback.Focus(); //フォーカスの是非を見ているので一度フォーカス一度が必要.
                U.ForceUpdate(injectionCallback,value);
                HideFloatingControlpanel(); //popアップがでることがあるので黙らせる. 
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
            if (arg.Type == EventScript.ArgType.MAPX || arg.Type == EventScript.ArgType.MAPY)
            {
                uint mapid = 0;
                NumericUpDown xObj;
                NumericUpDown yObj;
                if (arg.Type == EventScript.ArgType.MAPX)
                {
                    xObj = (NumericUpDown)sender;
                    yObj = ScriptEditSetTables[selectID + 1].ParamSrc;
                }
                else
                {
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

                    U.ForceUpdate(xObj , x);
                    U.ForceUpdate(yObj , y);

                    updateLock = false;
                    return 0;
                };

                PopupDialog_MAP((NumericUpDown)sender, mapid, (int)xObj.Value, (int)yObj.Value, movecallback);
            }
            else
            {
                Popup.Visible = false;
            }
        }

        void ParamSrc_UnFocused(object sender, EventArgs e)
        {
            Popup.Visible = false;
        }
        void PopupDialog_MAP(NumericUpDown sender, uint value, int x, int y, Func<int, int, int> movecallback)
        {
            Popup.Visible = true; //デザイナーのバグがあり、コンストラクタで初期化できないので、 Load イベントで初期化するため、ここで表示する.
            Popup.LoadMap(value);
            Popup.MAP.SetPoint(this.Script.SelectedIndex.ToString(), x, y);
            Popup.MAP.setNotifyMode(this.Script.SelectedIndex.ToString(), movecallback);

            AjustPopup(sender);
        }
        //ポップアップの位置調整
        void AjustPopup(NumericUpDown sender)
        {
            Popup.Visible = true;

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
        }

        private void Script_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowFloatingControlpanel();
        }
        private void HideFloatingControlpanel()
        {
            if (ControlPanel.Visible)
            {
                ControlPanel.Hide();
                this.Popup.Visible = false;
                Script.Focus();
            }
        }


        private void Script_KeyDown(object sender, KeyEventArgs e)
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
            else if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
                return;
            }

            int i = this.Script.SelectedIndex;
            if (i < 0)
            {
                return;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                U.SetClipboardText(EventToTextOne(i));
                U.SelectedIndexSafety(this.Script, i);
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                TextToEvent(Clipboard.GetText(), i);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                this.RemoveButton_Click(null,null);
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
            else if (e.Control && e.KeyCode == Keys.N)
            {
                ChangeDirect();
                return;
            }
            else if (e.KeyCode == Keys.Return)
            {
                ShowFloatingControlpanel();
                return;
            }
            else if ((e.Control && e.KeyCode == Keys.Divide) || (e.Control && e.KeyCode == Keys.OemQuestion))
            {
                ChangeComment();
                return;
            }
        }
        private byte[] LineToEventByte(string line)
        {
            List<byte> ret = new List<byte>();

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
                if (!U.ishex(line[i + 1]))
                {
                    break;
                }

                byte b = (byte)U.atoh(line.Substring(i, 2));
                ret.Add(b);
            }
            return ret.ToArray();
        }

        private void TextToEvent(string str, int insertPoint = -1, bool isClear = false)
        {
            this.PushUndo();

            if (isClear)
            {
                this.AIAsm.Clear();
            }

            //追加した場所.
            int insertedPoint;
            if (insertPoint <= -1)
            {
                insertedPoint = this.AIAsm.Count;
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

                EventScript.OneCode code = Program.AIScript.DisAseemble(bin, 0);
                if (insertPoint <= -1)
                {//末尾に追加.
                    this.AIAsm.Add(code);
                }
                else
                {//特定場所に追加.
                    this.AIAsm.Insert(insertPoint, code);
                }
            }

           this.Script.DummyAlloc(this.AIAsm.Count, insertedPoint);
        }

        private string EventToTextOne(int number)
        {
            StringBuilder sb = new StringBuilder();
            EventScript.OneCode code = this.AIAsm[number];
            return EventScriptInnerControl.EventToTextOne(code);
        }

        int ShowFloatingControlpanelInner(EventScript.OneCode code, int index)
        {
            if (index < 0 || index >= this.Script.Items.Count)
            {
                return 0;
            }
            //パラメータ数が少ない場合、ダイアログを上に引き揚げます.
            int heighest = ParamSrc5.Location.Y + ParamSrc5.Size.Height;
            {
                ControlPanelCommand.Location = new Point(0, heighest);
                ControlPanel.Height = heighest + ControlPanelCommand.Size.Height;
            }

            //編集する項目の近くに移動させます.
            Rectangle rect = this.Script.GetItemRectangle(index);
            int y = this.ListBoxPanel.Location.Y
                + this.Script.Location.Y
                + rect.Y + rect.Height + 20
                ;
            if (y + ControlPanel.Height >= Script.Height)
            {//下に余白がないので上に出す.
                y = this.ListBoxPanel.Location.Y
                    + this.Script.Location.Y
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
            int index = this.Script.SelectedIndex;
            if (index < 0 || index >= this.AIAsm.Count)
            {//一件もない
                this.ASMTextBox.Text = "";
                this.ScriptCodeName.Text = "";
                this.CommentTextBox.Text = "";
                this.AddressTextBox.Text = "";
                OneLineDisassembler();
            }
            else
            {
                EventScript.OneCode code = this.AIAsm[index];

                //コードを書く
                this.ASMTextBox.Text = U.convertByteToStringDump(code.ByteData);
                this.ScriptCodeName.Text = EventScript.makeCommandComboText(code.Script,false);
                this.CommentTextBox.Text = code.Comment;
                this.AddressTextBox.Text = U.ToHexString(EventScript.ConvertSelectedToAddr((uint)this.Address.Value, index, this.AIAsm));

                //Eventをデコードして、引数等を求めなおす.
                OneLineDisassembler();
            }
            //変更ボタンが光っていたら、それをやめさせる.
            InputFormRef.WriteButtonToYellow(this.UpdateButton, false);
            InputFormRef.WriteButtonToYellow(this.NewButton, false);

            ControlPanel.Show();
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
            p.EventAsm = EventScript.CloneEventList(this.AIAsm);
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
            this.AIAsm = EventScript.CloneEventList(u.EventAsm);

            //リストの更新.
           this.Script.DummyAlloc(this.AIAsm.Count, this.Script.SelectedIndex);
        }


        private void UpdateButton_Click(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            if (this.Script.SelectedIndex < 0 || this.Script.SelectedIndex >= this.AIAsm.Count)
            {//追加で処理する.
                NewButton.PerformClick();
                return;
            }
            OneLineDisassembler();

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.AIScript.DisAseemble(selectedByteData, 0);
            code.Comment = this.CommentTextBox.Text;

            //選択されているコードを入れ替える.
            this.AIAsm[this.Script.SelectedIndex] = code;

            //リストの更新.
           this.Script.DummyAlloc(this.AIAsm.Count, -1);


            HideFloatingControlpanel();

        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (this.Script.SelectedIndex < 0 || this.Script.SelectedIndex >= this.AIAsm.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            this.AIAsm.RemoveAt(this.Script.SelectedIndex);

            //リストの更新.
           this.Script.DummyAlloc(this.AIAsm.Count, this.Script.SelectedIndex - 1);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();

        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (this.AIAsm == null)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            OneLineDisassembler();

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.AIScript.DisAseemble(selectedByteData, 0);
            code.Comment = this.CommentTextBox.Text;

            int selected;
            //選択されている部分に追加
            if (this.Script.SelectedIndex < 0)
            {
                this.AIAsm.Add(code);
                selected = this.AIAsm.Count - 1;
            }
            else
            {
                this.AIAsm.Insert(this.Script.SelectedIndex + 1, code);
                selected = this.Script.SelectedIndex + 1;
            }

            //リストの更新.
           this.Script.DummyAlloc(this.AIAsm.Count, selected);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel(); 
            
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            //コントロールパネルを閉じる.
            HideFloatingControlpanel();
        }

        private void ScriptChangeButton_Click(object sender, EventArgs e)
        {
            AIScriptCategorySelectForm form = (AIScriptCategorySelectForm)InputFormRef.JumpFormLow<AIScriptCategorySelectForm>();
            DialogResult dr = form.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            EventScript.Script script = form.Script;

            //選択した命令を代入
            byte[] selectedByteData = script.Data;
            this.ASMTextBox.Text = U.convertByteToStringDump(selectedByteData);
            this.ScriptCodeName.Text = EventScript.makeCommandComboText(script,false);

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

        void FireChanegCommandByDirectMode()
        {
            AIScriptCategorySelectForm form = (AIScriptCategorySelectForm)InputFormRef.JumpFormLow<AIScriptCategorySelectForm>();
            DialogResult dr = form.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                //直接編集モードだったら、キャンセルされた場合は、コントロールパネルも消す.
                HideFloatingControlpanel();
                return;
            }

            EventScript.Script script = form.Script;

            //選択した命令を代入
            byte[] selectedByteData = script.Data;
            this.ASMTextBox.Text = U.convertByteToStringDump(selectedByteData);
            this.ScriptCodeName.Text = EventScript.makeCommandComboText(script,false);

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

        bool IsWritable()
        {
            if (this.AddressList.SelectedIndex > 0)
            {
                return true;
            }
            DialogResult dr = R.ShowNoYes("AI0を書き換えるのは危険です。\r\nAI0を書き換えると敵フェーズでフリーズすることがあります。\r\nそれでも、続行してもよろしいですか？");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return false;
            }
            return true;
        }
        uint GetWriteAddr(uint baseaddr)
        {
            uint addr = Program.ROM.p32(baseaddr);
            if (! U.isSafetyOffset(addr))
            {
                return 0;
            }
            if (U.isPointer(addr))
            {
                addr = U.toOffset(addr);
            }
            if (!U.CheckZeroAddressWriteHigh(addr))
            {
                return 0;
            }
            if (!U.CheckPaddingALIGN4(addr))
            {
                return 0;
            }

            bool r = IsDuplicateAddr(addr, baseaddr);
            if (!r)
            {
                return 0;
            }
            return addr;
        }
        bool IsDuplicateAddr(uint current_addr,uint baseaddr)
        {
            uint[] addlist = new uint[] { Program.ROM.RomInfo.ai1_pointer(), Program.ROM.RomInfo.ai2_pointer()};

            for (int aiType = 0; aiType < addlist.Length; aiType++)
            {
                uint aiAddr = addlist[aiType];
                if (aiAddr == 0)
                {
                    continue;
                }
                InputFormRef IFR = Init(null);
                IFR.ReInitPointer(aiAddr);
                uint p = InputFormRef.BaseAddress;
                for (uint i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    if (p == baseaddr)
                    {//自分自身
                        continue;
                    }
                    if (!U.isSafetyOffset(p))
                    {
                        continue;
                    }
                    uint addr = Program.ROM.p32(p);
                    if (addr == current_addr)
                    {//自分以外で使われている
                        return true;
                    }
                }
            }
            //自分しか使っていない
            return false;
        }


        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {
                R.ShowStopError(InputFormRef.GetBusyErrorExplain());
                return;
            }
            if (this.AIAsm == null || this.AIAsm.Count <= 0)
            {
                R.ShowStopError("リストが空なので書き込めません");
                return;
            }
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            if (! IsWritable())
            {
                return;
            }

            uint baseaddr = InputFormRef.SelectToAddr(this.AddressList);
            if (!U.isSafetyOffset(baseaddr))
            {
                return;
            }
            uint addr = GetWriteAddr(baseaddr);

            //終端コードの探索
            List<byte> databyte = new List<byte>();
            byte lastCode=0;
            for (int i = 0; i < this.AIAsm.Count; i++)
            {
                databyte.AddRange(this.AIAsm[i].ByteData);
                lastCode = this.AIAsm[i].ByteData[0];
            }

            //終端コードがない場合は、一番下に追加.
            if (lastCode != 0x03)
            {
                byte[] termCode = new byte[16] { 0x03, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                databyte.AddRange(termCode);
            }

            string undoname;
            if (addr == 0)
            {
                undoname = this.Text + ":NEW";
            }
            else
            {
                undoname = this.Text + ":" + U.ToHexString(addr);
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,undoname);

            uint newaddr = InputFormRef.WriteBinaryData(this
                , addr
                , databyte.ToArray()
                , get_data_pos_callback
                , undodata
            );
            Program.CommentCache.UpdateCode(newaddr, (uint)ReadCount.Value, this.AIAsm);

            Program.ROM.write_p32(baseaddr, newaddr, undodata);

            Program.Undo.Push(undodata);

            ReloadAISetting();

            InputFormRef.ReloadAddressList();
//            U.ReSelectList(this.AddressList);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
        }


        private MoveToUnuseSpace.ADDR_AND_LENGTH get_data_pos_callback(uint addr)
        {
            uint length = CalcLength(addr);

            //範囲外探索 00 00 00 00 が続く限り検索してみる.
            uint more = MoveToFreeSapceForm.SearchOutOfRange(addr + length);
            //16バイトアライメント
            more = (more / 16) * 16;

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = length + more;
            return aal;
        }

        private void AIScriptForm_KeyDown(object sender, KeyEventArgs e)
        {

        }
        public void ChangeDirect()
        {
            HideFloatingControlpanel();
            int i = this.Script.SelectedIndex;
            if (i < 0 || i >= this.Script.Items.Count)
            {
                return;
            }
            ShowFloatingControlpanel();
            ScriptChangeButton.Focus();
            FireChanegCommandByDirectMode();
        }

        public void ChangeComment()
        {
            int i = this.Script.SelectedIndex;
            if (i < 0 || i >= this.Script.Items.Count)
            {
                return;
            }
            ShowFloatingControlpanel();
            CommentTextBox.Focus();
        }

        private void N_ReloadListButton_Click(object sender, EventArgs e)
        {
            this.AIAsm.Clear();
            this.Script.Items.Clear();

            uint addr = (uint)this.Address.Value;
            uint bytecount = (uint)this.N_ReadCount.Value;

            uint limit = Math.Min(addr + bytecount, (uint)Program.ROM.Data.Length);
            while (addr < limit)
            {
                EventScript.OneCode code = Program.AIScript.DisAseemble(Program.ROM.Data, addr);
                this.AIAsm.Add(code);

                addr += (uint)code.Script.Size;
            }

           this.Script.DummyAlloc(this.AIAsm.Count, 0);
        }

        private void Address_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.N_ReloadListButton.PerformClick();
                return;
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

        private string EventToTextAll()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.AIAsm.Count; i++)
            {
                sb.Append(EventToTextOne(i));
            }
            return sb.ToString();
        }

        private void EventToFileButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|EA|*.event|All files|*");

            uint addr = U.toOffset((uint)this.Address.Value);

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, U.To0xHexString(addr));

            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileName;
            Program.LastSelectedFilename.Save(this, "", save);

            string ext = U.GetFilenameExt(filename);
            U.WriteAllText(filename, EventToTextAll());

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void FileToEventButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

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
            {
                string text = File.ReadAllText(filename);

                TextToEvent(text, -1, true);
            }
        }
        //ポインタの更新の通知
        public void NotifyChangePointer(uint oldaddr, uint newaddr)
        {
            Debug.Assert(U.isPointer(oldaddr));
            Debug.Assert(U.isPointer(newaddr));
            EventScript.NotifyChangePointer(this.AIAsm, oldaddr, newaddr);
        }

    }
}
