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
    public partial class SkillConfigFE8NVer2SkillForm : Form
    {
        uint[] BasePointer;


        public SkillConfigFE8NVer2SkillForm()
        {
            InitializeComponent();

            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                R.ShowStopError("スキル拡張 FE8Nver2 のポインタを取得できませんでした");
                return;
            }
            if (pointer.Length < 5)
            {
                R.ShowStopError("スキル拡張 FE8Nver2 のポインタが所定の数だけありません。\r\n{0}個必要ですが、{1}個しかありません。",5,pointer.Length);
                return;
            }
            this.BasePointer = pointer;
            if (pointer.Length > 5)
            {
                this.AnimeBaseAddress = Program.ROM.p32(pointer[5]);
            }

            InitAnime();

            N1_InputFormRef = N1_Init(this);
            N1_InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent_N1;
            N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N1_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            N2_InputFormRef = N2_Init(this);
            N2_InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent_N2;
            N2_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N2_AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);

            N3_InputFormRef = N3_Init(this);
            N3_InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent_N3;
            N3_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N3_AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this, pointer[4]);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.AddressList.OwnerDraw(DrawSkillAndText, DrawMode.OwnerDrawFixed);


            MainTab.SelectedIndex = 1;
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(AnimationInportButton, Properties.Resources.icon_upload);
            U.SetIcon(AnimationExportButton, Properties.Resources.icon_arrow);

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    ImportButton_Click(null, null);
                }
            });

        }

        //アニメを利用できない場合は消す. かならず Initの前に呼ぶ
        void InitAnime()
        {
            if (! U.isSafetyOffset(AnimeBaseAddress) )
            {
                //アニメを設定できないので消す.
                MainTab.TabPages.Remove(tabAnimePage);
                return;
            }

            U.AllowDropFilename(this, new string[] { ".TXT" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    AnimationImportButton_Click(null, null);
                }
            });
        }

        uint AnimeBaseAddress;

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, uint skl_tablePointer )
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , skl_tablePointer
                , 16
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (Program.ROM.u8(addr) == 0xFF)
                    {//終端コードが出てきたらそこで強制終了
                        return false;
                    }

                    return true;
                }
                , (int i, uint addr) =>
                {
                    string text = TextForm.Direct(Program.ROM.u16(addr + 0));
                    return U.ToHexString(i) + " " + SkillConfigFE8NSkillForm.ParseTextToSkillName(text);
                }
            );

            return ifr;
        }

        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {
            return new InputFormRef(self
                , "N1_"
                , 0
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint a = Program.ROM.u8(addr);
                    if (a == 0)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint unitid = Program.ROM.u8(addr);
                    return U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);
                }
            );
        }
        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {
            return new InputFormRef(self
                , "N2_"
                , 0
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint a = Program.ROM.u8(addr);
                    if (a == 0)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint classid = Program.ROM.u8(addr);
                    return U.ToHexString(classid) + " " + ClassForm.GetClassName(classid);
                }
            );
        }
        InputFormRef N3_InputFormRef;
        static InputFormRef N3_Init(Form self)
        {
            return new InputFormRef(self
                , "N3_"
                , 0
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint a = Program.ROM.u8(addr);
                    if (a == 0)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint itemid = Program.ROM.u8(addr);
                    return U.ToHexString(itemid) + " " + ItemForm.GetItemName(itemid);
                }
            );
        }

        public static void ClearCache()
        {
            g_Cache_Pointers = null;
        }
        static uint[] g_Cache_Pointers = null;
        public static uint[] FindSkillFE8NVer2IconPointers()
        {
            if (g_Cache_Pointers == null)
            {
                g_Cache_Pointers = FindSkillFE8NVer2IconPointersLow();
            }
            return g_Cache_Pointers;
        }
        static uint[] FindSkillFE8NVer2IconPointersLow()
        {
            uint iconExPointer = Program.ROM.u32(0x89268 + 4);
            if (!U.isSafetyPointer(iconExPointer))
            {
                return null;
            }
            iconExPointer = U.toOffset(iconExPointer);

            byte[] need = new byte[] { 0x50, 0x93,0x08,0x08 , 0x48, 0x93 ,0x08,0x08  };
            uint iconPointers = U.Grep(Program.ROM.Data, need, 0xE00000, 0, 4);
            if (iconPointers == U.NOT_FOUND)
            {
                return null;
            }
            iconPointers = iconPointers - (4*5);
            List<uint> pointer = new List<uint>();
            for (uint p = iconPointers; true; p += 4)
            {
                uint pp = Program.ROM.u32(p);
                if (!U.isSafetyPointer(pp))
                {
                    break;
                }
                pp = U.toOffset(pp);
                if (pp < 0xE00000)
                {//APIポインタと区別したい.
                    continue;
                }
                pointer.Add(p);
            }
            if (pointer.Count <= 0)
            {
                return null;
            }

            if (Program.ROM.u16(0x70B96) == 0x00)
            {
                //anime pointer
                uint p = iconPointers + (4 * 3);
                uint anime_pointer = Program.ROM.u32(p);
                if (U.isSafetyPointer(anime_pointer))
                {
                    pointer.Add(p);
                }
            }
            return pointer.ToArray();
        }


        private void SkillAssignmentClassFE8Nver2Form_Load(object sender, EventArgs e)
        {

        }

        private void P12_ValueChanged(object sender, EventArgs e)
        {
            N3_InputFormRef.ReInit((uint)P12.Value);
            N3_ZeroPointerPanel.Visible = InputFormRef.ShowZeroPointerPanel(this.AddressList, this.P12);
        }

        private void P8_ValueChanged(object sender, EventArgs e)
        {
            N2_InputFormRef.ReInit((uint)P8.Value);
            N2_ZeroPointerPanel.Visible = InputFormRef.ShowZeroPointerPanel(this.AddressList, this.P8);
        }

        private void P4_ValueChanged(object sender, EventArgs e)
        {
            N1_InputFormRef.ReInit((uint)P4.Value);
            N1_ZeroPointerPanel.Visible = InputFormRef.ShowZeroPointerPanel(this.AddressList, this.P4);
        }

        public static string GetSkillText(uint id)
        {
            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                return "";
            }
            if (pointer.Length < 5)
            {
                return "";
            }
            InputFormRef InputFormRef = Init(null, pointer[4]);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint textid = Program.ROM.u16(addr + 0 );
            string text = TextForm.Direct(textid);
            return SkillConfigFE8NSkillForm.ParseTextToSkillName(text);
        }

        public static Bitmap DrawSkillIcon(uint id)
        {
            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                return ImageUtil.BlankDummy();
            }
            if (pointer.Length < 5)
            {
                return ImageUtil.BlankDummy();
            }

            return DrawSkillIconLow(id, pointer[4]);
        }

        public static Bitmap DrawSkillIconLow(uint id, uint skl_tableP)
        {
            InputFormRef ifr = Init(null, skl_tableP);
            uint addr = ifr.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }
            uint pal = Program.ROM.u16(addr + 2);
            uint skillPaletteAddress = GetSkillPaletteAddress(pal);

            uint iconbaseaddr = Program.ROM.p32(Program.ROM.RomInfo.icon_pointer());
            uint icondatacount = (2 * 8 * 2 * 8) / 2; // /2しているのは16色のため
            uint iconaddr = iconbaseaddr + (icondatacount * (0x100 + id));

            return ImageItemIconForm.DrawIcon(iconaddr, skillPaletteAddress);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.BasePointer == null)
            {
                return;
            }

            SKILLICON.Image = DrawSkillIconLow((uint)AddressList.SelectedIndex, this.BasePointer[4]);
            DrawAnime();
        }

        void DrawAnime()
        {
            if (U.isSafetyOffset(AnimeBaseAddress))
            {
                uint animePointer = AnimeBaseAddress + (4 * (uint)AddressList.SelectedIndex);

                uint animeAddr = Program.ROM.p32(animePointer);

                if (U.isSafetyOffset(animeAddr))
                {
                    AnimePointer.Value = animeAddr;
                    ShowFrameUpDown_ValueChanged(null, null);
                    AnimationPanel.Visible = true;
                    return;
                }
            }
            AnimePointer.Value = 0;
            AnimationPanel.Visible = false;
        }

        public static bool IsFE8NVer2()
        {
            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                return false;
            }
            if (pointer.Length < 5)
            {
                return false;
            }
            return true;
        }
        //Skill + テキストを書くルーチン
        Size DrawSkillAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();
            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (ListBoxEx.OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;
            uint addr = InputFormRef.SelectToAddr(lb, (int)index);

            Bitmap bitmap = null;
            if (addr != U.NOT_FOUND)
            {
                uint palette = Program.ROM.u16(addr + 2);
                bitmap = DrawSkillIconLow((uint)index, this.BasePointer[4]);
            }
            else
            {
                bitmap = ImageUtil.BlankDummy();
            }
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);
            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;

            brush.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = bitmap = DrawSkillIconLow((uint)AddressList.SelectedIndex, this.BasePointer[4]);
            ImageFormRef.ExportImage(this,bitmap, InputFormRef.MakeSaveImageFilename());
        }

        private void AddressListExpandsEvent_N1(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;

            P4.Value = addr;
            WriteButton.PerformClick();

            N1_ReadCount.Value = eearg.NewDataCount;
            N1_InputFormRef.ReInit(addr, eearg.NewDataCount);
        }
        private void AddressListExpandsEvent_N2(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;

            P8.Value = addr;
            WriteButton.PerformClick();

            N2_ReadCount.Value = eearg.NewDataCount;
            N2_InputFormRef.ReInit(addr, eearg.NewDataCount);
        }
        private void AddressListExpandsEvent_N3(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;

            P12.Value = addr;
            WriteButton.PerformClick();

            N3_ReadCount.Value = eearg.NewDataCount;
            N3_InputFormRef.ReInit(addr, eearg.NewDataCount);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = ImageFormRef.ImportFilenameDialog(this);
            if (bitmap == null)
            {
                return;
            }
            int width = 2 * 8;
            int height = 2 * 8;
            if (bitmap.Width != width || bitmap.Height != height)
            {
                R.ShowStopError("画像サイズが正しくありません。\r\nWidth:{2} Height:{3} でなければなりません。\r\n\r\n選択された画像のサイズ Width:{0} Height:{1}", bitmap.Width, bitmap.Height, width, height);
                return;
            }

            uint skillPaletteAddress = GetSkillPaletteAddress((uint)W2.Value);
            //check palette
            {
                string palette_error =
                    ImageUtil.CheckPalette(bitmap.Palette
                        , Program.ROM.Data
                        , skillPaletteAddress
                        , U.NOT_FOUND
                        );
                if (palette_error != "")
                {
                    ErrorPaletteShowForm f = (ErrorPaletteShowForm)InputFormRef.JumpFormLow<ErrorPaletteShowForm>();
                    f.SetErrorMessage(palette_error);
                    f.SetOrignalImage(ImageUtil.OverraidePalette(bitmap, Program.ROM.Data, skillPaletteAddress));
                    f.SetReOrderImage1(ImageUtil.ReOrderPalette(bitmap, Program.ROM.Data, skillPaletteAddress));
                    f.ShowForceButton();
                    f.ShowDialog();

                    bitmap = f.GetResultBitmap();
                    if (bitmap == null)
                    {
                        return;
                    }
                }
            }

            uint index = (uint)this.AddressList.SelectedIndex;

            uint iconbaseaddr = Program.ROM.p32(Program.ROM.RomInfo.icon_pointer());
            uint size = 16 * 16 / 2; // /2しているのは16色のため
            uint addr = iconbaseaddr + (size * (0x100+index));

            byte[] image = ImageUtil.ImageToByte16Tile(bitmap, width, height);

            //画像等データの書き込み
            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            Program.ROM.write_range(U.toOffset(addr), image, undodata);
            Program.Undo.Push(undodata);

            InputFormRef.ReloadAddressList();
            InputFormRef.ShowWriteNotifyAnimation(this, addr);
        }
        static uint GetSkillPaletteAddress(uint pal)
        {
            if (pal == 0)
            {
                return Program.ROM.p32(Program.ROM.RomInfo.system_weapon_icon_palette_pointer());
            }
            else
            {
                return Program.ROM.p32(Program.ROM.RomInfo.icon_palette_pointer());
                //                return 0xA7429C;
            }
        }

        //スキル名を検索します.
        //残念ながら、スキルIDでの実装がされていないので文字列でヒントとかから検索します.
        public static uint FindSkillIconAndText(uint[] pointers, string searchSkillName, out string outText)
        {
            if (pointers == null || pointers.Length < 4)
            {
                outText = "";
                return U.NOT_FOUND;
            }

            InputFormRef InputFormRef = Init(null, pointers[4]);

            List<U.AddrResult> list = InputFormRef.MakeList();
            for (int n = 0; n < list.Count; n++)
            {
                uint addr = list[n].addr;
                if (list[n].name.IndexOf(searchSkillName) >= 0)
                {
                    outText = TextForm.Direct(Program.ROM.u16(addr + 0));
                    return (uint)(n + 0x100);
                }
            }

            outText = "";
            return U.NOT_FOUND;
        }

        public void JumpTo(uint id)
        {
            this.AddressList.SelectedIndex = (int)id;
        }


        public static int MakeClassSkillButtons(uint cid, Button[] buttons, ToolTipEx tooltip)
        {
            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                return 0;
            }
            if (pointer.Length < 5)
            {
                return 0;
            }

            InputFormRef InputFormRef = Init(null, pointer[4]);
            InputFormRef N2_InputFormRef = N2_Init(null);

            int skillCount = 0;
            List<U.AddrResult> list = InputFormRef.MakeList();
            for (int i = 1; i < list.Count; i++)
            {
                uint classAddr = Program.ROM.p32(list[i].addr + 8);
                if (! U.isSafetyOffset(classAddr))
                {
                    continue;
                }

                bool found = false;
                N2_InputFormRef.ReInit(classAddr);
                List<U.AddrResult> classList = N2_InputFormRef.MakeList();
                for (int n = 0; n < classList.Count; n++)
                {
                    uint classID = Program.ROM.u8(classList[n].addr);
                    if (cid == classID)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    continue;
                }

                uint textid = Program.ROM.u16(list[i].addr + 0);

                buttons[skillCount].BackgroundImage = DrawSkillIconLow((uint)i, pointer[4]);
                buttons[skillCount].Tag = (uint)i;
                tooltip.SetToolTipOverraide(buttons[skillCount], TextForm.Direct(textid));
                skillCount++;
                if (skillCount >= buttons.Length)
                {
                    break;
                }
            }

            return skillCount;
        }
        public static int MakeUnitSkillButtons(uint uid, Button[] buttons, ToolTipEx tooltip)
        {
            uint unitAddr = UnitForm.GetUnitAddr(uid - 1);
            if (unitAddr == U.NOT_FOUND)
            {
                return 0;
            }
            uint b38 = Program.ROM.u8(unitAddr + 38); //自軍
            uint b39 = Program.ROM.u8(unitAddr + 39); //友軍
            uint b49 = Program.ROM.u8(unitAddr + 49); //敵軍

            if (b38 <= 0 && b39 <= 0 && b49 <= 0)
            {//なし
                return 0;
            }

            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                return 0;
            }
            if (pointer.Length < 5)
            {
                return 0;
            }

            InputFormRef InputFormRef = Init(null, pointer[4]);
            InputFormRef N1_InputFormRef = N1_Init(null);

            int skillCount = 0;
            List<U.AddrResult> list = InputFormRef.MakeList();
            for (int i = 1; i < list.Count; i++)
            {
                if (b38 == i || b39 == i || b49 == i)
                {
                    uint textid = Program.ROM.u16(list[i].addr + 0);
                    string name = TextForm.Direct(textid);

                    if (b38 == i)
                    {
                        name += "\r\n" + R._("(自軍時のみ)");
                    }
                    if (b39 == i)
                    {
                        name += "\r\n" + R._("(敵軍時のみ)");
                    }
                    if (b49 == i)
                    {
                        name += "\r\n" + R._("(友軍時のみ)");
                    }

                    buttons[skillCount].BackgroundImage = DrawSkillIconLow((uint)i, pointer[4]);
                    buttons[skillCount].Tag = (uint)i;
                    tooltip.SetToolTipOverraide(buttons[skillCount], name);
                    skillCount++;
                    if (skillCount >= buttons.Length)
                    {
                        break;
                    }
                }
            }

            return skillCount;
        }

        private void ShowZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ShowZoomComboBox.SelectedIndex == 0)
            {
                AnimationPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                AnimationPictureBox.SizeMode = PictureBoxSizeMode.Normal;
            }
        }
        private void ShowFrameUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint anime = (uint)AnimePointer.Value;
            uint frame = (uint)ShowFrameUpDown.Value;
            AnimationPictureBox.Image = ImageUtilSkillSystemsAnimeCreator.Draw(anime, frame);
        }

        private void AnimationExportButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("スキルアニメスクリプト|*.txt|アニメGIF|*.gif|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, "skill_" + this.AddressList.Text.Trim());

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

            string ext = U.GetFilenameExt(save.FileName);
            if (ext == ".GIF")
            {
                ImageUtilSkillSystemsAnimeCreator.ExportGif(filename, (uint)AnimePointer.Value);
            }
            else
            {
                ImageUtilSkillSystemsAnimeCreator.Export(filename, (uint)AnimePointer.Value);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void AnimationImportButton_Click(object sender, EventArgs e)
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
                string filter = R._("スキルアニメスクリプト|*.txt|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                open.FileName = "skill_" + this.AddressList.Text.Trim();
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                if (!U.CanReadFileRetry(open))
                {
                    return;
                }
                filename = open.FileNames[0];
                Program.LastSelectedFilename.Save(this, "", open);
            }

            uint id = (uint)this.AddressList.SelectedIndex;
            string error = SkillAnimeImportDirect(id, filename);

            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }
        }

        public string SkillAnimeImportDirect(uint id, string filename)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return R._("現在他の処理中です");
            }

            if (id <= 0)
            {
                return R._("指定されたID({0})は存在しません。", U.To0xHexString(id));
            }
            uint animeP = AnimeBaseAddress + (4 * id);

            string error = "";

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                error = ImageUtilSkillSystemsAnimeCreator.Import(filename, animeP);
            }

            if (error != "")
            {
                return error;
            }

            U.ReSelectList(this.AddressList);
            //書き込み通知
            InputFormRef.ShowWriteNotifyAnimation(this, 0);

            return "";
        }


        private void X_N_JumpEditor_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            uint ID = (uint)AddressList.SelectedIndex;

            string filehint = AddressList.Text;

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            //テンポラリディレクトリを利用する
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                string filename = Path.Combine(tempdir.Dir, "anime.txt");
                ImageUtilSkillSystemsAnimeCreator.Export(filename, (uint)AnimePointer.Value);
                if (!File.Exists(filename))
                {
                    R.ShowStopError("アニメーションエディタを表示するために、アニメーションをエクスポートしようとしましたが、アニメをファイルにエクスポートできませんでした。\r\n\r\nファイル:{0}", filename);
                    return;
                }

                ToolAnimationCreatorForm f = (ToolAnimationCreatorForm)InputFormRef.JumpFormLow<ToolAnimationCreatorForm>();
                f.Init(ToolAnimationCreatorUserControl.AnimationTypeEnum.Skill
                    , ID, filehint, filename);
                f.Show();
            }
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (!U.isSafetyOffset(AnimeBaseAddress))
            {
                return ;
            }
            uint addr = AnimeBaseAddress + (4 * (uint)AddressList.SelectedIndex);
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "");
            Program.ROM.write_p32(addr, (uint)AnimePointer.Value, undodata);
            Program.Undo.Push(undodata);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            InputFormRef ifr;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.FE8N_ver2)
            {
                return;
            }

            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                return;
            }
            if (pointer.Length < 5)
            {
                return;
            }
            if (!U.isSafetyOffset(pointer[4]))
            {
                return;
            }

            ifr = Init(null, pointer[4]);
            FEBuilderGBA.Address.AddAddress(list, ifr, "SkillConfigFE8NVer2", new uint[] { 4, 8, 12 });

            if (pointer.Length > 5)
            {
                uint anime = Program.ROM.p32(pointer[5]);
                for (uint i = 0; i < ifr.DataCount; i++, anime += 4)
                {
                    if (!U.isSafetyOffset(anime))
                    {
                        break;
                    }
                    uint addr = Program.ROM.p32(anime);
                    if (!U.isSafetyOffset(addr))
                    {
                        continue;
                    }
                    string name = "SkillAnime:" + U.To0xHexString(i) + " ";
                    FEBuilderGBA.Address.AddAddress(list, addr, 0, anime, name, FEBuilderGBA.Address.DataTypeEnum.POINTER);

                    ImageUtilSkillSystemsAnimeCreator.RecycleOldAnime(ref list
                        , name
                        , isPointerOnly
                        , addr);
                }
            }

            {
                InputFormRef ifr_n1 = N1_Init(null);
                InputFormRef ifr_n2 = N2_Init(null);
                InputFormRef ifr_n3 = N3_Init(null);
                uint icondatacount = (2 * 8 * 2 * 8) / 2; // /2しているのは16色のため

                uint icon = Program.ROM.p32(pointer[4]);
                uint addr = ifr.BaseAddress;
                for (uint i = 0; i < ifr.DataCount;
                    i++, addr += ifr.BlockSize , icon += icondatacount)
                {
                    if (!U.isSafetyOffset(addr))
                    {
                        break;
                    }
                    ifr_n1.ReInitPointer(addr + 4);
                    ifr_n2.ReInitPointer(addr + 8);
                    ifr_n3.ReInitPointer(addr + 12);

                    FEBuilderGBA.Address.AddAddress(list,ifr_n1 
                        ,  "SkillUnit:" + U.To0xHexString(i)
                        , new uint[] {} );
                    FEBuilderGBA.Address.AddAddress(list, ifr_n2
                        , "SkillClass:" + U.To0xHexString(i)
                        , new uint[] { });
                    FEBuilderGBA.Address.AddAddress(list, ifr_n3
                        , "SkillUnit:" + U.To0xHexString(i)
                        , new uint[] { });

                    FEBuilderGBA.Address.AddAddress(list, icon
                        , icondatacount
                        , U.NOT_FOUND
                        , "SkillIcon:" + U.To0xHexString(i)
                        , FEBuilderGBA.Address.DataTypeEnum.IMG );
                }
            }
        }

        //テキストの取得
        public static void MakeTextIDArray(List<UseTextID> list)
        {
            InputFormRef ifr;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.FE8N_ver2)
            {
                return;
            }

            uint[] pointer = FindSkillFE8NVer2IconPointers();
            if (pointer == null)
            {
                return;
            }
            if (pointer.Length < 5)
            {
                return;
            }

            ifr = Init(null, pointer[4]);
            UseTextID.AppendTextID(list, FELint.Type.SKILL_CONFIG, ifr, new uint[] { 0 });
        }


    }
}
