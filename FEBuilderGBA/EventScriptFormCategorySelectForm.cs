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
    public partial class EventScriptFormCategorySelectForm : Form
    {
        Dictionary<string, string> CategoryDic;
        public EventScriptFormCategorySelectForm()
        {
            InitializeComponent();

            this.EventTemplate = new EventTemplateImpl();
            this.EventTemplate.LoadTemplate();

            this.CategoryDic = U.LoadTSVResourcePair((U.ConfigDataFilename("event_category_")));
            this.CategoryListBox.BeginUpdate();
            this.CategoryListBox.Items.Clear();
            foreach (var pair in CategoryDic)
            {
                this.CategoryListBox.Items.Add(pair.Key);
            }
            this.CategoryListBox.EndUpdate();
            CategoryListBox.SelectedIndex = 0;

            this.CategoryListBox.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed, false);
            this.ListBox.OwnerDraw(ListBoxEx.DrawEventCategory, DrawMode.OwnerDrawFixed, false);
        }
        EventTemplateImpl EventTemplate;

        private void CategoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = GetSelectedCategory();
            bool filtered = category.Length > 0;

            string lang = OptionForm.lang();
            bool isJP = (lang == "ja");
            string filterString = U.CleanupFindString(this.Filter.Text, isJP);

            bool showLowCommand = DisplayLowCommandCheckBox.Checked;

            this.ListBox.BeginUpdate();
            this.ListBox.Items.Clear();
            foreach (EventScript.Script script in Program.EventScript.Scripts)
            {
                if (filtered == true)
                {
                    if (script.Category.IndexOf(category) < 0)
                    {
                        continue;
                    }
                }
                string name = EventScript.makeCommandComboText(script,true);
                if (filterString.Length > 0)
                {
                    if (!U.StrStrEx(name + " " + script.PopupHint, filterString, isJP))
                    {//フィルターで消す.
                        continue;
                    }
                }
                if (showLowCommand == false)
                {
                    if (script.IsLowCommand)
                    {//LOW命令なので消す
                        continue;
                    }
                }

                this.ListBox.Items.Add(name);
            }

            if (filtered == true && category != "{TEMPLATE}")
            {
            }
            else
            {
                foreach (EventTemplateImpl.EventTemplate ev in this.EventTemplate.GetTemplateAll())
                {
                    string name = ev.Info + EVENT_TEMPLATE_MARK;
                    if (filterString.Length > 0)
                    {
                        if (!U.StrStrEx(name + " " + Path.GetFileName(ev.Filename), filterString, isJP))
                        {//フィルターで消す.
                            continue;
                        }
                    }

                    this.ListBox.Items.Add(name);
                }
            }

            this.ListBox.EndUpdate();

        }

        public EventScript.Script Script { get; private set; }
        bool UpdateEventSelected()
        {
            string text = this.ListBox.Text;
            if (text.Length <= 0)
            {
                return false;
            }

            foreach (EventScript.Script script in Program.EventScript.Scripts)
            {
                if (EventScript.makeCommandComboText(script,true) == text)
                {
                    this.Script = script;
                    return true;
                }
            }

            return false;
        }

        const string EVENT_TEMPLATE_MARK = "//{EVENT_TEMPLATE}";
        public List<EventScript.OneCode> EventTemplateCode { get; private set; }
        bool UpdateEventTemplateSelected()
        {
            string text = this.ListBox.Text;
            if (text.Length <= 0)
            {
                return false;
            }

            text = text.Replace(EVENT_TEMPLATE_MARK, "");
            foreach (EventTemplateImpl.EventTemplate ev in this.EventTemplate.GetTemplateAll())
            {
                if (ev.Info == text)
                {
                    this.EventTemplateCode = this.EventTemplate.GetCodes(ev);
                    return true;
                }
            }

            return false;
        }

        private void EventScriptFormCategorySelectForm_Load(object sender, EventArgs e)
        {
        }
        public void Init(uint mapid, EventScriptInnerControl currentControl)
        {
            EventTemplate.Init(mapid, currentControl);
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
            if (UpdateEventSelected())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            if (UpdateEventTemplateSelected())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Retry;
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
            int hoverindex = this.ListBox.IndexFromPoint(e.Location);
            ShowInfomation(this.ListBox, hoverindex);
        }
        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowInfomation(this.ListBox, this.ListBox.SelectedIndex);
        }

        private void ShowInfomation(ListBox lb, int hoverindex)
        {
            if (hoverindex < 0 || hoverindex > lb.Items.Count)
            {//範囲外
                this.Infomation.Text = "";
                return;
            }
            if (this.LastHoverIndex == hoverindex)
            {//最後に表示していた行と同じ
                return;
            }
            this.LastHoverIndex = hoverindex;
            this.Infomation.Text = "";

            string listtext = lb.Items[hoverindex].ToString();
            foreach (EventScript.Script script in Program.EventScript.Scripts)
            {
                if (EventScript.makeCommandComboText(script, true) == listtext)
                {
                    if (script.PopupHint == "")
                    {
                        break;
                    }

                    this.Infomation.Text = listtext + "\r\n" + script.PopupHint;

                    return;
                }
            }
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

        private void ListBox_MouseLeave(object sender, EventArgs e)
        {

        }

        private void DisplayLowCommandCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CategoryListBox_SelectedIndexChanged(sender, e);
        }


    }
}
