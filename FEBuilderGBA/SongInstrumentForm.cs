using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class SongInstrumentForm : Form
    {
        public SongInstrumentForm()
        {
            InitializeComponent();

            N00_L_1_COMBO.Items.Clear();
            N08_L_1_COMBO.Items.Clear();
            N10_L_1_COMBO.Items.Clear();
            N00_L_1_COMBO.BeginUpdate();
            N08_L_1_COMBO.BeginUpdate();
            N10_L_1_COMBO.BeginUpdate();
            for (uint i = 0; i < 128; i++)
            {
                string name = U.ToHexString(i) +"="+ SongUtil.getKeyCode(i);
                N00_L_1_COMBO.Items.Add(name);
                N08_L_1_COMBO.Items.Add(name);
                N10_L_1_COMBO.Items.Add(name);
            }
            N00_L_1_COMBO.EndUpdate();
            N08_L_1_COMBO.EndUpdate();
            N10_L_1_COMBO.EndUpdate();

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(N00_ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(N00_ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(N08_ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(N08_ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(N10_ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(N10_ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(N18_ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(N18_ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(Inst_ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(Inst_ExportButton, Properties.Resources.icon_arrow);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            if (self != null ||InstrumentHint == null)
            {
                PreLoadResourceInstrumentHint(U.ConfigDataFilename("song_instrument_"));
            }

            return new InputFormRef(self
                , ""
                , new List<String> { "N00_" ,"N01_", "N02_", "N03_", "N04_", "N08_", "N09_", "N0A_", "N0B_", "N0C_", "N10_", "N40_", "N80_" }
                , 0
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (i >= 128)
                    {//最大数over!
                        return false;
                    }

                    uint type = Program.ROM.u8(addr + 0);
                    if (type == 0x00    //directosound
                        || type == 0x08 //directosound
                        || type == 0x10 //directosound
                        || type == 0x18 //directosound
                        || type == 0x03 //wave
                        || type == 0x0B //wave
                        || type == 0x80 //drum
                        )
                    {
                        uint p = Program.ROM.u32(addr + 4);
                        if (! U.isSafetyPointer(p))
                        {//不正なアドレス
                            return false;
                        }
                        return true;
                    }
                    if (type == 0x40) //multisamples
                    {
                        uint p = Program.ROM.u32(addr + 4);
                        if (!U.isSafetyPointer(p))
                        {//不正なアドレス
                            return false;
                        }
                        p = Program.ROM.u32(addr + 8);
                        if (!U.isSafetyPointer(p))
                        {//不正なアドレス
                            return false;
                        }
                        return true;
                    }

                    if (type == 0x01 //Square Wave(Without Data)
                        || type == 0x02 //Square Wave(Without Data)
                        || type == 0x03 //Square Wave(Without Data)
                        || type == 0x04 //Noise(Without Data)
                        || type == 0x09 //Square Wave(Without Data)
                        || type == 0x0A //Square Wave(Without Data)
                        || type == 0x0C //Square Wave(Without Data)
                        )
                    {
                        return true;
                    }

                    return false;
                }
                , (int i, uint addr) =>
                {
                    string hint2 = "(" + i.ToString() + ", " + SongUtil.getKeyCode((uint)i) + ")";

                    string fingerprint = SongInstrumentForm.FingerPrint(addr);
                    if (fingerprint != "")
                    {
                        InstrumentHintSt hint;
                        if (InstrumentHint.TryGetValue(fingerprint, out hint))
                        {
                            return U.ToHexString(i) + " " + hint.name + " " + hint2;
                        }
                    }
                    uint type = Program.ROM.u8(addr);
                    return U.ToHexString(i) + " " + GetInstrumentTypeName(type) + " " + hint2;
                }
                );
        }



        private void SongVocaForm_Load(object sender, EventArgs e)
        {
        }
        public void JumpToAddr(uint addr)
        {
            InputFormRef.ReInit(addr);
        }
        public void JumpToSongID(uint song_id)
        {
            uint table = SongTableForm.GetSongAddr(song_id);
            if (table == U.NOT_FOUND)
            {
                return;
            }
            uint songheader = Program.ROM.p32(table);
            if (!U.isSafetyOffset(songheader))
            {
                return;
            }
            uint voca = Program.ROM.p32(songheader + 4);
            if (!U.isSafetyOffset(voca))
            {
                return;
            }
            InputFormRef.ReInit(voca);
        }

        public static void RecycleOldInstrument(ref List<Address> recycle, string basename, uint voca_basepointer)
        {
            if (!U.isSafetyOffset(voca_basepointer))
            {
                return ;
            }

            uint voca_baseaddress = Program.ROM.u32(voca_basepointer);
            if (!U.isPointer(voca_baseaddress))
            {
                return ;
            }
            voca_baseaddress = U.toOffset(voca_baseaddress);
            if (!U.isSafetyOffset(voca_baseaddress))
            {
                return ;
            }

            //既に記録しているならば無視.
            for (int i = 0; i < recycle.Count; i++)
            {
                if (recycle[i].Addr == voca_baseaddress)
                {
                    return;
                }
            }

            //楽器リスト本体
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(voca_basepointer);
            FEBuilderGBA.Address.AddAddress(recycle
                , InputFormRef
                , basename
                , new uint[] { 4, 8 }
                );

            uint voca_endaddress = voca_baseaddress + ((InputFormRef.DataCount+1) * InputFormRef.BlockSize);
            uint addr = voca_baseaddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint type = Program.ROM.u8(addr);
                if (type == 0x00 
                    || type == 0x08
                    || type == 0x10
                    || type == 0x18
                )
                {//directsound waveデータ.
                    uint songdata_addr = Program.ROM.p32(addr + 4);
                    if (!U.isSafetyOffset(songdata_addr))
                    {
                        continue;
                    }
                    uint sample_length = Program.ROM.u32(songdata_addr + 12);
                    if (!U.isSafetyLength(songdata_addr + 12 + 4, sample_length))
                    {//壊れたデータ 長さが取れない
                        FEBuilderGBA.Address.AddPointer(recycle
                            , addr + 4
                            , 0
                            , basename + U.To0xHexString(i) + "DIRECTSOUND(BROKEN)"
                            , FEBuilderGBA.Address.DataTypeEnum.SONGINSTDIRECTSOUND);
                        continue;
                    }
                    if (!SongUtil.IsDirectSoundData(Program.ROM.Data, songdata_addr))
                    {//壊れたデータ 
                        FEBuilderGBA.Address.AddPointer(recycle
                            , addr + 4
                            , 0
                            , basename + U.To0xHexString(i) + "DIRECTSOUND(BROKEN)"
                            , FEBuilderGBA.Address.DataTypeEnum.SONGINSTDIRECTSOUND);
                        continue;
                    }

                    FEBuilderGBA.Address.AddPointer(recycle
                        , addr + 4
                        , 12 + 4 + sample_length
                        , basename + U.To0xHexString(i) + "DIRECTSOUND"
                        , FEBuilderGBA.Address.DataTypeEnum.SONGINSTDIRECTSOUND);
                }
                else if (type == 0x03
                    || type == 0x0B
                    )
                {//波形データ
                    uint songdata_addr = Program.ROM.p32(addr + 4);
                    if (!U.isSafetyOffset(songdata_addr))
                    {
                        continue;
                    }
                    FEBuilderGBA.Address.AddPointer(recycle
                        , addr + 4
                        , 16
                        , basename + U.To0xHexString(i) + "WAVE"
                        , FEBuilderGBA.Address.DataTypeEnum.SONGINSTWAVE);
                }
                else if (type == 0x80)
                {//ドラム
                    uint drum_voices = Program.ROM.p32(addr + 4);
                    if (!U.isSafetyOffset(drum_voices))
                    {
                        continue;
                    }

                    RecycleOldInstrument(ref recycle
                        , basename + U.To0xHexString(i) + "DRUM "
                        , addr + 4);
                }
                else if (type == 0x40)
                {//マルチサンプル
                    uint multisample_voices = Program.ROM.p32(addr + 4);
                    uint sample_location = Program.ROM.p32(addr + 8);
                    if (!U.isSafetyOffset(multisample_voices))
                    {
                        continue;
                    }
                    if (!U.isSafetyOffset(sample_location))
                    {
                        continue;
                    }

                    RecycleOldInstrument(ref recycle
                        , basename + U.To0xHexString(i) + "MULTI "
                        , addr + 4);

                    FEBuilderGBA.Address.AddPointer(recycle
                        , addr + 8
                        , 128
                        , basename + U.To0xHexString(i) + "MULTI2"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
            }
        }
        //direct sound +P4 のソングデータを使って比較用のフィンガプリントを作ります.
        static string FingerPrint(uint vocaaddr)
        {
            if (!U.isSafetyOffset(vocaaddr))
            {
                return "";
            }

            uint type = Program.ROM.u8(vocaaddr + 0);
            if (type == 0x00
                || type == 0x08
                || type == 0x10
                || type == 0x18)
            {//directsound
                return DirectoSoundFingerPrint(vocaaddr);
            }
            else if (type == 0x03
                || type == 0x0B)
            {//wavememory
                return "WW"+WaveMemoryFingerPrint(vocaaddr);
            }
            else if (type == 0x01
                || type == 0x02
                || type == 0x09
                || type == 0x0A
                )
            {//squarewave
                return "SQ"+SquareWaveFingerPrint(vocaaddr);
            }
            else if (type == 0x04
                )
            {//Noise
                return "NZ"+SquareWaveFingerPrint(vocaaddr);
            }

            //ドラムとマルチセッションはネストするので作りようがない.作ってもほぼ無意味
            return "";
        }

        //direct sound +P4 のソングデータを使って比較用のフィンガプリントを作ります.
        static string DirectoSoundFingerPrint(uint vocaaddr)
        {
            uint type = Program.ROM.u8(vocaaddr + 0);
            Debug.Assert(type == 0x00
                || type == 0x08
                || type == 0x10
                || type == 0x18);

            List<byte> data = new List<byte>();
            //U.append_u8(data, Program.ROM.u8(vocaaddr + 0) ); //type切り替えだから入れてはいけない
            U.append_u8(data, Program.ROM.u8(vocaaddr + 1));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 2));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 3));
            // 4 5 6 7 はwaveへのポインタ
            U.append_u8(data, Program.ROM.u8(vocaaddr + 8));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 9));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 10));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 11));

            uint songdata_addr = Program.ROM.p32(vocaaddr + 4);
            if (!U.isSafetyOffset(songdata_addr))
            {
                return "";
            }

            uint length = Program.ROM.u32(songdata_addr + 12);
            if (length == U.NOT_FOUND)
            {
                return "";
            }
            if (!U.isSafetyLength(songdata_addr + 12 + 4, length))
            {
                return "";
            }

            byte[] wavedata = Program.ROM.getBinaryData(songdata_addr + 12, length);
            data.AddRange(wavedata);

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(data.ToArray());
            md5.Clear();

            System.Text.StringBuilder result = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

        //WaveMemory +P4 のソングデータを使って比較用のフィンガプリントを作ります.
        static string WaveMemoryFingerPrint(uint vocaaddr)
        {
            uint type = Program.ROM.u8(vocaaddr + 0);
            Debug.Assert(type == 0x03
                || type == 0x0B);


            List<byte> data = new List<byte>();
            //U.append_u8(data, Program.ROM.u8(vocaaddr + 0) ); //type切り替えだから入れてはいけない
            U.append_u8(data, Program.ROM.u8(vocaaddr + 1));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 2));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 3));
            // 4 5 6 7 はwaveへのポインタ
            U.append_u8(data, Program.ROM.u8(vocaaddr + 8));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 9));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 10));
            U.append_u8(data, Program.ROM.u8(vocaaddr + 11));

            uint songdata_addr = Program.ROM.p32(vocaaddr + 4);
            if (!U.isSafetyOffset(songdata_addr))
            {
                return "";
            }

            //12バイト固定のデータがある.
            if (! U.isSafetyOffset(songdata_addr + 12))
            {
                return "";
            }

            byte[] fixeddata = Program.ROM.getBinaryData(songdata_addr + 0, 12);
            data.AddRange(fixeddata);

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(data.ToArray());
            md5.Clear();

            System.Text.StringBuilder result = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

        //SquareWaveのデータ使って比較用のフィンガプリントを作ります.
        static string SquareWaveFingerPrint(uint vocaaddr)
        {
            //U.append_u8(data, Program.ROM.u8(vocaaddr + 0) ); //type切り替えだから入れてはいけない

            byte[] data = Program.ROM.getBinaryData(vocaaddr + 1, 11);

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(data);
            md5.Clear();

            System.Text.StringBuilder result = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fingerPrint = FingerPrint(InputFormRef.SelectToAddr(AddressList));
            this.FINGERPRINT.Text = fingerPrint;
        }

        public class InstrumentHintSt
        {
            public uint midi_mapping;
            public string name;
        };
        public static Dictionary<string, InstrumentHintSt> InstrumentHint;

        public static void PreLoadResourceInstrumentHint(string fullfilename)
        {
            InstrumentHint = new Dictionary<string, InstrumentHintSt>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return ;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }

                    string[] sp = line.Split('\t');
                    if (sp.Length < 3)
                    {
                        continue;
                    }

                    InstrumentHintSt p = new InstrumentHintSt();
                    if (sp[0] == "")
                    {
                        p.midi_mapping = U.NOT_FOUND;
                    }
                    else
                    {
                        p.midi_mapping = U.atoi(sp[0]);
                    }
                    p.name = sp[1];
                    string key = sp[2];
                    InstrumentHint[key] = p;
                }
            }
        }

        public static string GetInstrumentTypeName(uint type)
        {
            if (type == 0x00) return R._("DirectSound");
            if (type == 0x01) return R._("SquareWave1");
            if (type == 0x02) return R._("SquareWave2");
            if (type == 0x03) return R._("Wave Memory");
            if (type == 0x04) return R._("Noise");
            if (type == 0x08) return R._("DirectSound Fixed Freq");
            if (type == 0x09) return R._("SquareWave(消音プチノイズ)");
            if (type == 0x0A) return R._("SquareWave(消音プチノイズ)");
            if (type == 0x0B) return R._("Wave Memory(消音プチノイズ)");
            if (type == 0x0C) return R._("Noise(消音プチノイズ)");
            if (type == 0x10) return R._("DirectSound Reverse Playback");
            if (type == 0x18) return R._("DirectSound Fixed Freq Reverse Playback");
            if (type == 0x40) return R._("Multi Sample");
            if (type == 0x80) return R._("DrumPart");
            return "";
        }

        void ExportGBAWave(NumericUpDown addrNumObj,string fingerPrint)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("wav|*.wav|All files|*");

            string songname = "instrument_" + U.ToHexString(AddressList.SelectedIndex) + "_" + fingerPrint;
            uint addr = (uint)addrNumObj.Value;
            addr = U.toOffset(addr);

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


            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            byte[] wave = SongUtil.byteToWav(Program.ROM.Data, addr);
            U.WriteAllBytes(filename, wave);

            U.SelectFileByExplorer(filename);
        }

        void ImportGBAWave(NumericUpDown addrNumObj)
        {
            uint addr = (uint)addrNumObj.Value;
            addr = U.toOffset(addr);

            if (AddressList.SelectedIndex < 0)
            {
                return;
            }
            uint songtable_address = InputFormRef.BaseAddress + (InputFormRef.BlockSize * (uint)AddressList.SelectedIndex);

            string title = R._("インポートするwavファイルを選択してください");
            string filter = R._("wav|*.wav|All files|*");

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
            string filename = open.FileNames[0];

            SongInstrumentImportWaveForm f = (SongInstrumentImportWaveForm)InputFormRef.JumpFormLow<SongInstrumentImportWaveForm>();
            f.Init(filename);
            dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                f.Dettach();
                return;
            }
            byte[] wave = File.ReadAllBytes(f.GetFilename());
            byte[] gbawave = SongUtil.wavToByte(wave);
            if (gbawave == null)
            {
                return;
            }

            f.Dettach();

            if (! U.isSafetyOffset(addr))
            {
                addr = 0;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"Instrument Wave");
            uint newaddr = InputFormRef.WriteBinaryData(this, addr, gbawave, gbawave_length, undodata);
            Program.Undo.Push(undodata);

            addrNumObj.Value = newaddr;
            WriteButton.PerformClick();
        }
        public static MoveToUnuseSpace.ADDR_AND_LENGTH gbawave_length(uint addr)
        {
            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            uint length = Program.ROM.u32(addr + 12);

            aal.addr = addr;
            aal.length = 12 + 4 + length;
            return aal;
        }
        //midi楽器へ変換.
        public static uint ToMidiInstrument(uint InstrumentAddr,uint voca,uint unk = U.NOT_FOUND)
        {
            InputFormRef InputFormRef  = Init(null);
            InputFormRef.ReInit(InstrumentAddr);
            uint addr = InputFormRef.IDToAddr(voca);
            if (addr == U.NOT_FOUND)
            {
                return unk;
            }

            string fingerPrint = DirectoSoundFingerPrint(addr);
            if (fingerPrint == "")
            {
                return unk;
            }

            InstrumentHintSt hint;
            if(! InstrumentHint.TryGetValue(fingerPrint,out hint) )
            {
                return unk;
            }

            if (hint.midi_mapping == U.NOT_FOUND)
            {
                return unk;
            }
            return hint.midi_mapping;
        }
        private void N00_ExportButton_Click(object sender, EventArgs e)
        {
            ExportGBAWave(N00_P4, this.FINGERPRINT.Text);
        }

        private void N00_ImportButton_Click(object sender, EventArgs e)
        {
            ImportGBAWave(N00_P4);
        }

        private void N08_ExportButton_Click(object sender, EventArgs e)
        {
            ExportGBAWave(N08_P4, this.FINGERPRINT.Text);
        }

        private void N08_ImportButton_Click(object sender, EventArgs e)
        {
            ImportGBAWave(N08_P4);
        }

        private void N10_ExportButton_Click(object sender, EventArgs e)
        {
            ExportGBAWave(N10_P4, this.FINGERPRINT.Text);
        }

        private void N10_ImportButton_Click(object sender, EventArgs e)
        {
            ImportGBAWave(N10_P4);
        }

        private void N18_ExportButton_Click(object sender, EventArgs e)
        {
            ExportGBAWave(N18_P4, this.FINGERPRINT.Text);
        }

        private void N18_ImportButton_Click(object sender, EventArgs e)
        {
            ImportGBAWave(N18_P4);
        }

        public static List<U.AddrResult> MakeList(uint addr)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInit(addr);
            return InputFormRef.MakeList();
        }
        public static bool IsWaveMemory(uint instrumentCode)
        {
            if (instrumentCode == 0x03
                || instrumentCode == 0x0B
                )
            {
                Log.Debug("This is WaveMemory({0})", U.To0xHexString(instrumentCode));
                return true;
            }
            return false;
        }
        public static bool IsDirectSound(uint instrumentCode)
        {
            if (instrumentCode == 0x00
                || instrumentCode == 0x08
                || instrumentCode == 0x10
                || instrumentCode == 0x18
                )
            {
                Log.Debug("This is DirectSound({0})", U.To0xHexString(instrumentCode));
                return true;
            }
            return false;
        }
        public static void ExportAllLow(string filename, uint voca_baseaddress)
        {
            ExportAllLow(filename, voca_baseaddress, false);
        }
        public static void ExportOneLow(string filename, InputFormRef ifr, int index)
        {
            string dir = Path.GetDirectoryName(filename);
            string basename = Path.GetFileNameWithoutExtension(filename);

            List<string> lines = new List<string>();

            uint addr = ifr.BaseAddress + (((uint)index) * ifr.BlockSize);
            string str = ExportOneLow(addr, index, dir, basename, ifr, false);
            if (str == "")
            {
                return;
            }
            lines.Add(str);
            File.WriteAllLines(filename, lines);
        }
        static string ExportOneLow(uint addr, int index,
            string dir , string basename,
            InputFormRef ifr, bool isNest)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(U.ToHexString(Program.ROM.u8(addr + 0))); sb.Append("\t");
            sb.Append(U.ToHexString(Program.ROM.u8(addr + 1))); sb.Append("\t");
            sb.Append(U.ToHexString(Program.ROM.u8(addr + 2))); sb.Append("\t");
            sb.Append(U.ToHexString(Program.ROM.u8(addr + 3))); sb.Append("\t");

            uint type = Program.ROM.u8(addr);
            if (type == 0x00
                || type == 0x08
                || type == 0x10
                || type == 0x18
            )
            {//directsound waveデータ.
                uint songdata_addr = Program.ROM.p32(addr + 4);
                if (!U.isSafetyOffset(songdata_addr))
                {
                    return "";
                }
                uint sample_length = Program.ROM.u32(songdata_addr + 12);
                if (!U.isSafetyLength(songdata_addr + 12 + 4, sample_length))
                {
                    return "";
                }
                if (!SongUtil.IsDirectSoundData(Program.ROM.Data, songdata_addr))
                {//壊れたデータ 
                    return "";
                }

                string waveFilename = basename + U.To0xHexString(index) + ".DirectSound.bin";
                byte[] bin = Program.ROM.getBinaryData(songdata_addr, 12 + 4 + sample_length);
                U.WriteAllBytes(Path.Combine(dir, waveFilename), bin);

                sb.Append(waveFilename); sb.Append("\t");
            }
            else if (type == 0x03
                || type == 0x0B
                )
            {//波形データ
                uint songdata_addr = Program.ROM.p32(addr + 4);
                if (!U.isSafetyOffset(songdata_addr))
                {
                    return "";
                }

                byte[] bin = Program.ROM.getBinaryData(songdata_addr, 16);
                string waveFilename = basename + U.To0xHexString(index) + ".Wave.bin";
                U.WriteAllBytes(Path.Combine(dir, waveFilename), bin);

                sb.Append(waveFilename); sb.Append("\t");
            }
            else if (type == 0x80)
            {//ドラム
                uint drum_voices = Program.ROM.p32(addr + 4);
                if (!U.isSafetyOffset(drum_voices))
                {
                    return "";
                }

                uint voca_baseaddress = ifr.BaseAddress;
                if (drum_voices == voca_baseaddress)
                {
                    sb.Append("@SELF+0"); sb.Append("\t");
                }
                else if (isNest)
                {
                    uint voca_endaddress = voca_baseaddress + ((ifr.DataCount + 1) * ifr.BlockSize);
                    if (drum_voices >= voca_baseaddress && drum_voices < voca_endaddress)
                    {
                        sb.Append("@SELF+" + U.ToHexString(drum_voices - voca_baseaddress)); sb.Append("\t");
                    }
                    else
                    {
                        sb.Append("@BROKENDATA"); sb.Append("\t");
                    }
                }
                else
                {
                    string drumFilename = basename + U.To0xHexString(index) + ".Drum.instrument";
                    ExportAllLow(Path.Combine(dir, drumFilename), drum_voices, true);
                    sb.Append(drumFilename); sb.Append("\t");
                }
            }
            else if (type == 0x40)
            {//マルチサンプル
                uint multisample_voices = Program.ROM.p32(addr + 4);
                uint sample_location = Program.ROM.p32(addr + 8);
                if (!U.isSafetyOffset(multisample_voices))
                {
                    return "";
                }
                if (!U.isSafetyOffset(sample_location))
                {
                    return "";
                }

                uint voca_baseaddress = ifr.BaseAddress;
                if (multisample_voices == voca_baseaddress)
                {
                    sb.Append("@SELF+0"); sb.Append("\t");
                }
                else if (isNest)
                {
                    uint voca_endaddress = voca_baseaddress + ((ifr.DataCount + 1) * ifr.BlockSize);
                    if (multisample_voices >= voca_baseaddress && multisample_voices < voca_endaddress)
                    {
                        sb.Append("@SELF+" + U.ToHexString(multisample_voices - voca_baseaddress)); sb.Append("\t");
                    }
                    else
                    {
                        sb.Append("@BROKENDATA"); sb.Append("\t");
                    }
                }
                else
                {//自己参照以外を記録します
                    string multiFilename = basename + U.To0xHexString(index) + ".Multi.instrument";
                    ExportAllLow(Path.Combine(dir, multiFilename), multisample_voices, true);
                    sb.Append(multiFilename); sb.Append("\t");
                }

                byte[] bin = Program.ROM.getBinaryData(sample_location, 128);
                string waveFilename = basename + U.To0xHexString(index) + ".Multi.keys.bin";
                U.WriteAllBytes(Path.Combine(dir, waveFilename), bin);

                sb.Append(waveFilename); sb.Append("\t");
            }
            else
            {
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 4))); sb.Append("\t");
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 5))); sb.Append("\t");
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 6))); sb.Append("\t");
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 7))); sb.Append("\t");
            }

            if (type != 0x40)
            {//マルチサンプル以外は、最後の4バイトはデータです
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 8))); sb.Append("\t");
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 9))); sb.Append("\t");
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 10))); sb.Append("\t");
                sb.Append(U.ToHexString(Program.ROM.u8(addr + 11)));
            }

            U.AddrResult ar = ifr.LoopCallback(index, addr);
            if (!ar.isNULL())
            {
                sb.Append("\t//");
                sb.Append(ar.name);
            }
            return sb.ToString();
        }

        static void ExportAllLow(string filename, uint voca_baseaddress , bool isNest)
        {
            voca_baseaddress = U.toOffset(voca_baseaddress);
            if (!U.isSafetyOffset(voca_baseaddress))
            {
                return;
            }

            string dir = Path.GetDirectoryName(filename);
            string basename = Path.GetFileNameWithoutExtension(filename);

            //楽器リスト本体
            InputFormRef ifr = Init(null);
            ifr.ReInit(voca_baseaddress);

            List<string> lines = new List<string>();

            uint addr = voca_baseaddress;
            for (int i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
            {
                string str = ExportOneLow(addr, i, dir, basename, ifr , isNest);
                if (str == "")
                {
                    continue;
                }
                lines.Add(str);
            }
            File.WriteAllLines(filename,lines);
        }

        class DataWriteHelper
        {
            public byte[] Data;
            public uint RelativeAddress;
            public uint WriteOffset;

            public DataWriteHelper(byte[] data,int writeOffset)
            {
                this.Data = data;
                this.WriteOffset = (uint)writeOffset;
            }
            public DataWriteHelper(uint relative, int writeOffset)
            {
                this.RelativeAddress = relative;
                this.WriteOffset = (uint)writeOffset;
            }
        };

        static bool ImportOneLow(string line, int index, List<byte> bin, List<DataWriteHelper> data, string dir, Undo.UndoData undodata)
        {
            if (U.IsComment(line) || U.OtherLangLine(line))
            {
                return true;
            }
            line = U.ClipComment(line);
            if (line == "")
            {
                return true;
            }

            string[] sp = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (sp.Length < 4 + 2)
            {
                R.ShowStopError("データの形式が正しくありません。\r\n少なくとも{1}個のデータが必要です。\r\n{0}行目", index + 1, 6);
                return false;
            }
            uint type = U.atoh(sp[0]);
            U.append_u8(bin, type);
            U.append_u8(bin, U.atoh(sp[1]));
            U.append_u8(bin, U.atoh(sp[2]));
            U.append_u8(bin, U.atoh(sp[3]));

            if (type == 0x00
                || type == 0x08
                || type == 0x10
                || type == 0x18
            )
            {
                if (sp.Length < 4 + 1 + 4)
                {
                    R.ShowStopError("データの形式が正しくありません。\r\n少なくとも{1}個のデータが必要です。\r\n{0}行目", index + 1, 9);
                    return false;
                }

                string file = Path.Combine(dir, sp[4]);
                if (!File.Exists(file))
                {
                    R.ShowStopError("楽器データ({1})がありません。\r\n{0}行目", index + 1, file);
                    return false;
                }
                byte[] wav = File.ReadAllBytes(file);
                uint found = U.Grep(Program.ROM.Data, wav, 0x100, 0, 4);
                if (found == U.NOT_FOUND)
                {//既存のROMにないので追加
                    data.Add(new DataWriteHelper(wav, bin.Count));
                    U.append_u32(bin, 0); //後でこの位置にアドレスを書く.
                }
                else
                {//既にあるので使いまわす.
                    U.append_u32(bin, U.toPointer(found));
                }
                U.append_u8(bin, U.atoh(sp[5]));
                U.append_u8(bin, U.atoh(sp[6]));
                U.append_u8(bin, U.atoh(sp[7]));
                U.append_u8(bin, U.atoh(sp[8]));
            }
            else if (type == 0x03
                || type == 0x0B
            )
            {
                if (sp.Length < 4 + 1 + 4)
                {
                    R.ShowStopError("データの形式が正しくありません。\r\n少なくとも{1}個のデータが必要です。\r\n{0}行目", index + 1, 9);
                    return false;
                }

                string file = Path.Combine(dir, sp[4]);
                if (!File.Exists(file))
                {
                    R.ShowStopError("楽器データ({1})がありません。\r\n{0}行目", index + 1, file);
                    return false;
                }
                byte[] wav = File.ReadAllBytes(file);
                uint found = U.Grep(Program.ROM.Data, wav, 0x100, 0, 4);
                if (found == U.NOT_FOUND)
                {//既存のROMにないので追加
                    data.Add(new DataWriteHelper(wav, bin.Count));
                    U.append_u32(bin, 0); //後でこの位置にアドレスを書く.
                }
                else
                {//既にあるので使いまわす.
                    U.append_u32(bin, U.toPointer(found));
                }
                U.append_u8(bin, U.atoh(sp[5]));
                U.append_u8(bin, U.atoh(sp[6]));
                U.append_u8(bin, U.atoh(sp[7]));
                U.append_u8(bin, U.atoh(sp[8]));
            }
            else if (type == 0x80)
            {
                if (sp.Length < 4 + 1 + 4)
                {
                    R.ShowStopError("データの形式が正しくありません。\r\n少なくとも{1}個のデータが必要です。\r\n{0}行目", index + 1, 9);
                    return false;
                }

                string file = Path.Combine(dir, sp[4]);
                if (!File.Exists(file))
                {
                    if (file.IndexOf("@SELF+") >= 0)
                    {
                        uint relativeAddress = U.atoh(file.Substring(6));
                        data.Add(new DataWriteHelper(relativeAddress, bin.Count));
                        U.append_u32(bin, 0); //後でこの位置にアドレスを書く.
                    }
                    else if (file.IndexOf("@BROKENDATA") >= 0)
                    {//ドラム内でドラムがあるような変なデータ
                        uint relativeAddress = 0;
                        data.Add(new DataWriteHelper(relativeAddress, bin.Count));
                        U.append_u32(bin, 0); //とりあえず @SELF+0として扱う.
                    }
                    else
                    {
                        R.ShowStopError("楽器データ({1})がありません。\r\n{0}行目", index + 1, file);
                        return false;
                    }
                }
                else
                {
                    uint wav = ImportAllLow(file, undodata);
                    if (wav == U.NOT_FOUND)
                    {
                        R.ShowStopError("ネストする楽器データ({1})を登録できません。\r\n{0}行目", index + 1, file);
                        return false;
                    }
                    U.append_u32(bin, U.toPointer(wav));
                }
                U.append_u8(bin, U.atoh(sp[5]));
                U.append_u8(bin, U.atoh(sp[6]));
                U.append_u8(bin, U.atoh(sp[7]));
                U.append_u8(bin, U.atoh(sp[8]));
            }
            else if (type == 0x40)
            {
                string file = Path.Combine(dir, sp[4]);
                if (!File.Exists(file))
                {
                    if (file.IndexOf("@SELF+") < 0)
                    {
                        R.ShowStopError("楽器データ({1})がありません。\r\n{0}行目", index + 1, file);
                        return false;
                    }
                    uint relativeAddress = U.atoh(file.Substring(6));
                    data.Add(new DataWriteHelper(relativeAddress, bin.Count));
                    U.append_u32(bin, 0); //後でこの位置にアドレスを書く.
                }
                else
                {
                    uint wav = ImportAllLow(file, undodata);
                    if (wav == U.NOT_FOUND)
                    {
                        R.ShowStopError("ネストする楽器データ({1})を登録できません。\r\n{0}行目", index + 1, file);
                        return false;
                    }
                    U.append_u32(bin, U.toPointer(wav));
                }

                file = Path.Combine(dir, sp[5]);
                if (!File.Exists(file))
                {
                    R.ShowStopError("楽器データ({1})がありません。\r\n{0}行目", index + 1, file);
                }

                //データは後で入れましょう.
                byte[] multi = File.ReadAllBytes(file);
                uint found = U.Grep(Program.ROM.Data, multi, 0x100, 0, 4);
                if (found == U.NOT_FOUND)
                {//既存のROMにないので追加
                    data.Add(new DataWriteHelper(multi, bin.Count));
                    U.append_u32(bin, 0); //後でこの位置にアドレスを書く.
                }
                else
                {//既にあるので使いまわす.
                    U.append_u32(bin, U.toPointer(found));
                }
            }
            else
            {
                if (sp.Length < 4 + 4 + 4)
                {
                    R.ShowStopError("データの形式が正しくありません。\r\n少なくとも{1}個のデータが必要です。\r\n{0}行目", index + 1, 12);
                    return false;
                }
                U.append_u8(bin, U.atoh(sp[4]));
                U.append_u8(bin, U.atoh(sp[5]));
                U.append_u8(bin, U.atoh(sp[6]));
                U.append_u8(bin, U.atoh(sp[7]));
                U.append_u8(bin, U.atoh(sp[8]));
                U.append_u8(bin, U.atoh(sp[9]));
                U.append_u8(bin, U.atoh(sp[10]));
                U.append_u8(bin, U.atoh(sp[11]));
            }
            return true;
        }

        public static uint ImportAllLow(string filename, Undo.UndoData undodata)
        {
            string dir = Path.GetDirectoryName(filename);

            //まずはデータ数を知らないといけない.
            List<byte> bin = new List<byte>();
            List<DataWriteHelper> data = new List<DataWriteHelper>();
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                InputFormRef.DoEvents(null, filename + ":" + (i + 1));
                bool r = ImportOneLow(line, i, bin, data, dir, undodata);
                if (!r)
                {
                    return U.NOT_FOUND;
                }
            }
            //終端データ.
            U.append_u32(bin, 0);
            U.append_u32(bin, 0);
            U.append_u32(bin, 0);

            uint startaddr = InputFormRef.AppendBinaryData(bin.ToArray(), undodata);
            if (startaddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            //書き込みアドレスが決定しないと書き込めない追加データの書き戻しを行います。
            {
                bool r = WriteBackData(startaddr , data , undodata);
                if (!r)
                {
                    return U.NOT_FOUND;
                }
            }

            return startaddr;
        }
        //書き込みアドレスが決定しないと書き込めない追加データの書き戻しを行います。
        static bool WriteBackData(uint startaddr, List<DataWriteHelper> data, Undo.UndoData undodata)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Data == null)
                {
                    uint writeAddress = startaddr + data[i].WriteOffset;
                    Program.ROM.write_p32(writeAddress, data[i].RelativeAddress + startaddr, undodata);
                }
                else
                {
                    uint dataAddress = InputFormRef.AppendBinaryData(data[i].Data, undodata);
                    if (dataAddress == U.NOT_FOUND)
                    {
                        return false;
                    }

                    uint writeAddress = startaddr + data[i].WriteOffset;
                    Program.ROM.write_p32(writeAddress, dataAddress, undodata);
                }
            }
            return true;
        }
        static bool ImportOne(string filename,InputFormRef ifr, int index, Undo.UndoData undodata)
        {
            string dir = Path.GetDirectoryName(filename);

            //まずはデータ数を知らないといけない.
            List<byte> bin = new List<byte>();
            List<DataWriteHelper> data = new List<DataWriteHelper>();
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                InputFormRef.DoEvents(null, filename + ":" + (i + 1));
                bool r = ImportOneLow(line, i, bin, data, dir, undodata);
                if (!r)
                {
                    return false;
                }
            }
            Debug.Assert(bin.Count == 12);

            uint writeaddr = ifr.BaseAddress + ((uint)index * ifr.BlockSize);
            Program.ROM.write_range(writeaddr, bin.ToArray(), undodata);

            //書き込みアドレスが決定しないと書き込めない追加データの書き戻しを行います。
            {
                bool r = WriteBackData(ifr.BaseAddress, data, undodata);
                if (!r)
                {
                    return false;
                }
            }

            return true;
        }

        private void Inst_ExportButton_Click(object sender, EventArgs e)
        {
            if (AddressList.SelectedIndex < 0)
            {
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("MusicalInstrumentOne|*.instone|All files|*");

            string songname = "inst" + U.ToHexString2(AddressList.SelectedIndex);

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

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                ExportOneLow(filename, InputFormRef, AddressList.SelectedIndex);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void Inst_ImportButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            if (AddressList.SelectedIndex < 0)
            {
                return;
            }

            string filename;
            string title = R._("インポートする楽器ファイルを選択してください");
            string filter = R._("MusicalInstrumentOne|*.instone|All files|*");

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

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                Undo.UndoData undodata = Program.Undo.NewUndoData(this, "Instrument " + AddressList.Text);
                bool r = ImportOne(filename, InputFormRef, AddressList.SelectedIndex, undodata);
                if (!r)
                {
                    Program.Undo.Rollback(undodata);
                    R.ShowStopError("楽器データをインポートできませんでした。");
                    return;
                }
                Program.Undo.Push(undodata);
            }
            InputFormRef.ReloadAddressList();
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
        }
    }
}
