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
    public partial class EventUnitColorForm : Form
    {
        public EventUnitColorForm()
        {
            InitializeComponent();

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("", controls);
            U.AddCancelButton(this);

            MakeSlotCombobox(this.PlayerComboBox);
            MakeSlotCombobox(this.EnemyComboBox);
            MakeSlotCombobox(this.NPCComboBox);
            MakeSlotCombobox(this.FourthComboBox);
        }
        public Button GetApplyButton()
        {
            return ApplyButton;
        }
        void MakeSlotCombobox(ComboBox cbox)
        {
            cbox.Items.Add("0=変更せず");
            cbox.Items.Add("1=青");
            cbox.Items.Add("2=赤");
            cbox.Items.Add("3=緑");
            cbox.Items.Add("4=セピア");
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            uint a = U.atoh(this.PlayerComboBox.Text);
            uint b = U.atoh(this.EnemyComboBox.Text);
            uint c = U.atoh(this.NPCComboBox.Text);
            uint d = U.atoh(this.FourthComboBox.Text);

            ApplyButton.Tag = a | (b << 4) | (c << 8) | (d << 12);
        }

        public void JumpTo(uint value)
        {
            uint a = value & 0xF;
            uint b = (value>>4) & 0xF;
            uint c = (value>>8) & 0xF;
            uint d = (value >> 12) & 0xF;

            U.SelectedIndexSafety(this.PlayerComboBox, a);
            U.SelectedIndexSafety(this.EnemyComboBox, b);
            U.SelectedIndexSafety(this.NPCComboBox, c);
            U.SelectedIndexSafety(this.FourthComboBox, c);

            MESSAGE.Text = R._("変更する色を指定してください。");
        }

        private void EventUnitColorForm_Load(object sender, EventArgs e)
        {
            this.PlayerPictureBox.Image = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x8, 0, true);
            this.EnemyPictureBox.Image = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x8, 2, true);
            this.NPCPictureBox.Image = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x8, 1, true);
            this.FourthPictureBox.Image = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x8, 3, true);
        }

    }
}
