using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MapTileAnimation1Form : Form
    {
        public MapTileAnimation1Form()
        {
            InitializeComponent();
            SamplePaletteComboBox.SelectedIndex = 0;

            List<U.AddrResult> animeList = MakeTileAnimation1();
            U.ConvertComboBox(animeList, ref FilterComboBox);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            U.SetIcon(ExportAllButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportAllButton,Properties.Resources.icon_upload);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });
            U.AllowDropFilename(this, new string[]{".TXT"} , (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportAllButton_Click(null, null);
                }
            });
        }


        static List<U.AddrResult> MakeTileAnimation1()
        {
            Dictionary<uint, bool> alreadFound = new Dictionary<uint,bool>();
            List<U.AddrResult> ret_list = new List<U.AddrResult>();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint i = 0; i < mapmax; i++)
            {
                MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(i);
                if (plist.anime1_plist == 0)
                {
                    continue;
                }
                if (alreadFound.ContainsKey(plist.anime1_plist))
                {
                    continue;
                }

                uint addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.ANIMATION,plist.anime1_plist);
                string name = R._("タイルアニメーション1:{0}", U.ToHexString(plist.anime1_plist));
                if (addr == U.NOT_FOUND)
                {
                    name += R._("(破損)");
                }
                U.AddrResult ar = new U.AddrResult(addr , name , plist.anime1_plist);
                ret_list.Add(ar);

                alreadFound[plist.anime1_plist] = true;
            }
            return ret_list;
        }



        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 8
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr+4));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) ;
                }
                );
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef == null)
            {
                return;
            }

            U.AddrResult ar = InputFormRef.SelectToAddrResult(FilterComboBox, FilterComboBox.SelectedIndex);
            if (ar.isNULL())
            {
                return;
            }
            //サンプルのマップを表示します.
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint i = 0; i < mapmax; i++)
            {
                MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(i);
                if (plist.anime1_plist == ar.tag)
                {
                    this.MapPictureBox.LoadMap(i);
                    break;
                }
            }

            this.InputFormRef.ReInit(ar.addr);
        }


        public void JumpToPlist(uint anime1_plist)
        {
            int selected = -1;
            uint addr = U.NOT_FOUND;
            for (int i = 0; i < FilterComboBox.Items.Count; i++)
            {
                U.AddrResult ar = InputFormRef.SelectToAddrResult(FilterComboBox, i);
                if (ar.isNULL())
                {
                    continue;
                }
                if (ar.tag == anime1_plist)
                {//一致
                    selected = i;
                    addr = ar.addr;
                    break;
                }
            }
            if (addr == U.NOT_FOUND)
            {
                return;
            }

            FilterComboBox.SelectedIndex = selected;
            this.InputFormRef.ReInit(addr);
        }

        public static List<U.AddrResult> MakeList(uint addr = U.NOT_FOUND)
        {
            InputFormRef InputFormRef = Init(null);
            if (addr != U.NOT_FOUND)
            {
                InputFormRef.ReInit(addr);
            }
            return InputFormRef.MakeList();
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);

            List<U.AddrResult> urList = MakeTileAnimation1();
            for (int n = 0; n < urList.Count; n++ )
            {
                InputFormRef.ReInit(urList[n].addr);
                string name = urList[n].name;
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 4 });

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    uint addr = Program.ROM.p32(4 + p);
                    uint length = Program.ROM.u16(2 + p);

                    if (U.isSafetyOffset(addr))
                    {
                        FEBuilderGBA.Address.AddAddress(list,addr
                            , length
                            , 4 + p
                            , name + "_" + U.ToHexString(i)
                            , FEBuilderGBA.Address.DataTypeEnum.IMG );
                    }
                }
            }
        }
        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = this.MapPictureBox.GetMapID();
            if (mapid == U.NOT_FOUND)
            {
                return;
            }
            MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(mapid);
            if (plist.palette_plist == 0)
            {
                return;
            }
            uint palette = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE,plist.palette_plist);
            if (palette == U.NOT_FOUND)
            {
                return;
            }

            int palette_index = SamplePaletteComboBox.SelectedIndex;
            if (palette_index < 0)
            {
                palette_index = 0;
            }



            this.ChangePaletteBitmap = GetTileAnime1((uint)P4.Value, (uint)W2.Value, palette, palette_index);
            this.X_SAMPLE_BIG_PIC.Image = this.ChangePaletteBitmap;

            MapSettingForm.MapAnimations anime = new MapSettingForm.MapAnimations();
            anime.change_bitmap_bytes = GetTileAnime1((uint)P4.Value, (uint)W2.Value);

            //マップの再描画
            this.MapPictureBox.UpdateAnime(anime);
        }
        Bitmap ChangePaletteBitmap; //パレットを変更したものサンプルとexportに利用する.

        static int GetHight(uint count)
        {
            int height;
            if (count >= 0x1000)
            {
                height = (int)(count / (32 * 8 / 2) / 8) * 8;
            }
            else
            {
                height = 1 * 8;
            }
            return height;
        }

        static Bitmap GetTileAnime1(uint data_pointer,uint count,uint palette,int palette_index)
        {
            uint date_offset = U.toOffset(data_pointer);
            uint palette_offset = U.toOffset(palette);
            if (!U.isSafetyOffset(date_offset))
            {
                return null;
            }
            if (!U.isSafetyOffset(palette_offset))
            {
                return null;
            }

            int height = GetHight(count);
            return
                ImageUtil.ByteToImage16Tile(32 * 8, height, Program.ROM.Data, (int)date_offset, Program.ROM.Data, (int)palette_offset, palette_index);
        }
        static byte[] GetTileAnime1(uint data_pointer, uint count)
        {
            uint date_offset = U.toOffset(data_pointer);
            if (!U.isSafetyOffset(date_offset))
            {
                return null;
            }
     
            int height = GetHight(count);
            return Program.ROM.getBinaryData(date_offset, 32 * 8 / 2 * height); 
        }

        private void MapTileAnimation1Form_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(FilterComboBox,0);
        }

        private void SamplePaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(null, null);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            ImageFormRef.ExportImage(this, this.ChangePaletteBitmap, InputFormRef.MakeSaveImageFilename(), 5);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 32 * 8;
            int height = GetHight((uint)this.W2.Value);
            int palette_count = 5;
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


            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                //画像等データの書き込み
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);
                this.InputFormRef.WriteImageData(this.P4, image, false, undodata);
                Program.Undo.Push(undodata);
            }
            //ポインタの書き込み
            this.WriteButton.PerformClick();
        }

        //アニメーションの割り当て
        public static uint PreciseMapTileAnimation1Area(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.ANIMATION);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise ANIMATION1 Area", mapid.ToString("X"));

            //無圧縮bit mapdata
            byte[] data = new byte[0x1000];
            uint bitmap_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (bitmap_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            //アニメーション領域を新規に割り当てる.
            data = new byte[8 * 2];
            U.write_u16(data, 0, 0x1C);
            U.write_u16(data, 2, 0x1000);
            U.write_p32(data, 4, bitmap_addr);

            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.ANIMATION, plist, write_addr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            Program.Undo.Push(undodata);

            return plist;
        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("マップアニメ1|*.mapanime1.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", save, "");

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            uint mapid = this.MapPictureBox.GetMapID();
            if (mapid == U.NOT_FOUND)
            {
                return;
            }
            int palette_index = SamplePaletteComboBox.SelectedIndex;
            if (palette_index < 0)
            {
                palette_index = 0;
            }
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                ExportAll(filename, InputFormRef, mapid, palette_index);
            }

            U.SelectFileByExplorer(filename);
        }

        private void ImportAllButton_Click(object sender, EventArgs e)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(FilterComboBox, FilterComboBox.SelectedIndex);
            if (ar.isNULL())
            {
                R.ShowStopError("タイルアニメーションのPLISTを特定できません");
                return;
            }
            uint pointer;
            uint tileanime_addr = MapPointerForm.PlistToOffsetAddrFast(MapPointerForm.PLIST_TYPE.ANIMATION, ar.tag, out pointer);
            if (tileanime_addr == U.NOT_FOUND)
            {
                R.ShowStopError("タイルアニメーションのPLISTからポインタを求められません。\r\nPLIST:{0}",ar.tag);
                return;
            }

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {
                string title = R._("読込むファイル名を選択してください");
                string filter = R._("マップアニメ1|*.mapanime1.txt|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;

                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                if (open.FileNames.Length <= 0 || !U.CanReadFileRetry(open.FileNames[0]))
                {
                    return;
                }
                filename = open.FileNames[0];
                Program.LastSelectedFilename.Save(this, "", open);
            }

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                string errorMessage = ImportAll(filename, pointer , this.InputFormRef);
                if (errorMessage != "")
                {
                    R.ShowStopError(errorMessage);
                    return;
                }
            }

            //リストを読み直し.
            int selected = FilterComboBox.SelectedIndex;
            List<U.AddrResult> animeList = MakeTileAnimation1();
            U.ConvertComboBox(animeList, ref FilterComboBox);

            //現在地を再選択して、リストを描画する.
            U.ForceUpdate(FilterComboBox,selected);
        }
        static string ImportAll(string filename, uint pointer, InputFormRef ifr)
        {
            List<Address> recycle = new List<Address>();

            //古いデータをリサイクルリストに入れる.
            FEBuilderGBA.Address.AddAddress(recycle
                , ifr
                , ""
                , new uint[] { 4 } );
            uint addr = ifr.BaseAddress;
            if (U.isSafetyOffset(addr))
            {
                for (int i = 0; i < ifr.DataCount; i++ , addr += ifr.BlockSize)
                {
                    uint length = Program.ROM.u16(addr + 2);
                    uint p4 = Program.ROM.p32(addr + 4);
                    if (!U.isSafetyOffset(p4))
                    {
                        continue;
                    }
                    FEBuilderGBA.Address.AddPointer(recycle
                        , addr + 4
                        , length
                        , ""
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
            }

            RecycleAddress ra = new RecycleAddress(recycle);

            string dir = Path.GetDirectoryName(filename);
            Undo.UndoData undodata = Program.Undo.NewUndoData("TileAnimation1");

            List<byte> writedata = new List<byte>();
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                if (line == "")
                {
                    continue;
                }
                string[] sp = line.Split('\t');
                if (sp.Length < 2)
                {
                    continue;
                }
                uint wait = U.atoi(sp[0]);
                string imgFilename = Path.Combine(dir, sp[1]);
                Bitmap bitmap = ImageUtil.OpenBitmap(imgFilename);
                if (bitmap == null)
                {
                    return R.Error("タイルアニメーション({0})が見つかりません。\r\n\r\nスクリプト:{1}\r\n行数:{2}"
                        , imgFilename , filename , i);
                }
                byte[] bitmapByte = ImageUtil.ImageToByte16Tile(bitmap,bitmap.Width,bitmap.Height);
                uint newaddr = ra.Write(bitmapByte,undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    return R.Error("タイルアニメーション({0})を書き込めませんでした。\r\n\r\nスクリプト:{1}\r\n行数:{2}"
                        , imgFilename, filename, i);
                }
                bitmap.Dispose();

                U.append_u16(writedata, wait);  //wait
                U.append_u16(writedata, (uint)bitmapByte.Length);   //length
                U.append_u32(writedata, U.toPointer(newaddr)); //画像ポインタ
            }
            //終端データ
            U.append_u16(writedata, 0); //term
            U.append_u16(writedata, 0); //length
            U.append_u32(writedata, 0); //画像ポインタ

            uint newpointer = ra.WriteAndWritePointer(pointer, writedata.ToArray(), undodata);
            if (newpointer == U.NOT_FOUND)
            {
                return R.Error("タイルアニメーションのポインタを書き込めませんでした。\r\n\r\nスクリプト:{0}"
                    , filename);
            }
            ra.BlackOut(undodata);

            Program.Undo.Push(undodata);
            return "";
        }

        static void ExportAll(string filename, InputFormRef ifr, uint mapid, int palette_index)
        {
            uint addr = ifr.BaseAddress;
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            string dir = Path.GetDirectoryName(filename);
            string file = Path.GetFileNameWithoutExtension(filename);

            MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(mapid);
            if (plist.palette_plist == 0)
            {
                return;
            }
            uint palette = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, plist.palette_plist);
            if (palette == U.NOT_FOUND)
            {
                return;
            }

            List<string> lines = new List<string>();
            {
                string line = "//wait\tfilename";
                lines.Add(line);
            }
            for (int i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
            {
                uint w0 = Program.ROM.u16(addr + 0);
                uint length = Program.ROM.u16(addr + 2);
                uint p4 = Program.ROM.p32(addr + 4);
                if (!U.isSafetyOffset(p4))
                {
                    continue;
                }

                Bitmap bitmap = GetTileAnime1(p4, length, palette, palette_index);
                string imgfilename = file + "_" + i.ToString("000") + ".png";
                bitmap.Save(Path.Combine(dir,imgfilename));

                string line = w0 + "\t" + imgfilename;
                lines.Add(line);
            }

            File.WriteAllLines(filename, lines);
        }

    }
}
