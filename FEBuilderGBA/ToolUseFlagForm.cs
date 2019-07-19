using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolUseFlagForm : Form
    {
        public ToolUseFlagForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(Draw, DrawMode.OwnerDrawVariable);

            //マップIDリストを作る.
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref  this.MAP_LISTBOX);
            this.MAP_LISTBOX.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
        }

        private void ToolUseFlagForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
            U.SelectedIndexSafety(this.MAP_LISTBOX, 0);
        }
        public void JumpToMAPID(uint mapid)
        {
            U.SelectedIndexSafety(MAP_LISTBOX, mapid);
        }

        List<UseFlagID> FlagList = new List<UseFlagID>();
        void UpdateList()
        {
            this.FlagList.Clear();
            uint mapid = (uint)this.MAP_LISTBOX.SelectedIndex;
            EventCondForm.MakeFlagIDArray(mapid, this.FlagList);
            MapChangeForm.MakeFlagIDArray(mapid, this.FlagList);

            List<UseFlagID> flagListInner = new List<UseFlagID>();
            if (Program.ROM.RomInfo.version() == 8)
            {
                EventHaikuForm.MakeFlagIDArray(flagListInner);
                EventBattleTalkForm.MakeFlagIDArray(flagListInner);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {//7
                EventHaikuFE7Form.MakeFlagIDArray(flagListInner);
                EventBattleTalkFE7Form.MakeFlagIDArray(flagListInner);
            }
            else
            {//6
                EventHaikuFE6Form.MakeFlagIDArray(flagListInner);
                EventBattleTalkFE6Form.MakeFlagIDArray(flagListInner);
            }

            if (this.ShowANYCheckBox.Checked)
            {
                foreach (UseFlagID u in flagListInner)
                {
                    if (u.MapID == mapid || u.MapID == U.NOT_FOUND)
                    {
                        this.FlagList.Add(u);
                    }
                }
            }
            else
            {
                foreach (UseFlagID u in flagListInner)
                {
                    if (u.MapID == mapid)
                    {
                        this.FlagList.Add(u);
                    }
                }
            }


            this.FlagList.Sort((UseFlagID a, UseFlagID b) => 
            {
                if (a.ID == b.ID)
                {
                    if (a.MapID == b.MapID)
                    {
                        return (int)a.DataType - (int)b.DataType;
                    }
                    return (int)a.MapID - (int)b.MapID;
                }
                return (int)a.ID - (int)b.ID; 
            });
        }

        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateList();
            AddressList.DummyAlloc(this.FlagList.Count, this.AddressList.SelectedIndex);
        }

        public Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            SolidBrush brush = new SolidBrush(lb.ForeColor);
            SolidBrush foreKeywordBrush = new SolidBrush(OptionForm.Color_Keyword_ForeColor());
            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);
            Rectangle bounds = listbounds;
            int lineHeight = normalFont.Height;

            UseFlagID current = this.FlagList[index];
            string text;
            int maxWidth = 0;

            if (index == 0 || current.ID != this.FlagList[index - 1].ID)
            {//フラグ名を描画
                if (index != 0)
                {//空行を入れる
                    bounds.Y += lineHeight;
                }

                //フラグのアイコンを描画
                Bitmap bitmap = ImageSystemIconForm.FlagIcon();
                U.MakeTransparent(bitmap);

                Rectangle b = bounds;
                b.Width = lineHeight;
                b.Height = lineHeight;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                //フラグ名を書く
                string dummy;
                text = ":";
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
                
                text = U.ToHexString(current.ID);
                bounds.X += U.DrawText(text, g, boldFont, brush, isWithDraw, bounds);

                text = "  " + InputFormRef.GetFlagName(current.ID, out dummy);
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                //次の行へ
                maxWidth = bounds.X;
                bounds.Y += lineHeight;
                bounds.X = listbounds.X;
            }

            //名称の表示
            bounds.X += 10;
            text = MainSimpleMenuEventErrorForm.TypeToString(current.DataType, current.Addr, current.Tag);
            bounds.X += U.DrawText(text, g, boldFont, foreKeywordBrush, isWithDraw, bounds);

            //次の行へ
            maxWidth = Math.Max(bounds.X, maxWidth);
            bounds.Y += lineHeight;
            bounds.X = listbounds.X;

            //情報を書く.
            bounds.X += 6;
            text = current.Info;

            Size ss;
            if (current.DataType == FELint.Type.BATTTLE_TALK || current.DataType == FELint.Type.HAIKU)
            {
                ss = DrawUnitAllowToAllow(text, lb, g, bounds, isWithDraw);
            }
            else if (current.DataType == FELint.Type.EVENT_COND_ALWAYS)
            {
                ss = EventCondForm.DrawEventListAlwaysOneLiner(current.Addr, lb, g, bounds, isWithDraw);
            }
            else if (current.DataType == FELint.Type.EVENT_COND_OBJECT)
            {
                ss = EventCondForm.DrawEventListObjectOneLiner(current.Addr, lb, g, bounds, isWithDraw);
            }
            else if (current.DataType == FELint.Type.EVENT_COND_TALK)
            {
                ss = EventCondForm.DrawEventListTalkOneLiner(current.Addr, lb, g, bounds, isWithDraw);
            }
            else if (current.DataType == FELint.Type.EVENT_COND_TURN)
            {
                ss = EventCondForm.DrawEventListTurnOneLiner(current.Addr, lb, g, bounds, isWithDraw);
            }
            else if (current.DataType == FELint.Type.EVENTSCRIPT)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, current.Tag);
                ss = EventScriptForm.DrawCode(lb, g, bounds, isWithDraw, code);
            }
            else
            {
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
                bounds.Y += lineHeight;
                ss = new Size(bounds.X, bounds.Y);
            }
            bounds.X = ss.Width;
            bounds.Y = ss.Height;

            brush.Dispose();
            foreKeywordBrush.Dispose();

            //最後の改行
            maxWidth = Math.Max(bounds.X, maxWidth);
            return new Size(maxWidth, bounds.Y);
        }
        Size DrawUnitAllowToAllow(string text, ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = UnitForm.DrawUnitMapFacePicture(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = lineHeight;
            b.Height = lineHeight;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();

            string[] parsers = new string[] { " -> ", " <- ", " ?? ", " & " };
            bool found = false;
            foreach (string parser in parsers)
            {
                int p = text.IndexOf(parser);
                if (p < 0)
                {
                    continue;
                }

                p += parser.Length;

                //前半
                string a1 = text.Substring(0, p);
                b = bounds;
                bounds.X += U.DrawText(a1, g, normalFont, brush, isWithDraw, b);

                //後半
                string a2 = text.Substring(p);

                //後半のアイコンを描く
                icon = U.atoh(a2);
                bitmap = UnitForm.DrawUnitMapFacePicture(icon);
                U.MakeTransparent(bitmap);

                b = bounds;
                b.Width = lineHeight;
                b.Height = lineHeight;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                //後半の文字列
                b = bounds;
                bounds.X += U.DrawText(a2, g, normalFont, brush, isWithDraw, b);

                found = true;
                break;
            }

            if (found == false)
            {
                //見つからなかったので、普通にテキストを描く.
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }

            brush.Dispose();

            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }


        private void ShowANYCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            U.ReSelectList(this.MAP_LISTBOX);
        }

        private void AddressList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.AddressList.SelectedIndex;
            if (index < 0 || index >= this.AddressList.Items.Count)
            {
                return ;
            }

            UseFlagID current = this.FlagList[index];
            MainSimpleMenuEventErrorForm.GotoEvent(current.DataType, current.Addr, current.Tag, current.MapID);
        }
    }
}
