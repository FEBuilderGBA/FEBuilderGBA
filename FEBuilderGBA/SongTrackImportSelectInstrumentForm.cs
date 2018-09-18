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
    public partial class SongTrackImportSelectInstrumentForm : Form
    {
        public SongTrackImportSelectInstrumentForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }
        void PickupInstrument()
        {
            List<U.AddrResult> iset = SongUtil.SearchInstrumentSet(U.ConfigDataFilename("song_instrumentset_"));
            U.ConvertComboBox(iset, ref InstrumentSelectComboBox);

            if (iset.Count >= 1)
            {
                //NIMAPが利用できるならディフォルト指定
                U.SelectedIndexSafety(InstrumentSelectComboBox, 1);
            }
        }

        public uint GetInstrumentAddr()
        {
            return (uint)Instrument.Value;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void Init(uint instrumentAddr)
        {
            this.Instrument.Value = instrumentAddr;
            PickupInstrument();
        }

        private void SongTrackImportSelectInstrumentForm_Load(object sender, EventArgs e)
        {
        }


        private void InstrumentSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addr = U.atoh(InstrumentSelectComboBox.Text);
            if (addr > 0)
            {
                Instrument.Value = addr;
            }
        }
    }
}
