using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolRunHintMessageForm : Form
    {
        public ToolRunHintMessageForm()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (DoNotShowThisMessageAgain.Checked)
            {
                Program.Config["RunTestMessage"] = "1";
            }
            this.Close();
            MainFormUtil.RunAs("emulator");
        }

        public static void Run()
        {
            string emulator = Program.Config.at("emulator");
            if (emulator == "" || !File.Exists(emulator))
            {
                InputFormRef.JumpForm<ToolEmulatorSetupMessageForm>();
                return;
            }

            uint flag = U.atoi(Program.Config.at("RunTestMessage"));
            if (flag != 0)
            {
                MainFormUtil.RunAs("emulator");
                return;
            }

            InputFormRef.JumpForm<ToolRunHintMessageForm>();
        }
        public static void RemoveRunTestMenuIfUserWant(MenuStrip menu, ToolStripMenuItem targetMenuItem)
        {
            uint flag = U.atoi(Program.Config.at("RunTestMessage"));
            if (flag == 2)
            {
                menu.Items.Remove(targetMenuItem);
            }
        }

        private void ToolRunHintMessageForm_Load(object sender, EventArgs e)
        {
            Detail.Text +="\r\n"+MainFormUtil.GetCommunitiesURL();
            Detail.Select(0, 0); //全選択解除.
        }
    }
}
