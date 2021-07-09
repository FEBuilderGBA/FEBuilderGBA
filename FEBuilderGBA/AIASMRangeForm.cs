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
    public partial class AIASMRangeForm : Form
    {
        public AIASMRangeForm()
        {
            InitializeComponent();
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, this.B0, this.J_0, 0, "MAPXY", new string[] { "1" });
            InputFormRef.makeLinkEventHandler("", controls, this.B2, this.J_2, 0, "MAPXY", new string[] { "3" });

            InputFormRef.RegistNotifyNumlicUpdate(this.AllWriteButton, controls);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }

        public uint AllocIfNeed(NumericUpDown src)
        {
            if (src.Value == 0 || src.Value == U.NOT_FOUND)
            {
                string alllocQMessage = R._("新規に座標データを作成しますか？");
                DialogResult dr = R.ShowYesNo(alllocQMessage);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    src.Value = U.toPointer(NewAlloc());
                }
            }
            return (uint)src.Value;
        }
        public void JumpToAddr(uint addr)
        {
            addr = U.toOffset(addr);
            this.ReadStartAddress.Value = addr;
        }
        public uint GetBaseAddress()
        {
            return (uint)this.ReadStartAddress.Value;
        }

        uint NewAlloc()
        {
            byte[] alloc = new byte[4];

            Undo.UndoData undodata = Program.Undo.NewUndoData("NewAlloc AIRange");
            uint addr = InputFormRef.AppendBinaryData(alloc, undodata);
            if (addr == U.NOT_FOUND)
            {//割り当て失敗
                return U.NOT_FOUND;
            }

            Program.ROM.write_u8(addr + 0, 0, undodata); //
            Program.ROM.write_u8(addr + 1, 0, undodata); //
            Program.ROM.write_u8(addr + 2, 0, undodata); //
            Program.ROM.write_u8(addr + 3, 0, undodata); //

            //新規に追加した分のデータを書き込み.
            Program.Undo.Push(undodata);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
            return addr;
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            uint addr = (uint)this.ReadStartAddress.Value;
            if (!U.isSafetyOffset(addr + 3))
            {
                R.ShowStopError("座標の領域が確保されていません。");
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            Program.ROM.write_u8(addr + 0, (uint)this.B0.Value, undodata); //x1
            Program.ROM.write_u8(addr + 1, (uint)this.B1.Value, undodata); //y1
            Program.ROM.write_u8(addr + 2, (uint)this.B2.Value, undodata); //x2
            Program.ROM.write_u8(addr + 3, (uint)this.B3.Value, undodata); //y2

            Program.Undo.Push(undodata);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        //全データの取得
        public static void RecycleOldData(ref List<Address> recycle, uint script_pointer)
        {
            Address.AddPointer(recycle,script_pointer,4,"AI Range",Address.DataTypeEnum.BIN);
        }

        private void AIRangeForm_Load(object sender, EventArgs e)
        {
            uint mapid = MainSimpleMenuForm.GetCurrentMapID();
            this.MapPictureBox.LoadMap(mapid);

            uint addr = (uint)this.ReadStartAddress.Value;
            if (!U.isSafetyOffset(addr + 3))
            {
                return ;
            }
            this.B0.Value = Program.ROM.u8(addr + 0);
            this.B1.Value = Program.ROM.u8(addr + 1);
            this.B2.Value = Program.ROM.u8(addr + 2);
            this.B3.Value = Program.ROM.u8(addr + 3);

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.RegistNotifyNumlicUpdate(this.AllWriteButton, controls);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);

            this.ActiveControl = this.B0;
        }

        public static string GetRangePreview(uint v)
        {
            uint addr = U.toOffset(v);
            if (!U.isSafetyOffset(addr + 3))
            {
                return "";
            }

            uint x1 = Program.ROM.u8(addr + 0);
            uint y1 = Program.ROM.u8(addr + 1);
            uint x2 = Program.ROM.u8(addr + 2);
            uint y2 = Program.ROM.u8(addr + 3);
            string str = String.Format("({0},{1}) - ({2},{3})", x1,y1,x2,y2);
            return str;
        }
    }
}
