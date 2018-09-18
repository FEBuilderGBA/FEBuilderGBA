using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SupportUnitFE6Form : Form
    {
        public SupportUnitFE6Form()
        {
            InitializeComponent();
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_0);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_1);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_2);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_3);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_4);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_5);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_6);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_7);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_8);
            InputFormRef.markupJumpLabel(this.X_JUMP_SUPPORTTALK_9);

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.AddressListExpandsEvent += SupportUnitForm.AddressListExpandsEvent;
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.support_unit_pointer()
                , 32
                , (int i, uint addr) =>
                {//とりあえず 00 まで読む.
                    if (Program.ROM.u8(addr) != 0)
                    {//0ではないのでまだデータがある.
                        return true;
                    }
                    //飛び地になっていることがあるらしい.
                    //4ブロックほど検索してみる.
                    uint found_addr = addr;
                    for (int n = 0; n < 4; n++, found_addr += 32)
                    {
                        uint uid = UnitForm.GetUnitIDWhereSupportAddr(found_addr);
                        if (uid != U.NOT_FOUND)
                        {//発見!
                            return true;
                        }
                    }
                    //見つからない.
                    return false;
                }
                , (int i, uint addr) =>
                {
                    uint uid = UnitForm.GetUnitIDWhereSupportAddr(addr);
                    if (uid == U.NOT_FOUND)
                    {
                        return "-EMPTY-";
                    }

                    return U.ToHexString(uid + 1) +
                        " " + UnitForm.GetUnitName(uid + 1);
                }
                );
        }

        private void SupportUnitFE6Form_Load(object sender, EventArgs e)
        {
        }
        void GotoSupportTalk(NumericUpDown src)
        {
            if (src.Value <= 0)
            {
                return;
            }

            uint addr = InputFormRef.SelectToAddr(AddressList);
            if (U.NOT_FOUND == addr)
            {
                return;
            }
            uint uid = UnitForm.GetUnitIDWhereSupportAddr(addr);
            if (uid == U.NOT_FOUND)
            {
                return;
            }
            uid = uid + 1;  //IDは1から降るので

            if (Program.ROM.RomInfo.version() == 8)
            {
                SupportTalkForm f = (SupportTalkForm)InputFormRef.JumpForm<SupportTalkForm>(U.NOT_FOUND);
                f.JumpTo(uid, (uint)src.Value);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                SupportTalkFE7Form f = (SupportTalkFE7Form)InputFormRef.JumpForm<SupportTalkFE7Form>(U.NOT_FOUND);
                f.JumpTo(uid, (uint)src.Value);
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                SupportTalkFE6Form f = (SupportTalkFE6Form)InputFormRef.JumpForm<SupportTalkFE6Form>(U.NOT_FOUND);
                f.JumpTo(uid, (uint)src.Value);
            }
        }


        private void X_JUMP_SUPPORTTALK_0_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B0);
        }

        private void X_JUMP_SUPPORTTALK_1_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B1);
        }

        private void X_JUMP_SUPPORTTALK_2_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B2);
        }

        private void X_JUMP_SUPPORTTALK_3_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B3);
        }

        private void X_JUMP_SUPPORTTALK_4_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B4);
        }

        private void X_JUMP_SUPPORTTALK_5_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B5);
        }

        private void X_JUMP_SUPPORTTALK_6_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B6);
        }

        private void X_JUMP_SUPPORTTALK_7_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B7);
        }

        private void X_JUMP_SUPPORTTALK_8_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B8);
        }

        private void X_JUMP_SUPPORTTALK_9_Click(object sender, EventArgs e)
        {
            GotoSupportTalk(B9);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "SupportUnitFE6";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
        public void JumpToAddr(uint search_addr)
        {
            search_addr = U.toOffset(search_addr);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                if (addr == search_addr)
                {
                    this.AddressList.SelectedIndex = i;
                    X_WARNING_OWN_EXPANDS.Hide();
                    AddressListExpandsButton.Enabled = true;
                    return;
                }
            }

            //見つからなかった.
            InputFormRef.ReInit(search_addr, 1);
            U.SelectedIndexSafety(this.AddressList, 0);

            X_WARNING_OWN_EXPANDS.Show();
            AddressListExpandsButton.Enabled = false;
        }
    }
}
