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
    public partial class MapEditorResizeDialogForm : Form
    {
        public MapEditorResizeDialogForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }
        MapChangeForm.ChangeSt ChangeData;
        public void Init(MapChangeForm.ChangeSt change)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ChangeData = change;
            if (this.ChangeData.no == U.NOT_FOUND)
            {//メインマップ
                X.Enabled = false;
                Y.Enabled = false;
                X.Minimum = 0;
                Y.Minimum = 0;

                U.SelectedIndexSafety(W, 15, (int)W.Maximum, 15);
                U.SelectedIndexSafety(H, 10, (int)H.Maximum, 10);
            }
            else
            {//変更
                X.Enabled = true;
                Y.Enabled = true;
                X.Minimum = 0;
                Y.Minimum = 0;

                U.SelectedIndexSafety(W, 1, (int)W.Maximum, 1);
                U.SelectedIndexSafety(H, 1, (int)H.Maximum, 1);
            }

            if (change.x < 0) change.x = 0;
            if (change.y < 0) change.y = 0;
            if (change.width < W.Minimum) change.width = (uint)W.Minimum;
            if (change.height < H.Minimum) change.height = (uint)H.Minimum;

            X.Value = change.x;
            Y.Value = change.y;
            W.Value = change.width;
            H.Value = change.height;
            T.Value = 0;
            B.Value = 0;
            L.Value = 0;
            R.Value = 0;

        }

        private void T_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            int w = (int)(this.ChangeData.width + L.Value + R.Value);
            int h = (int)(this.ChangeData.height + T.Value + B.Value);

            if (w >= W.Maximum)
            {
                return;
            }
            if(w <= W.Minimum)
            {
                return;
            }
            if (h >= H.Maximum)
            {
                return;
            }
            if(h <= H.Minimum)
            {
                return;
            }

            W.Value = w;
            H.Value = h;
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            int w = (int)(this.ChangeData.width  + L.Value + R.Value);
            int h = (int)(this.ChangeData.height + T.Value + B.Value);

            if (this.ChangeData.no == U.NOT_FOUND)
            {//メインマップ
                if (w < 15 || h < 10)
                {
                    FEBuilderGBA.R.ShowStopError(FEBuilderGBA.R._("メインマップは 15x10 以下のサイズにできません。"));
                    return;
                }
            }
            else
            {//変更
                if (w < 1 || h < 1)
                {
                    FEBuilderGBA.R.ShowStopError("サイズを1x1以下にはできません");
                    return;
                }
            }
            uint limitWidth = ImageUtilMap.GetLimitMapWidth(h);
            if (limitWidth == 0)
            {
                FEBuilderGBA.R.ShowStopError(FEBuilderGBA.R._("マップが広すぎます。\r\n現在のサイズ({0},{1})", w, h));
                return;
            }
            if (w > limitWidth)
            {
                FEBuilderGBA.R.ShowStopError(FEBuilderGBA.R._("マップが広すぎます。\r\n現在のサイズ({0},{1})\r\nこの幅だと、利用可能な高さは、幅は{2}までです。", w, h, limitWidth));
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void MapEditResizeDialogForm_Load(object sender, EventArgs e)
        {

        }

    }
}
