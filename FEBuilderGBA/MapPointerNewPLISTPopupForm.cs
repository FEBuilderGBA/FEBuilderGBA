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
    public partial class MapPointerNewPLISTPopupForm : Form
    {
        public MapPointerNewPLISTPopupForm()
        {
            InitializeComponent();
        }
        public void Init(MapPointerForm.PLIST_TYPE searchType)
        {
            this.SearchType = searchType;
            InitUI();
        }

        void InitUI()
        {
            if (MapPointerForm.IsPlistSplits())
            {
                PLISTExtendsButton.Enabled = false;
                AlreadyExtendsLabel.Show();
                PLIST_EXPLAIN.Hide();
            }
            else if (MapPointerForm.IsExtendsPlist())
            {
                PLISTExtendsButton.Enabled = false;
                AlreadyExtendsLabel.Show();
                PLIST_EXPLAIN.Show();
            }
            else
            {
                PLISTExtendsButton.Enabled = true;
                AlreadyExtendsLabel.Hide();
                PLIST_EXPLAIN.Show();
            }

            Debug.Assert(SearchType != MapPointerForm.PLIST_TYPE.UNKNOWN);
            uint max = MapPointerForm.GetDataCount(this.SearchType) - 1;
            PLISTnumericUpDown.Maximum = max;

            PLISTnumericUpDown_ValueChanged(null, null);
        }

        private void PLISTExtendsButton_Click(object sender, EventArgs e)
        {
            MapPointerForm f = (MapPointerForm)InputFormRef.JumpForm<MapPointerForm>();
            f.PushSplitButton((s,ee) => {
                InitUI();
                f.Close();
                return;
            });
        }

        private void MapPointerNewPLISTPopupForm_Load(object sender, EventArgs e)
        {

        }

        private void PLISTnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint plist = (uint)PLISTnumericUpDown.Value;
            LINK_PLIST.Text = PlistToName(plist);
        }

        MapPointerForm.PLIST_TYPE SearchType = MapPointerForm.PLIST_TYPE.UNKNOWN;
        bool IsAlreadyUse;
        string PlistToName(uint plist)
        {
            Debug.Assert(SearchType != MapPointerForm.PLIST_TYPE.UNKNOWN);

            if (plist == 0)
            {
                IsAlreadyUse = true;
                return R._("PLIST=0は、書き込み禁止です。");
            }
            List<uint> maps;
            if (MapPointerForm.IsPlistSplits())
            {//分割しているので、マップ変化だけを調べる.
                maps = MapSettingForm.GetMapIDsWherePlist(this.SearchType, plist);
                if (maps.Count < 1)
                {
                    if (this.SearchType == MapPointerForm.PLIST_TYPE.PALETTE)
                    {//パレットはオブジェクトも調べる
                        maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.OBJECT , plist);
                    }
                    else if (this.SearchType == MapPointerForm.PLIST_TYPE.OBJECT)
                    {//オブジェクトはパレットも調べる
                        maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.PALETTE, plist);
                    }
                    else if (this.SearchType == MapPointerForm.PLIST_TYPE.ANIMATION)
                    {//ANIMATION1はANIMATION2も調べる
                        maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.ANIMATION2, plist);
                    }
                    else if (this.SearchType == MapPointerForm.PLIST_TYPE.ANIMATION2)
                    {//ANIMATION2はANIMATION1も調べる
                        maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.ANIMATION, plist);
                    }
                }
            }
            else
            {//分割していないので、全部調べる必要がある
                maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.UNKNOWN, plist);
            }
            if (maps.Count >= 1)
            {
                IsAlreadyUse = true;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < maps.Count; i++ )
                {
                    if (i != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(MapSettingForm.GetMapName(maps[i]));
                }
                return R._("既に利用されています。\r\n利用マップ:{0}", sb.ToString() );
            }
            IsAlreadyUse = false;
            return R._("マップ設定では参照されていません。\r\n(このPLISTの利用を推奨します)");
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public uint GetSelectPLIST()
        {
            return (uint)PLISTnumericUpDown.Value;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (this.IsAlreadyUse)
            {
                DialogResult dr = R.ShowNoYes("このPLISTはすでに利用されています。\r\n利用されているPLISTを上書きするのは危険です。\r\n本当に、このPLISTにデータを割り当てますか？\r\n");
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
