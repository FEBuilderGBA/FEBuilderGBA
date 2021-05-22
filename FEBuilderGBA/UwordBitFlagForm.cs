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
    public partial class UwordBitFlagForm : Form
    {
        public UwordBitFlagForm()
        {
            InitializeComponent();

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("", controls);
            U.AddCancelButton(this);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            uint flag1 = (((uint)B40.Value) & 0xFF);
            uint flag2 = (((uint)B41.Value) & 0xFF);
            uint flag3 = (((uint)B42.Value) & 0xFF);
            uint flag4 = (((uint)B43.Value) & 0xFF);

            ApplyButton.Tag = flag1 | (flag2 << 8) | (flag3 << 16) | (flag4 << 24);
        }
        public void JumpTo(EventScript.ArgType argtype, uint value)
        {
            B40.Value = value & 0xff;
            B41.Value = (value >> 8) & 0xff;
            B42.Value = (value >> 16) & 0xff;
            B43.Value = (value >> 24) & 0xff;

            string dataname;
            if (argtype == EventScript.ArgType.RAM_UNIT_STATE)
            {
                this.Text = "RAM_UNIT_STATE";
                this.MESSAGE.Text = R._("RAM上のユニットの特性を指定してください。");
                dataname = "ramunit_state_checkbox_";
            }
            else if (argtype == EventScript.ArgType.UNITCLASSABILITY)
            {
                this.Text = "UNITCLASSABILITY";
                this.MESSAGE.Text = R._("チェックをつけた属性を利用します。");
                dataname = "unitclass_checkbox_";
            }
            else
            {
                return;
            }

            List<Control> controls = InputFormRef.GetAllControls(this);
            X_Tooltip = InputFormRef.GetToolTip<UwordBitFlagForm>();
            InputFormRef.LoadCheckboxesResource(dataname, controls, X_Tooltip, "", "L_40_BIT_", "L_41_BIT_", "L_42_BIT_", "L_43_BIT_");
        }

        ToolTipEx X_Tooltip;

        private void UwordBitFlagForm_Load(object sender, EventArgs e)
        {

        }
    }
}
