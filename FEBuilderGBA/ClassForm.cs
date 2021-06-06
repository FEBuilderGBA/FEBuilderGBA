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
    public partial class ClassForm : Form
    {
        public ClassForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.InputFormRef.PreAddressListExpandsEvent += OnPreClassExtendsWarningHandler;
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
            MagicExtClassGrow.ValueChanged += X_SIM_ValueChanged;

            //初期値
            B11.ValueChanged += X_SIM_ValueChanged;
            B12.ValueChanged += X_SIM_ValueChanged;
            B13.ValueChanged += X_SIM_ValueChanged;
            B14.ValueChanged += X_SIM_ValueChanged;
            B15.ValueChanged += X_SIM_ValueChanged;
            B16.ValueChanged += X_SIM_ValueChanged;
            MagicExtClassBase.ValueChanged += X_SIM_ValueChanged;

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

        Button[] X_SkillButtons;
        PatchUtil.skill_system_enum X_SkillType = PatchUtil.skill_system_enum.NO;
        ToolTipEx X_Tooltip;

        private void ClassForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            this.X_Tooltip = InputFormRef.GetToolTip<ClassForm>();
            InputFormRef.LoadCheckboxesResource("unitclass_checkbox_", controls, this.X_Tooltip, "", "L_40_BIT_", "L_41_BIT_", "L_42_BIT_", "L_43_BIT_");

            if (Program.ROM.RomInfo.version() == 8)
            {//FE8の場合、分岐CCへ
                InputFormRef.markupJumpLabel(this.J_5);

                //スキル
                this.X_SkillType = PatchUtil.SearchSkillSystem();
                if ( this.X_SkillType == PatchUtil.skill_system_enum.SkillSystem
                  || this.X_SkillType == PatchUtil.skill_system_enum.FE8N_ver2
                  || this.X_SkillType == PatchUtil.skill_system_enum.FE8N_ver3
                    )
                {
                    InputFormRef.markupJumpLabel(this.X_CLASSSKILL);
                    this.X_CLASSSKILL.Show();
                    this.X_SkillButtons = new Button[] { X_SKILL_BUTTON1, X_SKILL_BUTTON2, X_SKILL_BUTTON3, X_SKILL_BUTTON4, X_SKILL_BUTTON5, X_SKILL_BUTTON6, X_SKILL_BUTTON7, X_SKILL_BUTTON8, X_SKILL_BUTTON9 };
                    for (int i = 0; i < this.X_SkillButtons.Length; i++)
                    {
                        this.X_SkillButtons[i].Click += X_CLASSSKILL_Button_Click;
                    }
                }
            }
            //魔法分離パッチ
            MagicSplitUtil.magic_split_enum magic_split = MagicSplitUtil.SearchMagicSplit();
            if (magic_split == MagicSplitUtil.magic_split_enum.FE8NMAGIC)
            {
                InitFE8NMagicExtends(controls);
            }
            else if (magic_split == MagicSplitUtil.magic_split_enum.FE7UMAGIC
                || magic_split == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
            {
                InitFE7UMagicExtends(controls);
            }

            //クラス拡張を表示するかどうか
            if (ClassForm.IsShowClassExetdns(this.AddressList))
            {
                AddressListExpandsButton_255.Show();
            }
            else
            {
                this.AddressList.Height += AddressListExpandsButton_255.Height;
                AddressListExpandsButton_255.Hide();
            }

            //SkillSystemsによる 特効リワーク
            InitFE8ClassType(controls);
        
            this.AddressList.Focus();
        }


        public static bool IsShowClassExetdns(ListBox list)
        {
            if (list.Items.Count > 0x80)
            {//拡張している場合、表示する
                return true;
            }
            return (OptionForm.show_class_extends() == OptionForm.show_extends_enum.Show) ;
        }

        //クラス名の取得
        public static String GetClassName(uint cid)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(cid);
            return GetClassNameLow(addr);
        }
        public static String GetClassNameLow(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint id = Program.ROM.u16(addr);
            return TextForm.Direct(id).Trim();
        }

        public static uint GetClassNameID(uint cid)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }
            uint id = Program.ROM.u16(addr);
            return id;
        }

        //顔画像からのクラス名の取得
        public static String GetClassNameWhereFaceID(uint face_id)
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                if (face_id == Program.ROM.u16(addr + 8))
                {
                    uint id = Program.ROM.u16(addr);
                    return TextForm.Direct(id);
                }
                addr += InputFormRef.BlockSize;
            }
            return "";
        }
        //顔画像
        public static Bitmap DrawClassFacePicture(uint cid)
        {
            if (cid <= 0)
            {
                return ImageUtil.BlankDummy();
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }
            uint face_id = Program.ROM.u16(addr + 8);
            return ImagePortraitForm.DrawPortraitClass(face_id);
        }
        //CCした場合のクラスID(実質 FE6,FE7のみ)
        public static uint GetChangeClassID(uint cid)
        {
            if (cid <= 0)
            {
                return 0;
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }
            uint cc_cid = Program.ROM.u8(addr + 5);
            return cc_cid;
        }
        //上位クラスかどうかの判定
        public static bool isHighClass(uint cid)
        {
            if (Program.ROM.RomInfo.version() <= 6)
            {
                return ClassFE6Form.isHighClassFE6(cid);
            }

            if (cid <= 0)
            {
                return false;
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            return isHighClassAddr(addr);
        }
        public static bool isHighClassAddr(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return false;
            }
            uint flag2 = Program.ROM.u8(addr + 41);
            if ((flag2 & 0x01) == 0x01)
            {
                return true;
            }
            return false;
        }
        //ロードユニットフラグの確認
        public static bool isLoadClass(uint cid)
        {
            if (Program.ROM.RomInfo.version() <= 6)
            {
                return ClassFE6Form.isLoadClass(cid);
            }

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

            uint flag2 = Program.ROM.u8(addr + 41);
            return ((flag2 & 0x20) == 0x20);
        }

        //待機アイコン
        public static Bitmap DrawWaitIcon(uint cid, int palette_type = 0, bool height16_limit = false, bool centering = true)
        {
            if (cid <= 0)
            {
                return ImageUtil.BlankDummy();
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }
            uint icon_id = Program.ROM.u8(addr + 6);
            return ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(icon_id, palette_type, height16_limit);
        }

        //待機アイコンからのクラス名の取得
        public static String GetClassNameWhereWaitIconID(uint icon_id)
        {
            InputFormRef InputFormRef = Init(null);

            uint cid = GetClassIDWhereWaitIconID(icon_id);
            return GetClassName(cid);
        }

        //待機アイコンからcidを取得
        public static uint GetClassIDWhereWaitIconID(uint icon_id)
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                if (icon_id == Program.ROM.u8(addr + 6))
                {
                    return (uint)i;
                }
                addr += InputFormRef.BlockSize;
            }
            return U.NOT_FOUND;
        }
        public static uint GetClassWaitIcon(uint cid)
        {
            if (cid <= 0)
            {
                return U.NOT_FOUND;
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }
            uint icon_id = Program.ROM.u8(addr + 6);
            return icon_id;
        }

        //No(移動アイコン)からのクラス名の取得
        public static String GetClassNameWhereNo(uint icon_id)
        {//移動アイコンはそのままcidへ変換可能
            uint cid = icon_id + 1;
            return GetClassName(cid);
        }
        public static uint GetClassMoveIcon(uint cid)
        {
            if (cid <= 0)
            {
                return U.NOT_FOUND;
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }
            uint icon_id = Program.ROM.u8(addr + 4);
            return icon_id;
        }

        //クラスリストを得る
        public static List<U.AddrResult> MakeClassList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        //クラスリストを得る
        public static List<U.AddrResult> MakeClassList(Func<uint, bool> condCallback)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList(condCallback);
        }

        public static uint GetClassAddr(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.IDToAddr(id);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            U.ForceUpdate(X_SIM, GrowSimulator.ClassMaxLevel((uint)this.AddressList.SelectedIndex));

            X_SIM_ValueChanged(null, null);
            SkillUtil.MakeClassSkillButtons(X_SkillType, (uint)this.AddressList.SelectedIndex, this.X_SkillButtons, this.X_Tooltip);

            //SkillSystemsによる 特効リワーク
            if (PatchUtil.SearchClassType() == PatchUtil.class_type_enum.SkillSystems_Rework)
            {
                X_CLASSTYPE.Text = ClassForm.GetClassType((uint)this.D80.Value);
            }
            CheckHardCodingWarning();
        }

        public static void SetSimClass(ref GrowSimulator sim,uint cid)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            sim.SetClassBase(
                  (int)(sbyte)Program.ROM.u8(addr + 11) //hp
                , (int)(sbyte)Program.ROM.u8(addr + 12) //str
                , (int)(sbyte)Program.ROM.u8(addr + 13) //skill
                , (int)(sbyte)Program.ROM.u8(addr + 14) //spd
                , (int)(sbyte)Program.ROM.u8(addr + 15) //def
                , (int)(sbyte)Program.ROM.u8(addr + 16) //res
                , (int)(sbyte)MagicSplitUtil.GetClassBaseMagicExtends(cid, addr) //ext_magic
                );
            sim.SetClassGrow(
                  (int)Program.ROM.u8(addr + 27) //hp
                , (int)Program.ROM.u8(addr + 28) //str
                , (int)Program.ROM.u8(addr + 29) //skill
                , (int)Program.ROM.u8(addr + 30) //spd
                , (int)Program.ROM.u8(addr + 31) //def
                , (int)Program.ROM.u8(addr + 32) //res
                , (int)Program.ROM.u8(addr + 33) //luck
                , (int)MagicSplitUtil.GetClassGrowMagicExtends(cid, addr) //ext_magic
                );
        }
        public GrowSimulator BuildSim()
        {
//            uint uid = UnitForm.GetUnitIDWhereSupportClass((uint)this.AddressList.SelectedIndex);
            uint uid = 0;

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
                , (int)MagicExtClassBase.Value //magic extends
                );
            sim.SetClassGrow(
                  (int)B27.Value //hp
                , (int)B28.Value //str
                , (int)B29.Value //skill
                , (int)B30.Value //spd
                , (int)B31.Value //def
                , (int)B32.Value //res
                , (int)B33.Value //luck
                , (int)MagicExtClassGrow.Value //magic extends
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
                U.SelectedIndexSafety(X_SIM_MAGICEX_Value, sim.sim_ext_magic);
                U.SelectedIndexSafety(X_SIM_SUM_RATE, sim.sim_sum_grow_rate);
            }
        }

        public static uint GetMoveCostPointerAddrLow(uint addr, uint costtype)
        {
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }

            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                switch (costtype)
                {
                    case 0:
                        return addr + 52; //移動コスト
                    case 1:
                        return 0; //移動コスト(雨)  FE6にはない
                    case 2:
                        return 0; //移動コスト(雪)  FE6にはない
                    case 3:
                        return addr + 56; //地形回避
                    case 4:
                        return addr + 60; //地形防御
                    case 5:
                        return addr + 64; //地形魔防
                    case 6:
                        return Program.ROM.RomInfo.terrain_recovery_pointer(); //地形回復
                    case 7:
                        return Program.ROM.RomInfo.terrain_bad_status_recovery_pointer(); //地形ステータス異常回復
                    case 8:
                        return Program.ROM.RomInfo.terrain_show_infomation_pointer(); //地形ウィンドウに情報表示
                    default:
                        return Program.ROM.RomInfo.terrain_recovery_pointer(); //地形回復
                }
            }
            else
            {//FE7 FE8
                switch (costtype)
                {
                    case 0:
                        return addr + 56; //移動コスト
                    case 1:
                        return addr + 60; //移動コスト(雨)
                    case 2:
                        return addr + 64; //移動コスト(雪)
                    case 3:
                        return addr + 68; //地形回避
                    case 4:
                        return addr + 72; //地形防御
                    case 5:
                        return addr + 76; //地形魔防
                    case 6:
                        return Program.ROM.RomInfo.terrain_recovery_pointer(); //地形回復
                    case 7:
                        return Program.ROM.RomInfo.terrain_bad_status_recovery_pointer(); //地形ステータス異常回復
                    case 8:
                        return Program.ROM.RomInfo.terrain_show_infomation_pointer(); //地形ウィンドウに情報表示
                    default:
                        return Program.ROM.RomInfo.terrain_recovery_pointer(); //地形回復
                }
            }
        }


        public static uint GetMoveCostAddrLow(uint addr, uint costtype)
        {
            uint p = GetMoveCostPointerAddrLow(addr, costtype);
            if (!U.isSafetyOffset(p))
            {
                return 0;
            }

            return Program.ROM.p32(p);
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
            uint tokusei2 = Program.ROM.u8(addr+41);
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

        
        //バトルアニメアドレスの取得
        public static uint GetBattleAnimeAddrWhereID(uint cid,out uint out_pointer)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                out_pointer = U.NOT_FOUND;
                return U.NOT_FOUND;
            }

            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                out_pointer = addr + 48;
                return Program.ROM.p32(addr + 48);
            }
            else
            {//FE7 FE8
                out_pointer = addr + 52;
                return Program.ROM.p32(addr + 52);
            }
            
        }
        //バトルアニメアドレスの取得
        public static uint GetBattleAnimeAddrWhereAddr(uint addr, out uint out_pointer)
        {
            if (!U.isSafetyOffset(addr))
            {
                out_pointer = U.NOT_FOUND;
                return U.NOT_FOUND;
            }

            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                out_pointer = addr + 48;
                return Program.ROM.p32(addr + 48);
            }
            else
            {//FE7 FE8
                out_pointer = addr + 52;
                return Program.ROM.p32(addr + 52);
            }

        }
        public static uint GetBattleAnimeAddrWhereID(uint cid)
        {
            uint pointer;
            return GetBattleAnimeAddrWhereID(cid, out pointer);
        }
        //戦闘アニメの設定アドレスから、クラスIDの逆変換
        public static uint GetIDWhereBattleAnimeAddr(uint find_address)
        {
            find_address = U.toOffset(find_address);
            if (!U.isSafetyOffset(find_address))
            {
                return U.NOT_FOUND;
            }

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint p;
                if (Program.ROM.RomInfo.version() == 6)
                {//FE6
                    p = Program.ROM.p32(addr + 48);
                }
                else
                {//FE7 FE8
                    p = Program.ROM.p32(addr + 52);
                }

                if (p == find_address)
                {
                    return (uint)i;
                }
            }

            return U.NOT_FOUND;
        }

        public static uint DataCount()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "Class";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 52 , 56, 60, 64 , 68, 72, 76});

            //移動コスト
            uint addr = InputFormRef.BaseAddress;
            for (uint cid = 0; cid < InputFormRef.DataCount; cid++ ,addr += InputFormRef.BlockSize)
            {
                uint pointer;
                pointer = Program.ROM.p32(addr + 56);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 56
                        , 66
                        , "MoveCost Clear"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 60);
                if (U.isSafetyOffset(pointer))
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 60
                        , 66
                        , "MoveCost Rain"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 64);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 64
                        , 66
                        , "MoveCost Snow"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 68);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 68
                        , 66
                        , "MoveCost avoid"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 72);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 72
                        , 66
                        , "MoveCost def"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
                pointer = Program.ROM.p32(addr + 76);
                if (U.isSafetyOffset(pointer) )
                {
                    FEBuilderGBA.Address.AddPointer(list
                        , addr + 76
                        , 66
                        , "MoveCost ref"
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
            }

            //全クラス共通:地形回復
            FEBuilderGBA.Address.AddPointer(list,
                  Program.ROM.RomInfo.terrain_recovery_pointer()
                , 66
                , "MoveCost ref"
                , FEBuilderGBA.Address.DataTypeEnum.BIN);

            //全クラス共通:地形バッドステータス回復
            FEBuilderGBA.Address.AddPointer(list,
                  Program.ROM.RomInfo.terrain_bad_status_recovery_pointer()
                , 66
                , "MoveCost recovery bad status"
                , FEBuilderGBA.Address.DataTypeEnum.BIN);

            //全クラス共通:地形ウィンドウに情報表示
            FEBuilderGBA.Address.AddPointer(list,
                  Program.ROM.RomInfo.terrain_show_infomation_pointer()
                , 66
                , "MoveCost show infomation"
                , FEBuilderGBA.Address.DataTypeEnum.BIN);
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            //戦闘アニメーションを0クリアしておきます。トラブルを避けるため
            //クラスIDもちゃんと割り振ろう
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ClearBattleAnimationPointer");
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);
            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                //クラスID
                Program.ROM.write_u8(addr + 4, (uint)i, undodata);

                //戦闘アニメクリア
                Program.ROM.write_u32(addr + 52, 0, undodata);

                addr += eearg.BlockSize;
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                CCBranchForm.ExpandsArea(this,eearg.OldDataCount, (uint)count, undodata);
            }
            if (Program.ROM.RomInfo.is_multibyte() )
            {
                OPClassAlphaNameForm.ExpandsArea(this,eearg.OldDataCount, (uint)count, undodata);
            }
            //移動アイコンはクラスIDと連動しているので増設しないといけない.
            ImageUnitMoveIconFrom.ExpandsArea(this, eearg.OldDataCount, (uint)count, undodata);

            Program.Undo.Push(undodata);
        }

        public static void OnPreClassExtendsWarningHandler(object sender,EventArgs e)
        {
            InputFormRef.ExpandsEventArgs eventarg = (InputFormRef.ExpandsEventArgs)e;

            DialogResult dr = R.ShowNoYes("クラスの拡張はさまざまな問題を引き起こし危険です。\r\nクラスを拡張するべきではないと思います。\r\nそれでもあなたがクラスを拡張したい場合、クラスを拡張した後に発生する問題を回避するために、以下のパッチを適応してください。\r\n\r\n拡張した0x80以降のクラスをセーブデータに記録できるようにするパッチ\r\n拡張した0x80以降の移動アニメーションを動作させるパッチ(Extended to Moving Map Animation 0xFF)\r\n\r\n本当に、クラスを拡張してもよろしいですか？\r\n");
            if (dr != DialogResult.Yes)
            {//キャンセル.
                eventarg.IsCancel = true;
                return;
            }
        }

        private void J_5_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                InputFormRef.JumpForm<CCBranchForm>((uint)this.AddressList.SelectedIndex);
            }
        }

        private void X_CLASSSKILL_Click(object sender, EventArgs e)
        {
            SkillUtil.JumpClassSkill(X_SkillType, (uint)this.AddressList.SelectedIndex);
        }
        private void X_CLASSSKILL_Button_Click(object sender, EventArgs e)
        {
            SkillUtil.JumpClassSkill(X_SkillType, (uint)this.AddressList.SelectedIndex,sender);
        }

        public static bool IsClassPromotedByAddr(uint addr)
        {
            uint v ;
            if (Program.ROM.RomInfo.version() == 6)
            {
                v = Program.ROM.u8(addr + 37);
            }
            else
            {
                v = Program.ROM.u8(addr + 41);
            }
            return (v & 0x1) == 0x1;
        }

        public static void GetWeaponType(uint cid
            , out bool out_melee
            , out bool out_magic
                )
        {
            uint sword = 0;
            uint lance = 0;
            uint axe = 0;
            uint bow = 0;
            uint staff = 0;
            uint anima = 0;
            uint light = 0;
            uint dark = 0;
            ClassForm.GetWeaponLevel(cid
                , out sword
                , out lance
                , out axe
                , out bow
                , out staff
                , out anima
                , out light
                , out dark
                );
            out_melee = sword > 0 || lance > 0 || axe > 0 || bow > 0;
            out_magic = staff > 0 || anima > 0 || light > 0 || dark > 0;
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
            if (Program.ROM.RomInfo.version() == 6)
            {
                ClassFE6Form.GetWeaponLevel(cid
                    , out out_sword
                    , out out_lance
                    , out out_axe
                    , out out_bow
                    , out out_staff
                    , out out_anima
                    , out out_light
                    , out out_dark
                    );
                return;
            }

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

            out_sword = Program.ROM.u8(addr + 44);
            out_lance = Program.ROM.u8(addr + 45);
            out_axe = Program.ROM.u8(addr + 46);
            out_bow = Program.ROM.u8(addr + 47);
            out_staff = Program.ROM.u8(addr + 48);
            out_anima = Program.ROM.u8(addr + 49);
            out_light = Program.ROM.u8(addr + 50);
            out_dark = Program.ROM.u8(addr + 51);
        }

        static bool isMeleeMagicMix(uint addr)
        {
            //武器
            uint b44 = Program.ROM.u8(addr + 44);
            uint b45 = Program.ROM.u8(addr + 45);
            uint b46 = Program.ROM.u8(addr + 46);
            uint b47 = Program.ROM.u8(addr + 47);
            bool is_melee = b44 > 0 || b45 > 0 || b46 > 0 || b47 > 0;
            //魔法
            uint b48 = Program.ROM.u8(addr + 48);
            uint b49 = Program.ROM.u8(addr + 49);
            uint b50 = Program.ROM.u8(addr + 50);
            uint b51 = Program.ROM.u8(addr + 51);
            bool is_range = b48 > 0 || b49 > 0 || b50 > 0 || b51 > 0;

            return (is_melee && is_range);
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.CLASS, U.NOT_FOUND
                    , R._("クラスデータが極端に少ないです。破損している可能性があります。")));
            }

            bool isFE6 = Program.ROM.RomInfo.version() == 6;

            //武器魔法混在パッチを適応しているか
            bool is_melee_range_fix = PatchUtil.SearchMeleeAndMagicFixPatch();

            uint class_addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, class_addr += InputFormRef.BlockSize)
            {
                uint name = Program.ROM.u16(class_addr + 0);
                FELint.CheckText(name, "NAME1", errors, FELint.Type.CLASS, class_addr, i);

                uint info = Program.ROM.u16(class_addr + 2);
                FELint.CheckText(info, "DETAIL3", errors, FELint.Type.CLASS, class_addr, i);

                uint id = Program.ROM.u8(class_addr + 4);
                if (name == 0 && id == 0)
                {//ただの使っていないデータ
                }
                else
                {//IDチェック
                    FELint.CheckID(id, i, errors, FELint.Type.CLASS, class_addr);
                }

                if (is_melee_range_fix == false)
                {//武器魔法混在パッチがないので、混在のチェックをします
                    bool mix;
                    if (isFE6)
                    {
                        mix = (ClassFE6Form.isMeleeMagicMix(class_addr));
                    }
                    else
                    {
                        mix = (isMeleeMagicMix(class_addr));
                    }

                    if (mix)
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.CLASS, class_addr
                            , R._("武器レベルで、近接と魔法を混在させています。\r\n混在を可能にするパッチを当てていない状態で、近接と魔法を混在すると、戦闘アニメが正しく動作しません。"), i));
                    }
                }

                //移動速度が0or1かどうかチェックする
                uint movespeed = Program.ROM.u8(class_addr + 7);
                if (movespeed >= 2)
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.CLASS, class_addr
                        , R._("「移動速度」に2以上が設定されています。「移動速度」が、2以上になっているユニットは移動した時に正しく描画されません。"), i));
                }

            }
        }

        void InitFE8NMagicExtends(List<Control> controls)
        {
            MagicExtClassBase.ValueChanged += X_SIM_ValueChanged;
            MagicExtClassGrow.ValueChanged += X_SIM_ValueChanged;

            InputFormRef.makeLinkEventHandler("", controls, D80, MagicExtClassBase, 80, "MAGICEX", new string[] { "B0" });
            InputFormRef.makeLinkEventHandler("", controls, D80, MagicExtClassGrow, 80, "MAGICEX", new string[] { "B1" });
            InputFormRef.makeLinkEventHandler("", controls, D80, MagicExtClassPromotionGain, 80, "MAGICEX", new string[] { "B2" });

            MagicExtClassBase.Show();
            MagicExtClassBaseLabel.Show();
            MagicExtClassGrow.Show();
            MagicExtClassGrowLabel.Show();
            MagicExtClassPromotionGain.Show();
            MagicExtClassPromotionGainLabel.Show();
            X_SIM_MAGICEX_Label.Show();
            X_SIM_MAGICEX_Value.Show();

            U.SwapControlPosition(J_18, MagicExtClassBaseLabel);
            U.SwapControlPosition(B18, MagicExtClassBase);
            B18.Location = new Point(J_18.Location.X , B18.Location.Y);

            U.ShiftControlPosition(J_25,J_26);
            U.ShiftControlPosition(B25, B26);
            U.SwapControlPosition(X_SIM_SUM_RATE_Label, X_SIM_MAGICEX_Label);
            U.SwapControlPosition(X_SIM_SUM_RATE, X_SIM_MAGICEX_Value);
        }

        void InitFE7UMagicExtends(List<Control> controls)
        {
            MagicExtClassBase.ValueChanged += X_SIM_ValueChanged;
            MagicExtClassGrow.ValueChanged += X_SIM_ValueChanged;
            MagicExtClassLimit.ValueChanged += X_SIM_ValueChanged;
            AddressList.SelectedIndexChanged += SelectedIndexChangedFE7UMagicExtends;
            this.InputFormRef.PreWriteHandler += WriteButtonFE7UMagicExtends;

            MagicExtClassBase.Show();
            MagicExtClassBaseLabel.Show();
            MagicExtClassGrow.Show();
            MagicExtClassGrowLabel.Show();
            MagicExtClassPromotionGain.Show();
            MagicExtClassPromotionGainLabel.Show();
            MagicExtClassLimit.Show();
            MagicExtClassLimitLabel.Show();
            X_SIM_MAGICEX_Label.Show();
            X_SIM_MAGICEX_Value.Show();

            U.SwapControlPosition(J_18, MagicExtClassBaseLabel);
            U.SwapControlPosition(B18, MagicExtClassBase);
            B18.Location = new Point(J_18.Location.X , B18.Location.Y);

            U.SwapControlPosition(J_26, MagicExtClassLimitLabel);
            U.SwapControlPosition(B26, MagicExtClassLimit);
            B26.Location = new Point(J_26.Location.X, B26.Location.Y);

            U.SwapControlPosition(X_SIM_SUM_RATE_Label, X_SIM_MAGICEX_Label);
            U.SwapControlPosition(X_SIM_SUM_RATE, X_SIM_MAGICEX_Value);

            U.SelectedIndexSafety(MagicExtClassBase, -128, 127, (int)MagicExtClassBase.Value);
        }
        void SelectedIndexChangedFE7UMagicExtends(object sender, EventArgs e)
        {
            if (MagicSplitUtil.SearchMagicSplit() != MagicSplitUtil.magic_split_enum.FE7UMAGIC)
            {
                if (MagicSplitUtil.SearchMagicSplit() != MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                {
                    return;
                }
            }

            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }

            uint cid = (uint)this.AddressList.SelectedIndex;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            this.MagicExtClassBase.Value = (sbyte)MagicSplitUtil.GetClassBaseMagicExtends(cid, addr);
            this.MagicExtClassGrow.Value = MagicSplitUtil.GetClassGrowMagicExtends(cid, addr);
            this.MagicExtClassLimit.Value = MagicSplitUtil.GetClassLimitMagicExtends(cid, addr);
            this.MagicExtClassPromotionGain.Value = MagicSplitUtil.GetClassPromotionGainMagicExtends(cid, addr);
        }
        void WriteButtonFE7UMagicExtends(object sender, EventArgs e)
        {
            if (MagicSplitUtil.SearchMagicSplit() != MagicSplitUtil.magic_split_enum.FE7UMAGIC)
            {
                if (MagicSplitUtil.SearchMagicSplit() != MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                {
                    return;
                }
            }

            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }

            uint cid = (uint)this.AddressList.SelectedIndex;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(cid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "MagicExtends");
            MagicSplitUtil.WriteClassBaseMagicExtends(cid, addr, U.ConvertNUDToUint(this.MagicExtClassBase) , undodata);
            MagicSplitUtil.WriteClassGrowMagicExtends(cid, addr, (uint)this.MagicExtClassGrow.Value , undodata);
            MagicSplitUtil.WriteClassLimitMagicExtends(cid, addr, (uint)this.MagicExtClassLimit.Value, undodata);
            MagicSplitUtil.WriteClassPromotionGainMagicExtends(cid, addr, (uint)this.MagicExtClassPromotionGain.Value, undodata);
            Program.Undo.Push(undodata);
        }

        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.CLASS, InputFormRef, new uint[] { 0, 2 });
        }

        void InitFE8ClassType(List<Control> controls)
        {
            //SkillSystemsによる 特効リワーク
            if (PatchUtil.SearchClassType() == PatchUtil.class_type_enum.SkillSystems_Rework)
            {
                J_80.Text = R._("ClassType");
                InputFormRef.makeJumpEventHandler(D80, J_80, "CLASSTYPE", new string[] { });
                InputFormRef.makeLinkEventHandler("", controls, D80, X_CLASSTYPE, 80, "CLASSTYPE", new string[] { });
                X_CLASSTYPE.Show();
            }
        }

        //Class Type拡張
        public static string GetClassType(uint class_type)
        {
            string text = "";
            if (Program.ROM.RomInfo.version() == 8 && Program.ROM.RomInfo.is_multibyte() == false)
            {
                text = SkillSystemsEffectivenessReworkClassTypeForm.GetText(class_type);
            }
            return text;
        }
        public static Bitmap DrawClassTypeIcon(uint class_type)
        {
            if (Program.ROM.RomInfo.version() == 8 && Program.ROM.RomInfo.is_multibyte() == false)
            {
                return SkillSystemsEffectivenessReworkClassTypeForm.DrawClassTypeIcon(class_type);
            }

            return ImageUtil.BlankDummy();
        }

        void CheckHardCodingWarning()
        {
            uint id = (uint)(this.AddressList.SelectedIndex);
            bool r = Program.AsmMapFileAsmCache.IsHardCodeClass(id);
            HardCodingWarningLabel.Visible = r;
        }
        private void HardCodingWarningLabel_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("HARDCODING_CLASS=" + U.ToHexString2(this.AddressList.SelectedIndex), 0);
        }
    }
}
