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
    public partial class DumpStructSelectToTextDialogForm : Form
    {
        public DumpStructSelectToTextDialogForm()
        {
            InitializeComponent();
        }

        private void GraphicsToolPatchMakerForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
        }

        Dictionary<string, string> AddFiles;
        public void Init(string filename, string text, Dictionary<string, string> addFiles)
        {
            TitleLabel.Text = filename;
            PatchText.Text = text;
            AddFiles = addFiles;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string GetFilter()
        {
            string ext = U.GetFilenameExt(TitleLabel.Text);
            if (ext == ".CSV")
            {
                return R._("CSV|*.csv|All files|*");
            }
            else if (ext == ".TSV")
            {
                return R._("TSV|*.tsv|TEXT|*.txt|All files|*");
            }
            else if (ext == ".NMM")
            {
                return R._("NMM|*.nmm|All files|*");
            }
            else if (ext == ".TXT")
            {
                return R._("TEXT|*.txt|All files|*");
            }
            else if (ext == ".H")
            {
                return R._("Header|*.h|TEXT|*.txt|All files|*");
            }
            else if (ext == ".EVENT")
            {
                return R._("EVENT|*.event|TEXT|*.txt|All files|*");
            }
            return R._("All files|*");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = GetFilter();

            string patchFilename = TitleLabel.Text;

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
//            save.Filter = filter;
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

            string text = PatchText.Text;
            U.WriteAllText(save.FileName, text);

            //追加ファイルがあれば出力する.
            string dir = Path.GetDirectoryName(save.FileName);
            foreach (var pair in this.AddFiles)
            {
                string addfileFullPath = Path.Combine(dir,pair.Key);
                U.WriteAllText(addfileFullPath, pair.Value);
            }

            U.SelectFileByExplorer(save.FileName);
            this.Close();
        }
    }
}
