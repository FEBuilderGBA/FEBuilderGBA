using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class AOERANGEForm : Form
    {
        public AOERANGEForm()
        {
            InitializeComponent();
        }

        private void AOERANGEForm_Load(object sender, EventArgs e)
        {
            this.B0.Focus();
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
        }

        public static void MakeDataLength(List<Address> list, uint pointer, string strname)
        {
            if (! U.isSafetyOffset(pointer))
            {
                return;
            }
            uint addr = Program.ROM.p32(pointer);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            uint w = Program.ROM.u8(addr + 0);
            uint h = Program.ROM.u8(addr + 1);
            uint length = 4 + (w * h);

            FEBuilderGBA.Address.AddPointer(list, pointer, length, strname, Address.DataTypeEnum.BIN);
        }

        NumericUpDown ParentNumnic = null;
        int BoxWidth;
        int BoxHeiht;
        int CenterX;
        int CenterY;
        byte[,] MapData = new byte[0,0];

        private void B0_ValueChanged(object sender, EventArgs e)
        {
            if (BoxWidth != B0.Value || BoxHeiht != B1.Value)
            {
                ReloadMapData();
                WriteButtonYellow(sender, e);
            }
        }
        private void B2_ValueChanged(object sender, EventArgs e)
        {
            if (CenterX != B2.Value || CenterY != B3.Value)
            {
                UpdateCenterMark();
                WriteButtonYellow(sender, e);
            }
        }

        void ReloadMapData()
        {
            UpdateControlToMapData();

            MapController.Controls.Clear();

            BoxWidth = (int)this.B0.Value;
            BoxHeiht = (int)this.B1.Value;

            int n = 0;
            for (int y = 0; y < BoxHeiht; y++)
            {
                for (int x = 0; x < BoxWidth; x++ , n++)
                {
                    NumericUpDown nud = new NumericUpDown();
                    MapController.Controls.Add(nud);
                    nud.Size = new System.Drawing.Size(57, 25);
                    nud.Minimum = 0;
                    nud.Maximum = 255;
                    nud.Location = new Point(x * 65, y * 30);
                    nud.Name = "B" + (n + 4);
                    nud.Hexadecimal = true;
                    nud.BackColor = OptionForm.Color_Input_BackColor();
                    nud.ForeColor = OptionForm.Color_Input_ForeColor();
                    nud.ValueChanged += WriteButtonYellow;
                    nud.Value = GetMapData(x, y);
                }
            }
            UpdateCenterMark();
        }

        void UpdateCenterMark()
        {
            for (int oldindex = 0; oldindex < MapController.Controls.Count; oldindex++)
            {
                Object o = MapController.Controls[oldindex];
                if (o is NumericUpDown)
                {
                    ((NumericUpDown)o).BackColor = OptionForm.Color_Input_BackColor();
                    ((NumericUpDown)o).ForeColor = OptionForm.Color_Input_ForeColor();
                }
            }

            CenterX = (int)this.B2.Value;
            CenterY = (int)this.B3.Value;

            int newindex = (CenterX + (CenterY * BoxWidth));
            if (newindex < MapController.Controls.Count)
            {
                Object o = MapController.Controls[newindex];
                if (o is NumericUpDown)
                {
                    ((NumericUpDown)o).BackColor = OptionForm.Color_Keyword_BackColor();
                    ((NumericUpDown)o).ForeColor = OptionForm.Color_Keyword_ForeColor();
                }
            }
        }

        byte GetMapData(int x, int y)
        {
            if (y >= this.MapData.GetLength(0))
            {
                return 0;
            }
            if (x >= this.MapData.GetLength(1))
            {
                return 0;
            }
            return this.MapData[y, x];
        }
        void UpdateControlToMapData()
        {
            if (MapController.Controls.Count <= 0)
            {
                return;
            }

            int n = 0;
            for (int y = 0; y < BoxHeiht; y++)
            {
                if (y >= this.MapData.GetLength(0))
                {
                    break;
                }

                for (int x = 0; x < BoxWidth; x++, n++)
                {
                    if (x >= this.MapData.GetLength(1))
                    {
                        continue;
                    }

                    Object o = MapController.Controls[n];
                    if (o is NumericUpDown)
                    {
                        this.MapData[y, x] = (byte)((NumericUpDown)o).Value;
                    }
                }
            }

        }

        private void ReloadListButton_Click(object sender, EventArgs e)
        {
            JumpTo(this.ParentNumnic, (uint)this.ReadStartAddress.Value);
            UpdateCenterMark();
            ReloadMapData();
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
        }
        private void WriteButtonYellow(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
        }

        public void JumpTo(NumericUpDown parentNumnic ,uint addr)
        {
            this.ParentNumnic = parentNumnic;
            this.BoxWidth = 0;
            this.BoxHeiht = 0;
            this.CenterX = 0;
            this.CenterY = 0;
            this.MapController.Controls.Clear();

            addr = U.toOffset(addr);
            if (! U.isSafetyOffset(addr + 2))
            {
                return;
            }
            this.ReadStartAddress.Value = addr;

            uint w = Program.ROM.u8(addr + 0);
            uint h = Program.ROM.u8(addr + 1);
            uint length = (w * h);
            if (!U.isSafetyOffset(addr + 4 + length - 1))
            {
                return;
            }

            this.MapData = new byte[h, w];

            int n = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++, n++)
                {
                    this.MapData[y, x] = (byte)Program.ROM.u8(addr + 4 + (uint)n);
                }
            }

            this.B0.Value = Program.ROM.u8(addr + 0);   //width
            this.B1.Value = Program.ROM.u8(addr + 1);   //height
            this.B2.Value = Program.ROM.u8(addr + 2);   //center x
            this.B3.Value = Program.ROM.u8(addr + 3);   //center y

            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
        }

        private MoveToUnuseSpace.ADDR_AND_LENGTH get_data_pos_callback(uint addr)
        {
            uint length = 0;
            if (U.isSafetyOffset(addr + 2))
            {
                uint w = Program.ROM.u8(addr + 0);
                uint h = Program.ROM.u8(addr + 1);
                length = 4 + (w * h);
            }

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = length;
            return aal;
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            //書き込むデータを作る
            uint w = (uint)this.B0.Value;
            uint h = (uint)this.B1.Value;
            uint centerX = (uint)this.B2.Value;
            uint centerY = (uint)this.B3.Value;

            byte[] bin = new byte[4 + w * h];
            U.write_u8(bin, 0, w);
            U.write_u8(bin, 1, h);
            U.write_u8(bin, 2, centerX);
            U.write_u8(bin, 3, centerY);

            for(int n = 0 ; n < MapController.Controls.Count ; n++)
            {
                Object o = MapController.Controls[n];
                if (o is NumericUpDown)
                {
                    bin[4 + n] = (byte) ((NumericUpDown)o).Value;
                }
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this.Text);

            uint addr = (uint)this.ReadStartAddress.Value;
            uint newaddr = InputFormRef.WriteBinaryData(this
                , addr
                , bin
                , get_data_pos_callback
                , undodata
            );

            Program.Undo.Push(undodata);

            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, newaddr);

            if (addr != newaddr)
            {
                if (this.ParentNumnic != null && this.ParentNumnic.IsDisposed == false)
                {
                    U.ForceUpdate(this.ParentNumnic, (int)U.toPointer(newaddr));
                }
            }

            this.ReloadListButton_Click(sender, e);
        }
    }
}
