using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SupportTalkForm : Form
    {
        public SupportTalkForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnit2AndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.support_talk_pointer()
                , 16
                , (int i, uint addr) =>
                {//とりあえず FF FF まで読む.
                    if (Program.ROM.u16(addr) == 0xFFFF)
                    {
                        return false;
                    }
                    if (i > 10 && Program.ROM.IsEmpty(addr, 16 * 10))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint uid1 = Program.ROM.u8(addr + 0);
                    uint uid2 = Program.ROM.u8(addr + 2);
                    return U.ToHexString(uid1) +
                        " " + UnitForm.GetUnitName( uid1 ) +
                        " & " 
                          +U.ToHexString(uid2) +
                        " " + UnitForm.GetUnitName( uid2 )
                        ;
                }
                );
        }

        private void SupportTalkForm_Load(object sender, EventArgs e)
        {
        }
        public void JumpTo(uint unit1, uint unit2)
        {
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint data_unit1 = Program.ROM.u16(addr + 0);
                uint data_unit2 = Program.ROM.u16(addr + 2);
                if ((unit1 == data_unit1 && unit2 == data_unit2)
                 || (unit2 == data_unit1 && unit1 == data_unit2)
                    )
                {
                    AddressList.SelectedIndex = i;
                    return;
                }
                addr += InputFormRef.BlockSize;
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "SupportTalk";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 2)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SUPPORT_TALK, U.NOT_FOUND
                    , R._("支援会話が極端に少ないです。破損している可能性があります。")));
            }

            uint battletalk_addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, battletalk_addr += InputFormRef.BlockSize)
            {
                uint id = (uint)i;

                uint textid = Program.ROM.u16(battletalk_addr + 4);
                FELint.ConversationTextMessage(textid, errors, FELint.Type.SUPPORT_TALK, battletalk_addr, (uint)i);

                textid = Program.ROM.u16(battletalk_addr + 6);
                FELint.ConversationTextMessage(textid, errors, FELint.Type.SUPPORT_TALK, battletalk_addr, (uint)i);

                textid = Program.ROM.u16(battletalk_addr + 8);
                FELint.ConversationTextMessage(textid, errors, FELint.Type.SUPPORT_TALK, battletalk_addr, (uint)i);
            }
        }
        public static void MakeTextIDArray(List<TextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            TextID.AppendTextID(list, FELint.Type.SUPPORT_TALK, InputFormRef, new uint[] { 4, 6, 8 });
        }

    }
}
