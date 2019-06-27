using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageCGFE7UForm : Form
    {
        public ImageCGFE7UForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawCGAndText, DrawMode.OwnerDrawFixed);
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
                , Program.ROM.RomInfo.bigcg_pointer()
                , 16
                , (int i, uint addr) =>
                {
                    return Program.ROM.u16(addr+2)==0x00;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void BigCGFE7UForm_Load(object sender, EventArgs e)
        {

        }

        public static string GetComment(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.GetComment(id);
        }
        public static Bitmap DrawImageByID(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }
            uint typeFlag = Program.ROM.u8(addr);

            uint table = Program.ROM.u32(addr+4);
            uint tsa = Program.ROM.u32(addr + 8);
            uint palette = Program.ROM.u32(addr + 12);

            if (typeFlag == 0x00)
            {//単体
                return DrawOneImage(table, tsa, palette);
            }
            else
            {//10分割
                return ImageCGForm.DrawImage(table, tsa, palette);
            }
        }
        static Bitmap DrawOneImage(uint image, uint tsa, uint palette)
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

            return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, 20 * 8, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette), Program.ROM.Data, (int)U.toOffset(tsa));
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint typeFlag = (uint)B0.Value;

            uint table = (uint)P4.Value;
            uint tsa = (uint)P8.Value;
            uint palette = (uint)P12.Value;
            
            if (typeFlag == 0x00)
            {//単体
                X_PIC.Image = DrawOneImage(table, tsa, palette);
            }
            else
            {//10分割
                X_PIC.Image = ImageCGForm.DrawImage(table, tsa, palette);
            }
        }

        private void ImageCGFE7UForm_Load(object sender, EventArgs e)
        {
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
            int palette_count = 8;
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
            if (bitmap_palette_count <= 1)
            {//16色
                byte[] image; //画像
                byte[] tsa;   //TSA
                string error_string = ImageUtil.ImageToByteHeaderPackedTSA(bitmap, width, height, out image, out tsa);
                if (error_string != "")
                {
                    error_string += "\r\n" + DecreaseColorTSAToolForm.GetExplainDecreaseColor();
                    R.ShowStopError(error_string);
                    return;
                }

                //パレット
                byte[] palette = ImageUtil.ImageToPalette(bitmap, palette_count);

                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    //画像等データの書き込み
                    Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                    this.InputFormRef.WriteImageData(this.P4, image, true, undodata);
                    this.InputFormRef.WriteImageData(this.P8, tsa, false, undodata);
                    this.InputFormRef.WriteImageData(this.P12, palette, false, undodata);
                    Program.Undo.Push(undodata);
                }

                B0.Value = 0;  //16色 分割なし

                //ポインタの書き込み
                this.WriteButton.PerformClick();
            }
            else if (bitmap_palette_count <= 8)
            {//16色*8種類カラー
                byte[] image; //画像
                byte[] tsa;   //TSA
                string error_string = ImageUtil.ImageToByteHeaderPackedTSA(bitmap, width, height, out image, out tsa);
                if (error_string != "")
                {
                    error_string += "\r\n" + DecreaseColorTSAToolForm.GetExplainDecreaseColor();
                    R.ShowStopError(error_string);
                    return;
                }

                //パレット
                byte[] palette = ImageUtil.ImageToPalette(bitmap, palette_count);

                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    //画像等データの書き込み
                    Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                    this.InputFormRef.WriteImageData10(this.P4, image, undodata);
                    this.InputFormRef.WriteImageData(this.P8, tsa, false, undodata);
                    this.InputFormRef.WriteImageData(this.P12, palette, false, undodata);
                    Program.Undo.Push(undodata);
                }
                B0.Value = 1; //10分割 16色*8種類

                //ポインタの書き込み
                this.WriteButton.PerformClick();
            }
            else 
            {
                string error = R._("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count) + "\r\n" + DecreaseColorTSAToolForm.GetExplainDecreaseColor();
                R.ShowStopError(error);
                return;
            }

        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawImageByID((uint)this.AddressList.SelectedIndex);
            uint typeFlag = (uint)B0.Value;

            if (typeFlag == 0x00)
            {//単体
                ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(), 8);
            }
            else
            {//10分割
                ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(), 8);
            }
        }

        private void DecreaseColorTSAToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "CG", new uint[] { 4, 8,12 });

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                string name = "CG " + U.To0xHexString(i);
                uint flag = Program.ROM.u8(0 + addr);

                uint image = Program.ROM.p32(4 + addr);
                if (flag != 1)
                {//16色
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 4
                        , name + " IMAGE"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);

                    FEBuilderGBA.Address.AddHeaderTSAPointer(list
                        , addr + 8
                        , name + " TSA"
                        , isPointerOnly);

                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 12
                        , 0x20 * 1
                        , name + " PALETTE"
                        , FEBuilderGBA.Address.DataTypeEnum.PAL);
                }
                else
                {//10分割
                    {
                        uint imageSPZ = image;
                        for (int n = 0; n < 10; n++, imageSPZ += 4)
                        {
                            FEBuilderGBA.Address.AddLZ77Pointer(list
                                , imageSPZ
                                , name + " IMAGE"
                                , isPointerOnly
                                , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                        }
                    }
                    FEBuilderGBA.Address.AddAddress(list,image
                        , 4 * 10
                        , addr + 4
                        , name + " IMAGE_HEADER"
                        , FEBuilderGBA.Address.DataTypeEnum.POINTER);

                    FEBuilderGBA.Address.AddHeaderTSAPointer(list
                        , addr + 8
                        , name + " TSA"
                        , isPointerOnly);

                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 12
                        , 0x20 * 8
                        , name + " PALETTE"
                        , FEBuilderGBA.Address.DataTypeEnum.PAL);
                }
            }
        }
        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

    }
}
