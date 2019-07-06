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
    public partial class AIUnitsForm : Form
    {
        public AIUnitsForm()
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
                , 0
                , 2
                , (int i, uint addr) =>
                {
                    return Program.ROM.u16(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint unitid = Program.ROM.u8(addr + 0);
                    return U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);
                }
                );
        }

        private void AIUnitsForm_Load(object sender, EventArgs e)
        {

        }
        public void JumpTo(uint addr)
        {
            this.InputFormRef.ReInit(addr);
        }
        public uint GetBaseAddress()
        {
            return this.InputFormRef.BaseAddress;
        }
        public static uint CalcLength(uint addr)
        {
            addr = U.toOffset(addr);
            uint start = addr;
            uint length = (uint)Program.ROM.Data.Length;
            for (; addr < length; addr += 2)
            {
                if (Program.ROM.u16(addr) == 0x00)
                {
                    break;
                }
            }

            return addr - start;
        }
        public static Bitmap DrawAIUnitsList(uint units_address, int iconSize)
        {
            units_address = U.toOffset(units_address);
            if (!U.isSafetyOffset(units_address))
            {
                return ImageUtil.BlankDummy();
            }

            int count = 0;
            uint addr = units_address;
            while (Program.ROM.u16(addr) != 0x0)
            {
                addr += 2;
                if (!U.isSafetyOffset(addr))
                {
                    break;
                }
                count++;
            }
            if (count <= 0)
            {
                return ImageUtil.BlankDummy();
            }

            Bitmap bitmap = new Bitmap(iconSize * count, iconSize);
            Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                addr = units_address;
                for (int i = 0; i < count; i++)
                {
                    uint unit_id = Program.ROM.u8(addr + 0);
                    Bitmap icon = UnitForm.DrawUnitMapFacePicture(unit_id);
                    if (ImageUtil.IsBlankBitmap(icon))
                    {
                        uint class_id = UnitForm.GetClassID(unit_id);
                        icon = ClassForm.DrawWaitIcon(class_id);
                    }
                    U.MakeTransparent(icon);

                    Rectangle b = bounds;
                    b.Width = iconSize;
                    b.Height = iconSize;

                    bounds.X += U.DrawPicture(icon, g, true, b);
                    addr += 2;
                }
            }
            return bitmap;
        }
    }
}
