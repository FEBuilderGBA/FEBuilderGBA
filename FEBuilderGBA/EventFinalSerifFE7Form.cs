using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventFinalSerifFE7Form : Form
    {
        public EventFinalSerifFE7Form()
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
                , Program.ROM.RomInfo.event_final_serif_pointer()
                , 8
                , (int i, uint addr) =>
                {
                    uint unit_id = Program.ROM.u32(addr + 0);
                    return unit_id <= 0xff && unit_id >= 0x1;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = Program.ROM.u32(addr + 0);
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitName(unit_id);
                }
                );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            string name = "EventFinalserif";
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.EVENT_FINAL_SERIF, InputFormRef, new uint[] { 4 });
        }

        private void EventFinalSerifFE7Form_Load(object sender, EventArgs e)
        {

        }


    }
}
