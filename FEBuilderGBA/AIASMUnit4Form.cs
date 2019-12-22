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
    public partial class AIASMUnit4Form : Form
    {
        public AIASMUnit4Form()
        {
            InitializeComponent();
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, this.B0, this.L_0_UNIT, 0, "UNIT", new string[] { "" });
            InputFormRef.makeLinkEventHandler("", controls, this.B1, this.L_1_UNIT, 1, "UNIT", new string[] { "" });
            InputFormRef.makeLinkEventHandler("", controls, this.B2, this.L_2_UNIT, 2, "UNIT", new string[] { "" });
            InputFormRef.makeLinkEventHandler("", controls, this.B3, this.L_3_UNIT, 3, "UNIT", new string[] { "" });
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
            JumpToAddrLow(addr);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }
        void JumpToAddrLow(uint addr)
        {
            this.ReadStartAddress.Value = addr;
            this.B0.Value = Program.ROM.u8(addr + 0);
            this.B1.Value = Program.ROM.u8(addr + 1);
            this.B2.Value = Program.ROM.u8(addr + 2);
            this.B3.Value = Program.ROM.u8(addr + 3);
        }
        public uint GetBaseAddress()
        {
            return (uint)this.ReadStartAddress.Value;
        }

        uint NewAlloc()
        {
            byte[] alloc = new byte[4];

            Undo.UndoData undodata = Program.Undo.NewUndoData("NewAlloc");
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

            Program.ROM.write_u8(addr + 0, (uint)this.B0.Value, undodata); //
            Program.ROM.write_u8(addr + 1, (uint)this.B1.Value, undodata); //
            Program.ROM.write_u8(addr + 2, (uint)this.B2.Value, undodata); //
            Program.ROM.write_u8(addr + 3, (uint)this.B3.Value, undodata); //

            Program.Undo.Push(undodata);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        //全データの取得
        public static void RecycleOldData(ref List<Address> recycle, uint script_pointer)
        {
            Address.AddPointer(recycle,script_pointer,4,"AI Unit4",Address.DataTypeEnum.BIN);
        }

        private void AICoordinateForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.RegistNotifyNumlicUpdate(this.AllWriteButton, controls);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }

        public static string GetUnit4Preview(uint v)
        {
            uint addr = U.toOffset(v);
            if (!U.isSafetyOffset(addr + 1))
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            for (uint i = 0; i < 4; i++)
            {
                uint uid = Program.ROM.u8(addr + i);
                if (uid == 0)
                {
                    continue;
                }
                string str = U.ToHexString(uid) + ": " + UnitForm.GetUnitName(uid) + " ";
                sb.Append(str);
            }
            return sb.ToString();
        }
    }
}
