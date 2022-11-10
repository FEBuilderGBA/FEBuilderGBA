using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace FEBuilderGBA
{
    public partial class ToolInitWizardForm : Form
    {
        public ToolInitWizardForm()
        {
            InitializeComponent();
            InputFormRef.TabControlHideTabOption(MainTab);
        }

        private void ToolInitWizardForm_Load(object sender, EventArgs e)
        {
            EmulatorTextBox.Text = Program.Config.at("emulator");
            DebuggerTextBox.Text = Program.Config.at("emulator2");
            ASMTextBox.Text = Program.Config.at("devkitpro_eabi");
            SappyTextBox.Text = Program.Config.at("sappy");
            EATextBox.Text = Program.Config.at("event_assembler");
            gba_mus_riper_TextBox.Text = Program.Config.at("gba_mus_riper");
            sox_TextBox.Text = Program.Config.at("sox");
            midfix4agb_TextBox.Text = Program.Config.at("midfix4agb");

            this.LANG_EN_Button.Text = "English"; ///No Translate
            this.LANG_JP_Button.Text = "日本語"; ///No Translate
            this.LANG_ZH_Button.Text = "中文"; ///No Translate
        }

        public static bool IsShowWizard()
        {
            string emulator = Program.Config.at("emulator");
            return (emulator == "") ;
        }

        enum Step1_Enum
        {
            Path
            ,DOWNLOAD_VBA_M
            , DOWNLOAD_mGBA
        }
        Step1_Enum Step1;

        enum Step2_Enum
        {
            Path
            ,DOWNLOAD_EA
            ,DO_NOT_SELECT
        }
        Step2_Enum Step2;

        enum Step3_Enum
        {
            Path
            ,DOWNLOAD_SAPPY
            ,DOWNLOAD_GBA_MUSIC_STDIO
            ,DO_NOT_SELECT
        }
        Step3_Enum Step3;

        enum Step4_Enum
        {
            Path
            ,DOWNLOAD_BOTH
            ,DO_NOT_SELECT
        }
        Step4_Enum Step4;

        enum Step5_Enum
        {
            Path
            ,DOWNLOAD_BOTH
            ,DO_NOT_SELECT
        }
        Step5_Enum Step5;

        void GotoPage2()
        {
            this.MainTab.SelectedTab = this.Step3Page;
            SetFocusIfEnable(EATextBox, DownloadEA, Step2NextButton);
        }

        private void Step1NextButton_Click(object sender, EventArgs e)
        {
            if (! File.Exists(EmulatorTextBox.Text))
            {
                return;
            }
            this.Step1 = Step1_Enum.Path;
            GotoPage2();
        }

        private void DownloadVBAM_Button_Click(object sender, EventArgs e)
        {
            this.Step1 = Step1_Enum.DOWNLOAD_VBA_M;
            GotoPage2();
        }

        private void DownloadMGBA_Button_Click(object sender, EventArgs e)
        {
            this.Step1 = Step1_Enum.DOWNLOAD_mGBA;
            GotoPage2();
        }

        void GotoPage3()
        {
            this.MainTab.SelectedTab = this.Step4Page;
            SetFocusIfEnable(SappyTextBox, DownloadVGMusicStudio, Step3NextButton);
        }

        private void Step2NextButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(EATextBox.Text))
            {
                return;
            }
            this.Step2 = Step2_Enum.Path;
            GotoPage3();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Step2 = Step2_Enum.DOWNLOAD_EA;
            GotoPage3();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Step2 = Step2_Enum.DO_NOT_SELECT;
            GotoPage3();
        }

        void GotoPage4()
        {
            this.MainTab.SelectedTab = this.Step4Page;
            SetFocusIfEnable(ASMTextBox, DownloadASM, Step4NextButton);
        }

        private void Step3NextButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(SappyTextBox.Text))
            {
                return;
            }
            this.Step3 = Step3_Enum.Path;
            GotoPage4();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Step3 = Step3_Enum.DOWNLOAD_SAPPY;
            GotoPage4();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            this.Step3 = Step3_Enum.DOWNLOAD_GBA_MUSIC_STDIO;
            GotoPage4();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            this.Step3 = Step3_Enum.DO_NOT_SELECT;
            GotoPage4();
        }

        void GotoPage5()
        {
            this.MainTab.SelectedTab = this.Step5Page;
            SetFocusIfEnable(midfix4agb_TextBox, DownloadMusictool, Step5NextButton);
        }

        private void Step4NextButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(DebuggerTextBox.Text))
            {
                return;
            }
            if (!File.Exists(ASMTextBox.Text))
            {
                return;
            }
            this.Step4 = Step4_Enum.Path;
            GotoPage5();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Step4 = Step4_Enum.DOWNLOAD_BOTH;
            GotoPage5();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Step4 = Step4_Enum.DO_NOT_SELECT;
            GotoPage5();
        }

        private void RefEmulatorButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("");
            if (r != "")
            {
                this.EmulatorTextBox.Text = r;
            }
        }

        private void RefEAButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("Core.exe|Core.exe|");
            if (r != "")
            {
                this.EATextBox.Text = r;
            }
        }

        private void RefSappyButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("SAPPY,VGMusicStudio|sappy.exe;VG Music Studio.exe|");
            if (r != "")
            {
                this.SappyTextBox.Text = r;
            }
        }

        private void RefDebuggerButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("NO$GBA.EXE|NO$GBA.EXE|");
            if (r != "")
            {
                this.DebuggerTextBox.Text = r;
            }
        }

        private void RefASMButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("arm-none-eabi-as.exe|arm-none-eabi-as.exe;as.exe|");
            if (r != "")
            {
                this.ASMTextBox.Text = r;
            }
        }
        InputFormRef.AutoPleaseWait SettingPleaseWait;
        void Setting()
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            this.MainTab.SelectedTab = this.SettingNowPage;
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                this.SettingPleaseWait = pleaseWait;
                SettingStep1();
                SettingStep2();
                SettingStep3();
                SettingStep4();
                SettingStep5();

                OptionForm f = new OptionForm();
                f.AutoClose();
            }
            this.MainTab.SelectedTab = this.EndPage;
        }

        bool IsErrorResult(string result)
        {
            if (result.IndexOf("\r\n") >= 0)
            {
                return true;
            }
            if (File.Exists(result))
            {//成功
                return false;
            }
            return true;
        }

        void SettingStep1()
        {
            string emu = "";
            if (this.Step1 == Step1_Enum.Path)
            {
                emu = this.EmulatorTextBox.Text;
            }
            else if (this.Step1 == Step1_Enum.DOWNLOAD_VBA_M)
            {
                string dir = Path.Combine(Program.BaseDirectory, "app", "VBA-M");
                string url = "https://github.com/FEBuilderGBA/FEBuilderGBA/releases/download/ver_20200316.21/VisualBoyAdvance-M.7z";
                string r = DownloadProgram_Direct(url, dir, "VisualBoyAdvance*.exe");
                if (IsErrorResult(r))
                {
                    R.ShowStopError(r);
                    return;
                }
                emu = r;
            }
            else if (this.Step1 == Step1_Enum.DOWNLOAD_mGBA)
            {
                string dir = Path.Combine(Program.BaseDirectory, "app", "mVBA");
                string url = "https://github.com/mgba-emu/mgba/releases/download/0.6.1/mGBA-0.6.1-win32.7z";
//                string pattern = "<a href=\"(/mgba-emu/mgba/releases/download/.+-win32.7z)\"";
                string r = DownloadProgram_Direct(url, dir, "mGBA.exe");
                if (IsErrorResult(r))
                {
                    R.ShowStopError(r);
                    return;
                }
                emu = r;
            }

            Program.Config["emulator"] = emu;
        }
        void SettingStep2()
        {
            string ea = "";
            if (this.Step2 == Step2_Enum.Path)
            {
                ea = this.EATextBox.Text;
            }
            else if (this.Step2 == Step2_Enum.DOWNLOAD_EA)
            {
                {
                    string dir = Path.Combine(Program.BaseDirectory, "app", "Event Assembler");
                    //https://feuniverse.us/t/event-assembler/1749
                    string url = "https://www.dropbox.com/s/4mql123thxb78kw/Event%20Assembler%20V11.1.3.zip?dl=1";
                    string r = DownloadProgram_Direct(url, dir, "Core.exe");
                    if (IsErrorResult(r))
                    {
                        R.ShowStopError(r);
                        return;
                    }
                    ea = r;
                }
                {
                    string dir = Path.Combine(Program.BaseDirectory, "app", "Event Assembler", "Tools");
                    string url = "https://github.com/StanHash/lyn/releases/download/v2.5.3/lyn.exe";
                    string r = DownloadProgram_DirectOneFile(url, dir, "lyn.exe");
                    if (IsErrorResult(r))
                    {
                        R.ShowStopError(r);
                        return;
                    }
                }
            }
            else if (this.Step2 == Step2_Enum.DO_NOT_SELECT)
            {
                return;
            }

            Program.Config["event_assembler"] = ea;
        }
        void SettingStep3()
        {
            string sappy = "";
            if (this.Step3 == Step3_Enum.Path)
            {
                sappy = this.SappyTextBox.Text;
            }
            else if (this.Step3 == Step3_Enum.DOWNLOAD_SAPPY)
            {
                string dir = Path.Combine(Program.BaseDirectory, "app", "sappy");
//                string url = "https://github.com/FEBuilderGBA/FEBuilderGBA/releases/download/ver_20200316.21/SappyInstaller.7z";
                string url = "https://www.dropbox.com/sh/723s9jdkfkx7pwa/AABrXCMghyx2f74fme6iDoTEa?dl=1";
                string r = DownloadProgram_Direct(url, dir, "sappy.exe");
                if (IsErrorResult(r))
                {
                    R.ShowStopError(r);
                    return;
                }
                RunSappyInstaller(dir);
                sappy = r;
            }
            else if (this.Step3 == Step3_Enum.DOWNLOAD_GBA_MUSIC_STDIO)
            {
                string dir = Path.Combine(Program.BaseDirectory, "app", "GBAMusicStdio");
                string url = "https://cdn.discordapp.com/attachments/600877394764627968/948313405431894077/VGMusicStudio.7z";
                string r = DownloadProgram_Direct(url, dir, "VG Music Studio.exe");
                if (IsErrorResult(r))
                {
                    R.ShowStopError(r);
                    return;
                }
                sappy = r;
            }
            else if (this.Step3 == Step3_Enum.DO_NOT_SELECT)
            {
                return;
            }

            Program.Config["sappy"] = sappy;

            if (this.Step3 == Step3_Enum.DOWNLOAD_GBA_MUSIC_STDIO)
            {//GBA MUSIC SUDIOの場合でもsappyをダウンロードしておきます。
                string dir = Path.Combine(Program.BaseDirectory, "app", "sappy");
                //                string url = "https://github.com/FEBuilderGBA/FEBuilderGBA/releases/download/ver_20200316.21/SappyInstaller.7z";
                string url = "https://www.dropbox.com/sh/723s9jdkfkx7pwa/AABrXCMghyx2f74fme6iDoTEa?dl=1";
                string r = DownloadProgram_Direct(url, dir, "sappy.exe");
                if (IsErrorResult(r))
                {
                    R.ShowStopError(r);
                    return;
                }
                sappy = r;
            }
            //もしmid2agbがあれば同時に設定する.
            {
                string r = GrepFile(Path.GetDirectoryName(sappy), "mid2agb.exe");
                if (!IsErrorResult(r))
                {
                    Program.Config["mid2agb"] = r;
                    Program.Config["func_midi_importer"] = "1";
                }
            }
        }

        void SettingStep4()
        {
            string debugger = "";
            string asm = "";
            if (this.Step4 == Step4_Enum.Path)
            {
                debugger = this.DebuggerTextBox.Text;
                asm = this.ASMTextBox.Text;
            }
            else if (this.Step4 == Step4_Enum.DOWNLOAD_BOTH)
            {
                {
                    string dir = Path.Combine(Program.BaseDirectory, "app", "no$gba");
                    string url = "https://problemkaputt.de/no$gba.zip";
                    string r = DownloadProgram_Direct(url, dir, "NO$GBA.EXE");
                    if (IsErrorResult(r))
                    {
                        url = "https://cdn.discordapp.com/attachments/470029781795078175/1039723314286956594/nogba.7z";
                        r = DownloadProgram_Direct(url, dir, "NO$GBA.EXE");
                        if (IsErrorResult(r))
                        {
                            R.ShowStopError(r);
                            return;
                        }
                    }
                    debugger = r;
                }
                {
                    string dir = Path.Combine(Program.BaseDirectory, "app", "asm");
                    string url = "https://github.com/FireEmblemUniverse/SkillSystem_FE8/raw/master/Tools/devkitARM/bin/arm-none-eabi-as.exe";
                    string r = DownloadProgram_Direct(url, dir, "arm-none-eabi-as.exe");
                    if (IsErrorResult(r))
                    {
                        R.ShowStopError(r);
                        return;
                    }
                    asm = r;
                }
            }
            else if (this.Step4 == Step4_Enum.DO_NOT_SELECT)
            {
                return;
            }

            Program.Config["devkitpro_eabi"] = asm;
            Program.Config["emulator2"] = debugger;
        }

        void SettingStep5()
        {
            string gba_mus_riper = "";
            string sox = "";
            string midfix4agb = "";
            if (this.Step5 == Step5_Enum.Path)
            {
                gba_mus_riper = this.gba_mus_riper_TextBox.Text;
                sox = this.sox_TextBox.Text;
                midfix4agb = this.midfix4agb_TextBox.Text;
            }
            else if (this.Step5 == Step5_Enum.DOWNLOAD_BOTH)
            {
                {
                    string dir = Path.Combine(Program.BaseDirectory, "app", "gba_mus_riper");
                    string url = "https://github.com/FEBuilderGBA/FEBuilderGBA/releases/download/ver_20200316.21/gba_mus_riper_v24.7z";
                    string r = DownloadProgram_Direct(url, dir, "song_riper.exe");
                    if (IsErrorResult(r))
                    {
                        R.ShowStopError(r);
                        return;
                    }
                    gba_mus_riper = r;
                }
                {
                    string dir = Path.Combine(Program.BaseDirectory, "app", "sox");
                    string url = "https://github.com/FEBuilderGBA/FEBuilderGBA/releases/download/ver_20200316.21/sox-14.4.2.7z";
                    string r = DownloadProgram_Direct(url, dir, "sox.exe");
                    if (IsErrorResult(r))
                    {
                        R.ShowStopError(r);
                        return;
                    }
                    sox = r;
                }
                {
                    string dir = Path.Combine(Program.BaseDirectory, "app", "midfix4agb");
                    string url = "https://cdn.discordapp.com/attachments/145137778710151168/775008685360807956/midfix4agb.7z";
                    string r = DownloadProgram_Direct(url, dir, "midfix4agb.exe");
                    if (IsErrorResult(r))
                    {
                        R.ShowStopError(r);
                        return;
                    }
                    midfix4agb = r;
                }
            }
            else if (this.Step5 == Step5_Enum.DO_NOT_SELECT)
            {
                return;
            }

            Program.Config["sox"] = sox;
            Program.Config["gba_mus_riper"] = gba_mus_riper;
            Program.Config["midfix4agb"] = midfix4agb;
        }

        string DownloadProgram_DirectOneFile(string url, string path, string filename)
        {
            string tempFilename = Path.GetTempFileName();
            try
            {
                U.HttpDownload(tempFilename, url, U.GetURLBaseDir(url), this.SettingPleaseWait);
            }
            catch (Exception e)
            {
                File.Delete(tempFilename);
                return R.Error("ファイルをダウンロードできませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfilename:{2}", url, path, filename) + "\r\n" + e.ToString();
            }
            if (!File.Exists(tempFilename))
            {
                File.Delete(tempFilename);
                return R.Error("ファイルをダウンロードできませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfilename:{2}", url, path, filename);
            }

            string retPath = Path.Combine(path, filename);
            File.Copy(tempFilename, retPath);
            return retPath;
        }
        string DownloadProgram_Direct(string url, string dir, string findEXE)
        {
            string tempFilename;
            try
            {
                tempFilename = Path.GetTempFileName();
            }
            catch (Exception e)
            {
                return R.Error("ダウンロードするためのテンポラリファイルを作成できませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfindEXE:{2}", url, dir, findEXE) + "\r\n" + e.ToString();
            }

            try
            {
                U.HttpDownload(tempFilename, url, U.GetURLBaseDir(url), this.SettingPleaseWait);
            }
            catch (Exception e)
            {
                File.Delete(tempFilename);
                return R.Error("ファイルをダウンロードできませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfindEXE:{2}", url, dir, findEXE) + "\r\n" + e.ToString();
            }

            if (! File.Exists(tempFilename))
            {
                return R.Error("ファイルをダウンロードできませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfindEXE:{2}", url, dir, findEXE);
            }

            try
            {
                string ext = Path.GetExtension(U.GetURLFilename(url));
                if (ext == "")
                {
                    ext = ".zip";
                }
                else if (ext == ".exe" || ext == ".EXE")
                {
                    string filename = U.GetURLFilename(url);
                    string retPath = Path.Combine(dir, filename);

                    U.mkdir(dir);
                    File.Copy(tempFilename, retPath, true);
                    File.Delete(tempFilename);
                    return retPath;
                }

                File.Move(tempFilename, tempFilename + ext);
                tempFilename = tempFilename + ext;

                this.SettingPleaseWait.DoEvents("Extract...");
                U.mkdir(dir);
                string r = ArchSevenZip.Extract(tempFilename, dir);
                if (r != "")
                {
                    return R.Error("ダウンロードしたファイルを解凍できませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfindEXE:{2}", url, dir, findEXE);
                }

                File.Delete(tempFilename);
                return GrepFile(dir, findEXE);
            }
            catch (Exception e)
            {
                return R.Error("ダウンロードしたファイルを開けませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfindEXE:{2}", url, dir, findEXE) + "\r\n" + e.ToString();
            }
        }
        string GrepFile(string dir , string findEXE)
        {
            this.SettingPleaseWait.DoEvents("Grep... :" + findEXE);
            string[] exeList = U.Directory_GetFiles_Safe(dir, findEXE, SearchOption.AllDirectories);
            if (exeList.Length <= 0)
            {
                return R.Error("ダウンロードしたファイルに目的のファイルがありません。\r\nPATH:{0}\r\nfindEXE:{1}", dir, findEXE);
            }

            return exeList[0];
        }
        string DownloadProgram_Grep(string url, string dir, string findEXE, string pattern)
        {
            string html = U.HttpGet(url);
            Match match = RegexCache.Match(html, pattern);
            if (match.Groups.Count <= 1)
            {
                return R.Error("ダウンロード先が見つかりませんでした。\r\nURL:{0}\r\nPATH:{1}\r\nfindEXE:{2}\r\npattern:{3}", url, dir, findEXE, pattern);
            }
            string binary_url = match.Groups[1].Value;
            binary_url = U.MakeFullURLPath(url, binary_url);

            return DownloadProgram_Direct(binary_url, dir, findEXE);
        }
        void RunSappyInstaller(string dir)
        {
            string windir = System.Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            string vbalCbEx6Path = Path.Combine(windir, "system32", "vbalCbEx6.ocx");
            if (File.Exists(vbalCbEx6Path))
            {
                DialogResult dr = R.ShowYesNo("既にVB6ランタイムがインストールされているようです。\r\nランタイムインストールを省略しますか？");
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            string sappyInstaller = GrepFile(dir, "Installateur_Sappy.exe");
            if (IsErrorResult(sappyInstaller))
            {
                R.ShowStopError(sappyInstaller);
                return ;
            }

            R.ShowOK("SappyはVB6で書かれているので、ランタイムをインストールする必要があります。\r\nランタイムのインストーラーを起動します。");

            try
            {
                Process p = Process.Start(sappyInstaller);
                p.WaitForExit();
            }
            catch (Exception ee)
            {
                R.ShowStopError(ee.ToString());
            }
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        void SetFocusIfEnable(TextBox path, Button download, Button setting = null)
        {
            if (setting != null && File.Exists(path.Text))
            {
                setting.Focus();
            }
            else
            {
                download.Text += R._("(ディフォルト)");
                download.Focus();
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.MainTab.SelectedTab = this.Step1Page;
            SetFocusIfEnable(EmulatorTextBox, DownloadVBAM_Button, Step1NextButton);
        }

        private void LANG_EN_Button_Click(object sender, EventArgs e)
        {
            Program.Config["func_lang"] = "en";
            CloseAndResetInstance();
        }

        private void LANG_JP_Button_Click(object sender, EventArgs e)
        {
            Program.Config["func_lang"] = "ja";
            CloseAndResetInstance();
        }

        private void LANG_ZH_Button_Click(object sender, EventArgs e)
        {
            Program.Config["func_lang"] = "zh";
            CloseAndResetInstance();
        }

        private void WhiteBGButton_Click(object sender, EventArgs e)
        {
            CloseAndResetInstance(1);
        }

        private void BlackBGButton_Click(object sender, EventArgs e)
        {
            CloseAndResetInstance(2);
        }
        private void BlackBG2Button_Click(object sender, EventArgs e)
        {
            CloseAndResetInstance(3);
        }

        void CloseAndResetInstance(int autocolor=0)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                OptionForm.ClearCache();

                this.SettingPleaseWait = pleaseWait;
                OptionForm f = new OptionForm();
                f.AutoClose(autocolor);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

        private void Step1PrevButton_Click(object sender, EventArgs e)
        {
            this.MainTab.SelectedTab = this.BeginPage;
        }

        private void Step2PrevButton_Click(object sender, EventArgs e)
        {
            this.MainTab.SelectedTab = this.Step1Page;
        }

        private void Step3PrevButton_Click(object sender, EventArgs e)
        {
            this.MainTab.SelectedTab = this.Step2Page;
        }

        private void Step4PrevButton_Click(object sender, EventArgs e)
        {
            this.MainTab.SelectedTab = this.Step3Page;
        }

        private void Step5PrevButton_Click(object sender, EventArgs e)
        {
            this.MainTab.SelectedTab = this.Step4Page;
        }

        private void Step5NextButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(gba_mus_riper_TextBox.Text))
            {
                return;
            }
            if (!File.Exists(sox_TextBox.Text))
            {
                return;
            }
            this.Step5 = Step5_Enum.Path;
            Setting();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Step5 = Step5_Enum.DOWNLOAD_BOTH;
            Setting();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Step5 = Step5_Enum.DO_NOT_SELECT;
            Setting();
        }

        private void Refgba_mus_riperButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("song_riper|song_riper.exe|");
            if (r != "")
            {
                this.gba_mus_riper_TextBox.Text = r;
            }
        }

        private void RefSOXButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("sox|sox.exe|");
            if (r != "")
            {
                this.sox_TextBox.Text = r;
            }
        }

        private void midfix4agbButton_Click(object sender, EventArgs e)
        {
            string r = OptionForm.EXESearch("midfix4agb|midfix4agb.exe|");
            if (r != "")
            {
                this.midfix4agb_TextBox.Text = r;
            }
        }



    }
}
