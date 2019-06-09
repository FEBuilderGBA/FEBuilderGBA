using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventUnitNewAllocForm : Form
    {
        public EventUnitNewAllocForm()
        {
            InitializeComponent();

//            if (InputFormRef.SearchSkillSystem() == InputFormRef.skill_system_enum.SkillSystem)
//            {//SkillSystemsがインストールされている場合、同時にロードできるユニット数は16体
//                AllocCountNumupdown.Maximum = 16;
//            }
        }

        public uint AllocCount { get; private set; }
        private void AllocButton_Click(object sender, EventArgs e)
        {
            AllocCount = (uint)AllocCountNumupdown.Value;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void EventUnitNewAllocForm_Load(object sender, EventArgs e)
        {

        }
    }
}
