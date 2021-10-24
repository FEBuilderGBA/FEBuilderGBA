using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace FEBuilderGBA
{
    //ROMの変更を監視します.
    class ROMUpdateWatcher
    {
        class WatchData
        {
            //public string FullFilename; //監視するROMファイル名. -> keyへ
            public string programName;
            public Process Process; //メインプロセスならNULL. (自プロセス)
            public DateTime LastModified; //最後の更新時刻.
        };
        Dictionary<string,WatchData> WatchDataDic;
        bool StopWatcher;

        public ROMUpdateWatcher()
        {
            WatchDataDic = new Dictionary<string, WatchData>();
            this.StopWatcher = false;
        }

        public void RegistMain(string fullfilename)
        {
            WatchData w  = new WatchData();
            //w.FullFilename = fullfilename;
            w.Process = null;
            w.programName = "";
            w.LastModified = File.GetLastWriteTime(fullfilename);
            WatchDataDic[fullfilename] = w;
        }
        public void RegistOtherProcess(Process process,string fullfilename)
        {
            if (process.HasExited == true)
            {
                return;
            }

            WatchData w = new WatchData();
            //w.FullFilename = fullfilename; //keyに移動する.
            w.Process = process;
            w.programName = process.ProcessName;
            w.LastModified = File.GetLastWriteTime(fullfilename);
            WatchDataDic[fullfilename] = w;
        }
        public Process GetRunning(string fullfilename)
        {
            WatchData watch;
            if (!WatchDataDic.TryGetValue(fullfilename, out watch))
            {
                return null;
            }
            return watch.Process;
        }
        public void KillProcess(string fullfilename, bool waitUntilDead = false)
        {
            WatchData watch;
            if (!WatchDataDic.TryGetValue(fullfilename, out watch))
            {
                return;
            }
            watch.Process.CloseMainWindow();
            WatchDataDic.Remove(fullfilename);

            if (waitUntilDead)
            {
                WaitUntilDead(watch.Process);
            }
        }
        void WaitUntilDead(Process p)
        {
            p.WaitForExit(1000);
            p.Close();
        }

        public void Stop()
        {
            this.StopWatcher = true;
        }
        public void Resume()
        {
            this.StopWatcher = false;
        }


        //セーブしたとかで更新があったとき
        public void UpdateLastModified(DateTime time)
        {
            foreach (var pair in WatchDataDic)
            {
                pair.Value.LastModified = time;
            }
        }
        public void CheckALL()
        {
            try
            {
                CheckALLLow();
            }
            catch (InvalidOperationException e)
            {//まれに探索中に別のイベントが発生して、状態が変わってしまい、foreachが失敗することがあるらしい.
             //どうしようもないので、ログを出力して、探索を打ち切る.
             //任務は、ROM変更の監視であるため、どうせ、探索が失敗したところでダメージは少ないため。
                Log.Error("InvalidOperationException",e.ToString(),e.StackTrace);
            }
        }
        void CheckALLLow()
        {
            if (this.StopWatcher)
            {
                return;
            }

            foreach (var pair in WatchDataDic)
            {
                string fullfilename = pair.Key;
                if (! File.Exists(fullfilename))
                {
                    continue;
                }

                DateTime date = File.GetLastWriteTime(fullfilename);
                if (pair.Value.LastModified >= date)
                {//変更されていない.
                    continue;
                }
                //変更されている場合、2度、同じ質問をしないように、日付を書き換える.
                pair.Value.LastModified = date;

                if (pair.Value.Process == null)
                {//メインプロセス(自プロセス)
                    string text = R._("現在開いているROMが外部プログラムで変更されています。\r\n変更されたROMを再読み込みしますか？");
                    string title = R._("ROMが変更されました");
                    DialogResult r =
                        MessageBox.Show(text
                            , title
                            , MessageBoxButtons.YesNo
                            , MessageBoxIcon.Exclamation);
                    if (r == System.Windows.Forms.DialogResult.Yes)
                    {
                        Undo.UndoData undodata = Program.Undo.NewUndoData("Edit Other Process:{0}", pair.Key);
                        Program.ROM.SwapNewROMData(File.ReadAllBytes(fullfilename)
                            , pair.Key, undodata);
                        Program.Undo.Push(undodata);
                        return;
                    }
                }
                else
                {//実行した外部プロセス
                    string text = R._("現在開いているROMが外部プログラムで変更されています。\r\n変更されたROMを再読み込みしますか？");
                    string title = R._("ROMが変更されました");
                    DialogResult r =
                        MessageBox.Show(text
                            , title
                            , MessageBoxButtons.YesNo
                            , MessageBoxIcon.Exclamation);
                    if (r == System.Windows.Forms.DialogResult.Yes)
                    {
                        Undo.UndoData undodata = Program.Undo.NewUndoData("Edit Other Process:{0}", pair.Key);
                        Program.ROM.SwapNewROMData(File.ReadAllBytes(fullfilename)
                            ,pair.Key , undodata);
                        Program.Undo.Push(undodata);
                        return;
                    }
                }

                if (File.Exists(fullfilename))
                {
                    pair.Value.LastModified = File.GetLastWriteTime(fullfilename);
                }
            }

            bool reloop;
            do
            {
                reloop = false;
                foreach (var pair in WatchDataDic)
                {
                    if (pair.Value.Process == null)
                    {//メインプロセス(自プロセス)
                        continue;
                    }
                    if ( U.IsProcessExit(pair.Value.Process) )
                    {//終了したかどうか？
                        WatchDataDic.Remove(pair.Key);

                        if (IsEmulatorProcess(pair.Key))
                        {
                            EmulatorMemoryForm.CloseIfAutoClose();
                        }

                        reloop = true; //削除があったのでループは最初からやり直す.
                        break;
                    }
                }
            } while (reloop);
        }

        bool IsEmulatorProcess(string romfilename)
        {
            string filename = Path.GetFileName(romfilename);
            if (filename.IndexOf(".emulator.") > 0)
            {//ROMに .emulator. と入っていれば emuだろうw
                return true;
            }
            return false;
        }
    }
}
