using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageUnitMoveIconFrom : Form
    {
        public ImageUnitMoveIconFrom()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawImageUnitMoveIconAndText, DrawMode.OwnerDrawFixed);

            this.X_PALETTE.SelectedIndex = 0;
            this.InputFormRef = Init(this);
            this.InputFormRef.PreAddressListExpandsEvent += OnPreClassExtendsWarningHandler;
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(ExportAPButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportAPButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });
            U.AllowDropFilename(this, new string[] { ".BIN" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportAPButton_Click(null, null);
                }
            });
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            uint classCount = ClassForm.DataCount();
            if (classCount <= 0)
            {
                classCount = 0x7f;
            }
            else if (classCount > 0xFF)
            {
                classCount = 0xFF;
            }
            classCount--;

            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.unit_move_icon_pointer()
                , 8
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (i >= classCount)
                    {
                        return false;
                    }
                    if (i == 0)
                    {//先頭データは確認しないことにする.
                        return true;
                    }
                    //0 と 4 がポインタであればデータがあると考える.
                    return U.isPointerOrNULL(Program.ROM.u32(addr + 0));
                }
                , (int i, uint addr) =>
                {
                    uint icon_id = (uint)(i);
                    String name = ClassForm.GetClassNameWhereNo(icon_id);

                    return U.ToHexString(icon_id + 1) + U.SA(name) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageUnitMoveIconFrom_Load(object sender, EventArgs e)
        {
            MakeAPAddressDic();

            this.APComboDic = U.LoadTSVResourcePair(U.ConfigDataFilename("ap_list_"));
            this.X_APCOMBO.BeginUpdate();
            this.X_APCOMBO.Items.Clear();
            foreach (var pair in this.APComboDic)
            {
                this.X_APCOMBO.Items.Add(pair.Value);
            }
            this.X_APCOMBO.EndUpdate();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint pic_address = (uint)P0.Value;
            X_ONE_STEP.Value = 0;
            int palette_type = X_PALETTE.SelectedIndex;

            X_PIC.Image = DrawMoveUnitIcon(pic_address, palette_type);
            X_ONE_PIC.Image = DrawMoveUnitIcon(pic_address, palette_type, 0);

            SelectAPComboFromAPAddresss((uint)P4.Value);
        }


        private void X_ONE_STEP_ValueChanged(object sender, EventArgs e)
        {
            uint pic_address = (uint)P0.Value;
            int step = (int)X_ONE_STEP.Value;
            int palette_type = X_PALETTE.SelectedIndex;
            X_ONE_PIC.Image = DrawMoveUnitIcon(pic_address, palette_type, step);
        }

        public static Bitmap DrawMoveUnitIcon(uint pic_address, int palette_type
            )
        {
            return LoadMoveUnitIcon(pic_address, palette_type);
        }

        public static Bitmap DrawMoveUnitIcon(uint pic_address, int palette_type, int step)
        {
            Bitmap bmp = LoadMoveUnitIcon(pic_address, palette_type);
            int height = (4 * 8) * step;
            Rectangle rect = new Rectangle(0, height, 4 * 8, 4 * 8);
            return ImageUtil.Copy(bmp, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Bitmap LoadMoveUnitIcon(uint pic_address, int palette_type)
        {
            uint palette;
            if (palette_type == 1)
            {//友軍
                palette = Program.ROM.RomInfo.unit_icon_npc_palette_address();
            }
            else if (palette_type == 2)
            {//敵軍
                palette = Program.ROM.RomInfo.unit_icon_enemey_palette_address();
            }
            else if (palette_type == 3)
            {//グレー
                palette = Program.ROM.RomInfo.unit_icon_gray_palette_address();
            }
            else if (palette_type == 4)
            {//4軍
                palette = Program.ROM.RomInfo.unit_icon_four_palette_address();
            }
            else
            {//自軍
                palette = Program.ROM.RomInfo.unit_icon_palette_address();
            }
            uint pic_address_offset = U.toOffset(pic_address);

            if (!U.isSafetyOffset(palette))
            {
                return ImageUtil.Blank(4 * 8, 4 * 8);
            }
            if (!U.isSafetyOffset(pic_address_offset))
            {
                return ImageUtil.Blank(4 * 8, 4 * 8);
            }
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, pic_address_offset);
            if (imageUZ.Length <= 0)
            {
                return ImageUtil.Blank(4 * 8, 4 * 8);
            }

            int width = 4 * 8;
            int height = ImageUtil.CalcHeight(width, imageUZ.Length);

            return ImageUtil.ByteToImage16Tile(width, height
                , imageUZ, 0
                , Program.ROM.Data, (int)palette
                , 0
                );
        }
        public static Bitmap DrawMoveUnitIconBitmap(uint icon_id, int palette_type, int step)
        {
            if (icon_id == 0)
            {
                return ImageUtil.BlankDummy();
            }
            else
            {
                icon_id = icon_id - 1;
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(icon_id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.Blank(2 * 8, 2 * 8);
            }
            uint pic_address = (uint)Program.ROM.u32(addr + 0);

            return DrawMoveUnitIcon(pic_address, palette_type,step);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            uint pic_address = (uint)P0.Value;
            int palette_type = X_PALETTE.SelectedIndex;

            string filename = ImageFormRef.SaveDialogPngOrGIF(InputFormRef);
            if (filename == "")
            {
                return;
            }

            string ext = U.GetFilenameExt(filename);
            if (ext == ".GIF")
            {
                bool r = SaveAnimeGif(filename, pic_address, palette_type);
                if (!r)
                {
                    return;
                }
            }
            else
            {
                Bitmap bitmap = DrawMoveUnitIcon(pic_address, palette_type);
                ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);
                U.BitmapSave(bitmap, filename);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        bool SaveAnimeGif(string filename, uint pic_address, int palette_type)
        {
            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();
            for (int showFrame = 0; showFrame < 0xF; showFrame++)
            {
                Bitmap bitmap = DrawMoveUnitIcon(pic_address, palette_type, showFrame);
                ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);
                uint wait = 10;
                bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, wait));
            }

            //アニメgif生成
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
            return true;
        }


        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 4 * 8;
            int height = 60 * 8;
            if (width != bitmap.Width || height > bitmap.Height )
            {
                Bitmap newbitmap = ImageUtil.ConvertSizeFormat(bitmap, width, height);
                if (newbitmap == null)
                {
                    R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                    return;
                }
                bitmap.Dispose();
                bitmap = newbitmap;
            }

            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , (Program.ROM.RomInfo.unit_icon_palette_address())
                        , (Program.ROM.RomInfo.unit_icon_enemey_palette_address())
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, (Program.ROM.RomInfo.unit_icon_palette_address())));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, (Program.ROM.RomInfo.unit_icon_palette_address())));
                    f.SetReOrderImage2(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, (Program.ROM.RomInfo.unit_icon_enemey_palette_address())));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, bitmap.Width, bitmap.Height);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            this.InputFormRef.WriteImageData(this.P0, image, true, undodata);
            Program.Undo.Push(undodata);

            //ポインタの書き込み
            this.WriteButton.PerformClick();

        }

        private void X_JUMP_WAITICON_Click(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }
            uint cid = (uint)this.AddressList.SelectedIndex + 1;
            uint icon = ClassForm.GetClassWaitIcon(cid);
            InputFormRef.JumpTo(null, icon , "WAITICON", new string[] { });
        }


        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            string name = "MoveUnitIcon";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 , 4});

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                name = "MoveUnitIcon " + U.To0xHexString(i);

                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , addr + 0 
                    , name
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);

                FEBuilderGBA.Address.AddAPPointer(list
                    , addr + 4
                    , name + " AP"
                    , isPointerOnly);

//                string ss = U.To0xHexString(i+1);
//                ss += " " + ClassForm.GetClassName((uint)i + 1);
//                uint ap_addr = Program.ROM.p32(addr + 4);
//                uint length = ImageUtilAP.CalcAPLength(ap_addr);
//                byte[] aa = Program.ROM.getBinaryData(ap_addr, length);
//                ss += " " + U.md5(aa) ;
//                Log.Debug(ss);
            }
        }
        //エラーチェック
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                FELint.CheckLZ77ImagePointerOrNull(addr + 0
                    , errors, FELint.Type.IMAGE_UNIT_MOVE_ICON, addr, 32, 0, (uint)i);
                FELint.CheckAPErrorsPointerOrNull(addr + 4
                    , errors, FELint.Type.IMAGE_UNIT_MOVE_ICON, addr, (uint)i);
            }
        }

        public static uint ExpandsArea(Form form, uint current_count, uint newdatacount, Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.ExpandsArea(form, newdatacount, undodata, Program.ROM.RomInfo.unit_move_icon_pointer());
        }

        public static void OnPreClassExtendsWarningHandler(object sender, EventArgs e)
        {
            InputFormRef.ExpandsEventArgs eventarg = (InputFormRef.ExpandsEventArgs)e;
            bool r = HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.ExtendedMovingMapAnimationList);
            if (!r)
            {
                DialogResult dr = R.ShowNoYes("拡張した領域にある移動アニメーションを利用するには、移動アニメーション拡張のパッチが別途必要です。\r\n移動アニメーションを拡張してもよろしいですか？\r\n");
                if (dr != DialogResult.Yes)
                {//キャンセル.
                    eventarg.IsCancel = true;
                    return;
                }
            }
        }


        private void ImportAPButton_Click(object sender, EventArgs e)
        {
            string title = R._("読込むファイル名を選択してください");
            string filter = R._("AP|*.romtcs.ap.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                if (open.FileNames.Length <= 0 || !U.CanReadFileRetry(open.FileNames[0]))
                {
                    return;
                }
                filename = open.FileNames[0];
                Program.LastSelectedFilename.Save(this, "", open);
            }

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                byte[] ap = File.ReadAllBytes(filename);
                uint ap_address = U.toOffset((uint)P4.Value);

                List<uint> pointers = U.GrepPointerAll(Program.ROM.Data, U.toPointer(ap_address), this.InputFormRef.BaseAddress, this.InputFormRef.BaseAddress + this.InputFormRef.BlockSize * this.InputFormRef.DataCount);
                if (pointers.Count >= 2)
                {//他からも参照されているので、上書き禁止
                    ap_address = 0;
                }

                string undoname = this.Text + " AP:" + U.ToHexString(ap_address);
                Undo.UndoData undodata = Program.Undo.NewUndoData(undoname);

                uint newaddr = InputFormRef.WriteBinaryData(this
                    , ap_address
                    , ap
                    , InputFormRef.get_data_pos_callback_ap
                    , undodata
                );
                if (newaddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    return;
                }
                Program.Undo.Push(undodata);

                this.P4.Value = U.toPointer(newaddr);
            }

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        private void ExportAPButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("AP|*.romtcs.ap.bin|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", save, InputFormRef.MakeSaveImageFilename(".romtcs.ap.bin"));

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }

            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);

            uint ap_address = U.toOffset((uint)P4.Value);
            uint ap_length = ImageUtilAP.CalcAPLength(ap_address);
            if (ap_length == 0)
            {
                return ;
            }
            byte[] current_ap = Program.ROM.getBinaryData(ap_address, ap_length);
            U.WriteAllBytes(filename, current_ap);

            U.SelectFileByExplorer(filename);
        }

        private void JumpToSystemPalette_Click(object sender, EventArgs e)
        {
            ImageSystemIconForm f = (ImageSystemIconForm)InputFormRef.JumpForm<ImageSystemIconForm>();
            f.JumpToPage(1);
        }

        //どのアドレスがAPMD5に対応しているかのマップ
        Dictionary<uint, string> APAddressDic = new Dictionary<uint, string>();
        //現在知られているAPリストのマップ ap_list_ALL.txt
        Dictionary<string, string> APComboDic = new Dictionary<string,string>();

        void MakeAPAddressDic()
        {
            this.APAddressDic = new Dictionary<uint, string>();
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint ap_addr = Program.ROM.p32(addr + 4);
                if (this.APAddressDic.ContainsKey(ap_addr))
                {
                    continue;
                }

                string ap_md5 = CalcAPMD5(ap_addr);
                this.APAddressDic[ap_addr] = ap_md5;
            }
        }
        static string CalcAPMD5(uint ap_addr)
        {
            uint length = ImageUtilAP.CalcAPLength(ap_addr);
            byte[] ap_bin = Program.ROM.getBinaryData(ap_addr, length);

            string ap_md5 = U.md5(ap_bin);
            return ap_md5;
        }

        bool AP_UpdateUILock = false;
        void SelectAPComboFromAPAddresss(uint ap_addr)
        {
            ap_addr = U.toOffset(ap_addr);

            string ap_md5 = CalcAPMD5(ap_addr);
            X_AP_MD5.Text = "AP_MD5: " + ap_md5;

            if (this.AP_UpdateUILock)
            {
                return;
            }
            this.AP_UpdateUILock = true;

            string value;
            if (APComboDic.TryGetValue(ap_md5, out value))
            {
                X_APCOMBO.SelectedIndex = X_APCOMBO.FindString(value);
            }
            else
            {
                X_APCOMBO.SelectedIndex = -1;
            }

            this.AP_UpdateUILock = false;
        }
        void SelectAPAddresssFromAPCombo()
        {
            if (this.AP_UpdateUILock)
            {
                return;
            }
            this.AP_UpdateUILock = true;

            uint ap_addr = SelectAPAddresssFromAPComboLow();
            if (ap_addr == U.NOT_FOUND)
            {
                R.ShowStopError("このROMには、このAPが指定されたクラスが存在しないため、アドレスを見つけられませんでした。\r\n手動でAPのアドレスを入力するか、インポートを行ってください。");
            }
            else
            {
                this.P4.Value = U.toPointer(ap_addr);
            }

            this.AP_UpdateUILock = false;
        }
        uint SelectAPAddresssFromAPComboLow()
        {
            string ap_md5 = "";
            string target = X_APCOMBO.Text;
            foreach (var pair in this.APComboDic)
            {
                if (pair.Value == target)
                {
                    ap_md5 = pair.Key;
                    break;
                }
            }

            if (ap_md5.Length <= 0)
            {
                return U.NOT_FOUND;
            }

            foreach (var pair in this.APAddressDic)
            {
                if (pair.Value == ap_md5)
                {
                    return pair.Key;
                }
            }
            return U.NOT_FOUND;
        }

        private void P4_ValueChanged(object sender, EventArgs e)
        {
            SelectAPComboFromAPAddresss((uint)P4.Value);
        }

        private void X_APCOMBO_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectAPAddresssFromAPCombo();
        }


    }
}
