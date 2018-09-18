using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventForceSortieForm : Form
    {
        public EventForceSortieForm()
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
                , Program.ROM.RomInfo.event_force_sortie_pointer()
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    return Program.ROM.u16(addr) != 0xFFFF;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = (uint)Program.ROM.u16(addr);
                    uint map_id = (uint)Program.ROM.u8(addr + 3);
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitNameAndANY(unit_id) +
                        " " + "(" + MapSettingForm.GetMapNameAndANYFF(map_id) + ")";
                }
            );
        }

        private void EventForceSortieForm_Load(object sender, EventArgs e)
        {
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "ForceSorite", new uint[] { });
            }
        }

    }
}
