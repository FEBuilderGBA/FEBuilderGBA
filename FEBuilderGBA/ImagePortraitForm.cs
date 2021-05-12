using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImagePortraitForm : Form
    {
        public ImagePortraitForm()
        {
            InitializeComponent();

            InputFormRef.markupJumpLabel(LinkInternt);

            this.InputFormRef = Init(this);
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.AddressList.OwnerDraw(ListBoxEx.DrawImagePortraitAndText, DrawMode.OwnerDrawFixed);

            D0.ValueChanged += AddressList_SelectedIndexChanged;
            D4.ValueChanged += AddressList_SelectedIndexChanged;
            D8.ValueChanged += AddressList_SelectedIndexChanged;
            D12.ValueChanged += AddressList_SelectedIndexChanged;
            D16.ValueChanged += AddressList_SelectedIndexChanged;
            B20.ValueChanged += AddressList_SelectedIndexChanged;
            B21.ValueChanged += AddressList_SelectedIndexChanged;
            B22.ValueChanged += AddressList_SelectedIndexChanged;
            B23.ValueChanged += AddressList_SelectedIndexChanged;
            B24.ValueChanged += AddressList_SelectedIndexChanged;

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
                            if (nullContinuousCount >= 1000)
                            {//NULLデータが連続して1000個出てきたら打ち切る.
                                Log.Notify("顔画像で nullが1000個連続して出てきたので探索を打ち切りました.");
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
                    String name = GetPortraitNameFast(face_id,addr);
                    return U.ToHexString(face_id) + " " + name;
                }
                );
        }

        //顔の名前
        public static string GetPortraitName(uint face_id)
        {
            InputFormRef InputFormRef = Init(null);
            string comment = InputFormRef.GetComment(face_id);
            
            String name = UnitForm.GetUnitNameWhereFaceID(face_id);
            if (name != "")
            {
                return name + U.SA(comment);
            }
            name = ClassForm.GetClassNameWhereFaceID(face_id);
            return name + U.SA(comment);
        }

        //顔の名前
        public static string GetPortraitNameFast(uint face_id,uint addr)
        {
            String name = UnitForm.GetUnitNameWhereFaceID(face_id);
            if (name != "")
            {
                return name + InputFormRef.GetCommentSA(addr);
            }
            name = ClassForm.GetClassNameWhereFaceID(face_id);
            return name + InputFormRef.GetCommentSA(addr);
        }

        private void ImagePortraitForm_Load(object sender, EventArgs e)
        {
        }
        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef != null && this.InputFormRef.IsUpdateLock)
            {
                return;
            }

            uint unit_face = (uint)D0.Value;
            uint map_face = (uint)D4.Value;
            uint palette =  (uint)D8.Value;
            uint mouth =  (uint)D12.Value;
            uint class_face =  (uint)D16.Value;
            byte mouth_x = (byte)B20.Value;
            byte mouth_y = (byte)B21.Value;
            byte eye_x = (byte)B22.Value;
            byte eye_y = (byte)B23.Value;
            byte state = (byte)B24.Value;
            byte b26 = (byte)B26.Value;
            int showFrame = (int)ShowFrameUpDown.Value;

            if (unit_face != 0)
            {
                X_UNIT_PIC.Image = DrawPortraitUnit(unit_face, palette, mouth, mouth_x, mouth_y, eye_x, eye_y, b26, class_face, state, showFrame);
                X_PIC_ZZZ.Image = DrawPortraitUnit(unit_face, palette, mouth, mouth_x, mouth_y, eye_x, eye_y, b26, class_face,6, 4);
            }
            else
            {
                X_UNIT_PIC.Image = DrawPortraitClass(class_face, palette);
                X_PIC_ZZZ.Image = null;

                //位置調整してねというメッセージを消す.
                this.DescriptionAfterImportLabel.Hide();
            }

            X_MAP_PIC.Image = DrawPortraitMap(map_face, palette);
        }


        public static Bitmap DrawPortraitUnit(
              uint unit_face
            , uint palette
            , uint mouth
            , byte mouth_x
            , byte mouth_y
            , byte eye_x
            , byte eye_y
            , byte b26  //MUG_EXCEED
            , uint mug_exceed_pos
            , byte state
            , int showFrame
            )
        {
            PatchUtil.portrait_extends portraitExtends = PatchUtil.SearchPortraitExtends();
            int seat_width = 32 * 8;
            int seat_height = 5 * 8;

            unit_face = U.toOffset(unit_face);
            palette = U.toOffset(palette);

            if (unit_face == 0
                || !U.isSafetyOffset(unit_face)
                || !U.isSafetyOffset(palette)
                )
            {
                //ない.
                return ImageUtil.Blank(face_width, face_height);
            }

            //TSA変換された顔画像の取得
            Bitmap allface;
            if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
            {//FE7Uは圧縮されている  (FE6も圧縮ただし結構違うので別ルーチン)
             //圧縮   FE7U FE6
             //無圧縮 FE7 FE8 FE8U
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, unit_face);
                if (imageUZ.Length <= 0)
                {//ない
                    return ImageUtil.Blank(face_width, face_height);
                }

                allface = ImageUtil.ByteToImage16Tile(seat_width, seat_height
                    , imageUZ, 0
                    , Program.ROM.Data, (int)palette
                    );
            }
            else
            {
                if (IsHalfBodyFlag(unit_face))
                {
                    seat_height = 10 * 8;
                }
                unit_face += 4; //謎の4バイトをスキップ.

                allface = ImageUtil.ByteToImage16Tile(seat_width, seat_height
                    , Program.ROM.Data, (int)unit_face
                    , Program.ROM.Data, (int)palette
                    );
            }


            //いちばんでかい顔を描画
            Bitmap face = ImageUtil.Blank(face_width, face_height, allface);
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

            if (portraitExtends == PatchUtil.portrait_extends.MUG_EXCEED)
            {
                if (b26 == 1)
                {
                    int mug_exceed_pos_x1 = (int)((mug_exceed_pos) & 0xff);
                    int mug_exceed_pos_y1 = (int)((mug_exceed_pos >> 8) & 0xff);
                    int mug_exceed_pos_x2 = (int)((mug_exceed_pos >> 16) & 0xff);
                    int mug_exceed_pos_y2 = (int)((mug_exceed_pos >> 24) & 0xff);

                    ImageUtil.BitBlt(face, mug_exceed_pos_x1 * 8, mug_exceed_pos_y1 * 8
                        , parts_width / 2, parts_height
                        , allface, (32 * 8) - (parts_width * 1), parts_height * 1
                        , 0, 0);
                    ImageUtil.BitBlt(face, mug_exceed_pos_x2 * 8, mug_exceed_pos_y2 * 8
                        , parts_width / 2, parts_height
                        , allface, (32 * 8) - (parts_width * 1) + parts_width / 2, parts_height * 1
                        , 0, 0);
                }
            }

            if (state == 0x06)
            {
                //目を閉じる
                ImageUtil.BitBlt(face, eye_x * 8, eye_y * 8
                    , parts_width, parts_height
                    , allface, (32 * 8) - (parts_width * 2), parts_height * 1
                    ,0,0);
            }
            if (showFrame == 0)
            {
                if (portraitExtends != PatchUtil.portrait_extends.HALFBODY)
                {
                    return face;
                }
            }

            //口
            mouth = U.toOffset(mouth);
            Bitmap mouthBitmap = ImageUtil.ByteToImage16Tile(parts_width, parts_height * 6
                    , Program.ROM.Data, (int)mouth
                    , Program.ROM.Data, (int)palette
                    );

            //表示フレーム指定
            switch (showFrame)
            {
                case 1://半目
                    ImageUtil.BitBlt(face, eye_x * 8, eye_y * 8
                        , parts_width, parts_height
                        , allface, (32 * 8) - (parts_width * 2), parts_height * 0,0,0);
                    break;
                case 2://目を閉じる
                    ImageUtil.BitBlt(face, eye_x * 8, eye_y * 8
                        , parts_width, parts_height
                        , allface, (32 * 8) - (parts_width * 2), parts_height * 1, 0, 0);
                    break;
                case 3://口ぱく1
                    ImageUtil.BitBlt(face, mouth_x * 8, mouth_y * 8, parts_width, parts_height
                        , mouthBitmap, 0, parts_height * 0, 0, 0);
                    break;
                case 4://口ぱく2
                    ImageUtil.BitBlt(face, mouth_x * 8, mouth_y * 8, parts_width, parts_height
                        , mouthBitmap, 0, parts_height * 1, 0, 0);
                    break;
                case 5://口ぱく3
                    ImageUtil.BitBlt(face, mouth_x * 8, mouth_y * 8, parts_width, parts_height
                        , mouthBitmap, 0, parts_height * 2, 0, 0);
                    break;
                case 6://口ぱく4
                    ImageUtil.BitBlt(face, mouth_x * 8, mouth_y * 8, parts_width, parts_height
                        , mouthBitmap, 0, parts_height * 3, 0, 0);
                    break;
                case 7://口ぱく5
                    ImageUtil.BitBlt(face, mouth_x * 8, mouth_y * 8, parts_width, parts_height
                        , mouthBitmap, 0, parts_height * 4, 0, 0);
                    break;
                case 8://口ぱく6
                    ImageUtil.BitBlt(face, mouth_x * 8, mouth_y * 8, parts_width, parts_height
                        , mouthBitmap, 0, parts_height * 5, 0, 0);
                    break;
                case 9://口ぱく7
                    ImageUtil.BitBlt(face, mouth_x * 8, mouth_y * 8, parts_width, parts_height
                        , allface, 32 * 8 - parts_width, 0, 0, 0);
                    break;
                default:
                    break;
            }

            if (portraitExtends == PatchUtil.portrait_extends.HALFBODY 
                && seat_height == 10 * 8)
            {
                Bitmap halfbody = ImageUtil.Blank(face_width + (16 * 2), face_height + (16 * 3), allface);
                ImageUtil.BitBlt(halfbody, 16, 0, face.Width, face.Height, face, 0, 0);
                ImageUtil.BitBlt(halfbody, 0, parts_height * 5
                    , parts_width * 4, parts_height * 2
                    , allface, 0, parts_height * 2);//ハーフボディの土台
                ImageUtil.BitBlt(halfbody, parts_width / 2, parts_height * 7
                    , parts_width, parts_height
                    , allface, (32 * 8) - (parts_width * 1), parts_height);//左下
                ImageUtil.BitBlt(halfbody, 0, parts_height * 7
                    , parts_width / 2, parts_height
                    , allface, parts_width * 4, parts_height * 2);//左端
                ImageUtil.BitBlt(halfbody, parts_width * 3 + (parts_width/2), parts_height * 7
                    , parts_width / 2, parts_height
                    , allface, parts_width * 4, parts_height * 3);//右端
                ImageUtil.BitBlt(halfbody, parts_width / 2, parts_height * 1
                    , parts_width / 2, parts_height * 2
                    , allface, parts_width * 4 + (parts_width / 2), parts_height * 2);//左上  
                ImageUtil.BitBlt(halfbody, parts_width * 3, parts_height * 1
                    , parts_width / 2, parts_height * 2
                    , allface, parts_width * 5, parts_height * 2);//右上  
                ImageUtil.BitBlt(halfbody, parts_width * 3 + (parts_width / 2), parts_height * 3
                    , parts_width / 2, parts_height * 2
                    , allface, parts_width * 5 + (parts_width/2), parts_height * 2);//右肩
                ImageUtil.BitBlt(halfbody, 0 , parts_height * 3
                    , parts_width / 2, parts_height * 2
                    , allface, parts_width * 6, parts_height * 2);//左肩
                ImageUtil.BitBlt(halfbody, parts_width * 1 + (parts_width / 2), parts_height * 7
                    , parts_width, parts_height
                    , allface, (32 * 8) - (parts_width * 1), parts_height*2);//中下
                ImageUtil.BitBlt(halfbody, parts_width * 2 + (parts_width / 2), parts_height * 7
                    , parts_width, parts_height
                    , allface, (32 * 8) - (parts_width * 1), parts_height * 3);//右下
                ImageUtil.BitBlt(halfbody, parts_width * 3 , 0
                    , 8, parts_height
                    , allface, 208, parts_height * 3);//右髪の端
                ImageUtil.BitBlt(halfbody, 24, 0
                    , 8, parts_height
                    , allface, 216, parts_height * 2);//左髪の端

                face.Dispose();
                return halfbody;
            }
            return face;
        }

        public static Bitmap DrawPortraitUnit(uint id)
        {
            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                //ない.
                return ImageUtil.BlankDummy();
            }

            uint unit_face = Program.ROM.u32(addr);
            uint palette = Program.ROM.u32(addr + 8);
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                uint mouth_x = Program.ROM.u8(addr + 12);
                uint mouth_y = Program.ROM.u8(addr + 13);
                return ImagePortraitFE6Form.DrawPortraitUnitFE6(unit_face, palette, mouth_x, mouth_y, 0);
            }
            else
            {
                uint mouth = (uint)Program.ROM.p32(addr + 12);
                byte mouth_x = (byte)Program.ROM.u8(addr + 20);
                byte mouth_y = (byte)Program.ROM.u8(addr + 21);
                byte eye_x = (byte)Program.ROM.u8(addr + 22);
                byte eye_y = (byte)Program.ROM.u8(addr + 23);
                byte state = (byte)Program.ROM.u8(addr + 24);
                byte bool_mug_exceed = (byte)Program.ROM.u8(addr + 26);
                uint mug_exceed_class = (uint)Program.ROM.u32(addr + 16);
                return DrawPortraitUnit(unit_face, palette, mouth, mouth_x, mouth_y, eye_x, eye_y, bool_mug_exceed, mug_exceed_class, state, 0);
            }
        }


        public static Bitmap DrawPortraitClass(
              uint class_face
            , uint palette
            )
        {
            int width = 10 * 8;
            int height = 10 * 8;

            class_face = U.toOffset(class_face);
            palette = U.toOffset(palette);
            if (!U.isSafetyOffset(class_face)
                || !U.isSafetyOffset(palette)  
            )
            {
                //ない.
                return ImageUtil.Blank(width, height);
            }

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, class_face);
            if (imageUZ.Length <= 0)
            {//ない
                return ImageUtil.Blank(width, height);
            }
            height = ImageUtil.CalcHeight(width , imageUZ.Length);
            if (height <= 0)
            {
                return ImageUtil.Blank(width, height);
            }

            return ImageUtil.ByteToImage16Tile(width, height
                , imageUZ, 0
                , Program.ROM.Data, (int)palette
                , 0
            );
        }

        public static Bitmap DrawPortraitMap( uint id)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                return ImagePortraitFE6Form.DrawPortraitFE6Map(id);
            }


            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);
            return DrawPortraitMap( map_face, palette);
        }
        public static Bitmap DrawPortraitMap(
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

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, map_face);
            if (imageUZ.Length <= 0)
            {
                //ない.
                return ImageUtil.Blank(width, height);
            }

            return ImageUtil.ByteToImage16Tile(width, height
                , imageUZ, 0
                , Program.ROM.Data, (int)palette
                , 0
            );
        }
        public static Bitmap DrawPortraitClass(uint id)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                return ImagePortraitFE6Form.DrawPortraitClassFE6(id);
            }

            InputFormRef InputFormRef = Init(null);
            uint baseaddress = InputFormRef.BaseAddress;
            uint blocksize = InputFormRef.BlockSize;
            //現在のIDに対応するデータ
            uint addr = baseaddress + (id * blocksize);
            if (! U.isSafetyOffset(addr + 16 + 4))
            {
                return ImageUtil.BlankDummy();
            }

            uint class_face = Program.ROM.u32(addr + 16);
            uint palette = Program.ROM.u32(addr + 8);

            return DrawPortraitClass(class_face, palette);
        }
        public static Bitmap DrawPortraitAuto(uint id)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                return ImagePortraitFE6Form.DrawPortraitAuto(id);
            }
            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint unit_face = Program.ROM.u32(addr);
            uint palette = Program.ROM.u32(addr + 8);

            uint class_face = Program.ROM.u32(addr + 16);
            uint mouth = (uint)Program.ROM.p32(addr + 12);
            byte mouth_x = (byte)Program.ROM.u8(addr + 20);
            byte mouth_y = (byte)Program.ROM.u8(addr + 21);
            byte eye_x = (byte)Program.ROM.u8(addr + 22);
            byte eye_y = (byte)Program.ROM.u8(addr + 23);
            byte state = (byte)Program.ROM.u8(addr + 24);
            byte bool_mug_exceed = (byte)Program.ROM.u8(addr + 26);

            if (unit_face != 0)
            {
                return DrawPortraitUnit(unit_face, palette, mouth, mouth_x, mouth_y, eye_x, eye_y, bool_mug_exceed , class_face, state, 0);
            }
            else
            {
                return DrawPortraitClass( class_face, palette);
            }
        }
        //顔画像リストを得る
        public static List<U.AddrResult> MakePortraitList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        //顔画像リストを得る
        public static List<U.AddrResult> MakePortraitList(Func<uint, bool> condCallback)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList(condCallback);
        }

        const int seet_width = (12 + 4) * 8;
        const int seet_height = (10 + 4) * 8;

        const int face_width = 12 * 8;
        const int face_height = 10 * 8;

        const int parts_width = 8 * 4;
        const int parts_height = 8 * 2;

        const int mapface_width = 4 * 8;
        const int mapface_height = 4 * 8;

        //顔画像シートを作成する.
        static Bitmap DrawPortraitSeet(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            PatchUtil.portrait_extends portraitExtends = PatchUtil.SearchPortraitExtends();

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint unit_face = Program.ROM.u32(addr);
            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);

            uint mouth = Program.ROM.u32(addr + 12);
            uint class_face = Program.ROM.u32(addr + 16);
            if (unit_face == 0)
            {
                return DrawPortraitClass(class_face, palette);
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

            Bitmap allface;
            if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
            {//FE7Uは圧縮されている  (FE6も圧縮ただし結構違うので別ルーチン)
                //圧縮   FE7U FE6
                //無圧縮 FE7 FE8 FE8U
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, unit_face);
                if (imageUZ.Length <= 0)
                {//ない
                    return ImageUtil.Blank(seet_width, seet_height);
                }

                allface = ImageUtil.ByteToImage16Tile(32 * 8, 5 * 8
                    , imageUZ, 0
                    , Program.ROM.Data, (int)palette
                    );
            }
            else
            {
                if (IsHalfBodyFlag(unit_face) )
                {
                    return DrawPortraitSeetHalfBody(addr);
                }
                unit_face += 4; //謎の4バイトをスキップ.

                allface = ImageUtil.ByteToImage16Tile(32 * 8, 5 * 8
                    , Program.ROM.Data, (int)unit_face
                    , Program.ROM.Data, (int)palette
                    );
            }

            //シートを作る
            Bitmap seet = ImageUtil.Blank(seet_width, seet_height, allface);

            //いちばんでかい顔を描画
            Bitmap face = ImageUtil.Blank(face_width, face_height, allface);
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

            //メインの顔を描画
            ImageUtil.BitBlt(seet, 0, 0, face_width, face_height, face, 0, 0);

            //マップ顔を描画
            map_face = U.toOffset(map_face);
            byte[] map_faceUZ = LZ77.decompress(Program.ROM.Data, map_face);
            Bitmap mapface_bitmap = ImageUtil.ByteToImage16Tile(mapface_width, mapface_height
                , map_faceUZ, 0
                , Program.ROM.Data, (int)palette
                , 0
            );
            //シートにマップ顔を転写
            ImageUtil.BitBlt(seet, face_width, parts_height, mapface_width, mapface_height, mapface_bitmap, 0, 0);
            
            //目の部分を描画
            //とろん目
            ImageUtil.BitBlt(seet, face_width, mapface_height + parts_height
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 0);

            //目を閉じる
            ImageUtil.BitBlt(seet, face_width, mapface_height + parts_height * 2
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 1);


            //口
            mouth = U.toOffset(mouth);
            Bitmap mouthBitmap = ImageUtil.ByteToImage16Tile(parts_width, parts_height * 6
                    , Program.ROM.Data, (int)mouth
                    , Program.ROM.Data, (int)palette
                    );
            ImageUtil.BitBlt(seet, parts_width * 0, face_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 0);
            ImageUtil.BitBlt(seet, parts_width * 1, face_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 1);
            ImageUtil.BitBlt(seet, parts_width * 2, face_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 2);
            ImageUtil.BitBlt(seet, parts_width * 0, face_height + parts_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 3);
            ImageUtil.BitBlt(seet, parts_width * 1, face_height + parts_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 4);
            ImageUtil.BitBlt(seet, parts_width * 2, face_height + parts_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 5);

            //右端の口 は顔画像でーたの方にある
            ImageUtil.BitBlt(seet, parts_width * 3, face_height, parts_width, parts_height
                , allface, 32 * 8 - parts_width, 0);
            //余白
            ImageUtil.BitBlt(seet, parts_width * 3, face_height + parts_height, parts_width, parts_height
                , allface, 32 * 8 - parts_width, parts_height);

            return seet;
        }
        static Bitmap DrawPortraitSeetHalfBody(uint addr)
        {
            uint unit_face = Program.ROM.u32(addr);
            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);

            uint mouth = Program.ROM.u32(addr + 12);
            uint class_face = Program.ROM.u32(addr + 16);
            if (unit_face == 0)
            {
                return DrawPortraitClass(class_face, palette);
            }

            unit_face = U.toOffset(unit_face);
            palette = U.toOffset(palette);

            if (unit_face == 0
                || !U.isSafetyOffset(unit_face)
                || !U.isSafetyOffset(palette)
                )
            {
                //ない.
                return ImageUtil.Blank(160, 160);
            }

            Bitmap allface;
            if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
            {//FE7Uは圧縮されている  (FE6も圧縮ただし結構違うので別ルーチン)
                //圧縮   FE7U FE6
                //無圧縮 FE7 FE8 FE8U
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, unit_face);
                if (imageUZ.Length <= 0)
                {//ない
                    return ImageUtil.Blank(seet_width, seet_height);
                }

                allface = ImageUtil.ByteToImage16Tile(32 * 8, (5 + 3) * 8
                    , imageUZ, 0
                    , Program.ROM.Data, (int)palette
                    );
            }
            else
            {
                unit_face += 4; //謎の4バイトをスキップ.

                allface = ImageUtil.ByteToImage16Tile(32 * 8, (5 + 3) * 8
                    , Program.ROM.Data, (int)unit_face
                    , Program.ROM.Data, (int)palette
                    );
            }

            //シートを作る
            Bitmap seet = ImageUtil.Blank(160, 160, allface);

            //いちばんでかい顔を描画
            Bitmap face = ImageUtil.Blank(face_width, face_height, allface);
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

            //メインの顔を描画
            ImageUtil.BitBlt(seet, 16, 0, face_width, face_height, face, 0, 0);

            //マップ顔を描画
            map_face = U.toOffset(map_face);
            byte[] map_faceUZ = LZ77.decompress(Program.ROM.Data, map_face);
            Bitmap mapface_bitmap = ImageUtil.ByteToImage16Tile(mapface_width, mapface_height
                , map_faceUZ, 0
                , Program.ROM.Data, (int)palette
                , 0
            );
            //シートにマップ顔を転写
            ImageUtil.BitBlt(seet, 128, 64, mapface_width, mapface_height, mapface_bitmap, 0, 0);

            //目の部分を描画
            //とろん目
            ImageUtil.BitBlt(seet, 128, 96
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 0);

            //目を閉じる
            ImageUtil.BitBlt(seet, 128, 112
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 1);


            //口
            mouth = U.toOffset(mouth);
            Bitmap mouthBitmap = ImageUtil.ByteToImage16Tile(parts_width, parts_height * 6
                    , Program.ROM.Data, (int)mouth
                    , Program.ROM.Data, (int)palette
                    );
            ImageUtil.BitBlt(seet, parts_width * 0, 128, parts_width, parts_height, mouthBitmap, 0, parts_height * 0);
            ImageUtil.BitBlt(seet, parts_width * 1, 128, parts_width, parts_height, mouthBitmap, 0, parts_height * 1);
            ImageUtil.BitBlt(seet, parts_width * 2, 128, parts_width, parts_height, mouthBitmap, 0, parts_height * 2);
            ImageUtil.BitBlt(seet, parts_width * 0, 128 + parts_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 3);
            ImageUtil.BitBlt(seet, parts_width * 1, 128 + parts_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 4);
            ImageUtil.BitBlt(seet, parts_width * 2, 128 + parts_height, parts_width, parts_height, mouthBitmap, 0, parts_height * 5);

            //右端の口 は顔画像でーたの方にある
            ImageUtil.BitBlt(seet, parts_width * 3, 128, parts_width, parts_height
                , allface, 32 * 8 - parts_width, 0);

            //余白は不要
            
            //ハーフボディ拡張部
            ImageUtil.BitBlt(seet, 0, parts_height * 5
                , parts_width * 4, parts_height * 2
                , allface, 0, parts_height * 2);//ハーフボディの土台
            ImageUtil.BitBlt(seet, parts_width / 2, parts_height * 7
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 1), parts_height);//左下
            ImageUtil.BitBlt(seet, 0, parts_height * 7
                , parts_width / 2, parts_height
                , allface, parts_width * 4, parts_height * 2);//左端
            ImageUtil.BitBlt(seet, parts_width * 3 + (parts_width / 2), parts_height * 7
                , parts_width / 2, parts_height
                , allface, parts_width * 4, parts_height * 3);//右端
            ImageUtil.BitBlt(seet, parts_width / 2, parts_height * 1
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 4 + (parts_width / 2), parts_height * 2);//左上  
            ImageUtil.BitBlt(seet, parts_width * 3, parts_height * 1
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5, parts_height * 2);//右上  
            ImageUtil.BitBlt(seet, parts_width * 3 + (parts_width / 2), parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5 + (parts_width / 2), parts_height * 2);//右肩
            ImageUtil.BitBlt(seet, 0, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 6, parts_height * 2);//左肩
            ImageUtil.BitBlt(seet, parts_width * 1 + (parts_width / 2), parts_height * 7
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 1), parts_height * 2);//中下
            ImageUtil.BitBlt(seet, parts_width * 2 + (parts_width / 2), parts_height * 7
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 1), parts_height * 3);//右下
            ImageUtil.BitBlt(seet, parts_width * 3, 0
                , 8, parts_height
                , allface, 208, parts_height * 3);//右髪の端
            ImageUtil.BitBlt(seet, 24, 0
                , 8, parts_height
                , allface, 216, parts_height * 2);//左髪の端

            return seet;
        }
        static void BuildPortraitSeet(uint id, Bitmap seet)
        { 
            Bitmap allface;
            Bitmap map_face;
            Bitmap mouth;
            BuildPortraitSeet(seet, out allface, out map_face, out mouth);
        }


        static void BuildPortraitSeet(Bitmap seet, out Bitmap allface, out Bitmap map_face, out Bitmap mouth)
        {
            //インポートできる顔を作る
            allface = ImageUtil.Blank(32 * 8, 4 * 8, seet);

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

            //目の部分を描画
            //とろん目
            ImageUtil.BitBltRev(seet, face_width, mapface_height + parts_height
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 0);

            //目を閉じる
            ImageUtil.BitBltRev(seet, face_width, mapface_height + parts_height * 2
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 1);


            //口
            mouth = ImageUtil.Blank(parts_width, parts_height * 6, allface);
            ImageUtil.BitBltRev(seet, parts_width * 0, face_height, parts_width, parts_height, mouth, 0, parts_height * 0);
            ImageUtil.BitBltRev(seet, parts_width * 1, face_height, parts_width, parts_height, mouth, 0, parts_height * 1);
            ImageUtil.BitBltRev(seet, parts_width * 2, face_height, parts_width, parts_height, mouth, 0, parts_height * 2);
            ImageUtil.BitBltRev(seet, parts_width * 0, face_height + parts_height, parts_width, parts_height, mouth, 0, parts_height * 3);
            ImageUtil.BitBltRev(seet, parts_width * 1, face_height + parts_height, parts_width, parts_height, mouth, 0, parts_height * 4);
            ImageUtil.BitBltRev(seet, parts_width * 2, face_height + parts_height, parts_width, parts_height, mouth, 0, parts_height * 5);

            //右端の口 は顔画像でーたの方にある
            ImageUtil.BitBltRev(seet, parts_width * 3, face_height, parts_width, parts_height
                , allface, 32 * 8 - parts_width, 0);
            //余白
            ImageUtil.BitBltRev(seet, parts_width * 3, face_height + parts_height, parts_width, parts_height
                , allface, 32 * 8 - parts_width, parts_height);
        }

        static void BuildPortraitSeetHalfBody(Bitmap seet, out Bitmap allface, out Bitmap map_face, out Bitmap mouth)
        {
            //インポートできる顔を作る
            allface = ImageUtil.Blank(32 * 8, (4 + 4) * 8, seet);

            //メインの顔を逆変換
            ImageUtil.BitBltRev(seet, parts_width / 2 + 16, 0
                , parts_width * 2, parts_height * 2
                , allface, 0, 0);//顔上
            ImageUtil.BitBltRev(seet, parts_width / 2 + 16, parts_height * 2
                , parts_width * 2, parts_height * 2
                , allface, parts_width * 2, 0);//顔下
            ImageUtil.BitBltRev(seet, parts_width / 2 + 16, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, 0);//右肩
            ImageUtil.BitBltRev(seet, 0 + 16, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5, 0);//右端
            ImageUtil.BitBltRev(seet, parts_width + parts_width / 2 + 16, parts_height * 4
                , parts_width, parts_height
                , allface, parts_width * 4, parts_height);//左肩
            ImageUtil.BitBltRev(seet, parts_width * 2 + parts_width / 2 + 16, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5 + parts_width / 2, 0);//左端

            //マップ顔を転写
            map_face = ImageUtil.Blank(mapface_width, mapface_height, allface);
            ImageUtil.BitBlt(map_face, 0, 0, mapface_width, mapface_height, seet, 128, 64);


            //目の部分を描画
            //とろん目
            ImageUtil.BitBltRev(seet, 128, 96
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 0);

            //目を閉じる
            ImageUtil.BitBltRev(seet, 128, 112
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 2), parts_height * 1);


            //口
            mouth = ImageUtil.Blank(parts_width, parts_height * 6, allface);
            ImageUtil.BitBltRev(seet, parts_width * 0, 128, parts_width, parts_height, mouth, 0, parts_height * 0);
            ImageUtil.BitBltRev(seet, parts_width * 1, 128, parts_width, parts_height, mouth, 0, parts_height * 1);
            ImageUtil.BitBltRev(seet, parts_width * 2, 128, parts_width, parts_height, mouth, 0, parts_height * 2);
            ImageUtil.BitBltRev(seet, parts_width * 0, 128 + parts_height, parts_width, parts_height, mouth, 0, parts_height * 3);
            ImageUtil.BitBltRev(seet, parts_width * 1, 128 + parts_height, parts_width, parts_height, mouth, 0, parts_height * 4);
            ImageUtil.BitBltRev(seet, parts_width * 2, 128 + parts_height, parts_width, parts_height, mouth, 0, parts_height * 5);

            //右端の口 は顔画像でーたの方にある
            ImageUtil.BitBltRev(seet, parts_width * 3, 128, parts_width, parts_height
                , allface, 32 * 8 - parts_width, 0);

            //余白は不要

            //ハーフボディ拡張部
            ImageUtil.BitBltRev(seet, 0, parts_height * 5
                , parts_width * 4, parts_height * 2
                , allface, 0, parts_height * 2);//ハーフボディの土台
            ImageUtil.BitBltRev(seet, parts_width / 2, parts_height * 7
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 1), parts_height);//左下
            ImageUtil.BitBltRev(seet, 0, parts_height * 7
                , parts_width / 2, parts_height
                , allface, parts_width * 4, parts_height * 2);//左端
            ImageUtil.BitBltRev(seet, parts_width * 3 + (parts_width / 2), parts_height * 7
                , parts_width / 2, parts_height
                , allface, parts_width * 4, parts_height * 3);//右端
            ImageUtil.BitBltRev(seet, parts_width / 2, parts_height * 1
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 4 + (parts_width / 2), parts_height * 2);//左上  
            ImageUtil.BitBltRev(seet, parts_width * 3, parts_height * 1
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5, parts_height * 2);//右上  
            ImageUtil.BitBltRev(seet, parts_width * 3 + (parts_width / 2), parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 5 + (parts_width / 2), parts_height * 2);//右肩
            ImageUtil.BitBltRev(seet, 0, parts_height * 3
                , parts_width / 2, parts_height * 2
                , allface, parts_width * 6, parts_height * 2);//左肩
            ImageUtil.BitBltRev(seet, parts_width * 1 + (parts_width / 2), parts_height * 7
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 1), parts_height * 2);//中下
            ImageUtil.BitBltRev(seet, parts_width * 2 + (parts_width / 2), parts_height * 7
                , parts_width, parts_height
                , allface, (32 * 8) - (parts_width * 1), parts_height * 3);//右下
            ImageUtil.BitBltRev(seet, parts_width * 3, 0
                , 8, parts_height
                , allface, 208, parts_height * 3);//右髪の端
            ImageUtil.BitBltRev(seet, 24, 0
                , 8, parts_height
                , allface, 216, parts_height * 2);//左髪の端
        }

        //顔シートの出力
        private void ExportButton_Click(object sender, EventArgs e)
        {
            uint unit_face = (uint)this.D0.Value;
            if (unit_face == 0)
            {
                Bitmap classbitmap = DrawPortraitClass((uint)this.AddressList.SelectedIndex);
                ImageFormRef.ExportImage(this, classbitmap, InputFormRef.MakeSaveImageFilename());
                return;
            }

            string filename = ImageFormRef.SaveDialogPngOrGIF(InputFormRef);
            if (filename == "")
            {
                return;
            }

            string ext = U.GetFilenameExt(filename);
            if (ext == ".GIF")
            {
                bool r = SaveAnimeGif(filename, (uint)this.AddressList.SelectedIndex);
                if (!r)
                {
                    return;
                }
            }
            else
            {
                Bitmap seetbitmap = DrawPortraitSeet((uint)this.AddressList.SelectedIndex);
                ImageUtil.BlackOutUnnecessaryColors(seetbitmap, 1);
                U.BitmapSave(seetbitmap, filename);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        Bitmap Convert16Color(Bitmap fullColor, string imagefilename)
        {
            Bitmap seetbitmap;
            if (ImageUtil.Is16ColorBitmap(fullColor))
            {
                seetbitmap = ImageUtil.OpenBitmap(imagefilename); //16色に変換して開く.
                if (seetbitmap == null)
                {
                    return null;
                }
                seetbitmap = ImageUtil.ConvertPaletteTransparentUI(seetbitmap);
                if (seetbitmap == null)
                {
                    return null;
                }
            }
            else
            {//取り込めないので提案をする.
                ImagePortraitImporterForm f = (ImagePortraitImporterForm)InputFormRef.JumpFormLow<ImagePortraitImporterForm>();
                f.SetOrignalImage(fullColor
                    , (int)this.B22.Value, (int)this.B23.Value
                    , (int)this.B20.Value, (int)this.B21.Value);
                f.ShowDialog();

                seetbitmap = f.GetResultBitmap();
                if (seetbitmap == null)
                {
                    return null;
                }

                if (f.IsDetailMode())
                {
                    this.B22.Value = f.GetEyeBlockX();
                    this.B23.Value = f.GetEyeBlockY();
                    this.B20.Value = f.GetMouthBlockX();
                    this.B21.Value = f.GetMouthBlockY();
                }
            }
            return seetbitmap;
        }

        void ImportFaceImage(Bitmap fullColor, string imagefilename)
        {
            Bitmap seetbitmap = Convert16Color(fullColor, imagefilename);
            if (seetbitmap == null)
            {
                return;
            }

            Bitmap seet;
            Bitmap map_face;
            Bitmap mouth;
            BuildPortraitSeet(seetbitmap, out seet, out map_face, out mouth);

            byte[] seet_image = ImageUtil.ImageToByte16Tile(seet, seet.Width, seet.Height);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
            {//FE7Uは圧縮されている  (FE6も圧縮ただし結構違うので別ルーチン)
                //圧縮   FE7U FE6
                //無圧縮 FE7 FE8 FE8U
                this.InputFormRef.WriteImageData(this.D0, seet_image, true, undodata);
            }
            else
            {
                seet_image = U.ArrayInsert(seet_image, 0, new byte[] { 0x00, 0x04, 0x10, 0x00 });
                this.InputFormRef.WriteImageData(this.D0, seet_image, false, undodata);
            }

            byte[] map_face_image = ImageUtil.ImageToByte16Tile(map_face, map_face.Width, map_face.Height);
            byte[] mouth_image = ImageUtil.ImageToByte16Tile(mouth, mouth.Width, mouth.Height);
            byte[] palette = ImageUtil.ImageToPalette(seet, 1);

            this.InputFormRef.WriteImageData(this.D4, map_face_image, true, undodata); //マップ顔だけ圧縮.
            this.InputFormRef.WriteImageData(this.D8, palette, false, undodata);
            this.InputFormRef.WriteImageData(this.D12, mouth_image, false, undodata);
            Program.Undo.Push(undodata);

            //クラス顔は使わないので0を入れる.
            this.D16.Value = 0;

            //位置調整してねというメッセージを表示
            this.DescriptionAfterImportLabel.Show();

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        static bool IsHalfBodyFlag(uint unit_face)
        {
            unit_face = U.toOffset(unit_face);
            if (!U.isSafetyOffset(unit_face + 4))
            {
                return false;
            }
            uint faceHeader = Program.ROM.u32(unit_face);
            return (faceHeader == 0x00200400);
        }

        void ImportHalfBody(Bitmap fullColor, string imagefilename)
        {
            Bitmap seetbitmap = Convert16Color(fullColor, imagefilename);
            if (seetbitmap == null)
            {
                return;
            }

            Bitmap seet;
            Bitmap map_face;
            Bitmap mouth;
            BuildPortraitSeetHalfBody(seetbitmap, out seet, out map_face, out mouth);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            PatchUtil.portrait_extends portraitExtends = PatchUtil.SearchPortraitExtends();
            byte[] header4;
            byte[] palette;
            if (Program.ROM.RomInfo.version() != 8
                || portraitExtends != FEBuilderGBA.PatchUtil.portrait_extends.HALFBODY)
            {//HALFBODYを利用できない環境の場合 拡張部分を削る
                seet = ImageUtil.Copy(seet, 0, 0, 32 * 8, 4 * 8);
                palette = ImageUtil.ImageToPalette(seet, 1);
                header4 = new byte[] { 0x00, 0x04, 0x10, 0x00 };
            }
            else
            {//利用できる環境
                palette = ImageUtil.ImageToPalette(seet, 2);

                //ハーフボディ用のフラグとして 0x20をセットします.
                header4 = new byte[] { 0x00, 0x04, 0x20, 0x00 };

                //現在のデータは、ハーフボディ用の領域を持っていますか?
                if (! IsHalfBodyFlag((uint)this.D0.Value))
                {//HALFBODY用のデータをもっていないので、一度無効にしたい.
                    this.D0.Value = 0;
                    this.D8.Value = 0;
                }
            }

            byte[] seet_image = ImageUtil.ImageToByte16Tile(seet, seet.Width, seet.Height);
            
            seet_image = U.ArrayInsert(seet_image, 0, header4);
            this.InputFormRef.WriteImageData(this.D0, seet_image, false, undodata);

            byte[] map_face_image = ImageUtil.ImageToByte16Tile(map_face, map_face.Width, map_face.Height);
            byte[] mouth_image = ImageUtil.ImageToByte16Tile(mouth, mouth.Width, mouth.Height);

            this.InputFormRef.WriteImageData(this.D4, map_face_image, true, undodata); //マップ顔だけ圧縮.
            this.InputFormRef.WriteImageData(this.D8, palette, false, undodata);
            this.InputFormRef.WriteImageData(this.D12, mouth_image, false, undodata);
            Program.Undo.Push(undodata);

            //クラス顔は使わないので0を入れる.
            this.D16.Value = 0;

            //位置調整してねというメッセージを表示
            this.DescriptionAfterImportLabel.Show();

            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }
        void ImportClassCard(Bitmap fullColor, string imagefilename)
        {
            Bitmap seetbitmap = ImageUtil.OpenBitmap(imagefilename); //16色に変換して開く.
            if (seetbitmap == null)
            {
                return;
            }
            seetbitmap = ImageUtil.ConvertPaletteTransparentUI(seetbitmap);
            if (seetbitmap == null)
            {
                return;
            }

            byte[] class_image = ImageUtil.ImageToByte16Tile(seetbitmap, seetbitmap.Width, seetbitmap.Height);
            byte[] palette = ImageUtil.ImageToPalette(seetbitmap, 1);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            this.InputFormRef.WriteImageData(this.D8, palette, false, undodata);
            this.InputFormRef.WriteImageData(this.D16, class_image, true, undodata); //クラス画像は圧縮されている.
            Program.Undo.Push(undodata);

            //顔パラメーターは使わないので0を入れる.
            this.D0.Value = 0;
            this.D4.Value = 0;
            this.D12.Value = 0;
            this.B20.Value = 0;
            this.B21.Value = 0;
            this.B22.Value = 0;
            this.B23.Value = 0;
            this.B24.Value = 0;

            //ポインタの書き込み
            this.WriteButton.PerformClick();
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
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    ImportFaceImage(fullColor, imagefilename);
                }
            }
            else if (fullColor.Width == 160 && fullColor.Height == 160)
            {
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    ImportHalfBody(fullColor, imagefilename);
                }
            }
            else if (fullColor.Width == 80 && 
                (fullColor.Height == 80 || fullColor.Height == 72) )
            {
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    ImportClassCard(fullColor, imagefilename);
                }
            }
            else
            {
                R.ShowStopError("画像サイズが正しくありません。\r\n顔画像ならば、128x112 \r\nクラス画像ならば 80x80 である必要があります。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", fullColor.Width, fullColor.Height, Width, Height);
            }
            fullColor.Dispose();
        }

        public static void RecyclePortrait(ref List<Address> recycle, string basename, bool isPointerOnly,uint portrait_addr)
        {
            if (!U.isSafetyOffset(portrait_addr))
            {
                return ;
            }
            uint seet_image = Program.ROM.p32(portrait_addr + 0);
            uint map_face = Program.ROM.p32(portrait_addr + 4);
            uint palette_face = Program.ROM.p32(portrait_addr + 8);
            uint mouth_face = Program.ROM.p32(portrait_addr + 12);
            uint class_face = Program.ROM.p32(portrait_addr + 16);
            bool isHalfBodyExtends = false;

            if (U.isSafetyOffset(seet_image))
            {
                isHalfBodyExtends = (Program.ROM.RomInfo.version() == 8 && IsHalfBodyFlag(seet_image));

                //4バイトヘッダ+無圧縮
                //圧縮   FE7U FE6
                //無圧縮 FE7 FE8 FE8U
                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
                {//FE7Uは圧縮されている  (FE6も圧縮ただし結構違うので別ルーチン)
                    FEBuilderGBA.Address.AddLZ77Pointer(recycle
                        , portrait_addr + 0
                        , basename + "FACE"
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                }
                else if (isHalfBodyExtends)
                {//HalfBody
                    FEBuilderGBA.Address.AddPointer(recycle
                        , portrait_addr + 0
                        , 0x4 + 0x2000
                        , basename + "FACE HALFBODY"
                        , FEBuilderGBA.Address.DataTypeEnum.IMG);
                }
                else
                {//無圧縮 FE7 FE8 FE8U
                    FEBuilderGBA.Address.AddPointer(recycle
                        , portrait_addr + 0
                        , 0x4 + 0x1000
                        , basename + "FACE"
                        , FEBuilderGBA.Address.DataTypeEnum.IMG);
                }
            }
            if (U.isSafetyOffset(map_face))
            {
                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , portrait_addr + 4
                    , basename + "MAP FACE"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            }
            if (U.isSafetyOffset(palette_face))
            {
                if (isHalfBodyExtends)
                {//HalfBody
                    FEBuilderGBA.Address.AddPointer(recycle
                        , portrait_addr + 0
                        , 0x40
                        , basename + "PAL HALFBODY"
                        , FEBuilderGBA.Address.DataTypeEnum.IMG);
                }
                else
                {
                    FEBuilderGBA.Address.AddPointer(recycle
                        , portrait_addr + 8
                        , 0x20
                        , basename + "PAL"
                        , FEBuilderGBA.Address.DataTypeEnum.PAL); //16色パレット
                }
            }
            if (U.isSafetyOffset(mouth_face))
            {
                FEBuilderGBA.Address.AddPointer(recycle
                    , portrait_addr + 12
                    , (parts_width / 2) * parts_height * 6
                    , basename + "MOUTH"
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
            if (U.isSafetyOffset(class_face))
            {
                FEBuilderGBA.Address.AddLZ77Pointer(recycle
                    , portrait_addr + 16
                    , basename + "CLASS CARD"
                    , isPointerOnly
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            }
        }


        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "Portrait", new uint[] { 0 , 4, 8, 12, 16});

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    string name = "Portrait:" + U.To0xHexString(i);
                    RecyclePortrait(ref list, name, isPointerOnly, addr);
                }
            }
        }

        private void J_8_Click(object sender, EventArgs e)
        {
            ImagePalletForm f = (ImagePalletForm)InputFormRef.JumpForm<ImagePalletForm>(U.NOT_FOUND);

            Bitmap baseBitmap = ImagePortraitForm.DrawPortraitUnit((uint)this.AddressList.SelectedIndex);
            f.JumpTo(baseBitmap, (uint)D8.Value , 1);
            f.FormClosed += (s, ee) =>
            {
                if (this.IsDisposed)
                {
                    return;
                }
                U.ReSelectList(this.AddressList);
            };
        }
        public static uint DataCount()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
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
                uint seet_image = Program.ROM.p32(portrait_addr + 0);
                //uint map_face = Program.ROM.p32(portrait_addr + 4);
                //uint palette_face = Program.ROM.p32(portrait_addr + 8);
                //uint mouth_face = Program.ROM.p32(portrait_addr + 12);
                uint class_face = Program.ROM.p32(portrait_addr + 16);

                if (U.isSafetyOffset(seet_image))
                {//4バイトヘッダ+無圧縮
                    //圧縮   FE7U FE6
                    //無圧縮 FE7 FE8 FE8U
                    if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
                    {//FE7Uは圧縮されている  (FE6も圧縮ただし結構違うので別ルーチン)
                        FELint.CheckLZ77(seet_image, errors, FELint.Type.PORTRAIT, portrait_addr, id);
                    }
                    else
                    {//無圧縮
                        string headerError = CheckFaceHeader(seet_image);
                        if (headerError != "")
                        {
                            errors.Add(new FELint.ErrorSt(FELint.Type.PORTRAIT, portrait_addr
                                , headerError , id));
                        }
                    }
                }
                if (seet_image == 0)
                {
                    if (U.isSafetyOffset(class_face))
                    {
                        FELint.CheckLZ77(class_face, errors, FELint.Type.PORTRAIT, portrait_addr, id);
                    }
                }
            }
        }
        static string CheckFaceHeader(uint seet_image)
        {
            //圧縮   FE7U FE6
            //無圧縮 FE7 FE8 FE8U
            if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
            {//FE7Uは圧縮されている  (FE6も圧縮ただし結構違うので別ルーチン)
                //ヘッダはないので常にOKを返す
                return "";
            }
            //無圧縮
            uint head1 = Program.ROM.u8(seet_image + 0);
            uint head2 = Program.ROM.u8(seet_image + 1);
            uint head3 = Program.ROM.u8(seet_image + 2);
            uint head4 = Program.ROM.u8(seet_image + 3);
            if (head1 != 0x00)
            {
                return R._("顔画像のユニット画像の先頭4バイトのヘッダが壊れています。インポートしなおすことを推奨します。\r\nHeader1: Addr: {0} Msg: 規定値は0x00ですが、{1}になっています。", U.To0xHexString(seet_image + 0), U.To0xHexString(head1));
            }
            if (head2 != 0x04)
            {
                return R._("顔画像のユニット画像の先頭4バイトのヘッダが壊れています。インポートしなおすことを推奨します。\r\nHeader2: Addr: {0} Msg: 規定値は0x04ですが、{1}になっています。", U.To0xHexString(seet_image + 1), U.To0xHexString(head2));
            }
            if (head3 != 0x10)
            {
                PatchUtil.portrait_extends portraitExtends = PatchUtil.SearchPortraitExtends();
                if (portraitExtends == PatchUtil.portrait_extends.HALFBODY)
                {
                    if (head3 != 0x20)
                    {
                        return R._("顔画像のユニット画像の先頭4バイトのヘッダが壊れています。インポートしなおすことを推奨します。\r\nHeader3: Addr: {0} Msg: 規定値は0x10または0x20ですが、{1}になっています。", U.To0xHexString(seet_image + 2), U.To0xHexString(head3));
                    }
                }
                return R._("顔画像のユニット画像の先頭4バイトのヘッダが壊れています。インポートしなおすことを推奨します。\r\nHeader3: Addr: {0} Msg: 規定値は0x10ですが、{1}になっています。", U.To0xHexString(seet_image + 2), U.To0xHexString(head3));
            }
            if (head4 != 0x00)
            {
                return R._("顔画像のユニット画像の先頭4バイトのヘッダが壊れています。インポートしなおすことを推奨します。\r\nHeader4: Addr: {0} Msg: 規定値は0x00ですが、{1}になっています。", U.To0xHexString(seet_image + 3), U.To0xHexString(head4));
            }
            return "";
        }

        private void X_MUG_EXCEED_Button_Click(object sender, EventArgs e)
        {

        }

        bool IsMugExceedUpdate;
        private void MUG_EXCEED_B16_ValueChanged(object sender, EventArgs e)
        {
            if (this.IsMugExceedUpdate)
            {
                return;
            }

            PatchUtil.portrait_extends portraitExtends = PatchUtil.SearchPortraitExtends();
            if (portraitExtends != FEBuilderGBA.PatchUtil.portrait_extends.MUG_EXCEED)
            {
                return;
            }
            this.IsMugExceedUpdate = true;
            D16.Value =
            ((uint)MUG_EXCEED_B16.Value & 0xff)
            | (((uint)MUG_EXCEED_B17.Value & 0xff) << 8)
            | (((uint)MUG_EXCEED_B18.Value & 0xff) << 16)
            | (((uint)MUG_EXCEED_B19.Value & 0xff) << 24)
            ;
            this.IsMugExceedUpdate = false;
            //再描画
            AddressList_SelectedIndexChanged(sender, e);
        }

        private void D16_ValueChanged(object sender, EventArgs e)
        {
            if (this.IsMugExceedUpdate)
            {
                return;
            }
            PatchUtil.portrait_extends portraitExtends = PatchUtil.SearchPortraitExtends();
            if (portraitExtends != FEBuilderGBA.PatchUtil.portrait_extends.MUG_EXCEED)
            {
                return;
            }
            this.IsMugExceedUpdate = true;
            uint class_face = (uint)D16.Value;
            MUG_EXCEED_B16.Value = (class_face & 0xff);
            MUG_EXCEED_B17.Value = ((class_face >> 8) & 0xff);
            MUG_EXCEED_B18.Value = ((class_face >> 16) & 0xff);
            MUG_EXCEED_B19.Value = ((class_face >> 24) & 0xff);
            this.IsMugExceedUpdate = false;
            //再描画
            AddressList_SelectedIndexChanged(sender, e);
        }

        private void B26_ValueChanged(object sender, EventArgs e)
        {
            PatchUtil.portrait_extends portraitExtends = PatchUtil.SearchPortraitExtends();
            if (portraitExtends != FEBuilderGBA.PatchUtil.portrait_extends.MUG_EXCEED)
            {
                return;
            }

            if (B26.Value == 1)
            {
                X_MUG_EXCEED.Show();
            }
            else
            {
                X_MUG_EXCEED.Hide();
            }
            //再描画
            AddressList_SelectedIndexChanged(sender, e);
        }

        public static List<U.AddrResult> MakeList()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                return ImagePortraitFE6Form.MakeList();
            }
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        bool SaveAnimeGif(string filename, uint id)
        {
            InputFormRef InputFormRef = Init(null);

            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return false;
            }
            uint unit_face = Program.ROM.u32(addr + 0);
            uint map_face = Program.ROM.u32(addr + 4);
            uint palette = Program.ROM.u32(addr + 8);
            uint mouth = Program.ROM.u32(addr + 12);
            uint class_face = Program.ROM.u32(addr + 16);
            byte mouth_x = (byte)Program.ROM.u8(addr + 20);
            byte mouth_y = (byte)Program.ROM.u8(addr + 21);
            byte eye_x = (byte)Program.ROM.u8(addr + 22);
            byte eye_y = (byte)Program.ROM.u8(addr + 23);
            byte state = (byte)Program.ROM.u8(addr + 24);
            byte b26 = (byte)Program.ROM.u8(addr + 26) ;

            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();
            for (int showFrame = 0; showFrame < 9; showFrame++ )
            {
                Bitmap bitmap = DrawPortraitUnit(unit_face, palette, mouth, mouth_x, mouth_y, eye_x, eye_y, b26, class_face, state, showFrame);
                ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);
                uint wait = 10;
                if (showFrame == 0)
                {
                    wait = 30;
                }
                bitmaps.Add(new ImageUtilAnimeGif.Frame(bitmap, wait));
            }

            //アニメgif生成
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
            return true;
        }

        private void LinkInternt_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoMoreData();
        }

    }
}
