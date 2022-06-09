using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SoundRoomCGForm : Form
    {
        public SoundRoomCGForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawCGAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.sound_room_cg_pointer
                , 4
                , (int i, uint addr) =>
                {//0xFF FF FF FFまで
                    if (Program.ROM.u32(addr) == 0xFFFFFFFF)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint cg_id = (uint)Program.ROM.u32(addr);
                    return U.ToHexString(cg_id) + " " + ImageCGFE7UForm.GetComment(cg_id);
                }
                );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "SoundRoomCG";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] {  });
        }

        private void SoundRoomCGForm_Load(object sender, EventArgs e)
        {

        }

    }
}
