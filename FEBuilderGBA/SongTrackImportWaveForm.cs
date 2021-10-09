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
            U.AddCancelButton(this);

            this.HZ.SelectedIndex = 4;
            this.Strip.SelectedIndex = 1;
            this.Chunnel.SelectedIndex = 1;
            this.Volume.SelectedIndex = 9;
            this.UseDPCMCompress.SelectedIndex = 0;
            this.DPCMLookahead.SelectedIndex = 3 - 1;
            this.LoopComboBox.SelectedIndex = 0;
            PreviewResult.Text = R._("結果を見るには、Previewボタンを押してください。");
        }
        public void Dettach()
        {
            if (WavePlayer != null)
            {
                WavePlayer.Stop();
                WavePlayer.Dispose();
                WavePlayer = null;
            }
            DeleteTemp();
        }
        System.Media.SoundPlayer WavePlayer = null;
        string FromFilename = "";
        string TempFilename = "";
        string TempFilename2 = ""; //dpcm圧縮を再生するためのバッファ
        public void Init(string fromFilename)
        {
            this.FromFilename = fromFilename;
            this.TempFilename = "";
            this.TempFilename2 = "";
        }
        public bool UseLoop()
        {
            return (bool)(LoopComboBox.SelectedIndex == 1);
        }
        public string GetFromFilename()
        {
            if (File.Exists(this.FromFilename))
            {
                return this.FromFilename;
            }
            return this.FromFilename;
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
            }
            if (File.Exists(this.TempFilename2))
            {
                File.Delete(this.TempFilename2);
                this.TempFilename2 = "";
            }
        }
        

        private void OKButton_Click(object sender, EventArgs e)
        {
            RunSox();
            if (! File.Exists(this.TempFilename))
            {
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Preview_button_Click(object sender, EventArgs e)
        {
            Dettach();

            RunSox();
            if (!File.Exists(this.TempFilename))
            {
                PreviewResult.Text = R._("");
                return;
            }

            long fromFileSize = U.GetFileSize(this.FromFilename);
            long tempFileSize;
            string playTempWave;
            string ext = U.GetFilenameExt(this.TempFilename);
            if (ext == ".S")
            {
                byte[] dpcmBin;
                string error = SongUtil.LoadWavS(this.TempFilename, out dpcmBin);
                if (error != "")
                {
                    PreviewResult.Text = error;
                    return;
                }
                tempFileSize = dpcmBin.Length;

                byte[] uncmpWavBin = SongUtil.byteToWavForDPCM(dpcmBin, 0);
                this.TempFilename2 = U.mktemp(".wav");
                File.WriteAllBytes(this.TempFilename2, uncmpWavBin);
                playTempWave = this.TempFilename2;

                double snr = SongUtil.CalculateSNR(this.UncompressWavBIN, uncmpWavBin);
                string str = R._("ファイルサイズ {0} -> {1} ({2}%)\r\n圧縮品質 SNR: {3}dB (高いほど良い。20dB以下は圧縮を使わない方がいいかも。)", fromFileSize, tempFileSize,
                    Math.Round(tempFileSize * 100 / (double)fromFileSize, 2) , Math.Round(snr , 3));
                PreviewResult.Text = str;
            }
            else if (ext == ".DPCM")
            {
                byte[] dpcmBin = File.ReadAllBytes(this.TempFilename);
                tempFileSize = dpcmBin.Length;

                byte[] uncmpWavBin = SongUtil.byteToWavForDPCM(dpcmBin, 0);
                this.TempFilename2 = U.mktemp(".wav");
                File.WriteAllBytes(this.TempFilename2, uncmpWavBin);
                playTempWave = this.TempFilename2;

                double snr = SongUtil.CalculateSNR(this.UncompressWavBIN, uncmpWavBin);
                string str = R._("ファイルサイズ {0} -> {1} ({2}%)\r\n圧縮品質 SNR: {3}dB (高いほど良い。20dB以下は圧縮を使わない方がいいかも。)", fromFileSize, tempFileSize,
                    Math.Round(tempFileSize * 100 / (double)fromFileSize, 2), Math.Round(snr, 3));
                PreviewResult.Text = str;
            }
            else
            {
                playTempWave = this.TempFilename;
                tempFileSize = U.GetFileSize(this.TempFilename);

                string str = R._("ファイルサイズ {0} -> {1} ({2}%)", fromFileSize, tempFileSize,
                    Math.Round(tempFileSize * 100 / (double)fromFileSize, 2));
                PreviewResult.Text = str;
            }


            this.WavePlayer = new System.Media.SoundPlayer(playTempWave);
            this.WavePlayer.Play();
        }

        void RunSox()
        {
            if (File.Exists(this.TempFilename))
            {
                File.Delete(this.TempFilename);
            }
            bool r;
            string output;

            this.TempFilename = U.mktemp(".wav");
            if (Chunnel.SelectedIndex == 0
                && HZ.SelectedIndex == 0
                && Strip.SelectedIndex == 0
                && Volume.SelectedIndex == 0
                )
            {
                U.CopyFile(this.FromFilename, this.TempFilename);
            }
            else
            {
                //soxでwavファイルを変換
                r = MainFormUtil.ConvertWaveBySOX(this.TempFilename,
                    this.FromFilename,
                    U.atoi(Chunnel.Text),
                    U.atoi(HZ.Text),
                    U.atoi(Strip.Text),
                    U.atoi(Volume.Text),
                    out output)
                    ;
                if (r == false)
                {
                    R.ShowStopError(output);
                    return;
                }
                if (!File.Exists(this.FromFilename))
                {
                    return;
                }
                if (!File.Exists(this.TempFilename))
                {
                    return;
                }
            }

            if (this.UseDPCMCompress.SelectedIndex == 0)
            {//圧縮を使わない場合は、ここで終了
                return;
            }

            //圧縮する場合
            PatchUtil.Cache_m4a_hq_mixer m4a_hq_mixer = PatchUtil.Search_m4a_hq_mixer();
            if (m4a_hq_mixer == PatchUtil.Cache_m4a_hq_mixer.NO)
            {
                return;
            }
/*
            string sfile = U.mktemp(".s");
            r = MainFormUtil.ConvertWav2agb(sfile
                , this.TempFilename
                , U.atoi(this.DPCMLookahead.Text)
                , out output);
            if (r == false)
            {
                File.Delete(this.TempFilename);
                this.TempFilename = "";
                R.ShowStopError(output);
                return;
            }
*/
            //自前encode
            string sfile = U.mktemp(".dpcm");
            byte[] waveBin = File.ReadAllBytes(this.TempFilename);
            byte[] dpcmBin = SongUtilDPCM.wavToDPCMByte(waveBin, U.atoi(this.DPCMLookahead.Text));
            File.WriteAllBytes(sfile, dpcmBin);
            if (!File.Exists(sfile))
            {
                File.Delete(this.TempFilename);
                this.TempFilename = "";
                return;
            }
            this.UncompressWavBIN = File.ReadAllBytes(this.TempFilename);
            File.Delete(this.TempFilename);
            this.TempFilename = sfile;

            return;
        }
        byte[] UncompressWavBIN;

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
            PatchUtil.Cache_m4a_hq_mixer m4a_hq_mixer = PatchUtil.Search_m4a_hq_mixer();
            if (m4a_hq_mixer == PatchUtil.Cache_m4a_hq_mixer.NO)
            {
                this.NO_DPCM_PATCH.Show();
                UseDPCMCompress.Hide();
            }
            else
            {
                this.NO_DPCM_PATCH.Hide();
                UseDPCMCompress.Show();
            }
        }

        private void UseDPCMCompress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UseDPCMCompress.SelectedIndex == 0)
            {
                LABEL_DPCM_LOOKAHEAD.Hide();
                DPCMLookahead.Hide();
            }
            else
            {
                LABEL_DPCM_LOOKAHEAD.Show();
                DPCMLookahead.Show();
            }
        }
    }
}
