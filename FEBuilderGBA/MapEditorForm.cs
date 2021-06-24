using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;

using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class MapEditorForm : Form
    {
        Pen MouseCursorPen = new Pen(Color.Red, 1);
        Pen SelectMarkupPen = new Pen(Color.Blue, 3);
        SolidBrush ForeBrush;
        SolidBrush BackBrush;
        
        public MapEditorForm()
        {
            InitializeComponent();


            this.ForeBrush = new SolidBrush(this.ForeColor);
            this.BackBrush = new SolidBrush(this.BackColor);
            this.IsModified = false;

            U.SetIcon(SaveASbutton, Properties.Resources.icon_arrow);
            U.SetIcon(LoadButton, Properties.Resources.icon_upload);
        }

        public static List<MapEditConfst> MakeMapStyleList(List<U.AddrResult> maplist)
        {
            //マップエディタのマップスタイル
            List < MapEditConfst >  mapeditconf = PreLoadResource(U.ConfigDataFilename("mapstyle_"));
            bool useSecondPalette = PatchUtil.SearchFlag0x28ToMapSecondPalettePatch() != PatchUtil.MapSecondPalette_extends.NO;

            for (int i = 0; i < maplist.Count; i++)
            {
                uint addr = maplist[i].addr;
                MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereAddr(addr);
                int style = FindMapStyle(mapeditconf, plists);
                if (style < 0)
                {
                    MapEditConfst st = new MapEditConfst();
                    st.obj_plist = plists.obj_plist;
                    st.palette_plist = plists.palette_plist;
                    st.config_plist = plists.config_plist;
                    st.anime1_plist = plists.anime1_plist;
                    st.anime2_plist = plists.anime2_plist;
                    st.Name = R._("追加スタイル({0})",MapSettingForm.GetMapNameWhereAddr(addr));
                    mapeditconf.Add(st);
                }

                if (useSecondPalette && plists.palette2_plist > 0)
                {
                    int style2 = FindMapStyle_UsePalette2(mapeditconf, plists);
                    if (style2 < 0)
                    {
                        MapEditConfst st = new MapEditConfst();
                        st.obj_plist = plists.obj_plist;
                        st.palette_plist = plists.palette2_plist;
                        st.config_plist = plists.config_plist;
                        st.anime1_plist = plists.anime1_plist;
                        st.anime2_plist = plists.anime2_plist;
                        st.Name = R._("追加スタイル({0})", MapSettingForm.GetMapNameWhereAddr(addr));
                        mapeditconf.Add(st);
                    }
                }
            }
            return mapeditconf;
        }

        void MakeMapStyleCombo()
        {
            //マップスタイルリストを作る
            this.MapStyle.Items.Clear();
            this.MapStyle.BeginUpdate();
            for (int i = 0; i < MapEditConf.Count; i++)
            {
                this.MapStyle.Items.Add(MapEditConf[i].MakeName());
            }
            this.MapStyle.EndUpdate();
        }

        private void MapEditForm_Load(object sender, EventArgs e)
        {
            //マップIDリストを作る.
            this.MapList = MapSettingForm.MakeMapIDList();
            //マップスタイルリストを作る
            this.MapEditConf = MapEditorForm.MakeMapStyleList(this.MapList);
            MakeMapStyleCombo();

            U.ConvertComboBox(this.MapList, ref  this.MAPCOMBO);
            U.SelectedIndexSafety(this.MAPCOMBO, 0, false);
            U.SelectedIndexSafety(this.Zoom, 0, false);
            U.SelectedIndexSafety(this.TilesetZoom, 0, false);

            this.Width = this.ControlPanel.Width;
            this.Height = this.ControlPanel.Height * 10;
            this.MaximizeBox = true;

            U.AllowDropFilename(this, new string[] { ".TMX", ".MAR", ".MAP" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    LoadButton_Click(null, null);
                }
            });
        }

        //マップリスト
        List<U.AddrResult> MapList;
        List<MapChangeForm.ChangeSt> ChangeList;

        //マップチップ
        Bitmap MapObjImage;
        //チップセット選択
        int[] SelectChipset;

        //チップセット(マップチップの画像をどう解釈するか定義するデータ)
        byte[] ConfigUZ;

        //マップデータ
        UInt16[] MAR;

        //マップの幅と高さ
        int MapWidth;
        int MapHeight;

        bool IsModified;

        bool alertWhenNoSave()
        {
            if (this.IsModified)
            {
                DialogResult dr = R.ShowYesNo("変更をセーブしていませんが、破棄して新しいマップを読み込んでよろしいですか？");
                if(dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return false;
                }
            }

            //変更マークをクリア
            ClearModifiedFlag();
            return true;
        }

        private void MAPCOMBO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MAPCOMBO.SelectedIndex < 0)
            {
                return;
            }

            ClearUndoBuffer();
            uint mapid = (uint)this.MAPCOMBO.SelectedIndex;

            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereMapID(mapid);
            this.MapStyle.SelectedIndex = FindMapStyle(this.MapEditConf, plists);
            if (this.MapStyle.SelectedIndex < 0)
            {//独自形式
                ReloadMap_Obj_Config_Palette(
                      plists.obj_plist
                    , plists.palette_plist
                    , plists.config_plist
                    );
            }


            //マップ配置データの読込
            byte[] mappointerUZ = ImageUtilMap.UnLZ77MapData(plists.mappointer_plist);
            if (mappointerUZ == null)
            {
                mappointerUZ = ErrorMap();
            }
            if (mappointerUZ[0] <= 0 || mappointerUZ[1] <= 0)
            {
                mappointerUZ = ErrorMap();
            }

            this.MapWidth = mappointerUZ[0];
            this.MapHeight = mappointerUZ[1];

            ReloadMapChange(mapid,plists, 0);

            //変更マークをクリア
            ClearModifiedFlag();
        }
        void ReloadMapChange(uint mapid,MapSettingForm.PLists plists, int selected)
        {
            this.ChangeList = MapChangeForm.MakeChangeList(mapid);
            MapChangeForm.ChangeSt p = new MapChangeForm.ChangeSt();
            p.no = U.NOT_FOUND; //マップ本体
            p.x = 0;
            p.y = 0;
            p.width = (uint)this.MapWidth;
            p.height = (uint)this.MapHeight;
            p.addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.MAP, plists.mappointer_plist);
            this.ChangeList.Insert(0, p);

            MapChange.Items.Clear();
            MapChange.BeginUpdate();
            for (int i = 0; i < this.ChangeList.Count; i++)
            {
                string name;
                if (this.ChangeList[i].no == U.NOT_FOUND)
                {
                    name = R._("メインマップ");
                }
                else
                {
                    name = R._("マップ変化ID:") + U.To0xHexString(this.ChangeList[i].no) + InputFormRef.GetCommentSA(this.ChangeList[i].self_change_addr);
                }
                MapChange.Items.Add(name);
            }

            MapChange.EndUpdate();
            if (selected < 0)
            {
                //強制変更のイベントを発生させない
            }
            else
            {
                U.ForceUpdate(MapChange, selected);
            }
        }
        private void MapStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return;
            }
            UpdateMapStyle();
        }
        public void UpdateMapStyle()
        {
            int id = this.MapStyle.SelectedIndex;

            this.BasePictureCache = null;

            ReloadMap_Obj_Config_Palette(
                 MapEditConf[id].obj_plist
                , MapEditConf[id].palette_plist
                , MapEditConf[id].config_plist
                );
            UpdateMapChip();
        }
        void ReloadMap_Obj_Config_Palette(uint obj_plist, uint palette_plist,uint config_plist)
        {
            //マップチップの読込
            this.MapObjImage = ImageUtilMap.DrawMapChipOnly(obj_plist, palette_plist);
            if (this.MapObjImage == null)
            {
                this.MapObjImage = ImageUtil.BlankDummy();
            }
            //チップセットの読込(マップチップの画像をどう解釈するか定義するデータ)
            this.ConfigUZ = ImageUtilMap.UnLZ77ChipsetData(config_plist);
            if (this.ConfigUZ == null)
            {
                this.ConfigUZ = new byte[0];
            }

            //タイルセットの取得
            Bitmap mapchiplist = BuildMapchipSet();

            //拡大処理
            int tilesetzoom = GetTilesetZoom();
            Bitmap zoomPic = new Bitmap(tilesetzoom * mapchiplist.Width, tilesetzoom * mapchiplist.Height);
            using (Graphics g = Graphics.FromImage(zoomPic))
            {
                g.DrawImage(mapchiplist, 0, 0, tilesetzoom * mapchiplist.Width, tilesetzoom * mapchiplist.Height);
            }

            MAPCHIPLIST.Image = zoomPic;
        }

        Bitmap BuildMapchipSet()
        {
            Bitmap mapObjCels = ImageUtil.Blank(16 * 32, ImageUtilMap.CHIPSET_SEP_BYTE / 16, this.MapObjImage);
            int chip = 0;
            int y = 0;
            while (chip < ImageUtilMap.CHIPSET_SEP_BYTE / 8)
            {
                for (int x = 0; x < 32; x++)
                {
                    ImageUtil.BitBlt(mapObjCels, x * 16, y * 16, 16, 16, ImageUtilMap.DrawOneChipset(chip << 2, this.ConfigUZ, this.MapObjImage), 0, 0);

                    chip++;
                }
                y++;
            }
            return mapObjCels;
        }


        private void MapChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapChange.SelectedIndex < 0)
            {
                return;
            }


            MapChangeForm.ChangeSt change = ChangeList[MapChange.SelectedIndex];
            U.ForceUpdate(this.MapAddress, change.addr);
        }
        private void MapAddress_ValueChanged(object sender, EventArgs e)
        {
            if (MapChange.SelectedIndex < 0)
            {
                return;
            }

            MapChangeForm.ChangeSt change = ChangeList[MapChange.SelectedIndex];


            if (change.no == U.NOT_FOUND)
            {//マップ本体
                LoadMapMainData((uint)MapAddress.Value, change);
            }
            else
            {//マップ変化の読込.
                LoadMapChangeData((uint)MapAddress.Value, change);
            }
            //マップを並べる
            UpdateMapChip();
            //書き込みボタンの黄色を消す
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
        }

        static ushort[] LoadMapMainDataLow(uint addr, out int outMapWidth, out int outMapHeight)
        {
            //マップ配置データの読込
            byte[] mappointerUZ = LZ77.decompress(Program.ROM.Data, addr); //tsa
            if (mappointerUZ.Length <= 2)
            {
                R.ShowStopError("マップが読み込めません。エラーマップを表示します。");
                mappointerUZ = ErrorMap();
            }

            int mapWidth = mappointerUZ[0];
            int mapHeight = mappointerUZ[1];

            ushort[] mar = new UInt16[(mapWidth) * (mapHeight)];
            int n = 0;
            int length = mappointerUZ.Length;
            for (int i = 2; i < length; i += 2, n++)
            {
                if (i + 1 >= length)
                {
                    break;
                }
                if (n >= mar.Length)
                {
                    break;
                }

                //マップデータを読む
                int m = (mappointerUZ[i] + ((UInt16)mappointerUZ[i + 1] << 8));
                mar[n] = (UInt16)m;
            }
            outMapWidth = mapWidth;
            outMapHeight = mapHeight;
            return mar;
        }
        void LoadMapMainData(uint addr, MapChangeForm.ChangeSt change)
        {
            //マップ配置データの読込
            this.MAR = LoadMapMainDataLow(addr, out this.MapWidth, out this.MapHeight);

            UpdateMapChip();

            UpdateSizeText(change);
        }
        void LoadMapChangeData(uint addr, MapChangeForm.ChangeSt change)
        {
            this.MapWidth = (int)change.width;
            this.MapHeight = (int)change.height;

            uint rom_length = (uint)Program.ROM.Data.Length;

            this.MAR = new UInt16[(this.MapWidth) * (this.MapHeight)];
            int n = 0;
            for (int i = 0; n < MAR.Length; i += 2, n++)
            {
                //変化 マップデータを読む
                uint p = addr + (uint)i;
                if (p + 2 > rom_length)
                {
                    Log.Error("ROM Broken! MappChange[{2}] is out of range. {0}+2/{1}", U.ToHexString8(addr), U.ToHexString8(rom_length), n.ToString());
                    break;
                }

                uint m = Program.ROM.u16(p);
                this.MAR[n] = (UInt16)m;
            }
            UpdateMapChip();
            UpdateSizeText(change);
        }
        void UpdateSizeText(MapChangeForm.ChangeSt change)
        {
            if (change.no == U.NOT_FOUND)
            {
                MapSizeText.Text = "" + change.width + "x" + change.height;
            }
            else
            {
                MapSizeText.Text = "" + change.x + "X" + change.y + "@" + change.width + "x" + change.height;
            }
        }

        //マップ変化の時に使う下絵のキャッシュ
        Bitmap BasePictureCache;
        Bitmap GetBasePicture(uint mapid, int mapChangeIndex)
        {
            if (this.BasePictureCache != null)
            {
                return this.BasePictureCache;
            }


            Bitmap basemap;
            if (MapStyle.SelectedIndex >= 0)
            {
                basemap = MapSettingForm.DrawMap(mapid
                    , MapEditConf[MapStyle.SelectedIndex].obj_plist
                    , MapEditConf[MapStyle.SelectedIndex].palette_plist
                    , MapEditConf[MapStyle.SelectedIndex].config_plist
                    );
            }
            else
            {
                basemap = MapSettingForm.DrawMap(mapid);
            }

            System.Drawing.Imaging.ColorMatrix cm =
            new System.Drawing.Imaging.ColorMatrix(
                new float[][]{
                    new float[]{0.3086f, 0.3086f, 0.3086f, 0 ,0},
                    new float[]{0.6094f, 0.6094f, 0.6094f, 0, 0},
                    new float[]{0.0820f, 0.0820f, 0.0820f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
            //ImageAttributesオブジェクトの作成
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrixを設定する
            ia.SetColorMatrix(cm);

            this.BasePictureCache = new Bitmap(basemap.Width, basemap.Height);
            using (Graphics g = Graphics.FromImage(this.BasePictureCache))
            {
                //下絵をグレースケールで描画
                g.DrawImage(basemap, new Rectangle(0, 0, basemap.Width, basemap.Height), 0, 0, basemap.Width, basemap.Height, GraphicsUnit.Pixel, ia);
            }
            return this.BasePictureCache;
        }

        void UpdateMapChip()
        {
            if (this.MAR == null || this.MapObjImage == null)
            {
                return;
            }
            if (this.MapWidth <= 0 || this.MapHeight <= 0)
            {
                R.ShowStopError("幅と高さのどちらかがゼロのマップなので、開くことができません。\r\n利用する場合は、「サイズ変更」ボタンを押して適切なサイズに設定してください。");
                return;
            }

            Bitmap map = ImageUtil.Blank((int)this.MapWidth * 16, (int)this.MapHeight * 16, 
this.MapObjImage);
            int x = 0;
            int y = 0;
            for (int i = 0; i < MAR.Length; i++)
            {
                Bitmap chip = ImageUtilMap.DrawOneChipset(MAR[i], this.ConfigUZ, this.MapObjImage);
                ImageUtil.BitBlt(map, x * 16, y * 16, 16, 16, chip, 0, 0);

                x++;
                if (x >= this.MapWidth)
                {
                    x = 0;
                    y++;
                    if (y >= this.MapHeight)
                    {
                        break;
                    }
                }
            }

            int mapChangeIndex = MapChange.SelectedIndex;
            if (mapChangeIndex == 0)
            {//メインだけ
                //拡大処理
                int zoom = GetZoom();
                Bitmap zoomPic = new Bitmap(zoom * map.Width, zoom * map.Height);
                using (Graphics g = Graphics.FromImage(zoomPic))
                {
                    g.DrawImage(map, 0, 0, zoom * map.Width, zoom * map.Height);
                }

                MAP.Image = zoomPic;
            }
            else
            {//部分変更
                //下絵
                uint mapid = (uint)MAPCOMBO.SelectedIndex;
                Bitmap baseimage = GetBasePicture(mapid,mapChangeIndex);

                //拡大処理
                int zoom = GetZoom();
                Bitmap zoomPic = new Bitmap(zoom * baseimage.Width, zoom * baseimage.Height);
                using (Graphics g = Graphics.FromImage(zoomPic))
                {
                    //下絵をグレースケールで描画
                    g.DrawImage(baseimage, 0, 0, zoom * baseimage.Width, zoom * baseimage.Height);

                    //部分変更を指定座標に描画
                    g.DrawImage(map
                        , new Rectangle(
                              (int)(ChangeList[mapChangeIndex].x * 16 * zoom)
                            , (int)(ChangeList[mapChangeIndex].y * 16 * zoom)
                            , map.Width * zoom, map.Height * zoom)
                        , (float)0, (float)0, (float)map.Width, (float)map.Height
                        , GraphicsUnit.Pixel);
                }

                MAP.Image = null;
                MAP.Image = zoomPic;
            }

        }

        private void MAPCHIPLIST_Paint(object sender, PaintEventArgs e)
        {
            int tilesetZoom = GetTilesetZoom();
            if (MAPCHIPLISTMouseCursor.X >= 0 && MAPCHIPLISTMouseCursor.Y >= 0)
            {
                //マウスカーソルの描画
                e.Graphics.DrawRectangle(this.MouseCursorPen
                    , MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y, 16 * tilesetZoom, 16 * tilesetZoom);
            }

            //選択しているチップの描画
            if (this.SelectChipset != null)
            {
                for (int i = 0; i < this.SelectChipset.Length; i++)
                {
                    Point pt = GetChipListIndexToPoint(this.SelectChipset[i], tilesetZoom);
                    e.Graphics.DrawRectangle(this.SelectMarkupPen,
                        pt.X, pt.Y, 16 * tilesetZoom, 16 * tilesetZoom);
                }
            }

            if (MAPCHIPLISTMouseCursor.X >= 0 && MAPCHIPLISTMouseCursor.Y >= 0)
            {
                int chipset_id = GetMapChipListIndex(MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y, tilesetZoom);
                DrawMapChipInfo(chipset_id<<2,MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y, e);
            }
        }
        int GetMapChipListIndex(int x, int y, int tilesetZoom)
        {
            return (x / (16 * tilesetZoom)) + (y / (16 * tilesetZoom) * 32);
        }
        Point GetChipListIndexToPoint(int chipindex,int tilesetZoom)
        {
            Point pt = new Point();
            pt.X = chipindex % (32) * 16 * tilesetZoom;
            pt.Y = chipindex / (32) * 16 * tilesetZoom;
            return pt;
        }
        void DrawMapChipInfo(int chipset_id, int x, int y, PaintEventArgs e)
        {
            DrawMapChipInfoLow(chipset_id, x, y, e, this.ConfigUZ, this.Font, this.ForeBrush, this.BackBrush);
        }

        static public void DrawMapChipInfoLow(int chipset_id, int x, int y, PaintEventArgs e, byte[] configUZ, Font font, SolidBrush foreBrush, SolidBrush backBrush)
        {
            PointF pt = new PointF();
            pt.X = x;
            pt.Y = y;

            //このチップセットの名前を問い合わせる.
            uint terrain_data = ImageUtilMap.GetChipsetID(chipset_id,configUZ);
            if (terrain_data == U.NOT_FOUND)
            {
                return;
            }
            const uint test_class = 1; //主人公クラス

            string terrain_name = MapTerrainNameForm.GetName(terrain_data);
            string terrain_kaihi = R._("地形回避") + ":" + MoveCostForm.GetMoveCost(test_class, terrain_data, 3).ToString(); //地形回避
            string terrain_kaifuku = R._("地形回復") + ":" + MoveCostForm.GetMoveCost(test_class, terrain_data, 6).ToString(); //地形回復

            string terrain_tuukou = R._("地形コスト") + ":";
            uint cost = MoveCostForm.GetMoveCost(test_class, terrain_data, 0);
            terrain_tuukou += cost.ToString();
            if (cost >= 255)
            {
                cost = MoveCostForm.GetMoveCost(ClassForm.GetFlyClass(), terrain_data, 0);
                if (cost < 255)
                {
                    terrain_tuukou += R._("(飛行のみ)");
                }
                else
                {
                    terrain_tuukou += R._("(全員不可)");
                }
            }


            Rectangle windowrc = new Rectangle();
            windowrc.X = (int)pt.X +48+ 1;
            windowrc.Y = (int)pt.Y;
            windowrc.Width = (int)0;
            windowrc.Height = (int)font.Height*4;

            //枠を描画する幅を特定します.
            SizeF textSize = e.Graphics.MeasureString(terrain_name, font, 1000);
            if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            textSize = e.Graphics.MeasureString(terrain_kaihi, font, 1000);
            if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            textSize = e.Graphics.MeasureString(terrain_kaifuku, font, 1000);
            if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            textSize = e.Graphics.MeasureString(terrain_tuukou, font,1000);
            if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;

            if (windowrc.X + windowrc.Width > e.ClipRectangle.Right)
            {//画面右端の場合、左側に押し出す.
                windowrc.X = windowrc.X - windowrc.Width - 32;
                windowrc.Y = windowrc.Y + 16 + 32;
            }
            if (windowrc.Y + windowrc.Height > e.ClipRectangle.Bottom)
            {//画面下端の場合、上側に押し出す.
                windowrc.X = windowrc.X - 16 - 32;
                windowrc.Y = windowrc.Y - windowrc.Height - 32;
            }
            if (windowrc.X < 0)
            {
                windowrc.X = 0;
                while(true)
                {
                    if (pt.X < windowrc.Width && pt.Y >= windowrc.Y && pt.Y <= windowrc.Y + windowrc.Height)
                    {
                        windowrc.Y += 16;
                        continue;
                    }
                    break;
                }
            }
            pt.X = windowrc.X;
            pt.Y = windowrc.Y;

            e.Graphics.FillRectangle(backBrush, windowrc);

            e.Graphics.DrawString(terrain_name, font, foreBrush, pt);

            pt.Y = pt.Y + font.Height;
            e.Graphics.DrawString(terrain_kaihi, font, foreBrush, pt);

            pt.Y = pt.Y + font.Height;
            e.Graphics.DrawString(terrain_kaifuku, font, foreBrush, pt);

            pt.Y = pt.Y + font.Height;
            e.Graphics.DrawString(terrain_tuukou, font, foreBrush, pt);
        }
        string MakeMapChipInfoToString(int chipset_id, int x, int y)
        {
            string text = R._("座標 X:{0} Y:{1}", x, y);

            //このチップセットの名前を問い合わせる.
            uint terrain_data = ImageUtilMap.GetChipsetID(chipset_id, this.ConfigUZ);
            if (terrain_data == U.NOT_FOUND)
            {
                return text;
            }
            const uint test_class = 1; //主人公クラス

            string terrain_name = MapTerrainNameForm.GetName(terrain_data, true);
            string terrain_kaihi = R._("地形回避") + ":" + MoveCostForm.GetMoveCost(test_class, terrain_data, 3).ToString(); //地形回避
            string terrain_kaifuku = R._("地形回復") + ":" + MoveCostForm.GetMoveCost(test_class, terrain_data, 6).ToString(); //地形回復

            string terrain_tuukou = R._("地形コスト") + ":";
            uint cost = MoveCostForm.GetMoveCost(test_class, terrain_data, 0);
            terrain_tuukou += cost.ToString();
            if (cost >= 255)
            {
                cost = MoveCostForm.GetMoveCost(ClassForm.GetFlyClass(), terrain_data, 0);
                if (cost < 255)
                {
                    terrain_tuukou += R._("(飛行のみ)"); ;
                }
                else
                {
                    terrain_tuukou += R._("(全員不可)"); ;
                }
            }

            return text + " " + terrain_name + "(" + U.To0xHexString(terrain_data) + ") " + terrain_kaihi + " " + terrain_kaifuku + " " + terrain_tuukou;
        }

        Point MAPCHIPLISTMouseCursor = new Point(-1, -1);
        private void MAPCHIPLIST_MouseMove(object sender, MouseEventArgs e)
        {
            int tilesetZoom = GetTilesetZoom();
            int x = e.X / (16 * tilesetZoom) * (16 * tilesetZoom);
            int y = e.Y / (16 * tilesetZoom) * (16 * tilesetZoom);

            MAPCHIPLISTMouseCursor.X = x;
            MAPCHIPLISTMouseCursor.Y = y;

            if ((Control.MouseButtons & MouseButtons.Left) > 0
                && SelectChipset != null && SelectChipset.Length >= 1)
            {
                //起点.
                Point chiplistxy = GetChipListIndexToPoint(this.SelectChipset[0],tilesetZoom);
                List<int> newSelectChipset = new List<int>();

                int xx = x;
                int yy = y;
                if (chiplistxy.Y > yy)
                {
                    if (chiplistxy.X > xx)
                    {
                        for (int yi = chiplistxy.Y; yi >= yy; yi--)
                        {
                            for (int xi = chiplistxy.X; xi >= xx; xi--)
                            {
                                newSelectChipset.Add(GetMapChipListIndex(xi, yi, tilesetZoom));
                            }
                        }
                    }
                    else
                    {
                        for (int yi = chiplistxy.Y; yi >= yy; yi--)
                        {
                            for (int xi = chiplistxy.X; xi <= xx; xi++)
                            {
                                newSelectChipset.Add(GetMapChipListIndex(xi, yi, tilesetZoom));
                            }
                        }
                    }
                }
                else
                {
                    if (chiplistxy.X > xx)
                    {
                        for (int yi = chiplistxy.Y; yi <= yy; yi++)
                        {
                            for (int xi = chiplistxy.X; xi >= xx; xi--)
                            {
                                newSelectChipset.Add(GetMapChipListIndex(xi, yi, tilesetZoom));
                            }
                        }
                    }
                    else
                    {
                        for (int yi = chiplistxy.Y; yi <= yy; yi++)
                        {
                            for (int xi = chiplistxy.X; xi <= xx; xi++)
                            {
                                newSelectChipset.Add(GetMapChipListIndex(xi, yi, tilesetZoom));
                            }
                        }
                    }
                }
                this.SelectChipset = newSelectChipset.ToArray();
            }

            this.MAPCHIPLIST.Invalidate();
        }
        private void MAPCHIPLIST_MouseDown(object sender, MouseEventArgs e)
        {
            int tilesetZoom = GetTilesetZoom();

            SelectChipset = new int[1];
            SelectChipset[0] = GetMapChipListIndex(this.MAPCHIPLISTMouseCursor.X, this.MAPCHIPLISTMouseCursor.Y, tilesetZoom);
        }


        private void MAPCHIPLIST_MouseLeave(object sender, EventArgs e)
        {
            MAPCHIPLISTMouseCursor.X = -1;
            MAPCHIPLISTMouseCursor.Y = -1;
            this.MAPCHIPLIST.Invalidate();
        }

        int GetMarChipIndex(int x,int y)
        {
            x = x /16;
            y = y /16;
            if (x < 0 || x >= this.MapWidth)
            {
                return 0;
            }
            if (y < 0 || y >= this.MapHeight)
            {
                return 0;
            }
            int n = x + y * this.MapWidth;
            if (this.MAR == null)
            {
                return 0;
            }
            if (n < 0 || n >= this.MAR.Length)
            {
                return 0;
            }
            return this.MAR[n];
        }
        int GetZoom()
        {
            if (Zoom.SelectedIndex <= 0)
            {
                return 1;
            }
            return Zoom.SelectedIndex + 1;
        }
        int GetTilesetZoom()
        {
            if (TilesetZoom.SelectedIndex <= 0)
            {
                return 1;
            }
            return TilesetZoom.SelectedIndex + 1;
        }

        Point MAPMouseCursor = new Point(-1, -1);
        private void MAP_Paint(object sender, PaintEventArgs e)
        {
            int zoom = GetZoom();
            if (MAPMouseCursor.X >= 0 && MAPMouseCursor.Y >= 0)
            {
                //マウスカーソルの描画
                e.Graphics.DrawRectangle(this.MouseCursorPen
                    , MAPMouseCursor.X , MAPMouseCursor.Y , 16 * zoom, 16 * zoom);
            }
        }
        private void MAP_MouseLeave(object sender, EventArgs e)
        {
            MAPMouseCursor.X = -1;
            MAPMouseCursor.Y = -1;
            this.MAP.Invalidate();
        }

        private void MAP_MouseMove(object sender, MouseEventArgs e)
        {
            int zoom = GetZoom();
            int chipsize = 16 * zoom;

            int x = e.X / chipsize * chipsize;
            int y = e.Y / chipsize * chipsize;

            MAPMouseCursor.X = x;
            MAPMouseCursor.Y = y;

            if ((Control.MouseButtons & MouseButtons.Left) > 0)
            {
                MAP_MouseDown(sender, e);
            }

            int mapchip_id = GetMarChipIndex(x / zoom, y / zoom);
            this.MapChipInfo.Text = MakeMapChipInfoToString(mapchip_id, x / zoom / 16, y / zoom / 16);

            this.MAP.Invalidate();
        }

        void MoveMapChangeMenuAction(uint x,uint y)
        {
            int mapChangeIndex = MapChange.SelectedIndex;
            if (mapChangeIndex <= 0)
            {//メインマップなので変更不可.
                return;
            }
            if (mapChangeIndex >= this.ChangeList.Count)
            {//範囲外
                return;
            }

            MapSizeChange(mapChangeIndex
                , (int)x
                , (int)y
                , (int)0
                , (int)0
                , (int)0
                , (int)0);
        }
        void ResizeDirectMapChangeMenuAction(int direct_w, int direct_h)
        {
            int mapChangeIndex = MapChange.SelectedIndex;
            if (mapChangeIndex <= 0)
            {//メインマップなので変更不可.
                return;
            }
            if (mapChangeIndex >= this.ChangeList.Count)
            {//範囲外
                return;
            }
            int xx = (int)this.ChangeList[mapChangeIndex].x;
            int yy = (int)this.ChangeList[mapChangeIndex].y;
            int w = (int)this.ChangeList[mapChangeIndex].width;
            int h = (int)this.ChangeList[mapChangeIndex].height;
            int right = direct_w - w;
            int bottom = direct_h - h;

            MapSizeChange(mapChangeIndex
                , (int)xx
                , (int)yy
                , (int)0
                , (int)0
                , (int)right
                , (int)bottom);
        }

        void IfMapChangeMouseDown(int mapChangeIndex,MouseEventArgs e)
        {
            if (mapChangeIndex <= 0)
            {//マップ変更ではない
                return;
            }
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
            {//左クリックではない
                return;
            }
            int zoom = GetZoom();
            int chipsize = 16 * zoom;

            int x = (e.X / chipsize);
            int y = (e.Y / chipsize);

            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;

            menuItem = new MenuItem(R._("タイルを選択する"));
            menuItem.Click += new EventHandler((sender, ee) =>
            {
                Spoit_MainMapTile(x, y);
            });
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.MenuItems.Add(new MenuItem("-"));
 
            menuItem = new MenuItem(R._("この位置に移動する"));
            menuItem.Click += new EventHandler((sender, ee) => {
                MoveMapChangeMenuAction((uint)x, (uint)y);
            });
            contextMenu.MenuItems.Add(menuItem);

//            menuItem = new MenuItem(R._("この位置までサイズを拡張する"));
//            menuItem.Click += new EventHandler((sender, ee) =>
//            {
//                ResizeMapChangeMenuAction((uint)x, (uint)y);
//            });
//            contextMenu.MenuItems.Add(menuItem);
            contextMenu.MenuItems.Add(new MenuItem("-"));

            int w = (int)this.ChangeList[mapChangeIndex].width;
            int h = (int)this.ChangeList[mapChangeIndex].height;

            menuItem = new MenuItem(R._("サイズ変更 1*1"));
            menuItem.Click += new EventHandler((sender, ee) =>
            {
                ResizeDirectMapChangeMenuAction(1, 1);
            });
            menuItem.Enabled = (! (w == 1 && w == 1) );
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("サイズ変更 2*2"));
            menuItem.Click += new EventHandler((sender, ee) =>
            {
                ResizeDirectMapChangeMenuAction(2, 2);
            });
            menuItem.Enabled = (!(w == 2 && w == 2));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("サイズ変更 3*3"));
            menuItem.Click += new EventHandler((sender, ee) =>
            {
                ResizeDirectMapChangeMenuAction(3, 3);
            });
            menuItem.Enabled = (!(w == 3 && w == 3));
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(MAP, new Point(e.X, e.Y));
        }

        //メインマップから選択しているタイルをスポイトする。 そのタイルを選択する.
        void Spoit_MainMapTile(int x,int y)
        {
            if (this.ChangeList.Count <= 0)
            {
                Debug.Assert(false);
                return;
            }
            if (x < 0 || y < 0)
            {
                return;
            }
            MapChangeForm.ChangeSt changest = this.ChangeList[0];
            int width,height;
            ushort[] mar = LoadMapMainDataLow(changest.addr, out width, out height);
            int p = x + y * width;
            if (p >= mar.Length)
            {
                return;
            }

            this.SelectChipset = new int[] { mar[p] >> 2 };
            MAPCHIPLIST.Invalidate();
        }

        private void MAP_MouseDown(object sender, MouseEventArgs e)
        {
            int zoom = GetZoom();
            int chipsize = 16 * zoom;

            int x = (e.X / chipsize);
            int y = (e.Y / chipsize);

            int mapChangeIndex = MapChange.SelectedIndex;
            if (mapChangeIndex > 0 && mapChangeIndex < this.ChangeList.Count)
            {//部分変更の場合、位置合わせを行うので、座標補正する
                x = x - (int)ChangeList[mapChangeIndex].x;
                y = y - (int)ChangeList[mapChangeIndex].y;
            }

            if (x < 0 || y < 0 || x >= MapWidth)
            {
                IfMapChangeMouseDown(mapChangeIndex,e);
                return ;
            }

            int p = x + y * MapWidth;
            if (p >= this.MAR.Length)
            {
                IfMapChangeMouseDown(mapChangeIndex, e);
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {//右ボタンを押すと、現在のマップチップを選択する スポイトツール
                this.SelectChipset = new int[]{ this.MAR[p] >> 2 };
                MAPCHIPLIST.Invalidate();
                return;
            }

            //左ボタンなので塗りつぶしモード.
            if (this.SelectChipset == null || this.SelectChipset.Length <= 0)
            {//マップチップが選択されていない.
                return;
            }

            if (this.MAR[p] == (UInt16)(this.SelectChipset[0] << 2))
            {//既に変更済み
                return;
            }

            PushUndo();
            //変更したというマークを出す.
            SetModified();

            //タイルセットの拡大
            int tilesetZoom = GetTilesetZoom();

            //選択しているチップで塗りつぶす.
            this.MAR[p] = (UInt16)(this.SelectChipset[0] << 2);
            Point chiplistxy = GetChipListIndexToPoint(this.SelectChipset[0],tilesetZoom);

            for (int i = 1; i < this.SelectChipset.Length; i++)
            {
                Point chiplistxy_current = GetChipListIndexToPoint(this.SelectChipset[i], tilesetZoom);

                int offsetX = (chiplistxy_current.X - chiplistxy.X) / (16 * tilesetZoom);
                int offsetY = (chiplistxy_current.Y - chiplistxy.Y) / (16 * tilesetZoom);
                int x16 = (x + offsetX);
                int y16 = (y + offsetY);
                if (x16 < 0 || x16 >= MapWidth || y16 < 0 || y16 >= MapHeight)
                {
                    continue;
                }

                p = x16 + y16 * MapWidth;
                if (p < 0 || p >= this.MAR.Length)
                {
                    continue;
                }
                this.MAR[p] = (UInt16)(this.SelectChipset[i]<<2);
            }

            UpdateMapChip();
        }




        private void Zoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMapChip();
            this.MAP.Invalidate();
        }

        private void SaveASbutton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("MapFormat|*.tmx;*.mar;*.map|Tiled|*.tmx|MAR|*.mar|FEMAPCREATOR|*.map|PNG Image|*.png|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);

            //セーブするので変更フラグを下す.
            ClearModifiedFlag();

            string ext = U.GetFilenameExt(save.FileName);
            if (ext == ".MAR")
            {
                SaveAsMAR(save.FileName);
            }
            else if (ext == ".MAP")
            {
                SaveAsMAP(save.FileName);
            }
            else if (ext == ".PNG")
            {
                SaveAsPNG(save.FileName);
            }
            else if (ext == ".TMX")
            {
                if (MapChange.Items.Count >= 2)
                {
                    string message = R.Notify("保存する内容にマップ変化も含めますか？\r\n「はい」の場合は、マップ変化も含めて出力します。\r\n「いいえ」の場合は、現在のマップだけ出力します。");
                    DialogResult dr = MessageBox.Show(message
                        , "?"
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        MapChange.SelectedIndex = 0;  //メインマップを選択
                        SaveAsTMX(save.FileName, true);
                    }
                    else
                    {
                        SaveAsTMX(save.FileName, false);
                    }
                }
                else
                {
                    SaveAsTMX(save.FileName, false);
                }
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(save.FileName);
        }
        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (!alertWhenNoSave())
            {
                return;
            }

            string title = R._("開くマップを選択してください");
            string filter = R._("MapFormat|*.tmx;*.mar;*.map;*.fmp|Tiled|*.tmx|MAR|*.mar|FEMAPCREATOR|*.map|MAPPLY FMP|*.fmp|Import Palette|*.png|All files|*");

            string mapfilename;
            if (ImageFormRef.GetDragFilePath(out mapfilename))
            {
            }
            else
            {
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
                mapfilename = open.FileNames[0];
            }

            string ext = U.GetFilenameExt(mapfilename);
            string errormessage = "";
            if ( ext == ".MAR" )
            {
                errormessage = LoadAsMAR(mapfilename);
            }
            else if (ext == ".MAP")
            {
                errormessage = LoadAsMAP(mapfilename);
            }
            else if (ext == ".FMP")
            {
                errormessage = LoadAsFMP(mapfilename);
            }
            else if (ext == ".PNG")
            {
                errormessage = LoadPaletteAsPNG(mapfilename);
            }
            else if (ext == ".TMX")
            {
                errormessage = LoadAsTMX(mapfilename);
            }

            if (errormessage != "")
            {
                R.ShowStopError(errormessage);
            }
        }

        string LoadAsMAR(string mapfilename)
        {
            byte[] data = File.ReadAllBytes(mapfilename);

            MapEditorMarSizeDialogForm f = (MapEditorMarSizeDialogForm)InputFormRef.JumpFormLow<MapEditorMarSizeDialogForm>();
            f.Init((uint)data.Length,this.ChangeList[this.MapChange.SelectedIndex]);

            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return "";
            }

            int width = (int)f.W.Value;
            int height = data.Length / 2 / width;
            UInt16[] newMAR = new UInt16[width * height];
            for (int i = 0; i < newMAR.Length; i++)
            {
                uint m = U.u16(data,(uint)i*2);
                m = m >> 3;

                newMAR[i] = (UInt16)m;
            }

            PushUndo();

            this.ChangeList[this.MapChange.SelectedIndex].width = (uint)width;
            this.ChangeList[this.MapChange.SelectedIndex].height = (uint)height;
            this.MapWidth = width;
            this.MapHeight = height;
            this.MAR = newMAR;

            UpdateSizeText(this.ChangeList[this.MapChange.SelectedIndex]);
            UpdateMapChip();


            //変更したというマークを出す.
            SetModified();
            return "";
        }

        public static uint FindStyle(List<MapEditConfst> mapceditonf, uint obj_plist,uint palette_plist,uint config_plist)
        {
            for (int i = 0; i < mapceditonf.Count; i++)
            {
                if (mapceditonf[i].obj_plist == obj_plist
                   || mapceditonf[i].palette_plist == palette_plist
                   || mapceditonf[i].config_plist == config_plist
                    )
                {
                    return (uint)i;
                }
            }
            return U.NOT_FOUND;
        }
        //マップスタイルの検索
        static int FindMapStyle(List<MapEditConfst> mapceditonf, MapSettingForm.PLists plists)
        {
            for (int i = 0; i < mapceditonf.Count; i++)
            {
                if (
                    mapceditonf[i].obj_plist == plists.obj_plist
                 && mapceditonf[i].palette_plist == plists.palette_plist
                 && mapceditonf[i].config_plist == plists.config_plist
                 && mapceditonf[i].anime1_plist == plists.anime1_plist
                 && mapceditonf[i].anime2_plist == plists.anime2_plist
                    )
                {
                    return i;
                }
            }
            return -1;
        }

        static int FindMapStyle_UsePalette2(List<MapEditConfst> mapceditonf, MapSettingForm.PLists plists)
        {
            Debug.Assert(plists.palette2_plist > 0);
            for (int i = 0; i < mapceditonf.Count; i++)
            {
                if (
                    mapceditonf[i].obj_plist == plists.obj_plist
                 && mapceditonf[i].palette_plist == plists.palette2_plist
                 && mapceditonf[i].config_plist == plists.config_plist
                 && mapceditonf[i].anime1_plist == plists.anime1_plist
                 && mapceditonf[i].anime2_plist == plists.anime2_plist
                    )
                {
                    return i;
                }
            }
            return -1;
        }

        string LoadPaletteAsPNG(string mapfilename)
        {
            string errormessage;
            Bitmap bitmap = ImageUtil.OpenBitmap(mapfilename, null, out errormessage);
            if (bitmap == null)
            {
                return errormessage;
            }
            uint palette_plist = this.MapEditConf[this.MapStyle.SelectedIndex].palette_plist;
            bool r = MapPaletteImport(bitmap, palette_plist);
            if (!r)
            {
                return "";
            }
            U.ReSelectList(MapStyle);
            UpdateMapChip();
            return "";
        }

        //map形式の読み込み
        string LoadAsMAP(string mapfilename)
        {
            using (StreamReader reader = new StreamReader(mapfilename))
            {
                string line; 
                //最初の一行目からスタイルの検索
                line = reader.ReadLine();
                string[] sp = line.Split('-');
                uint newStyle = (uint)MapStyle.SelectedIndex;
                if (sp.Length >= 3)
                {
                    line = sp[2].Trim();
                    if (line.Length >= 7)
                    {
                        uint obj_plist = U.atoh(line.Substring(0, 4));
                        uint palette_plist = U.atoh(line.Substring(4, 2));
                        uint config_plist = U.atoh(line.Substring(6, 2));
                        uint styleindex = FindStyle(this.MapEditConf, obj_plist, palette_plist, config_plist);
                        if (styleindex != U.NOT_FOUND)
                        {
                            newStyle = styleindex;
                        }
                    }
                }

                //最初の2行目から高さと幅の検索
                line = reader.ReadLine();
                sp = line.Split(' ');
                if (sp.Length <= 1)
                {
                    return R._("マップファイルの幅または高さが正しくありません。\r\n\r\nマップファイル:{0}\r\n最初の行: {1}\r\n", mapfilename, line);
                }
                int height = (int)U.atoi(sp[0]);
                int width = (int)U.atoi(sp[1]);
                if (height <= 0 || width <= 0 || width >= 0xff || height >= 0xff)
                {
                    return R._("マップファイルの幅または高さが正しくありません。\r\n\r\nマップファイル:{0}\r\n幅:{1} 高さ:{2}\r\n", mapfilename, width, height);
                }

                int index = 0;
                UInt16[] newMAR = new UInt16[width * height];
                for (int y = 0; y < height; y++)
                {
                    line = reader.ReadLine();
                    sp = line.Split(' ');
                    for (int x = 0; x < width; x++)
                    {
                        newMAR[index] = (UInt16)(U.atoi(sp[x]) << 2);
                        index++;
                    }
                }

                PushUndo();

                U.SelectedIndexSafety(this.MapStyle, newStyle);
                this.MapWidth = width;
                this.MapHeight = height;
                this.MAR = newMAR;

                UpdateSizeText(this.ChangeList[this.MapChange.SelectedIndex]);
                UpdateMapChip();
            }
            //変更したというマークを出す.
            SetModified();
            return "";
        }

        //fmp形式の読み込み
        string LoadAsFMP(string mapfilename)
        {
            byte[] data = File.ReadAllBytes(mapfilename);
            if (data.Length <= 0x20)
            {
                return R._("ファイルが壊れています。\r\n必須項目がありません。\r\nFile:{0}",mapfilename);
            }
            if (data[0] != 'F'
                || data[1] != 'O'
                || data[2] != 'R'
                || data[3] != 'M'
                || data[8] != 'F'
                || data[9] != 'M'
                || data[10] != 'A'
                || data[11] != 'P'
                || data[12] != 'M'
                || data[13] != 'P'
                || data[14] != 'H'
                || data[15] != 'D'
                )
            {
                return R._("ファイルが壊れています。\r\nヘッダーがありません。\r\nFile:{0}", mapfilename);
            }
            uint chunkSize = U.big32(data,0x10);
            int width = (int)U.big8(data, 0x18);
            int height = (int)U.big8(data, 0x1a);
            if (height <= 0 || width <= 0 || width >= 0xff || height >= 0xff)
            {
                return R._("マップファイルの幅または高さが正しくありません。\r\n\r\nマップファイル:{0}\r\n幅:{1} 高さ:{2}\r\n", mapfilename, width, height);
            }

            bool foundBody = false;
            UInt16[] newMAR = new UInt16[width * height];
            uint addr = 0x14 + chunkSize;
            while(addr + 8 < data.Length)
            {
                chunkSize = U.big32(data, addr + 0x4);
                if (data[addr + 0] == 'B'
                    && data[addr + 1] == 'O'
                    && data[addr + 2] == 'D'
                    && data[addr + 3] == 'Y'
                )
                {
                    int index = 0;
                    uint end = Math.Min(addr + 8 + chunkSize, (uint)data.Length);
                    for (uint i = addr + 8; i < end; i += 2, index ++)
                    {
                        if (index >= newMAR.Length)
                        {
                            break;
                        }
                        uint m = U.u16(data, (uint)i);
                        m = m >> 3;
                        newMAR[index] = (UInt16)m;
                    }
                    foundBody = true;
                    break;
                }

                addr += 8 + chunkSize;
            }

            if (foundBody == false)
            {
                return R._("ファイルが壊れています。\r\nBODYがありません。\r\nFile:{0}", mapfilename);
            }

            PushUndo();

            this.MapWidth = width;
            this.MapHeight = height;
            this.MAR = newMAR;

            UpdateSizeText(this.ChangeList[this.MapChange.SelectedIndex]);
            UpdateMapChip();

            //変更したというマークを出す.
            SetModified();
            return "";
        }
 
        static uint StripStyle(string line)
        {
            System.Text.RegularExpressions.Match match =
                RegexCache.Match(line, "([0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F])");
            if (match.Groups.Count <= 1)
            {//ダメ元でatoh
                return U.atoh(line);
            }
            string v = match.Groups[1].Value;
            return U.atoh(v);
        }

        static uint StripTMXStyle(string line)
        {
            string l = U.cut(line, "name=\"", "\"");
            return StripStyle(l);
        }
#if DEBUG
        public static void TEST_StripTMXStyle_1()
        {
            string l = "<tileset firstgid=\"1\" name=\"FE8 - Village - 0e000f10\" tilewidth=\"16\" tileheight=\"16\">";
            uint r = StripTMXStyle(l);
            Debug.Assert(r == 0x0e000f10);
        }
        public static void TEST_StripTMXStyle_2()
        {
            string l = "<tileset firstgid=\"1\" name=\"0E000F10\" tilewidth=\"16\" tileheight=\"16\">";
            uint r = StripTMXStyle(l);
            Debug.Assert(r == 0x0e000f10);
        }
#endif
        //streamのSeekがなぜかできないので、別処理で調べます。
        //C#のstreamはクソなので使わない方がいい。
        bool HasMainMapByTMX(string mapfilename)
        {
            using (StreamReader reader = new StreamReader(mapfilename))
            {
                string line = U.skipLine(reader, "name=\"Main\"");
                if (line == "")
                {
                    return false;
                }
            }
            return true;
        }
        //何枚のLayerがあるか調べます
        int HowManyLayerByTMX(string mapfilename)
        {
            int count = 0;
            using (StreamReader reader = new StreamReader(mapfilename))
            {
                while (!reader.EndOfStream)
                {
                    string line = U.skipLine(reader, "<layer ");
                    if (line == "")
                    {
                        return count;
                    }
                    count++;
                    if (count >= 255)
                    {
                        break;
                    }
                }
            }
            return count;
        }

        //TMX形式の読み込み
        string LoadAsTMX(string mapfilename)
        {
            int layerCount = HowManyLayerByTMX(mapfilename);
            if (layerCount >= 2)
            {
                string message = R.Notify("マップ変化を含めてインポートしてもよろしいですか？\r\n「はい」の場合は、マップとマップ変化の両方をTMXで指定されたものにすべて置き換えてロードします。\r\n「いいえ」の場合は、メインマップだけをロードします。");
                DialogResult dr = MessageBox.Show(message
                    , "?"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    return LoadAsTMXAllApply(mapfilename);
                }
                else
                {
                    return LoadAsTMXMainMap(mapfilename);
                }
            }
            else if (layerCount == 1)
            {
                return LoadAsTMXMainMap(mapfilename);
            }
            else
            {
                return R._("LAYERの指定がありません。");
            }
        }

        //TMX形式の読み込み メインマップ
        string LoadAsTMXMainMap(string mapfilename)
        {
            using (StreamReader reader = new StreamReader(mapfilename))
            {
                string line;
                line = U.skipLine(reader, "<tileset ");

                uint style = StripTMXStyle(line);
                uint obj_plist = U.ByteSwap16(style / 1000);
                uint palette_plist = (style / 10) & 0xFF;
                uint config_plist = style  & 0xFF;

                uint newStyle = (uint)this.MapStyle.SelectedIndex;
                uint styleindex = FindStyle(this.MapEditConf, obj_plist, palette_plist, config_plist);
                if (styleindex != U.NOT_FOUND)
                {
                    newStyle = styleindex;
                }

                line = U.skipLine(reader, "<layer ");
                if (line.IndexOf("width=") < 0)
                {
                    line = U.skipLine(reader, "width=\"");
                }
                int width  = (int)U.atoi(U.cut(line, "width=\"", "\""));
                int height = (int)U.atoi(U.cut(line, "height=\"", "\""));
                if (width <= 0 || height <= 0 || width >= 0xff || height >= 0xff)
                {
                    return R._("マップファイルの幅または高さが正しくありません。\r\n\r\nマップファイル:{0}\r\n幅:{1} 高さ:{2}\r\n", mapfilename, width, height);
                }

                if (HasMainMapByTMX(mapfilename))
                {//streamのSeekがなぜかできないので、別処理で調べます。
                 //C#のstreamはクソなので使わない方がいい。

                    line = U.skipLine(reader, "name=\"Main\"");
                }

                //マップ本体データの読込
                UInt16[] newMAR = ImportTMXData(reader, (uint)(width * height));
                if (newMAR == null)
                {
                    return R._("マップデータの形式が正しくありません。\r\nTiledで保存するデータ形式をcsvか、tmxディフォルトのxml形式にしてください。");
                }

                PushUndo();

                U.SelectedIndexSafety(this.MapStyle, newStyle);
                this.MapWidth = width;
                this.MapHeight = height;
                this.MAR = newMAR;

                UpdateSizeText(this.ChangeList[this.MapChange.SelectedIndex]);
                UpdateMapChip();
            }
            //変更したというマークを出す.
            SetModified();
            return "";
        }

        class TMXLayer
        {
            public uint id ;
            public uint x ;
            public uint y ;
            public uint width ;
            public uint height ;
            public UInt16[] newMAR;
        };


        //TMX形式の読み込み すべて適応
        string LoadAsTMXAllApply(string mapfilename)
        {
            //レイヤーの個数を調べます.
            List<TMXLayer> layerMapping = new List<TMXLayer>();
            using (StreamReader reader = new StreamReader(mapfilename))
            {
                while(!reader.EndOfStream)
                {
                    string line;
                    line = U.skipLine(reader, "<layer ");
                    line = U.skipLine(reader, "<properties>");

                    TMXLayer layer = new TMXLayer();
                    layer.id = U.NOT_FOUND;
                    layer.x = 0;
                    layer.y = 0;
                    layer.width = 0;
                    layer.height = 0;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();

                        if (line.IndexOf("name=\"Main\"") >= 0)
                        {//メインマップは別途処理するので無視する
                            layer.id = U.NOT_FOUND;
                            break;
                        }
                        if (line.IndexOf("</properties>") >= 0)
                        {
                            break;
                        }

                        string n = U.cut(line, "value=\"", "\"");
                        if (n == "")
                        {//spaceがある変なデータがある.
                            n = U.cut(line, "value =\"", "\"");
                        }
                        if (n == "")
                        {
                            continue;
                        }

                        if (line.IndexOf("name=\"Height\"") >= 0)
                        {
                            layer.height = U.atoi(n);
                        }
                        else if (line.IndexOf("name=\"ID\"") >= 0)
                        {
                            layer.id = U.atoi(n);
                        }
                        else if (line.IndexOf("name=\"Width\"") >= 0)
                        {
                            layer.width = U.atoi(n);
                        }
                        else if (line.IndexOf("name=\"X\"") >= 0)
                        {
                            layer.x = U.atoi(n);
                        }
                        else if (line.IndexOf("name=\"Y\"") >= 0)
                        {
                            layer.y = U.atoi(n);
                        }
                    }
                    if (layer.id == U.NOT_FOUND)
                    {//メインマップなので別処理します.
                        continue;
                    }
                    if (layer.width <= 0 || layer.height <= 0 || layer.width >= 0xff || layer.height >= 0xff)
                    {//必要な情報がとれたか確認.
                        continue;
                    }

                    //マップデータの読込
                    layer.newMAR = ImportTMXData(reader,U.NOT_FOUND);
                    if (layer.newMAR == null)
                    {
                        return R._("マップデータの形式が正しくありません。\r\nTiledで保存するデータ形式をcsvか、tmxディフォルトのxml形式にしてください。");
                    }

                    //読みこんだレイヤーを記録
                    layerMapping.Add(layer);
                }
            }

            //マップスタイルが最初に戻ってしまうので、現在状態を保存する.
            int mapStyle = MapStyle.SelectedIndex;
            //まずメインマップにきりかえ、こいつに書き込みを行います.
            MapChange.SelectedIndex = 0;
            //変化数の弓込
            MAPCOMBO_SelectedIndexChanged(null, null);

            if (layerMapping.Count > this.MapChange.Items.Count)
            {
                //マップ変化数がたりません...なら、増設しよう.
                uint mapid = (uint)this.MAPCOMBO.SelectedIndex;
                if (!CheckExistsMapChangePlist(mapid))
                {
                    return R._("このマップ「{0}」には、マップ変化を格納するPLISTが存在しません。\r\nまず「マップ変化追加」ボタンを押して、PLISTを確保してください。" , U.ToHexString(mapid) );
                }
                bool r = MapChangeForm.ResizeChangeList(mapid, (uint)(layerMapping.Count));
                if (!r)
                {
                    return R._("マップ変化数がたりません。{0}個必要です。インポートする前にマップ変化から確保してください。\r\n\r\nマップファイル:{1}\r\n", layerMapping.Count - 1 , mapfilename);
                }
            }

            //メインマップにきりかえ、こいつに書き込みを行います.
            MapChange.SelectedIndex = 0;
            //マップリストを再選択することで新規に確保した変化データを読み込み
            MAPCOMBO_SelectedIndexChanged(null, null);

            string errormessage = LoadAsTMXMainMap(mapfilename);
            if (errormessage != "")
            {
                return errormessage;
            }

            //メインマップ書き込み
            errormessage = WriteMapData(isShowNotifyMessage: false);
            if (errormessage != "")
            {
                return errormessage;
            }

            int id = 0;
            for (int currentLayer = 0; currentLayer < layerMapping.Count; currentLayer++)
            {
                TMXLayer layer = layerMapping[currentLayer];

                //ID==0 がメインマップ なので、+1します.
                id++;
                if (id >= MapChange.Items.Count)
                {
                    break;
                }

                //指定されたマップ変化に切り替え.
                MapChange.SelectedIndex = (int)id;

                this.MapWidth = (int)layer.width;
                this.MapHeight = (int)layer.height;
                this.MAR = layer.newMAR;
                this.ChangeList[(int)id].height = layer.height;
                this.ChangeList[(int)id].width = layer.width;
                this.ChangeList[(int)id].x = layer.x;
                this.ChangeList[(int)id].y = layer.y;

                //マップ変化の書き込み.
                WriteMapChangeData(isShowNotifyMessage: false);
            }

            //キャッシュを消す
            this.BasePictureCache = null;

            //メインマップに戻す.
            U.ReSelectList(MAPCOMBO);

            //マップスタイルが最初に戻ってしまうので、現在状態に戻す.
            //U.SelectedIndexSafety(MapStyle, mapStyle);

            return "";
        }


        static bool CheckExistsMapChangePlist(uint mapid)
        {
            MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(mapid);
            return plist.mapchange_plist != 0;
        }

        static List<UInt16> ImportTMXDataBase64(StreamReader reader, uint size, string compressMethod)
        {
            List<UInt16> newMAR = new List<UInt16>();
            string base64str = "";
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.IndexOf("</data>") >= 0)
                {
                    break;
                }
                base64str += line.Trim();
            }

            byte[] data = System.Convert.FromBase64String(base64str);
            byte[] raw;
            if (compressMethod.IndexOf("compression=\"zlib\"") >= 0)
            {
                raw = U.ConvertZlibToByte(data);
            }
            else if (compressMethod.IndexOf("compression=\"gzip\"") >= 0)
            {
                raw = U.ConvertGzipToByte(data);
            }
            else
            {//不明
                Debug.Assert(false);
                raw = data;
            }

            for (uint i = 0; i + 4 <= raw.Length; i+= 4 )
            {
                uint m = U.u32(raw, i );
                if (m == 0)
                {
                    continue;
                }
                m = m & 0xffff;
                newMAR.Add((UInt16)((m - 1) << 2));
            }
            return newMAR;
        }
        static List<UInt16> ImportTMXDataCSV(StreamReader reader, uint size)
        {
            List<UInt16> newMAR = new List<UInt16>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.IndexOf("</data>") >= 0)
                {
                    break;
                }
                //,で分割.
                string[] csv = line.Split(',');

                for (int i = 0; i < csv.Length; i++)
                {
                    if (csv[i] == "")
                    {//空データは無視.(両端に現れる.)
                        continue;
                    }

                    uint m = U.atou(csv[i]);
                    if (m == 0)
                    {
                        continue;
                    }
                    m = m & 0xffff;
                    newMAR.Add((UInt16)((m - 1) << 2));
                }
            }
            return newMAR;
        }
        static List<UInt16> ImportTMXDataPlain(StreamReader reader, uint size)
        {
            List<UInt16> newMAR = new List<UInt16>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.IndexOf("</data>") >= 0)
                {
                    break;
                }

                line = U.cut(line, "<tile gid=\"", "\"");
                if (line == "")
                {
                    continue;
                }
                uint m = U.atoi(line);
                if (m == 0)
                {
                    continue;
                }
                m = m & 0xffff;
                newMAR.Add((UInt16)((m - 1) << 2));
            }
            return newMAR;
        }

        static UInt16[] ImportTMXData(StreamReader reader, uint size = U.NOT_FOUND)
        {
            List<UInt16> newMAR = new List<UInt16>();

            string line = U.skipLine(reader, "<data");
            if (line.IndexOf("encoding=\"csv\"") >= 0)
            {//csv format
                newMAR = ImportTMXDataCSV(reader, size);
            }
            else if (line.IndexOf("compression=\"zlib\"") >= 0
                || line.IndexOf("compression=\"gzip\"") >= 0
                || line.IndexOf("encoding=\"base64\"") >= 0
                )
            {//zlib/gzip圧縮
                newMAR = ImportTMXDataBase64(reader, size, line);
            }
            else
            {//tile形式(ディフォルト)
                newMAR = ImportTMXDataPlain(reader, size);
            }

            if (size != U.NOT_FOUND)
            {
                for (uint i = (uint)newMAR.Count; i < size; i++)
                {
                    newMAR.Add(0);
                }
            }

            return newMAR.ToArray();
        }


        //マップチップを画像として保存する.
        void SaveMapObjImage(string mapfilepath , string stylename)
        {
            string path = Path.Combine(Path.GetDirectoryName(mapfilepath), stylename + ".png");

            Bitmap mapchipset =  BuildMapchipSet();
            int count = ImageUtil.GetPalette16Count(mapchipset);
            if (count > 5)
            {//10パレット利用するマップデータ
                ImageUtil.BlackOutUnnecessaryColors(mapchipset, 10);
                U.BitmapSave(mapchipset, path);
                mapchipset.Dispose();
            }
            else
            {//通常の5パレット
                ImageUtil.BlackOutUnnecessaryColors(mapchipset, 5);
                U.BitmapSave(mapchipset, path);
                mapchipset.Dispose();
            }
        }
        


        class UndoData
        {
            public UInt16[] MAR;//UNDO MARはサイズも小さいから、差分よりすべて記録する. 
            public uint x;
            public uint y;
            public uint width;
            public uint height;
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
            p.MAR = (UInt16[])this.MAR.Clone();
            p.x = this.ChangeList[this.MapChange.SelectedIndex].x;
            p.y = this.ChangeList[this.MapChange.SelectedIndex].y;
            p.width = this.ChangeList[this.MapChange.SelectedIndex].width;
            p.height = this.ChangeList[this.MapChange.SelectedIndex].height;

            this.UndoBuffer.Add( p );
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
            this.ChangeList[this.MapChange.SelectedIndex].x = u.x;
            this.ChangeList[this.MapChange.SelectedIndex].y = u.y;
            this.ChangeList[this.MapChange.SelectedIndex].width = u.width;
            this.ChangeList[this.MapChange.SelectedIndex].height = u.height;
            this.MAR = (UInt16[])u.MAR.Clone();

            UpdateSizeText(this.ChangeList[this.MapChange.SelectedIndex]);
            UpdateMapChip();
        }

        //mar形式で保存する.
        void SaveAsMAR(string filename)
        {
            byte[] bin = new byte[this.MAR.Length * 2];
            for (int i = 0; i < this.MAR.Length; i++)
            {
                U.write_u16( bin, (uint)i*2, (uint)(MAR[i] << 3));
            }

            using (FileStream fsStream = new FileStream(filename, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                writer.Write(bin);
            }
        }

        //png形式で保存する
        void SaveAsPNG(string filename)
        {
            Bitmap basemap;

            uint mapid = (uint)this.MAPCOMBO.SelectedIndex;
            if (MapStyle.SelectedIndex >= 0)
            {
                basemap = MapSettingForm.DrawMap(mapid
                    , MapEditConf[MapStyle.SelectedIndex].obj_plist
                    , MapEditConf[MapStyle.SelectedIndex].palette_plist
                    , MapEditConf[MapStyle.SelectedIndex].config_plist
                    );
            }
            else
            {
                basemap = MapSettingForm.DrawMap(mapid);
            }

            int count = ImageUtil.GetPalette16Count(basemap);
            if (count > 5)
            {//10パレット利用するマップデータ
                Bitmap frontBitmap = (Bitmap)basemap.Clone();
                ImageUtil.BlackOutUnnecessaryColors(frontBitmap, 10);
                U.BitmapSave(frontBitmap, filename);
                frontBitmap.Dispose();
            }
            else
            {
                {
                    Bitmap frontBitmap = (Bitmap)basemap.Clone();
                    ImageUtil.BlackOutUnnecessaryColors(frontBitmap, 5);
                    U.BitmapSave(frontBitmap, filename);
                    frontBitmap.Dispose();
                }
                {
                    Bitmap fogBitmap = ImageUtil.SwapPalette(basemap, 5, 0x10 * 5);
                    ImageUtil.BlackOutUnnecessaryColors(fogBitmap, 5);
                    string fogFilename = U.ChangeExtFilename(filename, ".png", "_fog");
                    U.BitmapSave(fogBitmap, fogFilename);
                    fogBitmap.Dispose();
                }
            }
            basemap.Dispose();
        }

        //map形式で保存する.
        void SaveAsMAP(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                string line;
                int index = MapStyle.SelectedIndex;
                string style = "style" + " - " + " "
                    + " - ";

                if (index < 0 || index >= this.MapEditConf.Count)
                {
//                    R.ShowWarning("選択中のマップスタイル{0} は、有効な範囲{1}～{2}を超えています。", index, 0, this.MapEditConf.Count);
                }
                else
                {
                    style = style
                        + MapEditConf[index].obj_plist.ToString("X04")
                        + MapEditConf[index].palette_plist.ToString("X02")
                        + MapEditConf[index].config_plist.ToString("X02")
                        ;
                }
                SaveMapObjImage(filename, style);

                writer.WriteLine(style);

                //幅高さ 高さと幅と入れていく.
                line = MapHeight.ToString() + " " + MapWidth.ToString();
                writer.WriteLine(line);

                //幅分データを10進数でいれる
                uint i = 0;
                for (int y = 0; y < MapHeight; y++)
                {
                    line = "";
                    for (int x = 0; x < MapWidth; x++)
                    {
                        line += " " + (this.MAR[i] >> 2);
                        i++;
                    }

                    line = line.Substring(1);
                    writer.WriteLine(line);
                }
            }
        }

        //tmx形式で保存する.
        void SaveAsTMX(string filename,bool withChange)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                string line;
                line = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                writer.WriteLine(line);

                line = "<map version=\"1.0\" orientation=\"orthogonal\" width=\"" + MapWidth + "\" height=\"" + MapHeight + "\" tilewidth=\"16\" tileheight=\"16\">";
                writer.WriteLine(line);

                int index = MapStyle.SelectedIndex;
                string tileset;
                if (index < 0 || index >= this.MapEditConf.Count)
                {
//                    R.ShowWarning("選択中のマップスタイル{0} は、有効な範囲{1}～{2}を超えています。", index, 0, this.MapEditConf.Count);
                    tileset = "nazo";
                }
                else
                {
                    tileset = U.ByteSwap16(MapEditConf[index].obj_plist).ToString("X04")
                        + MapEditConf[index].palette_plist.ToString("X02")
                        + MapEditConf[index].config_plist.ToString("X02");
                }
                SaveMapObjImage(filename, tileset);

                line = " <tileset firstgid=\"1\" name=\""+tileset+"\" tilewidth=\"16\" tileheight=\"16\">";
                writer.WriteLine(line);
                line = "  <image source=\""+tileset+".png\" trans=\"ffffff\"/>";
                writer.WriteLine(line);
                line = " </tileset>";
                writer.WriteLine(line);

                line = " <layer name=\"Tile Layer 1\" width=\""+MapWidth+"\" height=\""+MapHeight+"\">";
                writer.WriteLine(line);
                line = "  <properties>";
                writer.WriteLine(line);
                line = "   <property name=\"Main\" value =\"\"/>";
                writer.WriteLine(line);
                line = "  </properties>";
                writer.WriteLine(line);

                line = "  <data>";
                writer.WriteLine(line);

                for (int i = 0; i < MAR.Length; i++)
                {
                    line = "  <tile gid=\"" + ((MAR[i] >> 2) +1) + "\"/>";
                    writer.WriteLine(line);
                }

                line = "  </data>";
                writer.WriteLine(line);

                line = " </layer>";
                writer.WriteLine(line);


                if (withChange)
                {
                    for (int n = 1; n < ChangeList.Count; n++)
                    {
                        line = " <layer name=\"Map Change " + (n-1) + "\" width=\""+MapWidth+"\" height=\""+MapHeight+"\">";
                        writer.WriteLine(line);
                        line = "  <properties>";
                        writer.WriteLine(line);
                        line = "   <property name=\"ID\" value =\"" + (n-1) + "\"/>";
                        writer.WriteLine(line);
                        line = "   <property name=\"Height\" value =\"" + ChangeList[n].height + "\"/>";
                        writer.WriteLine(line);
                        line = "   <property name=\"Width\" value =\"" + ChangeList[n].width + "\"/>";
                        writer.WriteLine(line);
                        line = "   <property name=\"X\" value =\"" + ChangeList[n].x + "\"/>";
                        writer.WriteLine(line);
                        line = "   <property name=\"Y\" value =\"" + ChangeList[n].y + "\"/>";
                        writer.WriteLine(line);
                        line = "  </properties>";
                        writer.WriteLine(line);

                        line = "  <data>";
                        writer.WriteLine(line);

                        index = 0;
                        for (int i = 0; i < MAR.Length; i++)
                        {
                            uint m = 0;
                            int x = i % MapWidth;
                            int y = i / MapWidth;

                            if ((x >= ChangeList[n].x && x < ChangeList[n].x + ChangeList[n].width)
                                && (y >= ChangeList[n].y && y < ChangeList[n].y + ChangeList[n].height))
                            {
                                uint tileAddr = ChangeList[n].addr + (uint)index * 2;

                                if (!U.isSafetyOffset(tileAddr + 1))
                                {
                                    R.Error("Out of memory n:{0},i:{1} ,x:{2} ,y:{3} , index:{4} tileAddr:{5}", n, i, x, y, index, U.ToHexString8(tileAddr));
                                    line = "  <tile gid=\"0\"/>";
                                }
                                else
                                {
                                    m = Program.ROM.u16(tileAddr);
                                    index++;
                                    line = "  <tile gid=\"" + ((m >> 2) + 1) + "\"/>";
                                }
                            }
                            else
                            {
                                line = "  <tile gid=\"0\"/>";
                            }

                            writer.WriteLine(line);
                        }

                        line = "  </data>";
                        writer.WriteLine(line);

                        line = " </layer>";
                        writer.WriteLine(line);
                    }
                }
            

                line = "</map>";
                writer.WriteLine(line);
            }
        }


        private void UndoButon_Click(object sender, EventArgs e)
        {
            RunUndo();
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            RunRedo();
        }

        private void SizeChangeButton_Click(object sender, EventArgs e)
        {
            int mapChangeIndex = MapChange.SelectedIndex;
            if (mapChangeIndex < 0)
            {
                return ;
            }

            MapEditorResizeDialogForm f = (MapEditorResizeDialogForm)InputFormRef.JumpFormLow<MapEditorResizeDialogForm>();
            f.Init(this.ChangeList[mapChangeIndex]);

            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            try
            {
                MapSizeChange(mapChangeIndex
                    , (int)f.X.Value
                    , (int)f.Y.Value
                    , (int)f.L.Value
                    , (int)f.T.Value
                    , (int)f.R.Value
                    , (int)f.B.Value);
            }
            catch (Exception ee)
            {
                R.ShowStopError(R.ExceptionToString(ee));
                RunUndo();
            }
        }
        void MapSizeChange(int mapChangeIndex,int xx,int yy,int left,int top,int right,int bottom)
        {
            PushUndo();
            if (xx < 0)
            {
                xx = 0;
            }
            if (yy < 0)
            {
                yy = 0;
            }

            if (mapChangeIndex > 0)
            {//部分変化だった場合、X Yも変更しないといけない.
                this.ChangeList[mapChangeIndex].x = (uint)(xx);
                this.ChangeList[mapChangeIndex].y = (uint)(yy);
            }
            
            if (bottom > 0)
            {//下のラインを増やす.
                int resize = this.MAR.Length + (this.MapWidth * (int)bottom);
                UInt16[] newMAR = new UInt16[resize];
                Array.Copy(this.MAR, newMAR, this.MAR.Length);
                this.MAR = newMAR;
            }
            else if (bottom < 0)
            {//下のラインを減らす.
                int resize = this.MAR.Length - (this.MapWidth * Math.Abs((int)bottom));
                UInt16[] newMAR = new UInt16[resize];
                Array.Copy(this.MAR, newMAR, newMAR.Length);
                this.MAR = newMAR;
            }
            this.MapHeight += (int)bottom;
            this.ChangeList[mapChangeIndex].height = (uint)this.MapHeight;

            if (top > 0)
            {//上のラインを増やす.
                int resize = this.MAR.Length + (this.MapWidth * (int)top);
                UInt16[] newMAR = new UInt16[resize];
                Array.Copy(this.MAR,0, newMAR,(this.MapWidth * (int)top), this.MAR.Length);
                this.MAR = newMAR;
            }
            else if (top < 0)
            {//上のラインを減らす.
                int resize = this.MAR.Length - (this.MapWidth * Math.Abs((int)top));
                UInt16[] newMAR = new UInt16[resize];
                Array.Copy(this.MAR, (this.MapWidth * Math.Abs((int)top)), newMAR, 0, newMAR.Length);
                this.MAR = newMAR;
            }
            this.MapHeight += (int)top;
            this.ChangeList[mapChangeIndex].height = (uint)this.MapHeight;



            if (left > 0)
            {//左のラインを増やす.
                int resize = this.MAR.Length + (this.MapHeight * (int)left);
                int newWidth = this.MapWidth + (int)left;

                UInt16[] newMAR = new UInt16[resize];
                int padding = (int)left;
                for (int y = 0; y < this.MapHeight; y++)
                {
                    for (int x = 0; x < this.MapWidth; x++)
                    {
                        int src = x + (y * this.MapWidth);
                        int dest = x + padding + (y*newWidth);
                        if (src < 0 || src >= this.MAR.Length || dest < 0 || dest >= newMAR.Length)
                        {
                            continue;
                        }
                        newMAR[dest] = this.MAR[src];
                    }
                }
                
                this.MAR = newMAR;
            }
            else if (left < 0)
            {//左のラインを減らす.
                int resize = this.MAR.Length - (this.MapHeight * Math.Abs((int)left));
                int newWidth = this.MapWidth + (int)left;

                UInt16[] newMAR = new UInt16[resize];
                int padding = Math.Abs((int)left);
                for (int y = 0; y < this.MapHeight; y++)
                {
                    for (int x = 0; x < newWidth ; x++)
                    {
                        int src = x + padding + (y * this.MapWidth);
                        int dest = x + (y * newWidth);
                        if (src < 0 || src >= this.MAR.Length || dest < 0 || dest >= newMAR.Length)
                        {
                            continue;
                        }
                        newMAR[dest] = this.MAR[src];
                    }
                }

                this.MAR = newMAR;
            }
            this.MapWidth += (int)left;
            this.ChangeList[mapChangeIndex].width = (uint)this.MapWidth;

            if (right > 0)
            {//右のラインを増やす.
                int resize = this.MAR.Length + (this.MapHeight * (int)right);
                int newWidth = this.MapWidth + (int)right;

                UInt16[] newMAR = new UInt16[resize];
                int padding = (int)right;
                for (int y = 0; y < this.MapHeight; y++)
                {
                    for (int x = 0; x < this.MapWidth; x++)
                    {
                        int src = x + (y * this.MapWidth);
                        int dest = x + (y*newWidth);
                        if (src < 0 || src >= this.MAR.Length || dest < 0 || dest >= newMAR.Length)
                        {
                            continue;
                        }
                        newMAR[dest] = this.MAR[src];
                    }
                }

                this.MAR = newMAR;
            }
            else if (right < 0)
            {//右のラインを減らす.
                int resize = this.MAR.Length - (this.MapHeight * Math.Abs((int)right));
                int newWidth = this.MapWidth + (int)right;

                UInt16[] newMAR = new UInt16[resize];
                int padding = Math.Abs((int)right);
                for (int y = 0; y < this.MapHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        int src = x + (y * this.MapWidth);
                        int dest = x + (y * newWidth);
                        if (src < 0 || src >= this.MAR.Length || dest < 0 || dest >= newMAR.Length)
                        {
                            continue;
                        }
                        newMAR[dest] = this.MAR[src];
                    }
                }

                this.MAR = newMAR;
            }
            this.MapWidth += (int)right;
            this.ChangeList[mapChangeIndex].width = (uint)this.MapWidth;

            UpdateMapChip();
            MAP.Invalidate();

            if (this.ChangeList[mapChangeIndex].no == U.NOT_FOUND)
            {
                UpdateSizeText(this.ChangeList[mapChangeIndex]);
            }
            else
            {
                this.ChangeList[mapChangeIndex].x = (uint)xx;
                this.ChangeList[mapChangeIndex].y = (uint)yy;
                UpdateSizeText(this.ChangeList[mapChangeIndex]);
            }

            //変更したというマークを出す.
            SetModified();
        }
        void SetModified()
        {
            this.IsModified = true;
            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
        }
        void ClearModifiedFlag()
        {
            this.IsModified = false;
            InputFormRef.WriteButtonToYellow(this.WriteButton, false);
        }


        public class MapEditConfst
        {
            public string Name;
            public uint obj_plist;
            public uint palette_plist;
            public uint config_plist;
            public uint anime1_plist;
            public uint anime2_plist;

            public string MakeName()
            {
                string name = this.Name +
                    "("
                    + U.ToHexString(this.obj_plist)
                    + " "
                    + U.ToHexString(this.palette_plist)
                    + " "
                    + U.ToHexString(this.config_plist)
                    + " "
                    + U.ToHexString(this.anime1_plist)
                    + " "
                    + U.ToHexString(this.anime2_plist)
                    + ")"
                    ;
                return name;
            }
        };
        List<MapEditConfst> MapEditConf;

        public static List<MapEditConfst> PreLoadResource(string fullfilename)
        {
            List<MapEditConfst> conf = new List<MapEditConfst>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return conf;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    string[] sp = line.Split('\t');
                    if (sp.Length <= 5)
                    {
                        continue;
                    }

                    MapEditConfst p = new MapEditConfst();
                    p.Name = sp[0];
                    p.obj_plist = U.atoh(sp[1]);
                    p.palette_plist = U.atoh(sp[2]);
                    p.config_plist = U.atoh(sp[3]);
                    p.anime1_plist = U.atoh(sp[4]);
                    p.anime2_plist = U.atoh(sp[5]);

                    conf.Add(p);

                }
            }
            return conf;
        }

        //マップ変化からのジャンプ
        public void JumpTo(uint mapid,uint changeno)
        {
            U.SelectedIndexSafety(MAPCOMBO,mapid);
            if (changeno + 1 >= MapChange.Items.Count)
            {
                return;
            }
            U.ForceUpdate(MapChange, (int)changeno + 1);
        }

        //マップからのジャンプ
        public void JumpTo(uint mapid)
        {
            U.SelectedIndexSafety(MAPCOMBO, mapid);
            if (0 >= MapChange.Items.Count)
            {
                return;
            }
            U.ForceUpdate(MapChange, 0);
        }

        static byte[] ErrorMap()
        {
            byte mini_width = 15;
            byte mini_height = 10;
            byte[] map = new byte[2 + (mini_width * mini_height)] ;
            map[0] = mini_width;
            map[1] = mini_height;

            return map;
        }


        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {
                R.ShowStopError(InputFormRef.GetBusyErrorExplain());
                return;
            }
            if (this.MAR == null)
            {
                return;
            }

            string errormessage;
            if (MapChange.SelectedIndex == 0)
            {//MAIN
                errormessage = WriteMapData(isShowNotifyMessage: true);
                //メインが変わるので、マップ変化で使うグレーのマップキャッシュをクリアする.
                this.BasePictureCache = null;
            }
            else
            {//マップ変化
                errormessage = WriteMapChangeData(isShowNotifyMessage: true);
            }
            if (errormessage != "")
            {
                R.ShowStopError("書き込みに失敗しました。\r\n{0}", errormessage);
                return ;
            }

            //変更マークをクリア
            ClearModifiedFlag();

            if (MapChange.SelectedIndex != 0)
            {
                //マップ変化フォームに変更通知
                NotifyMapChange();
            }
        }
        string WriteMapChangeData(bool isShowNotifyMessage)
        {
            byte[] data = new byte[this.MAR.Length * 2];
            for (int i = 0; i < this.MAR.Length; i++)
            {
                U.write_u16(data, (uint)( (i * 2)), (uint)this.MAR[i]);
            }

            List<Address> mapchange_list = new List<Address>();
            MapChangeForm.MakeAllDataLength(mapchange_list);

            uint addr = (uint)this.MapAddress.Value;
            addr = U.toOffset(addr);

            //複数個所から参照されている場合
            if (MapChangeForm.CountRefence(addr, mapchange_list) >= 2)
            {//使いまわしてはいけない
                addr = 0;
            }

            int mapid = this.MAPCOMBO.SelectedIndex;
            if (mapid < 0)
            {
                return R.Error("MAPIDが0未満です");
            }

            int changeno = MapChange.SelectedIndex;
            if (changeno <= 0)
            {
                return R.Error("MapChangeIDが0以下です");
            }
            if (changeno >= this.ChangeList.Count)
            {
                return R.Error("内部構造に問題があります。this.ChangeList.Count({0}) <= changeno({1}) です", this.ChangeList.Count, changeno);
            }
            MapChangeForm.ChangeSt changest = this.ChangeList[changeno];
            //00 は メインマップとして使っているので、 -1 する.
            changeno = changeno - 1;

            string undo_name = MapSettingForm.GetMapName((uint)mapid);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undo_name);

            //元サイズを計算用にデータを取得.
            MapChangeForm.ChangeSt original = MapChangeForm.MakeChangeOne((uint)mapid, (uint)changeno);
            if (original.self_change_addr == 0)
            {
                Program.Undo.Rollback(undodata);
                return R.Error("original.self_change_addr == 0 と、なりました");
            }

            //変化データの書き込み
            Func<uint, MoveToUnuseSpace.ADDR_AND_LENGTH> get_original_size = (dummy) =>
            {
                MoveToUnuseSpace.ADDR_AND_LENGTH p = new MoveToUnuseSpace.ADDR_AND_LENGTH();
                p.addr = original.addr;
                p.length = original.height * original.width * 2;
                return p;
            };

            //まったく同じマップ変化があるならば共有する
            uint newaddr = MapChangeForm.SearchSameData(data, mapchange_list);
            if (newaddr == U.NOT_FOUND)
            {
                //まったく同じデータがない
                //よって、新たに書き込みます
                newaddr = InputFormRef.WriteBinaryData(this, addr, data, get_original_size, undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    return R.Error("アドレス({0})にデータを書き込めませんでした", addr);
                }
            }

            //マップ変化の本体データへの書き込み アドレス 幅高さ等が変わっている可能性があるので、必ず書き込む.
            changest.addr = newaddr;
            changest.self_change_addr = original.self_change_addr;
            changest.no = original.no;
            if (changest.no != changeno)
            {
                uint new_mapchange_no = MapChangeForm.CheckDuplicateMapChangeID((uint)mapid, changeno);
                if (new_mapchange_no != U.NOT_FOUND)
                {
                    changest.no = new_mapchange_no;
                }
            }
            MapChangeForm.Write_OneData(changest, undodata);

            if (addr != newaddr)
            {//アドレスが異なる場合、拡張領域に書き込んでいる
                this.MapAddress.Value = newaddr;
            }

            Program.Undo.Push(undodata);
            if (isShowNotifyMessage)
            {
                InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
            }

            return "";
        }

        string WriteMapData(bool isShowNotifyMessage)
        {
            byte[] data = new byte[this.MAR.Length * 2 + 2];
            data[0] = (byte)this.MapWidth;
            data[1] = (byte)this.MapHeight;
            for (int i = 0; i < this.MAR.Length; i++)
            {
                U.write_u16(data, (uint)(2 + (i * 2)), (uint)this.MAR[i]);
            }
            byte[] lz77data = LZ77.compress(data);

            uint addr = (uint)this.MapAddress.Value;
            addr = U.toOffset(addr);

            int mapid = this.MAPCOMBO.SelectedIndex;
            if (mapid < 0)
            {
                return R.Error("MAPIDが0未満です");
            }
            string undo_name = MapSettingForm.GetMapName((uint)mapid);
            Undo.UndoData undodata = Program.Undo.NewUndoData(undo_name);

            if (MapStyle.SelectedIndex >= 0)
            {
                MapSettingForm.Write_MapStyle( (uint)mapid
                    , MapEditConf[MapStyle.SelectedIndex].obj_plist
                    , MapEditConf[MapStyle.SelectedIndex].palette_plist
                    , MapEditConf[MapStyle.SelectedIndex].config_plist
                    , MapEditConf[MapStyle.SelectedIndex].anime1_plist
                    , MapEditConf[MapStyle.SelectedIndex].anime2_plist
                    ,undodata);
            }

            uint newaddr = InputFormRef.WriteBinaryData(this ,addr, lz77data, InputFormRef.get_data_pos_callback_lz77, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return R.Error("アドレス({0})にデータを書き込めませんでした", addr);
            }
            //拡張領域に書き込んでいる可能性もあるので plstを更新する.
            MapSettingForm .PLists plist = MapSettingForm.GetMapPListsWhereMapID((uint)mapid);
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.MAP,  plist.mappointer_plist, newaddr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return R.Error("PLIST({0})にアドレス({1})を書き込めませんでした", plist.mappointer_plist, newaddr);
            }
            //マップチェンジのアドレスを更新(これをしないとリロードしたときにおかしくなる)
            this.ChangeList[0].no = U.NOT_FOUND;
            this.ChangeList[0].x = 0;
            this.ChangeList[0].y = 0;
            this.ChangeList[0].addr = newaddr;
            this.ChangeList[0].width = (uint)this.MapWidth;
            this.ChangeList[0].height = (uint)this.MapHeight;

            //マップアドレスの更新.
            this.MapAddress.Value = newaddr;

            Program.Undo.Push(undodata);
            if (isShowNotifyMessage)
            {
                InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
            }
            return "";
        }

        private void StyleChangeButton_Click(object sender, EventArgs e)
        {
            MapStyleEditorForm f = (MapStyleEditorForm)InputFormRef.JumpForm<MapStyleEditorForm>(U.NOT_FOUND);
            f.JumpTo(MapStyle.SelectedIndex);
        }

        private void NewMapChange_Click(object sender, EventArgs e)
        {
            if (!alertWhenNoSave())
            {
                return;
            }
            uint mapid = (uint)this.MAPCOMBO.SelectedIndex;
            if (mapid < 0)
            {
                return;
            }

            MapEditorAddMapChangeDialogForm f = (MapEditorAddMapChangeDialogForm)InputFormRef.JumpFormLow<MapEditorAddMapChangeDialogForm>();
            DialogResult dr = f.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
            {
                //キャンセルされたので、とりあえずメインマップに戻す.
                MapChange.SelectedIndex = 0;
                return;
            }
            if (dr == System.Windows.Forms.DialogResult.Ignore)
            {
                //設定画面に移動
                InputFormRef.JumpForm<MapChangeForm>((uint)MAPCOMBO.SelectedIndex);
                return;
            }

            bool r;
            r = MapChangeForm.PreciseChangeList(mapid);
            if (!r)
            {
                //キャンセルされたので、とりあえずメインマップに戻す.
                MapChange.SelectedIndex = 0;
                return;
            }

            r = MapChangeForm.ResizeChangeList(mapid, (uint)(MapChange.Items.Count));
            if (!r)
            {
                //キャンセルされたので、とりあえずメインマップに戻す.
                MapChange.SelectedIndex = 0;

                R.ShowStopError("マップ変化に割り当てを拒否されました。MAPID:{0}",mapid);
                return;
            }
            MapChange.SelectedIndex = 0; //イベントの関係でいったんメインマップに戻す

            //マップリストを再選択することで新規に確保した変化データを読み込み
            MAPCOMBO_SelectedIndexChanged(null, null);

            //マップ変化リストの再選択
            U.SelectedIndexSafety(MapChange, MapChange.Items.Count - 1);

            //マップ変化フォームに変更通知
            NotifyMapChange();
            return;
        }

        private void MAPCOMBO_Validating(object sender, CancelEventArgs e)
        {
            if (!this.ActiveControl.CausesValidation)
            {
                return;
            }
            if (!alertWhenNoSave())
            {
                e.Cancel = true;
                return;
            }
        }

        private void MapChange_Validating(object sender, CancelEventArgs e)
        {
            if (!this.ActiveControl.CausesValidation)
            {
                return;
            }
            if (!alertWhenNoSave())
            {
                e.Cancel = true;
                return;
            }
        }

        private void MapAddress_Validating(object sender, CancelEventArgs e)
        {
            if (!alertWhenNoSave())
            {
                e.Cancel = true;
                return;
            }
        }

        private void MapEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //変更マークをクリア
            ClearModifiedFlag();
        }

        //C#バグっていて、 form closeでもバリデーションが実行されてしまうので阻止する.
        //http://bbs.wankuma.com/index.cgi?mode=al2&namber=60506&KLOG=101
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && m.WParam.ToInt32() == SC_CLOSE)
            {
                this.AutoValidate = AutoValidate.Disable;
            }

            base.WndProc(ref m);
        }

        private void MapEditorForm_KeyDown(object sender, KeyEventArgs e)
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

        private void MapEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MapObjImage != null)
            {
                MapObjImage.Dispose();
                MapObjImage = null;
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

        private void TilesetZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            MapStyle_SelectedIndexChanged(sender,e);
            this.MAPCHIPLIST.Invalidate();
        }

        public static void UpdateMapStyleIfOpen()
        {
            MapEditorForm f = (MapEditorForm)InputFormRef.GetForm<MapEditorForm>();
            if (f == null)
            {//ウィンドウを開いていない
                return;
            }
            f.UpdateMapStyle();
        }

        //マップタイル変更画面で変更があった場合に呼び出されるイベント
        public void OnUpdateMapChangeForm(uint mapid)
        {
            if (MAPCOMBO.SelectedIndex != mapid)
            {//現在表示しているマップではないので無視する
                return;
            }
            MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereMapID(mapid);
            if (!InputFormRef.IsWriteButtonToYellow(this.WriteButton))
            {//WriteButtonが黄色ではないので、マップ変化をリロードします.
                ClearUndoBuffer();
                ReloadMapChange(mapid, plists, MapChange.SelectedIndex);
                ClearModifiedFlag();
                return;
            }
            //WriteButtonが黄色なので、マップ変化をリロードできません。
            string q = R._("「マップエディタ」で編集中のマップの「タイル変化」データが変更されました。\r\n作成中のマップを無視して、再読み込みしてもよろしいですか？\r\n\r\n「はい」の場合は、再読み込みします。\r\n「いいえ」の場合は、何もしません。自分でマップエディタを閉じて、再読み込みしてください。");
            DialogResult dr = R.ShowYesNo(q);
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            ClearUndoBuffer();
            ReloadMapChange(mapid, plists, MapChange.SelectedIndex);
            ClearModifiedFlag();
        }
        void NotifyMapChange()
        {
            Form f = InputFormRef.GetForm<MapChangeForm>();
            if (f == null)
            {
                return;
            }
            uint mapid = (uint)MAPCOMBO.SelectedIndex;

            MapChangeForm mapchange = (MapChangeForm)f;
            mapchange.OnUpdateMapEditorForm(mapid);
        }

        public const int PARTS_MAP_PALETTE_COUNT = 5;  //パレットは5種類
        public const int MAX_MAP_PALETTE_COUNT = PARTS_MAP_PALETTE_COUNT * 2; //通常と霧がある
        bool MapPaletteImport(Bitmap bitmap, uint palette_plist)
        {
            int palette_count = MAX_MAP_PALETTE_COUNT;
            int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (palette_count <= 1)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return false;
            }

            if (bitmap_palette_count < palette_count)
            {
                DialogResult dr = R.ShowQ("これは晴天時のデータですか？\r\nそれとも霧のデータですか？\r\n\r\n「はい」の場合、晴天時のデータとしてインポートします。\r\n「いいえ」の場合、霧のデータとしてインポートします");
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    return PaletteImportOne(bitmap, palette_plist, false);
                }
                else if (dr == System.Windows.Forms.DialogResult.No)
                {
                    return PaletteImportOne(bitmap, palette_plist, true);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //全部のパレットを入れ替える
                return PaletteImportFull(bitmap, palette_plist);
            }
        }

        bool PaletteImportFull(Bitmap bitmap, uint palette_plist)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            //パレット情報の書き込み.
            uint palette_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
            if (palette_address == 0)
            {//未割り当てならば新規確保しようか
                palette_address = InputFormRef.AppendBinaryData(PaletteFormRef.NewNullPalette(MAX_MAP_PALETTE_COUNT), undodata);
            }

            //拡張領域に書き込んでいる可能性もあるので plstを更新する.
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist, palette_address, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return false;
            }

            Program.Undo.Push(undodata);
            return true;
        }
        bool PaletteImportOne(Bitmap bitmap, uint palette_plist, bool isFog)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            //パレット情報の書き込み.
            uint palette_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
            if (palette_address == 0)
            {//未割り当てならば新規確保しようか
                palette_address = InputFormRef.AppendBinaryData(PaletteFormRef.NewNullPalette(MAX_MAP_PALETTE_COUNT), undodata);
            }

            //拡張領域に書き込んでいる可能性もあるので plstを更新する.
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist, palette_address, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return false;
            }

            Program.Undo.Push(undodata);
            return true;
        }

    }
}
