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

            this.X_REF.ItemHeight = (int)(this.X_REF.Font.Height * 2.4);
            this.X_REF.OwnerDraw(InputFormRef.DrawRefTextList, DrawMode.OwnerDrawFixed, false);

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
                , Program.ROM.RomInfo.bg_pointer
                , 12
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint a0 = Program.ROM.u32(addr + 0);
                    uint a1 = Program.ROM.u32(addr + 4);
                    if (PatchUtil.BG256Color() == PatchUtil.BG256ColorPatch.BG256Color)
                    {
                        if (a1 <= 1)
                        {
                            return U.isPointerOrNULL(a0);
                        }
                    }

                    //0 と 4 がポインタであればデータがあると考える.
                    return U.isPointerOrNULL(a0) && U.isPointerOrNULL(a1);
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
            X_BG_PIC.Image = DrawBG((uint)P0.Value, (uint)P8.Value, (uint)P4.Value);
            UpdateRef((uint)this.AddressList.SelectedIndex);

            ShowWarningMessage();
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
                if (PatchUtil.BG256Color() == PatchUtil.BG256ColorPatch.BG256Color)
                {
                    byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));
                    if (tsa == 0)
                    {//256 color
                        return ImageUtil.ByteToImage256Tile(32 * 8, 20 * 8, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette));
                    }
                    else if (tsa == 1)
                    {//224 color
                        return ImageUtil.ByteToImage224BGTile(32 * 8, 20 * 8, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette));
                    }
                }

                return ImageUtil.BlankDummy();
            }

            {
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));
                return ImageUtil.ByteToImage16TileHeaderTSA(32 * 8, 20 * 8, imageUZ, 0, Program.ROM.Data, (int)U.toOffset(palette), Program.ROM.Data, (int)U.toOffset(tsa));
            }
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
            int palette_max = 8;
            if (PatchUtil.BG256Color() == PatchUtil.BG256ColorPatch.BG256Color)
            {
                if (P4.Value <= 1)
                {
                    palette_max = 16;
                }
            }
            ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename(), palette_max);
        }

        void ImportButton255(ImageBGSelectPopupForm.SelectedType selectType)
        {
            int width = 32 * 8;
            int height = 20 * 8;
            int palette_count = 16; //255 color
            Bitmap bitmap = ImageUtil.LoadAndConvertDecolorUI(this, null, width, height, false, palette_count);
            if (bitmap == null)
            {
                return;
            }

            byte[] image = ImageUtil.ImageToByte256Tile(bitmap, width, height);
            byte[] tsa = new byte[0];

            //パレット
            byte[] palette = ImageUtil.ImageToPalette(bitmap, palette_count);
            if (selectType == ImageBGSelectPopupForm.SelectedType.BG224)
            {
                ImageUtil.Convert255ColorTo224Color(image);
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData(this.P0, image, true, undodata);
                this.InputFormRef.WriteImageData(this.P4, tsa, false, undodata);
                this.InputFormRef.WriteImageData(this.P8, palette, false, undodata);
                if (selectType == ImageBGSelectPopupForm.SelectedType.BG224)
                {
                    this.P4.Value = 1;
                }
                else
                {
                    this.P4.Value = 0;
                }
                Program.Undo.Push(undodata);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (!CheckDangerUpdate())
            {
                return;
            }

            if (PatchUtil.BG256Color() == PatchUtil.BG256ColorPatch.BG256Color)
            {
                ImageBGSelectPopupForm f = (ImageBGSelectPopupForm)InputFormRef.JumpFormLow<ImageBGSelectPopupForm>();
                f.ShowDialog();
                if (f.Selected == ImageBGSelectPopupForm.SelectedType.BG224)
                {
                    ImportButton255(f.Selected);
                    return;
                }
                else if (f.Selected == ImageBGSelectPopupForm.SelectedType.BG255)
                {
                    ImportButton255(f.Selected);
                    return;
                }
                else if (f.Selected == ImageBGSelectPopupForm.SelectedType.None)
                {
                    return;
                }
            }

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
                this.InputFormRef.WriteImageData(this.P0, image, true, undodata);
                this.InputFormRef.WriteImageData(this.P4, tsa, false, undodata);
                this.InputFormRef.WriteImageData(this.P8, palette, false, undodata);
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
            bool isBG256ColorPatch = (PatchUtil.BG256Color() == PatchUtil.BG256ColorPatch.BG256Color);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize )
            {
                string name = "BG " + U.To0xHexString(i);
                uint tsa = Program.ROM.u32(addr + 4);
                if (isBG256ColorPatch)
                {
                    if (tsa == 0)
                    {//255色画像
                        FEBuilderGBA.Address.AddLZ77Pointer(list
                            , addr + 0
                            , name + " 255 color IMAGE"
                            , isPointerOnly
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                        FEBuilderGBA.Address.AddPointer(list
                            , addr + 8
                            , 0x20 * 16
                            , name + " PALETTE"
                            , FEBuilderGBA.Address.DataTypeEnum.PAL);
                        continue;
                    }
                    else if (tsa == 1)
                    {//224色画像
                        FEBuilderGBA.Address.AddLZ77Pointer(list
                            , addr + 0
                            , name + " 224 color IMAGE"
                            , isPointerOnly
                            , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                        FEBuilderGBA.Address.AddPointer(list
                            , addr + 8
                            , 0x20 * 16
                            , name + " PALETTE"
                            , FEBuilderGBA.Address.DataTypeEnum.PAL);
                        continue;
                    }
                }

                {//普通の画像
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
        }



        private void DecreaseColorTSAToolButton_Click(object sender, EventArgs e)
        {
            DecreaseColorTSAToolForm f = (DecreaseColorTSAToolForm)InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
            f.InitMethod(1);
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
                    FELint.CheckLZ77(image, errors, FELint.Type.BG, addr, (uint)i);
                }
            }
        }
        void UpdateRef(uint id)
        {
            InputFormRef.UpdateRef(this.X_REF, id, UseValsID.TargetTypeEnum.BG);
        }

        private void X_REF_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InputFormRef.GotoRef(this.X_REF);
        }

        private void X_REF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InputFormRef.GotoRef(this.X_REF);
            }
        }

        private void GraphicsToolButton_Click(object sender, EventArgs e)
        {
            uint image = U.toOffset(P0.Value);
            uint palette = U.toOffset(P8.Value);
            uint tsa = U.toOffset(P4.Value);

            int imageType, tsaType, paletteCount;
            if (PatchUtil.BG256Color() == PatchUtil.BG256ColorPatch.BG256Color)
            {
                if (tsa == 0)
                {//255 color
                    imageType = 3;
                    tsaType = 0;
                    paletteCount = 16;
                }
                else if (tsa == 1)
                {//224 color
                    imageType = 4;
                    tsaType = 0;
                    paletteCount = 16;
                }
                else
                {//通常の背景
                    imageType = 0;
                    tsaType = 3;
                    paletteCount = 8;
                }
            }
            else
            {//通常の背景
                imageType = 0;
                tsaType = 3;
                paletteCount = 8;
            }

            int width = 32 * 8;
            int height = 20 * 8;
            GraphicsToolForm f = (GraphicsToolForm)InputFormRef.JumpFormLow<GraphicsToolForm>();
            f.Jump(width
                , height
                , image
                , imageType
                , tsa
                , tsaType
                , palette
                , 0
                , paletteCount
                , 0);
            f.Show();
        }
        void ShowWarningMessage()
        {
            uint bgid = (uint)this.AddressList.SelectedIndex;
            if (bgid == Program.ROM.RomInfo.bg_reserve_black_bgid)
            {
                DetailErrorMessageBox.Text = R._("このデータは、真っ黒な背景として画像としてシステムに予約されています。\r\n変更しないことをお勧めします。");
                DetailErrorMessageBox.Show();
            }
            else if (bgid == Program.ROM.RomInfo.bg_reserve_random_bgid)
            {
                DetailErrorMessageBox.Text = R._("このデータは、システムメニューの支援会話表示に利用されるランダム背景として予約されています。\r\n変更しないことをお勧めします。");
                DetailErrorMessageBox.Show();
            }
            else if (Is255BG())
            {
                DetailErrorMessageBox.Text = R._("このデータは、カットシーン用の255色画像として登録されいます。\r\n顔画像を表示した会話イベントでは利用しないでください。");
                DetailErrorMessageBox.Show();
            }
            else
            {
                DetailErrorMessageBox.Text = "";
                DetailErrorMessageBox.Hide();
            }
        }

        bool Is255BG()
        {
            if (PatchUtil.BG256Color() != PatchUtil.BG256ColorPatch.BG256Color)
            {
                return false;
            }
            if (P0.Value != 0 && P4.Value == 0)
            {
                return true;
            }
            return false;
        }
        bool CheckDangerUpdate()
        {
            uint bgid = (uint)this.AddressList.SelectedIndex;
            if (bgid == Program.ROM.RomInfo.bg_reserve_black_bgid
                || bgid == Program.ROM.RomInfo.bg_reserve_random_bgid)
            {
                DialogResult dr = R.ShowNoYes("警告: このデータはシステムで予約されています。\r\nこのデータを書き換えると予期せぬ動作を招くことになります。\r\nそれでも、本当に書き換えてもよろしいですか？");
                if (dr == DialogResult.Yes)
                {
                    return true;
                }
                return false;
            }
            return true;

        }
    }
}
