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
    public partial class AIMapSettingForm : Form
    {
        public AIMapSettingForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ai_map_setting_pointer()
                , 4
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr) != 0xFF;
                }
                , (int i, uint addr) =>
                {
                    uint map_id = (uint)i;
                    return U.ToHexString(map_id) + " " + MapSettingForm.GetMapName(map_id);
                }
                );
        }

        ToolTipEx X_Tooltip;
        private void AITargetForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);

            X_Tooltip = InputFormRef.GetToolTip<AIMapSettingForm>();
            InputFormRef.LoadCheckboxesResource(U.ConfigDataFilename("ai_map_setting_checkbox_")
                , controls, X_Tooltip, ""
                , "L_0_BIT_", "L_1_BIT_", "L_2_BIT_", "L_3_BIT_");
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "AIMapSetting", new uint[] {  });
        }
    }
}
