using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventMapChangeForm : Form
    {
        public EventMapChangeForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);

            //マップIDリストを作る.
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref this.MAP_LISTBOX);
            //マップを最前面に移動する.
            MapPictureBox.BringToFront();
        }

        InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 12
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    return (addr).ToString("X08");
                }
                );
        }

        private void EventMapChangeForm_Load(object sender, EventArgs e)
        {
        }

        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                this.InputFormRef.ClearSelect();
                return;
            }
            uint addr = MapSettingForm.GetMapChangeAddrWhereMapID(mapid);
            if (!Program.ROM.isSafetyOffset(addr))
            {
                this.InputFormRef.ClearSelect();
                return;
            }

            this.ReadStartAddress.Value = addr;

            MapPictureBox.LoadMap(mapid);

            this.MapPictureBox.ClearAllPoint();
            this.InputFormRef.ReInit(addr);
        }

        public void JumpToMAPID(uint mapid)
        {
            MAP_LISTBOX.SelectedIndex = (int)mapid;
        }

        private void P8_ValueChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            int width = (int) B3.Value;
            int height = (int) B4.Value;
            uint change_address = (uint) P8.Value;
            MapPictureBox.SetDefualtIcon(
                MapSettingForm.DrawMapChange(mapid, width, height, change_address)
            );
        }
    }
}
