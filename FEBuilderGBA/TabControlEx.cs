using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    //https://dobon.net/vb/dotnet/control/tabsidebug.html#paint
    class TabControlEx : TabControl
    {
        public TabControlEx()
            : base()
        {
            //Paintイベントで描画できるようにする
            this.SetStyle(ControlStyles.UserPaint, true);
            //ダブルバッファリングを有効にする
            this.DoubleBuffered = true;
            //リサイズで再描画する
            this.ResizeRedraw = true;

            //ControlStyles.UserPaintをTrueすると、
            //SizeModeは強制的にTabSizeMode.Fixedにされる
            this.SizeMode = TabSizeMode.Fixed;
            this.ItemSize = new Size(200, 18);
            this.Appearance = TabAppearance.Normal;
            this.Multiline = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //TabControlの背景を塗る
            if (this.DesignMode)
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, this.ClientRectangle);
                return;
            }
            else
            {
                Color backColor = OptionForm.Color_Control_BackColor();
                Brush backBrush = new System.Drawing.SolidBrush(backColor);
                e.Graphics.FillRectangle(backBrush, this.ClientRectangle);
                backBrush.Dispose();
            }


            if (this.TabCount == 0)
            {
                return;
            }
            if (this.SelectedIndex < 0 || this.SelectedIndex >= this.TabCount)
            {
                return;
            }

            //TabPageの枠を描画する
            TabPage page = this.TabPages[this.SelectedIndex];
            Rectangle pageRect = new Rectangle(
                page.Bounds.X - 2,
                page.Bounds.Y - 2,
                page.Bounds.Width + 5,
                page.Bounds.Height + 5);
            if (Application.RenderWithVisualStyles)
            {
                TabRenderer.DrawTabPage(e.Graphics, pageRect);
            }

            //タブを描画する
            for (int i = 0; i < this.TabCount; i++)
            {
                page = this.TabPages[i];
                Rectangle tabRect = this.GetTabRect(i);

                bool focused = false;
                //選択されたタブとページの間の境界線を消すために、
                //描画する範囲を大きくする
                if (this.SelectedIndex == i)
                {
                    tabRect.Height += 1;
                    focused = true;
                }

                //画像のサイズを決定する
                Size imgSize = tabRect.Size;

                //タブの画像を作成する
                Bitmap bmp = new Bitmap(imgSize.Width, imgSize.Height);
                Graphics g = Graphics.FromImage(bmp);
                //高さに1足しているのは、下にできる空白部分を消すため
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height + 1);

                if (Application.RenderWithVisualStyles)
                {
                    //表示するタブの状態を決定する
                    System.Windows.Forms.VisualStyles.TabItemState state;
                    if (!this.Enabled)
                    {
                        state = System.Windows.Forms.VisualStyles.TabItemState.Disabled;
                    }
                    else if (focused)
                    {
                        state = System.Windows.Forms.VisualStyles.TabItemState.Selected;
                    }
                    else
                    {
                        state = System.Windows.Forms.VisualStyles.TabItemState.Normal;
                    }

                    TabRenderer.DrawTabItem(g,
                        rect,
                        false,
                        state);
                }
                else
                {
                    if (focused)
                    {
                        g.FillRectangle(SystemBrushes.Control, rect);
                    }
                    else
                    {
                        g.FillRectangle(SystemBrushes.ControlDark, rect);
                        g.DrawRectangle(new Pen(SystemBrushes.ControlDarkDark), rect);
                    }
                }

                if (page.Text.Length > 10)
                {
                    g.DrawString(page.Text, page.Font, SystemBrushes.ControlText, new Rectangle(rect.Left + 6, rect.Top + 4, rect.Width - 20, rect.Height - 4));
                }
                else
                {
                    g.DrawString(page.Text, page.Font, SystemBrushes.ControlText, new Rectangle(rect.Left + 12, rect.Top + 4, rect.Width - 20, rect.Height - 4));
                }

                Rectangle closeButton = TabRectToCloseButtonRect(tabRect);
                if (closeButton.Contains(this.LastMouseCursor))
                {//マウスカーソルが当たっている、ボタンの背景の色を変える.
                    Brush hoverBrush = new SolidBrush( OptionForm.Color_List_HoverColor() );
                    Rectangle rc = new Rectangle(closeButton.X - tabRect.X, closeButton.Y - tabRect.Y, closeButton.Width, closeButton.Height);
                    g.FillRectangle(hoverBrush,rc);
                    hoverBrush.Dispose();
                }
                g.DrawString("x", page.Font, SystemBrushes.ControlText, rect.Right - 15, rect.Top + 4);
                g.Dispose();

                //画像を描画する
                e.Graphics.DrawImage(bmp, tabRect.X, tabRect.Y, bmp.Width, bmp.Height);


                bmp.Dispose();
            }
        }

        Rectangle TabRectToCloseButtonRect(Rectangle r)
        {
            Rectangle closeButton = new Rectangle(r.Right - 15 - 4, r.Top, 15 + 4, r.Height);
            return closeButton;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            LastMouseCursor = new Point();
            Invalidate();
            base.OnMouseLeave(e);
        }

        Point LastMouseCursor = new Point();
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (LastMouseCursor.X != e.Location.X && LastMouseCursor.Y != e.Location.Y)
            {
                LastMouseCursor = e.Location;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            LastMouseCursor = e.Location;
            for (int i = 0; i < this.TabCount; i++)
            {
                Rectangle r = this.GetTabRect(i);
                Rectangle closeButton = TabRectToCloseButtonRect(r);
                if (closeButton.Contains(e.Location))
                {
                    CloseTab(i);
                    return;
                }
            }

            base.OnMouseDown(e);
        }
        bool OnCloseingCheckWriteConfirmation(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= this.TabCount)
            {
                return true;
            }
            TabPage tab = this.TabPages[tabIndex];

            List<Control> controls = InputFormRef.GetAllControls(tab);
            if (InputFormRef.IsChangeContents(controls))
            {
                DialogResult dr = R.ShowNoYes("書き込んでいないデータがありますが、フォームを閉じてもよろしいですか？");
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {//ユーザキャンセル.
                    return false;
                }
            }
            return true;
        }

        public void CloseTab(int tabIndex)
        {
            if (tabIndex < 0)
            {
                tabIndex = this.SelectedIndex;
            }

            //変更しているものがあったら警告する.
            if (!OnCloseingCheckWriteConfirmation(tabIndex))
            {//ユーザーキャンセル.
                return;
            }

            if (this.TabCount <= 1)
            {//最後の一つを閉じるときは、本体も閉じる.

                this.TabPages.Clear(); //タブに未保存の色があると2回閉じていいか聞くことになるので、すべてのタブを消す.

                Form f = U.ControlToParentForm(this);
                f.Close();
                return;
            }
            if (tabIndex >= this.TabCount)
            {
                return;
            }
            this.TabPages.RemoveAt(tabIndex);
            if (tabIndex >= 1)
            {//普通に閉じると、最初のタブが選択されるので、そうではなくてひとつ前のタブに戻す.
                this.SelectedIndex = tabIndex - 1;
            }
        }

        //次のタブに移動
        void ChangeNextTab(bool isPrev)
        {
            if (this.TabCount <= 1)
            {//1つしかない
                return;
            }

            int i = this.SelectedIndex;

            if (isPrev)
            {
                i--;
                if (i < 0)
                {//先端より前なので、最後に戻す.
                    i = this.TabCount - 1;
                }
            }
            else
            {
                i++;
                if (i >= this.TabCount)
                {//終端より先なので、最初に戻す.
                    i = 0;
                }
            }
            //履歴変更.
            this.SelectedIndex = i;
        }
        void ChangeTab(int index)
        {
            if (index < 0 || index >= this.TabCount)
            {
                return;
            }
            //タブ変更
            this.SelectedIndex = index;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.Alt || e.Control) && e.KeyCode == Keys.Left)
            {
                ChangeNextTab(true);
            }
            else if ((e.Alt || e.Control) && e.KeyCode == Keys.Right)
            {
                ChangeNextTab(false);
            }
            else if (e.Control && e.KeyCode == Keys.T)
            {
                NewTabEvent(this,new EventArgs());
            }
            else if (e.Control && e.KeyCode == Keys.D1)
            {
                ChangeTab(0);
            }
            else if (e.Control && e.KeyCode == Keys.D2)
            {
                ChangeTab(1);
            }
            else if (e.Control && e.KeyCode == Keys.D3)
            {
                ChangeTab(2);
            }
            else if (e.Control && e.KeyCode == Keys.D4)
            {
                ChangeTab(3);
            }
            else if (e.Control && e.KeyCode == Keys.D5)
            {
                ChangeTab(4);
            }
            else if (e.Control && e.KeyCode == Keys.D6)
            {
                ChangeTab(5);
            }
            else if (e.Control && e.KeyCode == Keys.D7)
            {
                ChangeTab(6);
            }
            else if (e.Control && e.KeyCode == Keys.D8)
            {
                ChangeTab(7);
            }
            else if (e.Control && e.KeyCode == Keys.D9)
            {
                ChangeTab(8);
            }
            base.OnKeyDown(e);
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (this.SelectedIndex < 0 ||  this.SelectedIndex >= this.TabCount)
            {
                return;
            }

            TabPage tab = this.TabPages[this.SelectedIndex];
            for (int n = 0; n < tab.Controls.Count; n++)
            {
                Control c = tab.Controls[n];

                //新しいページにフォース
                c.Focus();

                List<Control> controls = InputFormRef.GetAllControls(c);
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i].Name == "AddressList")
                    {
                        controls[i].Focus();
                        break;
                    }
                }
            }
        }

        public void Add(string name,Control c,uint addr)
        {
            TabPage tab = new TabPage(name);
            tab.Tag = addr;
            tab.Controls.Add(c);
//            c.Dock = DockStyle.Fill;
            this.TabPages.Add(tab);

            Form f = U.ControlToParentForm(this);
            if (f.Width < c.Width + 10)
            {
                f.Width = c.Width + 10;
            }
            if (f.Height < c.Height + 20)
            {
                f.Height = c.Height + 20;
            }

            //新しいページに切り替える.
            this.SelectedIndex = this.TabCount - 1;
        }
        public int FindTab(uint addr)
        {
            for (int i = 0; i < this.TabCount; i++)
            {
                TabPage tab = this.TabPages[i];
                if (tab.Tag != null && ((uint)tab.Tag) == addr)
                {
                    return i;
                }
            }
            return -1;
        }
        public void UpdateTab(int tabIndex, string name, uint addr)
        {
            if (this.SelectedIndex < 0 || tabIndex >= this.TabCount)
            {
                return;
            }
            this.TabPages[tabIndex].Tag = addr;
            this.TabPages[tabIndex].Text = name;
        }
        public Control GetTabControl(int tabIndex)
        {
            if (this.SelectedIndex < 0 || tabIndex >= this.TabCount)
            {
                return null;
            }
            if (this.TabPages[tabIndex].Controls.Count <= 0)
            {
                return null;
            }
            return this.TabPages[tabIndex].Controls[0];
        }

        public EventHandler NewTabEvent;

        //コントロールからTabExへの逆変換
        public static TabPage ControlToParentTabExPage(Control c)
        {
            if (c == null)
            {
                return null;
            }
            if (c is TabControlEx)
            {//いきなり、自分自身がタブコントロールだった場合
                TabControlEx tabex = (TabControlEx)c;
                return tabex.SelectedTab;
            }

            Control pc = c.Parent;
            if (pc is TabControlEx)
            {
                return (TabPage)c;
            }
            return ControlToParentTabExPage(pc);
        }

    }
}
