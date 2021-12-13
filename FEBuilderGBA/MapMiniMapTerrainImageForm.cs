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
    public partial class MapMiniMapTerrainImageForm : Form
    {
        public MapMiniMapTerrainImageForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            InitCombo();
        }
        void InitCombo()
        {
            string configFilename = U.ConfigDataFilename("map_minimap_tile_array_");

            this.L_0_COMBO.Items.Clear();
            if (File.Exists(configFilename))
            {
                string[] lines = File.ReadAllLines(configFilename);

                this.L_0_COMBO.BeginUpdate();
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    line = U.ClipComment(line);
                    line = line.Trim();
                    this.L_0_COMBO.Items.Add(line);
                }
                this.L_0_COMBO.EndUpdate();
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.map_minimap_tile_array_pointer()
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < Program.ROM.RomInfo.map_terrain_type_count();
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    string name = MapTerrainNameForm.GetName((uint)i);
                    return U.ToHexString(i) + " " + name;
                }
                );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "MapMiniMapTerrain";

            InputFormRef ifr = Init(null);
            FEBuilderGBA.Address.AddAddress(list
                , ifr
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            List<U.AddrResult> arlist = ifr.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);
        }

        private void MapMiniMapTerrainImageForm_Load(object sender, EventArgs e)
        {

        }
    }
}
