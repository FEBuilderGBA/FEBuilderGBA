using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace FEBuilderGBA
{
    public partial class HexBox : UserControl
    {
        public HexBox()
        {
            InitializeComponent();
            if (this.DesignMode)
            {
                return;
            }

            this.DoubleBuffered = true;
            HexStart = 0;
            CursolPosStart = 0;
            CursolPosEnd = 0;
            HighRange = true;
            Data = new byte[0];
            Marks = new List<Mark>();
        }

        private void HexBox_MouseDown(object sender, MouseEventArgs e)
        {
            int y = e.Y - (int)OneHeight;
            int x = e.X - (int)OneWidth * 4;
            y = y / (int)OneHeight;
            x = x / (int)OneWidth;

            uint select = this.HexStart + (uint)x + (uint)(y * 16);

            if (select > this.Data.Length)
            {
                select = (uint)this.Data.Length - 1;
            }

            this.CursolPosStart = (uint)select;
            this.CursolPosEnd = (uint)select;

            SelectChange();
        }
        private void HexBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Left) > 0)
            {
                int y = e.Y - (int)OneHeight;
                int x = e.X - (int)OneWidth * 4;
                y = y / (int)OneHeight;
                x = x / (int)OneWidth;

                uint select = this.HexStart + (uint)x + (uint)(y * 16);

                if (select > this.Data.Length)
                {
                    select = (uint)this.Data.Length - 1;
                }

                this.CursolPosEnd = (uint)select;

                SelectChange();
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        private void HexBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            uint newpos = this.CursolPosStart;
            int maxLines = (int)(this.Height / OneHeight);

            if (e.KeyCode == Keys.Up)
            {
                if (newpos > 0x10)
                {
                    newpos -= 0x10;
                }
                else
                {
                    newpos = 0;
                }
                this.HighRange = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (newpos + 0x10 < this.Data.Length)
                {
                    newpos += 0x10;
                }
                else
                {
                    newpos = (uint)this.Data.Length - 1;
                }
                this.HighRange = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (newpos > 0x0)
                {
                    newpos -= 0x1;
                }
                else
                {
                    newpos = 0;
                }
                this.HighRange = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (newpos + 0x1 < this.Data.Length)
                {
                    newpos += 0x1;
                }
                else
                {
                    newpos = (uint)this.Data.Length - 1;
                }
                this.HighRange = true;
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                if (newpos + maxLines*0x10 < this.Data.Length)
                {
                    newpos += (uint)(maxLines * 0x10);
                }
                else
                {
                    newpos = (uint)this.Data.Length - 1;
                }
                this.HighRange = true;
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                if (newpos > maxLines * 0x10 )
                {
                    newpos -= (uint)(maxLines * 0x10);
                }
                else
                {
                    newpos = 0;
                }
                this.HighRange = true;
            }
            else if (e.KeyCode == Keys.Home)
            {
                newpos = 0;
            }
            else if (e.KeyCode == Keys.End)
            {
                int newP = Math.Min((this.vScrollBar.Maximum - 1)* 16, this.Data.Length - 1);
                if (newP < 0)
                {
                    newP = 0;
                }
                newpos = (uint)newP;
            }
            else if (e.KeyCode == Keys.Back)
            {
                this.HighRange = false;
            }
            else if (e.KeyCode == Keys.Z && e.Control)
            {
                this.RunUndo();
                return;
            }
            else if (e.KeyCode == Keys.Y && e.Control)
            {
                this.RunRedo();
                return;
            }
            else if (e.KeyCode == Keys.C && e.Control)
            {
                this.ToClipBoard();
                return;
            }
            else if (e.KeyCode == Keys.V && e.Control)
            {
                this.FromClipBoard();
                return;
            }
            else if (e.KeyCode == Keys.F3 && LastSearch.Length > 0)
            {
                if (e.Shift)
                {
                    Search(this.LastSearch, this.LastSearchLittleEndian, true, false,true);
                }
                else
                {
                    Search(this.LastSearch, this.LastSearchLittleEndian, false, false, true);
                }
                return;
            }
            else if (e.Shift == false && e.Control == false && e.Alt == false &&
                (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9
                || e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.F))
            {
                byte k;
                if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
                {
                    k = (byte)(e.KeyCode - Keys.NumPad0);
                }
                else if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                {
                    k = (byte)(e.KeyCode - Keys.D0);
                }
                else
                {
                    k = (byte)(e.KeyCode - Keys.A + 10);
                }

                if (newpos >= this.Data.Length)
                {
                    Array.Resize(ref this.Data, (int)newpos+1);
                }

                if (this.HighRange)
                {
                    byte newvalue = (byte)((this.Data[newpos] & 0x0F) | (k << 4));
                    PushUndo(newpos, newvalue, this.Data[newpos]);

                    this.Data[newpos] = newvalue;
                    this.HighRange = false;
                }
                else
                {
                    byte newvalue = (byte)((this.Data[newpos] & 0xF0) | (k));
                    PushUndo(newpos, newvalue, this.Data[newpos]);
                    this.Data[newpos] = newvalue;

                    if (newpos + 0x1 < this.Data.Length)
                    {
                        newpos += 0x1;
                    }
                    else
                    {
                        newpos = (uint)this.Data.Length - 1;
                    }
                    this.HighRange = true;
                }
            }
            else
            {
                return;
            }

            if (e.Shift)
            {
                this.CursolPosStart = newpos;
            }
            else
            {
                this.CursolPosStart = newpos;
                this.CursolPosEnd = newpos;
            }

            if (e.KeyCode == Keys.PageDown)
            {
                this.HexStart = newpos / 16 * 16;
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                this.HexStart = newpos / 16 * 16;
            }
            else if (e.KeyCode == Keys.Home)
            {
                this.HexStart = newpos / 16 * 16;
            }
            else if (e.KeyCode == Keys.End)
            {
                this.HexStart = newpos / 16 * 16;
            }
            else if (maxLines * 0x10 + this.HexStart - 0x20 <= newpos)
            {
                this.HexStart += 0x10;
            }
            else if (this.HexStart > newpos)
            {
                this.HexStart -= 0x10;
            }
            SelectChange();
        }

        void SelectChange()
        {
            int newAddress = (int)(this.HexStart / 16);
            if (newAddress < 0 )
            {
                newAddress = 0;
            }
            else if (newAddress >= this.vScrollBar.Maximum)
            {
                if (this.vScrollBar.Maximum == 0)
                {
                    newAddress = 0;
                }
                else
                {
                    newAddress = (this.vScrollBar.Maximum - 1) ;
                }
            }

            this.vScrollBar.Tag = 1;
            this.vScrollBar.Value = newAddress;
            this.vScrollBar.Tag = 0;
            this.Invalidate();

            //変更を通知する.
            if (this.OnSelectionChange != null)
            {
                this.OnSelectionChange(this, null);
            }
        }

        uint HexStart ;
        uint CursolPosStart ;
        uint CursolPosEnd ;
        bool HighRange ;
        byte[] Data ;
        Brush PanelForeBrush;
        Brush PanelBackBrush;
        Brush ControlForeBrush;
        Brush ControlBackBrush;
        Brush ControlSelectedBrush;
        Brush RefColorBrush;
        Brush MarkColorBrush;
        public EventHandler OnSelectionChange;

        public class UndoData
        {
            public uint addr;     //アドレス
            public byte newvalue; //新値
            public byte orignal;  //元値
        };
        List<UndoData> UndoBuffer;
        int UndoPosstion;

        void ClearUndoBuffer()
        {
            this.UndoBuffer = new List<UndoData>();
            this.UndoPosstion = 0;
        }
        void PushUndo(uint addr, byte newvalue, byte orignal)
        {
            if (this.UndoPosstion < this.UndoBuffer.Count)
            {//常に先頭に追加したいので、リスト中に戻っている場合は、それ以降を消す.
                this.UndoBuffer.RemoveRange(this.UndoPosstion, this.UndoBuffer.Count - this.UndoPosstion);
            }
            UndoData p = new UndoData();
            p.addr = addr;     //アドレス
            p.newvalue = newvalue; //新値
            p.orignal = orignal;  //元値
            this.UndoBuffer.Add(p);
            this.UndoPosstion = this.UndoBuffer.Count;
        }
        void RunUndo()
        {
            if (this.UndoPosstion <= 0)
            {
                return; //無理
            }

            this.UndoPosstion = UndoPosstion - 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion],true);
        }
        void RunRedo()
        {
            if (this.UndoPosstion + 1 >= this.UndoBuffer.Count)
            {
                return; //無理
            }
            this.UndoPosstion = UndoPosstion + 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion],false);
        }
        void RunUndoRollback(UndoData u,bool isUndo)
        {
            if (isUndo)
            {
                this.Data[u.addr] = u.orignal;
            }
            else
            {
                this.Data[u.addr] = u.newvalue;
            }
            this.JumpTo(u.addr, false);
        }

        public uint getDataLength()
        {
            return (uint)this.Data.Length;
        }

        public List<UndoData> getWriteData()
        {
            List<UndoData> ret = new List<UndoData>();
            for (int i = 0; i < UndoPosstion; i++)
            {
                ret.Add(UndoBuffer[i]);
            }
            return ret;
        }

        public uint GetCursolPosStart()
        {
            return Math.Min (this.CursolPosStart , this.CursolPosEnd );
        }
        public uint GetCursolPosEnd()
        {
            return Math.Max(this.CursolPosStart, this.CursolPosEnd);
        }
        public void JumpTo(uint addr,bool forceLittleEndian)
        {
            if (this.Data.Length <= addr || forceLittleEndian)
            {
                if ((addr & 0x0F) == 0x08 || (addr & 0x0F) == 0x09)
                {//エンディアン解決する.
                    addr = U.ChangeEndian32(addr);
                    addr = U.toOffset(addr);
                }
            }
            this.CursolPosStart = addr;
            this.CursolPosEnd = addr;
            int maxLines = (int)(this.Height / OneHeight);
            if (CursolPosStart > this.HexStart && CursolPosStart < this.HexStart + maxLines * 16)
            {//画面内にあるので何もしない.
            }
            else
            {
                this.HexStart = (addr / 16) * 16;
            }

            SelectChange();

            this.Focus();
        }

        public void JumpTo(string addrString, bool forceLittleEndian)
        {
            uint addr;

            //空白を詰める.
            addrString = addrString.Replace(" ","");
            addrString = addrString.Replace("\t","");

            if (addrString.IndexOf("0x") == 0)
            {
                addr = U.atoi0x(addrString);
            }
            else
            {
                addr = U.atoh(addrString);
            }

            addr = U.toOffset(addr);
            JumpTo(addr, forceLittleEndian);
            this.Focus();
        }

        string LastSearch = "";
        bool LastSearchLittleEndian = false;
        public uint Search(string addrString
            ,bool isLittleEndian
            ,bool isRev
            ,bool isAlign4
            ,bool isNext)
        {
            if (addrString.Length <= 0)
            {
                return U.NOT_FOUND;
            }

            this.LastSearch = addrString;
            this.LastSearchLittleEndian = isLittleEndian;

            //空白を詰める.
            addrString = addrString.Replace(" ", "");
            addrString = addrString.Replace("\t", "");

            if (addrString.Length % 2 == 1)
            {//2文字で、1バイトなので、端数の時は、0を入れます.
                addrString = "0" + addrString;
            }
            int bitlength = addrString.Length / 2;

            addrString = addrString.ToUpper();
            byte[] need = new byte[bitlength];
            bool[] mask = new bool[bitlength];
            bool useMask = false;
            for (int i = 0; i < bitlength; i++)
            {
                string target = U.substr(addrString,i * 2, 2);
                need[i] = (byte)U.atoh( target );
                if (target.IndexOf("X") >= 0)
                {
                    useMask = true;
                    mask[i] = true;
                }
            }

            if (isLittleEndian)
            {
                byte[] littleneed = new byte[bitlength];
                bool[] littlemask = new bool[bitlength];
                int n = 0;
                while (n < bitlength)
                {
                    if (bitlength - n >= 4)
                    {
                        littleneed[n + 0] = need[n + 3];
                        littleneed[n + 1] = need[n + 2];
                        littleneed[n + 2] = need[n + 1];
                        littleneed[n + 3] = need[n + 0];
                        n += 4;
                    }
                    else if (bitlength - n >= 3)
                    {
                        littleneed[n + 0] = need[n + 2];
                        littleneed[n + 1] = need[n + 1];
                        littleneed[n + 2] = need[n + 0];
                        n += 3;
                    }
                    else if (bitlength - n >= 2)
                    {
                        littleneed[n + 0] = need[n + 1];
                        littleneed[n + 1] = need[n + 0];
                        n += 2;
                    }
                    else
                    {
                        littleneed[n + 0] = need[n + 0];
                        n += 1;
                    }
                }
                need = littleneed;
                mask = littlemask;
            }

            uint blockSize = 1;
            if (isAlign4)
            {
                blockSize = 4;
            }

            uint searchStart = this.CursolPosStart;
            uint found ;
            if (isRev)
            {
                if (isNext)
                {
                    searchStart--;
                }
                else if (searchStart + bitlength < this.Data.Length)
                {
                    searchStart += (uint)bitlength;
                }
                else
                {
                    searchStart = (uint)this.Data.Length - 1;
                }

                if (useMask)
                {
                    found = GrepPatternMatchRev(this.Data, need, mask, searchStart, blockSize);
                }
                else
                {
                    found = GrepRev(this.Data, need, searchStart, blockSize);
                }
            }
            else
            {
                if (isNext)
                {
                    searchStart ++;
                }
                else if (searchStart > bitlength)
                {
                    searchStart -= (uint)bitlength;
                }
                else
                {
                    searchStart = 0;
                }

                if (useMask)
                {
                    found = U.GrepPatternMatch(this.Data, need, mask, searchStart, (uint)this.Data.Length, blockSize);
                }
                else
                {
                    found = U.Grep(this.Data, need, searchStart, (uint)this.Data.Length, blockSize);
                }
            }

            if (found == U.NOT_FOUND)
            {//エラー存在しない.
                System.Media.SystemSounds.Beep.Play();
                return found;
            }

            //発見したところを選択.
            this.CursolPosStart = found;
            this.CursolPosEnd = found + (uint)need.Length - 1;
            
            int maxLines = (int)(this.Height / OneHeight);
            if (CursolPosStart > this.HexStart && CursolPosStart < this.HexStart + maxLines*16)
            {//画面内にあるので何もしない.
            }
            else
            {
                this.HexStart = (found / 16) * 16;
            }

            SelectChange();

            return found;
        }

        static uint GrepRev(byte[] data, byte[] need, uint start, uint blocksize)
        {
            uint i = 0;
            uint lastFound = U.NOT_FOUND;
            while (true)
            {
                uint a = U.Grep(data, need, i, start , blocksize);
                if (a == U.NOT_FOUND)
                {
                    break;
                }
                lastFound = a;
                i = (uint)(a + need.Length);
                if (i > start)
                {
                    break;
                }
            }
            return lastFound;
        }
        static uint GrepPatternMatchRev(byte[] data, byte[] need,bool[] isSkip, uint start, uint blocksize)
        {
            uint i = 0;
            uint lastFound = U.NOT_FOUND;
            while (true)
            {
                uint a = U.GrepPatternMatch(data, need,isSkip, i, start, blocksize);
                if (a == U.NOT_FOUND)
                {
                    break;
                }
                lastFound = a;
                i = (uint)(a + need.Length);
                if (i > start)
                {
                    break;
                }
            }
            return lastFound;
        }

        uint OneWidth = 16;
        uint OneHeight = 20;

        private void HexBox_Paint(object sender, PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            e.Graphics.FillRectangle(PanelBackBrush, 0, 0, this.Width, OneHeight);
            e.Graphics.FillRectangle(PanelBackBrush, 0, 0, OneWidth * (3 + 1), this.Height);
            e.Graphics.FillRectangle(ControlBackBrush, OneWidth * (3 + 1), OneHeight, this.Width, this.Height);

            e.Graphics.DrawString("ADDRESS", this.Font, PanelForeBrush, new PointF(0, 0));
            for (int i = 0; i < 16; i++)
            {
                e.Graphics.DrawString(i.ToString("X02"), this.Font, PanelForeBrush, new PointF(OneWidth * (3 + 1 + i), 0));
            }
            e.Graphics.DrawString("0123456789ABCDEF", this.Font, PanelForeBrush, new PointF(OneWidth * (3 + 1 + 16 + 1), 0));

            int maxLines = (int)(this.Height / OneHeight);
            uint disasm = this.HexStart;
            for (int y = 0; y < maxLines; y++)
            {
                string disasmLine = "";
                int drawPosY = (int)((y + 1) * OneHeight);
                e.Graphics.DrawString((HexStart + y * 16).ToString("X08"), this.Font, ControlForeBrush, new PointF(0, drawPosY));
                for (int i = 0; i < 16; i++)
                {
                    uint pos = (uint)(HexStart + (y * 16) + i);
                    uint drawPosX = (uint)(OneWidth * (3 + 1 + i));
                    //選択色
                    if (CursolPosStart < CursolPosEnd)
                    {
                        if (CursolPosStart <= pos && CursolPosEnd >= pos)
                        {
                            e.Graphics.FillRectangle(ControlSelectedBrush, drawPosX, drawPosY, OneWidth, OneHeight);
                        }
                    }
                    else
                    {
                        if (CursolPosStart >= pos && CursolPosEnd <= pos)
                        {
                            e.Graphics.FillRectangle(ControlSelectedBrush, drawPosX, drawPosY, OneWidth, OneHeight);
                        }
                    }
                    //マーク色
                    for (int n = 0; n < this.Marks.Count; n++)
                    {
                        if (pos == this.Marks[n].address)
                        {
                            e.Graphics.FillRectangle(MarkColorBrush, drawPosX, drawPosY, OneWidth, OneHeight);
                        }
                    }

                    if (pos >= this.Data.Length)
                    {
                        break;
                    }

                    uint b = this.Data[pos];
                    e.Graphics.DrawString(b.ToString("X02"), this.Font, ControlForeBrush, new PointF(drawPosX, drawPosY));
                }
                
                string bb = ConvertASCII( U.getBinaryData(this.Data,(uint)(HexStart + (y * 16)), 16));
                e.Graphics.DrawString(bb, this.Font, ControlForeBrush, new PointF(OneWidth * (3 + 1 + 16 + 1), drawPosY));

                e.Graphics.DrawString(disasmLine, this.Font, ControlForeBrush, new PointF(OneWidth * (3 + 1 + 16 + 1 + (16/2) ), drawPosY));
            }
        }


        string ConvertASCII(byte[] byte16)
        {
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();
            SystemTextEncoder encoder = Program.SystemTextEncoder;
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                if (i >= byte16.Length)
                {
                    break;
                }

                if (priorityCode == PatchUtil.PRIORITY_CODE.UTF8 )
                {
                    sb.Append((char)byte16[i]);
                    continue;
                }
                else if (i < 15 && U.isSJIS1stCode(byte16[i]) && U.isSJIS2ndCode(byte16[i + 1]))
                {
                    if (priorityCode == PatchUtil.PRIORITY_CODE.LAT1)
                    {//SJISと 1バイトUnicodeは範囲が重複するので、どちらかを優先しないといけない.
                        if (byte16[i] >= 0x81 && byte16[i] < 0xFE)
                        {//英語版FEにはUnicodeの1バイトだけ表記があるらしい.
                            sb.Append((char)byte16[i]);
                            sb.Append((char)byte16[i + 1]);
                            i++;
                            continue;
                        }
                    }
                    sb.Append(encoder.Decode(byte16, i, 2));
                    i++;
                    continue;
                }
                if (U.isAscii(byte16[i]))
                {
                    sb.Append( (char)byte16[i] );
                    continue;
                }

                if (byte16[i] >= 0x81 && byte16[i] < 0xFE)
                {//FE独自フォントの可能性.
                    if (priorityCode == PatchUtil.PRIORITY_CODE.LAT1)
                    {
                        sb.Append((char)byte16[i]);
                        continue;
                    }
                }

                //不明なコード
                sb.Append('.');
            }
            return sb.ToString();
        }

        private void HexBox_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            SetData(Program.ROM.Data);

            PanelForeBrush = new SolidBrush(OptionForm.Color_Control_ForeColor());
            PanelBackBrush = new SolidBrush(OptionForm.Color_Control_BackColor());
            ControlForeBrush = new SolidBrush(OptionForm.Color_Input_ForeColor());
            ControlBackBrush = new SolidBrush(OptionForm.Color_Input_BackColor());
            ControlSelectedBrush = new SolidBrush(OptionForm.Color_List_SelectedColor());
            RefColorBrush = new SolidBrush(OptionForm.Color_NotifyWrite_BackColor());
            MarkColorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());

            this.OneWidth = (uint)(this.Font.SizeInPoints * 2);
            this.OneHeight = (uint)(this.Font.Height + 2);
            ClearUndoBuffer();
        }
        public void SetData(byte[] data)
        {
            this.Data = (byte[])data.Clone();
            int maxScrollValue = this.Data.Length / 16;
            if (maxScrollValue <= 0)
            {
                this.VerticalScroll.Visible = false;
                this.vScrollBar.Tag = 1;
            }
            else
            {
                this.VerticalScroll.Minimum = 0;
                this.VerticalScroll.Maximum = maxScrollValue;
                this.VerticalScroll.Value = 0;
                this.VerticalScroll.Visible = true;
                this.vScrollBar.Tag = 1;
                this.vScrollBar.Minimum = 0;
                this.vScrollBar.Maximum = maxScrollValue;
                this.vScrollBar.Value = 0;
            }
        }

        public void ToClipBoard()
        {
            StringBuilder sb = new StringBuilder();

            uint from = GetCursolPosStart();
            uint to = GetCursolPosEnd();
            for (uint i = from; i <= to; i++)
            {
                sb.Append(" ");
                sb.Append(U.ToHexString(this.Data[i]));
            }
            if (sb.Length <= 0)
            {
                return;
            }
            string text = sb.ToString().Substring(1);
            U.SetClipboardText( text );
        }
        public void FromClipBoard()
        {
            uint from = GetCursolPosStart();

            string str = Clipboard.GetText();
            string[] sp = str.Split(' ');
            if (from + sp.Length >= this.Data.Length)
            {//もし新規にサイズを伸ばす場合
                this.Data = U.ResizeArray(this.Data, from + (uint)sp.Length);
            }

            for(int i = 0 ; i < sp.Length ; i++)
            {
                if (!U.isHexString(sp[i]))
                {
                    break;
                }
                uint newpos = (uint)(from + i); 
                byte newvalue = (byte)U.atoh(sp[i]);
                PushUndo(newpos, newvalue, this.Data[newpos]);
                this.Data[newpos] = newvalue;
            }
            this.CursolPosStart = (uint)(from + sp.Length);
            this.CursolPosEnd = this.CursolPosStart;
            SelectChange();
        }

        public class Mark
        {
            public uint address;
        };
        List<Mark> Marks;
        public void SetMark()
        {
            uint addr = this.GetCursolPosStart();

            bool found = false;
            for (int n = 0; n < this.Marks.Count; n++)
            {
                if (this.Marks[n].address == addr)
                {//マークがあれば消す
                    this.Marks.RemoveAt(n);
                    n--;
                    found = true;
                    continue;
                }
            }

            if (!found)
            {
                Mark m = new Mark();
                m.address = addr;
                this.Marks.Add(m);
            }
            SelectChange();
        }
        public List<Mark> GetMarks()
        {
            return this.Marks;
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int isnotJump = (int)this.vScrollBar.Tag;
            if (isnotJump == 0)
            {
                JumpTo((uint)(e.NewValue * 16), false);
            }
        }


    }
}
