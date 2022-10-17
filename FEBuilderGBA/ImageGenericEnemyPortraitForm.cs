using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageGenericEnemyPortraitForm : Form
    {
        public ImageGenericEnemyPortraitForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.CheckProtectionAddrHigh = false;
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);

            InputFormRef.markupJumpLabel(ExtendsBanner);
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
                , Program.ROM.RomInfo.generic_enemy_portrait_pointer
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < Program.ROM.RomInfo.generic_enemy_portrait_count
                        ;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addr = InputFormRef.SelectToAddr(this.AddressList);
            if (addr == U.NOT_FOUND)
            {
                return;
            }

            uint img = Program.ROM.u32(addr);
            uint pal = Program.ROM.u32(addr + (8 * 4));

            this.X_PALETTE.Value = pal;

            X_BG_PIC.Image = Draw(img,pal);
        }
        public static Bitmap Draw(uint image, uint palette)
        {
            if (!U.isPointer(image))
            {
                return ImageUtil.BlankDummy();
            }
            if (!U.isPointer(palette))
            {
                return ImageUtil.BlankDummy();
            }

            return ImageUtil.ByteToImage16Tile(4 * 8, 4 * 8
                , Program.ROM.Data, (int)U.toOffset(image)
                , Program.ROM.Data, (int)U.toOffset(palette)
                );
        }
        public static Bitmap Draw(uint id)
        {
            uint extends_pointer = U.NOT_FOUND;
            if (PatchUtil.SearchGenericEnemyPortraitExtendsPatch(out extends_pointer))
            {
                uint offset = Program.ROM.p32(extends_pointer) + id * 8;
                if (! U.isSafetyOffset(offset + 4 + 3))
                {
                    return ImageUtil.BlankDummy();
                }
                uint img = Program.ROM.u32(offset);
                uint pal = Program.ROM.u32(offset + 4);
                return Draw(img, pal);
            }
            else
            {
                InputFormRef ifr = Init(null);
                uint addr = ifr.IDToAddr(id);
                if (addr == U.NOT_FOUND)
                {
                    return ImageUtil.BlankDummy();
                }

                uint img = Program.ROM.u32(addr);
                uint pal = Program.ROM.u32(addr + (8 * 4));
                return Draw(img, pal);
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            uint addr = InputFormRef.SelectToAddr(this.AddressList);
            if (addr == U.NOT_FOUND)
            {
                return;
            }

            uint img = Program.ROM.u32(addr);
            uint pal = Program.ROM.u32(addr + (8 * 4));

            Bitmap bitmap = Draw(img, pal);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            int width = 8 * 4;
            int height = 8 * 4; //32
            int palette_count = 1;
            Bitmap bitmap = ImageUtil.LoadAndConvertDecolorUI(this, null, width, height, true, palette_count);
            if (bitmap == null)
            {
                return;
            }
            uint baseaddr = InputFormRef.SelectToAddr(this.AddressList);
            if (baseaddr == U.NOT_FOUND)
            {
                return;
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);
            byte[] palette = ImageUtil.ImageToPalette(bitmap, 1);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            this.InputFormRef.WriteImageData(this.D0, image, false, undodata);

            {
                InputFormRef palDummyIFR = Init(null);
                palDummyIFR.ReInit(this.InputFormRef.BaseAddress + (4 * 8));
                palDummyIFR.WriteImageData(this.X_PALETTE, palette, false, undodata);
                Program.ROM.write_p32(baseaddr + (8 * 4), (uint)this.X_PALETTE.Value, undodata);
            }

            Program.Undo.Push(undodata);

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
            string selfname = "GenericEnemyPortait";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddPointer(list
                , Program.ROM.RomInfo.generic_enemy_portrait_pointer
                , 8 * 2 * 4
                , selfname, FEBuilderGBA.Address.DataTypeEnum.POINTER);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize)
            {
                selfname = "GenericEnemyPortait 0x" + U.ToHexString(i);
                uint image = Program.ROM.p32(0 + addr);
                FEBuilderGBA.Address.AddPointer(list
                    ,addr 
                    ,(4*8/2)*(4*8)
                    ,selfname
                    ,FEBuilderGBA.Address.DataTypeEnum.IMG);

                FEBuilderGBA.Address.AddPointer(list
                    , 16 + addr
                    , 0x20 * 1
                    , selfname
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
        }

        private void ImageGenericEnemyPortraitForm_Load(object sender, EventArgs e)
        {
            uint extends_pointer = U.NOT_FOUND;
            if (PatchUtil.SearchGenericEnemyPortraitExtendsPatch(out extends_pointer))
            {
                this.ExtendsBanner.Text = R._("拡張パッチがインストールされているため、この設定は利用できません。パッチの方から修正してください。");
            }
            else
            {
                this.ExtendsBanner.Text = R._("もっとたくさん一般兵の顔画像を利用したい場合は、パッチを利用してください。");
            }
        }

        private void ExtendsBanner_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("Allow 254 generic minimugs", 0);
        }
    }
}
