using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SoundBossBGMForm : Form
    {
        public SoundBossBGMForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.sound_boss_bgm_pointer()
                , 8
                , (int i, uint addr) =>
                {//0xFFまで
                    if (Program.ROM.u8(addr) == 0xFF)
                    {
                        return false;
                    }
                    if (i > 10 && Program.ROM.IsEmpty(addr, 8 * 10))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = (uint)Program.ROM.u8(addr);
                    uint song_id = (uint)Program.ROM.u32(addr+4);
                    return U.ToHexString(unit_id) + " " + UnitForm.GetUnitName(unit_id) + " : " + U.ToHexString(song_id) + SongTableForm.GetSongName(song_id);
                }
                );
        }

        private void SoundBossBGMForm_Load(object sender, EventArgs e)
        {
        }

        public void JumpTo(uint search_unit_id)
        {
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint unit_id = (uint)Program.ROM.u8(addr);
                if (search_unit_id == unit_id)
                {
                    this.AddressList.SelectedIndex = i;
                    return;
                }
                addr += InputFormRef.BlockSize;
            }

        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "BossBGM";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] {  });
        }

    }
}
