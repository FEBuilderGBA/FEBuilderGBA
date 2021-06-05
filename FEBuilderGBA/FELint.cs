using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace FEBuilderGBA
{
    public class FELint
    {
        public enum Type
        {
             EVENT_COND_TURN
            ,EVENT_COND_TALK
            ,EVENT_COND_OBJECT
            ,EVENT_COND_ALWAYS
            ,EVENT_COND_TUTORIAL
            ,EVENT_COND_TRAP
            ,EVENT_COND_PLAYER_UNIT
            ,EVENT_COND_ENEMY_UNIT
            ,EVENT_COND_START_EVENT
            ,EVENT_COND_END_EVENT
            ,EVENTSCRIPT
            ,EVENTUNITS
            ,MAPSETTING
            ,MAPSETTING_PLIST_OBJECT
            ,MAPSETTING_PLIST_CONFIG
            ,MAPSETTING_PLIST_PALETTE
            ,MAPSETTING_PLIST_MAP
            ,MAPSETTING_WORLDMAP
            ,MAPSETTING_PLIST_MAPCHANGE
            ,MAPSETTING_PLIST_ANIMETION1
            ,MAPSETTING_PLIST_ANIMETION2
            ,MAPSETTING_PLIST_EVENT
            ,WORLDMAP_EVENT
            ,BATTLE_ANIME
            ,BATTLE_ANIME_CLASS
            ,PORTRAIT
            ,BG
            ,HAIKU
            ,BATTTLE_TALK
            ,SUPPORT_TALK
            ,SUPPORT_UNIT
            ,MAPCHANGE
            ,SOUND_FOOT_STEPS
            ,UNIT
            ,CLASS
            ,ITEM
            ,ITEM_WEAPON_EFFECT
            ,MOVECOST_NORMAL //普通
            ,MOVECOST_RAIN //雨
            ,MOVECOST_SHOW //雪
            ,MOVECOST_AVOID //地形回避
            ,MOVECOST_DEF //地形防御
            ,MOVECOST_RES //地形魔防
            ,OP_CLASS_DEMO //オープニングデモ
            ,WMAP_BASE_POINT //ワールドマップ拠点
            ,SOUNDROOM //サウンドルーム
            ,SENSEKI //戦績
            ,DIC     //辞書
            ,MENU    //メニュー
            ,MENU_DEFINE //メニュー定義
            ,STATUS  //ステータス
            ,ED      //エンディング
            ,TERRAIN //地形
            ,SKILL_CONFIG   //スキル設定
            ,RMENU   //Rメニュー設定
            ,ITEM_USAGE_POINTER //アイテム利用効果
            ,PATCH   //パッチ
            ,MAPEXIT  //離脱ポイント
            ,IMAGE_UNIT_MOVE_ICON  //ユニット移動画像
            ,IMAGE_UNIT_WAIT_ICON  //ユニット待機画像
            ,ITEM_EEFECT_POINTER //アイテム間接効果ポインタ
            ,IMAGE_UNIT_PALETTE //ユニットパレット
            ,IMAGE_BATTLE_SCREEN //戦闘画面
            ,MAGIC_ANIME_EXTENDS //魔法拡張アニメ
            ,STATUS_GAME_OPTION //ゲームオプション
            ,STATUS_UNITS_MENU //部隊メニュー
            ,PROCS
            ,AISCRIPT
            ,ASM
            ,ASMDATA
            ,POINTER_TALKGROUP
            ,POINTER_MENUEXTENDS
            ,POINTER_UNITSSHORTTEXT
            ,SONGTABLE
            ,SONGTRACK
            ,SONGINST
            ,BOSS_BGM
            ,WORLDMAP_BGM
            ,EVENT_FINAL_SERIF
            ,ROM_HEADER
            ,TEXTID_FOR_SYSTEM  //テキストID システム予約
            ,TEXTID_FOR_USER    //テキストID ユーザ定義
            ,SE_SYSTEM  //SongID システム予約
            ,FELINTBUZY_MESSAGE //計測中
            ,FELINT_SYSTEM_ERROR   //FELintシステムエラー
        }
        public static EventCondForm.CONDTYPE TypeToEventCond(Type filterCondtype)
        {
            switch (filterCondtype)
            {
                case Type.EVENT_COND_TURN:
                    return EventCondForm.CONDTYPE.TURN;
                case Type.EVENT_COND_TALK:
                    return EventCondForm.CONDTYPE.TALK;
                case Type.EVENT_COND_OBJECT:
                    return EventCondForm.CONDTYPE.OBJECT;
                case Type.EVENT_COND_ALWAYS:
                    return EventCondForm.CONDTYPE.ALWAYS;
                case Type.EVENT_COND_TUTORIAL:
                    return EventCondForm.CONDTYPE.TUTORIAL;
                case Type.EVENT_COND_PLAYER_UNIT:
                    return EventCondForm.CONDTYPE.PLAYER_UNIT;
                case Type.EVENT_COND_ENEMY_UNIT:
                    return EventCondForm.CONDTYPE.ENEMY_UNIT;
                case Type.EVENT_COND_START_EVENT:
                    return EventCondForm.CONDTYPE.START_EVENT;
                case Type.EVENT_COND_END_EVENT:
                    return EventCondForm.CONDTYPE.END_EVENT;
                case Type.EVENT_COND_TRAP:
                    return EventCondForm.CONDTYPE.TRAP;
            }
            Debug.Assert(false);
            throw new Exception("Unknown Type Type:" + filterCondtype);
        }
        public static Type EventCondToType(EventCondForm.CONDTYPE filterCondtype)
        {
            switch (filterCondtype)
            {
                case EventCondForm.CONDTYPE.TURN:
                    return Type.EVENT_COND_TURN;
                case EventCondForm.CONDTYPE.TALK:
                    return Type.EVENT_COND_TALK;
                case EventCondForm.CONDTYPE.OBJECT:
                    return Type.EVENT_COND_OBJECT;
                case EventCondForm.CONDTYPE.ALWAYS:
                    return Type.EVENT_COND_ALWAYS;
                case EventCondForm.CONDTYPE.TUTORIAL:
                    return Type.EVENT_COND_TUTORIAL;
                case EventCondForm.CONDTYPE.PLAYER_UNIT:
                    return Type.EVENT_COND_PLAYER_UNIT;
                case EventCondForm.CONDTYPE.ENEMY_UNIT:
                    return Type.EVENT_COND_ENEMY_UNIT;
                case EventCondForm.CONDTYPE.START_EVENT:
                    return Type.EVENT_COND_START_EVENT;
                case EventCondForm.CONDTYPE.END_EVENT:
                    return Type.EVENT_COND_END_EVENT;
                case EventCondForm.CONDTYPE.TRAP:
                    return Type.EVENT_COND_TRAP;
            }
            Debug.Assert(false);
            throw new Exception("Unknown filterCondtype:" + filterCondtype);
        }

        //イベントのラベルチェッカー
        public enum LabelCheckStEnum
        {
            Def     //ラベル定義
            ,Jump    //ラベルへジャンプ
        }
        public class LabelCheckSt
        {
            public LabelCheckStEnum DatType { get; private set; }
            public uint Label { get; private set; }  //ラベル
            public uint Addr  { get; private set; }   //この定義が書いてあったアドレス
            public LabelCheckSt(uint addr, uint label, LabelCheckStEnum dataType)
            {
                this.Addr = addr;
                this.Label = label;
                this.DatType = dataType;
            }
        }
        public static void LabelCheck(List<FELint.ErrorSt> errors,uint start_addr, List<LabelCheckSt> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
                LabelCheckSt c = list[i];
                if (c.DatType == LabelCheckStEnum.Jump)
                {//Jump
                    //存在しないラベルへのジャンプを検出
                    bool found = false;
                    for (int n = 0; n < list.Count; n++)
                    {
                        LabelCheckSt c2 = list[n];
                        if (c2.DatType != LabelCheckStEnum.Def)
                        {
                            continue;
                        }
                        if (c.Label == c2.Label)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.EVENTSCRIPT
                            , start_addr
                            , R._("イベント「{0}」 存在しないラベル「{1}」へジャンプしています。", U.To0xHexString(start_addr), U.To0xHexString(c.Label))
                            , c.Addr));
                    }
                }
            }
        }

        public class ErrorSt
        {
            public Type DataType { get; private set; }     //種類
            public uint Addr { get; private set; }               //問題のアドレス
            public string ErrorMessage { get; private set; }     //エラーメッセージ
            public uint Tag { get; private set; }              //問題となったイベントデータが書かれている場所

            public ErrorSt(Type datatype
                , uint addr
                , string message
                , uint tag = U.NOT_FOUND
                )
            {
                this.DataType = datatype; //種類
                this.Addr = addr;               //問題のアドレス
                this.ErrorMessage = message;     //エラーメッセージ
                this.Tag = tag;
            }
            public ErrorSt(EventCondForm.CONDTYPE filterCondtype
                , uint addr
                , string message
                )
            {
                this.DataType = EventCondToType(filterCondtype); //種類
                this.Addr = addr;               //問題のアドレス
                this.ErrorMessage = message;     //エラーメッセージ
                this.Tag = U.NOT_FOUND;
            }
        }

        public static void CheckPointer(uint event_addr, List<ErrorSt> errors, EventCondForm.CONDTYPE cond, uint addr, uint tag = U.NOT_FOUND)
        {
            CheckPointer(event_addr, errors, EventCondToType(cond), addr, tag);
        }
        public static void CheckPointer(uint event_addr, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            if (!U.isSafetyPointer(event_addr))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("ポインタ「{0}」が無効です。", U.To0xHexString(event_addr)), tag));
                return;
            }
        }
        public static void CheckPointerAlien4(uint event_addr, List<ErrorSt> errors, EventCondForm.CONDTYPE cond, uint addr, uint tag = U.NOT_FOUND)
        {
            CheckPointerAlien4(event_addr, errors, EventCondToType(cond), addr, tag);
        }
        public static void CheckPointerAlien4(uint event_addr, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            if (!U.isSafetyPointer(event_addr))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("ポインタ「{0}」が無効です。", U.To0xHexString(event_addr)), tag));
                return;
            }
            CheckAlien4(event_addr, errors, cond, addr, tag);
        }
        public static void CheckPointerOrNull(uint event_addr, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            if (event_addr == 0)
            {
                return;
            }
            CheckPointer(event_addr, errors, cond, addr , tag);
        }

        public static void CheckEventPointer(uint event_addr, List<ErrorSt> errors, EventCondForm.CONDTYPE cond, uint addr, bool isFixedEvent, List<uint> tracelist, uint tag = U.NOT_FOUND)
        {
            CheckEventPointer(event_addr, errors, EventCondToType(cond), addr, isFixedEvent, tracelist, tag);
        }

        public static void CheckEventPointer(uint event_addr, List<ErrorSt> errors, Type cond, uint addr, bool isFixedEvent, List<uint> tracelist, uint tag = U.NOT_FOUND)
        {
            if (isFixedEvent == false)
            {
                if (event_addr <= 1)
                {//1は無効イベント
                    return;
                }
            }
            if (!U.isSafetyPointer(event_addr))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("イベントポインタ「{0}」が無効です。", U.To0xHexString(event_addr)),tag));
                return;
            }

            event_addr = U.toOffset(event_addr);
            if (tracelist.IndexOf(event_addr) > 0)
            {//既に探索済み
                return;
            }

            CheckAlien4(event_addr, errors, cond, addr);

            bool isWorldMapEvent = (cond == Type.WORLDMAP_EVENT);

            //有効なイベントなのでチェックする.
            EventScriptForm.CheckEnableEvenet(event_addr, isWorldMapEvent, errors, tracelist);
        }
        public static void CheckEvent(uint event_addr, List<ErrorSt> errors, Type cond, uint addr, bool isFixedEvent, List<uint> tracelist)
        {
            if (isFixedEvent == false)
            {
                if (event_addr <= 1)
                {//1は無効イベント
                    return;
                }
            }
            if (!U.isSafetyOffset(event_addr))
            {//無効なアドレス
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("イベント「{0}」が無効です。", U.To0xHexString(event_addr))));
                return;
            }

            if (tracelist.IndexOf(event_addr) > 0)
            {//既に探索済み
                return;
            }

            CheckAlien4(event_addr, errors, cond, addr);

            bool isWorldMapEvent = (cond == Type.WORLDMAP_EVENT);

            //有効なイベントなのでチェックする.
            EventScriptForm.CheckEnableEvenet(event_addr, isWorldMapEvent, errors, tracelist);
        }

        public static void CheckASMPointer(uint event_addr, List<ErrorSt> errors, EventCondForm.CONDTYPE cond, uint addr)
        {
            CheckASMPointer(event_addr, errors, EventCondToType(cond), addr);
        }
        public static void CheckASMPointer(uint asm, List<ErrorSt> errors, Type cond, uint addr,uint tag = U.NOT_FOUND)
        {
            if (!U.isSafetyPointer(asm))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("ASM関数ポインタ「{0}」が無効です。", U.To0xHexString(asm)),tag));
                return;
            }
            if (U.IsValueOdd(asm) == false)
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("ASM関数ポインタ「{0}」が偶数です。\r\nThumb命令を呼び出すためPointer+1にする必要があります。", U.To0xHexString(asm)), tag));
                return;
            }


            if (IsASMCode(asm) == false)
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("ASM関数ポインタ「{0}」の先に無効な命令が存在します。ASMが壊れている可能性があります。", U.To0xHexString(asm)), tag));
            }
            return;
        }
        public static bool IsASMCode(uint asm)
        {
            if (U.IsValueOdd(asm))
            {
                asm = asm - 1;
            }
            uint a = U.toOffset(asm);

            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.Code code = 
            dis.Disassembler(Program.ROM.Data, a, 0, vm);
            if (code.Type == DisassemblerTrumb.CodeType.Unknown)
            {
                return false;
            }
            return true;
        }
        public static void CheckASMPointerOrNull(uint asm, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            if (asm == 0)
            {
                return;
            }
            CheckASMPointer( asm, errors, cond, addr,tag );
        }
        public static void CheckProcsPointer(uint procs, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            if (!U.isSafetyPointer(procs))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("Procs関数ポインタ「{0}」が無効です。", U.To0xHexString(procs)), tag));
            }
            else if (!U.isPadding4(procs))
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("Procs関数ポインタ「{0}」が4バイト単位でありません。\r\n必ず4の倍数である必要があります。", U.To0xHexString(procs)), tag));
            }

        }
        public static void CheckFlag(uint flag, List<ErrorSt> errors, EventCondForm.CONDTYPE cond, uint addr)
        {
            CheckFlag(flag, errors, EventCondToType(cond), addr);
        }
        public static void CheckFlag(uint flag, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            string errorMessage;
            InputFormRef.GetFlagName(flag, out errorMessage);
            if (errorMessage != "")
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("利用できないフラグ「{0}」を利用しています。{1}", U.To0xHexString(flag), errorMessage) , tag));
            }
        }
        public static void ConversationTextMessage(uint textid, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            string text = FETextDecode.Direct(textid);
            string errorMessage = TextForm.CheckConversationTextMessage(text, TextForm.MAX_SERIF_WIDTH);
            if (errorMessage != "")
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("TextID:{0}\r\n{1}", U.To0xHexString(textid), errorMessage) , tag));
            }
        }
        public static void DeathQuoteTextMessage(uint textid, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            string text = FETextDecode.Direct(textid);
            string errorMessage = TextForm.CheckConversationTextMessage(text, TextForm.MAX_DEATH_QUOTE_WIDTH);
            if (errorMessage != "")
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("TextID:{0}\r\n{1}", U.To0xHexString(textid), errorMessage), tag));
            }
        }
        public static void CheckText(uint textid, string arg1, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            string text = FETextDecode.Direct(textid);
            string errorMessage = TextForm.GetErrorMessage(text, textid,arg1);
            if (errorMessage != "")
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("TextID:{0}\r\n{1}", U.To0xHexString(textid), errorMessage), tag));
            }
        }

        public static void CheckID(uint eqID, uint id, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            if (id != eqID)
            {
                string errorMessage = R._("リストの並び順とIDが一致していません。\r\nこのデータのIDは、{0}になるべきです。\r\n\r\nIDはデータの逆引きに利用されます。\r\nリストの並び順と一致していないと、バグの原因になります。", U.ToHexString(id));

                if (tag == U.NOT_FOUND)
                {
                    tag = id;
                }

                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("ID:{0}\r\n{1}", U.To0xHexString(id), errorMessage), tag));
            }
        }

        public static void CheckLZ77Pointer(uint lz77pointer, List<ErrorSt> errors, Type cond, uint addr,string name, uint tag = U.NOT_FOUND)
        {
            uint p = Program.ROM.p32(lz77pointer);
            CheckLZ77(p, errors, cond, addr,  tag);
        }

        public static void CheckLZ77(uint lz77addr, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            CheckAlien4(lz77addr, errors, cond, addr, tag);
            if (!LZ77.iscompress(Program.ROM.Data, lz77addr))
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("アドレス({0})はlz77で圧縮されていません。\r\nデータが壊れています。", U.To0xHexString(lz77addr)), tag));
            }
        }
        public static void CkeckMagicLZ77Pointer(uint pointer, ref List<FELint.ErrorSt> errors, uint magic_baseaddress, string name, uint magicindex)
        {
            uint imageOffset = U.u32(Program.ROM.Data, pointer);
            if (U.isPointer(imageOffset))
            {
                imageOffset = U.toOffset(imageOffset);
                if (imageOffset == 0)
                {
                    return;
                }
                FELint.CheckLZ77(imageOffset, errors, FELint.Type.MAGIC_ANIME_EXTENDS, magic_baseaddress, magicindex);
            }
        }
        public static void CheckInputFormRefASM(InputFormRef ifr, List<ErrorSt> errors, bool isSwitch, Type cond)
        {
            Debug.Assert(ifr.BlockSize == 4);

            uint i = 0;
            uint limit = ifr.BaseAddress + ( ifr.DataCount * 4);
            if (isSwitch)
            {
                for (uint addr = ifr.BaseAddress; addr < limit; addr += 4 , i++)
                {
                    uint p = Program.ROM.u32(addr);
                    if (!U.isSafetyPointer(p))
                    {
                        errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                            , R._("アドレス「{0}」は無効なアドレスです。", U.To0xHexString(p)), i));
                    }
                    if (U.IsValueOdd(p))
                    {
                        errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                            , R._("アドレス「{0}」は偶数でなければいけません。", U.To0xHexString(p)), i));
                    }
                }
            }
            else
            {
                for (uint addr = ifr.BaseAddress; addr < limit; addr += 4 , i++)
                {
                    uint p = Program.ROM.u32(addr);
                    if (!U.isSafetyPointer(p))
                    {
                        errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                            , R._("アドレス「{0}」は無効なアドレスです。", U.To0xHexString(p)), i));
                    }
                    if (! U.IsValueOdd(p))
                    {
                        errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                            , R._("アドレス「{0}」は奇数でなければいけません。", U.To0xHexString(p)), i));
                    }
                }
            }
        }
        public static void CheckAPErrorsPointerOrNull(uint apAddress, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            uint p = Program.ROM.p32(apAddress);
            if (p == 0)
            {
                return;
            }
            ImageUtilAP.CheckAPErrors(p, errors, cond, addr, tag);
        }

        public static void CheckAPPointer(uint apAddress, List<ErrorSt> errors, Type cond, uint addr,uint tag = U.NOT_FOUND)
        {
            uint p = Program.ROM.p32(apAddress);
            ImageUtilAP.CheckAPErrors(p, errors, cond, addr, tag);
        }
        public static void CheckAP(uint apAddress, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            if (!U.isSafetyOffset(apAddress))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("アドレス「{0}」は無効なアドレスです。", U.To0xHexString(apAddress)), tag));
            }
            ImageUtilAP.CheckAPErrors(apAddress, errors, cond, addr, tag);
        }

        public static void CheckLZ77ImagePointerOrNull(uint lz77pointer, List<ErrorSt> errors, Type cond, uint addr, int width, int min_height, uint tag = U.NOT_FOUND)
        {
            uint p = Program.ROM.p32(lz77pointer);
            if (p == 0)
            {
                return;
            }

            CheckLZ77ImageErrors(p, errors, cond, addr, width, min_height, tag);
        }

        public static void CheckLZ77ImagePointer(uint lz77pointer, List<ErrorSt> errors, Type cond, uint addr, int width, int min_height, uint tag = U.NOT_FOUND)
        {
            uint p = Program.ROM.p32(lz77pointer);
            CheckLZ77ImageErrors(p,errors,cond ,addr, width, min_height, tag);
        }

        public static void CheckAlien4(uint targetaddr, List<ErrorSt> errors, EventCondForm.CONDTYPE cond, uint addr, uint tag = U.NOT_FOUND)
        {
            CheckAlien4(targetaddr, errors, EventCondToType(cond), addr, tag);
        }
        public static void CheckAlien4(uint targetaddr, List<ErrorSt> errors, Type cond, uint addr, uint tag = U.NOT_FOUND)
        {
            targetaddr = U.toOffset(targetaddr);
            if (!U.isSafetyOffset(targetaddr))
            {//無効なポインタ
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("アドレス「{0}」は無効なアドレスです。", U.To0xHexString(targetaddr)), tag));
                return;
            }
            if (!U.isPadding4(targetaddr))
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("アドレス「{0}」は4で割り切れない数字です。\r\n実行時にクラッシュする可能性があります。", U.To0xHexString(targetaddr)), tag));
                return;
            }
        }

        public static void CheckLZ77ImageErrors(uint lz77addr, List<ErrorSt> errors, Type cond, uint addr, int width, int min_height, uint tag = U.NOT_FOUND)
        {
            CheckAlien4(lz77addr, errors, cond, addr, tag);

            byte[] data = LZ77.decompress(Program.ROM.Data, lz77addr);
            if (data.Length <= 0)
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("このアドレス({0})はlz77で圧縮されていません。\r\nデータが壊れています。", U.To0xHexString(lz77addr)), tag));
            }
            int height = ImageUtil.CalcHeight(width,data.Length);
            if (height < min_height)
            {
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , R._("このアドレス({0})にある画像は、高さが最低値{1}より小さい値({2})です。\r\nデータが壊れています。", U.To0xHexString(lz77addr), min_height, height), tag));
            }
        }

        public static void Check85Command(List<FELint.ErrorSt> errors, byte[] frameData_UZ, uint offset,Type cond, uint addr, uint tag)
        {
            Debug.Assert(frameData_UZ[offset + 3] == 0x85); //0x85 コマンド
            if (frameData_UZ[offset] == 0x48)
            {//音楽再生
                uint song_id = U.u16(frameData_UZ, offset + 1);
                if (song_id <= 0)
                {
                    return ;
                }

                string errorMessage = SongTableForm.GetErrorMessage(song_id, "SFX");
                if (errorMessage == "")
                {
                    return;
                }
                errors.Add(new FELint.ErrorSt(cond, U.toOffset(addr)
                    , errorMessage + " " + R._("SongID: {0}", U.ToHexString(song_id)), tag));
            }
        }

        public static List<FELint.ErrorSt> HiddenErrorFilter(List<FELint.ErrorSt> errors)
        {
            for (int i = 0; i < errors.Count; )
            {
                if (Program.LintCache.CheckFast(errors[i].Addr))
                {
                    errors.RemoveAt(i);
                    continue;
                }
                i++;
            }

            return errors;
        }

        public const uint SYSTEM_MAP_ID = 0xEEEEEEEE;
        public static List<FELint.ErrorSt> ScanMAP(uint mapid, List<DisassemblerTrumb.LDRPointer> ldrmap)
        {
#if !DEBUG 
            try
            {
#endif
                return ScanMAPLow(mapid, ldrmap);
#if !DEBUG 
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());

                List<FELint.ErrorSt> errors = new List<ErrorSt>();
                errors.Add(new FELint.ErrorSt(Type.FELINT_SYSTEM_ERROR, U.NOT_FOUND
                    , R._("内部エラーが発生しました。\r\nreport7zを送ってください。\r\n{0}",e.Message)));
                return errors;
            }
#endif
        }
        static List<FELint.ErrorSt> ScanMAPLow(uint mapid, List<DisassemblerTrumb.LDRPointer> ldrmap)
        {
            List<FELint.ErrorSt> errors = new List<ErrorSt>();
            if (mapid == SYSTEM_MAP_ID)
            {
                ScanSystem(errors, ldrmap);
                return errors;
            }

            if (InputFormRef.DoEvents(null, "Scan Map " + U.ToHexString(mapid) )) return errors;
            EventCondForm.MakeCheckErrors(mapid, errors);

            if (InputFormRef.DoEvents(null, null)) return errors;
            MapSettingForm.MakeCheckErrors(mapid, errors);

            if (InputFormRef.DoEvents(null, null)) return errors;
            MapChangeForm.MakeCheckError(mapid, errors);

            if (InputFormRef.DoEvents(null, null)) return errors;
            MapExitPointForm.MakeCheckError(mapid, errors);

            if (Program.ROM.RomInfo.version() == 8)
            {
                if (InputFormRef.DoEvents(null, null)) return errors;
                WorldMapEventPointerForm.MakeCheckErrors(mapid, errors);

                if (mapid == 0)
                {
                    if (InputFormRef.DoEvents(null, null)) return errors;
                    //FELint.CheckPrologeEventPointer(0, errors);
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (InputFormRef.DoEvents(null, null)) return errors;
                WorldMapEventPointerFE7Form.MakeCheckErrors(mapid, errors);

                if (mapid == 0)
                {
                    if (InputFormRef.DoEvents(null, null)) return errors;
                    //FELint.CheckPrologeEventPointer(0, errors);
                }
            }
            else
            {//ver6
                if (InputFormRef.DoEvents(null, null)) return errors;
                WorldMapEventPointerFE6Form.MakeCheckErrors(mapid, errors);

                if (mapid == 1)
                {
                    if (InputFormRef.DoEvents(null, null)) return errors;
                    //FELint.CheckPrologeEventPointer(1, errors);
                }
            }

            return errors;
        }
        static void ROMCheck(List<FELint.ErrorSt> errors)
        {
            if (Program.ROM.Data.Length > 1024 * 1024 * 32)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.FELINT_SYSTEM_ERROR, U.NOT_FOUND
                    , R._("ROMの容量が32MBを超えています。\r\nGBAでは、32MBを超えたROMは実行できません。"
                )));
            }

            //ROM Headerのチェック
            //ロゴは商標があるので、ここには書けません。
            //よって、CRC32を取得してチェックしましょう
            byte[] logo = Program.ROM.getBinaryData(0x0, 0xA0);
            U.CRC32 crc32 = new U.CRC32();
            uint logo_crc32 = crc32.Calc(logo);
            if (logo_crc32 != 0x96b08a28)
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.ROM_HEADER, U.NOT_FOUND
                    , R._("ROM先頭にあるロゴ領域が破壊されています。\r\n自動修正する場合は、ここをクリックしてください。"
                )));
            }
        }

        static void ScanSystem(List<FELint.ErrorSt> errors, List<DisassemblerTrumb.LDRPointer> ldrmap)
        {
            ROMCheck(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ImageBattleAnimeForm")) return;
            ImageBattleAnimeForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ImageBGForm")) return;
            ImageBGForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem UnitForm")) return;
            UnitForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ClassForm")) return;
            ClassForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ItemForm")) return;
            ItemForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem MoveCostForm")) return;
            MoveCostForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem PatchForm")) return;
            PatchForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ImageUnitMoveIconFrom")) return;
            ImageUnitMoveIconFrom.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ImageUnitWaitIconFrom")) return;
            ImageUnitWaitIconFrom.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ItemEffectPointerForm")) return;
            ItemEffectPointerForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ImageUnitPaletteForm")) return;
            ImageUnitPaletteForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ImageBattleScreen")) return;
            ImageBattleScreenForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ImageMagic")) return;
            ImageMagicFEditorForm.MakeCheckError(errors);
            ImageMagicCSACreatorForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem Procs")) return;
            ProcsScriptForm.MakeCheckError(errors, ldrmap);

            if (InputFormRef.DoEvents(null, "ScanSystem MenuDefinition")) return;
            MenuDefinitionForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem EventUnit")) return;
            EventUnitForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem AIScript")) return;
            AIScriptForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem SongTable")) return;
            SongTableForm.MakeCheckError(errors);

            if (InputFormRef.DoEvents(null, "ScanSystem ItemWeaponEffect")) return;
            ItemWeaponEffectForm.MakeCheckError(errors);
           
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (InputFormRef.DoEvents(null, "ScanSystem SoundFootStepsForm")) return;
                SoundFootStepsForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem ImagePortraitForm")) return;
                ImagePortraitForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EventHaikuForm")) return;
                EventHaikuForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EventBattleTalkForm")) return;
                EventBattleTalkForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem SupportTalkForm")) return;
                SupportTalkForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem SupportUnitForm")) return;
                SupportUnitForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem SoundRoomForm")) return;
                SoundRoomForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EDForm")) return;
                EDForm.MakeCheckError(errors);

                if (Program.ROM.RomInfo.is_multibyte() == false)
                {
                    SkillConfigSkillSystemForm.MakeCheckError(errors);
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (InputFormRef.DoEvents(null, "ScanSystem ImagePortraitForm")) return;
                ImagePortraitForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EventHaikuFE7Form")) return;
                EventHaikuFE7Form.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EventBattleTalkFE7Form")) return;
                EventBattleTalkFE7Form.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem SupportTalkFE7Form")) return;
                SupportTalkFE7Form.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem SupportUnitForm")) return;
                SupportUnitForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem SoundRoomForm")) return;
                SoundRoomForm.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EDForm")) return;
                EDFE7Form.MakeCheckError(errors);
            }
            else
            {
                if (InputFormRef.DoEvents(null, "ScanSystem ImagePortraitFE6Form")) return;
                ImagePortraitFE6Form.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EventHaikuFE6Form")) return;
                EventHaikuFE6Form.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EventBattleTalkFE6Form")) return;
                EventBattleTalkFE6Form.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem SupportTalkFE6Form")) return;
                SupportTalkFE6Form.MakeCheckError(errors);

                if (InputFormRef.DoEvents(null, "ScanSystem EDForm")) return;
                EDFE6Form.MakeCheckError(errors);
            }
        }
    }
}
