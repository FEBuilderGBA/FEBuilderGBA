using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FEBuilderGBA
{
    public partial class SkillConfigFE8NVer3SkillForm : Form
    {
        public SkillConfigFE8NVer3SkillForm()
        {
            InitializeComponent();

            uint[] pointer = FindSkillFE8NVer3IconPointers();
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
            Debug.Assert(g_SkillBaseAddress != 0);

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

            N4_InputFormRef = N4_Init(this);
            N4_InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent_N4;
            N4_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N4_AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            N5_InputFormRef = N5_Init(this);
            N5_InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent_N4;
            N5_InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N4_AddressList.OwnerDraw(ListBoxEx.DrawSkillAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(false);
            this.AddressList.OwnerDraw(DrawSkillAndText, DrawMode.OwnerDrawFixed);

            MainTab.SelectedIndex = 1;
            U.SetIcon(ImportButton, Properties.Resources.icon_upload);
            U.SetIcon(ExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(AnimationInportButton, Properties.Resources.icon_upload);
            U.SetIcon(AnimationExportButton, Properties.Resources.icon_arrow);
            InputFormRef.markupJumpLabel(X_JUMP_TO_COMBAT_ART);

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
            if (! U.isSafetyOffset(g_AnimeBaseAddress) )
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

        static uint g_SkillBaseAddress = 0;
        static uint g_AnimeBaseAddress = 0;
        static uint g_ICON_LIST_SIZE = 0;

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self )
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , g_SkillBaseAddress
                , g_ICON_LIST_SIZE
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

        InputFormRef N4_InputFormRef;
        static InputFormRef N4_Init(Form self)
        {
            return new InputFormRef(self
                , "N4_"
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

        InputFormRef N5_InputFormRef;
        static InputFormRef N5_Init(Form self)
        {
            return new InputFormRef(self
                , "N5_"
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
                    uint skillid = Program.ROM.u8(addr);
                    return U.ToHexString(skillid) + " " + GetSkillText(skillid);
                }
            );
        }


        public static void ClearCache()
        {
            g_Cache_IconPointers = null;
        }
        static uint[] g_Cache_IconPointers = null;
        public static uint[] FindSkillFE8NVer3IconPointers()
        {
            if (g_Cache_IconPointers == null)
            {
                g_Cache_IconPointers = FindSkillFE8NVer3IconPointersLow();
            }
            return g_Cache_IconPointers;
        }
        static uint[] FindSkillFE8NVer3IconPointersLow()
        {
            //下位互換性のためポインタリストを返す.
            uint[] pointer = new uint[5];

            uint iconExPointer = Program.ROM.u32(0x89268 + 4);
            if (!U.isSafetyPointer(iconExPointer))
            {
                return null;
            }
            pointer[0] = iconExPointer;
            iconExPointer = U.toOffset(iconExPointer);

            uint skl_table = Program.ROM.u32(0x892A8 + 4);
            if (!U.isSafetyPointer(skl_table))
            {
                return null;
            }
            pointer[4] = 0x892A8 + 4;
            g_SkillBaseAddress = 0x892A8 + 4;

            g_ICON_LIST_SIZE = Program.ROM.u32(0x892A8 + 8);
            if (g_ICON_LIST_SIZE <= 0 || g_ICON_LIST_SIZE > 100)
            {
                return null;
            }

            uint skl_anime_table = Program.ROM.u32(0x892A8 + 20); //892BC
            if (U.isSafetyPointer(skl_anime_table))
            {
                g_AnimeBaseAddress = U.toOffset(skl_anime_table);
            }

            return pointer;
        }


        private void P16_ValueChanged(object sender, EventArgs e)
        {
            N4_InputFormRef.ReInit((uint)P16.Value);
            N4_ZeroPointerPanel.Visible = InputFormRef.ShowZeroPointerPanel(this.AddressList, this.P12);
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
            uint[] pointer = FindSkillFE8NVer3IconPointers();
            if (pointer == null)
            {
                return "";
            }
            InputFormRef InputFormRef = Init(null);
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
            uint[] pointer = FindSkillFE8NVer3IconPointers();
            if (pointer == null)
            {
                return ImageUtil.BlankDummy();
            }
            if (g_SkillBaseAddress == 0)
            {
                return ImageUtil.BlankDummy();
            }

            return DrawSkillIconLow(id);
        }

        static Bitmap DrawSkillIconLow(uint id)
        {
            InputFormRef ifr = Init(null);
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
            if (g_SkillBaseAddress == 0)
            {
                return;
            }

            SKILLICON.Image = DrawSkillIconLow((uint)AddressList.SelectedIndex);
            DrawAnime();
        }

        void DrawAnime()
        {
            if (U.isSafetyOffset(g_AnimeBaseAddress))
            {
                uint animePointer = g_AnimeBaseAddress + (4 * (uint)AddressList.SelectedIndex);

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

        public static bool IsFE8NVer3()
        {
            uint[] pointer = FindSkillFE8NVer3IconPointers();
            if (pointer == null)
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
                bitmap = DrawSkillIconLow((uint)index);
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
            Bitmap bitmap = bitmap = DrawSkillIconLow((uint)AddressList.SelectedIndex);
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
        private void AddressListExpandsEvent_N4(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;

            P16.Value = addr;
            WriteButton.PerformClick();

            N4_ReadCount.Value = eearg.NewDataCount;
            N4_InputFormRef.ReInit(addr, eearg.NewDataCount);
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

            InputFormRef InputFormRef = Init(null);

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
            U.SelectedIndexSafety(this.AddressList, id);
        }


        public static int MakeClassSkillButtons(uint cid, Button[] buttons, ToolTipEx tooltip)
        {
            uint[] pointer = FindSkillFE8NVer3IconPointers();
            if (pointer == null)
            {
                return 0;
            }
            if (pointer.Length < 5)
            {
                return 0;
            }

            InputFormRef InputFormRef = Init(null);
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

                buttons[skillCount].BackgroundImage = DrawSkillIconLow((uint)i);
                buttons[skillCount].Tag = (uint)i;
                tooltip.SetToolTip(buttons[skillCount], TextForm.Direct(textid));
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

            uint[] pointer = FindSkillFE8NVer3IconPointers();
            if (pointer == null)
            {
                return 0;
            }
            if (g_SkillBaseAddress == 0)
            {
                return 0;
            }

            InputFormRef InputFormRef = Init(null);
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

                    buttons[skillCount].BackgroundImage = DrawSkillIconLow((uint)i);
                    buttons[skillCount].Tag = (uint)i;
                    tooltip.SetToolTip(buttons[skillCount], name);
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
            string filter = R._("スキルアニメスクリプト|*.txt|アニメGIF|*.gif|Dump All|*.txt|All files|*");

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

            if (save.FilterIndex == 2)
            {//GIF
                ImageUtilSkillSystemsAnimeCreator.ExportGif(filename, (uint)AnimePointer.Value);
            }
            else if (save.FilterIndex == 3)
            {//All
                ImageUtilSkillSystemsAnimeCreator.Export(filename, (uint)AnimePointer.Value);
                filename = U.ChangeExtFilename(filename, ".gif");
                ImageUtilSkillSystemsAnimeCreator.ExportGif(filename, (uint)AnimePointer.Value);
            }
            else
            {//Script
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
            uint animeP = g_AnimeBaseAddress + (4 * id);

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
            if (!U.isSafetyOffset(g_AnimeBaseAddress))
            {
                return ;
            }
            uint addr = g_AnimeBaseAddress + (4 * (uint)AddressList.SelectedIndex);
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "");
            Program.ROM.write_p32(addr, (uint)AnimePointer.Value, undodata);
            Program.Undo.Push(undodata);
        }


        //全データの取得
        public static void MakeAllDataLength(List<Address> list, bool isPointerOnly)
        {
            InputFormRef ifr;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.FE8N_ver3)
            {
                return;
            }

            uint[] pointer = FindSkillFE8NVer3IconPointers();
            if (pointer == null)
            {
                return;
            }
            if (g_SkillBaseAddress == 0)
            {
                return;
            }
            if (!U.isSafetyOffset(g_SkillBaseAddress))
            {
                return;
            }

            ifr = Init(null);
            FEBuilderGBA.Address.AddAddress(list, ifr, "SkillConfigFE8NVer3", new uint[] { 4, 8, 12 , 16 , 20});

            if (g_AnimeBaseAddress != 0)
            {
                uint anime = Program.ROM.p32(g_AnimeBaseAddress);
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
                InputFormRef ifr_n4 = N4_Init(null);
                InputFormRef ifr_n5 = N5_Init(null);
                uint icondatacount = (2 * 8 * 2 * 8) / 2; // /2しているのは16色のため

                uint icon = Program.ROM.p32(g_SkillBaseAddress);
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
                    ifr_n4.ReInitPointer(addr + 16);
                    ifr_n5.ReInitPointer(addr + 20);

                    FEBuilderGBA.Address.AddAddress(list,ifr_n1 
                        ,  "SkillUnit:" + U.To0xHexString(i)
                        , new uint[] {} );
                    FEBuilderGBA.Address.AddAddress(list, ifr_n2
                        , "SkillClass:" + U.To0xHexString(i)
                        , new uint[] { });
                    FEBuilderGBA.Address.AddAddress(list, ifr_n3
                        , "SkillItem:" + U.To0xHexString(i)
                        , new uint[] { });
                    FEBuilderGBA.Address.AddAddress(list, ifr_n4
                        , "SkillItem2:" + U.To0xHexString(i)
                        , new uint[] { });
                    FEBuilderGBA.Address.AddAddress(list, ifr_n5
                        , "SkillComplex:" + U.To0xHexString(i)
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
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef ifr;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.FE8N_ver3)
            {
                return;
            }

            uint[] pointer = FindSkillFE8NVer3IconPointers();
            if (pointer == null)
            {
                return;
            }
            if (g_SkillBaseAddress == 0)
            {
                return;
            }

            ifr = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.SKILL_CONFIG, ifr, new uint[] { 0 });
        }

        private void X_JUMP_TO_COMBAT_ART_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpToSelectStruct("FE8N SKILL COMBAT ART", 0, this.AddressList.SelectedIndex);
        }

        private void P20_ValueChanged(object sender, EventArgs e)
        {
            N5_InputFormRef.ReInit((uint)P20.Value);
            N5_ZeroPointerPanel.Visible = InputFormRef.ShowZeroPointerPanel(this.AddressList, this.P12);
        }

        private void SkillConfigFE8NVer3SkillForm_Load(object sender, EventArgs e)
        {
        }

    }
}
