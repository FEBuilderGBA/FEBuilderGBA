using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class LinkArenaDenyUnitForm : Form
    {
        public LinkArenaDenyUnitForm()
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
                , Program.ROM.RomInfo.link_arena_deny_unit_pointer()
                , 2
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint unitid = Program.ROM.u8(addr + 0);
                    return U.ToHexString(unitid) + " "+  UnitForm.GetUnitName(unitid);
                }
                );
        }

        
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "LinkAreaDenyUnitForm", new uint[] { });
        }

        private void ArenaClassForm_Load(object sender, EventArgs e)
        {

        }
    }
}
