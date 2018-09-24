using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapBGMForm : Form
    {
        public WorldMapBGMForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.worldmap_bgm_pointer()
                , 4
                , (int i, uint addr) =>
                {//終端データは存在しない
                    return i < (54 / 2);
                }
                , (int i, uint addr) =>
                {
                    string name = WorldMapPointForm.GetWorldMapPointName((uint)i);
                    return U.ToHexString(i) + " " + name;
                }
                );
        }

        private void WorldMapPointForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "WorldMapBGM";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }

        private void MapPictureBox_Load(object sender, EventArgs e)
        {

        }

    }
}
