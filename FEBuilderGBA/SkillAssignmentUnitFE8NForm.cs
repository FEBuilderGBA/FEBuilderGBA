using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SkillAssignmentUnitFE8NForm : Form
    {
        public SkillAssignmentUnitFE8NForm()
        {
            InitializeComponent();

            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            Dictionary<uint, string> SCROLL1;
            Dictionary<uint, string> SCROLL2;
            Dictionary<uint, string> MASTERY;

            uint[] pointers;
            if (skill == PatchUtil.skill_system_enum.yugudora)
            {//ユグドラパッチ 初期FE8Nカスタム
                pointers = SkillConfigFE8NSkillForm.FindSkillFE8NVer1IconPointers();
                SCROLL1 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_yugudora_skill1_scroll_"));
                SCROLL2 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_yugudora_skill2_scroll_"));
                MASTERY = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill3_mastery_"));
            }
            else if (skill == PatchUtil.skill_system_enum.midori)
            {//緑パッチ
                pointers = SkillConfigFE8NSkillForm.FindSkillFE8NVer1IconPointers();
                SCROLL1 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_midori_skill1_scroll_"));
                SCROLL2 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_midori_skill2_scroll_"));
                MASTERY = U.LoadDicResource(U.ConfigDataFilename("skill_extends_midori_skill3_mastery_"));
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {//FE8N ver2
                pointers = SkillConfigFE8NVer2SkillForm.FindSkillFE8NVer2IconPointers();
                SCROLL1 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill1_scroll_"));
                SCROLL2 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill2_scroll_"));
                MASTERY = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill3_mastery_"));
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver3)
            {//FE8N ver3
                pointers = SkillConfigFE8NVer3SkillForm.FindSkillFE8NVer3IconPointers();
                SCROLL1 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill1_scroll_"));
                SCROLL2 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill2_scroll_"));
                MASTERY = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill3_mastery_"));
            }
            else
            {//FE8N
                pointers = SkillConfigFE8NSkillForm.FindSkillFE8NVer1IconPointers();
                SCROLL1 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill1_scroll_"));
                SCROLL2 = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill2_scroll_"));
                MASTERY = U.LoadDicResource(U.ConfigDataFilename("skill_extends_FE8N_skill3_mastery_"));
            }

            string find;
            string text;
            uint icon;

            find = U.at(SCROLL1, 0x01);
            icon = FindSkillIconAndText(skill,pointers, find, out text);
            L_40_BIT_01.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_0.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL1, 0x02);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_40_BIT_02.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_1.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL1, 0x04);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_40_BIT_04.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_2.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL1, 0x08);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_40_BIT_08.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_3.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL1, 0x10);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_40_BIT_10.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_4.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL1, 0x20);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_40_BIT_20.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_5.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL1, 0x40);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_40_BIT_40.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_6.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL1, 0x80);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_40_BIT_80.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE0_7.Image = DrawSkillIcon(skill, icon);


            find = U.at(SCROLL2, 0x01);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_01.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_0.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL2, 0x02);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_02.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_1.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL2, 0x04);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_04.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_2.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL2, 0x08);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_08.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_3.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL2, 0x10);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_10.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_4.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL2, 0x20);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_20.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_5.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL2, 0x40);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_40.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_6.Image = DrawSkillIcon(skill, icon);

            find = U.at(SCROLL2, 0x80);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_41_BIT_80.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE1_7.Image = DrawSkillIcon(skill, icon);

            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_0.Text = R._("なし");


            find = U.at(MASTERY, 0x01);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_1.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE2_1.Image = DrawSkillIcon(skill, icon);

            find = U.at(MASTERY, 0x02);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_2.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE2_2.Image = DrawSkillIcon(skill, icon);

            find = U.at(MASTERY, 0x03);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_3.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE2_3.Image = DrawSkillIcon(skill, icon);

            find = U.at(MASTERY, 0x04);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_4.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE2_4.Image = DrawSkillIcon(skill, icon);

            find = U.at(MASTERY, 0x05);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_5.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE2_5.Image = DrawSkillIcon(skill, icon);

            find = U.at(MASTERY, 0x06);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_6.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE2_6.Image = DrawSkillIcon(skill, icon);

            find = U.at(MASTERY, 0x07);
            icon = FindSkillIconAndText(skill, pointers, find, out text);
            L_39_RADIO_7.Text = U.nl2none(icon == U.NOT_FOUND ? find : text);
            IMAGE2_7.Image = DrawSkillIcon(skill, icon);

            InputFormRef.TabControlHideTabOption(SkillTab);

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("", controls);
        }

        static uint FindSkillIconAndText(PatchUtil.skill_system_enum skill, uint[] pointers, string searchSkillName, out string outText)
        {
            if (skill == PatchUtil.skill_system_enum.FE8N_ver3)
            {
                return SkillConfigFE8NVer3SkillForm.FindSkillIconAndText(pointers, searchSkillName, out outText);
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {
                return SkillConfigFE8NVer2SkillForm.FindSkillIconAndText(pointers, searchSkillName,out outText);
            }
            else if (skill == PatchUtil.skill_system_enum.midori)
            {//緑パッチ 詳細不明
                outText = "";
                return U.NOT_FOUND;
            }
            else
            {
                return SkillConfigFE8NSkillForm.FindSkillIconAndText(pointers, searchSkillName, out outText);
            }
        }
        static Bitmap DrawSkillIcon(PatchUtil.skill_system_enum skill, uint icon)
        {
            if (skill == PatchUtil.skill_system_enum.FE8N_ver3)
            {
                return ImageItemIconForm.DrawIconWhereID_UsingWeaponPalette_SKILLFE8NVer2(icon);
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {
                return ImageItemIconForm.DrawIconWhereID_UsingWeaponPalette_SKILLFE8NVer2(icon);
            }
            else
            {
                return ImageItemIconForm.DrawIconWhereID_UsingWeaponPalette(icon);
            }
        }

        private void SkillInputHelperFE8NForm_Load(object sender, EventArgs e)
        {

        }
        public void JumpToSCROLL1(uint value)
        {
            SkillTab.SelectedIndex = 0;
            B40.Value = value;
        }
        public void JumpToSCROLL2(uint value)
        {
            SkillTab.SelectedIndex = 1;
            B41.Value = value;
        }
        public void JumpToMASTERY(uint value)
        {
            SkillTab.SelectedIndex = 2;
            B39.Value = value;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if( SkillTab.SelectedIndex == 0)
            {
                ApplyButton.Tag = (uint)B40.Value;
            }
            else if( SkillTab.SelectedIndex == 1)
            {
                ApplyButton.Tag = (uint)B41.Value;
            }
            else if( SkillTab.SelectedIndex == 2)
            {
                ApplyButton.Tag = (uint)B39.Value;
            }
        }

        public Button GetApplyButton()
        {
            return this.ApplyButton;
        }

    }
}
