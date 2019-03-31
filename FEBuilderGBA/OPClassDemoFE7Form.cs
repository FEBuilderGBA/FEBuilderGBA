using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OPClassDemoFE7Form : Form
    {
        public OPClassDemoFE7Form()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N2_InputFormRef = N2_Init(this);
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.op_class_demo_pointer()
                , 32
                , (int i, uint addr) =>
                {
                    return i <= 0x41;
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(addr+15);
                    return U.ToHexString(id) + " " + ClassForm.GetClassName(id);
                }
                );
        }
        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {
            return new InputFormRef(self
                , "N2_"
                , 0
                , 6
                , (int i, uint addr) =>
                {
                    return i < 1; //1つだけ
                        ;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        private void ClassOPDemoFE7Form_Load(object sender, EventArgs e)
        {

        }

        private void P24_ValueChanged(object sender, EventArgs e)
        {
            N2_InputFormRef.ReInit((uint)this.P28.Value);
        }

        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "OPClassDemo", new uint[] { 0 , 8 ,28});

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    FEBuilderGBA.Address.AddCString(list, 0 + addr);

                    uint jpName = Program.ROM.p32(addr + 8);
                    if (!U.isSafetyOffset(jpName))
                    {
                        continue;
                    }
                    uint anime = Program.ROM.p32(addr + 28);
                    if (!U.isSafetyOffset(anime))
                    {
                        continue;
                    }

                    InputFormRef N2_InputFormRef = N2_Init(null);
                    N2_InputFormRef.ReInitPointer(addr + 28);
                    FEBuilderGBA.Address.AddAddress(list, N2_InputFormRef, "OPClassDemo_Anime", new uint[] { });
                }
            }
        }
        public static void MakeTextIDArray(List<UseTextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseTextID.AppendTextID(list, FELint.Type.OP_CLASS_DEMO, InputFormRef, new uint[] { 4 });
        }

        private void B17_ValueChanged(object sender, EventArgs e)
        {
            if (B14.Value == 0xFF)
            {//標準パレット
                X_BATTLEANIMEICON.Image = ImageBattleAnimeForm.DrawBattleAnime((uint)B17.Value + 1
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90, 0, 0, 0, (int)B16.Value);
            }
            else
            {
                X_BATTLEANIMEICON.Image = ImageBattleAnimeForm.DrawBattleAnime((uint)B17.Value + 1
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B14.Value + 1, 0, 0, (int)B16.Value);
            }

        }
    }
}
