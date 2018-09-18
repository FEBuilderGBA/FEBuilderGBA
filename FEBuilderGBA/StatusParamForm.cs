using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class StatusParamForm : Form
    {
        public StatusParamForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            InputFormRef.OwnerDrawColorCombo(L_8_COMBO);

            this.FilterComboBox.BeginUpdate();
            this.FilterComboBox.Items.Add(R._("0=ステータスパラメータ"));
            this.FilterComboBox.Items.Add(R._("1=所持アイテム"));
            if (Program.ROM.RomInfo.is_multibyte())
            {//日本語版のみ 武器の属性 "剣"みたいなのを出す機能がある.
                this.FilterComboBox.Items.Add(R._("2=武器レベル 武器"));
                this.FilterComboBox.Items.Add(R._("3=武器レベル 魔法"));
            }
            this.FilterComboBox.EndUpdate();

            this.FilterComboBox.SelectedIndex = 0;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 16
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr + 12));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + GetName(addr);
                }
                );
        }

        static string GetName(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint nameAddrP = Program.ROM.u32(addr + 12);
            return GetParamName(nameAddrP);
        }
        public static string GetParamName(uint nameAddrP)
        {
            nameAddrP = U.toOffset(nameAddrP);
            if (!U.isSafetyOffset(nameAddrP))
            {
                return "";
            }
            uint id = Program.ROM.p32(nameAddrP);
            if (id <= 0x10)
            {
                return "";
            }

            string name = "";
            if (id > 0xFFFF && U.isSafetyOffset(id))
            {
                name = Program.ROM.getString(id);
            }
            if(name=="")
            {
                name = TextForm.Direct(id);
            }

            return name;
        }
        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef == null)
            {
                return;
            }

            int selected = this.FilterComboBox.SelectedIndex;
            if (selected == 0)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_param1_pointer()));
            }
            else if (selected == 1)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_param2_pointer()));
            }
            else if (selected == 2)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_param3w_pointer()));
            }
            else if (selected == 3)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_param3m_pointer()));
            }
        }



        public static List<U.AddrResult> MakeList(uint addr = U.NOT_FOUND)
        {
            InputFormRef InputFormRef = Init(null);
            if (addr != U.NOT_FOUND)
            {
                InputFormRef.ReInit(addr);
            }
            return InputFormRef.MakeList();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            uint[] addlist = new uint[] { Program.ROM.RomInfo.status_param1_pointer(), Program.ROM.RomInfo.status_param2_pointer(), Program.ROM.RomInfo.status_param3w_pointer(), Program.ROM.RomInfo.status_param3m_pointer() };

            for (int n = 0; n < addlist.Length; n++)
            {
                uint addr = addlist[n];
                if (addr == 0)
                {
                    continue;
                }

                InputFormRef InputFormRef = Init(null);
                InputFormRef.ReInitPointer((addlist[n]));
                string name = "StatusParam" + n;
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 , 4 , 12 });

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    uint paddr = Program.ROM.p32(12 + p);
                    FEBuilderGBA.Address.AddCString(list
                        , p + 12
                        );
                }
            }
        }
        public static void MakeTextIDArray(List<TextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            TextID.AppendTextIDPP(list, FELint.Type.UNIT, InputFormRef, new uint[] { 12 });
        }
    }
}
