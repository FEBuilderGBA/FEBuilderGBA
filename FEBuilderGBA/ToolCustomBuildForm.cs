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
    public partial class ToolCustomBuildForm : Form
    {
        public ToolCustomBuildForm()
        {
            InitializeComponent();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {

        }

        void Run()
        {
            string target_filename = this.TargetFilenameTextBox.Text;
            if (! File.Exists(target_filename))
            {
                return;
            }
            //バニラROMをコピー

            //EAでビルドを行う.

            //マージデータの作成を行う

            //既存のルーチンの設定の保存

            //既存のルーチンをUnInstall

            //作成したマージデータを取り込む

            //設定の上書き
        }

        //バニラROMをコピー
        bool CopyVanilla()
        {
            string orignal = this.OrignalROMTextArea.Text;
            if (!File.Exists(orignal))
            {
                R.ShowStopError("無改造ROM({0})が見つかりませんでした",orignal);
                return false;
            }

            string target_filename = this.TargetFilenameTextBox.Text;
            string basedir = Path.GetDirectoryName(target_filename);

            string storename = Path.Combine(basedir,"FE8_clean.gba");

            try
            {
                File.Copy(orignal, storename);
            }
            catch (Exception e)
            {
                R.ShowStopError("無改造ROM({0})を、({1})にコピーできませんでした。\r\n{2}", orignal, storename, R.ExceptionToString(e));
                return false;
            }
            return true;
        }

        //EAでビルドを行う.
        bool BuildEA()
        {
            string target_filename = this.TargetFilenameTextBox.Text;

            //EventAssemblerで対象物をコンパイル
            string output,out_symbol;
            bool r = MainFormUtil.CompilerEventAssembler(target_filename, U.NOT_FOUND,U.NOT_FOUND, out output, out out_symbol);
            if (!r)
            {
                R.ShowStopError(output);
                return false;
            }
            return true;
        }

        //マージデータの作成を行う

        //既存のルーチンの設定の保存

        //既存のルーチンをUnInstall
        bool UnInstall()
        {
            PatchForm patchF = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            string PatchName = "Skill20200223";
            string PatchName2 = "Skill20200223";
            bool r = patchF.UnInstallPatch(PatchName, PatchName2 , isAutoClose:true , isForceUninstall: true);

            return r;
        }

        private void ToolCustomBuildForm_Load(object sender, EventArgs e)
        {

        }


        //作成したマージデータを取り込む

        //設定の上書き
    }
}
