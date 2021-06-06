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
    public partial class SkillConfigFE8NSkillForm : Form
    {
        public SkillConfigFE8NSkillForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(DrawSkillAndText, DrawMode.OwnerDrawFixed);

            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            if (skill == PatchUtil.skill_system_enum.FE8N 
                || skill == PatchUtil.skill_system_enum.yugudora)
            {
                this.Pointers = FindSkillFE8NVer1IconPointers();
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver3)
            {
                this.Pointers = SkillConfigFE8NVer3SkillForm.FindSkillFE8NVer3IconPointers();
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {
                this.Pointers = SkillConfigFE8NVer2SkillForm.FindSkillFE8NVer2IconPointers();
            }
            else
            {
                return;
            }

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.CheckProtectionPaddingALIGN4 = false;

            this.FilterComboBox.BeginUpdate();
            for (int i = 0; i < this.Pointers.Length; i++)
            {
                uint p = Program.ROM.p32(this.Pointers[i]);
                this.FilterComboBox.Items.Add(U.ToHexString(p));
            }
            this.FilterComboBox.EndUpdate();
            this.FilterComboBox.SelectedIndex = 0;
        }

        uint[] Pointers;


        public static void ClearCache()
        {
            g_Cache_Pointers = null;
        }
        static uint[] g_Cache_Pointers = null;
        public static uint[] FindSkillFE8NVer1IconPointers()
        {
            if (g_Cache_Pointers == null)
            {
                g_Cache_Pointers = FindSkillFE8NVer1IconPointersLow();
            }
            return g_Cache_Pointers;
        }
        static uint[] FindSkillFE8NVer1IconPointersLow()
        {
            uint iconExPointer = Program.ROM.u32(0x89268 + 4);
            if (!U.isSafetyPointer(iconExPointer))
            {
                return null;
            }
            iconExPointer = U.toOffset(iconExPointer);

            byte[] need = new byte[] { 0xF0, 0x40, 0x00, 0x02, 0x00, 0x3B, 0x00, 0x02 };
            uint iconPointers = U.Grep(Program.ROM.Data, need, 0xE00000,0, 4);
            if (iconPointers == U.NOT_FOUND)
            {
                return null;
            }
            iconPointers = iconPointers + (uint)need.Length +  4 + 4 + 4;
            List<uint> pointer = new List<uint>();
            for(uint p = iconPointers ; true ; p+= 4 )
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
            return pointer.ToArray();
        }

        public static string ParseTextToSkillName(string text)
        {
            string s = "『";///No Translate
            string e = "』";///No Translate

            int textstart = text.IndexOf(s);
            if (textstart < 0)
            {
                return "";
            }
            textstart += s.Length;
            int textend = text.IndexOf(e, textstart);
            if (textend < 0)
            {
                return "";
            }

            return U.substr(text, textstart, textend - textstart);
        }

        //スキル名を検索します.
        //残念ながら、スキルIDでの実装がされていないので文字列でヒントとかから検索します.
        public static uint FindSkillIconAndText(uint[] pointers, string searchSkillName, out string outText)
        {
            if (pointers == null)
            {
                outText = "";
                return U.NOT_FOUND;
            }

            InputFormRef InputFormRef = Init(null);
            for (int i = 0 ; i < pointers.Length ; i ++)
            {
                InputFormRef.ReInitPointer(pointers[i]);
                List<U.AddrResult> list = InputFormRef.MakeList();
                for (int n = 0; n < list.Count ; n++)
                {
                    uint addr = list[n].addr;
                    String name = Program.ROM.getString(addr+8, 12);
                    if (name.IndexOf(searchSkillName) >= 0)
                    {
                        outText = TextForm.Direct(Program.ROM.u16(addr + 2));
                        uint icon = Program.ROM.u16(addr + 0);
                        return icon;
                    }
                }
                for (int n = 0; n < list.Count; n++)
                {
                    uint addr = list[n].addr;
                    String text = TextForm.Direct(Program.ROM.u16(addr + 2));
                    if (text.IndexOf(searchSkillName) >= 0)
                    {
                        outText = text;
                        uint icon = Program.ROM.u16(addr + 0);
                        return icon;
                    }
                }
            }
            
            outText = "";
            return U.NOT_FOUND;
        }


        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , new List<String> { "N00_", "N01_", "N02_", "N03_"}
                , 0
                , 32
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint pp = Program.ROM.u16(addr);
                    if (pp == 0xFFFF)
                    {
                        return false;
                    }
                    if (pp == 0x0)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    String name = Program.ROM.getString(addr+4, 12);
                    uint c = Program.ROM.u8(addr+4);
                    if (name.Length <= 0 || c == 0xFF || c == 0x00)
                    {
                        String text = TextForm.Direct(Program.ROM.u16(addr + 2));
                        string parse_name = ParseTextToSkillName(text);
                        if (parse_name == "" && name.Length > 0)
                        {
                            parse_name = name.Trim(new char[]{
                                '\x0','\xFF',' ','　',''///No Translate
                            });
                        }
                        name = parse_name;
                    }
                    return U.ToHexString(i) + " " + name;
                }
                );
        }

        private void SkillNameFE8NSkillForm_Load(object sender, EventArgs e)
        {

        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterComboBox.SelectedIndex == 0)
            {//最初はユニットとして解釈することが多い気がする.
                ChangeType.SelectedIndex = 0;
            }
            else if (FilterComboBox.SelectedIndex == 1)
            {//2番目はクラスとして解釈することが多い気がする.
                ChangeType.SelectedIndex = 1;
            }
            InputFormRef.ReInit(U.atoh(this.FilterComboBox.Text));
        }

        //Skill + テキストを書くルーチン
        public static Size DrawSkillAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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
                uint icon = Program.ROM.u16(addr + 0);
                bitmap = ImageItemIconForm.DrawIconWhereID_UsingWeaponPalette_SKILLFE8NVer2(icon);
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

        private void ChangeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UNIONKEY.Value = ChangeType.SelectedIndex;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            uint[] pointer;

            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            if (skill == PatchUtil.skill_system_enum.FE8N
                || skill == PatchUtil.skill_system_enum.yugudora)
            {
                pointer = FindSkillFE8NVer1IconPointers();
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver3)
            {//ver3では利用しません
                return;
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {
                pointer = SkillConfigFE8NVer2SkillForm.FindSkillFE8NVer2IconPointers();
            }
            else
            {
                return;
            }

            if (pointer == null)
            {
                return;
            }

            InputFormRef ifr = Init(null);
            for (int i = 0; i < pointer.Length; i++)
            {
                ifr.ReInitPointer(pointer[i]);
                if (ifr.DataCount <= 0)
                {
                    continue;
                }
                FEBuilderGBA.Address.AddAddress(list, ifr
                    , "SkillConfigFE8N" + U.ToHexString(i)
                    , new uint[] { });
            }
        }

        //テキストの取得
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            uint[] pointer;

            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            if (skill == PatchUtil.skill_system_enum.FE8N
                || skill == PatchUtil.skill_system_enum.yugudora)
            {
                pointer = FindSkillFE8NVer1IconPointers();
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver3)
            {//ver3では利用しません
                return;
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {
                pointer = SkillConfigFE8NVer2SkillForm.FindSkillFE8NVer2IconPointers();
            }
            else
            {
                return;
            }

            if (pointer == null)
            {
                return;
            }

            InputFormRef ifr = Init(null);
            for (int i = 0; i < pointer.Length; i++)
            {
                ifr.ReInitPointer(pointer[i]);
                if (ifr.DataCount <= 0)
                {
                    continue;
                }
                UseValsID.AppendTextID(list, FELint.Type.SKILL_CONFIG, ifr, new uint[] { 2 });
            }
        }

    }
}
