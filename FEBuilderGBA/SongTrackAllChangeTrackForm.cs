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
    public partial class SongTrackAllChangeTrackForm : Form
    {
        public SongTrackAllChangeTrackForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        List<U.AddrResult> SongInstrumentList;
        List<SongUtil.ChangeVoiceSt> ChangeVoices;
        public void Init(uint instaddr,List<SongUtil.Track> tracks)
        {
            this.SongInstrumentList = SongInstrumentForm.MakeList(instaddr);

            this.ChangeVoices = new List<SongUtil.ChangeVoiceSt>();
            for (int i = 0; i < tracks.Count; i++)
            {
                SongUtil.GetVoices(tracks[i],ref this.ChangeVoices);
            }
            this.VolNumericUpDown.Value = 0;
            this.PanNumericUpDown.Value = 0;

            this.VoiceListbox.BeginUpdate();
            for (int i = 0; i < this.ChangeVoices.Count; i++ )
            {
                string v = GetVoiceName(this.ChangeVoices[i]);
                this.VoiceListbox.Items.Add(v);
            }
            this.VoiceListbox.EndUpdate();
        }
        string GetVoiceName(SongUtil.ChangeVoiceSt v)
        {
            return R._("楽器 {0}({1}) -> {2}({3}) #{4}"
                , v.from, GetVoiceName(v.from)
                , v.to, GetVoiceName(v.to)
                , U.To0xHexString(v.to)
                );
        }
        string GetVoiceName(int voice)
        {
            if (voice < 0 || voice >= this.SongInstrumentList.Count)
            {
                return "";
            }
            string name = this.SongInstrumentList[voice].name;
            return U.cut(name, " ", " (").TrimEnd();
        }

        public List<SongUtil.ChangeVoiceSt> GetChangeVoices()
        {
            return this.ChangeVoices;
        }
        public int GetChangeVol()
        {
            return (int)this.VolNumericUpDown.Value;
        }
        public int GetChangePan()
        {
            return (int)this.PanNumericUpDown.Value;
        }
        public int GetChangeTempo()
        {
            return (int)this.TempoUpDown1.Value;
        }

        private void VoiceListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = this.VoiceListbox.SelectedIndex;
            if (sel < 0 || sel >= this.ChangeVoices.Count)
            {
                return;
            }
            this.VoiceNumericUpDown.Value = this.ChangeVoices[sel].to;
        }

        private void VoiceNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            int sel = this.VoiceListbox.SelectedIndex;
            if (sel < 0 || sel >= this.ChangeVoices.Count)
            {
                return;
            }
            this.ChangeVoices[sel].to = (int)this.VoiceNumericUpDown.Value;

            string v = GetVoiceName(this.ChangeVoices[sel]);
            this.VoiceListbox.Items[sel] = v;
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void SongTrackAllChangeTrackForm_Load(object sender, EventArgs e)
        {

        }

    }
}
