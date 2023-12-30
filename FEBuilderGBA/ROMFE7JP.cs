using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE7JP : ROMFEINFO
    {
        public ROMFE7JP(ROM rom)
    	{
            VersionToFilename = "FE7J";
            TitleToFilename = "FE7";
            mask_point_base_pointer = 0x0006DC; // Huffman tree end (indirected twice)
            mask_pointer = 0x0006E0;  // Huffman tree start (indirected once)
            text_pointer = 0x13370; // textの開始位置
            text_recover_address = 0xBBB370; // textの開始位置(上記ポインタを壊している改造があるののでその対策)
            text_data_start_address = 0xB36950; // textデータの規定値の開始位置
            text_data_end_address = 0xBB72E0; // textデータの規定値の開始位置
            unit_pointer = 0x09aC50; // ユニットのの開始位置
            unit_maxcount = 253; // ユニットの最大数
            unit_datasize = 52; // ユニットのデータサイズ
            max_level_address = 0x29B42; // 最大レベルの値を格納しているアドレス
            max_luck_address = 0x29f0e; // 最大レベルの幸運の値を格納しているアドレス
            class_pointer = 0x017ce0; // クラスの開始位置
            class_datasize = 84;  // ユニットのデータサイズ
            bg_pointer = 0xb7b0; //BGベースアドレス
            portrait_pointer = 0x0069c0; //顔ベースアドレス
            portrait_datasize = 28;
            icon_pointer = 0x4cfc; // アイコンの開始位置
            icon_orignal_address = 0xC12F4; // アイコンの初期値
            icon_orignal_max = 0xAC; // アイコンの最大数

            icon_palette_pointer = 0x4c1c; // アイコンのパレットの開始位置
            unit_wait_icon_pointer = 0x02522c; // ユニット待機アイコンの開始位置
            unit_wait_barista_anime_address = 0x025CD0;  // ユニット待機アイコンのバリスタのアニメ指定アドレス
            unit_wait_barista_id = 0x52;  // ユニット待機アイコンのバリスタの位置
            unit_icon_palette_address = 0x1900E8; // ユニットのパレットの開始位置
            unit_icon_enemey_palette_address = 0x190108; // ユニット(敵軍)のパレットの開始位置
            unit_icon_npc_palette_address = 0x190128; // ユニット(友軍)のパレットの開始位置
            unit_icon_gray_palette_address = 0x190148; // ユニット(グレー))のパレットの開始位置
            unit_icon_four_palette_address = 0x190168; // ユニット(4軍))のパレットの開始位置
            unit_icon_lightrune_palette_address = 0x190188; // ユニット(光の結界)のパレットの開始位置
            unit_icon_sepia_palette_address = 0x1901A8; // ユニット(セピア)のパレットの開始位置

            unit_move_icon_pointer = 0x6DD60; // ユニット移動アイコンの開始位置
            lightrune_uniticon_id = 0x57; // ユニット(光の結界)のユニットアイコンのID
            map_setting_pointer = 0x31A6C;  // マップ設定の開始位置
            map_setting_datasize = 148; //マップ設定のデータサイズ
            map_setting_event_plist_pos = 116; //event plistの場所 
            map_setting_worldmap_plist_pos = 117; //woldmap event plistの場所 
            map_setting_clear_conditon_text_pos = 0x8A; //マップの右上に表示されているクリア条件の定義場所 
            map_setting_name_text_pos = 0x70; //マップ名のテキスト定義場所 
            map_config_pointer = 0x195B0;      //マップ設定の開始位置(config)
            map_obj_pointer = 0x019618;         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
            map_pal_pointer = 0x01964C;         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
            map_tileanime1_pointer = 0x02D824;  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_tileanime2_pointer = 0x02E364;  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_map_pointer_pointer = 0x031ABC; //マップ設定の開始位置(map)
            map_mapchange_pointer = 0x031AE8;   //マップ設定の開始位置(mapchange)
            map_event_pointer = 0x031B18;       //マップ設定の開始位置(event)
            map_worldmapevent_pointer = 0x0; //マップ設定の開始位置(worldmap (FE6のみ))
            map_map_pointer_list_default_size = 0xFC; //PLIST拡張をしていない時のバニラでのPLISTの数
            image_battle_animelist_pointer = 0x0549DC;   // 戦闘アニメリストの開始位置
            support_unit_pointer = 0xC4C184;   // 支援相手の開始位置
            support_talk_pointer = 0x79264;   // 支援相手の開始位置
            unit_palette_color_pointer = 0x0;  // ユニットのパレット(カラー)の開始位置
            unit_palette_class_pointer = 0x0;  // ユニットのパレット(クラス)の開始位置
            support_attribute_pointer = 0x26E24;  //支援効果の開始位置
            terrain_recovery_pointer = 0x19F0C; //地形回復 全クラス共通
            terrain_bad_status_recovery_pointer = 0x019F1C; //地形回復 全クラス共通
            terrain_show_infomation_pointer = 0x85F78; //地形ウィンドウに情報表示 全クラス共通
            ballista_movcost_pointer = 0x18BD8; //地形ウィンドウに情報表示 全クラス共通
            ccbranch_pointer = 0x0; // CC分岐の開始位置
            ccbranch2_pointer = 0x0; // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
            class_alphaname_pointer = 0x0; // クラスのアルファベット表記の開始位置
            map_terrain_name_pointer = 0x19efc; // マップの地名表記の開始位置
            image_chapter_title_pointer = 0x82e68; // 章タイトルの開始位置
            image_chapter_title_palette = 0x404f10; // 章タイトルのパレット 多分違う
            image_unit_palette_pointer = 0x549E0; // ユニットパレットの開始位置
            item_pointer = 0x164dc; // アイテムの開始位置
            item_datasize = 36; // アイテムのデータサイズ
            item_effect_pointer = 0x05330C; // アイテムエフェクトの開始位置
            sound_table_pointer = 0x3E2C; // ソングテーブルの開始位置
            sound_room_pointer = 0x1B844; // サウンドルームの開始位置
            sound_room_datasize = 16; // サウンドルームのデータサイズ
            sound_room_cg_pointer = 0xAC3A4; // サウンドルームの背景リスト(FE7のみ)
            event_ballte_talk_pointer = 0x79ab0; // 交戦時セリフの開始位置
            event_ballte_talk2_pointer = 0x79c24; // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
            event_haiku_pointer = 0x79d20; // 死亡時セリフの開始位置
            event_haiku_tutorial_1_pointer = 0x79D2C; // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_haiku_tutorial_2_pointer = 0x79D28; // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_force_sortie_pointer = 0x8e6f8; // 強制出撃の開始位置
            event_tutorial_pointer = 0x797D0; //イベントチュートリアルポインタ FE7のみ
            map_exit_point_pointer = 0x39E24; // 離脱ポイント開始サイズ
            map_exit_point_npc_blockadd = 0x30; // arr[+0x30] からNPCらしい.
            map_exit_point_blank = 0x1D92F0; // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
            sound_boss_bgm_pointer = 0x68934; // ボスBGMの開始位置
            sound_foot_steps_pointer = 0x0; // クラス足音の開始位置
            sound_foot_steps_switch2_address = 0x0;
            worldmap_point_pointer = 0x0; // ワールドマップ拠点の開始位置
            worldmap_bgm_pointer = 0x0; // ワールドマップのBGMテーブルの開始位置
            worldmap_icon_data_pointer = 0; // ワールドマップのアイコンデータのテーブルの開始位置
            worldmap_event_on_stageclear_pointer = 0x0; // ワールドマップイベントクリア時
            worldmap_event_on_stageselect_pointer = 0xb643c; // ワールドマップイベント 拠点選択時
            worldmap_county_border_pointer = 0; // ワールドマップ国名の表示
            worldmap_county_border_palette_pointer = 0x0; // ワールドマップ国名の表示のパレット
            item_shop_hensei_pointer = 0x0; //編成準備店
            item_cornered_pointer = 0x2A62C; //すくみの開始位置
            ed_1_pointer = 0xB8C40;  //ED 倒れたら撤退するかどうか
            ed_2_pointer = 0xB8C1C;  //ED 通り名
            ed_3a_pointer = 0xdc2894;  //ED その後 エリウッド編　
            ed_3b_pointer = 0xdc2898;  //ED その後 ヘクトル編
            ed_3c_pointer = 0xDC1FBC;  //ED その後 FE7 リン編(ポインタ指定できない)
            generic_enemy_portrait_pointer = 0x7450; //一般兵の顔
            generic_enemy_portrait_count = 0x8-2; //一般兵の顔の個数

            cc_item_hero_crest_itemid = 0x63;  //CCアイテム 英雄の証
            cc_item_knight_crest_itemid = 0x64;  //CCアイテム 騎士の勲章
            cc_item_orion_bolt_itemid = 0x65;  //CCアイテム オリオンの矢
            cc_elysian_whip_itemid = 0x66;  //CCアイテム 天空のムチ
            cc_guiding_ring_itemid = 0x67;  //CCアイテム 導きの指輪
            cc_fallen_contract_itemid = 0x96;  //CCアイテム ダミー8A(FE7では 覇者の証)
            cc_master_seal_itemid = 0x87;  //CCアイテム マスタープルフ
            cc_ocean_seal_itemid = 0x8B;  //CCアイテム 覇者の証(FE7では 闇の契約書)
            cc_moon_bracelet_itemid = 0x87;  //CCアイテム 月の腕輪
            cc_sun_bracelet_itemid = 0x89;  //CCアイテム 太陽の腕輪

            cc_item_hero_crest_pointer = 0x2798c;  //CCアイテム 英雄の証
            cc_item_knight_crest_pointer = 0x27994;  //CCアイテム 騎士の勲章
            cc_item_orion_bolt_pointer = 0x2799c;  //CCアイテム オリオンの矢
            cc_elysian_whip_pointer = 0x279a4;  //CCアイテム 天空のムチ
            cc_guiding_ring_pointer = 0x279ac;  //CCアイテム 導きの指輪
            cc_fallen_contract_pointer = 0x27a00;  //CCアイテム ダミー8A 闇の契約書
            cc_master_seal_pointer = 0x279b4;  //CCアイテム マスタープルフ
            cc_ocean_seal_pointer = 0x279d8;  //CCアイテム 覇者の証
            cc_moon_bracelet_pointer = 0x279cc;  //CCアイテム 月の腕輪 天の刻印
            cc_sun_bracelet_pointer = 0x279d0;  //CCアイテム 太陽の腕輪 天の刻印
            unit_increase_height_pointer = 0x70bc;  //ステータス画面で背を伸ばす
            unit_increase_height_switch2_address = 0x70ac;
            op_class_demo_pointer = 0xdb01e8; //OPクラスデモ
            op_class_font_pointer = 0x0;  //OPクラス日本語フォント
            op_class_font_palette_pointer = 0x0;  // OPクラス紹介フォントのパレット
            status_font_pointer = 0x5FA8;  //ステータス画面用のフォント
            status_font_count = 0xA;  //ステータス画面用のフォントの数(英語版と日本語で数が違う)
            ed_staffroll_image_pointer = 0x0; // スタッフロール
            ed_staffroll_palette_pointer = 0x0; // スタッフロールのパレット
            op_prologue_image_pointer = 0x0; // OP字幕
            op_prologue_palette_color_pointer = 0x0; // OP字幕のパレット ???

            arena_class_near_weapon_pointer = 0x2f024; //闘技場 近接武器クラス 
            arena_class_far_weapon_pointer = 0x2f030; // 闘技場 遠距離武器クラス
            arena_class_magic_weapon_pointer = 0x2F080; // 闘技場 魔法武器クラス
            arena_enemy_weapon_basic_pointer = 0x2F288; // 闘技場 敵武器テーブル基本武器
            arena_enemy_weapon_rankup_pointer = 0x2F2A8; // 闘技場 敵武器テーブルランクアップ武器
            link_arena_deny_unit_pointer = 0; //通信闘技場 禁止ユニット 
            worldmap_road_pointer = 0x0; // ワールドマップの道

            menu_definiton_pointer = 0x1BC80; //メニュー定義
            menu_promotion_pointer = 0x0; //CC決定する選択子
            menu_promotion_branch_pointer = 0x0; //FE8にある分岐CCメニュー
            menu_definiton_split_pointer = 0x0;  //FE8にある分岐メニュー
            menu_definiton_worldmap_pointer = 0x0; //FE8のワールドマップのメニュー
            menu_definiton_worldmap_shop_pointer = 0x0; //FE8のワールドマップ店のメニュー        
            menu_unit_pointer =  0xc04d70; // ユニットメニュー
            menu_game_pointer =  0xC04DB8; // ゲームメニュー
            menu_debug1_pointer = 0xC04B0C;  // デバッグメニュー
            menu_item_pointer = 0xC04C98;  // アイテム利用メニュー
            MenuCommand_UsabilityAlways = 0x04B0D4; //メニューを開くという値を返す関数のアドレス
            MenuCommand_UsabilityNever = 0x04B0DC; //メニューを開かないという値を返す関数のアドレス       
            status_rmenu_unit_pointer = 0x82288; // ステータス RMENU1
            status_rmenu_game_pointer = 0x82290; // ステータス RMENU2
            status_rmenu3_pointer = 0x822a8; // ステータス RMENU3
            status_rmenu4_pointer = 0x34700; // 戦闘予測 RMENU4
            status_rmenu5_pointer = 0x34718; // 戦闘予測 RMENU5
            status_rmenu6_pointer = 0x0; // 状況画面 RMENU6
            status_param1_pointer = 0x80bb4; // ステータス PARAM1
            status_param2_pointer = 0x80e80; // ステータス PARAM2
            status_param3w_pointer = 0x81220; // ステータス PARAM3 武器
            status_param3m_pointer = 0x811dc; // ステータス PARAM3 魔法

            systemmenu_common_image_pointer = 0x0865E8; //システムメニューの画像
            systemmenu_common_palette_pointer = 0x081EF0; //システムパレット 無圧縮4パレット
            systemmenu_goal_tsa_pointer = 0x0867D4; //システムメニュー 目的表示TSA
            systemmenu_terrain_tsa_pointer = 0x086040; //システムメニュー 地形表示TSA

            systemmenu_name_image_pointer = 0x0865E8; //システムメニュー 名前表示画像(FE8は共通画像)
            systemmenu_name_tsa_pointer = 0x085C48; //システムメニュー 名前表示TSA
            systemmenu_name_palette_pointer = 0x085844; //システムメニュー 名前表示パレット

            systemmenu_battlepreview_image_pointer = 0x0340E4; //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
            systemmenu_battlepreview_tsa_pointer = 0x033BC0; //戦闘プレビューTSA
            systemmenu_battlepreview_palette_pointer = 0x034004; //戦闘プレビューパレット

            systemarea_move_gradation_palette_pointer = 0x01D660; //行動範囲
            systemarea_attack_gradation_palette_pointer = 0x01D664; //攻撃範囲
            systemarea_staff_gradation_palette_pointer = 0x01D668; //杖の範囲

            systemmenu_badstatus_image_pointer = 0x85958; //無圧縮のバッドステータス画像
            systemmenu_badstatus_palette_pointer = 0x82FFC; //バッドステータスのパレット
            systemmenu_badstatus_old_image_pointer = 0x865E8; //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
            systemmenu_badstatus_old_palette_pointer = 0x9CAEC; //昔の圧縮のバッドステータス画像のパレット FE7 FE6

            bigcg_pointer = 0xb7978; // CG
            end_cg_address = 0x0; // END CG FE8のみ
            worldmap_big_image_pointer = 0xB6A14; //ワールドマップ フィールドになるでかい奴  
            worldmap_big_palette_pointer = 0xB6C00; //ワールドマップ フィールドになるでかい奴 パレット  
            worldmap_big_dpalette_pointer = 0x0; //ワールドマップ フィールドになるでかい奴 闇パレット  
            worldmap_big_palettemap_pointer = 0xB6A0C; //ワールドマップ フィールドになるでかい奴 パレットマップ
            worldmap_event_image_pointer = 0x0B6C2C; //ワールドマップ イベント用 
            worldmap_event_palette_pointer = 0xB6C28; //ワールドマップ イベント用 パレット  
            worldmap_event_tsa_pointer = 0xB6C38; //ワールドマップ イベント用 TSA
            worldmap_mini_image_pointer = 0x0; //ワールドマップ ミニマップ 
            worldmap_mini_palette_pointer = 0x0; //ワールドマップ ミニマップ パレット  
            worldmap_icon_palette_pointer = 0x0; //ワールドアイコンと道パレット
            worldmap_icon1_pointer = 0x0; //ワールドマップ アイコン1
            worldmap_icon2_pointer = 0x0; //ワールドマップ アイコン2
            worldmap_road_tile_pointer = 0x0; //ワールドマップ  道チップ
            map_load_function_pointer = 0x0; //マップチャプターに入ったときの処理(FE8のみ)
            map_load_function_switch1_address = 0x0;
            system_icon_pointer = 0x15A38;//システム アイコン集
            system_icon_palette_pointer = 0x15A44;//システム アイコンパレット集
            system_icon_width_address = 0x15A1C; //システムアイコンの幅が書かれているアドレス
            system_weapon_icon_pointer = 0x96d78;//剣　斧　弓などの武器属性アイコン集
            system_weapon_icon_palette_pointer = 0x90E70;//剣　斧　弓などの武器属性アイコン集のパレット
            system_music_icon_pointer = 0x0AF0F4;//音楽関係のアイコン集
            system_music_icon_palette_pointer = 0x0AF0E8;//音楽関係のアイコン集のパレット
            weapon_rank_s_bonus_address = 0x29348;//武器ランクSボーナス設定
            weapon_battle_flash_address = 0x53ba6;//神器 戦闘時フラッシュ
            weapon_effectiveness_2x3x_address = 0;//神器 2倍 3倍特効
            font_item_address = 0xBC1FEC;//アイテム名とかに使われるフォント
            font_serif_address = 0xBDC1E0; //セリフとかに使われるフォント
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
            battle_bg_pointer = 0x06b790; //戦闘背景
            battle_terrain_pointer = 0x4d8dc; //戦闘地形
            senseki_comment_pointer = 0x9b21c; //戦績コメント
            unit_custom_battle_anime_pointer = 0x530CC; //ユニット専用アニメ FE7にある
            magic_effect_pointer = 0x5609C; //魔法効果へのポインタ
            magic_effect_original_data_count = 0x03e; //もともとあった魔法数
            system_move_allowicon_pointer = 0x3042c;//移動するときの矢印アイコン
            system_move_allowicon_palette_pointer = 0x30434; //移動するときの矢印アイコン アイコンパレット集
            system_tsa_16color_304x240_pointer = 0xC0082C; //16色304x240 汎用TSAポインタ
            eventunit_data_size = 16; //ユニット配置のデータサイズ
            eventcond_tern_size = 16; //イベント条件 ターン条件のサイズ FE7->16 FE8->12
            eventcond_talk_size = 16; //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
            oping_event_pointer = 0xd68514;
            ending1_event_pointer = 0x1314C;
            ending2_event_pointer = 0x13194;
            RAMSlotTable_address = 0xC0216C;
            supply_pointer_address = 0x2EBF0;  //輸送隊RAMへのアドレス
            workmemory_player_units_address = 0x0202BD4C;    //ワークメモリ PLAYER UNIT
            workmemory_enemy_units_address = 0x0202CEBC;    //ワークメモリ PLAYER UNIT
            workmemory_npc_units_address = 0x0202DCCC;    //ワークメモリ PLAYER UNIT
            workmemory_mapid_address = 0x0202BC02;    //ワークメモリ マップID
            workmemory_chapterdata_address = workmemory_mapid_address - 0xE; //ワークメモリ章データ
            workmemory_chapterdata_size = 0x4C;    //ワークメモリ 章データのサイズ
            workmemory_battle_actor_address = 0x0203A3EC; //ワークメモリ 戦闘時のユニット構造体
            workmemory_battle_target_address = 0x0203A46C; //ワークメモリ 戦闘時のユニット構造体
            workmemory_worldmap_data_address = 0x0;//ワークメモリ ワールドマップ関係の起点
            workmemory_worldmap_data_size = 0x0; //ワークメモリ ワールドマップ関係のサイズ
            workmemory_arena_data_address = 0x0203A7F0;//ワークメモリ 闘技場関係の起点
            workmemory_ai_data_address = 0x0203A8E8; //ワークメモリ AI関係の起点
            workmemory_action_data_address = 0x0203A858; //ワークメモリ ActionData
            workmemory_dungeon_data_address = 0x0; //ワークメモリ ダンジョン FE8のみ
            workmemory_battlesome_data_address = 0x0203DFD4; //ワークメモリ バルトに関係する諸データ
            workmemory_battleround_data_address = 0x0203A508; //ワークメモリ　戦闘のラウンドデータ
            workmemory_last_string_address = 0x0202B5B0;  //ワークメモリ 最後に表示した文字列
            workmemory_text_buffer_address = 0x0202A5B0;  //ワークメモリ デコードされたテキスト
            workmemory_next_text_buffer_address = 0x03000040;  //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
            workmemory_local_flag_address = 0x030049F8;  //ワークメモリ グローバルフラグ
            workmemory_global_flag_address = 0x030049F0;  //ワークメモリ ローカルフラグ
            workmemory_trap_address = 0x0203A514;  //ワークメモリ ローカルフラグ
            workmemory_bwl_address = 0x0203E768;  //BWLデータ
            workmemory_clear_turn_address = 0x0203EBD8; //ワークメモリ クリアターン数
            workmemory_clear_turn_count = 0x30;  //クリアターン数 最大数
            workmemory_memoryslot_address = 0;  //ワークメモリ メモリスロットFE8
            workmemory_eventcounter_address = 0x0;  //イベントカウンター メモリスロットFE8
            workmemory_procs_forest_address = 0x02026A28;  //ワークメモリ Procs
            workmemory_procs_pool_address = 0x02024E20;  //ワークメモリ Procs
            function_sleep_handle_address = 0x08004960 + 1;  //ワークメモリ Procs待機中
            workmemory_user_stack_base_address = 0x03007DE0; //ワークメモリ スタックの一番底
            function_fe_main_return_address = 0x08000AC6 + 1; //スタックの一番底にある戻り先
            workmemory_control_unit_address = 0x030045B0; //ワークメモリ 操作ユニット
            workmemory_bgm_address = 0x02024E14; //ワークメモリ BGM
            function_event_engine_loop_address = 0x0800B2CC + 1; //イベントエンジン
            workmemory_reference_procs_event_address_offset = 0x2C; //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
            workmemory_procs_game_main_address = 0x02024E20; //ワークメモリ Procsの中でのGAMEMAIN
            workmemory_palette_address = 0x02022860; //RAMに記録されているダブルバッファのパレット領域
            workmemory_sound_player_00_address = 0x03005A30; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_01_address = 0x03005C40; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_02_address = 0x03005C80; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_03_address = 0x03005D50; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_04_address = 0x03005CC0; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_05_address = 0x030059B0; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_06_address = 0x030059F0; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_07_address = 0x03005C00; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_08_address = 0x03005D10; //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_keybuffer_address = 0x02024CC0; //RAMのキーバッファ
            procs_maptask_pointer = 0x2E644; //PROCSのMAPTASK 
            procs_soundroomUI_pointer = 0xAD1A0; //PROCSのSoundRoomUI 
            procs_game_main_address = 0x8C01744; //PROCSのGAME MAIN 
            summon_unit_pointer = 0; //召喚
            summons_demon_king_pointer = 0; //呼魔
            summons_demon_king_count_address = 0; //呼魔リストの数
            mant_command_pointer = 0x63FEC; //マント
            mant_command_startadd = 0x58; //マント開始数
            mant_command_count_address = 0x63FD8; //マント数 アドレス
            unit_increase_height_yes = 0x08007158;  //ステータス画面で背を伸ばす 伸ばす
            unit_increase_height_no =  0x0800715c;  //ステータス画面で背を伸ばす 伸ばさない
            battle_screen_TSA1_pointer = 0x04D730;  //戦闘画面
            battle_screen_TSA2_pointer = 0x04D734;  //戦闘画面
            battle_screen_TSA3_pointer = 0x04CE98;  //戦闘画面
            battle_screen_TSA4_pointer = 0x04CEA0;  //戦闘画面
            battle_screen_TSA5_pointer = 0x04DC04;  //戦闘画面
            battle_screen_palette_pointer = 0x04DC0C;  //戦闘画面 パレット
            battle_screen_image1_pointer = 0x04D9FC;  //戦闘画面 画像1
            battle_screen_image2_pointer = 0x04DA5C;  //戦闘画面 画像2
            battle_screen_image3_pointer = 0x04DABC;  //戦闘画面 画像3
            battle_screen_image4_pointer = 0x04DA5C;  //戦闘画面 画像4
            battle_screen_image5_pointer = 0x04DABC;  //戦闘画面 画像5
            ai1_pointer = 0xC07CB0;  //AI1ポインタ
            ai2_pointer = 0xC07CA4;  //AI2ポインタ
            ai3_pointer = 0x039758;  //AI3ポインタ
            ai_steal_item_pointer = 0x36D90;  //AI盗むAI アイテム評価テーブル 0x08C06564
            ai_preform_staff_pointer = 0x03AF88;  //AI杖 杖評価テーブル
            ai_preform_staff_direct_asm_pointer = 0x3B02C;  //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
            ai_preform_item_pointer = 0x03BEA8; //AIアイテム アイテム評価テーブル
            ai_preform_item_direct_asm_pointer = 0x3BF48;  //AIアイテム アイテム評価テーブル
            ai_map_setting_pointer = 0x34E18;  //AI 章ごとの設定テーブル 0x081D92F4
            item_usability_array_pointer = 0x27194; //アイテムを利用できるか判定する
            item_usability_array_switch2_address = 0x27182;
            item_effect_array_pointer = 0x2d508;    //アイテムを利用した場合の効果を定義する
            item_effect_array_switch2_address = 0x2d4ee;
            item_promotion1_array_pointer = 0x278b4;   //CCアイテムを使った場合の処理を定義する
            item_promotion1_array_switch2_address = 0x278a2;
            item_promotion2_array_pointer = 0x95eb0;  //CCアイテムかどうかを定義する(FE7のみ)
            item_promotion2_array_switch2_address = 0x95ea0;
            item_staff1_array_pointer = 0x275ac;    //アイテムのターゲット選択の方法を定義する(多分)
            item_staff1_array_switch2_address = 0x2759a;
            item_staff2_array_pointer = 0x685e4;    //杖の種類を定義する
            item_staff2_array_switch2_address = 0x685D2;
            item_statbooster1_array_pointer = 0x2d29c;    //ドーピングアイテムを利用した時のメッセージを定義する
            item_statbooster1_array_switch2_address = 0x2D28A;
            item_statbooster2_array_pointer = 0x284f8;    //ドーピングアイテムとCCアイテムかどうかを定義する
            item_statbooster2_array_switch2_address = 0x284e4;
            item_errormessage_array_pointer = 0x273F0;    //アイテム利用時のエラーメッセージ
            item_errormessage_array_switch2_address = 0x273DE;
            event_function_pointer_table_pointer = 0xD63C;    //イベント命令ポインタ
            event_function_pointer_table2_pointer = 0x0;   //イベント命令ポインタ2 ワールドマップ
            item_effect_pointer_table_pointer = 0x05609C;   //間接エフェクトポインタ
            command_85_pointer_table_pointer = 0x06804C;    //85Commandポインタ
            dic_main_pointer = 0x0;     //辞書メインポインタ
            dic_chaptor_pointer = 0x0;  //辞書章ポインタ
            dic_title_pointer = 0x0;   //辞書タイトルポインタ
            itemicon_mine_id = 0x8c;  // アイテムアイコンのフレイボムの位置
            item_gold_id = 0x76;  // お金を取得するイベントに利用されるゴールドのID
            unitaction_function_pointer = 0x2F710;  // ユニットアクションポインタ
            lookup_table_battle_terrain_00_pointer = 0x53214; //戦闘アニメの床
            lookup_table_battle_terrain_01_pointer = 0x53198; //戦闘アニメの床
            lookup_table_battle_terrain_02_pointer = 0x531A0;//戦闘アニメの床
            lookup_table_battle_terrain_03_pointer = 0x531A8; //戦闘アニメの床
            lookup_table_battle_terrain_04_pointer = 0x531B0; //戦闘アニメの床
            lookup_table_battle_terrain_05_pointer = 0x531B8; //戦闘アニメの床
            lookup_table_battle_terrain_06_pointer = 0x531C0; //戦闘アニメの床
            lookup_table_battle_terrain_07_pointer = 0x531C8; //戦闘アニメの床
            lookup_table_battle_terrain_08_pointer = 0x531D0; //戦闘アニメの床
            lookup_table_battle_terrain_09_pointer = 0x531D8; //戦闘アニメの床
            lookup_table_battle_terrain_10_pointer = 0x531E0; //戦闘アニメの床
            lookup_table_battle_terrain_11_pointer = 0x531E8; //戦闘アニメの床
            lookup_table_battle_terrain_12_pointer = 0x531F0; //戦闘アニメの床
            lookup_table_battle_terrain_13_pointer = 0x531F8; //戦闘アニメの床
            lookup_table_battle_terrain_14_pointer = 0x53200; //戦闘アニメの床
            lookup_table_battle_terrain_15_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_16_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_17_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_18_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_19_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_terrain_20_pointer = 0x00; //戦闘アニメの床
            lookup_table_battle_bg_00_pointer = 0x532EC; //戦闘アニメの背景
            lookup_table_battle_bg_01_pointer = 0x53274; //戦闘アニメの背景
            lookup_table_battle_bg_02_pointer = 0x5327C; //戦闘アニメの背景
            lookup_table_battle_bg_03_pointer = 0x53284; //戦闘アニメの背景
            lookup_table_battle_bg_04_pointer = 0x5328C; //戦闘アニメの背景
            lookup_table_battle_bg_05_pointer = 0x53294; //戦闘アニメの背景
            lookup_table_battle_bg_06_pointer = 0x5329C; //戦闘アニメの背景
            lookup_table_battle_bg_07_pointer = 0x532A4; //戦闘アニメの背景
            lookup_table_battle_bg_08_pointer = 0x532AC; //戦闘アニメの背景
            lookup_table_battle_bg_09_pointer = 0x532B4; //戦闘アニメの背景
            lookup_table_battle_bg_10_pointer = 0x532BC; //戦闘アニメの背景
            lookup_table_battle_bg_11_pointer = 0x532C4; //戦闘アニメの背景
            lookup_table_battle_bg_12_pointer = 0x532CC; //戦闘アニメの背背景
            lookup_table_battle_bg_13_pointer = 0x532D4; //戦闘アニメの背景
            lookup_table_battle_bg_14_pointer = 0x532DC; //戦闘アニメの背景
            lookup_table_battle_bg_15_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_16_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_17_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_18_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_19_pointer = 0x00; //戦闘アニメの背景
            lookup_table_battle_bg_20_pointer = 0x00; //戦闘アニメの背景
            map_terrain_type_count = 65; //地形の種類の数
            menu_J12_always_address = 0x4B0D4; //メニューの表示判定関数 常に表示する
            menu_J12_hide_address = 0x4B0DC;   //メニューの表示判定関数 表示しない
            status_game_option_pointer = 0xAEBC4; //ゲームオプション
            status_game_option_order_pointer = 0xDAF058; //ゲームオプションの並び順
            status_game_option_order2_pointer = 0xDAF060; //ゲームオプションの並び順2 FE7のみ
            status_game_option_order_count_address = 0xDAF054; //ゲームオプションの個数
            status_units_menu_pointer = 0x8B5DC; //部隊メニュー
            tactician_affinity_pointer = 0x1C3F4; //軍師属性(FE7のみ)
            event_final_serif_pointer = 0x7E9F8; //終章セリフ(FE7のみ)
            compress_image_borderline_address = 0xC7334; //これ以降に圧縮画像が登場するというアドレス
            Default_event_script_term_code = new byte[] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード
            Default_event_script_toplevel_code = new byte[] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード(各章のトップレベルのイベント)
            Default_event_script_mapterm_code = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //ワールドマップイベント命令を終了させるディフォルトコード
            main_menu_width_address = 0xC04DB2; //メインメニューの幅
            map_default_count = 0x45;    // ディフォルトのマップ数
            wait_menu_command_id = 0x67; //WaitメニューのID
            font_default_begin = 0xBC237C;
            font_default_end = 0xBFF760;
            item_name_article_pointer = 0x0; // a|an|the を切り替えるテーブル 英語版のみ
            item_name_article_switch2_address = 0x0;
            vanilla_field_config_address = 0x35FC08;    //バニラのタイルセット よく使われもの
            vanilla_field_image_address = 0x371394;
            vanilla_village_config_address = 0x362308;
            vanilla_village_image_address = 0x344F94;
            vanilla_casle_config_address = 0x36381C;
            vanilla_casle_image_address = 0x34C1BC;
            vanilla_plain_config_address = 0x364ADC;
            vanilla_plain_image_address = 0x352418;
            map_minimap_tile_array_pointer = 0xA30F0; //minimapのチップ割り当て
            bg_reserve_black_bgid = 0x5b;
            bg_reserve_random_bgid = U.NOT_FOUND;
            extends_address = 0x09000000;  //拡張領域
            orignal_crc32 = 0xf0c10e72; //無改造ROMのCRC32
            is_multibyte = true;    // マルチバイトを利用するか？
            version = 7;    // バージョン

            OverwriteROMConstants(rom);
        }

        override public uint patch_C01_hack(out uint enable_value) { enable_value = 0xF0C046C0; return 0x06642; } //C01 patch
        override public uint patch_C48_hack(out uint enable_value) { enable_value = 0x080C6970; return 0x53DD4; } //C48 patch
        override public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x0000000C; return 0x06EA860; } //16_tracks_12_sounds patch
        override public uint patch_chaptor_names_text_fix(out uint enable_value) { enable_value = 0x0; return 0x0; } //章の名前をテキストにするパッチ
        override public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x7428; } //一般兵の顔 拡張
        override public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x02F6E4; } //ユニットアクションの拡張
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
