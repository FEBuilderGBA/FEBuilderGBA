using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolThreeMargeForm : Form
    {
        public ToolThreeMargeForm()
        {
            InitializeComponent();
            this.AddressList.ItemHeight = (int)(this.AddressList.Font.Height * 5);
            this.AddressList.OwnerDraw(Draw, DrawMode.OwnerDrawFixed, false);
        }

        public void MakeListboxContextMenuN(ListBox listbox, KeyEventHandler keydownfunc)
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;
            menuItem = new MenuItem(R._("{0}のデータで上書き(&1)",this.ALabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.D1));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("{0}のデータで上書き(&2)", this.BLabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.D2));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("{0}のデータで上書き(&3)", this.CLabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.D3));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("マークの切り替え(&M)", this.CLabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.M));
            contextMenu.MenuItems.Add(menuItem);
            
            menuItem = new MenuItem(R._("マーク一覧を表示(&L)", this.CLabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.L));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("以下100件のデータを、{0}のデータで上書き(Ctrl + 1)", this.ALabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.D1));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("以下100件のデータを、{0}のデータで上書き(Ctrl + 2)", this.BLabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.D2));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("以下100件のデータを、{0}のデータで上書き換えマークを消す(Ctrl + 0)", this.CLabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.D0));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("{0}のデータで上書きし書き換えマークを消す(&0)", this.CLabel));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.D0));
            contextMenu.MenuItems.Add(menuItem);

            listbox.ContextMenu = contextMenu;
        }

        string ALabel;
        string BLabel;
        string CLabel;
        const int HeaderLength = 8;
        const int LabelLength = 64 + HeaderLength;
        private Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.ChangeDataList.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            ChangeDataSt code = this.ChangeDataList[index];

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            SolidBrush selectedBrush = new SolidBrush(OptionForm.Color_Keyword_ForeColor());
            Font normalFont = lb.Font;

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            if (code.mark)
            {//マークの背景になる部分を描画
                Brush markColorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());

                Rectangle rc = new Rectangle(listbounds.X, listbounds.Y, listbounds.Width, lineHeight);
                g.FillRectangle(markColorBrush, rc);

                markColorBrush.Dispose();
            }


            uint addr = code.addr;
            uint length = Math.Min(code.length , 32);
            string whatis = WhatIs(addr);
            if (whatis == "")
            {
                whatis = WhatIs(addr + length);
            }

            bounds.X = HeaderLength;
            string writeof = "";
            if (code.method == MargeMethod.A)
            {
                writeof = R._("<<対処法:{0}>>", this.ALabel);
            }
            else if (code.method == MargeMethod.B)
            {
                writeof = R._("<<対処法:{0}>>", this.BLabel);
            }
            else if (code.method == MargeMethod.C)
            {
                writeof = R._("<<対処法:{0}>>", this.CLabel);
            }

            text = R._("{0} 長さ:{1} {2} {3}", U.To0xHexString(addr), code.length,writeof, whatis);
            int labelEnd = U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            bounds.Y += lineHeight;

            text = U.HexDumpLiner(this.AData, addr, length);
            if (code.method == MargeMethod.A)
            {
                bounds.X = HeaderLength;
                U.DrawText(this.ALabel, g, normalFont, selectedBrush, isWithDraw, bounds);
                bounds.X = LabelLength;
                U.DrawText(text, g, normalFont, selectedBrush, isWithDraw, bounds);
            }
            else
            {
                bounds.X = HeaderLength;
                U.DrawText(this.ALabel, g, normalFont, brush, isWithDraw, bounds);
                bounds.X = LabelLength;
                U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            bounds.Y += lineHeight;

            text = U.HexDumpLiner(this.BData, addr, length);
            if (code.method == MargeMethod.B)
            {
                bounds.X = HeaderLength;
                U.DrawText(this.BLabel, g, normalFont, selectedBrush, isWithDraw, bounds);
                bounds.X = LabelLength;
                U.DrawText(text, g, normalFont, selectedBrush, isWithDraw, bounds);
            }
            else
            {
                bounds.X = HeaderLength;
                U.DrawText(this.BLabel, g, normalFont, brush, isWithDraw, bounds);
                bounds.X = LabelLength;
                U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            bounds.Y += lineHeight;

            text = U.HexDumpLiner(this.CData, addr, length);
            if (code.method == MargeMethod.C)
            {
                bounds.X = HeaderLength;
                U.DrawText(this.CLabel, g, normalFont, selectedBrush, isWithDraw, bounds);
                bounds.X = LabelLength;
                U.DrawText(text, g, normalFont, selectedBrush, isWithDraw, bounds);
            }
            else
            {
                bounds.X = HeaderLength;
                U.DrawText(this.CLabel, g, normalFont, brush, isWithDraw, bounds);
                bounds.X = LabelLength;
                U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            bounds.Y += lineHeight;
            bounds.Y += lineHeight;

            if (code.method != MargeMethod.NONE)
            {
                Rectangle rc = new Rectangle(listbounds.X, listbounds.Y, HeaderLength, bounds.Y - listbounds.Y);
                g.FillRectangle(selectedBrush, rc);
            }
            if (code.mark)
            {//マークの縦線になる部分を描画
                Brush markColorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());

                Rectangle rc = new Rectangle(listbounds.Width - HeaderLength, listbounds.Y, listbounds.Width, bounds.Y - listbounds.Y);
                g.FillRectangle(markColorBrush, rc);

                markColorBrush.Dispose();
            }

            brush.Dispose();
            selectedBrush.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        AsmMapFile AsmMapFile;
        List<Address> StructList;

        void MakeWhatIs()
        {
            this.AsmMapFile = new AsmMapFile();
            this.StructList = U.MakeAllStructPointersList(false);
        }
        string WhatIs(uint addr)
        {
            uint pointer = U.toPointer(addr);
            Dictionary<uint, AsmMapFile.AsmMapSt> asmMapList = this.AsmMapFile.GetAsmMap();
            foreach (var pair in asmMapList)
            {
                if (pointer >= pair.Key && pointer < pair.Key + pair.Value.Length)
                {
                    return pair.Value.Name + " " + pair.Value.ResultAndArgs;
                }
            }

            for (int i = 0; i < this.StructList.Count; i++)
            {
                if (addr >= this.StructList[i].Addr && addr < this.StructList[i].Addr + this.StructList[i].Length)
                {
                    return this.StructList[i].Info;
                }
                if (addr == this.StructList[i].Pointer)
                {
                    return this.StructList[i].Info;
                }
            }
            return "";
        }

        private void ThreeMargeForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
            U.SelectedIndexSafety(this.AddressList, 0, true);
        }


        public enum MargeMethod
        {
             NONE = 0
            ,A = 1 
            ,B = 2
            ,C = 3
        };

        public class ChangeDataSt
        {
            public uint addr { get; private set; }
            public uint length { get; private set; }
            public MargeMethod method { get; private set; }
            public bool mark { get; private set; }

            public ChangeDataSt(uint addr, uint length, MargeMethod method)
            {
                this.addr = addr;
                this.length = length;
                this.method = method;
                this.mark = false;
            }
            public void ToggleMark()
            {//マークを反転.
                this.mark = !this.mark;
            }
            public void SetMargeMethod(MargeMethod v)
            {
                this.method = v;
            }
        };
        List<ChangeDataSt> ChangeDataList;

        byte[] CData;
        byte[] BData;
        byte[] AData;
        public void InitUPS(InputFormRef.AutoPleaseWait wait, byte[] changeData, byte[] vanillaData)
        {
            this.ALabel = "VANILLA";
            this.BLabel = "UPS    ";
            this.CLabel = "CURRENT";
            this.CData = (byte[])Program.ROM.Data.Clone();
            this.BData = changeData;
            this.AData = vanillaData;

            this.ChangeDataList = new List<ChangeDataSt>();

            uint length = (uint)this.CData.Length;
            if (length < this.BData.Length)
            {
                length = (uint)this.BData.Length;
            }
            if (length < this.AData.Length)
            {
                length = (uint)this.AData.Length;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"automarge");
            Program.ROM.write_resize_data((uint)length);

            uint nextDoEvents = 0;

            for (uint i = 0; i < length;)
            {
                if (i > nextDoEvents)
                {//毎回更新するのは無駄なのである程度の間隔で更新して以降
                    wait.DoEvents(R._("データ確認中 {0}/{1}", i, length));
                    nextDoEvents = i + 0xfff;
                }

                byte c = U.at(this.CData, i);
                byte b = U.at(this.BData, i);
                if (c == b)
                {//変更なし
                    i++;
                    continue;
                }
                byte a = U.at(this.AData, i);
                if (b == a)
                {//変更データが無改造ROMと同じなので、無条件に無視できます
                    i++;
                    continue;
                }
                if (c == a)
                {//現行データが無改造ROMと同じなので、無条件にマージできます
                    uint addr = (uint)i;
                    Program.ROM.write_u8(addr, b, undodata);
                    i++;
                    continue;
                }
                //共に違い、無改造ROMとも違うデータ
                {
                    uint size = find_length_conflict_ups(i, length);
                    this.ChangeDataList.Add(new ChangeDataSt(i, size, MargeMethod.NONE));
                    i += size;
                }
            }

            if (undodata.list.Count > 0)
            {
                Program.Undo.Push(undodata);
            }

            if (! IsConflictData())
            {
                return;
            }
            wait.DoEvents(R._("ヒント用にマップファイルを構築しています"));
            MakeWhatIs();

            wait.DoEvents(R._("相違点をリストにまとめています"));
            this.AddressList.DummyAlloc( this.ChangeDataList.Count, 0);

            MakeListboxContextMenuN(AddressList, AddressList_KeyDown);
            UpdateTitle();
        }
        //衝突したデータの長さ
        uint find_length_conflict_ups(uint start, uint length)
        {
            uint last = start;
            uint miss = 0;
            for (uint i = start + 1; i < length; i++)
            {
                byte a = U.at(this.AData, i); //無改造
                byte b = U.at(this.BData, i); //ups
                if (a == b)
                {
                    miss = 0;
                    last = i;
                    continue;
                }

                byte c = U.at(this.CData, i); //current
                if (c == b || c == a)
                {
                    miss = 0;
                    last = i;
                    continue;
                }

                miss++;
                if (miss >= 10)
                {
                    break;
                }
            }
            return last - start + 1;
        }

        public enum DiffDebugMethod
        {
             Method1
           , Method2
        }

        public void InitDiffDebug(InputFormRef.AutoPleaseWait wait, byte[] okData, byte[] ngData , DiffDebugMethod method)
        {
            this.ALabel = "OK ROM ";
            this.BLabel = "NG ROM ";
            this.CLabel = "CURRENT";
            this.CData = (byte[])Program.ROM.Data.Clone();
            this.BData = ngData;
            this.AData = okData;

            this.ChangeDataList = new List<ChangeDataSt>();

            uint length = (uint)this.CData.Length;
            if (length < this.BData.Length)
            {
                length = (uint)this.BData.Length;
            }
            if (length < this.AData.Length)
            {
                length = (uint)this.AData.Length;
            }

            uint nextDoEvents = 0;

            for (uint i = 0; i < length; )
            {
                if (i > nextDoEvents)
                {//毎回更新するのは無駄なのである程度の間隔で更新して以降
                    wait.DoEvents(R._("データ確認中 {0}/{1}", i, length));
                    nextDoEvents = i + 0xfff;
                }

                byte a = U.at(this.AData, i);
                byte b = U.at(this.BData, i);
                if (a == b)
                {//NG ROMとOK ROMが同一なので無視
                    i++;
                    continue;
                }
                byte c = U.at(this.CData, i);
                if (a == c)
                {//現状がOK ROMと同じなので無視
                    i++;
                    continue;
                }

                if (method == DiffDebugMethod.Method1)
                {
                    if (b != c)
                    {//現状がNG ROMと同じではないので無視する.
                        i++;
                        continue;
                    }
                }

                //変更点
                {
                    uint size = find_length_conflict_diffdebug(i, length, method);
                    this.ChangeDataList.Add(new ChangeDataSt(i, size, MargeMethod.NONE));
                    i += size;
                }
            }

            wait.DoEvents(R._("ヒント用にマップファイルを構築しています"));
            MakeWhatIs();

            wait.DoEvents(R._("相違点をリストにまとめています"));
            this.AddressList.DummyAlloc( this.ChangeDataList.Count, 0);
            MakeListboxContextMenuN(AddressList, AddressList_KeyDown);
            UpdateTitle();
        }
        //衝突したデータの長さ
        uint find_length_conflict_diffdebug(uint start, uint length, DiffDebugMethod method)
        {
            uint last = start;
            uint miss = 0;
            for (uint i = start + 1; i < length; i++)
            {
                byte a = U.at(this.AData, i); //ok
                byte b = U.at(this.BData, i); //ng
                if (a != b)
                {
                    miss = 0;
                    last = i;
                    continue;
                }
                byte c = U.at(this.CData, i); //current
                if (a != c)
                {
                    miss = 0;
                    last = i;
                    continue;
                }

                if (method == DiffDebugMethod.Method1)
                {
                    if (b != c)
                    {//現状がNG ROMと同じではないので無視する.
                        miss = 0;
                        last = i;
                        continue;
                    }
                }

                miss++;
                if (miss >= 4)
                {
                    break;
                }
            }
            return last - start + 1;
        }


        public bool IsConflictData()
        {
            return this.ChangeDataList.Count >= 1;
        }


        //まだ未処理分が残っているか？
        int CalcLeftCount()
        {
            int count = 0;
            int length = this.ChangeDataList.Count;
            for (int i = 0; i < length; i++)
            {
                if (this.ChangeDataList[i].method == MargeMethod.NONE)
                {
                    count++;
                }
            }
            return count;
        }

        private void AddressList_KeyDown(object sender, KeyEventArgs e)
        {
            if (AddressList_KeyDownWithCtrl(sender, e))
            {
                return;
            }

            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.ChangeDataList.Count)
            {
                return;
            }
            ChangeDataSt code = this.ChangeDataList[index];

            byte[] bin;
            if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                bin = U.subrange(this.AData, code.addr, code.addr + code.length);
                code.SetMargeMethod(MargeMethod.A);
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                bin = U.subrange(this.BData, code.addr, code.addr + code.length);
                code.SetMargeMethod(MargeMethod.B);
            }
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
            {
                bin = U.subrange(this.CData, code.addr, code.addr + code.length);
                code.SetMargeMethod(MargeMethod.C);
            }
            else if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
            {
                bin = U.subrange(this.CData, code.addr, code.addr + code.length);
                code.SetMargeMethod(MargeMethod.NONE);
            }
            else if (e.KeyCode == Keys.M)
            {
                code.ToggleMark();
                this.AddressList.InvalidateLine();
                return;
            }
            else if (e.KeyCode == Keys.L)
            {
                MarkListButton_Click(sender, e);
                return;
            }
            else
            {
                return;
            }


            Program.Undo.Push("marge " + U.To0xHexString(code.addr), code.addr, (uint)bin.Length);
            if (Program.ROM.Data.Length < code.addr + bin.Length)
            {
                Program.ROM.write_resize_data(code.addr + (uint)bin.Length);
            }
            Program.ROM.write_range(code.addr, bin);
            InputFormRef.ShowWriteNotifyAnimation(this, code.addr);

            if (! e.Shift)
            {//面倒なので操作を逆にする. Shiftが押されていなければ、次へ移動する.
                if (this.AddressList.SelectedIndex + 1 < this.AddressList.Items.Count)
                {
                    this.AddressList.SelectedIndex++;
                }
                else
                {
                    this.AddressList.SelectedIndex = this.AddressList.Items.Count - 1;
                    this.AddressList.InvalidateLine();
                }
            }
            else
            {
                this.AddressList.InvalidateLine();
            }
            UpdateTitle();
        }

        //Ctrl押しながら100件一気にマージ
        private bool AddressList_KeyDownWithCtrl(object sender, KeyEventArgs e)
        {
            if ( ! e.Control)
            {
                return false;
            }

            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.ChangeDataList.Count)
            {
                return false;
            }

            int limit = Math.Min(index + 100, this.ChangeDataList.Count);
            uint startaddr = this.ChangeDataList[index].addr;
            Undo.UndoData undodata = Program.Undo.NewUndoData("marge " + U.To0xHexString(startaddr) + " With Control");

            for (; index < limit; index++)
            {
                ChangeDataSt code = this.ChangeDataList[index];

                byte[] bin;
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    bin = U.subrange(this.AData, code.addr, code.addr + code.length);
                    code.SetMargeMethod(MargeMethod.A);
                }
                else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    bin = U.subrange(this.BData, code.addr, code.addr + code.length);
                    code.SetMargeMethod(MargeMethod.B);
                }
                else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    bin = U.subrange(this.CData, code.addr, code.addr + code.length);
                    code.SetMargeMethod(MargeMethod.C);
                }
                else if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                {
                    bin = U.subrange(this.CData, code.addr, code.addr + code.length);
                    code.SetMargeMethod(MargeMethod.NONE);
                }
                else
                {
                    return false;
                }

                if (Program.ROM.Data.Length < code.addr + bin.Length)
                {
                    Program.ROM.write_resize_data(code.addr + (uint)bin.Length);
                }
                Program.ROM.write_range(code.addr, bin, undodata);
            }


            InputFormRef.ShowWriteNotifyAnimation(this, startaddr);
            Program.Undo.Push(undodata);

            if (!e.Shift)
            {//面倒なので操作を逆にする. Shiftが押されていなければ、次へ移動する.
                if (index < this.AddressList.Items.Count)
                {
                    this.AddressList.SelectedIndex = index;
                }
                else
                {
                    this.AddressList.SelectedIndex = this.AddressList.Items.Count - 1;
                }
            }
            this.AddressList.Invalidate();
            UpdateTitle();
            return true;
        }

        private void AddressList_DoubleClick(object sender, EventArgs e)
        {
            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.ChangeDataList.Count)
            {
                return;
            }
            Rectangle rc = this.AddressList.GetItemRectangle(index);
            this.AddressList.ContextMenu.Show(this.AddressList, new Point(rc.X , rc.Y));
        }

        void RunAllCancel()
        {
            if (this.CData.Length <= 0)
            {
                return;
            }

            //すべて rollback.
            Program.Undo.Push("all rollback", 0, (uint)this.CData.Length);
            Program.ROM.write_resize_data((uint)this.CData.Length);
            Program.ROM.write_range(0, this.CData);

            //すべて取り消す.
            for (int i = 0; i < ChangeDataList.Count; i++)
            {
                ChangeDataList[i].SetMargeMethod(MargeMethod.NONE);
            }
            this.AddressList.Invalidate();
            UpdateTitle();
        }

        private void AllCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = R.ShowNoYes("変更点をすべてキャンセルして、Diffツール開始時のROMまで巻き戻してもよろしいですか？");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            RunAllCancel();
        }

        void UpdateTitle()
        {
            this.Text = R._("マージ 相違点:{0}",CalcLeftCount());
        }

        private void SetMarkButton_Click(object sender, EventArgs e)
        {
            AddressList_KeyDown(sender, new KeyEventArgs(Keys.M));
        }

        private void MarkListButton_Click(object sender, EventArgs e)
        {
            HexEditorMark f = (HexEditorMark)InputFormRef.JumpFormLow<HexEditorMark>();

            f.Init(this.ChangeDataList);

            DialogResult dr = f.ShowDialog();
            this.AddressList.Focus();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            uint addr = f.SelectAddr();
            for (int i = 0; i < this.ChangeDataList.Count; i++)
            {
                if (ChangeDataList[i].addr == addr)
                {
                    U.SelectedIndexSafety(AddressList,i);
                    break;
                }
            }
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.ChangeDataList.Count)
            {
                return;
            }
            ChangeDataSt code = this.ChangeDataList[index];
            Address.Text = U.ToHexString(code.addr);
        }

        bool isAllMethodNone()
        {
            for (int i = 0; i < this.ChangeDataList.Count; i++)
            {
                if (this.ChangeDataList[i].method != MargeMethod.NONE)
                {
                    return false;
                }
            }
            return true;
        }

        bool isAllMethodNotNone()
        {
            for (int i = 0; i < this.ChangeDataList.Count; i++)
            {
                if (this.ChangeDataList[i].method == MargeMethod.NONE)
                {
                    return false;
                }
            }
            return true;
        }

        private void ToolThreeMargeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isAllMethodNone() || isAllMethodNotNone() )
            {
                return;
            }
            ToolThreeMargeCloseAlertForm f = (ToolThreeMargeCloseAlertForm)InputFormRef.JumpFormLow<ToolThreeMargeCloseAlertForm>();
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            if (f.DialogResult == System.Windows.Forms.DialogResult.No)
            {
                RunAllCancel();
                return;
            }

            //終了をキャンセルする. (終了しない)
            e.Cancel = true;
            return;
        }
    }
}
