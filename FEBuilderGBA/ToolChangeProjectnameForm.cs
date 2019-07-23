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
    public partial class ToolChangeProjectnameForm : Form
    {
        public ToolChangeProjectnameForm()
        {
            InitializeComponent();
            this.CurrentName.Text = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
            this.NewName.Text = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
        }

        private void ToolChangeProjectnameForm_Load(object sender, EventArgs e)
        {
            if (Program.ROM.Modified)
            {
                R.ShowStopError("変更したデータが保存されていません。\r\n名前を変更する前に、データを保存してください。");
                this.Close();
                return;
            }
            if (Program.ROM.IsVirtualROM)
            {
                R.ShowStopError("仮想ROMの名前を変更することはできません。");
                this.Close();
                return;
            }

            this.NewName.Focus();
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            if (Program.ROM.Modified)
            {
                R.ShowStopError("変更したデータが保存されていません。\r\n名前を変更する前に、データを保存してください。");
                return;
            }
            if (U.IsBadFilename(this.NewName.Text))
            {
                R.ShowStopError("ファイル名として利用できない文字が含まれています");
                return;
            }
            if (this.CurrentName.Text == this.NewName.Text)
            {//名前が同じなので何もしない
                return;
            }

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait())
            {
                try
                {
                    ChangeName(this.CurrentName.Text, this.NewName.Text);
                }
                catch (IOException ee)
                {
                    R.ShowStopError(R.ExceptionToString(ee));
                }
            }
        }

        void ChangeName(string oldName,string newName)
        {
            string dir = Path.GetDirectoryName(Program.ROM.Filename);
            //過去のバックアップを取得
            string[] files = U.Directory_GetFiles_Safe(dir, "*", SearchOption.TopDirectoryOnly);
            foreach (string f in files)
            {
                string name = Path.GetFileNameWithoutExtension(f);
                if (name.IndexOf(oldName) != 0)
                {//prefixを保持していない.
                    continue;
                }
                string ext = Path.GetExtension(f);

                string newFilename = newName + name.Substring(oldName.Length);

                string oldPath = f;
                string newPath = Path.Combine(dir, newFilename + ext);
                if (File.Exists(newPath))
                {//既にある場合消す.
                    File.Delete(newPath);
                }
                File.Move(oldPath, newPath);
            }

            {
                string f = Program.ROM.Filename;
                string ext = Path.GetExtension(f);
                string newROMPath = Path.Combine(dir, newName + ext);

                ChangeEtcDir(newROMPath);

                //メインフォームを開きなおさないといけない場合
                MainFormUtil.ReOpenMainForm();

                //保存したROMを開く.
                bool r = Program.LoadROM(newROMPath, "");
                if (!r)
                {//何かの理由で開けなかった場合、ファイルを消してはいけない.
                    return;
                }
            }
        }
        void ChangeEtcDir(string newROMFilename)
        {
            string currentDir = Path.GetDirectoryName(U.ConfigEtcFilename("flag"));
            string newDir = Path.GetDirectoryName(U.ConfigEtcFilename("flag", newROMFilename));

            if (! Directory.Exists(currentDir))
            {//現在のプロジェクトのetcディレクトリが存在しない
                return;
            }

            if (Directory.Exists(newDir))
            {//既にある場合消す.
                Directory.Delete(newDir);
            }
            Directory.Move(currentDir, newDir);
        }

    }
}
