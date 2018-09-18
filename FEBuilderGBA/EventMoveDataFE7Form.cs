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
    public partial class EventMoveDataFE7Form : Form
    {
        public EventMoveDataFE7Form()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , new List<String>()
                , 0
                , 1
                , (uint addr) =>
                {
                    uint type = Program.ROM.u8(addr);
                    if (IsAppnedData(type))
                    {
                        return addr + 2;
                    }
                    return addr + 1;
                }
                , (int i, uint addr) =>
                {//無効なデータまで検索
                    uint type = Program.ROM.u8(addr);
                    return (IsEnableData(type));
                }
                , (int i, uint addr) =>
                {
                    return new U.AddrResult(addr, (addr).ToString("X08"));
                }
            );
                
        }

        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        public void JumpToAddr(uint addr)
        {
            this.InputFormRef.ReInit(addr);
        }

        //全データの取得
        public static void RecycleOldData(ref List<Address> recycle, uint script_pointer)
        {
            uint script_addr = Program.ROM.u32(script_pointer);
            if (!U.isPointer(script_addr))
            {
                return ;
            }
            script_addr = U.toOffset(script_addr);
            if (!U.isSafetyOffset(script_addr))
            {
                return ;
            }

            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInit(script_addr);
            FEBuilderGBA.Address.AddAddress(recycle
                , InputFormRef
                , "FE7MoveData"
                , new uint[] { });
        }

        public void MakeAddressListExpandsCallback(EventHandler eventHandler)
        {
            this.InputFormRef.MakeAddressListExpandsCallback(eventHandler);
        }

        private void EventMoveDataFE7Form_Load(object sender, EventArgs e)
        {

        }

        static bool IsEnableData(uint data)
        {
            return data <= 3 || data == 0xA || data == 9 || data == 0xC;
        }
        static bool IsAppnedData(uint data)
        {
             return data == 9 || data == 0xC;
        }

        private void B0_ValueChanged(object sender, EventArgs e)
        {
            if (IsAppnedData((uint)B0.Value))
            {
                uint addr = (uint)this.Address.Value;
                if (U.isSafetyOffset(addr + 1))
                {
                    X_B1.Value = Program.ROM.u8(addr+1);
                }
                else
                {
                    X_B1.Value = 0;
                }
                X_TIME.Show();
            }
            else
            {
                X_TIME.Hide();
            }
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (IsAppnedData((uint)B0.Value))
            {
                Undo.UndoData undodata =  Program.Undo.NewUndoData(this,"EventMoveDataFE7Append");

                uint addr = (uint)this.Address.Value;
                if (U.isSafetyOffset(addr + 1))
                {
                    return;
                }
                uint writeVal = (uint)X_B1.Value;
                Program.ROM.write_u8(addr + 1, writeVal, undodata);

                Program.Undo.Push(undodata);
            }
        }

        private void X_B1_ValueChanged(object sender, EventArgs e)
        {
            if (IsAppnedData((uint)B0.Value))
            {
                uint addr = (uint)this.Address.Value;
                if (U.isSafetyOffset(addr + 1))
                {
                    InputFormRef.WriteButtonToYellow(this.WriteButton, true);
                }
            }

        }
    }
}
