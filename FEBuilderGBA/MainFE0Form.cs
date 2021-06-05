using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MainFE0Form : Form
    {
        public MainFE0Form()
        {
            InitializeComponent();
            FixedButton();
            ToolRunHintMessageForm.RemoveRunTestMenuIfUserWant(this.menuStrip1, this.TestRunStripMenuItem);
            InputFormRef.RecolorMenuStrip(this.menuStrip1);
            MainFormUtil.MakeExplainFunctions(this.ControlPanel);
        }

        private void SongTableButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SongTableForm>();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainFormUtil.IsNotSaveYet(this))
            {
                return;
            }
            MainFormUtil.Open(this);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.SaveOverraide(this);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.SaveAs(this);
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.Quit(this);
        }

        private void RunAsEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("emulator");
        }

        private void RunAsDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("emulator2");
        }

        private void RunAsBinaryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("binary_editor");
        }

        private void GraphicsToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("graphics_tool");
        }

        private void RunAsSappyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("sappy");
        }

        private void RunAsProgram1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program1");
        }

        private void RunAsProgram2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program2");
        }

        private void RunAsProgram3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program3");
        }

        private void RunAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs(this);
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUndoForm>();
        }

        private void MoveAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MoveToFreeSapceForm>();
        }

        private void PatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<PatchForm>();
        }

        private void GraphicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<GraphicsToolForm>();
        }

        private void LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<LogForm>();
        }

        private void SongImportOtherROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SongExchangeForm>();
        }

        private void PointerToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<PointerToolForm>();
        }

        private void SettingOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OptionForm>();
        }

        private void SettingVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<VersionForm>();
        }

        private void DisassemblerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>();
            f.JumpTo(0x0);
        }

        private void DisassemblerButton_Click(object sender, EventArgs e)
        {
            DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>();
            f.JumpTo(0x0);
        }
        private void LZ77ToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolLZ77Form>();
        }

        private void MainFE0Form_FormClosing(object sender, FormClosingEventArgs e)
        {
#if DEBUG
            //デバッグ時はいちいち聞かれると面倒なのでスルーする
#else
            if (! MainFormUtil.IsNotSaveYet(this))
            {
                e.Cancel = true;
            }
#endif
        }

        private void diffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolDiffForm>();
        }

        private void eventAssemblerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventAssemblerForm>();
        }

        private void OpenLastUsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OpenLastSelectedFileForm>();
        }

        private void HexEditorButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<HexEditorForm>();
        }

        private void DecreaseColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
        }

        private void OnlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoManual();
        }

        private void UPSSimpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUPSPatchSimpleForm>();
        }

        private void OAMSPButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OAMSPForm>();
        }

        void FixedButton()
        {
#if DEBUG
//            ROMRebuildButton.Show();
#else
//            ROMRebuildButton.Hide();
#endif
        }

        private void Filter_KeyUp(object sender, KeyEventArgs e)
        {
            MainFormUtil.ApplySearchFilter(Filter.Text, this.ControlPanel, null, null);
            FixedButton();
        }

        private void Filter_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Filter.Clear();
            Filter_KeyUp(null, null);
        }

        private void FilterLabel_Click(object sender, EventArgs e)
        {
            Filter_MouseDoubleClick(null, null);
        }

        private void MainFE6Form_Shown(object sender, EventArgs e)
        {
            this.Filter.Focus();
        }

        private void WelcomeDialogButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<WelcomeForm>();
        }

        private void ASMInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolASMInsertForm>();
        }

        private void DiscordURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoCommunities();
        }

        private void InitWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunToolInitWizard();
        }

        private void ChangeProjectNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolChangeProjectnameForm>();
        }


        private void TestRunStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolRunHintMessageForm.Run();
        }

        private void MainFE0Form_Load(object sender, EventArgs e)
        {

        }


    }
}
