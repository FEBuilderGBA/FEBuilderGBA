using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MantAnimationForm : Form
    {
        public MantAnimationForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawImageBattleAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.CheckProtectionAddrHigh = false; //書き換える対象がswitchなので低い位地に書き換えるデータがあります。
            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;
            InputFormRef.markupJumpLabel(JUMP_TO_BATTLEANIME);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.mant_command_pointer
                , 4
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    return U.isPointer(a);
                }
                , (int i, uint addr) =>
                {
                    uint id = (uint)i + Program.ROM.RomInfo.mant_command_startadd;
                    return U.ToHexString(id) + " " + ImageBattleAnimeForm.GetBattleAnimeName(id);
                }
                );
        }


        private void MantAnimationForm_Load(object sender, EventArgs e)
        {

        }

        //配置座標が拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;
            if (count <= 0)
            {
                return;
            }

            //マントコマンドは個数が書かれているので、修正しないといけない.
            uint count_addr = Program.ROM.RomInfo.mant_command_count_address;
            Program.Undo.Push("MantCount",count_addr,4);
            Program.ROM.write_u8(count_addr,(uint)count - 1);
        }

        private void JUMP_TO_BATTLEANIME_Click(object sender, EventArgs e)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            uint id = U.atoh(ar.name) - 1;

            InputFormRef.JumpTo(null, id, "BATTLEANIME", new string[] { "ANIMEID"});
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "Mant", new uint[] { 0 });

            uint p = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                FEBuilderGBA.Address.AddPointer(list
                    , p + 0
                    , 0x10
                    , "MANT_P:" + U.To0xHexString(i)
                    , FEBuilderGBA.Address.DataTypeEnum.POINTER);
            }
        }
    }
}
