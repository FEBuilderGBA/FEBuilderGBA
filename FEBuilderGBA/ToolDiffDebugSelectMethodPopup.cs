using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolDiffDebugSelectMethodPopup : Form
    {
        public ToolDiffDebugSelectMethodPopup()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        ToolThreeMargeForm.DiffDebugMethod Method;

        public ToolThreeMargeForm.DiffDebugMethod GetMethod()
        {
            return this.Method;
        }

        private void Method1Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Method = ToolThreeMargeForm.DiffDebugMethod.Method1;
            this.Close();
        }

        private void Method2Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Method = ToolThreeMargeForm.DiffDebugMethod.Method2;
            this.Close();
        }

        public void Init(string ng_rom,string ok_rom)
        {
            this.Explain.Text =
                R._("どの比較方法を利用しますか？\r\n\r\n正しく動いていた最後のROM(OK ROM) : {1}\r\nその次の世代の正しく動かなくなった最初のROM(NG ROM) : {1}\r\n現在のROM(CURRENT)\r\n"
                    , Path.GetFileName(ok_rom) , Path.GetFileName(ng_rom) );
        }


        private void ToolDiffDebugSelectMethodPopup_Load(object sender, EventArgs e)
        {

        }
    }
}
