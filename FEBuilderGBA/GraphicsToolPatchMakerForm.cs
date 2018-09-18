using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class GraphicsToolPatchMakerForm : Form
    {
        public GraphicsToolPatchMakerForm()
        {
            InitializeComponent();
        }

        private void GraphicsToolPatchMakerForm_Load(object sender, EventArgs e)
        {

        }
        public void Init(string text)
        {
            PatchText.Text = text;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

            string patchFilename = "PATCH_(NAME).txt";

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, patchFilename);

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);

            string name = U.substr(Path.GetFileNameWithoutExtension(save.FileName), "PATCH_".Length);
            //パッチ名が未定義の場合、パッチファイル名から自動的に取得する.
            string patchText = PatchText.Text.Replace("<<PATCH NAME>>", name);

            File.WriteAllText(save.FileName, patchText);

            U.SelectFileByExplorer(save.FileName);

            this.Close();
        }
    }
}
