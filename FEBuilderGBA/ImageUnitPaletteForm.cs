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
            SetExplain();
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
            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEventNoCopyP12;
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.PaletteZoomComboBox.SelectedIndex = 0;
            this.PaletteIndexComboBox.SelectedIndex = 0;
            this.PFR = new PaletteFormRef(this);
            PFR.MakePaletteUI(OnChangeColor,GetSampleBitmap);

            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
        }
        PaletteFormRef PFR;

        void SetExplain()
        {
            this.PaletteOverradeALL.AccessibleDescription = R._("チェックされている場合、敵やNPCの時のパレットにも同じパレットに設定します。\r\nこのパレットを利用するユニットが、自軍以外で登場する場合は、それぞれのパレットも用意する必要があります。\r\n例えば、敵軍として登場する場合は、敵軍用のパレットを用意する必要があります。\r\nこのチェックがされている場合、自動的にそれらのパレットにも同じ値を設定します。");
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
                        if (name == 0)
                        {//名前もnullなのでデータではないと思われる.
                            return false;
                        }
                    }
                    //有効値
                    return true;
                }
                , (int i, uint addr) =>
                {
                    String name = Program.ROM.getString(addr,12);
                    name = name.TrimEnd();

                    return U.ToHexString(i + 1) + U.SA(name) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageUnitPaletteForm_Load(object sender, EventArgs e)
        {
            List<Control>  controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeJumpEventHandler(this.X_BATTLEANIME, this.X_BATTLEANIME_LABEL, "BATTLEANIME", new string[] { "MINUS1" });
            InputFormRef.makeLinkEventHandler("X_", controls, this.X_BATTLEANIME, this.X_BATTLEANIME_INFO, 0, "BATTLEANIME", new string[] { });

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });
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
            return Program.ROM.getString(addr,12).TrimEnd() + InputFormRef.GetCommentSA(addr);
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

            PFR.MakePaletteROMToUI(addr, true, this.PaletteIndexComboBox.SelectedIndex);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            uint class_id = InputFormRef.SelectToAddr(this.UNITCLASS_LIST);
            uint battleAnimeID = ImageBattleAnimeForm.GetAnimeIDByClassID(class_id);
            U.ForceUpdate(this.X_BATTLEANIME, battleAnimeID);
        }
        private void X_DISPLAY_CLASS_ValueChanged(object sender, EventArgs e)
        {
            uint battleAnimeID = (uint)this.X_BATTLEANIME.Value;
            uint paletteno = (uint)AddressList.SelectedIndex + 1;
            int paletteIndex = this.PaletteIndexComboBox.SelectedIndex;
            DrawSample(battleAnimeID, paletteno, paletteIndex);
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
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , paletteno, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //何も描画されなければフレームをもうちょっと進めてみる.
                showframe += 2;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , paletteno, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //それでもだめならセクションを切り替える.
                showsecstion += 1;
                showframe = 0;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
                    , ImageBattleAnimeForm.ScaleTrim.SCALE_90
                    , paletteno, showsecstion, showframe, paletteIndex);
                if (!ImageUtil.IsBlankBitmap(animeframe[index], 10))
                {
                    continue;
                }
                //さらにダメならもう一つセクションを進める. それでもだめならあきらめる.
                showsecstion += 1;
                showframe = 0;
                animeframe[index] = ImageBattleAnimeForm.DrawBattleAnime(battleAnimeID
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
            uint battleAnimeID = ImageBattleAnimeForm.GetAnimeIDByClassID(class_id);
            U.ForceUpdate(this.X_BATTLEANIME, battleAnimeID);
        }
        Bitmap DrawBitmap;

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            if (PALETTE_ADDRESS.Value == 0)
            {
                R.ShowStopError("パレット領域が割り当てられていません。\r\nまずは、「新規パレット割り当て」ボタンを押して領域を確保してください。");
                return;
            }

            int paletteIndex = this.PaletteIndexComboBox.SelectedIndex;
            if (PaletteOverradeALL.Visible && PaletteOverradeALL.Checked)
            {//全部同じパレットにする
                paletteIndex = PaletteFormRef.OVERRAIDE_ALL_PALETTE;
            }
            uint newAddr = PFR.MakePaletteUIToROM((uint)PALETTE_ADDRESS.Value, true, paletteIndex);
            if (newAddr == U.NOT_FOUND)
            {
                return;
            }
            P12.Value = U.toPointer(newAddr);
            WriteButton.PerformClick();
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this,newAddr);
        }

        Bitmap GetSampleBitmap()
        {
            return this.DrawBitmap;
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
            bool r = PFR.MakePaletteBitmapToUIEx(0, this.DrawBitmap);
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
            PaletteOverradeALL.Visible = (PaletteIndexComboBox.SelectedIndex == 0);
        }

        private void PALETTE_TO_CLIPBOARD_BUTTON_Click(object sender, EventArgs e)
        {
            bool r = PFR.PALETTE_TO_CLIPBOARD_BUTTON_Click();
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
                return R._("{0}の色決定ルーチンは以下のようになります。\r\nユニット設定で、{1}、または、{2}が指定されているかを確認します。\r\nもし、パレットが指定されていれば、{3}を利用します。\r\nパレットで見つからなければ、戦闘アニメーションの汎用色のパレットが利用されます。\r\n", Program.ROM.RomInfo.TitleToFilename(), R._("下位クラス戦闘アニメ色"), R._("上位クラス戦闘アニメ色"), R._("ユニット別パレット"));
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);

            uint table_addr = InputFormRef.BaseAddress;
            uint limit_count = Math.Min(InputFormRef.DataCount, Program.ROM.RomInfo.magic_effect_original_data_count());
            for (int i = 0; i < InputFormRef.DataCount; i++, table_addr += InputFormRef.BlockSize)
            {
                uint id = (uint)i;
                uint p = Program.ROM.p32(table_addr + 0xC);
                if (p == 0)
                {
                    continue;
                }

                FELint.CheckLZ77(p, errors, FELint.Type.IMAGE_UNIT_PALETTE, table_addr, id);
            }
        }

        private void X_PIC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                return;
            }
            PFR.SpoitTool_SelectPalette(this.X_PIC, this.PaletteZoomComboBox.SelectedIndex, e);
        }

        private void UNDOButton_Click(object sender, EventArgs e)
        {
            PFR.RunUndo();
        }

        private void REDOButton_Click(object sender, EventArgs e)
        {
            PFR.RunRedo();
        }


    }
}
