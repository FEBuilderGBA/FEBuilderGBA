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
    public partial class EventTalkGroupFE7Form : Form
    {
        public EventTalkGroupFE7Form()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {//無効なデータまで検索
                    return i <= 0xD;
                }
                , (int i, uint addr) =>
                {
                    uint textid = Program.ROM.u16(addr);

                    return new U.AddrResult(addr, (textid).ToString("X08"));
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
            byte[] alloc = new byte[4*0xE];

            Undo.UndoData undodata = Program.Undo.NewUndoData("NewAlloc");
            uint addr = InputFormRef.AppendBinaryData(alloc, undodata);
            if (addr == U.NOT_FOUND)
            {//割り当て失敗
                return U.NOT_FOUND;
            }

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
                , new uint[] { }
                );
        }

        public static void MakeVarsIDArray(List<UseValsID> list, uint script_addr)
        {
            script_addr = U.toOffset(script_addr);
            if (!U.isSafetyOffset(script_addr))
            {
                return;
            }

            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInit(script_addr);

            UseValsID.AppendTextID(list, FELint.Type.POINTER_TALKGROUP, InputFormRef, new uint[] { 0 }  );
        }

        public void MakeAddressListExpandsCallback(EventHandler eventHandler)
        {
            this.InputFormRef.MakeAddressListExpandsCallback(eventHandler);
        }

        private void EventMoveDataFE7Form_Load(object sender, EventArgs e)
        {

        }

    }
}
