using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace FEBuilderGBA
{
    public partial class MapStyleEditorFormWarningVanillaTileOverraideForm : Form
    {
        public MapStyleEditorFormWarningVanillaTileOverraideForm()
        {
            InitializeComponent();
            string lang = OptionForm.lang();
            if (lang == "ja")
            {
                HelpTextBox.Text = "https://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:%E3%82%BF%E3%82%A4%E3%83%AB%E3%83%91%E3%83%AC%E3%83%83%E3%83%88%E3%81%AE%E5%A4%89%E6%9B%B4";
            }
            else
            {
                HelpTextBox.Text = "https://dw.ngmansion.xyz/doku.php?id=en:guide:febuildergba:how_to_change_tile_style_en";
            }
        }

        static bool IsSilent = false;
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void OverraideButton_Click(object sender, EventArgs e)
        {
            IsSilent = this.SilentCheckBox.Checked;
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void HelpTextBox_DoubleClick(object sender, EventArgs e)
        {
            U.OpenURLOrFile(HelpTextBox.Text);
        }

        public static bool CheckWarningUI(uint addr)
        {
            Debug.Assert(U.isOffset(addr));
            if (IsSilent)
            {
                return false;
            }
            if (U.isSafetyOffset(addr) == false)
            {
                return false;
            }

            if (addr == Program.ROM.RomInfo.vanilla_field_config_address())
            {
            }
            else if (addr == Program.ROM.RomInfo.vanilla_field_image_address())
            {
            }
            else if (addr == Program.ROM.RomInfo.vanilla_village_config_address())
            {
            }
            else if (addr == Program.ROM.RomInfo.vanilla_village_image_address())
            {
            }
            else if (addr == Program.ROM.RomInfo.vanilla_casle_config_address())
            {
            }
            else if (addr == Program.ROM.RomInfo.vanilla_casle_image_address())
            {
            }
            else if (addr == Program.ROM.RomInfo.vanilla_plain_config_address())
            {
            }
            else if (addr == Program.ROM.RomInfo.vanilla_plain_image_address())
            {
            }
            else
            {
                return false;
            }

            MapStyleEditorFormWarningVanillaTileOverraideForm f = (MapStyleEditorFormWarningVanillaTileOverraideForm)InputFormRef.JumpFormLow<MapStyleEditorFormWarningVanillaTileOverraideForm>();
            DialogResult dr = f.ShowDialog();
            return !(dr == System.Windows.Forms.DialogResult.Yes) ;
        }

        private void MapStyleEditorFormWarningVanillaTileOverraideForm_Load(object sender, EventArgs e)
        {
            MyCancelButton.Focus();
        }

    }
}
