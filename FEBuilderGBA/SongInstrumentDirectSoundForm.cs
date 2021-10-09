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
    public partial class SongInstrumentDirectSoundForm : Form
    {
        public SongInstrumentDirectSoundForm()
        {
            InitializeComponent();
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, this.D0, this.L_0_COMBO, 0, "COMBO", new string[] { "" });
            InputFormRef.makeLinkEventHandler("", controls, this.D4, this.L_4_HZ1024, 0, "HZ1024", new string[] { "" });
        }

        private void SongInstrumentDirectSoundForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.RegistNotifyNumlicUpdate(this.AllWriteButton, controls);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }
        public void JumpToAddr(uint addr)
        {
            addr = U.toOffset(addr);
            JumpToAddrLow(addr);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }
        void JumpToAddrLow(uint addr)
        {
            this.ReadStartAddress.Value = addr;
            this.D0.Value = Program.ROM.u32(addr + 0);
            this.D4.Value = Program.ROM.u32(addr + 4);
            this.D8.Value = Program.ROM.u32(addr + 8);
            this.D12.Value = Program.ROM.u32(addr + 12);
            this.D8.Maximum = this.D12.Value;

            if (SongUtil.IsDirectSoundWaveCompressedDPCM(addr))
            {
                J_12.Text = R._("圧縮前のサイズ");
                X_L_COMPRESSED_SIZE.Show();
                X_COMPRESSED_SIZE.Show();
                X_COMPRESSED_SIZE.Value = SongUtil.GetDirectSoundWaveDataLength(addr);
                Infomation.Text = R._("これより下のアドレスには、圧縮されたDPCMデータが、LengthByteだけ格納されています。");
            }
            else
            {
                J_12.Text = R._("LengthByte");
                X_L_COMPRESSED_SIZE.Hide();
                X_COMPRESSED_SIZE.Hide();
                X_COMPRESSED_SIZE.Value = 0;
                Infomation.Text = R._("これより下のアドレスには、waveデータが、LengthByteだけ格納されています。");
            }
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            uint addr = (uint)this.ReadStartAddress.Value;
            if (!U.isSafetyOffset(addr + 16))
            {
                R.ShowStopError("領域が確保されていません。");
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            Program.ROM.write_u32(addr + 0, (uint)this.D0.Value, undodata);
            Program.ROM.write_u32(addr + 4, (uint)this.D4.Value, undodata);
            Program.ROM.write_u32(addr + 8, (uint)this.D8.Value, undodata);
            Program.ROM.write_u32(addr + 12, (uint)this.D12.Value, undodata);

            Program.Undo.Push(undodata);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }
    }
}
