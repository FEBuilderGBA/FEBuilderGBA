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
    public partial class UnitsShortTextForm : Form
    {
        public UnitsShortTextForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 2
                , (int i, uint addr) =>
                {
                    return i < DATAMAX;
                }
                , (int i, uint addr) =>
                {
                    uint unitid = (uint)i;
                    return U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);
                }
                );
        }

        public void JumpTo(uint addr)
        {
            this.InputFormRef.ReInit(addr);
        }
        public uint GetBaseAddress()
        {
            return this.InputFormRef.BaseAddress;
        }

        private void UnitsShortTextForm_Load(object sender, EventArgs e)
        {

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

        const uint DATAMAX = 0x45 + 1;
        uint DefaultText()
        {
            if (Program.ROM.RomInfo.version == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte)
                {//FE8J
                    return 0x8EE;
                }
                else
                {//FE8U
                    return 0x92E;
                }
            }
            return 1;
        }
        uint NewAlloc()
        {
            byte[] alloc = new byte[U.Padding4(2 * (DATAMAX+1))];

            uint default_text = DefaultText();
            for (uint i = 1; i < DATAMAX; i++)
            {
                U.write_u16(alloc, i * 2, default_text);
            }

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
            FEBuilderGBA.Address.AddPointer(recycle, script_pointer, 2 * DATAMAX, "UnitsShortText", FEBuilderGBA.Address.DataTypeEnum.BIN);
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

            UseValsID.AppendTextID(list, FELint.Type.POINTER_UNITSSHORTTEXT, InputFormRef, new uint[] { 0 });
        }

    }
}
