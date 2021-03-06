using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE6JP : IROMFEINFO
    {
        public String game_id() { return "AFEJ01"; }    // ゲームバージョンコード
        public String VersionToFilename() { return "FE6"; }
        public String TitleToFilename() { return "FE6"; }
        public uint mask_point_base_pointer() { return 0x0006DC; } // Huffman tree end (indirected twice)
        public uint mask_pointer() { return 0x0006E0; }  // Huffman tree start (indirected once)
        public uint text_pointer() { return 0x13B10; } // textの開始位置
        public uint text_recover_address() { return 0xF635C; } // textの開始位置(上記ポインタを壊している改造があるののでその対策)
        public uint text_data_start_address() { return 0x9FAC0; } // textデータの規定値の開始位置
        public uint text_data_end_address()   { return 0xF9790; } // textデータの規定値の開始位置
        public uint unit_pointer() { return 0x17680; } // ユニットの開始位置
        public uint unit_maxcount() { return 226; } // ユニットの最大数
        public uint unit_datasize() { return 48; } // ユニットのデータサイズ
        public uint max_level_address() { return 0x25132; } // 最大レベルの値を格納しているアドレス(たぶん)
        public uint max_luck_address() { return 0x254ba; } // 最大レベルの幸運の値を格納しているアドレス
        public uint class_pointer() { return 0x176E0; } // クラスの開始位置
        public uint class_datasize() { return 72; }  // クラスのデータサイズ
        public uint bg_pointer() { return 0x0E374; } //BGベースアドレス
        public uint face_pointer() { return 0x7fd8; } //顔ベースアドレス
        public uint face_datasize() { return 16;  }
        public uint icon_pointer() { return 0x4cec; } // アイコンの開始位置
        public uint icon_orignal_address() { return 0xF9D80; } // アイコンの初期値
        public uint icon_orignal_max(){ return 0x9f; } // アイコンの最大数

        public uint icon_palette_pointer() { return 0x4ac4; } // アイコンのパレットの開始位置
        public uint unit_wait_icon_pointer() { return 0x21c74; } // ユニット待機アイコンの開始位置
        public uint unit_wait_barista_anime_address() { return 0x2218C; }  // ユニット待機アイコンのバリスタのアニメ指定アドレス
        public uint unit_wait_barista_id() { return 0x42; }  // ユニット待機アイコンのバリスタの位置
        public uint unit_icon_palette_address() { return 0x0100968; } // ユニットのパレットの開始位置
        public uint unit_icon_enemey_palette_address() { return 0x100988; } // ユニット(敵軍)のパレットの開始位置
        public uint unit_icon_npc_palette_address() { return 0x1009A8; } // ユニット(友軍)のパレットの開始位置
        public uint unit_icon_gray_palette_address() { return 0x1009C8; } // ユニット(グレー))のパレットの開始位置
        public uint unit_icon_four_palette_address() { return 0x1009E8; } // ユニット(4軍))のパレットの開始位置
        public uint unit_icon_lightrune_palette_address() { return 0x0; } // ユニット(光の結界)のパレットの開始位置
        public uint unit_icon_sepia_palette_address() { return 0x0;  } // ユニット(セピア)のパレットの開始位置

        public uint unit_move_icon_pointer() { return 0x60ED8; } // ユニット移動アイコンの開始位置
        public uint lightrune_uniticon_id() { return 0; } // ユニット(光の結界)のユニットアイコンのID
        public uint map_setting_pointer() { return 0x2bb20; }  // マップ設定の開始位置
        public uint map_setting_datasize() {
            if (PatchUtil.SearchSpecialHack() == PatchUtil.SpecialHack_enum.MoDUPS)
            {
                return 72;
            }
            return 68; 
        } //マップ設定のデータサイズ
        public uint map_setting_event_plist_pos() { return 58; } //event plistの場所 
        public uint map_setting_worldmap_plist_pos() { return 59; } //woldmap event plistの場所 
        public uint map_config_pointer() { return 0x018a7c; }      //マップ設定の開始位置(config)
        public uint map_obj_pointer() { return 0x018AE4; }         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_pal_pointer() { return 0x018B18; }         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_tileanime1_pointer() { return 0x028344; }  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_tileanime2_pointer() { return 0x028E7C; }  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_map_pointer_pointer() { return 0x2BB70; } //マップ設定の開始位置(map)
        public uint map_mapchange_pointer() { return 0x02BB9C; }   //マップ設定の開始位置(mapchange)
        public uint map_event_pointer() { return 0x02BBCC; }       //マップ設定の開始位置(event)
        public uint map_worldmapevent_pointer() { return 0x9310C; } //マップ設定の開始位置(worldmap (FE6のみ))
        public uint image_battle_animelist_pointer() { return 0x4b0f8; }   // 戦闘アニメリストの開始位置
        public uint support_unit_pointer() { return 0x6076fc; }   // 支援相手の開始位置
        public uint support_talk_pointer() { return 0x6afe0; }   // 支援会話の開始位置
        public uint unit_palette_color_pointer() { return 0x0; }  // ユニットのパレット(カラー)の開始位置
        public uint unit_palette_class_pointer() { return 0x0; }  // ユニットのパレット(クラス)の開始位置
        public uint support_attribute_pointer() { return 0x22dbc; }  //支援効果の開始位置
        public uint terrain_recovery_pointer() { return 0x192d0; } //地形回復 全クラス共通
        public uint terrain_bad_status_recovery_pointer() { return 0x0192E0; } //地形回復 全クラス共通
        public uint ccbranch_pointer() { return 0x0; } // CC分岐の開始位置
        public uint ccbranch2_pointer() { return 0x0; } // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
        public uint class_alphaname_pointer() { return 0x95B48; } // クラスのアルファベット表記の開始位置
        public uint map_terrain_name_pointer() { return 0x192c0; } // マップの地名表記の開始位置
        public uint image_chapter_title_pointer() { return 0x70d44; } // 章タイトルの開始位置
        public uint image_chapter_title_palette() { return 0x3094F4; } // 章タイトルのパレット 多分違う
        public uint image_unit_palette_pointer() { return 0x4B11C; } // ユニットパレットの開始位置
        public uint item_pointer() { return 0x16410; } // アイテムの開始位置
        public uint item_datasize() { return 32; } // アイテムのデータサイズ
        public uint item_effect_pointer() { return 0x49db4; } // アイテムエフェクトの開始位置
        public uint sound_table_pointer() { return 0x3748; } // ソングテーブルの開始位置
        public uint sound_room_pointer() { return 0x8b9c4; } // サウンドルームの開始位置
        public uint sound_room_datasize() { return 12;  } // サウンドルームのデータサイズ
        public uint sound_room_cg_pointer() { return 0x0; } // サウンドルームの背景リスト(FE7のみ)
        public uint event_ballte_talk_pointer() { return 0x6b660; } // 交戦時セリフの開始位置
        public uint event_ballte_talk2_pointer() { return 0x6b744;} // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
        public uint event_haiku_pointer() { return 0x6b7fc; } // 死亡時セリフの開始位置
        public uint event_haiku_tutorial_1_pointer() { return 0x0; } // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_haiku_tutorial_2_pointer() { return 0x0; } // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_force_sortie_pointer() { return 0x0; } // 強制出撃の開始位置
        public uint event_tutorial_pointer() { return 0x0; } //イベントチュートリアルポインタ FE7のみ
        public uint map_exit_point_pointer() { return 0x32c60; } // 離脱ポイント開始サイズ
        public uint map_exit_point_npc_blockadd() { return 0x2d; } // NPC離脱.
        public uint map_exit_point_blank() { return 0x10DA1C; } // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
        public uint sound_boss_bgm_pointer() { return 0x0; } // ボスBGMの開始位置
        public uint sound_foot_steps_pointer() { return 0x0; } // クラス足音の開始位置
        public uint sound_foot_steps_switch2_address() { return 0x0;}
        public uint worldmap_point_pointer() { return 0x0; } // ワールドマップ拠点の開始位置
        public uint worldmap_bgm_pointer(){ return 0x0; } // ワールドマップのBGMテーブルの開始位置
        public uint worldmap_icon_data_pointer() { return 0;  } // ワールドマップのアイコンデータのテーブルの開始位置
        public uint worldmap_event_on_stageclear_pointer() { return 0x0; } // ワールドマップイベントクリア時
        public uint worldmap_event_on_stageselect_pointer() { return 0x0; } // ワールドマップイベント 拠点選択時
        public uint worldmap_county_border_pointer() { return 0; } // ワールドマップ国名の表示
        public uint worldmap_county_border_palette_pointer() { return 0x0; } // ワールドマップ国名の表示のパレット
        public uint item_shop_hensei_pointer() { return 0x95f10; } //編成準備店
        public uint item_cornered_pointer() { return 0x25a9c; } //すくみの開始位置
        public uint ed_1_pointer() { return 0x0; }  //ED 倒れたら撤退するかどうか
        public uint ed_2_pointer() { return 0x0; }  //ED 通り名
        public uint ed_3a_pointer() { return 0x91834; }  //ED 後日談　
        public uint ed_3b_pointer() { return 0x0; }  //ED FE6にはない
        public uint ed_3c_pointer() { return 0x0; }  //ED FE6にはない
        public uint generic_enemy_portrait_pointer() { return 0x8df0; } //一般兵の顔
        public uint generic_enemy_portrait_count() { return 0x8-1; } //一般兵の顔の個数
        public uint cc_item_hero_crest_itemid() { return 0x5F; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_itemid() { return 0x60; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_itemid() { return 0x61; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_itemid() { return 0x62; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_itemid() { return 0x63; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_itemid() { return 0x0; }  //CCアイテム ダミー8A
        public uint cc_master_seal_itemid() { return 0x0; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_itemid() { return 0x0; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_itemid() { return 0x0; }  //CCアイテム 月の腕輪
        public uint cc_sun_bracelet_itemid() { return 0x0; }  //CCアイテム 太陽の腕輪

        public uint cc_item_hero_crest_pointer() { return 0x237b0; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_pointer() { return 0x237b8; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_pointer() { return 0x237c0; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_pointer() { return 0x237c8; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_pointer() { return 0x237f0; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_pointer() { return 0x0; }  //CCアイテム ダミー8A 闇の契約書
        public uint cc_master_seal_pointer() { return 0x0; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_pointer() { return 0x0; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_pointer() { return 0x0; }  //CCアイテム 月の腕輪 天の刻印
        public uint cc_sun_bracelet_pointer() { return 0x0; }  //CCアイテム 太陽の腕輪 天の刻印

        public uint unit_increase_height_pointer() { return 0x0; }  //ステータス画面で背を伸ばす
        public uint unit_increase_height_switch2_address() { return 0x0; }
        public uint op_class_demo_pointer() { return 0x0; } //OPクラスデモ
        public uint op_class_font_pointer() { return 0x0; }  //OPクラス日本語フォント
        public uint op_class_font_palette_pointer() { return 0x0; }  // OPクラス紹介フォントのパレット
        public uint status_font_pointer() { return 0x0; }  //ステータス画面用のフォント
        public uint status_font_count() { return 0x0; }  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
        public uint ed_staffroll_image_pointer() { return 0x0; } // スタッフロール
        public uint ed_staffroll_palette_pointer() { return 0x0; } // スタッフロールのパレット
        public uint op_prologue_image_pointer() { return 0x0; } // OP字幕
        public uint op_prologue_palette_color_pointer() { return 0x0; } // OP字幕のパレット ???

        public uint arena_class_near_weapon_pointer() { return 0x299fc; } //闘技場 近接武器クラス 
        public uint arena_class_far_weapon_pointer() { return 0x29a08; } // 闘技場 遠距離武器クラス
        public uint arena_class_magic_weapon_pointer() { return 0x29A58; } // 闘技場 魔法武器クラス

        public uint arena_enemy_weapon_basic_pointer() { return 0x29C44; } // 闘技場 敵武器テーブル基本武器
        public uint arena_enemy_weapon_rankup_pointer() { return 0x29c64; } // 闘技場 敵武器テーブルランクアップ武器
        public uint link_arena_deny_unit_pointer() { return 0;  } //通信闘技場 禁止ユニット 

        public uint worldmap_road_pointer() { return 0x0; } // ワールドマップの道

        public uint menu_definiton_pointer() { return 0x1B2B4; } //メニュー定義
        public uint menu_promotion_pointer() { return 0x0; } //CC決定する選択子
        public uint menu_promotion_branch_pointer() { return 0x0; } //FE8にある分岐CCメニュー
        public uint menu_definiton_split_pointer() { return 0x0; }  //FE8にある分岐メニュー
        public uint menu_definiton_worldmap_pointer() { return 0x0; } //FE8のワールドマップのメニュー
        public uint menu_definiton_worldmap_shop_pointer() { return 0x0; } //FE8のワールドマップ店のメニュー        
        public uint menu_unit_pointer() { return  0x5c7608; } // ユニットメニュー
        public uint menu_game_pointer() { return  0x5C7650; } // ゲームメニュー
        public uint menu_debug1_pointer() { return 0x5C73EC; }  // デバッグメニュー
        public uint MenuCommand_UsabilityAlways() { return 0x041E6C; } //メニューを開くという値を返す関数のアドレス
        public uint MenuCommand_UsabilityNever() { return 0x0; } //メニューを開かないという値を返す関数のアドレス       
        public uint status_rmenu_unit_pointer() { return 0x70344; } // ステータス RMENU1
        public uint status_rmenu_game_pointer() { return 0x7034c; } // ステータス RMENU2
        public uint status_rmenu3_pointer() { return 0x70364; } // ステータス RMENU3
        public uint status_rmenu4_pointer() { return 0x2E420; } // 戦闘予測 RMENU4
        public uint status_rmenu5_pointer() { return 0x2E438; } // 戦闘予測 RMENU5
        public uint status_rmenu6_pointer() { return 0x0; } // 状況画面 RMENU6
        public uint status_param1_pointer() { return 0x6ED8C; } // ステータス PARAM1
        public uint status_param2_pointer() { return 0x6F148; } // ステータス PARAM2
        public uint status_param3w_pointer() { return 0x6F3D8; } // ステータス PARAM3 武器
        public uint status_param3m_pointer() { return 0x6F394; } // ステータス PARAM3 魔法

        public uint systemmenu_common_image_pointer() { return 0x0732EC; } //システムメニューの画像
        public uint systemmenu_common_palette_pointer() { return 0x097008; } //システムパレット 無圧縮4パレット
        public uint systemmenu_goal_tsa_pointer() { return 0x0; } //システムメニュー 目的表示TSA FE6にはない
        public uint systemmenu_terrain_tsa_pointer() { return 0x072E4C; } //システムメニュー 地形表示TSA

        public uint systemmenu_name_image_pointer() { return 0x0732EC; } //システムメニュー 名前表示画像(FE8は共通画像)
        public uint systemmenu_name_tsa_pointer() { return 0x072AB4; } //システムメニュー 名前表示TSA
        public uint systemmenu_name_palette_pointer() { return 0x072754; } //システムメニュー 名前表示パレット

        public uint systemmenu_battlepreview_image_pointer() { return 0x02DE34; } //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
        public uint systemmenu_battlepreview_tsa_pointer() { return 0x02D97C; } //戦闘プレビューTSA
        public uint systemmenu_battlepreview_palette_pointer() { return 0x02DD54; } //戦闘プレビューパレット

        public uint systemarea_move_gradation_palette_pointer() { return 0x01BFC8; } //行動範囲
        public uint systemarea_attack_gradation_palette_pointer() { return 0x01BFCC; } //攻撃範囲
        public uint systemarea_staff_gradation_palette_pointer() { return 0x01BFD0; } //杖の範囲

        public uint systemmenu_badstatus_image_pointer() { return 0; } //無圧縮のバッドステータス画像
        public uint systemmenu_badstatus_palette_pointer() { return 0x70EE8; } //バッドステータスのパレット
        public uint systemmenu_badstatus_old_image_pointer() { return 0x732EC; } //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス

        public uint bigcg_pointer() { return 0x0; } // CG
        public uint end_cg_address() { return 0x0; } // END CG FE8のみ
        public uint worldmap_big_image_pointer() { return 0x68C2DC; } //ワールドマップ フィールドになるでかい奴  
        public uint worldmap_big_palette_pointer() { return 0x68C2E0; } //ワールドマップ フィールドになるでかい奴 パレット  
        public uint worldmap_big_dpalette_pointer() { return 0x0; } //ワールドマップ フィールドになるでかい奴 闇パレット  
        public uint worldmap_big_palettemap_pointer() { return 0x0; } //ワールドマップ フィールドになるでかい奴 パレットマップ
        public uint worldmap_event_image_pointer() { return 0x0; } //ワールドマップ イベント用 
        public uint worldmap_event_palette_pointer() { return 0x0; } //ワールドマップ イベント用 パレット  
        public uint worldmap_event_tsa_pointer() { return 0x0; } //ワールドマップ イベント用 TSA
        public uint worldmap_mini_image_pointer() { return 0x0; } //ワールドマップ ミニマップ 
        public uint worldmap_mini_palette_pointer() { return 0x0; } //ワールドマップ ミニマップ パレット  
        public uint worldmap_icon_palette_pointer() { return 0x0; } //ワールドアイコンと道パレット
        public uint worldmap_icon1_pointer() { return 0x0; } //ワールドマップ アイコン1
        public uint worldmap_icon2_pointer() { return 0x0; } //ワールドマップ アイコン2
        public uint worldmap_road_tile_pointer() { return 0x0; } //ワールドマップ  道チップ
        public uint map_load_function_pointer() { return 0x0; } //マップチャプターに入ったときの処理(FE8のみ)
        public uint map_load_function_switch1_address() { return 0x0; }
        public uint system_icon_pointer() { return 0x15B70; }//システム アイコン集
        public uint system_icon_palette_pointer() { return 0x15B7C; }//システム アイコンパレット集
        public uint system_icon_width_address() { return 0x15B54; } //システムアイコンの幅が書かれているアドレス
        public uint system_weapon_icon_pointer() { return 0x750DC; }//剣　斧　弓などの武器属性アイコン集
        public uint system_weapon_icon_palette_pointer() { return 0x750E4; }//剣　斧　弓などの武器属性アイコン集のパレット
        public uint system_music_icon_pointer() { return 0x08C98C; }//音楽関係のアイコン集
        public uint system_music_icon_palette_pointer() { return 0x08C980; }//音楽関係のアイコン集のパレット
        public uint weapon_rank_s_bonus_address() { return 0; }//武器ランクSボーナス設定
        public uint weapon_battle_flash_address() { return 0; }//神器 戦闘時フラッシュ
        public uint weapon_effectiveness_2x3x_address() { return 0; }//神器 2倍 3倍特効
        public uint font_item_address() { return 0x59027C; }//アイテム名とかに使われるフォント
        public uint font_serif_address()  { return 0x5A82B0; } //セリフとかに使われるフォント
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
        public uint battle_bg_pointer() { return 0x5F090; } //戦闘背景
        public uint battle_terrain_pointer() { return 0x44534; } //戦闘地形
        public uint senseki_comment_pointer() { return 0x0; } //戦績コメント
        public uint unit_custom_battle_anime_pointer() { return 0x0; } //ユニット専用アニメ FE7にある
        public uint magic_effect_pointer() { return 0x4C8C8; } //魔法効果へのポインタ
        public uint magic_effect_original_data_count() { return 0x33; } //もともとあった魔法数
        public uint system_move_allowicon_pointer() { return 0x2ad0c; }//移動するときの矢印アイコン
        public uint system_move_allowicon_palette_pointer() { return 0x2ad14;} //移動するときの矢印アイコン アイコンパレット集
        public uint system_tsa_16color_304x240_pointer() { return 0x8F058; } //16色304x240 汎用TSAポインタ
        public uint eventunit_data_size() { return 16; } //ユニット配置のデータサイズ
        public uint eventcond_tern_size() { return 12; } //イベント条件 ターン条件のサイズ FE7->16 FE8->12
        public uint eventcond_talk_size() { return 12; } //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
        public uint oping_event_pointer() { return 0x0; }
        public uint ending1_event_pointer() { return 0x0; }
        public uint ending2_event_pointer() { return 0x0; }
        public uint workmemory_player_units_address() { return 0x0202AB78; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_enemy_units_address() { return 0x0202BCE8; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_npc_units_address() { return 0x202CAF8; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_mapid_address() { return 0x0202AA56; }    //ワークメモリ マップID
        public uint workmemory_last_string_address() { return 0x0202A404; }  //ワークメモリ 最後に表示した文字列
        public uint workmemory_text_buffer_address() { return 0x02029404; }  //ワークメモリ デコードされたテキスト
        public uint workmemory_next_text_buffer_address() { return 0x03000038; }  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
        public uint workmemory_local_flag_address() { return 0x03004898; }  //ワークメモリ グローバルフラグ
        public uint workmemory_global_flag_address() { return 0x03004890; }  //ワークメモリ ローカルフラグ
        public uint workmemory_trap_address() { return 0x02039330; }  //ワークメモリ トラップ
        public uint workmemory_bwl_address() { return 0x0203D524; }  //BWLデータ
        public uint workmemory_clear_turn_address() { return 0x0203D994; } //ワークメモリ クリアターン数
        public uint workmemory_clear_turn_count() { return 0x20; }  //クリアターン数 最大数
        public uint workmemory_memoryslot_address() { return 0x02039330; }  //ワークメモリ メモリスロットFE8
        public uint workmemory_eventcounter_address() { return 0x0; }  //イベントカウンター メモリスロットFE8
        public uint workmemory_procs_forest_address() { return 0x020258CC; }  //ワークメモリ Procs
        public uint workmemory_procs_pool_address() { return 0x02023CC4; }  //ワークメモリ Procs
        public uint function_sleep_handle_address() { return 0; }  //ワークメモリ Procs待機中
        public uint workmemory_user_stack_base_address() { return 0x03007DE0; } //ワークメモリ スタックの一番底
        public uint function_fe_main_return_address() { return 0x08000ACE + 1; } //スタックの一番底にある戻り先
        public uint workmemory_control_unit_address() { return 0x030044B0; } //ワークメモリ 操作ユニット
        public uint workmemory_bgm_address() { return 0x02023CBC; } //ワークメモリ BGM
        public uint function_event_engine_loop_address() { return 0x0800DEDC + 1; } //イベントエンジン
        public uint workmemory_reference_procs_event_address_offset() { return 0x2C; } //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
        public uint workmemory_procs_game_main_address() { return 0x02023CC4; } //ワークメモリ Procsの中でのGAMEMAIN
        public uint workmemory_palette_address() { return 0x02021708; } //RAMに記録されているダブルバッファのパレット領域
        public uint workmemory_sound_player_00_address() { return 0x030062E0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_01_address() { return 0x030064F0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_02_address() { return 0x03006530; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_03_address() { return 0x03006600; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_04_address() { return 0x03006570; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_05_address() { return 0x03006260; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_06_address() { return 0x030062A0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_07_address() { return 0x030064B0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_08_address() { return 0x030065C0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint procs_game_main_address() { return 0x85C4A34; } //PROCSのGAME MAIN 
        public uint summon_unit_pointer() { return 0; } //召喚
        public uint summons_demon_king_pointer() { return 0; } //呼魔
        public uint summons_demon_king_count_address() { return 0; } //呼魔リストの数
        public uint mant_command_pointer() { return 0x57d50; } //マント
        public uint mant_command_startadd() { return 0x0D; } //マント開始数
        public uint mant_command_count_address() { return 0x57D3C; } //マント数 アドレス
        public uint unit_increase_height_yes() { return 0; }  //ステータス画面で背を伸ばす 伸ばす
        public uint unit_increase_height_no() { return 0; }  //ステータス画面で背を伸ばす 伸ばさない
        public uint battle_screen_TSA1_pointer() { return 0x044388; }  //戦闘画面
        public uint battle_screen_TSA2_pointer() { return 0x04438C; }  //戦闘画面
        public uint battle_screen_TSA3_pointer() { return 0x043B38; }  //戦闘画面
        public uint battle_screen_TSA4_pointer() { return 0x043B40; }  //戦闘画面
        public uint battle_screen_TSA5_pointer() { return 0x04485C; }  //戦闘画面
        public uint battle_screen_palette_pointer() { return 0x044864; }  //戦闘画面 パレット
        public uint battle_screen_image1_pointer() { return 0x044654; }  //戦闘画面 画像1
        public uint battle_screen_image2_pointer() { return 0x0446B4; }  //戦闘画面 画像2
        public uint battle_screen_image3_pointer() { return 0x044714; }  //戦闘画面 画像3
        public uint battle_screen_image4_pointer() { return 0x044774; }  //戦闘画面 画像4
        public uint battle_screen_image5_pointer() { return 0x044850; }  //戦闘画面 画像5
        public uint ai1_pointer() { return 0x5C97F8; }  //AI1ポインタ
        public uint ai2_pointer() { return 0x5C97EC; }  //AI2ポインタ
        public uint ai3_pointer() { return 0x0325AC; }  //AI3ポインタ
        public uint ai_steal_item_pointer() { return 0x30228; }  //AI盗む アイテム評価テーブル 0x085C8834
        public uint ai_preform_staff_pointer() { return 0x33C00;  }  //AI杖 杖評価テーブル
        public uint ai_preform_staff_direct_asm_pointer() { return 0x33C84; }  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
        public uint ai_preform_item_pointer() { return 0x034AA8; } //AIアイテム アイテム評価テーブル
        public uint ai_preform_item_direct_asm_pointer() { return 0x34B44; }  //AIアイテム アイテム評価テーブル
        public uint ai_map_setting_pointer() { return 0x2E520; }  //AI 章ごとの設定テーブル 0x0810DA20
        public uint item_usability_array_pointer() { return 0x22ff8; } //アイテムを利用できるか判定する
        public uint item_usability_array_switch2_address() { return 0x22fe6; }
        public uint item_effect_array_pointer() { return 0x27fd4; }    //アイテムを利用した場合の効果を定義する
        public uint item_effect_array_switch2_address() { return 0x27fba; }
        public uint item_promotion1_array_pointer() { return 0x23794; }   //CCアイテムを使った場合の処理を定義する
        public uint item_promotion1_array_switch2_address() { return 0x23784; }
        public uint item_promotion2_array_pointer() { return 0x0; }  //CCアイテムかどうかを定義する(FE7のみ)
        public uint item_promotion2_array_switch2_address() { return 0x0; }
        public uint item_staff1_array_pointer() { return 0x234b4; }    //アイテムのターゲット選択の方法を定義する(多分)
        public uint item_staff1_array_switch2_address() { return 0x234a2; }
        public uint item_staff2_array_pointer() { return 0x5c438; }    //杖の種類を定義する
        public uint item_staff2_array_switch2_address() { return 0x5c426; }
        public uint item_statbooster1_array_pointer() { return 0x27ee8; }    //ドーピングアイテムを利用した時のメッセージを定義する
        public uint item_statbooster1_array_switch2_address() { return 0x27ed2; }
        public uint item_statbooster2_array_pointer() { return 0x0; }    //ドーピングアイテムとCCアイテムかどうかを定義する  (FE6にはない)
        public uint item_statbooster2_array_switch2_address() { return 0x0; }
        public uint item_errormessage_array_pointer() { return 0x23294; }    //アイテム利用時のエラーメッセージ
        public uint item_errormessage_array_switch2_address() { return 0x23282; }
        public uint event_function_pointer_table_pointer() { return 0x0E038; }    //イベント命令ポインタ
        public uint event_function_pointer_table2_pointer() { return 0x0; }   //イベント命令ポインタ2 (FE8のみ)
        public uint item_effect_pointer_table_pointer() { return 0x04C8C8; }   //間接エフェクトポインタ
        public uint command_85_pointer_table_pointer() { return 0x05BF38; }    //85Commandポインタ
        public uint dic_main_pointer() { return 0x0; }     //辞書メインポインタ
        public uint dic_chaptor_pointer() { return 0x0; }  //辞書章ポインタ
        public uint dic_title_pointer() { return 0x0; }   //辞書タイトルポインタ
        public uint itemicon_mine_id(){ return 0x0;}  // アイテムアイコンのフレイボムの位置
        public uint item_gold_id() { return 0x6f;  }  // お金を取得するイベントに利用されるゴールドのID
        public uint unitaction_function_pointer() { return 0x2A054; }  // ユニットアクションポインタ
        public uint lookup_table_battle_terrain_00_pointer() { return 0x49CF8; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_01_pointer() { return 0x49CA4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_02_pointer() { return 0x49CAC; }//戦闘アニメの床
        public uint lookup_table_battle_terrain_03_pointer() { return 0x49CB4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_04_pointer() { return 0x49CBC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_05_pointer() { return 0x49CC4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_06_pointer() { return 0x49CCC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_07_pointer() { return 0x49CD4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_08_pointer() { return 0x49CDC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_09_pointer() { return 0x49CE4; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_10_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_11_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_12_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_13_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_14_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_15_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_16_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_17_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_18_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_19_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_20_pointer() { return 0x00; } //戦闘アニメの床
        public uint lookup_table_battle_bg_00_pointer() { return 0x49D94; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_01_pointer() { return 0x49D44; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_02_pointer() { return 0x49D4C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_03_pointer() { return 0x49D54; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_04_pointer() { return 0x49D5C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_05_pointer() { return 0x49D64; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_06_pointer() { return 0x49D6C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_07_pointer() { return 0x49D74; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_08_pointer() { return 0x49D7C; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_09_pointer() { return 0x49D84; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_10_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_11_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_12_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_13_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_14_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_15_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_16_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_17_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_18_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_19_pointer() { return 0x00; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_20_pointer() { return 0x00; } //戦闘アニメの背景
        public uint map_terrain_type_count() { return 48;  } //地形の種類の数
        public uint menu_J12_always_address() { return 0x41E6C; } //メニューの表示判定関数 常に表示する
        public uint menu_J12_hide_address() { return 0x0; }   //メニューの表示判定関数 表示しない
        public uint status_game_option_pointer() { return 0x8C490; } //ゲームオプション
        public uint status_game_option_order_pointer() { return 0x68AAE8; } //ゲームオプションの並び順
        public uint status_game_option_order2_pointer() { return 0x0; } //ゲームオプションの並び順2 FE7のみ
        public uint status_game_option_order_count_address() { return 0x68AAE4; } //ゲームオプションの個数
        public uint status_units_menu_pointer() { return 0;  } //部隊メニュー
        public uint tactician_affinity_pointer() { return 0; } //軍師属性(FE7のみ)
        public uint event_final_serif_pointer() { return 0x0; } //終章セリフ(FE7のみ)
        public uint compress_image_borderline_address() { return 0xF9D80; } //これ以降に圧縮画像が登場するというアドレス

        public uint patch_C01_hack(out uint enable_value) { enable_value = 0xFD32F568; return 0x2DBF5C; } //C01 patch
        public uint patch_C48_hack(out uint enable_value) { enable_value = 0x0804AD76;  return 0x4A768; } //C48 patch
        public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x0; return 0x0; } //16_tracks_12_sounds patch
        public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x0; } //章の名前をテキストにするパッチ
        public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x8DB8; } //一般兵の顔 拡張
        public uint patch_stairs_hack(out uint enable_value) { enable_value = 0x47184b00; return 0x0; } //階段拡張
        public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x02A028; } //ユニットアクションの拡張
        public uint patch_write_build_version(out uint enable_value) { enable_value = 0x0; return 0x0; } //ビルドバージョンを書き込む
        public uint builddate_address() { return 0x0; }

        public byte[] defualt_event_script_term_code() { return new byte[] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード
        public byte[] defualt_event_script_toplevel_code() { return new byte[] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード(各章のトップレベルのイベント)
        public byte[] defualt_event_script_mapterm_code() { return new byte[] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; } //ワールドマップイベント命令を終了させるディフォルトコード
        public uint main_menu_width_address() { return 0x5C764A; } //メインメニューの幅
        public uint map_default_count() { return 0x2D; }    // ディフォルトのマップ数
        public uint wait_menu_command_id() { return 0x00; } //WaitメニューのID FE6にMenu IDはない
        public uint font_default_begin() { return 0x59060C; }
        public uint font_default_end() { return 0x5C39A0; }
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
        public uint extends_address() { return 0x08800000; }  //拡張領域
        public uint orignal_crc32() { return 0xd38763e1; } //無改造ROMのCRC32
        public bool is_multibyte() { return true; }    // マルチバイトを利用するか？
        public int version() { return 6; }    // バージョン
    };

}
    