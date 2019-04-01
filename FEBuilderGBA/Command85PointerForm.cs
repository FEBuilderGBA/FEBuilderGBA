using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class Command85PointerForm : Form
    {
        public Command85PointerForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.CheckProtectionAddrHigh = false; //書き換える対象がswitchなので低い位地に書き換えるデータがあります。
            Comment_85command_Dic = U.LoadDicResource(U.ConfigDataFilename("battleanime_85command_"));
        }
        Dictionary<uint, string> Comment_85command_Dic;

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.command_85_pointer_table_pointer()
                , 4
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    if (! U.isPointerOrNULL(a))
                    {
                        return false;
                    }
                    if (a == 0)
                    {//nullがあるらしいのでその場合無視.
                        return true;
                    }
                    if (a <= 0x08000100)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    int id = (i + 0x19);
                    return U.ToHexString(id) + " " + "C" + id.ToString("X02");
                }
                );
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.X_NAME.Text = U.at(Comment_85command_Dic, this.AddressList.SelectedIndex + 0x19);
        }



        private void Command85PointerForm_Load(object sender, EventArgs e)
        {

        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "Command85Pointer";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);
        }
    }
}
