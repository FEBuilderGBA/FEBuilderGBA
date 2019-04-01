using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapTerrainNameForm : Form
    {
        public MapTerrainNameForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.map_terrain_name_pointer()
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    //0  がポインタであればデータがあると考える.
                    return U.isPointerOrNULL(Program.ROM.u32(addr + 0));
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    string name = Program.ROM.getString(Program.ROM.p32(addr));
                    return U.ToHexString(i + 1) + " " + name;
                }
                );
        }

        private void MapTerrainNameForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addr = InputFormRef.SelectToAddr(this.AddressList);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            uint c_addr = Program.ROM.p32(addr);
            if (!U.isSafetyOffset(c_addr))
            {
                return;
            }
            int length = 0;
            string str = Program.ROM.getString(c_addr, out length);

            this.TextBox.Text = str;
            this.BlockSize.Text = length.ToString();

            uint write_pointer = this.InputFormRef.BaseAddress + (this.InputFormRef.BlockSize * (uint)this.AddressList.SelectedIndex);
            uint write_addr = Program.ROM.p32(write_pointer);
            this.AddressPointer.Text = write_addr.ToString("X");
            InputFormRef.WriteButtonToYellow(this.TextWriteButton, false);
        }

        //地名リストを作る.
        public static List<U.AddrResult> MakeList()
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {
                return MapTerrainNameEngForm.MakeList();
            }

            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        public static string GetName(uint id)
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {
                
                string name = MapTerrainNameEngForm.GetName(id);
                name = TextForm.StripAllCode(name);
                return name;
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint c_addr = Program.ROM.p32(addr);
            if (!U.isSafetyOffset(c_addr))
            {
                return "";
            }
            return Program.ROM.getString(c_addr);
        }

        private void TextWriteButton_Click(object sender, EventArgs e)
        {
            byte[] stringbyte = Program.SystemTextEncoder.Encode(TextBox.Text);
            stringbyte = U.ArrayAppend(stringbyte, new byte[] { 0x00 });

            string undoname = this.Text + ":" + U.ToHexString(this.AddressList.SelectedIndex);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undoname);

            uint write_pointer = (uint)(this.InputFormRef.BaseAddress + (this.InputFormRef.BlockSize * this.AddressList.SelectedIndex));
            uint writeAddr = InputFormRef.WriteBinaryDataPointer(this
                ,write_pointer
                , stringbyte
                , InputFormRef.get_data_pos_callback
                , undodata
            );
            if (writeAddr == U.NOT_FOUND)
            {
                return;
            }

            InputFormRef.ReloadAddressList();
            InputFormRef.ShowWriteNotifyAnimation(this, writeAddr);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "Terrain";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 });

            uint p = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                uint nameAddr = Program.ROM.p32(0 + p);
                if (U.isSafetyOffset(nameAddr))
                {
                    int length;
                    name = Program.ROM.getString(nameAddr, out length);

                    FEBuilderGBA.Address.AddAddress(list,nameAddr
                        , (uint)length
                        , p + 0
                        , name
                        , Address.DataTypeEnum.BIN);
                }
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.TextWriteButton, true);
        }
    }
}
