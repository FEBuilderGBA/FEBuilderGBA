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
    public partial class ImageBGForm : Form
    {
        const int ICON_WIDTH = 30; //横長で少し大き目で描画します.
        const int ICON_HEGITH = 24;

        public ImageBGForm()
        {
            InitializeComponent();

            this.AddressList.ItemHeight = ICON_HEGITH;
            this.AddressList.OwnerDraw(ListBoxEx.DrawBGAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.bg_pointer()
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    //0 と 4 がポインタであればデータがあると考える.
                    return U.isPointerOrNULL(Program.ROM.u32(addr + 0))
                        && U.isPointerOrNULL(Program.ROM.u32(addr + 4));
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageBGForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_BG_PIC.Image = DrawBG((uint)D0.Value, (uint)D8.Value, (uint)D4.Value);
        }

        public static string GetComment(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.GetComment(id);
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

            return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, 20 * 8, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette), Program.ROM.Data, (int)U.toOffset(tsa));
        }
        public static Bitmap DrawBG(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
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
            Bitmap bitmap = DrawBG((uint)this.AddressList.SelectedIndex);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(), 8);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            int width = 32 * 8;
            int height = 20 * 8;
            int palette_count = 8;
            Bitmap bitmap = ImageUtil.LoadAndConvertDecolorUI(this, null, width, height, true, palette_count);
            if (bitmap == null)
            {
                return;
            }

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
                this.InputFormRef.WriteImageData(this.D0, image, true, undodata);
                this.InputFormRef.WriteImageData(this.D4, tsa, false, undodata);
                this.InputFormRef.WriteImageData(this.D8, palette, false, undodata);
                Program.Undo.Push(undodata);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "BG", new uint[] { 0 , 4 , 8 });

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize )
            {
                string name = "BG " + U.To0xHexString(i);

                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , addr + 0
                    , name + " IMAGE"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddHeaderTSAPointer(list
                    , addr + 4
                    , name + " TSA"
                    , isPointerOnly
                    );
                FEBuilderGBA.Address.AddPointer(list
                    , addr + 8
                    , 0x20 * 8
                    , name + " PALETTE"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
        }



        private void DecreaseColorTSAToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
        }
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.BG, U.NOT_FOUND
                    , R._("BGが極端に少ないです。破損している可能性があります。")));
            }

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint image = Program.ROM.p32(0 + addr);
                if (U.isSafetyOffset(image))
                {
                    FELint.CheckLZ77Errors(image, errors, FELint.Type.BG, addr, (uint)i);
                }
            }
        }

    }
}
