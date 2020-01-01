using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class TextScriptFormCategorySelectForm : Form
    {
        public class TextEscape
        {
            public string Code;     //@0003
            public string Info;     //改行します[A]
            public string Category; //{LINEBREAK}
        };

        public TextScriptFormCategorySelectForm()
        {
            InitializeComponent();

            this.CategoryListBox.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed, false);
            this.ListBox.OwnerDraw(DrawTextCategory, DrawMode.OwnerDrawFixed, false);
        }
        public void Init(bool isDetail)
        {
            this.IsDetail = isDetail;
        }

        void LoadTextEscapeList()
        {
            this.EscapeList = new List<TextEscape>();
            string[] lines = File.ReadAllLines(U.ConfigDataFilename("text_escape_"));
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
                if (sp.Length < 3)
                {
                    continue;
                }
                TextEscape te = new TextEscape();
                te.Code = sp[0];
                te.Info = sp[1];
                te.Category = sp[2];

                if (!IsDetail)
                {//詳細テキストじゃないと、移動とロードを出さない.
                    if (IsDetailOnly(te.Category))
                    {
                        continue;
                    }
                }

                this.EscapeList.Add(te);
            }

            //パッチで追加される新しいエスケープシーケンス
            Program.TextEscape.MargeExstraEscapeList(this.EscapeList);
        }

        bool IsDetail;
        List<TextEscape> EscapeList;
        Dictionary<string, string> CategoryDic;
        ToolTipEx ToolTip;
        private void EventScriptFormCategorySelectForm_Load(object sender, EventArgs e)
        {
            LoadTextEscapeList();

            this.CategoryDic = U.LoadTSVResourcePair((U.ConfigDataFilename("text_category_")));
            this.CategoryListBox.BeginUpdate();
            this.CategoryListBox.Items.Clear();
            foreach (var pair in CategoryDic)
            {
                if (!IsDetail)
                {//詳細テキストじゃないと、移動とロードを出さない.
                    if (IsDetailOnly(pair.Value))
                    {
                        continue;
                    }
                }
                this.CategoryListBox.Items.Add(pair.Key);
            }
            this.CategoryListBox.EndUpdate();
            CategoryListBox.SelectedIndex = 0;

            this.ToolTip = InputFormRef.GetToolTip<TextScriptFormCategorySelectForm>();
        }

        bool IsDetailOnly(string category)
        {
            if (category == "{MOVE_LOAD}" || category == "{POSITION}")
            {
                return true;
            }
            return false;
        }

        public static Size DrawTextCategory(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;

            string[] sp = text.Split(new string[] { "[", "//" }, StringSplitOptions.None);
            bounds.X += U.DrawText(sp[0], g, normalFont, brush, isWithDraw, bounds);
            for (int i = 1; i < sp.Length - 1; i++)
            {
                bounds.X += U.DrawText("[", g, normalFont, brush, isWithDraw, bounds);
                bounds.X += U.DrawText(sp[i], g, normalFont, brush, isWithDraw, bounds);
            }

            //コードをコメント形式で描画
            if (sp.Length > 0)
            {
                int i = sp.Length - 1;
                SolidBrush commentBrush = new SolidBrush(OptionForm.Color_Comment_ForeColor());
                if (bounds.X < 500)
                {
                    bounds.X = 500;
                }
                bounds.X += U.DrawText("//" + sp[i], g, normalFont, commentBrush, isWithDraw, bounds);
                commentBrush.Dispose();
            }
            brush.Dispose();
            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }

        string makeCommandComboText(TextEscape script)
        {
            return script.Info + "//" + script.Code;
        }

        List<TextEscape> ScriptCahce = new List<TextEscape>();
        private void CategoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = GetSelectedCategory();
            bool filtered = category.Length > 0;

            string lang = OptionForm.lang();
            bool isJP = (lang == "ja");
            string filterString = U.CleanupFindString(this.Filter.Text, isJP);

            this.ListBox.BeginUpdate();
            this.ListBox.Items.Clear();
            this.ScriptCahce.Clear();
            foreach (TextEscape script in this.EscapeList)
            {
                if (filtered == true)
                {
                    if (script.Category.IndexOf(category) < 0)
                    {
                        continue;
                    }
                }
                string name = makeCommandComboText(script);
                if (filterString.Length > 0)
                {
                    if(! U.StrStrEx(name,filterString,isJP) )
                    {//フィルターで消す.
                        continue;
                    }
                }

                this.ListBox.Items.Add(name);
                this.ScriptCahce.Add(script);
            }
            this.ListBox.EndUpdate();

        }
        public TextEscape Script { get; private set; }
        bool UpdateSelected()
        {
            string text = this.ListBox.Text;
            if (text.Length <= 0)
            {
                return false;
            }

            foreach (TextEscape script in this.EscapeList)
            {
                if (makeCommandComboText(script) == text)
                {
                    this.Script = script;
                    return true;
                }
            }

            return false;
        }

        string GetSelectedCategory()
        {
            if (this.CategoryDic.ContainsKey(CategoryListBox.Text))
            {
                string value = this.CategoryDic[CategoryListBox.Text];
                if (value == "{}")
                {
                    return "";
                }
                return value;
            }
            return "";
        }


        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (UpdateSelected())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
        private void ListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectButton_Click(null,null);
        }

        private void Filter_TextChanged(object sender, EventArgs e)
        {
            CategoryListBox_SelectedIndexChanged(null, null);
        }


        int LastHoverIndex = -1;
        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int hoverindex = lb.IndexFromPoint(e.Location);
            if (hoverindex < 0 || hoverindex > lb.Items.Count)
            {//範囲外
                return;
            }
            if (this.LastHoverIndex == hoverindex)
            {//最後に表示していた行と同じ
                return;
            }
            this.LastHoverIndex = hoverindex;
            this.ToolTip.HideEvent(sender, e);
/*
            string listtext = lb.Items[hoverindex].ToString();
            foreach (TextEscape script in this.EscapeList)
            {
                if (makeCommandComboText(script) == listtext)
                {
                    if (script.PopupHint == "")
                    {
                        break;
                    }
                    
                    Rectangle rc = lb.GetItemRectangle(hoverindex);
                    this.ToolTip.FireListEvent(lb, rc, listtext + "\r\n" + script.PopupHint);

                    return;
                }
            }
*/
        }

        private void ListBox_MouseLeave(object sender, EventArgs e)
        {
            this.ToolTip.HideEvent(sender, e);
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectButton_Click(null, null);
                return;
            }
            if (e.KeyCode == Keys.Up && this.ListBox.SelectedIndex == 0)
            {
                Filter.Focus();
                return;
            }
            if (e.KeyCode == Keys.Back)
            {
                Filter.Focus();
                return;
            }
            if (e.KeyCode == Keys.Left)
            {
                CategoryListBox.Focus();
                return;
            }
        }

        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                U.SelectedIndexSafety(ListBox, 0, true);
                return;
            }
            if (Filter.Text == "" && e.KeyCode == Keys.Left)
            {
                CategoryListBox.Focus();
                return;
            }
        }
        private void CategoryListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                Filter.Focus();
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                ListBox.Focus();
                return;
            }
        }


        private void Filter_DoubleClick(object sender, EventArgs e)
        {
            Filter.Clear();
            Filter_TextChanged(sender, e);
        }

        private void EventScriptFormCategorySelectForm_Shown(object sender, EventArgs e)
        {
            this.Filter.Focus();
        }

    }
}
