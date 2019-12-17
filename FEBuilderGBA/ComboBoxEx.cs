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
    public class ComboBoxEx : ComboBox
    {
        public ComboBoxEx()
        {
            this.DoubleBuffered = true;
        }

        public void SetToolTipEx(ToolTipEx tooltip)
        {
            this._ToolTipEx = tooltip;
        }
        ToolTipEx _ToolTipEx;
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_ToolTipEx != null)
            {
                _ToolTipEx.HideEvent(this, e);
            }
        }

        public void ShowTooltip(string msg)
        {
            if (this._ToolTipEx == null)
            {
                return;
            }
            
            _ToolTipEx.FireEvent(this, msg);
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
                //ComboBoxの制限で255以上の高さにすることはできない.
                //なんだこの変な制約は・・・たまげたなあ
                size.Height = 255;
            }
            if (size.Width <= 0)
            {
                size.Width = this.Width;
            }


            e.ItemWidth = this.Width;
            e.ItemHeight = size.Height;
        }


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
//            base.OnDrawItem(e);
            if (e.Index < 0 || e.Index >= this.Items.Count)
            {
                return;
            }
            
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            DrawBackground(e.Index, e.Graphics, e.Bounds, e.State, true);
            this.DrawFunc(this, e.Index, e.Graphics, e.Bounds, true);
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

            {
                Brush brush = new SolidBrush(OptionForm.Color_Input_BackColor());
                g.FillRectangle(brush, listbounds);
                brush.Dispose();
            }
        }

        public const int OWNER_DRAW_ICON_SIZE = 20;
        Func<ComboBoxEx, int, Graphics, Rectangle, bool, Size> DrawFunc = DrawTextOnly;

        public void OwnerDraw(Func<ComboBoxEx, int, Graphics, Rectangle, bool, Size> drawFunc
            , DrawMode drawMode)
        {
            //see http://www110.kir.jp/csharp/chip0256.html

            if (drawFunc != DrawTextOnly && this.ItemHeight < OWNER_DRAW_ICON_SIZE)
            {//アイコンを描画する場所で高さがアイコンよりも小さいなら、増やす.
                this.ItemHeight = OWNER_DRAW_ICON_SIZE;
            }
            this.DrawFunc = drawFunc;
            this.DrawMode = drawMode;
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

        //Uint + テキスト (PARSER) + Class テキストを書くルーチン
        public static Size DrawUnitAndClassAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

        //Unit + テキストを書く
        public static Size DrawUnitAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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
        public static Size DrawClassAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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
        public static Size DrawItemAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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


        //バトルアニメ + テキストを書くルーチン
        public static Size DrawImageBattleAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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
        public static Size DrawImagePortraitAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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


        //アイコンリストから描画. ツールチップもつけるよ!
        public static Size DrawIconAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

            AppendInfoSt aist = lb.GetAISTByIconList(text, index);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            if (aist.icon == null)
            {
                bounds.X += ListBoxEx.OWNER_DRAW_ICON_SIZE;
            }
            else
            {
                bounds.X += U.DrawPicture(aist.icon, g, isWithDraw, b);
            }
            //bitmap.Dispose(); //アイコンリストにあるので解放してはいけない.

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);
            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;

            brush.Dispose();

            if (aist.tooltip != "")
            {
                lb.ShowTooltip(aist.tooltip);
            }

            return new Size(bounds.X, bounds.Y);
        }

        //テキストを書く(hover利用のため)
        public static Size DrawTextOnly(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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
        //GBAColor + テキストを書く
        public static Size DrawColorAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
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

        //フォント名で書く.
        public static Size DrawFontAndText(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = new Font(text,9);
            Rectangle bounds = listbounds;

            int textmargineY = (OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            //テキストを描く.
            Rectangle b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);


            brush.Dispose();

            bounds.Y += OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        //アドレスリストを総入れ替えをする.
        //C#はクソだから、一部だけリストを更新した時には、高さの再計算が走らないようだ。
        //this.AddressList.Refresh();   this.AddressList.Invalidate(); やっても無視される.
        //仕方ないから、リストを全部消して、作り直して、強引に高さ再計算させる.　 
        public void DummyAlloc(int count, int newselected)
        {
            this.BeginUpdate();
            this.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                this.Items.Add(" ");
            }
            this.EndUpdate();

            if (newselected >= 0)
            {
                if (newselected < count)
                {
                    this.SelectedIndex = newselected;
                }
            }
        }

        struct AppendInfoSt
        {
            public Bitmap icon;
            public String tooltip;
        };

        Dictionary<string, AppendInfoSt> IconList = new Dictionary<string, AppendInfoSt>();
        public void AddIcon(uint name, Bitmap bitmap,string tooltip = "")
        {
            AddIcon(U.ToHexString(name), bitmap);
        }
        public void AddIcon(int name, Bitmap bitmap, string tooltip = "")
        {
            AddIcon(U.ToHexString(name), bitmap);
        }
        public void AddIcon(string name, Bitmap bitmap, string tooltip = "")
        {
            U.MakeTransparent(bitmap);
            AppendInfoSt aist = new AppendInfoSt();
            aist.icon = bitmap;
            aist.tooltip = tooltip;
            IconList[name] = aist;
        }
        public void AddIcon(string name, Icon icon, string tooltip = "")
        {
            AddIcon(name, icon.ToBitmap(), tooltip);
        }
        AppendInfoSt GetAISTByIconList(string str, int index)
        {
            if (IconList.ContainsKey(str))
            {
                return IconList[str];
            }
            string[] sp = str.Split('=');
            if (sp.Length >= 2)
            {
                if (IconList.ContainsKey(sp[1]))
                {
                    return IconList[sp[1]];
                }
                if (IconList.ContainsKey(sp[0]))
                {
                    return IconList[sp[0]];
                }
                if (U.isHexString(sp[0]))
                {
                    string num = U.atoh(sp[0]).ToString();
                    if (IconList.ContainsKey(num))
                    {
                        return IconList[num];
                    }
                }
            }

            if (IconList.ContainsKey(index.ToString()))
            {
                return IconList[index.ToString()];
            }

            return new AppendInfoSt();
        }
        public Bitmap GetCurrentIcon()
        {
            AppendInfoSt aist = this.GetAISTByIconList(this.Text, this.SelectedIndex);
            return aist.icon;
        }
    }
}
