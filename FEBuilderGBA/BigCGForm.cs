using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class BigCGForm : Form
    {
        public BigCGForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
        }

        InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.p32(Program.ROM.RomInfo.bigcg_pointer)
                , 12
                , (int i, uint addr) =>
                {
                    return ROM.isPointer(Program.ROM.u32(addr));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        private void BigCGForm_Load(object sender, EventArgs e)
        {

        }
        public static Bitmap DrawImage(uint table, uint tsa, uint palette)
        {
            if (!ROM.isPointer(table))
            {
                return ImageUtil.Blank(8, 8);
            }
            if (!ROM.isPointer(tsa))
            {
                return ImageUtil.Blank(8, 8);
            }
            if (!ROM.isPointer(palette))
            {
                return ImageUtil.Blank(8, 8);
            }

            table = ROM.toOffset(table);

            List<byte> imageUZList = new List<byte>();
            for (int i = 0; i < 10; i++)
            {
                uint image = Program.ROM.u32((uint)(table + (i*4)));
                if (!ROM.isPointer(image))
                {
                    return ImageUtil.Blank(8, 8);
                }
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, ROM.toOffset(image));
                imageUZList.AddRange(imageUZ);
            }
            return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, 20 * 8
                , imageUZList.ToArray(), 0
                , Program.ROM.Data, (int)ROM.toOffset(palette)
                , Program.ROM.Data, (int)ROM.toOffset(tsa)
                , 0x8000 //TSAに謎の係数を足しながら、最下層のラインから格納するらしい
                , -0x80  //パレット番号が0x80シフトされるらしい
                );
        }
        public static Bitmap DrawImageByID(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.Blank(8, 8);
            }

            uint table = Program.ROM.u32(addr);
            uint tsa = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);

            return DrawImage(table, tsa, palette);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_PIC.Image = DrawImage((uint)P0.Value, (uint)P4.Value,(uint)P8.Value);
        }


    }
}
