using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageItemIconForm : Form
    {
        public ImageItemIconForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawImageItemIconAndText, DrawMode.OwnerDrawFixed);
            this.X_ICON_REF_ITEM.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            this.X_ICON_REF_ITEM.ItemListToJumpForm("ITEM");

            if (Program.ROM.p32(Program.ROM.RomInfo.icon_pointer) == Program.ROM.RomInfo.icon_orignal_address)
            {
                this.ItemIconListExpandsButton.Enabled = true;
            }
            else
            {
                this.ItemIconListExpandsButton.Enabled = false;
            }
            
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            if (Program.ROM.RomInfo.version != 8)
            {
                this.InputFormRef.CheckProtectionAddrHigh = false;  //かなり手前のアドレスなので確認不可能
            }

            InputFormRef.markupJumpLabel(LinkInternt);

            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null,null);
                }
            });
        }

        static uint GetIconMax()
        {
            if (Program.ROM.p32(Program.ROM.RomInfo.icon_pointer) != Program.ROM.RomInfo.icon_orignal_address)
            {//リポイント済み
                return 0xFE;
            }
            if (Program.ROM.RomInfo.version == 7)
            {
                if (Program.ROM.RomInfo.is_multibyte == false)
                {//FE7Uでは、アイテムアイコンの中にFEditorAdv AutoPatchのデータがある
                    uint code = Program.ROM.u32(0xCB51A);
                    if (code == 0x18404902)
                    {//そのため、FE7UでFEditorAdv AutoPatchがあれば、個数は一つ下げる
                        return Program.ROM.RomInfo.icon_orignal_max - 1;
                    }
                }

            }

            return Program.ROM.RomInfo.icon_orignal_max;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            uint itemMax = GetIconMax();
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.icon_pointer
                , (2 * 8 * 2 * 8) / 2 // /2しているのは16色のため
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i <= itemMax;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageIconForm_Load(object sender, EventArgs e)
        {
            //アイテムアイコン拡張を表示するかどうか
            if (IsShowItemIconExetdns(this.AddressList.Items.Count))
            {
                ItemIconListExpandsButton.Show();
            }
            else
            {
                this.AddressList.Height += ItemIconListExpandsButton.Height;
                ItemIconListExpandsButton.Hide();
            }
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.X_ICON_PIC.Image = DrawIcon( (uint)this.Address.Value);
            this.X_ICON_BIG_PIC.Image = DrawIcon((uint)this.Address.Value);

            U.ConvertListBox(ItemForm.MakeItemListByUseIcon((uint)this.AddressList.SelectedIndex),
                ref this.X_ICON_REF_ITEM);
        }

        public static Bitmap DrawIcon(uint addr, uint customPalette = 0)
        {
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }
            if (customPalette == 0)
            {//アイテムアイコンパレットを利用.
                customPalette = Program.ROM.u32(Program.ROM.RomInfo.icon_palette_pointer);
            }

            return ImageUtil.ByteToImage16Tile(2 * 8, 2 * 8
                , Program.ROM.Data, (int)addr
                , Program.ROM.Data, (int)U.toOffset(customPalette)
                , 0
                );
        }

        public static Bitmap DrawIconWhereID(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy(16);
            }
            
           return  DrawIcon( addr);
        }

        public static Bitmap DrawIconWhereID_UsingWeaponPalette(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy(16);
            }

            uint palette = Program.ROM.u32(Program.ROM.RomInfo.system_weapon_icon_palette_pointer);
            return DrawIcon(addr, palette);
        }

        public static Bitmap DrawIconWhereID_UsingWeaponPalette_SKILLFE8NVer2(uint id)
        {
            if (id == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy(16);
            }

            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ(FE8NVer2はスキルアイコンのさらに下の領域に書き込まれる)
            uint addr = InputFormRef.BaseAddress + (InputFormRef.BlockSize * id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy(16);
            }

            uint palette = Program.ROM.u32(Program.ROM.RomInfo.system_weapon_icon_palette_pointer);
            return DrawIcon(addr, palette);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DrawIcon((uint)this.Address.Value);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }

            int width = 2 * 8;
            int height = 2 * 8;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width,bitmap.Height,width,height);
                return;
            }

            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , Program.ROM.p32(Program.ROM.RomInfo.icon_palette_pointer)
                        , Program.ROM.p32(Program.ROM.RomInfo.system_weapon_icon_palette_pointer)
                        , ""
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, Program.ROM.p32(Program.ROM.RomInfo.icon_palette_pointer)));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, Program.ROM.p32(Program.ROM.RomInfo.icon_palette_pointer)));
                    f.SetReOrderImage2(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, Program.ROM.p32(Program.ROM.RomInfo.system_weapon_icon_palette_pointer)));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            uint addr = (uint)this.Address.Value;

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap,width,height);


            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            Program.ROM.write_range(U.toOffset(addr), image, undodata);
            Program.Undo.Push(undodata);

            InputFormRef.ReloadAddressList();
            InputFormRef.ShowWriteNotifyAnimation(this, addr);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "ItemIcon";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list,InputFormRef, name, new uint[] {  });
            }
        }

        public static uint ExpandsArea(Form form, uint newdatacount, Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = Init(null);

            uint newdatasize = newdatacount;
            uint olddatasize = InputFormRef.DataCount;

            uint newaddr = InputFormRef.ExpandsArea(form
                , newdatasize
                , Program.ROM.RomInfo.icon_pointer
                , olddatasize
                , FEBuilderGBA.InputFormRef.ExpandsFillOption.FIRST 
                , InputFormRef.BlockSize
                , undodata);
            if (newaddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }

            return newaddr;
        }

        private void ItemIconListExpandsButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = R.ShowNoYes("拡張した領域にあるアイテムアイコンを利用するには、アイテムアイコン拡張のパッチが別途必要です。\r\nアイテムアイコンを拡張してもよろしいですか？\r\n");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.ItemIconExpands);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"expands");
            uint newaddr = ExpandsArea(this, 0xFF, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                R.ShowStopError("アイテムアイコンテーブルの拡張に失敗しました。");
                return;
            }
            Program.Undo.Push(undodata);

            InputFormRef.ReOpenForm<ImageItemIconForm>();
        }

        private void JumpToSystemPalette_Click(object sender, EventArgs e)
        {
            ImageSystemIconForm f = (ImageSystemIconForm)InputFormRef.JumpForm<ImageSystemIconForm>();
            f.JumpToPage(1);
        }

        bool IsShowItemIconExetdns(int count)
        {
            if (Program.ROM.RomInfo.version == 8)
            {//FE8では常に表示する
                return true;
            }

            if (OptionForm.show_fe76_item_icon_extends() == OptionForm.show_extends_enum.Show)
            {//表示する設定の場合は表示する.
                return true;
            }

            if (Program.ROM.p32(Program.ROM.RomInfo.icon_pointer) == Program.ROM.RomInfo.icon_orignal_address)
            {//拡張されていないので表示しない
                return false;
            }
            //拡張されているので表示する
            return true;
        }

        private void LinkInternt_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoMoreData();

        }


    }
}
