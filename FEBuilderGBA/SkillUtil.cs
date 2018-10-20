using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;

namespace FEBuilderGBA
{
    static class SkillUtil
    {
        public static void JumpClassSkill(InputFormRef.skill_system_enum skill,uint cid)
        {
            if (skill == InputFormRef.skill_system_enum.SkillSystem)
            {
                InputFormRef.JumpForm<SkillAssignmentClassSkillSystemForm>(cid);
            }
            else if (skill == InputFormRef.skill_system_enum.FE8N_ver2)
            {
                InputFormRef.JumpForm<SkillConfigFE8NVer2SkillForm>();
            }
        }
        public static void JumpClassSkill(InputFormRef.skill_system_enum skill, uint cid,object sender)
        {
            if (!(sender is Control))
            {
                return;
            }
            Control c = (Control)sender;
            if (!(c.Tag is uint))
            {
                return;
            }
            uint skillid = (uint)c.Tag;

            if (skill == InputFormRef.skill_system_enum.SkillSystem)
            {
                InputFormRef.JumpForm<SkillAssignmentClassSkillSystemForm>(cid);
            }
            else if (skill == InputFormRef.skill_system_enum.FE8N_ver2)
            {
                InputFormRef.JumpForm<SkillConfigFE8NVer2SkillForm>(skillid);
            }
        }
        public static void MakeClassSkillButtons(InputFormRef.skill_system_enum skill, uint cid, Button[] buttons, ToolTipEx tooltip)
        {
            if (buttons == null)
            {
                return;
            }

            int skillCount = 0;
            if (skill == InputFormRef.skill_system_enum.SkillSystem)
            {
                skillCount = SkillAssignmentClassSkillSystemForm.MakeClassSkillButtons(cid, buttons, tooltip);
            }
            else if (skill == InputFormRef.skill_system_enum.FE8N_ver2)
            {
                skillCount = SkillConfigFE8NVer2SkillForm.MakeClassSkillButtons(cid, buttons, tooltip);
            }
            SkillUtil.ApplyModButton(buttons, skillCount);
        }
        static void ApplyModButton(Button[] buttons,int mod = 0)
        {
            for (int i = 0; i < mod; i++)
            {
                buttons[i].Show();
            }
            for (int i = mod; i < buttons.Length; i++)
            {
                buttons[i].Hide();
            }
        }
        public static void JumpUnitSkill(InputFormRef.skill_system_enum skill, uint uid)
        {
            if (skill == InputFormRef.skill_system_enum.SkillSystem)
            {
                InputFormRef.JumpForm<SkillAssignmentUnitSkillSystemForm>(uid);
            }
            else if (skill == InputFormRef.skill_system_enum.FE8N_ver2)
            {
                InputFormRef.JumpForm<SkillConfigFE8NVer2SkillForm>();
            }
        }
        public static void JumpUnitSkill(InputFormRef.skill_system_enum skill, uint uid, object sender)
        {
            if (!(sender is Control))
            {
                return;
            }
            Control c = (Control)sender;
            if (!(c.Tag is uint))
            {
                return;
            }
            uint skillid = (uint)c.Tag;

            if (skill == InputFormRef.skill_system_enum.SkillSystem)
            {
                InputFormRef.JumpForm<SkillAssignmentUnitSkillSystemForm>(uid);
            }
            else if (skill == InputFormRef.skill_system_enum.FE8N_ver2)
            {
                InputFormRef.JumpForm<SkillConfigFE8NVer2SkillForm>(skillid);
            }
        }
        public static void MakeUnitSkillButtons(InputFormRef.skill_system_enum skill, uint uid, Button[] buttons, ToolTipEx tooltip)
        {
            if (buttons == null)
            {
                return;
            }

            int skillCount = 0;
            if (skill == InputFormRef.skill_system_enum.SkillSystem)
            {
                skillCount = SkillAssignmentUnitSkillSystemForm.MakeUnitSkillButtons(uid, buttons, tooltip);
            }
            else if (skill == InputFormRef.skill_system_enum.FE8N_ver2)
            {
                skillCount = SkillConfigFE8NVer2SkillForm.MakeUnitSkillButtons(uid, buttons, tooltip);
            }
            SkillUtil.ApplyModButton(buttons, skillCount);
        }


    }
}
