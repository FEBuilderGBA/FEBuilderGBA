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
    public partial class UbyteBitFlagForm : Form
    {
        public UbyteBitFlagForm()
        {
            InitializeComponent();

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("", controls);
            U.AddCancelButton(this);
        }

        public Button GetApplyButton()
        {
            return ApplyButton;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            uint flag1 = (((uint)B40.Value) & 0xFF);

            ApplyButton.Tag = flag1;
        }
        public void JumpTo(EventScript.ArgType argtype, uint value)
        {
            B40.Value = value & 0xff;

            string dataname;
            if (argtype == EventScript.ArgType.AOECONFIG)
            {
                this.Text = "AOECONFIG";
                this.MESSAGE.Text = R._("範囲攻撃の攻撃タイプを設定します。");
                dataname = "AOECONFIG_checkbox_";
            }
            else if (argtype == EventScript.ArgType.CALLMENUCONFIG)
            {
                this.Text = "CALLMENUCONFIG";
                this.MESSAGE.Text = R._("詳細設定");
                dataname = "CALLMENUCONFIG_checkbox_";
            }
            else
            {
                return;
            }

            List<Control> controls = InputFormRef.GetAllControls(this);
            X_Tooltip = InputFormRef.GetToolTip<UbyteBitFlagForm>();
            InputFormRef.LoadCheckboxesResource(dataname, controls, X_Tooltip, "", "L_40_BIT_", "L_41_BIT_", "L_42_BIT_", "L_43_BIT_");
        }

        ToolTipEx X_Tooltip;

        private void UshortBitFlagForm_Load(object sender, EventArgs e)
        {

        }
    }
}
