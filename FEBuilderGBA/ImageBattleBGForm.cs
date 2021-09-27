using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class ImageBattleBGForm : Form
    {
        public ImageBattleBGForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.battle_bg_pointer()
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    //0 と 4 がポインタであればデータがあると考える.
                    return U.isPointer(Program.ROM.u32(addr + 0))
                        && U.isPointer(Program.ROM.u32(addr + 4));
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    return U.ToHexString(i + 1) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageBattleBGForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_BG_PIC.Image = DrawBG((uint)D0.Value, (uint)D8.Value, (uint)D4.Value);
            U.ConvertListBox(MapTerrainBGLookupTableForm.MakeListByUseTerrain((uint)this.AddressList.SelectedIndex), ref this.X_REF);
        }

        public static Bitmap DrawBG(uint image, uint palette,uint tsa)
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
            byte[] paletteUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(palette));
            byte[] tsaUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(tsa));

            return ImageUtil.ByteToImage16Tile(30 * 8, 20 * 8
                , imageUZ , 0
                , paletteUZ, 0
                , tsaUZ, 0
                );
        }
        public static Bitmap DrawBG(uint id)
        {   
            if (id <= 0)
            {
                return ImageUtil.BlankDummy();
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id - 1);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }

            uint image = Program.ROM.u32(addr);
            uint palette = Program.ROM.u32(addr+8);
            uint tsa = Program.ROM.u32(addr+4);

            return DrawBG(image, palette, tsa);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawBG((uint)this.AddressList.SelectedIndex+1);
            int palette_count = ImageUtil.GetPalette16Count(bitmap);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(), palette_count);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            int width = 30 * 8;
            int height = 20 * 8;
            int palette_count = 8;
            Bitmap bitmap = ImageUtil.LoadAndConvertDecolorUI(this, null, 0, height, true, palette_count);
            if (bitmap == null)
            {
                return;
            }
            if (bitmap.Width > width || bitmap.Height > height)
            {//幅サイズが超えていたら削り落とす.
                Bitmap newBitmap = ImageUtil.Blank(width, height , bitmap);
                ImageUtil.BitBlt(newBitmap, 0, 0, width, height, bitmap, 0, 0);
                bitmap = newBitmap;
            }


            byte[] image; //画像
            byte[] tsa;   //TSA
            string error_string = ImageUtil.ImageToBytePackedTSA(bitmap, width, height,0, out image, out tsa);
            if (error_string != "")
            {
                R.ShowStopError(error_string);
                return;
            }

            //パレット
            byte[] palette = ImageUtil.ImageToPalette(bitmap, palette_count);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData(this.D0, image, true, undodata);
                this.InputFormRef.WriteImageData(this.D4, tsa, true, undodata);
                this.InputFormRef.WriteImageData(this.D8, palette, true, undodata);
                Program.Undo.Push(undodata);
            }
            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }
        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            string selfname = "BattleBG";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, selfname, new uint[] { 0 ,4 ,8});

            uint p = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
            {
                selfname = "BattleBG " + U.To0xHexString(i);

                uint image = Program.ROM.p32(0 + p);
                FEBuilderGBA.Address.AddAddress(list,image
                    , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, image)
                    , p + 0
                    , selfname
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);

                uint tsa = Program.ROM.p32(4 + p);
                FEBuilderGBA.Address.AddAddress(list,tsa
                    , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, tsa)
                    , p + 4
                    , selfname
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);

                uint palette = Program.ROM.p32(8 + p);
                FEBuilderGBA.Address.AddAddress(list,palette
                    , isPointerOnly ? 0 : LZ77.getCompressedSize(Program.ROM.Data, palette)
                    , p + 8
                    , selfname
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77PAL);
            }
        }

        private void DecreaseColorTSAToolButton_Click(object sender, EventArgs e)
        {
            DecreaseColorTSAToolForm f = (DecreaseColorTSAToolForm)InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
            f.InitMethod(2);
        }
        public static string GetName(uint id)
        {
            if (id <= 0)
            {
                return "null";
            }
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.GetComment(id - 1);
        }

        void JumpToRef()
        {
            MapTerrainBGLookupTableForm.JumpToRef(X_REF.Text);
        }
        private void X_REF_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            JumpToRef();
        }

        private void X_REF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                JumpToRef();
            }
        }

        private void GraphicsToolButton_Click(object sender, EventArgs e)
        {
            uint id = (uint)this.AddressList.SelectedIndex + 1;

            if (id <= 0)
            {
                return;
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id - 1);
            if (addr == U.NOT_FOUND)
            {
                return;
            }

            uint image = Program.ROM.u32(addr);
            uint palette = Program.ROM.u32(addr + 8);
            uint tsa = Program.ROM.u32(addr + 4);

            int width = 30 * 8;
            int height = 20 * 8;
            GraphicsToolForm f = (GraphicsToolForm)InputFormRef.JumpFormLow<GraphicsToolForm>();
            f.Jump(width
                , height
                , image
                , 0
                , tsa
                , 1
                , palette
                , 1
                , 8
                , 0);
            f.Show();
        }

    }
}
