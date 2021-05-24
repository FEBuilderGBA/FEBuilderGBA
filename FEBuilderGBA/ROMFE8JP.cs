using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE8JP : IROMFEINFO
    {
        public String game_id(){ return "BE8J01"; }    // ゲームバージョンコード
        public String VersionToFilename() { return "FE8J"; }
        public String TitleToFilename() { return "FE8"; }
        public uint mask_point_base_pointer() { return 0x0006DC; } // Huffman tree end (indirected twice)
        public uint mask_pointer() { return 0x0006E0; }  // Huffman tree start (indirected once)
        public uint text_pointer() { return 0x00A000; } // textの開始位置
        public uint text_recover_address() { return 0x14D08C; } // textの開始位置(上記ポインタを壊している改造があるののでその対策)
        public uint text_data_start_address() { return 0xED7F4; } // textデータの規定値の開始位置
        public uint text_data_end_address()   { return 0x14929B; } // textデータの規定値の開始位置
        public uint unit_pointer() { return 0x010650; } // ユニットのの開始位置
        public uint unit_maxcount() { return 255; } // ユニットの最大数
        public uint unit_datasize() { return 52; } // ユニットのデータサイズ
        public uint max_level_address() { return 0x02B9C0; } // 最大レベルの値を格納しているアドレス  
        public uint max_luck_address() { return 0x2bf5e; } // 最大レベルの幸運の値を格納しているアドレス
        public uint class_pointer() { return 0x017860; } // クラスの開始位置
        public uint class_datasize() { return 84; }  // ユニットのデータサイズ
        public uint bg_pointer() { return 0x00EAA4; } //BGベースアドレス
        public uint face_pointer() { return 0x00542C; } //顔ベースアドレス
        public uint face_datasize() { return 28; }
        public uint icon_pointer() { return 0x0036D4; } // アイコンの開始位置
        public uint icon_orignal_address() { return 0x5BA470; } // アイコンの初期値
        public uint icon_orignal_max() { return 0xDF; } // アイコンの最大数

        public uint icon_palette_pointer() { return 0x00351C; } // アイコンの開始位置
        public uint unit_wait_icon_pointer() { return 0x0266D4; } // ユニット待機アイコンの開始位置
        public uint unit_wait_barista_anime_address() { return 0x02727c; }  // ユニット待機アイコンのバリスタのアニメ指定アドレス
        public uint unit_wait_barista_id() { return 0x5b; }  // ユニット待機アイコンのバリスタの位置
        public uint unit_icon_palette_address() { return 0x5c7340; } // ユニット(自軍)のパレットのアドレス
        public uint unit_icon_enemey_palette_address() { return 0x5c7360; } // ユニット(敵軍)のパレットのアドレス
        public uint unit_icon_npc_palette_address() { return 0x5c7380; } // ユニット(友軍)のパレットのアドレス
        public uint unit_icon_gray_palette_address() { return 0x5c73A0; } // ユニット(グレー))のパレットの開始位置
        public uint unit_icon_four_palette_address() { return 0x5c73C0; } // ユニット(4軍))のパレットの開始位置
        public uint unit_icon_lightrune_palette_address() { return 0x5C73E0; } // ユニット(光の結界)のパレットの開始位置
        public uint unit_icon_sepia_palette_address() { return 0x5C7400; } // ユニット(セピア)のパレットの開始位置

        public uint unit_move_icon_pointer() { return 0x07B998; } // ユニット移動アイコンの開始位置
        public uint lightrune_uniticon_id() { return 0x66; } // ユニット(光の結界)のユニットアイコンのID
        public uint map_setting_pointer() { return 0x0BAB1C; }  // マップ設定の開始位置
        public uint map_setting_datasize() { return 148; } //マップ設定のデータサイズ
        public uint map_setting_event_plist_pos() { return 116; } //event plistの場所 
        public uint map_setting_worldmap_plist_pos() { return 117; } //woldmap event plistの場所 
        public uint map_setting_clear_conditon_text_pos() { return 0x8A; } //マップの右上に表示されているクリア条件の定義場所 
        public uint map_setting_name_text_pos() { return 0x70; } //マップ名のテキスト定義場所 
        public uint map_config_pointer() { return 0x0195D8; }      //マップ設定の開始位置(config)
        public uint map_obj_pointer() { return 0x019640; }         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_pal_pointer() { return 0x019674; }         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_tileanime1_pointer() { return 0x030084; }  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_tileanime2_pointer() { return 0x030BC8; }  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_map_pointer_pointer() { return 0x034588; } //マップ設定の開始位置(map)
        public uint map_mapchange_pointer() { return 0x0345B4; }   //マップ設定の開始位置(mapchange)
        public uint map_event_pointer() { return 0x0345E4; }       //マップ設定の開始位置(event)
        public uint map_worldmapevent_pointer() { return 0x0; } //マップ設定の開始位置(worldmap (FE6のみ))
        public uint image_battle_animelist_pointer() { return 0x05A97C; }   // 戦闘アニメリストの開始位置
        public uint support_unit_pointer() { return 0x8582E8; }   // 支援相手の開始位置
        public uint support_talk_pointer() { return 0x086A50; }   // 支援相手の開始位置
        public uint unit_palette_color_pointer() { return 0x582F4; }  // ユニットのパレット(カラー)の開始位置
        public uint unit_palette_class_pointer() { return 0xD1E90; }  // ユニットのパレット(クラス)の開始位置
        public uint support_attribute_pointer() { return 0x284C8; }  //支援効果の開始位置
        public uint terrain_recovery_pointer(){ return 0x19F34; } //地形回復 全クラス共通
        public uint terrain_bad_status_recovery_pointer() { return 0x019F44; } //地形回復 全クラス共通
        public uint ccbranch_pointer() { return 0xD14EC; } // CC分岐の開始位置
        public uint ccbranch2_pointer() { return 0xD14E4; } // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
        public uint class_alphaname_pointer() { return 0xD18CC; } // クラスのアルファベット表記の開始位置
        public uint map_terrain_name_pointer() { return 0x19f24;  } // マップの地名表記の開始位置
        public uint image_chapter_title_pointer() { return 0x8ba0c; } // 章タイトルの開始位置
        public uint image_chapter_title_palette() { return 0xa99fa8; } // 章タイトルのパレット 多分違う
        public uint image_unit_palette_pointer() { return 0x5b6cc;  } // ユニットパレットの開始位置
        public uint item_pointer() { return 0x17568; } // アイテムの開始位置
        public uint item_datasize() { return 36; } // アイテムのデータサイズ
        public uint item_effect_pointer() { return 0x7A664; } // アイテムエフェクトの開始位置
        public uint sound_table_pointer() { return 0xD5024; } // ソングテーブルの開始位置
        public uint sound_room_pointer() { return 0xB5044; } // サウンドルームの開始位置
        public uint sound_room_datasize() { return 16; } // サウンドルームのデータサイズ
        public uint sound_room_cg_pointer() { return 0x0; } // サウンドルームの背景リスト(FE7のみ)
        public uint event_ballte_talk_pointer() { return 0x86978; } // 交戦時セリフの開始位置
        public uint event_ballte_talk2_pointer() { return 0; } // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
        public uint event_haiku_pointer() { return 0x869F8; } // 死亡時セリフの開始位置
        public uint event_haiku_tutorial_1_pointer() { return 0x0; } // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_haiku_tutorial_2_pointer() { return 0x0; } // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_force_sortie_pointer() { return 0x86B08; } // 強制出撃の開始位置
        public uint event_tutorial_pointer() { return 0x0; } //イベントチュートリアルポインタ FE7のみ
        public uint map_exit_point_pointer() { return 0x3E83C; } // 離脱ポイント開始サイズ
        public uint map_exit_point_npc_blockadd() { return 65; } // arr[+65] からNPCらしい.
        public uint map_exit_point_blank() { return 0xDD1BC; } // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
        public uint sound_boss_bgm_pointer() { return 0x74DE4; } // ボスBGMの開始位置
        public uint sound_foot_steps_pointer() { return 0x7B1E8; } // クラス足音の開始位置
        public uint sound_foot_steps_switch2_address() { return 0x7b1d6; }
        public uint worldmap_point_pointer() { return 0xC8DC0; } // ワールドマップ拠点の開始位置
        public uint worldmap_bgm_pointer() { return 0xBEE28; } // ワールドマップのBGMテーブルの開始位置
        public uint worldmap_icon_data_pointer() { return 0xC04A4; } // ワールドマップのアイコンデータのテーブルの開始位置
        public uint worldmap_event_on_stageclear_pointer() { return 0xBF264; } // ワールドマップイベント 拠点クリア時
        public uint worldmap_event_on_stageselect_pointer() { return 0xBF2B0; } // ワールドマップイベント 拠点選択時
        public uint worldmap_county_border_pointer() { return 0xC792C; } // ワールドマップ国名の表示
        public uint worldmap_county_border_palette_pointer() { return 0xC755C; } // ワールドマップ国名の表示のパレット
        public uint item_shop_hensei_pointer() { return 0x9C144; } //編成準備店
        public uint item_cornered_pointer() { return 0x2c704; } //すくみの開始位置
        public uint ed_1_pointer() { return 0xBB224; }  //ED 倒れたら撤退するかどうか
        public uint ed_2_pointer() { return 0xBB200;}  //ED 通り名
        public uint ed_3a_pointer() { return 0xAC09E0; }  //ED その後 エイルーク編　
        public uint ed_3b_pointer() { return 0xAC09E4; }  //ED その後 エフラム編
        public uint ed_3c_pointer() { return 0x0; }  //ED その後 FE7 リン編
        public uint generic_enemy_portrait_pointer() { return 0x5E98; } //一般兵の顔
        public uint generic_enemy_portrait_count() { return 8; } //一般兵の顔の個数

        public uint cc_item_hero_crest_itemid() { return 0x64; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_itemid() { return 0x65; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_itemid() { return 0x66; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_itemid() { return 0x67; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_itemid() { return 0x68; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_itemid() { return 0x8A; }  //CCアイテム ダミー8A
        public uint cc_master_seal_itemid() { return 0x88; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_itemid() { return 0x97; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_itemid() { return 0x98; }  //CCアイテム 月の腕輪
        public uint cc_sun_bracelet_itemid() { return 0x99; }  //CCアイテム 太陽の腕輪

        public uint cc_item_hero_crest_pointer() { return 0x29340; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_pointer() { return 0x29348; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_pointer() { return 0x29350; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_pointer() { return 0x29358; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_pointer() { return 0x29360; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_pointer() { return 0x29380; }  //CCアイテム ダミー8A
        public uint cc_master_seal_pointer() { return 0x29368; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_pointer() { return 0x293B0; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_pointer() { return 0x29370; }  //CCアイテム 月の腕輪
        public uint cc_sun_bracelet_pointer() { return 0x29378; }  //CCアイテム 太陽の腕輪
        public uint unit_increase_height_pointer() { return 0x5b3c; }  //ステータス画面で背を伸ばす
        public uint unit_increase_height_switch2_address() { return 0x5b2a; }
        public uint op_class_demo_pointer() { return 0xAB0B18;  } //OPクラスデモ
        public uint op_class_font_pointer() { return  0xB7988; }  //OPクラス日本語フォント
        public uint op_class_font_palette_pointer() { return 0xb7994; }  // OPクラス紹介フォントのパレット
        public uint status_font_pointer() { return 0x49D0; }  //ステータス画面用のフォント
        public uint status_font_count() { return 0xA; }  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
        public uint ed_staffroll_image_pointer() { return 0x1F6AE8; } // スタッフロール
        public uint ed_staffroll_palette_pointer() { return 0xC9564; } // スタッフロールのパレット
        public uint op_prologue_image_pointer() { return 0xC9A74; } // OP字幕
        public uint op_prologue_palette_color_pointer() { return 0xC9564; } // OP字幕のパレット ???

        public uint arena_class_near_weapon_pointer() { return 0x31898; } //闘技場 近接武器クラス 
        public uint arena_class_far_weapon_pointer() { return 0x318a4; } // 闘技場 遠距離武器クラス
        public uint arena_class_magic_weapon_pointer() { return 0x318F4; } // 闘技場 魔法武器クラス
        public uint arena_enemy_weapon_basic_pointer() { return 0x31B04; } // 闘技場 敵武器テーブル基本武器
        public uint arena_enemy_weapon_rankup_pointer() { return 0x31B24; } // 闘技場 敵武器テーブルランクアップ武器
        public uint link_arena_deny_unit_pointer() { return 0x9A164; } //通信闘技場 禁止ユニット 
        public uint worldmap_road_pointer() { return 0xC598; } // ワールドマップの道

        public uint menu_definiton_pointer() { return 0x1BCBC; } //メニュー定義
        public uint menu_promotion_pointer() { return 0xD2900; } //CC決定する選択子
        public uint menu_promotion_branch_pointer() { return 0x0D2AC8; } //FE8にある分岐CCメニュー
        public uint menu_definiton_split_pointer() { return 0x887DC; }  //FE8にある分岐メニュー
        public uint menu_definiton_worldmap_pointer() { return 0xC12CC; } //FE8のワールドマップのメニュー
        public uint menu_definiton_worldmap_shop_pointer() { return 0xC142C; } //FE8のワールドマップ店のメニュー        
        public uint menu_unit_pointer() { return  0x5c56d8; } // ユニットメニュー
        public uint menu_game_pointer() { return  0x5C56FC; } // ゲームメニュー
        public uint menu_debug1_pointer() { return 0x5C5498; }  // デバッグメニュー
        public uint MenuCommand_UsabilityAlways() { return 0x0501BC; } //メニューを開くという値を返す関数のアドレス
        public uint MenuCommand_UsabilityNever() { return 0x0501C4; } //メニューを開かないという値を返す関数のアドレス       
        public uint status_rmenu_unit_pointer() { return 0x8ac64; } // ステータス RMENU1
        public uint status_rmenu_game_pointer() { return 0x8ac6c; } // ステータス RMENU2
        public uint status_rmenu3_pointer() { return 0x8ac84; } // ステータス RMENU3
        public uint status_rmenu4_pointer() { return 0x37510; } // 戦闘予測 RMENU4
        public uint status_rmenu5_pointer() { return 0x37528; } // 戦闘予測 RMENU5
        public uint status_rmenu6_pointer() { return 0xA73DF8; } // 状況画面 RMENU6
        public uint status_param1_pointer() { return 0x089454; } // ステータス PARAM1
        public uint status_param2_pointer() { return 0x089758; } // ステータス PARAM2
        public uint status_param3w_pointer() { return 0x089B54; } // ステータス PARAM3 武器
        public uint status_param3m_pointer() { return 0x089B10; } // ステータス PARAM3 魔法

        public uint systemmenu_common_image_pointer(){ return 0x5E0650; } //システムメニューの画像
        public uint systemmenu_common_palette_pointer(){ return 0x036E2C; } //システムパレット 無圧縮4パレット
        public uint systemmenu_goal_tsa_pointer(){ return 0x08F57C; } //システムメニュー 目的表示TSA
        public uint systemmenu_terrain_tsa_pointer() { return 0x08EE2C; } //システムメニュー 地形表示TSA

        public uint systemmenu_name_image_pointer(){ return 0x5E0650; } //システムメニュー 名前表示画像(FE8は共通画像)
        public uint systemmenu_name_tsa_pointer(){ return 0x08EA08; } //システムメニュー 名前表示TSA
        public uint systemmenu_name_palette_pointer(){ return 0x036E2C; } //システムメニュー 名前表示パレット

        public uint systemmenu_battlepreview_image_pointer(){ return 0x5E0650; } //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
        public uint systemmenu_battlepreview_tsa_pointer() { return 0x0369A0; } //戦闘プレビューTSA
        public uint systemmenu_battlepreview_palette_pointer(){ return 0x036E2C; } //戦闘プレビューパレット

        public uint systemarea_move_gradation_palette_pointer() { return 0x1d6b8; } //行動範囲
        public uint systemarea_attack_gradation_palette_pointer() { return 0x1d6bc; } //攻撃範囲
        public uint systemarea_staff_gradation_palette_pointer() { return 0x1d6c0; } //杖の範囲

        public uint systemmenu_badstatus_image_pointer() { return 0x8E750; } //無圧縮のバッドステータス画像
        public uint systemmenu_badstatus_palette_pointer() { return 0x8BBD0; } //バッドステータスのパレット
        public uint systemmenu_badstatus_old_image_pointer() { return 0; } //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
        public uint systemmenu_badstatus_old_palette_pointer() { return 0x0; } //昔の圧縮のバッドステータス画像のパレット FE7 FE6

        public uint bigcg_pointer() { return 0xbb174; } // CG
        public uint end_cg_address() { return 0x1F68CC; } // END CG FE8のみ
        public uint worldmap_big_image_pointer() { return 0xBF698; } //ワールドマップ フィールドになるでかい奴  
        public uint worldmap_big_palette_pointer() { return 0xBF6A4; } //ワールドマップ フィールドになるでかい奴 パレット  
        public uint worldmap_big_dpalette_pointer() { return 0xC4594; } //ワールドマップ フィールドになるでかい奴 闇パレット  
        public uint worldmap_big_palettemap_pointer() { return 0xBF69C; } //ワールドマップ フィールドになるでかい奴 パレットマップ
        public uint worldmap_event_image_pointer() { return 0xc6dfc; } //ワールドマップ イベント用 
        public uint worldmap_event_palette_pointer() { return 0xc6e00; } //ワールドマップ イベント用 パレット  
        public uint worldmap_event_tsa_pointer() { return 0xc6e04; } //ワールドマップ イベント用 TSA
        public uint worldmap_mini_image_pointer() { return 0xc8c24; } //ワールドマップ ミニマップ 
        public uint worldmap_mini_palette_pointer() { return 0xc8c2c; } //ワールドマップ ミニマップ パレット  
        public uint worldmap_icon_palette_pointer() { return 0xBDD10; } //ワールドアイコンと道パレット
        public uint worldmap_icon1_pointer() { return 0xBDD14; } //ワールドマップ アイコン1
        public uint worldmap_icon2_pointer() { return 0xBDD1C; } //ワールドマップ アイコン2
        public uint worldmap_road_tile_pointer() { return 0xBDE60; } //ワールドマップ  道チップ
        public uint map_load_function_pointer() { return 0xc1e90; } //マップチャプターに入ったときの処理(FE8のみ)
        public uint map_load_function_switch1_address() { return 0xc1e7c; }
        public uint system_icon_pointer() { return 0x156C8; }//システム アイコン集
        public uint system_icon_palette_pointer() { return 0x156d4; }//システム アイコンパレット集
        public uint system_icon_width_address() { return 0x156AC; } //システムアイコンの幅が書かれているアドレス
        public uint system_weapon_icon_pointer() { return 0x9fefc; }//剣　斧　弓などの武器属性アイコン集
        public uint system_weapon_icon_palette_pointer() { return 0x93450; }//剣　斧　弓などの武器属性アイコン集のパレット
        public uint system_music_icon_pointer() { return 0x0B689C; }//音楽関係のアイコン集
        public uint system_music_icon_palette_pointer() { return 0x0B6890; }//音楽関係のアイコン集のパレット
        public uint weapon_rank_s_bonus_address() { return 0x2ACE4; }//武器ランクSボーナス設定
        public uint weapon_battle_flash_address() { return 0x59902; }//神器 戦闘時フラッシュ
        public uint weapon_effectiveness_2x3x_address() { return 0x2aa88; }//神器 2倍 3倍特効
        public uint font_item_address() { return 0x57994C; }//アイテム名とかに使われるフォント 関数:080040b8 計算式とか0x08003f50
        public uint font_serif_address() { return  0x593F74; } //セリフとかに使われるフォント
        public uint monster_probability_pointer() { return 0x7A770; } //魔物発生確率
        public uint monster_item_item_pointer() { return 0x7A814; } //魔物所持アイテム アイテム確率
        public uint monster_item_probability_pointer() { return 0x7A810; } //魔物所持アイテム 所持確率
        public uint monster_item_table_pointer() { return 0x7A784; } //魔物所持アイテム アイテムと所持を管理するテーブル
        public uint monster_wmap_base_point_pointer() { return 0xC6694; } //ワールドマップに魔物を登場させる処理達
        public uint monster_wmap_stage_1_pointer() { return 0xC657C; }
        public uint monster_wmap_stage_2_pointer() { return 0xC65B4; }
        public uint monster_wmap_probability_1_pointer() { return 0xC6580; }
        public uint monster_wmap_probability_2_pointer() { return 0xC65B8; }
        public uint monster_wmap_probability_after_1_pointer() { return 0xC65D0; }
        public uint monster_wmap_probability_after_2_pointer() { return 0xC6690; }
        public uint battle_bg_pointer() { return 0x77e9c; } //戦闘背景
        public uint battle_terrain_pointer() { return 0x52b40; } //戦闘地形
        public uint senseki_comment_pointer() { return 0x0; } //戦績コメント FE8にはない
        public uint unit_custom_battle_anime_pointer() { return 0x0; } //ユニット専用アニメ FE7にある
        public uint magic_effect_pointer() { return 0x5C19C; } //魔法効果へのポインタ
        public uint magic_effect_original_data_count() { return 0x48; } //もともとあった魔法数
 
        public uint system_move_allowicon_pointer() { return 0x32db0; }//移動するときの矢印アイコン
        public uint system_move_allowicon_palette_pointer() { return 0x32db8;} //移動するときの矢印アイコン アイコンパレット集
        public uint system_tsa_16color_304x240_pointer() { return 0xB51B8; } //16色304x240 汎用TSAポインタ
        public uint eventunit_data_size() { return 20; } //ユニット配置のデータサイズ
        public uint eventcond_tern_size() { return 12; } //イベント条件 ターン条件のサイズ FE7->16 FE8->12
        public uint eventcond_talk_size() { return 16; } //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
        public uint oping_event_pointer() { return 0x907f78; }
        public uint ending1_event_pointer() { return 0x9e20; }
        public uint ending2_event_pointer() { return 0x9e38; }
        public uint RAMSlotTable_address() { return 0x5C2A50; }
        public uint supply_pointer_address() { return 0x31470; }  //輸送体RAMへのアドレス
        public uint workmemory_player_units_address() { return 0x0202BE48; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_enemy_units_address() { return 0x0202CFB8; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_npc_units_address() { return 0x0202DDC8; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_chapterdata_address() { return workmemory_mapid_address() - 0xE; } //ワークメモリ章データ
        public uint workmemory_mapid_address() { return 0x0202BCFA; }    //ワークメモリ マップID
        public uint workmemory_chapterdata_size() { return 0x4C; }    //ワークメモリ 章データのサイズ
        public uint workmemory_battle_actor_address() { return 0x0203a4e8; } //ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_battle_target_address() { return 0x0203a568; } //ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_worldmap_data_address() { return 0x03005270; }//ワークメモリ ワールドマップ関係の起点
        public uint workmemory_worldmap_data_size() { return 0xC4; } //ワークメモリ ワールドマップ関係のサイズ
        public uint workmemory_arena_data_address() { return 0x0203A8EC; }//ワークメモリ 闘技場関係の起点
        public uint workmemory_ai_data_address() { return 0x0203AA00; } //ワークメモリ AI関係の起点
        public uint workmemory_action_data_address() { return 0x0203A954; } //ワークメモリ ActionData
        public uint workmemory_dungeon_data_address() { return 0x03001798; } //ワークメモリ ダンジョン FE8のみ
        public uint workmemory_battlesome_data_address() { return 0x0203E0EC; } //ワークメモリ バルトに関係する諸データ
        public uint workmemory_last_string_address() { return 0x0202B6A8; }  //ワークメモリ 最後に表示した文字列
        public uint workmemory_text_buffer_address() { return 0x0202A6A8; }  //ワークメモリ デコードされたテキスト
        public uint workmemory_next_text_buffer_address() { return 0x03000040; }  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
        public uint workmemory_local_flag_address() { return 0x03005260; }  //ワークメモリ グローバルフラグ
        public uint workmemory_global_flag_address() { return 0x03005240; }  //ワークメモリ ローカルフラグ
        public uint workmemory_trap_address() { return 0x0203A610; }  //ワークメモリ ローカルフラグ
        public uint workmemory_bwl_address() { return 0x0203E880; }  //BWLデータ
        public uint workmemory_clear_turn_address() { return 0x0203ECF0; } //ワークメモリ クリアターン数
        public uint workmemory_clear_turn_count() { return 0x24; }  //クリアターン数 最大数
        public uint workmemory_memoryslot_address() { return 0x030004B0; }  //ワークメモリ メモリスロットFE8
        public uint workmemory_eventcounter_address() { return 0x03000560; }  //イベントカウンター メモリスロットFE8
        public uint workmemory_procs_forest_address() { return 0x02026A70; }  //ワークメモリ Procs
        public uint workmemory_procs_pool_address() { return 0x02024E68; }  //ワークメモリ Procs
        public uint function_sleep_handle_address() { return 0x080031DC + 1; }  //ワークメモリ Procs待機中
        public uint workmemory_user_stack_base_address() { return 0x03007DF0; } //ワークメモリ スタックの一番底
        public uint function_fe_main_return_address() { return 0x08000AB8 + 1; } //スタックの一番底にある戻り先
        public uint workmemory_control_unit_address() { return 0x03004df0; } //ワークメモリ 操作ユニット
        public uint workmemory_bgm_address() { return 0x02024E5C; } //ワークメモリ BGM
        public uint function_event_engine_loop_address() { return 0x0800D110 + 1; } //イベントエンジン
        public uint workmemory_reference_procs_event_address_offset() { return 0x34; } //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
        public uint workmemory_procs_game_main_address() { return 0x02024E68; } //ワークメモリ Procsの中でのGAMEMAIN
        public uint workmemory_palette_address() { return 0x020228A8; } //RAMに記録されているダブルバッファのパレット領域
        public uint workmemory_sound_player_00_address() { return 0x03006430; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_01_address() { return 0x03006640; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_02_address() { return 0x03006680; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_03_address() { return 0x030066C0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_04_address() { return 0x03006710; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_05_address() { return 0x03006750; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_06_address() { return 0x03006600; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_07_address() { return 0x030063F0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_08_address() { return 0x030063B0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint procs_game_main_address() { return 0x85B946C; } //PROCSのGAME MAIN 
        public uint summon_unit_pointer() { return 0x0243E0; } //召喚
        public uint summons_demon_king_pointer() { return 0x07D6AC; } //呼魔
        public uint summons_demon_king_count_address() { return 0x7D63C; } //呼魔リストの数
        public uint mant_command_pointer() { return 0x6F3F4; } //マント
        public uint mant_command_startadd() { return 0x6B; } //マント開始数
        public uint mant_command_count_address() { return 0x6F3DA; } //マント数 アドレス
        public uint unit_increase_height_yes() { return 0x08005b9c; }  //ステータス画面で背を伸ばす 伸ばす
        public uint unit_increase_height_no() { return  0x08005ba0; }  //ステータス画面で背を伸ばす 伸ばさない
        public uint battle_screen_TSA1_pointer() { return 0x529B4; }  //戦闘画面
        public uint battle_screen_TSA2_pointer() { return 0x529B8; }  //戦闘画面
        public uint battle_screen_TSA3_pointer() { return 0x520AC; }  //戦闘画面
        public uint battle_screen_TSA4_pointer() { return 0x520B4; }  //戦闘画面
        public uint battle_screen_TSA5_pointer() { return 0x52E68; }  //戦闘画面
        public uint battle_screen_palette_pointer() { return 0x52E70; }  //戦闘画面 パレット
        public uint battle_screen_image1_pointer() { return 0x52C60; }  //戦闘画面 画像1
        public uint battle_screen_image2_pointer() { return 0x52CC0; }  //戦闘画面 画像2 左側名前
        public uint battle_screen_image3_pointer() { return 0x52D20; }  //戦闘画面 画像3 左側武器
        public uint battle_screen_image4_pointer() { return 0x52D80; }  //戦闘画面 画像4 右側名前
        public uint battle_screen_image5_pointer() { return 0x52E5C; }  //戦闘画面 画像5 右側武器
        public uint ai1_pointer() { return 0x5D30EC; }  //AI1ポインタ
        public uint ai2_pointer() { return 0x5D30E0; }  //AI2ポインタ
        public uint ai3_pointer() { return 0x3E16C; }  //AI3ポインタ
        public uint ai_steal_item_pointer() { return 0x3B7C8; }  //AI盗むAI アイテム評価テーブル 0x085D22AC
        public uint ai_preform_staff_pointer() { return 0x3F9BC; }  //AI杖 杖評価テーブル
        public uint ai_preform_staff_direct_asm_pointer() { return 0x03FA60; }  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
        public uint ai_preform_item_pointer() { return 0x407A0; } //AIアイテム アイテム評価テーブル
        public uint ai_preform_item_direct_asm_pointer() { return 0x40848; }  //AIアイテム アイテム評価テーブル
        public uint ai_map_setting_pointer() { return 0x39784; }  //AI 章ごとの設定テーブル 0x080DD214
        public uint item_usability_array_pointer() { return 0x28858; } //アイテムを利用できるか判定する
        public uint item_usability_array_switch2_address() { return 0x28846; }
        public uint item_effect_array_pointer() { return 0x2fbdc; }    //アイテムを利用した場合の効果を定義する
        public uint item_effect_array_switch2_address() { return 0x2fbc2; }
        public uint item_promotion1_array_pointer() { return 0x291c0; }   //CCアイテムを使った場合の処理を定義する
        public uint item_promotion1_array_switch2_address() { return 0x291aa; }
        public uint item_promotion2_array_pointer() { return 0x0; }  //CCアイテムかどうかを定義する(FE7のみ)
        public uint item_promotion2_array_switch2_address() { return 0x0; }
        public uint item_staff1_array_pointer() { return 0x28e34; }    //アイテムのターゲット選択の方法を定義する(多分)
        public uint item_staff1_array_switch2_address() { return 0x28e22; }
        public uint item_staff2_array_pointer() { return 0x74a6c; }    //杖の種類を定義する
        public uint item_staff2_array_switch2_address() { return 0x74a58; }
        public uint item_statbooster1_array_pointer() { return 0x2f7dc; }    //ドーピングアイテムを利用した時のメッセージを定義する
        public uint item_statbooster1_array_switch2_address() { return 0x2f7ca; }
        public uint item_statbooster2_array_pointer() { return 0x29ebc; }    //ドーピングアイテムとCCアイテムかどうかを定義する
        public uint item_statbooster2_array_switch2_address() { return 0x29ea8; }
        public uint item_errormessage_array_pointer() { return 0x28BD4; }    //アイテム利用時のエラーメッセージ
        public uint item_errormessage_array_switch2_address() { return 0x28BC2; }
        public uint event_function_pointer_table_pointer() { return 0x0D1A4; }    //イベント命令ポインタ
        public uint event_function_pointer_table2_pointer() { return 0x0D1CC; }   //イベント命令ポインタ2 ワールドマップ
        public uint item_effect_pointer_table_pointer() { return 0x5C19C; }   //間接エフェクトポインタ
        public uint command_85_pointer_table_pointer() { return 0x074108; }    //85Commandポインタ
        public uint dic_main_pointer() { return 0xD4180; }     //辞書メインポインタ
        public uint dic_chaptor_pointer() { return 0xD2EF8; }  //辞書章ポインタ
        public uint dic_title_pointer() { return 0xD2F38; }   //辞書タイトルポインタ
        public uint itemicon_mine_id() { return 0x8c; }  // アイテムアイコンのフレイボムの位置
        public uint item_gold_id() { return 0x77; }  // お金を取得するイベントに利用されるゴールドのID
        public uint unitaction_function_pointer() { return 0x31FA8; }  // ユニットアクションポインタ
        public uint lookup_table_battle_terrain_00_pointer() { return 0x58D18; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_01_pointer() { return 0x58C6C; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_02_pointer() { return 0x58C74; }//戦闘アニメの床
        public uint lookup_table_battle_terrain_03_pointer() { return 0x58C7C; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_04_pointer() { return 0x58C84; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_05_pointer() { return 0x58C8C; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_06_pointer() { return 0x58C94; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_07_pointer() { return 0x58C9C; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_08_pointer() { return 0x58CA4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_09_pointer() { return 0x58CAC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_10_pointer() { return 0x58CB4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_11_pointer() { return 0x58CBC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_12_pointer() { return 0x58CC4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_13_pointer() { return 0x58CCC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_14_pointer() { return 0x58CD4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_15_pointer() { return 0x58CDC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_16_pointer() { return 0x58CE4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_17_pointer() { return 0x58CEC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_18_pointer() { return 0x58CF4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_19_pointer() { return 0x58CFC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_20_pointer() { return 0x58D04; } //戦闘アニメの床
        public uint lookup_table_battle_bg_00_pointer() { return 0x58E40; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_01_pointer() { return 0x58D94; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_02_pointer() { return 0x58D9C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_03_pointer() { return 0x58DA4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_04_pointer() { return 0x58DAC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_05_pointer() { return 0x58DB4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_06_pointer() { return 0x58DBC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_07_pointer() { return 0x58DC4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_08_pointer() { return 0x58DCC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_09_pointer() { return 0x58DD4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_10_pointer() { return 0x58DDC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_11_pointer() { return 0x58DE4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_12_pointer() { return 0x58DEC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_13_pointer() { return 0x58DF4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_14_pointer() { return 0x58DFC; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_15_pointer() { return 0x58E04; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_16_pointer() { return 0x58E0C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_17_pointer() { return 0x58E14; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_18_pointer() { return 0x58E1C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_19_pointer() { return 0x58E24; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_20_pointer() { return 0x58E2C; } //戦闘アニメの背景
        public uint map_terrain_type_count() { return 65; } //地形の種類の数
        public uint menu_J12_always_address() { return 0x0501BC; } //メニューの表示判定関数 常に表示する
        public uint menu_J12_hide_address() { return 0x0501C4; }   //メニューの表示判定関数 表示しない
        public uint status_game_option_pointer() { return 0xB638C; } //ゲームオプション
        public uint status_game_option_order_pointer() { return 0xB6318; } //ゲームオプションの並び順
        public uint status_game_option_order2_pointer() { return 0x0; } //ゲームオプションの並び順2 FE7のみ
        public uint status_game_option_order_count_address() { return 0xB6652; } //ゲームオプションの個数
        public uint status_units_menu_pointer() { return 0x94588; } //部隊メニュー
        public uint tactician_affinity_pointer() { return 0x0; } //軍師属性(FE7のみ)
        public uint event_final_serif_pointer() { return 0x0; } //終章セリフ(FE7のみ)
        public uint compress_image_borderline_address() { return 0xDB000; } //これ以降に圧縮画像が登場するというアドレス
        public uint patch_C01_hack(out uint enable_value) { enable_value = 0x47004800; return 0x5040; } //C01 patch
        public uint patch_C48_hack(out uint enable_value) { enable_value = 0x0805A440; return 0x59B94; } //C48 patch
        public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x00000010; return 0x02140BC; } //16_tracks_12_sounds patch
        public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x0; } //章の名前をテキストにするパッチ
        public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x5E70; } //一般兵の顔 拡張
        public uint patch_stairs_hack(out uint enable_value) { enable_value = 0x47184b00; return 0x225C4; } //階段拡張
        public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x031F58; } //ユニットアクションの拡張
        public uint patch_write_build_version(out uint enable_value) { enable_value = 0x47184b00; return 0xCA278; } //ビルドバージョンを書き込む
        public uint builddate_address() { return 0xDC110; }
        public byte[] defualt_event_script_term_code() { return new byte[] { 0x20, 0x01, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード
        public byte[] defualt_event_script_toplevel_code() { return new byte[] { 0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード
        public byte[] defualt_event_script_mapterm_code() { return new byte[] { 0x20, 0x01, 0x00, 0x00 }; } //ワールドマップイベント命令を終了させるディフォルトコード
        public uint main_menu_width_address() { return 0x5C56F6; } //メインメニューの幅
        public uint map_default_count() { return 0x4F; }    // ディフォルトのマップ数
        public uint wait_menu_command_id() { return 0x6B; } //WaitメニューのID
        public uint font_default_begin() { return 0x579CDC; }
        public uint font_default_end() { return 0x5B8CDC; }
        public string get_shop_name(uint shop_object)//店の名前
        {
            if (shop_object == 0x16)
            {
                return R._("武器屋");
            }
            else if (shop_object == 0x17)
            {
                return R._("道具屋");
            }
            else if (shop_object == 0x18)
            {
                return R._("秘密屋");
            }
            return "";
        }
        public uint extends_address() { return 0x09000000; }  //拡張領域
        public uint orignal_crc32() { return 0x9d76826f; } //無改造ROMのCRC32
        public bool is_multibyte() { return true; }    // マルチバイトを利用するか？
        public int version() { return 8; }    // バージョン
    };
}
