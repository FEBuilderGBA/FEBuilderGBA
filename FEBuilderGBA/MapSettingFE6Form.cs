using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapSettingFE6Form : Form
    {
        public MapSettingFE6Form()
        {
            InitializeComponent();

            InputFormRef.markupJumpLabel(X_MAPSTYLE_CHANGE);
            MapPictureBox.HideCommandBar();
            U.ConvertComboBox(InputFormRef.GetTerrainSetDic(), ref L_18_COMBO , true);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }
        private void MapSettingFE6Form_Load(object sender, EventArgs e)
        {
            //章拡張を表示するかどうか
            if (MapSettingForm.IsShowChapterExetdns(this.AddressList.Items.Count))
            {
                AddressListExpandsButton_255.Show();
            }
            else
            {
                this.AddressList.Height += AddressListExpandsButton_255.Height;
                AddressListExpandsButton_255.Hide();
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(MapSettingFE6Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.map_setting_pointer()
                , Program.ROM.RomInfo.map_setting_datasize()
                , (int i, uint addr) =>
                {
                    return IsMapSettingEnd(addr);
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                );
        }
        static bool IsMapSettingEnd(uint addr)
        {
            uint a = Program.ROM.u32(addr + 0);
            if (U.isPointer(a))
            {
                return true;
            }

            if (a == 0)
            {//CPを0にしている人対策
                uint weather = Program.ROM.u8(addr + 11);
                if (weather >= 0xE)
                {//天気で未知のデータがある
                    return false;
                }
                uint plist_direct = Program.ROM.u32(addr + 4);
                if (plist_direct == 0 || plist_direct == 0xFFFFFFFF)
                {//PLSITとして読めない
                    //念のためもう一つチェック
                    plist_direct = Program.ROM.u32(addr + 8);
                    if (plist_direct == 0 || plist_direct == 0xFFFFFFFF)
                    {//PLSITとして読めない
                        return false;
                    }
                }
                uint textmax = TextForm.GetDataCount();
                uint map1 = Program.ROM.u16(addr + 56);
                if (map1 >= textmax)
                {
                    return false;
                }
                uint clearcond1 = Program.ROM.u16(addr + 48);
                if (clearcond1 >= textmax)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint obj_plist = (uint)W4.Value;
            uint palette_plist = (uint)B6.Value;
            uint config_plist = (uint)B7.Value;
            uint mappointer_plist = (uint)B8.Value;
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(obj_plist, palette_plist, config_plist, mappointer_plist));
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "MapSetting";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 });

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                FEBuilderGBA.Address.AddCString(list
                    , addr + 0);
            }
        }

        private void W4_ValueChanged(object sender, EventArgs e)
        {
            if (InputFormRef.IsUpdateLock)
            {
                return;
            }
            AddressList_SelectedIndexChanged(sender, e);
        }

        private void X_MAPSTYLE_CHANGE_Click(object sender, EventArgs e)
        {
            MapStyleEditorAppendPopupForm f = (MapStyleEditorAppendPopupForm)InputFormRef.JumpFormLow<MapStyleEditorAppendPopupForm>();
            f.Init((uint)W4.Value, (uint)B6.Value, (uint)B7.Value, (uint)B8.Value, (uint)B9.Value, (uint)B10.Value);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            W4.Value = f.GetOBJPLIST();
            B6.Value = f.GetPALPLIST();
            B7.Value = f.GetCONFIGPLIST();
            B9.Value = f.GetANIME1PLIST();
            B10.Value = f.GetANIME2PLIST();

        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.MAPSETTING, InputFormRef, new uint[] { 48,50,52,60 });
            UseValsID.AppendSongID(list, FELint.Type.MAPSETTING, InputFormRef, new uint[] { 20, 21, 22, 23, 24 }, isByte: true);
        }
    }
}
