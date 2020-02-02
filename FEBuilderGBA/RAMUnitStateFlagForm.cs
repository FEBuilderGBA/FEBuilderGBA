using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class RAMUnitStateFlagForm : Form
    {
        public RAMUnitStateFlagForm()
        {
            InitializeComponent();

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("", controls);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            uint flag1 = (((uint)B40.Value) & 0xFF);
            uint flag2 = (((uint)B41.Value) & 0xFF);
            uint flag3 = (((uint)B42.Value) & 0xFF);
            uint flag4 = (((uint)B43.Value) & 0xFF);

            ApplyButton.Tag = flag1 | (flag2 << 8) | (flag3 << 16) | (flag4 << 24);
        }
        public void JumpTo(uint value)
        {
            B40.Value = value & 0xff;
            B41.Value = (value >> 8) & 0xff;
            B42.Value = (value >> 16) & 0xff;
            B43.Value = (value >> 24) & 0xff;
        }

        ToolTipEx X_Tooltip;
        private void RAMUnitStateFlagForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            X_Tooltip = InputFormRef.GetToolTip<RAMUnitStateFlagForm>();
            InputFormRef.LoadCheckboxesResource("ramunit_state_checkbox_", controls, X_Tooltip, "", "L_40_BIT_", "L_41_BIT_", "L_42_BIT_", "L_43_BIT_");
        }
    }
}
