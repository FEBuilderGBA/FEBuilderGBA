using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace FEBuilderGBA
{
    public class ImageFormRef
    {
        Form SelfForm = null;
        string Prefix;
        NumericUpDown IMAGE = null;
        NumericUpDown TSA = null;
        NumericUpDown HEADERTSA = null;
        NumericUpDown PALETTE = null;
        NumericUpDown ZPALETTE = null;
        NumericUpDown ZIMAGE = null;
        NumericUpDown Z2IMAGE = null;
        NumericUpDown Z256IMAGE = null;
        NumericUpDown ZTSA = null;
        NumericUpDown ZHEADERTSA = null;
        NumericUpDown ZLINER256IMAGE = null;
        PictureBox Picture = null;
        Button ExportButton = null;
        Button ImportButton = null;
        Button TSAEditorButton = null;
        Button PaletteEditorButton = null;
        Button AllWriteButton = null;
        Button JumpGraphicsToolButton = null;
        Button JumpDecreaseColorToolButton = null;
        Label PaletteEditorLabel = null;
        int Width;
        int Height;
        int PaletteCount;
        uint IMAGEPointer;
        uint IMAGE2Pointer;
        uint TSAPointer;
        uint PALETTEPointer;
        bool ImportKeepImage = false;
        bool ImportKeepTSA = false;
        bool ImportKeepPalette = false;
        bool ImportTSANoMargin = false;
        uint[] ForceSeparationAddress;

        public ImageFormRef(Form self, string prefix, int width, int height, int palette_count, uint image_pointer, uint tsa_pointer, uint palette_pointer, uint[] force_separation_address = null, uint image2_pointer = 0)
        {
            SelfForm = self;
            Prefix = prefix;
            Width = width;
            Height = height;
            PaletteCount = palette_count;

            IMAGEPointer = image_pointer;
            TSAPointer = tsa_pointer;
            PALETTEPointer = palette_pointer;

            ForceSeparationAddress = force_separation_address;
            IMAGE2Pointer = image2_pointer;
            List<Control> controls = InputFormRef.GetAllControls(self);

            foreach (Control info in controls)
            {
                if (info.Name.IndexOf("AllWriteButton") >= 0 && info is Button)
                {
                    AllWriteButton = (Button)info;
                    continue;
                }

                String name = InputFormRef.SkipPrefixName(info.Name, prefix);
                if (name.IndexOf("_IMAGE") >= 0 && info is NumericUpDown)
                {
                    IMAGE = (NumericUpDown)info;
                    IMAGE.Increment = 4;
                    IMAGE.Maximum = 0xffffffff;
                    IMAGE.Value = Program.ROM.u32(U.toOffset(image_pointer));
                    continue;
                }
                if (name.IndexOf("_TSA") >= 0 && info is NumericUpDown)
                {
                    TSA = (NumericUpDown)info;
                    TSA.Increment = 4;
                    TSA.Maximum = 0xffffffff;
                    TSA.Value = Program.ROM.u32(U.toOffset(tsa_pointer));
                    continue;
                }
                if (name.IndexOf("_PALETTE") >= 0 && info is NumericUpDown)
                {
                    PALETTE = (NumericUpDown)info;
                    PALETTE.Increment = 4;
                    PALETTE.Maximum = 0xffffffff;
                    if (palette_pointer != U.NOT_FOUND)
                    {//初期値を取得.
                        PALETTE.Value = Program.ROM.u32(U.toOffset(palette_pointer));
                    }
                    else
                    {//そのままの値を使う.
                        Debug.Assert(PALETTE.Value > 0);
                    }
                    continue;
                }
                if (name.IndexOf("_ZPALETTE") >= 0 && info is NumericUpDown)
                {
                    Debug.Assert(PALETTEPointer != U.NOT_FOUND);
                    Debug.Assert(U.isSafetyPointer(PALETTEPointer));
                    ZPALETTE = (NumericUpDown)info;
                    ZPALETTE.Increment = 4;
                    ZPALETTE.Maximum = 0xffffffff;
                    ZPALETTE.Value = Program.ROM.u32(U.toOffset(palette_pointer));
                    continue;
                }
                if (name.IndexOf("_ZIMAGE") >= 0 && info is NumericUpDown)
                {
                    ZIMAGE = (NumericUpDown)info;
                    ZIMAGE.Increment = 4;
                    ZIMAGE.Maximum = 0xffffffff;
                    ZIMAGE.Value = Program.ROM.u32(U.toOffset(image_pointer));
                    continue;
                }
                if (name.IndexOf("_Z2IMAGE") >= 0 && info is NumericUpDown)
                {
                    Z2IMAGE = (NumericUpDown)info;
                    Z2IMAGE.Increment = 4;
                    Z2IMAGE.Maximum = 0xffffffff;
                    Z2IMAGE.Value = Program.ROM.u32(U.toOffset(image2_pointer));
                    continue;
                }
                if (name.IndexOf("_Z256IMAGE") >= 0 && info is NumericUpDown)
                {
                    Z256IMAGE = (NumericUpDown)info;
                    Z256IMAGE.Increment = 4;
                    Z256IMAGE.Maximum = 0xffffffff;
                    Z256IMAGE.Value = Program.ROM.u32(U.toOffset(image_pointer));
                    continue;
                }
                if (name.IndexOf("__ZLINER256IMAGE") >= 0 && info is NumericUpDown)
                {
                    ZLINER256IMAGE = (NumericUpDown)info;
                    ZLINER256IMAGE.Increment = 4;
                    ZLINER256IMAGE.Maximum = 0xffffffff;
                    ZLINER256IMAGE.Value = Program.ROM.u32(U.toOffset(image_pointer));
                    continue;
                }
                if (name.IndexOf("_ZTSA") >= 0 && info is NumericUpDown)
                {
                    ZTSA = (NumericUpDown)info;
                    ZTSA.Increment = 4;
                    ZTSA.Maximum = 0xffffffff;
                    ZTSA.Value = Program.ROM.u32(U.toOffset(tsa_pointer));
                    continue;
                }
                if (name.IndexOf("_HEADERTSA") >= 0 && info is NumericUpDown)
                {
                    HEADERTSA = (NumericUpDown)info;
                    HEADERTSA.Increment = 4;
                    HEADERTSA.Maximum = 0xffffffff;
                    HEADERTSA.Value = Program.ROM.u32(U.toOffset(tsa_pointer));
                    continue;
                }
                if (name.IndexOf("_ZHEADERTSA") >= 0 && info is NumericUpDown)
                {
                    ZHEADERTSA = (NumericUpDown)info;
                    ZHEADERTSA.Increment = 4;
                    ZHEADERTSA.Maximum = 0xffffffff;
                    ZHEADERTSA.Value = Program.ROM.u32(U.toOffset(tsa_pointer));
                    continue;
                }
                if (name.IndexOf("Picture") >= 0 && info is PictureBox)
                {
                    Picture = (PictureBox)info;
                    continue;
                }
                if (name.IndexOf("Export") >= 0 && info is Button)
                {
                    ExportButton = (Button)info;
                    continue;
                }
                if (name.IndexOf("Import") >= 0 && info is Button)
                {
                    ImportButton = (Button)info;

                    if (name.IndexOf("_KeepImage") >= 0)
                    {
                        ImportKeepImage = true;
                    }
                    if (name.IndexOf("_KeepTSA") >= 0)
                    {
                        ImportKeepTSA = true;
                    }
                    if (name.IndexOf("_KeepPalette") >= 0)
                    {
                        ImportKeepPalette = true;
                    }
                    if (name.IndexOf("_TSANoMargin") >= 0)
                    {
                        ImportTSANoMargin = true;
                    }
                    
                    continue;
                }
                if (name.IndexOf("TSAEditor") >= 0 && info is Button)
                {
                    TSAEditorButton = (Button)info;
                    continue;
                }
                if (name.IndexOf("PaletteEditor") >= 0 && info is Button)
                {
                    PaletteEditorButton = (Button)info;
                    continue;
                }
                if (name.IndexOf("JumpGraphicsTool") >= 0 && info is Button)
                {
                    JumpGraphicsToolButton = (Button)info;
                    continue;
                }
                if (name.IndexOf("JumpDecreaseColorTool") >= 0 && info is Button)
                {
                    JumpDecreaseColorToolButton = (Button)info;
                    continue;
                }
                if (name.IndexOf("AllWriteButton") >= 0 && info is Button)
                {
                    AllWriteButton = (Button)info;
                    continue;
                }
                if (name.IndexOf("PaletteEditorLabel") >= 0 && info is Label)
                {
                    PaletteEditorLabel = (Label)info;
                    continue;
                }
            }
            DrawPictureBox();

            if (ExportButton != null)
            {
                ExportButton.Click += OnExportButton;
                U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            }
            if (PaletteEditorButton != null)
            {
                Debug.Assert(ZPALETTE == null); //圧縮パレットはここでは変更できない
                PaletteEditorButton.Click += OnPalette;
            }
            if (ImportButton != null)
            {
                ImportButton.Click += OnImport;
                U.SetIcon(ImportButton, Properties.Resources.icon_upload);
            }
            if (TSAEditorButton != null)
            {
                TSAEditorButton.Click += OnTSAEditor;
            }
            if (JumpGraphicsToolButton != null)
            {
                JumpGraphicsToolButton.Click += OnJumpGraphicsTool;
            }
            if (JumpDecreaseColorToolButton != null)
            {
                JumpDecreaseColorToolButton.Click += OnJumpDecreaseColorTool;
            }
            if (PaletteEditorLabel != null)
            {
                Debug.Assert(ZPALETTE == null); //圧縮パレットはここでは変更できない
                PaletteEditorLabel.Click += OnPalette;
                InputFormRef.markupJumpLabel(PaletteEditorLabel);
            }
        }
        public void UpdatePointers(uint image_pointer, uint tsa_pointer, uint palette_pointer, uint image2_pointer = 0)
        {
            IMAGEPointer = image_pointer;
            TSAPointer = tsa_pointer;
            PALETTEPointer = palette_pointer;
            IMAGE2Pointer = image2_pointer;
        }
        public bool UpdateNumericUpDown(NumericUpDown info, string name)
        {
            if (name.IndexOf("_IMAGE") >= 0)
            {
                IMAGE = info;
                return true;
            }
            if (name.IndexOf("_TSA") >= 0)
            {
                TSA = info;
                return true;
            }
            if (name.IndexOf("_PALETTE") >= 0)
            {
                PALETTE = info;
                return true;
            }
            if (name.IndexOf("_ZPALETTE") >= 0)
            {
                Debug.Assert(PALETTEPointer != U.NOT_FOUND);
                Debug.Assert(U.isSafetyPointer(PALETTEPointer));
                ZPALETTE = info;
                return true;
            }
            if (name.IndexOf("_ZIMAGE") >= 0)
            {
                ZIMAGE = info;
                return true;
            }
            if (name.IndexOf("_Z2IMAGE") >= 0)
            {
                Z2IMAGE = info;
                return true;
            }
            if (name.IndexOf("_Z256IMAGE") >= 0)
            {
                Z256IMAGE = info;
                return true;
            }
            if (name.IndexOf("__ZLINER256IMAGE") >= 0)
            {
                ZLINER256IMAGE = info;
                return true;
            }
            if (name.IndexOf("_ZTSA") >= 0)
            {
                ZTSA = info;
                return true;
            }
            if (name.IndexOf("_HEADERTSA") >= 0)
            {
                HEADERTSA = info;
                return true;
            }
            if (name.IndexOf("_ZHEADERTSA") >= 0)
            {
                ZHEADERTSA = info;
                return true;
            }
            return false;
        }

        void OnJumpDecreaseColorTool(Object sender, EventArgs e)
        {
            int option = 0;
            if (Z256IMAGE != null)
            {
                option = 5;
            }

            DecreaseColorTSAToolForm f = (DecreaseColorTSAToolForm)InputFormRef.JumpForm<DecreaseColorTSAToolForm>(U.NOT_FOUND);
            f.InitMethod(option);
        }

        void OnJumpGraphicsTool(Object sender, EventArgs e)
        {
            uint image = 0;
            uint image2 = 0;
            int imageOption = 0;
            if (ZIMAGE != null)
            {
                image = (uint)ZIMAGE.Value;
                imageOption = 0;
                if (Z2IMAGE != null)
                {
                    image2 = (uint)Z2IMAGE.Value;
                    imageOption = 2;
                }
            }
            if (IMAGE != null)
            {
                image = (uint)IMAGE.Value;
                imageOption = 1;
            }
            if (Z256IMAGE != null)
            {
                image = (uint)Z256IMAGE.Value;
                imageOption = 3;
            }
//            if (ZLINER256IMAGE != null)
//            {
//                image = (uint)ZLINER256IMAGE.Value;
//                imageOption = 4;
//            }

            uint tsa = 0;
            int tsaOption = 0;
            if (ZTSA != null)
            {
                tsa = (uint)ZTSA.Value;
                tsaOption = 1;
            }
            if (ZHEADERTSA != null)
            {
                tsa = (uint)ZHEADERTSA.Value;
                tsaOption = 2;
            }
            if (HEADERTSA != null)
            {
                tsa = (uint)HEADERTSA.Value;
                tsaOption = 3;
            }
            if (TSA != null)
            {
                tsa = (uint)TSA.Value;
                tsaOption = 4;
            }

            uint palette = 0;
            int paletteOption = 0;
            if (PALETTE != null)
            {
                palette = (uint)PALETTE.Value;
                paletteOption = 0;
            }
            if (ZPALETTE != null)
            {
                palette = (uint)ZPALETTE.Value;
                paletteOption = 1;
            }

            GraphicsToolForm f = (GraphicsToolForm)InputFormRef.JumpForm<GraphicsToolForm>(U.NOT_FOUND);
            f.Jump(this.Width, this.Height, image, imageOption, tsa, tsaOption, palette, paletteOption, this.PaletteCount, image2);
        }
        void OnExportButton(Object sender, EventArgs e)
        {
            Bitmap bitmap = Draw();
            NumericUpDown addr = null;
            if (ZIMAGE != null)
            {
                addr = ZIMAGE;
            }
            if (IMAGE != null)
            {
                addr = IMAGE;
            }
            if (Z256IMAGE != null)
            {
                addr = Z256IMAGE;
            }
            if (ZLINER256IMAGE != null)
            {
                addr = ZLINER256IMAGE;
            }
            ImageFormRef.ExportImage(this.SelfForm, bitmap, InputFormRef.MakeSaveImageFilename(this.SelfForm, addr), PaletteCount);
        }

        void OnPalette(Object sender, EventArgs e)
        {
            Bitmap bitmap = Draw();
            PaletteEditor(bitmap);
        }
        void OnImport(Object sender, EventArgs e)
        {
            Import();
        }
        void OnTSAEditor(Object sender, EventArgs e)
        {
            TSAEditor();
        }

        //パレットデータの書き込み.
        public static uint WritePalettePointerData(NumericUpDown numObj, byte[] palette,uint pointer, uint palette_number, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            uint addr = WritePaletteAddressData((uint)numObj.Value, palette, palette_number, undodata, forceSeparationAddress);
            if (addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            numObj.Value = U.toPointer(addr);

            //ポインタ先を書き換える.
            Program.ROM.write_p32(U.toOffset(pointer), addr, undodata);

            return addr;
        }
        public static uint WritePaletteAddressData(NumericUpDown numObj, byte[] palette, uint palette_number, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            uint addr = WritePaletteAddressData((uint)numObj.Value, palette, palette_number, undodata, forceSeparationAddress);
            if (addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            numObj.Value = U.toPointer(addr);

            return addr;
        }
        static uint WritePaletteAddressData(uint addr, byte[] palette, uint palette_number, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            if (checkForceSeparationAddress(addr, forceSeparationAddress))
            {//上書きできないので新規に確保しないといけない
                return InputFormRef.AppendBinaryData(palette, undodata);
            }

            int paletteCount = palette.Length / 0x20;
            if (paletteCount + palette_number > 16)
            {//16個を超えてしまうんですが・・・
                Program.ROM.write_range(U.toOffset(addr), palette, undodata);
            }
            else
            {//固定長
                Program.ROM.write_range(U.toOffset(addr) + (palette_number * 0x20), palette, undodata);
            }
            return addr;
        }

        //画像みたいなデータの書き込み.
        public static uint WriteImageData(Form self, NumericUpDown numObj, uint pointer, byte[] image, bool useLZ77, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            uint addr = (uint)numObj.Value;
            uint newAddr = WriteImageData(self, addr, pointer, image, useLZ77, undodata, forceSeparationAddress);
            if (newAddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            numObj.Value = U.toPointer(newAddr);
            return newAddr;
        }

        //上書き禁止アドレスかどうか調べる.
        public static bool checkForceSeparationAddress(uint addr,uint[] forceSeparationAddress)
        {
            if (!U.CheckZeroAddressWrite(U.toOffset(addr), false))
            {//危険なアドレスなので、上書きしてはいけない
                return true;
            }

            uint p = U.toPointer(addr);
            if (p == ImageFormRef.system_tsa_16color_304x240_address())
            {//汎用TSAだから上書きしてはいけない
                return true;
            }
            if (forceSeparationAddress != null)
            {
                for (int i = 0; i < forceSeparationAddress.Length; i++)
                {
                    if ( p == U.toPointer(forceSeparationAddress[i]))
                    {//上書きしてはいけない
                        return true;
                    }
                }
            }
            //上書きしてもいい.
            return false;
        }

        //画像みたいなデータの書き込み.
        public static uint WriteImageData(Form self, uint addr, uint pointer, byte[] image, bool useLZ77, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            if (!U.CheckZeroAddressWrite(U.toOffset(pointer)))
            {
                return U.NOT_FOUND;
            }

            if (useLZ77)
            {
                image = LZ77.compress(image);
            }

            uint newAddr;
            if ( ImageFormRef.checkForceSeparationAddress(addr, forceSeparationAddress))
            {//上書きしてはいけないので新規に確保する.
                newAddr = InputFormRef.AppendBinaryData(image, undodata);
            }
            else
            {
                addr = U.toOffset(addr);
                if (useLZ77)
                {
                    newAddr = InputFormRef.WriteBinaryData(self,addr, image , InputFormRef.get_data_pos_callback_lz77, undodata);
                }
                else
                {//固定長
                    Program.ROM.write_range(addr, image, undodata);
                    newAddr = addr;
                }
            }
            if (newAddr == U.NOT_FOUND)
            {//書き込みキャンセル.
                return U.NOT_FOUND;
            }

            //ポインタ先を書き換える.
            Program.ROM.write_p32(U.toOffset(pointer), newAddr, undodata);

            return U.toOffset(newAddr);
        }

        public static uint WriteImageData2(Form self, NumericUpDown numObj, uint pointer, NumericUpDown num2Obj, uint pointer2, byte[] image, bool useLZ77, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            //データサイズは必ず 0x3000ブロックずつにしないといけない.
            const uint splitByte = 0x3000;
            {
                byte[] pimage = U.subrange(image
                    , 0
                    , (uint)(splitByte)
                );

                uint addr = (uint)numObj.Value;
                uint newAddr = WriteImageData(self, addr, pointer, pimage, useLZ77, undodata, forceSeparationAddress);
                if (newAddr == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }
                numObj.Value = U.toPointer(newAddr);
            }
            {
                byte[] pimage = U.subrange(image
                    , (uint)(splitByte)
                    , (uint)image.Length
                );

                uint addr = (uint)num2Obj.Value;
                uint newAddr = WriteImageData(self, addr, pointer, pimage, useLZ77, undodata, forceSeparationAddress);
                if (newAddr == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }
                num2Obj.Value = U.toPointer(newAddr);
                return newAddr;
            }
        }


        public void DrawPictureBox()
        {
            if (Picture == null)
            {
                return;
            }
            Picture.Image = Draw();
        }

        Bitmap Draw()
        {
            uint addr;
            byte[] image;
            int image_pos;
            if (ZIMAGE != null)
            {//圧縮画像
                addr = U.toOffset(ZIMAGE.Value);
                if (! U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                image = LZ77.decompress(Program.ROM.Data,addr);
                image_pos = 0;
                if (image.Length <= 2)
                {
                    return ImageUtil.BlankDummy();
                }
                if (Z2IMAGE != null)
                {//第2画像
                    addr = U.toOffset(Z2IMAGE.Value);
                    if (! U.isSafetyOffset(addr))
                    {
                        return ImageUtil.BlankDummy();
                    }
                    byte[] image2 = LZ77.decompress(Program.ROM.Data, addr);
                    if (image2.Length <= 2)
                    {
                        return ImageUtil.BlankDummy();
                    }
                    List<byte> imageUZList = new List<byte>();
                    imageUZList.AddRange(image);
                    imageUZList.AddRange(image2);

                    image = imageUZList.ToArray();
                }
            }
            else if (Z256IMAGE != null)
            {//圧縮画像256色
                addr = U.toOffset(Z256IMAGE.Value);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                image = LZ77.decompress(Program.ROM.Data, addr);
                image_pos = 0;
                if (image.Length <= 2)
                {
                    return ImageUtil.BlankDummy();
                }
            }
            else if (ZLINER256IMAGE != null)
            {//256ライン画像
                addr = U.toOffset(ZLINER256IMAGE.Value);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                image = LZ77.decompress(Program.ROM.Data, addr);
                image_pos = 0;
                if (image.Length <= 2)
                {
                    return ImageUtil.BlankDummy();
                }
            }
            else if (IMAGE != null)
            {//無圧縮画像
                addr = U.toOffset(IMAGE.Value);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                image = Program.ROM.Data;
                image_pos = (int)addr;
            }
            else
            {
                return ImageUtil.BlankDummy();
            }

            byte[] palette;
            int palette_pos;
            if (ZPALETTE != null)
            {//圧縮パレットを利用する
                Debug.Assert(this.PALETTEPointer != U.NOT_FOUND);

                addr = U.toOffset(ZPALETTE.Value);
                if (! U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                palette = LZ77.decompress(Program.ROM.Data, addr);
                if (palette.Length <= 2)
                {
                    return ImageUtil.BlankDummy();
                }
                palette_pos = 0;
            }
            else if (PALETTE != null)
            {//無圧縮パレットを利用する
                addr = U.toOffset(PALETTE.Value);
                if (! U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                palette = Program.ROM.Data;
                palette_pos = (int)addr;
            }
            else if (PALETTEPointer == U.NOT_FOUND)
            {
                Debug.Assert(false);
                return ImageUtil.BlankDummy();
            }
            else
            {
                addr = U.toOffset(PALETTEPointer);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }
                addr = Program.ROM.p32(addr);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                palette = Program.ROM.Data;
                palette_pos = (int)addr;
            }

            const int palette_no = 0;

            byte[] tsa;
            int tsa_pos;
            if (TSA != null)
            {
                addr = U.toOffset(TSA.Value);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                tsa = Program.ROM.Data;
                tsa_pos = (int)addr;
                if (Z256IMAGE != null)
                {//256色画像
                    return ImageUtil.ByteToImage256Tile(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        );
                }
                else
                {
                    return ImageUtil.ByteToImage16Tile(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        , palette_no
                        );
                }
            }
            else if (ZTSA != null)
            {
                addr = U.toOffset(ZTSA.Value);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }

                tsa = LZ77.decompress(Program.ROM.Data, addr);
                tsa_pos = 0;

                if (tsa.Length <= 2)
                {
                    return ImageUtil.BlankDummy();
                }
                if (Z256IMAGE != null)
                {//256色画像
                    return ImageUtil.ByteToImage256Tile(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        );
                }
                else
                {
                    return ImageUtil.ByteToImage16Tile(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        , palette_no
                        );
                }
            }
            else if (HEADERTSA != null)
            {
                addr = U.toOffset(HEADERTSA.Value);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }
                tsa = Program.ROM.Data;
                tsa_pos = (int)addr;
                if (Z256IMAGE != null)
                {//256色画像
                    return ImageUtil.ByteToImage256TileHeaderTSA(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        );
                }
                else
                {
                    return ImageUtil.ByteToImage16TileHeaderTSA(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        );
                }
            }
            else if (ZHEADERTSA != null)
            {
                addr = U.toOffset(ZHEADERTSA.Value);
                if (!U.isSafetyOffset(addr))
                {
                    return ImageUtil.BlankDummy();
                }
                tsa = LZ77.decompress(Program.ROM.Data, addr);
                tsa_pos = 0;

                if (tsa.Length <= 2)
                {
                    return ImageUtil.BlankDummy();
                }

                if (Z256IMAGE != null)
                {//256色画像
                    return ImageUtil.ByteToImage256TileHeaderTSA(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        );
                }
                else
                {
                    return ImageUtil.ByteToImage16TileHeaderTSA(Width
                        , Height
                        , image
                        , image_pos
                        , palette
                        , palette_pos
                        , tsa
                        , tsa_pos
                        );
                }
            }
            else if (Z256IMAGE != null)
            {//256色画像
                return
                    ImageUtil.ByteToImage256Tile(Width
                    , Height
                    , image
                    , image_pos
                    , palette
                    , palette_pos
                        );
            }
            else if (ZLINER256IMAGE != null)
            {//256ライン画像(FE6の地図)
                return
                    ImageUtil.ByteToImage256Liner(Width
                    , Height
                    , image
                    , image_pos
                    , palette
                    , palette_pos
                        );
            }
            else
            {//TSAを利用しない
                return ImageUtil.ByteToImage16Tile(Width
                    , Height
                    , image
                    , image_pos
                    , palette
                    , palette_pos
                    , palette_no
                    );
            }
        }


        void PaletteEditor(Bitmap bitmap)
        {
            Debug.Assert(ZPALETTE == null); //圧縮パレットはここでは変更できない
            Debug.Assert(PALETTE != null); //パレットが必要.

            ImagePalletForm f = (ImagePalletForm)InputFormRef.JumpForm<ImagePalletForm>(U.NOT_FOUND);
            f.JumpTo(bitmap, (uint)(this.PALETTE.Value), this.PaletteCount);
        }

        void TSAEditor()
        {
            Debug.Assert(this.ZIMAGE != null);

            bool isHeaderTSA;
            bool isLZ77;
            if (this.HEADERTSA != null)
            {
                isHeaderTSA = true;
                isLZ77 = false;
            }
            else if (this.ZHEADERTSA != null)
            {
                isHeaderTSA = true;
                isLZ77 = true;
            }
            else if (this.ZTSA != null)
            {
                isHeaderTSA = false;
                isLZ77 = true;
            }
            else if (this.TSA != null)
            {
                isHeaderTSA = false;
                isLZ77 = false;
            }
            else
            {
                throw new Exception("Unlink TSAEditor!");
            }

            Debug.Assert(this.PALETTE != null);

            ImageTSAEditorForm f = (ImageTSAEditorForm)InputFormRef.JumpForm<ImageTSAEditorForm>();
            f.Init((uint)this.Width / 8
                , (uint)this.Height / 8
                , this.IMAGEPointer
                , isHeaderTSA, isLZ77
                , this.TSAPointer
                , this.PALETTEPointer
                , (uint)this.PALETTE.Value
                , this.PaletteCount
                );
        }

        bool CheckSizeByImport(Bitmap bitmap)
        {
            if (this.ImportKeepImage)
            {//画像を書き換えないので、幅高さは関係なくない?
                return true;
            }
            if (bitmap.Width == Width && bitmap.Height == Height)
            {//問題ない
                return true;
            }

            R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, Width, Height);
            return false;
        }
        Bitmap CheckAndConvertTSAByImport(Bitmap bitmap)
        {
            //TSAルールの確認
            if (TSA == null 
                && ZTSA == null
                && HEADERTSA == null
                && ZHEADERTSA == null)
            {
                //TSAを利用しない
                return bitmap;
            }
            //エラーを確認するためにTSAを構築してみる
            string errorMessage;
            ImageUtil.ImageToBytePlainTSA(bitmap, Width, Height, out errorMessage);
            if (errorMessage == "")
            {//問題なし
                return bitmap;
            }
            //問題があるので、補正案を提示する.
            int yohaku = 0;
            bool isReserve1StPalette = true;
            if (Width == 240)
            {//幅30の場合、必ず余白2がある.
                yohaku = 2;
            }

            ErrorTSAErrorForm f = (ErrorTSAErrorForm)InputFormRef.JumpFormLow<ErrorTSAErrorForm>();
            f.SetErrorMessage(errorMessage);
            f.SetReOrderImage1(bitmap, PaletteCount, yohaku, isReserve1StPalette);
            f.ShowDialog();

            return f.GetResultBitmap();
        }

        Bitmap CheckAndConvertPaletteByImport(Bitmap bitmap)
        {
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (this.ImportKeepPalette)
            {//パレットを変更できない. と、いうことはパレットが正しくなくても補正できる
                uint palette_address;
                if (PALETTEPointer == U.NOT_FOUND)
                {
                    if (PALETTE == null)
                    {
                        R.ShowStopError("パレットアドレスが存在しません");
                        return null;
                    }
                    palette_address = U.toOffset(PALETTE.Value);
                    if (!U.isSafetyOffset(palette_address))
                    {
                        R.ShowStopError("パレットアドレスに指定された値は正しくありません。\r\n{0}", palette_address);
                        return null;
                    }
                }
                else
                {
                    palette_address = U.toOffset(PALETTEPointer);
                    if (U.isSafetyOffset(palette_address))
                    {
                        palette_address = Program.ROM.p32(palette_address);
                    }
                    else
                    {
                        R.ShowStopError("パレットポインタに指定された値は正しくありません。\r\n{0} => {1}", PALETTEPointer, palette_address);
                        return null;
                    }
                }

                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , palette_address
                        , U.NOT_FOUND
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, palette_address));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, palette_address));
                    f.SetReOrderImage2(ImageUtil.ReOrderPaletteSetTransparent(bitmap, Program.ROM.Data, palette_address));
                    f.ShowForceButton();
                    f.ShowDialog();

                    return f.GetResultBitmap();
                }

            }
            else
            {
                if (bitmap_palette_count > PaletteCount)
                {//パレットの数が違うので、改善案を提示する.
                    string errorMessage = R._("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, PaletteCount);

                    ErrorTSAErrorForm f = (ErrorTSAErrorForm)InputFormRef.JumpFormLow<ErrorTSAErrorForm>();
                    f.SetErrorMessage(errorMessage);
                    f.SetReOrderImage1(bitmap, PaletteCount, 0, true);
                    f.ShowDialog();

                    return f.GetResultBitmap();
                }
            }

            return bitmap;
        }

        void Import()
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this.SelfForm, null,this.Prefix);
            if (bitmap == null)
            {
                return;
            }
            if (false == CheckSizeByImport(bitmap))
            {
                return;
            }

            bitmap = CheckAndConvertTSAByImport(bitmap);
            if (bitmap == null)
            {
                return;
            }

            bitmap = CheckAndConvertPaletteByImport(bitmap);
            if (bitmap == null)
            {
                return;
            }

            byte[] image = null;
            byte[] tsa = null;
            if (ZLINER256IMAGE != null)
            {
                image = ImageUtil.ImageToByte256Liner(bitmap, Width, Height);
            }
            else if (Z256IMAGE != null)
            {
                image = ImageUtil.ImageToByte256Tile(bitmap, Width, Height);
            }
            else
            {
                if (TSA != null || ZTSA != null)
                {
                    if (this.ImportKeepTSA)
                    {//TSAを維持しないといけない
                        image = ImageToByteKeepTSA(bitmap, Width, Height);
                    }
                    else
                    {//TSAを総入れ替えできる
                        string error_string = ImageUtil.ImageToBytePackedTSA(bitmap, Width, Height,0, out image, out tsa);
//                        string error_string = ImageUtil.ImageToByteNonPackedTSA(bitmap, Width, Height, out image, out tsa);
                        if (error_string != "")
                        {
                            R.ShowStopError(error_string);
                            return;
                        }
                    }
                }
                else if (HEADERTSA != null || ZHEADERTSA != null)
                {
                    if (this.ImportKeepTSA)
                    {//TSAを維持しないといけない
                        image = ImageToByteKeepTSA(bitmap, Width, Height);
                        if (image.Length <= 0)
                        {
                            return;
                        }
                    }
                    else
                    {//TSAを総入れ替えできる
                        int tsa_width_margin = 2;
                        if (this.ImportTSANoMargin)
                        {
                            tsa_width_margin = 0;
                        }
                        string error_string = ImageUtil.ImageToByteHeaderPackedTSA(bitmap, Width, Height, out image, out tsa, tsa_width_margin);
                        if (error_string != "")
                        {
                            R.ShowStopError(error_string);
                            return;
                        }
                    }
                }
                else
                {//TSAを使わない.
                    image = ImageUtil.ImageToByte16Tile(bitmap, Width, Height);
                }
            }

            byte[] palette;
            if (PALETTE != null)
            {
                palette = ImageUtil.ImageToPalette(bitmap, PaletteCount);
            }
            else if (ZPALETTE != null)
            {
                palette = ImageUtil.ImageToPalette(bitmap, PaletteCount);
            }
            else
            {
                palette = null;
            }

            uint writeAddr;
            Undo.UndoData undodata = Program.Undo.NewUndoData(SelfForm.Text);
            if (this.ImportKeepImage)
            {//画像の書き換え禁止
                writeAddr = 0;
            }
            else if (Z2IMAGE != null)
            {//第2画像がある場合
                writeAddr = WriteImageData2(this.SelfForm, ZIMAGE, IMAGEPointer, Z2IMAGE, IMAGE2Pointer, image, true, undodata,ForceSeparationAddress);
            }
            else if (ZIMAGE != null)
            {
                writeAddr = WriteImageData(this.SelfForm, ZIMAGE, IMAGEPointer, image, true, undodata,ForceSeparationAddress);
            }
            else if (Z256IMAGE != null)
            {
                writeAddr = WriteImageData(this.SelfForm, Z256IMAGE, IMAGEPointer, image, true, undodata, ForceSeparationAddress);
            }
            else if (ZLINER256IMAGE != null)
            {
                writeAddr = WriteImageData(this.SelfForm, ZLINER256IMAGE, IMAGEPointer, image, true, undodata,ForceSeparationAddress);
            }
            else
            {
                writeAddr = WriteImageData(this.SelfForm, IMAGE, IMAGEPointer, image, false, undodata,ForceSeparationAddress);
            }

            if (writeAddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                R.ShowStopError("画像データの書き込みに失敗しました。");
                return;
            }

            if (this.ImportKeepTSA)
            {//TSAを維持しないといけない
                writeAddr = 0;
            }
            else if (ZTSA != null)
            {
                writeAddr = WriteImageData(this.SelfForm, ZTSA, TSAPointer, tsa, true, undodata,ForceSeparationAddress);
            }
            else if (TSA != null)
            {
                writeAddr = WriteImageData(this.SelfForm, TSA, TSAPointer, tsa, false, undodata,ForceSeparationAddress);
            }
            else if (ZHEADERTSA != null)
            {
                writeAddr = WriteImageData(this.SelfForm, ZHEADERTSA, TSAPointer, tsa, true, undodata,ForceSeparationAddress);
            }
            else if (HEADERTSA != null)
            {
                writeAddr = WriteImageData(this.SelfForm, HEADERTSA, TSAPointer, tsa, false, undodata,ForceSeparationAddress);
            }

            if (writeAddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                R.ShowStopError("TSAデータの書き込みに失敗しました。");
                return;
            }


            if (this.ImportKeepPalette)
            {//パレットは変更できない.
                writeAddr = 0;
            }
            else if (PALETTE != null)
            {
                if (PALETTEPointer == U.NOT_FOUND)
                {
                    writeAddr = WritePaletteAddressData(PALETTE, palette, 0, undodata, ForceSeparationAddress);
                }
                else
                {
                    writeAddr = WritePalettePointerData(PALETTE, palette, PALETTEPointer, 0, undodata, ForceSeparationAddress);
                }
            }
            else if (ZPALETTE != null)
            {//圧縮すると可変長になるので、WritePaletteAddressDataは使えない.
                writeAddr = WriteImageData(this.SelfForm, ZPALETTE, PALETTEPointer, palette, true, undodata,ForceSeparationAddress);
            }

            if (writeAddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                R.ShowStopError("パレットデータの書き込みに失敗しました。");
                return;
            }


            Program.Undo.Push(undodata);
            AllWriteButton.PerformClick();

            DrawPictureBox();

            bitmap.Dispose();
        }

        byte[] ImageToByteKeepTSA(Bitmap bitmap,int width,int height)
        {
            ushort[] orignalTSA;
            byte[] orignalImage;
            if (this.IMAGE != null)
            {
                orignalImage = Program.ROM.getBinaryData((uint)U.toOffset(this.IMAGE.Value), (uint)(width * height / 2));
            }
            else if (this.ZIMAGE != null)
            {
                orignalImage = LZ77.decompress(Program.ROM.Data, U.toOffset(this.ZIMAGE.Value));
            }
            else
            {
                Debug.Assert(false);
                return new byte[]{};
            }
            if (orignalImage.Length <= 0)
            {
                R.ShowStopError("TSAを維持しようとしましたが元画像を取得できませんでした");
                Debug.Assert(false);
                return new byte[] { };
            }

            if (this.TSA != null)
            {
                orignalTSA = ImageUtil.ByteToTSA(Program.ROM.Data, (int)U.toOffset(this.TSA.Value), (int)width, (int)height);
            }
            else if (this.ZTSA != null)
            {
                byte[]  d = LZ77.decompress(Program.ROM.Data, U.toOffset(this.ZTSA.Value));
                if (d.Length <= 0)
                {
                    Debug.Assert(false);
                    return new byte[] { };
                }
                orignalTSA = ImageUtil.ByteToTSA(d, 0, (int)width, (int)height);
            }
            else if (this.ZHEADERTSA != null)
            {
                byte[] d = LZ77.decompress(Program.ROM.Data, U.toOffset(this.ZHEADERTSA.Value));
                if (d.Length <= 0)
                {
                    Debug.Assert(false);
                    return new byte[] { };
                }
                orignalTSA = ImageUtil.ByteToHeaderTSA(d, 0, (int)width, (int)height);
            }
            else if (this.HEADERTSA != null)
            {
                orignalTSA = ImageUtil.ByteToHeaderTSA(Program.ROM.Data, (int)U.toOffset(this.HEADERTSA.Value), (int)width, (int)height);
            }
            else
            {
                Debug.Assert(false);
                return new byte[] { };
            }

            if (orignalTSA.Length <= 0)
            {
                R.ShowStopError("TSAを維持しようとしましたが元TSAを取得できませんでした");
                Debug.Assert(false);
                return new byte[] { };
            }

            return ImageUtil.ImageToByteKeepTSA(bitmap, width, height, orignalTSA, orignalImage);
        }

        public void WritePointer(Undo.UndoData undodata)
        {
            if (IMAGE != null)
            {
                InputFormRef.WriteOnePointer(IMAGEPointer, IMAGE, undodata);
            }
            else if (ZIMAGE != null)
            {
                InputFormRef.WriteOnePointer(IMAGEPointer, ZIMAGE, undodata);
            }

            if (TSA != null)
            {
                InputFormRef.WriteOnePointer(TSAPointer, TSA, undodata);
            }
            else if (HEADERTSA != null)
            {
                InputFormRef.WriteOnePointer(TSAPointer, HEADERTSA, undodata);
            }
            else if (ZTSA != null)
            {
                InputFormRef.WriteOnePointer(TSAPointer, ZTSA, undodata);
            }
            else if (ZHEADERTSA != null)
            {
                InputFormRef.WriteOnePointer(TSAPointer, ZHEADERTSA, undodata);
            }

            if (PALETTE != null)
            {
                InputFormRef.WriteOnePointer(PALETTEPointer, PALETTE, undodata );
            }
            else if (ZPALETTE != null)
            {
                InputFormRef.WriteOnePointer(PALETTEPointer, ZPALETTE, undodata);
            }
        }
        public static string ExportImageLow(Control self, Bitmap bitmap, string recomendfilename)
        {
            string filename;
            {
                string title = R._("保存するファイル名を選択してください");
                string filter = R._("PNG|*.png|BMP|*.bmp|All files|*");

                SaveFileDialog save = new SaveFileDialog();
                save.Title = title;
                save.Filter = filter;
                save.AddExtension = true;
                Program.LastSelectedFilename.Load(self, "", save, recomendfilename);

                DialogResult dr = save.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return "";
                }
                if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
                {
                    return "";
                }
                filename = save.FileNames[0];
                Program.LastSelectedFilename.Save(self, "", save);
            }

            if (! U.BitmapSave(bitmap, filename) )
            {
                return "";
            }
            return filename;
        }

        public static string ExportImage(Control self, Bitmap bitmap, string recomendfilename, int paletterows = 1)
        {
            if (bitmap == null)
            {
                return "";
            }
            Bitmap bmp = ImageUtil.CloneBitmap(bitmap);

            ImageUtil.BlackOutUnnecessaryColors(bmp, paletterows);
            string savefilename = ExportImageLow(self, bmp, recomendfilename);

            //エクスプローラで選択.
            U.SelectFileByExplorer(savefilename);

            return savefilename;
        }
        public static string OpenFilenameDialogFullColor(Control self)
        {
            string filename;
            if (GetDragFilePath(out filename))
            {
                //drag されているファイルがあるらしい.
                return filename;
            }
            else
            {
                string title = R._("開くファイル名を選択してください");
                string filter = R._("IMAGES|*.png;*.bmp;*.jpg|PNG|*.png|BMP|*.bmp|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                Program.LastSelectedFilename.Load(self, "", open);
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return "";
                }
                if (!U.CanReadFileRetry(open))
                {
                    return "";
                }
                Program.LastSelectedFilename.Save(self, "", open);
                filename = open.FileNames[0];
            }
            return filename;
        }

        static string DragFilePath = "";

        public static bool GetDragFilePath(out string out_path)
        {
            if (DragFilePath == "")
            {
                out_path = "";
                return false;
            }

            out_path = DragFilePath;
            DragFilePath = ""; //利用したので、潰す

            return (File.Exists(out_path));
        }
        static void UpdateDragFilePath(string path)
        {
            DragFilePath = path;
        }
        public class AutoDrag : IDisposable
        {
            public AutoDrag(string filename)
            {
                UpdateDragFilePath(filename);
            }
            public void Dispose()
            {
                UpdateDragFilePath("");
            }
        }

        public readonly static string[] IMAGE_FILE_FILTER = new string[]{ ".PNG",".GIF",".BMP" };

        public static Bitmap ImportFilenameDialog(Control self, Bitmap paletteHint = null, string addName = "")
        {
            string filename;
            if (GetDragFilePath(out filename))
            {
                //drag されているファイルがあるらしい.
                Program.LastSelectedFilename.Save(self, addName , filename);
            }
            else
            {
                string title = R._("開くファイル名を選択してください");
                string filter = R._("IMAGES|*.png;*.bmp|PNG|*.png|BMP|*.bmp|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                Program.LastSelectedFilename.Load(self,addName, open);
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return null;
                }
                if (!U.CanReadFileRetry(open))
                {
                    return null;
                }
                Program.LastSelectedFilename.Save(self, addName, open);
                filename = open.FileNames[0];
            }


            string errormessage;
            Bitmap bitmap = ImageUtil.OpenBitmap(filename, paletteHint, out errormessage);
            if (bitmap == null)
            {
                R.ShowStopError(R._("画像の読み込みに失敗しました。\r\n\r\n{0}"), errormessage);
                return null;
            }
            return bitmap;
        }

        static uint system_tsa_16color_304x240_address()
        {
            return Program.ROM.u32(Program.ROM.RomInfo.system_tsa_16color_304x240_pointer());
        }

        public void UpdateAllWriteButton(Button allWriteButton)
        {
            this.AllWriteButton = allWriteButton;
        }

        public void RegistAllWriteEvent(string name)
        {
            this.AllWriteButton.Click += (sender ,e)=>{
                Undo.UndoData undodata = Program.Undo.NewUndoData(name);
                if (this.IMAGE != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.IMAGEPointer)
                       ,(uint)this.IMAGE.Value
                       , undodata);
                    ImageSystemIconForm.Fix_FE8_systemmenu_battlepreview_image(U.toOffset(this.IMAGEPointer), undodata);
                }
                if (this.ZIMAGE != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.IMAGEPointer)
                       , (uint)this.ZIMAGE.Value
                       , undodata);
                }
                if (this.Z2IMAGE != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.IMAGE2Pointer)
                       , (uint)this.Z2IMAGE.Value
                       , undodata);
                }
                if (this.TSA != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.TSAPointer)
                       ,(uint)this.TSA.Value
                       , undodata);
                }
                if (this.ZTSA != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.TSAPointer)
                       , (uint)this.ZTSA.Value
                       , undodata);
                }
                if (this.HEADERTSA != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.TSAPointer)
                       , (uint)this.HEADERTSA.Value
                       , undodata);
                }
                if (this.ZHEADERTSA != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.TSAPointer)
                       , (uint)this.ZHEADERTSA.Value
                       , undodata);
                }
                if (this.PALETTE != null)
                {
                    if (U.NOT_FOUND != this.PALETTEPointer)
                    {
                        Program.ROM.write_p32(
                            U.toOffset(this.PALETTEPointer)
                           , (uint)this.PALETTE.Value
                           , undodata);
                    }
                }
                if (this.ZPALETTE != null)
                {
                    Program.ROM.write_p32(
                        U.toOffset(this.PALETTEPointer)
                       , (uint)this.ZPALETTE.Value
                       , undodata);
                }

                Program.Undo.Push(undodata);
                InputFormRef.ShowWriteNotifyAnimation(this.SelfForm, U.toOffset(this.IMAGEPointer));
            };
        }
        public static string SaveDialogPngOrGIF(InputFormRef ifr)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("PNG|*.png|アニメGIF|*.gif|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(ifr.SelfForm, "", save, ifr.MakeSaveImageFilename());

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "";
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return "";
            }
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(ifr.SelfForm, "", save);

            return filename;
        }

        public static Size GetSizeToHeaderTSAByPointer(uint pointer)
        {
            if (!U.isSafetyOffset(pointer))
            {
                return new Size(256, 160);
            }
            uint addr = Program.ROM.p32(pointer);
            if (!U.isSafetyOffset(addr))
            {
                return new Size(256, 160);
            }

            int master_headerx = (int)Program.ROM.u8(addr);
            int master_headery = (int)Program.ROM.u8(addr + 1);
            int width = (master_headerx+1) * 8;
            int height = (master_headery+1) * 8;
            if (width <= 0 || width > 256)
            {
                width = 256;
            }
            if (height <= 0 || height > 256)
            {
                height = 256;
            }

            return new Size(width, height);
        }
    }
}
