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
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "OPClassDemo", new uint[] { 0 , 8 ,28});

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    string name = "OPClassDemo_Anime_" + U.ToHexString(i);
                    FEBuilderGBA.Address.AddCString(list, 0 + addr);

                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 8
                        , name + "_JP_NAME_IMG"
                        , isPointerOnly 
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);

                    uint anime = Program.ROM.p32(addr + 28);
                    if (!U.isSafetyOffset(anime))
                    {
                        continue;
                    }

                    InputFormRef N2_InputFormRef = N2_Init(null);
                    N2_InputFormRef.ReInitPointer(addr + 28);
                    FEBuilderGBA.Address.AddAddress(list, N2_InputFormRef, name + "_Anime", new uint[] { });
                }
            }

            FEBuilderGBA.Address.AddPointer(list
                , JP_FONT_PALETTE_POINTER
                , 2 * 16
                , "OPClassDemo_CommonPalette"
                , FEBuilderGBA.Address.DataTypeEnum.PAL);
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.OP_CLASS_DEMO, InputFormRef, new uint[] { 4 });
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

        const uint JP_FONT_PALETTE_POINTER = 0x0B0038;
        private void P8_ValueChanged(object sender, EventArgs e)
        {
            uint addr = U.toOffset(P8.Value);
            if (!U.isSafetyOffset(addr))
            {
                X_NAME_IMG.Image = null;
                return ;
            }
            byte[] img = LZ77.decompress(Program.ROM.Data,addr);
            if (img.Length <= 0)
            {
                X_NAME_IMG.Image = null;
                return;
            }
            uint paletteAddr = Program.ROM.p32(JP_FONT_PALETTE_POINTER);
            if (!U.isSafetyOffset(paletteAddr))
            {
                X_NAME_IMG.Image = null;
                return;
            }
            X_NAME_IMG.Image = ImageUtil.ByteToImage16Tile(32 * 8, 4 * 8, img, 0, Program.ROM.Data, (int)paletteAddr);
        }

        private void X_GOTO_GRAPHICS_TOOL_JP_NAME_Click(object sender, EventArgs e)
        {
            uint addr = U.toOffset(P8.Value);
            uint paletteAddr = Program.ROM.p32(JP_FONT_PALETTE_POINTER);

            GraphicsToolForm f = (GraphicsToolForm)InputFormRef.JumpForm<GraphicsToolForm>(U.NOT_FOUND);
            f.Jump(32 * 8, 4 * 8, addr, 0, 0, 0, paletteAddr, 0, 1 , 0);
        }
    }
}
