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
    public partial class MapTileAnimation2Form : Form
    {
        public MapTileAnimation2Form()
        {
            InitializeComponent();

            List<U.AddrResult> animeList = MakeTileAnimation2();
            U.ConvertComboBox(animeList, ref FilterComboBox);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.N_InputFormRef = N_Init(this);
            this.N_InputFormRef.PostAddressListExpandsEvent += N_AddressListExpandsEvent;
            this.N_AddressList.OwnerDraw(ListBoxEx.DrawColorAndText, DrawMode.OwnerDrawFixed);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);

            U.SetIcon(ExportAllButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportAllButton, Properties.Resources.icon_upload);

            U.AllowDropFilename(this, new string[] { ".TXT" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportAllButton_Click(null, null);
                }
            });
        }

        //リストが拡張されたとき
        void N_AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            this.P0.Value = addr;
            this.B5.Value = count;

            InputFormRef.WriteButtonToYellow(this.WriteButton, true);
        }

        static List<U.AddrResult> MakeTileAnimation2()
        {
            Dictionary<uint, bool> alreadFound = new Dictionary<uint, bool>();
            List<U.AddrResult> ret_list = new List<U.AddrResult>();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint i = 0; i < mapmax; i++)
            {
                MapSettingForm.PLists plist = MapSettingForm.GetMapPListsWhereMapID(i);
                if (plist.anime2_plist == 0)
                {
                    continue;
                }
                if (alreadFound.ContainsKey(plist.anime2_plist))
                {
                    continue;
                }

                uint addr = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.ANIMATION2, plist.anime2_plist);
                string name = R._("タイルアニメーション2 パレットアニメ:{0}", U.ToHexString(plist.anime2_plist));
                if (addr == U.NOT_FOUND)
                {
                    name += R._("(破損)");
                }
                U.AddrResult ar = new U.AddrResult(addr, name, plist.anime2_plist);
                ret_list.Add(ar);

                alreadFound[plist.anime2_plist] = true;
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
                    return U.isPointer(Program.ROM.u32(addr + 0));
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , 0
                , 2
                , (int i, uint addr) =>
                {
                    return false;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
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
                if (plist.anime2_plist == ar.tag)
                {
                    this.MapPictureBox.LoadMap(i);
                    break;
                }
            }

            this.InputFormRef.ReInit(ar.addr);
        }


        public void JumpToPlist(uint anime2_plist)
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
                if (ar.tag == anime2_plist)
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

            List<U.AddrResult> urList = MakeTileAnimation2();
            for (int n = 0; n < urList.Count; n++)
            {
                InputFormRef.ReInit(urList[n].addr);
                string name = urList[n].name;
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 });

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    uint addr = Program.ROM.p32(0 + p);
                    uint count = Program.ROM.u8(5 + p);

                    if (U.isSafetyOffset(addr))
                    {
                        FEBuilderGBA.Address.AddAddress(list,addr
                            , count * 2
                            , 0 + p
                            , name + "_" + U.To0xHexString(i)
                            , FEBuilderGBA.Address.DataTypeEnum.BIN);
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
            N_InputFormRef.ReInit((uint)P0.Value,(uint)B5.Value);

            MapSettingForm.MapAnimations anime = new MapSettingForm.MapAnimations();
            anime.change_palette_bytes = GetTileAnime2((uint)P0.Value, (uint)B5.Value);
            anime.change_palette_start_index = (uint)B6.Value;

            //マップの再描画
            this.MapPictureBox.UpdateAnime(anime);
        }

        static byte[] GetTileAnime2(uint data_pointer, uint count)
        {
            uint date_offset = U.toOffset(data_pointer);
            if (!U.isSafetyOffset(date_offset))
            {
                return null;
            }

            return Program.ROM.getBinaryData(date_offset, 2 * count);
        }

        private void MapTileAnimation1Form_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(FilterComboBox, 0);
        }

        private void SamplePaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(null, null);
        }

        private void MapTileAnimation2Form_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(FilterComboBox, 0);
        }

        //アニメーション2の割り当て
        public static uint PreciseMapTileAnimation2Area(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.ANIMATION2);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise ANIMATION2 Area", mapid.ToString("X"));

            //PALデータ 4つ
            byte[] data = new byte[2*4];
            U.write_u16(data, 0 , 0x16);
            U.write_u16(data, 2 , 0x9D);
            U.write_u16(data, 4 , 0x17F);
            U.write_u16(data, 6 , 0xA1F);

            uint palette_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (palette_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            //パレット領域を新規に割り当てる.
            data = new byte[8 * 2];
            U.write_p32(data, 0, palette_addr);
            U.write_u8(data, 4, 0x13); //アニメ感覚
            U.write_u8(data, 5, 0x4);//個数
            U.write_u8(data, 6, 0x3C); //書き換え開始パレット

            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }
            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.ANIMATION2, plist, write_addr, undodata);
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
            string filter = R._("マップアニメ2|*.mapanime2.txt|GifAnime|*.gif|All files|*");

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
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);
            string ext = U.GetFilenameExt(filename);
            if (ext == ".GIF")
            {
                ExportGif(filename);
            }
            else
            {
                ExportBatchTxt(filename);
            }
        }

        private void ExportGif(string filename)
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
            uint palette = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, plist.palette_plist);
            if (palette == U.NOT_FOUND)
            {
                return;
            }

            uint addr = InputFormRef.BaseAddress;
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            //同じアニメを何度も出力しないように記録する.
            List<ImageUtilAnimeGif.Frame> bitmaps = new List<ImageUtilAnimeGif.Frame>();

            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint p0 = Program.ROM.p32(addr + 0);
                uint wait = Program.ROM.u8(addr + 4);
                uint count = Program.ROM.u8(addr + 5);
                uint startindex = Program.ROM.u8(addr + 6);
                if (!U.isSafetyOffset(p0))
                {
                    continue;
                }

                MapSettingForm.MapAnimations anime = new MapSettingForm.MapAnimations();
                anime.change_palette_bytes = GetTileAnime2(p0, count);
                anime.change_palette_start_index = startindex;

                Bitmap mapBitmap = MapSettingForm.DrawMap(mapid, anime);
                bitmaps.Add(new ImageUtilAnimeGif.Frame(mapBitmap, wait));
            }

            //アニメgif生成
            ImageUtilAnimeGif.SaveAnimatedGif(filename, bitmaps);
            U.SelectFileByExplorer(filename);
        }
        void ExportBatchTxt(string filename)
        {
            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                ExportAll(filename, InputFormRef);
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
                R.ShowStopError("タイルアニメーションのPLISTからポインタを求められません。\r\nPLIST:{0}", ar.tag);
                return;
            }
            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {
                string title = R._("読込むファイル名を選択してください");
                string filter = R._("マップアニメ2|*.mapanime2.txt|All files|*");

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
                string errorMessage = ImportAll(filename, pointer, this.InputFormRef);
                if (errorMessage != "")
                {
                    R.ShowStopError(errorMessage);
                    return;
                }
            }

            //リストを読み直し.
            int selected = FilterComboBox.SelectedIndex;
            List<U.AddrResult> animeList = MakeTileAnimation2();
            U.ConvertComboBox(animeList, ref FilterComboBox);

            //現在地を再選択して、リストを描画する.
            U.ForceUpdate(FilterComboBox, selected);
        }
        static string ImportAll(string filename, uint pointer, InputFormRef ifr)
        {
            List<Address> recycle = new List<Address>();

            //古いデータをリサイクルリストに入れる.
            FEBuilderGBA.Address.AddAddress(recycle
                , ifr
                , ""
                , new uint[] { 0 });

            uint addr = ifr.BaseAddress;
            if (U.isSafetyOffset(addr))
            {
                for (int i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
                {
                    uint p0 = Program.ROM.p32(addr + 0);
                    uint wait = Program.ROM.u8(addr + 4);
                    uint count = Program.ROM.u8(addr + 5);
                    uint startindex = Program.ROM.u8(addr + 6);

                    if (!U.isSafetyOffset(p0))
                    {
                        continue;
                    }
                    FEBuilderGBA.Address.AddPointer(recycle
                        , addr + 0
                        , count * 2
                        , ""
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
            }

            RecycleAddress ra = new RecycleAddress(recycle);

            string dir = Path.GetDirectoryName(filename);
            Undo.UndoData undodata = Program.Undo.NewUndoData("TileAnimation2");

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
                uint startindex = U.atoi(sp[1]);
                uint count = 0;

                List<byte> palByte = new List<byte>();
                for (int n = 2; n < sp.Length; n++, count++)
                {
                    string pal = sp[n];
                    string[] rgbarray = pal.Split(',');
                    int r = (int)U.atoi0x(U.at(rgbarray, 0));
                    int g = (int)U.atoi0x(U.at(rgbarray, 1));
                    int b = (int)U.atoi0x(U.at(rgbarray, 2));

                    uint gbaRGB = ImageUtil.ColorToGBARGB(Color.FromArgb(r, g, b));
                    U.append_u16(palByte, gbaRGB);
                }
                uint newaddr = ra.Write(palByte.ToArray(), undodata);
                if (newaddr == U.NOT_FOUND)
                {
                    return R.Error("タイルアニメーション2を書き込めませんでした。\r\n\r\nスクリプト:{1}\r\n行数:{2}"
                        , filename, i);
                }

                U.append_u32(writedata, U.toPointer(newaddr)); //パレットポインタ
                U.append_u8(writedata, wait);  //wait
                U.append_u8(writedata, count);  //count
                U.append_u8(writedata, startindex);  //startindex
                U.append_u8(writedata, 0);  //0
            }
            //終端データ
            U.append_u32(writedata, 0); //パレットポインタ
            U.append_u8(writedata, 0);  //wait
            U.append_u8(writedata, 0);  //count
            U.append_u8(writedata, 0);  //startindex
            U.append_u8(writedata, 0);  //0

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

        static void ExportAll(string filename, InputFormRef ifr)
        {
            uint addr = ifr.BaseAddress;
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            string dir = Path.GetDirectoryName(filename);
            string file = Path.GetFileNameWithoutExtension(filename);

            List<string> lines = new List<string>();
            {
                string line = "//wait\tstartindex\tR,G,B Colors...";
                lines.Add(line);
            }
            for (int i = 0; i < ifr.DataCount; i++, addr += ifr.BlockSize)
            {
                uint p0 = Program.ROM.p32(addr + 0);
                uint wait = Program.ROM.u8(addr + 4);
                uint count = Program.ROM.u8(addr + 5);
                uint startindex = Program.ROM.u8(addr + 6);
                if (!U.isSafetyOffset(p0))
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(wait);
                sb.Append("\t");
                sb.Append(startindex);
                uint paladdr = p0;
                for (int n = 0; n < count; n++, paladdr += 2)
                {
                    uint pal = Program.ROM.u16(paladdr);
                    byte r = (byte) ((pal & 0x1F) << 3);
                    byte g = (byte) (((pal >> 5) & 0x1F) << 3);
                    byte b = (byte) (((pal >> 10) & 0x1F) << 3);

                    sb.Append("\t");
                    sb.Append(r + "," + g + "," + b);
                }

                lines.Add(sb.ToString());
            }

            File.WriteAllLines(filename, lines);
        }

    }
}
