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
                    return U.ToHexString(i) + " " + GetName(addr);
                }
                );
        }

        static string GetName(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint textid = Program.ROM.u16(addr + 0);
            return TextForm.DirectAndStripAllCode(textid);
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
                    , isPointerOnly ? "" : "GameOption " + GetName(p)
                    );
            }
        }
        public static void MakeTextIDArray(List<TextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            TextID.AppendTextIDPP(list, FELint.Type.STATUS_GAME_OPTION, InputFormRef, new uint[] { 0, 4, 6, 12, 14, 20, 22, 28, 30 });
        }
    }
}
