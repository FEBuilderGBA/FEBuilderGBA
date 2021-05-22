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
    public partial class UshortBitFlagForm : Form
    {
        public UshortBitFlagForm()
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

            ApplyButton.Tag = flag1 | (flag2 << 8) ;
        }
        public void JumpTo(EventScript.ArgType argtype, uint value)
        {
            B40.Value = value & 0xff;
            B41.Value = (value >> 8) & 0xff;

            string dataname;
            if (argtype == EventScript.ArgType.DISABLEOPTIONS)
            {
                this.Text = "DISABLEOPTIONS";
                this.MESSAGE.Text = R._("チェックをつけたメニューを無効にします。");
                dataname = "DISABLEOPTIONS_checkbox_";
            }
            else if (argtype == EventScript.ArgType.DISABLEWEAPONS)
            {
                this.Text = "DISABLEWEAPONS";
                this.MESSAGE.Text = R._("チェックをつけたメニューを無効にします。");
                dataname = "DISABLEWEAPONS_checkbox_";
            }
            else if (argtype == EventScript.ArgType.IGNORE_KEYS)
            {
                this.Text = "IGNORE_KEYS";
                this.MESSAGE.Text = R._("チェックをつけたキーを無効にします。");
                dataname = "IGNORE_KEYS_checkbox_";
            }
            else if (argtype == EventScript.ArgType.KEYS)
            {
                this.Text = "KEYS";
                this.MESSAGE.Text = R._("チェックをつけたキーコードを返します。");
                dataname = "IGNORE_KEYS_checkbox_";
            }
            else if (argtype == EventScript.ArgType.ATTACK_TYPE)
            {
                this.Text = "ATTACK_TYPE";
                this.MESSAGE.Text = R._("チェックをつけた攻撃を行います。");
                dataname = "EVENTBATTLE_checkbox_";
            }
            else
            {
                return;
            }

            List<Control> controls = InputFormRef.GetAllControls(this);
            X_Tooltip = InputFormRef.GetToolTip<UshortBitFlagForm>();
            InputFormRef.LoadCheckboxesResource(dataname, controls, X_Tooltip, "", "L_40_BIT_", "L_41_BIT_", "L_42_BIT_", "L_43_BIT_");
        }

        ToolTipEx X_Tooltip;

        private void UshortBitFlagForm_Load(object sender, EventArgs e)
        {

        }
    }
}
