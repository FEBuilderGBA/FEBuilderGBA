using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gee.External.Capstone;
using Gee.External.Capstone.Arm;
using Keystone;
using Unicorn;
using Unicorn.Arm;

namespace FEBuilderGBA
{
    public partial class ASMEditForm : Form
    {
        enum  CPUMode
        {
            arm,
            thumb
        };

        public ASMEditForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ReadStartAddress_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ReadCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ReloadListButton_Click(object sender, EventArgs e)
        {
            ArmDisassembleMode disassembleMode = ArmDisassembleMode.Arm;
            if(thumb.Checked)
            {
                disassembleMode = ArmDisassembleMode.Thumb;
            }
            uint addr = (uint)ReadStartAddress.Value;
            if ((addr & 1) == 1)
            {
                disassembleMode = ArmDisassembleMode.Thumb;
                addr--;
            }
            if (addr >= GBAMemory.ROM_ADDR)
            {
                addr -= GBAMemory.ROM_ADDR;
                U.ForceUpdate(ReadStartAddress, addr);
            }
            if (addr >= (uint)Program.ROM.Data.Length)
            {
                return;
            }
            int count = (int)ReadCount.Value;
            if(count == 0)
            {
                count = 1;
                U.ForceUpdate(ReadCount, count);
            }

            assembly.Clear();
            using (CapstoneArmDisassembler disassembler = CapstoneDisassembler.CreateArmDisassembler(disassembleMode))
            {
                disassembler.DisassembleSyntax = DisassembleSyntax.Intel;
                if(syntax_att.Checked)
                {
                    disassembler.DisassembleSyntax = DisassembleSyntax.Att;
                }
                if (syntax_msam.Checked)
                {
                    disassembler.DisassembleSyntax = DisassembleSyntax.Masm;
                }
                // ...
                //
                // By enabling Skip Data Mode, we let Capstone will automatically skip over data and continue
                // disassembling at the next valid instruction it finds.
                if (skip_data.Checked)
                {
                    disassembler.EnableSkipDataMode = true;
                }   
                ArmInstruction[] instructions = disassembler.Disassemble(Program.ROM.Data.Skip((int)addr).ToArray(), GBAMemory.ROM_ADDR + addr, count);
                foreach (ArmInstruction instruction in instructions)
                {
                    if (!instruction.IsSkippedData)
                    {
                        var address = instruction.Address;
                        ArmInstructionId id = instruction.Id;
                        if (!instruction.IsDietModeEnabled)
                        {
                            // ...
                            //
                            // An instruction's mnemonic and operand text are only available when Diet Mode is disabled.
                            // An exception is thrown otherwise!
                            var mnemonic = instruction.Mnemonic;
                            var operand = instruction.Operand;
                            assembly.AppendText(string.Format("/* {0:X} */ \t {1} \t {2}", address, mnemonic, operand));
                        }
                        assembly.AppendText(System.Environment.NewLine);
                    }
                }
            }

            return;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mode mode = Mode.ARM;
            if (thumb.Checked)
            {
                mode = Mode.THUMB;
            }
            uint addr = (uint)ReadStartAddress.Value;
            if ((addr & 1) == 1)
            {
                mode = Mode.THUMB;
                addr--;
            }
            if (addr >= GBAMemory.ROM_ADDR)
            {
                addr -= GBAMemory.ROM_ADDR;
                U.ForceUpdate(ReadStartAddress, addr);
            }
            if (addr >= (uint)Program.ROM.Data.Length)
            {
                return;
            }
            using (Engine keystone = new Engine(Architecture.ARM, mode) { ThrowOnError = true })
            {
                Undo.UndoData undodata = Program.Undo.NewUndoData(this);

                EncodedData enc = keystone.Assemble(assembly.Text, GBAMemory.ROM_ADDR + addr);
                //Log.Debug(enc.Address.ToString("x"), enc.StatementCount.ToString(), BitConverter.ToString(enc.Buffer));
                Program.ROM.write_range(addr, enc.Buffer, undodata);
                Program.Undo.Push(undodata);
                InputFormRef.ShowWriteNotifyAnimation(this, 0);
                InputFormRef.WriteButtonToYellow(this.AsmButton, false);
            }
        }

        private void assembly_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            uint addr = (uint)ReadStartAddress.Value;
            if ((addr & 1) == 1)
            {
                addr--;
            }
            if (addr >= GBAMemory.ROM_ADDR)
            {
                addr -= GBAMemory.ROM_ADDR;
                U.ForceUpdate(ReadStartAddress, addr);
            }
            if (addr >= (uint)Program.ROM.Data.Length)
            {
                return;
            }
            int count = (int)ReadCount.Value;
            if (count == 0)
            {
                count = 1;
                U.ForceUpdate(ReadCount, count);
            }
            var save = new SaveFileDialog();
            save.Filter = "Assembly (*.asm)|*.asm";
            save.FileName = "Assembly_" + Path.GetFileNameWithoutExtension(Program.ROM.Filename) + "_0x" + addr.ToString("x") + "_" + count.ToString();
            if (save.ShowDialog() == DialogResult.OK && save.FileName != "")
            {
                var sw = new StreamWriter(save.FileName);
                for (var i = 0; i < assembly.Lines.Length; i++)
                {
                    sw.WriteLine(assembly.Lines.GetValue(i).ToString());
                }
                sw.Close();
            }
        }

        private void load_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "Assembly (*.asm)|*.asm";
            if (open.ShowDialog() == DialogResult.OK && open.FileName != "")
            {
                var st = new StreamReader(open.FileName, System.Text.Encoding.UTF8);
                assembly.Clear();
                assembly.Text = st.ReadToEnd();
                assembly.Update();
                st.Close();
            }
        }

        private void emulate_Click(object sender, EventArgs e)
        {
            ArmMode arm_mode = ArmMode.Arm;
            if (thumb.Checked)
            {
                arm_mode = ArmMode.Thumb;
            }
            uint addr = (uint)ReadStartAddress.Value;
            if ((addr & 1) == 1)
            {
                arm_mode = ArmMode.Thumb;
                addr--;
            }
            if (addr >= GBAMemory.ROM_ADDR)
            {
                addr -= GBAMemory.ROM_ADDR;
                U.ForceUpdate(ReadStartAddress, addr);
            }
            if (addr >= (uint)Program.ROM.Data.Length)
            {
                return;
            }
            addr += GBAMemory.ROM_ADDR;
            EncodedData enc;
            // 即时汇编
            if (arm_mode == ArmMode.Thumb)
            {
                using (Engine keystone = new Engine(Architecture.ARM, Mode.THUMB) { ThrowOnError = true })
                {
                    enc = keystone.Assemble(assembly.Text, addr);
                }
            }
            else
            {
                using (Engine keystone = new Engine(Architecture.ARM, Mode.ARM) { ThrowOnError = true })
                {
                    enc = keystone.Assemble(assembly.Text, addr);
                }
            }
            // 模拟运行
            using (var emulator = new ArmEmulator(arm_mode))
            {
                // load register value
                emulator.Registers.R0 = (long)r0.Value;
                emulator.Registers.R1 = (long)r1.Value;
                emulator.Registers.R2 = (long)r2.Value;
                emulator.Registers.R3 = (long)r3.Value;
                emulator.Registers.R4 = (long)r4.Value;
                emulator.Registers.R5 = (long)r5.Value;
                emulator.Registers.R6 = (long)r6.Value;
                emulator.Registers.R7 = (long)r7.Value;
                emulator.Registers.R8 = (long)r8.Value;
                emulator.Registers.R9 = (long)r9.Value;
                emulator.Registers.R10 = (long)r10.Value;
                emulator.Registers.R11 = (long)r11.Value;
                emulator.Registers.R12 = (long)r12.Value;
                emulator.Registers.SP = (long)r13.Value;
                emulator.Registers.LR = (long)r14.Value;
                //emulator.Registers.PC = (long)r15.Value;

                // load ROM
                if(whole_rom.Checked)
                {
                    emulator.Memory.Map(GBAMemory.ROM_ADDR, Program.ROM.Data.Length, MemoryPermissions.Execute | MemoryPermissions.Read);
                    emulator.Memory.Write(GBAMemory.ROM_ADDR, Program.ROM.Data, Program.ROM.Data.Length);
                }
                else
                {
                    emulator.Memory.Map(addr, enc.Buffer.Length, MemoryPermissions.All);
                }

                // load assembly
                emulator.Memory.Write(addr, enc.Buffer, enc.Buffer.Length);

                // 加载GBA地址布局
                if (memory.Checked)
                {
                    // laod WRAM
                    emulator.Memory.Map(GBAMemory.EWRAM_ADDR, GBAMemory.EWRAM_MAX_LENGTH, MemoryPermissions.All);
                    emulator.Memory.Map(GBAMemory.IWRAM_ADDR, GBAMemory.IWRAM_MAX_LENGTH, MemoryPermissions.All);
                    if (!String.IsNullOrEmpty(sgm.Text))
                    {
                        if (File.Exists(sgm.Text))
                        {
                            //TODO load data in .sgm to EWRAM and IWRAM

                        }
                    }

                    // load SRAM
                    emulator.Memory.Map(GBAMemory.SRAM_ADDR, GBAMemory.SRAM_MAX_LENGTH, MemoryPermissions.Write | MemoryPermissions.Read);
                    if (!String.IsNullOrEmpty(sav.Text))
                    {
                        if (File.Exists(sav.Text))
                        {
                            FileInfo fi = new FileInfo(sav.Text);
                            byte[] buff = new byte[Math.Min(GBAMemory.SRAM_MAX_LENGTH, fi.Length)];

                            FileStream fs = fi.OpenRead();
                            fs.Read(buff, 0, Math.Min(buff.Length, Convert.ToInt32(fs.Length)));
                            fs.Close();

                            emulator.Memory.Write(GBAMemory.SRAM_ADDR, buff, buff.Length);
                        }
                    }

                    // load BIOS
                    emulator.Memory.Map(GBAMemory.BIOS_ADDR, GBAMemory.BIOS_MAX_LENGTH, MemoryPermissions.Execute);
                    if (!String.IsNullOrEmpty(bios.Text))
                    {
                        if (File.Exists(bios.Text))
                        {
                            FileInfo fi = new FileInfo(bios.Text);
                            byte[] buff = new byte[Math.Min(GBAMemory.BIOS_MAX_LENGTH, fi.Length)];

                            FileStream fs = fi.OpenRead();
                            fs.Read(buff, 0, Math.Min(buff.Length, Convert.ToInt32(fs.Length)));
                            fs.Close();

                            emulator.Memory.Write(GBAMemory.BIOS_ADDR, buff, buff.Length);
                        }
                        else
                        {
                            bios.Clear();
                        }
                    }

                    // load IOREG
                    // PageSize = 1KB, 不翻倍map内存的时候会报错
                    emulator.Memory.Map(GBAMemory.IOREG_ADDR, GBAMemory.IOREG_MAX_LENGTH * 2, MemoryPermissions.Read | MemoryPermissions.Write);
                    if (!String.IsNullOrEmpty(ioreg.Text))
                    {
                        if (File.Exists(ioreg.Text))
                        {
                            FileInfo fi = new FileInfo(ioreg.Text);
                            byte[] buff = new byte[Math.Min(GBAMemory.IOREG_MAX_LENGTH, fi.Length)];

                            FileStream fs = fi.OpenRead();
                            fs.Read(buff, 0, Math.Min(buff.Length, Convert.ToInt32(fs.Length)));
                            fs.Close();

                            emulator.Memory.Write(GBAMemory.IOREG_ADDR, buff, buff.Length);
                        }
                    }

                    // load PALRAM
                    // PageSize = 1KB, 不翻倍map内存的时候会报错
                    emulator.Memory.Map(GBAMemory.PALRAM_ADDR, GBAMemory.PALRAM_MAX_LENGTH * 2, MemoryPermissions.Read | MemoryPermissions.Write);
                    if (!String.IsNullOrEmpty(palram.Text))
                    {
                        if (File.Exists(palram.Text))
                        {
                            FileInfo fi = new FileInfo(palram.Text);
                            byte[] buff = new byte[Math.Min(GBAMemory.PALRAM_MAX_LENGTH, fi.Length)];

                            FileStream fs = fi.OpenRead();
                            fs.Read(buff, 0, Math.Min(buff.Length, Convert.ToInt32(fs.Length)));
                            fs.Close();

                            emulator.Memory.Write(GBAMemory.PALRAM_ADDR, buff, buff.Length);
                        }
                    }

                    // load VRAM
                    emulator.Memory.Map(GBAMemory.VRAM_ADDR, GBAMemory.VRAM_MAX_LENGTH, MemoryPermissions.Read | MemoryPermissions.Write);
                    if (!String.IsNullOrEmpty(vram.Text))
                    {
                        if (File.Exists(vram.Text))
                        {
                            FileInfo fi = new FileInfo(vram.Text);
                            byte[] buff = new byte[Math.Min(GBAMemory.VRAM_MAX_LENGTH, fi.Length)];

                            FileStream fs = fi.OpenRead();
                            fs.Read(buff, 0, Math.Min(buff.Length, Convert.ToInt32(fs.Length)));
                            fs.Close();

                            emulator.Memory.Write(GBAMemory.VRAM_ADDR, buff, buff.Length);
                        }
                    }

                    // load OAM
                    // PageSize = 1KB, 不翻倍map内存的时候会报错
                    emulator.Memory.Map(GBAMemory.OAM_ADDR, GBAMemory.OAM_MAX_LENGTH * 2, MemoryPermissions.Read | MemoryPermissions.Write);
                    if (!String.IsNullOrEmpty(oam.Text))
                    {
                        if (File.Exists(oam.Text))
                        {
                            FileInfo fi = new FileInfo(oam.Text);
                            byte[] buff = new byte[Math.Min(GBAMemory.OAM_MAX_LENGTH, fi.Length)];

                            FileStream fs = fi.OpenRead();
                            fs.Read(buff, 0, Math.Min(buff.Length, Convert.ToInt32(fs.Length)));
                            fs.Close();

                            emulator.Memory.Write(GBAMemory.OAM_ADDR, buff, buff.Length);
                        }
                    }

                    // load address value list
                    if (!String.IsNullOrEmpty(address_value.Text))
                    {
                        foreach (string line in address_value.Lines)
                        {
                            if (String.IsNullOrWhiteSpace(line))
                            {
                                string[] record = line.Split(':');
                                if (record.Length == 2)
                                {
                                    ulong address = ulong.Parse(record[0], System.Globalization.NumberStyles.HexNumber);
                                    string[] byte_list = (record[1]).Split();
                                    emulator.Memory.Write(address, Array.ConvertAll<string, byte>(byte_list, s => byte.Parse(s, System.Globalization.NumberStyles.HexNumber)), byte_list.Length);
                                }
                            }
                        }
                    }
                }

                // emulate
                emulator.Start(addr, addr + (ulong)enc.Buffer.Length, new TimeSpan(0, 0, Convert.ToInt32(timeout.Value)), Convert.ToInt32(code_num_limit.Value));

                // show registers
                r0.Value = emulator.Registers.R0;
                r0.Update();
                r1.Value = emulator.Registers.R1;
                r1.Update();
                r2.Value = emulator.Registers.R2;
                r2.Update();
                r3.Value = emulator.Registers.R3;
                r3.Update();
                r4.Value = emulator.Registers.R4;
                r4.Update();
                r5.Value = emulator.Registers.R5;
                r5.Update();
                r6.Value = emulator.Registers.R6;
                r6.Update();
                r7.Value = emulator.Registers.R7;
                r7.Update();
                r8.Value = emulator.Registers.R8;
                r8.Update();
                r9.Value = emulator.Registers.R9;
                r9.Update();
                r10.Value = emulator.Registers.R10;
                r10.Update();
                r11.Value = emulator.Registers.R11;
                r11.Update();
                r12.Value = emulator.Registers.R12;
                r12.Update();
                r13.Value = emulator.Registers.SP;
                r13.Update();
                r14.Value = emulator.Registers.LR;
                r14.Update();
                r15.Value = emulator.Registers.PC;
                r15.Update();

                if(memory.Checked)
                {
                    // dump binaries

                    if (!String.IsNullOrEmpty(sav.Text))
                    {
                        byte[] buff = new byte[GBAMemory.SRAM_MAX_LENGTH];
                        emulator.Memory.Read(GBAMemory.SRAM_ADDR, buff, buff.Length);
                        FileStream fs = new FileStream(sav.Text, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }

                    if (!String.IsNullOrEmpty(sgm.Text))
                    {
                        //TODO dump data in EWRAM and IWRAM to .sgm

                    }

                    if (!String.IsNullOrEmpty(ioreg.Text))
                    {
                        byte[] buff = new byte[GBAMemory.IOREG_MAX_LENGTH];
                        emulator.Memory.Read(GBAMemory.IOREG_ADDR, buff, buff.Length);
                        FileStream fs = new FileStream(ioreg.Text, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }

                    if (!String.IsNullOrEmpty(palram.Text))
                    {
                        byte[] buff = new byte[GBAMemory.PALRAM_MAX_LENGTH];
                        emulator.Memory.Read(GBAMemory.PALRAM_ADDR, buff, buff.Length);
                        FileStream fs = new FileStream(palram.Text, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }

                    if (!String.IsNullOrEmpty(vram.Text))
                    {
                        byte[] buff = new byte[GBAMemory.VRAM_MAX_LENGTH];
                        emulator.Memory.Read(GBAMemory.VRAM_ADDR, buff, buff.Length);
                        FileStream fs = new FileStream(vram.Text, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }

                    if (!String.IsNullOrEmpty(oam.Text))
                    {
                        byte[] buff = new byte[GBAMemory.OAM_MAX_LENGTH];
                        emulator.Memory.Read(GBAMemory.OAM_ADDR, buff, buff.Length);
                        FileStream fs = new FileStream(oam.Text, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(buff, 0, buff.Length);
                        bw.Close();
                        fs.Close();
                    }
                }
            }
        }

        private void open_sgm_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RealTime Save(*.sgm)|*.sgm";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sgm.Text = ofd.FileName;
                sgm.Update();
            }
        }

        private void open_save_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Battery Save(*.sav)|*.sav";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sav.Text = ofd.FileName;
                sav.Update();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "GBA BIOS(*.bin;*.gba)|*.bin;*.gba";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                bios.Text = ofd.FileName;
                bios.Update();
            }
        }

        private void open_ioreg_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "IO Registers(*.bin;*.dmp)|*.bin;*.dmp";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ioreg.Text = ofd.FileName;
                ioreg.Update();
            }
        }

        private void open_palram_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Palette RAM(*.bin;*.dmp)|*.bin;*.dmp";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                palram.Text = ofd.FileName;
                palram.Update();
            }
        }

        private void open_vram_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Video RAM(*.bin;*.dmp)|*.bin;*.dmp";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                vram.Text = ofd.FileName;
                vram.Update();
            }
        }

        private void open_oam_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "OBJ Attributes(*.bin;*.dmp)|*.bin;*.dmp";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                oam.Text = ofd.FileName;
                oam.Update();
            }
        }

        private void save_address_value_Click(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "Text (*.txt)|*.txt";
            if (save.ShowDialog() == DialogResult.OK && save.FileName != "")
            {
                var sw = new StreamWriter(save.FileName);
                sw.WriteLine(address_value.Text);
                sw.Close();
            }
        }

        private void load_address_value_Click(object sender, EventArgs e)
        {
            var open = new OpenFileDialog();
            open.Filter = "Text (*.txt)|*.txt";
            if (open.ShowDialog() == DialogResult.OK && open.FileName != "")
            {
                var st = new StreamReader(open.FileName, Encoding.UTF8);
                address_value.Clear();
                address_value.Text = st.ReadToEnd();
                address_value.Update();
                st.Close();
            }
        }

        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }
    }
}
