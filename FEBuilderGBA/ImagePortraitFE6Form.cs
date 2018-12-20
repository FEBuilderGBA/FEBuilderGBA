using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImagePortraitFE6Form : Form
    {
        public ImagePortraitFE6Form()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.AddressList.OwnerDraw(ListBoxEx.DrawImagePortraitAndText, DrawMode.OwnerDrawFixed);

            D0.ValueChanged += AddressList_SelectedIndexChanged;
            D4.ValueChanged += AddressList_SelectedIndexChanged;
            D8.ValueChanged += AddressList_SelectedIndexChanged;
            B12.ValueChanged += AddressList_SelectedIndexChanged;
            B13.ValueChanged += AddressList_SelectedIndexChanged;

            //パレット変更の部分にリンクを置く.
            InputFormRef.markupJumpLabel(this.J_8);

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
            //FEditor Advが文字列長を書いてくれていた場合
            uint FEditorHint = InputFormRef.GetFEditorLengthHint(Program.ROM.p32(Program.ROM.RomInfo.face_pointer()));
            //連続するnull個数
            int nullContinuousCount = 0;

            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.face_pointer()
                , Program.ROM.RomInfo.face_datasize()
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (i <= 0)
                    {
                        return true;
                    }
                    //0 と 4 がポインタであればデータがあると考える.
                    uint u0 = Program.ROM.u32(addr + 0);
                    uint u4 = Program.ROM.u32(addr + 4);
                    uint u8 = Program.ROM.u32(addr + 8);
                    if (U.isPointerOrNULL(u0)
                        && U.isPointerOrNULL(u4)
                        && U.isPointerOrNULL(u8)
                        )
                    {
                        if (u0 == 0 && u4 == 0 && u8 == 0)
                        {//NULLデータ. 怪しいがとりあえずOK
                            nullContinuousCount++;
                            if (nullContinuousCount >= 10)
                            {//NULLデータが連続して10個出てきたら打ち切る.
                                Log.Notify("顔画像で nullが10個連続して出てきたので探索を打ち切りました.");
                                return false;
                            }
                        }
                        else
                        {
                            nullContinuousCount = 0;
                        }
                        return true;
                    }
                    if (FEditorHint != U.NOT_FOUND && i < FEditorHint)
                    {//不明なデータではあるがFEditorがあるというので信用する.
                        nullContinuousCount = 0;
                        return true;
                    }

                    return false;
                }
                , (int i, uint addr) =>
                {
                    uint face_id = (uint)(i);

                    String name = ImagePortraitForm.GetPortraitNameFast(face_id, addr);
                    return U.ToHexString(face_id) + " " + name;
                }
                );
        }

        private void ImagePortraitFE6Form_Load(object sender, EventArgs e)
        {
        }
        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint unit_face = (uint)D0.Value;
            uint map_face = (uint)D4.Value;
            uint palette =  (uint)D8.Value;
            uint mouth_x = (uint)B12.Value;
            uint mouth_y = (uint)B13.Value;
            int showFrame = (int)ShowFrameUpDown.Value;
            if (map_face == 0)
            {
                X_UNIT_PIC.Image = ImagePortraitForm.DrawPortraitClass(unit_face, palette);
                X_PIC_ZZZ.Image = null;
            }
            else if (unit_face != 0)
            {
                X_UNIT_PIC.Image = DrawPortraitUnitFE6(unit_face, palette, mouth_x, mouth_y, showFrame);
                X_PIC_ZZZ.Image = DrawPortraitUnitFE6(unit_face, palette, mouth_x, mouth_y, 3);
            }

            X_MAP_PIC.Image = DrawPortraitFE6Map(map_face, palette);
        }

        //FE6用
        public static Bitmap DrawPortraitUnitFE6(
              uint unit_face
            , uint palette
            , uint mouth_x
            , uint mouth_y
            , int showFrame
            )
        {
            int width = 12 * 8;
            int height = 10 * 8;

            unit_face = U.toOffset(unit_face);
            palette = U.toOffset(palette);

            if (unit_face == 0
                || !U.isSafetyOffset(unit_face)
                || !U.isSafetyOffset(palette)
                )
            {
                //ない.
                return ImageUtil.Blank(width, height);
            }
            //FE6の顔画像は圧縮されている.
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, unit_face);
            if (imageUZ.Length <= 0)
            {//ない
                return ImageUtil.Blank(width, height);
            }

            Bitmap allface = ImageUtil.ByteToImage16Tile(32 * 8, 5 * 8
                , imageUZ, (int)0
                , Program.ROM.Data, (int)palette
                );

            //シートを作る
            Bitmap face = ImageUtil.Blank(face_width, face_height, allface);

            //メインの顔を転写
            ImageUtil.BitBlt(face, parts_width / 2, 0
                , parts_width * 2, parts_height * 2
                , allface, 0, 0);//顔上
            ImageUtil.BitBlt(face, parts_width / 2, parts_height * 2
                , parts_width * 2, parts_height * 2
                , allface, parts_width * 2, 0);//顔下
            ImageUtil.BitBlt(face, parts_width / 2, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, 0);//右肩
            ImageUtil.BitBlt(face, 0, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5, 0);//右端
            ImageUtil.BitBlt(face, parts_width + parts_width / 2, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, parts_height);//左肩
            ImageUtil.BitBlt(face, parts_width * 2 + parts_width / 2, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5 + parts_width / 2, 0);//左端

            //表示フレーム指定
            switch (showFrame)
            {
                case 1://口ぱく1
                    ImageUtil.BitBlt(face, (int)mouth_x * 8, (int)mouth_y * 8, parts_width, parts_height
                        , allface, 192, 0, 0, 0);
                    break;
                case 2://口ぱく2
                    ImageUtil.BitBlt(face, (int)mouth_x * 8, (int)mouth_y * 8, parts_width, parts_height
                        , allface, 224, 0, 0, 0);
                    break;
                case 3://口ぱく3
                    ImageUtil.BitBlt(face, (int)mouth_x * 8, (int)mouth_y * 8, parts_width, parts_height
                        , allface, 192, 16, 0, 0);
                    break;
                case 4://口ぱく4
                    ImageUtil.BitBlt(face, (int)mouth_x * 8, (int)mouth_y * 8, parts_width, parts_height
                        , allface, 224, 16, 0, 0);
                    break;
                case 5://口ぱく5
                    ImageUtil.BitBlt(face, (int)mouth_x * 8, (int)mouth_y * 8, parts_width, parts_height/2
                        , allface, 0, 32, 0, 0);
                    ImageUtil.BitBlt(face, (int)mouth_x * 8, (int)mouth_y * 8 + (parts_height / 2), parts_width, parts_height / 2
                        , allface, 32, 32, 0, 0);
                    break;
                default:
                    break;
            }
            allface.Dispose();

            return face;
        }
        public static Bitmap DrawPortraitFE6Map(
              uint map_face
            , uint palette
            )
        {
            int width = 4 * 8;
            int height = 4 * 8;

            map_face = U.toOffset(map_face);
            palette = U.toOffset(palette);
            if (map_face == 0
                || !U.isSafetyOffset(map_face)
                || !U.isSafetyOffset(palette)
                )
            {
                //ない.
                return ImageUtil.Blank(width, height);
            }

            return ImageUtil.ByteToImage16Tile(width, height
                , Program.ROM.Data, (int)map_face
                , Program.ROM.Data, (int)palette
                , 0
            );
        }
        const int seet_width = (12 + 4) * 8;
        const int seet_height = (10 + 4) * 8;

        const int face_width = 12 * 8;
        const int face_height = 10 * 8;

        const int parts_width = 8 * 4;
        const int parts_height = 8 * 2;

        const int mapface_width = 4 * 8;
        const int mapface_height = 4 * 8;

        public static Bitmap DrawPortraitAuto(uint id)
        {
            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint unit_face = Program.ROM.u32(addr);
            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);
            uint mouth_x = Program.ROM.u8(addr + 12);
            uint mouth_y = Program.ROM.u8(addr + 13);
            if (unit_face != 0)
            {
                return DrawPortraitUnitFE6(unit_face, palette, mouth_x, mouth_y , 0);
            }
            else
            {
                return DrawPortraitClassFE6(id);
            }
        }
        public static Bitmap DrawPortraitFE6Auto(uint id)
        {
            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint unit_face = Program.ROM.u32(addr);
            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);
            uint mouth_x = Program.ROM.u8(addr + 12);
            uint mouth_y = Program.ROM.u8(addr + 13);
            if (unit_face != 0)
            {
                return DrawPortraitUnitFE6(unit_face, palette, mouth_x, mouth_y, 0);
            }
            else
            {
                return DrawPortraitClassFE6(id);
            }
        }

        //マップ顔
        public static Bitmap DrawPortraitFE6Map(uint id)
        {
            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);
            return DrawPortraitFE6Map(map_face, palette);
        }

        //クラスカード
        public static Bitmap DrawPortraitClassFE6(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint baseaddress = InputFormRef.BaseAddress;
            uint blocksize = InputFormRef.BlockSize;
            //現在のIDに対応するデータ
            uint addr = baseaddress + (id * blocksize);

            uint unit_face = Program.ROM.u32(addr);
            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);
            if (map_face == 0)
            {
                return ImagePortraitForm.DrawPortraitClass(unit_face, palette);
            }
            return ImageUtil.BlankDummy();
        }


        //顔画像シートを作成する.
        public static Bitmap DrawPortraitSeetFE6(uint id)
        {
            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint unit_face = Program.ROM.u32(addr);
            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);
            if (unit_face == 0)
            {
                return ImagePortraitForm.DrawPortraitClass(unit_face, palette);
            }



            unit_face = U.toOffset(unit_face);
            palette = U.toOffset(palette);

            if (unit_face == 0
                || !U.isSafetyOffset(unit_face)
                || !U.isSafetyOffset(palette)
                )
            {
                //ない.
                return ImageUtil.Blank(seet_width, seet_height);
            }
            //FE6の顔画像は圧縮されている.
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, unit_face);
            if (imageUZ.Length <= 0)
            {//ない
                return ImageUtil.Blank(seet_width, seet_height);
            }


            Bitmap allface = ImageUtil.ByteToImage16Tile(32 * 8, 5 * 8
                , imageUZ, (int)0
                , Program.ROM.Data, (int)palette
                );

            //シートを作る
            Bitmap seet = ImageUtil.Blank(seet_width, seet_height, allface);

            //メインの顔を転写
            ImageUtil.BitBlt(seet, parts_width/2, 0
                , parts_width * 2, parts_height*2
                , allface, 0, 0);//顔上
            ImageUtil.BitBlt(seet, parts_width / 2, parts_height * 2
                , parts_width * 2, parts_height * 2
                , allface, parts_width * 2, 0);//顔下
            ImageUtil.BitBlt(seet, parts_width/2, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, 0);//右肩
            ImageUtil.BitBlt(seet, 0, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5, 0);//右端
            ImageUtil.BitBlt(seet, parts_width + parts_width/2, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, parts_height);//左肩
            ImageUtil.BitBlt(seet, parts_width * 2 + parts_width/2, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5 + parts_width/2, 0);//左端

            //マップ顔を描画
            map_face = U.toOffset(map_face);
            Bitmap mapface_bitmap = ImageUtil.ByteToImage16Tile(mapface_width, mapface_height
                , Program.ROM.Data, (int)map_face
                , Program.ROM.Data, (int)palette
                , 0
            );
            //シートにマップ顔を転写
            ImageUtil.BitBlt(seet, face_width, parts_height, mapface_width, mapface_height, mapface_bitmap, 0, 0);

            //口 いい具合に2*2と固まっているのでまとめてもっていく
            ImageUtil.BitBlt(seet, parts_width * 0, face_height, parts_width * 2, parts_height * 2, allface, (32 * 8) - (parts_width * 2), parts_height * 0);

            //右端の口
            ImageUtil.BitBlt(seet, parts_width * 3, face_height, parts_width, parts_height / 2, allface, 0, 4 * 8);
            //余白
            ImageUtil.BitBlt(seet, parts_width * 3, face_height + (parts_height / 2), parts_width, parts_height / 2, allface, 4 * 8, 4 * 8);

            return seet;
        }

        public static void BuildPortraitSeetFE6(Bitmap seet, out Bitmap allface, out Bitmap map_face)
        {
            //インポートできる顔を作る
            allface = ImageUtil.Blank(32 * 8, 5 * 8, seet);

            //メインの顔を逆変換
            ImageUtil.BitBltRev(seet, parts_width / 2, 0
                , parts_width * 2, parts_height * 2
                , allface, 0, 0);//顔上
            ImageUtil.BitBltRev(seet, parts_width / 2, parts_height * 2
                , parts_width * 2, parts_height * 2
                , allface, parts_width * 2, 0);//顔下
            ImageUtil.BitBltRev(seet, parts_width / 2, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, 0);//右肩
            ImageUtil.BitBltRev(seet, 0, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5, 0);//右端
            ImageUtil.BitBltRev(seet, parts_width + parts_width / 2, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, parts_height);//左肩
            ImageUtil.BitBltRev(seet, parts_width * 2 + parts_width / 2, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5 + parts_width / 2, 0);//左端
            //マップ顔を転写
            map_face = ImageUtil.Blank(mapface_width, mapface_height, allface);
            ImageUtil.BitBlt(map_face, 0, 0, mapface_width, mapface_height, seet, face_width, parts_height);

            //口 いい具合に2*2と固まっているのでまとめてもっていく
            ImageUtil.BitBltRev(seet, parts_width * 0, face_height, parts_width * 2, parts_height * 2, allface, (32 * 8) - (parts_width * 2), parts_height * 0);

            //右端の口
            ImageUtil.BitBltRev(seet, parts_width * 3, face_height, parts_width, parts_height / 2, allface, 0, 4 * 8);
            ImageUtil.BitBltRev(seet, parts_width * 3, face_height + (parts_height / 2), parts_width, parts_height / 2, allface, 4 * 8, 4 * 8);
        }
        
        //顔シートの出力
        private void ExportButton_Click(object sender, EventArgs e)
        {
            uint unit_face = (uint)this.D0.Value;
            if (unit_face != 0)
            {
                Bitmap seetbitmap = DrawPortraitSeetFE6((uint)this.AddressList.SelectedIndex);
                ImageFormRef.ExportImage(this,seetbitmap, InputFormRef.MakeSaveImageFilename());
            }
            else
            {
                Bitmap classbitmap = ImagePortraitForm.DrawPortraitClass((uint)this.AddressList.SelectedIndex);
                ImageFormRef.ExportImage(this,classbitmap, InputFormRef.MakeSaveImageFilename());
            }

        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (!InputFormRef.CheckWriteProtectionID00())
            {
                return;
            }

            string imagefilename = ImageFormRef.OpenFilenameDialogFullColor(this);
            if (imagefilename == "")
            {
                return;
            }

            Bitmap fullColor = ImageUtil.OpenLowBitmap(imagefilename); //bitmapそのものの色で開く.
            if (fullColor == null)
            {
                return;
            }

            if (fullColor.Width == 128 && fullColor.Height == 112)
            {
                Bitmap seetbitmap;
                if (ImageUtil.Is16ColorBitmap(fullColor))
                {
                    seetbitmap = ImageUtil.OpenBitmap(imagefilename); //16色に変換して開く.
                    if (seetbitmap == null)
                    {
                        fullColor.Dispose();
                        return;
                    }
                    seetbitmap = ImageUtil.ConvertPaletteTransparentUI(seetbitmap);
                    if (seetbitmap == null)
                    {
                        fullColor.Dispose();
                        return;
                    }
                }
                else
                {//取り込めないので提案をする.
                    ImagePortraitImporterForm f = (ImagePortraitImporterForm)InputFormRef.JumpFormLow<ImagePortraitImporterForm>();
                    f.SetOrignalImage(fullColor
                        , 0, 0, (int)this.B12.Value, (int)this.B13.Value);
                    f.ShowDialog();

                    seetbitmap = f.GetResultBitmap();
                    if (seetbitmap == null)
                    {
                        fullColor.Dispose();
                        return;
                    }
                    if (f.IsDetailMode())
                    {
                        U.ForceUpdate(this.B12, f.GetMouthBlockX());
                        U.ForceUpdate(this.B13, f.GetMouthBlockY());
                    }
                }

                Bitmap seet;
                Bitmap map_face;
                BuildPortraitSeetFE6(seetbitmap, out seet, out map_face);

                byte[] seet_image = ImageUtil.ImageToByte16Tile(seet, seet.Width, seet.Height);
                seet_image = U.subrange(seet_image,0,(uint)(seet_image.Length - (3*(8*8/2)*8))); //FE6の場合データサイズがさらに小さくなる
                byte[] map_face_image = ImageUtil.ImageToByte16Tile(map_face, map_face.Width, map_face.Height);
                byte[] palette = ImageUtil.ImageToPalette(seet, 1);


                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    //画像等データの書き込み
                    Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                    this.InputFormRef.WriteImageData(this.D0, seet_image, true, undodata); //FE6は顔画像も圧縮する
                    this.InputFormRef.WriteImageData(this.D4, map_face_image, false, undodata); //FE6のマップ顔は無圧縮
                    this.InputFormRef.WriteImageData(this.D8, palette, false, undodata);
                    Program.Undo.Push(undodata);
                }

                //ポインタの書き込み
                this.WriteButton.PerformClick();
            }
            else if (fullColor.Width == 80 &&
                (fullColor.Height == 80 || fullColor.Height == 72))
            {
                Bitmap seetbitmap = ImageUtil.OpenBitmap(imagefilename); //16色に変換して開く.
                if (seetbitmap == null)
                {
                    fullColor.Dispose();
                    return;
                }
                seetbitmap = ImageUtil.ConvertPaletteTransparentUI(seetbitmap);
                if (seetbitmap == null)
                {
                    fullColor.Dispose();
                    return;
                }

                byte[] class_image = ImageUtil.ImageToByte16Tile(seetbitmap, seetbitmap.Width, seetbitmap.Height);
                byte[] palette = ImageUtil.ImageToPalette(seetbitmap, 1);

                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    //画像等データの書き込み
                    Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                    this.InputFormRef.WriteImageData(this.D0, class_image, true, undodata); //クラス画像は圧縮されている.
                    this.InputFormRef.WriteImageData(this.D8, palette, false, undodata);
                    Program.Undo.Push(undodata);
                }

                //顔パラメーターは使わないので0を入れる.
                U.ForceUpdate(this.D4 ,0);
                U.ForceUpdate(this.B12, 0);
                U.ForceUpdate(this.B13, 0);

                //ポインタの書き込み
                this.WriteButton.PerformClick();

            }
            else
            {
                R.ShowStopError("画像サイズが正しくありません。\r\n顔画像ならば、128x112 \r\nクラス画像ならば 80x80 である必要があります。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", fullColor.Width, fullColor.Height, Width, Height);
            }
        }

        public static void RecyclePortrait(ref List<Address> recycle,string basename,bool isPointerOnly, uint portrait_addr)
        {
            if (!U.isSafetyOffset(portrait_addr))
            {
                return;
            }
            uint a0 = Program.ROM.p32(portrait_addr + 0);
            uint a4 = Program.ROM.p32(portrait_addr + 4);
            uint a8 = Program.ROM.p32(portrait_addr + 8);
            //顔画像は圧縮されている.
            if (U.isSafetyOffset(a0))
            {
                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , portrait_addr + 0
                    , basename + "FACE"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            }
            if (U.isSafetyOffset(a4))
            {
                FEBuilderGBA.Address.AddPointer(recycle
                    , portrait_addr + 4
                    , mapface_width * mapface_height / 2 //(/2は16色のため)
                    , basename + "MAP FACE"
                    , FEBuilderGBA.Address.DataTypeEnum.IMG);
            }
            if (U.isSafetyOffset(a8))
            {
                FEBuilderGBA.Address.AddPointer(recycle
                    , portrait_addr + 8
                    , 0x20 //16色パレット
                    , basename + "PAL"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            string name = "PortraitFE6";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 , 4, 8});

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    name = "Portrait:" + U.To0xHexString(i);
                    RecyclePortrait(ref list, name, isPointerOnly, addr);
                }
            }
        }

        private void J_8_Click(object sender, EventArgs e)
        {
            ImagePalletForm f = (ImagePalletForm)InputFormRef.JumpForm<ImagePalletForm>(U.NOT_FOUND);

            Bitmap baseBitmap = ImagePortraitForm.DrawPortraitUnit((uint)this.AddressList.SelectedIndex);
            f.JumpTo(baseBitmap, (uint)D8.Value, 1);
            f.FormClosed += (s, ee) =>
            {
                if (this.IsDisposed)
                {
                    return;
                }
                U.ReSelectList(this.AddressList);
            };
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.PORTRAIT, U.NOT_FOUND
                    , R._("顔画像が極端に少ないです。破損している可能性があります。")));
            }

            uint portrait_addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, portrait_addr += InputFormRef.BlockSize)
            {
                uint id = (uint)i;
                uint a0 = Program.ROM.p32(portrait_addr + 0);
                uint a4 = Program.ROM.p32(portrait_addr + 4);
                //uint a8 = Program.ROM.p32(portrait_addr + 8);
                //顔画像は圧縮されている.
                if (U.isSafetyOffset(a0))
                {
                    FELint.CheckLZ77Errors(a0, errors, FELint.Type.PORTRAIT, portrait_addr, id);
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
