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
    public partial class EventBattleDataFE7Form : Form
    {
        public EventBattleDataFE7Form()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    return (a & 0x800000) != 0x800000;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
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
        public uint AllocIfNeed(NumericUpDown src)
        {
            if (src.Value == 0 || src.Value == U.NOT_FOUND)
            {
                string alllocQMessage = R._("新規にデータを作成しますか？");
                DialogResult dr = R.ShowYesNo(alllocQMessage);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    src.Value = U.toPointer(NewAlloc());
                }
            }
            return (uint)src.Value;
        }
        uint NewAlloc()
        {
            byte[] alloc = new byte[4 * 3];

            Undo.UndoData undodata = Program.Undo.NewUndoData("NewAlloc");
            uint addr = InputFormRef.AppendBinaryData(alloc, undodata);
            if (addr == U.NOT_FOUND)
            {//割り当て失敗
                return U.NOT_FOUND;
            }

            Program.ROM.write_u8(addr + (4 * 2) + 2, 0x80, undodata);

            //新規に追加した分のデータを書き込み.
            Program.Undo.Push(undodata);

            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
            return addr;
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
            InputFormRef.ReInitPointer(script_pointer);
            FEBuilderGBA.Address.AddAddress(recycle
                , InputFormRef
                , ""
                , new uint[] {} 
                );
        }

        private void EventBattleDataFE7Form_Load(object sender, EventArgs e)
        {

        }

        public void MakeAddressListExpandsCallback(EventHandler eventHandler)
        {
            this.InputFormRef.MakeAddressListExpandsCallback(eventHandler);
        }
    }
}
