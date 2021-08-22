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
    public partial class ToolROMRebuildOpenSimpleForm : Form
    {
        public ToolROMRebuildOpenSimpleForm()
        {
            InitializeComponent();

            OrignalFilename.AllowDropFilename();
            U.AddCancelButton(this);

            UseFreeAreaComboBox.SelectedIndex = 1;
            this.FreeAreaMinimumSize.Value = 2048;
            this.FreeAreaStartAddress.Value = U.Padding4(Program.ROM.RomInfo.compress_image_borderline_address());

            X_UseFreeArea.AccessibleDescription = ToolROMRebuildOpenSimpleForm.GetExplainFreeArea();
            X_FreeAreaMinimumSize.AccessibleDescription = ToolROMRebuildOpenSimpleForm.GetExplainFreeAreaMinimumSize();
            X_FreeAreaStartAddress.AccessibleDescription = ToolROMRebuildOpenSimpleForm.GetExplainFreeAreaStartAddress();
        }

        private void OrignalSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("無改造ROMを選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            if (Program.LastSelectedFilename != null)
            {
                Program.LastSelectedFilename.Load(this, "", open);
            }
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return ;
            }
            if (!U.CanReadFileRetry(open))
            {
                return ;
            }

            if (Program.LastSelectedFilename != null)
            {
                Program.LastSelectedFilename.Save(this, "", open);
            }
            OrignalFilename.Text = open.FileNames[0];
        }

        private void ApplyROMRebuildButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string errorMessage = MainFormUtil.CheckOrignalROM(OrignalFilename.Text);
            if (errorMessage != "")
            {
                R.ShowStopError("無改造ROMを指定してください。" + "\r\n" + errorMessage);
                OrignalFilename.ErrorMessage = R._("無改造ROMを指定してください。" + "\r\n" + errorMessage);
                return;
            }
            OrignalFilename.ErrorMessage = "";

            ROM rom = new ROM();
            string version;
            bool r = rom.Load(OrignalFilename.Text, out version);
            if (!r)
            {
                R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                r = ToolROMRebuildForm.ApplyROMRebuild(pleaseWait, rom, this.ROMRebuildFilename
                    , UseFreeAreaComboBox.SelectedIndex
                    , (uint)FreeAreaMinimumSize.Value, (uint)FreeAreaStartAddress.Value);
                if (!r)
                {
                    U.SelectFileByExplorer(ToolROMRebuildApply.GetLogFilename(this.ROMRebuildFilename));
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

            if (this.UseReOpen)
            {//メインフォームを開きなおさないといけない場合
                MainFormUtil.ReOpenMainForm();
            }

            if (this.IsSaveFileCheckBox.Checked)
            {
                string newFilename = U.ChangeExtFilename(this.ROMRebuildFilename, ".gba");
                rom.Save(newFilename, false);

                //エクスプローラで選択しよう
                U.SelectFileByExplorer(newFilename);

                //保存したROMを開く.
                Program.LoadROM(newFilename, this.ForceVersion);
            }
            else
            {
                //保存しない場合、メモリ上の存在になる.
                Program.LoadVirtualROM(rom, this.ROMRebuildFilename);
            }
        }

        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }

        string ROMRebuildFilename;
        bool UseReOpen;
        string ForceVersion;
        public void OpenROMRebuild(string ROMRebuildFilename, bool useReOpen, string forceversion)
        {
            this.ROMRebuildFilename = ROMRebuildFilename;
            this.UseReOpen = useReOpen;
            this.ForceVersion = forceversion;
        }

        private void ROMRebuildOpenSimpleForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                String dir = Path.GetDirectoryName(this.ROMRebuildFilename);
                String filename = Path.GetFileName(this.ROMRebuildFilename);
                pleaseWait.DoEvents(R._("{0}に適合する元ファイルを自動検索中・・・", filename));

                string[] lines = File.ReadAllLines(this.ROMRebuildFilename);
                uint srcCRC32 = ToolROMRebuildApply.GetCRC32(lines);

                string orignal_romfile = MainFormUtil.FindOrignalROMByCRC32(dir, srcCRC32);
                this.OrignalFilename.Text = orignal_romfile;
            }
        }

        private void ToolROMRebuildOpenSimpleForm_Load(object sender, EventArgs e)
        {

        }

        private void UseFreeAreaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UseFreeAreaComboBox.SelectedIndex <= 0)
            {
                X_FreeAreaDef.Hide();
            }
            else
            {
                X_FreeAreaDef.Show();
            }
        }

        public static string GetExplainFreeArea()
        {
            return R._("ROMの前方にあるフリー領域も利用するかどうかを定義します。\r\nフリー領域も利用した方がROMが小さくなりますが、SkillSystemsなどのEA(buildfile)を使っている場合は、問題が発生することもあります。");
        }
        public static string GetExplainFreeAreaMinimumSize()
        {
            return R._("この数だけnullが連続する場所があればフリー領域とみなします。\r\nCSASpellや画像データなどの役割が判明していて誤爆しやすい場所は最初から除外されています。\r\nただ、未知のデータで偶然にもnullがたくさん入っているデータ構造がある場合は、値を小さくすると誤爆する可能性が高まります。\r\n値を小さくすると、より小さなnull領域も再利用してROMが小さくなりますが、誤爆の可能性も増えます。");
        }
        public static string GetExplainFreeAreaStartAddress()
        {
            return R._("空き領域の探索を開始するアドレスです。\r\nディフォルトは、compress_image_borderline_address です。\r\nこれは、プログラムコードとデータ領域を分離できるアドレスです。\r\n基本的に変更しないでください。");
        }
    }
}
