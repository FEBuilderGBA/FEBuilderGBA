using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class TextBadCharPopupForm : Form
    {
        public TextBadCharPopupForm()
        {
            InitializeComponent();
        }

        public void SetErrorMessage(string str)
        {
            this.Error_MessageLabel.Text = str;

            Color Color_Error_BackColor = OptionForm.Color_Error_BackColor();
            Color Color_Error_ForeColor = OptionForm.Color_Error_ForeColor();
            Error_MessageLabel.BackColor = Color_Error_BackColor;
            Error_MessageLabel.ForeColor = Color_Error_ForeColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static bool CheckTBLAndAntiHuffman()
        {
            if (OptionForm.textencoding() == OptionForm.textencoding_enum.Auto
                || OptionForm.textencoding() == OptionForm.textencoding_enum.Shift_JIS
                || OptionForm.textencoding() == OptionForm.textencoding_enum.LAT1
                || OptionForm.textencoding() == OptionForm.textencoding_enum.UTF8)
            {
                return true;
            }
            DialogResult dr = R.ShowNoYes("現在TBLを利用しています。\r\nTBLを利用しながらun-haffman patchを利用すると予期しないトラブルが発生することがあります。\r\n「はい」を選択すると、パッチのインストールを強行します。\r\n「いいえ」を選択すると、パッチ適応を取りやめます。(推奨)\r\n\r\nどうしますか？");
            return dr == DialogResult.Yes;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckTBLAndAntiHuffman() == false)
            {
                return;
            }

            this.Close();

            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.ApplyPatch("Anti-Huffman");
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            TextCharCodeForm f = (TextCharCodeForm)InputFormRef.JumpForm<TextCharCodeForm>();
        }

        private void TextBadCharPopupForm_Load(object sender, EventArgs e)
        {
            //メッセージ（問い合わせ）を鳴らす
            System.Media.SystemSounds.Question.Play();
        }
    }
}
