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
    public partial class UnitForm : Form
    {
        public UnitForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            //成長率
            B28.ValueChanged += X_SIM_ValueChanged;
            B29.ValueChanged += X_SIM_ValueChanged;
            B30.ValueChanged += X_SIM_ValueChanged;
            B31.ValueChanged += X_SIM_ValueChanged;
            B32.ValueChanged += X_SIM_ValueChanged;
            B33.ValueChanged += X_SIM_ValueChanged;
            B34.ValueChanged += X_SIM_ValueChanged;
            MagicExtUnitBase.ValueChanged += X_SIM_ValueChanged;

            //初期値
            B11.ValueChanged += X_SIM_ValueChanged;
            b12.ValueChanged += X_SIM_ValueChanged;
            b13.ValueChanged += X_SIM_ValueChanged;
            b14.ValueChanged += X_SIM_ValueChanged;
            b15.ValueChanged += X_SIM_ValueChanged;
            b16.ValueChanged += X_SIM_ValueChanged;
            b17.ValueChanged += X_SIM_ValueChanged;
            MagicExtUnitGrow.ValueChanged += X_SIM_ValueChanged;

            X_SIM.ValueChanged += X_SIM_ValueChanged;
            InputFormRef.markupJumpLabel(HardCodingWarningLabel);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            InputFormRef ifr = new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.unit_pointer()
                , Program.ROM.RomInfo.unit_datasize()
                , (int i, uint addr) =>
                {//個数が固定できまっている
                    return i < Program.ROM.RomInfo.unit_maxcount(); 
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u16(addr);
                    return U.ToHexString(i + 1) + " " + TextForm.Direct(id);
                }
                );

            if (Program.ROM.RomInfo.version() == 6)
            {//FE6だと、 0x00 へのポインタしかない・・・
                ifr.ReInit( Program.ROM.p32((Program.ROM.RomInfo.unit_pointer())) + Program.ROM.RomInfo.unit_datasize());
            }
            return ifr;
        }

        Button[] X_SkillButtons;
        PatchUtil.skill_system_enum X_SkillType = PatchUtil.skill_system_enum.NO;
        ToolTipEx X_Tooltip;
        private void UnitForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            X_Tooltip = InputFormRef.GetToolTip<UnitForm>();
            InputFormRef.LoadCheckboxesResource("unitclass_checkbox_", controls, X_Tooltip, "", "L_40_BIT_", "L_41_BIT_", "L_42_BIT_", "L_43_BIT_");

            if (Program.ROM.RomInfo.version() == 8)
            {//FE8の場合
                //スキル
                X_SkillType = PatchUtil.SearchSkillSystem();
                if (X_SkillType == PatchUtil.skill_system_enum.SkillSystem
                 || X_SkillType == PatchUtil.skill_system_enum.FE8N_ver2
                    )
                {
                    InputFormRef.markupJumpLabel(this.X_UNITSKILL);
                    this.X_UNITSKILL.Show();
                    this.X_SkillButtons = new Button[] { X_SKILL_BUTTON1, X_SKILL_BUTTON2, X_SKILL_BUTTON3, X_SKILL_BUTTON4, X_SKILL_BUTTON5, X_SKILL_BUTTON6, X_SKILL_BUTTON7, X_SKILL_BUTTON8, X_SKILL_BUTTON9 };
                    for (int i = 0; i < this.X_SkillButtons.Length; i++)
                    {
                        this.X_SkillButtons[i].Click += X_UNITSKILL_Button_Click;
                    }
                }

                //魔法分離パッチ
                MagicSplitUtil.magic_split_enum magic_split = MagicSplitUtil.SearchMagicSplit();
                if (magic_split == MagicSplitUtil.magic_split_enum.FE8NMAGIC)
                {
                    InitFE8NMagicExtends(controls);
                }
                else if (magic_split == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                {
                    InitFE8UMagicExtends(controls);
                }
            }

            this.AddressList.Focus();
        }

        public static uint GetUnitAddr(uint uid)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.IDToAddr(uid);
        }



        public static void SetSimUnit(ref GrowSimulator sim, uint uid)
        {
            if (uid == 0 || uid == U.NOT_FOUND)
            {
                return ;
            }
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid - 1);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            sim.SetUnitBase((int)Program.ROM.u8(addr+11) //LV
                , (int)(sbyte)Program.ROM.u8(addr + 12) //hp
                , (int)(sbyte)Program.ROM.u8(addr + 13) //str
                , (int)(sbyte)Program.ROM.u8(addr + 14) //skill
                , (int)(sbyte)Program.ROM.u8(addr + 15) //spd
                , (int)(sbyte)Program.ROM.u8(addr + 16) //def
                , (int)(sbyte)Program.ROM.u8(addr + 17) //res
                , (int)(sbyte)Program.ROM.u8(addr + 18) //luck
                , (int)(sbyte)MagicSplitUtil.GetUnitBaseMagicExtends(uid, addr) //magic ext
                );
            sim.SetUnitGrow(
                  (int)Program.ROM.u8(addr + 28) //hp
                , (int)Program.ROM.u8(addr + 29) //str
                , (int)Program.ROM.u8(addr + 30) //skill
                , (int)Program.ROM.u8(addr + 31) //spd
                , (int)Program.ROM.u8(addr + 32) //def
                , (int)Program.ROM.u8(addr + 33) //res
                , (int)Program.ROM.u8(addr + 34) //luck
                , (int)MagicSplitUtil.GetUnitGrowMagicExtends(uid,addr) //magic ext
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
                , (int)MagicExtUnitBase.Value //magic ext
                );
            sim.SetUnitGrow(
                  (int)B28.Value //hp
                , (int)B29.Value //str
                , (int)B30.Value //skill
                , (int)B31.Value //spd
                , (int)B32.Value //def
                , (int)B33.Value //res
                , (int)B34.Value //luck
                , (int)MagicExtUnitGrow.Value //magic ext
                );
            ClassForm.SetSimClass(ref sim
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

            using (U.ActiveControlSave uac = new U.ActiveControlSave(this))
            {
                GrowSimulator sim = BuildSim();
                sim.Grow((int)X_SIM.Value, GrowSimulator.GrowOptionEnum.UnitGrow);

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

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_SIM.Value = GrowSimulator.CalcMaxLevel((uint)B5.Value);
            X_SIM_ValueChanged(null, null);
            SkillUtil.MakeUnitSkillButtons(X_SkillType, (uint)this.AddressList.SelectedIndex+1, this.X_SkillButtons, this.X_Tooltip);
            CheckHardCodingWarning();
            
        }
        //支援クラス
        public static uint GetClassID(uint uid)
        {
            if (uid == 0)
            {
                return 0;
            }
            uid--;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }
            return Program.ROM.u8(addr + 5);
        }

        //CC後のクラス
        public static uint GetHighClass(uint uid)
        {
            if (Program.ROM.RomInfo.version() <= 7)
            {
                return UnitFE7Form.GetHighClassFE7(uid);
            }
            Debug.Assert(false); //未実装
            return U.NOT_FOUND;

        }
        //ユニット名の取得
        public static String GetUnitName(uint uid)
        {
            if (uid == 0)
            {
                return "";
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                if (uid == 0xFFFF)
                {
                    return R._("操作しているユニット");
                }
                if (uid == 0xFFFE)
                {
                    return R._("メモリスロットB 座標");
                }
                if (uid == 0xFFFD)
                {
                    return R._("メモリスロット2 UnitID");
                }
            }

            uid--;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }

            return GetUnitNameByAddr(addr);
        }
        //ユニット名の取得
        public static String GetUnitNameByAddr(uint addr)
        {
            uint id = Program.ROM.u16(addr);
            return TextForm.Direct(id).Trim();
        }
        //ユニット名の取得
        public static String GetUnitNameWithID(uint uid)
        {
            return U.ToHexString(uid) + " " + UnitForm.GetUnitName(uid);
        }
        //ユニットIDの取得
        public static uint GetUnitIDByAddr(uint addr)
        {
            return Program.ROM.u8(addr + 4);
        }

        //顔画像からのユニット名の取得
        public static String GetUnitNameWhereFaceID(uint face_id)
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                if ( face_id == Program.ROM.u16(addr + 6) )
                {
                    uint id = Program.ROM.u16(addr);
                    return TextForm.Direct(id);
                }
                addr += InputFormRef.BlockSize;
            }
            return "";
        }
        //顔画像からIDを取得.
        public static uint GetUnitIDWhereFaceID(uint face_id)
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                if (face_id == Program.ROM.u16(addr + 6))
                {
                    return (uint)i;
                }
                addr += InputFormRef.BlockSize;
            }
            return 0;
        }

        //支援クラスIDからユーザIDに変換 代表的なユーザデータがほしい.
        public static uint GetUnitIDWhereSupportClass(uint support_class_id)
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                if (support_class_id == Program.ROM.u8(addr + 5))
                {
                    return (uint)i;
                }
                addr += InputFormRef.BlockSize;
            }
            return U.NOT_FOUND;
        }

        public static uint GetUnitIDWhereSupportAddr(uint support_addr)
        {
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                if (support_addr == Program.ROM.p32(addr + 44))
                {
                    return (uint)i;
                }
                addr += InputFormRef.BlockSize;
            }
            return U.NOT_FOUND;
        }
        public static uint GetSupportAddrWhereUnitID(uint uid)
        {
            if (uid <= 0)
            {
                return U.NOT_FOUND;
            }
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress + (uid - 1)* InputFormRef.BlockSize;
            return Program.ROM.p32(addr + 44);
        }
        //顔画像
        public static Bitmap DrawUnitFacePicture(uint uid)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return UnitFE6Form.DrawUnitFacePicture(uid);
            }

            if (uid == 0)
            {
                return ImagePortraitForm.DrawPortraitAuto(0);
            }
            uid--;

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return ImagePortraitForm.DrawPortraitAuto(0);
            }
            uint face_id = Program.ROM.u16(addr+6);
            return ImagePortraitForm.DrawPortraitAuto(face_id);
        }
        //顔画像
        public static Bitmap DrawUnitFacePictureByAddr(uint addr,bool showClassCardIfZero)
        {
            if (!U.isSafetyOffset(addr))
            {
                return ImagePortraitForm.DrawPortraitMap(0);
            }
            uint face_id = Program.ROM.u16(addr + 6);
            if (face_id == 0 && showClassCardIfZero)
            {
                uint cid = Program.ROM.u8(addr + 5);
                return ClassForm.DrawClassFacePicture(cid);
            }
            return ImagePortraitForm.DrawPortraitAuto(face_id);
        }
        //マップ顔画像
        public static Bitmap DrawUnitMapFacePicture(uint uid)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                return UnitFE6Form.DrawUnitMapFacePicture(uid);
            }
            if (uid == 0)
            {
                return ImageUtil.BlankDummy();
            }
            uid--;

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return ImagePortraitForm.DrawPortraitMap(0);
            }
            uint face_id = Program.ROM.u16(addr + 6);
            return ImagePortraitForm.DrawPortraitMap(face_id);
        }
        //マップ顔画像
        public static Bitmap DrawUnitMapFacePictureByAddr(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return ImagePortraitForm.DrawPortraitMap(0);
            }
            uint face_id = Program.ROM.u16(addr + 6);
            return ImagePortraitForm.DrawPortraitMap(face_id);
        }
        //会話グループ取得
        public static uint GetTalkGroupByAddr(uint addr)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6にはない
                return U.NOT_FOUND;
            }
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }
            return Program.ROM.u8(addr + 48);
        }
        //仲間にできる最大UID (これ以降は戦績が戦績が記録されない)
        public const uint MAX_PLAYER_UNIT_ID = 0x45;

        //最大の会話グループを取得
        public static uint GetMaxTalkGroup()
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6にはない
                return 0;
            }
            InputFormRef InputFormRef = Init(null);

            uint maxTalkGroup = 0;

            int unitMax = Math.Min((int)InputFormRef.DataCount, (int)UnitForm.MAX_PLAYER_UNIT_ID + 1); //戦績が選べるのは 0x45まで
            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < unitMax; i++ , addr += InputFormRef.BlockSize)
            {
                uint talkGroup = Program.ROM.u8(addr + 48);
                if (maxTalkGroup < talkGroup)
                {
                    maxTalkGroup = talkGroup;
                }
            }
            return maxTalkGroup;
        }


        public static string GetUnitNameAndANY(uint uid)
        {
            if (uid == 0)
            {
                return R._("ANY");
            }
            return UnitForm.GetUnitName(uid);
        }
        //ユニットリストを得る
        public static List<U.AddrResult> MakeUnitList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        //ユニットリストを得る
        public static List<U.AddrResult> MakeUnitList(Func<uint, bool> condCallback)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList(condCallback);
        }

        public static string GetNameWhereCustomBattleAnime(uint custom_battle_id)
        {
            if (custom_battle_id == 0)
            {
                return "";
            }

            if (Program.ROM.RomInfo.version() != 7)
            {//専用アニメはFE7だけ?
                return "";
            }
            return UnitFE7Form.GetNameWhereCustomBattleAnime(custom_battle_id);
        }
        public static uint GetPaletteLowClass(uint uid)
        {
            if (Program.ROM.RomInfo.version() == 7)
            {
                return UnitFE7Form.GetPaletteLowClass(uid);
            }
            if (Program.ROM.RomInfo.version() == 6)
            {
                return UnitFE6Form.GetPaletteLowClass(uid);
            }
            //FE8の場合、CC分岐があるので、別途構造体で定義. UnitPaletteForm.cs
            return U.NOT_FOUND;
    }
        public static uint GetPaletteHighClass(uint uid)
        {
            if (Program.ROM.RomInfo.version() == 7)
            {
                return UnitFE7Form.GetPaletteHighClass(uid);
            }
            if (Program.ROM.RomInfo.version() == 6)
            {
                return UnitFE6Form.GetPaletteHighClass(uid);
            }
            //FE8の場合、CC分岐があるので、別途構造体で定義. UnitPaletteForm.cs
            return U.NOT_FOUND;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {

            string name = "UnitForm";
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 44 });


            //支援データのポインタを書きこむ
            //ポインタはFE6,7,8共有だ、FE6だけ構造体のサイズが違う.
            uint supportStructSize;
            if (Program.ROM.RomInfo.version() == 6)
            {
                supportStructSize = 32;
            }
            else
            {
                supportStructSize = 24;
            }

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
            {
                uint unitSupport = Program.ROM.p32(addr + 44);
                if (unitSupport > 0)
                {
                    FEBuilderGBA.Address.AddAddress(list, unitSupport
                        , supportStructSize
                        , addr + 44
                        , "unitSupport " + U.To0xHexString(i)
                        , FEBuilderGBA.Address.DataTypeEnum.BIN);
                }
            }
        }

        public static uint DataCount()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
        }

        //上級職かどうか
        public static bool IsUnitPromotedByAddr(uint addr)
        {
            uint v = Program.ROM.u8(addr + 41);
            return (v & 0x1) == 0x1;
        }

        private void X_UNITSKILL_Click(object sender, EventArgs e)
        {
            SkillUtil.JumpUnitSkill(X_SkillType, (uint)this.AddressList.SelectedIndex + 1);
        }
        private void X_UNITSKILL_Button_Click(object sender, EventArgs e)
        {
            SkillUtil.JumpUnitSkill(X_SkillType, (uint)this.AddressList.SelectedIndex + 1, sender);
        }

        public static void GetWeaponType(uint uid
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
            UnitForm.GetWeaponLevel(uid
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

        public static void GetWeaponLevel(uint uid
            ,out uint out_sword
            ,out uint out_lance
            ,out uint out_axe
            ,out uint out_bow
            ,out uint out_staff
            ,out uint out_anima
            ,out uint out_light
            ,out uint out_dark
                )
        {
            out_sword = 0;
            out_lance = 0;
            out_axe = 0;
            out_bow = 0;
            out_staff = 0;
            out_anima = 0;
            out_light = 0;
            out_dark = 0;
            if (uid == 0)
            {
                return ;
            }
            uid--;

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return ;
            }

            out_sword = Program.ROM.u8(addr + 20);
            out_lance = Program.ROM.u8(addr + 21);
            out_axe = Program.ROM.u8(addr + 22);
            out_bow = Program.ROM.u8(addr + 23);
            out_staff = Program.ROM.u8(addr + 24);
            out_anima = Program.ROM.u8(addr + 25);
            out_light = Program.ROM.u8(addr + 26);
            out_dark = Program.ROM.u8(addr + 27);
        }

        static bool isMeleeMagicMixAddr(uint addr)
        {
            //武器
            uint b20 = Program.ROM.u8(addr + 20);
            uint b21 = Program.ROM.u8(addr + 21);
            uint b22 = Program.ROM.u8(addr + 22);
            uint b23 = Program.ROM.u8(addr + 23);
            bool is_melee = b20 > 0 || b21 > 0 || b22 > 0 || b23 > 0;
            //魔法
            uint b24 = Program.ROM.u8(addr + 24);
            uint b25 = Program.ROM.u8(addr + 25);
            uint b26 = Program.ROM.u8(addr + 26);
            uint b27 = Program.ROM.u8(addr + 27);
            bool is_range = b24 > 0 || b25 > 0 || b26 > 0 || b27 > 0;

            return (is_melee && is_range);
        }

        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.UNIT, InputFormRef, new uint[] { 0 , 2});
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef = Init(null);
            if (InputFormRef.DataCount < 10)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.UNIT, U.NOT_FOUND
                    , R._("ユニットデータが極端に少ないです。破損している可能性があります。")));
            }

            uint base_pointer = Program.ROM.u32(Program.ROM.RomInfo.support_unit_pointer());
            if (!U.isSafetyPointer(base_pointer))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.UNIT, U.NOT_FOUND
                    , R._("ユニット0x01の支援ポインタが無効です。この値は支援の起点となる値です。バニラの値から変更しないでください。")));
                return;
            }

            //武器魔法混在パッチを適応しているか
            bool is_melee_range_fix = PatchUtil.SearchMeleeAndMagicFixPatch();

            uint unit_addr = InputFormRef.BaseAddress;
            for (uint i = 0; i < InputFormRef.DataCount; i++, unit_addr += InputFormRef.BlockSize)
            {
                uint name = Program.ROM.u16(unit_addr + 0);
                FELint.CheckText(name, "NAME1", errors, FELint.Type.UNIT, unit_addr, i);

                uint info = Program.ROM.u16(unit_addr + 2);
                FELint.CheckText(info, "DETAIL3", errors, FELint.Type.UNIT, unit_addr, i);

                uint id = Program.ROM.u8(unit_addr + 4);
                if (name == 0 && id == 0)
                {//ただの使っていないデータ
                }
                else
                {
                    FELint.CheckID(id, i + 1, errors, FELint.Type.UNIT, unit_addr, i);
                }

                if (is_melee_range_fix == false)
                {//武器魔法混在パッチがないので、混在のチェックをします
                    if (isMeleeMagicMixAddr(unit_addr))
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.UNIT, unit_addr
                            , R._("武器レベルで、近接と魔法を混在させています。\r\n混在を可能にするパッチを当てていない状態で、近接と魔法を混在すると、戦闘アニメが正しく動作しません。"),i));
                    }
                }
                uint support_pointer = Program.ROM.u32(unit_addr + 44);
                SupportUnitForm.MakeCheckErrorAddr(errors, support_pointer, FELint.Type.UNIT, unit_addr, i);
            }
        }

        void InitFE8NMagicExtends(List<Control> controls)
        {
            MagicExtUnitBase.ValueChanged += X_SIM_ValueChanged;
            MagicExtUnitGrow.ValueChanged += X_SIM_ValueChanged;

            InputFormRef.makeLinkEventHandler("", controls, B50, MagicExtUnitBase, 50, "MAGICEX", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, B51, MagicExtUnitGrow, 51, "MAGICEX", new string[] { });

            MagicExtUnitBase.Show();
            MagicExtUnitBaseLabel.Show();
            MagicExtUnitGrow.Show();
            MagicExtUnitGrowLabel.Show();
            X_SIM_MAGICEX_Label.Show();
            X_SIM_MAGICEX_Value.Show();

            U.SwapControlPosition(J_19, MagicExtUnitBaseLabel);
            U.SwapControlPosition(b19, MagicExtUnitBase);
            U.SwapControlPosition(X_SIM_SUM_RATE_Label, X_SIM_MAGICEX_Label);
            U.SwapControlPosition(X_SIM_SUM_RATE, X_SIM_MAGICEX_Value);
        }
        void InitFE8UMagicExtends(List<Control> controls)
        {
            MagicExtUnitBase.ValueChanged += X_SIM_ValueChanged;
            MagicExtUnitGrow.ValueChanged += X_SIM_ValueChanged;
            AddressList.SelectedIndexChanged += SelectedIndexChangedFE8UMagicExtends;
            this.InputFormRef.PreWriteHandler += WriteButtonFE8UMagicExtends;

            MagicExtUnitBase.Show();
            MagicExtUnitBaseLabel.Show();
            MagicExtUnitGrow.Show();
            MagicExtUnitGrowLabel.Show();
            X_SIM_MAGICEX_Label.Show();
            X_SIM_MAGICEX_Value.Show();

            U.SwapControlPosition(J_19, MagicExtUnitBaseLabel);
            U.SwapControlPosition(b19, MagicExtUnitBase);
            U.SwapControlPosition(X_SIM_SUM_RATE_Label, X_SIM_MAGICEX_Label);
            U.SwapControlPosition(X_SIM_SUM_RATE, X_SIM_MAGICEX_Value);

            U.SelectedIndexSafety(MagicExtUnitBase, -128, 127, (int)MagicExtUnitBase.Value);
        }
        void SelectedIndexChangedFE8UMagicExtends(object sender, EventArgs e)
        {
            if (MagicSplitUtil.SearchMagicSplit() != MagicSplitUtil.magic_split_enum.FE8UMAGIC)
            {
                return;
            }

            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }

            uint uid = (uint)this.AddressList.SelectedIndex;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            uid++;
            this.MagicExtUnitBase.Value = (sbyte)MagicSplitUtil.GetUnitBaseMagicExtends(uid, addr);
            this.MagicExtUnitGrow.Value = MagicSplitUtil.GetUnitGrowMagicExtends(uid, addr);
        }

        void WriteButtonFE8UMagicExtends(object sender, EventArgs e)
        {
            if (MagicSplitUtil.SearchMagicSplit() != MagicSplitUtil.magic_split_enum.FE8UMAGIC)
            {
                return;
            }

            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }

            uint uid = (uint)this.AddressList.SelectedIndex;
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            uid++;
            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "MagicExtends");
            MagicSplitUtil.WriteUnitBaseMagicExtends(uid, addr, U.ConvertNUDToUint(this.MagicExtUnitBase), undodata);
            MagicSplitUtil.WriteUnitGrowMagicExtends(uid, addr, (uint)this.MagicExtUnitGrow.Value, undodata);
            Program.Undo.Push(undodata);
        }
        public static bool isMainUnit(uint cid)
        {
            if (Program.ROM.RomInfo.version() <= 6)
            {
                return (cid == 0x01);
            }
            else if (Program.ROM.RomInfo.version() <= 7)
            {
                return (cid >= 0x01 && cid <= 0x03);
            }
            else
            {
                return (cid == 0x01 || cid == 0x0F) ;
            }
        }

        //ロードユニットフラグの確認
        public static bool isLoadClass(uint uid)
        {
            if (uid <= 0)
            {
                return false;
            }
            uid--;

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (addr == U.NOT_FOUND)
            {
                return false;
            }

            uint flag2 = Program.ROM.u8(addr + 41);
            return ((flag2 & 0x20) == 0x20);
        }

        //上位クラスかどうかの判定
        public static bool isHighClass(uint uid)
        {
            if (uid <= 0)
            {
                return false;
            }
            uid--;

            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(uid);
            if (addr == U.NOT_FOUND)
            {
                return false;
            }

            uint flag2 = Program.ROM.u8(addr + 41);
            return ((flag2 & 0x01) == 0x01);
        }

        //支援の付け替え
        public static void ChangeSupportPointer(uint uid, uint support_addr)
        {
            InputFormRef InputFormRef = Init(null);

            Undo.UndoData undodata = Program.Undo.NewUndoData("ChangeSupportPointer", U.ToHexString(uid), U.ToHexString(support_addr));
            uint addr = InputFormRef.BaseAddress;
            for (uint i = 1; i < InputFormRef.DataCount; i++)
            {
                if (uid == i)
                {
                    Program.ROM.write_p32(addr + 44, support_addr, undodata);
                }
                else if (support_addr == Program.ROM.p32(addr + 44))
                {
                    Program.ROM.write_u32(addr + 44, 0, undodata);
                }
                addr += InputFormRef.BlockSize;
            }
            Program.Undo.Push(undodata);
        }

        void CheckHardCodingWarning()
        {
            uint id = (uint)(this.AddressList.SelectedIndex + 1);
            bool r = Program.AsmMapFileAsmCache.IsHardCodeUnit(id);
            HardCodingWarningLabel.Visible = r;
        }
        private void HardCodingWarningLabel_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo("HARDCODING_UNIT=" + U.ToHexString2(this.AddressList.SelectedIndex + 1), 0);
        }

    }
}
