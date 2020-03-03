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
    public partial class VennouWeaponLockForm : Form
    {
        public VennouWeaponLockForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(Draw, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            uint typeID = 0;
            return new InputFormRef(self
                , ""
                , 0
                , 1
                , (int i, uint addr) =>
                {
                    if (i == 0)
                    {
                        return true;
                    }
                    return Program.ROM.u8(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(addr + 0);
                    if (i == 0)
                    {
                        typeID = id;
                        return TypeIDToString(id);
                    }
                    if (typeID <= 1)
                    {
                        return U.ToHexString(id) + " " + UnitForm.GetUnitName(id);
                    }
                    else
                    {
                        return U.ToHexString(id) + " " + ClassForm.GetClassName(id);
                    }
                }
                );
        }

        static string TypeIDToString(uint id)
        {
            if (id == 0)
            {
                return "Soft character lock";
            }
            else if (id == 1)
            {
                return "Hard character lock";
            }
            else if (id == 2)
            {
                return "Soft class lock";
            }
            else if (id == 3)
            {
                return "Hard class lock";
            }
            return "";
        }

        public void JumpTo(uint addr)
        {
            this.InputFormRef.ReInit(addr);
        }
        public uint GetBaseAddress()
        {
            return this.InputFormRef.BaseAddress;
        }
        static uint CalcLength(uint addr)
        {
            addr = U.toOffset(addr);
            if (! U.isSafetyOffset(addr))
            {
                return 0;
            }
            uint start = addr;
            uint length = (uint)Program.ROM.Data.Length;
            addr++;
            for (; addr < length; addr += 1)
            {
                if (Program.ROM.u8(addr) == 0x00)
                {
                    break;
                }
            }

            return addr - start;
        }
        public static string GetNamesByIndexCap(uint index)
        {
            if (index == 0)
            {
                return "-NULL-"; ///No Translate
            }
            string name = GetNamesByIndex(index);
            if (name == "")
            {
                return "-EMPTY-";///No Translate
            }
            return name;
        }
        public static string GetNamesByIndex(uint index)
        {
            if (index == 0)
            {
                return "";
            }

            uint addr = PatchUtil.SearchVennouWeaponLockArrayAddr();
            if (! U.isSafetyOffset(addr))
            {
                return "";
            }
            addr = addr + (index * 4);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            addr = Program.ROM.p32(addr);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            return GetNames(addr);
        }
        public static string GetNames(uint addr)
        {
            addr = U.toOffset(addr);
            if (! U.isSafetyOffset(addr))
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            uint start = addr;
            uint length = (uint)Program.ROM.Data.Length;

            uint typeID = Program.ROM.u8(addr);
            addr++;
            for (; addr < length; addr += 1)
            {
                uint id = Program.ROM.u8(addr);
                if (id == 0x00)
                {
                    break;
                }

                if (typeID <= 1)
                {
                    sb.Append(UnitForm.GetUnitName(id));
                }
                else
                {
                    sb.Append(ClassForm.GetClassName(id));
                }
                sb.Append(' ');
            }

            return sb.ToString();
        }

        private void VennouWeaponLockForm_Load(object sender, EventArgs e)
        {

        }

        uint GetTypeID()
        {
            uint addr = GetBaseAddress();
            if (! U.isSafetyOffset(addr))
            {
                return 0;
            }
            return Program.ROM.u8(addr);
        }

        private void B0_ValueChanged(object sender, EventArgs e)
        {
            uint id = (uint)this.B0.Value;
            if (AddressList.SelectedIndex <= 0)
            {
                X_LINK.Text = TypeIDToString(id);
                X_LINK_ICON.Image = null;
                return;
            }
            uint typeID = GetTypeID();
            if (typeID <= 1)
            {
                X_LINK.Text = UnitForm.GetUnitName(id);
                X_LINK_ICON.Image = UnitForm.DrawUnitMapFacePicture(id);
            }
            else
            {
                X_LINK.Text = ClassForm.GetClassName(id);
                X_LINK_ICON.Image = ClassForm.DrawWaitIcon(id, 0);
            }
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddressList.SelectedIndex <= 0)
            {
                J_0.Text = R._("種類");
                Explain.Show();
                return;
            }
            uint typeID = GetTypeID();
            if (typeID <= 1)
            {
                J_0.Text = R._("ユニット");
            }
            else
            {
                J_0.Text = R._("クラス");
            }
            Explain.Hide();
        }

        //待機アイコン + テキストを書くルーチン
        Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (ListBoxEx.OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap;
            if (index == 0)
            {
                bitmap = ImageUtil.Blank(8,8);
            }
            else
            {
                uint typeID = GetTypeID();
                if (typeID <= 1)
                {
                    bitmap = UnitForm.DrawUnitMapFacePicture(icon);
                }
                else
                {
                    bitmap = ClassForm.DrawWaitIcon(icon, 0, true);
                }
            }
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);
            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;

            brush.Dispose();
            return new Size(bounds.X, bounds.Y);
        }
        public static void MakeDataLength(List<Address> list, uint pointer, string strname)
        {
            uint addr = Program.ROM.u32(pointer);
            if (!U.isSafetyPointer(addr))
            {
                return;
            }
            uint length = CalcLength(addr);
            FEBuilderGBA.Address.AddAddress(list, addr, length, pointer
                , strname, FEBuilderGBA.Address.DataTypeEnum.BIN);
        }
    }
}
