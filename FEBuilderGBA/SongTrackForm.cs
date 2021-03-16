using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SongTrackForm : Form
    {
        public List<SongUtil.Track> Tracks = new List<SongUtil.Track>();
        ListBox[] TrackListBoxs;
        Label[] TrackLabels;
        U.FixDocsBugs fixDocsBugs;
        
        public SongTrackForm()
        {
            InitializeComponent();
            fixDocsBugs = new U.FixDocsBugs(this);

            this.TrackListBoxs = new ListBox[] { Track1, Track2, Track3, Track4, Track5, Track6, Track7, Track8, Track9, Track10, Track11, Track12, Track13, Track14, Track15, Track16 };
            this.TrackLabels = new Label[] { TrackLabel1, TrackLabel2, TrackLabel3, TrackLabel4, TrackLabel5, TrackLabel6, TrackLabel7, TrackLabel8, TrackLabel9, TrackLabel10, TrackLabel11, TrackLabel12, TrackLabel13, TrackLabel14, TrackLabel15, TrackLabel16 };

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので警告不可能
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            this.InputFormRef.CheckProtectionPaddingALIGN4 = false; //ALIGN 4である必要はない.

            for (int i = 0; i < this.TrackLabels.Length ; i++ )
            {
                this.TrackLabels[i].Click += TrackLabel_Click;
            }
            InputFormRef.markupJumpLabel(this.AllTracksLabel);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(SONGPLAY, Properties.Resources.icon_music);

            InputFormRef.markupJumpLabel(LinkInternt);

            U.AllowDropFilename(this, new string[] { ".S", ".MID", ".MIDI", ".WAV", ".INSTRUMENT" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });

        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , SongTableForm.GetSoundTablePointer()
                , 8
                , (int i, uint addr) =>
                {//読込最大値検索
                    return U.isPointer(Program.ROM.u32(addr));
                }
                , (int i, uint addr) =>
                {
                    uint   songheader = Program.ROM.p32(addr+0);
                    string name = U.ToHexString(i) + " " + SongTableForm.GetSongName((uint)i);
                    return new U.AddrResult(songheader, name);
                }
                );
        }

        private void SongTrackForm_Load(object sender, EventArgs e)
        {
            fixDocsBugs.AllowMaximizeBox();
        }


        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Tracks = SongUtil.ParseTrack((uint)Address.Value, (uint)B0.Value);

            for (int i = 0; i < this.TrackListBoxs.Length; i++)
            {
                if (Tracks.Count > i)
                {
                    SongUtil.TrackToListBox(this.TrackListBoxs[i], Tracks[i]);
                    InputFormRef.markupJumpLabel(this.TrackLabels[i]);
                }
                else
                {
                    this.TrackListBoxs[i].Items.Clear();
                    InputFormRef.unmarkupJumpLabel(this.TrackLabels[i]);
                }
            }

            if (this.Tracks.Count >= 12)
            {//このトラックは再生できますか？
                if (Program.ROM.RomInfo.version() >= 7 && Program.ROM.RomInfo.version() <= 8)
                {
                    HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.Track12_Over_By_SongTrack);
                }
            }
        }
        
        private void SongExchangeButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SongExchangeForm>((uint)this.AddressList.SelectedIndex);
        }
        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            if (!this.InputFormRef.CheckWriteProtectionID00())
            {
                return;
            }
            if (AddressList.SelectedIndex < 0)
            {
                return;
            }

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {
                string title = R._("インポートする音楽ファイルを選択してください");
                string filter = R._("sound|*.s;*.wav;*.mid;*.midi;*.instrument|s|*.s|midi|*.mid;*.midi|wav|*.wav|MusicalInstrument|*.instrument|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                Program.LastSelectedFilename.Load(this, "", open);
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                if (!U.CanReadFileRetry(open))
                {
                    return;
                }
                Program.LastSelectedFilename.Save(this, "", open);
                filename = open.FileNames[0];
            }

            uint songtable_address = InputFormRef.BaseAddress + (InputFormRef.BlockSize * (uint)AddressList.SelectedIndex);

            string error = "";

            string ext = U.GetFilenameExt(filename);
            if (ext == ".WAV" || ext == ".WAVE")
            {
                SongTrackImportWaveForm f = (SongTrackImportWaveForm)InputFormRef.JumpFormLow<SongTrackImportWaveForm>();
                f.Init(filename);
                DialogResult dr = f.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    f.Dettach();
                    return;
                }
                error = SongUtil.ImportWave(f.GetFilename(), songtable_address, f.UseLoop());
                f.Dettach();
            }
            else if (ext == ".MID" || ext == ".MIDI")
            {
                //楽器セットとオプションを選択してもらう.
                SongTrackImportMidiForm f = (SongTrackImportMidiForm)InputFormRef.JumpFormLow<SongTrackImportMidiForm>();
                f.Init((uint)P4.Value);
                DialogResult dr = f.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                //少し時間がかかるので、しばらくお待ちください表示.
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    if (f.UseMidfix4agb())
                    {
                        filename = SongUtil.ConvertMidfix4agb(filename);
                        if (filename == "")
                        {
                            return;
                        }
                    }

                    if (f.GetUseMID2AGB() == SongTrackImportMidiForm.ImportMethod.FEBuilderGBA)
                    {//FEBuilderGBAでimport
                        error = SongUtil.ImportMidiFile(filename, songtable_address
                            , f.GetInstrumentAddr()
                            , f.GetIgnoreMOD()
                            , f.GetIgnoreBEND()
                            , f.GetIgnoreLFOS()
                            , f.GetIgnoreHEAD()
                            , f.GetIgnoreBACK()
                            );
                    }
                    else
                    {//mid2agbでimport
                        error = SongUtil.ImportMidiFileMID2AGB(filename, songtable_address
                            , f.GetInstrumentAddr()
                            , f.GetMID2AGB_V()
                            , f.GetMID2AGB_R()
                            , f.GetIgnoreMOD()
                            , f.GetIgnoreBEND()
                            , f.GetIgnoreLFOS()
                            );
                    }
                }
            }
            else if (ext == ".INSTRUMENT")
            {
                //少し時間がかかるので、しばらくお待ちください表示.
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    error = SongUtil.ImportInstrument(filename, songtable_address);
                }
            }
            else
            {
                //楽器セットを選択してもらう.
                SongTrackImportSelectInstrumentForm f = (SongTrackImportSelectInstrumentForm)InputFormRef.JumpFormLow<SongTrackImportSelectInstrumentForm>();
                f.Init((uint)P4.Value);
                DialogResult dr = f.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                //少し時間がかかるので、しばらくお待ちください表示.
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    error = SongUtil.ImportS(filename, songtable_address, f.GetInstrumentAddr());
                }
            }
            

            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }

            int selectedIndex = AddressList.SelectedIndex;
            ReloadListButton.PerformClick();
            AddressList.SelectedIndex = selectedIndex;

            SongTableForm.ReloadList();
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }
        private void SONGPLAY_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAsSappy((uint)AddressList.SelectedIndex);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("sound|*.s;*.mid|s|*.s|midi|*.mid|MusicalInstrument|*.instrument|SondFont|*.sf2|All files|*");

            string songname = "song" + U.ToHexString(AddressList.SelectedIndex);

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, songname);

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);
            string filename = save.FileNames[0];

            int NumBlks = (int)B1.Value;
            int Priority = (int)B2.Value;
            int Reverb = (int)B3.Value;
            uint instrument_addr = (uint)P4.Value;

            string ext = U.GetFilenameExt(filename);

            if (ext == ".MID" || ext == ".MIDI")
            {
                if (SongUtil.UseGBAMusRiper())
                {
                    bool r = SongUtil.ExportMidiFileByGBAMusRiper(filename, (uint)this.Address.Value);
                    if (!r)
                    {//代用する
                        SongUtil.ExportMidiFile(filename, songname
                            , Tracks, NumBlks, Priority, Reverb, instrument_addr);
                    }
                }
                else
                {
                    SongUtil.ExportMidiFile(filename, songname
                        , Tracks, NumBlks, Priority, Reverb, instrument_addr);
                }
            }
            else if (ext == ".SF2")
            {
                SongUtil.ExportSoundFontByGBAMusRiper(filename, (uint)this.Address.Value);
            }
            else if (ext == ".INSTRUMENT")
            {
                //少し時間がかかるので、しばらくお待ちください表示.
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    SongUtil.ExportInstrument(filename, instrument_addr);
                }
            }
            else
            {
                SongUtil.ExportSFile(filename, songname
                    , Tracks, NumBlks, Priority, Reverb, instrument_addr);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }


        private void TrackLabel_Click(object sender, EventArgs e)
        {
            if (!(sender is Label))
            {
                return;
            }
            Label la = (Label)sender;
            string noString = U.substr(la.Name,10);
            int no = (int)U.atoi(noString);
            if (no <= 0)
            {
                return;
            }

            no = no - 1;
            if (no >= Tracks.Count)
            {
                return;
            }

            SongTrackChangeTrackForm f = (SongTrackChangeTrackForm) InputFormRef.JumpFormLow<SongTrackChangeTrackForm>();
            f.Init( (uint)P4.Value, this.Tracks[no] );
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            SongUtil.ChangeTrackAndWrite(this.Tracks[no]
                , f.GetChangeVoices()
                , f.GetChangeVol()
                , f.GetChangePan()
                , 0
                , f.IsChangeVelocity()
                );
            this.WriteButton.PerformClick();

            U.ReSelectList(this.AddressList);
        }

        private void AllTracksLabel_Click(object sender, EventArgs e)
        {
            SongTrackAllChangeTrackForm f = (SongTrackAllChangeTrackForm)InputFormRef.JumpFormLow<SongTrackAllChangeTrackForm>();
            f.Init((uint)P4.Value, this.Tracks);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            for (int i = 0; i < this.Tracks.Count; i++)
            {
                SongUtil.ChangeTrackAndWrite(this.Tracks[i]
                    , f.GetChangeVoices()
                    , f.GetChangeVol()
                    , f.GetChangePan()
                    , f.GetChangeTempo()
                    , false
                    );
            }
            this.WriteButton.PerformClick();

            U.ReSelectList(this.AddressList);
        }

        private void SongTrackForm_Resize(object sender, EventArgs e)
        {
            Track1.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel1.Height - 10;
            Track2.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel2.Height - 10;
            Track3.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel3.Height - 10;
            Track4.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel4.Height - 10;
            Track5.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel5.Height - 10;
            Track6.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel6.Height - 10;
            Track7.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel7.Height - 10;
            Track8.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel8.Height - 10;
            Track9.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel9.Height - 10;
            Track10.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel10.Height - 10;
            Track11.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel11.Height - 10;
            Track12.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel12.Height - 10;
            Track13.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel13.Height - 10;
            Track14.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel14.Height - 10;
            Track15.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel15.Height - 10;
            Track16.Height = TracksPanel.Height - AllTracksLabel.Height - TrackLabel16.Height - 10;
        }

        private void LinkInternt_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoMoreData();
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors, uint i, uint songaddr)
        {
            if (songaddr == 0)
            {
                return;
            }
            if (! U.isSafetyOffset(songaddr))
            {
                return;
            }
            uint track = Program.ROM.u8(songaddr + 0);
            if (i == 0)
            {
                if (track != 0)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.SONGTRACK, songaddr
                        , R._("SongID {0}のトラックは常に0である必要があります。現在値:{1}", U.To0xHexString(i), U.To0xHexString(track)), i));
                }
                return;
            }
            if (track > 16)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SONGTRACK, songaddr
                    , R._("SongID {0}のトラックは常に16以内である必要があります。現在値:{1}", U.To0xHexString(i), U.To0xHexString(track)), i));
            }
            if (track == 0)
            {//トラック数が0のダミートラックの場合、チェックしない
                return;
            }

            //楽器のチェック
            uint instPointer = Program.ROM.u32(songaddr + 4);
            if (!U.isSafetyPointer(instPointer))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(FELint.Type.SONGTRACK, U.toOffset(songaddr)
                    , R._("SongID {0}の楽器ポインタ「{1}」は無効です。", U.To0xHexString(i), U.To0xHexString(instPointer)), i));
                return;
            }

            //トラックのポインタチェック
            for (uint n = 0; n < track; n++)
            {
                uint trackPointer = Program.ROM.u32(songaddr + 4 + (n * 4) );
                if (!U.isSafetyPointer(trackPointer))
                {//無効なポインタ
                    errors.Add(new FELint.ErrorSt(FELint.Type.SONGTRACK, U.toOffset(songaddr)
                        , R._("SongID {0}のトラック{1}のポインタ「{2}」は無効です。\r\nトラック数が間違っていませんか？", U.To0xHexString(i), n, U.To0xHexString(trackPointer)), i));
                }
            }
        }


    }
}
