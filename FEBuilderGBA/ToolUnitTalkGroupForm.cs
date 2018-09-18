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
    public partial class ToolUnitTalkGroupForm : Form
    {
        public ToolUnitTalkGroupForm()
        {
            InitializeComponent();
            Init();
            this.AddressList.OwnerDraw(Draw, DrawMode.OwnerDrawFixed);
            U.AddCancelButton(this);
        }
        void Init()
        {
            //uint maxTalkGroup = UnitForm.GetMaxTalkGroup();
            this.AddressList.DummyAlloc( (int)0xD + 1, -1);
        }

        //Uint + テキスト (PARSER) + unit テキストを書くルーチン
        public static Size Draw(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;

            string text;
            Rectangle bounds = listbounds;

            int textmargineY = (ListBoxEx.OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            text = R._("会話グループ") + ":" + U.ToHexString(index);
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            List<U.AddrResult> units = UnitForm.MakeUnitList();
            int unitMax = Math.Min(units.Count, (int)UnitForm.MAX_PLAYER_UNIT_ID+1); //戦績が選べるのは 0x45まで
            for (int i = 0; i < unitMax; i++)
            {
                uint talkGroup = UnitForm.GetTalkGroupByAddr(units[i].addr);
                if (talkGroup == index)
                {
                    Bitmap bitmap = UnitForm.DrawUnitMapFacePictureByAddr(units[i].addr);
                    U.MakeTransparent(bitmap);

                    //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
                    Rectangle b = bounds;
                    b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
                    b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
                    bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                }
            }

            

            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        private void ToolUnitTalkGroupForm_Load(object sender, EventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {

        }


    }
}
