using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ProcsScriptInnerControl : UserControl
    {
        EventScriptForm.ScriptEditSetTable[] ScriptEditSetTables;

        public ProcsScriptInnerControl()
        {
            InitializeComponent();
            this.ProcsScript = new List<EventScript.OneCode>();
            this.Script.OwnerDraw(Draw, DrawMode.OwnerDrawVariable, false);

            ScriptEditSetTables = new EventScriptForm.ScriptEditSetTable[2];
            ScriptEditSetTables[0].ParamLabel = ParamLabel1; ScriptEditSetTables[0].ParamSrc = ParamSrc1; ScriptEditSetTables[0].ParamValue = ParamValue1;
            ScriptEditSetTables[1].ParamLabel = ParamLabel2; ScriptEditSetTables[1].ParamSrc = ParamSrc2; ScriptEditSetTables[1].ParamValue = ParamValue2;

            for (int i = 0; i < 2; i++)
            {
                ScriptEditSetTables[i].ParamSrc.ValueChanged += ParamSrc_ValueChanged;
                ScriptEditSetTables[i].ParamSrc.Enter += ParamSrc_Focused;
                ScriptEditSetTables[i].ParamSrc.Leave += ParamSrc_UnFocused;

                ScriptEditSetTables[i].ParamLabel.Click += ParamLabel_Clicked;
                ScriptEditSetTables[i].ParamValue.DoubleClick += ParamLabel_Clicked;
            }
            U.SetIcon(EventToFileButton, Properties.Resources.icon_arrow);
            U.SetIcon(FileToEventButton, Properties.Resources.icon_upload);
        }

        List<EventScript.OneCode> ProcsScript;

        //長さを求める.
        static uint CalcLength(uint addr)
        {
            uint start = addr;
            uint limit = (uint)Program.ROM.Data.Length;

            while (addr + 8 <= limit)
            {
                uint code = Program.ROM.u16(addr + 0);
                uint sarg = Program.ROM.u16(addr + 2);
                uint parg = Program.ROM.u32(addr + 4);
                addr += 8; //命令は8バイト固定.
                if (addr + 8 > limit)
                {
                    break;
                }
                
                if (code == 0x0C)
                {//parg is null 
                    if (sarg == 0)
                    {//GOTO LABEL 0があった
                        //Goto 0の時だけは、pargにゴミが入るときがある.

                        //先読みをしてみる.
                        uint sakiyomi = ProcsScriptForm.CalcLengthAndCheck(addr + 8);
                        if (sakiyomi == U.NOT_FOUND)
                        {//この先が壊れているなら、自分が終端.
                            if (start == addr)
                            {//自分が終端なのに、それが最初に出てくるのはおかしいよね.
                                return U.NOT_FOUND;
                            }
                            break;
                        }
                    }
                    else if (parg != 0)
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                }

                if (code == 0x800)
                {//EXIT その3
                    break;
                }

                if (code == 0x00)
                {//EXIT
                    break;
                }
            }
            return addr - start;
        }
        private Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.ProcsScript.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            EventScript.OneCode code = this.ProcsScript[index];
            return EventScriptForm.DrawCode(lb, g, listbounds, isWithDraw, code);
        }

        private void Script_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
        }
        private void OneLineDisassembler()
        {
            int i;
            if (this.ASMTextBox.Text.Length < 8)
            {//命令は最低でも8バイトでないとダメ.
                for (i = this.ASMTextBox.Text.Length; i < 8; i++)
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
            EventScript.OneCode code = Program.ProcsScript.DisAseemble(selectedByteData, 0, hint);

            //命令を選択.
            this.ScriptCodeName.Text = EventScript.makeCommandComboText(code.Script, false);

            //引数
            int n = 0;
            for (i = 0; i < 2; i++, n++)
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

            for (; i < 2; i++)
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
            if (this.Script.SelectedIndex < 0 || this.Script.SelectedIndex >= this.ProcsScript.Count)
            {
                return false;
            }

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);
            string hint = this.ScriptCodeName.Text;

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.ProcsScript.DisAseemble(selectedByteData, 0, hint);

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
            string errorMessage = "";
            if (arg.Type == EventScript.ArgType.TEXT
                || arg.Type == EventScript.ArgType.CONVERSATION_TEXT
                || arg.Type == EventScript.ArgType.SYSTEM_TEXT
                || arg.Type == EventScript.ArgType.ONELINE_TEXT
                )
            {
                text = TextForm.DirectAndStripAllCode((value));
            }
            else if (arg.Type == EventScript.ArgType.POINTER_TEXT)
            {
                text = TextForm.Direct(value);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_PROCS)
            {
                text = Program.AsmMapFileAsmCache.GetASMName(value, true, out errorMessage);
            }
            else if (arg.Type == EventScript.ArgType.POINTER_ASM)
            {
                text = Program.AsmMapFileAsmCache.GetASMName(value, false, out errorMessage);
            }
            else if (arg.Type == EventScript.ArgType.FSEC)
            {//FSEC
                text = InputFormRef.GetFSEC(value);
            }
            else if (arg.Type == EventScript.ArgType.MAPXY)
            {//MAPXY
                text = InputFormRef.GetMAPXY(value);
            }

            ScriptEditSetTables[selectID].ParamValue.Text = text;
            ScriptEditSetTables[selectID].ParamValue.BackgroundImage = backgroundImage;
            ScriptEditSetTables[selectID].ParamValue.ErrorMessage = errorMessage;

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
            if (arg.Type == EventScript.ArgType.POINTER_TEXT)
            {
                CStringForm f = (CStringForm)InputFormRef.JumpForm<CStringForm>();
                f.Init(src_object);
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
            else if (arg.Type == EventScript.ArgType.POINTER_AIUNIT)
            {
                AIUnitsForm f = (AIUnitsForm)InputFormRef.JumpFormLow<AIUnitsForm>();
                f.JumpTo(value);
                f.ShowDialog();
                U.SetActiveControl(src_object);
                U.ForceUpdate(src_object, f.GetBaseAddress());
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
                }
                else
                {
                    value = (uint)addressList.SelectedIndex;
                }
                injectionCallback.Focus(); //フォーカスの是非を見ているので一度フォーカス一度が必要.
                U.ForceUpdate(injectionCallback, value);
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
                return;
            }

            EventScript.Arg arg = code.Script.Args[argindex];
            uint value = EventScriptForm.GetValueOneScriptEditSetTables(ScriptEditSetTables[selectID], arg);
            if (arg.Type == EventScript.ArgType.MAPX || arg.Type == EventScript.ArgType.MAPY)
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
                    return 0;
                };
            }
        }

        void ParamSrc_UnFocused(object sender, EventArgs e)
        {
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
            else if (e.KeyCode == Keys.Enter)
            {
                ShowFloatingControlpanel();
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
                this.Script.SelectedIndex = i;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                TextToEvent(Clipboard.GetText(), i);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                RemoveButton_Click(null, null);
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
                this.ProcsScript.Clear();
            }

            string[] lines = str.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                byte[] bin = LineToEventByte(lines[i]);
                if (bin.Length < 4)
                {//壊れているか違うコード
                    continue;
                }

                EventScript.OneCode code = Program.ProcsScript.DisAseemble(bin, 0);
                if (insertPoint <= -1)
                {//末尾に追加.
                    this.ProcsScript.Add(code);
                }
                else
                {//特定場所に追加.
                    this.ProcsScript.Insert(insertPoint, code);
                }
            }

           this.Script.DummyAlloc(this.ProcsScript.Count, this.Script.SelectedIndex);
        }

        private string EventToTextOne(int number)
        {
            StringBuilder sb = new StringBuilder();
            EventScript.OneCode code = this.ProcsScript[number];
            return EventScriptInnerControl.EventToTextOne(code);
        }

        int ShowFloatingControlpanelInner(EventScript.OneCode code, int index)
        {
            if (index < 0 || index >= this.Script.Items.Count)
            {
                return 0;
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
            if (index < 0 || index >= this.ProcsScript.Count)
            {//一件もない
                this.ASMTextBox.Text = "";
                this.ScriptCodeName.Text = "";
                this.CommentTextBox.Text = "";
                this.AddressTextBox.Text = "";
                OneLineDisassembler();
            }
            else
            {
                EventScript.OneCode code = this.ProcsScript[index];

                //コードを書く
                this.ASMTextBox.Text = U.convertByteToStringDump(code.ByteData);
                this.ScriptCodeName.Text = EventScript.makeCommandComboText(code.Script, false);
                this.CommentTextBox.Text = code.Comment;
                this.AddressTextBox.Text = U.ToHexString(EventScript.ConvertSelectedToAddr((uint)this.Address.Value, index, this.ProcsScript));

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
            p.EventAsm = EventScript.CloneEventList(this.ProcsScript);
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
            this.ProcsScript = EventScript.CloneEventList(u.EventAsm);

            //リストの更新.
           this.Script.DummyAlloc(this.ProcsScript.Count, this.Script.SelectedIndex);
        }


        private void UpdateButton_Click(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            if (this.Script.SelectedIndex < 0 || this.Script.SelectedIndex >= this.ProcsScript.Count)
            {//追加で処理する.
                NewButton.PerformClick();
                return;
            }
            OneLineDisassembler();

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.ProcsScript.DisAseemble(selectedByteData, 0);
            code.Comment = this.CommentTextBox.Text;

            //選択されているコードを入れ替える.
            this.ProcsScript[this.Script.SelectedIndex] = code;

            //リストの更新.
           this.Script.DummyAlloc(this.ProcsScript.Count, -1);

            HideFloatingControlpanel();

        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (this.Script.SelectedIndex < 0 || this.Script.SelectedIndex >= this.ProcsScript.Count)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            this.ProcsScript.RemoveAt(this.Script.SelectedIndex);

            //リストの更新.
            this.Script.DummyAlloc(this.ProcsScript.Count, this.Script.SelectedIndex - 1);

            //コントロールパネルを閉じる.
            HideFloatingControlpanel();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            if (this.ProcsScript == null)
            {
                return;
            }
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            PushUndo();

            OneLineDisassembler();

            //文字列からバイト列に
            byte[] selectedByteData = U.convertStringDumpToByte(this.ASMTextBox.Text);

            //バイト列をイベント命令としてDisassembler.
            EventScript.OneCode code = Program.ProcsScript.DisAseemble(selectedByteData, 0);
            code.Comment = this.CommentTextBox.Text;

            int selected;
            //選択されている部分に追加
            if (this.Script.SelectedIndex < 0)
            {
                this.ProcsScript.Add(code);
                selected = this.ProcsScript.Count - 1;
            }
            else
            {
                this.ProcsScript.Insert(this.Script.SelectedIndex + 1, code);
                selected = this.Script.SelectedIndex + 1;
            }

            //リストの更新.
           this.Script.DummyAlloc(this.ProcsScript.Count, selected);

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
            ProcsScriptCategorySelectForm form = (ProcsScriptCategorySelectForm)InputFormRef.JumpFormLow<ProcsScriptCategorySelectForm>();
            DialogResult dr = form.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            EventScript.Script script = form.Script;

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
            }
        }

        void FireChanegCommandByDirectMode()
        {
            ProcsScriptCategorySelectForm form = (ProcsScriptCategorySelectForm)InputFormRef.JumpFormLow<ProcsScriptCategorySelectForm>();
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
            this.ScriptCodeName.Text = EventScript.makeCommandComboText(script, false);

            //イベントを逆アセンブルして確定する.
            OneLineDisassembler();
            //値1を自動選択
            if (ParamSrc1.Visible)
            {
                ParamSrc1.Focus();
            }
        }


        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(null))
            {
                R.ShowStopError(InputFormRef.GetBusyErrorExplain());
                return;
            }
            if (this.ProcsScript == null || this.ProcsScript.Count <= 0)
            {
                R.ShowStopError("リストが空なので書き込めません");
                return;
            }
            Form parent_self = U.ControlToParentForm(this);
            if (InputFormRef.IsPleaseWaitDialog(parent_self))
            {//2重割り込み禁止
                return;
            }
            uint addr = (uint)Address.Value;
            if (!U.CheckZeroAddressWriteHigh(addr))
            {
                return;
            }

            List<byte> databyte = new List<byte>();
            byte lastCode = 0;
            for (int i = 0; i < this.ProcsScript.Count; i++)
            {
                databyte.AddRange(this.ProcsScript[i].ByteData);
                lastCode = this.ProcsScript[i].ByteData[0];
            }

            //終端コードがない場合は、一番下に追加.
            if (lastCode != 0x00)
            {
                byte[] termCode = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                databyte.AddRange(termCode);
            }

            string undoname = this.Text + ":" + U.ToHexString(addr);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undoname);

            uint newaddr = InputFormRef.WriteBinaryData(parent_self
                , addr
                , databyte.ToArray()
                , get_data_pos_callback
                , undodata
            );
            Program.CommentCache.UpdateCode(newaddr, (uint)N_ReadCount.Value, this.ProcsScript);

            Program.Undo.Push(undodata);

            Address.Value = newaddr;
            FireAddressListExpandsEvent(addr, newaddr, (uint)databyte.Count);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(parent_self, newaddr);

            N_ReloadListButton_Click(null, null);
        }
        //拡張されていた場合イベントの送信.
        void FireAddressListExpandsEvent(uint addr, uint newaddr, uint newSize)
        {
            if (addr == newaddr)
            {//同一
                return;
            }
            if (this.AddressListExpandsEvent == null)
            {//イベントがない
                return;
            }

            InputFormRef.ExpandsEventArgs eventarg = new InputFormRef.ExpandsEventArgs();
            eventarg.OldBaseAddress = addr;
            eventarg.BlockSize = 8;
            eventarg.OldDataCount = 0;
            eventarg.NewBaseAddress = newaddr;
            eventarg.NewDataCount = newSize;
            this.AddressListExpandsEvent(this.Script, eventarg);

            //asm mapキャッシュの更新.
            Program.AsmMapFileAsmCache.ClearCache();
        }

        private MoveToUnuseSpace.ADDR_AND_LENGTH get_data_pos_callback(uint addr)
        {
            uint length = CalcLength(addr);

            //範囲外探索 00 00 00 00 が続く限り検索してみる.
            uint more = MoveToFreeSapceForm.SearchOutOfRange(addr + length);
            //8バイトアライメント
            more = (more / 8) * 8;

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = length + more;
            return aal;
        }

        private void ProcsScriptForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
                return;
            }
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
            this.ProcsScript.Clear();

            uint addr = (uint)this.Address.Value;
            addr = U.toOffset(addr);

            if (sender == null 
                || this.N_ReadCount.Value == 0)
            {
                if (U.isSafetyOffset(addr))
                {
                    uint length = CalcLength(addr);
                    U.ForceUpdate(this.N_ReadCount, length);
                }
                else
                {
                    U.ForceUpdate(this.N_ReadCount, 0);
                }
            }

            //変更通知.
            if (Navigation != null)
            {
                NavigationEventArgs arg = new NavigationEventArgs();
                arg.Address = addr;
                Navigation(this, arg);
            }

            uint bytecount = (uint)this.N_ReadCount.Value;
            uint limit = Math.Min(addr + bytecount, (uint)Program.ROM.Data.Length);

            while (addr < limit)
            {
                EventScript.OneCode code = Program.ProcsScript.DisAseemble(Program.ROM.Data, addr);
                this.ProcsScript.Add(code);
                addr += (uint)code.Script.Size;
            }
           this.Script.DummyAlloc(this.ProcsScript.Count, -1);
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

        public void JumpTo(uint addr)
        {
            addr = U.toOffset(addr);
            U.ForceUpdate(Address, addr);

            N_ReloadListButton_Click(null, null);
        }
        public void JumpTo(uint addr, uint currnt_procs)
        {
            JumpTo(addr);
            U.SelectedIndexSafety(this.Script, 0);

            if (currnt_procs != U.NOT_FOUND)
            {
                uint a = addr;
                for (int i = 0; i < this.ProcsScript.Count; i++)
                {
                    a += (uint)this.ProcsScript[i].Script.Size;

                    if (a > currnt_procs)
                    {
                        this.Script.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void ProcsScriptInnerControl_Load(object sender, EventArgs e)
        {
            InputFormRef.markupJumpLabel(AddressLabel);
            HideFloatingControlpanel();
            ClearUndoBuffer();

            InputFormRef.MakeEditListboxContextMenuN(this.Script, this.Script_KeyDown, this.Script_KeyDown , useTemplate: false);
        }
        public EventHandler AddressListExpandsEvent;

        ToolTipEx ToolTip;
        public void Init(ToolTipEx toolTip, KeyEventHandler parentFormKeydownfunc)
        {
            this.ToolTip = toolTip;
            for (int i = 0; i < 2; i++)
            {
                ScriptEditSetTables[i].ParamValue.SetToolTipEx(this.ToolTip);
            }
            InputFormRef.MakeEditListboxContextMenuN(this.Script, this.Script_KeyDown, parentFormKeydownfunc, useTemplate: false);
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
            if (this.Script.Items.Count == 0)
            {//件数が0件ならアドレス
                this.Address.Focus();
            }
            else
            {//件数があるならリスト
                this.Script.Focus();
            }
        }
        private string EventToTextAll()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.ProcsScript.Count; i++)
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
            EventScript.NotifyChangePointer(this.ProcsScript, oldaddr, newaddr);
        }

    }
}
