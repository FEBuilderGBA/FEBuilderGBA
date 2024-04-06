using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE8U : ROMFEINFO
    {
        public ROMFE8U(ROM rom)
        {
           VersionToFilename = "FE8U"; 
           TitleToFilename = "FE8"; 
           mask_point_base_pointer = 0x0006DC;  // Huffman tree end (indirected twice)
           mask_pointer = 0x0006E0;   // Huffman tree start (indirected once)
           text_pointer = 0x00A2A0;  // textの開始位置
           text_recover_address = 0x15D48C;  // textの開始位置(上記ポインタを壊している改造があるののでその対策)
           text_data_start_address = 0xE8414;  //textデータの規定値の開始位置
           text_data_end_address = 0x15AB80;  // textデータの規定値の開始位置
           unit_pointer = U.FindROMPointer(rom, 0x2c, new uint[] { 0x10108, 0x104EC, 0x10538, 0xA0B40, 0xA0BB8, 0xBE954});  // ユニットの開始位置
           unit_maxcount = 255;  // ユニットの最大数
           unit_datasize = 52;  // ユニットのデータサイズ
           max_level_address = 0x02BA78;  // 最大レベルの値を格納しているアドレス
           max_luck_address = 0x2c016;  // 最大レベルの幸運の値を格納しているアドレス
           class_pointer = U.FindROMPointer(rom, 0x88, new uint[] { 0x017AB8, 0x17DBC, 0x18DAC, 0x19458});  // クラスの開始位置
           class_datasize = 84;   // ユニットのデータサイズ
           bg_pointer = U.FindROMPointer(rom, 0x8, new uint[] { 0x00E894, 0x00ECF4, 0x00EDF8, 0x0010E44});  //BGベースアドレス
           portrait_pointer = 0x005524;  //顔ベースアドレス
           portrait_datasize = 28; 
           icon_pointer = 0x0036B4;  // アイコンの開始位置
           icon_orignal_address = 0x5926F4;  // アイコンの初期値
           icon_orignal_max = 0xDF;  // アイコンの最大数

           icon_palette_pointer = 0x0035D0;  // アイコンの開始位置
           unit_wait_icon_pointer = U.FindROMPointer(rom, 0x4, new uint[] { 0x026730, 0x267B0, 0x26838, 0x26C88, 0x28064, 0x27F74});  // ユニット待機アイコンの開始位置
           unit_wait_barista_anime_address = 0x272D8;   // ユニット待機アイコンのバリスタのアニメ指定アドレス
           unit_wait_barista_id = 0x5b;   // ユニット待機アイコンのバリスタの位置
           unit_icon_palette_address = 0x59EE20;  // ユニット(自軍)のパレットのアドレス
           unit_icon_enemey_palette_address = 0x59EE40;  // ユニット(敵軍)のパレットのアドレス
           unit_icon_npc_palette_address = 0x59EE60;  // ユニット(友軍)のパレットのアドレス
           unit_icon_gray_palette_address = 0x59EE80;  // ユニット(グレー)のパレットのアドレス
           unit_icon_four_palette_address = 0x59EEA0;  // ユニット(4軍))のパレットの開始位置
           unit_icon_lightrune_palette_address = 0x59EEC0;  // ユニット(光の結界)のパレットの開始位置
           unit_icon_sepia_palette_address = 0x59EEE0;  // ユニット(セピア)のパレットの開始位置

           unit_move_icon_pointer = U.FindROMPointer(rom, 0x4, new uint[] { 0x079584, 0x79598, 0xBAC2C, 0xBAC40});  // ユニット移動アイコンの開始位置
           lightrune_uniticon_id = 0x66;  // ユニット(光の結界)のユニットアイコンのID
           map_setting_pointer = U.FindROMPointer(rom, (uint addr) => { return rom.u32(0x12) < 0xE; }, new uint[] { 0x0B5F98, 0x0B61C0, 0x0B6328, 0x0B6500, 0x03462C, 0xB5E68 });   // マップ設定の開始位置
           map_setting_datasize = 148;  //マップ設定のデータサイズ
           map_setting_event_plist_pos = 116;  //event plistの場所 
           map_setting_worldmap_plist_pos = 117;  //woldmap event plistの場所 
           map_setting_clear_conditon_text_pos = 0x8A;  //マップの右上に表示されているクリア条件の定義場所 
           map_setting_name_text_pos = 0x70;  //マップ名のテキスト定義場所 
           map_config_pointer = 0x019900;       //マップ設定の開始位置(config)
           map_obj_pointer = 0x019968;          //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
           map_pal_pointer = 0x01999C;          //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
           map_tileanime1_pointer = 0x030134;   //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
           map_tileanime2_pointer = 0x030C78;   //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
           map_map_pointer_pointer = 0x034680;  //マップ設定の開始位置(map)
           map_mapchange_pointer = 0x0346AC;    //マップ設定の開始位置(mapchange)
           map_event_pointer = 0x0346DC;        //マップ設定の開始位置(event)
           map_worldmapevent_pointer = 0x0;  //マップ設定の開始位置(worldmap (FE6のみ))
           map_map_pointer_list_default_size = 0xEC;  //PLIST拡張をしていない時のバニラでのPLISTの数
           image_battle_animelist_pointer = U.FindROMPointer(rom, 0xC, new uint[] { 0x059BD8, 0x5A600, 0x5A694, 0x5A82C, 0x6F6A8, 0x70A60});    // 戦闘アニメリストの開始位置
           support_unit_pointer = 0x803D90;    // 支援相手の開始位置
           support_talk_pointer = 0x084784;    // 支援会話の開始位置
           unit_palette_color_pointer = 0x57394;   // ユニットのパレット(カラー)の開始位置
           unit_palette_class_pointer = 0x575B4;   // ユニットのパレット(クラス)の開始位置
           support_attribute_pointer = 0x28534;   //支援効果の開始位置
           terrain_recovery_pointer = 0x1A264;  //地形回復 全クラス共通
           terrain_bad_status_recovery_pointer = 0x01A274;  //地形回復 全クラス共通
           terrain_show_infomation_pointer = 0x8CA5C;  //地形ウィンドウに情報表示 全クラス共通
           ballista_movcost_pointer = 0x18D60; //地形ウィンドウに情報表示 全クラス共通
           ccbranch_pointer = 0xCC7D0;  // CC分岐の開始位置
           ccbranch2_pointer = 0xCC7C8;  // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
           class_alphaname_pointer = 0x0;  //英語版ではクラスの文字ID TEXTが、そのまま使われるらしいので不要
           map_terrain_name_pointer = 0x1a254;  //マップの地名表記の開始位置
           image_chapter_title_pointer = 0x89668;  //章タイトルの開始位置
           image_chapter_title_palette = 0xA19CAC;  // 章タイトルのパレット 多分違う
           image_unit_palette_pointer = 0x59BFC;  // ユニットパレットの開始位置
           item_pointer = U.FindROMPointer(rom, (uint addr) => { return rom.u32(0xC) == 0x0; }, new uint[] { 0x177C0, 0x16570, 0x166D0, 0x1671C, 0x167E0, 0x16A10, 0x16B18, 0x16BB4});  // アイテムの開始位置
           item_datasize = 36;  // アイテムのデータサイズ
           item_effect_pointer = 0x58014;  // アイテムエフェクトの開始位置
           sound_table_pointer = 0x28BC;  // ソングテーブルの開始位置
           sound_room_pointer = 0x1BC14;  //サウンドルームの開始位置
           sound_room_datasize = 16;  // サウンドルームのデータサイズ
           sound_room_cg_pointer = 0x0;  // サウンドルームの背景リスト(FE7のみ)
           event_ballte_talk_pointer = 0x846AC;  //交戦時セリフの開始位置
           event_ballte_talk2_pointer = 0;  // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
           event_haiku_pointer = 0x8472c;  //死亡時セリフの開始位置
           event_haiku_tutorial_1_pointer = 0x0;  // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
           event_haiku_tutorial_2_pointer = 0x0;  // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
           event_force_sortie_pointer = 0x8483C;  // 強制出撃の開始位置
           event_tutorial_pointer = 0x0;  //イベントチュートリアルポインタ FE7のみ
           map_exit_point_pointer = 0x3E8AC;  // 離脱ポイント開始サイズ
           map_exit_point_npc_blockadd = 65;  // arr[+65] からNPCらしい.
           map_exit_point_blank = 0xD84E0;  // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
           sound_boss_bgm_pointer = 0x72908;  // ボスBGMの開始位置
           sound_foot_steps_pointer = 0x78dd4;  //クラス足音の開始位置
           sound_foot_steps_switch2_address = 0x78DC2;
           sound_foot_steps_data_pointer = 0x7902C;    //足音のデータ構造の先頭
           worldmap_scroll_somedata_pointer = 0xBE528; //ワールドマップのスクロール関係のデータ
           worldmap_point_pointer = 0xBE84;  // ワールドマップ拠点の開始位置
           worldmap_bgm_pointer = 0xB9F94;  // ワールドマップのBGMテーブルの開始位置
           worldmap_icon_data_pointer = 0xBB674;  // ワールドマップのアイコンデータのテーブルの開始位置
           worldmap_event_on_stageclear_pointer = 0xBA3D0;  // ワールドマップイベント 拠点クリア時
           worldmap_event_on_stageselect_pointer = 0xBA41C;  // ワールドマップイベント 拠点選択時
           worldmap_county_border_pointer = 0xC2B74;  // ワールドマップ国名の表示
           worldmap_county_border_palette_pointer = 0xC27A4;  // ワールドマップ国名の表示のパレット
           item_shop_hensei_pointer = 0x99E64;  //編成準備店
           item_cornered_pointer = 0x2C7CC;  //すくみの開始位置
           ed_1_pointer = 0xB674C;   //ED 倒れたら撤退するかどうか
           ed_2_pointer = 0xb6728;   //ED 通り名
           ed_3a_pointer = 0xA3D1A8;   //ED その後 エイルーク編　
           ed_3b_pointer = 0xA3D1AC;   //ED その後 エフラム編
           ed_3c_pointer = 0x0;   //ED その後 FE7 リン編
           generic_enemy_portrait_pointer = 0x5F94;  //一般兵の顔
           generic_enemy_portrait_count = 8;  //一般兵の顔の個数

           cc_item_hero_crest_itemid = 0x64;   //CCアイテム 英雄の証
           cc_item_knight_crest_itemid = 0x65;   //CCアイテム 騎士の勲章
           cc_item_orion_bolt_itemid = 0x66;   //CCアイテム オリオンの矢
           cc_elysian_whip_itemid = 0x67;   //CCアイテム 天空のムチ
           cc_guiding_ring_itemid = 0x68;   //CCアイテム 導きの指輪
           cc_fallen_contract_itemid = 0x8A;   //CCアイテム ダミー8A
           cc_master_seal_itemid = 0x88;   //CCアイテム マスタープルフ
           cc_ocean_seal_itemid = 0x97;   //CCアイテム 覇者の証
           cc_moon_bracelet_itemid = 0x98;   //CCアイテム 月の腕輪
           cc_sun_bracelet_itemid = 0x99;   //CCアイテム 太陽の腕輪

           cc_item_hero_crest_pointer = 0x29398;   //CCアイテム 英雄の証
           cc_item_knight_crest_pointer = 0x293A0;   //CCアイテム 騎士の勲章
           cc_item_orion_bolt_pointer = 0x293A8;   //CCアイテム オリオンの矢
           cc_elysian_whip_pointer = 0x293B0;   //CCアイテム 天空のムチ
           cc_guiding_ring_pointer = 0x293B8;   //CCアイテム 導きの指輪
           cc_fallen_contract_pointer = 0x293d8;   //CCアイテム ダミー8A
           cc_master_seal_pointer = 0x293C0;   //CCアイテム マスタープルフ
           cc_ocean_seal_pointer = 0x029408;   //CCアイテム 覇者の証
           cc_moon_bracelet_pointer = 0x291D0;   //CCアイテム 月の腕輪
           cc_sun_bracelet_pointer = 0x29214;   //CCアイテム 太陽の腕輪
           unit_increase_height_pointer = 0x5C38;   //ステータス画面で背を伸ばす
           unit_increase_height_switch2_address = 0x5C26; 
           op_class_demo_pointer = 0xa2fcb4;  //OPクラスデモ
           op_class_font_pointer = 0xB2DA0;   //OPクラス日本語フォント
           op_class_font_palette_pointer = 0x95C80;   // OPクラス紹介フォントのパレット
           status_font_pointer = 0x4AC8;   //ステータス画面用のフォント
           status_font_count = 0x100;   //ステータス画面用のフォントの数(英語版と日本語で数が違う)
           ed_staffroll_image_pointer = 0x206E24;  // スタッフロール
           ed_staffroll_palette_pointer = 0xC45C4;  // スタッフロールのパレット
           op_prologue_image_pointer = 0xC4CBC;  //OP字幕
           op_prologue_palette_color_pointer = 0xC45C4;  // OP字幕のパレット ???

           arena_class_near_weapon_pointer = 0x3194C;  //闘技場 近接武器クラス 
           arena_class_far_weapon_pointer = 0x31958;  // 闘技場 遠距離武器クラス
           arena_class_magic_weapon_pointer = 0x319A8;  // 闘技場 魔法武器クラス
           arena_enemy_weapon_basic_pointer = 0x31BB8;  // 闘技場 敵武器テーブル基本武器
           arena_enemy_weapon_rankup_pointer = 0x31BD8;  // 闘技場 敵武器テーブルランクアップ武器
           link_arena_deny_unit_pointer = 0x97E8C;  //通信闘技場 禁止ユニット 
           worldmap_road_pointer = 0xC2C0;  // ワールドマップの道

           uint submenu_pointer = PatchUtil.SearchSubMenuMenuDefinePointerFE8U(rom);
           menu_definiton_pointer = U.FindROMPointer(rom, 8, new uint[] { submenu_pointer, 0x1C02C, 0x1C2F4, 0x1C0EC, 0x1C2E0, 0x33488, 0x299B4 });  //メニュー定義
           menu_promotion_pointer = 0xCDC10;  //CC決定する選択子
           menu_promotion_branch_pointer = 0xCDDCC;  //FE8にある分岐CCメニュー
           menu_definiton_split_pointer = 0x86510;   //FE8にある分岐メニュー
           menu_definiton_worldmap_pointer = 0xBC488;  //FE8のワールドマップのメニュー
           menu_definiton_worldmap_shop_pointer = 0xBC5EC;  //FE8のワールドマップ店のメニュー        
           menu_unit_pointer =  0x59D1F8;  // ユニットメニュー
           menu_game_pointer =  0x59D21C;  // ゲームメニュー
           menu_debug1_pointer = 0x59CFB8;   // デバッグメニュー
           menu_item_pointer = 0x59D120;   // アイテム利用メニュー
           MenuCommand_UsabilityAlways = 0x04F448;  //メニューを開くという値を返す関数のアドレス
           MenuCommand_UsabilityNever = 0x04F450;  //メニューを開かないという値を返す関数のアドレス       
           status_rmenu_unit_pointer = 0x889D8;  // ステータス RMENU1
           status_rmenu_game_pointer = 0x889E0;  // ステータス RMENU2
           status_rmenu3_pointer = 0x889F8;  // ステータス RMENU3
           status_rmenu4_pointer = 0x37450;  // 戦闘予測 RMENU4
           status_rmenu5_pointer = 0x37468;  // 戦闘予測 RMENU5
           status_rmenu6_pointer = 0xA01CE0;  // 状況画面 RMENU6
           status_param1_pointer = 0x871BC;  // ステータス PARAM1
           status_param2_pointer = 0x874F0;  // ステータス PARAM2
           status_param3w_pointer = 0x0;  // ステータス PARAM3 武器 海外版には"剣"みたいな武器の属性表示がありません
           status_param3m_pointer = 0x0;  // ステータス PARAM3 魔法
           systemmenu_common_image_pointer = 0x5B6470;  //システムメニューの画像
           systemmenu_common_palette_pointer = 0x036D2C;  //システムパレット 無圧縮4パレット
           systemmenu_goal_tsa_pointer = 0x08D280;  //システムメニュー 目的表示TSA
           systemmenu_terrain_tsa_pointer = 0x08CB30;  //システムメニュー 地形表示TSA

           systemmenu_name_image_pointer = 0x5B6470;  //システムメニュー 名前表示画像(FE8は共通画像)
           systemmenu_name_tsa_pointer = 0x08C70C;  //システムメニュー 名前表示TSA
           systemmenu_name_palette_pointer = 0x036D2C;  //システムメニュー 名前表示パレット

           systemmenu_battlepreview_image_pointer = 0x5B6470;  //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
           systemmenu_battlepreview_tsa_pointer = 0x0368A0;  //戦闘プレビューTSA
           systemmenu_battlepreview_palette_pointer = 0x036D2C;  //戦闘プレビューパレット

           systemarea_move_gradation_palette_pointer = 0x01DA54;  //行動範囲
           systemarea_attack_gradation_palette_pointer = 0x01DA58;  //攻撃範囲
           systemarea_staff_gradation_palette_pointer = 0x01DA5C;  //杖の範囲

           systemmenu_badstatus_image_pointer = 0x8C450;  //無圧縮のバッドステータス画像
           systemmenu_badstatus_palette_pointer = 0x898C0;  //バッドステータスのパレット
           systemmenu_badstatus_old_image_pointer = 0;  //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
           systemmenu_badstatus_old_palette_pointer = 0x0;  //昔の圧縮のバッドステータス画像のパレット FE7 FE6

           bigcg_pointer = 0xB65F0;  // CG
           end_cg_address = 0x206C08;  // END CG FE8のみ
           worldmap_big_image_pointer = 0xBA7FC;  //ワールドマップ フィールドになるでかい奴  
           worldmap_big_palette_pointer = 0xBA808;  //ワールドマップ フィールドになるでかい奴 パレット  
           worldmap_big_dpalette_pointer = 0xBF7B0;  //ワールドマップ フィールドになるでかい奴 闇パレット  
           worldmap_big_palettemap_pointer = 0xBA800;  //ワールドマップ フィールドになるでかい奴 パレットマップ
           worldmap_event_image_pointer = 0xC2044;  //ワールドマップ イベント用 
           worldmap_event_palette_pointer = 0xC2048;  //ワールドマップ イベント用 パレット  
           worldmap_event_tsa_pointer = 0xC204C;  //ワールドマップ イベント用 TSA
           worldmap_mini_image_pointer = 0xc3e6c;  //ワールドマップ ミニマップ 
           worldmap_mini_palette_pointer = 0xC3E74;  //ワールドマップ ミニマップ パレット  
           worldmap_icon_palette_pointer = 0xB8E48;  //ワールドアイコンと道パレット
           worldmap_icon1_pointer = 0xB8E4C;  //ワールドマップ アイコン1
           worldmap_icon2_pointer = 0xB8E54;  //ワールドマップ アイコン2
           worldmap_road_tile_pointer = 0xB8F98;  //ワールドマップ  道チップ
           map_load_function_pointer = 0xBD084;  //マップチャプターに入ったときの処理(FE8のみ)
           map_load_function_switch1_address = 0xBD070; 
           system_icon_pointer = 0x156AC; //システム アイコン集
           system_icon_palette_pointer = 0x156B8; //システム アイコンパレット集
           system_icon_width_address = 0x15690;  //システムアイコンの幅が書かれているアドレス
           system_weapon_icon_pointer = 0x9DC5C; //剣　斧　弓などの武器属性アイコン集
           system_weapon_icon_palette_pointer = 0x91178; //剣　斧　弓などの武器属性アイコン集のパレット
           system_music_icon_pointer = 0x0B1C7C; //音楽関係のアイコン集
           system_music_icon_palette_pointer = 0x0B1C70; //音楽関係のアイコン集のパレット
           weapon_rank_s_bonus_address = 0x2AD74; //武器ランクSボーナス設定
           weapon_battle_flash_address = 0x58ad2; //神器 戦闘時フラッシュ
           weapon_effectiveness_2x3x_address = 0x2ab18; //神器 2倍 3倍特効
           font_item_address =  0x58c7ec; //アイテム名とかに使われるフォント 関数 08004504
           font_serif_address = 0x58f6f4;  //セリフとかに使われるフォント
           monster_probability_pointer = 0x7834C;  //魔物発生確率
           monster_item_item_pointer = 0x783f0;  //魔物所持アイテム アイテム確率
           monster_item_probability_pointer = 0x783ec;  //魔物所持アイテム 所持確率
           monster_item_table_pointer = 0x78360;  //魔物所持アイテム アイテムと所持を管理するテーブル
           monster_wmap_base_point_pointer = 0xC18D8;  //ワールドマップに魔物を登場させる処理達
           monster_wmap_stage_1_pointer = 0xC17C0; 
           monster_wmap_stage_2_pointer = 0xC17F8; 
           monster_wmap_probability_1_pointer = 0xC17C4; 
           monster_wmap_probability_2_pointer = 0xC17FC; 
           monster_wmap_probability_after_1_pointer = 0xC1814; 
           monster_wmap_probability_after_2_pointer = 0xC18D4;
           worldmap_skirmish_startevent_pointer = 0x01540C;
           worldmap_skirmish_endevent_pointer = 0x0832C0;
           battle_bg_pointer = 0x75A68;  //戦闘背景
           battle_terrain_pointer = 0x51E48;  //戦闘地形
           senseki_comment_pointer = 0x0;  //戦績コメント FE8にはない
           unit_custom_battle_anime_pointer = 0x0;  //ユニット専用アニメ FE7にある

           magic_effect_pointer = 0x5B3F8;  //魔法効果へのポインタ
           magic_effect_original_data_count = 0x48;  //もともとあった魔法数

           system_move_allowicon_pointer = 0x32E98; //移動するときの矢印アイコン
           system_move_allowicon_palette_pointer = 0x32EA0;  //移動するときの矢印アイコン アイコンパレット集
           system_tsa_16color_304x240_pointer = 0xB05A0;  //16色304x240 汎用TSAポインタ
           eventunit_data_size = 20;  //ユニット配置のデータサイズ
           eventcond_tern_size = 12;  //イベント条件 ターン条件のサイズ FE7->16 FE8->12
           eventcond_talk_size = 16;  //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
           oping_event_pointer = 0x8B39F0; 
           ending1_event_pointer = 0x9EC0; 
           ending2_event_pointer = 0x9ED8; 
           RAMSlotTable_address = 0x59A5D0; 
           supply_pointer_address = 0x31524;   //輸送隊RAMへのアドレス
           workmemory_player_units_address = 0x0202BE4C;     //ワークメモリ PLAYER UNIT
           workmemory_enemy_units_address = 0x0202CFBC;     //ワークメモリ PLAYER UNIT
           workmemory_npc_units_address = 0x0202DDCC;     //ワークメモリ PLAYER UNIT
           workmemory_mapid_address = 0x0202BCFE;     //ワークメモリ マップID
           workmemory_chapterdata_address = workmemory_mapid_address - 0xE;  //ワークメモリ章データ
           workmemory_chapterdata_size = 0x4C;     //ワークメモリ 章データのサイズ
           workmemory_battle_actor_address = 0x0203A4EC;  //ワークメモリ 戦闘時のユニット構造体
           workmemory_battle_target_address = 0x0203A56C;  //ワークメモリ 戦闘時のユニット構造体
           workmemory_worldmap_data_address = 0x03005280; //ワークメモリ ワールドマップ関係の起点
           workmemory_worldmap_data_size = 0xC4;  //ワークメモリ ワールドマップ関係のサイズ
           workmemory_arena_data_address = 0x0203A8F0; //ワークメモリ 闘技場関係の起点
           workmemory_ai_data_address = 0x0203aa04;  //ワークメモリ AI関係の起点
           workmemory_action_data_address = 0x0203a958;  //ワークメモリ ActionData
           workmemory_dungeon_data_address = 0x030017A0;  //ワークメモリ ダンジョン FE8のみ
           workmemory_battlesome_data_address = 0x0203E0F0;  //ワークメモリ バルトに関係する諸データ
           workmemory_battleround_data_address = 0x0203a5ec;  //ワークメモリ　戦闘のラウンドデータ
           workmemory_last_string_address = 0x0202B6AC;   //ワークメモリ 最後に表示した文字列
           workmemory_text_buffer_address = 0x0202A6AC;   //ワークメモリ デコードされたテキスト
           workmemory_next_text_buffer_address = 0x03000048;   //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
           workmemory_local_flag_address = 0x03005270;   //ワークメモリ グローバルフラグ
           workmemory_global_flag_address = 0x03005250;   //ワークメモリ ローカルフラグ
           workmemory_trap_address = 0x0203A614;   //ワークメモリ トラップデータ
           workmemory_bwl_address = 0x0203E884;   //BWLデータ
           workmemory_clear_turn_address = 0x0203ECF4;  //ワークメモリ クリアターン数
           workmemory_clear_turn_count = 0x24;   //クリアターン数 最大数
           workmemory_memoryslot_address = 0x030004B8;   //ワークメモリ メモリスロットFE8
           workmemory_eventcounter_address = 0x03000568;   //イベントカウンター メモリスロットFE8
           workmemory_procs_forest_address = 0x02026A70;   //ワークメモリ Procs
           workmemory_procs_pool_address = 0x02024E68;   //ワークメモリ Procs
           function_sleep_handle_address = 0x08003290 + 1;   //ワークメモリ Procs待機中
           workmemory_user_stack_base_address = 0x03007DEC;  //ワークメモリ スタックの一番底
           function_fe_main_return_address = 0x08000AE0 + 1;  //スタックの一番底にある戻り先
           workmemory_control_unit_address = 0x03004E50;  //ワークメモリ 操作ユニット
           workmemory_bgm_address = 0x02024E5C;  //ワークメモリ BGM
           function_event_engine_loop_address = 0x0800ce4c + 1;  //イベントエンジン
           workmemory_reference_procs_event_address_offset = 0x34;  //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
           workmemory_procs_game_main_address = 0x02024E68;  //ワークメモリ Procsの中でのGAMEMAIN
           workmemory_palette_address = 0x020228A8;  //RAMに記録されているダブルバッファのパレット領域
           workmemory_sound_player_00_address = 0x03006440;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_01_address = 0x03006650;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_02_address = 0x03006690;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_03_address = 0x030066D0;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_04_address = 0x03006720;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_05_address = 0x03006760;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_06_address = 0x03006610;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_07_address = 0x03006400;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_sound_player_08_address = 0x030063C0;  //RAMに設定されているサウンドプレイヤーバッファ
           workmemory_keybuffer_address = 0x02024CC0;  //RAMのキーバッファ
           procs_maptask_pointer = 0x30FD8;  //PROCSのMAPTASK 
           procs_soundroomUI_pointer = 0xAFF2C;  //PROCSのSoundRoomUI 
           procs_game_main_address = 0x085916D4;  //PROCSのGAME MAIN 
           summon_unit_pointer = 0x02442C;  //召喚
           summons_demon_king_pointer = 0x7B32C;  //呼魔
           summons_demon_king_count_address = 0x7B2BC;  //呼魔リストの数
           mant_command_pointer = 0x06D0D0;  //マント
           mant_command_startadd = 0x6B;  //マント開始数
           mant_command_count_address = 0x6D0B6;  //マント数 アドレス
           unit_increase_height_yes = 0x08005C98;   //ステータス画面で背を伸ばす 伸ばす
           unit_increase_height_no =  0x08005C9C;   //ステータス画面で背を伸ばす 伸ばさない
           battle_screen_TSA1_pointer = 0x51CBC;   //戦闘画面
           battle_screen_TSA2_pointer = 0x51CC0;   //戦闘画面
           battle_screen_TSA3_pointer = 0x51378;   //戦闘画面
           battle_screen_TSA4_pointer = 0x51380;   //戦闘画面
           battle_screen_TSA5_pointer = 0x52170;   //戦闘画面
           battle_screen_palette_pointer = 0x52178;   //戦闘画面 パレット
           battle_screen_image1_pointer = 0x51F68;   //戦闘画面 画像1
           battle_screen_image2_pointer = 0x51FC8;   //戦闘画面 画像2 左側名前
           battle_screen_image3_pointer = 0x52028;   //戦闘画面 画像3 左側武器
           battle_screen_image4_pointer = 0x52088;   //戦闘画面 画像4 右側名前
           battle_screen_image5_pointer = 0x52164;   //戦闘画面 画像5 右側武器
           ai1_pointer = 0x5A91E4;   //AI1ポインタ
           ai2_pointer = 0x5A91D8;   //AI2ポインタ
           ai3_pointer = 0x3E1DC;   //AI3ポインタ
           ai_steal_item_pointer = 0x3B7C0;   //AI盗むAI アイテム評価テーブル 0x5A83A4
           ai_preform_staff_pointer = 0x3FA3C;   //AI杖 杖評価テーブル
           ai_preform_staff_direct_asm_pointer = 0x3FAE0;   //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
           ai_preform_item_pointer = 0x040820;  //AIアイテム アイテム評価テーブル
           ai_preform_item_direct_asm_pointer = 0x408C8;   //AIアイテム アイテム評価テーブル
           ai_map_setting_pointer = 0x3970C;   //AI 章ごとの設定テーブル 0xD8538
           item_usability_array_pointer = 0x288AC;  //アイテムを利用できるか判定する
           item_usability_array_switch2_address = 0x2889A; 
           item_effect_array_pointer = 0x2FC8C;     //アイテムを利用した場合の効果を定義する
           item_effect_array_switch2_address = 0x2FC72; 
           item_promotion1_array_pointer = 0x29218;    //CCアイテムを使った場合の処理を定義する
           item_promotion1_array_switch2_address = 0x29202; 
           item_promotion2_array_pointer = 0x0;   //CCアイテムかどうかを定義する(FE7のみ)
           item_promotion2_array_switch2_address = 0x0; 
           item_staff1_array_pointer = 0x28E88;     //アイテムのターゲット選択の方法を定義する(多分)
           item_staff1_array_switch2_address = 0x28E76; 
           item_staff2_array_pointer = 0x72590;     //杖の種類を定義する
           item_staff2_array_switch2_address = 0x7257C; 
           item_statbooster1_array_pointer = 0x2F8B4;     //ドーピングアイテムを利用した時のメッセージを定義する
           item_statbooster1_array_switch2_address = 0x2F8A4; 
           item_statbooster2_array_pointer = 0x29F30;     //ドーピングアイテムとCCアイテムかどうかを定義する
           item_statbooster2_array_switch2_address = 0x29F1C; 
           item_errormessage_array_pointer = 0x28C28;     //アイテム利用時のエラーメッセージ
           item_errormessage_array_switch2_address = 0x28C16; 
           event_function_pointer_table_pointer = 0x0CEE0;     //イベント命令ポインタ
           event_function_pointer_table2_pointer = 0x0CF08;    //イベント命令ポインタ2 ワールドマップ
           item_effect_pointer_table_pointer = 0x05B3F8;    //間接エフェクトポインタ
           command_85_pointer_table_pointer = 0x71C28;     //85Commandポインタ
           dic_main_pointer = 0xCE380;      //辞書メインポインタ
           dic_chaptor_pointer = 0xCE1FC;   //辞書章ポインタ
           dic_title_pointer = 0xCE23C;    //辞書タイトルポインタ
           itemicon_mine_id = 0x8c;   // アイテムアイコンのフレイボムの位置
           item_gold_id = 0x77;   // お金を取得するイベントに利用されるゴールドのID
           unitaction_function_pointer = 0x3205C;   // ユニットアクションポインタ
           lookup_table_battle_terrain_00_pointer = 0x57ECC;  //戦闘アニメの床
           lookup_table_battle_terrain_01_pointer = 0x57E20;  //戦闘アニメの床
           lookup_table_battle_terrain_02_pointer = 0x57E28; //戦闘アニメの床
           lookup_table_battle_terrain_03_pointer = 0x57E30;  //戦闘アニメの床
           lookup_table_battle_terrain_04_pointer = 0x57E38;  //戦闘アニメの床
           lookup_table_battle_terrain_05_pointer = 0x57E40;  //戦闘アニメの床
           lookup_table_battle_terrain_06_pointer = 0x57E48;  //戦闘アニメの床
           lookup_table_battle_terrain_07_pointer = 0x57E50;  //戦闘アニメの床
           lookup_table_battle_terrain_08_pointer = 0x57E58;  //戦闘アニメの床
           lookup_table_battle_terrain_09_pointer = 0x57E60;  //戦闘アニメの床
           lookup_table_battle_terrain_10_pointer = 0x57E68;  //戦闘アニメの床
           lookup_table_battle_terrain_11_pointer = 0x57E70;  //戦闘アニメの床
           lookup_table_battle_terrain_12_pointer = 0x57E78;  //戦闘アニメの床
           lookup_table_battle_terrain_13_pointer = 0x57E80;  //戦闘アニメの床
           lookup_table_battle_terrain_14_pointer = 0x57E88;  //戦闘アニメの床
           lookup_table_battle_terrain_15_pointer = 0x57E90;  //戦闘アニメの床
           lookup_table_battle_terrain_16_pointer = 0x57E98;  //戦闘アニメの床
           lookup_table_battle_terrain_17_pointer = 0x57EA0;  //戦闘アニメの床
           lookup_table_battle_terrain_18_pointer = 0x57EA8;  //戦闘アニメの床
           lookup_table_battle_terrain_19_pointer = 0x57EB0;  //戦闘アニメの床
           lookup_table_battle_terrain_20_pointer = 0x57EB8;  //戦闘アニメの床
           lookup_table_battle_bg_00_pointer = 0x57FF4;  //戦闘アニメの背景
           lookup_table_battle_bg_01_pointer = 0x57F48;  //戦闘アニメの背景
           lookup_table_battle_bg_02_pointer = 0x57F50;  //戦闘アニメの背景
           lookup_table_battle_bg_03_pointer = 0x57F58;  //戦闘アニメの背景
           lookup_table_battle_bg_04_pointer = 0x57F60;  //戦闘アニメの背景
           lookup_table_battle_bg_05_pointer = 0x57F68;  //戦闘アニメの背景
           lookup_table_battle_bg_06_pointer = 0x57F70;  //戦闘アニメの背景
           lookup_table_battle_bg_07_pointer = 0x57F78;  //戦闘アニメの背景
           lookup_table_battle_bg_08_pointer = 0x57F80;  //戦闘アニメの背景
           lookup_table_battle_bg_09_pointer = 0x57F88;  //戦闘アニメの背景
           lookup_table_battle_bg_10_pointer = 0x57F90;  //戦闘アニメの背景
           lookup_table_battle_bg_11_pointer = 0x57F98;  //戦闘アニメの背景
           lookup_table_battle_bg_12_pointer = 0x57FA0;  //戦闘アニメの背景
           lookup_table_battle_bg_13_pointer = 0x57FA8;  //戦闘アニメの背景
           lookup_table_battle_bg_14_pointer = 0x57FB0;  //戦闘アニメの背景
           lookup_table_battle_bg_15_pointer = 0x57FB8;  //戦闘アニメの背景
           lookup_table_battle_bg_16_pointer = 0x57FC0;  //戦闘アニメの背景
           lookup_table_battle_bg_17_pointer = 0x57FC8;  //戦闘アニメの背景
           lookup_table_battle_bg_18_pointer = 0x57FD0;  //戦闘アニメの背景
           lookup_table_battle_bg_19_pointer = 0x57FD8;  //戦闘アニメの背景
           lookup_table_battle_bg_20_pointer = 0x57FE0;  //戦闘アニメの背景
           map_terrain_type_count = 65;  //地形の種類の数
           menu_J12_always_address = 0x4F448;  //メニューの表示判定関数 常に表示する
           menu_J12_hide_address = 0x4F450;    //メニューの表示判定関数 表示しない
           status_game_option_pointer = 0xB17D8;  //ゲームオプション
           status_game_option_order_pointer = 0xB16F8;  //ゲームオプションの並び順
           status_game_option_order2_pointer = 0x0;  //ゲームオプションの並び順2 FE7のみ
           status_game_option_order_count_address = 0xB1A32;  //ゲームオプションの個数
           status_units_menu_pointer = 0x92290;  //部隊メニュー
           tactician_affinity_pointer = 0x0;  //軍師属性(FE7のみ)
           event_final_serif_pointer = 0x0;  //終章セリフ(FE7のみ)
           compress_image_borderline_address = 0xDB000;  //これ以降に圧縮画像が登場するというアドレス

           builddate_address = 0xD74D0; 

           Default_event_script_term_code = new byte[] { 0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00 };  //イベント命令を終了させるディフォルトコード
           Default_event_script_toplevel_code = new byte[] { 0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00 };  //イベント命令を終了させるディフォルトコード
           Default_event_script_mapterm_code = new byte[] { 0x20, 0x01, 0x00, 0x00 };  //ワールドマップイベント命令を終了させるディフォルトコード
           main_menu_width_address = 0x59D216;  //メインメニューの幅
           map_default_count = 0x4F;     // ディフォルトのマップ数
           wait_menu_command_id = 0x6B;  //WaitメニューのID
           font_default_begin = 0x589C9C; 
           font_default_end = 0x58FAF0; 
           item_name_article_pointer = 0x161AC;  // a|an|the を切り替えるテーブル 英語版のみ
           item_name_article_switch2_address = 0x16198; 
           vanilla_field_config_address = 0x1AB69C;     //バニラのタイルセット よく使われもの
           vanilla_field_image_address = 0x198D8C; 
           vanilla_village_config_address = 0x1AA140; 
           vanilla_village_image_address = 0x191B14; 
           vanilla_casle_config_address = 0x1A8CCC; 
           vanilla_casle_image_address = 0x18B89C; 
           vanilla_plain_config_address = 0x1A7674; 
           vanilla_plain_image_address = 0x185154; 
           map_minimap_tile_array_pointer = 0xA7A34;  //minimapのチップ割り当て
           bg_reserve_black_bgid = 0x35; 
           bg_reserve_random_bgid = 0x37; 

           worldmap_node_armory_empty_address = 0x8A3EFB4;  // ワールドマップ拠点での武器屋のnullアドレス
           worldmap_node_vendor_empty_address = 0x8A3F078;  // ワールドマップ拠点での道具屋のnullアドレス
           worldmap_node_secret_empty_address = 0x8A3F16A;  // ワールドマップ拠点での秘密の店のnullアドレス

           extends_address = 0x09000000;   //拡張領域
           orignal_crc32 = 0xa47246ae;  //無改造ROMのCRC32
           is_multibyte = false;     // マルチバイトを利用するか？
           version = 8;     // バージョン

           OverwriteROMConstants(rom);
        }

        override public uint patch_C01_hack(out uint enable_value) { enable_value = 0x47004800; return 0x5138; } //C01 patch
        override public uint patch_C48_hack(out uint enable_value) { enable_value = 0x08059698; return 0x58d64; } //C48 patch
        override public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x00000010; return 0x022440c; } //16_tracks_12_sounds patch
        override public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x89624; } //章の名前をテキストにするパッチ
        override public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x5F6C; } //一般兵の顔 拡張
        override public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x03200C; } //ユニットアクションの拡張
        override public uint patch_write_build_version(out uint enable_value) { enable_value = 0x47184b00; return 0xC54B0; } //ビルドバージョンを書き込む
        override public string get_shop_name(uint shop_object)//店の名前
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
    };

}
    