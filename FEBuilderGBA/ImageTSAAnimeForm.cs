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
    public partial class ImageTSAAnimeForm : Form
    {
        public ImageTSAAnimeForm()
        {
            InitializeComponent();
#if DEBUG
            ImageTSAAnimeForm.PreLoadResource();
#endif

            this.InputFormRef = Init(this);

            this.TSAANimeList.BeginUpdate();
            this.TSAANimeList.Items.Clear();
            foreach (var pair in g_TSAAnime)
            {
                string name = U.ToHexString(pair.Key) + " " + U.at(pair.Value, 1);
                this.TSAANimeList.Items.Add(name);
            }
            this.TSAANimeList.EndUpdate();
            U.SelectedIndexSafety(this.TSAANimeList, 0, true);
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
        static Dictionary<uint, string[]> g_TSAAnime;
        public static void PreLoadResource()
        {
            g_TSAAnime = U.LoadTSVResource(U.ConfigDataFilename("tsaanime_"));
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4*3
                , (int i, uint addr) =>
                {
                    uint p = Program.ROM.u32(addr);
                    return U.isPointer(p);
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
            uint pointer = U.atoh(this.TSAANimeList.Text);
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
            uint count = U.atoh(U.at(sp, 0));

            this.InputFormRef.ReInitPointer(pointer, count);
            //this.InputFormRef.ReInit(addr, count);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_BG_PIC.Image = DrawTSAAnime((uint)P0.Value, (uint)P4.Value, (uint)P8.Value);
        }


        public static Bitmap DrawTSAAnime(uint image, uint palette, uint tsa)
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
            byte[] tsaUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(tsa));
            int height = ImageUtil.CalcHeightbyTSA(32 * 8, tsaUZ.Length);
            Debug.Assert(height >= 20*8);

            return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, height, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette), tsaUZ, 0);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawTSAAnime((uint)P0.Value, (uint)P4.Value, (uint)P8.Value);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(), 8);
        }

        int GetThisImageHeight()
        {
            uint tsa = (uint)P8.Value;
            if (!U.isPointer(tsa))
            {
                return 20 * 8;
            }
            byte[] tsaUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(tsa));
            int height = ImageUtil.CalcHeightbyTSA(32 * 8, tsaUZ.Length);
            Debug.Assert(height >= 20 * 8);

            return height;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 32 * 8;
            int height = GetThisImageHeight();
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
            if (bitmap_palette_count > palette_count)
            {
                string error = R._("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count) + "\r\n" + DecreaseColorTSAToolForm.GetExplainDecreaseColor();
                R.ShowStopError(error);
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
                this.InputFormRef.WriteImageData(this.P0, image, true, undodata);
                this.InputFormRef.WriteImageData(this.P4, palette, false, undodata);
                this.InputFormRef.WriteImageData(this.P8, tsa, true, undodata);
                Program.Undo.Push(undodata);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
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
                uint count = U.atoh(U.at(pair.Value, 0));
                string basename = "TSAANIME " + U.at(pair.Value, 1) + " ";
                InputFormRef.ReInit(addr, count);

                FEBuilderGBA.Address.AddAddress(list, InputFormRef, basename, new uint[] { 0 , 4, 8});

                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    string name = basename + "" + U.To0xHexString(i);

                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 0
                        , name + " IMAGE"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 4
                        , 0x20 * 8
                        , name + " PALETTE"
                        , FEBuilderGBA.Address.DataTypeEnum.PAL);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 8
                        , name + " TSA"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77TSA);
                }
            }
        }



        private void DecreaseColorTSAToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
        }

        private void GraphicsToolButton_Click(object sender, EventArgs e)
        {
            uint image = U.toOffset(P0.Value);
            uint palette = U.toOffset(P4.Value);
            uint tsa = U.toOffset(P8.Value);

            int width = 32 * 8;
            int height = GetThisImageHeight();
            int palette_count = 8;
            GraphicsToolForm f = (GraphicsToolForm)InputFormRef.JumpFormLow<GraphicsToolForm>();
            f.Jump(width
                , height
                , image
                , 0
                , tsa
                , 2
                , palette
                , 0
                , palette_count
                , 0);
            f.Show();
        }
    }
}
