using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE6JP : ROMFEINFO
    {
        public ROMFE6JP(ROM rom)
    	{
    		VersionToFilename = "FE6";
    		TitleToFilename = "FE6";
    		
            mask_point_base_pointer = 0x0006DC; // Huffman tree end (indirected twice)
            mask_pointer = 0x0006E0;  // Huffman tree start (indirected once)
            text_pointer = 0x13B10; // textの開始位置
            text_recover_address = 0xF635C; // textの開始位置(上記ポインタを壊している改造があるののでその対策)
            text_data_start_address = 0x9FAC0; // textデータの規定値の開始位置
            text_data_end_address = 0xF9790; // textデータの規定値の開始位置
            unit_pointer = 0x17680; // ユニットの開始位置
            unit_maxcount = 226; // ユニットの最大数
            unit_datasize = 48; // ユニットのデータサイズ
            max_level_address = 0x25132; // 最大レベルの値を格納しているアドレス(たぶん)
            max_luck_address = 0x254ba; // 最大レベルの幸運の値を格納しているアドレス
            class_pointer = 0x176E0; // クラスの開始位置
            class_datasize = 72;  // クラスのデータサイズ
            bg_pointer = 0x0E374; //BGベースアドレス
            portrait_pointer = 0x7fd8; //顔ベースアドレス
            portrait_datasize = 16;  
            icon_pointer = 0x4cec; // アイコンの開始位置
            icon_orignal_address = 0xF9D80; // アイコンの初期値
            icon_orignal_max = 0x9f; // アイコンの最大数

            icon_palette_pointer = 0x4ac4; // アイコンのパレットの開始位置
            unit_wait_icon_pointer = 0x21c74; // ユニット待機アイコンの開始位置
            unit_wait_barista_anime_address = 0x2218C;  // ユニット待機アイコンのバリスタのアニメ指定アドレス
            unit_wait_barista_id = 0x42;  // ユニット待機アイコンのバリスタの位置
            unit_icon_palette_address = 0x0100968; // ユニットのパレットの開始位置
            unit_icon_enemey_palette_address = 0x100988; // ユニット(敵軍)のパレットの開始位置
            unit_icon_npc_palette_address = 0x1009A8; // ユニット(友軍)のパレットの開始位置
            unit_icon_gray_palette_address = 0x1009C8; // ユニット(グレー))のパレットの開始位置
            unit_icon_four_palette_address = 0x1009E8; // ユニット(4軍))のパレットの開始位置
            unit_icon_lightrune_palette_address = 0x0; // ユニット(光の結界)のパレットの開始位置
            unit_icon_sepia_palette_address = 0x0;   // ユニット(セピア)のパレットの開始位置

            unit_move_icon_pointer = 0x60ED8; // ユニット移動アイコンの開始位置
            lightrune_uniticon_id = 0; // ユニット(光の結界)のユニットアイコンのID
            map_setting_pointer = 0x2bb20;  // マップ設定の開始位置
            {
                if (rom.u16(0x2BB12) == 0x2048)
                {
                    map_setting_datasize= 72;
                }
                else
                {
                	map_setting_datasize = 68;
                }
            }    
            //マップ設定のデータサイズ
            map_setting_event_plist_pos = 58; //event plistの場所 
            map_setting_worldmap_plist_pos = 59; //woldmap event plistの場所 
            map_setting_clear_conditon_text_pos = 0x30; //マップの右上に表示されているクリア条件の定義場所 
            map_setting_name_text_pos = 0x38; //マップ名のテキスト定義場所 
            map_config_pointer = 0x018a7c;      //マップ設定の開始位置(config)
            map_obj_pointer = 0x018AE4;         //マップ設定の開始位置(obj) objとpalは同時参照があるので、同一値である必要がある 
            map_pal_pointer = 0x018B18;         //マップ設定の開始位置(pal) objとpalは同時参照があるので、同一値である必要がある 
            map_tileanime1_pointer = 0x028344;  //マップ設定の開始位置(titleanime1)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_tileanime2_pointer = 0x028E7C;  //マップ設定の開始位置(titleanime2)titleanime1とtitleanime2は同時参照があるので、同一値である必要がある 
            map_map_pointer_pointer = 0x2BB70; //マップ設定の開始位置(map)
            map_mapchange_pointer = 0x02BB9C;   //マップ設定の開始位置(mapchange)
            map_event_pointer = 0x02BBCC;       //マップ設定の開始位置(event)
            map_worldmapevent_pointer = 0x9310C; //マップ設定の開始位置(worldmap (FE6のみ))
            map_map_pointer_list_default_size = 0xe5; //PLIST拡張をしていない時のバニラでのPLISTの数
            image_battle_animelist_pointer = 0x4b0f8;   // 戦闘アニメリストの開始位置
            support_unit_pointer = 0x6076fc;   // 支援相手の開始位置
            support_talk_pointer = 0x6afe0;   // 支援会話の開始位置
            unit_palette_color_pointer = 0x0;  // ユニットのパレット(カラー)の開始位置
            unit_palette_class_pointer = 0x0;  // ユニットのパレット(クラス)の開始位置
            support_attribute_pointer = 0x22dbc;  //支援効果の開始位置
            terrain_recovery_pointer = 0x192d0;  //地形回復 全クラス共通
            terrain_bad_status_recovery_pointer = 0x0192E0;  //地形回復 全クラス共通
            terrain_show_infomation_pointer = 0x72DC4;  //地形ウィンドウに情報表示 全クラス共通
            ballista_movcost_pointer = 0x0; //地形ウィンドウに情報表示 全クラス共通
            ccbranch_pointer = 0x0;  // CC分岐の開始位置
            ccbranch2_pointer = 0x0;  // CC分岐の開始位置2 見習いのCCにのみ利用 CC分岐の開始位置+1の場所を指す
            class_alphaname_pointer = 0x95B48;  // クラスのアルファベット表記の開始位置
            map_terrain_name_pointer = 0x192c0;  // マップの地名表記の開始位置
            image_chapter_title_pointer = 0x70d44;  // 章タイトルの開始位置
            image_chapter_title_palette = 0x3094F4;  // 章タイトルのパレット 多分違う
            image_unit_palette_pointer = 0x4B11C;  // ユニットパレットの開始位置
            item_pointer = 0x16410;  // アイテムの開始位置
            item_datasize = 32;  // アイテムのデータサイズ
            item_effect_pointer = 0x49db4;  // アイテムエフェクトの開始位置
            sound_table_pointer = 0x3748;  // ソングテーブルの開始位置
            sound_room_pointer = 0x8b9c4;  // サウンドルームの開始位置
            sound_room_datasize = 12;   // サウンドルームのデータサイズ
            sound_room_cg_pointer = 0x0;  // サウンドルームの背景リスト(FE7のみ)
            event_ballte_talk_pointer = 0x6b660;  // 交戦時セリフの開始位置
            event_ballte_talk2_pointer = 0x6b744; // 交戦時セリフの開始位置2 (FE6だとボス汎用会話テーブルがある)
            event_haiku_pointer = 0x6b7fc;  // 死亡時セリフの開始位置
            event_haiku_tutorial_1_pointer = 0x0;  // リン編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_haiku_tutorial_2_pointer = 0x0;  // エリウッド編チュートリアル 死亡時セリフの開始位置 FE7のみ
            event_force_sortie_pointer = 0x0;  // 強制出撃の開始位置
            event_tutorial_pointer = 0x0;  //イベントチュートリアルポインタ FE7のみ
            map_exit_point_pointer = 0x32c60;  // 離脱ポイント開始サイズ
            map_exit_point_npc_blockadd = 0x2d;  // NPC離脱.
            map_exit_point_blank = 0x10DA1C;  // 一つも離脱ポインタがない時のNULLマーク 共通で使われる.
            sound_boss_bgm_pointer = 0x0;  // ボスBGMの開始位置
            sound_foot_steps_pointer = 0x0;  // クラス足音の開始位置
            sound_foot_steps_switch2_address = 0x0;
            worldmap_point_pointer = 0x0;  // ワールドマップ拠点の開始位置
            worldmap_bgm_pointer = 0x0;  // ワールドマップのBGMテーブルの開始位置
            worldmap_icon_data_pointer = 0;   // ワールドマップのアイコンデータのテーブルの開始位置
            worldmap_event_on_stageclear_pointer = 0x0;  // ワールドマップイベントクリア時
            worldmap_event_on_stageselect_pointer = 0x0;  // ワールドマップイベント 拠点選択時
            worldmap_county_border_pointer = 0;  // ワールドマップ国名の表示
            worldmap_county_border_palette_pointer = 0x0;  // ワールドマップ国名の表示のパレット
            item_shop_hensei_pointer = 0x95f10;  //編成準備店
            item_cornered_pointer = 0x25a9c;  //すくみの開始位置
            ed_1_pointer = 0x0;   //ED 倒れたら撤退するかどうか
            ed_2_pointer = 0x0;   //ED 通り名
            ed_3a_pointer = 0x91834;   //ED 後日談　
            ed_3b_pointer = 0x0;   //ED FE6にはない
            ed_3c_pointer = 0x0;   //ED FE6にはない
            generic_enemy_portrait_pointer = 0x8df0;  //一般兵の顔
            generic_enemy_portrait_count = 0x8-1;  //一般兵の顔の個数
            cc_item_hero_crest_itemid = 0x5F;   //CCアイテム 英雄の証
            cc_item_knight_crest_itemid = 0x60;   //CCアイテム 騎士の勲章
            cc_item_orion_bolt_itemid = 0x61;   //CCアイテム オリオンの矢
            cc_elysian_whip_itemid = 0x62;   //CCアイテム 天空のムチ
            cc_guiding_ring_itemid = 0x63;   //CCアイテム 導きの指輪
            cc_fallen_contract_itemid = 0x0;   //CCアイテム ダミー8A
            cc_master_seal_itemid = 0x0;   //CCアイテム マスタープルフ
            cc_ocean_seal_itemid = 0x0;   //CCアイテム 覇者の証
            cc_moon_bracelet_itemid = 0x0;   //CCアイテム 月の腕輪
            cc_sun_bracelet_itemid = 0x0;   //CCアイテム 太陽の腕輪

            cc_item_hero_crest_pointer = 0x237b0;   //CCアイテム 英雄の証
            cc_item_knight_crest_pointer = 0x237b8;   //CCアイテム 騎士の勲章
            cc_item_orion_bolt_pointer = 0x237c0;   //CCアイテム オリオンの矢
            cc_elysian_whip_pointer = 0x237c8;   //CCアイテム 天空のムチ
            cc_guiding_ring_pointer = 0x237f0;   //CCアイテム 導きの指輪
            cc_fallen_contract_pointer = 0x0;   //CCアイテム ダミー8A 闇の契約書
            cc_master_seal_pointer = 0x0;   //CCアイテム マスタープルフ
            cc_ocean_seal_pointer = 0x0;   //CCアイテム 覇者の証
            cc_moon_bracelet_pointer = 0x0;   //CCアイテム 月の腕輪 天の刻印
            cc_sun_bracelet_pointer = 0x0;   //CCアイテム 太陽の腕輪 天の刻印

            unit_increase_height_pointer = 0x0;   //ステータス画面で背を伸ばす
            unit_increase_height_switch2_address = 0x0; 
            op_class_demo_pointer = 0x0;  //OPクラスデモ
            op_class_font_pointer = 0x0;   //OPクラス日本語フォント
            op_class_font_palette_pointer = 0x0;   // OPクラス紹介フォントのパレット
            status_font_pointer = 0x0;   //ステータス画面用のフォント
            status_font_count = 0x0;   //ステータス画面用のフォントの数(英語版と日本語で数が違う)
            ed_staffroll_image_pointer = 0x0;  // スタッフロール
            ed_staffroll_palette_pointer = 0x0;  // スタッフロールのパレット
            op_prologue_image_pointer = 0x0;  // OP字幕
            op_prologue_palette_color_pointer = 0x0;  // OP字幕のパレット ???

            arena_class_near_weapon_pointer = 0x299fc;  //闘技場 近接武器クラス 
            arena_class_far_weapon_pointer = 0x29a08;  // 闘技場 遠距離武器クラス
            arena_class_magic_weapon_pointer = 0x29A58;  // 闘技場 魔法武器クラス

            arena_enemy_weapon_basic_pointer = 0x29C44;  // 闘技場 敵武器テーブル基本武器
            arena_enemy_weapon_rankup_pointer = 0x29c64;  // 闘技場 敵武器テーブルランクアップ武器
            link_arena_deny_unit_pointer = 0;   //通信闘技場 禁止ユニット 

            worldmap_road_pointer = 0x0;  // ワールドマップの道

            menu_definiton_pointer = 0x1B2B4;  //メニュー定義
            menu_promotion_pointer = 0x0;  //CC決定する選択子
            menu_promotion_branch_pointer = 0x0;  //FE8にある分岐CCメニュー
            menu_definiton_split_pointer = 0x0;   //FE8にある分岐メニュー
            menu_definiton_worldmap_pointer = 0x0;  //FE8のワールドマップのメニュー
            menu_definiton_worldmap_shop_pointer = 0x0;  //FE8のワールドマップ店のメニュー        
            menu_unit_pointer =  0x5c7608;  // ユニットメニュー
            menu_game_pointer =  0x5C7650;  // ゲームメニュー
            menu_debug1_pointer = 0x5C73EC;   // デバッグメニュー
            menu_item_pointer = 0x5C7554;   // アイテム利用メニュー
            MenuCommand_UsabilityAlways = 0x041E6C;  //メニューを開くという値を返す関数のアドレス
            MenuCommand_UsabilityNever = 0x0;  //メニューを開かないという値を返す関数のアドレス       
            status_rmenu_unit_pointer = 0x70344;  // ステータス RMENU1
            status_rmenu_game_pointer = 0x7034c;  // ステータス RMENU2
            status_rmenu3_pointer = 0x70364;  // ステータス RMENU3
            status_rmenu4_pointer = 0x2E420;  // 戦闘予測 RMENU4
            status_rmenu5_pointer = 0x2E438;  // 戦闘予測 RMENU5
            status_rmenu6_pointer = 0x0;  // 状況画面 RMENU6
            status_param1_pointer = 0x6ED8C;  // ステータス PARAM1
            status_param2_pointer = 0x6F148;  // ステータス PARAM2
            status_param3w_pointer = 0x6F3D8;  // ステータス PARAM3 武器
            status_param3m_pointer = 0x6F394;  // ステータス PARAM3 魔法

            systemmenu_common_image_pointer = 0x0732EC;  //システムメニューの画像
            systemmenu_common_palette_pointer = 0x097008;  //システムパレット 無圧縮4パレット
            systemmenu_goal_tsa_pointer = 0x0;  //システムメニュー 目的表示TSA FE6にはない
            systemmenu_terrain_tsa_pointer = 0x072E4C;  //システムメニュー 地形表示TSA

            systemmenu_name_image_pointer = 0x0732EC;  //システムメニュー 名前表示画像(FE8は共通画像)
            systemmenu_name_tsa_pointer = 0x072AB4;  //システムメニュー 名前表示TSA
            systemmenu_name_palette_pointer = 0x072754;  //システムメニュー 名前表示パレット

            systemmenu_battlepreview_image_pointer = 0x02DE34;  //戦闘プレビュー(fe8はシステムメニュー画像と同じ/ FE7,FE6は違う)
            systemmenu_battlepreview_tsa_pointer = 0x02D97C;  //戦闘プレビューTSA
            systemmenu_battlepreview_palette_pointer = 0x02DD54;  //戦闘プレビューパレット

            systemarea_move_gradation_palette_pointer = 0x01BFC8;  //行動範囲
            systemarea_attack_gradation_palette_pointer = 0x01BFCC;  //攻撃範囲
            systemarea_staff_gradation_palette_pointer = 0x01BFD0;  //杖の範囲

            systemmenu_badstatus_image_pointer = 0;  //無圧縮のバッドステータス画像
            systemmenu_badstatus_palette_pointer = 0x70EE8;  //バッドステータスのパレット
            systemmenu_badstatus_old_image_pointer = 0x732EC;  //昔の圧縮のバッドステータス画像 FE7-FE6で 毒などのステータス
            systemmenu_badstatus_old_palette_pointer = 0x6388;  //昔の圧縮のバッドステータス画像のパレット FE7 FE6

            bigcg_pointer = 0x0;  // CG
            end_cg_address = 0x0;  // END CG FE8のみ
            worldmap_big_image_pointer = 0x68C2DC;  //ワールドマップ フィールドになるでかい奴  
            worldmap_big_palette_pointer = 0x68C2E0;  //ワールドマップ フィールドになるでかい奴 パレット  
            worldmap_big_dpalette_pointer = 0x0;  //ワールドマップ フィールドになるでかい奴 闇パレット  
            worldmap_big_palettemap_pointer = 0x0;  //ワールドマップ フィールドになるでかい奴 パレットマップ
            worldmap_event_image_pointer = 0x0;  //ワールドマップ イベント用 
            worldmap_event_palette_pointer = 0x0;  //ワールドマップ イベント用 パレット  
            worldmap_event_tsa_pointer = 0x0;  //ワールドマップ イベント用 TSA
            worldmap_mini_image_pointer = 0x0;  //ワールドマップ ミニマップ 
            worldmap_mini_palette_pointer = 0x0;  //ワールドマップ ミニマップ パレット  
            worldmap_icon_palette_pointer = 0x0;  //ワールドアイコンと道パレット
            worldmap_icon1_pointer = 0x0;  //ワールドマップ アイコン1
            worldmap_icon2_pointer = 0x0;  //ワールドマップ アイコン2
            worldmap_road_tile_pointer = 0x0;  //ワールドマップ  道チップ
            map_load_function_pointer = 0x0;  //マップチャプターに入ったときの処理(FE8のみ)
            map_load_function_switch1_address = 0x0; 
            system_icon_pointer = 0x15B70; //システム アイコン集
            system_icon_palette_pointer = 0x15B7C; //システム アイコンパレット集
            system_icon_width_address = 0x15B54;  //システムアイコンの幅が書かれているアドレス
            system_weapon_icon_pointer = 0x750DC; //剣　斧　弓などの武器属性アイコン集
            system_weapon_icon_palette_pointer = 0x750E4; //剣　斧　弓などの武器属性アイコン集のパレット
            system_music_icon_pointer = 0x08C98C; //音楽関係のアイコン集
            system_music_icon_palette_pointer = 0x08C980; //音楽関係のアイコン集のパレット
            weapon_rank_s_bonus_address = 0; //武器ランクSボーナス設定
            weapon_battle_flash_address = 0; //神器 戦闘時フラッシュ
            weapon_effectiveness_2x3x_address = 0; //神器 2倍 3倍特効
            font_item_address = 0x59027C; //アイテム名とかに使われるフォント
            font_serif_address = 0x5A82B0;  //セリフとかに使われるフォント
            monster_probability_pointer = 0x0;  //魔物発生確率
            monster_item_item_pointer = 0x0;  //魔物所持アイテム アイテム確率
            monster_item_probability_pointer = 0x0;  //魔物所持アイテム 所持確率
            monster_item_table_pointer = 0x0;  //魔物所持アイテム アイテムと所持を管理するテーブル
            monster_wmap_base_point_pointer = 0x0;  //ワールドマップに魔物を登場させる処理達
            monster_wmap_stage_1_pointer = 0x0; 
            monster_wmap_stage_2_pointer = 0x0; 
            monster_wmap_probability_1_pointer = 0x0; 
            monster_wmap_probability_2_pointer = 0x0; 
            monster_wmap_probability_after_1_pointer = 0x0; 
            monster_wmap_probability_after_2_pointer = 0x0; 
            battle_bg_pointer = 0x5F090;  //戦闘背景
            battle_terrain_pointer = 0x44534;  //戦闘地形
            senseki_comment_pointer = 0x0;  //戦績コメント
            unit_custom_battle_anime_pointer = 0x0;  //ユニット専用アニメ FE7にある
            magic_effect_pointer = 0x4C8C8;  //魔法効果へのポインタ
            magic_effect_original_data_count = 0x33;  //もともとあった魔法数
            system_move_allowicon_pointer = 0x2ad0c; //移動するときの矢印アイコン
            system_move_allowicon_palette_pointer = 0x2ad14; //移動するときの矢印アイコン アイコンパレット集
            system_tsa_16color_304x240_pointer = 0x8F058;  //16色304x240 汎用TSAポインタ
            eventunit_data_size = 16;  //ユニット配置のデータサイズ
            eventcond_tern_size = 12;  //イベント条件 ターン条件のサイズ FE7->16 FE8->12
            eventcond_talk_size = 12;  //イベント条件 話す会話条件のサイズ FE6->12 FE7->16 FE8->16
            oping_event_pointer = 0x0; 
            ending1_event_pointer = 0x0; 
            ending2_event_pointer = 0x0; 
            RAMSlotTable_address = 0x5C5280; 
            supply_pointer_address = 0x296A8;   //輸送隊RAMへのアドレス
            workmemory_player_units_address = 0x0202AB78;     //ワークメモリ PLAYER UNIT
            workmemory_enemy_units_address = 0x0202BCE8;     //ワークメモリ PLAYER UNIT
            workmemory_npc_units_address = 0x202CAF8;     //ワークメモリ PLAYER UNIT
            workmemory_mapid_address = 0x0202AA56;     //ワークメモリ マップID
            workmemory_chapterdata_address = workmemory_mapid_address - 0xE;  //ワークメモリ章データ
            workmemory_chapterdata_size = 0x20;     //ワークメモリ 章データのサイズ
            workmemory_battle_actor_address = 0x02039214;  //ワークメモリ 戦闘時のユニット構造体
            workmemory_battle_target_address = 0x02039290;  //ワークメモリ 戦闘時のユニット構造体
            workmemory_worldmap_data_address = 0x0; //ワークメモリ ワールドマップ関係の起点
            workmemory_worldmap_data_size = 0x0;  //ワークメモリ ワールドマップ関係のサイズ
            workmemory_arena_data_address = 0x02039504; //ワークメモリ 闘技場関係の起点
            workmemory_ai_data_address = 0x02039600;  //ワークメモリ AI関係の起点
            workmemory_action_data_address = 0x0203956C;  //ワークメモリ ActionData
            workmemory_dungeon_data_address = 0x0;  //ワークメモリ ダンジョン FE8のみ
            workmemory_battlesome_data_address = 0x0;  //ワークメモリ バルトに関係する諸データ
            workmemory_battleround_data_address = 0x0;  //ワークメモリ　戦闘のラウンドデータ
            workmemory_last_string_address = 0x0202A404;   //ワークメモリ 最後に表示した文字列
            workmemory_text_buffer_address = 0x02029404;   //ワークメモリ デコードされたテキスト
            workmemory_next_text_buffer_address = 0x03000038;   //ワークメモリ 次に表示するTextBufferの位置を保持するポインタ
            workmemory_local_flag_address = 0x03004898;   //ワークメモリ グローバルフラグ
            workmemory_global_flag_address = 0x03004890;   //ワークメモリ ローカルフラグ
            workmemory_trap_address = 0x02039330;   //ワークメモリ トラップ
            workmemory_bwl_address = 0x0203D524;   //BWLデータ
            workmemory_clear_turn_address = 0x0203D994;  //ワークメモリ クリアターン数
            workmemory_clear_turn_count = 0x20;   //クリアターン数 最大数
            workmemory_memoryslot_address = 0x02039330;   //ワークメモリ メモリスロットFE8
            workmemory_eventcounter_address = 0x0;   //イベントカウンター メモリスロットFE8
            workmemory_procs_forest_address = 0x020258CC;   //ワークメモリ Procs
            workmemory_procs_pool_address = 0x02023CC4;   //ワークメモリ Procs
            function_sleep_handle_address = 0;   //ワークメモリ Procs待機中
            workmemory_user_stack_base_address = 0x03007DE0;  //ワークメモリ スタックの一番底
            function_fe_main_return_address = 0x08000ACE + 1;  //スタックの一番底にある戻り先
            workmemory_control_unit_address = 0x030044B0;  //ワークメモリ 操作ユニット
            workmemory_bgm_address = 0x02023CBC;  //ワークメモリ BGM
            function_event_engine_loop_address = 0x0800DEDC + 1;  //イベントエンジン
            workmemory_reference_procs_event_address_offset = 0x2C;  //Procsのイベントエンジンでのイベントのアドレスを格納するuser変数の場所
            workmemory_procs_game_main_address = 0x02023CC4;  //ワークメモリ Procsの中でのGAMEMAIN
            workmemory_palette_address = 0x02021708;  //RAMに記録されているダブルバッファのパレット領域
            workmemory_sound_player_00_address = 0x030062E0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_01_address = 0x030064F0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_02_address = 0x03006530;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_03_address = 0x03006600;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_04_address = 0x03006570;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_05_address = 0x03006260;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_06_address = 0x030062A0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_07_address = 0x030064B0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_sound_player_08_address = 0x030065C0;  //RAMに設定されているサウンドプレイヤーバッファ
            workmemory_keybuffer_address = 0x02023B20;  //RAMのキーバッファ
            procs_maptask_pointer = 0x2911C;  //PROCSのMAPTASK 
            procs_soundroomUI_pointer = 0x8BD68;  //PROCSのSoundRoomUI 
            procs_game_main_address = 0x85C4A34;  //PROCSのGAME MAIN 
            summon_unit_pointer = 0;  //召喚
            summons_demon_king_pointer = 0;  //呼魔
            summons_demon_king_count_address = 0;  //呼魔リストの数
            mant_command_pointer = 0x57d50;  //マント
            mant_command_startadd = 0x0D;  //マント開始数
            mant_command_count_address = 0x57D3C;  //マント数 アドレス
            unit_increase_height_yes = 0;   //ステータス画面で背を伸ばす 伸ばす
            unit_increase_height_no = 0;   //ステータス画面で背を伸ばす 伸ばさない
            battle_screen_TSA1_pointer = 0x044388;   //戦闘画面
            battle_screen_TSA2_pointer = 0x04438C;   //戦闘画面
            battle_screen_TSA3_pointer = 0x043B38;   //戦闘画面
            battle_screen_TSA4_pointer = 0x043B40;   //戦闘画面
            battle_screen_TSA5_pointer = 0x04485C;   //戦闘画面
            battle_screen_palette_pointer = 0x044864;   //戦闘画面 パレット
            battle_screen_image1_pointer = 0x044654;   //戦闘画面 画像1
            battle_screen_image2_pointer = 0x0446B4;   //戦闘画面 画像2
            battle_screen_image3_pointer = 0x044714;   //戦闘画面 画像3
            battle_screen_image4_pointer = 0x044774;   //戦闘画面 画像4
            battle_screen_image5_pointer = 0x044850;   //戦闘画面 画像5
            ai1_pointer = 0x5C97F8;   //AI1ポインタ
            ai2_pointer = 0x5C97EC;   //AI2ポインタ
            ai3_pointer = 0x0325AC;   //AI3ポインタ
            ai_steal_item_pointer = 0x30228;   //AI盗む アイテム評価テーブル 0x085C8834
            ai_preform_staff_pointer = 0x33C00;    //AI杖 杖評価テーブル
            ai_preform_staff_direct_asm_pointer = 0x33C84;   //AI杖 杖評価テーブル ai_preform_staff_pointer+4への参照
            ai_preform_item_pointer = 0x034AA8;  //AIアイテム アイテム評価テーブル
            ai_preform_item_direct_asm_pointer = 0x34B44;   //AIアイテム アイテム評価テーブル
            ai_map_setting_pointer = 0x2E520;   //AI 章ごとの設定テーブル 0x0810DA20
            item_usability_array_pointer = 0x22ff8;  //アイテムを利用できるか判定する
            item_usability_array_switch2_address = 0x22fe6; 
            item_effect_array_pointer = 0x27fd4;     //アイテムを利用した場合の効果を定義する
            item_effect_array_switch2_address = 0x27fba; 
            item_promotion1_array_pointer = 0x23794;    //CCアイテムを使った場合の処理を定義する
            item_promotion1_array_switch2_address = 0x23784; 
            item_promotion2_array_pointer = 0x0;   //CCアイテムかどうかを定義する(FE7のみ)
            item_promotion2_array_switch2_address = 0x0; 
            item_staff1_array_pointer = 0x234b4;     //アイテムのターゲット選択の方法を定義する(多分)
            item_staff1_array_switch2_address = 0x234a2; 
            item_staff2_array_pointer = 0x5c438;     //杖の種類を定義する
            item_staff2_array_switch2_address = 0x5c426; 
            item_statbooster1_array_pointer = 0x27ee8;     //ドーピングアイテムを利用した時のメッセージを定義する
            item_statbooster1_array_switch2_address = 0x27ed2; 
            item_statbooster2_array_pointer = 0x0;     //ドーピングアイテムとCCアイテムかどうかを定義する  (FE6にはない)
            item_statbooster2_array_switch2_address = 0x0; 
            item_errormessage_array_pointer = 0x23294;     //アイテム利用時のエラーメッセージ
            item_errormessage_array_switch2_address = 0x23282; 
            event_function_pointer_table_pointer = 0x0E038;     //イベント命令ポインタ
            event_function_pointer_table2_pointer = 0x0;    //イベント命令ポインタ2 (FE8のみ)
            item_effect_pointer_table_pointer = 0x04C8C8;    //間接エフェクトポインタ
            command_85_pointer_table_pointer = 0x05BF38;     //85Commandポインタ
            dic_main_pointer = 0x0;      //辞書メインポインタ
            dic_chaptor_pointer = 0x0;   //辞書章ポインタ
            dic_title_pointer = 0x0;    //辞書タイトルポインタ
            itemicon_mine_id = 0x0;  // アイテムアイコンのフレイボムの位置
            item_gold_id = 0x6f;    // お金を取得するイベントに利用されるゴールドのID
            unitaction_function_pointer = 0x2A054;   // ユニットアクションポインタ
            lookup_table_battle_terrain_00_pointer = 0x49CF8;  //戦闘アニメの床
            lookup_table_battle_terrain_01_pointer = 0x49CA4;  //戦闘アニメの床
            lookup_table_battle_terrain_02_pointer = 0x49CAC; //戦闘アニメの床
            lookup_table_battle_terrain_03_pointer = 0x49CB4;  //戦闘アニメの床
            lookup_table_battle_terrain_04_pointer = 0x49CBC;  //戦闘アニメの床
            lookup_table_battle_terrain_05_pointer = 0x49CC4;  //戦闘アニメの床
            lookup_table_battle_terrain_06_pointer = 0x49CCC;  //戦闘アニメの床
            lookup_table_battle_terrain_07_pointer = 0x49CD4;  //戦闘アニメの床
            lookup_table_battle_terrain_08_pointer = 0x49CDC;  //戦闘アニメの床
            lookup_table_battle_terrain_09_pointer = 0x49CE4;  //戦闘アニメの床
            lookup_table_battle_terrain_10_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_11_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_12_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_13_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_14_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_15_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_16_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_17_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_18_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_19_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_terrain_20_pointer = 0x00;  //戦闘アニメの床
            lookup_table_battle_bg_00_pointer = 0x49D94;  //戦闘アニメの背景
            lookup_table_battle_bg_01_pointer = 0x49D44;  //戦闘アニメの背景
            lookup_table_battle_bg_02_pointer = 0x49D4C;  //戦闘アニメの背景
            lookup_table_battle_bg_03_pointer = 0x49D54;  //戦闘アニメの背景
            lookup_table_battle_bg_04_pointer = 0x49D5C;  //戦闘アニメの背景
            lookup_table_battle_bg_05_pointer = 0x49D64;  //戦闘アニメの背景
            lookup_table_battle_bg_06_pointer = 0x49D6C;  //戦闘アニメの背景
            lookup_table_battle_bg_07_pointer = 0x49D74;  //戦闘アニメの背景
            lookup_table_battle_bg_08_pointer = 0x49D7C;  //戦闘アニメの背景
            lookup_table_battle_bg_09_pointer = 0x49D84;  //戦闘アニメの背景
            lookup_table_battle_bg_10_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_11_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_12_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_13_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_14_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_15_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_16_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_17_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_18_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_19_pointer = 0x00;  //戦闘アニメの背景
            lookup_table_battle_bg_20_pointer = 0x00;  //戦闘アニメの背景
            map_terrain_type_count = 48;   //地形の種類の数
            menu_J12_always_address = 0x41E6C;  //メニューの表示判定関数 常に表示する
            menu_J12_hide_address = 0x0;    //メニューの表示判定関数 表示しない
            status_game_option_pointer = 0x8C490;  //ゲームオプション
            status_game_option_order_pointer = 0x68AAE8;  //ゲームオプションの並び順
            status_game_option_order2_pointer = 0x0;  //ゲームオプションの並び順2 FE7のみ
            status_game_option_order_count_address = 0x68AAE4;  //ゲームオプションの個数
            status_units_menu_pointer = 0;   //部隊メニュー
            tactician_affinity_pointer = 0;  //軍師属性(FE7のみ)
            event_final_serif_pointer = 0x0;  //終章セリフ(FE7のみ)
            compress_image_borderline_address = 0xF9D80;  //これ以降に圧縮画像が登場するというアドレス

            Default_event_script_term_code = new byte[] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード
            Default_event_script_toplevel_code = new byte[] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //イベント命令を終了させるディフォルトコード(各章のトップレベルのイベント)
            Default_event_script_mapterm_code = new byte[] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //ワールドマップイベント命令を終了させるディフォルトコード
            main_menu_width_address = 0x5C764A;  //メインメニューの幅
            map_default_count = 0x2D;     // ディフォルトのマップ数
            wait_menu_command_id = 0x00;  //WaitメニューのID FE6にMenu IDはない
            font_default_begin = 0x59060C; 
            font_default_end = 0x5C39A0; 
            item_name_article_pointer = 0x0;  // a|an|the を切り替えるテーブル 英語版のみ
            item_name_article_switch2_address = 0x0; 
            vanilla_field_config_address = 0x23956C;     //バニラのタイルセット よく使われもの
            vanilla_field_image_address = 0x2478F0; 
            vanilla_village_config_address = 0x2384F8; 
            vanilla_village_image_address = 0x222628; 
            vanilla_casle_config_address = 0x2365F4; 
            vanilla_casle_image_address = 0x219DEC; 
            vanilla_plain_config_address = 0x22A6A4; 
            vanilla_plain_image_address = 0x1E3F54; 
            map_minimap_tile_array_pointer = 0x86E14;  //minimapのチップ割り当て
            bg_reserve_black_bgid = U.NOT_FOUND; 
            bg_reserve_random_bgid = U.NOT_FOUND; 

            extends_address = 0x08800000;   //拡張領域
            orignal_crc32 = 0xd38763e1;  //無改造ROMのCRC32
            is_multibyte = true;     // マルチバイトを利用するか？
            version = 6;     // バージョン

            OverwriteROMConstants(rom);
        }

        override public uint patch_C01_hack(out uint enable_value) { enable_value = 0xFD32F568; return 0x2DBF5C; } //C01 patch
        override public uint patch_C48_hack(out uint enable_value) { enable_value = 0x0804AD76; return 0x4A768; } //C48 patch
        override public uint patch_16_tracks_12_sounds(out uint enable_value) { enable_value = 0x0; return 0x0; } //16_tracks_12_sounds patch
        override public uint patch_generic_enemy_portrait_extends(out uint enable_value) { enable_value = 0x21FFB500; return 0x8DB8; } //一般兵の顔 拡張
        override public uint patch_unitaction_rework_hack(out uint enable_value) { enable_value = 0x4C03B510; return 0x02A028; } //ユニットアクションの拡張
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
    