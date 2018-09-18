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
    public partial class PatchFilterExForm : Form
    {
        public PatchFilterExForm()
        {
            InitializeComponent();
            this.TagFilter = "";
            U.AddCancelButton(this);
        }

        public string TagFilter ;

        void Close(string filter)
        {
            this.TagFilter = filter;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Close("");
        }

        private void InstalledButton_Click(object sender, EventArgs e)
        {
            Close("!");
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            Close("#IMAGE ");
        }

        private void SoundButton_Click(object sender, EventArgs e)
        {
            Close("#SOUND ");
        }

        private void EngineButton_Click(object sender, EventArgs e)
        {
            Close("#ENGINE ");
        }

        private void EventButton_Click(object sender, EventArgs e)
        {
            Close("#EVENT ");
        }

        private void ESSENTIALFIXESButton_Click(object sender, EventArgs e)
        {
            Close("#ESSENTIALFIXES ");
        }

    }
}
