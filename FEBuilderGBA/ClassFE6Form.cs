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
    public partial class ClassFE6Form : Form
    {
        public ClassFE6Form()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.PreAddressListExpandsEvent += ClassForm.OnPreClassExtendsWarningHandler;
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            //成長率
            B27.ValueChanged += X_SIM_ValueChanged;
            B28.ValueChanged += X_SIM_ValueChanged;
            B29.ValueChanged += X_SIM_ValueChanged;
            B30.ValueChanged += X_SIM_ValueChanged;
            B31.ValueChanged += X_SIM_ValueChanged;
            B32.ValueChanged += X_SIM_ValueChanged;
            B33.ValueChanged += X_SIM_ValueChanged;

            //初期値
            B11.ValueChanged += X_SIM_ValueChanged;
            B12.ValueChanged += X_SIM_ValueChanged;
            B13.ValueChanged += X_SIM_ValueChanged;
            B14.ValueChanged += X_SIM_ValueChanged;
            B15.ValueChanged += X_SIM_ValueChanged;
            B16.ValueChanged += X_SIM_ValueChanged;

            X_SIM.ValueChanged += X_SIM_ValueChanged;
            InputFormRef.markupJumpLabel(HardCodingWarningLabel);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.class_pointer()
                , Program.ROM.RomInfo.class_datasize()
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (i == 0)
                    {
                        return true;
                    }
                    if (i > 0xff)
                    {
                        return false;
                    }
                    uint no = Program.ROM.u8(addr + 4);
                    return (no != 0);
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u16(addr);
                    return U.ToHexString(i) + " " + TextForm.Direct(id);
                }
                );
        }

        private void ClassFE6Form_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            ToolTipEx tooltip = InputFormRef.GetToolTip<ClassFE6Form>();
            InputFormRef.LoadCheckboxesResource("unitclass_checkbox_", controls, tooltip, "", "L_36_BIT_", "L_37_BIT_", "L_38_BIT_", "L_39_BIT_");

            if (ClassForm.IsShowClassExetdns(this.AddressList))
            {
                AddressListExpandsButton_255.Show();
            }
            else
            {
                this.AddressList.Height += AddressListExpandsButton_255.Height;
                AddressListExpandsButton_255.Hide();
            }
            this.AddressList.Focus();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            U.ForceUpdate(X_SIM, GrowSimulator.CalcMaxLevel((uint)this.AddressList.SelectedIndex));

            X_SIM_ValueChanged(null, null);
            CheckHardCodingWarning();
        }

        public static void GetSim(ref GrowSimulator sim,uint cid)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            sim.SetClassBase(
                  (int)Program.ROM.u8(addr + 11) //hp
                , (int)Program.ROM.u8(addr + 12) //str
                , (int)Program.ROM.u8(addr + 13) //skill
                , (int)Program.ROM.u8(addr + 14) //spd
                , (int)Program.ROM.u8(addr + 15) //def
                , (int)Program.ROM.u8(addr + 16) //res
                , 0 //magic extends
                );
            sim.SetClassGrow(
                  (int)Program.ROM.u8(addr + 27) //hp
                , (int)Program.ROM.u8(addr + 28) //str
                , (int)Program.ROM.u8(addr + 29) //skill
                , (int)Program.ROM.u8(addr + 30) //spd
                , (int)Program.ROM.u8(addr + 31) //def
                , (int)Program.ROM.u8(addr + 32) //res
                , (int)Program.ROM.u8(addr + 33) //luck
                , 0 //magic extends
                );
        }
        public GrowSimulator BuildSim()
        {
            uint uid = UnitForm.GetUnitIDWhereSupportClass((uint)this.AddressList.SelectedIndex);

            GrowSimulator sim = new GrowSimulator();
            UnitForm.SetSimUnit(ref sim
                , uid //支援クラス
            );
            sim.SetClassBase(
                  (int)B11.Value //hp
                , (int)B12.Value //str
                , (int)B13.Value //skill
                , (int)B14.Value //spd
                , (int)B15.Value //def
                , (int)B16.Value //res
                , 0 //magic extends
                );
            sim.SetClassGrow(
                  (int)B27.Value //hp
                , (int)B28.Value //str
                , (int)B29.Value //skill
                , (int)B30.Value //spd
                , (int)B31.Value //def
                , (int)B32.Value //res
                , (int)B33.Value //luck
                , 0 //magic extends
                );

            return sim;
        }

        private void X_SIM_ValueChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef != null && this.InputFormRef.IsUpdateLock)
            {
                return;
            }

            using (U.ActiveControlSave uac = new U.ActiveControlSave(this))
            {
                GrowSimulator sim = BuildSim();
                sim.Grow((int)X_SIM.Value, GrowSimulator.GrowOptionEnum.ClassGrow);

                X_SIM.Value = sim.sim_lv;
                U.SelectedIndexSafety(X_SIM_HP, sim.sim_hp);
                U.SelectedIndexSafety(X_SIM_STR, sim.sim_str);
                U.SelectedIndexSafety(X_SIM_SKILL, sim.sim_skill);
                U.SelectedIndexSafety(X_SIM_SPD, sim.sim_spd);
                U.SelectedIndexSafety(X_SIM_DEF, sim.sim_def);
                U.SelectedIndexSafety(X_SIM_RES, sim.sim_res);
                U.SelectedIndexSafety(X_SIM_LUCK, sim.sim_luck);
                U.SelectedIndexSafety(X_SIM_SUM_RATE, sim.sim_sum_grow_rate);
            }
        }

        public static uint GetMoveCostAddrLow(uint addr, uint costtype)
        {
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }

            switch (costtype)
            {
                case 0:
                    return Program.ROM.p32(addr + 52); //移動コスト
                case 1:
                    return 0; //FE6にはない
                case 2:
                    return 0; //FE6にはない
                case 3:
                    return Program.ROM.p32(addr + 56); //地形回避
                case 4:
                    return Program.ROM.p32(addr + 60); //地形防御
                case 5:
                    return Program.ROM.p32(addr + 64); //地形魔防
                default:
                    return Program.ROM.p32(Program.ROM.RomInfo.terrain_recovery_pointer()); //地形回復
            }
        }
        public static uint GetMoveCostAddr(uint cid, uint costtype)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(cid);
            return GetMoveCostAddrLow(addr,costtype);
        }

        //貴方空が飛べまして？
        public static bool IsFlyClassLow(uint addr)
        {
            uint tokusei2 = Program.ROM.u8(addr+37);
            return (tokusei2 & 0x08) > 0 || (tokusei2 & 0x10) > 0;
        }
        //貴方空が飛べまして？
        public static bool IsFlyClass(uint cid)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(cid);
            return IsFlyClassLow(addr);
        }

        //空が飛べるキャラをだれか知りたい.
        public static uint GetFlyClass()
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 1; i < InputFormRef.DataCount; i++)
            {
                if (IsFlyClassLow(addr))
                {
                    return (uint)i;
                }

                addr += InputFormRef.BlockSize;
            }
            return U.NOT_FOUND;
        }
        //上位クラスかどうかの判定
        public static bool isHighClassFE6(uint cid)
        {
            if (cid <= 0)
            {
                return false;
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return false;
            }
            uint flag2 = Program.ROM.u8(addr + 37);
            if ((flag2 & 0x01) == 0x01)
            {
                return true;
            }
            return false;
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            //戦闘アニメーションを0クリアしておきます。トラブルを避けるため
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ClearBattleAnimationPointer");
            addr = addr + ((eearg.OldDataCount - 1) * eearg.BlockSize);            //-1は class は 0x00から始まるため
            for (int i = (int)eearg.OldDataCount - 1; i < count; i++)
            {
                //クラスID
                Program.ROM.write_u8(addr + 4, (uint)i, undodata);

                //戦闘アニメクリア
                Program.ROM.write_u32(addr + 48, 0, undodata);
                addr += eearg.BlockSize;
            }
            if (Program.ROM.RomInfo.is_multibyte())
            {
                OPClassAlphaNameForm.ExpandsArea(this, eearg.OldDataCount, (uint)count, undodata);
            }
            //移動アイコンはクラスIDと連動しているので増設しないといけない.
            ImageUnitMoveIconFrom.ExpandsArea(this, eearg.OldDataCount, (uint)count, undodata);

            Program.Undo.Push(undodata);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "Class";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 48,52, 56 , 60,  64 });


            //移動コスト
            //skip class:0
            uint addr = InputFormRef.BaseAddress + InputFormRef.BlockSize;

            for (uint cid = 1; cid < InputFormRef.DataCount; cid++, addr += InputFormRef.BlockSize)
            {
                uint pointer;
                pointer = Program.ROM.p32(addr + 48);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 48
                        , 52
                        , "MoveCost Clear"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 52);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 52
                        , 52
                        , "MoveCost avoid"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 56);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 56
                        , 52
                        , "MoveCost def"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 60);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 60
                        , 52
                        , "MoveCost ref"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
            }

            //全クラス共通地形回復
            FEBuilderGBA.Address.AddPointer(list,
                 Program.ROM.RomInfo.terrain_recovery_pointer()
                , 52
                , "MoveCost ref"
                , FEBuilderGBA.Address.DataTypeEnum.BIN);

            //全クラス共通地形バッドステータス回復
            FEBuilderGBA.Address.AddPointer(list,
                  Program.ROM.RomInfo.terrain_bad_status_recovery_pointer()
                , 52
                , "MoveCost recovery bad status"
                , FEBuilderGBA.Address.DataTypeEnum.BIN);

            //全クラス共通地形バッドステータス回復
            FEBuilderGBA.Address.AddPointer(list,
                  Program.ROM.RomInfo.terrain_show_infomation_pointer()
                , 52
                , "MoveCost show infomation"
                , FEBuilderGBA.Address.DataTypeEnum.BIN);
        }

        public static bool isMeleeMagicMix(uint addr)
        {
            //武器
            uint b40 = Program.ROM.u8(addr + 40);
            uint b41 = Program.ROM.u8(addr + 41);
            uint b42 = Program.ROM.u8(addr + 42);
            uint b43 = Program.ROM.u8(addr + 43);
            bool is_melee = b40 > 0 || b41 > 0 || b42 > 0 || b43 > 0;
            //魔法
            uint b44 = Program.ROM.u8(addr + 44);
            uint b45 = Program.ROM.u8(addr + 45);
            uint b46 = Program.ROM.u8(addr + 46);
            uint b47 = Program.ROM.u8(addr + 47);
            bool is_range = b44 > 0 || b45 > 0 || b46 > 0 || b47 > 0;

            return (is_melee && is_range);
        }
        public static void GetWeaponLevel(uint cid
            , out uint out_sword
            , out uint out_lance
            , out uint out_axe
            , out uint out_bow
            , out uint out_staff
            , out uint out_anima
            , out uint out_light
            , out uint out_dark
                )
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                out_sword = 0;
                out_lance = 0;
                out_axe = 0;
                out_bow = 0;
                out_staff = 0;
                out_anima = 0;
                out_light = 0;
                out_dark = 0;
                return;
            }

            out_sword = Program.ROM.u8(addr + 40);
            out_lance = Program.ROM.u8(addr + 41);
            out_axe = Program.ROM.u8(addr + 42);
            out_bow = Program.ROM.u8(addr + 43);
            out_staff = Program.ROM.u8(addr + 44);
            out_anima = Program.ROM.u8(addr + 45);
            out_light = Program.ROM.u8(addr + 46);
            out_dark = Program.ROM.u8(addr + 47);
        }

        //ロードユニットフラグの確認
        public static bool isLoadClass(uint cid)
        {
            if (cid <= 0)
            {
                return false;
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (addr == U.NOT_FOUND)
            {
                return false;
            }

            uint flag2 = Program.ROM.u8(addr + 37);
            return ((flag2 & 0x20) == 0x20);
        }

        void CheckHardCodingWarning()
        {
            uint id = (uint)(this.AddressList.SelectedIndex);
            bool r = Program.AsmMapFileAsmCache.IsHardCodeUnit(id);
            HardCodingWarningLabel.Visible = r;
        }
        private void HardCodingWarningLabel_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("HARDCODING_CLASS=" + U.ToHexString2(this.AddressList.SelectedIndex), 0);
        }
    }
}
