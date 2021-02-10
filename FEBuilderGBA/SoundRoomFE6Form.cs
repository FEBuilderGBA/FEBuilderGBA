using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SoundRoomFE6Form : Form
    {
        public SoundRoomFE6Form()
        {
            InitializeComponent();
            if (PatchUtil.SearchSoundRoomUnlock() == PatchUtil.soundroom_unlock_enum.Enable)
            {
                J_0_SONG.AccessibleDescription = "";
            }

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.sound_room_pointer()
                , Program.ROM.RomInfo.sound_room_datasize()
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (Program.ROM.u32(addr) == 0xFFFFFFFF)
                    {
                        return false;
                    }
                    if ( i > 10 
                        && Program.ROM.IsEmpty(addr, Program.ROM.RomInfo.sound_room_datasize() * 10))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    return (i + 1).ToString("D3") + " " + SoundRoomForm.GetSongName((uint)i);
                }
                );
        }

        private void SoundRoomForm_Load(object sender, EventArgs e)
        {
        }

        public void JumpToSongID(uint song_id)
        {
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint a = Program.ROM.u32(addr);
                if (song_id == a)
                {
                    U.SelectedIndexSafety(this.AddressList, i);
                    return;
                }
                addr += InputFormRef.BlockSize;
            }
            return ;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "SoundRoomFE6";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 4 , 8});
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.SOUNDROOM, InputFormRef, new uint[] { 4,8 });
            UseValsID.AppendSongID(list, FELint.Type.SOUNDROOM, InputFormRef, new uint[] { 0 });
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_RoomPosstionLabel.Text = SoundRoomForm.ConvertPosstion(this.AddressList.SelectedIndex);
        }

    }
}
