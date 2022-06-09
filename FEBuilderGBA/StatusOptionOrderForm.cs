using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class StatusOptionOrderForm : Form
    {
        public StatusOptionOrderForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawGameOptionAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;
            if (Program.ROM.RomInfo.version == 7)
            {
                this.InputFormRef.PreWriteHandler += PreWriteButtonFE7;
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.status_game_option_order_pointer
                , 1
                , (int i, uint addr) =>
                {
                    uint count = Program.ROM.u8(Program.ROM.RomInfo.status_game_option_order_count_address);
                    return i < count;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(addr);
                    return U.ToHexString(id) + " " + StatusOptionForm.GetNameIndex(id);
                }
                );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            string name = "GameOptionOrder";
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
        
        //書き込みボタンを押したとき
        void PreWriteButtonFE7(object sender, EventArgs arg)
        {
/*
            if (Program.ROM.RomInfo.version == 7)
            {
                uint order_pointer = Program.ROM.RomInfo.status_game_option_order_pointer;
                uint order2_pointer = Program.ROM.RomInfo.status_game_option_order2_pointer;
                if (order2_pointer == 0)
                {
                    return;
                }
                uint order_addr = Program.ROM.p32(order_pointer);
                uint order2_addr = Program.ROM.p32(order2_pointer);
                if (order_addr == order2_addr)
                {
                    return;
                }
                Undo.UndoData undodata = Program.Undo.NewUndoData(this, "StatusOptionOrder2 copy");
                Program.ROM.write_p32(order2_pointer, order_addr, undodata);
                Program.Undo.Push(undodata);
            }
*/
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            //件数を書く必要があるらしい.
            uint write_count_addr = Program.ROM.RomInfo.status_game_option_order_count_address;
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "StatusOptionOrder");
            Program.ROM.write_u8(write_count_addr, (uint)(count));
/*
            if (Program.ROM.RomInfo.version == 7)
            {
                uint order2_pointer = Program.ROM.RomInfo.status_game_option_order2_pointer;
                if (order2_pointer != 0)
                {
                    Program.ROM.write_p32(order2_pointer, addr, undodata);
                }
            }
*/
            Program.Undo.Push(undodata);
        }

        private void StatusOptionOrderForm_Load(object sender, EventArgs e)
        {

        }
    }
}
