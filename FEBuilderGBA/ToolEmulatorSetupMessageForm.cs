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
    public partial class ToolEmulatorSetupMessageForm : Form
    {
        public ToolEmulatorSetupMessageForm()
        {
            InitializeComponent();
        }

        private void UseInitWizardButton_Click(object sender, EventArgs e)
        {
            this.Close();
            InputFormRef.JumpForm<ToolInitWizardForm>();
        }

        private void UseOptionManualButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            InputFormRef.JumpForm<OptionForm>();
        }

    }
}
