using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OPPrologueForm : Form
    {
        public OPPrologueForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
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
                , Program.ROM.RomInfo.op_prologue_image_pointer
                , 12
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        private void OPPrologueForm_Load(object sender, EventArgs e)
        {

        }

        public static Bitmap DrawImage(uint image, uint tsa)
        {
            if (!U.isPointer(image))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isPointer(tsa))
            {
                return ImageUtil.BlankDummy();
            }
            uint palette =
                Program.ROM.p32(Program.ROM.RomInfo.op_prologue_palette_color_pointer);

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));
            byte[] tsaUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(tsa));
            return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, 20 * 8, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette), tsaUZ, 0);
        }
        public static Bitmap DrawImageByID(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }

            uint image = Program.ROM.u32(addr);
            uint tsa = Program.ROM.u32(addr + 4);

            return DrawImage(image, tsa);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_PIC.Image = DrawImage((uint)P0.Value, (uint)P4.Value);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawImageByID((uint)this.AddressList.SelectedIndex);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(), 1);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 32 * 8;
            int height = 20 * 8;
            int palette_count = 1;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                if (bitmap.Width != 30 * 8 || bitmap.Height != height)
                {
                    R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                    return;
                }
                //右側に余白がない場合、自動的に挿入する
                bitmap = ImageUtil.Copy(bitmap, 0, 0, width, height);
            }
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (bitmap_palette_count > palette_count)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return;
            }

            byte[] image; //画像
            byte[] tsa;   //TSA
            string error_string = ImageUtil.ImageToByteHeaderPackedTSA(bitmap, width, height, out image, out tsa);
            if (error_string != "")
            {
                R.ShowStopError(error_string);
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData(this.P0, image, true, undodata);
                this.InputFormRef.WriteImageData(this.P4, tsa, true, undodata);
                Program.Undo.Push(undodata);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            string name = "OPPrologue";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 , 4});

                FEBuilderGBA.Address.AddPointer(list
                    , Program.ROM.RomInfo.op_prologue_palette_color_pointer
                    , 2 * 16
                    , name + " Palette"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);

                
                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {

                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 0
                        , name + " image"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 4
                        , name + " tsa"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);
                }
            }
        }
    }
}
