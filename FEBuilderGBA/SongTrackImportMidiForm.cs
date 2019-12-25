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
    public partial class SongTrackImportMidiForm : Form
    {
        public SongTrackImportMidiForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }
        void PickupInstrument()
        {
            List<U.AddrResult> iset = PatchUtil.SearchInstrumentSet(this.OrignalInstrumentAddr);
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
        public bool GetIgnoreMOD()
        {
            return IgnoreMODCheckBox.Checked;
        }
        public bool GetIgnoreBEND()
        {
            return IgnoreBENDCheckBox.Checked;
        }
        public bool GetIgnoreLFOS()
        {
            return IgnoreLFOSCheckBox.Checked;
        }
        public bool GetIgnoreHEAD()
        {
            return IgnoreHEADCheckBox.Checked;
        }
        public bool GetIgnoreBACK()
        {
            return IgnoreBACKCheckBox.Checked;
        }
        public int GetMID2AGB_V()
        {
            return (int)Mid2agbV.Value;
        }
        public int GetMID2AGB_R()
        {
            return 0;
        }
        public ImportMethod GetUseMID2AGB()
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                return ImportMethod.FEBuilderGBA;
            }
            else
            {
                return ImportMethod.MID2AGB;
            }
        }

        public enum ImportMethod
        {
             FEBuilderGBA
            ,MID2AGB
        };
        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void Mid2AGBOKButton_Click(object sender, EventArgs e)
        {
            OKButton_Click(sender, e);
        }

        uint OrignalInstrumentAddr;
        public void Init(uint instrumentAddr)
        {
            this.OrignalInstrumentAddr = instrumentAddr;
            this.Instrument.Value = instrumentAddr;
            PickupInstrument();
        }


        private void InstrumentSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addr = U.atoh(InstrumentSelectComboBox.Text);
            if (addr > 0)
            {
                Instrument.Value = addr;
            }
        }

        bool HasNIMAP()
        {
            for (int i = 0; i < InstrumentSelectComboBox.Items.Count; i++)
            {
                string name = InstrumentSelectComboBox.Items[i].ToString();
                if (name.IndexOf("NatveInstrumentMap") >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void SongTrackImportMidiForm_Load(object sender, EventArgs e)
        {
            if (!HasNIMAP())
            {
                HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.NIMAP_By_SongTrack);
                PickupInstrument();
            }
            if (MainFormUtil.CanUseMID2AGB())
            {//mid2agbを利用できる.
                if (OptionForm.midi_importer() == OptionForm.midi_importer_enum.MID2AGB)
                {//mid2agbがディフォルト
                    this.tabControl1.SelectedIndex = 1;
                }
            }
            else
            {//mid2agbを利用できない.
                Explain_mid2agb.Text = R._("mid2agbが設定されていません。\r\n設定画面より、mid2agbの設定をしてください。");
                Mid2AGBOKButton.Enabled = false;
                this.tabControl1.SelectedIndex = 0;
            }
            Mid2agbV.Value = 80;
            //Mid2agbR.SelectedIndex = 0;
        }



    }
}
