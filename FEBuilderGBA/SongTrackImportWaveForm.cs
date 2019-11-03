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

            this.HZ.SelectedIndex = 1;
            this.Strip.SelectedIndex = 1;
            this.Chunnel.SelectedIndex = 1;
            PreviewResult.Text = R._("結果を見るには、Previewボタンを押してください。");
        }
        public new void Dispose()
        {
            DeleteTemp();
            base.Dispose();
        }
        string FromFilename = "";
        string TempFilename = "";
        public void Init(string fromFilename)
        {
            this.FromFilename = fromFilename;
            this.TempFilename = "";
        }
        public bool UseLoop()
        {
            return (bool)(LoopComboBox.SelectedIndex == 1);
        }
        public string GetFilename()
        {
            if (File.Exists(this.TempFilename))
            {
                return this.TempFilename;
            }
            return this.FromFilename;
        }
        void DeleteTemp()
        {
            if (File.Exists(this.TempFilename))
            {
                File.Delete(this.TempFilename);
                this.TempFilename = "";
                return ;
            }
        }
        

        private void OKButton_Click(object sender, EventArgs e)
        {
            RunSox();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Preview_button_Click(object sender, EventArgs e)
        {
            bool r = RunSox();
            if (!r)
            {
                PreviewResult.Text = R._("");
                return;
            }

            long fromFileSize = U.GetFileSize(this.FromFilename);
            long tempFileSize = U.GetFileSize(this.TempFilename);
            string str = R._("ファイルサイズ {0} -> {1} ({2}%)", fromFileSize, tempFileSize, 
                Math.Round(tempFileSize * 100 / (double)fromFileSize,2));
            PreviewResult.Text = str;

            System.Media.SoundPlayer player = new System.Media.SoundPlayer(this.TempFilename);
            player.Play();
        }
        bool RunSox()

        {
            if (Chunnel.SelectedIndex == 0
                && HZ.SelectedIndex == 0
                && Strip.SelectedIndex == 0
                )
            {
                return false;
            }

            if (this.TempFilename == "")
            {
                this.TempFilename = U.mktemp(".wav");
            }
            //soxでwavファイルを変換
            string output;
            bool r = MainFormUtil.ConvertWaveBySOX(this.TempFilename,
                this.FromFilename,
                U.atoi(Chunnel.Text),
                U.atoi(HZ.Text),
                U.atoi(Strip.Text),
                out output)
                ;
            if (r == false)
            {
                R.ShowStopError(output);
                return false;
            }
            if (!File.Exists(this.FromFilename))
            {
                return false;
            }
            if (!File.Exists(this.TempFilename))
            {
                return false;
            }

            return true;
        }

        private void PreviewResult_DoubleClick(object sender, EventArgs e)
        {
            if (this.TempFilename == "")
            {
                return;
            }
            U.SelectFileByExplorer(this.TempFilename);
        }

        private void SongTrackImportWaveForm_Load(object sender, EventArgs e)
        {

        }


    }
}
