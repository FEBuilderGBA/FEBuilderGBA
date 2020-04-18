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
    public partial class SelectAllocAreaForm : Form
    {
        public SelectAllocAreaForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
        }

        void Init(uint newArea,uint newLength,uint oldArea, uint oldLength)
        {
            INFO.Text = 
            R._("「データ({0}) 元の長さ({1} {2})」は、長さ({3} {4})のデータとなり、既存のスペースに入りきりません。\r\nこのデータを割り当てる領域を指定してください。\r\n\r\n指定しない場合は、\r\nディフォルトで割り当てられる({5})から変更せずに、割り当てボタンを押してください。"
                , U.To0xHexString(oldArea), oldLength, U.To0xHexString(oldLength)
                , newLength, U.To0xHexString(newLength), U.To0xHexString(newArea));
            Area.Value = newArea;
        }

        private void SelectAllocAreaForm_Load(object sender, EventArgs e)
        {
            this.Area.Focus();
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public static uint UseSelectAllocAreaForm(uint newArea, uint newLength, uint oldArea, uint oldLength)
        {
            if (OptionForm.rom_extends() == OptionForm.rom_extends_enum.YES_But_Manual)
            {
            }
            return newArea;
//            uint flag = U.atoi(Program.Config.at("RunTestMessage"));
//            if (flag == 2)
//            {
//                menu.Items.Remove(targetMenuItem);
//            }
        }
    }
}
