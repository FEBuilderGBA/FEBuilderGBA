using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolAnimationCreatorForm : Form
    {
        public ToolAnimationCreatorForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon_film;
        }

        ToolTipEx ToolTip;
        private void ToolAnimationCreatorForm_Load(object sender, EventArgs e)
        {
            this.ToolTip = InputFormRef.GetToolTip<ToolAnimationCreatorForm>();
            //最大化を許可する.
            this.MaximizeBox = true;

            this.MainTab.NewTabEvent += (s, ee) =>
            {
            };
            this.ToolAnimationCreatorForm_Resize(null, null);
        }

        public static uint MakeUniqID(ToolAnimationCreatorUserControl.AnimationTypeEnum type
            , uint id
           )
        {
            uint uniq = (((uint)type) << 24) + id;
            return uniq;
        }

        public void Init(
              ToolAnimationCreatorUserControl.AnimationTypeEnum type
            , uint id
            , string filehint
            , string filename)
        {
            uint uniq = MakeUniqID(type,id);
            int existingTabIndex = this.MainTab.FindTab(uniq);
            if (existingTabIndex >= 0)
            {//既存タブがある
                this.MainTab.SelectedIndex = existingTabIndex;
                return;
            }

            ToolAnimationCreatorUserControl f = new ToolAnimationCreatorUserControl();
            InputFormRef.InitControl(f, this.ToolTip);
            f.Init(type,id,filehint,filename);

            this.MainTab.Add(filehint, f, uniq);
            this.ToolAnimationCreatorForm_Resize(null, null);

            f.SetFocus();
        }


        private void ToolAnimationCreatorForm_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < MainTab.TabCount; i++)
            {
                TabPage tab = MainTab.TabPages[i];
                for (int n = 0; n < tab.Controls.Count; n++)
                {
                    Control c = tab.Controls[n];
                    if (c is ToolAnimationCreatorUserControl)
                    {
                        c.Width = tab.Width;
                        c.Height = tab.Height;
                    }
                }
            }
        }

        private void ToolAnimationCreatorForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

    }
}
