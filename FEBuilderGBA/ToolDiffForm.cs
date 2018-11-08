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
    public partial class ToolDiffForm : Form
    {
        public ToolDiffForm()
        {
            InitializeComponent();
            OtherFilename.AllowDropFilename();
            AFilename.AllowDropFilename();
            BFilename.AllowDropFilename();
        }

        string OpenFile()
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "";
            }
            if (!U.CanReadFileRetry(open))
            {
                return "";
            }

            Program.LastSelectedFilename.Save(this, "", open);
            return open.FileNames[0];
        }


        private void OtherSelectButton_Click(object sender, EventArgs e)
        {
            string filename = OpenFile();
            if (filename.Length > 0)
            {
                OtherFilename.Text = filename;
            }
        }

        private void OtherFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OtherSelectButton.PerformClick();
        }

        private void MakeBinPatchButton_Click(object sender, EventArgs e)
        {
            if (this.OtherFilename.Text.Length <= 0)
            {
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, "PATCH_(NAME)");

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

            List<string> lines = new List<string>();
            lines.Add("NAME=" + name);
            lines.Add("TYPE=BIN");
            lines.Add("");

            bool PATCHED_IF_NotYet = true;
            uint PATCHED_IF_MinSize = (uint)RecoverMissMatchNumericUpDown.Value;

            string bin_patchfilename = save.FileName;

            int RecoverMissMatch = (int)RecoverMissMatchNumericUpDown.Value;
            int checkpoint = -1;

            byte[] other = File.ReadAllBytes(OtherFilename.Text);

            int length = Math.Max(Program.ROM.Data.Length,other.Length);
            for (int i = 0; i < length; i++)
            {
                if ( U.at(Program.ROM.Data,i) == U.at(other,i))
                {
                    continue;
                }

                checkpoint = i;

                i++;
                int missCount = 0;
                for (; i < length; i++)
                {
                    if ( U.at(Program.ROM.Data,i) != U.at(other,i) )
                    {
                        missCount = 0;
                        continue;
                    }

                    if (missCount >= RecoverMissMatch)
                    {
                        i -= missCount;
                        break;
                    }

                    missCount++;
                }

                //checkpoint ～ i の間を相違点として記録.
                checkpoint = (checkpoint / 4) * 4;
                i = U.Padding4(i);

                string split_filename = U.ToHexString8(checkpoint) + ".bin";
                string split_filename_fullpath =
                    Path.Combine(Path.GetDirectoryName(bin_patchfilename), split_filename);

                byte[] diff = U.subrange(other, (uint)checkpoint, (uint)i);
                File.WriteAllBytes(split_filename_fullpath, diff);

                if (PATCHED_IF_NotYet)
                {
                    if (diff.Length > PATCHED_IF_MinSize)
                    {
                        lines.Add("PATCHED_IF:" + U.To0xHexString((uint)checkpoint) + "=" + U.DumpByte(diff) );

                        PATCHED_IF_NotYet = false;
                    }
                }

                lines.Add("BINF:" + U.To0xHexString((uint)checkpoint) + "=" + split_filename);
            }

            File.WriteAllLines(bin_patchfilename, lines);

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(bin_patchfilename);
        }

        private void AFileSelectButton_Click(object sender, EventArgs e)
        {
            string filename = OpenFile();
            if (filename.Length > 0)
            {
                AFilename.Text = filename;
            }
        }

        private void BFileSelectButton_Click(object sender, EventArgs e)
        {
            string filename = OpenFile();
            if (filename.Length > 0)
            {
                BFilename.Text = filename;
            }
        }

        private void AFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AFileSelectButton.PerformClick();
        }

        private void BFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BFileSelectButton.PerformClick();
        }

        private void MakeBinPatch3Button_Click(object sender, EventArgs e)
        {
            if (this.AFilename.Text.Length <= 0)
            {
                return;
            }
            if (this.BFilename.Text.Length <= 0)
            {
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("TEXT|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
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


            List<string> lines = new List<string>();
            lines.Add("TYPE=BIN");
            lines.Add("");

            bool PATCHED_IF_NotYet = true;
            uint PATCHED_IF_MinSize = (uint)RecoverMissMatchDiff3NumericUpDown.Value;

            string bin_patchfilename = save.FileName;

            int RecoverMissMatch = (int)RecoverMissMatchDiff3NumericUpDown.Value;
            int checkpoint = -1;

            byte[] a = File.ReadAllBytes(AFilename.Text);
            byte[] b = File.ReadAllBytes(BFilename.Text);

            int length = Math.Max(Math.Max(Program.ROM.Data.Length, a.Length),b.Length);
            if (Diff3Method.SelectedIndex == 0)
            {//AとBにあって、自分にだけないもの
                for (int i = 0; i < length; i++)
                {
                    uint ai = U.at(a, i);
                    uint bi = U.at(b, i);
                    uint ri = U.at(Program.ROM.Data, i);
                    if (ai != bi || ai == ri)
                    {
                        continue;
                    }

                    checkpoint = i;

                    i++;
                    int missCount = 0;
                    for (; i < length; i++)
                    {
                        ai = U.at(a, i);
                        bi = U.at(b, i);
                        ri = U.at(Program.ROM.Data, i);
                        if (!(ai != bi || ai == ri))
                        {
                            missCount = 0;
                            continue;
                        }

                        if (missCount >= RecoverMissMatch)
                        {
                            i -= missCount;
                            break;
                        }

                        missCount++;
                    }

                    //checkpoint ～ i の間を相違点として記録.
                    string split_filename = U.ToHexString8(checkpoint) + ".bin";
                    string split_filename_fullpath =
                        Path.Combine(Path.GetDirectoryName(bin_patchfilename), split_filename);

                    byte[] diff = U.subrange(a, (uint)checkpoint, (uint)i);
                    File.WriteAllBytes(split_filename_fullpath, diff);

                    if (PATCHED_IF_NotYet)
                    {
                        if (diff.Length > PATCHED_IF_MinSize)
                        {
                            lines.Add("PATCHED_IF:" + U.To0xHexString((uint)checkpoint) + "=" + U.DumpByte(diff));

                            PATCHED_IF_NotYet = false;
                        }
                    }

                    lines.Add("BINF:" + U.To0xHexString((uint)checkpoint) + "=" + split_filename);
                }
            }


            File.WriteAllLines(bin_patchfilename, lines);
            //エクスプローラで選択しよう
            U.SelectFileByExplorer(bin_patchfilename);
        }

        private void DiffToolForm_Load(object sender, EventArgs e)
        {

        }

        private void OtherFilename_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
