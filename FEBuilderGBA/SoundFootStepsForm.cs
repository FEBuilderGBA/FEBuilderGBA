using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SoundFootStepsForm : Form
    {
        public SoundFootStepsForm()
        {
            InitializeComponent();

            InputFormRef.LoadComboResource(L_0_COMBO, U.ConfigDataFilename("sound_foot_steps_"));

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.CheckProtectionAddrHigh = false; //switchを変更するので、保護を無効にする.

            ReInit(this.InputFormRef);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {
                    return U.isPointer( Program.ROM.u32(addr) );
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(Program.ROM.RomInfo.sound_foot_steps_switch2_address) + (uint)i;
                    return U.ToHexString(id) + ClassForm.GetClassName(id);
                }
                );
        }

        private void SoundFootStepsForm_Load(object sender, EventArgs e)
        {
            bool enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.sound_foot_steps_switch2_address);
            if (!enable)
            {
                this.ERROR_NOT_FOUND.Show();
                this.WriteButton.Hide();
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            ReInit(InputFormRef);
            FELint.CheckInputFormRefASM(InputFormRef, errors, true, FELint.Type.SOUND_FOOT_STEPS);
        }

        static uint ReInit(InputFormRef ifr)
        {
            if (ifr == null)
            {
                return U.NOT_FOUND;
            }
            uint addr = Program.ROM.p32(Program.ROM.RomInfo.sound_foot_steps_pointer);
            uint count = Program.ROM.u8(Program.ROM.RomInfo.sound_foot_steps_switch2_address + 2);
            bool enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.sound_foot_steps_switch2_address);
            if (enable == false)
            {
                return U.NOT_FOUND;
            }
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }

            ifr.ReInit(addr, count + 1);
            return addr;
        }
        private void SwitchListExpandsButton_Click(object sender, EventArgs e)
        {
            if (this.L_0_COMBO.Items.Count <= 0)
            {
                return;
            }
            uint defAddr = U.atoh(this.L_0_COMBO.Items[0].ToString());

            uint newCount = ClassForm.DataCount();

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"SoundFootStep SwitchExpands");

            PatchUtil.Switch2Expands(Program.ROM.RomInfo.sound_foot_steps_pointer
                , Program.ROM.RomInfo.sound_foot_steps_switch2_address
                , newCount
                , defAddr
                , undodata);

            if (Program.ROM.RomInfo.version == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte)
                {//Fix PlaySoundStepByClass(足音) HardCode
                    Program.ROM.write_range(0x7B198, new byte[] { 0x1c, 0xe0 }, undodata);
                }
                else
                {//Fix PlaySoundStepByClass(足音) HardCode
                    Program.ROM.write_range(0x78d84, new byte[] { 0x1c, 0xe0 }, undodata);
                }
            }

            Program.Undo.Push(undodata);

            ReInit(this.InputFormRef);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "SoundFootStepsPointer";

            InputFormRef ifr = Init(null);
            ReInit(ifr);
            FEBuilderGBA.Address.AddAddress(list
                , ifr
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            List<U.AddrResult> arlist = ifr.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);
        }

    }
}
