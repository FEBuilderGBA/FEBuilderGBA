using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace FEBuilderGBA
{
    public interface IROMFEINFO
    {
        String game_id();    // ゲームバージョンコード
        String VersionToFilename();
        String TitleToFilename();
        uint mask_point_base_pointer(); // Huffman tree end (indirected twice)
        uint mask_pointer();  // Huffman tree start (indirected once)
        uint text_pointer(); // textの開始位置
        uint text_recover_address(); // textの開始位置(上記ポインタを壊している改造があるののでその対策)
        uint text_data_start_address(); // textデータの規定値の開始位置
        uint text_data_end_address(); // textデータの規定値の開始位置
        uint unit_pointer(); // ユニットの開始位置
        uint unit_maxcount(); // ユニットの最大数
        uint unit_datasize(); // ユニットのデータサイズ
        uint max_level_address(); // 最大レベルの値を格納しているアドレス
        uint max_luck_address(); // 最大レベルの幸運の値を格納しているアドレス
        uint class_pointer(); // クラスの開始位置
        uint class_datasize(); // ユニットのデータサイズ
        uint bg_pointer(); //BGベースアドレス
        uint face_pointer(); //顔ベースアドレス
        uint face_datasize();
        uint icon_pointer(); // アイコンの開始位置
        uint icon_orignal_address(); // アイコンの初期値
        uint icon_orignal_max(); // アイコンの最大数

        uint icon_palette_pointer(); // アイコンのパレットの開始位置
        uint unit_wait_icon_pointer();  // ユニット待機アイコンの開始位置
        uint unit_wait_barista_anime_address();  // ユニット待機アイコンのバリスタのアニメ指定アドレス
        uint unit_wait_barista_id();  // ユニット待機アイコンのバリスタの位置
        uint unit_icon_palette_address(); // ユニットのパレットの開始位置
        uint unit_icon_npc_palette_address(); // ユニット(友軍)のパレットの開始位置
        uint unit_icon_enemey_palette_address(); // ユニット(敵軍)のパレットの開始位置
        uint unit_icon_gray_palette_address(); //ユニット(グレー)のパレットの開始位置
        uint unit_icon_four_palette_address(); // ユニット(4軍))のパレットの開始位置
        uint unit_icon_lightrune_palette_address(); // ユニット(光の結界)のパレットの開始位置
        uint unit_icon_sepia_palette_address(); // ユニット(セピア)のパレットの開始位置
        uint unit_move_icon_pointer();  // ユニット移動アイコンの開始位置
        uint lightrune_uniticon_id(); // ユニット(光の結界)のユニットアイコンのID
        uint map_setting_pointer();  // マップ設定の開始位置
        uint map_setting_datasize(); //マップ設定のデータサイズ
        uint map_setting_event_plist_pos(); //event plistの場所 
        uint map_setting_worldmap_plist_pos(); //woldmap event plistの場所 
        uint map_setting_clear_conditon_text_pos(); //マップの右上に表示されているクリア条件の定義場所 
        uint map_setting_name_text_pos(); //マップ名のテキスト定義場所 
        uint map_config_pointer();      //マップ設定の開始位置(config)
        uint map_obj_pointer();         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
        uint map_pal_pointer();         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
        uint map_tileanime1_pointer();  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        uint map_tileanime2_pointer();  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        uint map_map_pointer_pointer(); //マップ設定の開始位置(map)
        uint map_mapchange_pointer();   //マップ設定の開始位置(mapchange)
        uint map_event_pointer();       //マップ設定の開始位置(event)
        uint map_worldmapevent_pointer(); //マップ設定の開始位置(worldmap (FE6のみ))
        uint image_battle_animelist_pointer();  // 戦闘アニメリストの開始位置
        uint support_unit_pointer();  // 支援相手の開始位置
        uint support_talk_pointer();  // 支援会話の開始位置
        uint unit_palette_color_pointer();  // ユニットのパレット(カラー)の開始位置
        uint unit_palette_class_pointer();  // ユニットのパレット(クラス)の開始位置
        uint support_attribute_pointer();  //支援効果の開始位置
        uint terrain_recovery_pointer(); //地形回復 全クラス共通
        uint terrain_bad_status_recovery_pointer(); //地形回復 全クラス共通
        uint terrain_show_infomation_pointer(); //地形ウィンドウに情報表示 全クラス共通
        uint ccbranch_pointer(); // CC分岐の開始位置
        uint ccbranch2_pointer(); // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
        uint class_alphaname_pointer(); // クラスのアルファベット表記の開始位置
        uint map_terrain_name_pointer(); // マップの地名表記の開始位置
        uint image_chapter_title_pointer(); // 章タイトルの開始位置
        uint image_chapter_title_palette(); // 章タイトルのパレット
        uint image_unit_palette_pointer(); // ユニットパレットの開始位置
        uint item_pointer(); // アイテムの開始位置
        uint item_datasize(); // アイテムのデータサイズ
        uint item_effect_pointer(); // アイテムエフェクトの開始位置
        uint sound_table_pointer(); // ソングテーブルの開始位置
        uint sound_room_pointer(); // サウンドルームの開始位置
        uint sound_room_datasize(); // サウンドルームのデータサイズ
        uint sound_room_cg_pointer(); // サウンドルームの背景リスト(FE7のみ)
        uint event_ballte_talk_pointer(); // 交戦時セリフの開始位置
        uint event_ballte_talk2_pointer(); // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
        uint event_haiku_pointer(); // 死亡時セリフの開始位置
        uint event_haiku_tutorial_1_pointer(); // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
        uint event_haiku_tutorial_2_pointer(); // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
        uint event_force_sortie_pointer(); // 強制出撃の開始位置
        uint event_tutorial_pointer(); //イベントチュートリアルポインタ FE7のみ
        uint map_exit_point_pointer(); // 離脱ポイント開始サイズ
        uint map_exit_point_npc_blockadd() ; // arr[+65] からNPCらしい.
        uint map_exit_point_blank(); // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
        uint sound_boss_bgm_pointer(); // ボスBGMの開始位置
        uint sound_foot_steps_pointer(); // クラス足音の開始位置
        uint sound_foot_steps_switch2_address();
        uint worldmap_point_pointer(); // ワールドマップ拠点の開始位置
        uint worldmap_bgm_pointer(); // ワールドマップのBGMテーブルの開始位置
        uint worldmap_icon_data_pointer(); // ワールドマップのアイコンデータのテーブルの開始位置
        uint worldmap_event_on_stageclear_pointer(); // ワールドマップイベント 拠点クリア時
        uint worldmap_event_on_stageselect_pointer(); // ワールドマップイベント 拠点選択時
        uint worldmap_county_border_pointer(); // ワールドマップ国名の表示
        uint worldmap_county_border_palette_pointer(); // ワールドマップ国名の表示のパレット
        uint item_shop_hensei_pointer(); //編成準備店
        uint item_cornered_pointer(); //すくみり開始位置
        uint ed_1_pointer();  //ED 倒れたら撤退するかどうか 
        uint ed_2_pointer();  //ED 通り名
        uint ed_3a_pointer();  //ED その後 FE7 エリウッド FE8 エイルーク編
        uint ed_3b_pointer();  //ED その後 FE7 ヘクトル   FE8 エフラム編
        uint ed_3c_pointer();  //ED その後 FE7 リン編
        uint generic_enemy_portrait_pointer(); //一般兵の顔
        uint generic_enemy_portrait_count(); //一般兵の顔の個数
        uint cc_item_hero_crest_itemid();  //CCアイテム 英雄の証
        uint cc_item_knight_crest_itemid();  //CCアイテム 騎士の勲章
        uint cc_item_orion_bolt_itemid(); //CCアイテム オリオンの矢
        uint cc_elysian_whip_itemid();  //CCアイテム 天空のムチ
        uint cc_guiding_ring_itemid();  //CCアイテム 導きの指輪
        uint cc_fallen_contract_itemid(); //CCアイテム ダミー8A
        uint cc_master_seal_itemid();  //CCアイテム マスタープルフ
        uint cc_ocean_seal_itemid();  //CCアイテム 覇者の証
        uint cc_moon_bracelet_itemid();  //CCアイテム 月の腕輪
        uint cc_sun_bracelet_itemid(); //CCアイテム 太陽の腕輪

        uint cc_item_hero_crest_pointer();  //CCアイテム 英雄の証
        uint cc_item_knight_crest_pointer();  //CCアイテム 騎士の勲章
        uint cc_item_orion_bolt_pointer();  //CCアイテム オリオンの矢
        uint cc_elysian_whip_pointer();  //CCアイテム 天空のムチ
        uint cc_guiding_ring_pointer();  //CCアイテム 導きの指輪
        uint cc_fallen_contract_pointer();  //CCアイテム ダミー8A
        uint cc_master_seal_pointer();  //CCアイテム マスタープルフ
        uint cc_ocean_seal_pointer();  //CCアイテム 覇者の証
        uint cc_moon_bracelet_pointer();  //CCアイテム 月の腕輪
        uint cc_sun_bracelet_pointer();  //CCアイテム 太陽の腕輪

        uint unit_increase_height_pointer();  //ステータス画面で背を伸ばす
        uint unit_increase_height_switch2_address();
        uint op_class_demo_pointer();  //OPクラスデモ
        uint op_class_font_pointer();  //OPクラス日本語フォント
        uint op_class_font_palette_pointer(); // OPクラス紹介フォントのパレット
        uint status_font_pointer();  //ステータス画面用のフォント
        uint status_font_count();  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
        uint ed_staffroll_image_pointer(); // スタッフロール
        uint ed_staffroll_palette_pointer(); // スタッフロールのパレット
        uint op_prologue_image_pointer(); // OP字幕
        uint op_prologue_palette_color_pointer(); // OP字幕のパレット
        uint arena_class_near_weapon_pointer(); //闘技場 近接武器クラス 
        uint arena_class_far_weapon_pointer(); // 闘技場 遠距離武器クラス
        uint arena_class_magic_weapon_pointer(); // 闘技場 魔法武器クラス
        uint arena_enemy_weapon_basic_pointer(); // 闘技場 敵武器テーブル基本武器
        uint arena_enemy_weapon_rankup_pointer(); // 闘技場 敵武器テーブルランクアップ武器
        uint link_arena_deny_unit_pointer(); //通信闘技場 禁止ユニット 
        uint worldmap_road_pointer(); // ワールドマップの道
        uint menu_definiton_pointer();  //メニューの親玉 メニューを束ねる構造体です.
        uint menu_promotion_pointer(); //CC決定する選択子
        uint menu_promotion_branch_pointer(); //FE8にある分岐CC
        uint menu_definiton_split_pointer();  //FE8にある分岐メニュー
        uint menu_definiton_worldmap_pointer(); //FE8のワールドマップのメニュー
        uint menu_definiton_worldmap_shop_pointer(); //FE8のワールドマップ店のメニュー        
        uint menu_unit_pointer(); // ユニットメニュー
        uint menu_game_pointer(); // ゲームメニュー
        uint menu_debug1_pointer(); // デバッグメニュー
        uint MenuCommand_UsabilityAlways(); //メニューを開くという値を返す関数のアドレス
        uint MenuCommand_UsabilityNever(); //メニューを開かないという値を返す関数のアドレス       
        uint status_rmenu_unit_pointer(); // ステータス RMENU1
        uint status_rmenu_game_pointer(); // ステータス RMENU2
        uint status_rmenu3_pointer(); // ステータス RMENU3
        uint status_rmenu4_pointer(); // 戦闘予測 RMENU4
        uint status_rmenu5_pointer(); // 戦闘予測 RMENU5
        uint status_rmenu6_pointer(); // 状況画面 RMENU6
        uint status_param1_pointer(); // ステータス PARAM1
        uint status_param2_pointer(); // ステータス PARAM2
        uint status_param3w_pointer(); // ステータス PARAM3 武器
        uint status_param3m_pointer(); // ステータス PARAM3 魔法

        uint systemmenu_common_image_pointer(); //システムメニューの画像
        uint systemmenu_common_palette_pointer(); //システムパレット 無圧縮4パレット
        uint systemmenu_goal_tsa_pointer(); //システムメニュー 目的表示TSA
        uint systemmenu_terrain_tsa_pointer(); //システムメニュー 地形表示TSA

        uint systemmenu_name_image_pointer(); //システムメニュー 名前表示画像(FE8は共通画像)
        uint systemmenu_name_tsa_pointer(); //システムメニュー 名前表示TSA
        uint systemmenu_name_palette_pointer(); //システムメニュー 名前表示パレット

        uint systemmenu_battlepreview_image_pointer(); //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
        uint systemmenu_battlepreview_tsa_pointer(); //戦闘プレビューTSA
        uint systemmenu_battlepreview_palette_pointer(); //戦闘プレビューパレット

        uint systemarea_move_gradation_palette_pointer(); //行動範囲
        uint systemarea_attack_gradation_palette_pointer(); //攻撃範囲
        uint systemarea_staff_gradation_palette_pointer(); //杖の範囲

        uint systemmenu_badstatus_image_pointer(); //無圧縮のバッドステータス画像
        uint systemmenu_badstatus_palette_pointer(); //バッドステータスのパレット
        uint systemmenu_badstatus_old_image_pointer(); //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
        uint systemmenu_badstatus_old_palette_pointer(); //昔の圧縮のバッドステータス画像のパレット FE7 FE6

        uint bigcg_pointer(); // CG
        uint end_cg_address(); // END CG FE8のみ
        uint worldmap_big_image_pointer(); //ワールドマップ フィールドになるでかい奴  
        uint worldmap_big_palette_pointer(); //ワールドマップ フィールドになるでかい奴 パレット  
        uint worldmap_big_dpalette_pointer(); //ワールドマップ フィールドになるでかい奴 闇パレット  
        uint worldmap_big_palettemap_pointer(); //ワールドマップ フィールドになるでかい奴 パレットマップ
        uint worldmap_event_image_pointer(); //ワールドマップ イベント用 
        uint worldmap_event_palette_pointer(); //ワールドマップ イベント用 パレット  
        uint worldmap_event_tsa_pointer(); //ワールドマップ イベント用 TSA
        uint worldmap_mini_image_pointer(); //ワールドマップ ミニマップ 
        uint worldmap_mini_palette_pointer(); //ワールドマップ ミニマップ パレット  
        uint worldmap_icon_palette_pointer(); //ワールドアイコンと道パレット
        uint worldmap_icon1_pointer(); //ワールドマップ アイコン1
        uint worldmap_icon2_pointer(); //ワールドマップ アイコン2
        uint worldmap_road_tile_pointer();//ワールドマップ  道チップ
        uint map_load_function_pointer(); //マップチャプターに入ったときの処理(FE8のみ)
        uint map_load_function_switch1_address();
        uint system_icon_pointer();//システムアイコン集
        uint system_icon_palette_pointer(); //システム アイコンパレット集
        uint system_icon_width_address();//システムアイコンの幅が書かれているアドレス
        uint system_weapon_icon_pointer();//剣　斧　弓などの武器属性アイコン集
        uint system_weapon_icon_palette_pointer();//剣　斧　弓などの武器属性アイコン集のパレット
        uint system_music_icon_pointer();//音楽関係のアイコン集
        uint system_music_icon_palette_pointer();//音楽関係のアイコン集のパレット
        uint weapon_rank_s_bonus_address();//武器ランクSボーナス設定
        uint weapon_battle_flash_address();//神器 戦闘時フラッシュ
        uint weapon_effectiveness_2x3x_address();//神器 2倍 3倍特効
        uint font_item_address();//アイテム名とかに使われるフォント
        uint font_serif_address(); //セリフとかに使われるフォント
        uint monster_probability_pointer(); //魔物発生確率
        uint monster_item_item_pointer(); //魔物所持アイテム アイテム確率
        uint monster_item_probability_pointer(); //魔物所持アイテム 所持確率
        uint monster_item_table_pointer(); //魔物所持アイテム アイテムと所持を管理するテーブル
        uint monster_wmap_base_point_pointer(); //ワールドマップに魔物を登場させる処理達
        uint monster_wmap_stage_1_pointer();
        uint monster_wmap_stage_2_pointer();
        uint monster_wmap_probability_1_pointer();
        uint monster_wmap_probability_2_pointer();
        uint monster_wmap_probability_after_1_pointer();
        uint monster_wmap_probability_after_2_pointer();
        uint battle_bg_pointer(); //戦闘背景
        uint battle_terrain_pointer(); //戦闘地形
        uint senseki_comment_pointer(); //戦績コメント
        uint unit_custom_battle_anime_pointer(); //ユニット専用アニメ FE7にある
        uint magic_effect_pointer(); //魔法効果へのポインタ
        uint magic_effect_original_data_count(); //もともとあった魔法数
        uint system_move_allowicon_pointer();//移動するときの矢印アイコン
        uint system_move_allowicon_palette_pointer(); //移動するときの矢印アイコン アイコンパレット集
        uint system_tsa_16color_304x240_pointer(); //16色304x240 汎用TSAポインタ
        uint eventunit_data_size(); //ユニット配置のデータサイズ
        uint eventcond_tern_size(); //イベント条件 ターン条件のサイズ FE6->12 FE7->16 FE8->12
        uint eventcond_talk_size(); //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
        uint oping_event_pointer();
        uint ending1_event_pointer();
        uint ending2_event_pointer();
        uint RAMSlotTable_address();
        uint supply_pointer_address();  //輸送体RAMへのアドレス
        uint workmemory_player_units_address() ; //ワークメモリ PLAYER UNIT
        uint workmemory_enemy_units_address(); //ワークメモリ ENEMY UNIT
        uint workmemory_npc_units_address(); //ワークメモリ ENEMY UNIT
        uint workmemory_chapterdata_address();    //ワークメモリ 章データ
        uint workmemory_chapterdata_size();    //ワークメモリ 章データのサイズ
        uint workmemory_battle_actor_address();//ワークメモリ 戦闘時のユニット構造体
        uint workmemory_battle_target_address();//ワークメモリ 戦闘時のユニット構造体
        uint workmemory_worldmap_data_address();//ワークメモリ ワールドマップ関係の起点
        uint workmemory_worldmap_data_size(); //ワークメモリ ワールドマップ関係のサイズ
        uint workmemory_arena_data_address();//ワークメモリ 闘技場関係の起点
        uint workmemory_ai_data_address(); //ワークメモリ AI関係の起点
        uint workmemory_action_data_address(); //ワークメモリ ActionData
        uint workmemory_dungeon_data_address(); //ワークメモリ ダンジョン FE8のみ
        uint workmemory_battlesome_data_address(); //ワークメモリ バルトに関係する諸データ
        uint workmemory_battleround_data_address(); //ワークメモリ　戦闘のラウンドデータ
        uint workmemory_mapid_address();    //ワークメモリ マップID
        uint workmemory_last_string_address();  //ワークメモリ 最後に表示した文字列
        uint workmemory_text_buffer_address();  //ワークメモリ デコードされたテキスト
        uint workmemory_next_text_buffer_address();  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
        uint workmemory_local_flag_address();  //ワークメモリ グローバルフラグ
        uint workmemory_global_flag_address();  //ワークメモリ ローカルフラグ
        uint workmemory_trap_address();  //トラップデータ
        uint workmemory_bwl_address();  //BWLデータ
        uint workmemory_clear_turn_address();  //クリアターン数
        uint workmemory_clear_turn_count();  //クリアターン数 最大数
        uint workmemory_memoryslot_address();  //ワークメモリ メモリスロットFE8
        uint workmemory_eventcounter_address();  //イベントカウンター メモリスロットFE8
        uint workmemory_procs_forest_address();  //ワークメモリ Procs
        uint workmemory_procs_pool_address();  //ワークメモリ Procs
        uint function_sleep_handle_address();  //ワークメモリ Procs待機中
        uint workmemory_user_stack_base_address(); //ワークメモリ スタックの一番底
        uint function_fe_main_return_address(); //スタックの一番底にある戻り先
        uint workmemory_control_unit_address(); //ワークメモリ 操作ユニット
        uint workmemory_bgm_address(); //ワークメモリ BGM
        uint function_event_engine_loop_address(); //イベントエンジン
        uint workmemory_reference_procs_event_address_offset(); //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
        uint workmemory_procs_game_main_address(); //ワークメモリ Procsの中でのGAMEMAIN
        uint procs_game_main_address(); //PROCSのGAME MAIN 
        uint workmemory_palette_address(); //RAMに記録されているダブルバッファのパレット領域
        uint workmemory_sound_player_00_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_01_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_02_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_03_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_04_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_05_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_06_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_07_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint workmemory_sound_player_08_address(); //RAMに設定されているサウンドプレイヤーバッファ
        uint summon_unit_pointer(); //召喚
        uint summons_demon_king_pointer(); //呼魔
        uint summons_demon_king_count_address(); //呼魔リストの数
        uint mant_command_pointer(); //マント
        uint mant_command_startadd(); //マント開始数
        uint mant_command_count_address(); //マント数
        uint unit_increase_height_yes();  //ステータス画面で背を伸ばす 伸ばす
        uint unit_increase_height_no();  //ステータス画面で背を伸ばす 伸ばさない
        uint battle_screen_TSA1_pointer();  //戦闘画面
        uint battle_screen_TSA2_pointer();  //戦闘画面
        uint battle_screen_TSA3_pointer();  //戦闘画面
        uint battle_screen_TSA4_pointer();  //戦闘画面
        uint battle_screen_TSA5_pointer();  //戦闘画面
        uint battle_screen_palette_pointer();  //戦闘画面 パレット
        uint battle_screen_image1_pointer();  //戦闘画面 画像1
        uint battle_screen_image2_pointer();  //戦闘画面 画像2
        uint battle_screen_image3_pointer();  //戦闘画面 画像2
        uint battle_screen_image4_pointer();  //戦闘画面 画像2
        uint battle_screen_image5_pointer();  //戦闘画面 画像2
        uint ai1_pointer();  //AI1ポインタ
        uint ai2_pointer();  //AI2ポインタ
        uint ai3_pointer();  //AI3ポインタ
        uint ai_steal_item_pointer();  //AI盗む アイテム評価テーブル
        uint ai_preform_staff_pointer();  //AI杖 杖評価テーブル
        uint ai_preform_staff_direct_asm_pointer();  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
        uint ai_preform_item_pointer();  //AIアイテム アイテム評価テーブル
        uint ai_preform_item_direct_asm_pointer();  //AIアイテム アイテム評価テーブル
        uint ai_map_setting_pointer();  //AI 章ごとの設定テーブル
        uint item_usability_array_pointer(); //アイテムを利用できるか判定する
        uint item_usability_array_switch2_address();
        uint item_effect_array_pointer();    //アイテムを利用した場合の効果を定義する
        uint item_effect_array_switch2_address();
        uint item_promotion1_array_pointer();   //ターゲット
        uint item_promotion1_array_switch2_address();
        uint item_promotion2_array_pointer();  //CCアイテムかどうかを定義する(FE7のみ)
        uint item_promotion2_array_switch2_address();
        uint item_staff1_array_pointer();    //アイテムのターゲット選択の方法を定義する(多分)
        uint item_staff1_array_switch2_address();
        uint item_staff2_array_pointer();    //杖の種類を定義する
        uint item_staff2_array_switch2_address();
        uint item_statbooster1_array_pointer();    //ドーピングアイテムを利用した時のメッセージを定義する
        uint item_statbooster1_array_switch2_address();
        uint item_statbooster2_array_pointer();    //ドーピングアイテムとCCアイテムかどうかを定義する
        uint item_statbooster2_array_switch2_address();
        uint item_errormessage_array_pointer();    //アイテム利用時のエラーメッセージ
        uint item_errormessage_array_switch2_address();
        uint event_function_pointer_table_pointer();    //イベント命令ポインタ
        uint event_function_pointer_table2_pointer();    //イベント命令ポインタ2 ワールドマップ
        uint item_effect_pointer_table_pointer();   //間接エフェクトポインタ
        uint command_85_pointer_table_pointer();    //85Commandポインタ
        uint dic_main_pointer();     //辞書メインポインタ
        uint dic_chaptor_pointer();  //辞書章ポインタ
        uint dic_title_pointer();    //辞書タイトルポインタ
        uint itemicon_mine_id();  // アイテムアイコンのフレイボムの位置
        uint item_gold_id();  // お金を取得するイベントに利用されるゴールドのID
        uint unitaction_function_pointer();  // ユニットアクションポインタ
        uint lookup_table_battle_terrain_00_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_01_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_02_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_03_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_04_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_05_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_06_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_07_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_08_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_09_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_10_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_11_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_12_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_13_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_14_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_15_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_16_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_17_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_18_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_19_pointer(); //戦闘アニメの床
        uint lookup_table_battle_terrain_20_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_00_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_01_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_02_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_03_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_04_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_05_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_06_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_07_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_08_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_09_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_10_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_11_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_12_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_13_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_14_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_15_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_16_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_17_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_18_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_19_pointer(); //戦闘アニメの床
        uint lookup_table_battle_bg_20_pointer(); //戦闘アニメの床
        uint map_terrain_type_count(); //地形の種類の数
        uint menu_J12_always_address(); //メニューの表示判定関数 常に表示する
        uint menu_J12_hide_address();   //メニューの表示判定関数 表示しない
        uint status_game_option_pointer(); //ゲームオプション
        uint status_game_option_order_pointer(); //ゲームオプションの並び順
        uint status_game_option_order2_pointer(); //ゲームオプションの並び順2 FE7のみ
        uint status_game_option_order_count_address(); //ゲームオプションの個数
        uint status_units_menu_pointer(); //部隊メニュー
        uint tactician_affinity_pointer(); //軍師属性(FE7のみ)
        uint event_final_serif_pointer(); //終章セリフ(FE7のみ)
        uint compress_image_borderline_address(); //これ以降に圧縮画像が登場するというアドレス
        uint patch_C01_hack(out uint enable_value); //C01 patch
        uint patch_C48_hack(out uint enable_value); //C48 patch
        uint patch_16_tracks_12_sounds(out uint enable_value); //16_tracks_12_sounds patch
        uint patch_chaptor_names_text_fix(out uint enable_value); //章の名前をテキストにするパッチ
        uint patch_generic_enemy_portrait_extends(out uint enable_value);//一般兵の顔 拡張
        uint patch_stairs_hack(out uint enable_value); //階段拡張
        uint patch_unitaction_rework_hack(out uint enable_value); //ユニットアクションの拡張
        uint patch_write_build_version(out uint enable_value); //ビルドバージョンを書き込む
        uint builddate_address();
        byte[] defualt_event_script_term_code(); //イベント命令を終了させるディフォルトコード
        byte[] defualt_event_script_toplevel_code(); //イベント命令を終了させるディフォルトコード(各章のトップレベルのイベント)
        byte[] defualt_event_script_mapterm_code(); //イベント命令を終了させるディフォルトコード(WMAP)
        string get_shop_name(uint shop_object); //店の名前
        uint main_menu_width_address(); //メインメニューの幅
        uint map_default_count();    // ディフォルトのマップ数
        uint wait_menu_command_id(); //WaitメニューのID
        uint font_default_begin(); //フォント開始
        uint font_default_end();   //フォント終了
        uint item_name_article_pointer(); // a|an|the を切り替えるテーブル 英語版のみ
        uint item_name_article_switch2_address();
        uint vanilla_field_config_address();    //バニラのタイルセット よく使われもの
        uint vanilla_field_image_address();
        uint vanilla_village_config_address();
        uint vanilla_village_image_address();
        uint vanilla_casle_config_address();
        uint vanilla_casle_image_address();
        uint vanilla_plain_config_address();
        uint vanilla_plain_image_address();

        uint extends_address(); //拡張領域
        uint orignal_crc32(); //無改造ROMのCRC32
        bool is_multibyte();    // マルチバイトを利用するか？
        int version();    // バージョン
    };


    public class ROM
    {
        public string Filename { get; private set; }
        public IROMFEINFO RomInfo { get; private set; }
        public byte[] Data{ get; private set; }
        public bool Modified { get; private set; }
        public bool IsVirtualROM { get; private set; }

        public bool LoadLow(string name, byte[] data,string version)
        {
            this.Modified = false;
            this.Data = data;
            this.Filename = name;

            this.RomInfo = new ROMFE7JP();
            if (data.Length >= U.toOffset(this.RomInfo.extends_address()) 
                && version.IndexOf(this.RomInfo.game_id()) >= 0)
            {
                return true;
            }
            this.RomInfo = new ROMFE8JP();
            if (data.Length >= U.toOffset(this.RomInfo.extends_address())
                && version.IndexOf(this.RomInfo.game_id()) >= 0)
            {
                return true;
            }
            this.RomInfo = new ROMFE8U();
            if (data.Length >= U.toOffset(this.RomInfo.extends_address())
                && version.IndexOf(this.RomInfo.game_id()) >= 0)
            {
                return true;
            }
            this.RomInfo = new ROMFE7U();
            if (data.Length >= U.toOffset(this.RomInfo.extends_address())
                && version.IndexOf(this.RomInfo.game_id()) >= 0)
            {
                return true;
            }
            this.RomInfo = new ROMFE6JP();
            if (data.Length >= U.toOffset(this.RomInfo.extends_address())
                && version.IndexOf(this.RomInfo.game_id()) >= 0)
            {
                return true;
            }
            if (version == "NAZO")
            {
                this.RomInfo = new ROMFE0();
                return true;
            }
            return false;
        }
        public bool Load(string name,out string version)
        {
            if (!File.Exists(name))
            {
                version = "";
                return false;
            }

            byte[] data = File.ReadAllBytes(name);
            version = U.getASCIIString(data,U.toOffset(0x080000AC),6);

            return LoadLow(name, data, version);
        }

        public bool LoadForceVersion(string name,string forceversion)
        {
            if (!File.Exists(name))
            {
                return false;
            }

            byte[] data = File.ReadAllBytes(name);
            string version = U.getASCIIString(data, U.toOffset(0x080000AC), 6);

            if (forceversion.IndexOf("FE8J") >= 0)
            {
                version = new ROMFE8JP().game_id();
            }
            else if (forceversion.IndexOf("FE8U") >= 0)
            {
                version = new ROMFE8U().game_id();
            }
            else if (forceversion.IndexOf("FE7J") >= 0)
            {
                version = new ROMFE7JP().game_id();
            }
            else if (forceversion.IndexOf("FE7U") >= 0)
            {
                version = new ROMFE7U().game_id();
            }
            else if (forceversion.IndexOf("FE6") >= 0)
            {
                version = new ROMFE6JP().game_id();
            }
            else if (forceversion.IndexOf("NAZO") >= 0)
            {
                version = new ROMFE0().game_id();
            }
            else
            {
                return false;
            }

            return LoadLow(name, data, version);
        }

        public void Save(string name,bool silent)
        {
            U.WriteAllBytes(name, this.Data);

            if (!silent)
            {
                this.Modified = false;
            }
        }

        public bool IsEmpty(uint addr,uint count)
        {
            Debug.Assert(count > 0);
            if (addr + count >= this.Data.Length)
            {
                return false;
            }

            byte[] d = this.getBinaryData(addr, count);
            bool empty = true;
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i] != 0x00)
                {
                    empty = false;
                    break;
                }
            }
            if (empty)
            {
                return true;
            }
            empty = true;
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i] != 0xFF)
                {
                    empty = false;
                    break;
                }
            }
            return empty;
        }

        [MethodImpl(256)]
        public uint u32(uint addr)
        {
            return U.u32(Data, addr);
        }
        [MethodImpl(256)]
        public uint u16(uint addr)
        {
            return U.u16(Data, addr);
        }
        [MethodImpl(256)]
        public uint u8(uint addr)
        {
            return U.u8(Data, addr);
        }
        [MethodImpl(256)]
        public uint u4(uint addr, bool isHigh)
        {
            return U.u4(Data, addr, isHigh);
        }
        [MethodImpl(256)]
        public uint p32(uint addr)
        {
            if (addr >= this.Data.Length)
            {
                return 0;
            }
            uint a = u32(addr);

            a = U.toOffset(a);
            return a;
        }
        public uint p32p(uint addr)
        {
            uint a = p32(addr);
            return p32(a);
        }

        public void write_p32(uint addr, uint a)
        {
            U.write_u32(Data, addr, U.toPointer(a));
            Modified = true;
        }
        public void write_u32(uint addr, uint a)
        {
            U.write_u32(Data, addr, a);
            Modified = true;
        }
        public void write_u16(uint addr,uint a)
        {
            U.write_u16(Data, addr, a);
            Modified = true;
        }
        public void write_u8(uint addr,uint a)
        {
            U.write_u8(Data, addr, a);
            Modified = true;
        }
        public void write_u4(uint addr, uint a, bool isHigh)
        {
            U.write_u4(Data, addr, a, isHigh);
            Modified = true;
        }

        public bool write_resize_data(uint resize)
        {
            if (this.Data.Length == resize)
            {//サイズが同一なら何もしない
                return true;
            }
            if (resize > 0x02000000)
            {
                R.ShowStopError("32MB(0x02000000)より大きな領域を割り当てることはできません。\r\n要求サイズ:{0}", U.ToHexString(resize));
                return false;
            }
            if (Program.CommentCache != null)
            {
                Program.CommentCache.RemoveOverRange(resize);
            }

            //C#は refで プロパティを設定したものを渡せない愚かな仕様だから...
            //文句はMSまで.どうぞ.
            //Array.Resize(ref this.Data,(int)resize);
            byte[] _d = this.Data;
            Array.Resize(ref _d, (int)U.Padding4(resize));
            this.Data = _d;
            Modified = true;

            return true;
        }
        public void write_range(uint addr, byte[] write_data)
        {
            U.write_range(Data, addr, write_data);
            Modified = true;
        }
        public void write_fill(uint addr,uint length,byte fill = 0x00)
        {
//            Debug.Assert(length < 0xFFFF);
            U.write_fill(Data, addr, length, fill);
            Modified = true;
        }


        public void write_p32(uint addr, uint a, Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(addr, 4));
            write_p32(addr, a);
        }
        public void write_u32(uint addr, uint a, Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(addr, 4));
            write_u32(addr, a);
        }
        public void write_u16(uint addr, uint a, Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(addr, 2));
            write_u16(addr, a);
        }
        public void write_u8(uint addr, uint a, Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(addr, 1));
            write_u8(addr, a);
        }
        public void write_u4(uint addr, uint a, bool isHigh, Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(addr, 1));
            write_u4(addr, a, isHigh);
        }

        public void write_range(uint addr, byte[] write_data, Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(addr,(uint) write_data.Length));
            write_range(addr, write_data);
        }
        public void write_fill(uint addr,uint length,byte fill, Undo.UndoData undodata)
        {
            undodata.list.Add(new Undo.UndoPostion(addr,length));
            write_fill(addr, length, fill);
        }

        public uint getBlockDataCount(uint addr, uint blocksize, Func<int, uint, bool> is_data_exists_callback)
        {
            if (addr == 0 || addr == U.NOT_FOUND)
            {
                return 0;
            }

            uint length = (uint)Data.Length;
            int i = 0;
            while (addr + blocksize <= length)
            {
                if (!is_data_exists_callback(i,addr))
                {
                    return (uint)i;
                }
                addr += blocksize;
                i++;
            }

//            R.Error("警告:データが途中で終わってしまいました。 addr:{0} data.Length:{1} countI:{2}", U.ToHexString(addr), U.ToHexString(length), i);
//            Debug.Assert(false);
            return (uint)i;
        }

        public uint getBlockDataCount(uint addr, Func<int, uint, bool> is_data_exists_callback, Func<uint, uint> next_addr_callback,uint minimamLength)
        {
            if (addr == 0 || addr == U.NOT_FOUND)
            {
                return 0;
            }

            uint length = (uint)Data.Length;

            int i = 0;
            while (addr + minimamLength  <= length)
            {
                if (!is_data_exists_callback(i, addr))
                {
                    return (uint)i;
                }
                addr = next_addr_callback(addr);
                i++;
            }

//            R.Error("警告:データが途中で終わってしまいました。 addr:{0} data.Length:{1} countI:{2}", U.ToHexString(addr), U.ToHexString(length), i);
//            Debug.Assert(false);
            return (uint)i;
        }

        public String getString(uint addr, uint max_length = U.NOT_FOUND)
        {
            int length;
            return getString(addr, out length, max_length);
        }
        public String getString(uint addr, out int length, uint max_length = U.NOT_FOUND)
        {
            length = (int)getBlockDataCount(addr, 1, (i, p) => 
            {
                if (i >= max_length)
                {
                    return false;
                }
                return u8(p) != 0;
            });
            return getString(addr, length);
        }
        public String getString(uint addr,int length)
        {
            if (length == 0) return "";
            return Program.SystemTextEncoder.Decode(Data, (int)addr, length);
        }
        public String getASCIIString(uint addr, int length)
        {
            return U.getASCIIString(Program.ROM.Data, addr, length);
        }
        public byte[] getBinaryData(uint addr, int count)
        {
            return U.getBinaryData(this.Data, addr, count);
        }
        public byte[] getBinaryData(uint addr, uint count)
        {
            return U.getBinaryData(this.Data, addr, count);
        }

        //空き領域の探索.
        public uint FindFreeSpace(uint addr, uint needsize)
        {
            if (needsize > (uint)this.Data.Length)
            {
                return U.NOT_FOUND;
            }

            byte filldata = 0;

            uint length = (uint)this.Data.Length - needsize;
            addr = U.Padding4(addr);
            for (; addr < length; addr += 4)
            {
                if (this.Data[addr] == 0)
                {
                    filldata = 0x00;
                }
                else if (this.Data[addr] == 0xff)
                {
                    filldata = 0xff;
                }
                else
                {
                    continue;
                }

                uint start = addr;
                int  matchsize = 1;
                addr++;
                for (; addr < length; addr++)
                {
                    if (this.Data[addr] != filldata)
                    {
                        break;
                    }

                    matchsize++;
                    if (matchsize >= needsize)
                    {
                        return start;
                    }
                }
                addr = U.Padding4(addr);
            }
            return U.NOT_FOUND;
        }
        public void SetVirtualROMFlag(string srcfilename)
        {
            this.Filename = srcfilename + ".VIRTUAL";
            this.IsVirtualROM = true;
        }

        public void ClearModifiedFlag()
        {
            this.Modified = false;
        }

        public ROM Clone()
        {
            ROM newROM = new ROM();
            newROM.Filename = this.Filename;
            newROM.RomInfo = this.RomInfo; //書き込むデータがないので気にしない
            newROM.Data = (byte[])this.Data.Clone();
            newROM.Modified = this.Modified;
            newROM.IsVirtualROM = this.IsVirtualROM;
            return newROM;
        }

        public bool SwapNewROMData(byte[] newROMData, string name, Undo.UndoData undodata)
        {
            //0x0が壊されていないかチェックする
            byte[] romheader = Program.ROM.getBinaryData(0, 0x100);
            if (U.memcmp(U.subrange(newROMData, 0, 0x100), romheader) != 0)
            {
                System.Windows.Forms.DialogResult dr = R.ShowYesNo("event_assemblerが、0x0 - 0x100 の領域を書き換えました。\r\nこの領域への書き込みは危険です。\r\nこの内容を本当に適応してもよろしいですか？\r\n\r\n「はい」ならば適応します。\r\nそれ以外ならば変更を破棄します。");
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return false;
                }
            }

            //データが大きくなるので差分だけundoバッファに登録する
            const int RecoverMissMatch = 10;
            int checkpoint;
            int length = Math.Min(this.Data.Length, newROMData.Length);
            for (int i = 0; i < length; i++)
            {
                if (this.Data[i] == newROMData[i])
                {
                    continue;
                }

                checkpoint = i;

                i++;
                int missCount = 0;
                for (; i < length; i++)
                {
                    if (this.Data[i] != newROMData[i])
                    {
                        missCount = 0;
                        continue;
                    }

                    if (missCount >= RecoverMissMatch)
                    {
                        i -= missCount;
                        break;
                    }

                    missCount++;
                }

                uint size = (uint)(i - checkpoint);

                //checkpoint ～ i の間を相違点として記録.
                undodata.list.Add(new Undo.UndoPostion((uint)checkpoint , size ));
                //この範囲にコメントがある場合は再定義するので消す
                Program.CommentCache.RemoveRange((uint)checkpoint, (uint)checkpoint + size);
            }

            if (newROMData.Length != this.Data.Length)
            {//長さが増える場合、ROMを増設する.
                bool isResizeSuccess = this.write_resize_data((uint)newROMData.Length);
                if (isResizeSuccess == false)
                {
                    return false;
                }
            }
            this.write_range(0, newROMData);

            return true;
        }
        public void SwapNewROMDataDirect(byte[] newROMData)
        {
            this.Data = newROMData;
        }
        public bool CompareByte(uint addr, byte[] bin)
        {
            if (addr + bin.Length >= this.Data.Length)
            {
                return false;
            }

            for (uint i = 0; i < bin.Length; i++)
            {
                if (this.Data[addr + i] != bin[i])
                {
                    return false;
                }
            }
            return true;
        }

    }

}
    