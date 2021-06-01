using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SongTableForm : Form
    {
        public SongTableForm()
        {
            InitializeComponent();
            fixDocsBugs = new U.FixDocsBugs(this);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            this.X_REF.ItemHeight = (int)(this.X_REF.Font.Height * 2.4);
            this.X_REF.OwnerDraw(InputFormRef.DrawRefTextList, DrawMode.OwnerDrawFixed, false);
            //            this.Icon = Properties.Resources.icon_music;

            if (Program.ROM.RomInfo.version() == 0)
            {//未知のバージョンだとサウンドルームを出せない
                SoundRommPanel.Hide();
                this.X_REF.Hide();
            }

        }

        public static uint GetSoundTablePointer()
        {
            uint p = Program.ROM.RomInfo.sound_table_pointer();
            uint a = Program.ROM.u32(p);
            if (U.isSafetyPointer(a))
            {
                return p;
            }
            p = SongUtil.FindSongTablePointer(Program.ROM.Data);
            if (U.isSafetyOffset(p))
            {
                a = Program.ROM.u32(p);
                if (U.isSafetyPointer(a))
                {
                    return p;
                }
            }
            return 0;
        }

        U.FixDocsBugs fixDocsBugs;
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , GetSoundTablePointer()
                , 8
                , (int i, uint addr) =>
                {//読込最大値検索
                    return U.isPointer(Program.ROM.u32(addr));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + GetSongNameFast((uint) i , addr);
                }
                );
        }

        private void SongTableForm_Load(object sender, EventArgs e)
        {
            //アイテムアイコン拡張を表示するかどうか
            if (IsShowSongTableExetdns(this.AddressList.Items.Count))
            {
                AddressListExpandsButton_32766.Show();
            }
            else
            {
                this.AddressList.Height += AddressListExpandsButton_32766.Height;
                AddressListExpandsButton_32766.Hide();
            }
            fixDocsBugs.AllowMaximizeBox();
        }

        public static string GetSongName(uint song_id)
        {
            if (Program.ROM.RomInfo.version() == 0)
            {
                return "";
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(song_id);
            return GetSongNameFast(song_id, addr);
        }

        //名前の取得   アドレスを指定できるので、早く取得できる 
        public static string GetSongNameFast(uint song_id, uint addr)
        {
            string emptyTrackMessage = "";
            if (IsEmptyTrack(song_id,addr))
            {
                emptyTrackMessage = R._("[空きトラック]");
            }

            string name = SoundRoomForm.GetSongNameWhereSongID(song_id);
            if (name != "")
            {
                return name.Trim() + InputFormRef.GetCommentSA(addr) + U.SA(emptyTrackMessage);
            }
            //サウンドルームにない音楽はSEだろうから、SE Listから検索する.
            return U.at(SoundEffectList, song_id) + InputFormRef.GetCommentSA(addr) + U.SA(emptyTrackMessage);
        }

        static bool IsEmptyTrack(uint song_id , uint addr)
        {
            if (song_id == 0 || song_id >= 0x7FF)
            {//特殊指定。空きではないことにする
                return false;
            }

            uint songTrack = Program.ROM.p32(addr+0);
            if (songTrack == 0)
            {//空き
                return true;
            }
            if (!U.isSafetyOffset(songTrack))
            {//空きではないが変なデータ
                return false;
            }
            uint trackCount = Program.ROM.u8(songTrack);
            if (trackCount == 0)
            {//トラック数 ゼロ
                return true;
            }
            //空きではない.
            return false;
        }

        public static uint GetSongAddr(uint song_id)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.IDToAddr(song_id);
        }

        public static List<U.AddrResult> MakeItemList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        private void SONGPLAY_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAsSappy((uint)AddressList.SelectedIndex);
        }


        static Dictionary<uint, string> SoundEffectList;
        public static void PreLoadResource(string fullfilename)
        {
            SoundEffectList = U.LoadDicResource(fullfilename);
        }

        public static string GetC85SoundEffect(uint command)
        {
            string need = "@C85_" + U.ToHexString(command);

            StringBuilder sb = new StringBuilder();
            foreach (var pair in SoundEffectList)
            {
                if (pair.Value.IndexOf(need) >= 0)
                {
                    sb.AppendLine(U.ToHexString(pair.Key) + "=" + pair.Value);
                }
            }
            return sb.ToString();
        }

        public static string GetErrorMessage(uint song_id,string type)
        {
            if (type == "")
            {
                return "";
            }

            if (song_id == 0 || song_id == 0xFFFF)
            {
                return "";
            }

            uint addr = GetSongAddr(song_id);
            if (addr == U.NOT_FOUND)
            {
                return "";
            }
            uint playerType = Program.ROM.u32(addr + 4);
            if (type == "MAP")
            {
                if (playerType == 0x00010001)
                {
                    return ""; //OK
                }
                return R._("警告\r\nマップで再生する音楽は、SongTableのPriority(PlayerType)で、0x00010001を指定する必要があります。\r\n現在の設定:{0}", U.To0xHexString(playerType));
            }
            else if (type == "SFX")
            {
                if (playerType >= 0x00030003)
                {
                    return ""; //OK
                }
                return R._("警告\r\n効果音で利用する音楽は、SongTableのPriority(PlayerType)で、0x00030003より大きい値を指定する必要があります。\r\n現在の設定:{0}", U.To0xHexString(playerType));
            }
            return "";
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.SONGTABLE, U.NOT_FOUND
                    , R._("ソングテーブルが極端に少ないです。破損している可能性があります。")));
            }

            uint songtable_addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, songtable_addr += InputFormRef.BlockSize)
            {
                uint song_header = Program.ROM.u32(songtable_addr + 0);
                if (song_header == 0)
                {
                    continue;
                }
                FELint.CheckPointerAlien4(song_header, errors, FELint.Type.SONGTABLE, songtable_addr, i);
                SongTrackForm.MakeCheckError(errors, i , U.toOffset(song_header));
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string selfname = "SongTable";
            InputFormRef InputFormRef = Init(null);

            FEBuilderGBA.Address.AddAddress(list, InputFormRef, selfname, new uint[] { 0 });
            uint songpointer = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, songpointer += InputFormRef.BlockSize)
            {
                MakeAllDataLength_Song_And_Inst(list, i, songpointer);
            }
        }
        static void MakeAllDataLength_Song_And_Inst(List<Address> list, int i, uint songpointer)
        {
            uint songaddr = Program.ROM.p32(songpointer);
            if (!U.isSafetyOffset(songaddr))
            {
                return;
            }

            {//楽譜
                string name = "Song" + U.ToHexString(i) + " ";
                //リサイクルで回収できるので、仮にこのデータをリサイクルするとしたら、どうなるだけ求める(実際にリサイクルはしない)
                SongUtil.RecycleOldSong(ref list, name, songpointer);
            }

            uint instpointer = songaddr + 4;
            //uint instaddr = Program.ROM.p32(instpointer);
            {//楽器
                string name = "SongInst" + U.ToHexString(i) + " ";
                SongInstrumentForm.RecycleOldInstrument(ref list, name, instpointer);
            }
        }

        static void MakeAllDataLength_Song(List<Address> list, int i, uint songpointer)
        {
            uint songaddr = Program.ROM.p32(songpointer);
            if (!U.isSafetyOffset(songaddr))
            {
                return;
            }

            {//楽譜
                string name = "Song" + U.ToHexString(i) + " ";
                //リサイクルで回収できるので、仮にこのデータをリサイクルするとしたら、どうなるだけ求める(実際にリサイクルはしない)
                SongUtil.RecycleOldSong(ref list, name, songpointer);
            }
        }

        public static RecycleAddress MakeRecycleSong(uint songtable_address)
        {
            //今回の楽曲リサイクル
            List<FEBuilderGBA.Address> recycle = new List<FEBuilderGBA.Address>();
            MakeAllDataLength_Song(recycle, 0, songtable_address);

            //ポインタを共有しているといけないので調べる
            List<FEBuilderGBA.Address> list = new List<FEBuilderGBA.Address>(); 
            InputFormRef InputFormRef = Init(null);

            uint songpointer = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, songpointer += InputFormRef.BlockSize)
            {
                if (songtable_address == songpointer)
                {//自分自身
                    continue;
                }

                MakeAllDataLength_Song(list, i, songpointer);
            }

            RecycleAddress ra = new RecycleAddress(recycle);
            ra.SubRecycle(list);

            return ra;
        }

        public static RecycleAddress MakeRecycleSongAndInst(uint songtable_address)
        {
            //今回の楽曲リサイクル
            List<FEBuilderGBA.Address> recycle = new List<FEBuilderGBA.Address>();
            MakeAllDataLength_Song_And_Inst(recycle, 0, songtable_address);

            //ポインタを共有しているといけないので調べる
            List<FEBuilderGBA.Address> list = new List<FEBuilderGBA.Address>();
            InputFormRef InputFormRef = Init(null);

            uint songpointer = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, songpointer += InputFormRef.BlockSize)
            {
                if (songtable_address == songpointer)
                {//自分自身
                    continue;
                }

                MakeAllDataLength_Song_And_Inst(list, i, songpointer);
            }

            RecycleAddress ra = new RecycleAddress(recycle);
            ra.SubRecycle(list);

            return ra;
        }

        //SongHeaderアドレスから曲名への逆変換
        public static string GetSongNameWhereSongHeader(uint headerAddr)
        {
            headerAddr = U.toOffset(headerAddr);

            InputFormRef InputFormRef = Init(null);
            uint songpointer = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, songpointer += InputFormRef.BlockSize)
            {
                uint songaddr = Program.ROM.p32(songpointer);
                if (songaddr == headerAddr)
                {
                    return U.ToHexString(i) + " " +GetSongNameFast(i, songpointer);
                }
            }
            return "";
        }

        public static void ReloadList()
        {
            SongTableForm f = (SongTableForm)InputFormRef.GetForm<SongTableForm>();
            if (f == null)
            {//ウィンドウを開いていない
                return;
            }

            int selectedIndex = f.AddressList.SelectedIndex;
            f.ReloadListButton.PerformClick();
            f.AddressList.SelectedIndex = selectedIndex;
        }

        bool IsShowSongTableExetdns(int count)
        {
            if (OptionForm.show_song_table_extends() == OptionForm.show_extends_enum.Show)
            {//表示する設定の場合は表示する.
                return true;
            }

            uint addr = Program.ROM.u32(Program.ROM.RomInfo.sound_table_pointer());
            if (addr < Program.ROM.RomInfo.extends_address())
            {//拡張されていないので表示しない
                return false;
            }
            //拡張されているので表示する
            return true;
        }
        void UpdateRef(uint id)
        {
            if (Program.ROM.RomInfo.version() == 0)
            {
                return;
            }

            bool r = InputFormRef.UpdateRef(this.X_REF, id, UseValsID.TargetTypeEnum.SONG);
            if (r == false)
            {
                return;
            }

            if (id == 0)
            {
                return;
            }

            //サウンドルームにない音楽はSEだろうから、SE Listから検索する.
            if (!SoundEffectList.ContainsKey(id))
            {
                return;
            }

            var t = new UseValsID(FELint.Type.SE_SYSTEM, U.NOT_FOUND, SoundEffectList[id], id, UseValsID.TargetTypeEnum.SONG, id);
            this.X_REF.Items.Add(t);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRef((uint)this.AddressList.SelectedIndex);
        }

        private void X_REF_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InputFormRef.GotoRef(this.X_REF);
        }

        private void X_REF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InputFormRef.GotoRef(this.X_REF);
            }
        }
    }
}
