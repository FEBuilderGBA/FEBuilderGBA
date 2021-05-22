using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

namespace FEBuilderGBA
{
    public class EventScript
    {
        public enum ArgType
        {
              None   //特になし
            , FIXED  //固定
            , MAPX   //MAPX マップ 16x16チップ
            , MAPY   //MAPY
            , MAPXY
            , WMAPX   //MAPX マップ 16x16チップ
            , WMAPY   //MAPY
            , SCREENX   //SCREENX 画面 320x240
            , SCREENY   //SCREENY
            , SCREENXY
            , UNIT   //ユニット
            , CLASS  //CLASS
            , POINTER_PROCS //PROCポインタ
            , POINTER_ASM  //ASMポインタ
            , POINTER_UNIT  //配置増援ポインタ
            , POINTER_EVENT //イベント
            , POINTER_EVENTBATTLEDATA //イベント戦闘データ(FE7,FE6のみ)
            , POINTER_EVENTMOVEDATA //イベント配置座標データ(FE7,FE6のみ)
            , POINTER_TALKGROUP //会話グループ(FE7のみ)
            , POINTER_MENUEXTENDS //分岐拡張
            , POINTER_AICOORDINATE //AIの座標
            , POINTER_AIUNIT4      //AIのユニット4人
            , POINTER_AICALLTALK      //敵AIから会話イベントを実行する
            , POINTER_UNITSSHORTTEXT    //UNITと一致するshort型のテキスト
            , POINTER //その他ポインタ
            , MENUCOMMAND //チュートリアルで無効にするメニュー
            , MAPCHAPTER //マップ名
            , MUSIC //音楽
            , SOUND //効果音
            , TEXT //テキスト
            , CONVERSATION_TEXT //会話テキスト
            , SYSTEM_TEXT //システムテキスト
            , ONELINE_TEXT //一行テキスト
            , ITEM //アイテム
            , WMLOCATION //ワールドマップ拠点
            , WMPATH //ワールドマップの道
            , BG //背景
            , CG //CG (10分割)
            , AI1
            , AI2
            , AI3
            , AI4
            , DIRECTION //方向(00:← 01:→ 02:↓ 03↑)
            , FOG       //霧 (00:霧無し FF:マップ初期値)
            , WEATHER   //天気 (00:晴れ 01:雪 02:吹雪 04:雨 05:マグマ 06:砂嵐)
            , UNIT_COLOR//色(00:元 01:青 02:赤 03:緑 04:セピア)
            , IF_CONDITIONAL //条件ID分岐
            , LABEL_CONDITIONAL //条件ID分岐とび先ラベル
            , GOTO_CONDITIONAL //条件ID分岐とび先GOTOラベル
            , EARTHQUAKE //地震(00=横揺 01=縦揺)
            , ATTACK_TYPE //00:当てる 01:必殺 02:外す
            , WMREGION //世界地図の地名 (00=フレリア 01=グラド 02=ジャハナ 03=カルチノ 04=闇の樹海 05=ロストン 06=ポカラ 07=ルネス
            , WMENREGION //世界地図の英文字表記 for FE6
            , WMSTYLE1
            , WMSTYLE2
            , WMSTYLE3
            , PORTRAIT //顔画像
            , REVPORTRAIT //顔画像左右反転
            , PORTRAIT_DIRECTION //顔画像消去方法
            , FLAG //フラグ
            , AFFILIATION //所属 00で自軍(青)、40で友軍(緑)、80で敵軍(赤)
            , MAGVELY   //FE8の大陸移動 X軸は固定でY軸が-8 ～ 52 の範囲に動く
            , WMAPAFFILIATION //ワールドマップでの所属 00で自軍(青)、01で敵軍(赤)、02で友軍(緑)
            , WMAP2AFFILIATION //FE7所属2
            , EVENTUNITPOS //連続移動指定で利用するイベントユニット互換の位置設定
            , DECIMAL   //10進数の値
            , PROBABILITY //確率
            , MEMORYSLOT //メモリスロットFE8
            , PACKED_MEMORYSLOT //メモリスロットFE8 パック形式
            , POINTER_AIUNIT //AIで使用する ushort型のユニットリスト
            , POINTER_TEXT //C言語文字列表記 6Cで登場
            , POINTER_AITILE //AIで使用するBYTE型タイルリスト
            , RAM_UNIT_STATE     //ユニットの状態変更 FE7->(01＝透明にする 02＝行動済みにする 03＝透明にした後行動済みにする)
            , WMAP_SPRITE_ID    //ワールドマップに表示するスプライト番号
            , EVBIT             //EVBIT FE8のみ
            , EVBIT_MODIFY      //EVBIT_MODIFY FE8のみ
            , BADSTATUS         //状態異常 ターン <<8 | 状態
            , SKILL             //
            , MAPEMOTION        //マップ絵文字
            , COUNTER           //カウンタ(ビットシフト)
            , RAM_UNIT_PARAM    //RAMユニット 設定名
            , RAM_UNIT_VALUE    //RAMユニット 設定値
            , BOOL              //BOOL値型
            , TRAP              //RAM罠
            , FSEC              //フレーム秒
            , TILE              //マップのタイル名
            , EDITION           //編
            , DIFFICULTY        //難易度
            , SUPPORT_LEVEL     //支援レベル
            , GAMEOPTION        //ゲームオプション
            , GAMEOPTION_VALUE  //ゲームオプションの値
            , DISABLEOPTIONS    //メニューを無効にするビットフラグ FE8
            , DISABLEWEAPONS    //アイテムメニューを無効にするビットフラグ FE8
            , IGNORE_KEYS       //キー操作を無効にするビットフラグ FE8
            , KEYS              //キーコード
            , UNITCLASSABILITY  //ユニットクラス特性 ビットフラグ
            , MAP_CHANGE
        };

        public class Arg
        {
            public String  Name;
            public int    Position;
            public int    Size;
            public ArgType Type;
            public char    Symbol;     //XとかYとかZとかのシンボル
            public uint    Alias;      //不要ならU.NOT_FOUND 既に定義されている別名への参照
        }

        //探索を早くするため、スクリプトの種類を簡単に定義.
        public enum ScriptHas
        {
              NOTHING //特に探索に使うような値は持っていません.
            , UNKNOWN //不明
            , POINTER_UNIT_OR_EVENT //配置増援ポインタ
            , POINTER_OTHER //ポインタ
            , IF_CONDITIONAL //条件ID分岐
            , LABEL_CONDITIONAL //条件ID分岐とび先ラベル
            , GOTO_CONDITIONAL //条件ID分岐とび先GOTOラベル
            , MAP //MAP切替 読込
            , TEXT //テキストID
            , TERM //終端命令
            , MAPTERM //終端命令 FE7のみワールドマップイベントの終端
            , PATCH_EXTENDS //パッチによって拡張されました
            , GIVE_ITEM //アイテムをくれる.
        };

        public class Script{
            public byte[] Data;          //データ
            public String[] Info;        //説明

            public Arg[] Args;

            public ScriptHas Has;        //探索を早くするためもっている属性や種類を簡単にまとめます.
            public int Size;

            public string Category; //選びやすいように分類を作る.
            public string PopupHint; //ポップアップヒント
            public string LowCode;  //1234XXXX みたいな設定ファイルに書いてあるコード
            public bool IsExdends;  //パッチで追加されたコマンド
            public bool IsLowCommand; //危険なLOWコマンド
        };
        public Script[] Scripts { get; private set; }
        public Script Unknown { get; private set; }

        public EventScript(int unk_size = 4)
        {
            Init(unk_size);
        }
        void Init(int unk_size)
        {
           Script unknownscript = new Script();
           unknownscript.Data = new byte[4];
           unknownscript.Args = new Arg[0];
           unknownscript.Info = SplitInfo("WORD");
           unknownscript.Has = ScriptHas.UNKNOWN;
           unknownscript.Size = unk_size;
           unknownscript.Category = "";
           unknownscript.PopupHint = "";
           unknownscript.LowCode = "";
           this.Unknown = unknownscript;
        }

        public enum EventScriptType
        {
            Event
            ,Event_without_Patch
            ,Procs
            ,AI
        }
        public void Load(EventScriptType eventScriptType)
        {
            string fullfilename;
            if (eventScriptType == EventScriptType.Procs)
            {
                fullfilename = U.ConfigDataFilename("6c_script_");
            }
            else if (eventScriptType == EventScriptType.AI)
            {
                fullfilename = U.ConfigDataFilename("aiscript_");
            }
            else 
            {
                fullfilename = U.ConfigDataFilename("event_");
            }

            this.Load(fullfilename, eventScriptType);
        }
        void Load(string fullfilename , EventScriptType eventScriptType)
        {
            List<Script> scripts = new List<Script>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                this.Scripts = scripts.ToArray();
                return;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    Script script = ParseScriptLine(line);
                    if (script == null)
                    {
                        continue;
                    }
                    scripts.Add(script);
                }
            }

            if (eventScriptType == EventScriptType.Event)
            {
                //パッチに登録されているイベントのスキャン(少し重いです)
                Dictionary<uint, string> flags = new Dictionary<uint,string>();
                PatchForm.MakeEventScript(scripts, flags, Program.TextEscape, Program.ExportFunction);
                Program.FlagCache.MargeFlags(flags);
            }
//            else if (eventScriptType == EventScriptType.AI)
//            {
//                //パッチに登録されているイベントのスキャン(少し重いです)
//                PatchForm.MakeAIScript(scripts);
//            }

            //でかいイベントからマッチさせていきたいので、スクリプト長で、降順にソートする
            scripts.Sort((a, b) => 
            {
                if (b.Size != a.Size)
                {
                    return (int)(b.Size - a.Size);
                }

                //拡張命令がある場合、優先したいのでボーナスを追加.
                int argALength = a.Args.Length - (a.IsExdends ? 3 : 0);
                int argBLength = b.Args.Length - (b.IsExdends ? 3 : 0);

                //スクリプト長が同じなら引数が少ない順番で
                if (argALength != argBLength)
                {
                    return (int)(argALength - argBLength);
                }

                //引数数が同じなら、引数のサイズ順.
                return sumArgsSize(a.Args) - sumArgsSize(b.Args);
            });
            this.Scripts = scripts.ToArray();
        }
        static uint SelectAlias(List<Arg> args , char symbol)
        {
            for (int i = 0; i < args.Count; i++)
            {
                if (args[i].Symbol == symbol)
                {
                    return (uint)i;
                }
            }
            return U.NOT_FOUND;
        }

        public static Script ParseScriptLine(string line)
        {
            string[] sp = line.Split('\t');
            if (sp.Length <= 1)
            {
                return null;
            }
            string bytestring = sp[0];
            string infostring = sp[1];
            if (bytestring.Length < 8)
            {
                return null;
            }
            if (bytestring.Length % 8 != 0)
            {
                return null;
            }

            byte[] data = new byte[bytestring.Length / 2];

            List<Arg> args = new List<Arg>();
            for (int i = 0; i < bytestring.Length; )
            {
                if (bytestring[i] > 'F' && bytestring[i + 1] > 'F')
                {//変数
                    data[i / 2] = 0;
                    int start = i;
                    char symbol = bytestring[start];
                    for (i = i + 2; i < bytestring.Length; i += 2)
                    {
                        if (bytestring[i] != symbol)
                        {
                            break;
                        }
                        data[i / 2] = 0;
                    }
                    Arg arg = new Arg();
                    arg.Position = start / 2;
                    arg.Size = (i - start) / 2;
                    arg.Symbol = bytestring[start];
                    ParseInfoString(infostring, arg.Symbol, out arg.Type, out arg.Name);

                    if ( ErrorCheckArg(arg,line) == false)
                    {
                        continue;
                    }
                    arg.Alias = SelectAlias(args, bytestring[start]);

                    args.Add(arg);
                }
                else
                {//定数
                    data[i / 2] = (byte)U.atoh(bytestring.Substring(i, 2));
                    int start = i;
                    for (i = i + 2; i < bytestring.Length; i += 2)
                    {
                        if (bytestring[i] > 'F' && bytestring[i + 1] > 'F')
                        {
                            break;
                        }
                        data[i / 2] = (byte)U.atoh(bytestring.Substring(i, 2));
                    }
                    Arg arg = new Arg();
                    arg.Position = start / 2;
                    arg.Size = (i - start) / 2;
                    arg.Type = ArgType.FIXED;
                    arg.Alias = U.NOT_FOUND;
                    args.Add(arg);
                }
            }
            int size = bytestring.Length / 2;

            Script script = new Script();
            script.Info = SplitInfo(infostring);
            script.Data = data;
            script.Args = args.ToArray();
            script.Has = MakeScriptHas(script.Args, script.Info, line);
            script.Size = size;
            script.LowCode = sp[0];
            ParsePopupHintAndCategory(sp, out script.PopupHint, out script.Category , out script.IsLowCommand);

            return script;
        }

        static bool ErrorCheckArg(Arg arg,string line)
        {
            if (IsPointerArgs(arg.Type))
            {
                if (arg.Size < 4)
                {
                    R.ShowStopError("引数{0}({1})は、ポインタなのに、4バイトのサイズが確保されていません。{2}bytesです。\r\n{3}",arg.Name,arg.Symbol,arg.Size,line);
                    return false;
                }
            }
            //OK
            return true;
        }
        static void ParsePopupHintAndCategory(string[] sp
            , out string out_popup
            , out string out_category
            , out bool out_isLowCommand)
        {
            out_popup = "";
            out_category = "";
            out_isLowCommand = false;

            int i = 2;
            if (sp.Length < i)
            {
                return;
            }
            for (; i < sp.Length; i++)
            {
                if (sp[i].Length <= 0)
                {
                    continue;
                }
                if (sp[i][0] == '{')
                {
                    out_category += sp[i];
                    continue;
                }
                if (out_popup.Length > 0)
                {
                    out_popup += "\r\n";
                }
                out_popup += ReplaceHintWord(sp[i]);
            }

            if (sp[1].IndexOf("(LOW)") >= 0)
            {
                if (out_popup.Length > 0)
                {
                    out_popup += "\r\n";
                }
                out_popup += R._("この命令は低レイヤーの命令です。通常は利用しないでください。");

                out_isLowCommand = true;
            }
        }

        //引数のサイズを全部合計する(引数の数が同じならば、引数の数が少ない方を優先するため)
        static int sumArgsSize(Arg[] args)
        {
            int sum = 0;
            for (int i = 0; i < args.Length; i++)
            {
                sum += args[i].Size;
            }
            return sum;
        }



        static string Cut(string line, string start, string ifend)
        {
            int gameKeyword = line.IndexOf(start);
            if (gameKeyword < 0)
            {
                return "";
            }
            string l = line.Substring(gameKeyword + start.Length);
            if (ifend == null)
            {
                return l;
            }

            int term = l.IndexOf(ifend);
            if (term >= 0)
            {
                l = l.Substring(0, term);
            }
            return l;
        }
        public static void ParseInfoString(string infostring, char symbol, out ArgType type, out String name)
        {
             type = ArgType.None;
             name = symbol.ToString();
             string sym = Cut(infostring,"[" + name,"]");
             if (sym.Length <= 0)
             {
                 return ;
             }

             string[] sp = sym.Split(':');
             if ( sp.Length < 2)
             {
                name = sp[0];
                return ;
             }

             switch(sp[1].ToUpper())
             {
             case "FIXED":
                 type = ArgType.FIXED;
                 break;
             case "MAPX":
                 type = ArgType.MAPX;
                 break;
             case "MAPY":
                 type = ArgType.MAPY;
                 break;
             case "MAPXY":
                 type = ArgType.MAPXY;
                 break;
             case "WMAPX":
                 type = ArgType.WMAPX;
                 break;
             case "WMAPY":
                 type = ArgType.WMAPY;
                 break;
             case "SCREENX":
                 type = ArgType.SCREENX;
                 break;
             case "SCREENY":
                 type = ArgType.SCREENY;
                 break;
             case "SCREENXY":
                 type = ArgType.SCREENXY;
                 break;
             case "UNIT":
                 type = ArgType.UNIT;
                 break;
             case "CLASS":
                 type = ArgType.CLASS;
                 break;
             case "POINTER_ASM":
                 type = ArgType.POINTER_ASM;
                 break;
             case "POINTER_UNIT":
                 type = ArgType.POINTER_UNIT;
                 break;
             case "POINTER_EVENT":
                 type = ArgType.POINTER_EVENT;
                 break;
             case "POINTER_EVENTBATTLEDATA":
                 type = ArgType.POINTER_EVENTBATTLEDATA;
                 break;
             case "POINTER_EVENTMOVEDATA":
                 type = ArgType.POINTER_EVENTMOVEDATA;
                 break;
             case "POINTER_TALKGROUP":
                 type = ArgType.POINTER_TALKGROUP;
                 break;
             case "POINTER_MENUEXTENDS":
                 type = ArgType.POINTER_MENUEXTENDS;
                 break;
             case "POINTER_AICOORDINATE":
                 type = ArgType.POINTER_AICOORDINATE;
                 break;
             case "POINTER_AIUNIT4":
                 type = ArgType.POINTER_AIUNIT4;
                 break;
             case "POINTER_AICALLTALK":
                 type = ArgType.POINTER_AICALLTALK;
                 break;
             case "POINTER_UNITSSHORTTEXT":
                 type = ArgType.POINTER_UNITSSHORTTEXT;
                 break;
             case "POINTER":
                 type = ArgType.POINTER;
                 break;
             case "MENUCOMMAND":
                 type = ArgType.MENUCOMMAND;
                 break;
             case "MAPCHAPTER":
                 type = ArgType.MAPCHAPTER;
                 break;
             case "MUSIC":
                 type = ArgType.MUSIC;
                 break;
             case "SOUND":
                 type = ArgType.SOUND;
                 break;
             case "CONVERSATION_TEXT":
                 type = ArgType.CONVERSATION_TEXT;
                 break;
             case "SYSTEM_TEXT":
                 type = ArgType.SYSTEM_TEXT;
                 break;
             case "ONELINE_TEXT":
                 type = ArgType.ONELINE_TEXT;
                 break;
             case "TEXT":
                 type = ArgType.TEXT;
                 break;
             case "ITEM":
                 type = ArgType.ITEM;
                 break;
             case "WMLOCATION":
                 type = ArgType.WMLOCATION;
                 break;
             case "WMPATH":
                 type = ArgType.WMPATH;
                 break;
             case "BG":
                 type = ArgType.BG;
                 break;
             case "CG":
                 type = ArgType.CG;
                 break;
             case "AI1":
                 type = ArgType.AI1;
                 break;
             case "AI2":
                 type = ArgType.AI2;
                 break;
             case "AI3":
                 type = ArgType.AI3;
                 break;
             case "AI4":
                 type = ArgType.AI4;
                 break;
             case "DIRECTION":
                 type = ArgType.DIRECTION;
                 break;
             case "FOG":
                 type = ArgType.FOG;
                 break;
             case "WEATHER":
                 type = ArgType.WEATHER;
                 break;
             case "UNIT_COLOR":
                 type = ArgType.UNIT_COLOR;
                 break;
             case "IF_CONDITIONAL":
                 type = ArgType.IF_CONDITIONAL;
                 break;
             case "LABEL_CONDITIONAL":
                 type = ArgType.LABEL_CONDITIONAL;
                 break;
             case "GOTO_CONDITIONAL":
                 type = ArgType.GOTO_CONDITIONAL;
                 break;
             case "EARTHQUAKE":
                 type = ArgType.EARTHQUAKE;
                 break;
             case "ATTACK_TYPE":
                 type = ArgType.ATTACK_TYPE;
                 break;
             case "WMSTYLE1":
                 type = ArgType.WMSTYLE1;
                 break;
             case "WMSTYLE2":
                 type = ArgType.WMSTYLE2;
                 break;
             case "WMSTYLE3":
                 type = ArgType.WMSTYLE3;
                 break;
             case "WMENREGION":
                 type = ArgType.WMENREGION;
                 break;
             case "WMREGION":
                 type = ArgType.WMREGION;
                 break;
             case "PORTRAIT":
                 type = ArgType.PORTRAIT;
                 break;
             case "REVPORTRAIT":
                 type = ArgType.REVPORTRAIT;
                 break;
             case "PORTRAIT_DIRECTION":
                 type = ArgType.PORTRAIT_DIRECTION;
                 break;
             case "FLAG":
                 type = ArgType.FLAG;
                 break;
             case "AFFILIATION":
                 type = ArgType.AFFILIATION;
                 break;
             case "WMAPAFFILIATION":
                 type = ArgType.WMAPAFFILIATION;
                 break;
             case "WMAP2AFFILIATION":
                 type = ArgType.WMAP2AFFILIATION;
                 break;
             case "EVENTUNITPOS":
                 type = ArgType.EVENTUNITPOS;
                 break;
             case "MAGVELY":
                 type = ArgType.MAGVELY;
                 break;
             case "DECIMAL":
                 type = ArgType.DECIMAL;
                 break;
             case "PROBABILITY":
                 type = ArgType.PROBABILITY;
                 break;
             case "RAM_UNIT_STATE":
                 type = ArgType.RAM_UNIT_STATE;
                 break;
             case "WMAP_SPRITE_ID":
                 type = ArgType.WMAP_SPRITE_ID;
                 break;
             case "EVBIT":
                 type = ArgType.EVBIT;
                 break;
             case "EVBIT_MODIFY":
                 type = ArgType.EVBIT_MODIFY;
                 break;
             case "BADSTATUS":
                 type = ArgType.BADSTATUS;
                 break;
             case "SKILL":
                 type = ArgType.SKILL;
                 break;
             case "MAPEMOTION":
                 type = ArgType.MAPEMOTION;
                 break;
             case "COUNTER":
                 type = ArgType.COUNTER;
                 break;
             case "FSEC":
                 type = ArgType.FSEC;
                 break;
             case "TILE":
                 type = ArgType.TILE;
                 break;
             case "EDITION":
                 type = ArgType.EDITION;
                 break;
             case "DIFFICULTY":
                 type = ArgType.DIFFICULTY;
                 break;
             case "SUPPORT_LEVEL":
                 type = ArgType.SUPPORT_LEVEL;
                 break;
             case "GAMEOPTION":
                 type = ArgType.GAMEOPTION;
                 break;
             case "GAMEOPTION_VALUE":
                 type = ArgType.GAMEOPTION_VALUE;
                 break;
             case "DISABLEOPTIONS":
                 type = ArgType.DISABLEOPTIONS;
                 break;
             case "DISABLEWEAPONS":
                 type = ArgType.DISABLEWEAPONS;
                 break;
             case "IGNORE_KEYS":
                 type = ArgType.IGNORE_KEYS;
                 break;
             case "UNITCLASSABILITY":
                 type = ArgType.UNITCLASSABILITY;
                 break;
             case "KEYS":
                 type = ArgType.KEYS;
                 break;
             case "MAP_CHANGE":
                 type = ArgType.MAP_CHANGE;
                 break;
             case "RAM_UNIT_PARAM":
                 type = ArgType.RAM_UNIT_PARAM;
                 break;
             case "RAM_UNIT_VALUE":
                 type = ArgType.RAM_UNIT_VALUE;
                 break;
             case "BOOL":
                 type = ArgType.BOOL;
                 break;
             case "TRAP":
                 type = ArgType.TRAP;
                 break;
             case "MEMORYSLOT":
                 type = ArgType.MEMORYSLOT;
                 break;
             case "PACKED_MEMORYSLOT":
                 type = ArgType.PACKED_MEMORYSLOT;
                 break;
             case "POINTER_AIUNIT":
                 type = ArgType.POINTER_AIUNIT;
                 break;
             case "POINTER_AITILE":
                 type = ArgType.POINTER_AITILE;
                 break;
             case "POINTER_TEXT":
                 type = ArgType.POINTER_TEXT;
                 break;
             case "POINTER_PROCS":
                 type = ArgType.POINTER_PROCS;
                 break;
             }
             if ( sp.Length >= 3)
             {
                name = sp[2];
             }
        }
        static ScriptHas MakeScriptHas(Arg[] args,string[] info, string srcLine)
        {
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i] == "[TERM]")
                {
                    return ScriptHas.TERM; //終端命令
                }
                if (info[i] == "[MAPTERM]")
                {
                    return ScriptHas.MAPTERM; //ワールドマップイベント終端命令
                }
            }

            for (int i = 0; i < args.Length; i++)
            {
                Arg arg = args[i];
                if (arg.Type == ArgType.IF_CONDITIONAL)
                {
                    return ScriptHas.IF_CONDITIONAL; //条件ID分岐
                }
                if (arg.Type == ArgType.LABEL_CONDITIONAL)
                {
                    return ScriptHas.LABEL_CONDITIONAL; //条件ID分岐とび先ラベル
                }
                if (arg.Type == ArgType.GOTO_CONDITIONAL)
                {
                    return ScriptHas.GOTO_CONDITIONAL; //条件ID分岐とび先GOTOラベル
                }
                if (arg.Type == ArgType.POINTER_UNIT || arg.Type == ArgType.POINTER_EVENT)
                {
                    return ScriptHas.POINTER_UNIT_OR_EVENT; //配置増援ポインタまたはイベント
                }
                if (EventScript.IsPointerArgs(arg.Type) )
                {
                    return ScriptHas.POINTER_OTHER; //その他ポインタ
                }
                if (arg.Type == ArgType.MAPCHAPTER)
                {
                    return ScriptHas.MAP; //マップ切り替え
                }
                if (arg.Type == ArgType.TEXT
                    || arg.Type == EventScript.ArgType.CONVERSATION_TEXT
                    || arg.Type == EventScript.ArgType.SYSTEM_TEXT
                    || arg.Type == EventScript.ArgType.ONELINE_TEXT
                    )
                {
                    return ScriptHas.TEXT; //テキストID
                }
            }

            if (srcLine.IndexOf("{ITEM}") > 0)
            {//アイテムをくれる系
                for (int i = 0; i < args.Length; i++)
                {
                    Arg arg = args[i];
                    if (arg.Type == ArgType.ITEM)
                    {
                        return ScriptHas.GIVE_ITEM; //アイテムを得るイベント.
                    }
                }
            }

            //特に探索に使うような値は持っていません.
            return ScriptHas.NOTHING;
        }

        public class OneCode
        {
            public byte[] ByteData;
            public Script Script;
            public uint JisageCount;
            public string Comment;
        };

        //コピー
        public static OneCode CloneCode(OneCode code)
        {
            OneCode c = new OneCode();
            c.ByteData = (byte[])code.ByteData.Clone();
            c.Script = code.Script; //Scriptは書き換えないからそのまま C++ の constがほしい..
            c.JisageCount = code.JisageCount;
            c.Comment = code.Comment;
            return c;
        }

        public OneCode DisAseemble(byte[] data,uint startaddr)
        {
            startaddr = U.toOffset(startaddr);

            OneCode code = new OneCode();
            uint leftsize = ((uint)data.Length) - startaddr;
            for(int i = 0 ; i < Scripts.Length ; i++)
            {
                Script script = Scripts[i];

                if ( leftsize < script.Size )
                {//長さが足りないのだから、マッチするわけない.
                    continue;
                }

                //マッチテスト
                uint pos = 0;
                int n = 0;
                for( ; n < script.Args.Length ; n++)
                {
                    Arg arg = script.Args[n];
                    if (arg.Type == ArgType.FIXED)
                    {
                         int m = 0;
                         for( ; m < arg.Size ; m ++)
                         {
                             if (data.Length <= startaddr + pos)
                             {//途中でデータがなくなった.
//                                 Debug.Assert(false);
                                 break;
                             }
                             if (data[startaddr + pos] != script.Data[pos])
                             {
                                 break;
                             }
                             pos ++;
                         }
                         if (m < arg.Size)
                         {//途中で不一致があった
                             break;
                         }
                     }
                     else
                     {//変数
                         if (IsPointerArgsOrNULL(arg.Type))
                         {
                             uint vp = U.u32(data, startaddr + pos);
                             if (vp != 0 && U.isSafetyPointer(vp) == false)
                             {//不一致.
                                 break;
                             }
                         }
                         else if (IsPointerArgs(arg.Type))
                         {//ポインタ型であればポインタであることを確認する.
                             uint vp = U.u32(data, startaddr + pos);
                             if (U.isSafetyPointer(vp) == false)
                             {//不一致.
                                 break;
                             }
                         }
                         pos += (uint)arg.Size;
                     }
                }
                if(n >= script.Args.Length)
                {//マッチ
                    if (data.Length < startaddr
                        || data.Length < startaddr + script.Size)
                    {//データが足りないのでマッチしない.
                        Debug.Assert(false);
                        continue;
                    }
                    code.ByteData    = U.getBinaryData(data, startaddr, script.Size);
                    code.Script      = script;
                    code.JisageCount = 0;
                    code.Comment = Program.CommentCache.At(startaddr);
                    return code;
                }
            }


            //見つからない不明な命令.
            if (data.Length < this.Unknown.Size
                || data.Length < startaddr
                || data.Length < startaddr + this.Unknown.Size)
            {
                code.ByteData = new byte[4];
            }
            else
            {
                code.ByteData = U.getBinaryData(data, startaddr, this.Unknown.Size);
            }
            code.Script      = this.Unknown;
            code.JisageCount = 0;
            code.Comment = Program.CommentCache.At(startaddr);
            return code;
        }

        //HINTを最優先探索.
        public OneCode DisAseemble(byte[] data, uint startaddr,string hint)
        {
            if (hint == "")
            {
                return DisAseemble(data, startaddr);
            }
            OneCode code = new OneCode();
            uint leftsize = ((uint)data.Length) - startaddr;
            for (int i = 0; i < Scripts.Length; i++)
            {
                Script script = Scripts[i];

                if (leftsize != script.Size)
                {//長さが違うのだから、マッチするわけない.
                    continue;
                }

                string info = EventScript.makeCommandComboText(script,false);
                if (info == hint)
                {//ヒント区に書いてあるものとマッチ.
                    code.ByteData = U.getBinaryData(data, startaddr, script.Size);
                    code.Script = script;
                    code.JisageCount = 0;
                    code.Comment = Program.CommentCache.At(startaddr);
                    return code;
                }
            }
            //見つからないので、通常探索
            return DisAseemble(data, startaddr);
        }

        public static uint GetArgPointer(OneCode code, int arg_count,uint base_pointer)
        {
            Arg arg = code.Script.Args[arg_count];
            if (arg.Size != 4)
            {
                return U.NOT_FOUND;
            }
            return base_pointer + (uint)arg.Position;
        }

        public static uint GetArgValue(OneCode code, Arg arg)
        {
            if (arg.Size == 1)
            {//1バイト
                if (code.ByteData.Length <= arg.Position + 0)
                {
                    return 0;
                }
                else
                {
                    return U.u8(code.ByteData, (uint)arg.Position);
                }
            }
            else if (arg.Size == 2)
            {//2バイト
                if (code.ByteData.Length <= arg.Position + 1)
                {
                    return 0;
                }
                else
                {
                    return U.u16(code.ByteData, (uint)arg.Position);
                }
            }
            else if (arg.Size == 3)
            {//3バイト
                if (code.ByteData.Length <= arg.Position + 2)
                {
                    return 0;
                }
                else
                {
                    return U.u24(code.ByteData, (uint)arg.Position);
                }
            }
            else //if (arg.Size == 4)
            {//4バイト
                if (code.ByteData.Length <= arg.Position + 3)
                {
                    return 0;
                }
                else
                {
                    return U.u32(code.ByteData, (uint)arg.Position);
                }
            }
        }

        public static string GetArg(OneCode code, int arg_count, out uint v)
        {
            Debug.Assert(code.Script.Args.Length > arg_count);

            Arg arg = code.Script.Args[arg_count];
            v = GetArgValue(code, arg);

            if (IsSigned(arg.Type))
            {
                Debug.Assert(IsDecimal(arg.Type)); //符号を表示するなら10進数であるはずだ
                if (arg.Size == 1)
                {//1バイト
                    return ((sbyte)v).ToString();
                }
                else if (arg.Size == 2)
                {//2バイト
                    return ((short)v).ToString();
                }
                else //if (arg.Size == 4)
                {//4バイト
                    Debug.Assert(arg.Size != 3);//未対応
                    return ((int)v).ToString();
                }
            }
            if (IsDecimal(arg.Type))
            {
                return v.ToString();
            }
            return "0x" + v.ToString("X");
        }

        //10進数表記?
        public static bool IsDecimal(EventScript.ArgType type)
        {
            return (type == EventScript.ArgType.MAPX
                || type == EventScript.ArgType.MAPY
                || type == EventScript.ArgType.SCREENX
                || type == EventScript.ArgType.SCREENY
                || type == EventScript.ArgType.WMAPX
                || type == EventScript.ArgType.WMAPY
                || type == EventScript.ArgType.PROBABILITY
                || type == EventScript.ArgType.DECIMAL
                || type == EventScript.ArgType.MAGVELY
                || type == EventScript.ArgType.FSEC
                );
        }
        //符号を使ってマイナスを表示するかどうか
        public static bool IsSigned(EventScript.ArgType type)
        {
            return (type == EventScript.ArgType.MAGVELY);
        }

        static string[] SplitInfo(string info)
        {
            List<string> list = new List<string>();

            int pos = 0;
            while (pos < info.Length)
            {
                int kako_start = info.IndexOf("[", pos);
                if (kako_start < 0)
                {
                    list.Add(info.Substring(pos));
                    return list.ToArray();
                }

                int kako_end = info.IndexOf("]", pos + 1);
                if (kako_end < 0)
                {//なぜか閉じかっこがない
                    list.Add(info.Substring(pos));
                    return list.ToArray();
                }
                else
                {
                    list.Add(info.Substring(pos, kako_start - pos));
                    list.Add(info.Substring(kako_start, kako_end + 1 - kako_start));
                    pos = kako_end + 1;
                }
            }
            return list.ToArray();
        }

        //イベントの終端までサーチして長さを特定
        public static uint SearchEveneLength(byte[] romdata,uint start_addr,bool isWorldMapEvent = false)
        {
            uint lastBranchAddr = 0;
            int unknown_count = 0;
            uint addr = start_addr;
            while (true)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(romdata, addr);
                if (EventScript.IsExitCode(code, addr, lastBranchAddr))
                {//終端命令
                    addr += (uint)code.Script.Size;
                    break;
                }
                else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                {
                    unknown_count++;
                    if (unknown_count > 30)
                    {//不明命令が30個連続して続いたら打ち切る
                        break;
                    }
                }
                else
                {//不明でない命令
                    unknown_count = 0;

                    if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                    {//LABEL
                        lastBranchAddr = 0;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                    {//IF
                        lastBranchAddr = addr;
                    }
                }
                addr += (uint)code.Script.Size;
            }

            return addr - start_addr;
        }

        public static string makeCommandComboText(EventScript.Script sc, bool appendBinCode)
        {
            if (sc.Info.Length < 1)
            {
                Debug.Assert(false);
                return "";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(sc.Info[0]);
            for (int i = 1; i < sc.Info.Length; )
            {
                int v = sc.Info[i].IndexOf(':');
                if (v > 0)
                {
                    sb.Append("[");
                    v++; //skip :
                    if (sc.Info[i][v] == ':')
                    {
                        v++; //skip : more.
                    }
                    sb.Append(sc.Info[i].Substring(v) );
                }
                i++;
                if (i >= sc.Info.Length)
                {
                    break;
                }
                sb.Append(sc.Info[i]);
                i++;
            }
            if (appendBinCode)
            {
                sb.Append(" //" + sc.LowCode);
            }
            return sb.ToString();
        }

        public static List<EventScript.OneCode> CloneEventList(List<EventScript.OneCode> events)
        {
            List<EventScript.OneCode> eventAsm = new List<EventScript.OneCode>();

            int length = events.Count;
            for (int i = 0; i < length; i++)
            {
                eventAsm.Add(EventScript.CloneCode(events[i]));
            }
            return eventAsm;
        }

        //イベントから呼び出される特殊指定の領域を調べます.
        public static void MakeEventASMMAPList(List<Address> list)
        {
            MakeEventASMMAPList(list, false, "CALL_ASM_FROM_EVENT ",false);
            MakeEventASMMAPList(list, true, "CALL_EVENT ", false);
        }

        public static void MakeEventASMMAPList(List<Address> list, bool isEventOnly, string prefix, bool isPointerOnly)
        {
            List<uint> tracelist = new List<uint>();
            for (int i = 0; i < Program.EventScript.Scripts.Length; i++)
            {
                Script script = Program.EventScript.Scripts[i];
                int length = script.Data.Length / 4;
                for (int n = 0; n < length; n++)
                {
                    uint addr = U.u32(script.Data, (uint)n * 4);
                    if (!U.isSafetyPointer(addr))
                    {
                        continue;
                    }

                    string name = string.Join("", script.Info);
                    if ((addr & 0x01) == 0x01)
                    {//thumb
                        if (isEventOnly == true)
                        {
                            continue;
                        }
                        addr = U.toOffset(addr);
                        addr = DisassemblerTrumb.ProgramAddrToPlain(addr);

                        FEBuilderGBA.Address.AddAddress(list, addr, 0, U.NOT_FOUND, "CALL_ASM_FROM_EVENT " + name, Address.DataTypeEnum.ASM);
                    }
                    else
                    {//普通のイベント命令
                        if (isEventOnly == false)
                        {
                            continue;
                        }
                        if (isPointerOnly)
                        {
                            addr = U.toOffset(addr);
                            FEBuilderGBA.Address.AddAddress(list, addr, 0, U.NOT_FOUND, "CALL_EVENT " + name, Address.DataTypeEnum.EVENTSCRIPT);
                        }
                        else
                        {
                            uint eventPointer = U.GrepPointer(Program.ROM.Data, addr, 0x0500000);
                            if (eventPointer != U.NOT_FOUND)
                            {//イベント探索はポインタが必要なので、探す...
                                EventScriptForm.ScanScript( list, eventPointer, true, false, "CALL_EVENT " + name , tracelist);
                            }
                            else
                            {
                                addr = U.toOffset(addr);
                                FEBuilderGBA.Address.AddAddress(list, addr, 0, U.NOT_FOUND, "CALL_EVENT " + name, Address.DataTypeEnum.EVENTSCRIPT);
                            }
                        }
                    }
                }
            }
        }
        public static int GetAffiliation(EventScript.OneCode code, int current)
        {
            uint v;
            uint match = 0; //見つからない場合は自軍
            int distance = int.MaxValue; //距離の近さ

            for (int n = 0; n < code.Script.Args.Length; n++)
            {
                int d = Math.Abs(n - current);
                if (d > distance)
                {//もっと近いところでも発見している
                    continue;
                }
                if (d == 0)
                {//自分自身を見ても仕方ない.
                    continue;
                }

                EventScript.Arg arg = code.Script.Args[n];
                if (arg.Type == EventScript.ArgType.AFFILIATION)
                {
                    //所属 00で自軍(青)、40で友軍(緑)、80で敵軍(赤)
                    EventScript.GetArg(code, n, out v);
                    if (v == 0)
                    {//自軍
                        match = 0;
                        distance = d;
                    }
                    else if (v == 0x40)
                    {//友軍
                        match = 1;
                        distance = d;
                    }
                    else if (v == 0x80)
                    {//敵軍
                        match = 2;
                        distance = d;
                    }
                }
                else if (arg.Type == EventScript.ArgType.WMAPAFFILIATION)
                {
                    EventScript.GetArg(code, n, out v);
                    if (Program.ROM.RomInfo.version() == 7)
                    {
                        //00＝自軍通常 01＝自軍移動中 02＝自軍選択時　
                        //20＝敵軍通常 21＝敵軍移動中 22＝敵軍選択時
                        //40＝友軍通常 41＝友軍移動中 42＝友軍選択時
                        if (v >= 0 && v <= 02)
                        {//自軍
                            match = 0;
                            distance = d;
                        }
                        else if (v >= 0x40 && v <= 0x42)
                        {//友軍
                            match = 1;
                            distance = d;
                        }
                        else if (v >= 0x20 && v <= 0x22)
                        {//敵軍
                            match = 2;
                            distance = d;
                        }
                    }
                    else
                    {
                        //ワールドマップでの所属 00で自軍(青)、01で敵軍(赤)、02で友軍(緑)
                        if (v == 0)
                        {//自軍
                            match = 0;
                            distance = d;
                        }
                        else if (v == 0x2)
                        {//友軍
                            match = 1;
                            distance = d;
                        }
                        else if (v == 0x1)
                        {//敵軍
                            match = 2;
                            distance = d;
                        }
                    }
                }
                else if (arg.Type == EventScript.ArgType.WMAP2AFFILIATION)
                {
                    EventScript.GetArg(code, n, out v);
                    if (Program.ROM.RomInfo.version() == 7)
                    {
                        if (v == 0x1C)
                        {//自軍
                            match = 0;
                            distance = d;
                        }
                        else if (v == 0x1E)
                        {//友軍
                            match = 1;
                            distance = d;
                        }
                        else if (v == 0x1D)
                        {//敵軍
                            match = 2;
                            distance = d;
                        }
                    }
                    else
                    {
                    }
                }
                else if (arg.Type == EventScript.ArgType.UNIT_COLOR)
                {
                    //色(00:元 01:青 02:赤 03:緑 04:セピア)
                    EventScript.GetArg(code, n, out v);
                    if (v == 1)
                    {//青
                        match = 0;
                        distance = d;
                    }
                    else if (v == 0x3)
                    {//友軍
                        match = 1;
                        distance = d;
                    }
                    else if (v == 0x2)
                    {//敵軍
                        match = 2;
                        distance = d;
                    }
                    else if (v == 0x4)
                    {//セピア
                        match = 3;
                        distance = d;
                    }
                }
            }
            return (int)match;
        }
        static string ReplaceHintWord(string hint)
        {
            if (hint == "@STOREC")
            {
                return R._("結果はメモリスロットCに格納されます。");
            }
            else if (hint == "@READ1")
            {
                return R._("値はメモリスロット1の値が使われます。");
            }
            else if (hint == "@CMP_C_IMM")
            {
                return R._("直前の条件式結果が格納されるメモリスロットCと、指定した即値の数値を比較します。\r\n数字格納用にメモリスロット7を利用します。");
            }
            else if (hint == "@CMP_C")
            {
                return R._("直前の条件式結果が格納されるメモリスロットCと、指定したメモリスロットの値を比較します。");
            }
            else if (hint == "@FADESPEED")
            {
                return R._("10(最速)>>>>>01");
            }
            else if (hint == "@MOVESPEED")
            {
                return R._("Speedは、移動する速度を指定します。\r\n大きい数字を入れるほど早く移動します。\r\n注意:移動できないタイルへ移動させるとフリーズします。\r\n例えば、ジェネラルは山を越えられないので、山のタイルへ移動せよと命令するとフリーズします。");
            }
            else if (hint == "@MNC2BUG")
            {
                return R._("マップによってはワールドマップから入らなければ、フリーズするものがあります。\r\nその場合は、「フリーマップから入らないとフリーズする制約を消す」パッチを利用してください。");
            }
            else if (hint == "@MNCHBUG")
            {
                return R._("ワールドマップを利用する場合は、ワールドマップの設定を正しく行ってください。\r\n設定に失敗すると、序章に戻ったり、フリーズする原因になります。");
            }
            else if (hint == "@WARNING_DISA")
            {
                return R._("自軍のパーティにいるメンバーに対してDISAを使うと存在が抹消されます。\r\nユニットを一時的に離脱させて消したい場合は、REMUを利用してください。そして、後でREVEALで再加入させてください。\r\n演出でユニットを消す場合は、CLEAを推奨します。CLEAは、すべてのユニットを消去しますが、抹消も、離脱状態にもなりません。");
            }
            else if (hint == "@EXPLAIN_REMU")
            {
                return R._("ユニットを一時的にパーティから外します。\r\nユニットのデータは保存され非表示扱いになります。\r\nユニットと再開し復帰させるには、REVEALを利用してください。\r\nEAでは、INVISと書かれることもありますが、REMUはINVISの別名です。");
            }
            else if (hint == "@EXPLAIN_REVEAL")
            {
                return R._("REMU(INVIS)で一時的にパーティから外したユニットをパーティに再び復帰させます。");
            }
            else if (hint == "@WAIT")
            {
                return R._("指定したウェイト処理を行います");
            }
            else if (hint == "@SAFETY")
            {
                return R._("Safetyは、プレイヤーから攻撃される危険なタイルに立ち寄らなくなる数字です。\r\n小さい数字ほど危険なタイルに立ち寄りません。");
            }
            
            return hint.Replace("\\r\\n", "\r\n");
        }
        public static bool IsPointerArgs(ArgType argtype)
        {
            return
                   argtype == EventScript.ArgType.POINTER
                || argtype == EventScript.ArgType.POINTER_ASM
                || argtype == EventScript.ArgType.POINTER_PROCS
                || argtype == EventScript.ArgType.POINTER_EVENT
                || argtype == EventScript.ArgType.POINTER_UNIT
                || argtype == EventScript.ArgType.POINTER_EVENTBATTLEDATA
                || argtype == EventScript.ArgType.POINTER_EVENTMOVEDATA
                || argtype == EventScript.ArgType.POINTER_TALKGROUP
                || argtype == EventScript.ArgType.POINTER_MENUEXTENDS
                || argtype == EventScript.ArgType.POINTER_AICOORDINATE
                || argtype == EventScript.ArgType.POINTER_AIUNIT
                || argtype == EventScript.ArgType.POINTER_AITILE
                || argtype == EventScript.ArgType.POINTER_AIUNIT4
                || argtype == EventScript.ArgType.POINTER_AICALLTALK
                || argtype == EventScript.ArgType.POINTER_UNITSSHORTTEXT
                ;
        }

        public static bool IsPointerArgsOrNULL(ArgType argtype)
        {
            return
                   argtype == EventScript.ArgType.POINTER_UNITSSHORTTEXT
                ;
        }

/*
        void FixEAMix(string fullfilename)
        {
            string[] lines = File.ReadAllLines(fullfilename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                string[] sp = line.Split('\t');
                if (sp.Length < 2)
                {
                    continue;
                }
                if (sp[0].Length < 4)
                {
                    continue;
                }

                uint command;
                string command1 = sp[0].Substring(0, 2);
                if (Program.ROM.RomInfo.version() == 8)
                {
                    string command2 = sp[0].Substring(2, 2);
                    command = U.atoh(command2 + command1);
                }
                else
                {
                    command = U.atoh(command1);
                }

                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte())
                {//FE7の場合コマンド位置がずれるので補正する
                    if (command <= 0x16)
                    {//何もなし
                    }
                    else if (command <= 0x17)
                    {//+1 
                        command = command + 1;
                    }
                    else if (command <= 0xA6)
                    {//+2
                        command = command + 2;
                    }
                    else if (command <= 0xCC)
                    {//-1
                        command = command - 1;
                    }
                    else if (command <= 0xDB)
                    {//-3
                        command = command - 3;
                    }
                    else
                    {//-3
                        command = command - 4;
                    }
                }

                string eaname = FindEAName(command);
                if (eaname.Length <= 0)
                {
                    continue;
                }
                if (eaname[0] == '_' || eaname.IndexOf("Chapters") > 0 || eaname == "ASM" || eaname == "ASMC" || eaname == "ASMC2" || eaname == "CALL" || eaname == "SVAL")
                {
                    continue;
                }
                if (sp[1].IndexOf(eaname) >= 0)
                {
                    continue;
                }
                sp[1] += "(" + eaname + ")";
                //書き戻し
                lines[i] = string.Join("\t", sp);
            }
            File.WriteAllLines(fullfilename + ".new.txt", lines);
        }
        string FindEAName(uint command)
        {
            string commandstr;
            if (Program.ROM.RomInfo.version() == 8)
            {
                commandstr = "0x" + command.ToString("X04").ToUpper();
            }
            else
            {
                commandstr = "0x" + command.ToString("X02").ToUpper();
            }
            string gametype = Program.ROM.RomInfo.TitleToFilename();

            string[] files = U.Directory_GetFiles_Safe(@"C:\dropbox\FE8Edit7743\sample\Event Assembler V11.0.1\Language Raws\", "*.txt", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i]);
                for (int n = 0; n < lines.Length; n++)
                {
                    string line = lines[n];
                    if (line.IndexOf(commandstr) >= 0 && line.IndexOf(gametype) >= 0)
                    {
                        int c = line.IndexOf(',');
                        if (c <= 0)
                        {
                            continue;
                        }
                        return line.Substring(0, c);
                    }
                }
            }
            return "";
        }
*/
        //指定したリストの位置にあるイベントのアドレスを求める.
        public static uint ConvertSelectedToAddr(uint startAddr, int index, List<EventScript.OneCode> codes)
        {
            uint addr = startAddr;
            int maxIndex = Math.Min(index, codes.Count);
            for (int i = 0; i < maxIndex; i++)
            {
                Debug.Assert(codes[i].Script.Size == codes[i].ByteData.Length);
                addr += (uint)codes[i].Script.Size;
            }
            return addr;
        }
        public static int ConvertAddrToSelected(List<EventScript.OneCode> eventAsm, uint addr, uint currnt_event)
        {
            uint a = addr;
            for (int i = 0; i < eventAsm.Count; i++)
            {
                Debug.Assert(eventAsm[i].Script.Size == eventAsm[i].ByteData.Length);
                a += (uint)eventAsm[i].Script.Size;

                if (a > currnt_event)
                {
                    return i;
                }
            }

            //不明なので選択できない.
            return -1;
        }

        [MethodImpl(256)]
        public static bool IsExitCode(EventScript.OneCode code, uint addr, uint lastBranchAddr)
        {
            if (code.Script.Has != EventScript.ScriptHas.TERM
                && code.Script.Has != EventScript.ScriptHas.MAPTERM)
            {//終端ではない
                return false;
            }

            if (lastBranchAddr == 0)
            {//ダミーではない本当の終端...?
                if (Program.ROM.RomInfo.version() == 8 && IsFE8DummyEnd(code,addr) )
                {//ダミー終端
                    return false;
                }
                return true;
            }
            Debug.Assert(addr >= lastBranchAddr);
            if (addr - lastBranchAddr >= 0x28)
            {//ダミーではない
             //壊れたイベントがあると怖いので、ある程度の長さで探索を打ち切る.
                return true;
            }
            //おそらくダミーの終端. 無視するべき.
            return false;
        }

        static bool IsFE8DummyEnd(EventScript.OneCode code, uint addr)
        {
            if (code.Script.Has != EventScript.ScriptHas.TERM)
            {//終端ではない
                return false;
            }
            uint nextAddr = addr + (uint)code.Script.Size;
            if (nextAddr + 0x4 >= Program.ROM.Data.Length)
            {//ROM終端
                return false;
            }
            uint a1 = Program.ROM.u8(nextAddr + 0);
            uint a2 = Program.ROM.u8(nextAddr + 1);
            uint a3 = Program.ROM.u8(nextAddr + 2);
            uint a4 = Program.ROM.u8(nextAddr + 3);
//            if (! (a1 == 0x20 && a2 == 0x08 && a3 == 0x09 && a4 == 0x00))
//            {//ダミー終端ではない
//                return false;
//            }
            if (!(a1 == 0x20 && a2 == 0x08))
            {//ダミー終端ではない
                return false;
            }
            uint a5 = (Program.ROM.u8(nextAddr + 4) & 0xF0);
            if (! (a5 == 0x20 || a5 == 0x40))
            {//ダミー終端ではない
                return false;
            }
            //ダミー終端
            return true;
        }

        public static bool IsFixedArg(EventScript.Arg arg)
        {
            if (arg.Type == EventScript.ArgType.FIXED)
            {//固定値になっているところはパラメータを出さない.
                return true;
            }
            if (arg.Alias != U.NOT_FOUND)
            {//Aliasになっているところはパラメータを出さない.
                return true;
            }
            return false;
        }
        public static uint GetRAMUnitParamIndex(EventScript.OneCode code)
        {
            for (int n = 0; n < code.Script.Args.Length; n++)
            {
                EventScript.Arg arg = code.Script.Args[n];
                if (arg.Type == EventScript.ArgType.RAM_UNIT_PARAM)
                {
                    return (uint)n;
                }
            }
            return 0x0;
        }
        public static uint GetGameOptionParamIndex(EventScript.OneCode code)
        {
            for (int n = 0; n < code.Script.Args.Length; n++)
            {
                EventScript.Arg arg = code.Script.Args[n];
                if (arg.Type == EventScript.ArgType.GAMEOPTION)
                {
                    return (uint)n;
                }
            }
            return 0x0;
        }
        public static void SetDefaultFrameTo60(EventScript.Script script)
        {
            for(int i = 0 ; i < script.Args.Length ; i++ )
            {
                if (script.Args[i].Type == ArgType.FSEC)
                {
                    if ( script.Args[i].Size == 1)
                    {
                        U.write_u8(script.Data, (uint)script.Args[i].Position, 60);
                    }
                    else if (script.Args[i].Size == 2)
                    {
                        U.write_u16(script.Data, (uint)script.Args[i].Position, 60);
                    }
                    else if (script.Args[i].Size == 4)
                    {
                        U.write_u32(script.Data, (uint)script.Args[i].Position, 60);
                    }
                }
            }
        }

        //ポインタの更新の通知
        public static void NotifyChangePointer(List<EventScript.OneCode> list, uint oldaddr, uint newaddr)
        {
            if (!U.isPointer(oldaddr) || !U.isPointer(newaddr))
            {
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                EventScript.OneCode code = list[i];
                EventScript.NotifyChangePointer(code, oldaddr, newaddr);
            }
        }
        static void NotifyChangePointer(EventScript.OneCode code, uint oldaddr, uint newaddr)
        {
            if (!U.isPointer(oldaddr) || !U.isPointer(newaddr))
            {
                return;
            }

            EventScript.Arg[] args = code.Script.Args;
            for (int i = 0; i < args.Length; i++)
            {
                EventScript.Arg arg = args[i];
                if (!EventScript.IsPointerArgs(arg.Type))
                {
                    continue;
                }
                if (arg.Size != 4)
                {
                    continue;
                }
                uint p = U.u32(code.ByteData, (uint)arg.Position);
                if (p != oldaddr)
                {
                    continue;
                }
                U.write_u32(code.ByteData, (uint)arg.Position, newaddr);
            }

        }
    }
}
