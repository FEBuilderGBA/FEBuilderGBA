﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapLoadFunctionForm : Form
    {
        public MapLoadFunctionForm()
        {
            InitializeComponent();
            //FE8のみ
            L_0_COMBO.BeginUpdate();
            if (Program.ROM.RomInfo.is_multibyte())
            {
                //{0}=ワールドマップ以外からでも入れる
                string a = MyTranslateResource.str((string)L_0_COMBO.Items[0]);
                L_0_COMBO.Items[0] = a.Replace("{0}", U.ToHexString(0x80C1FF0));

                //{1}=必ずワールドマップから移動する必要がある
                a = MyTranslateResource.str((string)L_0_COMBO.Items[1]);
                L_0_COMBO.Items[1] = a.Replace("{0}", U.ToHexString(0x80C1FB4));
            }
            else
            {
                //{0}=ワールドマップ以外からでも入れる
                string a = MyTranslateResource.str((string)L_0_COMBO.Items[0]);
                L_0_COMBO.Items[0] = a.Replace("{0}", U.ToHexString(0x80BD1E4));

                //{1}=必ずワールドマップから移動する必要がある
                a = MyTranslateResource.str((string)L_0_COMBO.Items[1]);
                L_0_COMBO.Items[1] = a.Replace("{0}", U.ToHexString(0x80BD1A8));
            }
            L_0_COMBO.EndUpdate();

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.CheckProtectionAddrHigh = false; //書き換える対象がswitchなので低い位地に書き換えるデータがあります。

            ReInit(this.InputFormRef);
        }
        private void MapLoadFunctionForm_Load(object sender, EventArgs e)
        {
            bool enable = PatchUtil.IsSwitch1Enable(Program.ROM.RomInfo.map_load_function_switch1_address());
            if (!enable)
            {
                this.ERROR_NOT_FOUND.Show();
                this.WriteButton.Hide();
            }

            //章拡張を表示するかどうか
            uint map_count = MapSettingForm.GetDataCount();
            if (MapSettingForm.IsShowChapterExetdns((int)map_count))
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
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {
                    return false;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + MapSettingForm.GetMapName((uint)i);
                }
                );
        }

        static uint ReInit(InputFormRef ifr)
        {
            if (ifr == null)
            {
                return U.NOT_FOUND;
            }
            uint addr = Program.ROM.p32(Program.ROM.RomInfo.map_load_function_pointer());
            uint count = Program.ROM.u8(Program.ROM.RomInfo.map_load_function_switch1_address() + 0);
            bool enable = PatchUtil.IsSwitch1Enable(Program.ROM.RomInfo.map_load_function_switch1_address());
            if (enable == false)
            {
                return U.NOT_FOUND;
            }
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }

            ifr.ReInit(addr, count + 1);
            return addr;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SwitchListExpandsButton_Click(object sender, EventArgs e)
        {
            if (this.L_0_COMBO.Items.Count <= 0)
            {
                return;
            }
            uint defAddr = U.atoh(this.L_0_COMBO.Items[0].ToString());

            uint newCount = ImagePortraitForm.DataCount();

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"MapLoadFunction SwithExpands");

            PatchUtil.Switch1Expands(Program.ROM.RomInfo.unit_increase_height_pointer()
                , Program.ROM.RomInfo.map_load_function_switch1_address()
                , newCount
                , defAddr
                , undodata);
            Program.Undo.Push(undodata);

            ReInit(this.InputFormRef);
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "MapLoadFunction";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);
        }
        public static bool IsEnterChapterAlways(uint chapterID)
        {
            InputFormRef InputFormRef = Init(null);
            ReInit(InputFormRef);
            uint addr = InputFormRef.BaseAddress + (chapterID * InputFormRef.BlockSize);
            uint p = Program.ROM.u32(addr);

            if (Program.ROM.RomInfo.is_multibyte())
            {
                return p != 0x80C1FB4;
            }
            else
            {
                return p != 0x80BD1A8;
            }
        }

    
    }
}
