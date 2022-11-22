using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Drawing;

namespace FEBuilderGBA
{
    static class Program
    {
        public static Dictionary<string, string> ArgsDic { get; private set; }

        [STAThread]
        static void Main(string[] args)
        {
            //オプション引数 --mode=foo とかを、dic["--mode"]="foo" みたいに変換します. 
            ArgsDic = U.OptionMap(args, "--rom");
            //メインスレッド判定に利用するためにスレッドIDを保存
            MainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#if !DEBUG
            SetExceptionHandler();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //自プロセスのパスから、ベースディレクトリを特定します.
            Program.BaseDirectory = MakeBaseDirectory();

            Log.TouchLogDirectory();

            //設定の読み込み
            Config = new Config();
            Config.Load(System.IO.Path.Combine(BaseDirectory, "config", "config.xml"));
            Config.UpdateShortcutKeys();

            //システム側のテキストエンコード いかにしてUnicodeにするかどうか.
            ReBuildSystemTextEncoder();

            //多言語切り替え
            ReLoadTranslateResource();


            //外部プロセスからの書き換え監視等の開始
            UpdateWatcher = new ROMUpdateWatcher();

            string forceversion = U.at(ArgsDic, "--force-version");//強制バージョン指定 --force-version=FE8J
            if (ArgsDic.ContainsKey("--lastrom"))
            {
                MainFormUtil.Open(null, Program.GetLastROMFilename(), false, forceversion);
            }
            if (ArgsDic.ContainsKey("--rom"))
            {
                MainFormUtil.Open(null, ArgsDic["--rom"], false, forceversion);
            }
            
            {
                //自動アップデートがやりたいので、初期化だけはやろうか...
                WelcomeForm f = (WelcomeForm)InputFormRef.JumpFormLow<WelcomeForm>();
                if (Program.ROM == null)
                {//ROMが読み込めない場合、welcomeダイアログを出す.
                    WelcomeForm.CheckInitWizard();

                    f.ShowDialog();
                    if (Program.ROM == null)
                    {//それでもROMが読み込めない場合、終了.
                        return;
                    }
                }
            }

            //コマンドラインの処理があれば。
            if (ProcCommandLine())
            {//コマンドラインを実行できたので、即終了
                return;
            }

            //メインフォームを開く.
            do
            {
                Program.DoReOpen = false;
                Form f = MainForm();
                MainFormUtil.SetMainFormIcon(f);
                Application.Run(f);
            }
            while (Program.DoReOpen); //メインフォームを作り直すためループにする.

            //キャッシュスレッドが動いていたら止める
            if (AsmMapFileAsmCache != null)
            {
                AsmMapFileAsmCache.Join();
            }
            //ログの書き込み
            Log.SyncLog();
        }

        static bool ProcCommandLine()
        {
            if (ArgsDic.ContainsKey("--lint"))
            {//コマンドラインLint
                Program.IsCommandLine = true;
                Environment.Exit(ToolFELintForm.CommandLineLint());
                return true;
            }
            if (ArgsDic.ContainsKey("--rebuild"))
            {//コマンドラインrebuild
                Program.IsCommandLine = true;
                Environment.Exit(ToolROMRebuildForm.ComandLineRebuild());
                return true;
            }
            if (ArgsDic.ContainsKey("--pointercalc"))
            {//ポインタ変換
                Program.IsCommandLine = true;
                Environment.Exit(PointerToolForm.ComandLineSearch());
                return true;
            }
            if (ArgsDic.ContainsKey("--translate"))
            {//ROM翻訳の実行
                Program.IsCommandLine = true;
                Environment.Exit(ToolTranslateROMForm.CommandLineTranslate());
                return true;
            }
            if (ArgsDic.ContainsKey("--makeups"))
            {//UPSの作成
                Program.IsCommandLine = true;
                Environment.Exit(ToolUPSPatchSimpleForm.CommandLineMakeUPS());
                return true;
            }
            if (ArgsDic.ContainsKey("--songexchange"))
            {//曲交換
                Program.IsCommandLine = true;
                Environment.Exit(SongExchangeForm.CommandLineImport());
                return true;
            }
            if (ArgsDic.ContainsKey("--disasm"))
            {//逆汗
                Program.IsCommandLine = true;
                Environment.Exit(DisASMDumpAllForm.CommandLineDisasm(ArgsDic["--disasm"]));
                return true;
            }

#if DEBUG
            //デバッグの場合はテストを実行
            DebugTESTRunner();

            if (ArgsDic.ContainsKey("--translate_batch"))
            {//翻訳を自動実行
                DevTranslateForm.CommandLineTranslateOnly();
            }
            if (ArgsDic.ContainsKey("--testonly"))
            {
                //フルテストの場合、終了する.
                Environment.Exit(0);
                return true;
            }
#endif
            return false;
        }

#if DEBUG
        //静的メソッド TEST_hogehoge と定されたテストメソッドを全部実行する. 
        static void DebugTESTRunner()
        {
            //現在のコードを実行しているアセンブリを取得する
            Assembly asm = Assembly.GetExecutingAssembly();

            //アセンブリで定義されている型をすべて取得する
            Type[] ts = asm.GetTypes();

            //型の情報を表示する
            foreach (Type t in ts)
            {
                MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                foreach (MethodInfo m in methods)
                {
                    if (m.Name.IndexOf("TESTNOW_") == 0)
                    {
                        m.Invoke(null, null);
                    }
                }
            }

            foreach (Type t in ts)
            {
                MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                foreach (MethodInfo m in methods)
                {
                    if (m.Name.IndexOf("TEST_") == 0)
                    {
                        m.Invoke(null, null);
                    }
                }
            }

            if (!ArgsDic.ContainsKey("--test"))
            {
                if (!ArgsDic.ContainsKey("--testonly"))
                {
                    return;
                }
            }
            //フルテストの場合は、重たいテストも実行する
            foreach (Type t in ts)
            {
                MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                foreach (MethodInfo m in methods)
                {
                    if (m.Name.IndexOf("TESTFULL_") == 0)
                    {
                        m.Invoke(null, null);
                    }
                }
            }
        }
        public static string GetTestData(string name)
        {
            string fullfilename = System.IO.Path.Combine(Program.BaseDirectory, "..","..","test",name);
            return fullfilename;
        }
#endif
        static string MakeBaseDirectory()
        {
            string[] args = Environment.GetCommandLineArgs();
            Debug.Assert(args.Length >= 1);

            //コマンドライン引数の最初の引数にはプロセスへのパスが入っています.
            string selfPath = args[0];
            //コマンドライン引数で渡されると、相対パスになるので、一度フルパスに変換します.
            selfPath = Path.GetFullPath(selfPath);
            //ディレクトリ名の取得
            string currentDir = System.IO.Path.GetDirectoryName(selfPath);
            //現在のプロセスがあるディレクトリがベースディレクトリです.
            return Path.GetFullPath(currentDir);
        }

        static void SetExceptionHandler()
        {
            // ThreadExceptionイベント・ハンドラを登録する
            Application.ThreadException += (object sender, ThreadExceptionEventArgs e) =>
            {
                ShowErrorMessage(e.Exception);
            };

            // UnhandledExceptionイベント・ハンドラを登録する
            Thread.GetDomain().UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
            {
                Exception ex = e.ExceptionObject as Exception;
                if (ex != null)
                {
                    ShowErrorMessage(ex);
                }
            };
        }
        // ユーザー・フレンドリなダイアログを表示するメソッド
        public static void ShowErrorMessage(Exception ex)
        {
            string inputformref_debuginfo = InputFormRef.GetDebugInfo();

            if (ex is FETextDecode.FETextException)
            {
                R.ShowStopError(ex.ToString());
                return;
            }

            {
                ErrorReportForm f = (ErrorReportForm)InputFormRef.JumpFormLow<ErrorReportForm>();
                f.SetException(ex, inputformref_debuginfo);
                f.ShowDialog();
            }
        }



        public static Form MainForm()
        {
            if (Program.ROM.RomInfo.version == 0)
            {
                return InputFormRef.JumpFormLow<MainFE0Form>();
            }

            if (OptionForm.first_form() == OptionForm.first_form_enum.EASY 
                && U.stringbool(U.at(ArgsDic, "--force-detail","0")) == false )
            {
                return InputFormRef.JumpFormLow<MainSimpleMenuForm>();
            }

            if (Program.ROM.RomInfo.version == 6)
            {
                return InputFormRef.JumpFormLow<MainFE6Form>();
            }
            else if (Program.ROM.RomInfo.version == 7)
            {
                return InputFormRef.JumpFormLow<MainFE7Form>();
            }
            return InputFormRef.JumpFormLow<MainFE8Form>();
        }

        public static string OpenROMDialog()
        {
            string title = R._("編集するROMを選択してください");
            string filter = R._("ROMs|*.gba;*.ups|GBA ROMs|*.gba|UPS files|*.ups|GBA.7Z|*.gba.7z|ROMRebuild|*.rebuild|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            if (LastSelectedFilename != null)
            {
                LastSelectedFilename.Load(null, "rom", open);
            }
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return "";
            }
            if (!U.CanReadFileRetry(open))
            {
                return "";
            }
            return open.FileNames[0];
        }

        static bool LoadROMLow(string fullfilename, string forceversion)
        {
            if (! U.CanReadFileRetry(fullfilename))
            {
                return false;
            }

            string version = "";
            bool r = false;
            ROM rom = new ROM();
            if (forceversion == "")
            {
                r = rom.Load(fullfilename, out version);
                if (r)
                {
                    ROM = rom;
                    return true;
                }
                if (U.GetFileSize(fullfilename) >= 1024 * 1024 * 8)
                {//8MB以上ならROMの可能性あり
                    ErorrUnknownROM f = (ErorrUnknownROM)InputFormRef.JumpFormLow<ErorrUnknownROM>();
                    f.Init(version);
                    f.ShowDialog();

                    forceversion = f.GetResultVersion();
                }
            }

            if (forceversion != "")
            {//強制バージョン指定
                r = rom.LoadForceVersion(fullfilename, forceversion);
                if (r)
                {
                    ROM = rom;
                    return true;
                }
                version = forceversion;
            }

            R.ShowStopError("未対応のROMです。\r\ngame version={0}\r\nSize={1}"
                , version
                , U.To0xHexString((uint)U.GetFileSize(fullfilename))
            );
            return false;
        }

        //ROMを開く.
        public static bool LoadROM(string fullfilename, string forceversion)
        {
            //キャッシュスレッドが動いていたら止める
            if (AsmMapFileAsmCache != null)
            {
                AsmMapFileAsmCache.Join();
            }

            //必ず絶対パスに直します. 相対パスがあると後で参照するときに都合が悪い.
            fullfilename = Path.GetFullPath(fullfilename);
            if (!File.Exists(fullfilename))
            {
                return false;
            }
            //最後に利用したファイルを記録する機能を初期化. 何度もリテイクするだろうからしやすいようにする.
            LastSelectedFilename = new LastSelectedFilename(fullfilename);

            try
            {
                bool r = LoadROMLow(fullfilename, forceversion);
                if (!r)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                R.ShowStopError(R.ExceptionToString(e));
                return false;
            }

            InitSystem(fullfilename);
            return true;
        }

        //仮想ROMを開く.
        public static bool LoadVirtualROM(ROM rom,string srcfilename)
        {
            //キャッシュスレッドが動いていたら止める
            if (AsmMapFileAsmCache != null)
            {
                AsmMapFileAsmCache.Join();
            }
            //最後に利用したファイルを記録する機能を初期化. 何度もリテイクするだろうからしやすいようにする.
            LastSelectedFilename = new LastSelectedFilename(srcfilename);

            ROM = rom;
            rom.SetVirtualROMFlag(srcfilename);
            InitSystem("");
            return true;
        }

        //ROM読みこみに伴うシステムの初期化.
        static void InitSystem(string fullfilename)
        {
            Log.Notify("InitSystem:", Path.GetFileName(ROM.Filename), "ver:", ROM.RomInfo.VersionToFilename, "length:", ROM.Data.Length.ToString("X"), "FEBuilderGBA:", U.getVersion());

            //Undoバッファの準備
            Undo = new Undo();


            //数を求める部分はあまりにたくさん呼び出すのでキャッシュすることにしました.
            InputFormRef.ClearCacheDataCount();
            //パッチのインストールの是非の判定 FE8には策パッチがあるのでキャッシュする.
            PatchForm.ClearCheckIF();

            if (fullfilename != "")
            {
                //変更監視
                UpdateWatcher.RegistMain(fullfilename);
            }


            //tbl適応判定
            OptionForm.AutoUpdateTBLOption();

            //システム側のテキストエンコード どうやってUnicodeにするかどうか.
            ReBuildSystemTextEncoder();

            //FEテキストエンコード用のハフマンツリーマップの構成
            ReBuildFETextEncoder();

            //イベントの読込
            ReLoadEventScript();
               
            //イベント条件の解釈リスト
            EventCondForm.PreLoadResource(U.ConfigDataFilename("eventcond_"));

            //AI1 と 2, 3
            EventUnitForm.PreLoadResourceAI1(U.ConfigDataFilename("ai1_"));
            EventUnitForm.PreLoadResourceAI2(U.ConfigDataFilename("ai2_"));
            EventUnitForm.PreLoadResourceAI3(U.ConfigDataFilename("ai3_"));

            //アイテム 利用効果リスト
            ItemForm.PreLoadResource_item_weapon_effect(U.ConfigDataFilename("item_weapon_effect_"));
            ItemForm.PreLoadResource_item_staff_use_effect(U.ConfigDataFilename("item_staff_use_effect_"));

            //SondEffectリスト
            SongTableForm.PreLoadResource(U.ConfigDataFilename("sound_"));
            //UnitActionリスト
            UnitActionPointerForm.PreLoadResource(U.ConfigDataFilename("unitaction_"));

            //ROM内アニメ
            ImageRomAnimeForm.PreLoadResource();
            ImageTSAAnimeForm.PreLoadResource();
            if (Program.ROM.RomInfo.version == 8)
            {
                ImageTSAAnime2Form.PreLoadResource();
            }

            //MODの読込.
            ReLoadMod();

            //新規に追加ユニットリストキャッシュの削除
            EventUnitForm.ClearNewData();

            //システムアイコンキャッシュのクリア
            ImageSystemIconForm.ClearCache();

            //EVENTとASMのキャッシュをクリア
            AsmMapFileAsmCache = new FEBuilderGBA.AsmMapFileAsmCache();
            //asm mapキャッシュの更新.
            AsmMapFileAsmCache.ClearCache();

            //RAM
            ReBuildRAM();

            if (fullfilename != ""
                && fullfilename != Program.Config.at("Last_Rom_Filename"))
            {//最後に開いたファイル名を保存する.
                Program.Config["Last_Rom_Filename"] = fullfilename;
                Program.Config.Save();
            }
            //Log.Notify("InitSystem:Complate");
        }

        public static string GetLastROMFilename()
        {
            return Program.Config.at("Last_Rom_Filename");
        }

        public static void ReBuildFETextEncoder()
        {
            FETextEncoder = new FETextEncode();
        }
        static void ReBuildSystemTextEncoder()
        {
            SystemTextEncoder = new SystemTextEncoder();
        }
        static void ReLoadTranslateResource()
        {
            MyTranslateResource.LoadResource(System.IO.Path.Combine(BaseDirectory, "config", "translate", OptionForm.lang() + ".txt"));
        }

        public static void ReLoadSetting()
        {
            if (Program.ROM == null)
            {//ROMを読込んでいない場合は、システムの初期化だけ行う

                //システム側のテキストエンコード いかにしてUnicodeにするかどうか.
                ReBuildSystemTextEncoder();

                //多言語切り替え
                ReLoadTranslateResource();

                return;
            }


            if (AsmMapFileAsmCache != null)
            {
                //探索スレッドが動いているとまずいので停止させる.
                AsmMapFileAsmCache.Join();
            }

            //件数キャッシュの再構築
            InputFormRef.ClearCacheDataCount();

            //システム側のテキストエンコード いかにしてUnicodeにするかどうか.
            ReBuildSystemTextEncoder();

            //多言語切り替え
            ReLoadTranslateResource();

            //FEテキストエンコード用のハフマンツリーマップの構成
            ReBuildFETextEncoder();

            //イベントの読込
            ReLoadEventScript();

            //MODの読込.
            ReLoadMod();

            //ASMMapの再構築
            //以前のデータが残っているとまずいので完全に捨てて再作成します.
            AsmMapFileAsmCache = new FEBuilderGBA.AsmMapFileAsmCache();
            //asm mapキャッシュの更新.
            AsmMapFileAsmCache.ClearCache();
        }

        static void ReLoadEventScript()
        {
            FlagCache = new EtcCacheFLag();
            UseTextIDCache = new EtcCacheTextID();
            LintCache = new EtcCache("lint_");
            CommentCache = new EtcCache("comment_");
            WorkSupportCache = new EtcCache("worksupport_");
            ExportFunction = new ExportFunction();
            TextEscape = new FEBuilderGBA.TextEscape();

            EventScript = new EventScript();
            EventScript.Load(EventScript.EventScriptType.Event);

            ProcsScript = new EventScript(8);
            ProcsScript.Load(EventScript.EventScriptType.Procs);

            AIScript = new EventScript(16);
            AIScript.Load(EventScript.EventScriptType.AI);
        }
        static void ReLoadMod()
        {
            Mod = new Mod();
            Mod.Load();
        }
        static void ReBuildRAM()
        {
            if (RAM != null)
            {
                RAM.DisConnect();
                RAM = null;
            }
            RAM = new RAM();
        }
        static bool CheckConfigDirectory()
        {
            string[] dir_list = new string[]{
                 Path.Combine(BaseDirectory, "config")
                ,Path.Combine(BaseDirectory, "config", "data")
                ,Path.Combine(BaseDirectory, "config", "patch2")
                ,Path.Combine(BaseDirectory, "config", "translate")
            };
            for (int i = 0; i < dir_list.Length; i++)
            {
                string configdir = dir_list[i];
                if (!Directory.Exists(configdir))
                {
#if DEBUG
                    R.ShowStopError("The Config directory does not exist.\r\nIf you built the program, copy the Config directory under the Debug directory.\r\nConfigディレクトリがありません。\r\nあなたがプログラムをビルドした場合は、Debugディレクトリの下にConfigディレクトリをコピーしてください。\r\n\r\n{0}", configdir); ///No Translate
#else
                    R.ShowStopError("The Config directory does not exist.\r\nYou have failed to decompress 7z. Please try again.\r\nConfigディレクトリがありません。\r\n7zの解凍に失敗しています。やり直してください。\r\n\r\n{0}", configdir); ///No Translate
#endif
                    return false;
                }
            }
            return true;
        }
        static bool Check7ZipDLL()
        {
            string sevenzip = System.IO.Path.Combine(BaseDirectory, "7-zip32.dll");
            if (!File.Exists(sevenzip))
            {
#if DEBUG
                R.ShowStopError("The 7-zip32.dll does not exist.\r\nIf you built the program, copy the 7-zip32.dll under the Debug directory.\r\n7-zip32.dllがありません。\r\nあなたがプログラムをビルドした場合は、Debugディレクトリの下に7-zip32.dllをコピーしてください。\r\n\r\n{0}", sevenzip); ///No Translate
#else
                R.ShowStopError("The 7-zip32.dll directory does not exist.\r\nYou have failed to extract 7z. Please try unzip again.\r\n7-zip32.dllがありません。\r\n7zの解凍に失敗しています。やり直してください。\r\n\r\n{0}", sevenzip); ///No Translate
#endif
                return false;
            }
            return true;
        }
        //メインスレッド判定
        public static bool IsMainThread()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId == MainThreadID;
        }

        public static ROM ROM { get; private set; }
        public static string BaseDirectory { get; private set; }
        public static EventScript EventScript { get; private set; }
        public static EventScript ProcsScript { get; private set; }
        public static EventScript AIScript { get; private set; }
        public static Undo Undo { get; private set; }
        public static Config Config { get; private set; }
        public static ROMUpdateWatcher UpdateWatcher { get; private set; }
        public static FETextEncode FETextEncoder { get; private set; }
        public static SystemTextEncoder SystemTextEncoder { get; private set; }
        public static LastSelectedFilename LastSelectedFilename { get; private set; }
        public static Mod Mod { get; private set; }
        public static EtcCacheTextID UseTextIDCache { get; private set; }
        public static EtcCacheFLag FlagCache { get; private set; }
        public static EtcCache LintCache { get; private set; }
        public static EtcCache CommentCache { get; private set; }
        public static EtcCache WorkSupportCache { get; private set; }
        public static ExportFunction ExportFunction { get; private set; }
        public static AsmMapFileAsmCache AsmMapFileAsmCache { get; private set; }
        public static RAM RAM { get; private set; }
        public static TextEscape TextEscape { get; private set; }
        public static bool IsCommandLine { get; private set; }
        public static bool DoReOpen = false;
        static int MainThreadID;
    }
}
