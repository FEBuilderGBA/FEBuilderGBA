using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE8JP : ROMFEINFO
    {
        public ROMFE8JP(ROM rom)
        {
            VersionToFilename = "FE8J"; 
            TitleToFilename = "FE8"; 
            mask_point_base_pointer = 0x0006DC;  // Huffman tree end (indirected twice)
            mask_pointer = 0x0006E0;   // Huffman tree start (indirected once)
            text_pointer = 0x00A000;  // textの開始位置
            text_recover_address = 0x14D08C;  // textの開始位置(上記ポインタを壊している改造があるののでその対策)
            text_data_start_address = 0xED7F4;  // textデータの規定値の開始位置
            text_data_end_address = 0x14929B;  // textデータの規定値の開始位置
            unit_pointer = U.FindROMPointer(rom, 0x2c, new uint[] { 0x010650, 0x010268, 0x1069C, 0xA3B64, 0xA3CE0, 0xA4928, 0xA49A0, 0xA4C3C, 0xA5458, 0xA56A8, 0xA5860, 0xA595C, 0xA5BB0, 0xA5D28, 0xA61F8 });  // ユニットのの開始位置
            unit_maxcount = 255;  // ユニットの最大数
            unit_datasize = 52;  // ユニットのデータサイズ
            max_level_address = 0x02B9C0;  // 最大レベルの値を格納しているアドレス  
            max_luck_address = 0x2bf5e;  // 最大レベルの幸運の値を格納しているアドレス
            class_pointer = U.FindROMPointer(rom, 0x88, new uint[] {0x017860,0x17ADC,0x18AC0,0x19130});  // クラスの開始位置
            class_datasize = 84;   // ユニットのデータサイズ
            bg_pointer = U.FindROMPointer(rom, 0x8, new uint[] { 0x00EAA4, 0x00EF08, 0x00F00C, 0x010F8C });  //BGベースアドレス
            portrait_pointer = 0x00542C;  //顔ベースアドレス
            portrait_datasize = 28;
            icon_pointer = 0x0036D4;
            icon_orignal_address = 0x5BA470;  // アイコンの初期値
            icon_orignal_max = 0xDF;  // アイコンの最大数

            icon_palette_pointer = 0x00351C;  // アイコンの開始位置
            unit_wait_icon_pointer = U.FindROMPointer(rom, 0x4, new uint[] { 0x0266D4, 0x26754, 0x267DC, 0x26C2C, 0x27274, 0x27B44, 0x27C30, 0x27FF8 });  // ユニット待機アイコンの開始位置
            unit_wait_barista_anime_address = 0x02727c;   // ユニット待機アイコンのバリスタのアニメ指定アドレス
            unit_wait_barista_id = 0x5b;   // ユニット待機アイコンのバリスタの位置
            unit_icon_palette_address = 0x5c7340;  // ユニット(自軍)のパレットのアドレス
            unit_icon_enemey_palette_address = 0x5c7360;  // ユニット(敵軍)のパレットのアドレス
            unit_icon_npc_palette_address = 0x5c7380;  // ユニット(友軍)のパレットのアドレス
            unit_icon_gray_palette_address = 0x5c73A0;  // ユニット(グレー))のパレットの開始位置
            unit_icon_four_palette_address = 0x5c73C0;  // ユニット(4軍))のパレットの開始位置
            unit_icon_lightrune_palette_address = 0x5C73E0;  // ユニット(光の結界)のパレットの開始位置
            unit_icon_sepia_palette_address = 0x5C7400;  // ユニット(セピア)のパレットの開始位置

            unit_move_icon_pointer = U.FindROMPointer(rom, 0x4, new uint[] { 0x07B998, 0x7B9AC, 0xBFA7C,0xBFA90,  });  // ユニット移動アイコンの開始位置
            lightrune_uniticon_id = 0x66;  // ユニット(光の結界)のユニットアイコンのID
            map_setting_pointer = U.FindROMPointer(rom, (uint addr) => { return rom.u32(0x12) < 0xE; }, new uint[] { 0x0BAB1C, 0x34534, 0xBA9EC, 0xBAD44, 0xBAEAC });   // マップ設定の開始位置
            map_setting_datasize = 148;  //マップ設定のデータサイズ
            map_setting_event_plist_pos = 116;  //event plistの場所 
            map_setting_worldmap_plist_pos = 117;  //woldmap event plistの場所 
            map_setting_clear_conditon_text_pos = 0x8A;  //マップの右上に表示されているクリア条件の定義場所 
            map_setting_name_text_pos = 0x70;  //マップ名のテキスト定義場所 
            map_config_pointer = 0x0195D8;       //マップ設定の開始位置(config)
            map_obj_pointer = 0x019640;          //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
            map_pal_pointer = 0x019674;          //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
            map_tileanime1_pointer = 0x030084;   //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_tileanime2_pointer = 0x030BC8;   //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_map_pointer_pointer = 0x034588;  //マップ設定の開始位置(map)
            map_mapchange_pointer = 0x0345B4;    //マップ設定の開始位置(mapchange)
            map_event_pointer = 0x0345E4;        //マップ設定の開始位置(event)
            map_worldmapevent_pointer = 0x0;  //マップ設定の開始位置(worldmap (FE6のみ))
            map_map_pointer_list_default_size = 0xEB;  //PLIST拡張をしていない時のバニラでのPLISTの数
            image_battle_animelist_pointer = U.FindROMPointer(rom, 0xC, new uint[] { 0x05A97C, 0x5B3A4, 0x5B438, 0x5B5D0, 0x71C58, 0x72EE8, 0x73008 });    // 戦闘アニメリストの開始位置
            support_unit_pointer = 0x8582E8;    // 支援相手の開始位置
            support_talk_pointer = 0x086A50;    // 支援相手の開始位置
            unit_palette_color_pointer = 0x582F4;   // ユニットのパレット(カラー)の開始位置
            unit_palette_class_pointer = 0xD1E90;   // ユニットのパレット(クラス)の開始位置
            support_attribute_pointer = 0x284C8;   //支援効果の開始位置
            terrain_recovery_pointer = 0x19F34;  //地形回復 全クラス共通
            terrain_bad_status_recovery_pointer = 0x019F44;  //地形回復 全クラス共通
            terrain_show_infomation_pointer = 0x8ED58;  //地形ウィンドウに情報表示 全クラス共通
            ballista_movcost_pointer = 0x18A74; //地形ウィンドウに情報表示 全クラス共通
            ccbranch_pointer = 0xD14EC;  // CC分岐の開始位置
            ccbranch2_pointer = 0xD14E4;  // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
            class_alphaname_pointer = 0xD18CC;  // クラスのアルファベット表記の開始位置
            map_terrain_name_pointer = 0x19f24;   // マップの地名表記の開始位置
            image_chapter_title_pointer = 0x8ba0c;  // 章タイトルの開始位置
            image_chapter_title_palette = 0xa99fa8;  // 章タイトルのパレット 多分違う
            image_unit_palette_pointer = 0x5b6cc;   // ユニットパレットの開始位置
            item_pointer = U.FindROMPointer(rom, (uint addr) => { return rom.u32(0xC) == 0x0; }, new uint[] { 0x17568, 0x17554, 0x1753C, 0x17510, 0x172BC });  // アイテムの開始位置

            item_datasize = 36;  // アイテムのデータサイズ
            item_effect_pointer = 0x7A664;  // アイテムエフェクトの開始位置
            sound_table_pointer = 0xD5024;  // ソングテーブルの開始位置
            sound_room_pointer = 0xB5044;  // サウンドルームの開始位置
            sound_room_datasize = 16;  // サウンドルームのデータサイズ
            sound_room_cg_pointer = 0x0;  // サウンドルームの背景リスト(FE7のみ)
            event_ballte_talk_pointer = 0x86978;  // 交戦時セリフの開始位置
            event_ballte_talk2_pointer = 0;  // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
            event_haiku_pointer = 0x869F8;  // 死亡時セリフの開始位置
            event_haiku_tutorial_1_pointer = 0x0;  // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_haiku_tutorial_2_pointer = 0x0;  // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_force_sortie_pointer = 0x86B08;  // 強制出撃の開始位置
            event_tutorial_pointer = 0x0;  //イベントチュートリアルポインタ FE7のみ
            map_exit_point_pointer = 0x3E83C;  // 離脱ポイント開始サイズ
            map_exit_point_npc_blockadd = 65;  // arr[+65] からNPCらしい.
            map_exit_point_blank = 0xDD1BC;  // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
            sound_boss_bgm_pointer = 0x74DE4;  // ボスBGMの開始位置
            sound_foot_steps_pointer = 0x7B1E8;  // クラス足音の開始位置
            sound_foot_steps_switch2_address = 0x7b1d6;
            sound_foot_steps_data_pointer = 0x7B440;    //足音のデータ構造の先頭
            worldmap_scroll_somedata_pointer = 0xC3314; //ワールドマップのスクロール関係のデータ
            worldmap_point_pointer = 0xC8DC0;  // ワールドマップ拠点の開始位置
            worldmap_bgm_pointer = 0xBEE28;  // ワールドマップのBGMテーブルの開始位置
            worldmap_icon_data_pointer = 0xC04A4;  // ワールドマップのアイコンデータのテーブルの開始位置
            worldmap_event_on_stageclear_pointer = 0xBF264;  // ワールドマップイベント 拠点クリア時
            worldmap_event_on_stageselect_pointer = 0xBF2B0;  // ワールドマップイベント 拠点選択時
            worldmap_county_border_pointer = 0xC792C;  // ワールドマップ国名の表示
            worldmap_county_border_palette_pointer = 0xC755C;  // ワールドマップ国名の表示のパレット
            item_shop_hensei_pointer = 0x9C144;  //編成準備店
            item_cornered_pointer = 0x2c704;  //すくみの開始位置
            ed_1_pointer = 0xBB224;   //ED 倒れたら撤退するかどうか
            ed_2_pointer = 0xBB200;  //ED 通り名
            ed_3a_pointer = 0xAC09E0;   //ED その後 エイルーク編　
            ed_3b_pointer = 0xAC09E4;   //ED その後 エフラム編
            ed_3c_pointer = 0x0;   //ED その後 FE7 リン編
            generic_enemy_portrait_pointer = 0x5E98;  //一般兵の顔
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

            cc_item_hero_crest_pointer = 0x29340;   //CCアイテム 英雄の証
            cc_item_knight_crest_pointer = 0x29348;   //CCアイテム 騎士の勲章
            cc_item_orion_bolt_pointer = 0x29350;   //CCアイテム オリオンの矢
            cc_elysian_whip_pointer = 0x29358;   //CCアイテム 天空のムチ
            cc_guiding_ring_pointer = 0x29360;   //CCアイテム 導きの指輪
            cc_fallen_contract_pointer = 0x29380;   //CCアイテム ダミー8A
            cc_master_seal_pointer = 0x29368;   //CCアイテム マスタープルフ
            cc_ocean_seal_pointer = 0x293B0;   //CCアイテム 覇者の証
            cc_moon_bracelet_pointer = 0x29370;   //CCアイテム 月の腕輪
            cc_sun_bracelet_pointer = 0x29378;   //CCアイテム 太陽の腕輪
            unit_increase_height_pointer = 0x5b3c;   //ステータス画面で背を伸ばす
            unit_increase_height_switch2_address = 0x5b2a; 
            op_class_demo_pointer = 0xAB0B18;   //OPクラスデモ
            op_class_font_pointer =  0xB7988;   //OPクラス日本語フォント
            op_class_font_palette_pointer = 0xb7994;   // OPクラス紹介フォントのパレット
            status_font_pointer = 0x49D0;   //ステータス画面用のフォント
            status_font_count = 0xA;   //ステータス画面用のフォントの数(英語版と日本語で数が違う)
            ed_staffroll_image_pointer = 0x1F6AE8;  // スタッフロール
            ed_staffroll_palette_pointer = 0xC9564;  // スタッフロールのパレット
            op_prologue_image_pointer = 0xC9A74;  // OP字幕
            op_prologue_palette_color_pointer = 0xC9564;  // OP字幕のパレット ???

            arena_class_near_weapon_pointer = 0x31898;  //闘技場 近接武器クラス 
            arena_class_far_weapon_pointer = 0x318a4;  // 闘技場 遠距離武器クラス
            arena_class_magic_weapon_pointer = 0x318F4;  // 闘技場 魔法武器クラス
            arena_enemy_weapon_basic_pointer = 0x31B04;  // 闘技場 敵武器テーブル基本武器
            arena_enemy_weapon_rankup_pointer = 0x31B24;  // 闘技場 敵武器テーブルランクアップ武器
            link_arena_deny_unit_pointer = 0x9A164;  //通信闘技場 禁止ユニット 
            worldmap_road_pointer = 0xC598;  // ワールドマップの道

            uint submenu_pointer = PatchUtil.SearchSubMenuMenuDefinePointerFE8J(rom);
            menu_definiton_pointer = U.FindROMPointer(rom, 8, new uint[] { submenu_pointer, 0x1BCBC, 0x1BF84, 0x1BD7C, 0x1BF70, 0x33398, 0x29940, 0x242AC, 0x1DC34 });  //メニュー定義
                    
            menu_promotion_pointer = 0xD2900;  //CC決定する選択子
            menu_promotion_branch_pointer = 0x0D2AC8;  //FE8にある分岐CCメニュー
            menu_definiton_split_pointer = 0x887DC;   //FE8にある分岐メニュー
            menu_definiton_worldmap_pointer = 0xC12CC;  //FE8のワールドマップのメニュー
            menu_definiton_worldmap_shop_pointer = 0xC142C;  //FE8のワールドマップ店のメニュー        
            menu_unit_pointer =  0x5c56d8;  // ユニットメニュー
            menu_game_pointer =  0x5C56FC;  // ゲームメニュー
            menu_debug1_pointer = 0x5C5498;   // デバッグメニュー
            menu_item_pointer = 0x5C5600;   // アイテム利用メニュー
            MenuCommand_UsabilityAlways = 0x0501BC;  //メニューを開くという値を返す関数のアドレス
            MenuCommand_UsabilityNever = 0x0501C4;  //メニューを開かないという値を返す関数のアドレス       
            status_rmenu_unit_pointer = 0x8ac64;  // ステータス RMENU1
            status_rmenu_game_pointer = 0x8ac6c;  // ステータス RMENU2
            status_rmenu3_pointer = 0x8ac84;  // ステータス RMENU3
            status_rmenu4_pointer = 0x37510;  // 戦闘予測 RMENU4
            status_rmenu5_pointer = 0x37528;  // 戦闘予測 RMENU5
            status_rmenu6_pointer = 0xA73DF8;  // 状況画面 RMENU6
            status_param1_pointer = 0x089454;  // ステータス PARAM1
            status_param2_pointer = 0x089758;  // ステータス PARAM2
            status_param3w_pointer = 0x089B54;  // ステータス PARAM3 武器
            status_param3m_pointer = 0x089B10;  // ステータス PARAM3 魔法

            systemmenu_common_image_pointer = 0x5E0650;  //システムメニューの画像
            systemmenu_common_palette_pointer = 0x036E2C;  //システムパレット 無圧縮4パレット
            systemmenu_goal_tsa_pointer = 0x08F57C;  //システムメニュー 目的表示TSA
            systemmenu_terrain_tsa_pointer = 0x08EE2C;  //システムメニュー 地形表示TSA

            systemmenu_name_image_pointer = 0x5E0650;  //システムメニュー 名前表示画像(FE8は共通画像)
            systemmenu_name_tsa_pointer = 0x08EA08;  //システムメニュー 名前表示TSA
            systemmenu_name_palette_pointer = 0x036E2C;  //システムメニュー 名前表示パレット

            systemmenu_battlepreview_image_pointer = 0x5E0650;  //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
            systemmenu_battlepreview_tsa_pointer = 0x0369A0;  //戦闘プレビューTSA
            systemmenu_battlepreview_palette_pointer = 0x036E2C;  //戦闘プレビューパレット

            systemarea_move_gradation_palette_pointer = 0x1d6b8;  //行動範囲
            systemarea_attack_gradation_palette_pointer = 0x1d6bc;  //攻撃範囲
            systemarea_staff_gradation_palette_pointer = 0x1d6c0;  //杖の範囲

            systemmenu_badstatus_image_pointer = 0x8E750;  //無圧縮のバッドステータス画像
            systemmenu_badstatus_palette_pointer = 0x8BBD0;  //バッドステータスのパレット
            systemmenu_badstatus_old_image_pointer = 0;  //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
            systemmenu_badstatus_old_palette_pointer = 0x0;  //昔の圧縮のバッドステータス画像のパレット FE7 FE6

            bigcg_pointer = 0xbb174;  // CG
            end_cg_address = 0x1F68CC;  // END CG FE8のみ
            worldmap_big_image_pointer = 0xBF698;  //ワールドマップ フィールドになるでかい奴  
            worldmap_big_palette_pointer = 0xBF6A4;  //ワールドマップ フィールドになるでかい奴 パレット  
            worldmap_big_dpalette_pointer = 0xC4594;  //ワールドマップ フィールドになるでかい奴 闇パレット  
            worldmap_big_palettemap_pointer = 0xBF69C;  //ワールドマップ フィールドになるでかい奴 パレットマップ
            worldmap_event_image_pointer = 0xc6dfc;  //ワールドマップ イベント用 
            worldmap_event_palette_pointer = 0xc6e00;  //ワールドマップ イベント用 パレット  
            worldmap_event_tsa_pointer = 0xc6e04;  //ワールドマップ イベント用 TSA
            worldmap_mini_image_pointer = 0xc8c24;  //ワールドマップ ミニマップ 
            worldmap_mini_palette_pointer = 0xc8c2c;  //ワールドマップ ミニマップ パレット  
            worldmap_icon_palette_pointer = 0xBDD10;  //ワールドアイコンと道パレット
            worldmap_icon1_pointer = 0xBDD14;  //ワールドマップ アイコン1
            worldmap_icon2_pointer = 0xBDD1C;  //ワールドマップ アイコン2
            worldmap_road_tile_pointer = 0xBDE60;  //ワールドマップ  道チップ
            map_load_function_pointer = 0xc1e90;  //マップチャプターに入ったときの処理(FE8のみ)
            map_load_function_switch1_address = 0xc1e7c; 
            system_icon_pointer = 0x156C8; //システム アイコン集
            system_icon_palette_pointer = 0x156d4; //システム アイコンパレット集
            system_icon_width_address = 0x156AC;  //システムアイコンの幅が書かれているアドレス
            system_weapon_icon_pointer = 0x9fefc; //剣　斧　弓などの武器属性アイコン集
            system_weapon_icon_palette_pointer = 0x93450; //剣　斧　弓などの武器属性アイコン集のパレット
            system_music_icon_pointer = 0x0B689C; //音楽関係のアイコン集
            system_music_icon_palette_pointer = 0x0B6890; //音楽関係のアイコン集のパレット
            weapon_rank_s_bonus_address = 0x2ACE4; //武器ランクSボーナス設定
            weapon_battle_flash_address = 0x59902; //神器 戦闘時フラッシュ
            weapon_effectiveness_2x3x_address = 0x2aa88; //神器 2倍 3倍特効
            font_item_address = 0x57994C; //アイテム名とかに使われるフォント 関数:080040b8 計算式とか0x08003f50
            font_serif_address =  0x593F74;  //セリフとかに使われるフォント
            monster_probability_pointer = 0x7A770;  //魔物発生確率
            monster_item_item_pointer = 0x7A814;  //魔物所持アイテム アイテム確率
            monster_item_probability_pointer = 0x7A810;  //魔物所持アイテム 所持確率
            monster_item_table_pointer = 0x7A784;  //魔物所持アイテム アイテムと所持を管理するテーブル
            monster_wmap_base_point_pointer = 0xC6694;  //ワールドマップに魔物を登場させる処理達
            monster_wmap_stage_1_pointer = 0xC657C; 
            monster_wmap_stage_2_pointer = 0xC65B4; 
            monster_wmap_probability_1_pointer = 0xC6580; 
            monster_wmap_probability_2_pointer = 0xC65B8; 
            monster_wmap_probability_after_1_pointer = 0xC65D0; 
            monster_wmap_probability_after_2_pointer = 0xC6690;
            worldmap_skirmish_startevent_pointer = 0x015430;
            worldmap_skirmish_endevent_pointer = 0x0855F8;
            battle_bg_pointer = 0x77e9c;  //戦闘背景
            battle_terrain_pointer = 0x52b40;  //戦闘地形
            senseki_comment_pointer = 0x0;  //戦績コメント FE8にはない
            unit_custom_battle_anime_pointer = 0x0;  //ユニット専用アニメ FE7にある
            magic_effect_pointer = 0x5C19C;  //魔法効果へのポインタ
            magic_effect_original_data_count = 0x48;  //もともとあった魔法数
     
            system_move_allowicon_pointer = 0x32db0; //移動するときの矢印アイコン
            system_move_allowicon_palette_pointer = 0x32db8; //移動するときの矢印アイコン アイコンパレット集
            system_tsa_16color_304x240_pointer = 0xB51B8;  //16色304x240 汎用TSAポインタ
            eventunit_data_size = 20;  //ユニット配置のデータサイズ
            eventcond_tern_size = 12;  //イベント条件 ターン条件のサイズ FE7->16 FE8->12
            eventcond_talk_size = 16;  //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
            oping_event_pointer = 0x907f78; 
            ending1_event_pointer = 0x9e20; 
            ending2_event_pointer = 0x9e38; 
            RAMSlotTable_address = 0x5C2A50; 
            supply_pointer_address = 0x31470;   //輸送隊RAMへのアドレス
            workmemory_player_units_address = 0x0202BE48;     //ワークメモリ PLAYER UNIT
            workmemory_enemy_units_address = 0x0202CFB8;     //ワークメモリ PLAYER UNIT
            workmemory_npc_units_address = 0x0202DDC8;     //ワークメモリ PLAYER UNIT
            workmemory_mapid_address = 0x0202BCFA;     //ワークメモリ マップID
            workmemory_chapterdata_address = workmemory_mapid_address - 0xE;  //ワークメモリ章データ
            workmemory_chapterdata_size = 0x4C;     //ワークメモリ 章データのサイズ
            workmemory_battle_actor_address = 0x0203a4e8;  //ワークメモリ 戦闘時のユニット構造体
            workmemory_battle_target_address = 0x0203a568;  //ワークメモリ 戦闘時のユニット構造体
            workmemory_worldmap_data_address = 0x03005270; //ワークメモリ ワールドマップ関係の起点
            workmemory_worldmap_data_size = 0xC4;  //ワークメモリ ワールドマップ関係のサイズ
            workmemory_arena_data_address = 0x0203A8EC; //ワークメモリ 闘技場関係の起点
            workmemory_ai_data_address = 0x0203AA00;  //ワークメモリ AI関係の起点
            workmemory_action_data_address = 0x0203A954;  //ワークメモリ ActionData
            workmemory_dungeon_data_address = 0x03001798;  //ワークメモリ ダンジョン FE8のみ
            workmemory_battlesome_data_address = 0x0203E0EC;  //ワークメモリ バルトに関係する諸データ
            workmemory_battleround_data_address = 0x0203A5E8;  //ワークメモリ　戦闘のラウンドデータ
            workmemory_last_string_address = 0x0202B6A8;   //ワークメモリ 最後に表示した文字列
            workmemory_text_buffer_address = 0x0202A6A8;   //ワークメモリ デコードされたテキスト
            workmemory_next_text_buffer_address = 0x03000040;   //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
            workmemory_local_flag_address = 0x03005260;   //ワークメモリ グローバルフラグ
            workmemory_global_flag_address = 0x03005240;   //ワークメモリ ローカルフラグ
            workmemory_trap_address = 0x0203A610;   //ワークメモリ ローカルフラグ
            workmemory_bwl_address = 0x0203E880;   //BWLデータ
            workmemory_clear_turn_address = 0x0203ECF0;  //ワークメモリ クリアターン数
            workmemory_clear_turn_count = 0x24;   //クリアターン数 最大数
            workmemory_memoryslot_address = 0x030004B0;   //ワークメモリ メモリスロットFE8
            workmemory_eventcounter_address = 0x03000560;   //イベントカウンター メモリスロットFE8
            workmemory_procs_forest_address = 0x02026A70;   //ワークメモリ Procs
            workmemory_procs_pool_address = 0x02024E68;   //ワークメモリ Procs
            function_sleep_handle_address = 0x080031DC + 1;   //ワークメモリ Procs待機中
            workmemory_user_stack_base_address = 0x03007DF0;  //ワークメモリ スタックの一番底
            function_fe_main_return_address = 0x08000AB8 + 1;  //スタックの一番底にある戻り先
            workmemory_control_unit_address = 0x03004df0;  //ワークメモリ 操作ユニット
            workmemory_bgm_address = 0x02024E5C;  //ワークメモリ BGM
            function_event_engine_loop_address = 0x0800D110 + 1;  //イベントエンジン
            workmemory_reference_procs_event_address_offset = 0x34;  //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
            workmemory_procs_game_main_address = 0x02024E68;  //ワークメモリ Procsの中でのGAMEMAIN
            workmemory_palette_address = 0x020228A8;  //RAMに記録されているダブルバッファのパレット領域
            workmemory_sound_player_00_address = 0x03006430;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_01_address = 0x03006640;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_02_address = 0x03006680;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_03_address = 0x030066C0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_04_address = 0x03006710;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_05_address = 0x03006750;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_06_address = 0x03006600;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_07_address = 0x030063F0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_08_address = 0x030063B0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_keybuffer_address = 0x02024CC0;  //RAMのキーバッファ
            procs_maptask_pointer = 0x30F24;  //PROCSのMAPTASK 
            procs_soundroomUI_pointer = 0xB4B44;  //PROCSのSoundRoomUI 
            procs_game_main_address = 0x85B946C;  //PROCSのGAME MAIN 
            summon_unit_pointer = 0x0243E0;  //召喚
            summons_demon_king_pointer = 0x07D6AC;  //呼魔
            summons_demon_king_count_address = 0x7D63C;  //呼魔リストの数
            mant_command_pointer = 0x6F3F4;  //マント
            mant_command_startadd = 0x6B;  //マント開始数
            mant_command_count_address = 0x6F3DA;  //マント数 アドレス
            unit_increase_height_yes = 0x08005b9c;   //ステータス画面で背を伸ばす 伸ばす
            unit_increase_height_no =  0x08005ba0;   //ステータス画面で背を伸ばす 伸ばさない
            battle_screen_TSA1_pointer = 0x529B4;   //戦闘画面
            battle_screen_TSA2_pointer = 0x529B8;   //戦闘画面
            battle_screen_TSA3_pointer = 0x520AC;   //戦闘画面
            battle_screen_TSA4_pointer = 0x520B4;   //戦闘画面
            battle_screen_TSA5_pointer = 0x52E68;   //戦闘画面
            battle_screen_palette_pointer = 0x52E70;   //戦闘画面 パレット
            battle_screen_image1_pointer = 0x52C60;   //戦闘画面 画像1
            battle_screen_image2_pointer = 0x52CC0;   //戦闘画面 画像2 左側名前
            battle_screen_image3_pointer = 0x52D20;   //戦闘画面 画像3 左側武器
            battle_screen_image4_pointer = 0x52D80;   //戦闘画面 画像4 右側名前
            battle_screen_image5_pointer = 0x52E5C;   //戦闘画面 画像5 右側武器
            ai1_pointer = 0x5D30EC;   //AI1ポインタ
            ai2_pointer = 0x5D30E0;   //AI2ポインタ
            ai3_pointer = 0x3E16C;   //AI3ポインタ
            ai_steal_item_pointer = 0x3B7C8;   //AI盗むAI アイテム評価テーブル 0x085D22AC
            ai_preform_staff_pointer = 0x3F9BC;   //AI杖 杖評価テーブル
            ai_preform_staff_direct_asm_pointer = 0x03FA60;   //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
            ai_preform_item_pointer = 0x407A0;  //AIアイテム アイテム評価テーブル
            ai_preform_item_direct_asm_pointer = 0x40848;   //AIアイテム アイテム評価テーブル
            ai_map_setting_pointer = 0x39784;   //AI 章ごとの設定テーブル 0x080DD214
            item_usability_array_pointer = 0x28858;  //アイテムを利用できるか判定する

            {
                uint jumpPointer = rom.u32(0x2884C);
                if (U.isSafetyPointer(jumpPointer, rom) && U.IsValueOdd(jumpPointer))
                {//FE8N SkillBook
                    uint switch_address = jumpPointer - 1 + (2 * 9);
                    item_usability_array_switch2_address = U.toOffset(switch_address);
                }
                else
                {
                    item_usability_array_switch2_address = 0x28846;
                }
            }

            
            item_effect_array_pointer = 0x2fbdc;     //アイテムを利用した場合の効果を定義する
            {
                uint jumpPointer = rom.u32(0x2FBC4);
                if (U.isSafetyPointer(jumpPointer, rom) && U.IsValueOdd(jumpPointer))
                {//FE8N SkillBook
                    uint switch_address = jumpPointer - 1 + (2 * 8);
                    item_effect_array_switch2_address = U.toOffset(switch_address);
                }
                else
                {
                    item_effect_array_switch2_address = 0x2fbc2; 
                }
            }
            item_promotion1_array_pointer = 0x291c0;    //CCアイテムを使った場合の処理を定義する
            item_promotion1_array_switch2_address = 0x291aa; 
            item_promotion2_array_pointer = 0x0;   //CCアイテムかどうかを定義する(FE7のみ)
            item_promotion2_array_switch2_address = 0x0; 
            item_staff1_array_pointer = 0x28e34;     //アイテムのターゲット選択の方法を定義する(多分)
            item_staff1_array_switch2_address = 0x28e22; 
            item_staff2_array_pointer = 0x74a6c;     //杖の種類を定義する
            item_staff2_array_switch2_address = 0x74a58; 
            item_statbooster1_array_pointer = 0x2f7dc;     //ドーピングアイテムを利用した時のメッセージを定義する
            item_statbooster1_array_switch2_address = 0x2f7ca; 
            item_statbooster2_array_pointer = 0x29ebc;     //ドーピングアイテムとCCアイテムかどうかを定義する
             
            {
                uint jumpPointer = rom.u32(0x29EAC);
                if (U.isSafetyPointer(jumpPointer, rom) && U.IsValueOdd(jumpPointer))
                {//FE8N SkillBook
                    uint switch_address = jumpPointer - 1 + (2 * 9);
                    item_statbooster2_array_switch2_address = U.toOffset(switch_address);
                }
                else
                {
                    item_statbooster2_array_switch2_address = 0x29ea8;
                }
            }
            
            item_errormessage_array_pointer = 0x28BD4;     //アイテム利用時のエラーメッセージ
            {
                uint jumpPointer = rom.u32(0x28BC8);
                if (U.isSafetyPointer(jumpPointer, rom) && U.IsValueOdd(jumpPointer))
                {//FE8N SkillBook
                    uint switch_address = jumpPointer - 1 + (2 * 9);
                    item_errormessage_array_switch2_address = U.toOffset(switch_address);
                }
                else
                {
                    item_errormessage_array_switch2_address = 0x28BC2;
                }
            }
            
            event_function_pointer_table_pointer = 0x0D1A4;     //イベント命令ポインタ
            event_function_pointer_table2_pointer = 0x0D1CC;    //イベント命令ポインタ2 ワールドマップ
            item_effect_pointer_table_pointer = 0x5C19C;    //間接エフェクトポインタ
            command_85_pointer_table_pointer = 0x074108;     //85Commandポインタ
            dic_main_pointer = 0xD4180;      //辞書メインポインタ
            dic_chaptor_pointer = 0xD2EF8;   //辞書章ポインタ
            dic_title_pointer = 0xD2F38;    //辞書タイトルポインタ
            itemicon_mine_id = 0x8c;   // アイテムアイコンのフレイボムの位置
            item_gold_id = 0x77;   // お金を取得するイベントに利用されるゴールドのID
            unitaction_function_pointer = 0x31FA8;   // ユニットアクションポインタ
            lookup_table_battle_terrain_00_pointer = 0x58D18;  //戦闘アニメの床
            lookup_table_battle_terrain_01_pointer = 0x58C6C;  //戦闘アニメの床
            lookup_table_battle_terrain_02_pointer = 0x58C74; //戦闘アニメの床
            lookup_table_battle_terrain_03_pointer = 0x58C7C;  //戦闘アニメの床
            lookup_table_battle_terrain_04_pointer = 0x58C84;  //戦闘アニメの床
            lookup_table_battle_terrain_05_pointer = 0x58C8C;  //戦闘アニメの床
            lookup_table_battle_terrain_06_pointer = 0x58C94;  //戦闘アニメの床
            lookup_table_battle_terrain_07_pointer = 0x58C9C;  //戦闘アニメの床
            lookup_table_battle_terrain_08_pointer = 0x58CA4;  //戦闘アニメの床
            lookup_table_battle_terrain_09_pointer = 0x58CAC;  //戦闘アニメの床
            lookup_table_battle_terrain_10_pointer = 0x58CB4;  //戦闘アニメの床
            lookup_table_battle_terrain_11_pointer = 0x58CBC;  //戦闘アニメの床
            lookup_table_battle_terrain_12_pointer = 0x58CC4;  //戦闘アニメの床
            lookup_table_battle_terrain_13_pointer = 0x58CCC;  //戦闘アニメの床
            lookup_table_battle_terrain_14_pointer = 0x58CD4;  //戦闘アニメの床
            lookup_table_battle_terrain_15_pointer = 0x58CDC;  //戦闘アニメの床
            lookup_table_battle_terrain_16_pointer = 0x58CE4;  //戦闘アニメの床
            lookup_table_battle_terrain_17_pointer = 0x58CEC;  //戦闘アニメの床
            lookup_table_battle_terrain_18_pointer = 0x58CF4;  //戦闘アニメの床
            lookup_table_battle_terrain_19_pointer = 0x58CFC;  //戦闘アニメの床
            lookup_table_battle_terrain_20_pointer = 0x58D04;  //戦闘アニメの床
            lookup_table_battle_bg_00_pointer = 0x58E40;  //戦闘アニメの背景
            lookup_table_battle_bg_01_pointer = 0x58D94;  //戦闘アニメの背景
            lookup_table_battle_bg_02_pointer = 0x58D9C;  //戦闘アニメの背景
            lookup_table_battle_bg_03_pointer = 0x58DA4;  //戦闘アニメの背景
            lookup_table_battle_bg_04_pointer = 0x58DAC;  //戦闘アニメの背景
            lookup_table_battle_bg_05_pointer = 0x58DB4;  //戦闘アニメの背景
            lookup_table_battle_bg_06_pointer = 0x58DBC;  //戦闘アニメの背景
            lookup_table_battle_bg_07_pointer = 0x58DC4;  //戦闘アニメの背景
            lookup_table_battle_bg_08_pointer = 0x58DCC;  //戦闘アニメの背景
            lookup_table_battle_bg_09_pointer = 0x58DD4;  //戦闘アニメの背景
            lookup_table_battle_bg_10_pointer = 0x58DDC;  //戦闘アニメの背景
            lookup_table_battle_bg_11_pointer = 0x58DE4;  //戦闘アニメの背景
            lookup_table_battle_bg_12_pointer = 0x58DEC;  //戦闘アニメの背景
            lookup_table_battle_bg_13_pointer = 0x58DF4;  //戦闘アニメの背景
            lookup_table_battle_bg_14_pointer = 0x58DFC;  //戦闘アニメの背景
            lookup_table_battle_bg_15_pointer = 0x58E04;  //戦闘アニメの背景
            lookup_table_battle_bg_16_pointer = 0x58E0C;  //戦闘アニメの背景
            lookup_table_battle_bg_17_pointer = 0x58E14;  //戦闘アニメの背景
            lookup_table_battle_bg_18_pointer = 0x58E1C;  //戦闘アニメの背景
            lookup_table_battle_bg_19_pointer = 0x58E24;  //戦闘アニメの背景
            lookup_table_battle_bg_20_pointer = 0x58E2C;  //戦闘アニメの背景
            map_terrain_type_count = 65;  //地形の種類の数
            menu_J12_always_address = 0x0501BC;  //メニューの表示判定関数 常に表示する
            menu_J12_hide_address = 0x0501C4;    //メニューの表示判定関数 表示しない
            status_game_option_pointer = 0xB638C;  //ゲームオプション
            status_game_option_order_pointer = 0xB6318;  //ゲームオプションの並び順
            status_game_option_order2_pointer = 0x0;  //ゲームオプションの並び順2 FE7のみ
            status_game_option_order_count_address = 0xB6652;  //ゲームオプションの個数
            status_units_menu_pointer = 0x94588;  //部隊メニュー
            tactician_affinity_pointer = 0x0;  //軍師属性(FE7のみ)
            event_final_serif_pointer = 0x0;  //終章セリフ(FE7のみ)
            compress_image_borderline_address = 0xDB000;  //これ以降に圧縮画像が登場するというアドレス
            builddate_address = 0xDC110; 
            Default_event_script_term_code = new byte[] { 0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード
            Default_event_script_toplevel_code = new byte[] { 0x28, 0x02, 0x07, 0x00, 0x20, 0x01, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード
            Default_event_script_mapterm_code = new byte[] { 0x20, 0x01, 0x00, 0x00 }; //ワールドマップイベント命令を終了させるディフォルトコード
            main_menu_width_address = 0x5C56F6;  //メインメニューの幅
            map_default_count = 0x4F;     // ディフォルトのマップ数
            wait_menu_command_id = 0x6B;  //WaitメニューのID
            font_default_begin = 0x579CDC; 
            font_default_end = 0x5B8CDC; 
            item_name_article_pointer = 0x0;  // a|an|the を切り替えるテーブル 英語版のみ
            item_name_article_switch2_address = 0x0; 
            vanilla_field_config_address = 0x19B198;     //バニラのタイルセット よく使われもの
            vanilla_field_image_address = 0x188888; 
            vanilla_village_config_address = 0x199C3C; 
            vanilla_village_image_address = 0x181610; 
            vanilla_casle_config_address = 0x1987C8; 
            vanilla_casle_image_address = 0x17B398; 
            vanilla_plain_config_address = 0x197170; 
            vanilla_plain_image_address = 0x174C50; 
            map_minimap_tile_array_pointer = 0xAC44C;  //minimapのチップ割り当て
            bg_reserve_black_bgid = 0x4f; 
            bg_reserve_random_bgid = 0x51;

            worldmap_node_armory_empty_address = 0x8AC2978;  // ワールドマップ拠点での武器屋のnullアドレス
            worldmap_node_vendor_empty_address = 0x8AC2A3C;  // ワールドマップ拠点での道具屋のnullアドレス
            worldmap_node_secret_empty_address = 0x8AC2B2E;  // ワールドマップ拠点での秘密の店のnullアドレス

            extends_address = 0x09000000;   //拡張領域
            orignal_crc32 = 0x9d76826f;  //無改造ROMのCRC32
            is_multibyte = true;     // マルチバイトを利用するか？
            version = 8;     // バージョン

            OverwriteROMConstants(rom);
        }

        override public uint patch_C01_hack(out uint enable_value) { enable_value = 0x47004800; return 0x5040; } //C01 patch
        override public uint patch_C48_hack(out uint enable_value) { enable_value = 0x0805A440; return 0x59B94; } //C48 patch
        override public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x00000010; return 0x02140BC; } //16_tracks_12_sounds patch
        override public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x0; } //章の名前をテキストにするパッチ
        override public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x5E70; } //一般兵の顔 拡張
        override public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x031F58; } //ユニットアクションの拡張
        override public uint patch_write_build_version(out uint enable_value) { enable_value = 0x47184b00; return 0xCA278; } //ビルドバージョンを書き込む
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
