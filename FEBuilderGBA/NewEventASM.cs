using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace FEBuilderGBA
{
    class NewEventASM
    {
        public enum ArgType
        {
              ArgType_None   //特になし
            , ArgType_FIXED  //固定
            , ArgType_X      //X
            , ArgType_Y      //Y
            , ArgType_UNIT   //ユニット
            , ArgType_CLASS  //CLASS
            , ArgType_POINTER_ASM  //ASMポインタ
            , ArgType_POINTER_UNIT  //配置増援ポインタ
            , ArgType_POINTER_EVENT //イベント
            , ArgType_POINTER //その他ポインタ
            , ArgType_MAPCHAPTER //マップ名
            , ArgType_MUSIC //音楽
            , ArgType_SOUND //効果音
            , ArgType_TEXT //テキスト
            , ArgType_ITEM //アイテム
            , ArgType_WMLOCATION //ワールドマップ拠点
            , ArgType_WMPATH //ワールドマップの道
            , ArgType_BG //背景
            , ArgType_AI1
            , ArgType_AI2
            , ArgType_AI3
            , ArgType_AI4
            , ArgType_DIRECTION //方向(00:← 01:→ 02:↓ 03↑)
            , ArgType_FOG       //霧 (00:霧無し FF:マップ初期値)
            , ArgType_WEATHER   //天気 (00:晴れ 01:雪 02:吹雪 04:雨 05:マグマ 06:砂嵐)
            , ArgType_UNIT_COLOR//色(00:元 01:青 02:赤 03:緑 04:セピア)
            , ArgType_IF_CONDITIONAL //条件ID分岐
            , ArgType_LABEL_CONDITIONAL //条件ID分岐とび先ラベル
            , ArgType_GOTO_CONDITIONAL //条件ID分岐とび先GOTOラベル
            , ArgType_EARTHQUAKE //地震(00=横揺 01=縦揺)
            , ArgType_ATTACK_TYPE //00:当てる 01:必殺 02:外す
            , ArgType_WMREGION //世界地図の地名 (00=フレリア 01=グラド 02=ジャハナ 03=カルチノ 04=闇の樹海 05=ロストン 06=ポカラ 07=ルネス
            , ArgType_PORTRAIT //顔画像

        };

        public class Arg
        {
            public String  Name;
            public int    Position;
            public int    Size;
            public ArgType Type;
            public char    Symbol;     //XとかYとかZとかのシンボル
        }

        //探索を早くするため、スクリプトの種類を簡単に定義.
        public enum ScriptHas
        {
              ScriptHas_NOTHING //特に探索に使うような値は持っていません.
            , ScriptHas_UNKNOWN //不明
            , ScriptHas_POINTER_UNIT //配置増援ポインタ
            , ScriptHas_POINTER_EVENT //イベント
            , ScriptHas_IF_CONDITIONAL //条件ID分岐
            , ScriptHas_LABEL_CONDITIONAL //条件ID分岐とび先ラベル
            , ScriptHas_GOTO_CONDITIONAL //条件ID分岐とび先GOTOラベル
        };


        public class Script{
            public byte[] Data;          //データ
            public String[] Info;        //説明

            public Arg[] Args;

            public ScriptHas Has;        //探索を早くするためもっている属性や種類を簡単にまとめます.
            public int Size;
        };
        public Script[] Scripts { get; private set; }
        public Script Unknown { get; private set; }

        public NewEventASM()
        {
           Script unknownscript = new Script();
           unknownscript.Data = new byte[4];
           unknownscript.Args = new Arg[1];
           unknownscript.Info = SplitInfo("WORD [XXXXXXXX::BIN]");
           unknownscript.Has = ScriptHas.ScriptHas_UNKNOWN;
           unknownscript.Size = 4;
           Arg arg = new Arg();
           arg.Size = 4;
           arg.Name = "BIN";
           arg.Symbol = 'X';
           unknownscript.Args[0] = arg;
           this.Unknown = unknownscript;

        }

        public void Load(string fullfilename)
        {
            List<Script> scripts = new List<Script>();

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] sp = line.Split('\t');
                    if (sp.Length <= 1)
                    {
                        continue;
                    }
                    string bytestring = sp[0];
                    string infostring = sp[1];
                    if (bytestring.Length < 8)
                    {
                        continue;
                    }
                    if (bytestring.Length%8!=0)
                    {
                        continue;
                    }
                    
                    byte[] data = new byte[bytestring.Length/2];
                    
                    List<Arg> args = new List<Arg>();
                    for (int i = 0; i < bytestring.Length; )
                    {
                        if (bytestring[i] > 'F' && bytestring[i + 1] > 'F')
                        {//変数
                             data[i/2] = 0;
                             int start = i;
                             char symbol = bytestring[start];
                             for (i = i + 2; i < bytestring.Length; i += 2)
                             {
                                 if (bytestring[i] != symbol)
                                 {
                                     break;
                                 }
                                 data[i/2] = 0;
                             }
                             Arg arg = new Arg();
                             arg.Position = start / 2;
                             arg.Size = (i - start) /2;
                             arg.Symbol = bytestring[start];
                             ParseInfoString(infostring,arg.Symbol,out arg.Type,out arg.Name);

                             args.Add(arg);
                        }
                        else
                        {//定数
                             data[i/2] = (byte)U.atoh(bytestring.Substring(i,2));
                             int start = i;
                             for (i = i + 2; i < bytestring.Length; i+=2)
                             {
                                 if (bytestring[i] > 'F' && bytestring[i+1] > 'F')
                                 {
                                     break;
                                 }
                                 data[i / 2] = (byte)U.atoh(bytestring.Substring(i, 2));
                             }
                             Arg arg = new Arg();
                             arg.Position = start / 2;
                             arg.Size = (i - start)/2;
                             arg.Type = ArgType.ArgType_FIXED;
                             args.Add(arg);
                        }
                    }
                    int size = bytestring.Length / 2;

                    Script script = new Script();
                    script.Info = SplitInfo(infostring);
                    script.Data = data;
                    script.Args = args.ToArray();
                    script.Has = MakeScriptHas(script.Args);
                    script.Size = size;
                    scripts.Add(script);
                }
            }
            scripts.Sort((a, b) => { return (int)(b.Size - a.Size); });
            this.Scripts = scripts.ToArray();
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
             type = ArgType.ArgType_None;
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
                 type = ArgType.ArgType_FIXED;
                 break;
             case "X":
                 type = ArgType.ArgType_X;
                 break;
             case "Y":
                 type = ArgType.ArgType_Y;
                 break;
             case "UNIT":
                 type = ArgType.ArgType_UNIT;
                 break;
             case "CLASS":
                 type = ArgType.ArgType_CLASS;
                 break;
             case "POINTER_ASM":
                 type = ArgType.ArgType_POINTER_ASM;
                 break;
             case "POINTER_UNIT":
                 type = ArgType.ArgType_POINTER_UNIT;
                 break;
             case "POINTER_EVENT":
                 type = ArgType.ArgType_POINTER_EVENT;
                 break;
             case "POINTER":
                 type = ArgType.ArgType_POINTER;
                 break;
             case "POINTER_MAPCHAPTER":
                 type = ArgType.ArgType_MAPCHAPTER;
                 break;
             case "MUSIC":
                 type = ArgType.ArgType_MUSIC;
                 break;
             case "SOUND":
                 type = ArgType.ArgType_SOUND;
                 break;
             case "TEXT":
                 type = ArgType.ArgType_TEXT;
                 break;
             case "ITEM":
                 type = ArgType.ArgType_ITEM;
                 break;
             case "WMLOCATION":
                 type = ArgType.ArgType_WMLOCATION;
                 break;
             case "WMPATH":
                 type = ArgType.ArgType_WMPATH;
                 break;
             case "BG":
                 type = ArgType.ArgType_BG;
                 break;
             case "AI1":
                 type = ArgType.ArgType_AI1;
                 break;
             case "AI2":
                 type = ArgType.ArgType_AI2;
                 break;
             case "AI3":
                 type = ArgType.ArgType_AI3;
                 break;
             case "AI4":
                 type = ArgType.ArgType_AI4;
                 break;
             case "DIRECTION":
                 type = ArgType.ArgType_DIRECTION;
                 break;
             case "FOG":
                 type = ArgType.ArgType_FOG;
                 break;
             case "WEATHER":
                 type = ArgType.ArgType_WEATHER;
                 break;
             case "UNIT_COLOR":
                 type = ArgType.ArgType_UNIT_COLOR;
                 break;
             case "IF_CONDITIONAL":
                 type = ArgType.ArgType_IF_CONDITIONAL;
                 break;
             case "LABEL_CONDITIONAL":
                 type = ArgType.ArgType_LABEL_CONDITIONAL;
                 break;
             case "GOTO_CONDITIONAL":
                 type = ArgType.ArgType_GOTO_CONDITIONAL;
                 break;
             case "EARTHQUAKE":
                 type = ArgType.ArgType_EARTHQUAKE;
                 break;
             case "ATTACK_TYPE":
                 type = ArgType.ArgType_ATTACK_TYPE;
                 break;
             case "WMREGION":
                 type = ArgType.ArgType_WMREGION;
                 break;
             case "PORTRAIT":
                 type = ArgType.ArgType_PORTRAIT;
                 break;
             }
             if ( sp.Length >= 3)
             {
                name = sp[2];
             }
        }
        ScriptHas MakeScriptHas(Arg[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Type == ArgType.ArgType_IF_CONDITIONAL)
                {
                    return ScriptHas.ScriptHas_IF_CONDITIONAL; //条件ID分岐
                }
                if (args[i].Type == ArgType.ArgType_LABEL_CONDITIONAL)
                {
                    return ScriptHas.ScriptHas_LABEL_CONDITIONAL; //条件ID分岐とび先ラベル
                }
                if (args[i].Type == ArgType.ArgType_GOTO_CONDITIONAL)
                {
                    return ScriptHas.ScriptHas_GOTO_CONDITIONAL; //条件ID分岐とび先GOTOラベル
                }
                if (args[i].Type == ArgType.ArgType_POINTER_UNIT)
                {
                    return ScriptHas.ScriptHas_POINTER_UNIT; //配置増援ポインタ
                }
                if (args[i].Type == ArgType.ArgType_POINTER_EVENT)
                {
                    return ScriptHas.ScriptHas_POINTER_EVENT; //イベント
                }
            }
            //特に探索に使うような値は持っていません.
            return ScriptHas.ScriptHas_NOTHING;
        }

        public class OneCode
        {
            public byte[] ByteData;
            public Script Script;
            public uint JisageCount;
        };

        public OneCode DisAseemble(byte[] data,uint startaddr)
        {
            OneCode code = new OneCode();
            uint leftsize = ((uint)data.Length) - startaddr;
            for(int i = 0 ; i < Scripts.Length ; i++)
            {
                if ( leftsize < Scripts[i].Size )
                {//長さが足りないのだから、マッチするわけない.
                    continue;
                }

                //マッチテスト
                int pos = 0;
                int n = 0;
                for( ; n < Scripts[i].Args.Length ; n++)
                {
                    if (Scripts[i].Args[n].Type == ArgType.ArgType_FIXED)
                     {
                         int m = 0;
                         for( ; m < Scripts[i].Args[n].Size ; m ++)
                         {
                             if (data[startaddr + pos] != Scripts[i].Data[pos])
                             {
                                 break;
                             }
                             pos ++;
                         }
                         if (m < Scripts[i].Args[n].Size)
                         {//途中で不一致があった
                             break;
                         }
                     }
                     else
                     {//変数
                         pos += Scripts[i].Args[n].Size;
                     }
                }
                if(n >= Scripts[i].Args.Length)
                {//マッチ
                     code.ByteData    = ROM.getBinaryData(data, startaddr, (uint)Scripts[i].Size);
                     code.Script      = Scripts[i];
                     code.JisageCount = 0;
                     return code;
                }
            }

            //見つからない不明な命令.
            code.ByteData    = ROM.getBinaryData(data, startaddr, (uint)this.Unknown.Size);
            code.Script      = this.Unknown;
            code.JisageCount = 0;
            return code;
        }
        public static uint GetArgValue(OneCode code, int arg_count)
        {
            Arg arg = code.Script.Args[arg_count];
            if (arg.Size == 1)
            {//1バイト
                return ROM.u8(code.ByteData, (uint)arg.Position);
            }
            else if (arg.Size == 2)
            {//2バイト
                return ROM.u16(code.ByteData, (uint)arg.Position);
            }
            else //if (arg.Size == 4)
            {//4バイト
                return ROM.u32(code.ByteData, (uint)arg.Position);
            }
        }

        public static string GetArg(OneCode code, int arg_count, out uint v)
        {
            Arg arg = code.Script.Args[arg_count];
            if (arg.Size == 1)
            {//1バイト
                v = ROM.u8(code.ByteData, (uint)arg.Position);
            }
            else if (arg.Size == 2)
            {//2バイト
                v = ROM.u16(code.ByteData, (uint)arg.Position);
            }
            else //if (arg.Size == 4)
            {//4バイト
                v = ROM.u32(code.ByteData, (uint)arg.Position);
            }

            if (arg.Size == 1)
            {//1バイト
                if (arg.Type == ArgType.ArgType_X || arg.Type == ArgType.ArgType_Y)
                {
                    return v.ToString();
                }
                return "0x" + v.ToString("X");
            }
            else if (arg.Size == 2)
            {//2バイト
                if (arg.Type == ArgType.ArgType_X || arg.Type == ArgType.ArgType_Y)
                {
                    return v.ToString();
                }
                return "0x" + v.ToString("X");
            }
            else //if (arg.Size == 4)
            {//4バイト
                if (arg.Type == ArgType.ArgType_X || arg.Type == ArgType.ArgType_Y)
                {
                    return v.ToString();
                }
                return "0x" + v.ToString("X");
            }
            //return v.ToString();
        }

        string[] SplitInfo(string info)
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
    }
}
