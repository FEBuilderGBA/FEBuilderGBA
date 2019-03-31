using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace FEBuilderGBA
{
    public partial class MapStyleEditorForm : Form
    {
        Pen MouseCursorPen = new Pen(Color.Red, 1);
        Pen SelectMarkupPen = new Pen(Color.Blue, 3);
        SolidBrush ForeBrush;
        SolidBrush BackBrush;

        List<MapEditorForm.MapEditConfst> MapEditConf;

        public const int PARTS_MAP_PALETTE_COUNT = 5;  //パレットは5種類
        public const int MAX_MAP_PALETTE_COUNT = PARTS_MAP_PALETTE_COUNT * 2; //通常と霧がある

        public MapStyleEditorForm()
        {
            InitializeComponent();

            //マップエディタのマップスタイル
            this.MapEditConf = MapEditorForm.PreLoadResource(U.ConfigDataFilename("mapstyle_"));

            this.ForeBrush = new SolidBrush(this.ForeColor);
            this.BackBrush = new SolidBrush(this.BackColor);
            ObjImportOption.SelectedIndex = 1; //パレットもインポートする
            PaletteTypeCombo.SelectedIndex = 0; //霧なし

            U.SetIcon(MapChipExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(MapChipImportButton, Properties.Resources.icon_upload);
            U.SetIcon(ObjExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ObjImportButton, Properties.Resources.icon_upload);
            U.SetIcon(PaletteExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(PaletteImportButton, Properties.Resources.icon_upload);
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

        private void MapStyleEditorForm_Load(object sender, EventArgs e)
        {
            IsInit = true;

            //マップIDリストを作る.
            List < U.AddrResult > maplist = MapSettingForm.MakeMapIDList();
            //マップスタイルリストを作る
            this.MapEditConf = MapEditorForm.MakeMapStyleList(maplist);
            //マップスタイルリストを作る
            MakeMapStyleCombo();

            U.SelectedIndexSafety(this.MapStyle, 0);

            List<U.AddrResult> terrainList = MapTerrainNameForm.MakeList();
            U.ConvertComboBox(terrainList, ref this.ConfigTerrain);

            //マップオブジェクト
            this.MAP.SetChipSize(8);
            Bitmap black = ImageUtil.BlankDummy();
            U.MakeTransparent(black);
            this.MAP.SetDefualtIcon(black);

            //パレット
            PaletteFormRef.MakePaletteUI(this,OnChangeColor, GetSampleBitmap);
            U.SelectedIndexSafety(this.PaletteCombo, 0);

            //TSA変更のイベント適応.
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("Config_", controls);


            //最大化禁止
            //C#のバグである Anchorを四隅にすると、スクロールバーが消えるというバグに対処するために、
            //フォームを固定化しないとおかしなことになる。
            //クレームは microsoft あたりまでどうぞ.
            this.MaximizeBox = false;
            IsInit = false;

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER , (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ObjImportButton_Click(null, null);
                }
            });
            U.AllowDropFilename(this, new string[] { ".MAPCHIP_CONFIG" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    MapChipImportButton_Click(null, null);
                }
            });
        }

        Bitmap GetSampleBitmap()
        {
            if (this.IsInit)
            {
                return null;
            }
            uint palIndex = CalcPatelleIndex();
            if (palIndex == U.NOT_FOUND)
            {
                return null;
            }
            Bitmap newbitmap = ImageUtil.SwapPalette(this.MapObjImage, (int)palIndex);
            return newbitmap;
        }


        private bool OnChangeColor(Color color, int paletteno)
        {
            if (this.IsInit)
            {
                return true ;
            }
            uint palIndex = CalcPatelleIndex();
            if (palIndex == U.NOT_FOUND)
            {
                return true;
            }

            //色が変化したとき bitmapも変更する.
            ColorPalette palette = this.MapObjImage.Palette; //一度変数に入れないと反映されない.
            palette.Entries[palIndex * 0x10 + paletteno] = color;
            this.MapObjImage.Palette = palette;

            Bitmap newbitmap = ImageUtil.SwapPalette(this.MapObjImage, (int)palIndex);
            this.MAP.LoadMap(newbitmap);

            RedrawMAPCHIPLIST();
            newbitmap.Dispose();

            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, true);
            return true;
        }

        void RedrawMAPCHIPLIST()
        {
            Bitmap newbitmap;
            if (PaletteTypeCombo.SelectedIndex == 1)
            {
                newbitmap = ImageUtil.SwapPalette(this.MapObjImage, (int)PARTS_MAP_PALETTE_COUNT, (int)0x10 * PARTS_MAP_PALETTE_COUNT);
            }
            else
            {
                newbitmap = ImageUtil.SwapPalette(this.MapObjImage, 0, (int)0x10 * PARTS_MAP_PALETTE_COUNT);
            }
            MAPCHIPLIST.Image.Palette = newbitmap.Palette;
            MAPCHIPLIST.Invalidate();

            newbitmap.Dispose();
        }

        //マップチップ
        Bitmap MapObjImage;

        //チップセット(マップチップの画像をどう解釈するか定義するデータ)
        byte[] configUZ;

        int SelectChipset;

        bool IsInit;

        private void MapStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = this.MapStyle.SelectedIndex;
            if (id < 0)
            {
                return;
            }

            ReloadMap_Obj_Config_Palette(
                 this.MapEditConf[id].obj_plist
                , this.MapEditConf[id].palette_plist
                , this.MapEditConf[id].config_plist
                );

            //霧なし.
            PaletteTypeCombo.SelectedIndex = 0;

            //左上のを選択する.
            MAPCHIPLISTMouseCursor.X = 0;
            MAPCHIPLISTMouseCursor.Y = 0;
            MAPCHIPLIST_MouseDown(null, null);

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
            this.configUZ = ImageUtilMap.UnLZ77ChipsetData(config_plist);
            if (this.configUZ == null)
            {
                this.configUZ = new byte[0];
            }

            //Plistアドレスの更新
            Display_Plist(obj_plist, palette_plist, config_plist);

            //チップセットの描画
            Chipset_Update();

            //マップサンプルを出します.
            Bitmap newbitmap = ImageUtil.SwapPalette(this.MapObjImage, (int)0);
            this.MAP.LoadMap(newbitmap);
        }
        //Plistアドレスの更新
        void Display_Plist(uint obj_plist, uint palette_plist, uint config_plist)
        {
            uint config_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CONFIG ,config_plist);
            ChipsetConfigAddress.Value = config_offset;

            uint obj_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.OBJECT, obj_plist & 0xFF);
            ObjAddress.Value = obj_offset;

            uint obj2_plist = (obj_plist >> 8) & 0xFF; //FE8にはないが FE7は、 plistを2つ設定できることがある.
            uint obj2_offset = 0;
            uint palette_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
            PaletteAddress.Value = palette_offset;

            if (obj2_plist > 0)
            {//plist2があれば
                obj2_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.OBJECT , obj2_plist);
                ObjAddress2.Value = obj2_offset;
                ObjAddress2.Visible = true;
            }
            else
            {
                ObjAddress2.Value = 0;
                ObjAddress2.Visible = false;
            }

        }

        void Chipset_Update()
        {
            Bitmap mapObjCels = ImageUtil.Blank(16 * 32, ImageUtilMap.CHIPSET_SEP_BYTE / 16, this.MapObjImage);
            int chip = 0;
            int y = 0;
            while (chip < ImageUtilMap.CHIPSET_SEP_BYTE / 8)
            {
                for (int x = 0; x < 32; x++)
                {
                    ImageUtil.BitBlt(mapObjCels, x * 16, y * 16, 16, 16, ImageUtilMap.DrawOneChipset(chip << 2, this.configUZ, this.MapObjImage), 0, 0);

                    chip++;
                }
                y++;
            }
            MAPCHIPLIST.Image = mapObjCels;
        }

        private void MAPCHIPLIST_Paint(object sender, PaintEventArgs e)
        {
            if (MAPCHIPLISTMouseCursor.X >= 0 && MAPCHIPLISTMouseCursor.Y >= 0)
            {
                //マウスカーソルの描画
                e.Graphics.DrawRectangle(this.MouseCursorPen
                    , MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y, 16, 16);
            }

            //選択しているチップの描画
            Point pt = GetChipListIndexToPoint(this.SelectChipset);
            e.Graphics.DrawRectangle(this.SelectMarkupPen,
                pt.X, pt.Y, 16, 16);

            if (MAPCHIPLISTMouseCursor.X >= 0 && MAPCHIPLISTMouseCursor.Y >= 0)
            {
                int chipset_id = GetMapChipListIndex(MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y);
                DrawMapChipInfo(chipset_id<<2,MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y, e);
            }
        }
        int GetMapChipListIndex(int x,int y)
        {
            return (x/16) +  (y/16 * 32);
        }
        Point GetChipListIndexToPoint(int chipindex)
        {
            Point pt = new Point();
            pt.X = chipindex % (32) * 16;
            pt.Y = chipindex / (32) * 16;
            return pt;
        }

        void DrawMapChipInfo(int chipset_id,int x, int y, PaintEventArgs e)
        {
            PointF pt = new PointF();
            pt.X = x;
            pt.Y = y;

            //このチップセットの名前を問い合わせる.
            uint terrain_data = ImageUtilMap.GetChipsetID(chipset_id,this.configUZ);
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
            windowrc.Height = (int)this.Font.Height*4;

            //枠を描画する幅を特定します.
            SizeF textSize = e.Graphics.MeasureString(terrain_name, this.Font, 1000);
            if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            textSize = e.Graphics.MeasureString(terrain_kaihi, this.Font, 1000);
            if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            textSize = e.Graphics.MeasureString(terrain_kaifuku, this.Font, 1000);
            if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            textSize = e.Graphics.MeasureString(terrain_tuukou, this.Font,1000);
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

            e.Graphics.FillRectangle(this.BackBrush, windowrc);

            e.Graphics.DrawString(terrain_name, this.Font, this.ForeBrush, pt);

            pt.Y = pt.Y + this.Font.Height;
            e.Graphics.DrawString(terrain_kaihi, this.Font, this.ForeBrush, pt);

            pt.Y = pt.Y + this.Font.Height;
            e.Graphics.DrawString(terrain_kaifuku, this.Font, this.ForeBrush, pt);

            pt.Y = pt.Y + this.Font.Height;
            e.Graphics.DrawString(terrain_tuukou, this.Font, this.ForeBrush, pt);
        }

        void SetBadTSA()
        {
            Config_W0.Value = 0;
            Config_W2.Value = 0;
            Config_W4.Value = 0;
            Config_W6.Value = 0;
            return;
        }

        void ToConfigData(int chipset_id)
        {
            //このチップセットの名前を問い合わせる.
            uint terrain_data = ImageUtilMap.GetChipsetID(chipset_id, this.configUZ);
            if (terrain_data == U.NOT_FOUND)
            {
                return;
            }
            if (terrain_data >= this.ConfigTerrain.Items.Count)
            {
                return;
            }

            IsInit = true;

            this.ConfigNo.Text = "0x"+U.ToHexString(chipset_id);
            this.ConfigTerrain.SelectedIndex = (int)terrain_data;

            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= configUZ.Length)
            {//不正なTSA
                SetBadTSA();
                IsInit = false;
                return;
            }
            uint mapwidth8 = (uint)(this.MAP.GetMapBitmapWidth() / 8);
            if (mapwidth8 <= 1)
            {//元となる画像が正しくないのでTSAを描画できません.
                SetBadTSA();
                IsInit = false;

                return;
            }

            Config_W0.Value = (UInt16)U.u16(configUZ, (uint)tile_tsa_index + 0);
            Config_W2.Value = (UInt16)U.u16(configUZ, (uint)tile_tsa_index + 2);
            Config_W4.Value = (UInt16)U.u16(configUZ, (uint)tile_tsa_index + 4);
            Config_W6.Value = (UInt16)U.u16(configUZ, (uint)tile_tsa_index + 6);

            IsInit = false;
            InputFormRef.WriteButtonToYellow(this.ConfigWriteButton, false);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            Config_L_0_TSA_PALETTE_SelectedIndexChanged(Config_L_0_TSA_PALETTE, null);
            DrawConfigPictureBox(chipset_id);
        }

        void DrawConfigPictureBox(int chipset_id)
        {
            ConfigPictureBox.Image = 
                ImageUtilMap.DrawOneChipset(chipset_id, this.configUZ, this.MapObjImage);
        }

        Point MAPCHIPLISTMouseCursor = new Point(-1, -1);
        private void MAPCHIPLIST_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / 16 * 16;
            int y = e.Y / 16 * 16;

            MAPCHIPLISTMouseCursor.X = x;
            MAPCHIPLISTMouseCursor.Y = y;

            this.MAPCHIPLIST.Invalidate();
        }
        private void MAPCHIPLIST_MouseDown(object sender, MouseEventArgs e)
        {
            int chipset_id = GetMapChipListIndex(MAPCHIPLISTMouseCursor.X, MAPCHIPLISTMouseCursor.Y);
            this.SelectChipset = chipset_id;
            ToConfigData(chipset_id << 2);
            DrawConfigPictureBox(chipset_id << 2);
        }


        private void MAPCHIPLIST_MouseLeave(object sender, EventArgs e)
        {
            MAPCHIPLISTMouseCursor.X = -1;
            MAPCHIPLISTMouseCursor.Y = -1;
            this.MAPCHIPLIST.Invalidate();
        }

        uint CalcPatelleIndex()
        {
            if (PaletteCombo.SelectedIndex < 0 || PaletteTypeCombo.SelectedIndex < 0)
            {
                return U.NOT_FOUND;
            }

            uint pal;
            if (PaletteTypeCombo.SelectedIndex == 1)
            {
                pal = (uint)(PaletteCombo.SelectedIndex) + PARTS_MAP_PALETTE_COUNT;
            }
            else
            {
                pal = (uint)PaletteCombo.SelectedIndex;
            }
            Debug.Assert(pal >= 0 && pal < MAX_MAP_PALETTE_COUNT);
            return pal;
        }

        private void PaletteCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint palIndex = CalcPatelleIndex();
            if (palIndex == U.NOT_FOUND)
            {
                return;
            }

            PaletteFormRef.MakePaletteBitmapToUI(this, this.MapObjImage, (int)palIndex);

            Bitmap newbitmap = ImageUtil.SwapPalette(this.MapObjImage, (int)palIndex);
            this.MAP.LoadMap(newbitmap);

            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
        }
        private void PaletteTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint palIndex = CalcPatelleIndex();
            if (palIndex == U.NOT_FOUND)
            {
                return;
            }

            PaletteFormRef.MakePaletteBitmapToUI(this, this.MapObjImage, (int)palIndex);

            Bitmap newbitmap = ImageUtil.SwapPalette(this.MapObjImage, (int)palIndex);
            this.MAP.LoadMap(newbitmap);

            //霧パレットの切り替えは、マップチップも描画しなおす
            RedrawMAPCHIPLIST();

            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
        }




        private void Config_L_0_TSA_PALETTE_SelectedIndexChanged(object sender, EventArgs e)
        {//Config_L_0_TSA_PALETTE - Config_L_3_TSA_PALETTE までの変更
            if (IsInit)
            {
                return;
            }
            if (!(sender is ComboBox))
            {
                return ;
            }
            int select = ((ComboBox)(sender)).SelectedIndex;
            if (select < 0)
            {
                return;
            }
            this.PaletteCombo.SelectedIndex = select;
        }

        private void Config_W0_ValueChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            int chipset_id = this.SelectChipset << 2;
            if ( chipset_id < 0)
            {
                return ;
            }

            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= configUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(configUZ, (uint)tile_tsa_index+0, (uint)Config_W0.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.ConfigWriteButton, true);
        }
        private void Config_W2_ValueChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            int chipset_id = this.SelectChipset << 2;
            if (chipset_id < 0)
            {
                return;
            }

            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= configUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(configUZ, (uint)tile_tsa_index + 2, (uint)Config_W2.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.ConfigWriteButton, true);
        }

        private void Config_W4_ValueChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            int chipset_id = this.SelectChipset << 2;
            if (chipset_id < 0)
            {
                return;
            }

            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= configUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(configUZ, (uint)tile_tsa_index + 4, (uint)Config_W4.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.ConfigWriteButton, true);
        }

        private void Config_W6_ValueChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }
            int chipset_id = this.SelectChipset << 2;
            if (chipset_id < 0)
            {
                return;
            }

            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= configUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(configUZ, (uint)tile_tsa_index + 6, (uint)Config_W6.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.ConfigWriteButton, true);
        }

        private void ConfigTerrain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsInit)
            {
                return;
            }

            int chipset_id = this.SelectChipset << 2;
            if (chipset_id < 0)
            {
                return;
            }

            int terrain_offset = ((chipset_id >> 3) * 2) + ImageUtilMap.CHIPSET_SEP_BYTE;
            if (terrain_offset + 1 >= configUZ.Length)
            {//変なデータ
                return ;
            }

            byte terrain_data = (byte)ConfigTerrain.SelectedIndex;

            //説明は ImageUtilMap.GetChipsetID を見てください.
            if ((chipset_id & 0x4) > 0)
            {
                configUZ[terrain_offset + 1] = terrain_data;
            }
            else
            {
                configUZ[terrain_offset] = terrain_data;
            }
            InputFormRef.WriteButtonToYellow(this.ConfigWriteButton, true);
        }
        public void JumpTo(int select)
        {
            U.SelectedIndexSafety(this.MapStyle,select);
        }
        public void JumpToMAPID(uint mapid)
        {
            MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(mapid);
            for (int i = 0; i < this.MapEditConf.Count; i++)
            {
                if (plist.anime1_plist == this.MapEditConf[i].anime1_plist
                    && plist.anime2_plist == this.MapEditConf[i].anime2_plist
                    && plist.config_plist == this.MapEditConf[i].config_plist
                    && plist.obj_plist == this.MapEditConf[i].obj_plist
                    && plist.palette_plist == this.MapEditConf[i].palette_plist
                    )
                {
                    JumpTo(i);
                    return;
                }
            }
            for (int i = 0; i < this.MapEditConf.Count; i++)
            {
                if (plist.config_plist == this.MapEditConf[i].config_plist
                    && plist.obj_plist == this.MapEditConf[i].obj_plist
                    && plist.palette_plist == this.MapEditConf[i].palette_plist
                    )
                {
                    JumpTo(i);
                    return;
                }
            }
        }
        

        void SelectedChipset_Update()
        {
            int chipset_id = this.SelectChipset << 2;
            if (chipset_id < 0)
            {
                return;
            }
            DrawConfigPictureBox(chipset_id);
        }


        private void ObjExportButton_Click(object sender, EventArgs e)
        {
            Bitmap newbitmap = this.MapObjImage;
            ImageFormRef.ExportImage(this,newbitmap, InputFormRef.MakeSaveImageFilename(this, MapStyle), MAX_MAP_PALETTE_COUNT);
        }

        private void ObjImportButton_Click(object sender, EventArgs e)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return;
            }
            uint obj_plist = this.MapEditConf[this.MapStyle.SelectedIndex].obj_plist;
            uint obj2_plist = (obj_plist >> 8) & 0xFF; //FE8にはないが FE7は、 plistを2つ設定できることがある.

            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 32 * 8;
            int height = 32 * 8;
            int palette_count = MAX_MAP_PALETTE_COUNT;
            if (bitmap.Width != width || bitmap.Height < 128)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width,bitmap.Height,width,height);
                return;
            }
            height = ImageUtil.CalcHeight(width, width * bitmap.Height / 2);

            if (ObjImportOption.SelectedIndex == 1)
            {//パレットもインポートする場合 パレット数のチェック.
                int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
                if (bitmap_palette_count > palette_count)
                {
                    R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                    return;
                }
            }

            //画像
            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            if (obj2_plist > 0)
            {//FE7とかあるフィールド画像分割
                byte[] image1 = U.subrange(image,0,(uint)(image.Length / 2));
                byte[] image2 = U.subrange(image,(uint)(image.Length / 2),(uint)image.Length);
                byte[] image1Z = LZ77.compress(image1);
                byte[] image2Z = LZ77.compress(image2);
                uint newaddr = InputFormRef.WriteBinaryData(this, (uint)ObjAddress.Value, image1Z,InputFormRef.get_data_pos_callback_lz77,undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    return;
                }

                ObjAddress.Value = newaddr;
                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.OBJECT, obj_plist, newaddr, undodata);

                //分割されたデータを書き込み
                newaddr = InputFormRef.WriteBinaryData(this, (uint)ObjAddress2.Value, image2Z, InputFormRef.get_data_pos_callback_lz77, undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    return;
                }
                ObjAddress2.Value = newaddr;

                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.OBJECT, obj2_plist, newaddr, undodata);
                if (!r)
                {
                    Program.Undo.Rollback(undodata);
                    return;
                }

                //書き込んだ通知.
                InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
            }
            else
            {
                byte[] imageZ = LZ77.compress(image);
                uint newaddr = InputFormRef.WriteBinaryData(this, (uint)ObjAddress.Value, imageZ, InputFormRef.get_data_pos_callback_lz77, undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    return;
                }
                ObjAddress.Value = newaddr;

                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.OBJECT, obj_plist, newaddr, undodata);
                if (!r)
                {
                    Program.Undo.Rollback(undodata);
                    return;
                }
                //書き込んだ通知.
                InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
            }


            if (ObjImportOption.SelectedIndex == 1)
            {//パレットもインポートする場合
                //パレットの交換
                MapObjImage = bitmap;
                U.ForceUpdate(this.PaletteTypeCombo, 0);
                U.ForceUpdate(this.PaletteCombo, 0);

                //パレット情報の書き込み.
                uint palette_plist = this.MapEditConf[this.MapStyle.SelectedIndex].palette_plist;
                uint palette_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
                if (palette_address == 0)
                {//未割り当てならば新規確保しようか
                    palette_address = InputFormRef.AppendBinaryData(PaletteFormRef.NewNullPalette(palette_count), undodata);
                }
                PaletteFormRef.MakePaletteBitmapToROM(this, bitmap, palette_address, palette_count,undodata);

                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist, palette_address, undodata);
                if (!r)
                {
                    Program.Undo.Rollback(undodata);
                    return;
                }

                PaletteAddress.Value = palette_address;
            }
            else
            {//パレットはインポートしない場合
                //パレット情報の継承.
                bitmap.Palette = this.MapObjImage.Palette;
                //obj Bitmap交換
                this.MapObjImage = bitmap;
            }

            Program.Undo.Push(undodata);

            //チップセットの更新.
            Chipset_Update();
            SelectedChipset_Update();
            MapStyle_SelectedIndexChanged(sender, e);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
        }

        private void PaletteExportButton_Click(object sender, EventArgs e)
        {
            Bitmap newbitmap = this.MapObjImage;
            ImageFormRef.ExportImage(this,newbitmap, InputFormRef.MakeSaveImageFilename(this, MapStyle), MAX_MAP_PALETTE_COUNT);
        }

        private void PaletteImportButton_Click(object sender, EventArgs e)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return;
            }
            uint palette_plist = this.MapEditConf[this.MapStyle.SelectedIndex].palette_plist;
            uint obj_plist = this.MapEditConf[this.MapStyle.SelectedIndex].obj_plist;

            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }

            bool r = MapPaletteImport(this,bitmap , palette_plist);
            if (!r)
            {
                return;
            }

            //パレットの交換
            this.MapObjImage = ImageUtilMap.DrawMapChipOnly(obj_plist, palette_plist);
            if (this.MapObjImage == null)
            {
                this.MapObjImage = ImageUtil.BlankDummy();
            }
            U.ForceUpdate(this.PaletteCombo, 0);

            //チップセットの更新.
            Chipset_Update();
            SelectedChipset_Update();
            MapStyle_SelectedIndexChanged(sender, e);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);
        }

        public static bool MapPaletteImport(Form self,Bitmap bitmap, uint palette_plist)
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
                    return PaletteImportOne(self, bitmap, palette_plist, false);
                }
                else if (dr == System.Windows.Forms.DialogResult.No)
                {
                    return PaletteImportOne(self, bitmap, palette_plist, true);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //全部のパレットを入れ替える
                return PaletteImportFull(self, bitmap, palette_plist);
            }
        }

        static bool PaletteImportFull(Form self, Bitmap bitmap, uint palette_plist)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(self);

            //パレット情報の書き込み.
            uint palette_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
            if (palette_address == 0)
            {//未割り当てならば新規確保しようか
                palette_address = InputFormRef.AppendBinaryData(PaletteFormRef.NewNullPalette(MAX_MAP_PALETTE_COUNT), undodata);
            }
            PaletteFormRef.MakePaletteBitmapToROM(self, bitmap, palette_address, MAX_MAP_PALETTE_COUNT, undodata);

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
        static bool PaletteImportOne(Form self,Bitmap bitmap, uint palette_plist, bool isFog)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(self);

            //パレット情報の書き込み.
            uint palette_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
            if (palette_address == 0)
            {//未割り当てならば新規確保しようか
                palette_address = InputFormRef.AppendBinaryData(PaletteFormRef.NewNullPalette(MAX_MAP_PALETTE_COUNT), undodata);
            }

            if (isFog)
            {
                PaletteFormRef.MakePaletteBitmapToROM(self, bitmap, palette_address + (0x20 * PARTS_MAP_PALETTE_COUNT), PARTS_MAP_PALETTE_COUNT, undodata);
            }
            else
            {
                PaletteFormRef.MakePaletteBitmapToROM(self, bitmap, palette_address, PARTS_MAP_PALETTE_COUNT, undodata);
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

        private void PaletteWriteButton_Click(object sender, EventArgs e)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return;
            }
            uint palette_plist = this.MapEditConf[this.MapStyle.SelectedIndex].palette_plist;
            int palette_count = MAX_MAP_PALETTE_COUNT;

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            //パレット情報の書き込み.
            uint palette_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
            if (palette_address == 0)
            {//未割り当てならば新規確保しようか
                palette_address = InputFormRef.AppendBinaryData(PaletteFormRef.NewNullPalette(palette_count), undodata);
            }

            PaletteFormRef.MakePaletteColorPaletteToROM(this, this.MapObjImage.Palette, palette_address, palette_count, undodata);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            //拡張領域に書き込んでいる可能性もあるので plstを更新する.
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist, palette_address, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return;
            }

            Program.Undo.Push(undodata);
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return;
            }
            uint config_plist = this.MapEditConf[this.MapStyle.SelectedIndex].config_plist;
            uint chipset_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CONFIG, config_plist);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            byte[] chipsetConfigZ = LZ77.compress(this.configUZ);
            uint newaddr = InputFormRef.WriteBinaryData(this, (uint)ChipsetConfigAddress.Value, chipsetConfigZ, InputFormRef.get_data_pos_callback_lz77, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return;
            }
            ChipsetConfigAddress.Value = newaddr;

            //拡張領域に書き込んでいる可能性もあるので plstを更新する.
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.CONFIG,  config_plist, newaddr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return;
            }
            //書き込んだ通知.
            InputFormRef.ShowWriteNotifyAnimation(this, newaddr);

            Program.Undo.Push(undodata);
            InputFormRef.WriteButtonToYellow(this.ConfigWriteButton, false);
        }

        private void MapChipExportButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("mapchip_config|*.mapchip_config|All files|*");

            uint config_plist = this.MapEditConf[this.MapStyle.SelectedIndex].config_plist;

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, Program.ROM.RomInfo.TitleToFilename() + "_" + config_plist + ".mapchip_config" );

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);
            string filename = save.FileNames[0];

            string ext = U.GetFilenameExt(filename);
            if (ext == ".MAPCHIP_CONFIG")
            {
                File.WriteAllBytes(filename, this.configUZ);
            }
        }

        private void MapChipImportButton_Click(object sender, EventArgs e)
        {
            string title = R._("読み込むファイル名を選択してください");
            string filter = R._("mapchip_config|*.mapchip_config|All files|*");

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                Program.LastSelectedFilename.Load(this, "mapchip_config", open);
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return ;
                }
                if (!U.CanReadFileRetry(open))
                {
                    return ;
                }
                Program.LastSelectedFilename.Save(this, "mapchip_config", open);
                filename = open.FileNames[0];
            }

            if (U.GetFileSize(filename) < 9216)
            {
                R.ShowStopError("ファイルサイズが小さすぎます. 9216バイト必要です。");
                return;
            }

            this.configUZ = File.ReadAllBytes(filename);

            //チップセットの更新.
            Chipset_Update();
            SelectedChipset_Update();

            //書き込み
            ConfigWriteButton.PerformClick();
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



    
    }
}
