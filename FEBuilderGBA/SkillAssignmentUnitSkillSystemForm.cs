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
    public partial class SkillAssignmentUnitSkillSystemForm : Form
    {
        public SkillAssignmentUnitSkillSystemForm()
        {
            InitializeComponent();

            uint icon = SkillConfigSkillSystemForm.FindIconPointer();
            uint text = SkillConfigSkillSystemForm.FindTextPointer();
            uint assignUnit = SkillConfigSkillSystemForm.FindAssignPersonalSkillPointer();
            
            if (icon == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、アイコンを取得できません。");
                return;
            }
            if (text == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、テキストを取得できません。");
                return;
            }
            if (assignUnit == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、個人スキルを取得できません。");
                return;
            }
            this.SkillNames = U.LoadDicResource(U.ConfigDataFilename("skill_extends_skillsystem_name_"));

            this.TextBaseAddress = Program.ROM.p32(text);
            this.IconBaseAddress = Program.ROM.p32(icon);
            this.AssignUnitBaseAddress = Program.ROM.p32(assignUnit);

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            InputFormRef.markupJumpLabel(this.J_0_SKILLASSIGNMENT);
            InputFormRef = Init(this, assignUnit);
            InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, uint assignUnit)
        {
            uint unitDataCount = UnitForm.DataCount();

            InputFormRef ifr = new InputFormRef(self
                , ""
                , assignUnit
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < unitDataCount;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + UnitForm.GetUnitName((uint)i);
                }
            );
            return ifr;
        }

        private void SkillAssignmentUnitSkillSystemForm_Load(object sender, EventArgs e)
        {

        }

        uint TextBaseAddress;
        uint IconBaseAddress;
        uint AssignUnitBaseAddress;
        Dictionary<uint, string> SkillNames;


        private void B0_ValueChanged(object sender, EventArgs e)
        {
            SKILLICON.Image = SkillConfigSkillSystemForm.DrawIcon((uint)this.B0.Value,this.IconBaseAddress);
            SKILLTEXT.Text = SkillConfigSkillSystemForm.GetSkillText((uint)this.B0.Value, this.TextBaseAddress);
            SKILLNAME.Text = U.at(this.SkillNames,(uint)this.B0.Value);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            {
                uint assignUnitP = SkillConfigSkillSystemForm.FindAssignPersonalSkillPointer();

                if (assignUnitP == U.NOT_FOUND)
                {
                    return;
                }

                InputFormRef = Init(null, assignUnitP);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "SkillAssignmentUnitSkillSystem", new uint[] { });
            }
        }
        //全データの取得
        public static void ExportAllData(string filename)
        {
            InputFormRef InputFormRef;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            List<string> lines = new List<string>();
            {
                uint assignUnitP = SkillConfigSkillSystemForm.FindAssignPersonalSkillPointer();
                if (assignUnitP == U.NOT_FOUND)
                {
                    return;
                }

                InputFormRef = Init(null, assignUnitP);
                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(U.ToHexString(Program.ROM.u8(p + 0)));
                    lines.Add(sb.ToString());
                }
            }
            File.WriteAllLines(filename, lines);
        }
        public static void ImportAllData(string filename)
        {
            InputFormRef InputFormRef;
            if (InputFormRef.SearchSkillSystem() != InputFormRef.skill_system_enum.SkillSystem)
            {
                return;
            }

            string[] lines = File.ReadAllLines(filename);
            {
                uint assignUnitP = SkillConfigSkillSystemForm.FindAssignPersonalSkillPointer();
                if (assignUnitP == U.NOT_FOUND)
                {
                    return;
                }

                InputFormRef = Init(null, assignUnitP);
                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    if (i >= lines.Length)
                    {
                        break;
                    }
                    uint skill = U.atoh(lines[i]);
                    Program.ROM.write_u8(p + 0 , skill);
                }
            }
            File.WriteAllLines(filename, lines);
        }
        public static int MakeUnitSkillButtons(uint uid, Button[] buttons, ToolTipEx tooltip)
        {
            uint iconP = SkillConfigSkillSystemForm.FindIconPointer();
            uint textP = SkillConfigSkillSystemForm.FindTextPointer();
            uint assignUnitP = SkillConfigSkillSystemForm.FindAssignPersonalSkillPointer();

            if (iconP == U.NOT_FOUND)
            {
                return 0;
            }
            if (textP == U.NOT_FOUND)
            {
                return 0;
            }
            if (assignUnitP == U.NOT_FOUND)
            {
                return 0;
            }


            InputFormRef InputFormRef = Init(null, assignUnitP);
            List<U.AddrResult> list = InputFormRef.MakeList();
            if (uid < 0 || uid >= list.Count)
            {
                return 0;
            }

            uint classaddr = list[(int)uid].addr;
            if (!U.isSafetyOffset(classaddr))
            {
                return 0;
            }
            uint b0 = Program.ROM.u8(classaddr);
            if (b0 <= 0)
            {
                return 0;
            }

            uint icon = Program.ROM.p32(iconP);
            uint text = Program.ROM.p32(textP);

            int skillCount = 0;
            Bitmap bitmap = SkillConfigSkillSystemForm.DrawIcon((uint)b0, icon);
            U.MakeTransparent(bitmap);
            buttons[0].BackgroundImage = bitmap;
            buttons[0].Tag = b0;

            string skillCaption = SkillConfigSkillSystemForm.GetSkillText((uint)b0, text);
            tooltip.SetToolTipOverraide(buttons[skillCount], skillCaption);
            skillCount++;

            return skillCount;
        }


    }
}
