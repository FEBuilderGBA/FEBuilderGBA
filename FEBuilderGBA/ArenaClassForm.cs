using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ArenaClassForm : Form
    {
        public ArenaClassForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);

            FilterComboBox.SelectedIndex = 0;
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            if (Program.ROM.RomInfo.version() >= 8)
            {
                X_FE8J_COMMENT.Visible = true;
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.arena_class_near_weapon_pointer()
                , 1
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0x00;
                }
                , (int i, uint addr) =>
                {
                    uint classid = Program.ROM.u8(addr + 0);
                    return U.ToHexString(classid) + " "+  ClassForm.GetClassName(classid);
                }
                );
        }

        
        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef == null)
            {
                return;
            }

            if (FilterComboBox.SelectedIndex == 0)
            {//近距離
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.arena_class_near_weapon_pointer()) );
            }
            else
            {//遠距離
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.arena_class_far_weapon_pointer()));
            }

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.arena_class_near_weapon_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "AreaClassForm near weapon", new uint[] { });

            InputFormRef.ReInitPointer((Program.ROM.RomInfo.arena_class_far_weapon_pointer()));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "AreaClassForm far weapon", new uint[] { });
        }

        private void ArenaClassForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
        }
    }
}
