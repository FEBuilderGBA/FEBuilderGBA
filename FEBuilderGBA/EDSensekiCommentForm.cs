using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EDSensekiCommentForm : Form
    {
        public EDSensekiCommentForm()
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
                , Program.ROM.RomInfo.senseki_comment_pointer()
                , 16
                , (int i, uint addr) =>
                {//終端
                    return Program.ROM.u16(addr) != 0x0;
                }
                , (int i, uint addr) =>
                {
                    uint uid = Program.ROM.u32(addr + 0);
                    return U.ToHexString(uid) +
                        " " + UnitForm.GetUnitName(uid)
                        ;
                }
                );
        }

        private void EDSensekiCommentForm_Load(object sender, EventArgs e)
        {

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "EDSensekiForm";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] {  });
        }
        public static void MakeTextIDArray(List<TextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            TextID.AppendTextID(list, FELint.Type.UNIT, InputFormRef, new uint[] { 4, 8 , 12 });
        }
    }
}
