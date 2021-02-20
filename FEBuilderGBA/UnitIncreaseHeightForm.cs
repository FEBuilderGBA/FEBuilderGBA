using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class UnitIncreaseHeightForm : Form
    {
        public UnitIncreaseHeightForm()
        {
            InitializeComponent();

            //伸ばさない
            string a = MyTranslateResource.str((string)L_0_COMBO.Items[0]);
            L_0_COMBO.Items[0] = a.Replace("{0}", U.ToHexString(Program.ROM.RomInfo.unit_increase_height_no()));

            //伸ばす
            a = MyTranslateResource.str((string)L_0_COMBO.Items[1]);
            L_0_COMBO.Items[1] = a.Replace("{0}", U.ToHexString(Program.ROM.RomInfo.unit_increase_height_yes()));

            this.AddressList.OwnerDraw(ListBoxEx.DrawImagePortraitAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.CheckProtectionAddrHigh = false; //書き換える対象がswitchなので低い位地に書き換えるデータがあります。
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            ReInit(this.InputFormRef);
        }
        private void UnitIncreaseHeightForm_Load(object sender, EventArgs e)
        {
            bool enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.unit_increase_height_switch2_address());
            if (!enable)
            {
                this.ERROR_NOT_FOUND.Show();
                this.WriteButton.Hide();
            }
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
                    return false;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(Program.ROM.RomInfo.unit_increase_height_switch2_address()) + (uint)i;
                    return U.ToHexString(id) + " " + ImagePortraitForm.GetPortraitName(id);
                }
                );
        }

        static uint ReInit(InputFormRef ifr)
        {
            if (ifr == null)
            {
                return U.NOT_FOUND;
            }
            uint addr = Program.ROM.p32(Program.ROM.RomInfo.unit_increase_height_pointer());
            uint count = Program.ROM.u8(Program.ROM.RomInfo.unit_increase_height_switch2_address() + 2);
            bool enable = PatchUtil.IsSwitch2Enable(Program.ROM.RomInfo.unit_increase_height_switch2_address());
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

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SwitchListExpandsButton_Click(object sender, EventArgs e)
        {
            if (this.L_0_COMBO.Items.Count <= 0)
            {
                return;
            }
            uint defAddr = U.atoh(this.L_0_COMBO.Items[0].ToString());

            uint newCount = ImagePortraitForm.DataCount();

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"UnitIncreaseHeight SwitchExpands");

            PatchUtil.Switch2Expands(Program.ROM.RomInfo.unit_increase_height_pointer()
                , Program.ROM.RomInfo.unit_increase_height_switch2_address()
                , newCount
                , defAddr
                , undodata);
            Program.Undo.Push(undodata);

            ReInit(this.InputFormRef);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "UnitIncreaseHeight";

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
