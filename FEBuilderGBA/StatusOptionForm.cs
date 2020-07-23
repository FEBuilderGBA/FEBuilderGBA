using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class StatusOptionForm : Form
    {
        public StatusOptionForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawGameOptionAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.status_game_option_pointer()
                , 44
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr + 40));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + GetNameFast(addr);
                }
                );
        }

        static string GetNameFast(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint textid = Program.ROM.u16(addr + 0);
            return TextForm.DirectAndStripAllCode(textid);
        }
        public static string GetNameIndex(uint index)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(index);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            return GetNameFast(addr);
        }
        public static string GetValueNameIndex(uint index,uint value_index)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(index);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            if (value_index >= 4)
            {
                return "";
            }
            
            uint offset = (value_index * 8) + 6;
            return GetNameFast(addr + offset);
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
            string name = "GameOption";
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 40 });

            uint p = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                FEBuilderGBA.Address.AddFunction(list
                    , p + 40
                    , isPointerOnly ? "" : "GameOption " + GetNameFast(p)
                    );
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.STATUS_GAME_OPTION, InputFormRef, new uint[] { 0, 4, 6, 12, 14, 20, 22, 28, 30 });
        }

        //アイコン
        public static Bitmap DrawIcon(uint id, int palette_type = 0, bool height16_limit = false)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }
            uint icon_id = Program.ROM.u32(addr + 36);

            Bitmap bitmap = ImageSystemIconForm.MusicIcon(icon_id / 2);
            return bitmap;
        }
    }
}
