using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapPathEditorForm : Form
    {
        Pen MouseCursorPen = new Pen(Color.Red, 1);
        Pen SelectMarkupPen = new Pen(Color.Blue, 3);

        public WorldMapPathEditorForm()
        {
            InitializeComponent();
            U.SetIcon(SaveASbutton, Properties.Resources.icon_arrow);
            U.SetIcon(LoadButton, Properties.Resources.icon_upload);
        }

        Bitmap MapCache;
        Bitmap WorldmapBGCache;
        Bitmap PathCacheImage;
        
        private void WorldMapPathEditorForm_Load(object sender, EventArgs e)
        {
            this.WorldmapBGCache = WorldMapImageForm.DrawWorldMap();
            this.PathCacheImage = WorldMapPathForm.GetPathImage();
            this.MapCache = ImageUtil.CloneBitmap(this.WorldmapBGCache);
            
            MakePathCombo();
            MakePATHCHIPLIST();

            U.SelectedIndexSafety(PathType,0);

        }
        void MakePathCombo()
        {
            List<U.AddrResult> list = WorldMapPathForm.MakeList();
            U.ConvertComboBox(list, ref this.PathType);
        }
        void MakePATHCHIPLIST()
        {
            Bitmap pathChipList = ImageUtil.Blank(PathCacheImage.Width * 5, PathCacheImage.Height, PathCacheImage);
            ImageUtil.BitBlt(pathChipList, 0, 0, PathCacheImage.Width, PathCacheImage.Height, PathCacheImage, 0, 0);
            for (int y = 0; y < PathCacheImage.Height; y++ )
            {
                Bitmap flip = ImageUtil.Copy(PathCacheImage, 0, y * 8, 8, 8, true);
                ImageUtil.BitBlt(pathChipList, 8 * 1, y * 8, 8, 8, flip, 0, 0);

                flip = ImageUtil.Copy(PathCacheImage, 0, y * 8, 8, 8, false,true);
                ImageUtil.BitBlt(pathChipList, 8 * 2, y * 8, 8, 8, flip, 0, 0);

                flip = ImageUtil.Copy(PathCacheImage, 0, y * 8, 8, 8, true, true);
                ImageUtil.BitBlt(pathChipList, 8 * 3, y * 8, 8, 8, flip, 0, 0);
            }

            PATHCHIPLIST.Image = pathChipList;
        }

        class Path
        {
            public int worldmap_x;
            public int worldmap_y;
            public int path_x;
            public int path_y;
        };
        List<Path> PathList;

        void LoadPath()
        {
            PathList = new List<Path>();

            if(this.PathType.SelectedIndex < 0)
            {
                return ;
            }

            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.PathType, this.PathType.SelectedIndex);
            uint addr = ar.addr;
            if (!U.isSafetyOffset(addr))
            {
                return ;
            }
            uint p = Program.ROM.p32(addr+0);
            if (!U.isSafetyOffset(p))
            {
                return;
            }

            if (Program.ROM.u32(p) == 0x0)
            {//道データがnull 必須0x01 があるので 0はありえない
                return;
            }
            U.ForceUpdate(MapAddress, U.toPointer(p));
            LoadPathLow(Program.ROM.Data ,p);
        }
        void LoadPathLow(byte[] bin,uint addr)
        {
            PathList = new List<Path>();
            if (addr >= bin.Length)
            {
                return;
            }

            uint p = addr;
            while (true)
            {
                if (p + 4 >= bin.Length)
                {
                    return;
                }

                uint x8 = U.u8(bin, p + 0);
                uint y8 = U.u8(bin, p + 1);
                uint count = U.u8(bin, p + 2);

                if (x8 == 0xFF)
                {
                    return;
                }

                p += 4;
                for (uint ix = 0; ix < count; ix++)
                {
                    if (p + 2 >= bin.Length)
                    {
                        return;
                    }

                    uint tile = U.u8(bin, p + 0);
                    uint flag = U.u8(bin, p + 1);
                    p += 2;

                    Path path = new Path();
                    path.worldmap_x = (int)(x8 * 8 + (ix * 8));
                    path.worldmap_y = (int)(y8 * 8);
                    path.path_y = (int)tile * 8;

                    if (flag == 4)
                    {
                        path.path_x = 1 * 8;
                    }
                    else if (flag == 8)
                    {
                        path.path_x = 2 * 8;
                    }
                    else if (flag == 0xC)
                    {
                        path.path_x = 3 * 8;
                    }
                    else
                    {
                        path.path_x = 0 * 8;
                    }

                    PathList.Add(path);
                }
            }
        }

        void MakeWorldMap()
        {
            ImageUtil.BitBlt(this.MapCache
                , 0, 0
                , this.MapCache.Width, this.MapCache.Height
                , this.WorldmapBGCache
                , 0, 0);
            
            //道のパレット追加.
            int pathPaletteMapping = ImageUtil.AppendPalette(MapCache,PathCacheImage);
            
            int index = this.PathType.SelectedIndex;
            if (index <= -1)
            {
                this.WorldMap.Image = MapCache;
                return;
            }
            for (int i = 0; i < PathList.Count; i++)
            {
                int flip = PathList[i].path_x / 8;
                Bitmap pathOne;
                if (flip == 0)
                {
                    pathOne = ImageUtil.Copy(PathCacheImage,0,PathList[i].path_y,8,8,false,false);
                }
                else if (flip == 1)
                {
                    pathOne = ImageUtil.Copy(PathCacheImage,0,PathList[i].path_y,8,8,true,false);
                }
                else if (flip == 2)
                {
                    pathOne = ImageUtil.Copy(PathCacheImage,0,PathList[i].path_y,8,8,false,true);
                }
                else 
                {
                    pathOne = ImageUtil.Copy(PathCacheImage,0,PathList[i].path_y,8,8,true,true);
                }

                ImageUtil.BitBlt(MapCache
                    , PathList[i].worldmap_x
                    , PathList[i].worldmap_y
                    , 8
                    , 8
                    , pathOne, 0, 0
                    , pathPaletteMapping);
                
            }

            //拠点を追加.
            List<U.AddrResult> arlist = WorldMapPointForm.MakeWorldMapPointList();
            for (int i = 0; i < arlist.Count; i++)
            {
                WorldMapPointForm.DrawBasePointAddr(MapCache,arlist[i].addr);
            }

            this.WorldMap.Image = MapCache;
        }

        private void PathType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearUndoBuffer();
            LoadPath();
            MakeWorldMap();
        }

        Point MAPCHIPLISTMouseCursor = new Point();
        private void PATHCHIPLIST_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / 8 * 8;
            int y = e.Y / 8 * 8;

            MAPCHIPLISTMouseCursor.X = x;
            MAPCHIPLISTMouseCursor.Y = y;

            PATHCHIPLIST.Invalidate();
        }

        //チップセット選択
        Point SelectCursor = new Point();

        Point WorldmapMouseCursor = new Point();
        private void WorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / 8 * 8;
            int y = e.Y / 8 * 8;

            WorldmapMouseCursor.X = x;
            WorldmapMouseCursor.Y = y;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PutPathChip(x, y);
                return;
            }

            WorldMap.Invalidate();
        }

        void PutPathChip(int x,int y)
        {
            PushUndo();

            //4列目は消去指定
            bool isErase = (SelectCursor.X >= 4 * 8);

            int i = 0;
            for (; i < PathList.Count; i++)
            {
                if (PathList[i].worldmap_x == x && PathList[i].worldmap_y == y)
                {
                    if (isErase)
                    {//4列目は消去指定.
                        PathList.RemoveAt(i);
                    }
                    else
                    {
                        PathList[i].path_x = SelectCursor.X;
                        PathList[i].path_y = SelectCursor.Y;
                    }
                    MakeWorldMap();
                    return;
                }
            }

            if (isErase)
            {//4列目は消去指定.
                //nop
            }
            else
            {//0-3列目は追加.
                Path p = new Path();
                p.worldmap_x = x;
                p.worldmap_y = y;
                p.path_x = SelectCursor.X;
                p.path_y = SelectCursor.Y;
                PathList.Add(p);
            }
            MakeWorldMap();
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
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
            if (WorldmapMouseCursor.X >= 0 && WorldmapMouseCursor.Y >= 0)
            {
                //マウスカーソルの描画
                e.Graphics.DrawRectangle(this.MouseCursorPen
                    , WorldmapMouseCursor.X, WorldmapMouseCursor.Y, 8, 8);
            }
        }

        private void PATHCHIPLIST_MouseDown(object sender, MouseEventArgs e)
        {
            SelectCursor.X = e.X / 8 * 8;
            SelectCursor.Y = e.Y / 8 * 8;
        }

        private void WorldMap_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X / 8 * 8;
            int y = e.Y / 8 * 8;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {//右ボタンを押すと、現在のマップチップを選択する スポイトツール
                SelectSpointChip(x, y);
                PATHCHIPLIST.Invalidate();
                return;
            }

            PutPathChip(x, y);
        }

        void SelectSpointChip(int x, int y)
        {
            int i = 0;
            for (; i < PathList.Count; i++)
            {
                if (PathList[i].worldmap_x == x && PathList[i].worldmap_y == y)
                {
                    SelectCursor.X = PathList[i].path_x;
                    SelectCursor.Y = PathList[i].path_y;
                    return;
                }
            }
        }

        //道データの書き込み
        void WritePathData()
        {
            List<byte> data = new List<byte>();

            for (int i = 0; i < PathList.Count; )
            {
                U.append_u8(data, (uint)(PathList[i].worldmap_x / 8));
                U.append_u8(data, (uint)(PathList[i].worldmap_y / 8));
                //Y軸が同じデータがいくつ続くか調べる.
                int n = i + 1;
                int lasty = PathList[i].worldmap_y;
                for (; n < PathList.Count; n++)
                {
                    if (lasty != PathList[n].worldmap_y)
                    {
                        break;
                    }
                }

                U.append_u8(data, (uint)(n-i)); //Y軸の個数
                U.append_u8(data, (uint)1); //常に1

                for (; i < n; i++)
                {
                    uint tile = (uint)PathList[i].path_y / 8;
                    uint flag = (uint)PathList[i].path_x / 8;
                    if (flag == 0)
                    {
                        flag = 0x0;
                    }
                    else if (flag == 1)
                    {
                        flag = 0x4;
                    }
                    else if (flag == 2)
                    {
                        flag = 0x8;
                    }
                    else
                    {
                        flag = 0xC;
                    }
                    U.append_u8(data, tile);
                    U.append_u8(data, flag);
                }
            }
            //終端データ
            U.append_u8(data, 0xFF);
            U.append_u8(data, 0x0);
            U.append_u8(data, 0x0);
            U.append_u8(data, 0x0);

            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.PathType, this.PathType.SelectedIndex);
            if (!U.isSafetyOffset(ar.addr))
            {
                return;
            }

            string undo_name = PathType.Text;
            Undo.UndoData undodata = Program.Undo.NewUndoData(undo_name);

            uint addr = (uint)MapAddress.Value;
            uint newaddr = InputFormRef.WriteBinaryData(this
                , addr
                , data.ToArray()
                , get_path_data_length_callback
                , undodata);
            if (newaddr == U.NOT_FOUND)
            {
                return;
            }
            U.ForceUpdate(MapAddress, U.toPointer(newaddr));

            //道データのポインタ1を書き換える.
            Program.ROM.write_p32(ar.addr + 0, newaddr, undodata);

            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
        }
        //道データの長さを求める.
        static MoveToUnuseSpace.ADDR_AND_LENGTH get_path_data_length_callback(uint addr)
        {
            addr = U.toOffset(addr);

            uint length = WorldMapPathForm.CalcPathDataLength(addr);
            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = length;
            return aal;
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (this.PathType.SelectedIndex < 0)
            {
                return;
            }

            //左上から格納しないといけないので、ソートする
            PathList.Sort((Path a, Path b) =>
            {
                if (a.worldmap_y > b.worldmap_y)
                {
                    return 1;
                }
                if (a.worldmap_y < b.worldmap_y)
                {
                    return -1;
                }
                if (a.worldmap_x > b.worldmap_x)
                {
                    return 1;
                }
                if (a.worldmap_x < b.worldmap_x)
                {
                    return -1;
                }
                return 0;
            });

            //道データの書き込み
            WritePathData();

            U.ReSelectList(PathType);
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
        }
        public void JumpTo(uint pathid)
        {
            U.SelectedIndexSafety(PathType, pathid, false);
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            RunRedo();
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            RunUndo();
        }

        private void WorldMapPathEditorForm_KeyDown(object sender, KeyEventArgs e)
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
            public List<Path> MAR;//UNDO 小さいので、差分よりすべて記録する. 
        };
        List<UndoData> UndoBuffer;
        int UndoPosstion;
        //Undo履歴のクリア
        void ClearUndoBuffer()
        {
            this.UndoBuffer = new List<UndoData>();
            this.UndoPosstion = 0;
        }
        List<Path> ClonePathList(List<Path> mar)
        {
            List<Path> list = new List<Path>();
            for (int i = 0; i < mar.Count; i++)
            {
                Path p = new Path();
                p.path_x = mar[i].path_x;
                p.path_y = mar[i].path_y;
                p.worldmap_x = mar[i].worldmap_x;
                p.worldmap_y = mar[i].worldmap_y;

                list.Add(p);
            }
            return list;
        }
        void PushUndo()
        {
            if (this.UndoPosstion < this.UndoBuffer.Count)
            {//常に先頭に追加したいので、リスト中に戻っている場合は、それ以降を消す.
                this.UndoBuffer.RemoveRange(this.UndoPosstion, this.UndoBuffer.Count - this.UndoPosstion);
            }
            UndoData p = new UndoData();
            p.MAR = ClonePathList(this.PathList);

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
            this.PathList = ClonePathList(u.MAR);
            MakeWorldMap();
        }

        private void SaveASbutton_Click(object sender, EventArgs e)
        {
            uint addr = U.toOffset( (uint)this.MapAddress.Value );
            uint length = WorldMapPathForm.CalcPathDataLength(addr);
            if (! U.isSafetyOffset(addr))
            {
                return ;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("Road.BIN|*.road.bin|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "" , save);

            //書き込み.
            byte[] bin = Program.ROM.getBinaryData(addr , length);
            U.WriteAllBytes(save.FileName, bin);

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileName);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("Road.BIN|*.road.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            open.ShowDialog();
            if (!U.CanReadFileRetry(open))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", open);
            string filename = open.FileNames[0];

            byte[] bin = File.ReadAllBytes(filename);

            PushUndo();
            LoadPathLow(bin, 0);
            MakeWorldMap();
        }

    }
}
