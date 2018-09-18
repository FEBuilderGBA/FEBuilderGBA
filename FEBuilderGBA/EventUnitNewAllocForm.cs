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
        }

        public uint AllocCount { get; private set; }
        private void AllocButton_Click(object sender, EventArgs e)
        {
            AllocCount = (uint)AllocCountNumupdown.Value;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
