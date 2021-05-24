using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE8U : IROMFEINFO
    {
        public String game_id() { return "BE8E01"; }    // ゲームバージョンコード
        public String VersionToFilename() { return "FE8U"; }
        public String TitleToFilename() { return "FE8"; }
        public uint mask_point_base_pointer() { return 0x0006DC; } // Huffman tree end (indirected twice)
        public uint mask_pointer() { return 0x0006E0; }  // Huffman tree start (indirected once)
        public uint text_pointer() { return 0x00A2A0; } // textの開始位置
        public uint text_recover_address() { return 0x15D48C; } // textの開始位置(上記ポインタを壊している改造があるののでその対策)
        public uint text_data_start_address() { return 0xE8414; } //textデータの規定値の開始位置
        public uint text_data_end_address() { return 0x15AB80; } // textデータの規定値の開始位置
        public uint unit_pointer() { return 0x10108; } // ユニットの開始位置
        public uint unit_maxcount() { return 255; } // ユニットの最大数
        public uint unit_datasize() { return 52; } // ユニットのデータサイズ
        public uint max_level_address() { return 0x02BA78; } // 最大レベルの値を格納しているアドレス
        public uint max_luck_address() { return 0x2c016; } // 最大レベルの幸運の値を格納しているアドレス
        public uint class_pointer() { return 0x017AB8; } // クラスの開始位置
        public uint class_datasize() { return 84; }  // ユニットのデータサイズ
        public uint bg_pointer() { return 0xE894; } //BGベースアドレス
        public uint face_pointer() { return 0x005524; } //顔ベースアドレス
        public uint face_datasize() { return 28; }
        public uint icon_pointer() { return 0x0036B4; } // アイコンの開始位置
        public uint icon_orignal_address() { return 0x5926F4; } // アイコンの初期値
        public uint icon_orignal_max() { return 0xDF; } // アイコンの最大数

        public uint icon_palette_pointer() { return 0x0035D0; } // アイコンの開始位置
        public uint unit_wait_icon_pointer() { return 0x026730; } // ユニット待機アイコンの開始位置
        public uint unit_wait_barista_anime_address() { return 0x272D8; }  // ユニット待機アイコンのバリスタのアニメ指定アドレス
        public uint unit_wait_barista_id() { return 0x5b; }  // ユニット待機アイコンのバリスタの位置
        public uint unit_icon_palette_address() { return 0x59EE20; } // ユニット(自軍)のパレットのアドレス
        public uint unit_icon_enemey_palette_address() { return 0x59EE40; } // ユニット(敵軍)のパレットのアドレス
        public uint unit_icon_npc_palette_address() { return 0x59EE60; } // ユニット(友軍)のパレットのアドレス
        public uint unit_icon_gray_palette_address() { return 0x59EE80; } // ユニット(グレー)のパレットのアドレス
        public uint unit_icon_four_palette_address() { return 0x59EEA0; } // ユニット(4軍))のパレットの開始位置
        public uint unit_icon_lightrune_palette_address() { return 0x59EEC0; } // ユニット(光の結界)のパレットの開始位置
        public uint unit_icon_sepia_palette_address() { return 0x59EEE0; } // ユニット(セピア)のパレットの開始位置

        public uint unit_move_icon_pointer() { return 0x079584; } // ユニット移動アイコンの開始位置
        public uint lightrune_uniticon_id() { return 0x66; } // ユニット(光の結界)のユニットアイコンのID
        public uint map_setting_pointer() { return 0x0B5F98; }  // マップ設定の開始位置
        public uint map_setting_datasize() { return 148; } //マップ設定のデータサイズ
        public uint map_setting_event_plist_pos() { return 116; } //event plistの場所 
        public uint map_setting_worldmap_plist_pos() { return 117; } //woldmap event plistの場所 
        public uint map_setting_clear_conditon_text_pos() { return 0x8A; } //マップの右上に表示されているクリア条件の定義場所 
        public uint map_setting_name_text_pos() { return 0x70; } //マップ名のテキスト定義場所 
        public uint map_config_pointer() { return 0x019900; }      //マップ設定の開始位置(config)
        public uint map_obj_pointer() { return 0x019968; }         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_pal_pointer() { return 0x01999C; }         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
        public uint map_tileanime1_pointer() { return 0x030134; }  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_tileanime2_pointer() { return 0x030C78; }  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
        public uint map_map_pointer_pointer() { return 0x034680; } //マップ設定の開始位置(map)
        public uint map_mapchange_pointer() { return 0x0346AC; }   //マップ設定の開始位置(mapchange)
        public uint map_event_pointer() { return 0x0346DC; }       //マップ設定の開始位置(event)
        public uint map_worldmapevent_pointer() { return 0x0; } //マップ設定の開始位置(worldmap (FE6のみ))
        public uint image_battle_animelist_pointer() { return 0x059BD8; }   // 戦闘アニメリストの開始位置
        public uint support_unit_pointer() { return 0x803D90; }   // 支援相手の開始位置
        public uint support_talk_pointer() { return 0x084784; }   // 支援会話の開始位置
        public uint unit_palette_color_pointer() { return 0x57394; }  // ユニットのパレット(カラー)の開始位置
        public uint unit_palette_class_pointer() { return 0x575B4; }  // ユニットのパレット(クラス)の開始位置
        public uint support_attribute_pointer() { return 0x28534; }  //支援効果の開始位置
        public uint terrain_recovery_pointer() { return 0x1A264; } //地形回復 全クラス共通
        public uint terrain_bad_status_recovery_pointer() { return 0x01A274; } //地形回復 全クラス共通
        public uint ccbranch_pointer() { return 0xCC7D0; } // CC分岐の開始位置
        public uint ccbranch2_pointer() { return 0xCC7C8; } // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
        public uint class_alphaname_pointer() { return 0x0; } //英語版ではクラスの文字ID TEXTが、そのまま使われるらしいので不要
        public uint map_terrain_name_pointer() { return 0x1a254; } //マップの地名表記の開始位置
        public uint image_chapter_title_pointer() { return 0x89668; } //章タイトルの開始位置
        public uint image_chapter_title_palette() { return 0xA19CAC; } // 章タイトルのパレット 多分違う
        public uint image_unit_palette_pointer() { return 0x59BFC; } // ユニットパレットの開始位置
        public uint item_pointer() { return 0x177C0; } // アイテムの開始位置
        public uint item_datasize() { return 36; } // アイテムのデータサイズ
        public uint item_effect_pointer() { return 0x58014; } // アイテムエフェクトの開始位置
        public uint sound_table_pointer() { return 0x28BC; } // ソングテーブルの開始位置
        public uint sound_room_pointer() { return 0x1BC14; } //サウンドルームの開始位置
        public uint sound_room_datasize() { return 16; } // サウンドルームのデータサイズ
        public uint sound_room_cg_pointer() { return 0x0; } // サウンドルームの背景リスト(FE7のみ)
        public uint event_ballte_talk_pointer() { return 0x846AC; } //交戦時セリフの開始位置
        public uint event_ballte_talk2_pointer() { return 0; } // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
        public uint event_haiku_pointer() { return 0x8472c; } //死亡時セリフの開始位置
        public uint event_haiku_tutorial_1_pointer() { return 0x0; } // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_haiku_tutorial_2_pointer() { return 0x0; } // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
        public uint event_force_sortie_pointer() { return 0x8483C; } // 強制出撃の開始位置
        public uint event_tutorial_pointer() { return 0x0; } //イベントチュートリアルポインタ FE7のみ
        public uint map_exit_point_pointer() { return 0x3E8AC; } // 離脱ポイント開始サイズ
        public uint map_exit_point_npc_blockadd() { return 65; } // arr[+65] からNPCらしい.
        public uint map_exit_point_blank() { return 0xD84E0; } // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
        public uint sound_boss_bgm_pointer() { return 0x72908; } // ボスBGMの開始位置
        public uint sound_foot_steps_pointer() { return 0x78dd4; } //クラス足音の開始位置
        public uint sound_foot_steps_switch2_address() { return 0x78DC2; }
        public uint worldmap_point_pointer() { return 0xBE84; } // ワールドマップ拠点の開始位置
        public uint worldmap_bgm_pointer() { return 0xB9F94; } // ワールドマップのBGMテーブルの開始位置
        public uint worldmap_icon_data_pointer() { return 0xBB674; } // ワールドマップのアイコンデータのテーブルの開始位置
        public uint worldmap_event_on_stageclear_pointer() { return 0xBA3D0; } // ワールドマップイベント 拠点クリア時
        public uint worldmap_event_on_stageselect_pointer() { return 0xBA41C; } // ワールドマップイベント 拠点選択時
        public uint worldmap_county_border_pointer() { return 0xC2B74; } // ワールドマップ国名の表示
        public uint worldmap_county_border_palette_pointer() { return 0xC27A4; } // ワールドマップ国名の表示のパレット
        public uint item_shop_hensei_pointer() { return 0x99E64; } //編成準備店
        public uint item_cornered_pointer() { return 0x2C7CC; } //すくみの開始位置
        public uint ed_1_pointer() { return 0xB674C; }  //ED 倒れたら撤退するかどうか
        public uint ed_2_pointer() { return 0xb6728; }  //ED 通り名
        public uint ed_3a_pointer() { return 0xA3D1A8; }  //ED その後 エイルーク編　
        public uint ed_3b_pointer() { return 0xA3D1AC; }  //ED その後 エフラム編
        public uint ed_3c_pointer() { return 0x0; }  //ED その後 FE7 リン編
        public uint generic_enemy_portrait_pointer() { return 0x5F94; } //一般兵の顔
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

        public uint cc_item_hero_crest_pointer() { return 0x29398; }  //CCアイテム 英雄の証
        public uint cc_item_knight_crest_pointer() { return 0x293A0; }  //CCアイテム 騎士の勲章
        public uint cc_item_orion_bolt_pointer() { return 0x293A8; }  //CCアイテム オリオンの矢
        public uint cc_elysian_whip_pointer() { return 0x293B0; }  //CCアイテム 天空のムチ
        public uint cc_guiding_ring_pointer() { return 0x293B8; }  //CCアイテム 導きの指輪
        public uint cc_fallen_contract_pointer() { return 0x293d8; }  //CCアイテム ダミー8A
        public uint cc_master_seal_pointer() { return 0x293C0; }  //CCアイテム マスタープルフ
        public uint cc_ocean_seal_pointer() { return 0x029408; }  //CCアイテム 覇者の証
        public uint cc_moon_bracelet_pointer() { return 0x291D0; }  //CCアイテム 月の腕輪
        public uint cc_sun_bracelet_pointer() { return 0x29214; }  //CCアイテム 太陽の腕輪
        public uint unit_increase_height_pointer() { return 0x5C38; }  //ステータス画面で背を伸ばす
        public uint unit_increase_height_switch2_address() { return 0x5C26; }
        public uint op_class_demo_pointer() { return 0xa2fcb4; } //OPクラスデモ
        public uint op_class_font_pointer() { return 0xB2DA0; }  //OPクラス日本語フォント
        public uint op_class_font_palette_pointer() { return 0x95C80; }  // OPクラス紹介フォントのパレット
        public uint status_font_pointer() { return 0x4AC8; }  //ステータス画面用のフォント
        public uint status_font_count() { return 0x100; }  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
        public uint ed_staffroll_image_pointer() { return 0x206E24; } // スタッフロール
        public uint ed_staffroll_palette_pointer() { return 0xC45C4; } // スタッフロールのパレット
        public uint op_prologue_image_pointer() { return 0xC4CBC; } //OP字幕
        public uint op_prologue_palette_color_pointer() { return 0xC45C4; } // OP字幕のパレット ???

        public uint arena_class_near_weapon_pointer() { return 0x3194C; } //闘技場 近接武器クラス 
        public uint arena_class_far_weapon_pointer() { return 0x31958; } // 闘技場 遠距離武器クラス
        public uint arena_class_magic_weapon_pointer() { return 0x319A8; } // 闘技場 魔法武器クラス
        public uint arena_enemy_weapon_basic_pointer() { return 0x31BB8; } // 闘技場 敵武器テーブル基本武器
        public uint arena_enemy_weapon_rankup_pointer() { return 0x31BD8; } // 闘技場 敵武器テーブルランクアップ武器
        public uint link_arena_deny_unit_pointer() { return 0x97E8C; } //通信闘技場 禁止ユニット 
        public uint worldmap_road_pointer() { return 0xC2C0; } // ワールドマップの道

        public uint menu_definiton_pointer() { return 0x1C02C; } //メニュー定義
        public uint menu_promotion_pointer() { return 0xCDC10; } //CC決定する選択子
        public uint menu_promotion_branch_pointer() { return 0xCDDCC; } //FE8にある分岐CCメニュー
        public uint menu_definiton_split_pointer() { return 0x86510; }  //FE8にある分岐メニュー
        public uint menu_definiton_worldmap_pointer() { return 0xBC488; } //FE8のワールドマップのメニュー
        public uint menu_definiton_worldmap_shop_pointer() { return 0xBC5EC; } //FE8のワールドマップ店のメニュー        
        public uint menu_unit_pointer() { return  0x59D1F8; } // ユニットメニュー
        public uint menu_game_pointer() { return  0x59D21C; } // ゲームメニュー
        public uint menu_debug1_pointer() { return 0x59CFB8; }  // デバッグメニュー
        public uint MenuCommand_UsabilityAlways() { return 0x04F448; } //メニューを開くという値を返す関数のアドレス
        public uint MenuCommand_UsabilityNever() { return 0x04F450; } //メニューを開かないという値を返す関数のアドレス       
        public uint status_rmenu_unit_pointer() { return 0x889D8; } // ステータス RMENU1
        public uint status_rmenu_game_pointer() { return 0x889E0; } // ステータス RMENU2
        public uint status_rmenu3_pointer() { return 0x889F8; } // ステータス RMENU3
        public uint status_rmenu4_pointer() { return 0x37450; } // 戦闘予測 RMENU4
        public uint status_rmenu5_pointer() { return 0x37468; } // 戦闘予測 RMENU5
        public uint status_rmenu6_pointer() { return 0xA01CE0; } // 状況画面 RMENU6
        public uint status_param1_pointer() { return 0x871BC; } // ステータス PARAM1
        public uint status_param2_pointer() { return 0x874F0; } // ステータス PARAM2
        public uint status_param3w_pointer() { return 0x0; } // ステータス PARAM3 武器 海外版には"剣"みたいな武器の属性表示がありません
        public uint status_param3m_pointer() { return 0x0; } // ステータス PARAM3 魔法
        public uint systemmenu_common_image_pointer() { return 0x5B6470; } //システムメニューの画像
        public uint systemmenu_common_palette_pointer() { return 0x036D2C; } //システムパレット 無圧縮4パレット
        public uint systemmenu_goal_tsa_pointer(){ return 0x08D280; } //システムメニュー 目的表示TSA
        public uint systemmenu_terrain_tsa_pointer() { return 0x08CB30; } //システムメニュー 地形表示TSA

        public uint systemmenu_name_image_pointer() { return 0x5B6470; } //システムメニュー 名前表示画像(FE8は共通画像)
        public uint systemmenu_name_tsa_pointer(){ return 0x08C70C; } //システムメニュー 名前表示TSA
        public uint systemmenu_name_palette_pointer(){ return 0x036D2C; } //システムメニュー 名前表示パレット

        public uint systemmenu_battlepreview_image_pointer() { return 0x5B6470; } //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
        public uint systemmenu_battlepreview_tsa_pointer(){ return 0x0368A0; } //戦闘プレビューTSA
        public uint systemmenu_battlepreview_palette_pointer() { return 0x036D2C; } //戦闘プレビューパレット

        public uint systemarea_move_gradation_palette_pointer() { return 0x01DA54; } //行動範囲
        public uint systemarea_attack_gradation_palette_pointer() { return 0x01DA58; } //攻撃範囲
        public uint systemarea_staff_gradation_palette_pointer() { return 0x01DA5C; } //杖の範囲

        public uint systemmenu_badstatus_image_pointer() { return 0x8C450; } //無圧縮のバッドステータス画像
        public uint systemmenu_badstatus_palette_pointer() { return 0x898C0; } //バッドステータスのパレット
        public uint systemmenu_badstatus_old_image_pointer() { return 0; } //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
        public uint systemmenu_badstatus_old_palette_pointer() { return 0x0; } //昔の圧縮のバッドステータス画像のパレット FE7 FE6

        public uint bigcg_pointer() { return 0xB65F0; } // CG
        public uint end_cg_address() { return 0x206C08; } // END CG FE8のみ
        public uint worldmap_big_image_pointer() { return 0xBA7FC; } //ワールドマップ フィールドになるでかい奴  
        public uint worldmap_big_palette_pointer() { return 0xBA808; } //ワールドマップ フィールドになるでかい奴 パレット  
        public uint worldmap_big_dpalette_pointer() { return 0xBF7B0; } //ワールドマップ フィールドになるでかい奴 闇パレット  
        public uint worldmap_big_palettemap_pointer() { return 0xBA800; } //ワールドマップ フィールドになるでかい奴 パレットマップ
        public uint worldmap_event_image_pointer() { return 0xC2044; } //ワールドマップ イベント用 
        public uint worldmap_event_palette_pointer() { return 0xC2048; } //ワールドマップ イベント用 パレット  
        public uint worldmap_event_tsa_pointer() { return 0xC204C; } //ワールドマップ イベント用 TSA
        public uint worldmap_mini_image_pointer() { return 0xc3e6c; } //ワールドマップ ミニマップ 
        public uint worldmap_mini_palette_pointer() { return 0xC3E74; } //ワールドマップ ミニマップ パレット  
        public uint worldmap_icon_palette_pointer() { return 0xB8E48; } //ワールドアイコンと道パレット
        public uint worldmap_icon1_pointer() { return 0xB8E4C; } //ワールドマップ アイコン1
        public uint worldmap_icon2_pointer() { return 0xB8E54; } //ワールドマップ アイコン2
        public uint worldmap_road_tile_pointer() { return 0xB8F98; } //ワールドマップ  道チップ
        public uint map_load_function_pointer() { return 0xBD084; } //マップチャプターに入ったときの処理(FE8のみ)
        public uint map_load_function_switch1_address() { return 0xBD070; }
        public uint system_icon_pointer() { return 0x156AC; }//システム アイコン集
        public uint system_icon_palette_pointer() { return 0x156B8; }//システム アイコンパレット集
        public uint system_icon_width_address() { return 0x15690; } //システムアイコンの幅が書かれているアドレス
        public uint system_weapon_icon_pointer() { return 0x9DC5C; }//剣　斧　弓などの武器属性アイコン集
        public uint system_weapon_icon_palette_pointer() { return 0x91178; }//剣　斧　弓などの武器属性アイコン集のパレット
        public uint system_music_icon_pointer() { return 0x0B1C7C; }//音楽関係のアイコン集
        public uint system_music_icon_palette_pointer() { return 0x0B1C70; }//音楽関係のアイコン集のパレット
        public uint weapon_rank_s_bonus_address() { return 0x2AD74; }//武器ランクSボーナス設定
        public uint weapon_battle_flash_address() { return 0x58ad2; }//神器 戦闘時フラッシュ
        public uint weapon_effectiveness_2x3x_address() { return 0x2ab18; }//神器 2倍 3倍特効
        public uint font_item_address() { return  0x58c7ec; }//アイテム名とかに使われるフォント 関数 08004504
        public uint font_serif_address() { return 0x58f6f4; } //セリフとかに使われるフォント
        public uint monster_probability_pointer() { return 0x7834C; } //魔物発生確率
        public uint monster_item_item_pointer() { return 0x783f0; } //魔物所持アイテム アイテム確率
        public uint monster_item_probability_pointer() { return 0x783ec; } //魔物所持アイテム 所持確率
        public uint monster_item_table_pointer() { return 0x78360; } //魔物所持アイテム アイテムと所持を管理するテーブル
        public uint monster_wmap_base_point_pointer() { return 0xC18D8; } //ワールドマップに魔物を登場させる処理達
        public uint monster_wmap_stage_1_pointer() { return 0xC17C0; }
        public uint monster_wmap_stage_2_pointer() { return 0xC17F8; }
        public uint monster_wmap_probability_1_pointer() { return 0xC17C4; }
        public uint monster_wmap_probability_2_pointer() { return 0xC17FC; }
        public uint monster_wmap_probability_after_1_pointer() { return 0xC1814; }
        public uint monster_wmap_probability_after_2_pointer() { return 0xC18D4; }
        public uint battle_bg_pointer() { return 0x75A68; } //戦闘背景
        public uint battle_terrain_pointer() { return 0x51E48; } //戦闘地形
        public uint senseki_comment_pointer() { return 0x0; } //戦績コメント FE8にはない
        public uint unit_custom_battle_anime_pointer() { return 0x0; } //ユニット専用アニメ FE7にある

        public uint magic_effect_pointer() { return 0x5B3F8; } //魔法効果へのポインタ
        public uint magic_effect_original_data_count() { return 0x48; } //もともとあった魔法数

        public uint system_move_allowicon_pointer() { return 0x32E98; }//移動するときの矢印アイコン
        public uint system_move_allowicon_palette_pointer() { return 0x32EA0; } //移動するときの矢印アイコン アイコンパレット集
        public uint system_tsa_16color_304x240_pointer() { return 0xB05A0; } //16色304x240 汎用TSAポインタ
        public uint eventunit_data_size() { return 20; } //ユニット配置のデータサイズ
        public uint eventcond_tern_size() { return 12; } //イベント条件 ターン条件のサイズ FE7->16 FE8->12
        public uint eventcond_talk_size() { return 16; } //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
        public uint oping_event_pointer() { return 0x8B39F0; }
        public uint ending1_event_pointer() { return 0x9EC0; }
        public uint ending2_event_pointer() { return 0x9ED8; }
        public uint RAMSlotTable_address() { return 0x59A5D0; }
        public uint supply_pointer_address() { return 0x31524; }  //輸送体RAMへのアドレス
        public uint workmemory_player_units_address() { return 0x0202BE4C; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_enemy_units_address() { return 0x0202CFBC; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_npc_units_address() { return 0x0202DDCC; }    //ワークメモリ PLAYER UNIT
        public uint workmemory_chapterdata_address() { return workmemory_mapid_address() - 0xE; } //ワークメモリ章データ
        public uint workmemory_mapid_address() { return 0x0202BCFE; }    //ワークメモリ マップID
        public uint workmemory_chapterdata_size() { return 0x4C; }    //ワークメモリ 章データのサイズ
        public uint workmemory_battle_actor_address() { return 0x0203A4EC; } //ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_battle_target_address() { return 0x0203A56C; } //ワークメモリ 戦闘時のユニット構造体
        public uint workmemory_worldmap_data_address() { return 0x03005280; }//ワークメモリ ワールドマップ関係の起点
        public uint workmemory_worldmap_data_size() { return 0xC4; } //ワークメモリ ワールドマップ関係のサイズ
        public uint workmemory_arena_data_address() { return 0x0203A8F0; }//ワークメモリ 闘技場関係の起点
        public uint workmemory_ai_data_address() { return 0x0203aa04; } //ワークメモリ AI関係の起点
        public uint workmemory_action_data_address() { return 0x0203a958; } //ワークメモリ ActionData
        public uint workmemory_dungeon_data_address() { return 0x030017A0; } //ワークメモリ ダンジョン FE8のみ
        public uint workmemory_battlesome_data_address() { return 0x0203E0F0; } //ワークメモリ バルトに関係する諸データ
        public uint workmemory_last_string_address() { return 0x0202B6AC; }  //ワークメモリ 最後に表示した文字列
        public uint workmemory_text_buffer_address() { return 0x0202A6AC; }  //ワークメモリ デコードされたテキスト
        public uint workmemory_next_text_buffer_address() { return 0x03000048; }  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
        public uint workmemory_local_flag_address() { return 0x03005270; }  //ワークメモリ グローバルフラグ
        public uint workmemory_global_flag_address() { return 0x03005250; }  //ワークメモリ ローカルフラグ
        public uint workmemory_trap_address() { return 0x0203A614; }  //ワークメモリ ローカルフラグ
        public uint workmemory_bwl_address() { return 0x0203E884; }  //BWLデータ
        public uint workmemory_clear_turn_address() { return 0x0203ECF4; } //ワークメモリ クリアターン数
        public uint workmemory_clear_turn_count() { return 0x24; }  //クリアターン数 最大数
        public uint workmemory_memoryslot_address() { return 0x030004B8; }  //ワークメモリ メモリスロットFE8
        public uint workmemory_eventcounter_address() { return 0x03000568; }  //イベントカウンター メモリスロットFE8
        public uint workmemory_procs_forest_address() { return 0x02026A70; }  //ワークメモリ Procs
        public uint workmemory_procs_pool_address() { return 0x02024E68; }  //ワークメモリ Procs
        public uint function_sleep_handle_address() { return 0x08003290 + 1; }  //ワークメモリ Procs待機中
        public uint workmemory_user_stack_base_address() { return 0x03007DEC; } //ワークメモリ スタックの一番底
        public uint function_fe_main_return_address() { return 0x08000AE0 + 1; } //スタックの一番底にある戻り先
        public uint workmemory_control_unit_address() { return 0x03004E50; } //ワークメモリ 操作ユニット
        public uint workmemory_bgm_address() { return 0x02024E5C; } //ワークメモリ BGM
        public uint function_event_engine_loop_address() { return 0x0800ce4c + 1; } //イベントエンジン
        public uint workmemory_reference_procs_event_address_offset() { return 0x34; } //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
        public uint workmemory_procs_game_main_address() { return 0x02024E68; } //ワークメモリ Procsの中でのGAMEMAIN
        public uint workmemory_palette_address() { return 0x020228A8; } //RAMに記録されているダブルバッファのパレット領域
        public uint workmemory_sound_player_00_address() { return 0x03006440; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_01_address() { return 0x03006650; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_02_address() { return 0x03006690; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_03_address() { return 0x030066D0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_04_address() { return 0x03006720; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_05_address() { return 0x03006760; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_06_address() { return 0x03006610; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_07_address() { return 0x03006400; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint workmemory_sound_player_08_address() { return 0x030063C0; } //RAMに設定されているサウンドプレイヤーバッファ
        public uint procs_game_main_address() { return 0x085916D4; } //PROCSのGAME MAIN 
        public uint summon_unit_pointer() { return 0x02442C; } //召喚
        public uint summons_demon_king_pointer() { return 0x7B32C; } //呼魔
        public uint summons_demon_king_count_address() { return 0x7B2BC; } //呼魔リストの数
        public uint mant_command_pointer() { return 0x06D0D0; } //マント
        public uint mant_command_startadd() { return 0x6B; } //マント開始数
        public uint mant_command_count_address() { return 0x6D0B6; } //マント数 アドレス
        public uint unit_increase_height_yes() { return 0x08005C98; }  //ステータス画面で背を伸ばす 伸ばす
        public uint unit_increase_height_no() { return  0x08005C9C; }  //ステータス画面で背を伸ばす 伸ばさない
        public uint battle_screen_TSA1_pointer() { return 0x51CBC; }  //戦闘画面
        public uint battle_screen_TSA2_pointer() { return 0x51CC0; }  //戦闘画面
        public uint battle_screen_TSA3_pointer() { return 0x51378; }  //戦闘画面
        public uint battle_screen_TSA4_pointer() { return 0x51380; }  //戦闘画面
        public uint battle_screen_TSA5_pointer() { return 0x52170; }  //戦闘画面
        public uint battle_screen_palette_pointer() { return 0x52178; }  //戦闘画面 パレット
        public uint battle_screen_image1_pointer() { return 0x51F68; }  //戦闘画面 画像1
        public uint battle_screen_image2_pointer() { return 0x51FC8; }  //戦闘画面 画像2 左側名前
        public uint battle_screen_image3_pointer() { return 0x52028; }  //戦闘画面 画像3 左側武器
        public uint battle_screen_image4_pointer() { return 0x52088; }  //戦闘画面 画像4 右側名前
        public uint battle_screen_image5_pointer() { return 0x52164; }  //戦闘画面 画像5 右側武器
        public uint ai1_pointer() { return 0x5A91E4; }  //AI1ポインタ
        public uint ai2_pointer() { return 0x5A91D8; }  //AI2ポインタ
        public uint ai3_pointer() { return 0x3E1DC; }  //AI3ポインタ
        public uint ai_steal_item_pointer() { return 0x3B7C0; }  //AI盗むAI アイテム評価テーブル 0x5A83A4
        public uint ai_preform_staff_pointer() { return 0x3FA3C; }  //AI杖 杖評価テーブル
        public uint ai_preform_staff_direct_asm_pointer() { return 0x3FAE0; }  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
        public uint ai_preform_item_pointer() { return 0x040820; } //AIアイテム アイテム評価テーブル
        public uint ai_preform_item_direct_asm_pointer() { return 0x408C8; }  //AIアイテム アイテム評価テーブル
        public uint ai_map_setting_pointer() { return 0x3970C; }  //AI 章ごとの設定テーブル 0xD8538
        public uint item_usability_array_pointer() { return 0x288AC; } //アイテムを利用できるか判定する
        public uint item_usability_array_switch2_address() { return 0x2889A; }
        public uint item_effect_array_pointer() { return 0x2FC8C; }    //アイテムを利用した場合の効果を定義する
        public uint item_effect_array_switch2_address() { return 0x2FC72; }
        public uint item_promotion1_array_pointer() { return 0x29218; }   //CCアイテムを使った場合の処理を定義する
        public uint item_promotion1_array_switch2_address() { return 0x29202; }
        public uint item_promotion2_array_pointer() { return 0x0; }  //CCアイテムかどうかを定義する(FE7のみ)
        public uint item_promotion2_array_switch2_address() { return 0x0; }
        public uint item_staff1_array_pointer() { return 0x28E88; }    //アイテムのターゲット選択の方法を定義する(多分)
        public uint item_staff1_array_switch2_address() { return 0x28E76; }
        public uint item_staff2_array_pointer() { return 0x72590; }    //杖の種類を定義する
        public uint item_staff2_array_switch2_address() { return 0x7257C; }
        public uint item_statbooster1_array_pointer() { return 0x2F8B4; }    //ドーピングアイテムを利用した時のメッセージを定義する
        public uint item_statbooster1_array_switch2_address() { return 0x2F8A4; }
        public uint item_statbooster2_array_pointer() { return 0x29F30; }    //ドーピングアイテムとCCアイテムかどうかを定義する
        public uint item_statbooster2_array_switch2_address() { return 0x29F1C; }
        public uint item_errormessage_array_pointer() { return 0x28C28; }    //アイテム利用時のエラーメッセージ
        public uint item_errormessage_array_switch2_address() { return 0x28C16; }
        public uint event_function_pointer_table_pointer() { return 0x0CEE0; }    //イベント命令ポインタ
        public uint event_function_pointer_table2_pointer() { return 0x0CF08; }   //イベント命令ポインタ2 ワールドマップ
        public uint item_effect_pointer_table_pointer() { return 0x05B3F8; }   //間接エフェクトポインタ
        public uint command_85_pointer_table_pointer() { return 0x71C28; }    //85Commandポインタ
        public uint dic_main_pointer() { return 0xCE380; }     //辞書メインポインタ
        public uint dic_chaptor_pointer() { return 0xCE1FC; }  //辞書章ポインタ
        public uint dic_title_pointer() { return 0xCE23C; }   //辞書タイトルポインタ
        public uint itemicon_mine_id() { return 0x8c; }  // アイテムアイコンのフレイボムの位置
        public uint item_gold_id() { return 0x77; }  // お金を取得するイベントに利用されるゴールドのID
        public uint unitaction_function_pointer() { return 0x3205C; }  // ユニットアクションポインタ
        public uint lookup_table_battle_terrain_00_pointer() { return 0x57ECC; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_01_pointer() { return 0x57E20; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_02_pointer() { return 0x57E28; }//戦闘アニメの床
        public uint lookup_table_battle_terrain_03_pointer() { return 0x57E30; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_04_pointer() { return 0x57E38; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_05_pointer() { return 0x57E40; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_06_pointer() { return 0x57E48; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_07_pointer() { return 0x57E50; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_08_pointer() { return 0x57E58; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_09_pointer() { return 0x57E60; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_10_pointer() { return 0x57E68; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_11_pointer() { return 0x57E70; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_12_pointer() { return 0x57E78; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_13_pointer() { return 0x57E80; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_14_pointer() { return 0x57E88; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_15_pointer() { return 0x57E90; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_16_pointer() { return 0x57E98; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_17_pointer() { return 0x57EA0; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_18_pointer() { return 0x57EA8; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_19_pointer() { return 0x57EB0; } //戦闘アニメの床
        public uint lookup_table_battle_terrain_20_pointer() { return 0x57EB8; } //戦闘アニメの床
        public uint lookup_table_battle_bg_00_pointer() { return 0x57FF4; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_01_pointer() { return 0x57F48; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_02_pointer() { return 0x57F50; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_03_pointer() { return 0x57F58; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_04_pointer() { return 0x57F60; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_05_pointer() { return 0x57F68; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_06_pointer() { return 0x57F70; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_07_pointer() { return 0x57F78; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_08_pointer() { return 0x57F80; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_09_pointer() { return 0x57F88; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_10_pointer() { return 0x57F90; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_11_pointer() { return 0x57F98; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_12_pointer() { return 0x57FA0; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_13_pointer() { return 0x57FA8; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_14_pointer() { return 0x57FB0; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_15_pointer() { return 0x57FB8; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_16_pointer() { return 0x57FC0; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_17_pointer() { return 0x57FC8; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_18_pointer() { return 0x57FD0; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_19_pointer() { return 0x57FD8; } //戦闘アニメの背景
        public uint lookup_table_battle_bg_20_pointer() { return 0x57FE0; } //戦闘アニメの背景
        public uint map_terrain_type_count() { return 65; } //地形の種類の数
        public uint menu_J12_always_address() { return 0x4F448; } //メニューの表示判定関数 常に表示する
        public uint menu_J12_hide_address() { return 0x4F450; }   //メニューの表示判定関数 表示しない
        public uint status_game_option_pointer() { return 0xB17D8; } //ゲームオプション
        public uint status_game_option_order_pointer() { return 0xB16F8; } //ゲームオプションの並び順
        public uint status_game_option_order2_pointer() { return 0x0; } //ゲームオプションの並び順2 FE7のみ
        public uint status_game_option_order_count_address() { return 0xB1A32; } //ゲームオプションの個数
        public uint status_units_menu_pointer() { return 0x92290; } //部隊メニュー
        public uint tactician_affinity_pointer() { return 0x0; } //軍師属性(FE7のみ)
        public uint event_final_serif_pointer() { return 0x0; } //終章セリフ(FE7のみ)
        public uint compress_image_borderline_address() { return 0xDB000; } //これ以降に圧縮画像が登場するというアドレス

        public uint patch_C01_hack(out uint enable_value) { enable_value = 0x47004800; return 0x5138; } //C01 patch
        public uint patch_C48_hack(out uint enable_value) { enable_value = 0x08059698; return 0x58d64; } //C48 patch
        public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x00000010; return 0x022440c; } //16_tracks_12_sounds patch
        public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x89624; } //章の名前をテキストにするパッチ
        public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x5F6C; } //一般兵の顔 拡張
        public uint patch_stairs_hack(out uint enable_value) { enable_value = 0x47184b00; return 0x225F8; } //階段拡張
        public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x03200C; } //ユニットアクションの拡張
        public uint patch_write_build_version(out uint enable_value) { enable_value = 0x47184b00; return 0xC54B0; } //ビルドバージョンを書き込む
        public uint builddate_address() { return 0xD74D0; }

        public byte[] defualt_event_script_term_code() { return new byte[] { 0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード
        public byte[] defualt_event_script_toplevel_code() { return new byte[] { 0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00 }; } //イベント命令を終了させるディフォルトコード
        public byte[] defualt_event_script_mapterm_code() { return new byte[] { 0x20, 0x01, 0x00, 0x00 }; } //ワールドマップイベント命令を終了させるディフォルトコード
        public uint main_menu_width_address() { return 0x59D216; } //メインメニューの幅
        public uint map_default_count() { return 0x4F; }    // ディフォルトのマップ数
        public uint wait_menu_command_id() { return 0x6B; } //WaitメニューのID
        public uint font_default_begin() { return 0x589C9C; }
        public uint font_default_end() { return 0x58FAF0; }
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
        public uint orignal_crc32() { return 0xa47246ae; } //無改造ROMのCRC32
        public bool is_multibyte() { return false; }    // マルチバイトを利用するか？
        public int version() { return 8; }    // バージョン
    };

}
    