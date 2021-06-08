using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ImageBattleAnimeForm : Form
    {
        public ImageBattleAnimeForm()
        {
            InitializeComponent();

            MakeComboBattleAnimeSP(L_1_BATTLEANIMESP_0);

            InputFormRef.markupJumpLabel(LinkInternt);

            InputFormRef.LoadComboResource(ShowSectionCombo, U.ConfigDataFilename("battleanime_mode_"));
            U.SelectedIndexSafety(ShowSectionCombo, 0);
            U.SelectedIndexSafety(ShowDirectionComboBox, 0);
            U.SelectedIndexSafety(ShowZoomComboBox, 0);
            U.SelectedIndexSafety(ShowPaletteComboBox,0);

//            X_LZ77_INFO.ForeColor = OptionForm.Color_InputDecimal_ForeColor();
//            X_LZ77_INFO.BackColor = OptionForm.Color_InputDecimal_BackColor();

            this.CLASS_LISTBOX.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.ItemListToJumpForm("CLASS");
            U.ConvertListBox(ClassForm.MakeClassList(), ref  this.CLASS_LISTBOX);


            this.AddressList.OwnerDraw(ListBoxEx.DrawImageSPTypeAndText, DrawMode.OwnerDrawFixed);
            this.N_AddressList.OwnerDraw(ListBoxEx.DrawImageBattleAndText, DrawMode.OwnerDrawFixed);
            
            this.InputFormRef = Init(this);
            this.N_InputFormRef = N_Init(this);
                
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;
            U.SelectedIndexSafety(this.CLASS_LISTBOX, 1, false);

            //パレット変更の部分にリンクを置く.
            InputFormRef.markupJumpLabel(this.N_J_28);

            U.SetIcon(BattleAnimeImportButton, Properties.Resources.icon_upload);
            U.SetIcon(BattleAnimeExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(X_N_JumpEditor, Properties.Resources.icon_film);

            U.AllowDropFilename(this, new string[] { ".TXT", ".BIN" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    this.BattleAnimeImportButton_Click(null,null);
                }
            });
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(ImageBattleAnimeForm self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    //00まで検索
                    return Program.ROM.u32(addr + 0) != 0;
                }
                , (int i, uint addr) =>
                {
                    uint b0 = Program.ROM.u8(addr + 0);
                    uint b1 = Program.ROM.u8(addr + 1);
                    uint w2 = Program.ROM.u16(addr + 2);
                    return U.ToHexString(w2) + " " + ImageBattleAnimeForm.getSPTypeName(b0, b1);
                }
                );
        }
        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(ImageBattleAnimeForm self)
        {
            //FEditor Advが文字列長を書いてくれていた場合
            uint FEditorHint = InputFormRef.GetFEditorLengthHint(Program.ROM.p32(Program.ROM.RomInfo.image_battle_animelist_pointer()));
            if (FEditorHint >= 0xFF)
            {//余りにでかいヒントは信じない
                FEditorHint = U.NOT_FOUND;
            }

            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.image_battle_animelist_pointer()
                , 32
                , (int i, uint addr) =>
                {//読込最大値検索
                    //12 20 24 がポインタであればデータがあると考える.
                    if (U.isPointer(Program.ROM.u32(addr + 12))
                        && U.isPointer(Program.ROM.u32(addr + 20))
                        && U.isPointer(Program.ROM.u32(addr + 24))
                        )
                    {
                        return true;
                    }
                    if (FEditorHint != U.NOT_FOUND && i < FEditorHint)
                    {//不明なデータではあるがFEditorがあるというので信用する.
                        return true;
                    }

                    return false;
                }
                , (int i, uint addr) =>
                {
                    String animename = Program.ROM.getString(addr, 8);
                    return U.ToHexString(i+1) + U.SA(animename) + InputFormRef.GetCommentSA(addr);
                }
                );
        }

        private void ImageBattleAnimeForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            uint top_battleanime_baseaddress = N_InputFormRef.BaseAddress;
            uint bottum_battleanime_baseaddress = N_InputFormRef.BaseAddress + (N_InputFormRef.BlockSize * N_InputFormRef.DataCount);
//            ImageUtilOAM.MakeReColorRule(top_battleanime_baseaddress, bottum_battleanime_baseaddress);
#endif
        }

        private void CLASS_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint cid = (uint)CLASS_LISTBOX.SelectedIndex;
            uint addr = ClassForm.GetBattleAnimeAddrWhereID(cid);
            if (!U.isSafetyOffset(addr))
            {
                this.InputFormRef.ClearSelect(true);
                return;
            }

            this.InputFormRef.ReInit(addr);

            //他のクラスでこのデータを参照しているならば、独立ボタンを出す.
            IndependencePanel.Visible = UpdateIndependencePanel();
        }
        private void ShowSectionUpDown_ValueChanged(object sender, EventArgs e)
        {
            N_AddressList_SelectedIndexChanged(null,null);
        }


        private void N_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            U.ForceUpdate(ShowSectionCombo, 0);
            U.ForceUpdate(ShowFrameUpDown, 0);
            DrawSelectedAnime();
        }

        void UpdateLZ77Info(bool errorOver16Anime)
        {
            string error = "";
            string text = "Un-LZ77 ";
            uint frame = U.toOffset((uint)N_P16.Value);
            uint oam   = U.toOffset((uint)N_P20.Value);
            if (U.isSafetyOffset(frame))
            {
                uint size = LZ77.getUncompressSize(Program.ROM.Data, frame);
                text += "Frame: " + size + " ";

                string r = ImageUtilOAM.checkFrameSizeSimple((int)size);
                if (r != "")
                {
                    error += r;
                }
            }
            if (U.isSafetyOffset(oam))
            {
                uint size = LZ77.getUncompressSize(Program.ROM.Data, oam);
                text += "OAM: " + size + " ";

                string r = ImageUtilOAM.checkOAMSizeSimple((int)size);
                if (r != "")
                {
                    error += r;
                }
            }
            if (error != "")
            {
                error = error + "\r\n" + R._("これは警告であり、エラーではありません。\r\n大きいアニメーションは動かないことがあるので警告を表示しています。\r\n警告が出ていても動作することもあります。");
            }

            if (errorOver16Anime)
            {
                text = R._("16色を超える戦闘アニメーションです。") + " " + text;
            }

            X_LZ77_INFO.Text = text;
            X_LZ77_INFO.ErrorMessage = error;
        }



        void DrawSelectedAnime()
        {
            uint showSectionData = U.atoh(ShowSectionCombo.Text) - 1;
            uint showFrameData = (uint)ShowFrameUpDown.Value;
            int paletteIndex = (int)ShowPaletteComboBox.SelectedIndex;

            uint sectionData = (uint)N_P12.Value;
            uint frameData = (uint)N_P16.Value;
            uint rightToLeftOAM = (uint)N_P20.Value;
            uint leftToRightOAM = (uint)N_P24.Value;
            uint palettes = (uint)N_P28.Value;

            if (ShowDirectionComboBox.SelectedIndex == 1)
            {//敵軍の位置を表示
                rightToLeftOAM = leftToRightOAM;
            }

            Bitmap bitmap = ImageUtilOAM.DrawBattleAnime(showSectionData, showFrameData
                , sectionData, frameData, rightToLeftOAM, palettes);
            if (paletteIndex > 0)
            {
                bitmap = ImageUtil.SwapPalette(bitmap, paletteIndex);
            }

            bool errorOver16Anime;
            int palette_count = ImageUtil.GetPalette16Count(bitmap);
            if (palette_count >= 2)
            {
                errorOver16Anime = true;
            }
            else
            {
                errorOver16Anime = false;
            }
            UpdateLZ77Info(errorOver16Anime);

            X_B_ANIME_PIC2.Image = bitmap;
        }


        public enum ScaleTrim
        {
             NO
            ,SCALE_90
            ,SCALE_48
            ,NO_BUT_FLIP
        };

        public static Bitmap DrawBattleAnime(uint id, ScaleTrim trim = ScaleTrim.SCALE_90, uint custompalette = 0, uint showSectionData = 0, uint showFrameData = 0, int showPaletteIndex = 0)
        {
            if (id <= 0)
            {
                return ImageUtil.BlankDummy();
            }
            id = id - 1; //anime IDは 1 からスタート.

            InputFormRef InputFormRef = N_Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }

            uint sectionData = Program.ROM.u32(addr+12);
            uint frameData = Program.ROM.u32(addr+16);
            uint rightToLeftOAM = Program.ROM.u32(addr+20);
            uint palettes = Program.ROM.u32(addr+28);
            if (custompalette > 0)
            {
                uint p = ImageUnitPaletteForm.GetPaletteAddr(custompalette);
                if (U.isSafetyOffset(addr))
                {
                    palettes = p;
                }
            }

            Bitmap bitmap = ImageUtilOAM.DrawBattleAnime(showSectionData, showFrameData
                , sectionData, frameData, rightToLeftOAM, palettes);
            if (showPaletteIndex > 0)
            {
                bitmap = ImageUtil.SwapPalette(bitmap, showPaletteIndex);
            }

            
            if (trim == ScaleTrim.SCALE_48)
            {
                Bitmap trimBitmap = ImageUtil.Blank(48, 48, bitmap);
                ImageUtil.BitBlt(trimBitmap, 0, 0, trimBitmap.Width, trimBitmap.Height, bitmap
                    , 125, 58);
                bitmap.Dispose();
                return trimBitmap;
            }
            else if (trim == ScaleTrim.SCALE_90)
            {
                Bitmap trimBitmap = ImageUtil.Blank(90, 90, bitmap);
                ImageUtil.BitBlt(trimBitmap, 0, 0, trimBitmap.Width, trimBitmap.Height, bitmap
                    , 100, 30);
                bitmap.Dispose();
                return trimBitmap;
            }
            else if (trim == ScaleTrim.NO_BUT_FLIP)
            {
                Bitmap flipBitmap = ImageUtil.Copy(bitmap, 0, 0, bitmap.Width - 8, bitmap.Height, true);
                bitmap.Dispose();
                return flipBitmap;
            }
            else
            {
                return bitmap;
            }

            
        }

       //クラスのアニメを取得します。 とりあえず一番最初の奴を.
       public static uint GetAnimeIDByClassID(uint class_id)
       {
           uint battleanime = ClassForm.GetBattleAnimeAddrWhereID(class_id);
           return GetAnimeIDByAnimeSettingPointer(battleanime);
       }
       public static uint GetAnimeIDByAnimeSettingPointer(uint battleanime)
       {
           battleanime = U.toOffset(battleanime);
           if (!U.isSafetyOffset(battleanime))
           {
               return 0;
           }
           battleanime = U.toOffset(battleanime);

           uint w2 = Program.ROM.u16(battleanime + 2);
           return w2;
       }

       private void ShowFrameUpDown_ValueChanged(object sender, EventArgs e)
       {
           DrawSelectedAnime();
       }

       public void JumpToClassID(uint classid)
       {
           if (classid < CLASS_LISTBOX.Items.Count)
           {
               CLASS_LISTBOX.SelectedIndex = (int)(classid);
           }
           else
           {
               CLASS_LISTBOX.SelectedIndex = -1;
           }
       }
       public void JumpToAnimeSettingPointer(uint ptr)
       {
           uint cid = ClassForm.GetIDWhereBattleAnimeAddr(ptr);
           if (cid == U.NOT_FOUND || cid == 0 || cid >= CLASS_LISTBOX.Items.Count)
           {//クラスに該当の者がない
               CLASS_LISTBOX.SelectedIndex = -1;

               uint addr = U.toOffset(ptr);
               InputFormRef.ReInit(addr);
               return;
           }

           //クラスに該当のものがあるので、クラスを選択してリストを更新する.
           U.ForceUpdate(CLASS_LISTBOX, (int)cid);
       }
       public void JumpToAnimeID(uint animeid)
       {
           //ディフォルトフォーカスは戦闘アニメーションのリストの上へ
           U.SelectedIndexSafety(this.N_AddressList, (int)(animeid), true);
       }

       public static void MakeComboBattleAnimeSP(ComboBox combo)
       {
           combo.BeginUpdate();
           combo.Items.Clear();
           combo.Items.Add(R._("0=アイテム指定"));
           combo.Items.Add(R._("1=種類指定"));
           combo.Items.Add(R._("2=特殊指定不明"));
           combo.EndUpdate();
       }


       public static Bitmap getSPTypeIcon(uint b0, uint b1)
       {
           Bitmap bitmap;
           if (b1 == 0)
           {//アイテム指定 b0はアイテム名
               bitmap = ItemForm.DrawIcon(b0);
           }
           else if (b1 == 1)
           {//種類指定
               if (b0 >= 8)
               {
                   bitmap = ImageSystemIconForm.WeaponIcon(8);
               }
               else
               {
                   bitmap = ImageSystemIconForm.WeaponIcon(b0);
               }
           }
           else
           {
               bitmap = ImageSystemIconForm.WeaponIcon(8);
           }
           return bitmap;
       }

       public static string getSPTypeName(uint b0, uint b1)
       {
           if (b1==0)
           {//アイテム指定 b0はアイテム名
               return ItemForm.GetItemName(b0);
           }
           else if (b1 == 1)
           {//種類指定
               return InputFormRef.GetWeaponTypeName(b0);
           }
           return "??";
       }

       public static string GetBattleAnimeName(uint id)
       {
           if (id <= 0)
           {
               return "";
           }
           id--;

           InputFormRef InputFormRef = N_Init(null);
           uint addr = InputFormRef.IDToAddr(id);
           if (!U.isSafetyOffset(addr))
           {
               return "";
           }
           return Program.ROM.getString(addr, 8) + InputFormRef.GetCommentSA(addr);
       }

       private void W2_ValueChanged(object sender, EventArgs e)
       {
           int w2 = (int)W2.Value;
           if (w2 <= 0)
           {
               N_AddressList.SelectedIndex = -1;
               return;
           }

           w2 = w2 - 1;
           if (w2 < N_AddressList.Items.Count)
           {
               N_AddressList.SelectedIndex = w2;
           }
       }


       //リストが拡張されたとき
       void AddressListExpandsEvent(object sender, EventArgs arg)
       {
           FixBattleAnimeSettingDataOnAddressListExpandsEvent(sender, arg);
           FixBattleAnimePointerOnAddressListExpandsEvent(sender, arg);
       }

       void FixBattleAnimePointerOnAddressListExpandsEvent(object sender, EventArgs arg)
       {
           InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
           uint addr = eearg.NewBaseAddress;
           int count = (int)eearg.NewDataCount;

           if (CLASS_LISTBOX.SelectedIndex < 0)
           {
               return;
           }

           uint cid = (uint)CLASS_LISTBOX.SelectedIndex;
           uint pointer;
           ClassForm.GetBattleAnimeAddrWhereID(cid, out pointer);
           if (!U.isSafetyOffset(pointer))
           {
               return;
           }
           Undo.UndoData undodata = Program.Undo.NewUndoData(this, "FixBattleAnimeSetting");
           //クラスの戦闘アニメポインタを更新する.
           Program.ROM.write_p32(pointer, addr, undodata);
           Program.Undo.Push(undodata);

           this.InputFormRef.ReInit(addr, (uint)count);
       }
       void FixBattleAnimeSettingDataOnAddressListExpandsEvent(object sender, EventArgs arg)
       {
           InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
           uint addr = eearg.NewBaseAddress;
           int count = (int)eearg.NewDataCount;

           uint rom_length = (uint)Program.ROM.Data.Length;

           Undo.UndoData undodata = Program.Undo.NewUndoData(this, "FixBattleAnimeSetting");

           //途中にnullが含まれている場合は、補正します.
           for (int i = 0; i < count; i++)
           {
               if (addr + 4 > rom_length)
               {
                   Log.Error("ROM Broken! Address after allocation is out of range. {0}+2/{1}", U.ToHexString8(addr), U.ToHexString8(rom_length));
                   break;
               }
               if (Program.ROM.u32(addr + 0) == 0)
               {//アドレスが空だったら増やす必要がある
                   //とりあえずアイテム、剣、アニメ1
                   Program.ROM.write_u8(addr + 0, 0x0, undodata);
                   Program.ROM.write_u8(addr + 1, 0x1, undodata);
                   Program.ROM.write_u16(addr + 2, 0x1, undodata);
               }

               addr += eearg.BlockSize;
           }
           Program.Undo.Push(undodata);
       }

       public static string GetBattleAnimeHint(uint search_animeindex)
       {
           List<U.AddrResult> classlist = ClassForm.MakeClassList();
           for (int cid = 0; cid < classlist.Count; cid++)
           {
               uint addr = ClassForm.GetBattleAnimeAddrWhereID((uint)cid);
               if (!U.isSafetyOffset(addr))
               {
                   continue;
               }
               for (uint i = 0; true; i += 4)
               {
                   uint a = addr + i;
                   if ( ! U.isSafetyOffset(a + 3))
                   {
                       break;
                   }

                   uint item = Program.ROM.u8(a + 0 );
                   uint sp = Program.ROM.u8(a + 1);
                   uint anime = Program.ROM.u16(a + 2);
                   if (item == 0 && sp == 0 && anime == 0)
                   {
                       break;
                   }
                   if (anime != search_animeindex)
                   {
                       continue;
                   }
                   //発見!
                   string name = U.skip(classlist[cid].name," ").Trim();
                   return name + " " + getSPTypeName(item, sp); 
               }
           }

           //ない
           return "";
       }

       private void BattleAnimeExportButton_Click(object sender, EventArgs e)
       {
           uint battleanime_baseaddress = InputFormRef.SelectToAddr(N_AddressList);
           if (battleanime_baseaddress == U.NOT_FOUND)
           {
               return;
           }

           string title = R._("保存するファイル名を選択してください");
           string filter = R._("FEditorシリアライズ形式|*.bin|バトルアニメ コメントあり|*.txt|バトルアニメ コメントなし|*.txt|アニメGIF|*.gif|Dump All|*.bin|All files|*");
           
           SaveFileDialog save = new SaveFileDialog();
           save.Title = title;
           save.Filter = filter;
           Program.LastSelectedFilename.Load(this, "", save,N_L_0_SPLITSTRING_7.Text);

           DialogResult dr = save.ShowDialog();
           if (dr != DialogResult.OK)
           {
               return;
           }
           if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
           {
               return;
           }
           string filename = save.FileNames[0];
           Program.LastSelectedFilename.Save(this, "", save);

           uint sectionData = (uint)N_P12.Value;
           uint frameData = (uint)N_P16.Value;
           uint rightToLeftOAM = (uint)N_P20.Value;
           uint leftToRightOAM = (uint)N_P24.Value;
           uint palettes = (uint)N_P28.Value;

           string filehint = GetBattleAnimeHint((uint)N_AddressList.SelectedIndex+1);
           if (filehint == "")
           {//不明な場合、 FE7にある個別バトルにも問い合わせる
               filehint = UnitCustomBattleAnimeForm.GetBattleAnimeHint((uint)N_AddressList.SelectedIndex+1);
           }
           filehint = N_AddressList.Text + " " + filehint;
           int palette_count = ImageUtilOAM.CalcMaxPaletteCount(sectionData, frameData, rightToLeftOAM, palettes);

           string ext = U.GetFilenameExt(save.FileName);
           if (save.FilterIndex == 5)
           {
               {
                   string name = U.ChangeExtFilename(filename, ".gif");
                   uint showSectionData = U.atoh(ShowSectionCombo.Text) - 1;
                   ImageUtilOAM.ExportBattleAnimeGIF(name, showSectionData
                        , sectionData, frameData, rightToLeftOAM, palettes, palette_count);
               }
               {
                   string name = U.ChangeExtFilename(filename, ".bin");
                   ImageUtilOAM.ExportBattleAnimeOnFEditorSerialize(name, battleanime_baseaddress
                        , sectionData, frameData, rightToLeftOAM, leftToRightOAM, palettes, palette_count);
               }
               {
                   string name = U.ChangeExtFilename(filename, ".txt");
                   bool enableComment = false;
                   ImageUtilOAM.ExportBattleAnime(filehint, enableComment, name
                        , sectionData, frameData, rightToLeftOAM, palettes, palette_count);
               }
           }
           else if (save.FilterIndex == 4 || ext == ".GIF")
           {
               uint showSectionData = U.atoh(ShowSectionCombo.Text) - 1;
               ImageUtilOAM.ExportBattleAnimeGIF(filename, showSectionData
                    , sectionData, frameData, rightToLeftOAM, palettes, palette_count);
           }
           else if (save.FilterIndex == 0 || ext == ".BIN")
           {
               ImageUtilOAM.ExportBattleAnimeOnFEditorSerialize(filename, battleanime_baseaddress
                    , sectionData, frameData, rightToLeftOAM, leftToRightOAM, palettes, palette_count);
           }
           else
           {
               bool enableComment = true;
               if (save.FilterIndex == 3)
               {
                   enableComment = false;
               }
               ImageUtilOAM.ExportBattleAnime(filehint, enableComment, filename
                    , sectionData, frameData, rightToLeftOAM, palettes, palette_count);
           }

           //エクスプローラで選択しよう
           U.SelectFileByExplorer(filename);
       }

       private void BattleAnimeImportButton_Click(object sender, EventArgs e)
       {
           if (InputFormRef.IsPleaseWaitDialog(this))
           {//2重割り込み禁止
               return;
           }

           string filename;
           if (ImageFormRef.GetDragFilePath(out filename))
           {
           }
           else
           {
               string title = R._("開くファイル名を選択してください");
               string filter = R._("戦闘アニメ|*.bin;*.txt|FEditorシリアライズ形式|*.bin|FEditorシリアライズ形式(ワイルドカード)|*|バトルアニメ テキスト形式|*.txt|All files|*");

               OpenFileDialog open = new OpenFileDialog();
               open.Title = title;
               open.Filter = filter;
               Program.LastSelectedFilename.Load(this, "", open);
               DialogResult dr = open.ShowDialog();
               if (dr != DialogResult.OK)
               {
                   return;
               }
               if (!U.CanReadFileRetry(open))
               {
                   return;
               }
               Program.LastSelectedFilename.Save(this, "", open);
               filename = open.FileNames[0];
            }

            //インポート実行
            uint id = (uint)N_AddressList.SelectedIndex + 1;
            string error = BattleAnimeImportDirect(id, filename);
            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }
        }


       public string BattleAnimeImportDirect(uint id,string filename)
       {
           if (InputFormRef.IsPleaseWaitDialog(this))
           {//2重割り込み禁止
               return R._("現在他の処理中です");
           }

           if (id <= 0)
           {
               return R._("指定されたID({0})は存在しません。", U.To0xHexString(id)) ;
           }

           uint battleanime_baseaddress = N_InputFormRef.IDToAddr(id - 1);
           if (battleanime_baseaddress == U.NOT_FOUND)
           {
               return R._("指定されたID({0})は存在しません。", U.To0xHexString(id));
           }

           string error = "";

           //少し時間がかかるので、しばらくお待ちください表示.
           using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
           {
               uint top_battleanime_baseaddress = N_InputFormRef.BaseAddress;
               uint bottum_battleanime_baseaddress = N_InputFormRef.BaseAddress + (N_InputFormRef.BlockSize * N_InputFormRef.DataCount);

               string ext = U.GetFilenameExt(filename);
               if (ext == ".TXT")
               {//テキスト形式
                   error = ImageUtilOAM.ImportBattleAnime(filename
                       , battleanime_baseaddress
                       , top_battleanime_baseaddress
                       , bottum_battleanime_baseaddress);
               }
               else if (ext == "" || ext == ".BIN")
               {//拡張子なし FEditorシリアライズ
                   error = ImageUtilOAM.ImportBattleAnimeOnFEditorSerialize(filename
                       , battleanime_baseaddress
                       , top_battleanime_baseaddress
                       , bottum_battleanime_baseaddress);
               }
               else
               {
                   return R._("未対応の拡張子({0})が指定されました。",ext);
               }
           }

           if (error != "")
           {
               return error;
           }

           //選択しているところを再選択して画面を再描画
           U.ReSelectList(N_AddressList);
           //書き込み通知
           InputFormRef.ShowWriteNotifyAnimation(this, battleanime_baseaddress);

           return "";
       }

       private void X_N_JumpEditor_Click(object sender, EventArgs e)
       {
           if (InputFormRef.IsPleaseWaitDialog(this))
           {//2重割り込み禁止
               return;
           }

           uint battleanime_baseaddress = InputFormRef.SelectToAddr(N_AddressList);
           if(battleanime_baseaddress == U.NOT_FOUND)
           {
               return ;
           }
           uint sectionData = (uint)N_P12.Value;
           uint frameData = (uint)N_P16.Value;
           uint rightToLeftOAM = (uint)N_P20.Value;
           uint leftToRightOAM = (uint)N_P24.Value;
           uint palettes = (uint)N_P28.Value;

           uint ID = (uint)N_AddressList.SelectedIndex + 1;

           string filehint = GetBattleAnimeHint(ID);
           if (filehint == "")
           {//不明な場合、 FE7にある個別バトルにも問い合わせる
               filehint = UnitCustomBattleAnimeForm.GetBattleAnimeHint((uint)N_AddressList.SelectedIndex + 1);
           }
           filehint = N_AddressList.Text + " " + filehint;
           int palette_count = ImageUtilOAM.CalcMaxPaletteCount(sectionData, frameData, rightToLeftOAM, palettes);

           //少し時間がかかるので、しばらくお待ちください表示.
           using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
           //テンポラリディレクトリを利用する
           using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
           {
               string filename = Path.Combine(tempdir.Dir, "anime.txt");
               ImageUtilOAM.ExportBattleAnime("", false, filename
                    , sectionData, frameData, rightToLeftOAM, palettes, palette_count);
               if (!File.Exists(filename))
               {
                   R.ShowStopError("アニメーションエディタを表示するために、アニメーションをエクスポートしようとしましたが、アニメをファイルにエクスポートできませんでした。\r\n\r\nファイル:{0}", filename);
                   return;
               }

               ToolAnimationCreatorForm f = (ToolAnimationCreatorForm)InputFormRef.JumpFormLow<ToolAnimationCreatorForm>();
               f.Init( ToolAnimationCreatorUserControl.AnimationTypeEnum.BattleAnime
                   , ID, filehint, filename);
               f.Show();
           }
       }

       private void ShowZoom_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (ShowZoomComboBox.SelectedIndex == 0)
           {
               X_B_ANIME_PIC2.SizeMode = PictureBoxSizeMode.Zoom;
           }
           else
           {
               X_B_ANIME_PIC2.SizeMode = PictureBoxSizeMode.Normal;
           }
       }

//       public static uint CalcBattleAnimeSettingDataLength(uint addr)
//       {
//           InputFormRef InputFormRef = Init(null);
//          InputFormRef.ReInit(addr);
//           return InputFormRef.DataCount * InputFormRef.BlockSize;
//       }

       public static void MakeBattleAnimeSettingDataLength(List<Address> list, uint battleAnimeSettingPointer,string selfname)
       {
           InputFormRef InputFormRef = Init(null);
           InputFormRef.ReInitPointer(battleAnimeSettingPointer);

           FEBuilderGBA.Address.AddAddress(list, InputFormRef
               , selfname, new uint[] { });
       }

       //誤爆すると面倒なことになるフレームとOAMのデータ群
       public static void MakeBattleFrameAndOAMDictionary(Dictionary<uint,bool> dic)
       {
           InputFormRef N_InputFormRef = N_Init(null);

           uint addr = N_InputFormRef.BaseAddress;
           for (uint i = 0; i < N_InputFormRef.DataCount; i++, addr += N_InputFormRef.BlockSize)
           {
               dic[ Program.ROM.p32(addr + 12) ] = true; //Section
               dic[Program.ROM.p32(addr + 16)] = true; //frame
               dic[Program.ROM.p32(addr + 20)] = true; //OAM1
               dic[Program.ROM.p32(addr + 24)] = true; //OAM2
               dic[Program.ROM.p32(addr + 28)] = true; //Palette
           }
       }
       //全データの取得
       public static void MakeAllDataLength(List<Address> list, bool isPointerOnly )
       {
           string selfname ;
           InputFormRef InputFormRef = Init(null);

           uint addr;
           List<U.AddrResult> classList = ClassForm.MakeClassList();
           for (uint cid = 0; cid < classList.Count; cid++)
           {
               uint pointer;
               uint class_addr = classList[(int)cid].addr;
               addr = ClassForm.GetBattleAnimeAddrWhereAddr(class_addr, out pointer);
               if (!U.isSafetyOffset(addr))
               {
                   continue;
               }
               InputFormRef.ReInitPointer(pointer);

               selfname = "BattleAnimeSeting:" + U.To0xHexString(cid);
               FEBuilderGBA.Address.AddAddress(list,InputFormRef
                   , selfname, new uint[] { });
           }

           selfname = "BattleAnime";
           InputFormRef N_InputFormRef = N_Init(null);
           FEBuilderGBA.Address.AddAddress(list, N_InputFormRef, selfname, new uint[] {12,16,20,24,28 });

            //戦闘アニメーションはlz77圧縮の中にポインタがある特殊形式です
            addr = N_InputFormRef.BaseAddress;
            List<uint> seatNumberList = new List<uint>(256);
            for (int i = 0; i < N_InputFormRef.DataCount; i++, addr += N_InputFormRef.BlockSize)
            {
                if (!U.isSafetyOffset(12 + addr + 4))
                {
                    break;
                }

                uint section = Program.ROM.p32(12 + addr);
                if (!U.isSafetyOffset(section))
                {
                    break;
                }
                selfname = "BattleAnime:" + U.To0xHexString(i + 1);
                ImageUtilOAM.MakeAllDataLength(list, isPointerOnly, selfname, addr, seatNumberList);
            }
       }
       public static List<U.AddrResult> MakeBattleList()
       {
           InputFormRef N_InputFormRef = N_Init(null);
           return N_InputFormRef.MakeList();
       }

       private void ShowPaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
       {
           DrawSelectedAnime();
       }




       private void N_J_28_Click(object sender, EventArgs e)
       {
           X_N_JumpPalette.PerformClick();
       }
       private void X_N_JumpPalette_Click(object sender, EventArgs e)
       {
           ImageBattleAnimePalletForm f = (ImageBattleAnimePalletForm)InputFormRef.JumpFormLow<ImageBattleAnimePalletForm>();
           f.JumpTo((uint)N_AddressList.SelectedIndex + 1
               , (uint)N_P28.Value
               , (int)ShowPaletteComboBox.SelectedIndex);
           f.FormClosed += (s, ee) =>
           {
               if (this.IsDisposed)
               {
                   return;
               }
               U.ReSelectList(this.N_AddressList);
           };
           f.Show();
       }

       //斧使いが、手斧のモーションを持っているかテストする.
       static void MakeCheckErrorAxs(InputFormRef ifr
           , List<U.AddrResult> axsItems
           , List<U.AddrResult> classList
           , uint cid
           , List<FELint.ErrorSt> errors)
        {
            bool axsUser = false;
            List<uint> axsList = new List<uint>();

            List<U.AddrResult> list = ifr.MakeList();
            for (int i = 0; i < list.Count; i++)
            {
                uint b0 = Program.ROM.u8(list[i].addr);
                uint b1 = Program.ROM.u8(list[i].addr + 1);

                if (b1 == 1)
                {//1=種類 
                    if (b0 == 2)
                    {//2=斧
                        axsUser = true;
                    }
                }
                else if (b1 == 0)
                {//0=アイテム
                    axsList.Add(b0);
                }
            }

            if (axsUser == false)
            {//問題なし
                return ;
            }
            //斧使い 全種類の手斧をもっているか?
            for (int i = 0; i < axsItems.Count; i++)
            {
                uint itemid = axsItems[i].tag;
                if (axsList.IndexOf(itemid) >= 0)
                {//もっている.
                    continue;
                }

                //取りこぼし.

                //武器レベルS ?
                uint wlevel = ItemForm.GetItemWeaponLevelAddr(axsItems[i].addr);
                if (wlevel >= 250)
                {
                    //上級職ではない?
                    bool isHigh = ClassForm.isHighClassAddr(classList[(int)cid].addr);
                    if (isHigh == false)
                    {//上級職でないので、この武器を使えません.
                        continue;
                    }
                }

                if (PatchUtil.SearchCache_HandAxsWildCard() == PatchUtil.HandAxsWildCard_extends.Enable)
                {
                    if (axsList.IndexOf(0x28) >= 0)
                    {//手斧モーションで代用可
                        continue;
                    }
                }

                errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME_CLASS, ifr.BaseAddress
                    , R._("クラス({0})は、斧を利用できますが、手斧({1})の設定がありません。\r\nGBAFEでは、手斧系の武器は、アイテムごとに投げるモーションを設定する必要があります。\r\n個別に設定するのが面倒な場合は、「手斧モーションを投げ斧の汎用モーションとして利用する」パッチを有効にしてください。"
                    , classList[(int)cid].name
                    , axsItems[i].name), cid));
            }
        }

        //エラーチェック
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
           bool isFE6 = (Program.ROM.RomInfo.version() == 6);
           if (!isFE6)
           {//FE6の場合、パラディンなどが手斧モーションを持っていない.
            //そのため、FE7,FE8だけチェックします.
               InputFormRef InputFormRef = Init(null);

               List<U.AddrResult> handAxsItems = ItemForm.MakeItemListByHandAxs();
               List<U.AddrResult> classList = ClassForm.MakeClassList();
               for (uint cid = 1; cid < classList.Count; cid++)
               {
                   uint pointer;
                   uint class_addr = classList[(int)cid].addr;
                   uint addr = ClassForm.GetBattleAnimeAddrWhereAddr(class_addr, out pointer);
                   if (addr == 0)
                   {
                       continue;
                   }
                   if (!U.isSafetyOffset(addr))
                   {
                       errors.Add(new FELint.ErrorSt(FELint.Type.CLASS, class_addr
                           , R._("クラス({0})の{1}のポインタ({2})が危険です。"
                           , classList[(int)cid].name ,R._("戦闘アニメ"), U.To0xHexString(addr)), cid));
                       continue;
                   }

                    InputFormRef.ReInit(addr);
                    MakeCheckErrorAxs(InputFormRef
                        , handAxsItems, classList, cid, errors);
               }
           }

           {
               InputFormRef N_InputFormRef = N_Init(null);
               if (N_InputFormRef.DataCount < 10)
               {
                   errors.Add(new FELint.ErrorSt(FELint.Type.BATTLE_ANIME, U.NOT_FOUND
                       , R._("戦闘アニメが極端に少ないです。破損している可能性があります。")));
               }

               List<uint> seatNumberList = new List<uint>(256);
               uint p = N_InputFormRef.BaseAddress;
               for (int i = 0; i < N_InputFormRef.DataCount; i++, p += N_InputFormRef.BlockSize)
               {
                   if (isFE6 && i == 0x6c)
                   {
                       continue;
                   }
                   ImageUtilOAM.MakeCheckError(errors, p, (uint)(i), seatNumberList);
               }
           }
       }

       //他のクラスでこのデータを参照しているか?
       bool UpdateIndependencePanel()
       {
           if (this.CLASS_LISTBOX.SelectedIndex < 0)
           {
               return false;
           }
           uint classid = (uint)U.atoh(this.CLASS_LISTBOX.Text);

           uint currentP = ClassForm.GetBattleAnimeAddrWhereID(classid);
           if (!U.isSafetyOffset(currentP))
           {
               return false;
           }

           uint class_count = (uint)this.CLASS_LISTBOX.Items.Count;
           for (uint i = 0; i < class_count; i++)
           {
               if (i == classid)
               {//自分自身
                   continue;
               }
               uint p = ClassForm.GetBattleAnimeAddrWhereID(i);
               if (p == currentP)
               {
                   return true;
               }
           }

           return false;
       }
       private void IndependenceButton_Click(object sender, EventArgs e)
       {
           if (this.CLASS_LISTBOX.SelectedIndex < 0)
           {
               return;
           }
           uint classid = (uint)U.atoh(this.CLASS_LISTBOX.Text);
           uint classaddr = ClassForm.GetClassAddr(classid);
           string name = U.ToHexString(classid) + " " + ClassForm.GetClassNameLow(classaddr);

           uint pointer;
           uint currentP = ClassForm.GetBattleAnimeAddrWhereID(classid, out pointer);
           if (!U.isSafetyOffset(currentP))
           {
               return;
           }

           Undo.UndoData undodata = Program.Undo.NewUndoData(this, this.Name + " Independence");

           uint dataSize = (InputFormRef.DataCount + 1) * InputFormRef.BlockSize;
           PatchUtil.WriteIndependence(currentP, dataSize, pointer, name, undodata);
           Program.Undo.Push(undodata);

           InputFormRef.ShowWriteNotifyAnimation(this, currentP);

           U.ReSelectList(this.CLASS_LISTBOX);
       }

       private void ReadStartAddress_ValueChanged(object sender, EventArgs e)
       {
           ZeroPointerPanel.Visible = InputFormRef.ShowZeroPointerPanel(this.CLASS_LISTBOX, this.ReadStartAddress);
       }

       private void LinkInternt_Click(object sender, EventArgs e)
       {
           MainFormUtil.GotoMoreData();
       }

    }
}
