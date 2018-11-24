using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageUnitPaletteForm : Form
    {
        public ImageUnitPaletteForm()
        {
            InitializeComponent();
            this.UNITCLASS_LIST.OwnerDraw(ListBoxEx.DrawUnitAndClassAndText, DrawMode.OwnerDrawFixed);
            if (Program.ROM.RomInfo.version()==8)
            {
                this.UNITCLASS_LIST.ItemListToJumpForm( "UNITPALETTEFE8", new string[] { "UID" });
            }
            else
            {
                this.UNITCLASS_LIST.ItemListToJumpForm( "UNIT");
            }

            this.InputFormRef = Init(this);
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEventNoCopyP12;
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.PaletteZoomComboBox.SelectedIndex = 0;
            this.PaletteIndexComboBox.SelectedIndex = 0;
            PaletteFormRef.MakePaletteUI(this, OnChangeColor);

            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.image_unit_palette_pointer()
                , 16
                , (int i, uint addr) =>
                {//読込最大値検索
                    //12 がポインタであればデータがあると考える.
                    uint p = Program.ROM.u32(addr + 12);
                    if (U.isPointer(p))
                    {
                        return true;
                    }
                    if (p == 0)
                    {//0は有効値だけど終端データもnullなのでその場合は、名前判定.
                        uint name = Program.ROM.u32(addr + 0);
                        if (p == 0)
                        {//名前もnullなのでデータではないと思われる.
                            return false;
                        }
                    }
                    //有効値
                    return true;
                }
                , (int i, uint addr) =>
                {
                    String name = Program.ROM.getString(addr,3);

                    return U.ToHexString(i + 1) + U.SA(name) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageUnitPaletteForm_Load(object sender, EventArgs e)
        {
        }

        public static string GetPaletteName(uint paletteid)
        {
            if (paletteid <= 0)
            {
                return "";
            }
            paletteid = paletteid - 1;

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(paletteid);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            return Program.ROM.getString(addr,3) + InputFormRef.GetCommentSA(addr);
        }
        public static uint GetPaletteAddr(uint paletteid)
        {
            if (paletteid <= 0)
            {
                return U.NOT_FOUND;
            }
            paletteid = paletteid - 1;

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(paletteid);
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }
            return Program.ROM.p32(addr+12);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint selectindex = (uint)this.AddressList.SelectedIndex;
            MakeClassList(selectindex);

            U.SelectedIndexSafety(this.UNITCLASS_LIST,0);
        }

        void MakeClassList(uint selectindex)
        {
            if (Program.ROM.RomInfo.version() >= 8)
            {//FE8の場合キャラパレット指定が別途用意されている
                uint unit_palette_color_pointer = Program.ROM.p32(Program.ROM.RomInfo.unit_palette_color_pointer());
                uint unit_palette_class_pointer = Program.ROM.p32(Program.ROM.RomInfo.unit_palette_class_pointer());

                List<U.AddrResult> list = new List<U.AddrResult>();
                for (int i = 0; i < Program.ROM.RomInfo.unit_maxcount(); i++)
                {
                    for (uint n = 0; n < 7; n++)
                    {
                        uint paletteid = Program.ROM.u8(unit_palette_color_pointer + n);
                        if (paletteid <= 0)
                        {
                            continue;
                        }
                        if (paletteid - 1 != selectindex)
                        {
                            continue;
                        }
                        uint uid = (uint)i+1;
                        uint cid = Program.ROM.u8(unit_palette_class_pointer + n);
                        string name = U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid) + " -> " + U.ToHexString(cid) + " " + ClassForm.GetClassName(cid);

                        list.Add(new U.AddrResult(cid, name, uid));
                    }

                    unit_palette_color_pointer += 7;
                    unit_palette_class_pointer += 7;
                }
                U.ConvertListBox(list, ref UNITCLASS_LIST);
            }
            else
            {//FE7 , FE6 はユニットの部分に色指定がある
                List<U.AddrResult> list = new List<U.AddrResult>();
                for (int i = 0; i < Program.ROM.RomInfo.unit_maxcount(); i++)
                {
                    uint uid = (uint)i;
                    uint paletteid1 = UnitForm.GetPaletteLowClass(uid);
                    uint paletteid2 = UnitForm.GetPaletteHighClass(uid);

                    if (paletteid1 > 0 && paletteid1 - 1 == selectindex)
                    {
                        uint cid = UnitForm.GetClassID(uid);
                        string name = U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid) + " -> " + U.ToHexString(cid) + " " + ClassForm.GetClassName(cid);
                        list.Add(new U.AddrResult(cid, name,uid));
                    }
                    else if (paletteid2 > 0 && paletteid2 - 1 == selectindex)
                    {
                        uint cid = UnitForm.GetHighClass(uid);
                        string name = U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid) + " -> " + U.ToHexString(cid) + " " + ClassForm.GetClassName(cid);
                        list.Add(new U.AddrResult(cid, name,uid));
                    }
                }
                U.ConvertListBox(list, ref UNITCLASS_LIST);
            }
        }

        private void PALETTE_POINTER_ValueChanged(object sender, EventArgs e)
        {
            uint addr = (uint)PALETTE_ADDRESS.Value;
            if (!U.isSafetyPointer(addr))
            {
                return;
            }

            PaletteFormRef.MakePaletteROMToUI(this, addr, true, this.PaletteIndexComboBox.SelectedIndex);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            uint class_id = InputFormRef.SelectToAddr(this.UNITCLASS_LIST);
            uint paletteno = (uint)AddressList.SelectedIndex + 1;
            int paletteIndex = this.PaletteIndexComboBox.SelectedIndex;
            DrawSample(class_id, paletteno, paletteIndex);
        }

        void DrawSample(uint battleAnimeID //戦闘アニメID
            ,uint paletteno //利用するユニットパレット (カスタムカラー)
            ,int paletteIndex //敵味方色
            )
        {
            Bitmap[] animeframe = new Bitmap[12];

            uint showsecstion = 0;
            uint showframe = 0;
            for (int index = 0; index < animeframe.Length; index++, showframe += 2)
            {
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID(battleAnimeID)
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , paletteno, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //何も描画されなければフレームをもうちょっと進めてみる.
                showframe += 2;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID(battleAnimeID)
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , paletteno, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //それでもだめならセクションを切り替える.
                showsecstion += 1;
                showframe = 0;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID(battleAnimeID)
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , paletteno, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //さらにダメならもう一つセクションを進める. それでもだめならあきらめる.
                showsecstion += 1;
                showframe = 0;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID(battleAnimeID)
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , paletteno, showsecstion, showframe, paletteIndex);
            }

            this.DrawBitmap = ImageUtil.Blank(360, 290, animeframe[0]);
            int x = 0;
            int y = 0;
            for (int index = 0; index < animeframe.Length; index++)
            {
                ImageUtil.BitBlt(this.DrawBitmap, x, y, animeframe[index].Width, animeframe[index].Height, animeframe[index], 0, 0);
                x += animeframe[index].Width;
                if (x >= this.DrawBitmap.Width)
                {
                    x = 0;
                    y += animeframe[index].Height;
                }
            }

            ReDrawBitmap();
        }

        private void CLASS_LIST_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint class_id = InputFormRef.SelectToAddr(this.UNITCLASS_LIST);
            uint paletteno = (uint)AddressList.SelectedIndex + 1;
            int paletteIndex = this.PaletteIndexComboBox.SelectedIndex;
            DrawSample(class_id, paletteno, paletteIndex);
        }
        Bitmap DrawBitmap;

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            int paletteIndex = this.PaletteIndexComboBox.SelectedIndex;
            uint newAddr = PaletteFormRef.MakePaletteUIToROM(this, (uint)PALETTE_ADDRESS.Value, true, paletteIndex);
            if (newAddr == U.NOT_FOUND)
            {
                return;
            }
            P12.Value = U.toPointer(newAddr);
            WriteButton.PerformClick();
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this,newAddr);
        }


        private bool OnChangeColor(Color color,int paletteno)
        {
            if (this.DrawBitmap == null)
            {
                return true;
            }
            ColorPalette palette = this.DrawBitmap.Palette; //一度、値をとってからいじらないと無視される
            palette.Entries[paletteno] = color;
            this.DrawBitmap.Palette = palette;
            ReDrawBitmap();

            return true;
        }
        void ReDrawBitmap()
        {
            PaletteFormRef.SetScaleSampleImage(this.X_PIC,this.AutoScrollPanel, this.DrawBitmap, this.PaletteZoomComboBox.SelectedIndex);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (this.DrawBitmap == null)
            {
                return;
            }
            ImageFormRef.ExportImage(this,this.DrawBitmap, InputFormRef.MakeSaveImageFilename(), 1);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            bool r = PaletteFormRef.MakePaletteBitmapToUIEx(this, 0, this.DrawBitmap);
            if (!r)
            {
                return;
            }

            //書き込み
            PaletteWriteButton.PerformClick();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            string name = "UnitPalette";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 12 });

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    name = "UnitPalette " + U.To0xHexString(i);

                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 12
                        , name
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77PAL);
                }
            }
        }

        private void PaletteIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PALETTE_POINTER_ValueChanged(null, null);
        }

        private void PALETTE_TO_CLIPBOARD_BUTTON_Click(object sender, EventArgs e)
        {
            bool r = PaletteFormRef.PALETTE_TO_CLIPBOARD_BUTTON_Click(this);
            if (r)
            {
                //書き込み
                PaletteWriteButton.PerformClick();
            }
        }

        private void PaletteZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReDrawBitmap();
        }

        //リストが拡張されたとき P12イベントポインタをNULLにする.
        void AddressListExpandsEventNoCopyP12(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;
            InputFormRef.WriteButtonToYellow(WriteButton, false);

            //増えた分のP12をゼロにする.
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ClearP12Pointer");
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);
            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                Program.ROM.write_u32(addr + 12, 0, undodata);

                addr += eearg.BlockSize;
            }
            Program.Undo.Push(undodata);
        }

        private void P12_ValueChanged(object sender, EventArgs e)
        {
            PALETTE_ADDRESS.Value = P12.Value;
        }

        public static string GetExplainPaletteRule()
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                return R._("FE8の色決定ルーチンは以下のようになります。\r\n{0}から、ユニットの現在所属しているクラスに適合するパレットがあるかどうか探索します。\r\nもし、パレットが見つかれば、{1}を利用します。\r\nパレットで見つからなければ、戦闘アニメーションの汎用色のパレットが利用されます。\r\n", R._("ユニット別パレット"), R._("ユニット別パレット"));
            }
            else
            {
                return R._("{0}の色決定ルーチンは以下のようになります。\r\nユニット設定で、{1}、または、{2}が指定されているかを確認します。\r\nもし、パレットが指定されていれば、{3}を利用します。\r\nパレットで見つからなければ、戦闘アニメーションの汎用色のパレットが利用されます。\r\n", Program.ROM.TitleToFilename(), R._("下位クラス戦闘アニメ色"), R._("上位クラス戦闘アニメ色"), R._("ユニット別パレット"));
            }
        }

    }
}
