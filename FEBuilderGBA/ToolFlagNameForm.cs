using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolFlagNameForm : Form
    {
        public ToolFlagNameForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
            this.FlagNameTextBox.Placeholder = R._("フラグの名前");
        }


        private void ToolFlagNameForm_Load(object sender, EventArgs e)
        {
            Init();
        }

        Dictionary<uint, string> BaseFlag;
        void Init()
        {
            this.BaseFlag = U.LoadDicResource(U.ConfigDataFilename("flag_"));
            List<U.AddrResult> list = Program.FlagCache.MakeList();

            this.AddressList.BeginUpdate();
            this.AddressList.Items.Clear();
            for (int i = 0; i < list.Count ; i++)
            {
                this.AddressList.Items.Add(U.ToHexString(list[i].addr) + " " + list[i].name);
            }
            this.AddressList.EndUpdate();
            this.AddressList.Tag = list;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            if (ar.isNULL())
            {
                this.FlagNameTextBox.Text = "";
                return;
            }

            this.FlagNameTextBox.Text = ar.name;
            DeleteButton.Visible = (this.BaseFlag[ar.addr] != ar.name);
        }
        private void WriteButton_Click(object sender, EventArgs e)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            if (ar.isNULL())
            {
                return;
            }
            string name = this.FlagNameTextBox.Text;
            if (this.BaseFlag[ar.addr] == name)
            {
                return;
            }
            Program.FlagCache.Update(ar.addr, name, this.BaseFlag[ar.addr]);

            int selected = this.AddressList.SelectedIndex;

            //読み直し.
            Init();
            //再選択
            U.ForceUpdate(this.AddressList, selected);

            InputFormRef.ShowWriteNotifyAnimation(this, U.NOT_FOUND);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            this.FlagNameTextBox.Text = "";
            WriteButton.PerformClick();

            //元に戻す.
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            if (ar.isNULL())
            {
                return;
            }
            this.FlagNameTextBox.Text = this.BaseFlag[ar.addr];
        }

        uint MapID = U.NOT_FOUND;

        EventHandler UpdateCallbackEventHandler;
        public void JumpTo(uint value, EventHandler updateCallbackEventHandler, uint mapid)
        {
            this.MapID = mapid;
            this.UpdateCallbackEventHandler = updateCallbackEventHandler;
            uint selected = InputFormRef.AddrToSelect(this.AddressList, value);
            if (selected == U.NOT_FOUND)
            {
                return;
            }
            U.SelectedIndexSafety(this.AddressList, selected);
        }

        private void AddressList_DoubleClick(object sender, EventArgs e)
        {
            if (this.UpdateCallbackEventHandler == null)
            {
                return;
            }
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            InputFormRef.ExpandsEventArgs args = new InputFormRef.ExpandsEventArgs();
            args.NewBaseAddress = ar.addr;

            this.UpdateCallbackEventHandler(sender,args);
            this.Close();
        }

        private void ToolUseFlagButton_Click(object sender, EventArgs e)
        {
            ToolUseFlagForm f = (ToolUseFlagForm)InputFormRef.JumpForm<ToolUseFlagForm>();
            f.JumpToMAPID(this.MapID);
        }

    }
}
