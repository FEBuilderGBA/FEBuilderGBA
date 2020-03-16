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
            string target = this.TargetFilenameTextBox.Text;
            if (! File.Exists(target))
            {
                return;
            }
            //EAでビルドを行う.

            //マージデータの作成を行う

            //既存のルーチンの設定の保存

            //既存のルーチンをUnInstall

            //作成したマージデータを取り込む

            //設定の上書き


        }
    }
}
