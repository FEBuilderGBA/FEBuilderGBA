using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SearchHelperControl : UserControl
    {
        public SearchHelperControl()
        {
            InitializeComponent();
        }
        private void SearchHelperControl_Load(object sender, EventArgs e)
        {
            LangCode = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            Color Color_Control_BackColor = OptionForm.Color_Control_BackColor();
            Color Color_Control_ForeColor = OptionForm.Color_Control_ForeColor();
            EntryButton.BackColor = Color_Control_BackColor;
            EntryButton.ForeColor = Color_Control_ForeColor;
            NextButton.BackColor = Color_Control_BackColor;
            NextButton.ForeColor = Color_Control_ForeColor;
            this.BackColor = Color_Control_BackColor;
            this.ForeColor = Color_Control_ForeColor;
            searchlabel.BackColor = Color_Control_BackColor;
            searchlabel.ForeColor = Color_Control_ForeColor;


            Color Color_Input_BackColor = OptionForm.Color_Input_BackColor();
            Color Color_Input_ForeColor = OptionForm.Color_Input_ForeColor();
            SearchWord.BackColor = Color_Input_BackColor;
            SearchWord.ForeColor = Color_Input_ForeColor;
        }

        public bool IsInnerJump = false;
        public int LastMatch = -1; //最後にマッチしたところ

        string LangCode;
        ListBox TargetistBox;
        public void SetListBox(ListBox listbox)
        {
            this.TargetistBox = listbox;
        }

        void BackListBox()
        {
            this.Hide();
            //Application.DoEvents();
            this.TargetistBox.Focus();
        }
        void BackListBox(KeyEventArgs e)
        {
            e.Handled = true;  // 既定の処理を無効化
            e.SuppressKeyPress = true;  // 入力処理を抑制

            BackListBox();
        }

        private void EntryButton_Click(object sender, EventArgs e)
        {
            BackListBox();
        }

        private void SearchWord_KeyDown(object sender, KeyEventArgs e)
        {
            //特殊なキーが押されたばあい、リストボックスに戻る.
            if (e.KeyCode == Keys.Up)
            {
                if(this.TargetistBox.SelectedIndex>0)
                {
                    this.TargetistBox.SelectedIndex -= 1;
                }
                BackListBox(e);
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                if(this.TargetistBox.SelectedIndex < this.TargetistBox.Items.Count)
                {
                    this.TargetistBox.SelectedIndex += 1;
                }
                BackListBox(e);
                return;
            }
            if(    e.KeyCode == Keys.PageUp
                || e.KeyCode == Keys.PageDown
                || e.KeyCode == Keys.Escape
                || e.KeyCode == Keys.Enter
                )
            {
                BackListBox(e);
                return;
            }
            if (e.KeyCode == Keys.Back)
            {
                if (SearchWord.Text.Length <= 0)
                {//何も入力されているものがない状態で、バックスペース押したら元に戻す.
                    BackListBox(e);
                }
                return;
            }
        }

        void SelectIndex(int index, bool noListFocus)
        {
            if (IsDisposed)
            {
                return;
            }

            this.IsInnerJump = true;
            this.TargetistBox.SelectedIndex = index;

            if (noListFocus)
            {
            }
            else
            {
                Rectangle rect = this.TargetistBox.GetItemRectangle(index);
                Point pt = new Point(rect.Left, rect.Top);
                pt = this.TargetistBox.PointToScreen(pt);
                Cursor.Position = pt;
            }
            this.IsInnerJump = false;
            this.LastMatch = index;

            //カーソルを移動すると、フォーカスも変わってしまうらしい?
            this.ActiveControl = this.SearchWord;
        }

        private void SearchWord_TextChanged(object sender, EventArgs e)
        {
            string search = SearchWord.Text;
            int itemcount = this.TargetistBox.Items.Count;
            SearchFor(search, 0, itemcount, noListFocus: false);
        }
        bool SearchFor(string search , int start,int end , bool noListFocus)
        {
            if (search == "")
            {
                return false;
            }

            if (U.isHexString(search))
            {//hexの場合は、先頭のコードだけを見て探索します.
                uint searchhex = U.atoh(search);
                for (int i = start; i < end; i++)
                {
                    if (U.atoh((string)this.TargetistBox.Items[i]) == searchhex)
                    {
                        SelectIndex(i, noListFocus);
                        return true;
                    }
                }
            }

            //かな入力への配慮
            bool isJP = (this.LangCode == "ja");
            if (isJP && OptionForm.IsKanaToNumberMode() && U.isAsciiString(search))
            {
                string t = U.KanaToNumber(search);
                if (t != "")
                {
                    uint searchhex = U.atoh(t);
                    for (int i = start; i < end; i++)
                    {
                        if (U.atoh((string)this.TargetistBox.Items[i]) == searchhex)
                        {
                            SelectIndex(i, noListFocus);
                            return true;
                        }
                    }
                }
            }

            search = U.CleanupFindString(search, isJP);

            //部分一致
            for (int i = start; i < end; i++)
            {
                string t = U.CleanupFindString((string)this.TargetistBox.Items[i], true);
                if (t.IndexOf(search) >= 0)
                {
                    SelectIndex(i, noListFocus);
                    return true;
                }
            }
            return false;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            SearchNext();
        }
        public void SearchNext()
        {
            string search = SearchWord.Text;
            int itemcount = this.TargetistBox.Items.Count;
            if (this.LastMatch >= 0)
            {
                int searchStart = this.LastMatch + 1;
                bool r = SearchFor(search, searchStart, itemcount, noListFocus: true);
                if (r)
                {
                    return;
                }
            }
            //全件サーチ
            SearchFor(search, 0, itemcount, noListFocus: true);
        }

    }
}
