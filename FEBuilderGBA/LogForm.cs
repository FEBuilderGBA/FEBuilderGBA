using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }


        private void LogForm_Load(object sender, EventArgs e)
        {
            Log.SyncLog();
            LogReload(null, null);
            Log.UpdateEvent += LogReload;
        }
        private void LogReload(object sender, EventArgs e)
        {
            Log.LogToListBox(LogList);
        }

        private void LogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.UpdateEvent -= LogReload;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);

            Log.ToFile(save.FileName);
        }

        private void ClipbordButton_Click(object sender, EventArgs e)
        {
            U.SetClipboardText(Log.LogToString());
        }

        private void openLogDirButton_Click(object sender, EventArgs e)
        {
            Log.OpenLogDir();
        }
    }
}
