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
    public partial class ToolUseFlagForm : Form
    {
        public ToolUseFlagForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);

            //マップIDリストを作る.
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref  this.MAP_LISTBOX);
            this.MAP_LISTBOX.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
        }

        private void ToolUseFlagForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
            U.SelectedIndexSafety(this.MAP_LISTBOX, 0);
        }

        List<UseFlagID> FlagList = new List<UseFlagID>();
        void UpdateList()
        {
            this.FlagList.Clear();
            uint mapid = (uint)this.MAP_LISTBOX.SelectedIndex;
            EventCondForm.MakeFlagIDArray(mapid, this.FlagList);
            MapChangeForm.MakeFlagIDArray(mapid, this.FlagList);

            List<UseFlagID> flagListInner = new List<UseFlagID>();
            if (Program.ROM.RomInfo.version() == 8)
            {
                EventHaikuForm.MakeFlagIDArray(flagListInner);
                EventBattleTalkForm.MakeFlagIDArray(flagListInner);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {//7
                EventHaikuFE7Form.MakeFlagIDArray(flagListInner);
                EventBattleTalkFE7Form.MakeFlagIDArray(flagListInner);
            }
            else
            {//6
                EventHaikuFE6Form.MakeFlagIDArray(flagListInner);
                EventBattleTalkFE6Form.MakeFlagIDArray(flagListInner);
            }
            foreach (UseFlagID u in flagListInner)
            {
                if (u.MapID == mapid || u.MapID == U.NOT_FOUND)
                {
                    this.FlagList.Add(u);
                }
            }
        }

        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateList();
            AddressList.DummyAlloc(this.FlagList.Count, this.AddressList.SelectedIndex);
        }
    }
}
