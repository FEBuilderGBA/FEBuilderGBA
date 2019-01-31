using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MainSimpleMenuEventErrorForm : Form
    {
        public MainSimpleMenuEventErrorForm()
        {
            InitializeComponent();
            this.EventList.OwnerDraw(DrawEventList, DrawMode.OwnerDrawVariable, false);
        }

        private void ErrorEventErrorForm_Load(object sender, EventArgs e)
        {
            UpdateContextMenu();
        }
        void UpdateContextMenu()
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;

            menuItem = new MenuItem(R._("エラーに移動する"));
            menuItem.Click += new EventHandler(U.FireKeyDown(this.EventList, this.EventList_KeyDown, Keys.Enter));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            if (ShowAllError.Checked)
            {
                menuItem = new MenuItem(R._("このエラーを表示する"));
                menuItem.Click += ShowErrorButton_Click;
                contextMenu.MenuItems.Add(menuItem);
            }
            else
            {
                menuItem = new MenuItem(R._("このエラーは誤報なので表示しない"));
                menuItem.Click += IgnoreErrorButton_Click;
                contextMenu.MenuItems.Add(menuItem);
            }

            EventList.ContextMenu = contextMenu;
        }

        private void IgnoreErrorButton_Click(object sender, EventArgs e)
        {
            if (this.EventList.SelectedIndex < 0 || this.EventList.SelectedIndex >= this.ErrorList.Count)
            {
                return;
            }
            FELint.ErrorSt est = this.ErrorList[this.EventList.SelectedIndex];
            MainSimpleMenuEventErrorIgnoreErrorForm f = (MainSimpleMenuEventErrorIgnoreErrorForm)InputFormRef.JumpFormLow<MainSimpleMenuEventErrorIgnoreErrorForm>();
            f.Init(est);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            Program.LintCache.Update(est.Addr, f.GetComment());
            Scan();
            MainSimpleMenuForm.NeedSystemErrorCheck();
        }
        private void ShowErrorButton_Click(object sender, EventArgs e)
        {
            if (this.EventList.SelectedIndex < 0 || this.EventList.SelectedIndex >= this.ErrorList.Count)
            {
                return;
            }
            FELint.ErrorSt est = this.ErrorList[this.EventList.SelectedIndex];


            Program.LintCache.Update(est.Addr, "");
            Scan();
        }

        uint MapID = U.NOT_FOUND;
        List<FELint.ErrorSt> ErrorList;
        public void Init(uint mapid, bool showAllError)
        {
            this.ShowAllError.Checked = showAllError;
            this.MapID = mapid;
            this.EventCond_Label.Text = R._("エラー:") + MapSettingForm.GetMapName(this.MapID);
            Scan();
        }
        void Scan()
        {
            //次回システムチェックをする.
            MainSimpleMenuForm.NeedSystemErrorCheck();

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                this.ErrorList = FELint.ScanMAP(this.MapID);
                if (! this.ShowAllError.Checked)
                {
                    this.ErrorList = FELint.HiddenErrorFilter(this.ErrorList);
                }

                this.EventList.DummyAlloc(this.ErrorList.Count, -1);

                if (this.ErrorList.Count > 0)
                {
                    if (this.ShowAllError.Checked)
                    {
                        this.Explain.Text = R._("すべてのエラーを表示しています。非表示にしたエラーを再表示する場合、右クリックしてください。");
                    }
                    else
                    {
                        this.Explain.Text = R._("ダブルクリックでエラーに移動します。誤検出の場合は、右クリックして、エラーを非表示にできます。");
                    }
                }
                else
                {
                    this.Explain.Text = R._("すべてのエラーが解決されました。");
                }
            }
        }

        public static string TypeToString(FELint.Type dataType , uint addr, uint tag)
        {
            string text;
            uint show_tag = U.NOT_FOUND;
            if (dataType == FELint.Type.EVENT_COND_OBJECT)
            {
                text = R._("マップオブジェクト");
            }
            else if (dataType == FELint.Type.EVENT_COND_TALK)
            {
                text = R._("話すイベント");
            }
            else if (dataType == FELint.Type.EVENT_COND_TURN)
            {
                text = R._("ターンイベント");
            }
            else if (dataType == FELint.Type.EVENT_COND_ALWAYS)
            {
                text = R._("常時イベント");
            }
            else if (dataType == FELint.Type.EVENT_COND_START_EVENT)
            {
                text = R._("開始イベント");
            }
            else if (dataType == FELint.Type.EVENT_COND_END_EVENT)
            {
                text = R._("終了イベント");
            }
            else if (dataType == FELint.Type.EVENT_COND_START_EVENT)
            {
                text = R._("プレイヤー配置");
            }
            else if (dataType == FELint.Type.EVENT_COND_ENEMY_UNIT)
            {
                text = R._("敵配置");
            }
            else if (dataType == FELint.Type.EVENTSCRIPT)
            {
                text = R._("敵配置");
            }
            else if (dataType == FELint.Type.MAPSETTING)
            {
                text = R._("マップ設定");
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_OBJECT)
            {
                text = R._("マップ設定のマップオブジェクト");
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_CONFIG)
            {
                text = R._("マップ設定のコンフィグ");
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_MAP)
            {
                text = R._("マップ設定のマップ");
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_MAPCHANGE)
            {
                text = R._("マップ設定のマップ変化");
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_ANIMETION1)
            {
                text = R._("マップ設定のタイルアニメーション1");
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_ANIMETION2)
            {
                text = R._("マップ設定のタイルアニメーション2");
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_PALETTE)
            {
                text = R._("マップ設定のパレット");
            }
            else if (dataType == FELint.Type.WORLDMAP_EVENT)
            {
                text = R._("ワールドマップイベント内");
            }
            else if (dataType == FELint.Type.BATTLE_ANIME)
            {
                show_tag = tag + 1;
                text = R._("戦闘アニメ");
            }
            else if (dataType == FELint.Type.BATTLE_ANIME_CLASS)
            {
                show_tag = tag + 1;
                text = R._("戦闘アニメ");
            }
            else if (dataType == FELint.Type.PORTRAIT)
            {
                show_tag = tag;
                text = R._("顔画像");
            }
            else if (dataType == FELint.Type.BG)
            {
                show_tag = tag;
                text = R._("背景");
            }
            else if (dataType == FELint.Type.HAIKU)
            {
                show_tag = tag;
                text = R._("死亡セリフ");
            }
            else if (dataType == FELint.Type.BATTTLE_TALK)
            {
                show_tag = tag;
                text = R._("戦闘会話");
            }
            else if (dataType == FELint.Type.SUPPORT_TALK)
            {
                show_tag = tag;
                text = R._("支援会話");
            }
            else if (dataType == FELint.Type.SUPPORT_UNIT)
            {
                show_tag = tag;
                text = R._("支援ユニット");
            }
            else if (dataType == FELint.Type.MAPCHANGE)
            {
                show_tag = tag;
                text = R._("マップ変化");
            }
            else if (dataType == FELint.Type.UNIT)
            {
                show_tag = tag;
                text = R._("ユニット");
            }
            else if (dataType == FELint.Type.CLASS)
            {
                show_tag = tag;
                text = R._("クラス");
            }
            else if (dataType == FELint.Type.ITEM)
            {
                show_tag = tag;
                text = R._("アイテム");
            }
            else if (dataType == FELint.Type.ITEM_WEAPON_EFFECT)
            {
                show_tag = tag;
                text = R._("間接エフェクト");
            }
            else if (dataType == FELint.Type.SOUND_FOOT_STEPS)
            {
                text = R._("足音");
            }
            else if (dataType == FELint.Type.MOVECOST_NORMAL)
            {
                text = R._("移動コスト");
            }
            else if (dataType == FELint.Type.MOVECOST_RAIN)
            {
                text = R._("移動コスト(雨)");
            }
            else if (dataType == FELint.Type.MOVECOST_SHOW)
            {
                text = R._("移動コスト(雪)");
            }
            else if (dataType == FELint.Type.MOVECOST_AVOID)
            {
                text = R._("地形回避");
            }
            else if (dataType == FELint.Type.MOVECOST_DEF)
            {
                text = R._("地形防御");
            }
            else if (dataType == FELint.Type.MOVECOST_RES)
            {
                text = R._("地形魔防");
            }
            else if (dataType == FELint.Type.OP_CLASS_DEMO)
            {
                text = R._("OPクラス紹介");
            }
            else if (dataType == FELint.Type.WMAP_BASE_POINT)
            {
                text = R._("拠点");
            }
            else if (dataType == FELint.Type.SOUNDROOM)
            {
                text = R._("サウンドルーム");
            }
            else if (dataType == FELint.Type.SENSEKI)
            {
                text = R._("戦績コメント");
            }
            else if (dataType == FELint.Type.DIC)
            {
                text = R._("辞書");
            }
            else if (dataType == FELint.Type.MENU)
            {
                text = R._("メニュー");
            }
            else if (dataType == FELint.Type.STATUS)
            {
                text = R._("ステータスパラメータ");
            }
            else if (dataType == FELint.Type.ED)
            {
                text = R._("ED関係");
            }
            else if (dataType == FELint.Type.TERRAIN)
            {
                text = R._("地形名前");
            }
            else if (dataType == FELint.Type.SKILL_CONFIG)
            {
                text = R._("スキル拡張設定");
            }
            else if (dataType == FELint.Type.RMENU)
            {
                text = R._("ステータスRMenu");
            }
            else if (dataType == FELint.Type.ITEM_USAGE_POINTER)
            {
                text = R._("アイテム利用効果");
            }
            else if (dataType == FELint.Type.PATCH)
            {
                text = R._("パッチ");
            }
            else if (dataType == FELint.Type.MAPEXIT)
            {
                show_tag = tag;
                text = R._("離脱ポイント");
            }
            else if (dataType == FELint.Type.IMAGE_UNIT_WAIT_ICON)
            {
                show_tag = tag;
                text = R._("待機アイコン");
            }
            else if (dataType == FELint.Type.IMAGE_UNIT_MOVE_ICON)
            {
                show_tag = tag;
                text = R._("移動アイコン");
            }
            else if (dataType == FELint.Type.ITEM_EEFECT_POINTER)
            {
                show_tag = tag;
                text = R._("間接エフェクトポインタ");
            }
            else if (dataType == FELint.Type.IMAGE_UNIT_PALETTE)
            {
                show_tag = tag;
                text = R._("キャラパレット");
            }
            else if (dataType == FELint.Type.IMAGE_BATTLE_SCREEN)
            {
                show_tag = tag;
                text = R._("戦闘画面");
            }
            else if (dataType == FELint.Type.STATUS_GAME_OPTION)
            {
                show_tag = tag;
                text = R._("ゲームオプション");
            }
            else if (dataType == FELint.Type.FELINT_SYSTEM_ERROR)
            {
                show_tag = tag;
                text = R._("FELint内部エラー");
            }
            else
            {
                text = R._("不明");
            }

            
            if (show_tag != U.NOT_FOUND)
            {
                text += ":" + U.To0xHexString(show_tag) + "(" + U.To0xHexString(addr) + ")";
            }
            else if (addr != U.NOT_FOUND)
            {
                text += ":" + U.To0xHexString(addr);
            }

            return text;
        }

        private Size DrawEventList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.ErrorList.Count || this.MapID == U.NOT_FOUND)
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            FELint.ErrorSt est = this.ErrorList[index];

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            SolidBrush errorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font,FontStyle.Bold);

            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxWidth = 0;

            string text = TypeToString(est.DataType, est.Addr, est.Tag);

            U.DrawText(text, g, boldFont, errorBrush, isWithDraw, bounds);
            if (Program.LintCache.CheckFast(est.Addr))
            {
                string comment = "//" + R._("このエラーは非表示になっています。理由:「{0}」", Program.LintCache.At(est.Addr));

                bounds.X = listbounds.X + (lineHeight * 25);

                SolidBrush commentBrush = new SolidBrush(OptionForm.Color_Comment_ForeColor());
                bounds.X += U.DrawText(comment, g, normalFont, commentBrush, isWithDraw, bounds);
                commentBrush.Dispose();

                maxWidth = Math.Max(maxWidth, bounds.X);
            }
            bounds.Y += lineHeight;

            text = est.ErrorMessage;
            bounds.X = listbounds.X;
            Size size = U.DrawTextMulti(text, g, normalFont, brush, isWithDraw, bounds);
            bounds.X += size.Width;

            maxWidth = Math.Max(maxWidth, bounds.X);

            brush.Dispose();
            boldFont.Dispose();

            bounds.X = maxWidth;
            bounds.Y += size.Height + lineHeight;
            return new Size(bounds.X, bounds.Y);
        }
        public static void GotoEvent(FELint.Type dataType, uint addr, uint tag,uint mapid)
        {
            if (dataType == FELint.Type.EVENTSCRIPT
              || dataType == FELint.Type.WORLDMAP_EVENT)
            {//イベント内で発生したエラー
                EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                f.JumpTo(addr, tag);
                return;
            }
            else if (dataType == FELint.Type.MAPSETTING_PLIST_OBJECT
                || dataType == FELint.Type.MAPSETTING_PLIST_CONFIG
                || dataType == FELint.Type.MAPSETTING_PLIST_MAP
                || dataType == FELint.Type.MAPSETTING_PLIST_PALETTE
                || dataType == FELint.Type.MAPSETTING_PLIST_MAPCHANGE
                || dataType == FELint.Type.MAPSETTING_PLIST_ANIMETION1
                || dataType == FELint.Type.MAPSETTING_PLIST_ANIMETION2
                || dataType == FELint.Type.MAPSETTING_WORLDMAP
                || dataType == FELint.Type.MAPSETTING
                )
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    InputFormRef.JumpForm<MapSettingFE6Form>(mapid);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    if (!Program.ROM.RomInfo.is_multibyte())
                    {
                        InputFormRef.JumpForm<MapSettingFE7UForm>(mapid);
                    }
                    else
                    {
                        InputFormRef.JumpForm<MapSettingFE7Form>(mapid);
                    }
                }
                else
                {
                    InputFormRef.JumpForm<MapSettingForm>(mapid);
                }
                return;
            }
            else if (dataType == FELint.Type.BATTLE_ANIME)
            {
                ImageBattleAnimeForm f = (ImageBattleAnimeForm)InputFormRef.JumpForm<ImageBattleAnimeForm>(U.NOT_FOUND);
                f.JumpToAnimeID(tag);
                return;
            }
            else if (dataType == FELint.Type.BATTLE_ANIME_CLASS)
            {
                ImageBattleAnimeForm f = (ImageBattleAnimeForm)InputFormRef.JumpForm<ImageBattleAnimeForm>(U.NOT_FOUND);
                f.JumpToClassID(tag);
                return;
            }
            else if (dataType == FELint.Type.BG)
            {
                InputFormRef.JumpForm<ImageBGForm>(tag);
                return;
            }
            else if (dataType == FELint.Type.PORTRAIT)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    InputFormRef.JumpForm<ImagePortraitFE6Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<ImagePortraitForm>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.HAIKU)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.JumpForm<EventHaikuForm>(tag);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    InputFormRef.JumpForm<EventHaikuFE7Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<EventHaikuFE6Form>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.BATTTLE_TALK)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.JumpForm<EventBattleTalkForm>(tag);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    InputFormRef.JumpForm<EventBattleTalkFE7Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<EventBattleTalkFE6Form>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.SUPPORT_TALK)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.JumpForm<SupportTalkForm>(tag);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    InputFormRef.JumpForm<SupportTalkFE7Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<SupportTalkFE6Form>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.SUPPORT_UNIT)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    SupportUnitFE6Form f = (SupportUnitFE6Form)InputFormRef.JumpForm<SupportUnitFE6Form>();
                    f.JumpToAddr(addr);
                }
                else
                {
                    SupportUnitForm f = (SupportUnitForm)InputFormRef.JumpForm<SupportUnitForm>();
                    f.JumpToAddr(addr);
                }
                return;
            }
            else if (dataType == FELint.Type.CLASS)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    InputFormRef.JumpForm<ClassFE6Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<ClassForm>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.ITEM)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    InputFormRef.JumpForm<ItemFE6Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<ItemForm>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.ITEM_WEAPON_EFFECT)
            {
                ItemWeaponEffectForm f = (ItemWeaponEffectForm)InputFormRef.JumpForm<ItemWeaponEffectForm>(U.NOT_FOUND);
                f.JumpTo(tag);
                return;
            }
            else if (dataType == FELint.Type.UNIT)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.JumpForm<UnitForm>(tag);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    InputFormRef.JumpForm<UnitFE7Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<UnitFE6Form>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.MAPCHANGE)
            {
                MapChangeForm f = (MapChangeForm)InputFormRef.JumpForm<MapChangeForm>(tag);
                f.JumpToMAPIDAndAddr(mapid, tag);
                return;
            }
            else if (dataType == FELint.Type.SOUND_FOOT_STEPS)
            {
                SoundFootStepsForm f = (SoundFootStepsForm)InputFormRef.JumpForm<SoundFootStepsForm>(tag);
                return;
            }
            else if (dataType == FELint.Type.MOVECOST_NORMAL)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>();
                    f.JumpToClassID(tag, 0 + 1);
                }
                else
                {
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>();
                    f.JumpToClassID(tag, 0 + 1);
                }
                return;
            }
            else if (dataType == FELint.Type.MOVECOST_RAIN)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>();
                    f.JumpToClassID(tag, 1 + 1);
                }
                else
                {
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>();
                    f.JumpToClassID(tag, 1 + 1);
                }
                return;
            }
            else if (dataType == FELint.Type.MOVECOST_SHOW)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>();
                    f.JumpToClassID(tag, 2 + 1);
                }
                else
                {
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>();
                    f.JumpToClassID(tag, 2 + 1);
                }
                return;
            }
            else if (dataType == FELint.Type.MOVECOST_AVOID)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>();
                    f.JumpToClassID(tag, 3 + 1);
                }
                else
                {
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>();
                    f.JumpToClassID(tag, 3 + 1);
                }
                return;
            }
            else if (dataType == FELint.Type.MOVECOST_DEF)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>();
                    f.JumpToClassID(tag, 4 + 1);
                }
                else
                {
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>();
                    f.JumpToClassID(tag, 4 + 1);
                }
                return;
            }
            else if (dataType == FELint.Type.MOVECOST_RES)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>();
                    f.JumpToClassID(tag, 5 + 1);
                }
                else
                {
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>();
                    f.JumpToClassID(tag, 5 + 1);
                }
                return;
            }
            else if (dataType == FELint.Type.OP_CLASS_DEMO)
            {
                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<OPClassDemoFE7Form>(tag);
                }
                else if (Program.ROM.RomInfo.version() == 7 && !Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<OPClassDemoFE7UForm>(tag);
                }
                else if (Program.ROM.RomInfo.version() == 8 && Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<OPClassDemoForm>(tag);
                }
                else if (Program.ROM.RomInfo.version() == 8 && !Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<OPClassDemoFE8UForm>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.WMAP_BASE_POINT)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.JumpForm<WorldMapPointForm>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.SOUNDROOM)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    InputFormRef.JumpForm<SoundRoomFE6Form>(tag);
                }
                else
                {
                    InputFormRef.JumpForm<SoundRoomForm>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.SENSEKI)
            {
                if (Program.ROM.RomInfo.version() == 7)
                {
                    InputFormRef.JumpForm<EDSensekiCommentForm>(tag);
                }
                return;
            }
            else if (dataType == FELint.Type.DIC)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.JumpForm<TextDicForm>(U.NOT_FOUND);
                }
                return;
            }
            else if (dataType == FELint.Type.MENU)
            {
                MenuCommandForm f = (MenuCommandForm)InputFormRef.JumpForm<MenuCommandForm>();
                f.JumpToAddr(addr);
                return;
            }
            else if (dataType == FELint.Type.STATUS)
            {
                InputFormRef.JumpForm<StatusParamForm>();
                return;
            }
            else if (dataType == FELint.Type.ED)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    InputFormRef.JumpForm<EDFE6Form>();
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    InputFormRef.JumpForm<EDFE7Form>();
                }
                else
                {
                    InputFormRef.JumpForm<EDForm>();
                }
                return;
            }
            else if (dataType == FELint.Type.TERRAIN)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<MapTerrainNameForm>();
                }
                else
                {
                    InputFormRef.JumpForm<MapTerrainNameEngForm>();
                }
                return;
            }
            else if (dataType == FELint.Type.SKILL_CONFIG)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.skill_system_enum skill = InputFormRef.SearchSkillSystem();
                    if (skill == InputFormRef.skill_system_enum.SkillSystem)
                    {
                        InputFormRef.JumpForm<SkillConfigSkillSystemForm>();
                    }
                    else if (skill == InputFormRef.skill_system_enum.FE8N
                        || skill == InputFormRef.skill_system_enum.yugudora
                        || skill == InputFormRef.skill_system_enum.FE8N_ver2
                        )
                    {
                        InputFormRef.JumpForm<SkillConfigFE8NSkillForm>();
                    }
                }
                return ;
            }
            else if (dataType == FELint.Type.RMENU)
            {
                InputFormRef.JumpForm<StatusRMenuForm>(tag);
                return;
            }
            else if (dataType == FELint.Type.ITEM_USAGE_POINTER)
            {
                InputFormRef.JumpForm<ItemUsagePointerForm>(tag);
                return;
            }
            else if (dataType == FELint.Type.PATCH)
            {
                PatchForm f =(PatchForm) InputFormRef.JumpForm<PatchForm>();
                f.SelectPatchByTag(tag);
                return;
            }
            else if (dataType == FELint.Type.MAPEXIT)
            {
                InputFormRef.JumpForm<MapExitPointForm>(tag);
                return;
            }
            else if (dataType == FELint.Type.IMAGE_UNIT_WAIT_ICON)
            {
                InputFormRef.JumpForm<ImageUnitWaitIconFrom>(tag);
                return;
            }
            else if (dataType == FELint.Type.IMAGE_UNIT_MOVE_ICON)
            {
                InputFormRef.JumpForm<ImageUnitMoveIconFrom>(tag);
                return;
            }
            else if (dataType == FELint.Type.ITEM_EEFECT_POINTER)
            {
                InputFormRef.JumpForm<ItemEffectPointerForm>(tag);
                return;
            }
            else if (dataType == FELint.Type.IMAGE_UNIT_PALETTE)
            {
                InputFormRef.JumpForm<ImageUnitPaletteForm>(tag);
                return;
            }
            else if (dataType == FELint.Type.IMAGE_BATTLE_SCREEN)
            {
                InputFormRef.JumpForm<ImageBattleScreenForm>();
                return;
            }
            else if (dataType == FELint.Type.FELINT_SYSTEM_ERROR)
            {
                InputFormRef.JumpForm<ToolProblemReportForm>();
                return;
            }
            else if (dataType == FELint.Type.STATUS_GAME_OPTION)
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    InputFormRef.JumpForm<TextDicForm>(tag);
                }
                return;
            }

            //イベント
            {
                EventCondForm f = (EventCondForm)InputFormRef.JumpForm<EventCondForm>(U.NOT_FOUND);
                f.JumpToMAPIDAndAddr(mapid, FELint.TypeToEventCond(dataType), (uint)addr);
            }
        }

        void GotoEvent()
        {
            if (this.EventList.SelectedIndex < 0 || this.EventList.SelectedIndex >= this.ErrorList.Count)
            {
                return;
            }
            FELint.ErrorSt est = this.ErrorList[this.EventList.SelectedIndex];
            GotoEvent(est.DataType, est.Addr, est.Tag, this.MapID);
        }

        private void EventList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GotoEvent();
        }

        private void EventList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GotoEvent();
            }
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void MainSimpleMenuEventErrorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                Scan();
            }
        }

        private void ShowAllError_CheckedChanged(object sender, EventArgs e)
        {
            UpdateContextMenu();
            Scan();
        }

        private void EventList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                EventList.SelectedIndex = EventList.IndexFromPoint(e.X, e.Y);
            }
        }

        private void MainSimpleMenuEventErrorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //次回システムチェックをする.
            MainSimpleMenuForm.NeedSystemErrorCheck();
        }

    }
}
