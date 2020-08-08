using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageBattleTerrainForm : Form
    {
        public ImageBattleTerrainForm()
        {
            InitializeComponent();
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
                , Program.ROM.RomInfo.battle_terrain_pointer()
                , 24
                , (int i, uint addr) =>
                {//読込最大値検索
                    //12 がポインタであればデータがあると考える.
                    return U.isPointer(Program.ROM.u32(addr + 12))
                        ;
                }
                , (int i, uint addr) =>
                {
                    String name = Program.ROM.getString(addr, 11);
                    return U.ToHexString(i) + U.SA(name) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageBattleTerrainForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_BG_PIC.Image = Draw((uint)D12.Value, (uint)D16.Value);
            U.ConvertListBox(MapTerrainFloorLookupTableForm.MakeListByUseTerrain((uint)this.AddressList.SelectedIndex), ref this.X_REF);
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

            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, U.toOffset(image));
            int height = ImageUtil.CalcHeight(32 * 8, imageUZ.Length);

            return ImageUtil.ByteToImage16Tile(32 * 8, height
                , imageUZ, 0
                , Program.ROM.Data, (int)U.toOffset(palette)
                );
        }
        public static Bitmap Draw(uint id)
        {
            if (id <= 0)
            {
                return ImageUtil.BlankDummy();
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id - 1);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }

            uint image = Program.ROM.u32(addr+12);
            uint palette = Program.ROM.u32(addr + 16);

            return Draw(image, palette);
        }
        public enum RangeType
        {
             Melee = 0
            ,Range_Player = 1
            ,Range_Enemy = 2
        };

        public static Bitmap DrawSquare(uint id, RangeType type)
        {
            Bitmap bitmap = Draw(id);
            Bitmap ret_bitmap = ImageUtil.Blank(ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8, bitmap);

            if (type == RangeType.Range_Player || type == RangeType.Range_Enemy)
            {
                //かなり複雑なので、パーツ単位で一度組み立ててから転写します.
                Bitmap parts = ImageUtil.Blank(13 * 8, 5 * 8, bitmap);
                ImageUtil.BitBlt(parts, 0 * 8, 3 * 8, 13 * 8, 2 * 8, bitmap, 0, 2 * 8);
                ImageUtil.BitBlt(parts, 0 * 8, 1 * 8, 12 * 8, 2 * 8, bitmap, 13 * 8, 2 * 8);
                ImageUtil.BitBlt(parts, 0 * 8, 0 * 8, 6 * 8, 1 * 8, bitmap, 25 * 8, 2 * 8);
                ImageUtil.BitBlt(parts, 6 * 8, 0 * 8, 4 * 8, 1 * 8, bitmap, 25 * 8, 3 * 8);

                //負の数での反転ルーチンに問題があるので、事前に反転しておく.
                Bitmap parts2 = ImageUtil.Copy(parts, 0, 0, 13 * 8, 5 * 8, true);

                if (type == RangeType.Range_Player)
                {
                    ImageUtil.BitBlt(ret_bitmap, 16 * 8, 11 * 8, 13 * 8, 5 * 8, parts, 0, 0);
                    ImageUtil.BitBlt(ret_bitmap, -3 * 8, 11 * 8, 13 * 8, 5 * 8, parts2, 0, 0);
                }
                else
                {
                    ImageUtil.BitBlt(ret_bitmap, 19 * 8, 11 * 8, 13 * 8, 5 * 8, parts, 0, 0);
                    ImageUtil.BitBlt(ret_bitmap,  0 * 8, 11 * 8, 13 * 8, 5 * 8, parts2, 0, 0);
                }

                parts.Dispose();
                parts2.Dispose();
            }
            else
            {
                ImageUtil.BitBlt(ret_bitmap, 4 * 8, 14 * 8, 11 * 8, 2 * 8, bitmap, 0, 0, 0, 0xff, true);
                ImageUtil.BitBlt(ret_bitmap, 5 * 8, 12 * 8, 10 * 8, 2 * 8, bitmap, 11 * 8, 0, 0, 0xff, true);
                ImageUtil.BitBlt(ret_bitmap, 6 * 8, 11 * 8, 9 * 8, 1 * 8, bitmap, 21 * 8, 0, 0, 0xff, true);

                ImageUtil.BitBlt(ret_bitmap, 15 * 8, 14 * 8, 11 * 8, 2 * 8, bitmap, 0, 0);
                ImageUtil.BitBlt(ret_bitmap, 15 * 8, 12 * 8, 10 * 8, 2 * 8, bitmap, 11 * 8, 0);
                ImageUtil.BitBlt(ret_bitmap, 15 * 8, 11 * 8, 9 * 8, 1 * 8, bitmap, 21 * 8, 0);
            }

            return ret_bitmap;
        }
        public static string GetBattleTerrainName(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return "";
            }
            String name = Program.ROM.getString(addr, 11) + InputFormRef.GetCommentSA(addr);
            return name;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = Draw((uint)D12.Value, (uint)D16.Value);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            int width = 256;
            int height = 8 * 4; //32
            int palette_count = 1;
            Bitmap bitmap = ImageUtil.LoadAndConvertDecolorUI(this, null, width, height, true, palette_count);
            if (bitmap == null)
            {
                return;
            }

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap,width ,height);
            byte[] palette = ImageUtil.ImageToPalette(bitmap, 1);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            this.InputFormRef.WriteImageData(this.D12, image, true, undodata);
            this.InputFormRef.WriteImageData(this.D16, palette, false, undodata);
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
            string selfname = "BattleTerrain";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, selfname, new uint[] { 12, 16});

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize)
            {
                selfname = "BattleTerrain 0x"+U.ToHexString(i);
                uint image = Program.ROM.p32(12 + addr);
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    ,12 + addr 
                    ,selfname
                    ,isPointerOnly
                    ,FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);

                FEBuilderGBA.Address.AddPointer(list
                    , 16 + addr
                    , 0x20 * 1
                    , selfname
                    , FEBuilderGBA.Address.DataTypeEnum.PAL);
            }
        }
        void JumpToRef()
        {
            MapTerrainFloorLookupTableForm.JumpToRef(X_REF.Text);
        }

        private void X_REF_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            JumpToRef();
        }

        private void X_REF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                JumpToRef();
            }
        }
    }
}
