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
    public partial class PointerToolCopyToForm : Form
    {
        public PointerToolCopyToForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        uint Address;
        public void Init(uint addr)
        {
            this.Address = addr;
            this.ValueTextBox.Text = U.ToHexString(addr);

            this.HexButton.Enabled = (U.isSafetyOffset(U.toOffset(addr)));
        }


        private void CopyClipboard_Click(object sender, EventArgs e)
        {
            U.SetClipboardText(this.ValueTextBox.Text);
            this.Close();
        }

        private void CopyPointer_Click(object sender, EventArgs e)
        {
            U.SetClipboardText( U.ToHexString(U.toPointer(this.Address)));
            this.Close();
        }

        private void CopyLittleEndian_Click(object sender, EventArgs e)
        {
            uint a = U.toPointer(this.Address);
            uint r = (((a&0xFF) << 24)
                + ((a & 0xFF00) << 8)
                + ((a & 0xFF0000) >> 8)
                + ((a & 0xFF000000) >> 24));

            U.SetClipboardText(U.ToHexString(r));
            this.Close();
        }

        private void HexButton_Click(object sender, EventArgs e)
        {
            uint addr = U.toOffset(this.Address);
            if (U.isSafetyOffset(addr))
            {
                HexEditorForm hexeditor = (HexEditorForm)InputFormRef.JumpForm<HexEditorForm>(U.NOT_FOUND);
                hexeditor.JumpTo(addr);
                this.Close();
            }
            
        }

        private void PointerToolCopyToForm_Load(object sender, EventArgs e)
        {

        }

        private void CopyNoDollGBARadBreakPoint_Click(object sender, EventArgs e)
        {
            U.SetClipboardText("["+U.ToHexString(U.toPointer(this.Address))+"]?");
            this.Close();

        }

    }
}
