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
    public partial class PackedMemorySlotForm : Form
    {
        public PackedMemorySlotForm()
        {
            InitializeComponent();

            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.MakeLinkEvent("", controls);
            U.AddCancelButton(this);

            MakeSlotCombobox(this.AComboBox, useNull: false);
            MakeSlotCombobox(this.BComboBox, useNull: true);
            MakeSlotCombobox(this.CComboBox, useNull: true);
        }
        public Button GetApplyButton()
        {
            return ApplyButton;
        }
        void MakeSlotCombobox(ComboBox cbox, bool useNull)
        {
            if (useNull)
            {
                cbox.Items.Add("0=Slot0(Always 0)");
            }
            cbox.Items.Add("1=Slot1");
            cbox.Items.Add("2=Slot2");
            cbox.Items.Add("3=Slot3");
            cbox.Items.Add("4=Slot4");
            cbox.Items.Add("5=Slot5");
            cbox.Items.Add("6=Slot6");
            cbox.Items.Add("7=Slot7");
            cbox.Items.Add("8=Slot8");
            cbox.Items.Add("9=Slot9");
            cbox.Items.Add("A=SlotA");
            cbox.Items.Add("B=SlotB(Coord)");
            cbox.Items.Add("C=SlotC(Result)");
            cbox.Items.Add("D=SlotD(Queue)");
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            uint a = U.atoh(this.AComboBox.Text);
            uint b = U.atoh(this.BComboBox.Text);
            uint c = U.atoh(this.CComboBox.Text);

            ApplyButton.Tag = a | (b << 4) | (c << 8);
        }
        string MakeOpLabel(string argtype)
        {
            if (argtype.IndexOf("SSUB") >= 0) return " - ";
            if (argtype.IndexOf("SMUL") >= 0) return " * ";
            if (argtype.IndexOf("SDIV") >= 0) return " / ";
            if (argtype.IndexOf("SMOD") >= 0) return "MOD";
            if (argtype.IndexOf("SAND") >= 0) return "AND";
            if (argtype.IndexOf("SORR") >= 0) return "ORR";
            if (argtype.IndexOf("SXOR") >= 0) return "XOR";
            if (argtype.IndexOf("SLSL") >= 0) return " <<";
            if (argtype.IndexOf("SLSR") >= 0) return " >>";
            return " + ";
        }
        public void JumpTo(string argtype, uint value)
        {
            uint a = value & 0xF;
            uint b = (value>>4) & 0xF;
            uint c = (value>>8) & 0xF;

            if (a >= 1)
            {
                a--;
            }

            U.SelectedIndexSafety(this.AComboBox, a);
            U.SelectedIndexSafety(this.BComboBox, b);
            U.SelectedIndexSafety(this.CComboBox, c);

            OPLabelEx.Text = MakeOpLabel(argtype);
            MESSAGE.Text = R._("計算式を作成してください。");
        }

        private void PackedMemorySlotForm_Load(object sender, EventArgs e)
        {

        }
    }
}
