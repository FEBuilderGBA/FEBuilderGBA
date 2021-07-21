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
    public partial class RAMRewriteToolForm : Form
    {
        public RAMRewriteToolForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        uint TypeOf;
        uint Address;
        public void Init(uint addr , uint typeOf, bool isHex)
        {
            this.Address = addr;
            this.TypeOf = typeOf;
            this.ValueTextBox.Text = U.ToHexString(addr);

            if (typeOf == 1)
            {
                this.ReWriteValue.Value = Program.RAM.u8(addr);
                this.ReWriteValue.Maximum = 255;
            }
            else if (typeOf == 2)
            {
                this.ReWriteValue.Value = Program.RAM.u16(addr);
                this.ReWriteValue.Maximum = 65535;
            }
            else if (typeOf == 3)
            {
                this.ReWriteValue.Value = Program.RAM.u24(addr);
                this.ReWriteValue.Maximum = 16777215;
            }
            else
            {
                this.ReWriteValue.Value = Program.RAM.u32(addr);
                this.ReWriteValue.Maximum = 4294967295;
            }

            this.ReWriteValue.Hexadecimal = isHex;
            if (isHex)
            {//16進数
                this.ReWriteValue.BackColor = OptionForm.Color_Input_BackColor();
                this.ReWriteValue.ForeColor = OptionForm.Color_Input_ForeColor();
            }
            else
            {//10進数
                this.ReWriteValue.BackColor = OptionForm.Color_InputDecimal_BackColor();
                this.ReWriteValue.ForeColor = OptionForm.Color_InputDecimal_ForeColor();
            }
            this.ActiveControl = ReWriteValue;
        }

        private void CopyPointer_Click(object sender, EventArgs e)
        {
            U.SetClipboardText(U.ToHexString(U.toPointer(this.Address)));
            this.Close();
        }

        private void CopyClipboard_Click(object sender, EventArgs e)
        {
            U.SetClipboardText(this.ValueTextBox.Text);
            this.Close();
        }

        private void CopyLittleEndian_Click(object sender, EventArgs e)
        {
            uint a = U.toPointer(this.Address);
            uint r = (((a & 0xFF) << 24)
                + ((a & 0xFF00) << 8)
                + ((a & 0xFF0000) >> 8)
                + ((a & 0xFF000000) >> 24));

            U.SetClipboardText(U.ToHexString(r));
            this.Close();
        }

        private void CopyNoDollGBARadBreakPoint_Click(object sender, EventArgs e)
        {
            U.SetClipboardText("[" + U.ToHexString(U.toPointer(this.Address)) + "]?");
            this.Close();
        }

        private void ReWriteButton_Click(object sender, EventArgs e)
        {
            uint value = (uint)this.ReWriteValue.Value;
            if (this.TypeOf == 1)
            {
                Program.RAM.write_u8(this.Address, value);
            }
            else if (this.TypeOf == 2)
            {
                Program.RAM.write_u16(this.Address, value);
            }
            else if (this.TypeOf == 3)
            {
                Program.RAM.write_u24(this.Address, value);
            }
            else
            {
                Program.RAM.write_u32(this.Address, value);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

        private void RAMRewriteToolForm_Load(object sender, EventArgs e)
        {

        }

        private void ReWriteValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ReWriteButton.PerformClick();
            }
        }

    }
}
