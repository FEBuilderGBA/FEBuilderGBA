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
            UseShareSameDataComboBox.SelectedIndex = 1;
            this.FreeAreaMinimumSize.Value = 2048;
            this.FreeAreaStartAddress.Value = U.Padding4(Program.ROM.RomInfo.compress_image_borderline_address);
            AppendFreeAreaFilename.Placeholder = R._("無指定の場合は追加設定ファイルを利用しません。");

            X_RebuildAddress.AccessibleDescription = GetExplainRebuildAddress();
            X_UseFreeArea.AccessibleDescription = GetExplainFreeArea();
            X_FreeAreaMinimumSize.AccessibleDescription = GetExplainFreeAreaMinimumSize();
            X_FreeAreaStartAddress.AccessibleDescription = GetExplainFreeAreaStartAddress();
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

            if (Program.ROM.Modified)
            {
                DialogResult dr = R.ShowYesNo("デフラグを実行すると、保存していないデータは失われます。\r\nセーブをした後で再度試してください。\r\n");
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    return;
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

            //自動適応
            string newROM = ReOpen(save.FileName, OrignalFilename.Text
                , UseFreeAreaComboBox.SelectedIndex
                , (uint)FreeAreaMinimumSize.Value
                , (uint)FreeAreaStartAddress.Value
                , AppendFreeAreaFilename.Text
                , (uint)UseShareSameDataComboBox.SelectedIndex);
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
        static string ReOpen(string romRebuildFilename, string orignalFilename, int useFreeArea, uint freeAreaMinimumSize, uint freeAreaStartAddress, string appendFreeAreaFilename, uint useShareSameData)
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
                r = ToolROMRebuildForm.ApplyROMRebuild(pleaseWait, rom, romRebuildFilename
                    , useFreeArea
                    , freeAreaMinimumSize
                    , freeAreaStartAddress
                    , appendFreeAreaFilename
                    , useShareSameData);
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
            uint rebuildAddress = U.toOffset(Program.ROM.RomInfo.extends_address);
            Make(romRebuildFilename, orignalFilename, rebuildAddress);

            uint addr = U.Padding4(Program.ROM.RomInfo.compress_image_borderline_address);
            uint freeAreaMinimumSize = 2048;
            uint freeAreaStartAddress = U.Padding4(Program.ROM.RomInfo.compress_image_borderline_address);
            string appendFreeAreaFilename = "";
            uint useShareSameData = 1;
            string stdout = ReOpen(romRebuildFilename, orignalFilename, 1, freeAreaMinimumSize, freeAreaStartAddress, appendFreeAreaFilename, useShareSameData);
            U.echo(stdout);

            return 0;
        }

        public static bool ApplyROMRebuild(InputFormRef.AutoPleaseWait wait, ROM vanilla, string filename, int useFreeArea, uint freeAreaMinimumSize, uint freeAreaStartAddress, string appendFreeAreaFilename, uint useShareSameData)
        {
            ToolROMRebuildApply romRebuildApply = new ToolROMRebuildApply();
            return romRebuildApply.Apply(wait, vanilla, filename, useFreeArea ,freeAreaMinimumSize, freeAreaStartAddress, appendFreeAreaFilename, useShareSameData);
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
                    <= U.toOffset(Program.ROM.RomInfo.extends_address))
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
            this.RebuildAddress.Value = U.toOffset(Program.ROM.RomInfo.extends_address);
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
            if (rebuildAddress < U.toOffset(Program.ROM.RomInfo.extends_address))
            {
                DialogResult dr = R.ShowNoYes("拡張領域({0})より下のアドレス({1})をrebuildするのは危険です。\r\n続行してもよろしいですか?"
                    , U.To0xHexString(U.toOffset(Program.ROM.RomInfo.extends_address)), U.To0xHexString(rebuildAddress));
                if (dr != DialogResult.Yes)
                {
                    return false;
                }
            }
            return true;
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

        public static string GetExplainRebuildAddress()
        {
            return R._("このアドレス以降のデータを再構築します。\r\nディフォルトは{0}です。\r\n通常は変更しないでください。\r\nもしリビルドに失敗する用であれば、この値を大きくしてください。\r\nリビルドに失敗する主な理由は、独自に追加したASMです。\r\nリビルド後にそのASMを再インストールすると動くことがあります。\r\nもし、それができない場合は、この値を大きくして、そのASMがインストールされている領域まではリビルドしないことで、動作させることもできます。", U.To0xHexString(Program.ROM.RomInfo.extends_address) );
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

        private void AppendFreeAreaFilenameSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("逆アセンブルデータからFREEAREAとマークされている追加データをいれてください。");
            string filter = R._("Text|*.txt|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            AppendFreeAreaFilename.Text = open.FileNames[0];
        }
    }
}
