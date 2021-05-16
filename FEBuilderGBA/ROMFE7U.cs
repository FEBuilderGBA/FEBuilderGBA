﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE7U : IROMFEINFO
    {
        public String game_id() { return "AE7E01"; }    // ゲームバージョンコード
        public String VersionToFilename() { return "FE7U"; }
        public String TitleToFilename() { return "FE7"; }
        public uint mask_point_base_pointer() { return 0x0006B8; } // Huffman tree end (indirected twice)
        public uint mask_pointer() { return 0x0006BC; }  // Huffman tree start (indirected once)
        public uint text_pointer() { return 0x12CB8; } // textの開始位置
        public uint text_recover_address() { return 0xB808AC; } // textの開始位置(上記ポインタを壊している改造があるののでその対策)
        public uint text_data_start_address() { return 0xAEAE8C; } //textデータの規定値の開始位置
        public uint text_data_end_address()   { return 0xB7D719; } //textデータの規定値の開始位置
        public uint unit_pointer() { return 0x9A274; } //ユニットの開始位置
        public uint unit_maxcount() { return 253; } // ユニットの最大数
        public uint unit_datasize() { return 52; } // ユニットのデータサイズ
        public uint max_level_address() { return 0x29692; } //最大レベルの値を格納しているアドレス
        public uint max_luck_address() { return 0x29a5e; } // 最大レベルの幸運の値を格納しているアドレス
        public uint class_pointer() { return 0x178f0; } //クラスの開始位置
        public uint class_datasize() { return 84; }  // ユニットのデータサイズ
        public uint bg_pointer() { return 0xB874; } //BGベースアドレス
        public uint face_pointer() { return 0x6b30; } //顔ベースアドレス
        public uint face_datasize() { return 28; }
        public uint icon_pointer() { return 0x4E20; } // アイコンの開始位置
        public uint icon_orignal_address() { return 0xC5EA4; } // アイコンの初期値
        public uint icon_orignal_max() { return 0xAC; } // アイコンの最大数

        public uint icon_palette_pointer() { return 0x4D40; } // アイコンのパレットの開始位置
        public uint unit_wait_icon_pointer() { return 0x024DA0; } // ユニット待機アイコンの開始位置
        public uint unit_wait_barista_anime_address() { return 0x25844; }  // ユニット待機アイコンのバリスタのアニメ指定アドレス
        public uint unit_wait_barista_id() { return 0x52; }  // ユニット待機アイコンのバリスタの位置
        public uint unit_icon_palette_address() { return 0x194594; } //ユニットのパレットの開始位置
        public uint unit_icon_enemey_palette_address() { return 0x1945B4; } //ユニット(敵軍)のパレットの開始位置
        public uint unit_icon_npc_palette_address() { return 0x1945D4; } //ユニット(友軍)のパレットの開始位置
        public uint unit_icon_gray_palette_address() { return 0x1945F4; } // ユニット(グレー))のパレットの開始位置
        public uint unit_icon_four_palette_address() { return 0x194614; } // ユニット(4軍))のパレットの開始位置
        public uint unit_icon_lightrune_palette_address() { return 0x194634; } // ユニット(光の結界)のパレットの開始位置
        public uint unit_icon_sepia_palette_address() { return 0x194654; } // ユニット(セピア)のパレットの開始位置

        public uint unit_move_icon_pointer() { return 0x6D574; } // ユニット移動アイコンの開始位置
        public uint lightrune_uniticon_id() { return 0x57; } // ユニット(光の結界)のユニットアイコンのID
        public uint map_setting_pointer() { return 0x31580; }  // マップ設定の開始位置
        public uint map_setting_datasize() { return 152; } //マップ設定のデータサイズ
        public uint map_setting_event_plist_pos() { return 120; } //event plistの場所 
        public uint map_setting_worldmap_plist_pos() { return 121; } //woldmap event plistの場所 
        public uint map_setting_clear_conditon_text_pos() { return 0x8E; } //マップの右上に表示されているクリア条件の定義場所 
        public uint map_setting_name_text_pos() { return 0x74; } //マップ名のテキスト定義場所 
        public uint map_config_pointer() { return 0x191C8; }      //マップ設定の開始位置(config)
        public uint map_obj_pointer() { return 0x019230; }         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_pal_pointer() { return 0x019264; }         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_tileanime1_pointer() { return 0x02D364; }  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_tileanime2_pointer() { return 0x02DEA4; }  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_map_pointer_pointer() { return 0x03159C; } //マップ設定の開始位置(map)
        public uint map_mapchange_pointer() { return 0x0315B8; }   //マップ設定の開始位置(mapchange)
        public uint map_event_pointer() { return 0x315D4; }       //マップ設定の開始位置(event)
        public uint map_worldmapevent_pointer() { return 0x0; } //マップ設定の開始位置(worldmap (FE6のみ))
        public uint image_battle_animelist_pointer() { return 0x0541F4; }   // 戦闘アニメリストの開始位置
        public uint support_unit_pointer() { return 0xBDCE78; }   // 支援相手の開始位置
        public uint support_talk_pointer() { return 0x78A94; }   // 支援会話の開始位置
        public uint unit_palette_color_pointer() { return 0x0; }  // ユニットのパレット(カラー)の開始位置
        public uint unit_palette_class_pointer() { return 0x0; }  // ユニットのパレット(クラス)の開始位置
        public uint support_attribute_pointer() { return 0x26998; }  //支援効果の開始位置
        public uint terrain_recovery_pointer() { return 0x19B2C; } //地形回復 全クラス共通
        public uint terrain_bad_status_recovery_pointer() { return 0x019B3C; } //地形回復 全クラス共通
        public uint ccbranch_pointer() { return 0x0; } // CC分岐の開始位置
        public uint ccbranch2_pointer() { return 0x0; } // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
        public uint class_alphaname_pointer() { return 0x0; } // クラスのアルファベット表記の開始位置
        public uint map_terrain_name_pointer() { return 0x19B1C; } //マップの地名表記の開始位置
        public uint image_chapter_title_pointer() { return 0x0; } //不明 章タイトルの開始位置
        public uint image_chapter_title_palette() { return 0x3FE438; } // 章タイトルのパレット 多分違う
        public uint image_unit_palette_pointer() { return 0x541F8; } // ユニットパレットの開始位置
        public uint item_pointer() { return 0x174D4; } //アイテムの開始位置
        public uint item_datasize() { return 36; } // アイテムのデータサイズ
        public uint item_effect_pointer() { return 0x052B24; } // アイテムエフェクトの開始位置
        public uint sound_table_pointer() { return 0x3F50; } // ソングテーブルの開始位置
        public uint sound_room_pointer() { return 0x1B468; } //サウンドルームの開始位置
        public uint sound_room_datasize() { return 16; } // サウンドルームのデータサイズ
        public uint sound_room_cg_pointer() { return 0xAB4C0; } // サウンドルームの背景リスト(FE7のみ)
        public uint event_ballte_talk_pointer() { return 0x792e0; } //交戦時セリフの開始位置
        public uint event_ballte_talk2_pointer() { return 0x79454; } // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
        public uint event_haiku_pointer() { return 0x79550; } //死亡時セリフの開始位置
        public uint event_haiku_tutorial_1_pointer() { return 0x7955C; } // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_haiku_tutorial_2_pointer() { return 0x79558; } // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_force_sortie_pointer() { return 0x8DDCC; } // 強制出撃の開始位置
        public uint event_tutorial_pointer() { return 0x79000; } //イベントチュートリアルポインタ FE7のみ
        public uint map_exit_point_pointer() { return 0x39970; } // 離脱ポxxイント開始サイズ
        public uint map_exit_point_npc_blockadd() { return 0x30; } // arr[+0x30] からNPCらしい.
        public uint map_exit_point_blank() { return 0x1D3A5C; } // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
        public uint sound_boss_bgm_pointer() { return 0x68148; } // ボスBGMの開始位置
        public uint sound_foot_steps_pointer() { return 0x0; } // クラス足音の開始位置
        public uint sound_foot_steps_switch2_address() { return 0x0; }
        public uint worldmap_point_pointer() { return 0x0; } // ワールドマップ拠点の開始位置
        public uint worldmap_bgm_pointer() { return 0x0; } // ワールドマップのBGMテーブルの開始位置
        public uint worldmap_icon_data_pointer() { return 0; } // ワールドマップのアイコンデータのテーブルの開始位置
        public uint worldmap_event_on_stageclear_pointer() { return 0x0; } // ワールドマップイベントクリア時
        public uint worldmap_event_on_stageselect_pointer() { return 0xB561c; } //ワールドマップイベント 拠点選択時
        public uint worldmap_county_border_pointer() { return 0; } // ワールドマップ国名の表示
        public uint worldmap_county_border_palette_pointer() { return 0x0; } // ワールドマップ国名の表示のパレット
        public uint item_shop_hensei_pointer() { return 0x0; } //編成準備店
        public uint item_cornered_pointer() { return 0x2A17C; } //すくみの開始位置
        public uint ed_1_pointer() { return 0x0; }  //ED FE8のみ 倒れたら撤退するかどうか
        public uint ed_2_pointer() { return 0xb7eb8; }  //ED 通り名
        public uint ed_3a_pointer() { return 0xcee630; }  //ED その後 エリウッド編　
        public uint ed_3b_pointer() { return 0xcee634; }  //ED その後 ヘクトル編
        public uint ed_3c_pointer() { return 0xcedd48; }  //ED その後 FE7 リン編(ポインタ指定できない)
        public uint generic_enemy_portrait_pointer() { return 0x75C0; } //一般兵の顔
        public uint generic_enemy_portrait_count() { return 0x8-2; } //一般兵の顔の個数

        public uint cc_item_hero_crest_itemid() { return 0x63; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_itemid() { return 0x64; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_itemid() { return 0x65; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_itemid() { return 0x66; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_itemid() { return 0x67; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_itemid() { return 0x96; }  //CCアイテム ダミー8A(FE7では覇者の証)
        public uint cc_master_seal_itemid() { return 0x87; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_itemid() { return 0x8B; }  //CCアイテム 覇者の証(FE7では闇の契約書 なぜか逆になっている)
        public uint cc_moon_bracelet_itemid() { return 0x87; }  //CCアイテム 月の腕輪
        public uint cc_sun_bracelet_itemid() { return 0x89; }  //CCアイテム 太陽の腕輪

        public uint cc_item_hero_crest_pointer() { return 0x27500; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_pointer() { return 0x27508; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_pointer() { return 0x27510; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_pointer() { return 0x27518; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_pointer() { return 0x27520; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_pointer() { return 0x27574; }  //CCアイテム ダミー8A 闇の契約書
        public uint cc_master_seal_pointer() { return 0x27528; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_pointer() { return 0x2754C; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_pointer() { return 0x27540; }  //CCアイテム 月の腕輪 天の刻印
        public uint cc_sun_bracelet_pointer() { return 0x27544; }  //CCアイテム 太陽の腕輪 天の刻印
        public uint unit_increase_height_pointer() { return 0x722C; }  //ステータス画面で背を伸ばす
        public uint unit_increase_height_switch2_address() { return 0x721C; }
        public uint op_class_demo_pointer() { return 0xCE68E4; } //OPクラスデモ
        public uint op_class_font_pointer() { return 0x0; }  //OPクラス日本語フォント
        public uint op_class_font_palette_pointer() { return 0x0; }  // OPクラス紹介フォントのパレット
        public uint status_font_pointer() { return 0x6118; }  //ステータス画面用のフォント
        public uint status_font_count() { return 0x100; }  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
        public uint ed_staffroll_image_pointer() { return 0x0; } // スタッフロール
        public uint ed_staffroll_palette_pointer() { return 0x0; } // スタッフロールのパレット
        public uint op_prologue_image_pointer() { return 0x0; } // OP字幕
        public uint op_prologue_palette_color_pointer() { return 0x0; } // OP字幕のパレット ???

        public uint arena_class_near_weapon_pointer() { return 0x2EB58; } //闘技場 近接武器クラス 
        public uint arena_class_far_weapon_pointer() { return 0x2EB64; } // 闘技場 遠距離武器クラス
        public uint arena_class_magic_weapon_pointer() { return 0x2EBB4; } // 闘技場 魔法武器クラス
        public uint arena_enemy_weapon_basic_pointer() { return 0x2F1C0; } // 闘技場 敵武器テーブル基本武器
        public uint arena_enemy_weapon_rankup_pointer() { return 0x2EDDC; } // 闘技場 敵武器テーブルランクアップ武器
        public uint link_arena_deny_unit_pointer() { return 0; } //通信闘技場 禁止ユニット 
        public uint worldmap_road_pointer() { return 0x0; } // ワールドマップの道

        public uint menu_definiton_pointer() { return 0x1B8D0; } //メニュー定義
        public uint menu_promotion_pointer() { return 0x0; } //CC決定する選択子
        public uint menu_promotion_branch_pointer() { return 0x0; } //FE8にある分岐CCメニュー
        public uint menu_definiton_split_pointer() { return 0x0; }  //FE8にある分岐メニュー
        public uint menu_definiton_worldmap_pointer() { return 0x0; } //FE8のワールドマップのメニュー
        public uint menu_definiton_worldmap_shop_pointer() { return 0x0; } //FE8のワールドマップ店のメニュー        
        public uint menu_unit_pointer() { return  0xB95AB4; } // ユニットメニュー
        public uint menu_game_pointer() { return  0xB95AFC; } // ゲームメニュー
        public uint menu_debug1_pointer() { return 0xB95850; }  // デバッグメニュー
        public uint MenuCommand_UsabilityAlways() { return 0x04A8F8; } //メニューを開くという値を返す関数のアドレス
        public uint MenuCommand_UsabilityNever() { return 0x04A900; } //メニューを開かないという値を返す関数のアドレス       
        public uint status_rmenu_unit_pointer() { return 0x8152C; } // ステータス RMENU1
        public uint status_rmenu_game_pointer() { return 0x81534; } // ステータス RMENU2
        public uint status_rmenu3_pointer() { return 0x8154C; } // ステータス RMENU3
        public uint status_rmenu4_pointer() { return 0x34228; } // 戦闘予測 RMENU4
        public uint status_rmenu5_pointer() { return 0x34240; } // 戦闘予測 RMENU5
        public uint status_rmenu6_pointer() { return 0x0; } // 状況画面 RMENU6
        public uint status_param1_pointer() { return 0x7FE4C; } // ステータス PARAM1
        public uint status_param2_pointer() { return 0x80130; } // ステータス PARAM2
        public uint status_param3w_pointer() { return 0x0; } // ステータス PARAM3 武器 海外版には"剣"みたいな武器の属性表示がありません
        public uint status_param3m_pointer() { return 0x0; } // ステータス PARAM3 魔法

        public uint systemmenu_common_image_pointer() { return 0x085BE8; } //システムメニューの画像
        public uint systemmenu_common_palette_pointer() { return 0x081194; } //システムパレット 無圧縮4パレット
        public uint systemmenu_goal_tsa_pointer() { return 0x085DC8; } //システムメニュー 目的表示TSA
        public uint systemmenu_terrain_tsa_pointer() { return 0x085640; } //システムメニュー 地形表示TSA

        public uint systemmenu_name_image_pointer() { return 0x085BE8; } //システムメニュー 名前表示画像(FE8は共通画像)
        public uint systemmenu_name_tsa_pointer() { return 0x08524C; } //システムメニュー 名前表示TSA
        public uint systemmenu_name_palette_pointer() { return 0x084E48; } //システムメニュー 名前表示パレット

        public uint systemmenu_battlepreview_image_pointer() { return 0x033C0C; } //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
        public uint systemmenu_battlepreview_tsa_pointer() { return 0x0336E8; } //戦闘プレビューTSA
        public uint systemmenu_battlepreview_palette_pointer() { return 0x033B2C; } //戦闘プレビューパレット

        public uint systemarea_move_gradation_palette_pointer() { return 0x01D25C; } //行動範囲
        public uint systemarea_attack_gradation_palette_pointer() { return 0x01D260; } //攻撃範囲
        public uint systemarea_staff_gradation_palette_pointer() { return 0x01D264; } //杖の範囲

        public uint systemmenu_badstatus_image_pointer() { return 0x84F5C; } //無圧縮のバッドステータス画像
        public uint systemmenu_badstatus_palette_pointer() { return 0x825B0; } //バッドステータスのパレット
        public uint systemmenu_badstatus_old_image_pointer() { return 0x85BE8; } //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
        public uint systemmenu_badstatus_old_palette_pointer() { return 0x9C11C; } //昔の圧縮のバッドステータス画像のパレット FE7 FE6

        public uint bigcg_pointer() { return 0xB6B64; } //CG
        public uint end_cg_address() { return 0x0; } // END CG FE8のみ
        public uint worldmap_big_image_pointer() { return 0x0B5BF4; } //ワールドマップ フィールドになるでかい奴  
        public uint worldmap_big_palette_pointer() { return 0x0B5DF4; } //ワールドマップ フィールドになるでかい奴 パレット  
        public uint worldmap_big_dpalette_pointer() { return 0x0; } //ワールドマップ フィールドになるでかい奴 闇パレット  
        public uint worldmap_big_palettemap_pointer() { return 0xB5BEC; } //ワールドマップ フィールドになるでかい奴 パレットマップではなく、 TSA 12分割
        public uint worldmap_event_image_pointer() { return   0xB5E20; } //ワールドマップ イベント用 
        public uint worldmap_event_palette_pointer() { return 0xB5E1C; } //ワールドマップ イベント用 パレット  
        public uint worldmap_event_tsa_pointer() { return 0xB5E2C; } //ワールドマップ イベント用 TSA
        public uint worldmap_mini_image_pointer() { return 0x0; } //ワールドマップ ミニマップ 
        public uint worldmap_mini_palette_pointer() { return 0x0; } //ワールドマップ ミニマップ パレット  
        public uint worldmap_icon_palette_pointer() { return 0x0; } //ワールドアイコンと道パレット
        public uint worldmap_icon1_pointer() { return 0x0; } //ワールドマップ アイコン1
        public uint worldmap_icon2_pointer() { return 0x0; } //ワールドマップ アイコン2
        public uint worldmap_road_tile_pointer() { return 0x0; } //ワールドマップ  道チップ
        public uint map_load_function_pointer() { return 0x0; } //マップチャプターに入ったときの処理(FE8のみ)
        public uint map_load_function_switch1_address() { return 0x0; }
        public uint system_icon_pointer() { return 0x155BC; }//システム アイコン集
        public uint system_icon_palette_pointer() { return 0x155C8; }//システム アイコンパレット集
        public uint system_icon_width_address() { return 0x155A0; } //システムアイコンの幅が書かれているアドレス
        public uint system_weapon_icon_pointer() { return 0x965F4; }//剣　斧　弓などの武器属性アイコン集
        public uint system_weapon_icon_palette_pointer() { return 0x090530; }//剣　斧　弓などの武器属性アイコン集のパレット
        public uint system_music_icon_pointer() { return 0x0AE1DC; }//音楽関係のアイコン集
        public uint system_music_icon_palette_pointer() { return 0x0AE1D0; }//音楽関係のアイコン集のパレット
        public uint weapon_rank_s_bonus_address() { return 0x28e98; }//武器ランクSボーナス設定
        public uint weapon_battle_flash_address() { return 0x533be; }//神器 戦闘時フラッシュ
        public uint weapon_effectiveness_2x3x_address() { return 0; }//神器 2倍 3倍特効
        public uint font_item_address() { return 0xb896b0; }//アイテム名とかに使われるフォント 関数08005B9C
        public uint font_serif_address()  { return 0xb8b5b0; } //セリフとかに使われるフォント
        public uint monster_probability_pointer() { return 0x0; } //魔物発生確率
        public uint monster_item_item_pointer() { return 0x0; } //魔物所持アイテム アイテム確率
        public uint monster_item_probability_pointer() { return 0x0; } //魔物所持アイテム 所持確率
        public uint monster_item_table_pointer() { return 0x0; } //魔物所持アイテム アイテムと所持を管理するテーブル
        public uint monster_wmap_base_point_pointer() { return 0x0; } //ワールドマップに魔物を登場させる処理達
        public uint monster_wmap_stage_1_pointer() { return 0x0; }
        public uint monster_wmap_stage_2_pointer() { return 0x0; }
        public uint monster_wmap_probability_1_pointer() { return 0x0; }
        public uint monster_wmap_probability_2_pointer() { return 0x0; }
        public uint monster_wmap_probability_after_1_pointer() { return 0x0; }
        public uint monster_wmap_probability_after_2_pointer() { return 0x0; }
        public uint battle_bg_pointer() { return 0x06AFB0; } //戦闘背景
        public uint battle_terrain_pointer() { return 0x4D100; } //戦闘地形
        public uint senseki_comment_pointer() { return 0x9A844; } //戦績コメント
        public uint unit_custom_battle_anime_pointer() { return 0x528e4; } //ユニット専用アニメ FE7にある

        public uint magic_effect_pointer() { return 0x558B4; } //魔法効果へのポインタ
        public uint magic_effect_original_data_count() { return 0x3e; } //もともとあった魔法数

        public uint system_move_allowicon_pointer() { return 0x02FF64; }//移動するときの矢印アイコン
        public uint system_move_allowicon_palette_pointer() { return 0x2FF6C; } //移動するときの矢印アイコン アイコンパレット集
        public uint system_tsa_16color_304x240_pointer() { return 0xB9161C; } //16色304x240 汎用TSAポインタ
        public uint eventunit_data_size() { return 16; } //ユニット配置のデータサイズ
        public uint eventcond_tern_size() { return 16; } //イベント条件 ターン条件のサイズ FE7->16 FE8->12
        public uint eventcond_talk_size() { return 16; } //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
        public uint oping_event_pointer() { return 0xca0544; }
        public uint ending1_event_pointer() { return 0x12A94; }
        public uint ending2_event_pointer() { return 0x12ADC; }
        public uint workmemory_player_units_address() { return 0x0202BD50; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_enemy_units_address() { return 0x0202CEC0; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_npc_units_address() { return 0x0202DCD0; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_chapterdata_address() { return workmemory_mapid_address() - 0xE; } //ワークメモリ章データ
        public uint workmemory_mapid_address() { return 0x0202BC06; }    //ワークメモリ マップID
        public uint workmemory_chapterdata_size() { return 0x4C; }    //ワークメモリ 章データのサイズ
        public uint workmemory_battle_actor_address() { return 0x0203A3F0; } //ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_battle_target_address() { return 0x0203A470; } //ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_battle_unit_size() { return 0x80; }  //ワークメモリ 戦闘時のユニット構造体のサイズ
        public uint workmemory_worldmap_data_address() { return 0x0; }//ワークメモリ ワールドマップ関係の起点
        public uint workmemory_worldmap_data_size() { return 0x0; } //ワークメモリ ワールドマップ関係のサイズ
        public uint workmemory_last_string_address() { return 0x0202B5B4; }  //ワークメモリ 最後に表示した文字列
        public uint workmemory_text_buffer_address() { return 0x0202A5B4; }  //ワークメモリ デコードされたテキスト
        public uint workmemory_next_text_buffer_address() { return 0x03000040; }  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
        public uint workmemory_local_flag_address() { return 0x03004AD8; }  //ワークメモリ グローバルフラグ
        public uint workmemory_global_flag_address() { return 0x03004AD0; }  //ワークメモリ ローカルフラグ
        public uint workmemory_trap_address() { return 0x0203A518; }  //ワークメモリ ローカルフラグ
        public uint workmemory_bwl_address() { return 0x0203E790; }  //BWLデータ
        public uint workmemory_clear_turn_address() { return 0x0203EC00; } //ワークメモリ クリアターン数
        public uint workmemory_clear_turn_count() { return 0x30; }  //クリアターン数 最大数
        public uint workmemory_memoryslot_address() { return 0; }  //ワークメモリ メモリスロットFE8
        public uint workmemory_eventcounter_address() { return 0x0; }  //イベントカウンター メモリスロットFE8
        public uint workmemory_procs_forest_address() { return 0x02026A30; }  //ワークメモリ Procs
        public uint workmemory_procs_pool_address() { return 0x02024E28; }  //ワークメモリ Procs
        public uint function_sleep_handle_address() { return 0x08004A84 + 1; }  //ワークメモリ Procs待機中
        public uint workmemory_user_stack_base_address() { return 0x03007DE0; } //ワークメモリ スタックの一番底
        public uint function_fe_main_return_address() { return 0x08000AF2 + 1; } //スタックの一番底にある戻り先
        public uint workmemory_control_unit_address() { return 0x03004690; } //ワークメモリ 操作ユニット
        public uint workmemory_bgm_address() { return 0x02024E1C; } //ワークメモリ BGM
        public uint function_event_engine_loop_address() { return 0x0800B390 + 1; } //イベントエンジン
        public uint workmemory_reference_procs_event_address_offset() { return 0x2C; } //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
        public uint workmemory_procs_game_main_address() { return 0x02024E28; } //ワークメモリ Procsの中でのGAMEMAIN
        public uint workmemory_palette_address() { return 0x02022860; } //RAMに記録されているダブルバッファのパレット領域
        public uint workmemory_sound_player_00_address() { return 0x03005B10; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_01_address() { return 0x03005D20; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_02_address() { return 0x03005D60; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_03_address() { return 0x03005E30; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_04_address() { return 0x03005DA0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_05_address() { return 0x03005A90; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_06_address() { return 0x03005AD0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_07_address() { return 0x03005CE0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_08_address() { return 0x03005DF0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint procs_game_main_address() { return 0x8B924BC; } //PROCSのGAME MAIN 
        public uint summon_unit_pointer() { return 0; } //召喚
        public uint summons_demon_king_pointer() { return 0; } //呼魔
        public uint summons_demon_king_count_address() { return 0; } //呼魔リストの数
        public uint mant_command_pointer() { return 0x63804; } //マント
        public uint mant_command_startadd() { return 0x58; } //マント開始数
        public uint mant_command_count_address() { return 0x63804; } //マント数 アドレス
        public uint unit_increase_height_yes() { return 0x080072C8; }  //ステータス画面で背を伸ばす 伸ばす
        public uint unit_increase_height_no() { return  0x080072CC; }  //ステータス画面で背を伸ばす 伸ばさない
        public uint battle_screen_TSA1_pointer() { return 0x04CF54; }  //戦闘画面
        public uint battle_screen_TSA2_pointer() { return 0x04CF58; }  //戦闘画面
        public uint battle_screen_TSA3_pointer() { return 0x04C6BC; }  //戦闘画面
        public uint battle_screen_TSA4_pointer() { return 0x04C6C4; }  //戦闘画面
        public uint battle_screen_TSA5_pointer() { return 0x04D428; }  //戦闘画面
        public uint battle_screen_palette_pointer() { return 0x04D430; }  //戦闘画面 パレット
        public uint battle_screen_image1_pointer() { return 0x04D220; }  //戦闘画面 画像1
        public uint battle_screen_image2_pointer() { return 0x04D280; }  //戦闘画面 画像2
        public uint battle_screen_image3_pointer() { return 0x04D2E0; }  //戦闘画面 画像3
        public uint battle_screen_image4_pointer() { return 0x04D340; }  //戦闘画面 画像4
        public uint battle_screen_image5_pointer() { return 0x04D41C; }  //戦闘画面 画像5
        public uint ai1_pointer() { return 0xB989F0; }  //AI1ポインタ
        public uint ai2_pointer() { return 0xB989E4; }  //AI2ポインタ
        public uint ai3_pointer() { return 0x392A4; }  //AI3ポインタ
        public uint ai_steal_item_pointer() { return 0x368B8; }  //AI盗むAI アイテム評価テーブル 0x08B97290
        public uint ai_preform_staff_pointer() { return 0x3AAD4; }  //AI杖 杖評価テーブル
        public uint ai_preform_staff_direct_asm_pointer() { return 0x03AB78; }  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
        public uint ai_preform_item_pointer() { return 0x3B9F4; } //AIアイテム アイテム評価テーブル
        public uint ai_preform_item_direct_asm_pointer() { return 0x3BA94; }  //AIアイテム アイテム評価テーブル
        public uint ai_map_setting_pointer() { return 0x34940; }  //AI 章ごとの設定テーブル 0x081D3A60
        public uint item_usability_array_pointer() { return 0x26D0C; } //アイテムを利用できるか判定する
        public uint item_usability_array_switch2_address() { return 0x26CFA; }
        public uint item_effect_array_pointer() { return 0x2D048; }    //アイテムを利用した場合の効果を定義する
        public uint item_effect_array_switch2_address() { return 0x2D02E; }
        public uint item_promotion1_array_pointer() { return 0x27428; }   //CCアイテムを使った場合の処理を定義する
        public uint item_promotion1_array_switch2_address() { return 0x27416; }
        public uint item_promotion2_array_pointer() { return 0x95680; }  //CCアイテムかどうかを定義する(FE7のみ)
        public uint item_promotion2_array_switch2_address() { return 0x95670; }
        public uint item_staff1_array_pointer() { return 0x27124; }    //アイテムのターゲット選択の方法を定義する(多分)
        public uint item_staff1_array_switch2_address() { return 0x27112; }
        public uint item_staff2_array_pointer() { return 0x67DF8; }    //杖の種類を定義する
        public uint item_staff2_array_switch2_address() { return 0x67DE6; }
        public uint item_statbooster1_array_pointer() { return 0x2CDDC; }    //ドーピングアイテムを利用した時のメッセージを定義する
        public uint item_statbooster1_array_switch2_address() { return 0x2CDCA; }
        public uint item_statbooster2_array_pointer() { return 0x2806C; }    //ドーピングアイテムとCCアイテムかどうかを定義する
        public uint item_statbooster2_array_switch2_address() { return 0x28058; }
        public uint item_errormessage_array_pointer() { return 0x26F68; }    //アイテム利用時のエラーメッセージ
        public uint item_errormessage_array_switch2_address() { return 0x26F56; }
        public uint event_function_pointer_table_pointer() { return 0xD72C; }    //イベント命令ポインタ
        public uint event_function_pointer_table2_pointer() { return 0x0; }   //イベント命令ポインタ2 ワールドマップ
        public uint item_effect_pointer_table_pointer() { return 0x558B4; }   //間接エフェクトポインタ
        public uint command_85_pointer_table_pointer() { return 0x67860; }    //85Commandポインタ
        public uint dic_main_pointer() { return 0x0; }     //辞書メインポインタ
        public uint dic_chaptor_pointer() { return 0x0; }  //辞書章ポインタ
        public uint dic_title_pointer() { return 0x0; }   //辞書タイトルポインタ
        public uint itemicon_mine_id() { return 0x79; }  // アイテムアイコンのフレイボムの位置
        public uint item_gold_id() { return 0x76; }  // お金を取得するイベントに利用されるゴールドのID
        public uint unitaction_function_pointer() { return 0x2F244; }  // ユニットアクションポインタ
        public uint lookup_table_battle_terrain_00_pointer() { return 0x52A2C; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_01_pointer() { return 0x529B0; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_02_pointer() { return 0x529B8; }//戦闘アニメの床
        public uint lookup_table_battle_terrain_03_pointer() { return 0x529C0; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_04_pointer() { return 0x529C8; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_05_pointer() { return 0x529D0; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_06_pointer() { return 0x529D8; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_07_pointer() { return 0x529E0; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_08_pointer() { return 0x529E8; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_09_pointer() { return 0x529F0; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_10_pointer() { return 0x529F8; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_11_pointer() { return 0x52A00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_12_pointer() { return 0x52A08; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_13_pointer() { return 0x52A10; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_14_pointer() { return 0x52A18; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_15_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_16_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_17_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_18_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_19_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_20_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_bg_00_pointer() { return 0x52B04; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_01_pointer() { return 0x52A8C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_02_pointer() { return 0x52A94; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_03_pointer() { return 0x52A9C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_04_pointer() { return 0x52AA4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_05_pointer() { return 0x52AAC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_06_pointer() { return 0x52AB4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_07_pointer() { return 0x52ABC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_08_pointer() { return 0x52AC4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_09_pointer() { return 0x52ACC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_10_pointer() { return 0x52AD4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_11_pointer() { return 0x52ADC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_12_pointer() { return 0x52AE4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_13_pointer() { return 0x52AEC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_14_pointer() { return 0x52AF4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_15_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_16_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_17_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_18_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_19_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_20_pointer() { return 0x00; } //戦闘アニメの背景
        public uint map_terrain_type_count() { return 65; } //地形の種類の数
        public uint menu_J12_always_address() { return 0x04A8F8; } //メニューの表示判定関数 常に表示する
        public uint menu_J12_hide_address() { return 0x04A900; }   //メニューの表示判定関数 表示しない
        public uint status_game_option_pointer() { return 0xADCAC; } //ゲームオプション
        public uint status_game_option_order_pointer() { return 0xCE586C; } //ゲームオプションの並び順
        public uint status_game_option_order2_pointer() { return 0xCE5874; } //ゲームオプションの並び順2 FE7のみ
        public uint status_game_option_order_count_address() { return 0xCE5868; } //ゲームオプションの個数
        public uint status_units_menu_pointer() { return 0x8AC88; } //部隊メニュー
        public uint tactician_affinity_pointer() { return 0x1C024; } //軍師属性(FE7のみ)
        public uint event_final_serif_pointer() { return 0x7DC48; } //終章セリフ(FE7のみ)
        public uint compress_image_borderline_address() { return 0xCBEE4; } //これ以降に圧縮画像が登場するというアドレス
        public uint patch_C01_hack(out uint enable_value) { enable_value = 0x0000D124; return 0x67B0; } //C01 patch
        public uint patch_C48_hack(out uint enable_value) { enable_value = 0x08067AB0; return 0x67920; } //C48 patch
        public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x0000000C; return 0x069D670; } //16_tracks_12_sounds patch
        public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x0; } //章の名前をテキストにするパッチ
        public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x7598; } //一般兵の顔 拡張
        public uint patch_stairs_hack(out uint enable_value) { enable_value = 0x47184b00; return 0x21580; } //階段拡張
        public uint patch_escape_arrive_hack(out uint enable_value) { enable_value = 0x0; return 0x0; } //離脱/到達パッチ
        public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x02f218; } //ユニットアクションの拡張
        public uint patch_write_build_version(out uint enable_value) { enable_value = 0x0; return 0x0; } //ビルドバージョンを書き込む
        public uint builddate_address() { return 0x0; }

        public byte[] defualt_event_script_term_code() { return new byte[] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード
        public byte[] defualt_event_script_toplevel_code() { return new byte[] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード(各章のトップレベルのイベント)
        public byte[] defualt_event_script_mapterm_code() { return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; } //ワールドマップイベント命令を終了させるディフォルトコード
        public uint main_menu_width_address() { return 0xB95AF6; } //メインメニューの幅
        public uint font_default_begin() { return 0xB87B68; }
        public uint font_default_end() { return 0xB8B9AC; }
        public string get_shop_name(uint shop_object)//店の名前
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
        public uint map_default_count() { return 0x43; }    // ディフォルトのマップ数
        public uint wait_menu_command_id() { return 0x67; } //WaitメニューのID
        public uint extends_address() { return 0x09000000; }  //拡張領域
        public uint orignal_crc32() { return 0x2a524221; } //無改造ROMのCRC32
        public bool is_multibyte() { return false; }    // マルチバイトを利用するか？
        public int version() { return 7; }    // バージョン
    };
}
    