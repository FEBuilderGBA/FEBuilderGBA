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
    public partial class ImageTSAAnime2Form : Form
    {
        public ImageTSAAnime2Form()
        {
            InitializeComponent();
#if DEBUG
            ImageTSAAnimeForm.PreLoadResource();
#endif
            this.InputFormRef = Init(this);
            this.N1_InputFormRef = N1_Init(this);

            this.TSAANime2List.BeginUpdate();
            this.TSAANime2List.Items.Clear();
            foreach (var pair in g_TSAAnime)
            {
                string name = U.ToHexString(pair.Key) + " " + U.at(pair.Value, 0);
                this.TSAANime2List.Items.Add(name);
            }
            this.TSAANime2List.EndUpdate();
            U.SelectedIndexSafety(this.TSAANime2List, 0, true);
        }
        static Dictionary<uint, string[]> g_TSAAnime;
        public static void PreLoadResource()
        {
            g_TSAAnime = U.LoadTSVResource(U.ConfigDataFilename("tsaanime2_"));
        }


        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 12
                , (int i, uint addr) =>
                {
                    uint p = Program.ROM.u32(addr+8);
                    return U.isPointer(p);
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }
        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {
            return new InputFormRef(self
                , "N1_"
                , 0
                , 20
                , (int i, uint addr) =>
                {
                    return i < 1;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        private void ImageTSAAnimeForm_Load(object sender, EventArgs e)
        {

        }

        private void TSAANimeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint pointer = U.atoh(this.TSAANime2List.Text);
            string[] sp;
            if (!g_TSAAnime.TryGetValue(pointer, out sp))
            {
                return;
            }
            pointer = U.toOffset(pointer);
            if (!U.isSafetyOffset(pointer))
            {
                return;
            }
            uint addr = Program.ROM.p32( pointer );
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            this.N1_InputFormRef.ReInitPointer(pointer, 1);
            this.InputFormRef.ReInit(addr + 20); //ヘッダーの20バイト以降にTSAのデータがある
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_ANIME_PIC.Image = DrawTSAAnime2((uint)N1_P16.Value, (uint)N1_P4.Value, (uint)P8.Value);
        }


        static Bitmap DrawTSAAnime2(uint image, uint palette, uint tsa)
        {
            if (!U.isPointer(image))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isPointer(palette))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isPointer(tsa))
            {
                return ImageUtil.BlankDummy();
            }

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));
            int height = (int)ImageUtil.CalcByteLengthForHeaderTSAData(Program.ROM.Data, (int)U.toOffset(tsa));

            return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, height, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette), Program.ROM.Data, (int)U.toOffset(tsa));
        }

        private void N1_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_IMG_HEADER.Image = DrawTSAAnime2Header((uint)N1_P16.Value, (uint)N1_P4.Value);
        }
        static Bitmap DrawTSAAnime2Header(uint image, uint palette)
        {
            if (!U.isPointer(image))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isPointer(palette))
            {
                return ImageUtil.BlankDummy();
            }

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));
            int height = (int)ImageUtil.CalcHeight(30*8 , imageUZ.Length );

            return ImageUtil.ByteToImage16Tile(30 * 8, height, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette));
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef N1_InputFormRef = N1_Init(null);
            foreach (var pair in g_TSAAnime)
            {
                uint pointer = pair.Key;
                pointer = U.toOffset(pointer);
                if (!U.isSafetyOffset(pointer))
                {
                    continue;
                }
                uint addr = Program.ROM.p32(pointer);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                string basename = "TSAANIME2 " + U.at(pair.Value, 0) + " ";

                N1_InputFormRef.ReInitPointer(pointer, 1);
                InputFormRef.ReInit(addr + 20); //ヘッダーの20バイト以降にTSAのデータがある

                FEBuilderGBA.Address.AddAddress(list, InputFormRef, basename, new uint[] { 8});
                FEBuilderGBA.Address.AddAddress(list, N1_InputFormRef, basename, new uint[] { 4,16 });
                if (N1_InputFormRef.DataCount >= 1)
                {
                    string name = basename + " HEADER";

                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 16
                        , name + " IMAGE"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 4
                        , 0x20
                        , name + " PALETTE"
                        , FEBuilderGBA.Address.DataTypeEnum.PAL);
                }

                addr += 20; //SkipHeader
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    string name = basename + "" + U.To0xHexString(i);

                    FEBuilderGBA.Address.AddHeaderTSAPointer(list
                        , addr + 8
                        , name + " TSA"
                        , isPointerOnly
                    );
                }
            }
        }


    }
}
