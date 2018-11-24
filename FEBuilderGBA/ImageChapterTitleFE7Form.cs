using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageChapterTitleFE7Form : Form
    {
        public ImageChapterTitleFE7Form()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(ExportButton1, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton1, Properties.Resources.icon_upload);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.image_chapter_title_pointer()
                , 4
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


        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.X_SAVE_PIC.Image = DrawPic((uint)P0.Value, 8 * 32);
        }
        public static Bitmap DrawPic(uint addr,int width)
        {
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(addr));
            if (imageUZ.Length <= 0)
            {//ない
                return ImageUtil.Blank(width, 8);
            }

            int height = ImageUtil.CalcHeight(width, imageUZ.Length);
            uint palette = Program.ROM.RomInfo.image_chapter_title_palette();

            return ImageUtil.ByteToImage16Tile(width, height
                , imageUZ, 0
                , Program.ROM.Data, (int)U.toOffset(palette)
                , 0
            );
        }

        private void ImageChapterTitleFE7Form_Load(object sender, EventArgs e)
        {

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


        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            string name = "ChapterTitleImage";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 });

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize)
                {
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 0
                        , name + U.To0xHexString(i)
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
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
