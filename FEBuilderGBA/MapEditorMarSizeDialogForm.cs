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
    public partial class MapEditorMarSizeDialogForm : Form
    {
        public MapEditorMarSizeDialogForm()
        {
            InitializeComponent();
            this.DataSize = 0;
            U.AddCancelButton(this);
        }

        uint DataSize;
        MapChangeForm.ChangeSt ChangeData;
        public void Init(uint datasize, MapChangeForm.ChangeSt changedata)
        {
            this.DataSize = datasize;
            this.ChangeData = changedata;

            if (this.ChangeData.no == U.NOT_FOUND)
            {//メインマップ
                W.Value = 15;
                W.Minimum = 15;
            }
            else
            {//変更
                W.Value = 1;
                W.Minimum = 1;
            }

            W_ValueChanged(null,null);
        }

        bool SizeCheck(uint w)
        {
            return ((this.DataSize / 2) % w) == 0;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (!SizeCheck((uint)W.Value))
            {
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void W_ValueChanged(object sender, EventArgs e)
        {
            bool r = SizeCheck((uint)W.Value);
            this.ERROR_WIDTH.Visible = !r;
        }

        private void MapEditorMarSizeDialogForm_Load(object sender, EventArgs e)
        {

        }
    }
}
