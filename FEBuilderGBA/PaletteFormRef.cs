using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.ComponentModel;

namespace FEBuilderGBA
{
    public class PaletteFormRef
    {
        private PaletteFormRef()
        {
        }
        public PaletteFormRef(Form self)
        {
            this.SelfForm = self;
            this.Controls = InputFormRef.GetAllControls(self);
            ClearUndoBuffer();
        }
        Form SelfForm;
        List<Control> Controls;

        public void SpoitTool_SelectPalette(PictureBox pic, int options, MouseEventArgs e)
        {
            if (pic.Image == null)
            {
                return;
            }

            int x = e.X;
            int y = e.Y;

            Color rgb;

            try
            {
                rgb = ((Bitmap)(pic.Image)).GetPixel(x, y);
            }
            catch (Exception)
            {
                return;
            }
            uint or = (uint)(rgb.R & 0xFD);
            uint og = (uint)(rgb.G & 0xFD);
            uint ob = (uint)(rgb.B & 0xFD);

            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);

                uint dr = (uint)((uint)r.Value & 0xFD);
                uint dg = (uint)((uint)g.Value & 0xFD);
                uint db = (uint)((uint)b.Value & 0xFD);

                if (dr == or && og == dg && ob == db)
                {
                    r.Focus();
                    return;
                }
            }
        }


        public static void SetScaleSampleImage(PictureBox pic,Panel parentPanel, Bitmap bitmap,int options)
        {
            if (options == 0)
            {
                parentPanel.AutoScrollPosition = new Point(0, 0);
                pic.Image = null;
                pic.Size = new Size(parentPanel.Size.Width - parentPanel.Margin.Right
                    , parentPanel.Size.Height - parentPanel.Margin.Bottom);
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Image = bitmap;
            }
            else
            {
                pic.Image = null;
                pic.SizeMode = PictureBoxSizeMode.AutoSize;
                pic.Image = U.Zoom(bitmap,options);
            }
        }


        NumericUpDown FindNUD(string symbol, int paletteno)
        {
            foreach (Control info in Controls)
            {
                string[] sp = info.Name.Split('_');
                if (sp.Length < 3)
                {
                    continue;
                }
                if (sp[0] != "PALETTE")
                {
                    continue;
                }
                if (paletteno != U.atoi(sp[2]))
                {
                    continue;
                }

                if (sp[1] != symbol)
                {
                    continue;
                }
                if (!(info is NumericUpDown))
                {
                    continue;
                }
                return (NumericUpDown)info;
            }
            return null;
        }
        Label FindLabel(string symbol, int paletteno)
        {
            foreach (Control info in Controls)
            {
                string[] sp = info.Name.Split('_');
                if (sp.Length < 3)
                {
                    continue;
                }
                if (sp[0] != "PALETTE")
                {
                    continue;
                }
                if (paletteno != U.atoi(sp[2]))
                {
                    continue;
                }

                if (sp[1] != symbol)
                {
                    continue;
                }
                if (!(info is Label))
                {
                    continue;
                }
                return (Label)info;
            }
            return null;
        }

        MouseEventHandler Label_MouseClickEvent(Label obj, int paletteno)
        {
            return (sender, e) =>
            {
                if (e.Button != MouseButtons.Left)
                {
                    return;
                }

                //色変更ダイアログ
                ColorDialog cd = new ColorDialog();
                cd.Color = obj.BackColor;
                if (cd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                PushUndo();
                this.UndoLock = true;

                //選択された色の取得
                NumericUpDown r = FindNUD("R", paletteno);
                r.Value = (cd.Color.R >> 3) << 3;
                NumericUpDown g = FindNUD("G", paletteno);
                g.Value = (cd.Color.G >> 3) << 3;
                NumericUpDown b = FindNUD("B", paletteno);
                b.Value = (cd.Color.B >> 3) << 3;

                this.UndoLock = false;
            };
        }
        EventHandler Label_ColorSwap(Label obj, int paletteno)
        {
            return (sender, e) =>
            {
                PaletteSwapForm form = (PaletteSwapForm)InputFormRef.JumpFormLow<PaletteSwapForm>();
                form.SetMainColorIndex(paletteno);
                Color[] colormap = new Color[16 + 1];
                for (int i = 1; i <= 16; i++)
                {
                    NumericUpDown r = FindNUD("R", i);
                    NumericUpDown g = FindNUD("G", i);
                    NumericUpDown b = FindNUD("B", i);
                    form.SetColor(i, (int)r.Value, (int)g.Value, (int)b.Value);
                    colormap[i] = Color.FromArgb((int)r.Value, (int)g.Value, (int)b.Value);
                }
                DialogResult dr = form.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                int selected = form.GetSelectedColorIndex();

                PushUndo();
                this.UndoLock = true;
                for (int i = 1; i <= 16; i++)
                {
                    NumericUpDown r = FindNUD("R", i);
                    NumericUpDown g = FindNUD("G", i);
                    NumericUpDown b = FindNUD("B", i);

                    if (selected == i)
                    {
                        r.Value = colormap[paletteno].R;
                        g.Value = colormap[paletteno].G;
                        b.Value = colormap[paletteno].B;
                    }
                    else if (paletteno == i)
                    {
                        r.Value = colormap[selected].R;
                        g.Value = colormap[selected].G;
                        b.Value = colormap[selected].B;
                    }
                }
                this.UndoLock = false;
            }
            ;
        }

        EventHandler Label_ColorChanges(Label obj, int paletteno, Func<Bitmap> getSampleBitmap)
        {
            return (sender, e) =>
            {
                Bitmap sample = getSampleBitmap();
                if (sample == null)
                {
                    return;
                }

                PaletteChangeColorsForm form = (PaletteChangeColorsForm)InputFormRef.JumpFormLow<PaletteChangeColorsForm>();
                form.SetMainColorIndex(paletteno);
                form.SetPreviewBitmap(sample);
                Color[] colormap = new Color[16 + 1];
                for (int i = 1; i <= 16; i++)
                {
                    NumericUpDown r = FindNUD("R", i);
                    NumericUpDown g = FindNUD("G", i);
                    NumericUpDown b = FindNUD("B", i);
                    form.SetColor(i, (int)r.Value, (int)g.Value, (int)b.Value);
                    colormap[i] = Color.FromArgb((int)r.Value, (int)g.Value, (int)b.Value);
                }
                DialogResult dr = form.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                PushUndo();
                this.UndoLock = true;

                for (int i = 1; i <= 16; i++)
                {
                    NumericUpDown r = FindNUD("R", i);
                    NumericUpDown g = FindNUD("G", i);
                    NumericUpDown b = FindNUD("B", i);

                    Color rgb = form.GetColor(i);
                    r.Value = rgb.R;
                    g.Value = rgb.G;
                    b.Value = rgb.B;
                }

                this.UndoLock = false;
            }
            ;
        }
        EventHandler NumericUpDown_ChangeEvent(NumericUpDown obj, string symbol, int paletteno, Func<Color, int, bool> onChangeColor)
        {
            return (sender, e) =>
            {
                PushUndo();
                Label p = (Label)FindLabel("P", paletteno);
                int v = (int)obj.Value;
                obj.Tag = (uint)v;
                if (symbol == "R")
                {
                    obj.BackColor = Color.FromArgb(v, 0, 0);
                    obj.ForeColor = Color.FromArgb((v > 128 ? 0 : 255), 255, 255);
                    Color color = Color.FromArgb(v, p.BackColor.G, p.BackColor.B);
                    p.BackColor = color;
                    onChangeColor(color, paletteno - 1);
                }
                else if (symbol == "G")
                {
                    obj.BackColor = Color.FromArgb(0, v, 0);
                    obj.ForeColor = Color.FromArgb(0, (v > 128 ? 0 : 255), 255);
                    Color color = Color.FromArgb(p.BackColor.R, v, p.BackColor.B);
                    p.BackColor = color;
                    onChangeColor(color, paletteno - 1);
                }
                else if (symbol == "B")
                {
                    obj.BackColor = Color.FromArgb(0, 0, v);
                    obj.ForeColor = Color.FromArgb(255, 255, (v > 128 ? 0 : 255));
                    Color color = Color.FromArgb(p.BackColor.R, p.BackColor.G, v);
                    p.BackColor = color;
                    onChangeColor(color, paletteno - 1);
                }
            }
            ;
        }


        public void MakePaletteUI(Func<Color, int, bool> onChangeColor, Func<Bitmap> getSampleBitmap)
        {
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                Label p = (Label)FindLabel("P", paletteno);
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);

                p.MouseClick += Label_MouseClickEvent(p, paletteno);
                p.Cursor = Cursors.Hand;

                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem;

                menuItem = new MenuItem(R._("色の交換"));
                menuItem.Click += Label_ColorSwap(p, paletteno);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem(R._("色違いを作る"));
                menuItem.Click += Label_ColorChanges(p, paletteno, getSampleBitmap);
                contextMenu.MenuItems.Add(menuItem);

                p.ContextMenu = contextMenu;

                EventHandler r_eh = NumericUpDown_ChangeEvent(r, "R", paletteno, onChangeColor);
                r.ValueChanged += r_eh;
                r.Increment = 1 << 3;

                EventHandler g_eh = NumericUpDown_ChangeEvent(g, "G", paletteno, onChangeColor);
                g.ValueChanged += g_eh;
                g.Increment = 1 << 3;

                EventHandler b_eh = NumericUpDown_ChangeEvent(b, "B", paletteno, onChangeColor);
                b.ValueChanged += b_eh;
                b.Increment = 1 << 3;
            }
        }
        //現在選択されている色を全適応
        void MakePaletteUIApplyEvent()
        {
            PushUndo();
            this.UndoLock = true;
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);

                U.ForceUpdate(r, r.Value);
                U.ForceUpdate(g, g.Value);
                U.ForceUpdate(b, b.Value);
            }
            this.UndoLock = false;
        }
        public bool MakePaletteBitmapToUIEx(int palette_index, Bitmap hintBitmap)
        {
            Debug.Assert(ImageUtil.Is16ColorBitmap(hintBitmap));

            string imagefilename = ImageFormRef.OpenFilenameDialogFullColor(this.SelfForm);
            if (imagefilename == "")
            {
                return false;
            }

            Bitmap fullColor = ImageUtil.OpenLowBitmap(imagefilename); //bitmapそのものの色で開く.
            if (fullColor == null)
            {
                return false;
            }

            Bitmap bitmap;
            if (palette_index <= 0)
            {
                if (!ImageUtil.Is16ColorBitmap(fullColor))
                {
                    R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", palette_index, "fullcolor");
                    fullColor.Dispose();
                    return false;
                }
                bitmap = ImageUtil.OpenBitmap(imagefilename); //16色に変換して開く.
                if (bitmap == null)
                {
                    fullColor.Dispose();
                    return false;
                }
                if (ImageUtil.IsFullColorBitmap(fullColor) 
                    || !ImageUtil.CheckPaletteTransparent(bitmap))
                {//パレットではない画像なので何かしらの変換が必要
                    if (hintBitmap == null || 
                        (hintBitmap.Width != fullColor.Width && hintBitmap.Height != fullColor.Height) )
                    {
                        DialogResult dr = R.ShowNoYes("この画像のパレットは破壊されています。\r\n復元しようとしましたが、元画像と異なる画像のため、復元できませんでした。\r\n無視して強引にインポートすることは可能ですが、継続しますか？");
                        if (dr == DialogResult.No)
                        {
                            fullColor.Dispose();
                            bitmap.Dispose();
                            return false;
                        }
                    }
                    else
                    {//壊れたパレットを復元します.
                        Bitmap recolor = ImageUtil.ReColorPaletteWithHint(bitmap, hintBitmap);

                        ErrorPaletteMissMatchForm f = (ErrorPaletteMissMatchForm)InputFormRef.JumpFormLow<ErrorPaletteMissMatchForm>();
                        f.SetOrignalImage(bitmap, hintBitmap);
                        f.SetReOrderImage1(recolor, hintBitmap);
                        f.ShowDialog();
                        bitmap = f.GetResultBitmap();
                        if (bitmap == null)
                        {
                            fullColor.Dispose();
                            return false;
                        }
                    }
                }
            }
            else
            {//16色以上
                bitmap = ImageUtil.OpenBitmap(imagefilename); //16色に変換して開く.
                if (bitmap == null)
                {
                    fullColor.Dispose();
                    return false;
                }

                int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
                if (bitmap_palette_count > palette_index)
                {
                    R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_index);
                    fullColor.Dispose();
                    bitmap.Dispose();
                    return false;
                }

                if (ImageUtil.IsFullColorBitmap(fullColor) )
                {//パレットではない画像なので何かしらの変換が必要
                    if (hintBitmap == null 
                        || (hintBitmap.Width != fullColor.Width && hintBitmap.Height != fullColor.Height))
                    {
                        DialogResult dr = R.ShowNoYes("この画像のパレットは破壊されています。\r\n復元しようとしましたが、元画像と異なる画像のため、復元できませんでした。\r\n無視して強引にインポートすることは可能ですが、継続しますか？");
                        if (dr == DialogResult.No)
                        {
                            fullColor.Dispose();
                            bitmap.Dispose();
                            return false;
                        }
                    }
                    else
                    {//壊れたパレットを復元します.
                        Bitmap recolor = ImageUtil.ReColorPaletteWithHint(bitmap, hintBitmap);

                        ErrorPaletteMissMatchForm f = (ErrorPaletteMissMatchForm)InputFormRef.JumpFormLow<ErrorPaletteMissMatchForm>();
                        f.SetOrignalImage(bitmap, hintBitmap);
                        f.SetReOrderImage1(recolor, hintBitmap);
                        f.ShowDialog();
                        bitmap = f.GetResultBitmap();
                        if (bitmap == null)
                        {
                            fullColor.Dispose();
                            return false;
                        }
                    }
                }
            }

            MakePaletteBitmapToUI(bitmap, palette_index);
            fullColor.Dispose();
            bitmap.Dispose();
            return true;
        }


        public void MakePaletteBitmapToUI(Bitmap bitmap, int palette_index)
        {
            ColorPalette pal = bitmap.Palette;

            PushUndo();
            this.UndoLock = true;

            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);


                int index = palette_index * 0x10 + (paletteno - 1);
                byte dr = (byte)((pal.Entries[index].R));
                byte dg = (byte)((pal.Entries[index].G));
                byte db = (byte)((pal.Entries[index].B));

                //0だと ChangeValueイベントが発生しないので・・・
                if (0 == dr)
                {
                    r.Value = 1;
                }
                r.Value = dr;
                if (0 == dg)
                {
                    g.Value = 1;
                }
                g.Value = dg;
                if (0 == db)
                {
                    b.Value = 1;
                }
                b.Value = db;
            }
            this.UndoLock = false;
        }
        void MakePaletteByteGBAToUI(byte[] bin)
        {
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);

                uint p = U.u16(bin,(uint)(paletteno-1)*2);
                byte dr = (byte)((p & 0x1F) << 3);
                byte dg = (byte)(((p >> 5) & 0x1F) << 3);
                byte db = (byte)(((p >> 10) & 0x1F) << 3);

                //0だと ChangeValueイベントが発生しないので・・・
                if (0 == dr)
                {
                    r.Value = 1;
                }
                r.Value = dr;
                if (0 == dg)
                {
                    g.Value = 1;
                }
                g.Value = dg;
                if (0 == db)
                {
                    b.Value = 1;
                }
                b.Value = db;
            }
        }
        public void MakePaletteROMToUI(uint palette_address, bool isCompress, int palette_index)
        {
            uint addr = U.toOffset(palette_address);
            byte[] palttebyte;
            if (isCompress)
            {//圧縮されている場合
                if (addr == 0)
                {
                    return;
                }
                palttebyte = LZ77.decompress(Program.ROM.Data, addr);
                addr = 0;
                if (palttebyte.Length <= 0)
                {
                    R.ShowStopError("パレットをlz77解凍できません.");
                    return;
                }
            }
            else
            {//無圧縮パレット
                palttebyte = Program.ROM.Data;
            }

            this.UndoLock = true;
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);

                uint read_addr = addr + (uint)((paletteno - 1) * 2) + (uint)(palette_index * 0x20);
                if (read_addr >= palttebyte.Length)
                {//念のため読み込む範囲チェック
                    continue;
                }

                uint dp = U.u16(palttebyte, read_addr);
                byte dr = (byte)((dp & 0x1F));
                byte dg = (byte)(((dp >> 5) & 0x1F));
                byte db = (byte)(((dp >> 10) & 0x1F));

                //0だと ChangeValueイベントが発生しないので・・・
                if (0 == dr)
                {
                    r.Value = 1;
                }
                r.Value = dr << 3;
                if (0 == dg)
                {
                    g.Value = 1;
                }
                g.Value = dg << 3;
                if (0 == db)
                {
                    b.Value = 1;
                }
                b.Value = db << 3;
            }

            this.UndoLock = false;
        }
        public byte[] MakePaletteUIToByte()
        {
            byte[] palttebyte = new byte[16 * 2];

            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);

                uint dr = (((uint)r.Value) >> 3) & 0x1F;
                uint dg = (((uint)g.Value) >> 3) & 0x1F;
                uint db = (((uint)b.Value) >> 3) & 0x1F;

                uint dp = dr + (dg << 5) + (db << 10);
                U.write_u16(palttebyte, (uint)((paletteno - 1) * 2), dp);
            }
            return palttebyte;
        }

        byte[] MakePaletteUIToByte_OldValue()
        {
            byte[] palttebyte = new byte[16 * 2];

            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = FindNUD("R", paletteno);
                NumericUpDown g = FindNUD("G", paletteno);
                NumericUpDown b = FindNUD("B", paletteno);

                uint oldr = r.Tag is uint ? (uint)r.Tag : 0;
                uint oldg = g.Tag is uint ? (uint)g.Tag : 0;
                uint oldb = b.Tag is uint ? (uint)b.Tag : 0;

                uint dr = (oldr >> 3) & 0x1F;
                uint dg = (oldg >> 3) & 0x1F;
                uint db = (oldb >> 3) & 0x1F;

                uint dp = dr + (dg << 5) + (db << 10);
                U.write_u16(palttebyte, (uint)((paletteno - 1) * 2), dp);
            }
            return palttebyte;
        }

        public const int OVERRAIDE_ALL_PALETTE = 0xff;

        public uint MakePaletteUIToROM(uint palette_address, bool isCompress,int palette_index)
        {
            uint addr = U.toOffset(palette_address);
            if (!U.CheckZeroAddressWriteHigh(addr))
            {
                return U.NOT_FOUND;
            }

            string undo_name = "PALETTE " + U.To0xHexString(palette_address);

            byte[] palttebyte = MakePaletteUIToByte();
            if (isCompress)
            {//戦闘アニメのパレットは圧縮されている
                //現パレットの解凍
                byte[] currentdata = LZ77.decompress(Program.ROM.Data,addr);
                if (currentdata.Length <= 0)
                {//解凍できない アドレスを強引に書き換えたりしたのだろうか.
                    if (palette_index != 0 && palette_index != OVERRAIDE_ALL_PALETTE)
                    {
                        Log.Error("パレットを解凍できません.", U.To0xHexString(addr));
                        return U.NOT_FOUND;
                    }
                    //解凍できないけど、最初のパレットに書き込むなら、サイズは16*2で固定だから関係ない.
                    currentdata = palttebyte;
                }
                else if (palette_index == OVERRAIDE_ALL_PALETTE)
                {//複数あるパレットですべて同じパレットに設定する
                    int paletteCount = currentdata.Length / 0x20;
                    for (int i = 0; i < paletteCount; i++)
                    {
                        uint write_addr = (uint)(i * 0x20);
                        U.write_range(currentdata, write_addr, palttebyte);
                    }
                }
                else
                {//複数あるパレットの一部を書き換える.
                    uint write_addr = (uint)(palette_index * 0x20);
                    if (currentdata.Length < write_addr + palttebyte.Length)
                    {//サイズが足りない場合増設する.
                        Array.Resize(ref currentdata, (int)(write_addr + palttebyte.Length));
                    }

                    U.write_range(currentdata, write_addr, palttebyte);
                }

                byte[] palettebyteZ = LZ77.compress(currentdata);

                //圧縮されているということは、サイズが可変なわけで、元の場所に書き込めるか、拡張領域かを検討しないといけない。
                Undo.UndoData undodata = Program.Undo.NewUndoData(undo_name);
                uint newaddr = InputFormRef.WriteBinaryData(this.SelfForm, addr, palettebyteZ, InputFormRef.get_data_pos_callback_lz77, undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }
                Program.Undo.Push(undodata);
                return newaddr;
            }
            else
            {//無圧縮パレット
                if (palette_index == OVERRAIDE_ALL_PALETTE)
                {//無圧縮パレットでは、パレットのサイズがわからないので、すべて同じ色にすることは不可能
                    R.Error("無圧縮パレットなので、すべて同じパレットにすることはできません。");
                    Debug.Assert(false);
                    return U.NOT_FOUND;
                }

                addr = addr + (uint)(palette_index * 0x20);

                Program.Undo.Push(undo_name, addr, (uint)palttebyte.Length);
                Program.ROM.write_range(addr, palttebyte);
                return addr;
            }
        }

        public void MakePaletteBitmapToROM(Bitmap bitmap, uint palette_address, int palette_count,Undo.UndoData undodata)
        {
            ColorPalette pal = bitmap.Palette;
            MakePaletteColorPaletteToROM(pal, palette_address, palette_count, undodata);
        }
        public static byte[] NewNullPalette(int palette_count = 1)
        {
            return new byte[16 * 2 * palette_count];
        }
        public void MakePaletteColorPaletteToROM(ColorPalette pal, uint palette_address, int palette_count, Undo.UndoData undodata)
        {
            uint addr = U.toOffset(palette_address);
            if (!U.CheckZeroAddressWriteHigh(palette_address))
            {
                return;
            }
            Debug.Assert(palette_count >= 1);
            byte[] palttebyte = new byte[16 * 2 * palette_count];

            string undo_name = "PALETTE " + U.To0xHexString(palette_address);


            for (int paletteno = 1; paletteno <= 16 * palette_count ; paletteno++)
            {
                uint dr = (((uint)pal.Entries[paletteno - 1].R) >> 3) & 0x1F;
                uint dg = (((uint)pal.Entries[paletteno - 1].G) >> 3) & 0x1F;
                uint db = (((uint)pal.Entries[paletteno - 1].B) >> 3) & 0x1F;

                uint dp = dr + (dg << 5) + (db << 10);
                U.write_u16(palttebyte,  (uint)((paletteno - 1) * 2), dp);
            }

            //無圧縮パレットとして書き込む
            Program.ROM.write_range(addr, palttebyte, undodata);
            InputFormRef.ShowWriteNotifyAnimation(this.SelfForm, palette_address);
        }
        public bool PALETTE_TO_CLIPBOARD_BUTTON_Click()
        {
            byte[] palttebyte = MakePaletteUIToByte();
            if (palttebyte.Length < 2 * 16)
            {
                return false;
            }

            StringBuilder sb = new StringBuilder();
            for (uint n = 0; n < 16; n ++ )
            {
                uint p = U.big16(palttebyte, n * 2);
                sb.Append(  p.ToString("X04") );
            }
            PaletteClipboardForm f = (PaletteClipboardForm)InputFormRef.JumpFormLow<PaletteClipboardForm>();
            f.SetPalText(sb.ToString());
            DialogResult dr = f.ShowDialog();
            if (dr != DialogResult.Yes)
            {
                return false;
            }

            string paltext = f.GetPalText();
            for (uint n = 0; n < 16; n ++ )
            {
                string hexString = U.substr(paltext, (int)n * 4, 4);
                uint hex = U.atoh(hexString);

                U.write_big16(palttebyte, n * 2, hex);
            }
            MakePaletteByteGBAToUI(palttebyte);
            return true;
        }

        class UndoData
        {
            //UNDO サイズも小さいから、差分よりすべて記録する. 
            public byte[] PaletteBIN;
        };

        bool UndoLock;

        List<UndoData> UndoBuffer;
        int UndoPosstion;
        //Undo履歴のクリア
        public void ClearUndoBuffer()
        {
            this.UndoBuffer = new List<UndoData>();
            this.UndoPosstion = 0;
        }
        public void PushUndo()
        {
            if (UndoLock)
            {
                return;
            }

            if (this.UndoPosstion < this.UndoBuffer.Count)
            {//常に先頭に追加したいので、リスト中に戻っている場合は、それ以降を消す.
                this.UndoBuffer.RemoveRange(this.UndoPosstion, this.UndoBuffer.Count - this.UndoPosstion);
            }
            UndoData p = new UndoData();
            p.PaletteBIN = MakePaletteUIToByte_OldValue();
            this.UndoBuffer.Add(p);
            this.UndoPosstion = this.UndoBuffer.Count;
        }
        public void RunUndo()
        {
            if (this.UndoPosstion <= 0)
            {
                return; //無理
            }
            if (this.UndoPosstion == this.UndoBuffer.Count)
            {//現在が、undoがない最新版だったら、redoできるように、現状を保存する.
                PushUndo();
                this.UndoPosstion = UndoPosstion - 1;
            }

            this.UndoPosstion = UndoPosstion - 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);

        }
        public void RunRedo()
        {
            if (this.UndoPosstion + 1 >= this.UndoBuffer.Count)
            {
                return; //無理
            }
            this.UndoPosstion = UndoPosstion + 1;
            RunUndoRollback(this.UndoBuffer[UndoPosstion]);
        }
        void RunUndoRollback(UndoData u)
        {
            this.UndoLock = true;
            MakePaletteByteGBAToUI(u.PaletteBIN);
            this.UndoLock = false;
        }
    }
}
