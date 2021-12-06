using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class AOERANGEForm : Form
    {
        public AOERANGEForm()
        {
            InitializeComponent();
        }

        private void AOERANGEForm_Load(object sender, EventArgs e)
        {

        }

        public static void MakeDataLength(List<Address> list, uint pointer, string strname)
        {
            if (! U.isSafetyOffset(pointer))
            {
                return;
            }
            uint addr = Program.ROM.p32(pointer);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            uint w = Program.ROM.u8(addr + 0);
            uint h = Program.ROM.u8(addr + 1);
            uint length = 4 + (w * h);

            FEBuilderGBA.Address.AddPointer(list, pointer, length, strname, Address.DataTypeEnum.BIN);
        }

    }
}
