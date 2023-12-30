using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE7U : ROMFEINFO
    {
        public ROMFE7U(ROM rom)
    	{
            VersionToFilename = "FE7U";
            TitleToFilename = "FE7";
            mask_point_base_pointer = 0x0006B8; // Huffman tree end (indirected twice)
            mask_pointer = 0x0006BC;  // Huffman tree start (indirected once)
            text_pointer = 0x12CB8; // textの開始位置
            text_recover_address = 0xB808AC; // textの開始位置(上記ポインタを壊している改造があるののでその対策)
            text_data_start_address = 0xAEAE8C; //textデータの規定値の開始位置
            text_data_end_address = 0xB7D719; //textデータの規定値の開始位置
            unit_pointer = 0x9A274; //ユニットの開始位置
            unit_maxcount = 253; // ユニットの最大数
            unit_datasize = 52; // ユニットのデータサイズ
            max_level_address = 0x29692; //最大レベルの値を格納しているアドレス
            max_luck_address = 0x29a5e; // 最大レベルの幸運の値を格納しているアドレス
            class_pointer = 0x178f0; //クラスの開始位置
            class_datasize = 84;  // ユニットのデータサイズ
            bg_pointer = 0xB874; //BGベースアドレス
            portrait_pointer = 0x6b30; //顔ベースアドレス
            portrait_datasize = 28;
            icon_pointer = 0x4E20; // アイコンの開始位置
            icon_orignal_address = 0xC5EA4; // アイコンの初期値
            icon_orignal_max = 0xAC; // アイコンの最大数

            icon_palette_pointer = 0x4D40; // アイコンのパレットの開始位置
            unit_wait_icon_pointer = 0x024DA0; // ユニット待機アイコンの開始位置
            unit_wait_barista_anime_address = 0x25844;  // ユニット待機アイコンのバリスタのアニメ指定アドレス
            unit_wait_barista_id = 0x52;  // ユニット待機アイコンのバリスタの位置
            unit_icon_palette_address = 0x194594; //ユニットのパレットの開始位置
            unit_icon_enemey_palette_address = 0x1945B4; //ユニット(敵軍)のパレットの開始位置
            unit_icon_npc_palette_address = 0x1945D4; //ユニット(友軍)のパレットの開始位置
            unit_icon_gray_palette_address = 0x1945F4; // ユニット(グレー))のパレットの開始位置
            unit_icon_four_palette_address = 0x194614; // ユニット(4軍))のパレットの開始位置
            unit_icon_lightrune_palette_address = 0x194634; // ユニット(光の結界)のパレットの開始位置
            unit_icon_sepia_palette_address = 0x194654; // ユニット(セピア)のパレットの開始位置

            unit_move_icon_pointer = 0x6D574; // ユニット移動アイコンの開始位置
            lightrune_uniticon_id = 0x57; // ユニット(光の結界)のユニットアイコンのID
            map_setting_pointer = 0x31580;  // マップ設定の開始位置
            map_setting_datasize = 152; //マップ設定のデータサイズ
            map_setting_event_plist_pos = 120; //event plistの場所 
            map_setting_worldmap_plist_pos = 121; //woldmap event plistの場所 
            map_setting_clear_conditon_text_pos = 0x8E; //マップの右上に表示されているクリア条件の定義場所 
            map_setting_name_text_pos = 0x74; //マップ名のテキスト定義場所 
            map_config_pointer = 0x191C8;      //マップ設定の開始位置(config)
            map_obj_pointer = 0x019230;         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
            map_pal_pointer = 0x019264;         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
            map_tileanime1_pointer = 0x02D364;  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_tileanime2_pointer = 0x02DEA4;  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_map_pointer_pointer = 0x03159C; //マップ設定の開始位置(map)
            map_mapchange_pointer = 0x0315B8;   //マップ設定の開始位置(mapchange)
            map_event_pointer = 0x315D4;       //マップ設定の開始位置(event)
            map_worldmapevent_pointer = 0x0; //マップ設定の開始位置(worldmap (FE6のみ))
            map_map_pointer_list_default_size = 0xF7; //PLIST拡張をしていない時のバニラでのPLISTの数
            image_battle_animelist_pointer = 0x0541F4;   // 戦闘アニメリストの開始位置
            support_unit_pointer = 0xBDCE78;   // 支援相手の開始位置
            support_talk_pointer = 0x78A94;   // 支援会話の開始位置
            unit_palette_color_pointer = 0x0;  // ユニットのパレット(カラー)の開始位置
            unit_palette_class_pointer = 0x0;  // ユニットのパレット(クラス)の開始位置
            support_attribute_pointer = 0x26998;  //支援効果の開始位置
            terrain_recovery_pointer = 0x19B2C; //地形回復 全クラス共通
            terrain_bad_status_recovery_pointer = 0x019B3C; //地形回復 全クラス共通
            terrain_show_infomation_pointer = 0x85578; //地形ウィンドウに情報表示 全クラス共通
            ballista_movcost_pointer = 0x187E8; //地形ウィンドウに情報表示 全クラス共通
            ccbranch_pointer = 0x0; // CC分岐の開始位置
            ccbranch2_pointer = 0x0; // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
            class_alphaname_pointer = 0x0; // クラスのアルファベット表記の開始位置
            map_terrain_name_pointer = 0x19B1C; //マップの地名表記の開始位置
            image_chapter_title_pointer = 0x0; //不明 章タイトルの開始位置
            image_chapter_title_palette = 0x3FE438; // 章タイトルのパレット 多分違う
            image_unit_palette_pointer = 0x541F8; // ユニットパレットの開始位置
            item_pointer = 0x174D4; //アイテムの開始位置
            item_datasize = 36; // アイテムのデータサイズ
            item_effect_pointer = 0x052B24; // アイテムエフェクトの開始位置
            sound_table_pointer = 0x3F50; // ソングテーブルの開始位置
            sound_room_pointer = 0x1B468; //サウンドルームの開始位置
            sound_room_datasize = 16; // サウンドルームのデータサイズ
            sound_room_cg_pointer = 0xAB4C0; // サウンドルームの背景リスト(FE7のみ)
            event_ballte_talk_pointer = 0x792e0; //交戦時セリフの開始位置
            event_ballte_talk2_pointer = 0x79454; // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
            event_haiku_pointer = 0x79550; //死亡時セリフの開始位置
            event_haiku_tutorial_1_pointer = 0x7955C; // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_haiku_tutorial_2_pointer = 0x79558; // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_force_sortie_pointer = 0x8DDCC; // 強制出撃の開始位置
            event_tutorial_pointer = 0x79000; //イベントチュートリアルポインタ FE7のみ
            map_exit_point_pointer = 0x39970; // 離脱ポxxイント開始サイズ
            map_exit_point_npc_blockadd = 0x30; // arr[+0x30] からNPCらしい.
            map_exit_point_blank = 0x1D3A5C; // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
            sound_boss_bgm_pointer = 0x68148; // ボスBGMの開始位置
            sound_foot_steps_pointer = 0x0; // クラス足音の開始位置
            sound_foot_steps_switch2_address = 0x0;
            worldmap_point_pointer = 0x0; // ワールドマップ拠点の開始位置
            worldmap_bgm_pointer = 0x0; // ワールドマップのBGMテーブルの開始位置
            worldmap_icon_data_pointer = 0; // ワールドマップのアイコンデータのテーブルの開始位置
            worldmap_event_on_stageclear_pointer = 0x0; // ワールドマップイベントクリア時
            worldmap_event_on_stageselect_pointer = 0xB561c; //ワールドマップイベント 拠点選択時
            worldmap_county_border_pointer = 0; // ワールドマップ国名の表示
            worldmap_county_border_palette_pointer = 0x0; // ワールドマップ国名の表示のパレット
            item_shop_hensei_pointer = 0x0; //編成準備店
            item_cornered_pointer = 0x2A17C; //すくみの開始位置
            ed_1_pointer = 0xB7EDC;  //ED FE8のみ 倒れたら撤退するかどうか
            ed_2_pointer = 0xb7eb8;  //ED 通り名
            ed_3a_pointer = 0xcee630;  //ED その後 エリウッド編　
            ed_3b_pointer = 0xcee634;  //ED その後 ヘクトル編
            ed_3c_pointer = 0xcedd48;  //ED その後 FE7 リン編(ポインタ指定できない)
            generic_enemy_portrait_pointer = 0x75C0; //一般兵の顔
            generic_enemy_portrait_count = 0x8-2; //一般兵の顔の個数

            cc_item_hero_crest_itemid = 0x63;  //CCアイテム 英雄の証
            cc_item_knight_crest_itemid = 0x64;  //CCアイテム 騎士の勲章
            cc_item_orion_bolt_itemid = 0x65;  //CCアイテム オリオンの矢
            cc_elysian_whip_itemid = 0x66;  //CCアイテム 天空のムチ
            cc_guiding_ring_itemid = 0x67;  //CCアイテム 導きの指輪
            cc_fallen_contract_itemid = 0x96;  //CCアイテム ダミー8A(FE7では覇者の証)
            cc_master_seal_itemid = 0x87;  //CCアイテム マスタープルフ
            cc_ocean_seal_itemid = 0x8B;  //CCアイテム 覇者の証(FE7では闇の契約書 なぜか逆になっている)
            cc_moon_bracelet_itemid = 0x87;  //CCアイテム 月の腕輪
            cc_sun_bracelet_itemid = 0x89;  //CCアイテム 太陽の腕輪

            cc_item_hero_crest_pointer = 0x27500;  //CCアイテム 英雄の証
            cc_item_knight_crest_pointer = 0x27508;  //CCアイテム 騎士の勲章
            cc_item_orion_bolt_pointer = 0x27510;  //CCアイテム オリオンの矢
            cc_elysian_whip_pointer = 0x27518;  //CCアイテム 天空のムチ
            cc_guiding_ring_pointer = 0x27520;  //CCアイテム 導きの指輪
            cc_fallen_contract_pointer = 0x27574;  //CCアイテム ダミー8A 闇の契約書
            cc_master_seal_pointer = 0x27528;  //CCアイテム マスタープルフ
            cc_ocean_seal_pointer = 0x2754C;  //CCアイテム 覇者の証
            cc_moon_bracelet_pointer = 0x27540;  //CCアイテム 月の腕輪 天の刻印
            cc_sun_bracelet_pointer = 0x27544;  //CCアイテム 太陽の腕輪 天の刻印
            unit_increase_height_pointer = 0x722C;  //ステータス画面で背を伸ばす
            unit_increase_height_switch2_address = 0x721C;
            op_class_demo_pointer = 0xCE68E4; //OPクラスデモ
            op_class_font_pointer = 0x0;  //OPクラス日本語フォント
            op_class_font_palette_pointer = 0x0;  // OPクラス紹介フォントのパレット
            status_font_pointer = 0x6118;  //ステータス画面用のフォント
            status_font_count = 0x100;  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
            ed_staffroll_image_pointer = 0x0; // スタッフロール
            ed_staffroll_palette_pointer = 0x0; // スタッフロールのパレット
            op_prologue_image_pointer = 0x0; // OP字幕
            op_prologue_palette_color_pointer = 0x0; // OP字幕のパレット ???

            arena_class_near_weapon_pointer = 0x2EB58; //闘技場 近接武器クラス 
            arena_class_far_weapon_pointer = 0x2EB64; // 闘技場 遠距離武器クラス
            arena_class_magic_weapon_pointer = 0x2EBB4; // 闘技場 魔法武器クラス
            arena_enemy_weapon_basic_pointer = 0x2F1C0; // 闘技場 敵武器テーブル基本武器
            arena_enemy_weapon_rankup_pointer = 0x2EDDC; // 闘技場 敵武器テーブルランクアップ武器
            link_arena_deny_unit_pointer = 0; //通信闘技場 禁止ユニット 
            worldmap_road_pointer = 0x0; // ワールドマップの道

            menu_definiton_pointer = 0x1B8D0; //メニュー定義
            menu_promotion_pointer = 0x0; //CC決定する選択子
            menu_promotion_branch_pointer = 0x0; //FE8にある分岐CCメニュー
            menu_definiton_split_pointer = 0x0;  //FE8にある分岐メニュー
            menu_definiton_worldmap_pointer = 0x0; //FE8のワールドマップのメニュー
            menu_definiton_worldmap_shop_pointer = 0x0; //FE8のワールドマップ店のメニュー        
            menu_unit_pointer =  0xB95AB4; // ユニットメニュー
            menu_game_pointer =  0xB95AFC; // ゲームメニュー
            menu_debug1_pointer = 0xB95850;  // デバッグメニュー
            menu_item_pointer = 0xB959DC;  // アイテム利用メニュー
            MenuCommand_UsabilityAlways = 0x04A8F8; //メニューを開くという値を返す関数のアドレス
            MenuCommand_UsabilityNever = 0x04A900; //メニューを開かないという値を返す関数のアドレス       
            status_rmenu_unit_pointer = 0x8152C; // ステータス RMENU1
            status_rmenu_game_pointer = 0x81534; // ステータス RMENU2
            status_rmenu3_pointer = 0x8154C; // ステータス RMENU3
            status_rmenu4_pointer = 0x34228; // 戦闘予測 RMENU4
            status_rmenu5_pointer = 0x34240; // 戦闘予測 RMENU5
            status_rmenu6_pointer = 0x0; // 状況画面 RMENU6
            status_param1_pointer = 0x7FE4C; // ステータス PARAM1
            status_param2_pointer = 0x80130; // ステータス PARAM2
            status_param3w_pointer = 0x0; // ステータス PARAM3 武器 海外版には"剣"みたいな武器の属性表示がありません
            status_param3m_pointer = 0x0; // ステータス PARAM3 魔法

            systemmenu_common_image_pointer = 0x085BE8; //システムメニューの画像
            systemmenu_common_palette_pointer = 0x081194; //システムパレット 無圧縮4パレット
            systemmenu_goal_tsa_pointer = 0x085DC8; //システムメニュー 目的表示TSA
            systemmenu_terrain_tsa_pointer = 0x085640; //システムメニュー 地形表示TSA

            systemmenu_name_image_pointer = 0x085BE8; //システムメニュー 名前表示画像(FE8は共通画像)
            systemmenu_name_tsa_pointer = 0x08524C; //システムメニュー 名前表示TSA
            systemmenu_name_palette_pointer = 0x084E48; //システムメニュー 名前表示パレット

            systemmenu_battlepreview_image_pointer = 0x033C0C; //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
            systemmenu_battlepreview_tsa_pointer = 0x0336E8; //戦闘プレビューTSA
            systemmenu_battlepreview_palette_pointer = 0x033B2C; //戦闘プレビューパレット

            systemarea_move_gradation_palette_pointer = 0x01D25C; //行動範囲
            systemarea_attack_gradation_palette_pointer = 0x01D260; //攻撃範囲
            systemarea_staff_gradation_palette_pointer = 0x01D264; //杖の範囲

            systemmenu_badstatus_image_pointer = 0x84F5C; //無圧縮のバッドステータス画像
            systemmenu_badstatus_palette_pointer = 0x825B0; //バッドステータスのパレット
            systemmenu_badstatus_old_image_pointer = 0x85BE8; //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
            systemmenu_badstatus_old_palette_pointer = 0x9C11C; //昔の圧縮のバッドステータス画像のパレット FE7 FE6

            bigcg_pointer = 0xB6B64; //CG
            end_cg_address = 0x0; // END CG FE8のみ
            worldmap_big_image_pointer = 0x0B5BF4; //ワールドマップ フィールドになるでかい奴  
            worldmap_big_palette_pointer = 0x0B5DF4; //ワールドマップ フィールドになるでかい奴 パレット  
            worldmap_big_dpalette_pointer = 0x0; //ワールドマップ フィールドになるでかい奴 闇パレット  
            worldmap_big_palettemap_pointer = 0xB5BEC; //ワールドマップ フィールドになるでかい奴 パレットマップではなく、 TSA 12分割
            worldmap_event_image_pointer =   0xB5E20; //ワールドマップ イベント用 
            worldmap_event_palette_pointer = 0xB5E1C; //ワールドマップ イベント用 パレット  
            worldmap_event_tsa_pointer = 0xB5E2C; //ワールドマップ イベント用 TSA
            worldmap_mini_image_pointer = 0x0; //ワールドマップ ミニマップ 
            worldmap_mini_palette_pointer = 0x0; //ワールドマップ ミニマップ パレット  
            worldmap_icon_palette_pointer = 0x0; //ワールドアイコンと道パレット
            worldmap_icon1_pointer = 0x0; //ワールドマップ アイコン1
            worldmap_icon2_pointer = 0x0; //ワールドマップ アイコン2
            worldmap_road_tile_pointer = 0x0; //ワールドマップ  道チップ
            map_load_function_pointer = 0x0; //マップチャプターに入ったときの処理(FE8のみ)
            map_load_function_switch1_address = 0x0;
            system_icon_pointer = 0x155BC;//システム アイコン集
            system_icon_palette_pointer = 0x155C8;//システム アイコンパレット集
            system_icon_width_address = 0x155A0; //システムアイコンの幅が書かれているアドレス
            system_weapon_icon_pointer = 0x965F4;//剣　斧　弓などの武器属性アイコン集
            system_weapon_icon_palette_pointer = 0x090530;//剣　斧　弓などの武器属性アイコン集のパレット
            system_music_icon_pointer = 0x0AE1DC;//音楽関係のアイコン集
            system_music_icon_palette_pointer = 0x0AE1D0;//音楽関係のアイコン集のパレット
            weapon_rank_s_bonus_address = 0x28e98;//武器ランクSボーナス設定
            weapon_battle_flash_address = 0x533be;//神器 戦闘時フラッシュ
            weapon_effectiveness_2x3x_address = 0;//神器 2倍 3倍特効
            font_item_address = 0xb896b0;//アイテム名とかに使われるフォント 関数08005B9C
            font_serif_address = 0xb8b5b0; //セリフとかに使われるフォント
            monster_probability_pointer = 0x0; //魔物発生確率
            monster_item_item_pointer = 0x0; //魔物所持アイテム アイテム確率
            monster_item_probability_pointer = 0x0; //魔物所持アイテム 所持確率
            monster_item_table_pointer = 0x0; //魔物所持アイテム アイテムと所持を管理するテーブル
            monster_wmap_base_point_pointer = 0x0; //ワールドマップに魔物を登場させる処理達
            monster_wmap_stage_1_pointer = 0x0;
            monster_wmap_stage_2_pointer = 0x0;
            monster_wmap_probability_1_pointer = 0x0;
            monster_wmap_probability_2_pointer = 0x0;
            monster_wmap_probability_after_1_pointer = 0x0;
            monster_wmap_probability_after_2_pointer = 0x0;
            battle_bg_pointer = 0x06AFB0; //戦闘背景
            battle_terrain_pointer = 0x4D100; //戦闘地形
            senseki_comment_pointer = 0x9A844; //戦績コメント
            unit_custom_battle_anime_pointer = 0x528e4; //ユニット専用アニメ FE7にある

            magic_effect_pointer = 0x558B4; //魔法効果へのポインタ
            magic_effect_original_data_count = 0x3e; //もともとあった魔法数

            system_move_allowicon_pointer = 0x02FF64;//移動するときの矢印アイコン
            system_move_allowicon_palette_pointer = 0x2FF6C; //移動するときの矢印アイコン アイコンパレット集
            system_tsa_16color_304x240_pointer = 0xB9161C; //16色304x240 汎用TSAポインタ
            eventunit_data_size = 16; //ユニット配置のデータサイズ
            eventcond_tern_size = 16; //イベント条件 ターン条件のサイズ FE7->16 FE8->12
            eventcond_talk_size = 16; //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
            oping_event_pointer = 0xca0544;
            ending1_event_pointer = 0x12A94;
            ending2_event_pointer = 0x12ADC;
            RAMSlotTable_address = 0xB92EB0;
            supply_pointer_address = 0x2E724;  //輸送隊RAMへのアドレス
            workmemory_player_units_address = 0x0202BD50;    //ワークメモリ PLAYER UNIT
            workmemory_enemy_units_address = 0x0202CEC0;    //ワークメモリ PLAYER UNIT
            workmemory_npc_units_address = 0x0202DCD0;    //ワークメモリ PLAYER UNIT
            workmemory_mapid_address = 0x0202BC06;    //ワークメモリ マップID
            workmemory_chapterdata_address = workmemory_mapid_address - 0xE; //ワークメモリ章データ
            workmemory_chapterdata_size = 0x4C;    //ワークメモリ 章データのサイズ
            workmemory_battle_actor_address = 0x0203A3F0; //ワークメモリ 戦闘時のユニット構造体
            workmemory_battle_target_address = 0x0203A470; //ワークメモリ 戦闘時のユニット構造体
            workmemory_worldmap_data_address = 0x0;//ワークメモリ ワールドマップ関係の起点
            workmemory_worldmap_data_size = 0x0; //ワークメモリ ワールドマップ関係のサイズ
            workmemory_arena_data_address = 0x0203A7F4;//ワークメモリ 闘技場関係の起点
            workmemory_ai_data_address = 0x0203A8EC; //ワークメモリ AI関係の起点
            workmemory_action_data_address = 0x0203A85C; //ワークメモリ ActionData
            workmemory_dungeon_data_address = 0x0; //ワークメモリ ダンジョン FE8のみ
            workmemory_battlesome_data_address = 0x0203DFFC; //ワークメモリ バルトに関係する諸データ
            workmemory_battleround_data_address = 0x02039328; //ワークメモリ　戦闘のラウンドデータ
            workmemory_last_string_address = 0x0202B5B4;  //ワークメモリ 最後に表示した文字列
            workmemory_text_buffer_address = 0x0202A5B4;  //ワークメモリ デコードされたテキスト
            workmemory_next_text_buffer_address = 0x03000040;  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
            workmemory_local_flag_address = 0x03004AD8;  //ワークメモリ グローバルフラグ
            workmemory_global_flag_address = 0x03004AD0;  //ワークメモリ ローカルフラグ
            workmemory_trap_address = 0x0203A518;  //ワークメモリ ローカルフラグ
            workmemory_bwl_address = 0x0203E790;  //BWLデータ
            workmemory_clear_turn_address = 0x0203EC00; //ワークメモリ クリアターン数
            workmemory_clear_turn_count = 0x30;  //クリアターン数 最大数
            workmemory_memoryslot_address = 0;  //ワークメモリ メモリスロットFE8
            workmemory_eventcounter_address = 0x0;  //イベントカウンター メモリスロットFE8
            workmemory_procs_forest_address = 0x02026A30;  //ワークメモリ Procs
            workmemory_procs_pool_address = 0x02024E28;  //ワークメモリ Procs
            function_sleep_handle_address = 0x08004A84 + 1;  //ワークメモリ Procs待機中
            workmemory_user_stack_base_address = 0x03007DE0; //ワークメモリ スタックの一番底
            function_fe_main_return_address = 0x08000AF2 + 1; //スタックの一番底にある戻り先
            workmemory_control_unit_address = 0x03004690; //ワークメモリ 操作ユニット
            workmemory_bgm_address = 0x02024E1C; //ワークメモリ BGM
            function_event_engine_loop_address = 0x0800B390 + 1; //イベントエンジン
            workmemory_reference_procs_event_address_offset = 0x2C; //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
            workmemory_procs_game_main_address = 0x02024E28; //ワークメモリ Procsの中でのGAMEMAIN
            workmemory_palette_address = 0x02022860; //RAMに記録されているダブルバッファのパレット領域
            workmemory_sound_player_00_address = 0x03005B10; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_01_address = 0x03005D20; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_02_address = 0x03005D60; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_03_address = 0x03005E30; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_04_address = 0x03005DA0; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_05_address = 0x03005A90; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_06_address = 0x03005AD0; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_07_address = 0x03005CE0; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_08_address = 0x03005DF0; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_keybuffer_address = 0x02024C78; //RAMのキーバッファ
            procs_maptask_pointer = 0x2E184; //PROCSのMAPTASK 
            procs_soundroomUI_pointer = 0xAC2BC; //PROCSのSoundRoomUI 
            procs_game_main_address = 0x8B924BC; //PROCSのGAME MAIN 
            summon_unit_pointer = 0; //召喚
            summons_demon_king_pointer = 0; //呼魔
            summons_demon_king_count_address = 0; //呼魔リストの数
            mant_command_pointer = 0x63804; //マント
            mant_command_startadd = 0x58; //マント開始数
            mant_command_count_address = 0x63804; //マント数 アドレス
            unit_increase_height_yes = 0x080072C8;  //ステータス画面で背を伸ばす 伸ばす
            unit_increase_height_no =  0x080072CC;  //ステータス画面で背を伸ばす 伸ばさない
            battle_screen_TSA1_pointer = 0x04CF54;  //戦闘画面
            battle_screen_TSA2_pointer = 0x04CF58;  //戦闘画面
            battle_screen_TSA3_pointer = 0x04C6BC;  //戦闘画面
            battle_screen_TSA4_pointer = 0x04C6C4;  //戦闘画面
            battle_screen_TSA5_pointer = 0x04D428;  //戦闘画面
            battle_screen_palette_pointer = 0x04D430;  //戦闘画面 パレット
            battle_screen_image1_pointer = 0x04D220;  //戦闘画面 画像1
            battle_screen_image2_pointer = 0x04D280;  //戦闘画面 画像2
            battle_screen_image3_pointer = 0x04D2E0;  //戦闘画面 画像3
            battle_screen_image4_pointer = 0x04D340;  //戦闘画面 画像4
            battle_screen_image5_pointer = 0x04D41C;  //戦闘画面 画像5
            ai1_pointer = 0xB989F0;  //AI1ポインタ
            ai2_pointer = 0xB989E4;  //AI2ポインタ
            ai3_pointer = 0x392A4;  //AI3ポインタ
            ai_steal_item_pointer = 0x368B8;  //AI盗むAI アイテム評価テーブル 0x08B97290
            ai_preform_staff_pointer = 0x3AAD4;  //AI杖 杖評価テーブル
            ai_preform_staff_direct_asm_pointer = 0x03AB78;  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
            ai_preform_item_pointer = 0x3B9F4; //AIアイテム アイテム評価テーブル
            ai_preform_item_direct_asm_pointer = 0x3BA94;  //AIアイテム アイテム評価テーブル
            ai_map_setting_pointer = 0x34940;  //AI 章ごとの設定テーブル 0x081D3A60
            item_usability_array_pointer = 0x26D0C; //アイテムを利用できるか判定する
            item_usability_array_switch2_address = 0x26CFA;
            item_effect_array_pointer = 0x2D048;    //アイテムを利用した場合の効果を定義する
            item_effect_array_switch2_address = 0x2D02E;
            item_promotion1_array_pointer = 0x27428;   //CCアイテムを使った場合の処理を定義する
            item_promotion1_array_switch2_address = 0x27416;
            item_promotion2_array_pointer = 0x95680;  //CCアイテムかどうかを定義する(FE7のみ)
            item_promotion2_array_switch2_address = 0x95670;
            item_staff1_array_pointer = 0x27124;    //アイテムのターゲット選択の方法を定義する(多分)
            item_staff1_array_switch2_address = 0x27112;
            item_staff2_array_pointer = 0x67DF8;    //杖の種類を定義する
            item_staff2_array_switch2_address = 0x67DE6;
            item_statbooster1_array_pointer = 0x2CDDC;    //ドーピングアイテムを利用した時のメッセージを定義する
            item_statbooster1_array_switch2_address = 0x2CDCA;
            item_statbooster2_array_pointer = 0x2806C;    //ドーピングアイテムとCCアイテムかどうかを定義する
            item_statbooster2_array_switch2_address = 0x28058;
            item_errormessage_array_pointer = 0x26F68;    //アイテム利用時のエラーメッセージ
            item_errormessage_array_switch2_address = 0x26F56;
            event_function_pointer_table_pointer = 0xD72C;    //イベント命令ポインタ
            event_function_pointer_table2_pointer = 0x0;   //イベント命令ポインタ2 ワールドマップ
            item_effect_pointer_table_pointer = 0x558B4;   //間接エフェクトポインタ
            command_85_pointer_table_pointer = 0x67860;    //85Commandポインタ
            dic_main_pointer = 0x0;     //辞書メインポインタ
            dic_chaptor_pointer = 0x0;  //辞書章ポインタ
            dic_title_pointer = 0x0;   //辞書タイトルポインタ
            itemicon_mine_id = 0x79;  // アイテムアイコンのフレイボムの位置
            item_gold_id = 0x76;  // お金を取得するイベントに利用されるゴールドのID
            unitaction_function_pointer = 0x2F244;  // ユニットアクションポインタ
            lookup_table_battle_terrain_00_pointer = 0x52A2C; //戦闘アニメの床
            lookup_table_battle_terrain_01_pointer = 0x529B0; //戦闘アニメの床
            lookup_table_battle_terrain_02_pointer = 0x529B8;//戦闘アニメの床
            lookup_table_battle_terrain_03_pointer = 0x529C0; //戦闘アニメの床
            lookup_table_battle_terrain_04_pointer = 0x529C8; //戦闘アニメの床
            lookup_table_battle_terrain_05_pointer = 0x529D0; //戦闘アニメの床
            lookup_table_battle_terrain_06_pointer = 0x529D8; //戦闘アニメの床
            lookup_table_battle_terrain_07_pointer = 0x529E0; //戦闘アニメの床
            lookup_table_battle_terrain_08_pointer = 0x529E8; //戦闘アニメの床
            lookup_table_battle_terrain_09_pointer = 0x529F0; //戦闘アニメの床
            lookup_table_battle_terrain_10_pointer = 0x529F8; //戦闘アニメの床
            lookup_table_battle_terrain_11_pointer = 0x52A00; //戦闘アニメの床
            lookup_table_battle_terrain_12_pointer = 0x52A08; //戦闘アニメの床
            lookup_table_battle_terrain_13_pointer = 0x52A10; //戦闘アニメの床
            lookup_table_battle_terrain_14_pointer = 0x52A18; //戦闘アニメの床
            lookup_table_battle_terrain_15_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_16_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_17_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_18_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_19_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_20_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_bg_00_pointer = 0x52B04; //戦闘アニメの背景
            lookup_table_battle_bg_01_pointer = 0x52A8C; //戦闘アニメの背景
            lookup_table_battle_bg_02_pointer = 0x52A94; //戦闘アニメの背景
            lookup_table_battle_bg_03_pointer = 0x52A9C; //戦闘アニメの背景
            lookup_table_battle_bg_04_pointer = 0x52AA4; //戦闘アニメの背景
            lookup_table_battle_bg_05_pointer = 0x52AAC; //戦闘アニメの背景
            lookup_table_battle_bg_06_pointer = 0x52AB4; //戦闘アニメの背景
            lookup_table_battle_bg_07_pointer = 0x52ABC; //戦闘アニメの背景
            lookup_table_battle_bg_08_pointer = 0x52AC4; //戦闘アニメの背景
            lookup_table_battle_bg_09_pointer = 0x52ACC; //戦闘アニメの背景
            lookup_table_battle_bg_10_pointer = 0x52AD4; //戦闘アニメの背景
            lookup_table_battle_bg_11_pointer = 0x52ADC; //戦闘アニメの背景
            lookup_table_battle_bg_12_pointer = 0x52AE4; //戦闘アニメの背景
            lookup_table_battle_bg_13_pointer = 0x52AEC; //戦闘アニメの背景
            lookup_table_battle_bg_14_pointer = 0x52AF4; //戦闘アニメの背景
            lookup_table_battle_bg_15_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_16_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_17_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_18_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_19_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_20_pointer = 0x00; //戦闘アニメの背景
            map_terrain_type_count = 65; //地形の種類の数
            menu_J12_always_address = 0x04A8F8; //メニューの表示判定関数 常に表示する
            menu_J12_hide_address = 0x04A900;   //メニューの表示判定関数 表示しない
            status_game_option_pointer = 0xADCAC; //ゲームオプション
            status_game_option_order_pointer = 0xCE586C; //ゲームオプションの並び順
            status_game_option_order2_pointer = 0xCE5874; //ゲームオプションの並び順2 FE7のみ
            status_game_option_order_count_address = 0xCE5868; //ゲームオプションの個数
            status_units_menu_pointer = 0x8AC88; //部隊メニュー
            tactician_affinity_pointer = 0x1C024; //軍師属性(FE7のみ)
            event_final_serif_pointer = 0x7DC48; //終章セリフ(FE7のみ)
            compress_image_borderline_address = 0xCBEE4; //これ以降に圧縮画像が登場するというアドレス
            builddate_address = 0x0;

            Default_event_script_term_code = new byte[] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード
            Default_event_script_toplevel_code = new byte[] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード(各章のトップレベルのイベント)
            Default_event_script_mapterm_code = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //ワールドマップイベント命令を終了させるディフォルトコード
            main_menu_width_address = 0xB95AF6; //メインメニューの幅
            font_default_begin = 0xB87B68;
            font_default_end = 0xB8B9AC;
            item_name_article_pointer = 0x0; // a|an|the を切り替えるテーブル 英語版のみ
            item_name_article_switch2_address = 0x0;
            vanilla_field_config_address = 0x359F3C;    //バニラのタイルセット よく使われもの
            vanilla_field_image_address = 0x36B01C;
            vanilla_village_config_address = 0x35C63C;
            vanilla_village_image_address = 0x33F2C8;
            vanilla_casle_config_address = 0x35DB50;
            vanilla_casle_image_address = 0x3464F0;
            vanilla_plain_config_address = 0x35EE10;
            vanilla_plain_image_address = 0x34C74C;
            map_minimap_tile_array_pointer = 0xA247C; //minimapのチップ割り当て
            bg_reserve_black_bgid = 0x5b;
            bg_reserve_random_bgid = U.NOT_FOUND;

            map_default_count = 0x43;    // ディフォルトのマップ数
            wait_menu_command_id = 0x67; //WaitメニューのID
            extends_address = 0x09000000;  //拡張領域
            orignal_crc32 = 0x2a524221; //無改造ROMのCRC32
            is_multibyte = false;    // マルチバイトを利用するか？
            version = 7;    // バージョン

            OverwriteROMConstants(rom);
        }


        override public uint patch_C01_hack(out uint enable_value) { enable_value = 0x0000D124; return 0x67B0; } //C01 patch
        override public uint patch_C48_hack(out uint enable_value) { enable_value = 0x08067AB0; return 0x67920; } //C48 patch
        override public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x0000000C; return 0x069D670; } //16_tracks_12_sounds patch
        override public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x0; } //章の名前をテキストにするパッチ
        override public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x7598; } //一般兵の顔 拡張
        override public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x02f218; } //ユニットアクションの拡張
        override public uint patch_write_build_version(out uint enable_value) { enable_value = 0x0; return 0x0; } //ビルドバージョンを書き込む
        override public string get_shop_name(uint shop_object)//店の名前
        {
            if (shop_object == 0x13)
            {
                return R._("武器屋");
            }
            else if (shop_object == 0x14)
            {
                return R._("道具屋");
            }
            else if (shop_object == 0x15)
            {
                return R._("秘密屋");
            }
            return "";
        }
    };
}
    