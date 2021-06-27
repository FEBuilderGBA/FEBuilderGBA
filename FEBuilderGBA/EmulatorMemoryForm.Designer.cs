namespace FEBuilderGBA
{
    partial class EmulatorMemoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ERROR_EMU_CONNECT = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.EventPage = new System.Windows.Forms.TabPage();
            this.SubtileButton = new System.Windows.Forms.Button();
            this.SpeechButton = new System.Windows.Forms.Button();
            this.CurrentTextBox = new FEBuilderGBA.RichTextBoxEx();
            this.label93 = new System.Windows.Forms.Label();
            this.N_SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.RunningEventListBox = new FEBuilderGBA.ListBoxEx();
            this.RunningEventListBoxLabel = new System.Windows.Forms.Label();
            this.FlagListBox = new FEBuilderGBA.ListBoxEx();
            this.N1_LabelFilter = new System.Windows.Forms.Label();
            this.MemorySlotListBox = new FEBuilderGBA.ListBoxEx();
            this.MemorySlotLabel = new System.Windows.Forms.Label();
            this.EventHistoryPage = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.EventHistoryListBox = new FEBuilderGBA.ListBoxEx();
            this.ProcPage = new System.Windows.Forms.TabPage();
            this.Proc_ControlPanel = new System.Windows.Forms.Panel();
            this.PROCS_Address = new System.Windows.Forms.NumericUpDown();
            this.PROCS_SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label22 = new System.Windows.Forms.Label();
            this.PROCS_AddressLabel = new System.Windows.Forms.Label();
            this.ControlPanelCommand = new System.Windows.Forms.Panel();
            this.PROCS_B107 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B51 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B59 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B67 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B99 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B75 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B91 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B83 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B106 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B50 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B58 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B66 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B98 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B74 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B90 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B82 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B105 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B49 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B57 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B65 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B97 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B73 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B89 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B81 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B47 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B55 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B63 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B103 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B71 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B95 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B79 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B87 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B46 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B54 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B62 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B102 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B70 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B94 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B78 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B86 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B45 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B53 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B61 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B101 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B69 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B93 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B77 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B85 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_L_104_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_96_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_88_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_80_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_72_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_64_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_100_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_92_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_84_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_76_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_68_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_60_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_56_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_52_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_48_DWORD = new FEBuilderGBA.TextBoxEx();
            this.PROCS_L_44_DWORD = new FEBuilderGBA.TextBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PROCS_B41 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B43 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_CURSOL_CODE = new FEBuilderGBA.TextBoxEx();
            this.PROCS_B42 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_L_16_TEXT = new FEBuilderGBA.TextBoxEx();
            this.PROCS_J_44 = new System.Windows.Forms.Label();
            this.PROCS_P16 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B104 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_16_TEXT = new System.Windows.Forms.Label();
            this.PROCS_B44 = new System.Windows.Forms.NumericUpDown();
            this.textBoxEx16 = new FEBuilderGBA.TextBoxEx();
            this.PROCS_J_104 = new System.Windows.Forms.Label();
            this.numericUpDown17 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_48 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.PROCS_B48 = new System.Windows.Forms.NumericUpDown();
            this.textBoxEx17 = new FEBuilderGBA.TextBoxEx();
            this.PROCS_J_52 = new System.Windows.Forms.Label();
            this.numericUpDown18 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B52 = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.PROCS_J_56 = new System.Windows.Forms.Label();
            this.textBoxEx18 = new FEBuilderGBA.TextBoxEx();
            this.PROCS_B56 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown19 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_60 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.PROCS_B60 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B40 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_64 = new System.Windows.Forms.Label();
            this.PROCS_J_40 = new System.Windows.Forms.Label();
            this.PROCS_B64 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B39 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B100 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_39 = new System.Windows.Forms.Label();
            this.PROCS_J_68 = new System.Windows.Forms.Label();
            this.PROCS_B38 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_100 = new System.Windows.Forms.Label();
            this.PROCS_J_38 = new System.Windows.Forms.Label();
            this.PROCS_B68 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_W36 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B96 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_36 = new System.Windows.Forms.Label();
            this.PROCS_J_72 = new System.Windows.Forms.Label();
            this.PROCS_L_32_RAMPROCS = new FEBuilderGBA.TextBoxEx();
            this.PROCS_J_96 = new System.Windows.Forms.Label();
            this.PROCS_P32 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B72 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_32_RAMPROCS = new System.Windows.Forms.Label();
            this.PROCS_B92 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_L_28_RAMPROCS = new FEBuilderGBA.TextBoxEx();
            this.PROCS_J_76 = new System.Windows.Forms.Label();
            this.PROCS_P28 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_92 = new System.Windows.Forms.Label();
            this.PROCS_J_28_RAMPROCS = new System.Windows.Forms.Label();
            this.PROCS_B76 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_L_24_RAMPROCS = new FEBuilderGBA.TextBoxEx();
            this.PROCS_B88 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_P24 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_80 = new System.Windows.Forms.Label();
            this.PROCS_J_24_RAMPROCS = new System.Windows.Forms.Label();
            this.PROCS_J_88 = new System.Windows.Forms.Label();
            this.PROCS_L_20_RAMPROCS = new FEBuilderGBA.TextBoxEx();
            this.PROCS_B80 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_P20 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_B84 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_20_RAMPROCS = new System.Windows.Forms.Label();
            this.PROCS_J_84 = new System.Windows.Forms.Label();
            this.PROCS_L_12_ASM = new FEBuilderGBA.TextBoxEx();
            this.PROCS_P12 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_12_ASM = new System.Windows.Forms.Label();
            this.PROCS_L_8_ASM = new FEBuilderGBA.TextBoxEx();
            this.PROCS_P8 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_8_ASM = new System.Windows.Forms.Label();
            this.PROCS_P4 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_JUMP_CURSOL_CODE = new System.Windows.Forms.Label();
            this.PROCS_NAME = new FEBuilderGBA.TextBoxEx();
            this.PROCS_P0 = new System.Windows.Forms.NumericUpDown();
            this.PROCS_J_0_PROCS = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ProcsListBox = new FEBuilderGBA.ListBoxEx();
            this.EtcPage = new System.Windows.Forms.TabPage();
            this.PartyCombo = new FEBuilderGBA.ComboBoxEx();
            this.PartyCount = new System.Windows.Forms.Label();
            this.tabControlEtc = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.J_ACTIVEUNIT = new System.Windows.Forms.Label();
            this.ETC_UNIT_MEMORY_AND_NAME = new FEBuilderGBA.TextBoxEx();
            this.ETC_UNIT_MEMORY_AND_ICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.label58 = new System.Windows.Forms.Label();
            this.N_J_14_MAP = new System.Windows.Forms.Label();
            this.X_ETC_WorldmapNode_Text = new FEBuilderGBA.TextBoxEx();
            this.BGMName = new FEBuilderGBA.TextBoxEx();
            this.X_ETC_Edition_Text = new FEBuilderGBA.TextBoxEx();
            this.N_B14 = new System.Windows.Forms.NumericUpDown();
            this.X_ETC_Diffculty_Text = new FEBuilderGBA.TextBoxEx();
            this.N_L_14_MAP = new FEBuilderGBA.TextBoxEx();
            this.BGM = new System.Windows.Forms.NumericUpDown();
            this.J_BGM = new System.Windows.Forms.Label();
            this.SoundAddress = new FEBuilderGBA.TextBoxEx();
            this.SoundList = new FEBuilderGBA.ListBoxEx();
            this.label91 = new System.Windows.Forms.Label();
            this.tabPageTrapData = new System.Windows.Forms.TabPage();
            this.TrapList = new FEBuilderGBA.ListBoxEx();
            this.TrapAddress = new FEBuilderGBA.TextBoxEx();
            this.label62 = new System.Windows.Forms.Label();
            this.tabPagePalette = new System.Windows.Forms.TabPage();
            this.PaletteList = new FEBuilderGBA.ListBoxEx();
            this.PaletteSearchButton = new System.Windows.Forms.Button();
            this.PaletteAddress = new FEBuilderGBA.TextBoxEx();
            this.SelectPalette = new FEBuilderGBA.TextBoxEx();
            this.label92 = new System.Windows.Forms.Label();
            this.tabPageClearTurns = new System.Windows.Forms.TabPage();
            this.ClearTurnList = new FEBuilderGBA.ListBoxEx();
            this.ClearTurnAddress = new FEBuilderGBA.TextBoxEx();
            this.label82 = new System.Windows.Forms.Label();
            this.tabPageBWL = new System.Windows.Forms.TabPage();
            this.BWLList = new FEBuilderGBA.ListBoxEx();
            this.BWLAddress = new FEBuilderGBA.TextBoxEx();
            this.label95 = new System.Windows.Forms.Label();
            this.tabPageChapterData = new System.Windows.Forms.TabPage();
            this.ChapterDataList = new FEBuilderGBA.ListBoxEx();
            this.ChapterDataAddress = new FEBuilderGBA.TextBoxEx();
            this.label96 = new System.Windows.Forms.Label();
            this.tabPageSupplyData = new System.Windows.Forms.TabPage();
            this.SupplyDataAddress = new FEBuilderGBA.TextBoxEx();
            this.SupplyDataList = new FEBuilderGBA.ListBoxEx();
            this.label102 = new System.Windows.Forms.Label();
            this.tabPageActionData = new System.Windows.Forms.TabPage();
            this.ActionDataAddress = new FEBuilderGBA.TextBoxEx();
            this.ActionDataList = new FEBuilderGBA.ListBoxEx();
            this.label103 = new System.Windows.Forms.Label();
            this.tabPageArenaData = new System.Windows.Forms.TabPage();
            this.ArenaDataAddress = new FEBuilderGBA.TextBoxEx();
            this.ArenaDataList = new FEBuilderGBA.ListBoxEx();
            this.label100 = new System.Windows.Forms.Label();
            this.tabPageBattleActor = new System.Windows.Forms.TabPage();
            this.BattleActorList = new FEBuilderGBA.ListBoxEx();
            this.BattleActorAddress = new FEBuilderGBA.TextBoxEx();
            this.label97 = new System.Windows.Forms.Label();
            this.tabPageBattleTarget = new System.Windows.Forms.TabPage();
            this.BattleTargetList = new FEBuilderGBA.ListBoxEx();
            this.BattleTargetAddress = new FEBuilderGBA.TextBoxEx();
            this.label98 = new System.Windows.Forms.Label();
            this.tabPageAIData = new System.Windows.Forms.TabPage();
            this.AIDataAddress = new FEBuilderGBA.TextBoxEx();
            this.AIDataList = new FEBuilderGBA.ListBoxEx();
            this.label101 = new System.Windows.Forms.Label();
            this.tabPageBattleRound = new System.Windows.Forms.TabPage();
            this.BattleRoundDataAddress = new FEBuilderGBA.TextBoxEx();
            this.BattleRoundDataList = new FEBuilderGBA.ListBoxEx();
            this.label106 = new System.Windows.Forms.Label();
            this.tabPageBattleSome = new System.Windows.Forms.TabPage();
            this.BattleSomeDataAddress = new FEBuilderGBA.TextBoxEx();
            this.BattleSomeDataList = new FEBuilderGBA.ListBoxEx();
            this.label105 = new System.Windows.Forms.Label();
            this.tabPageWorldmap = new System.Windows.Forms.TabPage();
            this.WorldmapAddress = new FEBuilderGBA.TextBoxEx();
            this.WorldmapList = new FEBuilderGBA.ListBoxEx();
            this.label99 = new System.Windows.Forms.Label();
            this.tabPageDungeon = new System.Windows.Forms.TabPage();
            this.DungeonDataAddress = new FEBuilderGBA.TextBoxEx();
            this.DungeonDataList = new FEBuilderGBA.ListBoxEx();
            this.label104 = new System.Windows.Forms.Label();
            this.Party_ControlPanel = new System.Windows.Forms.Panel();
            this.Party_CloseButton = new System.Windows.Forms.Button();
            this.PARTY_Address = new System.Windows.Forms.NumericUpDown();
            this.PARTY_SelectAddress = new FEBuilderGBA.TextBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.PARTY_AI2_TEXT = new FEBuilderGBA.TextBoxEx();
            this.PARTY_AI1_TEXT = new FEBuilderGBA.TextBoxEx();
            this.PARTY_PORTRAIT = new FEBuilderGBA.InterpolatedPictureBox();
            this.PARTY_L_38_ITEMICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.PARTY_L_36_ITEMICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.PARTY_L_34_ITEMICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.PARTY_L_32_ITEMICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.PARTY_L_30_ITEMICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.PARTY_B71 = new System.Windows.Forms.NumericUpDown();
            this.label89 = new System.Windows.Forms.Label();
            this.PARTY_B70 = new System.Windows.Forms.NumericUpDown();
            this.label90 = new System.Windows.Forms.Label();
            this.PARTY_B69 = new System.Windows.Forms.NumericUpDown();
            this.label83 = new System.Windows.Forms.Label();
            this.PARTY_B68 = new System.Windows.Forms.NumericUpDown();
            this.label84 = new System.Windows.Forms.Label();
            this.PARTY_B67 = new System.Windows.Forms.NumericUpDown();
            this.label85 = new System.Windows.Forms.Label();
            this.PARTY_B66 = new System.Windows.Forms.NumericUpDown();
            this.label86 = new System.Windows.Forms.Label();
            this.PARTY_B65 = new System.Windows.Forms.NumericUpDown();
            this.label87 = new System.Windows.Forms.Label();
            this.PARTY_B64 = new System.Windows.Forms.NumericUpDown();
            this.label88 = new System.Windows.Forms.Label();
            this.PARTY_B63 = new System.Windows.Forms.NumericUpDown();
            this.label69 = new System.Windows.Forms.Label();
            this.PARTY_B62 = new System.Windows.Forms.NumericUpDown();
            this.label78 = new System.Windows.Forms.Label();
            this.PARTY_B61 = new System.Windows.Forms.NumericUpDown();
            this.label79 = new System.Windows.Forms.Label();
            this.PARTY_B60 = new System.Windows.Forms.NumericUpDown();
            this.label80 = new System.Windows.Forms.Label();
            this.PARTY_B59 = new System.Windows.Forms.NumericUpDown();
            this.label81 = new System.Windows.Forms.Label();
            this.PARTY_B58 = new System.Windows.Forms.NumericUpDown();
            this.PARTY_J_58 = new System.Windows.Forms.Label();
            this.PARTY_B57 = new System.Windows.Forms.NumericUpDown();
            this.label70 = new System.Windows.Forms.Label();
            this.PARTY_B56 = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.PARTY_B55 = new System.Windows.Forms.NumericUpDown();
            this.label72 = new System.Windows.Forms.Label();
            this.PARTY_B54 = new System.Windows.Forms.NumericUpDown();
            this.label73 = new System.Windows.Forms.Label();
            this.PARTY_B53 = new System.Windows.Forms.NumericUpDown();
            this.label74 = new System.Windows.Forms.Label();
            this.PARTY_B52 = new System.Windows.Forms.NumericUpDown();
            this.label75 = new System.Windows.Forms.Label();
            this.PARTY_B51 = new System.Windows.Forms.NumericUpDown();
            this.label76 = new System.Windows.Forms.Label();
            this.PARTY_B50 = new System.Windows.Forms.NumericUpDown();
            this.label77 = new System.Windows.Forms.Label();
            this.PARTY_B49 = new System.Windows.Forms.NumericUpDown();
            this.label67 = new System.Windows.Forms.Label();
            this.PARTY_B48 = new System.Windows.Forms.NumericUpDown();
            this.label68 = new System.Windows.Forms.Label();
            this.PARTY_B47 = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.PARTY_B46 = new System.Windows.Forms.NumericUpDown();
            this.label47 = new System.Windows.Forms.Label();
            this.PARTY_B45 = new System.Windows.Forms.NumericUpDown();
            this.label49 = new System.Windows.Forms.Label();
            this.PARTY_B44 = new System.Windows.Forms.NumericUpDown();
            this.label51 = new System.Windows.Forms.Label();
            this.PARTY_B43 = new System.Windows.Forms.NumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.PARTY_B42 = new System.Windows.Forms.NumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.PARTY_B41 = new System.Windows.Forms.NumericUpDown();
            this.label41 = new System.Windows.Forms.Label();
            this.PARTY_B40 = new System.Windows.Forms.NumericUpDown();
            this.label38 = new System.Windows.Forms.Label();
            this.PARTY_L_38_ITEM = new FEBuilderGBA.TextBoxEx();
            this.PARTY_B39 = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.PARTY_B38 = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.PARTY_L_36_ITEM = new FEBuilderGBA.TextBoxEx();
            this.PARTY_B37 = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.PARTY_B36 = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.PARTY_L_34_ITEM = new FEBuilderGBA.TextBoxEx();
            this.PARTY_B35 = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.PARTY_B34 = new System.Windows.Forms.NumericUpDown();
            this.label32 = new System.Windows.Forms.Label();
            this.PARTY_L_32_ITEM = new FEBuilderGBA.TextBoxEx();
            this.PARTY_B33 = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.PARTY_B32 = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.PARTY_L_30_ITEM = new FEBuilderGBA.TextBoxEx();
            this.PARTY_B31 = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.PARTY_B30 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.PARTY_B28 = new System.Windows.Forms.NumericUpDown();
            this.label66 = new System.Windows.Forms.Label();
            this.PARTY_B29 = new System.Windows.Forms.NumericUpDown();
            this.label65 = new System.Windows.Forms.Label();
            this.PARTY_RAMUNITAID = new FEBuilderGBA.TextBoxEx();
            this.PARTY_B27 = new System.Windows.Forms.NumericUpDown();
            this.label64 = new System.Windows.Forms.Label();
            this.PARTY_B26 = new System.Windows.Forms.NumericUpDown();
            this.PARTY_J_26 = new System.Windows.Forms.Label();
            this.PARTY_B25 = new System.Windows.Forms.NumericUpDown();
            this.label63 = new System.Windows.Forms.Label();
            this.PARTY_B24 = new System.Windows.Forms.NumericUpDown();
            this.label60 = new System.Windows.Forms.Label();
            this.PARTY_B23 = new System.Windows.Forms.NumericUpDown();
            this.label61 = new System.Windows.Forms.Label();
            this.PARTY_B22 = new System.Windows.Forms.NumericUpDown();
            this.label59 = new System.Windows.Forms.Label();
            this.PARTY_B21 = new System.Windows.Forms.NumericUpDown();
            this.label57 = new System.Windows.Forms.Label();
            this.PARTY_RAMUNITSTATE = new FEBuilderGBA.TextBoxEx();
            this.PARTY_ROMCLASSPOINTER = new FEBuilderGBA.TextBoxEx();
            this.PARTY_B10 = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxEx52 = new FEBuilderGBA.TextBoxEx();
            this.numericUpDown59 = new System.Windows.Forms.NumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.textBoxEx53 = new FEBuilderGBA.TextBoxEx();
            this.numericUpDown61 = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.textBoxEx54 = new FEBuilderGBA.TextBoxEx();
            this.numericUpDown64 = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.PARTY_B19 = new System.Windows.Forms.NumericUpDown();
            this.label36 = new System.Windows.Forms.Label();
            this.PARTY_B20 = new System.Windows.Forms.NumericUpDown();
            this.label39 = new System.Windows.Forms.Label();
            this.PARTY_B18 = new System.Windows.Forms.NumericUpDown();
            this.label40 = new System.Windows.Forms.Label();
            this.PARTY_B17 = new System.Windows.Forms.NumericUpDown();
            this.label43 = new System.Windows.Forms.Label();
            this.PARTY_B16 = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.PARTY_D12 = new System.Windows.Forms.NumericUpDown();
            this.label48 = new System.Windows.Forms.Label();
            this.PARTY_B11 = new System.Windows.Forms.NumericUpDown();
            this.label50 = new System.Windows.Forms.Label();
            this.PARTY_B9 = new System.Windows.Forms.NumericUpDown();
            this.label52 = new System.Windows.Forms.Label();
            this.PARTY_B8 = new System.Windows.Forms.NumericUpDown();
            this.label53 = new System.Windows.Forms.Label();
            this.PARTY_P4 = new System.Windows.Forms.NumericUpDown();
            this.label54 = new System.Windows.Forms.Label();
            this.PARTY_ROMUNITPOINTER = new FEBuilderGBA.TextBoxEx();
            this.PARTY_P0 = new System.Windows.Forms.NumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.PartyListBox = new FEBuilderGBA.ListBoxEx();
            this.label56 = new System.Windows.Forms.Label();
            this.CheatPage = new System.Windows.Forms.TabPage();
            this.label94 = new System.Windows.Forms.Label();
            this.CHEAT_TURN_VALUE = new System.Windows.Forms.NumericUpDown();
            this.CHEAT_TURN = new System.Windows.Forms.Button();
            this.CHEAT_WARP_NODE_VALUE = new System.Windows.Forms.NumericUpDown();
            this.CHEAT_WARP_NODE_JUMP = new System.Windows.Forms.Label();
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE = new System.Windows.Forms.Button();
            this.CHEAT_WARP_EDTION_VALUE = new System.Windows.Forms.NumericUpDown();
            this.CHEAT_WARP_EDTION_JUMP = new System.Windows.Forms.Label();
            this.CHEAT_WARP_CHPATER_VALUE = new System.Windows.Forms.NumericUpDown();
            this.CHEAT_WARP_CHPATER_JUMP = new System.Windows.Forms.Label();
            this.CHEAT_WARP = new System.Windows.Forms.Button();
            this.CHEAT_ALL_ENEMY_UNIT_HP_1 = new System.Windows.Forms.Button();
            this.CHEAT_ALL_UNIT_GROW = new System.Windows.Forms.Button();
            this.CHEAT_ALL_PLAYER_UNIT_GROW = new System.Windows.Forms.Button();
            this.label35 = new System.Windows.Forms.Label();
            this.CHEAT_WEATHER_VALUE = new System.Windows.Forms.NumericUpDown();
            this.CHEAT_WEATHER_COMBO = new System.Windows.Forms.ComboBox();
            this.CHEAT_WEATHER = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.CHEAT_FOG_VALUE = new System.Windows.Forms.NumericUpDown();
            this.CHEAT_ITEM_COUNT = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.CHEAT_ITEM_ID = new System.Windows.Forms.NumericUpDown();
            this.CHEAT_MONEY_VALUE = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.Dump03Button = new System.Windows.Forms.Button();
            this.Dump02Button = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.CHEAT_FOG = new System.Windows.Forms.Button();
            this.CHEAT_MONEY = new System.Windows.Forms.Button();
            this.CHEAT_ITEM_JUMP = new System.Windows.Forms.Label();
            this.CHEAT_UNIT_HAVE_ITEM = new System.Windows.Forms.Button();
            this.CHEAT_UNIT_HP_1 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.CHEAT_UNIT_GROW = new System.Windows.Forms.Button();
            this.CHEAT_SET_FLAG03 = new System.Windows.Forms.Button();
            this.CHEAT_WARP_NODE_NAME = new FEBuilderGBA.TextBoxEx();
            this.systemIconPictureBox5 = new FEBuilderGBA.SystemIconPictureBox();
            this.CHEAT_WARP_EDTION_NAME = new FEBuilderGBA.TextBoxEx();
            this.CHEAT_WARP_CHPATER_NAME = new FEBuilderGBA.TextBoxEx();
            this.systemIconPictureBox4 = new FEBuilderGBA.SystemIconPictureBox();
            this.CHEAT_UNIT_MEMORY_AND_NAME = new FEBuilderGBA.TextBoxEx();
            this.CHEAT_ITEM_NAME = new FEBuilderGBA.TextBoxEx();
            this.systemIconPictureBox3 = new FEBuilderGBA.SystemIconPictureBox();
            this.systemIconPictureBox2 = new FEBuilderGBA.SystemIconPictureBox();
            this.CHEAT_UNIT_MEMORY_AND_ICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.systemIconPictureBox1 = new FEBuilderGBA.SystemIconPictureBox();
            this.CHEAT_ITEM_ICON = new FEBuilderGBA.InterpolatedPictureBox();
            this.textBoxEx1 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx2 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx3 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx4 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx5 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx6 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx7 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx8 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx9 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx10 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx11 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx12 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx13 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx14 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx15 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx19 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx20 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx21 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx22 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx23 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx24 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx25 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx26 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx27 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx28 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx29 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx30 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx31 = new FEBuilderGBA.TextBoxEx();
            this.textBoxEx32 = new FEBuilderGBA.TextBoxEx();
            this.panel1.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.EventPage.SuspendLayout();
            this.EventHistoryPage.SuspendLayout();
            this.ProcPage.SuspendLayout();
            this.Proc_ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_Address)).BeginInit();
            this.ControlPanelCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B107)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B51)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B59)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B67)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B99)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B75)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B91)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B83)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B106)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B50)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B58)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B66)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B98)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B74)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B90)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B82)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B105)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B49)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B57)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B65)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B97)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B73)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B89)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B81)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B47)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B55)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B63)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B103)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B71)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B95)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B79)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B87)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B46)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B54)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B62)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B102)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B70)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B94)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B78)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B86)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B45)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B53)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B61)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B101)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B69)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B93)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B77)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B85)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B41)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B43)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B42)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B104)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B48)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B52)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B56)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B60)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B40)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B64)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B39)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B100)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B38)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B68)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_W36)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B96)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B72)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B92)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B76)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B88)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B80)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B84)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P0)).BeginInit();
            this.EtcPage.SuspendLayout();
            this.tabControlEtc.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ETC_UNIT_MEMORY_AND_ICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BGM)).BeginInit();
            this.tabPageTrapData.SuspendLayout();
            this.tabPagePalette.SuspendLayout();
            this.tabPageClearTurns.SuspendLayout();
            this.tabPageBWL.SuspendLayout();
            this.tabPageChapterData.SuspendLayout();
            this.tabPageSupplyData.SuspendLayout();
            this.tabPageActionData.SuspendLayout();
            this.tabPageArenaData.SuspendLayout();
            this.tabPageBattleActor.SuspendLayout();
            this.tabPageBattleTarget.SuspendLayout();
            this.tabPageAIData.SuspendLayout();
            this.tabPageBattleRound.SuspendLayout();
            this.tabPageBattleSome.SuspendLayout();
            this.tabPageWorldmap.SuspendLayout();
            this.tabPageDungeon.SuspendLayout();
            this.Party_ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_Address)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_PORTRAIT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_38_ITEMICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_36_ITEMICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_34_ITEMICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_32_ITEMICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_30_ITEMICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B71)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B70)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B69)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B68)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B67)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B66)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B65)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B64)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B63)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B62)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B61)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B60)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B59)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B58)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B57)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B56)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B55)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B54)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B53)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B52)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B51)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B50)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B49)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B48)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B47)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B46)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B45)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B43)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B42)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B41)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B40)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B39)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B38)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B37)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B36)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B35)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B34)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B30)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B29)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown59)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown61)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown64)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_D12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_P4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_P0)).BeginInit();
            this.CheatPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_TURN_VALUE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WARP_NODE_VALUE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WARP_EDTION_VALUE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WARP_CHPATER_VALUE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WEATHER_VALUE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_FOG_VALUE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_ITEM_COUNT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_ITEM_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_MONEY_VALUE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_UNIT_MEMORY_AND_ICON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_ITEM_ICON)).BeginInit();
            this.SuspendLayout();
            // 
            // AutoUpdateCheckBox
            // 
            this.AutoUpdateCheckBox.AutoSize = true;
            this.AutoUpdateCheckBox.Checked = true;
            this.AutoUpdateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoUpdateCheckBox.Location = new System.Drawing.Point(851, 5);
            this.AutoUpdateCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.AutoUpdateCheckBox.Name = "AutoUpdateCheckBox";
            this.AutoUpdateCheckBox.Size = new System.Drawing.Size(167, 22);
            this.AutoUpdateCheckBox.TabIndex = 1;
            this.AutoUpdateCheckBox.Text = "自動的に更新する";
            this.AutoUpdateCheckBox.UseVisualStyleBackColor = true;
            this.AutoUpdateCheckBox.CheckedChanged += new System.EventHandler(this.AutoUpdateCheckBox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ERROR_EMU_CONNECT);
            this.panel1.Controls.Add(this.AutoUpdateCheckBox);
            this.panel1.Location = new System.Drawing.Point(596, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1059, 30);
            this.panel1.TabIndex = 64;
            // 
            // ERROR_EMU_CONNECT
            // 
            this.ERROR_EMU_CONNECT.AutoSize = true;
            this.ERROR_EMU_CONNECT.Location = new System.Drawing.Point(2, 4);
            this.ERROR_EMU_CONNECT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ERROR_EMU_CONNECT.Name = "ERROR_EMU_CONNECT";
            this.ERROR_EMU_CONNECT.Size = new System.Drawing.Size(64, 18);
            this.ERROR_EMU_CONNECT.TabIndex = 2;
            this.ERROR_EMU_CONNECT.Text = "ERROR";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.EventPage);
            this.MainTabControl.Controls.Add(this.EventHistoryPage);
            this.MainTabControl.Controls.Add(this.ProcPage);
            this.MainTabControl.Controls.Add(this.EtcPage);
            this.MainTabControl.Controls.Add(this.CheatPage);
            this.MainTabControl.Location = new System.Drawing.Point(6, 5);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1654, 920);
            this.MainTabControl.TabIndex = 67;
            // 
            // EventPage
            // 
            this.EventPage.BackColor = System.Drawing.SystemColors.Control;
            this.EventPage.Controls.Add(this.SubtileButton);
            this.EventPage.Controls.Add(this.SpeechButton);
            this.EventPage.Controls.Add(this.CurrentTextBox);
            this.EventPage.Controls.Add(this.label93);
            this.EventPage.Controls.Add(this.N_SelectAddress);
            this.EventPage.Controls.Add(this.RunningEventListBox);
            this.EventPage.Controls.Add(this.RunningEventListBoxLabel);
            this.EventPage.Controls.Add(this.FlagListBox);
            this.EventPage.Controls.Add(this.N1_LabelFilter);
            this.EventPage.Controls.Add(this.MemorySlotListBox);
            this.EventPage.Controls.Add(this.MemorySlotLabel);
            this.EventPage.Location = new System.Drawing.Point(4, 28);
            this.EventPage.Margin = new System.Windows.Forms.Padding(2);
            this.EventPage.Name = "EventPage";
            this.EventPage.Padding = new System.Windows.Forms.Padding(2);
            this.EventPage.Size = new System.Drawing.Size(1646, 888);
            this.EventPage.TabIndex = 0;
            this.EventPage.Text = "イベント";
            // 
            // SubtileButton
            // 
            this.SubtileButton.Location = new System.Drawing.Point(537, -2);
            this.SubtileButton.Name = "SubtileButton";
            this.SubtileButton.Size = new System.Drawing.Size(46, 32);
            this.SubtileButton.TabIndex = 311;
            this.SubtileButton.UseVisualStyleBackColor = true;
            this.SubtileButton.Click += new System.EventHandler(this.SubtileButton_Click);
            // 
            // SpeechButton
            // 
            this.SpeechButton.Location = new System.Drawing.Point(490, -2);
            this.SpeechButton.Name = "SpeechButton";
            this.SpeechButton.Size = new System.Drawing.Size(46, 32);
            this.SpeechButton.TabIndex = 310;
            this.SpeechButton.UseVisualStyleBackColor = true;
            this.SpeechButton.Click += new System.EventHandler(this.SpeechButton_Click);
            // 
            // CurrentTextBox
            // 
            this.CurrentTextBox.ErrorMessage = "";
            this.CurrentTextBox.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CurrentTextBox.Location = new System.Drawing.Point(-1, 29);
            this.CurrentTextBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CurrentTextBox.Name = "CurrentTextBox";
            this.CurrentTextBox.Placeholder = "";
            this.CurrentTextBox.ReadOnly = true;
            this.CurrentTextBox.Size = new System.Drawing.Size(584, 163);
            this.CurrentTextBox.TabIndex = 309;
            this.CurrentTextBox.Text = "";
            this.CurrentTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CurrentTextBox_MouseDoubleClick);
            // 
            // label93
            // 
            this.label93.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label93.Location = new System.Drawing.Point(0, 2);
            this.label93.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(583, 31);
            this.label93.TabIndex = 312;
            this.label93.Text = "Text";
            this.label93.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // N_SelectAddress
            // 
            this.N_SelectAddress.ErrorMessage = "";
            this.N_SelectAddress.Location = new System.Drawing.Point(166, 195);
            this.N_SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.N_SelectAddress.Name = "N_SelectAddress";
            this.N_SelectAddress.Placeholder = "";
            this.N_SelectAddress.ReadOnly = true;
            this.N_SelectAddress.Size = new System.Drawing.Size(240, 25);
            this.N_SelectAddress.TabIndex = 307;
            // 
            // RunningEventListBox
            // 
            this.RunningEventListBox.FormattingEnabled = true;
            this.RunningEventListBox.HorizontalScrollbar = true;
            this.RunningEventListBox.IntegralHeight = false;
            this.RunningEventListBox.ItemHeight = 18;
            this.RunningEventListBox.Location = new System.Drawing.Point(588, 30);
            this.RunningEventListBox.Margin = new System.Windows.Forms.Padding(2);
            this.RunningEventListBox.Name = "RunningEventListBox";
            this.RunningEventListBox.Size = new System.Drawing.Size(1058, 850);
            this.RunningEventListBox.TabIndex = 168;
            this.RunningEventListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RunningEventListBox_KeyDown);
            this.RunningEventListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RunningEventListBox_MouseDoubleClick);
            // 
            // RunningEventListBoxLabel
            // 
            this.RunningEventListBoxLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RunningEventListBoxLabel.Location = new System.Drawing.Point(587, 0);
            this.RunningEventListBoxLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RunningEventListBoxLabel.Name = "RunningEventListBoxLabel";
            this.RunningEventListBoxLabel.Size = new System.Drawing.Size(1059, 31);
            this.RunningEventListBoxLabel.TabIndex = 169;
            this.RunningEventListBoxLabel.Text = "実行しているイベント";
            this.RunningEventListBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RunningEventListBoxLabel.Click += new System.EventHandler(this.J_CurrentEventAddress_Click);
            // 
            // FlagListBox
            // 
            this.FlagListBox.FormattingEnabled = true;
            this.FlagListBox.IntegralHeight = false;
            this.FlagListBox.ItemHeight = 18;
            this.FlagListBox.Location = new System.Drawing.Point(0, 220);
            this.FlagListBox.Margin = new System.Windows.Forms.Padding(2);
            this.FlagListBox.Name = "FlagListBox";
            this.FlagListBox.Size = new System.Drawing.Size(407, 660);
            this.FlagListBox.TabIndex = 16;
            this.FlagListBox.SelectedIndexChanged += new System.EventHandler(this.FlagListBox_SelectedIndexChanged);
            this.FlagListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FlagListBox_KeyDown);
            this.FlagListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FlagListBox_MouseDoubleClick);
            // 
            // N1_LabelFilter
            // 
            this.N1_LabelFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N1_LabelFilter.Location = new System.Drawing.Point(1, 195);
            this.N1_LabelFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N1_LabelFilter.Name = "N1_LabelFilter";
            this.N1_LabelFilter.Size = new System.Drawing.Size(406, 26);
            this.N1_LabelFilter.TabIndex = 108;
            this.N1_LabelFilter.Text = "フラグ";
            this.N1_LabelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MemorySlotListBox
            // 
            this.MemorySlotListBox.FormattingEnabled = true;
            this.MemorySlotListBox.IntegralHeight = false;
            this.MemorySlotListBox.ItemHeight = 18;
            this.MemorySlotListBox.Location = new System.Drawing.Point(410, 220);
            this.MemorySlotListBox.Margin = new System.Windows.Forms.Padding(2);
            this.MemorySlotListBox.Name = "MemorySlotListBox";
            this.MemorySlotListBox.Size = new System.Drawing.Size(173, 660);
            this.MemorySlotListBox.TabIndex = 14;
            this.MemorySlotListBox.SelectedIndexChanged += new System.EventHandler(this.MemorySlotListBox_SelectedIndexChanged);
            // 
            // MemorySlotLabel
            // 
            this.MemorySlotLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MemorySlotLabel.Location = new System.Drawing.Point(409, 195);
            this.MemorySlotLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MemorySlotLabel.Name = "MemorySlotLabel";
            this.MemorySlotLabel.Size = new System.Drawing.Size(174, 26);
            this.MemorySlotLabel.TabIndex = 162;
            this.MemorySlotLabel.Text = "メモリスロット";
            this.MemorySlotLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EventHistoryPage
            // 
            this.EventHistoryPage.BackColor = System.Drawing.SystemColors.Control;
            this.EventHistoryPage.Controls.Add(this.label10);
            this.EventHistoryPage.Controls.Add(this.label7);
            this.EventHistoryPage.Controls.Add(this.label5);
            this.EventHistoryPage.Controls.Add(this.label6);
            this.EventHistoryPage.Controls.Add(this.EventHistoryListBox);
            this.EventHistoryPage.Location = new System.Drawing.Point(4, 28);
            this.EventHistoryPage.Margin = new System.Windows.Forms.Padding(2);
            this.EventHistoryPage.Name = "EventHistoryPage";
            this.EventHistoryPage.Padding = new System.Windows.Forms.Padding(2);
            this.EventHistoryPage.Size = new System.Drawing.Size(1646, 888);
            this.EventHistoryPage.TabIndex = 2;
            this.EventHistoryPage.Text = "イベント履歴";
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(26, 13);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(680, 54);
            this.label10.TabIndex = 114;
            this.label10.Text = "イベント履歴(上が最新)\r\n時刻,イベント開始アドレス,実行中のアドレス,イベント";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(710, 13);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(928, 56);
            this.label7.TabIndex = 113;
            this.label7.Text = "定期的にエミュレータから取得したデータから記録したイベントの呼び出し履歴です。\r\n変更があった部分のみ記録しています。ダブルクリックでイベントエディタを開きます。" +
    "";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(5, 70);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 208);
            this.label5.TabIndex = 111;
            this.label5.Text = "↑最新";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(5, 680);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 204);
            this.label6.TabIndex = 112;
            this.label6.Text = "より古い↓";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EventHistoryListBox
            // 
            this.EventHistoryListBox.FormattingEnabled = true;
            this.EventHistoryListBox.HorizontalScrollbar = true;
            this.EventHistoryListBox.IntegralHeight = false;
            this.EventHistoryListBox.ItemHeight = 18;
            this.EventHistoryListBox.Location = new System.Drawing.Point(26, 70);
            this.EventHistoryListBox.Margin = new System.Windows.Forms.Padding(2);
            this.EventHistoryListBox.Name = "EventHistoryListBox";
            this.EventHistoryListBox.Size = new System.Drawing.Size(1613, 814);
            this.EventHistoryListBox.TabIndex = 109;
            this.EventHistoryListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EventHistoryListBox_KeyDown);
            this.EventHistoryListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.EventHistoryListBox_MouseDoubleClick);
            // 
            // ProcPage
            // 
            this.ProcPage.BackColor = System.Drawing.SystemColors.Control;
            this.ProcPage.Controls.Add(this.Proc_ControlPanel);
            this.ProcPage.Controls.Add(this.label1);
            this.ProcPage.Controls.Add(this.ProcsListBox);
            this.ProcPage.Location = new System.Drawing.Point(4, 28);
            this.ProcPage.Margin = new System.Windows.Forms.Padding(2);
            this.ProcPage.Name = "ProcPage";
            this.ProcPage.Size = new System.Drawing.Size(1646, 888);
            this.ProcPage.TabIndex = 1;
            this.ProcPage.Text = "Procs";
            // 
            // Proc_ControlPanel
            // 
            this.Proc_ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Proc_ControlPanel.Controls.Add(this.PROCS_Address);
            this.Proc_ControlPanel.Controls.Add(this.PROCS_SelectAddress);
            this.Proc_ControlPanel.Controls.Add(this.label22);
            this.Proc_ControlPanel.Controls.Add(this.PROCS_AddressLabel);
            this.Proc_ControlPanel.Controls.Add(this.ControlPanelCommand);
            this.Proc_ControlPanel.Controls.Add(this.CloseButton);
            this.Proc_ControlPanel.Location = new System.Drawing.Point(14, 241);
            this.Proc_ControlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.Proc_ControlPanel.Name = "Proc_ControlPanel";
            this.Proc_ControlPanel.Size = new System.Drawing.Size(1024, 639);
            this.Proc_ControlPanel.TabIndex = 162;
            this.Proc_ControlPanel.Visible = false;
            // 
            // PROCS_Address
            // 
            this.PROCS_Address.Hexadecimal = true;
            this.PROCS_Address.Location = new System.Drawing.Point(197, 5);
            this.PROCS_Address.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_Address.Name = "PROCS_Address";
            this.PROCS_Address.ReadOnly = true;
            this.PROCS_Address.Size = new System.Drawing.Size(161, 25);
            this.PROCS_Address.TabIndex = 304;
            // 
            // PROCS_SelectAddress
            // 
            this.PROCS_SelectAddress.ErrorMessage = "";
            this.PROCS_SelectAddress.Location = new System.Drawing.Point(490, 4);
            this.PROCS_SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.PROCS_SelectAddress.Name = "PROCS_SelectAddress";
            this.PROCS_SelectAddress.Placeholder = "";
            this.PROCS_SelectAddress.ReadOnly = true;
            this.PROCS_SelectAddress.Size = new System.Drawing.Size(210, 25);
            this.PROCS_SelectAddress.TabIndex = 303;
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(364, 2);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 30);
            this.label22.TabIndex = 302;
            this.label22.Text = "選択アドレス:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PROCS_AddressLabel
            // 
            this.PROCS_AddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PROCS_AddressLabel.Location = new System.Drawing.Point(5, 4);
            this.PROCS_AddressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_AddressLabel.Name = "PROCS_AddressLabel";
            this.PROCS_AddressLabel.Size = new System.Drawing.Size(187, 24);
            this.PROCS_AddressLabel.TabIndex = 301;
            this.PROCS_AddressLabel.Text = "アドレス";
            this.PROCS_AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ControlPanelCommand
            // 
            this.ControlPanelCommand.Controls.Add(this.PROCS_B107);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B51);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B59);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B67);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B99);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B75);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B91);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B83);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B106);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B50);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B58);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B66);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B98);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B74);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B90);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B82);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B105);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B49);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B57);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B65);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B97);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B73);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B89);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B81);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B47);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B55);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B63);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B103);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B71);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B95);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B79);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B87);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B46);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B54);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B62);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B102);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B70);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B94);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B78);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B86);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B45);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B53);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B61);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B101);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B69);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B93);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B77);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B85);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_104_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_96_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_88_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_80_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_72_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_64_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_100_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_92_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_84_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_76_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_68_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_60_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_56_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_52_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_48_DWORD);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_44_DWORD);
            this.ControlPanelCommand.Controls.Add(this.label8);
            this.ControlPanelCommand.Controls.Add(this.label3);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B41);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B43);
            this.ControlPanelCommand.Controls.Add(this.PROCS_CURSOL_CODE);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B42);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_16_TEXT);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_44);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P16);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B104);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_16_TEXT);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B44);
            this.ControlPanelCommand.Controls.Add(this.textBoxEx16);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_104);
            this.ControlPanelCommand.Controls.Add(this.numericUpDown17);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_48);
            this.ControlPanelCommand.Controls.Add(this.label25);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B48);
            this.ControlPanelCommand.Controls.Add(this.textBoxEx17);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_52);
            this.ControlPanelCommand.Controls.Add(this.numericUpDown18);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B52);
            this.ControlPanelCommand.Controls.Add(this.label26);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_56);
            this.ControlPanelCommand.Controls.Add(this.textBoxEx18);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B56);
            this.ControlPanelCommand.Controls.Add(this.numericUpDown19);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_60);
            this.ControlPanelCommand.Controls.Add(this.label27);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B60);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B40);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_64);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_40);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B64);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B39);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B100);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_39);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_68);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B38);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_100);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_38);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B68);
            this.ControlPanelCommand.Controls.Add(this.PROCS_W36);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B96);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_36);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_72);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_32_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_96);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P32);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B72);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_32_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B92);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_28_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_76);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P28);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_92);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_28_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B76);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_24_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B88);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P24);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_80);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_24_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_88);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_20_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B80);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P20);
            this.ControlPanelCommand.Controls.Add(this.PROCS_B84);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_20_RAMPROCS);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_84);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_12_ASM);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P12);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_12_ASM);
            this.ControlPanelCommand.Controls.Add(this.PROCS_L_8_ASM);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P8);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_8_ASM);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P4);
            this.ControlPanelCommand.Controls.Add(this.PROCS_JUMP_CURSOL_CODE);
            this.ControlPanelCommand.Controls.Add(this.PROCS_NAME);
            this.ControlPanelCommand.Controls.Add(this.PROCS_P0);
            this.ControlPanelCommand.Controls.Add(this.PROCS_J_0_PROCS);
            this.ControlPanelCommand.Location = new System.Drawing.Point(1, 34);
            this.ControlPanelCommand.Margin = new System.Windows.Forms.Padding(4);
            this.ControlPanelCommand.Name = "ControlPanelCommand";
            this.ControlPanelCommand.Size = new System.Drawing.Size(1016, 604);
            this.ControlPanelCommand.TabIndex = 0;
            // 
            // PROCS_B107
            // 
            this.PROCS_B107.Hexadecimal = true;
            this.PROCS_B107.Location = new System.Drawing.Point(838, 572);
            this.PROCS_B107.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B107.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B107.Name = "PROCS_B107";
            this.PROCS_B107.ReadOnly = true;
            this.PROCS_B107.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B107.TabIndex = 120;
            // 
            // PROCS_B51
            // 
            this.PROCS_B51.Hexadecimal = true;
            this.PROCS_B51.Location = new System.Drawing.Point(838, 361);
            this.PROCS_B51.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B51.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B51.Name = "PROCS_B51";
            this.PROCS_B51.ReadOnly = true;
            this.PROCS_B51.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B51.TabIndex = 27;
            // 
            // PROCS_B59
            // 
            this.PROCS_B59.Hexadecimal = true;
            this.PROCS_B59.Location = new System.Drawing.Point(838, 392);
            this.PROCS_B59.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B59.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B59.Name = "PROCS_B59";
            this.PROCS_B59.ReadOnly = true;
            this.PROCS_B59.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B59.TabIndex = 39;
            // 
            // PROCS_B67
            // 
            this.PROCS_B67.Hexadecimal = true;
            this.PROCS_B67.Location = new System.Drawing.Point(838, 422);
            this.PROCS_B67.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B67.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B67.Name = "PROCS_B67";
            this.PROCS_B67.ReadOnly = true;
            this.PROCS_B67.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B67.TabIndex = 52;
            // 
            // PROCS_B99
            // 
            this.PROCS_B99.Hexadecimal = true;
            this.PROCS_B99.Location = new System.Drawing.Point(838, 542);
            this.PROCS_B99.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B99.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B99.Name = "PROCS_B99";
            this.PROCS_B99.ReadOnly = true;
            this.PROCS_B99.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B99.TabIndex = 108;
            // 
            // PROCS_B75
            // 
            this.PROCS_B75.Hexadecimal = true;
            this.PROCS_B75.Location = new System.Drawing.Point(838, 451);
            this.PROCS_B75.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B75.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B75.Name = "PROCS_B75";
            this.PROCS_B75.ReadOnly = true;
            this.PROCS_B75.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B75.TabIndex = 64;
            // 
            // PROCS_B91
            // 
            this.PROCS_B91.Hexadecimal = true;
            this.PROCS_B91.Location = new System.Drawing.Point(838, 509);
            this.PROCS_B91.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B91.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B91.Name = "PROCS_B91";
            this.PROCS_B91.ReadOnly = true;
            this.PROCS_B91.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B91.TabIndex = 90;
            // 
            // PROCS_B83
            // 
            this.PROCS_B83.Hexadecimal = true;
            this.PROCS_B83.Location = new System.Drawing.Point(838, 481);
            this.PROCS_B83.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B83.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B83.Name = "PROCS_B83";
            this.PROCS_B83.ReadOnly = true;
            this.PROCS_B83.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B83.TabIndex = 78;
            // 
            // PROCS_B106
            // 
            this.PROCS_B106.Hexadecimal = true;
            this.PROCS_B106.Location = new System.Drawing.Point(766, 572);
            this.PROCS_B106.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B106.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B106.Name = "PROCS_B106";
            this.PROCS_B106.ReadOnly = true;
            this.PROCS_B106.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B106.TabIndex = 119;
            // 
            // PROCS_B50
            // 
            this.PROCS_B50.Hexadecimal = true;
            this.PROCS_B50.Location = new System.Drawing.Point(766, 361);
            this.PROCS_B50.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B50.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B50.Name = "PROCS_B50";
            this.PROCS_B50.ReadOnly = true;
            this.PROCS_B50.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B50.TabIndex = 26;
            // 
            // PROCS_B58
            // 
            this.PROCS_B58.Hexadecimal = true;
            this.PROCS_B58.Location = new System.Drawing.Point(766, 392);
            this.PROCS_B58.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B58.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B58.Name = "PROCS_B58";
            this.PROCS_B58.ReadOnly = true;
            this.PROCS_B58.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B58.TabIndex = 38;
            // 
            // PROCS_B66
            // 
            this.PROCS_B66.Hexadecimal = true;
            this.PROCS_B66.Location = new System.Drawing.Point(766, 422);
            this.PROCS_B66.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B66.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B66.Name = "PROCS_B66";
            this.PROCS_B66.ReadOnly = true;
            this.PROCS_B66.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B66.TabIndex = 51;
            // 
            // PROCS_B98
            // 
            this.PROCS_B98.Hexadecimal = true;
            this.PROCS_B98.Location = new System.Drawing.Point(766, 542);
            this.PROCS_B98.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B98.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B98.Name = "PROCS_B98";
            this.PROCS_B98.ReadOnly = true;
            this.PROCS_B98.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B98.TabIndex = 107;
            // 
            // PROCS_B74
            // 
            this.PROCS_B74.Hexadecimal = true;
            this.PROCS_B74.Location = new System.Drawing.Point(766, 451);
            this.PROCS_B74.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B74.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B74.Name = "PROCS_B74";
            this.PROCS_B74.ReadOnly = true;
            this.PROCS_B74.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B74.TabIndex = 63;
            // 
            // PROCS_B90
            // 
            this.PROCS_B90.Hexadecimal = true;
            this.PROCS_B90.Location = new System.Drawing.Point(766, 509);
            this.PROCS_B90.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B90.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B90.Name = "PROCS_B90";
            this.PROCS_B90.ReadOnly = true;
            this.PROCS_B90.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B90.TabIndex = 89;
            // 
            // PROCS_B82
            // 
            this.PROCS_B82.Hexadecimal = true;
            this.PROCS_B82.Location = new System.Drawing.Point(766, 481);
            this.PROCS_B82.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B82.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B82.Name = "PROCS_B82";
            this.PROCS_B82.ReadOnly = true;
            this.PROCS_B82.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B82.TabIndex = 77;
            // 
            // PROCS_B105
            // 
            this.PROCS_B105.Hexadecimal = true;
            this.PROCS_B105.Location = new System.Drawing.Point(692, 572);
            this.PROCS_B105.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B105.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B105.Name = "PROCS_B105";
            this.PROCS_B105.ReadOnly = true;
            this.PROCS_B105.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B105.TabIndex = 118;
            // 
            // PROCS_B49
            // 
            this.PROCS_B49.Hexadecimal = true;
            this.PROCS_B49.Location = new System.Drawing.Point(692, 361);
            this.PROCS_B49.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B49.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B49.Name = "PROCS_B49";
            this.PROCS_B49.ReadOnly = true;
            this.PROCS_B49.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B49.TabIndex = 25;
            // 
            // PROCS_B57
            // 
            this.PROCS_B57.Hexadecimal = true;
            this.PROCS_B57.Location = new System.Drawing.Point(692, 392);
            this.PROCS_B57.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B57.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B57.Name = "PROCS_B57";
            this.PROCS_B57.ReadOnly = true;
            this.PROCS_B57.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B57.TabIndex = 37;
            // 
            // PROCS_B65
            // 
            this.PROCS_B65.Hexadecimal = true;
            this.PROCS_B65.Location = new System.Drawing.Point(692, 422);
            this.PROCS_B65.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B65.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B65.Name = "PROCS_B65";
            this.PROCS_B65.ReadOnly = true;
            this.PROCS_B65.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B65.TabIndex = 50;
            // 
            // PROCS_B97
            // 
            this.PROCS_B97.Hexadecimal = true;
            this.PROCS_B97.Location = new System.Drawing.Point(692, 542);
            this.PROCS_B97.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B97.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B97.Name = "PROCS_B97";
            this.PROCS_B97.ReadOnly = true;
            this.PROCS_B97.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B97.TabIndex = 106;
            // 
            // PROCS_B73
            // 
            this.PROCS_B73.Hexadecimal = true;
            this.PROCS_B73.Location = new System.Drawing.Point(692, 451);
            this.PROCS_B73.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B73.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B73.Name = "PROCS_B73";
            this.PROCS_B73.ReadOnly = true;
            this.PROCS_B73.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B73.TabIndex = 62;
            // 
            // PROCS_B89
            // 
            this.PROCS_B89.Hexadecimal = true;
            this.PROCS_B89.Location = new System.Drawing.Point(692, 509);
            this.PROCS_B89.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B89.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B89.Name = "PROCS_B89";
            this.PROCS_B89.ReadOnly = true;
            this.PROCS_B89.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B89.TabIndex = 88;
            // 
            // PROCS_B81
            // 
            this.PROCS_B81.Hexadecimal = true;
            this.PROCS_B81.Location = new System.Drawing.Point(692, 481);
            this.PROCS_B81.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B81.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B81.Name = "PROCS_B81";
            this.PROCS_B81.ReadOnly = true;
            this.PROCS_B81.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B81.TabIndex = 76;
            // 
            // PROCS_B47
            // 
            this.PROCS_B47.Hexadecimal = true;
            this.PROCS_B47.Location = new System.Drawing.Point(349, 362);
            this.PROCS_B47.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B47.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B47.Name = "PROCS_B47";
            this.PROCS_B47.ReadOnly = true;
            this.PROCS_B47.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B47.TabIndex = 21;
            // 
            // PROCS_B55
            // 
            this.PROCS_B55.Hexadecimal = true;
            this.PROCS_B55.Location = new System.Drawing.Point(349, 392);
            this.PROCS_B55.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B55.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B55.Name = "PROCS_B55";
            this.PROCS_B55.ReadOnly = true;
            this.PROCS_B55.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B55.TabIndex = 33;
            // 
            // PROCS_B63
            // 
            this.PROCS_B63.Hexadecimal = true;
            this.PROCS_B63.Location = new System.Drawing.Point(349, 422);
            this.PROCS_B63.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B63.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B63.Name = "PROCS_B63";
            this.PROCS_B63.ReadOnly = true;
            this.PROCS_B63.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B63.TabIndex = 46;
            // 
            // PROCS_B103
            // 
            this.PROCS_B103.Hexadecimal = true;
            this.PROCS_B103.Location = new System.Drawing.Point(349, 572);
            this.PROCS_B103.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B103.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B103.Name = "PROCS_B103";
            this.PROCS_B103.ReadOnly = true;
            this.PROCS_B103.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B103.TabIndex = 114;
            // 
            // PROCS_B71
            // 
            this.PROCS_B71.Hexadecimal = true;
            this.PROCS_B71.Location = new System.Drawing.Point(349, 450);
            this.PROCS_B71.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B71.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B71.Name = "PROCS_B71";
            this.PROCS_B71.ReadOnly = true;
            this.PROCS_B71.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B71.TabIndex = 58;
            // 
            // PROCS_B95
            // 
            this.PROCS_B95.Hexadecimal = true;
            this.PROCS_B95.Location = new System.Drawing.Point(349, 542);
            this.PROCS_B95.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B95.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B95.Name = "PROCS_B95";
            this.PROCS_B95.ReadOnly = true;
            this.PROCS_B95.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B95.TabIndex = 102;
            // 
            // PROCS_B79
            // 
            this.PROCS_B79.Hexadecimal = true;
            this.PROCS_B79.Location = new System.Drawing.Point(349, 480);
            this.PROCS_B79.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B79.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B79.Name = "PROCS_B79";
            this.PROCS_B79.ReadOnly = true;
            this.PROCS_B79.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B79.TabIndex = 71;
            // 
            // PROCS_B87
            // 
            this.PROCS_B87.Hexadecimal = true;
            this.PROCS_B87.Location = new System.Drawing.Point(349, 509);
            this.PROCS_B87.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B87.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B87.Name = "PROCS_B87";
            this.PROCS_B87.ReadOnly = true;
            this.PROCS_B87.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B87.TabIndex = 84;
            // 
            // PROCS_B46
            // 
            this.PROCS_B46.Hexadecimal = true;
            this.PROCS_B46.Location = new System.Drawing.Point(277, 362);
            this.PROCS_B46.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B46.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B46.Name = "PROCS_B46";
            this.PROCS_B46.ReadOnly = true;
            this.PROCS_B46.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B46.TabIndex = 20;
            // 
            // PROCS_B54
            // 
            this.PROCS_B54.Hexadecimal = true;
            this.PROCS_B54.Location = new System.Drawing.Point(277, 392);
            this.PROCS_B54.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B54.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B54.Name = "PROCS_B54";
            this.PROCS_B54.ReadOnly = true;
            this.PROCS_B54.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B54.TabIndex = 32;
            // 
            // PROCS_B62
            // 
            this.PROCS_B62.Hexadecimal = true;
            this.PROCS_B62.Location = new System.Drawing.Point(277, 422);
            this.PROCS_B62.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B62.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B62.Name = "PROCS_B62";
            this.PROCS_B62.ReadOnly = true;
            this.PROCS_B62.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B62.TabIndex = 45;
            // 
            // PROCS_B102
            // 
            this.PROCS_B102.Hexadecimal = true;
            this.PROCS_B102.Location = new System.Drawing.Point(277, 572);
            this.PROCS_B102.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B102.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B102.Name = "PROCS_B102";
            this.PROCS_B102.ReadOnly = true;
            this.PROCS_B102.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B102.TabIndex = 113;
            // 
            // PROCS_B70
            // 
            this.PROCS_B70.Hexadecimal = true;
            this.PROCS_B70.Location = new System.Drawing.Point(277, 451);
            this.PROCS_B70.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B70.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B70.Name = "PROCS_B70";
            this.PROCS_B70.ReadOnly = true;
            this.PROCS_B70.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B70.TabIndex = 57;
            // 
            // PROCS_B94
            // 
            this.PROCS_B94.Hexadecimal = true;
            this.PROCS_B94.Location = new System.Drawing.Point(277, 542);
            this.PROCS_B94.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B94.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B94.Name = "PROCS_B94";
            this.PROCS_B94.ReadOnly = true;
            this.PROCS_B94.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B94.TabIndex = 101;
            // 
            // PROCS_B78
            // 
            this.PROCS_B78.Hexadecimal = true;
            this.PROCS_B78.Location = new System.Drawing.Point(277, 481);
            this.PROCS_B78.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B78.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B78.Name = "PROCS_B78";
            this.PROCS_B78.ReadOnly = true;
            this.PROCS_B78.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B78.TabIndex = 70;
            // 
            // PROCS_B86
            // 
            this.PROCS_B86.Hexadecimal = true;
            this.PROCS_B86.Location = new System.Drawing.Point(277, 510);
            this.PROCS_B86.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B86.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B86.Name = "PROCS_B86";
            this.PROCS_B86.ReadOnly = true;
            this.PROCS_B86.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B86.TabIndex = 83;
            // 
            // PROCS_B45
            // 
            this.PROCS_B45.Hexadecimal = true;
            this.PROCS_B45.Location = new System.Drawing.Point(205, 362);
            this.PROCS_B45.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B45.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B45.Name = "PROCS_B45";
            this.PROCS_B45.ReadOnly = true;
            this.PROCS_B45.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B45.TabIndex = 19;
            // 
            // PROCS_B53
            // 
            this.PROCS_B53.Hexadecimal = true;
            this.PROCS_B53.Location = new System.Drawing.Point(205, 392);
            this.PROCS_B53.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B53.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B53.Name = "PROCS_B53";
            this.PROCS_B53.ReadOnly = true;
            this.PROCS_B53.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B53.TabIndex = 31;
            // 
            // PROCS_B61
            // 
            this.PROCS_B61.Hexadecimal = true;
            this.PROCS_B61.Location = new System.Drawing.Point(205, 422);
            this.PROCS_B61.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B61.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B61.Name = "PROCS_B61";
            this.PROCS_B61.ReadOnly = true;
            this.PROCS_B61.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B61.TabIndex = 44;
            // 
            // PROCS_B101
            // 
            this.PROCS_B101.Hexadecimal = true;
            this.PROCS_B101.Location = new System.Drawing.Point(205, 572);
            this.PROCS_B101.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B101.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B101.Name = "PROCS_B101";
            this.PROCS_B101.ReadOnly = true;
            this.PROCS_B101.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B101.TabIndex = 112;
            // 
            // PROCS_B69
            // 
            this.PROCS_B69.Hexadecimal = true;
            this.PROCS_B69.Location = new System.Drawing.Point(205, 450);
            this.PROCS_B69.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B69.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B69.Name = "PROCS_B69";
            this.PROCS_B69.ReadOnly = true;
            this.PROCS_B69.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B69.TabIndex = 56;
            // 
            // PROCS_B93
            // 
            this.PROCS_B93.Hexadecimal = true;
            this.PROCS_B93.Location = new System.Drawing.Point(205, 542);
            this.PROCS_B93.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B93.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B93.Name = "PROCS_B93";
            this.PROCS_B93.ReadOnly = true;
            this.PROCS_B93.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B93.TabIndex = 99;
            // 
            // PROCS_B77
            // 
            this.PROCS_B77.Hexadecimal = true;
            this.PROCS_B77.Location = new System.Drawing.Point(205, 480);
            this.PROCS_B77.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B77.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B77.Name = "PROCS_B77";
            this.PROCS_B77.ReadOnly = true;
            this.PROCS_B77.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B77.TabIndex = 69;
            // 
            // PROCS_B85
            // 
            this.PROCS_B85.Hexadecimal = true;
            this.PROCS_B85.Location = new System.Drawing.Point(205, 509);
            this.PROCS_B85.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B85.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B85.Name = "PROCS_B85";
            this.PROCS_B85.ReadOnly = true;
            this.PROCS_B85.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B85.TabIndex = 97;
            // 
            // PROCS_L_104_DWORD
            // 
            this.PROCS_L_104_DWORD.ErrorMessage = "";
            this.PROCS_L_104_DWORD.Location = new System.Drawing.Point(908, 572);
            this.PROCS_L_104_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_104_DWORD.Name = "PROCS_L_104_DWORD";
            this.PROCS_L_104_DWORD.Placeholder = "";
            this.PROCS_L_104_DWORD.ReadOnly = true;
            this.PROCS_L_104_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_104_DWORD.TabIndex = 122;
            // 
            // PROCS_L_96_DWORD
            // 
            this.PROCS_L_96_DWORD.ErrorMessage = "";
            this.PROCS_L_96_DWORD.Location = new System.Drawing.Point(908, 542);
            this.PROCS_L_96_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_96_DWORD.Name = "PROCS_L_96_DWORD";
            this.PROCS_L_96_DWORD.Placeholder = "";
            this.PROCS_L_96_DWORD.ReadOnly = true;
            this.PROCS_L_96_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_96_DWORD.TabIndex = 109;
            // 
            // PROCS_L_88_DWORD
            // 
            this.PROCS_L_88_DWORD.ErrorMessage = "";
            this.PROCS_L_88_DWORD.Location = new System.Drawing.Point(908, 512);
            this.PROCS_L_88_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_88_DWORD.Name = "PROCS_L_88_DWORD";
            this.PROCS_L_88_DWORD.Placeholder = "";
            this.PROCS_L_88_DWORD.ReadOnly = true;
            this.PROCS_L_88_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_88_DWORD.TabIndex = 91;
            // 
            // PROCS_L_80_DWORD
            // 
            this.PROCS_L_80_DWORD.ErrorMessage = "";
            this.PROCS_L_80_DWORD.Location = new System.Drawing.Point(908, 481);
            this.PROCS_L_80_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_80_DWORD.Name = "PROCS_L_80_DWORD";
            this.PROCS_L_80_DWORD.Placeholder = "";
            this.PROCS_L_80_DWORD.ReadOnly = true;
            this.PROCS_L_80_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_80_DWORD.TabIndex = 79;
            // 
            // PROCS_L_72_DWORD
            // 
            this.PROCS_L_72_DWORD.ErrorMessage = "";
            this.PROCS_L_72_DWORD.Location = new System.Drawing.Point(908, 451);
            this.PROCS_L_72_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_72_DWORD.Name = "PROCS_L_72_DWORD";
            this.PROCS_L_72_DWORD.Placeholder = "";
            this.PROCS_L_72_DWORD.ReadOnly = true;
            this.PROCS_L_72_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_72_DWORD.TabIndex = 65;
            // 
            // PROCS_L_64_DWORD
            // 
            this.PROCS_L_64_DWORD.ErrorMessage = "";
            this.PROCS_L_64_DWORD.Location = new System.Drawing.Point(908, 422);
            this.PROCS_L_64_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_64_DWORD.Name = "PROCS_L_64_DWORD";
            this.PROCS_L_64_DWORD.Placeholder = "";
            this.PROCS_L_64_DWORD.ReadOnly = true;
            this.PROCS_L_64_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_64_DWORD.TabIndex = 53;
            // 
            // PROCS_L_100_DWORD
            // 
            this.PROCS_L_100_DWORD.ErrorMessage = "";
            this.PROCS_L_100_DWORD.Location = new System.Drawing.Point(422, 576);
            this.PROCS_L_100_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_100_DWORD.Name = "PROCS_L_100_DWORD";
            this.PROCS_L_100_DWORD.Placeholder = "";
            this.PROCS_L_100_DWORD.ReadOnly = true;
            this.PROCS_L_100_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_100_DWORD.TabIndex = 115;
            // 
            // PROCS_L_92_DWORD
            // 
            this.PROCS_L_92_DWORD.ErrorMessage = "";
            this.PROCS_L_92_DWORD.Location = new System.Drawing.Point(422, 546);
            this.PROCS_L_92_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_92_DWORD.Name = "PROCS_L_92_DWORD";
            this.PROCS_L_92_DWORD.Placeholder = "";
            this.PROCS_L_92_DWORD.ReadOnly = true;
            this.PROCS_L_92_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_92_DWORD.TabIndex = 103;
            // 
            // PROCS_L_84_DWORD
            // 
            this.PROCS_L_84_DWORD.ErrorMessage = "";
            this.PROCS_L_84_DWORD.Location = new System.Drawing.Point(422, 512);
            this.PROCS_L_84_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_84_DWORD.Name = "PROCS_L_84_DWORD";
            this.PROCS_L_84_DWORD.Placeholder = "";
            this.PROCS_L_84_DWORD.ReadOnly = true;
            this.PROCS_L_84_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_84_DWORD.TabIndex = 85;
            // 
            // PROCS_L_76_DWORD
            // 
            this.PROCS_L_76_DWORD.ErrorMessage = "";
            this.PROCS_L_76_DWORD.Location = new System.Drawing.Point(422, 480);
            this.PROCS_L_76_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_76_DWORD.Name = "PROCS_L_76_DWORD";
            this.PROCS_L_76_DWORD.Placeholder = "";
            this.PROCS_L_76_DWORD.ReadOnly = true;
            this.PROCS_L_76_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_76_DWORD.TabIndex = 72;
            // 
            // PROCS_L_68_DWORD
            // 
            this.PROCS_L_68_DWORD.ErrorMessage = "";
            this.PROCS_L_68_DWORD.Location = new System.Drawing.Point(422, 450);
            this.PROCS_L_68_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_68_DWORD.Name = "PROCS_L_68_DWORD";
            this.PROCS_L_68_DWORD.Placeholder = "";
            this.PROCS_L_68_DWORD.ReadOnly = true;
            this.PROCS_L_68_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_68_DWORD.TabIndex = 59;
            // 
            // PROCS_L_60_DWORD
            // 
            this.PROCS_L_60_DWORD.ErrorMessage = "";
            this.PROCS_L_60_DWORD.Location = new System.Drawing.Point(422, 422);
            this.PROCS_L_60_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_60_DWORD.Name = "PROCS_L_60_DWORD";
            this.PROCS_L_60_DWORD.Placeholder = "";
            this.PROCS_L_60_DWORD.ReadOnly = true;
            this.PROCS_L_60_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_60_DWORD.TabIndex = 47;
            // 
            // PROCS_L_56_DWORD
            // 
            this.PROCS_L_56_DWORD.ErrorMessage = "";
            this.PROCS_L_56_DWORD.Location = new System.Drawing.Point(908, 392);
            this.PROCS_L_56_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_56_DWORD.Name = "PROCS_L_56_DWORD";
            this.PROCS_L_56_DWORD.Placeholder = "";
            this.PROCS_L_56_DWORD.ReadOnly = true;
            this.PROCS_L_56_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_56_DWORD.TabIndex = 40;
            // 
            // PROCS_L_52_DWORD
            // 
            this.PROCS_L_52_DWORD.ErrorMessage = "";
            this.PROCS_L_52_DWORD.Location = new System.Drawing.Point(422, 392);
            this.PROCS_L_52_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_52_DWORD.Name = "PROCS_L_52_DWORD";
            this.PROCS_L_52_DWORD.Placeholder = "";
            this.PROCS_L_52_DWORD.ReadOnly = true;
            this.PROCS_L_52_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_52_DWORD.TabIndex = 34;
            // 
            // PROCS_L_48_DWORD
            // 
            this.PROCS_L_48_DWORD.ErrorMessage = "";
            this.PROCS_L_48_DWORD.Location = new System.Drawing.Point(908, 362);
            this.PROCS_L_48_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_48_DWORD.Name = "PROCS_L_48_DWORD";
            this.PROCS_L_48_DWORD.Placeholder = "";
            this.PROCS_L_48_DWORD.ReadOnly = true;
            this.PROCS_L_48_DWORD.Size = new System.Drawing.Size(101, 25);
            this.PROCS_L_48_DWORD.TabIndex = 28;
            // 
            // PROCS_L_44_DWORD
            // 
            this.PROCS_L_44_DWORD.ErrorMessage = "";
            this.PROCS_L_44_DWORD.Location = new System.Drawing.Point(422, 362);
            this.PROCS_L_44_DWORD.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_44_DWORD.Name = "PROCS_L_44_DWORD";
            this.PROCS_L_44_DWORD.Placeholder = "";
            this.PROCS_L_44_DWORD.ReadOnly = true;
            this.PROCS_L_44_DWORD.Size = new System.Drawing.Size(102, 25);
            this.PROCS_L_44_DWORD.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(8, 335);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(185, 18);
            this.label8.TabIndex = 301;
            this.label8.Text = "UserSpace";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(536, 337);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 14;
            this.label3.Text = "41(0x29)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B41
            // 
            this.PROCS_B41.Hexadecimal = true;
            this.PROCS_B41.Location = new System.Drawing.Point(620, 332);
            this.PROCS_B41.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B41.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B41.Name = "PROCS_B41";
            this.PROCS_B41.ReadOnly = true;
            this.PROCS_B41.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B41.TabIndex = 15;
            // 
            // PROCS_B43
            // 
            this.PROCS_B43.Hexadecimal = true;
            this.PROCS_B43.Location = new System.Drawing.Point(766, 332);
            this.PROCS_B43.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B43.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B43.Name = "PROCS_B43";
            this.PROCS_B43.ReadOnly = true;
            this.PROCS_B43.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B43.TabIndex = 16;
            // 
            // PROCS_CURSOL_CODE
            // 
            this.PROCS_CURSOL_CODE.ErrorMessage = "";
            this.PROCS_CURSOL_CODE.Location = new System.Drawing.Point(338, 35);
            this.PROCS_CURSOL_CODE.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_CURSOL_CODE.Name = "PROCS_CURSOL_CODE";
            this.PROCS_CURSOL_CODE.Placeholder = "";
            this.PROCS_CURSOL_CODE.ReadOnly = true;
            this.PROCS_CURSOL_CODE.Size = new System.Drawing.Size(671, 25);
            this.PROCS_CURSOL_CODE.TabIndex = 300;
            // 
            // PROCS_B42
            // 
            this.PROCS_B42.Hexadecimal = true;
            this.PROCS_B42.Location = new System.Drawing.Point(692, 332);
            this.PROCS_B42.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B42.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B42.Name = "PROCS_B42";
            this.PROCS_B42.ReadOnly = true;
            this.PROCS_B42.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B42.TabIndex = 298;
            // 
            // PROCS_L_16_TEXT
            // 
            this.PROCS_L_16_TEXT.ErrorMessage = "";
            this.PROCS_L_16_TEXT.Location = new System.Drawing.Point(338, 120);
            this.PROCS_L_16_TEXT.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_16_TEXT.Name = "PROCS_L_16_TEXT";
            this.PROCS_L_16_TEXT.Placeholder = "";
            this.PROCS_L_16_TEXT.ReadOnly = true;
            this.PROCS_L_16_TEXT.Size = new System.Drawing.Size(671, 25);
            this.PROCS_L_16_TEXT.TabIndex = 295;
            // 
            // PROCS_J_44
            // 
            this.PROCS_J_44.Location = new System.Drawing.Point(49, 366);
            this.PROCS_J_44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_44.Name = "PROCS_J_44";
            this.PROCS_J_44.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_44.TabIndex = 17;
            this.PROCS_J_44.Text = "44(0x2C)";
            this.PROCS_J_44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_P16
            // 
            this.PROCS_P16.Hexadecimal = true;
            this.PROCS_P16.Location = new System.Drawing.Point(197, 120);
            this.PROCS_P16.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P16.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P16.Name = "PROCS_P16";
            this.PROCS_P16.ReadOnly = true;
            this.PROCS_P16.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P16.TabIndex = 4;
            // 
            // PROCS_B104
            // 
            this.PROCS_B104.Hexadecimal = true;
            this.PROCS_B104.Location = new System.Drawing.Point(620, 572);
            this.PROCS_B104.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B104.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B104.Name = "PROCS_B104";
            this.PROCS_B104.ReadOnly = true;
            this.PROCS_B104.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B104.TabIndex = 117;
            // 
            // PROCS_J_16_TEXT
            // 
            this.PROCS_J_16_TEXT.Location = new System.Drawing.Point(5, 122);
            this.PROCS_J_16_TEXT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_16_TEXT.Name = "PROCS_J_16_TEXT";
            this.PROCS_J_16_TEXT.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_16_TEXT.TabIndex = 294;
            this.PROCS_J_16_TEXT.Text = "Name";
            this.PROCS_J_16_TEXT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B44
            // 
            this.PROCS_B44.Hexadecimal = true;
            this.PROCS_B44.Location = new System.Drawing.Point(133, 362);
            this.PROCS_B44.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B44.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B44.Name = "PROCS_B44";
            this.PROCS_B44.ReadOnly = true;
            this.PROCS_B44.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B44.TabIndex = 18;
            // 
            // textBoxEx16
            // 
            this.textBoxEx16.ErrorMessage = "";
            this.textBoxEx16.Location = new System.Drawing.Point(329, 830);
            this.textBoxEx16.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx16.Name = "textBoxEx16";
            this.textBoxEx16.Placeholder = "";
            this.textBoxEx16.ReadOnly = true;
            this.textBoxEx16.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx16.TabIndex = 289;
            // 
            // PROCS_J_104
            // 
            this.PROCS_J_104.Location = new System.Drawing.Point(533, 576);
            this.PROCS_J_104.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_104.Name = "PROCS_J_104";
            this.PROCS_J_104.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_104.TabIndex = 116;
            this.PROCS_J_104.Text = "104(0x68)";
            this.PROCS_J_104.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown17
            // 
            this.numericUpDown17.Hexadecimal = true;
            this.numericUpDown17.Location = new System.Drawing.Point(176, 830);
            this.numericUpDown17.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown17.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.numericUpDown17.Name = "numericUpDown17";
            this.numericUpDown17.ReadOnly = true;
            this.numericUpDown17.Size = new System.Drawing.Size(149, 25);
            this.numericUpDown17.TabIndex = 287;
            // 
            // PROCS_J_48
            // 
            this.PROCS_J_48.Location = new System.Drawing.Point(533, 365);
            this.PROCS_J_48.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_48.Name = "PROCS_J_48";
            this.PROCS_J_48.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_48.TabIndex = 23;
            this.PROCS_J_48.Text = "48(0x30)";
            this.PROCS_J_48.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(2, 833);
            this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(170, 18);
            this.label25.TabIndex = 288;
            this.label25.Text = "UserSpace3";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B48
            // 
            this.PROCS_B48.Hexadecimal = true;
            this.PROCS_B48.Location = new System.Drawing.Point(620, 362);
            this.PROCS_B48.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B48.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B48.Name = "PROCS_B48";
            this.PROCS_B48.ReadOnly = true;
            this.PROCS_B48.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B48.TabIndex = 24;
            // 
            // textBoxEx17
            // 
            this.textBoxEx17.ErrorMessage = "";
            this.textBoxEx17.Location = new System.Drawing.Point(329, 802);
            this.textBoxEx17.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx17.Name = "textBoxEx17";
            this.textBoxEx17.Placeholder = "";
            this.textBoxEx17.ReadOnly = true;
            this.textBoxEx17.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx17.TabIndex = 286;
            // 
            // PROCS_J_52
            // 
            this.PROCS_J_52.Location = new System.Drawing.Point(49, 396);
            this.PROCS_J_52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_52.Name = "PROCS_J_52";
            this.PROCS_J_52.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_52.TabIndex = 29;
            this.PROCS_J_52.Text = "52(0x34)";
            this.PROCS_J_52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown18
            // 
            this.numericUpDown18.Hexadecimal = true;
            this.numericUpDown18.Location = new System.Drawing.Point(176, 802);
            this.numericUpDown18.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown18.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.numericUpDown18.Name = "numericUpDown18";
            this.numericUpDown18.ReadOnly = true;
            this.numericUpDown18.Size = new System.Drawing.Size(149, 25);
            this.numericUpDown18.TabIndex = 284;
            // 
            // PROCS_B52
            // 
            this.PROCS_B52.Hexadecimal = true;
            this.PROCS_B52.Location = new System.Drawing.Point(133, 392);
            this.PROCS_B52.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B52.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B52.Name = "PROCS_B52";
            this.PROCS_B52.ReadOnly = true;
            this.PROCS_B52.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B52.TabIndex = 30;
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(2, 805);
            this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(170, 18);
            this.label26.TabIndex = 285;
            this.label26.Text = "UserSpace2";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_56
            // 
            this.PROCS_J_56.Location = new System.Drawing.Point(533, 396);
            this.PROCS_J_56.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_56.Name = "PROCS_J_56";
            this.PROCS_J_56.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_56.TabIndex = 35;
            this.PROCS_J_56.Text = "56(0x38)";
            this.PROCS_J_56.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEx18
            // 
            this.textBoxEx18.ErrorMessage = "";
            this.textBoxEx18.Location = new System.Drawing.Point(329, 773);
            this.textBoxEx18.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx18.Name = "textBoxEx18";
            this.textBoxEx18.Placeholder = "";
            this.textBoxEx18.ReadOnly = true;
            this.textBoxEx18.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx18.TabIndex = 283;
            // 
            // PROCS_B56
            // 
            this.PROCS_B56.Hexadecimal = true;
            this.PROCS_B56.Location = new System.Drawing.Point(620, 392);
            this.PROCS_B56.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B56.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B56.Name = "PROCS_B56";
            this.PROCS_B56.ReadOnly = true;
            this.PROCS_B56.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B56.TabIndex = 36;
            // 
            // numericUpDown19
            // 
            this.numericUpDown19.Hexadecimal = true;
            this.numericUpDown19.Location = new System.Drawing.Point(176, 773);
            this.numericUpDown19.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown19.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.numericUpDown19.Name = "numericUpDown19";
            this.numericUpDown19.ReadOnly = true;
            this.numericUpDown19.Size = new System.Drawing.Size(149, 25);
            this.numericUpDown19.TabIndex = 281;
            // 
            // PROCS_J_60
            // 
            this.PROCS_J_60.Location = new System.Drawing.Point(49, 426);
            this.PROCS_J_60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_60.Name = "PROCS_J_60";
            this.PROCS_J_60.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_60.TabIndex = 41;
            this.PROCS_J_60.Text = "60(0x3C)";
            this.PROCS_J_60.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(2, 776);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(170, 18);
            this.label27.TabIndex = 282;
            this.label27.Text = "UserSpace1";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B60
            // 
            this.PROCS_B60.Hexadecimal = true;
            this.PROCS_B60.Location = new System.Drawing.Point(133, 422);
            this.PROCS_B60.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B60.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B60.Name = "PROCS_B60";
            this.PROCS_B60.ReadOnly = true;
            this.PROCS_B60.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B60.TabIndex = 43;
            // 
            // PROCS_B40
            // 
            this.PROCS_B40.Hexadecimal = true;
            this.PROCS_B40.Location = new System.Drawing.Point(548, 292);
            this.PROCS_B40.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B40.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B40.Name = "PROCS_B40";
            this.PROCS_B40.ReadOnly = true;
            this.PROCS_B40.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B40.TabIndex = 13;
            // 
            // PROCS_J_64
            // 
            this.PROCS_J_64.Location = new System.Drawing.Point(533, 426);
            this.PROCS_J_64.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_64.Name = "PROCS_J_64";
            this.PROCS_J_64.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_64.TabIndex = 48;
            this.PROCS_J_64.Text = "64(0x40)";
            this.PROCS_J_64.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_40
            // 
            this.PROCS_J_40.Location = new System.Drawing.Point(374, 295);
            this.PROCS_J_40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_40.Name = "PROCS_J_40";
            this.PROCS_J_40.Size = new System.Drawing.Size(169, 18);
            this.PROCS_J_40.TabIndex = 234;
            this.PROCS_J_40.Text = "Block Counter";
            this.PROCS_J_40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B64
            // 
            this.PROCS_B64.Hexadecimal = true;
            this.PROCS_B64.Location = new System.Drawing.Point(620, 422);
            this.PROCS_B64.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B64.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B64.Name = "PROCS_B64";
            this.PROCS_B64.ReadOnly = true;
            this.PROCS_B64.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B64.TabIndex = 49;
            // 
            // PROCS_B39
            // 
            this.PROCS_B39.Hexadecimal = true;
            this.PROCS_B39.Location = new System.Drawing.Point(197, 290);
            this.PROCS_B39.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B39.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B39.Name = "PROCS_B39";
            this.PROCS_B39.ReadOnly = true;
            this.PROCS_B39.Size = new System.Drawing.Size(68, 25);
            this.PROCS_B39.TabIndex = 12;
            // 
            // PROCS_B100
            // 
            this.PROCS_B100.Hexadecimal = true;
            this.PROCS_B100.Location = new System.Drawing.Point(133, 572);
            this.PROCS_B100.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B100.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B100.Name = "PROCS_B100";
            this.PROCS_B100.ReadOnly = true;
            this.PROCS_B100.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B100.TabIndex = 111;
            // 
            // PROCS_J_39
            // 
            this.PROCS_J_39.Location = new System.Drawing.Point(5, 294);
            this.PROCS_J_39.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_39.Name = "PROCS_J_39";
            this.PROCS_J_39.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_39.TabIndex = 231;
            this.PROCS_J_39.Text = "Some kind of bitfield";
            this.PROCS_J_39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_68
            // 
            this.PROCS_J_68.Location = new System.Drawing.Point(49, 454);
            this.PROCS_J_68.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_68.Name = "PROCS_J_68";
            this.PROCS_J_68.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_68.TabIndex = 54;
            this.PROCS_J_68.Text = "68(0x44)";
            this.PROCS_J_68.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B38
            // 
            this.PROCS_B38.Hexadecimal = true;
            this.PROCS_B38.Location = new System.Drawing.Point(548, 260);
            this.PROCS_B38.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B38.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B38.Name = "PROCS_B38";
            this.PROCS_B38.ReadOnly = true;
            this.PROCS_B38.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B38.TabIndex = 11;
            // 
            // PROCS_J_100
            // 
            this.PROCS_J_100.Location = new System.Drawing.Point(49, 576);
            this.PROCS_J_100.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_100.Name = "PROCS_J_100";
            this.PROCS_J_100.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_100.TabIndex = 110;
            this.PROCS_J_100.Text = "100(0x64)";
            this.PROCS_J_100.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_38
            // 
            this.PROCS_J_38.Location = new System.Drawing.Point(374, 265);
            this.PROCS_J_38.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_38.Name = "PROCS_J_38";
            this.PROCS_J_38.Size = new System.Drawing.Size(169, 18);
            this.PROCS_J_38.TabIndex = 228;
            this.PROCS_J_38.Text = "Mark";
            this.PROCS_J_38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B68
            // 
            this.PROCS_B68.Hexadecimal = true;
            this.PROCS_B68.Location = new System.Drawing.Point(133, 451);
            this.PROCS_B68.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B68.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B68.Name = "PROCS_B68";
            this.PROCS_B68.ReadOnly = true;
            this.PROCS_B68.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B68.TabIndex = 55;
            // 
            // PROCS_W36
            // 
            this.PROCS_W36.Hexadecimal = true;
            this.PROCS_W36.Location = new System.Drawing.Point(197, 262);
            this.PROCS_W36.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_W36.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_W36.Name = "PROCS_W36";
            this.PROCS_W36.ReadOnly = true;
            this.PROCS_W36.Size = new System.Drawing.Size(106, 25);
            this.PROCS_W36.TabIndex = 9;
            // 
            // PROCS_B96
            // 
            this.PROCS_B96.Hexadecimal = true;
            this.PROCS_B96.Location = new System.Drawing.Point(620, 542);
            this.PROCS_B96.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B96.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B96.Name = "PROCS_B96";
            this.PROCS_B96.ReadOnly = true;
            this.PROCS_B96.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B96.TabIndex = 105;
            // 
            // PROCS_J_36
            // 
            this.PROCS_J_36.Location = new System.Drawing.Point(5, 265);
            this.PROCS_J_36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_36.Name = "PROCS_J_36";
            this.PROCS_J_36.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_36.TabIndex = 225;
            this.PROCS_J_36.Text = "Sleep Timer";
            this.PROCS_J_36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_72
            // 
            this.PROCS_J_72.Location = new System.Drawing.Point(533, 455);
            this.PROCS_J_72.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_72.Name = "PROCS_J_72";
            this.PROCS_J_72.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_72.TabIndex = 60;
            this.PROCS_J_72.Text = "72(0x48)";
            this.PROCS_J_72.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_L_32_RAMPROCS
            // 
            this.PROCS_L_32_RAMPROCS.ErrorMessage = "";
            this.PROCS_L_32_RAMPROCS.Location = new System.Drawing.Point(338, 234);
            this.PROCS_L_32_RAMPROCS.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_32_RAMPROCS.Name = "PROCS_L_32_RAMPROCS";
            this.PROCS_L_32_RAMPROCS.Placeholder = "";
            this.PROCS_L_32_RAMPROCS.ReadOnly = true;
            this.PROCS_L_32_RAMPROCS.Size = new System.Drawing.Size(671, 25);
            this.PROCS_L_32_RAMPROCS.TabIndex = 223;
            // 
            // PROCS_J_96
            // 
            this.PROCS_J_96.Location = new System.Drawing.Point(533, 546);
            this.PROCS_J_96.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_96.Name = "PROCS_J_96";
            this.PROCS_J_96.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_96.TabIndex = 104;
            this.PROCS_J_96.Text = "96(0x60)";
            this.PROCS_J_96.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_P32
            // 
            this.PROCS_P32.Hexadecimal = true;
            this.PROCS_P32.Location = new System.Drawing.Point(197, 234);
            this.PROCS_P32.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P32.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P32.Name = "PROCS_P32";
            this.PROCS_P32.ReadOnly = true;
            this.PROCS_P32.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P32.TabIndex = 8;
            // 
            // PROCS_B72
            // 
            this.PROCS_B72.Hexadecimal = true;
            this.PROCS_B72.Location = new System.Drawing.Point(620, 452);
            this.PROCS_B72.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B72.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B72.Name = "PROCS_B72";
            this.PROCS_B72.ReadOnly = true;
            this.PROCS_B72.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B72.TabIndex = 61;
            // 
            // PROCS_J_32_RAMPROCS
            // 
            this.PROCS_J_32_RAMPROCS.Location = new System.Drawing.Point(5, 236);
            this.PROCS_J_32_RAMPROCS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_32_RAMPROCS.Name = "PROCS_J_32_RAMPROCS";
            this.PROCS_J_32_RAMPROCS.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_32_RAMPROCS.TabIndex = 222;
            this.PROCS_J_32_RAMPROCS.Text = "Next Struct";
            this.PROCS_J_32_RAMPROCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B92
            // 
            this.PROCS_B92.Hexadecimal = true;
            this.PROCS_B92.Location = new System.Drawing.Point(133, 542);
            this.PROCS_B92.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B92.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B92.Name = "PROCS_B92";
            this.PROCS_B92.ReadOnly = true;
            this.PROCS_B92.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B92.TabIndex = 93;
            // 
            // PROCS_L_28_RAMPROCS
            // 
            this.PROCS_L_28_RAMPROCS.ErrorMessage = "";
            this.PROCS_L_28_RAMPROCS.Location = new System.Drawing.Point(338, 206);
            this.PROCS_L_28_RAMPROCS.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_28_RAMPROCS.Name = "PROCS_L_28_RAMPROCS";
            this.PROCS_L_28_RAMPROCS.Placeholder = "";
            this.PROCS_L_28_RAMPROCS.ReadOnly = true;
            this.PROCS_L_28_RAMPROCS.Size = new System.Drawing.Size(671, 25);
            this.PROCS_L_28_RAMPROCS.TabIndex = 220;
            // 
            // PROCS_J_76
            // 
            this.PROCS_J_76.Location = new System.Drawing.Point(49, 484);
            this.PROCS_J_76.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_76.Name = "PROCS_J_76";
            this.PROCS_J_76.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_76.TabIndex = 67;
            this.PROCS_J_76.Text = "76(0x4C)";
            this.PROCS_J_76.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_P28
            // 
            this.PROCS_P28.Hexadecimal = true;
            this.PROCS_P28.Location = new System.Drawing.Point(197, 206);
            this.PROCS_P28.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P28.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P28.Name = "PROCS_P28";
            this.PROCS_P28.ReadOnly = true;
            this.PROCS_P28.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P28.TabIndex = 7;
            // 
            // PROCS_J_92
            // 
            this.PROCS_J_92.Location = new System.Drawing.Point(49, 546);
            this.PROCS_J_92.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_92.Name = "PROCS_J_92";
            this.PROCS_J_92.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_92.TabIndex = 92;
            this.PROCS_J_92.Text = "92(0x5C)";
            this.PROCS_J_92.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_28_RAMPROCS
            // 
            this.PROCS_J_28_RAMPROCS.Location = new System.Drawing.Point(5, 209);
            this.PROCS_J_28_RAMPROCS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_28_RAMPROCS.Name = "PROCS_J_28_RAMPROCS";
            this.PROCS_J_28_RAMPROCS.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_28_RAMPROCS.TabIndex = 219;
            this.PROCS_J_28_RAMPROCS.Text = "Previous Struct";
            this.PROCS_J_28_RAMPROCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_B76
            // 
            this.PROCS_B76.Hexadecimal = true;
            this.PROCS_B76.Location = new System.Drawing.Point(133, 481);
            this.PROCS_B76.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B76.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B76.Name = "PROCS_B76";
            this.PROCS_B76.ReadOnly = true;
            this.PROCS_B76.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B76.TabIndex = 68;
            // 
            // PROCS_L_24_RAMPROCS
            // 
            this.PROCS_L_24_RAMPROCS.ErrorMessage = "";
            this.PROCS_L_24_RAMPROCS.Location = new System.Drawing.Point(338, 176);
            this.PROCS_L_24_RAMPROCS.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_24_RAMPROCS.Name = "PROCS_L_24_RAMPROCS";
            this.PROCS_L_24_RAMPROCS.Placeholder = "";
            this.PROCS_L_24_RAMPROCS.ReadOnly = true;
            this.PROCS_L_24_RAMPROCS.Size = new System.Drawing.Size(671, 25);
            this.PROCS_L_24_RAMPROCS.TabIndex = 217;
            // 
            // PROCS_B88
            // 
            this.PROCS_B88.Hexadecimal = true;
            this.PROCS_B88.Location = new System.Drawing.Point(620, 510);
            this.PROCS_B88.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B88.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B88.Name = "PROCS_B88";
            this.PROCS_B88.ReadOnly = true;
            this.PROCS_B88.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B88.TabIndex = 87;
            // 
            // PROCS_P24
            // 
            this.PROCS_P24.Hexadecimal = true;
            this.PROCS_P24.Location = new System.Drawing.Point(197, 176);
            this.PROCS_P24.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P24.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P24.Name = "PROCS_P24";
            this.PROCS_P24.ReadOnly = true;
            this.PROCS_P24.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P24.TabIndex = 6;
            // 
            // PROCS_J_80
            // 
            this.PROCS_J_80.Location = new System.Drawing.Point(533, 485);
            this.PROCS_J_80.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_80.Name = "PROCS_J_80";
            this.PROCS_J_80.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_80.TabIndex = 73;
            this.PROCS_J_80.Text = "80(0x50)";
            this.PROCS_J_80.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_24_RAMPROCS
            // 
            this.PROCS_J_24_RAMPROCS.Location = new System.Drawing.Point(5, 180);
            this.PROCS_J_24_RAMPROCS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_24_RAMPROCS.Name = "PROCS_J_24_RAMPROCS";
            this.PROCS_J_24_RAMPROCS.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_24_RAMPROCS.TabIndex = 216;
            this.PROCS_J_24_RAMPROCS.Text = "First Child Struct";
            this.PROCS_J_24_RAMPROCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_88
            // 
            this.PROCS_J_88.Location = new System.Drawing.Point(533, 512);
            this.PROCS_J_88.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_88.Name = "PROCS_J_88";
            this.PROCS_J_88.Size = new System.Drawing.Size(83, 18);
            this.PROCS_J_88.TabIndex = 86;
            this.PROCS_J_88.Text = "88(0x58)";
            this.PROCS_J_88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_L_20_RAMPROCS
            // 
            this.PROCS_L_20_RAMPROCS.ErrorMessage = "";
            this.PROCS_L_20_RAMPROCS.Location = new System.Drawing.Point(338, 149);
            this.PROCS_L_20_RAMPROCS.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_20_RAMPROCS.Name = "PROCS_L_20_RAMPROCS";
            this.PROCS_L_20_RAMPROCS.Placeholder = "";
            this.PROCS_L_20_RAMPROCS.ReadOnly = true;
            this.PROCS_L_20_RAMPROCS.Size = new System.Drawing.Size(671, 25);
            this.PROCS_L_20_RAMPROCS.TabIndex = 214;
            // 
            // PROCS_B80
            // 
            this.PROCS_B80.Hexadecimal = true;
            this.PROCS_B80.Location = new System.Drawing.Point(620, 482);
            this.PROCS_B80.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B80.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B80.Name = "PROCS_B80";
            this.PROCS_B80.ReadOnly = true;
            this.PROCS_B80.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B80.TabIndex = 75;
            // 
            // PROCS_P20
            // 
            this.PROCS_P20.Hexadecimal = true;
            this.PROCS_P20.Location = new System.Drawing.Point(197, 149);
            this.PROCS_P20.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P20.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P20.Name = "PROCS_P20";
            this.PROCS_P20.ReadOnly = true;
            this.PROCS_P20.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P20.TabIndex = 5;
            // 
            // PROCS_B84
            // 
            this.PROCS_B84.Hexadecimal = true;
            this.PROCS_B84.Location = new System.Drawing.Point(133, 510);
            this.PROCS_B84.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_B84.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_B84.Name = "PROCS_B84";
            this.PROCS_B84.ReadOnly = true;
            this.PROCS_B84.Size = new System.Drawing.Size(67, 25);
            this.PROCS_B84.TabIndex = 81;
            // 
            // PROCS_J_20_RAMPROCS
            // 
            this.PROCS_J_20_RAMPROCS.Location = new System.Drawing.Point(5, 152);
            this.PROCS_J_20_RAMPROCS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_20_RAMPROCS.Name = "PROCS_J_20_RAMPROCS";
            this.PROCS_J_20_RAMPROCS.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_20_RAMPROCS.TabIndex = 213;
            this.PROCS_J_20_RAMPROCS.Text = "Parent Procs";
            this.PROCS_J_20_RAMPROCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_J_84
            // 
            this.PROCS_J_84.Location = new System.Drawing.Point(49, 512);
            this.PROCS_J_84.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_84.Name = "PROCS_J_84";
            this.PROCS_J_84.Size = new System.Drawing.Size(78, 18);
            this.PROCS_J_84.TabIndex = 80;
            this.PROCS_J_84.Text = "84(0x54)";
            this.PROCS_J_84.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_L_12_ASM
            // 
            this.PROCS_L_12_ASM.ErrorMessage = "";
            this.PROCS_L_12_ASM.Location = new System.Drawing.Point(338, 91);
            this.PROCS_L_12_ASM.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_12_ASM.Name = "PROCS_L_12_ASM";
            this.PROCS_L_12_ASM.Placeholder = "";
            this.PROCS_L_12_ASM.ReadOnly = true;
            this.PROCS_L_12_ASM.Size = new System.Drawing.Size(671, 25);
            this.PROCS_L_12_ASM.TabIndex = 211;
            // 
            // PROCS_P12
            // 
            this.PROCS_P12.Hexadecimal = true;
            this.PROCS_P12.Location = new System.Drawing.Point(197, 91);
            this.PROCS_P12.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P12.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P12.Name = "PROCS_P12";
            this.PROCS_P12.ReadOnly = true;
            this.PROCS_P12.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P12.TabIndex = 3;
            // 
            // PROCS_J_12_ASM
            // 
            this.PROCS_J_12_ASM.Location = new System.Drawing.Point(5, 94);
            this.PROCS_J_12_ASM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_12_ASM.Name = "PROCS_J_12_ASM";
            this.PROCS_J_12_ASM.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_12_ASM.TabIndex = 210;
            this.PROCS_J_12_ASM.Text = "Loop Routine";
            this.PROCS_J_12_ASM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_L_8_ASM
            // 
            this.PROCS_L_8_ASM.ErrorMessage = "";
            this.PROCS_L_8_ASM.Location = new System.Drawing.Point(338, 62);
            this.PROCS_L_8_ASM.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_L_8_ASM.Name = "PROCS_L_8_ASM";
            this.PROCS_L_8_ASM.Placeholder = "";
            this.PROCS_L_8_ASM.ReadOnly = true;
            this.PROCS_L_8_ASM.Size = new System.Drawing.Size(671, 25);
            this.PROCS_L_8_ASM.TabIndex = 208;
            // 
            // PROCS_P8
            // 
            this.PROCS_P8.Hexadecimal = true;
            this.PROCS_P8.Location = new System.Drawing.Point(197, 62);
            this.PROCS_P8.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P8.Name = "PROCS_P8";
            this.PROCS_P8.ReadOnly = true;
            this.PROCS_P8.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P8.TabIndex = 2;
            // 
            // PROCS_J_8_ASM
            // 
            this.PROCS_J_8_ASM.Location = new System.Drawing.Point(5, 66);
            this.PROCS_J_8_ASM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_8_ASM.Name = "PROCS_J_8_ASM";
            this.PROCS_J_8_ASM.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_8_ASM.TabIndex = 207;
            this.PROCS_J_8_ASM.Text = "Destructor Routine";
            this.PROCS_J_8_ASM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PROCS_P4
            // 
            this.PROCS_P4.Hexadecimal = true;
            this.PROCS_P4.Location = new System.Drawing.Point(197, 34);
            this.PROCS_P4.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P4.Name = "PROCS_P4";
            this.PROCS_P4.ReadOnly = true;
            this.PROCS_P4.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P4.TabIndex = 1;
            // 
            // PROCS_JUMP_CURSOL_CODE
            // 
            this.PROCS_JUMP_CURSOL_CODE.Location = new System.Drawing.Point(5, 37);
            this.PROCS_JUMP_CURSOL_CODE.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_JUMP_CURSOL_CODE.Name = "PROCS_JUMP_CURSOL_CODE";
            this.PROCS_JUMP_CURSOL_CODE.Size = new System.Drawing.Size(188, 18);
            this.PROCS_JUMP_CURSOL_CODE.TabIndex = 204;
            this.PROCS_JUMP_CURSOL_CODE.Text = "Code cursor";
            this.PROCS_JUMP_CURSOL_CODE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PROCS_JUMP_CURSOL_CODE.Click += new System.EventHandler(this.PROCS_JUMP_CURSOL_CODE_Click);
            // 
            // PROCS_NAME
            // 
            this.PROCS_NAME.ErrorMessage = "";
            this.PROCS_NAME.Location = new System.Drawing.Point(338, 6);
            this.PROCS_NAME.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_NAME.Name = "PROCS_NAME";
            this.PROCS_NAME.Placeholder = "";
            this.PROCS_NAME.ReadOnly = true;
            this.PROCS_NAME.Size = new System.Drawing.Size(671, 25);
            this.PROCS_NAME.TabIndex = 202;
            // 
            // PROCS_P0
            // 
            this.PROCS_P0.Hexadecimal = true;
            this.PROCS_P0.Location = new System.Drawing.Point(197, 6);
            this.PROCS_P0.Margin = new System.Windows.Forms.Padding(2);
            this.PROCS_P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PROCS_P0.Name = "PROCS_P0";
            this.PROCS_P0.ReadOnly = true;
            this.PROCS_P0.Size = new System.Drawing.Size(134, 25);
            this.PROCS_P0.TabIndex = 0;
            // 
            // PROCS_J_0_PROCS
            // 
            this.PROCS_J_0_PROCS.Location = new System.Drawing.Point(5, 8);
            this.PROCS_J_0_PROCS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PROCS_J_0_PROCS.Name = "PROCS_J_0_PROCS";
            this.PROCS_J_0_PROCS.Size = new System.Drawing.Size(188, 18);
            this.PROCS_J_0_PROCS.TabIndex = 1;
            this.PROCS_J_0_PROCS.Text = "Start of code";
            this.PROCS_J_0_PROCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(842, 2);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(2);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(98, 32);
            this.CloseButton.TabIndex = 201;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(16, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1628, 26);
            this.label1.TabIndex = 160;
            this.label1.Text = "Procs";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProcsListBox
            // 
            this.ProcsListBox.FormattingEnabled = true;
            this.ProcsListBox.IntegralHeight = false;
            this.ProcsListBox.ItemHeight = 18;
            this.ProcsListBox.Location = new System.Drawing.Point(14, 28);
            this.ProcsListBox.Margin = new System.Windows.Forms.Padding(2);
            this.ProcsListBox.Name = "ProcsListBox";
            this.ProcsListBox.Size = new System.Drawing.Size(1629, 850);
            this.ProcsListBox.TabIndex = 161;
            this.ProcsListBox.SelectedIndexChanged += new System.EventHandler(this.ProcsListBox_SelectedIndexChanged);
            this.ProcsListBox.DoubleClick += new System.EventHandler(this.ProcsListBox_DoubleClick);
            this.ProcsListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProcsListBox_KeyDown);
            // 
            // EtcPage
            // 
            this.EtcPage.BackColor = System.Drawing.SystemColors.Control;
            this.EtcPage.Controls.Add(this.PartyCombo);
            this.EtcPage.Controls.Add(this.PartyCount);
            this.EtcPage.Controls.Add(this.tabControlEtc);
            this.EtcPage.Controls.Add(this.Party_ControlPanel);
            this.EtcPage.Controls.Add(this.PartyListBox);
            this.EtcPage.Controls.Add(this.label56);
            this.EtcPage.Location = new System.Drawing.Point(4, 28);
            this.EtcPage.Name = "EtcPage";
            this.EtcPage.Padding = new System.Windows.Forms.Padding(3);
            this.EtcPage.Size = new System.Drawing.Size(1646, 888);
            this.EtcPage.TabIndex = 4;
            this.EtcPage.Text = "Etc";
            // 
            // PartyCombo
            // 
            this.PartyCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PartyCombo.FormattingEnabled = true;
            this.PartyCombo.Items.AddRange(new object[] {
            "00=PLAYER",
            "40=NPC",
            "80=Enemy"});
            this.PartyCombo.Location = new System.Drawing.Point(318, 10);
            this.PartyCombo.Name = "PartyCombo";
            this.PartyCombo.Size = new System.Drawing.Size(459, 26);
            this.PartyCombo.TabIndex = 313;
            // 
            // PartyCount
            // 
            this.PartyCount.AutoSize = true;
            this.PartyCount.Location = new System.Drawing.Point(96, 20);
            this.PartyCount.Name = "PartyCount";
            this.PartyCount.Size = new System.Drawing.Size(94, 18);
            this.PartyCount.TabIndex = 333;
            this.PartyCount.Text = "PartyCount";
            // 
            // tabControlEtc
            // 
            this.tabControlEtc.Controls.Add(this.tabPageGeneral);
            this.tabControlEtc.Controls.Add(this.tabPageTrapData);
            this.tabControlEtc.Controls.Add(this.tabPagePalette);
            this.tabControlEtc.Controls.Add(this.tabPageClearTurns);
            this.tabControlEtc.Controls.Add(this.tabPageBWL);
            this.tabControlEtc.Controls.Add(this.tabPageChapterData);
            this.tabControlEtc.Controls.Add(this.tabPageSupplyData);
            this.tabControlEtc.Controls.Add(this.tabPageActionData);
            this.tabControlEtc.Controls.Add(this.tabPageArenaData);
            this.tabControlEtc.Controls.Add(this.tabPageBattleActor);
            this.tabControlEtc.Controls.Add(this.tabPageBattleTarget);
            this.tabControlEtc.Controls.Add(this.tabPageAIData);
            this.tabControlEtc.Controls.Add(this.tabPageBattleRound);
            this.tabControlEtc.Controls.Add(this.tabPageBattleSome);
            this.tabControlEtc.Controls.Add(this.tabPageWorldmap);
            this.tabControlEtc.Controls.Add(this.tabPageDungeon);
            this.tabControlEtc.Location = new System.Drawing.Point(785, 8);
            this.tabControlEtc.Multiline = true;
            this.tabControlEtc.Name = "tabControlEtc";
            this.tabControlEtc.SelectedIndex = 0;
            this.tabControlEtc.Size = new System.Drawing.Size(861, 876);
            this.tabControlEtc.TabIndex = 332;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageGeneral.Controls.Add(this.panel2);
            this.tabPageGeneral.Controls.Add(this.SoundAddress);
            this.tabPageGeneral.Controls.Add(this.SoundList);
            this.tabPageGeneral.Controls.Add(this.label91);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 52);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(853, 820);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "汎用";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.J_ACTIVEUNIT);
            this.panel2.Controls.Add(this.ETC_UNIT_MEMORY_AND_NAME);
            this.panel2.Controls.Add(this.ETC_UNIT_MEMORY_AND_ICON);
            this.panel2.Controls.Add(this.label58);
            this.panel2.Controls.Add(this.N_J_14_MAP);
            this.panel2.Controls.Add(this.X_ETC_WorldmapNode_Text);
            this.panel2.Controls.Add(this.BGMName);
            this.panel2.Controls.Add(this.X_ETC_Edition_Text);
            this.panel2.Controls.Add(this.N_B14);
            this.panel2.Controls.Add(this.X_ETC_Diffculty_Text);
            this.panel2.Controls.Add(this.N_L_14_MAP);
            this.panel2.Controls.Add(this.BGM);
            this.panel2.Controls.Add(this.J_BGM);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(847, 145);
            this.panel2.TabIndex = 332;
            // 
            // J_ACTIVEUNIT
            // 
            this.J_ACTIVEUNIT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_ACTIVEUNIT.Location = new System.Drawing.Point(2, 84);
            this.J_ACTIVEUNIT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_ACTIVEUNIT.Name = "J_ACTIVEUNIT";
            this.J_ACTIVEUNIT.Size = new System.Drawing.Size(142, 30);
            this.J_ACTIVEUNIT.TabIndex = 343;
            this.J_ACTIVEUNIT.Text = "操作中のユニット";
            this.J_ACTIVEUNIT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.J_ACTIVEUNIT.Click += new System.EventHandler(this.J_ACTIVEUNIT_Click);
            // 
            // ETC_UNIT_MEMORY_AND_NAME
            // 
            this.ETC_UNIT_MEMORY_AND_NAME.Cursor = System.Windows.Forms.Cursors.Default;
            this.ETC_UNIT_MEMORY_AND_NAME.ErrorMessage = "";
            this.ETC_UNIT_MEMORY_AND_NAME.Location = new System.Drawing.Point(243, 91);
            this.ETC_UNIT_MEMORY_AND_NAME.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.ETC_UNIT_MEMORY_AND_NAME.Name = "ETC_UNIT_MEMORY_AND_NAME";
            this.ETC_UNIT_MEMORY_AND_NAME.Placeholder = "";
            this.ETC_UNIT_MEMORY_AND_NAME.ReadOnly = true;
            this.ETC_UNIT_MEMORY_AND_NAME.Size = new System.Drawing.Size(599, 25);
            this.ETC_UNIT_MEMORY_AND_NAME.TabIndex = 341;
            this.ETC_UNIT_MEMORY_AND_NAME.DoubleClick += new System.EventHandler(this.ETC_UNIT_MEMORY_AND_NAME_DoubleClick);
            // 
            // ETC_UNIT_MEMORY_AND_ICON
            // 
            this.ETC_UNIT_MEMORY_AND_ICON.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ETC_UNIT_MEMORY_AND_ICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.ETC_UNIT_MEMORY_AND_ICON.Location = new System.Drawing.Point(151, 88);
            this.ETC_UNIT_MEMORY_AND_ICON.Margin = new System.Windows.Forms.Padding(2);
            this.ETC_UNIT_MEMORY_AND_ICON.Name = "ETC_UNIT_MEMORY_AND_ICON";
            this.ETC_UNIT_MEMORY_AND_ICON.Size = new System.Drawing.Size(56, 56);
            this.ETC_UNIT_MEMORY_AND_ICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ETC_UNIT_MEMORY_AND_ICON.TabIndex = 342;
            this.ETC_UNIT_MEMORY_AND_ICON.TabStop = false;
            this.ETC_UNIT_MEMORY_AND_ICON.Click += new System.EventHandler(this.ETC_UNIT_MEMORY_AND_ICON_Click);
            // 
            // label58
            // 
            this.label58.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label58.Location = new System.Drawing.Point(2, 0);
            this.label58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(142, 30);
            this.label58.TabIndex = 339;
            this.label58.Text = "編";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // N_J_14_MAP
            // 
            this.N_J_14_MAP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.N_J_14_MAP.Location = new System.Drawing.Point(2, 29);
            this.N_J_14_MAP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.N_J_14_MAP.Name = "N_J_14_MAP";
            this.N_J_14_MAP.Size = new System.Drawing.Size(142, 27);
            this.N_J_14_MAP.TabIndex = 335;
            this.N_J_14_MAP.Text = "マップID";
            this.N_J_14_MAP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // X_ETC_WorldmapNode_Text
            // 
            this.X_ETC_WorldmapNode_Text.ErrorMessage = "";
            this.X_ETC_WorldmapNode_Text.Location = new System.Drawing.Point(546, 2);
            this.X_ETC_WorldmapNode_Text.Margin = new System.Windows.Forms.Padding(2);
            this.X_ETC_WorldmapNode_Text.Name = "X_ETC_WorldmapNode_Text";
            this.X_ETC_WorldmapNode_Text.Placeholder = "";
            this.X_ETC_WorldmapNode_Text.ReadOnly = true;
            this.X_ETC_WorldmapNode_Text.Size = new System.Drawing.Size(297, 25);
            this.X_ETC_WorldmapNode_Text.TabIndex = 319;
            // 
            // BGMName
            // 
            this.BGMName.ErrorMessage = "";
            this.BGMName.Location = new System.Drawing.Point(243, 59);
            this.BGMName.Margin = new System.Windows.Forms.Padding(2);
            this.BGMName.Name = "BGMName";
            this.BGMName.Placeholder = "";
            this.BGMName.ReadOnly = true;
            this.BGMName.Size = new System.Drawing.Size(600, 25);
            this.BGMName.TabIndex = 338;
            // 
            // X_ETC_Edition_Text
            // 
            this.X_ETC_Edition_Text.ErrorMessage = "";
            this.X_ETC_Edition_Text.Location = new System.Drawing.Point(151, 2);
            this.X_ETC_Edition_Text.Margin = new System.Windows.Forms.Padding(2);
            this.X_ETC_Edition_Text.Name = "X_ETC_Edition_Text";
            this.X_ETC_Edition_Text.Placeholder = "";
            this.X_ETC_Edition_Text.ReadOnly = true;
            this.X_ETC_Edition_Text.Size = new System.Drawing.Size(225, 25);
            this.X_ETC_Edition_Text.TabIndex = 316;
            // 
            // N_B14
            // 
            this.N_B14.Hexadecimal = true;
            this.N_B14.Location = new System.Drawing.Point(151, 31);
            this.N_B14.Margin = new System.Windows.Forms.Padding(2);
            this.N_B14.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.N_B14.Name = "N_B14";
            this.N_B14.Size = new System.Drawing.Size(88, 25);
            this.N_B14.TabIndex = 334;
            this.N_B14.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // X_ETC_Diffculty_Text
            // 
            this.X_ETC_Diffculty_Text.ErrorMessage = "";
            this.X_ETC_Diffculty_Text.Location = new System.Drawing.Point(380, 2);
            this.X_ETC_Diffculty_Text.Margin = new System.Windows.Forms.Padding(2);
            this.X_ETC_Diffculty_Text.Name = "X_ETC_Diffculty_Text";
            this.X_ETC_Diffculty_Text.Placeholder = "";
            this.X_ETC_Diffculty_Text.ReadOnly = true;
            this.X_ETC_Diffculty_Text.Size = new System.Drawing.Size(162, 25);
            this.X_ETC_Diffculty_Text.TabIndex = 318;
            // 
            // N_L_14_MAP
            // 
            this.N_L_14_MAP.ErrorMessage = "";
            this.N_L_14_MAP.Location = new System.Drawing.Point(243, 31);
            this.N_L_14_MAP.Margin = new System.Windows.Forms.Padding(2);
            this.N_L_14_MAP.Name = "N_L_14_MAP";
            this.N_L_14_MAP.Placeholder = "";
            this.N_L_14_MAP.ReadOnly = true;
            this.N_L_14_MAP.Size = new System.Drawing.Size(599, 25);
            this.N_L_14_MAP.TabIndex = 336;
            // 
            // BGM
            // 
            this.BGM.Hexadecimal = true;
            this.BGM.Location = new System.Drawing.Point(151, 60);
            this.BGM.Margin = new System.Windows.Forms.Padding(2);
            this.BGM.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.BGM.Name = "BGM";
            this.BGM.Size = new System.Drawing.Size(88, 25);
            this.BGM.TabIndex = 333;
            // 
            // J_BGM
            // 
            this.J_BGM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.J_BGM.Location = new System.Drawing.Point(2, 55);
            this.J_BGM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.J_BGM.Name = "J_BGM";
            this.J_BGM.Size = new System.Drawing.Size(142, 30);
            this.J_BGM.TabIndex = 337;
            this.J_BGM.Text = "BGM";
            this.J_BGM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SoundAddress
            // 
            this.SoundAddress.ErrorMessage = "";
            this.SoundAddress.Location = new System.Drawing.Point(730, 153);
            this.SoundAddress.Margin = new System.Windows.Forms.Padding(2);
            this.SoundAddress.Name = "SoundAddress";
            this.SoundAddress.Placeholder = "";
            this.SoundAddress.ReadOnly = true;
            this.SoundAddress.Size = new System.Drawing.Size(116, 25);
            this.SoundAddress.TabIndex = 331;
            // 
            // SoundList
            // 
            this.SoundList.FormattingEnabled = true;
            this.SoundList.IntegralHeight = false;
            this.SoundList.ItemHeight = 18;
            this.SoundList.Location = new System.Drawing.Point(3, 181);
            this.SoundList.Name = "SoundList";
            this.SoundList.Size = new System.Drawing.Size(851, 175);
            this.SoundList.TabIndex = 326;
            this.SoundList.SelectedIndexChanged += new System.EventHandler(this.SoundList_SelectedIndexChanged);
            this.SoundList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SoundList_KeyDown);
            this.SoundList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SoundList_MouseDoubleClick);
            // 
            // label91
            // 
            this.label91.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label91.Location = new System.Drawing.Point(3, 152);
            this.label91.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(851, 30);
            this.label91.TabIndex = 325;
            this.label91.Text = "再生されている音楽";
            this.label91.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageTrapData
            // 
            this.tabPageTrapData.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTrapData.Controls.Add(this.TrapList);
            this.tabPageTrapData.Controls.Add(this.TrapAddress);
            this.tabPageTrapData.Controls.Add(this.label62);
            this.tabPageTrapData.Location = new System.Drawing.Point(4, 52);
            this.tabPageTrapData.Name = "tabPageTrapData";
            this.tabPageTrapData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTrapData.Size = new System.Drawing.Size(853, 820);
            this.tabPageTrapData.TabIndex = 1;
            this.tabPageTrapData.Text = "トラップデータ";
            // 
            // TrapList
            // 
            this.TrapList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrapList.FormattingEnabled = true;
            this.TrapList.IntegralHeight = false;
            this.TrapList.ItemHeight = 18;
            this.TrapList.Location = new System.Drawing.Point(3, 33);
            this.TrapList.Name = "TrapList";
            this.TrapList.Size = new System.Drawing.Size(847, 784);
            this.TrapList.TabIndex = 329;
            this.TrapList.SelectedIndexChanged += new System.EventHandler(this.TrapList_SelectedIndexChanged);
            this.TrapList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TrapList_KeyDown);
            this.TrapList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrapList_MouseDoubleClick);
            // 
            // TrapAddress
            // 
            this.TrapAddress.ErrorMessage = "";
            this.TrapAddress.Location = new System.Drawing.Point(732, 5);
            this.TrapAddress.Margin = new System.Windows.Forms.Padding(2);
            this.TrapAddress.Name = "TrapAddress";
            this.TrapAddress.Placeholder = "";
            this.TrapAddress.ReadOnly = true;
            this.TrapAddress.Size = new System.Drawing.Size(116, 25);
            this.TrapAddress.TabIndex = 330;
            // 
            // label62
            // 
            this.label62.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label62.Dock = System.Windows.Forms.DockStyle.Top;
            this.label62.Location = new System.Drawing.Point(3, 3);
            this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(847, 30);
            this.label62.TabIndex = 328;
            this.label62.Text = "トラップデータ";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPagePalette
            // 
            this.tabPagePalette.BackColor = System.Drawing.SystemColors.Control;
            this.tabPagePalette.Controls.Add(this.PaletteList);
            this.tabPagePalette.Controls.Add(this.PaletteSearchButton);
            this.tabPagePalette.Controls.Add(this.PaletteAddress);
            this.tabPagePalette.Controls.Add(this.SelectPalette);
            this.tabPagePalette.Controls.Add(this.label92);
            this.tabPagePalette.Location = new System.Drawing.Point(4, 52);
            this.tabPagePalette.Name = "tabPagePalette";
            this.tabPagePalette.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePalette.Size = new System.Drawing.Size(853, 820);
            this.tabPagePalette.TabIndex = 2;
            this.tabPagePalette.Text = "パレット";
            // 
            // PaletteList
            // 
            this.PaletteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaletteList.FormattingEnabled = true;
            this.PaletteList.IntegralHeight = false;
            this.PaletteList.ItemHeight = 18;
            this.PaletteList.Location = new System.Drawing.Point(3, 33);
            this.PaletteList.Name = "PaletteList";
            this.PaletteList.Size = new System.Drawing.Size(847, 784);
            this.PaletteList.TabIndex = 321;
            this.PaletteList.SelectedIndexChanged += new System.EventHandler(this.PaletteList_SelectedIndexChanged);
            this.PaletteList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PaletteList_KeyDown);
            this.PaletteList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PaletteList_MouseDoubleClick);
            // 
            // PaletteSearchButton
            // 
            this.PaletteSearchButton.Location = new System.Drawing.Point(659, 1);
            this.PaletteSearchButton.Name = "PaletteSearchButton";
            this.PaletteSearchButton.Size = new System.Drawing.Size(75, 29);
            this.PaletteSearchButton.TabIndex = 324;
            this.PaletteSearchButton.Text = "検索";
            this.PaletteSearchButton.UseVisualStyleBackColor = true;
            this.PaletteSearchButton.Click += new System.EventHandler(this.PaletteSearchButton_Click);
            // 
            // PaletteAddress
            // 
            this.PaletteAddress.ErrorMessage = "";
            this.PaletteAddress.Location = new System.Drawing.Point(734, 5);
            this.PaletteAddress.Margin = new System.Windows.Forms.Padding(2);
            this.PaletteAddress.Name = "PaletteAddress";
            this.PaletteAddress.Placeholder = "";
            this.PaletteAddress.ReadOnly = true;
            this.PaletteAddress.Size = new System.Drawing.Size(116, 25);
            this.PaletteAddress.TabIndex = 327;
            // 
            // SelectPalette
            // 
            this.SelectPalette.ErrorMessage = "";
            this.SelectPalette.Location = new System.Drawing.Point(98, 4);
            this.SelectPalette.Name = "SelectPalette";
            this.SelectPalette.Placeholder = "";
            this.SelectPalette.Size = new System.Drawing.Size(556, 25);
            this.SelectPalette.TabIndex = 322;
            // 
            // label92
            // 
            this.label92.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label92.Dock = System.Windows.Forms.DockStyle.Top;
            this.label92.Location = new System.Drawing.Point(3, 3);
            this.label92.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(847, 30);
            this.label92.TabIndex = 319;
            this.label92.Text = "パレット";
            this.label92.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageClearTurns
            // 
            this.tabPageClearTurns.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageClearTurns.Controls.Add(this.ClearTurnList);
            this.tabPageClearTurns.Controls.Add(this.ClearTurnAddress);
            this.tabPageClearTurns.Controls.Add(this.label82);
            this.tabPageClearTurns.Location = new System.Drawing.Point(4, 52);
            this.tabPageClearTurns.Name = "tabPageClearTurns";
            this.tabPageClearTurns.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageClearTurns.Size = new System.Drawing.Size(853, 820);
            this.tabPageClearTurns.TabIndex = 3;
            this.tabPageClearTurns.Text = "クリアターン数";
            // 
            // ClearTurnList
            // 
            this.ClearTurnList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClearTurnList.FormattingEnabled = true;
            this.ClearTurnList.IntegralHeight = false;
            this.ClearTurnList.ItemHeight = 18;
            this.ClearTurnList.Location = new System.Drawing.Point(3, 33);
            this.ClearTurnList.Name = "ClearTurnList";
            this.ClearTurnList.Size = new System.Drawing.Size(847, 784);
            this.ClearTurnList.TabIndex = 329;
            this.ClearTurnList.SelectedIndexChanged += new System.EventHandler(this.ClearTurnList_SelectedIndexChanged);
            this.ClearTurnList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ClearTurnList_MouseDoubleClick);
            // 
            // ClearTurnAddress
            // 
            this.ClearTurnAddress.ErrorMessage = "";
            this.ClearTurnAddress.Location = new System.Drawing.Point(732, 5);
            this.ClearTurnAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ClearTurnAddress.Name = "ClearTurnAddress";
            this.ClearTurnAddress.Placeholder = "";
            this.ClearTurnAddress.ReadOnly = true;
            this.ClearTurnAddress.Size = new System.Drawing.Size(116, 25);
            this.ClearTurnAddress.TabIndex = 330;
            // 
            // label82
            // 
            this.label82.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label82.Dock = System.Windows.Forms.DockStyle.Top;
            this.label82.Location = new System.Drawing.Point(3, 3);
            this.label82.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(847, 30);
            this.label82.TabIndex = 328;
            this.label82.Text = "クリアターン数";
            this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageBWL
            // 
            this.tabPageBWL.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBWL.Controls.Add(this.BWLList);
            this.tabPageBWL.Controls.Add(this.BWLAddress);
            this.tabPageBWL.Controls.Add(this.label95);
            this.tabPageBWL.Location = new System.Drawing.Point(4, 52);
            this.tabPageBWL.Name = "tabPageBWL";
            this.tabPageBWL.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBWL.Size = new System.Drawing.Size(853, 820);
            this.tabPageBWL.TabIndex = 4;
            this.tabPageBWL.Text = "BWL";
            // 
            // BWLList
            // 
            this.BWLList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BWLList.FormattingEnabled = true;
            this.BWLList.IntegralHeight = false;
            this.BWLList.ItemHeight = 18;
            this.BWLList.Location = new System.Drawing.Point(3, 33);
            this.BWLList.Name = "BWLList";
            this.BWLList.Size = new System.Drawing.Size(847, 784);
            this.BWLList.TabIndex = 330;
            this.BWLList.SelectedIndexChanged += new System.EventHandler(this.BWLList_SelectedIndexChanged);
            this.BWLList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BWLList_MouseDoubleClick);
            // 
            // BWLAddress
            // 
            this.BWLAddress.ErrorMessage = "";
            this.BWLAddress.Location = new System.Drawing.Point(732, 6);
            this.BWLAddress.Margin = new System.Windows.Forms.Padding(2);
            this.BWLAddress.Name = "BWLAddress";
            this.BWLAddress.Placeholder = "";
            this.BWLAddress.ReadOnly = true;
            this.BWLAddress.Size = new System.Drawing.Size(116, 25);
            this.BWLAddress.TabIndex = 332;
            // 
            // label95
            // 
            this.label95.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label95.Dock = System.Windows.Forms.DockStyle.Top;
            this.label95.Location = new System.Drawing.Point(3, 3);
            this.label95.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(847, 30);
            this.label95.TabIndex = 329;
            this.label95.Text = "戦歴データ";
            this.label95.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageChapterData
            // 
            this.tabPageChapterData.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageChapterData.Controls.Add(this.ChapterDataList);
            this.tabPageChapterData.Controls.Add(this.ChapterDataAddress);
            this.tabPageChapterData.Controls.Add(this.label96);
            this.tabPageChapterData.Location = new System.Drawing.Point(4, 52);
            this.tabPageChapterData.Name = "tabPageChapterData";
            this.tabPageChapterData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChapterData.Size = new System.Drawing.Size(853, 820);
            this.tabPageChapterData.TabIndex = 5;
            this.tabPageChapterData.Text = "ChapterData";
            // 
            // ChapterDataList
            // 
            this.ChapterDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChapterDataList.FormattingEnabled = true;
            this.ChapterDataList.IntegralHeight = false;
            this.ChapterDataList.ItemHeight = 18;
            this.ChapterDataList.Location = new System.Drawing.Point(3, 33);
            this.ChapterDataList.Name = "ChapterDataList";
            this.ChapterDataList.Size = new System.Drawing.Size(847, 784);
            this.ChapterDataList.TabIndex = 332;
            this.ChapterDataList.SelectedIndexChanged += new System.EventHandler(this.ChapterDataList_SelectedIndexChanged);
            this.ChapterDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChapterDataList_MouseDoubleClick);
            // 
            // ChapterDataAddress
            // 
            this.ChapterDataAddress.ErrorMessage = "";
            this.ChapterDataAddress.Location = new System.Drawing.Point(732, 5);
            this.ChapterDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ChapterDataAddress.Name = "ChapterDataAddress";
            this.ChapterDataAddress.Placeholder = "";
            this.ChapterDataAddress.ReadOnly = true;
            this.ChapterDataAddress.Size = new System.Drawing.Size(116, 25);
            this.ChapterDataAddress.TabIndex = 333;
            // 
            // label96
            // 
            this.label96.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label96.Dock = System.Windows.Forms.DockStyle.Top;
            this.label96.Location = new System.Drawing.Point(3, 3);
            this.label96.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(847, 30);
            this.label96.TabIndex = 331;
            this.label96.Text = "章データ";
            this.label96.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageSupplyData
            // 
            this.tabPageSupplyData.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSupplyData.Controls.Add(this.SupplyDataAddress);
            this.tabPageSupplyData.Controls.Add(this.SupplyDataList);
            this.tabPageSupplyData.Controls.Add(this.label102);
            this.tabPageSupplyData.Location = new System.Drawing.Point(4, 52);
            this.tabPageSupplyData.Name = "tabPageSupplyData";
            this.tabPageSupplyData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSupplyData.Size = new System.Drawing.Size(853, 820);
            this.tabPageSupplyData.TabIndex = 11;
            this.tabPageSupplyData.Text = "輸送隊";
            // 
            // SupplyDataAddress
            // 
            this.SupplyDataAddress.ErrorMessage = "";
            this.SupplyDataAddress.Location = new System.Drawing.Point(732, 5);
            this.SupplyDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.SupplyDataAddress.Name = "SupplyDataAddress";
            this.SupplyDataAddress.Placeholder = "";
            this.SupplyDataAddress.ReadOnly = true;
            this.SupplyDataAddress.Size = new System.Drawing.Size(116, 25);
            this.SupplyDataAddress.TabIndex = 348;
            // 
            // SupplyDataList
            // 
            this.SupplyDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SupplyDataList.FormattingEnabled = true;
            this.SupplyDataList.IntegralHeight = false;
            this.SupplyDataList.ItemHeight = 18;
            this.SupplyDataList.Location = new System.Drawing.Point(3, 33);
            this.SupplyDataList.Name = "SupplyDataList";
            this.SupplyDataList.Size = new System.Drawing.Size(847, 784);
            this.SupplyDataList.TabIndex = 347;
            this.SupplyDataList.SelectedIndexChanged += new System.EventHandler(this.SupplyDataList_SelectedIndexChanged);
            this.SupplyDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SupplyDataList_MouseDoubleClick);
            // 
            // label102
            // 
            this.label102.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label102.Dock = System.Windows.Forms.DockStyle.Top;
            this.label102.Location = new System.Drawing.Point(3, 3);
            this.label102.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(847, 30);
            this.label102.TabIndex = 346;
            this.label102.Text = "輸送隊の内容";
            this.label102.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageActionData
            // 
            this.tabPageActionData.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageActionData.Controls.Add(this.ActionDataAddress);
            this.tabPageActionData.Controls.Add(this.ActionDataList);
            this.tabPageActionData.Controls.Add(this.label103);
            this.tabPageActionData.Location = new System.Drawing.Point(4, 52);
            this.tabPageActionData.Name = "tabPageActionData";
            this.tabPageActionData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActionData.Size = new System.Drawing.Size(853, 820);
            this.tabPageActionData.TabIndex = 12;
            this.tabPageActionData.Text = "ActionData";
            // 
            // ActionDataAddress
            // 
            this.ActionDataAddress.ErrorMessage = "";
            this.ActionDataAddress.Location = new System.Drawing.Point(732, 5);
            this.ActionDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ActionDataAddress.Name = "ActionDataAddress";
            this.ActionDataAddress.Placeholder = "";
            this.ActionDataAddress.ReadOnly = true;
            this.ActionDataAddress.Size = new System.Drawing.Size(116, 25);
            this.ActionDataAddress.TabIndex = 348;
            // 
            // ActionDataList
            // 
            this.ActionDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionDataList.FormattingEnabled = true;
            this.ActionDataList.IntegralHeight = false;
            this.ActionDataList.ItemHeight = 18;
            this.ActionDataList.Location = new System.Drawing.Point(3, 33);
            this.ActionDataList.Name = "ActionDataList";
            this.ActionDataList.Size = new System.Drawing.Size(847, 784);
            this.ActionDataList.TabIndex = 347;
            this.ActionDataList.SelectedIndexChanged += new System.EventHandler(this.ActionDataList_SelectedIndexChanged);
            this.ActionDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ActionDataList_MouseDoubleClick);
            // 
            // label103
            // 
            this.label103.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label103.Dock = System.Windows.Forms.DockStyle.Top;
            this.label103.Location = new System.Drawing.Point(3, 3);
            this.label103.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(847, 30);
            this.label103.TabIndex = 346;
            this.label103.Text = "ユニットの行動で設定される項目";
            this.label103.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageArenaData
            // 
            this.tabPageArenaData.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageArenaData.Controls.Add(this.ArenaDataAddress);
            this.tabPageArenaData.Controls.Add(this.ArenaDataList);
            this.tabPageArenaData.Controls.Add(this.label100);
            this.tabPageArenaData.Location = new System.Drawing.Point(4, 52);
            this.tabPageArenaData.Name = "tabPageArenaData";
            this.tabPageArenaData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageArenaData.Size = new System.Drawing.Size(853, 820);
            this.tabPageArenaData.TabIndex = 9;
            this.tabPageArenaData.Text = "闘技場";
            // 
            // ArenaDataAddress
            // 
            this.ArenaDataAddress.ErrorMessage = "";
            this.ArenaDataAddress.Location = new System.Drawing.Point(732, 5);
            this.ArenaDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ArenaDataAddress.Name = "ArenaDataAddress";
            this.ArenaDataAddress.Placeholder = "";
            this.ArenaDataAddress.ReadOnly = true;
            this.ArenaDataAddress.Size = new System.Drawing.Size(116, 25);
            this.ArenaDataAddress.TabIndex = 342;
            // 
            // ArenaDataList
            // 
            this.ArenaDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ArenaDataList.FormattingEnabled = true;
            this.ArenaDataList.IntegralHeight = false;
            this.ArenaDataList.ItemHeight = 18;
            this.ArenaDataList.Location = new System.Drawing.Point(3, 33);
            this.ArenaDataList.Name = "ArenaDataList";
            this.ArenaDataList.Size = new System.Drawing.Size(847, 784);
            this.ArenaDataList.TabIndex = 341;
            this.ArenaDataList.SelectedIndexChanged += new System.EventHandler(this.ArenaDataList_SelectedIndexChanged);
            this.ArenaDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ArenaDataList_MouseDoubleClick);
            // 
            // label100
            // 
            this.label100.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label100.Dock = System.Windows.Forms.DockStyle.Top;
            this.label100.Location = new System.Drawing.Point(3, 3);
            this.label100.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(847, 30);
            this.label100.TabIndex = 340;
            this.label100.Text = "闘技場の相手選出に利用するデータ";
            this.label100.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageBattleActor
            // 
            this.tabPageBattleActor.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBattleActor.Controls.Add(this.BattleActorList);
            this.tabPageBattleActor.Controls.Add(this.BattleActorAddress);
            this.tabPageBattleActor.Controls.Add(this.label97);
            this.tabPageBattleActor.Location = new System.Drawing.Point(4, 52);
            this.tabPageBattleActor.Name = "tabPageBattleActor";
            this.tabPageBattleActor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBattleActor.Size = new System.Drawing.Size(853, 820);
            this.tabPageBattleActor.TabIndex = 6;
            this.tabPageBattleActor.Text = "BattleActor";
            // 
            // BattleActorList
            // 
            this.BattleActorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BattleActorList.FormattingEnabled = true;
            this.BattleActorList.IntegralHeight = false;
            this.BattleActorList.ItemHeight = 18;
            this.BattleActorList.Location = new System.Drawing.Point(3, 33);
            this.BattleActorList.Name = "BattleActorList";
            this.BattleActorList.Size = new System.Drawing.Size(847, 784);
            this.BattleActorList.TabIndex = 335;
            this.BattleActorList.SelectedIndexChanged += new System.EventHandler(this.BattleActorList_SelectedIndexChanged);
            this.BattleActorList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BattleActorList_MouseDoubleClick);
            // 
            // BattleActorAddress
            // 
            this.BattleActorAddress.ErrorMessage = "";
            this.BattleActorAddress.Location = new System.Drawing.Point(732, 5);
            this.BattleActorAddress.Margin = new System.Windows.Forms.Padding(2);
            this.BattleActorAddress.Name = "BattleActorAddress";
            this.BattleActorAddress.Placeholder = "";
            this.BattleActorAddress.ReadOnly = true;
            this.BattleActorAddress.Size = new System.Drawing.Size(116, 25);
            this.BattleActorAddress.TabIndex = 336;
            // 
            // label97
            // 
            this.label97.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label97.Dock = System.Windows.Forms.DockStyle.Top;
            this.label97.Location = new System.Drawing.Point(3, 3);
            this.label97.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(847, 30);
            this.label97.TabIndex = 334;
            this.label97.Text = "戦闘データ gBattleActor";
            this.label97.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageBattleTarget
            // 
            this.tabPageBattleTarget.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBattleTarget.Controls.Add(this.BattleTargetList);
            this.tabPageBattleTarget.Controls.Add(this.BattleTargetAddress);
            this.tabPageBattleTarget.Controls.Add(this.label98);
            this.tabPageBattleTarget.Location = new System.Drawing.Point(4, 52);
            this.tabPageBattleTarget.Name = "tabPageBattleTarget";
            this.tabPageBattleTarget.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBattleTarget.Size = new System.Drawing.Size(853, 820);
            this.tabPageBattleTarget.TabIndex = 7;
            this.tabPageBattleTarget.Text = "BattleTarget";
            // 
            // BattleTargetList
            // 
            this.BattleTargetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BattleTargetList.FormattingEnabled = true;
            this.BattleTargetList.IntegralHeight = false;
            this.BattleTargetList.ItemHeight = 18;
            this.BattleTargetList.Location = new System.Drawing.Point(3, 33);
            this.BattleTargetList.Name = "BattleTargetList";
            this.BattleTargetList.Size = new System.Drawing.Size(847, 784);
            this.BattleTargetList.TabIndex = 335;
            this.BattleTargetList.SelectedIndexChanged += new System.EventHandler(this.BattleTargetList_SelectedIndexChanged);
            this.BattleTargetList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BattleTargetList_MouseDoubleClick);
            // 
            // BattleTargetAddress
            // 
            this.BattleTargetAddress.ErrorMessage = "";
            this.BattleTargetAddress.Location = new System.Drawing.Point(732, 5);
            this.BattleTargetAddress.Margin = new System.Windows.Forms.Padding(2);
            this.BattleTargetAddress.Name = "BattleTargetAddress";
            this.BattleTargetAddress.Placeholder = "";
            this.BattleTargetAddress.ReadOnly = true;
            this.BattleTargetAddress.Size = new System.Drawing.Size(116, 25);
            this.BattleTargetAddress.TabIndex = 336;
            // 
            // label98
            // 
            this.label98.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label98.Dock = System.Windows.Forms.DockStyle.Top;
            this.label98.Location = new System.Drawing.Point(3, 3);
            this.label98.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(847, 30);
            this.label98.TabIndex = 334;
            this.label98.Text = "戦闘データ gBattleTarget";
            this.label98.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageAIData
            // 
            this.tabPageAIData.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAIData.Controls.Add(this.AIDataAddress);
            this.tabPageAIData.Controls.Add(this.AIDataList);
            this.tabPageAIData.Controls.Add(this.label101);
            this.tabPageAIData.Location = new System.Drawing.Point(4, 52);
            this.tabPageAIData.Name = "tabPageAIData";
            this.tabPageAIData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAIData.Size = new System.Drawing.Size(853, 820);
            this.tabPageAIData.TabIndex = 10;
            this.tabPageAIData.Text = "AIData";
            // 
            // AIDataAddress
            // 
            this.AIDataAddress.ErrorMessage = "";
            this.AIDataAddress.Location = new System.Drawing.Point(732, 5);
            this.AIDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.AIDataAddress.Name = "AIDataAddress";
            this.AIDataAddress.Placeholder = "";
            this.AIDataAddress.ReadOnly = true;
            this.AIDataAddress.Size = new System.Drawing.Size(116, 25);
            this.AIDataAddress.TabIndex = 345;
            // 
            // AIDataList
            // 
            this.AIDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AIDataList.FormattingEnabled = true;
            this.AIDataList.IntegralHeight = false;
            this.AIDataList.ItemHeight = 18;
            this.AIDataList.Location = new System.Drawing.Point(3, 33);
            this.AIDataList.Name = "AIDataList";
            this.AIDataList.Size = new System.Drawing.Size(847, 784);
            this.AIDataList.TabIndex = 344;
            this.AIDataList.SelectedIndexChanged += new System.EventHandler(this.AIDataList_SelectedIndexChanged);
            this.AIDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AIDataList_MouseDoubleClick);
            // 
            // label101
            // 
            this.label101.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label101.Dock = System.Windows.Forms.DockStyle.Top;
            this.label101.Location = new System.Drawing.Point(3, 3);
            this.label101.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(847, 30);
            this.label101.TabIndex = 343;
            this.label101.Text = "AIData";
            this.label101.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageBattleRound
            // 
            this.tabPageBattleRound.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBattleRound.Controls.Add(this.BattleRoundDataAddress);
            this.tabPageBattleRound.Controls.Add(this.BattleRoundDataList);
            this.tabPageBattleRound.Controls.Add(this.label106);
            this.tabPageBattleRound.Location = new System.Drawing.Point(4, 52);
            this.tabPageBattleRound.Name = "tabPageBattleRound";
            this.tabPageBattleRound.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBattleRound.Size = new System.Drawing.Size(853, 820);
            this.tabPageBattleRound.TabIndex = 15;
            this.tabPageBattleRound.Text = "BattleRound";
            // 
            // BattleRoundDataAddress
            // 
            this.BattleRoundDataAddress.ErrorMessage = "";
            this.BattleRoundDataAddress.Location = new System.Drawing.Point(732, 5);
            this.BattleRoundDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.BattleRoundDataAddress.Name = "BattleRoundDataAddress";
            this.BattleRoundDataAddress.Placeholder = "";
            this.BattleRoundDataAddress.ReadOnly = true;
            this.BattleRoundDataAddress.Size = new System.Drawing.Size(116, 25);
            this.BattleRoundDataAddress.TabIndex = 354;
            // 
            // BattleRoundDataList
            // 
            this.BattleRoundDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BattleRoundDataList.FormattingEnabled = true;
            this.BattleRoundDataList.IntegralHeight = false;
            this.BattleRoundDataList.ItemHeight = 18;
            this.BattleRoundDataList.Location = new System.Drawing.Point(3, 33);
            this.BattleRoundDataList.Name = "BattleRoundDataList";
            this.BattleRoundDataList.Size = new System.Drawing.Size(847, 784);
            this.BattleRoundDataList.TabIndex = 353;
            this.BattleRoundDataList.SelectedIndexChanged += new System.EventHandler(this.BattleRoundDataList_SelectedIndexChanged);
            this.BattleRoundDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BattleRoundDataList_MouseDoubleClick);
            // 
            // label106
            // 
            this.label106.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label106.Dock = System.Windows.Forms.DockStyle.Top;
            this.label106.Location = new System.Drawing.Point(3, 3);
            this.label106.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(847, 30);
            this.label106.TabIndex = 352;
            this.label106.Text = "BattleRounds";
            this.label106.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageBattleSome
            // 
            this.tabPageBattleSome.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBattleSome.Controls.Add(this.BattleSomeDataAddress);
            this.tabPageBattleSome.Controls.Add(this.BattleSomeDataList);
            this.tabPageBattleSome.Controls.Add(this.label105);
            this.tabPageBattleSome.Location = new System.Drawing.Point(4, 52);
            this.tabPageBattleSome.Name = "tabPageBattleSome";
            this.tabPageBattleSome.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBattleSome.Size = new System.Drawing.Size(853, 820);
            this.tabPageBattleSome.TabIndex = 14;
            this.tabPageBattleSome.Text = "BattleSome";
            // 
            // BattleSomeDataAddress
            // 
            this.BattleSomeDataAddress.ErrorMessage = "";
            this.BattleSomeDataAddress.Location = new System.Drawing.Point(732, 5);
            this.BattleSomeDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.BattleSomeDataAddress.Name = "BattleSomeDataAddress";
            this.BattleSomeDataAddress.Placeholder = "";
            this.BattleSomeDataAddress.ReadOnly = true;
            this.BattleSomeDataAddress.Size = new System.Drawing.Size(116, 25);
            this.BattleSomeDataAddress.TabIndex = 354;
            // 
            // BattleSomeDataList
            // 
            this.BattleSomeDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BattleSomeDataList.FormattingEnabled = true;
            this.BattleSomeDataList.IntegralHeight = false;
            this.BattleSomeDataList.ItemHeight = 18;
            this.BattleSomeDataList.Location = new System.Drawing.Point(3, 33);
            this.BattleSomeDataList.Name = "BattleSomeDataList";
            this.BattleSomeDataList.Size = new System.Drawing.Size(847, 784);
            this.BattleSomeDataList.TabIndex = 353;
            this.BattleSomeDataList.SelectedIndexChanged += new System.EventHandler(this.BattleSomeList_SelectedIndexChanged);
            this.BattleSomeDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BattleSomeList_MouseDoubleClick);
            // 
            // label105
            // 
            this.label105.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label105.Dock = System.Windows.Forms.DockStyle.Top;
            this.label105.Location = new System.Drawing.Point(3, 3);
            this.label105.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(847, 30);
            this.label105.TabIndex = 352;
            this.label105.Text = "戦闘に関係する諸データ";
            this.label105.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageWorldmap
            // 
            this.tabPageWorldmap.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageWorldmap.Controls.Add(this.WorldmapAddress);
            this.tabPageWorldmap.Controls.Add(this.WorldmapList);
            this.tabPageWorldmap.Controls.Add(this.label99);
            this.tabPageWorldmap.Location = new System.Drawing.Point(4, 52);
            this.tabPageWorldmap.Name = "tabPageWorldmap";
            this.tabPageWorldmap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWorldmap.Size = new System.Drawing.Size(853, 820);
            this.tabPageWorldmap.TabIndex = 8;
            this.tabPageWorldmap.Text = "Worldmap";
            // 
            // WorldmapAddress
            // 
            this.WorldmapAddress.ErrorMessage = "";
            this.WorldmapAddress.Location = new System.Drawing.Point(732, 5);
            this.WorldmapAddress.Margin = new System.Windows.Forms.Padding(2);
            this.WorldmapAddress.Name = "WorldmapAddress";
            this.WorldmapAddress.Placeholder = "";
            this.WorldmapAddress.ReadOnly = true;
            this.WorldmapAddress.Size = new System.Drawing.Size(116, 25);
            this.WorldmapAddress.TabIndex = 339;
            // 
            // WorldmapList
            // 
            this.WorldmapList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorldmapList.FormattingEnabled = true;
            this.WorldmapList.IntegralHeight = false;
            this.WorldmapList.ItemHeight = 18;
            this.WorldmapList.Location = new System.Drawing.Point(3, 33);
            this.WorldmapList.Name = "WorldmapList";
            this.WorldmapList.Size = new System.Drawing.Size(847, 784);
            this.WorldmapList.TabIndex = 338;
            this.WorldmapList.SelectedIndexChanged += new System.EventHandler(this.WorldmapList_SelectedIndexChanged);
            this.WorldmapList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.WorldmapList_MouseDoubleClick);
            // 
            // label99
            // 
            this.label99.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label99.Dock = System.Windows.Forms.DockStyle.Top;
            this.label99.Location = new System.Drawing.Point(3, 3);
            this.label99.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(847, 30);
            this.label99.TabIndex = 337;
            this.label99.Text = "Worldmap FE8";
            this.label99.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageDungeon
            // 
            this.tabPageDungeon.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDungeon.Controls.Add(this.DungeonDataAddress);
            this.tabPageDungeon.Controls.Add(this.DungeonDataList);
            this.tabPageDungeon.Controls.Add(this.label104);
            this.tabPageDungeon.Location = new System.Drawing.Point(4, 52);
            this.tabPageDungeon.Name = "tabPageDungeon";
            this.tabPageDungeon.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDungeon.Size = new System.Drawing.Size(853, 820);
            this.tabPageDungeon.TabIndex = 13;
            this.tabPageDungeon.Text = "Dungeon";
            // 
            // DungeonDataAddress
            // 
            this.DungeonDataAddress.ErrorMessage = "";
            this.DungeonDataAddress.Location = new System.Drawing.Point(732, 5);
            this.DungeonDataAddress.Margin = new System.Windows.Forms.Padding(2);
            this.DungeonDataAddress.Name = "DungeonDataAddress";
            this.DungeonDataAddress.Placeholder = "";
            this.DungeonDataAddress.ReadOnly = true;
            this.DungeonDataAddress.Size = new System.Drawing.Size(116, 25);
            this.DungeonDataAddress.TabIndex = 351;
            // 
            // DungeonDataList
            // 
            this.DungeonDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DungeonDataList.FormattingEnabled = true;
            this.DungeonDataList.IntegralHeight = false;
            this.DungeonDataList.ItemHeight = 18;
            this.DungeonDataList.Location = new System.Drawing.Point(3, 33);
            this.DungeonDataList.Name = "DungeonDataList";
            this.DungeonDataList.Size = new System.Drawing.Size(847, 784);
            this.DungeonDataList.TabIndex = 350;
            this.DungeonDataList.SelectedIndexChanged += new System.EventHandler(this.DungeonDataList_SelectedIndexChanged);
            this.DungeonDataList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DungeonDataList_MouseDoubleClick);
            // 
            // label104
            // 
            this.label104.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label104.Dock = System.Windows.Forms.DockStyle.Top;
            this.label104.Location = new System.Drawing.Point(3, 3);
            this.label104.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(847, 30);
            this.label104.TabIndex = 349;
            this.label104.Text = "塔と遺跡のデータ";
            this.label104.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Party_ControlPanel
            // 
            this.Party_ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Party_ControlPanel.Controls.Add(this.Party_CloseButton);
            this.Party_ControlPanel.Controls.Add(this.PARTY_Address);
            this.Party_ControlPanel.Controls.Add(this.PARTY_SelectAddress);
            this.Party_ControlPanel.Controls.Add(this.label2);
            this.Party_ControlPanel.Controls.Add(this.label4);
            this.Party_ControlPanel.Controls.Add(this.panel3);
            this.Party_ControlPanel.Controls.Add(this.button1);
            this.Party_ControlPanel.Location = new System.Drawing.Point(0, 151);
            this.Party_ControlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.Party_ControlPanel.Name = "Party_ControlPanel";
            this.Party_ControlPanel.Size = new System.Drawing.Size(778, 730);
            this.Party_ControlPanel.TabIndex = 312;
            this.Party_ControlPanel.Visible = false;
            // 
            // Party_CloseButton
            // 
            this.Party_CloseButton.Location = new System.Drawing.Point(710, -2);
            this.Party_CloseButton.Margin = new System.Windows.Forms.Padding(2);
            this.Party_CloseButton.Name = "Party_CloseButton";
            this.Party_CloseButton.Size = new System.Drawing.Size(67, 32);
            this.Party_CloseButton.TabIndex = 305;
            this.Party_CloseButton.Text = "Close";
            this.Party_CloseButton.UseVisualStyleBackColor = true;
            this.Party_CloseButton.Click += new System.EventHandler(this.Party_CloseButton_Click);
            // 
            // PARTY_Address
            // 
            this.PARTY_Address.Hexadecimal = true;
            this.PARTY_Address.Location = new System.Drawing.Point(152, 5);
            this.PARTY_Address.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_Address.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_Address.Name = "PARTY_Address";
            this.PARTY_Address.ReadOnly = true;
            this.PARTY_Address.Size = new System.Drawing.Size(160, 25);
            this.PARTY_Address.TabIndex = 304;
            // 
            // PARTY_SelectAddress
            // 
            this.PARTY_SelectAddress.ErrorMessage = "";
            this.PARTY_SelectAddress.Location = new System.Drawing.Point(444, 4);
            this.PARTY_SelectAddress.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.PARTY_SelectAddress.Name = "PARTY_SelectAddress";
            this.PARTY_SelectAddress.Placeholder = "";
            this.PARTY_SelectAddress.ReadOnly = true;
            this.PARTY_SelectAddress.Size = new System.Drawing.Size(259, 25);
            this.PARTY_SelectAddress.TabIndex = 303;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(318, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 30);
            this.label2.TabIndex = 302;
            this.label2.Text = "選択アドレス:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(5, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 24);
            this.label4.TabIndex = 301;
            this.label4.Text = "アドレス";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.PARTY_AI2_TEXT);
            this.panel3.Controls.Add(this.PARTY_AI1_TEXT);
            this.panel3.Controls.Add(this.PARTY_PORTRAIT);
            this.panel3.Controls.Add(this.PARTY_L_38_ITEMICON);
            this.panel3.Controls.Add(this.PARTY_L_36_ITEMICON);
            this.panel3.Controls.Add(this.PARTY_L_34_ITEMICON);
            this.panel3.Controls.Add(this.PARTY_L_32_ITEMICON);
            this.panel3.Controls.Add(this.PARTY_L_30_ITEMICON);
            this.panel3.Controls.Add(this.PARTY_B71);
            this.panel3.Controls.Add(this.label89);
            this.panel3.Controls.Add(this.PARTY_B70);
            this.panel3.Controls.Add(this.label90);
            this.panel3.Controls.Add(this.PARTY_B69);
            this.panel3.Controls.Add(this.label83);
            this.panel3.Controls.Add(this.PARTY_B68);
            this.panel3.Controls.Add(this.label84);
            this.panel3.Controls.Add(this.PARTY_B67);
            this.panel3.Controls.Add(this.label85);
            this.panel3.Controls.Add(this.PARTY_B66);
            this.panel3.Controls.Add(this.label86);
            this.panel3.Controls.Add(this.PARTY_B65);
            this.panel3.Controls.Add(this.label87);
            this.panel3.Controls.Add(this.PARTY_B64);
            this.panel3.Controls.Add(this.label88);
            this.panel3.Controls.Add(this.PARTY_B63);
            this.panel3.Controls.Add(this.label69);
            this.panel3.Controls.Add(this.PARTY_B62);
            this.panel3.Controls.Add(this.label78);
            this.panel3.Controls.Add(this.PARTY_B61);
            this.panel3.Controls.Add(this.label79);
            this.panel3.Controls.Add(this.PARTY_B60);
            this.panel3.Controls.Add(this.label80);
            this.panel3.Controls.Add(this.PARTY_B59);
            this.panel3.Controls.Add(this.label81);
            this.panel3.Controls.Add(this.PARTY_B58);
            this.panel3.Controls.Add(this.PARTY_J_58);
            this.panel3.Controls.Add(this.PARTY_B57);
            this.panel3.Controls.Add(this.label70);
            this.panel3.Controls.Add(this.PARTY_B56);
            this.panel3.Controls.Add(this.label71);
            this.panel3.Controls.Add(this.PARTY_B55);
            this.panel3.Controls.Add(this.label72);
            this.panel3.Controls.Add(this.PARTY_B54);
            this.panel3.Controls.Add(this.label73);
            this.panel3.Controls.Add(this.PARTY_B53);
            this.panel3.Controls.Add(this.label74);
            this.panel3.Controls.Add(this.PARTY_B52);
            this.panel3.Controls.Add(this.label75);
            this.panel3.Controls.Add(this.PARTY_B51);
            this.panel3.Controls.Add(this.label76);
            this.panel3.Controls.Add(this.PARTY_B50);
            this.panel3.Controls.Add(this.label77);
            this.panel3.Controls.Add(this.PARTY_B49);
            this.panel3.Controls.Add(this.label67);
            this.panel3.Controls.Add(this.PARTY_B48);
            this.panel3.Controls.Add(this.label68);
            this.panel3.Controls.Add(this.PARTY_B47);
            this.panel3.Controls.Add(this.label45);
            this.panel3.Controls.Add(this.PARTY_B46);
            this.panel3.Controls.Add(this.label47);
            this.panel3.Controls.Add(this.PARTY_B45);
            this.panel3.Controls.Add(this.label49);
            this.panel3.Controls.Add(this.PARTY_B44);
            this.panel3.Controls.Add(this.label51);
            this.panel3.Controls.Add(this.PARTY_B43);
            this.panel3.Controls.Add(this.label42);
            this.panel3.Controls.Add(this.PARTY_B42);
            this.panel3.Controls.Add(this.label44);
            this.panel3.Controls.Add(this.PARTY_B41);
            this.panel3.Controls.Add(this.label41);
            this.panel3.Controls.Add(this.PARTY_B40);
            this.panel3.Controls.Add(this.label38);
            this.panel3.Controls.Add(this.PARTY_L_38_ITEM);
            this.panel3.Controls.Add(this.PARTY_B39);
            this.panel3.Controls.Add(this.label34);
            this.panel3.Controls.Add(this.PARTY_B38);
            this.panel3.Controls.Add(this.label37);
            this.panel3.Controls.Add(this.PARTY_L_36_ITEM);
            this.panel3.Controls.Add(this.PARTY_B37);
            this.panel3.Controls.Add(this.label24);
            this.panel3.Controls.Add(this.PARTY_B36);
            this.panel3.Controls.Add(this.label29);
            this.panel3.Controls.Add(this.PARTY_L_34_ITEM);
            this.panel3.Controls.Add(this.PARTY_B35);
            this.panel3.Controls.Add(this.label31);
            this.panel3.Controls.Add(this.PARTY_B34);
            this.panel3.Controls.Add(this.label32);
            this.panel3.Controls.Add(this.PARTY_L_32_ITEM);
            this.panel3.Controls.Add(this.PARTY_B33);
            this.panel3.Controls.Add(this.label20);
            this.panel3.Controls.Add(this.PARTY_B32);
            this.panel3.Controls.Add(this.label23);
            this.panel3.Controls.Add(this.PARTY_L_30_ITEM);
            this.panel3.Controls.Add(this.PARTY_B31);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.PARTY_B30);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.PARTY_B28);
            this.panel3.Controls.Add(this.label66);
            this.panel3.Controls.Add(this.PARTY_B29);
            this.panel3.Controls.Add(this.label65);
            this.panel3.Controls.Add(this.PARTY_RAMUNITAID);
            this.panel3.Controls.Add(this.PARTY_B27);
            this.panel3.Controls.Add(this.label64);
            this.panel3.Controls.Add(this.PARTY_B26);
            this.panel3.Controls.Add(this.PARTY_J_26);
            this.panel3.Controls.Add(this.PARTY_B25);
            this.panel3.Controls.Add(this.label63);
            this.panel3.Controls.Add(this.PARTY_B24);
            this.panel3.Controls.Add(this.label60);
            this.panel3.Controls.Add(this.PARTY_B23);
            this.panel3.Controls.Add(this.label61);
            this.panel3.Controls.Add(this.PARTY_B22);
            this.panel3.Controls.Add(this.label59);
            this.panel3.Controls.Add(this.PARTY_B21);
            this.panel3.Controls.Add(this.label57);
            this.panel3.Controls.Add(this.PARTY_RAMUNITSTATE);
            this.panel3.Controls.Add(this.PARTY_ROMCLASSPOINTER);
            this.panel3.Controls.Add(this.PARTY_B10);
            this.panel3.Controls.Add(this.label21);
            this.panel3.Controls.Add(this.textBoxEx52);
            this.panel3.Controls.Add(this.numericUpDown59);
            this.panel3.Controls.Add(this.label28);
            this.panel3.Controls.Add(this.textBoxEx53);
            this.panel3.Controls.Add(this.numericUpDown61);
            this.panel3.Controls.Add(this.label30);
            this.panel3.Controls.Add(this.textBoxEx54);
            this.panel3.Controls.Add(this.numericUpDown64);
            this.panel3.Controls.Add(this.label33);
            this.panel3.Controls.Add(this.PARTY_B19);
            this.panel3.Controls.Add(this.label36);
            this.panel3.Controls.Add(this.PARTY_B20);
            this.panel3.Controls.Add(this.label39);
            this.panel3.Controls.Add(this.PARTY_B18);
            this.panel3.Controls.Add(this.label40);
            this.panel3.Controls.Add(this.PARTY_B17);
            this.panel3.Controls.Add(this.label43);
            this.panel3.Controls.Add(this.PARTY_B16);
            this.panel3.Controls.Add(this.label46);
            this.panel3.Controls.Add(this.PARTY_D12);
            this.panel3.Controls.Add(this.label48);
            this.panel3.Controls.Add(this.PARTY_B11);
            this.panel3.Controls.Add(this.label50);
            this.panel3.Controls.Add(this.PARTY_B9);
            this.panel3.Controls.Add(this.label52);
            this.panel3.Controls.Add(this.PARTY_B8);
            this.panel3.Controls.Add(this.label53);
            this.panel3.Controls.Add(this.PARTY_P4);
            this.panel3.Controls.Add(this.label54);
            this.panel3.Controls.Add(this.PARTY_ROMUNITPOINTER);
            this.panel3.Controls.Add(this.PARTY_P0);
            this.panel3.Controls.Add(this.label55);
            this.panel3.Location = new System.Drawing.Point(0, 30);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(777, 695);
            this.panel3.TabIndex = 0;
            // 
            // PARTY_AI2_TEXT
            // 
            this.PARTY_AI2_TEXT.ErrorMessage = "";
            this.PARTY_AI2_TEXT.Location = new System.Drawing.Point(620, 558);
            this.PARTY_AI2_TEXT.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_AI2_TEXT.Multiline = true;
            this.PARTY_AI2_TEXT.Name = "PARTY_AI2_TEXT";
            this.PARTY_AI2_TEXT.Placeholder = "";
            this.PARTY_AI2_TEXT.ReadOnly = true;
            this.PARTY_AI2_TEXT.Size = new System.Drawing.Size(154, 68);
            this.PARTY_AI2_TEXT.TabIndex = 435;
            // 
            // PARTY_AI1_TEXT
            // 
            this.PARTY_AI1_TEXT.ErrorMessage = "";
            this.PARTY_AI1_TEXT.Location = new System.Drawing.Point(620, 497);
            this.PARTY_AI1_TEXT.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_AI1_TEXT.Multiline = true;
            this.PARTY_AI1_TEXT.Name = "PARTY_AI1_TEXT";
            this.PARTY_AI1_TEXT.Placeholder = "";
            this.PARTY_AI1_TEXT.ReadOnly = true;
            this.PARTY_AI1_TEXT.Size = new System.Drawing.Size(154, 57);
            this.PARTY_AI1_TEXT.TabIndex = 434;
            // 
            // PARTY_PORTRAIT
            // 
            this.PARTY_PORTRAIT.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PARTY_PORTRAIT.Location = new System.Drawing.Point(260, 153);
            this.PARTY_PORTRAIT.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.PARTY_PORTRAIT.Name = "PARTY_PORTRAIT";
            this.PARTY_PORTRAIT.Size = new System.Drawing.Size(118, 104);
            this.PARTY_PORTRAIT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PARTY_PORTRAIT.TabIndex = 433;
            this.PARTY_PORTRAIT.TabStop = false;
            // 
            // PARTY_L_38_ITEMICON
            // 
            this.PARTY_L_38_ITEMICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PARTY_L_38_ITEMICON.Location = new System.Drawing.Point(590, 386);
            this.PARTY_L_38_ITEMICON.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_38_ITEMICON.Name = "PARTY_L_38_ITEMICON";
            this.PARTY_L_38_ITEMICON.Size = new System.Drawing.Size(32, 32);
            this.PARTY_L_38_ITEMICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PARTY_L_38_ITEMICON.TabIndex = 432;
            this.PARTY_L_38_ITEMICON.TabStop = false;
            // 
            // PARTY_L_36_ITEMICON
            // 
            this.PARTY_L_36_ITEMICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PARTY_L_36_ITEMICON.Location = new System.Drawing.Point(590, 354);
            this.PARTY_L_36_ITEMICON.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_36_ITEMICON.Name = "PARTY_L_36_ITEMICON";
            this.PARTY_L_36_ITEMICON.Size = new System.Drawing.Size(32, 32);
            this.PARTY_L_36_ITEMICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PARTY_L_36_ITEMICON.TabIndex = 431;
            this.PARTY_L_36_ITEMICON.TabStop = false;
            // 
            // PARTY_L_34_ITEMICON
            // 
            this.PARTY_L_34_ITEMICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PARTY_L_34_ITEMICON.Location = new System.Drawing.Point(590, 323);
            this.PARTY_L_34_ITEMICON.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_34_ITEMICON.Name = "PARTY_L_34_ITEMICON";
            this.PARTY_L_34_ITEMICON.Size = new System.Drawing.Size(32, 32);
            this.PARTY_L_34_ITEMICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PARTY_L_34_ITEMICON.TabIndex = 430;
            this.PARTY_L_34_ITEMICON.TabStop = false;
            // 
            // PARTY_L_32_ITEMICON
            // 
            this.PARTY_L_32_ITEMICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PARTY_L_32_ITEMICON.Location = new System.Drawing.Point(590, 291);
            this.PARTY_L_32_ITEMICON.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_32_ITEMICON.Name = "PARTY_L_32_ITEMICON";
            this.PARTY_L_32_ITEMICON.Size = new System.Drawing.Size(32, 32);
            this.PARTY_L_32_ITEMICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PARTY_L_32_ITEMICON.TabIndex = 429;
            this.PARTY_L_32_ITEMICON.TabStop = false;
            // 
            // PARTY_L_30_ITEMICON
            // 
            this.PARTY_L_30_ITEMICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.PARTY_L_30_ITEMICON.Location = new System.Drawing.Point(590, 260);
            this.PARTY_L_30_ITEMICON.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_30_ITEMICON.Name = "PARTY_L_30_ITEMICON";
            this.PARTY_L_30_ITEMICON.Size = new System.Drawing.Size(32, 32);
            this.PARTY_L_30_ITEMICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PARTY_L_30_ITEMICON.TabIndex = 428;
            this.PARTY_L_30_ITEMICON.TabStop = false;
            // 
            // PARTY_B71
            // 
            this.PARTY_B71.Hexadecimal = true;
            this.PARTY_B71.Location = new System.Drawing.Point(537, 643);
            this.PARTY_B71.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B71.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B71.Name = "PARTY_B71";
            this.PARTY_B71.ReadOnly = true;
            this.PARTY_B71.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B71.TabIndex = 427;
            // 
            // label89
            // 
            this.label89.Location = new System.Drawing.Point(471, 645);
            this.label89.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(65, 18);
            this.label89.TabIndex = 426;
            this.label89.Text = "??? 9";
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B70
            // 
            this.PARTY_B70.Hexadecimal = true;
            this.PARTY_B70.Location = new System.Drawing.Point(537, 614);
            this.PARTY_B70.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B70.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B70.Name = "PARTY_B70";
            this.PARTY_B70.ReadOnly = true;
            this.PARTY_B70.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B70.TabIndex = 425;
            // 
            // label90
            // 
            this.label90.Location = new System.Drawing.Point(471, 616);
            this.label90.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(65, 18);
            this.label90.TabIndex = 424;
            this.label90.Text = "??? 8";
            this.label90.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B69
            // 
            this.PARTY_B69.Hexadecimal = true;
            this.PARTY_B69.Location = new System.Drawing.Point(537, 585);
            this.PARTY_B69.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B69.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B69.Name = "PARTY_B69";
            this.PARTY_B69.ReadOnly = true;
            this.PARTY_B69.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B69.TabIndex = 423;
            // 
            // label83
            // 
            this.label83.Location = new System.Drawing.Point(432, 587);
            this.label83.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(101, 18);
            this.label83.TabIndex = 422;
            this.label83.Text = "AI2 Counter";
            this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B68
            // 
            this.PARTY_B68.Hexadecimal = true;
            this.PARTY_B68.Location = new System.Drawing.Point(537, 556);
            this.PARTY_B68.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B68.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B68.Name = "PARTY_B68";
            this.PARTY_B68.ReadOnly = true;
            this.PARTY_B68.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B68.TabIndex = 421;
            // 
            // label84
            // 
            this.label84.Location = new System.Drawing.Point(432, 558);
            this.label84.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(101, 18);
            this.label84.TabIndex = 420;
            this.label84.Text = "AI2";
            this.label84.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B67
            // 
            this.PARTY_B67.Hexadecimal = true;
            this.PARTY_B67.Location = new System.Drawing.Point(537, 527);
            this.PARTY_B67.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B67.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B67.Name = "PARTY_B67";
            this.PARTY_B67.ReadOnly = true;
            this.PARTY_B67.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B67.TabIndex = 419;
            // 
            // label85
            // 
            this.label85.Location = new System.Drawing.Point(432, 529);
            this.label85.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(101, 18);
            this.label85.TabIndex = 418;
            this.label85.Text = "AI1 Counter";
            this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B66
            // 
            this.PARTY_B66.Hexadecimal = true;
            this.PARTY_B66.Location = new System.Drawing.Point(537, 498);
            this.PARTY_B66.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B66.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B66.Name = "PARTY_B66";
            this.PARTY_B66.ReadOnly = true;
            this.PARTY_B66.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B66.TabIndex = 417;
            // 
            // label86
            // 
            this.label86.Location = new System.Drawing.Point(432, 500);
            this.label86.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(101, 18);
            this.label86.TabIndex = 416;
            this.label86.Text = "AI1";
            this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B65
            // 
            this.PARTY_B65.Hexadecimal = true;
            this.PARTY_B65.Location = new System.Drawing.Point(537, 471);
            this.PARTY_B65.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B65.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B65.Name = "PARTY_B65";
            this.PARTY_B65.ReadOnly = true;
            this.PARTY_B65.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B65.TabIndex = 415;
            // 
            // label87
            // 
            this.label87.Location = new System.Drawing.Point(432, 473);
            this.label87.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(101, 18);
            this.label87.TabIndex = 414;
            this.label87.Text = "AI4";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B64
            // 
            this.PARTY_B64.Hexadecimal = true;
            this.PARTY_B64.Location = new System.Drawing.Point(537, 442);
            this.PARTY_B64.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B64.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B64.Name = "PARTY_B64";
            this.PARTY_B64.ReadOnly = true;
            this.PARTY_B64.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B64.TabIndex = 413;
            // 
            // label88
            // 
            this.label88.Location = new System.Drawing.Point(432, 444);
            this.label88.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(101, 18);
            this.label88.TabIndex = 412;
            this.label88.Text = "AI3";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B63
            // 
            this.PARTY_B63.Hexadecimal = true;
            this.PARTY_B63.Location = new System.Drawing.Point(693, 403);
            this.PARTY_B63.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B63.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B63.Name = "PARTY_B63";
            this.PARTY_B63.ReadOnly = true;
            this.PARTY_B63.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B63.TabIndex = 411;
            // 
            // label69
            // 
            this.label69.Location = new System.Drawing.Point(629, 405);
            this.label69.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(57, 18);
            this.label69.TabIndex = 410;
            this.label69.Text = "??? 7";
            this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B62
            // 
            this.PARTY_B62.Hexadecimal = true;
            this.PARTY_B62.Location = new System.Drawing.Point(693, 374);
            this.PARTY_B62.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B62.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B62.Name = "PARTY_B62";
            this.PARTY_B62.ReadOnly = true;
            this.PARTY_B62.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B62.TabIndex = 409;
            // 
            // label78
            // 
            this.label78.Location = new System.Drawing.Point(629, 376);
            this.label78.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(57, 18);
            this.label78.TabIndex = 408;
            this.label78.Text = "??? 6";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B61
            // 
            this.PARTY_B61.Hexadecimal = true;
            this.PARTY_B61.Location = new System.Drawing.Point(693, 345);
            this.PARTY_B61.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B61.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B61.Name = "PARTY_B61";
            this.PARTY_B61.ReadOnly = true;
            this.PARTY_B61.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B61.TabIndex = 407;
            // 
            // label79
            // 
            this.label79.Location = new System.Drawing.Point(629, 347);
            this.label79.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(57, 18);
            this.label79.TabIndex = 406;
            this.label79.Text = "??? 5";
            this.label79.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B60
            // 
            this.PARTY_B60.Hexadecimal = true;
            this.PARTY_B60.Location = new System.Drawing.Point(693, 316);
            this.PARTY_B60.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B60.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B60.Name = "PARTY_B60";
            this.PARTY_B60.ReadOnly = true;
            this.PARTY_B60.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B60.TabIndex = 405;
            // 
            // label80
            // 
            this.label80.Location = new System.Drawing.Point(629, 318);
            this.label80.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(57, 18);
            this.label80.TabIndex = 404;
            this.label80.Text = "??? 4";
            this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B59
            // 
            this.PARTY_B59.Hexadecimal = true;
            this.PARTY_B59.Location = new System.Drawing.Point(693, 289);
            this.PARTY_B59.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B59.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B59.Name = "PARTY_B59";
            this.PARTY_B59.ReadOnly = true;
            this.PARTY_B59.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B59.TabIndex = 403;
            // 
            // label81
            // 
            this.label81.Location = new System.Drawing.Point(629, 291);
            this.label81.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(57, 18);
            this.label81.TabIndex = 402;
            this.label81.Text = "??? 3";
            this.label81.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B58
            // 
            this.PARTY_B58.Hexadecimal = true;
            this.PARTY_B58.Location = new System.Drawing.Point(693, 260);
            this.PARTY_B58.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B58.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B58.Name = "PARTY_B58";
            this.PARTY_B58.ReadOnly = true;
            this.PARTY_B58.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B58.TabIndex = 401;
            // 
            // PARTY_J_58
            // 
            this.PARTY_J_58.Location = new System.Drawing.Point(629, 262);
            this.PARTY_J_58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PARTY_J_58.Name = "PARTY_J_58";
            this.PARTY_J_58.Size = new System.Drawing.Size(57, 18);
            this.PARTY_J_58.TabIndex = 400;
            this.PARTY_J_58.Text = "??? 2";
            this.PARTY_J_58.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B57
            // 
            this.PARTY_B57.Hexadecimal = true;
            this.PARTY_B57.Location = new System.Drawing.Point(353, 606);
            this.PARTY_B57.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B57.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B57.Name = "PARTY_B57";
            this.PARTY_B57.ReadOnly = true;
            this.PARTY_B57.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B57.TabIndex = 399;
            // 
            // label70
            // 
            this.label70.Location = new System.Drawing.Point(227, 608);
            this.label70.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(122, 18);
            this.label70.TabIndex = 398;
            this.label70.Text = "支援フラグ";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B56
            // 
            this.PARTY_B56.Location = new System.Drawing.Point(353, 577);
            this.PARTY_B56.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B56.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B56.Name = "PARTY_B56";
            this.PARTY_B56.ReadOnly = true;
            this.PARTY_B56.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B56.TabIndex = 397;
            // 
            // label71
            // 
            this.label71.Location = new System.Drawing.Point(227, 579);
            this.label71.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(122, 18);
            this.label71.TabIndex = 396;
            this.label71.Text = "支援7";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B55
            // 
            this.PARTY_B55.Location = new System.Drawing.Point(353, 550);
            this.PARTY_B55.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B55.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B55.Name = "PARTY_B55";
            this.PARTY_B55.ReadOnly = true;
            this.PARTY_B55.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B55.TabIndex = 395;
            // 
            // label72
            // 
            this.label72.Location = new System.Drawing.Point(227, 552);
            this.label72.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(122, 18);
            this.label72.TabIndex = 394;
            this.label72.Text = "支援6";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B54
            // 
            this.PARTY_B54.Location = new System.Drawing.Point(353, 521);
            this.PARTY_B54.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B54.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B54.Name = "PARTY_B54";
            this.PARTY_B54.ReadOnly = true;
            this.PARTY_B54.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B54.TabIndex = 393;
            // 
            // label73
            // 
            this.label73.Location = new System.Drawing.Point(227, 523);
            this.label73.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(122, 18);
            this.label73.TabIndex = 392;
            this.label73.Text = "支援5";
            this.label73.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B53
            // 
            this.PARTY_B53.Location = new System.Drawing.Point(353, 492);
            this.PARTY_B53.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B53.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B53.Name = "PARTY_B53";
            this.PARTY_B53.ReadOnly = true;
            this.PARTY_B53.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B53.TabIndex = 391;
            // 
            // label74
            // 
            this.label74.Location = new System.Drawing.Point(227, 494);
            this.label74.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(122, 18);
            this.label74.TabIndex = 390;
            this.label74.Text = "支援4";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B52
            // 
            this.PARTY_B52.Location = new System.Drawing.Point(353, 463);
            this.PARTY_B52.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B52.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B52.Name = "PARTY_B52";
            this.PARTY_B52.ReadOnly = true;
            this.PARTY_B52.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B52.TabIndex = 389;
            // 
            // label75
            // 
            this.label75.Location = new System.Drawing.Point(227, 465);
            this.label75.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(122, 18);
            this.label75.TabIndex = 388;
            this.label75.Text = "支援3";
            this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B51
            // 
            this.PARTY_B51.Location = new System.Drawing.Point(353, 436);
            this.PARTY_B51.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B51.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B51.Name = "PARTY_B51";
            this.PARTY_B51.ReadOnly = true;
            this.PARTY_B51.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B51.TabIndex = 387;
            // 
            // label76
            // 
            this.label76.Location = new System.Drawing.Point(227, 438);
            this.label76.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(122, 18);
            this.label76.TabIndex = 386;
            this.label76.Text = "支援2";
            this.label76.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B50
            // 
            this.PARTY_B50.Location = new System.Drawing.Point(353, 407);
            this.PARTY_B50.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B50.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B50.Name = "PARTY_B50";
            this.PARTY_B50.ReadOnly = true;
            this.PARTY_B50.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B50.TabIndex = 385;
            // 
            // label77
            // 
            this.label77.Location = new System.Drawing.Point(227, 409);
            this.label77.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(122, 18);
            this.label77.TabIndex = 384;
            this.label77.Text = "支援1";
            this.label77.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B49
            // 
            this.PARTY_B49.Hexadecimal = true;
            this.PARTY_B49.Location = new System.Drawing.Point(150, 664);
            this.PARTY_B49.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B49.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B49.Name = "PARTY_B49";
            this.PARTY_B49.ReadOnly = true;
            this.PARTY_B49.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B49.TabIndex = 383;
            // 
            // label67
            // 
            this.label67.Location = new System.Drawing.Point(8, 666);
            this.label67.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(139, 18);
            this.label67.TabIndex = 382;
            this.label67.Text = "聖水松明";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B48
            // 
            this.PARTY_B48.Hexadecimal = true;
            this.PARTY_B48.Location = new System.Drawing.Point(150, 635);
            this.PARTY_B48.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B48.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B48.Name = "PARTY_B48";
            this.PARTY_B48.ReadOnly = true;
            this.PARTY_B48.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B48.TabIndex = 381;
            // 
            // label68
            // 
            this.label68.Location = new System.Drawing.Point(8, 637);
            this.label68.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(139, 18);
            this.label68.TabIndex = 380;
            this.label68.Text = "状態とターン";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B47
            // 
            this.PARTY_B47.Location = new System.Drawing.Point(150, 606);
            this.PARTY_B47.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B47.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B47.Name = "PARTY_B47";
            this.PARTY_B47.ReadOnly = true;
            this.PARTY_B47.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B47.TabIndex = 379;
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(8, 608);
            this.label45.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(139, 18);
            this.label45.TabIndex = 378;
            this.label45.Text = "闇 EXP";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B46
            // 
            this.PARTY_B46.Location = new System.Drawing.Point(150, 577);
            this.PARTY_B46.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B46.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B46.Name = "PARTY_B46";
            this.PARTY_B46.ReadOnly = true;
            this.PARTY_B46.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B46.TabIndex = 377;
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(8, 579);
            this.label47.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(139, 18);
            this.label47.TabIndex = 376;
            this.label47.Text = "光 EXP";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B45
            // 
            this.PARTY_B45.Location = new System.Drawing.Point(150, 550);
            this.PARTY_B45.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B45.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B45.Name = "PARTY_B45";
            this.PARTY_B45.ReadOnly = true;
            this.PARTY_B45.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B45.TabIndex = 375;
            // 
            // label49
            // 
            this.label49.Location = new System.Drawing.Point(8, 552);
            this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(139, 18);
            this.label49.TabIndex = 374;
            this.label49.Text = "理 EXP";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B44
            // 
            this.PARTY_B44.Location = new System.Drawing.Point(150, 521);
            this.PARTY_B44.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B44.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B44.Name = "PARTY_B44";
            this.PARTY_B44.ReadOnly = true;
            this.PARTY_B44.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B44.TabIndex = 373;
            // 
            // label51
            // 
            this.label51.Location = new System.Drawing.Point(8, 523);
            this.label51.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(139, 18);
            this.label51.TabIndex = 372;
            this.label51.Text = "杖 EXP";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B43
            // 
            this.PARTY_B43.Location = new System.Drawing.Point(150, 492);
            this.PARTY_B43.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B43.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B43.Name = "PARTY_B43";
            this.PARTY_B43.ReadOnly = true;
            this.PARTY_B43.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B43.TabIndex = 371;
            // 
            // label42
            // 
            this.label42.Location = new System.Drawing.Point(8, 494);
            this.label42.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(139, 18);
            this.label42.TabIndex = 370;
            this.label42.Text = "弓 EXP";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B42
            // 
            this.PARTY_B42.Location = new System.Drawing.Point(150, 463);
            this.PARTY_B42.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B42.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B42.Name = "PARTY_B42";
            this.PARTY_B42.ReadOnly = true;
            this.PARTY_B42.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B42.TabIndex = 369;
            // 
            // label44
            // 
            this.label44.Location = new System.Drawing.Point(8, 465);
            this.label44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(139, 18);
            this.label44.TabIndex = 368;
            this.label44.Text = "斧 EXP";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B41
            // 
            this.PARTY_B41.Location = new System.Drawing.Point(150, 436);
            this.PARTY_B41.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B41.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B41.Name = "PARTY_B41";
            this.PARTY_B41.ReadOnly = true;
            this.PARTY_B41.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B41.TabIndex = 367;
            // 
            // label41
            // 
            this.label41.Location = new System.Drawing.Point(8, 438);
            this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(139, 18);
            this.label41.TabIndex = 366;
            this.label41.Text = "槍 EXP";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B40
            // 
            this.PARTY_B40.Location = new System.Drawing.Point(150, 407);
            this.PARTY_B40.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B40.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B40.Name = "PARTY_B40";
            this.PARTY_B40.ReadOnly = true;
            this.PARTY_B40.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B40.TabIndex = 365;
            // 
            // label38
            // 
            this.label38.Location = new System.Drawing.Point(8, 409);
            this.label38.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(139, 18);
            this.label38.TabIndex = 364;
            this.label38.Text = "剣 EXP";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_L_38_ITEM
            // 
            this.PARTY_L_38_ITEM.ErrorMessage = "";
            this.PARTY_L_38_ITEM.Location = new System.Drawing.Point(430, 376);
            this.PARTY_L_38_ITEM.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_38_ITEM.Name = "PARTY_L_38_ITEM";
            this.PARTY_L_38_ITEM.Placeholder = "";
            this.PARTY_L_38_ITEM.ReadOnly = true;
            this.PARTY_L_38_ITEM.Size = new System.Drawing.Size(155, 25);
            this.PARTY_L_38_ITEM.TabIndex = 363;
            // 
            // PARTY_B39
            // 
            this.PARTY_B39.Location = new System.Drawing.Point(353, 380);
            this.PARTY_B39.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B39.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B39.Name = "PARTY_B39";
            this.PARTY_B39.ReadOnly = true;
            this.PARTY_B39.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B39.TabIndex = 362;
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(232, 382);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(117, 18);
            this.label34.TabIndex = 361;
            this.label34.Text = "アイテム数5";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B38
            // 
            this.PARTY_B38.Hexadecimal = true;
            this.PARTY_B38.Location = new System.Drawing.Point(150, 378);
            this.PARTY_B38.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B38.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B38.Name = "PARTY_B38";
            this.PARTY_B38.ReadOnly = true;
            this.PARTY_B38.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B38.TabIndex = 360;
            // 
            // label37
            // 
            this.label37.Location = new System.Drawing.Point(8, 380);
            this.label37.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(139, 18);
            this.label37.TabIndex = 359;
            this.label37.Text = "アイテムID5";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_L_36_ITEM
            // 
            this.PARTY_L_36_ITEM.ErrorMessage = "";
            this.PARTY_L_36_ITEM.Location = new System.Drawing.Point(430, 348);
            this.PARTY_L_36_ITEM.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_36_ITEM.Name = "PARTY_L_36_ITEM";
            this.PARTY_L_36_ITEM.Placeholder = "";
            this.PARTY_L_36_ITEM.ReadOnly = true;
            this.PARTY_L_36_ITEM.Size = new System.Drawing.Size(155, 25);
            this.PARTY_L_36_ITEM.TabIndex = 358;
            // 
            // PARTY_B37
            // 
            this.PARTY_B37.Location = new System.Drawing.Point(353, 352);
            this.PARTY_B37.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B37.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B37.Name = "PARTY_B37";
            this.PARTY_B37.ReadOnly = true;
            this.PARTY_B37.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B37.TabIndex = 357;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(232, 354);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(117, 18);
            this.label24.TabIndex = 356;
            this.label24.Text = "アイテム数4";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B36
            // 
            this.PARTY_B36.Hexadecimal = true;
            this.PARTY_B36.Location = new System.Drawing.Point(150, 350);
            this.PARTY_B36.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B36.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B36.Name = "PARTY_B36";
            this.PARTY_B36.ReadOnly = true;
            this.PARTY_B36.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B36.TabIndex = 355;
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(8, 352);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(139, 18);
            this.label29.TabIndex = 354;
            this.label29.Text = "アイテムID4";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_L_34_ITEM
            // 
            this.PARTY_L_34_ITEM.ErrorMessage = "";
            this.PARTY_L_34_ITEM.Location = new System.Drawing.Point(430, 323);
            this.PARTY_L_34_ITEM.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_34_ITEM.Name = "PARTY_L_34_ITEM";
            this.PARTY_L_34_ITEM.Placeholder = "";
            this.PARTY_L_34_ITEM.ReadOnly = true;
            this.PARTY_L_34_ITEM.Size = new System.Drawing.Size(155, 25);
            this.PARTY_L_34_ITEM.TabIndex = 353;
            // 
            // PARTY_B35
            // 
            this.PARTY_B35.Location = new System.Drawing.Point(353, 324);
            this.PARTY_B35.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B35.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B35.Name = "PARTY_B35";
            this.PARTY_B35.ReadOnly = true;
            this.PARTY_B35.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B35.TabIndex = 352;
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(232, 326);
            this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(117, 18);
            this.label31.TabIndex = 351;
            this.label31.Text = "アイテム数3";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B34
            // 
            this.PARTY_B34.Hexadecimal = true;
            this.PARTY_B34.Location = new System.Drawing.Point(150, 322);
            this.PARTY_B34.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B34.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B34.Name = "PARTY_B34";
            this.PARTY_B34.ReadOnly = true;
            this.PARTY_B34.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B34.TabIndex = 350;
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(8, 324);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(139, 18);
            this.label32.TabIndex = 349;
            this.label32.Text = "アイテムID3";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_L_32_ITEM
            // 
            this.PARTY_L_32_ITEM.ErrorMessage = "";
            this.PARTY_L_32_ITEM.Location = new System.Drawing.Point(430, 294);
            this.PARTY_L_32_ITEM.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_32_ITEM.Name = "PARTY_L_32_ITEM";
            this.PARTY_L_32_ITEM.Placeholder = "";
            this.PARTY_L_32_ITEM.ReadOnly = true;
            this.PARTY_L_32_ITEM.Size = new System.Drawing.Size(155, 25);
            this.PARTY_L_32_ITEM.TabIndex = 333;
            // 
            // PARTY_B33
            // 
            this.PARTY_B33.Location = new System.Drawing.Point(353, 297);
            this.PARTY_B33.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B33.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B33.Name = "PARTY_B33";
            this.PARTY_B33.ReadOnly = true;
            this.PARTY_B33.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B33.TabIndex = 332;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(232, 299);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(117, 18);
            this.label20.TabIndex = 331;
            this.label20.Text = "アイテム数2";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B32
            // 
            this.PARTY_B32.Hexadecimal = true;
            this.PARTY_B32.Location = new System.Drawing.Point(150, 295);
            this.PARTY_B32.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B32.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B32.Name = "PARTY_B32";
            this.PARTY_B32.ReadOnly = true;
            this.PARTY_B32.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B32.TabIndex = 330;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(8, 297);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(139, 18);
            this.label23.TabIndex = 329;
            this.label23.Text = "アイテムID2";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_L_30_ITEM
            // 
            this.PARTY_L_30_ITEM.ErrorMessage = "";
            this.PARTY_L_30_ITEM.Location = new System.Drawing.Point(430, 268);
            this.PARTY_L_30_ITEM.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_L_30_ITEM.Name = "PARTY_L_30_ITEM";
            this.PARTY_L_30_ITEM.Placeholder = "";
            this.PARTY_L_30_ITEM.ReadOnly = true;
            this.PARTY_L_30_ITEM.Size = new System.Drawing.Size(155, 25);
            this.PARTY_L_30_ITEM.TabIndex = 328;
            // 
            // PARTY_B31
            // 
            this.PARTY_B31.Location = new System.Drawing.Point(353, 269);
            this.PARTY_B31.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B31.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B31.Name = "PARTY_B31";
            this.PARTY_B31.ReadOnly = true;
            this.PARTY_B31.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B31.TabIndex = 327;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(232, 271);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(117, 18);
            this.label12.TabIndex = 326;
            this.label12.Text = "アイテム数1";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B30
            // 
            this.PARTY_B30.Hexadecimal = true;
            this.PARTY_B30.Location = new System.Drawing.Point(150, 267);
            this.PARTY_B30.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B30.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B30.Name = "PARTY_B30";
            this.PARTY_B30.ReadOnly = true;
            this.PARTY_B30.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B30.TabIndex = 325;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 269);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(139, 18);
            this.label9.TabIndex = 324;
            this.label9.Text = "アイテムID1";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B28
            // 
            this.PARTY_B28.Location = new System.Drawing.Point(689, 181);
            this.PARTY_B28.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B28.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B28.Name = "PARTY_B28";
            this.PARTY_B28.ReadOnly = true;
            this.PARTY_B28.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B28.TabIndex = 322;
            // 
            // label66
            // 
            this.label66.Location = new System.Drawing.Point(580, 184);
            this.label66.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(106, 18);
            this.label66.TabIndex = 323;
            this.label66.Text = "??? 1";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B29
            // 
            this.PARTY_B29.Location = new System.Drawing.Point(689, 208);
            this.PARTY_B29.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B29.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B29.Name = "PARTY_B29";
            this.PARTY_B29.ReadOnly = true;
            this.PARTY_B29.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B29.TabIndex = 320;
            // 
            // label65
            // 
            this.label65.Location = new System.Drawing.Point(560, 211);
            this.label65.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(126, 18);
            this.label65.TabIndex = 321;
            this.label65.Text = "移動＋";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_RAMUNITAID
            // 
            this.PARTY_RAMUNITAID.ErrorMessage = "";
            this.PARTY_RAMUNITAID.Location = new System.Drawing.Point(605, 152);
            this.PARTY_RAMUNITAID.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_RAMUNITAID.Name = "PARTY_RAMUNITAID";
            this.PARTY_RAMUNITAID.Placeholder = "";
            this.PARTY_RAMUNITAID.ReadOnly = true;
            this.PARTY_RAMUNITAID.Size = new System.Drawing.Size(157, 25);
            this.PARTY_RAMUNITAID.TabIndex = 319;
            this.PARTY_RAMUNITAID.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PARTY_RAMUNITAID_MouseDoubleClick);
            // 
            // PARTY_B27
            // 
            this.PARTY_B27.Hexadecimal = true;
            this.PARTY_B27.Location = new System.Drawing.Point(689, 123);
            this.PARTY_B27.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B27.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B27.Name = "PARTY_B27";
            this.PARTY_B27.ReadOnly = true;
            this.PARTY_B27.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B27.TabIndex = 317;
            // 
            // label64
            // 
            this.label64.Location = new System.Drawing.Point(559, 126);
            this.label64.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(126, 18);
            this.label64.TabIndex = 318;
            this.label64.Text = "同行者ID";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B26
            // 
            this.PARTY_B26.Location = new System.Drawing.Point(689, 94);
            this.PARTY_B26.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B26.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B26.Name = "PARTY_B26";
            this.PARTY_B26.ReadOnly = true;
            this.PARTY_B26.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B26.TabIndex = 315;
            // 
            // PARTY_J_26
            // 
            this.PARTY_J_26.Location = new System.Drawing.Point(559, 97);
            this.PARTY_J_26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PARTY_J_26.Name = "PARTY_J_26";
            this.PARTY_J_26.Size = new System.Drawing.Size(126, 18);
            this.PARTY_J_26.TabIndex = 316;
            this.PARTY_J_26.Text = "体格＋";
            this.PARTY_J_26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B25
            // 
            this.PARTY_B25.Location = new System.Drawing.Point(483, 235);
            this.PARTY_B25.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B25.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B25.Name = "PARTY_B25";
            this.PARTY_B25.ReadOnly = true;
            this.PARTY_B25.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B25.TabIndex = 313;
            // 
            // label63
            // 
            this.label63.Location = new System.Drawing.Point(432, 238);
            this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(47, 18);
            this.label63.TabIndex = 314;
            this.label63.Text = "運";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B24
            // 
            this.PARTY_B24.Location = new System.Drawing.Point(483, 207);
            this.PARTY_B24.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B24.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B24.Name = "PARTY_B24";
            this.PARTY_B24.ReadOnly = true;
            this.PARTY_B24.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B24.TabIndex = 311;
            // 
            // label60
            // 
            this.label60.Location = new System.Drawing.Point(418, 209);
            this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(61, 18);
            this.label60.TabIndex = 312;
            this.label60.Text = "魔防";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B23
            // 
            this.PARTY_B23.Location = new System.Drawing.Point(483, 178);
            this.PARTY_B23.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B23.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B23.Name = "PARTY_B23";
            this.PARTY_B23.ReadOnly = true;
            this.PARTY_B23.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B23.TabIndex = 309;
            // 
            // label61
            // 
            this.label61.Location = new System.Drawing.Point(380, 181);
            this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(99, 18);
            this.label61.TabIndex = 310;
            this.label61.Text = "守備";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B22
            // 
            this.PARTY_B22.Location = new System.Drawing.Point(483, 149);
            this.PARTY_B22.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B22.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B22.Name = "PARTY_B22";
            this.PARTY_B22.ReadOnly = true;
            this.PARTY_B22.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B22.TabIndex = 307;
            // 
            // label59
            // 
            this.label59.Location = new System.Drawing.Point(380, 152);
            this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(99, 18);
            this.label59.TabIndex = 308;
            this.label59.Text = "速さ";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B21
            // 
            this.PARTY_B21.Location = new System.Drawing.Point(483, 120);
            this.PARTY_B21.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B21.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B21.Name = "PARTY_B21";
            this.PARTY_B21.ReadOnly = true;
            this.PARTY_B21.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B21.TabIndex = 304;
            // 
            // label57
            // 
            this.label57.Location = new System.Drawing.Point(386, 123);
            this.label57.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(93, 18);
            this.label57.TabIndex = 306;
            this.label57.Text = "技";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_RAMUNITSTATE
            // 
            this.PARTY_RAMUNITSTATE.ErrorMessage = "";
            this.PARTY_RAMUNITSTATE.Location = new System.Drawing.Point(280, 65);
            this.PARTY_RAMUNITSTATE.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_RAMUNITSTATE.Name = "PARTY_RAMUNITSTATE";
            this.PARTY_RAMUNITSTATE.Placeholder = "";
            this.PARTY_RAMUNITSTATE.ReadOnly = true;
            this.PARTY_RAMUNITSTATE.Size = new System.Drawing.Size(482, 25);
            this.PARTY_RAMUNITSTATE.TabIndex = 302;
            // 
            // PARTY_ROMCLASSPOINTER
            // 
            this.PARTY_ROMCLASSPOINTER.ErrorMessage = "";
            this.PARTY_ROMCLASSPOINTER.Location = new System.Drawing.Point(280, 34);
            this.PARTY_ROMCLASSPOINTER.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_ROMCLASSPOINTER.Name = "PARTY_ROMCLASSPOINTER";
            this.PARTY_ROMCLASSPOINTER.Placeholder = "";
            this.PARTY_ROMCLASSPOINTER.ReadOnly = true;
            this.PARTY_ROMCLASSPOINTER.Size = new System.Drawing.Size(481, 25);
            this.PARTY_ROMCLASSPOINTER.TabIndex = 300;
            this.PARTY_ROMCLASSPOINTER.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PARTY_ROMCLASSPOINTER_MouseDoubleClick);
            // 
            // PARTY_B10
            // 
            this.PARTY_B10.Hexadecimal = true;
            this.PARTY_B10.Location = new System.Drawing.Point(152, 149);
            this.PARTY_B10.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B10.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B10.Name = "PARTY_B10";
            this.PARTY_B10.ReadOnly = true;
            this.PARTY_B10.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B10.TabIndex = 4;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(11, 151);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(136, 18);
            this.label21.TabIndex = 294;
            this.label21.Text = "回復モード";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEx52
            // 
            this.textBoxEx52.ErrorMessage = "";
            this.textBoxEx52.Location = new System.Drawing.Point(329, 830);
            this.textBoxEx52.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx52.Name = "textBoxEx52";
            this.textBoxEx52.Placeholder = "";
            this.textBoxEx52.ReadOnly = true;
            this.textBoxEx52.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx52.TabIndex = 289;
            // 
            // numericUpDown59
            // 
            this.numericUpDown59.Hexadecimal = true;
            this.numericUpDown59.Location = new System.Drawing.Point(176, 830);
            this.numericUpDown59.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown59.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.numericUpDown59.Name = "numericUpDown59";
            this.numericUpDown59.ReadOnly = true;
            this.numericUpDown59.Size = new System.Drawing.Size(149, 25);
            this.numericUpDown59.TabIndex = 287;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(2, 833);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(170, 18);
            this.label28.TabIndex = 288;
            this.label28.Text = "UserSpace3";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEx53
            // 
            this.textBoxEx53.ErrorMessage = "";
            this.textBoxEx53.Location = new System.Drawing.Point(329, 802);
            this.textBoxEx53.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx53.Name = "textBoxEx53";
            this.textBoxEx53.Placeholder = "";
            this.textBoxEx53.ReadOnly = true;
            this.textBoxEx53.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx53.TabIndex = 286;
            // 
            // numericUpDown61
            // 
            this.numericUpDown61.Hexadecimal = true;
            this.numericUpDown61.Location = new System.Drawing.Point(176, 802);
            this.numericUpDown61.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown61.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.numericUpDown61.Name = "numericUpDown61";
            this.numericUpDown61.ReadOnly = true;
            this.numericUpDown61.Size = new System.Drawing.Size(149, 25);
            this.numericUpDown61.TabIndex = 284;
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(2, 805);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(170, 18);
            this.label30.TabIndex = 285;
            this.label30.Text = "UserSpace2";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEx54
            // 
            this.textBoxEx54.ErrorMessage = "";
            this.textBoxEx54.Location = new System.Drawing.Point(329, 773);
            this.textBoxEx54.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx54.Name = "textBoxEx54";
            this.textBoxEx54.Placeholder = "";
            this.textBoxEx54.ReadOnly = true;
            this.textBoxEx54.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx54.TabIndex = 283;
            // 
            // numericUpDown64
            // 
            this.numericUpDown64.Hexadecimal = true;
            this.numericUpDown64.Location = new System.Drawing.Point(176, 773);
            this.numericUpDown64.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown64.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.numericUpDown64.Name = "numericUpDown64";
            this.numericUpDown64.ReadOnly = true;
            this.numericUpDown64.Size = new System.Drawing.Size(149, 25);
            this.numericUpDown64.TabIndex = 281;
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(2, 776);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(170, 18);
            this.label33.TabIndex = 282;
            this.label33.Text = "UserSpace1";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B19
            // 
            this.PARTY_B19.Location = new System.Drawing.Point(152, 235);
            this.PARTY_B19.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B19.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B19.Name = "PARTY_B19";
            this.PARTY_B19.ReadOnly = true;
            this.PARTY_B19.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B19.TabIndex = 12;
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(24, 239);
            this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(123, 18);
            this.label36.TabIndex = 231;
            this.label36.Text = "現在HP";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B20
            // 
            this.PARTY_B20.Location = new System.Drawing.Point(483, 94);
            this.PARTY_B20.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B20.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B20.Name = "PARTY_B20";
            this.PARTY_B20.ReadOnly = true;
            this.PARTY_B20.Size = new System.Drawing.Size(73, 25);
            this.PARTY_B20.TabIndex = 11;
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(371, 96);
            this.label39.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(108, 18);
            this.label39.TabIndex = 228;
            this.label39.Text = "力と魔力";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B18
            // 
            this.PARTY_B18.Location = new System.Drawing.Point(152, 207);
            this.PARTY_B18.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B18.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B18.Name = "PARTY_B18";
            this.PARTY_B18.ReadOnly = true;
            this.PARTY_B18.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B18.TabIndex = 9;
            // 
            // label40
            // 
            this.label40.Location = new System.Drawing.Point(14, 210);
            this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(133, 18);
            this.label40.TabIndex = 225;
            this.label40.Text = "最大HP";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B17
            // 
            this.PARTY_B17.Location = new System.Drawing.Point(283, 124);
            this.PARTY_B17.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B17.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B17.Name = "PARTY_B17";
            this.PARTY_B17.ReadOnly = true;
            this.PARTY_B17.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B17.TabIndex = 8;
            // 
            // label43
            // 
            this.label43.Location = new System.Drawing.Point(254, 126);
            this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(25, 18);
            this.label43.TabIndex = 222;
            this.label43.Text = "Y";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B16
            // 
            this.PARTY_B16.Location = new System.Drawing.Point(283, 95);
            this.PARTY_B16.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B16.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B16.Name = "PARTY_B16";
            this.PARTY_B16.ReadOnly = true;
            this.PARTY_B16.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B16.TabIndex = 7;
            // 
            // label46
            // 
            this.label46.Location = new System.Drawing.Point(254, 98);
            this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(25, 18);
            this.label46.TabIndex = 219;
            this.label46.Text = "X";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_D12
            // 
            this.PARTY_D12.Hexadecimal = true;
            this.PARTY_D12.Location = new System.Drawing.Point(151, 63);
            this.PARTY_D12.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_D12.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_D12.Name = "PARTY_D12";
            this.PARTY_D12.ReadOnly = true;
            this.PARTY_D12.Size = new System.Drawing.Size(125, 25);
            this.PARTY_D12.TabIndex = 6;
            // 
            // label48
            // 
            this.label48.Location = new System.Drawing.Point(24, 67);
            this.label48.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(123, 18);
            this.label48.TabIndex = 216;
            this.label48.Text = "状態";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B11
            // 
            this.PARTY_B11.Hexadecimal = true;
            this.PARTY_B11.Location = new System.Drawing.Point(152, 178);
            this.PARTY_B11.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B11.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B11.Name = "PARTY_B11";
            this.PARTY_B11.ReadOnly = true;
            this.PARTY_B11.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B11.TabIndex = 5;
            // 
            // label50
            // 
            this.label50.Location = new System.Drawing.Point(14, 181);
            this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(133, 18);
            this.label50.TabIndex = 213;
            this.label50.Text = "部隊表ID";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B9
            // 
            this.PARTY_B9.Location = new System.Drawing.Point(152, 120);
            this.PARTY_B9.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B9.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B9.Name = "PARTY_B9";
            this.PARTY_B9.ReadOnly = true;
            this.PARTY_B9.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B9.TabIndex = 3;
            // 
            // label52
            // 
            this.label52.Location = new System.Drawing.Point(67, 123);
            this.label52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(80, 18);
            this.label52.TabIndex = 210;
            this.label52.Text = "Exp";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_B8
            // 
            this.PARTY_B8.Location = new System.Drawing.Point(152, 92);
            this.PARTY_B8.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_B8.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_B8.Name = "PARTY_B8";
            this.PARTY_B8.ReadOnly = true;
            this.PARTY_B8.Size = new System.Drawing.Size(71, 25);
            this.PARTY_B8.TabIndex = 2;
            // 
            // label53
            // 
            this.label53.Location = new System.Drawing.Point(64, 96);
            this.label53.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(83, 18);
            this.label53.TabIndex = 207;
            this.label53.Text = "Lv";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_P4
            // 
            this.PARTY_P4.Hexadecimal = true;
            this.PARTY_P4.Location = new System.Drawing.Point(151, 34);
            this.PARTY_P4.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_P4.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_P4.Name = "PARTY_P4";
            this.PARTY_P4.ReadOnly = true;
            this.PARTY_P4.Size = new System.Drawing.Size(125, 25);
            this.PARTY_P4.TabIndex = 1;
            // 
            // label54
            // 
            this.label54.Location = new System.Drawing.Point(24, 37);
            this.label54.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(123, 18);
            this.label54.TabIndex = 204;
            this.label54.Text = "ROM Class";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PARTY_ROMUNITPOINTER
            // 
            this.PARTY_ROMUNITPOINTER.ErrorMessage = "";
            this.PARTY_ROMUNITPOINTER.Location = new System.Drawing.Point(280, 5);
            this.PARTY_ROMUNITPOINTER.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_ROMUNITPOINTER.Name = "PARTY_ROMUNITPOINTER";
            this.PARTY_ROMUNITPOINTER.Placeholder = "";
            this.PARTY_ROMUNITPOINTER.ReadOnly = true;
            this.PARTY_ROMUNITPOINTER.Size = new System.Drawing.Size(481, 25);
            this.PARTY_ROMUNITPOINTER.TabIndex = 202;
            this.PARTY_ROMUNITPOINTER.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PARTY_ROMUNITPOINTER_MouseDoubleClick);
            // 
            // PARTY_P0
            // 
            this.PARTY_P0.Hexadecimal = true;
            this.PARTY_P0.Location = new System.Drawing.Point(151, 6);
            this.PARTY_P0.Margin = new System.Windows.Forms.Padding(2);
            this.PARTY_P0.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.PARTY_P0.Name = "PARTY_P0";
            this.PARTY_P0.ReadOnly = true;
            this.PARTY_P0.Size = new System.Drawing.Size(125, 25);
            this.PARTY_P0.TabIndex = 0;
            // 
            // label55
            // 
            this.label55.Location = new System.Drawing.Point(24, 8);
            this.label55.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(123, 18);
            this.label55.TabIndex = 1;
            this.label55.Text = "ROM Unit";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(842, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 32);
            this.button1.TabIndex = 201;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // PartyListBox
            // 
            this.PartyListBox.FormattingEnabled = true;
            this.PartyListBox.HorizontalScrollbar = true;
            this.PartyListBox.IntegralHeight = false;
            this.PartyListBox.ItemHeight = 18;
            this.PartyListBox.Location = new System.Drawing.Point(0, 49);
            this.PartyListBox.Margin = new System.Windows.Forms.Padding(2);
            this.PartyListBox.Name = "PartyListBox";
            this.PartyListBox.Size = new System.Drawing.Size(778, 832);
            this.PartyListBox.TabIndex = 311;
            this.PartyListBox.SelectedIndexChanged += new System.EventHandler(this.PartyListBox_SelectedIndexChanged);
            this.PartyListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PartyListBox_KeyDown);
            this.PartyListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PartyListBox_MouseDoubleClick);
            // 
            // label56
            // 
            this.label56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label56.Location = new System.Drawing.Point(1, 8);
            this.label56.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(778, 42);
            this.label56.TabIndex = 310;
            this.label56.Text = "パーティー";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CheatPage
            // 
            this.CheatPage.BackColor = System.Drawing.SystemColors.Control;
            this.CheatPage.Controls.Add(this.label94);
            this.CheatPage.Controls.Add(this.CHEAT_TURN_VALUE);
            this.CheatPage.Controls.Add(this.CHEAT_TURN);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_NODE_VALUE);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_NODE_JUMP);
            this.CheatPage.Controls.Add(this.CHEAT_ALL_ENEMY_DO_NOT_MOVE);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_EDTION_VALUE);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_EDTION_JUMP);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_CHPATER_VALUE);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_CHPATER_JUMP);
            this.CheatPage.Controls.Add(this.CHEAT_WARP);
            this.CheatPage.Controls.Add(this.CHEAT_ALL_ENEMY_UNIT_HP_1);
            this.CheatPage.Controls.Add(this.CHEAT_ALL_UNIT_GROW);
            this.CheatPage.Controls.Add(this.CHEAT_ALL_PLAYER_UNIT_GROW);
            this.CheatPage.Controls.Add(this.label35);
            this.CheatPage.Controls.Add(this.CHEAT_WEATHER_VALUE);
            this.CheatPage.Controls.Add(this.CHEAT_WEATHER_COMBO);
            this.CheatPage.Controls.Add(this.CHEAT_WEATHER);
            this.CheatPage.Controls.Add(this.label19);
            this.CheatPage.Controls.Add(this.label18);
            this.CheatPage.Controls.Add(this.CHEAT_FOG_VALUE);
            this.CheatPage.Controls.Add(this.CHEAT_ITEM_COUNT);
            this.CheatPage.Controls.Add(this.label17);
            this.CheatPage.Controls.Add(this.CHEAT_ITEM_ID);
            this.CheatPage.Controls.Add(this.CHEAT_MONEY_VALUE);
            this.CheatPage.Controls.Add(this.label16);
            this.CheatPage.Controls.Add(this.Dump03Button);
            this.CheatPage.Controls.Add(this.Dump02Button);
            this.CheatPage.Controls.Add(this.label15);
            this.CheatPage.Controls.Add(this.label14);
            this.CheatPage.Controls.Add(this.label13);
            this.CheatPage.Controls.Add(this.CHEAT_FOG);
            this.CheatPage.Controls.Add(this.CHEAT_MONEY);
            this.CheatPage.Controls.Add(this.CHEAT_ITEM_JUMP);
            this.CheatPage.Controls.Add(this.CHEAT_UNIT_HAVE_ITEM);
            this.CheatPage.Controls.Add(this.CHEAT_UNIT_HP_1);
            this.CheatPage.Controls.Add(this.label11);
            this.CheatPage.Controls.Add(this.CHEAT_UNIT_GROW);
            this.CheatPage.Controls.Add(this.CHEAT_SET_FLAG03);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_NODE_NAME);
            this.CheatPage.Controls.Add(this.systemIconPictureBox5);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_EDTION_NAME);
            this.CheatPage.Controls.Add(this.CHEAT_WARP_CHPATER_NAME);
            this.CheatPage.Controls.Add(this.systemIconPictureBox4);
            this.CheatPage.Controls.Add(this.CHEAT_UNIT_MEMORY_AND_NAME);
            this.CheatPage.Controls.Add(this.CHEAT_ITEM_NAME);
            this.CheatPage.Controls.Add(this.systemIconPictureBox3);
            this.CheatPage.Controls.Add(this.systemIconPictureBox2);
            this.CheatPage.Controls.Add(this.CHEAT_UNIT_MEMORY_AND_ICON);
            this.CheatPage.Controls.Add(this.systemIconPictureBox1);
            this.CheatPage.Controls.Add(this.CHEAT_ITEM_ICON);
            this.CheatPage.Location = new System.Drawing.Point(4, 28);
            this.CheatPage.Margin = new System.Windows.Forms.Padding(2);
            this.CheatPage.Name = "CheatPage";
            this.CheatPage.Padding = new System.Windows.Forms.Padding(2);
            this.CheatPage.Size = new System.Drawing.Size(1646, 888);
            this.CheatPage.TabIndex = 3;
            this.CheatPage.Text = "チート";
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Location = new System.Drawing.Point(889, 369);
            this.label94.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(66, 18);
            this.label94.TabIndex = 332;
            this.label94.Text = "ターン数";
            // 
            // CHEAT_TURN_VALUE
            // 
            this.CHEAT_TURN_VALUE.Location = new System.Drawing.Point(1041, 362);
            this.CHEAT_TURN_VALUE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_TURN_VALUE.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CHEAT_TURN_VALUE.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CHEAT_TURN_VALUE.Name = "CHEAT_TURN_VALUE";
            this.CHEAT_TURN_VALUE.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_TURN_VALUE.TabIndex = 331;
            this.CHEAT_TURN_VALUE.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CHEAT_TURN
            // 
            this.CHEAT_TURN.Location = new System.Drawing.Point(887, 312);
            this.CHEAT_TURN.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_TURN.Name = "CHEAT_TURN";
            this.CHEAT_TURN.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_TURN.TabIndex = 330;
            this.CHEAT_TURN.Text = "ターン数を変更";
            this.CHEAT_TURN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_TURN.UseVisualStyleBackColor = true;
            this.CHEAT_TURN.Click += new System.EventHandler(this.CHEAT_TURN_Click);
            // 
            // CHEAT_WARP_NODE_VALUE
            // 
            this.CHEAT_WARP_NODE_VALUE.Hexadecimal = true;
            this.CHEAT_WARP_NODE_VALUE.Location = new System.Drawing.Point(1041, 188);
            this.CHEAT_WARP_NODE_VALUE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_WARP_NODE_VALUE.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CHEAT_WARP_NODE_VALUE.Name = "CHEAT_WARP_NODE_VALUE";
            this.CHEAT_WARP_NODE_VALUE.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_WARP_NODE_VALUE.TabIndex = 327;
            // 
            // CHEAT_WARP_NODE_JUMP
            // 
            this.CHEAT_WARP_NODE_JUMP.AutoSize = true;
            this.CHEAT_WARP_NODE_JUMP.Location = new System.Drawing.Point(889, 190);
            this.CHEAT_WARP_NODE_JUMP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CHEAT_WARP_NODE_JUMP.Name = "CHEAT_WARP_NODE_JUMP";
            this.CHEAT_WARP_NODE_JUMP.Size = new System.Drawing.Size(139, 18);
            this.CHEAT_WARP_NODE_JUMP.TabIndex = 328;
            this.CHEAT_WARP_NODE_JUMP.Text = "ワールドマップ拠点";
            // 
            // CHEAT_ALL_ENEMY_DO_NOT_MOVE
            // 
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.Location = new System.Drawing.Point(67, 676);
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.Name = "CHEAT_ALL_ENEMY_DO_NOT_MOVE";
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.TabIndex = 325;
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.Text = "すべての敵ユニットAIを「移動しない」に設定にします。";
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.UseVisualStyleBackColor = true;
            this.CHEAT_ALL_ENEMY_DO_NOT_MOVE.Click += new System.EventHandler(this.CHEAT_ALL_ENEMY_DO_NOT_MOVE_Click);
            // 
            // CHEAT_WARP_EDTION_VALUE
            // 
            this.CHEAT_WARP_EDTION_VALUE.Hexadecimal = true;
            this.CHEAT_WARP_EDTION_VALUE.Location = new System.Drawing.Point(1041, 155);
            this.CHEAT_WARP_EDTION_VALUE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_WARP_EDTION_VALUE.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CHEAT_WARP_EDTION_VALUE.Name = "CHEAT_WARP_EDTION_VALUE";
            this.CHEAT_WARP_EDTION_VALUE.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_WARP_EDTION_VALUE.TabIndex = 322;
            // 
            // CHEAT_WARP_EDTION_JUMP
            // 
            this.CHEAT_WARP_EDTION_JUMP.AutoSize = true;
            this.CHEAT_WARP_EDTION_JUMP.Location = new System.Drawing.Point(889, 157);
            this.CHEAT_WARP_EDTION_JUMP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CHEAT_WARP_EDTION_JUMP.Name = "CHEAT_WARP_EDTION_JUMP";
            this.CHEAT_WARP_EDTION_JUMP.Size = new System.Drawing.Size(26, 18);
            this.CHEAT_WARP_EDTION_JUMP.TabIndex = 323;
            this.CHEAT_WARP_EDTION_JUMP.Text = "編";
            // 
            // CHEAT_WARP_CHPATER_VALUE
            // 
            this.CHEAT_WARP_CHPATER_VALUE.Hexadecimal = true;
            this.CHEAT_WARP_CHPATER_VALUE.Location = new System.Drawing.Point(1041, 122);
            this.CHEAT_WARP_CHPATER_VALUE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_WARP_CHPATER_VALUE.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CHEAT_WARP_CHPATER_VALUE.Name = "CHEAT_WARP_CHPATER_VALUE";
            this.CHEAT_WARP_CHPATER_VALUE.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_WARP_CHPATER_VALUE.TabIndex = 319;
            this.CHEAT_WARP_CHPATER_VALUE.ValueChanged += new System.EventHandler(this.CHEAT_WARP_CHPATER_VALUE_ValueChanged);
            // 
            // CHEAT_WARP_CHPATER_JUMP
            // 
            this.CHEAT_WARP_CHPATER_JUMP.AutoSize = true;
            this.CHEAT_WARP_CHPATER_JUMP.Location = new System.Drawing.Point(889, 124);
            this.CHEAT_WARP_CHPATER_JUMP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CHEAT_WARP_CHPATER_JUMP.Name = "CHEAT_WARP_CHPATER_JUMP";
            this.CHEAT_WARP_CHPATER_JUMP.Size = new System.Drawing.Size(98, 18);
            this.CHEAT_WARP_CHPATER_JUMP.TabIndex = 320;
            this.CHEAT_WARP_CHPATER_JUMP.Text = "ワープする章";
            // 
            // CHEAT_WARP
            // 
            this.CHEAT_WARP.Location = new System.Drawing.Point(887, 56);
            this.CHEAT_WARP.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_WARP.Name = "CHEAT_WARP";
            this.CHEAT_WARP.Size = new System.Drawing.Size(742, 53);
            this.CHEAT_WARP.TabIndex = 318;
            this.CHEAT_WARP.Text = "この章へワープする";
            this.CHEAT_WARP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_WARP.UseVisualStyleBackColor = true;
            this.CHEAT_WARP.Click += new System.EventHandler(this.CHEAT_WARP_Click);
            // 
            // CHEAT_ALL_ENEMY_UNIT_HP_1
            // 
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.Location = new System.Drawing.Point(66, 623);
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.Name = "CHEAT_ALL_ENEMY_UNIT_HP_1";
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.TabIndex = 317;
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.Text = "すべての敵ユニットのHPを1にします。";
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.UseVisualStyleBackColor = true;
            this.CHEAT_ALL_ENEMY_UNIT_HP_1.Click += new System.EventHandler(this.CHEAT_ALL_ENEMY_UNIT_HP_1_Click);
            // 
            // CHEAT_ALL_UNIT_GROW
            // 
            this.CHEAT_ALL_UNIT_GROW.Location = new System.Drawing.Point(66, 550);
            this.CHEAT_ALL_UNIT_GROW.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_ALL_UNIT_GROW.Name = "CHEAT_ALL_UNIT_GROW";
            this.CHEAT_ALL_UNIT_GROW.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_ALL_UNIT_GROW.TabIndex = 315;
            this.CHEAT_ALL_UNIT_GROW.Text = "敵を含めて、すべてのユニットを最強にします";
            this.CHEAT_ALL_UNIT_GROW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_ALL_UNIT_GROW.UseVisualStyleBackColor = true;
            this.CHEAT_ALL_UNIT_GROW.Click += new System.EventHandler(this.CHEAT_ALL_UNIT_GROW_Click);
            // 
            // CHEAT_ALL_PLAYER_UNIT_GROW
            // 
            this.CHEAT_ALL_PLAYER_UNIT_GROW.Location = new System.Drawing.Point(66, 498);
            this.CHEAT_ALL_PLAYER_UNIT_GROW.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_ALL_PLAYER_UNIT_GROW.Name = "CHEAT_ALL_PLAYER_UNIT_GROW";
            this.CHEAT_ALL_PLAYER_UNIT_GROW.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_ALL_PLAYER_UNIT_GROW.TabIndex = 313;
            this.CHEAT_ALL_PLAYER_UNIT_GROW.Text = "すべての味方ユニットを最強にします。(Hotkey Ctrl + G)";
            this.CHEAT_ALL_PLAYER_UNIT_GROW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_ALL_PLAYER_UNIT_GROW.UseVisualStyleBackColor = true;
            this.CHEAT_ALL_PLAYER_UNIT_GROW.Click += new System.EventHandler(this.CHEAT_ALL_PLAYER_UNIT_GROW_Click);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(889, 555);
            this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(44, 18);
            this.label35.TabIndex = 312;
            this.label35.Text = "天気";
            // 
            // CHEAT_WEATHER_VALUE
            // 
            this.CHEAT_WEATHER_VALUE.Hexadecimal = true;
            this.CHEAT_WEATHER_VALUE.Location = new System.Drawing.Point(1041, 548);
            this.CHEAT_WEATHER_VALUE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_WEATHER_VALUE.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CHEAT_WEATHER_VALUE.Name = "CHEAT_WEATHER_VALUE";
            this.CHEAT_WEATHER_VALUE.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_WEATHER_VALUE.TabIndex = 309;
            // 
            // CHEAT_WEATHER_COMBO
            // 
            this.CHEAT_WEATHER_COMBO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CHEAT_WEATHER_COMBO.FormattingEnabled = true;
            this.CHEAT_WEATHER_COMBO.Items.AddRange(new object[] {
            "00=晴れ",
            "01=雪",
            "02=吹雪",
            "04=雨",
            "05=マグマ",
            "06=砂嵐",
            "07=曇り(非推奨)"});
            this.CHEAT_WEATHER_COMBO.Location = new System.Drawing.Point(1123, 547);
            this.CHEAT_WEATHER_COMBO.Margin = new System.Windows.Forms.Padding(5);
            this.CHEAT_WEATHER_COMBO.Name = "CHEAT_WEATHER_COMBO";
            this.CHEAT_WEATHER_COMBO.Size = new System.Drawing.Size(271, 26);
            this.CHEAT_WEATHER_COMBO.TabIndex = 310;
            // 
            // CHEAT_WEATHER
            // 
            this.CHEAT_WEATHER.Location = new System.Drawing.Point(887, 498);
            this.CHEAT_WEATHER.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_WEATHER.Name = "CHEAT_WEATHER";
            this.CHEAT_WEATHER.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_WEATHER.TabIndex = 308;
            this.CHEAT_WEATHER.Text = "天気を変更(次のターンから適用されます。)";
            this.CHEAT_WEATHER.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_WEATHER.UseVisualStyleBackColor = true;
            this.CHEAT_WEATHER.Click += new System.EventHandler(this.CHEAT_WEATHER_Click);
            // 
            // label19
            // 
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Location = new System.Drawing.Point(6, 13);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(796, 30);
            this.label19.TabIndex = 307;
            this.label19.Text = "デバッグ用に便利なチート機能です";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(1120, 460);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(313, 18);
            this.label18.TabIndex = 229;
            this.label18.Text = "0で霧なし。1が視界1マスの最大の霧です。";
            // 
            // CHEAT_FOG_VALUE
            // 
            this.CHEAT_FOG_VALUE.Hexadecimal = true;
            this.CHEAT_FOG_VALUE.Location = new System.Drawing.Point(1038, 458);
            this.CHEAT_FOG_VALUE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_FOG_VALUE.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CHEAT_FOG_VALUE.Name = "CHEAT_FOG_VALUE";
            this.CHEAT_FOG_VALUE.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_FOG_VALUE.TabIndex = 9;
            // 
            // CHEAT_ITEM_COUNT
            // 
            this.CHEAT_ITEM_COUNT.Location = new System.Drawing.Point(213, 414);
            this.CHEAT_ITEM_COUNT.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_ITEM_COUNT.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.CHEAT_ITEM_COUNT.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CHEAT_ITEM_COUNT.Name = "CHEAT_ITEM_COUNT";
            this.CHEAT_ITEM_COUNT.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_ITEM_COUNT.TabIndex = 5;
            this.CHEAT_ITEM_COUNT.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(68, 409);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(44, 18);
            this.label17.TabIndex = 221;
            this.label17.Text = "個数";
            // 
            // CHEAT_ITEM_ID
            // 
            this.CHEAT_ITEM_ID.Hexadecimal = true;
            this.CHEAT_ITEM_ID.Location = new System.Drawing.Point(213, 378);
            this.CHEAT_ITEM_ID.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.CHEAT_ITEM_ID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CHEAT_ITEM_ID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CHEAT_ITEM_ID.Name = "CHEAT_ITEM_ID";
            this.CHEAT_ITEM_ID.Size = new System.Drawing.Size(65, 25);
            this.CHEAT_ITEM_ID.TabIndex = 4;
            this.CHEAT_ITEM_ID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CHEAT_MONEY_VALUE
            // 
            this.CHEAT_MONEY_VALUE.Location = new System.Drawing.Point(1041, 271);
            this.CHEAT_MONEY_VALUE.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_MONEY_VALUE.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.CHEAT_MONEY_VALUE.Name = "CHEAT_MONEY_VALUE";
            this.CHEAT_MONEY_VALUE.Size = new System.Drawing.Size(187, 25);
            this.CHEAT_MONEY_VALUE.TabIndex = 7;
            this.CHEAT_MONEY_VALUE.Value = new decimal(new int[] {
            838861,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(895, 758);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(146, 18);
            this.label16.TabIndex = 216;
            this.label16.Text = "解析者向けの機能";
            // 
            // Dump03Button
            // 
            this.Dump03Button.Location = new System.Drawing.Point(893, 832);
            this.Dump03Button.Margin = new System.Windows.Forms.Padding(2);
            this.Dump03Button.Name = "Dump03Button";
            this.Dump03Button.Size = new System.Drawing.Size(742, 40);
            this.Dump03Button.TabIndex = 12;
            this.Dump03Button.Text = "メモリをダンプしてファイルに書き込む 0x03000000";
            this.Dump03Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Dump03Button.UseVisualStyleBackColor = true;
            this.Dump03Button.Click += new System.EventHandler(this.Dump0x03000000Button_Click);
            // 
            // Dump02Button
            // 
            this.Dump02Button.Location = new System.Drawing.Point(893, 778);
            this.Dump02Button.Margin = new System.Windows.Forms.Padding(2);
            this.Dump02Button.Name = "Dump02Button";
            this.Dump02Button.Size = new System.Drawing.Size(742, 40);
            this.Dump02Button.TabIndex = 10;
            this.Dump02Button.Text = "メモリをダンプしてファイルに書き込む 0x02000000";
            this.Dump02Button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Dump02Button.UseVisualStyleBackColor = true;
            this.Dump02Button.Click += new System.EventHandler(this.Dump0x02000000Button_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(889, 460);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 18);
            this.label15.TabIndex = 213;
            this.label15.Text = "霧レベル";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(64, 124);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(738, 46);
            this.label14.TabIndex = 212;
            this.label14.Text = "クリアフラグ以外の、他のフラグを変更したい場合は、イベント画面から、変更したいフラグをダブルクリックしてください。";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(889, 273);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 18);
            this.label13.TabIndex = 210;
            this.label13.Text = "所持金";
            // 
            // CHEAT_FOG
            // 
            this.CHEAT_FOG.Location = new System.Drawing.Point(887, 409);
            this.CHEAT_FOG.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_FOG.Name = "CHEAT_FOG";
            this.CHEAT_FOG.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_FOG.TabIndex = 8;
            this.CHEAT_FOG.Text = "霧レベルを変更(次のターンから適用されます。)";
            this.CHEAT_FOG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_FOG.UseVisualStyleBackColor = true;
            this.CHEAT_FOG.Click += new System.EventHandler(this.CHEAT_FOG_Click);
            // 
            // CHEAT_MONEY
            // 
            this.CHEAT_MONEY.Location = new System.Drawing.Point(887, 220);
            this.CHEAT_MONEY.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_MONEY.Name = "CHEAT_MONEY";
            this.CHEAT_MONEY.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_MONEY.TabIndex = 6;
            this.CHEAT_MONEY.Text = "所持金を以下の値に変更する";
            this.CHEAT_MONEY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_MONEY.UseVisualStyleBackColor = true;
            this.CHEAT_MONEY.Click += new System.EventHandler(this.CHEAT_MONEY_Click);
            // 
            // CHEAT_ITEM_JUMP
            // 
            this.CHEAT_ITEM_JUMP.AutoSize = true;
            this.CHEAT_ITEM_JUMP.Location = new System.Drawing.Point(68, 380);
            this.CHEAT_ITEM_JUMP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CHEAT_ITEM_JUMP.Name = "CHEAT_ITEM_JUMP";
            this.CHEAT_ITEM_JUMP.Size = new System.Drawing.Size(125, 18);
            this.CHEAT_ITEM_JUMP.TabIndex = 207;
            this.CHEAT_ITEM_JUMP.Text = "持たせるアイテム";
            // 
            // CHEAT_UNIT_HAVE_ITEM
            // 
            this.CHEAT_UNIT_HAVE_ITEM.Location = new System.Drawing.Point(66, 329);
            this.CHEAT_UNIT_HAVE_ITEM.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_UNIT_HAVE_ITEM.Name = "CHEAT_UNIT_HAVE_ITEM";
            this.CHEAT_UNIT_HAVE_ITEM.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_UNIT_HAVE_ITEM.TabIndex = 3;
            this.CHEAT_UNIT_HAVE_ITEM.Text = "このユニットに以下のアイテムをもたせる";
            this.CHEAT_UNIT_HAVE_ITEM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_UNIT_HAVE_ITEM.UseVisualStyleBackColor = true;
            this.CHEAT_UNIT_HAVE_ITEM.Click += new System.EventHandler(this.CHEAT_UNIT_HAVE_ITEM_Click);
            // 
            // CHEAT_UNIT_HP_1
            // 
            this.CHEAT_UNIT_HP_1.Location = new System.Drawing.Point(66, 274);
            this.CHEAT_UNIT_HP_1.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_UNIT_HP_1.Name = "CHEAT_UNIT_HP_1";
            this.CHEAT_UNIT_HP_1.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_UNIT_HP_1.TabIndex = 2;
            this.CHEAT_UNIT_HP_1.Text = "このユニットのHPを1にする。";
            this.CHEAT_UNIT_HP_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_UNIT_HP_1.UseVisualStyleBackColor = true;
            this.CHEAT_UNIT_HP_1.Click += new System.EventHandler(this.CHEAT_UNIT_HP_1_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(68, 197);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(199, 18);
            this.label11.TabIndex = 204;
            this.label11.Text = "現在、操作しているユニット";
            // 
            // CHEAT_UNIT_GROW
            // 
            this.CHEAT_UNIT_GROW.Location = new System.Drawing.Point(66, 220);
            this.CHEAT_UNIT_GROW.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_UNIT_GROW.Name = "CHEAT_UNIT_GROW";
            this.CHEAT_UNIT_GROW.Size = new System.Drawing.Size(742, 40);
            this.CHEAT_UNIT_GROW.TabIndex = 1;
            this.CHEAT_UNIT_GROW.Text = "このユニットのパラメータをカンストさせる。";
            this.CHEAT_UNIT_GROW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_UNIT_GROW.UseVisualStyleBackColor = true;
            this.CHEAT_UNIT_GROW.Click += new System.EventHandler(this.CHEAT_UNIT_GROW_Click);
            // 
            // CHEAT_SET_FLAG03
            // 
            this.CHEAT_SET_FLAG03.Location = new System.Drawing.Point(61, 58);
            this.CHEAT_SET_FLAG03.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_SET_FLAG03.Name = "CHEAT_SET_FLAG03";
            this.CHEAT_SET_FLAG03.Size = new System.Drawing.Size(742, 53);
            this.CHEAT_SET_FLAG03.TabIndex = 0;
            this.CHEAT_SET_FLAG03.Text = "現在の章をクリアします。(Hotkey: Ctrl + U)";
            this.CHEAT_SET_FLAG03.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CHEAT_SET_FLAG03.UseVisualStyleBackColor = true;
            this.CHEAT_SET_FLAG03.Click += new System.EventHandler(this.CHEAT_SET_FLAG03_Click);
            // 
            // CHEAT_WARP_NODE_NAME
            // 
            this.CHEAT_WARP_NODE_NAME.ErrorMessage = "";
            this.CHEAT_WARP_NODE_NAME.Location = new System.Drawing.Point(1122, 188);
            this.CHEAT_WARP_NODE_NAME.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.CHEAT_WARP_NODE_NAME.Name = "CHEAT_WARP_NODE_NAME";
            this.CHEAT_WARP_NODE_NAME.Placeholder = "";
            this.CHEAT_WARP_NODE_NAME.ReadOnly = true;
            this.CHEAT_WARP_NODE_NAME.Size = new System.Drawing.Size(350, 25);
            this.CHEAT_WARP_NODE_NAME.TabIndex = 329;
            // 
            // systemIconPictureBox5
            // 
            this.systemIconPictureBox5.IconNumber = ((uint)(3u));
            this.systemIconPictureBox5.IconType = FEBuilderGBA.SystemIconPictureBox.IconTypeEnum.Music;
            this.systemIconPictureBox5.Location = new System.Drawing.Point(824, 56);
            this.systemIconPictureBox5.Margin = new System.Windows.Forms.Padding(2);
            this.systemIconPictureBox5.Name = "systemIconPictureBox5";
            this.systemIconPictureBox5.Size = new System.Drawing.Size(56, 56);
            this.systemIconPictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.systemIconPictureBox5.TabIndex = 326;
            this.systemIconPictureBox5.TabStop = false;
            // 
            // CHEAT_WARP_EDTION_NAME
            // 
            this.CHEAT_WARP_EDTION_NAME.ErrorMessage = "";
            this.CHEAT_WARP_EDTION_NAME.Location = new System.Drawing.Point(1122, 155);
            this.CHEAT_WARP_EDTION_NAME.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.CHEAT_WARP_EDTION_NAME.Name = "CHEAT_WARP_EDTION_NAME";
            this.CHEAT_WARP_EDTION_NAME.Placeholder = "";
            this.CHEAT_WARP_EDTION_NAME.ReadOnly = true;
            this.CHEAT_WARP_EDTION_NAME.Size = new System.Drawing.Size(350, 25);
            this.CHEAT_WARP_EDTION_NAME.TabIndex = 324;
            // 
            // CHEAT_WARP_CHPATER_NAME
            // 
            this.CHEAT_WARP_CHPATER_NAME.ErrorMessage = "";
            this.CHEAT_WARP_CHPATER_NAME.Location = new System.Drawing.Point(1122, 122);
            this.CHEAT_WARP_CHPATER_NAME.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.CHEAT_WARP_CHPATER_NAME.Name = "CHEAT_WARP_CHPATER_NAME";
            this.CHEAT_WARP_CHPATER_NAME.Placeholder = "";
            this.CHEAT_WARP_CHPATER_NAME.ReadOnly = true;
            this.CHEAT_WARP_CHPATER_NAME.Size = new System.Drawing.Size(350, 25);
            this.CHEAT_WARP_CHPATER_NAME.TabIndex = 321;
            // 
            // systemIconPictureBox4
            // 
            this.systemIconPictureBox4.IconNumber = ((uint)(2u));
            this.systemIconPictureBox4.Location = new System.Drawing.Point(1, 487);
            this.systemIconPictureBox4.Margin = new System.Windows.Forms.Padding(2);
            this.systemIconPictureBox4.Name = "systemIconPictureBox4";
            this.systemIconPictureBox4.Size = new System.Drawing.Size(56, 56);
            this.systemIconPictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.systemIconPictureBox4.TabIndex = 316;
            this.systemIconPictureBox4.TabStop = false;
            // 
            // CHEAT_UNIT_MEMORY_AND_NAME
            // 
            this.CHEAT_UNIT_MEMORY_AND_NAME.ErrorMessage = "";
            this.CHEAT_UNIT_MEMORY_AND_NAME.Location = new System.Drawing.Point(290, 190);
            this.CHEAT_UNIT_MEMORY_AND_NAME.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.CHEAT_UNIT_MEMORY_AND_NAME.Name = "CHEAT_UNIT_MEMORY_AND_NAME";
            this.CHEAT_UNIT_MEMORY_AND_NAME.Placeholder = "";
            this.CHEAT_UNIT_MEMORY_AND_NAME.ReadOnly = true;
            this.CHEAT_UNIT_MEMORY_AND_NAME.Size = new System.Drawing.Size(513, 25);
            this.CHEAT_UNIT_MEMORY_AND_NAME.TabIndex = 225;
            // 
            // CHEAT_ITEM_NAME
            // 
            this.CHEAT_ITEM_NAME.ErrorMessage = "";
            this.CHEAT_ITEM_NAME.Location = new System.Drawing.Point(294, 378);
            this.CHEAT_ITEM_NAME.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.CHEAT_ITEM_NAME.Name = "CHEAT_ITEM_NAME";
            this.CHEAT_ITEM_NAME.Placeholder = "";
            this.CHEAT_ITEM_NAME.ReadOnly = true;
            this.CHEAT_ITEM_NAME.Size = new System.Drawing.Size(189, 25);
            this.CHEAT_ITEM_NAME.TabIndex = 219;
            // 
            // systemIconPictureBox3
            // 
            this.systemIconPictureBox3.IconNumber = ((uint)(5u));
            this.systemIconPictureBox3.Location = new System.Drawing.Point(830, 758);
            this.systemIconPictureBox3.Margin = new System.Windows.Forms.Padding(2);
            this.systemIconPictureBox3.Name = "systemIconPictureBox3";
            this.systemIconPictureBox3.Size = new System.Drawing.Size(56, 56);
            this.systemIconPictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.systemIconPictureBox3.TabIndex = 228;
            this.systemIconPictureBox3.TabStop = false;
            // 
            // systemIconPictureBox2
            // 
            this.systemIconPictureBox2.IconNumber = ((uint)(8u));
            this.systemIconPictureBox2.Location = new System.Drawing.Point(824, 220);
            this.systemIconPictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.systemIconPictureBox2.Name = "systemIconPictureBox2";
            this.systemIconPictureBox2.Size = new System.Drawing.Size(56, 56);
            this.systemIconPictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.systemIconPictureBox2.TabIndex = 227;
            this.systemIconPictureBox2.TabStop = false;
            // 
            // CHEAT_UNIT_MEMORY_AND_ICON
            // 
            this.CHEAT_UNIT_MEMORY_AND_ICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.CHEAT_UNIT_MEMORY_AND_ICON.Location = new System.Drawing.Point(1, 197);
            this.CHEAT_UNIT_MEMORY_AND_ICON.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_UNIT_MEMORY_AND_ICON.Name = "CHEAT_UNIT_MEMORY_AND_ICON";
            this.CHEAT_UNIT_MEMORY_AND_ICON.Size = new System.Drawing.Size(56, 56);
            this.CHEAT_UNIT_MEMORY_AND_ICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CHEAT_UNIT_MEMORY_AND_ICON.TabIndex = 226;
            this.CHEAT_UNIT_MEMORY_AND_ICON.TabStop = false;
            // 
            // systemIconPictureBox1
            // 
            this.systemIconPictureBox1.Location = new System.Drawing.Point(1, 56);
            this.systemIconPictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.systemIconPictureBox1.Name = "systemIconPictureBox1";
            this.systemIconPictureBox1.Size = new System.Drawing.Size(56, 56);
            this.systemIconPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.systemIconPictureBox1.TabIndex = 224;
            this.systemIconPictureBox1.TabStop = false;
            // 
            // CHEAT_ITEM_ICON
            // 
            this.CHEAT_ITEM_ICON.Interpolation = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            this.CHEAT_ITEM_ICON.Location = new System.Drawing.Point(487, 378);
            this.CHEAT_ITEM_ICON.Margin = new System.Windows.Forms.Padding(2);
            this.CHEAT_ITEM_ICON.Name = "CHEAT_ITEM_ICON";
            this.CHEAT_ITEM_ICON.Size = new System.Drawing.Size(32, 32);
            this.CHEAT_ITEM_ICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CHEAT_ITEM_ICON.TabIndex = 220;
            this.CHEAT_ITEM_ICON.TabStop = false;
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.ErrorMessage = "";
            this.textBoxEx1.Location = new System.Drawing.Point(490, 4);
            this.textBoxEx1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Placeholder = "";
            this.textBoxEx1.ReadOnly = true;
            this.textBoxEx1.Size = new System.Drawing.Size(210, 25);
            this.textBoxEx1.TabIndex = 303;
            // 
            // textBoxEx2
            // 
            this.textBoxEx2.ErrorMessage = "";
            this.textBoxEx2.Location = new System.Drawing.Point(908, 572);
            this.textBoxEx2.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx2.Name = "textBoxEx2";
            this.textBoxEx2.Placeholder = "";
            this.textBoxEx2.ReadOnly = true;
            this.textBoxEx2.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx2.TabIndex = 122;
            // 
            // textBoxEx3
            // 
            this.textBoxEx3.ErrorMessage = "";
            this.textBoxEx3.Location = new System.Drawing.Point(908, 542);
            this.textBoxEx3.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx3.Name = "textBoxEx3";
            this.textBoxEx3.Placeholder = "";
            this.textBoxEx3.ReadOnly = true;
            this.textBoxEx3.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx3.TabIndex = 109;
            // 
            // textBoxEx4
            // 
            this.textBoxEx4.ErrorMessage = "";
            this.textBoxEx4.Location = new System.Drawing.Point(908, 512);
            this.textBoxEx4.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx4.Name = "textBoxEx4";
            this.textBoxEx4.Placeholder = "";
            this.textBoxEx4.ReadOnly = true;
            this.textBoxEx4.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx4.TabIndex = 91;
            // 
            // textBoxEx5
            // 
            this.textBoxEx5.ErrorMessage = "";
            this.textBoxEx5.Location = new System.Drawing.Point(908, 481);
            this.textBoxEx5.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx5.Name = "textBoxEx5";
            this.textBoxEx5.Placeholder = "";
            this.textBoxEx5.ReadOnly = true;
            this.textBoxEx5.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx5.TabIndex = 79;
            // 
            // textBoxEx6
            // 
            this.textBoxEx6.ErrorMessage = "";
            this.textBoxEx6.Location = new System.Drawing.Point(908, 451);
            this.textBoxEx6.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx6.Name = "textBoxEx6";
            this.textBoxEx6.Placeholder = "";
            this.textBoxEx6.ReadOnly = true;
            this.textBoxEx6.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx6.TabIndex = 65;
            // 
            // textBoxEx7
            // 
            this.textBoxEx7.ErrorMessage = "";
            this.textBoxEx7.Location = new System.Drawing.Point(908, 422);
            this.textBoxEx7.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx7.Name = "textBoxEx7";
            this.textBoxEx7.Placeholder = "";
            this.textBoxEx7.ReadOnly = true;
            this.textBoxEx7.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx7.TabIndex = 53;
            // 
            // textBoxEx8
            // 
            this.textBoxEx8.ErrorMessage = "";
            this.textBoxEx8.Location = new System.Drawing.Point(422, 576);
            this.textBoxEx8.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx8.Name = "textBoxEx8";
            this.textBoxEx8.Placeholder = "";
            this.textBoxEx8.ReadOnly = true;
            this.textBoxEx8.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx8.TabIndex = 115;
            // 
            // textBoxEx9
            // 
            this.textBoxEx9.ErrorMessage = "";
            this.textBoxEx9.Location = new System.Drawing.Point(422, 546);
            this.textBoxEx9.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx9.Name = "textBoxEx9";
            this.textBoxEx9.Placeholder = "";
            this.textBoxEx9.ReadOnly = true;
            this.textBoxEx9.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx9.TabIndex = 103;
            // 
            // textBoxEx10
            // 
            this.textBoxEx10.ErrorMessage = "";
            this.textBoxEx10.Location = new System.Drawing.Point(422, 512);
            this.textBoxEx10.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx10.Name = "textBoxEx10";
            this.textBoxEx10.Placeholder = "";
            this.textBoxEx10.ReadOnly = true;
            this.textBoxEx10.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx10.TabIndex = 85;
            // 
            // textBoxEx11
            // 
            this.textBoxEx11.ErrorMessage = "";
            this.textBoxEx11.Location = new System.Drawing.Point(422, 480);
            this.textBoxEx11.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx11.Name = "textBoxEx11";
            this.textBoxEx11.Placeholder = "";
            this.textBoxEx11.ReadOnly = true;
            this.textBoxEx11.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx11.TabIndex = 72;
            // 
            // textBoxEx12
            // 
            this.textBoxEx12.ErrorMessage = "";
            this.textBoxEx12.Location = new System.Drawing.Point(422, 450);
            this.textBoxEx12.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx12.Name = "textBoxEx12";
            this.textBoxEx12.Placeholder = "";
            this.textBoxEx12.ReadOnly = true;
            this.textBoxEx12.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx12.TabIndex = 59;
            // 
            // textBoxEx13
            // 
            this.textBoxEx13.ErrorMessage = "";
            this.textBoxEx13.Location = new System.Drawing.Point(422, 422);
            this.textBoxEx13.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx13.Name = "textBoxEx13";
            this.textBoxEx13.Placeholder = "";
            this.textBoxEx13.ReadOnly = true;
            this.textBoxEx13.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx13.TabIndex = 47;
            // 
            // textBoxEx14
            // 
            this.textBoxEx14.ErrorMessage = "";
            this.textBoxEx14.Location = new System.Drawing.Point(908, 392);
            this.textBoxEx14.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx14.Name = "textBoxEx14";
            this.textBoxEx14.Placeholder = "";
            this.textBoxEx14.ReadOnly = true;
            this.textBoxEx14.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx14.TabIndex = 40;
            // 
            // textBoxEx15
            // 
            this.textBoxEx15.ErrorMessage = "";
            this.textBoxEx15.Location = new System.Drawing.Point(422, 392);
            this.textBoxEx15.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx15.Name = "textBoxEx15";
            this.textBoxEx15.Placeholder = "";
            this.textBoxEx15.ReadOnly = true;
            this.textBoxEx15.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx15.TabIndex = 34;
            // 
            // textBoxEx19
            // 
            this.textBoxEx19.ErrorMessage = "";
            this.textBoxEx19.Location = new System.Drawing.Point(908, 362);
            this.textBoxEx19.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx19.Name = "textBoxEx19";
            this.textBoxEx19.Placeholder = "";
            this.textBoxEx19.ReadOnly = true;
            this.textBoxEx19.Size = new System.Drawing.Size(101, 25);
            this.textBoxEx19.TabIndex = 28;
            // 
            // textBoxEx20
            // 
            this.textBoxEx20.ErrorMessage = "";
            this.textBoxEx20.Location = new System.Drawing.Point(422, 362);
            this.textBoxEx20.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx20.Name = "textBoxEx20";
            this.textBoxEx20.Placeholder = "";
            this.textBoxEx20.ReadOnly = true;
            this.textBoxEx20.Size = new System.Drawing.Size(102, 25);
            this.textBoxEx20.TabIndex = 22;
            // 
            // textBoxEx21
            // 
            this.textBoxEx21.ErrorMessage = "";
            this.textBoxEx21.Location = new System.Drawing.Point(338, 35);
            this.textBoxEx21.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx21.Name = "textBoxEx21";
            this.textBoxEx21.Placeholder = "";
            this.textBoxEx21.ReadOnly = true;
            this.textBoxEx21.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx21.TabIndex = 300;
            // 
            // textBoxEx22
            // 
            this.textBoxEx22.ErrorMessage = "";
            this.textBoxEx22.Location = new System.Drawing.Point(338, 120);
            this.textBoxEx22.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx22.Name = "textBoxEx22";
            this.textBoxEx22.Placeholder = "";
            this.textBoxEx22.ReadOnly = true;
            this.textBoxEx22.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx22.TabIndex = 295;
            // 
            // textBoxEx23
            // 
            this.textBoxEx23.ErrorMessage = "";
            this.textBoxEx23.Location = new System.Drawing.Point(329, 830);
            this.textBoxEx23.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx23.Name = "textBoxEx23";
            this.textBoxEx23.Placeholder = "";
            this.textBoxEx23.ReadOnly = true;
            this.textBoxEx23.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx23.TabIndex = 289;
            // 
            // textBoxEx24
            // 
            this.textBoxEx24.ErrorMessage = "";
            this.textBoxEx24.Location = new System.Drawing.Point(329, 802);
            this.textBoxEx24.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx24.Name = "textBoxEx24";
            this.textBoxEx24.Placeholder = "";
            this.textBoxEx24.ReadOnly = true;
            this.textBoxEx24.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx24.TabIndex = 286;
            // 
            // textBoxEx25
            // 
            this.textBoxEx25.ErrorMessage = "";
            this.textBoxEx25.Location = new System.Drawing.Point(329, 773);
            this.textBoxEx25.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx25.Name = "textBoxEx25";
            this.textBoxEx25.Placeholder = "";
            this.textBoxEx25.ReadOnly = true;
            this.textBoxEx25.Size = new System.Drawing.Size(291, 25);
            this.textBoxEx25.TabIndex = 283;
            // 
            // textBoxEx26
            // 
            this.textBoxEx26.ErrorMessage = "";
            this.textBoxEx26.Location = new System.Drawing.Point(338, 234);
            this.textBoxEx26.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx26.Name = "textBoxEx26";
            this.textBoxEx26.Placeholder = "";
            this.textBoxEx26.ReadOnly = true;
            this.textBoxEx26.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx26.TabIndex = 223;
            // 
            // textBoxEx27
            // 
            this.textBoxEx27.ErrorMessage = "";
            this.textBoxEx27.Location = new System.Drawing.Point(338, 206);
            this.textBoxEx27.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx27.Name = "textBoxEx27";
            this.textBoxEx27.Placeholder = "";
            this.textBoxEx27.ReadOnly = true;
            this.textBoxEx27.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx27.TabIndex = 220;
            // 
            // textBoxEx28
            // 
            this.textBoxEx28.ErrorMessage = "";
            this.textBoxEx28.Location = new System.Drawing.Point(338, 176);
            this.textBoxEx28.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx28.Name = "textBoxEx28";
            this.textBoxEx28.Placeholder = "";
            this.textBoxEx28.ReadOnly = true;
            this.textBoxEx28.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx28.TabIndex = 217;
            // 
            // textBoxEx29
            // 
            this.textBoxEx29.ErrorMessage = "";
            this.textBoxEx29.Location = new System.Drawing.Point(338, 149);
            this.textBoxEx29.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx29.Name = "textBoxEx29";
            this.textBoxEx29.Placeholder = "";
            this.textBoxEx29.ReadOnly = true;
            this.textBoxEx29.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx29.TabIndex = 214;
            // 
            // textBoxEx30
            // 
            this.textBoxEx30.ErrorMessage = "";
            this.textBoxEx30.Location = new System.Drawing.Point(338, 91);
            this.textBoxEx30.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx30.Name = "textBoxEx30";
            this.textBoxEx30.Placeholder = "";
            this.textBoxEx30.ReadOnly = true;
            this.textBoxEx30.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx30.TabIndex = 211;
            // 
            // textBoxEx31
            // 
            this.textBoxEx31.ErrorMessage = "";
            this.textBoxEx31.Location = new System.Drawing.Point(338, 62);
            this.textBoxEx31.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx31.Name = "textBoxEx31";
            this.textBoxEx31.Placeholder = "";
            this.textBoxEx31.ReadOnly = true;
            this.textBoxEx31.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx31.TabIndex = 208;
            // 
            // textBoxEx32
            // 
            this.textBoxEx32.ErrorMessage = "";
            this.textBoxEx32.Location = new System.Drawing.Point(338, 6);
            this.textBoxEx32.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEx32.Name = "textBoxEx32";
            this.textBoxEx32.Placeholder = "";
            this.textBoxEx32.ReadOnly = true;
            this.textBoxEx32.Size = new System.Drawing.Size(671, 25);
            this.textBoxEx32.TabIndex = 202;
            // 
            // EmulatorMemoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1680, 929);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MainTabControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EmulatorMemoryForm";
            this.Text = "エミュレータへ接続";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EmulatorMemoryForm_FormClosed);
            this.Load += new System.EventHandler(this.EmulatorMemoryForm_Load);
            this.Shown += new System.EventHandler(this.EmulatorMemoryForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EmulatorMemoryForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.EventPage.ResumeLayout(false);
            this.EventPage.PerformLayout();
            this.EventHistoryPage.ResumeLayout(false);
            this.ProcPage.ResumeLayout(false);
            this.Proc_ControlPanel.ResumeLayout(false);
            this.Proc_ControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_Address)).EndInit();
            this.ControlPanelCommand.ResumeLayout(false);
            this.ControlPanelCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B107)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B51)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B59)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B67)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B99)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B75)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B91)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B83)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B106)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B50)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B58)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B66)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B98)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B74)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B90)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B82)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B105)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B49)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B57)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B65)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B97)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B73)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B89)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B81)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B47)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B55)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B63)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B103)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B71)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B95)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B79)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B87)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B46)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B54)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B62)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B102)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B70)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B94)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B78)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B86)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B45)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B53)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B61)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B101)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B69)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B93)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B77)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B85)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B41)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B43)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B42)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B104)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B48)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B52)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B56)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B60)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B40)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B64)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B39)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B100)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B38)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B68)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_W36)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B96)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B72)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B92)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B76)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B88)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B80)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_B84)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PROCS_P0)).EndInit();
            this.EtcPage.ResumeLayout(false);
            this.EtcPage.PerformLayout();
            this.tabControlEtc.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ETC_UNIT_MEMORY_AND_ICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.N_B14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BGM)).EndInit();
            this.tabPageTrapData.ResumeLayout(false);
            this.tabPageTrapData.PerformLayout();
            this.tabPagePalette.ResumeLayout(false);
            this.tabPagePalette.PerformLayout();
            this.tabPageClearTurns.ResumeLayout(false);
            this.tabPageClearTurns.PerformLayout();
            this.tabPageBWL.ResumeLayout(false);
            this.tabPageBWL.PerformLayout();
            this.tabPageChapterData.ResumeLayout(false);
            this.tabPageChapterData.PerformLayout();
            this.tabPageSupplyData.ResumeLayout(false);
            this.tabPageSupplyData.PerformLayout();
            this.tabPageActionData.ResumeLayout(false);
            this.tabPageActionData.PerformLayout();
            this.tabPageArenaData.ResumeLayout(false);
            this.tabPageArenaData.PerformLayout();
            this.tabPageBattleActor.ResumeLayout(false);
            this.tabPageBattleActor.PerformLayout();
            this.tabPageBattleTarget.ResumeLayout(false);
            this.tabPageBattleTarget.PerformLayout();
            this.tabPageAIData.ResumeLayout(false);
            this.tabPageAIData.PerformLayout();
            this.tabPageBattleRound.ResumeLayout(false);
            this.tabPageBattleRound.PerformLayout();
            this.tabPageBattleSome.ResumeLayout(false);
            this.tabPageBattleSome.PerformLayout();
            this.tabPageWorldmap.ResumeLayout(false);
            this.tabPageWorldmap.PerformLayout();
            this.tabPageDungeon.ResumeLayout(false);
            this.tabPageDungeon.PerformLayout();
            this.Party_ControlPanel.ResumeLayout(false);
            this.Party_ControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_Address)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_PORTRAIT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_38_ITEMICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_36_ITEMICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_34_ITEMICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_32_ITEMICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_L_30_ITEMICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B71)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B70)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B69)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B68)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B67)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B66)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B65)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B64)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B63)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B62)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B61)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B60)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B59)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B58)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B57)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B56)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B55)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B54)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B53)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B52)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B51)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B50)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B49)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B48)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B47)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B46)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B45)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B43)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B42)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B41)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B40)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B39)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B38)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B37)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B36)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B35)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B34)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B30)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B29)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown59)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown61)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown64)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_D12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_B8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_P4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PARTY_P0)).EndInit();
            this.CheatPage.ResumeLayout(false);
            this.CheatPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_TURN_VALUE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WARP_NODE_VALUE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WARP_EDTION_VALUE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WARP_CHPATER_VALUE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_WEATHER_VALUE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_FOG_VALUE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_ITEM_COUNT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_ITEM_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_MONEY_VALUE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_UNIT_MEMORY_AND_ICON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemIconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHEAT_ITEM_ICON)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox AutoUpdateCheckBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label ERROR_EMU_CONNECT;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage ProcPage;
        private ListBoxEx ProcsListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel Proc_ControlPanel;
        private System.Windows.Forms.NumericUpDown PROCS_Address;
        private TextBoxEx PROCS_SelectAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label PROCS_AddressLabel;
        private System.Windows.Forms.Panel ControlPanelCommand;
        private TextBoxEx PROCS_CURSOL_CODE;
        private System.Windows.Forms.NumericUpDown PROCS_B43;
        private System.Windows.Forms.NumericUpDown PROCS_B42;
        private System.Windows.Forms.NumericUpDown PROCS_B41;
        private System.Windows.Forms.Label label3;
        private TextBoxEx PROCS_L_16_TEXT;
        private System.Windows.Forms.NumericUpDown PROCS_P16;
        private System.Windows.Forms.Label PROCS_J_16_TEXT;
        private System.Windows.Forms.NumericUpDown PROCS_B104;
        private System.Windows.Forms.Label PROCS_J_104;
        private TextBoxEx textBoxEx16;
        private System.Windows.Forms.NumericUpDown numericUpDown17;
        private System.Windows.Forms.Label label25;
        private TextBoxEx textBoxEx17;
        private System.Windows.Forms.NumericUpDown numericUpDown18;
        private System.Windows.Forms.Label label26;
        private TextBoxEx textBoxEx18;
        private System.Windows.Forms.NumericUpDown numericUpDown19;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.NumericUpDown PROCS_B100;
        private System.Windows.Forms.Label PROCS_J_100;
        private System.Windows.Forms.NumericUpDown PROCS_B96;
        private System.Windows.Forms.Label PROCS_J_96;
        private System.Windows.Forms.NumericUpDown PROCS_B92;
        private System.Windows.Forms.Label PROCS_J_92;
        private System.Windows.Forms.NumericUpDown PROCS_B88;
        private System.Windows.Forms.Label PROCS_J_88;
        private System.Windows.Forms.NumericUpDown PROCS_B84;
        private System.Windows.Forms.Label PROCS_J_84;
        private System.Windows.Forms.NumericUpDown PROCS_B80;
        private System.Windows.Forms.Label PROCS_J_80;
        private System.Windows.Forms.NumericUpDown PROCS_B76;
        private System.Windows.Forms.Label PROCS_J_76;
        private System.Windows.Forms.NumericUpDown PROCS_B72;
        private System.Windows.Forms.Label PROCS_J_72;
        private System.Windows.Forms.NumericUpDown PROCS_B68;
        private System.Windows.Forms.Label PROCS_J_68;
        private System.Windows.Forms.NumericUpDown PROCS_B64;
        private System.Windows.Forms.Label PROCS_J_64;
        private System.Windows.Forms.NumericUpDown PROCS_B60;
        private System.Windows.Forms.Label PROCS_J_60;
        private System.Windows.Forms.NumericUpDown PROCS_B56;
        private System.Windows.Forms.Label PROCS_J_56;
        private System.Windows.Forms.NumericUpDown PROCS_B52;
        private System.Windows.Forms.Label PROCS_J_52;
        private System.Windows.Forms.NumericUpDown PROCS_B48;
        private System.Windows.Forms.Label PROCS_J_48;
        private System.Windows.Forms.NumericUpDown PROCS_B44;
        private System.Windows.Forms.Label PROCS_J_44;
        private System.Windows.Forms.NumericUpDown PROCS_B40;
        private System.Windows.Forms.Label PROCS_J_40;
        private System.Windows.Forms.NumericUpDown PROCS_B39;
        private System.Windows.Forms.Label PROCS_J_39;
        private System.Windows.Forms.NumericUpDown PROCS_B38;
        private System.Windows.Forms.Label PROCS_J_38;
        private System.Windows.Forms.NumericUpDown PROCS_W36;
        private System.Windows.Forms.Label PROCS_J_36;
        private TextBoxEx PROCS_L_32_RAMPROCS;
        private System.Windows.Forms.NumericUpDown PROCS_P32;
        private System.Windows.Forms.Label PROCS_J_32_RAMPROCS;
        private TextBoxEx PROCS_L_28_RAMPROCS;
        private System.Windows.Forms.NumericUpDown PROCS_P28;
        private System.Windows.Forms.Label PROCS_J_28_RAMPROCS;
        private TextBoxEx PROCS_L_24_RAMPROCS;
        private System.Windows.Forms.NumericUpDown PROCS_P24;
        private System.Windows.Forms.Label PROCS_J_24_RAMPROCS;
        private TextBoxEx PROCS_L_20_RAMPROCS;
        private System.Windows.Forms.NumericUpDown PROCS_P20;
        private System.Windows.Forms.Label PROCS_J_20_RAMPROCS;
        private TextBoxEx PROCS_L_12_ASM;
        private System.Windows.Forms.NumericUpDown PROCS_P12;
        private System.Windows.Forms.Label PROCS_J_12_ASM;
        private TextBoxEx PROCS_L_8_ASM;
        private System.Windows.Forms.NumericUpDown PROCS_P8;
        private System.Windows.Forms.Label PROCS_J_8_ASM;
        private System.Windows.Forms.NumericUpDown PROCS_P4;
        private System.Windows.Forms.Label PROCS_JUMP_CURSOL_CODE;
        private TextBoxEx PROCS_NAME;
        private System.Windows.Forms.NumericUpDown PROCS_P0;
        private System.Windows.Forms.Label PROCS_J_0_PROCS;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TabPage EventHistoryPage;
        private ListBoxEx EventHistoryListBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private TextBoxEx PROCS_L_104_DWORD;
        private TextBoxEx PROCS_L_96_DWORD;
        private TextBoxEx PROCS_L_88_DWORD;
        private TextBoxEx PROCS_L_80_DWORD;
        private TextBoxEx PROCS_L_72_DWORD;
        private TextBoxEx PROCS_L_64_DWORD;
        private TextBoxEx PROCS_L_100_DWORD;
        private TextBoxEx PROCS_L_92_DWORD;
        private TextBoxEx PROCS_L_84_DWORD;
        private TextBoxEx PROCS_L_76_DWORD;
        private TextBoxEx PROCS_L_68_DWORD;
        private TextBoxEx PROCS_L_60_DWORD;
        private TextBoxEx PROCS_L_56_DWORD;
        private TextBoxEx PROCS_L_52_DWORD;
        private TextBoxEx PROCS_L_48_DWORD;
        private TextBoxEx PROCS_L_44_DWORD;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private TextBoxEx N_SelectAddress;
        private System.Windows.Forms.TabPage CheatPage;
        private System.Windows.Forms.Button CHEAT_SET_FLAG03;
        private System.Windows.Forms.Button CHEAT_UNIT_GROW;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button CHEAT_UNIT_HP_1;
        private System.Windows.Forms.Button CHEAT_UNIT_HAVE_ITEM;
        private System.Windows.Forms.Label CHEAT_ITEM_JUMP;
        private System.Windows.Forms.Button CHEAT_MONEY;
        private System.Windows.Forms.Button CHEAT_FOG;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button Dump02Button;
        private System.Windows.Forms.Button Dump03Button;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown CHEAT_MONEY_VALUE;
        private System.Windows.Forms.NumericUpDown CHEAT_ITEM_ID;
        private TextBoxEx CHEAT_ITEM_NAME;
        private InterpolatedPictureBox CHEAT_ITEM_ICON;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown CHEAT_ITEM_COUNT;
        private System.Windows.Forms.NumericUpDown CHEAT_FOG_VALUE;
        private SystemIconPictureBox systemIconPictureBox1;
        private TextBoxEx CHEAT_UNIT_MEMORY_AND_NAME;
        private InterpolatedPictureBox CHEAT_UNIT_MEMORY_AND_ICON;
        private SystemIconPictureBox systemIconPictureBox2;
        private SystemIconPictureBox systemIconPictureBox3;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown PROCS_B47;
        private System.Windows.Forms.NumericUpDown PROCS_B55;
        private System.Windows.Forms.NumericUpDown PROCS_B63;
        private System.Windows.Forms.NumericUpDown PROCS_B103;
        private System.Windows.Forms.NumericUpDown PROCS_B71;
        private System.Windows.Forms.NumericUpDown PROCS_B95;
        private System.Windows.Forms.NumericUpDown PROCS_B79;
        private System.Windows.Forms.NumericUpDown PROCS_B87;
        private System.Windows.Forms.NumericUpDown PROCS_B46;
        private System.Windows.Forms.NumericUpDown PROCS_B54;
        private System.Windows.Forms.NumericUpDown PROCS_B62;
        private System.Windows.Forms.NumericUpDown PROCS_B102;
        private System.Windows.Forms.NumericUpDown PROCS_B70;
        private System.Windows.Forms.NumericUpDown PROCS_B94;
        private System.Windows.Forms.NumericUpDown PROCS_B78;
        private System.Windows.Forms.NumericUpDown PROCS_B86;
        private System.Windows.Forms.NumericUpDown PROCS_B45;
        private System.Windows.Forms.NumericUpDown PROCS_B53;
        private System.Windows.Forms.NumericUpDown PROCS_B61;
        private System.Windows.Forms.NumericUpDown PROCS_B101;
        private System.Windows.Forms.NumericUpDown PROCS_B69;
        private System.Windows.Forms.NumericUpDown PROCS_B93;
        private System.Windows.Forms.NumericUpDown PROCS_B77;
        private System.Windows.Forms.NumericUpDown PROCS_B85;
        private System.Windows.Forms.NumericUpDown PROCS_B107;
        private System.Windows.Forms.NumericUpDown PROCS_B51;
        private System.Windows.Forms.NumericUpDown PROCS_B59;
        private System.Windows.Forms.NumericUpDown PROCS_B67;
        private System.Windows.Forms.NumericUpDown PROCS_B99;
        private System.Windows.Forms.NumericUpDown PROCS_B75;
        private System.Windows.Forms.NumericUpDown PROCS_B91;
        private System.Windows.Forms.NumericUpDown PROCS_B83;
        private System.Windows.Forms.NumericUpDown PROCS_B106;
        private System.Windows.Forms.NumericUpDown PROCS_B50;
        private System.Windows.Forms.NumericUpDown PROCS_B58;
        private System.Windows.Forms.NumericUpDown PROCS_B66;
        private System.Windows.Forms.NumericUpDown PROCS_B98;
        private System.Windows.Forms.NumericUpDown PROCS_B74;
        private System.Windows.Forms.NumericUpDown PROCS_B90;
        private System.Windows.Forms.NumericUpDown PROCS_B82;
        private System.Windows.Forms.NumericUpDown PROCS_B105;
        private System.Windows.Forms.NumericUpDown PROCS_B49;
        private System.Windows.Forms.NumericUpDown PROCS_B57;
        private System.Windows.Forms.NumericUpDown PROCS_B65;
        private System.Windows.Forms.NumericUpDown PROCS_B97;
        private System.Windows.Forms.NumericUpDown PROCS_B73;
        private System.Windows.Forms.NumericUpDown PROCS_B89;
        private System.Windows.Forms.NumericUpDown PROCS_B81;
        private System.Windows.Forms.TabPage EventPage;
        private ListBoxEx RunningEventListBox;
        private System.Windows.Forms.Label RunningEventListBoxLabel;
        private ListBoxEx FlagListBox;
        private System.Windows.Forms.Label N1_LabelFilter;
        private ListBoxEx MemorySlotListBox;
        private System.Windows.Forms.Label MemorySlotLabel;
        private RichTextBoxEx CurrentTextBox;
        private TextBoxEx textBoxEx1;
        private TextBoxEx textBoxEx2;
        private TextBoxEx textBoxEx3;
        private TextBoxEx textBoxEx4;
        private TextBoxEx textBoxEx5;
        private TextBoxEx textBoxEx6;
        private TextBoxEx textBoxEx7;
        private TextBoxEx textBoxEx8;
        private TextBoxEx textBoxEx9;
        private TextBoxEx textBoxEx10;
        private TextBoxEx textBoxEx11;
        private TextBoxEx textBoxEx12;
        private TextBoxEx textBoxEx13;
        private TextBoxEx textBoxEx14;
        private TextBoxEx textBoxEx15;
        private TextBoxEx textBoxEx19;
        private TextBoxEx textBoxEx20;
        private TextBoxEx textBoxEx21;
        private TextBoxEx textBoxEx22;
        private TextBoxEx textBoxEx23;
        private TextBoxEx textBoxEx24;
        private TextBoxEx textBoxEx25;
        private TextBoxEx textBoxEx26;
        private TextBoxEx textBoxEx27;
        private TextBoxEx textBoxEx28;
        private TextBoxEx textBoxEx29;
        private TextBoxEx textBoxEx30;
        private TextBoxEx textBoxEx31;
        private TextBoxEx textBoxEx32;
        private System.Windows.Forms.TabPage EtcPage;
        private System.Windows.Forms.Label label56;
        private ListBoxEx PartyListBox;
        private System.Windows.Forms.Panel Party_ControlPanel;
        private System.Windows.Forms.NumericUpDown PARTY_Address;
        private TextBoxEx PARTY_SelectAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private TextBoxEx PARTY_ROMCLASSPOINTER;
        private System.Windows.Forms.NumericUpDown PARTY_B10;
        private System.Windows.Forms.Label label21;
        private TextBoxEx textBoxEx52;
        private System.Windows.Forms.NumericUpDown numericUpDown59;
        private System.Windows.Forms.Label label28;
        private TextBoxEx textBoxEx53;
        private System.Windows.Forms.NumericUpDown numericUpDown61;
        private System.Windows.Forms.Label label30;
        private TextBoxEx textBoxEx54;
        private System.Windows.Forms.NumericUpDown numericUpDown64;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown PARTY_B19;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.NumericUpDown PARTY_B20;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.NumericUpDown PARTY_B18;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.NumericUpDown PARTY_B17;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.NumericUpDown PARTY_B16;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.NumericUpDown PARTY_D12;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.NumericUpDown PARTY_B11;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown PARTY_B9;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.NumericUpDown PARTY_B8;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.NumericUpDown PARTY_P4;
        private System.Windows.Forms.Label label54;
        private TextBoxEx PARTY_ROMUNITPOINTER;
        private System.Windows.Forms.NumericUpDown PARTY_P0;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Button button1;
        private TextBoxEx PARTY_RAMUNITSTATE;
        private System.Windows.Forms.NumericUpDown PARTY_B21;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.NumericUpDown PARTY_B22;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.NumericUpDown PARTY_B24;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.NumericUpDown PARTY_B23;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.NumericUpDown PARTY_B26;
        private System.Windows.Forms.Label PARTY_J_26;
        private System.Windows.Forms.NumericUpDown PARTY_B25;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.NumericUpDown PARTY_B27;
        private System.Windows.Forms.Label label64;
        private TextBoxEx PARTY_RAMUNITAID;
        private System.Windows.Forms.NumericUpDown PARTY_B28;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.NumericUpDown PARTY_B29;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown PARTY_B31;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown PARTY_B30;
        private TextBoxEx PARTY_L_32_ITEM;
        private System.Windows.Forms.NumericUpDown PARTY_B33;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown PARTY_B32;
        private System.Windows.Forms.Label label23;
        private TextBoxEx PARTY_L_30_ITEM;
        private TextBoxEx PARTY_L_38_ITEM;
        private System.Windows.Forms.NumericUpDown PARTY_B39;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.NumericUpDown PARTY_B38;
        private System.Windows.Forms.Label label37;
        private TextBoxEx PARTY_L_36_ITEM;
        private System.Windows.Forms.NumericUpDown PARTY_B37;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.NumericUpDown PARTY_B36;
        private System.Windows.Forms.Label label29;
        private TextBoxEx PARTY_L_34_ITEM;
        private System.Windows.Forms.NumericUpDown PARTY_B35;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.NumericUpDown PARTY_B34;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.NumericUpDown PARTY_B47;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.NumericUpDown PARTY_B46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.NumericUpDown PARTY_B45;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.NumericUpDown PARTY_B44;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.NumericUpDown PARTY_B43;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.NumericUpDown PARTY_B42;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.NumericUpDown PARTY_B41;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.NumericUpDown PARTY_B40;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.NumericUpDown PARTY_B49;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.NumericUpDown PARTY_B48;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.NumericUpDown PARTY_B57;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.NumericUpDown PARTY_B56;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.NumericUpDown PARTY_B55;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.NumericUpDown PARTY_B54;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.NumericUpDown PARTY_B53;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.NumericUpDown PARTY_B52;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.NumericUpDown PARTY_B51;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.NumericUpDown PARTY_B50;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.NumericUpDown PARTY_B63;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.NumericUpDown PARTY_B62;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.NumericUpDown PARTY_B61;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.NumericUpDown PARTY_B60;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.NumericUpDown PARTY_B59;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.NumericUpDown PARTY_B58;
        private System.Windows.Forms.Label PARTY_J_58;
        private System.Windows.Forms.NumericUpDown PARTY_B69;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.NumericUpDown PARTY_B68;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.NumericUpDown PARTY_B67;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.NumericUpDown PARTY_B66;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.NumericUpDown PARTY_B65;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.NumericUpDown PARTY_B64;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.NumericUpDown PARTY_B71;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.NumericUpDown PARTY_B70;
        private System.Windows.Forms.Label label90;
        private InterpolatedPictureBox PARTY_L_38_ITEMICON;
        private InterpolatedPictureBox PARTY_L_36_ITEMICON;
        private InterpolatedPictureBox PARTY_L_34_ITEMICON;
        private InterpolatedPictureBox PARTY_L_32_ITEMICON;
        private InterpolatedPictureBox PARTY_L_30_ITEMICON;
        private System.Windows.Forms.Button Party_CloseButton;
        private ComboBoxEx PartyCombo;
        private InterpolatedPictureBox PARTY_PORTRAIT;
        private TextBoxEx PARTY_AI2_TEXT;
        private TextBoxEx PARTY_AI1_TEXT;
        private System.Windows.Forms.Button SpeechButton;
        private System.Windows.Forms.Button SubtileButton;
        private System.Windows.Forms.Button CHEAT_WEATHER;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.NumericUpDown CHEAT_WEATHER_VALUE;
        private System.Windows.Forms.ComboBox CHEAT_WEATHER_COMBO;
        private System.Windows.Forms.Button CHEAT_ALL_PLAYER_UNIT_GROW;
        private System.Windows.Forms.Button CHEAT_ALL_UNIT_GROW;
        private SystemIconPictureBox systemIconPictureBox4;
        private System.Windows.Forms.Button CHEAT_ALL_ENEMY_UNIT_HP_1;
        private TextBoxEx X_ETC_Edition_Text;
        private TextBoxEx X_ETC_Diffculty_Text;
        private System.Windows.Forms.Label label92;
        private ListBoxEx PaletteList;
        private System.Windows.Forms.Button PaletteSearchButton;
        private TextBoxEx SelectPalette;
        private TextBoxEx PaletteAddress;
        private System.Windows.Forms.Button CHEAT_WARP;
        private System.Windows.Forms.NumericUpDown CHEAT_WARP_CHPATER_VALUE;
        private System.Windows.Forms.Label CHEAT_WARP_CHPATER_JUMP;
        private TextBoxEx CHEAT_WARP_CHPATER_NAME;
        private System.Windows.Forms.NumericUpDown CHEAT_WARP_EDTION_VALUE;
        private System.Windows.Forms.Label CHEAT_WARP_EDTION_JUMP;
        private TextBoxEx CHEAT_WARP_EDTION_NAME;
        private System.Windows.Forms.Button CHEAT_ALL_ENEMY_DO_NOT_MOVE;
        private SystemIconPictureBox systemIconPictureBox5;
        private System.Windows.Forms.NumericUpDown CHEAT_WARP_NODE_VALUE;
        private System.Windows.Forms.Label CHEAT_WARP_NODE_JUMP;
        private TextBoxEx CHEAT_WARP_NODE_NAME;
        private System.Windows.Forms.Label label62;
        private ListBoxEx TrapList;
        private TextBoxEx TrapAddress;
        private System.Windows.Forms.TabControl tabControlEtc;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageTrapData;
        private System.Windows.Forms.TabPage tabPagePalette;
        private TextBoxEx SoundAddress;
        private ListBoxEx SoundList;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tabPageClearTurns;
        private TextBoxEx ClearTurnAddress;
        private ListBoxEx ClearTurnList;
        private System.Windows.Forms.Label label82;
        private TextBoxEx X_ETC_WorldmapNode_Text;
        private System.Windows.Forms.Label N_J_14_MAP;
        private TextBoxEx BGMName;
        private System.Windows.Forms.NumericUpDown N_B14;
        private System.Windows.Forms.NumericUpDown BGM;
        private System.Windows.Forms.Label J_BGM;
        private TextBoxEx N_L_14_MAP;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Label PartyCount;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.NumericUpDown CHEAT_TURN_VALUE;
        private System.Windows.Forms.Button CHEAT_TURN;
        private System.Windows.Forms.TabPage tabPageBWL;
        private System.Windows.Forms.Label label95;
        private ListBoxEx BWLList;
        private System.Windows.Forms.TabPage tabPageChapterData;
        private ListBoxEx ChapterDataList;
        private System.Windows.Forms.Label label96;
        private TextBoxEx BWLAddress;
        private TextBoxEx ChapterDataAddress;
        private System.Windows.Forms.TabPage tabPageBattleActor;
        private System.Windows.Forms.TabPage tabPageBattleTarget;
        private TextBoxEx BattleActorAddress;
        private ListBoxEx BattleActorList;
        private System.Windows.Forms.Label label97;
        private TextBoxEx BattleTargetAddress;
        private ListBoxEx BattleTargetList;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.TabPage tabPageWorldmap;
        private TextBoxEx WorldmapAddress;
        private ListBoxEx WorldmapList;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.TabPage tabPageArenaData;
        private TextBoxEx ArenaDataAddress;
        private ListBoxEx ArenaDataList;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.TabPage tabPageAIData;
        private TextBoxEx AIDataAddress;
        private ListBoxEx AIDataList;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.TabPage tabPageSupplyData;
        private System.Windows.Forms.TabPage tabPageActionData;
        private TextBoxEx SupplyDataAddress;
        private ListBoxEx SupplyDataList;
        private System.Windows.Forms.Label label102;
        private TextBoxEx ActionDataAddress;
        private ListBoxEx ActionDataList;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.TabPage tabPageDungeon;
        private TextBoxEx DungeonDataAddress;
        private ListBoxEx DungeonDataList;
        private System.Windows.Forms.Label label104;
        private System.Windows.Forms.TabPage tabPageBattleSome;
        private TextBoxEx BattleSomeDataAddress;
        private ListBoxEx BattleSomeDataList;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.TabPage tabPageBattleRound;
        private TextBoxEx BattleRoundDataAddress;
        private ListBoxEx BattleRoundDataList;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.Label J_ACTIVEUNIT;
        private TextBoxEx ETC_UNIT_MEMORY_AND_NAME;
        private InterpolatedPictureBox ETC_UNIT_MEMORY_AND_ICON;
    }
}