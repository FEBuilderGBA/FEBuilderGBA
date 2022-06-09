using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace FEBuilderGBA
{
    public partial class ImageSystemAreaForm : Form
    {
        uint[] BasePointers = new uint[]{
             Program.ROM.RomInfo.systemarea_move_gradation_palette_pointer
            ,Program.ROM.RomInfo.systemarea_attack_gradation_palette_pointer
            ,Program.ROM.RomInfo.systemarea_staff_gradation_palette_pointer
        };

        public ImageSystemAreaForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.AddressList.OwnerDraw(ListBoxEx.DrawColorAndText, DrawMode.OwnerDrawFixed);
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
                    return i < 60/2;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + Program.ROM.u16(addr).ToString("X4");
                }
                );
        }


        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef == null)
            {
                return;
            }

            int seleceted = FilterComboBox.SelectedIndex;
            if (seleceted < 0 || seleceted >= this.BasePointers.Length)
            {
                return;
            }

            uint addr = Program.ROM.p32(this.BasePointers[seleceted]);
            this.InputFormRef.ReInit( addr );
       }


        public void JumpToAddr(uint addr)
        {
            for (int i = 0; i < this.BasePointers.Length; i++)
            {
                uint a = Program.ROM.p32(this.BasePointers[i]);
                if (a == addr)
                {
                    FilterComboBox.SelectedIndex = i;
                    break;
                }
            }

            this.InputFormRef.ReInit(addr);
        }

        public static List<U.AddrResult> MakeList(uint addr = U.NOT_FOUND)
        {
            InputFormRef InputFormRef = Init(null);
            if (addr != U.NOT_FOUND)
            {
                InputFormRef.ReInit(addr);
            }
            return InputFormRef.MakeList();
        }

        private void ImageSystemAreaForm_Load(object sender, EventArgs e)
        {

        }
    }
}
