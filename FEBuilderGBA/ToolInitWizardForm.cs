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
    public partial class ToolInitWizardForm : Form
    {
        public ToolInitWizardForm()
        {
            InitializeComponent();
        }

        private void ToolInitWizardForm_Load(object sender, EventArgs e)
        {
            this.LangComboBox.Items.Add("ja=日本語");  ///No Translate
            this.LangComboBox.Items.Add("en=English"); ///No Translate
            this.LangComboBox.Items.Add("zh=中文");    ///No Translate
        }

        static bool IsShowWizard()
        {
            string emulator = Program.Config.at("emulator");
            return (emulator == "") ;
        }
    }
}
