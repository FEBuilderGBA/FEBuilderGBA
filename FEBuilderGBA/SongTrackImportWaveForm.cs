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
    public partial class SongTrackImportWaveForm : Form
    {
        public SongTrackImportWaveForm()
        {
            InitializeComponent();
            LoopComboBox.SelectedIndex = 0;
            U.AddCancelButton(this);
        }
        void PickupInstrument()
        {
        }

        public bool UseLoop()
        {
            return (bool)(LoopComboBox.SelectedIndex == 1);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void Init()
        {
        }

        private void SongTrackImportWaveForm_Load(object sender, EventArgs e)
        {

        }

    }
}
