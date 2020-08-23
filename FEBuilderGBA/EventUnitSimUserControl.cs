using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventUnitSimUserControl : UserControl
    {
        public EventUnitSimUserControl()
        {
            InitializeComponent();
        }
        private void EventUnitSimUserControl_Load(object sender, EventArgs e)
        {
            InitMagic();
        }

        void InitMagic()
        {
            MagicSplitUtil.magic_split_enum magic_split = MagicSplitUtil.SearchMagicSplit();
            if (magic_split == MagicSplitUtil.magic_split_enum.NO)
            {
                this.Width = X_SIM_MAGICEX_Label.Left;
            }
            else
            {
                X_SIM_MAGICEX_Label.Show();
                X_SIM_MAGICEX_Value.Show();
                this.Width = X_SIM_MAGICEX_Label.Left + X_SIM_MAGICEX_Label.Width;

                X_SIM_MAGICEX_Label.Location = new Point(
                    X_SlidePanel.Location.X,
                    X_SIM_MAGICEX_Label.Location.Y
                    );
                X_SIM_MAGICEX_Value.Location = new Point(
                    X_SlidePanel.Location.X,
                    X_SIM_MAGICEX_Value.Location.Y
                    );
                X_SlidePanel.Location = new Point(
                    X_SlidePanel.Location.X + X_SIM_MAGICEX_Label.Width + 1,
                    X_SlidePanel.Location.Y
                    );
            }
        }

        public void SetParam(uint lv, uint grow, uint unitid, uint classid)
        {
            if (classid == 0)
            {
                classid = UnitForm.GetUnitIDWhereSupportClass(unitid);
            }

            GrowSimulator sim = new GrowSimulator();
            UnitForm.SetSimUnit(ref sim, unitid);
            ClassForm.SetSimClass(ref sim, classid);

            if (UnitForm.isHighClass(unitid) || ClassForm.isHighClass(classid))
            {
                lv += 10;
            }

            GrowSimulator.GrowOptionEnum growOption = GrowSimulator.GrowOptionEnum.None;
            if (grow == 1)
            {
                growOption = GrowSimulator.GrowOptionEnum.ClassGrow;
                sim.SetUnitLv1();
            }
            sim.Grow((int)lv, growOption);

            U.SelectedIndexSafety(X_SIM_HP, sim.sim_hp);
            U.SelectedIndexSafety(X_SIM_STR, sim.sim_str);
            U.SelectedIndexSafety(X_SIM_SKILL, sim.sim_skill);
            U.SelectedIndexSafety(X_SIM_SPD, sim.sim_spd);
            U.SelectedIndexSafety(X_SIM_DEF, sim.sim_def);
            U.SelectedIndexSafety(X_SIM_RES, sim.sim_res);
            U.SelectedIndexSafety(X_SIM_LUCK, sim.sim_luck);
            U.SelectedIndexSafety(X_SIM_MAGICEX_Value, sim.sim_ext_magic);
        }

    }
}
