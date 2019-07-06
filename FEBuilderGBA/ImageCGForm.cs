using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageCGForm : Form
    {
        const int ICON_WIDTH = 30; //横長で少し大き目で描画します.
        const int ICON_HEGITH = 24;
        public ImageCGForm()
        {
            InitializeComponent();

            this.AddressList.ItemHeight = ICON_HEGITH;
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
                , 12
                , (int i, uint addr) =>
                {
                    uint p = Program.ROM.u32(addr);
                    if (!U.isPointer(p) || !U.isSafetyPointer(p))
                    {
                        return false;
                    }
                    //FE7だと、ほかのポインタも混ざってしまうので、
                    //10分割イメージの特徴である 画像ポインタの先に10個ポインタがあるを利用する.
                    uint p2 = Program.ROM.u32(U.toOffset(p));
                    if (!U.isPointer(p2) || !U.isSafetyPointer(p2))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void BigCGForm_Load(object sender, EventArgs e)
        {

        }
        public static Bitmap DrawImage(uint table, uint tsa, uint palette)
        {
            if (!U.isPointer(table))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isPointer(tsa))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isPointer(palette))
            {
                return ImageUtil.BlankDummy();
            }

            table = U.toOffset(table);

            List<byte> imageUZList = new List<byte>();
            for (int i = 0; i < 10; i++)
            {
                uint image = Program.ROM.u32((uint)(table + (i*4)));
                if (!U.isPointer(image))
                {
                    return ImageUtil.BlankDummy();
                }
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));
                imageUZList.AddRange(imageUZ);
            }
            return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, 20 * 8, imageUZList.ToArray(), 0, Program.ROM.Data, (int)U.toOffset(palette), Program.ROM.Data, (int)U.toOffset(tsa));
        }
        public static Bitmap DrawImageByID(uint id)
        {
            if (Program.ROM.RomInfo.version() == 7
                && !Program.ROM.RomInfo.is_multibyte())
            {//FE7Uだけ別ルーチン.
                return ImageCGFE7UForm.DrawImageByID(id);
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }

            uint table = Program.ROM.u32(addr);
            uint tsa = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);

            return DrawImage(table, tsa, palette);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_PIC.Image = DrawImage((uint)P0.Value, (uint)P4.Value,(uint)P8.Value);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawImageByID((uint)this.AddressList.SelectedIndex);
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
                //パレット領域が他の領域を浸食していないか確認する
                if (IsPaletteDuplicate((uint)this.P8.Value, (uint)this.AddressList.SelectedIndex))
                {
                    //FE8のパレットはバグっていて、となりのパレットに浸食している.
                    this.P8.Value = 0;
                }

                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData10(this.P0, image, undodata);
                this.InputFormRef.WriteImageData(this.P4, tsa, false, undodata);
                this.InputFormRef.WriteImageData(this.P8, palette, false, undodata);
                Program.Undo.Push(undodata);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }


        private void DecreaseColorTSAToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
        }

        //パレット領域が他の領域を浸食していないか確認する
        //FE8のパレットはバグっていて、となりのパレットに浸食している.
        public static bool IsPaletteDuplicate(uint paletteAddrP , uint currentIndex)
        {
            paletteAddrP = U.toPointer(paletteAddrP);
            uint paletteEndAddrP = paletteAddrP + (0x20 * 8);

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                if (i == currentIndex)
                {//自分自身なので無視.
                    continue;
                }
                uint pal = Program.ROM.u32(addr + 8);
                if (pal >= paletteAddrP && pal < paletteEndAddrP)
                {//重複している
                    return true;
                }
            }
            return false;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "CG", new uint[] { 0 ,4, 8 });

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                string name = "CG " + U.To0xHexString(i);

                uint image = Program.ROM.p32(0 + addr);
                {
                    uint imageSPZ = image;
                    for (int n = 0; n < 10; n++, imageSPZ += 4)
                    {
                        FEBuilderGBA.Address.AddLZ77Pointer(list
                            , imageSPZ
                            , name + " IMAGE@" + n
                            , isPointerOnly
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    }
                }
                FEBuilderGBA.Address.AddAddress(list, image
                    , 4 * 10
                    , addr + 0
                    , name + " IMAGE_HEADER"
                    , FEBuilderGBA.Address.DataTypeEnum.POINTER);

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

        public static List<U.AddrResult> MakeList()
        {
            if (Program.ROM.RomInfo.version() == 7
                && !Program.ROM.RomInfo.is_multibyte())
            {//FE7Uだけ別ルーチン.
                return ImageCGFE7UForm.MakeList();
            }
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        public static string GetComment(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.GetComment(id);
        }
    }
}
