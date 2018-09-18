using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FEBuilderGBA
{
    public partial class UnitFormFE7 : Form
    {
        public UnitFormFE7()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);

            //成長率
            B28.ValueChanged += X_SIM_ValueChanged;
            B29.ValueChanged += X_SIM_ValueChanged;
            B30.ValueChanged += X_SIM_ValueChanged;
            B31.ValueChanged += X_SIM_ValueChanged;
            B32.ValueChanged += X_SIM_ValueChanged;
            B33.ValueChanged += X_SIM_ValueChanged;
            B34.ValueChanged += X_SIM_ValueChanged;

            //初期値
            B11.ValueChanged += X_SIM_ValueChanged;
            b12.ValueChanged += X_SIM_ValueChanged;
            b13.ValueChanged += X_SIM_ValueChanged;
            b14.ValueChanged += X_SIM_ValueChanged;
            b15.ValueChanged += X_SIM_ValueChanged;
            b16.ValueChanged += X_SIM_ValueChanged;
            b17.ValueChanged += X_SIM_ValueChanged;

            X_SIM.ValueChanged += X_SIM_ValueChanged;
        }

        InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.p32(Program.ROM.RomInfo.unit_pointer())
                , 52
                , (int i, uint addr) =>
                {//個数が固定できまっている
                    return i < Program.ROM.RomInfo.unit_maxcount(); 
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u16(addr);
                    return U.ToHexString(i + 1) + " " + FETextDecode.Direct(id);
                }
                );
        }

        private void UnitFormFE7_Load(object sender, EventArgs e)
        {
        }

        public static void GetSim(ref GrowSimulator sim,uint uid)
        {
            if (uid == 0)
            {
                return ;
            }
            uid--;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!Program.ROM.isSafetyOffset(addr))
            {
                return;
            }

            sim.SetUnitBase((int)Program.ROM.u8(addr+11) //LV
                , (int)Program.ROM.u8(addr + 12) //hp
                , (int)Program.ROM.u8(addr + 13) //str
                , (int)Program.ROM.u8(addr + 14) //skill
                , (int)Program.ROM.u8(addr + 15) //spd
                , (int)Program.ROM.u8(addr + 16) //def
                , (int)Program.ROM.u8(addr + 17) //res
                , (int)Program.ROM.u8(addr + 18) //luck
                );
            sim.SetUnitGrow(
                  (int)Program.ROM.u8(addr + 28) //hp
                , (int)Program.ROM.u8(addr + 29) //str
                , (int)Program.ROM.u8(addr + 30) //skill
                , (int)Program.ROM.u8(addr + 31) //spd
                , (int)Program.ROM.u8(addr + 32) //def
                , (int)Program.ROM.u8(addr + 33) //res
                , (int)Program.ROM.u8(addr + 34) //luck
                );
        }
        public GrowSimulator BuildSim()
        {
            GrowSimulator sim = new GrowSimulator();
            sim.SetUnitBase((int)B11.Value //LV
                , (int)b12.Value //hp
                , (int)b13.Value //str
                , (int)b14.Value //skill
                , (int)b15.Value //spd
                , (int)b16.Value //def
                , (int)b17.Value //res
                , (int)b18.Value //luck
                );
            sim.SetUnitGrow(
                  (int)B28.Value //hp
                , (int)B29.Value //str
                , (int)B30.Value //skill
                , (int)B31.Value //spd
                , (int)B32.Value //def
                , (int)B33.Value //res
                , (int)B34.Value //luck
                );
            ClassForm.GetSim(ref sim
                , (uint)B5.Value //支援クラス
            );

            return sim;
        }

        private void X_SIM_ValueChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef != null && this.InputFormRef.IsUpdateLock)
            {
                return;
            }

            GrowSimulator sim = BuildSim();
            sim.Grow((int)X_SIM.Value,true);

            X_SIM.Value = sim.sim_lv;
            X_SIM_HP.Value = sim.sim_hp;
            X_SIM_STR.Value = sim.sim_str;
            X_SIM_SKILL.Value = sim.sim_skill;
            X_SIM_SPD.Value = sim.sim_spd;
            X_SIM_DEF.Value = sim.sim_def;
            X_SIM_RES.Value = sim.sim_res;
            X_SIM_LUCK.Value = sim.sim_luck;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_SIM.Value = GrowSimulator.CalcMaxLevel((uint)B5.Value);

            X_SIM_ValueChanged(null, null);
        }

        public static uint GetPaletteLowClass(uint uid)
        {
            if (uid == 0)
            {
                return 0;
            }
            uid--;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!Program.ROM.isSafetyOffset(addr))
            {
                return 0;
            }
            return Program.ROM.u8(addr+35);
        }
        public static uint GetPaletteHighClass(uint uid)
        {
            if (uid == 0)
            {
                return 0;
            }
            uid--;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!Program.ROM.isSafetyOffset(addr))
            {
                return 0;
            }
            return Program.ROM.u8(addr + 36);
        }

    }
}
