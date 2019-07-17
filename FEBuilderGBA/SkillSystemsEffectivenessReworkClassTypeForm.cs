using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SkillSystemsEffectivenessReworkClassTypeForm : Form
    {
        public SkillSystemsEffectivenessReworkClassTypeForm()
        {
            InitializeComponent();

            L_50_BIT_01.Text = R._("アーマー系");
            L_50_BIT_02.Text = R._("騎兵系");
            L_50_BIT_04.Text = R._("飛行系");
            L_50_BIT_08.Text = R._("ドラゴン系");
            L_50_BIT_10.Text = R._("モンスター系");
            L_50_BIT_20.Text = R._("ソード系");
            L_50_BIT_40.Text = R._("不明1");
            L_50_BIT_80.Text = R._("不明2");

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("", controls);

            U.AddCancelButton(this);
        }

        public static string GetText(uint id)
        {
            List<string> list = new List<string>();

            if ((id & 0x01) == 0x01)
            {
                list.Add(R._("アーマー系"));
            }
            if ((id & 0x02) == 0x02)
            {
                list.Add(R._("騎兵系"));
            }
            if ((id & 0x04) == 0x04)
            {
                list.Add(R._("飛行系"));
            }
            if ((id & 0x08) == 0x08)
            {
                list.Add(R._("ドラゴン系"));
            }
            if ((id & 0x10) == 0x10)
            {
                list.Add(R._("モンスター系"));
            }
            if ((id & 0x20) == 0x20)
            {
                list.Add(R._("ソード系"));
            }
            if ((id & 0x40) == 0x40)
            {
                list.Add(R._("不明1"));
            }
            if ((id & 0x80) == 0x80)
            {
                list.Add(R._("不明2"));
            }

            return string.Join(",", list);
        }

        public static Bitmap DrawClassTypeIcon(uint id)
        {
            if ((id & 0x01) == 0x01)
            {
                return ClassForm.DrawWaitIcon(0x09, 0, true); //アーマー系
            }
            if ((id & 0x02) == 0x02)
            {
                return ClassForm.DrawWaitIcon(0x05, 0, true); //騎兵系
            }
            if ((id & 0x04) == 0x04)
            {
                return ClassForm.DrawWaitIcon(0x48, 0, true); //飛行系
            }
            if ((id & 0x08) == 0x08)
            {
                return ClassForm.DrawWaitIcon(0x1f, 0, true); //ドラゴン系
            }
            if ((id & 0x10) == 0x10)
            {
                return ClassForm.DrawWaitIcon(0x54, 0, true); //モンスター系
            }
            if ((id & 0x20) == 0x20)
            {
                return ClassForm.DrawWaitIcon(0x13, 0, true); //ソード系
            }
            return ImageUtil.BlankDummy();
        }

        private void SkillSystemsEffectivenessReworkClassTypeForm_Load(object sender, EventArgs e)
        {
            Bitmap a = ClassForm.DrawWaitIcon(0x09 , 0 ,true); //アーマー系
            U.MakeTransparent(a);
            IMAGE_1.Image = a;

            a = ClassForm.DrawWaitIcon(0x05, 0, true); //騎兵系
            U.MakeTransparent(a);
            IMAGE_2.Image = a;

            a = ClassForm.DrawWaitIcon(0x48, 0, true); //飛行系
            U.MakeTransparent(a);
            IMAGE_4.Image = a;

            a = ClassForm.DrawWaitIcon(0x1f, 0, true); //ドラゴン系
            U.MakeTransparent(a);
            IMAGE_8.Image = a;

            a = ClassForm.DrawWaitIcon(0x54, 0, true); //モンスター系
            U.MakeTransparent(a);
            IMAGE_10.Image = a;

            a = ClassForm.DrawWaitIcon(0x13, 0, true); //ソード系
            U.MakeTransparent(a);
            IMAGE_20.Image = a;
        }

        public void JumpToClassType(uint value)
        {
            W50.Value = value;
        }
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ApplyButton.Tag = (uint)W50.Value;
        }

        public Button GetApplyButton()
        {
            return this.ApplyButton;
        }
    }
}
