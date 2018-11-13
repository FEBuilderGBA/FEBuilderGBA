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
    public partial class ImageBattleScreenForm : Form
    {
        Pen MouseCursorPen = new Pen(Color.Cyan, 2);
        Pen SelectMarkupPen = new Pen(Color.Blue, 3);
        SolidBrush ForeBrush;
        SolidBrush BackBrush;

        public ImageBattleScreenForm()
        {
            InitializeComponent();
            U.SetIcon(ExportButton, U.GetShell32Icon(122));
            U.SetIcon(ImportButton, U.GetShell32Icon(45));
            this.ForeBrush = new SolidBrush(this.ForeColor);
            this.BackBrush = new SolidBrush(this.BackColor);
        }

        List<int> IsNotUseList = new List<int>();
        bool IsNotUse(int v)
        {
            return this.IsNotUseList.IndexOf(v) >= 0;
        }

        ImageFormRef Image1;
        ImageFormRef Image2;
        ImageFormRef Image3;
        ImageFormRef Image4;
        ImageFormRef Image5;
        private void BattleScreenForm_Load(object sender, EventArgs e)
        {
            ClearUndoBuffer();

            uint palette = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_palette_pointer());
            U.ForceUpdate(PALETTE_ADDRESS, palette);
            PaletteFormRef.MakePaletteUI(this, OnChangeColor);
            this.PaletteIndexComboBox.SelectedIndex = 0;

            InitLoadChipsetInfo();
            LoadChipsetInfo();  //チップセット関係の読込
            LoadBattleScreen(); //ROM TSAをメモリに読み込んで
//            MakeBattleScreen(); //TSA描画
            Zoom.SelectedIndex = 1; //2倍拡大
        }


        void InitLoadChipsetInfo()
        {
            int[] srcImageWidth = new int[5];
            Size size = U.CalcLZ77ImageToSizePointer(Program.ROM.RomInfo.battle_screen_image1_pointer());
            srcImageWidth[0] = size.Width;
            srcImageWidth[1] = U.CalcLZ77LinerImagePointerToWidth(Program.ROM.RomInfo.battle_screen_image2_pointer());
            srcImageWidth[2] = U.CalcLZ77LinerImagePointerToWidth(Program.ROM.RomInfo.battle_screen_image3_pointer());
            srcImageWidth[3] = U.CalcLZ77LinerImagePointerToWidth(Program.ROM.RomInfo.battle_screen_image4_pointer());
            srcImageWidth[4] = U.CalcLZ77LinerImagePointerToWidth(Program.ROM.RomInfo.battle_screen_image5_pointer());

            this.image1_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image1_pointer());
            this.Image1 = new ImageFormRef(this, "image1", size.Width, size.Height, 1, Program.ROM.RomInfo.battle_screen_image1_pointer(), 0, Program.ROM.RomInfo.battle_screen_palette_pointer());
            this.image2_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image2_pointer());
            this.Image2 = new ImageFormRef(this, "image2", srcImageWidth[1], 1 * 8, 1, Program.ROM.RomInfo.battle_screen_image2_pointer(), 0, Program.ROM.RomInfo.battle_screen_palette_pointer());
            this.image3_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image3_pointer());
            this.Image3 = new ImageFormRef(this, "image3", srcImageWidth[2], 1 * 8, 1, Program.ROM.RomInfo.battle_screen_image3_pointer(), 0, Program.ROM.RomInfo.battle_screen_palette_pointer());
            this.image4_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image4_pointer());
            this.Image4 = new ImageFormRef(this, "image4", srcImageWidth[3], 1 * 8, 1, Program.ROM.RomInfo.battle_screen_image4_pointer(), 0, Program.ROM.RomInfo.battle_screen_palette_pointer());
            this.image5_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image5_pointer());
            this.Image5 = new ImageFormRef(this, "image5", srcImageWidth[4], 1 * 8, 1, Program.ROM.RomInfo.battle_screen_image5_pointer(), 0, Program.ROM.RomInfo.battle_screen_palette_pointer());
        }
        void LoadChipsetInfo()
        {
            this.image1_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image1_pointer());
            this.image2_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image2_pointer());
            this.image3_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image3_pointer());
            this.image4_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image4_pointer());
            this.image5_ZIMAGE.Value = Program.ROM.u32(Program.ROM.RomInfo.battle_screen_image5_pointer());

            this.IsNotUseList = new List<int>();
            this.ChipCache = GetChipImage(this.IsNotUseList);
            this.DrawBitmap = ImageUtil.Blank(MAP_X * 8, MAP_Y * 8, this.ChipCache);

            MakeCHIPLIST();
        }

        static Bitmap GetChipImage(List<int> notUseList)
        {
            uint[] image_pos = new uint[] {
                 Program.ROM.RomInfo.battle_screen_image1_pointer()
                ,Program.ROM.RomInfo.battle_screen_image2_pointer()
                ,Program.ROM.RomInfo.battle_screen_image3_pointer()
                ,Program.ROM.RomInfo.battle_screen_image4_pointer()
                ,Program.ROM.RomInfo.battle_screen_image5_pointer()
            };
            List<byte[]>  unlz77_images = new List<byte[]>();
            int total_height = 0;
            for (int i = 0; i < image_pos.Length; i++)
            {
                uint image = Program.ROM.p32(image_pos[i]);
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
                int width = 8;
                int height = ImageUtil.CalcHeight(width, imageUZ.Length);
                unlz77_images.Add(imageUZ);
                total_height += height;
            }

            uint palette = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_palette_pointer());
            Bitmap bitmap = ImageUtil.Blank(8, total_height, Program.ROM.Data, (int)palette);

            int copy_height = 0;
            for (int i = 0; i < image_pos.Length; i++ )
            {
                byte[] imageUZ = unlz77_images[i];
                int width = 8;
                int height = ImageUtil.CalcHeight(width, imageUZ.Length);
                Bitmap src = ImageUtil.ByteToImage16Tile(width, height, imageUZ, 0, Program.ROM.Data, (int)palette);

                ImageUtil.BitBlt(bitmap, 0, copy_height, 8, height, src, 0, 0);
                copy_height += height;

                src.Dispose();

                if (i >= 1)
                {
                    notUseList.Add(copy_height - 8 - 8);
                    notUseList.Add(copy_height - 8);
                }
            }

            return bitmap;
        }


        void MakeCHIPLIST()
        {
            Bitmap pathChiplist = ImageUtil.Blank(this.ChipCache.Width * 4 * 2, this.ChipCache.Height, this.ChipCache);
            for (int y = 0; y < this.ChipCache.Height; y++)
            {
                Bitmap flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8,false,false);
                ImageUtil.BitBlt(pathChiplist, 0, y * 8, 8, 8, flip, 0, 0, 0);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, true,false);
                ImageUtil.BitBlt(pathChiplist, 8 * 1, y * 8, 8, 8, flip, 0, 0);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, false, true);
                ImageUtil.BitBlt(pathChiplist, 8 * 2, y * 8, 8, 8, flip, 0, 0);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, true, true);
                ImageUtil.BitBlt(pathChiplist, 8 * 3, y * 8, 8, 8, flip, 0, 0);
                flip.Dispose();


                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, false, false);
                ImageUtil.BitBlt(pathChiplist, 8 * 4, y * 8, 8, 8, flip, 0, 0, 1);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, true, false);
                ImageUtil.BitBlt(pathChiplist, 8 * 5, y * 8, 8, 8, flip, 0, 0, 1);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, false, true);
                ImageUtil.BitBlt(pathChiplist, 8 * 6, y * 8, 8, 8, flip, 0, 0, 1);
                flip.Dispose();

                flip = ImageUtil.Copy(this.ChipCache, 0, y * 8, 8, 8, true, true);
                ImageUtil.BitBlt(pathChiplist, 8 * 7, y * 8, 8, 8, flip, 0, 0, 1);
                flip.Dispose();
            }

            CHIPLIST.Image = pathChiplist;
        }

        const int MAP_X = 32;
        const int MAP_Y = 20;
        ushort[] Map;

        void LoadBattleScreen()
        {
            this.Map = LoadBattleScreenLow();
        }

        static ushort[] LoadBattleScreenLow()
        {
            ushort[] map = new ushort[MAP_X * MAP_Y];

            uint addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA1_pointer());
            for (int y = 0; y <= 5; y++)
            {
                for (int x = 1; x <= 15; x++)
                {
                    uint a = Program.ROM.u16(addr);
                    map[y * MAP_X + x] = (ushort)a;

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA2_pointer());
            for (int y = 0; y <= 5; y++)
            {
                for (int x = 16; x <= 30; x++)
                {
                    uint a = Program.ROM.u16(addr);
                    map[y * MAP_X + x] = (ushort)a;

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA3_pointer());
            for (int y = 13; y <= 19; y++)
            {
                for (int x = 1; x <= 15; x++)
                {
                    uint a = Program.ROM.u16(addr);
                    map[y * MAP_X + x] = (ushort)a;

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA4_pointer());
            for (int y = 13; y <= 19; y++)
            {
                for (int x = 16; x <= 31; x++)
                {
                    uint a = Program.ROM.u16(addr);
                    map[y * MAP_X + x] = (ushort)a;

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA5_pointer());
            for (int y = 0; y <= 19; y++)
            {
                for (int x = 31; x <= 32; x++)
                {
                    uint a = Program.ROM.u16(addr);

                    int xx = x;
                    if (x == 32)
                    {
                        xx = 0;
                    }
                    map[y * MAP_X + xx] = (ushort)a;

                    addr += 2;
                }
            }
            return map;
        }
        private void WriteButton_Click(object sender, EventArgs e)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"BattleScreen");

            uint addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA1_pointer());
            undodata.list.Add(new Undo.UndoPostion(addr, 6 * 16 * 2));
            for (int y = 0; y <= 5; y++)
            {
                for (int x = 1; x <= 15; x++)
                {
                    uint a = this.Map[y * MAP_X + x];
                    Program.ROM.write_u16(addr,a);

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA2_pointer());
            undodata.list.Add(new Undo.UndoPostion(addr, 6 * 16 * 2));
            for (int y = 0; y <= 5; y++)
            {
                for (int x = 16; x <= 30; x++)
                {
                    uint a = this.Map[y * MAP_X + x];
                    Program.ROM.write_u16(addr, a);

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA3_pointer());
            undodata.list.Add(new Undo.UndoPostion(addr, 6 * 16 * 2));
            for (int y = 13; y <= 19; y++)
            {
                for (int x = 1; x <= 15; x++)
                {
                    uint a = this.Map[y * MAP_X + x];
                    Program.ROM.write_u16(addr, a);

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA4_pointer());
            undodata.list.Add(new Undo.UndoPostion(addr, 6 * 17 * 2));
            for (int y = 13; y <= 19; y++)
            {
                for (int x = 16; x <= 31; x++)
                {
                    uint a = this.Map[y * MAP_X + x];
                    Program.ROM.write_u16(addr, a);

                    addr += 2;
                }
            }

            addr = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA5_pointer());
            undodata.list.Add(new Undo.UndoPostion(addr, 19 * 2 * 2));
            for (int y = 0; y <= 19; y++)
            {
                for (int x = 31; x <= 32; x++)
                {
                    int xx = x;
                    if (x == 32)
                    {
                        xx = 0;
                    }
                    uint a = this.Map[y * MAP_X + xx];
                    Program.ROM.write_u16(addr, a);

                    addr += 2;
                }
            }

            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.battle_screen_image1_pointer(), this.image1_ZIMAGE, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.battle_screen_image2_pointer(), this.image2_ZIMAGE, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.battle_screen_image3_pointer(), this.image3_ZIMAGE, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.battle_screen_image4_pointer(), this.image4_ZIMAGE, undodata);
            InputFormRef.WriteOnePointer(Program.ROM.RomInfo.battle_screen_image5_pointer(), this.image5_ZIMAGE, undodata);
            this.Image1.WritePointer(undodata);
            this.Image2.WritePointer(undodata);
            this.Image3.WritePointer(undodata);
            this.Image4.WritePointer(undodata);
            this.Image5.WritePointer(undodata);

            Program.Undo.Push(undodata);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);

            LoadChipsetInfo();  //チップセット関係の読込
            LoadBattleScreen(); //ROM TSAをメモリに読み込んで
            MakeBattleScreen(); //TSA描画
            this.Battle.Invalidate(); //キャンバスの再描画
        }

        Bitmap DrawBitmap;
        Bitmap ChipCache;
        void MakeBattleScreen()
        {
            for(int y = 0 ; y < MAP_Y ; y++)
            {
                for(int x = 0 ; x < MAP_X ; x++)
                {
                    ushort m = this.Map[y * MAP_X + x];
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
                return;
            }

            Battle.Invalidate();
        }

        void PutPathChip(int x,int y)
        {
            uint tile = (uint)SelectCursor.Y / 8;
            uint pal_flip = 0x00;

            switch (SelectCursor.X / 8)
            {
                case 0:
                    pal_flip = 0x00;
                    break;
                case 1:
                    pal_flip = 0x04;
                    break;
                case 2:
                    pal_flip = 0x08;
                    break;
                case 3:
                    pal_flip = 0x0C;
                    break;
                case 4:
                    pal_flip = 0x10;
                    break;
                case 5:
                    pal_flip = 0x14;
                    break;
                case 6:
                    pal_flip = 0x18;
                    break;
                case 7:
                    pal_flip = 0x1C;
                    break;
            }

            int write_index = (y) * MAP_X + (x);
            if (write_index < 0 || write_index >= this.Map.Length)
            {
                return;
            }

            PushUndo();
            this.Map[write_index] = (ushort)((pal_flip << 8) | tile);
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

//                if (this.IsNotUse(MAPCHIPLISTMouseCursor.Y))
//                {
//                    U.DrawRectanglePopupY(e,MAPCHIPLISTMouseCursor,this.Font,this.BackBrush,this.ForeBrush
//                        , R._("このタイルは、\r\n利用\r\nできません"));
//                }
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

        private void WorldMap_MouseDown(object sender, MouseEventArgs e)
        {
            int zoom = GetZoom();
            int chipsize = 8 * zoom;

            int x = e.X / chipsize * chipsize;
            int y = e.Y / chipsize * chipsize;
            PutPathChip(x / chipsize, y / chipsize);
        }


        private void PALETTE_POINTER_ValueChanged(object sender, EventArgs e)
        {
            if (PALETTE_ADDRESS.Value == 0)
            {
                return;
            }
            PaletteFormRef.MakePaletteROMToUI(this, (uint)PALETTE_ADDRESS.Value,false , this.PaletteIndexComboBox.SelectedIndex);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
        }
        private void PaletteIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PALETTE_POINTER_ValueChanged(null, null);
        }

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            uint addr = PaletteFormRef.MakePaletteUIToROM(this, (uint)PALETTE_ADDRESS.Value,false, this.PaletteIndexComboBox.SelectedIndex );
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            InputFormRef.ShowWriteNotifyAnimation(this, addr);
        }

        private bool OnChangeColor(Color color, int paletteno)
        {
            if (this.DrawBitmap == null)
            {
                return false;
            }

            ColorPalette palette = this.DrawBitmap.Palette; //一度、値をとってからいじらないと無視される
            palette.Entries[paletteno] = color;
            this.DrawBitmap.Palette = palette;
            this.Battle.Image = DrawZoomMap(this.DrawBitmap);
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

        private void BattleScreenForm_KeyDown(object sender, KeyEventArgs e)
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

        private void ExportButton_Click(object sender, EventArgs e)
        {
            MakeBattleScreen(); //TSA描画
            ImageFormRef.ExportImage(this,this.DrawBitmap, InputFormRef.MakeSaveImageFilename(this,R._("戦闘画面")), 4);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }

            int width = MAP_X * 8;
            int height = MAP_Y * 8;
            int palette_count = 4;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                return;
            }
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (bitmap_palette_count > palette_count)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return;
            }
            //元画像
            this.IsNotUseList = new List<int>();
            Bitmap orignalBitmap = GetChipImage(this.IsNotUseList);
            byte[] orignalImage = ImageUtil.ImageToByte16Tile(orignalBitmap, orignalBitmap.Width, orignalBitmap.Height);
            orignalBitmap.Dispose();

            //TSAを維持して、元画像に最小限の変化を加えたもの.
            byte[] saveImage = ImageUtil.ImageToByteKeepTSA(bitmap, width, height, this.Map, orignalImage);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像データを5分割して書き込み
                RevChipImage(saveImage, undodata);

                //パレットデータの書き込み
                byte[] paletteData = ImageUtil.ImageToPalette(bitmap, palette_count);
                uint newPaletteAddr = ImageFormRef.WriteImageData(this
                    , Program.ROM.p32(Program.ROM.RomInfo.battle_screen_palette_pointer())
                    , Program.ROM.RomInfo.battle_screen_palette_pointer()
                    , paletteData, false, undodata);

                Program.Undo.Push(undodata);

                PALETTE_ADDRESS.Value = newPaletteAddr;
                this.PaletteIndexComboBox.SelectedIndex = 0;

            }
            LoadChipsetInfo();  //チップセット関係の読込
            LoadBattleScreen(); //ROM TSAをメモリに読み込んで
            MakeBattleScreen(); //TSA描画
        }


        //GetChipImageの逆をする
        void RevChipImage(byte[] bigbitmap, Undo.UndoData undodata)
        {
            uint[] image_pos = new uint[] {
                 Program.ROM.RomInfo.battle_screen_image1_pointer()
                ,Program.ROM.RomInfo.battle_screen_image2_pointer()
                ,Program.ROM.RomInfo.battle_screen_image3_pointer()
                ,Program.ROM.RomInfo.battle_screen_image4_pointer()
                ,Program.ROM.RomInfo.battle_screen_image5_pointer()
            };

            uint readPos = 0;
            for (int i = 0; i < image_pos.Length; i++)
            {
                if (readPos >= bigbitmap.Length)
                {
                    Program.Undo.Rollback(undodata);
                    throw new Exception(R.Error("途中で書き込むデータが終了してしまいました。i: {0} readPos:{1} bigbitmap:{2}",i, readPos, bigbitmap.Length));
                }

                //数を知りたいので一度すべて解凍します.
                uint image = Program.ROM.p32(image_pos[i]);
                byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
                int width = 8;
                int height = ImageUtil.CalcHeight(width, imageUZ.Length);

                uint nextPos = (uint)(readPos + ((height / 8)*32));
                byte[] writeData = U.subrange(bigbitmap, readPos, nextPos);

                uint newAddr = ImageFormRef.WriteImageData(this, image,image_pos[i], writeData, true, undodata);
                if (newAddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    throw new Exception(R.Error("書き込みに失敗しました。i: {0} readPos:{1} bigbitmap:{2}", i, readPos, bigbitmap.Length));
                }
                readPos = nextPos;
            }
        }

        //サンプル用の戦闘画面描画
        public static Bitmap DrawBattleSreenBitmap()
        {
            List<int> dummyList = new List<int>();
            Bitmap chips = GetChipImage(dummyList);
            byte[] orignalImage = ImageUtil.ImageToByte16Tile(chips, chips.Width, chips.Height);

            const int palette_count = 4;
            ushort[] tsa = LoadBattleScreenLow();
            byte[] paletteData = ImageUtil.ImageToPalette(chips, palette_count);

            Bitmap ret = ImageUtil.ByteToImage16TileInner(MAP_X * 8
                , MAP_Y * 8
                , orignalImage
                , 0
                , paletteData
                , 0
                , tsa
                , 0);
            //FE7とFE6は、真ん中に変なデータがあるので塗りつぶす.
            if (Program.ROM.RomInfo.version() <= 7)
            {
                Bitmap black = ImageUtil.Blank(32*8,7*8, ret);
                ImageUtil.BitBlt(ret, 0, 6 * 8, 32 * 8, 7 * 8, black, 0, 0);
                black.Dispose();
            }
            return ret;
        }
        //サンプル用の戦闘画面描画(戦闘画面 30*20 で表示 左右1タイルずつ消す.)
        public static Bitmap DrawBattleSreenBitmap30x20()
        {
            Bitmap bitmap = DrawBattleSreenBitmap();
            return ImageUtil.Copy(bitmap, 8, 0, ImageUtilOAM.SCREEN_TILE_WIDTH_M1 * 8, ImageUtilOAM.SCREEN_TILE_HEIGHT * 8);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            uint tsa;
            tsa = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA1_pointer());
            FEBuilderGBA.Address.AddAddress(list, tsa
                , (5+1) * ((15+1)-1) * 2
                , Program.ROM.RomInfo.battle_screen_TSA1_pointer()
                , "battle_screen_TSA1"
                , FEBuilderGBA.Address.DataTypeEnum.TSA);
            tsa = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA2_pointer());
            FEBuilderGBA.Address.AddAddress(list, tsa
                , (5 + 1) * ((30 + 16) - 1) * 2
                , Program.ROM.RomInfo.battle_screen_TSA2_pointer()
                , "battle_screen_TSA2"
                , FEBuilderGBA.Address.DataTypeEnum.TSA);
            tsa = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA3_pointer());
            FEBuilderGBA.Address.AddAddress(list, tsa
                , ((19 + 1)-13) * ((15 + 1) - 1) * 2
                , Program.ROM.RomInfo.battle_screen_TSA3_pointer()
                , "battle_screen_TSA3"
                , FEBuilderGBA.Address.DataTypeEnum.TSA);
            tsa = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA4_pointer());
            FEBuilderGBA.Address.AddAddress(list, tsa
                , ((19 + 1) - 13) * ((31 + 1) - 16) * 2
                , Program.ROM.RomInfo.battle_screen_TSA4_pointer()
                , "battle_screen_TSA4"
                , FEBuilderGBA.Address.DataTypeEnum.TSA);

            tsa = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_TSA5_pointer());
            FEBuilderGBA.Address.AddAddress(list, tsa
                , ((19 + 1) - 0) * ((32 + 1) - 31) * 2
                , Program.ROM.RomInfo.battle_screen_TSA5_pointer()
                , "battle_screen_TSA5"
                , FEBuilderGBA.Address.DataTypeEnum.TSA);

            uint pal = Program.ROM.p32(Program.ROM.RomInfo.battle_screen_palette_pointer());
            FEBuilderGBA.Address.AddAddress(list, pal
                , 0x20 * 4
                , Program.ROM.RomInfo.battle_screen_palette_pointer()
                , "battle_screen_palette"
                , FEBuilderGBA.Address.DataTypeEnum.PAL);

            FEBuilderGBA.Address.AddLZ77Pointer(list, Program.ROM.RomInfo.battle_screen_image1_pointer()
                , "battle_screen_image1", isPointerOnly,FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list, Program.ROM.RomInfo.battle_screen_image2_pointer()
                , "battle_screen_image1", isPointerOnly, FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list, Program.ROM.RomInfo.battle_screen_image3_pointer()
                , "battle_screen_image1", isPointerOnly, FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list, Program.ROM.RomInfo.battle_screen_image4_pointer()
                , "battle_screen_image1", isPointerOnly, FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
            FEBuilderGBA.Address.AddLZ77Pointer(list, Program.ROM.RomInfo.battle_screen_image5_pointer()
                , "battle_screen_image1", isPointerOnly, FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);

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

        private void Zoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeBattleScreen();
        }

        private void ImageBattleScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MouseCursorPen != null)
            {
                MouseCursorPen.Dispose();
                MouseCursorPen = null;
            }
            if (SelectMarkupPen != null)
            {
                SelectMarkupPen.Dispose();
                SelectMarkupPen = null;
            }
            if (ForeBrush != null)
            {
                ForeBrush.Dispose();
                ForeBrush = null;
            }
            if (BackBrush != null)
            {
                BackBrush.Dispose();
                BackBrush = null;
            }
        }
    }
}
