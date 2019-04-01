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
    public partial class ExtraUnitFE8UForm : Form
    {
        public ExtraUnitFE8UForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.CheckProtectionAddrHigh = false; //書き換える対象がswitchなので低い位地に書き換えるデータがあります。
        }

        //エクストラユニットは、FE8JとFE8Uで実装が違う
        //FE8Jはif文
        //FE8Uはテーブル
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , 0x37D88
                , 0x8
                , (int i, uint addr) =>
                {
                    return U.isSafetyPointer(Program.ROM.u32(addr + 4) );
                }
                , (int i, uint addr) =>
                {
                    uint flag_id = Program.ROM.u32(addr + 0);
                    uint unitsAddr = Program.ROM.p32(addr + 4);
                    uint unit_id = Program.ROM.u8(unitsAddr);
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitName(unit_id) + " (" + R._("フラグ") + ":" + U.ToHexString(flag_id) + ")";
                }
                );
            return ifr;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "ExtraUnit", new uint[] { });

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize)
            {
                string name = "ExtraUnit";
                EventUnitForm.RecycleOldUnits(ref list, name, addr + 4);
            }
        }

        private void ExtraUnitForm_Load(object sender, EventArgs e)
        {

        }
    }
}
