using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace FEBuilderGBA
{
    public partial class MapPictureBox : UserControl
    {
        //マップ画像
        Bitmap MapBitMap;
        //選択された枠を描画するペン
        Pen MouseCursorPen = new Pen(Color.Red, 1);
        Pen SelectMarkupPen = new Pen(Color.Blue, 2);
        Pen SelectMarkup2Pen = new Pen(Color.BlueViolet, 2);
        Pen SelectMarkup3Pen = new Pen(Color.DodgerBlue, 2);
        
        int ChipSize = 16;
        Bitmap DefualtIcon;
        int DefualtIconDrawXAdd = 0;
        int DefualtIconDrawYAdd = 0;

        bool StopDrawMarkupMark = false;
        string StopDrawMarkupMarkNotUnionTab;
        int ZoomScale;

        //ワールドマップID
        uint ConstWorldMapID;

        //Map上でクリックされた時のカスタムイベント
        //user controlの子コントロールからイベントのリレー方法がよくわからん.
        public MouseEventHandler MapMouseDownEvent;
        public MouseEventHandler MapDoubleClickEvent;
 
        public MapPictureBox()
        {
            InitializeComponent();

            this.Disposed += OnDispose;
            //ここで何か追加すると、このuser controlを利用している画面でデザイナーがエラーを吐く.
            //まったくMSのやることは理解できない. 
        }
        private void MapPictureBox_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            this.ZoomComboBox.Text = "100%";
            this.StopDrawMarkupMarkNotUnionTab = "";

            U.ConvertComboBox(MapSettingForm.MakeMapIDList(), ref this.MapSelector);
            //最後にワールドマップを追加.
            this.MapSelector.Items.Add(R._("ワールドマップ"));

            this.ConstWorldMapID = MapSettingForm.GetDataCount();

            this.DefualtIcon = ImageSystemIconForm.YubiTate();
            U.MakeTransparent(this.DefualtIcon);
        }

        int Zoom(int a)
        {
            return a * this.ZoomScale / 100;
        }
        int ZoomCursolXY(int a)
        {
            return a / Zoom(this.ChipSize) * Zoom(this.ChipSize);
        }

        public int CursolToTile(int a)
        {
            return a / Zoom(this.ChipSize);
        }

        //チップセットのサイズ (WM用)
        public void SetChipSize(int size)
        {
            this.ChipSize = size;
        }

        public void LoadMap(uint mapid)
        {
            if (mapid >= this.MapSelector.Items.Count)
            {
                return ;
            }
            U.ForceUpdate(MapSelector, (int)mapid); //確実に変更イベントを発生させる
        }
        public void LoadWorldMap()
        {
            if (this.MapSelector.Items.Count <= 0)
            {
                return;
            }
            U.ForceUpdate(MapSelector, (int)(this.MapSelector.Items.Count - 1) ); //確実に変更イベントを発生させる
        }
        public bool IsMapLoad()
        {
            return this.MapBitMap != null;
        }
        public bool IsFocusedControl()
        {
            return (MapSelector.Focused || ZoomComboBox.Focused || ChangeComboBox.Focused) ;
        }


        private void MapSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)this.MapSelector.SelectedIndex;
            if (mapid < 0)
            {
                return;
            }

            LoadMapLow(mapid);
        }
        public void LoadMapLow(uint mapid)
        {
            //このマップのマップ変化一覧の読込
            ReloadMapchangeList(mapid);

            //マップ変化を変える.
            U.ForceUpdate(this.ChangeComboBox, 0);
        }
        public void SetMapChange(uint mapChangeID)
        {
            U.SelectedIndexSafety(ChangeComboBox, mapChangeID + 1);
        }

        void ReloadMapchangeList(uint mapid)
        {
            this.ChangeComboBox.Items.Clear();
            this.ChangeComboBox.BeginUpdate();

            string name = R._("メインマップ");
            this.ChangeComboBox.Items.Add(name);

            if (! IsWorldmap(mapid))
            {
                //マップ変化リストを作成する.
                List<MapChangeForm.ChangeSt> mapchange = MapChangeForm.MakeChangeList(mapid);
                for (int i = 0; i < mapchange.Count; i++)
                {
                    name = R._("マップ変化:") + U.ToHexString(mapchange[i].no) + InputFormRef.GetCommentSA(mapchange[i].self_change_addr);
                    this.ChangeComboBox.Items.Add(name);
                }
            }
            this.ChangeComboBox.EndUpdate();
        }

        public void LoadMap(Bitmap pic)
        {
            this.CommandBar.Visible = false;
            this.MapBitMap = new Bitmap(pic);
            this.Map.Image = MapBitMap;
        }
        public void HideCommandBar()
        {
            this.CommandBar.Visible = false;
        }
        public void HideCommandBar2()
        {
            this.CommandBar.Visible = false;
            this.Map.Location = new Point(0, 0);
        }

        Point LastMouseCursor = new Point(255,255);
        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            LastMouseCursor.X = e.X;
            LastMouseCursor.Y = e.Y;
            
            this.Map.Invalidate();
        }

        public void setNotifyMode(string name,Func<int,int,int> notifyCallback)
        {
            this.notifyCallback = notifyCallback;
            this.isNotifyMode = true;

            Markup m;
            if (MarkupPointes.TryGetValue(name, out m))
            {
                this.LastMouseCursor.X = m.x * (this.ChipSize);
                this.LastMouseCursor.Y = m.y * (this.ChipSize);
            }
            this.Map.Invalidate();
        }
        public void unsetNotifyMode()
        {
            this.isNotifyMode = false;
        }

        Func<int,int,int> notifyCallback;
        bool   isNotifyMode = false;

        public bool GetNotifyMode()
        {
            return this.isNotifyMode;
        }

        private void Map_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.MapMouseDownEvent != null)
            {
                this.MapMouseDownEvent(sender,e);
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (!isNotifyMode)
            {
                return;
            }
            //通知モードのときはクリックした座標が知りたい
            int x = CursolToTile(e.X);
            int y = CursolToTile(e.Y);

            notifyCallback(x, y);

            this.Map.Invalidate();
        }

        private void Map_Paint(object sender, PaintEventArgs e)
        {
            if (this.MapBitMap == null)
            {
                return;
            }
            e.Graphics.Clear(this.Map.BackColor);

            Bitmap ShowImage = new Bitmap(this.MapBitMap);
            e.Graphics.DrawImage(this.MapBitMap
                , 0, 0
                , Zoom(this.GetMapBitmapWidth())
                , Zoom(this.GetMapBitmapHeight())
                );

            //静的項目の描画
            foreach (var sitem in StaticItems)
            {
                e.Graphics.DrawImage(sitem.Value.bitmap
                    , Zoom(sitem.Value.x + sitem.Value.draw_x_add)
                    , Zoom(sitem.Value.y + sitem.Value.draw_y_add)
                    , Zoom(sitem.Value.bitmap.Width)
                    , Zoom(sitem.Value.bitmap.Height)
                    );
            }

            if (!StopDrawMarkupMark)
            {
                //マークアップ項目の描画
                var sorted = U.OrderBy(MarkupPointes, (x) => x.Value.x + x.Value.y * 2048);
                foreach (var mm in sorted)
                {
                    if (this.StopDrawMarkupMarkNotUnionTab != ""
                        && mm.Key.IndexOf(this.StopDrawMarkupMarkNotUnionTab) != 0)
                    {//UNIONでタブ切替できるときは、現在表示しているタブ以外のオブジェクトを表示しない.
                        continue;
                    }

                    Pen pen;
                    if ((mm.Value.uniqCount & 0x3) == 0)
                    {
                        pen = this.SelectMarkupPen;
                    }
                    else if ((mm.Value.uniqCount & 0x3) == 1)
                    {
                        pen = this.SelectMarkup2Pen;
                    }
                    else
                    {
                        pen = this.SelectMarkup3Pen;
                    }

                    if (mm.Value.is_defualt_icon)
                    {
                        e.Graphics.DrawImage(this.DefualtIcon
                            , Zoom(mm.Value.x + this.DefualtIconDrawXAdd)
                            , Zoom(mm.Value.y + this.DefualtIconDrawYAdd)
                            , Zoom(this.DefualtIcon.Width)
                            , Zoom(this.DefualtIcon.Height)
                            );
                        e.Graphics.DrawRectangle(pen
                            , Zoom(mm.Value.x + this.DefualtIconDrawXAdd)
                            , Zoom(mm.Value.y + this.DefualtIconDrawYAdd)
                            , Zoom(this.DefualtIcon.Width)
                            , Zoom(this.DefualtIcon.Height)
                            );
                    }
                    else
                    {
                        e.Graphics.DrawImage(mm.Value.bitmap
                            , Zoom(mm.Value.x)
                            , Zoom(mm.Value.y + mm.Value.draw_y_add)
                            , Zoom(mm.Value.bitmap.Width)
                            , Zoom(mm.Value.bitmap.Height)
                            );
                        e.Graphics.DrawRectangle(pen
                            , Zoom(mm.Value.x)
                            , Zoom(mm.Value.y + mm.Value.draw_y_add)
                            , Zoom(mm.Value.bitmap.Width)
                            , Zoom(mm.Value.bitmap.Height)
                            );
                    }
                }
            }
            //マウスカーソルの描画
            e.Graphics.DrawRectangle(this.MouseCursorPen
                , ZoomCursolXY((LastMouseCursor.X))
                , ZoomCursolXY((LastMouseCursor.Y))
                , Zoom(this.ChipSize)
                , Zoom(this.ChipSize)
                );
        }
        public class Markup
        {
            public int x;
            public int y;
            public bool is_defualt_icon;
            public Bitmap bitmap;
            public int draw_y_add; //縦に長いオブジェクトの場合の位置合わせ.

            public int uniqCount; //ペンの色を変えるため、個数値を入れる.描画時ソートをするので、何番目かわからなくなるので. 
        };
        Dictionary<string, Markup> MarkupPointes = new Dictionary<string, Markup>();


        public void SetPointIcon(string name, Bitmap bitmap, int draw_y_add = 0)
        {
            Markup m;
            if (MarkupPointes.TryGetValue(name, out m))
            {
                if (bitmap == null)
                {
                    m.is_defualt_icon = true;
                    m.draw_y_add = 0;
                }
                else
                {
                    m.is_defualt_icon = false;
                    m.bitmap = ImageUtil.CloneBitmap(bitmap);
                    U.MakeTransparent(m.bitmap);

                    m.draw_y_add = draw_y_add;
                }
            }
            else
            {
                m = new Markup();
                m.x = 65535; //無効な値を適当に入れる.
                m.y = 65535;
                m.uniqCount = this.MarkupPointes.Count;
                if (bitmap == null)
                {
                    m.is_defualt_icon = true;
                }
                else
                {
                    m.is_defualt_icon = false;
                    m.bitmap = bitmap;
                }
                MarkupPointes[name] = m;
            }
            this.Map.Invalidate();
        }

        public void SetDefualtIcon(Bitmap bitmap, int draw_x_add = 0, int draw_y_add = 0)
        {
            this.DefualtIcon = ImageUtil.CloneBitmap(bitmap);
            U.MakeTransparent(this.DefualtIcon);

            this.DefualtIconDrawXAdd = draw_x_add;
            this.DefualtIconDrawYAdd = draw_y_add;
            this.Map.Invalidate();
        }

        public void SetPoint(string name,int x,int y)
        {
            Markup m;
            if (MarkupPointes.TryGetValue(name, out m))
            {
            }
            else
            {
                m = new Markup();
                m.is_defualt_icon = true;
                m.uniqCount = this.MarkupPointes.Count;
            }
            m.x = x * this.ChipSize; //マップタイルは16x16
            m.y = y * this.ChipSize;
            MarkupPointes[name] = m;

            this.Map.Invalidate();
        }
        public void removePoint(string name)
        {
            MarkupPointes.Remove(name);
            this.Map.Invalidate();
        }
        public void removePoints(string name)
        {
            foreach (string key in MarkupPointes.Keys)
            {
                if (key.IndexOf(name) == 0)
                {
                    MarkupPointes.Remove(name);
                }
            }
            this.Map.Invalidate();
        }
        public void ClearAllPoint()
        {
            MarkupPointes.Clear();
            unsetNotifyMode();
            this.Map.Invalidate();
        }

        public class StaticItem
        {
            public int x;
            public int y;
            public int draw_x_add;
            public int draw_y_add;
            public Bitmap bitmap;
        };
        Dictionary<string, StaticItem> StaticItems = new Dictionary<string, StaticItem>();

        public void SetStaticItem(string name, int x, int y,Bitmap bitmap,int draw_x_add = 0, int draw_y_add = 0)
        {
            StaticItem m;
            if (!StaticItems.TryGetValue(name, out m))
            {
                m = new StaticItem();
            }
            m.x = x * this.ChipSize; //マップタイルは16x16
            m.y = y * this.ChipSize;
            m.draw_x_add = draw_x_add;
            m.draw_y_add = draw_y_add;
            m.bitmap = ImageUtil.CloneBitmap(bitmap);
            U.MakeTransparent(m.bitmap);
            StaticItems[name] = m;
        }
        public void ClearStaticItem()
        {
            StaticItems.Clear();
        }
        public void InvalidateMap()
        {
            this.Map.Invalidate();
        }
        public void SetStopDrawMarkupMark(bool flag)
        {
            this.StopDrawMarkupMark = flag;
            InvalidateMap();
        }
        public void SetStopDrawMarkupMarkNotUnionTab(string name)
        {
            this.StopDrawMarkupMarkNotUnionTab = name;
            InvalidateMap();
        }

        private void ZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ZoomScale = (int)U.atoi(ZoomComboBox.Text);
            if (this.MapBitMap != null)
            {
                this.Map.Width = Zoom(this.GetMapBitmapWidth());
                this.Map.Height = Zoom(this.GetMapBitmapHeight());
            }
            InvalidateMap();
        }
        public uint GetMapID()
        {
            return (uint)this.MapSelector.SelectedIndex;
        }
        public int GetMapBitmapWidth()
        {
            return this.MapBitMap.Width;
        }
        public int GetMapBitmapHeight()
        {
            return this.MapBitMap.Height;
        }

        public Bitmap GetMapTileBitmap(uint x, uint y, uint width = 1, uint heigh = 1)
        {
            return GetMapDotBitmap((int)x * ChipSize, (int)y * ChipSize
                , (int)width * ChipSize, (int)heigh * ChipSize);
        }
        public Bitmap GetMapTileBitmap(int x,int y,int width=1,int heigh = 1)
        {
            return GetMapDotBitmap((int)x * ChipSize, (int)y * ChipSize
                , (int)width * ChipSize, (int)heigh * ChipSize);
        }
        public Bitmap GetMapDotBitmap(uint x, uint y, uint width , uint heigh )
        {
            return GetMapDotBitmap((int)x, (int)y, (int)width, (int)heigh);
        }
        public Bitmap GetMapDotBitmap(int x, int y, int width, int heigh)
        {
            Bitmap bitmap = new Bitmap(width, heigh);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(this.MapBitMap
                , new Rectangle(0, 0, bitmap.Width, bitmap.Height)
                , new Rectangle(x
                    , y
                    , bitmap.Width, bitmap.Height
                    )
                , GraphicsUnit.Pixel
                );
            g.Dispose();

            return bitmap;
        }

        public void SetZoom(int zoom)
        {
            ZoomComboBox.Text = zoom + "%";
        }

        private void Map_DoubleClick(object sender, EventArgs e)
        {
            if (this.MapDoubleClickEvent != null)
            {
                //see https://stackoverflow.com/questions/690904/obtain-position-button-of-mouse-click-on-doubleclick-event
                MouseEventArgs me = e as MouseEventArgs;

                this.MapDoubleClickEvent(sender, me);
            }
        }

        public void UpdateAnime(MapSettingForm.MapAnimations anime)
        {
            this.MapAnime = anime;
            ChangeComboBox_SelectedIndexChanged(null, null);
        }
        MapSettingForm.MapAnimations MapAnime = null;

        public bool IsWorldmap(uint mapid)
        {
            return mapid == this.ConstWorldMapID;
        }

        private void ChangeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)MapSelector.SelectedIndex;
            if (IsWorldmap(mapid))
            {//ワールドマップ
                this.MapBitMap = WorldMapImageForm.DrawWorldMap();
                this.Map.Image = MapBitMap;
            }
            else
            {
                this.MapBitMap = MapSettingForm.DrawMap(mapid, this.MapAnime);
                int change = ChangeComboBox.SelectedIndex;
                if (change >= 1)
                {
                    change = change - 1;
                    List<MapChangeForm.ChangeSt> mapchange = MapChangeForm.MakeChangeList(mapid);
                    if (change < mapchange.Count)
                    {
                        Bitmap c = MapSettingForm.DrawMapChange((uint)mapid
                            , (int)mapchange[change].width
                            , (int)mapchange[change].height
                            , mapchange[change].addr
                            , this.MapAnime);
                        ImageUtil.BitBlt(this.MapBitMap
                            , (int)(mapchange[change].x * 16)
                            , (int)(mapchange[change].y * 16)
                            , c.Width
                            , c.Height
                            , c, 0, 0);
                        c.Dispose();
                    }
                }

                this.Map.Image = MapBitMap;
            }

            ZoomComboBox_SelectedIndexChanged(null, null);
        }
        private void OnDispose(object sender, EventArgs e)
        {
            if (MapBitMap != null)
            {
                MapBitMap.Dispose();
                MapBitMap = null;
            }
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
            if (SelectMarkup2Pen != null)
            {
                SelectMarkup2Pen.Dispose();
                SelectMarkup2Pen = null;
            }
            if (SelectMarkup3Pen != null)
            {
                SelectMarkup3Pen.Dispose();
                SelectMarkup3Pen = null;
            }
            if (DefualtIcon != null)
            {
                DefualtIcon.Dispose();
                DefualtIcon = null;
            }
        }
    }
}
