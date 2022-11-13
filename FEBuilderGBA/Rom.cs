using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection;


namespace FEBuilderGBA
{
    public class ROMFEINFO
    {
        public String VersionToFilename { get; protected set; }
        public String TitleToFilename { get; protected set; }
        public uint mask_point_base_pointer { get; protected set; } // Huffman tree end (indirected twice)
        public uint mask_pointer { get; protected set; }  // Huffman tree start (indirected once)
        public uint text_pointer { get; protected set; } // textの開始位置
        public uint text_recover_address { get; protected set; } // textの開始位置(上記ポインタを壊している改造があるののでその対策)
        public uint text_data_start_address { get; protected set; } // textデータの規定値の開始位置
        public uint text_data_end_address { get; protected set; } // textデータの規定値の開始位置
        public uint unit_pointer { get; protected set; } // ユニットの開始位置
        public uint unit_maxcount { get; protected set; } // ユニットの最大数
        public uint unit_datasize { get; protected set; } // ユニットのデータサイズ
        public uint max_level_address { get; protected set; } // 最大レベルの値を格納しているアドレス
        public uint max_luck_address { get; protected set; } // 最大レベルの幸運の値を格納しているアドレス
        public uint class_pointer { get; protected set; } // クラスの開始位置
        public uint class_datasize { get; protected set; } // ユニットのデータサイズ
        public uint bg_pointer { get; protected set; } //BGベースアドレス
        public uint portrait_pointer { get; protected set; } //顔ベースアドレス
        public uint portrait_datasize { get; protected set; }
        public uint icon_pointer { get; protected set; } // アイコンの開始位置
        public uint icon_orignal_address { get; protected set; } // アイコンの初期値
        public uint icon_orignal_max { get; protected set; } // アイコンの最大数

        public uint icon_palette_pointer { get; protected set; } // アイコンのパレットの開始位置
        public uint unit_wait_icon_pointer { get; protected set; }  // ユニット待機アイコンの開始位置
        public uint unit_wait_barista_anime_address { get; protected set; }  // ユニット待機アイコンのバリスタのアニメ指定アドレス
        public uint unit_wait_barista_id { get; protected set; }  // ユニット待機アイコンのバリスタの位置
        public uint unit_icon_palette_address { get; protected set; } // ユニットのパレットの開始位置
        public uint unit_icon_npc_palette_address { get; protected set; } // ユニット(友軍)のパレットの開始位置
        public uint unit_icon_enemey_palette_address { get; protected set; } // ユニット(敵軍)のパレットの開始位置
        public uint unit_icon_gray_palette_address { get; protected set; } //ユニット(グレー)のパレットの開始位置
        public uint unit_icon_four_palette_address { get; protected set; } // ユニット(4軍))のパレットの開始位置
        public uint unit_icon_lightrune_palette_address { get; protected set; } // ユニット(光の結界)のパレットの開始位置
        public uint unit_icon_sepia_palette_address { get; protected set; } // ユニット(セピア)のパレットの開始位置
        public uint unit_move_icon_pointer { get; protected set; }  // ユニット移動アイコンの開始位置
        public uint lightrune_uniticon_id { get; protected set; } // ユニット(光の結界)のユニットアイコンのID
        public uint map_setting_pointer { get; protected set; }  // マップ設定の開始位置
        public uint map_setting_datasize { get; protected set; } //マップ設定のデータサイズ
        public uint map_setting_event_plist_pos { get; protected set; } //event plistの場所 
        public uint map_setting_worldmap_plist_pos { get; protected set; } //woldmap event plistの場所 
        public uint map_setting_clear_conditon_text_pos { get; protected set; } //マップの右上に表示されているクリア条件の定義場所 
        public uint map_setting_name_text_pos { get; protected set; } //マップ名のテキスト定義場所 
        public uint map_config_pointer { get; protected set; }      //マップ設定の開始位置(config)
        public uint map_obj_pointer { get; protected set; }         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_pal_pointer { get; protected set; }         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_tileanime1_pointer { get; protected set; }  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_tileanime2_pointer { get; protected set; }  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_map_pointer_pointer { get; protected set; } //マップ設定の開始位置(map)
        public uint map_mapchange_pointer { get; protected set; }   //マップ設定の開始位置(mapchange)
        public uint map_event_pointer { get; protected set; }       //マップ設定の開始位置(event)
        public uint map_worldmapevent_pointer { get; protected set; } //マップ設定の開始位置(worldmap (FE6のみ))
        public uint map_map_pointer_list_default_size { get; protected set; } //PLIST拡張をしていない時のバニラでのPLISTの数
        public uint image_battle_animelist_pointer { get; protected set; }  // 戦闘アニメリストの開始位置
        public uint support_unit_pointer { get; protected set; }  // 支援相手の開始位置
        public uint support_talk_pointer { get; protected set; }  // 支援会話の開始位置
        public uint unit_palette_color_pointer { get; protected set; }  // ユニットのパレット(カラー)の開始位置
        public uint unit_palette_class_pointer { get; protected set; }  // ユニットのパレット(クラス)の開始位置
        public uint support_attribute_pointer { get; protected set; }  //支援効果の開始位置
        public uint terrain_recovery_pointer { get; protected set; } //地形回復 全クラス共通
        public uint terrain_bad_status_recovery_pointer { get; protected set; } //地形回復 全クラス共通
        public uint terrain_show_infomation_pointer { get; protected set; } //地形ウィンドウに情報表示 全クラス共通
        public uint ccbranch_pointer { get; protected set; } // CC分岐の開始位置
        public uint ccbranch2_pointer { get; protected set; } // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
        public uint class_alphaname_pointer { get; protected set; } // クラスのアルファベット表記の開始位置
        public uint map_terrain_name_pointer { get; protected set; } // マップの地名表記の開始位置
        public uint image_chapter_title_pointer { get; protected set; } // 章タイトルの開始位置
        public uint image_chapter_title_palette { get; protected set; } // 章タイトルのパレット
        public uint image_unit_palette_pointer { get; protected set; } // ユニットパレットの開始位置
        public uint item_pointer { get; protected set; } // アイテムの開始位置
        public uint item_datasize { get; protected set; } // アイテムのデータサイズ
        public uint item_effect_pointer { get; protected set; } // アイテムエフェクトの開始位置
        public uint sound_table_pointer { get; protected set; } // ソングテーブルの開始位置
        public uint sound_room_pointer { get; protected set; } // サウンドルームの開始位置
        public uint sound_room_datasize { get; protected set; } // サウンドルームのデータサイズ
        public uint sound_room_cg_pointer { get; protected set; } // サウンドルームの背景リスト(FE7のみ)
        public uint event_ballte_talk_pointer { get; protected set; } // 交戦時セリフの開始位置
        public uint event_ballte_talk2_pointer { get; protected set; } // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
        public uint event_haiku_pointer { get; protected set; } // 死亡時セリフの開始位置
        public uint event_haiku_tutorial_1_pointer { get; protected set; } // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_haiku_tutorial_2_pointer { get; protected set; } // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_force_sortie_pointer { get; protected set; } // 強制出撃の開始位置
        public uint event_tutorial_pointer { get; protected set; } //イベントチュートリアルポインタ FE7のみ
        public uint map_exit_point_pointer { get; protected set; } // 離脱ポイント開始サイズ
        public uint map_exit_point_npc_blockadd { get; protected set; } // arr[+65] からNPCらしい.
        public uint map_exit_point_blank { get; protected set; } // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
        public uint sound_boss_bgm_pointer { get; protected set; } // ボスBGMの開始位置
        public uint sound_foot_steps_pointer { get; protected set; } // クラス足音の開始位置
        public uint sound_foot_steps_switch2_address { get; protected set; }
        public uint sound_foot_steps_data_pointer { get; protected set; } //足音のデータ構造の先頭
        public uint worldmap_scroll_somedata_pointer { get; protected set; }    //FE8ワールドマップのスクロール関係のデータ
        public uint worldmap_point_pointer { get; protected set; } // ワールドマップ拠点の開始位置
        public uint worldmap_bgm_pointer { get; protected set; } // ワールドマップのBGMテーブルの開始位置
        public uint worldmap_icon_data_pointer { get; protected set; } // ワールドマップのアイコンデータのテーブルの開始位置
        public uint worldmap_event_on_stageclear_pointer { get; protected set; } // ワールドマップイベント 拠点クリア時
        public uint worldmap_event_on_stageselect_pointer { get; protected set; } // ワールドマップイベント 拠点選択時
        public uint worldmap_county_border_pointer { get; protected set; } // ワールドマップ国名の表示
        public uint worldmap_county_border_palette_pointer { get; protected set; } // ワールドマップ国名の表示のパレット
        public uint item_shop_hensei_pointer { get; protected set; } //編成準備店
        public uint item_cornered_pointer { get; protected set; } //すくみり開始位置
        public uint ed_1_pointer { get; protected set; }  //ED 倒れたら撤退するかどうか 
        public uint ed_2_pointer { get; protected set; }  //ED 通り名
        public uint ed_3a_pointer { get; protected set; }  //ED その後 FE7 エリウッド FE8 エイルーク編
        public uint ed_3b_pointer { get; protected set; }  //ED その後 FE7 ヘクトル   FE8 エフラム編
        public uint ed_3c_pointer { get; protected set; }  //ED その後 FE7 リン編
        public uint generic_enemy_portrait_pointer { get; protected set; } //一般兵の顔
        public uint generic_enemy_portrait_count { get; protected set; } //一般兵の顔の個数
        public uint cc_item_hero_crest_itemid { get; protected set; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_itemid { get; protected set; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_itemid { get; protected set; } //CCアイテム オリオンの矢
        public uint cc_elysian_whip_itemid { get; protected set; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_itemid { get; protected set; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_itemid { get; protected set; } //CCアイテム ダミー8A
        public uint cc_master_seal_itemid { get; protected set; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_itemid { get; protected set; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_itemid { get; protected set; }  //CCアイテム 月の腕輪
        public uint cc_sun_bracelet_itemid { get; protected set; } //CCアイテム 太陽の腕輪

        public uint cc_item_hero_crest_pointer { get; protected set; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_pointer { get; protected set; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_pointer { get; protected set; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_pointer { get; protected set; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_pointer { get; protected set; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_pointer { get; protected set; }  //CCアイテム ダミー8A
        public uint cc_master_seal_pointer { get; protected set; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_pointer { get; protected set; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_pointer { get; protected set; }  //CCアイテム 月の腕輪
        public uint cc_sun_bracelet_pointer { get; protected set; }  //CCアイテム 太陽の腕輪

        public uint unit_increase_height_pointer { get; protected set; }  //ステータス画面で背を伸ばす
        public uint unit_increase_height_switch2_address { get; protected set; }
        public uint op_class_demo_pointer { get; protected set; }  //OPクラスデモ
        public uint op_class_font_pointer { get; protected set; }  //OPクラス日本語フォント
        public uint op_class_font_palette_pointer { get; protected set; } // OPクラス紹介フォントのパレット
        public uint status_font_pointer { get; protected set; }  //ステータス画面用のフォント
        public uint status_font_count { get; protected set; }  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
        public uint ed_staffroll_image_pointer { get; protected set; } // スタッフロール
        public uint ed_staffroll_palette_pointer { get; protected set; } // スタッフロールのパレット
        public uint op_prologue_image_pointer { get; protected set; } // OP字幕
        public uint op_prologue_palette_color_pointer { get; protected set; } // OP字幕のパレット
        public uint arena_class_near_weapon_pointer { get; protected set; } //闘技場 近接武器クラス 
        public uint arena_class_far_weapon_pointer { get; protected set; } // 闘技場 遠距離武器クラス
        public uint arena_class_magic_weapon_pointer { get; protected set; } // 闘技場 魔法武器クラス
        public uint arena_enemy_weapon_basic_pointer { get; protected set; } // 闘技場 敵武器テーブル基本武器
        public uint arena_enemy_weapon_rankup_pointer { get; protected set; } // 闘技場 敵武器テーブルランクアップ武器
        public uint link_arena_deny_unit_pointer { get; protected set; } //通信闘技場 禁止ユニット 
        public uint worldmap_road_pointer { get; protected set; } // ワールドマップの道
        public uint menu_definiton_pointer { get; protected set; }  //メニューの親玉 メニューを束ねる構造体です.
        public uint menu_promotion_pointer { get; protected set; } //CC決定する選択子
        public uint menu_promotion_branch_pointer { get; protected set; } //FE8にある分岐CC
        public uint menu_definiton_split_pointer { get; protected set; }  //FE8にある分岐メニュー
        public uint menu_definiton_worldmap_pointer { get; protected set; } //FE8のワールドマップのメニュー
        public uint menu_definiton_worldmap_shop_pointer { get; protected set; } //FE8のワールドマップ店のメニュー        
        public uint menu_unit_pointer { get; protected set; } // ユニットメニュー
        public uint menu_game_pointer { get; protected set; } // ゲームメニュー
        public uint menu_debug1_pointer { get; protected set; } // デバッグメニュー
        public uint menu_item_pointer { get; protected set; } // アイテム利用メニュー
        public uint MenuCommand_UsabilityAlways { get; protected set; } //メニューを開くという値を返す関数のアドレス
        public uint MenuCommand_UsabilityNever { get; protected set; } //メニューを開かないという値を返す関数のアドレス       
        public uint status_rmenu_unit_pointer { get; protected set; } // ステータス RMENU1
        public uint status_rmenu_game_pointer { get; protected set; } // ステータス RMENU2
        public uint status_rmenu3_pointer { get; protected set; } // ステータス RMENU3
        public uint status_rmenu4_pointer { get; protected set; } // 戦闘予測 RMENU4
        public uint status_rmenu5_pointer { get; protected set; } // 戦闘予測 RMENU5
        public uint status_rmenu6_pointer { get; protected set; } // 状況画面 RMENU6
        public uint status_param1_pointer { get; protected set; } // ステータス PARAM1
        public uint status_param2_pointer { get; protected set; } // ステータス PARAM2
        public uint status_param3w_pointer { get; protected set; } // ステータス PARAM3 武器
        public uint status_param3m_pointer { get; protected set; } // ステータス PARAM3 魔法

        public uint systemmenu_common_image_pointer { get; protected set; } //システムメニューの画像
        public uint systemmenu_common_palette_pointer { get; protected set; } //システムパレット 無圧縮4パレット
        public uint systemmenu_goal_tsa_pointer { get; protected set; } //システムメニュー 目的表示TSA
        public uint systemmenu_terrain_tsa_pointer { get; protected set; } //システムメニュー 地形表示TSA

        public uint systemmenu_name_image_pointer { get; protected set; } //システムメニュー 名前表示画像(FE8は共通画像)
        public uint systemmenu_name_tsa_pointer { get; protected set; } //システムメニュー 名前表示TSA
        public uint systemmenu_name_palette_pointer { get; protected set; } //システムメニュー 名前表示パレット

        public uint systemmenu_battlepreview_image_pointer { get; protected set; } //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
        public uint systemmenu_battlepreview_tsa_pointer { get; protected set; } //戦闘プレビューTSA
        public uint systemmenu_battlepreview_palette_pointer { get; protected set; } //戦闘プレビューパレット

        public uint systemarea_move_gradation_palette_pointer { get; protected set; } //行動範囲
        public uint systemarea_attack_gradation_palette_pointer { get; protected set; } //攻撃範囲
        public uint systemarea_staff_gradation_palette_pointer { get; protected set; } //杖の範囲

        public uint systemmenu_badstatus_image_pointer { get; protected set; } //無圧縮のバッドステータス画像
        public uint systemmenu_badstatus_palette_pointer { get; protected set; } //バッドステータスのパレット
        public uint systemmenu_badstatus_old_image_pointer { get; protected set; } //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
        public uint systemmenu_badstatus_old_palette_pointer { get; protected set; } //昔の圧縮のバッドステータス画像のパレット FE7 FE6

        public uint bigcg_pointer { get; protected set; } // CG
        public uint end_cg_address { get; protected set; } // END CG FE8のみ
        public uint worldmap_big_image_pointer { get; protected set; } //ワールドマップ フィールドになるでかい奴  
        public uint worldmap_big_palette_pointer { get; protected set; } //ワールドマップ フィールドになるでかい奴 パレット  
        public uint worldmap_big_dpalette_pointer { get; protected set; } //ワールドマップ フィールドになるでかい奴 闇パレット  
        public uint worldmap_big_palettemap_pointer { get; protected set; } //ワールドマップ フィールドになるでかい奴 パレットマップ
        public uint worldmap_event_image_pointer { get; protected set; } //ワールドマップ イベント用 
        public uint worldmap_event_palette_pointer { get; protected set; } //ワールドマップ イベント用 パレット  
        public uint worldmap_event_tsa_pointer { get; protected set; } //ワールドマップ イベント用 TSA
        public uint worldmap_mini_image_pointer { get; protected set; } //ワールドマップ ミニマップ 
        public uint worldmap_mini_palette_pointer { get; protected set; } //ワールドマップ ミニマップ パレット  
        public uint worldmap_icon_palette_pointer { get; protected set; } //ワールドアイコンと道パレット
        public uint worldmap_icon1_pointer { get; protected set; } //ワールドマップ アイコン1
        public uint worldmap_icon2_pointer { get; protected set; } //ワールドマップ アイコン2
        public uint worldmap_road_tile_pointer { get; protected set; }//ワールドマップ  道チップ
        public uint map_load_function_pointer { get; protected set; } //マップチャプターに入ったときの処理(FE8のみ)
        public uint map_load_function_switch1_address { get; protected set; }
        public uint system_icon_pointer { get; protected set; }//システムアイコン集
        public uint system_icon_palette_pointer { get; protected set; } //システム アイコンパレット集
        public uint system_icon_width_address { get; protected set; }//システムアイコンの幅が書かれているアドレス
        public uint system_weapon_icon_pointer { get; protected set; }//剣　斧　弓などの武器属性アイコン集
        public uint system_weapon_icon_palette_pointer { get; protected set; }//剣　斧　弓などの武器属性アイコン集のパレット
        public uint system_music_icon_pointer { get; protected set; }//音楽関係のアイコン集
        public uint system_music_icon_palette_pointer { get; protected set; }//音楽関係のアイコン集のパレット
        public uint weapon_rank_s_bonus_address { get; protected set; }//武器ランクSボーナス設定
        public uint weapon_battle_flash_address { get; protected set; }//神器 戦闘時フラッシュ
        public uint weapon_effectiveness_2x3x_address { get; protected set; }//神器 2倍 3倍特効
        public uint font_item_address { get; protected set; }//アイテム名とかに使われるフォント
        public uint font_serif_address { get; protected set; } //セリフとかに使われるフォント
        public uint monster_probability_pointer { get; protected set; } //魔物発生確率
        public uint monster_item_item_pointer { get; protected set; } //魔物所持アイテム アイテム確率
        public uint monster_item_probability_pointer { get; protected set; } //魔物所持アイテム 所持確率
        public uint monster_item_table_pointer { get; protected set; } //魔物所持アイテム アイテムと所持を管理するテーブル
        public uint monster_wmap_base_point_pointer { get; protected set; } //ワールドマップに魔物を登場させる処理達
        public uint monster_wmap_stage_1_pointer { get; protected set; }
        public uint monster_wmap_stage_2_pointer { get; protected set; }
        public uint monster_wmap_probability_1_pointer { get; protected set; }
        public uint monster_wmap_probability_2_pointer { get; protected set; }
        public uint monster_wmap_probability_after_1_pointer { get; protected set; }
        public uint monster_wmap_probability_after_2_pointer { get; protected set; }
        public uint battle_bg_pointer { get; protected set; } //戦闘背景
        public uint battle_terrain_pointer { get; protected set; } //戦闘地形
        public uint senseki_comment_pointer { get; protected set; } //戦績コメント
        public uint unit_custom_battle_anime_pointer { get; protected set; } //ユニット専用アニメ FE7にある
        public uint magic_effect_pointer { get; protected set; } //魔法効果へのポインタ
        public uint magic_effect_original_data_count { get; protected set; } //もともとあった魔法数
        public uint system_move_allowicon_pointer { get; protected set; }//移動するときの矢印アイコン
        public uint system_move_allowicon_palette_pointer { get; protected set; } //移動するときの矢印アイコン アイコンパレット集
        public uint system_tsa_16color_304x240_pointer { get; protected set; } //16色304x240 汎用TSAポインタ
        public uint eventunit_data_size { get; protected set; } //ユニット配置のデータサイズ
        public uint eventcond_tern_size { get; protected set; } //イベント条件 ターン条件のサイズ FE6->12 FE7->16 FE8->12
        public uint eventcond_talk_size { get; protected set; } //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
        public uint oping_event_pointer { get; protected set; }
        public uint ending1_event_pointer { get; protected set; }
        public uint ending2_event_pointer { get; protected set; }
        public uint RAMSlotTable_address { get; protected set; }
        public uint supply_pointer_address { get; protected set; }  //輸送体RAMへのアドレス
        public uint workmemory_player_units_address { get; protected set; } //ワークメモリ PLAYER UNIT
        public uint workmemory_enemy_units_address { get; protected set; } //ワークメモリ ENEMY UNIT
        public uint workmemory_npc_units_address { get; protected set; } //ワークメモリ ENEMY UNIT
        public uint workmemory_chapterdata_address { get; protected set; }    //ワークメモリ 章データ
        public uint workmemory_chapterdata_size { get; protected set; }    //ワークメモリ 章データのサイズ
        public uint workmemory_battle_actor_address { get; protected set; }//ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_battle_target_address { get; protected set; }//ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_worldmap_data_address { get; protected set; }//ワークメモリ ワールドマップ関係の起点
        public uint workmemory_worldmap_data_size { get; protected set; } //ワークメモリ ワールドマップ関係のサイズ
        public uint workmemory_arena_data_address { get; protected set; }//ワークメモリ 闘技場関係の起点
        public uint workmemory_ai_data_address { get; protected set; } //ワークメモリ AI関係の起点
        public uint workmemory_action_data_address { get; protected set; } //ワークメモリ ActionData
        public uint workmemory_dungeon_data_address { get; protected set; } //ワークメモリ ダンジョン FE8のみ
        public uint workmemory_battlesome_data_address { get; protected set; } //ワークメモリ バルトに関係する諸データ
        public uint workmemory_battleround_data_address { get; protected set; } //ワークメモリ　戦闘のラウンドデータ
        public uint workmemory_mapid_address { get; protected set; }    //ワークメモリ マップID
        public uint workmemory_last_string_address { get; protected set; }  //ワークメモリ 最後に表示した文字列
        public uint workmemory_text_buffer_address { get; protected set; }  //ワークメモリ デコードされたテキスト
        public uint workmemory_next_text_buffer_address { get; protected set; }  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
        public uint workmemory_local_flag_address { get; protected set; }  //ワークメモリ グローバルフラグ
        public uint workmemory_global_flag_address { get; protected set; }  //ワークメモリ ローカルフラグ
        public uint workmemory_trap_address { get; protected set; }  //トラップデータ
        public uint workmemory_bwl_address { get; protected set; }  //BWLデータ
        public uint workmemory_clear_turn_address { get; protected set; }  //クリアターン数
        public uint workmemory_clear_turn_count { get; protected set; }  //クリアターン数 最大数
        public uint workmemory_memoryslot_address { get; protected set; }  //ワークメモリ メモリスロットFE8
        public uint workmemory_eventcounter_address { get; protected set; }  //イベントカウンター メモリスロットFE8
        public uint workmemory_procs_forest_address { get; protected set; }  //ワークメモリ Procs
        public uint workmemory_procs_pool_address { get; protected set; }  //ワークメモリ Procs
        public uint function_sleep_handle_address { get; protected set; }  //ワークメモリ Procs待機中
        public uint workmemory_user_stack_base_address { get; protected set; } //ワークメモリ スタックの一番底
        public uint function_fe_main_return_address { get; protected set; } //スタックの一番底にある戻り先
        public uint workmemory_control_unit_address { get; protected set; } //ワークメモリ 操作ユニット
        public uint workmemory_bgm_address { get; protected set; } //ワークメモリ BGM
        public uint function_event_engine_loop_address { get; protected set; } //イベントエンジン
        public uint workmemory_reference_procs_event_address_offset { get; protected set; } //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
        public uint workmemory_procs_game_main_address { get; protected set; } //ワークメモリ Procsの中でのGAMEMAIN
        public uint procs_game_main_address { get; protected set; } //PROCSのGAME MAIN 
        public uint workmemory_palette_address { get; protected set; } //RAMに記録されているダブルバッファのパレット領域
        public uint workmemory_sound_player_00_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_01_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_02_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_03_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_04_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_05_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_06_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_07_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_08_address { get; protected set; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_keybuffer_address { get; protected set; } //RAMのキーバッファ
        public uint procs_maptask_pointer { get; protected set; } //PROCSのMAPTASK 
        public uint procs_soundroomUI_pointer { get; protected set; } //PROCSのSoundRoomUI 
        public uint summon_unit_pointer { get; protected set; } //召喚
        public uint summons_demon_king_pointer { get; protected set; } //呼魔
        public uint summons_demon_king_count_address { get; protected set; } //呼魔リストの数
        public uint mant_command_pointer { get; protected set; } //マント
        public uint mant_command_startadd { get; protected set; } //マント開始数
        public uint mant_command_count_address { get; protected set; } //マント数
        public uint unit_increase_height_yes { get; protected set; }  //ステータス画面で背を伸ばす 伸ばす
        public uint unit_increase_height_no { get; protected set; }  //ステータス画面で背を伸ばす 伸ばさない
        public uint battle_screen_TSA1_pointer { get; protected set; }  //戦闘画面
        public uint battle_screen_TSA2_pointer { get; protected set; }  //戦闘画面
        public uint battle_screen_TSA3_pointer { get; protected set; }  //戦闘画面
        public uint battle_screen_TSA4_pointer { get; protected set; }  //戦闘画面
        public uint battle_screen_TSA5_pointer { get; protected set; }  //戦闘画面
        public uint battle_screen_palette_pointer { get; protected set; }  //戦闘画面 パレット
        public uint battle_screen_image1_pointer { get; protected set; }  //戦闘画面 画像1
        public uint battle_screen_image2_pointer { get; protected set; }  //戦闘画面 画像2
        public uint battle_screen_image3_pointer { get; protected set; }  //戦闘画面 画像2
        public uint battle_screen_image4_pointer { get; protected set; }  //戦闘画面 画像2
        public uint battle_screen_image5_pointer { get; protected set; }  //戦闘画面 画像2
        public uint ai1_pointer { get; protected set; }  //AI1ポインタ
        public uint ai2_pointer { get; protected set; }  //AI2ポインタ
        public uint ai3_pointer { get; protected set; }  //AI3ポインタ
        public uint ai_steal_item_pointer { get; protected set; }  //AI盗む アイテム評価テーブル
        public uint ai_preform_staff_pointer { get; protected set; }  //AI杖 杖評価テーブル
        public uint ai_preform_staff_direct_asm_pointer { get; protected set; }  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
        public uint ai_preform_item_pointer { get; protected set; }  //AIアイテム アイテム評価テーブル
        public uint ai_preform_item_direct_asm_pointer { get; protected set; }  //AIアイテム アイテム評価テーブル
        public uint ai_map_setting_pointer { get; protected set; }  //AI 章ごとの設定テーブル
        public uint item_usability_array_pointer { get; protected set; } //アイテムを利用できるか判定する
        public uint item_usability_array_switch2_address { get; protected set; }
        public uint item_effect_array_pointer { get; protected set; }    //アイテムを利用した場合の効果を定義する
        public uint item_effect_array_switch2_address { get; protected set; }
        public uint item_promotion1_array_pointer { get; protected set; }   //ターゲット
        public uint item_promotion1_array_switch2_address { get; protected set; }
        public uint item_promotion2_array_pointer { get; protected set; }  //CCアイテムかどうかを定義する(FE7のみ)
        public uint item_promotion2_array_switch2_address { get; protected set; }
        public uint item_staff1_array_pointer { get; protected set; }    //アイテムのターゲット選択の方法を定義する(多分)
        public uint item_staff1_array_switch2_address { get; protected set; }
        public uint item_staff2_array_pointer { get; protected set; }    //杖の種類を定義する
        public uint item_staff2_array_switch2_address { get; protected set; }
        public uint item_statbooster1_array_pointer { get; protected set; }    //ドーピングアイテムを利用した時のメッセージを定義する
        public uint item_statbooster1_array_switch2_address { get; protected set; }
        public uint item_statbooster2_array_pointer { get; protected set; }    //ドーピングアイテムとCCアイテムかどうかを定義する
        public uint item_statbooster2_array_switch2_address { get; protected set; }
        public uint item_errormessage_array_pointer { get; protected set; }    //アイテム利用時のエラーメッセージ
        public uint item_errormessage_array_switch2_address { get; protected set; }
        public uint event_function_pointer_table_pointer { get; protected set; }    //イベント命令ポインタ
        public uint event_function_pointer_table2_pointer { get; protected set; }    //イベント命令ポインタ2 ワールドマップ
        public uint item_effect_pointer_table_pointer { get; protected set; }   //間接エフェクトポインタ
        public uint command_85_pointer_table_pointer { get; protected set; }    //85Commandポインタ
        public uint dic_main_pointer { get; protected set; }     //辞書メインポインタ
        public uint dic_chaptor_pointer { get; protected set; }  //辞書章ポインタ
        public uint dic_title_pointer { get; protected set; }    //辞書タイトルポインタ
        public uint itemicon_mine_id { get; protected set; }  // アイテムアイコンのフレイボムの位置
        public uint item_gold_id { get; protected set; }  // お金を取得するイベントに利用されるゴールドのID
        public uint unitaction_function_pointer { get; protected set; }  // ユニットアクションポインタ
        public uint lookup_table_battle_terrain_00_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_01_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_02_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_03_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_04_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_05_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_06_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_07_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_08_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_09_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_10_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_11_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_12_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_13_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_14_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_15_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_16_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_17_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_18_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_19_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_20_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_00_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_01_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_02_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_03_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_04_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_05_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_06_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_07_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_08_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_09_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_10_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_11_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_12_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_13_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_14_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_15_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_16_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_17_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_18_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_19_pointer { get; protected set; } //戦闘アニメの床
        public uint lookup_table_battle_bg_20_pointer { get; protected set; } //戦闘アニメの床
        public uint map_terrain_type_count { get; protected set; } //地形の種類の数
        public uint menu_J12_always_address { get; protected set; } //メニューの表示判定関数 常に表示する
        public uint menu_J12_hide_address { get; protected set; }   //メニューの表示判定関数 表示しない
        public uint status_game_option_pointer { get; protected set; } //ゲームオプション
        public uint status_game_option_order_pointer { get; protected set; } //ゲームオプションの並び順
        public uint status_game_option_order2_pointer { get; protected set; } //ゲームオプションの並び順2 FE7のみ
        public uint status_game_option_order_count_address { get; protected set; } //ゲームオプションの個数
        public uint status_units_menu_pointer { get; protected set; } //部隊メニュー
        public uint tactician_affinity_pointer { get; protected set; } //軍師属性(FE7のみ)
        public uint event_final_serif_pointer { get; protected set; } //終章セリフ(FE7のみ)
        public uint compress_image_borderline_address { get; protected set; } //これ以降に圧縮画像が登場するというアドレス
        public uint builddate_address { get; protected set; }
        public byte[] Default_event_script_term_code { get; protected set; } //イベント命令を終了させるディフォルトコード
        public byte[] Default_event_script_toplevel_code { get; protected set; } //イベント命令を終了させるディフォルトコード(各章のトップレベルのイベント)
        public byte[] Default_event_script_mapterm_code { get; protected set; } //イベント命令を終了させるディフォルトコード(WMAP)
        public uint main_menu_width_address { get; protected set; } //メインメニューの幅
        public uint map_default_count { get; protected set; }    // ディフォルトのマップ数
        public uint wait_menu_command_id { get; protected set; } //WaitメニューのID
        public uint font_default_begin { get; protected set; } //フォント開始
        public uint font_default_end { get; protected set; }   //フォント終了
        public uint item_name_article_pointer { get; protected set; } // a|an|the を切り替えるテーブル 英語版のみ
        public uint item_name_article_switch2_address { get; protected set; }
        public uint vanilla_field_config_address { get; protected set; }    //バニラのタイルセット よく使われもの
        public uint vanilla_field_image_address { get; protected set; }
        public uint vanilla_village_config_address { get; protected set; }
        public uint vanilla_village_image_address { get; protected set; }
        public uint vanilla_casle_config_address { get; protected set; }
        public uint vanilla_casle_image_address { get; protected set; }
        public uint vanilla_plain_config_address { get; protected set; }
        public uint vanilla_plain_image_address { get; protected set; }
        public uint map_minimap_tile_array_pointer { get; protected set; } //minimapのチップ割り当て
        public uint bg_reserve_black_bgid { get; protected set; }
        public uint bg_reserve_random_bgid { get; protected set; }

        public uint item_vanilla_address { get; protected set; } // バニラのアイテムアドレス
        public uint itemicon_vanilla_address { get; protected set; } // バニラのアイテムアイコンアドレス
        public uint class_vanilla_address { get; protected set; } // バニラのクラスアドレス

        public uint extends_address { get; protected set; } //拡張領域
        public uint orignal_crc32 { get; protected set; } //無改造ROMのCRC32
        public bool is_multibyte { get; protected set; }    // マルチバイトを利用するか？
        public int version { get; protected set; }    // バージョン

        virtual public uint patch_C01_hack(out uint enable_value) { enable_value = 0x0; return 0x0; } //C01 patch
        virtual public uint patch_C48_hack(out uint enable_value) { enable_value = 0x0; return 0x0; } //C48 patch
        virtual public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x0; return 0x0; } //16_tracks_12_sounds patch
        virtual public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x0; } //章の名前をテキストにするパッチ
        virtual public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x0; return 0x0; } //一般兵の顔 拡張
        virtual public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x0; return 0x0; } //ユニットアクションの拡張
        virtual public uint patch_write_build_version(out uint enable_value) { enable_value = 0x0; return 0x0; } //ビルドバージョンを書き込む
        virtual public string get_shop_name(uint shop_object)//店の名前
        {
            return "";
        }
        //ROM定数の上書き
        protected void OverwriteROMConstants(ROM rom)
        {
            string custom_pointer_filename = U.ChangeExtFilename(rom.Filename, ".custom_pointer.txt");
            if (!File.Exists(custom_pointer_filename))
            {
                return;
            }
            Dictionary<string, uint> map = new Dictionary<string, uint>();
            try
            {
                using (StreamReader reader = File.OpenText(custom_pointer_filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (U.IsComment(line))
                        {
                            continue;
                        }
                        int term = U.ClipCommentIndexOf(line, "//");
                        if (term >= 0)
                        {//コメント
                            line = line.Substring(0, term);
                        }
                        if (line == "")
                        {
                            continue;
                        }
                        string[] sp = line.Split('\t');
                        if (sp.Length < 2)
                        {
                            continue;
                        }
                        map[sp[0]] = U.atoi0x(sp[1]);
                    }
                }

                MemberInfo[] members = this.GetType().GetMembers();
                foreach (MemberInfo info in members)
                {
                    if (info.MemberType != MemberTypes.Property)
                    {
                        continue;
                    }
                    uint v;
                    if (!map.TryGetValue(info.Name, out v))
                    {
                        continue;
                    }

                    PropertyInfo field = (PropertyInfo)info;
                    if (field.PropertyType == typeof(uint))
                    {
                        field.SetValue(this, v, null);
                    }
                    else if (field.PropertyType == typeof(int))
                    {
                        field.SetValue(this, (int)v, null);
                    }
                    else if (field.PropertyType == typeof(bool))
                    {
                        bool b = (v == 0 ? false : true);
                        field.SetValue(this, b, null);
                    }

                }
            }
            catch(Exception)
            {
                return ;
            }

        }
    };


    public class ROM
    {
        public string Filename { get; protected set; }
        public ROMFEINFO RomInfo { get; protected set; }
        public byte[] Data{ get; protected set; }
        public bool Modified { get; protected set; }
        public bool IsVirtualROM { get; protected set; }

        public bool LoadLow(string name, byte[] data,string version)
        {
            this.Modified = false;
            this.Data = data;
            this.Filename = name;

            if (data.Length >= 0x1000000 
                && version.IndexOf("AE7J01") >= 0)
            {
                this.RomInfo = new ROMFE7JP(this);
                return true;
            }
            if (data.Length >= 0x1000000
                && version.IndexOf("BE8J01") >= 0)
            {
                this.RomInfo = new ROMFE8JP(this);
                return true;
            }
            if (data.Length >= 0x1000000
                && version.IndexOf("BE8E01") >= 0)
            {
                this.RomInfo = new ROMFE8U(this);
                return true;
            }
            if (data.Length >= 0x1000000
                && version.IndexOf("AE7E01") >= 0)
            {
                this.RomInfo = new ROMFE7U(this);
                return true;
            }
            if (data.Length >= 0x800000
                && version.IndexOf("AFEJ01") >= 0)
            {
                this.RomInfo = new ROMFE6JP(this);
                return true;
            }
            if (version == "NAZO")
            {
                this.RomInfo = new ROMFE0(this);
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
                version = "BE8J01";
            }
            else if (forceversion.IndexOf("FE8U") >= 0)
            {
                version = "BE8E01";
            }
            else if (forceversion.IndexOf("FE7J") >= 0)
            {
                version = "AE7J01";
            }
            else if (forceversion.IndexOf("FE7U") >= 0)
            {
                version = "AE7E01";
            }
            else if (forceversion.IndexOf("FE6") >= 0)
            {
                version = "AFEJ01";
            }
            else if (forceversion.IndexOf("NAZO") >= 0)
            {
                version = "NAZO";
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

            //OutOfMemoryException が発生することがあるらしいので、メモリを解放しておきます
            GC.Collect();

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
            string str =  Program.SystemTextEncoder.Decode(Data, (int)addr, length);
            str = str.TrimEnd('\0');
            return str;
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
    