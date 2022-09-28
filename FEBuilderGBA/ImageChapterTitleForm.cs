using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageChapterTitleForm : Form
    {
        public ImageChapterTitleForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(ExportButton1, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton1, Properties.Resources.icon_upload);
            U.SetIcon(ExportButton2, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton2, Properties.Resources.icon_upload);
            U.SetIcon(ExportButton3, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton3, Properties.Resources.icon_upload);

            if (Program.ROM.RomInfo.is_multibyte == false)
            {
                this.EXPLAIN.Text += R._("FE8Uの場合は、章テキストから画像を自動生成するパッチを利用できます。\r\nパッチ画面から、「Convert Chapter Titles to Text」で検索してください。\r\n");
            }
            this.EXPLAIN.Text += R._("セーブ画面、章画像、章タイトルの3つを変更できますが、\r\nもし面倒ならば、セーブ画面の画像だけを作成し、他の2つは0x0を指定してゲームは動作します。\r\n");
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.image_chapter_title_pointer
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    //0 がポインタであればデータがあると考える.
                    return U.isPointer(Program.ROM.u32(addr + 0))
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint map_no = (uint)(i);
                    String name = MapSettingForm.GetMapNameFromMapNo(map_no);

                    return U.ToHexString(map_no) + " " + name;
                }
                );
        }

        private void ImageChapterTitleForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.X_SAVE_PIC.Image = DrawPic((uint)P0.Value, 8 * 32);
            this.X_CHAPTER_PIC.Image = DrawPic((uint)P4.Value, 8 * 10);
            this.X_TITLE_PIC.Image = DrawPic((uint)P8.Value, 8 * 18);
        }
        public static Bitmap DrawPic(uint addr,int width)
        {
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(addr));
            if (imageUZ.Length <= 0)
            {//ない
                return ImageUtil.Blank(width, 8);
            }

            int height = ImageUtil.CalcHeight(width, imageUZ.Length);
            uint palette = Program.ROM.RomInfo.image_chapter_title_palette;

            return ImageUtil.ByteToImage16Tile(width, height
                , imageUZ, 0
                , Program.ROM.Data, (int)U.toOffset(palette)
                , 0
            );
        }
        public static Bitmap DrawSample(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if(addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }
            uint saveimage_addr = Program.ROM.p32(addr);
            if (!U.isSafetyOffset(saveimage_addr))
            {
                return ImageUtil.BlankDummy();
            }

            return DrawPic(saveimage_addr, 8 * 32);
        }

        private void ExportButton1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawPic((uint)P0.Value, 8 * 32);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 32 * 8;
            int height = 2 * 8;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width,bitmap.Height,width,height);
                return;
            }

            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , Program.ROM.RomInfo.image_chapter_title_palette
                        , U.NOT_FOUND
                        , ""
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, Program.ROM.RomInfo.image_chapter_title_palette));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, Program.ROM.RomInfo.image_chapter_title_palette));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap,width,height);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData(this.P0, image, true, undodata);
                Program.Undo.Push(undodata);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        private void ExportButton2_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawPic((uint)P4.Value, 8 * 10);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton2_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 10 * 8;
            int height = 2 * 8;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width,bitmap.Height,width,height);
                return;
            }


            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , Program.ROM.RomInfo.image_chapter_title_palette
                        , U.NOT_FOUND
                        , ""
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, Program.ROM.RomInfo.image_chapter_title_palette));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, Program.ROM.RomInfo.image_chapter_title_palette));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData(this.P4, image, true, undodata);
                Program.Undo.Push(undodata);
            }
            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        private void ExportButton3_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawPic((uint)P8.Value, 8 * 18);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton3_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 18 * 8;
            int height = 2 * 8;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width,bitmap.Height,width,height);
                return;
            }

            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , Program.ROM.RomInfo.image_chapter_title_palette
                        , U.NOT_FOUND
                        , ""
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, Program.ROM.RomInfo.image_chapter_title_palette));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, Program.ROM.RomInfo.image_chapter_title_palette));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData(this.P8, image, true, undodata);
                Program.Undo.Push(undodata);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }


        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            string name = "ChapterTitleImage";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 ,4, 8});

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 0
                        , name + "_Save"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 4
                        , name + "_Number"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 8
                        , name + "_Title"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                }
            }
        }
        public static List<U.AddrResult> MakeList()
        {
            if (Program.ROM.RomInfo.version == 7
                && !Program.ROM.RomInfo.is_multibyte)
            {//FE7Jだけ別ルーチン.
                return ImageChapterTitleFE7Form.MakeList();
            }
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }
    }
}
