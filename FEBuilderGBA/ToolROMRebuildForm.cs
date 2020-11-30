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
    public partial class ToolROMRebuildForm : Form
    {
        public ToolROMRebuildForm()
        {
            InitializeComponent();

            OrignalFilename.AllowDropFilename();
            U.AddCancelButton(this);

            UseFreeAreaComboBox.SelectedIndex = 0;
        }

        private void OrignalSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("無改造ROMを選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

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
                return ;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            OrignalFilename.Text = open.FileNames[0];
        }

        string MakeROMRebuildFilename()
        {
            string upsFullPath = U.MakeFilename("R", ".rebuild");
            return upsFullPath;
        }

        private void MakeROMRebuildButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            if (!CheckRebuildAddress((uint)this.RebuildAddress.Value))
            {
                return;
            }

            if (this.DefragCheckBox.Checked)
            {
                if (Program.ROM.Modified)
                {
                    DialogResult dr = R.ShowYesNo("デフラグを実行すると、保存していないデータは失われます。\r\nセーブをした後で再度試してください。\r\n");
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            string errorMessage = MainFormUtil.CheckOrignalROM(OrignalFilename.Text);
            if (errorMessage != "")
            {
                R.ShowStopError("無改造ROMを指定してください。" + "\r\n" + errorMessage);
                OrignalFilename.ErrorMessage = R._("無改造ROMを指定してください。" + "\r\n" + errorMessage);
                return;
            }
            OrignalFilename.ErrorMessage = "";

            string title = R._("作成するROMRebuildファイル名を選択してください");
            string filter = R._("ROMRebuild|*.rebuild|All files|*");
            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, MakeROMRebuildFilename());

            {
                DialogResult dr = save.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);

            Make(save.FileName, OrignalFilename.Text, (uint)this.RebuildAddress.Value);

            if (this.DefragCheckBox.Checked)
            {
                string newROM = ReOpen(save.FileName, OrignalFilename.Text, UseFreeAreaComboBox.SelectedIndex);
                if (newROM == "")
                {
                    return;
                }

                //エクスプローラで選択しよう
                U.SelectFileByExplorer(newROM);
                R.ShowOK("デフラグ処理が完了しました。\r\n動作を検証するためクリアテストをお勧めします。\r\n\r\nデフラグされた新しいROM:\r\n{0}\r\n\r\nOKボタンを押すと、この新しいROMを開きます。", newROM);

                //メインフォームを開きなおさないといけない場合
                MainFormUtil.ReOpenMainForm();

                //保存したROMを開く.
                Program.LoadROM(newROM, "");
            }
            else
            {
                //エクスプローラで選択しよう
                U.SelectFileByExplorer(save.FileName);
                R.ShowOK("このROMの変更点を、すべてファイルに書きだしました。\r\nこのファイルをFEBuilderGBAから開いてください。\r\nファイル:{0}\r\n\r\n", save.FileName);
            }
            this.Close();
        }
        static void Make(string romRebuildFilename, string orignalFilename,uint rebuildAddress)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait())
            {
                ToolROMRebuildMake ROMRebuild = new ToolROMRebuildMake();
                ROMRebuild.Make(pleaseWait, orignalFilename, romRebuildFilename, rebuildAddress);
            }
        }
        static string ReOpen(string romRebuildFilename, string orignalFilename, int useFreeArea)
        {
            ROM rom = new ROM();
            string version;
            bool r = rom.Load(orignalFilename, out version);
            if (!r)
            {
                R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                return "";
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait())
            {
                r = ToolROMRebuildForm.ApplyROMRebuild(pleaseWait, rom, romRebuildFilename, useFreeArea);
                if (!r)
                {
                    U.SelectFileByExplorer(ToolROMRebuildApply.GetLogFilename(romRebuildFilename));
                    return "";
                }
            }

            string newFilename = U.ChangeExtFilename(romRebuildFilename, ".gba");
            rom.Save(newFilename, false);

            return newFilename;
        }

        public static int ComandLineRebuild()
        {
            String dir = Path.GetDirectoryName(Program.ROM.Filename);
            string orignalFilename = MainFormUtil.FindOrignalROM(dir);
            if (!File.Exists(orignalFilename))
            {
                U.echo(R.Error("無改造ROMを指定してください。"));
                return -1;
            }
            U.echo("ComandLineRebuild");

            string romRebuildFilename = Program.ROM.Filename;
            uint rebuildAddress = U.toOffset(Program.ROM.RomInfo.extends_address());
            Make(romRebuildFilename, orignalFilename, rebuildAddress);
            string stdout = ReOpen(romRebuildFilename,orignalFilename, 1);
            U.echo(stdout);

            return 0;
        }

        public static bool ApplyROMRebuild(InputFormRef.AutoPleaseWait wait, ROM vanilla, string filename, int useFreeArea)
        {
            ToolROMRebuildApply romRebuildApply = new ToolROMRebuildApply();
            return romRebuildApply.Apply(wait, vanilla, filename, useFreeArea);
        }

        private void OrignalFilename_DoubleClick(object sender, EventArgs e)
        {
            OrignalSelectButton.PerformClick();
        }

        private void ROMRebuildForm_Load(object sender, EventArgs e)
        {
            string orignal_romfile;
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(null))
            {
                if (Program.ROM.Data.Length 
                    <= U.toOffset(Program.ROM.RomInfo.extends_address()))
                {
                    R.ShowStopError("このROMは拡張領域を利用していないので、リビルドできません。");
                    this.Close();
                    return;
                }

                List<FELint.ErrorSt> systemErrorList = Program.AsmMapFileAsmCache.GetFELintCache(FELint.SYSTEM_MAP_ID);
                if (systemErrorList != null && systemErrorList.Count > 0)
                {
                    DialogResult dr =
                        R.ShowNoYes("警告\r\nこのROMにはシステムエラーがあるので、リビルドを実行するべきではありません。\r\n危険を承知した上でも、それでも実行しますか？");
                    if (dr != System.Windows.Forms.DialogResult.Yes)
                    {
                        this.Close();
                        return;
                    }
                }
                String dir = Path.GetDirectoryName(Program.ROM.Filename);
                orignal_romfile = MainFormUtil.FindOrignalROM(dir);
            }

            this.OrignalFilename.Text = orignal_romfile;
            this.RebuildAddress.Value = U.toOffset(Program.ROM.RomInfo.extends_address());
        }

        static bool CheckRebuildAddress(uint rebuildAddress)
        {
            if (!U.isPadding4(rebuildAddress))
            {
                R.ShowStopError("このアドレス({0})はリビルドに利用できません。\r\nアドレスが4で割り切れません。", U.To0xHexString(rebuildAddress));
                return false;
            }
            if (! U.isSafetyOffset(rebuildAddress))
            {
                R.ShowStopError("このアドレス({0})はリビルドに利用できません。\r\nアドレスの範囲が危険です。", U.To0xHexString(rebuildAddress));
                return false;
            }
            if (rebuildAddress < U.toOffset(Program.ROM.RomInfo.extends_address()))
            {
                DialogResult dr = R.ShowNoYes("拡張領域({0})より下のアドレス({1})をrebuildするのは危険です。\r\n続行してもよろしいですか?"
                    , U.To0xHexString(U.toOffset(Program.ROM.RomInfo.extends_address())), U.To0xHexString(rebuildAddress));
                if (dr != DialogResult.Yes)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
