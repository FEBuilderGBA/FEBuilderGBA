using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapTerrainNameEngForm : Form
    {
        public MapTerrainNameEngForm()
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
                , Program.ROM.RomInfo.map_terrain_name_pointer()
                , 2
                , (int i, uint addr) =>
                {//読込最大値検索
                    return Program.ROM.u16(addr + 0) != 0x00;
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    uint text_id = Program.ROM.u16(addr);
                    string hint = MapTerrainNameForm.MakeHint_Full((uint)i);
                    return U.ToHexString(i) + " " + text_id.ToString("X04") + " " + TextForm.Direct(text_id) + hint; 
                }
                );
        }

        private void MapTerrainNameEngForm_Load(object sender, EventArgs e)
        {
        }

        //地名リストを作る.
        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        public static string GetName(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint text_id = Program.ROM.u16(addr);
            return FETextDecode.Direct(text_id);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "TerrainEng";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.UNIT, InputFormRef, new uint[] { 0 });
        }

    }
}
