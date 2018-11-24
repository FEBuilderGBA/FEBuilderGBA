using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class FontForm : Form
    {
        byte[] SelectFontBitmapByte = null;

        public FontForm()
        {
            InitializeComponent();
            this.ZoomComboBox.SelectedIndex = 2;
            this.FontType.SelectedIndex = 0;
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);

            UseFontNameTextEdit.Text = UseFontNameTextEdit.Font.FontFamily.ToString();
            AutoGenbutton.AccessibleDescription = R._("ROMに存在しいフォントをPCに存在するフォントから自動的に作成します。\r\nまとめて複数のフォントを一気に作りたい場合は、ROM翻訳ツールから作ることをお勧めします。");

            if (Program.ROM.RomInfo.is_multibyte())
            {
                OptionForm.textencoding_enum textencoding = OptionForm.textencoding();
                if (textencoding == OptionForm.textencoding_enum.ZH_TBL)
                {
                    Debug.Assert(false);
                }
                else if (textencoding == OptionForm.textencoding_enum.EN_TBL)
                {
                    this.FontSample.Text = "AB@cd"; ///No Translate
                    this.SearchChar.Text = "X";     ///No Translate
                }
                else
                {
                    this.FontSample.Text = "あい@うえお"; ///No Translate
                    this.SearchChar.Text = "剣";          ///No Translate
                }
            }
            else
            {
                this.FontSample.Text = "AB@cd"; ///No Translate
                this.SearchChar.Text = "X";     ///No Translate
            }
        }


        //フォントを探す.
        public static uint FindFontData(uint topaddress
            , uint moji, InputFormRef.PRIORITY_CODE priorityCode
            )
        {
            uint prevaddr;
            return FindFontData(topaddress, moji, out prevaddr, Program.ROM, priorityCode);
        }
        public static uint FindFontData(uint topaddress
            , uint moji, out uint prevaddr, InputFormRef.PRIORITY_CODE priorityCode
            )
        {
            return FindFontData(topaddress, moji, out prevaddr, Program.ROM, priorityCode);
        }

        static uint FindFontDataSJIS(uint topaddress
            , uint moji, out uint prevaddr , ROM rom
            )
        {
            uint moji1 = ((moji >> 8) & 0xff);
            uint moji2 = (moji & 0xff);
            uint list;
            if (moji1 == 0)
            {//拡張 半角アルファベットフォント
                moji1 = 0x40;
            }
            else if (moji1 < 0x1f)
            {
                prevaddr = U.NOT_FOUND;
                //moji1バイト目が 0x1F より小さい コントロールコードにはフォントは存在しません
                return U.NOT_FOUND;
            }

            //リストの元になるポインタへ移動.
            list = topaddress + (moji1 << 2) - 0x100;
            prevaddr = list;

            if (!U.isSafetyOffset(list))
            {
                return U.NOT_FOUND;
            }
            uint p = rom.p32(list);
            if (!U.isSafetyOffset(p,rom))
            {
                return U.NOT_FOUND;
            }

            //同一ハッシュキーがあるため、リストをたどりながら目的のフォントを探します.
            //日本語版
            //struct{
            //    void* next;
            //    byte  moji2
            //    byte  width
            //    byte  nazo1
            //    byte  nazo2
            //} //sizeof()==8
            while (p > 0)
            {
                uint next = rom.p32(p);
                uint check = rom.u8(p + 4);

                //日本語版は、 moji1でハッシュして、 以下listをmoji2で探索します.
                if (check == moji2)
                {
                    //探していた文字コードである.
                    return p;
                }

                //現在地の保存.
                prevaddr = p;

                if (next == 0)
                {//リスト終端.
                    break;
                }

                if (!U.isSafetyOffset(next, rom))
                {//リストが壊れている.
                    break;
                }

                //次のリストへ進む.
                p = next;

            }

            //探していたフォントは存在しない!
            return U.NOT_FOUND;
        }


        static uint FindFontDataUTF8(uint topaddress
            , uint moji, out uint prevaddr, ROM rom
            )
        {
            uint moji1 = (moji & 0xff);

            //リストの元になるポインタへ移動.
            uint list = topaddress + (moji1 << 2);
            prevaddr = list;

            if (!U.isSafetyOffset(list))
            {
                return U.NOT_FOUND;
            }
            uint p = rom.p32(list);
            if (!U.isSafetyOffset(p, rom))
            {
                return U.NOT_FOUND;
            }

            //同一ハッシュキーがあるため、リストをたどりながら目的のフォントを探します.
            //日本語版
            //struct{
            //    void* next;
            //    byte  moji2
            //    byte  width
            //    byte  moji3
            //    byte  moji4
            //} //sizeof()==8
            uint moji2 = ((moji >> 8) & 0xff);
            uint moji3 = ((moji >> 16) & 0xff);
            uint moji4 = ((moji >> 24) & 0xff);

            while (p > 0)
            {
                uint next = rom.p32(p);
                uint check2 = rom.u8(p + 4);
                uint check3 = rom.u8(p + 6);
                uint check4 = rom.u8(p + 7);

                //日本語版は、 moji1でハッシュして、 以下listをmoji2で探索します.
                if (check2 == moji2 
                    && check3 == moji3
                    && check4 == moji4
                    )
                {
                    //探していた文字コードである.
                    return p;
                }

                //現在地の保存.
                prevaddr = p;

                if (next == 0)
                {//リスト終端.
                    break;
                }

                if (!U.isSafetyOffset(next, rom))
                {//リストが壊れている.
                    break;
                }

                //次のリストへ進む.
                p = next;

            }

            //探していたフォントは存在しない!
            return U.NOT_FOUND;
        }

        static uint FindFontDataLat1(uint topaddress
            , uint moji, out uint prevaddr, ROM rom
            )
        {
            uint moji2 = (moji & 0xff);

            //リストの元になるポインタへ移動. -0x100 は英語版では不要.
            uint list;
            list = topaddress + (moji2 << 2);
            prevaddr = list;

            if (!U.isSafetyOffset(list))
            {
                return U.NOT_FOUND;
            }
            uint p = rom.p32(list);
            if (!U.isSafetyOffset(p, rom))
            {
                return U.NOT_FOUND;
            }

            //英語版は 探索不要です.
            return p;
        }


        public static uint FindFontData(uint topaddress
            , uint moji, out uint prevaddr
            , ROM rom, InputFormRef.PRIORITY_CODE priorityCode
            )
        {
            if (!U.isSafetyOffset(topaddress))
            {
                prevaddr = U.NOT_FOUND;
                return U.NOT_FOUND;
            }

            if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8)
            {
                return FindFontDataUTF8(topaddress, moji, out prevaddr, rom);
            }
            else if (rom.RomInfo.is_multibyte())
            {
                return FindFontDataSJIS(topaddress, moji, out prevaddr, rom);
            }
            else
            {
                if (moji > 0xff && priorityCode == InputFormRef.PRIORITY_CODE.SJIS)
                {
                    return FindFontDataSJIS(topaddress, moji, out prevaddr, rom);
                }
                return FindFontDataLat1(topaddress, moji, out prevaddr, rom);
            }
        }
        static byte[] NewFontDataSJIS(uint moji, uint width, byte[] SelectFontBitmapByte)
        {
            byte[] newFontData = new byte[8 + 64];
            //U.write_u32(newFontData, 0, 0);   //NULL リストの末尾に追加するので.
            uint moji2 = (moji & 0xff);

            U.write_u8(newFontData, 4, moji2); //moji2バイト目
            U.write_u8(newFontData, 5, width);
            U.write_u8(newFontData, 6, 0); //??
            U.write_u8(newFontData, 7, 0); //??
            U.write_range(newFontData, 8, SelectFontBitmapByte); //64バイト書き込み

            return newFontData;
        }
        static byte[] NewFontDataLAT1(uint moji, uint width, byte[] SelectFontBitmapByte)
        {
            byte[] newFontData = new byte[8 + 64];
            //U.write_u32(newFontData, 0, 0);   //NULL リストの末尾に追加するので.

            U.write_u8(newFontData, 4, 0); //moji2バイト目
            U.write_u8(newFontData, 5, width);
            U.write_u8(newFontData, 6, 0); //??
            U.write_u8(newFontData, 7, 0); //??
            U.write_range(newFontData, 8, SelectFontBitmapByte); //64バイト書き込み

            return newFontData;
        }
        static byte[] NewFontDataUTF8(uint moji, uint width, byte[] SelectFontBitmapByte)
        {
            byte[] newFontData = new byte[8 + 64];
            //U.write_u32(newFontData, 0, 0);   //NULL リストの末尾に追加するので.
            uint moji2 = ((moji >> 8) & 0xff);
            uint moji3 = ((moji >> 16) & 0xff);
            uint moji4 = ((moji >> 24) & 0xff);

            U.write_u8(newFontData, 4, moji2); //moji2バイト目
            U.write_u8(newFontData, 5, width);
            U.write_u8(newFontData, 6, moji3); //??
            U.write_u8(newFontData, 7, moji4); //??
            U.write_range(newFontData, 8, SelectFontBitmapByte); //64バイト書き込み

            return newFontData;
        }
        public static byte[] MakeNewFontData(uint moji, uint width, byte[] SelectFontBitmapByte,
            ROM rom,InputFormRef.PRIORITY_CODE priorityCode)
        {
            if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8)
            {
                return NewFontDataUTF8(moji, width, SelectFontBitmapByte);
            }
            else if (rom.RomInfo.is_multibyte())
            {
                return NewFontDataSJIS(moji, width, SelectFontBitmapByte);
            }
            else
            {
                if (moji > 0xff && priorityCode == InputFormRef.PRIORITY_CODE.SJIS)
                {
                    return NewFontDataSJIS(moji, width, SelectFontBitmapByte);
                }
                return NewFontDataLAT1(moji, width, SelectFontBitmapByte);
            }
        }
        
        //フォントデータを読む.
        public static byte[] ReadFontData(uint addr,out int width)
        {
            width = (int)Program.ROM.u8(addr+5);
            return Program.ROM.getBinaryData(addr + 8, 64);
        }
        public static uint ReadFontDataWidthOnly(uint addr)
        {
            return Program.ROM.u8(addr + 5);
        }

        public static Color ItemFontColor = Color.FromArgb(0x68, 0x88, 0xa8);
        public static Color SerifFontColor = Color.FromArgb(0xE0, 0xE0, 0xE0);

        //フォントの描画
        void DrawFonts()
        {
            if (SearchChar.Text.Length <= 0)
            {
                return;
            }

            Bitmap fontSampleBitmap;

            uint fontlist_pointer = GetFontPointer(this.FontType.SelectedIndex == 0);
            Color bgcolor = GetFontColor(this.FontType.SelectedIndex == 0);
            fontSampleBitmap = ImageUtil.Blank(FontPictureBox.Width, FontPictureBox.Height);

            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();

            int drawwidth = 0;
            for (int i = 0; i < this.FontSample.Text.Length; i++)
            {
                uint search_char;
                if (this.FontSample.Text[i] == '@')
                {
                    search_char = U.ConvertMojiCharToUnitFast(this.SearchChar.Text, priorityCode);
                }
                else
                {
                    search_char = U.ConvertMojiCharToUnitFast(this.FontSample.Text.Substring(i, 1), priorityCode);
                }
                uint fontaddr = FindFontData(fontlist_pointer, search_char, priorityCode);
                if (fontaddr == U.NOT_FOUND)
                {
                    continue;
                }
                int width;
                byte[] fontbyte;
                if (this.FontSample.Text[i] == '@')
                {
                    width = (int)this.FontWidth.Value;
                    fontbyte = this.SelectFontBitmapByte;
                }
                else
                {
                    fontbyte = ReadFontData(fontaddr, out width);
                }

                if (width <= 0 || fontbyte == null)
                {
                    continue;
                }
                Bitmap bitmap = ImageUtil.ByteToImage4(16, 16, fontbyte, 0, bgcolor);
                
                fontSampleBitmap.Palette = bitmap.Palette;
                ImageUtil.BitBlt(fontSampleBitmap, drawwidth, 0, width, 16, bitmap, 0, 0);
                drawwidth += width;
            }
            FontPictureBox.Image = U.Zoom(fontSampleBitmap, ZoomComboBox.SelectedIndex);
        }

        //検索して表示
        private void SearchButton_Click(object sender, EventArgs e)
        {
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();
            uint search_char = U.ConvertMojiCharToUnitFast(this.SearchChar.Text, priorityCode);

            uint fontlist_pointer = GetFontPointer(this.FontType.SelectedIndex == 0);

            uint fontaddr = FindFontData(fontlist_pointer, search_char,priorityCode);
            if (fontaddr == U.NOT_FOUND)
            {
                U.ForceUpdate(this.FontWidth, 0);
                this.SelectFontBitmapByte = null;
                this.Address.Value = 0;

                R.ShowWarning(R._("フォント:{0} が見つかりませんでした。"), this.SearchChar.Text);
            }
            else
            {
                int width;
                this.SelectFontBitmapByte = ReadFontData(fontaddr, out width);
                this.Address.Value = U.toPointer(fontaddr);

                if (width >= 30)
                {//たまにでかい値が来る時があるらしい.
                    width = 30;
                }
                U.ForceUpdate(this.FontWidth, width);
            }

            DrawFonts();
        }

        private void FontWidth_ValueChanged(object sender, EventArgs e)
        {
            DrawFonts();
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (this.SelectFontBitmapByte == null || this.SelectFontBitmapByte.Length != 64)
            {
                return;
            }


            string undo_name = "FONT " + this.SearchChar.Text;
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();
            uint search_char = U.ConvertMojiCharToUnitFast(this.SearchChar.Text, priorityCode);

            uint fontlist_pointer = GetFontPointer(this.FontType.SelectedIndex == 0);

            uint prevaddr;
            uint fontaddr = FindFontData(fontlist_pointer, search_char, out prevaddr, priorityCode);
            if (fontaddr == U.NOT_FOUND)
            {//末尾に追加.
                if (prevaddr == U.NOT_FOUND)
                {//このフォントはルールにより登録できない.日本語フォントで 0x100以下とか.
                    return;
                }
                byte[] newFontData = MakeNewFontData(search_char
                    , (uint)this.FontWidth.Value
                    , this.SelectFontBitmapByte
                    , Program.ROM
                    , priorityCode
                    );

                Undo.UndoData undodata = Program.Undo.NewUndoData(undo_name);

                uint newaddr = InputFormRef.AppendBinaryData(newFontData, undodata);
                if (newaddr == U.NOT_FOUND)
                {//エラー
                    return;
                }

                //ひとつ前のフォントリストのポインタを、現在追加した最後尾にすげかえる.
                Program.ROM.write_u32(prevaddr + 0, U.toPointer(newaddr), undodata);
                fontaddr = newaddr;
            }
            else
            {//更新
                Program.Undo.Push(undo_name, fontaddr, 8 + 64);
                Program.ROM.write_u8(fontaddr + 5, (uint)this.FontWidth.Value );
                Program.ROM.write_range(fontaddr + 8, this.SelectFontBitmapByte); //64バイト書き込み
            }
            this.Address.Value = U.toPointer(fontaddr);
            InputFormRef.ShowWriteNotifyAnimation(this, fontaddr);
        }

        private void FontForm_Load(object sender, EventArgs e)
        {
            this.SearchButton.PerformClick();
        }
        public static Bitmap DrawFont(string one, bool IsItemFont, out int out_width, InputFormRef.PRIORITY_CODE priorityCode)
        {
            uint search_char = U.ConvertMojiCharToUnitFast(one, priorityCode);
            return DrawFont(one, IsItemFont, out out_width, priorityCode);
        }
        public static Bitmap DrawFont(string one, bool IsItemFont, InputFormRef.PRIORITY_CODE priorityCode)
        {
            int width;
            uint search_char = U.ConvertMojiCharToUnitFast(one, priorityCode);
            return DrawFont(search_char, IsItemFont, out width, priorityCode);
        }
        public static Bitmap DrawFont(uint search_char, bool IsItemFont, InputFormRef.PRIORITY_CODE priorityCode)
        {
            int width;
            return DrawFont(search_char, IsItemFont, out width, priorityCode);
        }
        public static Bitmap DrawFont(uint search_char, bool IsItemFont, out int out_width, InputFormRef.PRIORITY_CODE priorityCode)
        {
            uint fontlist_pointer = GetFontPointer(IsItemFont);
            Color bgcolor = GetFontColor(IsItemFont);

            uint fontaddr = FindFontData(fontlist_pointer, search_char , priorityCode);
            if (fontaddr == U.NOT_FOUND)
            {
                out_width = 0;
                return null;
            }

            byte[] fontbyte = ReadFontData(fontaddr, out out_width);
            Bitmap bitmap = ImageUtil.ByteToImage4(16, 16, fontbyte, 0, bgcolor);
            return bitmap;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (this.SearchChar.Text.Length <= 0)
            {
                return;
            }
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();
            if (this.FontType.SelectedIndex == 0)
            {
                //アイテム
                Bitmap bitmap = FontForm.DrawFont(this.SearchChar.Text, true, priorityCode);
                ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "Item_" + this.SearchChar.Text), 8);
            }
            else
            {
                //セリフ
                Bitmap bitmap = FontForm.DrawFont(this.SearchChar.Text, false, priorityCode);
                ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "Serif_" + this.SearchChar.Text), 8);
            }

        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (this.SearchChar.Text.Length <= 0)
            {
                return;
            }
            Color bgcolor = GetFontColor(this.FontType.SelectedIndex == 0);
            Bitmap paletteHint = ImageUtil.ByteToImage4(16, 16, new byte[64] , 0, bgcolor);
            Bitmap bitmap = ImageUtil.LoadAndCheckPaletteUI(this
                , paletteHint
                , 2 * 8, 2 * 8);
            if (bitmap == null)
            {
                return;
            }

            //画像
            this.SelectFontBitmapByte = ImageUtil.Image4ToByte(bitmap);
            U.ForceUpdate(this.FontWidth, 9);

            //画像等データの書き込み
            WriteButton.PerformClick();
            //即検索
            SearchButton.PerformClick();
        }

        private void SearchChar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SearchButton.PerformClick();
            }
        }

        public static Color GetFontColor(bool IsItemFont)
        {
            if (IsItemFont)
            {
                //アイテム
                return ItemFontColor;
            }
            else
            {
                //セリフ
                return SerifFontColor;
            }
        }
        public static uint GetFontPointer(bool IsItemFont,ROM rom)
        {
            if (IsItemFont)
            {
                //アイテム
                return rom.RomInfo.font_item_address();
            }
            else
            {
                //セリフ
                return rom.RomInfo.font_serif_address();
            }
        }
        public static uint GetFontPointer(bool IsItemFont)
        {
            return GetFontPointer(IsItemFont, Program.ROM);
        }
        
        public static Bitmap DrawFontString(string str, bool IsItemFont)
        {
            //フォントで描画した場合の幅と高さを求める.
            Size size = MeasureTextMultiLine(str, IsItemFont);
            if(size.Width <= 0 || size.Height <= 0)
            {
                return ImageUtil.BlankDummy();
            }
            uint fontlist_pointer = GetFontPointer(IsItemFont);
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();

            Bitmap bitmap = ImageUtil.Blank(size.Width,size.Height);
            string[] lines = str.Split(new string[] { "\r\n" },StringSplitOptions.None);

            for (int n = 0; n < lines.Length; n++)
            {
                string line = lines[n];
                int totalwidth = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    string one = str[i].ToString();
                    uint search_char = U.ConvertMojiCharToUnitFast(one, priorityCode);

                    int width;
                    Bitmap oneBitmap = DrawFont(search_char, IsItemFont, out width, priorityCode);
                    if (oneBitmap == null)
                    {
                        continue;
                    }
                    if (n == 0)
                    {//最初ならパレット設定.
                        bitmap.Palette = oneBitmap.Palette;
                    }
                    ImageUtil.BitBlt(bitmap,totalwidth,n*16,width,16,oneBitmap,0,0,0,0);

                    totalwidth += width;
                }
            }
            return bitmap;
        }


        //フォントがない場合は推測幅
        public static uint GetUnkownFontSize(string lang)
        {
            return GetUnkownFontSize(lang == "ja" || lang == "zh");
        }
        public static uint GetUnkownFontSize(bool isMultiByte)
        {
            if (isMultiByte)
            {//マルチバイトは一律幅10ぐらい
                return 10;
            }
            else
            {//シングル場合は一律5ぐらいで計算しておこう.
                return 5;
            }
        }

        

        //フォントで描画した場合の幅を求める.
        public static uint[] MeasureTextWidthOneLineInts(string str, bool IsItemFont)
        {
            uint fontlist_pointer = GetFontPointer(IsItemFont);
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();

            uint[] widths = new uint[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                string one = str[i].ToString();
                uint search_char = U.ConvertMojiCharToUnitFast(one, priorityCode);

                uint fontaddr = FindFontData(fontlist_pointer, search_char, priorityCode);
                if (fontaddr == U.NOT_FOUND)
                {//フォントがない.
                    continue;
                }

                widths[i] = ReadFontDataWidthOnly(fontaddr);
            }
            return widths;
        }
        //フォントで描画した場合の幅を求める.
        public static uint MeasureTextWidthOneLine(string str, bool IsItemFont)
        {
            uint unkownFontSize = GetUnkownFontSize(Program.ROM.RomInfo.is_multibyte());
            uint sum = 0;
            uint[] widths = MeasureTextWidthOneLineInts(str, IsItemFont);
            for (int i = 0; i < widths.Length; i++)
            {
                if (widths[i] <= 0)
                {
                    sum += unkownFontSize;
                }
                else
                {
                    sum += widths[i];
                }
            }
            return sum;
        }

        //フォントで描画した場合の幅と高さを求める.
        public static Size MeasureTextMultiLine(string str, bool IsItemFont)
        {
            uint maxwidth = 0;
            str = TextForm.StripAllCode(str);
            string[] lines = str.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                uint width = MeasureTextWidthOneLine(line, IsItemFont);
                if (width > maxwidth)
                {
                    maxwidth = width;
                }
            }
            return new Size((int)maxwidth, 16 * lines.Length);
        }

        public void JumpToItem(uint value)
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {//英語版は2バイトではないので、最初の1バイトのみ評価.
                value = value & 0xff;
            }

            byte[] str = new byte[2];
            U.write_u16(str, 0, value);

            FETextDecode decoder = new FETextDecode();
            this.SearchChar.Text = decoder.listbyte_to_string(str, str.Length);
            this.FontType.SelectedIndex = 0;
            SearchButton_Click(null, null);
        }
        public void JumpToSerif(uint value)
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {//英語版は2バイトではないので、最初の1バイトのみ評価.
                value = value & 0xff;
            }

            byte[] str = new byte[2];
            U.write_u16(str, 0, value);

            FETextDecode decoder = new FETextDecode();
            this.SearchChar.Text = decoder.listbyte_to_string(str, str.Length);
            this.FontType.SelectedIndex = 1;
            SearchButton_Click(null, null);
        }

        public static void MakeAllDataLength(List<Address> list)
        {
            //アイテム
            uint fontlist_pointer = GetFontPointer(true);
            MakeAllDataLengthInner(fontlist_pointer, ref list);
            //セリフ
            fontlist_pointer = GetFontPointer(false);
            MakeAllDataLengthInner(fontlist_pointer, ref list);

            //ステータスフォント
            MakeAllDataLengthStatusFont( 
                Program.ROM.RomInfo.status_font_pointer()
                , Program.ROM.RomInfo.status_font_count()
                , ref list);
        }

        static void MakeAllDataLengthInner(uint topaddress, ref List<Address> list)
        {
            string name = "Font";

            if (Program.ROM.RomInfo.is_multibyte())
            {//日本語版
                FEBuilderGBA.Address.AddAddress(list, topaddress
                    , 4 * (0xff - 0x1f)
                    , U.NOT_FOUND
                    , name
                    , FEBuilderGBA.Address.DataTypeEnum.POINTER);

                for (uint moji1 = 0x1f; moji1 <= 0xff; moji1++)
                {
                    //リストの元になるポインタへ移動.
                    uint fontlist = topaddress + (moji1 << 2) - 0x100;
                    if (!U.isSafetyOffset(fontlist))
                    {
                        continue;
                    }

                    uint p = Program.ROM.p32(fontlist);
                    if (!U.isSafetyOffset(p))
                    {
                        continue;
                    }
                    uint before_pointer = fontlist;

                    //同一ハッシュキーがあるため、リストをたどりながら目的のフォントを探します.
                    //struct{
                    //    void* next;
                    //    byte  moji2
                    //    byte  width
                    //    byte  nazo1
                    //    byte  nazo2
                    //} //sizeof()==8
                    //+64byte bitmap(4pp)
                    while (p > 0)
                    {
                        FEBuilderGBA.Address.AddAddress(list
                            ,p, 8 + 64
                            ,before_pointer
                            ,name
                            ,FEBuilderGBA.Address.DataTypeEnum.FONT);


                        uint next = Program.ROM.p32(p);
                        if (next == 0)
                        {//リスト終端.
                            break;
                        }

                        if (!U.isSafetyOffset(next))
                        {//リストが壊れている.
                            break;
                        }

                        before_pointer = p;

                        //次のリストへ進む.
                        p = next;
                    }
                }
            }
            else
            {//英語版
                FEBuilderGBA.Address.AddAddress(list, topaddress
                    , 4 * (0xff)
                    , U.NOT_FOUND
                    , name
                    , FEBuilderGBA.Address.DataTypeEnum.POINTER);

                for (uint moji2 = 0x0; moji2 <= 0xff; moji2++)
                {
                    uint fontlist = topaddress + (moji2 << 2);
                    if (!U.isSafetyOffset(fontlist))
                    {
                        continue;
                    }
                    uint p = Program.ROM.p32(fontlist);
                    if (!U.isSafetyOffset(p))
                    {
                        continue;
                    }

                    uint before_pointer = fontlist;

                    //同一ハッシュキーがあるため、リストをたどりながら目的のフォントを探します.
                    //struct{
                    //    void* next;
                    //    byte  moji2
                    //    byte  width
                    //    byte  nazo1
                    //    byte  nazo2
                    //} //sizeof()==8
                    //+64byte bitmap(4pp)
                    while (p > 0)
                    {
                        FEBuilderGBA.Address.AddAddress(list,p
                            , 8 + 64
                            ,before_pointer
                            ,name
                            ,FEBuilderGBA.Address.DataTypeEnum.FONT);


                        uint next = Program.ROM.p32(p);
                        if (next == 0)
                        {//リスト終端.
                            break;
                        }

                        if (!U.isSafetyOffset(next))
                        {//リストが壊れている.
                            break;
                        }

                        before_pointer = p;

                        //次のリストへ進む.
                        p = next;
                    }
                }
            }
        }
        static void MakeAllDataLengthStatusFont(uint toppointer , uint count , ref List<Address> list)
        {
            if (!U.isSafetyOffset(toppointer))
            {
                return;
            }
            uint topaddress = Program.ROM.p32(toppointer);
            if (!U.isSafetyOffset(topaddress))
            {
                return;
            }

            string name = "StatusFont";
            uint addr = topaddress;
            for (int i = 0; i < count; i++ , addr += 4)
            {
                uint font = Program.ROM.p32(addr);
                if (!U.isSafetyOffset(font))
                {
                    continue;
                }

                FEBuilderGBA.Address.AddAddress(list
                    , font, 8 + 64
                    , addr
                    , name
                    , FEBuilderGBA.Address.DataTypeEnum.FONT);
            }
        }

        private void AutoGenbutton_Click(object sender, EventArgs e)
        {//フォントの自動生成.

            string mojiText = SearchChar.Text;
            mojiText = mojiText.Trim();
            if (mojiText.Length <= 0)
            {
                return;
            }
            bool isItemFont = this.FontType.SelectedIndex == 0;
            int font_width;
            Bitmap autogen = ImageUtil.AutoGenerateFont(mojiText, UseFontNameTextEdit.Font, isItemFont, out font_width);
            if (autogen == null)
            {
                R.ShowStopError("フォントの自動生成に失敗しました。対応する日本語フォントがありません。");
                return;
            }

            DialogResult dr = R.ShowQ("フォントを自動生成しました。\r\nROMに書き込みますか？\r\n\r\n「はい」ならば、ROMに書き込みます。\r\n「いいえ」ならば、画像ファイルとして保存します。\r\n");
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                //画像
                this.SelectFontBitmapByte = ImageUtil.Image4ToByte(autogen);
                U.ForceUpdate(this.FontWidth, font_width);

                //画像等データの書き込み
                WriteButton.PerformClick();
                //即検索
                SearchButton.PerformClick();
            }
            else if (dr == System.Windows.Forms.DialogResult.No)
            {
                if (this.FontType.SelectedIndex == 0)
                {
                    //アイテム
                    ImageFormRef.ExportImage(this, autogen, InputFormRef.MakeSaveImageFilename(this, "Item_" + mojiText), font_width);
                }
                else
                {
                    //セリフ
                    ImageFormRef.ExportImage(this, autogen, InputFormRef.MakeSaveImageFilename(this, "Serif_" + mojiText), font_width);
                }
            }
        }

        private void ZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawFonts();
        }

        private void FontSample_TextChanged(object sender, EventArgs e)
        {
            DrawFonts();
        }

        private void FontType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SearchChar.Text.Length <= 0)
            {
                return;
            }
            this.SearchButton.PerformClick();
        }

        private void UseFontNameButton_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.ShowColor = false;
            fd.ShowEffects = false;
            fd.Font = UseFontNameTextEdit.Font;
            DialogResult dr = fd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            UseFontNameTextEdit.Font = fd.Font;
            UseFontNameTextEdit.Text = fd.Font.FontFamily.ToString();
        }

        public static void TransportFontStruct(byte[] newFontData 
            ,uint moji
            ,InputFormRef.PRIORITY_CODE myselfPriorityCode 
            ,InputFormRef.PRIORITY_CODE yourPriorityCode 
            )
        {
            if (yourPriorityCode == myselfPriorityCode)
            {
                return;
            }
            if (myselfPriorityCode == InputFormRef.PRIORITY_CODE.SJIS)
            {//SJISの場合は、4バイト目に最初の1バイトを入れる必要がある.
                uint moji2 = (moji & 0xff);
                U.write_u8(newFontData, 4 , moji2);
                U.write_u8(newFontData, 6 , 0);
                U.write_u8(newFontData, 7 , 0);
            }
            else if (myselfPriorityCode == InputFormRef.PRIORITY_CODE.LAT1)
            {//LAT1の場合、SJISのヒントは利用しないので0にする
                U.write_u8(newFontData, 4, 0);
                U.write_u8(newFontData, 6, 0);
                U.write_u8(newFontData, 7, 0);
            }
            else if (myselfPriorityCode == InputFormRef.PRIORITY_CODE.UTF8)
            {//UTF8の場合、2-4バイト目を格納する
                uint moji2 = ((moji >> 8) & 0xff);
                uint moji3 = ((moji >> 16) & 0xff);
                uint moji4 = ((moji >> 24) & 0xff);

                U.write_u8(newFontData, 4, moji2);
                U.write_u8(newFontData, 6, moji3);
                U.write_u8(newFontData, 7, moji4);
            }
        }

    }
}
