using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace FEBuilderGBA
{
    public class ListBoxEx : ListBox
    {
        public ListBoxEx()
        {
            this.IntegralHeight = false;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
//            base.OnMeasureItem(e);
            if (e.Index < 0 || e.Index >= this.Items.Count)
            {
                return;
            }

            Rectangle rc = new Rectangle(
              (int)(e.Graphics.VisibleClipBounds.X)
            , (int)(e.Graphics.VisibleClipBounds.Y)
            , (int)(e.Graphics.VisibleClipBounds.Width)
            , (int)(e.Graphics.VisibleClipBounds.Height)
            );

            Size size = this.DrawFunc(this , e.Index, e.Graphics, rc, false);

            if (size.Height > 255)
            {
                //ListBoxの制限で255以上の高さにすることはできない.
                //なんだこの変な制約は・・・たまげたなあ
                size.Height = 255;
            }
            if (size.Width <= 0)
            {
                size.Width = this.Width;
            }


            e.ItemWidth = this.Width;
            e.ItemHeight = size.Height;

            //オーナードローをしている場合、HorizontalExtentを設定しないと、横スクロールバーが表示されない
            //https://dobon.net/vb/bbs/log3-54/31604.html
            if (this.HorizontalExtent < size.Width)
            {
                this.HorizontalExtent = size.Width;
            }
        }
        public override int SelectedIndex 
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                base.SelectedIndex = value;
                if (base.SelectedIndex - base.TopIndex > 15)
                {
                    base.TopIndex = value;
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= this.Items.Count)
            {
                return;
            }

            try
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                DrawBackground(e.Index, e.Graphics, e.Bounds, e.State, true);
                this.DrawFunc(this, e.Index, e.Graphics, e.Bounds, true);
            }
            catch (Exception ee)
            {
                Log.Error(R.ExceptionToString(ee));
#if DEBUG
                throw;
#endif
            }
        }
        //ちらつきを防ぐため、背景を描画しない
        //http://www.atmarkit.co.jp/ait/articles/0408/19/news072.html
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // 何もしない
            // base.OnPaintBackground(pevent);
        }

        void OnHover(MouseEventArgs e)
        {
            int hoverindex = this.IndexFromPoint(e.Location);
            if (hoverindex == this.LastHoverIndex)
            {
                return;
            }

            if (hoverindex >= 0 && hoverindex < this.Items.Count 
                && hoverindex != this.SelectedIndex)
            {
                Rectangle rc = this.GetItemRectangle(hoverindex);
                this.Invalidate(rc);
            }
            if (this.LastHoverIndex >= 0 && this.LastHoverIndex < this.Items.Count 
                && this.LastHoverIndex != this.SelectedIndex)
            {
                Rectangle rc = this.GetItemRectangle(this.LastHoverIndex);
                this.Invalidate(rc);
            }
            this.LastHoverIndex = hoverindex;
        }

        void DrawBackground(int index, Graphics g, Rectangle listbounds, DrawItemState state, bool isWithDraw)
        {
            if (!isWithDraw)
            {
                return;
            }

            if ((state & DrawItemState.Selected) > 0)
            {//選択色
                Brush brush = new SolidBrush(OptionForm.Color_List_SelectedColor());
                g.FillRectangle(brush, listbounds);
                brush.Dispose();
                
                return;
            }

            if (index != this.LastHoverIndex)
            {//通常
                Brush brush = new SolidBrush(OptionForm.Color_Input_BackColor());
                g.FillRectangle(brush, listbounds);
                brush.Dispose();
            }
            else
            {//hover
                Brush brush = new SolidBrush(OptionForm.Color_List_HoverColor());
                g.FillRectangle(brush, listbounds);
                brush.Dispose();
                
            }
        }

        int LastHoverIndex = -1;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            OnHover(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (LastHoverIndex >= 0 && LastHoverIndex < this.Items.Count)
            {
                Rectangle rc = this.GetItemRectangle(LastHoverIndex);

                LastHoverIndex = -1;
                this.Invalidate(rc);
            }
        }

        public const int OWNER_DRAW_ICON_SIZE = 20;
        Func<ListBox, int, Graphics, Rectangle, bool, Size> DrawFunc = DrawTextOnly;

        public void OwnerDraw(
            Func<ListBox, int, Graphics, Rectangle, bool, Size> drawFunc
            , DrawMode drawMode
            , bool appendSearch = true)
        {
            //see http://www110.kir.jp/csharp/chip0256.html

            if (drawFunc != DrawTextOnly && this.ItemHeight < OWNER_DRAW_ICON_SIZE)
            {//アイコンを描画する場所で高さがアイコンよりも小さいなら、増やす.
                this.ItemHeight = OWNER_DRAW_ICON_SIZE;
            }
            this.DrawFunc = drawFunc;
            this.DrawMode = drawMode;

            if (appendSearch)
            {
                //ついでに賢い検索も付ける.
                MakeSmartListBoxJump();
            }
        }

        public void ItemListToJumpForm(string linktype, string[] args = null)
        {
            if (args == null)
            {
                args = new string[] { };
            }
            this.MouseDoubleClick += (sender, e) =>
            {
                int i = this.SelectedIndex;
                if (i < 0 || i >= this.Items.Count)
                {
                    return;
                }
                string name = this.Items[i].ToString();
                uint value = U.atoh(name);
                InputFormRef.JumpTo(null, value, linktype, args);
            };
        }

        static void setGrapihcsScaleMode(Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
        }

        //Uint + テキスト (PARSER) + unit テキストを書くルーチン
        public static Size DrawUnit2AndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;


            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = UnitForm.DrawUnitMapFacePicture(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
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
                b.Y += textmargineY;
                bounds.X += U.DrawText(a1, g, normalFont, brush, isWithDraw, b);

                //後半
                string a2 = text.Substring(p);

                //後半のアイコンを描く
                icon = U.atoh(a2);
                bitmap = UnitForm.DrawUnitMapFacePicture(icon);
                U.MakeTransparent(bitmap);

                b = bounds;
                b.Width = OWNER_DRAW_ICON_SIZE;
                b.Height = OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                //後半の文字列
                b = bounds;
                b.Y += textmargineY;
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
            

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }
        //Uint + テキスト (PARSER) + Class テキストを書くルーチン
        public static Size DrawUnitAndClassAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = UnitForm.DrawUnitMapFacePicture(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();

            string[] parsers = new string[] { " -> ", " <- ", " ?? ", " & " , " - "};
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
                b.Y += textmargineY;
                bounds.X += U.DrawText(a1, g, normalFont, brush, isWithDraw, b);

                //後半
                string a2 = text.Substring(p);

                //後半のアイコンを描く
                icon = U.atoh(a2);
                bitmap = ClassForm.DrawWaitIcon(icon, 0, true);
                U.MakeTransparent(bitmap);

                b = bounds;
                b.Width = OWNER_DRAW_ICON_SIZE;
                b.Height = OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                //後半の文字列
                b = bounds;
                b.Y += textmargineY;
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

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //Class + テキスト (PARSER) + Class テキストを書くルーチン
        public static Size DrawClass2AndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;


            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ClassForm.DrawWaitIcon(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
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
                b.Y += textmargineY;
                bounds.X += U.DrawText(a1, g, normalFont, brush, isWithDraw, b);

                //後半
                string a2 = text.Substring(p);

                //後半のアイコンを描く
                icon = U.atoh(a2);
                bitmap = ClassForm.DrawWaitIcon(icon);
                U.MakeTransparent(bitmap);

                b = bounds;
                b.Width = OWNER_DRAW_ICON_SIZE;
                b.Height = OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                //後半の文字列
                b = bounds;
                b.Y += textmargineY;
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


            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //Unit + テキストを書く
        public static Size DrawUnitAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = UnitForm.DrawUnitMapFacePicture(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            bitmap.Dispose();
            brush.Dispose();         

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //Class + テキストを書く
        public static Size DrawClassAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(クラス番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ClassForm.DrawWaitIcon(icon, 0, true);
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            bitmap.Dispose();
            brush.Dispose();         
            

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //Item + テキストを書く
        public static Size DrawItemAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(クラス番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ItemForm.DrawIcon(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);


            bitmap.Dispose();
            brush.Dispose();         

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //Attribute + テキストを書く
        public static Size DrawAtributeAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;
            

            //テキストの先頭にアイコン番号(属性番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageSystemIconForm.Attribute(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);


            bitmap.Dispose();
            brush.Dispose();         

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //WeaponTypeIcon + テキスト + (PARSER) + WeaponTypeIcon + テキスト  を書く
        public static Size DrawWeaponTypeIcon2AndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageSystemIconForm.WeaponIcon(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
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
                b.Y += textmargineY;
                bounds.X += U.DrawText(a1, g, normalFont, brush, isWithDraw, b);

                //後半
                string a2 = text.Substring(p);

                //後半のアイコンを描く
                icon = U.atoh(a2);
                bitmap = ImageSystemIconForm.WeaponIcon(icon);
                U.MakeTransparent(bitmap);

                b = bounds;
                b.Width = OWNER_DRAW_ICON_SIZE;
                b.Height = OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                //後半の文字列
                b = bounds;
                b.Y += textmargineY;
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

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //バトルアニメ + テキストを書くルーチン
        public static Size DrawImageBattleAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageBattleAnimeForm.DrawBattleAnime(icon, ImageBattleAnimeForm.ScaleTrim.SCALE_48);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            brush.Dispose();
            bitmap.Dispose();

            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //顔 + テキストを書くルーチン
        public static Size DrawImagePortraitAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImagePortraitForm.DrawPortraitAuto(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            bitmap.Dispose();
            brush.Dispose();

            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        const int ICON_WIDTH = 30; //横長で少し大き目で描画します.
        const int ICON_HEGITH = 24;

        //BG + テキストを書くルーチン
        public static Size DrawBGAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageBGForm.DrawBG(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = ICON_WIDTH;
            b.Height = ICON_HEGITH;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            brush.Dispose();
            bitmap.Dispose();


            bounds.Y += ICON_WIDTH;
            return new Size(bounds.X, bounds.Y);
        }

        //CG + テキストを書くルーチン
        public static Size DrawCGAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageCGForm.DrawImageByID(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
            Rectangle b = bounds;
            b.Width = ICON_WIDTH;
            b.Height = ICON_HEGITH;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            brush.Dispose();
            bitmap.Dispose();

            bounds.Y += ICON_WIDTH;
            return new Size(bounds.X, bounds.Y);
        }

        //テキストを書く(hover利用のため)
        public static Size DrawTextOnly(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            brush.Dispose();         
            

            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }
        //アイテムアイコン + テキストを書くルーチン
        public static Size DrawImageItemIconAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageItemIconForm.DrawIconWhereID(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
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
        //特殊タイプ + テキストを書くルーチン
        public static Size DrawImageSPTypeAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            uint addr = InputFormRef.SelectToAddr(lb, index);
            Bitmap bitmap;
            if (U.isSafetyOffset(addr))
            {
                uint b0 = Program.ROM.u8(addr + 0);
                uint b1 = Program.ROM.u8(addr + 1);
                bitmap = ImageBattleAnimeForm.getSPTypeIcon(b0, b1);
            }
            else
            {
                bitmap = ImageUtil.Blank(ListBoxEx.OWNER_DRAW_ICON_SIZE, ListBoxEx.OWNER_DRAW_ICON_SIZE);
            }
            U.MakeTransparent(bitmap);


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
        //GBAColor + テキストを書く
        public static Size DrawColorAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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
         

            uint addr = InputFormRef.SelectToAddr(lb, index);

            Bitmap bitmap = ImageUtil.Blank(ListBoxEx.OWNER_DRAW_ICON_SIZE, ListBoxEx.OWNER_DRAW_ICON_SIZE);
            if (U.isSafetyOffset(addr))
            {
                uint dp = Program.ROM.u16(addr);
                byte dr = (byte)((dp & 0x1F));
                byte dg = (byte)(((dp >> 5) & 0x1F));
                byte db = (byte)(((dp >> 10) & 0x1F));
                ColorPalette p = bitmap.Palette;
                p.Entries[0] = Color.FromArgb(dr << 3, dg << 3, db << 3);
                bitmap.Palette = p;
            }

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
        //待機アイコン + テキストを書くルーチン
        public static Size DrawImageUnitMoveIconAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageUnitMoveIconFrom.DrawMoveUnitIconBitmap(icon, 0, 0);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
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
        //待機アイコン + テキストを書くルーチン
        public static Size DrawImageUnitWaitIconAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(icon, 0, true);
            U.MakeTransparent(bitmap);

            //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
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

        SearchHelperControl SHControl;
        void MakeSmartListBoxJump()
        {
            Control f = U.ControlToParentFormOrUserControl(this);

            this.SHControl = new SearchHelperControl();
            this.SHControl.SetListBox(this);
            this.SHControl.BringToFront();

            this.SHControl.Hide();
            f.Controls.Add(this.SHControl);
            this.SHControl.BringToFront();

            this.SelectedIndexChanged += (sender, e) =>
            {
                if (!this.SHControl.IsInnerJump)
                {
                    this.SHControl.Hide();
                }
            };

            this.KeyPress += (sender, e) =>
            {
                e.Handled = true; //LISTBOX内部のジャンプを抑制.
            };

            this.KeyDown += (sender, e) =>
            {
                if (e.Control || e.Alt || e.Shift)
                {//コントロールキーが押されていたら対象外
                    return;
                }
                if (this.SelectedIndex < 0)
                {//無選択
                    return;
                }

                string c;
                if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                {//1-0
                    c = ((char)e.KeyCode).ToString();
                }
                else if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
                {//a-z
                    c = Convert.ToString((char)(((char)e.KeyCode) - 'A' + 'a'));
                }
                else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
                {//テンキー
                    c = ((char)((e.KeyCode - Keys.NumPad0) + Keys.D0)).ToString();
                }
                else if (e.KeyCode == Keys.KanaMode || e.KeyCode == Keys.KanjiMode || e.KeyCode == Keys.ProcessKey)
                {//IME
                    c = "";
                }
                else
                {
                    return;
                }

                Rectangle rect = this.GetItemRectangle(this.SelectedIndex);
                rect = U.ControlRectToFormRect(this, rect);
                this.SHControl.Location = new Point(rect.Right, rect.Top);
                this.SHControl.SearchWord.Text = c;
                this.SHControl.Show();
                this.SHControl.SearchWord.Focus();
                this.SHControl.SearchWord.Select(1, 0);
            };
        }

        //アドレスリストを総入れ替えをする.
        //C#はクソだから、一部だけリストを更新した時には、高さの再計算が走らないようだ。
        //this.AddressList.Refresh();   this.AddressList.Invalidate(); やっても無視される.
        //仕方ないから、リストを全部消して、作り直して、強引に高さ再計算させる.　 
        public void DummyAlloc(int count, int newselected)
        {
            int topItemIndex = this.TopIndex;

            if (this.Items.Count == count
                && this.DrawMode != System.Windows.Forms.DrawMode.OwnerDrawVariable)
            {//件数が同一なので再描画のみ
             //ただし、サイズがそれぞれ違う場合は、高さを再計算しないといけないので、更新ではダメらしい.
                this.Invalidate();
            }
            else
            {//件数が違うので作り直す
                this.BeginUpdate();
                this.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    this.Items.Add(" ");
                }
                this.EndUpdate();
            }

            if (newselected >= 0)
            {
                this.TopIndex = topItemIndex;
                if (newselected < count)
                {
                    this.SelectedIndex = newselected;
                }
            }
        }

        public void InvalidateLine(int line = -1)
        {
            if (line < 0)
            {
                line = this.SelectedIndex;
            }
            if (line < 0)
            {//不明なので全部更新.
                this.Invalidate();
                return ;
            }
            this.Invalidate(this.GetItemRectangle(line));
        }

        //DrawOPClassFont + テキストを書くルーチン
        public static Size DrawOPClassFontAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            //テキストの先頭にアイコン番号(キャラ番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap;
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    bitmap = OPClassFontForm.DrawFontByID(icon);
                }
                else
                {
                    bitmap = OPClassFontFE8UForm.DrawFontByID(icon);
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                bitmap = ImageUtil.Blank(8,8);
            }
            else
            {//ver 6
                bitmap = ImageUtil.Blank(8, 8);
            }

            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = ICON_WIDTH;
            b.Height = ICON_HEGITH;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            brush.Dispose();
            bitmap.Dispose();

            bounds.Y += ICON_WIDTH;
            return new Size(bounds.X, bounds.Y);
        }

        public void SearchNext()
        {
            if (this.SHControl == null)
            {
                return;
            }
            if (this.SHControl.Visible)
            {//現在検索中で、検索ボックスが表示されている場合
                //次を検索
                this.SHControl.SearchNext();
                return;
            }
            this.Focus();
            this.OnKeyDown(new KeyEventArgs(Keys.ProcessKey));
        }
        public static Size DrawEventCategory(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

                string nn = sp[i];
                int p = nn.IndexOf(':');
                if (p < 0)
                {
                    bounds.X += U.DrawText(nn, g, normalFont, brush, isWithDraw, bounds);
                    continue;
                }

                string item = nn.Substring(0, p);

                Bitmap bitmap = null;
                if (item == "UNIT")
                {
                    bitmap = UnitForm.DrawUnitMapFacePicture(1);
                }
                else if (item == "CLASS")
                {
                    bitmap = ClassForm.DrawWaitIcon(3);
                }
                else if (item == "POINTER_PROCS")
                {
                    bitmap = ImageSystemIconForm.WeaponIcon(4);
                }
                else if (item == "POINTER_ASM")
                {
                    bitmap = ImageSystemIconForm.WeaponIcon(8);
                }
                else if (item == "POINTER_UNIT")
                {
                    bitmap = ClassForm.DrawWaitIcon(7);
                }
                else if (item == "POINTER_EVENT")
                {//サブルーチンは目立つ緑のアイコン
                    bitmap = ImageSystemIconForm.MusicIcon(3);
                }
                else if (item == "MUSIC")
                {
                    bitmap = ImageSystemIconForm.MusicIcon(6);
                }
                else if (item == "SOUND")
                {
                    bitmap = ImageSystemIconForm.MusicIcon(7);
                }
                else if (item == "ITEM")
                {//アイテムアイコン
                    bitmap = ImageSystemIconForm.WeaponIcon(8);
                }
                else if (item == "FLAG")
                {
                    bitmap = ImageSystemIconForm.FlagIcon();
                }
                else if (item == "WMLOCATION")
                {//拠点
                    bitmap = WorldMapImageForm.DrawWorldMapIcon(0xB);
                }
                else if (item == "WMPATH")
                {//道
                    bitmap = WorldMapImageForm.DrawWorldMapIcon(0xB);
                }
                if (bitmap != null)
                {
                    U.MakeTransparent(bitmap);

                    Rectangle b = bounds;
                    b.Width = lineHeight;
                    b.Height = lineHeight;

                    bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                    bitmap.Dispose();

                    bounds.X += U.DrawText(nn.Substring(p), g, normalFont, brush, isWithDraw, bounds);
                }
                else
                {
                    bounds.X += U.DrawText(nn.Substring(p + 1), g, normalFont, brush, isWithDraw, bounds);
                }
            }

            //コードをコメント形式で描画
            if (sp.Length > 0)
            {
                int i = sp.Length - 1;
                SolidBrush commentBrush = new SolidBrush(OptionForm.Color_Comment_ForeColor());
                if (bounds.X < 700)
                {
                    bounds.X = 700;
                }
                bounds.X += U.DrawText("//" + sp[i], g, normalFont, commentBrush, isWithDraw, bounds);
                commentBrush.Dispose();
            }
            brush.Dispose();
            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }
        //ClassType + テキストを書く
        public static Size DrawClassTypeAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(クラス番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = ClassForm.DrawClassTypeIcon(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            bitmap.Dispose();
            brush.Dispose();


            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //GameOption + テキストを書く
        public static Size DrawGameOptionAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(クラス番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = StatusOptionForm.DrawIcon(icon, 0, true);
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);

            bitmap.Dispose();
            brush.Dispose();


            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }
        //Skill + テキストを書く
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

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストの先頭にアイコン番号(クラス番号が入っている. 無駄だが汎用性を考えるとほかに方法がない)
            uint icon = U.atoh(text);
            Bitmap bitmap = SkillUtil.DrawIcon(icon);
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = OWNER_DRAW_ICON_SIZE;
            b.Height = OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);


            bitmap.Dispose();
            brush.Dispose();

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }
    }
}
