using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace FEBuilderGBA
{
    public class PaletteFormRef
    {
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

        public static PictureBox MakePaletteUI_FindSampleImage(List<Control> controls)
        {
            foreach (Control info in controls)
            {
                if (info.Width < 128 || info.Height < 128)
                {//小さすぎ
                    continue;
                }

                if (!(info is PictureBox))
                {
                    continue;
                }

                return (PictureBox)info;
            }
            return null;
        }

        public static Control MakePaletteUI_FindObject<_TYPE>(List<Control> controls, string symbol, int paletteno)
        {
            foreach (Control info in controls)
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
                if (!(info is _TYPE))
                {
                    continue;
                }
                return info;
            }
            return null;
        }

        static MouseEventHandler MakePaletteUI_Label_MouseClickEvent(Form self, List<Control> controls, Label obj, int paletteno)
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
                //選択された色の取得
                NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", paletteno);
                r.Value = (cd.Color.R >> 3) << 3;
                NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", paletteno);
                g.Value = (cd.Color.G >> 3) << 3;
                NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", paletteno);
                b.Value = (cd.Color.B >> 3) << 3;
            }
            ;
        }
        static EventHandler MakePaletteUI_Label_ColorSwap(Form self, List<Control> controls, Label obj, int paletteno)
        {
            return (sender, e) =>
            {
                PaletteSwapForm form = (PaletteSwapForm)InputFormRef.JumpFormLow<PaletteSwapForm>();
                form.SetMainColorIndex(paletteno);
                Color[] colormap = new Color[16 + 1];
                for (int i = 1; i <= 16; i++)
                {
                    NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", i);
                    NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", i);
                    NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", i);
                    form.SetColor(i, (int)r.Value, (int)g.Value, (int)b.Value);
                    colormap[i] = Color.FromArgb((int)r.Value, (int)g.Value, (int)b.Value);
                }
                DialogResult dr = form.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                int selected = form.GetSelectedColorIndex();

                for (int i = 1; i <= 16; i++)
                {
                    NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", i);
                    NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", i);
                    NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", i);

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
            }
            ;
        }

        static EventHandler MakePaletteUI_Label_ColorChanges(Form self, List<Control> controls, Label obj, int paletteno, Func<Bitmap> getSampleBitmap)
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
                    NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", i);
                    NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", i);
                    NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", i);
                    form.SetColor(i, (int)r.Value, (int)g.Value, (int)b.Value);
                    colormap[i] = Color.FromArgb((int)r.Value, (int)g.Value, (int)b.Value);
                }
                DialogResult dr = form.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                for (int i = 1; i <= 16; i++)
                {
                    NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", i);
                    NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", i);
                    NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", i);

                    Color rgb = form.GetColor(i);
                    r.Value = rgb.R;
                    g.Value = rgb.G;
                    b.Value = rgb.B;
                }
            }
            ;
        }
        public static EventHandler MakePaletteUI_NumericUpDown_ChangeEvent(Form self, List<Control> controls, NumericUpDown obj, string symbol, int paletteno, Func<Color, int, bool> onChangeColor)
        {
            return (sender, e) =>
            {
                Label p = (Label)MakePaletteUI_FindObject<Label>(controls, "P", paletteno);
                int v = (int)obj.Value;
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
                    obj.ForeColor = Color.FromArgb(255, (v > 128 ? 0 : 255), 255);
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


        public static void MakePaletteUI(Form self, Func<Color, int, bool> onChangeColor, Func<Bitmap> getSampleBitmap)
        {
            List<Control> controls = InputFormRef.GetAllControls(self);
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                Label p = (Label)MakePaletteUI_FindObject<Label>(controls, "P", paletteno);
                NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", paletteno);
                NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", paletteno);
                NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", paletteno);

                p.MouseClick += MakePaletteUI_Label_MouseClickEvent(self, controls, p, paletteno);
                p.Cursor = Cursors.Hand;

                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem;

                menuItem = new MenuItem(R._("色の交換"));
                menuItem.Click += MakePaletteUI_Label_ColorSwap(self, controls, p, paletteno);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem(R._("色違いを作る"));
                menuItem.Click += MakePaletteUI_Label_ColorChanges(self, controls, p, paletteno, getSampleBitmap);
                contextMenu.MenuItems.Add(menuItem);

                p.ContextMenu = contextMenu;

                EventHandler r_eh = MakePaletteUI_NumericUpDown_ChangeEvent(self, controls, r, "R", paletteno, onChangeColor);
                r.ValueChanged += r_eh;
                r.Tag = r_eh;
                r.Increment = 1 << 3;

                EventHandler g_eh = MakePaletteUI_NumericUpDown_ChangeEvent(self, controls, g, "G", paletteno, onChangeColor);
                g.ValueChanged += g_eh;
                g.Tag = g_eh;
                g.Increment = 1 << 3;

                EventHandler b_eh = MakePaletteUI_NumericUpDown_ChangeEvent(self, controls, b, "B", paletteno, onChangeColor);
                b.ValueChanged += b_eh;
                b.Tag = b_eh;
                b.Increment = 1 << 3;
            }
        }
        //現在選択されている色を全適応
        public static void MakePaletteUIApplyEvent(Form self)
        {
            List<Control> controls = InputFormRef.GetAllControls(self);
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", paletteno);
                NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", paletteno);
                NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", paletteno);

                if (r.Tag != null)
                {
                    EventHandler r_eh = (EventHandler)r.Tag;
                    r_eh(self, null);
                }
                if (g.Tag != null)
                {
                    EventHandler g_eh = (EventHandler)g.Tag;
                    g_eh(self, null);
                }
                if (b.Tag != null)
                {
                    EventHandler b_eh = (EventHandler)b.Tag;
                    b_eh(self, null);
                }
            }
        }
        public static bool MakePaletteBitmapToUIEx(Form self, int palette_index, Bitmap hintBitmap)
        {
            Debug.Assert(ImageUtil.Is16ColorBitmap(hintBitmap));

            string imagefilename = ImageFormRef.OpenFilenameDialogFullColor(self);
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

            PaletteFormRef.MakePaletteBitmapToUI(self, bitmap, palette_index);
            fullColor.Dispose();
            bitmap.Dispose();
            return true;
        }


        public static void MakePaletteBitmapToUI(Form self, Bitmap bitmap, int palette_index)
        {
            ColorPalette pal = bitmap.Palette;

            List<Control> controls = InputFormRef.GetAllControls(self);
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", paletteno);
                NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", paletteno);
                NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", paletteno);


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
        }
        static void MakePaletteByteGBAToUI(Form self, byte[] bin)
        {
            List<Control> controls = InputFormRef.GetAllControls(self);
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", paletteno);
                NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", paletteno);
                NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", paletteno);

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
        public static void MakePaletteROMToUI(Form self, uint palette_address, bool isCompress, int palette_index)
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

            List<Control> controls = InputFormRef.GetAllControls(self);
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", paletteno);
                NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", paletteno);
                NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", paletteno);

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
        }
        public static byte[] MakePaletteUIToByte(Form self)
        {
            byte[] palttebyte = new byte[16 * 2];

            List<Control> controls = InputFormRef.GetAllControls(self);
            for (int paletteno = 1; paletteno <= 16; paletteno++)
            {
                NumericUpDown r = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "R", paletteno);
                NumericUpDown g = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "G", paletteno);
                NumericUpDown b = (NumericUpDown)MakePaletteUI_FindObject<NumericUpDown>(controls, "B", paletteno);

                uint dr = (((uint)r.Value) >> 3) & 0x1F;
                uint dg = (((uint)g.Value) >> 3) & 0x1F;
                uint db = (((uint)b.Value) >> 3) & 0x1F;

                uint dp = dr + (dg << 5) + (db << 10);
                U.write_u16(palttebyte, (uint)((paletteno - 1) * 2), dp);
            }
            return palttebyte;
        }

        public static uint MakePaletteUIToROM(Form self, uint palette_address, bool isCompress,int palette_index)
        {
            uint addr = U.toOffset(palette_address);
            if (!U.CheckZeroAddressWriteHigh(addr))
            {
                return U.NOT_FOUND;
            }

            string undo_name = "PALETTE " + U.To0xHexString(palette_address);

            byte[] palttebyte = MakePaletteUIToByte(self);
            if (isCompress)
            {//戦闘アニメのパレットは圧縮されている
                //現パレットの解凍
                byte[] currentdata = LZ77.decompress(Program.ROM.Data,addr);
                if (currentdata.Length <= 0)
                {//解凍できない アドレスを強引に書き換えたりしたのだろうか.
                    if (palette_index > 0)
                    {
                        Log.Error("パレットを解凍できません.", U.To0xHexString(addr));
                        return U.NOT_FOUND;
                    }
                    //解凍できないけど、最初のパレットに書き込むなら、サイズは16*2で固定だから関係ない.
                    currentdata = palttebyte;
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
                uint newaddr = InputFormRef.WriteBinaryData(self,addr, palettebyteZ, InputFormRef.get_data_pos_callback_lz77, undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }
                Program.Undo.Push(undodata);
                return newaddr;
            }
            else
            {//無圧縮パレット
                addr = addr + (uint)(palette_index * 0x20);

                Program.Undo.Push(undo_name, addr, (uint)palttebyte.Length);
                Program.ROM.write_range(addr, palttebyte);
                return addr;
            }
        }

        public static void MakePaletteBitmapToROM(Form self, Bitmap bitmap, uint palette_address, int palette_count,Undo.UndoData undodata)
        {
            ColorPalette pal = bitmap.Palette;
            MakePaletteColorPaletteToROM(self, pal, palette_address, palette_count,undodata);
        }
        public static byte[] NewNullPalette(int palette_count = 1)
        {
            return new byte[16 * 2 * palette_count];
        }
        public static void MakePaletteColorPaletteToROM(Form self, ColorPalette pal, uint palette_address, int palette_count, Undo.UndoData undodata)
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
            InputFormRef.ShowWriteNotifyAnimation(self, palette_address);
        }
        public static bool PALETTE_TO_CLIPBOARD_BUTTON_Click(Form self)
        {
            byte[] palttebyte = MakePaletteUIToByte(self);
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
            MakePaletteByteGBAToUI(self, palttebyte);
            return true;
        }
    }
}
