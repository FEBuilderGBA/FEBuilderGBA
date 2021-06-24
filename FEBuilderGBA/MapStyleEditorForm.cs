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
using System.Text.RegularExpressions;

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
            PaletteTypeCombo.SelectedIndex = 0; //霧なし

            U.SetIcon(MapChipExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(MapChipImportButton, Properties.Resources.icon_upload);
            U.SetIcon(ObjExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ObjImportButton, Properties.Resources.icon_upload);
            U.SetIcon(PaletteExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(PaletteImportButton, Properties.Resources.icon_upload);

            SetExplainText();
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
            this.PFR = new PaletteFormRef(this);
            PFR.MakePaletteUI(OnChangeColor, GetSampleBitmap);
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

        PaletteFormRef PFR;

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
        byte[] ConfigUZ;

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


            //左上のを選択する.
            MAPCHIPLISTMouseCursor.X = 0;
            MAPCHIPLISTMouseCursor.Y = 0;
            MAPCHIPLIST_MouseDown(null, null);

            //霧なしで再描画
            U.ForceUpdate(PaletteTypeCombo, 0);
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
                    ImageUtil.BitBlt(mapObjCels, x * 16, y * 16, 16, 16, ImageUtilMap.DrawOneChipset(chip << 2, this.ConfigUZ, this.MapObjImage), 0, 0);

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
            MapEditorForm.DrawMapChipInfoLow(chipset_id, x, y, e, this.ConfigUZ, this.Font, this.ForeBrush, this.BackBrush);
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
            uint terrain_data = ImageUtilMap.GetChipsetID(chipset_id, this.ConfigUZ);
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
            U.SelectedIndexSafety(this.ConfigTerrain, terrain_data);

            int tile_tsa_index = chipset_id << 1;
            if (tile_tsa_index + 7 >= ConfigUZ.Length)
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

            Config_W0.Value = (UInt16)U.u16(ConfigUZ, (uint)tile_tsa_index + 0);
            Config_W2.Value = (UInt16)U.u16(ConfigUZ, (uint)tile_tsa_index + 2);
            Config_W4.Value = (UInt16)U.u16(ConfigUZ, (uint)tile_tsa_index + 4);
            Config_W6.Value = (UInt16)U.u16(ConfigUZ, (uint)tile_tsa_index + 6);

            IsInit = false;
            InputFormRef.WriteButtonToYellow(this.Config_WriteButton, false);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            Config_L_0_TSA_PALETTE_SelectedIndexChanged(Config_L_0_TSA_PALETTE, null);
            DrawConfigPictureBox(chipset_id);
        }

        void DrawConfigPictureBox(int chipset_id)
        {
            ConfigPictureBox.Image = 
                ImageUtilMap.DrawOneChipset(chipset_id, this.ConfigUZ, this.MapObjImage);
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

            PFR.MakePaletteBitmapToUI(this.MapObjImage, (int)palIndex);
            PFR.ClearUndoBuffer();

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

            PFR.MakePaletteBitmapToUI(this.MapObjImage, (int)palIndex);

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
            if (select >= 5)
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
            if (tile_tsa_index + 7 >= ConfigUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(ConfigUZ, (uint)tile_tsa_index+0, (uint)Config_W0.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.Config_WriteButton, true);
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
            if (tile_tsa_index + 7 >= ConfigUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(ConfigUZ, (uint)tile_tsa_index + 2, (uint)Config_W2.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.Config_WriteButton, true);
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
            if (tile_tsa_index + 7 >= ConfigUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(ConfigUZ, (uint)tile_tsa_index + 4, (uint)Config_W4.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.Config_WriteButton, true);
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
            if (tile_tsa_index + 7 >= ConfigUZ.Length)
            {//不正なTSA
                return;
            }

            U.write_u16(ConfigUZ, (uint)tile_tsa_index + 6, (uint)Config_W6.Value);

            Chipset_Update();
            DrawConfigPictureBox(chipset_id);
            InputFormRef.WriteButtonToYellow(this.Config_WriteButton, true);
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
            if (terrain_offset + 1 >= ConfigUZ.Length)
            {//変なデータ
                return ;
            }

            byte terrain_data = (byte)ConfigTerrain.SelectedIndex;

            //説明は ImageUtilMap.GetChipsetID を見てください.
            if ((chipset_id & 0x4) > 0)
            {
                ConfigUZ[terrain_offset + 1] = terrain_data;
            }
            else
            {
                ConfigUZ[terrain_offset] = terrain_data;
            }
            InputFormRef.WriteButtonToYellow(this.Config_WriteButton, true);
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
            uint palIndex = CalcPatelleIndex();
            if (palIndex == 0 || palIndex == U.NOT_FOUND)
            {//パレットは正しい順場
                Bitmap newbitmap = this.MapObjImage;
                ImageFormRef.ExportImage(this, newbitmap, InputFormRef.MakeSaveImageFilename(this, MapStyle), MAX_MAP_PALETTE_COUNT);
            }
            else
            {//現在選択しているパレット
                Bitmap newbitmap = ImageUtil.SwapPalette(this.MapObjImage, (int)palIndex);
                string name = MapStyle.Text + "_swap_" + palIndex;
                ImageFormRef.ExportImage(this, newbitmap, InputFormRef.MakeSaveImageFilename(this, name), MAX_MAP_PALETTE_COUNT);
            }
        }

        //ファイル名にある _swap_[0-9] にある番号で、パレットを切り替える.
        Bitmap PaletteSwapper(Bitmap bitmap)
        {
            string lastFilename = Program.LastSelectedFilename.Load(this, "");

            Match m = RegexCache.Match(lastFilename, "_swap_([0-9])");
            if (m.Groups.Count < 2)
            {
                return bitmap;
            }
            int palIndex = (int)U.atoh(m.Groups[1].Value);
            if (palIndex <= 0 || palIndex >= 10)
            {
                return bitmap;
            }

            Bitmap newbitmap = ImageUtil.SwapPalette(bitmap, (int)palIndex);
            return newbitmap;
        }

        private void ObjImportButton_Click(object sender, EventArgs e)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return;
            }
            if (MapStyleEditorFormWarningVanillaTileOverraideForm.CheckWarningUI((uint)ObjAddress.Value))
            {
                return;
            }

            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }

            MapStyleEditorImportImageOptionForm f = (MapStyleEditorImportImageOptionForm)InputFormRef.JumpFormLow<MapStyleEditorImportImageOptionForm>();
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            if (f.ImportOption == MapStyleEditorImportImageOptionForm.ImportOptionEnum.WithPalette)
            {
                ImportObj(bitmap, importObjWithPalette: true);
            }
            else if (f.ImportOption == MapStyleEditorImportImageOptionForm.ImportOptionEnum.ImageOnly)
            {
                ImportObj(bitmap, importObjWithPalette: false);
            }
            else if (f.ImportOption == MapStyleEditorImportImageOptionForm.ImportOptionEnum.OnePicture)
            {
                ImportObjOnePicture(bitmap);
            }
        }

        void ImportObjOnePicture(Bitmap loadbitmap)
        {
            const int palette_count = MAX_MAP_PALETTE_COUNT;

            int bitmap_palette_count = ImageUtil.GetPalette16Count(loadbitmap);
            if (bitmap_palette_count > palette_count)
            {
                R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                return;
            }

            if (loadbitmap.Width > 512 || loadbitmap.Height > 512 || loadbitmap.Width % 8 != 0)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2}以下 Height:{3}以下、でなければなりません。\r\nまた、幅は8で割り切れる必要があります。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", loadbitmap.Width, loadbitmap.Height, 512, 512);
                return;
            }   
            //マップチップ用に512x512のキャンバスに再描画
            Bitmap bitmap = ImageUtil.Blank(512, 512, loadbitmap);
            ImageUtil.BitBlt(bitmap, 0, 0, loadbitmap.Width, loadbitmap.Height, loadbitmap, 0, 0);

            byte[] image;
            byte[] tsa;
            string error = ImageUtil.ImageToBytePackedTSA(bitmap, 512, 512, 0, out image, out tsa);
            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }

            if (image.Length > 0x8000)
            {
                R.ShowStopError("マップが広すぎて、0x8000バイトに収まりませんでした。\r\n入力されたサイズ:  {0}\r\n\r\nもっと小さいマップにするか、圧縮率を上げるために共通のパーツを増やしてください。", U.To0xHexString(image.Length) );
                return;
            }

            //写像した画像を再描画
            byte[] palette_bin = ImageUtil.ImageToPalette(bitmap,16);
            bitmap = ImageUtil.ByteToImage16Tile(256, 256, image, 0, palette_bin, 0);

            byte[] map_config = ImageUtilMap.ConvertTSAToMapConfig(tsa);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            bool r = WriteMapChipImage(image, undodata);
            if (!r)
            {
                return;
            }

            r = WriteMapChipPalette(bitmap, true,palette_count, undodata);
            if (!r)
            {
                return;
            }

            r = OverraideMapConfig(map_config, undodata);
            if (!r)
            {
                return;
            }

            Program.Undo.Push(undodata);
            MapStyle_SelectedIndexChanged(null, null);
            return;
        }
        void ImportObj(Bitmap bitmap, bool importObjWithPalette)
        {
            const int palette_count = MAX_MAP_PALETTE_COUNT;
            int width = 32 * 8;
            int height = 32 * 8;
            if (bitmap.Width != width || bitmap.Height < 128)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                return;
            }
            height = ImageUtil.CalcHeight(width, width * bitmap.Height / 2);

            if (importObjWithPalette)
            {//パレットもインポートする場合 パレット数のチェック.
                int bitmap_palette_count = ImageUtil.GetPalette16Count(bitmap);
                if (bitmap_palette_count > palette_count)
                {
                    R.ShowStopError("パレット数が正しくありません。\r\n{1}種類以下(16色*{1}種類) でなければなりません。\r\n\r\n選択された画像のパレット種類:{0}種類", bitmap_palette_count, palette_count);
                    return;
                }

                bitmap = PaletteSwapper(bitmap);
            }

            //画像
            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            bool r = WriteMapChipImage(image, undodata);
            if (!r)
            {
                return;
            }

            r = WriteMapChipPalette(bitmap,importObjWithPalette,palette_count, undodata);
            if (!r)
            {
                return;
            }

            Program.Undo.Push(undodata);

            //チップセットの更新.
            Chipset_Update();
            SelectedChipset_Update();
            MapStyle_SelectedIndexChanged(null,null);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            //マップエディタが開いていれば更新する
            MapEditorForm.UpdateMapStyleIfOpen();
        }

        bool WriteMapChipImage(byte[] image, Undo.UndoData undodata)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return false;
            }
            uint obj_plist = this.MapEditConf[this.MapStyle.SelectedIndex].obj_plist;
            uint obj2_plist = (obj_plist >> 8) & 0xFF; //FE8にはないが FE7は、 plistを2つ設定できることがある.

            if (obj2_plist > 0)
            {//FE7とかあるフィールド画像分割
                byte[] image1 = U.subrange(image, 0, (uint)(image.Length / 2));
                byte[] image2 = U.subrange(image, (uint)(image.Length / 2), (uint)image.Length);
                byte[] image1Z = LZ77.compress(image1);
                byte[] image2Z = LZ77.compress(image2);
                uint newaddr = InputFormRef.WriteBinaryData(this, (uint)ObjAddress.Value, image1Z, InputFormRef.get_data_pos_callback_lz77, undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    return false;
                }

                ObjAddress.Value = newaddr;
                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.OBJECT, obj_plist, newaddr, undodata);

                //分割されたデータを書き込み
                newaddr = InputFormRef.WriteBinaryData(this, (uint)ObjAddress2.Value, image2Z, InputFormRef.get_data_pos_callback_lz77, undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    Program.Undo.Rollback(undodata);
                    return false;
                }
                ObjAddress2.Value = newaddr;

                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.OBJECT, obj2_plist, newaddr, undodata);
                if (!r)
                {
                    Program.Undo.Rollback(undodata);
                    return false;
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
                    return false;
                }
                ObjAddress.Value = newaddr;

                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.OBJECT, obj_plist, newaddr, undodata);
                if (!r)
                {
                    Program.Undo.Rollback(undodata);
                    return false;
                }
                //書き込んだ通知.
                InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
            }
            return true;
        }
        bool WriteMapChipPalette(Bitmap bitmap,bool importObjWithPalette,int palette_count, Undo.UndoData undodata)
        {
            if (importObjWithPalette)
            {//パレットもインポートする場合
                //パレットの交換
                this.MapObjImage = bitmap;
                U.ForceUpdate(this.PaletteTypeCombo, 0);
                U.ForceUpdate(this.PaletteCombo, 0);

                //パレット情報の書き込み.
                uint palette_plist = this.MapEditConf[this.MapStyle.SelectedIndex].palette_plist;
                uint palette_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist);
                if (palette_address == 0)
                {//未割り当てならば新規確保しようか
                    palette_address = InputFormRef.AppendBinaryData(PaletteFormRef.NewNullPalette(palette_count), undodata);
                }
                PFR.MakePaletteBitmapToROM(bitmap, palette_address, palette_count, undodata);

                //拡張領域に書き込んでいる可能性もあるので plstを更新する.
                bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist, palette_address, undodata);
                if (!r)
                {
                    Program.Undo.Rollback(undodata);
                    return false;
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

            return true;
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

            bool r = MapPaletteImport(bitmap , palette_plist);
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

            //マップエディタが開いていれば更新する
            MapEditorForm.UpdateMapStyleIfOpen();
        }

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
            PFR.MakePaletteBitmapToROM(bitmap, palette_address, MAX_MAP_PALETTE_COUNT, undodata);

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

            if (isFog)
            {
                PFR.MakePaletteBitmapToROM(bitmap, palette_address + (0x20 * PARTS_MAP_PALETTE_COUNT), PARTS_MAP_PALETTE_COUNT, undodata);
            }
            else
            {
                PFR.MakePaletteBitmapToROM(bitmap, palette_address, PARTS_MAP_PALETTE_COUNT, undodata);
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

            PFR.MakePaletteColorPaletteToROM(this.MapObjImage.Palette, palette_address, palette_count, undodata);
            InputFormRef.WriteButtonToYellow(this.PaletteWriteButton, false);

            //拡張領域に書き込んでいる可能性もあるので plstを更新する.
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.PALETTE, palette_plist, palette_address, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return;
            }

            Program.Undo.Push(undodata);

            //マップエディタが開いていれば更新する
            MapEditorForm.UpdateMapStyleIfOpen();
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            bool r = WriteMapConfig(undodata);
            if (!r)
            {
                return;
            }

            Program.Undo.Push(undodata);
        }
        bool WriteMapConfig(Undo.UndoData undodata)
        {
            if (this.MapStyle.SelectedIndex < 0)
            {
                return false;
            }
            uint config_plist = this.MapEditConf[this.MapStyle.SelectedIndex].config_plist;
            uint chipset_address = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CONFIG, config_plist);

            byte[] chipsetConfigZ = LZ77.compress(this.ConfigUZ);
            uint newaddr = InputFormRef.WriteBinaryData(this, (uint)ChipsetConfigAddress.Value, chipsetConfigZ, InputFormRef.get_data_pos_callback_lz77, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return false;
            }
            ChipsetConfigAddress.Value = newaddr;

            //拡張領域に書き込んでいる可能性もあるので plstを更新する.
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.CONFIG, config_plist, newaddr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return false;
            }

            //書き込んだ通知.
            InputFormRef.ShowWriteNotifyAnimation(this, newaddr);
            InputFormRef.WriteButtonToYellow(this.Config_WriteButton, false);
            //マップエディタが開いていれば更新する
            MapEditorForm.UpdateMapStyleIfOpen();
            return true;
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
                U.WriteAllBytes(filename, this.ConfigUZ);
            }
        }

        private void MapChipImportButton_Click(object sender, EventArgs e)
        {
            if (MapStyleEditorFormWarningVanillaTileOverraideForm.CheckWarningUI((uint)ChipsetConfigAddress.Value))
            {
                return;
            }

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

            byte[] map_config = File.ReadAllBytes(filename);
            if (map_config.Length < 9216)
            {
                R.ShowStopError("ファイルサイズが小さすぎます. 9216バイト必要です。");
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            bool r = OverraideMapConfig(map_config, undodata);
            if (!r)
            {
                return;
            }

            Program.Undo.Push(undodata);
        }

        bool OverraideMapConfig(byte[] map_config, Undo.UndoData undodata)
        {
            this.ConfigUZ = map_config;

            //チップセットの更新.
            Chipset_Update();
            SelectedChipset_Update();

            //書き込み
            bool r = WriteMapConfig(undodata);
            if (r)
            {
                return false;
            }

            return true;
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

        private void UNDOButton_Click(object sender, EventArgs e)
        {
            PFR.RunUndo();
        }

        private void REDOButton_Click(object sender, EventArgs e)
        {
            PFR.RunRedo();
        }

        private void CopyTypeButton_Click(object sender, EventArgs e)
        {
            string str = this.Name + "\t" + "TypeOnly" 
                + "\t" + (ConfigTerrain.SelectedIndex);
            U.SetClipboardText(str);
        }

        private void CopyTileButton_Click(object sender, EventArgs e)
        {
            string str = this.Name + "\t" + "Tile" 
                + "\t" + (ConfigTerrain.SelectedIndex)
                + "\t" + (Config_W0.Value)
                + "\t" + (Config_W2.Value)
                + "\t" + (Config_W4.Value)
                + "\t" + (Config_W6.Value);
            U.SetClipboardText(str);
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            string str = Clipboard.GetText();
            string[] arr = str.Split('\t');
            if (arr.Length <= 2)
            {
                return;
            }
            if (arr[0] != this.Name)
            {
                return;
            }
            if (arr[1] == "TypeOnly" && arr.Length >= 1)
            {
                U.SelectedIndexSafety(ConfigTerrain, U.atoi(arr[2]));
            }
            if (arr[1] == "Tile" && arr.Length >= 7)
            {
                U.SelectedIndexSafety(ConfigTerrain, U.atoi(arr[2]));
                U.SelectedIndexSafety(Config_W0, U.atoi(arr[3]));
                U.SelectedIndexSafety(Config_W2, U.atoi(arr[4]));
                U.SelectedIndexSafety(Config_W4, U.atoi(arr[5]));
                U.SelectedIndexSafety(Config_W6, U.atoi(arr[6]));
            }
            Config_WriteButton.PerformClick();
        }

        private void MapStyleEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.C)
            {
                CopyTypeButton.PerformClick();
                return;
            }
            else if (e.Alt && e.KeyCode == Keys.T)
            {
                CopyTileButton.PerformClick();
                return;
            }
            else if (e.Alt && e.KeyCode == Keys.V)
            {
                PasteButton.PerformClick();
                return;
            }
        }
        void SetExplainText()
        {
            this.Explain_MapPalette.AccessibleDescription = R._("地図は5つのパレットとを利用できます。\r\n0-5パレットは、昼のパレットです。\r\n6-9パレットは、霧のパレットです。\r\n\r\n規格外になりますが、\r\nもし霧を利用しない場合は、\r\n6-9パレットも利用することもできます。");
        }


    }
}
