using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class CStringForm : Form
    {
        public CStringForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        public void Init(NumericUpDown num)
        {
            uint pointer = (uint)num.Value;
            this.Pointer = pointer;
            if (!U.isSafetyPointer(pointer))
            {
                return;
            }
            string str = TextForm.Direct(pointer);
            this.String.Text = str;

            this.Numeric = num;
        }

        NumericUpDown Numeric;
        uint Pointer;
        string ResultString;
        public uint GetResultPointer()
        {
            return this.Pointer;
        }
        public string GetResultString()
        {
            return this.ResultString;
        }
        private void WriteButton_Click(object sender, EventArgs e)
        {
            this.ResultString = this.String.Text;
            this.Pointer = WriteCString(this.Pointer, this.ResultString);

            if (this.Pointer == U.NOT_FOUND)
            {
                return;
            }
            if (this.Numeric != null)
            {
                U.ForceUpdate(this.Numeric, (int) U.toPointer(this.Pointer));
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public static uint WriteCString(uint pointer,string text)
        {
            byte[] stringbyte = Program.SystemTextEncoder.Encode(text);
            stringbyte = U.ArrayAppend(stringbyte, new byte[] { 0x00 });

            string undoname = text + ":" + U.ToHexString(pointer);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undoname);

            pointer = InputFormRef.WriteBinaryData(null
                ,pointer
                ,stringbyte
                , get_cstring_data_pos_callback
                , undodata
            );

            if (pointer == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            Program.Undo.Push(undodata);
            return pointer;
        }
        static MoveToUnuseSpace.ADDR_AND_LENGTH get_cstring_data_pos_callback(uint addr)
        {
            int length = 0;
            string str = Program.ROM.getString(addr, out length);

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = (uint)U.Padding2((uint)length + 1); //nullを入れる.
            return aal;
        }

        private void CStringForm_Load(object sender, EventArgs e)
        {

        }
    }
}
