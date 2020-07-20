using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageTSAEditorForm : Form
    {
        Pen MouseCursorPen = new Pen(Color.Cyan, 2);
        Pen SelectMarkupPen = new Pen(Color.Blue, 3);

        public ImageTSAEditorForm()
        {
            InitializeComponent();
        }

        ImageFormRef Image1;
        uint Width8;
        uint Height8;
        uint ZImgPointer;
        bool IsHeaderTSA;
        bool IsLZ77TSA;
        uint TSAPointer;
        uint PalettePointer;
        uint PaletteAddress;
        int PaletteCount;
        public void Init(uint width8, uint height8, uint zimgPointer, bool isHeaderTSA, bool isLZ77TSA, uint tsaPointer, uint palettePointer, uint paletteAddress, int paletteCount)
        {
            ClearUndoBuffer();

            this.ZImgPointer = zimgPointer;
            this.IsHeaderTSA = isHeaderTSA;
            this.IsLZ77TSA = isLZ77TSA;
            this.TSAPointer = tsaPointer;
            this.PalettePointer = palettePointer;
            this.PaletteAddress = paletteAddress;
            this.Width8 = width8;
            this.Height8 = height8;
            this.PaletteCount = paletteCount;

            if (this.IsHeaderTSA)
            {
                this.Width8 = Math.Max(256 / 8, this.Width8);
                this.Height8 = Math.Max(160 / 8, this.Height8);
            }

            if (this.PalettePointer == U.NOT_FOUND)
            {
                PALETTE_ADDRESS.Value = U.toOffset(this.PaletteAddress);
            }
            else
            {
                PALETTE_ADDRESS.Value = Program.ROM.p32(this.PalettePointer);
            }
            this.PFR = new PaletteFormRef(this);
            this.PFR.MakePaletteUI(OnChangeColor, GetSampleBitmap);
            this.PaletteIndexComboBox.SelectedIndex = 0;

            InitLoadChipsetInfo();
            LoadChipsetInfo();  //チップセット関係の読込
            LoadBattleScreen(); //ROM TSAをメモリに読み込んで
            //MakeBattleScreen(); //TSA描画
            Zoom.SelectedIndex = 1; //2倍拡大
            
            EraseSurplusPalette(); //余剰パレットの削除.
            ShowTSAInfo();
        }
        PaletteFormRef PFR;

        void ShowTSAInfo()
        {
            this.Info.Text = String.Format("{0}x{1}", this.Width8 * 8, this.Height8 * 8);
        }

        int SearchMaxPalette()
        {
            int count = this.PaletteCount;
            for (int i = 0; i < this.Map.Length; i++)
            {
                ushort m = this.Map[i];
                int pal  = (int)((m >> (8 + 4)) & 0x0f);
                if (count < pal)
                {
                    count = pal;
                }
            }
            return count;
        }
        void EraseSurplusPalette()
        {
            int palCount = SearchMaxPalette();
            while (true)
            {
                if (this.PaletteIndexComboBox.Items.Count <= palCount)
                {
                    break;
                }
                //所定数になるまで後ろからけずっていく.
                this.PaletteIndexComboBox.Items.RemoveAt(this.PaletteIndexComboBox.Items.Count - 1);
            }
        }

        private void ImageTSAEditorForm_Load(object sender, EventArgs e)
        {
        }

        void InitLoadChipsetInfo()
        {
            uint image = Program.ROM.p32(this.ZImgPointer);
            Size size = U.CalcLZ77ImageToSizePointer(this.ZImgPointer);

            this.image1_ZIMAGE.Value = image;
            this.Image1 = new ImageFormRef(this, "image1", size.Width, size.Height, 1, this.ZImgPointer, 0, this.PalettePointer);
        }

        void LoadChipsetInfo()
        {
            uint image = Program.ROM.p32(this.ZImgPointer);
            this.image1_ZIMAGE.Value = image;

            this.ChipCache = GetChipImage();
            this.DrawBitmap = ImageUtil.Blank((int)this.Width8 * 8, (int)this.Height8 * 8, this.ChipCache);

            MakeCHIPLIST();
        }

        Bitmap GetChipImage()
        {
            int pal = this.PaletteIndexComboBox.SelectedIndex;
            if (pal < 0)
            {
                pal = 0;
            }

            uint palette;
            if (this.PalettePointer == U.NOT_FOUND)
            {
                palette = U.toOffset(this.PaletteAddress);
            }
            else
            {
                palette = Program.ROM.p32(this.PalettePointer);
            }
            palette = palette + (uint)(pal*0x20);

            int total_height = 0;
            uint image = Program.ROM.p32(this.ZImgPointer);
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
            int width = 8;
            int height = ImageUtil.CalcHeight(width, imageUZ.Length);
            Bitmap src = ImageUtil.ByteToImage16Tile(width, height, imageUZ, 0, Program.ROM.Data, (int)palette);

            Bitmap bitmap = ImageUtil.Blank(8, height , Program.ROM.Data, (int)palette);
            ImageUtil.BitBlt(bitmap, 0, total_height, 8, height, src, 0, 0);
            total_height += height;

            src.Dispose();

            return bitmap;
        }


        void MakeCHIPLIST()
        {
            this.ChipDisplayCache = ImageUtil.Blank(this.ChipCache.Width * 4, this.ChipCache.Height, this.ChipCache);
            for (int y = 0; y < this.ChipCache.Height; y++)
            {
                Bitmap flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8);
                ImageUtil.BitBlt(this.ChipDisplayCache, 0, y * 8, 8, 8, flip, 0, 0, 0);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, true);
                ImageUtil.BitBlt(this.ChipDisplayCache, 8 * 1, y * 8, 8, 8, flip, 0, 0);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, false, true);
                ImageUtil.BitBlt(this.ChipDisplayCache, 8 * 2, y * 8, 8, 8, flip, 0, 0);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, true, true);
                ImageUtil.BitBlt(this.ChipDisplayCache, 8 * 3, y * 8, 8, 8, flip, 0, 0);
                flip.Dispose();
            }

            CHIPLIST.Image = this.ChipDisplayCache;
        }

        ushort[] Map;
        void LoadBattleScreen()
        {
            uint tsapos = Program.ROM.p32(this.TSAPointer);
            if (this.IsLZ77TSA)
            {
                byte[] tsadata = LZ77.decompress(Program.ROM.Data, tsapos);
                if (tsadata.Length <= 0)
                {//解凍できない
                    this.Map = new ushort[]{};
                    return;
                }
                if (this.IsHeaderTSA)
                {
                    this.Map = ImageUtil.ByteToHeaderTSA(tsadata, 0, (int)this.Width8 * 8, (int)this.Height8 * 8);
                }
                else
                {
                    this.Map = ImageUtil.ByteToTSA(tsadata, 0, (int)this.Width8 * 8, (int)this.Height8 * 8);
                }
            }
            else
            {
                if (this.IsHeaderTSA)
                {
                    this.Map = ImageUtil.ByteToHeaderTSA(Program.ROM.Data, (int)tsapos, (int)this.Width8 * 8, (int)this.Height8 * 8);
                }
                else
                {
                    this.Map = ImageUtil.ByteToTSA(Program.ROM.Data, (int)tsapos, (int)this.Width8 * 8, (int)this.Height8 * 8);
                }
            }
        }
        private void WriteButton_Click(object sender, EventArgs e)
        {
            uint addr = Program.ROM.p32(this.TSAPointer);
            byte[] tsaByte = new byte[this.Width8 * this.Height8 * 2];
            Buffer.BlockCopy(this.Map, 0, tsaByte, 0, tsaByte.Length); //(byte[])

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"TSA Editor");
            if (this.IsHeaderTSA)
            {
                uint tsaHeaderX = this.Width8;
                uint tsaHeaderY = this.Height8;

                uint tsa_width_margin = 32 - tsaHeaderX;
                tsaHeaderX += tsa_width_margin;
                tsaByte = ImageUtil.TSAToHeaderTSA(tsaByte, (int)tsaHeaderX * 8, (int)tsaHeaderY * 8, (int)tsa_width_margin);
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                uint newAddr = ImageFormRef.WriteImageData(this, addr, this.TSAPointer, tsaByte, this.IsLZ77TSA, undodata);

                InputFormRef.WriteOnePointer(this.ZImgPointer, this.image1_ZIMAGE, undodata);
                this.Image1.WritePointer(undodata);
                ImageSystemIconForm.Fix_FE8_systemmenu_battlepreview_image(U.toOffset(this.ZImgPointer), undodata);
                Program.Undo.Push(undodata);

                //パレットも書き込む.
                PFR.MakePaletteUIToROM((uint)PALETTE_ADDRESS.Value, false, this.PaletteIndexComboBox.SelectedIndex);

                InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
                InputFormRef.ShowWriteNotifyAnimation(this, newAddr);
            }

            //チップセットを読みこむときに、最初のパレットを選択しないとダメだ.
            int selectedPalette = this.PaletteIndexComboBox.SelectedIndex;
            this.PaletteIndexComboBox.SelectedIndex = 0;
            LoadChipsetInfo();  //チップセット関係の読込
            this.PaletteIndexComboBox.SelectedIndex = selectedPalette;

            LoadBattleScreen(); //ROM TSAをメモリに読み込んで
            MakeBattleScreen(); //TSA描画
            this.Battle.Invalidate(); //キャンバスの再描画
        }

        Bitmap DrawBitmap;
        Bitmap ChipCache;
        Bitmap ChipDisplayCache;
        void MakeBattleScreen()
        {
            for (int y = 0; y < this.Height8; y++)
            {
                for (int x = 0; x < this.Width8; x++)
                {
                    ushort m = this.Map[y * this.Width8 + x];
                    int tile = (int)(m & 0xff);
                    int flip = (int)((m >> 8) & 0x0f);
                    int pal  = (int)((m >> (8 + 4)) & 0x0f);

                    Bitmap pathOne;
                    if (flip == 0)
                    {
                        pathOne = ImageUtil.Copy(this.ChipCache, 0, tile * 8, 8, 8, false, false);
                    }
                    else if (flip == 4)
                    {
                        pathOne = ImageUtil.Copy(this.ChipCache, 0, tile * 8, 8, 8, true, false);
                    }
                    else if (flip == 8)
                    {
                        pathOne = ImageUtil.Copy(this.ChipCache, 0, tile * 8, 8, 8, false, true);
                    }
                    else
                    {
                        pathOne = ImageUtil.Copy(this.ChipCache, 0, tile * 8, 8, 8, true, true);
                    }

                    ImageUtil.BitBlt(this.DrawBitmap
                        , x * 8
                        , y * 8
                        , 8
                        , 8
                        , pathOne, 0, 0
                        , pal);
                }
            }

            this.Battle.Image = DrawZoomMap(this.DrawBitmap);
        }

        //拡大したbitmapを取得する.
        Bitmap DrawZoomMap(Bitmap map)
        {
            int zoom = GetZoom();
            Bitmap zoomPic = new Bitmap(zoom * map.Width, zoom * map.Height);
            using (Graphics g = Graphics.FromImage(zoomPic))
            {
                g.DrawImage(map, 0, 0, zoom * map.Width, zoom * map.Height);
            }
            return zoomPic;
        }
        int GetZoom()
        {
            if (Zoom.SelectedIndex <= 0)
            {
                return 1;
            }
            return Zoom.SelectedIndex + 1;
        }

        Point MAPCHIPLISTMouseCursor = new Point();
        private void PATHCHIPLIST_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / 8 * 8;
            int y = e.Y / 8 * 8;

            MAPCHIPLISTMouseCursor.X = x;
            MAPCHIPLISTMouseCursor.Y = y;

            this.TSAInfo.Text = GetTSAInfoText();
            CHIPLIST.Invalidate();
        }

        //チップセット選択
        Point SelectCursor = new Point();

        Point BattleScreenCursor = new Point();
        private void Battle_MouseMove(object sender, MouseEventArgs e)
        {
            int zoom = GetZoom();
            int chipsize = 8 * zoom;

            int x = e.X / chipsize * chipsize;
            int y = e.Y / chipsize * chipsize;

            BattleScreenCursor.X = x;
            BattleScreenCursor.Y = y;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PutPathChip(x / chipsize, y / chipsize);
                this.TSAInfo.Text = GetTSAInfoText();
                return;
            }

            this.TSAInfo.Text = GetTSAInfoText();
            Battle.Invalidate();
        }

        string GetTSAInfoText()
        {

            string ret;
            ret = "Selected:";
            {
                int chipsize = 8;

                //int x = MAPCHIPLISTMouseCursor.X / chipsize;
                int y = MAPCHIPLISTMouseCursor.Y / chipsize;
                ret += U.ToHexString2(y) ;
            }

            ret += "   ";
            ret += "Canvas:";
            {
                int zoom = GetZoom();
                int chipsize = 8 * zoom;

                int x = BattleScreenCursor.X / chipsize;
                int y = BattleScreenCursor.Y / chipsize;
                ret += GetMapInfoText(x, y);
            }
            return ret;
        }


        string GetMapInfoText(int x, int y)
        {
            int write_index = (y) * (int)this.Width8 + (x);
            if (write_index < 0 || write_index >= this.Map.Length)
            {
                return "";
            }

            uint m = this.Map[write_index];
            int tile = (int)(m & 0xff);

            return U.ToHexString2(tile);
        }

        void PutPathChip(int x,int y)
        {
            uint tile = (uint)SelectCursor.Y / 8;
            uint pal = (uint)this.PaletteIndexComboBox.SelectedIndex;
            uint flip ;

            switch (SelectCursor.X / 8)
            {
                case 0:
                default:
                    flip = 0x00;
                    break;
                case 1:
                    flip = 0x04;
                    break;
                case 2:
                    flip = 0x08;
                    break;
                case 3:
                    flip = 0x0C;
                    break;
            }

            int write_index = (y) * (int)this.Width8 + (x);
            if (write_index < 0 || write_index >= this.Map.Length)
            {
                return;
            }

            PushUndo();
            this.Map[write_index] = (ushort)(((pal & 0x0f) << (8 + 4)) | ((flip & 0x0f) << 8) | tile);
            MakeBattleScreen();

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
        }

        private void PATHCHIPLIST_Paint(object sender, PaintEventArgs e)
        {
            if (MAPCHIPLISTMouseCursor.X >= 0 && MAPCHIPLISTMouseCursor.Y >= 0)
            {
                //マウスカーソルの描画
                e.Graphics.DrawRectangle(this.MouseCursorPen
                    , MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y, 8, 8);
            }

            e.Graphics.DrawRectangle(this.SelectMarkupPen
                , SelectCursor.X, SelectCursor.Y, 8, 8);
        }

        private void WorldMap_Paint(object sender, PaintEventArgs e)
        {
            int zoom = GetZoom();
            if (BattleScreenCursor.X >= 0 && BattleScreenCursor.Y >= 0)
            {
                //マウスカーソルの描画
                e.Graphics.DrawRectangle(this.MouseCursorPen
                    , BattleScreenCursor.X, BattleScreenCursor.Y, 8 * zoom, 8 * zoom);
            }
        }

        private void PATHCHIPLIST_MouseDown(object sender, MouseEventArgs e)
        {
            SelectCursor.X = e.X / 8 * 8;
            SelectCursor.Y = e.Y / 8 * 8;
        }

        private void TSAEditor_MouseDown(object sender, MouseEventArgs e)
        {
            int zoom = GetZoom();
            int chipsize = 8 * zoom;

            int x = e.X / chipsize * chipsize;
            int y = e.Y / chipsize * chipsize;
            x = x / chipsize;
            y = y / chipsize;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {//右ボタンを押すと、現在のマップチップを選択する スポイトツール
                SelectSpointChip(x, y);
                CHIPLIST.Invalidate();
                return;
            }

            PutPathChip(x, y);
        }

        void SelectSpointChip(int x, int y)
        {
            int read_index = (y) * (int)this.Width8 + (x);
            if (read_index < 0 || read_index >= this.Map.Length)
            {
                return;
            }

            ushort a = this.Map[read_index];
            uint xx = 0;
            uint yy = (uint)(a & 0xff);

            uint pal_flip = (uint)((a >> 8) & 0xf);
            if (pal_flip == 0x00)
            {
                xx = 0;
            }
            else if (pal_flip == 0x04)
            {
                xx = 1 ;
            }
            else if (pal_flip == 0x08)
            {
                xx = 2 ;
            }
            else if (pal_flip == 0x0C)
            {
                xx = 3 ;
            }

            SelectCursor.X = (int)(xx * 8);
            SelectCursor.Y = (int)(yy * 8);
        }

        private void PALETTE_POINTER_ValueChanged(object sender, EventArgs e)
        {
            if (PALETTE_ADDRESS.Value == 0)
            {
                return;
            }
            PFR.MakePaletteROMToUI((uint)PALETTE_ADDRESS.Value,false , this.PaletteIndexComboBox.SelectedIndex);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            this.Battle.Image = this.DrawBitmap;
            this.CHIPLIST.Image = this.ChipDisplayCache;
        }
        private void PaletteIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PALETTE_POINTER_ValueChanged(null, null);
        }

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            uint addr = PFR.MakePaletteUIToROM((uint)PALETTE_ADDRESS.Value,false, this.PaletteIndexComboBox.SelectedIndex );
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            InputFormRef.ShowWriteNotifyAnimation(this, addr);
        }

        Bitmap GetSampleBitmap()
        {
            if (this.ChipCache == null || this.DrawBitmap == null)
            {
                return null;
            }

            int sel = PaletteIndexComboBox.SelectedIndex;
            if (sel < 0)
            {
                return null;
            }

            Bitmap newbitmap = ImageUtil.SwapPalette(this.DrawBitmap, (int)sel);
            return newbitmap;
        }

        private bool OnChangeColor(Color color, int paletteno)
        {
            if (this.ChipCache == null || this.DrawBitmap == null)
            {
                return false;
            }

            int sel = PaletteIndexComboBox.SelectedIndex;
            if (sel < 0)
            {
                return false;
            }

            {
                ColorPalette palette = this.ChipDisplayCache.Palette; //一度、値をとってからいじらないと無視される
                palette.Entries[paletteno] = color;
                this.ChipDisplayCache.Palette = palette;
            }
            {

                ColorPalette palette = this.ChipCache.Palette; //一度、値をとってからいじらないと無視される
                palette.Entries[paletteno + (sel * 0x10)] = color;
                this.ChipCache.Palette = palette;
            }
            {
                ColorPalette palette = this.DrawBitmap.Palette; //一度、値をとってからいじらないと無視される
                palette.Entries[paletteno + (sel*0x10) ] = color;
                this.DrawBitmap.Palette = palette;
            }

            this.Battle.Image = DrawZoomMap(this.DrawBitmap);
            this.CHIPLIST.Image = this.ChipDisplayCache;
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, true);
            return true;
        }


        private void UndoButton_Click(object sender, EventArgs e)
        {
            RunUndo();
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            RunRedo();
        }

        private void ImageTSAEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                RunUndo();
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                RunRedo();
            }
        }


        class UndoData
        {
            public ushort[] MAR;//UNDO 小さいので、差分よりすべて記録する. 
        };
        List<UndoData> UndoBuffer;
        int UndoPosstion;
        //Undo履歴のクリア
        void ClearUndoBuffer()
        {
            this.UndoBuffer = new List<UndoData>();
            this.UndoPosstion = 0;
        }
        void PushUndo()
        {
            if (this.UndoPosstion < this.UndoBuffer.Count)
            {//常に先頭に追加したいので、リスト中に戻っている場合は、それ以降を消す.
                this.UndoBuffer.RemoveRange(this.UndoPosstion, this.UndoBuffer.Count - this.UndoPosstion);
            }
            UndoData p = new UndoData();
            p.MAR = (UInt16[])this.Map.Clone();

            this.UndoBuffer.Add(p);
            this.UndoPosstion = this.UndoBuffer.Count;
        }
        void RunUndo()
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
        void RunRedo()
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
            this.Map = (ushort[])u.MAR.Clone();

            MakeBattleScreen();
        }

        private void PaletteIndexComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            PALETTE_POINTER_ValueChanged(null, null);
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

        private void Zoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeBattleScreen();
        }


        private void UndoPlaetteButton_Click(object sender, EventArgs e)
        {
            PFR.RunUndo();
        }
        private void RedoPaletteButton_Click(object sender, EventArgs e)
        {
            PFR.RunRedo();
        }
    }
}
