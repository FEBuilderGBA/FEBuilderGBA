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
    public partial class ExtraUnitForm : Form
    {
        public ExtraUnitForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.CheckProtectionAddrHigh = false;

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, FLAG, FLAG_LABEL,  0, "FLAG", new string[] {});
            InputFormRef.makeJumpEventHandler(FLAG, FLAG_JUMP, "FLAG", new string[] { });
        }

        static uint GetFlagAddr(int i)
        {
            return (uint)(i * 0x14 + 0x37E10);
        }

        //エクストラユニットは、FE8JとFE8Uで実装が違う
        //FE8Jはif文
        //FE8Uはテーブル
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , 0x0
                , 0x4
                , (int i, uint addr) =>
                {
                    return U.isSafetyPointer(Program.ROM.u32(addr) );
                }
                , (int i, uint addr) =>
                {
                    uint flagAddr = GetFlagAddr(i);
                    
                    uint flag_id = Program.ROM.u8(flagAddr);
                    uint unitsAddr = Program.ROM.p32(addr);
                    uint unit_id = Program.ROM.u8(unitsAddr);
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitName(unit_id) + " (" + R._("フラグ") + ":" + U.ToHexString(flag_id) + ")";
                }
                );
            ifr.ReInit(0x37EE4);
            return ifr;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "ExtraUnit", new uint[] { });
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                string name = "ExtraUnit";
                EventUnitForm.RecycleOldUnits(ref list, name, addr + 0);

                uint flagAddr = GetFlagAddr(i);
                FEBuilderGBA.Address.AddAddress(list
                    , flagAddr
                    , 1
                    , U.NOT_FOUND
                    , "ExtraUnit Flag"
                    ,FEBuilderGBA.Address.DataTypeEnum.BIN);
            }
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            int selected = AddressList.SelectedIndex;
            if (selected < 0)
            {
                return;
            }
            uint flagAddr = GetFlagAddr(selected);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"FLAG");
            Program.ROM.write_u8(flagAddr, (uint)FLAG.Value, undodata);
            Program.Undo.Push(undodata);
        }

        private void ExtraUnitForm_Load(object sender, EventArgs e)
        {

        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = AddressList.SelectedIndex;
            if (selected < 0)
            {
                return;
            }
            uint flagAddr = GetFlagAddr(selected);
            this.FLAG.Value = Program.ROM.u8(flagAddr);
        }
    }
}
