using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ClassOPDemoForm : Form
    {
        public ClassOPDemoForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.N1_InputFormRef = N1_Init(this);
            this.N2_InputFormRef = N2_Init(this);
        }
        InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.p32(Program.ROM.RomInfo.op_class_demo_pointer)
                , 28
                , (int i, uint addr) =>
                {
                    return ROM.isPointer(Program.ROM.u32(addr))
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(addr+14);
                    return U.ToHexString(i) + " " + ClassForm.GetClassName(id);
                }
                );
        }
        InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {
            return new InputFormRef(self
                , "N1_"
                , 0
                , 1
                , (int i, uint addr) =>
                {
                    return (Program.ROM.u8(addr)!=0xFF)
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint id = Program.ROM.u8(addr);
                    return U.ToHexString(id);
                }
                );
        }
        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {
            return new InputFormRef(self
                , "N2_"
                , 0
                , 6
                , (int i, uint addr) =>
                {
                    return i < 1; //1つだけ
                        ;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i);
                }
                );
        }

        private void ClassOPDemoForm_Load(object sender, EventArgs e)
        {

        }

        private void P8_ValueChanged(object sender, EventArgs e)
        {
            N1_InputFormRef.ReInit((uint)this.P8.Value);
        }

        private void P24_ValueChanged(object sender, EventArgs e)
        {
            N2_InputFormRef.ReInit((uint)this.P24.Value);
        }

    }
}
