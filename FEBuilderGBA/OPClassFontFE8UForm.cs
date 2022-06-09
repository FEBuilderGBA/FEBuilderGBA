using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OPClassFontFE8UForm : Form
    {
        public OPClassFontFE8UForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.AddressList.OwnerDraw(ListBoxEx.DrawOPClassFontAndText, DrawMode.OwnerDrawFixed);

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
                , Program.ROM.RomInfo.op_class_font_pointer
                , 4
                , (int i, uint addr) =>
                {
                    return i <= 0x7a;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        private void ClassOPFontFE8UForm_Load(object sender, EventArgs e)
        {

        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.X_PIC.Image = DrawFont((uint)P0.Value);
        }

        public static Bitmap DrawFont(uint image)
        {
            if (!U.isPointer(image))
            {
                return ImageUtil.BlankDummy(32);
            }
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.op_class_font_palette_pointer); 

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));

            return ImageUtil.ByteToImage16Tile(2 * 8, 4 * 8
                , imageUZ, 0
                , Program.ROM.Data, (int)U.toOffset(palette)
                );
        }
        public static Bitmap DrawFontByID(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }

            uint image = Program.ROM.u32(addr);
            return DrawFont(image);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            string name = "OPClassFont";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 });

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    name = "OPClassFont " + U.To0xHexString(i);
                    uint a = Program.ROM.p32(p);
                    if (a <= 0)
                    {
                        continue;
                    }
                    uint length = LZ77.getCompressedSize(Program.ROM.Data, a);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , p
                        , name
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                }
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawFontByID((uint)this.AddressList.SelectedIndex);
            ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            int width = 2 * 8;
            int height = 4 * 8;
            Bitmap bitmap = ImageUtil.LoadAndCheckPaletteUI(this
                , ImageUtil.MakePaletteHintP(Program.ROM.RomInfo.op_class_font_palette_pointer)
                , width, height);
            if (bitmap == null)
            {
                return;
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

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
    }
}
