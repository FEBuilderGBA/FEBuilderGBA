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
            TakeoverSkillAssignmentComboBox.SelectedIndex = 1;
        }

        private void TargetFilenameTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TargetFilenameTextBox.Text = TargetFilenameOpenFile();
        }
        private void TargetFilenameSelectButton_Click(object sender, EventArgs e)
        {
            TargetFilenameTextBox.Text = TargetFilenameOpenFile();
        }
        string TargetFilenameOpenFile()
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("MAKE CUSTOM_BUILD.cmd|MAKE CUSTOM_BUILD.cmd|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "TargetFilenameOpenFile", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "";
            }
            if (!U.CanReadFileRetry(open))
            {
                return "";
            }

            Program.LastSelectedFilename.Save(this, "TargetFilenameOpenFile", open);
            return open.FileNames[0];
        }


        private void OrignalROMSelectButton_Click(object sender, EventArgs e)
        {
            OrignalROMTextArea.Text = OrignalROMOpenFile();
        }

        private void OrignalROMTextArea_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OrignalROMTextArea.Text = OrignalROMOpenFile();
        }
        string OrignalROMOpenFile()
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "OrignalROMOpenFile", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "";
            }
            if (!U.CanReadFileRetry(open))
            {
                return "";
            }

            Program.LastSelectedFilename.Save(this, "OrignalROMOpenFile", open);
            return open.FileNames[0];
        }


        private void RunButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                Run();
            }
        }

        string EABuildROM_Filename = "";
        PatchForm.PatchSt ParentPatch;

        void Run()
        {
            string target_filename = this.TargetFilenameTextBox.Text;
            if (! File.Exists(target_filename))
            {
                R.ShowStopError("ターゲット({0})が見つかりませんでした。", target_filename);
                return;
            }
            string orignal = this.OrignalROMTextArea.Text;
            if (!File.Exists(orignal))
            {
                R.ShowStopError("無改造ROM({0})が見つかりませんでした", orignal);
                return;
            }

            string parentPatchFilename = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE8U"
                , "skill20200512", "PATCH_Skill20200512.txt");
            this.ParentPatch = PatchForm.LoadPatch(parentPatchFilename, isScanOnly: false);
            if (this.ParentPatch == null)
            {
                R.ShowStopError("ベースとなるシステム({0})が見つかりませんでした", parentPatchFilename);
                return;
            }

            bool r;
            if (U.GetFilenameExt(target_filename) == ".CMD")
            {
                r = BuildCMD();
            }
            else
            {
                r = BuildEA();
            }
            if (!r)
            {
                return;
            }

            //マージデータの作成を行う
            r = MargeAndUpdate();
            if (!r)
            {
                return;
            }

            R.ShowOK("完成");
        }

        //バニラROMをコピー
        bool CopyVanilla()
        {
            return true;
        }

        bool BuildCMD()
        {//CMDでビルドする
            string target_filename = this.TargetFilenameTextBox.Text;
            if (U.GetFilenameExt(target_filename) != ".CMD")
            {
                Debug.Assert(false);
                return false;
            }
            //実行ファイルなので愚直に実行.

            string basedir = Path.GetDirectoryName(target_filename);
            string eaTargetROM_Filename = Path.Combine(basedir, "FE8_clean.gba");
            this.EABuildROM_Filename = Path.Combine(basedir, "SkillsTest.gba");

            //まずは無改造ROMを所定の名前でコピーする
            string orignal = this.OrignalROMTextArea.Text;
            if (!File.Exists(orignal))
            {
                return false;
            }

            if (!File.Exists(eaTargetROM_Filename))
            {
                U.CopyFile(orignal, eaTargetROM_Filename);
            }

            string args = "";
            string output = MainFormUtil.ProgramRunAsAndEndWait(target_filename, args, basedir);
            Log.Notify("=== OUTPUT ===");
            Log.Notify(output);
            if (MainFormUtil.IsCompilerErrorByEventAssembler(output))
            {
                //エラーなので詳細を追加する.
                output = target_filename + " " + args + " \r\noutput:\r\n" + output;
                R.ShowStopError(output);
                return false;
            }

            return true;
        }

        //EAでビルドを行う.
        bool BuildEA()
        {
/*
            string target_filename = this.TargetFilenameTextBox.Text;
            if (U.GetFilenameExt(target_filename) == ".CMD")
            {
                Debug.Assert(false);
                return false;
            }

            //EventAssemblerで対象物をコンパイル
            string output, out_symbol;
            bool r = MainFormUtil.CompilerEventAssembler(target_filename, U.NOT_FOUND, U.NOT_FOUND, out output, out out_symbol);
            if (!r)
            {
                R.ShowStopError(output);
                return false;
            }

            this.EABuildROM_Filename = output;

            return true;
*/
            R.ShowStopError("まだ未実装です!");
            return false;
        }

        //マージデータの作成を行う
        bool MargeAndUpdate()
        {
            string orignal = this.OrignalROMTextArea.Text;
            if (!File.Exists(orignal))
            {
                return false;
            }
            byte[] orignalBIN = File.ReadAllBytes(orignal);
            byte[] buildBIN = File.ReadAllBytes(this.EABuildROM_Filename);

            string skill_CustomBuildDirectory = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE8U"
                , "skill_CustomBuild");
            U.mkdir(skill_CustomBuildDirectory);

            U.CopyDirectory(Path.GetDirectoryName(ParentPatch.PatchFileName), skill_CustomBuildDirectory);
            U.DeleteFile(skill_CustomBuildDirectory, "0*.bin");
            U.DeleteFile(skill_CustomBuildDirectory, "PATCH_*.txt");

            //シンボル情報のコピー
            CopySYM(skill_CustomBuildDirectory);

            //ビルドしたROMとバニラを比較して、差分を作る
            string custombuild = Path.Combine(skill_CustomBuildDirectory, "PATCH_SkillSystem_CustomBuild.txt");
            ToolDiffForm.MakeDiff(custombuild, orignalBIN, buildBIN, 10, true);

            //パッチテキストをマージする
            MargePatch(custombuild, ParentPatch.PatchFileName, U.atoh(TakeoverSkillAssignmentComboBox.Text));

            //作成したパッチを読込み
            PatchForm.PatchSt newPatchSt = PatchForm.LoadPatch(custombuild, isScanOnly: false);

            //パッチアップデート
            PatchForm patchF = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            patchF.UpdatePatch(newPatchSt);
            
            return true;
        }
        void CopySYM(string skillCustomBuildDirectory)
        {
            string target_filename = this.TargetFilenameTextBox.Text;
            string basedir = Path.GetDirectoryName(target_filename);

            string src = Path.Combine(basedir, "SkillsTest.sym");
            string dest = Path.Combine(skillCustomBuildDirectory, "symbol.sym");
            U.CopyFile(src, dest);
        }
        void MargePatch(string custombuild, string curentPatchFileName, uint takeoverSkillAssignment)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE_UNINSTALL:0=" + curentPatchFileName);
            string[] lines = File.ReadAllLines(curentPatchFileName);
            foreach (string line in lines)
            {
                if (line.IndexOf("PATCHED_IF") == 0)
                {
                    continue;
                }
                if (line.IndexOf("BINF:") == 0)
                {
                    continue;
                }
                if (line.IndexOf("BIN:") == 0)
                {
                    continue;
                }
                if (line.IndexOf("AFTER_TRY_EXECUTE:") == 0)
                {
                    continue;
                }
                if (line.IndexOf("NAME") == 0)
                {
                    continue;
                }
                if (line.IndexOf("INFO") == 0)
                {
                    continue;
                }
                if (line.IndexOf("TEXTADV:") == 0)
                {
                    continue;
                }
                if (line.IndexOf("EXTENDS:") == 0)
                {
                    continue;
                }
                if (takeoverSkillAssignment == 0)
                {//引き継がない
                    if (line.IndexOf("UPDATE_METHOD=SKILLSYSTEM") == 0)
                    {
                        continue;
                    }
                    if (line.IndexOf("EDIT_PATCH:") == 0)
                    {
                        continue;
                    }
                }
            
                sb.AppendLine(line);
            }

            sb.AppendLine(File.ReadAllText(custombuild));
            File.WriteAllText(custombuild,sb.ToString());
        }

        private void ToolCustomBuildForm_Load(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                this.Close();
                return;
            }
            PatchUtil.skill_system_enum skill_system = PatchUtil.SearchSkillSystem();
            if (skill_system != PatchUtil.skill_system_enum.SkillSystem)
            {
                R.ShowStopError("SkillSystemsがインストールされていません。");

                this.Close();
                return;
            }

            {
                String dir = Path.GetDirectoryName(Program.ROM.Filename);
                string orignal_romfile = MainFormUtil.FindOrignalROM(dir);
                this.OrignalROMTextArea.Text = orignal_romfile;
            }
            this.Explain.Text +=
                "https://dw.ngmansion.xyz/doku.php?id=en:en:guide:febuildergba:skillsystems_custombuild"; ///No Translate
        }



    }
}
