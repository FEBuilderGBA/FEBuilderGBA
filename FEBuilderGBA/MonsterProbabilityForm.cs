using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MonsterProbabilityForm : Form
    {
        public MonsterProbabilityForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            B5.ValueChanged += AddressList_SelectedIndexChanged;
            B6.ValueChanged += AddressList_SelectedIndexChanged;
            B7.ValueChanged += AddressList_SelectedIndexChanged;
            B8.ValueChanged += AddressList_SelectedIndexChanged;
            B9.ValueChanged += AddressList_SelectedIndexChanged;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.monster_probability_pointer()
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void MonsterProbabilityForm_Load(object sender, EventArgs e)
        {

        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sum = (int)B5.Value + (int)B6.Value + (int)B7.Value + (int)B8.Value + (int)B9.Value;
            SUM.Text = sum.ToString() + "%";
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                string name = "MonsterProbabilityForm";
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
            }
        }
        public static Bitmap DrawUnitsList(uint index, int iconSize, out string errorMessage)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(index);
            if (!U.isSafetyOffset(addr))
            {
                errorMessage = R._("範囲外のIDを指定しています。\r\n指定できるのは、最大{0}までです。", InputFormRef.DataCount);
                return ImageUtil.BlankDummy();
            }
            errorMessage = "";

            Bitmap bitmap = new Bitmap((iconSize*2) * 5, iconSize);
            Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            SolidBrush brush = new SolidBrush(OptionForm.Color_Input_ForeColor());
            Font font = new Font("MS UI Gothic", 8);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (uint i = 0; i < 5; i++)
                {
                    uint classid = Program.ROM.u8(addr + i);
                    if (classid <= 0)
                    {
                        continue;
                    }
                    Bitmap icon = ClassForm.DrawWaitIcon(classid, 2);

                    U.MakeTransparent(icon);

                    Rectangle b = bounds;
                    b.Width = iconSize;
                    b.Height = iconSize;

                    bounds.X += U.DrawPicture(icon, g, true, b);

                    uint probability = Program.ROM.u8(addr + 5 + i);
                    bounds.X += U.DrawText( probability + "", g , font, brush, true , bounds);
                }
            }
            brush.Dispose();
            font.Dispose();
            return bitmap;
        }
    }
}
