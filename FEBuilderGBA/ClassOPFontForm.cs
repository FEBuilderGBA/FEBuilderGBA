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
    public partial class ClassOPFontForm : Form
    {
        public ClassOPFontForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
        }
        
        InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.p32(Program.ROM.RomInfo.op_class_font_pointer())
                , 4
                , (int i, uint addr) =>
                {
                    return ROM.isPointer(Program.ROM.u32(addr))
                        ;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) ;
                }
                );
        }

        private void ClassOPFontForm_Load(object sender, EventArgs e)
        {

        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.X_PIC.Image = DrawFont((uint)P0.Value);
        }

        public static Bitmap DrawFont(uint image)
        {
            if (!ROM.isPointer(image))
            {
                return ImageUtil.Blank(4*8, 4*8);
            }
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.op_class_font_palette_pointer()); 

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, ROM.toOffset(image));

            return ImageUtil.ByteToImage16Tile(4 * 8, 4 * 8
                , imageUZ, 0
                , Program.ROM.Data, (int)ROM.toOffset(palette)
                );
        }
        public static Bitmap DrawFontByID(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.Blank(8, 8);
            }

            uint image = Program.ROM.u32(addr);
            return DrawFont(image);
        }
    
    }
}
