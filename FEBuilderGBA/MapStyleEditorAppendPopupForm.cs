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
    public partial class MapStyleEditorAppendPopupForm : Form
    {
        public MapStyleEditorAppendPopupForm()
        {
            InitializeComponent();
            MapPictureBox.HideCommandBar();
        }
        uint MapPointerPListReadOnly;
        public void Init(uint objPList, uint palPList, uint configPList
            ,uint mapPointerPListReadOnly
            ,uint anime1PList,uint anime2PList)
        {
            InitUI();
            MapPointerPListReadOnly = mapPointerPListReadOnly;
            U.SelectedIndexSafety(OBJ1numericUpDown, objPList & 0xff);
            U.SelectedIndexSafety(OBJ2numericUpDown, (objPList >> 8) & 0xff);
            U.SelectedIndexSafety(PALnumericUpDown, palPList);
            U.SelectedIndexSafety(CONFIGnumericUpDown, configPList);
            U.SelectedIndexSafety(ANIME1numericUpDown, anime1PList);
            U.SelectedIndexSafety(ANIME2numericUpDown, anime2PList);
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(objPList, palPList, configPList, mapPointerPListReadOnly));
        }

        void InitUI()
        {
            if (MapPointerForm.IsPlistSplits())
            {
                PLISTExtendsButton.Enabled = false;
                AlreadyExtendsLabel.Show();
            }
            else if (MapPointerForm.IsExtendsPlist())
            {
                PLISTExtendsButton.Enabled = false;
                AlreadyExtendsLabel.Show();
            }
            else
            {
                PLISTExtendsButton.Enabled = true;
                AlreadyExtendsLabel.Hide();
            }

            uint max = MapPointerForm.GetDataCount(MapPointerForm.PLIST_TYPE.UNKNOWN) - 1;
            OBJ1numericUpDown.Maximum = max;
            OBJ1numericUpDown_ValueChanged(null, null);
            OBJ2numericUpDown.Maximum = max;
            OBJ2numericUpDown_ValueChanged(null, null);
            PALnumericUpDown.Maximum = max;
            PALnumericUpDown_ValueChanged(null, null);
            CONFIGnumericUpDown.Maximum = max;
            CONFIGnumericUpDown_ValueChanged(null, null);
            ANIME1numericUpDown.Maximum = max;
            ANIME1numericUpDown_ValueChanged(null, null);
            ANIME2numericUpDown.Maximum = max;
            ANIME2numericUpDown_ValueChanged(null, null);
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

        string PlistToName(uint plist, MapPointerForm.PLIST_TYPE type)
        {
            Debug.Assert(type != MapPointerForm.PLIST_TYPE.UNKNOWN);

            if (plist == 0)
            {
                return "";
            }
            List<uint> maps;
            if (MapPointerForm.IsPlistSplits())
            {//分割しているので、マップ変化だけを調べる.
                maps = MapSettingForm.GetMapIDsWherePlist(type, plist);
                if (maps.Count < 1)
                {
                    if (type == MapPointerForm.PLIST_TYPE.PALETTE)
                    {//パレットはオブジェクトも調べる
                        maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.OBJECT , plist);
                    }
                    else if (type == MapPointerForm.PLIST_TYPE.OBJECT)
                    {//オブジェクトはパレットも調べる
                        maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.PALETTE, plist);
                    }
                    if (type == MapPointerForm.PLIST_TYPE.ANIMATION)
                    {//ANIMATION1はANIMATION2も調べる
                        maps = MapSettingForm.GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE.ANIMATION2, plist);
                    }
                    else if (type == MapPointerForm.PLIST_TYPE.ANIMATION2)
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
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < maps.Count; i++ )
                {
                    if (i != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(MapSettingForm.GetMapName(maps[i]));
                }
                return R._("利用マップ:{0}", sb.ToString() );
            }

            if (type == MapPointerForm.PLIST_TYPE.PALETTE)
            {
                if (this.OBJ1numericUpDown.Value == plist
                  || this.OBJ2numericUpDown.Value == plist
                    )
                {
                    return R._("{0}と重複しています。\r\n未使用のPLISTですが、データを共有する{1}と同じ番号を利用しているため、利用できません。",  R._("オブジェクト"),R._("パレット"));
                }
            }
            else if (type == MapPointerForm.PLIST_TYPE.OBJECT)
            {
                if (this.PALnumericUpDown.Value == plist)
                {
                    return R._("{0}と重複しています。\r\n未使用のPLISTですが、データを共有する{1}と同じ番号を利用しているため、利用できません。", R._("パレット"), R._("オブジェクト"));
                }
            }
            else if (type == MapPointerForm.PLIST_TYPE.ANIMATION)
            {
                if (this.ANIME2numericUpDown.Value == plist)
                {
                    return R._("{0}と重複しています。\r\n未使用のPLISTですが、データを共有する{1}と同じ番号を利用しているため、利用できません。", R._("タイルアニメーション2"), R._("タイルアニメーション1"));
                }
            }
            else if (type == MapPointerForm.PLIST_TYPE.ANIMATION2)
            {
                if (this.ANIME1numericUpDown.Value == plist)
                {
                    return R._("{0}と重複しています。\r\n未使用のPLISTですが、データを共有する{1}と同じ番号を利用しているため、利用できません。", R._("タイルアニメーション1"), R._("タイルアニメーション2"));
                }
            }

            return R._("未使用のPLISTです。新規にデザインを作りたいときに利用します。データはマップスタイルエディタからインポートしてください。");
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public uint GetOBJPLIST()
        {
            return (uint)OBJ1numericUpDown.Value | ((uint)OBJ2numericUpDown.Value << 8);
        }
        public uint GetPALPLIST()
        {
            return (uint)PALnumericUpDown.Value;
        }
        public uint GetCONFIGPLIST()
        {
            return (uint)CONFIGnumericUpDown.Value;
        }
        public uint GetANIME1PLIST()
        {
            return (uint)ANIME1numericUpDown.Value;
        }
        public uint GetANIME2PLIST()
        {
            return (uint)ANIME2numericUpDown.Value;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (OBJ1numericUpDown.Value == 0)
            {
                R.ShowStopError("「オブジェクトタイプ」に 0 を設定することができません。");
                return;
            }
            if (PALnumericUpDown.Value == 0)
            {
                R.ShowStopError("「パレット」に 0 を設定することができません。");
                return;
            }
            if (CONFIGnumericUpDown.Value == 0)
            {
                R.ShowStopError("「チップセットタイプ」に 0 を設定することができません。");
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

            //マップエディタとスタイルエディタが開いていると危険なので一度閉じる.
            InputFormRef.CloseForm<MapStyleEditorForm>();
            InputFormRef.CloseForm<MapEditorForm>();
        }
        private void OBJ1numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint plist = (uint)OBJ1numericUpDown.Value;
            LINK_PLIST_OBJ1.Text = PlistToName(plist, MapPointerForm.PLIST_TYPE.OBJECT);
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(GetOBJPLIST(), GetPALPLIST(), GetCONFIGPLIST(), this.MapPointerPListReadOnly));
        }

        private void OBJ2numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint plist = (uint)OBJ2numericUpDown.Value;
            LINK_PLIST_OBJ2.Text = PlistToName(plist, MapPointerForm.PLIST_TYPE.OBJECT);
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(GetOBJPLIST(), GetPALPLIST(), GetCONFIGPLIST(), this.MapPointerPListReadOnly));
        }

        private void PALnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint plist = (uint)PALnumericUpDown.Value;
            LINK_PLIST_PAL.Text = PlistToName(plist, MapPointerForm.PLIST_TYPE.PALETTE);
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(GetOBJPLIST(), GetPALPLIST(), GetCONFIGPLIST(), this.MapPointerPListReadOnly));
        }

        private void CONFIGnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint plist = (uint)CONFIGnumericUpDown.Value;
            LINK_PLIST_CONFIG.Text = PlistToName(plist, MapPointerForm.PLIST_TYPE.CONFIG);
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(GetOBJPLIST(), GetPALPLIST(), GetCONFIGPLIST(), this.MapPointerPListReadOnly));
        }

        private void ANIME1numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint plist = (uint)ANIME1numericUpDown.Value;
            LINK_PLIST_ANIME1.Text = PlistToName(plist, MapPointerForm.PLIST_TYPE.ANIMATION);
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(GetOBJPLIST(), GetPALPLIST(), GetCONFIGPLIST(), this.MapPointerPListReadOnly));
        }

        private void ANIME2numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint plist = (uint)ANIME2numericUpDown.Value;
            LINK_PLIST_ANIME2.Text = PlistToName(plist, MapPointerForm.PLIST_TYPE.ANIMATION2);
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(GetOBJPLIST(), GetPALPLIST(), GetCONFIGPLIST(), this.MapPointerPListReadOnly));
        }

    }
}
