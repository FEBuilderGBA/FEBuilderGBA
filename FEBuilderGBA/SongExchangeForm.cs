using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace FEBuilderGBA
{
    public partial class SongExchangeForm : Form
    {
        public SongExchangeForm()
        {
            InitializeComponent();

            this.MySongList = SongTableToSongList(Program.ROM.Data);
            SongListToListBox(this.MySongList, this.SongTable, true);

            this.SongTable.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
            this.OtherROMSongTable.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
        }
        public class SongSt
        {
            public uint number;  //ソングテーブルの位置 songtable[number];
            public uint table;   //選択されたソングテーブルのデータ table=songtable[number]   table[0:4] =header  table[4:4]=フラグ sizeof(8)
            public uint header;  //曲ヘッダ header[4:4] = voices  header[0:1] = tracks
            public uint voices;  //楽器データ
            public uint tracks;  //楽譜データ            
        }

        List<SongSt> MySongList;
        List<SongSt> OtherSongList;
        byte[] OtherROMData;
        string OtherROMFilename;
        string ErrorMessage;

        public static int CommandLineImport()
        {
            U.echo("CommandLineImport");
            string fromrom = U.at(Program.ArgsDic, "--fromrom");
            if (fromrom == "")
            {
                fromrom = U.at(Program.ArgsDic, "--target");
            }
            if (!File.Exists(fromrom))
            {
                U.echo("曲を取り出すROMを「--fromrom」で指定してください。");
                return -2;
            }

            SongExchangeForm f = (SongExchangeForm)InputFormRef.JumpFormLow<SongExchangeForm>();
            f.LoadSongListAtOtherROM(fromrom);

            uint tosong = U.atoi0x(U.at(Program.ArgsDic, "--tosong"));
            if (tosong <= 0)
            {
                U.echo("曲をインポートする場所を「--tosong」で指定してください。");
                return -2;
            }
            U.SelectedIndexSafety(f.SongTable, tosong);

            uint fromsong = U.atoi0x(U.at(Program.ArgsDic, "--fromsong"));
            if (fromsong <= 0)
            {
                U.echo("曲をエクスポートする場所を「--fromsong」で指定してください。");
                return -2;
            }
            U.SelectedIndexSafety(f.OtherROMSongTable, fromsong);

            f.ConvertButton_Click(null, null);

            //インポートに成功したら保存する.
            if (Program.ROM.Modified)
            {
                MainFormUtil.SaveForce(f);
            }
            else
            {
                U.echo("曲のインポートに失敗しました。");
                return -1;
            }

            return 0;
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {
                R.ShowStopError(InputFormRef.GetBusyErrorExplain());
                return;
            }

            if (OtherSongList == null)
            {
                R.Error("相手のROMが読み込まれていません");
                return;
            }
            if (OtherROMSongTable.SelectedIndex < 0 || OtherROMSongTable.SelectedIndex > OtherSongList.Count)
            {
                R.Error("エクスポートする曲がリストから選択されていません");
                return;
            }
            if (SongTable.SelectedIndex < 0 || SongTable.SelectedIndex > MySongList.Count)
            {
                R.Error("インポートする曲がリストから選択されていません.");
                return;
            }
            if (SongTable.SelectedIndex == 0)
            {
                R.ShowStopError("SongID: 0x0に書き込むことはできません");
                return;
            }
            {
                DialogResult dr = R.ShowYesNo("この音楽({0})を、現在のROMの({1})に、移植してもよろしいですか？", OtherROMSongTable.Text, SongTable.Text);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            ConvertSong(OtherROMData, OtherSongList[OtherROMSongTable.SelectedIndex], Program.ROM.Data, MySongList[SongTable.SelectedIndex]);
            R.Notify("曲のインポートが完了しました。");

            //変更があったので、データを取り直して描画しなおす.
            int nowselect = SongTable.SelectedIndex;
            this.MySongList = SongTableToSongList(Program.ROM.Data);
            SongListToListBox(this.MySongList, this.SongTable, true);
            SongTable.SelectedIndex = nowselect;

            SongTableForm.ReloadList();
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        private void OpenOtherROMButton_Click(object sender, EventArgs e)
        {
            string title = R._("編集するROMを選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            open.ShowDialog();
            if (!U.CanReadFileRetry(open))
            {
                return ;
            }
            Program.LastSelectedFilename.Save(this, "", open);
            LoadSongListAtOtherROM(open.FileNames[0]);
        }
        void LoadSongListAtOtherROM(string filename)
        {
            this.OtherROMFilename = filename;
            this.OtherROMData = File.ReadAllBytes(filename);

            this.OtherSongList = SongTableToSongList(this.OtherROMData);
            SongListToListBox(this.OtherSongList, this.OtherROMSongTable, false);
        }

        private void SongExchangeForm_Load(object sender, EventArgs e)
        {

        }


        static uint FindSongTable(byte[] data)
        {
            uint songpointer = SongUtil.FindSongTablePointer(data);
            if (songpointer == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }

            uint songlist = U.u32(data,songpointer);
            if (!U.isPointer(songlist))
            {
                return U.NOT_FOUND;
            }
            return U.toOffset(songlist);
        }


        List<SongSt> SongTableToSongList(byte[] data)
        {
            List<SongSt> sonlist = new List<SongSt>();

            uint songlist = FindSongTable(data);
            if (songlist == U.NOT_FOUND)
            {
                return sonlist;
            }
//            songlist += 8;
            for (int i = 0; true; i++, songlist += 8)
            {
                uint header = U.u32(data, songlist);
                if (i != 0 && !U.isPointer(header))
                {
                    break;
                }

                header = U.toOffset(header);
                uint voices = U.p32(data, header+4);
                uint tracks = U.u8(data, header);

                SongSt song = new SongSt();
                song.number = (uint)i;
                song.table = songlist;
                song.header = header;
                song.voices = voices;
                song.tracks = tracks;
                sonlist.Add(song);
            }

            return sonlist;
        }

        void SongListToListBox(List<SongSt> songTable, ListBox list, bool isSourceROM)
        {
            list.BeginUpdate();
            list.Items.Clear();
            for (int i = 0; i < songTable.Count; i++)
            {
                string a = U.ToHexString(songTable[i].number);
                if (isSourceROM)
                {
                    a += U.SA(SongTableForm.GetSongNameFast(songTable[i].number, songTable[i].table)); 
                }
                a = a + " Table: " + U.To0xHexString(songTable[i].table) + " Header: " + U.To0xHexString(songTable[i].header) + " Voices: " + U.To0xHexString(songTable[i].voices) + " Tracks: " + U.To0xHexString(songTable[i].tracks);
                list.Items.Add(a);
            }
            list.EndUpdate();
        }
        void ConvertSong(byte[] srcdata, SongSt srcsong, byte[] destdata, SongSt destsong)
        {
            InstrumentMap instrument_map = new InstrumentMap(srcdata, srcsong.voices);
            List<List<byte>> trackdata = new List<List<byte>>();

            if (instrument_map.ErrorMessage != "")
            {
                DialogResult dr = R.ShowNoYes("この曲のデータ構造は壊れているようです。\r\nインポートを強行しますか？\r\n強行する場合は、正しく認識できたトラックのみをインポートします。" + "\r\n\r\n" + instrument_map.ErrorMessage);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            //曲の取出し.
            this.ErrorMessage = "";
            bool success = Rip(srcdata, srcsong, instrument_map, trackdata);
            if (success == false)
            {
                R.ShowStopError("この曲のデータ構造は壊れているため、Rip出来ませんでした。" + "\r\n\r\n" + this.ErrorMessage);
                return;
            }

            //取り出した曲を書き込む.
            Burn(destsong, instrument_map, trackdata);
        }

        void Burn( SongSt song, InstrumentMap instrument_map, List<List<byte>> trackdata)
        {
            //必要なサイズを計算する.
            uint use_size = 8 + (4 * (uint)trackdata.Count); //ヘッダー
            for (int track = 0; track < trackdata.Count; track++)
            {
                use_size += (uint)trackdata[track].Count; //楽譜
            }
            use_size  = U.Padding4(use_size); //楽譜と楽器の間は 4バイトアライメントが必要.
            use_size += (uint)(instrument_map.Instrument_mapping.Count * 12); //楽器
            use_size += (uint)instrument_map.Sample_data.Count;      //楽器データ

            uint write_pointer = InputFormRef.AllocBinaryData(use_size);
            if (write_pointer == U.NOT_FOUND)
            {
                R.ShowStopError("データサイズを({0})確保できません。\r\nROM容量がもうないか、音楽のパースに失敗しています。",use_size);
                return;
            }
            U.toPointer(write_pointer);

            byte[] data = new byte[use_size];
            U.write_u8(data, 0, (uint)trackdata.Count );//トラック数
            U.write_u8(data, 1, 0x0);  //常にゼロ.
            U.write_u8(data, 2, 0x0A); //Do these values matter?
            U.write_u8(data, 3, 0x80); //This is just copying what the stock ROM does...
            uint offset = 8 + (4 * (uint)trackdata.Count);

            //データ構造
            //ヘッダー
            //[track数] [0] [0x0A] [0x80] [楽器ポンタ] [楽譜1ポインタ] [楽譜2ポインタ].... [楽譜Nポインタ]
            //
            //実データ
            //[楽譜1データ].......
            //[楽譜2データ].......
            //
            //[楽器データ]
            //[楽器サンプルデータ]

            //楽譜
            for (int track = 0; track < trackdata.Count; track++)
            {
                //楽譜ポインタの書き込み
                U.write_u32(data, 8 + (4 * (uint)track), U.toPointer(write_pointer + offset)); 

                //楽譜データを書き込む.
                burn_track(data, offset, write_pointer, trackdata[track].ToArray() );
                offset += (uint)trackdata[track].Count;
            }
            offset = U.Padding4(offset); //楽譜と楽器の間は 4バイトアライメントが必要.

            //楽器ポインタ
            U.write_u32(data, 4, U.toPointer(write_pointer + offset)); 

            uint instrument_start = offset; //楽器開始
            uint instrumentdata_start = instrument_start + (12 * (uint)instrument_map.Instrument_mapping.Count); //楽器データ開始
            Log.Debug("instrumentdata_start(", instrumentdata_start.ToString() , ") = instrument_start(", instrument_start.ToString(), ") + 12 * instrument_count(", instrument_map.Instrument_mapping.Count.ToString(), ")");



            U.write_range(data,instrument_start,instrument_map.Instrument_codes.ToArray() );
            U.write_range(data, instrumentdata_start, instrument_map.Sample_data.ToArray());

            //楽器
            uint resyclesize = 0;
            for (int i = 0; i < instrument_map.Instrument_mapping.Count; i++)
            {
                uint this_instrument = instrument_start + (12 * (uint)i);

                Log.Debug("track:", i.ToString(), " ", data[this_instrument + 0].ToString());
                uint instrumentCode = U.u8(data,this_instrument+0) ;
                if (SongInstrumentForm.IsDirectSound(instrumentCode) 
                    || SongInstrumentForm.IsWaveMemory(instrumentCode))
                {
                    uint sample_data_start = U.u32(data, this_instrument + 4);
                    sample_data_start += instrumentdata_start;
                    if (sample_data_start < resyclesize)
                    {
                        Log.Error("BAD INSTRUMENT:", i.ToString(), (sample_data_start - instrumentdata_start).ToString(), sample_data_start.ToString(), resyclesize.ToString());
                        continue;
                    }
                    sample_data_start -= resyclesize;
                    uint sample_data_len;
                    if (SongInstrumentForm.IsWaveMemory(instrumentCode))
                    {
        				sample_data_len = 16;
                    }
                    else
                    {
				        sample_data_len = U.u32(data,sample_data_start+12);
                        sample_data_len = U.Padding4(sample_data_len);
                    }
                    Log.Debug("d ", sample_data_start.ToString("X"), " ", sample_data_len.ToString());

			        uint found_address = U.Grep(Program.ROM.Data, U.subrange(data,sample_data_start,sample_data_start+sample_data_len),100,0,4);
        			if ( found_address != U.NOT_FOUND )
                    {
                        Log.Debug("recycle ", sample_data_start.ToString("X"), " len ", sample_data_len.ToString(), " -> ", found_address.ToString("X"));
				        //existing address in ROM.
				        //recycle
                        data = U.del(data,sample_data_start,sample_data_start+sample_data_len);
                        U.write_u32(data,this_instrument+4, U.toPointer(found_address) );

				        resyclesize += sample_data_len;
                    }
                    else
                    {
				        //nothing to recycle, write the data.
                        uint baseoffset = U.u32(data, this_instrument + 4); //相対アドレスが書いてあるので、それを求めに絶対値に変換する
                        U.write_u32(data, this_instrument + 4
                            , U.toPointer((instrumentdata_start + write_pointer + baseoffset) - resyclesize));
                    }

                }
                else if (instrumentCode == 0x80)
                {//ドラム
                    uint baseoffset = U.u32(data, this_instrument + 4); //相対アドレスが書いてあるので、それを求めに絶対値に変換する
                    U.write_u32(data, this_instrument + 4
                        , U.toPointer(instrument_start + write_pointer+baseoffset));
                }
                else if (instrumentCode == 0x40)
                {
                    Log.Debug("MULTI TRACK!");
                    uint baseoffset = U.u32(data, this_instrument + 4); //相対アドレスが書いてあるので、それを求めに絶対値に変換する
                    U.write_u32(data, this_instrument + 4
                        , U.toPointer(instrument_start + write_pointer + baseoffset));


                    baseoffset = U.u32(data, this_instrument + 8); //相対アドレスが書いてあるので、それを求めに絶対値に変換する
                    U.write_u32(data, this_instrument + 8
                        , U.toPointer((instrumentdata_start + write_pointer + baseoffset) - resyclesize));
                }
            }
            Log.Notify("resyclesize:", resyclesize.ToString(), U.To0xHexString(resyclesize));

            string undo_name = "";
            Undo.UndoData undodata = Program.Undo.NewUndoData(undo_name);
            undodata.list.Add(new Undo.UndoPostion(song.table, 8));

            Program.ROM.write_u32(song.table, U.toPointer(write_pointer));
            InputFormRef.WriteBinaryDataDirect(write_pointer, data, undodata);

            uint priority = GetSongPriority(trackdata.Count);
            Program.ROM.write_u32(song.table + 4, priority, undodata);

            Program.Undo.Push(undodata);
        }

        uint GetSongPriority(int trackdata)
        {
            if (trackdata <= 1)
            {//SFX?
                return 0x60006;
            }
            else
            {//MAP
                return 0x10001;
            }
        }

        void burn_track(byte[] data,uint offset,uint write_pointer ,byte[] trackdata)
        {
            for (int i = 0; i < trackdata.Length; )
            {
                byte b = trackdata[i];
                data[i + offset] = b;
                i++;
                if (b == 0xB2 || b == 0xB3)
                {
                    uint p = U.u32(trackdata, (uint)i);
                    U.write_u32(data, (uint)(i + offset), U.toPointer(p + offset + write_pointer));
                    i += 4;
                }
            }
        }


        bool Rip(byte[] data, SongSt song, InstrumentMap instrument_map, List<List<byte>> trackdata)
        {
            //Rip
            for (uint track = 0; track < song.tracks; track++)
            {
                uint songtrack_pointer = song.header + 8 + (4 * track);
                uint songtrackdata_pointer = U.u32(data, songtrack_pointer);
                if (!U.isPointer(songtrackdata_pointer))
                {
                    this.ErrorMessage += "\r\n" +
                        R.Error("track:" + track.ToString() + " can not pointer! addr:" + songtrack_pointer.ToString("X08") + " data:" + songtrackdata_pointer.ToString("X08"));
                    return false;
                }
                songtrackdata_pointer = U.toOffset(songtrackdata_pointer);

                List<byte> track_data = process_track(data, songtrackdata_pointer, instrument_map);

                trackdata.Add(track_data);
            }
            //ドラムがない曲の場合、それは困るので、ダミーのドラムを追加する.
            instrument_map.appendDrumIfNoDrum();
            return true;
        }

        List<byte> process_track(byte[] data, uint songtrackdata_pointer, InstrumentMap instrument_map)
        {
            List<byte> ret = new List<byte>();

            //Transform the data.
            uint position = songtrackdata_pointer;
            uint percussion = 0;
            while(true)
            {
                uint b = U.u8(data,position);
                position ++;
                ret.Add((byte)b);

                if (b == 0xB1)
                {
                    break;
                }
                else if (b == 0xB2 || b == 0xB3)
                {
			        //repointer
                    U.append_u32(ret, U.p32(data, position) - songtrackdata_pointer);
			        position += 4;
                }
                else if (b == 0xBD)
                {
                    uint next_byte = U.u8(data,position);
                    position ++;
                    
                    uint translated = instrument_map.translate(next_byte);
                    if (translated == 0)
                    {
                        percussion = next_byte;
                    }
                    ret.Add((byte)translated);
                }
                else if (b == 0xBB || b == 0xBC || b == 0xBE || b == 0xBF || b == 0xC0 || b == 0xC1 || b == 0xC2 || b == 0xC3 || b == 0xC4 || b == 0xC5 || b == 0xC8)
                {
        			// These commands take a data byte that must not be processed.
                    ret.Add( (byte) U.u8(data,position) );
                    position++;
                }
                else if (b == 0xb9)
                {//MEMACC 4バイト命令
                    //最初の1バイトはコピー済みなので、残りの3バイトコピーする.
                    ret.Add((byte)U.u8(data, position));
                    position++;
                    ret.Add((byte)U.u8(data, position));
                    position++;
                    ret.Add((byte)U.u8(data, position));
                    position++;
                }
                else if (percussion != 0 && b < 0x80)
                {
                    uint inst = instrument_map.translate_percussion(percussion, b);
                    ret[ret.Count - 1] = ((byte)inst);
                    //There might be a volume marker, and then a 'gate' byte
                    //For now, assuming that any subsequent low-value bytes are extra data
                    //that should be passed as-is - even though previous experimentation suggested
                    //that these bytes could be used to specify a chord...
                    while (U.u8(data, position) < 0x80)
                    {// Volume marker
                        ret.Add((byte)U.u8(data, position));
                        position++;
                    }
                }
            }
            return ret;
        }

    
        public class InstrumentMap
        {
            public List<byte> Instrument_codes;
            public List<byte> Sample_data;

            public Dictionary<string, uint> Instrument_mapping;
            byte[] Data;
            uint Instrument_map_offset;

            public string ErrorMessage;

            //Maps non-percussion instruments to sequential values 1..n,
            //and extracts the corresponding samples. If percussion is
            //encountered, all percussion maps are re-assigned ID 0,
            //and the mapped percussion samples are assigned sequential
            //values in the same mapping.
            public InstrumentMap(byte[] data,uint instrument_map_pointer)
            {
                this.ErrorMessage = "";
                this.Instrument_codes = new List<byte>();
                this.Sample_data = new List<byte>();
                this.Instrument_mapping = new Dictionary<string, uint>();

                this.Instrument_codes.AddRange(new byte[]{0x80,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00});
                this.Data = data;
                this.Instrument_map_offset = U.toOffset(instrument_map_pointer);
            }
            public uint Count()
            {
                uint bytecount = (uint)this.Instrument_codes.Count;
                Debug.Assert(bytecount % 12 == 0);
                
                uint result = bytecount / 12;
                Debug.Assert(result < 0x80); //allocated too many instruments?
                return result;
            }
            public void appendDrumIfNoDrum()
            {
                foreach (var pair in this.Instrument_mapping)
                {
                    if (pair.Key.Length <= 0)
                    {
                        continue;
                    }
                    if (pair.Key[0] == 'D')
                    {//ドラム発見
                        return;
                    }
                }
                //ドラムが見つからない!
                //ドラムは必ず0にマッピングするのでないと困る.
                //そのためダミーのドラムを追加する.
                this.Instrument_mapping["Drum_Dummy"] = 0;
            }

            public byte[] get_instrument(uint index, uint baseindex = 0)
            {
                if (baseindex == 0)
                {
                    baseindex = this.Instrument_map_offset;
                }
                baseindex += 12 * index;

                return U.subrange(this.Data,baseindex,baseindex+12);
            }

            public uint translate(uint original_id)
            {
                string original_id_str = original_id.ToString();
                if ( this.Instrument_mapping.ContainsKey(original_id_str) )
                {
                    return this.Instrument_mapping[original_id_str];
                }
                byte[] instrument_code = this.get_instrument( original_id );
                return this._prepare(instrument_code, original_id.ToString() , false);
            }

            //for drum
            public uint translate_percussion(uint original_id, uint pitch)
            {
                string key = "D" +"_"+ original_id.ToString() + "_" + pitch.ToString();
                if ( this.Instrument_mapping.ContainsKey(key) )
                {
                    Log.Debug("translate_percussion", original_id.ToString(), pitch.ToString(), key.ToString(), "found!");
                    return this.Instrument_mapping[key];
                }

                Log.Debug("translate_percussion", original_id.ToString(), pitch.ToString(), key.ToString(), "notfound!");
                byte[] instrument = this.get_instrument(original_id);
                uint instrument_offset = U.p32(instrument,4);

                byte[] instrument_code = this.get_instrument(pitch,instrument_offset );
                return this._prepare(instrument_code, key , true);
            }

            //for MultiSample
            public uint translate_multisample(uint original_id, uint multivoices)
            {
                string key = "M" +"_"+ original_id.ToString() + "_" + multivoices.ToString();
                if ( this.Instrument_mapping.ContainsKey(key) )
                {
                    return this.Instrument_mapping[key];
                }

                byte[] instrument_code = this.get_instrument(original_id,multivoices);
                return this._prepare(instrument_code, key , true);
            }

            byte[] bad_inst()
            {
                return  new byte[]{
                    0x04,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
            }

            bool _prepare_DirectSound(byte[] instrument_code, string key, bool is_deps)
            {
                Debug.Assert(SongInstrumentForm.IsDirectSound(instrument_code[0]));
                uint sample_location = U.p32(instrument_code, 4);
                if (sample_location > this.Data.Length)
                {
                    this.ErrorMessage += "\r\n" + 
                        R.Error("DirectSoundの中に、おかしなデータがありました。無視します。 sample_location:{0} > {1} ROM Size"
                        , U.To0xHexString(sample_location), U.To0xHexString(this.Data.Length));

                    //ダメな楽器として認識する.
                    return false;
                }
                uint sample_hz1024 = U.u32(this.Data, sample_location + 4) / 1024;
                uint sample_length = U.u32(this.Data, sample_location + 12);
                Log.Debug(R._("DirectSound Sample:{0} bytes ({1} *1024 hz)", sample_length, sample_hz1024));

                if (is_deps)
                {
                    if (sample_length > 1024*1024 * 1   //1MB
                        || sample_hz1024 > 48 * 1024    //48khz Over
                        )
                    {
                        this.ErrorMessage += "\r\n" +
                            R.Error("Multi または Drumの中に、おかしなデータがありました。無視します。 OverHZ Sample:{0} bytes ({1} *1024 hz)", sample_length, sample_hz1024);

                        //ダメな楽器として認識する.
                        return false;
                    }
                }

                List<byte> current_sample = U.subrangeToList(this.Data, sample_location, sample_location + 16 + sample_length);
                //4バイトアライメント
                while ((current_sample.Count % 4) != 0)
                {
                    current_sample.Add(0);
                }

                Log.Debug(Instrument_mapping.Count.ToString(), sample_length.ToString("X"), this.Sample_data.Count.ToString("X"));

                U.write_u32(instrument_code, 4, (uint)this.Sample_data.Count);
                this.Sample_data.AddRange(current_sample);
                Log.Debug(R._("SampleData:{0} bytes (append({1}bytes))", this.Sample_data.Count, current_sample.Count));

                return true;
            }
            bool _prepare_WaveMemory(byte[] instrument_code, string key, bool is_deps)
            {
                Debug.Assert(SongInstrumentForm.IsWaveMemory(instrument_code[0]));
                uint sample_location = U.p32(instrument_code,4);
                if (sample_location > this.Data.Length)
                {
                    this.ErrorMessage += "\r\n" +
                        R.Error("DirectSoundの中に、おかしなデータがありました。無視します。 sample_location:{0} > {1} ROM Size"
                            , U.To0xHexString(sample_location),U.To0xHexString(this.Data.Length));

                    //ダメな楽器として認識する.
                    return false;
                }
           
                List<byte> current_sample = U.subrangeToList(this.Data,sample_location ,sample_location + 16 );

                //4バイトアライメント
                while ((current_sample.Count % 4) != 0)
                {
                    current_sample.Add(0);
                }

                Log.Debug(U.HexDump(instrument_code));
                    
                U.write_u32(instrument_code,4,(uint)this.Sample_data.Count);
                this.Sample_data.AddRange(current_sample);
                Log.Debug(R._("SampleData:{0} bytes (append({1}bytes))", this.Sample_data.Count, current_sample.Count));

                return true;
            }

            bool _prepare_MultiSample(byte[] instrument_code, string key, bool is_deps)
            {
                Log.Debug("multisample");

                uint multisample_voices = U.p32(instrument_code, 4);
                uint sample_location = U.p32(instrument_code, 8);
                if (multisample_voices > this.Data.Length)
                {
                    this.ErrorMessage += "\r\n" +
                        R.Error("MultiSampleの中に、おかしなデータがありました。無視します。 multisample_voices:{0} sample_location:{1} > {2} ROM Size"
                            , U.To0xHexString(multisample_voices), U.To0xHexString(sample_location), U.To0xHexString(this.Data.Length));

                    //ダメな楽器として認識する.
                    return false;
                }
                if (sample_location > this.Data.Length)
                {
                    this.ErrorMessage += "\r\n" +
                        R.Error("MultiSampleの中に、おかしなデータがありました。無視します。 multisample_voices:{0} sample_location:{1} > {2} ROM Size"
                            , U.To0xHexString(multisample_voices), U.To0xHexString(sample_location), U.To0xHexString(this.Data.Length));

                    //ダメな楽器として認識する.
                    return false;
                }

                List<byte> current_sample = U.subrangeToList(this.Data, sample_location, sample_location + 128);

                Dictionary<int, uint> dic = new Dictionary<int, uint>();
                for (int i = 0; i < current_sample.Count; i++)
                {
                    int id = current_sample[i];
                    if (id > 0x7F)
                    {
                        continue;
                    }
                    if (!dic.ContainsKey(id))
                    {
                        dic[id] = this.translate_multisample((uint)id, multisample_voices);

                    }
                    current_sample[i] = (byte)dic[id];
                }

                U.write_u32(instrument_code, 4, 0); //follow instrument start posstion
                U.write_u32(instrument_code, 8, (uint)this.Sample_data.Count);
                this.Sample_data.AddRange(current_sample);

                Log.Debug(U.HexDump(instrument_code));
                Log.Debug(U.HexDump(current_sample));
                Log.Debug(R._("SampleData:{0} bytes (append({1}bytes))", this.Sample_data.Count, current_sample.Count));

                return true;
            }
            uint _prepare(byte[] instrument_code, string key, bool is_deps)
            {
                // Fix instrument pointer to be an offset relative to start of sample data.
                // The pointer for the first instrument - which is the percussion map - is
                // of course relative to the start of instrument data, being zero. The
                // burn procedure is aware of this.
                if (is_deps && (instrument_code[0] == 0x80 || instrument_code[0] == 0x40))
                {
                    //print "song in song error!"
                    //print hexdump(instrument_code)
                    Log.Error(U.HexDump(instrument_code));
                    this.ErrorMessage += "\r\n" +
                        R.Error("Multi または Drumの中に、さらにMulti または Drumがありました。\r\nこういう複雑なものは対応できないので無視します。\r\n");

                    instrument_code = bad_inst();
                    Debug.Assert(false);
                }
                else if (SongInstrumentForm.IsDirectSound(instrument_code[0]))
                {
                    bool success = _prepare_DirectSound(instrument_code, key, is_deps);
                    if (success == false)
                    {
                        instrument_code = bad_inst();
                    }
                }
                else if (SongInstrumentForm.IsWaveMemory(instrument_code[0]))
                {
                    bool success = _prepare_WaveMemory(instrument_code, key, is_deps);
                    if (success == false)
                    {
                        instrument_code = bad_inst();
                    }
                }
                else if (instrument_code[0] == 0x80)
                {
                    Log.Debug("Drum");
                    Log.Debug(U.HexDump(instrument_code));
                    
                    //drum instrument is always id:0
			        this.Instrument_mapping[key] = 0;
			        return 0;
                }
                else if (instrument_code[0] == 0x40)
                {
                    bool success = _prepare_MultiSample(instrument_code, key, is_deps);
                    if (success == false)
                    {
                        instrument_code = bad_inst();
                    }
                }
                else
                {
                    Log.Debug("???");
                    Log.Debug(U.HexDump(instrument_code));
                }
                Debug.Assert(instrument_code.Length >= 0xC);

                uint result = this.Count();
                this.Instrument_mapping[key] = result;
                this.Instrument_codes.AddRange(instrument_code);
                return result;
            }
        };

        private void SongTable_DoubleClick(object sender, EventArgs e)
        {
            uint song_id = (uint)(SongTable.SelectedIndex);
            if (song_id <= 0)
            {
                return;
            }
            MainFormUtil.RunAsSappy(song_id);
        }

        private void OtherROMSongTable_MouseDoubleClick(object sender, MouseEventArgs ee)
        {
            uint song_id = (uint)(OtherROMSongTable.SelectedIndex);
            if (song_id <= 0)
            {
                return;
            }

            Process p;
            try
            {
                p = MainFormUtil.PoolRunAs("sappy", this.OtherROMFilename);
            }
            catch (Exception e)
            {
                R.ShowStopError("Sappyプロセスを実行できません。\r\n{0}",  e.ToString());
                return;
            }
            if (p == null)
            {
                return;
            }

            SappyPlaying sappy = new SappyPlaying();
            sappy.StartPlay(p, song_id);
        }

    }
}
