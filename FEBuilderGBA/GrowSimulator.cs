using System;
using System.Collections.Generic;

using System.Text;

namespace FEBuilderGBA
{
    public class GrowSimulator
    {
        public int unit_lv { get; private set; }
        public int unit_hp { get; private set; }
        public int unit_str { get; private set; }
        public int unit_skill { get; private set; }
        public int unit_spd { get; private set; }
        public int unit_def { get; private set; }
        public int unit_res { get; private set; }
        public int unit_luck { get; private set; }
        public int unit_ext_magic { get; private set; }

        public int unit_grow_hp { get; private set; }
        public int unit_grow_str { get; private set; }
        public int unit_grow_skill { get; private set; }
        public int unit_grow_spd { get; private set; }
        public int unit_grow_def { get; private set; }
        public int unit_grow_res { get; private set; }
        public int unit_grow_luck { get; private set; }
        public int unit_grow_ext_magic { get; private set; }

        public int class_hp { get; private set; }
        public int class_str { get; private set; }
        public int class_skill { get; private set; }
        public int class_spd { get; private set; }
        public int class_def { get; private set; }
        public int class_res { get; private set; }
        public int class_ext_magic { get; private set; }

        public int class_grow_hp { get; private set; }
        public int class_grow_str { get; private set; }
        public int class_grow_skill { get; private set; }
        public int class_grow_spd { get; private set; }
        public int class_grow_def { get; private set; }
        public int class_grow_res { get; private set; }
        public int class_grow_luck { get; private set; }
        public int class_grow_ext_magic { get; private set; }

        public int sim_lv { get; private set; }
        public int sim_hp { get; private set; }
        public int sim_str { get; private set; }
        public int sim_skill { get; private set; }
        public int sim_spd { get; private set; }
        public int sim_def { get; private set; }
        public int sim_res { get; private set; }
        public int sim_luck { get; private set; }
        public int sim_ext_magic { get; private set; }

        public int sim_sum_grow_rate { get; private set; }

        public static int ClassMaxLevel(uint classs_id)
        {
            int max_level = (int)PatchUtil.GetLevelMaxCaps();
            if (ClassForm.isHighClass(classs_id))
            {//上級職は、最大LV+10として計算します.
                return max_level + 10;
            }
            return max_level;
        }
        public static int CalcMaxLevel(uint shien_classs_id)
        {
            int cc_count = CCBranchForm.GetCCCount(shien_classs_id);

            int level;

            //最大値LV
            int max_level = (int)Program.ROM.u8(Program.ROM.RomInfo.max_level_address());
            if (cc_count >= 2)
            {//見習いからのCC
                level = max_level * (cc_count + 1) - 10;
            }
            else
            {
                level = max_level * (cc_count + 1);
            }
            if (level <= 0)
            {
                level = 0;
            }
            else if (level >= 255)
            {
                level = 255;
            }
            return level;
        }

        public void SetUnitBase(int lv, int hp, int str, int skill, int spd, int def, int res, int luck , int ext_magic)
        {
            this.unit_lv = lv;
            this.unit_hp = hp;
            this.unit_str = str;
            this.unit_skill = skill;
            this.unit_spd = spd;
            this.unit_def = def;
            this.unit_res = res;
            this.unit_luck = luck;
            this.unit_ext_magic = ext_magic;
            
        }
        public void SetClassBase(int hp, int str, int skill, int spd, int def, int res, int ext_magic)
        {
            this.class_hp = hp;
            this.class_str = str;
            this.class_skill = skill;
            this.class_spd = spd;
            this.class_def = def;
            this.class_res = res;
            this.class_ext_magic = ext_magic;
        }
        public void SetUnitGrow(int hp, int str, int skill, int spd, int def, int res, int luck, int ext_magic)
        {
            this.unit_grow_hp = hp;
            this.unit_grow_str = str;
            this.unit_grow_skill = skill;
            this.unit_grow_spd = spd;
            this.unit_grow_def = def;
            this.unit_grow_res = res;
            this.unit_grow_luck = luck;
            this.unit_grow_ext_magic = ext_magic;
        }
        public void SetClassGrow(int hp, int str, int skill, int spd, int def, int res, int luck, int ext_magic)
        {
            this.class_grow_hp = hp;
            this.class_grow_str = str;
            this.class_grow_skill = skill;
            this.class_grow_spd = spd;
            this.class_grow_def = def;
            this.class_grow_res = res;
            this.class_grow_luck = luck;
            this.class_grow_ext_magic = ext_magic;
        }

        public void SetUnitLv1()
        {
            this.unit_lv = 1;
        }

        public enum GrowOptionEnum
        {
            None,
            UnitGrow,
            ClassGrow,
        }
        public void Grow(int lv, GrowOptionEnum growOption)
        {
            this.sim_lv = this.unit_lv;
            this.sim_hp = this.unit_hp + this.class_hp;
            this.sim_str = this.unit_str + this.class_str;
            this.sim_skill = this.unit_skill + this.class_skill;
            this.sim_spd = this.unit_spd + this.class_spd;
            this.sim_def = this.unit_def + this.class_def;
            this.sim_res = this.unit_res + this.class_res;
            this.sim_luck = this.unit_luck + 0;
            this.sim_ext_magic = this.unit_ext_magic + this.class_ext_magic;

            int grow_hp = this.unit_grow_hp;
            int grow_str = this.unit_grow_str;
            int grow_skill = this.unit_grow_skill;
            int grow_spd = this.unit_grow_spd;
            int grow_def = this.unit_grow_def;
            int grow_res = this.unit_grow_res;
            int grow_luck = this.unit_grow_luck;
            int grow_ext_magic = this.unit_grow_ext_magic;
            if (growOption == GrowOptionEnum.ClassGrow)
            {
                grow_hp = this.class_grow_hp;
                grow_str = this.class_grow_str;
                grow_skill = this.class_grow_skill;
                grow_spd = this.class_grow_spd;
                grow_def = this.class_grow_def;
                grow_res = this.class_grow_res;
                grow_luck = this.class_grow_luck;
                grow_ext_magic = this.class_grow_ext_magic;
            }
            else if (growOption == GrowOptionEnum.None)
            {
                grow_hp = 0;
                grow_str = 0;
                grow_skill = 0;
                grow_spd = 0;
                grow_def = 0;
                grow_res = 0;
                grow_luck = 0;
                grow_ext_magic = 0;
            }

            if (this.sim_lv < lv)
            {
                int grow_count = lv - this.sim_lv;
                this.sim_hp    += (int)Math.Round(((double)grow_hp    / 100) * (grow_count));
                this.sim_str   += (int)Math.Round(((double)grow_str   / 100) * (grow_count));
                this.sim_skill += (int)Math.Round(((double)grow_skill / 100) * (grow_count));
                this.sim_spd   += (int)Math.Round(((double)grow_spd   / 100) * (grow_count));
                this.sim_def   += (int)Math.Round(((double)grow_def   / 100) * (grow_count));
                this.sim_res   += (int)Math.Round(((double)grow_res   / 100) * (grow_count));
                this.sim_luck  += (int)Math.Round(((double)grow_luck  / 100) * (grow_count));
                this.sim_ext_magic += (int)Math.Round(((double)grow_ext_magic / 100) * (grow_count));
                this.sim_lv = lv;
            }

            this.sim_sum_grow_rate = grow_hp + grow_str + grow_skill + grow_spd + grow_def + grow_res + grow_luck + grow_ext_magic; 

            if (sim_hp > 256) sim_hp = 255;
            if (sim_str > 256) sim_str = 255;
            if (sim_skill > 256) sim_skill = 255;
            if (sim_spd > 256) sim_spd = 255;
            if (sim_def > 256) sim_def = 255;
            if (sim_res > 256) sim_res = 255;
            if (sim_luck > 256) sim_luck = 255;
            if (sim_ext_magic > 256) sim_ext_magic = 255;
            if (sim_hp < 0) sim_hp = 0;
            if (sim_str < 0) sim_str = 0;
            if (sim_skill < 0) sim_skill = 0;
            if (sim_spd < 0) sim_spd = 0;
            if (sim_def < 0) sim_def = 0;
            if (sim_res < 0) sim_res = 0;
            if (sim_luck < 0) sim_luck = 0;
            if (sim_ext_magic < 0) sim_ext_magic = 0;
        }
    }
}
