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
    public partial class OtherTextForm : Form
    {
        public OtherTextForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
            UpdateAddressList();
        }

        void UpdateAddressList()
        {
            int selected = this.AddressList.SelectedIndex;
            this.AddressList.BeginUpdate();
            this.AddressList.Items.Clear();

            List<U.AddrResult> list = MakeOtherTextMap();
            for (int i = 0; i < list.Count; i++)
            {
                this.AddressList.Items.Add(list[i].name);
            }
            this.AddressList.Tag = list;
            this.AddressList.EndUpdate();

            this.AddressList.SelectedIndex = selected;
        }

        static List<U.AddrResult> MakeOtherTextMap()
        {
            List<U.AddrResult> list = new List<U.AddrResult>();
            string fullfilename = U.ConfigDataFilename("other_text_");
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return list;
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
                    line = U.ClipComment(line);
                    uint addr = U.toOffset(U.atoh(line));
                    if (!U.isSafetyOffset(addr))
                    {
                        continue;
                    }

                    uint p_str = Program.ROM.p32(addr);
                    string str = U.ToHexString(p_str) + " " + Program.ROM.getString(p_str);

                    list.Add(new U.AddrResult(addr,str,p_str));
                }
            }
            return list;
        }

        private void OtherTextForm_Load(object sender, EventArgs e)
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
            this.AddressPointer.Text = U.ToHexString(c_addr);
            this.BlockSize.Text = length.ToString();
        }

        public static List<U.AddrResult> MakeList()
        {
            return MakeOtherTextMap();
        }

        private void TextWriteButton_Click(object sender, EventArgs e)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(AddressList, AddressList.SelectedIndex);
            if (ar.isNULL())
            {
                return;
            }

            byte[] stringbyte = Program.SystemTextEncoder.Encode(TextBox.Text);
            stringbyte = U.ArrayAppend(stringbyte, new byte[] { 0x00 });

            string undoname = this.Text + ":" + U.ToHexString(this.AddressList.SelectedIndex);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undoname);

            uint writeAddr = InputFormRef.WriteBinaryDataPointer(this
                , ar.addr
                , stringbyte
                , InputFormRef.get_data_pos_callback
                , undodata
            );

            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this, writeAddr);

            UpdateAddressList();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "OtherText";

            List<U.AddrResult> textlist = MakeOtherTextMap();
            for (int i = 0; i < textlist.Count; i++)
            {
                uint p = textlist[i].addr;
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
    }
}
