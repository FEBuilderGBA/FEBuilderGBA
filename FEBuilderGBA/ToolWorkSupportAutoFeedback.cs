using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FEBuilderGBA
{
    public class ToolWorkSupportAutoFeedback
    {
        bool IsAutoFeedBack = false;
        string USERHASH = "";
        string VERSION = "";

        string AUTOFEEDBACK_POST_USERHASH = "";
        string AUTOFEEDBACK_POST_VERSION = "";
        string AUTOFEEDBACK_POST_CHAPTER = "";
        string AUTOFEEDBACK_POST_DEADUNIT = "";
        string AUTOFEEDBACK_POST_BASE64 = "";
        string AUTOFEEDBACK_URL = "";
        uint AUTOFEEDBACK_ENABLE_FLAG = 0;
        uint AUTOFEEDBACK_ENABLE_FLAG_MAPID = 0;

        string SavFilename = "";

        const int FEEDBACK_WAIT_LONG_MINUTE = 10;
        const int FEEDBACK_WAIT_MINUTE = 1;
        DateTime LastFeedBackLongPostTime = DateTime.Now.AddMinutes(-FEEDBACK_WAIT_LONG_MINUTE);
        DateTime LastFeedBackPostTime = DateTime.Now.AddMinutes(-FEEDBACK_WAIT_MINUTE);
        string LastFeedBackType = "";
        bool IsBusy = false;

        public void Init(Dictionary<string, string> lines, bool enableAutoFeedback)
        {
            string url = U.at(lines, "AUTOFEEDBACK_URL");
            AUTOFEEDBACK_URL = url;

            AUTOFEEDBACK_POST_USERHASH = U.at(lines, "AUTOFEEDBACK_POST_USERHASH");
            AUTOFEEDBACK_POST_VERSION = U.at(lines, "AUTOFEEDBACK_POST_VERSION");
            AUTOFEEDBACK_POST_CHAPTER = U.at(lines, "AUTOFEEDBACK_POST_CHAPTER");
            AUTOFEEDBACK_POST_DEADUNIT = U.at(lines, "AUTOFEEDBACK_POST_DEADUNIT");
            AUTOFEEDBACK_POST_BASE64 = U.at(lines, "AUTOFEEDBACK_POST_BASE64");
            AUTOFEEDBACK_ENABLE_FLAG = U.atoi0x(U.at(lines, "AUTOFEEDBACK_ENABLE_FLAG"));
            AUTOFEEDBACK_ENABLE_FLAG_MAPID = U.atoi0x(U.at(lines, "AUTOFEEDBACK_ENABLE_FLAG_MAPID"));

            this.USERHASH = MakeUserHash();
            this.VERSION = MakeVersion();
            this.SavFilename = MakeSavFilename();
            if (url == "")
            {//URLが無効なのでできません
                IsAutoFeedBack = false;
            }
            else if (enableAutoFeedback == false)
            {//ユーザーが拒否しているのでできません
                IsAutoFeedBack = false;
            }
            else
            {//OK,自動フィードバックできる
                IsAutoFeedBack = true;
            }
        }
        public void SetAutoFeedBackStatus(bool r)
        {
            if (AUTOFEEDBACK_URL == "")
            {
                IsAutoFeedBack = false;
                return;
            }
            IsAutoFeedBack = r;
        }

        string MakeSavFilename()
        {
            return U.ChangeExtFilename(Program.ROM.Filename, ".SAV", ".emulator");
        }
        string MakeUserHash()
        {
            string users = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            byte[] data = System.Text.Encoding.ASCII.GetBytes(users);
            uint sum = U.CalcCheckSUM(data);
            return sum.ToString();
        }
        string MakeVersion()
        {
            uint size = (uint) (Program.ROM.Data.Length / 1024);
            if (! File.Exists(Program.ROM.Filename))
            {
                return "_" + size;
            }
            string filename = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
            DateTime dt = U.GetFileDateLastWriteTime(Program.ROM.Filename);
            return filename + "\r\n" + dt.ToString("yyyyMMdd") + "\r\n" + size;
        }
        public bool GetIsAutoFeedBack()
        {
            return IsAutoFeedBack;
        }
        public bool GetAutoFeedBackEnable()
        {
            if (AUTOFEEDBACK_URL == "")
            {
                return false;
            }
            return true;
        }

        uint FailConnect = 0;   //接続失敗数
        const uint FEEDBACK_FAIL_REPORT_COUNT = 3; //クラッシュと判断する接続失敗数。1つに付き1秒
        public void AutoFeedbackFail(uint mapid)
        {
            if (!IsAutoFeedBack)
            {//フィードバックはしない
                this.FailConnect = 0;
                return;
            }
            if (!Program.RAM.GetIsPageError())
            {//ページエラーではない
                this.FailConnect = 0;
                return;
            }
            if (IsBusy)
            {//レポートスレッドが動作しているので、何もしない
                return;
            }

            this.FailConnect++;
            if (this.FailConnect == FEEDBACK_FAIL_REPORT_COUNT)
            {
                DateTime now = DateTime.Now;
                SendFeedBack_Fail(now, mapid);
                this.FailConnect = 0;
            }
        }

        public void SendFeedBack(uint mapid)
        {
            //接続に成功しているので失敗カウントを消します
            this.FailConnect = 0;

            if (!IsAutoFeedBack)
            {//フィードバックはしない
                return;
            }
            if (IsBusy)
            {//レポートスレッドが動作しているので、何もしない
                return;
            }

            DateTime now = DateTime.Now;
            if (SendFeedBack_EndEvent(now, mapid))
            {
            }
            else if (SendFeedBack_DeadEvent(now, mapid))
            {
            }
            else if (SendFeedBack_VillageDestroyXY(now, mapid))
            {
            }
            else if (SendFeedBack_ReloadDetection(now, mapid))
            {
            }

            EnableAutoFeedbackFlag(now, mapid);
        }
        void EnableAutoFeedbackFlag(DateTime now, uint mapid)
        {
            if (mapid != AUTOFEEDBACK_ENABLE_FLAG_MAPID)
            {
                return;
            }
            if (AUTOFEEDBACK_ENABLE_FLAG == 0)
            {
                return;
            }
            if (now <= LastFeedBackPostTime)
            {//クールダウン中
                return;
            }
            bool flag = EmulatorMemoryForm.IsFlagEnable(AUTOFEEDBACK_ENABLE_FLAG);
            if (flag)
            {//有効なら何もしない
                return;
            }
            //無効なら有効にする
            EmulatorMemoryForm.WriteFlagLow(AUTOFEEDBACK_ENABLE_FLAG, true);
        }

        bool SendFeedBack_EndEvent(DateTime now, uint mapid)
        {
            //LOMAでマップIDを切り替えるケースがあるので、長いクールダウンを利用します
            if (now <= LastFeedBackLongPostTime)
            {//クールダウン中
                return false;
            }
            bool flag0x03 = EmulatorMemoryUtil.IsFlag0x03Enable();
            if (! flag0x03)
            {
                return false;
            }

            string eventType = "EndEvent" + mapid;
            if (LastFeedBackType == eventType)
            {//連続して報告はしない
                return false;
            }
            uint maptaskProcs = EmulatorMemoryUtil.SearchMapTaskProcsAddr();
            if (maptaskProcs == U.NOT_FOUND)
            {//マップに入っていないので、章タイトルを表示している最中だろうから、送ってはいけない
                return false;
            }

            string chapter = GetChapterAndInfo(mapid);
            Send(chapter, "");
            LastFeedBackType = eventType;
            LastFeedBackLongPostTime = DateTime.Now.AddMinutes(FEEDBACK_WAIT_LONG_MINUTE);
            return true;
        }
        bool SendFeedBack_DeadEvent(DateTime now, uint mapid)
        {
            if (now <= LastFeedBackPostTime)
            {//クールダウン中
                return false;
            }

            uint deadunit = EmulatorMemoryUtil.DeadPlayerUnit();
            if (deadunit == 0)
            {
                return false;
            }

            string eventType = "UnitDead" + deadunit;
            if (LastFeedBackType == eventType)
            {//連続して報告はしない
                return false;
            }
            uint maptaskProcs = EmulatorMemoryUtil.SearchMapTaskProcsAddr();
            if (maptaskProcs == U.NOT_FOUND)
            {//マップに入っていないので、章タイトルを表示している最中だろうから、送ってはいけない
                return false;
            }

            string chapter = GetChapterAndInfo(mapid);
            string deadunitString = GetDeadUnitAndInfo(deadunit);
            Send(chapter, deadunitString);
            LastFeedBackType = eventType;
            LastFeedBackPostTime = DateTime.Now.AddMinutes(FEEDBACK_WAIT_MINUTE);
            return true;
        }
        bool SendFeedBack_VillageDestroyXY(DateTime now, uint mapid)
        {
            if (now <= LastFeedBackPostTime)
            {//クールダウン中
                return false;
            }

            uint villageDestroyXY = EmulatorMemoryUtil.VillageDestoryXY();
            if (villageDestroyXY == U.NOT_FOUND)
            {
                return false;
            }
            string eventType = "VillageDestroy" + villageDestroyXY;
            if (LastFeedBackType == eventType)
            {//連続して報告はしない
                return false;
            }
            uint maptaskProcs = EmulatorMemoryUtil.SearchMapTaskProcsAddr();
            if (maptaskProcs == U.NOT_FOUND)
            {//マップに入っていないので、章タイトルを表示している最中だろうから、送ってはいけない
                return false;
            }

            string chapter = GetChapterAndInfo(mapid);
            string deadunitString = GetVillageDestroyXYAndInfo(villageDestroyXY);
            Send(chapter, deadunitString);

            LastFeedBackType = eventType;
            LastFeedBackPostTime = DateTime.Now.AddMinutes(FEEDBACK_WAIT_MINUTE);
            return true;
        }

        uint SendFeedBack_TurnStartClock = 0;
        bool SendFeedBack_ReloadDetection(DateTime now, uint mapid)
        {
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address;
            uint turnStartClock = Program.RAM.u32(stageStructAddr + 0);

            if (turnStartClock == 0)
            {//ゲームが開始していない タイトルメニュー等
                SendFeedBack_TurnStartClock = 0;
                return false;
            }
            if (SendFeedBack_TurnStartClock != 0)
            {
                SendFeedBack_TurnStartClock = turnStartClock;
                return false;
            }
            SendFeedBack_TurnStartClock = turnStartClock;
            if (now <= LastFeedBackPostTime)
            {//クールダウン中
                return false;
            }

            //0から有効な値になったので、リロードされたと判定する
            string eventType = "ReloadGame" + turnStartClock;
//            if (LastFeedBackType == eventType)
//            {//連続して報告はしない
//                return false;
//            }

            string chapter = GetChapterAndInfo(mapid);
            string deadunit = "reload game";
            Send(chapter, deadunit);

            LastFeedBackType = eventType;
            LastFeedBackPostTime = DateTime.Now.AddMinutes(FEEDBACK_WAIT_MINUTE);
            return true;
        }

        bool SendFeedBack_Fail(DateTime now, uint mapid)
        {
            if (now <= LastFeedBackPostTime)
            {//クールダウン中
                return false;
            }

            string eventType = "Hang-up detection" + mapid;
//            if (LastFeedBackType == eventType)
//            {//連続して報告はしない
//                return false;
//            }

            string chapter = GetChapterAndInfo(mapid);
            string deadunit = "Hang-up detection";
            Send(chapter, deadunit);
            LastFeedBackType = eventType;
            LastFeedBackPostTime = DateTime.Now.AddMinutes(FEEDBACK_WAIT_MINUTE);
            return true;
        }

        string MakeBase64()
        {
            if (! File.Exists(this.SavFilename))
            {//セーブファイルがない!
                return "";
            }

            try
            {
                using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
                {
                    string filenameOnly = Path.GetFileName(this.SavFilename);
                    string srcFilename = Path.Combine(tempdir.Dir, filenameOnly);
                    string dest7zFilename = Path.Combine(tempdir.Dir, filenameOnly + ".7z");
                    File.Copy(this.SavFilename, srcFilename);
                    ArchSevenZip.Compress(dest7zFilename, srcFilename, 10);
                    byte[] bin = File.ReadAllBytes(dest7zFilename);
                    return System.Convert.ToBase64String(bin);
                }
            }
            catch (Exception e)
            {
                R.Error("セーブデータを7z圧縮できませんでした", e.ToString());
                return "";
            }
        }
        string GetChapterAndInfo(uint mapid)
        {
            string chapter = MapSettingForm.GetMapName(mapid);
            if (chapter == "")
            {
                return mapid.ToString("X02") + " ????";
            }
            chapter = mapid.ToString("X02") + " " + chapter
                + "\r\n" + EmulatorMemoryUtil.MakeChapterExtraInfo();
            return chapter;
        }
        string GetDeadUnitAndInfo(uint uid)
        {
            string deadunit;
            if (uid == 0)
            {
                deadunit = "";
            }
            else
            {
                deadunit = UnitForm.GetUnitName(uid);
                deadunit = uid.ToString("X02") + " " + deadunit
                    + "\r\n" + EmulatorMemoryUtil.MakeUnitBattleExtraInfo();
            }
            return deadunit;
        }
        string GetVillageDestroyXYAndInfo(uint xy)
        {
            string deadunit;
            if (xy == U.NOT_FOUND)
            {
                deadunit = "";
            }
            else
            {
                deadunit = "VillageDestory:" + (xy & 0xFF) + "," + ((xy >> 8) & 0xFF);
                deadunit += "\r\n" + EmulatorMemoryUtil.MakeActiveUnitExtraInfo();
            }
            return deadunit;
        }

        void Send(string chapter, string deadunit)
        {
            System.Threading.Thread s1 = new System.Threading.Thread(t =>
            {
                IsBusy = true;
                try
                {
                    string base64 = MakeBase64();
                    Send(chapter, deadunit, base64);
                }
                catch (Exception)
                {
                }
                IsBusy = false;
            });
            s1.Start();
        }

        void Send(string chapter, string deadunit, string base64)
        {
            if (AUTOFEEDBACK_URL == "")
            {
                return;
            }

            try
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                args["ifq"] = "";
                if (AUTOFEEDBACK_POST_USERHASH != "") args[AUTOFEEDBACK_POST_USERHASH] = this.USERHASH;
                if (AUTOFEEDBACK_POST_VERSION != "") args[AUTOFEEDBACK_POST_VERSION] = this.VERSION;
                if (AUTOFEEDBACK_POST_CHAPTER != "") args[AUTOFEEDBACK_POST_CHAPTER] = chapter;
                if (AUTOFEEDBACK_POST_DEADUNIT != "") args[AUTOFEEDBACK_POST_DEADUNIT] = deadunit;
                if (AUTOFEEDBACK_POST_BASE64 != "") args[AUTOFEEDBACK_POST_BASE64] = base64;
                args["submit"] = "Submit";

                U.HttpPost(AUTOFEEDBACK_URL, args);
                Log.Notify("AutoFeedback", AUTOFEEDBACK_URL, this.USERHASH, this.VERSION, chapter, deadunit, base64);
            }
            catch (Exception)
            {
            }
        }
    }
}
