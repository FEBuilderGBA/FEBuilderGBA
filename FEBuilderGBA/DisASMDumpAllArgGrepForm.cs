using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class DisASMDumpAllArgGrepForm : Form
    {
        public DisASMDumpAllArgGrepForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);
            SRCFileTextBox.AllowDropFilename();

            this.SearhRegister.SelectedIndex = 0;
        }

        private void GrepButton_Click(object sender, EventArgs e)
        {
            string filename = SRCFileTextBox.Text;
            if (!File.Exists(filename))
            {
                return;
            }
            string searchFunction = this.TargtFunctionTextBox.Text;
            uint addr = U.atoh(searchFunction);
            if (addr != 0)
            {
                searchFunction = U.To0xHexString(U.toPointer(addr));
            }

            string searchReg = " " + this.SearhRegister.Text ;

            string ret = Grep(filename, searchFunction ,searchReg, (int)this.AllowRowsNumbe.Value);

            DumpStructSelectToTextDialogForm f = (DumpStructSelectToTextDialogForm)InputFormRef.JumpFormLow<DumpStructSelectToTextDialogForm>();
            f.Init("ArgGrep_" + searchFunction + " " + this.SearhRegister.Text + ".TXT"
                , ret, new Dictionary<string, string>());
            f.ShowDialog();
        }

        string Grep(string filename, string searchFunction, string searchReg, int allowNumber)
        {
            bool resultOptionHideFunctionCall = ResultOptionHideFunctionCallCheckBox.Checked;
            bool resultOptionHideUnknowArg = ResultOptionHideUnknowArgCheckBox.Checked;

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                wait.DoEvents(R._("データを準備中..."));

                StringBuilder ret = new StringBuilder();
                int regLine = 0;
                int nextDoEvents = 0;

                //面倒だから全部読む.
                string[] lines = File.ReadAllLines(filename);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > nextDoEvents)
                    {//毎回更新するのは無駄なのである程度の間隔で更新して以降
                        wait.DoEvents(String.Format("{0}/{1}", i, lines.Length));
                        nextDoEvents = i + 0xfff;
                    }
                    string line = lines[i];

                    if (regLine <= 0)
                    {
                        if (IsSearchRegister(line, searchReg))
                        {
                            if (resultOptionHideUnknowArg)
                            {
                                if (line.IndexOf('(') > 0)
                                {
                                    continue;
                                }
                            }
                            regLine = i;
                        }
                    }
                    else
                    {
                        if (line.IndexOf(searchFunction) >= 0)
                        {
                            int limit = i;
                            if (resultOptionHideFunctionCall)
                            {
                                limit--;
                            }
                            for (int n = regLine; n <= limit; n++)
                            {
                                ret.AppendLine(lines[n].Trim());
                            }
                            //区切りとして改行をいれる
                            ret.AppendLine();

                            regLine = 0;
                        }
                        else if (IsSearchRegister(line, searchReg))
                        {//もっと近場に 探しているレジスタ参照があった
                            if (resultOptionHideUnknowArg)
                            {
                                if (line.IndexOf('(') > 0)
                                {
                                    regLine = 0;
                                    continue;
                                }
                            }
                            regLine = i;
                        }
                        else if (i - regLine >= allowNumber)
                        {//範囲を超えたので没にする
                            i = regLine; //regのあった次の行に戻る.
                            regLine = 0;
                        }
                    }
                }
                return ret.ToString();
            }
        }
        bool IsSearchRegister(string line,string searchReg)
        {
            if (line.IndexOf(" mov") < 0)
            {
                if (line.IndexOf(" ldr") < 0)
                {
                    return false;
                }
            }

            return line.IndexOf(searchReg) >= 0;
        }

        private void SRCFileSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("Grepする逆アセンブラソースを選択してください");
            string filter = R._("Text|*.txt|ASM|*.asm|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return ;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            SRCFileTextBox.Text = open.FileNames[0];
        }

        private void SRCFileTextBox_DoubleClick(object sender, EventArgs e)
        {
            this.SRCFileSelectButton.PerformClick();
        }

        private void DisASMDumpAllArgGrepForm_Load(object sender, EventArgs e)
        {

        }

    }
}
