using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class DecompileResult : Form
    {
        public DecompileResult()
        {
            InitializeComponent();
            string output_c = Path.ChangeExtension(Program.ROM.Filename, ".c");
            if (File.Exists(output_c))
            {
                ReadTextFile(output_c);
            }
            else
            {
                code.AppendText("Decompilation Failed.");
            }
        }
        /// <summary>
        /// 读入文本文件并在TextBox中显示
        /// </summary>
        /// <param name="filePath">文本文件名</param>
        private void ReadTextFile(string filePath)
        {
            // 读入文本文件的所有行
            string[] lines = File.ReadAllLines(filePath);
            // 在textBox1中显示文件内容
            foreach (string line in lines)
            {
                code.AppendText(line + Environment.NewLine);
            }
        }

        private void code_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
