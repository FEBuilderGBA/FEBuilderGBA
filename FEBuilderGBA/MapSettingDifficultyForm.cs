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
    public partial class MapSettingDifficultyForm : Form
    {
        public MapSettingDifficultyForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        public uint GetDifficultyValue()
        {
            return (uint)this.DifficultyValue.Value;
        }
        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void MapSettingDifficultyForm_Load(object sender, EventArgs e)
        {

        }

        private void HardBoost_ValueChanged(object sender, EventArgs e)
        {
            this.DifficultyValue.Value =
            ((uint)this.HardBoost.Value << 4) | ((uint)this.EasyPenalty.Value << 0) | ((uint)this.NormalPenalty.Value << 8);
        }
        public void SetDifficultyValue(uint u16Difficulty)
        {
            this.DifficultyValue.Value = u16Difficulty & 0xffff;
            this.HardBoost.Value = (u16Difficulty & 0xf0) >> 4;
            this.EasyPenalty.Value = (u16Difficulty & 0x0f);
            this.NormalPenalty.Value = (u16Difficulty & 0x0f00) >> 8;
        }
        public static string GetDiffcultyText(uint u16Difficulty)
        {
            uint hardhBoost = (u16Difficulty & 0xf0) >> 4;
            uint easyPenalty = (u16Difficulty & 0x0f);
            uint normalPenalty = (u16Difficulty & 0x0f00) >> 8;

            return R._("Hard:+{0} Normal:-{1} Easy:-{2}" ,hardhBoost,normalPenalty,easyPenalty );
        }
    }
}
