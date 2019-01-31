using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class ToolDecompileResultForm : Form
    {
        public ToolDecompileResultForm()
        {
            InitializeComponent();
        }
        ToolTipEx ToolTip;

        private void DecompileResult_Load(object sender, EventArgs e)
        {
            this.ToolTip = InputFormRef.GetToolTip<DisASMForm>();
            //最大化を許可する.
            this.MaximizeBox = true;
            this.MainTab.NewTabEvent += (s, ee) =>
            {
                JumpTo(0x0, 0x0);
            };
        }

        string ConvertName(uint addr, out int out_ExistingTabIndex)
        {
            addr = U.toOffset(addr);

            string errormessage;
            string name;
            if (U.isSafetyOffset(addr))
            {
                addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
                string comment = Program.AsmMapFileAsmCache.GetASMName(U.toPointer(addr + 1), false, out errormessage);
                name = addr.ToString("X");
                if (comment.Length > 0)
                {
                    name = comment;
                }
            }
            else
            {
                name = "";
            }

            if (addr == 0)
            {//addr == 0 のタブだけは特別に何個も開ける.
                out_ExistingTabIndex = -1;
            }
            else
            {
                out_ExistingTabIndex = this.MainTab.FindTab(addr);
            }
            return name;
        }
        public void JumpTo(uint addr, uint size)
        {
            int existingTabIndex;
            string name = ConvertName(addr, out existingTabIndex);
            if (existingTabIndex >= 0)
            {//既存タブがある
                this.MainTab.SelectedIndex = existingTabIndex;
                return;
            }

            ToolDecompileResultInnerControl f = new ToolDecompileResultInnerControl();
            InputFormRef.InitControl(f, this.ToolTip);
            f.JumpTo(addr, size);

            this.MainTab.Add(name, f, addr);
            f.Navigation += OnNavigation;

            ToolDecompileResultForm_Resize(null, null);

            f.SetFocus();
        }
        void OnNavigation(object sender, EventArgs e)
        {
            if (!(e is ToolDecompileResultInnerControl.NavigationEventArgs))
            {
                return;
            }
            ToolDecompileResultInnerControl.NavigationEventArgs arg = (ToolDecompileResultInnerControl.NavigationEventArgs)e;
            if (arg.IsNewTab)
            {
                JumpTo(arg.Address , arg.Size);
            }
            else
            {
                int dummy;
                string name = ConvertName(arg.Address, out dummy);
                MainTab.UpdateTab(this.MainTab.SelectedIndex, name, arg.Address);
            }
        }

        private void ToolDecompileResultForm_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < MainTab.TabCount; i++)
            {
                TabPage tab = MainTab.TabPages[i];
                for (int n = 0; n < tab.Controls.Count; n++)
                {
                    Control c = tab.Controls[n];
                    if (c is ToolDecompileResultInnerControl)
                    {
                        c.Width = tab.Width;
                        c.Height = tab.Height;
                    }
                }
            }
        }
    }
}
