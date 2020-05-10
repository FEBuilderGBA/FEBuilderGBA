using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SupportAttributeForm : Form
    {
        public SupportAttributeForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawAtributeAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.support_attribute_pointer()
                , 8
                , (int i, uint addr) =>
                {//個数が固定できまっている
                    uint v = Program.ROM.u8(addr);
                    return v != 0;
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    return U.ToHexString(i + 1) + " " + InputFormRef.GetAttributeName((uint)i + 1) + InputFormRef.GetCommentSA(addr);
                }
                );
        }
        
        private void SupportAttributeForm_Load(object sender, EventArgs e)
        {
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "SupportAttribute";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
    }
}
