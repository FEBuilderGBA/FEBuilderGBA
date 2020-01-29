﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MainFE7Form : Form
    {
        public MainFE7Form()
        {
            InitializeComponent();
            FixedButton();
            InputFormRef.RecolorMenuStrip(this.menuStrip1);
            MainFormUtil.MakeExplainFunctions(this.ControlPanel);
        }


        private void UnitButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<UnitFE7Form>();
        }

        private void SupportUnitButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SupportUnitForm>();
        }

        private void SupportTalkButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SupportTalkFE7Form>();
        }

        private void SupportAttributeButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SupportAttributeForm>();
        }

        private void ClassButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ClassForm>();
        }


        private void MoveCostButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MoveCostForm>();
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemForm>();
        }

        private void ItemEffectButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemWeaponEffectForm>();
        }

        private void ITEMSTATBOOSTERButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemStatBonusesForm>();
        }

        private void ItemCriticalButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemEffectivenessForm>();
        }

        private void ItemShopButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemShopForm>();
        }

        private void ItemCorneredButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemWeaponTriangleForm>();
        }

        private void ImagePortraitButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImagePortraitForm>();
        }

        private void ImageIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageItemIconForm>();
        }

        private void ImageUnitWaitIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageUnitWaitIconFrom>();
        }

        private void ImageUnitMoveIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageUnitMoveIconFrom>();
        }

        private void ImageBattleAnimeButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleAnimeForm>(0,"N_AddressList");
        }

        private void MapSettingButton_Click(object sender, EventArgs e)
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {
                InputFormRef.JumpForm<MapSettingFE7UForm>();
            }
            else
            {
                InputFormRef.JumpForm<MapSettingFE7Form>();
            }
        }

        private void MapPointerButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapPointerForm>();
        }

        private void EventCondButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventCondForm>();
        }

        private void EventScriptButton_Click(object sender, EventArgs e)
        {
            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>();
            f.JumpTo(0x0);
        }

        private void EventUnitButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventUnitFE7Form>();
        }

        private void EventBattleTalkButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventBattleTalkFE7Form>();
        }

        private void EventHaikuButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventHaikuFE7Form>();
        }

        private void MapExitPointButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapExitPointForm>();
        }

        private void TextButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<TextForm>();
        }

        private void TextCharCodeButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<TextCharCodeForm>();
        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<FontForm>();
        }

        private void MapTerrainNameButton_Click(object sender, EventArgs e)
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {
                InputFormRef.JumpForm<MapTerrainNameEngForm>();
            }
            else
            {
                InputFormRef.JumpForm<MapTerrainNameForm>();
            }
        }

        private void SoundRoomuttoBn_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SoundRoomForm>();
        }

        private void SoundBossBGMButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SoundBossBGMForm>();
        }

        private void SongTableButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SongTableForm>();
        }

        private void ImageBGButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBGForm>();
        }

        private void ImageChapterTitleButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageChapterTitleFE7Form>();
        }

        private void BigCGButton_Click(object sender, EventArgs e)
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {
                InputFormRef.JumpForm<ImageCGFE7UForm>();
            }
            else
            {
                InputFormRef.JumpForm<ImageCGForm>();
            }
        }

        private void EventMapChangeButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapChangeForm>();
        }

        private void ArenaClassButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ArenaClassForm>();
        }

        private void ImageUnitPaletteButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageUnitPaletteForm>();
        }

        private void EDButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EDFE7Form>();
        }

        private void ImageBattleFieldButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleBGForm>();
            
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            MenuCommandForm f = (MenuCommandForm)InputFormRef.JumpForm<MenuCommandForm>();
            f.JumpToAddr(Program.ROM.p32( Program.ROM.RomInfo.menu1_pointer()));
        }

        private void ItemCCButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemPromotionForm>();

        }

        private void ImageBattleTerrainButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleTerrainForm>();
        }

        private void SensekiCommentButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EDSensekiCommentForm>();
        }

        private void ClassOPDemoButton_Click(object sender, EventArgs e)
        {
            if (!Program.ROM.RomInfo.is_multibyte())
            {
                InputFormRef.JumpForm<OPClassDemoFE7UForm>();
            }
            else
            {
                InputFormRef.JumpForm<OPClassDemoFE7Form>();
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainFormUtil.IsNotSaveYet(this))
            {
                return;
            }
            MainFormUtil.Open(this);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.SaveOverraide(this);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.SaveAs(this);
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.Quit(this);
        }

        private void RunAsEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("emulator");
        }

        private void RunAsDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("emulator2");
        }

        private void RunAsBinaryEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("binary_editor");
        }

        private void GraphicsToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("graphics_tool");
        }

        private void RunAsSappyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("sappy");
        }

        private void RunAsProgram1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program1");
        }

        private void RunAsProgram2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program2");
        }

        private void RunAsProgram3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs("program3");
        }

        private void RunAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAs(this);
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUndoForm>();
        }

        private void MoveAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MoveToFreeSapceForm>();
        }

        private void PatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<PatchForm>();
            this.PatchMainFilter.CleanChache();
        }

        private void GraphicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<GraphicsToolForm>();
        }

        private void LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<LogForm>();
        }

        private void SongImportOtherROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SongExchangeForm>();
        }

        private void MapEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapEditorForm>();
        }

        private void PointerToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<PointerToolForm>();
        }

        private void SettingOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OptionForm>();
        }

        private void SettingVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<VersionForm>();
        }

        private void WorldMapEventPointerButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<WorldMapEventPointerFE7Form>();
        }

        ToolTipEx ToolTip;
        PatchMainFilter PatchMainFilter;
        private void MainFE7Form_Load(object sender, EventArgs e)
        {
            this.ToolTip = InputFormRef.GetToolTip<MainFE7Form>();
            this.PatchMainFilter = new PatchMainFilter(this.PatchButton, this.PatchResult, new Button[] { this.Patch0, this.Patch1, this.Patch2, this.Patch3, this.Patch4, this.Patch5, this.Patch6, this.Patch7 });

            this.Filter.Focus();
        }

        private void DisassemblerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>();
            f.JumpTo(0x0);
        }

        private void MapEditorButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapEditorForm>();
        }

        private void MapStyleEditorButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapStyleEditorForm>();
        }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<PatchForm>();
            this.PatchMainFilter.CleanChache();
        }

        private void DisassemblerButton_Click(object sender, EventArgs e)
        {
            DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>();
            f.JumpTo(0x0);
        }


        private void SystemIconButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageSystemIconForm>();
        }

        private void UnitCustomBattleAnime_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<UnitCustomBattleAnimeForm>();
        }
        private void LZ77ToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolLZ77Form>();
        }

        private void MainFE7Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OptionForm.first_form() == OptionForm.first_form_enum.EASY)
            {
                return;
            }

#if DEBUG
            //デバッグ時はいちいち聞かれると面倒なのでスルーする
#else
            if (! MainFormUtil.IsNotSaveYet(this))
            {
                e.Cancel = true;
            }
#endif

        }

        private void SimpleMenuButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MainSimpleMenuForm>();
        }

        private void EventForceSortieButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventForceSortieFE7Form>();
        }

        private void WorldMapImageButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<WorldMapImageFE7Form>();
        }

        private void MainSimpleMenuImageSubButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MainSimpleMenuImageSubForm>();
        }


        private void diffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolDiffForm>();
        }

        private void ImageMagicButton_Click(object sender, EventArgs e)
        {
            HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.MagicPatch_By_Menu);

            ImageUtilMagic.magic_system_enum magic = ImageUtilMagic.SearchMagicSystem();
            if (magic == ImageUtilMagic.magic_system_enum.FEDITOR_ADV)
            {
                InputFormRef.JumpForm<ImageMagicFEditorForm>();
            }
            else if (magic == ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                InputFormRef.JumpForm<ImageMagicCSACreatorForm>();
            }
        }

        private void eventAssembler_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventAssemblerForm>();
        }

        private void OpenLastUsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OpenLastSelectedFileForm>();
        }

        private void HexEditorButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<HexEditorForm>();
        }

        private void TranslateROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolTranslateROMForm>();
        }

        private void OtherTextButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OtherTextForm>();
        }

        private void DecreaseColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DecreaseColorTSAToolForm>();
        }

        private void BattleScreenButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageBattleScreenForm>();
        }

        private void MantAnimationButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MantAnimationForm>();
        }

        private void UnitIncreaseHeightFormButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<UnitIncreaseHeightForm>();
        }

        private void MenuDefinitionButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MenuDefinitionForm>();
        }

        private void StatusParamButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<StatusParamForm>();
        }

        private void StatusRMenuButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<StatusRMenuForm>();
        }

        private void AIFormButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<AIScriptForm>();
        }

        private void AI3FormButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<AITargetForm>();
        }

        private void ItemUsagePointerButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemUsagePointerForm>();
        }

        private void ArenaEnemyWeaponButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ArenaEnemyWeaponForm>();
        }

        private void OnlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoManual();
        }

        private void ProcsScriptButton_Click(object sender, EventArgs e)
        {
            ProcsScriptForm f = (ProcsScriptForm)InputFormRef.JumpForm<ProcsScriptForm>(U.NOT_FOUND);
            f.JumpTo(0);
        }

        private void MapTileAnimation1Button_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapTileAnimation1Form>();
        }

        private void MapTileAnimation2Button_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapTileAnimation2Form>();
        }

        private void UPSSimpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUPSPatchSimpleForm>();
        }

        private void OAMSPButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<OAMSPForm>();
        }
        void FixedButton()
        {
#if DEBUG
//            ROMRebuildButton.Show();
#else
//            ROMRebuildButton.Hide();
#endif
            if (!Program.ROM.RomInfo.is_multibyte())
            {//FE7Uでは章タイトルの画像はない
                //テキストデータを     40260c nazo font で描画しています.
                ImageChapterTitleButton.Hide();
            }
        }

        private void Filter_KeyUp(object sender, KeyEventArgs e)
        {
            MainFormUtil.ApplySearchFilter(Filter.Text, this.ControlPanel, this.PatchMainFilter, this.ToolTip);
            FixedButton();
        }

        private void Filter_DoubleClick(object sender, EventArgs e)
        {
            Filter.Clear();
            Filter_KeyUp(null, null);
        }

        private void MainFE7Form_Shown(object sender, EventArgs e)
        {
            this.Filter.Focus();
        }

        private void EventFunctionPointerButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventFunctionPointerFE7Form>();
        }

        private void Command85PointerButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<Command85PointerForm>();
        }

        private void ItemEffectPointerButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ItemEffectPointerForm>();
        }

        private void WelcomeDialogButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<WelcomeForm>();
        }

        private void DiffDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolDiffDebugSelectForm>();
        }

        private void lintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolFELintForm>();
        }


        private void ASMInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolASMInsertForm>();
        }

        private void PatchResult_Click(object sender, EventArgs e)
        {
            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo(this.Filter.Text, 0);
            this.PatchMainFilter.CleanChache();

        }

        private void FlagNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolFlagNameForm>();
        }

        private void EmulatorMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EmulatorMemoryForm>();
        }

        private void ExportEAEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolExportEAEventForm f = (ToolExportEAEventForm)InputFormRef.JumpForm<ToolExportEAEventForm>();
        }

        private void FlagNameToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolFlagNameForm>();
        }

        private void ExportEAEventToolButton_Click(object sender, EventArgs e)
        {
            ToolExportEAEventForm f = (ToolExportEAEventForm)InputFormRef.JumpForm<ToolExportEAEventForm>();
        }

        private void EmulatorMemoryToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EmulatorMemoryForm>();
        }

        private void AIStealItemButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<AIStealItemForm>();
        }

        private void AIMapSettingButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<AIMapSettingForm>();
        }

        private void ROMAnimeButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageRomAnimeForm>();
        }

        private void ToolProblemReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolProblemReportForm>();
        }

        private void ImageGenericEnemyPortraitButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ImageGenericEnemyPortraitForm>();
        }

        private void ROMRebuildButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolROMRebuildForm>();
        }

        public void SetFilter()
        {
            this.Filter.Focus();
        }

        private void ToolProblemReportToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolProblemReportForm>();
        }

        private void DiscordURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoCommunities();
        }

        private void MapTerrainFloorLookupTableButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapTerrainFloorLookupTableForm>();
        }

        private void MapTerrainBGLookupTableButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<MapTerrainBGLookupTableForm>();
        }

        private void AIPerformStaffButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<AIPerformStaffForm>();
        }

        private void SoundRoomCGButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<SoundRoomCGForm>();
        }

        private void TacticianAffinity_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<TacticianAffinityFE7>();
        }

        private void GameOptionButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<StatusOptionForm>();
        }

        private void GameOptionOrderButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<StatusOptionOrderForm>();
        }

        private void ToolUseFlagButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUseFlagForm>();
        }

        private void ToolUseFlagStripMenuItem6_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolUseFlagForm>();
        }

        private void UnitActionPointerButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<UnitActionPointerForm>();
        }

        private void StatusUnitsMenuButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<StatusUnitsMenuForm>();
        }

        private void EventFinalSerifButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<EventFinalSerifFE7Form>();
        }

        private void InitWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunToolInitWizard();
        }

        private void ChangeProjectNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolChangeProjectnameForm>();
        }

        private void WorkSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolWorkSupport>();
        }
    }
}
