using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace FEBuilderGBA
{
    //メインフォームが GBA 3部作でわかれてしまうのでコピペするより共通部分を切り出す
    public class MainFormUtil
    {
        public static void Open(Form self)
        {
            if (!MainFormUtil.IsNotSaveYet(self))
            {
                return ;
            }
            MainFormUtil.ClearModifiedFlag();

            string romfilename = Program.OpenROMDialog();
            if (romfilename == "")
            {
                return;
            }
            Open(self, romfilename,true,"");
        }
        public static bool Open(Form self, string romfilename, bool useReOpen, string forceversion)
        {
            if (romfilename == "")
            {
                return false;
            }
            if (!MainFormUtil.IsNotSaveYet(self))
            {
                return false;
            }
            MainFormUtil.ClearModifiedFlag();

            string ext = U.GetFilenameExt(romfilename);
            if (ext == ".UPS")
            {
                ToolUPSOpenSimpleForm ups = (ToolUPSOpenSimpleForm)InputFormRef.JumpFormLow<ToolUPSOpenSimpleForm>();
                ups.OpenUPS(romfilename, useReOpen, forceversion);
                ups.ShowDialog();

                return ups.DialogResult == DialogResult.OK;
            }
            else if (ext == ".REBUILD")
            {
                ToolROMRebuildOpenSimpleForm ROMRebuild = (ToolROMRebuildOpenSimpleForm)InputFormRef.JumpFormLow<ToolROMRebuildOpenSimpleForm>();
                ROMRebuild.OpenROMRebuild(romfilename, useReOpen, forceversion);
                ROMRebuild.ShowDialog();

                return ROMRebuild.DialogResult == DialogResult.OK;
            }
            else if (ext == ".7Z")
            {
                return OpenGBA7ZROM(romfilename, useReOpen, forceversion);
            }
            else
            {
                if (useReOpen)
                {
                    ReOpenMainForm();
                }
                bool r = Program.LoadROM(romfilename, forceversion);
                if (r)
                {
                    ToolWorkSupportForm.CheckUpdateSlientByThread();
                }
                return r;
            }
        }

        public static void ReOpenMainForm()
        {
            Program.DoReOpen = true;
            Form f = Program.MainForm();
            f.Close();
        }
        public static void ForceReopen()
        {
            Program.ROM.ClearModifiedFlag();
            ReOpenMainForm();
            Program.LoadROM(Program.ROM.Filename, "");
        }

        public static void NeedSystemErrorCheck()
        {
            Program.DoReOpen = true;
            Form f = Program.MainForm();
            f.Close();
        }


        static void SaveWithLint(Form self)
        {
            ToolFELintForm f = (ToolFELintForm)InputFormRef.JumpForm<ToolFELintForm>();
            bool IsOnceRejected = false;
            f.OnErrorEventHandler += (sender, arg) =>
            {
                IsOnceRejected = true;
            };
            f.OnNoErrorEventHandler += (sender, arg) =>
            {
                if (IsOnceRejected)
                {
                    DialogResult dr = R.ShowYesNo("エラーがなくなりました。\r\nファイルを保存しますか？");
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    //エラーがないのでウィンドウを閉じる
                    f.Close();
                    SaveForce(self);
                }
                else
                {
                    //エラーがないのでウィンドウを閉じる
                    f.Close();
                    //保存する.
                    Save(self);
                }
            };
        }

        static void SaveWithLintInErrorDialog(Form self)
        {
            ToolFELintForm f = (ToolFELintForm)InputFormRef.JumpForm<ToolFELintForm>();
            bool IsOnceRejected = false;
            f.OnErrorEventHandler += (sender, arg) =>
            {
                OverraideCheckWithErrorDialog dialog = (OverraideCheckWithErrorDialog)InputFormRef.JumpFormLow<OverraideCheckWithErrorDialog>();
                DialogResult r = dialog.ShowDialog();
                if (r == System.Windows.Forms.DialogResult.Retry)
                {//エラーを表示する
                    IsOnceRejected = true;
                }
                else if (r == System.Windows.Forms.DialogResult.Yes)
                {//保存する
                    f.Close();
                    //保存する.
                    SaveForce(self);
                }
                else
                {//保存しない
                    f.Close();
                }
            };
            f.OnNoErrorEventHandler += (sender, arg) =>
            {
                if (IsOnceRejected)
                {
                    DialogResult dr = R.ShowYesNo("エラーがなくなりました。\r\nファイルを保存しますか？");
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    //エラーがないのでウィンドウを閉じる
                    f.Close();
                    SaveForce(self);
                }
                else
                {
                    //エラーがないのでウィンドウを閉じる
                    f.Close();
                    //保存する.
                    Save(self);
                }
            };
        }

        public static void SaveOverraide(Form self, bool forceCheck = false)
        {
            if (OptionForm.overraide_simple_error_check() == OptionForm.overraide_simple_error_check_enum.None)
            {
                if (forceCheck == false)
                {
                    Save(self);
                    return;
                }
            }

            AsmMapFileAsmCache.HasError_Enum hasError = Program.AsmMapFileAsmCache.HasError();
            if (hasError == AsmMapFileAsmCache.HasError_Enum.NOT_PREP)
            {//まだ準備できていない!
                SaveWithLintInErrorDialog(self);
                return;
            }
            if (hasError == AsmMapFileAsmCache.HasError_Enum.HAS_ERROR)
            {//エラーがある場合
                OverraideCheckWithErrorDialog dialog = (OverraideCheckWithErrorDialog)InputFormRef.JumpFormLow<OverraideCheckWithErrorDialog>();
                DialogResult r = dialog.ShowDialog();
                if (r == System.Windows.Forms.DialogResult.Retry)
                {//エラーを表示する
                    SaveWithLint(self);
                }
                else if (r == System.Windows.Forms.DialogResult.Yes)
                {//保存する
                    SaveForce(self);
                }
                else
                {//保存しない
                }
                return;
            }

            //エラーはないので保存する.
            Save(self);
        }

        static void Save(Form self)
        {
            if (Program.ROM.IsVirtualROM)
            {
                SaveAs(self);
                return;
            }

            string text = R._("現在の内容を以下のファイルに書き込んでもよろしいですか？") + "\r\n"+ Program.ROM.Filename;
            string title = R._("上書き確認");

            DialogResult r =
                MessageBox.Show(text
                    , title
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Exclamation);
            if (r != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            SaveForce(self);
        }

        static void RunAutoBackup7ZWithThread(string backupFullPath)
        {
            try
            {
                string backupFullPath7Z = backupFullPath + ".7z";
                string errormessage = ArchSevenZip.Compress(backupFullPath7Z, backupFullPath, 1024);
                if (errormessage.Length > 0)
                {
                    R.Error("バックアップ7zファイル生成に失敗しました。{0} ", errormessage);
                }
                else
                {//正しく 7zに圧縮できたら、元ファイルを消す.
                    File.Delete(backupFullPath);
                }
            }
            catch (Exception e)
            {
                R.Error("バックアップ7zファイル生成に失敗しました。{0} ", e.ToString());
            }
        }
        //自動バックアップ.
        static void RunAutoBackup()
        {
            if (!Program.ROM.Modified)
            {//もし、変更されていないならバックアップは無駄なのでとらない.
                return;
            }
            if (OptionForm.auto_backup() == OptionForm.auto_backup_enum.NO)
            {//設定で自動バックアップを取らないとしている場合
                return;
            }
            if (Program.ROM.IsVirtualROM)
            {//仮想ROMなのでバックアップの取りようがない
                return;
            }
            if (Program.IsCommandLine)
            {//コマンドラインでの実行なのでバックアップは不要です
                return;
            }

            string backup = "backup." + DateTime.Now.ToString("yyyyMMddHHmmss");
            string backupFullPath = U.MakeFilename(backup);
            try
            {
                File.Copy(Program.ROM.Filename, backupFullPath);

                if (OptionForm.auto_backup() == OptionForm.auto_backup_enum.YES_7Z)
                {//7Z圧縮　圧縮は時間がかかるのでスレッドを作る.

                    //スレッドの中でカレントを変更すると失敗するときがあるようなので、外で変更してしまおう.
                    Directory.SetCurrentDirectory(Path.GetDirectoryName(backupFullPath));
                    System.Threading.Thread s1 = new System.Threading.Thread(t =>
                    {
                        RunAutoBackup7ZWithThread(backupFullPath);
                    });
                    s1.Start();
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("バックアップを保存できませんでした。\r\n{0}\r\n{1}", backupFullPath, e.ToString());
                throw;
            }

        }

        public static void SaveForce(Form self)
        {
            if (Program.ROM.IsVirtualROM)
            {
                SaveAs(self);
                return;
            }

            try
            {
                //上書きする前に自動バックアップ
                RunAutoBackup();
            }
            catch (Exception e)
            {
                R.ShowStopError("バックアップを保存できませんでした。\r\n危険なのでセーブ自体をキャンセルします。\r\nセーブをやり直してください。\r\n{0}",  e.ToString());
                return;
            }

            try
            {
                //保存
                Program.ROM.Save(Program.ROM.Filename, false);
            }
            catch (Exception e)
            {
                R.ShowStopError("ROMを保存できませんでした。\r\n{0}\r\n{1}",Program.ROM.Filename, e.ToString());
                return;
            }

            //更新通知
            Program.UpdateWatcher.UpdateLastModified(File.GetLastWriteTime(Program.ROM.Filename));
            //etcファイルの保存
            Program.CommentCache.Save(Program.ROM.Filename);
            Program.FlagCache.Save(Program.ROM.Filename);
            Program.LintCache.Save(Program.ROM.Filename);
            Program.UseTextIDCache.Save(Program.ROM.Filename);
        }

        public static void SaveAs(Form self)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }

            Program.ROM.Save(save.FileName, false);
            Program.UpdateWatcher.UpdateLastModified(File.GetLastWriteTime(save.FileName));

            Program.LastSelectedFilename.Save(self, "", save);
            Program.CommentCache.Save(save.FileName);
            Program.FlagCache.Save(save.FileName);
            Program.LintCache.Save(save.FileName);
            Program.UseTextIDCache.Save(Program.ROM.Filename);
        }

        public static void Quit(Form self)
        {
            if (IsNotSaveYet(self))
            {
                MainFormUtil.ClearModifiedFlag();
                self.Close();
            }
        }

        public static void ClearModifiedFlag()
        {
            if (Program.ROM == null)
            {
                return ;
            }
            Program.ROM.ClearModifiedFlag();
        }

        public static bool IsNotSaveYet(Form self)
        {
            if (Program.ROM == null)
            {
                return true;
            }
            if (!Program.ROM.Modified)
            {
                return true;
            }

            //セーブしないで終わろうとすると警告
            string text = R._("警告\r\n変更した内容をROMに保存していませんが、終了してもよろしいですか？");
            string title = R._("保存確認");
            DialogResult r =
                MessageBox.Show(text
                    , title
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Warning
                    , MessageBoxDefaultButton.Button2);
            if (r == System.Windows.Forms.DialogResult.Yes)
            {
                return true;
            }
            return false;
        }

        public static void RunAs(Form self)
        {
            string title = R._("ROMを開く外部プログラムを選択してください");
            string filter = R._("EXE|*.exe|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(self, "", open);
            open.ShowDialog();
            if (!U.CanReadFileRetry(open))
            {
                return;
            }
            Program.LastSelectedFilename.Save(self, "", open);
            string exe = open.FileNames[0];

            string tempfilename;
            try
            {
                tempfilename = U.WriteTempROM("another");
            }
            catch (Exception e)
            {
                R.ShowStopError(R._("プロセスを起動するための作業用ROMの作成に失敗しました。\r\n返されたエラー\r\n{0}"), e.ToString());
                return;
            }
            string args = U.escape_shell_args(tempfilename);

            Process p;
            try
            {
                p = ProgramRunAs(exe, args);
            }
            catch (Exception e)
            {
                R.ShowStopError(R._("プロセスを実行できませんでした。\r\nプロセス:{0}\r\n引数:{1}\r\n返されたエラー:\r\n{2}"), exe, args, e.ToString());
                return ;
            }

            Program.UpdateWatcher.RegistOtherProcess(p, tempfilename);

        }

        public static Process RunAs(string run_name, string arg1 = "<<ROM>>", string arg2 = "")
        {
            if (run_name == "binary_editor")
            {
                if (Program.Config.at(run_name, "") == "")
                {//バイナリエディタが未定義の場合内臓バイナリエディタを起動する.
                    HexEditorForm hexeditor = (HexEditorForm)InputFormRef.JumpForm<HexEditorForm>(U.NOT_FOUND);
                    hexeditor.JumpTo(0);
                    return null;
                }
            }
            //既に起動している?
            if (IsAlreadyRunningProcess(run_name, arg1))
            {
                DialogResult dr = R.ShowNoYes("既に({0})のプロセスを実行しています。\r\n二重実行になってしまいますが、続行してもよろしいですか？", run_name);
                if (dr != DialogResult.Yes)
                {
                    SetForcusProcess(run_name, arg1);
                    return null;
                }
            }

            if (InputFormRef.IsPleaseWaitDialog(null))
            {//2重割り込み禁止
                return null;
            }
            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait())
            {
                wait.DoEvents(R._("プロセスを起動中:{0}", run_name));

                string emulator = Program.Config.at(run_name, "");
                if (emulator == "" || !File.Exists(emulator))
                {
                    R.ShowStopError("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "emulator");
                    return null;
                }

                string tempfilename;
                if (arg1 == "<<ROM>>")
                {
                    try
                    {
                        tempfilename = U.WriteTempROM(run_name);
                    }
                    catch (Exception e)
                    {
                        R.ShowStopError(R._("ファイルに書き込めませんでした。\r\nファイルが他アプリケーションで利用中の可能性があります。\r\n{0}"), e.ToString());
                        return null;
                    }

                    //no$gba debuggerの場合は、
                    if (isNoDollGBADebugger(emulator))
                    {
                        try
                        {//セーブファイルを転送してあげる.
                            CopyToNoDollGBADebugger(emulator, tempfilename);
                        }
                        catch(Exception e)
                        {
                            R.ShowStopError(R._("no$gba debugerにsavファイルを転送できませんでした。\r\nファイルが他アプリケーションで利用中の可能性があります。\r\n{0}"), e.ToString());
                        }

                        try
                        {//シンボルを生成してあげよう.
                            MakeSymNoDollGBA(tempfilename, wait);
                        }
                        catch (Exception e)
                        {
                            R.ShowStopError(R._("no$gba debuger用のsymbolファイルを転送できませんでした。\r\nファイルが他アプリケーションで利用中の可能性があります。\r\n{0}"), e.ToString());
                        }
                    }
                }
                else
                {
                    tempfilename = arg1;
                }

                string s_tempfilename = U.escape_shell_args(tempfilename);
                string args = s_tempfilename;

                string emulator_arg = Program.Config.at(run_name + "_arg", "");
                if (emulator_arg != "")
                {
                    args = string.Format(emulator_arg, s_tempfilename);
                }
                if (arg2 != "")
                {
                    args = arg2 + " " + args;
                }

                Process p;
                try
                {
                    p = ProgramRunAs(emulator, args);
                }
                catch (Exception e)
                {
                    R.ShowStopError(R._("プロセスを実行できませんでした。\r\nプロセス:{0}\r\n引数:{1}\r\n返されたエラー:\r\n{2}"), emulator, args, e.ToString());
                    return null;
                }

                if (arg1 == "<<ROM>>")
                {
                    Program.UpdateWatcher.RegistOtherProcess(p, tempfilename);
                    AutoConnectEmulator(run_name, p);

                    Program.Undo.SetF5();
                }
                else if (run_name == "emulator")
                {
                    Program.UpdateWatcher.RegistOtherProcess(p, tempfilename);
                    AutoConnectEmulator(run_name, p);
                }

                return p;
            }
        }

        //自動的にエミュレータへ接続ツールを起動する場合.
        static void AutoConnectEmulator(string run_name, Process p)
        {
            if (run_name != "emulator")
            {//エミュレータを動作させたわけではない.
                return;
            }
            if (OptionForm.auto_connect_emulator_enum.None == OptionForm.auto_connect_emulator())
            {//自動的に接続しない.
                return;
            }
            if (Program.ROM.RomInfo.version() == 0)
            {//未知のバージョンなので接続できない
                return;
            }
            //自動接続する.
            EmulatorMemoryForm f = (EmulatorMemoryForm)InputFormRef.JumpFormLow<EmulatorMemoryForm>();
            f.Show();

            //エミュレータが起動しているなら、それにフォーカスを移す
            U.SetFocusByProcess(p);
        }

        public static Process ProgramRunAs(string appPath, string args, int waitMainwindowMiriSec = 60000)
        {
            //see
            //http://www.slotware.net/blog/2009/11/processstart.html

            Process p = new Process();
            p.StartInfo.FileName = appPath;
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            p.Start();

            p.WaitForInputIdle(waitMainwindowMiriSec);

            if (waitMainwindowMiriSec <= 0)
            {
                return p;
            }
            //ウィンドウハンドルが取得できるか、
            //生成したプロセスが終了するまで待ってみる。
            int waitLoopMiriSec = 0;
            while (true)
            {
                if (p.HasExited == true)
                {//プロセスは既に終了した
                    break;
                }
                if (p.MainWindowHandle != IntPtr.Zero)
                {//メインハンドルが作られた
                    break;
                }

                System.Threading.Thread.Sleep(1);
                waitLoopMiriSec += 1;
                if (waitLoopMiriSec > waitMainwindowMiriSec)
                {//タイムオーバー
                    break;
                }
            }
            return p;
        }

        public static string ProgramRunAsAndEndWait(string appPath, string args ,string current_dir="")
        {
            StringBuilder sb = new StringBuilder();

            Process p = new Process();
            p.StartInfo.FileName = appPath;
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            if (current_dir != "")
            {//カレントディレクトリの設定.
                p.StartInfo.FileName = appPath;
                p.StartInfo.WorkingDirectory = current_dir;
            }
            p.StartInfo.RedirectStandardOutput = true; // リダイレクト
            p.StartInfo.RedirectStandardError = true;  // 
            p.OutputDataReceived += (sender, e) =>
            {
                sb.AppendLine(e.Data);
            };
            p.ErrorDataReceived += (sender, e) =>
            {
                sb.AppendLine(e.Data);
            };

            p.Start();

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            p.WaitForExit();
            p.Close();

            return sb.ToString();
        }
        static void KillProcessIfRunning(string run_name, string arg1 = "<<ROM>>")
        {
            string emulator = Program.Config.at(run_name, "");
            if (emulator == "" || !File.Exists(emulator))
            {
                return ;
            }

            string tempfilename;
            if (arg1 == "<<ROM>>")
            {
                tempfilename = U.MakeFilename(run_name);
            }
            else
            {
                tempfilename = arg1;
            }

            //強制終了
            Program.UpdateWatcher.KillProcess(tempfilename);
        }

        static bool IsAlreadyRunningProcess(string run_name, string arg1 = "<<ROM>>")
        {
            string emulator = Program.Config.at(run_name, "");
            if (emulator == "" || !File.Exists(emulator))
            {
                return false;
            }

            string tempfilename;
            if (arg1 == "<<ROM>>")
            {
                tempfilename = U.MakeFilename(run_name);
            }
            else
            {
                tempfilename = arg1;
            }

            //既に実行しているか？
            Process p = Program.UpdateWatcher.GetRunning(tempfilename);
            if (p == null)
            {//未実行
                return false;
            }
            //実行済み
            return true;
        }

        static void SetForcusProcess(string run_name, string arg1 = "<<ROM>>")
        {
            string emulator = Program.Config.at(run_name, "");
            if (emulator == "" || !File.Exists(emulator))
            {
                return ;
            }

            string tempfilename;
            if (arg1 == "<<ROM>>")
            {
                tempfilename = U.MakeFilename(run_name);
            }
            else
            {
                tempfilename = arg1;
            }

            //既に実行しているなら、そのプロセスのメインウィンドウにフォーカスを当てる
            Process p = Program.UpdateWatcher.GetRunning(tempfilename);
            U.SetFocusByProcess(p);
        }

        public static Process PoolRunAs(string run_name, string arg1 = "<<ROM>>", string arg2 = "")
        {
            string emulator = Program.Config.at(run_name, "");
            if (emulator == "" || !File.Exists(emulator))
            {
                R.ShowStopError("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "emulator");
                return null;
            }

            string tempfilename;
            if (arg1 == "<<ROM>>")
            {
                tempfilename = U.MakeFilename(run_name);
            }
            else
            {
                tempfilename = arg1;
            }
            DateTime last_write_time = File.GetLastWriteTime(tempfilename);
            if (Program.Undo.getLastModify() > last_write_time)
            {//渡したときより更新していたら、再度実行しないとダメだ
                return RunAs(run_name, arg1);
            }

            //既に実行しているか？
            Process p = Program.UpdateWatcher.GetRunning(tempfilename);
            if (p == null)
            {//未実行なので実行する
                return RunAs(run_name, arg1 , arg2);
            }
            return p;
        }
        static bool CheckVGMusicStudio(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            return (name == "VG Music Studio");
        }
        
        public static void RunAsSappy(uint song_id)
        {
            if (song_id <= 0)
            {
                return;
            }
            if (song_id >= 0xFFFF)
            {
                return;
            }


            string sappyexe = Program.Config.at("sappy", "");
            if (sappyexe == "" || !File.Exists(sappyexe))
            {
                R.ShowStopError("Sappyが存在しません。\r\n設定->オプションから正しいパスを設定してください。");
                return;
            }
            bool isVGMusicStudio = CheckVGMusicStudio(sappyexe);

            if (isVGMusicStudio)
            {
                KillProcessIfRunning("sappy", "<<ROM>>");

                try
                {
                    string args2 = String.Format("-mp2k -songid {0} -filename", song_id);
                    MainFormUtil.RunAs("sappy", "<<ROM>>", args2);
                }
                catch (Exception e)
                {
                    R.ShowStopError("VGMusicStudioプロセスを実行できません。\r\n{0}", e.ToString());
                    return;
                }
            }
            else
            {
                Process p;
                try
                {
                    p = MainFormUtil.PoolRunAs("sappy");
                }
                catch (Exception e)
                {
                    R.ShowStopError("Sappyプロセスを実行できません。\r\n{0}", e.ToString());
                    return;
                }
                if (p == null)
                {
                    return;
                }

                SappyPlaying sappy = new SappyPlaying();
                sappy.StartPlay(p, song_id);
            }
        }


        //Devkit pro Enbiで対象物をコンパイル
        public static bool CompilerDevkitPro(string target_filename, out string output, out string out_symbol, CompileType compileType)
        {
            output = "";
            out_symbol = "";

            string devkitpro_eabi = Program.Config.at("devkitpro_eabi", "");
            if (devkitpro_eabi == "" || !File.Exists(devkitpro_eabi))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。"
                    , "devkitpro_eabi");
                return false;
            }

            string tooldir = Path.GetDirectoryName(devkitpro_eabi);


            string target = Path.Combine(Path.GetDirectoryName(target_filename), Path.GetFileNameWithoutExtension(target_filename));
            string ext = U.GetFilenameExt(target_filename);
            string compiler;
            if (ext == ".C")
            {
                compiler = "*gcc.exe";
            }
            else if (ext == ".CPP")
            {
                compiler = "*g++.exe";
            }
            else
            {//不明の場合、とりあえずアセンブラ
                compiler = "*as.exe";
            }

            string compiler_exe = U.FindFileOne(tooldir, compiler);
            if (compiler_exe == "")
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。"
                    , "devkitpro_eabi " + compiler);
                return false;
            }
            if (compileType == CompileType.CONVERT_LYN)
            {//LYNに変換する場合、ELFを作るのとは別の処理になる.
                return ConvertLYN(target_filename, out output, out out_symbol);
            }

            string output_temp_filename = target + ".elf";
            string target_filedir = Path.GetDirectoryName(target_filename);

            //Assemble into an elf
            string args = "-g -mcpu=arm7tdmi -mthumb-interwork "
                + " " + U.escape_shell_args(target_filename)
                + " -o " + U.escape_shell_args(output_temp_filename)
                ;

            //add custom compiler option
            if (ext == ".C" || ext == ".CPP")
            {
                args += " " + OptionForm.GetCFLAGS();
            }
            string FEClib = OptionForm.GetFECLIB();
            if (File.Exists(FEClib))
            {
                string FEClibDir = Path.GetDirectoryName(FEClib);
                args += " -I " + U.escape_shell_args(FEClibDir);
            }

            output = ProgramRunAsAndEndWait(compiler_exe, args, target_filedir);
            if (!File.Exists(output_temp_filename) || U.GetFileSize(output_temp_filename) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            Elf elf = new Elf(output_temp_filename , useHookMode: false);
            string lostLabelErrorMessage = elf.CheckLostLabel();
            if (lostLabelErrorMessage != "")
            {
                output = lostLabelErrorMessage;
                return false;
            }
            out_symbol = elf.ToEASymbol();

            //Extract raw assembly binary (text section) from elf
            output_temp_filename = target + ".dmp";
            U.WriteAllBytes(output_temp_filename, elf.ProgramBIN);

            Log.Notify(target + ".dmp");
            Log.Notify("=== SYMBOL ===");
            Log.Notify(out_symbol);

            output = output_temp_filename;
            if (compileType != CompileType.KEEP_ELF)
            {
                File.Delete(target + ".elf");
            }
            return true;
        }

        static bool ConvertLYN(string target_filename, out string output, out string out_symbol)
        {
            out_symbol = "";

            bool r;
            r = ConvertLYN_S_to_O(target_filename, out output);
            if (r == false || !File.Exists(output))
            {
                return false;
            }

            string obj_filename = output;
            r = ConvertLYN_O_to_Event(obj_filename, out output);

            Elf elf = new Elf(obj_filename, useHookMode: false);
            out_symbol = elf.ToEASymbol();
            
            File.Delete(obj_filename);
            if (r == false || !File.Exists(output))
            {
                return false;
            }

            return true;
        }

        static bool ConvertLYN_S_to_O(string target_filename, out string output)
        {
            string devkitpro_eabi = Program.Config.at("devkitpro_eabi", "");
            string tooldir = Path.GetDirectoryName(devkitpro_eabi);
            string target = Path.Combine(Path.GetDirectoryName(target_filename), Path.GetFileNameWithoutExtension(target_filename));
            string compiler = "*as.exe";
            string compiler_exe = U.FindFileOne(tooldir, compiler);
            if (compiler_exe == "")
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。"
                    , "devkitpro_eabi " + compiler);
                return false;
            }
            string output_temp_filename = U.ChangeExtFilename(target_filename, ".o");

            string args = "-g -mcpu=arm7tdmi -mthumb-interwork "
                + " " + U.escape_shell_args(target_filename);

            string reference = GetFEClibReference();
            if (File.Exists(reference))
            {//FEClibがあればコンパイルオプションに追加
                args += " " + U.escape_shell_args(reference);
            }
            args += " -o " + U.escape_shell_args(output_temp_filename);

            string target_filedir = Path.GetDirectoryName(target_filename);
            output = ProgramRunAsAndEndWait(compiler_exe, args, target_filedir);
            if (!File.Exists(output_temp_filename) || U.GetFileSize(output_temp_filename) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            output = output_temp_filename;
            return true;
        }
        static bool ConvertLYN_O_to_Event(string target_filename, out string output)
        {
            string EACoreEXE = Program.Config.at("event_assembler", "");
            string lynEXE = Path.Combine(Path.GetDirectoryName(EACoreEXE), "Tools/lyn.exe");
            if (!File.Exists(lynEXE))
            {
                output = R.Error("lyn.exeが見つかりません。\r\n{0}",lynEXE);
                return false;
            }

            string args = U.escape_shell_args(target_filename);
            string target_filedir = Path.GetDirectoryName(target_filename);
            output = ProgramRunAsAndEndWait(lynEXE, args, target_filedir);
            if (output.IndexOf("ALIGN 4") < 0)
            {
                output = lynEXE + " " + args + " \r\noutput:\r\n" + output;

                string reference = GetFEClibReference();
                if (!File.Exists(reference))
                {
                    output += R.Error("FEClib Referenceが見つかりません。");
                }
                return false;
            }

            string output_temp_filename = U.ChangeExtFilename(target_filename, ".lyn.event");
            File.WriteAllText(output_temp_filename,output);

            output = output_temp_filename;
            return true;
        }

        static string GetFEClibReference()
        {
            string FEClib = OptionForm.GetFECLIB();
            string dir = Path.GetDirectoryName(FEClib);
            dir = Path.Combine(dir, "../reference/");

            if (!Directory.Exists(dir))
            {
                return "";
            }

            string filename = Program.ROM.RomInfo.VersionToFilename() + "*.s";
            string[] list = U.Directory_GetFiles_Safe(dir, filename, SearchOption.TopDirectoryOnly);

            if (list.Length <= 0)
            {
                return "";
            }

            Array.Sort(list);
            Array.Reverse(list);

            return list[0];
        }
        public static bool IsColorzCore(string core_path)
        {
            string filename = Path.GetFileName(core_path);
            return filename == "ColorzCore.exe";
        }

        static string CompilerEventAssemblerInner(string compiler_exe ,string tooldir,string  freeareadef_targetfile_fullpath,string  output_target_rom,string  output_symFile)
        {
            bool isColorzCore = IsColorzCore(compiler_exe);

            string args = "A "
                + Program.ROM.RomInfo.TitleToFilename() + " "
                + U.escape_shell_args("-input:" + freeareadef_targetfile_fullpath) + " "
                + U.escape_shell_args("-output:" + output_target_rom) + " ";
            if (isColorzCore)
            {
                args += U.escape_shell_args("--nocash-sym:" + output_symFile);
            }
            else
            {
                args += U.escape_shell_args("-symOutput:" + output_symFile);
            }

            Log.Notify(args);
            string output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);
            Log.Notify("=== OUTPUT ===");
            Log.Notify(output);
            if (!IsCompilerErrorByEventAssembler(output))
            {//エラーではない
                return output;
            }

            //エラーなので詳細を追加する.
            output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;

            if (output.IndexOf("Tool lyn not found.") >= 0)
            {
                output = R.Error("このパッチを使うには、lynが必要です。EAのToolsフォルダにlynをインストールしてください。以下のマニュアルを参考にしてください。\r\n{0}\r\n\r\n詳細エラーメッセージ:\r\n{1}", GetLynProgramURL(), output);
                return output;
            }

            if (output.IndexOf("symOutput doesn't exist.") >= 0
                || output.IndexOf("Unrecognized flag: symOutput") >= 0)
            {//古いEAらしいので、symOutputを外して実行する
                args = "A "
                    + Program.ROM.RomInfo.TitleToFilename() + " "
                    + U.escape_shell_args("-input:" + freeareadef_targetfile_fullpath) + " "
                    + U.escape_shell_args("-output:" + output_target_rom);
                Log.Notify(args);
                Log.Notify("=== OUTPUT ===");
                Log.Notify(output);
                output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);
                if (!IsCompilerErrorByEventAssembler(output))
                {//エラーではない
                    return output;
                }

                //エラーなので詳細を追加する.
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
            }

            return output;
        }
        public static bool IsCompilerErrorByEventAssembler(string output)
        {
            if (output.IndexOf("No errors or warnings.") >= 0)
            {
                return false;
            }
            if (output.IndexOf("No errors. Please continue being awesome.") >= 0)
            {
                return false;
            }
            return true;
        }


        //EventAssemblerで対象物をコンパイル
        public static bool CompilerEventAssembler(string target_filename, uint freearea,uint org_sp, out string output, out string out_symbol)
        {
            output = "";
            out_symbol = "";

            string compiler_exe = Program.Config.at("event_assembler", "");
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "event_assembler core.exe");
                return false;
            }
            bool isColorzCore = MainFormUtil.IsColorzCore(compiler_exe);

            string autoDef = EAUtil.MakeEAAutoDef(target_filename, freearea, org_sp, isColorzCore);

            string freeareadef_targetfile = "_FBG_Temp_" +　DateTime.Now.Ticks.ToString() + ".event";
            string freeareadef_targetfile_fullpath = Path.Combine(Path.GetDirectoryName(target_filename), freeareadef_targetfile);
            freeareadef_targetfile_fullpath = Path.GetFullPath(freeareadef_targetfile_fullpath);

            U.WriteAllText(freeareadef_targetfile_fullpath, autoDef);
            Log.Notify("_FBG_Temp_");
            Log.Notify(autoDef);
            Log.Notify("-----------------");

            if (!File.Exists(freeareadef_targetfile_fullpath))
            {
                output = R._("ファイルに書き込めませんでした。\r\nディレクトリに、書き込みできるようになっているか確認してください。\r\n{0}", freeareadef_targetfile_fullpath);
                return false;
            }

            string output_symFile = Path.GetTempFileName();

            string output_target_rom;
            try
            {
                output_target_rom = U.WriteTempROM("event_assembler");
            }
            catch (Exception e)
            {
                output = R._("ファイルに書き込めませんでした。\r\nファイルが他アプリケーションで利用中の可能性があります。\r\n{0}",e.ToString());
                return false;
            }

            string tooldir = Path.GetDirectoryName(compiler_exe);

            //A FE8 "-input:EA.txt" "-output:temp.gba"
            output = CompilerEventAssemblerInner(compiler_exe ,tooldir, freeareadef_targetfile_fullpath, output_target_rom, output_symFile);
            if (IsCompilerErrorByEventAssembler(output))
            {
                File.Delete(output_target_rom);
                File.Delete(freeareadef_targetfile_fullpath);
                File.Delete(output_symFile);
                return false;
            }

            if (File.Exists(output_symFile))
            {
                Log.Notify("=== SYMBOL ===");
                out_symbol = File.ReadAllText(output_symFile);
                Log.Notify(out_symbol);
                File.Delete(output_symFile);
            }

            File.Delete(freeareadef_targetfile_fullpath);
            output = output_target_rom;
            return true;
        }

        //GoldRoadで対象物をコンパイル
        public static bool CompilerGoldRoad(string target_filename, out string output, out string out_symbol)
        {
            output = "";
            out_symbol = "";

            string goldroad = Program.Config.at("goldroad_asm", "");
            if (goldroad == "" || !File.Exists(goldroad))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "goldroad");
                return false;
            }

            string targetNotExt = Path.GetFileNameWithoutExtension(target_filename);
            string targetdir = Path.GetDirectoryName(target_filename);
            string target = Path.Combine(targetdir, targetNotExt);
            string tooldir = Path.GetDirectoryName(goldroad);
            string output_temp_filename = targetNotExt + ".gba";
            
            //Assemble into an gba
            string args = U.escape_shell_args(target_filename)
            +" " + U.escape_shell_args(output_temp_filename);

            //goldroadはバグっていて複雑な出力先をいれると落ちるため、ファイル名だけを渡しし、toolカレントに出力させる.
            output = ProgramRunAsAndEndWait(goldroad, args, tooldir);

            output_temp_filename = Path.Combine(tooldir, targetNotExt + ".gba");
            if (File.Exists(output_temp_filename) == false 
             || U.GetFileSize(output_temp_filename) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = goldroad + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            //正規の場所に移動 (拡張子 gbaは怖いので binにする)
            output = target + ".bin";
            if (File.Exists(output))
            {//先客がいると移動できないので消す.
                File.Delete(output);
            }
            File.Move(output_temp_filename, output);
            return true;
        }

        public enum CompileType
        {
             NONE
            ,KEEP_ELF
            ,CONVERT_LYN
        }

        //対象物をコンパイル
        public static bool Compile(string target_filename, out string output, out string out_symbol, CompileType compileType)
        {
            string ext = U.GetFilenameExt(target_filename);
            if (ext == ".ASM")
            {
                string text = File.ReadAllText(target_filename);
                text = text.ToLower();
                if (text.IndexOf(".thumb") >= 0)
                {
                }
                else if (text.IndexOf("@thumb") >= 0)
                {
                    return CompilerGoldRoad(target_filename, out output, out out_symbol);
                }
            }
            return CompilerDevkitPro(target_filename, out output, out out_symbol, compileType);
        }
        //無改造ROMを探索する CRCと言語で探索
        public static string FindOrignalROM(string current_dir)
        {
            uint crc32 = Program.ROM.RomInfo.orignal_crc32();
            string orignalFilename = MainFormUtil.FindOrignalROMByCRC32(current_dir, crc32);
            if (orignalFilename == "")
            {
                string lang = OptionForm.lang();
                orignalFilename = MainFormUtil.FindOrignalROMByLang(current_dir, lang);
            }
            return orignalFilename;
        }

        //無改造ROMを探索する 言語で探索
        public static string FindOrignalROMByLang(string current_dir, string lang)
        {
            List<string> findDirList = new List<string>();
            string orignal_romfile = MainFormUtil.FindOrignalROMLow(current_dir, lang);
            findDirList.Add(orignal_romfile);

            if (orignal_romfile == "")
            {//見つからない場合、FEBuilderGBAのカレントも探そう.
                string dir = Program.BaseDirectory;
                if (findDirList.IndexOf(dir) < 0)
                {
                    orignal_romfile = MainFormUtil.FindOrignalROMLow(dir, lang);
                    findDirList.Add(dir);
                }
            }

            if (orignal_romfile == "")
            {//見つからない場合、最後に開いたROMのディレクトリも探そう.
                string lastfilename = Program.GetLastROMFilename();
                if (lastfilename != "")
                {
                    string dir = Path.GetDirectoryName(lastfilename);
                    if (findDirList.IndexOf(dir) < 0)
                    {
                        orignal_romfile = MainFormUtil.FindOrignalROMLow(dir, lang);
                        findDirList.Add(dir);
                    }
                }
            }

            if (orignal_romfile == "")
            {//見つからない場合、エミュレータのディレクトリはどうだろう？
                string emulator = Program.Config.at("emulator");
                if (emulator != "")
                {
                    string dir = Path.GetDirectoryName(emulator);
                    if (findDirList.IndexOf(dir) < 0)
                    {
                        orignal_romfile = MainFormUtil.FindOrignalROMLow(dir, lang, SearchOption.AllDirectories);
                        findDirList.Add(dir);
                    }
                }
            }

            return orignal_romfile;
        }


        public struct ROMBaseTableSt
        {
            public string name;
            public uint ver;
            public string lang;
            public string header;
            public uint romsize;
            public uint crc32;
        };
        static ROMBaseTableSt[] GetROMBaseTable()
        {
            return new ROMBaseTableSt[]{
                new ROMBaseTableSt{name = "FE6J" , ver = 6, lang = "ja" , header = "AFEJ01", romsize = 0x800000, crc32 = 0xd38763e1},
                new ROMBaseTableSt{name = "FE6U" , ver = 6, lang = "en" , header = "AFEJ01", romsize = 0x1000000, crc32 =0x35F5B06B},
                new ROMBaseTableSt{name = "FE6CN" , ver = 6, lang = "zh" , header = "AFEJ01", romsize = 0x800000, crc32 = 	0x1F19D989},
                new ROMBaseTableSt{name = "FE7J" , ver = 7, lang = "ja" , header = "AE7J01", romsize = 0x1000000, crc32 = 	0xf0c10e72},
                new ROMBaseTableSt{name = "FE7U" , ver = 7, lang = "en" , header = "AE7E01", romsize = 0x1000000, crc32 = 	0x2a524221},
                new ROMBaseTableSt{name = "FE7CN" , ver = 7, lang = "zh" , header = "AE7J01", romsize = 0x1000000, crc32 = 	0x5F286460},
                new ROMBaseTableSt{name = "FE8J" , ver = 8, lang = "ja" , header = "BE8J01", romsize = 0x1000000, crc32 = 	0x9d76826f},
                new ROMBaseTableSt{name = "FE8U" , ver = 8, lang = "en" , header = "BE8E01", romsize = 0x1000000, crc32 = 	0xa47246ae},
                new ROMBaseTableSt{name = "FE8CN" , ver = 8, lang = "zh" , header = "BE8J01", romsize = 0x1000000, crc32 = 	0x79609D14},
            };
        }

        //現在のディレクトリにある未改造ROMの探索
        static string FindOrignalROMLow(string dir, string lang, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (dir == "" || !Directory.Exists(dir))
            {
                return "";
            }
            if (lang == "zh-CH")
            {
                lang = "zh";
            }

            int version;
            if (Program.ROM == null)
            {
                version = 0; //不明
            }
            else
            {
                version = Program.ROM.RomInfo.version();
            }

            string orignal_header = "";
            uint orignal_size = 0;
            uint orignal_crc32 = 0;
            
            ROMBaseTableSt[] table = GetROMBaseTable();
            foreach(ROMBaseTableSt t in table)
            {
                //FE6U	ver	en	AFEJ01	0x1000000	0x35F5B06B
                if (version != 0)
                {
                    if (version != t.ver)
                    {
                        continue;
                    }
                }
                if (lang != "")
                {
                    if (lang != t.lang)
                    {
                        continue;
                    }
                }

                orignal_header = t.header;
                orignal_size = t.romsize;
                orignal_crc32 = t.crc32;
            }

            if (orignal_size == 0 || orignal_crc32 == 0)
            {//不明
                return "";
            }

            U.CRC32 crc32 = new U.CRC32();

            string[] files = U.Directory_GetFiles_Safe(dir, "*.gba", searchOption);
            for (int i = 0; i < files.Length; i++)
            {
                string filename = Path.GetFileName(files[i]);
                if (filename.IndexOf(".backup.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".emulator.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".emulator2.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".sappy.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".binary_editor.") > 0)
                {
                    continue;
                }

                if (U.GetFileSize(files[i]) != orignal_size)
                {
                    continue;
                }

                byte[] file;
                try
                {
                    file = File.ReadAllBytes(files[i]);
                }
                catch (System.UnauthorizedAccessException e)
                {
                    Log.Error(R.ExceptionToString(e));
                    continue;
                }
                catch (System.IO.IOException e)
                {
                    Log.Error(R.ExceptionToString(e));
                    continue;
                }
                    
                if (U.getASCIIString(file, 0xAC, 6) != orignal_header)
                {
                    continue;
                }

                uint crc = crc32.Calc(file);
                if (crc != orignal_crc32)
                {
                    continue;
                }
                //発見!
                return files[i];
            }

            //ない
            return "";
        }

        //無改造ROMを探索する CRCで探索
        public static string FindOrignalROMByCRC32(string current_dir, uint search_crc)
        {
            List<string> findDirList = new List<string>();
            string orignal_romfile = MainFormUtil.FindOrignalROMLow(current_dir, search_crc);
            findDirList.Add(orignal_romfile);

            if (orignal_romfile == "")
            {//見つからない場合、FEBuilderGBAのカレントも探そう.
                string dir = Program.BaseDirectory;
                if (findDirList.IndexOf(dir) < 0)
                {
                    orignal_romfile = MainFormUtil.FindOrignalROMLow(dir, search_crc);
                    findDirList.Add(dir);
                }
            }

            if (orignal_romfile == "")
            {//見つからない場合、最後に開いたROMのディレクトリも探そう.
                string lastfilename = Program.GetLastROMFilename();
                if (lastfilename != "")
                {
                    string dir = Path.GetDirectoryName(lastfilename);
                    if (findDirList.IndexOf(dir) < 0)
                    {
                        orignal_romfile = MainFormUtil.FindOrignalROMLow(dir, search_crc);
                        findDirList.Add(dir);
                    }
                }
            }

            if (orignal_romfile == "")
            {//見つからない場合、エミュレータのディレクトリはどうだろう？
                string emulator = Program.Config.at("emulator");
                if (emulator != "")
                {
                    string dir = Path.GetDirectoryName(emulator);
                    if (findDirList.IndexOf(dir) < 0)
                    {
                        orignal_romfile = MainFormUtil.FindOrignalROMLow(dir, search_crc, SearchOption.AllDirectories);
                        findDirList.Add(dir);
                    }
                }
            }

            return orignal_romfile;
        }

        //現在のディレクトリにある未改造ROMの探索
        static string FindOrignalROMLow(string dir, uint search_crc, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (dir == "" || !Directory.Exists(dir))
            {
                return "";
            }

            U.CRC32 crc32 = new U.CRC32();

            string[] files = U.Directory_GetFiles_Safe(dir, "*.gba", searchOption);
            for (int i = 0; i < files.Length; i++)
            {
                string filename = Path.GetFileName(files[i]);
                if (filename.IndexOf(".backup.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".emulator.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".emulator2.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".sappy.") > 0)
                {
                    continue;
                }
                if (filename.IndexOf(".binary_editor.") > 0)
                {
                    continue;
                }

                byte[] file;
                try
                {
                    file = File.ReadAllBytes(files[i]);
                }
                catch (System.UnauthorizedAccessException e)
                {
                    Log.Error(R.ExceptionToString(e));
                    continue;
                }
                catch (System.IO.IOException e)
                {
                    Log.Error(R.ExceptionToString(e));
                    continue;
                }

                uint crc = crc32.Calc(file);
                if (crc != search_crc)
                {
                    continue;
                }
                //発見!
                return files[i];
            }

            //ない
            return "";
        }

        static bool isNoDollGBADebugger(string noDollGBAPath)
        {
            string filename = Path.GetFileName(noDollGBAPath).ToUpper();
            return filename == "NO$GBA.EXE";
        }


        public static string FindCurrentSavFilename(string romFilename)
        {
            string nowSaveFile = Path.GetFileNameWithoutExtension(romFilename) + ".sav";
            if (File.Exists(nowSaveFile))
            {
                return nowSaveFile;
            }
            if (romFilename.IndexOf(".emulator.gba") >= 0)
            {
                nowSaveFile = romFilename.Replace(".emulator.gba", ".sav");
                if (File.Exists(nowSaveFile))
                {
                    return nowSaveFile;
                }
            }
            if (romFilename.IndexOf(".emulator2.gba") >= 0)
            {
                nowSaveFile = romFilename.Replace(".emulator2.gba", ".emulator.sav");
                if (File.Exists(nowSaveFile))
                {
                    return nowSaveFile;
                }
                nowSaveFile = romFilename.Replace(".emulator2.gba", ".sav");
                if (File.Exists(nowSaveFile))
                {
                    return nowSaveFile;
                }
            }
            nowSaveFile = U.GetFirstPeriodFilename(romFilename) + ".sav";
            if (File.Exists(nowSaveFile))
            {
                return nowSaveFile;
            }

            //無理
            return "";
        }
        static void MakeSymNoDollGBA(string romFilename, InputFormRef.AutoPleaseWait wait)
        {
            if (OptionForm.create_nodoll_gba_sym() == OptionForm.create_nodoll_gba_sym_enum.None)
            {
                return;
            }

            string nameSYM = U.GetFirstPeriodFilename(romFilename) + ".SYM";
            nameSYM = Path.Combine(Path.GetDirectoryName(romFilename), nameSYM);

            if (File.Exists(nameSYM))
            {
                if (Program.Undo.getLastModify() < File.GetLastWriteTime(nameSYM))
                {
                    return;
                }
            }
            DisASMDumpAllForm.AllMakeNoDollSymFile(null, nameSYM , wait);
        }

        //no$gba debuggerは変わっていて、
        //彼がセーブデータを探索するのは、 自身のBATTERYフォルダだけ
        //そして、ファイル名は最初のピリオドまでで切られる. aaa.bbb.ccc.gba -> aaa.sav を探索
        //ここでは、そんなおかしい彼に、第1エミュレータのセーブファイルを正しく引き継がせる。
        static bool CopyToNoDollGBADebugger(string noDollGBAPath, string romFilename)
        {
            Debug.Assert(isNoDollGBADebugger(noDollGBAPath));

            string nowSaveFile = FindCurrentSavFilename(romFilename);
            if (!File.Exists(nowSaveFile))
            {
                return false;
            }

            string nodollGBA_Directory = Path.GetDirectoryName(noDollGBAPath);
            string batteryDirectory = Path.Combine(nodollGBA_Directory, "BATTERY");
            if (!Directory.Exists(batteryDirectory))
            {//無理 バッテリーディレクトリがない.
                return false;
            }

            string nameSAV = U.GetFirstPeriodFilename(romFilename) + ".SAV";
            string nodollSavFilename = Path.Combine(batteryDirectory, nameSAV);

            //no$gbaへセーブファイルをコピーする.
            File.Copy(nowSaveFile, nodollSavFilename , true);
            return true;
        }

        //EventAssemblerでイベントを取り出します
        public static bool DisasembleEventAssembler(uint dumpAddr, string saveEventFile, string endAddr, string eaOption, bool addEndGuards, out string output)
        {
            output = "";

            string compiler_exe = Program.Config.at("event_assembler", "");
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "event_assembler core.exe");
                return false;
            }

            string output_target_rom;
            try
            {
                output_target_rom = U.WriteTempROM("event_assembler");
            }
            catch (Exception e)
            {
                output = R._("ファイルに書き込めませんでした。\r\nファイルが他アプリケーションで利用中の可能性があります。\r\n{0}", e.ToString());
                return false;
            }

            string tooldir = Path.GetDirectoryName(compiler_exe);
            if (eaOption.Length > 0)
            {
                eaOption += " ";
            }

            //Core D FE8 ToEnd 0xA39768 none -input:fe8e.gba "-output:Prologue.event"
            string args = "D "
                + Program.ROM.RomInfo.TitleToFilename() + " "
                + endAddr  + " "
                + U.To0xHexString(dumpAddr) + " "
                + eaOption 
                + U.escape_shell_args("-input:" + output_target_rom) + " "
                + U.escape_shell_args("-output:" + saveEventFile);
            if (addEndGuards)
            {
                args += " -addEndGuards";
            }
            output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);
            if (output.IndexOf("No errors or warnings.") < 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                File.Delete(output_target_rom);
                return false;
            }

            output = output_target_rom;
            return true;
        }

        public static void GotoMoreData()
        {
            U.OpenURLOrFile(GotoMoreDataURL());
        }
        public static string GotoMoreDataURL()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:more_data";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:more_data";
            }
            return url;
        }

        public static void GotoManual()
        {
            U.OpenURLOrFile(GetManualURL());
        }
        public static string GetCommunitiesURL()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=column:discord_chat";
            }
            else if (lang == "zh")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=zh:discord_chat";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:discord_chat";
            }
            return url;
        }
        public static string GetExplainOfSFileURL()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:s_file%E3%82%A4%E3%83%B3%E3%83%9D%E3%83%BC%E3%83%88%E4%B8%AD%E3%81%AE%E3%82%A8%E3%83%A9%E3%83%BC%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6%E3%81%AE%E8%A7%A3%E8%AA%AC";
            }
            else if (lang == "zh")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=zh:guide:s%E6%96%87%E4%BB%B6%E5%AF%BC%E5%85%A5%E6%9C%9F%E9%97%B4%E7%9A%84%E9%94%99%E8%AF%AF%E8%AF%B4%E6%98%8E_zh";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:guide:explanation_of_s_file_en";
            }
            return url;
        }

        public static string GetAboutTragetAI3()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:%E6%A8%99%E7%9A%84ai%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:about_targetai";
            }
            return url;
        }


        public static void GotoReport7zURL()
        {
            U.OpenURLOrFile(GetReport7zURL());
        }
        public static string GetReport7zURL()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:report7z";
            }
            else if (lang == "zh")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=zh:guide:febuildergba:report7z";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:guide:febuildergba:report7z";
            }
            return url;
        }

        public static string GetManualURL()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:index";
            }
            else if (lang == "zh")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=zh:guide:febuildergba:index";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:guide:febuildergba:index";
            }
            return url;
        }

        static string GetLynProgramURL()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:febuildergba%E3%82%92%E5%88%A9%E7%94%A8%E3%81%99%E3%82%8B%E4%B8%8A%E3%81%A7%E5%BF%85%E8%A6%81%E3%81%AB%E3%81%AA%E3%82%8B%E3%83%84%E3%83%BC%E3%83%AB%E3%81%AE%E5%85%A5%E6%89%8B%E6%96%B9%E6%B3%95#lyn";
            }
            else if (lang == "zh")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=zh:guide:febuildergba:febuildergba%E9%9C%80%E8%A6%81%E7%9A%84%E5%B7%A5%E5%85%B7%E5%85%A5%E6%89%8B%E6%96%B9%E6%B3%95_zh#lyn";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:guide:febuildergba:how_to_obtain_necessary_tools_to_use_febuildergba_en#lyn";
            }
            return url;
        }

        public static string GetNecessaryProgramURL()
        {
            string lang = OptionForm.lang();

            string url;
            if (lang == "ja")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=guide:febuildergba:febuildergba%E3%82%92%E5%88%A9%E7%94%A8%E3%81%99%E3%82%8B%E4%B8%8A%E3%81%A7%E5%BF%85%E8%A6%81%E3%81%AB%E3%81%AA%E3%82%8B%E3%83%84%E3%83%BC%E3%83%AB%E3%81%AE%E5%85%A5%E6%89%8B%E6%96%B9%E6%B3%95";
            }
            else if (lang == "zh")
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=zh:guide:febuildergba:febuildergba%E9%9C%80%E8%A6%81%E7%9A%84%E5%B7%A5%E5%85%B7%E5%85%A5%E6%89%8B%E6%96%B9%E6%B3%95_zh";
            }
            else
            {
                url = "https://dw.ngmansion.xyz/doku.php?id=en:guide:febuildergba:how_to_obtain_necessary_tools_to_use_febuildergba_en";
            }
            return url;
        }

        public static void GotoCommunities()
        {
            string url = GetCommunitiesURL();
            U.OpenURLOrFile(url);
        }
        static bool OpenGBA7ZROM(string romfilename, bool useReOpen, string forceversion)
        {
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                ArchSevenZip.Extract(romfilename, tempdir.Dir);
                string[] files = U.Directory_GetFiles_Safe(tempdir.Dir, "*.gba", SearchOption.AllDirectories);
                if (files.Length < 1)
                {
                    //ROMがなければupsを調べてみよう.
                    files = U.Directory_GetFiles_Safe(tempdir.Dir, "*.ups", SearchOption.AllDirectories);
                    if (files.Length < 1)
                    {//upsもない
                        return false;
                    }
                    //UPSがあった
                    ToolUPSOpenSimpleForm ups = new ToolUPSOpenSimpleForm();
                    ups.OpenUPS(files[0], useReOpen, forceversion);
                    ups.ShowDialog();
                    return ups.DialogResult == DialogResult.OK;
                }
                ROM rom = new ROM();
                string version;
                bool r = rom.Load(files[0], out version);
                if (!r)
                {
                    R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                    return false;
                }

                if (useReOpen)
                {//メインフォームを開きなおす場合
                    MainFormUtil.ReOpenMainForm();
                }

                //保存しない場合、メモリ上の存在になる.
                Program.LoadVirtualROM(rom, romfilename);
           }
           return true;
        }

        public static void ApplySearchFilter(string filter, Control controlPanel,PatchMainFilter patchMainFilter, ToolTipEx tooltip)
        {
            Debug.Assert(controlPanel != null);

            if (filter == "")
            {
                for (int i = 0; i < controlPanel.Controls.Count; i++)
                {
                    controlPanel.Controls[i].Show();
                }
                if (patchMainFilter != null)
                {
                    patchMainFilter.ApplyFilter("", false, tooltip);
                }
            }
            else
            {
                string lang = OptionForm.lang();
                bool isJP = (lang == "ja");
                filter = U.CleanupFindString(filter , isJP);

                for (int i = 0; i < controlPanel.Controls.Count; i++)
                {
                    Control c = controlPanel.Controls[i];
                    if (U.StrStrEx(c.Text, filter, isJP))
                    {
                        c.Show();
                        continue;
                    }
                    if (U.StrStrEx(c.Name, filter, isJP))
                    {
                        c.Show();
                        continue;
                    }
                    if (U.StrStrEx(tooltip.GetToolTip(c), filter, isJP))
                    {
                        c.Show();
                        continue;
                    }
                    c.Hide();
                }
                if (patchMainFilter != null)
                {
                    patchMainFilter.ApplyFilter(filter, isJP, tooltip);
                }
            }
        }


        public static bool CanUseMID2AGB()
        {
            string compiler_exe = Program.Config.at("mid2agb", "");
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                return false;
            }
            return true;
        }

        static bool IsMid2AgbBadFilename(string filename)
        {
            string name = Path.GetFileNameWithoutExtension(filename);
            return ! RegexCache.IsMatch(name, "^[a-zA-Z][a-zA-Z0-9_]+$");
        }

        //mid2agbでmidをsへコンパイル
        public static bool CompilerMID2AGB(string target_filename, int optuon_v, int optuon_r, out string output)
        {
            output = "";

            string compiler_exe = Program.Config.at("mid2agb", "");
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "mid2agb");
                return false;
            }

            string tooldir = Path.GetDirectoryName(target_filename);

            string args = ""
                + U.escape_shell_args( Path.GetFileName(target_filename) );
            if (optuon_v != 127)
            {
                args += " " + "-V" + optuon_v.ToString();
            }
            if (optuon_r != 0)
            {
                args += " " + "-R" + optuon_r.ToString();
            }
            bool isMid2AgbBadFilename = IsMid2AgbBadFilename(target_filename);
            if (isMid2AgbBadFilename)
            {
                args += " " + "-Lfeb" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);

            if (output.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) == 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            string output_target = Path.Combine(tooldir, Path.GetFileNameWithoutExtension(target_filename) + ".s");
            if (!File.Exists(output_target) || U.GetFileSize(output_target) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            output = output_target;
            return true;
        }

        //GBAMusRiperでmidiをexport
        public static bool ExportMidiByGBAMusRiper(string save_filename, uint songtrack_addr, out string output)
        {
            output = "";

            string compiler_exe = OptionForm.GetGBAMusRiper();
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "gba_mus_riper");
                return false;
            }

            string tempfilename;
            try
            {
                tempfilename = U.WriteTempROM("gbamusripper");
            }
            catch (Exception e)
            {
                R.ShowStopError(R._("ファイルに書き込めませんでした。\r\nファイルが他アプリケーションで利用中の可能性があります。\r\n{0}"), e.ToString());
                return false;
            }
            

            string tooldir = Path.GetDirectoryName(save_filename);

            string args = ""
                + U.escape_shell_args(tempfilename)
                + " "
                + U.escape_shell_args(save_filename)
                + " "
                + U.To0xHexString(songtrack_addr);

            output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);
            File.Delete(tempfilename);

            if (output.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) == 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            string output_target = save_filename;
            if (!File.Exists(output_target) || U.GetFileSize(output_target) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            output = output_target;
            return true;
        }
        //GBAMusRiperでsf2をexport
        public static bool ExportSoundFontByGBAMusRiper(string save_filename, uint songtrack_addr, out string output)
        {
            output = "";

            string compiler_exe = OptionForm.GetGBAMusRiper();
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "gba_mus_riper");
                return false;
            }
            compiler_exe = Path.GetDirectoryName(compiler_exe);
            compiler_exe = Path.Combine(compiler_exe , "sound_font_riper.exe");
            if (!File.Exists(compiler_exe))
            {
                output = R.Error(("ファイルがありません。\r\nファイル名:{0}"), compiler_exe);
                return false;
            }

            string tempfilename;
            try
            {
                tempfilename = U.WriteTempROM("gbamusripper");
            }
            catch (Exception e)
            {
                R.ShowStopError(R._("ファイルに書き込めませんでした。\r\nファイルが他アプリケーションで利用中の可能性があります。\r\n{0}"), e.ToString());
                return false;
            }

            string tooldir = Path.GetDirectoryName(save_filename);

            string args = ""
                + U.escape_shell_args(tempfilename)
                + " "
                + U.escape_shell_args(save_filename)
                + " "
                + U.To0xHexString(songtrack_addr);

            output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);
            File.Delete(tempfilename);

            if (output.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) == 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            string output_target = save_filename;
            if (!File.Exists(output_target) || U.GetFileSize(output_target) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            output = output_target;
            return true;
        }
        //soxでwavファイルを変換
        public static bool ConvertWaveBySOX(string save_filename, string from_file, uint chunnel, uint hz, uint strip, uint volume100, out string output)
        {
            output = "";

            string compiler_exe = OptionForm.GetSox();
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "sox");
                return false;
            }

            string tooldir = Path.GetDirectoryName(save_filename);

            string args = "";
            if (volume100 != 0)
            {
                args += String.Format(" -v {0} ",volume100 / 100.0f);
            }
            args += " " + U.escape_shell_args(from_file);
            if (hz != 0)
            {
                args += String.Format(" -r {0}" , hz);
            }
            if (chunnel != 0)
            {
                args += String.Format(" -b 8 "); //8bit
                args += String.Format(" -c {0}", chunnel);
            }
            args += " " + U.escape_shell_args(save_filename);
            if (strip != 0)
            {
                args += String.Format(" silence 1 0.2 {0}% reverse silence 1 0.2 {0}% reverse", strip - 1);
            }
            if (volume100 != 0)
            {
                args += String.Format(" gain -h ");
            }

            output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);
            if (output.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) == 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            string output_target = save_filename;
            if (!File.Exists(output_target) || U.GetFileSize(output_target) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            output = output_target;
            return true;
        }

        //Midfix4agbでmidiを補正
        public static bool ConvertMidfix4agb(string filename, out string output)
        {
            output = "";

            string compiler_exe = OptionForm.GetMidfix4agb();
            if (compiler_exe == "" || !File.Exists(compiler_exe))
            {
                output = R._("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "midfix4agb");
                return false;
            }
            if (!File.Exists(compiler_exe))
            {
                output = R.Error(("ファイルがありません。\r\nファイル名:{0}"), compiler_exe);
                return false;
            }

            string middir = Path.GetDirectoryName(filename);
            string output_target = Path.Combine(middir, Path.GetFileNameWithoutExtension(filename) + "_FINAL.mid");
            if (File.Exists(output_target))
            {
                File.Delete(output_target);
            }

            string tooldir = Path.GetDirectoryName(compiler_exe);

            string args = ""
                + U.escape_shell_args(filename)
                ;

            output = ProgramRunAsAndEndWait(compiler_exe, args, tooldir);

            if (!File.Exists(output_target) || U.GetFileSize(output_target) <= 0)
            {//エラーなのでコマンド名もついでに付与
                output = compiler_exe + " " + args + " \r\noutput:\r\n" + output;
                return false;
            }

            output = output_target;
            return true;
        }

        public static byte[] OpenROMToByte(string path, string orignalFile = "")
        {
            if (path == "")
            {
                return new byte[0];
            }
            if (!U.CanReadFileRetry(path))
            {
                return new byte[0];
            }

            byte[] bin;
            string ext = U.GetFilenameExt(path);
            if (ext == ".7Z")
            {//7zの場合展開する.
                using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
                {
                    ArchSevenZip.Extract(path, tempdir.Dir);
                    string[] files = U.Directory_GetFiles_Safe(tempdir.Dir, "*.gba", SearchOption.AllDirectories);
                    if (files.Length < 1)
                    {
                        return new byte[0];
                    }
                    bin = File.ReadAllBytes(files[0]);
                }
            }
            else if (ext == ".UPS")
            {//UPS適応
                if (!U.CanReadFileRetry(orignalFile))
                {
                    return new byte[0];
                }
                ROM rom = new ROM();
                string version;
                bool r = rom.Load(orignalFile, out version);
                if (!r)
                {
                    R.ShowStopError("未対応のROMです。\r\ngame version={0}", version);
                    return new byte[0];
                }
                r = UPSUtil.ApplyUPS(rom, path);
                if (!r)
                {
                    R.ShowStopError("UPSパッチを適応できませんでした");
                    return new byte[0];
                }
                bin = rom.Data;
            }
            else
            {
                bin = File.ReadAllBytes(path);
            }
            return bin;
        }
        static string GetForHighLevelUser()
        {
            return "\r\n" + R._("この機能は上級者向けの機能です。");
        }

        public static string GetExplain(string name)
        {
            if (name == "UnitButton")
            {
                return R._("ユニットの基礎データの設定を行います。\r\n名前や初期パラメータ、ユニット成長率の設定できます。");
            }
            if (name == "SupportUnitButton")
            {
                return R._("支援会話ができるユニットを設定します。");
            }
            if (name == "SupportTalkButton")
            {
                return R._("支援会話の内容を設定します。");
            }
            if (name == "UnitPaletteButton")
            {
                return R._("ユニットごとにどのクラスでどのパレットを利用するかを決定します。\r\nFE8では分岐CCが採用されているため、複雑な指定になっています。\r\nパレットを指定しないと汎用色で描画されます。");
            }
            if (name == "SupportAttributeButton")
            {
                return R._("支援のボーナス効果を設定します。");
            }
            if (name == "UnitIncreaseHeightFormButton")
            {
                return R._("ステータス画面での顔画像の位置調整をします。\r\n背が低いユニットの場合、背を伸ばして表示しないと下に埋もれてしまいます。");
            }
            if (name == "MantAnimationButton")
            {
                return R._("魔法使いの戦闘アニメで、魔法を発動している最中にマントを揺らす処理の指定をします。\r\nただし、この指定はあまりに複雑でまだ解析されていません。\r\nここで設定するのではなく、C01ハックを利用したマントを揺らす方法を利用することをお勧めします。\r\n");
            }
            if (name == "MapEditorButton")
            {
                return R._("マップを編集します。\r\nmar,map,tmx形式をインポート/エクスポートすることも可能です。");
            }
            if (name == "MapStyleEditorButton")
            {
                return R._("マップチップとパレットの設定をします。\r\nマップチップを構成する方法と、それがどんな種類の地形かを設定します。\r\nマップチップと画像のインポート/エクスポートもできます。\r\n");
            }
            if (name == "MapTileAnimation1Button")
            {
                return R._("マップチップのアニメーションを指定します。\r\nタイルアニメーション1は、川のせせらぎといったタイルそのものを変化させるアニメーションの設定になります。");
            }
            if (name == "MapTileAnimation2Button")
            {
                return R._("マップチップのアニメーションを指定します。\r\nタイルアニメーション2は、パレットアニメーションになります。");
            }
            if (name == "SkillConfigButton")
            {
                PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
                if (skill == PatchUtil.skill_system_enum.SkillSystem)
                {
                    return R._("スキル拡張を基本的な設定をします。\r\nスキルの名前やアイコン、スキルのアニメーションを設定します。");
                }
                else if (skill == PatchUtil.skill_system_enum.FE8N)
                {
                    return R._("スキルのアイコンと名前の設定を行います。");
                }
                else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
                {
                    return R._("旧バージョンから引き継いだスキルのアイコン設定をします。\r\nメインの機能は「スキル_クラス別割当」へ移動されました。");
                }
                return R._("スキル拡張を基本的な設定をします。");
            }
            if (name == "SkillAssignmentUnitButton")
            {
                return R._("スキル拡張のユニット別のスキル割り当ての設定を行います。");
            }
            if (name == "SkillAssignmentClassButton")
            {
                return R._("スキル拡張のクラス別のスキル割り当ての設定を行います。");
            }
            if (name == "OAMSPButton")
            {
                return R._("GBAFEのROM内にある特殊OAMデータの閲覧を行います。") + GetForHighLevelUser();
            }
            if (name == "ProcsScriptButton")
            {
                return R._("GBAFEのタスクシステムであるProcsの閲覧と編集をします。") + GetForHighLevelUser();
            }
            if (name == "DisassemblerButton")
            {
                return R._("ROMを逆アセンブルします。\r\nまた、逆アセンブルした結果をファイルに保存します。") + GetForHighLevelUser();
            }
            if (name == "HexEditorButton")
            {
                return R._("ROMをバイナリエディタで編集します。") + GetForHighLevelUser(); ;
            }
            if (name == "ClassButton")
            {
                return R._("クラスの基礎データの設定を行います。\r\n名前やクラスの初期パラメータ、パラメータの上限を設定できます。");
            }
            if (name == "MoveCostButton")
            {
                return R._("移動コストと、地形ボーナスの設定をします。\r\n移動コストは、通常と雨などの天候ごとに設定できます。\r\n地形ボーナスは、地形ごとに回復や回避率を設定できます。");
            }
            if (name == "CCBranchButton")
            {
                return R._("FE8から採用されたCC分岐の設定をします。\r\nユニットが昇格できる上級職を設定します。");
            }
            if (name == "ArenaClassButton")
            {
                return R._("闘技場の敵のクラスの設定します。\r\n近接攻撃と遠隔攻撃ごとに設定できます。");
            }
            if (name == "ArenaEnemyWeaponButton")
            {
                return R._("闘技場で利用できる武器を設定します。");
            }
            if (name == "SoundFootStepsButton")
            {
                return R._("クラスごとの足音を設定します。");
            }
            if (name == "ItemButton")
            {
                return R._("アイテムの設定を行います。\r\n名前やアイテムの種類、攻撃力、耐久力などの設定が設定できます。");
            }
            if (name == "ItemEffectButton")
            {
                return R._("武器で攻撃したときのアニメーションを設定します。\r\n遠隔攻撃をしたときにどんなアニメーションを再生するかを定義します。");
            }
            if (name == "ITEMSTATBOOSTERButton")
            {
                return R._("ドーピングアイテムを利用したときの効果を設定します。\r\nまた、武器がもっている能力補正値のボーナス値の設定も行えます。");
            }
            if (name == "ItemCriticalButton")
            {
                return R._("アーマーキラーのように特定のクラスに特効効果があるアイテムの設定を行います。");
            }
            if (name == "ItemShopButton")
            {
                return R._("お店の設定を行います。\r\n武器屋/道具屋/秘密の店/編成店で売っている品物を編集できます。");
            }
            if (name == "ItemCCButton")
            {
                return R._("CCアイテムごとに、どのユニットがどのアイテムを利用して昇格できるかを設定します。");
            }
            if (name == "ItemCorneredButton")
            {
                return R._("武器の三すくみを定義します。");
            }
            if (name == "ItemUsagePointerButton")
            {
                return R._("杖やアイテムなどを利用したときの処理を定義します。") + GetForHighLevelUser() + "\r\n" + R._("初心者は、杖やアイテムを変更しないことをおすすめします。");
            }
            if (name == "ItemEffectPointerButton")
            {
                return R._("既存のROMに存在する間接攻撃した場合に呼び出されるASM関数を設定します。") + GetForHighLevelUser();
            }
            if (name == "ItemEtcButton")
            {
                return R._("神将器/双聖器の設定を行います。\r\n武器レベルSでの必殺率の設定も行えます。");
            }
            if (name == "ImageBGButton")
            {
                return R._("会話イベントで表示される背景画像の設定を行います。");
            }
            if (name == "ImagePortraitButton")
            {
                return R._("顔画像の設定を行います。");
            }
            if (name == "ImageIconButton")
            {
                return R._("アイテムアイコンの設定を行います。");
            }
            if (name == "ImageUnitWaitIconButton")
            {
                return R._("ユニットが待機状態のときに表示されるアイコンの設定を行います。");
            }
            if (name == "ImageUnitMoveIconButton")
            {
                return R._("ユニットの移動状態のときに表示されるアイコンの設定を行います。");
            }
            if (name == "ImageBattleAnimeButton")
            {
                return R._("武器で攻撃したときの戦闘アニメーションの設定を行います。");
            }
            if (name == "ImageMagicButton")
            {
                return R._("魔法拡張パッチを利用して、新規に魔法エフェクトを定義します。\r\nFEの魔法は戦闘アニメと違い、プログラムでハードコーディングされているため、魔法を追加するには、パッチを当てて魔法システムが必要になります。");
            }
            if (name == "ImageBattleFieldButton")
            {
                return R._("戦闘時に表示される背景を設定します。");
            }
            if (name == "ImageBattleTerrainButton")
            {
                return R._("戦闘時に表示される地形の設定を行います。");
            }
            if (name == "BattleScreenButton")
            {
                return R._("戦闘画面の設定を行います。\r\nただし、戦闘画面は複雑な構成をしているので、安易に変更すると危険です。\r\n変更する場合は、バックアップを取ってからやってください。");
            }
            if (name == "BigCGButton")
            {
                string str = R._("イベントで表示されるCGの設定を行います。");
                if (Program.ROM.RomInfo.version() == 8)
                {
                    str += "\r\n" + R._("FE8ではパッチを利用することでCGをイベントで利用することができます。");
                }
                return str;
            }
            if (name == "ImageChapterTitleButton")
            {
                return R._("章タイトルを画像で設定します。");
            }
            if (name == "ImageUnitPaletteButton")
            {
                return R._("ユニットごとに利用するカラーパレットを設定します。\r\nカラーパレットが定義されていない場合は、汎用の色が採用されます。");
            }
            if (name == "Command85PointerButton")
            {
                return R._("戦闘アニメのC01などの85Commandで呼び出されるASM関数を設定します。") + GetForHighLevelUser();
            }
            if (name == "ImageTSAAnimeButton")
            {
                return R._("イベントでマップ上に表示されるアニメーションの設定を行います。\r\nこのアニメーションはプログラムでハードコーディングされているので、増やすことはできません。");
            }
            if (name == "ImageTSAAnime2Button")
            {
                return R._("イベントでマップ上に表示されるアニメーションの設定を行います。\r\nこのアニメーションはプログラムでハードコーディングされているので、増やすことはできません。");
            }
            if (name == "ROMAnimeButton")
            {
                return R._("ROM内に存在する魔法データの閲覧を行います。\r\nGBAFEでは、魔法はプログラムでハードコーディングされているため、一部データだけを閲覧できます。\r\nこのデータを書き換えることもできますが、書き換えないことを推奨します。\r\n魔法を改造したい場合は、魔法拡張の方を利用してください。") + GetForHighLevelUser();
            }
            if (name == "MapSettingButton")
            {
                return R._("章の設定を行います。\r\n章で表示するマップの種類や、BGM、勝利条件の表示の設定を行います。");
            }
            if (name == "MapPointerButton")
            {
                return R._("PLISTの表示を行います。\r\nPLISTは、GBAFEのマップデータにかかわるすべてのポインタを格納した巨大なリストです。\r\nFEBuilderGBAでは、PLISTを直接変更しなくてもいいように作られてます。") + GetForHighLevelUser();
            }
            if (name == "EventCondButton")
            {
                return R._("章のイベントを発生させる条件を設定します。\r\n増援などのターンごとのイベントや\r\n話す、村、宝箱、お店、制圧ポイント、ゲームオーバーイベントなどのあらゆる条件を設定します。");
            }
            if (name == "EventScriptButton")
            {
                return R._("イベントスクリプトの設定を行います。\r\nイベントでどんなセリフを表示するかといったスクリプトを書いていきます。");
            }
            if (name == "EventUnitButton")
            {
                return R._("ユニットをマップのどこに登場させるかといった設定を行います。");
            }
            if (name == "EventBattleTalkButton")
            {
                return R._("ボスと戦うときに表示されるセリフを設定します。\r\nGBAFEでは、ボスと会話した場合は、フラグ0x01を利用することが習慣になっています。");
            }
            if (name == "EventHaikuButton")
            {
                return R._("ユニットが死亡したときに表示されるセリフを設定します。\r\nGBAFEでは、ボスを倒した場合は、フラグ0x02を利用することが習慣になっています。");
            }
            if (name == "EventForceSortieButton")
            {
                string str = R._("必ず出撃しなければいけないユニットを設定します。");
                if (Program.ROM.RomInfo.version() == 7)
                {
                    str = "\r\n" + R._("FE7では、このデータは一部のみ設定できます。");
                }
                else if (Program.ROM.RomInfo.version() == 8)
                {
                    str += "\r\n" + R._("逆に出撃できないユニットを作るには、パッチを利用してください");
                }
                return str;
            }
            if (name == "EventMapChangeButton")
            {
                return R._("ドアや閉じ村、廃村などのマップ変化の指定を行います。\r\nこの指定は、マップエディタから設定した方が簡単です。");
            }
            if (name == "MapExitPointButton")
            {
                return R._("盗賊が離脱するのに利用する離脱ポイントを設定します。");
            }
            if (name == "AIFormButton")
            {
                return R._("敵AIのスクリプトを編集します。") + GetForHighLevelUser();
            }
            if (name == "AI3FormButton")
            {
                return R._("敵が攻撃するユニットを決定するために利用する係数パラメータを変更します。\r\nこの値は、AI3で参照されます。");
            }
            if (name == "AIStealItemButton")
            {
                return R._("敵の盗賊AIがアイテムを盗むときに、どのアイテムを優先的に盗むかを設定します。");
            }
            if (name == "AIMapSettingButton")
            {
                return R._("敵AIができることを章ごとに設定します。\r\n例えば、扉の鍵をもっているユニットが、そのカギを利用して扉を開けることができるかどうかの設定をします。\r\nもし、彼が門番ならば勝手に扉を開けてもらっては困りますし、侵略者ならば扉を開けないといけません。\r\n");
            }
            if (name == "EventFunctionPointerButton")
            {
                return R._("イベントスクリプトの命令で呼び出されるASMを設定します。") + GetForHighLevelUser();
            }
            if (name == "WorldMapEventPointerButton")
            {
                string str = R._("章が開始される前のワールドマップで発生するイベントを設定します。");
                if (Program.ROM.RomInfo.version() == 8)
                {
                    str += "\r\n" + R._("FE8では、ワールドマップを自由に移動できるため、拠点を選択した時と、クリアした時の2種類のイベントを設定できます。");
                }
                return str;
            }
            if (name == "WorldMapRoadButton")
            {
                return R._("ワールドマップに描画される道の設定を行います。\r\nどの拠点間を結ぶかを指定します。");
            }
            if (name == "WorldMapEventPointButton")
            {
                return R._("ワールドマップの城や砦などの拠点を設定します。");
            }
            if (name == "WorldMapImageButton")
            {
                return R._("ワールドマップに利用される画像データの設定を行います。");
            }
            if (name == "MonsterProbabilityButton")
            {
                return R._("モンスターが発生する確率を指定します。");
            }
            if (name == "MonsterDropItemButton")
            {
                return R._("モンスターがもっている所持品を設定します。");
            }
            if (name == "MonsterWMapProbabilityButton")
            {
                return R._("モンスターがワールドマップのどこに発生するかを設定します。") + GetForHighLevelUser();
            }
            if (name == "SummonButton")
            {
                return R._("召喚スキルを持つユニットが、召喚できるユニットを設定します。");
            }
            if (name == "SummonsDemonKingButton")
            {
                return R._("魔王が呼魔で呼び出すことができるモンスターの設定を行います。");
            }
            if (name == "SystemIconButton")
            {
                return R._("ゲーム中に利用されるさまざな画像の設定を行えます。\r\nユニットや敵、友軍、アイテムのパレット\r\nシステムメニューの外観\r\n攻撃範囲や移動範囲の色指定\r\nステータス異常(バッドステータス)");
            }
            if (name == "WorldMapRoadEditorbutton")
            {
                return R._("ワールドマップの道の形を定義します。");
            }
            if (name == "WorldMapRoadMoveEditorbutton")
            {
                return R._("ワールドマップの道を移動するときの経路を指定します。");
            }
            if (name == "MapLoadFunction")
            {
                return R._("マップをロードする時に利用するASM関数を指定します。") + GetForHighLevelUser();
            }
            if (name == "ExtraUnitButton")
            {
                return R._("塔や遺跡をクリアしたときに、パーティーに加入するエクストラユニットの定義を行います。");
            }
            if (name == "ImageGenericEnemyPortraitButton")
            {
                return R._("顔画像がないと一般兵に表示する画像を指定します。");
            }
            if (name == "LinkArenaDenyUnitButton")
            {
                return R._("通信闘技場に登場させられないユニットを指定します。");
            }
            if (name == "TextButton")
            {
                return R._("ゲーム内で表示される文字列の設定を行います。");
            }
            if (name == "TextCharCodeButton")
            {
                return R._("文字ごとにハフマン符号化の設定を行います。\r\n現在は、Anti-Huffmanパッチを利用してハフマン符号化を無視してテキスト設定できるため、利用しなくても問題ありません。");
            }
            if (name == "FontButton")
            {
                return R._("ゲームで利用するフォントを指定します。\r\nアイテムフォントとテキストフォントの2種類を設定できます。");
            }
            if (name == "MapTerrainNameButton")
            {
                return R._("地形ごとの名前を設定します。\r\n草原、高い山、村、砦など。");
            }
            if (name == "EDButton")
            {
                return R._("EDで利用される各種設定を行います。\r\nユニットのその後の活動や、ユニットの称号、ペアエンドなどを定義します。");
            }
            if (name == "OPPrologueButton")
            {
                return R._("OPに表示される字幕画像の設定を行います。");
            }
            if (name == "ClassOPDemoButton")
            {
                return R._("OPに利用されるクラス紹介の設定を行います。");
            }
            if (name == "ClassOPFontButton")
            {
                return R._("OPのクラス紹介で利用される特殊フォントの設定を行います。");
            }
            if (name == "OPClassAlphaNameButton")
            {
                return R._("OPのクラス紹介で利用されるクラスの英語名表記の設定を行います。\r\nこの設定は、CCするときにも利用されます。");
            }
            if (name == "EDStaffRollButton")
            {
                return R._("EDに表示されるスタッフロールの画像を設定します。");
            }
            if (name == "TextDicButton")
            {
                return R._("辞書を選んだときに表示される項目の設定を行います。");
            }
            if (name == "OtherTextButton")
            {
                return R._("ROMに直接書き込まれているテキスト文字列の編集を行います。");
            }
            if (name == "FlagNameToolButton")
            {
                return R._("フラグに理解しやすい名前を設定します。\r\nこの設定はFEBuilderGBAのデータとして保存され、ROMには書き込まれません。");
            }
            if (name == "ExportEAEventToolButton")
            {
                return R._("章ごとの設定をEAでエクスポートします。");
            }
            if (name == "EmulatorMemoryToolButton")
            {
                return R._("FEBuilderGBAから起動したエミュレータへ接続します。\r\nエミュレータのRAMをスキャンして、イベントやProcs等の状態をわかりやすく表示します。");
            }
            if (name == "SoundRoomuttoBn")
            {
                return R._("サウンドルームに掲載する曲の設定を行います。");
            }
            if (name == "SoundBossBGMButton")
            {
                return R._("ボスと戦闘するときに再生されるBGMを設定します。");
            }
            if (name == "SongTableButton")
            {
                return R._("音楽と効果音の一覧を表示します。\r\nGBAでは、音楽と効果音はソングテーブルに一緒に格納されています。");
            }
            if (name == "SongTrackButton")
            {
                return R._("楽譜を表示します。\r\n音楽のimport/exportをすることができます。");
            }
            if (name == "MenuButton")
            {
                return R._("メニューの名前や、表示するときの条件、メニューを選択した場合に動作させるASMの設定を行います。");
            }
            if (name == "MenuDefinitionButton")
            {
                return R._("メニューを束ねるメニュー定義の設定をします。\r\nメニュー定義の中に、メニューコマンドが複数格納されています。");
            }
            if (name == "StatusParamButton")
            {
                return R._("ステータス画面のパラメータの設定をします。");
            }
            if (name == "StatusRMenuButton")
            {
                return R._("RMenuでヘルプを呼出した時に表示される設定を行います。");
            }
            if (name == "StatusRMenuButton")
            {
                return R._("RMenuでヘルプを呼出した時に表示される設定を行います。");
            }
            if (name == "MainSimpleMenuImageSubButton")
            {
                return R._("画像編集のサブメニューを開きます。");
            }
            if (name == "SimpleMenuButton")
            {
                return R._("簡易メニューを表示します。");
            }
            if (name == "DetailMenuButton")
            {
                return R._("詳細メニューを表示します。\r\nFEBuilderGBAのすべての機能にアクセスできます。");
            }
            if (name == "WelcomeDialogButton")
            {
                return R._("FEBuilderGBAを起動したときのWelcomeDialogを表示します。\r\nアップデートはここからできます。");
            }
            if (name == "ROMRebuildButton")
            {
                return R._("ROMを再構築して無駄な領域を解放します。\r\nただし、この処理は危険ですので説明をよく読んでから利用してください。\r\nバックアップを取るのを忘れないように!");
            }
            if (name == "戦績コメント")
            {
                return R._("ゲームクリア後に表示される評価の設定を行います。");
            }
            if (name == "個別アニメ")
            {
                return R._("独自の戦闘アニメを定義します。\r\n通常はクラスごとの戦闘アニメを表示しますが、特定のユニットごとに違うアニメーションを表示したい場合に利用されます。\r\nFE7では、ロイドなどの一部のユニットに利用されています。");
            }
            if (name == "DiffDebugToolButton")
            {
                return R._("原因不明のバグが発生したときに、過去のバックアップと比較して、バグの原因を突き止めるツールです。");
            }
            if (name == "LintButton")
            {
                return R._("ROM内に存在するエラーを自動的に検知します。\r\nROMを静的解析して、よくある間違いを自動的に見つけます。\r\n常にエラーがない状態を維持するのが望ましいです。");
            }
            if (name == "ToolRunAsEventAssemblerEAButton")
            {
                return R._("EventAssemblerでデータを追加します。");
            }
            if (name == "ToolRunAsASMButton")
            {
                return R._("ASMをアセンブリ(コンパイル)したり、ASMをROMへ直接差し込みます。");
            }
            if (name == "DecreaseColorToolButton")
            {
                return R._("画像を減色します。\r\nFEBuilderGBAでは、TSAルールに従った賢い減色処理をすることができます。");
            }
            if (name == "ToolProblemReportToolButton")
            {
                return R._("原因不明のバグが発生した場合、このツールを利用して、report.7zを作成して、コミニティに投稿してください。\r\nreport.7zには、問題を再現するためのupsデータが入っています。\r\nこれを利用すればバグを素早く再現し、修正することができるでしょう。");
            }
            if (name == "PatchButton")
            {
                return R._("ゲームシステムを拡張するさまざまなパッチを利用することができます。\r\nFEBuilderGBAではパッチを簡単に導入できるようにライブラリ化しています。\r\nパッチを作ってくれた皆様に感謝いたします。\r\n\r\nパッチによっては不安定なものもあるので、バックアップを取ってから利用してください。");
            }
            if (name == "PointerToolButton")
            {
                return R._("ゲーム間のポインタの位置を自動的に計算します。\r\nゲームやバージョンを超えた移植に利用します。") + GetForHighLevelUser();
            }
            if (name == "WorldMapBGMButton")
            {
                return R._("ワールドマップで再生するBGMを設定します。");
            }
            if (name == "MapTerrainFloorLookupTableButton")
            {
                return R._("地形のごとに、戦闘アニメの床の画像の設定を行います。");
            }
            if (name == "MapTerrainBGLookupTableButton")
            {
                return R._("地形のごとに、戦闘アニメの背景の画像の設定を行います。");
            }
            if (name == "GameOptionButton")
            {
                return R._("ゲームの設定オプション項目を変更できます。");
            }
            if (name == "AIPerformStaffButton")
            {
                return R._("AIが杖を利用できるかどうか判断する関数テーブルを設定します。");
            }
            if (name == "AIPerformItemButton")
            {
                return R._("AIがアイテムを利用できるかどうか判断する関数テーブルを設定します。");
            }
            if (name == "TacticianAffinity")
            {
                return R._("軍師の属性を決定するテーブルを設定します。");
            }
            if (name == "GameOptionOrderButton")
            {
                return R._("ゲームオプションの表示順番を設定します。");
            }
            if (name == "ToolUseFlagButton")
            {
                return R._("章で利用しているフラグから、イベントを逆に参照します。");
            }
            if (name == "UnitActionPointerButton")
            {
                return R._("UnitActionテーブルを表示します。\r\nUnitActionテーブルは、マップ上のユニットの動作を定義するテーブルです。") + GetForHighLevelUser();
            }
            if (name == "ToolCustomBuildButton")
            {
                return R._("スキル拡張のカスタムビルドを行います。") + GetForHighLevelUser();
            }

            return "";
        }
        public static void MakeExplainFunctions(Control controlPanel)
        {
            for (int i = 0; i < controlPanel.Controls.Count; i++)
            {
                Control c = controlPanel.Controls[i];
                c.AccessibleDescription = GetExplain(c.Name);
            }
        }

        public static string CheckOrignalROM(string filename)
        {
            if (!File.Exists(filename))
            {
                return R._("ファイルが見つかりません");
            }

            U.CRC32 crc32 = new U.CRC32();
            uint targetCRC32 = crc32.Calc(File.ReadAllBytes(filename));

            ROMBaseTableSt[] table = GetROMBaseTable();
            foreach(ROMBaseTableSt t in table)
            {
                if (t.crc32 == targetCRC32)
                {//CRCマッチ
                    return "";
                }
            }

            uint orignalCRC32 = 0;
            if (Program.ROM != null)
            {
                orignalCRC32 = Program.ROM.RomInfo.orignal_crc32();
            }

            string noMatch = R._("指定されたROMは無改造ROMではありません。\r\nCRC32が一致しません。\r\n正規品の無改造ROMを指定してください。\r\n正規品のROMからupsを作らないと誰もそれを開けません。\r\n\r\n指定されたROMのCRC32:\r\n{0}\r\n正規品のCRC32:\r\n{1}", U.ToHexString8(targetCRC32), U.ToHexString8(orignalCRC32));
            return noMatch;
        }

        public static void SetMainFormIcon(Form mainForm)
        {
//やっぱりやめよう
//            mainForm.Icon = Properties.Resources.Icon1;
        }

        //テキストエディタで行番号を指定して開く
        public static void OpenTextEditor(string textFilename,uint number = U.NOT_FOUND)
        {
            string editor = OptionForm.GetSrccodeTexteditor();
            if (!File.Exists(editor))
            {
                string ext = Path.GetExtension(textFilename);
                editor = U.FindAssociatedExecutable(ext);
                if (!File.Exists(editor))
                {
                    return;
                }
            }
            string args;
            string editorFilename = Path.GetFileName(editor);

            editorFilename = editorFilename.ToLower();
            if (number == U.NOT_FOUND)
            {
                args = U.escape_shell_args(textFilename);
            }
            else if (editorFilename == "sakura.exe")
            {
                args = U.escape_shell_args(textFilename) + " -Y=" + number;
            }
            else if (editorFilename == "hidemaru.exe")
            {
                args = U.escape_shell_args(textFilename) + " -j=" + number;
            }
            else if (editorFilename == "vim.exe" || editorFilename == "gvim.exe")
            {
                args = U.escape_shell_args(textFilename) + " -c " + number;
            }
            else if (editorFilename == "emditor.exe")
            {
                args = U.escape_shell_args(textFilename) + " -l=" + number;
            }
            else if (editorFilename == "notepad++.exe")
            {
                args = U.escape_shell_args(textFilename) + " -n=" + number;
            }
            else if (editorFilename == "vscode.exe" || editorFilename == "subl.exe")
            {
                args = U.escape_shell_args(textFilename + ":" + number);
            }
            else
            {
                args = U.escape_shell_args(textFilename);
            }

            Process.Start(editor , args);
        }
        public static void RunToolInitWizard()
        {
            do
            {
                ToolInitWizardForm f = (ToolInitWizardForm)InputFormRef.JumpFormLow<ToolInitWizardForm>();
                DialogResult dr = f.ShowDialog();
                if (dr != DialogResult.Retry)
                {
                    break;
                }
            }
            while (true);
        }
        static bool IsSorceCodeExits(string srccode_filename)
        {
            if (!File.Exists(srccode_filename))
            {
                return false;
            }
            if (U.GetFileSize(srccode_filename) <= 100)
            {
                return false;
            }
            return true;
        }

        static uint GrepAddrInSrcCode(string srccode_filename,uint addr)
        {
            string search_addr = U.ToHexString8(U.toPointer(addr));

            try
            {
                uint number = 0;
                using (StreamReader reader = new StreamReader(srccode_filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        number++;

                        if (line.IndexOf(search_addr) == 0)
                        {
                            return number;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return 0;
        }

        public static void OpenDisassembleSrcCode(uint addr = U.NOT_FOUND)
        {
            string srccode_directory = OptionForm.GetSrccodeDirectory();
            if (srccode_directory == "")
            {
                R.ShowStopError("ソースコードを保存するディレクトリが設定されていません。\r\n設定のパス2の「ソースコードの保存場所」を設定してください。");
                return;
            }

            string srccode_filename = Path.Combine(srccode_directory, "_" + Program.ROM.RomInfo.VersionToFilename() + ".TXT");
            if (!IsSorceCodeExits(srccode_filename))
            {//ソースコードがないので生成する
                ToolDisasmSourceCode f = (ToolDisasmSourceCode)InputFormRef.JumpFormLow<ToolDisasmSourceCode>();
                f.StoreSrcCode = srccode_filename;
                f.ShowDialog();

                if (!IsSorceCodeExits(srccode_filename))
                {//ソースコードを開けませんでした。
                    return;
                }
            }

            uint number = GrepAddrInSrcCode(srccode_filename, addr);

            try
            {
                MainFormUtil.OpenTextEditor(srccode_filename, number);
            }
            catch (Exception ee)
            {
                R.ShowStopError(ee.ToString());
            }
        }
    }
}
