using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class FontZHForm : Form
    {
        //中国語は独自のデータ構造をしているため別フォームに作る
        byte[] SelectFontBitmapByte = null;

        static bool isZH()
        {
            if (Program.ROM.RomInfo.is_multibyte())
            {
                OptionForm.textencoding_enum textencoding = OptionForm.textencoding();
                if (textencoding == OptionForm.textencoding_enum.ZH_TBL)
                {
                    return true;
                }
            }
            return false;
        }

        public FontZHForm()
        {
            InitializeComponent();
            this.ZoomComboBox.SelectedIndex = 2;
            this.FontType.SelectedIndex = 0;
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);

            U.SetIcon(ExportALLButton, Properties.Resources.icon_arrow);

            UseFontNameTextEdit.Text = UseFontNameTextEdit.Font.FontFamily.ToString();
            AutoGenbutton.AccessibleDescription = R._("ROMに存在しいフォントをPCに存在するフォントから自動的に作成します。\r\nまとめて複数のフォントを一気に作りたい場合は、ROM翻訳ツールから作ることをお勧めします。");

            Debug.Assert(isZH());

            this.FontSample.Text = "艾@珂"; ///No Translate
            this.SearchChar.Text = "瑞";          ///No Translate
        }

        //フォントを探す.
        static uint FindFontDataZH(uint topaddress
            , uint sjis,out uint prevaddr
            )
        {
            return FindFontDataZH(topaddress, sjis, Program.ROM,out prevaddr);
        }

        static uint FindFontDataZH(uint topaddress
            , uint sjis, ROM rom, out uint prevaddr
            )
        {
            prevaddr = U.NOT_FOUND;
            if (!isZH())
            {
                Debug.Assert(false);
                return U.NOT_FOUND;
            }
            if (!U.isSafetyOffset(topaddress))
            {
                return U.NOT_FOUND;
            }

            uint sjis1 = ((sjis >> 8) & 0xff);
            uint sjis2 = (sjis & 0xff);

            uint list;
            if (sjis1 == 0)
            {//拡張 半角アルファベットフォント
                sjis1 = 0x40;
            }
            else if (sjis1 < 0x1f)
            {
                //SJIS1バイト目が 0x1F より小さい コントロールコードにはフォントは存在しません
                return U.NOT_FOUND;
            }
            uint codeA = sjis2;
            uint codeB = sjis1;

            if(codeA<0x81&&codeA>0x98&&codeB<0x80)
            {//範囲外の場合
                //探していたフォントは存在しない!
                return U.NOT_FOUND;
            }
            codeA		-=	0x81;
            codeA		*=	0x80;
            codeB		-=	0x80;
            codeB		+=	codeA;		//偏移字数 オフセット語
            codeB		*=	0x54;		//字体大小 文字サイズ

            //リストの元になるポインタへ移動.
            list = topaddress + codeB;
            prevaddr = list;
            return list;
        }

        //フォントデータを読む.
        static byte[] ReadFontDataZH(uint addr,out int width)
        {
            width = (int)Program.ROM.u8(addr+2);
            byte[] d = Program.ROM.getBinaryData(addr + 4, 40);
            return d;
        }
        static uint ReadFontDataWidthOnlyZH(uint addr)
        {
            return Program.ROM.u8(addr + 2);
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

            bool IsItemFont = this.FontType.SelectedIndex == 0;

            uint fontlist_pointer = GetFontPointer(IsItemFont);
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();

            Color bgcolor = GetFontColor(IsItemFont);
            fontSampleBitmap = ImageUtil.Blank(FontPictureBox.Width, FontPictureBox.Height);

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

                uint prevaddr;
                uint fontaddr = FindFontDataZH(fontlist_pointer, search_char, out prevaddr);
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
                    fontbyte = ReadFontDataZH(fontaddr, out width);
                }

                if (width <= 0 || fontbyte == null)
                {
                    continue;
                }
                Bitmap bitmap;
                if (IsItemFont)
                {
                    bitmap = ImageUtil.ByteToImage4ZH(width+1, 0xD, fontbyte, 0, bgcolor);
                }
                else
                {
                    bitmap = ImageUtil.ByteToImage4ZH(width, 0xD, fontbyte, 0, bgcolor);
                }
                
                fontSampleBitmap.Palette = bitmap.Palette;
                ImageUtil.BitBlt(fontSampleBitmap, drawwidth, 0, width, 16, bitmap, 0, 0);
                drawwidth += width;
            }
            FontPictureBox.Image = U.Zoom(fontSampleBitmap, ZoomComboBox.SelectedIndex);
        }

        //検索して表示
        private void SearchButton_Click(object sender, EventArgs e)
        {
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();
            uint search_char = U.ConvertMojiCharToUnitFast(this.SearchChar.Text, priorityCode);

            uint fontlist_pointer = GetFontPointer(this.FontType.SelectedIndex == 0);

            uint prevaddr;
            uint fontaddr = FindFontDataZH(fontlist_pointer, search_char, out prevaddr);
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
                this.SelectFontBitmapByte = ReadFontDataZH(fontaddr, out width);

                if (width >= 30)
                {//たまにでかい値が来る時があるらしい.
                    width = 30;
                }
                U.ForceUpdate(this.FontWidth, width);
                this.Address.Value = U.toPointer(fontaddr);
            }

            DrawFonts();
        }

        private void FontWidth_ValueChanged(object sender, EventArgs e)
        {
            DrawFonts();
        }
        static byte[] MakeNewFontDataZH(uint moji, uint width, byte[] SelectFontBitmapByte)
        {
            byte[] newFontData = new byte[4 + 40];

            U.write_u8(newFontData, 0, 0xD); //moji2バイト目
            U.write_u8(newFontData, 1, width);
            U.write_u8(newFontData, 2, 0xD); //高さ?
            U.write_u8(newFontData, 3, 0); //??
            U.write_range(newFontData, 4, SelectFontBitmapByte); //40バイト書き込み

            return newFontData;
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (this.SelectFontBitmapByte == null || this.SelectFontBitmapByte.Length != 40)
            {
                return;
            }

            string undo_name = "FONT " + this.SearchChar.Text;
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();
            uint search_char = U.ConvertMojiCharToUnitFast(this.SearchChar.Text, priorityCode);

            uint fontlist_pointer = GetFontPointer(this.FontType.SelectedIndex == 0);

            uint width = (uint)this.FontWidth.Value;
            bool isItemFont = this.FontType.SelectedIndex == 0;
            if (isItemFont)
            {//なぜか中国語フォントでは、アイテムフォントは幅+1しないといけない
                width = width - 1;
            }

            uint prevaddr;
            uint fontaddr = FindFontDataZH(fontlist_pointer, search_char, out prevaddr);
            if (fontaddr == U.NOT_FOUND)
            {//末尾に追加.
                if (prevaddr == U.NOT_FOUND)
                {//このフォントはルールにより登録できない..
                    return;
                }

                byte[] newFontData = MakeNewFontDataZH(search_char
                    , width
                    , this.SelectFontBitmapByte
                    );

                Undo.UndoData undodata = Program.Undo.NewUndoData(undo_name);

                uint newaddr = InputFormRef.AppendBinaryData(newFontData, undodata);
                if (newaddr == U.NOT_FOUND)
                {//エラー
                    return;
                }

                //ひとつ前のフォントリストのポインタを、現在追加した最後尾にすげかえる.
                Program.ROM.write_u32(prevaddr, U.toPointer(newaddr), undodata);
                fontaddr = newaddr;
            }
            else
            {//更新
                Program.Undo.Push(undo_name, fontaddr, 4 + 40);
                Program.ROM.write_u8(fontaddr + 1, width);
                Program.ROM.write_range(fontaddr + 8, this.SelectFontBitmapByte); //40バイト書き込み
            }
            this.Address.Value = U.toPointer(fontaddr);
            InputFormRef.ShowWriteNotifyAnimation(this, fontaddr);
        }

        private void FontForm_Load(object sender, EventArgs e)
        {
            this.SearchButton.PerformClick();
        }
        public static Bitmap DrawFont(string one, bool IsItemFont, out int out_width)
        {
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();
            uint search_char = U.ConvertMojiCharToUnitFast(one, priorityCode);
            return DrawFont(search_char, IsItemFont, out out_width);
        }
        public static Bitmap DrawFont(uint search_char, bool IsItemFont, out int out_width)
        {
            uint fontlist_pointer = GetFontPointer(IsItemFont);
            Color bgcolor = GetFontColor(IsItemFont);

            uint prevaddr;
            uint fontaddr = FindFontDataZH(fontlist_pointer, search_char, out prevaddr);
            if (fontaddr == U.NOT_FOUND)
            {
                out_width = 0;
                return null;
            }

            byte[] fontbyte = ReadFontDataZH(fontaddr, out out_width);

            Bitmap bitmap;
            if (IsItemFont)
            {
                bitmap = ImageUtil.ByteToImage4ZH(out_width + 1, 0xD, fontbyte, 0, bgcolor);
            }
            else
            {
                bitmap = ImageUtil.ByteToImage4ZH(out_width, 0xD, fontbyte, 0, bgcolor);
            }
           
            return bitmap;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (this.SearchChar.Text.Length <= 0)
            {
                return;
            }
            int width;
            if (this.FontType.SelectedIndex == 0)
            {
                //アイテム
                Bitmap bitmap = FontZHForm.DrawFont(this.SearchChar.Text, true, out width);
                ImageFormRef.ExportImage(this, bitmap, InputFormRef.MakeSaveImageFilename(this, "Item_" + this.SearchChar.Text), 8);
            }
            else
            {
                //セリフ
                Bitmap bitmap = FontZHForm.DrawFont(this.SearchChar.Text, false, out width);
                ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename(this, "Serif_" + this.SearchChar.Text), 8);
            }

        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (this.SearchChar.Text.Length <= 0)
            {
                return;
            }

            bool IsItemFont = (this.FontType.SelectedIndex == 0);
            int w = (int)FontWidth.Value;

            Color bgcolor = GetFontColor(IsItemFont);
            Bitmap paletteHint;
            if (IsItemFont)
            {
                paletteHint = ImageUtil.ByteToImage4ZH(w + 1, 0xD, new byte[64], 0, bgcolor);
            }
            else
            {
                paletteHint = ImageUtil.ByteToImage4ZH(w, 0xD, new byte[64], 0, bgcolor);
            }

            Bitmap bitmap = ImageUtil.LoadAndCheckPaletteUI(this
                , paletteHint
                , 0, 0);
            if (bitmap == null)
            {
                return;
            }

            uint width = (uint)bitmap.Width;

            //画像
            if (IsItemFont)
            {
                this.SelectFontBitmapByte = ImageUtil.Image4ToByteZH(bitmap, (int)width + 1);
            }
            else
            {
                this.SelectFontBitmapByte = ImageUtil.Image4ToByteZH(bitmap, (int)width);
            }
            U.ForceUpdate(this.FontWidth, width);

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

        static Color GetFontColor(bool IsItemFont)
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
            Debug.Assert(isZH());
            if (rom.RomInfo.version() == 8)
            {
                if (IsItemFont)
                {
                    //セリフ
                    return 0x578020;
                }
                else
                {
                    //アイテム
                    return 0x577ff4;
                }
            }
            return 0;
        }
        public static uint GetFontPointer(bool IsItemFont)
        {
            return GetFontPointer(IsItemFont, Program.ROM);
        }
        
        //フォントで描画した場合の幅を求める.
        public static int[] MeasureTextWidthOneLineInts(string str, bool IsItemFont)
        {
            uint fontlist_pointer = GetFontPointer(IsItemFont);
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();

            int[] widths = new int[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                string one = str.Substring(i, 1);
                uint search_char = U.ConvertMojiCharToUnitFast(one, priorityCode);

                uint prevaddr;
                uint fontaddr = FindFontDataZH(fontlist_pointer, search_char, out prevaddr);
                if (fontaddr == U.NOT_FOUND)
                {//フォントがない.
                    continue;
                }

                ReadFontDataZH(fontaddr, out widths[i]);
            }
            return widths;
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
            Dictionary<uint, string> codeBMap = MakeCodeBMap(isKanjiOnly: false);

            //アイテム
            MakeAllDataLengthInner(true, ref list, codeBMap);
            //セリフ
            MakeAllDataLengthInner(false, ref list, codeBMap);
        }

        static Dictionary<uint, string> MakeCodeBMap(bool isKanjiOnly)
        {
            Dictionary<string, uint> encodeMap = Program.SystemTextEncoder.GetTBLEncodeDicLow();
            Dictionary<uint, string> codeBMap = new Dictionary<uint,string>();
            foreach(var p in encodeMap)
            {
                if (isKanjiOnly)
                {
                    uint bigendian = ((p.Value & 0xff) << 8) | ((p.Value >> 8) & 0xff);
                    if (bigendian <= 0x8397)
                    {
                        continue;
                    }
                }
                
                uint codeA = p.Value & 0xff;
                uint codeB = (p.Value>>8) & 0xff;
                uint codeBB = CalcCodeB(codeA,codeB);
                codeBMap[codeBB] = p.Key;
            }
            return codeBMap;
        }


        static void MakeAllDataLengthInner(bool isItemFont, ref List<Address> list, Dictionary<uint, string> codeBMap)
        {
            string name = isItemFont ? "FontItem" : "FontText";
            uint topaddress = GetFontPointer(isItemFont);

            //中国語は、直参照形式.
            //struct{
            //	byte	unk1;		0D nazo
            //	byte	width;		0C <- たぶん幅 しかし+1 しないといけない
            //	byte	unk2;		0D <- 高さ?
            //	byte	unk3;		00
            //} //sizeof()==8
            //+40byte bitmap(4pp)
            const uint fontSize = (4 + 40);

            foreach (var p in codeBMap)
            {
                uint addr = topaddress + p.Key;
                FEBuilderGBA.Address.AddAddress(list, addr
                    , fontSize
                    , U.NOT_FOUND
                    , name + p.Value
                    , FEBuilderGBA.Address.DataTypeEnum.FONTCN);
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

        private void AutoGenbutton_Click(object sender, EventArgs e)
        {//フォントの自動生成.
            string mojiText = SearchChar.Text;
            mojiText = mojiText.Trim();
            if (mojiText.Length <= 0)
            {
                return;
            }

            bool isItemFont = this.FontType.SelectedIndex == 0;
            bool isSquareFont = true;
            int font_width;
            Bitmap autogen = ImageUtil.AutoGenerateFont(mojiText, UseFontNameTextEdit.Font, isItemFont, isSquareFont, out font_width);
            if (autogen == null)
            {
                R.ShowStopError("フォントの自動生成に失敗しました。対応する日本語フォントがありません。");
                return;
            }

            DialogResult dr = R.ShowQ("フォントを自動生成しました。\r\nROMに書き込みますか？\r\n\r\n「はい」ならば、ROMに書き込みます。\r\n「いいえ」ならば、画像ファイルとして保存します。\r\n");
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                //画像
                this.SelectFontBitmapByte = ImageUtil.Image4ToByteZH(autogen, font_width);
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

        private void ExportALLButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            bool isUserFontOnly = false;

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TXT|*.fontall.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, "font");

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
            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                string basedir = Path.GetDirectoryName(filename);
                Dictionary<uint, string> codeBMap = MakeCodeBMap(isKanjiOnly: true);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("//char\ttype\tWidth\tFilename");
                sb.Append(ExportALL(pleaseWait, basedir, true , isUserFontOnly, codeBMap));
                sb.Append(ExportALL(pleaseWait, basedir, false, isUserFontOnly, codeBMap));

                File.WriteAllText(filename, sb.ToString());
            }
            Program.LastSelectedFilename.Save(this, "", save);

        }
        StringBuilder ExportALL(InputFormRef.AutoPleaseWait pleaseWait, string basedir, bool isItemFont, bool isUserFontOnly, Dictionary<uint, string> codeBMap)
        {
            List<Address> list = new List<Address>();
            MakeAllDataLengthInner(isItemFont, ref list, codeBMap);
            PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode();
            Color bgcolor = GetFontColor(isItemFont);
            StringBuilder sb = new StringBuilder();

            foreach (Address a in list)
            {
                if (a.DataType != FEBuilderGBA.Address.DataTypeEnum.FONTCN)
                {
                    continue;
                }

                if (isUserFontOnly
                    && a.Addr <= Program.ROM.RomInfo.font_default_end())
                {//規定のフォント
                    continue;
                }

                int width;
                byte[] fontbyte = ReadFontDataZH(a.Addr, out width);
                if (width == 0)
                {
                    continue;
                }
                string ch = GetFontCharFromExportName(a.Info);
                if (ch == "")
                {
                    continue;
                }

                string type = (isItemFont ? "item" : "text");

                Bitmap bitmap;
                if (isItemFont)
                {
                    bitmap = ImageUtil.ByteToImage4ZH(width + 1, 0xD, fontbyte, 0, bgcolor);
                    bitmap = ConvertVanillaFontSizeBitmap(bitmap, 2);
                }
                else
                {
                    bitmap = ImageUtil.ByteToImage4ZH(width, 0xD, fontbyte, 0, bgcolor);
                    bitmap = ConvertVanillaFontSizeBitmap(bitmap, 1);
                }

                ImageUtil.BlackOutUnnecessaryColors(bitmap, 1);
                string name = U.escape_filename(a.Info);
                string font_filename = Path.Combine(basedir, name + ".png");
                U.BitmapSave(bitmap, font_filename);
                bitmap.Dispose();

                sb.Append(ch);
                sb.Append("\t");
                sb.Append(type);
                sb.Append("\t");
                sb.Append(width);
                sb.Append("\t");
                sb.AppendLine(a.Info + ".png");
            }

            return sb;
        }
        static string GetFontCharFromExportName(string name)
        {
            string a = name.Substring(8);
            return a;
        }

        static Bitmap ConvertVanillaFontSizeBitmap(Bitmap zhFont, int shiftHeight)
        {
            Bitmap bitmap = ImageUtil.Blank(16, 16, zhFont);
            ImageUtil.BitBlt(bitmap, 0, shiftHeight, 16, 16, zhFont, 0, 0);

            return bitmap;
        }
        static uint CalcCodeB(uint codeA, uint codeB)
        {
            codeA -= 0x81;
            codeA *= 0x80;
            codeB -= 0x80;
            codeB += codeA;		//偏移字数 オフセット語
            codeB *= 0x54;		//字体大小 文字サイズ
            return codeB;
        }

    }
}
