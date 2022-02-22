using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OPClassDemoForm : Form
    {
        public OPClassDemoForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N1_InputFormRef = N1_Init(this);
            this.N1_AddressList.OwnerDraw(ListBoxEx.DrawOPClassFontAndText, DrawMode.OwnerDrawFixed);
            this.N1_InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.N2_InputFormRef = N2_Init(this);
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.op_class_demo_pointer()
                , 28
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr + 0xF) <= 4;   
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(addr+14);
                    return U.ToHexString(id) + " " + ClassForm.GetClassName(id);
                }
                );
        }
        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {
            return new InputFormRef(self
                , "N1_"
                , 0
                , 1
                , (int i, uint addr) =>
                {
                    return (Program.ROM.u8(addr)!=0xFF)
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(addr);
                    return U.ToHexString(id);
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

        private void ClassOPDemoForm_Load(object sender, EventArgs e)
        {
        }

        private void P8_ValueChanged(object sender, EventArgs e)
        {
            N1_InputFormRef.ReInit((uint)this.P8.Value , (uint)this.B12.Value);
        }
        private void B12_ValueChanged(object sender, EventArgs e)
        {
            P8_ValueChanged(sender, e);
        }

        private void P24_ValueChanged(object sender, EventArgs e)
        {
            N2_InputFormRef.ReInit((uint)this.P24.Value);
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
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "OPClassDemo", new uint[] { 0, 8 ,24 });

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    FEBuilderGBA.Address.AddCString(list, 0 + addr);

                    uint jpName = Program.ROM.p32(addr + 8); 
                    if ( ! U.isSafetyOffset(jpName) )
                    {
                        continue;
                    }
                    uint anime = Program.ROM.p32(addr + 24);
                    if (!U.isSafetyOffset(anime))
                    {
                        continue;
                    }

                    InputFormRef N1_InputFormRef = N1_Init(null);
                    N1_InputFormRef.ReInitPointer(addr + 8);
                    FEBuilderGBA.Address.AddAddress(list, N1_InputFormRef, "OPClassDemo_JPName", new uint[] {  });

                    InputFormRef N2_InputFormRef = N2_Init(null);
                    N2_InputFormRef.ReInitPointer(addr + 24);
                    FEBuilderGBA.Address.AddAddress(list, N2_InputFormRef, "OPClassDemo_Anime", new uint[] {  });
                }
            }
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.OP_CLASS_DEMO, InputFormRef, new uint[] { 4 });
        }

        private void B16_ValueChanged(object sender, EventArgs e)
        {
            if (B13.Value == 0xFF)
            {//標準パレット
                X_BATTLEANIMEICON.Image = ImageBattleAnimeForm.DrawBattleAnime((uint)B16.Value + 1
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90, 0, 0, 0, (int)B15.Value);
            }
            else
            {
                X_BATTLEANIMEICON.Image = ImageBattleAnimeForm.DrawBattleAnime((uint)B16.Value + 1
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B13.Value + 1, 0, 0, (int)B15.Value);
            }

        }


    }
}
